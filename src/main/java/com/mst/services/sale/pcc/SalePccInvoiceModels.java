package com.mst.services.sale.pcc;

import com.mst.models.saleinvoice.SaleInvoiceModels.NotParam;
import com.mst.models.saleinvoice.SaleInvoiceModels.Nullable;
import com.mst.models.saleinvoice.SaleInvoiceModels.StockDetail;
import com.mst.models.saleinvoice.SaleInvoiceModels.VoucherDetail;
import com.mst.models.saleinvoice.SaleInvoiceModels.VoucherHead;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

/**
 * Architecture.Model.pcc.InvSaleInvoice (0507) and its child models 0508-0512, shared by the three Sale Pcc invoice screens
 * (546 SaleInvoiceDirectConcrete / 1856, 547 SaleInvoiceAgainstGDNConcrete / 1861, 553 SaleInvoiceReturnConcrete / 1862).
 * Public field names are the procedure parameter names (GenericProvider.SetProc sends every non-virtual property); the model's
 * virtual properties are marked @NotParam. An unset (null) value is not sent, exactly like the desktop's unset string.
 */
public final class SalePccInvoiceModels {
    private SalePccInvoiceModels() { }

    /** pcc.USP_InvSaleInvoice_Insert / _Update. */
    public static class Head {
        public boolean IsApproved;
        public LocalDateTime ApprovedDate;
        public LocalDateTime DeliveryStartDate;
        public LocalDateTime DocDate;
        public LocalDateTime DueDate;
        public LocalDateTime EntryDate;
        public LocalDateTime ExpiryDate;
        public LocalDateTime ModifyDate;
        public BigDecimal BillAmount = BigDecimal.ZERO;
        public BigDecimal BillAmountWithoutDiscount = BigDecimal.ZERO;
        public BigDecimal SettlementDiscountHeader = BigDecimal.ZERO;
        public BigDecimal CommissionAmount = BigDecimal.ZERO;
        public BigDecimal CommissionRate = BigDecimal.ZERO;
        public BigDecimal CommissionUom = BigDecimal.ZERO;
        public BigDecimal DiscountAmountHeader = BigDecimal.ZERO;
        public BigDecimal ExchangeRate = BigDecimal.ZERO;
        public BigDecimal FcyAmount = BigDecimal.ZERO;
        public BigDecimal FrieghtAmountHeader = BigDecimal.ZERO;
        public BigDecimal InvoiceQty = BigDecimal.ZERO;
        public BigDecimal InvoiceWeight = BigDecimal.ZERO;
        public BigDecimal ItemAmountHeader = BigDecimal.ZERO;
        public BigDecimal ItemNetAmountHeader = BigDecimal.ZERO;
        public BigDecimal OtherWagesHeader = BigDecimal.ZERO;
        public BigDecimal PartyAddLessHeader = BigDecimal.ZERO;
        public String Distance;
        public int ActionId;
        public int ApprovedUserId;
        public int SettlementDiscountAccountId;
        public int CommissionDebitAcId;
        public int BranchesId;
        public int CommissionAgentId;
        public int CompanyId;
        public int CurrencyId;
        public int DeliveryDays;
        public int DocNo;
        public int DocumentTypeId;
        public int DueDays;
        public int EntryUserId;
        public int FinancialYearId;
        public int Id;
        public int ModifyUserId;
        public int OrganizationId;
        public int OtherWagesAccountId;
        public int PaymentTermsId;
        public int PendingForView;
        public int ProjectsId;
        public int RefSalesManId;
        public int SupplierCustomerId;
        public int TransporterId;
        public int VisitedById;
        public int AutoUpdate;
        public String CommissionRemarks;
        public String CommissionType;
        public String DeliveryRemarks;
        public String DeliveryTerm;
        public String ManualBillNo;
        public String ReferencPartyAddress;
        public String ReferencPartyCellNo;
        public String ReferencPartyName;
        public String RefrenenceNo;
        public String RemarksHeader;
        public int ReferencePartyId;
        public int BuildingHeightId;
        public int BuildingStoreyId;
        public String BuildingArea;
    }

    /** pcc.USP_InvSaleInvoiceDetail_Insert. */
    public static class Detail {
        public boolean CommOnSale;
        public boolean IsApproved;
        public boolean IsFOC;
        public LocalDateTime GpDate;
        public BigDecimal CommissionAmount = BigDecimal.ZERO;
        public BigDecimal ExchangeRate = BigDecimal.ZERO;
        public BigDecimal ExpenseAmount = BigDecimal.ZERO;
        public BigDecimal FcyAmount = BigDecimal.ZERO;
        public BigDecimal FreightAmount = BigDecimal.ZERO;
        public BigDecimal ItemAmount = BigDecimal.ZERO;
        public BigDecimal ItemAmountWithDisc = BigDecimal.ZERO;
        public BigDecimal ItemDiscountAmount = BigDecimal.ZERO;
        public BigDecimal ItemDiscountRate = BigDecimal.ZERO;
        public BigDecimal ItemNetAmount = BigDecimal.ZERO;
        public BigDecimal ItemNetWeight = BigDecimal.ZERO;
        public BigDecimal ItemQty = BigDecimal.ZERO;
        public BigDecimal ItemRate = BigDecimal.ZERO;
        public BigDecimal ItemWeight = BigDecimal.ZERO;
        public BigDecimal JournalAmount = BigDecimal.ZERO;
        public BigDecimal RateCut = BigDecimal.ZERO;
        public BigDecimal RateCutAmount = BigDecimal.ZERO;
        public BigDecimal VarientEquivalent = BigDecimal.ZERO;
        public BigDecimal WagesAmount = BigDecimal.ZERO;
        public BigDecimal ItemRateWithOutAddLess = BigDecimal.ZERO;
        public BigDecimal RateAddLess = BigDecimal.ZERO;
        public BigDecimal CgsRate = BigDecimal.ZERO;
        public int ScheduleId;
        public int ActionTypeId;
        public int CityId;
        public int CurrencyId;
        public int GpNo;
        public int Id;
        public int InvGdnDetailId;
        public int InvGdnId;
        public int InvSaleInvoiceId;
        public int ItemAttributeVarientId;
        public int ItemDiscountTypeId;
        public int ItemUomId;
        public int ItemId;
        public int JobLotId;
        public int LineId;
        public int SaleOrderDetailId;
        public int SaleOrderId;
        public int WarehouseId;
        public String RemarksDetail;
        public String VehicleNo;
        // virtual properties of the model (not parameters)
        @NotParam public int SaleGLAC;
        @NotParam public int SaleOrderNo;
        @NotParam public int GdnNo;
        @NotParam public String ItemName;
    }

    /** pcc.USP_InvSaleInvoiceExpense_Insert. */
    public static class Expense {
        public BigDecimal Amount = BigDecimal.ZERO;
        public BigDecimal Qty = BigDecimal.ZERO;
        public BigDecimal Rate = BigDecimal.ZERO;
        public int Id;
        public int InvOtherItemId;
        public int InvSaleInvoiceId;
        public String Remarks;
    }

    /** pcc.USP_InvSaleInvoiceFreight_Insert. */
    public static class Freight {
        public BigDecimal Debit = BigDecimal.ZERO;
        public BigDecimal FreightAmount = BigDecimal.ZERO;
        public BigDecimal FrQty = BigDecimal.ZERO;
        public BigDecimal FrRate = BigDecimal.ZERO;
        public BigDecimal FrWeight = BigDecimal.ZERO;
        public int Id;
        public int InvGdnId;
        public int InvSaleInvoiceId;
        public int TansporterId;
        public int TransporterSupCustId;
        public String Remarks;
    }

    /** pcc.USP_InvSaleInvoiceJournal_Insert (the Party Add / Less grid). */
    public static class Journal {
        public BigDecimal JvCredit = BigDecimal.ZERO;
        public BigDecimal JvDebit = BigDecimal.ZERO;
        public BigDecimal JvPrcnt = BigDecimal.ZERO;
        public BigDecimal JvQty = BigDecimal.ZERO;
        public BigDecimal JvRate = BigDecimal.ZERO;
        public int ChartofAccountId;
        public int Id;
        public int InvGdnId;
        public int InvSaleInvoiceId;
        public int TransporterSupCustId;
        public String JvRemarks;
    }

    /** pcc.USP_InvSaleInvoiceWagesDetail_Insert. */
    public static class Wages {
        public BigDecimal Amount = BigDecimal.ZERO;
        public BigDecimal Qty = BigDecimal.ZERO;
        public BigDecimal ItemNetWeight = BigDecimal.ZERO;
        public BigDecimal Rate = BigDecimal.ZERO;
        public BigDecimal AddLess = BigDecimal.ZERO;
        public BigDecimal NetAmount = BigDecimal.ZERO;
        public BigDecimal VarientEquivalent = BigDecimal.ZERO;
        public int ContractorId;
        public int ContractorWagesRateScheduleId;
        public int Id;
        public int WagesTypeId;
        public int InvSaleInvoiceId;
        public int ItemAttributeVarientId;
        public int ItemId;
        public int ParentUomId;
        public int SaleOrderId;
        public int SaleOrderWagesId;
        public int InvGdnId;
        public int InvGdnWagesId;
        public String Remarks;
    }

    /** The invoice as the pcc BLL/DAL sees it (InvSaleInvoice with its child lists and CommonIdsForFinancials). */
    public static class Invoice {
        public Head h = new Head();
        public List<Detail> details = new ArrayList<>();
        public List<Freight> freights = new ArrayList<>();
        public List<Journal> journals = new ArrayList<>();
        public List<Expense> expenses = new ArrayList<>();
        public List<Wages> wages = new ArrayList<>();
        /** CommonIdsForFinancials */
        public int supplierGlAccountId;
        public int commissionGlAccountId;
        public String companyName = "";
        /** pcc.InvSaleInvoice.VoucherHeadInvoices (the BLL fills it, the DAL appends the CGS rows) */
        public VoucherHead voucherHead = new VoucherHead();
        public List<VoucherDetail> voucherDetails = new ArrayList<>();
        public List<StockDetail> stock = new ArrayList<>();
    }
}
