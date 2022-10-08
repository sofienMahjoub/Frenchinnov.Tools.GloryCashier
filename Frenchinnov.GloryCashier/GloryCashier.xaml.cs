using Frenchinnov.GloryCashier.Helpers; 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Frenchinnov.GloryCashier
{
    /// <summary>
    /// Logique d'interaction pour GloryCashier.xaml
    /// </summary>
    public partial class GloryCashier : Window, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
         
        #region Constructeur
        public GloryCashier()
        {
            InitializeComponent();
            this.Loaded += GloryCashier_Loaded;
        }
        #endregion

        #region Méthodes
        private void GloryCashier_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;
        }
        private void CancelCmd_Click(object sender, RoutedEventArgs e)
        {
            OnCancelCmd(new EventArgs());
        }
        #endregion

        #region Events
        public event EventHandler CancelCmdEvent;
        public void OnCancelCmd(EventArgs e)
        {
            CancelCmdEvent?.Invoke(this, e);
        }
        #endregion

        #region Proprietes
        private float _salesTotal;
        public float SalesTotal
        {
            get
            {
                return _salesTotal;
            }
            set
            {
                if (value != _salesTotal)
                {
                    _salesTotal = value;
                    NotifyPropertyChanged(nameof(SalesTotal));
                }
            }
        }

        private float _depositTotal;
        public float DepositTotal
        {
            get
            {
                return _depositTotal;
            }
            set
            {
                if (value != _depositTotal)
                {
                    _depositTotal = value;
                    NotifyPropertyChanged(nameof(DepositTotal));
                }
            }
        }

        private bool _cancelEnabled = true;
        public bool CancelEnabled
        {
            get
            {
                return _cancelEnabled;
            }
            set
            {
                if (value != _cancelEnabled)
                {
                    _cancelEnabled = value;
                    NotifyPropertyChanged(nameof(CancelEnabled));
                }
            }
        }

        private string _gloryMessage;
        public string GloryMessage
        {
            get
            {
                return _gloryMessage;
            }
            set
            {
                if (value != _gloryMessage)
                {
                    _gloryMessage = value;
                    NotifyPropertyChanged(nameof(GloryMessage));
                }
            }
        }

        #endregion
    }
}
