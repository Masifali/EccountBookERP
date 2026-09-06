using System;
using System.Runtime.InteropServices;

namespace Architecture.HvLibrary;

public class CHCNetSDK
{
	public struct NET_DVR_TIME
	{
		public uint dwYear;

		public uint dwMonth;

		public uint dwDay;

		public uint dwHour;

		public uint dwMinute;

		public uint dwSecond;
	}

	public struct NET_DVR_TIME_V30
	{
		public ushort wYear;

		public byte byMonth;

		public byte byDay;

		public byte byHour;

		public byte byMinute;

		public byte bySecond;

		public byte byRes;

		public ushort wMilliSec;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;
	}

	public struct NET_DVR_TIME_EX
	{
		public ushort wYear;

		public byte byMonth;

		public byte byDay;

		public byte byHour;

		public byte byMinute;

		public byte bySecond;

		public byte byRes;
	}

	public struct NET_DVR_SCHEDTIME
	{
		public byte byStartHour;

		public byte byStartMin;

		public byte byStopHour;

		public byte byStopMin;
	}

	public struct NET_DVR_STRUCTHEAD
	{
		public ushort wLength;

		public byte byVersion;

		public byte byRes;
	}

	public struct NET_DVR_HANDLEEXCEPTION_V41
	{
		public uint dwHandleType;

		public uint dwMaxRelAlarmOutChanNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4128, ArraySubType = UnmanagedType.U4)]
		public uint[] dwRelAlarmOut;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_HANDLEEXCEPTION_V40
	{
		public uint dwHandleType;

		public uint dwMaxRelAlarmOutChanNum;

		public uint dwRelAlarmOutChanNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.U4)]
		public uint[] dwRelAlarmOut;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_HANDLEEXCEPTION_V30
	{
		public uint dwHandleType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 96, ArraySubType = UnmanagedType.I1)]
		public byte[] byRelAlarmOut;
	}

	public struct NET_DVR_HANDLEEXCEPTION
	{
		public uint dwHandleType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
		public byte[] byRelAlarmOut;
	}

	public struct NET_DVR_DEVICECFG
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sDVRName;

		public uint dwDVRID;

		public uint dwRecycleRecord;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.I1)]
		public byte[] sSerialNumber;

		public uint dwSoftwareVersion;

		public uint dwSoftwareBuildDate;

		public uint dwDSPSoftwareVersion;

		public uint dwDSPSoftwareBuildDate;

		public uint dwPanelVersion;

		public uint dwHardwareVersion;

		public byte byAlarmInPortNum;

		public byte byAlarmOutPortNum;

		public byte byRS232Num;

		public byte byRS485Num;

		public byte byNetworkPortNum;

		public byte byDiskCtrlNum;

		public byte byDiskNum;

		public byte byDVRType;

		public byte byChanNum;

		public byte byStartChan;

		public byte byDecordChans;

		public byte byVGANum;

		public byte byUSBNum;

		public byte byAuxoutNum;

		public byte byAudioNum;

		public byte byIPChanNum;
	}

	public struct NET_DVR_IPADDR
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sIpV4;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public void Init()
		{
			byRes = new byte[128];
		}
	}

	public struct NET_DVR_ETHERNET_V30
	{
		public NET_DVR_IPADDR struDVRIP;

		public NET_DVR_IPADDR struDVRIPMask;

		public uint dwNetInterface;

		public ushort wDVRPort;

		public ushort wMTU;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
		public byte[] byMACAddr;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_ETHERNET
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sDVRIP;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sDVRIPMask;

		public uint dwNetInterface;

		public ushort wDVRPort;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
		public byte[] byMACAddr;
	}

	public struct NET_DVR_PPPOECFG
	{
		public uint dwPPPOE;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sPPPoEUser;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sPPPoEPassword;

		public NET_DVR_IPADDR struPPPoEIP;
	}

	public struct NET_DVR_NETCFG_V30
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_ETHERNET_V30[] struEtherNet;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_IPADDR[] struRes1;

		public NET_DVR_IPADDR struAlarmHostIpAddr;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;

		public ushort wAlarmHostIpPort;

		public byte byUseDhcp;

		public byte byRes3;

		public NET_DVR_IPADDR struDnsServer1IpAddr;

		public NET_DVR_IPADDR struDnsServer2IpAddr;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byIpResolver;

		public ushort wIpResolverPort;

		public ushort wHttpPortNo;

		public NET_DVR_IPADDR struMulticastIpAddr;

		public NET_DVR_IPADDR struGatewayIpAddr;

		public NET_DVR_PPPOECFG struPPPoE;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_ETHERNET_MULTI
	{
		public NET_DVR_IPADDR struDVRIP;

		public NET_DVR_IPADDR struDVRIPMask;

		public uint dwNetInterface;

		public byte byCardType;

		public byte byRes1;

		public ushort wMTU;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
		public byte[] byMACAddr;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;

		public byte byUseDhcp;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes3;

		public NET_DVR_IPADDR struGatewayIpAddr;

		public NET_DVR_IPADDR struDnsServer1IpAddr;

		public NET_DVR_IPADDR struDnsServer2IpAddr;
	}

	public struct NET_DVR_NETCFG_MULTI
	{
		public uint dwSize;

		public byte byDefaultRoute;

		public byte byNetworkCardNum;

		public byte byWorkMode;

		public byte byRes;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_ETHERNET_MULTI[] struEtherNet;

		public NET_DVR_IPADDR struManageHost1IpAddr;

		public NET_DVR_IPADDR struManageHost2IpAddr;

		public NET_DVR_IPADDR struAlarmHostIpAddr;

		public ushort wManageHost1Port;

		public ushort wManageHost2Port;

		public ushort wAlarmHostIpPort;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byIpResolver;

		public ushort wIpResolverPort;

		public ushort wDvrPort;

		public ushort wHttpPortNo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;

		public NET_DVR_IPADDR struMulticastIpAddr;

		public NET_DVR_PPPOECFG struPPPoE;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes3;
	}

	public struct NET_DVR_NETCFG
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_ETHERNET[] struEtherNet;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sManageHostIP;

		public ushort wManageHostPort;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sIPServerIP;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sMultiCastIP;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sGatewayIP;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sNFSIP;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
		public byte[] sNFSDirectory;

		public uint dwPPPOE;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sPPPoEUser;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sPPPoEPassword;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sPPPoEIP;

		public ushort wHttpPort;
	}

	public struct NET_DVR_SIP_CFG
	{
		public uint dwSize;

		public byte byEnableAutoLogin;

		public byte byLoginStatus;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_DVR_IPADDR stuServerIP;

		public ushort wServerPort;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byUserName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byPassWord;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byLocalNo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
		public byte[] byDispalyName;

		public ushort wLocalPort;

		public byte byLoginCycle;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 129, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_IP_VIEW_DEVCFG
	{
		public uint dwSize;

		public byte byDefaultRing;

		public byte byRingVolume;

		public byte byInputVolume;

		public byte byOutputVolume;

		public ushort wRtpPort;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public uint dwPreviewDelayTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_IP_VIEW_AUDIO_CFG
	{
		public uint dwSize;

		public byte byAudioEncPri1;

		public byte byAudioEncPri2;

		public ushort wAudioPacketLen1;

		public ushort wAudioPacketLen2;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 30, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_IP_VIEW_CALL_CFG
	{
		public uint dwSize;

		public byte byEnableAutoResponse;

		public byte byAudoResponseTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public byte byEnableAlarmNumber1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byAlarmNumber1;

		public byte byEnableAlarmNumber2;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes3;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byAlarmNumber2;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 72, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes4;
	}

	public struct NET_DVR_RECORDCHAN
	{
		public uint dwMaxRecordChanNum;

		public uint dwCurRecordChanNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.U4)]
		public uint dwRelRecordChan;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_MOTION_V30
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6144, ArraySubType = UnmanagedType.I1)]
		public byte[] byMotionScope;

		public byte byMotionSensitive;

		public byte byEnableHandleMotion;

		public byte byEnableDisplay;

		public byte reservedData;

		public NET_DVR_HANDLEEXCEPTION_V30 struMotionHandleType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 56, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SCHEDTIME[] struAlarmTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRelRecordChan;
	}

	public struct NET_DVR_MOTION
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 396, ArraySubType = UnmanagedType.I1)]
		public byte[] byMotionScope;

		public byte byMotionSensitive;

		public byte byEnableHandleMotion;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
		public string reservedData;

		public NET_DVR_HANDLEEXCEPTION strMotionHandleType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 28, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SCHEDTIME[] struAlarmTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byRelRecordChan;
	}

	public struct NET_DVR_HIDEALARM_V30
	{
		public uint dwEnableHideAlarm;

		public ushort wHideAlarmAreaTopLeftX;

		public ushort wHideAlarmAreaTopLeftY;

		public ushort wHideAlarmAreaWidth;

		public ushort wHideAlarmAreaHeight;

		public NET_DVR_HANDLEEXCEPTION_V30 strHideAlarmHandleType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 56, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SCHEDTIME[] struAlarmTime;
	}

	public struct NET_DVR_HIDEALARM
	{
		public uint dwEnableHideAlarm;

		public ushort wHideAlarmAreaTopLeftX;

		public ushort wHideAlarmAreaTopLeftY;

		public ushort wHideAlarmAreaWidth;

		public ushort wHideAlarmAreaHeight;

		public NET_DVR_HANDLEEXCEPTION strHideAlarmHandleType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 28, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SCHEDTIME[] struAlarmTime;
	}

	public struct NET_DVR_VILOST_V30
	{
		public byte byEnableHandleVILost;

		public NET_DVR_HANDLEEXCEPTION_V30 strVILostHandleType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 56, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SCHEDTIME[] struAlarmTime;
	}

	public struct NET_DVR_VILOST
	{
		public byte byEnableHandleVILost;

		public NET_DVR_HANDLEEXCEPTION strVILostHandleType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 56, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SCHEDTIME[] struAlarmTime;
	}

	public struct NET_DVR_SHELTER
	{
		public ushort wHideAreaTopLeftX;

		public ushort wHideAreaTopLeftY;

		public ushort wHideAreaWidth;

		public ushort wHideAreaHeight;
	}

	public struct NET_DVR_COLOR
	{
		public byte byBrightness;

		public byte byContrast;

		public byte bySaturation;

		public byte byHue;
	}

	public struct NET_DVR_PICCFG_V30
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sChanName;

		public uint dwVideoFormat;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byReservedData;

		public uint dwShowChanName;

		public ushort wShowNameTopLeftX;

		public ushort wShowNameTopLeftY;

		public NET_DVR_VILOST_V30 struVILost;

		public NET_DVR_VILOST_V30 struRes;

		public NET_DVR_MOTION_V30 struMotion;

		public NET_DVR_HIDEALARM_V30 struHideAlarm;

		public uint dwEnableHide;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SHELTER[] struShelter;

		public uint dwShowOsd;

		public ushort wOSDTopLeftX;

		public ushort wOSDTopLeftY;

		public byte byOSDType;

		public byte byDispWeek;

		public byte byOSDAttrib;

		public byte byHourOSDType;

		public byte byFontSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 63, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_PICCFG_EX
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sChanName;

		public uint dwVideoFormat;

		public byte byBrightness;

		public byte byContrast;

		public byte bySaturation;

		public byte byHue;

		public uint dwShowChanName;

		public ushort wShowNameTopLeftX;

		public ushort wShowNameTopLeftY;

		public NET_DVR_VILOST struVILost;

		public NET_DVR_MOTION struMotion;

		public NET_DVR_HIDEALARM struHideAlarm;

		public uint dwEnableHide;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SHELTER[] struShelter;

		public uint dwShowOsd;

		public ushort wOSDTopLeftX;

		public ushort wOSDTopLeftY;

		public byte byOSDType;

		public byte byDispWeek;

		public byte byOSDAttrib;

		public byte byHourOsdType;
	}

	public struct NET_DVR_PICCFG
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sChanName;

		public uint dwVideoFormat;

		public byte byBrightness;

		public byte byContrast;

		public byte bySaturation;

		public byte byHue;

		public uint dwShowChanName;

		public ushort wShowNameTopLeftX;

		public ushort wShowNameTopLeftY;

		public NET_DVR_VILOST struVILost;

		public NET_DVR_MOTION struMotion;

		public NET_DVR_HIDEALARM struHideAlarm;

		public uint dwEnableHide;

		public ushort wHideAreaTopLeftX;

		public ushort wHideAreaTopLeftY;

		public ushort wHideAreaWidth;

		public ushort wHideAreaHeight;

		public uint dwShowOsd;

		public ushort wOSDTopLeftX;

		public ushort wOSDTopLeftY;

		public byte byOSDType;

		public byte byDispWeek;

		public byte byOSDAttrib;

		public byte reservedData2;
	}

	public struct NET_DVR_COMPRESSION_INFO_V30
	{
		public byte byStreamType;

		public byte byResolution;

		public byte byBitrateType;

		public byte byPicQuality;

		public uint dwVideoBitrate;

		public uint dwVideoFrameRate;

		public ushort wIntervalFrameI;

		public byte byIntervalBPFrame;

		public byte byres1;

		public byte byVideoEncType;

		public byte byAudioEncType;

		public byte byVideoEncComplexity;

		public byte byEnableSvc;

		public byte byFormatType;

		public byte byAudioBitRate;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
		public byte[] byres;
	}

	public struct NET_DVR_COMPRESSIONCFG_V30
	{
		public uint dwSize;

		public NET_DVR_COMPRESSION_INFO_V30 struNormHighRecordPara;

		public NET_DVR_COMPRESSION_INFO_V30 struRes;

		public NET_DVR_COMPRESSION_INFO_V30 struEventRecordPara;

		public NET_DVR_COMPRESSION_INFO_V30 struNetPara;
	}

	public struct NET_DVR_COMPRESSION_INFO
	{
		public byte byStreamType;

		public byte byResolution;

		public byte byBitrateType;

		public byte byPicQuality;

		public uint dwVideoBitrate;

		public uint dwVideoFrameRate;
	}

	public struct NET_DVR_COMPRESSIONCFG
	{
		public uint dwSize;

		public NET_DVR_COMPRESSION_INFO struRecordPara;

		public NET_DVR_COMPRESSION_INFO struNetPara;
	}

	public struct NET_DVR_COMPRESSION_INFO_EX
	{
		public byte byStreamType;

		public byte byResolution;

		public byte byBitrateType;

		public byte byPicQuality;

		public uint dwVideoBitrate;

		public uint dwVideoFrameRate;

		public ushort wIntervalFrameI;

		public byte byIntervalBPFrame;

		public byte byRes;
	}

	public struct NET_DVR_COMPRESSIONCFG_EX
	{
		public uint dwSize;

		public NET_DVR_COMPRESSION_INFO_EX struRecordPara;

		public NET_DVR_COMPRESSION_INFO_EX struNetPara;
	}

	public struct NET_DVR_RECORDSCHED
	{
		public NET_DVR_SCHEDTIME struRecordTime;

		public byte byRecordType;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
		public string reservedData;
	}

	public struct NET_DVR_RECORDDAY
	{
		public ushort wAllDayRecord;

		public byte byRecordType;

		public byte reservedData;
	}

	public struct NET_DVR_RECORD_V30
	{
		public uint dwSize;

		public uint dwRecord;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_RECORDDAY[] struRecAllDay;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 56, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_RECORDSCHED[] struRecordSched;

		public uint dwRecordTime;

		public uint dwPreRecordTime;

		public uint dwRecorderDuration;

		public byte byRedundancyRec;

		public byte byAudioRec;

		public byte byStreamType;

		public byte byPassbackRecord;

		public ushort wLockDuration;

		public byte byRecordBackup;

		public byte bySVCLevel;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
		public byte[] byReserve;
	}

	public struct NET_DVR_RECORD
	{
		public uint dwSize;

		public uint dwRecord;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_RECORDDAY[] struRecAllDay;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 28, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_RECORDSCHED[] struRecordSched;

		public uint dwRecordTime;

		public uint dwPreRecordTime;
	}

	public struct NET_DVR_PTZ_PROTOCOL
	{
		public uint dwType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byDescribe;
	}

	public struct NET_DVR_PTZCFG
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 200, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_PTZ_PROTOCOL[] struPtz;

		public uint dwPtzNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_DECODERCFG_V30
	{
		public uint dwSize;

		public uint dwBaudRate;

		public byte byDataBit;

		public byte byStopBit;

		public byte byParity;

		public byte byFlowcontrol;

		public ushort wDecoderType;

		public ushort wDecoderAddress;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.I1)]
		public byte[] bySetPreset;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.I1)]
		public byte[] bySetCruise;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.I1)]
		public byte[] bySetTrack;
	}

	public struct NET_DVR_DECODERCFG
	{
		public uint dwSize;

		public uint dwBaudRate;

		public byte byDataBit;

		public byte byStopBit;

		public byte byParity;

		public byte byFlowcontrol;

		public ushort wDecoderType;

		public ushort wDecoderAddress;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
		public byte[] bySetPreset;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
		public byte[] bySetCruise;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
		public byte[] bySetTrack;
	}

	public struct NET_DVR_PPPCFG_V30
	{
		public NET_DVR_IPADDR struRemoteIP;

		public NET_DVR_IPADDR struLocalIP;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sLocalIPMask;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sUsername;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] sPassword;

		public byte byPPPMode;

		public byte byRedial;

		public byte byRedialMode;

		public byte byDataEncrypt;

		public uint dwMTU;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string sTelephoneNumber;
	}

	public struct NET_DVR_PPPCFG
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sRemoteIP;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sLocalIP;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sLocalIPMask;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sUsername;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] sPassword;

		public byte byPPPMode;

		public byte byRedial;

		public byte byRedialMode;

		public byte byDataEncrypt;

		public uint dwMTU;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string sTelephoneNumber;
	}

	public struct NET_DVR_SINGLE_RS232
	{
		public uint dwBaudRate;

		public byte byDataBit;

		public byte byStopBit;

		public byte byParity;

		public byte byFlowcontrol;

		public uint dwWorkMode;
	}

	public struct NET_DVR_RS232CFG_V30
	{
		public uint dwSize;

		public NET_DVR_SINGLE_RS232 struRs232;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 84, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public NET_DVR_PPPCFG_V30 struPPPConfig;
	}

	public struct NET_DVR_RS232CFG
	{
		public uint dwSize;

		public uint dwBaudRate;

		public byte byDataBit;

		public byte byStopBit;

		public byte byParity;

		public byte byFlowcontrol;

		public uint dwWorkMode;

		public NET_DVR_PPPCFG struPPPConfig;
	}

	public struct NET_DVR_PRESETCHAN_INFO
	{
		public uint dwEnablePresetChan;

		public uint dwPresetPointNo;
	}

	public struct NET_DVR_CRUISECHAN_INFO
	{
		public uint dwEnableCruiseChan;

		public uint dwCruiseNo;
	}

	public struct NET_DVR_PTZTRACKCHAN_INFO
	{
		public uint dwEnablePtzTrackChan;

		public uint dwPtzTrackNo;
	}

	public struct NET_DVR_ALARMINCFG_V40
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sAlarmInName;

		public byte byAlarmType;

		public byte byAlarmInHandle;

		public byte byChannel;

		public byte byRes1;

		public uint dwHandleType;

		public uint dwMaxRelAlarmOutChanNum;

		public uint dwRelAlarmOutChanNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4128, ArraySubType = UnmanagedType.U4)]
		public uint[] dwRelAlarmOut;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 56, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SCHEDTIME[] struAlarmTime;

		public uint dwMaxRecordChanNum;

		public uint dwCurRecordChanNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.U4)]
		public uint[] dwRelRecordChan;

		public uint dwMaxEnablePtzCtrlNun;

		public uint dwEnablePresetChanNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_PRESETCHAN_INFO[] struPresetChanInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 516, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;

		public uint dwEnableCruiseChanNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_CRUISECHAN_INFO[] struCruiseChanInfo;

		public uint dwEnablePtzTrackChanNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_PTZTRACKCHAN_INFO[] struPtzTrackInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_ALARMINCFG_V30
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sAlarmInName;

		public byte byAlarmType;

		public byte byAlarmInHandle;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_DVR_HANDLEEXCEPTION_V30 struAlarmHandleType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 56, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SCHEDTIME[] struAlarmTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRelRecordChan;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byEnablePreset;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byPresetNo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 192, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byEnableCruise;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byCruiseNo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byEnablePtzTrack;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byPTZTrack;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes3;
	}

	public struct STRUCT_IO_ALARM
	{
		public uint dwAlarmInputNo;

		public uint dwTrigerAlarmOutNum;

		public uint dwTrigerRecordChanNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 116, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct STRUCT_ALARM_CHANNEL
	{
		public uint dwAlarmChanNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 124, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct STRUCT_ALARM_HD
	{
		public uint dwAlarmHardDiskNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 124, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct UNION_ALARMINFO_FIXED
	{
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
		public byte[] byUnionLen;

		[FieldOffset(0)]
		public STRUCT_IO_ALARM struIOAlarm;

		[FieldOffset(0)]
		public STRUCT_ALARM_CHANNEL struAlarmChannel;

		[FieldOffset(0)]
		public STRUCT_ALARM_HD struAlarmHardDisk;
	}

	public struct NET_DVR_ALRAM_FIXED_HEADER
	{
		public uint dwAlarmType;

		public NET_DVR_TIME_EX struAlarmTime;

		public UNION_ALARMINFO_FIXED uStruAlarm;
	}

	public struct NET_DVR_ALARMINFO_V40
	{
		public NET_DVR_ALRAM_FIXED_HEADER struAlarmFixedHeader;

		public IntPtr pAlarmData;
	}

	public struct NET_DVR_ALARMINCFG
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sAlarmInName;

		public byte byAlarmType;

		public byte byAlarmInHandle;

		public byte byChannel;

		public byte byRes;

		public NET_DVR_HANDLEEXCEPTION struAlarmHandleType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 28, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SCHEDTIME[] struAlarmTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byRelRecordChan;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byEnablePreset;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byPresetNo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byEnableCruise;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byCruiseNo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byEnablePtzTrack;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byPTZTrack;
	}

	public struct NET_DVR_ANALOG_ALARMINCFG
	{
		public uint dwSize;

		public byte byEnableAlarmHandle;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byAlarmInName;

		public ushort wAlarmInUpper;

		public ushort wAlarmInLower;

		public NET_DVR_HANDLEEXCEPTION_V30 struAlarmHandleType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 56, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SCHEDTIME[] struAlarmTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRelRecordChan;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 100, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_ALARMINFO_V30
	{
		public uint dwAlarmType;

		public uint dwAlarmInputNumber;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 96, ArraySubType = UnmanagedType.I1)]
		public byte[] byAlarmOutputNumber;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byAlarmRelateChannel;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byChannel;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 33, ArraySubType = UnmanagedType.I1)]
		public byte[] byDiskNumber;

		public void Init()
		{
			dwAlarmType = 0u;
			dwAlarmInputNumber = 0u;
			byAlarmRelateChannel = new byte[64];
			byChannel = new byte[64];
			byAlarmOutputNumber = new byte[96];
			byDiskNumber = new byte[33];
			for (int k = 0; k < 64; k++)
			{
				byAlarmRelateChannel[k] = Convert.ToByte(0);
				byChannel[k] = Convert.ToByte(0);
			}
			for (int j = 0; j < 96; j++)
			{
				byAlarmOutputNumber[j] = Convert.ToByte(0);
			}
			for (int i = 0; i < 33; i++)
			{
				byDiskNumber[i] = Convert.ToByte(0);
			}
		}

		public void Reset()
		{
			dwAlarmType = 0u;
			dwAlarmInputNumber = 0u;
			for (int k = 0; k < 64; k++)
			{
				byAlarmRelateChannel[k] = Convert.ToByte(0);
				byChannel[k] = Convert.ToByte(0);
			}
			for (int j = 0; j < 96; j++)
			{
				byAlarmOutputNumber[j] = Convert.ToByte(0);
			}
			for (int i = 0; i < 33; i++)
			{
				byDiskNumber[i] = Convert.ToByte(0);
			}
		}
	}

	public struct NET_DVR_ALARM_HOT_SPARE
	{
		public uint dwSize;

		public uint dwExceptionCase;

		public NET_DVR_IPADDR struDeviceIP;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_ALARMINFO
	{
		public int dwAlarmType;

		public int dwAlarmInputNumber;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U4)]
		public int[] dwAlarmOutputNumber;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.U4)]
		public int[] dwAlarmRelateChannel;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.U4)]
		public int[] dwChannel;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.U4)]
		public int[] dwDiskNumber;

		public void Init()
		{
			dwAlarmType = 0;
			dwAlarmInputNumber = 0;
			dwAlarmOutputNumber = new int[4];
			dwAlarmRelateChannel = new int[16];
			dwChannel = new int[16];
			dwDiskNumber = new int[16];
			for (int k = 0; k < 4; k++)
			{
				dwAlarmOutputNumber[k] = 0;
			}
			for (int j = 0; j < 16; j++)
			{
				dwAlarmRelateChannel[j] = 0;
				dwChannel[j] = 0;
			}
			for (int i = 0; i < 16; i++)
			{
				dwDiskNumber[i] = 0;
			}
		}

		public void Reset()
		{
			dwAlarmType = 0;
			dwAlarmInputNumber = 0;
			for (int k = 0; k < 4; k++)
			{
				dwAlarmOutputNumber[k] = 0;
			}
			for (int j = 0; j < 16; j++)
			{
				dwAlarmRelateChannel[j] = 0;
				dwChannel[j] = 0;
			}
			for (int i = 0; i < 16; i++)
			{
				dwDiskNumber[i] = 0;
			}
		}
	}

	public struct NET_DVR_IPDEVINFO
	{
		public uint dwEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sUserName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] sPassword;

		public NET_DVR_IPADDR struIP;

		public ushort wDVRPort;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 34, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public void Init()
		{
			sUserName = new byte[32];
			sPassword = new byte[16];
			byRes = new byte[34];
		}
	}

	public struct NET_DVR_IPDEVINFO_V31
	{
		public byte byEnable;

		public byte byProType;

		public byte byEnableQuickAdd;

		public byte byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sUserName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] sPassword;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byDomain;

		public NET_DVR_IPADDR struIP;

		public ushort wDVRPort;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 34, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;

		public void Init()
		{
			sUserName = new byte[32];
			sPassword = new byte[16];
			byDomain = new byte[64];
			byRes2 = new byte[34];
		}
	}

	public struct NET_DVR_IPCHANINFO
	{
		public byte byEnable;

		public byte byIPID;

		public byte byChannel;

		public byte byIPIDHigh;

		public byte byTransProtocol;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 31, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public void Init()
		{
			byRes = new byte[31];
		}
	}

	public struct NET_DVR_IPPARACFG
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_IPDEVINFO[] struIPDevInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byAnalogChanEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_IPCHANINFO[] struIPChanInfo;

		public void Init()
		{
			int i = 0;
			struIPDevInfo = new NET_DVR_IPDEVINFO[32];
			for (i = 0; i < 32; i++)
			{
				struIPDevInfo[i].Init();
			}
			byAnalogChanEnable = new byte[32];
			struIPChanInfo = new NET_DVR_IPCHANINFO[32];
			for (i = 0; i < 32; i++)
			{
				struIPChanInfo[i].Init();
			}
		}
	}

	public struct NET_DVR_IPPARACFG_V31
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_IPDEVINFO_V31[] struIPDevInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byAnalogChanEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_IPCHANINFO[] struIPChanInfo;

		public void Init()
		{
			int i = 0;
			struIPDevInfo = new NET_DVR_IPDEVINFO_V31[32];
			for (i = 0; i < 32; i++)
			{
				struIPDevInfo[i].Init();
			}
			byAnalogChanEnable = new byte[32];
			struIPChanInfo = new NET_DVR_IPCHANINFO[32];
			for (i = 0; i < 32; i++)
			{
				struIPChanInfo[i].Init();
			}
		}
	}

	public struct NET_DVR_IPSERVER_STREAM
	{
		public byte byEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public NET_DVR_IPADDR struIPServer;

		public ushort wPort;

		public ushort wDvrNameLen;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byDVRName;

		public ushort wDVRSerialLen;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U2)]
		public ushort[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.I1)]
		public byte[] byDVRSerialNumber;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byUserName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byPassWord;

		public byte byChannel;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 11, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;

		public void Init()
		{
			byRes = new byte[3];
			byDVRName = new byte[32];
			byRes1 = new ushort[2];
			byDVRSerialNumber = new byte[48];
			byUserName = new byte[32];
			byPassWord = new byte[16];
			byRes2 = new byte[11];
		}
	}

	public struct NET_DVR_STREAM_MEDIA_SERVER_CFG
	{
		public byte byValid;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_DVR_IPADDR struDevIP;

		public ushort wDevPort;

		public byte byTransmitType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 69, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_DEV_CHAN_INFO
	{
		public NET_DVR_IPADDR struIP;

		public ushort wDVRPort;

		public byte byChannel;

		public byte byTransProtocol;

		public byte byTransMode;

		public byte byFactoryType;

		public byte byDeviceType;

		public byte byDispChan;

		public byte bySubDispChan;

		public byte byResolution;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byDomain;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sUserName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] sPassword;
	}

	public struct NET_DVR_PU_STREAM_CFG
	{
		public uint dwSize;

		public NET_DVR_STREAM_MEDIA_SERVER_CFG struStreamMediaSvrCfg;

		public NET_DVR_DEV_CHAN_INFO struDevChanInfo;
	}

	public struct NET_DVR_DDNS_STREAM_CFG
	{
		public byte byEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_DVR_IPADDR struStreamServer;

		public ushort wStreamServerPort;

		public byte byStreamServerTransmitType;

		public byte byRes2;

		public NET_DVR_IPADDR struIPServer;

		public ushort wIPServerPort;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes3;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sDVRName;

		public ushort wDVRNameLen;

		public ushort wDVRSerialLen;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.I1)]
		public byte[] sDVRSerialNumber;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sUserName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] sPassWord;

		public ushort wDVRPort;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes4;

		public byte byChannel;

		public byte byTransProtocol;

		public byte byTransMode;

		public byte byFactoryType;

		public void Init()
		{
			byRes1 = new byte[3];
			byRes3 = new byte[2];
			sDVRName = new byte[32];
			sDVRSerialNumber = new byte[48];
			sUserName = new byte[32];
			sPassWord = new byte[16];
			byRes4 = new byte[2];
		}
	}

	public struct NET_DVR_PU_STREAM_URL
	{
		public byte byEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 240, ArraySubType = UnmanagedType.I1)]
		public byte[] strURL;

		public byte byTransPortocol;

		public ushort wIPID;

		public byte byChannel;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public void Init()
		{
			strURL = new byte[240];
			byRes = new byte[7];
		}
	}

	public struct NET_DVR_HKDDNS_STREAM
	{
		public byte byEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byDDNSDomain;

		public ushort wPort;

		public ushort wAliasLen;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byAlias;

		public ushort wDVRSerialLen;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.I1)]
		public byte[] byDVRSerialNumber;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byUserName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byPassWord;

		public byte byChannel;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 11, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;

		public void Init()
		{
			byRes = new byte[3];
			byDDNSDomain = new byte[64];
			byAlias = new byte[32];
			byRes1 = new byte[2];
			byDVRSerialNumber = new byte[48];
			byUserName = new byte[32];
			byPassWord = new byte[16];
			byRes2 = new byte[11];
		}
	}

	public struct NET_DVR_IPCHANINFO_V40
	{
		public byte byEnable;

		public byte byRes1;

		public ushort wIPID;

		public uint dwChannel;

		public byte byTransProtocol;

		public byte byTransMode;

		public byte byFactoryType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 241, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct NET_DVR_GET_STREAM_UNION
	{
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 492, ArraySubType = UnmanagedType.I1)]
		public byte[] byUnion;
	}

	public struct NET_DVR_STREAM_MODE
	{
		public byte byGetStreamType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public NET_DVR_GET_STREAM_UNION uGetStream;

		public void Init()
		{
			byGetStreamType = 0;
			byRes = new byte[3];
		}
	}

	public struct NET_DVR_IPPARACFG_V40
	{
		public uint dwSize;

		public uint dwGroupNum;

		public uint dwAChanNum;

		public uint dwDChanNum;

		public uint dwStartDChan;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byAnalogChanEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_IPDEVINFO_V31[] struIPDevInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_STREAM_MODE[] struStreamMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_ALARMINFO_DEV
	{
		public uint dwAlarmType;

		public NET_DVR_TIME struTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public uint dwNumber;

		public IntPtr pNO;
	}

	public struct NET_DVR_IPALARMOUTINFO
	{
		public byte byIPID;

		public byte byAlarmOut;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 18, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public void Init()
		{
			byRes = new byte[18];
		}
	}

	public struct NET_DVR_IPALARMOUTCFG
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_IPALARMOUTINFO[] struIPAlarmOutInfo;

		public void Init()
		{
			struIPAlarmOutInfo = new NET_DVR_IPALARMOUTINFO[64];
			for (int i = 0; i < 64; i++)
			{
				struIPAlarmOutInfo[i].Init();
			}
		}
	}

	public struct NET_DVR_IPALARMOUTINFO_V40
	{
		public uint dwIPID;

		public uint dwAlarmOut;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_IPALARMOUTCFG_V40
	{
		public uint dwSize;

		public uint dwCurIPAlarmOutNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4096, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_IPALARMOUTINFO_V40[] struIPAlarmOutInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_IPALARMININFO
	{
		public byte byIPID;

		public byte byAlarmIn;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 18, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_IPALARMINCFG
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_IPALARMININFO[] struIPAlarmInInfo;
	}

	public struct NET_DVR_IPALARMININFO_V40
	{
		public uint dwIPID;

		public uint dwAlarmIn;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_IPALARMINCFG_V40
	{
		public uint dwSize;

		public uint dwCurIPAlarmInNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4096, ArraySubType = UnmanagedType.I1)]
		public NET_DVR_IPALARMININFO_V40[] struIPAlarmInInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_IPALARMINFO
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_IPDEVINFO[] struIPDevInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byAnalogChanEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_IPCHANINFO[] struIPChanInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_IPALARMININFO[] struIPAlarmInInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_IPALARMOUTINFO[] struIPAlarmOutInfo;
	}

	public struct NET_DVR_IPALARMINFO_V31
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_IPDEVINFO_V31[] struIPDevInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byAnalogChanEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_IPCHANINFO[] struIPChanInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_IPALARMININFO[] struIPAlarmInInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_IPALARMOUTINFO[] struIPAlarmOutInfo;
	}

	public struct NET_DVR_IPALARMINFO_V40
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_IPDEVINFO_V31[] struIPDevInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byAnalogChanEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_IPCHANINFO[] struIPChanInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_IPALARMININFO[] struIPAlarmInInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_IPALARMOUTINFO[] struIPAlarmOutInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public enum HD_STAT
	{
		HD_STAT_OK = 0,
		HD_STAT_UNFORMATTED = 1,
		HD_STAT_ERROR = 2,
		HD_STAT_SMART_FAILED = 3,
		HD_STAT_MISMATCH = 4,
		HD_STAT_IDLE = 5,
		NET_HD_STAT_OFFLINE = 6,
		HD_RIADVD_EXPAND = 7,
		HD_STAT_REPARING = 10,
		HD_STAT_FORMATING = 11
	}

	public struct NET_DVR_SINGLE_HD
	{
		public uint dwHDNo;

		public uint dwCapacity;

		public uint dwFreeSpace;

		public uint dwHdStatus;

		public byte byHDAttr;

		public byte byHDType;

		public byte byDiskDriver;

		public byte byRes1;

		public uint dwHdGroup;

		public byte byRecycling;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;

		public uint dwStorageType;

		public uint dwPictureCapacity;

		public uint dwFreePictureSpace;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 104, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes3;
	}

	public struct NET_DVR_HDCFG
	{
		public uint dwSize;

		public uint dwHDCount;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 33, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SINGLE_HD[] struHDInfo;
	}

	public struct NET_DVR_SINGLE_HDGROUP_V40
	{
		public uint dwHDGroupNo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.U4)]
		public uint[] dwRelRecordChan;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_HDGROUP_CFG_V40
	{
		public uint dwSize;

		public uint dwMaxHDGroupNum;

		public uint dwCurHDGroupNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SINGLE_HDGROUP_V40[] struHDGroupAttr;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_SINGLE_HDGROUP
	{
		public uint dwHDGroupNo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byHDGroupChans;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_HDGROUP_CFG
	{
		public uint dwSize;

		public uint dwHDGroupCount;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SINGLE_HDGROUP[] struHDGroupAttr;
	}

	public struct NET_DVR_SCALECFG
	{
		public uint dwSize;

		public uint dwMajorScale;

		public uint dwMinorScale;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
		public uint[] dwRes;
	}

	public struct NET_DVR_ALARMOUTCFG_V30
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sAlarmOutName;

		public uint dwAlarmOutDelay;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 56, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SCHEDTIME[] struAlarmOutTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_ALARMOUTCFG
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sAlarmOutName;

		public uint dwAlarmOutDelay;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 28, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SCHEDTIME[] struAlarmOutTime;
	}

	public struct NET_DVR_PREVIEWCFG_V30
	{
		public uint dwSize;

		public byte byPreviewNumber;

		public byte byEnableAudio;

		public ushort wSwitchTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.I1)]
		public byte[] bySwitchSeq;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_PREVIEWCFG
	{
		public uint dwSize;

		public byte byPreviewNumber;

		public byte byEnableAudio;

		public ushort wSwitchTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] bySwitchSeq;
	}

	public struct NET_DVR_VGAPARA
	{
		public ushort wResolution;

		public ushort wFreq;

		public uint dwBrightness;
	}

	public struct NET_DVR_MATRIXPARA_V30
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.U2)]
		public ushort[] wOrder;

		public ushort wSwitchTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 14, ArraySubType = UnmanagedType.I1)]
		public byte[] res;
	}

	public struct NET_DVR_MATRIXPARA
	{
		public ushort wDisplayLogo;

		public ushort wDisplayOsd;
	}

	public struct NET_DVR_VOOUT
	{
		public byte byVideoFormat;

		public byte byMenuAlphaValue;

		public ushort wScreenSaveTime;

		public ushort wVOffset;

		public ushort wBrightness;

		public byte byStartMode;

		public byte byEnableScaler;
	}

	public struct NET_DVR_VIDEOOUT_V30
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_VOOUT[] struVOOut;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_VGAPARA[] struVGAPara;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_MATRIXPARA_V30[] struMatrixPara;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_VIDEOOUT
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_VOOUT[] struVOOut;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_VGAPARA[] struVGAPara;

		public NET_DVR_MATRIXPARA struMatrixPara;
	}

	public struct NET_DVR_USER_INFO_V40
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sUserName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] sPassword;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byLocalRight;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRemoteRight;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.U4)]
		public uint[] dwNetPreviewRight;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.U4)]
		public uint[] dwLocalRecordRight;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.U4)]
		public uint[] dwNetRecordRight;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.U4)]
		public uint[] dwLocalPlaybackRight;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.U4)]
		public uint[] dwNetPlaybackRight;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.U4)]
		public uint[] dwLocalPTZRight;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.U4)]
		public uint[] dwNetPTZRight;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.U4)]
		public uint[] dwLocalBackupRight;

		public NET_DVR_IPADDR struUserIP;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
		public byte[] byMACAddr;

		public byte byPriority;

		public byte byAlarmOnRight;

		public byte byAlarmOffRight;

		public byte byBypassRight;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 118, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_USER_INFO_V30
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sUserName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] sPassword;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byLocalRight;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRemoteRight;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byNetPreviewRight;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byLocalPlaybackRight;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byNetPlaybackRight;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byLocalRecordRight;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byNetRecordRight;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byLocalPTZRight;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byNetPTZRight;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byLocalBackupRight;

		public NET_DVR_IPADDR struUserIP;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
		public byte[] byMACAddr;

		public byte byPriority;

		public byte byAlarmOnRight;

		public byte byAlarmOffRight;

		public byte byBypassRight;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 14, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_USER_INFO_EX
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sUserName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] sPassword;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.U4)]
		public uint[] dwLocalRight;

		public uint dwLocalPlaybackRight;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.U4)]
		public uint[] dwRemoteRight;

		public uint dwNetPreviewRight;

		public uint dwNetPlaybackRight;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sUserIP;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
		public byte[] byMACAddr;
	}

	public struct NET_DVR_USER_INFO
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sUserName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] sPassword;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.U4)]
		public uint[] dwLocalRight;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.U4)]
		public uint[] dwRemoteRight;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sUserIP;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
		public byte[] byMACAddr;
	}

	public struct NET_DVR_USER_V40
	{
		public uint dwSize;

		public uint dwMaxUserNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_USER_INFO_V40[] struUser;
	}

	public struct NET_DVR_USER_V30
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_USER_INFO_V30[] struUser;
	}

	public struct NET_DVR_USER_EX
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_USER_INFO_EX[] struUser;
	}

	public struct NET_DVR_USER
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_USER_INFO[] struUser;
	}

	public struct NET_DVR_EXCEPTION_V40
	{
		public uint dwSize;

		public uint dwMaxGroupNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_HANDLEEXCEPTION_V41[] struExceptionHandle;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_EXCEPTION_V30
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_HANDLEEXCEPTION_V30[] struExceptionHandleType;
	}

	public struct NET_DVR_EXCEPTION
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_HANDLEEXCEPTION[] struExceptionHandleType;
	}

	public struct NET_DVR_CHANNELSTATE_V30
	{
		public byte byRecordStatic;

		public byte bySignalStatic;

		public byte byHardwareStatic;

		public byte byRes1;

		public uint dwBitRate;

		public uint dwLinkNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_IPADDR[] struClientIP;

		public uint dwIPLinkNum;

		public byte byExceedMaxLink;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public uint dwChannelNo;

		public void Init()
		{
			struClientIP = new NET_DVR_IPADDR[6];
			for (int i = 0; i < 6; i++)
			{
				struClientIP[i].Init();
			}
			byRes = new byte[12];
		}
	}

	public struct NET_DVR_CHANNELSTATE
	{
		public byte byRecordStatic;

		public byte bySignalStatic;

		public byte byHardwareStatic;

		public byte reservedData;

		public uint dwBitRate;

		public uint dwLinkNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.U4)]
		public uint[] dwClientIP;
	}

	public struct NET_DVR_DISKSTATE
	{
		public uint dwVolume;

		public uint dwFreeSpace;

		public uint dwHardDiskStatic;
	}

	public struct NET_DVR_WORKSTATE_V40
	{
		public uint dwSize;

		public uint dwDeviceStatic;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 33, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_DISKSTATE[] struHardDiskStatic;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_CHANNELSTATE_V30[] struChanStatic;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4128, ArraySubType = UnmanagedType.U4)]
		public uint[] dwHasAlarmInStatic;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4128, ArraySubType = UnmanagedType.U4)]
		public uint[] dwHasAlarmOutStatic;

		public uint dwLocalDisplay;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byAudioInChanStatus;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 126, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_GETWORKSTATE_COND
	{
		public uint dwSize;

		public byte byFindHardByCond;

		public byte byFindChanByCond;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 33, ArraySubType = UnmanagedType.U4)]
		public uint[] dwFindHardStatus;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.U4)]
		public uint[] dwFindChanNo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_WORKSTATE_V30
	{
		public uint dwDeviceStatic;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 33, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_DISKSTATE[] struHardDiskStatic;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_CHANNELSTATE_V30[] struChanStatic;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 160, ArraySubType = UnmanagedType.I1)]
		public byte[] byAlarmInStatic;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 96, ArraySubType = UnmanagedType.I1)]
		public byte[] byAlarmOutStatic;

		public uint dwLocalDisplay;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byAudioChanStatus;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public void Init()
		{
			struHardDiskStatic = new NET_DVR_DISKSTATE[33];
			struChanStatic = new NET_DVR_CHANNELSTATE_V30[64];
			for (int i = 0; i < 64; i++)
			{
				struChanStatic[i].Init();
			}
			byAlarmInStatic = new byte[96];
			byAlarmOutStatic = new byte[96];
			byAudioChanStatus = new byte[2];
			byRes = new byte[10];
		}
	}

	public struct NET_DVR_WORKSTATE
	{
		public uint dwDeviceStatic;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_DISKSTATE[] struHardDiskStatic;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_CHANNELSTATE[] struChanStatic;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byAlarmInStatic;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
		public byte[] byAlarmOutStatic;

		public uint dwLocalDisplay;

		public void Init()
		{
			struHardDiskStatic = new NET_DVR_DISKSTATE[16];
			struChanStatic = new NET_DVR_CHANNELSTATE[16];
			byAlarmInStatic = new byte[16];
			byAlarmOutStatic = new byte[4];
		}
	}

	public struct NET_DVR_LOG_V30
	{
		public NET_DVR_TIME strLogTime;

		public uint dwMajorType;

		public uint dwMinorType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] sPanelUser;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] sNetUser;

		public NET_DVR_IPADDR struRemoteHostAddr;

		public uint dwParaType;

		public uint dwChannel;

		public uint dwDiskNumber;

		public uint dwAlarmInPort;

		public uint dwAlarmOutPort;

		public uint dwInfoLen;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11840)]
		public string sInfo;
	}

	public struct NET_DVR_LOG
	{
		public NET_DVR_TIME strLogTime;

		public uint dwMajorType;

		public uint dwMinorType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] sPanelUser;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] sNetUser;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sRemoteHostAddr;

		public uint dwParaType;

		public uint dwChannel;

		public uint dwDiskNumber;

		public uint dwAlarmInPort;

		public uint dwAlarmOutPort;
	}

	public struct NET_DVR_ALARMHOST_SEARCH_LOG_PARAM
	{
		public ushort wMajorType;

		public ushort wMinorType;

		public NET_DVR_TIME struStartTime;

		public NET_DVR_TIME struEndTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_ALARMHOST_LOG_RET
	{
		public NET_DVR_TIME struLogTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sUserName;

		public NET_DVR_IPADDR struIPAddr;

		public ushort wMajorType;

		public ushort wMinorType;

		public ushort wParam;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public uint dwInfoLen;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11840)]
		public string sInfo;
	}

	public struct NET_DVR_ALARMOUTSTATUS_V30
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 96, ArraySubType = UnmanagedType.I1)]
		public byte[] Output;

		public void Init()
		{
			Output = new byte[96];
		}
	}

	public struct NET_DVR_ALARMOUTSTATUS
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
		public byte[] Output;
	}

	public struct NET_DVR_TRADEINFO
	{
		public ushort m_Year;

		public ushort m_Month;

		public ushort m_Day;

		public ushort m_Hour;

		public ushort m_Minute;

		public ushort m_Second;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24, ArraySubType = UnmanagedType.I1)]
		public byte[] DeviceName;

		public uint dwChannelNumer;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] CardNumber;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
		public string cTradeType;

		public uint dwCash;
	}

	public struct NET_DVR_FRAMETYPECODE
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.I1)]
		public byte[] code;
	}

	public struct NET_DVR_FRAMEFORMAT
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sATMIP;

		public uint dwATMType;

		public uint dwInputMode;

		public uint dwFrameSignBeginPos;

		public uint dwFrameSignLength;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.I1)]
		public byte[] byFrameSignContent;

		public uint dwCardLengthInfoBeginPos;

		public uint dwCardLengthInfoLength;

		public uint dwCardNumberInfoBeginPos;

		public uint dwCardNumberInfoLength;

		public uint dwBusinessTypeBeginPos;

		public uint dwBusinessTypeLength;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_FRAMETYPECODE[] frameTypeCode;
	}

	public struct NET_DVR_FILTER
	{
		public byte byEnable;

		public byte byMode;

		public byte byFrameBeginPos;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byFilterText;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_IDENTIFICAT
	{
		public byte byStartMode;

		public byte byEndMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public NET_DVR_FRAMETYPECODE struStartCode;

		public NET_DVR_FRAMETYPECODE struEndCode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;
	}

	public struct NET_DVR_PACKAGE_LOCATION
	{
		public byte byOffsetMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public uint dwOffsetPos;

		public NET_DVR_FRAMETYPECODE struTokenCode;

		public byte byMultiplierValue;

		public byte byEternOffset;

		public byte byCodeMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_PACKAGE_LENGTH
	{
		public byte byLengthMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public uint dwFixLength;

		public uint dwMaxLength;

		public uint dwMinLength;

		public byte byEndMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;

		public NET_DVR_FRAMETYPECODE struEndCode;

		public uint dwLengthPos;

		public uint dwLengthLen;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes3;
	}

	public struct NET_DVR_OSD_POSITION
	{
		public byte byPositionMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public uint dwPos_x;

		public uint dwPos_y;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_DATE_FORMAT
	{
		public byte byItem1;

		public byte byItem2;

		public byte byItem3;

		public byte byDateForm;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
		public string chSeprator;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
		public string chDisplaySeprator;

		public byte byDisplayForm;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 27, ArraySubType = UnmanagedType.I1)]
		public byte[] res;
	}

	public struct NET_DVRT_TIME_FORMAT
	{
		public byte byTimeForm;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 23, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public byte byHourMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
		public string chSeprator;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
		public string chDisplaySeprator;

		public byte byDisplayForm;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes3;

		public byte byDisplayHourMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 19, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes4;
	}

	public struct NET_DVR_OVERLAY_CHANNEL
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byChannel;

		public uint dwDelayTime;

		public byte byEnableDelayTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 11, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_ATM_PACKAGE_ACTION
	{
		public NET_DVR_PACKAGE_LOCATION struPackageLocation;

		public NET_DVR_OSD_POSITION struOsdPosition;

		public NET_DVR_FRAMETYPECODE struActionCode;

		public NET_DVR_FRAMETYPECODE struPreCode;

		public byte byActionCodeMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_ATM_PACKAGE_DATE
	{
		public NET_DVR_PACKAGE_LOCATION struPackageLocation;

		public NET_DVR_DATE_FORMAT struDateForm;

		public NET_DVR_OSD_POSITION struOsdPosition;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] res;
	}

	public struct NET_DVR_ATM_PACKAGE_TIME
	{
		public NET_DVR_PACKAGE_LOCATION location;

		public NET_DVRT_TIME_FORMAT struTimeForm;

		public NET_DVR_OSD_POSITION struOsdPosition;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_ATM_PACKAGE_OTHERS
	{
		public NET_DVR_PACKAGE_LOCATION struPackageLocation;

		public NET_DVR_PACKAGE_LENGTH struPackageLength;

		public NET_DVR_OSD_POSITION struOsdPosition;

		public NET_DVR_FRAMETYPECODE struPreCode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] res;
	}

	public struct NET_DVR_ATM_USER_DEFINE_PROTOCOL
	{
		public NET_DVR_IDENTIFICAT struIdentification;

		public NET_DVR_FILTER struFilter;

		public NET_DVR_ATM_PACKAGE_OTHERS struCardNoPara;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_ATM_PACKAGE_ACTION[] struTradeActionPara;

		public NET_DVR_ATM_PACKAGE_OTHERS struAmountPara;

		public NET_DVR_ATM_PACKAGE_OTHERS struSerialNoPara;

		public NET_DVR_OVERLAY_CHANNEL struOverlayChan;

		public NET_DVR_ATM_PACKAGE_DATE struRes1;

		public NET_DVR_ATM_PACKAGE_TIME struRes2;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 124, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes3;
	}

	public struct NET_DVR_ATM_FRAMEFORMAT_V30
	{
		public uint dwSize;

		public byte byEnable;

		public byte byInputMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 34, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_DVR_IPADDR struAtmIp;

		public ushort wAtmPort;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;

		public uint dwAtmType;

		public NET_DVR_ATM_USER_DEFINE_PROTOCOL struAtmUserDefineProtocol;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes3;
	}

	public struct NET_DVR_ATM_PROTO_TYPE
	{
		public uint dwAtmType;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string chDesc;
	}

	public struct NET_DVR_ATM_PROTO_LIST
	{
		public uint dwAtmProtoNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1025, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_ATM_PROTO_TYPE[] struAtmProtoType;
	}

	public struct NET_DVR_ATM_PROTOCOL
	{
		public uint dwSize;

		public NET_DVR_ATM_PROTO_LIST struNetListenList;

		public NET_DVR_ATM_PROTO_LIST struSerialListenList;

		public NET_DVR_ATM_PROTO_LIST struNetProtoList;

		public NET_DVR_ATM_PROTO_LIST struSerialProtoList;

		public NET_DVR_ATM_PROTO_TYPE struCustomProto;
	}

	public struct NET_DVR_DECODERINFO
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byEncoderIP;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byEncoderUser;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byEncoderPasswd;

		public byte bySendMode;

		public byte byEncoderChannel;

		public ushort wEncoderPort;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
		public byte[] reservedData;
	}

	public struct NET_DVR_DECODERSTATE
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byEncoderIP;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byEncoderUser;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byEncoderPasswd;

		public byte byEncoderChannel;

		public byte bySendMode;

		public ushort wEncoderPort;

		public uint dwConnectState;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
		public byte[] reservedData;
	}

	public struct NET_DVR_DECCHANINFO
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sDVRIP;

		public ushort wDVRPort;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sUserName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] sPassword;

		public byte byChannel;

		public byte byLinkMode;

		public byte byLinkType;
	}

	public struct NET_DVR_DECINFO
	{
		public byte byPoolChans;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_DECCHANINFO[] struchanConInfo;

		public byte byEnablePoll;

		public byte byPoolTime;
	}

	public struct NET_DVR_DECCFG
	{
		public uint dwSize;

		public uint dwDecChanNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_DECINFO[] struDecInfo;
	}

	public struct NET_DVR_PORTINFO
	{
		public uint dwEnableTransPort;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sDecoderIP;

		public ushort wDecoderPort;

		public ushort wDVRTransPort;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
		public string cReserve;
	}

	public struct NET_DVR_PORTCFG
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_PORTINFO[] struTransPortInfo;
	}

	public struct bytime
	{
		public uint dwChannel;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sUserName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] sPassword;

		public NET_DVR_TIME struStartTime;

		public NET_DVR_TIME struStopTime;
	}

	public struct NET_DVR_PLAYREMOTEFILE
	{
		[StructLayout(LayoutKind.Explicit)]
		public struct mode_size
		{
			[FieldOffset(0)]
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 100, ArraySubType = UnmanagedType.I1)]
			public byte[] byFile;

			[FieldOffset(0)]
			public bytime bytime;
		}

		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sDecoderIP;

		public ushort wDecoderPort;

		public ushort wLoadMode;
	}

	public struct NET_DVR_DECCHANSTATUS
	{
		[StructLayout(LayoutKind.Explicit)]
		public struct objectInfo
		{
			public struct userInfo
			{
				[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
				public byte[] sUserName;

				[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
				public byte[] sPassword;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 52)]
				public string cReserve;
			}

			public struct fileInfo
			{
				[MarshalAs(UnmanagedType.ByValArray, SizeConst = 100, ArraySubType = UnmanagedType.I1)]
				public byte[] fileName;
			}

			public struct timeInfo
			{
				public uint dwChannel;

				[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
				public byte[] sUserName;

				[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
				public byte[] sPassword;

				public NET_DVR_TIME struStartTime;

				public NET_DVR_TIME struStopTime;
			}
		}

		public uint dwWorkType;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sDVRIP;

		public ushort wDVRPort;

		public byte byChannel;

		public byte byLinkMode;

		public uint dwLinkType;
	}

	public struct NET_DVR_DECSTATUS
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_DECCHANSTATUS[] struTransPortInfo;
	}

	public struct NET_DVR_SHOWSTRINGINFO
	{
		public ushort wShowString;

		public ushort wStringSize;

		public ushort wShowStringTopLeftX;

		public ushort wShowStringTopLeftY;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 44)]
		public string sString;
	}

	public struct NET_DVR_SHOWSTRING_V30
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SHOWSTRINGINFO[] struStringInfo;
	}

	public struct NET_DVR_SHOWSTRING_EX
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SHOWSTRINGINFO[] struStringInfo;
	}

	public struct NET_DVR_SHOWSTRING
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SHOWSTRINGINFO[] struStringInfo;
	}

	public struct struReceiver
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.I1)]
		public byte[] sAddress;
	}

	public struct NET_DVR_EMAILCFG_V30
	{
		public struct struSender
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
			public byte[] sName;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.I1)]
			public byte[] sAddress;
		}

		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sAccount;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sPassword;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.I1)]
		public byte[] sSmtpServer;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.I1)]
		public byte[] sPop3Server;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.Struct)]
		public struReceiver[] struStringInfo;

		public byte byAttachment;

		public byte bySmtpServerVerify;

		public byte byMailInterval;

		public byte byEnableSSL;

		public ushort wSmtpPort;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 74, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_CRUISE_PARA
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byPresetNo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byCruiseSpeed;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.U2)]
		public ushort[] wDwellTime;

		public byte byEnableThisCruise;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 15, ArraySubType = UnmanagedType.I1)]
		public byte[] res;
	}

	public struct NET_DVR_TIMEPOINT
	{
		public uint dwMonth;

		public uint dwWeekNo;

		public uint dwWeekDate;

		public uint dwHour;

		public uint dwMin;
	}

	public struct NET_DVR_ZONEANDDST
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public uint dwEnableDST;

		public byte byDSTBias;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;

		public NET_DVR_TIMEPOINT struBeginPoint;

		public NET_DVR_TIMEPOINT struEndPoint;
	}

	public struct NET_DVR_JPEGPARA
	{
		public ushort wPicSize;

		public ushort wPicQuality;
	}

	public struct NET_DVR_AUXOUTCFG
	{
		public uint dwSize;

		public uint dwAlarmOutChan;

		public uint dwAlarmChanSwitchTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U4)]
		public uint[] dwAuxSwitchTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byAuxOrder;
	}

	public struct NET_DVR_NTPPARA
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] sNTPServer;

		public ushort wInterval;

		public byte byEnableNTP;

		public byte cTimeDifferenceH;

		public byte cTimeDifferenceM;

		public byte res1;

		public ushort wNtpPort;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] res2;
	}

	public struct NET_DVR_DDNSPARA
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sUsername;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] sPassword;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] sDomainName;

		public byte byEnableDDNS;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 15, ArraySubType = UnmanagedType.I1)]
		public byte[] res;
	}

	public struct NET_DVR_DDNSPARA_EX
	{
		public byte byHostIndex;

		public byte byEnableDDNS;

		public ushort wDDNSPort;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sUsername;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] sPassword;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] sDomainName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] sServerName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct struDDNS
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sUsername;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] sPassword;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] sDomainName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] sServerName;

		public ushort wDDNSPort;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_DDNSPARA_V30
	{
		public byte byEnableDDNS;

		public byte byHostIndex;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.Struct)]
		public struDDNS[] struDDNS;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_EMAILPARA
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] sUsername;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] sPassword;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] sSmtpServer;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] sPop3Server;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] sMailAddr;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] sEventMailAddr1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] sEventMailAddr2;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] res;
	}

	public struct NET_DVR_NETAPPCFG
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sDNSIp;

		public NET_DVR_NTPPARA struNtpClientParam;

		public NET_DVR_DDNSPARA struDDNSClientParam;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 464, ArraySubType = UnmanagedType.I1)]
		public byte[] res;
	}

	public struct NET_DVR_SINGLE_NFS
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sNfsHostIPAddr;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
		public byte[] sNfsDirectory;

		public void Init()
		{
			sNfsDirectory = new byte[128];
		}
	}

	public struct NET_DVR_NFSCFG
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SINGLE_NFS[] struNfsDiskParam;

		public void Init()
		{
			struNfsDiskParam = new NET_DVR_SINGLE_NFS[8];
			for (int i = 0; i < 8; i++)
			{
				struNfsDiskParam[i].Init();
			}
		}
	}

	public struct NET_DVR_ISCSI_CFG
	{
		public uint dwSize;

		public ushort wVrmPort;

		public byte byEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 69, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public NET_DVR_IPADDR struVrmAddr;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string chNvtIndexCode;
	}

	public struct NET_DVR_CRUISE_POINT
	{
		public byte PresetNum;

		public byte Dwell;

		public byte Speed;

		public byte Reserve;

		public void Init()
		{
			PresetNum = 0;
			Dwell = 0;
			Speed = 0;
			Reserve = 0;
		}
	}

	public struct NET_DVR_CRUISE_RET
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_CRUISE_POINT[] struCruisePoint;

		public void Init()
		{
			struCruisePoint = new NET_DVR_CRUISE_POINT[32];
			for (int i = 0; i < 32; i++)
			{
				struCruisePoint[i].Init();
			}
		}
	}

	public struct NET_DVR_NETCFG_OTHER
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sFirstDNSIP;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sSecondDNSIP;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string sRes;
	}

	public struct NET_DVR_MATRIX_DECINFO
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sDVRIP;

		public ushort wDVRPort;

		public byte byChannel;

		public byte byTransProtocol;

		public byte byTransMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sUserName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] sPassword;
	}

	public struct NET_DVR_MATRIX_DYNAMIC_DEC
	{
		public uint dwSize;

		public NET_DVR_MATRIX_DECINFO struDecChanInfo;
	}

	public struct NET_DVR_MATRIX_DEC_CHAN_STATUS
	{
		public uint dwSize;

		public uint dwIsLinked;

		public uint dwStreamCpRate;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string cRes;
	}

	public struct NET_DVR_MATRIX_DEC_CHAN_INFO
	{
		public uint dwSize;

		public NET_DVR_MATRIX_DECINFO struDecChanInfo;

		public uint dwDecState;

		public NET_DVR_TIME StartTime;

		public NET_DVR_TIME StopTime;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string sFileName;
	}

	public struct NET_DVR_MATRIX_DECCHANINFO
	{
		public uint dwEnable;

		public NET_DVR_MATRIX_DECINFO struDecChanInfo;
	}

	public struct NET_DVR_MATRIX_LOOP_DECINFO
	{
		public uint dwSize;

		public uint dwPoolTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_MATRIX_DECCHANINFO[] struchanConInfo;
	}

	public struct TTY_CONFIG
	{
		public byte baudrate;

		public byte databits;

		public byte stopbits;

		public byte parity;

		public byte flowcontrol;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] res;
	}

	public struct NET_DVR_MATRIX_TRAN_CHAN_INFO
	{
		public byte byTranChanEnable;

		public byte byLocalSerialDevice;

		public byte byRemoteSerialDevice;

		public byte res1;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sRemoteDevIP;

		public ushort wRemoteDevPort;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] res2;

		public TTY_CONFIG RemoteSerialDevCfg;
	}

	public struct NET_DVR_MATRIX_TRAN_CHAN_CONFIG
	{
		public uint dwSize;

		public byte by232IsDualChan;

		public byte by485IsDualChan;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] res;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_MATRIX_TRAN_CHAN_INFO[] struTranInfo;
	}

	public struct NET_DVR_MATRIX_DEC_REMOTE_PLAY
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sDVRIP;

		public ushort wDVRPort;

		public byte byChannel;

		public byte byReserve;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sUserName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] sPassword;

		public uint dwPlayMode;

		public NET_DVR_TIME StartTime;

		public NET_DVR_TIME StopTime;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string sFileName;
	}

	public struct NET_DVR_MATRIX_DEC_REMOTE_PLAY_CONTROL
	{
		public uint dwSize;

		public uint dwPlayCmd;

		public uint dwCmdParam;
	}

	public struct NET_DVR_MATRIX_DEC_REMOTE_PLAY_STATUS
	{
		public uint dwSize;

		public uint dwCurMediaFileLen;

		public uint dwCurMediaFilePosition;

		public uint dwCurMediaFileDuration;

		public uint dwCurPlayTime;

		public uint dwCurMediaFIleFrames;

		public uint dwCurDataType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 72, ArraySubType = UnmanagedType.I1)]
		public byte[] res;
	}

	public struct NET_DVR_MATRIX_PASSIVEMODE
	{
		public ushort wTransProtol;

		public ushort wPassivePort;

		public NET_DVR_IPADDR struMcastIP;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] res;
	}

	public struct NET_DVR_MATRIX_TRAN_CHAN_INFO_V30
	{
		public byte byTranChanEnable;

		public byte byLocalSerialDevice;

		public byte byRemoteSerialDevice;

		public byte byRes1;

		public NET_DVR_IPADDR struRemoteDevIP;

		public ushort wRemoteDevPort;

		public byte byIsEstablished;

		public byte byRes2;

		public TTY_CONFIG RemoteSerialDevCfg;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byUsername;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byPassword;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes3;
	}

	public struct NET_DVR_MATRIX_TRAN_CHAN_CONFIG_V30
	{
		public uint dwSize;

		public byte by232IsDualChan;

		public byte by485IsDualChan;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] vyRes;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_MATRIX_TRAN_CHAN_INFO[] struTranInfo;
	}

	public struct NET_DVR_MATRIX_CHAN_INFO_V30
	{
		public uint dwEnable;

		public NET_DVR_STREAM_MEDIA_SERVER_CFG streamMediaServerCfg;

		public NET_DVR_DEV_CHAN_INFO struDevChanInfo;
	}

	public struct NET_DVR_MATRIX_LOOP_DECINFO_V30
	{
		public uint dwSize;

		public uint dwPoolTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_MATRIX_CHAN_INFO_V30[] struchanConInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_MATRIX_DEC_CHAN_INFO_V30
	{
		public uint dwSize;

		public NET_DVR_STREAM_MEDIA_SERVER_CFG streamMediaServerCfg;

		public NET_DVR_DEV_CHAN_INFO struDevChanInfo;

		public uint dwDecState;

		public NET_DVR_TIME StartTime;

		public NET_DVR_TIME StopTime;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string sFileName;

		public uint dwGetStreamMode;

		public NET_DVR_MATRIX_PASSIVEMODE struPassiveMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_MATRIX_ABILITY
	{
		public uint dwSize;

		public byte byDecNums;

		public byte byStartChan;

		public byte byVGANums;

		public byte byBNCNums;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 96, ArraySubType = UnmanagedType.I1)]
		public byte[] byVGAWindowMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
		public byte[] byBNCWindowMode;

		public byte byDspNums;

		public byte byHDMINums;

		public byte byDVINums;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 13, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] bySupportResolution;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byHDMIWindowMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byDVIWindowMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_DISP_LOGOCFG
	{
		public uint dwCorordinateX;

		public uint dwCorordinateY;

		public ushort wPicWidth;

		public ushort wPicHeight;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public byte byFlash;

		public byte byTranslucent;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;

		public uint dwLogoSize;
	}

	public struct NET_DVR_MATRIX_CHAN_STATUS
	{
		public byte byDecodeStatus;

		public byte byStreamType;

		public byte byPacketType;

		public byte byRecvBufUsage;

		public byte byDecBufUsage;

		public byte byFpsDecV;

		public byte byFpsDecA;

		public byte byCpuLoad;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public uint dwDecodedV;

		public uint dwDecodedA;

		public ushort wImgW;

		public ushort wImgH;

		public byte byVideoFormat;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;

		public uint dwDecChan;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes3;
	}

	public enum VGA_MODE
	{
		VGA_NOT_AVALIABLE,
		VGA_THS8200_MODE_SVGA_60HZ,
		VGA_THS8200_MODE_SVGA_75HZ,
		VGA_THS8200_MODE_XGA_60HZ,
		VGA_THS8200_MODE_XGA_75HZ,
		VGA_THS8200_MODE_SXGA_60HZ,
		VGA_THS8200_MODE_720P_60HZ,
		VGA_THS8200_MODE_1080I_60HZ,
		VGA_THS8200_MODE_1080P_30HZ,
		VGA_THS8200_MODE_UXGA_30HZ,
		HDMI_SII9134_MODE_XGA_60HZ,
		HDMI_SII9134_MODE_SXGA_60HZ,
		HDMI_SII9134_MODE_SXGA2_60HZ,
		HDMI_SII9134_MODE_720P_60HZ,
		HDMI_SII9134_MODE_720P_50HZ,
		HDMI_SII9134_MODE_1080I_60HZ,
		HDMI_SII9134_MODE_1080I_50HZ,
		HDMI_SII9134_MODE_1080P_25HZ,
		HDMI_SII9134_MODE_1080P_30HZ,
		HDMI_SII9134_MODE_1080P_50HZ,
		HDMI_SII9134_MODE_1080P_60HZ,
		HDMI_SII9134_MODE_UXGA_60HZ,
		DVI_SII9134_MODE_XGA_60HZ,
		DVI_SII9134_MODE_SXGA_60HZ,
		DVI_SII9134_MODE_SXGA2_60HZ,
		DVI_SII9134_MODE_720P_60HZ,
		DVI_SII9134_MODE_720P_50HZ,
		DVI_SII9134_MODE_1080I_60HZ,
		DVI_SII9134_MODE_1080I_50HZ,
		DVI_SII9134_MODE_1080P_25HZ,
		DVI_SII9134_MODE_1080P_30HZ,
		DVI_SII9134_MODE_1080P_50HZ,
		DVI_SII9134_MODE_1080P_60HZ,
		DVI_SII9134_MODE_UXGA_60HZ,
		VGA_DECSVR_MODE_SXGA2_60HZ,
		HDMI_DECSVR_MODE_1080P_24HZ,
		DVI_DECSVR_MODE_1080P_24HZ,
		YPbPr_DECSVR_MODE_720P_60HZ,
		YPbPr_DECSVR_MODE_1080I_60HZ
	}

	public enum VIDEO_STANDARD
	{
		VS_NON,
		VS_NTSC,
		VS_PAL
	}

	public struct UNION_VIDEOPLATFORM
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byJoinDecoderId;
	}

	public struct UNION_NOTVIDEOPLATFORM
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_VGA_DISP_CHAN_CFG
	{
		[StructLayout(LayoutKind.Explicit)]
		public struct struDiff
		{
			[FieldOffset(0)]
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;

			[FieldOffset(0)]
			public UNION_VIDEOPLATFORM struVideoPlatform;

			[FieldOffset(0)]
			public UNION_NOTVIDEOPLATFORM struNotVideoPlatform;
		}

		public uint dwSize;

		public byte byAudio;

		public byte byAudioWindowIdx;

		public byte byVgaResolution;

		public byte byVedioFormat;

		public uint dwWindowMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byJoinDecChan;

		public byte byEnlargeStatus;

		public byte byEnlargeSubWindowIndex;

		public byte byUnionType;

		public byte byScale;
	}

	public struct NET_DVR_DISP_CHAN_STATUS
	{
		public byte byDispStatus;

		public byte byBVGA;

		public byte byVideoFormat;

		public byte byWindowMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byJoinDecChan;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byFpsDisp;

		public byte byScreenMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 31, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_DECODER_WORK_STATUS
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_MATRIX_CHAN_STATUS[] struDecChanStatus;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_DISP_CHAN_STATUS[] struDispChanStatus;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byAlarmInStatus;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byAalarmOutStatus;

		public byte byAudioInChanStatus;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 127, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_PASSIVEDECODE_CONTROL
	{
		public uint dwSize;

		public uint dwPlayCmd;

		public uint dwCmdParam;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_MATRIX_DECCHAN_CONTROL
	{
		public uint dwSize;

		public byte byDecChanScaleStatus;

		public byte byDecodeDelay;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 66, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_SUBSYSTEMINFO
	{
		public byte bySubSystemType;

		public byte byChan;

		public byte byLoginType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_DVR_IPADDR struSubSystemIP;

		public ushort wSubSystemPort;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;

		public NET_DVR_IPADDR struSubSystemIPMask;

		public NET_DVR_IPADDR struGatewayIpAddr;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sUserName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] sPassword;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string sDomainName;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string sDnsAddress;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.I1)]
		public byte[] sSerialNumber;
	}

	public struct NET_DVR_ALLSUBSYSTEMINFO
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 80, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SUBSYSTEMINFO[] struSubSystemInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_LOOPPLAN_SUBCFG
	{
		public uint dwSize;

		public uint dwPoolTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_MATRIX_CHAN_INFO_V30[] struChanConInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_ALARMMODECFG
	{
		public uint dwSize;

		public byte byAlarmMode;

		public ushort wLoopTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_CODESPLITTERINFO
	{
		public uint dwSize;

		public NET_DVR_IPADDR struIP;

		public ushort wPort;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sUserName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] sPassword;

		public byte byChan;

		public byte by485Port;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 14, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_ASSOCIATECFG
	{
		public byte byAssociateType;

		public ushort wAlarmDelay;

		public byte byAlarmNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_DYNAMICDECODE
	{
		public uint dwSize;

		public NET_DVR_ASSOCIATECFG struAssociateCfg;

		public NET_DVR_PU_STREAM_CFG struPuStreamCfg;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_DECODESCHED
	{
		public NET_DVR_SCHEDTIME struSchedTime;

		public byte byDecodeType;

		public byte byLoopGroup;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public NET_DVR_PU_STREAM_CFG struDynamicDec;
	}

	public struct NET_DVR_PLANDECODE
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 28, ArraySubType = UnmanagedType.I1)]
		public NET_DVR_DECODESCHED[] struDecodeSched;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] byres;
	}

	public struct NET_DVR_VIDEOPLATFORM_ABILITY
	{
		public uint dwSize;

		public byte byCodeSubSystemNums;

		public byte byDecodeSubSystemNums;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.I1)]
		public byte[] byWindowMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_SUBSYSTEM_ABILITY
	{
		public byte bySubSystemType;

		public byte byChanNum;

		public byte byStartChan;

		public byte bySlotNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public struDecoderSystemAbility _struAbility;
	}

	public struct struDecoderSystemAbility
	{
		public byte byVGANums;

		public byte byBNCNums;

		public byte byHDMINums;

		public byte byDVINums;

		public byte byLayerNums;

		public byte bySpartan;

		public byte byDecType;

		public byte byOutputSwitch;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 39, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public byte byDecoderType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 152, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct struAbility
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 200, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_VIDEOPLATFORM_ABILITY_V40
	{
		public uint dwSize;

		public byte byCodeSubSystemNums;

		public byte byDecodeSubSystemNums;

		public byte bySupportNat;

		public byte byInputSubSystemNums;

		public byte byOutputSubSystemNums;

		public byte byCodeSpitterSubSystemNums;

		public byte byAlarmHostSubSystemNums;

		public byte bySupportBigScreenNum;

		public byte byVCASubSystemNums;

		public byte byV6SubSystemNums;

		public byte byV6DecoderSubSystemNums;

		public byte bySupportBigScreenX;

		public byte bySupportBigScreenY;

		public byte bySupportSceneNums;

		public byte byVcaSupportChanMode;

		public byte bySupportScreenNums;

		public byte bySupportLayerNums;

		public byte byNotSupportPreview;

		public byte byNotSupportStorage;

		public byte byUploadLogoMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 120, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SUBSYSTEM_ABILITY[] struSubSystemAbility;

		public byte by485Nums;

		public byte by232Nums;

		public byte bySerieStartChan;

		public byte byScreenMode;

		public byte byDevVersion;

		public byte bySupportBaseMapNums;

		public ushort wBaseLengthX;

		public ushort wBaseLengthY;

		public byte bySupportPictureTrans;

		public byte bySupportPreAllocDec;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 628, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_SINGLESCREENCFG
	{
		public byte byScreenSeq;

		public byte bySubSystemNum;

		public byte byDispNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_BIGSCREENCFG
	{
		public uint dwSize;

		public byte byEnable;

		public byte byModeX;

		public byte byModeY;

		public byte byMainDecodeSystem;

		public byte byMainDecoderDispChan;

		public byte byVideoStandard;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public uint dwResolution;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 100, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SINGLESCREENCFG[] struFollowSingleScreen;

		public ushort wBigScreenX;

		public ushort wBigScreenY;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;

		public void Init()
		{
			byRes1 = new byte[2];
			struFollowSingleScreen = new NET_DVR_SINGLESCREENCFG[100];
			byRes2 = new byte[12];
		}
	}

	public struct NET_DVR_EMAILCFG
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string sUserName;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string sPassWord;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string sFromName;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
		public string sFromAddr;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string sToName1;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string sToName2;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
		public string sToAddr1;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
		public string sToAddr2;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string sEmailServer;

		public byte byServerType;

		public byte byUseAuthen;

		public byte byAttachment;

		public byte byMailinterval;
	}

	public struct NET_DVR_COMPRESSIONCFG_NEW
	{
		public uint dwSize;

		public NET_DVR_COMPRESSION_INFO_EX struLowCompression;

		public NET_DVR_COMPRESSION_INFO_EX struEventCompression;
	}

	public struct NET_DVR_PRESET_NAME
	{
		public uint dwSize;

		public ushort wPresetNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byName;

		public ushort wPanPos;

		public ushort wTiltPos;

		public ushort wZoomPos;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 58, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_PTZPOS
	{
		public ushort wAction;

		public ushort wPanPos;

		public ushort wTiltPos;

		public ushort wZoomPos;
	}

	public struct NET_DVR_PTZSCOPE
	{
		public ushort wPanPosMin;

		public ushort wPanPosMax;

		public ushort wTiltPosMin;

		public ushort wTiltPosMax;

		public ushort wZoomPosMin;

		public ushort wZoomPosMax;
	}

	public struct NET_DVR_RTSPCFG
	{
		public uint dwSize;

		public ushort wPort;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 54, ArraySubType = UnmanagedType.I1)]
		public byte[] byReserve;
	}

	public struct NET_DVR_DEVICEINFO
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.I1)]
		public byte[] sSerialNumber;

		public byte byAlarmInPortNum;

		public byte byAlarmOutPortNum;

		public byte byDiskNum;

		public byte byDVRType;

		public byte byChanNum;

		public byte byStartChan;
	}

	public struct NET_DVR_DEVICEINFO_V30
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.I1)]
		public byte[] sSerialNumber;

		public byte byAlarmInPortNum;

		public byte byAlarmOutPortNum;

		public byte byDiskNum;

		public byte byDVRType;

		public byte byChanNum;

		public byte byStartChan;

		public byte byAudioChanNum;

		public byte byIPChanNum;

		public byte byZeroChanNum;

		public byte byMainProto;

		public byte bySubProto;

		public byte bySupport;

		public byte bySupport1;

		public byte bySupport2;

		public ushort wDevType;

		public byte bySupport3;

		public byte byMultiStreamProto;

		public byte byStartDChan;

		public byte byStartDTalkChan;

		public byte byHighDChanNum;

		public byte bySupport4;

		public byte byLanguageType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public enum SDK_NETWORK_ENVIRONMENT
	{
		LOCAL_AREA_NETWORK,
		WIDE_AREA_NETWORK
	}

	public enum DISPLAY_MODE
	{
		NORMALMODE,
		OVERLAYMODE
	}

	public enum SEND_MODE
	{
		PTOPTCPMODE,
		PTOPUDPMODE,
		MULTIMODE,
		RTPMODE,
		RESERVEDMODE
	}

	public enum CAPTURE_MODE
	{
		BMP_MODE,
		JPEG_MODE
	}

	public enum REALSOUND_MODE
	{
		MONOPOLIZE_MODE = 1,
		SHARE_MODE
	}

	public struct NET_DVR_CLIENTINFO
	{
		public int lChannel;

		public int lLinkMode;

		public IntPtr hPlayWnd;

		public string sMultiCastIP;
	}

	public struct NET_DVR_SDKSTATE
	{
		public uint dwTotalLoginNum;

		public uint dwTotalRealPlayNum;

		public uint dwTotalPlayBackNum;

		public uint dwTotalAlarmChanNum;

		public uint dwTotalFormatNum;

		public uint dwTotalFileSearchNum;

		public uint dwTotalLogSearchNum;

		public uint dwTotalSerialNum;

		public uint dwTotalUpgradeNum;

		public uint dwTotalVoiceComNum;

		public uint dwTotalBroadCastNum;

		public uint dwTotalListenNum;

		public uint dwEmailTestNum;

		public uint dwBackupNum;

		public uint dwTotalInquestUploadNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.U4)]
		public uint[] dwRes;
	}

	public struct NET_DVR_SDKABL
	{
		public uint dwMaxLoginNum;

		public uint dwMaxRealPlayNum;

		public uint dwMaxPlayBackNum;

		public uint dwMaxAlarmChanNum;

		public uint dwMaxFormatNum;

		public uint dwMaxFileSearchNum;

		public uint dwMaxLogSearchNum;

		public uint dwMaxSerialNum;

		public uint dwMaxUpgradeNum;

		public uint dwMaxVoiceComNum;

		public uint dwMaxBroadCastNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.U4)]
		public uint[] dwRes;
	}

	public struct NET_DVR_ALARMER
	{
		public byte byUserIDValid;

		public byte bySerialValid;

		public byte byVersionValid;

		public byte byDeviceNameValid;

		public byte byMacAddrValid;

		public byte byLinkPortValid;

		public byte byDeviceIPValid;

		public byte bySocketIPValid;

		public int lUserID;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.I1)]
		public byte[] sSerialNumber;

		public uint dwDeviceVersion;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string sDeviceName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
		public byte[] byMacAddr;

		public ushort wLinkPort;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string sDeviceIP;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string sSocketIP;

		public byte byIpProtocol;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 11, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_DISPLAY_PARA
	{
		public int bToScreen;

		public int bToVideoOut;

		public int nLeft;

		public int nTop;

		public int nWidth;

		public int nHeight;

		public int nReserved;
	}

	public struct NET_DVR_CARDINFO
	{
		public int lChannel;

		public int lLinkMode;

		[MarshalAs(UnmanagedType.LPStr)]
		public string sMultiCastIP;

		public NET_DVR_DISPLAY_PARA struDisplayPara;
	}

	public struct NET_DVR_FIND_DATA
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
		public string sFileName;

		public NET_DVR_TIME struStartTime;

		public NET_DVR_TIME struStopTime;

		public uint dwFileSize;
	}

	public struct NET_DVR_FINDDATA_V30
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
		public string sFileName;

		public NET_DVR_TIME struStartTime;

		public NET_DVR_TIME struStopTime;

		public uint dwFileSize;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string sCardNum;

		public byte byLocked;

		public byte byFileType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_FINDDATA_V40
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
		public string sFileName;

		public NET_DVR_TIME struStartTime;

		public NET_DVR_TIME struStopTime;

		public uint dwFileSize;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string sCardNum;

		public byte byLocked;

		public byte byFileType;

		public byte byQuickSearch;

		public byte byRes;

		public uint dwFileIndex;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;
	}

	public struct NET_DVR_FINDDATA_CARD
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
		public string sFileName;

		public NET_DVR_TIME struStartTime;

		public NET_DVR_TIME struStopTime;

		public uint dwFileSize;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string sCardNum;
	}

	public struct NET_DVR_FILECOND
	{
		public int lChannel;

		public uint dwFileType;

		public uint dwIsLocked;

		public uint dwUseCardNo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sCardNumber;

		public NET_DVR_TIME struStartTime;

		public NET_DVR_TIME struStopTime;
	}

	public struct NET_DVR_POINT_FRAME
	{
		public int xTop;

		public int yTop;

		public int xBottom;

		public int yBottom;

		public int bCounter;
	}

	public struct NET_DVR_COMPRESSION_AUDIO
	{
		public byte byAudioEncType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7, ArraySubType = UnmanagedType.I1)]
		public byte[] byres;
	}

	public struct NET_DVR_AP_INFO
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string sSsid;

		public uint dwMode;

		public uint dwSecurity;

		public uint dwChannel;

		public uint dwSignalStrength;

		public uint dwSpeed;
	}

	public struct NET_DVR_AP_INFO_LIST
	{
		public uint dwSize;

		public uint dwCount;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_AP_INFO[] struApInfo;
	}

	public struct NET_DVR_WIFIETHERNET
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sIpAddress;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sIpMask;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
		public byte[] byMACAddr;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] bRes;

		public uint dwEnableDhcp;

		public uint dwAutoDns;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sFirstDns;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sSecondDns;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sGatewayIpAddr;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] bRes2;
	}

	public struct UNION_EAP_TTLS
	{
		public byte byEapolVersion;

		public byte byAuthType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byAnonyIdentity;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byUserName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byPassword;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 44, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct UNION_EAP_PEAP
	{
		public byte byEapolVersion;

		public byte byAuthType;

		public byte byPeapVersion;

		public byte byPeapLabel;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byAnonyIdentity;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byUserName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byPassword;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 44, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct UNION_EAP_TLS
	{
		public byte byEapolVersion;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byIdentity;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byPrivateKeyPswd;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 76, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct WIFI_AUTH_PARAM
	{
		[FieldOffset(0)]
		public UNION_EAP_TTLS EAP_TTLS;

		[FieldOffset(0)]
		public UNION_EAP_PEAP EAP_PEAP;

		[FieldOffset(0)]
		public UNION_EAP_TLS EAP_TLS;
	}

	public struct UNION_WEP
	{
		public uint dwAuthentication;

		public uint dwKeyLength;

		public uint dwKeyType;

		public uint dwActive;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 132)]
		public string sKeyInfo;
	}

	public struct UNION_WPA_PSK
	{
		public uint dwKeyLength;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 63)]
		public string sKeyInfo;

		public byte byEncryptType;
	}

	public struct UNION_WPA_WPA2
	{
		public byte byEncryptType;

		public byte byAuthType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public WIFI_AUTH_PARAM auth_param;
	}

	public struct NET_DVR_WIFI_CFG_EX
	{
		[StructLayout(LayoutKind.Explicit)]
		public struct key
		{
			[FieldOffset(0)]
			public UNION_WEP wep;

			[FieldOffset(0)]
			public UNION_WPA_PSK wpa_psk;

			[FieldOffset(0)]
			public UNION_WPA_WPA2 wpa_wpa2;
		}

		public NET_DVR_WIFIETHERNET struEtherNet;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string sEssid;

		public uint dwMode;

		public uint dwSecurity;
	}

	public struct NET_DVR_WIFI_CFG
	{
		public uint dwSize;

		public NET_DVR_WIFI_CFG_EX struWifiCfg;
	}

	public struct NET_DVR_WIFI_CONNECT_STATUS
	{
		public uint dwSize;

		public byte byCurStatus;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public uint dwErrorCode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 244, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_WIFI_WORKMODE
	{
		public uint dwSize;

		public uint dwNetworkInterfaceMode;
	}

	public struct NET_VCA_CTRLINFO
	{
		public byte byVCAEnable;

		public byte byVCAType;

		public byte byStreamWithVCA;

		public byte byMode;

		public byte byControlType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_CTRLCFG
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
		public NET_VCA_CTRLINFO[] struCtrlInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_DEV_ABILITY
	{
		public uint dwSize;

		public byte byVCAChanNum;

		public byte byPlateChanNum;

		public byte byBBaseChanNum;

		public byte byBAdvanceChanNum;

		public byte byBFullChanNum;

		public byte byATMChanNum;

		public byte byPDCChanNum;

		public byte byITSChanNum;

		public byte byBPrisonChanNum;

		public byte byFSnapChanNum;

		public byte byFSnapRecogChanNum;

		public byte byFRetrievalChanNum;

		public byte bySupport;

		public byte byFRecogChanNum;

		public byte byBPPerimeterChanNum;

		public byte byTPSChanNum;

		public byte byTFSChanNum;

		public byte byFSnapBFullChanNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 22, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public enum VCA_ABILITY_TYPE : uint
	{
		TRAVERSE_PLANE_ABILITY = 1u,
		ENTER_AREA_ABILITY = 2u,
		EXIT_AREA_ABILITY = 4u,
		INTRUSION_ABILITY = 8u,
		LOITER_ABILITY = 0x10u,
		LEFT_TAKE_ABILITY = 0x20u,
		PARKING_ABILITY = 0x40u,
		RUN_ABILITY = 0x80u,
		HIGH_DENSITY_ABILITY = 0x100u,
		LF_TRACK_ABILITY = 0x200u,
		VIOLENT_MOTION_ABILITY = 0x400u,
		REACH_HIGHT_ABILITY = 0x800u,
		GET_UP_ABILITY = 0x1000u,
		LEFT_ABILITY = 0x2000u,
		TAKE_ABILITY = 0x4000u,
		LEAVE_POSITION = 0x8000u,
		TRAIL_ABILITY = 0x10000u,
		KEY_PERSON_GET_UP_ABILITY = 0x20000u,
		FALL_DOWN_ABILITY = 0x80000u,
		AUDIO_ABNORMAL_ABILITY = 0x100000u,
		ADV_REACH_HEIGHT_ABILITY = 0x200000u,
		TOILET_TARRY_ABILITY = 0x400000u,
		YARD_TARRY_ABILITY = 0x800000u,
		ADV_TRAVERSE_PLANE_ABILITY = 0x1000000u,
		HUMAN_ENTER_ABILITY = 0x10000000u,
		OVER_TIME_ABILITY = 0x20000000u,
		STICK_UP_ABILITY = 0x40000000u,
		INSTALL_SCANNER_ABILITY = 0x80000000u
	}

	public enum VCA_CHAN_ABILITY_TYPE
	{
		VCA_BEHAVIOR_BASE = 1,
		VCA_BEHAVIOR_ADVANCE,
		VCA_BEHAVIOR_FULL,
		VCA_PLATE,
		VCA_ATM,
		VCA_PDC,
		VCA_ITS,
		VCA_BEHAVIOR_PRISON,
		VCA_FACE_SNAP,
		VCA_FACE_SNAPRECOG,
		VCA_FACE_RETRIEVAL,
		VCA_FACE_RECOG,
		VCA_BEHAVIOR_PRISON_PERIMETER,
		VCA_TPS,
		VCA_TFS,
		VCA_BEHAVIOR_FACESNAP
	}

	public enum VCA_CHAN_MODE_TYPE
	{
		VCA_ATM_PANEL,
		VCA_ATM_SURROUND,
		VCA_ATM_FACE
	}

	public enum TFS_CHAN_MODE_TYPE
	{
		TFS_CITYROAD,
		TFS_FREEWAY
	}

	public enum BEHAVIOR_SCENE_MODE_TYPE
	{
		BEHAVIOR_SCENE_DEFAULT,
		BEHAVIOR_SCENE_WALL,
		BEHAVIOR_SCENE_INDOOR
	}

	public struct NET_VCA_CHAN_IN_PARAM
	{
		public byte byVCAType;

		public byte byMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_BEHAVIOR_ABILITY
	{
		public uint dwSize;

		public uint dwAbilityType;

		public byte byMaxRuleNum;

		public byte byMaxTargetNum;

		public byte bySupport;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_ITS_ABILITY
	{
		public uint dwSize;

		public uint dwAbilityType;

		public byte byMaxRuleNum;

		public byte byMaxTargetNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_POINT
	{
		public float fX;

		public float fY;
	}

	public struct NET_VCA_RECT
	{
		public float fX;

		public float fY;

		public float fWidth;

		public float fHeight;
	}

	public enum VCA_EVENT_TYPE : uint
	{
		VCA_TRAVERSE_PLANE = 1u,
		VCA_ENTER_AREA = 2u,
		VCA_EXIT_AREA = 4u,
		VCA_INTRUSION = 8u,
		VCA_LOITER = 0x10u,
		VCA_LEFT_TAKE = 0x20u,
		VCA_PARKING = 0x40u,
		VCA_RUN = 0x80u,
		VCA_HIGH_DENSITY = 0x100u,
		VCA_VIOLENT_MOTION = 0x200u,
		VCA_REACH_HIGHT = 0x400u,
		VCA_GET_UP = 0x800u,
		VCA_LEFT = 0x1000u,
		VCA_TAKE = 0x2000u,
		VCA_LEAVE_POSITION = 0x4000u,
		VCA_TRAIL = 0x8000u,
		VCA_KEY_PERSON_GET_UP = 0x10000u,
		VCA_FALL_DOWN = 0x80000u,
		VCA_AUDIO_ABNORMAL = 0x100000u,
		VCA_ADV_REACH_HEIGHT = 0x200000u,
		VCA_TOILET_TARRY = 0x400000u,
		VCA_YARD_TARRY = 0x800000u,
		VCA_ADV_TRAVERSE_PLANE = 0x1000000u,
		VCA_HUMAN_ENTER = 0x10000000u,
		VCA_OVER_TIME = 0x20000000u,
		VCA_STICK_UP = 0x40000000u,
		VCA_INSTALL_SCANNER = 0x80000000u
	}

	public enum VCA_RULE_EVENT_TYPE_EX
	{
		ENUM_VCA_EVENT_TRAVERSE_PLANE = 1,
		ENUM_VCA_EVENT_ENTER_AREA = 2,
		ENUM_VCA_EVENT_EXIT_AREA = 3,
		ENUM_VCA_EVENT_INTRUSION = 4,
		ENUM_VCA_EVENT_LOITER = 5,
		ENUM_VCA_EVENT_LEFT_TAKE = 6,
		ENUM_VCA_EVENT_PARKING = 7,
		ENUM_VCA_EVENT_RUN = 8,
		ENUM_VCA_EVENT_HIGH_DENSITY = 9,
		ENUM_VCA_EVENT_VIOLENT_MOTION = 10,
		ENUM_VCA_EVENT_REACH_HIGHT = 11,
		ENUM_VCA_EVENT_GET_UP = 12,
		ENUM_VCA_EVENT_LEFT = 13,
		ENUM_VCA_EVENT_TAKE = 14,
		ENUM_VCA_EVENT_LEAVE_POSITION = 15,
		ENUM_VCA_EVENT_TRAIL = 16,
		ENUM_VCA_EVENT_KEY_PERSON_GET_UP = 17,
		ENUM_VCA_EVENT_FALL_DOWN = 20,
		ENUM_VCA_EVENT_AUDIO_ABNORMAL = 21,
		ENUM_VCA_EVENT_ADV_REACH_HEIGHT = 22,
		ENUM_VCA_EVENT_TOILET_TARRY = 23,
		ENUM_VCA_EVENT_YARD_TARRY = 24,
		ENUM_VCA_EVENT_ADV_TRAVERSE_PLANE = 25,
		ENUM_VCA_EVENT_HUMAN_ENTER = 29,
		ENUM_VCA_EVENT_OVER_TIME = 30,
		ENUM_VCA_EVENT_STICK_UP = 31,
		ENUM_VCA_EVENT_INSTALL_SCANNER = 32
	}

	public enum VCA_CROSS_DIRECTION
	{
		VCA_BOTH_DIRECTION,
		VCA_LEFT_GO_RIGHT,
		VCA_RIGHT_GO_LEFT
	}

	public struct NET_VCA_LINE
	{
		public NET_VCA_POINT struStart;

		public NET_VCA_POINT struEnd;
	}

	public struct NET_VCA_POLYGON
	{
		public uint dwPointNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.Struct)]
		public NET_VCA_POINT[] struPos;
	}

	public struct NET_VCA_TRAVERSE_PLANE
	{
		public NET_VCA_LINE struPlaneBottom;

		public VCA_CROSS_DIRECTION dwCrossDirection;

		public byte byRes1;

		public byte byPlaneHeight;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 38, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_VCA_AREA
	{
		public NET_VCA_POLYGON struRegion;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_INTRUSION
	{
		public NET_VCA_POLYGON struRegion;

		public ushort wDuration;

		public byte bySensitivity;

		public byte byRate;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_LOITER
	{
		public NET_VCA_POLYGON struRegion;

		public ushort wDuration;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_TAKE_LEFT
	{
		public NET_VCA_POLYGON struRegion;

		public ushort wDuration;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_PARKING
	{
		public NET_VCA_POLYGON struRegion;

		public ushort wDuration;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_RUN
	{
		public NET_VCA_POLYGON struRegion;

		public float fRunDistance;

		public byte byRes1;

		public byte byMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_HIGH_DENSITY
	{
		public NET_VCA_POLYGON struRegion;

		public float fDensity;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public ushort wDuration;
	}

	public struct NET_VCA_VIOLENT_MOTION
	{
		public NET_VCA_POLYGON struRegion;

		public ushort wDuration;

		public byte bySensitivity;

		public byte byMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_REACH_HIGHT
	{
		public NET_VCA_LINE struVcaLine;

		public ushort wDuration;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_GET_UP
	{
		public NET_VCA_POLYGON struRegion;

		public ushort wDuration;

		public byte byMode;

		public byte bySensitivity;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_LEFT
	{
		public NET_VCA_POLYGON struRegion;

		public ushort wDuration;

		public byte bySensitivity;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_TAKE
	{
		public NET_VCA_POLYGON struRegion;

		public ushort wDuration;

		public byte bySensitivity;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_OVER_TIME
	{
		public NET_VCA_POLYGON struRegion;

		public ushort wDuration;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_HUMAN_ENTER
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 23, ArraySubType = UnmanagedType.U4)]
		public uint[] dwRes;
	}

	public struct NET_VCA_STICK_UP
	{
		public NET_VCA_POLYGON struRegion;

		public ushort wDuration;

		public byte bySensitivity;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_SCANNER
	{
		public NET_VCA_POLYGON struRegion;

		public ushort wDuration;

		public byte bySensitivity;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_LEAVE_POSITION
	{
		public NET_VCA_POLYGON struRegion;

		public ushort wLeaveDelay;

		public ushort wStaticDelay;

		public byte byMode;

		public byte byPersonType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_TRAIL
	{
		public NET_VCA_POLYGON struRegion;

		public ushort wRes;

		public byte bySensitivity;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_FALL_DOWN
	{
		public NET_VCA_POLYGON struRegion;

		public ushort wDuration;

		public byte bySensitivity;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_AUDIO_ABNORMAL
	{
		public ushort wDecibel;

		public byte bySensitivity;

		public byte byAudioMode;

		public byte byEnable;

		public byte byThreshold;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 54, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_AUDIO_EXCEPTION
	{
		public uint dwSize;

		public byte byEnableAudioInException;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_VCA_AUDIO_ABNORMAL struAudioAbnormal;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 56, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SCHEDTIME[] struAlarmSched;

		public NET_DVR_HANDLEEXCEPTION_V40 struHandleException;

		public uint dwMaxRelRecordChanNum;

		public uint dwRelRecordChanNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.U4)]
		public uint[] byRelRecordChan;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_VCA_TOILET_TARRY
	{
		public NET_VCA_POLYGON struRegion;

		public ushort wDelay;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_YARD_TARRY
	{
		public NET_VCA_POLYGON struRegion;

		public ushort wDelay;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_ADV_REACH_HEIGHT
	{
		public NET_VCA_POLYGON struRegion;

		public uint dwCrossDirection;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_ADV_TRAVERSE_PLANE
	{
		public NET_VCA_POLYGON struRegion;

		public uint dwCrossDirection;

		public byte bySensitivity;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct NET_VCA_EVENT_UNION
	{
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 23, ArraySubType = UnmanagedType.U4)]
		public uint[] uLen;

		[FieldOffset(0)]
		public NET_VCA_TRAVERSE_PLANE struTraversePlane;

		[FieldOffset(0)]
		public NET_VCA_AREA struArea;

		[FieldOffset(0)]
		public NET_VCA_INTRUSION struIntrusion;

		[FieldOffset(0)]
		public NET_VCA_LOITER struLoiter;

		[FieldOffset(0)]
		public NET_VCA_TAKE_LEFT struTakeTeft;

		[FieldOffset(0)]
		public NET_VCA_PARKING struParking;

		[FieldOffset(0)]
		public NET_VCA_RUN struRun;

		[FieldOffset(0)]
		public NET_VCA_HIGH_DENSITY struHighDensity;

		[FieldOffset(0)]
		public NET_VCA_VIOLENT_MOTION struViolentMotion;

		[FieldOffset(0)]
		public NET_VCA_REACH_HIGHT struReachHight;

		[FieldOffset(0)]
		public NET_VCA_GET_UP struGetUp;

		[FieldOffset(0)]
		public NET_VCA_LEFT struLeft;

		[FieldOffset(0)]
		public NET_VCA_TAKE struTake;

		[FieldOffset(0)]
		public NET_VCA_HUMAN_ENTER struHumanEnter;

		[FieldOffset(0)]
		public NET_VCA_OVER_TIME struOvertime;

		[FieldOffset(0)]
		public NET_VCA_STICK_UP struStickUp;

		[FieldOffset(0)]
		public NET_VCA_SCANNER struScanner;

		[FieldOffset(0)]
		public NET_VCA_LEAVE_POSITION struLeavePos;

		[FieldOffset(0)]
		public NET_VCA_TRAIL struTrail;

		[FieldOffset(0)]
		public NET_VCA_FALL_DOWN struFallDown;

		[FieldOffset(0)]
		public NET_VCA_AUDIO_ABNORMAL struAudioAbnormal;

		[FieldOffset(0)]
		public NET_VCA_ADV_REACH_HEIGHT struReachHeight;

		[FieldOffset(0)]
		public NET_VCA_TOILET_TARRY struToiletTarry;

		[FieldOffset(0)]
		public NET_VCA_YARD_TARRY struYardTarry;

		[FieldOffset(0)]
		public NET_VCA_ADV_TRAVERSE_PLANE struAdvTraversePlane;
	}

	public enum SIZE_FILTER_MODE
	{
		IMAGE_PIX_MODE,
		REAL_WORLD_MODE,
		DEFAULT_MODE
	}

	public struct NET_VCA_SIZE_FILTER
	{
		public byte byActive;

		public byte byMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public NET_VCA_RECT struMiniRect;

		public NET_VCA_RECT struMaxRect;
	}

	public struct NET_VCA_ONE_RULE
	{
		public byte byActive;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRuleName;

		public VCA_EVENT_TYPE dwEventType;

		public NET_VCA_EVENT_UNION uEventParam;

		public NET_VCA_SIZE_FILTER struSizeFilter;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 14, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SCHEDTIME[] struAlarmTime;

		public NET_DVR_HANDLEEXCEPTION_V30 struHandleType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRelRecordChan;
	}

	public struct NET_VCA_RULECFG
	{
		public uint dwSize;

		public byte byPicProType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public NET_DVR_JPEGPARA struPictureParam;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
		public NET_VCA_ONE_RULE[] struRule;
	}

	public struct NET_VCA_FILTER_STRATEGY
	{
		public byte byStrategy;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 11, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_RULE_TRIGGER_PARAM
	{
		public byte byTriggerMode;

		public byte byTriggerPoint;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public float fTriggerArea;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_VCA_ONE_RULE_V41
	{
		public byte byActive;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public ushort wEventTypeEx;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRuleName;

		public VCA_EVENT_TYPE dwEventType;

		public NET_VCA_EVENT_UNION uEventParam;

		public NET_VCA_SIZE_FILTER struSizeFilter;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 56, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SCHEDTIME[] struAlarmTime;

		public NET_DVR_HANDLEEXCEPTION_V30 struHandleType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRelRecordChan;

		public ushort wAlarmDelay;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;

		public NET_VCA_FILTER_STRATEGY struFilterStrategy;

		public NET_VCA_RULE_TRIGGER_PARAM struTriggerParam;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_RULECFG_V41
	{
		public uint dwSize;

		public byte byPicProType;

		public byte byUpLastAlarm;

		public byte byPicRecordEnable;

		public byte byRes1;

		public NET_DVR_JPEGPARA struPictureParam;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
		public NET_VCA_ONE_RULE_V41[] struRule;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_TARGET_INFO
	{
		public uint dwID;

		public NET_VCA_RECT struRect;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_RULE_INFO
	{
		public byte byRuleID;

		public byte byRes;

		public ushort wEventTypeEx;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRuleName;

		public VCA_EVENT_TYPE dwEventType;

		public NET_VCA_EVENT_UNION uEventParam;
	}

	public struct NET_VCA_DEV_INFO
	{
		public NET_DVR_IPADDR struDevIP;

		public ushort wPort;

		public byte byChannel;

		public byte byRes;
	}

	public struct NET_VCA_RULE_ALARM
	{
		public uint dwSize;

		public uint dwRelativeTime;

		public uint dwAbsTime;

		public NET_VCA_RULE_INFO struRuleInfo;

		public NET_VCA_TARGET_INFO struTargetInfo;

		public NET_VCA_DEV_INFO struDevInfo;

		public uint dwPicDataLen;

		public byte byPicType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U4)]
		public uint[] dwRes;

		public IntPtr pImage;
	}

	public struct NET_VCA_DRAW_MODE
	{
		public uint dwSize;

		public byte byDspAddTarget;

		public byte byDspAddRule;

		public byte byDspPicAddTarget;

		public byte byDspPicAddRule;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public enum OBJECT_TYPE_ENUM
	{
		ENUM_OBJECT_TYPE_COAT = 1
	}

	public struct NET_DVR_OBJECT_COLOR_COND
	{
		public uint dwChannel;

		public uint dwObjType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_PIC
	{
		public byte byPicType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public uint dwPicWidth;

		public uint dwPicHeight;

		public uint dwPicDataLen;

		public uint dwPicDataBuffLen;

		public IntPtr byPicDataBuff;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 40, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_OBJECT_COLOR_UNION
	{
		public NET_DVR_COLOR struColor;

		public NET_DVR_PIC struPicture;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_OBJECT_COLOR
	{
		public uint dwSize;

		public byte byEnable;

		public byte byColorMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_DVR_OBJECT_COLOR_UNION uObjColor;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public enum AREA_TYPE_ENUM
	{
		ENUM_OVERLAP_REGION = 1,
		ENUM_BED_LOCATION
	}

	public struct NET_DVR_AUXAREA
	{
		public uint dwAreaType;

		public byte byEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_VCA_POLYGON struPolygon;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_AUXAREA_LIST
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_AUXAREA[] struArea;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public enum CHAN_WORKMODE_ENUM
	{
		ENUM_CHAN_WORKMODE_INDEPENDENT = 1,
		ENUM_CHAN_WORKMODE_MASTER,
		ENUM_CHAN_WORKMODE_SLAVE
	}

	public struct NET_DVR_CHANNEL_WORKMODE
	{
		public uint dwSize;

		public byte byWorkMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 63, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_CHANNEL
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byAddress;

		public ushort wDVRPort;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sUserName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] sPassword;

		public uint dwChannel;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct NET_DVR_SLAVE_CHANNEL_UNION
	{
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 152, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		[FieldOffset(0)]
		public uint dwLocalChannel;

		[FieldOffset(0)]
		public NET_DVR_CHANNEL struRemoteChannel;
	}

	public struct NET_DVR_SLAVE_CHANNEL_PARAM
	{
		public byte byChanType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_DVR_SLAVE_CHANNEL_UNION uSlaveChannel;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_SLAVE_CHANNEL_CFG
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SLAVE_CHANNEL_PARAM[] struChanParam;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public enum VQD_EVENT_ENUM
	{
		ENUM_VQD_EVENT_BLUR = 1,
		ENUM_VQD_EVENT_LUMA,
		ENUM_VQD_EVENT_CHROMA,
		ENUM_VQD_EVENT_SNOW,
		ENUM_VQD_EVENT_STREAK,
		ENUM_VQD_EVENT_FREEZE,
		ENUM_VQD_EVENT_SIGNAL_LOSS,
		ENUM_VQD_EVENT_PTZ,
		ENUM_VQD_EVENT_SCNENE_CHANGE,
		ENUM_VQD_EVENT_VIDEO_ABNORMAL,
		ENUM_VQD_EVENT_VIDEO_BLOCK
	}

	public struct NET_DVR_VQD_EVENT_COND
	{
		public uint dwChannel;

		public uint dwEventType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_VQD_EVENT_PARAM
	{
		public byte byThreshold;

		public byte byTriggerMode;

		public byte byUploadPic;

		public byte byRes1;

		public uint dwTimeInterval;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_VQD_EVENT_RULE
	{
		public uint dwSize;

		public byte byEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_DVR_VQD_EVENT_PARAM struEventParam;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 56, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SCHEDTIME[] struAlarmTime;

		public NET_DVR_HANDLEEXCEPTION_V30 struHandleType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
		public byte[] byRelRecordChan;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_BASELINE_SCENE
	{
		public uint dwSize;

		public byte byEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 63, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_CONTROL_BASELINE_SCENE_PARAM
	{
		public uint dwSize;

		public uint dwChannel;

		public byte byCommand;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 127, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_VQD_ALARM
	{
		public uint dwSize;

		public uint dwRelativeTime;

		public uint dwAbsTime;

		public NET_VCA_DEV_INFO struDevInfo;

		public uint dwEventType;

		public float fThreshold;

		public uint dwPicDataLen;

		public IntPtr pImage;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_CB_POINT
	{
		public NET_VCA_POINT struPoint;

		public NET_DVR_PTZPOS struPtzPos;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_TRACK_CALIBRATION_PARAM
	{
		public byte byPointNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_CB_POINT[] struCBPoint;
	}

	public struct NET_DVR_TRACK_CFG
	{
		public uint dwSize;

		public byte byEnable;

		public byte byFollowChan;

		public byte byDomeCalibrate;

		public byte byRes;

		public NET_DVR_TRACK_CALIBRATION_PARAM struCalParam;
	}

	public enum TRACK_MODE
	{
		MANUAL_CTRL,
		ALARM_TRACK
	}

	public struct NET_DVR_MANUAL_CTRL_INFO
	{
		public NET_VCA_POINT struCtrlPoint;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_TRACK_MODE
	{
		[StructLayout(LayoutKind.Explicit)]
		public struct uModeParam
		{
			[FieldOffset(0)]
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U4)]
			public uint[] dwULen;

			[FieldOffset(0)]
			public NET_DVR_MANUAL_CTRL_INFO struManualCtrl;
		}

		public uint dwSize;

		public byte byTrackMode;

		public byte byRuleConfMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_ALARM_JPEG
	{
		public byte byPicProType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public NET_DVR_JPEGPARA struPicParam;
	}

	public struct NET_IVMS_ONE_RULE
	{
		public byte byActive;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRuleName;

		public VCA_EVENT_TYPE dwEventType;

		public NET_VCA_EVENT_UNION uEventParam;

		public NET_VCA_SIZE_FILTER struSizeFilter;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 68, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_IVMS_RULECFG
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
		public NET_IVMS_ONE_RULE[] struRule;
	}

	public struct NET_IVMS_BEHAVIORCFG
	{
		public uint dwSize;

		public byte byPicProType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public NET_DVR_JPEGPARA struPicParam;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 28, ArraySubType = UnmanagedType.Struct)]
		public NET_IVMS_RULECFG[] struRuleCfg;
	}

	public struct NET_IVMS_DEVSCHED
	{
		public NET_DVR_SCHEDTIME struTime;

		public NET_DVR_PU_STREAM_CFG struPUStream;
	}

	public struct NET_IVMS_STREAMCFG
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 28, ArraySubType = UnmanagedType.Struct)]
		public NET_IVMS_DEVSCHED[] struDevSched;
	}

	public struct NET_VCA_MASK_REGION
	{
		public byte byEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public NET_VCA_POLYGON struPolygon;
	}

	public struct NET_VCA_MASK_REGION_LIST
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.Struct)]
		public NET_VCA_MASK_REGION[] struMask;
	}

	public struct NET_VCA_ENTER_REGION
	{
		public uint dwSize;

		public byte byEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_VCA_POLYGON struPolygon;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_IVMS_MASK_REGION_LIST
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 28, ArraySubType = UnmanagedType.Struct)]
		public NET_VCA_MASK_REGION_LIST[] struList;
	}

	public struct NET_IVMS_ENTER_REGION
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 28, ArraySubType = UnmanagedType.Struct)]
		public NET_VCA_ENTER_REGION[] struEnter;
	}

	public struct NET_IVMS_ALARM_JPEG
	{
		public byte byPicProType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public NET_DVR_JPEGPARA struPicParam;
	}

	public struct NET_IVMS_SEARCHCFG
	{
		public uint dwSize;

		public NET_DVR_MATRIX_DEC_REMOTE_PLAY struRemotePlay;

		public NET_IVMS_ALARM_JPEG struAlarmJpeg;

		public NET_IVMS_RULECFG struRuleCfg;
	}

	public struct NET_DVR_IDENTIFICATION_PARAM
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sUserName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] sPassword;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct NET_DVR_MOUNT_PARAM_UNION
	{
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 52, ArraySubType = UnmanagedType.I1)]
		public byte[] uLen;

		[FieldOffset(0)]
		public NET_DVR_IDENTIFICATION_PARAM struIdentificationParam;
	}

	public struct NET_DVR_NAS_MOUNT_PARAM
	{
		public byte byMountType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public NET_DVR_MOUNT_PARAM_UNION uMountParam;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct NET_DVR_MOUNTMETHOD_PARAM_UNION
	{
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 56, ArraySubType = UnmanagedType.I1)]
		public byte[] uLen;

		[FieldOffset(0)]
		public NET_DVR_NAS_MOUNT_PARAM struNasMountParam;
	}

	public struct NET_DVR_SINGLE_NET_DISK_INFO
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_DVR_IPADDR struNetDiskAddr;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
		public byte[] sDirectory;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 68, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_NET_DISKCFG
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SINGLE_NET_DISK_INFO[] struNetDiskParam;
	}

	public enum MAIN_EVENT_TYPE
	{
		EVENT_MOT_DET = 0,
		EVENT_ALARM_IN = 1,
		EVENT_VCA_BEHAVIOR = 2,
		EVENT_INQUEST = 3,
		EVENT_VCA_DETECTION = 4,
		EVENT_STREAM_INFO = 100
	}

	public enum VCA_DETECTION_MINOR_TYPE : uint
	{
		EVENT_VCA_TRAVERSE_PLANE = 1u,
		EVENT_FIELD_DETECTION = 2u,
		EVENT_AUDIO_INPUT_ALARM = 3u,
		EVENT_SOUND_INTENSITY_ALARM = 4u,
		EVENT_FACE_DETECTION = 5u,
		EVENT_VIRTUAL_FOCUS_ALARM = 6u,
		EVENT_SCENE_CHANGE_ALARM = 7u,
		EVENT_ALL = uint.MaxValue
	}

	public enum BEHAVIOR_MINOR_TYPE
	{
		EVENT_TRAVERSE_PLANE = 0,
		EVENT_ENTER_AREA = 1,
		EVENT_EXIT_AREA = 2,
		EVENT_INTRUSION = 3,
		EVENT_LOITER = 4,
		EVENT_LEFT_TAKE = 5,
		EVENT_PARKING = 6,
		EVENT_RUN = 7,
		EVENT_HIGH_DENSITY = 8,
		EVENT_STICK_UP = 9,
		EVENT_INSTALL_SCANNER = 10,
		EVENT_OPERATE_OVER_TIME = 11,
		EVENT_FACE_DETECT = 12,
		EVENT_LEFT = 13,
		EVENT_TAKE = 14,
		EVENT_LEAVE_POSITION = 15,
		EVENT_TRAIL_INFO = 16,
		EVENT_FALL_DOWN_INFO = 19,
		EVENT_OBJECT_PASTE = 20,
		EVENT_FACE_CAPTURE_INFO = 21,
		EVENT_MULTI_FACES_INFO = 22,
		EVENT_AUDIO_ABNORMAL_INFO = 23,
		EVENT_DETECT = 24
	}

	public enum STREAM_INFO_MINOR_TYPE
	{
		EVENT_STREAM_ID,
		EVENT_TIMING,
		EVENT_MOTION_DETECT,
		EVENT_ALARM,
		EVENT_ALARM_OR_MOTION_DETECT,
		EVENT_ALARM_AND_MOTION_DETECT,
		EVENT_COMMAND_TRIGGER,
		EVENT_MANNUAL,
		EVENT_BACKUP_VOLUME
	}

	public struct NET_DVR_STREAM_INFO
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byID;

		public uint dwChannel;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public void init()
		{
			byID = new byte[32];
			byRes = new byte[32];
		}
	}

	public struct EVENT_ALARM_BYBIT
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 160, ArraySubType = UnmanagedType.I1)]
		public byte[] byAlarmInNo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 140, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public void init()
		{
			byAlarmInNo = new byte[160];
			byRes = new byte[236];
		}
	}

	public struct EVENT_ALARM_BYVALUE
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.U2)]
		public ushort[] wAlarmInNo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 44, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public void init()
		{
			wAlarmInNo = new ushort[128];
			byRes = new byte[44];
		}
	}

	public struct EVENT_MOTION_BYBIT
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byMotDetChanNo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 236, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public void init()
		{
			byMotDetChanNo = new byte[64];
			byRes = new byte[236];
		}
	}

	public struct EVENT_MOTION_BYVALUE
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.U2)]
		public ushort[] wMotDetChanNo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 172, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public void init()
		{
			wMotDetChanNo = new ushort[64];
			byRes = new byte[172];
		}
	}

	public struct EVENT_VCA_BYBIT
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byChanNo;

		public byte byRuleID;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 235, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public void init()
		{
			byChanNo = new byte[64];
			byRes1 = new byte[235];
		}
	}

	public struct EVENT_VCA_BYVALUE
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.U2)]
		public ushort[] wChanNo;

		public byte byRuleID;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 171, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public void init()
		{
			wChanNo = new ushort[64];
			byRes = new byte[171];
		}
	}

	public struct EVENT_INQUEST_PARAM
	{
		public byte byRoomIndex;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 299, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public void init()
		{
			byRes = new byte[299];
		}
	}

	public struct EVENT_VCADETECT_BYBIT
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.I1)]
		public byte[] byChan;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 44, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public void init()
		{
			byChan = new byte[256];
			byRes = new byte[44];
		}
	}

	public struct EVENT_VCADETECT_BYVALUE
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 63, ArraySubType = UnmanagedType.U4)]
		public uint[] dwChanNo;

		public byte byAll;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 47, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public void init()
		{
			dwChanNo = new uint[63];
			byRes = new byte[47];
		}
	}

	public struct EVENT_STREAMID_PARAM
	{
		public NET_DVR_STREAM_INFO struIDInfo;

		public uint dwCmdType;

		public byte byBackupVolumeNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 223, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public void init()
		{
			struIDInfo.init();
			byRes = new byte[223];
		}
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct SEARCH_EVENT_UNION
	{
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 300, ArraySubType = UnmanagedType.I1)]
		public byte[] byLen;

		[FieldOffset(0)]
		public EVENT_ALARM_BYBIT struAlarmParam;

		[FieldOffset(0)]
		public EVENT_ALARM_BYVALUE struAlarmParamByValue;

		[FieldOffset(0)]
		public EVENT_MOTION_BYBIT struMotionParam;

		[FieldOffset(0)]
		public EVENT_MOTION_BYVALUE struMotionParamByValue;

		[FieldOffset(0)]
		public EVENT_VCA_BYBIT struVcaParam;

		[FieldOffset(0)]
		public EVENT_VCA_BYVALUE struVcaParamByValue;

		[FieldOffset(0)]
		public EVENT_INQUEST_PARAM struInquestParam;

		[FieldOffset(0)]
		public EVENT_VCADETECT_BYBIT struVCADetectByBit;

		[FieldOffset(0)]
		public EVENT_VCADETECT_BYVALUE struVCADetectByValue;

		[FieldOffset(0)]
		public EVENT_STREAMID_PARAM struStreamIDParam;
	}

	public struct NET_DVR_SEARCH_EVENT_PARAM
	{
		public ushort wMajorType;

		public ushort wMinorType;

		public NET_DVR_TIME struStartTime;

		public NET_DVR_TIME struEndTime;

		public byte byLockType;

		public byte byValue;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 130, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public SEARCH_EVENT_UNION uSeniorPara;
	}

	public struct EVENT_ALARM_RET
	{
		public uint dwAlarmInNo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 300, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public void init()
		{
			byRes = new byte[300];
		}
	}

	public struct EVENT_MOTION_RET
	{
		public uint dwMotDetNo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 300, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public void init()
		{
			byRes = new byte[300];
		}
	}

	public struct EVENT_VCA_RET
	{
		public uint dwChanNo;

		public byte byRuleID;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRuleName;

		public NET_VCA_EVENT_UNION uEvent;

		public void init()
		{
			byRes1 = new byte[3];
			byRuleName = new byte[32];
		}
	}

	public struct EVENT_INQUEST_RET
	{
		public byte byRoomIndex;

		public byte byDriveIndex;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public uint dwSegmentNo;

		public ushort wSegmetSize;

		public ushort wSegmentState;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 288, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;

		public void init()
		{
			byRes1 = new byte[6];
			byRes2 = new byte[288];
		}
	}

	public struct EVENT_STREAMID_RET
	{
		public uint dwRecordType;

		public uint dwRecordLength;

		public byte byLockFlag;

		public byte byDrawFrameType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byFileName;

		public uint dwFileIndex;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public void init()
		{
			byRes1 = new byte[2];
			byFileName = new byte[32];
			byRes = new byte[256];
		}
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct SEARCH_EVENT_RET
	{
		[FieldOffset(0)]
		public EVENT_ALARM_RET struAlarmRet;

		[FieldOffset(0)]
		public EVENT_MOTION_RET struMotionRet;

		[FieldOffset(0)]
		public EVENT_VCA_RET struVcaRet;

		[FieldOffset(0)]
		public EVENT_INQUEST_RET struInquestRet;

		[FieldOffset(0)]
		public EVENT_STREAMID_RET struStreamIDRet;

		public void init()
		{
			struAlarmRet = default(EVENT_ALARM_RET);
			struAlarmRet.init();
		}
	}

	public struct NET_DVR_SEARCH_EVENT_RET
	{
		public ushort wMajorType;

		public ushort wMinorType;

		public NET_DVR_TIME struStartTime;

		public NET_DVR_TIME struEndTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byChan;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 36, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public SEARCH_EVENT_RET uSeniorRet;

		public void init()
		{
			byChan = new byte[64];
			byRes = new byte[36];
			uSeniorRet = default(SEARCH_EVENT_RET);
			uSeniorRet.init();
		}
	}

	public enum tagCALIBRATE_TYPE
	{
		PDC_CALIBRATE = 1,
		BEHAVIOR_OUT_CALIBRATE,
		BEHAVIOR_IN_CALIBRATE,
		ITS_CALBIRETE
	}

	public struct NET_DVR_RECT_LIST
	{
		public byte byRectNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 11, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.Struct)]
		public NET_VCA_RECT[] struVcaRect;
	}

	public struct NET_DVR_PDC_CALIBRATION
	{
		public NET_DVR_RECT_LIST struRectList;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 120, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public enum LINE_MODE
	{
		HEIGHT_LINE,
		LENGTH_LINE
	}

	public struct NET_DVR_CAMERA_PARAM
	{
		public byte byEnableHeight;

		public byte byEnableAngle;

		public byte byEnableHorizon;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public float fCameraHeight;

		public float fCameraAngle;

		public float fHorizon;
	}

	public struct NET_DVR_LINE_SEGMENT
	{
		public byte byLineMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public NET_VCA_POINT struStartPoint;

		public NET_VCA_POINT struEndPoint;

		public float fValue;
	}

	public struct NET_DVR_BEHAVIOR_OUT_CALIBRATION
	{
		public uint dwLineSegNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_LINE_SEGMENT[] struLineSegment;

		public NET_DVR_CAMERA_PARAM struCameraParam;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_IN_CAL_SAMPLE
	{
		public NET_VCA_RECT struVcaRect;

		public NET_DVR_LINE_SEGMENT struLineSegment;
	}

	public struct NET_DVR_BEHAVIOR_IN_CALIBRATION
	{
		public uint dwCalSampleNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_IN_CAL_SAMPLE[] struCalSample;

		public NET_DVR_CAMERA_PARAM struCameraParam;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_ITS_CALIBRATION
	{
		public uint dwPointNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.Struct)]
		public NET_VCA_POINT[] struPoint;

		public float fWidth;

		public float fHeight;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 100, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct NET_DVR_CALIBRATION_PRARM_UNION
	{
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 240, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		[FieldOffset(0)]
		public NET_DVR_PDC_CALIBRATION struPDCCalibration;

		[FieldOffset(0)]
		public NET_DVR_BEHAVIOR_OUT_CALIBRATION struBehaviorOutCalibration;

		[FieldOffset(0)]
		public NET_DVR_BEHAVIOR_IN_CALIBRATION struBehaviorInCalibration;

		[FieldOffset(0)]
		public NET_DVR_ITS_CALIBRATION struITSCalibration;
	}

	public struct NET_DVR_CALIBRATION_CFG
	{
		public uint dwSize;

		public byte byEnable;

		public byte byCalibrationType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_DVR_CALIBRATION_PRARM_UNION uCalibrateParam;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_PDC_ENTER_DIRECTION
	{
		public NET_VCA_POINT struStartPoint;

		public NET_VCA_POINT struEndPoint;
	}

	public struct NET_DVR_PDC_RULE_CFG
	{
		public uint dwSize;

		public byte byEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 23, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_VCA_POLYGON struPolygon;

		public NET_DVR_PDC_ENTER_DIRECTION struEnterDirection;
	}

	public struct NET_DVR_PDC_RULE_CFG_V41
	{
		public uint dwSize;

		public byte byEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 23, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_VCA_POLYGON struPolygon;

		public NET_DVR_PDC_ENTER_DIRECTION struEnterDirection;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 56, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SCHEDTIME struAlarmTime;

		public NET_DVR_TIME_EX struDayStartTime;

		public NET_DVR_TIME_EX struNightStartTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 100, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_TRIAL_VERSION_CFG
	{
		public uint dwSize;

		public ushort wReserveTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 62, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_SYN_CHANNEL_NAME_PARAM
	{
		public uint dwSize;

		public uint dwChannel;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_RESET_COUNTER_CFG
	{
		public uint dwSize;

		public byte byEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_TIME_EX[] struTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_VCA_CTRLINFO_COND
	{
		public uint dwSize;

		public NET_DVR_STREAM_INFO struStreamInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_VCA_CTRLINFO_CFG
	{
		public uint dwSize;

		public byte byVCAEnable;

		public byte byVCAType;

		public byte byStreamWithVCA;

		public byte byMode;

		public byte byControlType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 83, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public enum PDC_PARAM_KEY
	{
		HUMAN_GENERATE_RATE = 50,
		DETECT_SENSITIVE
	}

	public struct NET_DVR_PDC_TARGET_INFO
	{
		public uint dwTargetID;

		public NET_VCA_RECT struTargetRect;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;
	}

	public struct NET_DVR_PDC_TARGET_IN_FRAME
	{
		public byte byTargetNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] yRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 30, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_PDC_TARGET_INFO[] struTargetInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct UNION_STATFRAME
	{
		public uint dwRelativeTime;

		public uint dwAbsTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 92, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct UNION_STATTIME
	{
		public NET_DVR_TIME tmStart;

		public NET_DVR_TIME tmEnd;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 92, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_PDC_ALRAM_INFO
	{
		[StructLayout(LayoutKind.Explicit)]
		public struct uStatModeParam
		{
			[FieldOffset(0)]
			public UNION_STATFRAME struStatFrame;

			[FieldOffset(0)]
			public UNION_STATTIME struStatTime;
		}

		public uint dwSize;

		public byte byMode;

		public byte byChannel;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_VCA_DEV_INFO struDevInfo;

		public uint dwLeaveNum;

		public uint dwEnterNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 40, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_PDC_QUERY
	{
		public NET_DVR_TIME tmStart;

		public NET_DVR_TIME tmEnd;

		public uint dwLeaveNum;

		public uint dwEnterNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;
	}

	public struct NET_DVR_PTZ_POSITION
	{
		public byte byEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byPtzPositionName;

		public NET_DVR_PTZPOS struPtzPos;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 40, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_POSITION_RULE_CFG
	{
		public uint dwSize;

		public NET_DVR_PTZ_POSITION struPtzPosition;

		public NET_VCA_RULECFG struVcaRuleCfg;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 80, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_POSITION_RULE_CFG_V41
	{
		public uint dwSize;

		public NET_DVR_PTZ_POSITION struPtzPosition;

		public NET_VCA_RULECFG_V41 struVcaRuleCfg;

		public byte byTrackEnable;

		public byte byRes1;

		public ushort wTrackDuration;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 76, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_LIMIT_ANGLE
	{
		public byte byEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_DVR_PTZPOS struUp;

		public NET_DVR_PTZPOS struDown;

		public NET_DVR_PTZPOS struLeft;

		public NET_DVR_PTZPOS struRight;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_POSITION_INDEX
	{
		public byte byIndex;

		public byte byRes1;

		public ushort wDwell;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_POSITION_TRACK_CFG
	{
		public uint dwSize;

		public byte byNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_POSITION_INDEX[] struPositionIndex;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_PATROL_SCENE_INFO
	{
		public ushort wDwell;

		public byte byPositionID;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_PATROL_TRACKCFG
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_PATROL_SCENE_INFO[] struPatrolSceneInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_TRACK_PARAMCFG
	{
		public uint dwSize;

		public ushort wAlarmDelayTime;

		public ushort wTrackHoldTime;

		public byte byTrackMode;

		public byte byPreDirection;

		public byte byTrackSmooth;

		public byte byZoomAdjust;

		public byte byMaxTrackZoom;

		public byte byStopTrackWhenFindFace;

		public byte byStopTrackThreshold;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_DOME_MOVEMENT_PARAM
	{
		public ushort wMaxZoom;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 42, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public enum TRAFFIC_AID_TYPE
	{
		CONGESTION = 1,
		PARKING = 2,
		INVERSE = 4,
		PEDESTRIAN = 8,
		DEBRIS = 0x10,
		SMOKE = 0x20,
		OVERLINE = 0x40,
		VEHICLE_CONTROL_LIST = 0x80,
		SPEED = 0x100
	}

	public enum TRAFFIC_SCENE_MODE
	{
		FREEWAY,
		TUNNEL,
		BRIDGE
	}

	public enum ITS_ABILITY_TYPE
	{
		ITS_CONGESTION_ABILITY = 1,
		ITS_PARKING_ABILITY = 2,
		ITS_INVERSE_ABILITY = 4,
		ITS_PEDESTRIAN_ABILITY = 8,
		ITS_DEBRIS_ABILITY = 0x10,
		ITS_SMOKE_ABILITY = 0x20,
		ITS_OVERLINE_ABILITY = 0x40,
		ITS_VEHICLE_CONTROL_LIST_ABILITY = 0x80,
		ITS_SPEED_ABILITY = 0x100,
		ITS_LANE_VOLUME_ABILITY = 0x10000,
		ITS_LANE_VELOCITY_ABILITY = 0x20000,
		ITS_TIME_HEADWAY_ABILITY = 0x40000,
		ITS_SPACE_HEADWAY_ABILITY = 0x80000,
		ITS_TIME_OCCUPANCY_RATIO_ABILITY = 0x100000,
		ITS_SPACE_OCCUPANCY_RATIO_ABILITY = 0x200000,
		ITS_LANE_QUEUE_ABILITY = 0x400000,
		ITS_VEHICLE_TYPE_ABILITY = 0x800000,
		ITS_TRAFFIC_STATE_ABILITY = 0x1000000
	}

	public enum ITS_TPS_TYPE
	{
		LANE_VOLUME = 1,
		LANE_VELOCITY = 2,
		TIME_HEADWAY = 4,
		SPACE_HEADWAY = 8,
		TIME_OCCUPANCY_RATIO = 0x10,
		SPACE_OCCUPANCY_RATIO = 0x20,
		QUEUE = 0x40,
		VEHICLE_TYPE = 0x80,
		TRAFFIC_STATE = 0x100
	}

	public struct NET_DVR_REGION_LIST
	{
		public uint dwSize;

		public byte byNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
		public NET_VCA_POLYGON[] struPolygon;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_DIRECTION
	{
		public NET_VCA_POINT struStartPoint;

		public NET_VCA_POINT struEndPoint;
	}

	public struct NET_DVR_ONE_LANE
	{
		public byte byEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 11, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byLaneName;

		public NET_DVR_DIRECTION struFlowDirection;

		public NET_VCA_POLYGON struPolygon;
	}

	public struct NET_DVR_LANE_CFG
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_ONE_LANE[] struLane;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 40, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;
	}

	public struct NET_DVR_AID_PARAM
	{
		public ushort wParkingDuration;

		public ushort wPedestrianDuration;

		public ushort wDebrisDuration;

		public ushort wCongestionLength;

		public ushort wCongestionDuration;

		public ushort wInverseDuration;

		public ushort wInverseDistance;

		public ushort wInverseAngleTolerance;

		public ushort wIllegalParkingTime;

		public ushort wIllegalParkingPicNum;

		public byte byMergePic;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 23, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;
	}

	public struct NET_DVR_ONE_AID_RULE
	{
		public byte byEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRuleName;

		public uint dwEventType;

		public NET_VCA_SIZE_FILTER struSizeFilter;

		public NET_VCA_POLYGON struPolygon;

		public NET_DVR_AID_PARAM struAIDParam;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 14, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SCHEDTIME[] struAlarmTime;

		public NET_DVR_HANDLEEXCEPTION_V30 struHandleType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRelRecordChan;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_AID_RULECFG
	{
		public uint dwSize;

		public byte byPicProType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_DVR_JPEGPARA struPictureParam;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_ONE_AID_RULE[] struOneAIDRule;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_ONE_AID_RULE_V41
	{
		public byte byEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRuleName;

		public uint dwEventType;

		public NET_VCA_SIZE_FILTER struSizeFilter;

		public NET_VCA_POLYGON struPolygon;

		public NET_DVR_AID_PARAM struAIDParam;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 56, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SCHEDTIME[] struAlarmTime;

		public NET_DVR_HANDLEEXCEPTION_V30 struHandleType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
		public byte[] byRelRecordChan;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 60, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_AID_RULECFG_V41
	{
		public uint dwSize;

		public byte byPicProType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_DVR_JPEGPARA struPictureParam;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_ONE_AID_RULE_V41[] struAIDRule;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_ONE_TPS_RULE
	{
		public byte byEnable;

		public byte byLaneID;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public uint dwCalcType;

		public NET_VCA_SIZE_FILTER struSizeFilter;

		public NET_VCA_POLYGON struVitrualLoop;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 14, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SCHEDTIME[] struAlarmTime;

		public NET_DVR_HANDLEEXCEPTION_V30 struHandleType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_TPS_RULECFG
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_ONE_TPS_RULE[] struOneTpsRule;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 40, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_ONE_TPS_RULE_V41
	{
		public byte byEnable;

		public byte byLaneID;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public uint dwCalcType;

		public NET_VCA_SIZE_FILTER struSizeFilter;

		public NET_VCA_POLYGON struVitrualLoop;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 56, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SCHEDTIME[] struAlarmTime;

		public NET_DVR_HANDLEEXCEPTION_V30 struHandleType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 60, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_TPS_RULECFG_V41
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_ONE_TPS_RULE_V41[] struOneTpsRule;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_AID_INFO
	{
		public byte byRuleID;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRuleName;

		public uint dwAIDType;

		public NET_DVR_DIRECTION struDirect;

		public byte bySpeedLimit;

		public byte byCurrentSpeed;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 38, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_AID_ALARM
	{
		public uint dwSize;

		public uint dwRelativeTime;

		public uint dwAbsTime;

		public NET_VCA_DEV_INFO struDevInfo;

		public NET_DVR_AID_INFO struAIDInfo;

		public uint dwPicDataLen;

		public IntPtr pImage;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 40, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_LANE_QUEUE
	{
		public NET_VCA_POINT struHead;

		public NET_VCA_POINT struTail;

		public uint dwLength;
	}

	public enum TRAFFIC_DATA_VARY_TYPE
	{
		NO_VARY,
		VEHICLE_ENTER,
		VEHICLE_LEAVE,
		UEUE_VARY
	}

	public struct NET_DVR_LANE_PARAM
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRuleName;

		public byte byRuleID;

		public byte byVaryType;

		public byte byLaneType;

		public byte byRes1;

		public uint dwLaneVolume;

		public uint dwLaneVelocity;

		public uint dwTimeHeadway;

		public uint dwSpaceHeadway;

		public float fSpaceOccupyRation;

		public NET_DVR_LANE_QUEUE struLaneQueue;

		public NET_VCA_POINT struRuleLocation;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_TPS_INFO
	{
		public uint dwLanNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_LANE_PARAM[] struLaneParam;
	}

	public struct NET_DVR_TPS_ALARM
	{
		public uint dwSize;

		public uint dwRelativeTime;

		public uint dwAbsTime;

		public NET_VCA_DEV_INFO struDevInfo;

		public NET_DVR_TPS_INFO struTPSInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;
	}

	public enum TRAFFIC_DATA_VARY_TYPE_EX_ENUM
	{
		ENUM_TRAFFIC_VARY_NO = 0,
		ENUM_TRAFFIC_VARY_VEHICLE_ENTER = 1,
		ENUM_TRAFFIC_VARY_VEHICLE_LEAVE = 2,
		ENUM_TRAFFIC_VARY_QUEUE = 4,
		ENUM_TRAFFIC_VARY_STATISTIC = 8
	}

	public struct NET_DVR_LANE_PARAM_V41
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRuleName;

		public byte byRuleID;

		public byte byLaneType;

		public byte byTrafficState;

		public byte byRes1;

		public uint dwVaryType;

		public uint dwTpsType;

		public uint dwLaneVolume;

		public uint dwLaneVelocity;

		public uint dwTimeHeadway;

		public uint dwSpaceHeadway;

		public float fSpaceOccupyRation;

		public float fTimeOccupyRation;

		public uint dwLightVehicle;

		public uint dwMidVehicle;

		public uint dwHeavyVehicle;

		public NET_DVR_LANE_QUEUE struLaneQueue;

		public NET_VCA_POINT struRuleLocation;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_TPS_INFO_V41
	{
		public uint dwLanNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_LANE_PARAM_V41[] struLaneParam;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_FACEDETECT_RULECFG
	{
		public uint dwSize;

		public byte byEnable;

		public byte byEventType;

		public byte byUpLastAlarm;

		public byte byUpFacePic;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRuleName;

		public NET_VCA_POLYGON struVcaPolygon;

		public byte byPicProType;

		public byte bySensitivity;

		public ushort wDuration;

		public NET_DVR_JPEGPARA struPictureParam;

		public NET_VCA_SIZE_FILTER struSizeFilter;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 14, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SCHEDTIME[] struAlarmTime;

		public NET_DVR_HANDLEEXCEPTION_V30 struHandleType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRelRecordChan;

		public byte byPicRecordEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 39, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_FACE_PIPCFG
	{
		public byte byEnable;

		public byte byBackChannel;

		public byte byPosition;

		public byte byPIPDiv;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_FACEDETECT_RULECFG_V41
	{
		public uint dwSize;

		public byte byEnable;

		public byte byEventType;

		public byte byUpLastAlarm;

		public byte byUpFacePic;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRuleName;

		public NET_VCA_POLYGON struVcaPolygon;

		public byte byPicProType;

		public byte bySensitivity;

		public ushort wDuration;

		public NET_DVR_JPEGPARA struPictureParam;

		public NET_VCA_SIZE_FILTER struSizeFilter;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 56, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SCHEDTIME[] struAlarmTime;

		public NET_DVR_HANDLEEXCEPTION_V30 struHandleType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRelRecordChan;

		public byte byPicRecordEnable;

		public byte byRes1;

		public ushort wAlarmDelay;

		public NET_DVR_FACE_PIPCFG struFacePIP;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 28, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_FACEDETECT_ALARM
	{
		public uint dwSize;

		public uint dwRelativeTime;

		public uint dwAbsTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRuleName;

		public NET_VCA_TARGET_INFO struTargetInfo;

		public NET_VCA_DEV_INFO struDevInfo;

		public uint dwPicDataLen;

		public byte byAlarmPicType;

		public byte byPanelChan;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public uint dwFacePicDataLen;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public IntPtr pFaceImage;

		public IntPtr pImage;
	}

	public struct NET_DVR_EVENT_PARAM_UNION
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U4)]
		public uint[] uLen;

		public uint dwHumanIn;

		public float fCrowdDensity;
	}

	public struct NET_DVR_EVENT_INFO
	{
		public byte byRuleID;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRuleName;

		public uint dwEventType;

		public NET_DVR_EVENT_PARAM_UNION uEventParam;
	}

	public struct NET_DVR_EVENT_INFO_LIST
	{
		public byte byNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_EVENT_INFO[] struEventInfo;
	}

	public struct NET_DVR_RULE_INFO_ALARM
	{
		public uint dwSize;

		public uint dwRelativeTime;

		public uint dwAbsTime;

		public NET_VCA_DEV_INFO struDevInfo;

		public NET_DVR_EVENT_INFO_LIST struEventInfoList;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 40, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_ONE_SCENE_TIME
	{
		public byte byActive;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public uint dwSceneID;

		public NET_DVR_SCHEDTIME struEffectiveTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_SCENE_TIME_CFG
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_ONE_SCENE_TIME[] struSceneTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_ONE_SCENE_CFG
	{
		public byte byEnable;

		public byte byDirection;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public uint dwSceneID;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] bySceneName;

		public NET_DVR_PTZPOS struPtzPos;

		public uint dwTrackTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_SCENE_CFG
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_ONE_SCENE_CFG[] struSceneCfg;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 40, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_SCENE_COND
	{
		public uint dwSize;

		public int lChannel;

		public uint dwSceneID;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_FORENSICS_MODE
	{
		public uint dwSize;

		public byte byMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 23, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_SCENE_INFO
	{
		public uint dwSceneID;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] bySceneName;

		public byte byDirection;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_DVR_PTZPOS struPtzPos;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_AID_ALARM_V41
	{
		public uint dwSize;

		public uint dwRelativeTime;

		public uint dwAbsTime;

		public NET_VCA_DEV_INFO struDevInfo;

		public NET_DVR_AID_INFO struAIDInfo;

		public NET_DVR_SCENE_INFO struSceneInfo;

		public uint dwPicDataLen;

		public IntPtr pImage;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_TPS_ALARM_V41
	{
		public uint dwSize;

		public uint dwRelativeTime;

		public uint dwAbsTime;

		public NET_VCA_DEV_INFO struDevInfo;

		public NET_DVR_TPS_INFO_V41 struTPSInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_VCA_VERSION
	{
		public ushort wMajorVersion;

		public ushort wMinorVersion;

		public ushort wRevisionNumber;

		public ushort wBuildNumber;

		public ushort wVersionYear;

		public byte byVersionMonth;

		public byte byVersionDay;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_PLATE_PARAM
	{
		public byte byPlateRecoMode;

		public byte byBelive;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 22, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_PLATECFG
	{
		public uint dwSize;

		public uint dwEnable;

		public byte byPicProType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_DVR_JPEGPARA struPictureParam;

		public NET_DVR_PLATE_PARAM struPlateParam;

		public NET_DVR_HANDLEEXCEPTION struHandleType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 28, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SCHEDTIME[] struAlarmTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byRelRecordChan;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_PLATE_INFO
	{
		public byte byPlateType;

		public byte byColor;

		public byte byBright;

		public byte byLicenseLen;

		public byte byEntireBelieve;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 35, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public NET_VCA_RECT struPlateRect;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sLicense;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byBelieve;
	}

	public struct NET_DVR_PLATERECO_RESULE
	{
		public uint dwSize;

		public uint dwRelativeTime;

		public uint dwAbsTime;

		public NET_VCA_DEV_INFO struDevInfo;

		public NET_DVR_PLATE_INFO struPlateInfo;

		public uint dwPicDataLen;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U4)]
		public uint[] dwRes;

		public IntPtr pImage;
	}

	public struct NET_DVR_IO_INCFG
	{
		public uint dwSize;

		public byte byIoInStatus;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_IO_OUTCFG
	{
		public uint dwSize;

		public byte byDefaultStatus;

		public byte byIoOutStatus;

		public ushort wAheadTime;

		public uint dwTimePluse;

		public uint dwTimeDelay;

		public byte byFreqMulti;

		public byte byDutyRate;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_FLASH_OUTCFG
	{
		public uint dwSize;

		public byte byMode;

		public byte byRelatedIoIn;

		public byte byRecognizedLane;

		public byte byDetectBrightness;

		public byte byBrightnessThreld;

		public byte byStartHour;

		public byte byStartMinute;

		public byte byEndHour;

		public byte byEndMinute;

		public byte byFlashLightEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_LIGHTSNAPCFG
	{
		public uint dwSize;

		public byte byLightIoIn;

		public byte byTrigIoIn;

		public byte byRelatedDriveWay;

		public byte byTrafficLight;

		public byte bySnapTimes1;

		public byte bySnapTimes2;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U2)]
		public ushort[] wIntervalTime1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U2)]
		public ushort[] wIntervalTime2;

		public byte byRecord;

		public byte bySessionTimeout;

		public byte byPreRecordTime;

		public byte byVideoDelay;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_MEASURESPEEDCFG
	{
		public uint dwSize;

		public byte byTrigIo1;

		public byte byTrigIo2;

		public byte byRelatedDriveWay;

		public byte byTestSpeedTimeOut;

		public uint dwDistance;

		public byte byCapSpeed;

		public byte bySpeedLimit;

		public byte bySnapTimes1;

		public byte bySnapTimes2;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U2)]
		public ushort[] wIntervalTime1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U2)]
		public ushort[] wIntervalTime2;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_VIDEOEFFECT
	{
		public byte byBrightnessLevel;

		public byte byContrastLevel;

		public byte bySharpnessLevel;

		public byte bySaturationLevel;

		public byte byHueLevel;

		public byte byEnableFunc;

		public byte byLightInhibitLevel;

		public byte byGrayLevel;
	}

	public struct NET_DVR_GAIN
	{
		public byte byGainLevel;

		public byte byGainUserSet;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public uint dwMaxGainValue;
	}

	public struct NET_DVR_WHITEBALANCE
	{
		public byte byWhiteBalanceMode;

		public byte byWhiteBalanceModeRGain;

		public byte byWhiteBalanceModeBGain;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_EXPOSURE
	{
		public byte byExposureMode;

		public byte byAutoApertureLevel;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public uint dwVideoExposureSet;

		public uint dwExposureUserSet;

		public uint dwRes;
	}

	public struct NET_DVR_WDR
	{
		public byte byWDREnabled;

		public byte byWDRLevel1;

		public byte byWDRLevel2;

		public byte byWDRContrastLevel;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_DAYNIGHT
	{
		public byte byDayNightFilterType;

		public byte bySwitchScheduleEnabled;

		public byte byBeginTime;

		public byte byEndTime;

		public byte byDayToNightFilterLevel;

		public byte byNightToDayFilterLevel;

		public byte byDayNightFilterTime;

		public byte byBeginTimeMin;

		public byte byBeginTimeSec;

		public byte byEndTimeMin;

		public byte byEndTimeSec;

		public byte byAlarmTrigState;
	}

	public struct NET_DVR_GAMMACORRECT
	{
		public byte byGammaCorrectionEnabled;

		public byte byGammaCorrectionLevel;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_BACKLIGHT
	{
		public byte byBacklightMode;

		public byte byBacklightLevel;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public uint dwPositionX1;

		public uint dwPositionY1;

		public uint dwPositionX2;

		public uint dwPositionY2;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_NOISEREMOVE
	{
		public byte byDigitalNoiseRemoveEnable;

		public byte byDigitalNoiseRemoveLevel;

		public byte bySpectralLevel;

		public byte byTemporalLevel;

		public byte byDigitalNoiseRemove2DEnable;

		public byte byDigitalNoiseRemove2DLevel;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_CMOSMODECFG
	{
		public byte byCaptureMod;

		public byte byBrightnessGate;

		public byte byCaptureGain1;

		public byte byCaptureGain2;

		public uint dwCaptureShutterSpeed1;

		public uint dwCaptureShutterSpeed2;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_CAMERAPARAMCFG
	{
		public uint dwSize;

		public NET_DVR_VIDEOEFFECT struVideoEffect;

		public NET_DVR_GAIN struGain;

		public NET_DVR_WHITEBALANCE struWhiteBalance;

		public NET_DVR_EXPOSURE struExposure;

		public NET_DVR_GAMMACORRECT struGammaCorrect;

		public NET_DVR_WDR struWdr;

		public NET_DVR_DAYNIGHT struDayNight;

		public NET_DVR_BACKLIGHT struBackLight;

		public NET_DVR_NOISEREMOVE struNoiseRemove;

		public byte byPowerLineFrequencyMode;

		public byte byIrisMode;

		public byte byMirror;

		public byte byDigitalZoom;

		public byte byDeadPixelDetect;

		public byte byBlackPwl;

		public byte byEptzGate;

		public byte byLocalOutputGate;

		public byte byCoderOutputMode;

		public byte byLineCoding;

		public byte byDimmerMode;

		public byte byPaletteMode;

		public byte byEnhancedMode;

		public byte byDynamicContrastEN;

		public byte byDynamicContrast;

		public byte byJPEGQuality;

		public NET_DVR_CMOSMODECFG struCmosModeCfg;

		public byte byFilterSwitch;

		public byte byFocusSpeed;

		public byte byAutoCompensationInterval;

		public byte bySceneMode;
	}

	public struct NET_DVR_DEFOGCFG
	{
		public byte byMode;

		public byte byLevel;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_ELECTRONICSTABILIZATION
	{
		public byte byEnable;

		public byte byLevel;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_CORRIDOR_MODE_CCD
	{
		public byte byEnableCorridorMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 11, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_SMARTIR_PARAM
	{
		public byte byMode;

		public byte byIRDistance;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_PIRIS_PARAM
	{
		public byte byMode;

		public byte byPIrisAperture;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_CAMERAPARAMCFG_EX
	{
		public uint dwSize;

		public NET_DVR_VIDEOEFFECT struVideoEffect;

		public NET_DVR_GAIN struGain;

		public NET_DVR_WHITEBALANCE struWhiteBalance;

		public NET_DVR_EXPOSURE struExposure;

		public NET_DVR_GAMMACORRECT struGammaCorrect;

		public NET_DVR_WDR struWdr;

		public NET_DVR_DAYNIGHT struDayNight;

		public NET_DVR_BACKLIGHT struBackLight;

		public NET_DVR_NOISEREMOVE struNoiseRemove;

		public byte byPowerLineFrequencyMode;

		public byte byIrisMode;

		public byte byMirror;

		public byte byDigitalZoom;

		public byte byDeadPixelDetect;

		public byte byBlackPwl;

		public byte byEptzGate;

		public byte byLocalOutputGate;

		public byte byCoderOutputMode;

		public byte byLineCoding;

		public byte byDimmerMode;

		public byte byPaletteMode;

		public byte byEnhancedMode;

		public byte byDynamicContrastEN;

		public byte byDynamicContrast;

		public byte byJPEGQuality;

		public NET_DVR_CMOSMODECFG struCmosModeCfg;

		public byte byFilterSwitch;

		public byte byFocusSpeed;

		public byte byAutoCompensationInterval;

		public byte bySceneMode;

		public NET_DVR_DEFOGCFG struDefogCfg;

		public NET_DVR_ELECTRONICSTABILIZATION struElectronicStabilization;

		public NET_DVR_CORRIDOR_MODE_CCD struCorridorMode;

		public byte byExposureSegmentEnable;

		public byte byBrightCompensate;

		public byte byCaptureModeN;

		public byte byCaptureModeP;

		public NET_DVR_SMARTIR_PARAM struSmartIRParam;

		public NET_DVR_PIRIS_PARAM struPIrisParam;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 296, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;
	}

	public enum VCA_PLATE_COLOR
	{
		VCA_BLUE_PLATE,
		VCA_YELLOW_PLATE,
		VCA_WHITE_PLATE,
		VCA_BLACK_PLATE,
		VCA_GREEN_PLATE
	}

	public enum VCA_PLATE_TYPE
	{
		VCA_STANDARD92_PLATE,
		VCA_STANDARD02_PLATE,
		VCA_WJPOLICE_PLATE,
		VCA_JINGCHE_PLATE,
		STANDARD92_BACK_PLATE,
		VCA_SHIGUAN_PLATE,
		VCA_NONGYONG_PLATE,
		VCA_MOTO_PLATE
	}

	public enum VLR_VEHICLE_CLASS
	{
		VLR_OTHER,
		VLR_VOLKSWAGEN,
		VLR_BUICK,
		VLR_BMW,
		VLR_HONDA,
		VLR_PEUGEOT,
		VLR_TOYOTA,
		VLR_FORD,
		VLR_NISSAN,
		VLR_AUDI,
		VLR_MAZDA,
		VLR_CHEVROLET,
		VLR_CITROEN,
		VLR_HYUNDAI,
		VLR_CHERY
	}

	public struct NET_DVR_VEHICLE_INFO
	{
		public uint dwIndex;

		public byte byVehicleType;

		public byte byColorDepth;

		public byte byColor;

		public byte byRes1;

		public ushort wSpeed;

		public ushort wLength;

		public byte byIllegalType;

		public byte byVehicleLogoRecog;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byCustomInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public void Init()
		{
			byRes2 = new byte[2];
			byCustomInfo = new byte[16];
			byRes = new byte[16];
		}
	}

	public struct NET_DVR_PLATE_RESULT
	{
		public uint dwSize;

		public byte byResultType;

		public byte byChanIndex;

		public ushort wAlarmRecordID;

		public uint dwRelativeTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byAbsTime;

		public uint dwPicLen;

		public uint dwPicPlateLen;

		public uint dwVideoLen;

		public byte byTrafficLight;

		public byte byPicNum;

		public byte byDriveChan;

		public byte byVehicleType;

		public uint dwBinPicLen;

		public uint dwCarPicLen;

		public uint dwFarCarPicLen;

		public IntPtr pBuffer3;

		public IntPtr pBuffer4;

		public IntPtr pBuffer5;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes3;

		public NET_DVR_PLATE_INFO struPlateInfo;

		public NET_DVR_VEHICLE_INFO struVehicleInfo;

		public IntPtr pBuffer1;

		public IntPtr pBuffer2;

		public void Init()
		{
			byAbsTime = new byte[32];
			byRes3 = new byte[8];
		}
	}

	public struct NET_DVR_IMAGEOVERLAYCFG
	{
		public uint dwSize;

		public byte byOverlayInfo;

		public byte byOverlayMonitorInfo;

		public byte byOverlayTime;

		public byte byOverlaySpeed;

		public byte byOverlaySpeeding;

		public byte byOverlayLimitFlag;

		public byte byOverlayPlate;

		public byte byOverlayColor;

		public byte byOverlayLength;

		public byte byOverlayType;

		public byte byOverlayColorDepth;

		public byte byOverlayDriveChan;

		public byte byOverlayMilliSec;

		public byte byOverlayIllegalInfo;

		public byte byOverlayRedOnTime;

		public byte byFarAddPlateJpeg;

		public byte byNearAddPlateJpeg;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byMonitorInfo1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 44, ArraySubType = UnmanagedType.I1)]
		public byte[] byMonitorInfo2;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 52, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_SNAPCFG
	{
		public uint dwSize;

		public byte byRelatedDriveWay;

		public byte bySnapTimes;

		public ushort wSnapWaitTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U2)]
		public ushort[] wIntervalTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public enum ITC_MAINMODE_ABILITY
	{
		ITC_MODE_UNKNOW = 0,
		ITC_POST_MODE = 1,
		ITC_EPOLICE_MODE = 2,
		ITC_POSTEPOLICE_MODE = 4
	}

	public enum ITC_RECOG_REGION_TYPE
	{
		ITC_REGION_RECT,
		ITC_REGION_POLYGON
	}

	public struct NET_DVR_SNAP_ABILITY
	{
		public uint dwSize;

		public byte byIoInNum;

		public byte byIoOutNum;

		public byte bySingleSnapNum;

		public byte byLightModeArrayNum;

		public byte byMeasureModeArrayNum;

		public byte byPlateEnable;

		public byte byLensMode;

		public byte byPreTriggerSupport;

		public uint dwAbilityType;

		public byte byIoSpeedGroup;

		public byte byIoLightGroup;

		public byte byRecogRegionType;

		public byte bySupport;

		public ushort wSupportMultiRadar;

		public byte byICRPresetNum;

		public byte byICRTimeSlot;

		public byte bySupportRS485Num;

		public byte byExpandRs485SupportSensor;

		public byte byExpandRs485SupportSignalLampDet;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 13, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_ITC_ICRTIMECFG
	{
		public NET_DVR_SCHEDTIME struTime;

		public byte byAssociateRresetNo;

		public byte bySubSwitchMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_ITC_ICR_TIMESWITCH_PARAM
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
		public NET_ITC_ICRTIMECFG[] struAutoCtrlTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] byICRPreset;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_ITC_ICR_MANUALSWITCH_PARAM
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] byICRPreset;

		public byte bySubSwitchMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 147, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_ITC_ICR_AOTOSWITCH_PARAM
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] byICRPreset;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 148, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_ITC_ICR_PARAM_UNION
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 156, ArraySubType = UnmanagedType.I1)]
		public byte[] uLen;

		public NET_ITC_ICR_AOTOSWITCH_PARAM struICRAutoSwitch;

		public NET_ITC_ICR_MANUALSWITCH_PARAM struICRManualSwitch;

		public NET_ITC_ICR_TIMESWITCH_PARAM struICRTimeSwitch;
	}

	public struct NET_ITC_ICRCFG
	{
		public uint dwSize;

		public byte bySwitchType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public NET_ITC_ICR_PARAM_UNION uICRParam;
	}

	public struct NET_ITC_HANDLEEXCEPTION
	{
		public uint dwHandleType;

		public byte byEnable;

		public byte byRes;

		public ushort wDuration;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byAlarmOutTriggered;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;
	}

	public struct NET_ITC_EXCEPTION
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
		public NET_ITC_HANDLEEXCEPTION[] struSnapExceptionType;
	}

	public struct NET_DVR_TRIGCOORDINATE
	{
		public ushort wTopLeftX;

		public ushort wTopLeftY;

		public ushort wWdith;

		public ushort wHeight;
	}

	public enum PROVINCE_CITY_IDX
	{
		ANHUI_PROVINCE,
		AOMEN_PROVINCE,
		BEIJING_PROVINCE,
		CHONGQING_PROVINCE,
		FUJIAN_PROVINCE,
		GANSU_PROVINCE,
		GUANGDONG_PROVINCE,
		GUANGXI_PROVINCE,
		GUIZHOU_PROVINCE,
		HAINAN_PROVINCE,
		HEBEI_PROVINCE,
		HENAN_PROVINCE,
		HEILONGJIANG_PROVINCE,
		HUBEI_PROVINCE,
		HUNAN_PROVINCE,
		JILIN_PROVINCE,
		JIANGSU_PROVINCE,
		JIANGXI_PROVINCE,
		LIAONING_PROVINCE,
		NEIMENGGU_PROVINCE,
		NINGXIA_PROVINCE,
		QINGHAI_PROVINCE,
		SHANDONG_PROVINCE,
		SHANXI_JIN_PROVINCE,
		SHANXI_SHAN_PROVINCE,
		SHANGHAI_PROVINCE,
		SICHUAN_PROVINCE,
		TAIWAN_PROVINCE,
		TIANJIN_PROVINCE,
		XIZANG_PROVINCE,
		XIANGGANG_PROVINCE,
		XINJIANG_PROVINCE,
		YUNNAN_PROVINCE,
		ZHEJIANG_PROVINCE
	}

	public struct NET_DVR_GEOGLOCATION
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
		public int[] iRes;

		public uint dwCity;
	}

	public enum SCENE_MODE
	{
		UNKOWN_SCENE_MODE,
		HIGHWAY_SCENE_MODE,
		SUBURBAN_SCENE_MODE,
		URBAN_SCENE_MODE,
		TUNNEL_SCENE_MODE
	}

	public struct NET_DVR_VTPARAM
	{
		public uint dwSize;

		public byte byEnable;

		public byte byIsDisplay;

		public byte byLoopPos;

		public byte bySnapGain;

		public uint dwSnapShutter;

		public NET_DVR_TRIGCOORDINATE struTrigCoordinate;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_TRIGCOORDINATE[] struRes;

		public byte byTotalLaneNum;

		public byte byPolarLenType;

		public byte byDayAuxLightMode;

		public byte byLoopToCalRoadBright;

		public byte byRoadGrayLowTh;

		public byte byRoadGrayHighTh;

		public ushort wLoopPosBias;

		public uint dwHfrShtterInitValue;

		public uint dwSnapShtterInitValue;

		public uint dwHfrShtterMaxValue;

		public uint dwSnapShtterMaxValue;

		public uint dwHfrShtterNightValue;

		public uint dwSnapShtterNightMinValue;

		public uint dwSnapShtterNightMaxValue;

		public uint dwInitAfe;

		public uint dwMaxAfe;

		public ushort wResolutionX;

		public ushort wResolutionY;

		public uint dwGainNightValue;

		public uint dwSceneMode;

		public uint dwRecordMode;

		public NET_DVR_GEOGLOCATION struGeogLocation;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.I1)]
		public byte[] byTrigFlag;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.I1)]
		public byte[] byTrigSensitive;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 62, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_SNAPENABLECFG
	{
		public uint dwSize;

		public byte byPlateEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public byte byFrameFlip;

		public ushort wFlipAngle;

		public ushort wLightPhase;

		public byte byLightSyncPower;

		public byte byFrequency;

		public byte byUploadSDEnable;

		public byte byPlateMode;

		public byte byUploadInfoFTP;

		public byte byAutoFormatSD;

		public ushort wJpegPicSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 56, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_FTPCFG
	{
		public uint dwSize;

		public uint dwEnableFTP;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sFTPIP;

		public uint dwFTPPort;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sUserName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] sPassword;

		public uint dwDirLevel;

		public ushort wTopDirMode;

		public ushort wSubDirMode;

		public byte byEnableAnony;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 23, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_PICTURE_NAME
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 15, ArraySubType = UnmanagedType.I1)]
		public byte[] byItemOrder;

		public byte byDelimiter;
	}

	public struct NET_DVR_PICTURE_NAME_EX
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 15, ArraySubType = UnmanagedType.I1)]
		public byte[] byItemOrder;

		public byte byDelimiter;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_SERIAL_CATCHPIC_PARA
	{
		public byte byStrFlag;

		public byte byEndFlag;

		public ushort wCardIdx;

		public uint dwCardLen;

		public uint dwTriggerPicChans;
	}

	public struct NET_DVR_JPEGCFG_V30
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_JPEGPARA[] struJpegPara;

		public ushort wBurstMode;

		public ushort wUploadInterval;

		public NET_DVR_PICTURE_NAME struPicNameRule;

		public byte bySaveToHD;

		public byte byRes1;

		public ushort wCatchInterval;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;

		public NET_DVR_SERIAL_CATCHPIC_PARA struRs232Cfg;

		public NET_DVR_SERIAL_CATCHPIC_PARA struRs485Cfg;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.U4)]
		public uint[] dwTriggerPicTimes;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 160, ArraySubType = UnmanagedType.U4)]
		public uint[] dwAlarmInPicChanTriggered;
	}

	public struct NET_DVR_MANUALSNAP
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_SPRCFG
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byDefaultCHN;

		public byte byPlateOSD;

		public byte bySendJPEG1;

		public byte bySendJPEG2;

		public ushort wDesignedPlateWidth;

		public byte byTotalLaneNum;

		public byte byRes1;

		public ushort wRecognizedLane;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.Struct)]
		public NET_VCA_RECT[] struLaneRect;

		public uint dwRecogMode;

		public byte bySendPRRaw;

		public byte bySendBinImage;

		public byte byDelayCapture;

		public byte byUseLED;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 68, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_PLCCFG
	{
		public uint dwSize;

		public byte byPlcEnable;

		public byte byPlateExpectedBright;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public byte byTradeoffFlash;

		public byte byCorrectFactor;

		public ushort wLoopStatsEn;

		public byte byPlcBrightOffset;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 19, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_DEVICESTATECFG
	{
		public uint dwSize;

		public ushort wPreviewNum;

		public ushort wFortifyLinkNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_IPADDR[] struPreviewIP;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_IPADDR[] struFortifyIP;

		public uint dwVideoFrameRate;

		public byte byResolution;

		public byte bySnapResolution;

		public byte byStreamType;

		public byte byTriggerType;

		public uint dwSDVolume;

		public uint dwSDFreeSpace;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.I1)]
		public byte[] byDetectorState;

		public byte byDetectorLinkState;

		public byte bySDStatus;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.I1)]
		public byte[] byFortifyLevel;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 116, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_POSTEPOLICECFG
	{
		public uint dwSize;

		public uint dwDistance;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.U4)]
		public uint[] dwLightChan;

		public byte byCapSpeed;

		public byte bySpeedLimit;

		public byte byTrafficDirection;

		public byte byRes1;

		public ushort wLoopPreDist;

		public ushort wTrigDelay;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 124, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_PROTO_TYPE
	{
		public uint dwType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byDescribe;
	}

	public struct NET_DVR_IPC_PROTO_LIST
	{
		public uint dwSize;

		public uint dwProtoNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_PROTO_TYPE[] struProto;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_TRAVERSE_PLANE_SEARCHCOND
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
		public NET_VCA_TRAVERSE_PLANE[] struVcaTraversePlane;

		public uint dwPreTime;

		public uint dwDelayTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5656, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_INTRUSION_SEARCHCOND
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
		public NET_VCA_INTRUSION[] struVcaIntrusion;

		public uint dwPreTime;

		public uint dwDelayTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5400, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct NET_DVR_AREA_SMARTSEARCH_COND_UNION
	{
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6144, ArraySubType = UnmanagedType.I1)]
		public byte[] byLen;

		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6144, ArraySubType = UnmanagedType.I1)]
		public byte[] byMotionScope;

		[FieldOffset(0)]
		public NET_DVR_TRAVERSE_PLANE_SEARCHCOND struTraversPlaneCond;

		[FieldOffset(0)]
		public NET_DVR_INTRUSION_SEARCHCOND struIntrusionCond;
	}

	public struct NET_DVR_SMART_SEARCH_PARAM
	{
		public byte byChan;

		public byte bySearchCondType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_DVR_TIME struStartTime;

		public NET_DVR_TIME struEndTime;

		public NET_DVR_AREA_SMARTSEARCH_COND_UNION uSmartSearchCond;

		public byte bySensitivity;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 11, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_SMART_SEARCH_RET
	{
		public NET_DVR_TIME struStartTime;

		public NET_DVR_TIME struEndTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_IPSAN_SERACH_PARAM
	{
		public NET_DVR_IPADDR struIP;

		public ushort wPort;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_IPSAN_SERACH_RET
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
		public byte[] byDirectory;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_DEVICECFG_V40
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sDVRName;

		public uint dwDVRID;

		public uint dwRecycleRecord;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.I1)]
		public byte[] sSerialNumber;

		public uint dwSoftwareVersion;

		public uint dwSoftwareBuildDate;

		public uint dwDSPSoftwareVersion;

		public uint dwDSPSoftwareBuildDate;

		public uint dwPanelVersion;

		public uint dwHardwareVersion;

		public byte byAlarmInPortNum;

		public byte byAlarmOutPortNum;

		public byte byRS232Num;

		public byte byRS485Num;

		public byte byNetworkPortNum;

		public byte byDiskCtrlNum;

		public byte byDiskNum;

		public byte byDVRType;

		public byte byChanNum;

		public byte byStartChan;

		public byte byDecordChans;

		public byte byVGANum;

		public byte byUSBNum;

		public byte byAuxoutNum;

		public byte byAudioNum;

		public byte byIPChanNum;

		public byte byZeroChanNum;

		public byte bySupport;

		public byte byEsataUseage;

		public byte byIPCPlug;

		public byte byStorageMode;

		public byte bySupport1;

		public ushort wDevType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24, ArraySubType = UnmanagedType.I1)]
		public byte[] byDevTypeName;

		public byte bySupport2;

		public byte byAnalogAlarmInPortNum;

		public byte byStartAlarmInNo;

		public byte byStartAlarmOutNo;

		public byte byStartIPAlarmInNo;

		public byte byStartIPAlarmOutNo;

		public byte byHighIPChanNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_ZEROCHANCFG
	{
		public uint dwSize;

		public byte byEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public uint dwVideoBitrate;

		public uint dwVideoFrameRate;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_ZERO_ZOOMCFG
	{
		public uint dwSize;

		public NET_VCA_POINT struPoint;

		public byte byState;

		public byte byPreviewNumber;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byPreviewSeq;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 30, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_SNMPCFG
	{
		public uint dwSize;

		public byte byEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public ushort wVersion;

		public ushort wServerPort;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byReadCommunity;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byWriteCommunity;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byTrapHostIP;

		public ushort wTrapHostPort;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byTrapName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 70, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_SNMPv3_USER
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byUserName;

		public byte bySecLevel;

		public byte byAuthtype;

		public byte byPrivtype;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byAuthpass;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byPrivpass;
	}

	public struct NET_DVR_SNMPCFG_V30
	{
		public uint dwSize;

		public byte byEnableV1;

		public byte byEnableV2;

		public byte byEnableV3;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public ushort wServerPort;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byReadCommunity;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byWriteCommunity;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byTrapHostIP;

		public ushort wTrapHostPort;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;

		public NET_DVR_SNMPv3_USER struRWUser;

		public NET_DVR_SNMPv3_USER struROUser;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byTrapName;
	}

	public struct NET_DVR_SADPINFO
	{
		public NET_DVR_IPADDR struIP;

		public ushort wPort;

		public ushort wFactoryType;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
		public string chSoftwareVersion;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string chSerialNo;

		public ushort wEncCnt;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
		public byte[] byMACAddr;

		public NET_DVR_IPADDR struSubDVRIPMask;

		public NET_DVR_IPADDR struGatewayIpAddr;

		public NET_DVR_IPADDR struDnsServer1IpAddr;

		public NET_DVR_IPADDR struDnsServer2IpAddr;

		public byte byDns;

		public byte byDhcp;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 158, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_SADPINFO_LIST
	{
		public uint dwSize;

		public ushort wSadpNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SADPINFO[] struSadpInfo;
	}

	public struct NET_DVR_SADP_VERIFY
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string chPassword;

		public NET_DVR_IPADDR struOldIP;

		public ushort wOldPort;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 62, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_DESC_NODE
	{
		public int iValue;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byDescribe;

		public uint dwFreeSpace;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_DISKABILITY_LIST
	{
		public uint dwSize;

		public uint dwNodeNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_DESC_NODE[] struDescNode;
	}

	public struct NET_DVR_BACKUP_NAME_PARAM
	{
		public uint dwFileNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_FINDDATA_V30[] struFileList;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byDiskDes;

		public byte byWithPlayer;

		public byte byContinue;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 34, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_BACKUP_TIME_PARAM
	{
		public int lChannel;

		public NET_DVR_TIME struStartTime;

		public NET_DVR_TIME struStopTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byDiskDes;

		public byte byWithPlayer;

		public byte byContinue;

		public byte byDrawFrame;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 33, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public enum COMPRESSION_ABILITY_TYPE
	{
		COMPRESSION_STREAM_ABILITY,
		MAIN_RESOLUTION_ABILITY,
		SUB_RESOLUTION_ABILITY,
		EVENT_RESOLUTION_ABILITY,
		FRAME_ABILITY,
		BITRATE_TYPE_ABILITY,
		BITRATE_ABILITY,
		THIRD_RESOLUTION_ABILITY,
		STREAM_TYPE_ABILITY,
		PIC_QUALITY_ABILITY,
		INTERVAL_BPFRAME_ABILITY,
		VIDEO_ENC_ABILITY,
		AUDIO_ENC_ABILITY,
		VIDEO_ENC_COMPLEXITY_ABILITY,
		FORMAT_ABILITY
	}

	public struct NET_DVR_ABILITY_LIST
	{
		public uint dwAbilityType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public uint dwNodeNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_DESC_NODE[] struDescNode;
	}

	public struct NET_DVR_COMPRESSIONCFG_ABILITY
	{
		public uint dwSize;

		public uint dwAbilityNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_ABILITY_LIST[] struAbilityNode;
	}

	public struct NET_DVR_HOLIDATE_MODEA
	{
		public byte byStartMonth;

		public byte byStartDay;

		public byte byEndMonth;

		public byte byEndDay;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_HOLIDATE_MODEB
	{
		public byte byStartMonth;

		public byte byStartWeekNum;

		public byte byStartWeekday;

		public byte byEndMonth;

		public byte byEndWeekNum;

		public byte byEndWeekday;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_HOLIDATE_MODEC
	{
		public ushort wStartYear;

		public byte byStartMon;

		public byte byStartDay;

		public ushort wEndYear;

		public byte byEndMon;

		public byte byEndDay;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct NET_DVR_HOLIDATE_UNION
	{
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U4)]
		public uint[] dwSize;

		[FieldOffset(0)]
		public NET_DVR_HOLIDATE_MODEA struModeA;

		[FieldOffset(0)]
		public NET_DVR_HOLIDATE_MODEB struModeB;

		[FieldOffset(0)]
		public NET_DVR_HOLIDATE_MODEC struModeC;
	}

	public enum HOLI_DATE_MODE
	{
		HOLIDATE_MODEA,
		HOLIDATE_MODEB,
		HOLIDATE_MODEC
	}

	public struct NET_DVR_HOLIDAY_PARAM
	{
		public byte byEnable;

		public byte byDateMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_DVR_HOLIDATE_UNION uHolidate;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_HOLIDAY_PARAM_CFG
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_HOLIDAY_PARAM[] struHolidayParam;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 40, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_HOLIDAY_HANDLE
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SCHEDTIME[] struAlarmTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 240, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_HOLIDAY_RECORD
	{
		public uint dwSize;

		public NET_DVR_RECORDDAY struRecDay;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_RECORDSCHED[] struRecordSched;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_ONE_LINK
	{
		public NET_DVR_IPADDR struIP;

		public int lChannel;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_LINK_STATUS
	{
		public uint dwSize;

		public ushort wLinkNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_ONE_LINK[] struOneLink;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_ONE_BONDING
	{
		public byte byMode;

		public byte byUseDhcp;

		public byte byMasterCard;

		public byte byStatus;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
		public byte[] byBond;

		public NET_DVR_ETHERNET_V30 struEtherNet;

		public NET_DVR_IPADDR struGatewayIpAddr;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_NETWORK_BONDING
	{
		public uint dwSize;

		public byte byEnable;

		public byte byNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_ONE_BONDING[] struOneBond;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 40, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_DISK_QUOTA
	{
		public byte byQuotaType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public uint dwHCapacity;

		public uint dwLCapacity;

		public uint dwHUsedSpace;

		public uint dwLUsedSpace;

		public byte byQuotaRatio;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 21, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_DISK_QUOTA_CFG
	{
		public uint dwSize;

		public NET_DVR_DISK_QUOTA struPicQuota;

		public NET_DVR_DISK_QUOTA struRecordQuota;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 60, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_TIMING_CAPTURE
	{
		public NET_DVR_JPEGPARA struJpegPara;

		public uint dwPicInterval;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_REL_CAPTURE_CHAN
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byChan;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_REL_CAPTURE_CHAN_V40
	{
		public uint dwMaxRelCaptureChanNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.U4)]
		public uint[] dwChanNo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_EVENT_CAPTURE_V40
	{
		public NET_DVR_JPEGPARA struJpegPara;

		public uint dwPicInterval;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_REL_CAPTURE_CHAN_V40[] struRelCaptureChan;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_REL_CAPTURE_CHAN_V40[] struAlarmInCapture;

		public uint dwMaxGroupNum;

		public byte byCapTimes;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 59, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_EVENT_CAPTURE
	{
		public NET_DVR_JPEGPARA struJpegPara;

		public uint dwPicInterval;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_REL_CAPTURE_CHAN[] struRelCaptureChan;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_REL_CAPTURE_CHAN[] struAlarmInCapture;

		public byte byCapTimes;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 59, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_JPEG_CAPTURE_CFG_V40
	{
		public uint dwSize;

		public NET_DVR_TIMING_CAPTURE struTimingCapture;

		public NET_DVR_EVENT_CAPTURE_V40 struEventCapture;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes3;
	}

	public struct NET_DVR_JPEG_CAPTURE_CFG
	{
		public uint dwSize;

		public NET_DVR_TIMING_CAPTURE struTimingCapture;

		public NET_DVR_EVENT_CAPTURE struEventCapture;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes3;
	}

	public struct NET_DVR_CAPTURE_DAY
	{
		public byte byAllDayCapture;

		public byte byCaptureType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_CAPTURE_SCHED
	{
		public NET_DVR_SCHEDTIME struCaptureTime;

		public byte byCaptureType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_SCHED_CAPTURECFG
	{
		public uint dwSize;

		public byte byEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_CAPTURE_DAY[] struCaptureDay;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 56, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_CAPTURE_SCHED[] struCaptureSched;

		public NET_DVR_CAPTURE_DAY struCaptureHoliday;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_CAPTURE_SCHED[] struHolidaySched;

		public uint dwRecorderDuration;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 40, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_FLOW_TEST_PARAM
	{
		public uint dwSize;

		public int lCardIndex;

		public uint dwInterval;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_FLOW_INFO
	{
		public uint dwSize;

		public uint dwSendFlowSize;

		public uint dwRecvFlowSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_RECORD_LABEL
	{
		public uint dwSize;

		public NET_DVR_TIME struTimeLabel;

		public byte byQuickAdd;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 40, ArraySubType = UnmanagedType.I1)]
		public byte[] sLabelName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 40, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_LABEL_IDENTIFY
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] sLabelIdentify;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_DEL_LABEL_PARAM
	{
		public uint dwSize;

		public byte byMode;

		public byte byRes1;

		public ushort wLabelNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_LABEL_IDENTIFY[] struIndentify;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 160, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_MOD_LABEL_PARAM
	{
		public NET_DVR_LABEL_IDENTIFY struIndentify;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 40, ArraySubType = UnmanagedType.I1)]
		public byte[] sLabelName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 40, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_FIND_LABEL
	{
		public uint dwSize;

		public int lChannel;

		public NET_DVR_TIME struStartTime;

		public NET_DVR_TIME struStopTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 40, ArraySubType = UnmanagedType.I1)]
		public byte[] sLabelName;

		public byte byDrawFrame;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 39, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_FINDLABEL_DATA
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 40, ArraySubType = UnmanagedType.I1)]
		public byte[] sLabelName;

		public NET_DVR_TIME struTimeLabel;

		public NET_DVR_LABEL_IDENTIFY struLabelIdentify;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;
	}

	public struct NET_DVR_FIND_PICTURE_PARAM
	{
		public uint dwSize;

		public int lChannel;

		public byte byFileType;

		public byte byNeedCard;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 40, ArraySubType = UnmanagedType.I1)]
		public byte[] sCardNum;

		public NET_DVR_TIME struStartTime;

		public NET_DVR_TIME struStopTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 40, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_FIND_PICTURE
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string sFileName;

		public NET_DVR_TIME struTime;

		public uint dwFileSize;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
		public string sCardNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_BACKUP_PICTURE_PARAM
	{
		public uint dwSize;

		public uint dwPicNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_FIND_PICTURE[] struPicture;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byDiskDes;

		public byte byWithPlayer;

		public byte byContinue;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 34, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_COMPRESSION_LIMIT
	{
		public uint dwSize;

		public uint dwChannel;

		public byte byCompressType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 15, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public NET_DVR_COMPRESSIONCFG_V30 struCurrentCfg;
	}

	public struct NET_DVR_VIDEO_EFFECT
	{
		public uint dwBrightValue;

		public uint dwContrastValue;

		public uint dwSaturationValue;

		public uint dwHueValue;

		public uint dwSharpness;

		public uint dwDenoising;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_VIDEO_INPUT_EFFECT
	{
		public uint dwSize;

		public ushort wEffectMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 146, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_DVR_VIDEO_EFFECT struVideoEffect;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 60, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_VIDEOPARA_V40
	{
		public uint dwChannel;

		public uint dwVideoParamType;

		public uint dwVideoParamValue;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_DEFAULT_VIDEO_COND
	{
		public uint dwSize;

		public uint dwChannel;

		public uint dwVideoMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_ENCODE_JOINT_PARAM
	{
		public uint dwSize;

		public byte byJointed;

		public byte byDevType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_DVR_IPADDR struIP;

		public ushort wPort;

		public ushort wChannel;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_VCA_CHAN_WORKSTATUS
	{
		public byte byJointed;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_DVR_IPADDR struIP;

		public ushort wPort;

		public ushort wChannel;

		public byte byVcaChanStatus;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 19, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_VCA_DEV_WORKSTATUS
	{
		public uint dwSize;

		public byte byDeviceStatus;

		public byte byCpuLoad;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_VCA_CHAN_WORKSTATUS[] struVcaChanStatus;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 40, ArraySubType = UnmanagedType.U4)]
		public uint[] dwRes;
	}

	public struct UNION_VIDEOPLATFORM_V40
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byJoinDecoderId;

		public byte byDecResolution;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 143, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct UNION_NOTVIDEOPLATFORM_V40
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 160, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_VGA_DISP_CHAN_CFG_V40
	{
		[StructLayout(LayoutKind.Explicit)]
		public struct struDiff
		{
			[FieldOffset(0)]
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 160, ArraySubType = UnmanagedType.I1)]
			public byte[] byRes;

			[FieldOffset(0)]
			public UNION_VIDEOPLATFORM_V40 struVideoPlatform;

			[FieldOffset(0)]
			public UNION_NOTVIDEOPLATFORM_V40 struNotVideoPlatform;
		}

		public uint dwSize;

		public byte byAudio;

		public byte byAudioWindowIdx;

		public byte byVgaResolution;

		public byte byVedioFormat;

		public uint dwWindowMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byJoinDecChan;

		public byte byEnlargeStatus;

		public byte byEnlargeSubWindowIndex;

		public byte byScale;

		public byte byUnionType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 120, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_V6SUBSYSTEMPARAM
	{
		public byte bySerialTrans;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 35, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_CORRECT_DEADPIXEL_PARAM
	{
		public uint dwSize;

		public uint dwCommand;

		public uint dwDeadPixelX;

		public uint dwDeadPixelY;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_REDAREACFG
	{
		public uint dwSize;

		public uint dwCorrectEnable;

		public uint dwCorrectLevel;

		public uint dwAreaNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.Struct)]
		public NET_VCA_RECT[] struLaneRect;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_HISTORICDATACFG
	{
		public uint dwSize;

		public uint dwTotalNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_INQUEST_ROOM
	{
		public byte byRoomIndex;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 23, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_INQUEST_MESSAGE
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 44)]
		public string sMessage;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 46, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_INQUEST_SENSOR_DEVICE
	{
		public ushort wDeviceType;

		public ushort wDeviceAddr;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 28, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_INQUEST_SENSOR_INFO
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_INQUEST_SENSOR_DEVICE[] struSensorDevice;

		public uint dwSupportPro;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 120, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_INQUEST_ROOM_INFO
	{
		[StructLayout(LayoutKind.Explicit)]
		public struct uCalcMode
		{
			[FieldOffset(0)]
			public byte byBitRate;

			[FieldOffset(0)]
			public byte byInquestTime;
		}

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string szCDName;

		public byte byCalcType;

		public byte byAutoDelRecord;

		public byte byAlarmThreshold;

		public byte byInquestChannelResolution;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 11, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_INQUEST_SYSTEM_INFO
	{
		public uint dwRecordMode;

		public uint dwWorkMode;

		public uint dwResolutionMode;

		public NET_DVR_INQUEST_SENSOR_INFO struSensorInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_INQUEST_ROOM_INFO[] struInquestRoomInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_INQUEST_RESUME_SEGMENT
	{
		public NET_DVR_TIME struStartTime;

		public NET_DVR_TIME struStopTime;

		public byte byRoomIndex;

		public byte byDriveIndex;

		public ushort wSegmetSize;

		public uint dwSegmentNo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_INQUEST_RESUME_EVENT
	{
		public uint dwResumeNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_INQUEST_RESUME_SEGMENT[] struResumeSegment;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 200, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_INQUEST_DEVICE_VERSION
	{
		public byte byMainVersion;

		public byte bySubVersion;

		public byte byUpgradeVersion;

		public byte byCustomizeVersion;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 60, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_DISK_RAID_INFO
	{
		public uint dwSize;

		public byte byEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 35, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_SYNCHRONOUS_IPC
	{
		public uint dwSize;

		public byte byEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_IPC_PASSWD
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sOldPasswd;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sNewPasswd;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DEVICE_NET_USING_INFO
	{
		public uint dwSize;

		public uint dwPreview;

		public uint dwPlayback;

		public uint dwIPCModule;

		public uint dwNetDiskRW;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] res;
	}

	public struct NET_DVR_IPC_NETCFG
	{
		public uint dwSize;

		public NET_DVR_IPADDR struIP;

		public ushort wPort;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 126)]
		public string res;
	}

	public struct NET_DVR_TIME_LOCK
	{
		public uint dwSize;

		public NET_DVR_TIME strBeginTime;

		public NET_DVR_TIME strEndTime;

		public uint dwChannel;

		public uint dwRecordType;

		public uint dwLockDuration;

		public NET_DVR_TIME_EX strUnlockTimePoint;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_LOCK_RETURN
	{
		public uint dwSize;

		public NET_DVR_TIME strBeginTime;

		public NET_DVR_TIME strEndTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public enum NET_SDK_UPLOAD_TYPE
	{
		UPGRADE_CERT_FILE,
		UPLOAD_CERT_FILE,
		TRIAL_CERT_FILE,
		CONFIGURATION_FILE
	}

	public enum NET_SDK_DOWNLOAD_TYPE
	{
		NET_SDK_DOWNLOAD_CERT,
		NET_SDK_DOWNLOAD_IPC_CFG_FILE,
		NET_SDK_DOWNLOAD_BASELINE_SCENE_PIC,
		NET_SDK_DOWNLOAD_VQD_ALARM_PIC,
		NET_SDK_DOWNLOAD_CONFIGURATION_FILE
	}

	public enum NET_SDK_DOWNLOAD_STATUS
	{
		NET_SDK_DOWNLOAD_STATUS_SUCCESS = 1,
		NET_SDK_DOWNLOAD_STATUS_PROCESSING,
		NET_SDK_DOWNLOAD_STATUS_FAILED,
		NET_SDK_DOWNLOAD_STATUS_UNKOWN_ERROR
	}

	public struct NET_DVR_BONJOUR_CFG
	{
		public uint dwSize;

		public byte byEnableBonjour;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byFriendlyName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_SOCKS_CFG
	{
		public uint dwSize;

		public byte byEnableSocks;

		public byte byVersion;

		public ushort wProxyPort;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byProxyaddr;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byUserName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byPassword;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 96, ArraySubType = UnmanagedType.I1)]
		public byte[] byLocalAddr;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_QOS_CFG
	{
		public uint dwSize;

		public byte byManageDscp;

		public byte byAlarmDscp;

		public byte byVideoDscp;

		public byte byAudioDscp;

		public byte byFlag;

		public byte byEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 126, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_HTTPS_CFG
	{
		public uint dwSize;

		public ushort wHttpsPort;

		public byte byEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 125, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_CERT_NAME
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
		public byte[] byCountry;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byState;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byLocality;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byOrganization;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byUnit;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byCommonName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byEmail;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_CERT_PARAM
	{
		public uint dwSize;

		public ushort wCertFunc;

		public ushort wCertType;

		public byte byFileType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 35, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_CERT_INFO
	{
		public uint dwSize;

		public NET_DVR_CERT_PARAM struCertParam;

		public uint dwValidDays;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byPasswd;

		public NET_DVR_CERT_NAME struCertName;

		public NET_DVR_CERT_NAME struIssuerName;

		public NET_DVR_TIME_EX struBeginTime;

		public NET_DVR_TIME_EX struEndTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] serialNumber;

		public byte byVersion;

		public byte byKeyAlgorithm;

		public byte byKeyLen;

		public byte bySignatureAlgorithm;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_CHANS_RECORD_STATUS
	{
		public byte byValid;

		public byte byRecord;

		public ushort wChannelNO;

		public uint dwRelatedHD;

		public byte byOffLineRecord;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_IP_ALARM_GROUP_NUM
	{
		public uint dwSize;

		public uint dwIPAlarmInGroup;

		public uint dwIPAlarmInNum;

		public uint dwIPAlarmOutGroup;

		public uint dwIPAlarmOutNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_CHAN_GROUP_RECORD_STATUS
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_CHANS_RECORD_STATUS[] struChanStatus;
	}

	public struct NET_DVR_RECTCFG
	{
		public ushort wXCoordinate;

		public ushort wYCoordinate;

		public ushort wWidth;

		public ushort wHeight;
	}

	public struct NET_DVR_WINCFG
	{
		public uint dwSize;

		public byte byVaild;

		public byte byInputIdx;

		public byte byLayerIdx;

		public byte byTransparency;

		public NET_DVR_RECTCFG struWin;

		public ushort wScreenHeight;

		public ushort wScreenWidth;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_ALLWINCFG
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_WINCFG[] struWinCfg;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_SCREENZOOM
	{
		public uint dwSize;

		public uint dwScreenNum;

		public NET_DVR_POINT_FRAME struPointFrame;

		public byte byLayer;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 11, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_MATRIX_CAMERAINFO
	{
		public uint dwGlobalCamId;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sCamName;

		public uint dwMatrixId;

		public uint dwLocCamId;

		public byte byValid;

		public byte byPtzCtrl;

		public byte byUseType;

		public byte byUsedByTrunk;

		public byte byTrunkReq;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_DVR_TIME struInstallTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sPurpose;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_MATRIX_MONITORINFO
	{
		public uint dwGloalMonId;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sMonName;

		public uint dwMatrixId;

		public uint dwLocalMonId;

		public byte byValid;

		public byte byTrunkType;

		public byte byUsedByTrunk;

		public byte byTrunkReq;

		public NET_DVR_TIME struInstallTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sPurpose;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_MATRIX_DIGITALMATRIX
	{
		public NET_DVR_IPADDR struAddress;

		public ushort wPort;

		public byte byNicNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 69, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_MATRIX_ANALOGMATRIX
	{
		public byte bySerPortNum;

		public byte byMatrixSerPortType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_DVR_SINGLE_RS232 struRS232;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 200, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct NET_MATRIX_UNION
	{
		[FieldOffset(0)]
		public NET_MATRIX_DIGITALMATRIX struDigitalMatrix;

		[FieldOffset(0)]
		public NET_MATRIX_ANALOGMATRIX struAnalogMatrix;
	}

	public struct NET_MATRIX_MATRIXINFO
	{
		public uint dwSize;

		public uint dwMatrixId;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sDevName;

		public byte byCtrlType;

		public byte byProtocolType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_MATRIX_UNION struMatrixUnion;

		public uint dwMaxPortsIn;

		public uint dwMaxPortsOut;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sUserName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] sPassword;

		public NET_DVR_TIME struInstallTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sPurpose;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_MATRIXLIST
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public uint dwMatrixNum;

		public IntPtr pBuffer;

		public uint dwBufLen;
	}

	public struct NET_MATRIX_UARTPARAM
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byPortName;

		public ushort wUserId;

		public byte byPortType;

		public byte byFuncType;

		public byte byProtocolType;

		public byte byBaudRate;

		public byte byDataBits;

		public byte byStopBits;

		public byte byParity;

		public byte byFlowCtrl;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 22, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_MATRIX_USERPARAM
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sUserName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] sPassword;

		public byte byRole;

		public byte byLevel;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 18, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_MATRIX_RESOURSEGROUPPARAM
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byGroupName;

		public byte byGroupType;

		public byte byRes1;

		public ushort wMemNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.U4)]
		public uint[] dwGlobalId;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_MATRIX_USERGROUPPARAM
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sGroupName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 255, ArraySubType = UnmanagedType.U2)]
		public ushort[] wUserMember;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 255, ArraySubType = UnmanagedType.U2)]
		public ushort[] wResorceGroupMember;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byPermission;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_MATRIX_TRUNKPARAM
	{
		public uint dwSize;

		public uint dwTrunkId;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sTrunkName;

		public uint dwSrcMonId;

		public uint dwDstCamId;

		public byte byTrunkType;

		public byte byAbility;

		public byte bySubChan;

		public byte byLevel;

		public ushort wReserveUserID;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 18, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_MATRIX_TRUNKLIST
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public uint dwTrunkNum;

		public IntPtr pBuffer;

		public uint dwBufLen;
	}

	public struct NET_DVR_PROTO_TYPE_EX
	{
		public ushort wType;

		public ushort wCommunitionType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byDescribe;
	}

	public struct NET_DVR_MATRIXMANAGE_ABIILITY
	{
		public uint dwSize;

		public uint dwMaxCameraNum;

		public uint dwMaxMonitorNum;

		public ushort wMaxMatrixNum;

		public ushort wMaxSerialNum;

		public ushort wMaxUser;

		public ushort wMaxResourceArrayNum;

		public ushort wMaxUserArrayNum;

		public ushort wMaxTrunkNum;

		public byte nStartUserNum;

		public byte nStartUserGroupNum;

		public byte nStartResourceGroupNum;

		public byte nStartSerialNum;

		public uint dwMatrixProtoNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_PROTO_TYPE_EX[] struMatrixProto;

		public uint dwKeyBoardProtoNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_PROTO_TYPE_EX[] struKeyBoardProto;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_SINGLE_FACESNAPCFG
	{
		public byte byActive;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public NET_VCA_SIZE_FILTER struSizeFilter;

		public NET_VCA_POLYGON struVcaPolygon;
	}

	public struct NET_VCA_FACESNAPCFG
	{
		public uint dwSize;

		public byte bySnapTime;

		public byte bySnapInterval;

		public byte bySnapThreshold;

		public byte byGenerateRate;

		public byte bySensitive;

		public byte byReferenceBright;

		public byte byMatchType;

		public byte byMatchThreshold;

		public NET_DVR_JPEGPARA struPictureParam;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
		public NET_VCA_SINGLE_FACESNAPCFG[] struRule;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 100, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_VCA_FACESNAP_RESULT
	{
		public uint dwSize;

		public uint dwRelativeTime;

		public uint dwAbsTime;

		public uint dwFacePicID;

		public uint dwFaceScore;

		public NET_VCA_TARGET_INFO struTargetInfo;

		public NET_VCA_RECT struRect;

		public NET_VCA_DEV_INFO struDevInfo;

		public uint dwFacePicLen;

		public uint dwBackgroundPicLen;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 60, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public IntPtr pBuffer1;

		public IntPtr pBuffer2;
	}

	public struct NET_DVR_DEFOCUS_ALARM
	{
		public uint dwSize;

		public NET_VCA_DEV_INFO struDevInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_AUDIOEXCEPTION_ALARM
	{
		public uint dwSize;

		public byte byAlarmType;

		public byte byRes1;

		public ushort wAudioDecibel;

		public NET_VCA_DEV_INFO struDevInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_BUTTON_DOWN_EXCEPTION_ALARM
	{
		public uint dwSize;

		public NET_VCA_DEV_INFO struDevInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_FD_IMAGE_CFG
	{
		public uint dwWidth;

		public uint dwHeight;

		public uint dwImageLen;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public IntPtr pImage;
	}

	public struct NET_VCA_FD_PROCIMG_CFG
	{
		public uint dwSize;

		public byte byEnable;

		public byte bySensitivity;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 22, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_VCA_SIZE_FILTER struSizeFilter;

		public NET_VCA_POLYGON struPolygon;

		public NET_VCA_FD_IMAGE_CFG struFDImage;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_VCA_SUB_PROCIMG
	{
		public uint dwImageLen;

		public uint dwFaceScore;

		public NET_VCA_RECT struVcaRect;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public IntPtr pImage;
	}

	public struct NET_VCA_FD_PROCIMG_RESULT
	{
		public uint dwSize;

		public uint dwImageId;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public uint dwSubImageNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 30, ArraySubType = UnmanagedType.Struct)]
		public NET_VCA_SUB_PROCIMG[] struProcImg;
	}

	public struct NET_VCA_PICMODEL_RESULT
	{
		public uint dwImageLen;

		public uint dwModelLen;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public IntPtr pImage;

		public IntPtr pModel;
	}

	public struct NET_VCA_REGISTER_PIC
	{
		public uint dwImageID;

		public uint dwFaceScore;

		public NET_VCA_RECT struVcaRect;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_AREAINFOCFG
	{
		public ushort wNationalityID;

		public ushort wProvinceID;

		public ushort wCityID;

		public ushort wCountyID;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_HUMAN_ATTRIBUTE
	{
		public byte bySex;

		public byte byCertificateType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.I1)]
		public byte[] byBirthDate;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byName;

		public NET_DVR_AREAINFOCFG struNativePlace;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byCertificateNumber;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_VCA_HUMANATTRIBUTE_COND
	{
		public byte bySex;

		public byte byCertificateType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.I1)]
		public byte[] byStartBirthDate;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.I1)]
		public byte[] byEndBirthDate;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byName;

		public NET_DVR_AREAINFOCFG struNativePlace;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byCertificateNumber;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_BLACKLIST_INFO
	{
		public uint dwSize;

		public uint dwRegisterID;

		public uint dwGroupNo;

		public byte byType;

		public byte byLevel;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_VCA_HUMAN_ATTRIBUTE struAttribute;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRemark;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_VCA_BLACKLIST_PARA
	{
		public uint dwSize;

		public NET_VCA_BLACKLIST_INFO struBlackListInfo;

		public uint dwRegisterPicNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.Struct)]
		public NET_VCA_PICMODEL_RESULT[] struRegisterPic;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 40, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_BLACKLIST_COND
	{
		public int lChannel;

		public uint dwGroupNo;

		public byte byType;

		public byte byLevel;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_VCA_HUMAN_ATTRIBUTE struAttribute;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_BLACKLIST_PIC
	{
		public uint dwSize;

		public uint dwFacePicNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.Struct)]
		public NET_VCA_PICMODEL_RESULT[] struBlackListPic;
	}

	public struct NET_VCA_FIND_PICTURECOND
	{
		public int lChannel;

		public NET_DVR_TIME struStartTime;

		public NET_DVR_TIME struStopTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_SUB_SNAPPIC_DATA
	{
		public uint dwFacePicID;

		public uint dwFacePicLen;

		public NET_DVR_TIME struSnapTime;

		public uint dwSimilarity;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6144)]
		public string sPicBuf;
	}

	public struct NET_VCA_ADVANCE_FIND
	{
		public uint dwFacePicID;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 36, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_NORMAL_FIND
	{
		public uint dwImageID;

		public uint dwFaceScore;

		public NET_VCA_RECT struVcaRect;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct NET_VCA_FIND_SNAPPIC_UNION
	{
		[FieldOffset(0)]
		public NET_VCA_NORMAL_FIND struNormalFind;

		[FieldOffset(0)]
		public NET_VCA_ADVANCE_FIND struAdvanceFind;
	}

	public enum VCA_FIND_SNAPPIC_TYPE
	{
		VCA_NORMAL_FIND,
		VCA_ADVANCE_FIND
	}

	public struct NET_VCA_FIND_PICTURECOND_ADVANCE
	{
		public int lChannel;

		public NET_DVR_TIME struStartTime;

		public NET_DVR_TIME struStopTime;

		public byte byThreshold;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 23, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public VCA_FIND_SNAPPIC_TYPE dwFindType;

		public NET_VCA_FIND_SNAPPIC_UNION uFindParam;
	}

	public struct NET_VCA_FACESNAP_INFO_ALARM
	{
		public uint dwRelativeTime;

		public uint dwAbsTime;

		public uint dwSnapFacePicID;

		public uint dwSnapFacePicLen;

		public NET_VCA_DEV_INFO struDevInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public IntPtr pBuffer1;
	}

	public struct NET_VCA_BLACKLIST_INFO_ALARM
	{
		public NET_VCA_BLACKLIST_INFO struBlackListInfo;

		public uint dwBlackListPicLen;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public IntPtr pBuffer1;
	}

	public struct NET_VCA_FACESNAP_MATCH_ALARM
	{
		public uint dwSize;

		public float fSimilarity;

		public NET_VCA_FACESNAP_INFO_ALARM struSnapInfo;

		public NET_VCA_BLACKLIST_INFO_ALARM struBlackListInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 60, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_BLACKLIST_INFO_ALARM_LOG
	{
		public NET_VCA_BLACKLIST_INFO struBlackListInfo;

		public uint dwBlackListPicID;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_FACESNAP_INFO_ALARM_LOG
	{
		public uint dwRelativeTime;

		public uint dwAbsTime;

		public uint dwSnapFacePicID;

		public NET_VCA_DEV_INFO struDevInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_FACESNAP_MATCH_ALARM_LOG
	{
		public uint dwSize;

		public float fSimilarity;

		public NET_VCA_FACESNAP_INFO_ALARM_LOG struSnapInfoLog;

		public NET_VCA_BLACKLIST_INFO_ALARM_LOG struBlackListInfoLog;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 60, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_FACEMATCH_PICCOND
	{
		public uint dwSize;

		public uint dwSnapFaceID;

		public uint dwBlackListID;

		public uint dwBlackListFaceID;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_FACEMATCH_PICTURE
	{
		public uint dwSize;

		public uint dwSnapFaceLen;

		public uint dwBlackListFaceLen;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public IntPtr pSnapFace;

		public IntPtr pBlackListFace;
	}

	public struct NET_VCA_BLACKLIST_FASTREGISTER_PARA
	{
		public uint dwSize;

		public NET_VCA_BLACKLIST_INFO struBlackListInfo;

		public uint dwImageLen;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 124, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public IntPtr pImage;
	}

	public struct NET_VCA_SINGLE_PATH
	{
		public byte byActive;

		public byte byType;

		public byte bySaveAlarmPic;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public uint dwDiskDriver;

		public uint dwLeftSpace;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_VCA_SAVE_PATH_CFG
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 33, ArraySubType = UnmanagedType.Struct)]
		public NET_VCA_SINGLE_PATH[] struPathInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 40, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_DEV_ACCESS_CFG
	{
		public uint dwSize;

		public NET_DVR_IPADDR struIP;

		public ushort wDevicePort;

		public byte byEnable;

		public byte byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sUserName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] sPassword;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 60, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_DISPWINDOWMODE
	{
		public byte byDispChanType;

		public byte byDispChanSeq;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.I1)]
		public byte[] byDispMode;
	}

	public struct NET_DVR_DISPINFO
	{
		public byte byChanNums;

		public byte byStartChan;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.U1)]
		public uint[] dwSupportResolution;
	}

	public struct NET_DVR_SCREENINFO
	{
		public byte bySupportBigScreenNums;

		public byte byStartBigScreenNum;

		public byte byMaxScreenX;

		public byte byMaxScreenY;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_MATRIX_ABILITY_V41
	{
		public uint dwSize;

		public byte byDspNums;

		public byte byDecChanNums;

		public byte byStartChan;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_DVR_DISPINFO struVgaInfo;

		public NET_DVR_DISPINFO struBncInfo;

		public NET_DVR_DISPINFO struHdmiInfo;

		public NET_DVR_DISPINFO struDviInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_DISPWINDOWMODE[] struDispMode;

		public NET_DVR_SCREENINFO struBigScreenInfo;

		public byte bySupportAutoReboot;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 119, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct NET_DVR_VIDEO_PLATFORM
	{
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 160, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		[FieldOffset(0)]
		public VideoPlatform struVideoPlatform;

		[FieldOffset(0)]
		public NotVideoPlatform struNotVideoPlatform;
	}

	public struct VideoPlatform
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 36, ArraySubType = UnmanagedType.I1)]
		public byte[] byJoinDecoderId;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 36, ArraySubType = UnmanagedType.I1)]
		public byte[] byDecResolution;

		public NET_DVR_RECTCFG struPosition;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 80, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NotVideoPlatform
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 160, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_MATRIX_VOUTCFG
	{
		public uint dwSize;

		public byte byAudio;

		public byte byAudioWindowIdx;

		public byte byDispChanType;

		public byte byVedioFormat;

		public uint dwResolution;

		public uint dwWindowMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 36, ArraySubType = UnmanagedType.I1)]
		public byte[] byJoinDecChan;

		public byte byEnlargeStatus;

		public byte byEnlargeSubWindowIndex;

		public byte byScale;

		public byte byUnionType;

		public NET_DVR_VIDEO_PLATFORM struDiff;

		public uint dwDispChanNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 76, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_DISP_CHAN_STATUS_V41
	{
		public byte byDispStatus;

		public byte byBVGA;

		public byte byVideoFormat;

		public byte byWindowMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 36, ArraySubType = UnmanagedType.I1)]
		public byte[] byJoinDecChan;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 36, ArraySubType = UnmanagedType.I1)]
		public byte[] byFpsDisp;

		public byte byScreenMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public uint dwDispChan;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_DECODER_WORK_STATUS_V41
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_MATRIX_CHAN_STATUS[] struDecChanStatus;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_DISP_CHAN_STATUS_V41[] struDispChanStatus;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byAlarmInStatus;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byAlarmOutStatus;

		public byte byAudioInChanStatus;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 127, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_MATRIX_DEC_REMOTE_PLAY_V41
	{
		public uint dwSize;

		public NET_DVR_IPADDR struIP;

		public ushort wDVRPort;

		public byte byChannel;

		public byte byReserve;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sUserName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] sPassword;

		public uint dwPlayMode;

		public NET_DVR_TIME StartTime;

		public NET_DVR_TIME StopTime;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string sFileName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_RECTCFG_SCENE
	{
		public ushort wXCoordinate;

		public ushort wYCoordinate;

		public ushort wWidth;

		public ushort wHeight;
	}

	public struct NET_DVR_SCENEDISPCFG
	{
		public byte byEnable;

		public byte bySoltNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public byte byDispChanNum;

		public byte byAudio;

		public byte byAudioWindowIdx;

		public byte byVedioFormat;

		public byte byWindowMode;

		public byte byEnlargeStatus;

		public byte byEnlargeSubWindowIndex;

		public byte byScale;

		public uint dwResolution;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 36, ArraySubType = UnmanagedType.I1)]
		public byte[] byJoinDecChan;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 36, ArraySubType = UnmanagedType.I1)]
		public byte[] byJoinDecoderId;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 36, ArraySubType = UnmanagedType.I1)]
		public byte[] byDecResolution;

		public byte byRow;

		public byte byColumn;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;

		public NET_DVR_RECTCFG struDisp;
	}

	public struct NET_DVR_DEV_CHAN_INFO_SCENE
	{
		public NET_DVR_IPADDR struIP;

		public ushort wDVRPort;

		public byte byChannel;

		public byte byTransProtocol;

		public byte byTransMode;

		public byte byFactoryType;

		public byte byDeviceType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sUserName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] sPassword;
	}

	public struct NET_DVR_STREAM_MEDIA_SERVER_CFG_SCENE
	{
		public byte byValid;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_DVR_IPADDR struDevIP;

		public ushort wDevPort;

		public byte byTransmitType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_PU_STREAM_CFG_SCENE
	{
		public NET_DVR_STREAM_MEDIA_SERVER_CFG_SCENE streamMediaServerCfg;

		public NET_DVR_DEV_CHAN_INFO_SCENE struDevChanInfo;
	}

	public struct NET_DVR_CYC_SUR_CHAN_ELE_SCENE
	{
		public byte byEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public NET_DVR_STREAM_MEDIA_SERVER_CFG_SCENE struStreamMediaSvrCfg;

		public NET_DVR_DEV_CHAN_INFO_SCENE struDecChanInfo;
	}

	public struct NET_DVR_MATRIX_LOOP_DECINFO_SCENE
	{
		public ushort wPoolTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_CYC_SUR_CHAN_ELE_SCENE[] struChanArray;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_DECODECHANCFG_SCENE
	{
		public byte byDecodeEnable;

		public byte bySlotNum;

		public byte byDecChan;

		public byte byJointAllDecodeChan;

		public byte byJointSlotNum;

		public byte byJointChanNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public STRUDECCFG struDecCfg;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct STRUDECCFG
	{
		[FieldOffset(0)]
		public NET_DVR_PU_STREAM_CFG_SCENE struSceneDynamicDecCfg;

		[FieldOffset(0)]
		public NET_DVR_MATRIX_LOOP_DECINFO_SCENE struSceneCycDecCfg;
	}

	public struct NET_DVR_BIGSCREENCFG_SCENE
	{
		public byte byAllValid;

		public byte byAssociateBaseMap;

		public byte byEnableSpartan;

		public byte byRes;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_WINCFG[] struWinCfg;

		public NET_DVR_BIGSCREENCFG struBigScreen;

		public void Init()
		{
			struBigScreen = default(NET_DVR_BIGSCREENCFG);
			struWinCfg = new NET_DVR_WINCFG[32];
		}
	}

	public struct NET_DVR_MATRIX_SCENECFG
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sSceneName;

		public byte byBigScreenNums;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public ushort wDecChanNums;

		public ushort wDispChanNums;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;

		public IntPtr pBigScreenBuffer;

		public IntPtr pDecChanBuffer;

		public IntPtr pDispChanBuffer;

		public void Init()
		{
			sSceneName = new byte[32];
			byRes1 = new byte[3];
			byRes1 = new byte[12];
		}
	}

	public struct NET_DVR_BIGSCREENASSOCIATECFG
	{
		public uint dwSize;

		public byte byEnableBaseMap;

		public byte byAssociateBaseMap;

		public byte byEnableSpartan;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 21, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_SCREEN_WINCFG
	{
		public uint dwSize;

		public byte byVaild;

		public byte byInputType;

		public ushort wInputIdx;

		public uint dwLayerIdx;

		public NET_DVR_RECTCFG struWin;

		public byte byWndIndex;

		public byte byCBD;

		public byte bySubWnd;

		public byte byRes1;

		public uint dwDeviceIndex;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_WINLIST
	{
		public uint dwSize;

		public ushort wScreenSeq;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public uint dwWinNum;

		public IntPtr pBuffer;

		public uint dwBufLen;
	}

	public struct NET_DVR_LAYOUTCFG
	{
		public uint dwSize;

		public byte byValid;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byLayoutName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 224, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SCREEN_WINCFG[] struWinCfg;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_LAYOUT_LIST
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_LAYOUTCFG[] struLayoutInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public enum NET_DVR_CAM_MODE
	{
		NET_DVR_UNKNOW,
		NET_DVR_CAM_BNC,
		NET_DVR_CAM_VGA,
		NET_DVR_CAM_DVI,
		NET_DVR_CAM_HDMI,
		NET_DVR_CAM_IP,
		NET_DVR_CAM_RGB,
		NET_DVR_CAM_DECODER,
		NET_DVR_CAM_MATRIX,
		NET_DVR_CAM_YPBPR,
		NET_DVR_CAM_USB
	}

	public struct NET_DVR_INPUTSTREAMCFG
	{
		public uint dwSize;

		public byte byValid;

		public byte byCamMode;

		public ushort wInputNo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sCamName;

		public NET_DVR_VIDEOEFFECT struVideoEffect;

		public NET_DVR_PU_STREAM_CFG struPuStream;

		public ushort wBoardNum;

		public ushort wInputIdxOnBoard;

		public ushort wResolutionX;

		public ushort wResolutionY;

		public byte byVideoFormat;

		public byte byNetSignalResolution;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sGroupName;

		public byte byJointMatrix;

		public byte byRes;
	}

	public struct NET_DVR_INPUTSTREAM_LIST
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 224, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_INPUTSTREAMCFG[] struInputStreamInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_OUTPUTPARAM
	{
		public uint dwSize;

		public byte byMonMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public uint dwResolution;

		public NET_DVR_VIDEOEFFECT struVideoEffect;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_OUTPUTCFG
	{
		public uint dwSize;

		public byte byScreenLayX;

		public byte byScreenLayY;

		public ushort wOutputChanNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_DVR_OUTPUTPARAM struOutputParam;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] sWallName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_SCREENSERVER_ABILITY
	{
		public uint dwSize;

		public byte byIsSupportScreenNum;

		public byte bySerialNums;

		public byte byMaxInputNums;

		public byte byMaxLayoutNums;

		public byte byMaxWinNums;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 19, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public byte byMaxScreenLayX;

		public byte byMaxScreenLayY;

		public ushort wMatrixProtoNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_PROTO_TYPE[] struScreenProto;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_SCREENCONTROL_ABILITY
	{
		public uint dwSize;

		public byte byLayoutNum;

		public byte byWinNum;

		public byte byOsdNum;

		public byte byLogoNum;

		public byte byInputStreamNum;

		public byte byOutputChanNum;

		public byte byCamGroupNum;

		public byte byPlanNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public byte byIsSupportPlayBack;

		public byte byMatrixInputNum;

		public byte byMatrixOutputNum;

		public NET_DVR_DISPINFO struVgaInfo;

		public NET_DVR_DISPINFO struBncInfo;

		public NET_DVR_DISPINFO struHdmiInfo;

		public NET_DVR_DISPINFO struDviInfo;

		public byte byMaxUserNums;

		public byte byPicSpan;

		public ushort wDVCSDevNum;

		public ushort wNetSignalNum;

		public ushort wBaseCoordinateX;

		public ushort wBaseCoordinateY;

		public byte byExternalMatrixNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 49, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_ANALOGINPUTSTATUS
	{
		public uint dwLostFrame;

		public byte byHaveSignal;

		public byte byVideoFormat;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 46, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct NET_DVR_INPUTSTATUS_UNION
	{
		[FieldOffset(0)]
		public NET_DVR_MATRIX_CHAN_STATUS struIpInputStatus;

		[FieldOffset(0)]
		public NET_DVR_ANALOGINPUTSTATUS struAnalogInputStatus;
	}

	public struct NET_DVR_INPUTSTATUS
	{
		public ushort wInputNo;

		public byte byInputType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_DVR_INPUTSTATUS_UNION struStatusUnion;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_SCREENINPUTSTATUS
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public uint dwNums;

		public IntPtr pBuffer;

		public uint dwBufLen;
	}

	public struct NET_DVR_SCREENALARMCFG
	{
		public uint dwSize;

		public byte byAlarmType;

		public byte byBoardType;

		public byte bySubException;

		public byte byRes1;

		public ushort wStartInputNum;

		public ushort wEndInputNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_MATRIX_CFG
	{
		public byte byValid;

		public byte byCommandProtocol;

		public byte byScreenType;

		public byte byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byScreenToMatrix;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_DIGITALSCREEN
	{
		public NET_DVR_IPADDR struAddress;

		public ushort wPort;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 26, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_ANALOGSCREEN
	{
		public byte byDevSerPortNum;

		public byte byScreenSerPort;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 130, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public NET_DVR_MATRIX_CFG struMatrixCfg;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct NET_DVR_SCREEN_UNION
	{
		[FieldOffset(0)]
		public NET_DVR_DIGITALSCREEN struDigitalScreen;

		[FieldOffset(0)]
		public NET_DVR_ANALOGSCREEN struAnalogScreen;
	}

	public struct NET_DVR_SCREEN_SCREENINFO
	{
		public uint dwSize;

		public byte byValid;

		public byte nLinkMode;

		public byte byDeviceType;

		public byte byScreenLayX;

		public byte byScreenLayY;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sUserName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] sPassword;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sDevName;

		public NET_DVR_SCREEN_UNION struScreenUnion;

		public byte byInputNum;

		public byte byOutputNum;

		public byte byCBDNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 29, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_BASEMAP_CFG
	{
		public byte byScreenIndex;

		public byte byMapNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] res;

		public ushort wSourWidth;

		public ushort wSourHeight;
	}

	public struct NET_DVR_OSDCFG
	{
		public uint dwSize;

		public byte byValid;

		public byte byDispMode;

		public byte byFontColorY;

		public byte byFontColorU;

		public byte byFontColorV;

		public byte byBackColorY;

		public byte byBackColorU;

		public byte byBackColorV;

		public ushort wXCoordinate;

		public ushort wYCoordinate;

		public ushort wWidth;

		public ushort wHeight;

		public uint dwCharCnt;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.U2)]
		public ushort[] wOSDChar;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_SERIAL_CONTROL
	{
		public uint dwSize;

		public byte bySerialNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] bySerial;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public enum INPUT_INTERFACE_TYPE
	{
		INTERFACE_VGA,
		INTERFACE_SVIDEO,
		INTERFACE_YPBPR,
		INTERFACE_DVI,
		INTERFACE_BNC,
		INTERFACE_DVI_LOOP,
		INTERFACE_BNC_LOOP,
		INTERFACE_HDMI
	}

	public struct NET_DVR_INPUT_INTERFACE_CTRL
	{
		public byte byInputSourceType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 15, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_DISPLAY_COLOR_CTRL
	{
		public byte byColorType;

		public char byScale;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 14, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_DISPLAY_POSITION_CTRL
	{
		public byte byPositionType;

		public char byScale;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 14, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct NET_DVR_SCREEN_CONTROL_PARAM
	{
		[FieldOffset(0)]
		public NET_DVR_INPUT_INTERFACE_CTRL struInputCtrl;

		[FieldOffset(0)]
		public NET_DVR_DISPLAY_COLOR_CTRL struDisplayCtrl;

		[FieldOffset(0)]
		public NET_DVR_DISPLAY_POSITION_CTRL struPositionCtrl;

		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_SCREEN_CONTROL
	{
		public uint dwSize;

		public uint dwCommand;

		public byte byProtocol;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_DVR_SCREEN_CONTROL_PARAM struControlParam;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 52, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_SCREEN_CONTROL_V41
	{
		public uint dwSize;

		public byte bySerialNo;

		public byte byBeginAddress;

		public byte byEndAddress;

		public byte byProtocol;

		public uint dwCommand;

		public NET_DVR_SCREEN_CONTROL_PARAM struControlParam;

		public byte byWallNo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 51, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public enum NET_DVR_PLAN_OPERATE_TYPE
	{
		NET_DVR_SWITCH_LAYOUT = 1,
		NET_DVR_SCREEN_POWER_OFF,
		NET_DVR_SCREEN_POWER_ON
	}

	public struct NET_DVR_PLAN_INFO
	{
		public byte byValid;

		public byte byType;

		public ushort wLayoutNo;

		public byte byScreenStyle;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public uint dwDelayTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_CYCLE_TIME
	{
		public byte byValid;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public NET_DVR_TIME_EX struTime;
	}

	public struct NET_DVR_PLAN_CFG
	{
		public uint dwSize;

		public byte byValid;

		public byte byWorkMode;

		public byte byWallNo;

		public byte byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byPlanName;

		public NET_DVR_TIME_EX struTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_CYCLE_TIME[] struTimeCycle;

		public uint dwWorkCount;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_PLAN_INFO[] strPlanEntry;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_PLAN_LIST
	{
		public uint dwSize;

		public uint dwPlanNums;

		public IntPtr pBuffer;

		public byte byWallNo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public uint dwBufLen;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_CONTROL_PARAM
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sDeviceID;

		public ushort wChan;

		public byte byIndex;

		public byte byRes1;

		public uint dwControlParam;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_DEVICE_RUN_STATUS
	{
		public uint dwSize;

		public uint dwMemoryTotal;

		public uint dwMemoryUsage;

		public byte byCPUUsage;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 127, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_ACCESS_CAMERA_INFO
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string sCameraInfo;

		public byte byInterfaceType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public uint dwChannel;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_AUDIO_INPUT_PARAM
	{
		public byte byAudioInputType;

		public byte byVolume;

		public byte byEnableNoiseFilter;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.I1)]
		public byte[] byres;
	}

	public struct NET_DVR_CAMERA_DEHAZE_CFG
	{
		public uint dwSize;

		public byte byDehazeMode;

		public byte byLevel;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_INPUT_SIGNAL_LIST
	{
		public uint dwSize;

		public uint dwInputSignalNums;

		public IntPtr pBuffer;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public uint dwBufLen;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_RECORD_TIME_SPAN_INQUIRY
	{
		public uint dwSize;

		public byte byType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 63, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_RECORD_TIME_SPAN
	{
		public uint dwSize;

		public NET_DVR_TIME strBeginTime;

		public NET_DVR_TIME strEndTime;

		public byte byType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 35, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_DRAWFRAME_DISK_QUOTA_CFG
	{
		public uint dwSize;

		public byte byPicQuota;

		public byte byRecordQuota;

		public byte byDrawFrameRecordQuota;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 61, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_NAT_PORT
	{
		public ushort wEnable;

		public ushort wExtPort;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_NAT_CFG
	{
		public uint dwSize;

		public ushort wEnableUpnp;

		public ushort wEnableNat;

		public NET_DVR_IPADDR struIpAddr;

		public NET_DVR_NAT_PORT struHttpPort;

		public NET_DVR_NAT_PORT struCmdPort;

		public NET_DVR_NAT_PORT struRtspPort;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byFriendName;

		public byte byNatType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_DVR_NAT_PORT struHttpsPort;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 76, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_UPNP_PORT_STATE
	{
		public uint dwEnabled;

		public ushort wInternalPort;

		public ushort wExternalPort;

		public uint dwStatus;

		public NET_DVR_IPADDR struNatExternalIp;

		public NET_DVR_IPADDR struNatInternalIp;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_UPNP_NAT_STATE
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_UPNP_PORT_STATE[] strUpnpPort;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 200, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_PLAYCOND
	{
		public uint dwChannel;

		public NET_DVR_TIME struStartTime;

		public NET_DVR_TIME struStopTime;

		public byte byDrawFrame;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 63, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_VOD_PARA
	{
		public uint dwSize;

		public NET_DVR_STREAM_INFO struIDInfo;

		public NET_DVR_TIME struBeginTime;

		public NET_DVR_TIME struEndTime;

		public IntPtr hWnd;

		public byte byDrawFrame;

		public byte byVolumeType;

		public byte byVolumeNum;

		public byte byRes1;

		public uint dwFileIndex;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_ATMFINDINFO
	{
		public byte byTransactionType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public uint dwTransationAmount;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct NET_DVR_SPECIAL_FINDINFO_UNION
	{
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] byLenth;

		[FieldOffset(0)]
		public NET_DVR_ATMFINDINFO struATMFindInfo;
	}

	public struct NET_DVR_FILECOND_V40
	{
		public int lChannel;

		public uint dwFileType;

		public uint dwIsLocked;

		public uint dwUseCardNo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] sCardNumber;

		public NET_DVR_TIME struStartTime;

		public NET_DVR_TIME struStopTime;

		public byte byDrawFrame;

		public byte byFindType;

		public byte byQuickSearch;

		public byte bySpecialFindInfoType;

		public uint dwVolumeNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byWorkingDeviceGUID;

		public NET_DVR_SPECIAL_FINDINFO_UNION uSpecialFindInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_AES_KEY_INFO
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] sAESKey;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_POE_CFG
	{
		public NET_DVR_IPADDR struIP;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_CUSTOM_PROTOCAL
	{
		public uint dwSize;

		public uint dwEnabled;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sProtocalName;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public uint dwEnableSubStream;

		public byte byMainProType;

		public byte byMainTransType;

		public ushort wMainPort;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string sMainPath;

		public byte bySubProType;

		public byte bySubTransType;

		public ushort wSubPort;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string sSubPath;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 200, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes2;
	}

	public struct NET_DVR_PREVIEWINFO
	{
		public int lChannel;

		public uint dwStreamType;

		public uint dwLinkMode;

		public IntPtr hPlayWnd;

		public bool bBlocked;

		public bool bPassbackRecord;

		public byte byPreviewMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byStreamID;

		public byte byProtoType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public uint dwDisplayBufNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 216, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_ITS_OVERLAPCFG_COND
	{
		public uint dwSize;

		public uint dwChannel;

		public uint dwConfigMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_ITS_OVERLAP_SINGLE_ITEM_PARAM
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public byte byItemType;

		public byte byChangeLineNum;

		public byte bySpaceNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 15, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_ITS_OVERLAP_ITEM_PARAM
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50, ArraySubType = UnmanagedType.Struct)]
		public NET_ITS_OVERLAP_SINGLE_ITEM_PARAM[] struSingleItem;

		public uint dwLinePercent;

		public uint dwItemsStlye;

		public ushort wStartPosTop;

		public ushort wStartPosLeft;

		public ushort wCharStyle;

		public ushort wCharSize;

		public ushort wCharInterval;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public uint dwForeClorRGB;

		public uint dwBackClorRGB;

		public byte byColorAdapt;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 31, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_ITS_OVERLAP_INFO_PARAM
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
		public byte[] bySite;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRoadNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byInstrumentNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byDirection;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byDirectionDesc;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byLaneDes;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 44, ArraySubType = UnmanagedType.I1)]
		public byte[] byMonitoringSite1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byMonitoringSite2;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_ITS_OVERLAP_CFG
	{
		public uint dwSize;

		public byte byEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		public NET_ITS_OVERLAP_ITEM_PARAM struOverLapItem;

		public NET_ITS_OVERLAP_INFO_PARAM struOverLapInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_SETUPALARM_PARAM
	{
		public uint dwSize;

		public byte byLevel;

		public byte byAlarmInfoType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 14, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public delegate void EXCEPYIONCALLBACK(uint dwType, int lUserID, int lHandle, IntPtr pUser);

	public delegate int MESSCALLBACK(int lCommand, string sDVRIP, string pBuf, uint dwBufLen);

	public delegate int MESSCALLBACKEX(int iCommand, int iUserID, string pBuf, uint dwBufLen);

	public delegate int MESSCALLBACKNEW(int lCommand, string sDVRIP, string pBuf, uint dwBufLen, ushort dwLinkDVRPort);

	public delegate int MESSAGECALLBACK(int lCommand, IntPtr sDVRIP, IntPtr pBuf, uint dwBufLen, uint dwUser);

	public delegate void MSGCallBack(int lCommand, ref NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser);

	public delegate void REALDATACALLBACK(int lRealHandle, uint dwDataType, IntPtr pBuffer, uint dwBufSize, IntPtr pUser);

	public delegate void DRAWFUN(int lRealHandle, IntPtr hDc, uint dwUser);

	public delegate void SETREALDATACALLBACK(int lRealHandle, uint dwDataType, IntPtr pBuffer, uint dwBufSize, uint dwUser);

	public delegate void STDDATACALLBACK(int lRealHandle, uint dwDataType, ref byte pBuffer, uint dwBufSize, uint dwUser);

	public delegate void PLAYDATACALLBACK(int lPlayHandle, uint dwDataType, IntPtr pBuffer, uint dwBufSize, uint dwUser);

	public delegate void VOICEDATACALLBACK(int lVoiceComHandle, string pRecvDataBuffer, uint dwBufSize, byte byAudioFlag, uint dwUser);

	public delegate void VOICEDATACALLBACKV30(int lVoiceComHandle, IntPtr pRecvDataBuffer, uint dwBufSize, byte byAudioFlag, IntPtr pUser);

	public delegate void VOICEAUDIOSTART(string pRecvDataBuffer, uint dwBufSize, IntPtr pUser);

	public delegate void SERIALDATACALLBACK(int lSerialHandle, string pRecvDataBuffer, uint dwBufSize, uint dwUser);

	public enum IVS_PARAM_KEY
	{
		OBJECT_DETECT_SENSITIVE = 1,
		BACKGROUND_UPDATE_RATE = 2,
		SCENE_CHANGE_RATIO = 3,
		SUPPRESS_LAMP = 4,
		MIN_OBJECT_SIZE = 5,
		OBJECT_GENERATE_RATE = 6,
		MISSING_OBJECT_HOLD = 7,
		MAX_MISSING_DISTANCE = 8,
		OBJECT_MERGE_SPEED = 9,
		REPEATED_MOTION_SUPPRESS = 10,
		ILLUMINATION_CHANGE = 11,
		TRACK_OUTPUT_MODE = 12,
		ENTER_CHANGE_HOLD = 13,
		RESUME_DEFAULT_PARAM = 255
	}

	public struct NET_DVR_LF_CALIBRATION_PARAM
	{
		public byte byPointNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_CB_POINT[] struCBPoint;
	}

	public struct NET_DVR_LF_CFG
	{
		public uint dwSize;

		public byte byEnable;

		public byte byFollowChan;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public NET_DVR_LF_CALIBRATION_PARAM struCalParam;
	}

	public struct NET_DVR_LF_MANUAL_CTRL_INFO
	{
		public NET_VCA_POINT struCtrlPoint;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_LF_TRACK_TARGET_INFO
	{
		public uint dwTargetID;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_DVR_LF_TRACK_MODE
	{
		[StructLayout(LayoutKind.Explicit)]
		public struct uModeParam
		{
			[FieldOffset(0)]
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U4)]
			public uint[] dwULen;

			[FieldOffset(0)]
			public NET_DVR_LF_MANUAL_CTRL_INFO struManualCtrl;

			[FieldOffset(0)]
			public NET_DVR_LF_TRACK_TARGET_INFO struTargetTrack;
		}

		public uint dwSize;

		public byte byTrackMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public enum VCA_RECOGNIZE_SCENE
	{
		VCA_LOW_SPEED_SCENE,
		VCA_HIGH_SPEED_SCENE,
		VCA_MOBILE_CAMERA_SCENE
	}

	public enum VCA_RECOGNIZE_RESULT
	{
		VCA_RECOGNIZE_FAILURE,
		VCA_IMAGE_RECOGNIZE_SUCCESS,
		VCA_VIDEO_RECOGNIZE_SUCCESS_OF_BEST_LICENSE,
		VCA_VIDEO_RECOGNIZE_SUCCESS_OF_NEW_LICENSE,
		VCA_VIDEO_RECOGNIZE_FINISH_OF_CUR_LICENSE
	}

	public enum VCA_TRIGGER_TYPE
	{
		INTER_TRIGGER,
		EXTER_TRIGGER
	}

	public struct NET_VCA_PLATE_PARAM
	{
		public NET_VCA_RECT struSearchRect;

		public NET_VCA_RECT struInvalidateRect;

		public ushort wMinPlateWidth;

		public ushort wTriggerDuration;

		public byte byTriggerType;

		public byte bySensitivity;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byCharPriority;
	}

	public struct NET_VCA_PLATEINFO
	{
		public VCA_RECOGNIZE_SCENE eRecogniseScene;

		public NET_VCA_PLATE_PARAM struModifyParam;
	}

	public struct NET_VCA_PLATECFG
	{
		public uint dwSize;

		public byte byPicProType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		public NET_DVR_JPEGPARA struPictureParam;

		public NET_VCA_PLATEINFO struPlateInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 14, ArraySubType = UnmanagedType.Struct)]
		public NET_DVR_SCHEDTIME[] struAlarmTime;

		public NET_DVR_HANDLEEXCEPTION_V30 struHandleType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
		public byte[] byRelRecordChan;
	}

	public struct NET_VCA_PLATE_INFO
	{
		public VCA_RECOGNIZE_RESULT eResultFlag;

		public VCA_PLATE_TYPE ePlateType;

		public VCA_PLATE_COLOR ePlateColor;

		public NET_VCA_RECT struPlateRect;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U4)]
		public uint[] dwRes;

		public uint dwLicenseLen;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sLicense;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string sBelieve;
	}

	public struct NET_VCA_PLATE_RESULT
	{
		public uint dwSize;

		public uint dwRelativeTime;

		public uint dwAbsTime;

		public byte byPlateNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.Struct)]
		public NET_VCA_PLATE_INFO[] struPlateInfo;

		public uint dwPicDataLen;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U4)]
		public uint[] dwRes2;

		public IntPtr pImage;
	}

	public struct NET_VCA_LINE_SEGMENT
	{
		public NET_VCA_POINT struStartPoint;

		public NET_VCA_POINT struEndPoint;

		public float fValue;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct NET_VCA_LINE_SEG_LIST
	{
		public uint dwSize;

		public byte bySegNum;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.Struct)]
		public NET_VCA_LINE_SEGMENT[] struSeg;
	}

	public struct NET_DVR_PLATE_RET
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byPlateNum;

		public byte byVehicleType;

		public byte byTrafficLight;

		public byte byPlateColor;

		public byte byDriveChan;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
		public byte[] byTimeInfo;

		public byte byCarSpeed;

		public byte byCarSpeedH;

		public byte byCarSpeedL;

		public byte byRes;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 988, ArraySubType = UnmanagedType.I1)]
		public byte[] byInfo;

		public uint dwPicLen;
	}

	public struct NET_DVR_CCD_CFG
	{
		public uint dwSize;

		public byte byBlc;

		public byte byBlcMode;

		public byte byAwb;

		public byte byAgc;

		public byte byDayNight;

		public byte byMirror;

		public byte byShutter;

		public byte byIrCutTime;

		public byte byLensType;

		public byte byEnVideoTrig;

		public byte byCapShutter;

		public byte byEnRecognise;
	}

	public struct tagCAMERAPARAMCFG
	{
		public uint dwSize;

		public uint dwPowerLineFrequencyMode;

		public uint dwWhiteBalanceMode;

		public uint dwWhiteBalanceModeRGain;

		public uint dwWhiteBalanceModeBGain;

		public uint dwExposureMode;

		public uint dwExposureSet;

		public uint dwExposureUserSet;

		public uint dwExposureTarget;

		public uint dwIrisMode;

		public uint dwGainLevel;

		public uint dwBrightnessLevel;

		public uint dwContrastLevel;

		public uint dwSharpnessLevel;

		public uint dwSaturationLevel;

		public uint dwHueLevel;

		public uint dwGammaCorrectionEnabled;

		public uint dwGammaCorrectionLevel;

		public uint dwWDREnabled;

		public uint dwWDRLevel1;

		public uint dwWDRLevel2;

		public uint dwWDRContrastLevel;

		public uint dwDayNightFilterType;

		public uint dwSwitchScheduleEnabled;

		public uint dwBeginTime;

		public uint dwEndTime;

		public uint dwDayToNightFilterLevel;

		public uint dwNightToDayFilterLevel;

		public uint dwDayNightFilterTime;

		public uint dwBacklightMode;

		public uint dwPositionX1;

		public uint dwPositionY1;

		public uint dwPositionX2;

		public uint dwPositionY2;

		public uint dwBacklightLevel;

		public uint dwDigitalNoiseRemoveEnable;

		public uint dwDigitalNoiseRemoveLevel;

		public uint dwMirror;

		public uint dwDigitalZoom;

		public uint dwDeadPixelDetect;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.U4)]
		public uint[] dwRes;
	}

	public struct tagIMAGEREGION
	{
		public uint dwSize;

		public ushort wImageRegionTopLeftX;

		public ushort wImageRegionTopLeftY;

		public ushort wImageRegionWidth;

		public ushort wImageRegionHeight;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct tagIMAGESUBPARAM
	{
		public NET_DVR_SCHEDTIME struImageStatusTime;

		public byte byImageEnhancementLevel;

		public byte byImageDenoiseLevel;

		public byte byImageStableEnable;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct tagIMAGEPARAM
	{
		public uint dwSize;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 28, ArraySubType = UnmanagedType.Struct)]
		public tagIMAGESUBPARAM[] struImageParamSched;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] byRes;
	}

	public struct PLAY_INFO
	{
		public int iUserID;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
		public string strDeviceIP;

		public int iDevicePort;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string strDevAdmin;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string strDevPsd;

		public int iChannel;

		public int iLinkMode;

		public bool bUseMedia;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
		public string strMediaIP;

		public int iMediaPort;
	}

	public struct BLOCKTIME
	{
		public ushort wYear;

		public byte bMonth;

		public byte bDay;

		public byte bHour;

		public byte bMinute;

		public byte bSecond;
	}

	public struct VODSEARCHPARAM
	{
		public IntPtr sessionHandle;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
		public string dvrIP;

		public uint dvrPort;

		public uint channelNum;

		public BLOCKTIME startTime;

		public BLOCKTIME stopTime;

		public bool bUseIPServer;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string SerialNumber;
	}

	public struct SECTIONLIST
	{
		public BLOCKTIME startTime;

		public BLOCKTIME stopTime;

		public byte byRecType;

		public IntPtr pNext;
	}

	public struct VODOPENPARAM
	{
		public IntPtr sessionHandle;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
		public string dvrIP;

		public uint dvrPort;

		public uint channelNum;

		public BLOCKTIME startTime;

		public BLOCKTIME stopTime;

		public uint uiUser;

		public bool bUseIPServer;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string SerialNumber;

		public VodStreamFrameData streamFrameData;
	}

	public struct CONNPARAM
	{
		public uint uiUser;

		public ErrorCallback errorCB;
	}

	public delegate void ErrorCallback(IntPtr hSession, uint dwUser, int lErrorType);

	public delegate void VodStreamFrameData(IntPtr hStream, uint dwUser, int lFrameType, IntPtr pBuffer, uint dwSize);

	public struct PACKET_INFO
	{
		public int nPacketType;

		public IntPtr pPacketBuffer;

		public uint dwPacketSize;

		public int nYear;

		public int nMonth;

		public int nDay;

		public int nHour;

		public int nMinute;

		public int nSecond;

		public uint dwTimeStamp;
	}

	public struct STOREINFO
	{
		public int iMaxChannels;

		public int iDiskGroup;

		public int iStreamType;

		public bool bAnalyze;

		public bool bCycWrite;

		public uint uiFileSize;

		public CALLBACKFUN_MESSAGE funCallback;
	}

	public struct CREATEFILE_INFO
	{
		public int iHandle;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string strCameraid;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string strFileName;

		public BLOCKTIME tFileCreateTime;
	}

	public struct CLOSEFILE_INFO
	{
		public int iHandle;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string strCameraid;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string strFileName;

		public BLOCKTIME tFileSwitchTime;
	}

	public delegate int CALLBACKFUN_MESSAGE(int iMessageType, IntPtr pBuf, int iBufLen);

	public struct NET_DVR_LOG_MATRIX
	{
		public NET_DVR_TIME strLogTime;

		public uint dwMajorType;

		public uint dwMinorType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] sPanelUser;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
		public byte[] sNetUser;

		public NET_DVR_IPADDR struRemoteHostAddr;

		public uint dwParaType;

		public uint dwChannel;

		public uint dwDiskNumber;

		public uint dwAlarmInPort;

		public uint dwAlarmOutPort;

		public uint dwInfoLen;

		public byte byDevSequence;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
		public byte[] byMacAddr;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.I1)]
		public byte[] sSerialNumber;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11785)]
		public string sInfo;
	}

	public struct tagVEDIOPLATLOG
	{
		public byte bySearchCondition;

		public byte byDevSequence;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.I1)]
		public byte[] sSerialNumber;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
		public byte[] byMacAddr;
	}

	public const int SDK_PLAYMPEG4 = 1;

	public const int SDK_HCNETSDK = 2;

	public const int NAME_LEN = 32;

	public const int PASSWD_LEN = 16;

	public const int GUID_LEN = 16;

	public const int DEV_TYPE_NAME_LEN = 24;

	public const int MAX_NAMELEN = 16;

	public const int MAX_RIGHT = 32;

	public const int SERIALNO_LEN = 48;

	public const int MACADDR_LEN = 6;

	public const int MAX_ETHERNET = 2;

	public const int MAX_NETWORK_CARD = 4;

	public const int PATHNAME_LEN = 128;

	public const int MAX_NUMBER_LEN = 32;

	public const int MAX_NAME_LEN = 128;

	public const int MAX_TIMESEGMENT_V30 = 8;

	public const int MAX_TIMESEGMENT = 4;

	public const int MAX_ICR_NUM = 8;

	public const int MAX_SHELTERNUM = 4;

	public const int PHONENUMBER_LEN = 32;

	public const int MAX_DISKNUM = 16;

	public const int MAX_DISKNUM_V10 = 8;

	public const int MAX_WINDOW_V30 = 32;

	public const int MAX_WINDOW = 16;

	public const int MAX_VGA_V30 = 4;

	public const int MAX_VGA = 1;

	public const int MAX_USERNUM_V30 = 32;

	public const int MAX_USERNUM = 16;

	public const int MAX_EXCEPTIONNUM_V30 = 32;

	public const int MAX_EXCEPTIONNUM = 16;

	public const int MAX_LINK = 6;

	public const int MAX_ITC_EXCEPTIONOUT = 32;

	public const int MAX_DECPOOLNUM = 4;

	public const int MAX_DECNUM = 4;

	public const int MAX_TRANSPARENTNUM = 2;

	public const int MAX_CYCLE_CHAN = 16;

	public const int MAX_CYCLE_CHAN_V30 = 64;

	public const int MAX_DIRNAME_LENGTH = 80;

	public const int MAX_STRINGNUM_V30 = 8;

	public const int MAX_STRINGNUM = 4;

	public const int MAX_STRINGNUM_EX = 8;

	public const int MAX_AUXOUT_V30 = 16;

	public const int MAX_AUXOUT = 4;

	public const int MAX_HD_GROUP = 16;

	public const int MAX_NFS_DISK = 8;

	public const int IW_ESSID_MAX_SIZE = 32;

	public const int IW_ENCODING_TOKEN_MAX = 32;

	public const int WIFI_WEP_MAX_KEY_COUNT = 4;

	public const int WIFI_WEP_MAX_KEY_LENGTH = 33;

	public const int WIFI_WPA_PSK_MAX_KEY_LENGTH = 63;

	public const int WIFI_WPA_PSK_MIN_KEY_LENGTH = 8;

	public const int WIFI_MAX_AP_COUNT = 20;

	public const int MAX_SERIAL_NUM = 64;

	public const int MAX_DDNS_NUMS = 10;

	public const int MAX_EMAIL_ADDR_LEN = 48;

	public const int MAX_EMAIL_PWD_LEN = 32;

	public const int MAXPROGRESS = 100;

	public const int MAX_SERIALNUM = 2;

	public const int CARDNUM_LEN = 20;

	public const int CARDNUM_LEN_OUT = 32;

	public const int MAX_VIDEOOUT_V30 = 4;

	public const int MAX_VIDEOOUT = 2;

	public const int MAX_PRESET_V30 = 256;

	public const int MAX_TRACK_V30 = 256;

	public const int MAX_CRUISE_V30 = 256;

	public const int MAX_PRESET = 128;

	public const int MAX_TRACK = 128;

	public const int MAX_CRUISE = 128;

	public const int CRUISE_MAX_PRESET_NUMS = 32;

	public const int MAX_SERIAL_PORT = 8;

	public const int MAX_PREVIEW_MODE = 8;

	public const int MAX_MATRIXOUT = 16;

	public const int LOG_INFO_LEN = 11840;

	public const int DESC_LEN = 16;

	public const int PTZ_PROTOCOL_NUM = 200;

	public const int MAX_AUDIO = 1;

	public const int MAX_AUDIO_V30 = 2;

	public const int MAX_CHANNUM = 16;

	public const int MAX_ALARMIN = 16;

	public const int MAX_ALARMOUT = 4;

	public const int MAX_ANALOG_CHANNUM = 32;

	public const int MAX_ANALOG_ALARMOUT = 32;

	public const int MAX_ANALOG_ALARMIN = 32;

	public const int MAX_IP_DEVICE = 32;

	public const int MAX_IP_DEVICE_V40 = 64;

	public const int MAX_IP_CHANNEL = 32;

	public const int MAX_IP_ALARMIN = 128;

	public const int MAX_IP_ALARMOUT = 64;

	public const int MAX_IP_ALARMIN_V40 = 4096;

	public const int MAX_IP_ALARMOUT_V40 = 4096;

	public const int MAX_RECORD_FILE_NUM = 20;

	public const int MAX_ATM_NUM = 1;

	public const int MAX_ACTION_TYPE = 12;

	public const int ATM_FRAMETYPE_NUM = 4;

	public const int MAX_ATM_PROTOCOL_NUM = 1025;

	public const int ATM_PROTOCOL_SORT = 4;

	public const int ATM_DESC_LEN = 32;

	public const int MAX_CHANNUM_V30 = 64;

	public const int MAX_ALARMOUT_V30 = 96;

	public const int MAX_ALARMIN_V30 = 160;

	public const int MAX_CHANNUM_V40 = 512;

	public const int MAX_ALARMOUT_V40 = 4128;

	public const int MAX_ALARMIN_V40 = 4128;

	public const int MAX_HUMAN_PICTURE_NUM = 10;

	public const int MAX_HUMAN_BIRTHDATE_LEN = 10;

	public const int MAX_LAYERNUMS = 32;

	public const int MAX_ROIDETECT_NUM = 8;

	public const int MAX_LANERECT_NUM = 5;

	public const int MAX_FORTIFY_NUM = 10;

	public const int MAX_INTERVAL_NUM = 4;

	public const int MAX_CHJC_NUM = 3;

	public const int MAX_VL_NUM = 5;

	public const int MAX_DRIVECHAN_NUM = 16;

	public const int MAX_COIL_NUM = 3;

	public const int MAX_SIGNALLIGHT_NUM = 6;

	public const int LEN_32 = 32;

	public const int LEN_31 = 31;

	public const int MAX_CABINET_COUNT = 8;

	public const int MAX_ID_LEN = 48;

	public const int MAX_PARKNO_LEN = 16;

	public const int MAX_ALARMREASON_LEN = 32;

	public const int MAX_UPGRADE_INFO_LEN = 48;

	public const int MAX_CUSTOMDIR_LEN = 32;

	public const int MAX_TRANSPARENT_CHAN_NUM = 4;

	public const int MAX_TRANSPARENT_ACCESS_NUM = 4;

	public const int MAX_PARKING_STATUS = 8;

	public const int MAX_PARKING_NUM = 4;

	public const int MAX_ITS_SCENE_NUM = 16;

	public const int MAX_SCENE_TIMESEG_NUM = 16;

	public const int MAX_IVMS_IP_CHANNEL = 128;

	public const int DEVICE_ID_LEN = 48;

	public const int MONITORSITE_ID_LEN = 48;

	public const int MAX_AUXAREA_NUM = 16;

	public const int MAX_SLAVE_CHANNEL_NUM = 16;

	public const int MAX_SCH_TASKS_NUM = 10;

	public const int MAX_SERVERID_LEN = 64;

	public const int MAX_SERVERDOMAIN_LEN = 128;

	public const int MAX_AUTHENTICATEID_LEN = 64;

	public const int MAX_AUTHENTICATEPASSWD_LEN = 32;

	public const int MAX_SERVERNAME_LEN = 64;

	public const int MAX_COMPRESSIONID_LEN = 64;

	public const int MAX_SIPSERVER_ADDRESS_LEN = 128;

	public const int MAX_PlATE_NO_LEN = 32;

	public const int UPNP_PORT_NUM = 12;

	public const int MAX_LOCAL_ADDR_LEN = 96;

	public const int MAX_COUNTRY_NAME_LEN = 4;

	public const int NORMALCONNECT = 1;

	public const int MEDIACONNECT = 2;

	public const int HCDVR = 1;

	public const int MEDVR = 2;

	public const int PCDVR = 3;

	public const int HC_9000 = 4;

	public const int HF_I = 5;

	public const int PCNVR = 6;

	public const int HC_76NVR = 8;

	public const int DS8000HC_NVR = 0;

	public const int DS9000HC_NVR = 1;

	public const int DS8000ME_NVR = 2;

	public const int NET_DVR_NOERROR = 0;

	public const int NET_DVR_PASSWORD_ERROR = 1;

	public const int NET_DVR_NOENOUGHPRI = 2;

	public const int NET_DVR_NOINIT = 3;

	public const int NET_DVR_CHANNEL_ERROR = 4;

	public const int NET_DVR_OVER_MAXLINK = 5;

	public const int NET_DVR_VERSIONNOMATCH = 6;

	public const int NET_DVR_NETWORK_FAIL_CONNECT = 7;

	public const int NET_DVR_NETWORK_SEND_ERROR = 8;

	public const int NET_DVR_NETWORK_RECV_ERROR = 9;

	public const int NET_DVR_NETWORK_RECV_TIMEOUT = 10;

	public const int NET_DVR_NETWORK_ERRORDATA = 11;

	public const int NET_DVR_ORDER_ERROR = 12;

	public const int NET_DVR_OPERNOPERMIT = 13;

	public const int NET_DVR_COMMANDTIMEOUT = 14;

	public const int NET_DVR_ERRORSERIALPORT = 15;

	public const int NET_DVR_ERRORALARMPORT = 16;

	public const int NET_DVR_PARAMETER_ERROR = 17;

	public const int NET_DVR_CHAN_EXCEPTION = 18;

	public const int NET_DVR_NODISK = 19;

	public const int NET_DVR_ERRORDISKNUM = 20;

	public const int NET_DVR_DISK_FULL = 21;

	public const int NET_DVR_DISK_ERROR = 22;

	public const int NET_DVR_NOSUPPORT = 23;

	public const int NET_DVR_BUSY = 24;

	public const int NET_DVR_MODIFY_FAIL = 25;

	public const int NET_DVR_PASSWORD_FORMAT_ERROR = 26;

	public const int NET_DVR_DISK_FORMATING = 27;

	public const int NET_DVR_DVRNORESOURCE = 28;

	public const int NET_DVR_DVROPRATEFAILED = 29;

	public const int NET_DVR_OPENHOSTSOUND_FAIL = 30;

	public const int NET_DVR_DVRVOICEOPENED = 31;

	public const int NET_DVR_TIMEINPUTERROR = 32;

	public const int NET_DVR_NOSPECFILE = 33;

	public const int NET_DVR_CREATEFILE_ERROR = 34;

	public const int NET_DVR_FILEOPENFAIL = 35;

	public const int NET_DVR_OPERNOTFINISH = 36;

	public const int NET_DVR_GETPLAYTIMEFAIL = 37;

	public const int NET_DVR_PLAYFAIL = 38;

	public const int NET_DVR_FILEFORMAT_ERROR = 39;

	public const int NET_DVR_DIR_ERROR = 40;

	public const int NET_DVR_ALLOC_RESOURCE_ERROR = 41;

	public const int NET_DVR_AUDIO_MODE_ERROR = 42;

	public const int NET_DVR_NOENOUGH_BUF = 43;

	public const int NET_DVR_CREATESOCKET_ERROR = 44;

	public const int NET_DVR_SETSOCKET_ERROR = 45;

	public const int NET_DVR_MAX_NUM = 46;

	public const int NET_DVR_USERNOTEXIST = 47;

	public const int NET_DVR_WRITEFLASHERROR = 48;

	public const int NET_DVR_UPGRADEFAIL = 49;

	public const int NET_DVR_CARDHAVEINIT = 50;

	public const int NET_DVR_PLAYERFAILED = 51;

	public const int NET_DVR_MAX_USERNUM = 52;

	public const int NET_DVR_GETLOCALIPANDMACFAIL = 53;

	public const int NET_DVR_NOENCODEING = 54;

	public const int NET_DVR_IPMISMATCH = 55;

	public const int NET_DVR_MACMISMATCH = 56;

	public const int NET_DVR_UPGRADELANGMISMATCH = 57;

	public const int NET_DVR_MAX_PLAYERPORT = 58;

	public const int NET_DVR_NOSPACEBACKUP = 59;

	public const int NET_DVR_NODEVICEBACKUP = 60;

	public const int NET_DVR_PICTURE_BITS_ERROR = 61;

	public const int NET_DVR_PICTURE_DIMENSION_ERROR = 62;

	public const int NET_DVR_PICTURE_SIZ_ERROR = 63;

	public const int NET_DVR_LOADPLAYERSDKFAILED = 64;

	public const int NET_DVR_LOADPLAYERSDKPROC_ERROR = 65;

	public const int NET_DVR_LOADDSSDKFAILED = 66;

	public const int NET_DVR_LOADDSSDKPROC_ERROR = 67;

	public const int NET_DVR_DSSDK_ERROR = 68;

	public const int NET_DVR_VOICEMONOPOLIZE = 69;

	public const int NET_DVR_JOINMULTICASTFAILED = 70;

	public const int NET_DVR_CREATEDIR_ERROR = 71;

	public const int NET_DVR_BINDSOCKET_ERROR = 72;

	public const int NET_DVR_SOCKETCLOSE_ERROR = 73;

	public const int NET_DVR_USERID_ISUSING = 74;

	public const int NET_DVR_SOCKETLISTEN_ERROR = 75;

	public const int NET_DVR_PROGRAM_EXCEPTION = 76;

	public const int NET_DVR_WRITEFILE_FAILED = 77;

	public const int NET_DVR_FORMAT_READONLY = 78;

	public const int NET_DVR_WITHSAMEUSERNAME = 79;

	public const int NET_DVR_DEVICETYPE_ERROR = 80;

	public const int NET_DVR_LANGUAGE_ERROR = 81;

	public const int NET_DVR_PARAVERSION_ERROR = 82;

	public const int NET_DVR_IPCHAN_NOTALIVE = 83;

	public const int NET_DVR_RTSP_SDK_ERROR = 84;

	public const int NET_DVR_CONVERT_SDK_ERROR = 85;

	public const int NET_DVR_IPC_COUNT_OVERFLOW = 86;

	public const int NET_PLAYM4_NOERROR = 500;

	public const int NET_PLAYM4_PARA_OVER = 501;

	public const int NET_PLAYM4_ORDER_ERROR = 502;

	public const int NET_PLAYM4_TIMER_ERROR = 503;

	public const int NET_PLAYM4_DEC_VIDEO_ERROR = 504;

	public const int NET_PLAYM4_DEC_AUDIO_ERROR = 505;

	public const int NET_PLAYM4_ALLOC_MEMORY_ERROR = 506;

	public const int NET_PLAYM4_OPEN_FILE_ERROR = 507;

	public const int NET_PLAYM4_CREATE_OBJ_ERROR = 508;

	public const int NET_PLAYM4_CREATE_DDRAW_ERROR = 509;

	public const int NET_PLAYM4_CREATE_OFFSCREEN_ERROR = 510;

	public const int NET_PLAYM4_BUF_OVER = 511;

	public const int NET_PLAYM4_CREATE_SOUND_ERROR = 512;

	public const int NET_PLAYM4_SET_VOLUME_ERROR = 513;

	public const int NET_PLAYM4_SUPPORT_FILE_ONLY = 514;

	public const int NET_PLAYM4_SUPPORT_STREAM_ONLY = 515;

	public const int NET_PLAYM4_SYS_NOT_SUPPORT = 516;

	public const int NET_PLAYM4_FILEHEADER_UNKNOWN = 517;

	public const int NET_PLAYM4_VERSION_INCORRECT = 518;

	public const int NET_PALYM4_INIT_DECODER_ERROR = 519;

	public const int NET_PLAYM4_CHECK_FILE_ERROR = 520;

	public const int NET_PLAYM4_INIT_TIMER_ERROR = 521;

	public const int NET_PLAYM4_BLT_ERROR = 522;

	public const int NET_PLAYM4_UPDATE_ERROR = 523;

	public const int NET_PLAYM4_OPEN_FILE_ERROR_MULTI = 524;

	public const int NET_PLAYM4_OPEN_FILE_ERROR_VIDEO = 525;

	public const int NET_PLAYM4_JPEG_COMPRESS_ERROR = 526;

	public const int NET_PLAYM4_EXTRACT_NOT_SUPPORT = 527;

	public const int NET_PLAYM4_EXTRACT_DATA_ERROR = 528;

	public const int NET_DVR_SUPPORT_DDRAW = 1;

	public const int NET_DVR_SUPPORT_BLT = 2;

	public const int NET_DVR_SUPPORT_BLTFOURCC = 4;

	public const int NET_DVR_SUPPORT_BLTSHRINKX = 8;

	public const int NET_DVR_SUPPORT_BLTSHRINKY = 16;

	public const int NET_DVR_SUPPORT_BLTSTRETCHX = 32;

	public const int NET_DVR_SUPPORT_BLTSTRETCHY = 64;

	public const int NET_DVR_SUPPORT_SSE = 128;

	public const int NET_DVR_SUPPORT_MMX = 256;

	public const int LIGHT_PWRON = 2;

	public const int WIPER_PWRON = 3;

	public const int FAN_PWRON = 4;

	public const int HEATER_PWRON = 5;

	public const int AUX_PWRON1 = 6;

	public const int AUX_PWRON2 = 7;

	public const int SET_PRESET = 8;

	public const int CLE_PRESET = 9;

	public const int ZOOM_IN = 11;

	public const int ZOOM_OUT = 12;

	public const int FOCUS_NEAR = 13;

	public const int FOCUS_FAR = 14;

	public const int IRIS_OPEN = 15;

	public const int IRIS_CLOSE = 16;

	public const int TILT_UP = 21;

	public const int TILT_DOWN = 22;

	public const int PAN_LEFT = 23;

	public const int PAN_RIGHT = 24;

	public const int UP_LEFT = 25;

	public const int UP_RIGHT = 26;

	public const int DOWN_LEFT = 27;

	public const int DOWN_RIGHT = 28;

	public const int PAN_AUTO = 29;

	public const int FILL_PRE_SEQ = 30;

	public const int SET_SEQ_DWELL = 31;

	public const int SET_SEQ_SPEED = 32;

	public const int CLE_PRE_SEQ = 33;

	public const int STA_MEM_CRUISE = 34;

	public const int STO_MEM_CRUISE = 35;

	public const int RUN_CRUISE = 36;

	public const int RUN_SEQ = 37;

	public const int STOP_SEQ = 38;

	public const int GOTO_PRESET = 39;

	public const int NET_DVR_PLAYSTART = 1;

	public const int NET_DVR_PLAYSTOP = 2;

	public const int NET_DVR_PLAYPAUSE = 3;

	public const int NET_DVR_PLAYRESTART = 4;

	public const int NET_DVR_PLAYFAST = 5;

	public const int NET_DVR_PLAYSLOW = 6;

	public const int NET_DVR_PLAYNORMAL = 7;

	public const int NET_DVR_PLAYFRAME = 8;

	public const int NET_DVR_PLAYSTARTAUDIO = 9;

	public const int NET_DVR_PLAYSTOPAUDIO = 10;

	public const int NET_DVR_PLAYAUDIOVOLUME = 11;

	public const int NET_DVR_PLAYSETPOS = 12;

	public const int NET_DVR_PLAYGETPOS = 13;

	public const int NET_DVR_PLAYGETTIME = 14;

	public const int NET_DVR_PLAYGETFRAME = 15;

	public const int NET_DVR_GETTOTALFRAMES = 16;

	public const int NET_DVR_GETTOTALTIME = 17;

	public const int NET_DVR_THROWBFRAME = 20;

	public const int NET_DVR_SETSPEED = 24;

	public const int NET_DVR_KEEPALIVE = 25;

	public const int NET_DVR_PLAYSETTIME = 26;

	public const int NET_DVR_PLAYGETTOTALLEN = 27;

	public const int NET_DVR_PLAY_FORWARD = 29;

	public const int NET_DVR_PLAY_REVERSE = 30;

	public const int NET_DVR_SET_TRANS_TYPE = 32;

	public const int NET_DVR_PLAY_CONVERT = 33;

	public const int KEY_CODE_1 = 1;

	public const int KEY_CODE_2 = 2;

	public const int KEY_CODE_3 = 3;

	public const int KEY_CODE_4 = 4;

	public const int KEY_CODE_5 = 5;

	public const int KEY_CODE_6 = 6;

	public const int KEY_CODE_7 = 7;

	public const int KEY_CODE_8 = 8;

	public const int KEY_CODE_9 = 9;

	public const int KEY_CODE_0 = 10;

	public const int KEY_CODE_POWER = 11;

	public const int KEY_CODE_MENU = 12;

	public const int KEY_CODE_ENTER = 13;

	public const int KEY_CODE_CANCEL = 14;

	public const int KEY_CODE_UP = 15;

	public const int KEY_CODE_DOWN = 16;

	public const int KEY_CODE_LEFT = 17;

	public const int KEY_CODE_RIGHT = 18;

	public const int KEY_CODE_EDIT = 19;

	public const int KEY_CODE_ADD = 20;

	public const int KEY_CODE_MINUS = 21;

	public const int KEY_CODE_PLAY = 22;

	public const int KEY_CODE_REC = 23;

	public const int KEY_CODE_PAN = 24;

	public const int KEY_CODE_M = 25;

	public const int KEY_CODE_A = 26;

	public const int KEY_CODE_F1 = 27;

	public const int KEY_CODE_F2 = 28;

	public const int KEY_PTZ_UP_START = 15;

	public const int KEY_PTZ_UP_STOP = 32;

	public const int KEY_PTZ_DOWN_START = 16;

	public const int KEY_PTZ_DOWN_STOP = 33;

	public const int KEY_PTZ_LEFT_START = 17;

	public const int KEY_PTZ_LEFT_STOP = 34;

	public const int KEY_PTZ_RIGHT_START = 18;

	public const int KEY_PTZ_RIGHT_STOP = 35;

	public const int KEY_PTZ_AP1_START = 19;

	public const int KEY_PTZ_AP1_STOP = 36;

	public const int KEY_PTZ_AP2_START = 24;

	public const int KEY_PTZ_AP2_STOP = 37;

	public const int KEY_PTZ_FOCUS1_START = 26;

	public const int KEY_PTZ_FOCUS1_STOP = 38;

	public const int KEY_PTZ_FOCUS2_START = 25;

	public const int KEY_PTZ_FOCUS2_STOP = 39;

	public const int KEY_PTZ_B1_START = 40;

	public const int KEY_PTZ_B1_STOP = 41;

	public const int KEY_PTZ_B2_START = 42;

	public const int KEY_PTZ_B2_STOP = 43;

	public const int KEY_CODE_11 = 44;

	public const int KEY_CODE_12 = 45;

	public const int KEY_CODE_13 = 46;

	public const int KEY_CODE_14 = 47;

	public const int KEY_CODE_15 = 48;

	public const int KEY_CODE_16 = 49;

	public const int NET_DVR_GET_DEVICECFG = 100;

	public const int NET_DVR_SET_DEVICECFG = 101;

	public const int NET_DVR_GET_NETCFG = 102;

	public const int NET_DVR_SET_NETCFG = 103;

	public const int NET_DVR_GET_PICCFG = 104;

	public const int NET_DVR_SET_PICCFG = 105;

	public const int NET_DVR_GET_COMPRESSCFG = 106;

	public const int NET_DVR_SET_COMPRESSCFG = 107;

	public const int NET_DVR_GET_RECORDCFG = 108;

	public const int NET_DVR_SET_RECORDCFG = 109;

	public const int NET_DVR_GET_DECODERCFG = 110;

	public const int NET_DVR_SET_DECODERCFG = 111;

	public const int NET_DVR_GET_RS232CFG = 112;

	public const int NET_DVR_SET_RS232CFG = 113;

	public const int NET_DVR_GET_ALARMINCFG = 114;

	public const int NET_DVR_SET_ALARMINCFG = 115;

	public const int NET_DVR_GET_ALARMOUTCFG = 116;

	public const int NET_DVR_SET_ALARMOUTCFG = 117;

	public const int NET_DVR_GET_TIMECFG = 118;

	public const int NET_DVR_SET_TIMECFG = 119;

	public const int NET_DVR_GET_PREVIEWCFG = 120;

	public const int NET_DVR_SET_PREVIEWCFG = 121;

	public const int NET_DVR_GET_VIDEOOUTCFG = 122;

	public const int NET_DVR_SET_VIDEOOUTCFG = 123;

	public const int NET_DVR_GET_USERCFG = 124;

	public const int NET_DVR_SET_USERCFG = 125;

	public const int NET_DVR_GET_EXCEPTIONCFG = 126;

	public const int NET_DVR_SET_EXCEPTIONCFG = 127;

	public const int NET_DVR_GET_ZONEANDDST = 128;

	public const int NET_DVR_SET_ZONEANDDST = 129;

	public const int NET_DVR_GET_SHOWSTRING = 130;

	public const int NET_DVR_SET_SHOWSTRING = 131;

	public const int NET_DVR_GET_EVENTCOMPCFG = 132;

	public const int NET_DVR_SET_EVENTCOMPCFG = 133;

	public const int NET_DVR_GET_AUXOUTCFG = 140;

	public const int NET_DVR_SET_AUXOUTCFG = 141;

	public const int NET_DVR_GET_PREVIEWCFG_AUX = 142;

	public const int NET_DVR_SET_PREVIEWCFG_AUX = 143;

	public const int NET_DVR_GET_PICCFG_EX = 200;

	public const int NET_DVR_SET_PICCFG_EX = 201;

	public const int NET_DVR_GET_USERCFG_EX = 202;

	public const int NET_DVR_SET_USERCFG_EX = 203;

	public const int NET_DVR_GET_COMPRESSCFG_EX = 204;

	public const int NET_DVR_SET_COMPRESSCFG_EX = 205;

	public const int NET_DVR_GET_NETAPPCFG = 222;

	public const int NET_DVR_SET_NETAPPCFG = 223;

	public const int NET_DVR_GET_NTPCFG = 224;

	public const int NET_DVR_SET_NTPCFG = 225;

	public const int NET_DVR_GET_DDNSCFG = 226;

	public const int NET_DVR_SET_DDNSCFG = 227;

	public const int NET_DVR_GET_EMAILCFG = 228;

	public const int NET_DVR_SET_EMAILCFG = 229;

	public const int NET_DVR_GET_NFSCFG = 230;

	public const int NET_DVR_SET_NFSCFG = 231;

	public const int NET_DVR_GET_SHOWSTRING_EX = 238;

	public const int NET_DVR_SET_SHOWSTRING_EX = 239;

	public const int NET_DVR_GET_NETCFG_OTHER = 244;

	public const int NET_DVR_SET_NETCFG_OTHER = 245;

	public const int NET_DVR_GET_EMAILPARACFG = 250;

	public const int NET_DVR_SET_EMAILPARACFG = 251;

	public const int NET_DVR_GET_DDNSCFG_EX = 274;

	public const int NET_DVR_SET_DDNSCFG_EX = 275;

	public const int NET_DVR_SET_PTZPOS = 292;

	public const int NET_DVR_GET_PTZPOS = 293;

	public const int NET_DVR_GET_PTZSCOPE = 294;

	public const int NET_DVR_GET_AP_INFO_LIST = 305;

	public const int NET_DVR_SET_WIFI_CFG = 306;

	public const int NET_DVR_GET_WIFI_CFG = 307;

	public const int NET_DVR_SET_WIFI_WORKMODE = 308;

	public const int NET_DVR_GET_WIFI_WORKMODE = 309;

	public const int NET_DVR_GET_WIFI_STATUS = 310;

	public const int NET_DVR_GET_NETCFG_V30 = 1000;

	public const int NET_DVR_SET_NETCFG_V30 = 1001;

	public const int NET_DVR_GET_PICCFG_V30 = 1002;

	public const int NET_DVR_SET_PICCFG_V30 = 1003;

	public const int NET_DVR_GET_RECORDCFG_V30 = 1004;

	public const int NET_DVR_SET_RECORDCFG_V30 = 1005;

	public const int NET_DVR_GET_USERCFG_V30 = 1006;

	public const int NET_DVR_SET_USERCFG_V30 = 1007;

	public const int NET_DVR_GET_DDNSCFG_V30 = 1010;

	public const int NET_DVR_SET_DDNSCFG_V30 = 1011;

	public const int NET_DVR_GET_EMAILCFG_V30 = 1012;

	public const int NET_DVR_SET_EMAILCFG_V30 = 1013;

	public const int NET_DVR_GET_CRUISE = 1020;

	public const int NET_DVR_SET_CRUISE = 1021;

	public const int NET_DVR_GET_ALARMINCFG_V30 = 1024;

	public const int NET_DVR_SET_ALARMINCFG_V30 = 1025;

	public const int NET_DVR_GET_ALARMOUTCFG_V30 = 1026;

	public const int NET_DVR_SET_ALARMOUTCFG_V30 = 1027;

	public const int NET_DVR_GET_VIDEOOUTCFG_V30 = 1028;

	public const int NET_DVR_SET_VIDEOOUTCFG_V30 = 1029;

	public const int NET_DVR_GET_SHOWSTRING_V30 = 1030;

	public const int NET_DVR_SET_SHOWSTRING_V30 = 1031;

	public const int NET_DVR_GET_EXCEPTIONCFG_V30 = 1034;

	public const int NET_DVR_SET_EXCEPTIONCFG_V30 = 1035;

	public const int NET_DVR_GET_RS232CFG_V30 = 1036;

	public const int NET_DVR_SET_RS232CFG_V30 = 1037;

	public const int NET_DVR_GET_NET_DISKCFG = 1038;

	public const int NET_DVR_SET_NET_DISKCFG = 1039;

	public const int NET_DVR_GET_COMPRESSCFG_V30 = 1040;

	public const int NET_DVR_SET_COMPRESSCFG_V30 = 1041;

	public const int NET_DVR_GET_DECODERCFG_V30 = 1042;

	public const int NET_DVR_SET_DECODERCFG_V30 = 1043;

	public const int NET_DVR_GET_PREVIEWCFG_V30 = 1044;

	public const int NET_DVR_SET_PREVIEWCFG_V30 = 1045;

	public const int NET_DVR_GET_PREVIEWCFG_AUX_V30 = 1046;

	public const int NET_DVR_SET_PREVIEWCFG_AUX_V30 = 1047;

	public const int NET_DVR_GET_IPPARACFG = 1048;

	public const int NET_DVR_SET_IPPARACFG = 1049;

	public const int NET_DVR_GET_IPPARACFG_V40 = 1062;

	public const int NET_DVR_SET_IPPARACFG_V40 = 1063;

	public const int NET_DVR_GET_IPALARMINCFG = 1050;

	public const int NET_DVR_SET_IPALARMINCFG = 1051;

	public const int NET_DVR_GET_IPALARMOUTCFG = 1052;

	public const int NET_DVR_SET_IPALARMOUTCFG = 1053;

	public const int NET_DVR_GET_HDCFG = 1054;

	public const int NET_DVR_SET_HDCFG = 1055;

	public const int NET_DVR_GET_HDGROUP_CFG = 1056;

	public const int NET_DVR_SET_HDGROUP_CFG = 1057;

	public const int NET_DVR_GET_COMPRESSCFG_AUD = 1058;

	public const int NET_DVR_SET_COMPRESSCFG_AUD = 1059;

	public const int NET_DVR_GET_IPPARACFG_V31 = 1060;

	public const int NET_DVR_SET_IPPARACFG_V31 = 1061;

	public const int NET_DVR_GET_DEVICECFG_V40 = 1100;

	public const int NET_DVR_SET_DEVICECFG_V40 = 1101;

	public const int NET_DVR_GET_NETCFG_MULTI = 1161;

	public const int NET_DVR_SET_NETCFG_MULTI = 1162;

	public const int NET_DVR_GET_NETWORK_BONDING = 1254;

	public const int NET_DVR_SET_NETWORK_BONDING = 1255;

	public const int NET_DVR_GET_PRESET_NAME = 3383;

	public const int NET_DVR_SET_PRESET_NAME = 3382;

	public const int NET_DVR_GET_NAT_CFG = 6111;

	public const int NET_DVR_SET_NAT_CFG = 6112;

	public const int MAJOR_ALARM = 1;

	public const int MINOR_ALARM_IN = 1;

	public const int MINOR_ALARM_OUT = 2;

	public const int MINOR_MOTDET_START = 3;

	public const int MINOR_MOTDET_STOP = 4;

	public const int MINOR_HIDE_ALARM_START = 5;

	public const int MINOR_HIDE_ALARM_STOP = 6;

	public const int MINOR_VCA_ALARM_START = 7;

	public const int MINOR_VCA_ALARM_STOP = 8;

	public const int MAJOR_EXCEPTION = 2;

	public const int MINOR_VI_LOST = 33;

	public const int MINOR_ILLEGAL_ACCESS = 34;

	public const int MINOR_HD_FULL = 35;

	public const int MINOR_HD_ERROR = 36;

	public const int MINOR_DCD_LOST = 37;

	public const int MINOR_IP_CONFLICT = 38;

	public const int MINOR_NET_BROKEN = 39;

	public const int MINOR_REC_ERROR = 40;

	public const int MINOR_IPC_NO_LINK = 41;

	public const int MINOR_VI_EXCEPTION = 42;

	public const int MINOR_IPC_IP_CONFLICT = 43;

	public const int MINOR_FANABNORMAL = 49;

	public const int MINOR_FANRESUME = 50;

	public const int MINOR_SUBSYSTEM_ABNORMALREBOOT = 51;

	public const int MINOR_MATRIX_STARTBUZZER = 52;

	public const int MAJOR_OPERATION = 3;

	public const int MINOR_START_DVR = 65;

	public const int MINOR_STOP_DVR = 66;

	public const int MINOR_STOP_ABNORMAL = 67;

	public const int MINOR_REBOOT_DVR = 68;

	public const int MINOR_LOCAL_LOGIN = 80;

	public const int MINOR_LOCAL_LOGOUT = 81;

	public const int MINOR_LOCAL_CFG_PARM = 82;

	public const int MINOR_LOCAL_PLAYBYFILE = 83;

	public const int MINOR_LOCAL_PLAYBYTIME = 84;

	public const int MINOR_LOCAL_START_REC = 85;

	public const int MINOR_LOCAL_STOP_REC = 86;

	public const int MINOR_LOCAL_PTZCTRL = 87;

	public const int MINOR_LOCAL_PREVIEW = 88;

	public const int MINOR_LOCAL_MODIFY_TIME = 89;

	public const int MINOR_LOCAL_UPGRADE = 90;

	public const int MINOR_LOCAL_RECFILE_OUTPUT = 91;

	public const int MINOR_LOCAL_FORMAT_HDD = 92;

	public const int MINOR_LOCAL_CFGFILE_OUTPUT = 93;

	public const int MINOR_LOCAL_CFGFILE_INPUT = 94;

	public const int MINOR_LOCAL_COPYFILE = 95;

	public const int MINOR_LOCAL_LOCKFILE = 96;

	public const int MINOR_LOCAL_UNLOCKFILE = 97;

	public const int MINOR_LOCAL_DVR_ALARM = 98;

	public const int MINOR_IPC_ADD = 99;

	public const int MINOR_IPC_DEL = 100;

	public const int MINOR_IPC_SET = 101;

	public const int MINOR_LOCAL_START_BACKUP = 102;

	public const int MINOR_LOCAL_STOP_BACKUP = 103;

	public const int MINOR_LOCAL_COPYFILE_START_TIME = 104;

	public const int MINOR_LOCAL_COPYFILE_END_TIME = 105;

	public const int MINOR_LOCAL_ADD_NAS = 106;

	public const int MINOR_LOCAL_DEL_NAS = 107;

	public const int MINOR_LOCAL_SET_NAS = 108;

	public const int MINOR_REMOTE_LOGIN = 112;

	public const int MINOR_REMOTE_LOGOUT = 113;

	public const int MINOR_REMOTE_START_REC = 114;

	public const int MINOR_REMOTE_STOP_REC = 115;

	public const int MINOR_START_TRANS_CHAN = 116;

	public const int MINOR_STOP_TRANS_CHAN = 117;

	public const int MINOR_REMOTE_GET_PARM = 118;

	public const int MINOR_REMOTE_CFG_PARM = 119;

	public const int MINOR_REMOTE_GET_STATUS = 120;

	public const int MINOR_REMOTE_ARM = 121;

	public const int MINOR_REMOTE_DISARM = 122;

	public const int MINOR_REMOTE_REBOOT = 123;

	public const int MINOR_START_VT = 124;

	public const int MINOR_STOP_VT = 125;

	public const int MINOR_REMOTE_UPGRADE = 126;

	public const int MINOR_REMOTE_PLAYBYFILE = 127;

	public const int MINOR_REMOTE_PLAYBYTIME = 128;

	public const int MINOR_REMOTE_PTZCTRL = 129;

	public const int MINOR_REMOTE_FORMAT_HDD = 130;

	public const int MINOR_REMOTE_STOP = 131;

	public const int MINOR_REMOTE_LOCKFILE = 132;

	public const int MINOR_REMOTE_UNLOCKFILE = 133;

	public const int MINOR_REMOTE_CFGFILE_OUTPUT = 134;

	public const int MINOR_REMOTE_CFGFILE_INTPUT = 135;

	public const int MINOR_REMOTE_RECFILE_OUTPUT = 136;

	public const int MINOR_REMOTE_DVR_ALARM = 137;

	public const int MINOR_REMOTE_IPC_ADD = 138;

	public const int MINOR_REMOTE_IPC_DEL = 139;

	public const int MINOR_REMOTE_IPC_SET = 140;

	public const int MINOR_REBOOT_VCA_LIB = 141;

	public const int MINOR_REMOTE_ADD_NAS = 142;

	public const int MINOR_REMOTE_DEL_NAS = 143;

	public const int MINOR_REMOTE_SET_NAS = 144;

	public const int MINOR_SUBSYSTEMREBOOT = 160;

	public const int MINOR_MATRIX_STARTTRANSFERVIDEO = 161;

	public const int MINOR_MATRIX_STOPTRANSFERVIDEO = 162;

	public const int MINOR_REMOTE_SET_ALLSUBSYSTEM = 163;

	public const int MINOR_REMOTE_GET_ALLSUBSYSTEM = 164;

	public const int MINOR_REMOTE_SET_PLANARRAY = 165;

	public const int MINOR_REMOTE_GET_PLANARRAY = 166;

	public const int MINOR_MATRIX_STARTTRANSFERAUDIO = 167;

	public const int MINOR_MATRIX_STOPRANSFERAUDIO = 168;

	public const int MINOR_LOGON_CODESPITTER = 169;

	public const int MINOR_LOGOFF_CODESPITTER = 170;

	public const int MAJOR_INFORMATION = 4;

	public const int MINOR_HDD_INFO = 161;

	public const int MINOR_SMART_INFO = 162;

	public const int MINOR_REC_START = 163;

	public const int MINOR_REC_STOP = 164;

	public const int MINOR_REC_OVERDUE = 165;

	public const int MINOR_LINK_START = 166;

	public const int MINOR_LINK_STOP = 167;

	public const int MINOR_NET_DISK_INFO = 168;

	public const int PARA_VIDEOOUT = 1;

	public const int PARA_IMAGE = 2;

	public const int PARA_ENCODE = 4;

	public const int PARA_NETWORK = 8;

	public const int PARA_ALARM = 16;

	public const int PARA_EXCEPTION = 32;

	public const int PARA_DECODER = 64;

	public const int PARA_RS232 = 128;

	public const int PARA_PREVIEW = 256;

	public const int PARA_SECURITY = 512;

	public const int PARA_DATETIME = 1024;

	public const int PARA_FRAMETYPE = 2048;

	public const int PARA_VCA_RULE = 4096;

	public const int NET_DVR_FILE_SUCCESS = 1000;

	public const int NET_DVR_FILE_NOFIND = 1001;

	public const int NET_DVR_ISFINDING = 1002;

	public const int NET_DVR_NOMOREFILE = 1003;

	public const int NET_DVR_FILE_EXCEPTION = 1004;

	public const int COMM_ALARM = 4352;

	public const int COMM_ALARM_RULE = 4354;

	public const int COMM_ALARM_PDC = 4355;

	public const int COMM_ALARM_ALARMHOST = 4357;

	public const int COMM_ALARM_FACE = 4358;

	public const int COMM_RULE_INFO_UPLOAD = 4359;

	public const int COMM_ALARM_AID = 4368;

	public const int COMM_ALARM_TPS = 4369;

	public const int COMM_UPLOAD_FACESNAP_RESULT = 4370;

	public const int COMM_ALARM_TFS = 4371;

	public const int COMM_ALARM_TPS_V41 = 4372;

	public const int COMM_ALARM_AID_V41 = 4373;

	public const int COMM_ALARM_VQD_EX = 4374;

	public const int COMM_SENSOR_VALUE_UPLOAD = 4384;

	public const int COMM_SENSOR_ALARM = 4385;

	public const int COMM_SWITCH_ALARM = 4386;

	public const int COMM_ALARMHOST_EXCEPTION = 4387;

	public const int COMM_ALARMHOST_OPERATEEVENT_ALARM = 4388;

	public const int COMM_ALARMHOST_SAFETYCABINSTATE = 4389;

	public const int COMM_ALARMHOST_ALARMOUTSTATUS = 4390;

	public const int COMM_ALARMHOST_CID_ALARM = 4391;

	public const int COMM_ALARMHOST_EXTERNAL_DEVICE_ALARM = 4392;

	public const int COMM_ALARMHOST_DATA_UPLOAD = 4393;

	public const int COMM_ALARM_AUDIOEXCEPTION = 4432;

	public const int COMM_ALARM_DEFOCUS = 4433;

	public const int COMM_ALARM_BUTTON_DOWN_EXCEPTION = 4434;

	public const int COMM_ALARM_ALARMGPS = 4610;

	public const int COMM_TRADEINFO = 5376;

	public const int COMM_UPLOAD_PLATE_RESULT = 10240;

	public const int COMM_ITC_STATUS_DETECT_RESULT = 10256;

	public const int COMM_IPC_AUXALARM_RESULT = 10272;

	public const int COMM_UPLOAD_PICTUREINFO = 10496;

	public const int COMM_SNAP_MATCH_ALARM = 10498;

	public const int COMM_ITS_PLATE_RESULT = 12368;

	public const int COMM_ITS_TRAFFIC_COLLECT = 12369;

	public const int COMM_ITS_GATE_VEHICLE = 12370;

	public const int COMM_ITS_GATE_FACE = 12371;

	public const int COMM_ITS_GATE_COSTITEM = 12372;

	public const int COMM_ITS_GATE_HANDOVER = 12373;

	public const int COMM_ITS_PARK_VEHICLE = 12374;

	public const int COMM_ITS_BLACKLIST_ALARM = 12375;

	public const int COMM_ALARM_V30 = 16384;

	public const int COMM_IPCCFG = 16385;

	public const int COMM_IPCCFG_V31 = 16386;

	public const int COMM_IPCCFG_V40 = 16387;

	public const int COMM_ALARM_DEVICE = 16388;

	public const int COMM_ALARM_CVR = 16389;

	public const int COMM_ALARM_HOT_SPARE = 16390;

	public const int COMM_ALARM_V40 = 16391;

	public const int COMM_ITS_ROAD_EXCEPTION = 17664;

	public const int COMM_ITS_EXTERNAL_CONTROL_ALARM = 17696;

	public const int COMM_SCREEN_ALARM = 20480;

	public const int COMM_DVCS_STATE_ALARM = 20481;

	public const int COMM_ALARM_VQD = 24576;

	public const int COMM_PUSH_UPDATE_RECORD_INFO = 24577;

	public const int COMM_DIAGNOSIS_UPLOAD = 20736;

	public const int EXCEPTION_EXCHANGE = 32768;

	public const int EXCEPTION_AUDIOEXCHANGE = 32769;

	public const int EXCEPTION_ALARM = 32770;

	public const int EXCEPTION_PREVIEW = 32771;

	public const int EXCEPTION_SERIAL = 32772;

	public const int EXCEPTION_RECONNECT = 32773;

	public const int EXCEPTION_ALARMRECONNECT = 32774;

	public const int EXCEPTION_SERIALRECONNECT = 32775;

	public const int EXCEPTION_PLAYBACK = 32784;

	public const int EXCEPTION_DISKFMT = 32785;

	public const int NET_DVR_SYSHEAD = 1;

	public const int NET_DVR_STREAMDATA = 2;

	public const int NET_DVR_AUDIOSTREAMDATA = 3;

	public const int NET_DVR_STD_VIDEODATA = 4;

	public const int NET_DVR_STD_AUDIODATA = 5;

	public const int NET_DVR_REALPLAYEXCEPTION = 111;

	public const int NET_DVR_REALPLAYNETCLOSE = 112;

	public const int NET_DVR_REALPLAY5SNODATA = 113;

	public const int NET_DVR_REALPLAYRECONNECT = 114;

	public const int NET_DVR_PLAYBACKOVER = 101;

	public const int NET_DVR_PLAYBACKEXCEPTION = 102;

	public const int NET_DVR_PLAYBACKNETCLOSE = 103;

	public const int NET_DVR_PLAYBACK5SNODATA = 104;

	public const int DVR = 1;

	public const int ATMDVR = 2;

	public const int DVS = 3;

	public const int DEC = 4;

	public const int ENC_DEC = 5;

	public const int DVR_HC = 6;

	public const int DVR_HT = 7;

	public const int DVR_HF = 8;

	public const int DVR_HS = 9;

	public const int DVR_HTS = 10;

	public const int DVR_HB = 11;

	public const int DVR_HCS = 12;

	public const int DVS_A = 13;

	public const int DVR_HC_S = 14;

	public const int DVR_HT_S = 15;

	public const int DVR_HF_S = 16;

	public const int DVR_HS_S = 17;

	public const int ATMDVR_S = 18;

	public const int LOWCOST_DVR = 19;

	public const int DEC_MAT = 20;

	public const int DVR_MOBILE = 21;

	public const int DVR_HD_S = 22;

	public const int DVR_HD_SL = 23;

	public const int DVR_HC_SL = 24;

	public const int DVR_HS_ST = 25;

	public const int DVS_HW = 26;

	public const int DS630X_D = 27;

	public const int IPCAM = 30;

	public const int MEGA_IPCAM = 31;

	public const int IPCAM_X62MF = 32;

	public const int IPDOME = 40;

	public const int IPDOME_MEGA200 = 41;

	public const int IPDOME_MEGA130 = 42;

	public const int IPMOD = 50;

	public const int DS71XX_H = 71;

	public const int DS72XX_H_S = 72;

	public const int DS73XX_H_S = 73;

	public const int DS76XX_H_S = 76;

	public const int DS81XX_HS_S = 81;

	public const int DS81XX_HL_S = 82;

	public const int DS81XX_HC_S = 83;

	public const int DS81XX_HD_S = 84;

	public const int DS81XX_HE_S = 85;

	public const int DS81XX_HF_S = 86;

	public const int DS81XX_AH_S = 87;

	public const int DS81XX_AHF_S = 88;

	public const int DS90XX_HF_S = 90;

	public const int DS91XX_HF_S = 91;

	public const int DS91XX_HD_S = 92;

	public const int NOACTION = 0;

	public const int WARNONMONITOR = 1;

	public const int WARNONAUDIOOUT = 2;

	public const int UPTOCENTER = 4;

	public const int TRIGGERALARMOUT = 8;

	public const int TRIGGERCATPIC = 16;

	public const int SEND_PIC_FTP = 512;

	public const int NCR = 0;

	public const int DIEBOLD = 1;

	public const int WINCOR_NIXDORF = 2;

	public const int SIEMENS = 3;

	public const int OLIVETTI = 4;

	public const int FUJITSU = 5;

	public const int HITACHI = 6;

	public const int SMI = 7;

	public const int IBM = 8;

	public const int BULL = 9;

	public const int YiHua = 10;

	public const int LiDe = 11;

	public const int GDYT = 12;

	public const int Mini_Banl = 13;

	public const int GuangLi = 14;

	public const int DongXin = 15;

	public const int ChenTong = 16;

	public const int NanTian = 17;

	public const int XiaoXing = 18;

	public const int GZYY = 19;

	public const int QHTLT = 20;

	public const int DRS918 = 21;

	public const int KALATEL = 22;

	public const int NCR_2 = 23;

	public const int NXS = 24;

	public const int NET_DEC_STARTDEC = 1;

	public const int NET_DEC_STOPDEC = 2;

	public const int NET_DEC_STOPCYCLE = 3;

	public const int NET_DEC_CONTINUECYCLE = 4;

	public const int MAX_RESOLUTIONNUM = 64;

	public const int NET_DVR_ENCODER_UNKOWN = 0;

	public const int NET_DVR_ENCODER_H264 = 1;

	public const int NET_DVR_ENCODER_S264 = 2;

	public const int NET_DVR_ENCODER_MPEG4 = 3;

	public const int NET_DVR_ORIGINALSTREAM = 4;

	public const int NET_DVR_PICTURE = 5;

	public const int NET_DVR_ENCODER_MJPEG = 6;

	public const int NET_DVR_ECONDER_MPEG2 = 7;

	public const int NET_DVR_STREAM_TYPE_UNKOWN = 0;

	public const int NET_DVR_STREAM_TYPE_HIKPRIVT = 1;

	public const int NET_DVR_STREAM_TYPE_TS = 7;

	public const int NET_DVR_STREAM_TYPE_PS = 8;

	public const int NET_DVR_STREAM_TYPE_RTP = 9;

	public const int NET_DVR_MAX_DISPREGION = 16;

	public const int LOW_DEC_FPS_1_2 = 51;

	public const int LOW_DEC_FPS_1_4 = 52;

	public const int LOW_DEC_FPS_1_8 = 53;

	public const int LOW_DEC_FPS_1_16 = 54;

	public const int MAX_DECODECHANNUM = 32;

	public const int MAX_DISPCHANNUM = 24;

	public const int PASSIVE_DEC_PAUSE = 1;

	public const int PASSIVE_DEC_RESUME = 2;

	public const int PASSIVE_DEC_FAST = 3;

	public const int PASSIVE_DEC_SLOW = 4;

	public const int PASSIVE_DEC_NORMAL = 5;

	public const int PASSIVE_DEC_ONEBYONE = 6;

	public const int PASSIVE_DEC_AUDIO_ON = 7;

	public const int PASSIVE_DEC_AUDIO_OFF = 8;

	public const int PASSIVE_DEC_RESETBUFFER = 9;

	public const int MAX_SUBSYSTEM_NUM = 80;

	public const int MAX_SUBSYSTEM_NUM_V40 = 120;

	public const int MAX_SERIALLEN = 36;

	public const int MAX_LOOPPLANNUM = 16;

	public const int DECODE_TIMESEGMENT = 4;

	public const int MAX_DOMAIN_NAME = 64;

	public const int MAX_DISKNUM_V30 = 33;

	public const int MAX_DAYS = 7;

	public const int MAX_DISPNUM_V41 = 32;

	public const int MAX_WINDOWS_NUM = 12;

	public const int MAX_VOUTNUM = 32;

	public const int MAX_SUPPORT_RES = 32;

	public const int MAX_BIGSCREENNUM = 100;

	public const int VIDEOPLATFORM_ABILITY = 528;

	public const int MATRIXDECODER_ABILITY_V41 = 608;

	public const int NET_DVR_MATRIX_BIGSCREENCFG_GET = 1140;

	public const int MAX_VCA_CHAN = 16;

	public const int MAX_NET_DISK = 16;

	public const int INQUEST_START_INFO = 4097;

	public const int INQUEST_STOP_INFO = 4098;

	public const int INQUEST_TAG_INFO = 4099;

	public const int INQUEST_SEGMENT_INFO = 4100;

	public const int MAX_ID_COUNT = 256;

	public const int MAX_STREAM_ID_COUNT = 1024;

	public const int STREAM_ID_LEN = 32;

	public const int PLAN_ID_LEN = 32;

	public const int SEARCH_EVENT_INFO_LEN = 300;

	public const int MAX_RECT_NUM = 6;

	public const int MAX_LINE_SEG_NUM = 8;

	public const int MAX_SAMPLE_NUM = 5;

	public const int CALIB_PT_NUM = 4;

	public const int MAX_POSITION_NUM = 10;

	public const int MAX_REGION_NUM = 8;

	public const int MAX_TPS_RULE = 8;

	public const int MAX_AID_RULE = 8;

	public const int MAX_LANE_NUM = 8;

	public const int PICNAME_ITEM_DEV_NAME = 1;

	public const int PICNAME_ITEM_DEV_NO = 2;

	public const int PICNAME_ITEM_DEV_IP = 3;

	public const int PICNAME_ITEM_CHAN_NAME = 4;

	public const int PICNAME_ITEM_CHAN_NO = 5;

	public const int PICNAME_ITEM_TIME = 6;

	public const int PICNAME_ITEM_CARDNO = 7;

	public const int PICNAME_ITEM_PLATE_NO = 8;

	public const int PICNAME_ITEM_PLATE_COLOR = 9;

	public const int PICNAME_ITEM_CAR_CHAN = 10;

	public const int PICNAME_ITEM_CAR_SPEED = 11;

	public const int PICNAME_ITEM_CARCHAN = 12;

	public const int PICNAME_ITEM_PIC_NUMBER = 13;

	public const int PICNAME_ITEM_CAR_NUMBER = 14;

	public const int PICNAME_ITEM_SPEED_LIMIT_VALUES = 15;

	public const int PICNAME_ITEM_ILLEGAL_CODE = 16;

	public const int PICNAME_ITEM_CROSS_NUMBER = 17;

	public const int PICNAME_ITEM_DIRECTION_NUMBER = 18;

	public const int PICNAME_MAXITEM = 15;

	public const int PICNAME_ITEM_PARK_DEV_IP = 1;

	public const int PICNAME_ITEM_PARK_PLATE_NO = 2;

	public const int PICNAME_ITEM_PARK_TIME = 3;

	public const int PICNAME_ITEM_PARK_INDEX = 4;

	public const int PICNAME_ITEM_PARK_STATUS = 5;

	public const int IPC_PROTOCOL_NUM = 50;

	public const int MAX_ALERTLINE_NUM = 8;

	public const int MAX_INTRUSIONREGION_NUM = 8;

	public const int MAX_ZEROCHAN_NUM = 16;

	public const int DESC_LEN_64 = 64;

	public const int PROCESSING = 0;

	public const int PROCESS_SUCCESS = 100;

	public const int PROCESS_EXCEPTION = 400;

	public const int PROCESS_FAILED = 500;

	public const int PROCESS_QUICK_SETUP_PD_COUNT = 501;

	public const int SOFTWARE_VERSION_LEN = 48;

	public const int MAX_SADP_NUM = 256;

	public const int DESC_LEN_32 = 32;

	public const int MAX_NODE_NUM = 256;

	public const int BACKUP_SUCCESS = 100;

	public const int BACKUP_CHANGE_DEVICE = 101;

	public const int BACKUP_SEARCH_DEVICE = 300;

	public const int BACKUP_SEARCH_FILE = 301;

	public const int BACKUP_SEARCH_LOG_FILE = 302;

	public const int BACKUP_EXCEPTION = 400;

	public const int BACKUP_FAIL = 500;

	public const int BACKUP_TIME_SEG_NO_FILE = 501;

	public const int BACKUP_NO_RESOURCE = 502;

	public const int BACKUP_DEVICE_LOW_SPACE = 503;

	public const int BACKUP_DISK_FINALIZED = 504;

	public const int BACKUP_DISK_EXCEPTION = 505;

	public const int BACKUP_DEVICE_NOT_EXIST = 506;

	public const int BACKUP_OTHER_BACKUP_WORK = 507;

	public const int BACKUP_USER_NO_RIGHT = 508;

	public const int BACKUP_OPERATE_FAIL = 509;

	public const int BACKUP_NO_LOG_FILE = 510;

	public const int MAX_ABILITYTYPE_NUM = 12;

	public const int MAX_HOLIDAY_NUM = 32;

	public const int MAX_LINK_V30 = 128;

	public const int MAX_BOND_NUM = 2;

	public const int MAX_PIC_EVENT_NUM = 32;

	public const int MAX_ALARMIN_CAPTURE = 16;

	public const int LABEL_NAME_LEN = 40;

	public const int LABEL_IDENTIFY_LEN = 64;

	public const int MAX_DEL_LABEL_IDENTIFY = 20;

	public const int CARDNUM_LEN_V30 = 40;

	public const int PICTURE_NAME_LEN = 64;

	public const int MAX_RECORD_PICTURE_NUM = 50;

	public const int STEP_READY = 0;

	public const int STEP_RECV_DATA = 1;

	public const int STEP_UPGRADE = 2;

	public const int STEP_BACKUP = 3;

	public const int STEP_SEARCH = 255;

	public const int NET_DVR_V6PSUBSYSTEMARAM_GET = 1501;

	public const int NET_DVR_V6PSUBSYSTEMARAM_SET = 1502;

	public const int MAX_REDAREA_NUM = 6;

	public const int INQUEST_MESSAGE_LEN = 44;

	public const int INQUEST_MAX_ROOM_NUM = 2;

	public const int MAX_RESUME_SEGMENT = 2;

	public const int UPLOAD_CERTIFICATE = 1;

	public const int MATRIX_PROTOCOL_NUM = 20;

	public const int KEYBOARD_PROTOCOL_NUM = 20;

	public const int MAX_FACE_PIC_LEN = 6144;

	public const int NOT_AVALIABLE = 0;

	public const int SVGA_60HZ = 52505660;

	public const int SVGA_75HZ = 52505675;

	public const int XGA_60HZ = 67207228;

	public const int XGA_75HZ = 67207243;

	public const int SXGA_60HZ = 84017212;

	public const int SXGA2_60HZ = 84009020;

	public const int _720P_60HZ = 83978300;

	public const int _720P_50HZ = 83978290;

	public const int _1080I_60HZ = 394402876;

	public const int _1080I_50HZ = 394402866;

	public const int _1080P_60HZ = 125967420;

	public const int _1080P_50HZ = 125967410;

	public const int _1080P_30HZ = 125967390;

	public const int _1080P_25HZ = 125967385;

	public const int _1080P_24HZ = 125967384;

	public const int UXGA_60HZ = 105011260;

	public const int UXGA_30HZ = 105011230;

	public const int WSXGA_60HZ = 110234940;

	public const int WUXGA_60HZ = 125982780;

	public const int WUXGA_30HZ = 125982750;

	public const int WXGA_60HZ = 89227324;

	public const int SXGA_PLUS_60HZ = 91884860;

	public const int MAX_WINDOWS = 16;

	public const int MAX_WINDOWS_V41 = 36;

	public const int STARTDISPCHAN_VGA = 1;

	public const int STARTDISPCHAN_BNC = 9;

	public const int STARTDISPCHAN_HDMI = 25;

	public const int STARTDISPCHAN_DVI = 29;

	public const int MAX_BIGSCREENNUM_SCENE = 100;

	public const int NET_DVR_GET_ALLWINCFG = 1503;

	public const int MAX_WIN_COUNT = 224;

	public const int MAX_LAYOUT_COUNT = 16;

	public const int MAX_CAM_COUNT = 224;

	public const int SCREEN_PROTOCOL_NUM = 20;

	public const int MAX_OSDCHAR_NUM = 256;

	public const int MAX_PLAN_ACTION_NUM = 32;

	public const int DAYS_A_WEEK = 7;

	public const int MAX_PLAN_COUNT = 16;

	public const int PULL_DISK_SUCCESS = 1;

	public const int PULL_DISK_FAIL = 2;

	public const int PULL_DISK_PROCESSING = 3;

	public const int PULL_DISK_NO_ARRAY = 4;

	public const int PULL_DISK_NOT_SUPPORT = 5;

	public const int SCAN_RAID_SUC = 1;

	public const int SCAN_RAID_FAIL = 2;

	public const int SCAN_RAID_PROCESSING = 3;

	public const int SCAN_RAID_NOT_SUPPORT = 4;

	public const int SET_CAMERA_TYPE_SUCCESS = 1;

	public const int SET_CAMERA_TYPE_FAIL = 2;

	public const int SET_CAMERA_TYPE_PROCESSING = 3;

	public const int MAX_PRO_PATH = 256;

	public const int MAX_OVERLAP_ITEM_NUM = 50;

	public const int NET_ITS_GET_OVERLAP_CFG = 5072;

	public const int NET_ITS_SET_OVERLAP_CFG = 5073;

	public const int NET_DVR_SHOWLOGO = 1;

	public const int NET_DVR_HIDELOGO = 2;

	public const int DISP_CMD_ENLARGE_WINDOW = 1;

	public const int DISP_CMD_RENEW_WINDOW = 2;

	public const int DS6001_HF_B = 60;

	public const int DS6001_HF_P = 61;

	public const int DS6002_HF_B = 62;

	public const int DS6101_HF_B = 63;

	public const int IDS52XX = 64;

	public const int DS9000_IVS = 65;

	public const int DS8004_AHL_A = 66;

	public const int DS6101_HF_P = 67;

	public const int VCA_DEV_ABILITY = 256;

	public const int VCA_CHAN_ABILITY = 272;

	public const int MATRIXDECODER_ABILITY = 512;

	public const int NET_DVR_SET_PLATECFG = 150;

	public const int NET_DVR_GET_PLATECFG = 151;

	public const int NET_DVR_SET_RULECFG = 152;

	public const int NET_DVR_GET_RULECFG = 153;

	public const int NET_DVR_SET_LF_CFG = 160;

	public const int NET_DVR_GET_LF_CFG = 161;

	public const int NET_DVR_SET_IVMS_STREAMCFG = 162;

	public const int NET_DVR_GET_IVMS_STREAMCFG = 163;

	public const int NET_DVR_SET_VCA_CTRLCFG = 164;

	public const int NET_DVR_GET_VCA_CTRLCFG = 165;

	public const int NET_DVR_SET_VCA_MASK_REGION = 166;

	public const int NET_DVR_GET_VCA_MASK_REGION = 167;

	public const int NET_DVR_SET_VCA_ENTER_REGION = 168;

	public const int NET_DVR_GET_VCA_ENTER_REGION = 169;

	public const int NET_DVR_SET_VCA_LINE_SEGMENT = 170;

	public const int NET_DVR_GET_VCA_LINE_SEGMENT = 171;

	public const int NET_DVR_SET_IVMS_MASK_REGION = 172;

	public const int NET_DVR_GET_IVMS_MASK_REGION = 173;

	public const int NET_DVR_SET_IVMS_ENTER_REGION = 174;

	public const int NET_DVR_GET_IVMS_ENTER_REGION = 175;

	public const int NET_DVR_SET_IVMS_BEHAVIORCFG = 176;

	public const int NET_DVR_GET_IVMS_BEHAVIORCFG = 177;

	public const int NET_DVR_IVMS_SET_SEARCHCFG = 178;

	public const int NET_DVR_IVMS_GET_SEARCHCFG = 179;

	public const int VCA_MAX_POLYGON_POINT_NUM = 10;

	public const int MAX_RULE_NUM = 8;

	public const int MAX_TARGET_NUM = 30;

	public const int MAX_CALIB_PT = 6;

	public const int MIN_CALIB_PT = 4;

	public const int MAX_TIMESEGMENT_2 = 2;

	public const int MAX_LICENSE_LEN = 16;

	public const int MAX_PLATE_NUM = 3;

	public const int MAX_MASK_REGION_NUM = 4;

	public const int MAX_SEGMENT_NUM = 6;

	public const int MIN_SEGMENT_NUM = 3;

	public const int MAX_CHINESE_CHAR_NUM = 64;

	public const int PLATE_INFO_LEN = 1024;

	public const int PLATE_NUM_LEN = 16;

	public const int FILE_NAME_LEN = 256;

	public const int NET_DVR_GET_CCDPARAMCFG = 1067;

	public const int NET_DVR_SET_CCDPARAMCFG = 1068;

	public const int NET_DVR_GET_IMAGEREGION = 1062;

	public const int NET_DVR_SET_IMAGEREGION = 1063;

	public const int NET_DVR_GET_IMAGEPARAM = 1064;

	public const int NET_DVR_SET_IMAGEPARAM = 1065;

	public const int WM_NETERROR = 1126;

	public const int WM_STREAMEND = 1127;

	public const int FILE_HEAD = 0;

	public const int VIDEO_I_FRAME = 1;

	public const int VIDEO_B_FRAME = 2;

	public const int VIDEO_P_FRAME = 3;

	public const int VIDEO_BP_FRAME = 4;

	public const int VIDEO_BBP_FRAME = 5;

	public const int AUDIO_PACKET = 10;

	public const int DATASTREAM_HEAD = 0;

	public const int DATASTREAM_BITBLOCK = 1;

	public const int DATASTREAM_KEYFRAME = 2;

	public const int DATASTREAM_NORMALFRAME = 3;

	public const int MESSAGEVALUE_DISKFULL = 1;

	public const int MESSAGEVALUE_SWITCHDISK = 2;

	public const int MESSAGEVALUE_CREATEFILE = 3;

	public const int MESSAGEVALUE_DELETEFILE = 4;

	public const int MESSAGEVALUE_SWITCHFILE = 5;

	public const int REGIONTYPE = 0;

	public const int MATRIXTYPE = 11;

	public const int DEVICETYPE = 2;

	public const int CHANNELTYPE = 3;

	public const int USERTYPE = 5;

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_Init();

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_Cleanup();

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SetDVRMessage(uint nMessage, IntPtr hWnd);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SetExceptionCallBack_V30(uint nMessage, IntPtr hWnd, EXCEPYIONCALLBACK fExceptionCallBack, IntPtr pUser);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SetDVRMessCallBack(MESSCALLBACK fMessCallBack);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SetDVRMessCallBack_EX(MESSCALLBACKEX fMessCallBack_EX);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SetDVRMessCallBack_NEW(MESSCALLBACKNEW fMessCallBack_NEW);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SetDVRMessageCallBack(MESSAGECALLBACK fMessageCallBack, uint dwUser);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SetDVRMessageCallBack_V30(MSGCallBack fMessageCallBack, IntPtr pUser);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SetConnectTime(uint dwWaitTime, uint dwTryTimes);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SetReconnect(uint dwInterval, int bEnableRecon);

	[DllImport("HCNetSDK.dll")]
	public static extern uint NET_DVR_GetSDKVersion();

	[DllImport("HCNetSDK.dll")]
	public static extern uint NET_DVR_GetSDKBuildVersion();

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_IsSupport();

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_StartListen(string sLocalIP, ushort wLocalPort);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_StopListen();

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_StartListen_V30(string sLocalIP, ushort wLocalPort, MSGCallBack DataCallback, IntPtr pUserData);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_StopListen_V30(int lListenHandle);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_Login(string sDVRIP, ushort wDVRPort, string sUserName, string sPassword, ref NET_DVR_DEVICEINFO lpDeviceInfo);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_Logout(int iUserID);

	[DllImport("HCNetSDK.dll")]
	public static extern uint NET_DVR_GetLastError();

	[DllImport("HCNetSDK.dll")]
	public static extern string NET_DVR_GetErrorMsg(ref int pErrorNo);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SetShowMode(uint dwShowType, uint colorKey);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetDVRIPByResolveSvr(string sServerIP, ushort wServerPort, string sDVRName, ushort wDVRNameLen, string sDVRSerialNumber, ushort wDVRSerialLen, IntPtr pGetIP);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetDVRIPByResolveSvr_EX(string sServerIP, ushort wServerPort, byte[] sDVRName, ushort wDVRNameLen, byte[] sDVRSerialNumber, ushort wDVRSerialLen, byte[] sGetIP, ref uint dwPort);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_RealPlay(int iUserID, ref NET_DVR_CLIENTINFO lpClientInfo);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_SDK_RealPlay(int iUserLogID, ref NET_DVR_CLIENTINFO lpDVRClientInfo);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_RealPlay_V30(int iUserID, ref NET_DVR_CLIENTINFO lpClientInfo, REALDATACALLBACK fRealDataCallBack_V30, IntPtr pUser, uint bBlocked);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_RealPlay_V40(int iUserID, ref NET_DVR_PREVIEWINFO lpPreviewInfo, REALDATACALLBACK fRealDataCallBack_V30, IntPtr pUser);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_StopRealPlay(int iRealHandle);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_RigisterDrawFun(int lRealHandle, DRAWFUN fDrawFun, uint dwUser);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SetPlayerBufNumber(int lRealHandle, uint dwBufNum);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_ThrowBFrame(int lRealHandle, uint dwNum);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SetAudioMode(uint dwMode);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_OpenSound(int lRealHandle);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_CloseSound();

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_OpenSoundShare(int lRealHandle);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_CloseSoundShare(int lRealHandle);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_Volume(int lRealHandle, ushort wVolume);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SaveRealData(int lRealHandle, string sFileName);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_StopSaveRealData(int lRealHandle);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SetRealDataCallBack(int lRealHandle, SETREALDATACALLBACK fRealDataCallBack, uint dwUser);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SetStandardDataCallBack(int lRealHandle, STDDATACALLBACK fStdDataCallBack, uint dwUser);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_CapturePicture(int lRealHandle, string sPicFileName);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_MakeKeyFrame(int lUserID, int lChannel);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_MakeKeyFrameSub(int lUserID, int lChannel);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetPTZCtrl(int lRealHandle);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetPTZCtrl_Other(int lUserID, int lChannel);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_PTZControl(int lRealHandle, uint dwPTZCommand, uint dwStop);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_PTZControl_Other(int lUserID, int lChannel, uint dwPTZCommand, uint dwStop);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_TransPTZ(int lRealHandle, string pPTZCodeBuf, uint dwBufSize);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_TransPTZ_Other(int lUserID, int lChannel, string pPTZCodeBuf, uint dwBufSize);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_PTZPreset(int lRealHandle, uint dwPTZPresetCmd, uint dwPresetIndex);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_PTZPreset_Other(int lUserID, int lChannel, uint dwPTZPresetCmd, uint dwPresetIndex);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_TransPTZ_EX(int lRealHandle, string pPTZCodeBuf, uint dwBufSize);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_PTZControl_EX(int lRealHandle, uint dwPTZCommand, uint dwStop);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_PTZPreset_EX(int lRealHandle, uint dwPTZPresetCmd, uint dwPresetIndex);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_PTZCruise(int lRealHandle, uint dwPTZCruiseCmd, byte byCruiseRoute, byte byCruisePoint, ushort wInput);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_PTZCruise_Other(int lUserID, int lChannel, uint dwPTZCruiseCmd, byte byCruiseRoute, byte byCruisePoint, ushort wInput);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_PTZCruise_EX(int lRealHandle, uint dwPTZCruiseCmd, byte byCruiseRoute, byte byCruisePoint, ushort wInput);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_PTZTrack(int lRealHandle, uint dwPTZTrackCmd);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_PTZTrack_Other(int lUserID, int lChannel, uint dwPTZTrackCmd);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_PTZTrack_EX(int lRealHandle, uint dwPTZTrackCmd);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_PTZControlWithSpeed(int lRealHandle, uint dwPTZCommand, uint dwStop, uint dwSpeed);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_PTZControlWithSpeed_Other(int lUserID, int lChannel, uint dwPTZCommand, uint dwStop, uint dwSpeed);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_PTZControlWithSpeed_EX(int lRealHandle, uint dwPTZCommand, uint dwStop, uint dwSpeed);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetPTZCruise(int lUserID, int lChannel, int lCruiseRoute, ref NET_DVR_CRUISE_RET lpCruiseRet);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_PTZMltTrack(int lRealHandle, uint dwPTZTrackCmd, uint dwTrackIndex);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_PTZMltTrack_Other(int lUserID, int lChannel, uint dwPTZTrackCmd, uint dwTrackIndex);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_PTZMltTrack_EX(int lRealHandle, uint dwPTZTrackCmd, uint dwTrackIndex);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_FindFile(int lUserID, int lChannel, uint dwFileType, ref NET_DVR_TIME lpStartTime, ref NET_DVR_TIME lpStopTime);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_FindNextFile(int lFindHandle, ref NET_DVR_FIND_DATA lpFindData);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_FindClose(int lFindHandle);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_FindNextFile_V30(int lFindHandle, ref NET_DVR_FINDDATA_V30 lpFindData);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_FindNextFile_V40(int lFindHandle, ref NET_DVR_FINDDATA_V40 lpFindData);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_FindFile_V30(int lUserID, ref NET_DVR_FILECOND pFindCond);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_FindFile_V40(int lUserID, ref NET_DVR_FILECOND_V40 pFindCond);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_FindClose_V30(int lFindHandle);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_FindNextFile_Card(int lFindHandle, ref NET_DVR_FINDDATA_CARD lpFindData);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_FindFile_Card(int lUserID, int lChannel, uint dwFileType, ref NET_DVR_TIME lpStartTime, ref NET_DVR_TIME lpStopTime);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_LockFileByName(int lUserID, string sLockFileName);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_UnlockFileByName(int lUserID, string sUnlockFileName);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_PlayBackByName(int lUserID, string sPlayBackFileName, IntPtr hWnd);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_PlayBackByTime(int lUserID, int lChannel, ref NET_DVR_TIME lpStartTime, ref NET_DVR_TIME lpStopTime, IntPtr hWnd);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_PlayBackByTime_V40(int lUserID, ref NET_DVR_VOD_PARA pVodPara);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_PlayBackControl(int lPlayHandle, uint dwControlCode, uint dwInValue, ref uint LPOutValue);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_PlayBackControl_V40(int lPlayHandle, uint dwControlCode, IntPtr lpInBuffer, uint dwInValue, IntPtr lpOutBuffer, ref uint LPOutValue);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_StopPlayBack(int lPlayHandle);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SetPlayDataCallBack(int lPlayHandle, PLAYDATACALLBACK fPlayDataCallBack, uint dwUser);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_PlayBackSaveData(int lPlayHandle, string sFileName);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_StopPlayBackSave(int lPlayHandle);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetPlayBackOsdTime(int lPlayHandle, ref NET_DVR_TIME lpOsdTime);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_PlayBackCaptureFile(int lPlayHandle, string sFileName);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_GetFileByName(int lUserID, string sDVRFileName, string sSavedFileName);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_GetFileByTime(int lUserID, int lChannel, ref NET_DVR_TIME lpStartTime, ref NET_DVR_TIME lpStopTime, string sSavedFileName);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_GetFileByTime_V40(int lUserID, string sSavedFileName, ref NET_DVR_PLAYCOND pDownloadCond);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_StopGetFile(int lFileHandle);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_GetDownloadPos(int lFileHandle);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_GetPlayBackPos(int lPlayHandle);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_Upgrade(int lUserID, string sFileName);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_GetUpgradeState(int lUpgradeHandle);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_GetUpgradeProgress(int lUpgradeHandle);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_CloseUpgradeHandle(int lUpgradeHandle);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SetNetworkEnvironment(uint dwEnvironmentLevel);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_FormatDisk(int lUserID, int lDiskNumber);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetFormatProgress(int lFormatHandle, ref int pCurrentFormatDisk, ref int pCurrentDiskPos, ref int pFormatStatic);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_CloseFormatHandle(int lFormatHandle);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_SetupAlarmChan(int lUserID);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_CloseAlarmChan(int lAlarmHandle);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_SetupAlarmChan_V30(int lUserID);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_CloseAlarmChan_V30(int lAlarmHandle);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_StartVoiceCom(int lUserID, VOICEDATACALLBACK fVoiceDataCallBack, uint dwUser);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_StartVoiceCom_V30(int lUserID, uint dwVoiceChan, bool bNeedCBNoEncData, VOICEDATACALLBACKV30 fVoiceDataCallBack, IntPtr pUser);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SetVoiceComClientVolume(int lVoiceComHandle, ushort wVolume);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_StopVoiceCom(int lVoiceComHandle);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_StartVoiceCom_MR(int lUserID, VOICEDATACALLBACK fVoiceDataCallBack, uint dwUser);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_StartVoiceCom_MR_V30(int lUserID, uint dwVoiceChan, VOICEDATACALLBACKV30 fVoiceDataCallBack, IntPtr pUser);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_VoiceComSendData(int lVoiceComHandle, string pSendBuf, uint dwBufSize);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_ClientAudioStart();

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_ClientAudioStart_V30(VOICEAUDIOSTART fVoiceAudioStart, IntPtr pUser);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_ClientAudioStop();

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_AddDVR(int lUserID);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_AddDVR_V30(int lUserID, uint dwVoiceChan);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_DelDVR(int lUserID);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_DelDVR_V30(int lVoiceHandle);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SerialStart(int lUserID, int lSerialPort, SERIALDATACALLBACK fSerialDataCallBack, uint dwUser);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SerialSend(int lSerialHandle, int lChannel, string pSendBuf, uint dwBufSize);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SerialStop(int lSerialHandle);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SendTo232Port(int lUserID, string pSendBuf, uint dwBufSize);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SendToSerialPort(int lUserID, uint dwSerialPort, uint dwSerialIndex, string pSendBuf, uint dwBufSize);

	[DllImport("HCNetSDK.dll")]
	public static extern IntPtr NET_DVR_InitG722Decoder(int nBitrate);

	[DllImport("HCNetSDK.dll")]
	public static extern void NET_DVR_ReleaseG722Decoder(IntPtr pDecHandle);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_DecodeG722Frame(IntPtr pDecHandle, ref byte pInBuffer, ref byte pOutBuffer);

	[DllImport("HCNetSDK.dll")]
	public static extern IntPtr NET_DVR_InitG722Encoder();

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_EncodeG722Frame(IntPtr pEncodeHandle, ref byte pInBuffer, ref byte pOutBuffer);

	[DllImport("HCNetSDK.dll")]
	public static extern void NET_DVR_ReleaseG722Encoder(IntPtr pEncodeHandle);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_ClickKey(int lUserID, int lKeyIndex);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_StartDVRRecord(int lUserID, int lChannel, int lRecordType);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_StopDVRRecord(int lUserID, int lChannel);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_InitDevice_Card(ref int pDeviceTotalChan);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_ReleaseDevice_Card();

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_InitDDraw_Card(IntPtr hParent, uint colorKey);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_ReleaseDDraw_Card();

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_RealPlay_Card(int lUserID, ref NET_DVR_CARDINFO lpCardInfo, int lChannelNum);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_ResetPara_Card(int lRealHandle, ref NET_DVR_DISPLAY_PARA lpDisplayPara);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_RefreshSurface_Card();

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_ClearSurface_Card();

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_RestoreSurface_Card();

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_OpenSound_Card(int lRealHandle);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_CloseSound_Card(int lRealHandle);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SetVolume_Card(int lRealHandle, ushort wVolume);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_AudioPreview_Card(int lRealHandle, int bEnable);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_GetCardLastError_Card();

	[DllImport("HCNetSDK.dll")]
	public static extern IntPtr NET_DVR_GetChanHandle_Card(int lRealHandle);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_CapturePicture_Card(int lRealHandle, string sPicFileName);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetSerialNum_Card(int lChannelNum, ref uint pDeviceSerialNo);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_FindDVRLog(int lUserID, int lSelectMode, uint dwMajorType, uint dwMinorType, ref NET_DVR_TIME lpStartTime, ref NET_DVR_TIME lpStopTime);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_FindNextLog(int lLogHandle, ref NET_DVR_LOG lpLogData);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_FindLogClose(int lLogHandle);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_FindDVRLog_V30(int lUserID, int lSelectMode, uint dwMajorType, uint dwMinorType, ref NET_DVR_TIME lpStartTime, ref NET_DVR_TIME lpStopTime, bool bOnlySmart);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_FindNextLog_V30(int lLogHandle, ref NET_DVR_LOG_V30 lpLogData);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_FindLogClose_V30(int lLogHandle);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_FindFileByCard(int lUserID, int lChannel, uint dwFileType, int nFindType, ref byte sCardNumber, ref NET_DVR_TIME lpStartTime, ref NET_DVR_TIME lpStopTime);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_CaptureJPEGPicture(int lUserID, int lChannel, ref NET_DVR_JPEGPARA lpJpegPara, string sPicFileName);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_CaptureJPEGPicture_NEW(int lUserID, int lChannel, ref NET_DVR_JPEGPARA lpJpegPara, byte[] sJpegPicBuffer, uint dwPicSize, ref uint lpSizeReturned);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_GetRealPlayerIndex(int lRealHandle);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_GetPlayBackPlayerIndex(int lPlayHandle);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SetScaleCFG(int lUserID, uint dwScale);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetScaleCFG(int lUserID, ref uint lpOutScale);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SetScaleCFG_V30(int lUserID, ref NET_DVR_SCALECFG pScalecfg);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetScaleCFG_V30(int lUserID, ref NET_DVR_SCALECFG pScalecfg);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SetATMPortCFG(int lUserID, ushort wATMPort);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetATMPortCFG(int lUserID, ref ushort LPOutATMPort);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_InitDDrawDevice();

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_ReleaseDDrawDevice();

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_GetDDrawDeviceTotalNums();

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SetDDrawDevice(int lPlayPort, uint nDeviceNum);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_PTZSelZoomIn(int lRealHandle, ref NET_DVR_POINT_FRAME pStruPointFrame);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_PTZSelZoomIn_EX(int lUserID, int lChannel, ref NET_DVR_POINT_FRAME pStruPointFrame);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_StartDecode(int lUserID, int lChannel, ref NET_DVR_DECODERINFO lpDecoderinfo);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_StopDecode(int lUserID, int lChannel);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetDecoderState(int lUserID, int lChannel, ref NET_DVR_DECODERSTATE lpDecoderState);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SetDecInfo(int lUserID, int lChannel, ref NET_DVR_DECCFG lpDecoderinfo);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetDecInfo(int lUserID, int lChannel, ref NET_DVR_DECCFG lpDecoderinfo);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SetDecTransPort(int lUserID, ref NET_DVR_PORTCFG lpTransPort);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetDecTransPort(int lUserID, ref NET_DVR_PORTCFG lpTransPort);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_DecPlayBackCtrl(int lUserID, int lChannel, uint dwControlCode, uint dwInValue, ref uint LPOutValue, ref NET_DVR_PLAYREMOTEFILE lpRemoteFileInfo);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_StartDecSpecialCon(int lUserID, int lChannel, ref NET_DVR_DECCHANINFO lpDecChanInfo);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_StopDecSpecialCon(int lUserID, int lChannel, ref NET_DVR_DECCHANINFO lpDecChanInfo);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_DecCtrlDec(int lUserID, int lChannel, uint dwControlCode);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_DecCtrlScreen(int lUserID, int lChannel, uint dwControl);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetDecCurLinkStatus(int lUserID, int lChannel, ref NET_DVR_DECSTATUS lpDecStatus);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_MatrixStartDynamic(int lUserID, uint dwDecChanNum, ref NET_DVR_MATRIX_DYNAMIC_DEC lpDynamicInfo);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_MatrixStopDynamic(int lUserID, uint dwDecChanNum);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_MatrixGetDecChanInfo(int lUserID, uint dwDecChanNum, ref NET_DVR_MATRIX_DEC_CHAN_INFO lpInter);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_MatrixSetLoopDecChanInfo(int lUserID, uint dwDecChanNum, ref NET_DVR_MATRIX_LOOP_DECINFO lpInter);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_MatrixGetLoopDecChanInfo(int lUserID, uint dwDecChanNum, ref NET_DVR_MATRIX_LOOP_DECINFO lpInter);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_MatrixSetLoopDecChanEnable(int lUserID, uint dwDecChanNum, uint dwEnable);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_MatrixGetLoopDecChanEnable(int lUserID, uint dwDecChanNum, ref uint lpdwEnable);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_MatrixGetLoopDecEnable(int lUserID, ref uint lpdwEnable);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_MatrixSetDecChanEnable(int lUserID, uint dwDecChanNum, uint dwEnable);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_MatrixGetDecChanEnable(int lUserID, uint dwDecChanNum, ref uint lpdwEnable);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_MatrixGetDecChanStatus(int lUserID, uint dwDecChanNum, ref NET_DVR_MATRIX_DEC_CHAN_STATUS lpInter);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_MatrixSetTranInfo(int lUserID, ref NET_DVR_MATRIX_TRAN_CHAN_CONFIG lpTranInfo);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_MatrixGetTranInfo(int lUserID, ref NET_DVR_MATRIX_TRAN_CHAN_CONFIG lpTranInfo);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_MatrixSetRemotePlay(int lUserID, uint dwDecChanNum, ref NET_DVR_MATRIX_DEC_REMOTE_PLAY lpInter);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_MatrixSetRemotePlayControl(int lUserID, uint dwDecChanNum, uint dwControlCode, uint dwInValue, ref uint LPOutValue);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_MatrixGetRemotePlayStatus(int lUserID, uint dwDecChanNum, ref NET_DVR_MATRIX_DEC_REMOTE_PLAY_STATUS lpOuter);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_MatrixStartDynamic_V30(int lUserID, uint dwDecChanNum, ref NET_DVR_PU_STREAM_CFG lpDynamicInfo);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_MatrixSetLoopDecChanInfo_V30(int lUserID, uint dwDecChanNum, ref NET_DVR_MATRIX_LOOP_DECINFO_V30 lpInter);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_MatrixGetLoopDecChanInfo_V30(int lUserID, uint dwDecChanNum, ref NET_DVR_MATRIX_LOOP_DECINFO_V30 lpInter);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_MatrixGetDecChanInfo_V30(int lUserID, uint dwDecChanNum, ref NET_DVR_MATRIX_DEC_CHAN_INFO_V30 lpInter);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_MatrixSetTranInfo_V30(int lUserID, ref NET_DVR_MATRIX_TRAN_CHAN_CONFIG_V30 lpTranInfo);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_MatrixGetTranInfo_V30(int lUserID, ref NET_DVR_MATRIX_TRAN_CHAN_CONFIG_V30 lpTranInfo);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_MatrixGetDisplayCfg(int lUserID, uint dwDispChanNum, ref NET_DVR_VGA_DISP_CHAN_CFG lpDisplayCfg);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_MatrixSetDisplayCfg(int lUserID, uint dwDispChanNum, ref NET_DVR_VGA_DISP_CHAN_CFG lpDisplayCfg);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_MatrixStartPassiveDecode(int lUserID, uint dwDecChanNum, ref NET_DVR_MATRIX_PASSIVEMODE lpPassiveMode);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_MatrixSendData(int lPassiveHandle, IntPtr pSendBuf, uint dwBufSize);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_MatrixStopPassiveDecode(int lPassiveHandle);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_UploadLogo(int lUserID, uint dwDispChanNum, ref NET_DVR_DISP_LOGOCFG lpDispLogoCfg, IntPtr sLogoBuffer);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_LogoSwitch(int lUserID, uint dwDecChan, uint dwLogoSwitch);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_MatrixGetDeviceStatus(int lUserID, ref NET_DVR_DECODER_WORK_STATUS lpDecoderCfg);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_RigisterPlayBackDrawFun(int lRealHandle, DRAWFUN fDrawFun, uint dwUser);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_MatrixDiaplayControl(int lUserID, uint dwDispChanNum, uint dwDispChanCmd, uint dwCmdParam);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_RefreshPlay(int lPlayHandle);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_RestoreConfig(int lUserID);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SaveConfig(int lUserID);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_RebootDVR(int lUserID);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_ShutDownDVR(int lUserID);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetDVRConfig(int lUserID, uint dwCommand, int lChannel, IntPtr lpOutBuffer, uint dwOutBufferSize, ref uint lpBytesReturned);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SetDVRConfig(int lUserID, uint dwCommand, int lChannel, IntPtr lpInBuffer, uint dwInBufferSize);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetDVRWorkState_V30(int lUserID, IntPtr pWorkState);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetDVRWorkState(int lUserID, ref NET_DVR_WORKSTATE lpWorkState);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SetVideoEffect(int lUserID, int lChannel, uint dwBrightValue, uint dwContrastValue, uint dwSaturationValue, uint dwHueValue);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetVideoEffect(int lUserID, int lChannel, ref uint pBrightValue, ref uint pContrastValue, ref uint pSaturationValue, ref uint pHueValue);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_ClientGetframeformat(int lUserID, ref NET_DVR_FRAMEFORMAT lpFrameFormat);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_ClientSetframeformat(int lUserID, ref NET_DVR_FRAMEFORMAT lpFrameFormat);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetAtmProtocol(int lUserID, ref NET_DVR_ATM_PROTOCOL lpAtmProtocol);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetAlarmOut_V30(int lUserID, IntPtr lpAlarmOutState);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetAlarmOut(int lUserID, ref NET_DVR_ALARMOUTSTATUS lpAlarmOutState);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SetAlarmOut(int lUserID, int lAlarmOutPort, int lAlarmOutStatic);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetUpnpNatState(int lUserID, ref NET_DVR_UPNP_NAT_STATE lpState);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_ClientSetVideoEffect(int lRealHandle, uint dwBrightValue, uint dwContrastValue, uint dwSaturationValue, uint dwHueValue);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_ClientGetVideoEffect(int lRealHandle, ref uint pBrightValue, ref uint pContrastValue, ref uint pSaturationValue, ref uint pHueValue);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetConfigFile(int lUserID, string sFileName);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SetConfigFile(int lUserID, string sFileName);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetConfigFile_V30(int lUserID, string sOutBuffer, uint dwOutSize, ref uint pReturnSize);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetConfigFile_EX(int lUserID, string sOutBuffer, uint dwOutSize);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SetConfigFile_EX(int lUserID, string sInBuffer, uint dwInSize);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SetLogToFile(int bLogEnable, string strLogDir, bool bAutoDel);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetSDKState(ref NET_DVR_SDKSTATE pSDKState);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetSDKAbility(ref NET_DVR_SDKABL pSDKAbl);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetPTZProtocol(int lUserID, ref NET_DVR_PTZCFG pPtzcfg);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_LockPanel(int lUserID);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_UnLockPanel(int lUserID);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SetRtspConfig(int lUserID, uint dwCommand, ref NET_DVR_RTSPCFG lpInBuffer, uint dwInBufferSize);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetRtspConfig(int lUserID, uint dwCommand, ref NET_DVR_RTSPCFG lpOutBuffer, uint dwOutBufferSize);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_MatrixGetSceneCfg(int lUserID, uint dwSceneNum, ref NET_DVR_MATRIX_SCENECFG lpSceneCfg);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_MatrixSetSceneCfg(int lUserID, uint dwSceneNum, ref NET_DVR_MATRIX_SCENECFG lpSceneCfg);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetDeviceAbility(int lUserID, uint dwAbilityType, IntPtr pInBuf, uint dwInLength, IntPtr pOutBuf, uint dwOutLength);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SetBehaviorParamKey(int lUserID, int lChannel, uint dwParameterKey, int nValue);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetBehaviorParamKey(int lUserID, int lChannel, uint dwParameterKey, ref int pValue);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetVCADrawMode(int lUserID, int lChannel, ref NET_VCA_DRAW_MODE lpDrawMode);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SetVCADrawMode(int lUserID, int lChannel, ref NET_VCA_DRAW_MODE lpDrawMode);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SetLFTrackMode(int lUserID, int lChannel, ref NET_DVR_LF_TRACK_MODE lpTrackMode);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetLFTrackMode(int lUserID, int lChannel, ref NET_DVR_LF_TRACK_MODE lpTrackMode);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_VCA_RestartLib(int lUserID, int lChannel);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetRealHeight(int lUserID, int lChannel, ref NET_VCA_LINE lpLine, ref float lpHeight);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetRealLength(int lUserID, int lChannel, ref NET_VCA_LINE lpLine, ref float lpLength);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SaveRealData_V30(int lRealHandle, uint dwTransType, string sFileName);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_EncodeG711Frame(uint iType, ref byte pInBuffer, ref byte pOutBuffer);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_DecodeG711Frame(uint iType, ref byte pInBuffer, ref byte pOutBuffer);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_EmailTest(int lUserID);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_FindFileByEvent(int lUserID, ref NET_DVR_SEARCH_EVENT_PARAM lpSearchEventParam);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_FindNextEvent(int lSearchHandle, ref NET_DVR_SEARCH_EVENT_RET lpSearchEventRet);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_INVOKE_PLATE_RECOGNIZE(int lUserID, int lChannel, string pPicFileName, ref NET_DVR_PLATE_RET pPlateRet, string pPicBuf, uint dwPicBufLen);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetCCDCfg(int lUserID, int lChannel, ref NET_DVR_CCD_CFG lpCCDCfg);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_SetCCDCfg(int lUserID, int lChannel, ref NET_DVR_CCD_CFG lpCCDCfg);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_GetParamSetMode(int lUserID, ref uint dwParamSetMode);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_Login_V30(string sDVRIP, int wDVRPort, string sUserName, string sPassword, ref NET_DVR_DEVICEINFO_V30 lpDeviceInfo);

	[DllImport("HCNetSDK.dll")]
	public static extern bool NET_DVR_Logout_V30(int lUserID);

	[DllImport("GetStream.dll")]
	public static extern bool CLIENT_SDK_Init();

	[DllImport("GetStream.dll")]
	public static extern bool CLIENT_SDK_UnInit();

	[DllImport("GetStream.dll")]
	public static extern int CLIENT_SDK_GetStream(PLAY_INFO lpPlayInfo);

	[DllImport("GetStream.dll")]
	public static extern bool CLIENT_SetRealDataCallBack(int iRealHandle, SETREALDATACALLBACK fRealDataCallBack, uint lUser);

	[DllImport("GetStream.dll")]
	public static extern bool CLIENT_SDK_StopStream(int iRealHandle);

	[DllImport("GetStream.dll")]
	public static extern bool CLIENT_SDK_GetVideoEffect(int iRealHandle, ref int iBrightValue, ref int iContrastValue, ref int iSaturationValue, ref int iHueValue);

	[DllImport("GetStream.dll")]
	public static extern bool CLIENT_SDK_SetVideoEffect(int iRealHandle, int iBrightValue, int iContrastValue, int iSaturationValue, int iHueValue);

	[DllImport("GetStream.dll")]
	public static extern bool CLIENT_SDK_MakeKeyFrame(int iRealHandle);

	[DllImport("PdCssVodClient.dll")]
	public static extern bool VODServerConnect(string strServerIp, uint uiServerPort, ref IntPtr hSession, ref CONNPARAM struConn, IntPtr hWnd);

	[DllImport("PdCssVodClient.dll")]
	public static extern bool VODServerDisconnect(IntPtr hSession);

	[DllImport("PdCssVodClient.dll")]
	public static extern bool VODStreamSearch(IntPtr pSearchParam, ref IntPtr pSecList);

	[DllImport("PdCssVodClient.dll")]
	public static extern bool VODDeleteSectionList(IntPtr pSecList);

	[DllImport("PdCssVodClient.dll")]
	public static extern bool VODOpenStream(IntPtr pOpenParam, ref IntPtr phStream);

	[DllImport("PdCssVodClient.dll")]
	public static extern bool VODCloseStream(IntPtr hStream);

	[DllImport("PdCssVodClient.dll")]
	public static extern bool VODOpenDownloadStream(ref VODOPENPARAM struVodParam, ref IntPtr phStream);

	[DllImport("PdCssVodClient.dll")]
	public static extern bool VODCloseDownloadStream(IntPtr hStream);

	[DllImport("PdCssVodClient.dll")]
	public static extern bool VODStartStreamData(IntPtr phStream);

	[DllImport("PdCssVodClient.dll")]
	public static extern bool VODPauseStreamData(IntPtr hStream, bool bPause);

	[DllImport("PdCssVodClient.dll")]
	public static extern bool VODStopStreamData(IntPtr hStream);

	[DllImport("PdCssVodClient.dll")]
	public static extern bool VODSeekStreamData(IntPtr hStream, IntPtr pStartTime);

	[DllImport("PdCssVodClient.dll")]
	public static extern bool VODSetStreamSpeed(IntPtr hStream, int iSpeed);

	[DllImport("PdCssVodClient.dll")]
	public static extern bool VODGetStreamCurrentTime(IntPtr hStream, ref BLOCKTIME pCurrentTime);

	[DllImport("AnalyzeData.dll")]
	public static extern int AnalyzeDataGetSafeHandle();

	[DllImport("AnalyzeData.dll")]
	public static extern bool AnalyzeDataOpenStreamEx(int iHandle, byte[] pFileHead);

	[DllImport("AnalyzeData.dll")]
	public static extern bool AnalyzeDataClose(int iHandle);

	[DllImport("AnalyzeData.dll")]
	public static extern bool AnalyzeDataInputData(int iHandle, IntPtr pBuffer, uint uiSize);

	[DllImport("AnalyzeData.dll")]
	public static extern int AnalyzeDataGetPacket(int iHandle, ref PACKET_INFO pPacketInfo);

	[DllImport("AnalyzeData.dll")]
	public static extern bool AnalyzeDataGetTail(int iHandle, ref IntPtr pBuffer, ref uint uiSize);

	[DllImport("AnalyzeData.dll")]
	public static extern uint AnalyzeDataGetLastError(int iHandle);

	[DllImport("RecordDLL.dll")]
	public static extern int Initialize(STOREINFO struStoreInfo);

	[DllImport("RecordDLL.dll")]
	public static extern int Release();

	[DllImport("RecordDLL.dll")]
	public static extern int OpenChannelRecord(string strCameraid, IntPtr pHead, uint dwHeadLength);

	[DllImport("RecordDLL.dll")]
	public static extern bool CloseChannelRecord(int iRecordHandle);

	[DllImport("RecordDLL.dll")]
	public static extern int GetData(int iHandle, int iDataType, IntPtr pBuf, uint uiSize);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_FindNextLog_MATRIX(int iLogHandle, ref NET_DVR_LOG_MATRIX lpLogData);

	[DllImport("HCNetSDK.dll")]
	public static extern int NET_DVR_FindDVRLog_Matrix(int iUserID, int lSelectMode, uint dwMajorType, uint dwMinorType, ref tagVEDIOPLATLOG lpVedioPlatLog, ref NET_DVR_TIME lpStartTime, ref NET_DVR_TIME lpStopTime);
}
