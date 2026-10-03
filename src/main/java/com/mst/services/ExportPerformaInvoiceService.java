package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportPerformaInvoiceRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.ExportInvoiceTransferSupport.*;

/**
 * BLL side of 200 "Export Performa Invoice" - Architecture.WinApp.Export.ExImProformaInvoice (caption
 * "Export : Performa Invoice", DocumentTypeId 200, table ExImProformaInvoice).
 *
 * Desktop behaviour kept:
 *  - formvalidation order and texts: Branch / Project / Doc No / Destination Port / Loading Port / Fcy Code /
 *    "No Of Containers Must Be Greater Than 0" / Status. The customer is NOT validated (the desktop does not); an empty
 *    customer is sent as 0, which then fails the history join (INNER JOIN SupplierCustomer) - kept.
 *  - Status is saved as the combo TEXT (Open / Complete / Cancel, a fixed DataTable on the desktop).
 *  - The detail model converts with Conversion.ToInt: QtyInner, QtyOuter and NetWeightKgs lose their decimals
 *    (NetWeightKgs is an int column) - kept; ItemRate and FcAmount stay doubles.
 *  - FcAmountTotal is never set by the form (0 is sent). IsApproved false, EntryDate/ModifyDate = now, Entry/ModifyUser = user.
 *  - Save and Update are the same handler; Update sends Sp_ExImProformaInvoice_Update (which deletes the old detail rows).
 *  - "Save SuccessFully" / "Update SuccessFully" only when the returned value > 0.
 * Rights: View / Save / Update of ScreenDefinition 200 (the desktop form has no rights checks of its own; the web port
 * applies the screen's rights as every screen does). Tenancy and user from the session only.
 */
@Service
public class ExportPerformaInvoiceService {

    public static final int SCREEN_ID = 200;
    public static final int DOCUMENT_TYPE_ID = 200;

    @Autowired private ExportPerformaInvoiceRepository repo;
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

    /** ImProformaInvoice_Load: generateCode, branches, projects, customers, sea ports, fcy code, status, items, pack types. */
    public Map<String, Object> setup() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, "Save"));
        perm.put("Update", allowed(u, "Update"));
        out.put("permissions", perm);
        out.put("docNo", docNo(u));
        out.put("branches", pick(repo.branches(u), "Id", "BranchName"));
        out.put("projects", pick(repo.projects(u), "Id", "ProjectName"));
        out.put("customers", pick(repo.customers(u), "Id", "CompanyName"));
        out.put("ports", pick(repo.seaPorts(u), "Id", "PortName"));
        out.put("currencies", pick(repo.currencies(u), "Id", "CurrencyCode"));
        out.put("items", pick(repo.items(u), "Id", "ItemName"));
        out.put("packTypes", pick(repo.packTypes(u), "Id", "Description"));
        return out;
    }

    /** generateCode: the DocNo when > 0, otherwise the box keeps its (empty) text. */
    private String docNo(UserAccount u) {
        int c = repo.generateCode(u, DOCUMENT_TYPE_ID);
        return c > 0 ? String.valueOf(c) : "";
    }

    public Map<String, Object> newCode() { return row("docNo", docNo(user("View"))); }

    /** DetailFormReset -> item(): the item list is re-read. */
    public List<Map<String, Object>> items() { return pick(repo.items(user("View")), "Id", "ItemName"); }

    /** PackUOM (cmbItem ValueChanged): Id + Equivalent for Rate / Inner / Outer UOM. */
    public List<Map<String, Object>> uoms(int itemId) {
        UserAccount u = user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.uoms(u, itemId)) out.add(row("Id", asInt(ci(r, "Id")), "Equivalent", text(ci(r, "Equivalent"))));
        return out;
    }

    /** HistoryGridFill (tab "History" selected): the desktop's projected columns. */
    public List<Map<String, Object>> history() {
        UserAccount u = user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.history(u)) {
            out.add(row("Id", asInt(ci(r, "Id")), "DocNo", text(ci(r, "DocNo")), "DocDate", ddMMMyyyy(ci(r, "DocDate")),
                    "Customer", text(ci(r, "CompanyName")), "NoOfContainer", asInt(ci(r, "NoOfContainer")),
                    "LoadingPort", text(ci(r, "LoadingPort")), "DestinationPort", text(ci(r, "DestinationPort")),
                    "LastShipmentDate", ddMMMyyyy(ci(r, "LastShipmentDate")), "Status", text(ci(r, "Status")),
                    "Remarks", text(ci(r, "RemarksHeader"))));
        }
        return out;
    }

    /** ReadById (history double-click). */
    public Map<String, Object> readById(int id) {
        UserAccount u = user("View");
        Map<String, Object> h = repo.header(id);
        if (asInt(ci(h, "CompanyId")) != u.getCompanyId() || asInt(ci(h, "OrganizationId")) != u.getOrganizationId())
            throw new AccessDeniedException("Record belongs to another company");
        Map<String, Object> hd = new LinkedHashMap<>();
        hd.put("Id", asInt(ci(h, "Id")));
        hd.put("BranchId", asInt(ci(h, "BranchId")));
        hd.put("ProjectId", asInt(ci(h, "ProjectId")));
        hd.put("DocDate", iso(ci(h, "DocDate")));
        hd.put("DocNo", text(ci(h, "DocNo")));
        hd.put("SupplierCustomerId", asInt(ci(h, "SupplierCustomerId")));
        hd.put("CustomerRefNo", text(ci(h, "CustomerRefNo")));
        hd.put("QuotReference", text(ci(h, "QuotReference")));
        hd.put("InquieryReference", text(ci(h, "InquieryReference")));
        hd.put("SeaPortsIdDestination", asInt(ci(h, "SeaPortsIdDestination")));
        hd.put("SeaPortsIdLoading", asInt(ci(h, "SeaPortsIdLoading")));
        hd.put("MultiCurrencyId", asInt(ci(h, "MultiCurrencyId")));
        hd.put("NoOfContainer", text(ci(h, "NoOfContainer")));
        hd.put("LastShipmentDate", iso(ci(h, "LastShipmentDate")));
        hd.put("Status", text(ci(h, "Status")));
        hd.put("RemarksHeader", text(ci(h, "RemarksHeader")));
        hd.put("ShipmentDetail", text(ci(h, "ShipmentDetail")));
        hd.put("PaymentDetail", text(ci(h, "PaymentDetail")));
        hd.put("OtherTermsConditions", text(ci(h, "OtherTermsConditions")));
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : repo.detail(id)) {
            /* dtdetail.Rows.Add(ItemId, ItemName, Description, PackTypeId, PackTypeName, QtyInner, UOMScheduleIdInner,
               UOMScheduleIdInnerName, QtyOuter, UOMScheduleIdOuter, UOMScheduleIdOuterName, NetWeightKgs, ItemRate,
               UOMScheduleIdRate, UOMScheduleIdRateName, FcAmount) - the *Name columns are the UOM Equivalent numbers. */
            rows.add(row("ItemId", asInt(ci(d, "ItemId")), "Item", text(ci(d, "ItemName")), "ItemDesc", text(ci(d, "Description")),
                    "PackTypeId", asInt(ci(d, "ExImPackMaterialTypeId")), "PackType", text(ci(d, "ExImPackMaterialTypeName")),
                    "InnerQty", clr(asDouble(ci(d, "QtyInner"))), "InnerUOMId", asInt(ci(d, "UOMScheduleIdInner")),
                    "InnerUOM", clr(asDouble(ci(d, "UOMScheduleIdInnerName"))),
                    "OuterQty", clr(asDouble(ci(d, "QtyOuter"))), "OuterUOMId", asInt(ci(d, "UOMScheduleIdOuter")),
                    "OuterUOM", clr(asDouble(ci(d, "UOMScheduleIdOuterName"))),
                    "Weight", asDouble(ci(d, "NetWeightKgs")), "Rate", asDouble(ci(d, "ItemRate")),
                    "RateUOMId", asInt(ci(d, "UOMScheduleIdRate")), "RateUOM", clr(asDouble(ci(d, "UOMScheduleIdRateName"))),
                    "FcyAmount", asDouble(ci(d, "FcAmount"))));
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", hd);
        out.put("rows", rows);
        return out;
    }

    /** btnsave_Click / btnupdate_Click. */
    public Map<String, Object> save(Map<String, Object> body) {
        int recId = asInt(body.get("recId"));
        boolean updateMode = asBool(body.get("updateMode")) && recId > 0;
        UserAccount u = user(updateMode ? "Update" : "Save");
        Map<String, Object> h = map(body.get("header"));
        validate(h);
        if (updateMode) {
            Map<String, Object> cur = repo.header(recId);
            if (asInt(ci(cur, "CompanyId")) != u.getCompanyId() || asInt(ci(cur, "OrganizationId")) != u.getOrganizationId())
                throw new AccessDeniedException("Record belongs to another company");
        }
        java.sql.Timestamp now = now();
        /* GenericProvider.SetProc: every non-virtual property of Model.Export.ExImProformaInvoice, declaration order. */
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("IsApproved", false);
        m.put("DocDate", pickerDate(h.get("txtDocDate")));
        m.put("EntryDate", now);
        m.put("LastShipmentDate", pickerDate(h.get("txtLastDate")));
        m.put("ModifyDate", now);
        m.put("FcAmountTotal", 0.0);
        m.put("BranchId", asInt(h.get("cmbbranches")));
        m.put("CompanyId", u.getCompanyId());
        m.put("DocNo", asInt(h.get("txtdocno")));
        m.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        m.put("EntryUser", u.getId());
        m.put("Id", updateMode ? recId : 0);
        m.put("ModifyUser", u.getId());
        m.put("MultiCurrencyId", asInt(h.get("cmbFcyCode")));
        m.put("NoOfContainer", asInt(h.get("txtNoofContainer")));
        m.put("OrganizationId", u.getOrganizationId());
        m.put("ProjectId", asInt(h.get("cmbproject")));
        m.put("SeaPortsIdDestination", asInt(h.get("cmbDestinationPort")));
        m.put("SeaPortsIdLoading", asInt(h.get("cmbLoadingPort")));
        m.put("SupplierCustomerId", asInt(h.get("cmbCustomer")));
        m.put("CustomerRefNo", text(h.get("txtcustomerRef")));
        m.put("InquieryReference", text(h.get("txtinquiryRef")));
        m.put("OtherTermsConditions", text(h.get("txttermNCondition")).trim());
        m.put("PaymentDetail", text(h.get("txtPaymentDetail")).trim());
        m.put("QuotReference", text(h.get("txtquotRef")));
        m.put("RemarksHeader", text(h.get("txtRemarks")).trim());
        m.put("ShipmentDetail", text(h.get("txtShipmentDetail")).trim());
        m.put("Status", text(h.get("cmbStatusText")));
        List<Map<String, Object>> details = new ArrayList<>();
        for (Map<String, Object> r : list(body.get("rows"))) {
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("FcAmount", asDouble(r.get("FcyAmount")));
            d.put("ItemRate", asDouble(r.get("Rate")));
            d.put("QtyInner", (double) asInt(r.get("InnerQty")));
            d.put("QtyOuter", (double) asInt(r.get("OuterQty")));
            d.put("ExImPackMaterialTypeId", asInt(r.get("PackTypeId")));
            d.put("ExImProformaInvoiceHeaderId", 0);
            d.put("Id", 0);
            d.put("ItemId", asInt(r.get("ItemId")));
            d.put("NetWeightKgs", asInt(r.get("Weight")));
            d.put("UOMScheduleIdInner", asInt(r.get("InnerUOMId")));
            d.put("UOMScheduleIdOuter", asInt(r.get("OuterUOMId")));
            d.put("UOMScheduleIdRate", asInt(r.get("RateUOMId")));
            d.put("Description", text(r.get("ItemDesc")));
            /* RemarksDetail is never set -> CLR null -> not sent. */
            details.add(d);
        }
        int success = repo.save(m, updateMode ? "Sp_ExImProformaInvoice_Update" : "Sp_ExImProformaInvoice_Insert", details);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", success > 0);
        out.put("message", success > 0 ? (updateMode ? "Update SuccessFully" : "Save SuccessFully") : "");
        return out;
    }

    /** formvalidation. */
    private void validate(Map<String, Object> h) {
        if (text(h.get("cmbbranchesText")).trim().isEmpty()) throw new IllegalArgumentException("Branch Is Required");
        if (text(h.get("cmbprojectText")).trim().isEmpty()) throw new IllegalArgumentException("Project Is Required");
        if (text(h.get("txtdocno")).isEmpty()) throw new IllegalArgumentException("Doc No Is Required");
        if (text(h.get("cmbDestinationPortText")).trim().isEmpty()) throw new IllegalArgumentException("Destination Port Is Required");
        if (text(h.get("cmbLoadingPortText")).trim().isEmpty()) throw new IllegalArgumentException("Loading Port Is Required");
        if (text(h.get("cmbFcyCodeText")).trim().isEmpty()) throw new IllegalArgumentException("Fcy Code Is Required");
        if (asInt(h.get("txtNoofContainer")) <= 0) throw new IllegalArgumentException("No Of Containers Must Be Greater Than 0");
        if (text(h.get("cmbStatusText")).trim().isEmpty()) throw new IllegalArgumentException("Status Is Required");
    }
}
