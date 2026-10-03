package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportGdFiPopupsRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.ExportQSupport.*;

/**
 * BLL side of two pop-up forms of 952 GD / Bank Invoice Mapping (Architecture.WinApp.Export), neither with a
 * ScreenDefinition row of its own:
 *
 *   GdContainerBreakUp  "GD Container BreakUp"                          /export/gd-container-break-up?gdId=
 *   frmFIOpening        "FI Opening Balances (Balance Advance Payment)"  /export/fi-opening[?id=]
 *
 * Rights: the parent screen 952 (no rights code on either desktop form except grid-bar options, so "View"
 * gates every action). Tenancy and the user come from the session.
 */
@Service
public class ExportGdFiPopupsService {

    public static final int[] PARENTS = {952};

    @Autowired private ExportGdFiPopupsRepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private DesktopReportRights rights;

    private UserAccount user() {
        UserAccount u = currentUserContext.requireAccountingUser();
        requireAny(rights, u, PARENTS, "View");
        return u;
    }

    // ================================================================================================
    // GdContainerBreakUp
    // ================================================================================================

    /** GdNoBind: GDBreakUpHeader.getGdsForGdContainerBreakUp(CompanyId, GdIdFromBreakUp) - Id, GDNO, ExImInvoiceId (hidden), InvoiceNo. */
    public Map<String, Object> gds(int gdIdFromBreakUp) {
        UserAccount u = user();
        Map<String, Object> out = new LinkedHashMap<>();
        put(out, "gds", () -> project(repo.gdsForContainerBreakUp(u, gdIdFromBreakUp),
                "Id", "Id", "GDNO", "GDNO", "ExImInvoiceId", "ExImInvoiceId", "InvoiceNo", "InvoiceNo"));
        return out;
    }

    /** CmbGdNo_Leave: getGdContainerBreakUpAgainstGd(CompanyId, Id) into dtGdContainerBreakUp's columns. */
    public List<Map<String, Object>> containerRows(int gdId) {
        UserAccount u = user();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.containerBreakUpAgainstGd(u, gdId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("GdId", asInt(ci(r, "GdId")));
            m.put("GdNo", text(ci(r, "GDNO")));
            m.put("ExImInvoiceId", asInt(ci(r, "ExImInvoiceId")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("ContainerNo", text(ci(r, "ContainerNo")));
            m.put("ContainerSize", text(ci(r, "ContainerSize")));
            m.put("NoOfBags", asDouble(ci(r, "NoOfBags")));
            m.put("NetWeight", asDouble(ci(r, "NetWeight")));
            out.add(m);
        }
        return out;
    }

    /**
     * Insert(): grid empty -> "Detail Grid Not found.Please Enter at Least one entry"; every grid row becomes a
     * GdContainerBreakUp model (NetWeight, NoOfBags, ExImInvoiceId, GdId, Id = 0, ContainerNo, ContainerSize -
     * the model's property order); GDBreakUpHeader.SaveGdContainerBreakUp; "Save SuccessFully". The page asks
     * "Are you sure to Save?" before the request. The add-row rules (form validation, one GD per grid) are
     * repeated so a hand-made request cannot skip them.
     */
    public Map<String, Object> saveContainers(Map<String, Object> body) {
        user();
        List<Map<String, Object>> rows = list(body.get("rows"));
        if (rows.isEmpty()) throw new IllegalArgumentException("Detail Grid Not found.Please Enter at Least one entry");
        int gd = asInt(rows.get(0).get("GdId"));
        List<Map<String, Object>> items = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            if (asInt(r.get("GdId")) != gd) throw new IllegalArgumentException("You can only add data against one Gd in the grid");
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("NetWeight", dec(r.get("NetWeight")));
            d.put("NoOfBags", dec(r.get("NoOfBags")));
            d.put("ExImInvoiceId", asInt(r.get("ExImInvoiceId")));
            d.put("GdId", asInt(r.get("GdId")));
            d.put("Id", 0);
            d.put("ContainerNo", raw(r.get("ContainerNo")));
            d.put("ContainerSize", raw(r.get("ContainerSize")));
            items.add(d);
        }
        int id = repo.saveContainerBreakUp(items);
        return saved(id, "Save SuccessFully");
    }

    // ================================================================================================
    // frmFIOpening
    // ================================================================================================

    /** InitializeComponentMethod: FormHistory -> BindGrid, FcyCodeBind, BankBind, ExportCustomersFillFomGlobal. */
    public Map<String, Object> fiSetup() {
        UserAccount u = user();
        Map<String, Object> out = new LinkedHashMap<>();
        put(out, "history", () -> fiRows(repo.fiOpeningHistory(u)));
        combos(out, u);
        return out;
    }

    /** btnRefresh_Click: global parties re-read, then the three combos. */
    public Map<String, Object> fiCombos() {
        UserAccount u = user();
        Map<String, Object> out = new LinkedHashMap<>();
        combos(out, u);
        return out;
    }

    public Map<String, Object> fiHistory() {
        UserAccount u = user();
        Map<String, Object> out = new LinkedHashMap<>();
        put(out, "history", () -> fiRows(repo.fiOpeningHistory(u)));
        return out;
    }

    private void combos(Map<String, Object> out, UserAccount u) {
        /* FcyCodeBind: MultiCurrency.GetAll, AllColumns. */
        put(out, "currencies", () -> project(repo.currencies(u), "Id", "Id", "CurrencyCode", "CurrencyCode",
                "CurrencyName", "CurrencyName", "CurrencyRate", "CurrencyRate", "CurrencySymbol", "CurrencySymbol"));
        /* BankBind: only IsHomeland == "Home Country"; Id, BranchName, BankIBANNo. */
        put(out, "banks", () -> {
            List<Map<String, Object>> b = new ArrayList<>();
            for (Map<String, Object> r : repo.banks(u)) {
                if (!"Home Country".equals(raw(ci(r, "IsHomeland")))) continue;
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("Id", asInt(ci(r, "Id")));
                m.put("BranchName", text(ci(r, "BranchName")));
                m.put("BankIBANNo", text(ci(r, "BankIBANNo")));
                b.add(m);
            }
            return b;
        });
        /* ExportCustomersFillFomGlobal: CustomerGroupId == 7; Id, CompanyName, GlAccountId (hidden), GlAccountCurrency, GlAccountCurrencyId (hidden). */
        put(out, "consignees", () -> {
            List<Map<String, Object>> c = new ArrayList<>();
            for (Map<String, Object> r : repo.supplierCustomers(u)) {
                if (asInt(ci(r, "CustomerGroupId")) != 7) continue;
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("Id", asInt(ci(r, "Id")));
                m.put("CompanyName", text(ci(r, "CompanyName")));
                m.put("GlAccountId", asInt(ci(r, "GlAccountId")));
                m.put("GlAccountCurrency", text(ci(r, "GlAccountCurrencyCode")));
                m.put("GlAccountCurrencyId", asInt(ci(r, "GlAccountCurrencyId")));
                c.add(m);
            }
            return c;
        });
    }

    /** BindGrid: dtHistoryGrid's 17 columns in its order. */
    private static List<Map<String, Object>> fiRows(List<Map<String, Object>> dt) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : dt) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("OpeningDate", isoDateTime(ci(r, "OpeningDate")));
            m.put("FIDate", isoDateTime(ci(r, "FIDate")));
            m.put("FINumber", text(ci(r, "FINumber")));
            m.put("FIAmount", asDouble(ci(r, "FIAmount")));
            m.put("ExpiryDate", isoDateTime(ci(r, "ExpiryDate")));
            m.put("FcyId", asInt(ci(r, "FcyId")));
            m.put("FcyCode", text(ci(r, "FcyCode")));
            m.put("ExchangeRate", asDouble(ci(r, "ExchangeRate")));
            m.put("LcyAmount", asDouble(ci(r, "LcyAmount")));
            m.put("BankName", text(ci(r, "BankName")));
            m.put("Consignee", text(ci(r, "Consignee")));
            m.put("Remarks", text(ci(r, "Remarks")));
            m.put("EntryUser", text(ci(r, "EntryUser")));
            m.put("EntryDate", isoDateTime(ci(r, "EntryDate")));
            m.put("ModifyUser", text(ci(r, "ModifyUser")));
            m.put("ModifyDate", isoDateTime(ci(r, "ModifyDate")));
            out.add(m);
        }
        return out;
    }

    /**
     * Insert(): ValidateInputs (desktop texts and order), then FIOpening.Save with the 19 non-virtual model
     * properties (DocumentTypeId 1; the BLL stamps EntryDate / ModifyDate with now). The toolbar Save sends
     * recId 0 (btnSave_Click sets RecId = 0); Update sends the loaded RecId.
     */
    public Map<String, Object> saveFi(Map<String, Object> body) {
        UserAccount u = user();
        int recId = asInt(body.get("recId"));
        String fiNumber = raw(body.get("fiNumber"));
        if (fiNumber.trim().isEmpty()) throw new IllegalArgumentException("FI Number field is required");
        if (asDouble(body.get("fiAmount")) == 0.0) throw new IllegalArgumentException("FIAmount field is required");
        if (asInt(body.get("fcyId")) == 0) throw new IllegalArgumentException("Fcy Code field is required");
        if (asDouble(body.get("exchangeRate")) == 0.0) throw new IllegalArgumentException("Exchange Rate field is required");
        if (asDouble(body.get("lcyAmount")) == 0.0) throw new IllegalArgumentException("LcyAmount field is required");
        java.sql.Timestamp now = now();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("EntryDate", now);
        m.put("FIDate", ts(body.get("fiDate")));
        m.put("ModifyDate", now);
        m.put("OpeningDate", ts(body.get("openingDate")));
        m.put("ExpiryDate", ts(body.get("expiryDate")));
        m.put("ExchangeRate", dec(body.get("exchangeRate")));
        m.put("FIAmount", dec(body.get("fiAmount")));
        m.put("LcyAmount", dec(body.get("lcyAmount")));
        m.put("CompanyId", u.getCompanyId());
        m.put("DocumentTypeId", 1);
        m.put("EntryUserId", u.getId());
        m.put("FcyId", asInt(body.get("fcyId")));
        m.put("BankId", asInt(body.get("bankId")));
        m.put("ConsigneeId", asInt(body.get("consigneeId")));
        m.put("Id", recId);
        m.put("ModifyUserId", u.getId());
        m.put("OrganizationId", u.getOrganizationId());
        m.put("FINumber", fiNumber.trim());
        m.put("Remarks", raw(body.get("remarks")));
        int id = repo.saveFiOpening(m);
        return saved(id, recId > 0 ? "Record updated successfully." : "Record saved successfully.");
    }
}
