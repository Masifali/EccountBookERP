package com.mst.services.cmagt;

import com.mst.models.cmagt.dto.TradeBillAgainstGdnCmagtDto;
import com.mst.repositories.cmagt.TradeBillAgainstGdnCmagtRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

@Service
public class TradeBillAgainstGdnCmagtService {

    @Autowired
    private TradeBillAgainstGdnCmagtRepository repository;

    @Autowired
    private CurrentUserContext currentUserContext;

    // ============================================================================== SAVE

    /** RecId > 0 -> update (btnUpdate_Click 5827), else insert (btnsave_Click 4865). */
    @org.springframework.transaction.annotation.Transactional(rollbackFor = Exception.class)
    public Map<String, Object> save(TradeBillAgainstGdnCmagtDto form) {
        return save(form, DOCUMENT_TYPE_ID);
    }

    @org.springframework.transaction.annotation.Transactional(rollbackFor = Exception.class)
    public Map<String, Object> save(TradeBillAgainstGdnCmagtDto form, int documentTypeId) {
        BillVariant variant = variant(documentTypeId);
        Map<String, Object> result = new LinkedHashMap<>();
        Map<String, Object> h = form.header == null ? new LinkedHashMap<>() : form.header;
        int recId = toInt(h.get("RecId"));
        if (!hasRight(recId > 0 ? RIGHT_UPDATE : RIGHT_SAVE, variant.screenName())) {
            return error(recId > 0 ? "You do not have Update rights on this screen."
                                   : "You do not have Save rights on this screen.");
        }
        if (recId > 0) {
            Map<String, Object> existing = repository.readById(recId);
            if (existing == null || !belongsToSession(existing, variant)) return error("RecId not Found");
        }
        TradeBillAgainstGdnCmagtDto.Bill obj;
        try {
            obj = buildBill(form, recId, variant);
        } catch (FormRefusal e) {
            return error(e.getMessage());
        }
        /* BLL Save (0547:760): MakeVoucher, ActionId, DAL SetData. Refusals thrown here roll back. */
        TradeBillVoucherBuilder.Result v = TradeBillVoucherBuilder.makeVoucher(obj,
                repository.glAccountsForFinancialEffects(obj.OrganizationId, obj.CompanyId));
        obj.ActionId = obj.Id == 0 ? 1 : 2;
        int id = repository.saveWithVoucher(obj, v.head, v.details);
        result.put("status", "SUCCESS");
        result.put("id", id);
        result.put("docNo", obj.DocNo);
        result.put("message", (recId > 0 ? "Record Update Successfully [ " : "Record Save Successfully  [ ")
                + obj.DocNo + " ] ");
        return result;
    }

    /** A refusal the desktop raises before the BLL is reached (MessageBox + return / throw). */
    public static class FormRefusal extends RuntimeException {
        public FormRefusal(String m) { super(m); }
    }

    private static Map<String, Object> error(String m) {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("status", "ERROR");
        r.put("message", m);
        return r;
    }

    /**
     * frmCommissionAgentTradeBillAgainstGdn.Insert() (4928-5684) - the object the desktop builds
     * from its controls, with every refusal in the same order and with the same words.
     * CommissionAgentAccoutValidation (1785) is not here: it catches its own exceptions and only
     * shows them, so it never stops a save (the page shows those warnings).
     */
    TradeBillAgainstGdnCmagtDto.Bill buildBill(TradeBillAgainstGdnCmagtDto form, int recId, BillVariant variant) {
        Map<String, Object> h = form.header;
        List<Map<String, Object>> rows = nz(form.rows);
        if (rows.isEmpty()) throw new FormRefusal("Grid Record Not Found");

        int org = currentUserContext.currentOrganizationId();
        int co = currentUserContext.currentCompanyId();

        formValidation(h, form);

        Map<Integer, Integer> dtSupplier = repository.partyGlAccounts(org, co);
        TradeBillAgainstGdnCmagtDto.Bill obj = new TradeBillAgainstGdnCmagtDto.Bill();
        obj.OrganizationId = org;
        obj.CompanyId = co;
        obj.BranchesId = currentUserContext.currentBranchId();
        obj.ProjectsId = currentUserContext.currentBranchId();       // desktop: ProjectsId = BranchesId
        obj.FinancialYearId = currentUserContext.currentFinancialYearId();
        obj.DocumentTypeId = variant.documentTypeId();
        obj.Id = recId;
        Timestamp now = new Timestamp(System.currentTimeMillis());
        obj.EnteryDate = now;
        obj.ModifyDate = now;
        obj.ApprovedDate = now;
        int user = currentUserContext.currentUserId();
        obj.EnteryUserId = user;
        obj.ModifyUserId = user;
        obj.ApprovalUserId = user;
        obj.IsApproved = Boolean.FALSE;
        LocalDate docDate = date(h.get("txtDocDate"));
        if (docDate == null) throw new FormRefusal("Doc Date is required");
        obj.DocDate = Timestamp.valueOf(docDate.atStartOfDay());
        obj.DocNo = toInt(h.get("TxtDocNo"));
        obj.BranchSrNo = toInt(h.get("txtBranchSrNo"));
        obj.TradingGlAccountId = toInt(h.get("CmbTradingAccount"));
        obj.RemarksHeader = str(h.get("txtMainRemarks")).trim();
        obj.SupplierId = toInt(h.get("CmbSupplier"));
        obj.DeliveryTermPurchaseId = toInt(h.get("CmbDeliveryTermPurchase"));
        obj.DeliveryTerm = str(h.get("CmbDeliveryTermPurchaseText"));
        obj.PaymentTermId = toInt(h.get("CmbPaymentTermPurchase"));
        obj.DueDays = toInt(h.get("txtDueDaysPurchase"));
        obj.DueDate = ts(h.get("txtDueDatePurchase"), docDate);
        obj.PurchaseTaxAccountId = toInt(h.get("CmbTaxAccountPurchase"));
        obj.PurchaseBillAmount = dbl(h.get("txtPurchaseBillAmount"));
        obj.CustomerId = toInt(h.get("CmbCustomer"));
        obj.DeliveryTermSaleId = toInt(h.get("CmbDeliveryTermSale"));
        obj.DeliveryTermSale = str(h.get("CmbDeliveryTermSaleText"));
        obj.SalePaymentTermId = toInt(h.get("CmbPaymentTermSale"));
        obj.SaleDueDays = toInt(h.get("txtDueDaysSale"));
        obj.SaleDueDate = ts(h.get("txtDueDateSale"), docDate);
        obj.SaleTaxAccountId = toInt(h.get("CmbTaxAccountSale"));
        obj.SaleBillAmount = dbl(h.get("txtSaleBillAmount"));
        obj.SupplierFirstWeight = dec(h.get("txtSupplierFirstWeight"));
        obj.SupplierSecondWeight = dec(h.get("txtSupplierSecondWeight"));
        obj.SupplierNetWeight = dec(h.get("txtSupplierWeight"));
        obj.SupplierBillWeight = dec(h.get("txtBillWeightGrn"));
        obj.BuyerBillWeight = dec(h.get("txtBillWeightGdn"));
        obj.BuyerFirstWeight = dec(h.get("txtBuyerFirstWeight"));
        obj.BuyerSecondWeight = dec(h.get("txtBuyerSecondWeight"));
        obj.BuyerNetWeight = dec(h.get("txtBuyerWeight"));
        obj.PLAmount = dbl(h.get("txtProfitLoss"));
        obj.ScreenName = variant.screenName();

        boolean isPurchaseDelivered = isPonch(obj.DeliveryTermPurchaseId, obj.DeliveryTerm);
        boolean isSaleDelivered = isPonch(obj.DeliveryTermSaleId, obj.DeliveryTermSale);
        boolean isSaleXFactory = isLoad(obj.DeliveryTermSaleId, obj.DeliveryTermSale);

        BigDecimal purchaseNet = dec(h.get("txtTotalFreightPurchase"));
        BigDecimal purchaseAmount = dec(h.get("txtFreightAmountPurchase"));
        BigDecimal purchaseAddLess = dec(h.get("txtFreightAddLessPurchase"));
        int purchaseAccId = toInt(h.get("CmbFreightAccountPurchase"));
        validateFreight(purchaseNet, purchaseAmount, purchaseAddLess, purchaseAccId, "Purchase");
        BigDecimal saleNet = dec(h.get("txtTotalFreightSale"));
        BigDecimal saleAmount = dec(h.get("txtFreightAmountSale"));
        BigDecimal saleAddLess = dec(h.get("txtFreightAddLessSale"));
        int saleAccId = toInt(h.get("CmbFreightAccountSale"));
        validateFreight(saleNet, saleAmount, saleAddLess, saleAccId, "Sale");

        BigDecimal tolerance = dec(repository.configValue(org, co, "PurchaseSaleFreightDiffTolerancePercent"));
        if (purchaseNet.signum() > 0 && saleNet.signum() > 0 && tolerance.signum() > 0) {
            BigDecimal toleranceAmount = purchaseNet.multiply(tolerance).divide(BigDecimal.valueOf(100), 10, RoundingMode.HALF_UP);
            if (purchaseNet.subtract(saleNet).abs().compareTo(toleranceAmount) > 0) {
                throw new FormRefusal("Freight difference exceeds allowed tolerance. \nPurchase: " + purchaseNet.toPlainString()
                        + ", Sale: " + saleNet.toPlainString() + ", \nTolerance: " + tolerance.toPlainString() + "%.");
            }
        }
        int purchaseFreightAccountGlId = gl(dtSupplier, purchaseAccId);
        int saleFreightAccountGlId = gl(dtSupplier, saleAccId);
        if (purchaseFreightAccountGlId > 0 && obj.TradingGlAccountId > 0 && purchaseFreightAccountGlId == obj.TradingGlAccountId)
            throw new FormRefusal("Purchase Freight Account cannot be same as Trading Account.");
        if (saleFreightAccountGlId > 0 && obj.TradingGlAccountId > 0 && saleFreightAccountGlId == obj.TradingGlAccountId)
            throw new FormRefusal("Sale Freight Account cannot be same as Trading Account.");

        int grnId = firstNonNull(rows, "GrnId");
        int gdnId = firstNonNull(rows, "GdnId");
        if (isPurchaseDelivered && purchaseNet.signum() > 0 && purchaseAccId > 0) {
            TradeBillAgainstGdnCmagtDto.FreightDetail c = new TradeBillAgainstGdnCmagtDto.FreightDetail();
            c.Id = 0; c.EntrySideId = 1; c.grnSupplierLoadingMasterId = grnId; c.gdnBuyerDispatchMasterId = 0;
            c.AccountId = purchaseAccId; c.Amount = purchaseAmount; c.AddLessAmount = purchaseAddLess; c.NetAmount = purchaseNet;
            c.Remarks = str(h.get("txtFreightRemarksPurchase"));
            obj.CommisionAgentBillFreightDetailList.add(c);
        }
        if (isSaleDelivered && saleNet.signum() > 0 && saleAccId > 0) {
            TradeBillAgainstGdnCmagtDto.FreightDetail c = new TradeBillAgainstGdnCmagtDto.FreightDetail();
            c.Id = 0; c.EntrySideId = 2; c.grnSupplierLoadingMasterId = 0; c.gdnBuyerDispatchMasterId = gdnId;
            c.AccountId = saleAccId; c.Amount = saleAmount; c.AddLessAmount = saleAddLess; c.NetAmount = saleNet;
            c.Remarks = str(h.get("txtFreightRemarksSale"));
            obj.CommisionAgentBillFreightDetailList.add(c);
        }

        BigDecimal purchaseTaxPercent = dec(h.get("txtWhtTaxPercentPurchase"));
        BigDecimal purchaseTaxAmount = dec(h.get("txtWhtTaxAmountPurchase"));
        int purchaseTaxAccId = toInt(h.get("CmbWhtAccountPurchase"));
        int purchaseTaxTypeId = toInt(h.get("CmbWhtTaxTypePurchase"));
        BigDecimal saleTaxPercent = dec(h.get("txtWhtTaxPercentSale"));
        BigDecimal saleTaxAmount = dec(h.get("txtwhtTaxAmountSale"));
        int saleTaxAccId = toInt(h.get("CmbWhtAccountSale"));
        int saleTaxTypeId = toInt(h.get("CmbWhtTaxTypeSale"));
        validateTax(purchaseTaxPercent, purchaseTaxAmount, purchaseTaxAccId, purchaseTaxTypeId, "Purchase");
        validateTax(saleTaxPercent, saleTaxAmount, saleTaxAccId, saleTaxTypeId, "Sale");
        int supplierAccountId = gl(dtSupplier, obj.SupplierId);
        int customerAccountId = gl(dtSupplier, obj.CustomerId);
        if (purchaseTaxAccId > 0 && supplierAccountId > 0 && purchaseTaxAccId == supplierAccountId)
            throw new FormRefusal("Purchase WHT/TAX Account cannot be the same as Supplier Account.");
        if (saleTaxAccId > 0 && customerAccountId > 0 && saleTaxAccId == customerAccountId)
            throw new FormRefusal("Sales WHT/TAX Account cannot be the same as Customer Account.");
        if (purchaseTaxAmount.signum() > 0 && purchaseTaxAccId > 0 && purchaseTaxTypeId > 0) {
            TradeBillAgainstGdnCmagtDto.TaxDetail t = new TradeBillAgainstGdnCmagtDto.TaxDetail();
            t.Id = 0; t.EntrySideId = 1; t.EntrySide = "Purchase"; t.TaxAccountId = purchaseTaxAccId;
            t.TaxAccountTitle = str(h.get("CmbWhtAccountPurchaseText")); t.TaxTypeId = purchaseTaxTypeId;
            t.TaxTypeName = str(h.get("CmbWhtTaxTypePurchaseText")); t.TaxPercantage = purchaseTaxPercent;
            t.TaxAmount = purchaseTaxAmount; t.Remarks = ""; t.sortNo = 1;
            obj.CommisionAgentBillTaxDetailList.add(t);
        }
        if (saleTaxAmount.signum() > 0 && saleTaxAccId > 0 && saleTaxTypeId > 0) {
            TradeBillAgainstGdnCmagtDto.TaxDetail t = new TradeBillAgainstGdnCmagtDto.TaxDetail();
            t.Id = 0; t.EntrySideId = 2; t.EntrySide = "Sale"; t.TaxAccountId = saleTaxAccId;
            t.TaxAccountTitle = str(h.get("CmbWhtAccountSaleText")); t.TaxTypeId = saleTaxTypeId;
            t.TaxTypeName = str(h.get("CmbWhtTaxTypeSaleText")); t.TaxPercantage = saleTaxPercent;
            t.TaxAmount = saleTaxAmount; t.Remarks = ""; t.sortNo = 2;
            obj.CommisionAgentBillTaxDetailList.add(t);
        }

        /* lstRemoveRecord first, then the grid rows (ActionTypeId 3 / 1 / 2). */
        for (Map<String, Object> r : nz(form.removedRows)) {
            int rid = toInt(r.get("Id"));
            if (rid <= 0 || recId <= 0) continue;
            TradeBillAgainstGdnCmagtDto.Detail d = new TradeBillAgainstGdnCmagtDto.Detail();
            d.Id = rid;
            d.ActionTypeId = 3;
            fillDetail(d, r);
            obj.InvCommAgentTradeBillDetailslist.add(d);
        }
        BigDecimal grossP = BigDecimal.ZERO, grossS = BigDecimal.ZERO, billP = BigDecimal.ZERO, billS = BigDecimal.ZERO;
        for (int i = 0; i < rows.size(); i++) {
            Map<String, Object> r = rows.get(i);
            TradeBillAgainstGdnCmagtDto.Detail d = new TradeBillAgainstGdnCmagtDto.Detail();
            d.Id = recId > 0 ? toInt(r.get("Id")) : 0;
            d.ActionTypeId = d.Id <= 0 ? 1 : 2;
            fillDetail(d, r);
            required(d.ItemId, "Item Name", i);
            required(d.Crop, "Crop Year", i);
            required(d.PackingTypeId, "Packing Type", i);
            required(d.PackUomId, "Pack Uom", i);
            required(d.Qty, "Loading Qty", i);
            required(d.VehicleNo, "Vehicle No", i);
            required(d.BiltyNo, "Bilty No", i);
            required(d.GrossWeight, "Purchase Gross Weight", i);
            required(d.BillWeight, "Purchase Net Bill Weight", i);
            required(d.RatePurchase, "Purchase Rate", i);
            required(d.RateUomId, "Purchase Rate UOM", i);
            required(d.ItemAmount, "Purchase Amount", i);
            required(d.SaleGrossWeight, "Sale Gross Weight", i);
            required(d.SaleBillWeight, "Sale Bill Weight", i);
            required(d.RateSale, "Sale Rate", i);
            required(d.SaleRateUomId, "Sale Rate UOM", i);
            required(d.SaleAmount, "Sale Amount", i);
            grossP = grossP.add(d.GrossWeight);
            grossS = grossS.add(d.SaleGrossWeight);
            billP = billP.add(d.BillWeight);
            billS = billS.add(d.SaleBillWeight);
            obj.InvCommAgentTradeBillDetailslist.add(d);
        }
        if (obj.SupplierNetWeight.compareTo(grossP) != 0)
            throw new FormRefusal("Supplier Net Weight:" + f3(obj.SupplierNetWeight) + " not equal to detail Purchase gross weight:" + f3(grossP) + ". please Check!");
        if (obj.BuyerNetWeight.compareTo(grossS) != 0)
            throw new FormRefusal("Buyer Net Weight:" + f3(obj.BuyerNetWeight) + " not equal to detail Sale gross weight:" + f3(grossS) + ". please Check!");
        if (obj.SupplierBillWeight.compareTo(billP) != 0)   // the desktop prints SupplierNetWeight here
            throw new FormRefusal("Supplier BillWeight:" + f3(obj.SupplierNetWeight) + " not equal to detail Purchase Bill weight:" + f3(billP) + ". please Check!");
        if (obj.BuyerBillWeight.compareTo(billS) != 0)
            throw new FormRefusal("Buyer BillWeight:" + f3(obj.BuyerBillWeight) + " not equal to detail Sale Bill weight:" + f3(billS) + ". please Check!");

        List<Map<String, Object>> pe = nz(form.purchaseOtherExpenses);
        for (int i = 0; i < pe.size(); i++) {
            Map<String, Object> r = pe.get(i);
            if (toInt(r.get("ItemId")) > 0 && toInt(r.get("Amount")) > 0) {
                TradeBillAgainstGdnCmagtDto.PurchaseExpense x = new TradeBillAgainstGdnCmagtDto.PurchaseExpense();
                x.grnSupplierLoadingMasterId = toInt(r.get("GrnId"));
                x.InvExpItemId = toInt(r.get("ItemId"));
                x.OtherItemName = str(r.get("ItemName"));
                x.Qty = dbl(r.get("Qty"));
                x.Rate = dbl(r.get("Rate"));
                x.Amount = dbl(r.get("Amount"));
                x.Remarks = str(r.get("Remarks")).trim();
                if ((x.Remarks.equals("0") || x.Remarks.isEmpty()) && dbl(r.get("Amount")) > 0.0)
                    x.Remarks = "[ItemName:" + x.OtherItemName + ", Qty:" + csd(x.Qty) + ", Rate:" + csd(x.Rate) + ", Amount:" + csd(x.Amount) + "]";
                obj.InvCommAgentTradePurchaseExpList.add(x);
            } else if (toInt(r.get("ItemId")) > 0 && toInt(r.get("Amount")) < 0) {
                throw new FormRefusal("Amount Can't be Less than 0 in Purchase Expense Grid in Row#" + (i + 1));
            }
        }
        obj.PurchaseExpenseDetailDescription = obj.InvCommAgentTradePurchaseExpList.isEmpty() ? "" :
                joinMap(obj.InvCommAgentTradePurchaseExpList, x -> "[ItemName:" + x.OtherItemName + ", Qty:" + csd(x.Qty)
                        + ", Rate:" + csd(x.Rate) + ", Amount:" + csd(x.Amount) + "]", ", ");

        List<Map<String, Object>> se = nz(form.saleOtherExpenses);
        for (int i = 0; i < se.size(); i++) {
            Map<String, Object> r = se.get(i);
            if (toInt(r.get("ItemId")) > 0 && dbl(r.get("Amount")) > 0.0) {
                TradeBillAgainstGdnCmagtDto.SaleExpense x = new TradeBillAgainstGdnCmagtDto.SaleExpense();
                x.gdnBuyerDispatchMasterId = toInt(r.get("GdnId"));
                x.InvExpItemId = toInt(r.get("ItemId"));
                x.OtherItemName = str(r.get("ItemName"));
                x.Qty = dbl(r.get("Qty"));
                x.Rate = dbl(r.get("Rate"));
                x.Amount = dbl(r.get("Amount"));
                x.Remarks = str(r.get("Remarks")).trim();
                if ((x.Remarks.equals("0") || x.Remarks.isEmpty()) && dbl(r.get("Amount")) > 0.0)
                    x.Remarks = "[ItemName:" + x.OtherItemName + ", Qty:" + csd(x.Qty) + ", Rate:" + csd(x.Rate) + ", Amount:" + csd(x.Amount) + "]";
                obj.InvCommAgentTradeSaleExpList.add(x);
            } else if (toInt(r.get("ItemId")) > 0 && toInt(r.get("Amount")) < 0) {
                throw new FormRefusal("Amount Can't be Less than 0 in Sale Expense Grid in Row#" + (i + 1));
            }
        }
        if (!obj.InvCommAgentTradeSaleExpList.isEmpty()) {
            obj.SaleExpenseDetailDescription = joinMap(obj.InvCommAgentTradeSaleExpList, x -> "[ItemName:" + x.OtherItemName
                    + ", Qty:" + csd(x.Qty) + ", Rate:" + csd(x.Rate) + ", Amount:" + csd(x.Amount) + "]", ", ");
        } else {
            /* Desktop 5270: the else branch clears the PURCHASE description, and the sale one
               stays null (so its parameter is not sent). Reproduced. */
            obj.PurchaseExpenseDetailDescription = "";
        }

        List<Map<String, Object>> spm = nz(form.salePmExpenses);
        for (int i = 0; i < spm.size(); i++) {
            Map<String, Object> r = spm.get(i);
            if (dbl(r.get("Amount")) > 0.0) {
                if (isSaleXFactory && toInt(r.get("AccountId")) == customerAccountId)
                    throw new FormRefusal("PM Sale Expense Account And Customer Gl Account Can't be Same in Row#" + (i + 1));
                TradeBillAgainstGdnCmagtDto.SaleExpenseCreditToReleventAc x = new TradeBillAgainstGdnCmagtDto.SaleExpenseCreditToReleventAc();
                x.gdnBuyerDispatchMasterId = toInt(r.get("GdnId"));
                x.AccountId = toInt(r.get("AccountId"));
                x.AccountTitle = str(r.get("AccountTitle"));
                if (x.AccountId == 0) throw new FormRefusal("Account is Required in Sale PM Expense Grid in Row#" + (i + 1));
                x.PackingTypeId = toInt(r.get("PackingTypeId"));
                x.PackingType = str(r.get("PackingType"));
                x.PmItemId = toInt(r.get("PmItemId"));
                x.PmItemName = str(r.get("PmItemName"));
                x.EBWeightDeductionTermId = toInt(r.get("EmptyBagTermId"));
                x.EBWeightDeductionTerm = str(r.get("EmptyBagTerm"));
                x.Qty = dbl(r.get("Qty"));
                x.Rate = dbl(r.get("Rate"));
                x.Amount = dbl(r.get("Amount"));
                x.Remarks = str(r.get("Remarks")).trim();
                if (x.Remarks.equals("0") || x.Remarks.isEmpty())
                    x.Remarks = pmRemarks(x.AccountTitle, x.PackingTypeId, x.PackingType, x.PmItemId, x.PmItemName,
                            x.EBWeightDeductionTermId, x.EBWeightDeductionTerm, x.Qty, x.Rate, x.Amount);
                obj.CommisionAgentBillSaleExpenseCreditToReleventAcsList.add(x);
            } else if (toInt(r.get("AccountId")) > 0 && toInt(r.get("Amount")) < 0) {
                throw new FormRefusal("Amount Can't be Less than 0 in Sale PM Expense Grid in Row#" + (i + 1));
            }
        }
        obj.SaleExpenseCreditToReleventAccountDetailDescription = obj.CommisionAgentBillSaleExpenseCreditToReleventAcsList.isEmpty() ? "" :
                joinMap(obj.CommisionAgentBillSaleExpenseCreditToReleventAcsList, x -> "[Account:" + x.AccountTitle + ", Qty:" + csd(x.Qty)
                        + ", Rate:" + csd(x.Rate) + ", Amount:" + csd(x.Amount) + "\nPackingType:" + x.PackingType + ", PmItem:"
                        + x.PmItemName + ", EmptyBagTerm:" + x.EBWeightDeductionTerm + "]", ", ");

        List<Map<String, Object>> ppm = nz(form.purchasePmExpenses);
        String purchaseTermText = obj.DeliveryTerm;
        for (int i = 0; i < ppm.size(); i++) {
            Map<String, Object> r = ppm.get(i);
            if (dbl(r.get("Amount")) > 0.0) {
                if (isPurchaseDelivered) {
                    if (toInt(r.get("AccountId")) == supplierAccountId)
                        throw new FormRefusal("Freight Account And Supplier Gl Account Can't be Same in Row# " + (i + 1)
                                + " \nBecause Purchase Delivery Term is " + purchaseTermText);
                } else if (toInt(r.get("AccountId")) == obj.TradingGlAccountId) {
                    throw new FormRefusal("Freight Account And TradingAccount Can't be Same in Row# " + (i + 1)
                            + " \nBecause Purchase Delivery Term is " + purchaseTermText);
                }
                TradeBillAgainstGdnCmagtDto.PurchaseFreightExpense x = new TradeBillAgainstGdnCmagtDto.PurchaseFreightExpense();
                x.grnSupplierLoadingMasterId = toInt(r.get("GrnId"));
                x.AccountId = toInt(r.get("AccountId"));
                x.AccountTitle = str(r.get("AccountTitle"));
                if (x.AccountId == 0) throw new FormRefusal("Account is Required in Purchase PM Expense Grid in Row#" + (i + 1));
                x.PackingTypeId = toInt(r.get("PackingTypeId"));
                x.PackingType = str(r.get("PackingType"));
                x.PmItemId = toInt(r.get("PmItemId"));
                x.PmItemName = str(r.get("PmItemName"));
                x.EBWeightDeductionTermId = toInt(r.get("EmptyBagTermId"));
                x.EBWeightDeductionTerm = str(r.get("EmptyBagTerm"));
                x.Qty = dec(r.get("Qty"));
                x.Rate = dec(r.get("Rate"));
                x.Amount = dec(r.get("Amount"));
                x.Remarks = str(r.get("Remarks")).trim();
                if (x.Remarks.equals("0") || x.Remarks.isEmpty())
                    x.Remarks = pmRemarksDec(x.AccountTitle, x.PackingTypeId, x.PackingType, x.PmItemId, x.PmItemName,
                            x.EBWeightDeductionTermId, x.EBWeightDeductionTerm, x.Qty, x.Rate, x.Amount);
                obj.InvCommAgentTradeFreightExpList.add(x);
            } else if (toInt(r.get("AccountId")) > 0 && toInt(r.get("Amount")) < 0) {
                throw new FormRefusal("Amount Can't be Less than 0 in Purchase Pm Expense Grid in Row#" + (i + 1));
            }
        }
        obj.PurchaseFreightExpenseDetailDescription = obj.InvCommAgentTradeFreightExpList.isEmpty() ? "" :
                joinMap(obj.InvCommAgentTradeFreightExpList, x -> "[Account:" + x.AccountTitle + ", Qty:" + x.Qty.toPlainString()
                        + ", Rate:" + x.Rate.toPlainString() + ", Amount:" + x.Amount.toPlainString() + "\nPackingType:" + x.PackingType
                        + ", PmItem:" + x.PmItemName + ", EmptyBagTerm:" + x.EBWeightDeductionTerm + "]", ", ");

        addCommissionOrBrokery(obj, form.purchaseCommission, "CommissionAgentId", "CommissionAmount", "CommissionTypeId", "CommissionUomId", "CommissionRate", 1, 1, "PoId");
        addCommissionOrBrokery(obj, form.saleCommission, "CommissionAgentId", "CommissionAmount", "CommissionTypeId", "CommissionUomId", "CommissionRate", 1, 2, "SoId");
        addCommissionOrBrokery(obj, form.purchaseBrokery, "BrokeryAgentId", "BrokeryAmount", "BrokeryTypeId", "BrokeryUomId", "BrokeryRate", 2, 1, "PoId");
        addCommissionOrBrokery(obj, form.saleBrokery, "BrokeryAgentId", "BrokeryAmount", "BrokeryTypeId", "BrokeryUomId", "BrokeryRate", 2, 2, "SoId");
        obj.PurchaseCommisionDetailDescription = commissionDescription(obj, 1, 1, "Agent");
        obj.SaleCommisionDetailDescription = commissionDescription(obj, 2, 1, "Agent");
        obj.PurchaseBrokeryDetailDescription = commissionDescription(obj, 1, 2, "Broker");
        obj.SaleBrokeryDetailDescription = commissionDescription(obj, 2, 2, "Broker");

        obj.CommisionAgentBillPaymentDetailList = new ArrayList<>();
        paymentDetails(obj, nz(form.purchasePayment), 1, h, docDate);
        paymentDetails(obj, nz(form.salePayment), 2, h, docDate);
        return obj;
    }

    /** FormValidation (1809-1927). */
    private void formValidation(Map<String, Object> h, TradeBillAgainstGdnCmagtDto form) {
        String docNo = str(h.get("TxtDocNo")).trim();
        if (docNo.isEmpty() || docNo.equals("0")) throw new FormRefusal("DocNo Field is Required");
        String br = str(h.get("txtBranchSrNo")).trim();
        if (br.isEmpty() || br.equals("0")) throw new FormRefusal("Branch Doc No Field is Required");
        if (toInt(h.get("CmbTradingAccount")) == 0) throw new FormRefusal("TradingAccount Field is Required");
        if (toInt(h.get("CmbSupplier")) == 0) throw new FormRefusal("Supplier Field is Required");
        if (toInt(h.get("CmbDeliveryTermPurchase")) == 0) throw new FormRefusal("Delivery Term Field is Required");
        if (sum(form.purchasePayment, "Amount").signum() == 0) {
            if (toInt(h.get("CmbPaymentTermPurchase")) == 0) throw new FormRefusal("Payment Term Field is Required");
            if ("Credit".equals(str(h.get("CmbPaymentTermPurchaseText"))) && toInt(h.get("txtDueDaysPurchase")) == 0)
                throw new FormRefusal("Due Days Field Required");
        }
        if (toInt(h.get("CmbCustomer")) == 0) throw new FormRefusal("Customer Field is Required");
        if (toInt(h.get("CmbDeliveryTermSale")) == 0) throw new FormRefusal("Delivery Term Sale Field is Required");
        if (sum(form.salePayment, "Amount").signum() == 0) {
            if (toInt(h.get("CmbPaymentTermSale")) == 0) throw new FormRefusal("Payment Term Sale Field is Required");
            if ("Credit".equals(str(h.get("CmbPaymentTermSaleText"))) && toInt(h.get("txtDueDaysSale")) == 0)
                throw new FormRefusal("Due Days Sale Field Required");
        }
        if (toInt(h.get("CmbSupplier")) == toInt(h.get("CmbCustomer")))
            throw new FormRefusal("Supplier Ac and Customer  Ac cannot be same");
        if (sum(form.rows, "PurchaseTaxAmount").signum() > 0 && toInt(h.get("CmbTaxAccountPurchase")) == 0)
            throw new FormRefusal("Purchase Tax Account field is required");
        if (sum(form.rows, "SaleTaxAmount").signum() > 0 && toInt(h.get("CmbTaxAccountSale")) == 0)
            throw new FormRefusal("Sale Tax Account field is required");
        if (dbl(h.get("txtBuyerWeight")) == 0.0) throw new FormRefusal("Buyer Wb NetWeight Should Be Greater Than 0");
        if (dbl(h.get("txtSupplierWeight")) == 0.0) throw new FormRefusal("Supplier Wb NetWeight Should Be Greater Than 0");
        if (dbl(h.get("txtPurchaseBillAmount")) == 0.0) throw new FormRefusal("Purchase Bill Amount Should Be Greater Than 0");
        if (dbl(h.get("txtSaleBillAmount")) == 0.0) throw new FormRefusal("Sale Bill Amount Should Be Greater Than 0");
    }

    /** ValidateFreight (4878). */
    private static void validateFreight(BigDecimal net, BigDecimal amount, BigDecimal addLess, int accId, String type) {
        if (net.signum() > 0 || amount.signum() > 0) {
            if (accId <= 0) throw new FormRefusal(type + " Freight Account is required.");
            if (amount.signum() < 0) throw new FormRefusal(type + " Freight Amount cannot be negative.");
            if (addLess.abs().compareTo(amount) > 0) throw new FormRefusal(type + " Freight Add/Less is not logical.");
            if (net.subtract(amount.add(addLess)).abs().compareTo(new BigDecimal("0.01")) > 0)
                throw new FormRefusal(type + " Freight Net Amount mismatch.");
            if (net.signum() == 0) throw new FormRefusal(type + " Freight Net Amount cannot be zero.");
        }
    }

    /** ValidateTax (4905). */
    private static void validateTax(BigDecimal percent, BigDecimal amount, int accId, int taxTypeId, String side) {
        if (percent.signum() > 0 || amount.signum() > 0 || taxTypeId > 0) {
            if (amount.signum() <= 0) throw new FormRefusal(side + " Tax amount must be greater than zero.");
            if (percent.signum() <= 0) throw new FormRefusal(side + " Tax percent must be greater than zero.");
            if (accId <= 0) throw new FormRefusal(side + " Tax account is required.");
            if (taxTypeId <= 0) throw new FormRefusal(side + " Tax type is required.");
        }
    }

    /** FillDetailListCommonForInsertAndDelete (4735). */
    private static void fillDetail(TradeBillAgainstGdnCmagtDto.Detail d, Map<String, Object> r) {
        d.gdnBuyerDispatchMasterId = toInt(r.get("GdnId"));
        d.gdnBuyerDispatchDetailId = toInt(r.get("GdnDetailId"));
        d.grnSupplierLoadingMasterId = toInt(r.get("GrnId"));
        d.grnSupplierLoadingDetailId = toInt(r.get("GrnDetailId"));
        d.purchaseOrderMasterId = toInt(r.get("PoId"));
        d.purchaseOrderDetailId = toInt(r.get("PoDetailId"));
        d.saleOrderMasterId = toInt(r.get("SoId"));
        d.saleOrderDetailId = toInt(r.get("SoDetailId"));
        d.ItemId = toInt(r.get("ItemId"));
        d.ItemName = str(r.get("ItemName"));
        d.Crop = toInt(r.get("CropYearId"));
        d.CropYear = str(r.get("CropYear"));
        d.PackingTypeId = toInt(r.get("PackTypeId"));
        d.PackTypeDesc = str(r.get("PackTypeDesc"));
        d.PackUomId = toInt(r.get("UomId"));
        d.PackUom = str(r.get("UomCode"));
        d.Qty = dec(r.get("ItemQty"));
        d.RefDocNo = str(r.get("RefDocNo"));
        d.VehicleNo = str(r.get("VehicleNo"));
        d.BiltyNo = str(r.get("BiltyNo"));
        d.GrossWeight = dec(r.get("PurchaseGrossWeight"));
        d.EbCut = dbl(r.get("PurchaseEbCut"));
        d.EbCutTotal = dbl(r.get("PurchaseEbCutTotal"));
        d.AddLessPurchase = dec(r.get("PurchaseAddLessWeight"));
        d.BillWeight = dec(r.get("PurchaseBillWeight"));
        d.PurchaseRateWithoutAddLess = dec(r.get("PurchaseRateWithoutAddLess"));
        d.PurchaseRateAddLess = dec(r.get("PurchaseRateAddLess"));
        d.RatePurchase = dec(r.get("PurchaseNetRate"));
        d.RateUomId = toInt(r.get("PurchaseRateUomId"));
        d.ItemAmount = dbl(r.get("PurchaseAmount"));
        d.PurchaseExpenses = dbl(r.get("PurchaseExpenses"));
        d.PurchaseFreight = dec(r.get("PurchaseFreight"));
        d.PurchaseBrokery = dec(r.get("PurchaseBrokery"));
        d.PurchaseCommission = dec(r.get("PurchaseCommission"));
        d.PurchaseFreightLess = dbl(r.get("PurchaseFreightLess"));
        d.TaxNameIdPurchase = toInt(r.get("PurchaseTaxNameId"));
        d.TaxNamePurchase = str(r.get("PurchaseTaxName"));
        d.TaxPercentPurchase = dec(r.get("PurchaseTax%"));
        d.TaxAmountPurchase = dec(r.get("PurchaseTaxAmount"));
        d.PurchaseNetAmount = dbl(r.get("PurchaseNetAmount"));
        d.SaleGrossWeight = dec(r.get("SaleGrossWeight"));
        d.SaleEbCut = dbl(r.get("SaleEbCut"));
        d.SaleEbCutTotal = dbl(r.get("SaleEbCutTotal"));
        d.AddLessSale = dec(r.get("SaleAddLess"));
        d.SaleBillWeight = dec(r.get("SaleBillWeight"));
        d.SaleRateWithoutAddLess = dec(r.get("SaleRateWithoutAddLess"));
        d.SaleRateAddLess = dec(r.get("SaleRateAddLess"));
        d.RateSale = dec(r.get("SaleNetRate"));
        d.SaleRateUomId = toInt(r.get("SaleRateUomId"));
        d.SaleAmount = dbl(r.get("SaleAmount"));
        d.SaleExpenses = dbl(r.get("SaleExpenses"));
        d.SaleCommission = dec(r.get("SaleCommission"));
        d.SaleBrokery = dec(r.get("SaleBrokery"));
        d.TaxNameIdSale = toInt(r.get("SaleTaxNameId"));
        d.TaxNameSale = str(r.get("SaleTaxName"));
        d.TaxPercentSale = dec(r.get("SaleTax%"));
        d.TaxAmountSale = dec(r.get("SaleTaxAmount"));
        d.SaleNetAmount = dbl(r.get("SaleNetAmount"));
        d.RemarksDetail = str(r.get("RemarksDetail")).trim();
    }

    /** AddCommissionOrBrokery (4805). */
    private static void addCommissionOrBrokery(TradeBillAgainstGdnCmagtDto.Bill obj, List<Map<String, Object>> grid,
                                               String agentCol, String amountCol, String typeCol, String uomCol,
                                               String rateCol, int agentTypeId, int entrySideId, String orderCol) {
        for (Map<String, Object> r : nz(grid)) {
            BigDecimal commAmount = dec(r.get(amountCol));
            int agentId = toInt(r.get(agentCol));
            String agentText = str(r.get("AgentText"));
            if (agentId <= 0 || commAmount.signum() <= 0) continue;
            TradeBillAgainstGdnCmagtDto.CommissionDetail c = new TradeBillAgainstGdnCmagtDto.CommissionDetail();
            if (entrySideId == 1) c.purchaseOrderMasterId = toInt(r.get(orderCol));
            else c.saleOrderMasterId = toInt(r.get(orderCol));
            c.commissionAgentId = agentId;
            c.CommissionAgentName = agentText;
            c.commissionTypeId = toInt(r.get(typeCol));
            c.CommissionType = str(r.get("TypeText"));
            if (c.commissionTypeId == 0) throw new FormRefusal("Commission Type not found");
            c.rateUomId = toInt(r.get(uomCol));
            c.RateUom = (double) toInt(r.get("UomText"));                 // Conversion.ToInt(cell.Text)
            c.commissionRate = dec(r.get(rateCol));
            if (c.commissionRate.signum() == 0) throw new FormRefusal("Rate not found");
            c.commissionAmount = commAmount;
            c.debitAccountId = toInt(r.get("DebitAccountId"));
            c.AccountTitle = str(r.get("DebitAccountText"));
            c.agentTypeId = agentTypeId;
            c.EntrySideId = entrySideId;
            List<String> parts = new ArrayList<>();
            parts.add((agentTypeId == 1 ? "Agent" : "Broker") + ":" + agentText);
            parts.add("CommissionType:" + c.CommissionType);
            if (c.commissionTypeId == 3) parts.add("UOM:" + csd(c.RateUom));
            parts.add("Rate:" + c.commissionRate.toPlainString());
            parts.add("Amount:" + c.commissionAmount.toPlainString());
            if (c.debitAccountId > 0) parts.add("Debit Account:" + c.AccountTitle);
            c.commissionRemarks = "[" + String.join(", ", parts) + "]";
            obj.CommisionAgentBillCommissionDetailList.add(c);
        }
    }

    private static String commissionDescription(TradeBillAgainstGdnCmagtDto.Bill obj, int side, int agentType, String label) {
        if (obj.CommisionAgentBillCommissionDetailList.isEmpty()) return "";
        List<String> out = new ArrayList<>();
        for (TradeBillAgainstGdnCmagtDto.CommissionDetail x : obj.CommisionAgentBillCommissionDetailList) {
            if (x.EntrySideId != side || x.agentTypeId != agentType) continue;
            out.add("[" + label + ":" + x.CommissionAgentName + ", Type:" + x.CommissionType + ", "
                    + (x.commissionTypeId == 3 ? "Uom:" + csd(x.RateUom) + ", " : "") + "Rate:" + x.commissionRate.toPlainString()
                    + ", Amount:" + x.commissionAmount.toPlainString()
                    + (x.debitAccountId > 0 ? ", DebitAccount:" + x.AccountTitle : "") + "]");
        }
        return String.join(", ", out);
    }

    /** The purchase (side 1, 5476-5557) and sale (side 2, 5558-5642) payment blocks. */
    private void paymentDetails(TradeBillAgainstGdnCmagtDto.Bill obj, List<Map<String, Object>> grid, int side,
                                Map<String, Object> h, LocalDate docDate) {
        boolean p = side == 1;
        BigDecimal billAmount = BigDecimal.valueOf(p ? obj.PurchaseBillAmount : obj.SaleBillAmount);
        BigDecimal gridAmount = sum(grid, "Amount");
        BigDecimal paymentDetailAmount = BigDecimal.ZERO;
        BigDecimal prcnt = BigDecimal.ZERO;
        BigDecimal diff = billAmount.subtract(gridAmount);
        String orderCol = p ? "PoId" : "SoId";
        if (gridAmount.signum() > 0) {
            int count = grid.size();
            for (int i = 0; i < grid.size(); i++) {
                Map<String, Object> r = grid.get(i);
                BigDecimal due = dec(r.get("Amount"));
                BigDecimal paymentDue = (count > 1 && diff.compareTo(BigDecimal.ONE) <= 0) ? due.add(diff) : due;
                TradeBillAgainstGdnCmagtDto.PaymentDetail pd = new TradeBillAgainstGdnCmagtDto.PaymentDetail();
                pd.EntrySideId = side;
                int orderId = toInt(r.get(orderCol));
                if (p) { pd.purchaseOrderMasterId = orderId; pd.PurchaseOrderNo = toInt(r.get("PoNo")); }
                else   { pd.saleOrderMasterId = orderId; pd.SaleOrderNo = toInt(r.get("SoNo")); }
                pd.PaymentTermId = toInt(r.get("PaymentTermId"));
                pd.PaymentTerm = str(r.get("PaymentTermText"));
                pd.DueDays = toInt(r.get("DueDays"));
                pd.pctOfTotal = dec(r.get("%OfTotal"));
                if (pd.pctOfTotal.signum() == 0) throw new FormRefusal("%Of Total not found");
                pd.dueAmount = count == 1 ? billAmount : paymentDue;
                pd.BaseDueDateTypeId = toInt(r.get("BaseDateTypeId"));
                if (!p) pd.BaseDateType = str(r.get("BaseDateTypeText"));
                pd.DueDate = pd.BaseDueDateTypeId == 1 ? Timestamp.valueOf(docDate.plusDays(pd.DueDays).atStartOfDay())
                        : pd.BaseDueDateTypeId == 4 ? ts(r.get("DueDate"), null) : null;
                double orderPct = 0;
                for (Map<String, Object> o : grid) if (toInt(o.get(orderCol)) == orderId) orderPct += dbl(o.get("%OfTotal"));
                if (orderPct != 100.0) {
                    throw new FormRefusal(p ? "Percent Of Purcahse Order:" + pd.PurchaseOrderNo + " not equal to 100. Please Check!"
                                            : "Percent Of Sale Order:" + pd.SaleOrderNo + " not equal to 100. Please Check!");
                }
                prcnt = prcnt.add(pd.pctOfTotal);
                paymentDetailAmount = paymentDetailAmount.add(pd.dueAmount);
                diff = BigDecimal.ZERO;
                if (pd.PaymentTermId <= 0) throw new FormRefusal("Payment Term Required in row#" + (i + 1));
                if (pd.PaymentTermId == 2 && pd.DueDays <= 0) throw new FormRefusal("Due Days Required In case Of Credit row in row#" + (i + 1));
                obj.CommisionAgentBillPaymentDetailList.add(pd);
            }
        } else {
            TradeBillAgainstGdnCmagtDto.PaymentDetail pd = new TradeBillAgainstGdnCmagtDto.PaymentDetail();
            pd.EntrySideId = side;
            pd.PaymentTermId = toInt(h.get(p ? "CmbPaymentTermPurchase" : "CmbPaymentTermSale"));
            pd.PaymentTerm = str(h.get(p ? "CmbPaymentTermPurchaseText" : "CmbPaymentTermSaleText"));
            pd.DueDays = toInt(h.get(p ? "txtDueDaysPurchase" : "txtDueDaysSale"));
            pd.BaseDueDateTypeId = 1;
            pd.DueDate = Timestamp.valueOf(docDate.plusDays(pd.DueDays).atStartOfDay());
            pd.pctOfTotal = BigDecimal.valueOf(100);
            pd.dueAmount = billAmount;
            paymentDetailAmount = pd.dueAmount;
            prcnt = BigDecimal.valueOf(100);
            obj.CommisionAgentBillPaymentDetailList.add(pd);
        }
        if (billAmount.subtract(paymentDetailAmount).abs().compareTo(new BigDecimal("0.3")) > 0) {
            throw new FormRefusal((p ? "Purchase Payment Detail Amount" : "Sale Payment Detail Amount")
                    + TradeBillVoucherBuilder.fmt(paymentDetailAmount, "#,##0.####")
                    + (p ? " Not Equal to Total Supplier Amount" : " Not Equal to Total Customer Amount")
                    + TradeBillVoucherBuilder.fmt(billAmount, "#,##0.####"));
        }
        prcnt = prcnt.setScale(4, RoundingMode.HALF_EVEN);
        if (BigDecimal.valueOf(100).subtract(prcnt).abs().compareTo(new BigDecimal("0.01")) > 0) {
            throw new FormRefusal(p ? "Payment Detail Total% not near to 100" : "Sale Payment Detail Total% not near to 100");
        }
        List<String> desc = new ArrayList<>();
        for (TradeBillAgainstGdnCmagtDto.PaymentDetail x : obj.CommisionAgentBillPaymentDetailList) {
            if (x.EntrySideId != side) continue;
            desc.add("[Payment Term:" + s(x.PaymentTerm) + ", Days:" + x.DueDays + ", %OfTotal:" + x.pctOfTotal.toPlainString()
                    + " ,Amount:" + x.dueAmount.toPlainString() + ", BaseDateType:" + s(x.BaseDateType) + "]");
        }
        String d = obj.CommisionAgentBillPaymentDetailList.isEmpty() ? "" : String.join(", ", desc);
        if (p) obj.PurchasePaymentDetailDescription = d; else obj.SalePaymentDetailDescription = d;
    }

    private static String pmRemarks(String account, int packId, String pack, int pmId, String pm, int ebId, String eb,
                                    double qty, double rate, double amount) {
        List<String> parts = new ArrayList<>();
        parts.add("Account:" + account);
        if (packId > 0) parts.add("PackingType:" + pack);
        if (pmId > 0) parts.add("PmItem:" + pm);
        if (ebId > 0) parts.add("EmptyBagTerm:" + eb);
        if (qty > 0.0) parts.add("Qty:" + csd(qty));
        if (rate > 0.0) parts.add("Rate:" + csd(rate));
        if (amount > 0.0) parts.add("Amount:" + csd(amount));
        return "[" + String.join(", ", parts) + "]";
    }

    private static String pmRemarksDec(String account, int packId, String pack, int pmId, String pm, int ebId, String eb,
                                       BigDecimal qty, BigDecimal rate, BigDecimal amount) {
        List<String> parts = new ArrayList<>();
        parts.add("Account:" + account);
        if (packId > 0) parts.add("PackingType:" + pack);
        if (pmId > 0) parts.add("PmItem:" + pm);
        if (ebId > 0) parts.add("EmptyBagTerm:" + eb);
        if (qty.signum() > 0) parts.add("Qty:" + qty.toPlainString());
        if (rate.signum() > 0) parts.add("Rate:" + rate.toPlainString());
        if (amount.signum() > 0) parts.add("Amount:" + amount.toPlainString());
        return "[" + String.join(", ", parts) + "]";
    }

    /** FormHelper.ValidateField (FormHelper.cs:503). */
    private static void required(Object value, String field, int rowIndex) {
        boolean bad = value == null
                || (value instanceof Integer && (Integer) value == 0)
                || (value instanceof Double && (Double) value <= 0.0)
                || (value instanceof BigDecimal && ((BigDecimal) value).signum() <= 0)
                || (value instanceof String && ((String) value).trim().isEmpty());
        if (bad) throw new FormRefusal(field + " is required in Detail Grid at row No: " + (rowIndex + 1));
    }

    /** FormHelper.DeliveryTermIsPonch / DeliveryTermIsLoad (FormHelper.cs:52-68). */
    static boolean isPonch(int id, String name) {
        return id == 2 || id == 5 || id == 6
                || "Ponch".equals(name) || "Ponch & PartyWeight".equals(name) || "Ponch & FactoryWeight".equals(name);
    }

    static boolean isLoad(int id, String name) {
        return id == 1 || id == 3 || id == 4
                || "Load".equals(name) || "Load & PartyWeight".equals(name) || "Load & FactoryWeight".equals(name);
    }

    private static int gl(Map<Integer, Integer> dt, int id) {
        Integer v = dt.get(id);
        return v == null ? 0 : v;
    }

    private static int firstNonNull(List<Map<String, Object>> rows, String col) {
        for (Map<String, Object> r : rows) if (r.get(col) != null) return toInt(r.get(col));
        return 0;
    }

    private static BigDecimal sum(List<Map<String, Object>> rows, String col) {
        BigDecimal t = BigDecimal.ZERO;
        for (Map<String, Object> r : nz(rows)) t = t.add(dec(r.get(col)));
        return t;
    }

    private static <T> String joinMap(List<T> list, java.util.function.Function<T, String> f, String sep) {
        List<String> out = new ArrayList<>();
        for (T t : list) out.add(f.apply(t));
        return String.join(sep, out);
    }

    private static List<Map<String, Object>> nz(List<Map<String, Object>> l) { return l == null ? new ArrayList<>() : l; }

    private static String s(String v) { return v == null ? "" : v; }

    private static String str(Object v) { return v == null ? "" : String.valueOf(v); }

    private static String csd(double v) { return TradeBillVoucherBuilder.csDouble(v); }

    private static String f3(BigDecimal v) { return TradeBillVoucherBuilder.fmt(v, "#,##0.###"); }

    /** Conversion.ToDecimal of a control's text: thousands separators allowed, blank -> 0, scale kept. */
    static BigDecimal dec(Object v) {
        if (v == null) return BigDecimal.ZERO;
        if (v instanceof BigDecimal) return (BigDecimal) v;
        if (v instanceof Integer || v instanceof Long) return BigDecimal.valueOf(((Number) v).longValue());
        if (v instanceof Number) return BigDecimal.valueOf(((Number) v).doubleValue());
        String t = String.valueOf(v).replace(",", "").trim();
        if (t.isEmpty()) return BigDecimal.ZERO;
        try { return new BigDecimal(t); } catch (NumberFormatException e) { return BigDecimal.ZERO; }
    }

    static double dbl(Object v) { return dec(v).doubleValue(); }

    private static LocalDate date(Object v) {
        String t = str(v).trim();
        if (t.length() < 10) return null;
        try { return LocalDate.parse(t.substring(0, 10)); } catch (Exception e) { return null; }
    }

    private static Timestamp ts(Object v, LocalDate fallback) {
        LocalDate d = date(v);
        if (d == null) d = fallback;
        return d == null ? null : Timestamp.valueOf(d.atStartOfDay());
    }

    // ============================================================================ lookups

    /**
     * DatatableHelper.GetAccountsFromGlobalByTypeIds (DatatableHelper.cs:109-172) over
     * USP_GETAllAccountsFromCustomGroups. Every caller on this form:
     *   trading       withTypeIds {10}                                 (TradingAccountsBindFromGlobal)
     *   tax           withTypeIds {19}                                 (TaxAccountsBindFromGlobal)
     *   whtPurchase   withCustomGroupIds {CustomGroupForWHTAccounts}
     *   whtSale       withCustomGroupIds {CustomGroupForWHTAccountsSale}
     *   purchasePm    withoutTypeIds {2,11,12,13,14,15,20,21,22}      (PurchasePmExpenseGridComboBind)
     *   salePm        no filter                                        (PmGridCombBind)
     *   commissionDr  withoutTypeIds {2,4,15}, withClassIds {2,3,4,5}  (Grid*CommissionComboBind)
     * First row per ChartOfAccountId, as the desktop's seenIds does.
     */
    public Map<String, List<Map<String, Object>>> accountLists() {
        int org = currentUserContext.currentOrganizationId();
        int co = currentUserContext.currentCompanyId();
        List<Map<String, Object>> all = repository.allAccountsWithCustomGroup(org, co);
        int whtP = toInt(repository.configValue(org, co, "CustomGroupForWHTAccounts"));
        int whtS = toInt(repository.configValue(org, co, "CustomGroupForWHTAccountsSale"));
        Map<String, List<Map<String, Object>>> out = new LinkedHashMap<>();
        out.put("trading", filterAccounts(all, set(10), null, null, null));
        out.put("tax", filterAccounts(all, set(19), null, null, null));
        out.put("whtPurchase", filterAccounts(all, null, null, null, set(whtP)));
        out.put("whtSale", filterAccounts(all, null, null, null, set(whtS)));
        out.put("purchasePm", filterAccounts(all, null, set(2, 11, 12, 13, 14, 15, 20, 21, 22), null, null));
        out.put("salePm", filterAccounts(all, null, null, null, null));
        out.put("commissionDr", filterAccounts(all, null, set(2, 4, 15), set(2, 3, 4, 5), null));
        return out;
    }

    private static java.util.Set<Integer> set(int... v) {
        java.util.Set<Integer> s = new java.util.HashSet<>();
        for (int i : v) s.add(i);
        return s;
    }

    private static List<Map<String, Object>> filterAccounts(List<Map<String, Object>> all, java.util.Set<Integer> withTypes,
                                                           java.util.Set<Integer> withoutTypes, java.util.Set<Integer> withClass,
                                                           java.util.Set<Integer> withGroups) {
        List<Map<String, Object>> out = new ArrayList<>();
        java.util.Set<Integer> seen = new java.util.HashSet<>();
        for (Map<String, Object> a : all) {
            int type = toInt(pick(a, "AccountTypeId"));
            if (withTypes != null && !withTypes.contains(type)) continue;
            if (withoutTypes != null && withoutTypes.contains(type)) continue;
            if (withClass != null && !withClass.contains(toInt(pick(a, "AccountClass")))) continue;
            if (withGroups != null && !withGroups.contains(toInt(pick(a, "CustomGroupId")))) continue;
            int id = toInt(pick(a, "ChartOfAccountId"));
            if (!seen.add(id)) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", id);
            m.put("AccountTitle", pick(a, "AccountTitle"));
            m.put("AccountCode", pick(a, "AccountCode"));
            m.put("ParentAccountTitle", pick(a, "ParentAccountTitle"));
            m.put("AccountClass", pick(a, "AccountClassName"));
            out.add(m);
        }
        return out;
    }

    /** DatatableHelper.TaxTypesDbCall - CmbWhtTaxTypePurchase. */
    public List<Map<String, Object>> taxTypes() {
        return repository.taxTypes(currentUserContext.currentOrganizationId(), currentUserContext.currentCompanyId());
    }

    /** PurchaseTaxCalculation - TaxPercent of the first schedule row, else 0. */
    public Map<String, Object> whtPercent(int taxNameId, String docDate) {
        LocalDate d = date(docDate);
        List<Map<String, Object>> rows = repository.taxSchedulePercent(currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(), d == null ? null : java.sql.Date.valueOf(d), taxNameId);
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("taxPercent", rows.isEmpty() ? 0 : pick(rows.get(0), "TaxPercent"));
        return r;
    }

    /** SaleTaxTypesBind - CmbWhtTaxTypeSale (rows carry TaxPercent, read by SaleTaxCalculation). */
    public List<Map<String, Object>> saleTaxTypes(int customerId, String docDate) {
        LocalDate d = date(docDate);
        return repository.taxScheduleBySupplierCustomer(currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(), customerId, d == null ? null : java.sql.Date.valueOf(d));
    }

    /** GetConfigurationsFromGlobalandBind + GetCommissionAgentConfigurationsFromGlobalandBind. */
    public Map<String, Object> configs() {
        int org = currentUserContext.currentOrganizationId();
        int co = currentUserContext.currentCompanyId();
        String[] names = {"PurchaseSaleFreightDiffTolerancePercent", "DefaultDaysToLessFromHistoryFromDate",
                "DefaultWhtAccountPurchaseIdForCommissionAgentPortal", "DefaultWhtAccountSaleIdForCommissionAgentPortal",
                "DefaultTaxTypeSaleIdForCommissionAgentPortal", "DefaultTaxTypePurchaseeIdForCommissionAgentPortal",
                "ItemSearchByCode", "DisableFreightFieldsOnBillFromGdn", "EnableSaleAddLessWeightOnBillFromGdn",
                "DefaultTradingAccountIdForCommissionAgentPortal", "DefaultTaxAccountIdForCommissionAgentPortal",
                "DefaultPaymentTermIdForCommissionAgentPortal", "DefaultDeliveryTermIdForCommissionAgentPortal",
                "DefaultCropYearIdForCommissionAgentPortal", "DefaultPackingTypeIdForCommissionAgentPortal",
                "Default NoofDecimal Points For Amount"};
        Map<String, Object> out = new LinkedHashMap<>();
        for (String n : names) {
            String v;
            try { v = repository.configValue(org, co, n); } catch (Exception e) { v = null; }
            out.put(n, v);
        }
        return out;
    }

    /** frmPendingGdnLoader ComboDbCall. */
    public List<Map<String, Object>> loaderDropdowns() {
        return repository.loaderDropdowns(currentUserContext.currentOrganizationId(), currentUserContext.currentCompanyId());
    }

    /** frmPendingGdnLoader PendingDataDbCall - DocumentTypeId 1055 (the loader's own, :123). */
    public List<List<Map<String, Object>>> pendingGdn(String fromDate, String toDate, int fromDocNo, int toDocNo,
                                                      int commissionAgentId, int supplierId, int buyerId, int itemId,
                                                      int deliverToPartyId, String shipToAddress) {
        LocalDate f = date(fromDate), t = date(toDate);
        return repository.pendingGdnForBill(currentUserContext.currentOrganizationId(), currentUserContext.currentCompanyId(),
                currentUserContext.currentBranchId(), currentUserContext.currentFinancialYearId(), 1055,
                f == null ? null : java.sql.Date.valueOf(f), t == null ? null : java.sql.Date.valueOf(t),
                fromDocNo, toDocNo, commissionAgentId, supplierId, buyerId, itemId, deliverToPartyId, shipToAddress);
    }


    /** frmCommissionAgentTradeBillAgainstGdn line 882. */
    public static final int DOCUMENT_TYPE_ID = 1056;

    /** frmCommissionAgentTradeBillAgainstGdn line 883 - the key its rights are resolved against. */
    public static final String DESKTOP_SCREEN_NAME = "frmCommissionAgentTradeBillAgainstGdn";

    /* Right names exactly as CommonServices.SetRightsValueInRightsObject compares them
       (Architecture.WinApp.Common/CommonServices.cs 17596-17660) - note the space. */
    private static final String RIGHT_CAN_VIEW_ALL_RECORDS = "CanView AllRecord";
    private static final String RIGHT_DELETE = "Delete";
    private static final String RIGHT_SAVE = "Save";
    private static final String RIGHT_UPDATE = "Update";

    @Autowired
    private com.mst.repositories.cmagt.SaleOrderCmagtRepository rightsRepo;

    /**
     * HistoryGridFill (5943-6039): org, company, branch, year and DocumentTypeId from the
     * session; CanViewAllRecord from the screen's rights; EntryUser pinned to the current user
     * when that right is absent.
     */
    public List<Map<String, Object>> getHistory(String fromDate, String toDate,
                                                Integer docNoFrom, Integer docNoTo,
                                                Integer tradingAccountId, Integer supplierId,
                                                Integer customerId) {
        return getHistory("doc", fromDate, toDate, docNoFrom, docNoTo, tradingAccountId, supplierId, customerId, DOCUMENT_TYPE_ID);
    }

    /** dateKind: doc | entry | modify - the drdocdate / rdentrydate / rdmodifydate radios. */
    public List<Map<String, Object>> getHistory(String dateKind, String fromDate, String toDate,
                                                Integer docNoFrom, Integer docNoTo,
                                                Integer tradingAccountId, Integer supplierId,
                                                Integer customerId) {
        return getHistory(dateKind,fromDate,toDate,docNoFrom,docNoTo,tradingAccountId,supplierId,customerId,DOCUMENT_TYPE_ID);
    }

    public List<Map<String, Object>> getHistory(String dateKind, String fromDate, String toDate,
                                                Integer docNoFrom, Integer docNoTo,
                                                Integer tradingAccountId, Integer supplierId,
                                                Integer customerId, int documentTypeId) {
        BillVariant variant=variant(documentTypeId);
        boolean all = hasRight(RIGHT_CAN_VIEW_ALL_RECORDS,variant.screenName());
        return repository.formHistory(
                currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(),
                currentUserContext.currentBranchId(),
                currentUserContext.currentFinancialYearId(),
                variant.documentTypeId(), all,
                all ? null : currentUserContext.currentUserId(),
                dateKind, fromDate, toDate, docNoFrom, docNoTo, tradingAccountId, supplierId, customerId);
    }

    /** HistoryComboDbCall / HistoryComboBind (6228-6296). */
    public List<Map<String, Object>> historyCombos() {
        return repository.historyCombos(currentUserContext.currentOrganizationId(), currentUserContext.currentCompanyId());
    }

    /** ActiveYr.Start_Period for the "Financial Year" date type. */
    public Map<String, Object> yearStart() {
        Object v = repository.financialYearStart(currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(), currentUserContext.currentFinancialYearId());
        Map<String, Object> r = new LinkedHashMap<>();
        String t = v == null ? null : String.valueOf(v);
        r.put("financialYearStart", t == null ? null : t.substring(0, Math.min(10, t.length())));
        return r;
    }

    /**
     * ReadById + the nine child collections. The procedure reads by @Id alone; the desktop can
     * only reach ids its own company's history returned, the web can be sent any id - so the
     * row's OrganizationId/CompanyId/DocumentTypeId are checked against the session, and a
     * user without "CanView AllRecord" may open only their own bills (the same set History
     * shows them).
     */
    public Map<String, Object> getById(Integer id) {
        return getById(id,DOCUMENT_TYPE_ID);
    }

    public Map<String, Object> getById(Integer id,int documentTypeId) {
        BillVariant variant=variant(documentTypeId);
        Map<String, Object> result = new LinkedHashMap<>();
        Map<String, Object> header = id == null ? null : repository.readById(id);
        if (header == null || !belongsToSession(header,variant)) {
            result.put("status", "ERROR");
            result.put("message", "Record not found");
            return result;
        }
        if (!hasRight(RIGHT_CAN_VIEW_ALL_RECORDS,variant.screenName())
                && toInt(header.get("EnteryUserId")) != currentUserContext.currentUserId()) {
            result.put("status", "ERROR");
            result.put("message", "You do not have permission to open Trade Bills entered by another user.");
            return result;
        }
        result.put("status", "SUCCESS");
        result.put("data", header);
        return result;
    }

    /** DocumentNoDbCall (1149) and BranchSrNoDbCall (1175). */
    public Map<String, Object> generateCodes() {
        return generateCodes(DOCUMENT_TYPE_ID);
    }

    public Map<String, Object> generateCodes(int documentTypeId) {
        BillVariant variant=variant(documentTypeId);
        int org = currentUserContext.currentOrganizationId();
        int co = currentUserContext.currentCompanyId();
        int yr = currentUserContext.currentFinancialYearId();
        int br = currentUserContext.currentBranchId();
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("docNo", repository.generateCode("GenerateCode", org, co, variant.documentTypeId(), yr, br));
        r.put("branchSrNo", repository.generateCode("GenerateBranchSrCode", org, co, variant.documentTypeId(), yr, br));
        return r;
    }

    /**
     * BtnDelete_Click (5843-5864) -> BLL DeleteByID(UserAccount.ID, RecId), inside one
     * transaction as the BLL opens one. The button is enabled by formright.DoHaveCanDelete (953).
     */
    @org.springframework.transaction.annotation.Transactional
    public Map<String, Object> deleteById(Integer id) {
        return deleteById(id,DOCUMENT_TYPE_ID);
    }

    @org.springframework.transaction.annotation.Transactional
    public Map<String, Object> deleteById(Integer id,int documentTypeId) {
        BillVariant variant=variant(documentTypeId);
        Map<String, Object> result = new LinkedHashMap<>();
        if (id == null || id == 0) {
            result.put("status", "ERROR");
            result.put("message", "Record Id Not Found");
            return result;
        }
        if (!hasRight(RIGHT_DELETE,variant.screenName())) {
            result.put("status", "ERROR");
            result.put("message", "You do not have Delete rights on this screen.");
            return result;
        }
        Map<String, Object> header = repository.readById(id);
        if (header == null || !belongsToSession(header,variant)) {
            result.put("status", "ERROR");
            result.put("message", "Record not found");
            return result;
        }
        repository.deleteById(currentUserContext.currentUserId(), id);
        result.put("status", "SUCCESS");
        return result;
    }

    private boolean belongsToSession(Map<String, Object> h) { return belongsToSession(h,variant(DOCUMENT_TYPE_ID)); }

    private boolean belongsToSession(Map<String, Object> h,BillVariant variant) {
        return toInt(h.get("OrganizationId")) == currentUserContext.currentOrganizationId()
                && toInt(h.get("CompanyId")) == currentUserContext.currentCompanyId()
                && toInt(h.get("DocumentTypeId")) == variant.documentTypeId();
    }

    /**
     * SetRightsValueInRightsObject: role "Admin" starts with every right; then each row of the
     * user's grant grid overrides. For "CanView AllRecord" an Admin stays true; for "Delete"
     * the row wins even for Admin (the desktop has no Admin guard on that branch).
     */
    /** formright flags the form applies at :951-954 (btnsave/btnUpdate/BtnDelete/btnPrint .Enabled). */
    public Map<String, Boolean> formRights() {
        return formRights(DOCUMENT_TYPE_ID);
    }

    public Map<String, Boolean> formRights(int documentTypeId) {
        BillVariant variant=variant(documentTypeId);
        Map<String, Boolean> r = new LinkedHashMap<>();
        r.put("save", hasRight(RIGHT_SAVE,variant.screenName()));
        r.put("update", hasRight(RIGHT_UPDATE,variant.screenName()));
        r.put("delete", hasRight(RIGHT_DELETE,variant.screenName()));
        r.put("print", hasRight("Print",variant.screenName()));
        r.put("canViewAllRecords", hasRight(RIGHT_CAN_VIEW_ALL_RECORDS,variant.screenName()));
        return r;
    }

    private boolean hasRight(String rightName) {
        return hasRight(rightName,DESKTOP_SCREEN_NAME);
    }

    private boolean hasRight(String rightName,String screenName) {
        String role = currentUserContext.currentRoleName();
        boolean admin = "Admin".equalsIgnoreCase(role) || "Administrator".equalsIgnoreCase(role);
        boolean value = admin;
        try {
            for (Map<String, Object> r : rightsRepo.userRightsForScreen(
                    currentUserContext.currentUserId(), screenName, role,
                    currentUserContext.currentCompanyId())) {
                Object name = pick(r, "RightName");
                if (name != null && rightName.equalsIgnoreCase(name.toString().trim())) {
                    if (admin && (RIGHT_CAN_VIEW_ALL_RECORDS.equals(rightName) || RIGHT_SAVE.equals(rightName)
                            || RIGHT_UPDATE.equals(rightName))) return true;
                    value = toBool(pick(r, "Value"));
                    break;
                }
            }
        } catch (Exception ignored) {
            // An unreadable grant grid must not become an implicit grant.
            return admin && RIGHT_CAN_VIEW_ALL_RECORDS.equals(rightName);
        }
        return value;
    }

    private static BillVariant variant(int documentTypeId) {
        return switch(documentTypeId) {
            case 1056 -> new BillVariant(1056,DESKTOP_SCREEN_NAME);
            case 160 -> new BillVariant(160,"frmCommissionAgentTradeBill");
            case 162 -> new BillVariant(162,"CommissionAgentTradeBill_162");
            default -> throw new IllegalArgumentException("Unsupported commission agent trade bill document type.");
        };
    }
    private record BillVariant(int documentTypeId,String screenName) {}

    private static Object pick(Map<String, Object> row, String key) {
        if (row.containsKey(key)) return row.get(key);
        for (Map.Entry<String, Object> e : row.entrySet())
            if (e.getKey().equalsIgnoreCase(key)) return e.getValue();
        return null;
    }

    private static boolean toBool(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        return v != null && ("1".equals(v.toString().trim()) || "true".equalsIgnoreCase(v.toString().trim()));
    }

    private static int toInt(Object v) {
        if (v instanceof Number) return ((Number) v).intValue();
        if (v == null) return 0;
        String t = v.toString().replace(",", "").trim();
        if (t.isEmpty()) return 0;
        try { return (int) Double.parseDouble(t); } catch (Exception e) { return 0; }
    }
}
