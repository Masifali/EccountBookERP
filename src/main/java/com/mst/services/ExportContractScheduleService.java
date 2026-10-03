package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportContractScheduleRepository;
import com.mst.repositories.ExportSalesContractRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.temporal.ChronoUnit;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.ExportGSupport.*;

/**
 * The BLL side of 216 frmSaleContractSchedule "Export Contract Schedule" (Architecture.WinApp.Export),
 * rights screen FrmExportSalesContractSchedule, DocumentTypeId 244, ClientSize 1619 x 699.
 *
 * tabControlMain: "Main Form" | "Pre Shipment Planning". Main Form = tabControl1 "Form" | "History"
 * ("HistoryNotInUse" is removed at Load). Form: "Contracts Info" (pending contracts, Not Reffered /
 * Reffered / All, Load All, 395-Slip), "Contract Schedule" (the editable main grid: FCL, M_Ton, Loading
 * / Production / Inspection / Packing Material dates, Destination Port, Document Custom Group, Remarks;
 * X / + / Add Attachment / Document Custom Group Save buttons) and "Contract Schedule Brand Detail
 * Loading Date wise" (the Detail entry: Loading Date, Item / Brand, Crop Year, Packing Type, No Of Bags,
 * Pack Uom, Inner Qty, Weight / M.Ton, Bal MTon, and the grid grouped by Loading Date / Destination
 * Port). History: Loading Date From / To, Port, Customer, Item, Show, 551-Print, 551_01-Print.
 * Pre Shipment Planning: From / To Date Loading, Add Days, the department grid and its entry.
 *
 * Rights: View, Save (btnsave / btnSavedetailSchedule), Update (history Edit), Print.
 */
@Service
public class ExportContractScheduleService {

    public static final int SCREEN_ID = 216;
    public static final int DOCUMENT_TYPE_ID = 244;

    @Autowired private ExportContractScheduleRepository repo;
    @Autowired private ExportSalesContractRepository contracts;
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

    /** frmSaleContractSchedule_Load. */
    public Map<String, Object> setup() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, "Save"));
        perm.put("Update", allowed(u, "Update"));
        perm.put("Print", allowed(u, "Print"));
        out.put("permissions", perm);
        boolean sdf = false, pmrp = false;
        try { sdf = repo.erpFeature(u, 12); pmrp = repo.erpFeature(u, 23); } catch (Exception e) { /* off */ }
        out.put("shipmentDocumentsFeature", sdf);
        out.put("packingMaterialPlanningFeature", pmrp);
        out.put("config", config(u));
        try { out.put("ports", pairs(repo.seaPorts(u), "Id", "PortName")); } catch (Exception e) { out.put("ports", new ArrayList<>()); }
        try { out.put("cropYears", pairs(repo.cropYears(u), "Id", "CropYear")); } catch (Exception e) { out.put("cropYears", new ArrayList<>()); }
        try { out.put("packTypes", pairs(repo.packTypes(u), "Id", "Description")); } catch (Exception e) { out.put("packTypes", new ArrayList<>()); }
        try { out.put("plants", pairs(repo.productionPlants(u), "Id", "Description")); } catch (Exception e) { out.put("plants", new ArrayList<>()); }
        /* ActionIdForContractGridBid = 1 -> 50 records, Not Reffered */
        try { out.put("pending", pending(u, 1, 50)); } catch (Exception e) { out.put("pending", new ArrayList<>()); out.put("pendingError", msg(e)); }
        try { out.putAll(historyCombos(u)); } catch (Exception e) { out.put("historyPorts", new ArrayList<>()); out.put("historyCustomers", new ArrayList<>()); out.put("historyItems", new ArrayList<>()); }
        try { out.put("latestAttentiveLoadDate", iso(repo.latestAttentiveLoadDate(u))); } catch (Exception e) { out.put("latestAttentiveLoadDate", ""); }
        return out;
    }

    /** skipFCLContractValidationOnSchedule, ExportSalesContractApprovalMandatory. */
    private Map<String, Object> config(UserAccount u) {
        Map<String, Object> c = new LinkedHashMap<>();
        c.put("skipFCLContractValidationOnSchedule", asBool(repo.config(u, "skipFCLContractValidationOnSchedule")));
        c.put("exportSalesContractApprovalMandatory", asBool(repo.config(u, "ExportSalesContractApprovalMandatory")));
        return c;
    }

    /** BtnRefreshScheduleMain_Click: skipFCL config and the ports (the grid's Destination Port list). */
    public Map<String, Object> refreshMain() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("config", config(u));
        out.put("ports", pairs(repo.seaPorts(u), "Id", "PortName"));
        return out;
    }

    /** GetpendingContractsforShipmentSchedule(NoOfRecords, ActionId) - tablePendingContracts. */
    public List<Map<String, Object>> pending(UserAccount u, int actionId, int noOfRecords) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.pendingContracts(u, actionId, noOfRecords)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "ExImLcOrderId")));
            m.put("ContractNo", text(ci(r, "LcOrderNo")));
            m.put("ContractDate", iso(ci(r, "LcOrderDate")));
            m.put("NoOfContainers", asDouble(ci(r, "NoOfContainers")));
            m.put("WeightM_Ton", asDouble(ci(r, "WeightM_Ton")));
            m.put("SupplierCustomerId", asInt(ci(r, "SupCustId")));
            m.put("CustomerName", text(ci(r, "Customer")));
            m.put("DestinationPortId", asInt(ci(r, "DestinationPortId")));
            m.put("StartShipmentDate", iso(ci(r, "ShipmentStartDate")));
            m.put("LastShipmentDate", iso(ci(r, "LastShipmentDate")));
            m.put("IsApproved", asBool(ci(r, "IsApproved")));
            m.put("ApprovedStatus", text(ci(r, "ApprovedStatus")));
            out.add(m);
        }
        return out;
    }

    public List<Map<String, Object>> pendingApi(int actionId, int noOfRecords) { return pending(user("View"), actionId, noOfRecords); }

    /** ComboBindForHistory: GetDataForDropDownFromShipmentSchedule -> DestinationPort / Customer / Item. */
    private Map<String, Object> historyCombos(UserAccount u) {
        List<Map<String, Object>> ports = new ArrayList<>(), customers = new ArrayList<>(), items = new ArrayList<>();
        for (Map<String, Object> r : repo.historyDropDowns(u)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("name", text(ci(r, "ReferenceName")));
            String a = text(ci(r, "Activity"));
            if ("DestinationPort".equals(a)) ports.add(m);
            else if ("Customer".equals(a)) customers.add(m);
            else if ("Item".equals(a)) items.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("historyPorts", ports);
        out.put("historyCustomers", customers);
        out.put("historyItems", items);
        return out;
    }

    public Map<String, Object> historyCombosApi() { return historyCombos(user("View")); }

    // ================================================================= contract read

    /**
     * grdPendingContractByContractId / ReadById: ReadbyMainScheduleByContractId (the schedule rows,
     * NetWeight / 1000, PackingMaterialDate defaulting to today when null), ReadScheduleDetailByContactId,
     * AttentiveLoadDateFillByContractId, ItemNameFillByContractId and the Document Custom Groups of
     * the first schedule row's customer (ContractScheduleGridComboFill).
     */
    public Map<String, Object> contract(int contractId) {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> main = mainRows(repo.scheduleByContract(contractId));
        out.put("scheduleMain", main);
        out.put("scheduleDetail", detailRows(repo.packingDetailByContract(contractId)));
        out.put("loadDates", loadDates(contractId, null));
        out.put("items", items(u, contractId));
        int cust = 0;
        for (Map<String, Object> r : main) { if (asInt(r.get("SupplierCustomerId")) > 0) { cust = asInt(r.get("SupplierCustomerId")); break; } }
        try { out.put("customGroups", pairs(repo.customGroupsByCustomer(u, cust), "CustomGroupId", "CustomGroup")); } catch (Exception e) { out.put("customGroups", new ArrayList<>()); }
        List<Map<String, Object>> hdr = contracts.header(contractId);
        out.put("contractWeightMTon", hdr.isEmpty() ? 0 : asDouble(ci(hdr.get(0), "NetWeightKgs")));
        out.put("contractNoOfContainers", hdr.isEmpty() ? 0 : asDouble(ci(hdr.get(0), "NoOfContainers")));
        out.put("contractNo", hdr.isEmpty() ? "" : text(ci(hdr.get(0), "LcOrderNo")));
        return out;
    }

    static List<Map<String, Object>> mainRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        String today = LocalDate.now().toString();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("ContractId", asInt(ci(r, "ExImLcOrderId")));
            m.put("ContractNo", text(ci(r, "LcOrderNo")));
            m.put("ScheduleNo", text(ci(r, "ScheduleCode")));
            m.put("SupplierCustomerId", asInt(ci(r, "SupplierCustomerId")));
            m.put("CustomerName", text(ci(r, "CustomerName")));
            m.put("CustomerContractNo", text(ci(r, "CustomerContractNo")));
            m.put("NoOfContainers", asDouble(ci(r, "NoOfContainer")));
            m.put("WeightM_Ton", asDouble(ci(r, "NetWeight")) / 1000.0);
            m.put("AttentiveLoadingDate", iso(ci(r, "AttentiveLoadingDate")));
            m.put("DestinationPortId", asInt(ci(r, "DestinationPortId")));
            m.put("DestinationPort", text(ci(r, "DestinationPort")));
            m.put("AttentiveProductionDate", iso(ci(r, "AttentiveProductionDate")));
            m.put("AttentiveInspectionDate", iso(ci(r, "AttentiveInspectionDate")));
            String pmd = iso(ci(r, "PackingMaterialDate"));
            m.put("PackingMaterialDate", pmd.isEmpty() || pmd.startsWith("1900") || pmd.startsWith("0001") ? today : pmd);
            m.put("CustomGroupId", asInt(ci(r, "CustomGroupId")));
            m.put("Remarks", text(ci(r, "ScheduleRemarks")));
            m.put("Status", text(ci(r, "Status")));
            m.put("ReferredStatus", text(ci(r, "RefferedStatus")));
            m.put("NoOfAttachments", asInt(ci(r, "NoOfAttachments")));
            m.put("IsApproved", asBool(ci(r, "ContractApproveStatus")));
            m.put("EntryDate", iso(ci(r, "EntryDate")));
            m.put("EntryUser", text(ci(r, "EntryUser")));
            m.put("ModifyDate", iso(ci(r, "ModifyDate")));
            m.put("ModifyUser", text(ci(r, "ModifyUser")));
            out.add(m);
        }
        return out;
    }

    /** ReadScheduleDetailByContactId - the "table" rows; DestinationPort = "dd-MMM-yyyy port scheduleCode". */
    static List<Map<String, Object>> detailRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("ScheduleId", asInt(ci(r, "ExImLcOrderShipmentScheduleId")));
            m.put("ContractId", asInt(ci(r, "ExImLcOrderId")));
            m.put("ContractDetailId", asInt(ci(r, "ContractDetailId")));
            m.put("AttentiveLoadingDate", iso(ci(r, "AttentiveLoadingDate")));
            m.put("DestinationPort", ddMMMyyyy(iso(ci(r, "AttentiveLoadingDate"))) + " " + text(ci(r, "DestinationPort")) + " " + text(ci(r, "ScheduleCode")));
            m.put("ItemId", asInt(ci(r, "ItemId")));
            m.put("Item", text(ci(r, "ItemName")));
            m.put("ItemCode", text(ci(r, "ItemCode")));
            m.put("CropYearId", asInt(ci(r, "CropYearId")));
            m.put("CropYear", text(ci(r, "CropYear")));
            m.put("PackingTypeId", asInt(ci(r, "PackingTypeId")));
            m.put("PackingType", text(ci(r, "PackingType")));
            m.put("NoOfBags", asDouble(ci(r, "NoOfBags")));
            m.put("PackUomId", asInt(ci(r, "PackUomId")));
            m.put("PackUom", text(ci(r, "PackUom")));
            m.put("InnerQty", asDouble(ci(r, "InnerQty")));
            m.put("WeightM_Ton", asDouble(ci(r, "NetWeight")) / 1000.0);
            m.put("Status", text(ci(r, "Status")));
            m.put("ReferredStatus", text(ci(r, "ReferredStatus")));
            out.add(m);
        }
        return out;
    }

    private static final String[] MON = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

    /** DateTime.ToString("dd-MMM-yyyy") of a yyyy-MM-dd string. */
    static String ddMMMyyyy(String isoDate) {
        if (isoDate == null || isoDate.length() < 10) return "";
        int mon = Integer.parseInt(isoDate.substring(5, 7));
        return isoDate.substring(8, 10) + "-" + MON[mon - 1] + "-" + isoDate.substring(0, 4);
    }

    /** AttentiveLoadDateFillByContractId(Id, DetailRowIds) - ScheduleId, AttentiveLoadingDate ("dd-MMM-yyy"), ContractId, DestinationPort, ScheduleNo. */
    public List<Map<String, Object>> loadDates(int contractId, String detailRecIds) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.attentiveLoadDates(contractId, detailRecIds)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ScheduleId", asInt(ci(r, "ScheduleId")));
            String d = iso(ci(r, "AttentiveLoadingDate"));
            m.put("AttentiveLoadingDate", ddMMMyyyy(d));
            m.put("LoadingDateIso", d);
            m.put("ContractId", asInt(ci(r, "ContractId")));
            m.put("DestinationPort", text(ci(r, "DestinationPort")));
            m.put("ScheduleNo", text(ci(r, "ScheduleCode")));
            out.add(m);
        }
        return out;
    }

    public List<Map<String, Object>> loadDatesApi(int contractId, String detailRecIds) { user("View"); return loadDates(contractId, detailRecIds); }

    /** ItemNameFillByContractId - dtitem (ContractDetailId is the combo's value). */
    public List<Map<String, Object>> items(UserAccount u, int contractId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.itemsBySalesContract(u, contractId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "ItemId")));
            m.put("ItemName", text(ci(r, "ItemName")));
            m.put("ContractDetailId", asInt(ci(r, "ContractDetailId")));
            m.put("RatePrice", asDouble(ci(r, "RatePrice")));
            m.put("ItemCode", text(ci(r, "ItemCode")));
            m.put("CropYearId", asInt(ci(r, "CropYearId")));
            m.put("CropYear", text(ci(r, "CropYear")));
            m.put("PackTypeId", asInt(ci(r, "InvPackingMaterialTypeId")));
            m.put("PackType", text(ci(r, "PackType")));
            m.put("PackUomId", asInt(ci(r, "ItemUomId")));
            m.put("PackUom", text(ci(r, "PackSize")));
            m.put("OuterQty", asDouble(ci(r, "Qty")));
            m.put("M_Ton", asDouble(ci(r, "MTon")));
            m.put("IsApproved", asBool(ci(r, "IsApproved")));
            out.add(m);
        }
        return out;
    }

    public List<Map<String, Object>> itemsApi(int contractId) { return items(user("View"), contractId); }

    /** bindItemPackUom: GetUomScheduleByItemId(item) - Id, UOMCode, Equivalent, QtyEquivalent, BaseRateUom. */
    public List<Map<String, Object>> uom(int itemId) {
        UserAccount u = user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.uomByItem(u, itemId)) out.add(ExportSalesContractService.uomRow(r));
        return out;
    }

    /** GetTotalMTonOfDate: NetWeight / 1000 (and NoOfContainer) of the schedule row. */
    public Map<String, Object> scheduleMton(int contractId, int scheduleId) {
        user("View");
        List<Map<String, Object>> r = repo.containersBySchedule(contractId, scheduleId);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("mton", r.isEmpty() ? 0.0 : asDouble(ci(r.get(0), "NetWeight")) / 1000.0);
        out.put("noOfContainer", r.isEmpty() ? 0.0 : asDouble(ci(r.get(0), "NoOfContainer")));
        return out;
    }

    public List<Map<String, Object>> customGroupsApi(int supplierCustomerId) {
        return pairs(repo.customGroupsByCustomer(user("View"), supplierCustomerId), "CustomGroupId", "CustomGroup");
    }

    // ================================================================= saves

    /**
     * InsertScheduleMain: the removed rows (ActionId 3) first, then each grid row with the desktop's
     * row checks ("AttentiveLoadingDate cannot be null in Row No:n", "FCL Required in Row No:n",
     * "WeightM_Ton Required in Row No:n", "DestinationPort Required in Row No:n", "Schedule No Required
     * in Grid"), the total M.Ton against the contract's, then ExImLcOrderShipmentSchedule.Save.
     * "Main Schedule Save SuccessFully " / "Main Schedule Update SuccessFully".
     */
    public Map<String, Object> saveMain(Map<String, Object> b) {
        UserAccount u = user("Save");
        List<Map<String, Object>> rows = list(b.get("rows"));
        List<Map<String, Object>> removed = list(b.get("removed"));
        boolean update = false;
        for (Map<String, Object> r : rows) if (asInt(r.get("Id")) > 0) { update = true; break; }
        if (rows.isEmpty()) throw new IllegalArgumentException("Schedule Grid have no row");
        List<Map<String, Object>> list = new ArrayList<>();
        for (Map<String, Object> r : removed) list.add(mainModel(u, r, 3, false));
        double weightDetail = 0;
        int n = 1;
        for (Map<String, Object> r : rows) {
            if (dateNull(asDate(r.get("AttentiveLoadingDate")))) throw new IllegalArgumentException("AttentiveLoadingDate cannot be null in Row No:" + n);
            if (asDouble(r.get("NoOfContainers")) == 0) throw new IllegalArgumentException("FCL Required in Row No:" + n);
            if (dec(r.get("WeightM_Ton")).signum() == 0) throw new IllegalArgumentException("WeightM_Ton Required in Row No:" + n);
            weightDetail += asDouble(r.get("WeightM_Ton"));
            if (asInt(r.get("DestinationPortId")) == 0) throw new IllegalArgumentException("DestinationPort Required in Row No:" + n);
            if (text(r.get("ScheduleNo")).isEmpty()) throw new IllegalArgumentException("Schedule No Required in Grid");
            list.add(mainModel(u, r, asInt(r.get("Id")) <= 0 ? 1 : 2, true));
            n++;
        }
        /* Weight_MTon_Contract: the pending grid's WeightM_Ton on the desktop; the page sends it, the contract's NetWeightKgs is the fallback. */
        double contractWeight = b.containsKey("contractWeightMTon") ? asDouble(b.get("contractWeightMTon")) : 0;
        if (!b.containsKey("contractWeightMTon")) {
            List<Map<String, Object>> hdr = contracts.header(asInt(rows.get(0).get("ContractId")));
            contractWeight = hdr.isEmpty() ? 0 : asDouble(ci(hdr.get(0), "NetWeightKgs"));
        }
        if (weightDetail > contractWeight)
            throw new IllegalArgumentException("WeightM_Ton " + num(weightDetail) + " can not be Greater than total WeightM_Ton:" + num(contractWeight) + " of This Contract");
        repo.saveScheduleMain(list);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("update", update);
        out.put("message", update ? "Main Schedule Update SuccessFully" : "Main Schedule Save SuccessFully ");
        return out;
    }

    /** The ExImLcOrderShipmentSchedule model in property order (removed rows carry what the delete branch sets). */
    private Map<String, Object> mainModel(UserAccount u, Map<String, Object> r, int actionId, boolean full) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("AttentiveInspectionDate", ts(r.get("AttentiveInspectionDate")));
        m.put("AttentiveLoadingDate", ts(r.get("AttentiveLoadingDate")));
        m.put("AttentiveProductionDate", ts(r.get("AttentiveProductionDate")));
        m.put("PackingMaterialDate", ts(r.get("PackingMaterialDate")));
        m.put("EntryDate", now());
        m.put("ModifyDate", now());
        m.put("NetWeight", dec(r.get("WeightM_Ton")).multiply(BigDecimal.valueOf(1000)));
        m.put("ActionId", actionId);
        m.put("BranchesId", u.getBranchesId() == null ? 0 : u.getBranchesId());
        m.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        m.put("CompanyId", u.getCompanyId());
        m.put("DestinationPortId", asInt(r.get("DestinationPortId")));
        m.put("CustomGroupId", asInt(r.get("CustomGroupId")));
        m.put("EntryUserId", u.getId());
        m.put("ExImLcOrderId", asInt(r.get("ContractId")));
        m.put("Id", asInt(r.get("Id")));
        m.put("ModifyUserId", u.getId());
        m.put("NoOfContainer", asInt(r.get("NoOfContainers")));
        m.put("OrganizationId", u.getOrganizationId());
        m.put("FinancialYearId", full ? financialYearId() : 0);
        m.put("SortNo", 0);
        m.put("SupplierCustomerId", asInt(r.get("SupplierCustomerId")));
        m.put("ScheduleCode", full ? text(r.get("ScheduleNo")) : (r.get("ScheduleNo") == null ? null : text(r.get("ScheduleNo"))));
        m.put("ScheduleRemarks", full ? text(r.get("Remarks")) : null);
        m.put("AttachmentsValues", text(r.get("AttachmentsValues")));
        m.put("CustomAttachmentsValues", text(r.get("CustomAttachmentsValues")));
        m.put("CustomerContractNo", text(r.get("CustomerContractNo")));
        return m;
    }

    /**
     * InsertScheduleDetail: the removed rows (ActionTypeId 3) first, then every grid row whose
     * ReferredStatus is not "Referred" with the desktop's checks ("Item Required in Row No:n",
     * "CropYear ...", "PackingType ...", "NoOfBags ...", "PackUom ...", "WeightM_Ton ..."), then
     * ExImLcOrderShipmentSchedule.SaveDetail. "Detail Schedule Save SuccessFully" / "... Update SuccessFully".
     */
    public Map<String, Object> saveDetail(Map<String, Object> b) {
        user("Save");
        List<Map<String, Object>> rows = list(b.get("rows"));
        List<Map<String, Object>> removed = list(b.get("removed"));
        boolean update = false;
        for (Map<String, Object> r : rows) if (asInt(r.get("Id")) > 0) { update = true; break; }
        if (rows.isEmpty()) throw new IllegalArgumentException("Schedule Grid have no row");
        List<Map<String, Object>> list = new ArrayList<>();
        for (Map<String, Object> r : removed) list.add(detailModel(r, 3));
        int n = 1;
        for (Map<String, Object> r : rows) {
            if (!"Referred".equals(text(r.get("ReferredStatus")))) {
                if (asInt(r.get("ItemId")) == 0) throw new IllegalArgumentException("Item Required in Row No:" + n);
                if (asInt(r.get("CropYearId")) == 0) throw new IllegalArgumentException("CropYear Required in Row No:" + n);
                if (asInt(r.get("PackingTypeId")) == 0) throw new IllegalArgumentException("PackingType Required in Row No:" + n);
                if (asInt(r.get("NoOfBags")) == 0) throw new IllegalArgumentException("NoOfBags Required in Row No:" + n);
                if (asInt(r.get("PackUomId")) == 0) throw new IllegalArgumentException("PackUom Required in Row No:" + n);
                if (dec(r.get("WeightM_Ton")).signum() == 0) throw new IllegalArgumentException("WeightM_Ton Required in Row No:" + n);
                list.add(detailModel(r, asInt(r.get("Id")) <= 0 ? 1 : 2));
            }
            n++;
        }
        repo.saveScheduleDetail(list);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("update", update);
        out.put("message", update ? "Detail Schedule Update SuccessFully" : "Detail Schedule Save SuccessFully");
        return out;
    }

    private static Map<String, Object> detailModel(Map<String, Object> r, int actionTypeId) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("InnerQty", asDouble(r.get("InnerQty")));
        m.put("NetWeight", dec(r.get("WeightM_Ton")).multiply(BigDecimal.valueOf(1000)));
        m.put("CropYearId", asInt(r.get("CropYearId")));
        m.put("ExImLcOrderId", asInt(r.get("ContractId")));
        m.put("ContractDetailId", actionTypeId == 3 ? 0 : asInt(r.get("ContractDetailId")));
        m.put("ExImLcOrderShipmentScheduleId", asInt(r.get("ScheduleId")));
        m.put("Id", asInt(r.get("Id")));
        m.put("ItemId", asInt(r.get("ItemId")));
        m.put("NoOfBags", asInt(r.get("NoOfBags")));
        m.put("PackingTypeId", asInt(r.get("PackingTypeId")));
        m.put("PackUomId", asInt(r.get("PackUomId")));
        m.put("ActionTypeId", actionTypeId);
        return m;
    }

    /** grdContractShedule "Save" button: ExportContractSchedule.CustomGroupDocumentSave(244, Id, CustomGroupId). */
    public Map<String, Object> customGroupSave(Map<String, Object> b) {
        user("Save");
        int id = asInt(b.get("id")), cg = asInt(b.get("customGroupId"));
        if (id <= 0) throw new IllegalArgumentException("This record is not saved so can't save custom documents");
        if (cg <= 0) throw new IllegalArgumentException("Custom Group field is required");
        repo.customGroupDocumentSave(DOCUMENT_TYPE_ID, id, cg);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("message", "Documents save successfully");
        return out;
    }

    // ================================================================= history

    /** DataGridHistoryFill(50): the filter dates are always sent (the pickers have no check box). */
    public Map<String, Object> history(Map<String, Object> f) {
        UserAccount u = user("View");
        LocalDate from = asDate(f.get("fromDate")), to = asDate(f.get("toDate"));
        List<Map<String, Object>> raw = repo.formHistory(u, sqlDate(from), sqlDate(to), asInt(f.get("customerId")), asInt(f.get("portId")), asInt(f.get("itemId")));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : raw) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("RecordNo", asInt(ci(r, "RecordNo")));
            m.put("Id", asInt(ci(r, "Id")));
            m.put("ExImLcOrderId", asInt(ci(r, "ExImLcOrderId")));
            m.put("ContractNo", text(ci(r, "ContractNo")));
            m.put("SupplierCustomerId", asInt(ci(r, "SupplierCustomerId")));
            m.put("CustomerName", text(ci(r, "CustomerName")));
            m.put("ItemName", text(ci(r, "ItemName")));
            m.put("NoOfContainer", asInt(ci(r, "NoOfContainer")));
            m.put("MTon", asDouble(ci(r, "MTon")));
            m.put("Contract_MTon", asDouble(ci(r, "Contract_MTon")));
            m.put("MtonUsedInForwarding", asDouble(ci(r, "MtonUsedInForwarding")));
            m.put("ContractBalMton", asDouble(ci(r, "ContractBalMton")));
            m.put("DestinationPort", text(ci(r, "DestinationPort")));
            m.put("LoadingDate", iso(ci(r, "AttentiveLoadingDate")));
            m.put("ProductionDate", iso(ci(r, "AttentiveProductionDate")));
            m.put("InspectionDate", iso(ci(r, "AttentiveInspectionDate")));
            m.put("PackingMaterialDate", iso(ci(r, "PackingMaterialDate")));
            m.put("Remarks", text(ci(r, "ScheduleRemarks")));
            m.put("Status", text(ci(r, "Status")));
            m.put("EntryDate", iso(ci(r, "EntryDate")));
            m.put("EntryUser", text(ci(r, "EntryUser")));
            m.put("NoOfAttachments", asInt(ci(r, "NoOfAttachments")));
            out.add(m);
        }
        Map<String, Object> res = new LinkedHashMap<>();
        res.put("rows", out);
        return res;
    }

    // ================================================================= Pre Shipment Planning

    /** DepartmentDetailGridFill: the dates only when ticked; ProductionDays = LoadingDate - ProductionSchedule. */
    public List<Map<String, Object>> department(Map<String, Object> f) {
        UserAccount u = user("View");
        LocalDate from = asBool(f.get("fromChecked")) ? asDate(f.get("fromDate")) : null;
        LocalDate to = asBool(f.get("toChecked")) ? asDate(f.get("toDate")) : null;
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.departmentRows(u, sqlDate(from), sqlDate(to))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("ContractScheduleId", asInt(ci(r, "ContractScheduleId")));
            m.put("ScheduleCode", text(ci(r, "ScheduleCode")));
            m.put("ExImLcOrderId", asInt(ci(r, "ExImLcOrderId")));
            m.put("ContractNo", text(ci(r, "ContractNo")));
            m.put("SupplierCustomerId", asInt(ci(r, "SupplierCustomerId")));
            m.put("CustomerName", text(ci(r, "CustomerName")));
            m.put("DestinationPortId", asInt(ci(r, "DestinationPortId")));
            m.put("DestinationPort", text(ci(r, "DestinationPort")));
            m.put("FCL", asDouble(ci(r, "FCL")));
            m.put("ScheduleMTon", asDouble(ci(r, "ScheduleMTon")));
            String loading = iso(ci(r, "LoadingDate")), prod = iso(ci(r, "ProductionSchedule"));
            m.put("LoadingDate", loading);
            m.put("ContractScheduleDetailItemId", asInt(ci(r, "ContractScheduleDetailItemId")));
            m.put("ItemCode", text(ci(r, "ItemCode")));
            m.put("ItemName", text(ci(r, "ItemName")));
            m.put("ItemMTon", asDouble(ci(r, "ItemMTon")));
            LocalDate ld = asDate(loading), pd = asDate(prod);
            /* Conversion.ToDateTime(null) is MinValue on the desktop; the day count is kept as the desktop computes it */
            m.put("ProductionDays", ld == null ? 0 : (int) ChronoUnit.DAYS.between(pd == null ? LocalDate.of(1, 1, 1) : pd, ld));
            m.put("ProductionSchedule", prod);
            m.put("ProductionStart", iso(ci(r, "ProductionStart")));
            m.put("JobOrderId", asInt(ci(r, "JobOrderId")));
            m.put("InspectionSchedule", iso(ci(r, "InspectionSchedule")));
            m.put("InspectionStart", iso(ci(r, "InspectionStart")));
            m.put("PreShipmentInspectionId", asInt(ci(r, "PreShipmentInspectionId")));
            m.put("PackMaterialSchedule", iso(ci(r, "PackMaterialSchedule")));
            m.put("PackMaterialStart", iso(ci(r, "PackMaterialStart")));
            m.put("PmStartRefDocumentTypeId", asInt(ci(r, "PmStartRefDocumentTypeId")));
            m.put("PmStartRefId", asInt(ci(r, "PmStartRefId")));
            m.put("PlantId", asInt(ci(r, "PlantId")));
            m.put("PlantName", text(ci(r, "PlantName")));
            m.put("ScheduleLockId", asInt(ci(r, "ScheduleLockId")));
            m.put("ScheduleLockStatus", text(ci(r, "ScheduleLockStatus")));
            out.add(m);
        }
        return out;
    }

    public List<Map<String, Object>> plants() { return pairs(repo.productionPlants(user("View")), "Id", "Description"); }

    /**
     * InsertDepartmentDetail after DepartmentDetailFormvalidation: Contract No / Schedule No / Customer /
     * Port / Schedule Mton / Item / Item Mton / Production Days / Plant required, the Production
     * (and ticked Inspection / Packing Material) schedule dates not after the Loading Date, Schedule
     * Lock Status required; ExImLcContractScheduleDepartment.Save with Weight = Item Mton * 1000 and the
     * three *Start dates = now. "Detail Save SuccessFully" / "Detail Update SuccessFully".
     */
    public Map<String, Object> saveDepartment(Map<String, Object> b) {
        user("Save");
        int recId = asInt(b.get("recId"));
        if (asInt(b.get("contractId")) == 0) throw new IllegalArgumentException("Contract No is Required!");
        if (asInt(b.get("scheduleId")) == 0) throw new IllegalArgumentException("Schedule No is Required!");
        if (asInt(b.get("customerId")) == 0) throw new IllegalArgumentException("Customer is Required!");
        if (asInt(b.get("portId")) == 0) throw new IllegalArgumentException("Port is Required!");
        if (asDouble(b.get("scheduleMton")) == 0) throw new IllegalArgumentException("Schedule Mton Field is Required!");
        if (asInt(b.get("itemId")) == 0) throw new IllegalArgumentException("Item is Required!");
        if (asDouble(b.get("itemMton")) == 0) throw new IllegalArgumentException("Item Mton Field is Required!");
        if (asInt(b.get("productionDays")) == 0) throw new IllegalArgumentException("Production Days is Required!");
        if (asInt(b.get("plantId")) == 0) throw new IllegalArgumentException("Plant is Required!");
        LocalDate loading = asDate(b.get("loadingDate")), prod = asDate(b.get("productionDate"));
        LocalDate insp = asDate(b.get("inspectionDate")), pmd = asDate(b.get("pmDate"));
        if (dateNull(prod)) throw new IllegalArgumentException("'Production Schedule Date' Is not Valid !");
        if (loading != null && prod.isAfter(loading)) throw new IllegalArgumentException("'Production Schedule Date' can not be Greater than 'Loading Date' !");
        boolean inspChecked = asBool(b.get("inspectionChecked")), pmChecked = asBool(b.get("pmChecked"));
        if (inspChecked && dateNull(insp)) throw new IllegalArgumentException("'Inspection Schedule Date' Is not Valid !\nPlease Correct the Date or Un check the Field");
        if (inspChecked && loading != null && insp.isAfter(loading)) throw new IllegalArgumentException("'Inspection Schedule Date' can not be Greater than 'Loading Date' !");
        if (pmChecked && dateNull(pmd)) throw new IllegalArgumentException("'Packing Material Schedule Date' Is not Valid !\nPlease Correct the Date or Un check the Field");
        if (pmChecked && loading != null && pmd.isAfter(loading)) throw new IllegalArgumentException("'Packing Material Schedule Date' can not be Greater than 'Loading Date' !");
        int lock = asInt(b.get("scheduleLockId"));
        if (lock == 0) throw new IllegalArgumentException("Schedule Lock Status is Required!");

        Map<String, Object> m = new LinkedHashMap<>();
        m.put("InspectionSchedule", ts(b.get("inspectionDate")));
        m.put("InspectionStart", now());
        m.put("PackMaterialSchedule", ts(b.get("pmDate")));
        m.put("PackMaterialStart", now());
        m.put("ProductionSchedule", ts(b.get("productionDate")));
        m.put("ProductionStart", now());
        m.put("Weight", dec(b.get("itemMton")).multiply(BigDecimal.valueOf(1000)));
        m.put("ActionTypeId", recId == 0 ? 1 : 2);
        m.put("ContractScheduleDetailItemId", asInt(b.get("itemId")));
        m.put("ContractScheduleId", asInt(b.get("itemId")) > 0 ? asInt(b.get("scheduleId")) : 0);
        m.put("Id", recId);
        m.put("Plant", asInt(b.get("plantId")));
        m.put("ScheduleLockId", lock);
        repo.saveDepartment(m);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("message", recId == 0 ? "Detail Save SuccessFully" : "Detail Update SuccessFully");
        return out;
    }
}
