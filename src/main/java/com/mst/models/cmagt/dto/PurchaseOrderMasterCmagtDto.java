package com.mst.models.cmagt.dto;

import com.fasterxml.jackson.annotation.JsonAlias;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;

/**
 * CMAGT purchaseOrderMaster — the document behind BOTH Supplier Offer and Purchase Order Cmagt.
 *
 * WHY ONE DTO SERVES TWO SCREENS
 *   frmSupplierOfferCmagt:3357   -> purchaseOrderMaster.Save(obj), DocumentTypeId 1051
 *   frmPurchaseOrderCmagt        -> purchaseOrderMaster.Save(obj), DocumentTypeId 1052
 *   Same model, same BLL, same DAL, same procedures. Equivalence is proven at the contract
 *   level, so the shape is shared and only documentTypeId differs — set by each service.
 *
 * WHY THIS FILE EXISTS AT ALL
 *   The Java Supplier Offer was built against [cmagt].[USP_SupplierOfferMaster_InsertAndUpdate]
 *   and a SupplierOfferMaster BLL/DAL/model that NO DESKTOP FORM CONSTRUCTS. That family is
 *   orphaned code. A Supplier Offer saved from the web therefore landed in a different table
 *   family from the one the desktop reads, so neither app could see the other's documents.
 *   This is the third same-name trap in this port, after frmProductionJobOrder and the two
 *   frmProductionInput files.
 *
 * CONTRACT
 *   BLL purchaseOrderMaster.Save  ->  actionId = purchaseOrderMasterId == 0 ? 1 : 2
 *   DAL purchaseOrderMaster.SetData — ONE SqlTransaction, guard "Detail list not found", then:
 *     [cmagt].[USP_purchaseOrderMaster_InsertAndUpdate]              42 params
 *     [cmagt].[USP_purchaseOrderDetail_Insert]                       30 per row
 *     [cmagt].[USP_purchaseOrderPaymentDetail_Insert]                10 per row
 *     [cmagt].[USP_purchaseOrderEmptyBagDetail_Insert]               11 per row
 *     [cmagt].[USP_purchaseOrderSupplierExpenseDetail_Insert]        10 per row
 *     [cmagt].[USP_purchaseOrderCommissionDetail_Insert]             10 per row
 *     [cmagt].[USP_purchaseOrderSaleOrderMapping_Insert]             13 per row
 *     [cmagt].[USP_SupplierOfferBuyerInquiryMappingDetail_Insert]    15 per row
 *     Proc_DMSAttachments_Insert                                     attachments
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

public class PurchaseOrderMasterCmagtDto {

    private Boolean isApproved;
    private Boolean isSupplierOtherChargesAllowed;
    private Boolean isWhtApplied;
    private String approvedDate;
    private String deliveryStartDate;
    private String docDate;
    private String entryDate;
    private String modifyDate;
    private String ValidityDate;
    private BigDecimal fcyAmount;
    private BigDecimal fcyFxRate;
    private Integer actionId;
    private Integer approvedUserId;
    private Integer branchId;
    private Integer commissionAgentId;
    private Integer companyId;
    private Integer deliveryDays;
    private Integer deliveryTermId;
    private Integer docNo;
    private Integer documentTypeId;
    private Integer EBWeightDeductionTermId;
    private Integer entryUserId;
    private Integer fcyId;
    private Integer financialYearId;
    private Integer modifyUserId;
    private Integer organizationId;
    private Integer projectId;
    private Integer purchaseOrderMasterId;
    private Integer statusId;
    private Integer supplierId;
    private String approvalRemarks;
    private String attachmentsValues;
    private String customAttachmentsValues;
    private String remarksHeader;
    private String paymentScheduleDescription;
    private String purchaseOrderEmptyBagDetailDescription;
    private String purchaseOrderEmptyBagDetailDescriptionII;
    private String purchaseOrderSupplierExpenseDetailDescription;
    private String purchaseOrderCommissionDetailDescription;
    private String ShipToAddress;
    private Integer shipToAddressId;
    private Integer DeliveryToPartyId;

    /* Display only — the model marks these virtual, so SetProc skips them and they are NOT
       procedure parameters. */
    private String SupplierName;
    private String CommissionAgentName;
    private String DeliveryTerm;
    private String status;

    private List<DetailDto> purchaseOrderDetailList = new ArrayList<>();
    private List<PaymentDto> purchaseOrderPaymentDetailList = new ArrayList<>();
    private List<EmptyBagDto> purchaseOrderEmptyBagDetailList = new ArrayList<>();
    private List<SupplierExpenseDto> purchaseOrderSupplierExpenseDetailList = new ArrayList<>();
    private List<CommissionDto> purchaseOrderCommissionDetailList = new ArrayList<>();
    private List<SaleOrderMappingDto> purchaseOrderSaleOrderMappingList = new ArrayList<>();
    private List<OfferInquiryMappingDto> supplierOfferBuyerInquiryMappingDetailList = new ArrayList<>();

    /** [cmagt].[USP_purchaseOrderDetail_Insert] — 30 parameters, the non-virtual properties of
     *  Architecture.Model.CommissionAgent.purchaseOrderDetail in declaration order. */
    public static class DetailDto {
        private Boolean isApproved;
        private BigDecimal fcyAmount;
        private BigDecimal ItemAmount;
        private BigDecimal itemQty;
        private BigDecimal itemRate;
        private BigDecimal itemWeight;
        private BigDecimal TaxAmount;
        private BigDecimal TaxPercent;
        private BigDecimal TotalAmount;
        private Integer actionTypeId;
        private Integer cropYearId;
        private Integer inquiryBookingDetailId;
        private Integer inquiryBookingMasterId;
        private Integer inventoryParentCategoryId;
        private Integer itemId;
        private Integer packingTypeId;
        private Integer packUomId;
        private Integer purchaseOrderDetailId;
        private Integer purchaseOrderMasterId;
        private Integer rateUomId;
        private Integer buyerId;
        private Integer saleOrderDetailId;
        private Integer saleOrderMasterId;
        private Integer sortNo;
        private Integer supplierOfferDetailId;
        private Integer supplierOfferId;
        private Integer TaxNameId;
        private String cropYear;
        private String qualitySpecification;
        private String remarks;

        public Boolean isApproved() { return isApproved; }
        public void setIsApproved(Boolean isApproved) { this.isApproved = isApproved; }
        public BigDecimal getFcyAmount() { return fcyAmount; }
        public void setFcyAmount(BigDecimal fcyAmount) { this.fcyAmount = fcyAmount; }
        public BigDecimal getItemAmount() { return ItemAmount; }
        @JsonAlias({"ItemAmount"})
        public void setItemAmount(BigDecimal ItemAmount) { this.ItemAmount = ItemAmount; }
        public BigDecimal getItemQty() { return itemQty; }
        public void setItemQty(BigDecimal itemQty) { this.itemQty = itemQty; }
        public BigDecimal getItemRate() { return itemRate; }
        public void setItemRate(BigDecimal itemRate) { this.itemRate = itemRate; }
        public BigDecimal getItemWeight() { return itemWeight; }
        public void setItemWeight(BigDecimal itemWeight) { this.itemWeight = itemWeight; }
        public BigDecimal getTaxAmount() { return TaxAmount; }
        @JsonAlias({"TaxAmount"})
        public void setTaxAmount(BigDecimal TaxAmount) { this.TaxAmount = TaxAmount; }
        public BigDecimal getTaxPercent() { return TaxPercent; }
        @JsonAlias({"TaxPercent"})
        public void setTaxPercent(BigDecimal TaxPercent) { this.TaxPercent = TaxPercent; }
        public BigDecimal getTotalAmount() { return TotalAmount; }
        @JsonAlias({"TotalAmount"})
        public void setTotalAmount(BigDecimal TotalAmount) { this.TotalAmount = TotalAmount; }
        public Integer getActionTypeId() { return actionTypeId; }
        public void setActionTypeId(Integer actionTypeId) { this.actionTypeId = actionTypeId; }
        public Integer getCropYearId() { return cropYearId; }
        public void setCropYearId(Integer cropYearId) { this.cropYearId = cropYearId; }
        public Integer getInquiryBookingDetailId() { return inquiryBookingDetailId; }
        public void setInquiryBookingDetailId(Integer inquiryBookingDetailId) { this.inquiryBookingDetailId = inquiryBookingDetailId; }
        public Integer getInquiryBookingMasterId() { return inquiryBookingMasterId; }
        public void setInquiryBookingMasterId(Integer inquiryBookingMasterId) { this.inquiryBookingMasterId = inquiryBookingMasterId; }
        public Integer getInventoryParentCategoryId() { return inventoryParentCategoryId; }
        public void setInventoryParentCategoryId(Integer inventoryParentCategoryId) { this.inventoryParentCategoryId = inventoryParentCategoryId; }
        public Integer getItemId() { return itemId; }
        public void setItemId(Integer itemId) { this.itemId = itemId; }
        public Integer getPackingTypeId() { return packingTypeId; }
        public void setPackingTypeId(Integer packingTypeId) { this.packingTypeId = packingTypeId; }
        public Integer getPackUomId() { return packUomId; }
        public void setPackUomId(Integer packUomId) { this.packUomId = packUomId; }
        public Integer getPurchaseOrderDetailId() { return purchaseOrderDetailId; }
        public void setPurchaseOrderDetailId(Integer purchaseOrderDetailId) { this.purchaseOrderDetailId = purchaseOrderDetailId; }
        public Integer getPurchaseOrderMasterId() { return purchaseOrderMasterId; }
        public void setPurchaseOrderMasterId(Integer purchaseOrderMasterId) { this.purchaseOrderMasterId = purchaseOrderMasterId; }
        public Integer getRateUomId() { return rateUomId; }
        public void setRateUomId(Integer rateUomId) { this.rateUomId = rateUomId; }
        public Integer getBuyerId() { return buyerId; }
        public void setBuyerId(Integer buyerId) { this.buyerId = buyerId; }
        public Integer getSaleOrderDetailId() { return saleOrderDetailId; }
        public void setSaleOrderDetailId(Integer saleOrderDetailId) { this.saleOrderDetailId = saleOrderDetailId; }
        public Integer getSaleOrderMasterId() { return saleOrderMasterId; }
        public void setSaleOrderMasterId(Integer saleOrderMasterId) { this.saleOrderMasterId = saleOrderMasterId; }
        public Integer getSortNo() { return sortNo; }
        public void setSortNo(Integer sortNo) { this.sortNo = sortNo; }
        public Integer getSupplierOfferDetailId() { return supplierOfferDetailId; }
        public void setSupplierOfferDetailId(Integer supplierOfferDetailId) { this.supplierOfferDetailId = supplierOfferDetailId; }
        public Integer getSupplierOfferId() { return supplierOfferId; }
        public void setSupplierOfferId(Integer supplierOfferId) { this.supplierOfferId = supplierOfferId; }
        public Integer getTaxNameId() { return TaxNameId; }
        @JsonAlias({"TaxNameId"})
        public void setTaxNameId(Integer TaxNameId) { this.TaxNameId = TaxNameId; }
        public String getCropYear() { return cropYear; }
        public void setCropYear(String cropYear) { this.cropYear = cropYear; }
        public String getQualitySpecification() { return qualitySpecification; }
        public void setQualitySpecification(String qualitySpecification) { this.qualitySpecification = qualitySpecification; }

        /* Display-only (model: virtual ItemName). Never a procedure parameter - the repository
           lists its parameters explicitly. Read by the validator for the desktop's own
           "No mapping found for Item <name>" message (Insert():3297). */
        @JsonAlias({"ItemName"})
        private String itemName;
        public String getItemName() { return itemName; }
        public void setItemName(String itemName) { this.itemName = itemName; }
        public String getRemarks() { return remarks; }
        public void setRemarks(String remarks) { this.remarks = remarks; }
    }

    /** [cmagt].[USP_purchaseOrderPaymentDetail_Insert] — 10 parameters, the non-virtual properties of
     *  Architecture.Model.CommissionAgent.purchaseOrderPaymentDetail in declaration order. */
    public static class PaymentDto {
        private String DueDate;
        private BigDecimal dueAmount;
        private BigDecimal pctOfTotal;
        private Integer BaseDueDateTypeId;
        private Integer DueDays;
        private Integer PaymentTermId;
        private Integer purchaseOrderMasterId;
        private Integer purchaseOrderPaymentDetailId;
        private Integer sortNo;
        private String remarks;

        public String getDueDate() { return DueDate; }
        @JsonAlias({"DueDate"})
        public void setDueDate(String DueDate) { this.DueDate = DueDate; }
        public BigDecimal getDueAmount() { return dueAmount; }
        public void setDueAmount(BigDecimal dueAmount) { this.dueAmount = dueAmount; }
        public BigDecimal getPctOfTotal() { return pctOfTotal; }
        public void setPctOfTotal(BigDecimal pctOfTotal) { this.pctOfTotal = pctOfTotal; }
        public Integer getBaseDueDateTypeId() { return BaseDueDateTypeId; }
        @JsonAlias({"BaseDueDateTypeId"})
        public void setBaseDueDateTypeId(Integer BaseDueDateTypeId) { this.BaseDueDateTypeId = BaseDueDateTypeId; }
        public Integer getDueDays() { return DueDays; }
        @JsonAlias({"DueDays"})
        public void setDueDays(Integer DueDays) { this.DueDays = DueDays; }
        public Integer getPaymentTermId() { return PaymentTermId; }
        @JsonAlias({"PaymentTermId"})
        public void setPaymentTermId(Integer PaymentTermId) { this.PaymentTermId = PaymentTermId; }
        public Integer getPurchaseOrderMasterId() { return purchaseOrderMasterId; }
        public void setPurchaseOrderMasterId(Integer purchaseOrderMasterId) { this.purchaseOrderMasterId = purchaseOrderMasterId; }
        public Integer getPurchaseOrderPaymentDetailId() { return purchaseOrderPaymentDetailId; }
        public void setPurchaseOrderPaymentDetailId(Integer purchaseOrderPaymentDetailId) { this.purchaseOrderPaymentDetailId = purchaseOrderPaymentDetailId; }
        public Integer getSortNo() { return sortNo; }
        public void setSortNo(Integer sortNo) { this.sortNo = sortNo; }
        public String getRemarks() { return remarks; }
        public void setRemarks(String remarks) { this.remarks = remarks; }
    }

    /** [cmagt].[USP_purchaseOrderEmptyBagDetail_Insert] — 11 parameters, the non-virtual properties of
     *  Architecture.Model.CommissionAgent.purchaseOrderEmptyBagDetail in declaration order. */
    public static class EmptyBagDto {
        private BigDecimal Rate;
        private BigDecimal weightCutKg;
        private Integer PackingTypeId;
        private Integer purchaseOrderEmptyBagDetailId;
        private Integer purchaseOrderMasterId;
        private Integer saleOrderEmptyBagDetailId;
        private Integer saleOrderMasterId;
        private Integer entryTypeId;
        private Integer emptyBagPackingMaterialItemId;
        private Integer sortNo;
        private String remarks;

        public BigDecimal getRate() { return Rate; }
        @JsonAlias({"Rate"})
        public void setRate(BigDecimal Rate) { this.Rate = Rate; }
        public BigDecimal getWeightCutKg() { return weightCutKg; }
        public void setWeightCutKg(BigDecimal weightCutKg) { this.weightCutKg = weightCutKg; }
        public Integer getPackingTypeId() { return PackingTypeId; }
        @JsonAlias({"PackingTypeId"})
        public void setPackingTypeId(Integer PackingTypeId) { this.PackingTypeId = PackingTypeId; }
        public Integer getPurchaseOrderEmptyBagDetailId() { return purchaseOrderEmptyBagDetailId; }
        public void setPurchaseOrderEmptyBagDetailId(Integer purchaseOrderEmptyBagDetailId) { this.purchaseOrderEmptyBagDetailId = purchaseOrderEmptyBagDetailId; }
        public Integer getPurchaseOrderMasterId() { return purchaseOrderMasterId; }
        public void setPurchaseOrderMasterId(Integer purchaseOrderMasterId) { this.purchaseOrderMasterId = purchaseOrderMasterId; }
        public Integer getSaleOrderEmptyBagDetailId() { return saleOrderEmptyBagDetailId; }
        public void setSaleOrderEmptyBagDetailId(Integer saleOrderEmptyBagDetailId) { this.saleOrderEmptyBagDetailId = saleOrderEmptyBagDetailId; }
        public Integer getSaleOrderMasterId() { return saleOrderMasterId; }
        public void setSaleOrderMasterId(Integer saleOrderMasterId) { this.saleOrderMasterId = saleOrderMasterId; }
        public Integer getEntryTypeId() { return entryTypeId; }
        public void setEntryTypeId(Integer entryTypeId) { this.entryTypeId = entryTypeId; }
        public Integer getEmptyBagPackingMaterialItemId() { return emptyBagPackingMaterialItemId; }
        public void setEmptyBagPackingMaterialItemId(Integer emptyBagPackingMaterialItemId) { this.emptyBagPackingMaterialItemId = emptyBagPackingMaterialItemId; }
        public Integer getSortNo() { return sortNo; }
        public void setSortNo(Integer sortNo) { this.sortNo = sortNo; }
        public String getRemarks() { return remarks; }
        public void setRemarks(String remarks) { this.remarks = remarks; }
    }

    /** [cmagt].[USP_purchaseOrderSupplierExpenseDetail_Insert] — 10 parameters, the non-virtual properties of
     *  Architecture.Model.CommissionAgent.purchaseOrderSupplierExpenseDetail in declaration order. */
    public static class SupplierExpenseDto {
        private BigDecimal Qty;
        private Double amount;
        private Double rate;
        private Integer ItemId;
        private Integer purchaseOrderMasterId;
        private Integer purchaseOrderSupplierExpenseDetailId;
        private Integer sortNo;
        private Integer saleOrderMasterId;
        private Integer saleOrderBuyerOtherExpenseDetailId;
        private String remarks;

        public BigDecimal getQty() { return Qty; }
        @JsonAlias({"Qty"})
        public void setQty(BigDecimal Qty) { this.Qty = Qty; }
        public Double getAmount() { return amount; }
        public void setAmount(Double amount) { this.amount = amount; }
        public Double getRate() { return rate; }
        public void setRate(Double rate) { this.rate = rate; }
        public Integer getItemId() { return ItemId; }
        @JsonAlias({"ItemId"})
        public void setItemId(Integer ItemId) { this.ItemId = ItemId; }
        public Integer getPurchaseOrderMasterId() { return purchaseOrderMasterId; }
        public void setPurchaseOrderMasterId(Integer purchaseOrderMasterId) { this.purchaseOrderMasterId = purchaseOrderMasterId; }
        public Integer getPurchaseOrderSupplierExpenseDetailId() { return purchaseOrderSupplierExpenseDetailId; }
        public void setPurchaseOrderSupplierExpenseDetailId(Integer purchaseOrderSupplierExpenseDetailId) { this.purchaseOrderSupplierExpenseDetailId = purchaseOrderSupplierExpenseDetailId; }
        public Integer getSortNo() { return sortNo; }
        public void setSortNo(Integer sortNo) { this.sortNo = sortNo; }
        public Integer getSaleOrderMasterId() { return saleOrderMasterId; }
        public void setSaleOrderMasterId(Integer saleOrderMasterId) { this.saleOrderMasterId = saleOrderMasterId; }
        public Integer getSaleOrderBuyerOtherExpenseDetailId() { return saleOrderBuyerOtherExpenseDetailId; }
        public void setSaleOrderBuyerOtherExpenseDetailId(Integer saleOrderBuyerOtherExpenseDetailId) { this.saleOrderBuyerOtherExpenseDetailId = saleOrderBuyerOtherExpenseDetailId; }
        public String getRemarks() { return remarks; }
        public void setRemarks(String remarks) { this.remarks = remarks; }
    }

    /** [cmagt].[USP_purchaseOrderCommissionDetail_Insert] — 10 parameters, the non-virtual properties of
     *  Architecture.Model.CommissionAgent.purchaseOrderCommissionDetail in declaration order. */
    public static class CommissionDto {
        private BigDecimal commissionAmount;
        private BigDecimal commissionRate;
        private Integer agentTypeId;
        private Integer commissionAgentId;
        private Integer commissionTypeId;
        private Integer purchaseOrderCommissionDetailId;
        private Integer purchaseOrderMasterId;
        private Integer rateUomId;
        private Integer sortNo;
        private String commissionRemarks;

        public BigDecimal getCommissionAmount() { return commissionAmount; }
        public void setCommissionAmount(BigDecimal commissionAmount) { this.commissionAmount = commissionAmount; }
        public BigDecimal getCommissionRate() { return commissionRate; }
        public void setCommissionRate(BigDecimal commissionRate) { this.commissionRate = commissionRate; }
        public Integer getAgentTypeId() { return agentTypeId; }
        public void setAgentTypeId(Integer agentTypeId) { this.agentTypeId = agentTypeId; }
        public Integer getCommissionAgentId() { return commissionAgentId; }
        public void setCommissionAgentId(Integer commissionAgentId) { this.commissionAgentId = commissionAgentId; }
        public Integer getCommissionTypeId() { return commissionTypeId; }
        public void setCommissionTypeId(Integer commissionTypeId) { this.commissionTypeId = commissionTypeId; }
        public Integer getPurchaseOrderCommissionDetailId() { return purchaseOrderCommissionDetailId; }
        public void setPurchaseOrderCommissionDetailId(Integer purchaseOrderCommissionDetailId) { this.purchaseOrderCommissionDetailId = purchaseOrderCommissionDetailId; }
        public Integer getPurchaseOrderMasterId() { return purchaseOrderMasterId; }
        public void setPurchaseOrderMasterId(Integer purchaseOrderMasterId) { this.purchaseOrderMasterId = purchaseOrderMasterId; }
        public Integer getRateUomId() { return rateUomId; }
        public void setRateUomId(Integer rateUomId) { this.rateUomId = rateUomId; }
        public Integer getSortNo() { return sortNo; }
        public void setSortNo(Integer sortNo) { this.sortNo = sortNo; }
        public String getCommissionRemarks() { return commissionRemarks; }
        public void setCommissionRemarks(String commissionRemarks) { this.commissionRemarks = commissionRemarks; }
    }

    /** [cmagt].[USP_purchaseOrderSaleOrderMapping_Insert] — 13 parameters, the non-virtual properties of
     *  Architecture.Model.CommissionAgent.purchaseOrderSaleOrderMapping in declaration order. */
    public static class SaleOrderMappingDto {
        private BigDecimal itemNetWeight;
        private BigDecimal itemQty;
        private Integer buyerId;
        private Integer itemId;
        private Integer purchaseOrderDetailId;
        private Integer purchaseOrderMasterId;
        private Integer purchaseOrderSaleOrderMappingId;
        private Integer saleOrderDetailId;
        private Integer saleOrderMasterId;
        private Integer sortNo;
        private Integer supplierId;
        private Integer actionTypeId;
        private String Remarks;

        public BigDecimal getItemNetWeight() { return itemNetWeight; }
        public void setItemNetWeight(BigDecimal itemNetWeight) { this.itemNetWeight = itemNetWeight; }
        public BigDecimal getItemQty() { return itemQty; }
        public void setItemQty(BigDecimal itemQty) { this.itemQty = itemQty; }
        public Integer getBuyerId() { return buyerId; }
        public void setBuyerId(Integer buyerId) { this.buyerId = buyerId; }
        public Integer getItemId() { return itemId; }
        public void setItemId(Integer itemId) { this.itemId = itemId; }
        public Integer getPurchaseOrderDetailId() { return purchaseOrderDetailId; }
        public void setPurchaseOrderDetailId(Integer purchaseOrderDetailId) { this.purchaseOrderDetailId = purchaseOrderDetailId; }
        public Integer getPurchaseOrderMasterId() { return purchaseOrderMasterId; }
        public void setPurchaseOrderMasterId(Integer purchaseOrderMasterId) { this.purchaseOrderMasterId = purchaseOrderMasterId; }
        public Integer getPurchaseOrderSaleOrderMappingId() { return purchaseOrderSaleOrderMappingId; }
        public void setPurchaseOrderSaleOrderMappingId(Integer purchaseOrderSaleOrderMappingId) { this.purchaseOrderSaleOrderMappingId = purchaseOrderSaleOrderMappingId; }

        /* Display/validation only, never procedure parameters (the model's ValidityDate and
           ItemName are virtual). frmPurchaseOrderCmagt Insert() reads the sale order's expiry
           from the mapping grid (:3264) and refuses a row whose date is before the PO's
           (:3279); ItemName names an unknown item in the :3290 message. */
        @JsonAlias({"ValidityDate"})
        private String validityDate;
        @JsonAlias({"ItemName"})
        private String itemName;
        public String getValidityDate() { return validityDate; }
        public void setValidityDate(String validityDate) { this.validityDate = validityDate; }
        public String getItemName() { return itemName; }
        public void setItemName(String itemName) { this.itemName = itemName; }
        public Integer getSaleOrderDetailId() { return saleOrderDetailId; }
        public void setSaleOrderDetailId(Integer saleOrderDetailId) { this.saleOrderDetailId = saleOrderDetailId; }
        public Integer getSaleOrderMasterId() { return saleOrderMasterId; }
        public void setSaleOrderMasterId(Integer saleOrderMasterId) { this.saleOrderMasterId = saleOrderMasterId; }
        public Integer getSortNo() { return sortNo; }
        public void setSortNo(Integer sortNo) { this.sortNo = sortNo; }
        public Integer getSupplierId() { return supplierId; }
        public void setSupplierId(Integer supplierId) { this.supplierId = supplierId; }
        public Integer getActionTypeId() { return actionTypeId; }
        public void setActionTypeId(Integer actionTypeId) { this.actionTypeId = actionTypeId; }
        public String getRemarks() { return Remarks; }
        @JsonAlias({"Remarks"})
        public void setRemarks(String Remarks) { this.Remarks = Remarks; }
    }

    /** [cmagt].[USP_SupplierOfferBuyerInquiryMappingDetail_Insert] — 15 parameters, the non-virtual properties of
     *  Architecture.Model.CommissionAgent.SupplierOfferBuyerInquiryMappingDetail in declaration order. */
    public static class OfferInquiryMappingDto {
        private BigDecimal itemNetWeight;
        private BigDecimal itemQty;
        private Integer actionTypeId;
        private Integer buyerId;
        private Integer inquiryBookingDetailId;
        private Integer inquiryBookingMasterId;
        private Integer itemId;
        private Integer purchaseOrderDetailId;
        private Integer purchaseOrderMasterId;
        private Integer sortNo;
        private Integer supplierId;
        private Integer SupplierOfferBuyerInquiryMappingDetailId;
        private Integer supplierOfferDetailId;
        private Integer supplierOfferMasterId;
        private String Remarks;

        public BigDecimal getItemNetWeight() { return itemNetWeight; }
        public void setItemNetWeight(BigDecimal itemNetWeight) { this.itemNetWeight = itemNetWeight; }
        public BigDecimal getItemQty() { return itemQty; }
        public void setItemQty(BigDecimal itemQty) { this.itemQty = itemQty; }
        public Integer getActionTypeId() { return actionTypeId; }
        public void setActionTypeId(Integer actionTypeId) { this.actionTypeId = actionTypeId; }
        public Integer getBuyerId() { return buyerId; }
        public void setBuyerId(Integer buyerId) { this.buyerId = buyerId; }
        public Integer getInquiryBookingDetailId() { return inquiryBookingDetailId; }
        public void setInquiryBookingDetailId(Integer inquiryBookingDetailId) { this.inquiryBookingDetailId = inquiryBookingDetailId; }
        public Integer getInquiryBookingMasterId() { return inquiryBookingMasterId; }
        public void setInquiryBookingMasterId(Integer inquiryBookingMasterId) { this.inquiryBookingMasterId = inquiryBookingMasterId; }
        public Integer getItemId() { return itemId; }
        public void setItemId(Integer itemId) { this.itemId = itemId; }
        public Integer getPurchaseOrderDetailId() { return purchaseOrderDetailId; }
        public void setPurchaseOrderDetailId(Integer purchaseOrderDetailId) { this.purchaseOrderDetailId = purchaseOrderDetailId; }
        public Integer getPurchaseOrderMasterId() { return purchaseOrderMasterId; }
        public void setPurchaseOrderMasterId(Integer purchaseOrderMasterId) { this.purchaseOrderMasterId = purchaseOrderMasterId; }
        public Integer getSortNo() { return sortNo; }
        public void setSortNo(Integer sortNo) { this.sortNo = sortNo; }
        public Integer getSupplierId() { return supplierId; }
        public void setSupplierId(Integer supplierId) { this.supplierId = supplierId; }
        public Integer getSupplierOfferBuyerInquiryMappingDetailId() { return SupplierOfferBuyerInquiryMappingDetailId; }
        @JsonAlias({"SupplierOfferBuyerInquiryMappingDetailId"})
        public void setSupplierOfferBuyerInquiryMappingDetailId(Integer SupplierOfferBuyerInquiryMappingDetailId) { this.SupplierOfferBuyerInquiryMappingDetailId = SupplierOfferBuyerInquiryMappingDetailId; }
        public Integer getSupplierOfferDetailId() { return supplierOfferDetailId; }
        public void setSupplierOfferDetailId(Integer supplierOfferDetailId) { this.supplierOfferDetailId = supplierOfferDetailId; }
        public Integer getSupplierOfferMasterId() { return supplierOfferMasterId; }
        public void setSupplierOfferMasterId(Integer supplierOfferMasterId) { this.supplierOfferMasterId = supplierOfferMasterId; }
        public String getRemarks() { return Remarks; }
        @JsonAlias({"Remarks"})
        public void setRemarks(String Remarks) { this.Remarks = Remarks; }
    }

    public Boolean isApproved() { return isApproved; }
    public void setIsApproved(Boolean isApproved) { this.isApproved = isApproved; }
    public Boolean isSupplierOtherChargesAllowed() { return isSupplierOtherChargesAllowed; }
    public void setIsSupplierOtherChargesAllowed(Boolean isSupplierOtherChargesAllowed) { this.isSupplierOtherChargesAllowed = isSupplierOtherChargesAllowed; }
    public Boolean isWhtApplied() { return isWhtApplied; }
    public void setIsWhtApplied(Boolean isWhtApplied) { this.isWhtApplied = isWhtApplied; }
    public String getApprovedDate() { return approvedDate; }
    public void setApprovedDate(String approvedDate) { this.approvedDate = approvedDate; }
    public String getDeliveryStartDate() { return deliveryStartDate; }
    public void setDeliveryStartDate(String deliveryStartDate) { this.deliveryStartDate = deliveryStartDate; }
    public String getDocDate() { return docDate; }
    public void setDocDate(String docDate) { this.docDate = docDate; }
    public String getEntryDate() { return entryDate; }
    public void setEntryDate(String entryDate) { this.entryDate = entryDate; }
    public String getModifyDate() { return modifyDate; }
    public void setModifyDate(String modifyDate) { this.modifyDate = modifyDate; }
    public String getValidityDate() { return ValidityDate; }
    @JsonAlias({"ValidityDate"})
    public void setValidityDate(String ValidityDate) { this.ValidityDate = ValidityDate; }
    public BigDecimal getFcyAmount() { return fcyAmount; }
    public void setFcyAmount(BigDecimal fcyAmount) { this.fcyAmount = fcyAmount; }
    public BigDecimal getFcyFxRate() { return fcyFxRate; }
    public void setFcyFxRate(BigDecimal fcyFxRate) { this.fcyFxRate = fcyFxRate; }
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
    public Integer getEBWeightDeductionTermId() { return EBWeightDeductionTermId; }
    @JsonAlias({"EBWeightDeductionTermId", "ebWeightDeductionTermId"})
    public void setEBWeightDeductionTermId(Integer EBWeightDeductionTermId) { this.EBWeightDeductionTermId = EBWeightDeductionTermId; }
    public Integer getEntryUserId() { return entryUserId; }
    public void setEntryUserId(Integer entryUserId) { this.entryUserId = entryUserId; }
    public Integer getFcyId() { return fcyId; }
    public void setFcyId(Integer fcyId) { this.fcyId = fcyId; }
    public Integer getFinancialYearId() { return financialYearId; }
    public void setFinancialYearId(Integer financialYearId) { this.financialYearId = financialYearId; }
    public Integer getModifyUserId() { return modifyUserId; }
    public void setModifyUserId(Integer modifyUserId) { this.modifyUserId = modifyUserId; }
    public Integer getOrganizationId() { return organizationId; }
    public void setOrganizationId(Integer organizationId) { this.organizationId = organizationId; }
    public Integer getProjectId() { return projectId; }
    public void setProjectId(Integer projectId) { this.projectId = projectId; }
    public Integer getPurchaseOrderMasterId() { return purchaseOrderMasterId; }
    public void setPurchaseOrderMasterId(Integer purchaseOrderMasterId) { this.purchaseOrderMasterId = purchaseOrderMasterId; }
    public Integer getStatusId() { return statusId; }
    public void setStatusId(Integer statusId) { this.statusId = statusId; }
    public Integer getSupplierId() { return supplierId; }
    public void setSupplierId(Integer supplierId) { this.supplierId = supplierId; }
    public String getApprovalRemarks() { return approvalRemarks; }
    public void setApprovalRemarks(String approvalRemarks) { this.approvalRemarks = approvalRemarks; }
    public String getAttachmentsValues() { return attachmentsValues; }
    public void setAttachmentsValues(String attachmentsValues) { this.attachmentsValues = attachmentsValues; }
    public String getCustomAttachmentsValues() { return customAttachmentsValues; }
    public void setCustomAttachmentsValues(String customAttachmentsValues) { this.customAttachmentsValues = customAttachmentsValues; }
    public String getRemarksHeader() { return remarksHeader; }
    public void setRemarksHeader(String remarksHeader) { this.remarksHeader = remarksHeader; }
    public String getPaymentScheduleDescription() { return paymentScheduleDescription; }
    public void setPaymentScheduleDescription(String paymentScheduleDescription) { this.paymentScheduleDescription = paymentScheduleDescription; }
    public String getPurchaseOrderEmptyBagDetailDescription() { return purchaseOrderEmptyBagDetailDescription; }
    public void setPurchaseOrderEmptyBagDetailDescription(String purchaseOrderEmptyBagDetailDescription) { this.purchaseOrderEmptyBagDetailDescription = purchaseOrderEmptyBagDetailDescription; }
    public String getPurchaseOrderEmptyBagDetailDescriptionII() { return purchaseOrderEmptyBagDetailDescriptionII; }
    public void setPurchaseOrderEmptyBagDetailDescriptionII(String purchaseOrderEmptyBagDetailDescriptionII) { this.purchaseOrderEmptyBagDetailDescriptionII = purchaseOrderEmptyBagDetailDescriptionII; }
    public String getPurchaseOrderSupplierExpenseDetailDescription() { return purchaseOrderSupplierExpenseDetailDescription; }
    public void setPurchaseOrderSupplierExpenseDetailDescription(String purchaseOrderSupplierExpenseDetailDescription) { this.purchaseOrderSupplierExpenseDetailDescription = purchaseOrderSupplierExpenseDetailDescription; }
    public String getPurchaseOrderCommissionDetailDescription() { return purchaseOrderCommissionDetailDescription; }
    public void setPurchaseOrderCommissionDetailDescription(String purchaseOrderCommissionDetailDescription) { this.purchaseOrderCommissionDetailDescription = purchaseOrderCommissionDetailDescription; }
    public String getShipToAddress() { return ShipToAddress; }
    @JsonAlias({"ShipToAddress"})
    public void setShipToAddress(String ShipToAddress) { this.ShipToAddress = ShipToAddress; }
    public Integer getShipToAddressId() { return shipToAddressId; }
    public void setShipToAddressId(Integer shipToAddressId) { this.shipToAddressId = shipToAddressId; }
    public Integer getDeliveryToPartyId() { return DeliveryToPartyId; }
    @JsonAlias({"DeliveryToPartyId"})
    public void setDeliveryToPartyId(Integer DeliveryToPartyId) { this.DeliveryToPartyId = DeliveryToPartyId; }
    public String getSupplierName() { return SupplierName; }
    @JsonAlias({"SupplierName"})
    public void setSupplierName(String SupplierName) { this.SupplierName = SupplierName; }
    public String getCommissionAgentName() { return CommissionAgentName; }
    @JsonAlias({"CommissionAgentName"})
    public void setCommissionAgentName(String CommissionAgentName) { this.CommissionAgentName = CommissionAgentName; }
    public String getDeliveryTerm() { return DeliveryTerm; }
    @JsonAlias({"DeliveryTerm"})
    public void setDeliveryTerm(String DeliveryTerm) { this.DeliveryTerm = DeliveryTerm; }
    public String getStatus() { return status; }
    public void setStatus(String status) { this.status = status; }

    public List<DetailDto> getPurchaseOrderDetailList() { return purchaseOrderDetailList; }
    public void setPurchaseOrderDetailList(List<DetailDto> v) { this.purchaseOrderDetailList = v; }
    public List<PaymentDto> getPurchaseOrderPaymentDetailList() { return purchaseOrderPaymentDetailList; }
    public void setPurchaseOrderPaymentDetailList(List<PaymentDto> v) { this.purchaseOrderPaymentDetailList = v; }
    public List<EmptyBagDto> getPurchaseOrderEmptyBagDetailList() { return purchaseOrderEmptyBagDetailList; }
    public void setPurchaseOrderEmptyBagDetailList(List<EmptyBagDto> v) { this.purchaseOrderEmptyBagDetailList = v; }
    public List<SupplierExpenseDto> getPurchaseOrderSupplierExpenseDetailList() { return purchaseOrderSupplierExpenseDetailList; }
    public void setPurchaseOrderSupplierExpenseDetailList(List<SupplierExpenseDto> v) { this.purchaseOrderSupplierExpenseDetailList = v; }
    public List<CommissionDto> getPurchaseOrderCommissionDetailList() { return purchaseOrderCommissionDetailList; }
    public void setPurchaseOrderCommissionDetailList(List<CommissionDto> v) { this.purchaseOrderCommissionDetailList = v; }
    public List<SaleOrderMappingDto> getPurchaseOrderSaleOrderMappingList() { return purchaseOrderSaleOrderMappingList; }
    public void setPurchaseOrderSaleOrderMappingList(List<SaleOrderMappingDto> v) { this.purchaseOrderSaleOrderMappingList = v; }
    public List<OfferInquiryMappingDto> getSupplierOfferBuyerInquiryMappingDetailList() { return supplierOfferBuyerInquiryMappingDetailList; }
    public void setSupplierOfferBuyerInquiryMappingDetailList(List<OfferInquiryMappingDto> v) { this.supplierOfferBuyerInquiryMappingDetailList = v; }
}
