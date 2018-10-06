using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Xml;


namespace SmartHome
{
    class VeraController
    {
        private const uint m_uPort = 3480;

        private HttpClient m_Client = new HttpClient();
        private XmlDocument m_UserDataXml = null;

        public delegate void DebugPrintHandler(string sMsg);

        public event DebugPrintHandler DebugPrint;

        protected void OnLog(string sMsg)
        {
            DebugPrint?.Invoke(sMsg);
        }

        public Dictionary<int, string> Rooms = new Dictionary<int, string>();
        public List<Device> Devices = new List<Device>();
        public string Address { get; private set; }
        public VeraController(string sAddress)
        {
            Address = sAddress;
        }

        public async Task<string> SendCommandAsync(string sCmd)
        {
            string sRequest = String.Format("http://{0}:{1}/{2}", Address, m_uPort, sCmd);
            string sResponseString = await m_Client.GetStringAsync(sRequest);

            OnLog(String.Format("Request: {0}\nResponse: {1}", sRequest, sResponseString));

            return sResponseString;
        }

        public async Task ConnectAsync()
        {
            string sResponseString = await SendCommandAsync("data_request?id=user_data&output_format=xml");

            m_UserDataXml = new XmlDocument();
            m_UserDataXml.LoadXml(sResponseString);

            PopulateRooms();
            PopulateDevices();
        }

        private Device CreateDevice(XmlNode device)
        {
            XmlAttributeCollection xmlAttribs = device.Attributes;

            switch (xmlAttribs["device_type"].Value)
            {
                case Device.SwitchDeviceType: return new SwitchDevice(device, this);
                default: return new Device(device, this);
            }
        }

        private void PopulateDevices()
        {
            XmlNodeList devices = m_UserDataXml.SelectNodes("root/devices/device");
            foreach (XmlNode xmlDevice in devices)
            {
                Device device = CreateDevice(xmlDevice);
                Devices.Add(device);
            }
        }

        private void PopulateRooms()
        {
            Rooms.Clear();
            XmlNodeList rooms = m_UserDataXml.SelectNodes("root/rooms/room");
            foreach (XmlNode room in rooms)
            {
                XmlAttributeCollection xmlAttribs = room.Attributes;
                Rooms[Convert.ToInt32(xmlAttribs["id"].Value)] = xmlAttribs["name"].Value;
            }
        }

    }
}
