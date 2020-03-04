using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P1_CMMT
{
    class Common
    {
    }

    enum Value
    {
        U1,
        U2,
        U3,
        U4,
        U5,
        U6,
        U7
    }

    enum ReadState
    {
        None,             //初始状态
        Read,             //读表状态
        Clear             //清零状态
    }

    enum ImageState
    {
        First,
        Second,
        None
    }


    enum Result
    {
        None,
        OK,
        NG
    }

    public enum IOState
    {
        ON,
        OFF
    }

    public delegate void mydelegate();


    enum MachineState
    {
        Free,
        Working
    }

    enum WorkState
    {
        Free,
        Count,
        Supply
    }

    enum BoxState
    {
        Empty,
        Half,
        Full
    }

    enum BoxWorkState
    {
        Free,
        Working
    }

}
