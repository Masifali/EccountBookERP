package com.mst.models.cmagt.dto;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;

public class GrnLoadingChallanCmagtDto {

    private Integer grnSupplierLoadingMasterId;
    private Integer docNo;
    private String docDate;
    private Integer purchaseOrderMasterId;
    private String purchaseOrderDocNo;
    private Integer supplierId;
    private String supplierName;
    private Integer commissionAgentId;
    private String commissionAgentName;
    private String vehicleNo;
    private String biltyNo;
    private String driverName;
    private String driverMobileNo;
    private String remarksHeader;

    private Integer organizationId;
    private Integer companyId;
    private Integer branchId;
    private Integer financialYearId;
    private Integer entryUserId;
    private Integer modifyUserId;
    private Integer documentTypeId = 1054;

    private List<DetailDto> grnSupplierLoadingDetailList = new ArrayList<>();

    public static class DetailDto {
        private Integer grnSupplierLoadingDetailId;
        private Integer grnSupplierLoadingMasterId;
        private Integer purchaseOrderDetailId;
        private Integer itemId;
        private String itemName;
        private Integer cropYearId;
        private Integer packTypeId;
        private BigDecimal weight;
        private BigDecimal qty;
        private BigDecimal rate;
        private BigDecimal amount;
        private String remarksDetail;

        public Integer getGrnSupplierLoadingDetailId() { return grnSupplierLoadingDetailId; }
        public void setGrnSupplierLoadingDetailId(Integer grnSupplierLoadingDetailId) { this.grnSupplierLoadingDetailId = grnSupplierLoadingDetailId; }
        public Integer getGrnSupplierLoadingMasterId() { return grnSupplierLoadingMasterId; }
        public void setGrnSupplierLoadingMasterId(Integer grnSupplierLoadingMasterId) { this.grnSupplierLoadingMasterId = grnSupplierLoadingMasterId; }
        public Integer getPurchaseOrderDetailId() { return purchaseOrderDetailId; }
        public void setPurchaseOrderDetailId(Integer purchaseOrderDetailId) { this.purchaseOrderDetailId = purchaseOrderDetailId; }
        public Integer getItemId() { return itemId; }
        public void setItemId(Integer itemId) { this.itemId = itemId; }
        public String getItemName() { return itemName; }
        public void setItemName(String itemName) { this.itemName = itemName; }
        public Integer getCropYearId() { return cropYearId; }
        public void setCropYearId(Integer cropYearId) { this.cropYearId = cropYearId; }
        public Integer getPackTypeId() { return packTypeId; }
        public void setPackTypeId(Integer packTypeId) { this.packTypeId = packTypeId; }
        public BigDecimal getWeight() { return weight; }
        public void setWeight(BigDecimal weight) { this.weight = weight; }
        public BigDecimal getQty() { return qty; }
        public void setQty(BigDecimal qty) { this.qty = qty; }
        public BigDecimal getRate() { return rate; }
        public void setRate(BigDecimal rate) { this.rate = rate; }
        public BigDecimal getAmount() { return amount; }
        public void setAmount(BigDecimal amount) { this.amount = amount; }
        public String getRemarksDetail() { return remarksDetail; }
        public void setRemarksDetail(String remarksDetail) { this.remarksDetail = remarksDetail; }
    }

    // Getters and Setters
    public Integer getGrnSupplierLoadingMasterId() { return grnSupplierLoadingMasterId; }
    public void setGrnSupplierLoadingMasterId(Integer grnSupplierLoadingMasterId) { this.grnSupplierLoadingMasterId = grnSupplierLoadingMasterId; }
    public Integer getDocNo() { return docNo; }
    public void setDocNo(Integer docNo) { this.docNo = docNo; }
    public String getDocDate() { return docDate; }
    public void setDocDate(String docDate) { this.docDate = docDate; }
    public Integer getPurchaseOrderMasterId() { return purchaseOrderMasterId; }
    public void setPurchaseOrderMasterId(Integer purchaseOrderMasterId) { this.purchaseOrderMasterId = purchaseOrderMasterId; }
    public String getPurchaseOrderDocNo() { return purchaseOrderDocNo; }
    public void setPurchaseOrderDocNo(String purchaseOrderDocNo) { this.purchaseOrderDocNo = purchaseOrderDocNo; }
    public Integer getSupplierId() { return supplierId; }
    public void setSupplierId(Integer supplierId) { this.supplierId = supplierId; }
    public String getSupplierName() { return supplierName; }
    public void setSupplierName(String supplierName) { this.supplierName = supplierName; }
    public Integer getCommissionAgentId() { return commissionAgentId; }
    public void setCommissionAgentId(Integer commissionAgentId) { this.commissionAgentId = commissionAgentId; }
    public String getCommissionAgentName() { return commissionAgentName; }
    public void setCommissionAgentName(String commissionAgentName) { this.commissionAgentName = commissionAgentName; }
    public String getVehicleNo() { return vehicleNo; }
    public void setVehicleNo(String vehicleNo) { this.vehicleNo = vehicleNo; }
    public String getBiltyNo() { return biltyNo; }
    public void setBiltyNo(String biltyNo) { this.biltyNo = biltyNo; }
    public String getDriverName() { return driverName; }
    public void setDriverName(String driverName) { this.driverName = driverName; }
    public String getDriverMobileNo() { return driverMobileNo; }
    public void setDriverMobileNo(String driverMobileNo) { this.driverMobileNo = driverMobileNo; }
    public String getRemarksHeader() { return remarksHeader; }
    public void setRemarksHeader(String remarksHeader) { this.remarksHeader = remarksHeader; }

    public Integer getOrganizationId() { return organizationId; }
    public void setOrganizationId(Integer organizationId) { this.organizationId = organizationId; }
    public Integer getCompanyId() { return companyId; }
    public void setCompanyId(Integer companyId) { this.companyId = companyId; }
    public Integer getBranchId() { return branchId; }
    public void setBranchId(Integer branchId) { this.branchId = branchId; }
    public Integer getFinancialYearId() { return financialYearId; }
    public void setFinancialYearId(Integer financialYearId) { this.financialYearId = financialYearId; }
    public Integer getEntryUserId() { return entryUserId; }
    public void setEntryUserId(Integer entryUserId) { this.entryUserId = entryUserId; }
    public Integer getModifyUserId() { return modifyUserId; }
    public void setModifyUserId(Integer modifyUserId) { this.modifyUserId = modifyUserId; }
    public Integer getDocumentTypeId() { return documentTypeId; }
    public void setDocumentTypeId(Integer documentTypeId) { this.documentTypeId = documentTypeId; }

    public List<DetailDto> getGrnSupplierLoadingDetailList() { return grnSupplierLoadingDetailList; }
    public void setGrnSupplierLoadingDetailList(List<DetailDto> grnSupplierLoadingDetailList) { this.grnSupplierLoadingDetailList = grnSupplierLoadingDetailList; }
}
