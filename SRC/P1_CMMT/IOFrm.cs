using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P1_CMMT
{
    public partial class IOFrm : Form
    {

        MainForm form1;

        private PictureBox[] pic_input = new PictureBox[32];
        private CheckBox[] ck_output = new CheckBox[32];
        public IOFrm()
        {
            InitializeComponent();
        }

        public IOFrm(MainForm form1)
        {
            InitializeComponent();
            this.form1 = form1;
            myInit();
        }

        private void myInit()
        {
            #region picbox ckbox赋值
            pic_input[0] = pictureBox1;
            pic_input[1] = pictureBox2;
            pic_input[2] = pictureBox3;
            pic_input[3] = pictureBox4;
            pic_input[4] = pictureBox5;
            pic_input[5] = pictureBox6;
            pic_input[6] = pictureBox7;
            pic_input[7] = pictureBox8;
            pic_input[8] = pictureBox9;
            pic_input[9] = pictureBox10;
            pic_input[10] = pictureBox11;
            pic_input[11] = pictureBox12;
            pic_input[12] = pictureBox13;
            pic_input[13] = pictureBox14;
            pic_input[14] = pictureBox15;
            pic_input[15] = pictureBox16;


            pic_input[16] = pictureBox32;
            pic_input[17] = pictureBox31;
            pic_input[18] = pictureBox30;
            pic_input[19] = pictureBox29;
            pic_input[20] = pictureBox28;
            pic_input[21] = pictureBox27;
            pic_input[22] = pictureBox26;
            pic_input[23] = pictureBox25;
            pic_input[24] = pictureBox24;
            pic_input[25] = pictureBox23;
            pic_input[26] = pictureBox22;
            pic_input[27] = pictureBox21;
            pic_input[28] = pictureBox20;
            pic_input[29] = pictureBox19;
            pic_input[30] = pictureBox18;
            pic_input[31] = pictureBox17;



            ck_output[0] = checkBox1;
            ck_output[1] = checkBox2;
            ck_output[2] = checkBox3;
            ck_output[3] = checkBox4;
            ck_output[4] = checkBox5;
            ck_output[5] = checkBox6;
            ck_output[6] = checkBox7;
            ck_output[7] = checkBox8;
            ck_output[8] = checkBox9;
            ck_output[9] = checkBox10;
            ck_output[10] = checkBox11;
            ck_output[11] = checkBox12;
            ck_output[12] = checkBox13;
            ck_output[13] = checkBox14;
            ck_output[14] = checkBox15;
            ck_output[15] = checkBox16;

            ck_output[16] = checkBox32;
            ck_output[17] = checkBox31;
            ck_output[18] = checkBox30;
            ck_output[19] = checkBox29;
            ck_output[20] = checkBox28;
            ck_output[21] = checkBox27;
            ck_output[22] = checkBox26;
            ck_output[23] = checkBox25;
            ck_output[24] = checkBox24;
            ck_output[25] = checkBox23;
            ck_output[26] = checkBox22;
            ck_output[27] = checkBox21;
            ck_output[28] = checkBox20;
            ck_output[29] = checkBox19;
            ck_output[30] = checkBox18;
            ck_output[31] = checkBox17;

            form1.GNumEvent += IOShow;

            #endregion
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            try
            {
                CheckBox chk = (CheckBox)sender;
                int index = Convert.ToInt32(chk.Tag);
                if (chk.Checked)
                {
                    //IO on方法
                    //Global.outIO[index - 1] = IOState.ON;
                    ecat_io.EC_Set_Digital_Chn_Out(2, (short)index, 1, 0);



                }
                else
                {
                    //IO off方法
                    //Global.outIO[index - 1] = IOState.OFF;
                    ecat_io.EC_Set_Digital_Chn_Out(2, (short)index, 0, 0);
                }
            }
            catch (Exception eee)
            {
                MessageBox.Show(eee.ToString());
            }
        }


        private void IOShow()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new mydelegate(IOShow), new object[] { });
                return;
            }

            for (int i = 0; i < 32; i++)
            {
                try
                {
                    if (Global.inputIO[i] == IOState.ON)
                    {
                        pic_input[i].BackColor = Color.Lime;
                    }
                    else
                    {
                        pic_input[i].BackColor = Color.FromArgb(128, 128, 255);
                    }

                    if (Global.outIO[i] == IOState.ON)
                    {
                        ck_output[i].BackColor = Color.Lime;
                        ck_output[i].Checked = true;
                    }
                    else
                    {
                        ck_output[i].BackColor = Color.FromArgb(128, 128, 255);
                        ck_output[i].Checked = false;
                    }

                }
                catch
                {
                }
            }

        }



    }
}
