using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Fiddler;


namespace XSniffer
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        public MemoryMappedFile MeM;
        public Regex R = new Regex(".*?");
        public List<Data> DataList = new List<Data>();
        public bool IsStop = true;
        private delegate void myDelegate();
        public int Type = 0;
        public bool ClosePipe = false;
        public string MemStr = "";

        public void ShutDown()
        {
            try
            {
                if (Fiddler.FiddlerApplication.IsStarted())
                {
                    Fiddler.FiddlerApplication.Shutdown();
                }
            }
            catch
            {

            }

        }
        private string GetHeaderStr(HTTPRequestHeaders headers)
        {
            string cook = "";

            for (int i = 0; i < headers.Count(); i++)
            {
                cook += headers[i].Name + ":" + headers[i].Value + "\r\n";
            }
            return cook;
        }
        public void UpdateMainListView()
        {
            MemStr = "";
            if (this.InvokeRequired)
            {

                this.Invoke(new myDelegate(UpdateMainListView));

            }
            else
            {

                listView_main.BeginUpdate();
                if (DataList == null)
                {

                }
                else if (DataList.Count == 0)
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        listView_main.Items.Clear();
                    }));

                }
                else if (listView_main.Items.Count > DataList.Count)
                {
                    listView_main.Items.Clear();
                }
                for (int i = 0; i < DataList.Count; i++)
                {
                    MemStr += DataList[i].Request_Url + "\r\n";
                    int id = i + 1;
                    ListViewItem item = new ListViewItem();
                    if (listView_main.Items.Count > i)
                    {
                        if (listView_main.Items[i].Text != id.ToString())
                        {
                            listView_main.Items[i].Text = id.ToString();

                        }
                        if (listView_main.Items[i].SubItems[1].Text != DataList[i].Request_Method.ToString())
                        {
                            listView_main.Items[i].SubItems[1].Text = DataList[i].Request_Method.ToString();
                        }
                        if (listView_main.Items[i].SubItems[2].Text != DataList[i].Request_Url.ToString())
                        {
                            listView_main.Items[i].SubItems[2].Text = DataList[i].Request_Url.ToString();
                        }


                    }
                    else
                    {
                        item.Text = id.ToString();
                        item.SubItems.Add(DataList[i].Request_Method.ToString());
                        item.SubItems.Add(DataList[i].Request_Url.ToString());
                        item.Tag = DataList[i];
                        listView_main.Items.Add(item);

                    }
                }

                listView_main.EndUpdate();
                if (listView_main.Items.Count > 1)
                {
                    listView_main.EnsureVisible(listView_main.Items.Count - 1);
                }

            }

        }
        public void Sniffer()
        {
            Fiddler.FiddlerApplication.BeforeRequest += delegate(Fiddler.Session os)
            {

            };
            Fiddler.FiddlerApplication.AfterSessionComplete += delegate(Fiddler.Session os)
            {
                if (os.RequestMethod.Length==0)
                {
                    return;
                }
                if (Type == 1 && os.RequestMethod.ToUpper() != "GET")
                {
                    return;
                }
                else if (Type == 2 && os.RequestMethod.ToUpper() != "POST")
                {
                    return;
                }
                if (textBox_regex.Text.Length > 0)
                {
                    R = new Regex(textBox_regex.Text);
                    if (R.IsMatch(os.fullUrl))
                    {
                        Data d = new Data();
                        d.Request_Method = os.RequestMethod;
                        d.Request_Url = os.fullUrl;
                        d.Request_Body = os.GetRequestBodyEncoding().GetString(os.RequestBody);
                        d.Request_Headers = GetHeaderStr(os.RequestHeaders);
                        DataList.Add(d);
                    }
                }
                else
                {
                    Data d = new Data();
                    d.Request_Method = os.RequestMethod;
                    d.Request_Url = os.fullUrl;
                    d.Request_Body = os.GetRequestBodyEncoding().GetString(os.RequestBody);
                    d.Request_Headers = GetHeaderStr(os.RequestHeaders);
                    DataList.Add(d);
                }

                UpdateMainListView();
            };
            Fiddler.FiddlerApplication.Startup(7777, true, true);
        }

        public void StartShareMemory()
        {
            Thread thread = new Thread(delegate()
            {



                try
                {
                    MeM = MemoryMappedFile.CreateNew("XSniffer", 9999999);
                }
                catch
                {
                    MeM = MemoryMappedFile.OpenExisting("XSniffer");
                }
                while (true)
                {
                    try
                    {
                        lock ("StartShareMemory")
                        {
                            MemoryMappedViewStream stream = MeM.CreateViewStream(0, 9999999, MemoryMappedFileAccess.Write);
                            stream.Write(new byte[9999999], 0, 9999999);
                            Byte[] data = System.Text.Encoding.UTF8.GetBytes(GetJsonStr());
                            if (data.Length > 0)
                            {
                                stream = MeM.CreateViewStream(0, data.Length, MemoryMappedFileAccess.Write);
                                stream.Write(data, 0, data.Length);
                            }
                        }

                    }
                    catch (Exception)
                    {

                    }
                    Thread.Sleep(100);
                    if (ClosePipe)
                    {
                        break;
                    }
                }
                MeM.Dispose();
            });
            thread.IsBackground = true;
            thread.Start();
        }

        public string GetJsonStr()
        {
            lock ("GetJsonStr")
            {
                Data[] d = DataList.ToArray();
                MemoryStream stream = new MemoryStream();
                DataContractJsonSerializer json = new DataContractJsonSerializer(d.GetType());
                json.WriteObject(stream, d);
                string str = Encoding.UTF8.GetString(stream.ToArray());
                stream.Close();
                return str;
            }



        }


        private void frmMain_Load(object sender, EventArgs e)
        {
            StartShareMemory();
            Point news = new Point();
            news.X = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Width - this.Width;
            news.Y = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Height - this.Height;
            this.Location = news;

            if (Program.StartArgs != null & Program.StartArgs.Length == 3)
            {
                textBox_regex.Text = Program.StartArgs[2];
                if (Program.StartArgs[1].ToLower() == "get")
                {
                    仅GETToolStripMenuItem_Click(null, null);
                }
                else if (Program.StartArgs[1].ToLower() == "post")
                {
                    仅POSTToolStripMenuItem_Click(null, null);
                }
                if (Program.StartArgs[0].ToLower() == "start")
                {
                    btn_start_Click(null, null);
                }
            }
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            if (IsStop)
            {
                Sniffer();
                btn_start.Image = Properties.Resources.btn_stop;
                btn_start.Text = "停止监听";
                IsStop = false;
                notifyIcon.ShowBalloonTip(1000, "监听中...", "现在请打开任意浏览器并进行相应操作", ToolTipIcon.Info);
            }
            else
            {
                ShutDown();
                btn_start.Image = Properties.Resources.btn_start;
                btn_start.Text = "开始监听";
                IsStop = true;
            }
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            ClosePipe = true;
            try
            {
                MeM.Dispose();

            }
            catch
            {

            }
            ShutDown();
            notifyIcon.Dispose();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            richTextBox.Text = "";
            DataList.Clear();
            UpdateMainListView();
        }

        private void 仅GETToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Type = 1;
            btn_Type.Text = "仅Get";
        }

        private void 仅POSTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Type = 2;
            btn_Type.Text = "仅Post";
        }

        private void Get和PostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Type = 0;
            btn_Type.Text = "Get和Post";
        }

        private void listView_main_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView_main.SelectedItems.Count > 0)
            {
                string DataStr = "";
                Data d = (Data)listView_main.SelectedItems[0].Tag;
                DataStr += d.Request_Method + " " + d.Request_Url + "\r\n\r\n";
                DataStr += d.Request_Headers + "\r\n\r\n";
                DataStr += d.Request_Body;
                richTextBox.Text = DataStr;

            }

        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            this.IsStop = true;
            this.WindowState = FormWindowState.Normal;
        }

        private void listView_main_MouseEnter(object sender, EventArgs e)
        {

            this.Focus();
        }
        private void toolStrip_MouseEnter(object sender, EventArgs e)
        {
            this.Focus();
        }

        private void richTextBox_MouseEnter(object sender, EventArgs e)
        {
            this.Focus();
        }

        private void frmMain_MouseEnter(object sender, EventArgs e)
        {
            this.Focus();
        }

    }

    public class Data
    {
        public string Request_Method = "GET";
        public string Request_Url = "";
        public string Request_Body = "";
        public string Request_Headers = "";
    }
}
