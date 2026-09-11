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
    }
}
