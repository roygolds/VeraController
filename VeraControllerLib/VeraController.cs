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
        private XmlDocument m_StatusXml = null;

        public delegate void DebugPrintHandler(string sMsg);

        public event DebugPrintHandler DebugPrint;

        protected void OnLog(string sMsg)
        {
            DebugPrint?.Invoke(sMsg);
        }

        public Dictionary<int, string> Rooms = new Dictionary<int, string>();
        public Dictionary<int, Device> Devices = new Dictionary<int, Device>();
        public List<Alert> Alerts = new List<Alert>();
        public string Address { get; private set; }
        public VeraController(string sAddress)
        {
            Address = sAddress;
        }

        public async Task<string> SendRequestAsync(string sRequest)
        {
            string sResponseString = await m_Client.GetStringAsync(sRequest);

            OnLog(String.Format("Request: {0}\nResponse: {1}", sRequest, sResponseString));

            return sResponseString;
        }

        public async Task<string> SendCommandAsync(string sCmd)
        {
            string sRequest = String.Format("http://{0}:{1}/{2}", Address, m_uPort, sCmd);
            return await SendRequestAsync(sRequest);
        }

        public async Task<string> SendCgiCommandAsync(string sCmd)
        {
            string sRequest = String.Format("http://{0}/cgi-bin/{1}", Address, sCmd);
            return await SendRequestAsync(sRequest);
        }

        public async Task<string> SendActionCommandAsync(int iDeviceNum, string sServiceName, string sActionName, List<Tuple<string, string>> parameters = null)
        {
            string sParams = (parameters == null) ? "" : "&" + string.Join("&", parameters.Select(t => string.Format("{0}={1}", t.Item1, t.Item2)));
            string sRequest = String.Format("data_request?id=action&output_format=xml&DeviceNum={0}&serviceId={1}&action={2}{3}",
                iDeviceNum, sServiceName, sActionName, sParams);
            return await SendCommandAsync(sRequest);
        }

        public async Task ConnectAsync()
        {
            string sResponseString = await SendCommandAsync("data_request?id=user_data&output_format=xml");

            m_UserDataXml = new XmlDocument();
            m_UserDataXml.LoadXml(sResponseString);

            sResponseString = await SendCommandAsync("data_request?id=status&output_format=xml");

            m_StatusXml = new XmlDocument();
            m_StatusXml.LoadXml(sResponseString);

            PopulateRooms();
            PopulateDevices();
            PopulateAlerts();

            DumpDevicesInfo();
            DumpAlerts();
        }

        private Device CreateDevice(XmlNode device)
        {
            XmlAttributeCollection xmlAttribs = device.Attributes;

            switch (xmlAttribs["device_type"].Value)
            {
                case Device.SwitchDeviceType: return new SwitchDevice(device, this);
                case Device.RollerShutterDeviceType: return new RollerShutter(device, this);
                default: return new Device(device, this);
            }
        }

        private void PopulateDevices()
        {
            XmlNodeList devices = m_UserDataXml.SelectNodes("root/devices/device");
            foreach (XmlNode xmlDevice in devices)
            {
                Device device = CreateDevice(xmlDevice);
                Devices.Add(device.ID, device);
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
        private void PopulateAlerts()
        {
            Alerts.Clear();
            XmlNodeList alerts = m_StatusXml.SelectNodes("root/alerts/alert");
            foreach (XmlNode alert in alerts)
            {
                Alerts.Add(AlertFactory.Construct(alert, this));
            }
        }

        public async Task DeleteAlertsAsync()
        {
            string sResponseString = await SendCommandAsync("data_request?id=del_alert&pk_alert=*");
            Alerts.Clear();
        }

        public async Task RebootControllerAsync()
        {
            string sResponseString = await SendCgiCommandAsync("cmh/cmh_reboot.sh?cmd=now");
        }

        public void DumpDevicesInfo()
        {
            string sMsg = "";
            bool bAddNewLine = false;
            foreach (Device device in Devices.Values)
            {
                if (bAddNewLine) sMsg += Environment.NewLine;
                bAddNewLine = true;

                sMsg += String.Format("{0} --- {1} --- {2}: {3}", device.LocalUDN, device.ID, device.Name, device.Room);
            }
            OnLog(sMsg);
        }

        public void DumpAlerts()
        {
            string sMsg = "";
            bool bAddNewLine = false;
            foreach (Alert alert in Alerts)
            {
                if (bAddNewLine) sMsg += Environment.NewLine;
                bAddNewLine = true;

                sMsg += alert.ToString();
            }
            OnLog(sMsg);
        }
    }
}
