using System; 
namespace Frenchinnov.GloryCashier.Events
{
    public class GloryAmountEventArgs : EventArgs
    {
        public GloryAmountEventArgs(float amount)
        {
            this.Amount = amount; 
        }
        public float Amount { get; private set; }
    }
}
