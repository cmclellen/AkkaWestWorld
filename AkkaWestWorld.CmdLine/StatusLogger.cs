using AkkaWestWorld.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace AkkaWestWorld.CmdLine
{


    public class StatusLogger : IStatusLogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
