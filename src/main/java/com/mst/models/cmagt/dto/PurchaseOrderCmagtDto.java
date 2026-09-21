package com.mst.models.cmagt.dto;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;

public class PurchaseOrderCmagtDto {

    private Integer purchaseOrderMasterId;
    private Integer docNo;
    private String docDate;
    private String deliveryStartDate;
    private String validityDate;
    private Integer deliveryDays;
    private Integer supplierId;
    private String supplierName;
    private Integer commissionAgentId;
    private String commissionAgentName;
    private Integer deliveryTermId;
    private String deliveryTerm;
    private Boolean isSupplierOtherChargesAllowed;
    private Boolean isWhtApplied;
    private Integer ebWeightDeductionTermId;
    private String remarksHeader;
    private Integer shipToAddressId;
    private Integer deliveryToPartyId;
    private String shipToAddress;
    private String paymentScheduleDescription;

    private Integer organizationId;
    private Integer companyId;
    private Integer branchId;
    private Integer financialYearId;
    private Integer entryUserId;
    private Integer modifyUserId;
    private Integer documentTypeId = 1052;

    private List<ItemDetailDto> purchaseOrderDetailList = new ArrayList<>();
    private List<PaymentDetailDto> purchaseOrderPaymentDetailList = new ArrayList<>();
    private List<EmptyBagDetailDto> purchaseOrderEmptyBagDetailList = new ArrayList<>();
    private List<ExpenseDetailDto> purchaseOrderSupplierExpenseDetailList = new ArrayList<>();
    private List<SaleOrderMapDto> purchaseOrderSaleOrderMappingList = new ArrayList<>();

    public static class ItemDetailDto {
        private Integer purchaseOrderDetailId;
        private Integer purchaseOrderMasterId;
        private Integer itemId;
        private String itemName;
        private Integer cropYearId;
        private String cropYear;
        private Integer packTypeId;
        private String packTypeDesc;
        private BigDecimal weight;
        private BigDecimal qty;
        private Integer packUomId;
        private String packUomName;
        private BigDecimal rate;
        private Integer rateUomId;
        private String rateUomName;
        private BigDecimal amount;
        private Integer taxId;
        private BigDecimal taxPercent;
        private BigDecimal taxAmount;
        private BigDecimal totalAmount;
        private String remarksDetail;

        private Integer commissionTypeId;
        private BigDecimal commissionRate;
        private Integer commissionRateUomId;
        private BigDecimal commissionAmount;
        private Integer commissionGlAccountId;

        private Integer brokeryTypeId;
        private BigDecimal brokeryRate;
        private Integer brokeryRateUomId;
        private BigDecimal brokeryAmount;
        private Integer brokeryGlAccountId;

        public Integer getPurchaseOrderDetailId() { return purchaseOrderDetailId; }
        public void setPurchaseOrderDetailId(Integer purchaseOrderDetailId) { this.purchaseOrderDetailId = purchaseOrderDetailId; }
        public Integer getPurchaseOrderMasterId() { return purchaseOrderMasterId; }
        public void setPurchaseOrderMasterId(Integer purchaseOrderMasterId) { this.purchaseOrderMasterId = purchaseOrderMasterId; }
        public Integer getItemId() { return itemId; }
        public void setItemId(Integer itemId) { this.itemId = itemId; }
        public String getItemName() { return itemName; }
        public void setItemName(String itemName) { this.itemName = itemName; }
        public Integer getCropYearId() { return cropYearId; }
        public void setCropYearId(Integer cropYearId) { this.cropYearId = cropYearId; }
        public String getCropYear() { return cropYear; }
        public void setCropYear(String cropYear) { this.cropYear = cropYear; }
        public Integer getPackTypeId() { return packTypeId; }
        public void setPackTypeId(Integer packTypeId) { this.packTypeId = packTypeId; }
        public String getPackTypeDesc() { return packTypeDesc; }
        public void setPackTypeDesc(String packTypeDesc) { this.packTypeDesc = packTypeDesc; }
        public BigDecimal getWeight() { return weight; }
        public void setWeight(BigDecimal weight) { this.weight = weight; }
        public BigDecimal getQty() { return qty; }
        public void setQty(BigDecimal qty) { this.qty = qty; }
        public Integer getPackUomId() { return packUomId; }
        public void setPackUomId(Integer packUomId) { this.packUomId = packUomId; }
        public String getPackUomName() { return packUomName; }
        public void setPackUomName(String packUomName) { this.packUomName = packUomName; }
        public BigDecimal getRate() { return rate; }
        public void setRate(BigDecimal rate) { this.rate = rate; }
        public Integer getRateUomId() { return rateUomId; }
        public void setRateUomId(Integer rateUomId) { this.rateUomId = rateUomId; }
        public String getRateUomName() { return rateUomName; }
        public void setRateUomName(String rateUomName) { this.rateUomName = rateUomName; }
        public BigDecimal getAmount() { return amount; }
        public void setAmount(BigDecimal amount) { this.amount = amount; }
        public Integer getTaxId() { return taxId; }
        public void setTaxId(Integer taxId) { this.taxId = taxId; }
        public BigDecimal getTaxPercent() { return taxPercent; }
        public void setTaxPercent(BigDecimal taxPercent) { this.taxPercent = taxPercent; }
        public BigDecimal getTaxAmount() { return taxAmount; }
        public void setTaxAmount(BigDecimal taxAmount) { this.taxAmount = taxAmount; }
        public BigDecimal getTotalAmount() { return totalAmount; }
        public void setTotalAmount(BigDecimal totalAmount) { this.totalAmount = totalAmount; }
        public String getRemarksDetail() { return remarksDetail; }
        public void setRemarksDetail(String remarksDetail) { this.remarksDetail = remarksDetail; }
        public Integer getCommissionTypeId() { return commissionTypeId; }
        public void setCommissionTypeId(Integer commissionTypeId) { this.commissionTypeId = commissionTypeId; }
        public BigDecimal getCommissionRate() { return commissionRate; }
        public void setCommissionRate(BigDecimal commissionRate) { this.commissionRate = commissionRate; }
        public Integer getCommissionRateUomId() { return commissionRateUomId; }
        public void setCommissionRateUomId(Integer commissionRateUomId) { this.commissionRateUomId = commissionRateUomId; }
        public BigDecimal getCommissionAmount() { return commissionAmount; }
        public void setCommissionAmount(BigDecimal commissionAmount) { this.commissionAmount = commissionAmount; }
        public Integer getCommissionGlAccountId() { return commissionGlAccountId; }
        public void setCommissionGlAccountId(Integer commissionGlAccountId) { this.commissionGlAccountId = commissionGlAccountId; }
        public Integer getBrokeryTypeId() { return brokeryTypeId; }
        public void setBrokeryTypeId(Integer brokeryTypeId) { this.brokeryTypeId = brokeryTypeId; }
        public BigDecimal getBrokeryRate() { return brokeryRate; }
        public void setBrokeryRate(BigDecimal brokeryRate) { this.brokeryRate = brokeryRate; }
        public Integer getBrokeryRateUomId() { return brokeryRateUomId; }
        public void setBrokeryRateUomId(Integer brokeryRateUomId) { this.brokeryRateUomId = brokeryRateUomId; }
        public BigDecimal getBrokeryAmount() { return brokeryAmount; }
        public void setBrokeryAmount(BigDecimal brokeryAmount) { this.brokeryAmount = brokeryAmount; }
        public Integer getBrokeryGlAccountId() { return brokeryGlAccountId; }
        public void setBrokeryGlAccountId(Integer brokeryGlAccountId) { this.brokeryGlAccountId = brokeryGlAccountId; }
    }

    public static class PaymentDetailDto {
        private Integer purchaseOrderPaymentDetailId;
        private Integer purchaseOrderMasterId;
        private Integer paymentTermId;
        private Integer dueDays;
        private String dueDate;
        private BigDecimal amount;
        private Integer baseDateTypeId;
        private String remarks;

        public Integer getPurchaseOrderPaymentDetailId() { return purchaseOrderPaymentDetailId; }
        public void setPurchaseOrderPaymentDetailId(Integer purchaseOrderPaymentDetailId) { this.purchaseOrderPaymentDetailId = purchaseOrderPaymentDetailId; }
        public Integer getPurchaseOrderMasterId() { return purchaseOrderMasterId; }
        public void setPurchaseOrderMasterId(Integer purchaseOrderMasterId) { this.purchaseOrderMasterId = purchaseOrderMasterId; }
        public Integer getPaymentTermId() { return paymentTermId; }
        public void setPaymentTermId(Integer paymentTermId) { this.paymentTermId = paymentTermId; }
        public Integer getDueDays() { return dueDays; }
        public void setDueDays(Integer dueDays) { this.dueDays = dueDays; }
        public String getDueDate() { return dueDate; }
        public void setDueDate(String dueDate) { this.dueDate = dueDate; }
        public BigDecimal getAmount() { return amount; }
        public void setAmount(BigDecimal amount) { this.amount = amount; }
        public Integer getBaseDateTypeId() { return baseDateTypeId; }
        public void setBaseDateTypeId(Integer baseDateTypeId) { this.baseDateTypeId = baseDateTypeId; }
        public String getRemarks() { return remarks; }
        public void setRemarks(String remarks) { this.remarks = remarks; }
    }

    public static class EmptyBagDetailDto {
        private Integer purchaseOrderEmptyBagDetailId;
        private Integer purchaseOrderMasterId;
        private Integer itemId;
        private String itemName;
        private BigDecimal qty;
        private BigDecimal rate;
        private Integer rateUomId;
        private BigDecimal amount;
        private Integer packingTypeId;
        private String remarks;

        public Integer getPurchaseOrderEmptyBagDetailId() { return purchaseOrderEmptyBagDetailId; }
        public void setPurchaseOrderEmptyBagDetailId(Integer purchaseOrderEmptyBagDetailId) { this.purchaseOrderEmptyBagDetailId = purchaseOrderEmptyBagDetailId; }
        public Integer getPurchaseOrderMasterId() { return purchaseOrderMasterId; }
        public void setPurchaseOrderMasterId(Integer purchaseOrderMasterId) { this.purchaseOrderMasterId = purchaseOrderMasterId; }
        public Integer getItemId() { return itemId; }
        public void setItemId(Integer itemId) { this.itemId = itemId; }
        public String getItemName() { return itemName; }
        public void setItemName(String itemName) { this.itemName = itemName; }
        public BigDecimal getQty() { return qty; }
        public void setQty(BigDecimal qty) { this.qty = qty; }
        public BigDecimal getRate() { return rate; }
        public void setRate(BigDecimal rate) { this.rate = rate; }
        public Integer getRateUomId() { return rateUomId; }
        public void setRateUomId(Integer rateUomId) { this.rateUomId = rateUomId; }
        public BigDecimal getAmount() { return amount; }
        public void setAmount(BigDecimal amount) { this.amount = amount; }
        public Integer getPackingTypeId() { return packingTypeId; }
        public void setPackingTypeId(Integer packingTypeId) { this.packingTypeId = packingTypeId; }
        public String getRemarks() { return remarks; }
        public void setRemarks(String remarks) { this.remarks = remarks; }
    }

    public static class ExpenseDetailDto {
        private Integer purchaseOrderSupplierExpenseDetailId;
        private Integer purchaseOrderMasterId;
        private Integer otherItemId;
        private String otherItemName;
        private BigDecimal rate;
        private Integer rateUomId;
        private BigDecimal amount;
        private String remarks;

        public Integer getPurchaseOrderSupplierExpenseDetailId() { return purchaseOrderSupplierExpenseDetailId; }
        public void setPurchaseOrderSupplierExpenseDetailId(Integer purchaseOrderSupplierExpenseDetailId) { this.purchaseOrderSupplierExpenseDetailId = purchaseOrderSupplierExpenseDetailId; }
        public Integer getPurchaseOrderMasterId() { return purchaseOrderMasterId; }
        public void setPurchaseOrderMasterId(Integer purchaseOrderMasterId) { this.purchaseOrderMasterId = purchaseOrderMasterId; }
        public Integer getOtherItemId() { return otherItemId; }
        public void setOtherItemId(Integer otherItemId) { this.otherItemId = otherItemId; }
        public String getOtherItemName() { return otherItemName; }
        public void setOtherItemName(String otherItemName) { this.otherItemName = otherItemName; }
        public BigDecimal getRate() { return rate; }
        public void setRate(BigDecimal rate) { this.rate = rate; }
        public Integer getRateUomId() { return rateUomId; }
        public void setRateUomId(Integer rateUomId) { this.rateUomId = rateUomId; }
        public BigDecimal getAmount() { return amount; }
        public void setAmount(BigDecimal amount) { this.amount = amount; }
        public String getRemarks() { return remarks; }
        public void setRemarks(String remarks) { this.remarks = remarks; }
    }

    public static class SaleOrderMapDto {
        private Integer purchaseOrderSaleOrderMappingId;
        private Integer purchaseOrderMasterId;
        private Integer saleOrderMasterId;
        private Integer saleOrderDetailId;
        private Integer itemId;
        private BigDecimal qty;

        public Integer getPurchaseOrderSaleOrderMappingId() { return purchaseOrderSaleOrderMappingId; }
        public void setPurchaseOrderSaleOrderMappingId(Integer purchaseOrderSaleOrderMappingId) { this.purchaseOrderSaleOrderMappingId = purchaseOrderSaleOrderMappingId; }
        public Integer getPurchaseOrderMasterId() { return purchaseOrderMasterId; }
        public void setPurchaseOrderMasterId(Integer purchaseOrderMasterId) { this.purchaseOrderMasterId = purchaseOrderMasterId; }
        public Integer getSaleOrderMasterId() { return saleOrderMasterId; }
        public void setSaleOrderMasterId(Integer saleOrderMasterId) { this.saleOrderMasterId = saleOrderMasterId; }
        public Integer getSaleOrderDetailId() { return saleOrderDetailId; }
        public void setSaleOrderDetailId(Integer saleOrderDetailId) { this.saleOrderDetailId = saleOrderDetailId; }
        public Integer getItemId() { return itemId; }
        public void setItemId(Integer itemId) { this.itemId = itemId; }
        public BigDecimal getQty() { return qty; }
        public void setQty(BigDecimal qty) { this.qty = qty; }
    }

    // Getters and Setters
    public Integer getPurchaseOrderMasterId() { return purchaseOrderMasterId; }
    public void setPurchaseOrderMasterId(Integer purchaseOrderMasterId) { this.purchaseOrderMasterId = purchaseOrderMasterId; }
    public Integer getDocNo() { return docNo; }
    public void setDocNo(Integer docNo) { this.docNo = docNo; }
    public String getDocDate() { return docDate; }
    public void setDocDate(String docDate) { this.docDate = docDate; }
    public String getDeliveryStartDate() { return deliveryStartDate; }
    public void setDeliveryStartDate(String deliveryStartDate) { this.deliveryStartDate = deliveryStartDate; }
    public String getValidityDate() { return validityDate; }
    public void setValidityDate(String validityDate) { this.validityDate = validityDate; }
    public Integer getDeliveryDays() { return deliveryDays; }
    public void setDeliveryDays(Integer deliveryDays) { this.deliveryDays = deliveryDays; }
    public Integer getSupplierId() { return supplierId; }
    public void setSupplierId(Integer supplierId) { this.supplierId = supplierId; }
    public String getSupplierName() { return supplierName; }
    public void setSupplierName(String supplierName) { this.supplierName = supplierName; }
    public Integer getCommissionAgentId() { return commissionAgentId; }
    public void setCommissionAgentId(Integer commissionAgentId) { this.commissionAgentId = commissionAgentId; }
    public String getCommissionAgentName() { return commissionAgentName; }
    public void setCommissionAgentName(String commissionAgentName) { this.commissionAgentName = commissionAgentName; }
    public Integer getDeliveryTermId() { return deliveryTermId; }
    public void setDeliveryTermId(Integer deliveryTermId) { this.deliveryTermId = deliveryTermId; }
    public String getDeliveryTerm() { return deliveryTerm; }
    public void setDeliveryTerm(String deliveryTerm) { this.deliveryTerm = deliveryTerm; }
    public Boolean getIsSupplierOtherChargesAllowed() { return isSupplierOtherChargesAllowed; }
    public void setIsSupplierOtherChargesAllowed(Boolean isSupplierOtherChargesAllowed) { this.isSupplierOtherChargesAllowed = isSupplierOtherChargesAllowed; }
    public Boolean getIsWhtApplied() { return isWhtApplied; }
    public void setIsWhtApplied(Boolean isWhtApplied) { this.isWhtApplied = isWhtApplied; }
    public Integer getEbWeightDeductionTermId() { return ebWeightDeductionTermId; }
    public void setEbWeightDeductionTermId(Integer ebWeightDeductionTermId) { this.ebWeightDeductionTermId = ebWeightDeductionTermId; }
    public String getRemarksHeader() { return remarksHeader; }
    public void setRemarksHeader(String remarksHeader) { this.remarksHeader = remarksHeader; }
    public Integer getShipToAddressId() { return shipToAddressId; }
    public void setShipToAddressId(Integer shipToAddressId) { this.shipToAddressId = shipToAddressId; }
    public Integer getDeliveryToPartyId() { return deliveryToPartyId; }
    public void setDeliveryToPartyId(Integer deliveryToPartyId) { this.deliveryToPartyId = deliveryToPartyId; }
    public String getShipToAddress() { return shipToAddress; }
    public void setShipToAddress(String shipToAddress) { this.shipToAddress = shipToAddress; }
    public String getPaymentScheduleDescription() { return paymentScheduleDescription; }
    public void setPaymentScheduleDescription(String paymentScheduleDescription) { this.paymentScheduleDescription = paymentScheduleDescription; }

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

    public List<ItemDetailDto> getPurchaseOrderDetailList() { return purchaseOrderDetailList; }
    public void setPurchaseOrderDetailList(List<ItemDetailDto> purchaseOrderDetailList) { this.purchaseOrderDetailList = purchaseOrderDetailList; }
    public List<PaymentDetailDto> getPurchaseOrderPaymentDetailList() { return purchaseOrderPaymentDetailList; }
    public void setPurchaseOrderPaymentDetailList(List<PaymentDetailDto> purchaseOrderPaymentDetailList) { this.purchaseOrderPaymentDetailList = purchaseOrderPaymentDetailList; }
    public List<EmptyBagDetailDto> getPurchaseOrderEmptyBagDetailList() { return purchaseOrderEmptyBagDetailList; }
    public void setPurchaseOrderEmptyBagDetailList(List<EmptyBagDetailDto> purchaseOrderEmptyBagDetailList) { this.purchaseOrderEmptyBagDetailList = purchaseOrderEmptyBagDetailList; }
    public List<ExpenseDetailDto> getPurchaseOrderSupplierExpenseDetailList() { return purchaseOrderSupplierExpenseDetailList; }
    public void setPurchaseOrderSupplierExpenseDetailList(List<ExpenseDetailDto> purchaseOrderSupplierExpenseDetailList) { this.purchaseOrderSupplierExpenseDetailList = purchaseOrderSupplierExpenseDetailList; }
    public List<SaleOrderMapDto> getPurchaseOrderSaleOrderMappingList() { return purchaseOrderSaleOrderMappingList; }
    public void setPurchaseOrderSaleOrderMappingList(List<SaleOrderMapDto> purchaseOrderSaleOrderMappingList) { this.purchaseOrderSaleOrderMappingList = purchaseOrderSaleOrderMappingList; }
}
