using System;
using System.Collections.Generic;
using System.Text;

namespace ChatApp
{
    class Constants
    {
        public static string Id;
        public static string User = "Andries";
        public static Uri SocketServer = new Uri("ws://192.168.56.1:9090/");
        //public static Uri SocketServer = new Uri("wss://socket-io-chat.now.sh/");
        //public static Uri SocketServer = new Uri("ws://chat.rickstoit.nl:9090");
    }
}
