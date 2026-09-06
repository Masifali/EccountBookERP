using System;
using EDSDKLib;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace CameraControl
{

public class CameraModel : Observable
{
	public enum Status
	{
		NONE,
		DOWNLOADING,
		DELETEING,
		CANCELING
	}

	public Status _ExecuteStatus;

	public IntPtr Camera { get; set; }

	public string ModelName { get; set; }

	public bool isTypeDS { get; set; }

	public uint AEMode { get; set; }

	public uint AFMode { get; set; }

	public uint DriveMode { get; set; }

	public uint WhiteBalance { get; set; }

	public uint Av { get; set; }

	public uint Tv { get; set; }

	public uint Iso { get; set; }

	public uint MeteringMode { get; set; }

	public uint ExposureCompensation { get; set; }

	public uint ImageQuality { get; set; }

	public uint AvailableShot { get; set; }

	public uint EvfMode { get; set; }

	public uint StartupEvfOutputDevice { get; set; }

	public uint EvfOutputDevice { get; set; }

	public uint EvfDepthOfFieldPreview { get; set; }

	public uint EvfAFMode { get; set; }

	public EDSDK.EdsFocusInfo FocusInfo { get; set; }

	public uint BatteryLebel { get; set; }

	public uint Zoom { get; set; }

	public EDSDK.EdsRect ZoomRect { get; set; }

	public uint FlashMode { get; set; }

	public bool canDownloadImage { get; set; }

	public bool isEvfEnable { get; set; }

	public uint TempStatus { get; set; }

	public uint RollPitch { get; set; }

	public uint MovieQuality { get; set; }

	public uint MovieHFR { get; set; }

	public uint PictureStyle { get; set; }

	public uint Aspect { get; set; }

	public EDSDK.EdsRect VisibleRect { get; set; }

	public uint FixedMovie { get; set; }

	public uint MirrorUpSetting { get; set; }

	public uint MirrorLockUpState { get; set; }

	public byte[] ClickWB { get; set; }

	public int ClickPoint { get; set; }

	public uint AutoPowerOff { get; set; }

	public EDSDK.EdsSize SizeJpegLarge { get; set; }

	public EDSDK.EdsPropertyDesc AEModeDesc { get; set; }

	public EDSDK.EdsPropertyDesc DriveModeDesc { get; set; }

	public EDSDK.EdsPropertyDesc WhiteBalanceDesc { get; set; }

	public EDSDK.EdsPropertyDesc AvDesc { get; set; }

	public EDSDK.EdsPropertyDesc TvDesc { get; set; }

	public EDSDK.EdsPropertyDesc IsoDesc { get; set; }

	public EDSDK.EdsPropertyDesc MeteringModeDesc { get; set; }

	public EDSDK.EdsPropertyDesc ExposureCompensationDesc { get; set; }

	public EDSDK.EdsPropertyDesc ImageQualityDesc { get; set; }

	public EDSDK.EdsPropertyDesc EvfAFModeDesc { get; set; }

	public EDSDK.EdsPropertyDesc ZoomDesc { get; set; }

	public EDSDK.EdsPropertyDesc FlashModeDesc { get; set; }

	public EDSDK.EdsPropertyDesc MovieQualityDesc { get; set; }

	public EDSDK.EdsPropertyDesc MovieHFRDesc { get; set; }

	public EDSDK.EdsPropertyDesc PictureStyleDesc { get; set; }

	public EDSDK.EdsPropertyDesc AspectDesc { get; set; }

	public EDSDK.EdsPropertyDesc AutoPowerOffDesc { get; set; }

	public CameraModel(IntPtr camera)
	{
		Camera = camera;
		AEMode = uint.MaxValue;
		AFMode = uint.MaxValue;
		DriveMode = uint.MaxValue;
		WhiteBalance = uint.MaxValue;
		Av = uint.MaxValue;
		Tv = uint.MaxValue;
		Iso = uint.MaxValue;
		MeteringMode = uint.MaxValue;
		ExposureCompensation = uint.MaxValue;
		ImageQuality = uint.MaxValue;
		EvfMode = uint.MaxValue;
		EvfOutputDevice = uint.MaxValue;
		EvfDepthOfFieldPreview = uint.MaxValue;
		EvfAFMode = uint.MaxValue;
		BatteryLebel = uint.MaxValue;
		Zoom = uint.MaxValue;
		FlashMode = uint.MaxValue;
		AvailableShot = 0u;
		canDownloadImage = true;
		TempStatus = uint.MaxValue;
		RollPitch = 1u;
		MovieQuality = uint.MaxValue;
		MovieHFR = uint.MaxValue;
		PictureStyle = uint.MaxValue;
		Aspect = uint.MaxValue;
		FixedMovie = uint.MaxValue;
		MirrorUpSetting = uint.MaxValue;
		MirrorLockUpState = uint.MaxValue;
		AutoPowerOff = uint.MaxValue;
	}

	public void SetPropertyUInt32(uint propertyID, uint value)
	{
		switch (propertyID)
		{
		case 1078u:
			AEMode = value;
			break;
		case 1028u:
			AFMode = value;
			break;
		case 1025u:
			DriveMode = value;
			break;
		case 1030u:
			Tv = value;
			break;
		case 1029u:
			Av = value;
			break;
		case 1026u:
			Iso = value;
			break;
		case 1027u:
			MeteringMode = value;
			break;
		case 1031u:
			ExposureCompensation = value;
			break;
		case 256u:
			ImageQuality = value;
			break;
		case 1281u:
			EvfMode = value;
			break;
		case 1280u:
			if (EvfOutputDevice == uint.MaxValue)
			{
				StartupEvfOutputDevice = value;
			}
			EvfOutputDevice = value;
			break;
		case 1284u:
			EvfDepthOfFieldPreview = value;
			break;
		case 1294u:
			EvfAFMode = value;
			break;
		case 1034u:
			AvailableShot = value;
			break;
		case 1536u:
			Zoom = value;
			break;
		case 1537u:
			FlashMode = value;
			break;
		case 16778261u:
			TempStatus = value;
			break;
		case 276u:
			PictureStyle = value;
			break;
		case 16778333u:
			MovieHFR = value;
			break;
		case 16778289u:
			Aspect = value;
			break;
		case 16778274u:
			FixedMovie = value;
			break;
		case 16778296u:
			MirrorUpSetting = value;
			break;
		case 16778273u:
			MirrorLockUpState = value;
			break;
		case 16778334u:
			AutoPowerOff = value;
			break;
		}
	}

	public void SetPropertyInt32(uint propertyID, uint value)
	{
		switch (propertyID)
		{
		case 262u:
			WhiteBalance = value;
			break;
		case 8u:
			BatteryLebel = value;
			break;
		}
	}

	public void SetPropertyString(uint propertyID, string str)
	{
		if (propertyID == 2)
		{
			ModelName = str;
			isTypeDS = str.Contains("EOS");
		}
	}

	public void SetPropertyFocusInfo(uint propertyID, EDSDK.EdsFocusInfo info)
	{
		if (propertyID == 260)
		{
			FocusInfo = info;
		}
	}

	public void SetPropertyByteBlock(uint propertyID, byte[] data)
	{
		switch (propertyID)
		{
		case 16778275u:
			MovieQuality = BitConverter.ToUInt32(data, 0);
			break;
		case 16778502u:
			ClickWB = data;
			break;
		}
	}

	public void SetPropertyRect(uint propertyID, EDSDK.EdsRect info)
	{
		switch (propertyID)
		{
		case 1345u:
			ZoomRect = info;
			break;
		case 16778566u:
			VisibleRect = info;
			break;
		}
	}

	public void SetPropertyDesc(uint propertyID, ref EDSDK.EdsPropertyDesc desc)
	{
		switch (propertyID)
		{
		case 1078u:
			AEModeDesc = desc;
			break;
		case 1025u:
			DriveModeDesc = desc;
			break;
		case 262u:
			WhiteBalanceDesc = desc;
			break;
		case 1030u:
			TvDesc = desc;
			break;
		case 1029u:
			AvDesc = desc;
			break;
		case 1026u:
			IsoDesc = desc;
			break;
		case 1027u:
			MeteringModeDesc = desc;
			break;
		case 1031u:
			ExposureCompensationDesc = desc;
			break;
		case 256u:
			ImageQualityDesc = desc;
			break;
		case 1294u:
			EvfAFModeDesc = desc;
			break;
		case 1536u:
			ZoomDesc = desc;
			break;
		case 1537u:
			FlashModeDesc = desc;
			break;
		case 16778275u:
			MovieQualityDesc = desc;
			break;
		case 16778333u:
			MovieHFRDesc = desc;
			break;
		case 276u:
			PictureStyleDesc = desc;
			break;
		case 16778289u:
			AspectDesc = desc;
			break;
		case 16778334u:
			AutoPowerOffDesc = desc;
			break;
		}
	}

	public EDSDK.EdsPropertyDesc GetPropertyDesc(uint propertyID)
	{
		EDSDK.EdsPropertyDesc desc = default(EDSDK.EdsPropertyDesc);
		switch (propertyID)
		{
		case 1078u:
			desc = AEModeDesc;
			break;
		case 1025u:
			desc = DriveModeDesc;
			break;
		case 262u:
			desc = WhiteBalanceDesc;
			break;
		case 1030u:
			desc = TvDesc;
			break;
		case 1029u:
			desc = AvDesc;
			break;
		case 1026u:
			desc = IsoDesc;
			break;
		case 1027u:
			desc = MeteringModeDesc;
			break;
		case 1031u:
			desc = ExposureCompensationDesc;
			break;
		case 256u:
			desc = ImageQualityDesc;
			break;
		case 1294u:
			desc = EvfAFModeDesc;
			break;
		case 1536u:
			desc = ZoomDesc;
			break;
		case 1537u:
			desc = FlashModeDesc;
			break;
		case 16778275u:
			desc = MovieQualityDesc;
			break;
		case 16778333u:
			desc = MovieHFRDesc;
			break;
		case 276u:
			desc = PictureStyleDesc;
			break;
		case 16778289u:
			desc = AspectDesc;
			break;
		case 16778334u:
			desc = AutoPowerOffDesc;
			break;
		}
		return desc;
	}

	public EDSDK.EdsPoint GetZoomPosition()
	{
		EDSDK.EdsPoint zoomPosition = default(EDSDK.EdsPoint);
		zoomPosition.x = ZoomRect.x;
		zoomPosition.y = ZoomRect.y;
		return zoomPosition;
	}
}

}
