using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace P1_CMMT
{
    class Global
    {
        //public static string[] pName = new string[3] { "匹配度", "均值", "方差" };


        public static Result myResult1 = Result.OK;

        public static Result myResult2 = Result.OK;


        public static HTuple Threshold1 = 0.6;

        public static HTuple Threshold2 = 100;


        public static short Offset = 350;


        public static bool ForceOK1 = false;

        public static bool ForceOK2 = false;

        public static string ImagePath1 = @"D:\Product\Save\";

        public static string NGImagePath = @"D:\Product\NGSave\";


        public static bool isSaveNGImage = true;


        public static string OKImagePath = @"D:\Product\OKSave\";

        public static bool isSaveOKImage = false;


        public static string ConfigPath = @"D:\Product\";


        public static IOState[] inputIO = new IOState[32];
        public static IOState[] outIO = new IOState[32];
        public static IOState[] oldinputIO = new IOState[32];


        public static MachineState myMState = MachineState.Free;

        


        public static WorkState myWorkState = WorkState.Free;
        public static BoxState box1State = BoxState.Empty;
        public static BoxState box2State = BoxState.Empty;
        public static BoxState box3State = BoxState.Empty;


        public static int Box1Num = 0;           //粗计框
        public static int BoxSupplyNum = 0;      //补给框
        public static int Box2Num = 0;           //精计框


        public static int Box1ForSupplyNum = 500;   //box1补给球数，超过关相机
        public static int Box1NeedNum = 1000;  //box1粗计球数，超过关相机
        public static int Box2NeedNum = 1400;  //box2需要的球数
        public static int Box3MinNum = 500;

        public static int Box1NeedNum2 = 800;  //box1第二号参数  用于关闭大阀门

        public static bool CountNotFullFlag = true;  //box1未满flag

        public static int TimeQ1 = 2000;
        public static int TimeCam = 2000;

        public static bool IsRegister = false;
        public static int TimeLeft = 10;

        public static string MNum = "";

        public static int NeedDays = 100;

        public static BoxWorkState Box1WorkState = BoxWorkState.Free;
        public static BoxWorkState Box2WorkState = BoxWorkState.Free;


        public static bool isFirstRun = true;

        public static bool haveBall = false;   //io_in15检测小球

        public static bool isOK2Out = false;

        public static bool isButtonOut = false;

        public static bool needCloseU1 = false;   //判断在放料过程中是否该关闭U1

        public static bool needStopGO = false;    //判断放料是否可以结束  相机当然会延时一段时间后关闭


        public static bool needSupplyBalls = true;  //判断要不要继续补球

        public static int goTime1 = 100;   //U7关----U1U2打开
        public static int goTime2 = 100;   //U1U2打开----U1U2关闭
        public static int goTime3 = 100;   //U1U2关闭---U7打开
        public static int goTime4 = 100;   //U7打开---U7关闭


        public static int spTime1 = 20;    //补球顶上去时间
        public static int spTime2 = 10;    //补球回落时间
        public static int spTime3 = 100;   //补球在下面等球最长时间

        public static string pName = "";   //产品名字 及配置文件及名称

        public static bool box1OKNG=true;
        public static bool box2OKNG = true;

        public static bool boxLastOKNG = true;   //最终判断是否OK

        public static HTuple hv_channels = null;
        public static HTuple hv_myLength = null;
        public static HTuple hv_myThreashod = null;

        public static void InitHTuple()
        {
            hv_channels = new HTuple();
            hv_channels[0] = 0;
            hv_channels[1] = 785;
            hv_channels[2] = 1525;
            hv_channels[3] = 2250;
            hv_channels[4] = 3000;
            hv_channels[5] = 3700;
            hv_channels[6] = 4440;
            hv_channels[7] = 5190;
            hv_channels[8] = 5900;
            hv_channels[9] = 6630;
            hv_channels[10] = 7360;
            hv_channels[11] = 8192;
            hv_channels[12] = 8192;
            hv_channels[13] = 8192;
            hv_channels[14] = 8192;
            hv_channels[15] = 8192;

            hv_myLength = new HTuple();
            hv_myLength[0] = 450;
            hv_myLength[1] = 450;
            hv_myLength[2] = 450;
            hv_myLength[3] = 450;
            hv_myLength[4] = 450;
            hv_myLength[5] = 450;
            hv_myLength[6] = 450;
            hv_myLength[7] = 450;
            hv_myLength[8] = 450;
            hv_myLength[9] = 450;
            hv_myLength[10] = 450;
            hv_myLength[11] = 450;
            hv_myLength[12] = 450;
            hv_myLength[13] = 450;
            hv_myLength[14] = 450;

            hv_myThreashod = new HTuple();
            hv_myThreashod[0] = 200;
            hv_myThreashod[1] = 200;
            hv_myThreashod[2] = 200;
            hv_myThreashod[3] = 200;
            hv_myThreashod[4] = 200;
            hv_myThreashod[5] = 200;
            hv_myThreashod[6] = 200;
            hv_myThreashod[7] = 200;
            hv_myThreashod[8] = 200;
            hv_myThreashod[9] = 200;
            hv_myThreashod[10] = 200;
            hv_myThreashod[11] = 200;
            hv_myThreashod[12] = 200;
            hv_myThreashod[13] = 200;
            hv_myThreashod[14] = 200;

        }




    }
}
