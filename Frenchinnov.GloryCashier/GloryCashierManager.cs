using Frenchinnov.GloryCashier.Helpers;
using Frenchinnov.GloryCashier.Events;
using System; 
using System.Windows;

namespace Frenchinnov.GloryCashier
{
    public sealed class GloryCashierManager
    {
        private static readonly Lazy<GloryCashierManager> lazy = new Lazy<GloryCashierManager>(() => new GloryCashierManager());
        public static GloryCashierManager Instance { get { return lazy.Value; } }
        private GloryCashierManager() { }

        // Class for communicating with FCC 
        public FccInterfaceLib.FccOperator fccOperator;

        // Sequence number of command request
        int seqNumber;

        private string GetId()
        {
            return DateTime.Now.ToString();
        }

        private String GetSequenceNumber()
        {
            String seqnum = DateTime.Today.ToString("yyyyMMdd");
            seqnum += seqNumber.ToString("000#");
            return seqnum;
        }

        //'
        //' Get Status
        //'
        async void GetStatus()
        {
            var request = new FccInterfaceLib.StatusRequest();

            request.Id = GetId();
            request.SeqNo = GetSequenceNumber();
            request.Option = FccInterfaceLib.FccConst.Option.TYPE_OFF;

            var response = await fccOperator.GetStatusAsync(request);
            GetStatusCompletedEventHandler(response);
        }

        public GloryCashierMessage Init()
        {
            GloryCashierMessage gloryCashierMessage = new GloryCashierMessage();
            try
            {
                InitCallBack();

                // Create FCC Operator
                fccOperator = new FccInterfaceLib.FccOperator(Properties.Settings.Default.FCCService);

                // Set command timeout(Change:600sec, Other:60sec)
                fccOperator.SetChangeTimeout(600000);
                fccOperator.SetTimeout(60000);

                // Listen Event
                FccInterfaceLib.FccEventControler.Instance.Listen(Properties.Settings.Default.AcceptPort);

                // Register Event Request
                var request = new FccInterfaceLib.RegisterEventRequest();
                request.Id = GetId();
                request.SeqNo = GetSequenceNumber();
                request.Url = Properties.Settings.Default.DestinationAddress;
                request.Port = Properties.Settings.Default.AcceptPort;
                fccOperator.RegisterEvent(request);

                // Get FCC Status when initializing
                GetStatus();

                gloryCashierMessage.State = GloryCashierStateEnum.OK;
            }
            catch (Exception ex)
            {
                gloryCashierMessage.State = GloryCashierStateEnum.KO;
                gloryCashierMessage.Message = ex.Message;
                throw new GloryCashierException(ex.Message.ToString());
            }
            return gloryCashierMessage;
        }


        private void InitCallBack()
        {    // StatusChangeEventHandler
            FccInterfaceLib.FccEventControler.Instance.Received_StatusChangeEvent +=
                new Action<FccInterfaceLib.StatusChangeEvent>((ev) => { StatusChangeEventHandler(ev); });

            // GlyCashierEventHandlers
            FccInterfaceLib.FccEventControler.Instance.Received_eventWaitForRemoving +=
                new Action<int, FccInterfaceLib.eventWaitForRemoving>((devid, ev) => { eventWaitForRemovingHandler(ev, devid); });
            FccInterfaceLib.FccEventControler.Instance.Received_eventRemoved +=
                new Action<int, FccInterfaceLib.eventRemoved>((devid, ev) => { eventRemovedHandler(ev, devid); });
            FccInterfaceLib.FccEventControler.Instance.Received_eventStatusChange +=
                new Action<int, FccInterfaceLib.eventStatusChange>((devid, ev) => { eventStatusChangeHandler(ev, devid); });
            FccInterfaceLib.FccEventControler.Instance.Received_eventEmpty +=
                new Action<int, FccInterfaceLib.eventEmpty>((devid, ev) => { eventEmptyHandler(ev, devid); });
            FccInterfaceLib.FccEventControler.Instance.Received_eventLow +=
                new Action<int, FccInterfaceLib.eventLow>((devid, ev) => { eventLowHandler(ev, devid); });
            FccInterfaceLib.FccEventControler.Instance.Received_eventExist +=
                new Action<int, FccInterfaceLib.eventExist>((devid, ev) => { eventExistHandler(ev, devid); });
            FccInterfaceLib.FccEventControler.Instance.Received_eventHigh +=
                new Action<int, FccInterfaceLib.eventHigh>((devid, ev) => { eventHighHandler(ev, devid); });
            FccInterfaceLib.FccEventControler.Instance.Received_eventFull +=
                new Action<int, FccInterfaceLib.eventFull>((devid, ev) => { eventFullHandler(ev, devid); });
            FccInterfaceLib.FccEventControler.Instance.Received_eventMissing +=
                new Action<int, FccInterfaceLib.eventMissing>((devid, ev) => { eventMissingHandler(ev, devid); });
            FccInterfaceLib.FccEventControler.Instance.Received_eventDepositCountChange +=
                new Action<int, FccInterfaceLib.eventDepositCountChange>((devid, ev) => { eventDepositCountChangeHandler(ev, devid); });
            FccInterfaceLib.FccEventControler.Instance.Received_eventError +=
                         new Action<int, FccInterfaceLib.eventError>((devid, ev) => { eventErrorHandler(ev, devid); });

            // ResponseEventHandler
            FccInterfaceLib.FccEventControler.Instance.Received_StatusResponse +=
                new Action<FccInterfaceLib.StatusResponse>((ev) => { GetStatusCompletedEventHandler(ev); });
        }

        //'
        //' Get Status - Complete Event
        //'
        async void GetStatusCompletedEventHandler(FccInterfaceLib.StatusResponse response)
        {
            if (response.result < 0)
            {
                MessageBox.Show("GetStatusCompletedEventHandler Error.\nMsg=" + response.ErrorMessage);
            }

            if (response.result == FccInterfaceLib.FccConst.ResultCode.SUCCESS)
            {
                // SetGuidance("Connected");

                // FCC Ready
                if (response.Status.Code == FccInterfaceLib.FccConst.FccStatus.IDLE)
                {
                    //SetGuidance("Ready");
                    //UpdateInventory();
                }
                // FCC Initializing or Resetting
                else if ((response.Status.Code == FccInterfaceLib.FccConst.FccStatus.INITIALIZING) ||
                         (response.Status.Code == FccInterfaceLib.FccConst.FccStatus.RESETTING))
                {
                    System.Threading.Thread.Sleep(1000);
                    GetStatus();
                }
                // FCC Error or Processing
                else
                {
                    var reque = new FccInterfaceLib.ResetRequest();
                    reque.Id = "Initial Reset";
                    reque.SeqNo = GetSequenceNumber();

                    System.Threading.Thread.Sleep(1000);
                    var resetResponse = await fccOperator.ResetAsync(reque);
                    ResetOperationCompletedEventHandler(resetResponse);

                    GetStatus();
                }
            }
            else
            {
                System.Threading.Thread.Sleep(1000);
                GetStatus();
            }
        }
         
        //'
        //' Reset - Complete Event
        //'
        public void ResetOperationCompletedEventHandler(FccInterfaceLib.ResetResponse response)
        {
            if (response.result < 0)
            {
                MessageBox.Show("ResetOperationCompletedEventHandler Error.\nMsg=" + response.ErrorMessage);
            }

            if (response.result != FccInterfaceLib.FccConst.ResultCode.SUCCESS)
            {
                MessageBox.Show("Reset could not be achieved.");
            }

            //ClearInput();
            //resetToolStripMenuItem.Enabled = true;
        }

        //'
        //' StatusChange - FCCEvent
        //'
        void StatusChangeEventHandler(FccInterfaceLib.StatusChangeEvent ev)
        {
            if (ev == null)
            {
                MessageBox.Show("StatusChangeEventHandler Timeout.");
            }

            //Get node object of each element
            String convUser = ev.User;

            //Check if the fcc is under change operation
            if ((ev.Status == FccInterfaceLib.FccConst.FccStatus.WAITING_INSERTION_OF_CASH) ||
                (ev.Status == FccInterfaceLib.FccConst.FccStatus.COUNTING))
            {
                //if doing change operation, then the amount should store deposit amount.
                double dblCashin = (double)ev.Amount / 100;
                SetDeposit(new GloryAmountEventArgs((float)dblCashin));
            }

            //SetGuidance(GetStatusString(ev.Status));
        }

        //'
        //' eventWaitForRemoving - GlyCashierEvent
        //'
        void eventWaitForRemovingHandler(FccInterfaceLib.eventWaitForRemoving ev, int devid = 0)
        {
            //Wait removeing notes from RBW
            SetGloryMessage(new GloryMessageEventArgs("Merci de récupérer le reste de votre argent de la machine"));
            SetGloryCancelEnabled(new GloryCancelEnableEventArgs(false));
        }

        //'
        //' eventRemoved - GlyCashierEvent
        //'
        void eventRemovedHandler(FccInterfaceLib.eventRemoved ev, int devid = 0)
        {
            //Note was removed from RBW
            ChangeOperationCompleted(new EventArgs());
        }

        //'
        //' eventStatusChange - GlyCashierEvent
        //'
        void eventStatusChangeHandler(FccInterfaceLib.eventStatusChange ev, int devid = 0)
        {
            //Status Change
            //string strIDName = GetDeviceStatusIDString(ev.DeviceStatusID);
            //SetStatus(devid, "StatusChange " + strIDName);
        }

        //'
        //' eventEmpty - GlyCashierEvent
        //'
        void eventEmptyHandler(FccInterfaceLib.eventEmpty ev, int devid = 0)
        {
            //Empty
            //string strIDName = GetDevicePositionIDString(ev.DevicePositionID);
            //SetStatus(devid, "Empty " + strIDName);
        }

        //'
        //' eventLow - GlyCashierEvent
        //'
        void eventLowHandler(FccInterfaceLib.eventLow ev, int devid = 0)
        {
            //Near Empty
            //string strIDName = GetDevicePositionIDString(ev.DevicePositionID);
            //SetStatus(devid, "Near Empty " + strIDName);
        }

        //'
        //' eventExist - GlyCashierEvent
        //'
        void eventExistHandler(FccInterfaceLib.eventExist ev, int devid = 0)
        {
            //Exist 
            //string strIDName = GetDevicePositionIDString(ev.DevicePositionID);
            //SetStatus(devid, "Exist " + strIDName);
        }

        //'
        //' eventHigh - GlyCashierEvent
        //'
        void eventHighHandler(FccInterfaceLib.eventHigh ev, int devid = 0)
        {
            //Near Full
            //string strIDName = GetDevicePositionIDString(ev.DevicePositionID);
            //SetStatus(devid, "High " + strIDName);
        }

        //'
        //' eventFull - GlyCashierEvent
        //'
        void eventFullHandler(FccInterfaceLib.eventFull ev, int devid = 0)
        {
            //Full
            //string strIDName = GetDevicePositionIDString(ev.DevicePositionID);
            //SetStatus(devid, "Full " + strIDName);
        }

        //'
        //' eventMissing - GlyCashierEvent
        //'
        void eventMissingHandler(FccInterfaceLib.eventMissing ev, int devid = 0)
        {
            //Missing unit.
            //string strIDName = GetDevicePositionIDString(ev.DevicePositionID);
            //SetStatus(devid, "Missing " + strIDName);
        }

        //'
        //' eventDepositCountChange - GlyCashierEvent
        //'
        void eventDepositCountChangeHandler(FccInterfaceLib.eventDepositCountChange ev, int devid = 0)
        {
        }

        //'
        //' eventError - GlyCashierEvent
        //'
        void eventErrorHandler(FccInterfaceLib.eventError ev, int devid = 0)
        {
            //SetStatus(devid, "Error " + ev.ErrorCode);
            String errCode = Int32.Parse(ev.ErrorCode).ToString("X");
            int i;
            for (i = 1; i <= 4 - errCode.Length; i++)
                errCode = "0" + errCode;

            if (!String.IsNullOrEmpty(ev.RecoveryURL))
            {
                ShowRecoveryScreen(errCode, ev.RecoveryURL);
            }
        }

        public void ShowRecoveryScreen(string text, string text2)
        {
            //if (frmErrorRecovery.WebBrowser1.InvokeRequired)
            //{
            //    ShowRecoveryScreenCallback d = new ShowRecoveryScreenCallback(ShowRecoveryScreen);
            //    this.Invoke(d, new Object[] { text, text2 });
            //}
            //else
            //{
            //    printer.PrintJournal("Error Code:                    " + text);
            //    if (btnBeginDeposit.Enabled == false)
            //    {
            //        printer.PrintJournal(printer.Center("Abort Transaction"));
            //        printer.PrintJournal("===================================");
            //        ClearInput();
            //    }
            //    btnBeginDeposit.Enabled = false;
            //    frmErrorRecovery.lblErrorCode.Text = text;
            //    frmErrorRecovery.WebBrowser1.Navigate(new Uri(text2));
            //    frmErrorRecovery.Show();
            //}
        }

        //'
        //' Start Cashin
        //'
        private async void StartCashin()
        {
            var request = new FccInterfaceLib.StartCashinRequest();

            request.Id = GetId();
            request.SeqNo = GetSequenceNumber(); 
            var response = await fccOperator.StartCashinAsync(request);
            StartCashinOperationCompletedEventHandler(response);
        }

        //'
        //' Start Cashin - Complete Event
        //'
        void StartCashinOperationCompletedEventHandler(FccInterfaceLib.StartCashinResponse response)
        {
            if (response.result < 0)
            {
                MessageBox.Show("StartCashinOperationCompletedEventHandler Error.\nMsg=" + response.ErrorMessage);
            }

            if (response.result != FccInterfaceLib.FccConst.ResultCode.SUCCESS)
            {
                MessageBox.Show("Start Cashin failed.");
            }
            else
            {
            }
        }

        //'
        //' Change Operation
        //'
        private async void StartChangeAsync(float salesTotal)
        {
            var request = new FccInterfaceLib.ChangeRequest();

            request.Id = GetId();
            request.SeqNo = GetSequenceNumber();
            request.Amount = Convert.ToInt32(salesTotal * 100);
            SetSalesTotal(new GloryAmountEventArgs(salesTotal));
            var response = await fccOperator.ChangeAsync(request);
            ChangeOperationCompletedEventHandler(response);
        }

        //'
        //' Change Operation - Complete Event
        //'
        void ChangeOperationCompletedEventHandler(FccInterfaceLib.ChangeResponse response)
        {
            if (response.result < 0)
            {
                MessageBox.Show("ChangeOperationCompletedEventHandler Error.\nMsg=" + response.ErrorMessage);
            }

            if (response.result == FccInterfaceLib.FccConst.ResultCode.SUCCESS)
            {
                ChangeOperationCompleted(new EventArgs());
            }
            else if ((response.result == FccInterfaceLib.FccConst.ResultCode.CANCEL))
            {
                //printer.PrintCancelCashin(response.Cash);
                //ClearInput();
                //UpdateInventory();
            }
            else if ((response.result == FccInterfaceLib.FccConst.ResultCode.SHORTAGE_CASH) ||
                     (response.result == FccInterfaceLib.FccConst.ResultCode.DISCORD_CASH) ||
                     (response.result == FccInterfaceLib.FccConst.ResultCode.ERROR_DLL))
            {
                string insertedDenom = "";
                long insertedAmount = 0;
                string dispensedDenom = "";
                long dispensedAmount = 0;
                long changeAmount;

                // Invenotry insufficient
                //insertedDenom = getDetailDenom(response.Cash[0].Denomination, ref insertedAmount, response.ManualDeposit);
                //dispensedDenom = getDetailDenom(response.Cash[1].Denomination, ref dispensedAmount, 0);
                changeAmount = insertedAmount - response.Amount - dispensedAmount;
                if (changeAmount > 0)
                {
                    MessageBox.Show("Cash Changer could not pay changes because of insufficient inventory.\n" +
                                    "Inserted Amount is " + insertedDenom + "\n" +
                                    "Dispensed Amount is " + dispensedDenom + "\n" +
                                    "Take change " + String.Format("{0:f2}", (double)((double)changeAmount / 100)) + " at the customer desk.");
                }

                //printer.PrintFooter(false, changeAmount);
                //ClearInput();
                //UpdateInventory();
            }
            else
            {
                MessageBox.Show("Change Operation failed. " + response.result.ToString());
                // btnReturnDeposit.Enabled = false;
            }
             
        }

        //'
        //' Cash In Cancel - Complete Event
        //'
        void CashinCancelOperationCompletedEventHandler(FccInterfaceLib.CashinCancelResponse response)
        {
            if (response.result < 0)
            {
                MessageBox.Show("CashinCancelOperationCompletedEventHandler Error.\nMsg=" + response.ErrorMessage);
            }

            if (response.result == FccInterfaceLib.FccConst.ResultCode.EXCLUCIVE_ERROR)
            {
                MessageBox.Show("Cancel Cashin excluded.");
                return;
            }
            else if ((response.result == FccInterfaceLib.FccConst.ResultCode.SUCCESS) ||
                     (response.result == FccInterfaceLib.FccConst.ResultCode.CANCEL_SHORTAGE_CASH))
            {
                //printer.PrintCancelCashin(response.Cash);
                //UpdateInventory();
            }
            else
            {
                MessageBox.Show("Cancel Cashin failed.");
            }

            // ClearInput();
        }


        //'
        //' Change Cancel
        //'
        private async void CancelChangeOperation()
        {
            var request = new FccInterfaceLib.ChangeCancelRequest();

            request.Id = GetId();
            request.SeqNo = GetSequenceNumber();

            var response = await fccOperator.ChangeCancelAsync(request);
            ChangeCancelOperationCompletedEventHandler(response);
        }

        //'
        //' ChangeCancel - Complete Event
        //'
        private void ChangeCancelOperationCompletedEventHandler(FccInterfaceLib.ChangeCancelResponse response)
        {
            if (response.result < 0)
            {
                MessageBox.Show("ChangeCancelOperationCompletedEventHandler Error.\nMsg=" + response.ErrorMessage);
            }

            if (response.result != FccInterfaceLib.FccConst.ResultCode.SUCCESS)
            {
                MessageBox.Show("Change Cancel could not be achieved.");
            }
            // MessageBox.Show("Prenez votre argent de la machine");
            //gloryCashierWindow.Close();
        }

        #region Events

        private event EventHandler ChangeOperationCompletedEvent;
        private void ChangeOperationCompleted(EventArgs e)
        {
            ChangeOperationCompletedEvent?.Invoke(this, e);
        }

        private event EventHandler<GloryAmountEventArgs> SetDepositEvent;
        private void SetDeposit(GloryAmountEventArgs e)
        {
            SetDepositEvent?.Invoke(this, e);
        }

        private event EventHandler<GloryAmountEventArgs> SetSalesTotalEvent;
        private void SetSalesTotal(GloryAmountEventArgs e)
        {
            SetSalesTotalEvent?.Invoke(this, e);
        }

        private event EventHandler<GloryMessageEventArgs> SetGloryMessageEvent;
        private void SetGloryMessage(GloryMessageEventArgs e)
        {
            SetGloryMessageEvent?.Invoke(this, e);
        }

        private event EventHandler<GloryCancelEnableEventArgs> SetGloryCancelEnabledEvent;
        private void SetGloryCancelEnabled(GloryCancelEnableEventArgs e)
        {
            SetGloryCancelEnabledEvent?.Invoke(this, e);
        }

        #endregion

        public GloryCashierMessage ShowDialog(float salesTotal)
        {

            GloryCashierMessage gloryCashierMessage = new GloryCashierMessage();
            try
            {
                GloryCashier gloryCashierWindow = new GloryCashier();

                ChangeOperationCompletedEvent += (s, e) => { gloryCashierWindow.Close(); };
                SetDepositEvent += (s, e) => { gloryCashierWindow.DepositTotal = e.Amount; };
                SetSalesTotalEvent += (s, e) => { gloryCashierWindow.SalesTotal = e.Amount; };
                SetGloryMessageEvent += (s, e) => { gloryCashierWindow.GloryMessage = e.GloryMessage; };

                StartCashin();
                StartChangeAsync(salesTotal);

                SetGloryCancelEnabledEvent += (s, e) => { gloryCashierWindow.CancelEnabled = e.CanCancel; };
                gloryCashierWindow.CancelCmdEvent += (s, e) =>
                {
                    CancelChangeOperation();
                    gloryCashierMessage.State = GloryCashierStateEnum.CANCELLED;
                    //afficher message pour prendre l'argent si annuler cmd
                    // gloryCashierWindow.Close();
                };
                gloryCashierWindow.ShowDialog();

                if (gloryCashierMessage.State != GloryCashierStateEnum.CANCELLED)
                    gloryCashierMessage.State = GloryCashierStateEnum.OK;
            }
            catch (Exception ex)
            {
                // GloryCashierService.ResetOperation();

                gloryCashierMessage.State = GloryCashierStateEnum.KO;
                gloryCashierMessage.Message = ex.Message;
                throw new GloryCashierException(ex.Message.ToString());
            }
            return gloryCashierMessage;
        }
    }
}
