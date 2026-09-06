using System;
using System.Runtime.InteropServices;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace EDSDKLib
{

public class EDSDK
{
	public delegate uint EdsProgressCallback(uint inPercent, IntPtr inContext, ref bool outCancel);

	public delegate uint EdsCameraAddedHandler(IntPtr inContext);

	public delegate uint EdsPropertyEventHandler(uint inEvent, uint inPropertyID, uint inParam, IntPtr inContext);

	public delegate uint EdsObjectEventHandler(uint inEvent, IntPtr inRef, IntPtr inContext);

	public delegate uint EdsStateEventHandler(uint inEvent, uint inParameter, IntPtr inContext);

	public enum EdsDataType : uint
	{
		Unknown = 0u,
		Bool = 1u,
		String = 2u,
		Int8 = 3u,
		UInt8 = 6u,
		Int16 = 4u,
		UInt16 = 7u,
		Int32 = 8u,
		UInt32 = 9u,
		Int64 = 10u,
		UInt64 = 11u,
		Float = 12u,
		Double = 13u,
		ByteBlock = 14u,
		Rational = 20u,
		Point = 21u,
		Rect = 22u,
		Time = 23u,
		Bool_Array = 30u,
		Int8_Array = 31u,
		Int16_Array = 32u,
		Int32_Array = 33u,
		UInt8_Array = 34u,
		UInt16_Array = 35u,
		UInt32_Array = 36u,
		Rational_Array = 37u,
		FocusInfo = 101u,
		PictureStyleDesc = 102u
	}

	public enum EdsEvfAf : uint
	{
		CameraCommand_EvfAf_OFF,
		CameraCommand_EvfAf_ON
	}

	public enum EdsShutterButton : uint
	{
		CameraCommand_ShutterButton_OFF = 0u,
		CameraCommand_ShutterButton_Halfway = 1u,
		CameraCommand_ShutterButton_Completely = 3u,
		CameraCommand_ShutterButton_Halfway_NonAF = 65537u,
		CameraCommand_ShutterButton_Completely_NonAF = 65539u
	}

	public enum EdsSeekOrigin : uint
	{
		Cur,
		Begin,
		End
	}

	public enum EdsAccess : uint
	{
		Read = 0u,
		Write = 1u,
		ReadWrite = 2u,
		Error = uint.MaxValue
	}

	public enum EdsFileCreateDisposition : uint
	{
		CreateNew,
		CreateAlways,
		OpenExisting,
		OpenAlways,
		TruncateExsisting
	}

	public enum EdsTargetImageType : uint
	{
		Unknown = 0u,
		Jpeg = 1u,
		TIFF = 7u,
		TIFF16 = 8u,
		RGB = 9u,
		RGB16 = 10u
	}

	public enum EdsImageSource : uint
	{
		FullView,
		Thumbnail,
		Preview
	}

	public enum EdsProgressOption : uint
	{
		NoReport,
		Done,
		Periodically
	}

	public enum EdsFileAttribute : uint
	{
		Normal = 0u,
		ReadOnly = 1u,
		Hidden = 2u,
		System = 4u,
		Archive = 0x20u
	}

	public enum EdsSaveTo : uint
	{
		Camera = 1u,
		Host,
		Both
	}

	public enum EdsStorageType : uint
	{
		Non,
		CF,
		SD
	}

	public enum EdsTransferOption : uint
	{
		ByDirectTransfer = 1u,
		ByRelease = 2u,
		ToDesktop = 0x100u
	}

	public enum EdsMirrorLockupState : uint
	{
		Disable,
		Enable,
		DuringShooting
	}

	public enum EdsMirrorUpSetting : uint
	{
		Off,
		On
	}

	public enum EdsEvfAFMode : uint
	{
		Evf_AFMode_Quick = 0u,
		Evf_AFMode_Live = 1u,
		Evf_AFMode_LiveFace = 2u,
		Evf_AFMode_LiveMulti = 3u,
		Evf_AFMode_LiveZone = 4u,
		Evf_AFMode_LiveCatchAF = 9u,
		Evf_AFMode_LiveSpotAF = 10u
	}

	public enum EdsStroboMode
	{
		kEdsStroboModeInternal,
		kEdsStroboModeExternalETTL,
		kEdsStroboModeExternalATTL,
		kEdsStroboModeExternalTTL,
		kEdsStroboModeExternalAuto,
		kEdsStroboModeExternalManual,
		kEdsStroboModeManual
	}

	public enum EdsETTL2Mode
	{
		kEdsETTL2ModeEvaluative,
		kEdsETTL2ModeAverage
	}

	public enum DcStrobe : uint
	{
		DcStrobeAuto,
		DcStrobeOn,
		DcStrobeSlowsynchro,
		DcStrobeOff
	}

	public enum DcLensBarrelState : uint
	{
		DcLensBarrelStateInner,
		DcLensBarrelStateOuter
	}

	public enum DcRemoteShootingMode : uint
	{
		DcRemoteShootingModeStop,
		DcRemoteShootingModeStart
	}

	public enum ImageQuality : uint
	{
		EdsImageQuality_LJ = 1113871u,
		EdsImageQuality_MJ = 17891087u,
		EdsImageQuality_M1J = 84999951u,
		EdsImageQuality_M2J = 101777167u,
		EdsImageQuality_SJ = 34668303u,
		EdsImageQuality_S1J = 235994895u,
		EdsImageQuality_S2J = 252772111u,
		EdsImageQuality_LJF = 1310479u,
		EdsImageQuality_LJN = 1244943u,
		EdsImageQuality_MJF = 18087695u,
		EdsImageQuality_MJN = 18022159u,
		EdsImageQuality_SJF = 34864911u,
		EdsImageQuality_SJN = 34799375u,
		EdsImageQuality_S1JF = 236191503u,
		EdsImageQuality_S1JN = 236125967u,
		EdsImageQuality_S2JF = 252968719u,
		EdsImageQuality_S3JF = 269745935u,
		EdsImageQuality_LR = 6618895u,
		EdsImageQuality_LRLJF = 6553619u,
		EdsImageQuality_LRLJN = 6553618u,
		EdsImageQuality_LRMJF = 6553875u,
		EdsImageQuality_LRMJN = 6553874u,
		EdsImageQuality_LRSJF = 6554131u,
		EdsImageQuality_LRSJN = 6554130u,
		EdsImageQuality_LRS1JF = 6557203u,
		EdsImageQuality_LRS1JN = 6557202u,
		EdsImageQuality_LRS2JF = 6557459u,
		EdsImageQuality_LRS3JF = 6557715u,
		EdsImageQuality_LRLJ = 6553616u,
		EdsImageQuality_LRMJ = 6553872u,
		EdsImageQuality_LRM1J = 6554896u,
		EdsImageQuality_LRM2J = 6555152u,
		EdsImageQuality_LRSJ = 6554128u,
		EdsImageQuality_LRS1J = 6557200u,
		EdsImageQuality_LRS2J = 6557456u,
		EdsImageQuality_MR = 23396111u,
		EdsImageQuality_MRLJF = 23330835u,
		EdsImageQuality_MRLJN = 23330834u,
		EdsImageQuality_MRMJF = 23331091u,
		EdsImageQuality_MRMJN = 23331090u,
		EdsImageQuality_MRSJF = 23331347u,
		EdsImageQuality_MRSJN = 23331346u,
		EdsImageQuality_MRS1JF = 23334419u,
		EdsImageQuality_MRS1JN = 23334418u,
		EdsImageQuality_MRS2JF = 23334675u,
		EdsImageQuality_MRS3JF = 23334931u,
		EdsImageQuality_MRLJ = 23330832u,
		EdsImageQuality_MRM1J = 23332112u,
		EdsImageQuality_MRM2J = 23332368u,
		EdsImageQuality_MRSJ = 23331344u,
		EdsImageQuality_SR = 40173327u,
		EdsImageQuality_SRLJF = 40108051u,
		EdsImageQuality_SRLJN = 40108050u,
		EdsImageQuality_SRMJF = 40108307u,
		EdsImageQuality_SRMJN = 40108306u,
		EdsImageQuality_SRSJF = 40108563u,
		EdsImageQuality_SRSJN = 40108562u,
		EdsImageQuality_SRS1JF = 40111635u,
		EdsImageQuality_SRS1JN = 40111634u,
		EdsImageQuality_SRS2JF = 40111891u,
		EdsImageQuality_SRS3JF = 40112147u,
		EdsImageQuality_SRLJ = 40108048u,
		EdsImageQuality_SRM1J = 40109328u,
		EdsImageQuality_SRM2J = 40109584u,
		EdsImageQuality_SRSJ = 40108560u,
		EdsImageQuality_CR = 6553359u,
		EdsImageQuality_CRLJF = 6488083u,
		EdsImageQuality_CRMJF = 6488339u,
		EdsImageQuality_CRM1JF = 6489363u,
		EdsImageQuality_CRM2JF = 6489619u,
		EdsImageQuality_CRSJF = 6488595u,
		EdsImageQuality_CRS1JF = 6491667u,
		EdsImageQuality_CRS2JF = 6491923u,
		EdsImageQuality_CRS3JF = 6492179u,
		EdsImageQuality_CRLJN = 6488082u,
		EdsImageQuality_CRMJN = 6488338u,
		EdsImageQuality_CRM1JN = 6489362u,
		EdsImageQuality_CRM2JN = 6489618u,
		EdsImageQuality_CRSJN = 6488594u,
		EdsImageQuality_CRS1JN = 6491666u,
		EdsImageQuality_CRLJ = 6488080u,
		EdsImageQuality_CRMJ = 6488336u,
		EdsImageQuality_CRM1J = 6489360u,
		EdsImageQuality_CRM2J = 6489616u,
		EdsImageQuality_CRSJ = 6488592u,
		EdsImageQuality_CRS1J = 6491664u,
		EdsImageQuality_CRS2J = 6491920u,
		EdsImageQuality_HEIFL = 8453903u,
		EdsImageQuality_RHEIFL = 6553728u,
		EdsImageQuality_CRHEIFL = 6488192u,
		EdsImageQuality_HEIFLF = 8650511u,
		EdsImageQuality_HEIFLN = 8584975u,
		EdsImageQuality_HEIFMF = 25427727u,
		EdsImageQuality_HEIFMN = 25362191u,
		EdsImageQuality_HEIFS1F = 243531535u,
		EdsImageQuality_HEIFS1N = 243465999u,
		EdsImageQuality_HEIFS2F = 260308751u,
		EdsImageQuality_RHEIFLF = 6553731u,
		EdsImageQuality_RHEIFLN = 6553730u,
		EdsImageQuality_RHEIFMF = 6553987u,
		EdsImageQuality_RHEIFMN = 6553986u,
		EdsImageQuality_RHEIFS1F = 6557315u,
		EdsImageQuality_RHEIFS1N = 6557314u,
		EdsImageQuality_RHEIFS2F = 6557571u,
		EdsImageQuality_CRHEIFLF = 6488195u,
		EdsImageQuality_CRHEIFLN = 6488194u,
		EdsImageQuality_CRHEIFMF = 6488451u,
		EdsImageQuality_CRHEIFMN = 6488450u,
		EdsImageQuality_CRHEIFS1F = 6491779u,
		EdsImageQuality_CRHEIFS1N = 6491778u,
		EdsImageQuality_CRHEIFS2F = 6492035u,
		EdsImageQuality_Unknown = uint.MaxValue
	}

	public struct EdsPoint
	{
		public int x;

		public int y;
	}

	public struct EdsRect
	{
		public int x;

		public int y;

		public int width;

		public int height;
	}

	public struct EdsSize
	{
		public int width;

		public int height;
	}

	public struct EdsRational
	{
		public int Numerator;

		public uint Denominator;
	}

	public struct EdsTime
	{
		public int Year;

		public int Month;

		public int Day;

		public int Hour;

		public int Minute;

		public int Second;

		public int Milliseconds;
	}

	public struct EdsDeviceInfo
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string szPortName;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string szDeviceDescription;

		public uint DeviceSubType;

		public uint reserved;
	}

	public struct EdsVolumeInfo
	{
		public uint StorageType;

		public uint Access;

		public ulong MaxCapacity;

		public ulong FreeSpaceInBytes;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string szVolumeLabel;
	}

	public struct EdsDirectoryItemInfo
	{
		public ulong Size;

		public int isFolder;

		public uint GroupID;

		public uint Option;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string szFileName;

		public uint format;

		public uint dateTime;
	}

	public struct EdsImageInfo
	{
		public uint Width;

		public uint Height;

		public uint NumOfComponents;

		public uint ComponentDepth;

		public EdsRect EffectiveRect;

		public uint reserved1;

		public uint reserved2;
	}

	public struct EdsSaveImageSetting
	{
		public uint JPEGQuality;

		private IntPtr iccProfileStream;

		public uint reserved;
	}

	public struct EdsPropertyDesc
	{
		public int Form;

		public uint Access;

		public int NumElements;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		public int[] PropDesc;
	}

	public struct EdsPictureStyleDesc
	{
		public int contrast;

		public uint sharpness;

		public int saturation;

		public int colorTone;

		public uint filterEffect;

		public uint toningEffect;

		public uint sharpFineness;

		public uint sharpThreshold;
	}

	public struct EdsFocusPoint
	{
		public uint valid;

		public uint selected;

		public uint justFocus;

		public EdsRect rect;

		public uint reserved;
	}

	public struct EdsFocusInfo
	{
		public EdsRect imageRect;

		public uint pointNumber;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1053)]
		public EdsFocusPoint[] focusPoint;

		public uint executeMode;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public struct EdsCapacity
	{
		public int NumberOfFreeClusters;

		public int BytesPerSector;

		public int Reset;
	}

	public struct EdsCameraPos
	{
		public int status;

		public int position;

		public int rolling;

		public int pitching;
	}

	public struct FocusShiftSetting
	{
		public uint Version;

		public uint FocusShiftFunction;

		public uint ShootingNumber;

		public uint StepWidth;

		public uint ExposureSmoothing;
	}

	public struct EdsManualWBData
	{
		public uint Valid;

		public uint dataSize;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string szCaption;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
		public byte[] data;
	}

	public const uint PropID_Unknown = 65535u;

	public const uint PropID_ProductName = 2u;

	public const uint PropID_BodyIDEx = 21u;

	public const uint PropID_OwnerName = 4u;

	public const uint PropID_MakerName = 5u;

	public const uint PropID_DateTime = 6u;

	public const uint PropID_FirmwareVersion = 7u;

	public const uint PropID_BatteryLevel = 8u;

	public const uint PropID_CFn = 9u;

	public const uint PropID_SaveTo = 11u;

	public const uint kEdsPropID_CurrentStorage = 12u;

	public const uint kEdsPropID_CurrentFolder = 13u;

	public const uint PropID_BatteryQuality = 16u;

	public const uint PropID_ImageQuality = 256u;

	public const uint PropID_Orientation = 258u;

	public const uint PropID_ICCProfile = 259u;

	public const uint PropID_FocusInfo = 260u;

	public const uint PropID_WhiteBalance = 262u;

	public const uint PropID_ColorTemperature = 263u;

	public const uint PropID_WhiteBalanceShift = 264u;

	public const uint PropID_ColorSpace = 269u;

	public const uint PropID_PictureStyle = 276u;

	public const uint PropID_PictureStyleDesc = 277u;

	public const uint PropID_PictureStyleCaption = 512u;

	public const uint PropID_AEMode = 1024u;

	public const uint PropID_AEModeSelect = 1078u;

	public const uint PropID_DriveMode = 1025u;

	public const uint PropID_ISOSpeed = 1026u;

	public const uint PropID_MeteringMode = 1027u;

	public const uint PropID_AFMode = 1028u;

	public const uint PropID_Av = 1029u;

	public const uint PropID_Tv = 1030u;

	public const uint PropID_ExposureCompensation = 1031u;

	public const uint PropID_FocalLength = 1033u;

	public const uint PropID_AvailableShots = 1034u;

	public const uint PropID_Bracket = 1035u;

	public const uint PropID_WhiteBalanceBracket = 1036u;

	public const uint PropID_LensName = 1037u;

	public const uint PropID_AEBracket = 1038u;

	public const uint PropID_FEBracket = 1039u;

	public const uint PropID_ISOBracket = 1040u;

	public const uint PropID_NoiseReduction = 1041u;

	public const uint PropID_FlashOn = 1042u;

	public const uint PropID_RedEye = 1043u;

	public const uint PropID_FlashMode = 1044u;

	public const uint PropID_LensStatus = 1046u;

	public const uint PropID_Artist = 1048u;

	public const uint PropID_Copyright = 1049u;

	public const uint PropID_Evf_OutputDevice = 1280u;

	public const uint PropID_Evf_Mode = 1281u;

	public const uint PropID_Evf_WhiteBalance = 1282u;

	public const uint PropID_Evf_ColorTemperature = 1283u;

	public const uint PropID_Evf_DepthOfFieldPreview = 1284u;

	public const uint PropID_Evf_Zoom = 1287u;

	public const uint PropID_Evf_ZoomPosition = 1288u;

	public const uint PropID_Evf_ImagePosition = 1291u;

	public const uint PropID_Evf_HistogramStatus = 1292u;

	public const uint PropID_Evf_AFMode = 1294u;

	public const uint PropID_Evf_HistogramY = 1301u;

	public const uint PropID_Evf_HistogramR = 1302u;

	public const uint PropID_Evf_HistogramG = 1303u;

	public const uint PropID_Evf_HistogramB = 1304u;

	public const uint PropID_Evf_CoordinateSystem = 1344u;

	public const uint PropID_Evf_ZoomRect = 1345u;

	public const uint PropID_Record = 1296u;

	public const uint PropID_GPSVersionID = 2048u;

	public const uint PropID_GPSLatitudeRef = 2049u;

	public const uint PropID_GPSLatitude = 2050u;

	public const uint PropID_GPSLongitudeRef = 2051u;

	public const uint PropID_GPSLongitude = 2052u;

	public const uint PropID_GPSAltitudeRef = 2053u;

	public const uint PropID_GPSAltitude = 2054u;

	public const uint PropID_GPSTimeStamp = 2055u;

	public const uint PropID_GPSSatellites = 2056u;

	public const uint PropID_GPSStatus = 2057u;

	public const uint PropID_GPSMapDatum = 2066u;

	public const uint PropID_GPSDateStamp = 2077u;

	public const uint PropID_DC_Zoom = 1536u;

	public const uint PropID_DC_Strobe = 1537u;

	public const uint PropID_LensBarrelStatus = 1541u;

	public const uint PropID_TempStatus = 16778261u;

	public const uint PropID_Evf_RollingPitching = 16778564u;

	public const uint PropID_FixedMovie = 16778274u;

	public const uint PropID_MovieParam = 16778275u;

	public const uint PropID_Aspect = 16778289u;

	public const uint PropID_Evf_ClickWBCoeffs = 16778502u;

	public const uint PropID_Evf_VisibleRect = 16778566u;

	public const uint PropID_ManualWhiteBalanceData = 16777732u;

	public const uint PropID_MirrorUpSetting = 16778296u;

	public const uint PropID_MirrorLockUpState = 16778273u;

	public const uint PropID_UTCTime = 16777238u;

	public const uint PropID_TimeZone = 16777239u;

	public const uint PropID_SummerTimeSetting = 16777240u;

	public const uint PropID_AutoPowerOffSetting = 16778334u;

	public const uint PropID_StillMovieDivideSetting = 16778352u;

	public const uint PropID_CardExtension = 16778353u;

	public const uint PropID_MovieCardExtension = 16778354u;

	public const uint PropID_StillCurrentMedia = 16778355u;

	public const uint PropID_MovieCurrentMedia = 16778356u;

	public const uint PropID_MovieHFRSetting = 16778333u;

	public const uint PropID_FocusShiftSetting = 16778327u;

	public const uint CameraCommand_TakePicture = 0u;

	public const uint CameraCommand_ExtendShutDownTimer = 1u;

	public const uint CameraCommand_BulbStart = 2u;

	public const uint CameraCommand_BulbEnd = 3u;

	public const uint CameraCommand_DoEvfAf = 258u;

	public const uint CameraCommand_DriveLensEvf = 259u;

	public const uint CameraCommand_DoClickWBEvf = 260u;

	public const uint CameraCommand_MovieSelectSwON = 263u;

	public const uint CameraCommand_MovieSelectSwOFF = 264u;

	public const uint CameraCommand_PressShutterButton = 4u;

	public const uint CameraCommand_SetRemoteShootingMode = 271u;

	public const uint CameraCommand_RequestRollPitchLevel = 265u;

	public const uint CameraCommand_RequestSensorCleaning = 274u;

	public const uint CameraState_UILock = 0u;

	public const uint CameraState_UIUnLock = 1u;

	public const uint CameraState_EnterDirectTransfer = 2u;

	public const uint CameraState_ExitDirectTransfer = 3u;

	public const uint EvfDriveLens_Near1 = 1u;

	public const uint EvfDriveLens_Near2 = 2u;

	public const uint EvfDriveLens_Near3 = 3u;

	public const uint EvfDriveLens_Far1 = 32769u;

	public const uint EvfDriveLens_Far2 = 32770u;

	public const uint EvfDriveLens_Far3 = 32771u;

	public const uint EvfDepthOfFieldPreview_OFF = 0u;

	public const uint EvfDepthOfFieldPreview_ON = 1u;

	public const int ImageFormat_Unknown = 0;

	public const int ImageFormat_Jpeg = 1;

	public const int ImageFormat_CRW = 2;

	public const int ImageFormat_RAW = 4;

	public const int ImageFormat_CR2 = 6;

	public const int ImageFormat_CR2_Jpeg = 7;

	public const int ImageFormat_HEIF = 8;

	public const int ImageSize_Large = 0;

	public const int ImageSize_Middle = 1;

	public const int ImageSize_Small = 2;

	public const int ImageSize_Middle1 = 5;

	public const int ImageSize_Middle2 = 6;

	public const int ImageSize_Small1 = 14;

	public const int ImageSize_Small2 = 15;

	public const int ImageSize_Small3 = 16;

	public const int ImageSize_Unknown = -1;

	public const int ImageQuality_Normal = 2;

	public const int ImageQuality_Fine = 3;

	public const int ImageQuality_Lossless = 4;

	public const int ImageQuality_SuperFine = 5;

	public const int ImageQuality_Unknown = -1;

	public const int CompressQuality_Normal = 2;

	public const int CompressQuality_Fine = 3;

	public const int CompressQuality_Lossless = 4;

	public const int CompressQuality_SuperFine = 5;

	public const int CompressQuality_Unknown = -1;

	public const uint BatteryLevel_Empty = 1u;

	public const uint BatteryLevel_Low = 30u;

	public const uint BatteryLevel_Half = 50u;

	public const uint BatteryLevel_Normal = 80u;

	public const uint BatteryLevel_AC = uint.MaxValue;

	public const int WhiteBalance_Click = -1;

	public const int WhiteBalance_Auto = 0;

	public const int WhiteBalance_Daylight = 1;

	public const int WhiteBalance_Cloudy = 2;

	public const int WhiteBalance_Tungsten = 3;

	public const int WhiteBalance_Fluorescent = 4;

	public const int WhiteBalance_Strobe = 5;

	public const int WhiteBalance_Shade = 8;

	public const int WhiteBalance_ColorTemp = 9;

	public const int WhiteBalance_Manual1 = 6;

	public const int WhiteBalance_Manual2 = 15;

	public const int WhiteBalance_Manual3 = 16;

	public const int WhiteBalance_Manual4 = 18;

	public const int WhiteBalance_Manual5 = 19;

	public const int WhiteBalance_PCSet1 = 10;

	public const int WhiteBalance_PCSet2 = 11;

	public const int WhiteBalance_PCSet3 = 12;

	public const int WhiteBalance_PCSet4 = 20;

	public const int WhiteBalance_PCSet5 = 21;

	public const int WhiteBalance_AwbWhite = 23;

	public const uint ColorSpace_sRGB = 1u;

	public const uint ColorSpace_AdobeRGB = 2u;

	public const uint ColorSpace_Unknown = uint.MaxValue;

	public const uint PictureStyle_Standard = 129u;

	public const uint PictureStyle_Portrait = 130u;

	public const uint PictureStyle_Landscape = 131u;

	public const uint PictureStyle_Neutral = 132u;

	public const uint PictureStyle_Faithful = 133u;

	public const uint PictureStyle_Monochrome = 134u;

	public const uint PictureStyle_Auto = 135u;

	public const uint PictureStyle_FineDetail = 136u;

	public const uint PictureStyle_User1 = 33u;

	public const uint PictureStyle_User2 = 34u;

	public const uint PictureStyle_User3 = 35u;

	public const uint PictureStyle_PC1 = 65u;

	public const uint PictureStyle_PC2 = 66u;

	public const uint PictureStyle_PC3 = 67u;

	public const uint AEMode_Program = 0u;

	public const uint AEMode_Tv = 1u;

	public const uint AEMode_Av = 2u;

	public const uint AEMode_Mamual = 3u;

	public const uint AEMode_Bulb = 4u;

	public const uint AEMode_A_DEP = 5u;

	public const uint AEMode_DEP = 6u;

	public const uint AEMode_Custom = 7u;

	public const uint AEMode_Lock = 8u;

	public const uint AEMode_Green = 9u;

	public const uint AEMode_NigntPortrait = 10u;

	public const uint AEMode_Sports = 11u;

	public const uint AEMode_Portrait = 12u;

	public const uint AEMode_Landscape = 13u;

	public const uint AEMode_Closeup = 14u;

	public const uint AEMode_FlashOff = 15u;

	public const uint AEMode_CreativeAuto = 19u;

	public const uint AEMode_Movie = 20u;

	public const uint AEMode_PhotoInMovie = 21u;

	public const uint AEMode_SceneIntelligentAuto = 22u;

	public const uint AEMode_SCN = 25u;

	public const uint AEMode_HandheldNightScenes = 23u;

	public const uint AEMode_Hdr_BacklightControl = 24u;

	public const uint AEMode_Children = 26u;

	public const uint AEMode_Food = 27u;

	public const uint AEMode_CandlelightPortraits = 28u;

	public const uint AEMode_CreativeFilter = 29u;

	public const uint AEMode_RoughMonoChrome = 30u;

	public const uint AEMode_SoftFocus = 31u;

	public const uint AEMode_ToyCamera = 32u;

	public const uint AEMode_Fisheye = 33u;

	public const uint AEMode_WaterColor = 34u;

	public const uint AEMode_Miniature = 35u;

	public const uint AEMode_Hdr_Standard = 36u;

	public const uint AEMode_Hdr_Vivid = 37u;

	public const uint AEMode_Hdr_Bold = 38u;

	public const uint AEMode_Hdr_Embossed = 39u;

	public const uint AEMode_Movie_Fantasy = 40u;

	public const uint AEMode_Movie_Old = 41u;

	public const uint AEMode_Movie_Memory = 42u;

	public const uint AEMode_Movie_DirectMono = 43u;

	public const uint AEMode_Movie_Mini = 44u;

	public const uint AEMode_Panning = 45u;

	public const uint AEMode_GroupPhoto = 46u;

	public const uint AEMode_SelfPortrait = 50u;

	public const uint AEMode_PlusMovieAuto = 51u;

	public const uint AEMode_SmoothSkin = 52u;

	public const uint AEMode_Panorama = 53u;

	public const uint AEMode_Silent = 54u;

	public const uint AEMode_Flexible = 55u;

	public const uint AEMode_OilPainting = 56u;

	public const uint AEMode_Fireworks = 57u;

	public const uint AEMode_StarPortrait = 58u;

	public const uint AEMode_StarNightscape = 59u;

	public const uint AEMode_StarTrails = 60u;

	public const uint AEMode_StarTimelapseMovie = 61u;

	public const uint AEMode_BackgroundBlur = 62u;

	public const uint AEMode_VideoBlog = 63u;

	public const uint AEMode_Unknown = uint.MaxValue;

	public const uint Bracket_AEB = 1u;

	public const uint Bracket_ISOB = 2u;

	public const uint Bracket_WBB = 4u;

	public const uint Bracket_FEB = 8u;

	public const uint Bracket_Unknown = uint.MaxValue;

	public const uint EvfOutputDevice_TFT = 1u;

	public const uint EvfOutputDevice_PC = 2u;

	public const uint EvfOutputDevice_PC_Small = 8u;

	public const int EvfZoom_Fit = 1;

	public const int EvfZoom_x5 = 5;

	public const int EvfZoom_x10 = 10;

	public const uint PropertyEvent_All = 256u;

	public const uint PropertyEvent_PropertyChanged = 257u;

	public const uint PropertyEvent_PropertyDescChanged = 258u;

	public const uint ObjectEvent_All = 512u;

	public const uint ObjectEvent_VolumeInfoChanged = 513u;

	public const uint ObjectEvent_VolumeUpdateItems = 514u;

	public const uint ObjectEvent_FolderUpdateItems = 515u;

	public const uint ObjectEvent_DirItemCreated = 516u;

	public const uint ObjectEvent_DirItemRemoved = 517u;

	public const uint ObjectEvent_DirItemInfoChanged = 518u;

	public const uint ObjectEvent_DirItemContentChanged = 519u;

	public const uint ObjectEvent_DirItemRequestTransfer = 520u;

	public const uint ObjectEvent_DirItemRequestTransferDT = 521u;

	public const uint ObjectEvent_DirItemCancelTransferDT = 522u;

	public const uint ObjectEvent_VolumeAdded = 524u;

	public const uint ObjectEvent_VolumeRemoved = 525u;

	public const uint StateEvent_All = 768u;

	public const uint StateEvent_Shutdown = 769u;

	public const uint StateEvent_JobStatusChanged = 770u;

	public const uint StateEvent_WillSoonShutDown = 771u;

	public const uint StateEvent_ShutDownTimerUpdate = 772u;

	public const uint StateEvent_CaptureError = 773u;

	public const uint StateEvent_InternalError = 774u;

	public const uint StateEvent_AfResult = 777u;

	public const int EDS_MAX_NAME = 256;

	public const int EDS_TRANSFER_BLOCK_SIZE = 512;

	public const uint EDS_ISSPECIFIC_MASK = 2147483648u;

	public const uint EDS_COMPONENTID_MASK = 2130706432u;

	public const uint EDS_RESERVED_MASK = 16711680u;

	public const uint EDS_ERRORID_MASK = 65535u;

	public const uint EDS_CMP_ID_CLIENT_COMPONENTID = 16777216u;

	public const uint EDS_CMP_ID_LLSDK_COMPONENTID = 33554432u;

	public const uint EDS_CMP_ID_HLSDK_COMPONENTID = 50331648u;

	public const uint EDS_ERR_OK = 0u;

	public const uint EDS_ERR_UNIMPLEMENTED = 1u;

	public const uint EDS_ERR_INTERNAL_ERROR = 2u;

	public const uint EDS_ERR_MEM_ALLOC_FAILED = 3u;

	public const uint EDS_ERR_MEM_FREE_FAILED = 4u;

	public const uint EDS_ERR_OPERATION_CANCELLED = 5u;

	public const uint EDS_ERR_INCOMPATIBLE_VERSION = 6u;

	public const uint EDS_ERR_NOT_SUPPORTED = 7u;

	public const uint EDS_ERR_UNEXPECTED_EXCEPTION = 8u;

	public const uint EDS_ERR_PROTECTION_VIOLATION = 9u;

	public const uint EDS_ERR_MISSING_SUBCOMPONENT = 10u;

	public const uint EDS_ERR_SELECTION_UNAVAILABLE = 11u;

	public const uint EDS_ERR_FILE_IO_ERROR = 32u;

	public const uint EDS_ERR_FILE_TOO_MANY_OPEN = 33u;

	public const uint EDS_ERR_FILE_NOT_FOUND = 34u;

	public const uint EDS_ERR_FILE_OPEN_ERROR = 35u;

	public const uint EDS_ERR_FILE_CLOSE_ERROR = 36u;

	public const uint EDS_ERR_FILE_SEEK_ERROR = 37u;

	public const uint EDS_ERR_FILE_TELL_ERROR = 38u;

	public const uint EDS_ERR_FILE_READ_ERROR = 39u;

	public const uint EDS_ERR_FILE_WRITE_ERROR = 40u;

	public const uint EDS_ERR_FILE_PERMISSION_ERROR = 41u;

	public const uint EDS_ERR_FILE_DISK_FULL_ERROR = 42u;

	public const uint EDS_ERR_FILE_ALREADY_EXISTS = 43u;

	public const uint EDS_ERR_FILE_FORMAT_UNRECOGNIZED = 44u;

	public const uint EDS_ERR_FILE_DATA_CORRUPT = 45u;

	public const uint EDS_ERR_FILE_NAMING_NA = 46u;

	public const uint EDS_ERR_DIR_NOT_FOUND = 64u;

	public const uint EDS_ERR_DIR_IO_ERROR = 65u;

	public const uint EDS_ERR_DIR_ENTRY_NOT_FOUND = 66u;

	public const uint EDS_ERR_DIR_ENTRY_EXISTS = 67u;

	public const uint EDS_ERR_DIR_NOT_EMPTY = 68u;

	public const uint EDS_ERR_PROPERTIES_UNAVAILABLE = 80u;

	public const uint EDS_ERR_PROPERTIES_MISMATCH = 81u;

	public const uint EDS_ERR_PROPERTIES_NOT_LOADED = 83u;

	public const uint EDS_ERR_INVALID_PARAMETER = 96u;

	public const uint EDS_ERR_INVALID_HANDLE = 97u;

	public const uint EDS_ERR_INVALID_POINTER = 98u;

	public const uint EDS_ERR_INVALID_INDEX = 99u;

	public const uint EDS_ERR_INVALID_LENGTH = 100u;

	public const uint EDS_ERR_INVALID_FN_POINTER = 101u;

	public const uint EDS_ERR_INVALID_SORT_FN = 102u;

	public const uint EDS_ERR_DEVICE_NOT_FOUND = 128u;

	public const uint EDS_ERR_DEVICE_BUSY = 129u;

	public const uint EDS_ERR_DEVICE_INVALID = 130u;

	public const uint EDS_ERR_DEVICE_EMERGENCY = 131u;

	public const uint EDS_ERR_DEVICE_MEMORY_FULL = 132u;

	public const uint EDS_ERR_DEVICE_INTERNAL_ERROR = 133u;

	public const uint EDS_ERR_DEVICE_INVALID_PARAMETER = 134u;

	public const uint EDS_ERR_DEVICE_NO_DISK = 135u;

	public const uint EDS_ERR_DEVICE_DISK_ERROR = 136u;

	public const uint EDS_ERR_DEVICE_CF_GATE_CHANGED = 137u;

	public const uint EDS_ERR_DEVICE_DIAL_CHANGED = 138u;

	public const uint EDS_ERR_DEVICE_NOT_INSTALLED = 139u;

	public const uint EDS_ERR_DEVICE_STAY_AWAKE = 140u;

	public const uint EDS_ERR_DEVICE_NOT_RELEASED = 141u;

	public const uint EDS_ERR_STREAM_IO_ERROR = 160u;

	public const uint EDS_ERR_STREAM_NOT_OPEN = 161u;

	public const uint EDS_ERR_STREAM_ALREADY_OPEN = 162u;

	public const uint EDS_ERR_STREAM_OPEN_ERROR = 163u;

	public const uint EDS_ERR_STREAM_CLOSE_ERROR = 164u;

	public const uint EDS_ERR_STREAM_SEEK_ERROR = 165u;

	public const uint EDS_ERR_STREAM_TELL_ERROR = 166u;

	public const uint EDS_ERR_STREAM_READ_ERROR = 167u;

	public const uint EDS_ERR_STREAM_WRITE_ERROR = 168u;

	public const uint EDS_ERR_STREAM_PERMISSION_ERROR = 169u;

	public const uint EDS_ERR_STREAM_COULDNT_BEGIN_THREAD = 170u;

	public const uint EDS_ERR_STREAM_BAD_OPTIONS = 171u;

	public const uint EDS_ERR_STREAM_END_OF_STREAM = 172u;

	public const uint EDS_ERR_COMM_PORT_IS_IN_USE = 192u;

	public const uint EDS_ERR_COMM_DISCONNECTED = 193u;

	public const uint EDS_ERR_COMM_DEVICE_INCOMPATIBLE = 194u;

	public const uint EDS_ERR_COMM_BUFFER_FULL = 195u;

	public const uint EDS_ERR_COMM_USB_BUS_ERR = 196u;

	public const uint EDS_ERR_USB_DEVICE_LOCK_ERROR = 208u;

	public const uint EDS_ERR_USB_DEVICE_UNLOCK_ERROR = 209u;

	public const uint EDS_ERR_STI_UNKNOWN_ERROR = 224u;

	public const uint EDS_ERR_STI_INTERNAL_ERROR = 225u;

	public const uint EDS_ERR_STI_DEVICE_CREATE_ERROR = 226u;

	public const uint EDS_ERR_STI_DEVICE_RELEASE_ERROR = 227u;

	public const uint EDS_ERR_DEVICE_NOT_LAUNCHED = 228u;

	public const uint EDS_ERR_ENUM_NA = 240u;

	public const uint EDS_ERR_INVALID_FN_CALL = 241u;

	public const uint EDS_ERR_HANDLE_NOT_FOUND = 242u;

	public const uint EDS_ERR_INVALID_ID = 243u;

	public const uint EDS_ERR_WAIT_TIMEOUT_ERROR = 244u;

	public const uint EDS_ERR_SESSION_NOT_OPEN = 8195u;

	public const uint EDS_ERR_INVALID_TRANSACTIONID = 8196u;

	public const uint EDS_ERR_INCOMPLETE_TRANSFER = 8199u;

	public const uint EDS_ERR_INVALID_STRAGEID = 8200u;

	public const uint EDS_ERR_DEVICEPROP_NOT_SUPPORTED = 8202u;

	public const uint EDS_ERR_INVALID_OBJECTFORMATCODE = 8203u;

	public const uint EDS_ERR_SELF_TEST_FAILED = 8209u;

	public const uint EDS_ERR_PARTIAL_DELETION = 8210u;

	public const uint EDS_ERR_SPECIFICATION_BY_FORMAT_UNSUPPORTED = 8212u;

	public const uint EDS_ERR_NO_VALID_OBJECTINFO = 8213u;

	public const uint EDS_ERR_INVALID_CODE_FORMAT = 8214u;

	public const uint EDS_ERR_UNKNOWN_VENDER_CODE = 8215u;

	public const uint EDS_ERR_CAPTURE_ALREADY_TERMINATED = 8216u;

	public const uint EDS_ERR_INVALID_PARENTOBJECT = 8218u;

	public const uint EDS_ERR_INVALID_DEVICEPROP_FORMAT = 8219u;

	public const uint EDS_ERR_INVALID_DEVICEPROP_VALUE = 8220u;

	public const uint EDS_ERR_SESSION_ALREADY_OPEN = 8222u;

	public const uint EDS_ERR_TRANSACTION_CANCELLED = 8223u;

	public const uint EDS_ERR_SPECIFICATION_OF_DESTINATION_UNSUPPORTED = 8224u;

	public const uint EDS_ERR_UNKNOWN_COMMAND = 40961u;

	public const uint EDS_ERR_OPERATION_REFUSED = 40965u;

	public const uint EDS_ERR_LENS_COVER_CLOSE = 40966u;

	public const uint EDS_ERR_LOW_BATTERY = 41217u;

	public const uint EDS_ERR_OBJECT_NOTREADY = 41218u;

	public const uint EDS_ERR_TAKE_PICTURE_AF_NG = 36097u;

	public const uint EDS_ERR_TAKE_PICTURE_RESERVED = 36098u;

	public const uint EDS_ERR_TAKE_PICTURE_MIRROR_UP_NG = 36099u;

	public const uint EDS_ERR_TAKE_PICTURE_SENSOR_CLEANING_NG = 36100u;

	public const uint EDS_ERR_TAKE_PICTURE_SILENCE_NG = 36101u;

	public const uint EDS_ERR_TAKE_PICTURE_NO_CARD_NG = 36102u;

	public const uint EDS_ERR_TAKE_PICTURE_CARD_NG = 36103u;

	public const uint EDS_ERR_TAKE_PICTURE_CARD_PROTECT_NG = 36104u;

	public const uint EDS_ERR_LAST_GENERIC_ERROR_PLUS_ONE = 245u;

	[DllImport("EDSDK.dll")]
	public static extern uint EdsInitializeSDK();

	[DllImport("EDSDK.dll")]
	public static extern uint EdsTerminateSDK();

	[DllImport("EDSDK.dll")]
	public static extern uint EdsRetain(IntPtr inRef);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsRelease(IntPtr inRef);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsGetChildCount(IntPtr inRef, out int outCount);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsGetChildAtIndex(IntPtr inRef, int inIndex, out IntPtr outRef);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsGetParent(IntPtr inRef, out IntPtr outParentRef);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsGetPropertySize(IntPtr inRef, uint inPropertyID, int inParam, out EdsDataType outDataType, out int outSize);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsGetPropertyData(IntPtr inRef, uint inPropertyID, int inParam, int inPropertySize, IntPtr outPropertyData);

	public static uint EdsGetPropertyData(IntPtr inRef, uint inPropertyID, int inParam, out uint outPropertyData)
	{
		int size = Marshal.SizeOf(typeof(uint));
		IntPtr ptr = Marshal.AllocHGlobal(size);
		uint result = EdsGetPropertyData(inRef, inPropertyID, inParam, size, ptr);
		outPropertyData = (uint)Marshal.PtrToStructure(ptr, typeof(uint));
		Marshal.FreeHGlobal(ptr);
		return result;
	}

	public static uint EdsGetPropertyData(IntPtr inRef, uint inPropertyID, int inParam, out EdsTime outPropertyData)
	{
		int size = Marshal.SizeOf(typeof(EdsTime));
		IntPtr ptr = Marshal.AllocHGlobal(size);
		uint result = EdsGetPropertyData(inRef, inPropertyID, inParam, size, ptr);
		outPropertyData = (EdsTime)Marshal.PtrToStructure(ptr, typeof(EdsTime));
		Marshal.FreeHGlobal(ptr);
		return result;
	}

	public static uint EdsGetPropertyData(IntPtr inRef, uint inPropertyID, int inParam, out EdsFocusInfo outPropertyData)
	{
		int size = Marshal.SizeOf(typeof(EdsFocusInfo));
		IntPtr ptr = Marshal.AllocHGlobal(size);
		uint result = EdsGetPropertyData(inRef, inPropertyID, inParam, size, ptr);
		outPropertyData = (EdsFocusInfo)Marshal.PtrToStructure(ptr, typeof(EdsFocusInfo));
		Marshal.FreeHGlobal(ptr);
		return result;
	}

	public static uint EdsGetPropertyData(IntPtr inRef, uint inPropertyID, int inParam, out EdsPoint outPropertyData)
	{
		int size = Marshal.SizeOf(typeof(EdsPoint));
		IntPtr ptr = Marshal.AllocHGlobal(size);
		uint result = EdsGetPropertyData(inRef, inPropertyID, inParam, size, ptr);
		outPropertyData = (EdsPoint)Marshal.PtrToStructure(ptr, typeof(EdsPoint));
		Marshal.FreeHGlobal(ptr);
		return result;
	}

	public static uint EdsGetPropertyData(IntPtr inRef, uint inPropertyID, int inParam, out EdsRect outPropertyData)
	{
		int size = Marshal.SizeOf(typeof(EdsRect));
		IntPtr ptr = Marshal.AllocHGlobal(size);
		uint result = EdsGetPropertyData(inRef, inPropertyID, inParam, size, ptr);
		outPropertyData = (EdsRect)Marshal.PtrToStructure(ptr, typeof(EdsRect));
		Marshal.FreeHGlobal(ptr);
		return result;
	}

	public static uint EdsGetPropertyData(IntPtr inRef, uint inPropertyID, int inParam, out EdsSize outPropertyData)
	{
		int size = Marshal.SizeOf(typeof(EdsSize));
		IntPtr ptr = Marshal.AllocHGlobal(size);
		uint result = EdsGetPropertyData(inRef, inPropertyID, inParam, size, ptr);
		outPropertyData = (EdsSize)Marshal.PtrToStructure(ptr, typeof(EdsSize));
		Marshal.FreeHGlobal(ptr);
		return result;
	}

	public static uint EdsGetPropertyData(IntPtr inRef, uint inPropertyID, int inParam, out string outPropertyData)
	{
		IntPtr ptr = Marshal.AllocHGlobal(256);
		uint result = EdsGetPropertyData(inRef, inPropertyID, inParam, 256, ptr);
		outPropertyData = Marshal.PtrToStringAnsi(ptr);
		Marshal.FreeHGlobal(ptr);
		return result;
	}

	public static uint EdsGetPropertyData(IntPtr inRef, uint inPropertyID, int inParam, out int[] outPropertyData)
	{
		EdsGetPropertySize(inRef, inPropertyID, 0, out var _, out var size);
		IntPtr ptr = Marshal.AllocHGlobal(size);
		uint result = EdsGetPropertyData(inRef, inPropertyID, inParam, size, ptr);
		int len = size / 4;
		outPropertyData = new int[len];
		Marshal.Copy(ptr, outPropertyData, 0, len);
		Marshal.FreeHGlobal(ptr);
		return result;
	}

	public static uint EdsGetPropertyData(IntPtr inRef, uint inPropertyID, int inParam, out EdsCameraPos outPropertyData)
	{
		int size = Marshal.SizeOf(typeof(EdsCameraPos));
		IntPtr ptr = Marshal.AllocHGlobal(size);
		uint result = EdsGetPropertyData(inRef, inPropertyID, inParam, size, ptr);
		outPropertyData = (EdsCameraPos)Marshal.PtrToStructure(ptr, typeof(EdsCameraPos));
		Marshal.FreeHGlobal(ptr);
		return result;
	}

	public static uint EdsGetPropertyData(IntPtr inRef, uint inPropertyID, int inParam, out FocusShiftSetting outPropertyData)
	{
		int size = Marshal.SizeOf(typeof(FocusShiftSetting));
		IntPtr ptr = Marshal.AllocHGlobal(size);
		uint result = EdsGetPropertyData(inRef, inPropertyID, inParam, size, ptr);
		outPropertyData = (FocusShiftSetting)Marshal.PtrToStructure(ptr, typeof(FocusShiftSetting));
		Marshal.FreeHGlobal(ptr);
		return result;
	}

	public static uint EdsGetPropertyData(IntPtr inRef, uint inPropertyID, int inParam, out byte[] outPropertyData)
	{
		EdsGetPropertySize(inRef, inPropertyID, 0, out var _, out var size);
		IntPtr ptr = Marshal.AllocHGlobal(size);
		uint result = EdsGetPropertyData(inRef, inPropertyID, inParam, size, ptr);
		int len = size;
		outPropertyData = new byte[len];
		Marshal.Copy(ptr, outPropertyData, 0, len);
		Marshal.FreeHGlobal(ptr);
		return result;
	}

	public static byte[] ConvertMWB(EdsManualWBData pcwb)
	{
		int headerSize = 40;
		int MWBHEADERSIZE = 12;
		int datasize = (int)pcwb.dataSize;
		if (datasize < IntPtr.Size)
		{
			datasize = IntPtr.Size;
		}
		int num = (int)pcwb.dataSize + MWBHEADERSIZE + headerSize;
		IntPtr ptr = Marshal.AllocHGlobal(datasize + headerSize);
		pcwb.dataSize += (uint)MWBHEADERSIZE;
		Marshal.StructureToPtr(pcwb, ptr, fDeleteOld: true);
		pcwb.dataSize -= (uint)MWBHEADERSIZE;
		byte[] buff = new byte[num];
		Marshal.Copy(ptr, buff, 0, headerSize);
		int i = 0;
		for (i = 0; i < MWBHEADERSIZE; i++)
		{
			buff[headerSize + i] = 0;
		}
		for (int j = 0; j < pcwb.dataSize; j++)
		{
			buff[headerSize + i + j] = pcwb.data[j];
		}
		Marshal.FreeHGlobal(ptr);
		return buff;
	}

	public static EdsManualWBData MarshalPtrToManualWBData(IntPtr ptr)
	{
		EdsManualWBData userdata = (EdsManualWBData)Marshal.PtrToStructure(ptr, typeof(EdsManualWBData));
		int headerSize = 40;
		byte[] tmp = new byte[userdata.dataSize + headerSize];
		userdata.data = new byte[userdata.dataSize];
		Marshal.Copy(ptr, tmp, 0, (int)userdata.dataSize + headerSize);
		for (int i = 0; i < userdata.dataSize; i++)
		{
			userdata.data[i] = tmp[headerSize + i];
		}
		return userdata;
	}

	[DllImport("EDSDK.dll")]
	public static extern uint EdsSetPropertyData(IntPtr inRef, uint inPropertyID, int inParam, int inPropertySize, [In][MarshalAs(UnmanagedType.AsAny)] object inPropertyData);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsGetPropertyDesc(IntPtr inRef, uint inPropertyID, out EdsPropertyDesc outPropertyDesc);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsGetCameraList(out IntPtr outCameraListRef);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsGetDeviceInfo(IntPtr inCameraRef, out EdsDeviceInfo outDeviceInfo);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsOpenSession(IntPtr inCameraRef);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsCloseSession(IntPtr inCameraRef);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsSendCommand(IntPtr inCameraRef, uint inCommand, int inParam);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsSendStatusCommand(IntPtr inCameraRef, uint inCameraState, int inParam);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsSetCapacity(IntPtr inCameraRef, EdsCapacity inCapacity);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsGetVolumeInfo(IntPtr inCameraRef, out EdsVolumeInfo outVolumeInfo);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsFormatVolume(IntPtr inVolumeRef);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsGetDirectoryItemInfo(IntPtr inDirItemRef, out EdsDirectoryItemInfo outDirItemInfo);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsDeleteDirectoryItem(IntPtr inDirItemRef);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsDownload(IntPtr inDirItemRef, ulong inReadSize, IntPtr outStream);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsDownloadCancel(IntPtr inDirItemRef);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsDownloadComplete(IntPtr inDirItemRef);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsDownloadThumbnail(IntPtr inDirItemRef, IntPtr outStream);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsGetAttribute(IntPtr inDirItemRef, out EdsFileAttribute outFileAttribute);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsSetAttribute(IntPtr inDirItemRef, EdsFileAttribute inFileAttribute);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsCreateFileStream(string inFileName, EdsFileCreateDisposition inCreateDisposition, EdsAccess inDesiredAccess, out IntPtr outStream);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsCreateMemoryStream(ulong inBufferSize, out IntPtr outStream);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsCreateStreamEx(string inFileName, EdsFileCreateDisposition inCreateDisposition, EdsAccess inDesiredAccess, out IntPtr outStream);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsCreateMemoryStreamFromPointer(IntPtr inUserBuffer, ulong inBufferSize, out IntPtr outStream);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsGetPointer(IntPtr inStreamRef, out IntPtr outPointer);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsRead(IntPtr inStreamRef, ulong inReadSize, IntPtr outBuffer, out ulong outReadSize);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsWrite(IntPtr inStreamRef, ulong inWriteSize, IntPtr inBuffer, out uint outWrittenSize);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsSeek(IntPtr inStreamRef, long inSeekOffset, EdsSeekOrigin inSeekOrigin);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsGetPosition(IntPtr inStreamRef, out ulong outPosition);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsGetLength(IntPtr inStreamRef, out ulong outLength);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsCopyData(IntPtr inStreamRef, ulong inWriteSize, IntPtr outStreamRef);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsSetProgressCallback(IntPtr inRef, EdsProgressCallback inProgressFunc, EdsProgressOption inProgressOption, IntPtr inContext);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsCreateImageRef(IntPtr inStreamRef, out IntPtr outImageRef);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsGetImageInfo(IntPtr inImageRef, EdsImageSource inImageSource, out EdsImageInfo outImageInfo);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsGetImage(IntPtr inImageRef, EdsImageSource inImageSource, EdsTargetImageType inImageType, EdsRect inSrcRect, EdsSize inDstSize, IntPtr outStreamRef);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsSetCameraAddedHandler(EdsCameraAddedHandler inCameraAddedHandler, IntPtr inContext);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsSetPropertyEventHandler(IntPtr inCameraRef, uint inEvnet, EdsPropertyEventHandler inPropertyEventHandler, IntPtr inContext);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsSetObjectEventHandler(IntPtr inCameraRef, uint inEvnet, EdsObjectEventHandler inObjectEventHandler, IntPtr inContext);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsSetCameraStateEventHandler(IntPtr inCameraRef, uint inEvnet, EdsStateEventHandler inStateEventHandler, IntPtr inContext);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsSetFramePoint(IntPtr inCameraRef, EdsSize inFramePoint, bool inLockAfFrame);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsCreateEvfImageRef(IntPtr inStreamRef, out IntPtr outEvfImageRef);

	[DllImport("EDSDK.dll")]
	public static extern uint EdsDownloadEvfImage(IntPtr inCameraRef, IntPtr outEvfImageRef);
}

}
