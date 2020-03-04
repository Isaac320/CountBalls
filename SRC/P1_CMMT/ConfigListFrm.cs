using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace P1_CMMT
{
    public partial class ConfigListFrm : Form
    {

        public  string pName;
        public ConfigListFrm()
        {
            InitializeComponent();
        }

        private void showData()
        {

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

        private void ConfigListFrm_Load(object sender, EventArgs e)
        {
            showData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                MyConfig.LoadConfig("//" + listBox1.SelectedItem.ToString());
                pName = listBox1.SelectedItem.ToString();
                DialogResult = DialogResult.OK;
            }
            catch (Exception eee)
            {
                MessageBox.Show("检查是否选择列表位置");
                MessageBox.Show(eee.ToString());
            }
        }
    }
}
