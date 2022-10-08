using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FccInterfaceLib
{
	//-------------------------------------------
	// Json
	//-------------------------------------------
	public static class JsonUtility
	{
		public static object ReadJsonArray(JsonReader reader, Type type, JsonSerializer serializer)
		{
			object value = null;
			
			JContainer jContainer = JArray.Load(reader);
			value = Activator.CreateInstance(type);
			serializer.Populate(jContainer.CreateReader(), value);

			return value;
		}
	}

	public class ArrayConverter<T> : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteValue(value.ToString());
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.StartArray)
			{
				existingValue = JsonUtility.ReadJsonArray(reader, objectType, serializer);
				((List<T>)existingValue).RemoveAll((v) => { return v == null; });
			}

			return existingValue;
		}

		public override bool CanConvert(Type objectType)
		{
			return true;
		}
	}

	//-------------------------------------------
	// Common
	//-------------------------------------------
	public class Cash
	{
		public int type;
		public string coin_destination;
		public string note_destination;
		public DenominationList Denomination = new DenominationList();
	}

	[JsonConverter(typeof(ArrayConverter<Cash>))]
	public class CashList : List<Cash>
	{
	}

	public class Denomination
	{
		public int devid;
		public string cc;
		public int fv;
		public int rev;
		public int Piece;
		public int Status;
		public string SerialNo;
	}

	[JsonConverter(typeof(ArrayConverter<Denomination>))]
	public class DenominationList : List<Denomination>
	{
	}

	public class DepositCurrency
	{
		public CurrencyList Currency = new CurrencyList();
	}

	public class Currency
	{
		public int type;
		public string cc;
		public int fv;
		public int piece;
	}

	[JsonConverter(typeof(ArrayConverter<Currency>))]
	public class CurrencyList : List<Currency>
	{
	}

	public class DepositDetails
	{
		public MainAmount MainAmount;
		public ForeignCurrencyExchange ForeignCurrency;
	}

	public class MainAmount
	{
		public string cc;
		public int Amount;
	}

	public class ForeignAmount
	{
		public string cc;
		public int Amount;
	}

	public class ForeignCurrency
	{
		public string cc;
		public double rate;
	}

	public class ForeignCurrencyExchange
	{
		public string cc;
		public double rate;
		public int PreAmount;
		public int ExchangedAmount;
	}

	//-------------------------------------------
	// Base
	//-------------------------------------------
	public class BaseRequest
	{
		public string Id;
		public string SeqNo;
		public string SessionID;
	}

	public class BaseResponse
	{
		public int result;
		public string Id;
		public string SeqNo;
		public string User;

		public string ErrorMessage;
	}

	//-------------------------------------------
	// RegisterEvent
	//-------------------------------------------
	public class RegisterEventRequest : BaseRequest
	{
		public string Url;
		public int Port;

		public int DestinationType = FccConst.RegisterEvent.TYPE_DEST_POS;
		public int Encryption = FccConst.Option.TYPE_OFF;

		public List<int> RequireEventList;
	}

	public class RegisterEventResponse : BaseResponse
	{
	}

	//-------------------------------------------
	// UnRegisterEvent
	//-------------------------------------------
	public class UnRegisterEventRequest : BaseRequest
	{
		public string Url;
		public int Port;
	}

	public class UnRegisterEventResponse : BaseResponse
	{
	}

	//-------------------------------------------
	// GetStatus
	//-------------------------------------------
	public class StatusRequest : BaseRequest
	{
		public int Option = FccConst.Option.TYPE_OFF;
		public int RequireVerification = FccConst.Option.TYPE_OFF;
	}

	public class StatusResponse : BaseResponse
	{
		public Status Status;
		public CashList Cash;
		public RequireVerifyInfos RequireVerifyInfos;
	}

	public class DevStatus
	{
		public int devid;
		public int val;
		public int st;
	}

	[JsonConverter(typeof(ArrayConverter<DevStatus>))]
	public class DevStatusList : List<DevStatus>
	{
	}

	public class Status
	{
		public int Code;
		public DevStatusList DevStatus;
	}

	public class RequireVerifyInfos
	{
		public RequireVerifyDenominationInfos RequireVerifyDenominationInfos;
		public RequireVerifyCollectionContainerInfos RequireVerifyCollectionContainerInfos;
		public RequireVerifyMixStackerInfos RequireVerifyMixStackerInfos;
	}

	public class RequireVerifyDenominationInfos
	{
		public RequireVerifyDenominationList RequireVerifyDenomination;
	}

	public class RequireVerifyCollectionContainerInfos
	{
		public RequireVerifyCollectionContainerList RequireVerifyCollectionContainer;
	}

	public class RequireVerifyMixStackerInfos
	{
		public RequireVerifyMixStackerList RequireVerifyMixStacker;
	}

	public class RequireVerifyDenomination
	{
		public int devid;
		public int val;
		public CashList Cash;
	}

	[JsonConverter(typeof(ArrayConverter<RequireVerifyDenomination>))]
	public class RequireVerifyDenominationList : List<RequireVerifyDenomination>
	{
	}

	public class RequireVerifyCollectionContainer
	{
		public int devid;
		public int val;
		public string SerialNo;
	}

	[JsonConverter(typeof(ArrayConverter<RequireVerifyCollectionContainer>))]
	public class RequireVerifyCollectionContainerList : List<RequireVerifyCollectionContainer>
	{
	}

	public class RequireVerifyMixStacker
	{
		public int devid;
		public int val;
	}

	[JsonConverter(typeof(ArrayConverter<RequireVerifyMixStacker>))]
	public class RequireVerifyMixStackerList : List<RequireVerifyMixStacker>
	{
	}

	//-------------------------------------------
	// Inventory
	//-------------------------------------------
	public class InventoryRequest : BaseRequest
	{
		public int Option = FccConst.Inventory.TYPE_ALL;
	}

	public class InventoryResponse : BaseResponse
	{
		public string TransactionId;
		public CashList Cash;
		public CashUnitsList CashUnits;
		public string InvalidCassette;
	}

	public class CashUnits
	{
		public int devid;
		public CashUnitList CashUnit;
	}

	[JsonConverter(typeof(ArrayConverter<CashUnits>))]
	public class CashUnitsList : List<CashUnits>
	{
	}

	public class CashUnit
	{
		public int unitno;
		public int st;
		public int nf;
		public int ne;
		public int max;
		public DenominationList Denomination;

		public int getPiece()
		{
			int Piece = 0;

			for (int i = 0; i < Denomination.Count; i++)
			{
				Piece += Denomination[i].Piece;
			}

			return Piece;
		}
	}

	[JsonConverter(typeof(ArrayConverter<CashUnit>))]
	public class CashUnitList : List<CashUnit>
	{
	}

	//-------------------------------------------
	// Change
	//-------------------------------------------
	public class ChangeRequest : BaseRequest
	{
		public int Amount;
		public int Option = FccConst.Option.TYPE_OFF;
		public Cash Cash;
		public ForeignCurrency ForeignCurrency;
	}

	public class ChangeResponse : BaseResponse
	{
		public string TransactionId;
		public int Amount;
		public Status Status;
		public int ManualDeposit;
		public DepositDetails ManualDepositDetails;
		public DepositCurrency DepositCurrency;
		public CashList Cash;
		public DepositDetails CashInAmountDetails;
	}

	//-------------------------------------------
	// ChangeCancel
	//-------------------------------------------
	public class ChangeCancelRequest : BaseRequest
	{
		public int Option = FccConst.Option.TYPE_OFF;
	}

	public class ChangeCancelResponse : BaseResponse
	{
	}

	//-------------------------------------------
	// StartCashin
	//-------------------------------------------
	public class StartCashinRequest : BaseRequest
	{
		public int Option = FccConst.Option.TYPE_OFF;
		public ForeignCurrency ForeignCurrency;
	}

	public class StartCashinResponse : BaseResponse
	{
	}

	//-------------------------------------------
	// EndCashin
	//-------------------------------------------
	public class EndCashinRequest : BaseRequest
	{
	}

	public class EndCashinResponse : BaseResponse
	{
		public string TransactionId;
		public int ManualDeposit;
		public DepositDetails ManualDepositDetails;
		public DepositCurrency DepositCurrency;
		public CashList Cash;
		public DepositDetails CashInAmountDetails;
	}

	//-------------------------------------------
	// CashinCancel
	//-------------------------------------------
	public class CashinCancelRequest : BaseRequest
	{
	}

	public class CashinCancelResponse : BaseResponse
	{
		public string TransactionId;
		public int ManualDeposit;
		public DepositDetails ManualDepositDetails;
		public DepositCurrency DepositCurrency;
		public CashList Cash;
		public DepositDetails CashInAmountDetails;
	}

	//-------------------------------------------
	// UpdateManualDepositTotal
	//-------------------------------------------
	public class UpdateManualDepositTotalRequest : BaseRequest
	{
		public int Amount;
		public ForeignAmount ForeignAmount;
		public DepositCurrency DepositCurrency;
	}

	public class UpdateManualDepositTotalResponse : BaseResponse
	{
	}

	//-------------------------------------------
	// StartReplenishmentFromEntrance
	//-------------------------------------------
	public class StartReplenishmentFromEntranceRequest : BaseRequest
	{
	}

	public class StartReplenishmentFromEntranceResponse : BaseResponse
	{
	}

	//-------------------------------------------
	// EndReplenishmentFromEntrance
	//-------------------------------------------
	public class EndReplenishmentFromEntranceRequest : BaseRequest
	{
	}

	public class EndReplenishmentFromEntranceResponse : BaseResponse
	{
		public string TransactionId;
		public int ManualDeposit;
		public DepositCurrency DepositCurrency;
		public CashList Cash;
	}

	//-------------------------------------------
	// ReplenishmentFromEntranceCancel
	//-------------------------------------------
	public class ReplenishmentFromEntranceCancelRequest : BaseRequest
	{
	}

	public class ReplenishmentFromEntranceCancelResponse : BaseResponse
	{
		public string TransactionId;
		public int ManualDeposit;
		public DepositCurrency DepositCurrency;
		public CashList Cash;
	}

	//-------------------------------------------
	// StartReplenishmentFromCassette
	//-------------------------------------------
	public class StartReplenishmentFromCassetteRequest : BaseRequest
	{
	}

	public class StartReplenishmentFromCassetteResponse : BaseResponse
	{
	}

	//-------------------------------------------
	// EndReplenishmentFromCassette
	//-------------------------------------------
	public class EndReplenishmentFromCassetteRequest : BaseRequest
	{
	}

	public class EndReplenishmentFromCassetteResponse : BaseResponse
	{
		public string TransactionId;
		public CashList Cash;
	}

	//-------------------------------------------
	// RefreshSalesTotal
	//-------------------------------------------
	public class RefreshSalesTotalRequest : BaseRequest
	{
		public int Amount;
	}

	public class RefreshSalesTotalResponse : BaseResponse
	{
	}

	//-------------------------------------------
	// Cashout
	//-------------------------------------------
	public class CashoutRequest : BaseRequest
	{
		public int Delay;
		public Cash Cash;
	}

	public class CashoutResponse : BaseResponse
	{
		public string TransactionId;
		public CashList Cash;
	}

	//-------------------------------------------
	// Collect
	//-------------------------------------------
	public class CollectRequest : BaseRequest
	{
		public int Option = FccConst.Collect.TYPE_OPTION_CASSETTE;
		public int RequireVerification = FccConst.Option.TYPE_OFF;
		public int IFCassette = FccConst.Option.TYPE_OFF;
		public int Mix = FccConst.Collect.TYPE_MIX_OFF;
		public int Partial = FccConst.Collect.TYPE_PARTIAL_OFF;
		public int COFBClearOption = FccConst.Option.TYPE_OFF;
		public Cash Cash;
	}

	public class CollectResponse : BaseResponse
	{
		public string TransactionId;
		public CashList Cash;
	}

	//-------------------------------------------
	// Reset
	//-------------------------------------------
	public class ResetRequest : BaseRequest
	{
	}

	public class ResetResponse : BaseResponse
	{
	}

	//-------------------------------------------
	// LoginUser
	//-------------------------------------------
	public class LoginUserRequest : BaseRequest
	{
		public string User;
	}

	public class LoginUserResponse : BaseResponse
	{
	}

	//-------------------------------------------
	// LogoutUser
	//-------------------------------------------
	public class LogoutUserRequest : BaseRequest
	{
	}

	public class LogoutUserResponse : BaseResponse
	{
	}

	//-------------------------------------------
	// StartLogread
	//-------------------------------------------
	public class StartLogreadRequest : BaseRequest
	{
	}

	public class StartLogreadResponse : BaseResponse
	{
	}

	//-------------------------------------------
	// StartDownload
	//-------------------------------------------
	public class StartDownloadRequest : BaseRequest
	{
	}

	public class StartDownloadResponse : BaseResponse
	{
	}

	//-------------------------------------------
	// Open
	//-------------------------------------------
	public class OpenRequest : BaseRequest
	{
		public string User;
		public string UserPwd;
	}

	public class OpenResponse : BaseResponse
	{
		public string SessionID;
	}

	//-------------------------------------------
	// Close
	//-------------------------------------------
	public class CloseRequest : BaseRequest
	{
	}

	public class CloseResponse : BaseResponse
	{
	}

	//-------------------------------------------
	// Occupy
	//-------------------------------------------
	public class OccupyRequest : BaseRequest
	{
	}

	public class OccupyResponse : BaseResponse
	{
	}

	//-------------------------------------------
	// Release
	//-------------------------------------------
	public class ReleaseRequest : BaseRequest
	{
	}

	public class ReleaseResponse : BaseResponse
	{
	}

	//-------------------------------------------
	// ReturnCash
	//-------------------------------------------
	public class ReturnCashRequest : BaseRequest
	{
		public int Option;
	}

	public class ReturnCashResponse : BaseResponse
	{
	}

	//-------------------------------------------
	// EnableDenom
	//-------------------------------------------
	public class EnableDenomRequest : BaseRequest
	{
		public Cash Cash;
	}

	public class EnableDenomResponse : BaseResponse
	{
	}

	//-------------------------------------------
	// DisableDenom
	//-------------------------------------------
	public class DisableDenomRequest : BaseRequest
	{
		public Cash Cash;
	}

	public class DisableDenomResponse : BaseResponse
	{
	}

	//-------------------------------------------
	// PowerControl
	//-------------------------------------------
	public class PowerControlRequest : BaseRequest
	{
		public int Option;
	}

	public class PowerControlResponse : BaseResponse
	{
	}

	//-------------------------------------------
	// AdjustTime
	//-------------------------------------------
	public class AdjustTimeRequest : BaseRequest
	{
		public Date Date;
		public Time Time;
	}

	public class AdjustTimeResponse : BaseResponse
	{
	}

	public class Date
	{
		public int month;
		public int day;
		public int year;
	}

	public class Time
	{
		public int hour;
		public int minute;
		public int second;
	}

	//-------------------------------------------
	// UnLockUnit
	//-------------------------------------------
	public class UnLockUnitRequest : BaseRequest
	{
		public int Option;
	}

	public class UnLockUnitResponse : BaseResponse
	{
	}

	//-------------------------------------------
	// LockUnit
	//-------------------------------------------
	public class LockUnitRequest : BaseRequest
	{
		public int Option;
	}

	public class LockUnitResponse : BaseResponse
	{
	}

	//-------------------------------------------
	// OpenExitCover
	//-------------------------------------------
	public class OpenExitCoverRequest : BaseRequest
	{
	}

	public class OpenExitCoverResponse : BaseResponse
	{
	}

	//-------------------------------------------
	// CloseExitCover
	//-------------------------------------------
	public class CloseExitCoverRequest : BaseRequest
	{
	}

	public class CloseExitCoverResponse : BaseResponse
	{
	}

	//-------------------------------------------
	// SetExchangeRate
	//-------------------------------------------
	public class SetExchangeRateRequest : BaseRequest
	{
		public ExchangeRateSetting ExchangeRateSetting;
	}

	public class SetExchangeRateResponse : BaseResponse
	{
	}

	public class ExchangeRateSetting
	{
		public ExchangeRateList ExchangeRate = new ExchangeRateList();
	}

	public class ExchangeRate
	{
		public string from;
		public string to;
		public double rate;
	}

	[JsonConverter(typeof(ArrayConverter<ExchangeRate>))]
	public class ExchangeRateList : List<ExchangeRate>
	{
	}

	//-------------------------------------------
	// FCC Event
	//-------------------------------------------
	public class FCCEvent
	{
		public RegisterEventResponse RegisterEventResponse;
		public UnRegisterEventResponse UnRegisterEventResponse;
		public StatusResponse StatusResponse;
		public InventoryResponse InventoryResponse;
		public StartCashinResponse StartCashinResponse;
		public CashinCancelResponse CashinCancelResponse;
		public EndCashinResponse EndCashinResponse;
		public ChangeResponse ChangeResponse;
		public ChangeCancelResponse ChangeCancelResponse;
		public UpdateManualDepositTotalResponse UpdateManualDepositTotalResponse;
		public RefreshSalesTotalResponse RefreshSalesTotalResponse;
		public StartReplenishmentFromEntranceResponse StartReplenishmentFromEntranceResponse;
		public ReplenishmentFromEntranceCancelResponse ReplenishmentFromEntranceCancelResponse;
		public EndReplenishmentFromEntranceResponse EndReplenishmentFromEntranceResponse;
		public StartReplenishmentFromCassetteResponse StartReplenishmentFromCassetteResponse;
		public EndReplenishmentFromCassetteResponse EndReplenishmentFromCassetteResponse;
		public CashoutResponse CashoutResponse;
		public CollectResponse CollectResponse;
		public ResetResponse ResetResponse;
		public LoginUserResponse LoginUserResponse;
		public LogoutUserResponse LogoutUserResponse;
		public StartLogreadResponse StartLogreadResponse;
		public StartDownloadResponse StartDownloadResponse;
		public OpenResponse OpenResponse;
		public CloseResponse CloseResponse;
		public OccupyResponse OccupyResponse;
		public ReleaseResponse ReleaseResponse;
		public ReturnCashResponse ReturnCashResponse;
		public EnableDenomResponse EnableDenomResponse;
		public DisableDenomResponse DisableDenomResponse;
		public PowerControlResponse PowerControlResponse;
		public AdjustTimeResponse AdjustTimeResponse;
		public UnLockUnitResponse UnLockUnitResponse;
		public LockUnitResponse LockUnitResponse;
		public OpenExitCoverResponse OpenExitCoverResponse;
		public CloseExitCoverResponse CloseExitCoverResponse;
		public SetExchangeRateResponse SetExchangeRateResponse;

		public HeartBeatEvent HeartBeatEvent;
		public StatusChangeEvent StatusChangeEvent;
		public AmountDetailsEvent AmountDetailsEvent;
		public DispenseDetailsEvent DispenseDetailsEvent;
		public ChangeInventoryStatus ChangeInventoryStatus;
		public SpecialDeviceError SpecialDeviceError;
		public IncompleteTransaction IncompleteTransaction;
	}

	public class HeartBeatEvent
	{
		public string SerialNo;
	}

	public class StatusChangeEvent
	{
		public int Status;
		public int Amount;
		public int Error;
		public string RecoveryURL;
		public string User;
		public string SeqNo;
	}

	public class AmountDetailsEvent
	{
		public MainAmountAmountDetails MainAmount;
		public ForeignCurrencyAmountDetails ForeignCurrency;
	}

	public class MainAmountAmountDetails
	{
		public string cc;
		public int MainAmount;
	}

	public class ForeignCurrencyAmountDetails
	{
		public string cc;
		public double rate;
		public int PreAmount;
		public int ExchangedAmount;
	}

	public class DispenseDetailsEvent
	{
		public DenominationList Denomination;
	}

	public class ChangeInventoryStatus
	{
		public string CausedTransaction;
		public CashList Cash;
		public OptionUnitList OptionUnit;
	}

	public class OptionUnit
	{
		public string type;
		public int devid;
		public string Status;
	}

	[JsonConverter(typeof(ArrayConverter<OptionUnit>))]
	public class OptionUnitList : List<OptionUnit>
	{
	}

	public class SpecialDeviceError
	{
		public int devid;
		public int ErrorCode;
		public string ErrorMessage;
	}

	public class IncompleteTransaction
	{
		public string TransactionId;
		public string TransactionName;
		public Committed Committed;
		public Current Current;
		public ManualDepositInfo ManualDepositInfo;
		public DepositCategoryInfo DepositCategoryInfo;
	}

	public class Committed
	{
		public CashList Cash;
	}

	public class Current
	{
		public CashList Cash;
	}

	public class ManualDepositInfo
	{
		DepositCurrency DepositCurrency;
	}

	public class DepositCategoryInfo
	{
		public CashList Cash;
	}

	//-------------------------------------------
	// Device Event
	//-------------------------------------------
	public class DeviceEvent
	{
		public int devid;
		public eventStatusChange eventStatusChange;
		public eventWaitForRemoving eventWaitForRemoving;
		public eventRemoved eventRemoved;
		public eventCassetteInserted eventCassetteInserted;
		public eventCassetteChecked eventCassetteChecked;
		public eventCassetteInventoryOnRemoval eventCassetteInventoryOnRemoval;
		public eventCassetteInventoryOnInsertion eventCassetteInventoryOnInsertion;
		public eventEmpty eventEmpty;
		public eventLow eventLow;
		public eventExist eventExist;
		public eventHigh eventHigh;
		public eventFull eventFull;
		public eventMissing eventMissing;
		public eventDepositCountChange eventDepositCountChange;
		public eventDepositCountMonitor eventDepositCountMonitor;
		public eventReplenishCountChange eventReplenishCountChange;
		public eventError eventError;
		public eventDownloadProgress eventDownloadProgress;
		public eventLogreadProgress eventLogreadProgress;
		public eventRequireVerifyDenomination eventRequireVerifyDenomination;
		public eventRequireVerifyCollectionContainer eventRequireVerifyCollectionContainer;
		public eventRequireVerifyMixStacker eventRequireVerifyMixStacker;
		public eventExactDenomination eventExactDenomination;
		public eventExactCollectionContainer eventExactCollectionContainer;
		public eventExactMixStacker eventExactMixStacker;
		public eventWaitForOpening eventWaitForOpening;
		public eventOpened eventOpened;
		public eventClosed eventClosed;
		public eventLocked eventLocked;
		public eventWaitForInsertion eventWaitForInsertion;
		public eventDepositCategoryInfo eventDepositCategoryInfo;
		public eventCountedCategory2 eventCountedCategory2;
		public eventCountedCategory3 eventCountedCategory3;
	}

	public class eventStatusChange
	{
		public int DeviceStatusID;
	}

	public class eventWaitForRemoving
	{
		public int DevicePositionID;
	}

	public class eventRemoved
	{
		public int DevicePositionID;
		public string InvalidCassette;
	}

	public class eventCassetteInserted
	{
		public string SerialNo;
		public string InvalidCassette;
	}

	public class eventCassetteChecked
	{
		public int ReasonCode;
		public string SerialNo;
	}

	public class eventCassetteInventoryOnRemoval
	{
		public string SerialNo;
		public CashList Cash;
		public string InvalidCassette;
	}

	public class eventCassetteInventoryOnInsertion
	{
		public string SerialNo;
		public CashList Cash;
		public string InvalidCassette;
	}

	public class eventEmpty
	{
		public int DevicePositionID;
	}

	public class eventLow
	{
		public int DevicePositionID;
	}

	public class eventExist
	{
		public int DevicePositionID;
	}

	public class eventHigh
	{
		public int DevicePositionID;
	}

	public class eventFull
	{
		public int DevicePositionID;
	}

	public class eventMissing
	{
		public int DevicePositionID;
	}

	public class eventDepositCountChange
	{
		public DenominationList Denomination;
	}

	public class eventDepositCountMonitor
	{
		public DenominationList Denomination;
	}

	public class eventReplenishCountChange
	{
		public DenominationList Denomination;
	}

	public class eventError
	{
		public string ErrorCode;
		public string RecoveryURL;
	}

	public class eventDownloadProgress
	{
		public int totalsize;
		public int sentsize;
	}

	public class eventLogreadProgress
	{
		public int totalsize;
		public int sentsize;
	}

	public class eventRequireVerifyDenomination
	{
		public CashList Cash;
	}

	public class eventRequireVerifyCollectionContainer
	{
		public string SerialNo;
	}

	public class eventRequireVerifyMixStacker
	{
	}

	public class eventExactDenomination
	{
	}

	public class eventExactCollectionContainer
	{
		public string SerialNo;
	}

	public class eventExactMixStacker
	{
	}

	public class eventWaitForOpening
	{
		public int DevicePositionID;
	}

	public class eventOpened
	{
		public int DevicePositionID;
	}

	public class eventClosed
	{
		public int DevicePositionID;
	}

	public class eventLocked
	{
		public int DevicePositionID;
	}

	public class eventWaitForInsertion
	{
		public int DevicePositionID;
	}

	public class eventDepositCategoryInfo
	{
		public string TransactionId;
		public CashList Cash;
	}

	public class eventCountedCategory2 
	{
	}

	public class eventCountedCategory3
	{
	}
}
