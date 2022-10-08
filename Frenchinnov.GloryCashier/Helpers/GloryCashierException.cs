using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frenchinnov.GloryCashier.Helpers
{
    [Serializable]
    public class GloryCashierException : Exception
    {
        public GloryCashierException() { }

        public GloryCashierException(string exception)
            : base(String.Format("GloryCashierException : {0}", exception))
        {

        }
    }

}
