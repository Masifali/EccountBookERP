package com.mst.models.hrm.device;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.DeviceManagement.mmProductDevice (model 0726) - every property, as
 * GenericProvider.SetProc sends it to Sp_mmProductDevice_Insert / Sp_mmProductDevice_Update
 * (58 parameters; every one is declared by both procedures - checked with proc.py).
 * frmDeviceManagement fills only Location / Device Name / Port / IP / IsActive and the audit fields;
 * the rest keep the CLR defaults (0 / false / null) the desktop sends.
 */
public class MmProductDevice extends DesktopModel {
    public int ActionTypeId;
    public boolean IsActive;
    public boolean IsClearLog;
    public boolean IsServing;
    public LocalDateTime AlteredOn;
    public LocalDateTime CreatedOn;
    public int BaudRateProfileId;
    public int CommCodeProfileId;
    public int CommModeProfileId;
    public int CommProtocolProfileId;
    public int CommTypeProfileId;
    public int DataBitProfileId;
    public int DICOMFileCounter;
    public int HandShakeProfileId;
    public int ParityBitProfileId;
    public int ProductDeviceId;
    public int StartBitProfileId;
    public int StopBitProfileId;
    public int OrganizationId;
    public int CompanyId;
    public long AlteredById;
    public long BranchId;
    public long CreatedById;
    public long LocationId;
    public long PartyLocationId;
    public long ProductId;
    public long RoomPortionId;
    public long UserLogId;
    public String ADSAccessPath;
    public String ADSUserName;
    public String ADSUserPassword;
    public String CableConfigFilePath;
    public String CompressedFolderPath;
    public String DestinationFilePath;
    public String DestinationFolderPath;
    public String DeviceLocationText;
    public String DeviceName;
    public String DICOMFileExtension;
    public String DICOMFileFormat;
    public String DICOMHostingURL;
    public String FileExtension;
    public String FileHostingURL;
    public String FinalFilePath;
    public String HostInterfaceFilePath;
    public String MWLAET;
    public String MWLIP;
    public String MWLPort;
    public String NetworkAET;
    public String NetworkIP;
    public String NetworkPort;
    public String OrphanFolderPath;
    public String ProcessFilePath;
    public String ProcessFolderPath;
    public String ReaconFolderPath;
    public String ResultFilePath;
    public String SharePath;
    public String SourceFolderPath;
    public String WANIP;
}
