package com.mst.models.cmagt.dto;

import com.fasterxml.jackson.annotation.JsonAlias;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;

/**
 * GRN Loading Challan (CMAGT) - frmGrnLoadingChallanCmagt.
 *
 * TRACED CONTRACT
 *   Form btnSave_Click (:2560-2760)
 *     -> Architecture.Model.CommissionAgent.grnSupplierLoadingMaster
 *     -> BLL grnSupplierLoadingMaster.Save  (sets actionId = masterId == 0 ? 1 : 2)
 *     -> DAL grnSupplierLoadingMaster.SetData - ONE SqlTransaction over all of:
 *          [cmagt].[USP_grnSupplierLoadingMaster_InsertAndUpdate]     43 params
 *          [cmagt].[USP_grnSupplierLoadingDetail_Insert]              35 params, per row
 *          [cmagt].[USP_grnSupplierLoadingEmptyBagDetail_Insert]      10 params, per row
 *          [cmagt].[USP_grnSupplierLoadingExpenseDetail_Insert]       10 params, per row
 *
 * WHY EVERY FIELD IS HERE
 *   The previous DTO carried 15 of the master's 43 parameters and invented three more
 *   (purchaseOrderMasterId, driverName, driverMobileNo) that the desktop model does not
 *   declare. SimpleJdbcCall silently applies the procedure's own defaults for anything not
 *   passed, so a challan saved from the web stored no freight, no weights, no transporter,
 *   no cities, no delivery term and no vehicle type - and reported success.
 *
 *   actionId was among the missing. The BLL derives it (1 = insert, 2 = update) and the
 *   procedure branches on it, so an update sent without it was not reliably an update.
 *
 * The two empty-bag and expense child collections had no Java representation at all.
 */
/*
 * JSON BINDING NOTE
 * -----------------
 * Several fields below carry the desktop model's own spelling, which begins with a capital
 * letter (ItemAmount, TaxNameId, ShipToAddress, EBWeightDeductionTermId, ...). Jackson derives
 * a property name from the ACCESSOR, not the field, so `getItemAmount()` publishes the property
 * as "itemAmount" and a payload that posts "ItemAmount" - the name the desktop uses - does not
 * bind. Spring Boot leaves FAIL_ON_UNKNOWN_PROPERTIES off, so such a key is dropped in silence:
 * no error, no log line, just a missing value and a save that reports success.
 *
 * Every such field therefore carries @JsonAlias with the desktop's spelling, so both forms bind.
 * Aliases only ADD accepted spellings; nothing that worked before changes.
 */

public class GrnLoadingChallanCmagtDto {

    private Boolean isApproved;
    private Boolean isSupplierOtherChargesAllowed;
    private String approvedDate;
    private String biltyDate;
    private String deliveryStartDate;
    private String docDate;
    private String entryDate;
    private String modifyDate;
    private BigDecimal biltyFreight;
    private BigDecimal biltyQty;
    private BigDecimal loadWeight;
    private BigDecimal otherAdLesCharges;
    private BigDecimal scaleNetWeight;
    private BigDecimal tareWeight;
    private BigDecimal totalFreight;
    private Integer actionId;
    private Integer approvedUserId;
    private Integer branchId;
    private Integer commissionAgentId;
    private Integer companyId;
    private Integer deliveryDays;
    private Integer deliveryTermId;
    private Integer docNo;
    private Integer documentTypeId;
    private Integer entryUserId;
    private Integer financialYearId;
    private Integer grnSupplierLoadingMasterId;
    private Integer loadingCityId;
    private Integer modifyUserId;
    private Integer organizationId;
    private Integer projectId;
    private Integer supplierId;
    private Integer transporterId;
    private Integer unloadingCityId;
    private Integer vehicleTypeId;
    private String approvalRemarks;
    private String attachmentsValues;
    private String biltyNo;
    private String customAttachmentsValues;
    private String remarksHeader;
    private String supplierRefDocNo;
    private String transporterName;
    private String vehicleNo;

    /* Display only - not procedure parameters. */
    private String supplierName;
    private String commissionAgentName;
    private Integer purchaseOrderDocNo;

    private List<DetailDto> grnSupplierLoadingDetailList = new ArrayList<>();
    private List<EmptyBagDto> grnSupplierLoadingEmptyBagDetailList = new ArrayList<>();
    private List<ExpenseDto> grnSupplierLoadingExpenseDetailList = new ArrayList<>();

    /**
     * [cmagt].[USP_grnSupplierLoadingDetail_Insert]
     *
     * 35 parameters, in the desktop model's declaration order. GenericProvider.SetProc
     * reflects over the model's NON-VIRTUAL properties, so this list is the procedure's
     * signature - not a subset chosen here.
     */
    public static class DetailDto {
        private BigDecimal addLessWeight;
        private BigDecimal ebwPerUnit;
        private BigDecimal ebwTotal;
        private BigDecimal loadingQty;
        private BigDecimal netBillWeight;
        private BigDecimal wbGrossWeight;
        private Integer actionTypeId;
        private Integer buyerId;
        private Integer cropYearId;
        @JsonAlias({"DeliverToAddressId"})
        private Integer DeliverToAddressId;
        @JsonAlias({"DeliverToPartyId"})
        private Integer DeliverToPartyId;
        private Integer grnSupplierLoadingDetailId;
        private Integer grnSupplierLoadingMasterId;
        private Integer inquiryBookingDetailId;
        private Integer inquiryBookingMasterId;
        private Integer inventoryParentCategoryId;
        private Integer itemId;
        private Integer loadingCityId;
        private Integer modifyUserId;
        private Integer packingTypeId;
        private Integer packUomId;
        private Integer purchaseOrderDetailId;
        private Integer purchaseOrderMasterId;
        private Integer saleOrderDetailId;
        private Integer saleOrderMasterId;
        @JsonAlias({"EBWeightDeductionTermId", "ebWeightDeductionTermId"})
        private Integer EBWeightDeductionTermId;
        private Integer purchaseOrderSaleOrderMappingId;
        private Integer sortNo;
        private Integer supplierOfferDetailId;
        private Integer supplierOfferId;
        private Integer unloadingCityId;
        private String cropYear;
        private String remarks;
        private String warningRemarks;
        @JsonAlias({"DeliverToAddress"})
        private String DeliverToAddress;

        /* Display only - not procedure parameters. */
        private String itemName;

        public BigDecimal getAddLessWeight() { return addLessWeight; }
        public void setAddLessWeight(BigDecimal addLessWeight) { this.addLessWeight = addLessWeight; }
        public BigDecimal getEbwPerUnit() { return ebwPerUnit; }
        public void setEbwPerUnit(BigDecimal ebwPerUnit) { this.ebwPerUnit = ebwPerUnit; }
        public BigDecimal getEbwTotal() { return ebwTotal; }
        public void setEbwTotal(BigDecimal ebwTotal) { this.ebwTotal = ebwTotal; }
        public BigDecimal getLoadingQty() { return loadingQty; }
        public void setLoadingQty(BigDecimal loadingQty) { this.loadingQty = loadingQty; }
        public BigDecimal getNetBillWeight() { return netBillWeight; }
        public void setNetBillWeight(BigDecimal netBillWeight) { this.netBillWeight = netBillWeight; }
        public BigDecimal getWbGrossWeight() { return wbGrossWeight; }
        public void setWbGrossWeight(BigDecimal wbGrossWeight) { this.wbGrossWeight = wbGrossWeight; }
        public Integer getActionTypeId() { return actionTypeId; }
        public void setActionTypeId(Integer actionTypeId) { this.actionTypeId = actionTypeId; }
        public Integer getBuyerId() { return buyerId; }
        public void setBuyerId(Integer buyerId) { this.buyerId = buyerId; }
        public Integer getCropYearId() { return cropYearId; }
        public void setCropYearId(Integer cropYearId) { this.cropYearId = cropYearId; }
        public Integer getDeliverToAddressId() { return DeliverToAddressId; }
        public void setDeliverToAddressId(Integer DeliverToAddressId) { this.DeliverToAddressId = DeliverToAddressId; }
        public Integer getDeliverToPartyId() { return DeliverToPartyId; }
        public void setDeliverToPartyId(Integer DeliverToPartyId) { this.DeliverToPartyId = DeliverToPartyId; }
        public Integer getGrnSupplierLoadingDetailId() { return grnSupplierLoadingDetailId; }
        public void setGrnSupplierLoadingDetailId(Integer grnSupplierLoadingDetailId) { this.grnSupplierLoadingDetailId = grnSupplierLoadingDetailId; }
        public Integer getGrnSupplierLoadingMasterId() { return grnSupplierLoadingMasterId; }
        public void setGrnSupplierLoadingMasterId(Integer grnSupplierLoadingMasterId) { this.grnSupplierLoadingMasterId = grnSupplierLoadingMasterId; }
        public Integer getInquiryBookingDetailId() { return inquiryBookingDetailId; }
        public void setInquiryBookingDetailId(Integer inquiryBookingDetailId) { this.inquiryBookingDetailId = inquiryBookingDetailId; }
        public Integer getInquiryBookingMasterId() { return inquiryBookingMasterId; }
        public void setInquiryBookingMasterId(Integer inquiryBookingMasterId) { this.inquiryBookingMasterId = inquiryBookingMasterId; }
        public Integer getInventoryParentCategoryId() { return inventoryParentCategoryId; }
        public void setInventoryParentCategoryId(Integer inventoryParentCategoryId) { this.inventoryParentCategoryId = inventoryParentCategoryId; }
        public Integer getItemId() { return itemId; }
        public void setItemId(Integer itemId) { this.itemId = itemId; }
        public Integer getLoadingCityId() { return loadingCityId; }
        public void setLoadingCityId(Integer loadingCityId) { this.loadingCityId = loadingCityId; }
        public Integer getModifyUserId() { return modifyUserId; }
        public void setModifyUserId(Integer modifyUserId) { this.modifyUserId = modifyUserId; }
        public Integer getPackingTypeId() { return packingTypeId; }
        public void setPackingTypeId(Integer packingTypeId) { this.packingTypeId = packingTypeId; }
        public Integer getPackUomId() { return packUomId; }
        public void setPackUomId(Integer packUomId) { this.packUomId = packUomId; }
        public Integer getPurchaseOrderDetailId() { return purchaseOrderDetailId; }
        public void setPurchaseOrderDetailId(Integer purchaseOrderDetailId) { this.purchaseOrderDetailId = purchaseOrderDetailId; }
        public Integer getPurchaseOrderMasterId() { return purchaseOrderMasterId; }
        public void setPurchaseOrderMasterId(Integer purchaseOrderMasterId) { this.purchaseOrderMasterId = purchaseOrderMasterId; }
        public Integer getSaleOrderDetailId() { return saleOrderDetailId; }
        public void setSaleOrderDetailId(Integer saleOrderDetailId) { this.saleOrderDetailId = saleOrderDetailId; }
        public Integer getSaleOrderMasterId() { return saleOrderMasterId; }
        public void setSaleOrderMasterId(Integer saleOrderMasterId) { this.saleOrderMasterId = saleOrderMasterId; }
        public Integer getEBWeightDeductionTermId() { return EBWeightDeductionTermId; }
        public void setEBWeightDeductionTermId(Integer EBWeightDeductionTermId) { this.EBWeightDeductionTermId = EBWeightDeductionTermId; }
        public Integer getPurchaseOrderSaleOrderMappingId() { return purchaseOrderSaleOrderMappingId; }
        public void setPurchaseOrderSaleOrderMappingId(Integer purchaseOrderSaleOrderMappingId) { this.purchaseOrderSaleOrderMappingId = purchaseOrderSaleOrderMappingId; }
        public Integer getSortNo() { return sortNo; }
        public void setSortNo(Integer sortNo) { this.sortNo = sortNo; }
        public Integer getSupplierOfferDetailId() { return supplierOfferDetailId; }
        public void setSupplierOfferDetailId(Integer supplierOfferDetailId) { this.supplierOfferDetailId = supplierOfferDetailId; }
        public Integer getSupplierOfferId() { return supplierOfferId; }
        public void setSupplierOfferId(Integer supplierOfferId) { this.supplierOfferId = supplierOfferId; }
        public Integer getUnloadingCityId() { return unloadingCityId; }
        public void setUnloadingCityId(Integer unloadingCityId) { this.unloadingCityId = unloadingCityId; }
        public String getCropYear() { return cropYear; }
        public void setCropYear(String cropYear) { this.cropYear = cropYear; }
        public String getRemarks() { return remarks; }
        public void setRemarks(String remarks) { this.remarks = remarks; }
        public String getWarningRemarks() { return warningRemarks; }
        public void setWarningRemarks(String warningRemarks) { this.warningRemarks = warningRemarks; }
        public String getDeliverToAddress() { return DeliverToAddress; }
        public void setDeliverToAddress(String DeliverToAddress) { this.DeliverToAddress = DeliverToAddress; }
        public String getItemName() { return itemName; }
        public void setItemName(String itemName) { this.itemName = itemName; }
    }

    /**
     * [cmagt].[USP_grnSupplierLoadingEmptyBagDetail_Insert]
     *
     * 10 parameters, in the desktop model's declaration order. GenericProvider.SetProc
     * reflects over the model's NON-VIRTUAL properties, so this list is the procedure's
     * signature - not a subset chosen here.
     */
    public static class EmptyBagDto {
        @JsonAlias({"Rate"})
        private BigDecimal Rate;
        private BigDecimal weightCutKg;
        private Integer grnSupplierLoadingEmptyBagDetailId;
        private Integer grnSupplierLoadingMasterId;
        private Integer purchaseOrderMasterId;
        private Integer purchaseOrderEmptyBagDetailId;
        @JsonAlias({"PackingTypeId"})
        private Integer PackingTypeId;
        private Integer emptyBagPackingMaterialItemId;
        private Integer sortNo;
        private String remarks;

        /* Display only - not procedure parameters. */


        public BigDecimal getRate() { return Rate; }
        public void setRate(BigDecimal Rate) { this.Rate = Rate; }
        public BigDecimal getWeightCutKg() { return weightCutKg; }
        public void setWeightCutKg(BigDecimal weightCutKg) { this.weightCutKg = weightCutKg; }
        public Integer getGrnSupplierLoadingEmptyBagDetailId() { return grnSupplierLoadingEmptyBagDetailId; }
        public void setGrnSupplierLoadingEmptyBagDetailId(Integer grnSupplierLoadingEmptyBagDetailId) { this.grnSupplierLoadingEmptyBagDetailId = grnSupplierLoadingEmptyBagDetailId; }
        public Integer getGrnSupplierLoadingMasterId() { return grnSupplierLoadingMasterId; }
        public void setGrnSupplierLoadingMasterId(Integer grnSupplierLoadingMasterId) { this.grnSupplierLoadingMasterId = grnSupplierLoadingMasterId; }
        public Integer getPurchaseOrderMasterId() { return purchaseOrderMasterId; }
        public void setPurchaseOrderMasterId(Integer purchaseOrderMasterId) { this.purchaseOrderMasterId = purchaseOrderMasterId; }
        public Integer getPurchaseOrderEmptyBagDetailId() { return purchaseOrderEmptyBagDetailId; }
        public void setPurchaseOrderEmptyBagDetailId(Integer purchaseOrderEmptyBagDetailId) { this.purchaseOrderEmptyBagDetailId = purchaseOrderEmptyBagDetailId; }
        public Integer getPackingTypeId() { return PackingTypeId; }
        public void setPackingTypeId(Integer PackingTypeId) { this.PackingTypeId = PackingTypeId; }
        public Integer getEmptyBagPackingMaterialItemId() { return emptyBagPackingMaterialItemId; }
        public void setEmptyBagPackingMaterialItemId(Integer emptyBagPackingMaterialItemId) { this.emptyBagPackingMaterialItemId = emptyBagPackingMaterialItemId; }
        public Integer getSortNo() { return sortNo; }
        public void setSortNo(Integer sortNo) { this.sortNo = sortNo; }
        public String getRemarks() { return remarks; }
        public void setRemarks(String remarks) { this.remarks = remarks; }
    }

    /**
     * [cmagt].[USP_grnSupplierLoadingExpenseDetail_Insert]
     *
     * 10 parameters, in the desktop model's declaration order. GenericProvider.SetProc
     * reflects over the model's NON-VIRTUAL properties, so this list is the procedure's
     * signature - not a subset chosen here.
     */
    public static class ExpenseDto {
        @JsonAlias({"Qty"})
        private BigDecimal Qty;
        private Double amount;
        private Double rate;
        private Integer grnSupplierLoadingExpenseDetailId;
        private Integer grnSupplierLoadingMasterId;
        private Integer purchaseOrderMasterId;
        private Integer purchaseOrderExpenseDetailId;
        @JsonAlias({"ItemId"})
        private Integer ItemId;
        private Integer sortNo;
        private String remarks;

        /* Display only - not procedure parameters. */


        public BigDecimal getQty() { return Qty; }
        public void setQty(BigDecimal Qty) { this.Qty = Qty; }
        public Double getAmount() { return amount; }
        public void setAmount(Double amount) { this.amount = amount; }
        public Double getRate() { return rate; }
        public void setRate(Double rate) { this.rate = rate; }
        public Integer getGrnSupplierLoadingExpenseDetailId() { return grnSupplierLoadingExpenseDetailId; }
        public void setGrnSupplierLoadingExpenseDetailId(Integer grnSupplierLoadingExpenseDetailId) { this.grnSupplierLoadingExpenseDetailId = grnSupplierLoadingExpenseDetailId; }
        public Integer getGrnSupplierLoadingMasterId() { return grnSupplierLoadingMasterId; }
        public void setGrnSupplierLoadingMasterId(Integer grnSupplierLoadingMasterId) { this.grnSupplierLoadingMasterId = grnSupplierLoadingMasterId; }
        public Integer getPurchaseOrderMasterId() { return purchaseOrderMasterId; }
        public void setPurchaseOrderMasterId(Integer purchaseOrderMasterId) { this.purchaseOrderMasterId = purchaseOrderMasterId; }
        public Integer getPurchaseOrderExpenseDetailId() { return purchaseOrderExpenseDetailId; }
        public void setPurchaseOrderExpenseDetailId(Integer purchaseOrderExpenseDetailId) { this.purchaseOrderExpenseDetailId = purchaseOrderExpenseDetailId; }
        public Integer getItemId() { return ItemId; }
        public void setItemId(Integer ItemId) { this.ItemId = ItemId; }
        public Integer getSortNo() { return sortNo; }
        public void setSortNo(Integer sortNo) { this.sortNo = sortNo; }
        public String getRemarks() { return remarks; }
        public void setRemarks(String remarks) { this.remarks = remarks; }
    }

    public Boolean isApproved() { return isApproved; }
    public void setIsApproved(Boolean isApproved) { this.isApproved = isApproved; }
    public Boolean isSupplierOtherChargesAllowed() { return isSupplierOtherChargesAllowed; }
    public void setIsSupplierOtherChargesAllowed(Boolean isSupplierOtherChargesAllowed) { this.isSupplierOtherChargesAllowed = isSupplierOtherChargesAllowed; }
    public String getApprovedDate() { return approvedDate; }
    public void setApprovedDate(String approvedDate) { this.approvedDate = approvedDate; }
    public String getBiltyDate() { return biltyDate; }
    public void setBiltyDate(String biltyDate) { this.biltyDate = biltyDate; }
    public String getDeliveryStartDate() { return deliveryStartDate; }
    public void setDeliveryStartDate(String deliveryStartDate) { this.deliveryStartDate = deliveryStartDate; }
    public String getDocDate() { return docDate; }
    public void setDocDate(String docDate) { this.docDate = docDate; }
    public String getEntryDate() { return entryDate; }
    public void setEntryDate(String entryDate) { this.entryDate = entryDate; }
    public String getModifyDate() { return modifyDate; }
    public void setModifyDate(String modifyDate) { this.modifyDate = modifyDate; }
    public BigDecimal getBiltyFreight() { return biltyFreight; }
    public void setBiltyFreight(BigDecimal biltyFreight) { this.biltyFreight = biltyFreight; }
    public BigDecimal getBiltyQty() { return biltyQty; }
    public void setBiltyQty(BigDecimal biltyQty) { this.biltyQty = biltyQty; }
    public BigDecimal getLoadWeight() { return loadWeight; }
    public void setLoadWeight(BigDecimal loadWeight) { this.loadWeight = loadWeight; }
    public BigDecimal getOtherAdLesCharges() { return otherAdLesCharges; }
    public void setOtherAdLesCharges(BigDecimal otherAdLesCharges) { this.otherAdLesCharges = otherAdLesCharges; }
    public BigDecimal getScaleNetWeight() { return scaleNetWeight; }
    public void setScaleNetWeight(BigDecimal scaleNetWeight) { this.scaleNetWeight = scaleNetWeight; }
    public BigDecimal getTareWeight() { return tareWeight; }
    public void setTareWeight(BigDecimal tareWeight) { this.tareWeight = tareWeight; }
    public BigDecimal getTotalFreight() { return totalFreight; }
    public void setTotalFreight(BigDecimal totalFreight) { this.totalFreight = totalFreight; }
    public Integer getActionId() { return actionId; }
    public void setActionId(Integer actionId) { this.actionId = actionId; }
    public Integer getApprovedUserId() { return approvedUserId; }
    public void setApprovedUserId(Integer approvedUserId) { this.approvedUserId = approvedUserId; }
    public Integer getBranchId() { return branchId; }
    public void setBranchId(Integer branchId) { this.branchId = branchId; }
    public Integer getCommissionAgentId() { return commissionAgentId; }
    public void setCommissionAgentId(Integer commissionAgentId) { this.commissionAgentId = commissionAgentId; }
    public Integer getCompanyId() { return companyId; }
    public void setCompanyId(Integer companyId) { this.companyId = companyId; }
    public Integer getDeliveryDays() { return deliveryDays; }
    public void setDeliveryDays(Integer deliveryDays) { this.deliveryDays = deliveryDays; }
    public Integer getDeliveryTermId() { return deliveryTermId; }
    public void setDeliveryTermId(Integer deliveryTermId) { this.deliveryTermId = deliveryTermId; }
    public Integer getDocNo() { return docNo; }
    public void setDocNo(Integer docNo) { this.docNo = docNo; }
    public Integer getDocumentTypeId() { return documentTypeId; }
    public void setDocumentTypeId(Integer documentTypeId) { this.documentTypeId = documentTypeId; }
    public Integer getEntryUserId() { return entryUserId; }
    public void setEntryUserId(Integer entryUserId) { this.entryUserId = entryUserId; }
    public Integer getFinancialYearId() { return financialYearId; }
    public void setFinancialYearId(Integer financialYearId) { this.financialYearId = financialYearId; }
    public Integer getGrnSupplierLoadingMasterId() { return grnSupplierLoadingMasterId; }
    public void setGrnSupplierLoadingMasterId(Integer grnSupplierLoadingMasterId) { this.grnSupplierLoadingMasterId = grnSupplierLoadingMasterId; }
    public Integer getLoadingCityId() { return loadingCityId; }
    public void setLoadingCityId(Integer loadingCityId) { this.loadingCityId = loadingCityId; }
    public Integer getModifyUserId() { return modifyUserId; }
    public void setModifyUserId(Integer modifyUserId) { this.modifyUserId = modifyUserId; }
    public Integer getOrganizationId() { return organizationId; }
    public void setOrganizationId(Integer organizationId) { this.organizationId = organizationId; }
    public Integer getProjectId() { return projectId; }
    public void setProjectId(Integer projectId) { this.projectId = projectId; }
    public Integer getSupplierId() { return supplierId; }
    public void setSupplierId(Integer supplierId) { this.supplierId = supplierId; }
    public Integer getTransporterId() { return transporterId; }
    public void setTransporterId(Integer transporterId) { this.transporterId = transporterId; }
    public Integer getUnloadingCityId() { return unloadingCityId; }
    public void setUnloadingCityId(Integer unloadingCityId) { this.unloadingCityId = unloadingCityId; }
    public Integer getVehicleTypeId() { return vehicleTypeId; }
    public void setVehicleTypeId(Integer vehicleTypeId) { this.vehicleTypeId = vehicleTypeId; }
    public String getApprovalRemarks() { return approvalRemarks; }
    public void setApprovalRemarks(String approvalRemarks) { this.approvalRemarks = approvalRemarks; }
    public String getAttachmentsValues() { return attachmentsValues; }
    public void setAttachmentsValues(String attachmentsValues) { this.attachmentsValues = attachmentsValues; }
    public String getBiltyNo() { return biltyNo; }
    public void setBiltyNo(String biltyNo) { this.biltyNo = biltyNo; }
    public String getCustomAttachmentsValues() { return customAttachmentsValues; }
    public void setCustomAttachmentsValues(String customAttachmentsValues) { this.customAttachmentsValues = customAttachmentsValues; }
    public String getRemarksHeader() { return remarksHeader; }
    public void setRemarksHeader(String remarksHeader) { this.remarksHeader = remarksHeader; }
    public String getSupplierRefDocNo() { return supplierRefDocNo; }
    public void setSupplierRefDocNo(String supplierRefDocNo) { this.supplierRefDocNo = supplierRefDocNo; }
    public String getTransporterName() { return transporterName; }
    public void setTransporterName(String transporterName) { this.transporterName = transporterName; }
    public String getVehicleNo() { return vehicleNo; }
    public void setVehicleNo(String vehicleNo) { this.vehicleNo = vehicleNo; }
    public String getSupplierName() { return supplierName; }
    public void setSupplierName(String supplierName) { this.supplierName = supplierName; }
    public String getCommissionAgentName() { return commissionAgentName; }
    public void setCommissionAgentName(String commissionAgentName) { this.commissionAgentName = commissionAgentName; }
    public Integer getPurchaseOrderDocNo() { return purchaseOrderDocNo; }
    public void setPurchaseOrderDocNo(Integer purchaseOrderDocNo) { this.purchaseOrderDocNo = purchaseOrderDocNo; }

    public List<DetailDto> getGrnSupplierLoadingDetailList() { return grnSupplierLoadingDetailList; }
    public void setGrnSupplierLoadingDetailList(List<DetailDto> v) { this.grnSupplierLoadingDetailList = v; }
    public List<EmptyBagDto> getGrnSupplierLoadingEmptyBagDetailList() { return grnSupplierLoadingEmptyBagDetailList; }
    public void setGrnSupplierLoadingEmptyBagDetailList(List<EmptyBagDto> v) { this.grnSupplierLoadingEmptyBagDetailList = v; }
    public List<ExpenseDto> getGrnSupplierLoadingExpenseDetailList() { return grnSupplierLoadingExpenseDetailList; }
    public void setGrnSupplierLoadingExpenseDetailList(List<ExpenseDto> v) { this.grnSupplierLoadingExpenseDetailList = v; }
}
