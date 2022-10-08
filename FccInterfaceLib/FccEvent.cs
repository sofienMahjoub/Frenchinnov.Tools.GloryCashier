using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections;
using Newtonsoft.Json;

namespace FccInterfaceLib
{
	public class FccEventControler
	{
		public Action<RegisterEventResponse> Received_RegisterEventResponse { get; set; }
		public Action<UnRegisterEventResponse> Received_UnRegisterEventResponse { get; set; }
		public Action<StatusResponse> Received_StatusResponse { get; set; }
		public Action<InventoryResponse> Received_InventoryResponse { get; set; }
		public Action<StartCashinResponse> Received_StartCashinResponse { get; set; }
		public Action<CashinCancelResponse> Received_CashinCancelResponse { get; set; }
		public Action<EndCashinResponse> Received_EndCashinResponse { get; set; }
		public Action<ChangeResponse> Received_ChangeResponse { get; set; }
		public Action<ChangeCancelResponse> Received_ChangeCancelResponse { get; set; }
		public Action<UpdateManualDepositTotalResponse> Received_UpdateManualDepositTotalResponse { get; set; }
		public Action<RefreshSalesTotalResponse> Received_RefreshSalesTotalResponse { get; set; }
		public Action<StartReplenishmentFromEntranceResponse> Received_StartReplenishmentFromEntranceResponse { get; set; }
		public Action<ReplenishmentFromEntranceCancelResponse> Received_ReplenishmentFromEntranceCancelResponse { get; set; }
		public Action<EndReplenishmentFromEntranceResponse> Received_EndReplenishmentFromEntranceResponse { get; set; }
		public Action<StartReplenishmentFromCassetteResponse> Received_StartReplenishmentFromCassetteResponse { get; set; }
		public Action<EndReplenishmentFromCassetteResponse> Received_EndReplenishmentFromCassetteResponse { get; set; }
		public Action<CashoutResponse> Received_CashoutResponse { get; set; }
		public Action<CollectResponse> Received_CollectResponse { get; set; }
		public Action<ResetResponse> Received_ResetResponse { get; set; }
		public Action<LoginUserResponse> Received_LoginUserResponse { get; set; }
		public Action<LogoutUserResponse> Received_LogoutUserResponse { get; set; }
		public Action<StartLogreadResponse> Received_StartLogreadResponse { get; set; }
		public Action<StartDownloadResponse> Received_StartDownloadResponse { get; set; }
		public Action<OpenResponse> Received_OpenResponse { get; set; }
		public Action<CloseResponse> Received_CloseResponse { get; set; }
		public Action<OccupyResponse> Received_OccupyResponse { get; set; }
		public Action<ReleaseResponse> Received_ReleaseResponse { get; set; }
		public Action<ReturnCashResponse> Received_ReturnCashResponse { get; set; }
		public Action<EnableDenomResponse> Received_EnableDenomResponse { get; set; }
		public Action<DisableDenomResponse> Received_DisableDenomResponse { get; set; }
		public Action<PowerControlResponse> Received_PowerControlResponse { get; set; }
		public Action<AdjustTimeResponse> Received_AdjustTimeResponse { get; set; }
		public Action<UnLockUnitResponse> Received_UnLockUnitResponse { get; set; }
		public Action<LockUnitResponse> Received_LockUnitResponse { get; set; }
		public Action<OpenExitCoverResponse> Received_OpenExitCoverResponse { get; set; }
		public Action<CloseExitCoverResponse> Received_CloseExitCoverResponse { get; set; }
		public Action<SetExchangeRateResponse> Received_SetExchangeRateResponse { get; set; }

		public Action<HeartBeatEvent> Received_HeartBeatEvent { get; set; }
		public Action<StatusChangeEvent> Received_StatusChangeEvent { get; set; }
		public Action<AmountDetailsEvent> Received_AmountDetailsEvent { get; set; }
		public Action<DispenseDetailsEvent> Received_DispenseDetailsEvent { get; set; }
		public Action<ChangeInventoryStatus> Received_ChangeInventoryStatus { get; set; }
		public Action<SpecialDeviceError> Received_SpecialDeviceError { get; set; }

		public Action<int, eventStatusChange> Received_eventStatusChange { get; set; }
		public Action<int, eventWaitForRemoving> Received_eventWaitForRemoving { get; set; }
		public Action<int, eventRemoved> Received_eventRemoved { get; set; }
		public Action<int, eventCassetteInserted> Received_eventCassetteInserted { get; set; }
		public Action<int, eventCassetteChecked> Received_eventCassetteChecked { get; set; }
		public Action<int, eventCassetteInventoryOnRemoval> Received_eventCassetteInventoryOnRemoval { get; set; }
		public Action<int, eventCassetteInventoryOnInsertion> Received_eventCassetteInventoryOnInsertion { get; set; }
		public Action<int, eventEmpty> Received_eventEmpty { get; set; }
		public Action<int, eventLow> Received_eventLow { get; set; }
		public Action<int, eventExist> Received_eventExist { get; set; }
		public Action<int, eventHigh> Received_eventHigh { get; set; }
		public Action<int, eventFull> Received_eventFull { get; set; }
		public Action<int, eventMissing> Received_eventMissing { get; set; }
		public Action<int, eventDepositCountChange> Received_eventDepositCountChange { get; set; }
		public Action<int, eventDepositCountMonitor> Received_eventDepositCountMonitor { get; set; }
		public Action<int, eventReplenishCountChange> Received_eventReplenishCountChange { get; set; }
		public Action<int, eventError> Received_eventError { get; set; }
		public Action<int, eventDownloadProgress> Received_eventDownloadProgress { get; set; }
		public Action<int, eventLogreadProgress> Received_eventLogreadProgress { get; set; }
		public Action<int, eventRequireVerifyDenomination> Received_eventRequireVerifyDenomination { get; set; }
		public Action<int, eventRequireVerifyCollectionContainer> Received_eventRequireVerifyCollectionContainer { get; set; }
		public Action<int> Received_eventRequireVerifyMixStacker { get; set; }
		public Action<int> Received_eventExactDenomination { get; set; }
		public Action<int, eventExactCollectionContainer> Received_eventExactCollectionContainer { get; set; }
		public Action<int> Received_eventExactMixStacker { get; set; }
		public Action<int, eventWaitForOpening> Received_eventWaitForOpening { get; set; }
		public Action<int, eventOpened> Received_eventOpened { get; set; }
		public Action<int, eventClosed> Received_eventClosed { get; set; }
		public Action<int, eventLocked> Received_eventLocked { get; set; }
		public Action<int, eventWaitForInsertion> Received_eventWaitForInsertion { get; set; }
		public Action<int, eventDepositCategoryInfo> Received_eventDepositCategoryInfo { get; set; }
		public Action<int> Received_eventCountedCategory2 { get; set; }
		public Action<int> Received_eventCountedCategory3 { get; set; }

		private static FccEventControler _Instance = null;
		public static FccEventControler Instance
		{ 
			get 
			{
				if (_Instance == null) _Instance = new FccEventControler();
 				return _Instance;
			}
		}

		private FccEventControler()
		{
		}

		public void Listen(int port)
		{
			Task.Run(new Action(() =>
			{
				var listener = new System.Net.Sockets.TcpListener(System.Net.IPAddress.Any, port);
				System.Net.Sockets.TcpClient tcp = null;

				while (true)
				{
					try
					{
						listener.Start();

						Console.WriteLine("Start listern on Port{0}", port);

						tcp = listener.AcceptTcpClient();
						Console.WriteLine("Client connected.");

						System.Net.Sockets.NetworkStream ns = tcp.GetStream();

						int resSize;
						string recv_data = "";
						bool CloseSock = false;
						do
						{
							byte[] bytes = new byte[tcp.ReceiveBufferSize];
							if (true == ns.CanRead)
							{
								resSize = ns.Read(bytes, 0, tcp.ReceiveBufferSize);

								if (resSize == 0)
								{
									Console.WriteLine("Client is disconnected.");
									CloseSock = true;
									break;
								}
							}
							else
							{
								Console.WriteLine("ns.CanRead false");
								break;
							}

							//-------------------------------------------------------|
							recv_data += System.Text.Encoding.ASCII.GetString(bytes, 0, resSize);
							int index = recv_data.IndexOf("\0");
							if (index > 0)
							{
								while (index > 0)
								{
									var recv_xml = recv_data.Substring(0, index);

									// Here the event handling should be located.>>>>>>>>>>>>>>>
									CheckRecvComand(recv_xml);

									// if one packet includes several events, then the next event 
									// also should be handled.
									recv_data = recv_data.Substring(index + 1);
									index = recv_data.IndexOf("\0");
								}
							}
                            else if(index == 0)
                            {
                                recv_data = "";
                            }
							//-------------------------------------------------------|
						} while (true);

						if (CloseSock == false)
						{
							tcp.Close();
							Console.WriteLine("Disconnected");
							continue;
						}

						Console.WriteLine("Listener is closed.");
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.Message.ToString());
						if (tcp != null)
						{
							tcp.Close();
							tcp = null;
						}
					}
					finally
					{
						listener.Stop();
					}
				}
			}));
		}

		private string[] ArrayValueList = {
			"Denomination",
			"Cash",
			"CashUnit",
			"CashUnits",
			"Currency",
			"DevStatus",
			"RequireVeriyMixStacker",
			"RequireVerifyCollectionContainer",
			"RequireVerifyDenomination"
		};

		private void NormalizeArrayValue(XElement elem, int depth)
		{
			if (depth > 20) return;
		
			foreach( var val in ArrayValueList )
			{
				var child_list = elem.Elements(val);
				if( child_list.Count() == 1 )
				{
					elem.Add(new XElement(val));
				}
			}
			
			foreach( var child in elem.Elements() )
			{
				NormalizeArrayValue(child, depth + 1);
			}
		}

		private void CheckRecvComand(string xml)
		{
			var root = XDocument.Parse(xml).Root;
			var elem = (XElement)root.FirstNode;
			string elemName = elem.Name.LocalName;

			if (elemName == "GlyCashierEvent")
			{
				root = elem;
				elem = (XElement)root.FirstNode;
				elemName = elem.Name.LocalName;
			}

			NormalizeArrayValue(elem, 0);

			string ev = JsonConvert.SerializeXNode(root, Formatting.None, true).Replace("@", "");
			switch (elemName)
			{
				case "RegisterEventResponse":
					if (Received_RegisterEventResponse != null) { Received_RegisterEventResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).RegisterEventResponse); } break;
				case "UnRegisterEventResponse":
					if (Received_UnRegisterEventResponse != null) { Received_UnRegisterEventResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).UnRegisterEventResponse); } break;
				case "StatusResponse":
					if (Received_StatusResponse != null) { Received_StatusResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).StatusResponse); } break;
				case "InventoryResponse":
					if (Received_InventoryResponse != null) { Received_InventoryResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).InventoryResponse); } break;
				case "StartCashinResponse":
					if (Received_StartCashinResponse != null) { Received_StartCashinResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).StartCashinResponse); } break;
				case "CashinCancelResponse":
					if (Received_CashinCancelResponse != null) { Received_CashinCancelResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).CashinCancelResponse); } break;
				case "EndCashinResponse":
					if (Received_EndCashinResponse != null) { Received_EndCashinResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).EndCashinResponse); } break;
				case "ChangeResponse":
					if (Received_ChangeResponse != null) { Received_ChangeResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).ChangeResponse); } break;
				case "ChangeCancelResponse":
					if (Received_ChangeCancelResponse != null) { Received_ChangeCancelResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).ChangeCancelResponse); } break;
				case "UpdateManualDepositTotalResponse":
					if (Received_UpdateManualDepositTotalResponse != null) { Received_UpdateManualDepositTotalResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).UpdateManualDepositTotalResponse); } break;
				case "RefreshSalesTotalResponse":
					if (Received_RefreshSalesTotalResponse != null) { Received_RefreshSalesTotalResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).RefreshSalesTotalResponse); } break;
				case "StartReplenishmentFromEntranceResponse":
					if (Received_StartReplenishmentFromEntranceResponse != null) { Received_StartReplenishmentFromEntranceResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).StartReplenishmentFromEntranceResponse); } break;
				case "ReplenishmentFromEntranceCancelResponse":
					if (Received_ReplenishmentFromEntranceCancelResponse != null) { Received_ReplenishmentFromEntranceCancelResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).ReplenishmentFromEntranceCancelResponse); } break;
				case "EndReplenishmentFromEntranceResponse":
					if (Received_EndReplenishmentFromEntranceResponse != null) { Received_EndReplenishmentFromEntranceResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).EndReplenishmentFromEntranceResponse); } break;
				case "StartReplenishmentFromCassetteResponse":
					if (Received_StartReplenishmentFromCassetteResponse != null) { Received_StartReplenishmentFromCassetteResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).StartReplenishmentFromCassetteResponse); } break;
				case "EndReplenishmentFromCassetteResponse":
					if (Received_EndReplenishmentFromCassetteResponse != null) { Received_EndReplenishmentFromCassetteResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).EndReplenishmentFromCassetteResponse); } break;
				case "CashoutResponse":
					if (Received_CashoutResponse != null) { Received_CashoutResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).CashoutResponse); } break;
				case "CollectResponse":
					if (Received_CollectResponse != null) { Received_CollectResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).CollectResponse); } break;
				case "ResetResponse":
					if (Received_ResetResponse != null) { Received_ResetResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).ResetResponse); } break;
				case "LoginUserResponse":
					if (Received_LoginUserResponse != null) { Received_LoginUserResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).LoginUserResponse); } break;
				case "LogoutUserResponse":
					if (Received_LogoutUserResponse != null) { Received_LogoutUserResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).LogoutUserResponse); } break;
				case "StartLogreadResponse":
					if (Received_StartLogreadResponse != null) { Received_StartLogreadResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).StartLogreadResponse); } break;
				case "StartDownloadResponse":
					if (Received_StartDownloadResponse != null) { Received_StartDownloadResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).StartDownloadResponse); } break;
				case "OpenResponse":
					if (Received_OpenResponse != null) { Received_OpenResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).OpenResponse); } break;
				case "CloseResponse":
					if (Received_CloseResponse != null) { Received_CloseResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).CloseResponse); } break;
				case "OccupyResponse":
					if (Received_OccupyResponse != null) { Received_OccupyResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).OccupyResponse); } break;
				case "ReleaseResponse":
					if (Received_ReleaseResponse != null) { Received_ReleaseResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).ReleaseResponse); } break;
				case "ReturnCashResponse":
					if (Received_ReturnCashResponse != null) { Received_ReturnCashResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).ReturnCashResponse); } break;
				case "EnableDenomResponse":
					if (Received_EnableDenomResponse != null) { Received_EnableDenomResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).EnableDenomResponse); } break;
				case "DisableDenomResponse":
					if (Received_DisableDenomResponse != null) { Received_DisableDenomResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).DisableDenomResponse); } break;
				case "PowerControlResponse":
					if (Received_PowerControlResponse != null) { Received_PowerControlResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).PowerControlResponse); } break;
				case "AdjustTimeResponse":
					if (Received_AdjustTimeResponse != null) { Received_AdjustTimeResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).AdjustTimeResponse); } break;
				case "UnLockUnitResponse":
					if (Received_UnLockUnitResponse != null) { Received_UnLockUnitResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).UnLockUnitResponse); } break;
				case "LockUnitResponse":
					if (Received_LockUnitResponse != null) { Received_LockUnitResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).LockUnitResponse); } break;
				case "OpenExitCoverResponse":
					if (Received_OpenExitCoverResponse != null) { Received_OpenExitCoverResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).OpenExitCoverResponse); } break;
				case "CloseExitCoverResponse":
					if (Received_CloseExitCoverResponse != null) { Received_CloseExitCoverResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).CloseExitCoverResponse); } break;
				case "SetExchangeRateResponse":
					if (Received_SetExchangeRateResponse != null) { Received_SetExchangeRateResponse.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).SetExchangeRateResponse); } break;

				case "HeartBeatEvent":
					if (Received_HeartBeatEvent != null) { Received_HeartBeatEvent.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).HeartBeatEvent); } break;
				case "StatusChangeEvent":
					if (Received_StatusChangeEvent != null) { Received_StatusChangeEvent.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).StatusChangeEvent); } break;
				case "AmountDetailsEvent":
					if (Received_AmountDetailsEvent != null) { Received_AmountDetailsEvent.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).AmountDetailsEvent); } break;
				case "DispenseDetailsEvent":
					if (Received_DispenseDetailsEvent != null)
					{
						var fcc_ev = JsonConvert.DeserializeObject<FCCEvent>(ev);
						if (fcc_ev.DispenseDetailsEvent == null) fcc_ev.DispenseDetailsEvent = new DispenseDetailsEvent();
						Received_DispenseDetailsEvent.Invoke(fcc_ev.DispenseDetailsEvent);
					}
					break;
				case "ChangeInventoryStatus":
					if (Received_ChangeInventoryStatus != null) { Received_ChangeInventoryStatus.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).ChangeInventoryStatus); } break;
				case "SpecialDeviceError":
					if (Received_SpecialDeviceError != null) { Received_SpecialDeviceError.Invoke(JsonConvert.DeserializeObject<FCCEvent>(ev).SpecialDeviceError); } break;

				case "eventStatusChange":
					if (Received_eventStatusChange != null) { var dev_ev = JsonConvert.DeserializeObject<DeviceEvent>(ev); Received_eventStatusChange.Invoke(dev_ev.devid, dev_ev.eventStatusChange); } break;
				case "eventWaitForRemoving":
					if (Received_eventWaitForRemoving != null) { var dev_ev = JsonConvert.DeserializeObject<DeviceEvent>(ev); Received_eventWaitForRemoving.Invoke(dev_ev.devid, dev_ev.eventWaitForRemoving); } break;
				case "eventRemoved":
					if (Received_eventRemoved != null) { var dev_ev = JsonConvert.DeserializeObject<DeviceEvent>(ev); Received_eventRemoved.Invoke(dev_ev.devid, dev_ev.eventRemoved); } break;
				case "eventCassetteInserted":
					if (Received_eventCassetteInserted != null) { var dev_ev = JsonConvert.DeserializeObject<DeviceEvent>(ev); Received_eventCassetteInserted.Invoke(dev_ev.devid, dev_ev.eventCassetteInserted); } break;
				case "eventCassetteChecked":
					if (Received_eventCassetteChecked != null) { var dev_ev = JsonConvert.DeserializeObject<DeviceEvent>(ev); Received_eventCassetteChecked.Invoke(dev_ev.devid, dev_ev.eventCassetteChecked); } break;
				case "eventCassetteInventoryOnRemoval":
					if (Received_eventCassetteInventoryOnRemoval != null) { var dev_ev = JsonConvert.DeserializeObject<DeviceEvent>(ev); Received_eventCassetteInventoryOnRemoval.Invoke(dev_ev.devid, dev_ev.eventCassetteInventoryOnRemoval); } break;
				case "eventCassetteInventoryOnInsertion":
					if (Received_eventCassetteInventoryOnInsertion != null) { var dev_ev = JsonConvert.DeserializeObject<DeviceEvent>(ev); Received_eventCassetteInventoryOnInsertion.Invoke(dev_ev.devid, dev_ev.eventCassetteInventoryOnInsertion); } break;
				case "eventEmpty":
					if (Received_eventEmpty != null) { var dev_ev = JsonConvert.DeserializeObject<DeviceEvent>(ev); Received_eventEmpty.Invoke(dev_ev.devid, dev_ev.eventEmpty); } break;
				case "eventLow":
					if (Received_eventLow != null) { var dev_ev = JsonConvert.DeserializeObject<DeviceEvent>(ev); Received_eventLow.Invoke(dev_ev.devid, dev_ev.eventLow); } break;
				case "eventExist":
					if (Received_eventExist != null) { var dev_ev = JsonConvert.DeserializeObject<DeviceEvent>(ev); Received_eventExist.Invoke(dev_ev.devid, dev_ev.eventExist); } break;
				case "eventHigh":
					if (Received_eventHigh != null) { var dev_ev = JsonConvert.DeserializeObject<DeviceEvent>(ev); Received_eventHigh.Invoke(dev_ev.devid, dev_ev.eventHigh); } break;
				case "eventFull":
					if (Received_eventFull != null) { var dev_ev = JsonConvert.DeserializeObject<DeviceEvent>(ev); Received_eventFull.Invoke(dev_ev.devid, dev_ev.eventFull); } break;
				case "eventMissing":
					if (Received_eventMissing != null) { var dev_ev = JsonConvert.DeserializeObject<DeviceEvent>(ev); Received_eventMissing.Invoke(dev_ev.devid, dev_ev.eventMissing); } break;
				case "eventDepositCountChange":
					if (Received_eventDepositCountChange != null)
					{
						var dev_ev = JsonConvert.DeserializeObject<DeviceEvent>(ev);
						if (dev_ev.eventDepositCountChange == null) dev_ev.eventDepositCountChange = new eventDepositCountChange();
						Received_eventDepositCountChange.Invoke(dev_ev.devid, dev_ev.eventDepositCountChange);
					}
					break;
				case "eventDepositCountMonitor":
					if (Received_eventDepositCountMonitor != null)
					{
						var dev_ev = JsonConvert.DeserializeObject<DeviceEvent>(ev);
						if (dev_ev.eventDepositCountMonitor == null) dev_ev.eventDepositCountMonitor = new eventDepositCountMonitor();
						Received_eventDepositCountMonitor.Invoke(dev_ev.devid, dev_ev.eventDepositCountMonitor);
					}
					break;
				case "eventReplenishCountChange":
					if (Received_eventReplenishCountChange != null)
					{
						var dev_ev = JsonConvert.DeserializeObject<DeviceEvent>(ev);
						if (dev_ev.eventReplenishCountChange == null) dev_ev.eventReplenishCountChange = new eventReplenishCountChange();
						Received_eventReplenishCountChange.Invoke(dev_ev.devid, dev_ev.eventReplenishCountChange);
					}
					break;
				case "eventError":
					if (Received_eventError != null) { var dev_ev = JsonConvert.DeserializeObject<DeviceEvent>(ev); Received_eventError.Invoke(dev_ev.devid, dev_ev.eventError); } break;
				case "eventDownloadProgress":
					if (Received_eventDownloadProgress != null) { var dev_ev = JsonConvert.DeserializeObject<DeviceEvent>(ev); Received_eventDownloadProgress.Invoke(dev_ev.devid, dev_ev.eventDownloadProgress); } break;
				case "eventLogreadProgress":
					if (Received_eventLogreadProgress != null) { var dev_ev = JsonConvert.DeserializeObject<DeviceEvent>(ev); Received_eventLogreadProgress.Invoke(dev_ev.devid, dev_ev.eventLogreadProgress); } break;
				case "eventRequireVerifyDenomination":
					if (Received_eventRequireVerifyDenomination != null) { var dev_ev = JsonConvert.DeserializeObject<DeviceEvent>(ev); Received_eventRequireVerifyDenomination.Invoke(dev_ev.devid, dev_ev.eventRequireVerifyDenomination); } break;
				case "eventRequireVerifyCollectionContainer":
					if (Received_eventRequireVerifyCollectionContainer != null) { var dev_ev = JsonConvert.DeserializeObject<DeviceEvent>(ev); Received_eventRequireVerifyCollectionContainer.Invoke(dev_ev.devid, dev_ev.eventRequireVerifyCollectionContainer); } break;
				case "eventRequireVerifyMixStacker":
					if (Received_eventRequireVerifyMixStacker != null) { var dev_ev = JsonConvert.DeserializeObject<DeviceEvent>(ev); Received_eventRequireVerifyMixStacker.Invoke(dev_ev.devid); } break;
				case "eventExactDenomination":
					if (Received_eventExactDenomination != null) { var dev_ev = JsonConvert.DeserializeObject<DeviceEvent>(ev); Received_eventExactDenomination.Invoke(dev_ev.devid); } break;
				case "eventExactCollectionContainer":
					if (Received_eventExactCollectionContainer != null) { var dev_ev = JsonConvert.DeserializeObject<DeviceEvent>(ev); Received_eventExactCollectionContainer.Invoke(dev_ev.devid, dev_ev.eventExactCollectionContainer); } break;
				case "eventExactMixStacker":
					if (Received_eventExactMixStacker != null) { var dev_ev = JsonConvert.DeserializeObject<DeviceEvent>(ev); Received_eventExactMixStacker.Invoke(dev_ev.devid); } break;
				case "eventWaitForOpening":
					if (Received_eventWaitForOpening != null) { var dev_ev = JsonConvert.DeserializeObject<DeviceEvent>(ev); Received_eventWaitForOpening.Invoke(dev_ev.devid, dev_ev.eventWaitForOpening); } break;
				case "eventOpened":
					if (Received_eventOpened != null) { var dev_ev = JsonConvert.DeserializeObject<DeviceEvent>(ev); Received_eventOpened.Invoke(dev_ev.devid, dev_ev.eventOpened); } break;
				case "eventClosed":
					if (Received_eventClosed != null) { var dev_ev = JsonConvert.DeserializeObject<DeviceEvent>(ev); Received_eventClosed.Invoke(dev_ev.devid, dev_ev.eventClosed); } break;
				case "eventLocked":
					if (Received_eventLocked != null) { var dev_ev = JsonConvert.DeserializeObject<DeviceEvent>(ev); Received_eventLocked.Invoke(dev_ev.devid, dev_ev.eventLocked); } break;
				case "eventWaitForInsertion":
					if (Received_eventWaitForInsertion != null) { var dev_ev = JsonConvert.DeserializeObject<DeviceEvent>(ev); Received_eventWaitForInsertion.Invoke(dev_ev.devid, dev_ev.eventWaitForInsertion); } break;
				case "eventDepositCategoryInfo":
					if (Received_eventDepositCategoryInfo != null) { var dev_ev = JsonConvert.DeserializeObject<DeviceEvent>(ev); Received_eventDepositCategoryInfo.Invoke(dev_ev.devid, dev_ev.eventDepositCategoryInfo); } break;
				case "eventCountedCategory2":
					if (Received_eventCountedCategory2 != null) { var dev_ev = JsonConvert.DeserializeObject<DeviceEvent>(ev); Received_eventCountedCategory2.Invoke(dev_ev.devid); } break;
				case "eventCountedCategory3":
					if (Received_eventCountedCategory3 != null) { var dev_ev = JsonConvert.DeserializeObject<DeviceEvent>(ev); Received_eventCountedCategory3.Invoke(dev_ev.devid); } break;
			}
		}
	}
}
