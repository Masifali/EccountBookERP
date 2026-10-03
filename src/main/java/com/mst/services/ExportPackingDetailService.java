package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.ContraVoucherDto;
import com.mst.repositories.ExportPackingDetailRepository;
import com.mst.repositories.ExportPreInvoiceRepository;
import com.mst.repositories.StockAdjustmentRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.lang.reflect.Field;
import java.math.BigDecimal;
import java.math.RoundingMode;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.ExportPreInvoiceService.asDouble;
import static com.mst.services.ExportPreInvoiceService.asInt;
import static com.mst.services.ExportPreInvoiceService.ci;
import static com.mst.services.ExportPreInvoiceService.clr;
import static com.mst.services.ExportPreInvoiceService.iso;
import static com.mst.services.ExportPreInvoiceService.list;
import static com.mst.services.ExportPreInvoiceService.pick;
import static com.mst.services.ExportPreInvoiceService.row;
import static com.mst.services.ExportPreInvoiceService.text;
import static com.mst.services.ExportPreInvoiceService.ts;

/**
 * BLL side of 193 "Packing Detail" - Architecture.WinApp.Export.PackingDetailForCommercialInvoice
 * ("Packing List for Commercial Invoice", DocumentTypeId 211 = the Commercial Invoice it belongs to).
 *
 * The form: pick a Commercial Invoice without a packing list (UPS_GetCommercialInvoicenumberForPackingDetail);
 * "Invoice Information" (every box disabled) fills from ExImInvoice.GetByID and the grid seeds one row per
 * invoice detail row (WarehouseId 0, Id 0); the operator sets Warehouse / JobLot, splits rows with the "Add"
 * column (balance quantity / weights of the invoice), edits Qty, Packing Wt, Packing Total Wt, Container No and
 * Seal No (Janus EditType 1 / 4; Net and Gross Weight are EditType 0 = not editable) and saves through
 * ExImInvoice.SetDataForCommercialInvoicePackingList, which also posts stock (FIFO costing when ERP feature 5 is
 * on, with the CGS lines added to the invoice's voucher). History (Edit / Slip, double-click) re-opens a saved
 * list for Update; Delete (only after Edit, when the user has the Delete right) runs Sp_InvoicesVouchersandStocksDelete
 * for DocumentTypeId 211 and the invoice id - the desktop's own call (InvPurchaseInvoice.RemoveByID).
 */
@Service
public class ExportPackingDetailService {

    public static final int SCREEN_ID = 193;
    public static final int DOCUMENT_TYPE_ID = 211;

    @Autowired private ExportPackingDetailRepository repo;
    @Autowired private ExportPreInvoiceRepository shared;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private DesktopReportRights rights;

    private UserAccount user(String action) {
        UserAccount u = currentUserContext.requireAccountingUser();
        rights.require(u, SCREEN_ID, action);
        return u;
    }

    private boolean allowed(UserAccount u, String action) {
        try { rights.require(u, SCREEN_ID, action); return true; } catch (AccessDeniedException e) { return false; }
    }

    private int fy() { return currentUserContext.currentFinancialYearId(); }

    // ================================================================= load

    /** PackingDetailForCommercialInvoice_Load: rights, CustomerGetAll, DeliveryTermFill, LoadingPortFill, MultiCurrencyfill, BindInvoices, CarierType, grid value lists. */
    public Map<String, Object> setup() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, "Save"));
        perm.put("Update", allowed(u, "Update"));
        perm.put("Delete", allowed(u, "Delete"));
        perm.put("Print", allowed(u, "Print"));
        out.put("permissions", perm);
        out.put("customers", pick(shared.exportCustomers(u), "Id", "CompanyName"));
        out.put("deliveryTerms", pick(shared.deliveryTerms(), "Id", "Code"));
        out.put("ports", pick(shared.seaPorts(u), "Id", "PortName"));
        out.put("currencies", pick(shared.currencies(u), "Id", "CurrencyName"));
        out.put("warehouses", pick(shared.warehouses(u), "Id", "WareHouseName"));
        out.put("jobLots", pick(shared.jobLots(u), "Id", "JobLotDescription"));
        out.put("invoices", invoices(u));
        return out;
    }

    private List<Map<String, Object>> invoices(UserAccount u) { return pick(repo.invoices(u), "Id", "InvoiceNo"); }

    /** btnrefersh_Click / FormReset: BindInvoices. */
    public List<Map<String, Object>> refresh() { return invoices(user("View")); }

    /** The header boxes ExImInvoice.GetByID fills (CommercialInvoiceDataByInvoiceId / ReadById). */
    private Map<String, Object> header(UserAccount u, int invoiceId) {
        Map<String, Object> h = shared.invoiceById(invoiceId);
        if (h == null) throw new IllegalArgumentException("Index was outside the bounds of the array.");
        if (asInt(ci(h, "CompanyId")) != u.getCompanyId() || asInt(ci(h, "OrganizationId")) != u.getOrganizationId())
            throw new AccessDeniedException("Record belongs to another company");
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", asInt(ci(h, "Id")));
        m.put("InvoiceNo", text(ci(h, "InvoiceNo")));
        m.put("DocCode", asInt(ci(h, "DocCode")));
        m.put("DocDate", iso(ci(h, "DocDate")));
        m.put("SupplierCustomerId", asInt(ci(h, "SupplierCustomerId")));
        m.put("DestinationPortId", asInt(ci(h, "DestinationPortId")));
        m.put("OtherCustomerId", asInt(ci(h, "OtherCustomerId")));
        m.put("OtherDestinationPortId", asInt(ci(h, "OtherDestinationPortId")));
        m.put("LotNoRef", text(ci(h, "LotNoRef")));
        m.put("DeliveryTermId", asInt(ci(h, "DeliveryTermId")));
        m.put("LoadingPortId", asInt(ci(h, "LoadingPortId")));
        m.put("ConversionRate", asDouble(ci(h, "ConversionRate")));
        m.put("CarierType", text(ci(h, "CarierType")));
        m.put("FcurrencyId", asInt(ci(h, "FcurrencyId")));
        m.put("GrossWeight", clr(asDouble(ci(h, "GrossWeight"))));        // PIH.GrossWeight.ToString()
        m.put("NetWeight", clr(asDouble(ci(h, "NetWeight"))));
        m.put("NoOfContainers", clr(asDouble(ci(h, "NoOfContainers"))));
        m.put("EFormNo", text(ci(h, "EFormNo")));
        m.put("EFormDate", iso(ci(h, "EFormDate")));
        return m;
    }

    /** Cmbinvoiceno_TextChanged -> CommercialInvoiceDataByInvoiceId: the header and one grid row per invoice detail row. */
    public Map<String, Object> invoiceData(int invoiceId) {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", header(u, invoiceId));
        List<Map<String, Object>> rows = new ArrayList<>();
        double totalQty = 0;
        for (Map<String, Object> d : shared.invoiceDetail(invoiceId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", 0);
            m.put("CommercialInvoiceId", asInt(ci(d, "ExImInvoiceId")));
            m.put("CommercialInvoiceDetailId", asInt(ci(d, "Id")));
            m.put("RefDocumentTypeId", DOCUMENT_TYPE_ID);
            m.put("ItemId", asInt(ci(d, "ItemId")));
            m.put("ItemName", text(ci(d, "ItemName")));
            m.put("PackTypeId", asInt(ci(d, "PackingMaterialTypeId")));
            m.put("PackType", text(ci(d, "PackMaterilaType")));
            m.put("CropYearId", asInt(ci(d, "CropYearId")));
            m.put("CropYear", text(ci(d, "CropYear")));
            m.put("WarehouseId", 0);
            m.put("JobLotId", asInt(ci(d, "JobLotId")));
            m.put("OuterQty", asDouble(ci(d, "OuterQty")));
            m.put("UOMScheduleIdOuter", asInt(ci(d, "OuterQtyUomId")));
            m.put("OuterPackUOM", text(ci(d, "OuterUOM")));
            m.put("OuterEquivalent", asDouble(ci(d, "OuterEquivalent")));
            m.put("NetWeight", asDouble(ci(d, "NetWeight")));
            m.put("PackingWeight", asDouble(ci(d, "PackingWeight")));
            m.put("PackingWeightTotal", asDouble(ci(d, "TotalPackingWeight")));
            m.put("GrossWeight", asDouble(ci(d, "GrossWeight")));
            m.put("ContainerNo", "");
            m.put("SealNo", "");
            m.put("RatePrice", asDouble(ci(d, "RatePrice")));
            m.put("RateUomId", asInt(ci(d, "RateUomId")));
            totalQty += asDouble(ci(d, "OuterQty"));
            rows.add(m);
        }
        out.put("rows", rows);
        out.put("totalQty", clr(totalQty));
        return out;
    }

    /** ReadById(Id): the invoice header and its saved packing list (USP_ExImPackingListDetail_ReadById), update mode. */
    public Map<String, Object> readById(int invoiceId) {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", header(u, invoiceId));
        List<Map<String, Object>> rows = new ArrayList<>();
        double totalQty = 0;
        for (Map<String, Object> d : repo.packingListByInvoice(invoiceId)) {
            rows.add(savedRow(d));
            totalQty += asDouble(ci(d, "OuterQty"));
        }
        out.put("rows", rows);
        out.put("totalQty", clr(totalQty));
        out.put("canDelete", allowed(u, "Delete"));
        return out;
    }

    private static Map<String, Object> savedRow(Map<String, Object> d) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", asInt(ci(d, "Id")));
        m.put("CommercialInvoiceId", asInt(ci(d, "ExImInvoiceId")));
        m.put("CommercialInvoiceDetailId", asInt(ci(d, "ExImInvoicePackingDetailId")));
        m.put("RefDocumentTypeId", asInt(ci(d, "RefDocumentTypeId")));
        m.put("ItemId", asInt(ci(d, "ItemId")));
        m.put("ItemName", text(ci(d, "ItemName")));
        m.put("PackTypeId", asInt(ci(d, "ExImPackMaterilaTypeId")));
        m.put("PackType", text(ci(d, "PackingMaterial")));
        m.put("CropYearId", asInt(ci(d, "InvCropYearId")));
        m.put("CropYear", text(ci(d, "CropYear")));
        m.put("WarehouseId", asInt(ci(d, "WarehouseId")));
        m.put("WarehouseName", text(ci(d, "WareHouseName")));
        m.put("JobLotId", asInt(ci(d, "JobLotid")));
        m.put("JobLot", text(ci(d, "JobLotDescription")));
        m.put("OuterQty", asDouble(ci(d, "OuterQty")));
        m.put("UOMScheduleIdOuter", asInt(ci(d, "UOMScheduleIdOuter")));
        m.put("OuterPackUOM", text(ci(d, "OuterUomCode")));
        m.put("OuterEquivalent", asDouble(ci(d, "OuterUomEquivalent")));
        m.put("NetWeight", asDouble(ci(d, "NetWeight")));
        m.put("PackingWeight", asDouble(ci(d, "PackingWeight")));
        m.put("PackingWeightTotal", asDouble(ci(d, "PackingWeightTotal")));
        m.put("GrossWeight", asDouble(ci(d, "GrossWeight")));
        m.put("ContainerNo", text(ci(d, "ContainerNo")));
        m.put("SealNo", text(ci(d, "SealNo")));
        m.put("RatePrice", asDouble(ci(d, "RatePrice")));
        m.put("RateUomId", asInt(ci(d, "RateUomId")));
        return m;
    }

    /** GetDetailByHeaderId - the lower history grid. */
    public List<Map<String, Object>> historyDetail(int invoiceId) {
        user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : repo.packingListByInvoice(invoiceId)) out.add(savedRow(d));
        return out;
    }

    // ================================================================= history

    /** HistoryGridFill(50) on the History tab, HistoryGridFill() (all) from LoadAll. */
    public List<Map<String, Object>> history(int noOfRecords) {
        UserAccount u = user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.history(u, fy(), noOfRecords)) {
            out.add(row("Id", asInt(ci(r, "Id")), "VoucherHeadId", asInt(ci(r, "VoucherHeadId")), "InvoiceNo", text(ci(r, "InvoiceNo")),
                    "DocCode", asInt(ci(r, "DocCode")), "DocDate", iso(ci(r, "DocDate")), "CustomerName", text(ci(r, "Customer")),
                    "NoOfContainers", asDouble(ci(r, "NoOfContainers")), "GrossWeight", asDouble(ci(r, "GrossWeight")),
                    "NetWeight", asDouble(ci(r, "NetWeight")), "FcyAmount", asDouble(ci(r, "FCurrencyAmount")),
                    "AddLess", asDouble(ci(r, "AddLessAmount")), "TotalAmount", asDouble(ci(r, "TotalAmount")),
                    "LoadingPort", text(ci(r, "LoadingPort")), "DestinationPort", text(ci(r, "DestinationPort")),
                    "NoOfAttachments", text(ci(r, "NoOfAttachments"))));
        }
        return out;
    }

    // ================================================================= save / delete

    /**
     * Insert(): "Grid Record not found"; (the page asks Save / Update); detail Net / Gross totals against the
     * invoice (Math.Round(sum) - Math.Round(invoice, 2), |diff| > 0.99 -> the desktop's two messages, Gross summed
     * from the "#,##0.##"-rounded cells); the per-row checks with the desktop's text ("...\n In Row No" + RowIndex +
     * "1" - string concatenation, so row 0 reads "In Row No01"); InnerQty = OuterQty, UOMScheduleIdInner =
     * UOMScheduleIdOuter, SortNo set by the DAL; ActionTypeId 2 when the row has an Id, else 1; then
     * SetDataForCommercialInvoicePackingList with the invoice's ConversionRate; "Save SuccessFully" / "Update SuccessFully".
     */
    public Map<String, Object> save(Map<String, Object> body) {
        int recId = asInt(body.get("recId"));
        UserAccount u = user(recId > 0 ? "Update" : "Save");
        int invoiceId = asInt(body.get("invoiceId"));
        List<Map<String, Object>> rows = list(body.get("rows"));
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid Record not found");
        Map<String, Object> inv = shared.invoiceById(invoiceId);
        if (inv == null) throw new IllegalArgumentException("Index was outside the bounds of the array.");
        if (asInt(ci(inv, "CompanyId")) != u.getCompanyId() || asInt(ci(inv, "OrganizationId")) != u.getOrganizationId())
            throw new AccessDeniedException("Record belongs to another company");
        double net = 0, gross = 0;
        for (Map<String, Object> r : rows) {
            net += asDouble(r.get("NetWeight"));
            gross += BigDecimal.valueOf(asDouble(r.get("GrossWeight"))).setScale(2, RoundingMode.HALF_UP).doubleValue();
        }
        double hdrNet = asDouble(ci(inv, "NetWeight")), hdrGross = asDouble(ci(inv, "GrossWeight"));
        double value = Math.rint(net) - BigDecimal.valueOf(hdrNet).setScale(2, RoundingMode.HALF_EVEN).doubleValue();
        double diffGross = Math.rint(gross) - BigDecimal.valueOf(hdrGross).setScale(2, RoundingMode.HALF_EVEN).doubleValue();
        if (Math.abs(value) > 0.99) throw new IllegalArgumentException("Total NetWeight Of Detail grid Can not be greater than Invoice NetWeight...");
        if (Math.abs(diffGross) > 0.99) throw new IllegalArgumentException("Total GrossWeight Of Detail grid Can not be greater than Invoice GrossWeight...");

        ExportPackingDetailRepository.SaveInput in = new ExportPackingDetailRepository.SaveInput();
        int idx = 0;
        for (Map<String, Object> r : rows) {
            String rowNo = idx + "1";
            Map<String, Object> vd = new LinkedHashMap<>();
            vd.put("Id", asInt(r.get("Id")));
            vd.put("RefDocumentTypeId", DOCUMENT_TYPE_ID);
            vd.put("ExImInvoiceId", asInt(r.get("CommercialInvoiceId")));
            vd.put("ExImInvoicePackingDetailId", asInt(r.get("CommercialInvoiceDetailId")));
            if (asInt(r.get("ItemId")) == 0) throw new IllegalArgumentException("Item Field is Required in Grid...\n In Row No" + rowNo);
            vd.put("ItemId", asInt(r.get("ItemId")));
            if (asInt(r.get("CropYearId")) == 0) throw new IllegalArgumentException("CropYear Field is Required in Grid...\n In Row No" + rowNo);
            vd.put("InvCropYearId", asInt(r.get("CropYearId")));
            if (asInt(r.get("JobLotId")) == 0) throw new IllegalArgumentException("JobLot Field is Required in Grid...\n In Row No" + rowNo);
            vd.put("JobLotid", asInt(r.get("JobLotId")));
            if (asInt(r.get("PackTypeId")) == 0) throw new IllegalArgumentException("PackType Field is Required in Grid...\n In Row No" + rowNo);
            vd.put("ExImPackMaterilaTypeId", asInt(r.get("PackTypeId")));
            if (asDouble(r.get("OuterQty")) == 0.0) throw new IllegalArgumentException("OuterQty Field is Required in Grid...\n In Row No" + rowNo);
            vd.put("InnerQty", asDouble(r.get("OuterQty")));
            vd.put("UOMScheduleIdInner", asInt(r.get("UOMScheduleIdOuter")));
            vd.put("OuterQty", asDouble(r.get("OuterQty")));
            if (asInt(r.get("UOMScheduleIdOuter")) == 0) throw new IllegalArgumentException("OuterUom Field is Required in Grid...\n In Row No" + rowNo);
            vd.put("UOMScheduleIdOuter", asInt(r.get("UOMScheduleIdOuter")));
            vd.put("ActionTypeId", asInt(r.get("Id")) > 0 ? 2 : 1);
            if (asDouble(r.get("NetWeight")) == 0.0) throw new IllegalArgumentException("NetWeight Field is Required in Grid...\n In Row No" + rowNo);
            if (asDouble(r.get("GrossWeight")) == 0.0) throw new IllegalArgumentException("GrossWeight Field is Required in Grid...\n In Row No" + rowNo);
            vd.put("GrossWeight", asDouble(r.get("GrossWeight")));
            vd.put("NetWeight", asDouble(r.get("NetWeight")));
            vd.put("PackingWeight", asDouble(r.get("PackingWeight")));
            vd.put("PackingWeightTotal", asDouble(r.get("PackingWeightTotal")));
            vd.put("ContainerNo", text(r.get("ContainerNo")));
            vd.put("SealNo", text(r.get("SealNo")));
            if (asInt(r.get("WarehouseId")) == 0) throw new IllegalArgumentException("Warehouse Field is Required in Grid...\n In Row No" + rowNo);
            vd.put("WarehouseId", asInt(r.get("WarehouseId")));
            vd.put("SortNo", 1);
            vd.put("RateUomId", asInt(r.get("RateUomId")));
            vd.put("RatePrice", asDouble(r.get("RatePrice")));
            vd.put("CropYear", text(r.get("CropYear")));            // virtual, FIFO / validation only
            in.rows.add(vd);
            idx++;
        }
        in.invoiceId = invoiceId;
        /* txtDocDate / txtdocno / cmbSupCust are disabled boxes holding the invoice's own values (GetByID). */
        Object docDate = ci(inv, "DocDate");
        in.invoiceDate = docDate instanceof Timestamp ? (Timestamp) docDate : ts(iso(docDate));
        in.docNo = asInt(ci(inv, "DocCode"));
        in.supplierCustomerId = asInt(ci(inv, "SupplierCustomerId"));
        in.branchesId = u.getBranchesId() == null ? 0 : u.getBranchesId();
        in.userId = u.getId();
        in.exchangeRate = asDouble(ci(inv, "ConversionRate"));
        in.voucherHead = ExportPreInvoiceService.model(voucherHead(u, inv));
        in.factory = new ExportPackingDetailRepository.ModelFactory() {
            public Map<String, Object> evaluation() { return StockAdjustmentRepository.evaluationModel(); }
            public Map<String, Object> voucherDetail(Map<String, Object> values) { return detailModel(values); }
            public String num(double d) { return clr(d); }
        };
        repo.savePackingList(u, in);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", invoiceId);
        out.put("message", recId == 0 ? "Save SuccessFully" : "Update SuccessFully");
        return out;
    }

    /** The VoucherHead DAL SetDataForCommercialInvoicePackingList builds from the invoice (before its transaction). */
    private ContraVoucherDto.Head voucherHead(UserAccount u, Map<String, Object> inv) {
        ContraVoucherDto.Head vh = new ContraVoucherDto.Head();
        int cust = asInt(ci(inv, "SupplierCustomerId"));
        for (Map<String, Object> g : shared.customerGlAccounts(u)) {
            if (asInt(ci(g, "Id")) == cust) { vh.RefAccountId = asInt(ci(g, "GlAccountId")); break; }
        }
        String fmt = "yyyy-MM-dd HH:mm:ss";
        String now = LocalDateTime.now().withNano(0).format(DateTimeFormatter.ofPattern(fmt));
        vh.OrganizationId = asInt(ci(inv, "OrganizationId"));
        vh.CompanyId = asInt(ci(inv, "CompanyId"));
        vh.DocumentTypeId = asInt(ci(inv, "DocumentTypeId"));
        vh.DocumentTypeSrNo = asInt(ci(inv, "Id"));
        vh.RefDocNoId = asInt(ci(inv, "Id"));
        vh.VoucherDate = dateTime(ci(inv, "DocDate"));
        vh.VoucherCode = asInt(ci(inv, "DocCode"));
        vh.BranchId = asInt(ci(inv, "BranchesId"));
        vh.FinancialYearId = asInt(ci(inv, "FinancialYearId"));
        vh.ModifyUser = u.getId();
        vh.ModifyDate = now;
        vh.EntryDate = now;
        vh.ChequeDate = LocalDate.now() + " 00:00:00";
        vh.IncludeWHT = false;
        vh.ProjectId = asInt(ci(inv, "ProjectsId"));
        vh.BillAmount = asDouble(ci(inv, "TotalAmount"));
        vh.ManualBillNo = text(ci(inv, "InvoiceNo"));
        vh.DueDate = dateTime(ci(inv, "EFormDate"));
        vh.DueDays = 0;
        vh.Remarks = text(ci(inv, "RemarksHeader"));
        vh.RemarksOtherLingo = "";
        vh.VoucherAmount = asDouble(ci(inv, "EquivalentAmount"));
        vh.FcAmount = asDouble(ci(inv, "TotalAmount"));
        vh.ExchangeCurrencyRate = asDouble(ci(inv, "ConversionRate"));
        vh.MultiCurrencyId = asInt(ci(inv, "FcurrencyId"));
        return vh;
    }

    private static String dateTime(Object v) {
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().withNano(0).format(DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss"));
        String s = iso(v);
        return (s.isEmpty() ? LocalDate.now().toString() : s) + " 00:00:00";
    }

    /** A VoucherDetail model (ContraVoucherDto.Detail field order) with the given values set. */
    private static Map<String, Object> detailModel(Map<String, Object> values) {
        ContraVoucherDto.Detail d = new ContraVoucherDto.Detail();
        for (Map.Entry<String, Object> e : values.entrySet()) {
            try {
                Field f = ContraVoucherDto.Detail.class.getField(e.getKey());
                Object v = e.getValue();
                if (f.getType() == Integer.class) v = asInt(v);
                else if (f.getType() == Double.class) v = asDouble(v);
                else if (f.getType() == String.class) v = v == null ? null : String.valueOf(v);
                f.set(d, v);
            } catch (NoSuchFieldException | IllegalAccessException ex) {
                throw new IllegalStateException("VoucherDetail has no field " + e.getKey());
            }
        }
        return ExportPreInvoiceService.model(d);
    }

    /** btnDelete_Click: RecId > 0 -> (the page asks) -> RemoveByID(DocumentTypeId 211, Id = RecId) -> "Delete Export Packing list Successfully". */
    public Map<String, Object> delete(int recId) {
        UserAccount u = user("Delete");
        if (recId <= 0) throw new IllegalArgumentException("Record Not Found For Deletion");
        Map<String, Object> inv = shared.invoiceById(recId);
        if (inv == null || asInt(ci(inv, "CompanyId")) != u.getCompanyId() || asInt(ci(inv, "OrganizationId")) != u.getOrganizationId())
            throw new IllegalArgumentException("Record Not Found For Deletion");
        repo.delete(u, recId);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("message", "Delete Export Packing list Successfully");
        return out;
    }
}
