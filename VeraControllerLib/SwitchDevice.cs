using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SmartHome
{
    class SwitchDevice : Device
    {
        public int Status { get { return m_States.GetIntValue(m_sSwitchServiceName, "Status"); } }
        public int Target { get { return m_States.GetIntValue(m_sSwitchServiceName, "Target"); } }

        public SwitchDevice(XmlNode device, VeraController controller) :
            base(device, controller)
        { }

        public override string ToString()
        {
            string sInfo = base.ToString() + Environment.NewLine;
            sInfo += String.Format("Status: {0}", Status) + Environment.NewLine;
            sInfo += String.Format("Target: {0}", Target);

            return sInfo;
        }

        public async Task SwitchAsync(bool bState)
        {
            List<Tuple<string, string>> parameters = new List<Tuple<string, string>>{ new Tuple<string, string>("newTargetValue", (bState ? "1" : "0")) };
            string sResponseString = await Controller.SendActionCommandAsync(ID, m_sSwitchServiceName, "SetTarget", parameters);
        }
    }
}
