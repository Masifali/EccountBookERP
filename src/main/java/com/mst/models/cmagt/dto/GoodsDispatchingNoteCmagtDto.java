package com.mst.models.cmagt.dto;

import com.fasterxml.jackson.annotation.JsonAlias;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;

/**
 * Goods Dispatching Note (1055) - model 1081 gdnBuyerDispatchMaster with its three child
 * collections (1078 detail, 1079 empty bags, 1080 expenses), exactly the collections DAL 0539
 * SetData writes. Field names follow the procedure parameters; @JsonAlias accepts the column
 * spelling the GetAllMethod read activities return (BuyerId, DeliverToAddressId, BillWeight,
 * EBWeightDeductionTermId ...) so a record read back can be posted back unchanged.
 *
 * driverName / driverMobileNo / saleOrderMasterId were removed from the header: none is a
 * parameter of USP_gdnBuyerDispatchMaster_InsertAndUpdate and neither driver string exists in
 * frmGoodsDispatchingNoteCmagt.cs. The detail's weight/qty/rate/amount/packTypeId/remarksDetail
 * were removed for the same reason - USP_gdnBuyerDispatchDetail_Insert has no such parameters.
 * grnDate is not persisted; it carries the loaded GRN's date for the Insert() date checks
 * (:1827-1850).
 */
public class GoodsDispatchingNoteCmagtDto {

    private Integer gdnBuyerDispatchMasterId;
    private Integer docNo;
    private String docDate;
    @JsonAlias({"BuyerId"})
    private Integer buyerId;
    @JsonAlias({"BuyerName"})
    private String buyerName;
    private Integer commissionAgentId;
    @JsonAlias({"CommissionAgentName"})
    private String commissionAgentName;
    private String vehicleNo;
    private String biltyNo;
    private String remarksHeader;
    private Integer organizationId;
    private Integer companyId;
    private Integer branchId;
    private Integer financialYearId;
    private Integer entryUserId;
    private Integer modifyUserId;
    private Integer documentTypeId;
    @JsonAlias({"BuyerRefDocNo"})
    private String buyerRefDocNo;
    @JsonAlias({"DeliverToPartyId"})
    private Integer deliverToPartyId;
    @JsonAlias({"DeliverToPartyName"})
    private String deliverToPartyName;
    @JsonAlias({"DeliverToAddressId"})
    private Integer deliverToAddressId;
    @JsonAlias({"DeliverToAddress"})
    private String deliverToAddress;
    private Integer loadingCityId;
    private Integer unloadingCityId;
    private Integer transporterId;
    private String transporterName;
    private BigDecimal biltyFreight;
    private BigDecimal otherAdLesCharges;
    private BigDecimal totalFreight;
    @JsonAlias({"FreightRemarks"})
    private String freightRemarks;
    private Integer vehicleTypeId;
    private String biltyDate;
    private Integer deliveryTermId;
    private BigDecimal biltyQty;
    private BigDecimal loadWeight;
    private BigDecimal tareWeight;
    private BigDecimal scaleNetWeight;
    @JsonAlias({"BillWeight"})
    private BigDecimal billWeight;
    private String warningRemarks;
    @JsonAlias({"GrnDate"})
    private String grnDate;
    @JsonAlias({"GdnBuyerDispatchDetailList"})
    private List<DetailDto> gdnBuyerDispatchDetailList = new ArrayList<>();
    private List<EmptyBagDto> gdnBuyerDispatchEmptyBagDetailList = new ArrayList<>();
    private List<ExpenseDto> gdnBuyerDispatchExpenseDetailList = new ArrayList<>();

    public static class DetailDto {
        private Integer gdnBuyerDispatchDetailId;
        private Integer gdnBuyerDispatchMasterId;
        private Integer supplierOfferId;
        private Integer supplierOfferDetailId;
        private Integer purchaseOrderMasterId;
        private Integer purchaseOrderDetailId;
        private Integer inquiryBookingMasterId;
        private Integer inquiryBookingDetailId;
        private Integer saleOrderMasterId;
        private Integer saleOrderDetailId;
        private Integer grnSupplierLoadingMasterId;
        private Integer grnSupplierLoadingDetailId;
        private Integer supplierId;
        @JsonAlias({"DeliverToAddressId"})
        private Integer deliverToAddressId;
        @JsonAlias({"DeliverToAddress"})
        private String deliverToAddress;
        private Integer loadingCityId;
        private Integer unloadingCityId;
        private Integer inventoryParentCategoryId;
        private Integer itemId;
        @JsonAlias({"ItemName"})
        private String itemName;
        private Integer packUomId;
        private Integer cropYearId;
        private String cropYear;
        private Integer packingTypeId;
        private BigDecimal loadingQty;
        private BigDecimal wbGrossWeight;
        private BigDecimal ebwPerUnit;
        private BigDecimal ebwTotal;
        private BigDecimal addLessWeight;
        private BigDecimal netBillWeight;
        private Integer sortNo;
        private String remarks;
        private Integer actionTypeId;
        @JsonAlias({"EBWeightDeductionTermId", "ebweightDeductionTermId"})
        private Integer ebWeightDeductionTermId;
        private Integer purchaseOrderSaleOrderMappingId;
        private String warningRemarks;
        @JsonAlias({"GrnDate"})
        private String grnDate;
        @JsonAlias({"GrnNo"})
        private Integer grnNo;

        public Integer getGdnBuyerDispatchDetailId() { return gdnBuyerDispatchDetailId; }
        public void setGdnBuyerDispatchDetailId(Integer v) { this.gdnBuyerDispatchDetailId = v; }
        public Integer getGdnBuyerDispatchMasterId() { return gdnBuyerDispatchMasterId; }
        public void setGdnBuyerDispatchMasterId(Integer v) { this.gdnBuyerDispatchMasterId = v; }
        public Integer getSupplierOfferId() { return supplierOfferId; }
        public void setSupplierOfferId(Integer v) { this.supplierOfferId = v; }
        public Integer getSupplierOfferDetailId() { return supplierOfferDetailId; }
        public void setSupplierOfferDetailId(Integer v) { this.supplierOfferDetailId = v; }
        public Integer getPurchaseOrderMasterId() { return purchaseOrderMasterId; }
        public void setPurchaseOrderMasterId(Integer v) { this.purchaseOrderMasterId = v; }
        public Integer getPurchaseOrderDetailId() { return purchaseOrderDetailId; }
        public void setPurchaseOrderDetailId(Integer v) { this.purchaseOrderDetailId = v; }
        public Integer getInquiryBookingMasterId() { return inquiryBookingMasterId; }
        public void setInquiryBookingMasterId(Integer v) { this.inquiryBookingMasterId = v; }
        public Integer getInquiryBookingDetailId() { return inquiryBookingDetailId; }
        public void setInquiryBookingDetailId(Integer v) { this.inquiryBookingDetailId = v; }
        public Integer getSaleOrderMasterId() { return saleOrderMasterId; }
        public void setSaleOrderMasterId(Integer v) { this.saleOrderMasterId = v; }
        public Integer getSaleOrderDetailId() { return saleOrderDetailId; }
        public void setSaleOrderDetailId(Integer v) { this.saleOrderDetailId = v; }
        public Integer getGrnSupplierLoadingMasterId() { return grnSupplierLoadingMasterId; }
        public void setGrnSupplierLoadingMasterId(Integer v) { this.grnSupplierLoadingMasterId = v; }
        public Integer getGrnSupplierLoadingDetailId() { return grnSupplierLoadingDetailId; }
        public void setGrnSupplierLoadingDetailId(Integer v) { this.grnSupplierLoadingDetailId = v; }
        public Integer getSupplierId() { return supplierId; }
        public void setSupplierId(Integer v) { this.supplierId = v; }
        public Integer getDeliverToAddressId() { return deliverToAddressId; }
        public void setDeliverToAddressId(Integer v) { this.deliverToAddressId = v; }
        public String getDeliverToAddress() { return deliverToAddress; }
        public void setDeliverToAddress(String v) { this.deliverToAddress = v; }
        public Integer getLoadingCityId() { return loadingCityId; }
        public void setLoadingCityId(Integer v) { this.loadingCityId = v; }
        public Integer getUnloadingCityId() { return unloadingCityId; }
        public void setUnloadingCityId(Integer v) { this.unloadingCityId = v; }
        public Integer getInventoryParentCategoryId() { return inventoryParentCategoryId; }
        public void setInventoryParentCategoryId(Integer v) { this.inventoryParentCategoryId = v; }
        public Integer getItemId() { return itemId; }
        public void setItemId(Integer v) { this.itemId = v; }
        public String getItemName() { return itemName; }
        public void setItemName(String v) { this.itemName = v; }
        public Integer getPackUomId() { return packUomId; }
        public void setPackUomId(Integer v) { this.packUomId = v; }
        public Integer getCropYearId() { return cropYearId; }
        public void setCropYearId(Integer v) { this.cropYearId = v; }
        public String getCropYear() { return cropYear; }
        public void setCropYear(String v) { this.cropYear = v; }
        public Integer getPackingTypeId() { return packingTypeId; }
        public void setPackingTypeId(Integer v) { this.packingTypeId = v; }
        public BigDecimal getLoadingQty() { return loadingQty; }
        public void setLoadingQty(BigDecimal v) { this.loadingQty = v; }
        public BigDecimal getWbGrossWeight() { return wbGrossWeight; }
        public void setWbGrossWeight(BigDecimal v) { this.wbGrossWeight = v; }
        public BigDecimal getEbwPerUnit() { return ebwPerUnit; }
        public void setEbwPerUnit(BigDecimal v) { this.ebwPerUnit = v; }
        public BigDecimal getEbwTotal() { return ebwTotal; }
        public void setEbwTotal(BigDecimal v) { this.ebwTotal = v; }
        public BigDecimal getAddLessWeight() { return addLessWeight; }
        public void setAddLessWeight(BigDecimal v) { this.addLessWeight = v; }
        public BigDecimal getNetBillWeight() { return netBillWeight; }
        public void setNetBillWeight(BigDecimal v) { this.netBillWeight = v; }
        public Integer getSortNo() { return sortNo; }
        public void setSortNo(Integer v) { this.sortNo = v; }
        public String getRemarks() { return remarks; }
        public void setRemarks(String v) { this.remarks = v; }
        public Integer getActionTypeId() { return actionTypeId; }
        public void setActionTypeId(Integer v) { this.actionTypeId = v; }
        public Integer getEbWeightDeductionTermId() { return ebWeightDeductionTermId; }
        public void setEbWeightDeductionTermId(Integer v) { this.ebWeightDeductionTermId = v; }
        public Integer getPurchaseOrderSaleOrderMappingId() { return purchaseOrderSaleOrderMappingId; }
        public void setPurchaseOrderSaleOrderMappingId(Integer v) { this.purchaseOrderSaleOrderMappingId = v; }
        public String getWarningRemarks() { return warningRemarks; }
        public void setWarningRemarks(String v) { this.warningRemarks = v; }
        public String getGrnDate() { return grnDate; }
        public void setGrnDate(String v) { this.grnDate = v; }
        public Integer getGrnNo() { return grnNo; }
        public void setGrnNo(Integer v) { this.grnNo = v; }
    }

    public static class EmptyBagDto {
        private Integer gdnBuyerDispatchEmptyBagDetailId;
        private Integer gdnBuyerDispatchMasterId;
        private Integer saleOrderEmptyBagDetailId;
        private Integer saleOrderMasterId;
        @JsonAlias({"PackingTypeId"})
        private Integer packingTypeId;
        private BigDecimal weightCutKg;
        @JsonAlias({"Rate"})
        private BigDecimal rate;
        private Integer sortNo;
        private String remarks;
        private Integer emptyBagPackingMaterialItemId;

        public Integer getGdnBuyerDispatchEmptyBagDetailId() { return gdnBuyerDispatchEmptyBagDetailId; }
        public void setGdnBuyerDispatchEmptyBagDetailId(Integer v) { this.gdnBuyerDispatchEmptyBagDetailId = v; }
        public Integer getGdnBuyerDispatchMasterId() { return gdnBuyerDispatchMasterId; }
        public void setGdnBuyerDispatchMasterId(Integer v) { this.gdnBuyerDispatchMasterId = v; }
        public Integer getSaleOrderEmptyBagDetailId() { return saleOrderEmptyBagDetailId; }
        public void setSaleOrderEmptyBagDetailId(Integer v) { this.saleOrderEmptyBagDetailId = v; }
        public Integer getSaleOrderMasterId() { return saleOrderMasterId; }
        public void setSaleOrderMasterId(Integer v) { this.saleOrderMasterId = v; }
        public Integer getPackingTypeId() { return packingTypeId; }
        public void setPackingTypeId(Integer v) { this.packingTypeId = v; }
        public BigDecimal getWeightCutKg() { return weightCutKg; }
        public void setWeightCutKg(BigDecimal v) { this.weightCutKg = v; }
        public BigDecimal getRate() { return rate; }
        public void setRate(BigDecimal v) { this.rate = v; }
        public Integer getSortNo() { return sortNo; }
        public void setSortNo(Integer v) { this.sortNo = v; }
        public String getRemarks() { return remarks; }
        public void setRemarks(String v) { this.remarks = v; }
        public Integer getEmptyBagPackingMaterialItemId() { return emptyBagPackingMaterialItemId; }
        public void setEmptyBagPackingMaterialItemId(Integer v) { this.emptyBagPackingMaterialItemId = v; }
    }

    public static class ExpenseDto {
        private Integer gdnBuyerDispatchExpenseDetailId;
        private Integer gdnBuyerDispatchMasterId;
        private Integer saleOrderExpenseDetailId;
        private Integer saleOrderMasterId;
        @JsonAlias({"ItemId"})
        private Integer itemId;
        @JsonAlias({"OtherItemName"})
        private String otherItemName;
        @JsonAlias({"Qty"})
        private BigDecimal qty;
        private Double rate;
        private Double amount;
        private Integer sortNo;
        private String remarks;

        public Integer getGdnBuyerDispatchExpenseDetailId() { return gdnBuyerDispatchExpenseDetailId; }
        public void setGdnBuyerDispatchExpenseDetailId(Integer v) { this.gdnBuyerDispatchExpenseDetailId = v; }
        public Integer getGdnBuyerDispatchMasterId() { return gdnBuyerDispatchMasterId; }
        public void setGdnBuyerDispatchMasterId(Integer v) { this.gdnBuyerDispatchMasterId = v; }
        public Integer getSaleOrderExpenseDetailId() { return saleOrderExpenseDetailId; }
        public void setSaleOrderExpenseDetailId(Integer v) { this.saleOrderExpenseDetailId = v; }
        public Integer getSaleOrderMasterId() { return saleOrderMasterId; }
        public void setSaleOrderMasterId(Integer v) { this.saleOrderMasterId = v; }
        public Integer getItemId() { return itemId; }
        public void setItemId(Integer v) { this.itemId = v; }
        public String getOtherItemName() { return otherItemName; }
        public void setOtherItemName(String v) { this.otherItemName = v; }
        public BigDecimal getQty() { return qty; }
        public void setQty(BigDecimal v) { this.qty = v; }
        public Double getRate() { return rate; }
        public void setRate(Double v) { this.rate = v; }
        public Double getAmount() { return amount; }
        public void setAmount(Double v) { this.amount = v; }
        public Integer getSortNo() { return sortNo; }
        public void setSortNo(Integer v) { this.sortNo = v; }
        public String getRemarks() { return remarks; }
        public void setRemarks(String v) { this.remarks = v; }
    }

    public Integer getGdnBuyerDispatchMasterId() { return gdnBuyerDispatchMasterId; }
    public void setGdnBuyerDispatchMasterId(Integer v) { this.gdnBuyerDispatchMasterId = v; }
    public Integer getDocNo() { return docNo; }
    public void setDocNo(Integer v) { this.docNo = v; }
    public String getDocDate() { return docDate; }
    public void setDocDate(String v) { this.docDate = v; }
    public Integer getBuyerId() { return buyerId; }
    public void setBuyerId(Integer v) { this.buyerId = v; }
    public String getBuyerName() { return buyerName; }
    public void setBuyerName(String v) { this.buyerName = v; }
    public Integer getCommissionAgentId() { return commissionAgentId; }
    public void setCommissionAgentId(Integer v) { this.commissionAgentId = v; }
    public String getCommissionAgentName() { return commissionAgentName; }
    public void setCommissionAgentName(String v) { this.commissionAgentName = v; }
    public String getVehicleNo() { return vehicleNo; }
    public void setVehicleNo(String v) { this.vehicleNo = v; }
    public String getBiltyNo() { return biltyNo; }
    public void setBiltyNo(String v) { this.biltyNo = v; }
    public String getRemarksHeader() { return remarksHeader; }
    public void setRemarksHeader(String v) { this.remarksHeader = v; }
    public Integer getOrganizationId() { return organizationId; }
    public void setOrganizationId(Integer v) { this.organizationId = v; }
    public Integer getCompanyId() { return companyId; }
    public void setCompanyId(Integer v) { this.companyId = v; }
    public Integer getBranchId() { return branchId; }
    public void setBranchId(Integer v) { this.branchId = v; }
    public Integer getFinancialYearId() { return financialYearId; }
    public void setFinancialYearId(Integer v) { this.financialYearId = v; }
    public Integer getEntryUserId() { return entryUserId; }
    public void setEntryUserId(Integer v) { this.entryUserId = v; }
    public Integer getModifyUserId() { return modifyUserId; }
    public void setModifyUserId(Integer v) { this.modifyUserId = v; }
    public Integer getDocumentTypeId() { return documentTypeId; }
    public void setDocumentTypeId(Integer v) { this.documentTypeId = v; }
    public String getBuyerRefDocNo() { return buyerRefDocNo; }
    public void setBuyerRefDocNo(String v) { this.buyerRefDocNo = v; }
    public Integer getDeliverToPartyId() { return deliverToPartyId; }
    public void setDeliverToPartyId(Integer v) { this.deliverToPartyId = v; }
    public String getDeliverToPartyName() { return deliverToPartyName; }
    public void setDeliverToPartyName(String v) { this.deliverToPartyName = v; }
    public Integer getDeliverToAddressId() { return deliverToAddressId; }
    public void setDeliverToAddressId(Integer v) { this.deliverToAddressId = v; }
    public String getDeliverToAddress() { return deliverToAddress; }
    public void setDeliverToAddress(String v) { this.deliverToAddress = v; }
    public Integer getLoadingCityId() { return loadingCityId; }
    public void setLoadingCityId(Integer v) { this.loadingCityId = v; }
    public Integer getUnloadingCityId() { return unloadingCityId; }
    public void setUnloadingCityId(Integer v) { this.unloadingCityId = v; }
    public Integer getTransporterId() { return transporterId; }
    public void setTransporterId(Integer v) { this.transporterId = v; }
    public String getTransporterName() { return transporterName; }
    public void setTransporterName(String v) { this.transporterName = v; }
    public BigDecimal getBiltyFreight() { return biltyFreight; }
    public void setBiltyFreight(BigDecimal v) { this.biltyFreight = v; }
    public BigDecimal getOtherAdLesCharges() { return otherAdLesCharges; }
    public void setOtherAdLesCharges(BigDecimal v) { this.otherAdLesCharges = v; }
    public BigDecimal getTotalFreight() { return totalFreight; }
    public void setTotalFreight(BigDecimal v) { this.totalFreight = v; }
    public String getFreightRemarks() { return freightRemarks; }
    public void setFreightRemarks(String v) { this.freightRemarks = v; }
    public Integer getVehicleTypeId() { return vehicleTypeId; }
    public void setVehicleTypeId(Integer v) { this.vehicleTypeId = v; }
    public String getBiltyDate() { return biltyDate; }
    public void setBiltyDate(String v) { this.biltyDate = v; }
    public Integer getDeliveryTermId() { return deliveryTermId; }
    public void setDeliveryTermId(Integer v) { this.deliveryTermId = v; }
    public BigDecimal getBiltyQty() { return biltyQty; }
    public void setBiltyQty(BigDecimal v) { this.biltyQty = v; }
    public BigDecimal getLoadWeight() { return loadWeight; }
    public void setLoadWeight(BigDecimal v) { this.loadWeight = v; }
    public BigDecimal getTareWeight() { return tareWeight; }
    public void setTareWeight(BigDecimal v) { this.tareWeight = v; }
    public BigDecimal getScaleNetWeight() { return scaleNetWeight; }
    public void setScaleNetWeight(BigDecimal v) { this.scaleNetWeight = v; }
    public BigDecimal getBillWeight() { return billWeight; }
    public void setBillWeight(BigDecimal v) { this.billWeight = v; }
    public String getWarningRemarks() { return warningRemarks; }
    public void setWarningRemarks(String v) { this.warningRemarks = v; }
    public String getGrnDate() { return grnDate; }
    public void setGrnDate(String v) { this.grnDate = v; }
    public List<DetailDto> getGdnBuyerDispatchDetailList() { return gdnBuyerDispatchDetailList; }
    public void setGdnBuyerDispatchDetailList(List<DetailDto> v) { this.gdnBuyerDispatchDetailList = v; }
    public List<EmptyBagDto> getGdnBuyerDispatchEmptyBagDetailList() { return gdnBuyerDispatchEmptyBagDetailList; }
    public void setGdnBuyerDispatchEmptyBagDetailList(List<EmptyBagDto> v) { this.gdnBuyerDispatchEmptyBagDetailList = v; }
    public List<ExpenseDto> getGdnBuyerDispatchExpenseDetailList() { return gdnBuyerDispatchExpenseDetailList; }
    public void setGdnBuyerDispatchExpenseDetailList(List<ExpenseDto> v) { this.gdnBuyerDispatchExpenseDetailList = v; }
}
