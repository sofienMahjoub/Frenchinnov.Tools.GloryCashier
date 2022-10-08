using System;
namespace Frenchinnov.GloryCashier.Events
{
    public class GloryMessageEventArgs : EventArgs
    {
        public GloryMessageEventArgs(string msg)
        {
            this.GloryMessage = msg; 
        }
        public string GloryMessage { get; private set; }
    }
}
