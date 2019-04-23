using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Xml;

namespace SmartHome
{
    public partial class frmMain : Form
    {
        private VeraController m_Controller;

        public frmMain()
        {
            InitializeComponent();
        }

        private void AddTextToOutput(string sText)
        {
            int iCurrentPos = txtOutput.Text.Length;
            txtOutput.AppendText(sText.Replace("\n", "\r\n"));
            txtOutput.AppendText(Environment.NewLine);
            txtOutput.AppendText("------------------------------------------------");
            txtOutput.AppendText(Environment.NewLine);
            txtOutput.SelectionStart = iCurrentPos;
            txtOutput.ScrollToCaret();
        }

        private void PopulateDeviceTreeView()
        {
            tvwDevices.Nodes.Clear();

            foreach (Device device in m_Controller.Devices.Values)
            {
                TreeNodeCollection parentNodes = tvwDevices.Nodes;
                if (device.ParentID > -1)
                {
                    TreeNode[] nodes = tvwDevices.Nodes.Find(device.ParentID.ToString(), true);
                    if (nodes.Length > 0) parentNodes = nodes[0].Nodes;
                }

                string sDeviceName = ((device.RoomID > -1) ? device.Room + ": " : "") + device.Name;
                TreeNode newNode = parentNodes.Add(device.ID.ToString(), sDeviceName);
                newNode.Tag = device;
            }

            UpdateAlertsInTreeView();

            tvwDevices.ExpandAll();
        }

        private void ClearAlertsFromSubTree(TreeNode node)
        {
            Font oldFont = ((node.NodeFont == null) ? node.TreeView.Font : node.NodeFont);

            node.NodeFont = new Font(oldFont, oldFont.Style & (~FontStyle.Bold));
            node.ToolTipText = null;

            foreach(TreeNode child in node.Nodes)
            {
                ClearAlertsFromSubTree(child);
            }
        }

        private void ClearAlertsFromTreeView()
        {
            foreach (TreeNode node in tvwDevices.Nodes)
            {
                ClearAlertsFromSubTree(node);
            }
        }

        private void UpdateAlertsInTreeView()
        {
            ClearAlertsFromTreeView();

            foreach (Alert alert in m_Controller.Alerts)
            {
                if (alert is DeviceAlert)
                {
                    DeviceAlert devAlert = (DeviceAlert)alert;
                    TreeNode[] nodes = tvwDevices.Nodes.Find(devAlert.DeviceID.ToString(), true);
                    if (nodes.Length > 0)
                    {
                        foreach (TreeNode node in nodes)
                        {
                            Font oldFont = ((node.NodeFont == null) ? node.TreeView.Font : node.NodeFont);
                            node.NodeFont = new Font(oldFont, oldFont.Style | FontStyle.Bold);

                            string sAlert = ((node.ToolTipText == null) ? "" : (node.ToolTipText + Environment.NewLine));

                            node.ToolTipText = sAlert + devAlert.ToString();
                        }
                    }
                }
            }
        }

        private async void btnGetDevices_ClickAsync(object sender, EventArgs e)
        {
            m_Controller = new VeraController(txtControllerAddress.Text);

            m_Controller.DebugPrint += AddTextToOutput;

            await m_Controller.ConnectAsync();

            PopulateDeviceTreeView();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtOutput.Clear();
        }

        private async void btnPrintDeviceInfo_ClickAsync(object sender, EventArgs e)
        {
            Device device = (Device)(tvwDevices.SelectedNode.Tag);
            await device.PollAndUpdateAsync();
            AddTextToOutput(device.ToString());
        }

        private async void btnSwitchOn_ClickAsync(object sender, EventArgs e)
        {
            SwitchDevice device = tvwDevices.SelectedNode.Tag as SwitchDevice;
            if (device != null) await device.SwitchAsync(true);
        }

        private async void btnSwitchOff_ClickAsync(object sender, EventArgs e)
        {
            SwitchDevice device = tvwDevices.SelectedNode.Tag as SwitchDevice;
            if (device != null) await device.SwitchAsync(false);
        }

        private async void btnSetLevel_Click(object sender, EventArgs e)
        {
            Dimmer device = tvwDevices.SelectedNode.Tag as Dimmer;
            if (device != null) await device.SetLoadLevelAsync(Convert.ToUInt16(txtLevel.Text));
        }

        private async void btnDeleteAlerts_ClickAsync(object sender, EventArgs e)
        {
            await m_Controller.DeleteAlertsAsync();
        }

        private async void btnReboot_ClickAsync(object sender, EventArgs e)
        {
            await m_Controller.RebootControllerAsync();
        }
    }
}
