package com.mst.models.cmagt.dto;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import com.fasterxml.jackson.annotation.JsonProperty;

import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;
import java.math.BigDecimal;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Commission Agent Trade Bill Against GDN (DocumentTypeId 1056).
 *
 * <p>Two halves.</p>
 *
 * <p><b>The request body</b> (the top-level fields of this class) is the web equivalent of the
 * desktop form's controls and grids: {@code header} carries the control values
 * frmCommissionAgentTradeBillAgainstGdn.Insert() reads (4928-5684), and each grid is a list of
 * rows keyed by the desktop's own DataTable column names (TradeBill_Helper.Initialize*Table),
 * e.g. {@code "PurchaseTax%"}, {@code "%OfTotal"}. The server rebuilds the BLL object from them
 * exactly as Insert() does, so every validation and derived value is computed server-side.</p>
 *
 * <p><b>The nested model classes</b> are generated field-for-field from the desktop models
 * (Architecture.Model.Inventory 0961, 0967, 0950, 0952, 0951, 1077, 0904, 0905, 0903, 0902).
 * GenericProvider.SetProc sends every NON-virtual property as @Name; fields marked
 * {@link NotParam} are the model's virtual properties and are never sent. Each non-virtual list
 * was checked against the procedure's declared parameters (every one exists; the procedure's
 * extra parameters - @RowVersion on the master, @SupplierOfferId/@SupplierOfferDetailId/
 * @InquiryBookingMasterId/@InquiryBookingDetailId on the detail - have defaults and the desktop
 * never sends them either). Numeric fields start at the CLR default 0, never null; a null
 * String / nullable date is OMITTED, which is what SqlClient does with a null AddWithValue.</p>
 */
@JsonIgnoreProperties(ignoreUnknown = true)
public class TradeBillAgainstGdnCmagtDto {

    /** Marks a desktop model property declared {@code virtual}: read-only, never a parameter. */
    @Retention(RetentionPolicy.RUNTIME)
    @Target(ElementType.FIELD)
    public @interface NotParam { }

    // ============================================================ request body (form state)

    /** Control values - keys are listed in TradeBillAgainstGdnCmagtService.buildBill. */
    @JsonProperty("header") public Map<String, Object> header = new LinkedHashMap<>();
    /** grd (dtDetail). */
    @JsonProperty("rows") public List<Map<String, Object>> rows = new ArrayList<>();
    /** lstRemoveRecord - grid rows with Id > 0 deleted from grd (DeleteDetailGridRow 2482). */
    @JsonProperty("removedRows") public List<Map<String, Object>> removedRows = new ArrayList<>();
    @JsonProperty("purchaseOtherExpenses") public List<Map<String, Object>> purchaseOtherExpenses = new ArrayList<>();
    @JsonProperty("saleOtherExpenses") public List<Map<String, Object>> saleOtherExpenses = new ArrayList<>();
    /** grdPurchasePmExpense (dtPurchasePmExpense - InitializeFreightDetailTable). */
    @JsonProperty("purchasePmExpenses") public List<Map<String, Object>> purchasePmExpenses = new ArrayList<>();
    /** grdSalePmExpenseSale (dtSalePmExpense - InitializeExpensePackingMaterialTable). */
    @JsonProperty("salePmExpenses") public List<Map<String, Object>> salePmExpenses = new ArrayList<>();
    @JsonProperty("purchaseCommission") public List<Map<String, Object>> purchaseCommission = new ArrayList<>();
    @JsonProperty("saleCommission") public List<Map<String, Object>> saleCommission = new ArrayList<>();
    @JsonProperty("purchaseBrokery") public List<Map<String, Object>> purchaseBrokery = new ArrayList<>();
    @JsonProperty("saleBrokery") public List<Map<String, Object>> saleBrokery = new ArrayList<>();
    @JsonProperty("purchasePayment") public List<Map<String, Object>> purchasePayment = new ArrayList<>();
    @JsonProperty("salePayment") public List<Map<String, Object>> salePayment = new ArrayList<>();


    /** Generated from 0961_Architecture.Model.Inventory.InvCommAgentTradeBill.cs. */
    public static class Bill {
        public Boolean IsApproved = Boolean.FALSE;
        public Timestamp ApprovedDate;
        public Timestamp DocDate;
        public Timestamp DueDate;
        public Timestamp SaleDueDate;
        public Timestamp EnteryDate;
        public Timestamp ModifyDate;
        public Double PurchaseBillAmount = 0d;
        public Double SaleBillAmount = 0d;
        public Double PLAmount = 0d;
        public Double BrokeryRate = 0d;
        public Double CommRate = 0d;
        public Double CommAmount = 0d;
        public Double BrokeryAmount = 0d;
        public Integer ApprovalUserId = 0;
        public Integer BranchesId = 0;
        public Integer BrokerId = 0;
        public Integer CommissionAgentId = 0;
        public Integer CompanyId = 0;
        public Integer CustomerId = 0;
        public Integer TradingGlAccountId = 0;
        public Integer DeliveryTermPurchaseId = 0;
        public Integer DeliveryTermSaleId = 0;
        public String DeliveryTerm;
        public String DeliveryTermSale;
        public Integer DocNo = 0;
        public Integer BranchSrNo = 0;
        public Integer DueDays = 0;
        public Integer SaleDueDays = 0;
        public Integer EnteryUserId = 0;
        public Integer Id = 0;
        public Integer InvCommAgentOrderId = 0;
        public Integer ModifyUserId = 0;
        public Integer OrganizationId = 0;
        public Integer PaymentTermId = 0;
        public Integer SalePaymentTermId = 0;
        public Integer ProjectsId = 0;
        public Integer SupplierId = 0;
        public Integer FinancialYearId = 0;
        public String RemarksHeader;
        public Integer DocumentTypeId = 0;
        public Integer CommissionDebitGLId = 0;
        public Integer BrokeryDebitGLId = 0;
        public Integer CustCommissionAgentId = 0;
        public Double CustCommRate = 0d;
        public Double CustCommAmount = 0d;
        public Integer CustCommissionDebitGLId = 0;
        public Integer CustBrokerId = 0;
        public Double CustBrokeryRate = 0d;
        public Double CustBrokeryAmount = 0d;
        public Integer CustBrokeryDebitGLId = 0;
        public String ScreenName;
        public Integer ActionId = 0;
        public BigDecimal BuyerFirstWeight = BigDecimal.ZERO;
        public BigDecimal BuyerNetWeight = BigDecimal.ZERO;
        public BigDecimal BuyerSecondWeight = BigDecimal.ZERO;
        public BigDecimal SupplierFirstWeight = BigDecimal.ZERO;
        public BigDecimal SupplierNetWeight = BigDecimal.ZERO;
        public BigDecimal SupplierSecondWeight = BigDecimal.ZERO;
        public BigDecimal SupplierBillWeight = BigDecimal.ZERO;
        public BigDecimal BuyerBillWeight = BigDecimal.ZERO;
        public Integer PurchaseTaxAccountId = 0;
        public Integer SaleTaxAccountId = 0;
        public String PurchaseExpenseDetailDescription;
        public String SaleExpenseDetailDescription;
        public String PurchaseFreightExpenseDetailDescription;
        public String SaleExpenseCreditToReleventAccountDetailDescription;
        public String PurchaseCommisionDetailDescription;
        public String PurchaseBrokeryDetailDescription;
        public String SaleCommisionDetailDescription;
        public String SaleBrokeryDetailDescription;
        public String PurchasePaymentDetailDescription;
        public String SalePaymentDetailDescription;
        @NotParam public String SupplierName;
        @NotParam public String CustomerName;
        @NotParam public String PurchaseTaxAccount;
        @NotParam public String SaleTaxAccount;
        @NotParam public String TradingGlAccount;
        @NotParam public List<Detail> InvCommAgentTradeBillDetailslist = new ArrayList<>();
        @NotParam public List<PurchaseExpense> InvCommAgentTradePurchaseExpList = new ArrayList<>();
        @NotParam public List<SaleExpense> InvCommAgentTradeSaleExpList = new ArrayList<>();
        @NotParam public List<SaleExpenseCreditToReleventAc> CommisionAgentBillSaleExpenseCreditToReleventAcsList = new ArrayList<>();
        @NotParam public List<PurchaseFreightExpense> InvCommAgentTradeFreightExpList = new ArrayList<>();
        @NotParam public List<PaymentDetail> CommisionAgentBillPaymentDetailList = new ArrayList<>();
        @NotParam public List<CommissionDetail> CommisionAgentBillCommissionDetailList = new ArrayList<>();
        @NotParam public List<FreightDetail> CommisionAgentBillFreightDetailList = new ArrayList<>();
        @NotParam public List<TaxDetail> CommisionAgentBillTaxDetailList = new ArrayList<>();
    }

    /** Generated from 0967_Architecture.Model.Inventory.InvCommAgentTradeBillDetail.cs. */
    public static class Detail {
        public BigDecimal BillWeight = BigDecimal.ZERO;
        public BigDecimal AddLessSale = BigDecimal.ZERO;
        public BigDecimal SaleBillWeight = BigDecimal.ZERO;
        public BigDecimal GrossWeight = BigDecimal.ZERO;
        public BigDecimal SaleGrossWeight = BigDecimal.ZERO;
        public BigDecimal Qty = BigDecimal.ZERO;
        public BigDecimal RatePurchase = BigDecimal.ZERO;
        public BigDecimal PurchaseRateWithoutAddLess = BigDecimal.ZERO;
        public BigDecimal PurchaseRateAddLess = BigDecimal.ZERO;
        public BigDecimal RateSale = BigDecimal.ZERO;
        public BigDecimal SaleRateAddLess = BigDecimal.ZERO;
        public BigDecimal SaleRateWithoutAddLess = BigDecimal.ZERO;
        public BigDecimal PurchaseFreight = BigDecimal.ZERO;
        public BigDecimal PurchaseCommission = BigDecimal.ZERO;
        public BigDecimal PurchaseBrokery = BigDecimal.ZERO;
        public BigDecimal SaleCommission = BigDecimal.ZERO;
        public BigDecimal SaleBrokery = BigDecimal.ZERO;
        public Double PurchaseFreightLess = 0d;
        public Double ItemAmount = 0d;
        public Double EbCut = 0d;
        public Double EbCutTotal = 0d;
        public Double PurchaseExpenses = 0d;
        public Double SaleExpenses = 0d;
        public Double PurchaseNetAmount = 0d;
        public Double SaleNetAmount = 0d;
        public Double SaleEbCut = 0d;
        public Double SaleEbCutTotal = 0d;
        public Integer ActionTypeId = 0;
        public Integer Crop = 0;
        public Integer Id = 0;
        public Integer InvCommAgentTradeBillId = 0;
        public Integer ItemId = 0;
        public Integer PackingTypeId = 0;
        public Integer PackUomId = 0;
        public Integer RateUomId = 0;
        public String RefDocNo;
        public String RemarksDetail;
        public String VehicleNo;
        public Double SaleAmount = 0d;
        public BigDecimal AddLessPurchase = BigDecimal.ZERO;
        public BigDecimal AmountAfterTaxPurchase = BigDecimal.ZERO;
        public BigDecimal AmountAfterTaxSale = BigDecimal.ZERO;
        public BigDecimal FcyAmountPurchase = BigDecimal.ZERO;
        public BigDecimal FcyAmountSale = BigDecimal.ZERO;
        public BigDecimal TaxAmountPurchase = BigDecimal.ZERO;
        public BigDecimal TaxAmountSale = BigDecimal.ZERO;
        public BigDecimal TaxPercentPurchase = BigDecimal.ZERO;
        public BigDecimal TaxPercentSale = BigDecimal.ZERO;
        public Integer SaleRateUomId = 0;
        public Integer TaxNameIdPurchase = 0;
        public Integer TaxNameIdSale = 0;
        public String BiltyNo;
        public Integer grnSupplierLoadingMasterId = 0;
        public Integer grnSupplierLoadingDetailId = 0;
        public Integer gdnBuyerDispatchMasterId = 0;
        public Integer gdnBuyerDispatchDetailId = 0;
        public Integer purchaseOrderMasterId = 0;
        public Integer purchaseOrderDetailId = 0;
        public Integer saleOrderMasterId = 0;
        public Integer saleOrderDetailId = 0;
        @NotParam public String ItemName;
        @NotParam public String ItemCode;
        @NotParam public String PackTypeDesc;
        @NotParam public String CropYear;
        @NotParam public String PackUom;
        @NotParam public Double PackUomEquivalent = 0d;
        @NotParam public String RateUom;
        @NotParam public Double RateUomEquivalent = 0d;
        @NotParam public String SaleRateUom;
        @NotParam public Double SaleRateUomEquivalent = 0d;
        @NotParam public String TaxNamePurchase;
        @NotParam public String TaxNameSale;
        @NotParam public Integer PurchaseOrderNo = 0;
        @NotParam public Timestamp PurchaseOrderDate;
        @NotParam public Integer SaleOrderNo = 0;
        @NotParam public Timestamp SaleOrderDate;
        @NotParam public Integer GdnNo = 0;
        @NotParam public Timestamp GdnDate;
        @NotParam public Integer GrnNo = 0;
        @NotParam public Timestamp GrnDate;
    }

    /** Generated from 0950_Architecture.Model.Inventory.CommisionAgentBillPurchaseExpense.cs. */
    public static class PurchaseExpense {
        public Double Amount = 0d;
        public Double Qty = 0d;
        public Double Rate = 0d;
        public Integer Id = 0;
        public Integer InvCommAgentBillTradeId = 0;
        public Integer InvExpItemId = 0;
        public String Remarks;
        public Integer grnSupplierLoadingMasterId = 0;
        public Integer purchaseOrderMasterId = 0;
        @NotParam public String OtherItemName;
        @NotParam public Integer PurchaseOrderNo = 0;
        @NotParam public Timestamp PurchaseOrderDate;
        @NotParam public Integer GrnNo = 0;
        @NotParam public Timestamp GrnDate;
    }

    /** Generated from 0952_Architecture.Model.Inventory.CommisionAgentBillSaleExpense.cs. */
    public static class SaleExpense {
        public Double Amount = 0d;
        public Double Qty = 0d;
        public Double Rate = 0d;
        public Integer Id = 0;
        public Integer InvCommAgentBillTradeId = 0;
        public Integer InvExpItemId = 0;
        public String Remarks;
        public Integer gdnBuyerDispatchMasterId = 0;
        public Integer saleOrderMasterId = 0;
        @NotParam public String OtherItemName;
        @NotParam public Integer SaleOrderNo = 0;
        @NotParam public Timestamp SaleOrderDate;
        @NotParam public Integer GdnNo = 0;
        @NotParam public Timestamp GdnDate;
    }

    /** Generated from 0951_Architecture.Model.Inventory.CommisionAgentBillSaleExpenseCreditToReleventAc.cs. */
    public static class SaleExpenseCreditToReleventAc {
        public Double Amount = 0d;
        public Double Qty = 0d;
        public Double Rate = 0d;
        public Integer Id = 0;
        public Integer InvCommAgentBillTradeId = 0;
        public Integer AccountId = 0;
        public String Remarks;
        public Double DebitAmount = 0d;
        public Integer gdnBuyerDispatchMasterId = 0;
        public Integer saleOrderMasterId = 0;
        public Integer PmItemId = 0;
        public Integer PackingTypeId = 0;
        public Integer EBWeightDeductionTermId = 0;
        @NotParam public String PmItemName;
        @NotParam public String PmItemCode;
        @NotParam public String PackingType;
        @NotParam public String EBWeightDeductionTerm;
        @NotParam public String AccountTitle;
        @NotParam public Integer SaleOrderNo = 0;
        @NotParam public Timestamp SaleOrderDate;
        @NotParam public Integer GdnNo = 0;
        @NotParam public Timestamp GdnDate;
    }

    /** Generated from 1077_Architecture.Model.Inventory.CommisionAgentBillPurchaseFreightExpense.cs. */
    public static class PurchaseFreightExpense {
        public BigDecimal Amount = BigDecimal.ZERO;
        public BigDecimal Qty = BigDecimal.ZERO;
        public BigDecimal Rate = BigDecimal.ZERO;
        public Integer AccountId = 0;
        public Integer Id = 0;
        public Integer InvCommAgentBillTradeId = 0;
        public String Remarks;
        public Double DebitAmount = 0d;
        public Integer grnSupplierLoadingMasterId = 0;
        public Integer purchaseOrderMasterId = 0;
        public Integer PmItemId = 0;
        public Integer PackingTypeId = 0;
        public Integer EBWeightDeductionTermId = 0;
        @NotParam public String PmItemName;
        @NotParam public String PmItemCode;
        @NotParam public String PackingType;
        @NotParam public String EBWeightDeductionTerm;
        @NotParam public String AccountTitle;
        @NotParam public Integer PurchaseOrderNo = 0;
        @NotParam public Timestamp PurchaseOrderDate;
        @NotParam public Integer GrnNo = 0;
        @NotParam public Timestamp GrnDate;
    }

    /** Generated from 0904_Architecture.Model.Inventory.CommisionAgentBillPaymentDetail.cs. */
    public static class PaymentDetail {
        public Timestamp DueDate;
        public BigDecimal dueAmount = BigDecimal.ZERO;
        public BigDecimal pctOfTotal = BigDecimal.ZERO;
        public Integer BaseDueDateTypeId = 0;
        public Integer DueDays = 0;
        public Integer EntrySideId = 0;
        public Integer Id = 0;
        public Integer InvCommAgentBillTradeId = 0;
        public Integer PaymentTermId = 0;
        public Integer purchaseOrderMasterId = 0;
        public Integer saleOrderMasterId = 0;
        public Integer sortNo = 0;
        public String remarks;
        @NotParam public String PaymentTerm;
        @NotParam public String BaseDateType;
        @NotParam public Integer PurchaseOrderNo = 0;
        @NotParam public Timestamp PurchaseOrderDate;
        @NotParam public Integer SaleOrderNo = 0;
        @NotParam public Timestamp SaleOrderDate;
    }

    /** Generated from 0905_Architecture.Model.Inventory.CommisionAgentBillCommissionDetail.cs. */
    public static class CommissionDetail {
        public BigDecimal commissionAmount = BigDecimal.ZERO;
        public BigDecimal commissionRate = BigDecimal.ZERO;
        public Integer agentTypeId = 0;
        public Integer commissionAgentId = 0;
        public Integer commissionTypeId = 0;
        public Integer EntrySideId = 0;
        public Integer Id = 0;
        public Integer InvCommAgentBillTradeId = 0;
        public Integer purchaseOrderMasterId = 0;
        public Integer rateUomId = 0;
        public Integer debitAccountId = 0;
        public Integer saleOrderMasterId = 0;
        public Integer sortNo = 0;
        public String commissionRemarks;
        @NotParam public String AgentType;
        @NotParam public String CommissionAgentName;
        @NotParam public String CommissionType;
        @NotParam public String EntrySide;
        @NotParam public String AccountTitle;
        @NotParam public Double RateUom = 0d;
        @NotParam public Integer SaleOrderNo = 0;
        @NotParam public Timestamp SaleOrderDate;
        @NotParam public Integer PurchaseOrderNo = 0;
        @NotParam public Timestamp PurchaseOrderDate;
    }

    /** Generated from 0903_Architecture.Model.Inventory.CommisionAgentBillFreightDetail.cs. */
    public static class FreightDetail {
        public Integer Id = 0;
        public Integer InvCommAgentBillTradeId = 0;
        public Integer EntrySideId = 0;
        public Integer AccountId = 0;
        public Integer grnSupplierLoadingMasterId = 0;
        public Integer gdnBuyerDispatchMasterId = 0;
        public BigDecimal Amount = BigDecimal.ZERO;
        public BigDecimal AddLessAmount = BigDecimal.ZERO;
        public BigDecimal NetAmount = BigDecimal.ZERO;
        public Integer sortNo = 0;
        public String Remarks;
        @NotParam public String AccountBusinessName;
        @NotParam public String AccountNickName;
        @NotParam public String AccountPartyCode;
        @NotParam public Integer GrnNo = 0;
        @NotParam public Timestamp GrnDate;
        @NotParam public Integer GdnNo = 0;
        @NotParam public Timestamp GdnDate;
    }

    /** Generated from 0902_Architecture.Model.Inventory.CommisionAgentBillTaxDetail.cs. */
    public static class TaxDetail {
        public Integer Id = 0;
        public Integer InvCommAgentBillTradeId = 0;
        public Integer EntrySideId = 0;
        public Integer TaxAccountId = 0;
        public Integer TaxTypeId = 0;
        public BigDecimal TaxPercantage = BigDecimal.ZERO;
        public BigDecimal TaxAmount = BigDecimal.ZERO;
        public Integer sortNo = 0;
        public String Remarks;
        @NotParam public String EntrySide;
        @NotParam public String TaxAccountTitle;
        @NotParam public String TaxTypeName;
    }
}
