package com.mst.services.sale.steel;

import com.mst.models.saleinvoice.SaleInvoiceModels.VoucherDetail;
import com.mst.models.saleinvoice.SaleInvoiceModels.VoucherHead;
import com.mst.repositories.SaleInvoiceRepository;
import com.mst.services.sale.engr.SaleEngrSupport;
import org.springframework.stereotype.Component;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import static com.mst.services.sale.engr.SaleEngrSupport.ci;
import static com.mst.services.sale.engr.SaleEngrSupport.str;
import static com.mst.services.sale.engr.SaleEngrSupport.toInt;

/**
 * Architecture.BLL.Steel.Inventory.Financials.SaleInvoice (BLL 0246 SaleInvoiceFinancial.MakeVoucherForSaleInvoice and BLL 0247
 * SaleInvoiceGeneralFinancialMethods) - the voucher of a Steel sale invoice (document types 1509 and 1510).
 *
 * Order of the voucher lines: ItemAndCustomerByWeightFinancial (supplier debit + item sale GL credit per line, the commission pair, the freight debits
 * to the item sale GL), FreightFinancial (transporter credits), SupplierAddLessFinancial (journal pairs), OtherExpenseFinancial.
 */
@Component
public class SaleStInvoiceFinancial {

    /** InvSaleInvoice (the header and its four lists) as the desktop fills it in Insert(). */
    public static class Doc {
        public int id, documentTypeId, docNo, supplierReferenceNo, supplierCustomerId, commAgentId, commUom, branchesId, projectsId, financialYearId, organizationId, companyId,
                entryUserId, modifyUserId, dueDays, currencyId, paymentTermsId, invoiceTypeId, transporterId, deliveryDays, approvedUserId;
        public String manualBillNo = "", commType = "", commRemarks = "", remarksHeader = "", deliveryTerm = "";
        public LocalDateTime docDate, dueDate, deliveryStartDate, expiryDate;
        public BigDecimal commRate = BigDecimal.ZERO, commAmount = BigDecimal.ZERO, exchangeRate = BigDecimal.ZERO, fcyAmount = BigDecimal.ZERO, totalQty = BigDecimal.ZERO,
                totalWeight = BigDecimal.ZERO, billAmount = BigDecimal.ZERO, freightAmount = BigDecimal.ZERO;
        public List<Dt> details = new ArrayList<>();
        public List<Frt> freights = new ArrayList<>();
        public List<Jrn> journals = new ArrayList<>();
        public List<Exp> expenses = new ArrayList<>();
    }

    /** InvSaleInvoiceDetail. */
    public static class Dt {
        public int id, lineId, invGdnId, invGdnDetailId, saleOrderId, saleOrderDetailId, itemId, jobLotId, packingTypeId, packUomId, rateUomScheduleId, warehouseId, cityId, gpNo,
                currencyId, actionTypeId, saleOrder;
        public String uomCode = "", rateUom = "", vehicleNo = "", remarksDetail = "", jobLotDescription = "";
        public BigDecimal itemQty = BigDecimal.ZERO, grossWeight = BigDecimal.ZERO, weightCut = BigDecimal.ZERO, weightCutTotal = BigDecimal.ZERO, netBillWeight = BigDecimal.ZERO,
                netStockWeight = BigDecimal.ZERO, itemRate = BigDecimal.ZERO, rateCut = BigDecimal.ZERO, rateCutAmount = BigDecimal.ZERO, itemAmount = BigDecimal.ZERO,
                itemNetAmount = BigDecimal.ZERO, freightAmount = BigDecimal.ZERO, journalAmount = BigDecimal.ZERO, expenseAmount = BigDecimal.ZERO, commissionAmount = BigDecimal.ZERO,
                exchangeRate = BigDecimal.ZERO, fcyAmount = BigDecimal.ZERO, adLsWeight = BigDecimal.ZERO;
        /** Set only by screens whose desktop form fills these (544): sent to the procedure; null = left out like an unset property. */
        public LocalDateTime gpDate;
        public String remarksSent;
    }

    public static class Frt { public int invGdnId, tansporterId; public BigDecimal creditAmount = BigDecimal.ZERO, debitAmount = BigDecimal.ZERO, percentage = BigDecimal.ZERO, frQty = BigDecimal.ZERO, frRate = BigDecimal.ZERO; public String remarks; }
    public static class Jrn { public int chartofAccountId; public String jvRemarks = ""; public BigDecimal jvPrcnt = BigDecimal.ZERO, jvQty = BigDecimal.ZERO, jvRate = BigDecimal.ZERO, jvDebit = BigDecimal.ZERO, jvCredit = BigDecimal.ZERO; }
    public static class Exp { public int invOtherItemId; public BigDecimal qty = BigDecimal.ZERO, rate = BigDecimal.ZERO, amount = BigDecimal.ZERO; public String remarks = ""; }

    /** voucherHead + its voucherDetailList. */
    public static class Voucher { public VoucherHead head = new VoucherHead(); public List<VoucherDetail> details = new ArrayList<>(); }

    private final SaleInvoiceRepository repo;
    private final SaleEngrSupport sup;

    public SaleStInvoiceFinancial(SaleInvoiceRepository repo, SaleEngrSupport sup) { this.repo = repo; this.sup = sup; }

    private static double d(BigDecimal b) { return b == null ? 0.0 : b.doubleValue(); }

    /** decimal.ToString() of a value that came from a double: no trailing zeros. */
    private static String t(BigDecimal b) { return b == null || b.signum() == 0 ? "0" : b.stripTrailingZeros().toPlainString(); }

    /** MakeVoucherForSaleInvoice (BLL 0246). */
    public Voucher make(Doc obj) {
        Voucher out = new Voucher();
        VoucherHead vh = out.head;
        vh.DocumentTypeId = obj.documentTypeId;
        vh.DocumentTypeSrNo = obj.id;
        vh.RefDocNoId = obj.id;
        vh.VoucherCode = obj.docNo;
        vh.VoucherDate = obj.docDate;
        vh.Remarks = obj.remarksHeader == null ? "" : obj.remarksHeader;
        vh.RemarksOtherLingo = "";
        vh.VoucherAmount = d(obj.billAmount);
        vh.ChequeDate = LocalDate.now().atStartOfDay();
        vh.IncludeWHT = false;
        vh.BranchId = obj.branchesId;
        vh.ProjectId = obj.projectsId;
        vh.BillAmount = d(obj.billAmount);
        vh.ManualBillNo = obj.manualBillNo == null ? "" : obj.manualBillNo;
        vh.DueDate = obj.dueDate;
        vh.DueDays = obj.dueDays;
        vh.OrganizationId = obj.organizationId;
        vh.CompanyId = obj.companyId;
        vh.FinancialYearId = obj.financialYearId;
        vh.EntryUser = obj.entryUserId;
        vh.EntryDate = LocalDateTime.now();
        vh.ModifyDate = LocalDateTime.now();
        vh.ModifyUser = obj.modifyUserId;

        String cfg = sup.config("StockAgainstAccount");
        if (cfg == null || cfg.isEmpty()) throw new IllegalArgumentException("Stock AgainstAc Configuration Not Found");
        int offset = toInt(cfg);
        Map<Integer, Map<String, Object>> items = repo.itemGl(obj.organizationId, obj.companyId);
        List<Map<String, Object>> others = repo.otherItems(obj.organizationId, obj.companyId);
        Map<Integer, Map<String, Object>> parties = repo.partyGl(obj.organizationId, obj.companyId);
        if (!parties.isEmpty()) {
            if (obj.commAmount.signum() > 0) {
                if (obj.commAgentId <= 0) throw new IllegalArgumentException("Commission Agent GLAccountId not Found");
                Map<String, Object> ag = parties.get(obj.commAgentId);
                if (ag != null) vh.AgainstAccountId = toInt(ci(ag, "GlAccountId"));
            }
            Map<String, Object> sp = parties.get(obj.supplierCustomerId);
            if (sp == null) throw new IllegalArgumentException("Supplier GLAccountId not Found");
            vh.RefAccountId = toInt(ci(sp, "GlAccountId"));
        }
        final int supplierGl = vh.RefAccountId, commissionGl = vh.AgainstAccountId;

        itemAndCustomerByWeight(obj, items, supplierGl, commissionGl, offset, out.details);
        freight(obj, supplierGl, offset, out.details);
        supplierAddLess(obj, supplierGl, out.details);
        otherExpense(obj, others, items, supplierGl, offset, out.details);
        return out;
    }

    private boolean steelType(Doc obj) { return obj.documentTypeId == 1509 || obj.documentTypeId == 1510; }

    /** SupplierAddLessFinancial (BLL 0247). */
    private void supplierAddLess(Doc obj, int supplierGl, List<VoucherDetail> out) {
        if (steelType(obj) && !obj.journals.isEmpty()) {
            for (Jrn j : obj.journals) {
                if (j.chartofAccountId != 0) {
                    VoucherDetail a = new VoucherDetail();
                    a.AccountId = j.chartofAccountId; a.AgainstAccountId = supplierGl; a.Comments = j.jvRemarks == null ? "" : j.jvRemarks;
                    a.DebitAmount = d(j.jvDebit); a.CreditAmount = d(j.jvCredit);
                    out.add(a);
                    VoucherDetail b = new VoucherDetail();
                    b.AccountId = supplierGl; b.AgainstAccountId = j.chartofAccountId; b.Comments = j.jvRemarks == null ? "" : j.jvRemarks;
                    b.CreditAmount = d(j.jvDebit); b.DebitAmount = d(j.jvCredit);
                    out.add(b);
                }
            }
        }
    }

    /** FreightFinancial (BLL 0247). The header FreightAmount branch tests 1509 twice and the amount is never set on this screen. */
    private void freight(Doc obj, int supplierGl, int offset, List<VoucherDetail> out) {
        if (steelType(obj) && !obj.freights.isEmpty()) {
            for (Frt f : obj.freights) {
                if (f.tansporterId != 0) {
                    VoucherDetail a = new VoucherDetail();
                    a.AccountId = f.tansporterId; a.AgainstAccountId = offset; a.Comments = obj.remarksHeader;
                    a.DebitAmount = 0.0; a.CreditAmount = d(f.creditAmount);
                    out.add(a);
                }
            }
        }
        if (obj.freightAmount.signum() > 0 && (obj.documentTypeId == 1509)) {
            VoucherDetail a = new VoucherDetail();
            a.AccountId = obj.transporterId; a.AgainstAccountId = supplierGl; a.Comments = obj.remarksHeader; a.DebitAmount = d(obj.freightAmount); a.CreditAmount = 0;
            out.add(a);
            VoucherDetail b = new VoucherDetail();
            b.AccountId = supplierGl; b.AgainstAccountId = obj.transporterId; b.Comments = obj.remarksHeader; b.CreditAmount = d(obj.freightAmount); b.DebitAmount = 0;
            out.add(b);
        }
    }

    /** ItemAndCustomerByWeightFinancial (BLL 0247). */
    private void itemAndCustomerByWeight(Doc obj, Map<Integer, Map<String, Object>> items, int supplierGl, int commissionGl, int offset, List<VoucherDetail> out) {
        if (steelType(obj) && !obj.details.isEmpty()) {
            for (Dt item : obj.details) {
                if (item.actionTypeId == 3) continue;
                String text = obj.remarksHeader;
                if (!items.isEmpty()) {
                    Map<String, Object> it = items.get(item.itemId);
                    if (it != null) {
                        int saleGl = toInt(ci(it, "SaleGLAC"));
                        text = str(ci(it, "ItemName")) + "  " + (item.jobLotDescription == null ? "" : item.jobLotDescription) + "  " + (item.uomCode == null ? "" : item.uomCode)
                                + "  " + t(item.netBillWeight) + " @" + t(item.itemRate) + "/- ";
                        if (item.gpNo > 0) text = text + "GP# " + item.gpNo;
                        if (item.vehicleNo != null && !item.vehicleNo.isEmpty()) text = text + " V# " + item.vehicleNo;
                        if (item.remarksDetail != null && !item.remarksDetail.isEmpty()) text = text + " " + item.remarksDetail;

                        VoucherDetail a = new VoucherDetail();
                        a.LineId = 0; a.AccountId = supplierGl; a.AgainstAccountId = saleGl; a.Comments = text;
                        a.DebitAmount = d(item.itemAmount); fill(a, item, obj);
                        out.add(a);
                        VoucherDetail b = new VoucherDetail();
                        b.LineId = 0; b.AccountId = saleGl; b.AgainstAccountId = supplierGl; b.Comments = text;
                        b.CreditAmount = d(item.itemAmount); fill(b, item, obj);
                        out.add(b);
                        if (item.commissionAmount.signum() > 0) {
                            VoucherDetail c3 = new VoucherDetail();
                            c3.AccountId = saleGl; c3.AgainstAccountId = commissionGl; c3.Comments = obj.commRemarks == null ? "" : obj.commRemarks;
                            c3.DebitAmount = d(item.commissionAmount); c3.JobLotId = item.jobLotId;
                            out.add(c3);
                            VoucherDetail c4 = new VoucherDetail();
                            c4.AccountId = commissionGl; c4.AgainstAccountId = saleGl; c4.Comments = obj.commRemarks == null ? "" : obj.commRemarks;
                            c4.DebitAmount = 0; c4.CreditAmount = d(item.commissionAmount); c4.JobLotId = item.jobLotId;
                            out.add(c4);
                        }
                        continue;
                    }
                    throw new IllegalArgumentException("Item Record Not found");
                }
                throw new IllegalArgumentException("Item Record Not found");
            }
            for (Dt f : obj.details) {
                if (f.freightAmount.signum() > 0) {
                    if (items.isEmpty()) throw new IllegalArgumentException("Item Record Not found");
                    Map<String, Object> it = items.get(f.itemId);
                    if (it == null) throw new IllegalArgumentException("Item Record Not found");
                    VoucherDetail a = new VoucherDetail();
                    a.AccountId = toInt(ci(it, "SaleGLAC")); a.AgainstAccountId = offset; a.Comments = f.remarksDetail;
                    a.DebitAmount = d(f.freightAmount); a.CreditAmount = 0; a.JobLotId = f.jobLotId;
                    out.add(a);
                }
            }
        }
    }

    private static void fill(VoucherDetail v, Dt item, Doc obj) {
        v.ItemId = item.itemId;
        v.QtyOut = d(item.itemQty);
        v.ItemRate = d(item.itemRate);
        v.WeightOut = d(item.netBillWeight);
        v.RateCut = d(item.rateCut);
        v.RateCutAmount = d(item.rateCutAmount);
        v.ItemAmount = d(item.itemAmount);
        v.Expenses = d(item.expenseAmount);
        v.Freight = d(item.freightAmount);
        v.Commission = d(item.commissionAmount);
        v.OrderNo = item.saleOrder;
        v.GpNo = item.gpNo;
        v.JobLotId = item.jobLotId;
        v.VehicleNo = item.vehicleNo;
        v.SupplierCustomerId = obj.supplierCustomerId;
    }

    /** OtherExpenseFinancial (BLL 0247). */
    private void otherExpense(Doc obj, List<Map<String, Object>> others, Map<Integer, Map<String, Object>> items, int supplierGl, int offset, List<VoucherDetail> out) {
        if (steelType(obj) && !obj.expenses.isEmpty()) {
            if (SaleInvoiceRepository.truthy(sup.config("CreditAmountInItemSaleGL"))) {
                for (Exp e : obj.expenses) {
                    if (e.amount.signum() > 0) {
                        if (others.isEmpty()) throw new IllegalArgumentException("Other Items not found");
                        Map<String, Object> o = other(others, e.invOtherItemId);
                        if (o == null) throw new IllegalArgumentException("Other Items Sale GL Account not found");
                        String cm = str(ci(o, "OtherItemName")) + " " + (e.remarks == null ? "" : e.remarks);
                        VoucherDetail a = new VoucherDetail();
                        a.AccountId = supplierGl; a.AgainstAccountId = toInt(ci(o, "SaleGLAcId")); a.Comments = cm; a.DebitAmount = d(e.amount);
                        out.add(a);
                        VoucherDetail b = new VoucherDetail();
                        b.AccountId = toInt(ci(o, "SaleGLAcId")); b.AgainstAccountId = supplierGl; b.Comments = cm; b.CreditAmount = d(e.amount);
                        out.add(b);
                    }
                }
                return;
            }
            String text = "";
            for (Exp e : obj.expenses) {
                if (e.amount.signum() > 0) {
                    if (others.isEmpty()) throw new IllegalArgumentException("Other Items not found");
                    Map<String, Object> o = other(others, e.invOtherItemId);
                    if (o == null) throw new IllegalArgumentException("Other Items Sale GL Account not found");
                    VoucherDetail a = new VoucherDetail();
                    a.AccountId = supplierGl; a.AgainstAccountId = offset; a.Comments = str(ci(o, "OtherItemName")) + " " + (e.remarks == null ? "" : e.remarks); a.DebitAmount = d(e.amount);
                    out.add(a);
                    text += a.Comments;
                }
            }
            for (Dt x : obj.details) {
                if (x.expenseAmount.signum() > 0) {
                    if (items.isEmpty()) throw new IllegalArgumentException("Items not found");
                    Map<String, Object> it = items.get(x.itemId);
                    if (it == null) throw new IllegalArgumentException("Items Sale GL Account not found");
                    VoucherDetail a = new VoucherDetail();
                    a.AccountId = toInt(ci(it, "SaleGLAC")); a.AgainstAccountId = offset; a.Comments = text; a.CreditAmount = d(x.expenseAmount);
                    out.add(a);
                }
            }
        }
    }

    private static Map<String, Object> other(List<Map<String, Object>> others, int id) {
        for (Map<String, Object> o : others) if (toInt(ci(o, "Id")) == id) return o;
        return null;
    }
}
