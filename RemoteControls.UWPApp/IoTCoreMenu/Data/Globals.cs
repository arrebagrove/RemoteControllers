using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteControls
{
    public static class Globals
    {
        public static SerialTerminalPage SerialTerminalPage { get; set; } = null;

        public static BluetoothSerialTerminalPage BluetoothSerialTerminalPage { get; set; } = null;

        public static BluetoothSerialTerminalPage2 BluetoothSerialTerminalPage2 { get; set; } = null;

        public static MainPage MP { get; set; } = null;

        public static ListRemoteControlsPage LTP { get; set; } = null;

        public static RemoteControlAPI RemoteControlAPI { get; set; } = null;

        public static string EndBlanks { get; set; } = "           ";
    }
}
