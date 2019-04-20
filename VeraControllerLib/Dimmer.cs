using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SmartHome
{
    class Dimmer : SwitchDevice
   {
        public int LoadLevelStatus { get { return m_States.GetIntValue(m_sDimmerServiceName, "LoadLevelStatus"); } }
        public int LoadLevelLast { get { return m_States.GetIntValue(m_sDimmerServiceName, "LoadLevelLast"); } }
        public int LoadLevelTarget { get { return m_States.GetIntValue(m_sDimmerServiceName, "LoadLevelTarget"); } }

        public Dimmer(XmlNode device, VeraController controller) :
            base(device, controller)
        { }

        public override string ToString()
        {
            string sInfo = base.ToString() + Environment.NewLine;
            sInfo += String.Format("Load Level Status: {0}", LoadLevelStatus) + Environment.NewLine;
            sInfo += String.Format("Load Level Last: {0}", LoadLevelLast) + Environment.NewLine;
            sInfo += String.Format("Load Level Target: {0}", LoadLevelTarget);

            return sInfo;
        }

        public async Task SetLoadLevelAsync(UInt16 uLevel)
        {
            if (uLevel > 100) uLevel = 100;
            List<Tuple<string, string>> parameters = new List<Tuple<string, string>> { new Tuple<string, string>("newLoadlevelTarget", uLevel.ToString()) };
            string sResponseString = await Controller.SendActionCommandAsync(ID, m_sDimmerServiceName, "SetLoadLevelTarget", parameters);
        }

    }
}
