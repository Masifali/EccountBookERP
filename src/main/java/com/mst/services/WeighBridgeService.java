package com.mst.services;

import com.mst.models.Company;
import com.mst.models.UserAccount;
import com.mst.models.dto.ContraVoucherDto;
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

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Screen 411 "Weigh Bridge" — {@code Architecture.WinApp.WeightBridge.frmWeightbridge},
 * DocTypeId 102, ScreenDefinition 411 in module 44.
 *
 * ---------------------------------------------------------------------------------------------
 * WHAT COMES FROM WHERE
 * ---------------------------------------------------------------------------------------------
 * Every picker is the desktop's own DDL source (see WeighBridgeRepository, one BLL method per
 * call). DocTypeId (102), the dates the form stamps, the user, organisation, company, branch,
 * financial year and the weighbridge scale id are all set HERE from the signed-in session and
 * the scales list, never taken from the request, exactly as btnsave_Click / btnUpdate_Click set
 * them from UserAccount and clsGlobalVariables rather than from any control.
 *
 * ---------------------------------------------------------------------------------------------
 * VALIDATION IS THE DESKTOP'S — FormValidation():1531, IN ITS ORDER, WITH ITS TEXT
 * ---------------------------------------------------------------------------------------------
 * The page runs the same checks first so it can focus the offending control, but the page is
 * not trusted: every refusal below runs again here, so a replayed or hand-built request meets
 * the same rules. The two-space typos ("First Weight  Field is Required") are the desktop's and
 * are kept.
 *
 * ---------------------------------------------------------------------------------------------
 * DESKTOP BEHAVIOURS REPRODUCED AS THEY ARE, NOT "FIXED" (see the project doc for detail)
 * ---------------------------------------------------------------------------------------------
 *  - Update sends FirstWeighBridgeId = 0: btnUpdate_Click never sets it (it sets only
 *    SecondWeighBridgeId). Harmless: the UPDATE in Sp_WbTransactions_Update (procdure.sql) does
 *    not set FirstWeighBridgeId, TicketNo, ReferenceDocNoId or EntryUser.
 *  - Update sends ReferenceDocNoId = RECID (the ticket's own row id) for Move Order and General,
 *    where Save sent the ticket number. That is :3044 / :3050 as written. Also harmless: the
 *    UPDATE does not write ReferenceDocNoId, and its 74/92 check keys on WorkingReportNo.
 *  - On update the weighbridge voucher lines are inserted again with no delete of the previous
 *    ones (DAL 0083 has no delete step).
 *  - "Get Saved Vehicle Weight Against MoveOrder" on + Move Order: validation reads cmbVehicleNo,
 *    which GetSavedVehicles() would populate but nothing ever calls it, so the check always
 *    refuses. Reproduced; flagged.
 *  - The indicator (F5) and the Hikvision cameras are driven by the server, which runs on the
 *    weighbridge PC: see WeighBridgeDeviceService. The Browse/Preview picture buttons are not
 *    ported (they only copied a file into the attachment folder without recording it).
 */
@Service
public class WeighBridgeService {

    private static final Logger LOG = LoggerFactory.getLogger(WeighBridgeService.class);

    /** btnsave_Click:2428 / btnUpdate_Click:3076 — wb.DocTypeId = 102. */
    public static final int DOC_TYPE_ID = 102;

    private static final String SCREEN_NAME = "frmWeightbridge";

    private static final String SQL_USER_RIGHTS =
            "EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?, @ScreenName=?, @RightName=?, "
          + "@CompanyId=?, @Activity=?";

    /** The configuration rows GetConfigurationsFromGlobal():727 and
     *  GetConfigurationsFromGlobalAndBindValuesInColumns():753 read. */
    private static final String CFG_CAMERA_PATH           = "CameraPictureBox";
    private static final String CFG_LAB_MARKET            = "LabCompulsoryForWeighBridgeAgainstMarketPurchase";
    private static final String CFG_LAB_GATE              = "LabCompulsoryForWeighBridgeAgainstGatePurchase";
    private static final String CFG_LAB_REGULAR           = "LabCompulsoryForWeighBridge";
    private static final String CFG_LAB_BEFORE_FIRST      = "Lab Compulsory Befor First Weight";
    private static final String CFG_SAVED_VEHICLE         = "Get Saved Vehicle Weight Against MoveOrder";
    private static final String CFG_MULTI_WEIGHT          = "More Than 1 Weight By 1GpNoO Against";
    private static final String CFG_OUTWARD_PARTY_ITEM    = "WeighBridgeForOutwardPartyAndItemWise";
    private static final String CFG_TOLERANCE             = "BillWeightAndStockWeightDifferenceTolerance";
    private static final String CFG_PACKING_TYPE          = "Paking Type";
    private static final String CFG_CASH_ACCOUNT          = "CashWithWeighbridgeAccount";
    private static final String CFG_OTHER_INCOME_ACCOUNT  = "OtherIncomeWithWeighbridgeAccount";

    @Autowired private WeighBridgeRepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private ICompanyRepository companies;
    @Autowired private WeighBridgeDeviceService devices;

    // ===================================================================== the request shape

    /**
     * What the page posts. Only what an operator can type or pick is here. The "…Selected"
     * booleans are the desktop's {@code ActiveRow != null} tests on the Infragistics combos —
     * facts about the page, which FormValidation reads.
     */
    public static class Request {
        public Integer id = 0;                       // RECID (update only)
        public Integer ticketNo = 0;                 // txtTickectNo
        public String  biltyNo;                      // txtBilityNo
        public Integer supplierCustomerId = 0;       // CmbPartyName.Value
        public String  partyName;                    // CmbPartyName.Text
        public boolean partySelected;
        public Integer invoiceId = 0;                // CmbInvoiceNo.Value
        public boolean invoiceSelected;
        public Double  wbCharges = 0d;               // txtWbCharges
        public String  vehicleNo;                    // txtVehicleNo
        public String  savedVehicleNo;               // cmbVehicleNo.Text (never populated — see note)
        public String  vehicleType;                  // cmbVehicleType.Text
        public String  weighBridgeType;              // CmbWeighBridgeType.Text
        public boolean weighBridgeTypeSelected;
        public String  packingType;                  // cmbPackingType.Text
        public Integer packingTypeId = 0;            // cmbPackingType.Value
        public boolean packingTypeSelected;
        public Double  itemQty = 0d;                 // txtItemqty
        public Integer itemId = 0;                   // CmbItem.Value
        public String  itemDescription;              // CmbItem.Text
        public boolean itemSelected;
        public Integer itemPartyId = 0;              // CmbItem.SelectedRow.Cells["PartyId"]
        public String  remarks;                      // txtRemarks
        public Double  firstWeight = 0d;             // txtFirstWeight
        public Double  secondWeight = 0d;            // txtsecondWeight
        public Double  netWeight = 0d;               // txtNetWeight
        public String  firstDateTime;                // txtFirstDate.Value  (ISO local)
        public String  secondDateTime;               // txtSecondDate.Value (ISO local)
        public Double  packingUnitWeight = 0d;       // txtEbUnit
        public Double  packingTotalWeight = 0d;      // txtEbTotalWeight
        public Double  weightAfterPackingWeight = 0d;// txtWeightAfterPakingWeight
        public Integer refTypeId = 0;                // cmbReferenceType.Value
        public String  refTypeText;                  // cmbReferenceType.Text
        public boolean refTypeSelected;
        public Integer refDocId = 0;                 // cmbRefDocNo.Value
        public boolean refDocSelected;
        public String  refDocGatepassType;           // cmbRefDocNo.SelectedRow.Cells["GatepassType"]
        public String  moveOrderRefNo;               // txtrefdocNumberofMoveOrder
        public String  containerNo;                  // txtContainerNo
        public String  workingReportNo;              // txtWorkingReportNo (text: "" and "0" differ from 0 only in wording)
        public String  weightDiffComments;           // txtWeightDiffComments
        /* The picture names the server's cameras wrote on F5 (UniqFileName..UniqFileName6). */
        public String  firstWtPicReading, firstWtUpperPic, firstWtFrontPic;
        public String  secondPicReading, secondWtUpperPic, secondWtFrontPic;
        public boolean multiItem;                    // ChkMultiItem
        public Double  supplierFirstWeight = 0d;     // txtSupplierFirstWeight
        public Double  supplierSecondWeight = 0d;    // txtSupplierSecondWeight
        public Double  supplierNetWeight = 0d;       // txtSupplierNetWeight
    }

    // ================================================================================== load

    /**
     * InitializeComponentMethod():661 — rights, ticket no, the reference types, the weighbridge
     * party/item lookups, the WeighBridge gate-pass types, vehicle types; then
     * GetConfigurationsFromGlobal, the global items and packing types.
     */
    public Map<String, Object> lookups() {
        UserAccount u = currentUserContext.requireAccountingUser();
        int fy = currentUserContext.currentFinancialYearId();
        Map<String, Object> out = new LinkedHashMap<>();

        out.put("canSave",          hasRight("Save"));
        out.put("canUpdate",        hasRight("Update"));
        out.put("canPrint",         hasRight("Print"));
        out.put("canViewAllRecord", hasRight("CanView AllRecord"));

        out.put("ticketNo",        repo.nextTicketNo(u, fy));
        out.put("referenceTypes",  repo.gatePassReferenceTypes(u));
        out.put("wbTypes",         repo.gatePassTypes(u, "WeighBridge"));
        out.put("vehicleTypes",    repo.vehicleTypes());
        out.put("packingTypes",    repo.packingTypes());
        out.putAll(partiesAndItems(u));
        out.put("items",           repo.allItems(u));
        out.put("scales",          scalesPublic(repo.scalesList(u)));
        out.putAll(configuration(u));

        String compName = "";
        try {
            Company c = companies.findById(u.getCompanyId()).orElse(null);
            if (c != null && c.getCompName() != null) compName = c.getCompName();
        } catch (Exception e) {
            LOG.warn("Company name could not be read", e);
        }
        out.put("companyName", compName);
        out.put("userId", u.getId());
        return out;
    }

    /** btnRefresh_Click:4629 — vehicle types, packing types, the party/item-wise flag, and the
     *  weighbridge parties plus global items. */
    public Map<String, Object> refreshLists() {
        UserAccount u = currentUserContext.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("vehicleTypes", repo.vehicleTypes());
        out.put("packingTypes", repo.packingTypes());
        out.put("weighBridgeForOutwardPartyAndItemWise", flag(u, CFG_OUTWARD_PARTY_ITEM));
        out.putAll(partiesAndItems(u));
        out.put("items", repo.allItems(u));
        return out;
    }

    /** BtnRefreshHistory_Click:3706 — binds CommonServices.GatePassReferenceTypes() into BOTH
     *  the WB-type and the gate-pass-type combos. The WB-type combo then holds reference types
     *  rather than WeighBridge gate-pass types until the page is reloaded. Reproduced. */
    public Map<String, Object> refreshHistoryLists() {
        UserAccount u = currentUserContext.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> ref = repo.gatePassReferenceTypes(u);
        out.put("referenceTypes", ref);
        out.put("wbTypesFromReferenceTypes", ref);
        return out;
    }

    public int nextTicketNo() {
        UserAccount u = currentUserContext.requireAccountingUser();
        return repo.nextTicketNo(u, currentUserContext.currentFinancialYearId());
    }

    /** WeightBridge_Helper.WbPartyandItemsDtFillDbCall — WbType "PartyName" rows are parties,
     *  every other row is an item lookup. */
    private Map<String, Object> partiesAndItems(UserAccount u) {
        List<Map<String, Object>> parties = new ArrayList<>();
        List<Map<String, Object>> itemLookups = new ArrayList<>();
        for (Map<String, Object> r : repo.wbPartiesAndItems(u)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", WeighBridgeRepository.ci(r, "Id"));
            if ("PartyName".equals(str(WeighBridgeRepository.ci(r, "WbType")))) {
                o.put("PartyName", WeighBridgeRepository.ci(r, "PartyName"));
                parties.add(o);
            } else {
                o.put("ItemName", WeighBridgeRepository.ci(r, "ItemName"));
                itemLookups.add(o);
            }
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("wbParties", parties);
        out.put("wbItemLookups", itemLookups);
        return out;
    }

    /** The scales list minus nothing the page may not see — it needs the serial settings to read
     *  the indicator itself (see the page note on Web Serial). */
    private static List<Map<String, Object>> scalesPublic(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (String k : Arrays.asList("Id", "WbsNameDescription", "ModelName", "PortName", "BaudRate",
                                          "DataBits", "StopBits", "Parity", "ReadTimeOutWb", "WriteTimeOutWb")) {
                o.put(k, WeighBridgeRepository.ci(r, k));
            }
            out.add(o);
        }
        return out;
    }

    private Map<String, Object> configuration(UserAccount u) {
        Map<String, Object> out = new LinkedHashMap<>();
        String cameraPath = cfg(u, CFG_CAMERA_PATH);
        boolean labMarket  = flag(u, CFG_LAB_MARKET);
        boolean labGate    = flag(u, CFG_LAB_GATE);
        boolean labRegular = flag(u, CFG_LAB_REGULAR);
        out.put("cameraPathConfigured", !cameraPath.trim().isEmpty());
        out.put("isLabCompulsoryOnWBForMarketPur", labMarket);
        out.put("isLabCompulsoryOnWBForGatePur", labGate);
        out.put("isLabCompulsoryOnWBForRegularPur", labRegular);
        /* :736 GroupInwardLabInfo.Visible */
        out.put("showInwardLabInfo", labRegular || labGate || labMarket);
        out.put("labCompulsoryBeforFirstWeight", flag(u, CFG_LAB_BEFORE_FIRST));
        out.put("lstIsSavedVehicle", flag(u, CFG_SAVED_VEHICLE));
        out.put("multiWeightBridge", flag(u, CFG_MULTI_WEIGHT));
        out.put("weighBridgeForOutwardPartyAndItemWise", flag(u, CFG_OUTWARD_PARTY_ITEM));
        out.put("weighbridgeTolerance", toDouble(cfg(u, CFG_TOLERANCE)));
        out.put("defaultPackingTypeId", WeighBridgeRepository.intOf(cfg(u, CFG_PACKING_TYPE)));
        return out;
    }

    // ============================================================= gate pass pickers (bending)

    /**
     * GatePassBending():3816 — which list cmbRefDocNo is bound to. The decision is made on the
     * reference type's TEXT first, exactly in the desktop's order, so the 1601 "Engr" branch is
     * reached only when the text is not "GatePass Inward" (as written, it never is, because the
     * first branch already takes that text).
     */
    public Map<String, Object> refDocs(String mode, int documentTypeId) {
        UserAccount u = currentUserContext.requireAccountingUser();
        int fy = currentUserContext.currentFinancialYearId();
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> rows;
        switch (mode == null ? "" : mode) {
            case "lab": {
                /* WeightBridge_Helper.GetGpNoFromLab — only Id and GpNo are copied across. */
                rows = new ArrayList<>();
                for (Map<String, Object> r : repo.gatePassesFromLab(u, fy, "Auto")) {
                    Map<String, Object> o = new LinkedHashMap<>();
                    o.put("Id", WeighBridgeRepository.intOf(WeighBridgeRepository.ci(r, "Id")));
                    o.put("GpNo", WeighBridgeRepository.intOf(WeighBridgeRepository.ci(r, "GpSrNo")));
                    o.put("VehicleNo", "");
                    o.put("GatepassType", "");
                    rows.add(o);
                }
                break;
            }
            case "engr":
                rows = rename(repo.gatePassesInwardEngr(u, fy), "GpSrNo", "GpNo");
                break;
            case "outward": {
                /* RefDocNoGatePassOutward():1012 — the "More Than 1 Weight By 1GpNoO Against"
                   row decides CheckConfig. RECID is always 0 here: bending only runs in Save
                   mode. */
                boolean checkConfig = flag(u, CFG_MULTI_WEIGHT);
                rows = new ArrayList<>();
                for (Map<String, Object> r : repo.gatePassesOutward(u, fy, checkConfig, 0)) {
                    Map<String, Object> o = new LinkedHashMap<>();
                    o.put("Id", WeighBridgeRepository.ci(r, "Id"));
                    o.put("GpNo", WeighBridgeRepository.ci(r, "GpSrNo"));
                    o.put("VehicleNo", WeighBridgeRepository.ci(r, "VehicleNo"));
                    o.put("GatepassType", WeighBridgeRepository.ci(r, "GatepassType"));
                    rows.add(o);
                }
                break;
            }
            case "general": {
                if (documentTypeId != 52 && documentTypeId != 92) {
                    throw new IllegalArgumentException("Unknown general gate pass type");
                }
                rows = new ArrayList<>();
                for (Map<String, Object> r : repo.gatePassesGeneral(u, fy, documentTypeId)) {
                    Map<String, Object> o = new LinkedHashMap<>();
                    o.put("Id", WeighBridgeRepository.ci(r, "Id"));
                    o.put("GpNo", WeighBridgeRepository.ci(r, "GpSrNo"));
                    rows.add(o);
                }
                break;
            }
            case "party":
                rows = rename(repo.gatePassesPartyProcessing(u, documentTypeId), "GpSrNo", "GpNo");
                break;
            default:
                throw new IllegalArgumentException("Unknown gate pass list");
        }
        out.put("rows", rows);
        return out;
    }

    /**
     * cmbRefDocNo_Leave():3946 — the chosen gate pass's row. {@code isGatePassEntryUser} is the
     * :4010 test (UserAccount.ID == the gate pass's EntryUser), which decides whether the
     * supplier weights are shown or zeroed.
     */
    public Map<String, Object> gatePass(String kind, int documentTypeId, int id, int gpSrNo) {
        UserAccount u = currentUserContext.requireAccountingUser();
        int fy = currentUserContext.currentFinancialYearId();
        List<Map<String, Object>> rows;
        switch (kind == null ? "" : kind) {
            case "inward":
                if (documentTypeId != 51 && documentTypeId != 1601) throw new IllegalArgumentException("Unknown inward gate pass type");
                rows = repo.gatePassInwardById(u, fy, documentTypeId, id);
                break;
            case "outward":
                rows = repo.gatePassOutwardById(u, fy, 91, id);
                break;
            case "general":
                if (documentTypeId != 52 && documentTypeId != 92) throw new IllegalArgumentException("Unknown general gate pass type");
                rows = repo.gatePassGeneralByNo(u, fy, documentTypeId, gpSrNo);
                break;
            case "party":
                rows = repo.gatePassPartyProcessingByNo(u, fy, documentTypeId, gpSrNo);
                break;
            default:
                throw new IllegalArgumentException("Unknown gate pass kind");
        }
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> row = rows.isEmpty() ? null : rows.get(0);
        out.put("row", row);
        out.put("isGatePassEntryUser", row != null
                && u.getId() != null
                && u.getId() == WeighBridgeRepository.intOf(WeighBridgeRepository.ci(row, "EntryUser")));
        return out;
    }

    /** LabItemsBindByGpId(ActionId):1343. */
    public List<Map<String, Object>> labItems(int gpId, int actionId) {
        UserAccount u = currentUserContext.requireAccountingUser();
        return repo.labItemsByGatePass(u, gpId, actionId);
    }

    /** ItemsFromDeliveryOrder:1259. */
    public List<Map<String, Object>> deliveryOrderItems(int deliveryOrderId, int gpId) {
        UserAccount u = currentUserContext.requireAccountingUser();
        return repo.itemsByDeliveryOrder(u, deliveryOrderId, gpId);
    }

    /** ItemsFromSaleOrder:1301. */
    public List<Map<String, Object>> saleOrderItems(int saleOrderId) {
        UserAccount u = currentUserContext.requireAccountingUser();
        return repo.itemsBySaleOrder(u, saleOrderId);
    }

    /**
     * InvDeliveryOrder.GetPartiesAndItemsFromDeliveryOrderByGpId and
     * getInvoiceNoFromDeliveryOrderByGpId take a gate pass id and nothing else — no
     * organisation, no company. On the desktop that is safe because the id came out of this
     * company's own list. A web endpoint can be called with any id, so the outward gate pass is
     * first read through the company-scoped ReadByGpNo; an id that is not this company's returns
     * nothing rather than another company's parties or invoice numbers.
     */
    public List<Map<String, Object>> deliveryOrderPartiesAndItems(int gpId, int recId) {
        if (!ownsOutwardGatePass(gpId)) return new ArrayList<>();
        return repo.partiesAndItemsFromDeliveryOrder(gpId, recId);
    }

    public List<Map<String, Object>> deliveryOrderInvoices(int gpId) {
        if (!ownsOutwardGatePass(gpId)) return new ArrayList<>();
        return repo.invoiceNosFromDeliveryOrder(gpId);
    }

    private boolean ownsOutwardGatePass(int gpId) {
        if (gpId <= 0) return false;
        UserAccount u = currentUserContext.requireAccountingUser();
        return !repo.gatePassOutwardById(u, currentUserContext.currentFinancialYearId(), 91, gpId).isEmpty();
    }

    // =========================================================================== pending grids

    public List<Map<String, Object>> pendingFirst(String fromDate, String toDate, int gpNoFrom, int gpNoTo) {
        UserAccount u = currentUserContext.requireAccountingUser();
        return repo.pendingForFirstWeight(u, pickerValue(fromDate), pickerValue(toDate), gpNoFrom, gpNoTo);
    }

    /** bindGridPendingForSecond():1975 — FirstWeighBridgeId = dtWbList.Rows[0]["Id"]. */
    public List<Map<String, Object>> pendingSecond(String fromDate, String toDate, int gpNoFrom, int gpNoTo) {
        UserAccount u = currentUserContext.requireAccountingUser();
        int scaleId = firstScaleId(u);
        return repo.pendingForSecondWeight(u, currentUserContext.currentFinancialYearId(), scaleId,
                pickerValue(fromDate), pickerValue(toDate), gpNoFrom, gpNoTo);
    }

    // ================================================================================ history

    /** BindHistoryGrid():3537. dateMode is drdocdate | rdentrydate | rdmodifydate |
     *  rdapproveddate: it decides WHICH pair of date parameters the procedure receives. */
    public List<Map<String, Object>> history(String dateMode, String fromDate, String toDate,
                                             int ticketNoFrom, int ticketNoTo, int gpNoFrom, int gpNoTo,
                                             String vehicleNo, int refDocumentTypeId, int wbTypeId, String wbTypeText) {
        UserAccount u = currentUserContext.requireAccountingUser();
        HistoryFilter f = new HistoryFilter();
        f.canViewAllRecord = hasRight("CanView AllRecord");
        f.entryUser = u.getId() == null ? 0 : u.getId();
        f.firstWeighBridgeId = firstScaleId(u);
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
        /* :3584 — the WB-type TEXT, and only when its Value is > 0. */
        f.weighBridgeType = wbTypeId > 0 ? (wbTypeText == null ? "" : wbTypeText) : "";
        return repo.history(u, currentUserContext.currentFinancialYearId(), f);
    }

    // ============================================================================== read by id

    /**
     * ReadById(ID):2587. Sp_WbTransation_GetAllMethod ReadById takes only @Id — the desktop never
     * scopes it, because the id always came from this company's own grid. Over the web any id can
     * be asked for, so a row that carries a CompanyId belonging to another company is refused.
     * {@code approved} is the :2741 rule: a ticket with a second weight is approved, whatever its
     * IsApproved column says.
     */
    public Map<String, Object> load(int id) {
        UserAccount u = currentUserContext.requireAccountingUser();
        Map<String, Object> row = repo.readById(id);
        if (row == null) return null;
        Object company = WeighBridgeRepository.ci(row, "CompanyId");
        if (company != null && WeighBridgeRepository.intOf(company) != u.getCompanyId()) return null;
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("row", row);
        out.put("approved", approved(row));
        return out;
    }

    private static boolean approved(Map<String, Object> row) {
        if (WeighBridgeRepository.intOf(WeighBridgeRepository.ci(row, "SecondWeight")) > 0) return true;
        return toBool(WeighBridgeRepository.ci(row, "IsApproved"));
    }

    // ================================================================================ prints

    /** WbTransactionsReports.WbTransactionSlip280 — "280" without DocumentTypeId, "281 with
     *  images" with DocumentTypeId 102, exactly as the two print paths call it. */
    public List<Map<String, Object>> slip(int id, boolean withImages) {
        UserAccount u = currentUserContext.requireAccountingUser();
        return repo.slip280(u, id, withImages ? DOC_TYPE_ID : 0);
    }

    /** CommonServices.InwardGatePassWithWbAndLabSlip:14268 — the 257 slip and its two
     *  sub-reports. The weighbridge sub-report takes only @GpId, so it is read only after the
     *  company-scoped main set has confirmed the gate pass is this company's. */
    public Map<String, Object> inwardGatePassSlip(int gatePassId) {
        if (gatePassId <= 0) throw new IllegalArgumentException("No Record Found For Display");
        UserAccount u = currentUserContext.requireAccountingUser();
        List<Map<String, Object>> main = repo.inwardGatePassSlip(u, gatePassId);
        if (main.isEmpty()) throw new IllegalArgumentException("No Record Found For Display");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("main", main);
        out.put("lab", repo.labAnalysisByGatePassSubReport(u, gatePassId));
        out.put("weighBridge", repo.wbByGatePassSubReport(gatePassId));
        return out;
    }

    // ================================================================================== save

    /** btnsave_Click:2347. */
    public Map<String, Object> save(Request r) {
        UserAccount u = currentUserContext.requireAccountingUser();
        if (!hasRight("Save")) throw new IllegalStateException("You do not have the Save right on Weigh Bridge.");
        if (r == null) throw new IllegalArgumentException("Nothing to save.");
        r.id = 0;
        boolean lstIsSavedVehicle = flag(u, CFG_SAVED_VEHICLE);
        boolean partyItemWise = flag(u, CFG_OUTWARD_PARTY_ITEM);
        formValidation(u, r, false, lstIsSavedVehicle, partyItemWise);

        int scaleId = firstScaleId(u);
        boolean cameraPath = !cfg(u, CFG_CAMERA_PATH).trim().isEmpty();
        Timestamp now = Timestamp.valueOf(LocalDateTime.now());

        WeighBridgeRecord wb = new WeighBridgeRecord();
        wb.TicketNo = nz(r.ticketNo);
        wb.BiltyNo = s(r.biltyNo);
        wb.EntryNo = 0;
        wb.SupplierCustomerId = nz(r.supplierCustomerId);
        wb.InvoiceId = nz(r.invoiceId);
        wb.PartyName = s(r.partyName);
        wb.WbCharges = nd(r.wbCharges);
        wb.VehicleNo = (nz(r.refTypeId) == 74 && lstIsSavedVehicle) ? s(r.savedVehicleNo) : s(r.vehicleNo);
        wb.VehicleType = s(r.vehicleType);
        wb.WeighBridgeType = s(r.weighBridgeType);
        wb.PackingType = s(r.packingType);
        wb.ItemQty = nd(r.itemQty);
        wb.ItemId = nz(r.itemId);
        wb.ItemDescription = s(r.itemDescription);
        wb.WbRemarks = s(r.remarks);
        wb.FirstWeight = nd(r.firstWeight);
        wb.SecondWeight = nd(r.secondWeight);
        wb.FirstDateTime = clientTime(r.firstDateTime, now);
        wb.SecondDateTime = clientTime(r.secondDateTime, now);
        wb.PackingUnitWeight = nd(r.packingUnitWeight);
        wb.PackingTotalWeight = nd(r.packingTotalWeight);
        wb.WeightAfterPackingWeight = nd(r.weightAfterPackingWeight);
        applyReference(wb, r, false);
        wb.DocTypeId = DOC_TYPE_ID;
        wb.DocDate = now;
        wb.EntryDate = now;
        wb.ModifyDate = now;
        wb.EntryUser = u.getId();
        wb.ModifyUser = u.getId();
        wb.BranchId = u.getBranchesId() == null ? 0 : u.getBranchesId();
        wb.OrganizationId = u.getOrganizationId();
        wb.CompanyId = u.getCompanyId();
        wb.FirstWeighBridgeId = scaleId;
        wb.FinancialYearId = currentUserContext.currentFinancialYearId();
        wb.MultiItem = false;
        /* :2470 — the condition is always true, so only the camera path decides: the names are
           UniqFileName..3, i.e. what F5 captured on the server ("" when nothing was taken). */
        if (cameraPath) {
            wb.FirstWtPicReading = picture(r.firstWtPicReading, "");
            wb.FirstWtUpperPic = picture(r.firstWtUpperPic, "");
            wb.FirstWtFrontPic = picture(r.firstWtFrontPic, "");
        }
        wb.LoadWeight = nd(r.supplierFirstWeight);
        wb.TearWeight = nd(r.supplierSecondWeight);
        wb.SupplierWbNetWeight = nd(r.supplierNetWeight);

        int saved = bllSave(u, wb);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", saved > 0);
        out.put("id", saved);
        out.put("message", "Record Save Successfully" + wb.TicketNo);
        out.put("nextTicketNo", repo.nextTicketNo(u, currentUserContext.currentFinancialYearId()));
        return out;
    }

    /** btnUpdate_Click:2951. */
    public Map<String, Object> update(Request r) {
        UserAccount u = currentUserContext.requireAccountingUser();
        if (!hasRight("Update")) throw new IllegalStateException("You do not have the Update right on Weigh Bridge.");
        if (r == null || r.id == null || r.id <= 0) throw new IllegalArgumentException("Record Not Found");

        Map<String, Object> existing = load(r.id);
        if (existing == null) throw new IllegalArgumentException("Record Not Found");
        @SuppressWarnings("unchecked")
        Map<String, Object> row = (Map<String, Object>) existing.get("row");
        /* :2955 */
        if (Boolean.TRUE.equals(existing.get("approved"))) {
            throw new IllegalArgumentException("Record cannot be Updated because Record has Approved");
        }

        boolean lstIsSavedVehicle = flag(u, CFG_SAVED_VEHICLE);
        boolean partyItemWise = flag(u, CFG_OUTWARD_PARTY_ITEM);
        formValidation(u, r, true, lstIsSavedVehicle, partyItemWise);

        /* :2963 — GatePass Inward: a net-weight difference beyond the tolerance needs remarks. */
        if ("GatePass Inward".equals(r.refTypeText)) {
            double tolerance = toDouble(cfg(u, CFG_TOLERANCE));
            double wbNet = nd(r.netWeight), supplierNet = nd(r.supplierNetWeight);
            if (Math.abs(wbNet - supplierNet) > tolerance && s(r.weightDiffComments).trim().isEmpty()) {
                throw new IllegalArgumentException(
                        "Supplier Net Weight in Weigh Bridge does not match the Weighbridge Net Weight.\n"
                      + "Supplier Net Weight: " + cs(supplierNet) + "\n"
                      + "Weighbridge Net Weight: " + cs(wbNet) + "\n\n"
                      + "Weighbridge Tolerance is : " + cs(tolerance) + "\n\n"
                      + "Difference Weight Remarks are required");
            }
        }

        int scaleId = firstScaleId(u);
        boolean cameraPath = !cfg(u, CFG_CAMERA_PATH).trim().isEmpty();
        Timestamp now = Timestamp.valueOf(LocalDateTime.now());

        WeighBridgeRecord wb = new WeighBridgeRecord();
        wb.Id = r.id;
        wb.TicketNo = nz(r.ticketNo);
        wb.BiltyNo = s(r.biltyNo).trim();
        wb.EntryNo = 0;
        wb.SupplierCustomerId = nz(r.supplierCustomerId);
        wb.InvoiceId = nz(r.invoiceId);
        wb.PartyName = s(r.partyName).trim();
        wb.WbCharges = nd(r.wbCharges);
        wb.VehicleNo = s(r.vehicleNo).trim();              // :3001 — txtVehicleNo, never cmbVehicleNo
        wb.VehicleType = s(r.vehicleType);
        wb.WeighBridgeType = s(r.weighBridgeType);
        wb.PackingType = s(r.packingType);
        wb.ItemQty = nd(r.itemQty);
        wb.ItemId = nz(r.itemId);
        wb.ItemDescription = s(r.itemDescription).trim();
        wb.WbRemarks = s(r.remarks).trim();
        wb.SecondWeight = nd(r.secondWeight);
        wb.NetWbWeight = nd(r.netWeight);
        wb.SecondDateTime = clientTime(r.secondDateTime, now);
        wb.FirstDateTime = clientTime(r.firstDateTime, now);
        wb.FirstWeight = nd(r.firstWeight);
        wb.IsApproved = true;
        wb.ApprovedDateTime = now;
        wb.ApprovedUserId = u.getId();
        wb.WeightDiffComments = s(r.weightDiffComments).trim();
        wb.PackingUnitWeight = nd(r.packingUnitWeight);
        wb.PackingTotalWeight = nd(r.packingTotalWeight);
        wb.WeightAfterPackingWeight = nd(r.weightAfterPackingWeight);
        applyReference(wb, r, true);
        wb.LoadWeight = nd(r.supplierFirstWeight);
        wb.TearWeight = nd(r.supplierSecondWeight);
        wb.SupplierWbNetWeight = nd(r.supplierNetWeight);
        wb.DocTypeId = DOC_TYPE_ID;
        wb.DocDate = now;
        wb.EntryDate = now;
        wb.ModifyDate = now;
        wb.EntryUser = u.getId();
        wb.ModifyUser = u.getId();
        wb.BranchId = u.getBranchesId() == null ? 0 : u.getBranchesId();
        wb.OrganizationId = u.getOrganizationId();
        wb.CompanyId = u.getCompanyId();
        wb.SecondWeighBridgeId = scaleId;                  // FirstWeighBridgeId stays 0 — see the class note
        wb.FinancialYearId = currentUserContext.currentFinancialYearId();
        wb.MultiItem = r.multiItem;
        if (cameraPath) {
            /* :3088 — UniqFileName4..6: loaded from the ticket by ReadById, replaced by F5's
               second-weight capture when one was taken. */
            wb.SecondPicReading = picture(r.secondPicReading, str(WeighBridgeRepository.ci(row, "SecondPicReading")));
            wb.SecondWtUpperPic = picture(r.secondWtUpperPic, str(WeighBridgeRepository.ci(row, "SecondWtUpperPic")));
            wb.SecondWtFrontPic = picture(r.secondWtFrontPic, str(WeighBridgeRepository.ci(row, "SecondWtFrontPic")));
        }

        int saved = bllSave(u, wb);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", saved > 0);
        out.put("id", saved);
        out.put("message", saved > 0 ? "Record Update Successfully" + wb.TicketNo : "");
        out.put("nextTicketNo", repo.nextTicketNo(u, currentUserContext.currentFinancialYearId()));
        return out;
    }

    /** The ReferenceDocTypeId / ReferenceDocNoId / ContainerNo / WorkingReportNo blocks of
     *  btnsave_Click:2400-2426 and btnUpdate_Click:3029-3061 — by reference-type TEXT, then the
     *  54/55 party-processing test by VALUE. */
    private static void applyReference(WeighBridgeRecord wb, Request r, boolean update) {
        String text = s(r.refTypeText);
        int wrn = toInt(r.workingReportNo);
        if ("GatePass Inward".equals(text)) {
            wb.ReferenceDocTypeId = 51;
            wb.ReferenceDocNoId = nz(r.refDocId);
        }
        if ("GatePass OutWard".equals(text)) {
            wb.ReferenceDocTypeId = 91;
            wb.ReferenceDocNoId = nz(r.refDocId);
            wb.ContainerNo = s(r.containerNo).trim();
            wb.WorkingReportNo = wrn;
        }
        if ("Move Order".equals(text)) {
            wb.ReferenceDocTypeId = 74;
            wb.ReferenceDocNoId = update ? nz(r.id) : toInt(r.moveOrderRefNo);
            wb.WorkingReportNo = wrn;
        }
        if ("General".equals(text)) {
            wb.ReferenceDocTypeId = 75;
            wb.ReferenceDocNoId = update ? nz(r.id) : toInt(r.moveOrderRefNo);
        }
        if ("General GatePass Inward".equals(text)) {
            wb.ReferenceDocTypeId = 52;
            wb.ReferenceDocNoId = nz(r.refDocId);
            wb.WorkingReportNo = wrn;
        }
        if ("General GatePass Outward".equals(text)) {
            wb.ReferenceDocTypeId = 92;
            wb.ReferenceDocNoId = nz(r.refDocId);
            wb.WorkingReportNo = wrn;
        }
        int v = nz(r.refTypeId);
        if (v == 54 || v == 55) {
            wb.ReferenceDocTypeId = v;
            wb.ReferenceDocNoId = nz(r.refDocId);
        }
    }

    /**
     * BLL 0091 WbTransactions.Save:448 — the voucher is composed FIRST (so it carries the
     * operator as ModifyUser even on an insert), then an insert zeroes ModifyUser and, for Move
     * Order / General, replaces ReferenceDocNoId with the ticket number.
     */
    /** Shared with WeighBridgeManualService — screen 410 calls the same BLL Save. */
    public int bllSave(UserAccount u, WeighBridgeRecord wb) {
        int cash = WeighBridgeRepository.intOf(repo.config(u, CFG_CASH_ACCOUNT));
        int income = WeighBridgeRepository.intOf(repo.config(u, CFG_OTHER_INCOME_ACCOUNT));
        ContraVoucherDto.Head head = null;
        List<ContraVoucherDto.Detail> lines = null;
        if (nd(wb.WbCharges) > 0d && income > 0 && cash > 0) {
            head = new ContraVoucherDto.Head();
            lines = new ArrayList<>();
            makeVoucher(wb, cash, income, head, lines);
        }
        String proc;
        if (wb.Id == null || wb.Id == 0) {
            wb.ModifyUser = 0;
            if (nz(wb.ReferenceDocTypeId) == 74 || nz(wb.ReferenceDocTypeId) == 75) {
                wb.ReferenceDocNoId = wb.TicketNo;
            }
            proc = "dbo.Sp_WbTransactions_Insert";
        } else {
            proc = "dbo.Sp_WbTransactions_Update";
        }
        return repo.save(wb, proc, head, lines);
    }

    /** BLL 0091 WbTransactions.MakeVoucher:395 — one debit to the cash account and one credit to
     *  other income, both carrying the same comment. */
    private static void makeVoucher(WeighBridgeRecord wb, int cashAccount, int otherIncomeAccount,
                                    ContraVoucherDto.Head h, List<ContraVoucherDto.Detail> lines) {
        String docDate = iso(wb.DocDate);
        String now = iso(Timestamp.valueOf(LocalDateTime.now()));
        h.DocumentTypeId = wb.DocTypeId;
        h.DocumentTypeSrNo = wb.Id;
        h.RefDocNoId = wb.Id;
        h.VoucherCode = wb.TicketNo;
        h.VoucherDate = docDate;
        h.Remarks = s(wb.WbRemarks);
        h.RemarksOtherLingo = "";
        h.ChequeDate = LocalDate.now().atStartOfDay().format(ISO);   // DateTime.Today
        h.IncludeWHT = false;
        h.BranchId = wb.BranchId;
        h.ProjectId = wb.ProjectId;
        h.BillAmount = 0d;
        h.ManualBillNo = "";
        h.DueDate = docDate;
        h.DueDays = 0;
        h.OrganizationId = wb.OrganizationId;
        h.CompanyId = wb.CompanyId;
        h.FinancialYearId = wb.FinancialYearId;
        h.EntryUser = wb.EntryUser;
        h.EntryDate = now;
        h.ModifyDate = now;
        h.ModifyUser = wb.ModifyUser;

        String text = "Vehicle #" + s(wb.VehicleNo) + " | Charges: " + cs(nd(wb.WbCharges))
                    + " | First Weight: " + cs(nd(wb.FirstWeight));
        if (nd(wb.SecondWeight) > 0d) {
            text += " | Second Weight: " + cs(nd(wb.SecondWeight)) + " | Net Weight: " + cs(nd(wb.NetWbWeight));
        }
        ContraVoucherDto.Detail debit = new ContraVoucherDto.Detail();
        debit.AccountId = cashAccount;
        debit.AgainstAccountId = otherIncomeAccount;
        debit.Comments = text;
        debit.DebitAmount = nd(wb.WbCharges);
        lines.add(debit);

        ContraVoucherDto.Detail credit = new ContraVoucherDto.Detail();
        credit.AccountId = otherIncomeAccount;
        credit.AgainstAccountId = cashAccount;
        credit.Comments = text;
        credit.CreditAmount = nd(wb.WbCharges);
        lines.add(credit);
    }

    /**
     * FormValidation():1531. {@code update} is "btnUpdate is Visible and Enabled" — the form is
     * showing a loaded ticket; otherwise it is "btnsave is Visible and Enabled". The rights part
     * of Enabled has already been enforced by the caller.
     */
    private void formValidation(UserAccount u, Request r, boolean update,
                                boolean lstIsSavedVehicle, boolean partyItemWise) {
        int refType = nz(r.refTypeId);
        String text = s(r.refTypeText);
        if (!r.refTypeSelected) fail("ReferenceType Field is Required");
        if (refType == 51) {
            if (nd(r.supplierFirstWeight) == 0d) fail("Supplier First Weight Field is Required");
            if (update) {
                if (nd(r.supplierSecondWeight) == 0d) fail("Supplier Second Weight Field is Required");
                if (nd(r.supplierNetWeight) == 0d) fail("Supplier Net Weight Field is Required");
            }
        }
        boolean moveOrGeneral = "Move Order".equals(text) || "General".equals(text);
        if (moveOrGeneral) {
            String m = s(r.moveOrderRefNo).trim();
            if (m.isEmpty() || "0".equals(m)) fail("GatePass No Field is Required");
        } else if (!r.refDocSelected) {
            fail("GatePass No Field is Required");
        }
        if (!r.weighBridgeTypeSelected) fail("WeighBridge Type Field is Required");
        if (refType == 91 && r.refDocSelected && nz(r.refDocId) > 0
                && "Export".equals(s(r.refDocGatepassType))
                && (!r.invoiceSelected || nz(r.invoiceId) == 0)) {
            fail("Invoice number Field is Required");
        }
        if ((refType == 74 || refType == 75) && (!r.packingTypeSelected || nz(r.packingTypeId) == 0)) {
            fail("PackingType Field is Required");
        }
        if (update) {
            if (nd(r.secondWeight) == 0d) fail("Second Weight  Field is Required");
            String wrn = s(r.workingReportNo).trim();
            boolean wrnMissing = wrn.isEmpty() || "0".equals(wrn);
            if ("Move Order".equals(text) || "General GatePass Inward".equals(text)
                    || "General GatePass Outward".equals(text)) {
                if (wrnMissing) fail("WorkingReportNo Field is Required");
            } else if ("GatePass OutWard".equals(text) && wrnMissing) {
                if ("StockTransfer".equals(repo.deliveryOrderType(u, nz(r.refDocId)))) {
                    fail("WorkingReportNo Field is Required");
                }
            }
        }
        if (!update && nd(r.firstWeight) == 0d) fail("First Weight  Field is Required");
        if (refType == 74 && lstIsSavedVehicle) {
            if (s(r.savedVehicleNo).trim().isEmpty()) fail("Vehicle Number Field is Required");
        } else if (s(r.vehicleNo).trim().isEmpty()) {
            fail("Vehicle Number Field is Required");
        }
        if (nz(r.ticketNo) == 0) fail("Ticket No Field is Required");
        if (partyItemWise && "GatePass OutWard".equals(text) && !"Export".equals(s(r.refDocGatepassType))) {
            if (!r.partySelected || nz(r.supplierCustomerId) == 0) fail("PartyName Field is Required");
            if (!r.itemSelected || nz(r.itemId) == 0) fail("ItemName Field is Required");
            if (nz(r.supplierCustomerId) != nz(r.itemPartyId)) fail("PartyName Field is Required");
        }
        if (s(r.remarks).trim().isEmpty()) fail("Remarks Field is Required");
    }

    private static void fail(String message) { throw new IllegalArgumentException(message); }

    /** dtWbList.Rows[0]["Id"]. With an empty scales list the desktop throws the DataTable's own
     *  "There is no row at position 0." — the same words are returned here. */
    private int firstScaleId(UserAccount u) {
        List<Map<String, Object>> scales = repo.scalesList(u);
        if (scales.isEmpty()) throw new IllegalArgumentException("There is no row at position 0.");
        return WeighBridgeRepository.intOf(WeighBridgeRepository.ci(scales.get(0), "Id"));
    }

    // ================================================================================= rights

    /** CommonServices.SetRightsValueInRightsObject:17565 — the grant row for THIS screen, with
     *  the desktop's Admin short-circuit for Save, Update, Print and CanView AllRecord. */
    private boolean hasRight(String rightName) {
        String role = currentUserContext.currentRoleName();
        if ("Admin".equals(role)) return true;
        try {
            List<Map<String, Object>> rights = jdbcTemplate.queryForList(
                    SQL_USER_RIGHTS,
                    currentUserContext.currentUserId(),
                    SCREEN_NAME,
                    role == null ? "" : role,
                    currentUserContext.currentCompanyId(),
                    "GetByUserId");
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

    // ============================================================================== helpers

    private String cfg(UserAccount u, String name) {
        try {
            return repo.config(u, name);
        } catch (Exception e) {
            /* GetConfigValueFromGlobal returns "" for a missing row; Conversion.ToBool("") is
               false and Conversion.ToInt("") is 0. A read failure is logged and treated the same. */
            LOG.warn("Configuration '{}' could not be read; treating as unset", name, e);
            return "";
        }
    }

    private boolean flag(UserAccount u, String name) { return toBool(cfg(u, name)); }

    /** A picture name from the page is written only if this session's cameras took it or the
     *  ticket already holds it; anything else keeps the stored value. null means "not sent". */
    private String picture(String sent, String stored) {
        if (sent == null) return stored;
        return devices.acceptablePictureName(sent, stored) ? sent : stored;
    }

    private static final DateTimeFormatter ISO = DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss");

    private static String iso(Timestamp t) {
        return t == null ? null : t.toLocalDateTime().format(ISO);
    }

    /**
     * A WinForms DateTimePicker keeps the time of day it was created with; choosing a date only
     * changes the date. The history and pending pickers start at DateTime.Now, so the value the
     * desktop sends is the chosen DAY at the current time of day. Reproduced: null when the
     * picker's checkbox was cleared (the page sends nothing).
     */
    private static Timestamp pickerValue(String day) {
        if (day == null || day.trim().isEmpty()) return null;
        try {
            LocalDate d = LocalDate.parse(day.trim().substring(0, 10));
            return Timestamp.valueOf(LocalDateTime.of(d, LocalTime.now().withNano(0)));
        } catch (Exception e) {
            throw new IllegalArgumentException("Invalid date: " + day);
        }
    }

    /** txtFirstDate / txtSecondDate — time pickers the operator cannot edit (they are disabled
     *  in every live path), whose value is the moment the form was last refreshed, or the stored
     *  FirstDateTime after a ticket was opened. The page sends that value back. */
    private static Timestamp clientTime(String iso, Timestamp fallback) {
        if (iso == null || iso.trim().isEmpty()) return fallback;
        String t = iso.trim().replace('T', ' ');
        if (t.length() == 16) t += ":00";
        if (t.length() > 19) t = t.substring(0, 19);
        try {
            return Timestamp.valueOf(t);
        } catch (Exception e) {
            return fallback;
        }
    }

    private static List<Map<String, Object>> rename(List<Map<String, Object>> rows, String from, String to) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : r.entrySet()) {
                o.put(e.getKey().equalsIgnoreCase(from) ? to : e.getKey(), e.getValue());
            }
            out.add(o);
        }
        return out;
    }

    /** C# double.ToString() for the values this screen formats: no trailing ".0". */
    static String cs(double d) {
        if (d == Math.rint(d) && Math.abs(d) < 1e15) return String.valueOf((long) d);
        return BigDecimal.valueOf(d).stripTrailingZeros().toPlainString();
    }

    private static String s(String v) { return v == null ? "" : v; }
    private static String str(Object v) { return v == null ? "" : String.valueOf(v); }
    private static int nz(Integer v) { return v == null ? 0 : v; }
    private static double nd(Double v) { return v == null || v.isNaN() ? 0d : v; }

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
        try { return Double.parseDouble(v.trim()); } catch (NumberFormatException e) { return 0d; }
    }

    private static boolean toBool(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number)  return ((Number) v).intValue() != 0;
        if (v == null) return false;
        String t = String.valueOf(v).trim();
        return "1".equals(t) || "true".equalsIgnoreCase(t) || "yes".equalsIgnoreCase(t);
    }
}
