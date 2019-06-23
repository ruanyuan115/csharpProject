using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetProject.Infrastructure.Services;
using System.Runtime.InteropServices;

namespace dotnetProject.Services
{
    public class InteropServce : IInteropService
    {
        public string Message()
        {
            return "value";
        }
    }
}
