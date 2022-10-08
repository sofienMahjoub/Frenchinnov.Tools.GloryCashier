using System;
namespace Frenchinnov.GloryCashier.Events
{
    public class GloryCancelEnableEventArgs : EventArgs
    {
        public GloryCancelEnableEventArgs(bool canCancel)
        {
            this.CanCancel = canCancel; 
        }
        public bool CanCancel { get; private set; }
    }
}
