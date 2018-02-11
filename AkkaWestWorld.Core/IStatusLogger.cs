using System;
using System.Collections.Generic;
using System.Text;

namespace AkkaWestWorld.Core
{
    public interface IStatusLogger
    {
        void Log(string message);
    }
}
