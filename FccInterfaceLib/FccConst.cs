using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FccInterfaceLib
{
	public class FccConst
	{
		public static class Currency
		{
			public const string USD = "USD";
			public const string JPY = "JPY";
			public const string UAH = "UAH";
			public const string EUR = "EUR";
			public const string GBP = "GBP";
			public const string RUB = "RUB";
		}

		public static class DeviceName
		{
			public const string RBW100 = "RBW100";
			public const string RBW50 = "RBW50";
			public const string RCW100 = "RCW100";
			public const string RCW50 = "RCW50";
			public const string RBW150 = "RBW150";
			public const string RZ50 = "RZ50";
			public const string RBG200 = "RBG200";
		}

		public static class Devid
		{
			public const int NONE = -1;
			public const int ALL = 0;
			public const int BILL = 1;
			public const int COIN = 2;
		}

		public static class DeviceUnitNo
		{
			public const int STCK1 = 4043;
			public const int STCK2 = 4044;
			public const int STCK3 = 4045;
			public const int STCK4 = 4046;
			public const int STCK5 = 4047;
			public const int STCK6 = 4048;
			public const int STCK7 = 4054;
			public const int STCK8 = 4055;
			public const int CBOXC1 = 4056;
			public const int CBOXC2 = 4057;
			public const int CBOXC3 = 4058;
			public const int CBOXC4A = 4059;
			public const int CBOXC4B = 4060;
			public const int STCK1C3C4B = 4061;
			public const int STCK2C3C4B = 4062;
			public const int STCK3C3C4B = 4063;
			public const int CBIN = 4069;
			public const int COFB = 4084;
			public const int UBOX1C1 = 4108;
			public const int UBOX1C2 = 4109;
			public const int UBOX1C3 = 4110;
			public const int UBOX1C4A = 4111;
			public const int UBOX1C4B = 4112;
			public const int UBOX2C1 = 4113;
			public const int UBOX2C2 = 4114;
			public const int UBOX2C3 = 4115;
			public const int UBOX2C4A = 4116;
			public const int UBOX2C4B = 4117;
			public const int MIXSTCK1 = 4118;
			public const int MIXSTCK2 = 4119;
			public const int MIXSTCK3 = 4120;
			public const int MIXSTCK4 = 4121;
			public const int MIXSTCK5 = 4122;
			public const int MIXSTCK6 = 4123;
			public const int CMIXSTCK = 4165;
		}

		public static class DevicePositionId
		{
			public const int ERROR = 0;
			public const int ENTRANCE = 1;
			public const int EXIT = 2;
			public const int ESCROW = 27;
			public const int COLLECTION_BOX = 21;

			public const int CPS1_COUNTER = 3;
			public const int CPS2_COUNTER = 4;
			public const int CPS3_COUNTER = 5;
			public const int CPS4_COUNTER = 6;
			public const int CPS5_COUNTER = 7;
			public const int CPS6_COUNTER = 8;
			public const int CPS7_COUNTER = 9;
			public const int CPS8_COUNTER = 10;
			public const int CPS9_COUNTER = 28;
			public const int CPS10_COUNTER = 29;

			public const int CPS1_C2_COUNTER = 49;
			public const int CPS2_C2_COUNTER = 50;
			public const int CPS3_C2_COUNTER = 51;
			public const int CPS4_C2_COUNTER = 52;
			public const int CPS5_C2_COUNTER = 53;
			public const int CPS6_C2_COUNTER = 54;
			public const int CPS7_C2_COUNTER = 55;
			public const int CPS8_C2_COUNTER = 56;
			public const int CPS9_C2_COUNTER = 57;
			public const int CPS10_C2_COUNTER = 58;

			public const int CPS1_C3_COUNTER = 16;
			public const int CPS2_C3_COUNTER = 17;
			public const int CPS3_C3_COUNTER = 18;
			public const int CPS4_C3_COUNTER = 22;
			public const int CPS5_C3_COUNTER = 23;
			public const int CPS6_C3_COUNTER = 24;
			public const int CPS7_C3_COUNTER = 25;
			public const int CPS8_C3_COUNTER = 26;
			public const int CPS9_C3_COUNTER = 30;
			public const int CPS10_C3_COUNTER = 31;

			public const int CPS1_C4B_COUNTER = 32;
			public const int CPS2_C4B_COUNTER = 33;
			public const int CPS3_C4B_COUNTER = 34;
			public const int CPS4_C4B_COUNTER = 35;
			public const int CPS5_C4B_COUNTER = 36;
			public const int CPS6_C4B_COUNTER = 37;
			public const int CPS7_C4B_COUNTER = 38;
			public const int CPS8_C4B_COUNTER = 39;
			public const int CPS9_C4B_COUNTER = 40;
			public const int CPS10_C4B_COUNTER = 41;

			public const int CONTAINER_C1_COUNTER = 11;
			public const int CONTAINER_C2_COUNTER = 12;
			public const int CONTAINER_C3_COUNTER = 13;
			public const int CONTAINER_C4A_COUNTER = 14;
			public const int CONTAINER_C4B_COUNTER = 15;

			public const int CAPTUREBIN_COUNTER = 19;
			public const int CONTAINER_FIT_COUNTER = 20;

			public const int RCW100_JAM_DOOR = 42;	
			public const int RCW100_TRANSPORT_UNIT = 43;
			public const int RCW100_ENTRANCE_UNIT = 44;
			public const int RCW100_OVER_FLOW_BOX = 73;
			public const int RCW100_MIX_STACKER = 74;

			public const int RBW100_MAINTE_DOOR = 45;
			public const int RBW100_UPPER_DOOR = 46;
			public const int RBW100_IF_CASETTE = 47;
			public const int RBW100_STACK_CASETTE = 48;

			public const int SDRB100_RJB_COUNTER = 59;

			public const int UPPER_UNIT = 64;
			public const int LOWER_UNIT = 65;

			public const int SDRB100_LOWER_DOOR = 66;
			public const int SDRB100_BAG = 67;
			public const int SDRB100_BAG_TOP = 68;
			public const int SDRB100_BAG_SEAL = 69;
			public const int SDRB100_IFCASSETTE = 70;

			public const int SDRC100_OVERFLOW_BOX = 71;
			public const int SDRC100_RETURN_BOX = 72;
			public const int SDRC100_DISPENSE_DRAWER = 73;
			public const int EXIT_REJECT = 74;

			public const int RBW150_UPPERUNIT_BOX1 = 75;
			public const int RBW150_UPPERUNIT_BOX2 = 76;
			public const int EXIT_SHUTTER = 77;

			public const int RBW50_MAIN_UNIT = 78;
			public const int RCW50_MAIN_UNIT = 79;
		}

		public static class DeviceDoorId
		{
			public const int LOWER_DOOR = 1;
			public const int MAINTE_DOOR = 2;
			public const int UPPER_DOOR	= 3;
			public const int JAM_DOOR = 4;
		}

		public static class CashUnitStatus
		{
			public const int Empty = 0;
			public const int LOW = 1;
			public const int EXIST = 2;
			public const int NEARFULL = 3;
			public const int FULL = 4;
			public const int NA = 22;
		}

		public static class Option
		{
			public const int TYPE_OFF = 0;
			public const int TYPE_ON = 1;
		}

		public static class TargetDevice
		{
			public const int TYPE_BOTH = 0;
			public const int TYPE_BILL = 1;
			public const int TYPE_COIN = 2;
		}

		public static class RegisterEvent
		{
			public const int TYPE_DEST_POS = 0;
			public const int TYPE_DEST_SERVER = 1;
		}

		public static class Inventory
		{
			public const int TYPE_ALL = 0;
			public const int TYPE_DEVICE = 1;
			public const int TYPE_PAY_AMOUNT = 2;
			public const int TYPE_NUM_STACKER = 3;
		}

		public static class Collect
		{
			public const int TYPE_OPTION_CASSETTE = 0;
			public const int TYPE_OPTION_EXIT = 1;
			public const int TYPE_PARTIAL_OFF = 0;
			public const int TYPE_PARTIAL_CASH = 1;
			public const int TYPE_PARTIAL_FILE = 2;
			public const int TYPE_MIX_OFF = 0;
			public const int TYPE_MIX_COIN = 1;
			public const int TYPE_MIX_BILL = 2;
			public const int TYPE_MIX_BOTH = 3;
		}

		public static class PowerControl
		{
			public const int TYPE_SHUTDOWN = 0;
			public const int TYPE_REBOOT = 1;
		}

		public static class CashType
		{
			public const int CASH_IN = 1;
			public const int CASH_OUT = 2;
			public const int MACHINE_INV = 3;
			public const int DISPENSABLE_INV = 4;
			public const int DENOMI_CONTROL = 5;
			public const int PAYMENT_DENOMI = 6;
			public const int CASH_OUT_COFB = 8;
			public const int CASH_OUT_MIX = 9;
			public const int IF_CASSETTE = 10;
			public const int VERIFY_IF_CASSETTE = 11;
		}

		public static class CurrencyType
		{
			public const int TYPE_CURRENCY_OTHER = 0;
			public const int TYPE_CURRENCY_NOTE = 1;
			public const int TYPE_CURRENCY_COIN = 2;
			public const int TYPE_CURRENCY_CREDIT = 3;
			public const int TYPE_CURRENCY_CHECK = 4;
			public const int TYPE_CURRENCY_COUPON = 5;
		}

		public static class FccStatus
		{
			public const int INITIALIZING = 0;
			public const int IDLE = 1;
			public const int AT_STARTING_CHANGE = 2;
			public const int WAITING_INSERTION_OF_CASH = 3;
			public const int COUNTING = 4;
			public const int DISPENSING = 5;
			public const int WAITING_REMOVAL_OF_CASH_IN_REJECT = 6;
			public const int WAITING_REMOVAL_OF_CASH_OUT_REJECT = 7;
			public const int RESETTING = 8;
			public const int CANCELING_CHANGE = 9;
			public const int CALCULATING_CHANGE_AMOUNT = 10;
			public const int CANCELING_DEPOSIT = 11;
			public const int COLLECTING = 12;
			public const int ERROR = 13;
			public const int UPLOAD_FIRMWARE = 14;
			public const int READING_LOG = 15;
			public const int WAITING_REFILL = 16;
			public const int COUNTING_REFILL = 17;
			public const int UNLOCKING = 18;
			public const int WAINTING_INVENTORY = 19;
			public const int AT_FIX_DEPOSIT_AMOUNT = 20;
			public const int AT_FIX_DISPENSE_AMOUNT = 21;
			public const int WAITING_DISPENSE = 22;
			public const int WAITING_CHANGE_CANCEL = 23;
			public const int COUNTED_CATEOGORY2_NOTE = 24;
			public const int WAITING_DEPOSIT_END = 25;
			public const int WAITING_REMOVAL_OF_COFB = 26;
			public const int WAITING_ERROR_RECOVERY = 30;
		}

		public static class DeviceStatus
		{
			public const int INITIALIZE = 0;
			public const int IDLE = 1000;
			public const int IDLE_OCCUPY = 1500;
			public const int DEPOSIT_BUSY = 2000;
			public const int DEPOSIT_COUNTING = 2050;
			public const int DEPOSIT_END = 2055;
			public const int WAIT_STORE = 2100;
			public const int STORE_BUSY = 2200;
			public const int STORE_END = 2300;
			public const int WAIT_RETURN = 2500;
			public const int COUNT_BUSY = 2600;
			public const int COUNT_COUNTING = 2610;
			public const int REPLENISH_BUSY = 2700;
			public const int DISPENSE_BUSY = 3000;
			public const int WAIT_DISPENSE = 3100;
			public const int REFILL = 4000;
			public const int REFILL_COUNTING = 4050;
			public const int REFILL_END = 4055;
			public const int RESET = 5000;
			public const int COLLECT_BUSY = 6000;
			public const int VERIFY_BUSY = 6500;
			public const int VERIFYCOLLECT_BUSY = 6600;
			public const int CLEAR_CASH_UNITS = 7000;
			public const int INVENTORY_ADJUST = 7100;
			public const int DOWNLOAD_BUSY = 8000;
			public const int LOG_READ_BUSY = 8100;
			public const int BUSY = 9100;
			public const int ERROR = 9200;
			public const int COM_ERROR = 9300;
			public const int WAIT_FOR_RESET = 9400;
			public const int CONFIG_ERROR = 9500;
			public const int LOCKED_BY_OTHER_SESSION = 50000;
		}

		public static class DeviceEventStatus
		{
			public const int INTERNAL_ERROR = 0;
			public const int IDLE = 1;
			public const int COUNTING = 2;
			public const int USING_OWN = 3;
			public const int BUSY = 4;
			public const int ERROR = 5;
			public const int ERROR_COMMUNICATION = 6;
			public const int DLL_INITIALIZE_BUSY = 7;
		}

		public static class ResultCode
		{
			public const int SUCCESS = 0;
			public const int CANCEL = 1;
			public const int RESET = 2;
			public const int OCCUPY = 3;
			public const int OCCUPY_OFF = 4;
			public const int NOT_OCCUPY = 5;
			public const int DESIGNATION_SHORTAGE_CASH = 6;
			public const int CANCEL_SHORTAGE_CASH = 9;
			public const int SHORTAGE_CASH = 10;
			public const int EXCLUCIVE_ERROR = 11;
			public const int DISCORD_CASH = 12;
			public const int RECOVERY_ERROR = 13;
			public const int INVALID_USER = 15;
			public const int SESSION_OVERFLOW = 16;
			public const int NOT_OCCUPY_BY_ITSEIF = 17;
			public const int SESSION_OFF = 20;
			public const int INVALID_SESSION = 21;
			public const int SESSION_TIMEOUT = 22;
			public const int MANUALCASH_MISMATCH = 26;
			public const int REQUIRE_VERIFY = 32;
			public const int INVALID_REFILL_CASH = 33;
			public const int SHORTAGE_STACKER = 34;
			public const int SERVER_COM_ERROR = 35;
			public const int EVENTDIST_OVERFLOW = 36;
			public const int INVALID_CASSETTE_NO = 40;
			public const int IMPROPER_CASSETTE = 41;
			public const int NOTHING_EVENT = 42;
			public const int NOTHING_EXCHANGE_RATE = 43;
			public const int COUNTED_CATEGORY2 = 44;
			public const int COLLECT_UPPERLIMIT_OVER = 45;
			public const int DUPLICATE_TRANSACTION = 96;
			public const int INVALID_DENOMINATION = 97;
			public const int ERROR_PARAMETER = 98;
			public const int ERROR_PROGRAM = 99;
			public const int ERROR_DLL = 100;
		}

		public static class InternalSetting
		{
			public const string ENCRYPTION_KEY = "glory";
		}
	}
}
