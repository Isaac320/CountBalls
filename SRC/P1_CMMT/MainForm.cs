using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using HalconDotNet;
using System.IO;
using System.Drawing.Imaging;

namespace P1_CMMT
{
    public partial class MainForm : Form
    {
        VideoFrm videofrm;
        SettingFrm settingfrm;
        IOFrm iofrm;

        Thread MStateThread;  //机器状态  工作 或者空闲

        Thread WorkStateThread;  //机器工作状态  补货，计数，空闲

        Thread CheckIOThread;  //监视IO线程 赋值给Global的IO

        Thread CountNumThread;  //计算图片上的小球个数，内部包含关闭相机等操作

        Thread CountNumSupplyThread;  //补给xiaoqiu计数

        Queue<HImage> myHImageQue = new Queue<HImage>();

        ImageProcess imgProcess = new ImageProcess();

        TestMyXXX mytestReg = new TestMyXXX();  //测试是否注册

        SoftReg mySoftReg = new SoftReg();


        Thread box1WorkThread;
        Thread box2WorkThread;

        Thread buttonOutThread;

        LineScan myLS = new LineScan();

        Thread LetBallsGOThread;  //放球线程

        public MainForm()
        {
            InitializeComponent();
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                if (this.ActiveMdiChild == videofrm)
                {
                    Cursor.Current = Cursors.Arrow;
                    return;

                }
                videofrm.MdiParent = this;
                videofrm.Dock = DockStyle.Fill;
                videofrm.Show();
                videofrm.Activate();
                Cursor.Current = Cursors.Arrow;

            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                MyConfig.LoadConfig("xxx");
                SplashFrm.num = 10;

                Global.InitHTuple(); //初始化检查球直径的阈值参数

                Global.MNum = mySoftReg.GetMNum();
                Global.TimeLeft = mytestReg.TimeLeft();
                if (Global.TimeLeft > 100)
                {
                    Global.IsRegister = true;
                }
                else
                {
                    Global.IsRegister = false;
                }

                //MyConfig.LoadData();
                videofrm = new VideoFrm(this);

                SplashFrm.num = 50;
                // Thread.Sleep(1000);

                settingfrm = new SettingFrm(this);

                iofrm = new IOFrm(this);

                SplashFrm.num = 99;

                Thread.Sleep(200);
                SplashFrm.num = 100;
                this.Visible = true;
                videofrm.MdiParent = this;
                videofrm.Dock = DockStyle.Fill;

                videofrm.Show();
                videofrm.Activate();
                this.WindowState = FormWindowState.Maximized;


                //InitIOCard();  //初始化IO卡


                //MyInit();  //初始化各个线程

                //myLS.init(); //初始化相机

                myLS.ImageGrabbed += MyLS_ImageGrabbed;

            }
            catch (Exception eee)
            {
                MessageBox.Show(eee.ToString());
            }
            Opacity = 100;
        }

        private void MyLS_ImageGrabbed(HImage image)
        {
            if(image!=null)
            {
                myHImageQue.Enqueue(image);
            }

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                if (this.ActiveMdiChild == settingfrm)
                {
                    Cursor.Current = Cursors.Arrow;
                    return;

                }
                settingfrm.MdiParent = this;
                settingfrm.Dock = DockStyle.Fill;
                settingfrm.Show();
                settingfrm.Activate();
                Cursor.Current = Cursors.Arrow;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }

        }
        protected override void OnClosing(CancelEventArgs e)
        {
            if (MessageBox.Show("退出本系统?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                myLS.Destory();
                Application.Exit();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                if (this.ActiveMdiChild == iofrm)
                {
                    Cursor.Current = Cursors.Arrow;
                    return;

                }
                iofrm.MdiParent = this;
                iofrm.Dock = DockStyle.Fill;
                iofrm.Show();
                iofrm.Activate();
                Cursor.Current = Cursors.Arrow;

            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }

        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AboutFrm aboutFrm = new AboutFrm())
            {
                aboutFrm.ShowDialog();
            }
        }



        private void MyInit()
        {
            MStateThread = new Thread(ShowMState);
            MStateThread.IsBackground = true;
            MStateThread.Start();

            CheckNumThread = new Thread(CheckNum);
            CheckNumThread.IsBackground = true;
            CheckNumThread.Start();

            CheckIOThread = new Thread(CheckIO);
            CheckIOThread.IsBackground = true;
            CheckIOThread.Start();

            WorkStateThread = new Thread(ShowWorkState);
            WorkStateThread.IsBackground = true;
            WorkStateThread.Start();

            box1WorkThread = new Thread(DoBox1Work);
            box1WorkThread.IsBackground = true;
            box1WorkThread.Start();

            box2WorkThread = new Thread(DoBox2Work);
            box2WorkThread.IsBackground = true;
            box2WorkThread.Start();


            buttonOutThread = new Thread(DoButtonOutWork);
            buttonOutThread.IsBackground = true;
            buttonOutThread.Start();
            

        }

        private void DoButtonOutWork()
        {
            while (true)
            {
                if (Global.isButtonOut && Global.isOK2Out)
                {
                    
                    ValueCtr(Value.U6, true);
                    Thread.Sleep(5000);
                    ValueCtr(Value.U6, false);


                    Global.isOK2Out = false;
                    Global.isButtonOut = false;

                    videofrm.showNothing();

                    Thread.Sleep(2000);
                    Global.Box2Num = 0;
                    Global.box2State = BoxState.Empty;   
                                       
                }
                else
                {
                    Thread.Sleep(200);
                }
            }
        }


        private void ShowMState()
        {
            while (true)
            {
                switch (Global.myMState)
                {
                    case MachineState.Free:
                        toolStripStatusLabel5.Text = "空闲";
                        break;
                    case MachineState.Working:
                        toolStripStatusLabel5.Text = "工作";
                        break;
                    default:
                        toolStripStatusLabel5.Text = "未知";
                        break;
                }
                Thread.Sleep(200);
            }
        }

        private void ShowWorkState()
        {
            while (true)
            {
                switch (Global.myWorkState)
                {
                    case WorkState.Free:
                        toolStripStatusLabel6.Text = "空闲";
                        break;
                    case WorkState.Count:
                        toolStripStatusLabel6.Text = "计数";
                        break;
                    case WorkState.Supply:
                        toolStripStatusLabel6.Text = "补料";
                        break;
                    default:
                        toolStripStatusLabel6.Text = "未知";
                        break;
                }
            }
        }

        /// <summary>
        /// 判断是否需要补给的方法
        /// </summary>
        private void ChangeSupplyState()
        {
            if (Global.myMState == MachineState.Working)
            {
                if (Global.BoxSupplyNum < Global.Box3MinNum)
                {
                    if (Global.myWorkState != WorkState.Supply)
                    {
                        Global.myWorkState = WorkState.Supply;
                    }
                }
                else
                {
                    if (Global.myWorkState != WorkState.Count)
                    {
                        Global.myWorkState = WorkState.Count;
                    }
                }
            }
        }



        #region 检测数据
        public delegate void NumStatusHandler();
        public event NumStatusHandler GNumEvent;

        Thread CheckNumThread;



        private void CheckNum()
        {
            while (true)
            {
                Thread.Sleep(100);
                GNumEvent?.Invoke();                
            }
        }


        #endregion

        private void CheckIO()
        {
            while(true)
            {
                Thread.Sleep(10);
                GetIOOutputStatus();
                GetIOInputStatus();
            }
        }


        private void GetIOOutputStatus()
        {
            //获取IO output的方法
            //将Global.outputIO填满
            for (int i=1;i<33;i++)
            {
                short value = 0;
                ecat_io.EC_Get_Digital_Chn_Out(2, (short)i, ref value, 0);
                if(value==0)
                {
                    Global.outIO[i - 1] = IOState.OFF;
                }
                else
                {
                    Global.outIO[i - 1] = IOState.ON;
                }
            }
            
        }

        private void GetIOInputStatus()
        {
            //int tempInput = my204.ReadInputChannels();
            //for (int i = 0; i < 16; i++)
            //{
            //    if (((tempInput >> i + 8) & 1) == 1)
            //    {
            //        Global.inputIO[i] = IOState.ON;
            //    }
            //    else
            //    {
            //        Global.inputIO[i] = IOState.OFF;
            //    }
            //}

            //这里填写获取IO的函数
            for (int i = 1; i < 33; i++)
            {
                short value = 0;
                ecat_io.EC_Get_Digital_Chn_In(1, (short)i, ref value, 0);
                if (value == 0)
                {
                    Global.inputIO[i-1] = IOState.OFF;
                }
                else
                {
                    Global.inputIO[i - 1] = IOState.ON;
                }
            }




            #region IO处理


            if (Global.inputIO[0] != Global.oldinputIO[0])
            {
                Global.oldinputIO[0] = Global.inputIO[0];
                if (Global.inputIO[0] == IOState.OFF)
                {
                    Thread EmgThread = new Thread(in0ThreadFunction1);
                    EmgThread.IsBackground = true;
                    EmgThread.Start();
                }

                if (Global.inputIO[0] == IOState.ON)
                {
                    Thread EmgThread2 = new Thread(in0ThreadFunction2);
                    EmgThread2.IsBackground = true;
                    EmgThread2.Start();
                }


            }

            if (Global.inputIO[1] != Global.oldinputIO[1])
            {
                Global.oldinputIO[1] = Global.inputIO[1];
                if (Global.inputIO[1] == IOState.ON)
                {
                    Thread StartThread = new Thread(in1ThreadFunction);
                    StartThread.IsBackground = true;
                    StartThread.Start();
                }
            }

            if (Global.inputIO[2] != Global.oldinputIO[2])
            {
                Global.oldinputIO[2] = Global.inputIO[2];
                if (Global.inputIO[2] == IOState.ON)
                {
                    Thread StopThread = new Thread(in2ThreadFunction);
                    StopThread.IsBackground = true;
                    StopThread.Start();
                }
            }

            if (Global.inputIO[3] != Global.oldinputIO[3])
            {
                Global.oldinputIO[3] = Global.inputIO[3];
                if (Global.inputIO[3] == IOState.ON)
                {
                    Thread in3Thread = new Thread(in3ThreadFunction);
                    in3Thread.IsBackground = true;
                    in3Thread.Start();
                }
            }

            if (Global.inputIO[4] != Global.oldinputIO[4])
            {
                Global.oldinputIO[4] = Global.inputIO[4];
                if (Global.inputIO[4] == IOState.ON)
                {
                    Thread in4Thread = new Thread(in4ThreadFunction);
                    in4Thread.IsBackground = true;
                    in4Thread.Start();
                }
            }

            if (Global.inputIO[5] != Global.oldinputIO[5])
            {
                Global.oldinputIO[5] = Global.inputIO[5];
                if (Global.inputIO[5] == IOState.ON)
                {
                    Thread in5Thread = new Thread(in5ThreadFunction);
                    in5Thread.IsBackground = true;
                    in5Thread.Start();
                }
            }

            if (Global.inputIO[6] != Global.oldinputIO[6])
            {
                Global.oldinputIO[6] = Global.inputIO[6];
                if (Global.inputIO[6] == IOState.ON)
                {
                    Thread in6Thread = new Thread(in6ThreadFunction);
                    in6Thread.IsBackground = true;
                    in6Thread.Start();
                }
            }

            if (Global.inputIO[7] != Global.oldinputIO[7])
            {
                Global.oldinputIO[7] = Global.inputIO[7];
                if (Global.inputIO[7] == IOState.ON)
                {
                    Thread in7Thread = new Thread(in7ThreadFunction);
                    in7Thread.IsBackground = true;
                    in7Thread.Start();
                }
            }

            if (Global.inputIO[8] != Global.oldinputIO[8])
            {
                Global.oldinputIO[8] = Global.inputIO[8];
                if (Global.inputIO[8] == IOState.ON)
                {
                    Thread in8Thread = new Thread(in8ThreadFunction);
                    in8Thread.IsBackground = true;
                    in8Thread.Start();
                }
            }

            if (Global.inputIO[9] != Global.oldinputIO[9])
            {
                Global.oldinputIO[9] = Global.inputIO[9];
                if (Global.inputIO[9] == IOState.ON)
                {
                    Thread in9Thread = new Thread(in9ThreadFunction);
                    in9Thread.IsBackground = true;
                    in9Thread.Start();
                }
            }

            if (Global.inputIO[10] != Global.oldinputIO[10])
            {
                Global.oldinputIO[10] = Global.inputIO[10];
                if (Global.inputIO[10] == IOState.ON)
                {
                    Thread in10Thread = new Thread(in10ThreadFunction);
                    in10Thread.IsBackground = true;
                    in10Thread.Start();
                }
            }

            if (Global.inputIO[11] != Global.oldinputIO[11])
            {
                Global.oldinputIO[11] = Global.inputIO[11];
                if (Global.inputIO[11] == IOState.ON)
                {
                    Thread in11Thread = new Thread(in11ThreadFunction);
                    in11Thread.IsBackground = true;
                    in11Thread.Start();
                }
            }

            if (Global.inputIO[12] != Global.oldinputIO[12])
            {
                Global.oldinputIO[12] = Global.inputIO[12];
                if (Global.inputIO[12] == IOState.ON)
                {
                    Thread in12Thread = new Thread(in12ThreadFunction);
                    in12Thread.IsBackground = true;
                    in12Thread.Start();
                }
            }

            if (Global.inputIO[13] != Global.oldinputIO[13])
            {
                Global.oldinputIO[13] = Global.inputIO[13];
                if (Global.inputIO[13] == IOState.ON)
                {
                    Thread in13Thread = new Thread(in13ThreadFunction);
                    in13Thread.IsBackground = true;
                    in13Thread.Start();
                }
            }
            if (Global.inputIO[14] != Global.oldinputIO[14])
            {
                Global.oldinputIO[14] = Global.inputIO[14];
                if (Global.inputIO[14] == IOState.ON)
                {
                    Thread in14Thread = new Thread(in14ThreadFunction);
                    in14Thread.IsBackground = true;
                    in14Thread.Start();
                }
            }

            if (Global.inputIO[15] != Global.oldinputIO[15])
            {
                Global.oldinputIO[15] = Global.inputIO[15];
                if (Global.inputIO[15] == IOState.ON)
                {
                    Thread in15Thread = new Thread(in15ThreadFunction);
                    in15Thread.IsBackground = true;
                    in15Thread.Start();
                }
            }

            #endregion


        }

        #region 一些IO信号触发的线程
        private void in0ThreadFunction1()
        {

        }

        private void in0ThreadFunction2()
        {

        }

        private void in1ThreadFunction()
        {

        }
        private void in2ThreadFunction()
        {

        }

        private void in3ThreadFunction()
        {

        }

        private void in4ThreadFunction()
        {

        }

        private void in5ThreadFunction()
        {

        }

        private void in6ThreadFunction()
        {

        }

        private void in7ThreadFunction()
        {

        }

        private void in8ThreadFunction()
        {

        }

        private void in9ThreadFunction()
        {

        }

        private void in10ThreadFunction()
        {

        }

        private void in11ThreadFunction()
        {

        }

        private void in12ThreadFunction()
        {

        }

        private void in13ThreadFunction()
        {

        }

        private void in14ThreadFunction()
        {
            Global.haveBall = true;
        }

        private void in15ThreadFunction()
        {
            if((!Global.isButtonOut)&&(Global.isOK2Out))
            {
                Global.isButtonOut = true;
            }
        }



        #endregion

        /// <summary>
        /// 总的干活方法
        /// </summary>
        private void DoBox1Work()
        {
            while (true)
            {                
                switch (Global.myMState)
                {
                    case MachineState.Free:                       
                        if (Global.Box1WorkState != BoxWorkState.Free)
                            Global.Box1WorkState = BoxWorkState.Free;
                        //机器空闲状态方法
                        MachineFreeFunc();
                        break;
                    case MachineState.Working:
                        if (Global.Box1WorkState != BoxWorkState.Working)
                            Global.Box1WorkState = BoxWorkState.Working;
                        //机器工作方法
                        if(Global.isFirstRun)
                        {
                            Global.isFirstRun = false;
                            ValueCtr(Value.U5, false);
                            ValueCtr(Value.U6, false);
                        }
                        MachineWorkFunc();
                        break;
                }
            }
            
        }

        /// <summary>
        /// 机器在空闲状态下屁都不干
        /// </summary>
        private void MachineFreeFunc()
        {
            
                Thread.Sleep(300);
            
        }

        /// <summary>
        /// 机器在工作状态下干活
        /// </summary>
        private void MachineWorkFunc()
        {
           
                ChangeSupplyState();   //开始工作先检查box3是否需要补给，就是更新下机器状态
                switch (Global.myWorkState)
                {
                    case WorkState.Count:
                        //做计数的事;
                        DoCountWork();
                        break;
                    case WorkState.Supply:
                        //做补给该干的事;
                        DoSupplyWork();
                        break;
                    case WorkState.Free:
                        //屁事不干
                        Thread.Sleep(200);
                        break;
                    default:
                        //屁事不干
                        Thread.Sleep(200);
                        break;
                }
            
        }

        /// <summary>
        /// 计数工作
        /// </summary>
        private void DoCountWork()
        {
            videofrm.listBoxShowMessage("开始往粗计算框补球...");
            myHImageQue.Clear();

            Global.needCloseU1 = false;
            Global.needStopGO = false;
            

            Thread.Sleep(1000);

            //打开相机抓图grab
            myLS.Grab();

            //开始计数线程，Global.box1num开始累加，累计到一定数量关闭阀门U1，关闭相机，将box1状态设置为满。
            CountNumFuncCountOpenThread();

            //打开闸门Q1放球
            //ValueCtr(Value.U1, true);
            //ValueCtr(Value.U2, true);

            //开始放球 新的放球方式
            
            LetBallsGo();


            //box1状态设置为half
            Global.box1State = BoxState.Half;

            //循环box1状态是否为满
            while (Global.box1State!=BoxState.Full)
            {
                Thread.Sleep(200);
            }

            //循环检查box2状态是否为空。
            while(Global.box2State!=BoxState.Empty)
            {
                Thread.Sleep(200);
            }

            //打开闸门Q2， box2num=box1num，延迟n秒，关闭阀门Q2，将box1状态设为空，将box2状态设为half。
            ValueCtr(Value.U5, true);
            Global.Box2Num = Global.Box1Num;
            Global.box2OKNG = Global.box1OKNG;
            Global.Box1Num = 0;
            Thread.Sleep(Global.TimeQ1);
            ValueCtr(Value.U5, false);
            Global.box1State = BoxState.Empty;
            Global.box2State = BoxState.Half;

        }

        //补给工作
        private void DoSupplyWork()
        {
            videofrm.listBoxShowMessage("开始往给补给区补球...");
            myHImageQue.Clear();

            Global.needCloseU1 = true;
            Global.needStopGO = false;

            //打开补给仓门
            ValueCtr(Value.U3, true);
            Thread.Sleep(1000);

            //打开相机抓图
            myLS.Grab();

            //开始计数线程，box1num累加，累计到补给数量关闭阀门U1，关闭相机，将box1状态设置为满。
            CountNumFuncSupplyOpenThread();

            //打开闸门Q1放球
            //ValueCtr(Value.U1, true);
            //ValueCtr(Value.U2, true);

            LetBallsGo();


            //box1状态设置为half
            Global.box1State = BoxState.Half;

            //循环box1状态是否为满
            while (Global.box1State != BoxState.Full)
            {
                Thread.Sleep(200);
            }

            //打开闸门Q3 bo3num=box3num+box1num ,放入补给仓 延迟n秒，关闭闸门Q3。
            Global.BoxSupplyNum = Global.BoxSupplyNum + Global.Box1Num;
            ValueCtr(Value.U3, false);

            //box1球数量为0 状态为空
            Global.Box1Num = 0;
            Global.box1State = BoxState.Empty;


        }

        /// <summary>
        /// 精计框Box2的一些工作，补球，满了就放球
        /// </summary>
        private void DoBox2Work()
        {
            while(true)
            {
                switch(Global.box2State)
                {
                    case BoxState.Empty:
                        //屁事不干
                        Thread.Sleep(200);
                        break;
                    case BoxState.Full:
                        //盒子满了该干啥干啥
                        Box2FullWork();
                        break;
                    case BoxState.Half:
                        //盒子半满，补球
                        Box2HalfWork();
                        break;
                    default:
                        //屁事不干
                        Thread.Sleep(200);
                        break;
                }
            }
        }

        private void Box2FullWork()
        {
            //打开闸门Q5 延迟n秒，关闭闸门Q5，box2num=0，box2状态设为空。
            Thread.Sleep(200);
            if(!Global.isOK2Out)
            {
                Global.isOK2Out = true;
            }
        }

        private void Box2HalfWork()
        {
            //计算补球数，xxx=box2NeedNum-box2Num.打开闸门Q4，开始补球。直到补球完成。关闭闸门Q4。box2Num=box2NeedNum，box3num=box3num-xxx，box2状态设为Full。
            int xxx = Global.Box2NeedNum - Global.Box2Num;
            supplyBalls(xxx);

            Global.boxLastOKNG = Global.box2OKNG && (Global.Box2Num == Global.Box2NeedNum);

            videofrm.showOKNG();

            Global.box2State = BoxState.Full;
            Thread.Sleep(200);
        }


        private void CountNumFuncCountOpenThread()
        {
            CountNumThread = new Thread(CountNumFuncCount);
            CountNumThread.IsBackground = true;
            CountNumThread.Start();
        }

        private void CountNumFuncSupplyOpenThread()
        {
            CountNumSupplyThread = new Thread(CountNumFuncSupply);
            CountNumSupplyThread.IsBackground = true;
            CountNumSupplyThread.Start();
        }

        private void CountNumFuncCount()
        {
            CountNumFunc(Global.Box1NeedNum,Global.Box1NeedNum2);
        }

        private void CountNumFuncSupply()
        {
            CountNumFunc(Global.Box1ForSupplyNum);
        }

        private void LetBallsGo()
        {
            LetBallsGOThread = new Thread(GoBalls);
            LetBallsGOThread.IsBackground = true;
            LetBallsGOThread.Start();
        }



       // int nnnn = 1;


        Thread freezeCameraThread;

        public void FreezeCam()
        {
            Thread.Sleep(Global.TimeCam);
            //关闭相机
            myLS.Freeze();
            //
            Global.CountNotFullFlag = false;
        }

        public void freezeCamera()
        {
            myLS.Freeze();
        }

        /// <summary>
        /// Global.box1num开始累加，累计到一定数量关闭阀门U1，关闭相机，将box1状态设置为满。
        /// </summary>
        private void CountNumFunc(int needNum)
        {
            Global.CountNotFullFlag = true;
            bool tempFlag = true;
            while (Global.CountNotFullFlag||myHImageQue.Count>1)
            {
                if(Global.Box1Num> needNum&&tempFlag)
                {
                    tempFlag = false;
                    if (Global.CountNotFullFlag)
                    {
                        //关闭阀门
                        //ValueCtr(Value.U1, false);
                        //ValueCtr(Value.U2, false);
                        //Thread.Sleep(Global.TimeCam);
                        ////关闭相机
                        //myLS.Freeze();
                        ////
                        //Global.CountNotFullFlag = false;
                        videofrm.listBoxShowMessage("阀门关闭");
                        Global.needStopGO = true;

                        freezeCameraThread = new Thread(FreezeCam);
                        freezeCameraThread.IsBackground = true;
                        freezeCameraThread.Start();
                       
                    }
                }
                if(myHImageQue.Count>1)
                {
                    try
                    {
                        HImage image1 = myHImageQue.Dequeue();
                        //HOperatorSet.WriteImage(image1, "tiff", 0, "d://" + nnnn.ToString() + ".tif");
                       // nnnn++;
                        HImage image2 = myHImageQue.ElementAt(0);
                        HObject stitchImg = null;
                        HOperatorSet.GenEmptyObj(out stitchImg);
                        stitchImg.Dispose();
                        imgProcess.StitchImg(image1, image2, out stitchImg);
                        HObject xld = null;
                        HOperatorSet.GenEmptyObj(out xld);
                        xld.Dispose();
                        imgProcess.CountNum(stitchImg, out xld,out int tempNum,out HTuple NGLengths,out HTuple NGChannels);
                        Global.Box1Num += tempNum;

                        //图像显示在界面上
                        //
                        //
                        videofrm.ShowImage(stitchImg);
                        videofrm.ShowImage(xld);

                        //是否要释放上面的图像
                        image1.Dispose();
                        stitchImg.Dispose();
                        xld.Dispose();

                    }
                    catch(Exception eee)
                    {
                        MessageBox.Show(eee.ToString());
                    }
                    //测试要不要加一下两句
                    //GC.Collect();
                    //GC.WaitForPendingFinalizers();

                }
                else
                {
                    //没图像在这空转
                    Thread.Sleep(100);
                }
                
            }

            Global.box1State = BoxState.Full;

        }


        private void CountNumFunc(int needNum,int needNum2)
        {
            Global.box1OKNG = true;  //初始默认box1的球都是好的
            Global.CountNotFullFlag = true;
            bool tempFlag = true;
            bool tempFlag2 = true;
            while (Global.CountNotFullFlag || myHImageQue.Count > 1)
            {
                if(Global.Box1Num>needNum2&&tempFlag2)
                {
                    tempFlag2 = false;
                    Global.needCloseU1 = true;
                    videofrm.listBoxShowMessage("启动小阀补球模式");
                }
                if (Global.Box1Num > needNum && tempFlag)
                {
                    tempFlag = false;
                    if (Global.CountNotFullFlag)
                    {
                        //关闭阀门
                        //ValueCtr(Value.U1, false);
                        //ValueCtr(Value.U2, false);
                        //Thread.Sleep(Global.TimeCam);
                        ////关闭相机
                        //myLS.Freeze();
                        ////
                        //Global.CountNotFullFlag = false;
                        videofrm.listBoxShowMessage("阀门关闭");

                        Global.needStopGO = true;

                        freezeCameraThread = new Thread(FreezeCam);
                        freezeCameraThread.IsBackground = true;
                        freezeCameraThread.Start();

                    }
                }
                if (myHImageQue.Count > 1)
                {
                    try
                    {
                        HImage image1 = myHImageQue.Dequeue();
                        //HOperatorSet.WriteImage(image1, "tiff", 0, "d://" + nnnn.ToString() + ".tif");
                        // nnnn++;
                        HImage image2 = myHImageQue.ElementAt(0);
                        HObject stitchImg = null;
                        HOperatorSet.GenEmptyObj(out stitchImg);
                        stitchImg.Dispose();
                        imgProcess.StitchImg(image1, image2, out stitchImg);
                        HObject xld = null;
                        HOperatorSet.GenEmptyObj(out xld);
                        xld.Dispose();

                        
                        bool isBox1OK=imgProcess.CountNum(stitchImg, out xld, out int tempNum,out HTuple NGLengths,out HTuple NGChannels);


                        if (!isBox1OK)
                        {
                            //这里拿到NG小球直径NGLengths 和它所在流道NGChannels   、、、保存到log里吧,还有显示在主界面那个listbox里
                            string NGBallsRs = NGChannels.ToString();
                            string NGBallsChannels = NGChannels.ToString();

                            //保存log  
                            Log_NGBalls.WriteLog(NGBallsRs);
                            Log_NGBalls.WriteLog(NGBallsChannels);


                            //显示在messbox框里
                            videofrm.listBoxShowMessage(NGBallsRs);
                            videofrm.listBoxShowMessage(NGChannels);
                                                   
                        }
                        



                        Global.Box1Num += tempNum;

                        Global.box1OKNG = Global.box1OKNG && isBox1OK;   //每次处理是否好的取并，只要有坏球就是false

                        //图像显示在界面上
                        //
                        //
                        videofrm.ShowImage(stitchImg);
                        videofrm.ShowImage(xld);

                        //是否要释放上面的图像
                        image1.Dispose();
                        stitchImg.Dispose();
                        xld.Dispose();

                    }
                    catch (Exception eee)
                    {
                        MessageBox.Show(eee.ToString());
                    }
                    //测试要不要加一下两句
                    //GC.Collect();
                    //GC.WaitForPendingFinalizers();

                }
                else
                {
                    //没图像在这空转
                    Thread.Sleep(100);
                }

            }

            Global.box1State = BoxState.Full;

        }



        public void InitIOCard()
        {
            string eni = "eni.xml";
            char[] enopath = eni.ToCharArray();
            int ret = ecat_io.EC_Open(0, 0);

            ret = ecat_io.EC_LoadEni(eni, 0);
            ret = ecat_io.EC_ConnectECAT(0, 0);
            if (ret != 0)
            {
                MessageBox.Show("IO卡连接失败");
            }
        }


        Object _lock = new object();
        private void ValueCtr(Value value,bool flag)
        {
            lock (_lock)
            {
                short a = (short)(flag ? 1 : 0);
                switch (value)
                {
                    case Value.U1:
                        ecat_io.EC_Set_Digital_Chn_Out(2, 2, a, 0);
                        break;
                    case Value.U2:
                        ecat_io.EC_Set_Digital_Chn_Out(2, 3, a, 0);
                        break;
                    case Value.U3:
                        ecat_io.EC_Set_Digital_Chn_Out(2, 4, a, 0);
                        break;
                    case Value.U4:
                        ecat_io.EC_Set_Digital_Chn_Out(2, 5, a, 0);
                        break;
                    case Value.U5:
                        ecat_io.EC_Set_Digital_Chn_Out(2, 6, a, 0);
                        break;
                    case Value.U6:
                        ecat_io.EC_Set_Digital_Chn_Out(2, 7, a, 0);
                        break;
                    case Value.U7:
                        ecat_io.EC_Set_Digital_Chn_Out(2, 1, a, 0);
                        break;

                    default:
                        break;

                }
            }
        }


        ///
        private void supplyBalls(int num)
        {
            int tempNum = 0;
            Global.haveBall = false;

            Global.needSupplyBalls = true;

            while (tempNum<num&&Global.needSupplyBalls)
            {
                Global.haveBall = false;
                ValueCtr(Value.U4, true);
                Thread.Sleep(Global.spTime1*10);
                ValueCtr(Value.U4, false);
                Thread.Sleep(Global.spTime2 * 10);
                for (int i=0;i<Global.spTime3;i++)
                {
                    if(Global.haveBall)
                    {
                        tempNum++;
                        Global.Box2Num++;
                        Global.BoxSupplyNum--;
                        Global.haveBall = false;
                        break;
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                }
            }

        }


        public void GoBalls()
        {
            while(true)
            {
                ValueCtr(Value.U7, false);
                for(int i=0;i<Global.goTime1;i++)
                {
                    if (Global.needStopGO)
                        break;
                    Thread.Sleep(10);                    
                }
                if (Global.needStopGO)
                    break;

                if (!Global.needStopGO)
                {
                    if(!Global.needCloseU1)
                    {
                        ValueCtr(Value.U1, true);                        
                    }
                    ValueCtr(Value.U2, true);

                }
                for (int i = 0; i < Global.goTime2; i++)
                {
                    if (Global.needStopGO)
                        break;
                    Thread.Sleep(10);
                }
                if (Global.needStopGO)
                    break;


                if (!Global.needStopGO)
                {
                    ValueCtr(Value.U1, false);
                    ValueCtr(Value.U2, false);
                }
                if (Global.needStopGO)
                    break;


                for (int i = 0; i < Global.goTime3; i++)
                {
                    if (Global.needStopGO)
                        break;
                    Thread.Sleep(10);
                }
                if (Global.needStopGO)
                    break;

                if (!Global.needStopGO)
                {
                    ValueCtr(Value.U7, true);                    
                }
                if (Global.needStopGO)
                    break;

                for (int i = 0; i < Global.goTime4; i++)
                {
                    if (Global.needStopGO)
                        break;
                    Thread.Sleep(10);
                }
                if (Global.needStopGO)
                    break;
            }

            ValueCtr(Value.U1, false);
            ValueCtr(Value.U2, false);
            ValueCtr(Value.U3, false);
        }

        public void ShowPInfo()
        {
            videofrm.ShowInfo();
        }

      
    }
}

