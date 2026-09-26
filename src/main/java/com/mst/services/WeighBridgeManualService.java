package com.mst.services;

import com.mst.models.Company;
import com.mst.models.UserAccount;
import com.mst.repositories.ICompanyRepository;
import com.mst.repositories.WeighBridgeRepository;
import com.mst.repositories.WeighBridgeRepository.HistoryFilter;
import com.mst.repositories.WeighBridgeRepository.WeighBridgeRecord;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Screen 410 "Weigh Bridge Manual" — {@code Architecture.WinApp.WeightBridge.WeightbridgeMannual},
 * DocTypeId 113, module 44.
 *
 * ---------------------------------------------------------------------------------------------
 * HOW IT DIFFERS FROM SCREEN 411 (frmWeightbridge) — ALL OF IT FROM THE DESKTOP SOURCE
 * ---------------------------------------------------------------------------------------------
 *  - Weights are TYPED (digits only). No indicator, no cameras, no "Enable Weights" gate.
 *  - DocTypeId 113, not 102. No FirstWeighBridgeId / SecondWeighBridgeId is ever set, so both go 0.
 *  - The same BLL WbTransactions.Save, so the same audit row and the same weighbridge voucher
 *    when charges and both accounts are configured (shared through WeighBridgeService.bllSave).
 *  - VehicleType is saved as cmbVehicleType.VALUE — the vehicle type's Id, as text — not its
 *    caption (Save :1789, Update :1874). ReadById puts it back with Value = ToInt(VehicleType).
 *  - WbCharges is Conversion.ToSingle on Save (a float, widened to double) and Conversion.ToInt
 *    on Update. ItemQty, FirstWeight, SecondWeight and NetWbWeight are Conversion.ToInt.
 *  - ReferenceDocTypeId is the reference combo's VALUE and ReferenceDocNoId the gate-pass combo's
 *    VALUE, for every type; no per-caption mapping as on 411.
 *  - Approved is the IsApproved column only (ReadById :2051) — not "has a second weight".
 *  - Packing types come from SpStaticColumnNames 'PackingTypeForWB'; items from
 *    Sp_Item_GetAllMethod 'ReadAllItemsWithPackingAndStore'; the gate-pass lists use status
 *    "Mannual" (lab) / "Manual" (party processing) and GetGatePassOutwardForMannualWeighBridge.
 *  - FormValidation is a different list with different text ("GatePassType No Field is
 *    Required", "Load Weight Field is Required", "Tare Weight Field is Required", ...), and
 *    requires First Weight and an Item on BOTH Save and Update.
 *
 * ---------------------------------------------------------------------------------------------
 * DESKTOP BEHAVIOURS REPRODUCED AND FLAGGED, NOT FIXED
 * ---------------------------------------------------------------------------------------------
 *  - With "Get Saved Vehicle Weight Against MoveOrder" on and Move Order chosen, the vehicle box
 *    is swapped for the saved-vehicle picker, but FormValidation still checks the hidden
 *    txtVehicleNo, which nothing fills — so Save always refuses "Vehicle Number Field is
 *    Required" unless a vehicle number was typed before the swap.
 *  - Update sends FirstWeighBridgeId = 0 and SecondWeighBridgeId = 0. The UPDATE does not set
 *    FirstWeighBridgeId; it does write SecondWeighBridgeId = 0. The "Second Weigh BridgeId Not
 *    Found" check in the procedure applies to 102 only.
 *  - Sp_WbTransactions_Insert/Update refuse DocTypeId 113 unless ERP feature 20 (manual
 *    weighbridge) is on for the company: "Operation denied: Manual weighbridge mode is disabled".
 *  - The update tolerance message omits the tolerance line the 411 screen shows.
 */
@Service
public class WeighBridgeManualService {

    private static final Logger LOG = LoggerFactory.getLogger(WeighBridgeManualService.class);

    /** btnsave_Click:1812 / btnUpdate_Click:1897 — wb.DocTypeId = 113. */
    public static final int DOC_TYPE_ID = 113;

    /** base.Name, which SetRightsValueInRightsObject looks the grants up by. */
    private static final String SCREEN_NAME = "WeightbridgeMannual";

    private static final String SQL_USER_RIGHTS =
            "EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?, @ScreenName=?, @RightName=?, "
          + "@CompanyId=?, @Activity=?";

    @Autowired private WeighBridgeRepository repo;
    @Autowired private WeighBridgeService weighBridge;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private ICompanyRepository companies;

    /** What the page posts — only controls the operator types or picks. */
    public static class Request {
        public Integer id = 0;                         // RECID
        public Integer ticketNo = 0;
        public String  biltyNo;
        public Integer supplierCustomerId = 0;         // CmbPartyName.Value
        public String  partyName;                      // CmbPartyName.Text
        public boolean partySelected;
        public String  wbCharges;                      // txtWbCharges TEXT (ToSingle on save, ToInt on update)
        public String  vehicleNo;                      // txtVehicleNo
        public String  savedVehicleNo;                 // cmbVehicleNo.Text
        public String  vehicleTypeValue;               // cmbVehicleType.Value
        public String  weighBridgeType;                // CmbWeighBridgeType.Text
        public boolean weighBridgeTypeSelected;
        public String  packingType;                    // cmbPackingType.Text
        public Integer packingTypeId = 0;
        public boolean packingTypeSelected;
        public String  itemQty;                        // txtItemqty TEXT (ToInt)
        public Integer itemId = 0;                     // CmbItem.Value
        public String  itemDescription;                // CmbItem.Text
        public boolean itemSelected;
        public Integer itemPartyId = 0;                // CmbItem.SelectedRow.Cells["PartyId"]
        public String  remarks;
        public String  firstWeight;                    // TEXT — FormValidation tests "" before ToDouble
        public String  secondWeight;
        public String  netWeight;
        public String  firstDateTime;
        public String  secondDateTime;
        public Integer refTypeId = 0;                  // cmbReferenceType.Value
        public String  refTypeText;
        public boolean refTypeSelected;
        public Integer refDocId = 0;                   // CmbGatePassNo.Value
        public boolean refDocSelected;
        public String  refDocGatepassType;             // SelectedRow.Cells["GatepassType"], "" when the list has no such column
        public Integer invoiceId = 0;
        public boolean invoiceSelected;
        public String  workingReportNo;
        public String  weightDiffComments;
        public boolean multiItem;
        public String  loadWeight;                     // txtLoadWeight
        public String  tearWeight;                     // txtTearWeight
        public String  supplierNetWeight;              // txtSupplierWbNetWeight
    }

    // ================================================================================== load

    /** frmWeightbridge_Load:349. */
    public Map<String, Object> lookups() {
        UserAccount u = currentUserContext.requireAccountingUser();
        int fy = currentUserContext.currentFinancialYearId();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("canSave", hasRight("Save"));
        out.put("canUpdate", hasRight("Update"));
        out.put("canPrint", hasRight("Print"));
        out.put("canViewAllRecord", hasRight("CanView AllRecord"));
        out.put("lstIsSavedVehicle", flag(u, "Get Saved Vehicle Weight Against MoveOrder"));
        out.put("multiWeightBridge", flag(u, "More Than 1 Weight By 1GpNoO Against"));
        out.put("weighBridgeForOutwardPartyAndItemWise", flag(u, "WeighBridgeForOutwardPartyAndItemWise"));
        out.put("labCompulsoryBeforFirstWeight", flag(u, "Lab Compulsory Befor First Weight"));
        out.put("weighbridgeTolerance", toDouble(cfg(u, "BillWeightAndStockWeightDifferenceTolerance")));
        out.put("vehicleTypes", repo.vehicleTypes());
        out.put("items", items(u));
        out.put("referenceTypes", repo.gatePassReferenceTypes(u));
        out.put("wbTypes", repo.gatePassTypes(u, "WeighBridge"));
        out.put("ticketNo", repo.nextTicketNo(u, fy));
        out.put("packingTypes", repo.staticColumns("PackingTypeForWB"));
        out.put("parties", parties(u));
        String compName = "";
        try {
            Company c = companies.findById(u.getCompanyId()).orElse(null);
            if (c != null && c.getCompName() != null) compName = c.getCompName();
        } catch (Exception e) {
            LOG.warn("Company name could not be read", e);
        }
        out.put("companyName", compName);
        return out;
    }

    /** btnRefresh_Click:2179 — the two flags again, packing types, parties and items. */
    public Map<String, Object> refreshLists() {
        UserAccount u = currentUserContext.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("lstIsSavedVehicle", flag(u, "Get Saved Vehicle Weight Against MoveOrder"));
        out.put("weighBridgeForOutwardPartyAndItemWise", flag(u, "WeighBridgeForOutwardPartyAndItemWise"));
        out.put("packingTypes", repo.staticColumns("PackingTypeForWB"));
        out.put("parties", parties(u));
        out.put("items", items(u));
        return out;
    }

    /** BtnRefreshHistory_Click:2829 — WbType() and GatePassTypeFill(). */
    public Map<String, Object> refreshHistoryLists() {
        UserAccount u = currentUserContext.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("wbTypes", repo.gatePassTypes(u, "WeighBridge"));
        out.put("referenceTypes", repo.gatePassReferenceTypes(u));
        return out;
    }

    public int nextTicketNo() {
        UserAccount u = currentUserContext.requireAccountingUser();
        return repo.nextTicketNo(u, currentUserContext.currentFinancialYearId());
    }

    /** PartyNameBind():440 — the WbType "PartyName" rows only. */
    public List<Map<String, Object>> parties(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.wbPartiesAndItems(u)) {
            if (!"PartyName".equals(str(WeighBridgeRepository.ci(r, "WbType")))) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", WeighBridgeRepository.ci(r, "Id"));
            o.put("PartyName", WeighBridgeRepository.ci(r, "PartyName"));
            out.add(o);
        }
        return out;
    }

    /** getAllitems():943 — Id and ItemName only are copied into dtitem. */
    private List<Map<String, Object>> items(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.itemsWithPackingAndStore(u)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", WeighBridgeRepository.ci(r, "Id"));
            o.put("ItemName", WeighBridgeRepository.ci(r, "ItemName"));
            out.add(o);
        }
        return out;
    }

    // ===================================================================== gate-pass pickers

    /**
     * GatePassBending():1183 — mode: outward | steel | lab | engr | general | party. The Move
     * Order / General / 218 branch binds one row made of the ticket number and needs no call.
     */
    public List<Map<String, Object>> refDocs(String mode, int documentTypeId, int recId) {
        UserAccount u = currentUserContext.requireAccountingUser();
        int fy = currentUserContext.currentFinancialYearId();
        switch (mode == null ? "" : mode) {
            case "outward":
                return pick(repo.gatePassesOutwardManual(u, fy, 91, flag(u, "More Than 1 Weight By 1GpNoO Against"), recId),
                            true);
            case "steel":
                return pick(repo.gatePassesOutwardManual(u, fy, 1507, false, 0), false);
            case "lab":
                return repo.gatePassesFromLab(u, fy, "Mannual");
            case "engr":
                return repo.gatePassesInwardEngr(u, fy);
            case "general":
                if (documentTypeId != 52 && documentTypeId != 92) throw new IllegalArgumentException("Unknown general gate pass type");
                return pick(repo.gatePassesGeneral(u, fy, documentTypeId), false);
            case "party":
                return repo.gatePassesPartyProcessingManual(u, documentTypeId);
            default:
                throw new IllegalArgumentException("Unknown gate pass list");
        }
    }

    /** The desktop copies Id, GpSrNo [, VehicleNo, GatepassType] into a fresh DataTable. */
    private static List<Map<String, Object>> pick(List<Map<String, Object>> rows, boolean withVehicleAndType) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", WeighBridgeRepository.ci(r, "Id"));
            o.put("GpSrNo", WeighBridgeRepository.ci(r, "GpSrNo"));
            if (withVehicleAndType) {
                o.put("VehicleNo", WeighBridgeRepository.ci(r, "VehicleNo"));
                o.put("GatepassType", WeighBridgeRepository.ci(r, "GatepassType"));
            }
            out.add(o);
        }
        return out;
    }

    /** BindingAgainstGatePass():1305 — kind: outward (91) | steel (1507) | inward (51/1601) |
     *  general (52/92, by GP number text) | party (54/55, by gate-pass id). */
    public Map<String, Object> gatePass(String kind, int documentTypeId, int id, int gpSrNo) {
        UserAccount u = currentUserContext.requireAccountingUser();
        int fy = currentUserContext.currentFinancialYearId();
        List<Map<String, Object>> rows;
        switch (kind == null ? "" : kind) {
            case "outward": rows = repo.gatePassOutwardById(u, fy, 91, id); break;
            case "steel":   rows = repo.gatePassOutwardById(u, fy, 1507, id); break;
            case "inward":
                if (documentTypeId != 51 && documentTypeId != 1601) throw new IllegalArgumentException("Unknown inward gate pass type");
                rows = repo.gatePassInwardById(u, fy, documentTypeId, id);
                break;
            case "general":
                if (documentTypeId != 52 && documentTypeId != 92) throw new IllegalArgumentException("Unknown general gate pass type");
                rows = repo.gatePassGeneralByNoNoBranch(u, fy, documentTypeId, gpSrNo);
                break;
            case "party":   rows = repo.gatePassPartyProcessingById(u, fy, documentTypeId, id); break;
            default: throw new IllegalArgumentException("Unknown gate pass kind");
        }
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> row = rows.isEmpty() ? null : rows.get(0);
        out.put("row", row);
        out.put("isGatePassEntryUser", row != null && u.getId() != null
                && u.getId() == WeighBridgeRepository.intOf(WeighBridgeRepository.ci(row, "EntryUser")));
        return out;
    }

    /** GetSavedVehicles():875. */
    public List<Map<String, Object>> savedVehicles() {
        UserAccount u = currentUserContext.requireAccountingUser();
        return repo.savedVehicleWeights(u);
    }

    // ================================================================= pending grids / history

    /** GatepassPendingForWeighBridge():2413 — no filters on this form. */
    public List<Map<String, Object>> pendingFirst() {
        UserAccount u = currentUserContext.requireAccountingUser();
        return repo.pendingForFirstWeight(u, null, null, 0, 0);
    }

    /** bindGridPending():2221 — no FirstWeighBridgeId, no filters. */
    public List<Map<String, Object>> pendingSecond() {
        UserAccount u = currentUserContext.requireAccountingUser();
        return repo.pendingForSecondWeight(u, currentUserContext.currentFinancialYearId(), 0, null, null, 0, 0);
    }

    /** bindGrid():2595 — FormHistory without the scale id (ActionId stays 0). */
    public List<Map<String, Object>> history(String dateMode, String fromDate, String toDate,
                                             int ticketNoFrom, int ticketNoTo, int gpNoFrom, int gpNoTo,
                                             String vehicleNo, int refDocumentTypeId, int wbTypeId, String wbTypeText) {
        UserAccount u = currentUserContext.requireAccountingUser();
        HistoryFilter f = new HistoryFilter();
        f.canViewAllRecord = hasRight("CanView AllRecord");
        f.entryUser = u.getId() == null ? 0 : u.getId();
        f.ticketNoFrom = ticketNoFrom;
        f.ticketNoTo = ticketNoTo;
        Timestamp from = pickerValue(fromDate), to = pickerValue(toDate);
        switch (dateMode == null ? "doc" : dateMode) {
            case "entry":    f.entryFrom = from;    f.entryTo = to;    break;
            case "modify":   f.modifyFrom = from;   f.modifyTo = to;   break;
            case "approved": f.approvedFrom = from; f.approvedTo = to; break;
            default:         f.dateFrom = from;     f.dateTo = to;     break;
        }
        f.gpNoFrom = gpNoFrom;
        f.gpNoTo = gpNoTo;
        f.vehicleNo = vehicleNo == null ? "" : vehicleNo.trim();
        f.refDocumentTypeId = refDocumentTypeId;
        f.weighBridgeType = wbTypeId > 0 ? (wbTypeText == null ? "" : wbTypeText) : "";
        return repo.history(u, currentUserContext.currentFinancialYearId(), f);
    }

    /** ReadById(ID):1947 — the row, company-checked; approved = IsApproved only (:2051). */
    public Map<String, Object> load(int id) {
        UserAccount u = currentUserContext.requireAccountingUser();
        Map<String, Object> row = repo.readById(id);
        if (row == null) return null;
        Object company = WeighBridgeRepository.ci(row, "CompanyId");
        if (company != null && WeighBridgeRepository.intOf(company) != u.getCompanyId()) return null;
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("row", row);
        out.put("approved", toBool(WeighBridgeRepository.ci(row, "IsApproved")));
        return out;
    }

    /** CommonServices.WbTransactionSlip280(PrintId):8861 — refuses 0, no DocumentTypeId. */
    public List<Map<String, Object>> slip(int id) {
        if (id == 0) throw new IllegalArgumentException("No Record Found For Display");
        UserAccount u = currentUserContext.requireAccountingUser();
        return repo.slip280(u, id, 0);
    }

    // ================================================================================== save

    /** btnsave_Click:1768. */
    public Map<String, Object> save(Request r) {
        UserAccount u = currentUserContext.requireAccountingUser();
        if (!hasRight("Save")) throw new IllegalStateException("You do not have the Save right on Weigh Bridge Manual.");
        if (r == null) throw new IllegalArgumentException("Nothing to save.");
        r.id = 0;
        boolean lstIsSavedVehicle = flag(u, "Get Saved Vehicle Weight Against MoveOrder");
        formValidation(u, r, false, flag(u, "WeighBridgeForOutwardPartyAndItemWise"));

        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        WeighBridgeRecord wb = new WeighBridgeRecord();
        wb.TicketNo = toInt(str(r.ticketNo));
        wb.BiltyNo = s(r.biltyNo);
        wb.SupplierCustomerId = nz(r.supplierCustomerId);
        wb.WbCharges = (double) (float) toDouble(r.wbCharges);         // Conversion.ToSingle
        wb.VehicleNo = (nz(r.refTypeId) == 74 && lstIsSavedVehicle) ? s(r.savedVehicleNo) : s(r.vehicleNo);
        wb.VehicleType = s(r.vehicleTypeValue);
        wb.WeighBridgeType = s(r.weighBridgeType);
        wb.PackingType = s(r.packingType);
        wb.ItemQty = (double) toInt(r.itemQty);
        wb.ItemId = nz(r.itemId);
        wb.ItemDescription = s(r.itemDescription);
        wb.PartyName = s(r.partyName);
        wb.WbRemarks = s(r.remarks);
        wb.FirstWeight = (double) toInt(r.firstWeight);
        wb.LoadWeight = toDouble(r.loadWeight);
        wb.FirstDateTime = clientTime(r.firstDateTime, now);
        wb.SecondDateTime = clientTime(r.secondDateTime, now);
        wb.ReferenceDocTypeId = nz(r.refTypeId);
        wb.ReferenceDocNoId = nz(r.refDocId);
        wb.InvoiceId = nz(r.invoiceId);
        wb.WorkingReportNo = toInt(s(r.workingReportNo).trim());
        stamp(u, wb, now);
        wb.MultiItem = r.multiItem;

        int saved = weighBridge.bllSave(u, wb);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", saved > 0);
        out.put("id", saved);
        out.put("message", saved > 0 ? "Record Save Successfully" + wb.TicketNo : "");
        return out;
    }

    /** btnUpdate_Click:1847. */
    public Map<String, Object> update(Request r) {
        UserAccount u = currentUserContext.requireAccountingUser();
        if (!hasRight("Update")) throw new IllegalStateException("You do not have the Update right on Weigh Bridge Manual.");
        if (r == null || r.id == null || r.id <= 0) throw new IllegalArgumentException("Record Not Found");
        Map<String, Object> existing = load(r.id);
        if (existing == null) throw new IllegalArgumentException("Record Not Found");
        if (Boolean.TRUE.equals(existing.get("approved"))) {
            throw new IllegalArgumentException("Record Not Update because record has approve");
        }
        formValidation(u, r, true, flag(u, "WeighBridgeForOutwardPartyAndItemWise"));
        if ("GatePass Inward".equals(r.refTypeText)) {
            double tolerance = toDouble(cfg(u, "BillWeightAndStockWeightDifferenceTolerance"));
            double wbNet = toDouble(r.netWeight), supplierNet = toDouble(r.supplierNetWeight);
            if (Math.abs(wbNet - supplierNet) > tolerance && s(r.weightDiffComments).trim().isEmpty()) {
                throw new IllegalArgumentException(
                        "Supplier Net Weight in Weigh Bridge does not match the Weighbridge Net Weight.\n"
                      + "Supplier Net Weight: " + WeighBridgeService.cs(supplierNet) + "\n"
                      + "Weighbridge Net Weight: " + WeighBridgeService.cs(wbNet) + "\n\n"
                      + "Difference Weight Remarks are required");
            }
        }

        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        WeighBridgeRecord wb = new WeighBridgeRecord();
        wb.Id = r.id;
        wb.TicketNo = toInt(str(r.ticketNo));
        wb.BiltyNo = s(r.biltyNo).trim();
        wb.PartyName = s(r.partyName).trim();
        wb.SupplierCustomerId = nz(r.supplierCustomerId);
        wb.WbCharges = (double) toInt(s(r.wbCharges).trim());          // Conversion.ToInt on update
        wb.VehicleNo = s(r.vehicleNo).trim();
        wb.VehicleType = s(r.vehicleTypeValue);
        wb.WeighBridgeType = s(r.weighBridgeType);
        wb.ItemQty = (double) toInt(s(r.itemQty).trim());
        wb.ItemId = nz(r.itemId);
        wb.ItemDescription = s(r.itemDescription).trim();
        wb.PackingType = s(r.packingType);
        wb.WbRemarks = s(r.remarks).trim();
        wb.SecondWeight = (double) toInt(s(r.secondWeight).trim());
        wb.NetWbWeight = (double) toInt(s(r.netWeight).trim());
        wb.SecondDateTime = clientTime(r.secondDateTime, now);
        wb.FirstDateTime = clientTime(r.firstDateTime, now);
        wb.FirstWeight = (double) toInt(s(r.firstWeight).trim());
        wb.LoadWeight = toDouble(s(r.loadWeight).trim());
        wb.TearWeight = toDouble(s(r.tearWeight).trim());
        wb.SupplierWbNetWeight = toDouble(s(r.supplierNetWeight).trim());
        wb.IsApproved = true;
        wb.ApprovedDateTime = now;
        wb.ApprovedUserId = u.getId();
        wb.ReferenceDocTypeId = nz(r.refTypeId);
        wb.ReferenceDocNoId = nz(r.refDocId);
        wb.InvoiceId = nz(r.invoiceId);
        wb.WorkingReportNo = toInt(s(r.workingReportNo).trim());
        wb.WeightDiffComments = s(r.weightDiffComments).trim();
        stamp(u, wb, now);
        wb.MultiItem = r.multiItem;

        int saved = weighBridge.bllSave(u, wb);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", saved > 0);
        out.put("id", saved);
        out.put("message", saved > 0 ? "Record Update Successfully" + wb.TicketNo : "");
        return out;
    }

    private void stamp(UserAccount u, WeighBridgeRecord wb, Timestamp now) {
        wb.DocTypeId = DOC_TYPE_ID;
        wb.DocDate = now;
        wb.EntryDate = now;
        wb.ModifyDate = now;
        wb.EntryUser = u.getId();
        wb.ModifyUser = u.getId();
        wb.OrganizationId = u.getOrganizationId();
        wb.CompanyId = u.getCompanyId();
        wb.BranchId = u.getBranchesId() == null ? 0 : u.getBranchesId();
        wb.FinancialYearId = currentUserContext.currentFinancialYearId();
    }

    /** FormValidation():1612 — its order and its text. {@code update} = btnUpdate visible. */
    private void formValidation(UserAccount u, Request r, boolean update, boolean partyItemWise) {
        int refType = nz(r.refTypeId);
        String gpType = s(r.refDocGatepassType);
        if (!r.refTypeSelected) fail("GatePassType No Field is Required");
        if (!r.refDocSelected) fail("GatePass No Field is Required");
        if (!r.weighBridgeTypeSelected) fail("WeighBridge Type Field is Required");
        if (refType == 91 && r.refDocSelected && nz(r.refDocId) > 0 && "Export".equals(gpType)
                && (!r.invoiceSelected || nz(r.invoiceId) == 0)) fail("Invoice number Field is Required");
        if ((refType == 74 || refType == 75) && (!r.packingTypeSelected || nz(r.packingTypeId) == 0)) {
            fail("PackingType Field is Required");
        }
        if (refType == 51 && (s(r.loadWeight).isEmpty() || toDouble(r.loadWeight) == 0d)) fail("Load Weight Field is Required");
        String first = s(r.firstWeight).trim();
        if (first.isEmpty() || toDouble(first) == 0d) fail("First Weight Field is Required");
        if (update) {
            String second = s(r.secondWeight).trim();
            if (second.isEmpty() || toDouble(second) == 0d) fail("Second Weight Field is Required");
            if (refType == 51) {
                String tare = s(r.tearWeight).trim();
                if (tare.isEmpty() || toDouble(tare) == 0d) fail("Tare Weight Field is Required");
                String net = s(r.supplierNetWeight).trim();
                if (net.isEmpty() || toDouble(net) == 0d) fail("Supplier Net Weight Field is Required");
            }
            String wrn = s(r.workingReportNo).trim();
            boolean wrnMissing = wrn.isEmpty() || "0".equals(wrn);
            if ((refType == 52 || refType == 74 || refType == 92) && wrnMissing) fail("WorkingReportNo Field is Required");
            if (refType == 91 && wrnMissing
                    && "StockTransfer".equals(repo.deliveryOrderType(u, nz(r.refDocId)))) {
                fail("WorkingReportNo Field is Required");
            }
        }
        String ticket = str(r.ticketNo).trim();
        if (ticket.isEmpty() || "0".equals(ticket)) fail("Ticket No Field is Required");
        if (s(r.vehicleNo).trim().isEmpty()) fail("Vehicle Number Field is Required");
        boolean outwardPartyRule = partyItemWise && "GatePass OutWard".equals(s(r.refTypeText))
                && !"Export".equals(gpType) && !"General".equals(gpType);
        if (outwardPartyRule && (!r.partySelected || nz(r.supplierCustomerId) == 0)) fail("PartyName Field is Required");
        if (!r.itemSelected || nz(r.itemId) == 0) fail("ItemName Field is Required");
        if (outwardPartyRule && nz(r.supplierCustomerId) != nz(r.itemPartyId)) fail("PartyName Field is Required");
    }

    private static void fail(String m) { throw new IllegalArgumentException(m); }

    // ================================================================================= rights

    private boolean hasRight(String rightName) {
        String role = currentUserContext.currentRoleName();
        if ("Admin".equals(role)) return true;
        try {
            List<Map<String, Object>> rights = jdbcTemplate.queryForList(
                    SQL_USER_RIGHTS, currentUserContext.currentUserId(), SCREEN_NAME,
                    role == null ? "" : role, currentUserContext.currentCompanyId(), "GetByUserId");
            for (Map<String, Object> r : rights) {
                Object name = WeighBridgeRepository.ci(r, "RightName");
                if (name != null && rightName.equals(String.valueOf(name).trim())) {
                    return toBool(WeighBridgeRepository.ci(r, "Value"));
                }
            }
        } catch (Exception e) {
            LOG.warn("Could not read the '{}' right for {}; denying", rightName, SCREEN_NAME, e);
        }
        return false;
    }

    // ================================================================================ helpers

    private String cfg(UserAccount u, String name) {
        try { return repo.config(u, name); }
        catch (Exception e) { LOG.warn("Configuration '{}' could not be read; treating as unset", name, e); return ""; }
    }
    private boolean flag(UserAccount u, String name) { return toBool(cfg(u, name)); }

    private static Timestamp pickerValue(String day) {
        if (day == null || day.trim().isEmpty()) return null;
        try {
            LocalDate d = LocalDate.parse(day.trim().substring(0, 10));
            return Timestamp.valueOf(LocalDateTime.of(d, LocalTime.now().withNano(0)));
        } catch (Exception e) {
            throw new IllegalArgumentException("Invalid date: " + day);
        }
    }

    private static Timestamp clientTime(String iso, Timestamp fallback) {
        if (iso == null || iso.trim().isEmpty()) return fallback;
        String t = iso.trim().replace('T', ' ');
        if (t.length() == 16) t += ":00";
        if (t.length() > 19) t = t.substring(0, 19);
        try { return Timestamp.valueOf(t); } catch (Exception e) { return fallback; }
    }

    private static String s(String v) { return v == null ? "" : v; }
    private static String str(Object v) { return v == null ? "" : String.valueOf(v); }
    private static int nz(Integer v) { return v == null ? 0 : v; }

    /** Conversion.ToInt on a text box: whole numbers only; anything else is 0. */
    private static int toInt(String v) {
        if (v == null) return 0;
        String t = v.trim();
        if (t.isEmpty()) return 0;
        try { return Integer.parseInt(t); }
        catch (NumberFormatException e) {
            try { return (int) Math.round(Double.parseDouble(t)); } catch (NumberFormatException e2) { return 0; }
        }
    }

    private static double toDouble(String v) {
        if (v == null) return 0d;
        try { return Double.parseDouble(v.trim().replace(",", "")); } catch (NumberFormatException e) { return 0d; }
    }

    private static boolean toBool(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        if (v == null) return false;
        String t = String.valueOf(v).trim();
        return "1".equals(t) || "true".equalsIgnoreCase(t) || "yes".equalsIgnoreCase(t);
    }
}
