using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SmartHome
{
    class RollerShutter : Dimmer
    {
        public RollerShutter(XmlNode device, VeraController controller) :
            base(device, controller)
        { }

    }
}
