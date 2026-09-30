package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportInvoicePackingListRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeParseException;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.ExportGdBreakUpRepository.ci;

/**
 * The BLL side of 882 frmExportInvoicePackingList "Export Invoice Packing List"
 * (Architecture.WinApp.Export). ScreenName "frmExportInvoicePackingList", DocumentTypeId 204.
 *
 * The desktop form: pick a Commercial Invoice that a Delivery Order refers to; the header (all boxes
 * disabled) fills from USP_ExImInvoice_ReadByIdForPackingList and the grid seeds itself from the DO
 * rows (USP_InvDeliveryOrder_ReadByInvoiceForPackingList); the operator edits Commodity Detail, Pm
 * Detail, Package Date, Expiry Date, Lot No, Gross Weight and Remarks in the grid (the entry panel
 * "panelDetail" is Visible = false on the desktop), deletes rows with the X column, and saves. From
 * History the same invoice opens for Update (Edit button / double-click), Delete (BtnDelete, soft
 * delete of every row) and the three prints (529A / 529B / 529C).
 *
 * Rights: View to load, Save / Update / Delete / Print as the buttons are enabled on the desktop
 * (formrights.DoHaveSaveRight, DoHaveUpdateRights, DoHaveCanDelete, DoHavePrintRights). Organisation,
 * company, the active financial year and the audit user come from the session.
 */
@Service
public class ExportInvoicePackingListService {

    public static final int SCREEN_ID = 882;
    public static final int DOCUMENT_TYPE_ID = 204;

    @Autowired private ExportInvoicePackingListRepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private DesktopReportRights rights;

    private UserAccount user(String action) {
        UserAccount u = currentUserContext.requireAccountingUser();
        rights.require(u, SCREEN_ID, action);
        return u;
    }

    private boolean allowed(UserAccount u, String action) {
        try { rights.require(u, SCREEN_ID, action); return true; }
        catch (AccessDeniedException ex) { return false; }
    }

    private int financialYearId() { return currentUserContext.currentFinancialYearId(); }

    // ================================================================= load

    /**
     * InitializeComponentMethod: GetERPFeatureById(9) (Export Company box), rights,
     * GetDataForDropDownFromExportInvoice (history Customer / Invoice combos),
     * ExImInvoice_GetInvoicesRefferedInDo, GetConfigurationsFromGlobal (the desktop calls it only from
     * Refresh - at load DefaultDaysToLessFromHistoryFromDate is still 0, so From date = today - 3; the
     * same value is applied here), ChkPrintPreview checked.
     */
    public Map<String, Object> setup() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, "Save"));
        perm.put("Update", allowed(u, "Update"));
        perm.put("Delete", allowed(u, "Delete"));
        perm.put("Print", allowed(u, "Print"));
        out.put("permissions", perm);
        try { out.put("allowExportMultiCompanies", repo.erpFeature(u, 9)); }
        catch (Exception e) { out.put("allowExportMultiCompanies", false); }
        out.put("config", config(u));
        try { out.put("invoices", invoices(u)); } catch (Exception e) { out.put("invoices", new ArrayList<>()); out.put("invoicesError", msg(e)); }
        try { out.putAll(historyCombos(u)); } catch (Exception e) { out.put("historyCustomers", new ArrayList<>()); out.put("historyInvoices", new ArrayList<>()); out.put("historyCombosError", msg(e)); }
        return out;
    }

    /** GetConfigurationsFromGlobal: AllowExportMultiCompanies, DefaultDaysToLessFromHistoryFromDate, ItemSearchByCode. */
    public Map<String, Object> config(UserAccount u) {
        Map<String, Object> c = new LinkedHashMap<>();
        int days;
        try { days = (int) Double.parseDouble(repo.config(u, "DefaultDaysToLessFromHistoryFromDate")); }
        catch (NumberFormatException e) { days = 0; }
        c.put("defaultDaysToLessFromHistoryFromDate", days);
        c.put("itemSearchByCode", asBool(repo.config(u, "ItemSearchByCode")));
        c.put("allowExportMultiCompaniesConfig", asBool(repo.config(u, "AllowExportMultiCompanies")));
        return c;
    }

    /** btnRefresh_Click: configuration and, when the invoice combo is enabled, the invoice list. */
    public Map<String, Object> refresh() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("config", config(u));
        out.put("invoices", invoices(u));
        return out;
    }

    /** InvoiceNoBind - Id, InvoiceNo. */
    private List<Map<String, Object>> invoices(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.invoicesReferredInDo(u, financialYearId())) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            out.add(m);
        }
        return out;
    }

    /** HistoryComboBind: ActivityType "Customer" -> Customer combo, "Invoice" -> Invoice# combo (Id, name). */
    private Map<String, Object> historyCombos(UserAccount u) {
        List<Map<String, Object>> customers = new ArrayList<>(), invoices = new ArrayList<>();
        for (Map<String, Object> r : repo.historyDropDowns(u)) {
            String a = text(ci(r, "ActivityType"));
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("name", text(ci(r, "name")));
            if ("Customer".equals(a)) customers.add(m);
            else if ("Invoice".equals(a)) invoices.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("historyCustomers", customers);
        out.put("historyInvoices", invoices);
        return out;
    }

    /** btnRefreshHistory_Click. */
    public Map<String, Object> historyComboRefresh() { return historyCombos(user("View")); }

    // ================================================================= ReadById

    /**
     * ReadById(ID, History): the header row, dtdetail (the invoice's own detail, used by the weight
     * checks) and the grid rows - the saved packing list when opened from History, else the Delivery
     * Order rows (FillDetaildtsFromDeliveryOrder, distinct by DO detail id) with today as Package and
     * Expiry date and blank Commodity / Pm Detail / Lot No / Remarks.
     */
    public Map<String, Object> readById(int invoiceId, boolean history) {
        UserAccount u = user("View");
        if (invoiceId == 0) throw new IllegalArgumentException("Please Select an Invoice First");
        List<Map<String, Object>> hdr = repo.headerById(invoiceId);
        if (hdr.isEmpty()) throw new IllegalArgumentException("Record not found");
        Map<String, Object> row = hdr.get(0);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", header(row));
        out.put("detail", detailRows(repo.invoiceDetail(invoiceId)));
        List<Map<String, Object>> rows = new ArrayList<>();
        if (history) {
            for (Map<String, Object> d : repo.packingListDetail(invoiceId)) rows.add(packRow(d));
        } else {
            Set<Integer> seen = new HashSet<>();
            String today = LocalDate.now().toString();
            for (Map<String, Object> d : repo.deliveryOrderRows(invoiceId)) {
                int detailId = asInt(ci(d, "DetailId"));
                if (!seen.add(detailId)) continue;
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("Id", 0);
                m.put("DoDocumentTypeId", asInt(ci(d, "DocumentTypeId")));
                m.put("DoId", asInt(ci(d, "InvDeliveryOrderId")));
                m.put("DoDetailId", detailId);
                m.put("ContainerId", asInt(ci(d, "ContainerId")));
                m.put("ContainerNo", text(ci(d, "ContainerNo")));
                m.put("ItemId", asInt(ci(d, "ItemId")));
                m.put("ItemCode", text(ci(d, "ItemCode")));
                m.put("ItemName", text(ci(d, "ItemName")));
                m.put("ItemCommodityDetail", "");
                m.put("ItemPmDetail", "");
                m.put("PackageDate", today);
                m.put("ExpiryDate", today);
                m.put("LotNo", "");
                m.put("ThirdPartyAnalysisId", asInt(ci(d, "ThirdPartyAnalysisId")));
                m.put("ThirdPartyAnalysisNo", text(ci(d, "ThirdPartyAnalysisNo")));
                m.put("ThirdPartyAnalysisSubId", asInt(ci(d, "ThirdPartyAnalysisSubId")));
                m.put("ThirdPartyAnalysisSubNo", text(ci(d, "ThirdPartyAnalysisSubNo")));
                m.put("OuterQty", asDouble(ci(d, "OuterQty")));
                m.put("OuterUomId", asInt(ci(d, "OuterUomId")));
                m.put("OuterUom", text(ci(d, "OuterUom")));
                m.put("OuterUomEquivalent", asDouble(ci(d, "OuterUomEquivalent")));
                m.put("InnerQty", asDouble(ci(d, "InnerQty")));
                m.put("InnerUomId", asInt(ci(d, "InnerUomId")));
                m.put("InnerUom", text(ci(d, "InnerUom")));
                m.put("InnerUomEquivalent", asDouble(ci(d, "InnerUomEquivalent")));
                m.put("NetWeight", asDouble(ci(d, "NetWeight")));
                m.put("GrossWeight", asDouble(ci(d, "GrossWeight")));
                m.put("Remarks", "");
                rows.add(m);
            }
        }
        out.put("rows", rows);
        return out;
    }

    /** The header boxes exactly as ReadById fills them (the desktop's number formats are applied on the page). */
    private static Map<String, Object> header(Map<String, Object> row) {
        Map<String, Object> h = new LinkedHashMap<>();
        h.put("Id", asInt(ci(row, "Id")));
        h.put("InvoiceNo", text(ci(row, "InvoiceNo")));
        h.put("SupplierCustomerId", asInt(ci(row, "SupplierCustomerId")));
        h.put("ExportCompany", text(ci(row, "ExportCompany")));
        h.put("DocCode", text(ci(row, "DocCode")));
        h.put("DocDate", iso(ci(row, "DocDate")));
        h.put("DocumentaryCreditNo", text(ci(row, "DocumentaryCreditNo")));
        h.put("DocumentaryCreditNoIssueDate", iso(ci(row, "DocumentaryCreditNoIssueDate")));
        h.put("Customer", text(ci(row, "Customer")));
        h.put("Consignee", text(ci(row, "Consignee")));
        h.put("CustomerContractNos", text(ci(row, "CustomerContractNos")));
        h.put("ContractDates", text(ci(row, "ContractDates")));
        h.put("NotifyParty1Name", text(ci(row, "NotifyParty1Name")));
        h.put("NotifyParty2Name", text(ci(row, "NotifyParty2Name")));
        h.put("NotifyParty3Name", text(ci(row, "NotifyParty3Name")));
        h.put("ImporterBank", text(ci(row, "ImporterBank")));
        h.put("ExporterBank", text(ci(row, "ExporterBank")));
        h.put("LotNoRef", text(ci(row, "LotNoRef")));
        h.put("EFormNo", text(ci(row, "EFormNo")));
        h.put("EFormDate", iso(ci(row, "EFormDate")));
        h.put("LoadingPort", text(ci(row, "LoadingPort")));
        h.put("DestinationPort", text(ci(row, "DestinationPort")));
        h.put("CarierType", text(ci(row, "CarierType")));
        h.put("DeliveryTerm", text(ci(row, "DeliveryTerm")));
        h.put("DeliveryRemarks", text(ci(row, "DeliveryRemarks")));
        h.put("TCPRegNo", text(ci(row, "TCPRegNo")));
        h.put("ContinentName", text(ci(row, "ContinentName")));
        h.put("OriginCountry", text(ci(row, "OriginCountry")));
        h.put("ImporterCountry", text(ci(row, "ImporterCountry")));
        h.put("PlaceOfDelivery", text(ci(row, "PlaceOfDelivery")));
        h.put("CurrencyCode", text(ci(row, "CurrencyCode")));
        h.put("FCurrencyAmount", asDouble(ci(row, "FCurrencyAmount")));
        h.put("ConversionRate", asDouble(ci(row, "ConversionRate")));
        h.put("EquivalentAmount", asDouble(ci(row, "EquivalentAmount")));
        h.put("AddLessComments", text(ci(row, "AddLessComments")));
        h.put("AddLessAmount", asDouble(ci(row, "AddLessAmount")));
        h.put("TotalAmount", asDouble(ci(row, "TotalAmount")));
        h.put("NoOfContainers", asInt(ci(row, "NoOfContainers")));
        /* txtGrossWeight / txtNetWeight: the MTon column when > 0, else the kg column / 1000. */
        double grossMton = asDouble(ci(row, "GrossMton")), grossWeight = asDouble(ci(row, "GrossWeight"));
        double netMton = asDouble(ci(row, "NetMton")), netWeight = asDouble(ci(row, "NetWeight"));
        h.put("GrossMTons", grossMton > 0 ? grossMton : grossWeight / 1000.0);
        h.put("NetMTons", netMton > 0 ? netMton : netWeight / 1000.0);
        h.put("Certificate1", text(ci(row, "Certificate1")));
        h.put("Certificate2", text(ci(row, "Certificate2")));
        h.put("Remarks1", text(ci(row, "Remarks1")));
        h.put("Remarks2", text(ci(row, "Remarks2")));
        h.put("OtherRemarks1", text(ci(row, "OtherRemarks1")));
        h.put("OtherRemarks2", text(ci(row, "OtherRemarks2")));
        double fcyCustom = asDouble(ci(row, "FcyAmountCustom")), addLessCustom = asDouble(ci(row, "AddLessAmountCustom"));
        h.put("FcyAmountCustom", fcyCustom);
        h.put("AddLessAmountCustom", addLessCustom);
        h.put("TotalFcyAmountCustom", fcyCustom + addLessCustom);
        h.put("LocalAmountCustom", (fcyCustom + addLessCustom) * asDouble(ci(row, "ConversionRate")));
        return h;
    }

    /** dtdetail - the columns CheckItemIds and ValidatePackListWithDetailGrid read. */
    private static List<Map<String, Object>> detailRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ItemId", asInt(ci(d, "ItemId")));
            m.put("ItemName", text(ci(d, "ItemName")));
            m.put("OuterQty", asDouble(ci(d, "OuterQty")));
            m.put("MTon", asDouble(ci(d, "MTon")));
            m.put("NetWeight", asDouble(ci(d, "NetWeight")));
            m.put("GrossWeight", asDouble(ci(d, "GrossWeight")));
            out.add(m);
        }
        return out;
    }

    /** FillPackingListDetailFromListCommonForReadById - one dtPackingListDetail row. */
    private static Map<String, Object> packRow(Map<String, Object> d) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", asInt(ci(d, "Id")));
        m.put("DoDocumentTypeId", asInt(ci(d, "RefDocumentTypeId")));
        m.put("DoId", asInt(ci(d, "RefDocId")));
        m.put("DoDetailId", asInt(ci(d, "RefDocSubId")));
        m.put("ContainerId", asInt(ci(d, "ContainerId")));
        m.put("ContainerNo", text(ci(d, "ContainerNo")));
        m.put("ItemId", asInt(ci(d, "ItemId")));
        m.put("ItemCode", text(ci(d, "ItemCode")));
        m.put("ItemName", text(ci(d, "ItemName")));
        m.put("ItemCommodityDetail", text(ci(d, "CommodityDetail")));
        m.put("ItemPmDetail", text(ci(d, "PmDetail")));
        m.put("PackageDate", iso(ci(d, "PackageDate")));
        m.put("ExpiryDate", iso(ci(d, "ExpiryDate")));
        m.put("LotNo", text(ci(d, "LotNo")));
        m.put("ThirdPartyAnalysisId", asInt(ci(d, "ThirdPartyAnalysisId")));
        m.put("ThirdPartyAnalysisNo", text(ci(d, "ThirdPartyAnalysisNo")));
        m.put("ThirdPartyAnalysisSubId", asInt(ci(d, "ThirdPartyAnalysisSubId")));
        m.put("ThirdPartyAnalysisSubNo", text(ci(d, "ThirdPartyAnalysisSubNo")));
        m.put("OuterQty", asDouble(ci(d, "OuterQty")));
        m.put("OuterUomId", asInt(ci(d, "OuterUomId")));
        m.put("OuterUom", text(ci(d, "OuterUom")));
        m.put("OuterUomEquivalent", asDouble(ci(d, "OuterUomEquivalent")));
        m.put("InnerQty", asDouble(ci(d, "InnerQty")));
        m.put("InnerUomId", asInt(ci(d, "InnerUomId")));
        m.put("InnerUom", text(ci(d, "InnerUom")));
        m.put("InnerUomEquivalent", asDouble(ci(d, "InnerUomEquivalent")));
        m.put("NetWeight", asDouble(ci(d, "NetWeight")));
        m.put("GrossWeight", asDouble(ci(d, "GrossWeight")));
        m.put("Remarks", text(ci(d, "Remarks")));
        return m;
    }

    /** GetDetailGrdByHeadId - the lower History grid for the selected row. */
    public List<Map<String, Object>> packingListDetail(int invoiceId) {
        user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : repo.packingListDetail(invoiceId)) out.add(packRow(d));
        return out;
    }

    /** F1 on Commodity Detail (DetailTypeId 1) / Pm Detail (2): ItemCommodityDetailDbCall(itemId).Select("DetailTypeId=n"). */
    public List<Map<String, Object>> commodityRemarks(int itemId, int detailTypeId) {
        UserAccount u = user("View");
        if (itemId <= 0) throw new IllegalArgumentException("Please Select an Item First");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.commodityRemarks(u, itemId, 0)) {
            if (asInt(ci(r, "DetailTypeId")) != detailTypeId) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("Remarks", text(ci(r, "Remarks")));
            m.put("HSCode", text(ci(r, "HSCode")));
            out.add(m);
        }
        return out;
    }

    // ================================================================= save / delete

    /**
     * Insert(): grid empty -> "Detail Record Not Found"; header Gross MTons < Net MTons -> "Gross Weight
     * must be equal to or greater than Net Weight. Thank you."; CheckItemIds (invoice detail net / gross
     * kg minus the packing list's by more than 1 kg -> "Validation failed: Total Net Weight in Detail
     * (x kg) does not match PackList (y kg)."); then the removed rows (only when updating) and each grid
     * row (Id kept only when updating; 1 = insert, 2 = update) with FormHelper.ValidateField on
     * Container, Item, Commodity Detail, Pm Detail, Package Date, Expiry Date, Outer Qty, Net Weight and
     * Gross Weight; SavePackingList; "Save SuccessFully" / "Update SuccessFully".
     */
    public Map<String, Object> save(Map<String, Object> body) {
        int recId = asInt(body.get("recId"));
        UserAccount u = user(recId > 0 ? "Update" : "Save");
        int invoiceId = asInt(body.get("invoiceId"));
        if (recId > 0 && invoiceId != recId) throw new IllegalArgumentException("Rec Id not found...");
        if (invoiceId == 0) throw new IllegalArgumentException("Please Select an Invoice First");
        List<Map<String, Object>> rows = list(body.get("rows"));
        List<Map<String, Object>> removed = list(body.get("removed"));
        if (rows.isEmpty()) throw new IllegalArgumentException("Detail Record Not Found");

        List<Map<String, Object>> hdr = repo.headerById(invoiceId);
        if (hdr.isEmpty()) throw new IllegalArgumentException("Record not found");
        Map<String, Object> header = header(hdr.get(0));
        double gross = asDouble(header.get("GrossMTons")), net = asDouble(header.get("NetMTons"));
        if (gross < net) throw new IllegalArgumentException("Gross Weight must be equal to or greater than Net Weight. Thank you.");
        checkItemIds(detailRows(repo.invoiceDetail(invoiceId)), rows);

        List<Map<String, Object>> items = new ArrayList<>();
        if (recId > 0) {
            for (Map<String, Object> r : removed) {
                Map<String, Object> vd = model(r);
                vd.put("Id", asInt(r.get("Id")));
                vd.put("ActionTypeId", 3);
                items.add(vd);
            }
        }
        int i = 0;
        for (Map<String, Object> r : rows) {
            Map<String, Object> vd = model(r);
            int id = recId != 0 ? asInt(r.get("Id")) : 0;
            vd.put("Id", id);
            vd.put("ActionTypeId", id <= 0 ? 1 : 2);
            validateField(vd.get("ContainerId"), "Container", i);
            validateField(vd.get("ItemId"), "Item", i);
            validateField(vd.get("CommodityDetail"), "Commodity Detail", i);
            validateField(vd.get("PmDetail"), "Pm Detail", i);
            validateField(vd.get("PackageDate"), "Package Date", i);
            validateField(vd.get("ExpiryDate"), "Expiry Date", i);
            validateField(vd.get("OuterQty"), "Outer Qty", i);
            validateField(vd.get("NetWeight"), "Net Weight", i);
            validateField(vd.get("GrossWeight"), "Gross Weight", i);
            items.add(vd);
            i++;
        }
        int id = repo.savePackingList(invoiceId, items);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", id);
        out.put("message", recId == 0 ? "Save SuccessFully" : "Update SuccessFully");
        return out;
    }

    /** CheckItemIds(dtdetail, dtPackingListDetail). */
    private static void checkItemIds(List<Map<String, Object>> detail, List<Map<String, Object>> pack) {
        double dNet = 0, dGross = 0, pNet = 0, pGross = 0;
        for (Map<String, Object> r : detail) { dNet += asDouble(r.get("NetWeight")); dGross += asDouble(r.get("GrossWeight")); }
        for (Map<String, Object> r : pack) { pNet += asDouble(r.get("NetWeight")); pGross += asDouble(r.get("GrossWeight")); }
        if (dNet - pNet > 1.0)
            throw new IllegalArgumentException("Validation failed: Total Net Weight in Detail (" + num(dNet) + " kg) does not match PackList (" + num(pNet) + " kg).");
        if (dGross - pGross > 1.0)
            throw new IllegalArgumentException("Validation failed: Total Gross Weight in Detail (" + num(dGross) + " kg) does not match PackList (" + num(pGross) + " kg).");
    }

    /**
     * FillDetailListCommonForInsertAndDelete + the model's remaining non-virtual properties, as
     * GenericProvider.SetProc sends them. EntryUserId / ModifyUserId are never set by the form (0);
     * EntryDate / ModifyDate are null DateTime? and therefore not sent (the procedure stamps GETDATE()).
     * ItemName, ItemCode, ThirdPartyAnalysisNo, the Uom names, the equivalents and ContainerNo are
     * virtual and never sent.
     */
    private static Map<String, Object> model(Map<String, Object> r) {
        Map<String, Object> vd = new LinkedHashMap<>();
        vd.put("Id", 0);
        vd.put("ExImInvoiceId", 0);
        vd.put("RefDocumentTypeId", asInt(r.get("DoDocumentTypeId")));
        vd.put("RefDocId", asInt(r.get("DoId")));
        vd.put("RefDocSubId", asInt(r.get("DoDetailId")));
        vd.put("ContainerId", asInt(r.get("ContainerId")));
        vd.put("ItemId", asInt(r.get("ItemId")));
        vd.put("CommodityDetail", text(r.get("ItemCommodityDetail")));
        vd.put("PmDetail", text(r.get("ItemPmDetail")));
        vd.put("PackageDate", ts(r.get("PackageDate")));
        vd.put("ExpiryDate", ts(r.get("ExpiryDate")));
        vd.put("LotNo", text(r.get("LotNo")));
        vd.put("ThirdPartyAnalysisId", asInt(r.get("ThirdPartyAnalysisId")));
        vd.put("ThirdPartyAnalysisSubId", asInt(r.get("ThirdPartyAnalysisSubId")));
        vd.put("SubLotNo", text(r.get("ThirdPartyAnalysisSubNo")));
        vd.put("OuterQty", asDouble(r.get("OuterQty")));
        vd.put("OuterUomId", asInt(r.get("OuterUomId")));
        vd.put("InnerQty", asDouble(r.get("InnerQty")));
        vd.put("InnerUomId", asInt(r.get("InnerUomId")));
        vd.put("NetWeight", asDouble(r.get("NetWeight")));
        vd.put("GrossWeight", asDouble(r.get("GrossWeight")));
        vd.put("Remarks", text(r.get("Remarks")));
        vd.put("EntryUserId", 0);
        vd.put("ModifyUserId", 0);
        vd.put("ActionTypeId", 0);
        return vd;
    }

    /** BtnDelete_Click: RecId == 0 -> "RecId not found"; PackingListDeleteByInvoiceId(cmbInvoiceNo.Value, UserAccount.ID). */
    public Map<String, Object> delete(int recId) {
        UserAccount u = user("Delete");
        if (recId == 0) throw new IllegalArgumentException("RecId not found");
        repo.deleteByInvoiceId(recId, u.getId());
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("message", "Delete Record Successfully");
        return out;
    }

    // ================================================================= history

    /**
     * HistoryGridFill: Ids = "204"; the ticked From/To go to the pair of the checked radio (Doc / Entry /
     * Modify / Approved date); FromDocNo, ToDocNo, SupplierCustomerId (Customer), Id (Invoice#) when
     * non-zero; ActionId 2 Not Referred (the default), 1 Referred, 0 All; ApprovedFilter "All".
     */
    public List<Map<String, Object>> history(Map<String, Object> f) {
        UserAccount u = user("View");
        Map<String, Object> g = new LinkedHashMap<>();
        LocalDate from = asBool(f.get("fromChecked")) ? asDate(f.get("fromDate")) : null;
        LocalDate to = asBool(f.get("toChecked")) ? asDate(f.get("toDate")) : null;
        String by = text(f.get("dateBy"));
        String fromKey = "FromDate", toKey = "ToDate";
        if ("entry".equals(by)) { fromKey = "EntryFromDate"; toKey = "EntryToDate"; }
        else if ("modify".equals(by)) { fromKey = "ModifyFromDate"; toKey = "ModifyToDate"; }
        else if ("approved".equals(by)) { fromKey = "ApprovedFromDate"; toKey = "ApprovedToDate"; }
        if (from != null) g.put(fromKey, java.sql.Date.valueOf(from));
        if (to != null) g.put(toKey, java.sql.Date.valueOf(to));
        int cust = asInt(f.get("customerId"));
        if (cust != 0) g.put("SupplierCustomerId", cust);
        g.put("DocumentTypeIds", String.valueOf(DOCUMENT_TYPE_ID));
        int id = asInt(f.get("invoiceId"));
        if (id != 0) g.put("Id", id);
        int fromDoc = asInt(f.get("fromDocNo")), toDoc = asInt(f.get("toDocNo"));
        if (fromDoc != 0) g.put("DocNoFrom", fromDoc);
        if (toDoc != 0) g.put("DocNoTo", toDoc);
        String ref = text(f.get("referred"));
        int actionId = "referred".equals(ref) ? 1 : "all".equals(ref) ? 0 : 2;
        if (actionId != 0) g.put("ActionId", actionId);

        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.history(u, financialYearId(), g)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("DocCode", asInt(ci(r, "DocCode")));
            m.put("DocDate", iso(ci(r, "DocDate")));
            m.put("CustomerName", text(ci(r, "Customer")));
            m.put("NoOfContainers", asDouble(ci(r, "NoOfContainers")));
            m.put("GrossWeight", asDouble(ci(r, "GrossWeight")));
            m.put("NetWeight", asDouble(ci(r, "NetWeight")));
            m.put("FcyAmount", asDouble(ci(r, "FCurrencyAmount")));
            m.put("AddLess", asDouble(ci(r, "AddLessAmount")));
            m.put("TotalAmount", asDouble(ci(r, "TotalAmount")));
            m.put("LoadingPort", text(ci(r, "LoadingPort")));
            m.put("DestinationPort", text(ci(r, "DestinationPort")));
            m.put("EntryUser", text(ci(r, "EntryUserName")));
            m.put("EntryDate", isoDateTime(ci(r, "EntryDate")));
            m.put("ModifyUser", text(ci(r, "ModifyUserName")));
            m.put("ModifyDate", isoDateTime(ci(r, "ModifyDate")));
            m.put("NoOfAttachments", asInt(ci(r, "NoOfAttachments")));
            out.add(m);
        }
        return out;
    }

    // ================================================================= helpers

    private static void validateField(Object value, String field, int rowIndex) {
        boolean bad = value == null
                || (value instanceof Integer && (Integer) value == 0)
                || (value instanceof Double && (Double) value <= 0)
                || (value instanceof String && ((String) value).trim().isEmpty());
        if (bad) throw new IllegalArgumentException(field + " is required in Packing List at row No: " + (rowIndex + 1));
    }

    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> list(Object v) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (v instanceof List) for (Object o : (List<Object>) v) if (o instanceof Map) out.add((Map<String, Object>) o);
        return out;
    }

    private static String msg(Exception e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        return t.getMessage() == null ? String.valueOf(e.getMessage()) : t.getMessage();
    }

    /** double.ToString() - an integral value prints without a decimal part. */
    private static String num(double d) {
        return d == Math.rint(d) && Math.abs(d) < 1e15 ? String.valueOf((long) d) : String.valueOf(d);
    }

    private static String text(Object v) { return v == null ? "" : String.valueOf(v).trim(); }

    static int asInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        try { return Integer.parseInt(String.valueOf(v).trim()); }
        catch (NumberFormatException e) {
            try { return (int) Double.parseDouble(String.valueOf(v).trim().replace(",", "")); }
            catch (NumberFormatException e2) { return 0; }
        }
    }

    static double asDouble(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).doubleValue();
        try { return Double.parseDouble(String.valueOf(v).trim().replace(",", "")); }
        catch (NumberFormatException e) { return 0; }
    }

    private static boolean asBool(Object v) {
        if (v == null) return false;
        if (v instanceof Boolean) return (Boolean) v;
        String s = String.valueOf(v).trim().toLowerCase();
        return "true".equals(s) || "1".equals(s) || "on".equals(s);
    }

    private static LocalDate asDate(Object v) {
        if (v == null) return null;
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return null;
        if (s.length() > 10) s = s.substring(0, 10);
        try { return LocalDate.parse(s); }
        catch (DateTimeParseException e) { return null; }
    }

    private static Timestamp ts(Object v) {
        if (v instanceof Timestamp) return (Timestamp) v;
        if (v == null) return null;
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return null;
        try { return Timestamp.valueOf(LocalDateTime.parse(s.length() == 10 ? s + "T00:00:00" : s.length() == 16 ? s + ":00" : s)); }
        catch (DateTimeParseException e) {
            try { return Timestamp.valueOf(s.replace('T', ' ')); }
            catch (IllegalArgumentException e2) { return null; }
        }
    }

    static String iso(Object v) {
        if (v == null) return "";
        if (v instanceof java.sql.Timestamp) return ((java.sql.Timestamp) v).toLocalDateTime().toLocalDate().toString();
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString();
        if (v instanceof java.util.Date) return new java.sql.Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().toLocalDate().toString();
        if (v instanceof java.time.LocalDateTime) return ((java.time.LocalDateTime) v).toLocalDate().toString();
        if (v instanceof java.time.LocalDate) return v.toString();
        String s = String.valueOf(v).trim();
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }

    /** yyyy-MM-ddTHH:mm:ss for the "dd-MM-yyyy hh:mm tt" history columns. */
    static String isoDateTime(Object v) {
        if (v == null) return "";
        if (v instanceof java.sql.Timestamp) return ((java.sql.Timestamp) v).toLocalDateTime().withNano(0).toString();
        if (v instanceof java.util.Date) return new java.sql.Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().withNano(0).toString();
        if (v instanceof java.time.LocalDateTime) return ((java.time.LocalDateTime) v).withNano(0).toString();
        return String.valueOf(v).trim();
    }
}
