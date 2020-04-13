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
    public partial class SettingFrm : Form
    {
        MainForm form1;

        
        public SettingFrm()
        {
            InitializeComponent();
            Init();
        }

        public SettingFrm(MainForm form1)
        {
            InitializeComponent();
            this.form1 = form1;
            Init();
        }

        public void Init()
        {

            showTextBox();
            showData();

        }

        public void showTextBox()
        {
            try
            {
                textBox1.Text = Global.Box1NeedNum.ToString();
                textBox2.Text = Global.Box1ForSupplyNum.ToString();
                textBox3.Text = Global.Box2NeedNum.ToString();
                textBox4.Text = Global.Box3MinNum.ToString();
                textBox5.Text = (Global.TimeQ1 / 1000).ToString();
                textBox6.Text = (Global.TimeCam / 1000).ToString();
                textBox7.Text = Global.Box1NeedNum2.ToString();

                textBoxT1.Text = Global.goTime1.ToString();
                textBoxT2.Text = Global.goTime2.ToString();
                textBoxT3.Text = Global.goTime3.ToString();
                textBoxT4.Text = Global.goTime4.ToString();

                textBoxSpT1.Text = Global.spTime1.ToString();
                textBoxSpT2.Text = Global.spTime2.ToString();
                textBoxSpT3.Text = Global.spTime3.ToString();


                tb_L1.Text = Global.hv_myLength[0].I.ToString();
                tb_L2.Text = Global.hv_myLength[1].I.ToString();
                tb_L3.Text = Global.hv_myLength[2].I.ToString();
                tb_L4.Text = Global.hv_myLength[3].I.ToString();
                tb_L5.Text = Global.hv_myLength[4].I.ToString();
                tb_L6.Text = Global.hv_myLength[5].I.ToString();
                tb_L7.Text = Global.hv_myLength[6].I.ToString();
                tb_L8.Text = Global.hv_myLength[7].I.ToString();
                tb_L9.Text = Global.hv_myLength[8].I.ToString();
                tb_L10.Text = Global.hv_myLength[9].I.ToString();
                tb_L11.Text = Global.hv_myLength[10].I.ToString();
                tb_L12.Text = Global.hv_myLength[11].I.ToString();
                tb_L13.Text = Global.hv_myLength[12].I.ToString();
                tb_L14.Text = Global.hv_myLength[13].I.ToString();
                tb_L15.Text = Global.hv_myLength[14].I.ToString();


                tb_th1.Text = Global.hv_myThreashod[0].I.ToString();
                tb_th2.Text = Global.hv_myThreashod[1].I.ToString();
                tb_th3.Text = Global.hv_myThreashod[2].I.ToString();
                tb_th4.Text = Global.hv_myThreashod[3].I.ToString();
                tb_th5.Text = Global.hv_myThreashod[4].I.ToString();
                tb_th6.Text = Global.hv_myThreashod[5].I.ToString();
                tb_th7.Text = Global.hv_myThreashod[6].I.ToString();
                tb_th8.Text = Global.hv_myThreashod[7].I.ToString();
                tb_th9.Text = Global.hv_myThreashod[8].I.ToString();
                tb_th10.Text = Global.hv_myThreashod[9].I.ToString();
                tb_th11.Text = Global.hv_myThreashod[10].I.ToString();
                tb_th12.Text = Global.hv_myThreashod[11].I.ToString();
                tb_th13.Text = Global.hv_myThreashod[12].I.ToString();
                tb_th14.Text = Global.hv_myThreashod[13].I.ToString();
                tb_th15.Text = Global.hv_myThreashod[14].I.ToString();

                tb_ch0.Text = Global.hv_channels[0].I.ToString();
                tb_ch1.Text = Global.hv_channels[1].I.ToString();
                tb_ch2.Text = Global.hv_channels[2].I.ToString();
                tb_ch3.Text = Global.hv_channels[3].I.ToString();
                tb_ch4.Text = Global.hv_channels[4].I.ToString();
                tb_ch5.Text = Global.hv_channels[5].I.ToString();
                tb_ch6.Text = Global.hv_channels[6].I.ToString();
                tb_ch7.Text = Global.hv_channels[7].I.ToString();
                tb_ch8.Text = Global.hv_channels[8].I.ToString();
                tb_ch9.Text = Global.hv_channels[9].I.ToString();
                tb_ch10.Text = Global.hv_channels[10].I.ToString();
                tb_ch11.Text = Global.hv_channels[11].I.ToString();
                tb_ch12.Text = Global.hv_channels[12].I.ToString();
                tb_ch13.Text = Global.hv_channels[13].I.ToString();
                tb_ch14.Text = Global.hv_channels[14].I.ToString();
                tb_ch15.Text = Global.hv_channels[15].I.ToString();



            }
            catch(Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }


        private void getTextBox()
        {
            try
            {
                Global.Box1NeedNum = int.Parse(textBox1.Text);
                Global.Box1ForSupplyNum = int.Parse(textBox2.Text);
                Global.Box2NeedNum = int.Parse(textBox3.Text);
                Global.Box3MinNum = int.Parse(textBox4.Text);
                Global.Box1NeedNum2 = int.Parse(textBox7.Text);

                Global.TimeQ1 = int.Parse(textBox5.Text) * 1000;
                Global.TimeCam = int.Parse(textBox6.Text) * 1000;


                Global.goTime1 = int.Parse(textBoxT1.Text);
                Global.goTime2 = int.Parse(textBoxT2.Text);
                Global.goTime3 = int.Parse(textBoxT3.Text);
                Global.goTime4 = int.Parse(textBoxT4.Text);


                Global.spTime1 = int.Parse(textBoxSpT1.Text);
                Global.spTime2 = int.Parse(textBoxSpT2.Text);
                Global.spTime3 = int.Parse(textBoxSpT3.Text);


                Global.hv_myLength[0] = int.Parse(tb_L1.Text);
                Global.hv_myLength[1] = int.Parse(tb_L2.Text);
                Global.hv_myLength[2] = int.Parse(tb_L3.Text);
                Global.hv_myLength[3] = int.Parse(tb_L4.Text);
                Global.hv_myLength[4] = int.Parse(tb_L5.Text);
                Global.hv_myLength[5] = int.Parse(tb_L6.Text);
                Global.hv_myLength[6] = int.Parse(tb_L7.Text);
                Global.hv_myLength[7] = int.Parse(tb_L8.Text);
                Global.hv_myLength[8] = int.Parse(tb_L9.Text);
                Global.hv_myLength[9] = int.Parse(tb_L10.Text);
                Global.hv_myLength[10] = int.Parse(tb_L11.Text);
                Global.hv_myLength[11] = int.Parse(tb_L12.Text);
                Global.hv_myLength[12] = int.Parse(tb_L13.Text);
                Global.hv_myLength[13] = int.Parse(tb_L14.Text);
                Global.hv_myLength[14] = int.Parse(tb_L15.Text);

                Global.hv_myThreashod[0] = int.Parse(tb_th1.Text);
                Global.hv_myThreashod[1] = int.Parse(tb_th2.Text);
                Global.hv_myThreashod[2] = int.Parse(tb_th3.Text);
                Global.hv_myThreashod[3] = int.Parse(tb_th4.Text);
                Global.hv_myThreashod[4] = int.Parse(tb_th5.Text);
                Global.hv_myThreashod[5] = int.Parse(tb_th6.Text);
                Global.hv_myThreashod[6] = int.Parse(tb_th7.Text);
                Global.hv_myThreashod[7] = int.Parse(tb_th8.Text);
                Global.hv_myThreashod[8] = int.Parse(tb_th9.Text);
                Global.hv_myThreashod[9] = int.Parse(tb_th10.Text);
                Global.hv_myThreashod[10] = int.Parse(tb_th11.Text);
                Global.hv_myThreashod[11] = int.Parse(tb_th12.Text);
                Global.hv_myThreashod[12] = int.Parse(tb_th13.Text);
                Global.hv_myThreashod[13] = int.Parse(tb_th14.Text);
                Global.hv_myThreashod[14] = int.Parse(tb_th15.Text);

                Global.hv_channels[0] = int.Parse(tb_ch0.Text);
                Global.hv_channels[1] = int.Parse(tb_ch1.Text);
                Global.hv_channels[2] = int.Parse(tb_ch2.Text);
                Global.hv_channels[3] = int.Parse(tb_ch3.Text);
                Global.hv_channels[4] = int.Parse(tb_ch4.Text);
                Global.hv_channels[5] = int.Parse(tb_ch5.Text);
                Global.hv_channels[6] = int.Parse(tb_ch6.Text);
                Global.hv_channels[7] = int.Parse(tb_ch7.Text);
                Global.hv_channels[8] = int.Parse(tb_ch8.Text);
                Global.hv_channels[9] = int.Parse(tb_ch9.Text);
                Global.hv_channels[10] = int.Parse(tb_ch10.Text);
                Global.hv_channels[11] = int.Parse(tb_ch11.Text);
                Global.hv_channels[12] = int.Parse(tb_ch12.Text);
                Global.hv_channels[13] = int.Parse(tb_ch13.Text);
                Global.hv_channels[14] = int.Parse(tb_ch14.Text);
                Global.hv_channels[15] = int.Parse(tb_ch15.Text);


            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                getTextBox();

                MyConfig.SaveConfig("//" + listBox1.SelectedItem.ToString());

            }
            catch(Exception ee)
            {
                MessageBox.Show("检查是否选择列表位置");
                MessageBox.Show(ee.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            form1.ShowPInfo();
        }

        private void showData()
        {
            listBox1.Items.Clear();

            List<string> datefolders = new List<string>();
            DirectoryInfo dir2 = new DirectoryInfo(Global.ConfigPath);
            DirectoryInfo[] fi2 = dir2.GetDirectories();
            foreach (DirectoryInfo ff in fi2)
            {
                datefolders.Add(ff.Name);
            }
            foreach (string ss in datefolders)
            {
                listBox1.Items.Add(ss);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {               
                MyConfig.LoadConfig("//" + listBox1.SelectedItem.ToString());
                showTextBox();
            }
            catch(Exception eee)
            {
                MessageBox.Show("检查是否选择列表位置");
                MessageBox.Show(eee.ToString());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                DirectoryInfo subDir = new DirectoryInfo(Global.ConfigPath  + listBox1.SelectedItem.ToString());
                subDir.Delete(true);
                Thread.Sleep(200);
                showData();

            }
            catch (Exception eee)
            {
                MessageBox.Show("检查是否选择列表位置");
                MessageBox.Show(eee.ToString());
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                using (WriteName wNamefrm = new WriteName())
                {
                    if (wNamefrm.ShowDialog() == DialogResult.OK)
                    {                       
                        getTextBox();
                        MyConfig.SaveConfig("\\"+wNamefrm.pName);
                        Thread.Sleep(200);
                        showData();
                    }
                }
            }
            catch(Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}
