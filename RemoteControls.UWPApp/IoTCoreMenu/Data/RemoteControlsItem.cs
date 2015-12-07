using System;
using System.Collections.Generic;
using System.Text;
using Windows.Data.Json;

namespace RemoteControls
{
    public static class SerialRecvState
    {
        public enum States
        {
            Start,
            Recving,
            Done
        }
        public static States Current {get; set; } = States.Start;
    }
    //Note: To change the table from the Azure Mobile Service (ie Same service and therefore same app key) ..
    // Just change the class name here using Refactoring
    public class RemoteControl
    {

        public string Id { get; set; }

        public string Unit { get; set; } = "";

        public int IndexX { get; set; } = 0;
        public int IndexY { get; set; } = 0;

        public string Text { get; set; } = "";

        public UInt16 ValueHi { get; set; } = 0;

        public UInt16 ValueLo { get; set; } = 0;

        public int Decode_Type { get; set; } = 0;

        public int Bits { get; set; } = 0;

        public UInt16 Address { get; set; } = 0;

        public string Raw { get; set; } = "";

        public UInt16 Overflow { get; set; } = 0;

        public bool Repeat { get; set; } = false;
    }

    public class Item
    {
        public string Id { get; set; }
        public string Text { get; set; }
    }
}

