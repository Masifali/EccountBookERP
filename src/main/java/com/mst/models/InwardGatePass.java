package com.mst.models;

import lombok.Data;

import javax.persistence.*;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

@Entity
@Table(name = "GatePassInward")
@Data
public class InwardGatePass {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "OrganizationId")
    private Integer organizationId;

    @Column(name = "CompanyId")
    private Integer companyId;

    @Column(name = "BranchesId")
    private Integer branchesId;

    @Column(name = "FinancialYearId")
    private Integer financialYearId;

    @Column(name = "DocumentTypeId")
    private Integer documentTypeId;

    @Column(name = "GpDate")
    @Temporal(TemporalType.TIMESTAMP)
    private Date gpDate;

    @Column(name = "GpSrNo")
    private Integer gpSrNo;

    @Column(name = "GatepassType")
    private String gatepassType;

    @Column(name = "GpTypeSrNo")
    private Integer gpTypeSrNo;

    @Column(name = "SupplierCustomerId")
    private Integer supplierCustomerId;

    @Column(name = "CityId")
    private Integer cityId;

    @Column(name = "ItemId")
    private Integer itemId;

    @Column(name = "PurchaseOrderId")
    private Integer purchaseOrderId;

    @Column(name = "VehicleType")
    private String vehicleType;

    @Column(name = "VehicleNo")
    private String vehicleNo;

    @Column(name = "BiltyNo")
    private String biltyNo;

    @Column(name = "BiltyDate")
    @Temporal(TemporalType.TIMESTAMP)
    private Date biltyDate;

    @Column(name = "Freight")
    private Double freight = 0.0;

    @Column(name = "AdvanceByParty")
    private Double advanceByParty = 0.0;

    @Column(name = "AdvanceByFactory")
    private Double advanceByFactory = 0.0;

    @Column(name = "FreightOn")
    private Integer freightOn = 0;

    @Column(name = "NetPaid")
    private Double netPaid = 0.0;

    @Column(name = "SupplierFirstWeight")
    private Double supplierFirstWeight = 0.0;

    @Column(name = "SupplierSecondWeight")
    private Double supplierSecondWeight = 0.0;

    @Column(name = "SupplierWeight")
    private Double supplierWeight = 0.0;

    @Column(name = "FactoryWeight")
    private Double factoryWeight = 0.0;

    @Column(name = "DifferenceWeight")
    private Double differenceWeight = 0.0;

    @Column(name = "InDateTimeStamp")
    @Temporal(TemporalType.TIMESTAMP)
    private Date inDateTimeStamp;

    @Column(name = "OutDateTimeStamp")
    @Temporal(TemporalType.TIMESTAMP)
    private Date outDateTimeStamp;

    @Column(name = "WeighBridgeId")
    private Integer weighBridgeId = 0;

    @Column(name = "Status")
    private String status;

    @Column(name = "PackingTypeId")
    private Integer packingTypeId;

    @Column(name = "AccessWeight")
    private Double accessWeight = 0.0;

    @Column(name = "PackUnit")
    private Double packUnit = 0.0;

    @Column(name = "WeightComparedToPoWt")
    private Double weightComparedToPoWt = 0.0;

    @Column(name = "NoOfPackages")
    private Integer noOfPackages = 0;

    @Column(name = "Container")
    private String container;

    @Column(name = "Container1")
    private String container1;

    @Column(name = "OtherRemarks")
    private String otherRemarks;

    @Column(name = "OtherSupCust")
    private String otherSupCust;

    @Column(name = "SupplierContractCode")
    private String supplierContractCode;

    @Column(name = "VarietyName")
    private String varietyName;

    @Column(name = "SupplierDispatchId")
    private Integer supplierDispatchId = 0;

    @Column(name = "WarehouseId")
    private Integer warehouseId = 0;

    @Column(name = "RefDocumentEntryNo")
    private Integer refDocumentEntryNo = 0;

    @Column(name = "RefDocumentTypeId")
    private Integer refDocumentTypeId = 0;

    @Column(name = "ActionIdForSpecialApproval")
    private Integer actionIdForSpecialApproval = 0;

    @Column(name = "DriverName")
    private String driverName;

    @Column(name = "DriverCNICNO")
    private String driverCNICNO;

    @Column(name = "DriverMobileNo")
    private String driverMobileNo;

    @Column(name = "WeightDiffComments")
    private String weightDiffComments;

    @Column(name = "driverBioDataId")
    private Integer driverBioDataId = 0;

    @Column(name = "IsApproved")
    private Boolean isApproved = false;

    @Column(name = "PostState")
    private Boolean postState = false;

    @Column(name = "EntryDate")
    @Temporal(TemporalType.TIMESTAMP)
    private Date entryDate;

    @Column(name = "EntryUser")
    private Integer entryUser;

    @Column(name = "ModifyDate")
    @Temporal(TemporalType.TIMESTAMP)
    private Date modifyDate;

    @Column(name = "ModifyUser")
    private Integer modifyUser;

    @Column(name = "PostDate")
    @Temporal(TemporalType.TIMESTAMP)
    private Date postDate;

    @Column(name = "PostUser")
    private Integer postUser;

    // Non-persisted virtual / UI display fields matching desktop model
    @Transient
    private String whatsappNo;

    @Transient
    private String alternateCellNo;

    @Transient
    private String fatherName;

    @Transient
    private String fatherCnicNo;

    @Transient
    private Integer freightVoucherId;

    @Transient
    private String cityName;

    @Transient
    private String supplierName;

    @Transient
    private String itemName;

    @Transient
    private List<InwardGatePassDetail> gatePassInwardDetails = new ArrayList<>();

    @Transient
    private List<InwardGatePassPurchaseBreakUp> gatePassInwardPurchaseBreakUpList = new ArrayList<>();
}
