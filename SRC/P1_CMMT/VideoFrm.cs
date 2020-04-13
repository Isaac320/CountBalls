using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using System.IO;
using System.Threading;

namespace P1_CMMT
{
    public partial class VideoFrm : Form
    {
        MainForm form1;
        delegate void delegateShowMessage(string s);       

        public VideoFrm()
        {
            InitializeComponent();
        }

        public VideoFrm(MainForm form1)
        {
            InitializeComponent();
            this.form1 = form1;
            myinit();
        }

        private void myinit()
        {
            form1.GNumEvent += NumShow;
        }

        

        private void NumShow()
        {
            if(InvokeRequired)
            {
                BeginInvoke(new mydelegate(NumShow), new object[] { });
                return;
            }
            textBox1.Text = Global.Box1Num.ToString();
            textBox2.Text = Global.BoxSupplyNum.ToString();
            textBox3.Text = Global.Box2Num.ToString();
        }


        public void ShowImage(HObject obj)
        {
            hSmartWindowControl1.HalconWindow.DispObj(obj);
        }

        public void showOKNG()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new mydelegate(showOKNG), new object[] { });
                return;
            }
            if(Global.boxLastOKNG)
            {
                lb_OKNG.BackColor = Color.Lime;
                lb_OKNG.Text = "OK";               
            }
            else
            {
                lb_OKNG.BackColor = Color.Red;
                lb_OKNG.Text = "NG";
            }

        }

        public void showNothing()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new mydelegate(showNothing), new object[] { });
                return;
            }
            lb_OKNG.Text = "";

        }



        #region   没用的


        //public void ShowList(Product_Info PI1)
        //{
        //    //try
        //{
        //    if (listView1.InvokeRequired)
        //    {
        //        listView1.Invoke(new DelegateShowList(ShowList), new object[] { PI1 });
        //    }
        //    else
        //    {
        //        listView1.Items.Clear();
        //        listView1.BeginUpdate();
        //        ListViewItem lvi = new ListViewItem();
        //        lvi.ImageIndex = 0;
        //        lvi.Text = Global.pName[0];
        //        lvi.SubItems.Add(PI1.score.ToString());
        //        listView1.Items.Add(lvi);

        //        lvi = new ListViewItem();
        //        lvi.ImageIndex = 1;
        //        lvi.Text = Global.pName[1];
        //        lvi.SubItems.Add(PI1.mean.ToString());
        //        listView1.Items.Add(lvi);

        //        lvi = new ListViewItem();
        //        lvi.ImageIndex = 2;
        //        lvi.Text = Global.pName[2];
        //        lvi.SubItems.Add(PI1.deviation.ToString());
        //        listView1.Items.Add(lvi);

        //        listView1.EndUpdate();

        //    }
        //}
        //catch(Exception ee)
        //{
        //    //APLog.Write(Priority.ALERT, ee.ToString());
        //    LogManager.WriteLog(ee.ToString());
        //}
        //}

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string path = Global.ImagePath1 + DateTime.Now.ToString("yyyyMMdd") + "\\";

        //        if (!Directory.Exists(path))
        //        {
        //            Directory.CreateDirectory(path);
        //        }

        //        HOperatorSet.WriteImage(hWindowControl1.HalconWindow.DumpWindowImage(), "tiff", 0, path + DateTime.Now.ToString("HH_mm_ss")+" J");


        //    }
        //    catch(Exception ee)
        //    {
        //        LogManager.WriteLog("界面保存图像错误" + ee.ToString());
        //    }


        //}

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string path = Global.ImagePath1 + DateTime.Now.ToString("yyyyMMdd") + "\\";

        //        if (!Directory.Exists(path))
        //        {
        //            Directory.CreateDirectory(path);
        //        }

        //        HOperatorSet.WriteImage(hWindowControl2.HalconWindow.DumpWindowImage(), "tiff", 0, path + DateTime.Now.ToString("HH_mm_ss")+" X");


        //    }
        //    catch (Exception ee)
        //    {
        //        LogManager.WriteLog("界面保存图像错误" + ee.ToString());
        //    }
        //}

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            if (Global.TimeLeft > 0)
            {
                Global.myMState = MachineState.Working;
                button1.Enabled = false;
                button1.BackColor = Color.LightCoral;
                button2.Enabled = true;
                button2.BackColor = Color.Green;
                listBoxShowMessage("开始！切换为工作状态");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBoxShowMessage("暂停!切换为空闲状态中...");
            listBoxShowMessage("box1将不进行下一轮工作，直到按开始继续工作");
            Global.myMState = MachineState.Free;
            //Global.myWorkState = WorkState.Free;

            button2.Enabled = false;
            button2.BackColor = Color.LightCoral;

            button1.Enabled = true;
            button1.BackColor = Color.Green;

        }

        
        private void button3_Click(object sender, EventArgs e)
        {
            listBoxShowMessage("清空!清空设备所有产品..计数清零");
            ecat_io.EC_Set_Digital_Chn_Out(2, 6, 1, 0);
            ecat_io.EC_Set_Digital_Chn_Out(2, 7, 1, 0);           
            Global.Box1Num = 0;
            Global.Box2Num = 0;
            Global.BoxSupplyNum = 0;

            Global.isFirstRun = true;
            button1.Enabled = true;
            button1.BackColor = Color.Green;
        }

       

        private void button4_Click(object sender, EventArgs e)
        {
            Global.BoxSupplyNum = 500;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Global.box2State = BoxState.Half;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Global.Box1WorkState == BoxWorkState.Free)
            {
                button3.Enabled = true;
                button3.BackColor = Color.Lime;
                button5.Enabled = true;
                button5.BackColor = Color.Lime;
            }
            else
            {
                button3.Enabled = false;
                button3.BackColor = Color.LightCoral;
                button5.Enabled = false;
                button5.BackColor = Color.LightCoral;
            }
        }


        public void listBoxShowMessage(string s)
        {
            if (listBox1.InvokeRequired)
            {
                BeginInvoke(new delegateShowMessage(listBoxShowMessage), new object[] { s });

            }
            else
            {
                string mystring = DateTime.Now.ToString("HH:mm:ss") + " " + s;
                listBox1.Items.Add(mystring);

                //写log
                LogManager.WriteLog(s);

                if (listBox1.Items.Count > 200)
                {
                    for (int i = 80; i > -1; i--)
                    {
                        listBox1.Items.RemoveAt(i);
                    }
                }
                listBox1.TopIndex = listBox1.Items.Count - 1;
            }
        }

        public void ShowInfo()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new mydelegate(ShowInfo), new object[] { });
                return;
            }
            lb_pName.Text = "ttt";
            lb_pLength.Text = "16mm";
            lb_pNum.Text = "1000";
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            using (ConfigListFrm cogfrm = new ConfigListFrm())
            {
                if (cogfrm.ShowDialog() == DialogResult.OK)
                {
                    lb_pName.Text = cogfrm.pName;
                    lb_pNum.Text = Global.Box2NeedNum.ToString();
                    lb_pLength.Text = "15mm";

                    listBoxShowMessage("切换为" + cogfrm.pName + "产品");
                }
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            listBoxShowMessage("停机!切换为空闲状态中...");
            Global.myMState = MachineState.Free;
            Global.myWorkState = WorkState.Free;
            Global.Box1Num = 9999999;
            Global.Box2Num = 9999999;
            Global.BoxSupplyNum = 9999999;

            Global.box1State = BoxState.Full;
            Global.needStopGO = true;

            Global.needSupplyBalls = false;

            form1.freezeCamera();
            
        }
    }
}
