using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Net.Http;

namespace SmartHome
{
    class Device
    {
        public class States
        {
            public class State
            {
                public string Service { get; private set; }
                public string Variable { get; private set; }
                public string Value { get; private set; }
                public int ID { get; private set; }

                public string CanonicalName
                {
                    get
                    {
                        return Service + ":" + Variable;
                    }
                }

                public State(XmlNode state)
                {
                    XmlAttributeCollection xmlAttribs = state.Attributes;

                    Service = xmlAttribs["service"].Value;
                    Variable = xmlAttribs["variable"].Value;
                    Value = xmlAttribs["value"].Value;
                    ID = Convert.ToInt32(xmlAttribs["id"].Value);
                }
            }

            private Dictionary<string, State> m_StatesMap { get; set; } = new Dictionary<string, State>();

            public States(XmlNode device)
            {
                Update(device);
            }

            public void Update(XmlNode device)
            {
                m_StatesMap.Clear();
                XmlNodeList states = device.SelectNodes("states/state");
                foreach (XmlNode state in states)
                {
                    State newState = new State(state);
                    m_StatesMap[newState.CanonicalName] = newState;
                }
            }

            public bool GetStateValue(string sServiceName, string sVarName, out string sValue)
            {
                State state;
                bool bRet = m_StatesMap.TryGetValue(sServiceName + ":" + sVarName, out state);
                sValue = bRet ? state.Value : null;
                return bRet;
            }

            public bool GetStateValue(string sServiceName, string sVarName, out int iValue)
            {
                string sValue;
                bool bRet = GetStateValue(sServiceName, sVarName, out sValue);
                iValue = bRet ? Convert.ToInt32(sValue) : -1;
                return bRet;
            }

            public bool GetStateValue(string sServiceName, string sVarName, out float fValue)
            {
                string sValue;
                fValue = float.NaN;
                bool bRet = GetStateValue(sServiceName, sVarName, out sValue);
                bRet = bRet ? Single.TryParse(sValue, out fValue) : false;
                return bRet;
            }

            public float GetFloatValue(string sServiceName, string sVarName)
            {
                float fValue;
                bool bRet = GetStateValue(sServiceName, sVarName, out fValue);
                return fValue;
            }

            public int GetIntValue(string sServiceName, string sVarName)
            {
                string sValue;
                bool bRet = GetStateValue(sServiceName, sVarName, out sValue);
                return bRet ? Convert.ToInt32(sValue) : -1;
            }
        }

        public const string SwitchDeviceType = "urn:schemas-upnp-org:device:BinaryLight:1";
        public const string DimmerDeviceType = "urn:schemas-upnp-org:device:Dimmer:1";
        public const string RollerShutterDeviceType = "urn:schemas-micasaverde-com:device:WindowCovering:1";

        protected const string m_sEnergyServiceName = "urn:micasaverde-com:serviceId:EnergyMetering1";
        protected const string m_sSwitchServiceName = "urn:upnp-org:serviceId:SwitchPower1";
        protected const string m_sDimmerServiceName = "urn:upnp-org:serviceId:Dimming1";
        protected const string m_sZWaveNetworkServiceName = "urn:micasaverde-com:serviceId:ZWaveNetwork1";

        

        public string Name { get; private set; }
        public string Room { get; private set; }
        public int ID { get; private set; }
        public string LocalUDN { get; private set; }
        public string Type { get; private set; }
        public int ParentID { get; private set; }
        public float Watts { get { return m_States.GetFloatValue(m_sEnergyServiceName, "Watts"); } }
        public float KWH { get { return m_States.GetFloatValue(m_sEnergyServiceName, "KWH"); } }
        public UnixTime KwhReading { get { return m_States.GetIntValue(m_sEnergyServiceName, "KWHReading"); } }
        public UnixTime LastPollSuccess { get { return m_States.GetIntValue(m_sZWaveNetworkServiceName, "LastPollSuccess"); } }
        public int RoomID { get; private set; }
        protected States m_States { get; set; } = null;
        protected VeraController Controller { get; set; }

        public Device(XmlNode device, VeraController controller)
        {
            Controller = controller;
            Update(device, Controller.Rooms);
        }

        public override string ToString()
        {
            string sInfo = String.Format("Device Name: {0}", Name) + Environment.NewLine;
            sInfo += String.Format("Device Type: {0}", Type) + Environment.NewLine;
            sInfo += String.Format("Device ID: {0}", ID) + Environment.NewLine;
            sInfo += String.Format("Device Local UDN: {0}", LocalUDN) + Environment.NewLine;
            sInfo += String.Format("Parent ID: {0}", ParentID) + Environment.NewLine;
            sInfo += String.Format("Room: {0}", Room) + Environment.NewLine;
            sInfo += String.Format("Watts: {0}", Watts) + Environment.NewLine;
            sInfo += String.Format("kWH: {0}", KWH) + Environment.NewLine;
            sInfo += String.Format("kWH Reading: {0}", KwhReading) + Environment.NewLine;
            sInfo += String.Format("Last Poll Success: {0}", LastPollSuccess);

            return sInfo;
        }

        public void Update(XmlNode device, Dictionary<int, string> rooms = null)
        {
            XmlAttributeCollection xmlAttribs = device.Attributes;

            ID = Convert.ToInt32(xmlAttribs["id"].Value);
            Name = xmlAttribs["name"].Value;
            Type = xmlAttribs["device_type"].Value;
            LocalUDN = xmlAttribs["local_udn"].Value;

            XmlAttribute parentIdAttrib = xmlAttribs["id_parent"];
            ParentID = ((parentIdAttrib != null) ? Convert.ToInt32(parentIdAttrib.Value) : -1);

            XmlAttribute roomIdAttrib = xmlAttribs["room"];
            RoomID = ((roomIdAttrib != null) ? Convert.ToInt32(roomIdAttrib.Value) : -1);

            string sRoom = "";
            if (rooms != null) rooms.TryGetValue(RoomID, out sRoom);
            Room = sRoom;

            m_States = new States(device);
        }

        public async Task PollAndUpdateAsync()
        {
            string sRequest = String.Format("data_request?id=status&output_format=xml&DeviceNum={0}", ID);
            string sResponseString = await Controller.SendCommandAsync(sRequest);

            XmlDocument deviceDataXml = new XmlDocument();
            deviceDataXml.LoadXml(sResponseString);

            XmlNode device = deviceDataXml.SelectSingleNode(String.Format("root/Device_Num_{0}", ID));

            m_States.Update(device);
        }

    }
}
