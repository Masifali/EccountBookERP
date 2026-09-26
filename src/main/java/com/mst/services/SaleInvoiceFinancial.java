package com.mst.services;

import com.mst.models.saleinvoice.SaleInvoiceModels.*;
import com.mst.repositories.SaleInvoiceRepository;

import java.math.BigDecimal;
import java.math.MathContext;
import java.math.RoundingMode;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.*;

import static com.mst.repositories.SaleInvoiceRepository.*;

/**
 * BLL 0612 SaleInvoiceFinancial.MakeVoucherForSaleInvoice and BLL 0613 SaleInvoiceGeneralFinancialMethods,
 * line for line. Every document-type set, every account choice and every comment string is copied from
 * the recovered C#; nothing is simplified. Doubles are formatted the way .NET Framework's
 * double.ToString() does ("G", 15 significant digits) so the voucher comments match the desktop's.
 *
 * The only branch not ported is GetAvgRatesAndStockInHand.GetAvgRateQtyAndStockInHand, reached only by
 * document types 103, 126, 133 and 145; it throws rather than posting a guessed rate.
 */
public class SaleInvoiceFinancial {

    /** Architecture.Model.Inventory.CommonIdsForFinancials - the fields this path reads. */
    public static class CommonIds {
        public int SupplierGlAccountId, CommissionGlAccountId, OffsetAccountId, StockPartyGlAccountId, CostCenterId, SaleGLAccountId;
        public String CompanyName = "";
    }

    /** The sale invoice as the BLL sees it: header, child lists and CommonIdsForFinancials. */
    public static class Invoice {
        public Head h;
        public List<Detail> details = new ArrayList<>();
        public List<Freight> freights = new ArrayList<>();
        public List<Journal> journals = new ArrayList<>();
        public List<Expense> expenses = new ArrayList<>();
        public List<Commission> commissions = new ArrayList<>();
        public List<PaymentTerm> paymentTerms = new ArrayList<>();
        public CommonIds ids = new CommonIds();
    }

    /** VoucherHead with its detail list (the model's virtual voucherDetailList). */
    public static class Voucher {
        public VoucherHead head = new VoucherHead();
        public List<VoucherDetail> details = new ArrayList<>();
    }

    private final SaleInvoiceRepository repo;
    public SaleInvoiceFinancial(SaleInvoiceRepository repo) { this.repo = repo; }

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

    // =========================================================================== 0612 MakeVoucherForSaleInvoice

    public Voucher makeVoucherForSaleInvoice(Invoice obj) {
        Head h = obj.h;
        org = h.OrganizationId; company = h.CompanyId;
        Voucher v = new Voucher();
        VoucherHead vh = v.head;
        vh.DocumentTypeId = h.DocumentTypeId;
        vh.DocumentTypeSrNo = h.Id;
        vh.RefDocNoId = h.Id;
        vh.VoucherCode = h.DocNo;
        vh.VoucherDate = h.DocDate;
        vh.Remarks = s(h.RemarksHeader);
        vh.RemarksOtherLingo = "";
        vh.VoucherAmount = h.BillAmount;
        vh.ChequeDate = LocalDate.now().atStartOfDay();
        vh.IncludeWHT = false;
        vh.BranchId = h.BranchesId;
        vh.ProjectId = h.ProjectsId;
        vh.BillAmount = h.BillAmount;
        vh.ManualBillNo = s(h.ManualBillNo);
        vh.DueDate = h.DueDate;
        vh.DueDays = h.DueDays;
        vh.MultiCurrencyId = h.CurrencyId;
        vh.ExchangeCurrencyRate = h.ExchangeRate.doubleValue();
        vh.FcAmount = h.FcyAmount.doubleValue();
        vh.OrganizationId = h.OrganizationId;
        vh.CompanyId = h.CompanyId;
        vh.FinancialYearId = h.FinancialYearId;
        vh.EntryUser = h.EntryUser;
        vh.EntryDate = LocalDateTime.now();
        vh.ModifyDate = LocalDateTime.now();
        vh.ModifyUser = h.ModifyUser;

        String stockAgainst = config("StockAgainstAccount");
        if (stockAgainst.isEmpty()) throw new IllegalStateException("Stock AgainstAc Configuration Not Found");
        int num = i(stockAgainst);
        Map<Integer, Map<String, Object>> items = repo.itemGl(org, company);
        List<Map<String, Object>> otherItems = repo.otherItems(org, company);
        Map<Integer, Map<String, Object>> parties = repo.partyGl(org, company);

        if (h.CommAmount > 0.0 && h.CommissionAgentId > 0) {
            Map<String, Object> agent = parties.get(h.CommissionAgentId);
            if (agent == null) throw new IllegalStateException("Commission Agent GLAccountId not Found");
            vh.AgainstAccountId = i(col(agent, "GlAccountId"));
        }
        Map<String, Object> party = parties.get(h.SupplierCustomerId);
        if (party == null) throw new IllegalStateException("Party GLAccountId not Found");
        vh.RefAccountId = i(col(party, "GlAccountId"));
        String text = s(col(party, "CompanyName"));
        int stockPartyGl = 0;
        if (h.DocumentTypeId == 222) {
            Map<String, Object> sp = parties.get(h.StockPartyId);
            if (sp != null) stockPartyGl = i(col(sp, "GlAccountId"));
        }
        int costCenterId = 0;
        if (h.DocumentTypeId == 95) costCenterId = obj.details.stream().mapToInt(x -> x.CostCenterId).max().orElseThrow();
        boolean foc = false, zakat = false;
        if (h.DocumentTypeId == 99) { foc = h.OtherCategoryId == 55; zakat = h.OtherCategoryId == 56; }
        CommonIds ids = new CommonIds();
        ids.SupplierGlAccountId = vh.RefAccountId;
        ids.CommissionGlAccountId = vh.AgainstAccountId;
        ids.OffsetAccountId = num;
        ids.CompanyName = text;
        ids.StockPartyGlAccountId = stockPartyGl;
        ids.CostCenterId = costCenterId;
        obj.ids = ids;

        v.details.addAll(itemAndCustomerByWeightFinancial(obj, items, parties));
        if (!foc && !zakat) v.details.addAll(freightFinancial(obj));
        if (!foc && !zakat) v.details.addAll(supplierAddLessFinancial(obj));
        v.details.addAll(otherExpenseFinancial(obj, otherItems));
        if (!foc && !zakat) v.details.addAll(salesTaxFinancial(obj));

        vh.Remarks = (h.DocumentTypeId == 1608 || h.DocumentTypeId == 1609 || h.DocumentTypeId == 1611) ? h.OtherRemarks : h.RemarksHeader;
        return v;
    }

    // =========================================================================== 0613 SupplierAddLessFinancial

    List<VoucherDetail> supplierAddLessFinancial(Invoice obj) {
        Head h = obj.h;
        int ref = obj.ids.SupplierGlAccountId;
        List<VoucherDetail> out = new ArrayList<>();
        if (Set.of(95, 96, 99, 103, 126, 133, 171, 208, 1608, 1609, 1661, 1660, 1611, 1662, 1809, 145, 222).contains(h.DocumentTypeId)
                && !obj.journals.isEmpty()) {
            for (Journal j : obj.journals) {
                int acc = j.ChartofAccountId;
                if (acc == 0) continue;
                VoucherDetail a = new VoucherDetail();
                a.AccountId = acc; a.AgainstAccountId = ref; a.Comments = s(j.JvRemarks);
                a.DebitAmount = j.JvDebit; a.CreditAmount = j.JvCredit;
                a.SubsidiaryTypeId = 1; a.SubsidiaryAccountId = j.TransporterSupCustId; a.SupplierCustomerId = j.TransporterSupCustId;
                a.SubsidiaryAgainstTypeId = 1; a.SubsidiaryAgainstAccountId = h.SupplierCustomerId; a.BranchesId = h.BranchesId;
                out.add(a);
                VoucherDetail b = new VoucherDetail();
                b.AccountId = ref; b.AgainstAccountId = acc; b.Comments = s(j.JvRemarks);
                b.CreditAmount = j.JvDebit; b.DebitAmount = j.JvCredit;
                b.SubsidiaryTypeId = 1; b.SubsidiaryAccountId = h.SupplierCustomerId; b.SupplierCustomerId = h.SupplierCustomerId;
                b.SubsidiaryAgainstTypeId = 1; b.SubsidiaryAgainstAccountId = j.TransporterSupCustId; b.BranchesId = h.BranchesId;
                b.CostCenterId = obj.ids.CostCenterId;
                out.add(b);
            }
        }
        return out;
    }

    // =========================================================================== 0613 SalesTaxFinancial

    List<VoucherDetail> salesTaxFinancial(Invoice obj) {
        Head h = obj.h;
        int ref = obj.ids.SupplierGlAccountId;
        List<VoucherDetail> out = new ArrayList<>();
        if (!Set.of(96, 103, 126, 133, 1608, 1609, 1661, 1660, 1611, 145, 1662).contains(h.DocumentTypeId) || obj.details.isEmpty()) return out;
        double num = 0.0;
        for (Detail d : obj.details) num += d.TaxAmount;
        if (!(num > 0.0)) return out;
        if (h.DocumentTypeId == 1611 || h.DocumentTypeId == 1662) {
            VoucherDetail a = new VoucherDetail();
            a.AccountId = h.TaxAccountId; a.AgainstAccountId = ref; a.Comments = "Tax %  " + g(num); a.DebitAmount = num;
            a.TaxesTotalAmount = num; a.IsTaxable = "True"; a.TaxTypeId = 2; a.SubsidiaryAgainstTypeId = 1;
            a.SubsidiaryAgainstAccountId = h.SupplierCustomerId; a.BranchesId = h.BranchesId;
            out.add(a);
            VoucherDetail b = new VoucherDetail();
            b.AccountId = ref; b.AgainstAccountId = h.TaxAccountId; b.Comments = "Tax %  " + g(num); b.CreditAmount = num;
            b.TaxesTotalAmount = num; b.TaxPrcnt = 17.0; b.IsTaxable = "True"; b.TaxTypeId = 2; b.SubsidiaryTypeId = 1;
            b.SubsidiaryAccountId = h.SupplierCustomerId; b.SupplierCustomerId = h.SupplierCustomerId; b.BranchesId = h.BranchesId;
            out.add(b);
            return out;
        }
        boolean taxAcc = Set.of(1608, 1609, 1661, 1660, 145).contains(h.DocumentTypeId);
        VoucherDetail a = new VoucherDetail();
        a.AccountId = ref; a.Comments = "Tax " + g(num);
        a.AgainstAccountId = taxAcc ? h.TaxAccountId : h.ReferencePartyId;
        a.DebitAmount = num; a.TaxesTotalAmount = num; a.IsTaxable = "True"; a.TaxTypeId = 2; a.SubsidiaryTypeId = 1;
        a.SubsidiaryAccountId = h.SupplierCustomerId; a.SupplierCustomerId = h.SupplierCustomerId; a.BranchesId = h.BranchesId;
        a.CostCenterId = obj.ids.CostCenterId;
        out.add(a);
        VoucherDetail b = new VoucherDetail();
        b.AccountId = taxAcc ? h.TaxAccountId : h.ReferencePartyId;
        b.Comments = "Tax " + g(num); b.AgainstAccountId = ref; b.CreditAmount = num; b.TaxesTotalAmount = num;
        b.IsTaxable = "True"; b.TaxTypeId = 2; b.SubsidiaryAgainstTypeId = 1; b.SubsidiaryAgainstAccountId = h.SupplierCustomerId;
        b.BranchesId = h.BranchesId;
        out.add(b);
        return out;
    }

    // =========================================================================== 0613 FreightFinancial

    List<VoucherDetail> freightFinancial(Invoice obj) {
        Head h = obj.h;
        int ref = obj.ids.SupplierGlAccountId;
        List<VoucherDetail> out = new ArrayList<>();
        if (Set.of(95, 96, 103, 126, 133, 171, 1608, 1609, 1661, 1660, 1611, 145, 99, 222).contains(h.DocumentTypeId) && !obj.freights.isEmpty()) {
            for (Freight f : obj.freights) {
                if (f.TansporterId <= 0) continue;
                VoucherDetail x = new VoucherDetail();
                boolean own = f.TansporterId == ref;
                x.AccountId = f.TansporterId;
                x.AgainstAccountId = obj.ids.SaleGLAccountId > 0 ? obj.ids.SaleGLAccountId : f.TansporterId;
                x.Comments = !isBlank(f.Remarks) ? f.Remarks : "Freight";
                x.DebitAmount = f.Debit; x.CreditAmount = f.FreightAmount;
                x.SubsidiaryTypeId = 1;
                x.SubsidiaryAccountId = own ? h.SupplierCustomerId : f.TransporterSupCustId;
                x.SupplierCustomerId = own ? h.SupplierCustomerId : f.TransporterSupCustId;
                x.CostCenterId = own ? obj.ids.CostCenterId : 0;
                x.BranchesId = h.BranchesId;
                out.add(x);
            }
        }
        if (h.FreightAmount > 0.0) {
            if (h.DocumentTypeId == 99 || h.DocumentTypeId == 222) {
                boolean own = h.TransporterId == ref;
                VoucherDetail a = new VoucherDetail();
                a.AccountId = h.TransporterId; a.AgainstAccountId = ref; a.Comments = "Freight"; a.DebitAmount = h.FreightAmount; a.CreditAmount = 0.0;
                a.SubsidiaryTypeId = 1;
                a.SubsidiaryAccountId = own ? h.SupplierCustomerId : h.TransporterCreditPartyId;
                a.SupplierCustomerId = own ? h.SupplierCustomerId : h.TransporterCreditPartyId;
                a.SubsidiaryAgainstTypeId = 1; a.SubsidiaryAgainstAccountId = h.SupplierCustomerId;
                a.CostCenterId = own ? obj.ids.CostCenterId : 0; a.BranchesId = h.BranchesId;
                out.add(a);
                VoucherDetail b = new VoucherDetail();
                b.AccountId = ref; b.AgainstAccountId = h.TransporterId; b.Comments = "Freight"; b.DebitAmount = 0.0; b.CreditAmount = h.FreightAmount;
                b.SubsidiaryTypeId = 1; b.SubsidiaryAccountId = h.SupplierCustomerId; b.SupplierCustomerId = h.SupplierCustomerId;
                b.SubsidiaryAgainstTypeId = 1; b.SubsidiaryAgainstAccountId = h.TransporterCreditPartyId; b.BranchesId = h.BranchesId;
                out.add(b);
            }
            if (Set.of(1609, 1608, 1661, 1660, 1662).contains(h.DocumentTypeId)) {
                String remark = !isBlank(h.FreightRemark) ? h.FreightRemark : "Freight";
                if (h.TransporterDebitGLId > 0) {
                    VoucherDetail a = new VoucherDetail();
                    a.AccountId = h.TransporterDebitGLId; a.AgainstAccountId = h.TransporterId; a.Comments = remark; a.DebitAmount = h.FreightAmount;
                    a.SubsidiaryTypeId = 1; a.SubsidiaryAccountId = h.TransporterDebitPartyId; a.SupplierCustomerId = h.TransporterDebitPartyId;
                    a.SubsidiaryAgainstTypeId = 1; a.SubsidiaryAgainstAccountId = h.TransporterCreditPartyId; a.BranchesId = h.BranchesId;
                    out.add(a);
                }
                VoucherDetail b = new VoucherDetail();
                b.AccountId = h.TransporterId; b.AgainstAccountId = h.TransporterDebitGLId; b.Comments = remark; b.DebitAmount = 0.0; b.CreditAmount = h.FreightAmount;
                b.SubsidiaryTypeId = 1; b.SubsidiaryAccountId = h.TransporterCreditPartyId; b.SupplierCustomerId = h.TransporterCreditPartyId;
                b.SubsidiaryAgainstTypeId = 1; b.SubsidiaryAgainstAccountId = h.TransporterDebitPartyId; b.BranchesId = h.BranchesId;
                out.add(b);
            }
        }
        return out;
    }

    // =========================================================================== 0613 ItemAndCustomerByWeightFinancial

    List<VoucherDetail> itemAndCustomerByWeightFinancial(Invoice obj, Map<Integer, Map<String, Object>> dtitem,
                                                         Map<Integer, Map<String, Object>> dtSupplier) {
        Head h = obj.h;
        int ref = obj.ids.SupplierGlAccountId;
        List<VoucherDetail> out = new ArrayList<>();
        boolean flag = truthy(config("DebitAmountChargetoExpenseAcOfCommission"));
        int num = i(config("DiscountAllowed"));
        boolean flag2 = false, flag3 = false;
        if (h.DocumentTypeId == 99) { flag2 = h.OtherCategoryId == 55; flag3 = h.OtherCategoryId == 56; }
        Set<Integer> hashSet = Set.of(95, 96, 99, 103, 60, 126, 128, 133, 139, 171, 208, 1608, 1609, 1611, 184, 186, 1809, 145, 222, 1661, 1660, 1662);
        Set<Integer> hashSet2 = Set.of(1611, 1662);
        boolean flag4 = Set.of(103, 126, 133).contains(h.DocumentTypeId);
        Set<Integer> contractDocs = Set.of(1608, 1609, 1611, 1661, 1660);
        if (!hashSet.contains(h.DocumentTypeId) || obj.details.isEmpty()) return out;

        /* The JSON-cloned, item/branch-grouped list only feeds the 1608-family remark text. */
        if (contractDocs.contains(h.DocumentTypeId)) {
            LinkedHashMap<String, double[]> grouped = new LinkedHashMap<>();
            LinkedHashMap<String, Detail> firsts = new LinkedHashMap<>();
            for (Detail d : obj.details) {
                String key = d.ItemId + "|" + d.BranchId;
                firsts.putIfAbsent(key, d);
                grouped.computeIfAbsent(key, k -> new double[1])[0] += d.ItemQty;
            }
            StringBuilder text = new StringBuilder();
            int num12 = 0; LocalDateTime date = LocalDateTime.now();
            for (var e : firsts.entrySet()) {
                Detail d = e.getValue();
                Map<String, Object> it = dtitem.get(d.ItemId);
                text.append(s(col(it, "ItemName"))).append("  Qty ").append(g(grouped.get(e.getKey())[0])).append("  ");
                num12 = d.SaleOrder; date = d.OrderDate;
            }
            h.OtherRemarks = h.RemarksHeader + " " + text;
            if (num12 > 0) h.OtherRemarks = h.OtherRemarks + " ContractNo " + num12 + "  " + (date == null ? "01-Jan-01" : date.format(DateTimeFormatter.ofPattern("dd-MMM-yy", Locale.ENGLISH)));
        }

        int num14 = 0;
        double freightSum = obj.freights.stream().mapToDouble(x -> x.FreightAmount).sum();
        if (freightSum > 0.0 && obj.freights.stream().map(x -> x.TansporterId).distinct().count() == 1)
            num14 = obj.freights.stream().mapToInt(x -> x.TansporterId).max().orElse(0);

        boolean flag6 = false;
        double num15 = 0.0;
        for (Detail item3 : obj.details) {
            if (item3.RefRefDocumentTypeId > 0 && item3.RefRefDocIdNo > 0 && item3.RefDocSubId > 0) flag6 = true;
            double value = item3.NetBillWeight / 40.0;
            if (dtitem.isEmpty()) throw new IllegalStateException("Item Record Not found");
            Map<String, Object> list4 = dtitem.get(item3.ItemId);
            if (list4 == null) throw new IllegalStateException("Item Record Not found");
            int purchaseGl = i(col(list4, "PurchaseGLAC")), saleGl = i(col(list4, "SaleGLAC")), cogsGl = i(col(list4, "COGSGLAC"));
            int jobLotAcc = item3.JobLotId > 0 ? repo.jobLotAccountById(item3.JobLotId) : 0;
            String itemName = s(col(list4, "ItemName"));
            List<String> list5 = new ArrayList<>();
            if (Set.of(60, 128, 184, 186, 145).contains(h.DocumentTypeId)) {
                if (h.DocumentTypeId == 186 && !isBlank(h.ManualBillNo)) list5.add(h.ManualBillNo);
                if (!isBlank(h.RemarksHeader)) list5.add(h.RemarksHeader);
                list5.add("Item: " + itemName);
                list5.add("Qty: " + g(item3.ItemQty));
                list5.add("Rate: " + g(item3.ItemRate));
                if (item3.ItemDiscountAmount > 0.0) list5.add("Discount: " + g(item3.ItemDiscountAmount));
                if (item3.ExpenseAmount > 0.0) list5.add("Expense: " + g(item3.ExpenseAmount));
                if (item3.CommissionAmount > 0.0) list5.add("Commission: " + g(item3.CommissionAmount));
            } else if (flag4) {
                if (!isBlank(h.RemarksHeader)) list5.add(h.RemarksHeader);
                list5.add("Item: " + itemName);
                list5.add("Qty: " + g(item3.ItemQty));
                list5.add("Rate: " + g(item3.ItemRate));
                if (item3.FreightAmount > 0.0) list5.add("Freight: " + g(item3.FreightAmount));
                if (item3.CommissionAmount > 0.0) list5.add("Commission: " + g(item3.CommissionAmount));
            } else if (contractDocs.contains(h.DocumentTypeId)) {
                if (item3.SaleOrder > 0) list5.add("Contract No. " + item3.SaleOrder + " " + (item3.OrderDate == null ? "01-Jan-0001" : item3.OrderDate.format(DateTimeFormatter.ofPattern("dd-MMM-yyyy", Locale.ENGLISH))));
                list5.add(itemName + " Qty " + g(item3.ItemQty) + " @Rate " + g(item3.ItemRate));
                if (item3.ItemDiscount > 0.0 && item3.ItemDiscountAmount > 0.0) list5.add("Discount " + g(Math.rint(item3.ItemDiscountAmount)));
                list5.add("NetAmount " + g(item3.ItemAmount));
                if (h.PaymentTermId == 2) list5.add("Term " + h.DueDays + " days Credit DueDate " + (h.DueDate == null ? "" : h.DueDate.format(DateTimeFormatter.ofPattern("dd-MMM-yyyy", Locale.ENGLISH))));
                else if (h.PaymentTermId == 1) list5.add("Term Cash");
                if (item3.GpNo != 0) list5.add("Gp # " + item3.GpNo);
                if (!isBlank(item3.VehicleNo) && !"0".equals(item3.VehicleNo)) list5.add("Vehicle # " + item3.VehicleNo);
                if (!isBlank(item3.VariantDescription) && !"0".equals(item3.VariantDescription)) list5.add("Modal Description # " + item3.VariantDescription);
            } else {
                if (!isBlank(h.RemarksHeader)) list5.add(h.RemarksHeader);
                String text2 = "Sale " + g(item3.ItemQty) + " Bags (" + g(item3.GrossWeight);
                if (item3.EBTotalWt > 0.0) text2 += ", " + g(round(item3.EBTotalWt, 3));
                text2 += ", " + g(item3.NetBillWeight) + ")";
                list5.add(text2);
                list5.add("of " + itemName);
                list5.add(g(round(value, 2)) + " Mound");
                list5.add("@ NetRate " + g(item3.ItemRate));
                list5.add("= Rs. " + g(item3.ItemAmount));
                if (item3.ItemDiscount > 0.0 && item3.ItemDiscountAmount > 0.0) list5.add("Discount% " + g(item3.ItemDiscount) + " Amount " + g(Math.rint(item3.ItemDiscountAmount)));
                if (item3.RateCut != 0.0) list5.add("RateCut " + g(item3.RateCut) + " Amount " + g(Math.rint(item3.RateCutAmount)));
                if (item3.CommissionAmount > 0.0) list5.add("Commission @ " + g(h.CommRate) + " = Rs. " + g(Math.rint(item3.CommissionAmount)));
                if (item3.FreightAmount > 0.0) list5.add("Freight = Rs. " + g(Math.rint(item3.FreightAmount)));
                if (!isBlank(item3.VehicleNo) && !"0".equals(item3.VehicleNo)) list5.add("Vehicle # " + item3.VehicleNo);
                if (item3.GpNo != 0) list5.add("Gp # " + item3.GpNo);
                if (item3.ExpenseAmount > 0.0) list5.add("Expense = Rs. " + g(Math.rint(item3.ExpenseAmount)));
                if (item3.SaleOrder > 0) list5.add("Contract # " + item3.SaleOrder);
                if (!isBlank(h.ManualBillNo)) list5.add("Manual # " + h.ManualBillNo);
                if (!isBlank(item3.WareHouseName)) list5.add("Warehouse " + item3.WareHouseName);
                if (!isBlank(item3.VariantDescription) && !"0".equals(item3.VariantDescription)) list5.add("Modal Description # " + item3.VariantDescription);
                list5.add("From " + obj.ids.CompanyName);
            }
            String empty = String.join(", ", list5);
            if (!hashSet2.contains(h.DocumentTypeId)) {
                if (!flag2 && !flag3) {
                    VoucherDetail a = new VoucherDetail();
                    a.LineId = 0;
                    a.AccountId = ref;
                    if (h.DocumentTypeId == 60 || h.DocumentTypeId == 145) { a.LineId = item3.LineId; a.AgainstAccountId = jobLotAcc > 0 ? jobLotAcc : purchaseGl; }
                    else if (h.DocumentTypeId == 222) a.AgainstAccountId = obj.ids.StockPartyGlAccountId;
                    else a.AgainstAccountId = jobLotAcc > 0 ? jobLotAcc : saleGl;
                    a.Comments = empty;
                    a.DebitAmount = item3.ItemAmount;
                    itemLine(a, item3, true);
                    a.SubsidiaryTypeId = 1; a.SubsidiaryAccountId = h.SupplierCustomerId; a.SupplierCustomerId = h.SupplierCustomerId;
                    out.add(a);
                    VoucherDetail b = new VoucherDetail();
                    b.LineId = 0;
                    if (h.DocumentTypeId == 60 || h.DocumentTypeId == 145) { b.LineId = item3.LineId; b.AccountId = jobLotAcc > 0 ? jobLotAcc : purchaseGl; }
                    else if (h.DocumentTypeId == 222) b.AccountId = obj.ids.StockPartyGlAccountId;
                    else b.AccountId = jobLotAcc > 0 ? jobLotAcc : saleGl;
                    b.AgainstAccountId = ref;
                    b.Comments = empty + "  " + obj.ids.CompanyName;
                    b.CreditAmount = item3.ItemAmount;
                    itemLine(b, item3, true);
                    b.SubsidiaryAgainstTypeId = 1; b.SubsidiaryAgainstAccountId = h.SupplierCustomerId;
                    out.add(b);
                    num15 = b.CreditAmount;
                }
                if (h.DocumentTypeId == 222 && h.TransTypeId == 2) {
                    out.add(addVoucherDetail(purchaseGl, saleGl, item3.ItemAmount, null, false, item3, empty, h.SupplierCustomerId, obj.ids.CompanyName));
                    out.add(addVoucherDetail(saleGl, purchaseGl, null, item3.ItemAmount, false, item3, empty, h.SupplierCustomerId, obj.ids.CompanyName));
                    out.add(addVoucherDetail(cogsGl, purchaseGl, item3.ItemAmount, null, true, item3, empty, h.SupplierCustomerId, obj.ids.CompanyName));
                    out.add(addVoucherDetail(purchaseGl, cogsGl, null, item3.ItemAmount, true, item3, empty, h.SupplierCustomerId, obj.ids.CompanyName));
                }
            }
            if (hashSet2.contains(h.DocumentTypeId)) {
                VoucherDetail a = new VoucherDetail();
                a.LineId = 0; a.AccountId = jobLotAcc > 0 ? jobLotAcc : purchaseGl; a.AgainstAccountId = ref; a.Comments = empty;
                a.DebitAmount = item3.ItemAmount; a.ItemId = item3.ItemId; a.QtyIn = item3.ItemQty; a.ItemRate = item3.ItemRate; a.WeightIn = item3.NetBillWeight;
                a.ItemAmount = item3.ItemAmount; a.Expenses = item3.ExpenseAmount; a.Freight = item3.FreightAmount; a.Commission = item3.CommissionAmount;
                a.OrderNo = item3.SaleOrder; a.GpNo = item3.GpNo; a.JobLotId = item3.JobLotId; a.VehicleNo = item3.VehicleNo;
                a.DMultiCurrencyId = item3.CurrencyId; a.DExchangeCurrencyRate = item3.ExchangeRate.doubleValue(); a.DCurrencyAmount = item3.FcyAmount.doubleValue();
                a.SubsidiaryAgainstTypeId = 1; a.SubsidiaryAgainstAccountId = h.SupplierCustomerId; a.BranchesId = item3.BranchId;
                out.add(a);
                VoucherDetail b = new VoucherDetail();
                b.LineId = 0; b.AccountId = ref; b.AgainstAccountId = jobLotAcc > 0 ? jobLotAcc : purchaseGl; b.Comments = empty + "  " + obj.ids.CompanyName;
                b.CreditAmount = item3.ItemAmount; b.ItemId = item3.ItemId; b.QtyIn = item3.ItemQty; b.ItemRate = item3.ItemRate; b.WeightIn = item3.NetBillWeight;
                b.ItemAmount = item3.ItemAmount; b.Expenses = item3.ExpenseAmount; b.Commission = item3.CommissionAmount; b.Freight = item3.FreightAmount;
                b.OrderNo = item3.SaleOrder; b.GpNo = item3.GpNo; b.VehicleNo = item3.VehicleNo; b.JobLotId = item3.JobLotId;
                b.DMultiCurrencyId = item3.CurrencyId; b.DExchangeCurrencyRate = item3.ExchangeRate.doubleValue(); b.DCurrencyAmount = item3.FcyAmount.doubleValue();
                b.SubsidiaryTypeId = 1; b.SubsidiaryAccountId = h.SupplierCustomerId; b.SupplierCustomerId = h.SupplierCustomerId; b.BranchesId = item3.BranchId;
                out.add(b);
            }
            if (item3.ItemDiscountAmount > 0.0 && !Set.of(1608, 1609, 1611, 1661, 1662, 1660, 184, 186).contains(h.DocumentTypeId)) {
                if (num == 0) throw new IllegalStateException("Discount Allowed Account Not Found");
                VoucherDetail a = new VoucherDetail();
                a.LineId = 0; a.AccountId = num; a.AgainstAccountId = ref; a.Comments = empty; a.DebitAmount = item3.ItemAmount;
                itemLine(a, item3, true);
                a.SubsidiaryAgainstTypeId = 1; a.SubsidiaryAgainstAccountId = h.SupplierCustomerId;
                out.add(a);
                VoucherDetail b = new VoucherDetail();
                b.LineId = 0; b.AccountId = ref; b.AgainstAccountId = num; b.Comments = empty + "  " + obj.ids.CompanyName; b.CreditAmount = item3.ItemAmount;
                itemLine(b, item3, true);
                b.SubsidiaryTypeId = 1; b.SubsidiaryAccountId = h.SupplierCustomerId; b.SupplierCustomerId = h.SupplierCustomerId;
                out.add(b);
            }
            if (h.DocumentTypeId != 60 && h.DocumentTypeId != 1611 && h.DocumentTypeId != 1662 && h.DocumentTypeId != 222) {
                boolean flag7 = truthy(config("InventoryFinancialsEffectsInActive"));
                if (jobLotAcc == 0 && !flag7) cgsLines(obj, item3, list4, flag2, flag3, flag6, num15, out);
            }
            if (Set.of(95, 96, 103, 126, 128, 133, 171, 1608, 1609, 1611, 1661, 1660, 145, 99, 222).contains(h.DocumentTypeId)
                    && item3.FreightAmount > 0.0 && h.TransporterDebitGLId == 0 && !flag2 && !flag3) {
                List<String> l = new ArrayList<>();
                if (h.DocumentTypeId == 145) l.add("Purchase Return " + g(item3.ItemQty) + " Qty of " + itemName);
                else {
                    if (!flag4) l.add("Sale " + g(item3.ItemQty) + " Qty (" + g(item3.NetBillWeight) + ") of " + itemName);
                    if (flag4) l.add("Sale " + g(item3.ItemQty) + " of " + itemName);
                    if (!flag4) l.add(g(round(value, 2)) + " Mond");
                }
                l.add("@ NetRate " + g(item3.ItemRate));
                l.add("= Rs. " + g(item3.ItemAmount));
                l.add("Freight " + g(item3.FreightAmount));
                if (item3.SaleOrder > 0) l.add("Under contract # " + item3.SaleOrder);
                if (!isBlank(item3.VehicleNo)) l.add("Vehicle # " + item3.VehicleNo);
                if (item3.GpNo != 0) l.add("Gp # " + item3.GpNo);
                if (!isBlank(h.ManualBillNo)) l.add("Manual # " + h.ManualBillNo);
                l.add("From " + obj.ids.CompanyName);
                VoucherDetail x = new VoucherDetail();
                x.AccountId = (h.DocumentTypeId == 1611 || h.DocumentTypeId == 145) ? (jobLotAcc > 0 ? jobLotAcc : purchaseGl) : (jobLotAcc > 0 ? jobLotAcc : saleGl);
                x.AgainstAccountId = num14 > 0 ? num14 : x.AccountId;
                x.Comments = String.join(", ", l);
                x.DebitAmount = item3.FreightAmount; x.CreditAmount = 0.0; x.JobLotId = item3.JobLotId; x.BranchesId = item3.BranchId;
                x.CostCenterId = obj.ids.CostCenterId;
                out.add(x);
            }
            if (Set.of(95, 99, 103, 128, 133, 139, 171, 184, 186, 1809, 1608, 1609, 1661, 1660, 222).contains(h.DocumentTypeId)
                    && !truthy(config("CreditAmountInItemSaleGL")) && item3.ExpenseAmount > 0.0) {
                List<String> l = new ArrayList<>();
                l.add("Sale " + g(item3.ItemQty) + " Bags (" + g(item3.NetBillWeight) + ") of " + itemName);
                l.add(g(round(value, 2)) + " Mond");
                l.add("@ NetRate " + g(item3.ItemRate));
                l.add("= Rs. " + g(item3.ItemAmount));
                l.add("Other Exp " + g(item3.ExpenseAmount));
                if (item3.SaleOrder > 0) l.add("Under contract # " + item3.SaleOrder);
                if (!isBlank(item3.VehicleNo)) l.add("Vehicle # " + item3.VehicleNo);
                if (item3.GpNo != 0) l.add("Gp # " + item3.GpNo);
                if (!isBlank(h.ManualBillNo)) l.add("Manual # " + h.ManualBillNo);
                l.add("From " + obj.ids.CompanyName);
                String c3 = String.join(", ", l);
                VoucherDetail a = new VoucherDetail();
                a.AccountId = ref; a.AgainstAccountId = jobLotAcc > 0 ? jobLotAcc : saleGl; a.DebitAmount = item3.ExpenseAmount; a.Comments = c3;
                a.JobLotId = item3.JobLotId; a.SubsidiaryTypeId = 1; a.SubsidiaryAccountId = h.SupplierCustomerId; a.SupplierCustomerId = h.SupplierCustomerId;
                a.BranchesId = item3.BranchId; a.CostCenterId = item3.CostCenterId;
                out.add(a);
                VoucherDetail b = new VoucherDetail();
                b.AccountId = jobLotAcc > 0 ? jobLotAcc : saleGl; b.AgainstAccountId = ref; b.CreditAmount = item3.ExpenseAmount; b.Comments = c3;
                b.JobLotId = item3.JobLotId; b.SubsidiaryAgainstTypeId = 1; b.SubsidiaryAgainstAccountId = h.SupplierCustomerId;
                b.BranchesId = item3.BranchId; b.CostCenterId = item3.CostCenterId;
                out.add(b);
            }
            if (h.DocumentTypeId == 145 && item3.ExpenseAmount > 0.0) {
                String t3 = "Purchase Return " + itemName + " Qty " + g(item3.ItemQty) + " @ NetRate. " + g(item3.ItemRate) + "   = Rs. " + g(item3.ItemAmount);
                t3 += " Other Exp " + g(item3.ExpenseAmount);
                if (item3.SaleOrder > 0) t3 += " Under contract # " + item3.SaleOrder;
                if (!"".equals(item3.VehicleNo)) t3 += " Vehicle # " + item3.VehicleNo;
                if (item3.GpNo != 0) t3 += " Gp # " + item3.GpNo;
                if (!"".equals(h.ManualBillNo)) t3 += " Manual # " + h.ManualBillNo;
                t3 += " from " + obj.ids.CompanyName;
                VoucherDetail a = new VoucherDetail();
                a.AccountId = ref; a.AgainstAccountId = jobLotAcc > 0 ? jobLotAcc : purchaseGl; a.DebitAmount = item3.ExpenseAmount; a.Comments = t3;
                a.JobLotId = item3.JobLotId; a.SubsidiaryTypeId = 1; a.SubsidiaryAccountId = h.SupplierCustomerId; a.SupplierCustomerId = h.SupplierCustomerId;
                a.BranchesId = item3.BranchId;
                out.add(a);
                VoucherDetail b = new VoucherDetail();
                b.AccountId = jobLotAcc > 0 ? jobLotAcc : purchaseGl; b.AgainstAccountId = ref; b.CreditAmount = item3.ExpenseAmount; b.Comments = t3;
                b.JobLotId = item3.JobLotId; b.SubsidiaryAgainstTypeId = 1; b.SubsidiaryAgainstAccountId = h.SupplierCustomerId; b.BranchesId = item3.BranchId;
                out.add(b);
            }
        }

        Set<Integer> hashSet4 = Set.of(95, 99, 103, 60, 128, 126, 133, 139, 171, 1809, 222);
        if (!obj.commissions.isEmpty() && hashSet4.contains(h.DocumentTypeId) && !flag2 && !flag3) {
            int num23 = 0;
            if (!flag && obj.commissions.stream().map(x -> x.CommissionAgentId).distinct().count() == 1) num23 = obj.ids.CommissionGlAccountId;
            for (Detail item2 : obj.details) {
                double num24 = flag4 ? 0.0 : item2.NetBillWeight / 40.0;
                if (dtitem.isEmpty()) throw new IllegalStateException("Item Record Not found");
                Map<String, Object> list6 = dtitem.get(item2.ItemId);
                if (list6 == null) throw new IllegalStateException("Item Record Not found");
                int jobLotAcc2 = item2.JobLotId > 0 ? repo.jobLotAccountById(item2.JobLotId) : 0;
                if (item2.CommissionAmount > 0.0 && h.CommissionDebitAcGLId == 0 && !flag) {
                    List<String> l = new ArrayList<>();
                    String itemName5 = s(col(list6, "ItemName"));
                    if (flag4) l.add("Sale " + g(item2.ItemQty) + ") of " + itemName5);
                    else l.add("Sale " + g(item2.ItemQty) + " Bags (" + g(item2.NetBillWeight) + ") of " + itemName5);
                    if (num24 > 0.0) l.add(g(round(num24, 2)) + " Mond");
                    l.add("@ NetRate " + g(item2.ItemRate));
                    l.add("= Rs. " + g(item2.ItemAmount));
                    Commission c = obj.commissions.stream().filter(x -> x.SaleOrderId == item2.SaleOrderId).findFirst().orElse(null);
                    String text4 = "", arg = "";
                    if (c != null) {
                        int t = (int) c.CommType;
                        arg = g(c.Rate);
                        text4 = t == 1 ? "Flat" : t == 2 ? "Percent" : t == 3 ? "OnWeight" : "";
                    }
                    l.add("CommType " + text4);
                    l.add("Commission @ " + arg + " = Rs. " + g(Math.rint(item2.CommissionAmount)));
                    if (item2.SaleOrder > 0) l.add("Under contract # " + item2.SaleOrder);
                    if (!isBlank(item2.VehicleNo)) l.add("Vehicle # " + item2.VehicleNo);
                    if (item2.GpNo != 0) l.add("Gp # " + item2.GpNo);
                    if (!isBlank(h.ManualBillNo)) l.add("Manual # " + h.ManualBillNo);
                    l.add("From " + obj.ids.CompanyName);
                    String empty2 = String.join(", ", l);
                    VoucherDetail x = new VoucherDetail();
                    x.AccountId = jobLotAcc2 > 0 ? jobLotAcc2 : i(col(list6, "SaleGLAC"));
                    x.AgainstAccountId = num23 > 0 ? num23 : x.AccountId;
                    x.Comments = "".equals(h.CommissionRemarks) ? empty2 : empty2 + s(h.CommissionRemarks);
                    x.DebitAmount = item2.CommissionAmount; x.JobLotId = item2.JobLotId; x.BranchesId = item2.BranchId;
                    out.add(x);
                }
            }
            int num26 = 0;
            if (!flag) {
                List<Integer> distinctItems = obj.details.stream().map(x -> x.ItemId).distinct().toList();
                if (distinctItems.size() == 1) {
                    Map<String, Object> it = dtitem.get(distinctItems.get(0));
                    if (it != null) { num26 = i(col(it, "SaleGLAC")); obj.ids.SaleGLAccountId = num26; }
                }
            }
            for (Commission item : obj.commissions) {
                Map<String, Object> agent = dtSupplier.get(item.CommissionAgentId);
                if (agent == null) throw new IllegalStateException("Commission AgentGlId Not Found");
                int agentGl = i(col(agent, "GlAccountId"));
                String text6 = "", text7 = "", text8 = "";
                double qty, weight, amount;
                if (item.SaleOrderId > 0) {
                    List<Detail> l10 = obj.details.stream().filter(x -> x.SaleOrderId == item.SaleOrderId).toList();
                    qty = l10.stream().mapToDouble(x -> x.ItemQty).sum();
                    weight = l10.stream().mapToDouble(x -> x.NetBillWeight).sum();
                    amount = l10.stream().mapToDouble(x -> x.ItemAmount).sum();
                    if (!l10.isEmpty()) { text6 = String.valueOf(l10.get(0).SaleOrder); text7 = s(l10.get(0).VehicleNo); }
                } else {
                    qty = obj.details.stream().mapToDouble(x -> x.ItemQty).sum();
                    weight = obj.details.stream().mapToDouble(x -> x.NetBillWeight).sum();
                    amount = obj.details.stream().mapToDouble(x -> x.ItemAmount).sum();
                }
                String type = item.CommType == 1.0 ? "Flat" : item.CommType == 2.0 ? "Percent" : item.CommType != 3.0 ? "" : "OnWeight";
                List<String> l = new ArrayList<>();
                l.add("Sale ItemQty " + g(qty));
                if (!flag4) l.add("BillWeight " + g(weight));
                l.add("ItemAmount " + g(amount));
                l.add("CommType " + type);
                l.add("Commission paid @ " + g(item.Rate) + " = Rs. " + g(Math.rint(item.CommAmount)));
                if (!isBlank(text6)) l.add("Under contract # " + text6);
                if (!isBlank(text7)) l.add("Vehicle # " + text7);
                if (!isBlank(text8)) l.add("Gp # " + text8);
                if (!isBlank(h.ManualBillNo)) l.add("Manual # " + h.ManualBillNo);
                l.add("From " + obj.ids.CompanyName);
                String text5 = String.join(", ", l);
                String comments = "".equals(item.Remarks) ? text5 : text5 + s(item.Remarks);
                if (item.CommAmount > 0.0 && item.CommissionAgentId > 0 && flag) {
                    if (item.DebitAccountId == 0) throw new IllegalStateException("Commission Debit Account Not Found in Commission Agent Grid");
                    VoucherDetail a = new VoucherDetail();
                    a.AccountId = item.DebitAccountId; a.AgainstAccountId = agentGl; a.Comments = comments; a.DebitAmount = item.CommAmount;
                    a.SubsidiaryTypeId = 1; a.SubsidiaryAccountId = item.CommDebitSupCustId; a.SupplierCustomerId = item.CommDebitSupCustId;
                    a.SubsidiaryAgainstTypeId = a.SubsidiaryTypeId; a.SubsidiaryAgainstAccountId = item.CommissionAgentId;
                    a.BranchesId = h.BranchesId; a.CostCenterId = obj.ids.CostCenterId;
                    out.add(a);
                    VoucherDetail b = new VoucherDetail();
                    b.AccountId = agentGl; b.AgainstAccountId = item.DebitAccountId; b.Comments = comments; b.CreditAmount = item.CommAmount;
                    b.SubsidiaryTypeId = 1; b.SubsidiaryAccountId = item.CommissionAgentId; b.SupplierCustomerId = item.CommissionAgentId;
                    /* The C# then overwrites SubsidiaryAccountId with CommDebitSupCustId - kept. */
                    b.SubsidiaryAccountId = item.CommDebitSupCustId;
                    b.BranchesId = h.BranchesId; b.CostCenterId = obj.ids.CostCenterId;
                    out.add(b);
                }
                if (item.CommAmount > 0.0 && item.CommissionAgentId > 0 && !flag) {
                    VoucherDetail x = new VoucherDetail();
                    x.AccountId = agentGl; x.AgainstAccountId = num26 > 0 ? num26 : x.AccountId; x.Comments = comments; x.CreditAmount = item.CommAmount;
                    x.SubsidiaryTypeId = 1; x.SubsidiaryAccountId = item.CommissionAgentId; x.SupplierCustomerId = item.CommissionAgentId;
                    x.BranchesId = h.BranchesId; x.CostCenterId = obj.ids.CostCenterId;
                    out.add(x);
                }
            }
        }
        if (h.DiscountAmount.compareTo(BigDecimal.ZERO) > 0 && Set.of(1608, 1609, 1661, 1660, 1662).contains(h.DocumentTypeId)) {
            if (h.DiscountAccountId == 0) throw new IllegalStateException("Discount debit account fields is required");
            boolean ret = h.DocumentTypeId == 1662;
            String c = "Discount to " + obj.ids.CompanyName;
            VoucherDetail a = new VoucherDetail();
            a.LineId = 0; a.AccountId = ret ? ref : h.DiscountAccountId; a.AgainstAccountId = ret ? h.DiscountAccountId : ref; a.Comments = c;
            a.DebitAmount = h.DiscountAmount.doubleValue(); a.DMultiCurrencyId = h.CurrencyId; a.DExchangeCurrencyRate = h.ExchangeRate.doubleValue();
            a.DCurrencyAmount = h.FcyAmount.doubleValue(); a.SubsidiaryAgainstTypeId = 1; a.SubsidiaryAgainstAccountId = h.SupplierCustomerId;
            a.BranchesId = h.BranchesId; a.CostCenterId = obj.ids.CostCenterId;
            out.add(a);
            VoucherDetail b = new VoucherDetail();
            b.LineId = 0; b.AccountId = ret ? h.DiscountAccountId : ref; b.AgainstAccountId = ret ? ref : h.DiscountAccountId; b.Comments = c;
            b.CreditAmount = h.DiscountAmount.doubleValue(); b.DMultiCurrencyId = h.CurrencyId; b.DExchangeCurrencyRate = h.ExchangeRate.doubleValue();
            b.DCurrencyAmount = h.FcyAmount.doubleValue(); b.SubsidiaryTypeId = 1; b.SubsidiaryAccountId = h.SupplierCustomerId; b.SupplierCustomerId = h.SupplierCustomerId;
            b.BranchesId = h.BranchesId; b.CostCenterId = obj.ids.CostCenterId;
            out.add(b);
        }
        return out;
    }

    /** The item fields every customer/sale-GL and discount line carries (0613:581-602 and siblings). */
    private static void itemLine(VoucherDetail x, Detail d, boolean withCostCenter) {
        x.ItemId = d.ItemId; x.QtyOut = d.ItemQty; x.ItemRate = d.ItemRate; x.WeightOut = d.NetBillWeight;
        x.RateCut = d.RateCut; x.RateCutAmount = d.RateCutAmount; x.ItemAmount = d.ItemAmount; x.Expenses = d.ExpenseAmount;
        x.Freight = d.FreightAmount; x.Commission = d.CommissionAmount; x.OrderNo = d.SaleOrder; x.GpNo = d.GpNo;
        x.JobLotId = d.JobLotId; x.VehicleNo = d.VehicleNo; x.DMultiCurrencyId = d.CurrencyId;
        x.DExchangeCurrencyRate = d.ExchangeRate.doubleValue(); x.DCurrencyAmount = d.FcyAmount.doubleValue();
        x.BranchesId = d.BranchId;
        if (withCostCenter) x.CostCenterId = d.CostCenterId;
    }

    /** 0613:786-1103 - the CGS pair (or the 145 stock difference) for one detail line. */
    private void cgsLines(Invoice obj, Detail item3, Map<String, Object> list4, boolean flag2, boolean flag3, boolean flag6,
                          double num15, List<VoucherDetail> out) {
        Head h = obj.h;
        boolean eRPFeature5 = feature(5);
        boolean num16 = truthy(config("CGSEntryAllow"));
        boolean flag8 = truthy(config("SaleCostingJobOrderWise"));
        int num17 = flag2 ? i(config("FOCInventoryExpenseAccount")) : 0;
        int num18 = flag3 ? i(config("ZakatInventoryExpensesAccount")) : 0;
        if (flag2 && num17 == 0) throw new IllegalStateException("Foc Inventory Expense account not found from configuration...");
        if (flag3 && num18 == 0) throw new IllegalStateException("Zakat Inventory Expense account not found from configuration...");
        if (!(num16 || flag8 || (eRPFeature5 && flag6))) return;
        boolean flag9 = h.DocumentTypeId == 145;
        Set<Integer> hashSet3 = Set.of(103, 126, 133, 145);
        boolean flag10 = hashSet3.contains(h.DocumentTypeId) || feature(1);
        boolean feature2 = feature(2);
        boolean flag11 = feature(3);
        if (eRPFeature5 && flag6 && !flag9) flag11 = true;
        if (flag11) {
            if (item3.RefRefDocumentTypeId <= 0 || item3.RefRefDocIdNo <= 0) throw new IllegalStateException("RefIds Not Found");
            Map<String, Object> r = repo.avgRateFromLoaderStock(org, company, item3.RefRefDocumentTypeId, item3.RefRefDocIdNo, item3.RefDocSubId);
            if (r != null) { item3.ItemCgsRate = d(col(r, "AvgRate")); item3.RateUOM = d(col(r, "RateUom")); item3.CgsRateUomId = i(col(r, "RateUomId")); }
            if (item3.ItemCgsRate <= 0.0) throw new IllegalStateException("CGS Rate not found");
        } else if (flag10 || feature2) {
            if (hashSet3.contains(h.DocumentTypeId))
                throw new UnsupportedOperationException("GetAvgRatesAndStockInHand.GetAvgRateQtyAndStockInHand (document type " + h.DocumentTypeId + ") is not ported");
            else if (flag8) {
                item3.ItemCgsRate = repo.avgRateByJobOrder(org, company, h.DocDate, item3.JobLotId, item3.ItemId, h.Id > 0 ? h.DocumentTypeId : 0, h.Id > 0 ? h.Id : 0);
                if (item3.ItemCgsRate <= 0.0) item3.ItemCgsRate = 1.0;
            } else {
                item3.ItemCgsRate = repo.avgRateOnlyForCgs(org, company, item3.ItemId, h.DocDate, h.Id > 0 ? h.DocumentTypeId : 0, h.Id > 0 ? h.Id : 0,
                        item3.JobLotId, item3.CropYear, item3.WarehouseId);
                if (item3.ItemCgsRate <= 0.0) throw new IllegalStateException("CGS Rate not found.for Item " + s(item3.ItemName));
            }
        }
        if (!(flag11 || flag10 || feature2)) return;
        double num19 = 0.0;
        List<String> list5 = new ArrayList<>();
        int num20 = i(col(list4, "PurchaseGLAC")), num21 = i(col(list4, "COGSGLAC"));
        String itemName2 = s(col(list4, "ItemName"));
        if (h.DocumentTypeId == 128) {
            list5.add("NoOfBags: " + g(item3.ItemQty)); list5.add(itemName2); list5.add("Net Weight: " + g(item3.NetBillWeight));
            list5.add("Rate: " + g(item3.ItemCgsRate));
            if (item3.ExpenseAmount > 0.0) list5.add("Expense Amount: " + g(item3.ExpenseAmount));
            if (item3.CommissionAmount > 0.0) list5.add("Commission Amount: " + g(item3.CommissionAmount));
            throw new UnsupportedOperationException("CommonServies.GetEqvilentByItemId (document type 128) is not ported");
        } else if (Set.of(103, 126, 133, 184, 145).contains(h.DocumentTypeId)) {
            list5.add("ItemQty: " + g(item3.ItemQty)); list5.add(itemName2);
            if (item3.NetBillWeight > 0.0) list5.add("Net Weight: " + g(item3.NetBillWeight));
            list5.add("Rate: " + g(item3.ItemCgsRate));
            if (item3.ItemDiscountAmount > 0.0) list5.add("Discount Amount: " + g(item3.ItemDiscountAmount));
            if (item3.FreightAmount > 0.0) list5.add("Credit Amount: " + g(item3.FreightAmount));
            if (item3.CommissionAmount > 0.0) list5.add("Commission Amount: " + g(item3.CommissionAmount));
            num19 = item3.ItemCgsRate * item3.ItemQty;
        } else {
            list5.add("NoOfBags: " + g(item3.ItemQty)); list5.add(itemName2); list5.add("GrossWeight: " + g(item3.GrossWeight));
            list5.add("Net Weight: " + g(item3.NetBillWeight)); list5.add("Rate: " + g(item3.ItemCgsRate));
            if (item3.EBTotalWt > 0.0) list5.add("EmptyBagsDeduction: " + g(item3.EBTotalWt));
            if (item3.ExpenseAmount > 0.0) list5.add("Expense Amount: " + g(item3.ExpenseAmount));
            if (item3.CommissionAmount > 0.0) list5.add("Commission Amount: " + g(item3.CommissionAmount));
            if (item3.NetBillWeight == 0.0) throw new IllegalStateException("Validation Error: Net Bill Weight is missing.");
            if (item3.RateUOM == 0.0) throw new IllegalStateException("Validation Error: Rate UOM is missing.");
        }
        list5.add("Party: " + obj.ids.CompanyName);
        String comments = String.join(", ", list5);
        if (!Set.of(128, 103, 126, 133, 145).contains(h.DocumentTypeId) && item3.ItemCgsRate > 0.0 && item3.NetBillWeight > 0.0) {
            if (!flag11) {
                item3.RateUOM = repo.equivalent(org, company, item3.ItemId, item3.UomScheduleIdRate);
                item3.ItemCgsRate = item3.RateUOM * item3.ItemCgsRate;
            }
            num19 = item3.NetStockWeight / item3.RateUOM * item3.ItemCgsRate;
        }
        if (!flag9) {
            int debitAcc = num17 > 0 ? num17 : (num18 > 0 ? num18 : num21);
            VoucherDetail a = cgs(item3, debitAcc, num20, comments, num19, h.SupplierCustomerId);
            a.DebitAmount = num19;
            out.add(a);
            VoucherDetail b = cgs(item3, num20, debitAcc, comments, num19, h.SupplierCustomerId);
            b.CreditAmount = num19;
            out.add(b);
        } else {
            double num22 = num15 - num19;
            if (num22 != 0.0) {
                VoucherDetail a = new VoucherDetail();
                a.IsCGS = 1; a.AccountId = num20; a.AgainstAccountId = num21; a.Comments = "Stock Difference";
                if (num22 > 0.0) a.DebitAmount = num22; else a.CreditAmount = num22;
                a.BranchesId = h.BranchesId;
                out.add(a);
                VoucherDetail b = new VoucherDetail();
                b.IsCGS = 1; b.AccountId = num21; b.AgainstAccountId = num20; b.Comments = "Stock Difference";
                if (num22 > 0.0) b.CreditAmount = num22; else b.DebitAmount = num22;
                b.BranchesId = h.BranchesId;
                out.add(b);
            }
        }
    }

    private static VoucherDetail cgs(Detail item3, int acc, int against, String comments, double amount, int party) {
        VoucherDetail x = new VoucherDetail();
        x.LineId = item3.LineId; x.AccountId = acc; x.AgainstAccountId = against; x.Comments = comments;
        x.DocumentTypeIdRef = item3.RefRefDocumentTypeId; x.InvoiceNoRefId = item3.RefRefDocIdNo; x.RefInvoiceNo = String.valueOf(item3.RefDocSubId);
        x.ItemId = item3.ItemId; x.QtyOut = item3.ItemQty; x.WeightOut = item3.NetBillWeight; x.ItemCgsRate = item3.ItemCgsRate;
        x.RateCut = item3.RateCut; x.RateCutAmount = item3.RateCutAmount; x.ItemAmount = amount; x.Expenses = item3.ExpenseAmount;
        x.Commission = item3.CommissionAmount; x.Freight = item3.FreightAmount; x.OrderNo = item3.SaleOrder; x.GpNo = item3.GpNo;
        x.VehicleNo = item3.VehicleNo; x.JobLotId = item3.JobLotId; x.SupplierCustomerId = party; x.BranchesId = item3.BranchId;
        x.IsCGS = 1; x.CostCenterId = item3.CostCenterId;
        return x;
    }

    /** 0613:1693 AddVoucherDetail. */
    private static VoucherDetail addVoucherDetail(int accountId, int againstAccountId, Double debit, Double credit, boolean isQtyOut,
                                                  Detail item, String remarks, int supplierCustomerId, String companyName) {
        VoucherDetail x = new VoucherDetail();
        x.LineId = item.LineId; x.AccountId = accountId; x.AgainstAccountId = againstAccountId;
        x.Comments = remarks + (credit != null ? "  " + companyName : "");
        x.DebitAmount = debit == null ? 0 : debit; x.CreditAmount = credit == null ? 0 : credit;
        x.ItemId = item.ItemId; x.QtyIn = isQtyOut ? 0.0 : item.ItemQty; x.QtyOut = isQtyOut ? item.ItemQty : 0.0; x.ItemRate = item.ItemRate;
        x.WeightIn = isQtyOut ? 0.0 : item.NetBillWeight; x.WeightOut = isQtyOut ? item.NetBillWeight : 0.0;
        x.RateCut = item.RateCut; x.RateCutAmount = item.RateCutAmount; x.ItemAmount = item.ItemAmount; x.Expenses = item.ExpenseAmount;
        x.Freight = item.FreightAmount; x.Commission = item.CommissionAmount; x.OrderNo = item.SaleOrder; x.GpNo = item.GpNo;
        x.JobLotId = item.JobLotId; x.VehicleNo = item.VehicleNo; x.DMultiCurrencyId = item.CurrencyId;
        x.DExchangeCurrencyRate = item.ExchangeRate.doubleValue(); x.DCurrencyAmount = item.FcyAmount.doubleValue();
        x.SubsidiaryTypeId = debit != null ? 1 : 0; x.SubsidiaryAccountId = debit != null ? supplierCustomerId : 0;
        x.SubsidiaryAgainstTypeId = credit != null ? 1 : 0; x.SubsidiaryAgainstAccountId = credit != null ? supplierCustomerId : 0;
        x.SupplierCustomerId = supplierCustomerId; x.BranchesId = item.BranchId; x.IsCGS = 0; x.CostCenterId = item.CostCenterId;
        return x;
    }

    // =========================================================================== 0613 OtherExpenseFinancial

    List<VoucherDetail> otherExpenseFinancial(Invoice obj, List<Map<String, Object>> dtItemOther) {
        Head h = obj.h;
        int ref = obj.ids.SupplierGlAccountId;
        List<VoucherDetail> out = new ArrayList<>();
        if (!Set.of(95, 99, 103, 139, 171, 184, 186, 1809, 1608, 1609, 1661, 1660, 222, 1662).contains(h.DocumentTypeId) || obj.expenses.isEmpty()) return out;
        if (!truthy(config("CreditAmountInItemSaleGL"))) return out;
        if (dtItemOther.isEmpty()) throw new IllegalStateException("Other Items not found");
        for (Expense e : obj.expenses) {
            if (e.Amount <= 0.0) continue;
            Map<String, Object> other = dtItemOther.stream().filter(r -> i(col(r, "Id")) == e.InvRevExpItemId).findFirst().orElse(null);
            if (other == null) throw new IllegalStateException("Other Items Sale GL Account not found");
            int saleGl = i(col(other, "SaleGLAcId"));
            String comments = s(col(other, "OtherItemName")) + " " + s(e.Remarks);
            boolean ret = h.DocumentTypeId == 1662;
            VoucherDetail a = new VoucherDetail();
            a.AccountId = ret ? saleGl : ref; a.AgainstAccountId = ret ? ref : saleGl; a.Comments = comments; a.DebitAmount = e.Amount;
            a.SubsidiaryTypeId = 1; a.SubsidiaryAccountId = h.SupplierCustomerId; a.SupplierCustomerId = h.SupplierCustomerId;
            a.BranchesId = h.BranchesId; a.CostCenterId = obj.ids.CostCenterId;
            out.add(a);
            VoucherDetail b = new VoucherDetail();
            b.AccountId = ret ? ref : saleGl; b.AgainstAccountId = ret ? saleGl : ref; b.Comments = comments; b.CreditAmount = e.Amount;
            b.SubsidiaryAgainstTypeId = 1; b.SubsidiaryAgainstAccountId = h.SupplierCustomerId; b.BranchesId = h.BranchesId;
            b.CostCenterId = obj.ids.CostCenterId;
            out.add(b);
        }
        return out;
    }

    // =========================================================================== .NET formatting

    static boolean isBlank(String v) { return v == null || v.trim().isEmpty(); }

    private static final double[] POW10 = {1E0, 1E1, 1E2, 1E3, 1E4, 1E5, 1E6, 1E7, 1E8, 1E9, 1E10, 1E11, 1E12, 1E13, 1E14, 1E15};

    /** .NET Framework Math.Round(double, digits): scale by 10^digits, round half to even, scale back. */
    public static double round(double v, int digits) {
        if (Double.isNaN(v) || Double.isInfinite(v)) return v;
        if (digits < 0 || digits > 15) digits = Math.max(0, Math.min(15, digits));
        if (Math.abs(v) >= 1E16) return v;
        double p = POW10[digits];
        return Math.rint(v * p) / p;
    }

    /** .NET Framework Math.Round(double, digits, MidpointRounding.AwayFromZero): ModF on the scaled value, |fraction| >= 0.5 steps away. */
    public static double roundAway(double v, int digits) {
        if (Double.isNaN(v) || Double.isInfinite(v)) return v;
        if (digits < 0 || digits > 15) digits = Math.max(0, Math.min(15, digits));
        if (Math.abs(v) >= 1E16) return v;
        double p = POW10[digits];
        double scaled = v * p;
        double whole = scaled < 0 ? Math.ceil(scaled) : Math.floor(scaled);
        double fraction = scaled - whole;
        if (Math.abs(fraction) >= 0.5) whole += Math.signum(fraction);
        return whole / p;
    }

    /** .NET Framework double.ToString(): "G" with 15 significant digits. */
    public static String g(double v) {
        if (Double.isNaN(v)) return "NaN";
        if (Double.isInfinite(v)) return v > 0 ? "Infinity" : "-Infinity";
        if (v == 0) return "0";
        BigDecimal b = new BigDecimal(v).round(new MathContext(15, RoundingMode.HALF_EVEN)).stripTrailingZeros();
        int exponent = b.precision() - b.scale() - 1;
        if (exponent >= 15 || exponent < -5) {
            String digits = b.unscaledValue().abs().toString();
            String mantissa = digits.length() > 1 ? digits.charAt(0) + "." + digits.substring(1) : digits;
            return (b.signum() < 0 ? "-" : "") + mantissa + "E" + (exponent < 0 ? "-" : "+") + String.format("%02d", Math.abs(exponent));
        }
        return b.toPlainString();
    }
}
