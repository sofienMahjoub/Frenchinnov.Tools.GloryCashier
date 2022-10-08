using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frenchinnov.GloryCashier.Helpers
{
    public enum GloryCashierStateEnum
    {
        OK,
        CANCELLED,
        KO
    }
    public class GloryCashierMessage
    { 
        public GloryCashierStateEnum State { get;set; }
        public string Message { get; set; }
    }
}
