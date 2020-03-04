using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using HalconDotNet;

namespace P1_CMMT
{
    class MyConfig
    {
        static IniFile myini = new IniFile(Global.ConfigPath + "\\Config.ini");


        

        //static IniFile myini = new IniFile("\\Config.ini");

        public static void SaveData()
        {
            try
            {
                
                myini.IniWriteValue("Threshold", "mean1", Global.Threshold1.ToString());
                myini.IniWriteValue("Threshold", "mean2", Global.Threshold2.ToString());

                myini.IniWriteValue("OFFSET", "height", Global.Offset.ToString());

            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
                
                LogManager.WriteLog("保存参数错误"+ee.ToString());
            }
        }


        public static void LoadData()
        {
            try
            {
                
                Global.Threshold1 = double.Parse(myini.IniReadValue("Threshold", "mean1"));
                Global.Threshold2 = int.Parse(myini.IniReadValue("Threshold", "mean2"));


                Global.Offset = short.Parse(myini.IniReadValue("OFFSET", "height"));
            }
            catch(Exception ee)
            {
                MessageBox.Show(ee.ToString());

                LogManager.WriteLog("载入参数出错"+ee.ToString());
            }
        }



        public static void SaveConfig(string path=null)
        {
            try
            {
                if(!Directory.Exists(Global.ConfigPath + path))
                {
                    Directory.CreateDirectory(Global.ConfigPath + path);
                }
                myini = new IniFile(Global.ConfigPath + path + "\\Config.ini");
                myini.IniWriteValue("Num", "Box1ForSupplyNum", Global.Box1ForSupplyNum.ToString());
                myini.IniWriteValue("Num", "Box1NeedNum", Global.Box1NeedNum.ToString());
                myini.IniWriteValue("Num", "Box1NeedNum2", Global.Box1NeedNum2.ToString());

                myini.IniWriteValue("Num", "Box2NeedNum", Global.Box2NeedNum.ToString());
                myini.IniWriteValue("Num", "Box3MinNum", Global.Box3MinNum.ToString());

                myini.IniWriteValue("Time", "TimeQ1", Global.TimeQ1.ToString());
                myini.IniWriteValue("Time", "TimeCam", Global.TimeCam.ToString());


                myini.IniWriteValue("TimeGo", "T1", Global.goTime1.ToString());
                myini.IniWriteValue("TimeGo", "T2", Global.goTime2.ToString());
                myini.IniWriteValue("TimeGo", "T3", Global.goTime3.ToString());
                myini.IniWriteValue("TimeGo", "T4", Global.goTime4.ToString());


                myini.IniWriteValue("TimeSp", "T1", Global.spTime1.ToString());
                myini.IniWriteValue("TimeSp", "T2", Global.spTime2.ToString());
                myini.IniWriteValue("TimeSp", "T3", Global.spTime3.ToString());

                HOperatorSet.WriteTuple(Global.hv_channels, Global.ConfigPath + path + "\\ch.tup");
                HOperatorSet.WriteTuple(Global.hv_myLength, Global.ConfigPath + path + "\\lh.tup");
                HOperatorSet.WriteTuple(Global.hv_myThreashod, Global.ConfigPath + path + "\\th.tup");


                MessageBox.Show("保存成功");
            }
            catch (Exception eee)
            {
                MessageBox.Show(eee.ToString());
            }

        }

        public static void LoadConfig(string path=null)
        {
            try
            {
                myini = new IniFile(Global.ConfigPath + path + "\\Config.ini");
                Global.Box1ForSupplyNum = int.Parse(myini.IniReadValue("Num", "Box1ForSupplyNum"));
                Global.Box1NeedNum = int.Parse(myini.IniReadValue("Num", "Box1NeedNum"));
                Global.Box2NeedNum = int.Parse(myini.IniReadValue("Num", "Box2NeedNum"));
                Global.Box3MinNum = int.Parse(myini.IniReadValue("Num", "Box3MinNum"));
                Global.Box1NeedNum2 = int.Parse(myini.IniReadValue("Num", "Box1NeedNum2"));

                Global.TimeQ1 = int.Parse(myini.IniReadValue("Time", "TimeQ1"));
                Global.TimeCam= int.Parse(myini.IniReadValue("Time", "TimeCam"));

                Global.goTime1 = int.Parse(myini.IniReadValue("TimeGo", "T1"));
                Global.goTime2 = int.Parse(myini.IniReadValue("TimeGo", "T2"));
                Global.goTime3 = int.Parse(myini.IniReadValue("TimeGo", "T3"));
                Global.goTime4 = int.Parse(myini.IniReadValue("TimeGo", "T4"));

                Global.spTime1 = int.Parse(myini.IniReadValue("TimeSp", "T1"));
                Global.spTime2 = int.Parse(myini.IniReadValue("TimeSp", "T2"));
                Global.spTime3 = int.Parse(myini.IniReadValue("TimeSp", "T3"));

                HOperatorSet.ReadTuple(Global.ConfigPath + path + "\\ch.tup", out Global.hv_channels);
                HOperatorSet.ReadTuple(Global.ConfigPath + path + "\\lh.tup", out Global.hv_myLength);
                HOperatorSet.ReadTuple(Global.ConfigPath + path + "\\th.tup", out Global.hv_myThreashod);

            }
            catch(Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }


    }
           
}
