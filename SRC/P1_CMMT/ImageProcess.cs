using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HalconDotNet;

namespace P1_CMMT
{
    class ImageProcess
    {

        int[] channels = new int[12] { 0,785, 1525, 2250, 3000, 3700, 4440, 5190, 5900, 6630, 7360 ,8192};
        public void StitchImg(HObject image1,HObject image2, out HObject imageOut)
        {

            //填写拼接图像函数方法

            stitch_image(image1, image2, out imageOut);

        }

        /// <summary>
        /// 小球计数方法
        /// </summary>
        /// <param name="image">输入图像</param>
        /// <param name="xld">输出小球xld</param>
        /// <param name="num">输出小球个数</param>
        /// <param name="NGLengths">输出NG小球直径</param>
        /// <param name="NGChannels">输出NG小球所在流道</param>
        /// <returns></returns>
        public bool CountNum(HObject image,out HObject xld,out int num,out HTuple NGLengths,out HTuple NGChannels)
        {
            //填写图像计数方法
            HTuple myNum = 0;
            HTuple liudao, liudaoR;
            Count_Num(image, out xld, out myNum);
            num = myNum.I;

            getLiuDaoR(xld, out liudao, out liudaoR);

            HTuple myResult = 1;

            //HTuple NGLengths, NGChannels;

            //testOKNG(liudao, Global.hv_channels, liudaoR, Global.hv_myLength, Global.hv_myThreashod, out myResult);

             
            testOKNG2(liudao, Global.hv_channels, liudaoR, Global.hv_myLength, Global.hv_myThreashod, out myResult, out NGLengths, out NGChannels);

            if(myResult==0)
            {
                return false;
            }
            else
            {
                return true;
            }            
            
        }



        public void Count_Num(HObject ho_MixedImage1, out HObject ho_Contours, out HTuple hv_Number)
        {

            // Local iconic variables 
            HObject ho_Region, ho_RegionOpening, ho_ConnectedRegions;
            HObject ho_Rectangle1, ho_Rectangle2, ho_Rectangle3, ho_RegionUnion;
            HObject ho_ConnectedRegions1, ho_DestRegions, ho_RegionDifference;
            HObject ho_RegionUnion1, ho_ConnectedRegions2, ho_DestRegions1;
            HObject ho_RegionUnion2, ho_RegionIntersection, ho_ConnectedRegions3;
            HObject ho_SelectedRegions, ho_RegionUnion3;

            // Local control variables 

            HTuple hv_Area = new HTuple(), hv_Row = new HTuple();
            HTuple hv_Column = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Contours);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_Rectangle1);
            HOperatorSet.GenEmptyObj(out ho_Rectangle2);
            HOperatorSet.GenEmptyObj(out ho_Rectangle3);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions1);
            HOperatorSet.GenEmptyObj(out ho_DestRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion1);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions2);
            HOperatorSet.GenEmptyObj(out ho_DestRegions1);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion2);
            HOperatorSet.GenEmptyObj(out ho_RegionIntersection);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions3);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion3);
            //threshold (MixedImage1, Region, 0, 100)

            ho_Region.Dispose();
            HOperatorSet.FastThreshold(ho_MixedImage1, out ho_Region, 0, 80, 20);
            ho_RegionOpening.Dispose();
            HOperatorSet.OpeningRectangle1(ho_Region, out ho_RegionOpening, 100, 1);

            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(ho_Region, out ho_ConnectedRegions);
            ho_Rectangle1.Dispose();
            HOperatorSet.GenRectangle1(out ho_Rectangle1, 0, 0, 1000, 8192);
            ho_Rectangle2.Dispose();
            HOperatorSet.GenRectangle1(out ho_Rectangle2, 1000, 0, 2000, 8192);
            ho_Rectangle3.Dispose();
            HOperatorSet.GenRectangle1(out ho_Rectangle3, 2000, 0, 3000, 8192);


            ho_RegionUnion.Dispose();
            HOperatorSet.Union2(ho_Rectangle1, ho_Region, out ho_RegionUnion);
            ho_ConnectedRegions1.Dispose();
            HOperatorSet.Connection(ho_RegionUnion, out ho_ConnectedRegions1);
            ho_DestRegions.Dispose();
            HOperatorSet.SelectRegionPoint(ho_ConnectedRegions1, out ho_DestRegions, 500,
                4096);
            ho_RegionDifference.Dispose();
            HOperatorSet.Difference(ho_Rectangle2, ho_DestRegions, out ho_RegionDifference
                );

            ho_RegionUnion1.Dispose();
            HOperatorSet.Union2(ho_Rectangle3, ho_Region, out ho_RegionUnion1);
            ho_ConnectedRegions2.Dispose();
            HOperatorSet.Connection(ho_RegionUnion1, out ho_ConnectedRegions2);
            ho_DestRegions1.Dispose();
            HOperatorSet.SelectRegionPoint(ho_ConnectedRegions2, out ho_DestRegions1, 2500,
                4096);

            ho_RegionUnion2.Dispose();
            HOperatorSet.Union2(ho_DestRegions1, ho_RegionDifference, out ho_RegionUnion2
                );
            ho_RegionIntersection.Dispose();
            HOperatorSet.Intersection(ho_Region, ho_RegionUnion2, out ho_RegionIntersection
                );

            ho_ConnectedRegions3.Dispose();
            HOperatorSet.Connection(ho_RegionIntersection, out ho_ConnectedRegions3);

            ho_SelectedRegions.Dispose();
            HOperatorSet.SelectShape(ho_ConnectedRegions3, out ho_SelectedRegions, "area",
                "and", 1000, 1000000);

            ho_RegionUnion3.Dispose();
            HOperatorSet.Union1(ho_SelectedRegions, out ho_RegionUnion3);

            ho_Contours.Dispose();
            HOperatorSet.GenContourRegionXld(ho_RegionUnion3, out ho_Contours, "border");

            HOperatorSet.CountObj(ho_SelectedRegions, out hv_Number);

            if ((int)(new HTuple(hv_Number.TupleEqual(1))) != 0)
            {
                HOperatorSet.AreaCenter(ho_RegionUnion3, out hv_Area, out hv_Row, out hv_Column);
                if ((int)(new HTuple(hv_Area.TupleEqual(0))) != 0)
                {
                    hv_Number = 0;
                }

            }
            ho_Region.Dispose();
            ho_RegionOpening.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_Rectangle1.Dispose();
            ho_Rectangle2.Dispose();
            ho_Rectangle3.Dispose();
            ho_RegionUnion.Dispose();
            ho_ConnectedRegions1.Dispose();
            ho_DestRegions.Dispose();
            ho_RegionDifference.Dispose();
            ho_RegionUnion1.Dispose();
            ho_ConnectedRegions2.Dispose();
            ho_DestRegions1.Dispose();
            ho_RegionUnion2.Dispose();
            ho_RegionIntersection.Dispose();
            ho_ConnectedRegions3.Dispose();
            ho_SelectedRegions.Dispose();
            ho_RegionUnion3.Dispose();

            return;
        }

        public void stitch_image(HObject ho_Image1Bmp, HObject ho_Image2Bmp, out HObject ho_TiledImage)
        {
            // Local iconic variables 

            HObject ho_ObjectsConcat;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_TiledImage);
            HOperatorSet.GenEmptyObj(out ho_ObjectsConcat);
            ho_ObjectsConcat.Dispose();
            HOperatorSet.ConcatObj(ho_Image1Bmp, ho_Image2Bmp, out ho_ObjectsConcat);
            ho_TiledImage.Dispose();
            HOperatorSet.TileImages(ho_ObjectsConcat, out ho_TiledImage, 1, "vertical");
            ho_ObjectsConcat.Dispose();

            return;
        }


        public void getLiuDaoR(HObject ho_Contours, out HTuple hv_liudao, out HTuple hv_liudaoR)
        {
            // Local iconic variables 

            // Local control variables 

            HTuple hv_Row1 = null, hv_Column1 = null, hv_Row2 = null;
            HTuple hv_Column2 = null;
            // Initialize local and output iconic variables 
            HOperatorSet.SmallestRectangle1Xld(ho_Contours, out hv_Row1, out hv_Column1,
                out hv_Row2, out hv_Column2);
            hv_liudao = (hv_Column2 + hv_Column1) / 2;
            hv_liudaoR = hv_Column2 - hv_Column1;

            return;
        }

        public void testOKNG(HTuple hv_liudao, HTuple hv_channels, HTuple hv_liudaoR,
       HTuple hv_myLength, HTuple hv_myThreashod, out HTuple hv_myResult)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_num = null, hv_num2 = null, hv_Index1 = null;
            HTuple hv_Index2 = new HTuple(), hv_lll = new HTuple();
            HTuple hv_ff1 = new HTuple(), hv_ff2 = new HTuple(), hv_ff = new HTuple();
            // Initialize local and output iconic variables 
            hv_myResult = 1;
            hv_num = new HTuple(hv_liudao.TupleLength());
            hv_num2 = new HTuple(hv_channels.TupleLength());

            HTuple end_val4 = hv_num - 1;
            HTuple step_val4 = 1;
            for (hv_Index1 = 0; hv_Index1.Continue(end_val4, step_val4); hv_Index1 = hv_Index1.TupleAdd(step_val4))
            {
                HTuple end_val5 = hv_num2;
                HTuple step_val5 = 1;
                for (hv_Index2 = 1; hv_Index2.Continue(end_val5, step_val5); hv_Index2 = hv_Index2.TupleAdd(step_val5))
                {
                    if ((int)(new HTuple(((hv_liudao.TupleSelect(hv_Index1))).TupleLess(hv_channels.TupleSelect(
                        hv_Index2)))) != 0)
                    {
                        break;
                    }
                }
                hv_lll = hv_Index2 - 1;
                hv_ff1 = new HTuple(((hv_liudaoR.TupleSelect(hv_Index1))).TupleGreater((hv_myLength.TupleSelect(
                    hv_lll)) - (hv_myThreashod.TupleSelect(hv_lll))));
                hv_ff2 = new HTuple(((hv_liudaoR.TupleSelect(hv_Index1))).TupleLess((hv_myLength.TupleSelect(
                    hv_lll)) + (hv_myThreashod.TupleSelect(hv_lll))));
                hv_ff = hv_ff1.TupleAnd(hv_ff2);
                hv_myResult = hv_myResult.TupleAnd(hv_ff);
            }

            return;
        }


        /// <summary>
        /// 测试小球OKNG的方法
        /// </summary>
        /// <param name="hv_liudao">小球所在位置</param>
        /// <param name="hv_channels">流道信息</param>
        /// <param name="hv_liudaoR">小球直径（像素值）</param>
        /// <param name="hv_myLength">小球标准直径（像素值）</param>
        /// <param name="hv_myThreashod">小球阈值范围</param>
        /// <param name="hv_myResult">是否都是好的</param>
        /// <param name="hv_NGLengths">NG小球直径</param>
        /// <param name="hv_NGChls">NG小球所在流道</param>
        public void testOKNG2(HTuple hv_liudao, HTuple hv_channels, HTuple hv_liudaoR,
      HTuple hv_myLength, HTuple hv_myThreashod, out HTuple hv_myResult, out HTuple hv_NGLengths,
      out HTuple hv_NGChls)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_num = null, hv_num2 = null, hv_Index1 = null;
            HTuple hv_Index2 = new HTuple(), hv_lll = new HTuple();
            HTuple hv_ff1 = new HTuple(), hv_ff2 = new HTuple(), hv_ff = new HTuple();
            // Initialize local and output iconic variables 
            hv_myResult = 1;
            hv_num = new HTuple(hv_liudao.TupleLength());
            hv_num2 = new HTuple(hv_channels.TupleLength());

            hv_NGChls = new HTuple();
            hv_NGLengths = new HTuple();


            HTuple end_val8 = hv_num - 1;
            HTuple step_val8 = 1;
            for (hv_Index1 = 0; hv_Index1.Continue(end_val8, step_val8); hv_Index1 = hv_Index1.TupleAdd(step_val8))
            {
                HTuple end_val9 = hv_num2;
                HTuple step_val9 = 1;
                for (hv_Index2 = 1; hv_Index2.Continue(end_val9, step_val9); hv_Index2 = hv_Index2.TupleAdd(step_val9))
                {
                    if ((int)(new HTuple(((hv_liudao.TupleSelect(hv_Index1))).TupleLess(hv_channels.TupleSelect(
                        hv_Index2)))) != 0)
                    {
                        break;
                    }
                }
                hv_lll = hv_Index2 - 1;
                hv_ff1 = new HTuple(((hv_liudaoR.TupleSelect(hv_Index1))).TupleGreater((hv_myLength.TupleSelect(
                    hv_lll)) - (hv_myThreashod.TupleSelect(hv_lll))));
                hv_ff2 = new HTuple(((hv_liudaoR.TupleSelect(hv_Index1))).TupleLess((hv_myLength.TupleSelect(
                    hv_lll)) + (hv_myThreashod.TupleSelect(hv_lll))));
                hv_ff = hv_ff1.TupleAnd(hv_ff2);
                if ((int)(hv_ff) != 0)
                {
                }
                else
                {
                    hv_NGChls = hv_NGChls.TupleConcat(hv_Index2);
                    hv_NGLengths = hv_NGLengths.TupleConcat(hv_liudaoR.TupleSelect(hv_Index1));
                }
                hv_myResult = hv_myResult.TupleAnd(hv_ff);
            }

            return;

            
        }
    }
}
