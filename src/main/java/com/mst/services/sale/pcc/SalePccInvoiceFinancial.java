package com.mst.services.sale.pcc;

import com.mst.models.saleinvoice.SaleInvoiceModels.VoucherDetail;
import com.mst.models.saleinvoice.SaleInvoiceModels.VoucherHead;
import com.mst.repositories.SaleInvoiceRepository;
import com.mst.services.sale.pcc.SalePccInvoiceModels.*;
import org.springframework.stereotype.Component;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.*;

import static com.mst.repositories.SaleInvoiceRepository.*;

/**
 * Architecture.BLL.pcc.SaleInvoice.SaleInvoiceFinancial.MakeVoucherForSaleInvoice (0358) and SaleInvoiceGeneralFinancialMethods (0359):
 * the voucher of the Sale Pcc invoices 1856 (Direct), 1861 (Against GDN) and 1862 (Return). The CGS rows the DAL appends afterwards
 * are built in SalePccInvoicePersist.
 */
@Component
public class SalePccInvoiceFinancial {
    private final SaleInvoiceRepository repo;

    public SalePccInvoiceFinancial(SaleInvoiceRepository repo) { this.repo = repo; }

    private int org, company;
    private final Map<String, String> configCache = new HashMap<>();
    private Set<Integer> features;

    private String config(String name) { return configCache.computeIfAbsent(name, k -> repo.config(org, company, k)); }

    private boolean feature(int id) {
        if (features == null) {
            features = new HashSet<>();
            for (var r : repo.q("EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?, @CompanyId=?", org, company)) features.add(i(col(r, "Id")));
        }
        return features.contains(id);
    }

    // .NET helpers
    static String r0(BigDecimal v) { return v.setScale(0, RoundingMode.HALF_EVEN).toPlainString(); }
    static String r3(BigDecimal v) { return (v.scale() > 3 ? v.setScale(3, RoundingMode.HALF_EVEN) : v).toPlainString(); }
    static double dbl(BigDecimal v) { return v == null ? 0.0 : v.doubleValue(); }

    /** 0358 MakeVoucherForSaleInvoice. Fills inv.voucherHead / inv.voucherDetails and the CommonIdsForFinancials. */
    public void makeVoucher(Invoice inv) {
        Head h = inv.h;
        org = h.OrganizationId; company = h.CompanyId;
        configCache.clear(); features = null;
        VoucherHead vh = new VoucherHead();
        List<VoucherDetail> out = new ArrayList<>();
        vh.DocumentTypeId = h.DocumentTypeId;
        vh.DocumentTypeSrNo = h.Id;
        vh.RefDocNoId = h.Id;
        vh.VoucherCode = h.DocNo;
        vh.VoucherDate = h.DocDate;
        vh.Remarks = s(h.RemarksHeader);
        vh.RemarksOtherLingo = "";
        vh.VoucherAmount = dbl(h.BillAmount);
        vh.ChequeDate = LocalDate.now().atStartOfDay();
        vh.IncludeWHT = false;
        vh.BranchId = h.BranchesId;
        vh.ProjectId = h.ProjectsId;
        vh.BillAmount = dbl(h.BillAmount);
        vh.ManualBillNo = s(h.ManualBillNo);
        vh.DueDate = h.DueDate;
        vh.DueDays = h.DueDays;
        vh.MultiCurrencyId = h.CurrencyId;
        vh.ExchangeCurrencyRate = dbl(h.ExchangeRate);
        vh.FcAmount = dbl(h.FcyAmount);
        vh.OrganizationId = h.OrganizationId;
        vh.CompanyId = h.CompanyId;
        vh.FinancialYearId = h.FinancialYearId;
        vh.EntryUser = h.EntryUserId;
        vh.EntryDate = LocalDateTime.now();
        vh.ModifyDate = LocalDateTime.now();
        vh.ModifyUser = h.ModifyUserId;

        Map<Integer, Map<String, Object>> items = repo.itemGl(org, company);          // GetItemListForFinancialEffects
        List<Map<String, Object>> otherItems = repo.otherItems(org, company);         // GetOtherItemForFinancialEffects
        Map<Integer, Map<String, Object>> parties = repo.partyGl(org, company);       // GetSupplierCustomerListForFinancialEffects
        String companyName = "";
        if (!parties.isEmpty()) {
            if (h.CommissionAmount.signum() > 0) {
                if (h.CommissionAgentId <= 0) throw new IllegalStateException("Commission Agent GLAccountId not Found");
                Map<String, Object> agent = parties.get(h.CommissionAgentId);
                if (agent != null) vh.AgainstAccountId = i(col(agent, "GlAccountId"));
            }
            Map<String, Object> party = parties.get(h.SupplierCustomerId);
            if (party == null) throw new IllegalStateException("Supplier GLAccountId not Found");
            vh.RefAccountId = i(col(party, "GlAccountId"));
            companyName = s(col(party, "CompanyName"));
        }
        inv.supplierGlAccountId = vh.RefAccountId;
        inv.commissionGlAccountId = vh.AgainstAccountId;
        inv.companyName = companyName;

        out.addAll(itemAndCustomerByWeight(inv, items));
        final int doc = h.DocumentTypeId;
        if ((doc == 1856 || doc == 1861) && h.FrieghtAmountHeader.signum() > 0) {
            double amt = dbl(h.FrieghtAmountHeader);
            VoucherDetail a = new VoucherDetail();
            a.AccountId = vh.RefAccountId; a.AgainstAccountId = h.TransporterId; a.Comments = "Carriage / Freight"; a.DebitAmount = amt;
            a.SubsidiaryTypeId = 1; a.SubsidiaryAccountId = h.SupplierCustomerId; a.SupplierCustomerId = h.SupplierCustomerId;
            out.add(a);
            VoucherDetail b = new VoucherDetail();
            b.AccountId = h.TransporterId; b.AgainstAccountId = vh.RefAccountId; b.Comments = "Carriage / Freight"; b.CreditAmount = amt; b.DebitAmount = 0;
            b.SubsidiaryAgainstTypeId = 1; b.SubsidiaryAgainstAccountId = h.SupplierCustomerId;
            out.add(b);
        }
        if ((doc == 1856 || doc == 1861) && h.OtherWagesHeader.signum() > 0) {
            double amt = dbl(h.OtherWagesHeader);
            VoucherDetail a = new VoucherDetail();
            a.AccountId = vh.RefAccountId; a.AgainstAccountId = h.OtherWagesAccountId; a.Comments = "Labour / Wages"; a.DebitAmount = amt;
            a.SubsidiaryTypeId = 1; a.SubsidiaryAccountId = h.SupplierCustomerId; a.SupplierCustomerId = h.SupplierCustomerId;
            out.add(a);
            VoucherDetail b = new VoucherDetail();
            b.AccountId = h.OtherWagesAccountId; b.AgainstAccountId = vh.RefAccountId; b.Comments = "Labour / Wages"; b.CreditAmount = amt; b.DebitAmount = 0;
            b.SubsidiaryAgainstTypeId = 1; b.SubsidiaryAgainstAccountId = h.SupplierCustomerId;
            out.add(b);
        }
        out.addAll(supplierAddLess(inv));
        out.addAll(otherExpense(inv, otherItems));
        if ((doc == 1856 || doc == 1861 || doc == 1862) && h.CommissionAmount.signum() > 0) {
            double amt = dbl(h.CommissionAmount);
            if (doc != 1862) {
                VoucherDetail a = new VoucherDetail();
                a.AccountId = h.CommissionDebitAcId; a.AgainstAccountId = vh.AgainstAccountId; a.Comments = s(h.CommissionRemarks); a.DebitAmount = amt;
                a.SubsidiaryAgainstTypeId = 1; a.SubsidiaryAgainstAccountId = h.CommissionAgentId;
                out.add(a);
                VoucherDetail b = new VoucherDetail();
                b.AccountId = vh.AgainstAccountId; b.AgainstAccountId = h.CommissionDebitAcId; b.Comments = s(h.CommissionRemarks); b.CreditAmount = amt;
                b.SubsidiaryTypeId = 1; b.SubsidiaryAccountId = h.CommissionAgentId; b.SupplierCustomerId = h.CommissionAgentId;
                out.add(b);
            } else {
                VoucherDetail a = new VoucherDetail();
                a.AccountId = vh.AgainstAccountId; a.AgainstAccountId = h.CommissionDebitAcId; a.Comments = s(h.CommissionRemarks); a.DebitAmount = amt;
                a.SubsidiaryTypeId = 1; a.SubsidiaryAccountId = h.CommissionAgentId; a.SupplierCustomerId = h.CommissionAgentId;
                out.add(a);
                VoucherDetail b = new VoucherDetail();
                b.AccountId = h.CommissionDebitAcId; b.AgainstAccountId = vh.AgainstAccountId; b.Comments = s(h.CommissionRemarks); b.CreditAmount = amt;
                out.add(b);
            }
        }
        if ((doc == 1856 || doc == 1861) && h.SettlementDiscountHeader.signum() > 0) {
            double amt = dbl(h.SettlementDiscountHeader);
            if (h.SettlementDiscountAccountId > 0) {
                VoucherDetail a = new VoucherDetail();
                a.AccountId = h.SettlementDiscountAccountId; a.AgainstAccountId = vh.RefAccountId; a.Comments = "Settle Discount " + companyName; a.DebitAmount = amt;
                a.SubsidiaryTypeId = 1; a.SubsidiaryAccountId = h.SupplierCustomerId;
                out.add(a);
            }
            VoucherDetail b = new VoucherDetail();
            b.AccountId = vh.RefAccountId;
            b.AgainstAccountId = h.SettlementDiscountAccountId > 0 ? h.SettlementDiscountAccountId : vh.RefAccountId;
            b.Comments = "Settle Discount"; b.CreditAmount = amt;
            b.SubsidiaryTypeId = 1; b.SubsidiaryAccountId = h.SupplierCustomerId; b.SupplierCustomerId = h.SupplierCustomerId;
            out.add(b);
        }
        out.addAll(cgsTransactions(inv, items));
        boolean flag = inv.details.stream().anyMatch(rr -> rr.InvGdnId > 0 && rr.ActionTypeId != 3);
        if (doc == 1861 && !flag) out.addAll(contractWages(inv, parties));
        vh.Remarks = h.RemarksHeader;
        inv.voucherHead = vh;
        inv.voucherDetails = out;
    }

    // ------------------------------------------------------------------ 0359 ItemAndCustomerByWeightFinancial / SupplierAddLessFinancial

    private List<VoucherDetail> supplierAddLess(Invoice inv) {
        List<VoucherDetail> out = new ArrayList<>();
        Head h = inv.h;
        if ((h.DocumentTypeId == 1856 || h.DocumentTypeId == 1861 || h.DocumentTypeId == 1862) && !inv.journals.isEmpty()) {
            for (Journal j : inv.journals) {
                if (j.ChartofAccountId == 0) continue;
                VoucherDetail a = new VoucherDetail();
                a.AccountId = j.ChartofAccountId; a.AgainstAccountId = inv.supplierGlAccountId; a.Comments = s(j.JvRemarks);
                a.DebitAmount = dbl(j.JvDebit); a.CreditAmount = dbl(j.JvCredit);
                a.SubsidiaryTypeId = 1; a.SubsidiaryAccountId = j.TransporterSupCustId; a.SupplierCustomerId = j.TransporterSupCustId;
                out.add(a);
                VoucherDetail b = new VoucherDetail();
                b.AccountId = inv.supplierGlAccountId; b.AgainstAccountId = j.ChartofAccountId; b.Comments = s(j.JvRemarks);
                b.CreditAmount = dbl(j.JvDebit); b.DebitAmount = dbl(j.JvCredit);
                b.SubsidiaryTypeId = 1; b.SubsidiaryAccountId = h.SupplierCustomerId; b.SupplierCustomerId = h.SupplierCustomerId;
                out.add(b);
            }
        }
        return out;
    }

    private List<VoucherDetail> itemAndCustomerByWeight(Invoice inv, Map<Integer, Map<String, Object>> items) {
        List<VoucherDetail> out = new ArrayList<>();
        Head h = inv.h;
        final int doc = h.DocumentTypeId;
        if (!((doc == 1856 || doc == 1861 || doc == 1862) && !inv.details.isEmpty())) return out;
        final DateTimeFormatter dd = DateTimeFormatter.ofPattern("dd-MMM-yyyy", Locale.ENGLISH);
        for (Detail item : inv.details) {
            if (items.isEmpty()) throw new IllegalStateException("Item Record Not found");
            Map<String, Object> ig = items.get(item.ItemId);
            if (ig == null) throw new IllegalStateException("Item Record Not found");
            if (item.ActionTypeId == 3) continue;
            String itemName = s(col(ig, "ItemName"));
            int saleGl = i(col(ig, "SaleGLAC"));
            String text = "";
            if (doc == 1856 || doc == 1861 || doc == 1862) {
                if (!s(h.RemarksHeader).isEmpty()) text = h.RemarksHeader;
                text = text + (doc == 1862 ? "  Sale Return " : "  Sale ") + r0(item.ItemQty) + " Qty ( " + r0(item.VarientEquivalent);
                text = text + "  " + r0(item.ItemNetWeight) + ")  of " + itemName + " @Rate. " + r0(item.ItemRate) + "   = Rs. " + r0(item.ItemAmount);
                if (item.ItemDiscountAmount.signum() > 0) text = text + " Discount " + r3(item.ItemDiscountAmount);
                if (h.PaymentTermsId == 2) text = text + " Term " + h.DueDays + " days Credit DueDate " + h.DueDate.format(dd);
                else if (h.PaymentTermsId == 1) text += " Term Cash";
                if (item.RateCut.signum() != 0) text = text + " RateCut " + r0(item.RateCut) + " RateCutAmount " + r0(item.RateCutAmount);
                if (item.VehicleNo != null && !item.VehicleNo.isEmpty()) text = text + " Vehicle # " + item.VehicleNo;
                if (item.GpNo != 0) text = text + " Gp # " + item.GpNo;
                if (item.ExpenseAmount.signum() > 0) text = text + " Expense Amount " + r0(item.ExpenseAmount);
                if (item.SaleOrderNo > 0) text = text + " Under contract # " + item.SaleOrderNo;
                if (!s(h.ManualBillNo).isEmpty()) text = text + " Manual # " + h.ManualBillNo;
                text = text + " from " + inv.companyName;
            }
            if (!item.IsFOC) {
                if (doc != 1862) {
                    VoucherDetail a = itemRow(item, text, inv);
                    a.AccountId = inv.supplierGlAccountId; a.AgainstAccountId = saleGl; a.DebitAmount = dbl(item.ItemAmountWithDisc);
                    a.QtyOut = dbl(item.ItemQty); a.WeightOut = dbl(item.ItemNetWeight); a.ItemRate = dbl(item.ItemRate);
                    a.SubsidiaryTypeId = 1; a.SubsidiaryAccountId = h.SupplierCustomerId; a.SupplierCustomerId = h.SupplierCustomerId;
                    out.add(a);
                    VoucherDetail b = itemRow(item, text, inv);
                    b.AccountId = saleGl; b.AgainstAccountId = inv.supplierGlAccountId; b.CreditAmount = dbl(item.ItemAmountWithDisc);
                    b.QtyOut = dbl(item.ItemQty); b.WeightOut = dbl(item.ItemNetWeight); b.ItemRate = dbl(item.ItemRate);
                    b.SubsidiaryAgainstTypeId = 1; b.SubsidiaryAgainstAccountId = h.SupplierCustomerId;
                    out.add(b);
                } else {
                    VoucherDetail a = itemRow(item, text, inv);
                    a.AccountId = saleGl; a.AgainstAccountId = inv.supplierGlAccountId; a.DebitAmount = dbl(item.ItemAmountWithDisc);
                    a.QtyIn = dbl(item.ItemQty); a.WeightIn = dbl(item.ItemNetWeight); a.ItemRate = dbl(item.ItemRate);
                    a.SubsidiaryAgainstTypeId = 1; a.SubsidiaryAgainstAccountId = h.SupplierCustomerId;
                    out.add(a);
                    VoucherDetail b = itemRow(item, text, inv);
                    b.AccountId = inv.supplierGlAccountId; b.AgainstAccountId = saleGl; b.CreditAmount = dbl(item.ItemAmountWithDisc);
                    b.QtyIn = dbl(item.ItemQty); b.WeightIn = dbl(item.ItemNetWeight); b.ItemRate = dbl(item.ItemRate);
                    b.SubsidiaryTypeId = 1; b.SubsidiaryAccountId = h.SupplierCustomerId; b.SupplierCustomerId = h.SupplierCustomerId;
                    out.add(b);
                }
            }
            if ((doc == 1856 || doc == 1861) && !truthy(config("CreditAmountInItemSaleGL")) && item.ExpenseAmount.signum() > 0) {
                VoucherDetail a = new VoucherDetail();
                a.AccountId = inv.supplierGlAccountId; a.AgainstAccountId = saleGl; a.DebitAmount = dbl(item.ExpenseAmount); a.Comments = text; a.JobLotId = item.JobLotId;
                a.SubsidiaryTypeId = 1; a.SubsidiaryAccountId = h.SupplierCustomerId; a.SupplierCustomerId = h.SupplierCustomerId;
                out.add(a);
                VoucherDetail b = new VoucherDetail();
                b.AccountId = saleGl; b.AgainstAccountId = inv.supplierGlAccountId; b.CreditAmount = dbl(item.ExpenseAmount); b.Comments = text; b.JobLotId = item.JobLotId;
                b.SubsidiaryAgainstTypeId = 1; b.SubsidiaryAgainstAccountId = h.SupplierCustomerId;
                out.add(b);
            }
        }
        return out;
    }

    /** The properties every item row sets (RateCut ... DCurrencyAmount). */
    private static VoucherDetail itemRow(Detail item, String text, Invoice inv) {
        VoucherDetail v = new VoucherDetail();
        v.LineId = 0;
        v.Comments = text;
        v.ItemId = item.ItemId;
        v.RateCut = dbl(item.RateCut);
        v.RateCutAmount = dbl(item.RateCutAmount);
        v.ItemAmount = dbl(item.ItemAmountWithDisc);
        v.Expenses = dbl(item.ExpenseAmount);
        v.Freight = dbl(item.FreightAmount);
        v.Commission = dbl(item.CommissionAmount);
        v.OrderNo = item.SaleOrderNo;
        v.GpNo = item.GpNo;
        v.VehicleNo = item.VehicleNo;
        v.JobLotId = item.JobLotId;
        v.DMultiCurrencyId = item.CurrencyId;
        v.DExchangeCurrencyRate = dbl(item.ExchangeRate);
        v.DCurrencyAmount = dbl(item.FcyAmount);
        return v;
    }

    // ------------------------------------------------------------------ CGSTransactions

    private List<VoucherDetail> cgsTransactions(Invoice inv, Map<Integer, Map<String, Object>> items) {
        List<VoucherDetail> out = new ArrayList<>();
        Head h = inv.h;
        final int doc = h.DocumentTypeId;
        if (doc == 1856 || doc == 1861) {
            if (inv.details.isEmpty()) return out;
            for (Detail d : inv.details) {                                              // settle discount of the line (JournalAmount)
                if (items.isEmpty()) throw new IllegalStateException("Item Record Not found");
                Map<String, Object> ig = items.get(d.ItemId);
                if (ig == null) throw new IllegalStateException("Item Record Not found");
                if (d.ActionTypeId != 3 && d.JournalAmount.signum() > 0 && h.SettlementDiscountAccountId == 0) {
                    VoucherDetail a = new VoucherDetail();
                    a.AccountId = i(col(ig, "COGSGLAC")); a.AgainstAccountId = inv.supplierGlAccountId;
                    a.Comments = "Settle Discount " + s(col(ig, "ItemName")); a.DebitAmount = dbl(d.JournalAmount);
                    out.add(a);
                }
            }
            for (Detail d : inv.details) {                                              // CGS at the average rate (CGSEntryAllow + feature 1)
                if (items.isEmpty()) throw new IllegalStateException("Item Record Not found");
                Map<String, Object> ig = items.get(d.ItemId);
                if (ig == null) throw new IllegalStateException("Item Record Not found");
                if (d.ActionTypeId == 3) continue;
                if (truthy(config("CGSEntryAllow")) && feature(1)) {
                    double rate = repo.avgRateOnlyForCgs(org, company, d.ItemId, h.DocDate, h.Id > 0 ? doc : 0, h.Id > 0 ? h.Id : 0, d.JobLotId, null, d.WarehouseId);
                    if (rate <= 0) throw new IllegalStateException("CGS Rate not found");
                    BigDecimal amount = d.ItemQty.multiply(d.VarientEquivalent).multiply(BigDecimal.valueOf(rate));
                    String itemName = s(col(ig, "ItemName"));
                    String text = "Qty: " + plain(d.ItemQty) + "   " + itemName + " Varient: " + plain(d.VarientEquivalent) + "  Rate:" + com.mst.services.SaleInvoiceFinancialDirect.g(rate);
                    String comment = text + "  " + inv.companyName;
                    for (int k = 0; k < 2; k++) {
                        VoucherDetail v = new VoucherDetail();
                        v.LineId = d.LineId;
                        v.AccountId = k == 0 ? i(col(ig, "COGSGLAC")) : i(col(ig, "PurchaseGLAC"));
                        v.AgainstAccountId = k == 0 ? i(col(ig, "PurchaseGLAC")) : i(col(ig, "COGSGLAC"));
                        v.Comments = comment;
                        if (k == 0) v.DebitAmount = amount.doubleValue(); else v.CreditAmount = amount.doubleValue();
                        v.ItemId = d.ItemId; v.QtyOut = dbl(d.ItemQty); v.WeightOut = dbl(d.ItemNetWeight); v.ItemCgsRate = rate;
                        v.RateCut = dbl(d.RateCut); v.RateCutAmount = dbl(d.RateCutAmount); v.ItemAmount = amount.doubleValue();
                        v.Expenses = dbl(d.ExpenseAmount); v.Commission = dbl(d.CommissionAmount); v.Freight = dbl(d.FreightAmount);
                        v.OrderNo = d.SaleOrderNo; v.GpNo = d.GpNo; v.VehicleNo = d.VehicleNo; v.JobLotId = d.JobLotId;
                        v.SupplierCustomerId = h.SupplierCustomerId; v.IsCGS = 1;
                        out.add(v);
                    }
                }
            }
            return out;
        }
        if (doc == 1862) {
            boolean f5 = feature(5);
            if (truthy(config("CGSEntryAllow")) || f5) {
                for (Detail d : inv.details) {
                    if (items.isEmpty()) throw new IllegalStateException("Item GlAccount Not found");
                    Map<String, Object> ig = items.get(d.ItemId);
                    if (ig == null) throw new IllegalStateException("Item GlAccount Not found");
                    d.ItemName = s(col(ig, "ItemName"));
                    LinkedHashMap<String, Object> p = params("OrganizationId", org, "CompanyId", company, "ItemId", d.ItemId, "DocDate", h.DocDate);
                    if (h.Id > 0) { p.put("RecId", (long) h.Id); p.put("DocumentTypeId", doc); }
                    var rate = repo.proc("pcc.usp_getRatefromLastPurchase", p);
                    if (rate != null && !rate.isEmpty()) d.CgsRate = dec(col(rate.get(0), "CgsRate"));
                    if (d.CgsRate.signum() <= 0) throw new IllegalStateException("CGS Rate not found against " + d.ItemName + " and row qty is " + plain(d.ItemQty));
                    String text = "Bags: " + plain(d.ItemQty) + "   " + d.ItemName + " Rate: " + plain(d.CgsRate);
                    BigDecimal amount = d.ItemQty.multiply(d.VarientEquivalent).multiply(d.CgsRate);
                    String comment = text + "  " + inv.companyName;
                    for (int k = 0; k < 2; k++) {
                        VoucherDetail v = new VoucherDetail();
                        v.AccountId = k == 0 ? i(col(ig, "PurchaseGLAC")) : i(col(ig, "COGSGLAC"));
                        v.AgainstAccountId = k == 0 ? i(col(ig, "COGSGLAC")) : i(col(ig, "PurchaseGLAC"));
                        v.Comments = comment;
                        if (k == 0) v.DebitAmount = amount.doubleValue(); else v.CreditAmount = amount.doubleValue();
                        v.ItemId = d.ItemId; v.QtyIn = dbl(d.ItemQty); v.WeightIn = dbl(d.ItemWeight); v.ItemCgsRate = dbl(d.CgsRate);
                        v.ItemAmount = amount.doubleValue(); v.Commission = dbl(d.CommissionAmount); v.GpNo = d.GpNo; v.VehicleNo = d.VehicleNo;
                        v.JobLotId = d.JobLotId; v.SupplierCustomerId = h.SupplierCustomerId;
                        out.add(v);
                    }
                }
            }
        }
        return out;
    }

    private static String plain(BigDecimal v) { return v.toPlainString(); }

    // ------------------------------------------------------------------ OtherExpenseFinancial / ContractWagesFinancial

    private List<VoucherDetail> otherExpense(Invoice inv, List<Map<String, Object>> otherItems) {
        List<VoucherDetail> out = new ArrayList<>();
        Head h = inv.h;
        if ((h.DocumentTypeId == 1856 || h.DocumentTypeId == 1861) && !inv.expenses.isEmpty() && truthy(config("CreditAmountInItemSaleGL"))) {
            for (Expense e : inv.expenses) {
                if (e.Amount.signum() <= 0) continue;
                if (otherItems.isEmpty()) throw new IllegalStateException("Other Items not found");
                Map<String, Object> it = null;
                for (Map<String, Object> o : otherItems) if (i(col(o, "Id")) == e.InvOtherItemId) { it = o; break; }
                if (it == null) throw new IllegalStateException("Other Items Sale GL Account not found");
                String text = s(col(it, "OtherItemName")) + " " + s(e.Remarks);
                VoucherDetail a = new VoucherDetail();
                a.AccountId = inv.supplierGlAccountId; a.AgainstAccountId = i(col(it, "SaleGLAcId")); a.Comments = text; a.DebitAmount = dbl(e.Amount);
                a.SubsidiaryTypeId = 1; a.SubsidiaryAccountId = h.SupplierCustomerId; a.SupplierCustomerId = h.SupplierCustomerId;
                out.add(a);
                VoucherDetail b = new VoucherDetail();
                b.AccountId = i(col(it, "SaleGLAcId")); b.AgainstAccountId = inv.supplierGlAccountId; b.Comments = text; b.CreditAmount = dbl(e.Amount);
                b.SubsidiaryAgainstTypeId = 1; b.SubsidiaryAgainstAccountId = h.SupplierCustomerId;
                out.add(b);
            }
        }
        return out;
    }

    private List<VoucherDetail> contractWages(Invoice inv, Map<Integer, Map<String, Object>> parties) {
        List<VoucherDetail> out = new ArrayList<>();
        Head h = inv.h;
        if (h.DocumentTypeId == 1856 && !inv.wages.isEmpty()) {
            for (Wages w : inv.wages) {
                if (w.NetAmount.signum() <= 0) continue;
                if (parties.isEmpty()) throw new IllegalStateException("Contractor record not found");
                Map<String, Object> c = parties.get(w.ContractorId);
                if (c == null) throw new IllegalStateException("Contractor GlAccountId not found");
                List<Map<String, Object>> act = w.WagesTypeId != 2
                        ? repo.q("EXEC dbo.USP_GetActivityGlIdAndName @Id=?", w.ContractorWagesRateScheduleId)
                        : repo.q("EXEC [dbo].[USP_GetActivityGlIdAndNameForExtra] @Id=?", w.ContractorWagesRateScheduleId);
                if (act == null || act.isEmpty()) throw new IllegalStateException("Activity Record Not Found Against ScheduleId " + w.ContractorWagesRateScheduleId);
                int actGl = i(col(act.get(0), "GlAccountId"));
                int contractorGl = i(col(c, "GlAccountId"));
                VoucherDetail a = new VoucherDetail();
                a.AccountId = actGl; a.AgainstAccountId = contractorGl; a.Comments = w.Remarks; a.DebitAmount = dbl(w.NetAmount);
                a.SubsidiaryAgainstTypeId = 1; a.SubsidiaryAgainstAccountId = w.ContractorId;
                out.add(a);
                VoucherDetail b = new VoucherDetail();
                b.AccountId = contractorGl; b.AgainstAccountId = actGl; b.Comments = s(w.Remarks); b.CreditAmount = dbl(w.NetAmount);
                b.SubsidiaryTypeId = 1; b.SubsidiaryAccountId = w.ContractorId; b.SupplierCustomerId = w.ContractorId;
                out.add(b);
            }
        }
        return out;
    }
}
