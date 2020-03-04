using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace P1_CMMT
{
    public partial class WriteName : Form
    {

        public string pName = "";
        public WriteName()
        {
            InitializeComponent();
        }      

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text.Trim(' ') =="")
                {
                    MessageBox.Show("名称不能为空");
                }
                else
                {
                    pName = textBox1.Text;
                    DialogResult = DialogResult.OK;
                }
            }
            catch(Exception eee)
            {
                MessageBox.Show(eee.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
