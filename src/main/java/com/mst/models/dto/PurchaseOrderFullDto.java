package com.mst.models.dto;

import lombok.Data;

import java.util.ArrayList;
import java.util.Date;
import java.util.List;

@Data
public class PurchaseOrderFullDto {
    private Integer purchaseOrderMasterId = 0;
    private Integer documentTypeId = 1052;
    private Integer docNo;
    private Integer branchNo;
    private String docDate;
    private Integer supplierId;
    private String supplierName;
    private String deliveryStartDate;
    private Integer deliveryDays = 7;
    private String expiryDate;
    private Integer paymentTermId;
    private Integer dueDays = 0;
    private String paymentDueDate;
    private Integer deliveryTermId;
    private Integer bookingPersonId;

    // Commission & Brokery
    private Integer commissionAgentId;
    private String commissionAgentName;
    private Integer commissionTypeId;
    private String commissionTypeName;
    private Double commRate = 0.0;
    private Integer commUomId;
    private Double commAmount = 0.0;
    private Integer commissionAccountId;

    private Integer brokerAccountId;
    private Integer brokeryTypeId;
    private String brokeryTypeName;
    private Double brokeryRate = 0.0;
    private Integer brokeryRateUomId;
    private Double brokeryAmount = 0.0;

    private Double juteBagCut = 0.0;
    private Double ppBagCut = 0.0;
    private Double cashFreight = 0.0;
    private Double creditFreight = 0.0;

    private Boolean isSupplierOtherChargesAllowed = false;
    private Boolean isWhtApplied = false;

    private Integer deliveryToPartyId;
    private Integer shipToAddressId;
    private String shipToAddress;

    private String remarksHeader;
    private String paymentScheduleDescription;

    private List<PurchaseOrderDetailItemDto> lineItems = new ArrayList<>();
    private List<PurchaseOrderEmptyBagDto> emptyBags = new ArrayList<>();
    private List<PurchaseOrderSupplierExpenseDto> supplierExpenses = new ArrayList<>();
    private List<PurchaseOrderExpensesChargeToProductDto> expensesChargeToProduct = new ArrayList<>();
    private List<PurchaseOrderPaymentTermsDetailDto> paymentTermsDetail = new ArrayList<>();

    public Integer getPurchaseOrderMasterId() { return purchaseOrderMasterId; }
    public void setPurchaseOrderMasterId(Integer purchaseOrderMasterId) { this.purchaseOrderMasterId = purchaseOrderMasterId; }
    public Integer getDocumentTypeId() { return documentTypeId; }
    public void setDocumentTypeId(Integer documentTypeId) { this.documentTypeId = documentTypeId; }
    public Integer getDocNo() { return docNo; }
    public void setDocNo(Integer docNo) { this.docNo = docNo; }
    public Integer getBranchNo() { return branchNo; }
    public void setBranchNo(Integer branchNo) { this.branchNo = branchNo; }
    public String getDocDate() { return docDate; }
    public void setDocDate(String docDate) { this.docDate = docDate; }
    public Integer getSupplierId() { return supplierId; }
    public void setSupplierId(Integer supplierId) { this.supplierId = supplierId; }
    public String getSupplierName() { return supplierName; }
    public void setSupplierName(String supplierName) { this.supplierName = supplierName; }
    public String getDeliveryStartDate() { return deliveryStartDate; }
    public void setDeliveryStartDate(String deliveryStartDate) { this.deliveryStartDate = deliveryStartDate; }
    public Integer getDeliveryDays() { return deliveryDays; }
    public void setDeliveryDays(Integer deliveryDays) { this.deliveryDays = deliveryDays; }
    public String getExpiryDate() { return expiryDate; }
    public void setExpiryDate(String expiryDate) { this.expiryDate = expiryDate; }
    public Integer getPaymentTermId() { return paymentTermId; }
    public void setPaymentTermId(Integer paymentTermId) { this.paymentTermId = paymentTermId; }
    public Integer getDueDays() { return dueDays; }
    public void setDueDays(Integer dueDays) { this.dueDays = dueDays; }
    public String getPaymentDueDate() { return paymentDueDate; }
    public void setPaymentDueDate(String paymentDueDate) { this.paymentDueDate = paymentDueDate; }
    public Integer getDeliveryTermId() { return deliveryTermId; }
    public void setDeliveryTermId(Integer deliveryTermId) { this.deliveryTermId = deliveryTermId; }
    public Integer getBookingPersonId() { return bookingPersonId; }
    public void setBookingPersonId(Integer bookingPersonId) { this.bookingPersonId = bookingPersonId; }

    public Integer getCommissionAgentId() { return commissionAgentId; }
    public void setCommissionAgentId(Integer commissionAgentId) { this.commissionAgentId = commissionAgentId; }
    public String getCommissionAgentName() { return commissionAgentName; }
    public void setCommissionAgentName(String commissionAgentName) { this.commissionAgentName = commissionAgentName; }
    public Integer getCommissionTypeId() { return commissionTypeId; }
    public void setCommissionTypeId(Integer commissionTypeId) { this.commissionTypeId = commissionTypeId; }
    public String getCommissionTypeName() { return commissionTypeName; }
    public void setCommissionTypeName(String commissionTypeName) { this.commissionTypeName = commissionTypeName; }
    public Double getCommRate() { return commRate; }
    public void setCommRate(Double commRate) { this.commRate = commRate; }
    public Integer getCommUomId() { return commUomId; }
    public void setCommUomId(Integer commUomId) { this.commUomId = commUomId; }
    public Double getCommAmount() { return commAmount; }
    public void setCommAmount(Double commAmount) { this.commAmount = commAmount; }
    public Integer getCommissionAccountId() { return commissionAccountId; }
    public void setCommissionAccountId(Integer commissionAccountId) { this.commissionAccountId = commissionAccountId; }

    public Integer getBrokerAccountId() { return brokerAccountId; }
    public void setBrokerAccountId(Integer brokerAccountId) { this.brokerAccountId = brokerAccountId; }
    public Integer getBrokeryTypeId() { return brokeryTypeId; }
    public void setBrokeryTypeId(Integer brokeryTypeId) { this.brokeryTypeId = brokeryTypeId; }
    public String getBrokeryTypeName() { return brokeryTypeName; }
    public void setBrokeryTypeName(String brokeryTypeName) { this.brokeryTypeName = brokeryTypeName; }
    public Double getBrokeryRate() { return brokeryRate; }
    public void setBrokeryRate(Double brokeryRate) { this.brokeryRate = brokeryRate; }
    public Integer getBrokeryRateUomId() { return brokeryRateUomId; }
    public void setBrokeryRateUomId(Integer brokeryRateUomId) { this.brokeryRateUomId = brokeryRateUomId; }
    public Double getBrokeryAmount() { return brokeryAmount; }
    public void setBrokeryAmount(Double brokeryAmount) { this.brokeryAmount = brokeryAmount; }

    public Double getJuteBagCut() { return juteBagCut; }
    public void setJuteBagCut(Double juteBagCut) { this.juteBagCut = juteBagCut; }
    public Double getPpBagCut() { return ppBagCut; }
    public void setPpBagCut(Double ppBagCut) { this.ppBagCut = ppBagCut; }
    public Double getCashFreight() { return cashFreight; }
    public void setCashFreight(Double cashFreight) { this.cashFreight = cashFreight; }
    public Double getCreditFreight() { return creditFreight; }
    public void setCreditFreight(Double creditFreight) { this.creditFreight = creditFreight; }

    public Boolean getIsSupplierOtherChargesAllowed() { return isSupplierOtherChargesAllowed; }
    public void setIsSupplierOtherChargesAllowed(Boolean isSupplierOtherChargesAllowed) { this.isSupplierOtherChargesAllowed = isSupplierOtherChargesAllowed; }
    public Boolean getIsWhtApplied() { return isWhtApplied; }
    public void setIsWhtApplied(Boolean isWhtApplied) { this.isWhtApplied = isWhtApplied; }

    public Integer getDeliveryToPartyId() { return deliveryToPartyId; }
    public void setDeliveryToPartyId(Integer deliveryToPartyId) { this.deliveryToPartyId = deliveryToPartyId; }
    public Integer getShipToAddressId() { return shipToAddressId; }
    public void setShipToAddressId(Integer shipToAddressId) { this.shipToAddressId = shipToAddressId; }
    public String getShipToAddress() { return shipToAddress; }
    public void setShipToAddress(String shipToAddress) { this.shipToAddress = shipToAddress; }

    public String getRemarksHeader() { return remarksHeader; }
    public void setRemarksHeader(String remarksHeader) { this.remarksHeader = remarksHeader; }
    public String getPaymentScheduleDescription() { return paymentScheduleDescription; }
    public void setPaymentScheduleDescription(String paymentScheduleDescription) { this.paymentScheduleDescription = paymentScheduleDescription; }

    public List<PurchaseOrderDetailItemDto> getLineItems() { return lineItems; }
    public void setLineItems(List<PurchaseOrderDetailItemDto> lineItems) { this.lineItems = lineItems; }
    public List<PurchaseOrderEmptyBagDto> getEmptyBags() { return emptyBags; }
    public void setEmptyBags(List<PurchaseOrderEmptyBagDto> emptyBags) { this.emptyBags = emptyBags; }
    public List<PurchaseOrderSupplierExpenseDto> getSupplierExpenses() { return supplierExpenses; }
    public void setSupplierExpenses(List<PurchaseOrderSupplierExpenseDto> supplierExpenses) { this.supplierExpenses = supplierExpenses; }
    public List<PurchaseOrderExpensesChargeToProductDto> getExpensesChargeToProduct() { return expensesChargeToProduct; }
    public void setExpensesChargeToProduct(List<PurchaseOrderExpensesChargeToProductDto> expensesChargeToProduct) { this.expensesChargeToProduct = expensesChargeToProduct; }
    public List<PurchaseOrderPaymentTermsDetailDto> getPaymentTermsDetail() { return paymentTermsDetail; }
    public void setPaymentTermsDetail(List<PurchaseOrderPaymentTermsDetailDto> paymentTermsDetail) { this.paymentTermsDetail = paymentTermsDetail; }

    @Data
    public static class PurchaseOrderDetailItemDto {
        private Integer purchaseOrderDetailId = 0;
        private Integer inventoryParentCategoryId;
        private String inventoryParentCategory;
        private Integer itemId;
        private String itemCode;
        private String itemName;
        private Integer cropYearId;
        private String cropYear;
        private Integer packUomId;
        private String packUomCode;
        private Double packUomEquivalent = 1.0;
        private Double itemQty = 0.0;
        private Double itemWeight = 0.0;
        private Double itemRate = 0.0;
        private Integer rateUomId;
        private String rateUomCode;
        private Double rateUomEquivalent = 1.0;
        private Double itemAmount = 0.0;
        private Integer taxNameId;
        private String taxName;
        private Double taxPercent = 0.0;
        private Double taxAmount = 0.0;
        private Double totalAmount = 0.0;
        private Integer jobLotId;
        private String jobLotName;
        private Integer loadingLocationCityId;
        private String loadingLocationCityName;
        private Double moisturePercent = 0.0;
        private String factoryType = "Standard"; // Sample or Standard
        private String remarks;

        public Integer getPurchaseOrderDetailId() { return purchaseOrderDetailId; }
        public void setPurchaseOrderDetailId(Integer purchaseOrderDetailId) { this.purchaseOrderDetailId = purchaseOrderDetailId; }
        public Integer getInventoryParentCategoryId() { return inventoryParentCategoryId; }
        public void setInventoryParentCategoryId(Integer inventoryParentCategoryId) { this.inventoryParentCategoryId = inventoryParentCategoryId; }
        public String getInventoryParentCategory() { return inventoryParentCategory; }
        public void setInventoryParentCategory(String inventoryParentCategory) { this.inventoryParentCategory = inventoryParentCategory; }
        public Integer getItemId() { return itemId; }
        public void setItemId(Integer itemId) { this.itemId = itemId; }
        public String getItemCode() { return itemCode; }
        public void setItemCode(String itemCode) { this.itemCode = itemCode; }
        public String getItemName() { return itemName; }
        public void setItemName(String itemName) { this.itemName = itemName; }
        public Integer getCropYearId() { return cropYearId; }
        public void setCropYearId(Integer cropYearId) { this.cropYearId = cropYearId; }
        public String getCropYear() { return cropYear; }
        public void setCropYear(String cropYear) { this.cropYear = cropYear; }
        public Integer getPackUomId() { return packUomId; }
        public void setPackUomId(Integer packUomId) { this.packUomId = packUomId; }
        public String getPackUomCode() { return packUomCode; }
        public void setPackUomCode(String packUomCode) { this.packUomCode = packUomCode; }
        public Double getPackUomEquivalent() { return packUomEquivalent; }
        public void setPackUomEquivalent(Double packUomEquivalent) { this.packUomEquivalent = packUomEquivalent; }
        public Double getItemQty() { return itemQty; }
        public void setItemQty(Double itemQty) { this.itemQty = itemQty; }
        public Double getItemWeight() { return itemWeight; }
        public void setItemWeight(Double itemWeight) { this.itemWeight = itemWeight; }
        public Double getItemRate() { return itemRate; }
        public void setItemRate(Double itemRate) { this.itemRate = itemRate; }
        public Integer getRateUomId() { return rateUomId; }
        public void setRateUomId(Integer rateUomId) { this.rateUomId = rateUomId; }
        public String getRateUomCode() { return rateUomCode; }
        public void setRateUomCode(String rateUomCode) { this.rateUomCode = rateUomCode; }
        public Double getRateUomEquivalent() { return rateUomEquivalent; }
        public void setRateUomEquivalent(Double rateUomEquivalent) { this.rateUomEquivalent = rateUomEquivalent; }
        public Double getItemAmount() { return itemAmount; }
        public void setItemAmount(Double itemAmount) { this.itemAmount = itemAmount; }
        public Integer getTaxNameId() { return taxNameId; }
        public void setTaxNameId(Integer taxNameId) { this.taxNameId = taxNameId; }
        public String getTaxName() { return taxName; }
        public void setTaxName(String taxName) { this.taxName = taxName; }
        public Double getTaxPercent() { return taxPercent; }
        public void setTaxPercent(Double taxPercent) { this.taxPercent = taxPercent; }
        public Double getTaxAmount() { return taxAmount; }
        public void setTaxAmount(Double taxAmount) { this.taxAmount = taxAmount; }
        public Double getTotalAmount() { return totalAmount; }
        public void setTotalAmount(Double totalAmount) { this.totalAmount = totalAmount; }
        public Integer getJobLotId() { return jobLotId; }
        public void setJobLotId(Integer jobLotId) { this.jobLotId = jobLotId; }
        public String getJobLotName() { return jobLotName; }
        public void setJobLotName(String jobLotName) { this.jobLotName = jobLotName; }
        public Integer getLoadingLocationCityId() { return loadingLocationCityId; }
        public void setLoadingLocationCityId(Integer loadingLocationCityId) { this.loadingLocationCityId = loadingLocationCityId; }
        public String getLoadingLocationCityName() { return loadingLocationCityName; }
        public void setLoadingLocationCityName(String loadingLocationCityName) { this.loadingLocationCityName = loadingLocationCityName; }
        public Double getMoisturePercent() { return moisturePercent; }
        public void setMoisturePercent(Double moisturePercent) { this.moisturePercent = moisturePercent; }
        public String getFactoryType() { return factoryType; }
        public void setFactoryType(String factoryType) { this.factoryType = factoryType; }
        public String getRemarks() { return remarks; }
        public void setRemarks(String remarks) { this.remarks = remarks; }
    }

    /**
     * Ditto of the real desktop model Architecture.Model.Inventory.PurchaseorderEmptyBags
     * (recovered_source/projects/architecture.model/0990_Architecture.Model.Inventory.PurchaseorderEmptyBags.cs)
     * and the real table [dbo].[PurchaseOrderEmptyBags] (Id, PurchaseOrderId, Type, ItemId,
     * PackingTypeId, Rate, WeightCut). NOTE: the real schema has NO Qty / Amount column at all -
     * the desktop's "Packing Material (Empty Bags)" grid never had a quantity or a computed total;
     * it only ever had Type, Item, PackingType, Rate and WeightCut. A previous Java implementation
     * fabricated Qty/Rate/Amount fields that do not exist on the desktop or in the database - this
     * DTO replaces that fabricated shape with the real one.
     */
    @Data
    public static class PurchaseOrderEmptyBagDto {
        private Integer id = 0;
        private Integer purchaseOrderId = 0;
        /** vEmptyBagTypes.Id: 1=Normal (Purchase/Receive), 2=Purchase Against Weight, 3=Free of Cost, 4=Retained, 5=Returned. */
        private Integer type;
        private String typeName;
        private Integer itemId;
        private String itemName;
        private Integer packingTypeId;
        private String packingTypeName;
        private Double rate = 0.0;
        private Double weightCut = 0.0;

        public Integer getId() { return id; }
        public void setId(Integer id) { this.id = id; }
        public Integer getPurchaseOrderId() { return purchaseOrderId; }
        public void setPurchaseOrderId(Integer purchaseOrderId) { this.purchaseOrderId = purchaseOrderId; }
        public Integer getType() { return type; }
        public void setType(Integer type) { this.type = type; }
        public String getTypeName() { return typeName; }
        public void setTypeName(String typeName) { this.typeName = typeName; }
        public Integer getItemId() { return itemId; }
        public void setItemId(Integer itemId) { this.itemId = itemId; }
        public String getItemName() { return itemName; }
        public void setItemName(String itemName) { this.itemName = itemName; }
        public Integer getPackingTypeId() { return packingTypeId; }
        public void setPackingTypeId(Integer packingTypeId) { this.packingTypeId = packingTypeId; }
        public String getPackingTypeName() { return packingTypeName; }
        public void setPackingTypeName(String packingTypeName) { this.packingTypeName = packingTypeName; }
        public Double getRate() { return rate; }
        public void setRate(Double rate) { this.rate = rate; }
        public Double getWeightCut() { return weightCut; }
        public void setWeightCut(Double weightCut) { this.weightCut = weightCut; }
    }

    /**
     * Ditto of Architecture.Model.Inventory.PurchaseOrderSupplierExpense (used by PurchsaeOrder.cs's
     * "Supplier Expense (Credit To Supplier & Debit To Product)" tab, grdInvExp/dtInvExp) and the real
     * table [dbo].[PurchaseOrderSupplierExpense] (Id, PurchaseOrderId, InvRevExpItemId, Qty, Rate,
     * Amount, Remarks). One row per real "Other Item" master row (InventoryItemsOther - e.g. BARDANA,
     * OTHER CHARGES, MUNSHIANA, SOTRI), seeded automatically when a Purchase Order is opened; only rows
     * with ItemId != 0 and Amount > 0 are actually persisted (ditto desktop's Save/Update filter).
     */
    @Data
    public static class PurchaseOrderSupplierExpenseDto {
        private Integer id = 0;
        private Integer purchaseOrderId = 0;
        private Integer invRevExpItemId;
        private String otherItemName;
        private Double qty = 0.0;
        private Double rate = 0.0;
        private Double amount = 0.0;
        private String remarks;

        public Integer getId() { return id; }
        public void setId(Integer id) { this.id = id; }
        public Integer getPurchaseOrderId() { return purchaseOrderId; }
        public void setPurchaseOrderId(Integer purchaseOrderId) { this.purchaseOrderId = purchaseOrderId; }
        public Integer getInvRevExpItemId() { return invRevExpItemId; }
        public void setInvRevExpItemId(Integer invRevExpItemId) { this.invRevExpItemId = invRevExpItemId; }
        public String getOtherItemName() { return otherItemName; }
        public void setOtherItemName(String otherItemName) { this.otherItemName = otherItemName; }
        public Double getQty() { return qty; }
        public void setQty(Double qty) { this.qty = qty; }
        public Double getRate() { return rate; }
        public void setRate(Double rate) { this.rate = rate; }
        public Double getAmount() { return amount; }
        public void setAmount(Double amount) { this.amount = amount; }
        public String getRemarks() { return remarks; }
        public void setRemarks(String remarks) { this.remarks = remarks; }
    }

    /**
     * Ditto of Architecture.Model.Inventory.PurchaseOrderExpensesChargeToProduct (PurchsaeOrder.cs's
     * "Account Credit _Charge to Product" tab, grdExpensesChargeToProduct/dtChargeToProduct) and the
     * real table [dbo].[PurchaseOrderExpensesChargeToProduct] (Id, PurchaseOrderId, AccountId,
     * Percentage, Qty, Rate, Amount, Remarks, SupplierCustomerId). AccountId is a real Chart of
     * Account (4th-level) Id from Sp_COAAllocation_GetAllMethod. A row is only persisted when
     * Amount > 0 (AccountTitle then becomes required), ditto the desktop's Save/Update validation.
     */
    @Data
    public static class PurchaseOrderExpensesChargeToProductDto {
        private Integer id = 0;
        private Integer purchaseOrderId = 0;
        private Integer accountId;
        private String accountTitle;
        private Integer supplierCustomerId = 0;
        private String supplierCustomerName;
        private Double percentage = 0.0;
        private Double qty = 0.0;
        private Double rate = 0.0;
        private Double amount = 0.0;
        private String remarks;

        public Integer getId() { return id; }
        public void setId(Integer id) { this.id = id; }
        public Integer getPurchaseOrderId() { return purchaseOrderId; }
        public void setPurchaseOrderId(Integer purchaseOrderId) { this.purchaseOrderId = purchaseOrderId; }
        public Integer getAccountId() { return accountId; }
        public void setAccountId(Integer accountId) { this.accountId = accountId; }
        public String getAccountTitle() { return accountTitle; }
        public void setAccountTitle(String accountTitle) { this.accountTitle = accountTitle; }
        public Integer getSupplierCustomerId() { return supplierCustomerId; }
        public void setSupplierCustomerId(Integer supplierCustomerId) { this.supplierCustomerId = supplierCustomerId; }
        public String getSupplierCustomerName() { return supplierCustomerName; }
        public void setSupplierCustomerName(String supplierCustomerName) { this.supplierCustomerName = supplierCustomerName; }
        public Double getPercentage() { return percentage; }
        public void setPercentage(Double percentage) { this.percentage = percentage; }
        public Double getQty() { return qty; }
        public void setQty(Double qty) { this.qty = qty; }
        public Double getRate() { return rate; }
        public void setRate(Double rate) { this.rate = rate; }
        public Double getAmount() { return amount; }
        public void setAmount(Double amount) { this.amount = amount; }
        public String getRemarks() { return remarks; }
        public void setRemarks(String remarks) { this.remarks = remarks; }
    }

    /**
     * Ditto of Architecture.Model.Inventory.PurchaseOrderPaymentTermsDetail (PurchsaeOrder.cs's
     * "Payment Detail" tab, grdPaymentDetail/dtPaymentTerm) and the real table
     * [dbo].[PurchaseOrderPaymentTermsDetail] (Id, PurchaseOrderId, PaymentTermId, PrcntOfTotal,
     * Amount, DueDays, PaymentRemarks, SortNo, DueDate). PaymentTermId comes from the real InvDueTerms
     * master (Id=1 'Cash', Id=2 'Credit'; DueDays is required when PaymentTermId=2). PrcntOfTotal and
     * Amount are two-way calculated against the Purchase Order Detail grid's total Amount, and DueDays/
     * DueDate are two-way calculated against DocDate - ditto grdPaymentDetail_CellUpdated().
     */
    @Data
    public static class PurchaseOrderPaymentTermsDetailDto {
        private Integer id = 0;
        private Integer purchaseOrderId = 0;
        private Integer paymentTermId;
        private String paymentTerm;
        private Integer dueDays = 0;
        private String dueDate;
        private Double prcntOfTotal = 0.0;
        private Double amount = 0.0;
        private Integer sortNo = 0;
        private String paymentRemarks;

        public Integer getId() { return id; }
        public void setId(Integer id) { this.id = id; }
        public Integer getPurchaseOrderId() { return purchaseOrderId; }
        public void setPurchaseOrderId(Integer purchaseOrderId) { this.purchaseOrderId = purchaseOrderId; }
        public Integer getPaymentTermId() { return paymentTermId; }
        public void setPaymentTermId(Integer paymentTermId) { this.paymentTermId = paymentTermId; }
        public String getPaymentTerm() { return paymentTerm; }
        public void setPaymentTerm(String paymentTerm) { this.paymentTerm = paymentTerm; }
        public Integer getDueDays() { return dueDays; }
        public void setDueDays(Integer dueDays) { this.dueDays = dueDays; }
        public String getDueDate() { return dueDate; }
        public void setDueDate(String dueDate) { this.dueDate = dueDate; }
        public Double getPrcntOfTotal() { return prcntOfTotal; }
        public void setPrcntOfTotal(Double prcntOfTotal) { this.prcntOfTotal = prcntOfTotal; }
        public Double getAmount() { return amount; }
        public void setAmount(Double amount) { this.amount = amount; }
        public Integer getSortNo() { return sortNo; }
        public void setSortNo(Integer sortNo) { this.sortNo = sortNo; }
        public String getPaymentRemarks() { return paymentRemarks; }
        public void setPaymentRemarks(String paymentRemarks) { this.paymentRemarks = paymentRemarks; }
    }
}
