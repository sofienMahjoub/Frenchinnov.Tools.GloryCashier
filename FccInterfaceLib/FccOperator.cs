using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Forms;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace FccInterfaceLib
{
	public class FccOperator
	{
		private BrueBoxService.BrueBoxPortTypeClient ServiceClient;
		private BrueBoxService.BrueBoxPortTypeClient ServiceClientForChange;
		private string EndPoint;

		public FccOperator(string url)
		{
			EndPoint = url;

			System.ServiceModel.WSHttpBinding binding;

			if (url.IndexOf("https:") >= 0)
			{
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(OnRemoteCertificateValidationCallback);
				
				binding = new System.ServiceModel.WSHttpBinding(System.ServiceModel.SecurityMode.Transport);
				binding.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.None;
			}
			else
			{
				binding = new System.ServiceModel.WSHttpBinding(System.ServiceModel.SecurityMode.None);
			}

			binding.OpenTimeout = binding.CloseTimeout = TimeSpan.FromSeconds(5);
			binding.SendTimeout = binding.ReceiveTimeout = TimeSpan.FromSeconds(120);

			var address = new System.ServiceModel.EndpointAddress(EndPoint);
			ServiceClient = new BrueBoxService.BrueBoxPortTypeClient(binding, address);
			ServiceClientForChange = new BrueBoxService.BrueBoxPortTypeClient(binding, address);
		}

		//-------------------------------------------
		// ServerCertificateValidationCallback
		//-------------------------------------------
		private bool OnRemoteCertificateValidationCallback(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
            //  return true;  // "SSL certificate to use no problem" and that
                              // If the certificate is always OK to True returns only OK
			if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
			{
				return true;
			}

			if (sender is System.Net.WebRequest)
			{
				System.Net.WebRequest Request = (System.Net.WebRequest)sender;
                //Only if the certificate matches the host name is set to OK
                if (EndPoint.IndexOf(Request.RequestUri.Host) > 0)
				{
					return true;
				}
			}

			return false;
		}

		//-------------------------------------------
		// SetTimeout
		//-------------------------------------------
		public void SetTimeout(int msec)
		{
			ServiceClient.Endpoint.Binding.SendTimeout = ServiceClient.Endpoint.Binding.ReceiveTimeout = TimeSpan.FromMilliseconds(msec);
		}

		public void SetChangeTimeout(int msec)
		{
			ServiceClientForChange.Endpoint.Binding.SendTimeout = ServiceClientForChange.Endpoint.Binding.ReceiveTimeout = TimeSpan.FromMilliseconds(msec);
		}

		//-------------------------------------------
		// RegisterEvent
		//-------------------------------------------
		public RegisterEventResponse RegisterEvent(RegisterEventRequest request)
		{
			RegisterEventResponse response = new RegisterEventResponse();

			var service_request = new BrueBoxService.RegisterEventRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;
			service_request.SessionID = request.SessionID;

			service_request.Url = request.Url;
			service_request.Port = request.Port.ToString();
			
			service_request.DestinationType = new BrueBoxService.StatusOptionType();
			service_request.DestinationType.type = request.DestinationType.ToString();

			service_request.Encryption = new BrueBoxService.StatusOptionType();
			service_request.Encryption.type = request.Encryption.ToString();

			if (request.RequireEventList != null)
			{
				service_request.RequireEventList = new BrueBoxService.RequireEventType[request.RequireEventList.Count];
				for (int i = 0; i < request.RequireEventList.Count; i++)
				{
                    service_request.RequireEventList[i] = new BrueBoxService.RequireEventType();
					service_request.RequireEventList[i].eventno = request.RequireEventList[i].ToString();
				}
			}

			try
			{
				var service_response = ServiceClient.RegisterEventOperation(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;
			}
			catch (Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<RegisterEventResponse> RegisterEventAsync(RegisterEventRequest request)
		{
			return Task<StatusResponse>.Run<RegisterEventResponse>(() => { return RegisterEvent(request); });
		}

		//-------------------------------------------
		// UnRegisterEvent
		//-------------------------------------------
		public UnRegisterEventResponse UnRegisterEvent(UnRegisterEventRequest request)
		{
			UnRegisterEventResponse response = new UnRegisterEventResponse();

			var service_request = new BrueBoxService.UnRegisterEventRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;
			service_request.SessionID = request.SessionID;

			service_request.Url = request.Url;
			service_request.Port = request.Port.ToString();

			try
			{
				var service_response = ServiceClient.UnRegisterEventOperation(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;
			}
			catch (Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<UnRegisterEventResponse> UnRegisterEventAsync(UnRegisterEventRequest request)
		{
			return Task<UnRegisterEventResponse>.Run<UnRegisterEventResponse>(() => { return UnRegisterEvent(request); });
		}

		//-------------------------------------------
		// GetStatus
		//-------------------------------------------
		public StatusResponse GetStatus(StatusRequest request)
		{
			StatusResponse response = new StatusResponse();

			var service_request = new BrueBoxService.StatusRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;
			service_request.SessionID = request.SessionID;

			service_request.Option = new BrueBoxService.StatusOptionType();
			service_request.Option.type = request.Option.ToString();

			service_request.RequireVerification = new BrueBoxService.RequireVerificationType();
			service_request.RequireVerification.type = request.RequireVerification.ToString();

			try
			{
				var service_response = ServiceClient.GetStatus(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;

				response.Status = ConvertToFCC_Status(service_response.Status);

				if (service_response.Cash != null)
				{
					response.Cash = ConvertToFCC_CashList(new BrueBoxService.CashType[] { service_response.Cash });
				}

				if (service_response.RequireVerifyInfos != null)
				{
					response.RequireVerifyInfos = new RequireVerifyInfos();

					response.RequireVerifyInfos.RequireVerifyDenominationInfos = new RequireVerifyDenominationInfos();
					response.RequireVerifyInfos.RequireVerifyDenominationInfos.RequireVerifyDenomination = new RequireVerifyDenominationList();
					foreach (var info in service_response.RequireVerifyInfos.RequireVerifyDenominationInfos)
					{
						var f_req_verify = new RequireVerifyDenomination();

						f_req_verify.devid = int.Parse(info.devid);
						f_req_verify.val = int.Parse(info.val);
						f_req_verify.Cash = ConvertToFCC_CashList(new BrueBoxService.CashType[] { info.Cash });

						response.RequireVerifyInfos.RequireVerifyDenominationInfos.RequireVerifyDenomination.Add(f_req_verify);
					}

					response.RequireVerifyInfos.RequireVerifyCollectionContainerInfos = new RequireVerifyCollectionContainerInfos();
					response.RequireVerifyInfos.RequireVerifyCollectionContainerInfos.RequireVerifyCollectionContainer = new RequireVerifyCollectionContainerList();
					foreach (var info in service_response.RequireVerifyInfos.RequireVerifyCollectionContainerInfos)
					{
						var f_req_verify = new RequireVerifyCollectionContainer();

						f_req_verify.devid = int.Parse(info.devid);
						f_req_verify.val = int.Parse(info.val);
						f_req_verify.SerialNo = info.SerialNo;

						response.RequireVerifyInfos.RequireVerifyCollectionContainerInfos.RequireVerifyCollectionContainer.Add(f_req_verify);
					}

					response.RequireVerifyInfos.RequireVerifyMixStackerInfos = new RequireVerifyMixStackerInfos();
					response.RequireVerifyInfos.RequireVerifyMixStackerInfos.RequireVerifyMixStacker = new RequireVerifyMixStackerList();
					foreach (var info in service_response.RequireVerifyInfos.RequireVerifyMixStackerInfos)
					{
						var f_req_verify = new RequireVerifyMixStacker();

						f_req_verify.devid = int.Parse(info.devid);
						f_req_verify.val = int.Parse(info.val);

						response.RequireVerifyInfos.RequireVerifyMixStackerInfos.RequireVerifyMixStacker.Add(f_req_verify);
					}
				}
			}
			catch(Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<StatusResponse> GetStatusAsync(StatusRequest request)
		{
			return Task<StatusResponse>.Run<StatusResponse>(() => { return GetStatus(request); });
		}

		//-------------------------------------------
		// GetInventory
		//-------------------------------------------
		public InventoryResponse GetInventory(InventoryRequest request)
		{
			InventoryResponse response = new InventoryResponse();

			var service_request = new BrueBoxService.InventoryRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;
			service_request.SessionID = request.SessionID;

			service_request.Option = new BrueBoxService.InventoryOptionType();
			service_request.Option.type = request.Option.ToString();

			try
			{
				var service_response = ServiceClient.InventoryOperation(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;
				response.InvalidCassette = service_response.InvalidCassette;

				if (service_response.Cash != null)
				{
					response.Cash = ConvertToFCC_CashList(service_response.Cash);
				}

				if (service_response.CashUnits != null)
				{
					response.CashUnits = ConvertToFCC_CashUnitsList(service_response.CashUnits);
				}
			}
			catch(Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<InventoryResponse> GetInventoryAsync(InventoryRequest request)
		{
			return Task<InventoryResponse>.Run<InventoryResponse>(() => { return GetInventory(request); });
		}

		//-------------------------------------------
		// Change
		//-------------------------------------------
		public ChangeResponse Change(ChangeRequest request)
		{
			ChangeResponse response = new ChangeResponse();

			var service_request = new BrueBoxService.ChangeRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;
			service_request.SessionID = request.SessionID;
			service_request.Amount = request.Amount.ToString();

			service_request.Option = new BrueBoxService.CashinOptionType();
			service_request.Option.type = request.Option.ToString();

			if (request.Cash != null)
			{
				service_request.Cash = ConvertToSOAP_Cash(request.Cash);
			}

			if (request.ForeignCurrency != null)
			{
				service_request.ForeignCurrency = new BrueBoxService.ForeignCurrencyRequestType();
				service_request.ForeignCurrency.cc = request.ForeignCurrency.cc;
				service_request.ForeignCurrency.Rate = request.ForeignCurrency.rate.ToString();
			}

			try
			{
				var service_response = ServiceClientForChange.ChangeOperation(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;
				response.Amount = int.Parse(service_response.Amount);
				response.ManualDeposit = int.Parse(service_response.ManualDeposit);

				response.Status = ConvertToFCC_Status(service_response.Status);

				if (service_response.Cash != null)
				{
					response.Cash = ConvertToFCC_CashList(service_response.Cash);
				}

				if (service_response.DepositCurrency != null)
				{
					response.DepositCurrency = new DepositCurrency();
					response.DepositCurrency.Currency = ConvertToFCC_CurrencyList(service_response.DepositCurrency);
				}

				if (service_response.CashInAmountDetails != null)
				{
					response.CashInAmountDetails = ConvertToFCC_DepositDetails(service_response.ManualDepositDetails);
				}

				if (service_response.ManualDepositDetails != null)
				{
					response.ManualDepositDetails = ConvertToFCC_DepositDetails(service_response.ManualDepositDetails);
				}
			}
			catch (Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<ChangeResponse> ChangeAsync(ChangeRequest request)
		{
			return Task<ChangeResponse>.Run<ChangeResponse>(() => { return Change(request); });
		}

		//-------------------------------------------
		// ChangeCancel
		//-------------------------------------------
		public ChangeCancelResponse ChangeCancel(ChangeCancelRequest request)
		{
			ChangeCancelResponse response = new ChangeCancelResponse();

			var service_request = new BrueBoxService.ChangeCancelRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;
			service_request.SessionID = request.SessionID;

			service_request.Option = new BrueBoxService.CashinOptionType();
			service_request.Option.type = request.Option.ToString();

			try
			{
				var service_response = ServiceClient.ChangeCancelOperation(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;
			}
			catch (Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<ChangeCancelResponse> ChangeCancelAsync(ChangeCancelRequest request)
		{
			return Task<ChangeCancelResponse>.Run<ChangeCancelResponse>(() => { return ChangeCancel(request); });
		}

		//-------------------------------------------
		// StartCashin
		//-------------------------------------------
		public StartCashinResponse StartCashin(StartCashinRequest request)
		{
			StartCashinResponse response = new StartCashinResponse();

			var service_request = new BrueBoxService.StartCashinRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;
			service_request.SessionID = request.SessionID;

			service_request.Option = new BrueBoxService.CashinOptionType();
			service_request.Option.type = request.Option.ToString();

			if (request.ForeignCurrency != null)
			{
				service_request.ForeignCurrency = new BrueBoxService.ForeignCurrencyRequestType();
				service_request.ForeignCurrency.cc = request.ForeignCurrency.cc;
				service_request.ForeignCurrency.Rate = request.ForeignCurrency.rate.ToString();
			}

			try
			{
				var service_response = ServiceClient.StartCashinOperation(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;
			}
			catch(Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<StartCashinResponse> StartCashinAsync(StartCashinRequest request)
		{
			return Task<StartCashinResponse>.Run<StartCashinResponse>(() => { return StartCashin(request); });
		}

		//-------------------------------------------
		// CashinCancel
		//-------------------------------------------
		public CashinCancelResponse CashinCancel(CashinCancelRequest request)
		{
			CashinCancelResponse response = new CashinCancelResponse();

			var service_request = new BrueBoxService.CashinCancelRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;
			service_request.SessionID = request.SessionID;

			try
			{
				var service_response = ServiceClient.CashinCancelOperation(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;

				response.ManualDeposit = int.Parse(service_response.ManualDeposit);

				if (service_response.Cash != null)
				{
					response.Cash = ConvertToFCC_CashList(service_response.Cash);
				}

				if (service_response.DepositCurrency != null)
				{
					response.DepositCurrency = new DepositCurrency();
					response.DepositCurrency.Currency = ConvertToFCC_CurrencyList(service_response.DepositCurrency);
				}

				if (service_response.CashInAmountDetails != null)
				{
					response.CashInAmountDetails = ConvertToFCC_DepositDetails(service_response.ManualDepositDetails);
				}

				if (service_response.ManualDepositDetails != null)
				{
					response.ManualDepositDetails = ConvertToFCC_DepositDetails(service_response.ManualDepositDetails);
				}
			}
			catch(Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<CashinCancelResponse> CashinCancelAsync(CashinCancelRequest request)
		{
			return Task<CashinCancelResponse>.Run<CashinCancelResponse>(() => { return CashinCancel(request); });
		}

		//-------------------------------------------
		// EndCashin
		//-------------------------------------------
		public EndCashinResponse EndCashin(EndCashinRequest request)
		{
			EndCashinResponse response = new EndCashinResponse();

			var service_request = new BrueBoxService.EndCashinRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;
			service_request.SessionID = request.SessionID;

			try
			{
				var service_response = ServiceClient.EndCashinOperation(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;

				response.ManualDeposit = int.Parse(service_response.ManualDeposit);

				if (service_response.Cash != null)
				{
					response.Cash = ConvertToFCC_CashList(new BrueBoxService.CashType[]{ service_response.Cash });
				}

				if (service_response.DepositCurrency != null)
				{
					response.DepositCurrency = new DepositCurrency();
					response.DepositCurrency.Currency = ConvertToFCC_CurrencyList(service_response.DepositCurrency);
				}

				if (service_response.CashInAmountDetails != null)
				{
					response.CashInAmountDetails = ConvertToFCC_DepositDetails(service_response.ManualDepositDetails);
				}

				if (service_response.ManualDepositDetails != null)
				{
					response.ManualDepositDetails = ConvertToFCC_DepositDetails(service_response.ManualDepositDetails);
				}
			}
			catch(Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<EndCashinResponse> EndCashinAsync(EndCashinRequest request)
		{
			return Task<EndCashinResponse>.Run<EndCashinResponse>(() => { return EndCashin(request); });
		}

		//-------------------------------------------
		// UpdateManualDepositTotal
		//-------------------------------------------
		public UpdateManualDepositTotalResponse UpdateManualDepositTotal(UpdateManualDepositTotalRequest request)
		{
			UpdateManualDepositTotalResponse response = new UpdateManualDepositTotalResponse();

			var service_request = new BrueBoxService.UpdateManualDepositTotalRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;
			service_request.SessionID = request.SessionID;
			service_request.Amount = request.Amount.ToString();

			if (request.DepositCurrency != null)
			{
				service_request.DepositCurrency = ConvertToSOAP_CurrencyList(request.DepositCurrency.Currency);
			}

			if (request.ForeignAmount != null)
			{
				service_request.ForeignAmount = new BrueBoxService.ForeignAmountType();
				service_request.ForeignAmount.Amount = request.ForeignAmount.Amount.ToString();
				service_request.ForeignAmount.cc = request.ForeignAmount.cc;
			}

			try
			{
				var service_response = ServiceClient.UpdateManualDepositTotalOperation(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;
			}
			catch(Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<UpdateManualDepositTotalResponse> UpdateManualDepositTotalAsync(UpdateManualDepositTotalRequest request)
		{
			return Task<UpdateManualDepositTotalResponse>.Run<UpdateManualDepositTotalResponse>(() => { return UpdateManualDepositTotal(request); });
		}

		//-------------------------------------------
		// RefreshSalesTotal
		//-------------------------------------------
		public RefreshSalesTotalResponse RefreshSalesTotal(RefreshSalesTotalRequest request)
		{
			RefreshSalesTotalResponse response = new RefreshSalesTotalResponse();

			var service_request = new BrueBoxService.RefreshSalesTotalRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;
			service_request.SessionID = request.SessionID;
			service_request.Amount = request.Amount.ToString();

			try
			{
				var service_response = ServiceClient.RefreshSalesTotalOperation(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;
			}
			catch(Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<RefreshSalesTotalResponse> RefreshSalesTotalAsync(RefreshSalesTotalRequest request)
		{
			return Task<RefreshSalesTotalResponse>.Run<RefreshSalesTotalResponse>(() => { return RefreshSalesTotal(request); });
		}

		//-------------------------------------------
		// StartReplenishmentFromEntrance
		//-------------------------------------------
		public StartReplenishmentFromEntranceResponse StartReplenishmentFromEntrance(StartReplenishmentFromEntranceRequest request)
		{
			StartReplenishmentFromEntranceResponse response = new StartReplenishmentFromEntranceResponse();

			var service_request = new BrueBoxService.StartReplenishmentFromEntranceRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;
			service_request.SessionID = request.SessionID;

			try
			{
				var service_response = ServiceClient.StartReplenishmentFromEntranceOperation(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;
			}
			catch(Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<StartReplenishmentFromEntranceResponse> StartReplenishmentFromEntranceAsync(StartReplenishmentFromEntranceRequest request)
		{
			return Task<StartReplenishmentFromEntranceResponse>.Run<StartReplenishmentFromEntranceResponse>(() => { return StartReplenishmentFromEntrance(request); });
		}

		//-------------------------------------------
		// ReplenishmentFromEntranceCancel
		//-------------------------------------------
		public ReplenishmentFromEntranceCancelResponse ReplenishmentFromEntranceCancel(ReplenishmentFromEntranceCancelRequest request)
		{
			ReplenishmentFromEntranceCancelResponse response = new ReplenishmentFromEntranceCancelResponse();

			var service_request = new BrueBoxService.ReplenishmentFromEntranceCancelRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;
			service_request.SessionID = request.SessionID;

			try
			{
				var service_response = ServiceClient.ReplenishmentFromEntranceCancelOperation(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;

				response.ManualDeposit = int.Parse(service_response.ManualDeposit);

				if (service_response.Cash != null)
				{
					response.Cash = ConvertToFCC_CashList(service_response.Cash);
				}

				if (service_response.DepositCurrency != null)
				{
					response.DepositCurrency = new DepositCurrency();
					response.DepositCurrency.Currency = ConvertToFCC_CurrencyList(service_response.DepositCurrency);
				}
			}
			catch(Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<ReplenishmentFromEntranceCancelResponse> ReplenishmentFromEntranceCancelAsync(ReplenishmentFromEntranceCancelRequest request)
		{
			return Task<ReplenishmentFromEntranceCancelResponse>.Run<ReplenishmentFromEntranceCancelResponse>(() => { return ReplenishmentFromEntranceCancel(request); });
		}

		//-------------------------------------------
		// EndReplenishmentFromEntrance
		//-------------------------------------------
		public EndReplenishmentFromEntranceResponse EndReplenishmentFromEntrance(EndReplenishmentFromEntranceRequest request)
		{
			EndReplenishmentFromEntranceResponse response = new EndReplenishmentFromEntranceResponse();

			var service_request = new BrueBoxService.EndReplenishmentFromEntranceRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;
			service_request.SessionID = request.SessionID;

			try
			{
				var service_response = ServiceClient.EndReplenishmentFromEntranceOperation(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;

				if (service_response.Cash != null)
				{
					response.Cash = ConvertToFCC_CashList(new BrueBoxService.CashType[] { service_response.Cash });
				}

				if (service_response.DepositCurrency != null)
				{
					response.DepositCurrency = new DepositCurrency();
					response.DepositCurrency.Currency = ConvertToFCC_CurrencyList(service_response.DepositCurrency);
				}
			}
			catch(Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<EndReplenishmentFromEntranceResponse> EndReplenishmentFromEntranceAsync(EndReplenishmentFromEntranceRequest request)
		{
			return Task<EndReplenishmentFromEntranceResponse>.Run<EndReplenishmentFromEntranceResponse>(() => { return EndReplenishmentFromEntrance(request); });
		}

		//-------------------------------------------
		// StartReplenishmentFromCassette
		//-------------------------------------------
		public StartReplenishmentFromCassetteResponse StartReplenishmentFromCassette(StartReplenishmentFromCassetteRequest request)
		{
			StartReplenishmentFromCassetteResponse response = new StartReplenishmentFromCassetteResponse();

			var service_request = new BrueBoxService.StartReplenishmentFromCassetteRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;
			service_request.SessionID = request.SessionID;

			try
			{
				var service_response = ServiceClient.StartReplenishmentFromCassetteOperation(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;
			}
			catch(Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<StartReplenishmentFromCassetteResponse> StartReplenishmentFromCassetteAsync(StartReplenishmentFromCassetteRequest request)
		{
			return Task<StartReplenishmentFromCassetteResponse>.Run<StartReplenishmentFromCassetteResponse>(() => { return StartReplenishmentFromCassette(request); });
		}

		//-------------------------------------------
		// EndReplenishmentFromCassette
		//-------------------------------------------
		public EndReplenishmentFromCassetteResponse EndReplenishmentFromCassette(EndReplenishmentFromCassetteRequest request)
		{
			EndReplenishmentFromCassetteResponse response = new EndReplenishmentFromCassetteResponse();

			var service_request = new BrueBoxService.EndReplenishmentFromCassetteRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;
			service_request.SessionID = request.SessionID;

			try
			{
				var service_response = ServiceClient.EndReplenishmentFromCassetteOperation(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;

				if (service_response.Cash != null)
				{
					response.Cash = ConvertToFCC_CashList(new BrueBoxService.CashType[] { service_response.Cash });
				}
			}
			catch(Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<EndReplenishmentFromCassetteResponse> EndReplenishmentFromCassetteAsync(EndReplenishmentFromCassetteRequest request)
		{
			return Task<EndReplenishmentFromCassetteResponse>.Run<EndReplenishmentFromCassetteResponse>(() => { return EndReplenishmentFromCassette(request); });
		}

		//-------------------------------------------
		// Cashout
		//-------------------------------------------
		public CashoutResponse Cashout(CashoutRequest request)
		{
			CashoutResponse response = new CashoutResponse();

			var service_request = new BrueBoxService.CashoutRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;
			service_request.SessionID = request.SessionID;
			service_request.Cash = ConvertToSOAP_Cash(request.Cash);

			try
			{
				var service_response = ServiceClient.CashoutOperation(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;

				if (service_response.Cash != null)
				{
					response.Cash = ConvertToFCC_CashList(new BrueBoxService.CashType[] { service_response.Cash });
				}
			}
			catch(Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<CashoutResponse> CashoutAsync(CashoutRequest request)
		{
			return Task<CashoutResponse>.Run<CashoutResponse>(() => { return Cashout(request); });
		}

		//-------------------------------------------
		// Collect
		//-------------------------------------------
		public CollectResponse Collect(CollectRequest request)
		{
			CollectResponse response = new CollectResponse();

			var service_request = new BrueBoxService.CollectRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;
			service_request.SessionID = request.SessionID;

			service_request.Option = new BrueBoxService.CollectOptionType();
			service_request.Option.type = request.Option.ToString();

			service_request.RequireVerification = new BrueBoxService.RequireVerificationType();
			service_request.RequireVerification.type = request.RequireVerification.ToString();

			service_request.IFCassette = new BrueBoxService.CollectOptionType();
			service_request.IFCassette.type = request.IFCassette.ToString();

			service_request.Mix = new BrueBoxService.CollectOptionType();
			service_request.Mix.type = request.Mix.ToString();

			service_request.Partial = new BrueBoxService.CollectPartialType();
			service_request.Partial.type = request.Partial.ToString();

			service_request.COFBClearOption = new BrueBoxService.CollectOptionType();
			service_request.COFBClearOption.type = request.COFBClearOption.ToString();

			service_request.Cash = ConvertToSOAP_Cash(request.Cash);

			try
			{
				var service_response = ServiceClient.CollectOperation(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;

				if (service_response.Cash != null)
				{
					response.Cash = ConvertToFCC_CashList(service_response.Cash);
				}
			}
			catch(Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<CollectResponse> CollectAsync(CollectRequest request)
		{
			return Task<CollectResponse>.Run<CollectResponse>(() => { return Collect(request); });
		}

		//-------------------------------------------
		// Reset
		//-------------------------------------------
		public ResetResponse Reset(ResetRequest request)
		{
			ResetResponse response = new ResetResponse();

			var service_request = new BrueBoxService.ResetRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;
			service_request.SessionID = request.SessionID;

			try
			{
				var service_response = ServiceClient.ResetOperation(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;
			}
			catch(Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<ResetResponse> ResetAsync(ResetRequest request)
		{
			return Task<ResetResponse>.Run<ResetResponse>(() => { return Reset(request); });
		}

		//-------------------------------------------
		// LoginUser
		//-------------------------------------------
		public LoginUserResponse LoginUser(LoginUserRequest request)
		{
			LoginUserResponse response = new LoginUserResponse();

			var service_request = new BrueBoxService.LoginUserRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;
			service_request.User = request.User;

			try
			{
				var service_response = ServiceClient.LoginUserOperation(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;
			}
			catch(Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<LoginUserResponse> LoginUserAsync(LoginUserRequest request)
		{
			return Task<LoginUserResponse>.Run<LoginUserResponse>(() => { return LoginUser(request); });
		}

		//-------------------------------------------
		// LogoutUser
		//-------------------------------------------
		public LogoutUserResponse LogoutUser(LogoutUserRequest request)
		{
			LogoutUserResponse response = new LogoutUserResponse();

			var service_request = new BrueBoxService.LogoutUserRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;

			try
			{
				var service_response = ServiceClient.LogoutUserOperation(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;
			}
			catch(Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<LogoutUserResponse> LogoutUserAsync(LogoutUserRequest request)
		{
			return Task<LogoutUserResponse>.Run<LogoutUserResponse>(() => { return LogoutUser(request); });
		}

		//-------------------------------------------
		// Open
		//-------------------------------------------
		public OpenResponse Open(OpenRequest request)
		{
			OpenResponse response = new OpenResponse();

			var service_request = new BrueBoxService.OpenRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;
			service_request.User = request.User;
			service_request.UserPwd = request.UserPwd;
			service_request.DeviceName = "";

			try
			{
				var service_response = ServiceClient.OpenOperation(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;
				response.SessionID = service_response.SessionID;
			}
			catch(Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<OpenResponse> OpenAsync(OpenRequest request)
		{
			return Task<OpenResponse>.Run<OpenResponse>(() => { return Open(request); });
		}

		//-------------------------------------------
		// Close
		//-------------------------------------------
		public CloseResponse Close(CloseRequest request)
		{
			CloseResponse response = new CloseResponse();

			var service_request = new BrueBoxService.CloseRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;
			service_request.SessionID = request.SessionID;

			try
			{
				var service_response = ServiceClient.CloseOperation(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;
			}
			catch(Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<CloseResponse> CloseAsync(CloseRequest request)
		{
			return Task<CloseResponse>.Run<CloseResponse>(() => { return Close(request); });
		}

		//-------------------------------------------
		// Occupy
		//-------------------------------------------
		public OccupyResponse Occupy(OccupyRequest request)
		{
			OccupyResponse response = new OccupyResponse();

			var service_request = new BrueBoxService.OccupyRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;
			service_request.SessionID = request.SessionID;

			try
			{
				var service_response = ServiceClient.OccupyOperation(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;
			}
			catch(Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<OccupyResponse> OccupyAsync(OccupyRequest request)
		{
			return Task<OccupyResponse>.Run<OccupyResponse>(() => { return Occupy(request); });
		}

		//-------------------------------------------
		// Release
		//-------------------------------------------
		public ReleaseResponse Release(ReleaseRequest request)
		{
			ReleaseResponse response = new ReleaseResponse();

			var service_request = new BrueBoxService.ReleaseRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;
			service_request.SessionID = request.SessionID;

			try
			{
				var service_response = ServiceClient.ReleaseOperation(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;
			}
			catch(Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<ReleaseResponse> ReleaseAsync(ReleaseRequest request)
		{
			return Task<ReleaseResponse>.Run<ReleaseResponse>(() => { return Release(request); });
		}

		//-------------------------------------------
		// StartLogread
		//-------------------------------------------
		public StartLogreadResponse StartLogread(StartLogreadRequest request)
		{
			StartLogreadResponse response = new StartLogreadResponse();

			var service_request = new BrueBoxService.StartLogreadRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;
			service_request.SessionID = request.SessionID;

			try
			{
				var service_response = ServiceClient.StartLogreadOperation(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;
			}
			catch(Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<StartLogreadResponse> StartLogreadAsync(StartLogreadRequest request)
		{
			return Task<StartLogreadResponse>.Run<StartLogreadResponse>(() => { return StartLogread(request); });
		}

		//-------------------------------------------
		// StartDownload
		//-------------------------------------------
		public StartDownloadResponse StartDownload(StartDownloadRequest request)
		{
			StartDownloadResponse response = new StartDownloadResponse();

			var service_request = new BrueBoxService.StartDownloadRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;
			service_request.SessionID = request.SessionID;

			try
			{
				var service_response = ServiceClient.StartDownloadOperation(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;
			}
			catch(Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<StartDownloadResponse> StartDownloadAsync(StartDownloadRequest request)
		{
			return Task<StartDownloadResponse>.Run<StartDownloadResponse>(() => { return StartDownload(request); });
		}

		//-------------------------------------------
		// ReturnCash
		//-------------------------------------------
		public ReturnCashResponse ReturnCash(ReturnCashRequest request)
		{
			ReturnCashResponse response = new ReturnCashResponse();

			var service_request = new BrueBoxService.ReturnCashRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;
			service_request.SessionID = request.SessionID;

			service_request.Option = new BrueBoxService.ReturnCashOptionType();
			service_request.Option.type = request.Option.ToString();

			try
			{
				var service_response = ServiceClient.ReturnCashOperation(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;
			}
			catch(Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<ReturnCashResponse> ReturnCashAsync(ReturnCashRequest request)
		{
			return Task<ReturnCashResponse>.Run<ReturnCashResponse>(() => { return ReturnCash(request); });
		}

		//-------------------------------------------
		// EnableDenom
		//-------------------------------------------
		public EnableDenomResponse EnableDenom(EnableDenomRequest request)
		{
			EnableDenomResponse response = new EnableDenomResponse();

			var service_request = new BrueBoxService.EnableDenomRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;
			service_request.SessionID = request.SessionID;
			service_request.Cash = ConvertToSOAP_Cash(request.Cash);

			try
			{
				var service_response = ServiceClient.EnableDenomOperation(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;
			}
			catch(Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<EnableDenomResponse> EnableDenomAsync(EnableDenomRequest request)
		{
			return Task<EnableDenomResponse>.Run<EnableDenomResponse>(() => { return EnableDenom(request); });
		}

		//-------------------------------------------
		// DisableDenom
		//-------------------------------------------
		public DisableDenomResponse DisableDenom(DisableDenomRequest request)
		{
			DisableDenomResponse response = new DisableDenomResponse();

			var service_request = new BrueBoxService.DisableDenomRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;
			service_request.SessionID = request.SessionID;
			service_request.Cash = ConvertToSOAP_Cash(request.Cash);

			try
			{
				var service_response = ServiceClient.DisableDenomOperation(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;
			}
			catch(Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<DisableDenomResponse> DisableDenomAsync(DisableDenomRequest request)
		{
			return Task<DisableDenomResponse>.Run<DisableDenomResponse>(() => { return DisableDenom(request); });
		}

		//-------------------------------------------
		// PowerControl
		//-------------------------------------------
		public PowerControlResponse PowerControl(PowerControlRequest request)
		{
			PowerControlResponse response = new PowerControlResponse();

			var service_request = new BrueBoxService.PowerControlRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;
			service_request.SessionID = request.SessionID;

			service_request.Option = new BrueBoxService.PowerControlOptionType();
			service_request.Option.type = request.Option.ToString();

			try
			{
				var service_response = ServiceClient.PowerControlOperation(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;
			}
			catch(Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<PowerControlResponse> PowerControlAsync(PowerControlRequest request)
		{
			return Task<PowerControlResponse>.Run<PowerControlResponse>(() => { return PowerControl(request); });
		}

		//-------------------------------------------
		// AdjustTime
		//-------------------------------------------
		public AdjustTimeResponse AdjustTime(AdjustTimeRequest request)
		{
			AdjustTimeResponse response = new AdjustTimeResponse();

			var service_request = new BrueBoxService.AdjustTimeRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;
			service_request.SessionID = request.SessionID;

			if (request.Date != null)
			{
				service_request.Date = new BrueBoxService.AdjustTimeDateType();
				service_request.Date.year = request.Date.year.ToString();
				service_request.Date.month = request.Date.month.ToString();
				service_request.Date.day = request.Date.day.ToString();
			}

			if (request.Time != null)
			{
				service_request.Time = new BrueBoxService.AdjustTimeTimeType();
				service_request.Time.hour = request.Time.hour.ToString();
				service_request.Time.minute = request.Time.minute.ToString();
				service_request.Time.second = request.Time.second.ToString();
			}

			try
			{
				var service_response = ServiceClient.AdjustTimeOperation(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;
			}
			catch(Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<AdjustTimeResponse> AdjustTimeAsync(AdjustTimeRequest request)
		{
			return Task<AdjustTimeResponse>.Run<AdjustTimeResponse>(() => { return AdjustTime(request); });
		}

		//-------------------------------------------
		// UnLockUnit
		//-------------------------------------------
		public UnLockUnitResponse UnLockUnit(UnLockUnitRequest request)
		{
			UnLockUnitResponse response = new UnLockUnitResponse();

			var service_request = new BrueBoxService.UnLockUnitRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;
			service_request.SessionID = request.SessionID;

			service_request.Option = new BrueBoxService.UnLockUnitOptionType();
			service_request.Option.type = request.Option.ToString();

			try
			{
				var service_response = ServiceClient.UnLockUnitOperation(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;
			}
			catch(Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<UnLockUnitResponse> UnLockUnitAsync(UnLockUnitRequest request)
		{
			return Task<UnLockUnitResponse>.Run<UnLockUnitResponse>(() => { return UnLockUnit(request); });
		}

		//-------------------------------------------
		// LockUnit
		//-------------------------------------------
		public LockUnitResponse LockUnit(LockUnitRequest request)
		{
			LockUnitResponse response = new LockUnitResponse();

			var service_request = new BrueBoxService.LockUnitRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;
			service_request.SessionID = request.SessionID;

			service_request.Option = new BrueBoxService.LockUnitOptionType();
			service_request.Option.type = request.Option.ToString();

			try
			{
				var service_response = ServiceClient.LockUnitOperation(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;
			}
			catch(Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<LockUnitResponse> LockUnitAsync(LockUnitRequest request)
		{
			return Task<LockUnitResponse>.Run<LockUnitResponse>(() => { return LockUnit(request); });
		}

		//-------------------------------------------
		// OpenExitCover
		//-------------------------------------------
		public OpenExitCoverResponse OpenExitCover(OpenExitCoverRequest request)
		{
			OpenExitCoverResponse response = new OpenExitCoverResponse();

			var service_request = new BrueBoxService.OpenExitCoverRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;
			service_request.SessionID = request.SessionID;

			try
			{
				var service_response = ServiceClient.OpenExitCoverOperation(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;
			}
			catch(Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<OpenExitCoverResponse> OpenExitCoverAsync(OpenExitCoverRequest request)
		{
			return Task<OpenExitCoverResponse>.Run<OpenExitCoverResponse>(() => { return OpenExitCover(request); });
		}

		//-------------------------------------------
		// CloseExitCover
		//-------------------------------------------
		public CloseExitCoverResponse CloseExitCover(CloseExitCoverRequest request)
		{
			CloseExitCoverResponse response = new CloseExitCoverResponse();

			var service_request = new BrueBoxService.CloseExitCoverRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;
			service_request.SessionID = request.SessionID;

			try
			{
				var service_response = ServiceClient.CloseExitCoverOperation(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;
			}
			catch(Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<CloseExitCoverResponse> CloseExitCoverAsync(CloseExitCoverRequest request)
		{
			return Task<CloseExitCoverResponse>.Run<CloseExitCoverResponse>(() => { return CloseExitCover(request); });
		}

		//-------------------------------------------
		// SetExchangeRate
		//-------------------------------------------
		public SetExchangeRateResponse SetExchangeRate(SetExchangeRateRequest request)
		{
			SetExchangeRateResponse response = new SetExchangeRateResponse();

			var service_request = new BrueBoxService.SetExchangeRateRequestType();
			service_request.Id = request.Id;
			service_request.SeqNo = request.SeqNo;
			service_request.SessionID = request.SessionID;

			if (request.ExchangeRateSetting != null)
			{
				service_request.ExchangeRateSetting = new BrueBoxService.ExchangeRateType[request.ExchangeRateSetting.ExchangeRate.Count];
				for( int i = 0; i < request.ExchangeRateSetting.ExchangeRate.Count; i++)
				{
                    service_request.ExchangeRateSetting[i] = new BrueBoxService.ExchangeRateType();
					service_request.ExchangeRateSetting[i].Rate = request.ExchangeRateSetting.ExchangeRate[i].rate.ToString();
					service_request.ExchangeRateSetting[i].from = request.ExchangeRateSetting.ExchangeRate[i].from;
					service_request.ExchangeRateSetting[i].to = request.ExchangeRateSetting.ExchangeRate[i].to;
				}
			}

			try
			{
				var service_response = ServiceClient.SetExchangeRateOperation(service_request);
				response.result = int.Parse(service_response.result);
				response.Id = service_response.Id;
				response.SeqNo = service_response.SeqNo;
				response.User = service_response.User;
			}
			catch (Exception ex)
			{
				response.result = -1;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		public Task<SetExchangeRateResponse> SetExchangeRateAsync(SetExchangeRateRequest request)
		{
			return Task<SetExchangeRateResponse>.Run<SetExchangeRateResponse>(() => { return SetExchangeRate(request); });
		}

		//-------------------------------------------
		// ConvertToSOAP
		//-------------------------------------------
		private BrueBoxService.CashType ConvertToSOAP_Cash(Cash cash)
		{
			if (cash == null) return null;

			var s_cash = new BrueBoxService.CashType();

			s_cash.type = cash.type.ToString();
			s_cash.Denomination = ConvertToSOAP_DenominationList(cash.Denomination);

			return s_cash;
		}

		private BrueBoxService.CashType[] ConvertToSOAP_CashList(CashList cash_list)
		{
			var s_cash_list = new BrueBoxService.CashType[cash_list.Count];

			for (int i = 0; cash_list != null && i < cash_list.Count; i++)
			{
				s_cash_list[i] = ConvertToSOAP_Cash(cash_list[i]);
			}

			return s_cash_list;
		}

		private BrueBoxService.DenominationType[] ConvertToSOAP_DenominationList(DenominationList denom_list)
		{
			var s_denom_list = new BrueBoxService.DenominationType[denom_list.Count];

			for (int i = 0; denom_list != null && i < denom_list.Count; i++)
            {
                s_denom_list[i] = new BrueBoxService.DenominationType();
				s_denom_list[i].devid = denom_list[i].devid.ToString();
				s_denom_list[i].cc = denom_list[i].cc;
				s_denom_list[i].fv = denom_list[i].fv.ToString();
				s_denom_list[i].rev = denom_list[i].rev.ToString();
				s_denom_list[i].Piece = denom_list[i].Piece.ToString();
				s_denom_list[i].Status = denom_list[i].Status.ToString();
			}

			return s_denom_list;
		}

		private BrueBoxService.CurrencyType[] ConvertToSOAP_CurrencyList(CurrencyList currency_list)
		{
			var s_currency_list = new BrueBoxService.CurrencyType[currency_list.Count];

			for (int i = 0; currency_list != null && i < currency_list.Count; i++)
            {
                s_currency_list[i] = new BrueBoxService.CurrencyType();
				s_currency_list[i].type = currency_list[i].type.ToString();
				s_currency_list[i].cc = currency_list[i].cc;
				s_currency_list[i].fv = currency_list[i].fv.ToString();
				s_currency_list[i].piece = currency_list[i].piece.ToString();
			}

			return s_currency_list;
		}

		//-------------------------------------------
		// ConvertToFCC
		//-------------------------------------------
		private Cash ConvertToFCC_Cash(BrueBoxService.CashType cash)
		{
			if (cash == null) return null;

			var f_cash = new Cash();

			f_cash.type = int.Parse(cash.type);
			f_cash.Denomination = ConvertToFCC_DenominationList(cash.Denomination);

			return f_cash;
		}

		private CashList ConvertToFCC_CashList(BrueBoxService.CashType[] cash_list)
		{
			var f_cash_list = new CashList();

			for (int i = 0; cash_list != null && i < cash_list.Length; i++)
			{
				f_cash_list.Add(ConvertToFCC_Cash(cash_list[i]));
			}

			return f_cash_list;
		}

		private DenominationList ConvertToFCC_DenominationList(BrueBoxService.DenominationType[] denom_list)
		{
			var f_denom_list = new DenominationList();

			for (int i = 0; denom_list != null && i < denom_list.Length; i++)
			{
				var denom = new Denomination();

				denom.devid = int.Parse(denom_list[i].devid);
				denom.cc = denom_list[i].cc;
				denom.fv = int.Parse(denom_list[i].fv);
				denom.rev = int.Parse(denom_list[i].rev);
				denom.Piece = int.Parse(denom_list[i].Piece);
				denom.Status = int.Parse(denom_list[i].Status);

				f_denom_list.Add(denom);
			}

			return f_denom_list;
		}

		private CashUnitsList ConvertToFCC_CashUnitsList(BrueBoxService.CashUnitsType[] cashunits_list)
		{
			var f_cashunits_list = new CashUnitsList();

			for (int i = 0; cashunits_list != null && i < cashunits_list.Length; i++)
			{
				var cashunits = new CashUnits();

				cashunits.devid = int.Parse(cashunits_list[i].devid);
				cashunits.CashUnit = ConvertToFCC_CashUnitList(cashunits_list[i].CashUnit);

				f_cashunits_list.Add(cashunits);
			}

			return f_cashunits_list;
		}

		private CashUnitList ConvertToFCC_CashUnitList(BrueBoxService.CashUnitType[] cashunit_list)
		{
			var f_cashunit_list = new CashUnitList();

			for (int i = 0; cashunit_list != null && i < cashunit_list.Length; i++)
			{
				var cashunit = new CashUnit();

				cashunit.unitno = int.Parse(cashunit_list[i].unitno);
				cashunit.st = int.Parse(cashunit_list[i].st);
				cashunit.nf = int.Parse(cashunit_list[i].nf);
				cashunit.ne = int.Parse(cashunit_list[i].ne);
				cashunit.max = int.Parse(cashunit_list[i].max);
				cashunit.Denomination = ConvertToFCC_DenominationList(cashunit_list[i].Denomination);

				f_cashunit_list.Add(cashunit);
			}

			return f_cashunit_list;
		}

		private Status ConvertToFCC_Status(BrueBoxService.StatusType status)
		{
			var f_status = new Status();
			
			f_status.Code = int.Parse(status.Code);

			f_status.DevStatus = new DevStatusList();
			foreach (var dev_status in status.DevStatus)
			{
				var f_dev_status = new DevStatus();

				f_dev_status.devid = int.Parse(dev_status.devid);
				f_dev_status.st = int.Parse(dev_status.st);
				f_dev_status.val = int.Parse(dev_status.val);

				f_status.DevStatus.Add(f_dev_status);
			}

			return f_status;
		}

		private CurrencyList ConvertToFCC_CurrencyList(BrueBoxService.CurrencyType[] currency_list)
		{
			var f_currency_list = new CurrencyList();

			for (int i = 0; currency_list != null && i < currency_list.Length; i++)
			{
				var currency = new Currency();

				currency.type = int.Parse(currency_list[i].type);
				currency.cc = currency_list[i].cc;
				currency.fv = int.Parse(currency_list[i].fv);
				currency.piece = int.Parse(currency_list[i].piece);

				f_currency_list.Add(currency);
			}

			return f_currency_list;
		}

		private DepositDetails ConvertToFCC_DepositDetails(BrueBoxService.DepositDetailsType details)
		{
			var f_details = new DepositDetails();

			f_details.MainAmount = new MainAmount();
			f_details.MainAmount.cc = details.MainAmount.cc;
			f_details.MainAmount.Amount = int.Parse(details.MainAmount.Amount);

			f_details.ForeignCurrency = new ForeignCurrencyExchange();
			f_details.ForeignCurrency.cc = details.ForeignCurrency.cc;
			f_details.ForeignCurrency.rate = details.ForeignCurrency.rate;
			f_details.ForeignCurrency.PreAmount = int.Parse(details.ForeignCurrency.PreAmount);
			f_details.ForeignCurrency.ExchangedAmount = int.Parse(details.ForeignCurrency.ExchangedAmount);

			return f_details;
		}
	}
}
