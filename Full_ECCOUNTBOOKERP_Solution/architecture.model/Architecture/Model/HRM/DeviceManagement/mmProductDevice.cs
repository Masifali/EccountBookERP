using System;

namespace Architecture.Model.HRM.DeviceManagement;

public class mmProductDevice
{
	public int ActionTypeId { get; set; }

	public bool IsActive { get; set; }

	public bool IsClearLog { get; set; }

	public bool IsServing { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public int BaudRateProfileId { get; set; }

	public int CommCodeProfileId { get; set; }

	public int CommModeProfileId { get; set; }

	public int CommProtocolProfileId { get; set; }

	public int CommTypeProfileId { get; set; }

	public int DataBitProfileId { get; set; }

	public int DICOMFileCounter { get; set; }

	public int HandShakeProfileId { get; set; }

	public int ParityBitProfileId { get; set; }

	public int ProductDeviceId { get; set; }

	public int StartBitProfileId { get; set; }

	public int StopBitProfileId { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public long AlteredById { get; set; }

	public long BranchId { get; set; }

	public long CreatedById { get; set; }

	public long LocationId { get; set; }

	public long PartyLocationId { get; set; }

	public long ProductId { get; set; }

	public long RoomPortionId { get; set; }

	public long UserLogId { get; set; }

	public string ADSAccessPath { get; set; }

	public string ADSUserName { get; set; }

	public string ADSUserPassword { get; set; }

	public string CableConfigFilePath { get; set; }

	public string CompressedFolderPath { get; set; }

	public string DestinationFilePath { get; set; }

	public string DestinationFolderPath { get; set; }

	public string DeviceLocationText { get; set; }

	public string DeviceName { get; set; }

	public string DICOMFileExtension { get; set; }

	public string DICOMFileFormat { get; set; }

	public string DICOMHostingURL { get; set; }

	public string FileExtension { get; set; }

	public string FileHostingURL { get; set; }

	public string FinalFilePath { get; set; }

	public string HostInterfaceFilePath { get; set; }

	public string MWLAET { get; set; }

	public string MWLIP { get; set; }

	public string MWLPort { get; set; }

	public string NetworkAET { get; set; }

	public string NetworkIP { get; set; }

	public string NetworkPort { get; set; }

	public string OrphanFolderPath { get; set; }

	public string ProcessFilePath { get; set; }

	public string ProcessFolderPath { get; set; }

	public string ReaconFolderPath { get; set; }

	public string ResultFilePath { get; set; }

	public string SharePath { get; set; }

	public string SourceFolderPath { get; set; }

	public string WANIP { get; set; }
}
