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
            string sRequest = String.Format("data_request?id=action&output_format=xml&DeviceNum={0}&serviceId={1}&action=SetTarget&newTargetValue={2}", 
                ID, m_sSwitchServiceName, (bState ? 1 : 0));
            string sResponseString = await Controller.SendCommandAsync(sRequest);
        }
    }
}
