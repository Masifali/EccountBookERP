package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.saleinvoice.SaleInvoiceModels.ApprovalDetail;
import com.mst.models.saleinvoice.SaleInvoiceModels.Detail;
import com.mst.models.saleinvoice.SaleInvoiceModels.Head;
import com.mst.models.saleinvoice.SaleInvoiceModels.Journal;
import com.mst.models.saleinvoice.SaleInvoiceModels.StockDetail;
import com.mst.models.saleinvoice.SaleInvoiceModels.VoucherDetail;
import com.mst.models.saleinvoice.SaleInvoiceModels.VoucherHead;
import com.mst.repositories.ExportInvoiceAgainstForwardingRepository;
import com.mst.repositories.SaleInvoiceRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.ExportShipmentFormsSupport.*;

/**
 * Architecture.WinApp.Export.ExportInvoiceAgainstForwarding "Export Invoice" (ClientSize 1145 x 692) - an InvSaleInvoice with
 * DocumentTypeId 208 built from pending export forwardings (loader LoadExportForwarding, DocumentTypeId 206). No ScreenDefinition
 * row: rights by the form name "ExportInvoiceAgainstForwarding" (ExportShipmentFormsSupport) - Save / Update / Print (103-Voucher).
 *
 * Save: Insert() -> BLL 0580 InvSaleInvoice.Save (Id 0: ModifyUser = 0, "Record cannot be inserted because detailId greater than
 * zero"; else EntryUser = 0) -> SaleInvoiceFinancial.MakeVoucherForSaleInvoice (reused, SaleInvoiceFinancial) -> DAL 0433 SetData,
 * which for 208 runs (this class, persist208):
 *   header Sp_InvSaleInvoice_Insert | _Update; Sp_InvSaleInvoiceDetail_Insert per row (LineId 1..n, BillAmount = ItemAmount +
 *   ExpenseAmount - CommissionAmount - FreightAmount); Sp_InvSaleInvoiceJournal_Insert per party add/less row;
 *   ERP feature 5 (FIFO) ON  -> CommonServices.FIFOImplemention per row (USP_GetStockByFifoMethod, earlier rows reserved through
 *                               @FIFOXML), CGS voucher lines per FIFO layer (unless the job lot has an account or
 *                               InventoryFinancialsEffectsInActive), USP_InventoryQtyReverseAndDeleteByReferenceId on update,
 *                               USP_InventoryStockEvalautionDetail_Insert per layer (PrepareStockEvaluationDetail);
 *   ERP feature 5 OFF -> UpdateInventoryReference (Sp_InventoryStockEvalautionDetail_Update with the four reference fields);
 *   usp_StockInTransit_VoucherDelete_ByGdnId; USP_InventoryValidation per row; voucher head Insert|Update (GetVoucherHeadId),
 *   Sp_VoucherDetail_Insert (RefDocSubIdNo by LineId), USP_VoucherBalanceCheck, Sp_VoucherHead_H_Insert, Sp_VoucherDetail_H_Insert,
 *   [DAW].[USp_DocumentApprovalDetail_Insert]. One transaction.
 *
 * Desktop behaviour reproduced, not corrected:
 *  Q1  The header's BranchesId / ProjectsId / CurrencyId / ExchangeRate are never set by this form (0).
 *  Q2  The Expense grid (panel10) is hidden; Insert() only checks it ("Please Select an Item Against Expense First") and never
 *      adds InvSaleInvoiceExpenseList.
 *  Q3  UomScheduleIdRate = the item's UOM schedule row whose Equivalent = Conversion.ToInt(RateUom); with no schedule rows at all
 *      the row is still saved with UomScheduleIdRate 0 and RateUOM 0.
 *  Q4  cmbsuppliername_ValueChanged reads a field DataTable that SupplierNameFilll never fills (it binds a local), so the
 *      customer GL id stays empty and the party add/less "You Can No Select Customer Account Here...." test compares with 0.
 *  Q5  DueDate = Conversion.ToDateTime(txtDueDate.Text) - the picker's text, i.e. the date at midnight.
 *  Q6  "Load Farwarding" clears the grid first; closing the loader without loading leaves it empty. The loader's item filter
 *      and the checked rows only pick the forwarding ids - every pending line of those forwardings is loaded.
 *  Q7  ReadById_Update reads txtItemRateHeader / txtRateUomHeader from the HISTORY detail grid's current row; with no current
 *      row it stops there with the NullReference message (the rows are already loaded).
 * Not ported: attachments (DMS dialog / file store) - AttachmentsValues of an opened invoice are written back unchanged.
 */
@Service
public class ExportInvoiceAgainstForwardingService {

    public static final String SCREEN_NAME = "ExportInvoiceAgainstForwarding";
    public static final int DOCUMENT_TYPE_ID = 208;

    @Autowired private ExportInvoiceAgainstForwardingRepository repo;
    @Autowired private SaleInvoiceRepository si;
    @Autowired private CurrentUserContext ctx;

    // ================================================================= load

    /** InvfrmPurchaseInvoice_Load: DocumentNo, rights, party add/less accounts, other items, customers. */
    public Map<String, Object> setup() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights(si, ctx, u, SCREEN_NAME));
        put(out, "docNo", () -> si.nextDocNo(u, ctx.currentFinancialYearId(), DOCUMENT_TYPE_ID));
        put(out, "customers", () -> customers(u));
        put(out, "otherItems", () -> {
            List<Map<String, Object>> rows = new ArrayList<>();
            for (Map<String, Object> r : si.otherItems(u)) {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("ItemId", asInt(ci(r, "Id")));
                m.put("ItemName", raw(ci(r, "OtherItemName")));
                m.put("Remarks", "");
                m.put("Amount", 0d);
                rows.add(m);
            }
            return rows;
        });
        put(out, "partyAddLessAccounts", () -> {
            List<Map<String, Object>> rows = new ArrayList<>();
            for (Map<String, Object> r : si.accountTitlesByTypes(u, null, "2,11,12,15")) {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("Id", asInt(ci(r, "Id")));
                m.put("AccountTitle", raw(ci(r, "AccountTitle")));
                rows.add(m);
            }
            return rows;
        });
        return out;
    }

    /** btnFrmRefresh_Click: SupplierNameFilll. */
    public List<Map<String, Object>> customers() { return customers(ctx.requireAccountingUser()); }

    private List<Map<String, Object>> customers(UserAccount u) {
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : repo.customersForExport(u)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("CompanyName", raw(ci(r, "CompanyName")));
            rows.add(m);
        }
        return rows;
    }

    /** LoadExportForwarding Load: PartyNameFill (group "7"), ItemNameFill (GetItemsFromForwarding 206). */
    public Map<String, Object> loaderCombos() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        /* LoadInvoices_Load: FromDate = clsGlobalVariables.ActiveYr.Start_Period (sent with the search). */
        put(out, "financialYearStart", () -> {
            LocalDateTime start = si.financialYearStart(u, ctx.currentFinancialYearId());
            return start == null ? "" : start.toLocalDate().toString();
        });
        put(out, "parties", () -> {
            List<Map<String, Object>> rows = new ArrayList<>();
            for (Map<String, Object> r : repo.partiesByGroup(u, "7")) {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("Id", asInt(ci(r, "Id")));
                m.put("Name", raw(ci(r, "CompanyName")));
                rows.add(m);
            }
            return rows;
        });
        put(out, "items", () -> {
            List<Map<String, Object>> rows = new ArrayList<>();
            for (Map<String, Object> r : repo.itemsFromForwarding(u, 206)) {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("Id", asInt(ci(r, "ItemId")));
                m.put("Name", raw(ci(r, "ItemName")));
                rows.add(m);
            }
            return rows;
        });
        return out;
    }

    /** ExportInvoicesLoad: SupplierCustomerId, FromDate, ToDate (Convert.ToDateTime(picker.Value) - with the time of day). */
    public List<Map<String, Object>> loaderRows(int partyId, String from, String to) {
        UserAccount u = ctx.requireAccountingUser();
        return plain(repo.forwardingPending(u, ctx.currentFinancialYearId(), partyId, null,
                asDate(from) == null ? null : pickerDate(from), asDate(to) == null ? null : pickerDate(to)));
    }

    /** LoadDataDetailGridAgainstForwarding(",id,id"): Ids only (dates unset, party 0). */
    public List<Map<String, Object>> forwardingRows(String ids) {
        UserAccount u = ctx.requireAccountingUser();
        if (ids == null || ids.isEmpty()) return new ArrayList<>();
        return plain(repo.forwardingPending(u, ctx.currentFinancialYearId(), 0, ids, null, null));
    }

    // ================================================================= history / read

    /** GetAll(NoOfRecords): 50 when the History tab opens, 0 for "LoadAll". */
    public List<Map<String, Object>> history(int noOfRecords) {
        UserAccount u = ctx.requireAccountingUser();
        boolean viewAll = Boolean.TRUE.equals(rights(si, ctx, u, SCREEN_NAME).get("CanViewAllRecord"));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.history(u, ctx.currentFinancialYearId(), viewAll, noOfRecords)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("RecordNo", raw(ci(r, "RecordNo")));
            m.put("Id", asInt(ci(r, "Id")));
            m.put("VoucherHeadId", asInt(ci(r, "VoucherHeadId")));
            m.put("DocNo", raw(ci(r, "DocNo")));
            m.put("DocDate", iso(ci(r, "DocDate")));
            m.put("CustomerName", raw(ci(r, "CustomerName")));
            m.put("DueDate", iso(ci(r, "DueDate")));
            m.put("ManualBillNo", raw(ci(r, "ManualBillNo")));
            m.put("SupplierCustomerId", asInt(ci(r, "SupplierCustomerId")));
            m.put("RemarksHeader", raw(ci(r, "RemarksHeader")));
            m.put("BillAmount", asDouble(ci(r, "BillAmount")));
            m.put("EntryDate", iso(ci(r, "EntryDate")));
            m.put("EntryUser", raw(ci(r, "UserName")));
            m.put("NoOfAttachments", raw(ci(r, "NoOfAttachments")));
            out.add(m);
        }
        return out;
    }

    /** InvSaleInvoice.GetByID: header + SaleDetailReadByInvSaleInvoiceId + journals (ReadById_Update / GetDetailGrdByHeadId). */
    public Map<String, Object> readById(int id) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> h = si.readById(id);
        if (h == null || asInt(ci(h, "CompanyId")) != u.getCompanyId() || asInt(ci(h, "DocumentTypeId")) != DOCUMENT_TYPE_ID)
            throw new IllegalArgumentException("Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", asInt(ci(h, "Id")));
        out.put("DocNo", asInt(ci(h, "DocNo")));
        out.put("DocDate", iso(ci(h, "DocDate")));
        out.put("SupplierCustomerId", asInt(ci(h, "SupplierCustomerId")));
        out.put("StockPartyId", asInt(ci(h, "StockPartyId")));
        out.put("ManualBillNo", raw(ci(h, "ManualBillNo")));
        out.put("RemarksHeader", raw(ci(h, "RemarksHeader")));
        out.put("BillAmount", asDouble(ci(h, "BillAmount")));
        out.put("DueDate", iso(ci(h, "DueDate")));
        out.put("DueDays", asInt(ci(h, "DueDays")));
        out.put("IsApproved", asBool(ci(h, "IsApproved")));
        out.put("VoucherHeadId", si.voucherHeadId(u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, id));
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : list(h.get("details"))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(d, "Id")));
            m.put("InvforwardingId", asInt(ci(d, "InvForwardingId")));
            m.put("ForwardingDocNo", raw(ci(d, "ForwardingDocNo")));
            m.put("ReferenceNo", raw(ci(d, "ReferenceNo")));
            m.put("ItemId", asInt(ci(d, "ItemId")));
            m.put("ItemName", raw(ci(d, "ItemName")));
            m.put("PackingTypeId", asInt(ci(d, "PackingTypeId")));
            m.put("PackingType", raw(ci(d, "PackTypeDesc")));
            m.put("PackUomId", asInt(ci(d, "ItemUOMId")));
            m.put("PackUom", raw(ci(d, "UOMCodeItem")));
            m.put("ItemQty", asDouble(ci(d, "ItemQty")));
            m.put("NetBillWeight", asDouble(ci(d, "NetBillWeight")));
            m.put("AddLessWeight", asDouble(ci(d, "AdLsWeight")));
            m.put("GrossWeight", asDouble(ci(d, "GrossWeight")));
            m.put("ItemRate", asDouble(ci(d, "ItemRate")));
            m.put("RateUomId", asInt(ci(d, "UomScheduleIdRate")));
            m.put("RateUom", asDouble(ci(d, "RateUOM")));
            m.put("ItemAmount", asDouble(ci(d, "ItemAmount")));
            m.put("GpNo", raw(ci(d, "GpNo")));
            m.put("VehicleNo", raw(ci(d, "VehicleNo")));
            m.put("BiltyNo", 0);
            m.put("CropYear", raw(ci(d, "CropYear")));
            m.put("WarehouseId", asInt(ci(d, "WarehouseId")));
            m.put("WarehouseName", raw(ci(d, "WareHouseName")));
            m.put("JobLotId", asInt(ci(d, "JobLotId")));
            m.put("JobLot", raw(ci(d, "JobLotDescription")));
            m.put("ItemDescription", raw(ci(d, "RemarksDetail")));
            rows.add(m);
        }
        out.put("rows", rows);
        List<Map<String, Object>> jv = new ArrayList<>();
        for (Map<String, Object> j : list(h.get("journals"))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("AccountId", asInt(ci(j, "ChartofAccountId")));
            m.put("Percentage", asDouble(ci(j, "JvPrcnt")));
            m.put("Qty", asDouble(ci(j, "JvQty")));
            m.put("Rate", asDouble(ci(j, "JvRate")));
            m.put("Debit", asDouble(ci(j, "JvDebit")));
            m.put("Credit", asDouble(ci(j, "JvCredit")));
            m.put("Remarks", raw(ci(j, "JvRemarks")));
            jv.add(m);
        }
        out.put("journals", jv);
        return out;
    }

    // ================================================================= save

    /**
     * Insert(): FormValidation ("Party Name is Required", "Doc No is Required"), the expense check, the detail rows with their
     * own MessageBox stops ("Item Name Feild Required In Detail Grid", "Item Rate Feild Required In Detail Grid",
     * "Rate Uom Feild Required In Detail Grid", "this RateUom not define please check"), "Grid Record Not Found",
     * then InvSaleInvoice.Save.
     */
    @Transactional(rollbackFor = Exception.class)
    public Map<String, Object> save(Map<String, Object> body) {
        UserAccount u = ctx.requireAccountingUser();
        int id = asInt(body.get("id"));
        Map<String, Boolean> r = rights(si, ctx, u, SCREEN_NAME);
        if (id > 0) require(r, "Update", "You do not have the Update right for this screen.");
        else require(r, "Save", "You do not have the Save right for this screen.");

        Map<String, Object> existing = null;
        if (id > 0) {
            existing = si.readById(id);
            if (existing == null || asInt(ci(existing, "CompanyId")) != u.getCompanyId() || asInt(ci(existing, "DocumentTypeId")) != DOCUMENT_TYPE_ID)
                throw new IllegalArgumentException("Record Not Update because Record Id not found");
            if (asBool(ci(existing, "IsApproved"))) throw new IllegalArgumentException("Record Not Update because Record has approved");
        }
        int customerId = asInt(body.get("supplierCustomerId"));
        if (customerId == 0) throw new IllegalArgumentException("Party Name is Required");
        String docNoText = text(body.get("docNo"));
        if (docNoText.isEmpty() || "0".equals(docNoText)) throw new IllegalArgumentException("Doc No is Required");
        for (Map<String, Object> e : list(body.get("expenses"))) {
            if (asDouble(e.get("Amount")) > 0 && asInt(e.get("ItemId")) == 0) throw new IllegalArgumentException("Please Select an Item Against Expense First");
        }

        LocalDateTime nowDt = LocalDateTime.now().withNano(0);
        Head h = new Head();
        h.Id = id;
        h.DocDate = docDate(text(body.get("docDate")), existing);
        h.DocNo = asInt(docNoText);
        h.DocumentTypeId = DOCUMENT_TYPE_ID;
        h.ManualBillNo = text(body.get("manualBillNo"));
        h.RemarksHeader = text(body.get("remarks"));
        h.DueDays = asInt(body.get("dueDays"));
        LocalDate due = asDate(body.get("dueDate"));
        h.DueDate = (due == null ? LocalDate.now() : due).atStartOfDay();                       // Q5
        h.SupplierCustomerId = customerId;
        h.BillAmount = asDouble(body.get("billAmountText"));
        h.EntryDate = nowDt;
        h.ModifyDate = nowDt;
        h.SupplierInvoiceDate = nowDt;
        h.EntryUser = u.getId();
        h.ModifyUser = u.getId();
        h.OrganizationId = u.getOrganizationId();
        h.CompanyId = u.getCompanyId();
        h.FinancialYearId = ctx.currentFinancialYearId();
        h.AttachmentsValues = existing == null ? "" : raw(ci(existing, "AttachmentsValues"));
        h.CustomAttachmentsValues = existing == null ? "" : raw(ci(existing, "CustomAttachmentsValues"));

        List<Map<String, Object>> grid = list(body.get("rows"));
        if (grid.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");
        SaleInvoiceFinancial.Invoice inv = new SaleInvoiceFinancial.Invoice();
        inv.h = h;
        int lineId = 1;
        for (Map<String, Object> g : grid) {
            Detail d = new Detail();
            d.LineId = lineId;
            d.InvForwardingDetailId = asInt(g.get("Id"));
            d.InvForwardingId = asInt(g.get("InvforwardingId"));
            d.ReferenceNo = raw(g.get("ReferenceNo"));
            d.VehicleNo = raw(g.get("VehicleNo"));
            d.GpNo = asInt(g.get("GpNo"));
            if (asInt(g.get("ItemId")) == 0) throw new IllegalArgumentException("Item Name Feild Required In Detail Grid");
            d.ItemId = asInt(g.get("ItemId"));
            d.RemarksDetail = raw(g.get("ItemDescription"));
            d.WarehouseId = asInt(g.get("WarehouseId"));
            d.JobLotId = asInt(g.get("JobLotId"));
            d.CropYear = raw(g.get("CropYear"));
            d.PackingTypeId = asInt(g.get("PackingTypeId"));
            d.ItemUOMId = asInt(g.get("PackUomId"));
            d.ItemQty = asDouble(g.get("ItemQty"));
            d.NetBillWeight = asDouble(g.get("NetBillWeight"));
            d.AdLsWeight = asDouble(g.get("AddLessWeight"));
            d.GrossWeight = asDouble(g.get("GrossWeight"));
            d.NetStockWeight = d.NetBillWeight;
            d.GpDate = nowDt;
            if (asDouble(g.get("ItemRate")) == 0) throw new IllegalArgumentException("Item Rate Feild Required In Detail Grid");
            d.ItemRate = asDouble(g.get("ItemRate"));
            String rateUomText = text(g.get("RateUom"));
            if (asDouble(rateUomText) == 0 || rateUomText.isEmpty()) throw new IllegalArgumentException("Rate Uom Feild Required In Detail Grid");
            List<Map<String, Object>> sched = si.uomScheduleByItem(u.getOrganizationId(), u.getCompanyId(), d.ItemId);
            if (!sched.isEmpty()) {                                                               // Q3
                int eq = asInt(rateUomText);
                Map<String, Object> match = null;
                for (Map<String, Object> s : sched) if (asDouble(ci(s, "Equivalent")) == eq) { match = s; break; }
                if (match == null) throw new IllegalArgumentException("this RateUom not define please check");
                d.UomScheduleIdRate = asInt(ci(match, "Id"));
                d.RateUOM = asDouble(rateUomText);
            }
            d.ItemAmount = asDouble(g.get("ItemAmount"));
            d.BillAmount = d.ItemAmount;
            inv.details.add(d);
            lineId++;
        }
        for (Map<String, Object> j : list(body.get("journals"))) {
            if (asInt(j.get("AccountId")) == 0) continue;
            Journal pj = new Journal();
            pj.ChartofAccountId = asInt(j.get("AccountId"));
            pj.JvRemarks = raw(j.get("Remarks"));
            pj.JvPrcnt = asDouble(j.get("Percentage"));
            pj.JvQty = asDouble(j.get("Qty"));
            pj.JvRate = asDouble(j.get("Rate"));
            pj.JvDebit = asDouble(j.get("Debit"));
            pj.JvCredit = asDouble(j.get("Credit"));
            inv.journals.add(pj);
        }

        /* BLL 0580 Save */
        if (h.Id == 0) {
            for (Detail d : inv.details) if (d.Id > 0) throw new IllegalStateException("Record cannot be inserted because detailId greater than zero");
            h.ModifyUser = 0;
        } else {
            h.EntryUser = 0;
        }
        SaleInvoiceFinancial.Voucher voucher = new SaleInvoiceFinancial(si).makeVoucherForSaleInvoice(inv);
        int saved = persist208(u, inv, voucher, h.Id == 0 ? "Sp_InvSaleInvoice_Insert" : "Sp_InvSaleInvoice_Update");
        Map<String, Object> out = ok(id > 0 ? "Record Update Successfully [" + h.DocNo + "] " : "Record Saved Successfully [" + h.DocNo + "] ", saved);
        out.put("voucherHeadId", si.voucherHeadId(u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, saved));
        return out;
    }

    /** txtDocDate.Value: a new invoice carries the time of day; an opened one keeps the stored DocDate's time. */
    private static LocalDateTime docDate(String iso, Map<String, Object> existing) {
        LocalDate d = asDate(iso);
        if (d == null) d = LocalDate.now();
        if (existing != null) {
            Object v = ci(existing, "DocDate");
            LocalTime t = LocalTime.MIDNIGHT;
            if (v instanceof Timestamp) t = ((Timestamp) v).toLocalDateTime().toLocalTime();
            else if (v instanceof LocalDateTime) t = ((LocalDateTime) v).toLocalTime();
            return LocalDateTime.of(d, t);
        }
        return LocalDateTime.of(d, LocalTime.now().withNano(0));
    }

    // ================================================================= DAL 0433 SetData for DocumentTypeId 208

    private int persist208(UserAccount u, SaleInvoiceFinancial.Invoice obj, SaleInvoiceFinancial.Voucher voucher, String procName) {
        Head h = obj.h;
        if (obj.details.isEmpty()) throw new IllegalStateException("Detail List not found");
        int num3 = si.setProc(procName, h);
        if (num3 > 0) h.Id = num3; else num3 = h.Id;
        int num4 = 1;
        for (Detail d : obj.details) {
            d.LineId = num4;
            d.InvSaleInvoiceId = h.Id;
            d.BillAmount = d.ItemAmount + d.ExpenseAmount - d.CommissionAmount - d.FreightAmount;
            d.Id = si.setProc("Sp_InvSaleInvoiceDetail_Insert", d);
            num4++;
        }
        for (Journal j : obj.journals) { j.InvSaleInvoiceId = h.Id; si.setProc("Sp_InvSaleInvoiceJournal_Insert", j); }

        int org = h.OrganizationId, company = h.CompanyId;
        Map<Integer, Map<String, Object>> itemGl = si.itemGl(org, company);
        Map<Integer, Integer> jobLots = si.jobLotAccounts(org, company);
        boolean num9 = si.feature(u, 5);                     // 208 is not in {103,126,133,145}; no RefRef ids on these rows
        boolean flag3 = SaleInvoiceRepository.truthy(si.config(org, company, "InventoryFinancialsEffectsInActive"));
        if (num9) {
            List<StockDetail> layers = new ArrayList<>();
            for (Detail item : obj.details) {
                Map<String, Object> ig = itemGl.get(item.ItemId);
                if (ig == null) continue;
                String itemName = raw(ci(ig, "ItemName"));
                boolean hasJobLotAccount = jobLots.getOrDefault(item.JobLotId, 0) > 0;
                for (StockDetail sd : fifo(u, h, item, itemName, layers)) {
                    if (!hasJobLotAccount && !flag3) {
                        String remarks = "ItemQty: " + SaleInvoiceFinancial.g(sd.QtyOut) + " " + itemName + " Net Weight: " + SaleInvoiceFinancial.g(sd.StockWeightOut)
                                + " CGS Rate: " + SaleInvoiceFinancial.g(sd.CgsRate) + " " + obj.ids.CompanyName;
                        voucher.details.add(cgsDetail(item, sd, remarks, asInt(ci(ig, "COGSGLAC")), asInt(ci(ig, "PurchaseGLAC")), sd.CgsAmount, 0d, h.SupplierCustomerId));
                        voucher.details.add(cgsDetail(item, sd, remarks, asInt(ci(ig, "PurchaseGLAC")), asInt(ci(ig, "COGSGLAC")), 0d, sd.CgsAmount, h.SupplierCustomerId));
                    }
                    layers.add(sd);
                }
            }
            if (!layers.isEmpty()) {
                if (h.ModifyUser > 0) {
                    si.run("EXEC [dbo].[USP_InventoryQtyReverseAndDeleteByReferenceId] @OrganizationId=?, @CompanyId=?, @RefDocumentTypeId=?, @RefDocIdNo=?",
                            org, company, h.DocumentTypeId, h.Id);
                }
                for (StockDetail sd : layers) {
                    prepareStockEvaluationDetail(sd, obj);
                    si.setProc("USP_InventoryStockEvalautionDetail_Insert", sd);
                }
            }
        } else {
            StockDetail sd = new StockDetail();                                  // UpdateInventoryReference
            sd.OrganizationId = org; sd.CompanyId = company; sd.RefDocumentTypeId = h.DocumentTypeId; sd.RefDocIdNo = h.Id;
            si.setProc("Sp_InventoryStockEvalautionDetail_Update", sd);
        }
        si.run("EXEC dbo.usp_StockInTransit_VoucherDelete_ByGdnId @Id=?", h.Id);
        for (Detail d : obj.details) {
            si.inventoryValidation(org, company, h.DocumentTypeId, h.DocDate, d.ItemId, d.WarehouseId, d.JobLotId, d.CropYear,
                    d.PackingTypeId, d.ItemUOMId, d.NetStockWeight, d.RefRefDocumentTypeId, d.RefRefDocIdNo, d.RefDocSubId, d.ItemConditionId);
        }
        VoucherHead vh = voucher.head;
        int existing = si.voucherHeadId(org, company, h.DocumentTypeId, h.Id);
        if (existing > 0) vh.Id = existing;
        vh.DocumentTypeSrNo = h.Id;
        vh.RefDocNoId = h.Id;
        int num = si.setProc(existing == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", vh);
        if (num > 0) vh.Id = num;
        for (VoucherDetail vd : voucher.details) {
            vd.VoucherHeadId = vh.Id;
            vd.BranchesId = h.BranchesId;
            for (Detail d : obj.details) if (d.LineId == vd.LineId && d.LineId > 0 && vd.LineId > 0) { vd.RefDocSubIdNo = d.Id; break; }
            si.setProc("Sp_VoucherDetail_Insert", vd);
        }
        if (voucher.details.isEmpty()) throw new IllegalStateException("Voucher Detail list Not Found");
        si.run("EXEC dbo.USP_VoucherBalanceCheck @OrganizationId=?, @CompanyId=?, @Id=?", org, company, vh.Id);
        int docTypeRef = si.setProc("Sp_VoucherHead_H_Insert", vh);
        for (VoucherDetail vd : voucher.details) {
            vd.VoucherHeadId = vh.Id;
            vd.DocumentTypeIdRef = docTypeRef;
            si.setProc("Sp_VoucherDetail_H_Insert", vd);
        }
        ApprovalDetail a = new ApprovalDetail();
        a.OrganizationId = org; a.CompanyId = company; a.DocumentTypeId = h.DocumentTypeId; a.Id = h.Id;
        a.LimitAmount = BigDecimal.valueOf(h.BillAmount);
        si.setProc("[DAW].[USp_DocumentApprovalDetail_Insert]", a);
        return num3;
    }

    /**
     * CommonServices.FIFOImplemention(CreateStockReportParameter(obj, item, name), reserved): the stock layers for one row,
     * earlier rows' layers reserved through @FIFOXML; refusals with the desktop's texts.
     */
    private List<StockDetail> fifo(UserAccount u, Head h, Detail item, String itemName, List<StockDetail> reserved) {
        boolean update = h.ModifyUser > 0;
        String xml = null;
        if (!reserved.isEmpty()) {
            StringBuilder sb = new StringBuilder("<ArrayOfFIFOStockEvaluation>");
            for (StockDetail r : reserved) {
                sb.append("<FIFOStockEvaluation>")
                        .append("<RefDocumentTypeId>").append(r.RefRefDocumentTypeId).append("</RefDocumentTypeId>")
                        .append("<RefDocIdNo>").append(r.RefRefDocIdNo).append("</RefDocIdNo>")
                        .append("<RefDocSubIdNo>").append(r.RefRefDocSubIdNo).append("</RefDocSubIdNo>")
                        .append("<ReserveQty>").append(r.QtyOut).append("</ReserveQty>")
                        .append("<ReserveWeight>").append(r.StockWeightOut).append("</ReserveWeight>")
                        .append("</FIFOStockEvaluation>");
            }
            xml = sb.append("</ArrayOfFIFOStockEvaluation>").toString();
        }
        List<Map<String, Object>> stocks = repo.stockByFifo(u, item.ItemId, Timestamp.valueOf(h.DocDate), item.ItemUOMId, item.WarehouseId,
                item.JobLotId, item.PackingTypeId, item.CropYear, update ? h.DocumentTypeId : 0, update ? h.Id : 0, xml);
        if (stocks.isEmpty()) throw new IllegalStateException("Stock Not Found this Item " + itemName + " against FIFO Method .....");
        double available = 0;
        for (Map<String, Object> s : stocks) available += asDouble(ci(s, "NetBalWeight"));
        double itemQty = item.ItemQty, netWeight = item.NetStockWeight;
        if (!(netWeight <= round(available, 2)))
            throw new IllegalStateException("Weight available is " + SaleInvoiceFinancial.g(available) + " and row Weight is " + SaleInvoiceFinancial.g(netWeight)
                    + " this item " + itemName + " against FIFO....");
        List<StockDetail> out = new ArrayList<>();
        double num5 = 0, num7 = 0;
        for (Map<String, Object> s : stocks) {
            double avgRate = asDouble(ci(s, "AvgRate"));
            int rateUomId = asInt(ci(s, "RateUomId"));
            if (avgRate <= 0) throw new IllegalStateException("Rate Not Found this Item " + itemName + " against FIFO Method");
            if (rateUomId == 0) throw new IllegalStateException("RateUomId not found  this " + itemName + " against FIFO Method");
            double num6 = asDouble(ci(s, "NetBalWeight")), num4 = asDouble(ci(s, "NetBalQty"));
            double num = si.equivalent(h.OrganizationId, h.CompanyId, item.ItemId, rateUomId);
            if (num == 0) throw new IllegalStateException("RateUom Not Found");
            StockDetail sd = new StockDetail();
            sd.Id = asInt(ci(s, "Id"));
            sd.LineId = item.LineId;
            sd.ItemId = item.ItemId;
            sd.WarehouseId = item.WarehouseId;
            sd.RateUom = rateUomId;
            sd.JobLotId = item.JobLotId;
            sd.InvPackingTypeId = item.PackingTypeId;
            sd.ItemUom = item.ItemUOMId;
            sd.CropBatch = item.CropYear == null ? "" : item.CropYear;
            sd.RefRefDocumentTypeId = asInt(ci(s, "RefDocumentTypeId"));
            sd.RefRefDocIdNo = asInt(ci(s, "RefDocIdNo"));
            sd.RefRefDocSubIdNo = asInt(ci(s, "RefDocSubIdNo"));
            sd.CgsRate = avgRate * num;
            sd.DocDate = h.DocDate;
            if (num6 <= netWeight - num7) {
                num7 += num6;
                num5 += num4;
                sd.QtyOut = num4;
                sd.BillWeightOut = num6;
                sd.StockWeightOut = num6;
            } else {
                sd.QtyOut = itemQty - num5;
                sd.BillWeightOut = netWeight - num7;
                sd.StockWeightOut = netWeight - num7;
                num5 += sd.QtyOut;
                num7 += sd.BillWeightOut;
            }
            sd.CgsAmount = sd.BillWeightOut / num * sd.CgsRate;
            out.add(sd);
            if (netWeight == num7) break;
        }
        return out;
    }

    /** DAL 0433 PrepareStockEvaluationDetail. */
    private void prepareStockEvaluationDetail(StockDetail detail, SaleInvoiceFinancial.Invoice obj) {
        Head h = obj.h;
        detail.OrganizationId = h.OrganizationId;
        detail.CompanyId = h.CompanyId;
        detail.DocDate = h.DocDate;
        detail.DocCodeNo = h.DocNo;
        detail.SupplierCustomerId = h.SupplierCustomerId;
        detail.BranchesId = h.BranchesId;
        detail.RefDocumentTypeId = h.DocumentTypeId;
        detail.EntryUser = h.EntryUser;
        detail.ModifyUser = h.ModifyUser;
        detail.CalcType = "Weight";
        Detail line = null;
        for (Detail d : obj.details) if (d.LineId == detail.LineId && detail.LineId > 0) { line = d; break; }
        if (line == null) return;
        double eq = si.equivalent(h.OrganizationId, h.CompanyId, detail.ItemId, line.UomScheduleIdRate);
        if (detail.BillWeightOut > 0 && line.ItemRate > 0 && eq > 0) {
            detail.AmountOut = detail.BillWeightOut / eq * line.ItemRate;
            double num = line.ExpenseAmount > 0 && line.ItemQty > 0 && detail.QtyOut > 0 ? line.ExpenseAmount / line.ItemQty * detail.QtyOut : 0;
            double num2 = line.FreightAmount > 0 && line.NetBillWeight > 0 && detail.BillWeightOut > 0 ? line.FreightAmount / line.NetBillWeight * detail.BillWeightOut : 0;
            double num3 = line.CommissionAmount > 0 && line.ItemAmount > 0 && detail.AmountOut > 0 ? line.CommissionAmount / line.ItemAmount * detail.AmountOut : 0;
            detail.AmountOut += num - num2 - num3;
        }
        detail.ItemRate = line.ItemRate;
        detail.RefDocIdNo = line.InvSaleInvoiceId;
        detail.RefDocSubIdNo = line.Id;
    }

    /** DAL 0433 CreateCgsVoucherDetail. */
    private static VoucherDetail cgsDetail(Detail item, StockDetail sd, String remarks, int accountId, int againstAccountId,
                                           double debit, double credit, int party) {
        VoucherDetail x = new VoucherDetail();
        x.LineId = item.LineId; x.IsCGS = 1; x.AccountId = accountId; x.AgainstAccountId = againstAccountId; x.Comments = remarks;
        x.DebitAmount = debit; x.CreditAmount = credit;
        x.RefDocumentTypeId = sd.RefRefDocumentTypeId; x.RefDocNoId = sd.RefRefDocIdNo; x.RefDocNoDetailId = sd.RefRefDocSubIdNo;
        x.ItemId = item.ItemId; x.QtyOut = sd.QtyOut; x.WeightOut = sd.StockWeightOut; x.ItemCgsRate = sd.CgsRate;
        x.RateCut = item.RateCut; x.RateCutAmount = item.RateCutAmount; x.ItemAmount = sd.AmountOut; x.Expenses = item.ExpenseAmount;
        x.Commission = item.CommissionAmount; x.Freight = item.FreightAmount; x.OrderNo = item.SaleOrder; x.GpNo = item.GpNo;
        x.VehicleNo = item.VehicleNo; x.JobLotId = item.JobLotId; x.SupplierCustomerId = party; x.BranchesId = item.BranchId;
        x.CostCenterId = item.CostCenterId;
        return x;
    }

    /** Header 103-Voucher / history Voucher: Print right (btnPrint.Enabled); VoucherHeadId comes from the page. */
    public Map<String, Object> printCheck() {
        UserAccount u = ctx.requireAccountingUser();
        require(rights(si, ctx, u, SCREEN_NAME), "Print", "You do not have the Print right for this screen.");
        return ok("", 0);
    }
}
