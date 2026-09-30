package com.mst.services.cmagt;

import com.mst.models.cmagt.dto.TradeBillAgainstGdnCmagtDto.Bill;
import com.mst.models.cmagt.dto.TradeBillAgainstGdnCmagtDto.CommissionDetail;
import com.mst.models.cmagt.dto.TradeBillAgainstGdnCmagtDto.Detail;
import com.mst.models.cmagt.dto.TradeBillAgainstGdnCmagtDto.FreightDetail;
import com.mst.models.cmagt.dto.TradeBillAgainstGdnCmagtDto.PurchaseExpense;
import com.mst.models.cmagt.dto.TradeBillAgainstGdnCmagtDto.PurchaseFreightExpense;
import com.mst.models.cmagt.dto.TradeBillAgainstGdnCmagtDto.SaleExpense;
import com.mst.models.cmagt.dto.TradeBillAgainstGdnCmagtDto.SaleExpenseCreditToReleventAc;
import com.mst.models.cmagt.dto.TradeBillAgainstGdnCmagtDto.TaxDetail;
import com.mst.models.dto.ContraVoucherDto;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.text.DecimalFormat;
import java.text.DecimalFormatSymbols;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashSet;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.Set;

/**
 * Architecture.BLL.Inventory.InvCommAgentTradeBill.MakeVoucher (BLL 0547:19-680), line for line,
 * plus its three private remark builders (:682-758).
 *
 * The accumulators keep the desktop names' meaning: {@code num6} purchase side (ends up credited
 * to Trading), {@code num7} sale side, {@code num8} the supplier item credit total that becomes
 * VoucherAmount and BillAmount, {@code text} the remarks carried by the single balancing line on
 * TradingGlAccountId. All of them are double, as on the desktop, and every guard, side, account
 * and refusal message is the desktop's own - including its asymmetries (sale lines gated on
 * SaleNetAmount but posting SaleAmount; the sale tax-detail pair reversed relative to purchase;
 * freight-detail side 1 accumulating to num7).
 *
 * The header-level commission/brokery blocks (voucher lines 11-18) are ported although the form
 * never fills CommAmount/BrokeryAmount/CustCommAmount/CustBrokeryAmount (Insert() leaves them 0),
 * so on this screen they never fire - the per-row CommissionDetail list carries commission.
 */
public final class TradeBillVoucherBuilder {

    private static final String NL = "\r\n";   // Environment.NewLine on Windows
    private static final DateTimeFormatter ISO = DateTimeFormatter.ofPattern("yyyy-MM-dd'T'HH:mm:ss.SSS");

    public static final class Result {
        public final ContraVoucherDto.Head head;
        public final List<ContraVoucherDto.Detail> details;
        Result(ContraVoucherDto.Head h, List<ContraVoucherDto.Detail> d) { head = h; details = d; }
    }

    private TradeBillVoucherBuilder() { }

    /**
     * @param glList CommonServies.GetSupplierCustomerListForFinancialEffects - Id -> GlAccountId
     */
    public static Result makeVoucher(Bill obj, Map<Integer, Integer> glList) {
        ContraVoucherDto.Head vh = new ContraVoucherDto.Head();
        List<ContraVoucherDto.Detail> lines = new ArrayList<>();
        int num = 0, num2 = 0, num3 = 0, num4 = 0, num5 = 0;
        double num6 = 0.0, num7 = 0.0;
        String text = (obj.RemarksHeader != null && !obj.RemarksHeader.isEmpty()) ? obj.RemarksHeader + " " : "";

        LocalDateTime now = LocalDateTime.now();
        vh.DocumentTypeId = obj.DocumentTypeId;
        vh.DocumentTypeSrNo = obj.Id;
        vh.RefDocNoId = obj.Id;
        vh.VoucherCode = obj.DocNo;
        vh.VoucherDate = obj.DocDate == null ? null : obj.DocDate.toLocalDateTime().format(ISO);
        vh.Remarks = obj.RemarksHeader == null ? "" : obj.RemarksHeader;       // Conversion.ToString
        vh.RemarksOtherLingo = "";
        vh.ChequeDate = now.toLocalDate().atStartOfDay().format(ISO);            // DateTime.Today
        vh.IncludeWHT = Boolean.FALSE;
        vh.BranchId = obj.BranchesId;
        vh.ProjectId = obj.ProjectsId;
        vh.ManualBillNo = String.valueOf(obj.DocNo);
        vh.DueDate = now.format(ISO);
        vh.OrganizationId = obj.OrganizationId;
        vh.CompanyId = obj.CompanyId;
        vh.FinancialYearId = obj.FinancialYearId;
        vh.EntryUser = obj.EnteryUserId;
        vh.EntryDate = now.format(ISO);
        vh.ModifyDate = now.format(ISO);
        vh.ModifyUser = obj.ModifyUserId;
        vh.InclusiveTax = null;          // bool? - null, omitted

        if (glList == null || glList.isEmpty()) throw new IllegalStateException("SupplierCustomer list not Found");
        Integer supGl = glList.get(obj.SupplierId);
        if (supGl == null) throw new IllegalStateException("Supplier GLAccountId not Found");
        vh.RefAccountId = supGl;
        Integer cusGl = glList.get(obj.CustomerId);
        if (cusGl == null) throw new IllegalStateException("Customer GLAccountId not Found");
        num = cusGl;
        if (obj.CommissionAgentId > 0) {
            Integer g = glList.get(obj.CommissionAgentId);
            if (g == null) throw new IllegalStateException("Commission Agent GLAccountId not Found");
            num2 = g;
        }
        if (obj.BrokerId > 0) {
            Integer g = glList.get(obj.BrokerId);
            if (g == null) throw new IllegalStateException("Broker Agent GLAccountId not Found");
            num4 = g;
        }
        if (obj.CustCommissionAgentId > 0) {
            Integer g = glList.get(obj.CustCommissionAgentId);
            if (g == null) throw new IllegalStateException("Commission Agent GLAccountId not Found");
            num3 = g;
        }
        if (obj.CustBrokerId > 0) {
            Integer g = glList.get(obj.CustBrokerId);
            if (g == null) throw new IllegalStateException("Broker Agent GLAccountId not Found");
            num5 = g;
        }
        if (vh.RefAccountId == 0) throw new IllegalStateException("SupplierGlId Not Found");
        if (num == 0) throw new IllegalStateException("CustomerGlId Not Found");
        if (obj.TradingGlAccountId == 0) throw new IllegalStateException("TradingGlAccountId Not Found");
        int trading = obj.TradingGlAccountId;
        int sup = vh.RefAccountId;

        if (obj.InvCommAgentTradeBillDetailslist.isEmpty()) {
            return new Result(vh, lines);
        }

        List<String> list8 = distinctVehicles(obj);
        double num8 = 0.0;

        for (Detail item3 : obj.InvCommAgentTradeBillDetailslist) {
            if (item3.ActionTypeId == 3) continue;
            if (item3.ItemAmount > 0.0 && item3.RatePurchase.compareTo(BigDecimal.ZERO) > 0) {
                text += "Purchase : RefNo: " + s(item3.RefDocNo) + ", Item: " + s(item3.ItemName) + ", Qty: " + dec(item3.Qty)
                      + ", Weight: " + dec(item3.BillWeight) + ", Rate: " + dec(item3.RatePurchase)
                      + ", VehicleNo: " + s(item3.VehicleNo) + NL;
                num6 += item3.ItemAmount;
                ContraVoucherDto.Detail d = new ContraVoucherDto.Detail();
                d.AccountId = sup;
                d.AgainstAccountId = trading;
                d.Comments = buildSupplierRemarks(item3);
                d.RefInvoiceNo = s(item3.RefDocNo);
                d.CreditAmount = item3.ItemAmount;
                d.ItemId = item3.ItemId;
                d.QtyIn = item3.Qty.doubleValue();
                d.ItemRate = item3.RatePurchase.doubleValue();
                d.WeightIn = item3.BillWeight.doubleValue();
                d.ItemAmount = item3.ItemAmount;
                d.VehicleNo = s(item3.VehicleNo);
                lines.add(d);
                num8 += d.CreditAmount;
            }
            if (item3.SaleNetAmount > 0.0 && item3.RateSale.compareTo(BigDecimal.ZERO) > 0) {
                text += "Sales : " + s(item3.ItemName) + " Qty: " + dec(item3.Qty) + ", Weight: " + dec(item3.SaleBillWeight)
                      + ", Rate: " + dec(item3.RateSale) + NL;
                ContraVoucherDto.Detail d = new ContraVoucherDto.Detail();
                d.AccountId = num;
                d.AgainstAccountId = trading;
                d.Comments = buildCustomerRemarks(item3);
                d.RefInvoiceNo = s(item3.RefDocNo);
                d.DebitAmount = item3.SaleAmount;
                d.ItemId = item3.ItemId;
                d.QtyOut = item3.Qty.doubleValue();
                d.ItemRate = item3.RateSale.doubleValue();
                d.WeightOut = item3.SaleBillWeight.doubleValue();
                d.ItemAmount = item3.SaleAmount;
                d.VehicleNo = s(item3.VehicleNo);
                lines.add(d);
                num7 += item3.SaleAmount;
            }
        }

        for (PurchaseExpense item4 : obj.InvCommAgentTradePurchaseExpList) {
            if (item4.Amount > 0.0 && item4.InvExpItemId > 0) {
                text = text + "Purchase Expenses : " + s(item4.Remarks) + NL;
                num6 += item4.Amount;
                String text2 = item4.Remarks;
                if (!list8.isEmpty()) text2 = s(text2) + " Against " + vehicleLabel(list8) + ": " + String.join(", ", list8);
                ContraVoucherDto.Detail d = new ContraVoucherDto.Detail();
                d.AccountId = sup;
                d.AgainstAccountId = trading;
                d.Comments = text2;
                d.CreditAmount = item4.Amount;
                d.ItemId = item4.InvExpItemId;
                d.QtyIn = item4.Qty;
                d.ItemRate = item4.Rate;
                d.ItemAmount = item4.Amount;
                lines.add(d);
            }
        }

        for (SaleExpense item5 : obj.InvCommAgentTradeSaleExpList) {
            if (item5.Amount > 0.0 && item5.InvExpItemId > 0) {
                String text4 = item5.Remarks;
                if (!list8.isEmpty()) text4 = s(text4) + " Against " + vehicleLabel(list8) + ": " + String.join(", ", list8);
                ContraVoucherDto.Detail d = new ContraVoucherDto.Detail();
                d.AccountId = num;
                d.AgainstAccountId = trading;
                d.Comments = text4;
                d.DebitAmount = item5.Amount;
                d.ItemId = item5.InvExpItemId;
                d.QtyOut = item5.Qty;
                d.ItemRate = item5.Rate;
                d.ItemAmount = item5.Amount;
                lines.add(d);
                num7 += item5.Amount;
                text = text + "Sales Expenses : " + s(item5.Remarks) + NL;
            }
        }

        boolean saleIsLoad = isLoadName(obj.DeliveryTermSale);
        for (SaleExpenseCreditToReleventAc item6 : obj.CommisionAgentBillSaleExpenseCreditToReleventAcsList) {
            if (!(item6.Amount > 0.0) || item6.AccountId <= 0) continue;
            if (saleIsLoad) {
                if (num == item6.AccountId)
                    throw new IllegalStateException("Sale Expenses Credit Account and Customer Account cannot be same because delivery term is load.");
                ContraVoucherDto.Detail d5 = new ContraVoucherDto.Detail();
                d5.AccountId = num;
                d5.AgainstAccountId = item6.AccountId;
                d5.Comments = s(item6.Remarks);
                d5.DebitAmount = item6.Amount;
                lines.add(d5);
                if (trading == item6.AccountId && "Load".equals(obj.DeliveryTermSale)) {
                    text = text + "Sales Packing Material : " + s(item6.Remarks) + NL;
                    num7 += item6.Amount;
                    continue;
                }
                ContraVoucherDto.Detail d6 = new ContraVoucherDto.Detail();
                d6.AccountId = item6.AccountId;
                d6.AgainstAccountId = num;
                d6.Comments = s(item6.Remarks);
                d6.CreditAmount = item6.Amount;
                lines.add(d6);
            } else {
                text = text + "Sales Packing Material : " + s(item6.Remarks) + NL;
                num6 += item6.Amount;
                ContraVoucherDto.Detail d7 = new ContraVoucherDto.Detail();
                d7.AccountId = item6.AccountId;
                d7.AgainstAccountId = trading;
                d7.Comments = s(item6.Remarks);
                d7.CreditAmount = item6.Amount;
                lines.add(d7);
            }
        }

        boolean purchaseIsLoad = isLoadName(obj.DeliveryTerm);
        for (PurchaseFreightExpense item7 : obj.InvCommAgentTradeFreightExpList) {
            if (!(item7.Amount.compareTo(BigDecimal.ZERO) > 0) || item7.AccountId <= 0) continue;
            double amt = item7.Amount.doubleValue();
            if (purchaseIsLoad) {
                text = text + "Purchase Freight Expenses : " + s(item7.Remarks) + NL;
                num6 += amt;
                ContraVoucherDto.Detail d8 = new ContraVoucherDto.Detail();
                d8.AccountId = item7.AccountId;
                d8.AgainstAccountId = trading;
                d8.Comments = s(item7.Remarks);
                d8.CreditAmount = amt;
                lines.add(d8);
                continue;
            }
            if (sup == item7.AccountId)
                throw new IllegalStateException("Freight Credit Account and Supplier Account cannot be same because delivery term is ponch.");
            ContraVoucherDto.Detail d9 = new ContraVoucherDto.Detail();
            d9.AccountId = sup;
            d9.AgainstAccountId = item7.AccountId;
            d9.Comments = s(item7.Remarks);
            d9.DebitAmount = amt;
            lines.add(d9);
            if (trading != item7.AccountId) {
                ContraVoucherDto.Detail d10 = new ContraVoucherDto.Detail();
                d10.AccountId = item7.AccountId;
                d10.AgainstAccountId = sup;
                d10.Comments = s(item7.Remarks);
                d10.CreditAmount = amt;
                lines.add(d10);
            } else {
                text = text + "Purchase Freight Expenses : " + s(item7.Remarks) + NL;
                num7 += amt;
            }
        }

        Set<Integer> ponch = new HashSet<>(Arrays.asList(2, 5, 6));
        for (FreightDetail item2 : obj.CommisionAgentBillFreightDetailList) {
            if (item2.Amount.compareTo(BigDecimal.ZERO) <= 0 || item2.AccountId <= 0) continue;
            Integer g = glList.get(item2.AccountId);
            if (g == null) throw new IllegalStateException((item2.EntrySideId == 1 ? "Purchase" : "Sale") + " Freight GLAccountId not Found");
            int num9 = g;
            if (item2.EntrySideId == 1) {
                if (ponch.contains(obj.DeliveryTermPurchaseId)) {
                    text = text + "Purchase Freight : " + s(item2.Remarks) + NL;
                    num7 += item2.NetAmount.doubleValue();
                    ContraVoucherDto.Detail d = new ContraVoucherDto.Detail();
                    d.AccountId = num9;
                    d.AgainstAccountId = trading;
                    d.DebitAmount = item2.NetAmount.doubleValue();
                    d.Comments = buildFreightRemarks(obj, item2);
                    lines.add(d);
                }
            } else if (item2.EntrySideId == 2 && ponch.contains(obj.DeliveryTermSaleId)) {
                text = text + "Sale Freight : " + s(item2.Remarks) + NL;
                num6 += item2.NetAmount.doubleValue();
                ContraVoucherDto.Detail d = new ContraVoucherDto.Detail();
                d.AccountId = num9;
                d.AgainstAccountId = trading;
                d.CreditAmount = item2.NetAmount.doubleValue();
                d.Comments = buildFreightRemarks(obj, item2);
                lines.add(d);
            }
        }

        /* Lines 11-18: header commission / brokery (dead on this screen - see class note). */
        double[] acc = {num6, num7};
        text = headerCommission(lines, acc, text, trading, obj.CommAmount, obj.CommissionAgentId, obj.CommissionDebitGLId,
                num2, obj.CommRate, "Purchase Commission");
        text = headerCommission(lines, acc, text, trading, obj.BrokeryAmount, obj.BrokerId, obj.BrokeryDebitGLId,
                num4, obj.BrokeryRate, "Purchase Brokery");
        text = headerCommission(lines, acc, text, trading, obj.CustCommAmount, obj.CustCommissionAgentId, obj.CustCommissionDebitGLId,
                num3, obj.CustCommRate, "Sales Commission");
        text = headerCommission(lines, acc, text, trading, obj.CustBrokeryAmount, obj.CustBrokerId, obj.CustBrokeryDebitGLId,
                num5, obj.CustBrokeryRate, "Sales Brokery");
        num6 = acc[0];
        num7 = acc[1];

        /* Lines 19/20: commission detail list. */
        for (CommissionDetail item : obj.CommisionAgentBillCommissionDetailList) {
            if (!(item.commissionAmount.compareTo(BigDecimal.ZERO) > 0) || item.commissionAgentId <= 0) continue;
            Integer g = glList.get(item.commissionAgentId);
            if (g == null) {
                if (item.agentTypeId == 1) throw new IllegalStateException("Commission Agent GlAccountId Not Found");
                throw new IllegalStateException("Broker Agent GlAccountId Not Found");
            }
            int agentGl = g;
            double amt = item.commissionAmount.doubleValue();
            if (trading == item.debitAccountId || item.debitAccountId == 0) {
                text += s(item.commissionRemarks);
                num6 += amt;
            } else {
                ContraVoucherDto.Detail d = new ContraVoucherDto.Detail();
                d.AccountId = item.debitAccountId;
                d.AgainstAccountId = agentGl;
                d.Comments = item.commissionRemarks;
                d.DebitAmount = amt;
                lines.add(d);
            }
            if (trading == agentGl) {
                text += s(item.commissionRemarks);
                num7 += amt;
                continue;
            }
            ContraVoucherDto.Detail d = new ContraVoucherDto.Detail();
            d.AccountId = agentGl;
            d.AgainstAccountId = item.debitAccountId;
            d.Comments = item.commissionRemarks;
            d.CreditAmount = amt;
            lines.add(d);
        }

        /* Lines 21-24: header tax from the detail rows (self-balancing, no accumulator). */
        BigDecimal num10 = BigDecimal.ZERO;
        for (Detail x : obj.InvCommAgentTradeBillDetailslist) num10 = num10.add(x.TaxAmountPurchase);
        if (num10.compareTo(BigDecimal.ZERO) > 0) {
            LinkedHashSet<String> parts = new LinkedHashSet<>();
            for (Detail x : obj.InvCommAgentTradeBillDetailslist)
                if (x.TaxAmountPurchase.compareTo(BigDecimal.ZERO) > 0) parts.add(dec(x.TaxPercentPurchase) + "% " + s(x.TaxNamePurchase));
            String comments = String.join(", ", parts);
            if (obj.PurchaseTaxAccountId == 0) throw new IllegalStateException("Purchase TaxAccount field is required");
            lines.add(line(obj.PurchaseTaxAccountId, sup, comments, num10.doubleValue(), 0));
            lines.add(line(sup, obj.PurchaseTaxAccountId, comments, 0, num10.doubleValue()));
        }
        BigDecimal num11 = BigDecimal.ZERO;
        for (Detail x : obj.InvCommAgentTradeBillDetailslist) num11 = num11.add(x.TaxAmountSale);
        if (num11.compareTo(BigDecimal.ZERO) > 0) {
            LinkedHashSet<String> parts = new LinkedHashSet<>();
            for (Detail x : obj.InvCommAgentTradeBillDetailslist)
                if (x.TaxAmountSale.compareTo(BigDecimal.ZERO) > 0) parts.add(dec(x.TaxPercentSale) + "% " + s(x.TaxNameSale));
            String comments2 = String.join(", ", parts);
            if (obj.SaleTaxAccountId == 0) throw new IllegalStateException("Sales TaxAccount field is required");
            lines.add(line(num, obj.SaleTaxAccountId, comments2, num11.doubleValue(), 0));
            lines.add(line(obj.SaleTaxAccountId, num, comments2, 0, num11.doubleValue()));
        }

        /* Lines 25-28: tax detail list (WHT). */
        List<TaxDetail> src = obj.CommisionAgentBillTaxDetailList;
        if (src != null && !src.isEmpty()) {
            List<TaxDetail> list11 = new ArrayList<>();
            for (TaxDetail x : src) if (x.EntrySideId == 1 && x.TaxAmount.compareTo(BigDecimal.ZERO) > 0) list11.add(x);
            if (!list11.isEmpty()) {
                LinkedHashSet<String> l12 = new LinkedHashSet<>();
                for (TaxDetail x : list11)
                    l12.add("Purchase Tax - " + s(x.TaxTypeName) + " @ " + dec(x.TaxPercantage) + "% Amount: " + n2(x.TaxAmount));
                List<String> l = new ArrayList<>(l12);
                if (!list8.isEmpty()) l.add("Against " + vehicleLabel(list8) + ": " + String.join(", ", list8));
                String comments3 = String.join(" | ", l);
                for (TaxDetail t : list11) {
                    if (t.TaxAccountId == 0) throw new IllegalStateException("Purchase Tax Account is required.");
                    ContraVoucherDto.Detail a = line(sup, t.TaxAccountId, comments3, t.TaxAmount.doubleValue(), 0);
                    a.TaxTypeId = t.TaxTypeId;
                    lines.add(a);
                    ContraVoucherDto.Detail b = line(t.TaxAccountId, sup, comments3, 0, t.TaxAmount.doubleValue());
                    b.TaxTypeId = t.TaxTypeId;
                    lines.add(b);
                }
            }
            List<TaxDetail> list13 = new ArrayList<>();
            for (TaxDetail x : src) if (x.EntrySideId == 2 && x.TaxAmount.compareTo(BigDecimal.ZERO) > 0) list13.add(x);
            if (!list13.isEmpty()) {
                LinkedHashSet<String> l14 = new LinkedHashSet<>();
                for (TaxDetail x : list13)
                    l14.add("Sales Tax - " + s(x.TaxTypeName) + " @ " + dec(x.TaxPercantage) + "% Amount: " + n2(x.TaxAmount));
                List<String> l = new ArrayList<>(l14);
                if (!list8.isEmpty()) l.add("Against " + vehicleLabel(list8) + ": " + String.join(", ", list8));
                String comments4 = String.join(" | ", l);
                for (TaxDetail t : list13) {
                    if (t.TaxAccountId == 0) throw new IllegalStateException("Sale Tax Account is required.");
                    ContraVoucherDto.Detail a = line(num, t.TaxAccountId, comments4, 0, t.TaxAmount.doubleValue());
                    a.TaxTypeId = t.TaxTypeId;
                    lines.add(a);
                    ContraVoucherDto.Detail b = line(t.TaxAccountId, num, comments4, t.TaxAmount.doubleValue(), 0);
                    b.TaxTypeId = t.TaxTypeId;
                    lines.add(b);
                }
            }
        }

        vh.VoucherAmount = num8;
        vh.BillAmount = num8;
        double num12 = num6 - num7;
        if (num12 != 0.0) {
            ContraVoucherDto.Detail d29 = new ContraVoucherDto.Detail();
            d29.AccountId = trading;
            d29.AgainstAccountId = trading;
            d29.Comments = text;
            if (num12 > 0.0) d29.DebitAmount = num12;
            else d29.CreditAmount = Math.abs(num12);
            lines.add(d29);
        }
        return new Result(vh, lines);
    }

    private static String headerCommission(List<ContraVoucherDto.Detail> lines, double[] acc, String text, int trading,
                                           Double amount, Integer agentId, Integer debitGl, int agentGl, Double rate,
                                           String label) {
        double a = amount == null ? 0 : amount;
        int ag = agentId == null ? 0 : agentId;
        int dg = debitGl == null ? 0 : debitGl;
        if (!(a > 0.0 && ag > 0)) return text;
        String r = csDouble(rate == null ? 0 : rate);
        String am = csDouble(a);
        if (trading == dg || dg == 0) {
            text += label + " : % " + r + " : " + am + NL;
            acc[0] += a;
        } else {
            lines.add(line(dg, agentGl, "% " + r + " " + am, a, 0));
        }
        if (trading == agentGl) {
            text += label + " : % " + r + " : " + am + NL;
            acc[1] += a;
        } else {
            lines.add(line(agentGl, dg, "% " + r + " " + am, 0, a));
        }
        return text;
    }

    private static ContraVoucherDto.Detail line(int account, int against, String comments, double debit, double credit) {
        ContraVoucherDto.Detail d = new ContraVoucherDto.Detail();
        d.AccountId = account;
        d.AgainstAccountId = against;
        d.Comments = comments;
        d.DebitAmount = debit;
        d.CreditAmount = credit;
        return d;
    }

    // ------------------------------------------------------------------ remark builders (:682-758)

    private static String buildSupplierRemarks(Detail item) {
        List<String> l = new ArrayList<>();
        l.add("Item Name : '" + s(item.ItemName) + "' Qty : " + fmt(item.Qty, "#,##0.###") + " Weight : " + fmt(item.BillWeight, "#,##0.###"));
        l.add("Rate : " + fmt(item.RatePurchase, "#,##0.###") + " Amount : " + fmt(BigDecimal.valueOf(item.ItemAmount), "#,##0.###"));
        if (item.VehicleNo != null && !item.VehicleNo.trim().isEmpty()) l.add("Vehicle : " + item.VehicleNo);
        return String.join(" ", l);
    }

    private static String buildCustomerRemarks(Detail item) {
        List<String> l = new ArrayList<>();
        l.add("Item Name : '" + s(item.ItemName) + "' Qty : " + fmt(item.Qty, "#,##0.###") + " Weight : " + fmt(item.SaleBillWeight, "#,##0.###"));
        l.add("Rate : " + fmt(item.RateSale, "#,##0.###") + " Amount : " + fmt(BigDecimal.valueOf(item.SaleAmount), "#,##0.###"));
        if (item.VehicleNo != null && !item.VehicleNo.trim().isEmpty()) l.add("Vehicle : " + item.VehicleNo);
        return String.join(" ", l);
    }

    private static String buildFreightRemarks(Bill obj, FreightDetail item) {
        List<String> l = new ArrayList<>();
        if (item.Remarks != null && !item.Remarks.trim().isEmpty()) l.add(item.Remarks.trim());
        if (item.AddLessAmount.compareTo(BigDecimal.ZERO) != 0) {
            l.add("Freight: " + fmt(item.Amount, "#,##0.###"));
            l.add("Add/Less: " + fmt(item.AddLessAmount, "#,##0.###"));
            l.add("Net Freight: " + fmt(item.NetAmount, "#,##0.###"));
        } else {
            l.add("Freight: " + fmt(item.NetAmount, "#,##0.###"));
        }
        List<String> v = distinctVehicles(obj);
        if (!v.isEmpty()) l.add("Against " + vehicleLabel(v) + ": " + String.join(", ", v));
        return String.join(" | ", l);
    }

    // ------------------------------------------------------------------------------ helpers

    private static List<String> distinctVehicles(Bill obj) {
        LinkedHashSet<String> set = new LinkedHashSet<>();
        for (Detail x : obj.InvCommAgentTradeBillDetailslist)
            if (x.VehicleNo != null && !x.VehicleNo.trim().isEmpty()) set.add(x.VehicleNo.trim());
        return new ArrayList<>(set);
    }

    private static String vehicleLabel(List<String> v) { return v.size() == 1 ? "Vehicle No" : "Vehicle Nos"; }

    private static boolean isLoadName(String t) {
        return "Load".equals(t) || "Load & PartyWeight".equals(t) || "Load & FactoryWeight".equals(t);
    }

    private static String s(String v) { return v == null ? "" : v; }

    /** C# decimal.ToString(): the value with its own scale (as carried by the BigDecimal). */
    static String dec(BigDecimal v) { return v == null ? "0" : v.toPlainString(); }

    /** C# double.ToString() (shortest round-trip, no trailing ".0"). */
    static String csDouble(double v) {
        if (v == Math.rint(v) && !Double.isInfinite(v) && Math.abs(v) < 1e15) return String.valueOf((long) v);
        return BigDecimal.valueOf(v).stripTrailingZeros().toPlainString();
    }

    /** .NET custom numeric format, rounding half away from zero. */
    static String fmt(BigDecimal v, String pattern) {
        DecimalFormat f = new DecimalFormat(pattern, DecimalFormatSymbols.getInstance(Locale.US));
        f.setRoundingMode(RoundingMode.HALF_UP);
        return f.format(v == null ? BigDecimal.ZERO : v);
    }

    private static String n2(BigDecimal v) { return fmt(v, "#,##0.00"); }
}
