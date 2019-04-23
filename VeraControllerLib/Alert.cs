using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Net.Http;
using System.ComponentModel;

namespace SmartHome
{
    class Alert
    {
        public enum EventType
        {
            [Description("All")]
            All = -1,
            [Description("Camera sent image")]
            CameraSentImage = 1,
            [Description("Camera sent video")]
            CameraVideoSent = 2,
            [Description("User created trigger fired")]
            UserCreatedTriggerFired = 3,
            [Description("Device changed state")]
            DeviceChangedState = 4,
            [Description("User logged in")]
            UserLoggedIn = 5,
            [Description("Gateway came online")]
            GatewayOnline = 6,
            [Description("System error")]
            SysError = 7,
            [Description("Email validation")]
            ValidateEmail = 8,
            [Description("SMS validation")]
            ValidateSms = 9,
            [Description("System message")]
            SysMessage = 10,
            [Description("Test SMS/Email")]
            TestSmsEmail = 11,
            [Description("Burglary")]
            Burglary = 12,
            [Description("Fire")]
            Fire = 13,
            [Description("Internet down")]
            InternetDown = 14,
            [Description("Gateway offline")]
            GatewayOffline = 15,
            [Description("Device status")]
            DeviceStatus = 16,
            [Description("Installer access")]
            InstallerAccess = 17,
            [Description("Installer access active")]
            InstallerAccessActive = 18,
            [Description("Welcome email")]
            WelcomeEmail = 19,
            [Description("Installer welcome email")]
            InstallerWelcomeEmail = 20,
            [Description("Invitation email admin")]
            InvitationEmailAdmin = 21,
            [Description("Invitation email simple guest")]
            InvitationEmailSimpleGuest = 22,
            [Description("Invitation email notifier")]
            InvitationEmailNotifier = 23,
            [Description("Vera battery disconnected")]
            BatteryDisconnected = 70,
            [Description("Doorbell ringing")]
            DoorbellRinging = 71,
            [Description("Doorbell ring accepted")]
            DoorbellAccepted = 72,
            [Description("Doorbell ring rejected")]
            DoorbellRejected = 73,
            [Description("Doorbell ring missed")]
            DoorbellMissed = 74,
            [Description("Doorbell ring answered")]
            DoorbellAnswered = 75
        }

        public enum SourceTypes
        {
            [Description("All")]
            All = -1,
            [Description("Alarms")]
            Alarms = 24,
            [Description("Thermostats")]
            Thermostats = 5,
            [Description("Dimmers")]
            Dimmers = 2,
            [Description("Switches")]
            DeviceTypeBinaryLightPlural = 3,
            [Description("Cameras")]
            Cameras = 6,
            [Description("Locks")]
            Locks = 7,
            [Description("Sensors")]
            Sensors = 4
        }

        public string Description { get; private set; }
        public ulong ID { get; private set; }
        public EventType Type { get; private set; }
        public UnixTime Timestamp { get; private set; }
        public DateTime LocalTime { get; private set; }
        protected VeraController Controller { get; set; }

        public Alert(XmlNode alert, VeraController controller)
        {
            XmlAttributeCollection xmlAttribs = alert.Attributes;

            Controller = controller;
            Description = xmlAttribs["Description"].Value;
            Timestamp = Convert.ToInt64(xmlAttribs["LocalTimestamp"].Value);
            LocalTime = Convert.ToDateTime(xmlAttribs["LocalDate"].Value);
            ID = Convert.ToUInt64(xmlAttribs["PK_Alert"].Value);
            Type = (EventType)Convert.ToUInt32(xmlAttribs["EventType"].Value);
        }

        public async Task DeleteAlertAsync()
        {
            string sRequest = String.Format("data_request?id=del_alert&pk_alert={0}", ID);
            string sResponseString = await Controller.SendCommandAsync(sRequest);
        }

        public override string ToString()
        {
            return String.Format("{0}: {1} - {2}", LocalTime, Type.GetDescription(), Description);
        }
    }

    class DeviceAlert : Alert
    {
        public int DeviceID { get; private set; }
        public int RoomID { get; private set; }
        public string DeviceName { get; private set; }
        public string DeviceType { get; private set; }

        public DeviceAlert(XmlNode alert, VeraController controller) :
            base(alert, controller)
        {
            XmlAttributeCollection xmlAttribs = alert.Attributes;

            DeviceID = Convert.ToInt32(xmlAttribs["PK_Device"].Value);
            RoomID = Convert.ToInt32(xmlAttribs["Room"].Value);
            DeviceName = xmlAttribs["DeviceName"].Value;
            DeviceType = xmlAttribs["DeviceType"].Value;
        }

        public override string ToString()
        {
            //Device device = Controller.Devices[DeviceID];
            return String.Format("{0}: {1} - {2}: {3}", Timestamp, Type.GetDescription(), DeviceName, Description);
        }
    }

    class AlertFactory
    {
        public static Alert Construct(XmlNode alert, VeraController controller)
        {
            XmlAttributeCollection  xmlAttribs = alert.Attributes;
            Alert.EventType         iEventType = (Alert.EventType)Convert.ToInt32(xmlAttribs["EventType"].Value);
            Alert retAlert = null;

            switch(iEventType)
            {
                case Alert.EventType.DeviceStatus:
                    retAlert = new DeviceAlert(alert, controller);
                    break;
                case Alert.EventType.SysError:
                default:
                    retAlert = new Alert(alert, controller);
                    break;
            }

            return retAlert;
        }
    }
}
