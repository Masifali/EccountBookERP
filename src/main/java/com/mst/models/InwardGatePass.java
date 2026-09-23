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

    // Sent to the existing desktop procedure; no schema generation for this field.
    @Transient
    private String docAttachment;

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

    // Explicit Getters and Setters
    public Integer getId() { return id; }
    public void setId(Integer id) { this.id = id; }
    public Integer getOrganizationId() { return organizationId; }
    public void setOrganizationId(Integer organizationId) { this.organizationId = organizationId; }
    public Integer getCompanyId() { return companyId; }
    public void setCompanyId(Integer companyId) { this.companyId = companyId; }
    public Integer getBranchesId() { return branchesId; }
    public void setBranchesId(Integer branchesId) { this.branchesId = branchesId; }
    public Integer getFinancialYearId() { return financialYearId; }
    public void setFinancialYearId(Integer financialYearId) { this.financialYearId = financialYearId; }
    public Integer getDocumentTypeId() { return documentTypeId; }
    public void setDocumentTypeId(Integer documentTypeId) { this.documentTypeId = documentTypeId; }
    public Date getGpDate() { return gpDate; }
    public void setGpDate(Date gpDate) { this.gpDate = gpDate; }
    public Integer getGpSrNo() { return gpSrNo; }
    public void setGpSrNo(Integer gpSrNo) { this.gpSrNo = gpSrNo; }
    public String getStatus() { return status; }
    public void setStatus(String status) { this.status = status; }
    public Integer getEntryUser() { return entryUser; }
    public void setEntryUser(Integer entryUser) { this.entryUser = entryUser; }
    public Integer getSupplierCustomerId() { return supplierCustomerId; }
    public void setSupplierCustomerId(Integer supplierCustomerId) { this.supplierCustomerId = supplierCustomerId; }
    public Integer getCityId() { return cityId; }
    public void setCityId(Integer cityId) { this.cityId = cityId; }
    public Date getInDateTimeStamp() { return inDateTimeStamp; }
    public void setInDateTimeStamp(Date inDateTimeStamp) { this.inDateTimeStamp = inDateTimeStamp; }
    public Date getOutDateTimeStamp() { return outDateTimeStamp; }
    public void setOutDateTimeStamp(Date outDateTimeStamp) { this.outDateTimeStamp = outDateTimeStamp; }
    public Double getSupplierWeight() { return supplierWeight; }
    public void setSupplierWeight(Double supplierWeight) { this.supplierWeight = supplierWeight; }
    public Double getFactoryWeight() { return factoryWeight; }
    public void setFactoryWeight(Double factoryWeight) { this.factoryWeight = factoryWeight; }
    public Double getDifferenceWeight() { return differenceWeight; }
    public void setDifferenceWeight(Double differenceWeight) { this.differenceWeight = differenceWeight; }
    public String getDriverName() { return driverName; }
    public void setDriverName(String driverName) { this.driverName = driverName; }
    public Integer getDriverBioDataId() { return driverBioDataId; }
    public void setDriverBioDataId(Integer driverBioDataId) { this.driverBioDataId = driverBioDataId; }
    public Date getEntryDate() { return entryDate; }
    public void setEntryDate(Date entryDate) { this.entryDate = entryDate; }
    public Integer getGpTypeSrNo() { return gpTypeSrNo; }
    public void setGpTypeSrNo(Integer gpTypeSrNo) { this.gpTypeSrNo = gpTypeSrNo; }
    public String getGatepassType() { return gatepassType; }
    public void setGatepassType(String gatepassType) { this.gatepassType = gatepassType; }
    public Date getModifyDate() { return modifyDate; }
    public void setModifyDate(Date modifyDate) { this.modifyDate = modifyDate; }
    public List<InwardGatePassDetail> getGatePassInwardDetails() { return gatePassInwardDetails; }
    public void setGatePassInwardDetails(List<InwardGatePassDetail> gatePassInwardDetails) { this.gatePassInwardDetails = gatePassInwardDetails; }
    public List<InwardGatePassPurchaseBreakUp> getGatePassInwardPurchaseBreakUpList() { return gatePassInwardPurchaseBreakUpList; }
    public void setGatePassInwardPurchaseBreakUpList(List<InwardGatePassPurchaseBreakUp> gatePassInwardPurchaseBreakUpList) { this.gatePassInwardPurchaseBreakUpList = gatePassInwardPurchaseBreakUpList; }
    public Integer getItemId() { return itemId; }
    public void setItemId(Integer itemId) { this.itemId = itemId; }
    public Integer getPurchaseOrderId() { return purchaseOrderId; }
    public void setPurchaseOrderId(Integer purchaseOrderId) { this.purchaseOrderId = purchaseOrderId; }
    public String getVehicleType() { return vehicleType; }
    public void setVehicleType(String vehicleType) { this.vehicleType = vehicleType; }
    public String getVehicleNo() { return vehicleNo; }
    public void setVehicleNo(String vehicleNo) { this.vehicleNo = vehicleNo; }
    public String getBiltyNo() { return biltyNo; }
    public void setBiltyNo(String biltyNo) { this.biltyNo = biltyNo; }
    public Date getBiltyDate() { return biltyDate; }
    public void setBiltyDate(Date biltyDate) { this.biltyDate = biltyDate; }
    public Double getFreight() { return freight; }
    public void setFreight(Double freight) { this.freight = freight; }
    public Double getAdvanceByParty() { return advanceByParty; }
    public void setAdvanceByParty(Double advanceByParty) { this.advanceByParty = advanceByParty; }
    public Double getAdvanceByFactory() { return advanceByFactory; }
    public void setAdvanceByFactory(Double advanceByFactory) { this.advanceByFactory = advanceByFactory; }
    public Integer getFreightOn() { return freightOn; }
    public void setFreightOn(Integer freightOn) { this.freightOn = freightOn; }
    public Double getNetPaid() { return netPaid; }
    public void setNetPaid(Double netPaid) { this.netPaid = netPaid; }
    public Double getSupplierFirstWeight() { return supplierFirstWeight; }
    public void setSupplierFirstWeight(Double supplierFirstWeight) { this.supplierFirstWeight = supplierFirstWeight; }
    public Double getSupplierSecondWeight() { return supplierSecondWeight; }
    public void setSupplierSecondWeight(Double supplierSecondWeight) { this.supplierSecondWeight = supplierSecondWeight; }
    public Integer getWeighBridgeId() { return weighBridgeId; }
    public void setWeighBridgeId(Integer weighBridgeId) { this.weighBridgeId = weighBridgeId; }
    public Integer getPackingTypeId() { return packingTypeId; }
    public void setPackingTypeId(Integer packingTypeId) { this.packingTypeId = packingTypeId; }
    public Double getAccessWeight() { return accessWeight; }
    public void setAccessWeight(Double accessWeight) { this.accessWeight = accessWeight; }
    public Double getPackUnit() { return packUnit; }
    public void setPackUnit(Double packUnit) { this.packUnit = packUnit; }
    public Double getWeightComparedToPoWt() { return weightComparedToPoWt; }
    public void setWeightComparedToPoWt(Double weightComparedToPoWt) { this.weightComparedToPoWt = weightComparedToPoWt; }
    public Integer getNoOfPackages() { return noOfPackages; }
    public void setNoOfPackages(Integer noOfPackages) { this.noOfPackages = noOfPackages; }
    public String getContainer() { return container; }
    public void setContainer(String container) { this.container = container; }
    public String getContainer1() { return container1; }
    public void setContainer1(String container1) { this.container1 = container1; }
    public String getOtherRemarks() { return otherRemarks; }
    public void setOtherRemarks(String otherRemarks) { this.otherRemarks = otherRemarks; }
    public String getOtherSupCust() { return otherSupCust; }
    public void setOtherSupCust(String otherSupCust) { this.otherSupCust = otherSupCust; }
    public String getSupplierContractCode() { return supplierContractCode; }
    public void setSupplierContractCode(String supplierContractCode) { this.supplierContractCode = supplierContractCode; }
    public String getVarietyName() { return varietyName; }
    public void setVarietyName(String varietyName) { this.varietyName = varietyName; }
    public Integer getSupplierDispatchId() { return supplierDispatchId; }
    public void setSupplierDispatchId(Integer supplierDispatchId) { this.supplierDispatchId = supplierDispatchId; }
    public Integer getWarehouseId() { return warehouseId; }
    public void setWarehouseId(Integer warehouseId) { this.warehouseId = warehouseId; }
    public Integer getRefDocumentTypeId() { return refDocumentTypeId; }
    public void setRefDocumentTypeId(Integer refDocumentTypeId) { this.refDocumentTypeId = refDocumentTypeId; }
    public Integer getActionIdForSpecialApproval() { return actionIdForSpecialApproval; }
    public void setActionIdForSpecialApproval(Integer actionIdForSpecialApproval) { this.actionIdForSpecialApproval = actionIdForSpecialApproval; }
    public String getDriverCNICNO() { return driverCNICNO; }
    public void setDriverCNICNO(String driverCNICNO) { this.driverCNICNO = driverCNICNO; }
    public String getDriverMobileNo() { return driverMobileNo; }
    public void setDriverMobileNo(String driverMobileNo) { this.driverMobileNo = driverMobileNo; }
    public String getWeightDiffComments() { return weightDiffComments; }
    public void setWeightDiffComments(String weightDiffComments) { this.weightDiffComments = weightDiffComments; }
    public Boolean getIsApproved() { return isApproved; }
    public void setIsApproved(Boolean isApproved) { this.isApproved = isApproved; }
    public Boolean getPostState() { return postState; }
    public void setPostState(Boolean postState) { this.postState = postState; }
    public Integer getModifyUser() { return modifyUser; }
    public void setModifyUser(Integer modifyUser) { this.modifyUser = modifyUser; }
    public String getWhatsappNo() { return whatsappNo; }
    public void setWhatsappNo(String whatsappNo) { this.whatsappNo = whatsappNo; }
    public String getAlternateCellNo() { return alternateCellNo; }
    public void setAlternateCellNo(String alternateCellNo) { this.alternateCellNo = alternateCellNo; }
    public String getFatherName() { return fatherName; }
    public void setFatherName(String fatherName) { this.fatherName = fatherName; }
    public String getFatherCnicNo() { return fatherCnicNo; }
    public void setFatherCnicNo(String fatherCnicNo) { this.fatherCnicNo = fatherCnicNo; }
    public Date getPostDate() { return postDate; }
    public void setPostDate(Date postDate) { this.postDate = postDate; }
}
