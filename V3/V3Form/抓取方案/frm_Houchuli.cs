using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Threading;
namespace V3.V3Form
{
    public partial class frm_Houchuli : DevExpress.XtraEditors.XtraForm
    {
        public frm_Houchuli()
        {
            InitializeComponent();
            frm_ChildEngRules.CheckForIllegalCrossThreadCalls = false;
        }
        public Model.抓取相关模型.RulesEngLv1 ruleslv1 = new Model.抓取相关模型.RulesEngLv1();
        public bool issave = false;
        public string cookietxt = "";

        /// <summary>
        /// 保存信息到类
        /// </summary>
        private void SetInfo()
        {
            string[] tempstr = txtReplacedata.Lines;
            System.Collections.ArrayList list = new System.Collections.ArrayList();
            for (int i = 0; i < tempstr.Length; i++)
            {
                if (tempstr[i].Trim().Split('→').Length == 2)
                {
                    list.Add(tempstr[i].Trim());
                }
            }
            ruleslv1.HouReplaceData.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                string[] temp = list[i].ToString().Split('→');
                if (!ruleslv1.HouReplaceData.ContainsKey(temp[0]))
                {
                    ruleslv1.HouReplaceData.Add(temp[0], temp[1]);
                }
                else
                {
                    ruleslv1.HouReplaceData[temp[0]] = temp[1];
                }
            }

            string[] s = textBoxX_Filter.Lines;
            System.Collections.ArrayList ar = new System.Collections.ArrayList();
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i].Trim().Length > 0)
                    ar.Add(s[i].Trim());
            }
            ruleslv1.Guolvbiaoqian = new string[ar.Count];
            for (int i = 0; i < ar.Count; i++)
            {
                ruleslv1.Guolvbiaoqian[i] = ar[i].ToString();
            }
        }
        /// <summary>
        /// 从类读取信息
        /// </summary>
        private void GetInfo()
        {
            txtReplacedata.Text = "";
            foreach (KeyValuePair<string, string> value in ruleslv1.HouReplaceData)
            {
                txtReplacedata.Text += value.Key + "→" + value.Value + "\r\n";
            }
            textBoxX_Filter.Text = "";
            foreach (string ss in ruleslv1.Guolvbiaoqian)
                textBoxX_Filter.Text += ss + "\r\n";
        }

        private void buttonItem3_Click(object sender, EventArgs e)
        {
          
        }

        private void buttonItem1_Click(object sender, EventArgs e)
        {
          
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            txtReplacedata.SelectedText = "→";
            txtReplacedata.Focus();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
          
            textBoxX_Filter.Focus();
        }

        private void frm_Houchuli_Load(object sender, EventArgs e)
        {
            GetInfo();
        }

        private void btn_save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetInfo();
            issave = true;
            this.Close();
        }

        private void btn_cancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            issave = false;
            this.Close();

        }

        private void linkLabel_Replace_Default_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            txtReplacedata.Text += "<script[\\s\\S]*?</script>→\r\n";
        }

        private void linkLabel_Filter_Default_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBoxX_Filter.Text += "script\r\na\r\ndiv\r\nstyle\r\nspan\r\ntd\r\ntr\r\nfont\r\ntable\r\nul\r\ni\r\nem\r\nli\r\n";
        }

    }
}