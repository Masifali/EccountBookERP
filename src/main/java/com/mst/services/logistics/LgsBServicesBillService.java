package com.mst.services.logistics;

import com.mst.models.UserAccount;
import com.mst.models.logistics.LgsBServicesBillDetail;
import com.mst.models.logistics.LgsBServicesBillFreightDetail;
import com.mst.models.logistics.LgsBServicesBillHeader;
import com.mst.models.logistics.LgsBVoucherDetail;
import com.mst.models.logistics.LgsBVoucherHead;
import com.mst.repositories.logistics.LgsBRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.services.hrm.HrmSupport.*;
import static com.mst.services.logistics.LgsBPurchaseOrderService.combo;
import static com.mst.services.logistics.LgsBPurchaseOrderService.field;
import static com.mst.services.logistics.LgsBPurchaseOrderService.nonZeroDouble;
import static com.mst.services.logistics.LgsBPurchaseOrderService.nonZeroInt;
import static com.mst.services.logistics.LgsBSupport.*;

/**
 * 955 Logistic Purchase Services Bill - Architecture.WinApp.Service.lgstcm.frmLogisticPurchaseServicesBill
 * (DocumentTypeId 1304), with its loaders frmLoadPurchaseOrderForServicesBill (Load PO) and
 * frmLoadPendingFreightVoucherExportForServicesBill (Load Freight Voucher). BLL lgstcm.ServicesBillHeader (0388), DAL (0345).
 *
 * Save = BLL Save: MakeVoucher (party GL from Sp_SupplierCustomer_GetAllMethod, item purchase GL from Sp_Item_GetAllMethod),
 * then DAL SetData in one transaction: [lgstcm].[USP_ServicesBillHeader_InsertAndUpdate], [lgstcm].[USP_ServicesBillDetail_Insert]
 * per row, [lgstcm].[USP_ServicesBillFreightVoucherDetail_Insert] per freight row, [lgstcm].[USP_Purchase_ValidateWithInvoice]
 * when an invoice is set, then the voucher (LgsBVoucherPoster, no balance check in this DAL).
 */
@Service
public class LgsBServicesBillService {

    public static final int SCREEN = 955;
    public static final String SCREEN_NAME = "frmLogisticPurchaseServicesBill";
    public static final int DOC_TYPE = 1304;

    @Autowired private LgsBSupport sup;
    @Autowired private LgsBRepository repo;
    @Autowired private LgsBVoucherPoster poster;

    // ================================================================== load

    public Map<String, Object> setup() {
        UserAccount u = sup.user(SCREEN);
        Map<String, Object> out = lists(u);
        out.put("rights", sup.rights(u, SCREEN, SCREEN_NAME));
        out.put("docNo", repo.sbGenerateCode(u, sup.fy(), DOC_TYPE));
        out.put("historyCombos", repo.sbDropDown(u, String.valueOf(DOC_TYPE)));
        out.put("yearStart", sup.yearStart(u));
        return out;
    }

    /** btnRefresh_Click. */
    public Map<String, Object> refresh() { return lists(sup.user(SCREEN)); }

    /**
     * AllLookUpComboBind (+ dtDebitAccounts = GetAccountsFromGlobalByTypeIds(6, 8, 10)), SupplierDtFillFromGlobal (every
     * party, no group filter on this form), CurrencyBindFromGlobal, service items, cities, configuration.
     */
    private Map<String, Object> lists(UserAccount u) {
        return map("lookups", sup.lookUps(u), "suppliers", sup.suppliers(u, false), "currencies", sup.currencies(u),
                "items", sup.serviceItems(u), "cities", sup.cities(u), "debitAccounts", sup.accountsOfTypes(sup.accounts(u), 6, 8, 10),
                "config", config(u));
    }

    private Map<String, Object> config(UserAccount u) {
        Map<String, Object> c = sup.formats(u);
        c.put("defaultTransportServiceItemId", toInt(sup.config(u, "DefaultTranspotationServiceItemId")));
        c.put("defaultDays", toInt(sup.config(u, "DefaultDaysToLessFromHistoryFromDate")));
        c.put("itemSearchByCode", toBool(sup.config(u, "ItemSearchByCode")));
        c.put("foreignBaseCurrency", toInt(sup.config(u, "ForeignBaseCurrency")));
        c.put("fcyBaseCurrencyRate", toDouble(sup.config(u, "FcyBaseCurrencyRate")));
        c.put("localBaseCurrency", toInt(sup.config(u, "Base Currency")));
        c.put("localBaseCurrencyRate", toDouble(sup.config(u, "BaseCurrencyRate")));
        c.put("taxPercentEditable", toBool(sup.config(u, "TaxPercentEditable")));
        c.put("multiCurrency", sup.feature(u, 6));
        return c;
    }

    public List<Map<String, Object>> ports() { return sup.ports(sup.user(SCREEN)); }

    public List<Map<String, Object>> tax(int itemId, String docDate) {
        return LgsBPurchaseOrderService.taxOf(repo, sup.user(SCREEN), itemId, docDate);
    }

    public List<Map<String, Object>> taxByItems(String itemIds, String docDate) {
        UserAccount u = sup.user(SCREEN);
        LocalDateTime d = toDate(docDate);
        return repo.taxForItems(u, itemIds, d == null ? LocalDateTime.now() : d);
    }

    /** LogisticsServicesBill_Helper.ShowInvoiceInfo (hover card). */
    public Map<String, Object> invoiceInfo(int invoiceId) {
        UserAccount u = sup.user(SCREEN);
        List<Map<String, Object>> r = repo.invoiceInfo(u, invoiceId);
        return r.isEmpty() ? map("found", false) : map("found", true, "row", r.get(0));
    }

    // ================================================================== read

    public Map<String, Object> read(int id) {
        UserAccount u = sup.user(SCREEN);
        Map<String, Object> h = ownHeader(u, id);
        return map("header", h, "detail", detailRows(repo.sbDetail(id), false), "freight", freightRows(repo.sbFreightDetail(id)));
    }

    private Map<String, Object> ownHeader(UserAccount u, int id) {
        List<Map<String, Object>> r = repo.sbHeader(id);
        if (r.isEmpty() || !ownRow(u, r.get(0))) throw invalid("No Record Found");
        return r.get(0);
    }

    /** LogisticsServicesBill_Helper.FillDetailFromListCommonForReadById (AddExtraColumns adds DebitAccountTitle for the history). */
    static List<Map<String, Object>> detailRows(List<Map<String, Object>> rows, boolean extra) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : rows) {
            int lt = toInt(d.get("LocationTypeId"));
            Map<String, Object> m = map("Id", toInt(d.get("ServicesBillDetailId")), "ChargesTypeId", toInt(d.get("ChargesTypeId")),
                    "AgreementHeaderId", toInt(d.get("AgreementHeaderId")), "AgreementDetailId", toInt(d.get("AgreementDetailId")),
                    "AgreementNo", toInt(d.get("AgreementDocNo")),
                    "PurchaseOrderHeaderId", toInt(d.get("PurchaseOrderHeaderId")), "PurchaseOrderDetailId", toInt(d.get("PurchaseOrderDetailId")),
                    "PurchaseOrderNo", toInt(d.get("PurchaseOrderDocNo")),
                    "PartyRefDocNo", str(d.get("partyRefDocNo")), "PartyRefDocDate", d.get("partyRefDocDate"),
                    "ItemId", toInt(d.get("serviceItemId")), "ServiceItemCode", str(d.get("ItemCode")), "ServiceItemName", str(d.get("ItemName")),
                    "TransactionCurrencyId", toInt(d.get("transactionCurrencyId")), "TransactionCurrency", str(d.get("TransactionCurrencyCode")),
                    "TransactionExchangeRate", toDouble(d.get("transactionExchangeRate")), "ServiceRate", toDouble(d.get("ServiceRate")),
                    "RateBaseId", toInt(d.get("RateBaseUomId")), "RateBase", str(d.get("RateBaseUom")), "RateBaseEquivalent", toDouble(d.get("RateBaseUomEquivalent")),
                    "Qty", toDouble(d.get("Qty")), "NetWeight", toDouble(d.get("NetWeight")),
                    "TcyAmount", toDouble(d.get("tcyAmount")), "LcyAmount", toDouble(d.get("lcyAmount")),
                    "TaxNameId", toInt(d.get("TaxNameId")), "TaxName", str(d.get("TaxName")), "Tax%", toDouble(d.get("TaxPercent")),
                    "TaxAmount", toDouble(d.get("TaxAmount")), "TotalTcyAmount", toDouble(d.get("TotalAmount")),
                    "DebitAccountId", toInt(d.get("DebitAccountId")),
                    "LocationTypeId", lt, "LocationType", str(d.get("LocationType")),
                    "LocationFromId", toInt(d.get(lt == 16 ? "LoadingPortId" : "FromCityId")), "LocationFrom", str(d.get(lt == 16 ? "LoadingPort" : "FromCity")),
                    "LocationToId", toInt(d.get(lt == 16 ? "DestinationPortId" : "ToCityId")), "LocationTo", str(d.get(lt == 16 ? "DestinationPort" : "ToCity")),
                    "Remarks", str(d.get("Remarks")));
            if (extra) m.put("DebitAccountTitle", str(d.get("DebitAccountTitle")));
            out.add(m);
        }
        return out;
    }

    /** FillFreightVoucherDetailFromListCommonForReadById: dtFreightDetail. */
    static List<Map<String, Object>> freightRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : rows) {
            out.add(map("Id", toInt(d.get("ServicesBillFreightVoucherDetailId")), "FreightVoucherOutwardId", toInt(d.get("FreightVoucherOutwardId")),
                    "GatePassOutwardId", toInt(d.get("GatePassOutwardId")), "DeliveryOrderId", toInt(d.get("DeliveryOrderId")),
                    "Qty", toDouble(d.get("Qty")), "Weight", toDouble(d.get("Weight")), "FreightRate", toDouble(d.get("FreightRate")),
                    "RateBaseUomId", toInt(d.get("RateBaseUomId")), "QtyForRate", toDouble(d.get("QtyForRate")),
                    "BiltyFreight", toDouble(d.get("BiltyFreight")), "OtherCharges", toDouble(d.get("OtherCharges")),
                    "TotalBiltyFreight", toDouble(d.get("TotalBiltyFreight")), "ThisRowFreightAmount", toDouble(d.get("ThisRowFreightAmount")),
                    "Remarks", str(d.get("Remarks"))));
        }
        return out;
    }

    // ================================================================== save

    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = sup.user(SCREEN);
        int recId = Math.max(0, i(b, "recId"));
        sup.require(u, SCREEN, recId > 0 ? "Update" : "Save");
        List<Map<String, Object>> rows = list(b, "rows");
        List<Map<String, Object>> removed = list(b, "removed");
        List<Map<String, Object>> freight = list(b, "freight");
        if (rows.isEmpty()) throw invalid("Detail Record Not Found");
        nonZeroInt(s(b, "docNo"), "Doc No");
        combo(i(b, "serviceTypeId"), "Service Type");
        combo(i(b, "paymentTermId"), "Payment Term");
        nonZeroInt(s(b, "dueDays"), "Due Days");
        combo(i(b, "fcyCurrencyId"), "Foreign Currency");
        nonZeroDouble(s(b, "fcyExchangeRate"), "Fcy Rate");
        if (sup.feature(u, 6)) {
            combo(i(b, "glcyCurrencyId"), "Glcy Currency");
            nonZeroDouble(s(b, "glcyRate"), "Glcy Rate");
            nonZeroDouble(s(b, "glcyAmount"), "Glcy Amount");
        }
        Map<String, Object> existing = recId > 0 ? ownHeader(u, recId) : null;
        LocalDateTime now = LocalDateTime.now();
        LocalDateTime docDate = toDate(b.get("docDate"));
        if (docDate == null) docDate = now;

        LgsBServicesBillHeader h = new LgsBServicesBillHeader();
        h.ServicesBillHeaderId = recId;
        h.CompanyId = u.getCompanyId();
        h.BranchesId = toInt(u.getBranchesId());
        h.ProjectsId = toInt(u.getBranchesId());
        h.OrganizationId = u.getOrganizationId();
        h.FinancialYearId = sup.fy();
        h.DocumentTypeId = DOC_TYPE;
        h.DocumentDate = docDate;
        h.DocumentNo = toInt(s(b, "docNo"));
        h.serviceTypeId = i(b, "serviceTypeId");
        h.ExportInvoiceId = i(b, "exportInvoiceId");
        String exportInvoiceNo = s(b, "exportInvoiceNo");
        /* ExImInvoice.GetInvoiceInformationByInvoiceIdForServices(ExportInvoiceId) -> CustomerName (for the voucher remarks). */
        String exportInvoiceCustomer = "";
        List<Map<String, Object>> inv = repo.invoiceInfo(u, h.ExportInvoiceId);
        if (!inv.isEmpty()) exportInvoiceCustomer = str(inv.get(0).get("CustomerName"));
        h.BilltoPartyId = i(b, "billToPartyId");
        h.BrokerOrServiceProviderId = i(b, "serviceProviderId");
        h.ReferenceNo = s(b, "referenceNo");
        h.paymentTermId = i(b, "paymentTermId");
        h.DueDays = toInt(s(b, "dueDays"));
        h.fcyCurrencyId = i(b, "fcyCurrencyId");
        h.fcyExchangeRate = d(b, "fcyExchangeRate");
        h.RemarksHeader = s(b, "remarks");
        Set<Integer> fvIds = new LinkedHashSet<>();
        for (Object o : (b.get("freightVoucherIds") instanceof List ? (List<?>) b.get("freightVoucherIds") : new ArrayList<>())) {
            int id = toInt(o);
            if (id != 0) fvIds.add(id);
        }
        for (int fv : fvIds) {
            List<Map<String, Object>> f = repo.fvById(fv);
            if (f.isEmpty() || !ownRow(u, f.get(0))) throw invalid("No Record Found");
        }
        h.FreightVoucherOutwardIds = String.join(",", fvIds.stream().map(String::valueOf).toArray(String[]::new));
        h.GlcyCurrencyId = i(b, "glcyCurrencyId");
        h.GlcyExchangeRate = d(b, "glcyRate");
        h.GlcyAmount = d(b, "glcyAmount");
        h.EntryUserId = u.getId();
        h.EntryDate = now;
        h.ModifyDate = now;
        h.ModifyUserId = u.getId();
        h.ApprovedUserId = u.getId();
        h.ApprovedDate = now;
        h.IsApproved = false;
        h.ActionId = recId == 0 ? 1 : 2;
        h.AttachmentsValues = existing == null ? "" : str(existing.get("AttachmentsValues"));
        h.CustomAttachmentsValues = existing == null ? "" : str(existing.get("CustomAttachmentsValues"));
        boolean noFreight = h.FreightVoucherOutwardIds.isEmpty();

        List<LgsBServicesBillDetail> details = new ArrayList<>();
        List<Integer> own = new ArrayList<>();
        if (recId > 0) for (Map<String, Object> d : repo.sbDetail(recId)) own.add(toInt(d.get("ServicesBillDetailId")));
        if (recId > 0) {
            for (Map<String, Object> r : removed) {
                int did = i(r, "Id");
                if (did <= 0 || !own.contains(did)) continue;
                LgsBServicesBillDetail d = detail(b, r);
                d.ServicesBillDetailId = did;
                d.ActionTypeId = 3;
                details.add(d);
            }
        }
        boolean poExists = false;
        Set<Integer> poHeaders = new LinkedHashSet<>();
        for (int n = 0; n < rows.size(); n++) {
            Map<String, Object> r = rows.get(n);
            LgsBServicesBillDetail vd = detail(b, r);
            vd.ServicesBillDetailId = recId != 0 ? i(r, "Id") : 0;
            if (vd.ServicesBillDetailId > 0 && !own.contains(vd.ServicesBillDetailId)) throw invalid("No Record Found");
            vd.ActionTypeId = vd.ServicesBillDetailId <= 0 ? 1 : 2;
            if (noFreight) field(vd.partyRefDocNo, "Party Ref DocNo", n);
            field(vd.serviceItemId, "Service Item", n);
            field(vd.transactionCurrencyId, "Transaction Currency", n);
            field(vd.transactionExchangeRate, "Transaction Exchange Rate", n);
            if (noFreight) {
                field(vd.ServiceRate, "Rate", n);
                field(vd.RateBaseUomId, "Rate Base", n);
            }
            field(vd.Qty, "Qty", n);
            field(vd.NetWeight, "NetWeight", n);
            field(vd.tcyAmount, "Tcy amount", n);
            field(vd.lcyAmount, "Lcy amount", n);
            if (vd.TaxNameId > 0 || vd.TaxPercent > 0.0 || vd.TaxAmount > 0.0) {
                field(vd.TaxNameId, "Tax Name", n);
                field(vd.TaxPercent, "Tax Percent", n);
                field(vd.TaxAmount, "Tax Amount", n);
            }
            field(vd.TotalAmount, "Total Tcy Amount", n);
            if (!noFreight) field(vd.DebitAccountId, "Debit Account", n);
            if (noFreight) {
                field(vd.LocationTypeId, "Location Type", n);
                if (vd.LocationTypeId == 16) {
                    field(vd.LoadingPortId, "Loading Port", n);
                    field(vd.DestinationPortId, "Destination Port", n);
                    if (vd.LoadingPortId == vd.DestinationPortId)
                        throw invalid("Loading Port And Destination Port Can't be Same in Detail  at row No: " + (n + 1));
                }
                if (vd.LocationTypeId == 17) {
                    field(vd.FromCityId, "From City", n);
                    field(vd.ToCityId, "To City", n);
                    if (vd.FromCityId == vd.ToCityId) throw invalid("FromCity And ToCity Can't be Same in Detail  at row No: " + (n + 1));
                }
            }
            if (h.RemarksHeader == null || h.RemarksHeader.trim().isEmpty()) h.RemarksHeader = vd.Remarks;
            if (h.transactionCurrencyId == 0) {
                h.transactionCurrencyId = vd.transactionCurrencyId;
                h.transactionExchangeRate = vd.transactionExchangeRate;
            }
            if (!poExists) poExists = vd.PurchaseOrderHeaderId > 0 && vd.PurchaseOrderDetailId > 0;
            if (vd.PurchaseOrderHeaderId > 0) poHeaders.add(vd.PurchaseOrderHeaderId);
            details.add(vd);
        }
        if (noFreight && !poExists) throw invalid("At least one detail row must be from a Purchase Order (PO).");
        for (int po : poHeaders) {
            List<Map<String, Object>> ph = repo.poHeader(po);
            if (ph.isEmpty() || !ownRow(u, ph.get(0))) throw invalid("No Record Found");
        }
        List<LgsBServicesBillFreightDetail> fds = new ArrayList<>();
        for (Map<String, Object> r : freight) {
            LgsBServicesBillFreightDetail f = new LgsBServicesBillFreightDetail();
            f.ServicesBillFreightVoucherDetailId = recId != 0 ? i(r, "Id") : 0;
            f.FreightVoucherOutwardId = i(r, "FreightVoucherOutwardId");
            f.GatePassOutwardId = i(r, "GatePassOutwardId");
            f.DeliveryOrderId = i(r, "DeliveryOrderId");
            f.Qty = d(r, "Qty");
            f.Weight = d(r, "Weight");
            f.FreightRate = d(r, "FreightRate");
            f.RateBaseUomId = i(r, "RateBaseUomId");
            f.QtyForRate = d(r, "QtyForRate");
            f.BiltyFreight = d(r, "BiltyFreight");
            f.OtherCharges = d(r, "OtherCharges");
            f.TotalBiltyFreight = d(r, "TotalBiltyFreight");
            f.ThisRowFreightAmount = d(r, "ThisRowFreightAmount");
            f.Remarks = s(r, "Remarks");
            if (!fvIds.contains(f.FreightVoucherOutwardId)) throw invalid("No Record Found");
            fds.add(f);
        }

        /* BLL Save: MakeVoucher before the DAL runs. */
        LgsBVoucherPoster.Voucher v = makeVoucher(u, h, details, exportInvoiceNo, exportInvoiceCustomer);
        int id = repo.tx(() -> {
            if (details.isEmpty()) throw invalid("Detail list not found");
            int n = repo.set("[lgstcm].[USP_ServicesBillHeader_InsertAndUpdate]", h);
            h.ServicesBillHeaderId = n > 0 ? n : h.ServicesBillHeaderId;
            for (LgsBServicesBillDetail d : details) {
                d.ServicesBillHeaderId = h.ServicesBillHeaderId;
                repo.exec("[lgstcm].[USP_ServicesBillDetail_Insert]", d);
            }
            for (LgsBServicesBillFreightDetail f : fds) {
                f.ServicesBillHeaderId = h.ServicesBillHeaderId;
                f.ExImInvoiceId = h.ExportInvoiceId;
                repo.exec("[lgstcm].[USP_ServicesBillFreightVoucherDetail_Insert]", f);
            }
            if (h.ExportInvoiceId > 0) {
                repo.exec("[lgstcm].[USP_Purchase_ValidateWithInvoice]", LgsBRepository.p("ExportInvoiceId", h.ExportInvoiceId, "DocumentTypeId", DOC_TYPE,
                        "CurrentDocId", h.ServicesBillHeaderId > 0 ? h.ServicesBillHeaderId : null));
            }
            poster.post(u, v.head, v.lines, h.ServicesBillHeaderId, false);
            return h.ServicesBillHeaderId;
        });
        return map("success", true, "id", id, "message", (recId > 0 ? "Data Update Successfully....  " : "Data Save Successfully....  ") + h.DocumentNo,
                "docNo", repo.sbGenerateCode(u, sup.fy(), DOC_TYPE));
    }

    /**
     * ServicesBillHeader.MakeVoucher: head from the bill; RefAccountId = the Bill To Party's GL ("Party GL Account not found.");
     * per detail row a debit line (row debit account, or the item's PurchaseGLAC) and a credit line to the party, both with the
     * auto remark; VoucherAmount = BillAmount = SUM(TotalAmount).
     */
    private LgsBVoucherPoster.Voucher makeVoucher(UserAccount u, LgsBServicesBillHeader o, List<LgsBServicesBillDetail> details,
                                                     String exportInvoiceNo, String exportInvoiceCustomer) {
        LocalDateTime now = LocalDateTime.now();
        LgsBVoucherHead vh = new LgsBVoucherHead();
        vh.DocumentTypeId = o.DocumentTypeId;
        vh.DocumentTypeSrNo = o.ServicesBillHeaderId;
        vh.RefDocNoId = o.ServicesBillHeaderId;
        vh.VoucherCode = o.DocumentNo;
        vh.VoucherDate = o.DocumentDate;
        vh.BranchId = o.BranchesId;
        vh.ProjectId = o.ProjectsId;
        vh.ManualBillNo = str(o.ReferenceNo);
        vh.DueDate = o.DocumentDate.plusDays(o.DueDays);
        vh.OrganizationId = o.OrganizationId;
        vh.CompanyId = o.CompanyId;
        vh.FinancialYearId = o.FinancialYearId;
        vh.EntryUser = o.EntryUserId;
        vh.ModifyUser = o.ModifyUserId;
        vh.EntryDate = now;
        vh.ModifyDate = now;
        vh.IncludeWHT = false;
        vh.Remarks = "";
        vh.RemarksOtherLingo = "";
        vh.ChequeDate = now.toLocalDate().atStartOfDay();
        Integer partyGl = null;
        for (Map<String, Object> r : repo.supplierGlAccounts(u)) {
            if (toInt(r.get("Id")) == o.BilltoPartyId) { partyGl = toInt(r.get("GlAccountId")); break; }
        }
        if (partyGl == null) throw invalid("Party GL Account not found.");
        vh.RefAccountId = partyGl;
        List<Map<String, Object>> items = repo.itemGlAccounts(u);
        if (items.isEmpty()) throw invalid("Item GL mapping not found.");
        String firstItemName = str(items.get(0).get("ItemName"));
        List<LgsBVoucherDetail> lines = new ArrayList<>();
        double total = 0;
        for (LgsBServicesBillDetail d : details) {
            /* Rows removed on the page (ActionTypeId 3) are not posted: the desktop posts them too, which leaves the
               removed amount in the ledger (fixed - see the report). */
            if (d.ActionTypeId == 3) continue;
            int debit = d.DebitAccountId > 0 ? d.DebitAccountId : purchaseGl(items, d.serviceItemId);
            List<String> parts = new ArrayList<>();
            if (!str(o.ReferenceNo).trim().isEmpty()) parts.add("Reference No: " + o.ReferenceNo);
            if (!firstItemName.trim().isEmpty()) parts.add("Service Item: " + firstItemName);
            if (d.ServiceRate != 0.0) parts.add("Service Rate: " + netDouble(d.ServiceRate));
            if (d.lcyAmount != 0.0) parts.add("Amount: " + netDouble(d.lcyAmount));
            if (!exportInvoiceNo.trim().isEmpty()) parts.add("ExportInvoiceNo: " + exportInvoiceNo);
            if (!exportInvoiceCustomer.trim().isEmpty()) parts.add("ExportInvoice_Customer: " + exportInvoiceCustomer);
            String auto = String.join(" ", parts);
            String text = str(d.Remarks).trim().isEmpty() ? auto : d.Remarks + " " + auto;
            lines.add(line(debit, vh.RefAccountId, d, true, text));
            lines.add(line(vh.RefAccountId, debit, d, false, text));
            total += d.TotalAmount;
        }
        vh.VoucherAmount = total;
        vh.BillAmount = total;
        return new LgsBVoucherPoster.Voucher(vh, lines);
    }

    private static int purchaseGl(List<Map<String, Object>> items, int itemId) {
        for (Map<String, Object> r : items) if (toInt(r.get("Id")) == itemId) return toInt(r.get("PurchaseGLAC"));
        throw invalid("Item GL Account not found.");
    }

    /** ServicesBillHeader.AddVoucherDetail. */
    private static LgsBVoucherDetail line(int account, int against, LgsBServicesBillDetail d, boolean debit, String auto) {
        LgsBVoucherDetail l = new LgsBVoucherDetail();
        l.AccountId = account;
        l.AgainstAccountId = against;
        l.Comments = auto == null || auto.trim().isEmpty() ? str(d.Remarks) : auto;
        l.DebitAmount = debit ? d.lcyAmount : 0.0;
        l.CreditAmount = debit ? 0.0 : d.lcyAmount;
        l.ItemId = d.serviceItemId;
        l.ItemRate = d.ServiceRate;
        l.ItemAmount = d.lcyAmount + d.TaxAmount * d.transactionExchangeRate;
        l.DMultiCurrencyId = d.transactionCurrencyId;
        l.DExchangeCurrencyRate = d.transactionExchangeRate;
        l.DCurrencyAmount = d.TotalAmount;
        l.ThirdCurrencyId = d.GlcyCurrencyId;
        l.ThirdCurrencyFcyExchangeRate = d.GlcyExchangeRate;
        l.ThirdCurrencyAmount = d.GlcyAmount;
        l.BaseFcyId = d.FcyCurrencyId;
        l.BaseFcyExchangeRate = d.FcyExchangeRate;
        l.BaseFcyAmount = d.fcyAmount;
        return l;
    }

    /** FillDetailListCommonForInsertAndDelete: Fcy / Glcy rate = the row's transaction rate when the currencies are the same. */
    private static LgsBServicesBillDetail detail(Map<String, Object> b, Map<String, Object> r) {
        LgsBServicesBillDetail vd = new LgsBServicesBillDetail();
        vd.ChargesTypeId = i(r, "ChargesTypeId");
        vd.AgreementHeaderId = i(r, "AgreementHeaderId");
        vd.AgreementDetailId = i(r, "AgreementDetailId");
        vd.PurchaseOrderHeaderId = i(r, "PurchaseOrderHeaderId");
        vd.PurchaseOrderDetailId = i(r, "PurchaseOrderDetailId");
        vd.partyRefDocNo = s(r, "PartyRefDocNo");
        LocalDateTime rd = toDate(r.get("PartyRefDocDate"));
        vd.partyRefDocDate = rd == null ? LocalDateTime.now() : rd;
        vd.serviceItemId = i(r, "ItemId");
        vd.transactionCurrencyId = i(r, "TransactionCurrencyId");
        vd.transactionExchangeRate = d(r, "TransactionExchangeRate");
        vd.ServiceRate = d(r, "ServiceRate");
        vd.RateBaseUomId = i(r, "RateBaseId");
        vd.Qty = d(r, "Qty");
        vd.NetWeight = d(r, "NetWeight");
        vd.tcyAmount = d(r, "TcyAmount");
        vd.lcyAmount = d(r, "LcyAmount");
        vd.FcyCurrencyId = i(b, "fcyCurrencyId");
        vd.FcyExchangeRate = vd.FcyCurrencyId == vd.transactionCurrencyId ? vd.transactionExchangeRate : d(b, "fcyExchangeRate");
        vd.fcyAmount = vd.FcyExchangeRate > 0.0 ? vd.lcyAmount / vd.FcyExchangeRate : 0.0;
        vd.GlcyCurrencyId = i(b, "glcyCurrencyId");
        vd.GlcyExchangeRate = vd.GlcyCurrencyId == vd.transactionCurrencyId ? vd.transactionExchangeRate : d(b, "glcyRate");
        vd.GlcyAmount = vd.GlcyExchangeRate > 0.0 ? vd.lcyAmount / vd.GlcyExchangeRate : 0.0;
        vd.TaxNameId = i(r, "TaxNameId");
        vd.TaxPercent = d(r, "Tax%");
        vd.TaxAmount = d(r, "TaxAmount");
        vd.TotalAmount = d(r, "TotalTcyAmount");
        vd.DebitAccountId = i(r, "DebitAccountId");
        vd.LocationTypeId = i(r, "LocationTypeId");
        if (vd.LocationTypeId == 16) {
            vd.LoadingPortId = i(r, "LocationFromId");
            vd.DestinationPortId = i(r, "LocationToId");
        } else {
            vd.FromCityId = i(r, "LocationFromId");
            vd.ToCityId = i(r, "LocationToId");
        }
        vd.Remarks = s(r, "Remarks");
        return vd;
    }

    // ================================================================== delete / history / prints

    public Map<String, Object> delete(int id) {
        UserAccount u = sup.user(SCREEN);
        sup.require(u, SCREEN, "Delete");
        if (id <= 0) throw invalid("No record found to Delete");
        ownHeader(u, id);
        repo.tx(() -> { repo.sbDelete(u.getId(), id); return 0; });
        return map("success", true, "message", "Delete Record Successfully", "docNo", repo.sbGenerateCode(u, sup.fy(), DOC_TYPE));
    }

    public List<Map<String, Object>> historyCombos() { return repo.sbDropDown(sup.user(SCREEN), String.valueOf(DOC_TYPE)); }

    /**
     * HistoryGridFill: ServicesBillHeader.FormHistory. The form puts the Service Provider filter in obj.ServiceProviderId, which
     * this BLL never sends (it sends BrokerOrServiceProviderId / BrokerAgentId, both left 0) - so that filter has no effect,
     * as on the desktop. Service Type goes to @ServiceTypeId.
     */
    public List<Map<String, Object>> history(Map<String, Object> f) {
        UserAccount u = sup.user(SCREEN);
        Map<String, Object> p = sup.historyParams(u, DOC_TYPE, sup.canViewAll(u, SCREEN_NAME), f);
        int st = i(f, "serviceTypeId");
        if (st != 0) p.put("ServiceTypeId", st);
        return repo.formHistory("[lgstcm].[USP_ServicesBillHeader_GetAllMethod]", p);
    }

    public List<Map<String, Object>> historyDetail(int id) {
        UserAccount u = sup.user(SCREEN);
        ownHeader(u, id);
        return detailRows(repo.sbDetail(id), true);
    }

    public Map<String, Object> printCheck(int id) {
        UserAccount u = sup.user(SCREEN);
        sup.require(u, SCREEN, "Print");
        ownHeader(u, id);
        return map("id", id);
    }

    /** btnPrintVoucher_Click / history Voucher: CommonServices.VoucherHeadIdGet(RecId, 1304) for VoucherReport_118. */
    public Map<String, Object> voucherId(int id) {
        UserAccount u = sup.user(SCREEN);
        sup.require(u, SCREEN, "Print");
        if (id <= 0) throw invalid("Record Not Found For Display");
        ownHeader(u, id);
        return map("voucherHeadId", repo.voucherHeadId(u, DOC_TYPE, id));
    }

    // ================================================================== Load PO (frmLoadPurchaseOrderForServicesBill)

    public List<Map<String, Object>> poCombos() { return repo.poDropDown(sup.user(SCREEN)); }

    /** PendingDataDbCall: PurchaseOrderHeader.LoadPendingSaleOrderOnInvoice (DocumentTypeId 1303). */
    public List<Map<String, Object>> pendingPo(Map<String, Object> f) {
        UserAccount u = sup.user(SCREEN);
        return repo.poPendingForServicesBill(u, sup.fy(), LgsBPurchaseOrderService.DOC_TYPE, toDate(f.get("fromDate")), toDate(f.get("toDate")),
                i(f, "fromDocNo"), i(f, "toDocNo"), i(f, "serviceTypeId"), i(f, "billToPartyId"), i(f, "serviceProviderId"), i(f, "itemId"));
    }

    // ================================================================== Load Freight Voucher (frmLoadPendingFreightVoucherExportForServicesBill)

    public List<Map<String, Object>> fvCombos() { return repo.fvDropDown(sup.user(SCREEN), null, null); }

    /** PendingDataDbCall: "Please select Invoice No", then FreightVoucherOutward_PendingForLogisticServicesBill (All 0 / Pending 1 / Complete 2). */
    public List<Map<String, Object>> pendingFreight(Map<String, Object> f) {
        UserAccount u = sup.user(SCREEN);
        if (i(f, "invoiceId") == 0) throw invalid("Please select Invoice No");
        return repo.fvPendingForServicesBill(u, sup.fy(), i(f, "invoiceId"), i(f, "customerId"), i(f, "transporterId"), i(f, "actionId"), null);
    }

    /** LoadInGridDetailFromVoucherByIds: the same procedure for the chosen invoice and ids ("No pending freight voucher data found."). */
    public List<Map<String, Object>> freightByIds(int invoiceId, String ids) {
        UserAccount u = sup.user(SCREEN);
        List<Map<String, Object>> r = repo.fvPendingForServicesBill(u, sup.fy(), invoiceId, 0, 0, 0, ids);
        if (r.isEmpty()) throw invalid("No pending freight voucher data found.");
        return r;
    }
}
