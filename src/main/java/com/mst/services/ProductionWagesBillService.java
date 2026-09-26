package com.mst.services;

import com.mst.repositories.ProductionWagesBillRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.PlatformTransactionManager;
import org.springframework.transaction.support.TransactionTemplate;

import java.math.BigDecimal;
import java.math.MathContext;
import java.math.RoundingMode;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;

import static com.mst.repositories.ProductionWagesBillRepository.DOC_TYPE_WAGES;
import static com.mst.repositories.ProductionWagesBillRepository.ci;
import static com.mst.repositories.ProductionWagesBillRepository.str;
import static com.mst.repositories.ProductionWagesBillRepository.toBool;
import static com.mst.repositories.ProductionWagesBillRepository.toDouble;
import static com.mst.repositories.ProductionWagesBillRepository.toInt;

/**
 * frmwagesBillHeader.cs - Contractor Labour Wages bill (DocumentTypeId 101).
 *
 * The page keeps the form's DataTables (dtdetail = Regular Wages, dtStiching = Other Wages) in the
 * browser and runs the grid events there; this service answers every BLL call the form makes and
 * runs Insert() from its first data check onwards: the reference-row checks, both grid loops with
 * their exact messages, then BLL InvContractorWagesBillHeader.Save = MakeVoucher + DAL SetData.
 *
 * Tenancy (organization, company, branch, financial year, user, role) always comes from the
 * session. The browser sends only what the operator typed, picked or loaded.
 */
@Service
public class ProductionWagesBillService {

    /** Thrown for the desktop's own validation messages (shown verbatim, HTTP 400). */
    public static class WagesValidationException extends RuntimeException {
        public WagesValidationException(String m) { super(m); }
    }

    @Autowired private ProductionWagesBillRepository repo;
    @Autowired private CurrentUserContext cu;
    @Autowired private PlatformTransactionManager txManager;

    private int org()    { return cu.currentOrganizationId(); }
    private int comp()   { return cu.currentCompanyId(); }
    private int branch() { return cu.currentBranchId(); }
    private int year()   { return cu.currentFinancialYearId(); }
    private int user()   { return cu.currentUserId(); }
    private String role() { String r = cu.currentRoleName(); return r == null ? "" : r; }

    private static final String[] BOOL_CONFIGS = {
            "EnableAddLessOnWagesRegular",
            "ContractorWageComparisonbyActivityForGRN",
            "ContractorWageComparisonbyActivityForGDN",
            "ContractorWageComparisonbyActivityForForwarding",
            "ContractorWageComparisonbyActivityForStockTransfer",
            "ContractorWageComparisonbyActivityForProductionInput",
            "ContractorWageComparisonbyActivityForProductionConsumption",
            "StichingWagesCompulsory",
            "OtherWagesCompulsoryForStockConversion"};

    // ========================================================================================= Load

    /**
     * frmwagesBillHeader_Load:321 - everything Load reads once: the configurations, rights
     * (SetRightsValueInRightsObject), feature 11 (MultiBranchFeatur), the wages-type table, the
     * contractor list (suppliercustomer()), the next doc no (GenerateDocNo) and, when a reference
     * document was passed, GetIdByRefDocTypeIdAndRefDocId (RECID).
     */
    public Map<String, Object> state(int refDocTypeId, int refDocId) {
        int org = org(), comp = comp();
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> cfg = new LinkedHashMap<>();
        for (String n : BOOL_CONFIGS) cfg.put(n, toBool(repo.config(org, comp, n)));
        cfg.put("PercentageForRateAddLess", toDouble(repo.config(org, comp, "PercentageForRateAddLess")));
        /* WagesAmountCalculationsConfigurations:557 - only when the value is not "". */
        String q = repo.config(org, comp, "WagesAmountCalculateOnQty");
        cfg.put("WagesAmountCalculateOnQty", !q.isEmpty() && toBool(q));
        out.put("config", cfg);
        out.put("rights", rights());
        out.put("roleName", role());
        out.put("isAdmin", "Admin".equals(role()));
        boolean multiBranch = repo.erpFeature(org, comp, 11);
        out.put("multiBranch", multiBranch);
        out.put("wagesTypeIds", repo.wagesTypeIdsAgainstDocumentType());
        out.put("contractors", contractors(multiBranch));
        out.put("docNo", repo.generateCode(org, comp, year()));
        int recId = 0;
        if (refDocTypeId > 0 && refDocId > 0) {
            recId = repo.idByReference(org, comp, refDocTypeId, refDocId, year(), null);
        }
        out.put("recId", recId);
        return out;
    }

    /**
     * SetRightsValueInRightsObject: Admin gets Save/Update/Print; otherwise the grant rows decide.
     * Load:345-347 - btnsave.Enabled / Print.Enabled / btnUpdate.Enabled.
     */
    public Map<String, Boolean> rights() {
        boolean admin = "Admin".equals(role());
        Map<String, Boolean> r = new LinkedHashMap<>();
        r.put("save", admin);
        r.put("update", admin);
        r.put("print", admin);
        for (Map<String, Object> row : repo.userRights(user(), role(), comp())) {
            String name = str(ci(row, "RightName"));
            boolean v = toBool(ci(row, "Value"));
            if ("Save".equals(name)) r.put("save", admin || v);
            else if ("Update".equals(name)) r.put("update", admin || v);
            else if ("Print".equals(name)) r.put("print", admin || v);
        }
        return r;
    }

    /** suppliercustomer():493 - ReadByOrganizationCompanyIdForContractorWages, or the branch list. */
    public List<Map<String, Object>> contractors(boolean multiBranch) {
        List<Map<String, Object>> rows = multiBranch
                ? repo.contractorsAllocatedToBranch(org(), comp(), branch())
                : repo.contractorsForWages(org(), comp());
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(ci(r, "Id")));
            m.put("CompanyName", str(ci(r, "CompanyName")));
            out.add(m);
        }
        return out;
    }

    /** btnRefresh_Click:2537 - suppliercustomer() again. */
    public List<Map<String, Object>> contractors() {
        return contractors(repo.erpFeature(org(), comp(), 11));
    }

    /** CommonServices.GetWagesAccount(ids, 0, 1) - dtwagesacc / dtStichingItems. */
    public List<Map<String, Object>> wagesAccounts(String ids) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.wagesAccounts(org(), comp(), ids, 0, 1)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(ci(r, "Id")));
            m.put("WagesAccountName", str(ci(r, "WagesAccountName")));
            out.add(m);
        }
        return out;
    }

    public int nextDocNo() { return repo.generateCode(org(), comp(), year()); }

    /** GetIdByRefDocTypeIdAndRefDocId with an optional RefDocument (LoadDataForWages:2186). */
    public int idByReference(int refDocumentTypeId, int refDocNoId, String refDocument) {
        return repo.idByReference(org(), comp(), refDocumentTypeId, refDocNoId, year(), refDocument);
    }

    /** DocumentTypeFillForCombo():569 - rows whose Activity is "RefDocumentType". */
    public List<Map<String, Object>> historyDocumentTypes() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.comboAgainstContractorWages(org(), comp(), "RefDocumentType",
                String.valueOf(branch()))) {
            if (!"RefDocumentType".equals(str(ci(r, "Activity")))) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ci(r, "Id"));
            m.put("name", ci(r, "ReferenceName"));
            out.add(m);
        }
        return out;
    }

    /** PendingGrnAndGdn():1974 when a reference document was passed, PendingTicket():2040 otherwise. */
    public List<Map<String, Object>> pending(int refDocTypeId, int refDocId) {
        List<Map<String, Object>> rows = (refDocTypeId > 0 && refDocId > 0)
                ? repo.pendingByReference(org(), comp(), refDocTypeId, refDocId)
                : repo.pendingAll(org(), comp(), branch(), refDocTypeId, refDocId);
        List<Map<String, Object>> out = new ArrayList<>();
        /* :2003-2008 - RefLineId numbers the rows of one Id 1, 2, 3 ... */
        Map<Integer, Integer> idToRefLineId = new LinkedHashMap<>();
        for (Map<String, Object> r : rows) {
            int id = toInt(ci(r, "Id"));
            int line = idToRefLineId.containsKey(id) ? idToRefLineId.get(id) + 1 : 1;
            idToRefLineId.put(id, line);
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", id);
            m.put("DocumentTypeId", toInt(ci(r, "DocumentTypeId")));
            m.put("DocumentTypeDescription", ci(r, "DocumentTypeDescription"));
            m.put("DocDate", ci(r, "DocDate"));
            m.put("DocNo", ci(r, "DocNo"));
            m.put("SupplierCustomer", ci(r, "SupplierCustomer"));
            m.put("GpNo", ci(r, "GpNo"));
            m.put("VehicleNo", ci(r, "VehicleNo"));
            m.put("BiltyNo", ci(r, "BiltyNo"));
            m.put("GrossWeight", ci(r, "GrossWeight"));
            m.put("TotalQty", ci(r, "TotalQty"));
            m.put("RefDocQty", ci(r, "GrossWeight"));
            m.put("RefDocWeight", ci(r, "TotalQty"));
            m.put("RefLineId", line);
            out.add(m);
        }
        return out;
    }

    /** LoadDataForWages:2209 - InvGrn.GetGrnDetialForContractorWages. */
    public List<Map<String, Object>> referenceDetail(int id, int documentTypeId, String reqType) {
        return repo.referenceDetail(org(), comp(), id, documentTypeId, reqType);
    }

    /** CheckItemsFreeofcostforWages for several rows (the desktop calls it once per row). */
    public List<Boolean> freeOfCost(List<Map<String, Object>> rows) {
        List<Boolean> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            out.add(repo.freeOfCost(org(), comp(), dateOf(r.get("date")), toInt(r.get("refDocumentTypeId")),
                    toInt(r.get("itemId")), toInt(r.get("wagesAccountId"))));
        }
        return out;
    }

    /** CommonServices.GetWagesRate for several rows (AddContractorValuesForAllInGrid loops it). */
    public List<Map<String, Object>> wagesRates(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            double[] v = repo.wagesRate(org(), comp(), dateOf(r.get("date")), toDouble(r.get("packSize")),
                    toInt(r.get("wagesAccountId")), toInt(r.get("contractorId")));
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("WagesRate", v[0]);
            m.put("ScheduleId", (int) v[1]);
            out.add(m);
        }
        return out;
    }

    /** InvContractorWagesBillHeader.GetByID. */
    public Map<String, Object> bill(int id) { return repo.getById(id); }

    /** RetreivalDetailGridDeletedData:2343. */
    public List<Map<String, Object>> previousRecords(int refDocTypeId, int refDocId) {
        return repo.readPreviousRecords(org(), comp(), year(), refDocTypeId, refDocId);
    }

    /** ValidationOnformClose:2614/2626 - WagesDeleteByRefDocTypeAndId. */
    public void wagesDeleteByRefDocTypeAndId(int refDocTypeId, int refDocId) {
        repo.wagesDeleteByRefDocTypeAndId(org(), comp(), refDocTypeId, refDocId);
    }

    /** bindHistory():3247. */
    public List<Map<String, Object>> history(String from, String to, String fromDocNo, String toDocNo,
                                             int refDocumentTypeId) {
        return repo.formHistory(org(), comp(), year(), branch(), dateOrNull(from), dateOrNull(to),
                toInt(fromDocNo), toInt(toDocNo), refDocumentTypeId);
    }

    /** DataGridHistory_ColumnButtonClick "Voucher":3420 - VoucherHeadIdGet(Id, 101). */
    public int voucherHeadId(int id) { return repo.voucherHeadId(org(), comp(), DOC_TYPE_WAGES, id); }

    /** BtnCancelPendingRecords_Click:4037 - Admin only (the button is only visible to Admin). */
    public String cancelPending(List<Map<String, Object>> rows) {
        if (!"Admin".equals(role())) throw new WagesValidationException("Access denied.");
        if (rows == null || rows.isEmpty()) throw new WagesValidationException("Please select check box first");
        final int org = org(), comp = comp(), user = user();
        new TransactionTemplate(txManager).executeWithoutResult(s -> {
            for (Map<String, Object> r : rows) {
                repo.cancelPendingRecord(org, comp, toInt(r.get("documentTypeId")), toInt(r.get("id")), user);
            }
        });
        return "Record Approve Successfully";
    }

    // ================================================================================== Insert()

    /**
     * Insert():2702 from the reference-row check onwards (FormValidation, the 68/806 total check and
     * the Save/Update confirmation already ran in the page), then BLL Save -> MakeVoucher -> SetData.
     */
    @SuppressWarnings("unchecked")
    public Map<String, Object> save(Map<String, Object> in) {
        final int org = org(), comp = comp(), user = user();
        Map<String, Boolean> rights = rights();
        int recId = toInt(in.get("recId"));
        if (recId > 0 ? !Boolean.TRUE.equals(rights.get("update")) : !Boolean.TRUE.equals(rights.get("save"))) {
            throw new WagesValidationException("Access denied.");
        }
        List<Map<String, Object>> regular = (List<Map<String, Object>>) in.getOrDefault("regular", new ArrayList<>());
        List<Map<String, Object>> other = (List<Map<String, Object>>) in.getOrDefault("other", new ArrayList<>());
        Map<String, Object> accountNames = (Map<String, Object>) in.getOrDefault("wagesAccountNames", new LinkedHashMap<>());

        int refDocTypeId = toInt(in.get("refDocTypeId"));        // the dialog's RefDocTypeId
        int cmb = toInt(in.get("refDocumentTypeId"));            // cmbReferenceDocType.Value
        String entryType = str(in.get("entryType")).trim();      // txtEntryType.Text.Trim()

        boolean onQty = cfgOnQty(org, comp);
        boolean cGrn = cfg(org, comp, "ContractorWageComparisonbyActivityForGRN");
        boolean cGdn = cfg(org, comp, "ContractorWageComparisonbyActivityForGDN");
        boolean cFwd = cfg(org, comp, "ContractorWageComparisonbyActivityForForwarding");
        boolean cSt = cfg(org, comp, "ContractorWageComparisonbyActivityForStockTransfer");
        boolean cIn = cfg(org, comp, "ContractorWageComparisonbyActivityForProductionInput");
        boolean cCons = cfg(org, comp, "ContractorWageComparisonbyActivityForProductionConsumption");

        /* StichingWagesConfig():442 */
        boolean stichingCompulsory = false;
        if (cmb == 112 || refDocTypeId == 112) stichingCompulsory = cfg(org, comp, "StichingWagesCompulsory");
        else if (cmb == 66 || refDocTypeId == 66) stichingCompulsory = cfg(org, comp, "OtherWagesCompulsoryForStockConversion");

        /* :2734 - note "|| RefDocTypeId == 80 ||" stands outside its configuration guard. */
        if (cmb == 112 || refDocTypeId == 112 || cmb == 66 || refDocTypeId == 66
                || (cGrn && (cmb == 46 || refDocTypeId == 46))
                || (cGdn && (cmb == 86 || refDocTypeId == 86))
                || (cFwd && (cmb == 205 || refDocTypeId == 205))
                || (cSt && (cmb == 68 || refDocTypeId == 68 || cmb == 806 || refDocTypeId == 806))
                || (cIn && cmb == 80) || refDocTypeId == 80
                || (cCons && (cmb == 181 || refDocTypeId == 181))) {
            if (onQty) {
                validationForRefRowQty(regular, "Regular_Wages", accountNames);
                validationForRefRowQty(other, "Other_Wages", accountNames);
            } else {
                validationForRefRowWeight(regular, "Regular_Wages", accountNames);
                validationForRefRowWeight(other, "Other_Wages", accountNames);
            }
        }

        LocalDateTime now = LocalDateTime.now();
        Map<String, Object> h = new LinkedHashMap<>();
        h.put("Id", recId > 0 ? recId : 0);
        h.put("CompanyId", comp);
        h.put("OrganizationId", org);
        h.put("BranchesId", branch());
        h.put("DocNo", toInt(in.get("docNo")));
        h.put("DocDate", dateOf(in.get("docDate")));
        h.put("DocumentTypeId", DOC_TYPE_WAGES);
        h.put("RefDocumentTypeId", cmb);
        h.put("RefDocNoId", toInt(in.get("refDocNoId")));
        h.put("RefDocNo", toInt(in.get("refDocNo")));
        h.put("WeightTotal", toDouble(in.get("grossWeight")));
        h.put("OtherRemarks", str(in.get("remarks")));
        h.put("EntryUser", user);
        h.put("ModifyUser", user);
        h.put("ScaleSlipNo", toInt(str(in.get("gpNo")).trim()));
        h.put("RefDocument", entryType);
        h.put("ModifyDate", now);
        h.put("EntryDate", now);
        h.put("FinancialYearId", year());
        if (toBool(in.get("isApproved"))) {
            h.put("IsAproved", true);
            h.put("ApprovedDate", now);
            h.put("ApprovedUserId", user);
        } else {
            h.put("IsAproved", false);
            h.put("ApprovedUserId", 0);
        }
        h.put("ProjectsId", 0);
        h.put("JobOrderId", 0);
        h.put("StockPartyId", 0);

        List<Map<String, Object>> details = new ArrayList<>();
        if (regular.isEmpty()) throw new WagesValidationException("Regular Wages Grid... Record Not Found");
        for (Map<String, Object> r : regular) {
            Map<String, Object> wd = detailDefaults();
            LocalDateTime rowDate = dateOf(r.get("Date"));
            h.put("DocDate", rowDate);                       // :2786 - every row overwrites DocDate
            wd.put("RefDocDate", rowDate);
            /* :2788 compares DocDate with the same row date - it can never throw. */
            if (toInt(r.get("WagesId")) == 0 || text(r, "WagesId", "WagesText").isEmpty())
                throw new WagesValidationException("WagesAccount Field required In Regular Wages Grid...");
            wd.put("InvConractorWagesAccountsId", toInt(r.get("WagesId")));
            if (toInt(r.get("Quantity")) == 0 || regText(r, "Quantity").isEmpty())
                throw new WagesValidationException("Quantity Field required In Regular Wages Grid...");
            double qty = toDouble(r.get("Quantity"));
            wd.put("Qty", qty);
            wd.put("BillQty", qty);
            if (toDouble(r.get("Weight")) == 0d || regText(r, "Weight").isEmpty())
                throw new WagesValidationException("Weight Field required In Regular Wages Grid...");
            double weight = toDouble(regText(r, "Weight"));
            wd.put("Weight", weight);
            wd.put("BillWeight", toDouble(r.get("Weight")));    // :2809 - the Weight cell, not BillWeight
            if (toDouble(r.get("PackSize")) == 0d || regText(r, "PackSize").isEmpty())
                throw new WagesValidationException("PackSize Field required In Regular Wages Grid...");
            double packSize = toDouble(regText(r, "PackSize"));
            wd.put("PackSize", packSize);
            String wagesType = str(r.get("WagesType"));
            double rate;
            if ("Free Of Cost".equals(wagesType) || "Dryer".equals(str(r.get("WarehouseType")))) {
                wd.put("FreeOfCost", true);
                rate = 0d;
                wd.put("RateAddLess", 0d);
                wd.put("WagesAmount", 0d);
                wd.put("InvContractorWagesScheduleId", 0);
            } else {
                rate = toDouble(regText(r, "Rate"));
                wd.put("InvContractorWagesScheduleId", toInt(r.get("WagesScheduleId")));
                wd.put("RateAddLess", toDouble(regText(r, "RateAddLess")));
                if (rate == 0d) throw new WagesValidationException("Rate Field required In Regular Wages Grid...");
            }
            wd.put("WageRate", rate);
            boolean foc = "Free Of Cost".equals(wagesType);
            wd.put("FreeOfCost", foc);
            if (foc) wd.put("WagesAmount", 0d);
            else if (onQty) wd.put("WagesAmount", round(qty * rate, 2));
            else wd.put("WagesAmount", round(weight / packSize * rate, 2));
            if (toInt(r.get("SupplierId")) == 0 || text(r, "SupplierId", "SupplierText").isEmpty())
                throw new WagesValidationException("ContractorAccount Field required In Regular Wages Grid...");
            wd.put("ContractorId", toInt(r.get("SupplierId")));
            if (toInt(r.get("ItemId")) == 0 || regText(r, "ItemId").isEmpty())
                throw new WagesValidationException("Item Name Field required In Regular Wages Grid...");
            wd.put("ItemId", toInt(regText(r, "ItemId")));
            if (toInt(r.get("PurchaseGLAC")) == 0 || regText(r, "PurchaseGLAC").isEmpty())
                throw new WagesValidationException("PurchaseGLAC Field required In Regular Wages Grid...");
            wd.put("PurchaseGLAC", toInt(regText(r, "PurchaseGLAC")));
            wd.put("ItemName", str(r.get("Item")));
            wd.put("CompanyName", text(r, "SupplierId", "SupplierText"));
            wd.put("WagesAccountName", text(r, "WagesId", "WagesText"));
            wd.put("WagesTypeId", toInt(r.get("WagesType")));    // "Regular" -> 0
            wd.put("JobLotId", toInt(regText(r, "jobLotId")));
            wd.put("Crop", regText(r, "Crop"));
            wd.put("InvPackingTypeId", toInt(regText(r, "packingTypeId")));
            wd.put("WareHouseFromId", toInt(regText(r, "MoveFromId")));
            wd.put("WareHouseToId", toInt(regText(r, "MoveToId")));
            wd.put("BillQty", toDouble(regText(r, "BillQty")));
            wd.put("WeightCut", toDouble(regText(r, "WeightCut")));
            wd.put("RefDocQty", toDouble(regText(r, "RefDocQty")));
            wd.put("RefDocWeight", toDouble(regText(r, "RefDocWeight")));
            wd.put("RefLineId", toInt(regText(r, "RefLineId")));
            details.add(wd);
        }

        /* :2885 */
        if (other.isEmpty() && stichingCompulsory
                && ((cmb == 66 && !"Issue".equals(entryType)) || cmb == 68 || cmb == 112 || cmb == 806)) {
            for (Map<String, Object> r : regular) {
                if (toDouble(regText(r, "PackSize")) < 100d) {
                    throw new WagesValidationException(
                            "Stitching Wages Grid... Record Not Found\n Stitching Wages Compulsory Configuration is On");
                }
            }
        }
        /* :2902 - "(DocType != 68 || DocType != 806)" is always true, so only MoveOrder relaxes a check. */
        boolean checks = !"MoveOrder".equals(str(in.get("entryType")));   // txtEntryType.Text, untrimmed
        String g = "Other Wages Grid ...";
        for (Map<String, Object> r : other) {
            boolean noWages = toInt(r.get("WagesId")) == 0 || text(r, "WagesId", "WagesText").isEmpty();
            boolean noSupplier = toInt(r.get("SupplierId")) == 0 || text(r, "SupplierId", "SupplierText").isEmpty();
            if ("Issue".equals(entryType) || (!stichingCompulsory && noWages && noSupplier)) continue;
            Map<String, Object> wd = detailDefaults();
            if (noWages && checks) throw new WagesValidationException("WagesAccount Field required In " + g);
            wd.put("InvConractorWagesAccountsId", toInt(r.get("WagesId")));
            if ((toInt(r.get("Quantity")) == 0 || rawText(r, "Quantity").isEmpty()) && checks)
                throw new WagesValidationException("Quantity Field required In " + g);
            double qty = toDouble(r.get("Quantity"));
            wd.put("Qty", qty);
            wd.put("BillQty", qty);
            if ((toDouble(r.get("Weight")) == 0d || rawText(r, "Weight").isEmpty()) && checks)
                throw new WagesValidationException("Weight Field required In " + g);
            double weight = toDouble(rawText(r, "Weight"));
            wd.put("Weight", weight);
            wd.put("BillWeight", toDouble(r.get("Weight")));
            if ((toDouble(r.get("PackSize")) == 0d || rawText(r, "PackSize").isEmpty()) && checks)
                throw new WagesValidationException("PackSize Field Required In " + g);
            double packSize = toDouble(rawText(r, "PackSize"));
            wd.put("PackSize", packSize);
            if ("Free Of Cost".equals(str(r.get("WagesType"))) || "Dryer".equals(str(r.get("WarehouseType")))) {
                wd.put("FreeOfCost", true);
                wd.put("WageRate", 0d);
                wd.put("RateAddLess", 0d);
                wd.put("WagesAmount", 0d);
                wd.put("InvContractorWagesScheduleId", 0);
            } else {
                double rate = toDouble(rawText(r, "Rate"));
                wd.put("WageRate", rate);
                wd.put("InvContractorWagesScheduleId", toInt(r.get("WagesScheduleId")));
                wd.put("RateAddLess", toDouble(rawText(r, "RateAddLess")));
                if (rate == 0d) throw new WagesValidationException("Rate Field required In " + g);
                wd.put("WagesAmount", onQty ? round(qty * rate, 2) : round(weight / packSize * rate, 2));
            }
            if (noSupplier && checks) throw new WagesValidationException("ContractorAccount Field required In " + g);
            wd.put("ContractorId", toInt(r.get("SupplierId")));
            if ((toInt(r.get("ItemId")) == 0 || rawText(r, "ItemId").isEmpty()) && checks)
                throw new WagesValidationException("Item Name Field required In " + g);
            wd.put("ItemId", toInt(rawText(r, "ItemId")));
            if ((toInt(r.get("PurchaseGLAC")) == 0 || rawText(r, "PurchaseGLAC").isEmpty()) && checks)
                throw new WagesValidationException("PurchaseGLAC Field required In " + g);
            wd.put("PurchaseGLAC", toInt(rawText(r, "PurchaseGLAC")));
            wd.put("ItemName", str(r.get("Item")));
            wd.put("CompanyName", text(r, "SupplierId", "SupplierText"));
            wd.put("WagesAccountName", text(r, "WagesId", "WagesText"));
            wd.put("RefDocDate", dateOf(r.get("Date")));
            wd.put("JobLotId", toInt(rawText(r, "jobLotId")));
            wd.put("Crop", rawText(r, "Crop"));
            wd.put("InvPackingTypeId", toInt(rawText(r, "packingTypeId")));
            wd.put("WareHouseFromId", toInt(rawText(r, "MoveFromId")));
            wd.put("WareHouseToId", toInt(rawText(r, "MoveToId")));
            wd.put("BillQty", (double) toInt(rawText(r, "BillQty")));   // :2987 - Conversion.ToInt
            wd.put("WeightCut", toDouble(rawText(r, "WeightCut")));
            wd.put("RefDocQty", toDouble(rawText(r, "RefDocQty")));
            wd.put("RefDocWeight", toDouble(rawText(r, "RefDocWeight")));
            wd.put("RefLineId", toInt(rawText(r, "RefLineId")));
            wd.put("FreeOfCost", "Free Of Cost".equals(str(r.get("WagesType"))));
            wd.put("WagesTypeId", 2);
            details.add(wd);
        }
        h.put("QtyTotal", toDouble(in.get("qty")));
        h.put("WeightTotal", toDouble(in.get("grossWeight")));

        /* BLL Save: MakeVoucher first (outside the DAL transaction), then SetData. */
        Map<String, Object> voucher = makeVoucher(h, details, org, comp);
        final boolean insert = toInt(h.get("Id")) == 0;
        Integer id = new TransactionTemplate(txManager).execute(s -> setData(h, details, voucher,
                insert ? "Sp_InvContractorWagesBillHeader_Insert" : "Sp_InvContractorWagesBillHeader_Update"));

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", id);
        out.put("docNo", h.get("DocNo"));
        /* :3009 / :3013 - two blanks before the bracket. */
        out.put("message", (recId > 0 ? "Updated SuccessFully  [" : "Saved SuccessFully  [") + h.get("DocNo") + "]");
        return out;
    }

    // ------------------------------------------------------------------ reference-row validations

    /** ValidationforRefRowWeight():2634 - group by (WagesId, RefLineId, RefDocWeight), sum BillWeight. */
    private void validationForRefRowWeight(List<Map<String, Object>> rows, String gridName, Map<String, Object> names) {
        Map<String, double[]> groups = new LinkedHashMap<>();
        for (Map<String, Object> r : rows) {
            int w = toInt(r.get("WagesId")), l = toInt(r.get("RefLineId"));
            double ref = toDouble(r.get("RefDocWeight"));
            String k = w + "|" + l + "|" + ref;
            double[] a = groups.computeIfAbsent(k, x -> new double[]{w, l, ref, 0d});
            a[3] += toDouble(r.get("BillWeight"));
        }
        for (double[] a : groups.values()) {
            if (a[3] > a[2]) {
                throw new WagesValidationException(
                        "TotalBillWeight against Reference RowNo and Account Should be Equal to or less than Reference Row Weight\n"
                      + "Here TotalBillWeight (" + netG(a[3]) + ") exceeds Reference Row Weight (" + netG(a[2])
                      + ") for WagesAccount (" + name(names, (int) a[0]) + ") and RowNo " + (int) a[1] + "  in "
                      + gridName + " Grid");
            }
        }
    }

    /** ValidationforRefRowQty():2668 - group by (WagesId, RefLineId, RefDocQty), sum Quantity. */
    private void validationForRefRowQty(List<Map<String, Object>> rows, String gridName, Map<String, Object> names) {
        Map<String, double[]> groups = new LinkedHashMap<>();
        for (Map<String, Object> r : rows) {
            int w = toInt(r.get("WagesId")), l = toInt(r.get("RefLineId"));
            double ref = toDouble(r.get("RefDocQty"));
            String k = w + "|" + l + "|" + ref;
            double[] a = groups.computeIfAbsent(k, x -> new double[]{w, l, ref, 0d});
            a[3] += toDouble(r.get("Quantity"));
        }
        for (double[] a : groups.values()) {
            if (a[3] > a[2]) {
                throw new WagesValidationException(
                        "TotalQty against Reference RowNo and Wages Account Should be Equal to or less than Reference Row Qty\n"
                      + "Here TotalQty (" + netG(a[3]) + ") exceeds Reference Row Qty (" + netG(a[2])
                      + ") for WagesAccount (" + name(names, (int) a[0]) + ") and RowNo " + (int) a[1] + " in "
                      + gridName + " Grid");
            }
        }
    }

    /** dtwagesacc name lookup - "" when the id is not in the list (?? ""). */
    private static String name(Map<String, Object> names, int id) {
        Object v = names == null ? null : names.get(String.valueOf(id));
        return v == null ? "" : String.valueOf(v);
    }

    // ================================================================================ MakeVoucher

    /**
     * BLL InvContractorWagesBillHeader.MakeVoucher (IL 0x0000-0x104c), the DocumentTypeId 101 path.
     * Two lines per charged detail row: Dr debit account / Cr contractor, and the reverse.
     */
    private Map<String, Object> makeVoucher(Map<String, Object> h, List<Map<String, Object>> details, int org, int comp) {
        Map<String, Object> vh = voucherHeadDefaults();
        vh.put("DocumentTypeId", h.get("DocumentTypeId"));
        vh.put("DocumentTypeSrNo", h.get("Id"));
        vh.put("RefDocNoId", h.get("Id"));                         // 0 on insert - obj.Id is not known yet
        vh.put("VoucherCode", toInt(h.get("DocNo")));
        vh.put("VoucherDate", h.get("DocDate"));
        vh.put("Remarks", str(h.get("OtherRemarks")));
        vh.put("RemarksOtherLingo", "");
        vh.put("ChequeDate", LocalDate.now().atStartOfDay());      // new DateTime?(DateTime.Today)
        vh.put("IncludeWHT", false);
        vh.put("BranchId", h.get("BranchesId"));
        vh.put("ProjectId", h.get("ProjectsId"));
        vh.put("BillAmount", 0d);
        vh.put("ManualBillNo", String.valueOf(toInt(h.get("RefDocNo"))));
        vh.put("DueDate", h.get("DocDate"));
        vh.put("DueDays", 0);
        vh.put("OrganizationId", h.get("OrganizationId"));
        vh.put("CompanyId", h.get("CompanyId"));
        vh.put("FinancialYearId", h.get("FinancialYearId"));
        vh.put("EntryUser", h.get("EntryUser"));
        vh.put("EntryDate", LocalDateTime.now());
        vh.put("ModifyDate", LocalDateTime.now());
        vh.put("ModifyUser", h.get("ModifyUser"));
        List<Map<String, Object>> lines = new ArrayList<>();
        vh.put("voucherDetailList", lines);

        List<Map<String, Object>> suppliers = repo.supplierCustomerGlAccounts(org, comp);
        List<Map<String, Object>> items = new ArrayList<>();
        List<Map<String, Object>> wagesAccounts = null;
        boolean wip = false, chargeToProduct = false, forwarding = false;
        int docType = toInt(h.get("DocumentTypeId"));
        int refType = toInt(h.get("RefDocumentTypeId"));
        if (docType != 219 && (refType == 80 || refType == 112)) {
            wip = toBool(repo.config(org, comp, "ProductionWagesChargeToWIPAccount"));
        } else if (docType == 101 && refType == 205) {
            forwarding = toBool(repo.config(org, comp, "LabourWagesChargedtoContractAccountonForwarding"));
        } else if (docType == 101 && (refType == 46 || refType == 68 || refType == 806)) {
            items = repo.itemGlIdsAndItemName(org, comp);
            chargeToProduct = toBool(repo.config(org, comp, "ContractWagesChargetoProduct"));
        }
        if ((docType == 101 || docType == 810) && !details.isEmpty()) {
            for (Map<String, Object> item : details) {
                if (toBool(item.get("FreeOfCost"))) continue;
                int debit = 0;
                if (wip && (refType == 80 || refType == 112 || refType == 181)) {
                    if (docType == 101) {
                        List<Map<String, Object>> t = repo.wipAccountsByProductionId(org, comp, toInt(h.get("RefDocNoId")));
                        if (t == null || t.isEmpty()) throw new WagesValidationException("Work in process Account not found against JobOrder");
                        debit = toInt(ci(t.get(0), "WIPAccountId"));
                    } else {
                        List<Map<String, Object>> t = repo.glAccountsByJobOrderId(org, comp, toInt(h.get("JobOrderId")));
                        if (t == null || t.isEmpty()) throw new WagesValidationException("Work in process Account not found against JobOrder");
                        debit = toInt(ci(t.get(0), "WorkInProccessAcId"));
                    }
                }
                if (chargeToProduct) {
                    Map<String, Object> it = firstById(items, toInt(item.get("ItemId")));
                    if (it == null) throw new WagesValidationException("ItemId Not Found");
                    debit = toInt(ci(it, "PurchaseGLAC"));
                }
                if (forwarding) {
                    List<Map<String, Object>> t = repo.exportInvoiceDataByForwardingId(org, comp, toInt(h.get("RefDocNoId")));
                    if (t == null || t.isEmpty()) throw new WagesValidationException("Contract Credit Account Not found against Forwarding");
                    debit = toInt(ci(t.get(0), "ContractGLAcId"));
                }
                if (debit == 0) {
                    if (wagesAccounts == null || wagesAccounts.isEmpty()) wagesAccounts = repo.contractorWagesAccountsAll(org, comp);
                    if (wagesAccounts.isEmpty()) throw new WagesValidationException("Contactor Wages Account Id Not Found");
                    Map<String, Object> wa = firstById(wagesAccounts, toInt(item.get("InvConractorWagesAccountsId")));
                    if (wa == null) throw new WagesValidationException("Contactor Wages Account Id Not Found");
                    debit = toInt(ci(wa, "GlAccountId"));
                }
                if (debit == 0) throw new WagesValidationException("Debit AccountId Not Found");
                Map<String, Object> sup = firstById(suppliers, toInt(item.get("ContractorId")));
                if (sup == null) continue;                            // no contractor row - no lines
                /* the 810 stock-party override (IsCompany) does not apply to 101 */
                int supplierGl = toInt(ci(sup, "GlAccountId"));
                String comments = str(h.get("RefDocument")) + "  :" + str(item.get("WagesAccountName"))
                        + " Item Name :" + str(item.get("ItemName"))
                        + "  Wages Qty :  " + netG(toDouble(item.get("Qty")))
                        + "  Wages Rate :  " + netG(toDouble(item.get("WageRate")))
                        + "  Wages Amount :  " + netG(toDouble(item.get("WagesAmount")))
                        + "  BillWeight :  " + netG(toDouble(item.get("BillWeight")));
                boolean foc = toBool(item.get("FreeOfCost"));
                double amount = foc ? 0d : toDouble(item.get("WagesAmount"));
                Map<String, Object> dr = voucherDetailDefaults();
                dr.put("AccountId", debit);
                dr.put("AgainstAccountId", supplierGl);
                dr.put("Comments", comments);
                dr.put("DebitAmount", amount);
                dr.put("ItemAmount", amount);
                dr.put("CreditAmount", 0d);
                dr.put("ItemRate", toDouble(item.get("WageRate")));
                dr.put("OrderNo", h.get("JobOrderId"));
                lines.add(dr);
                Map<String, Object> cr = voucherDetailDefaults();
                cr.put("AccountId", supplierGl);
                cr.put("AgainstAccountId", debit);
                cr.put("Comments", comments);
                cr.put("DebitAmount", 0d);
                cr.put("CreditAmount", amount);
                cr.put("ItemAmount", amount);
                cr.put("ItemRate", toDouble(item.get("WageRate")));
                cr.put("OrderNo", h.get("JobOrderId"));
                lines.add(cr);
            }
        }
        return vh;
    }

    private static Map<String, Object> firstById(List<Map<String, Object>> rows, int id) {
        for (Map<String, Object> r : rows) if (toInt(ci(r, "Id")) == id) return r;
        return null;
    }

    // =================================================================================== SetData

    /**
     * DAL InvContractorWagesBillHeader.SetData (IL 0x0000-0x05e9), inside one transaction:
     * header Insert/Update, every detail, then either the voucher (lookup, head Insert/Update,
     * details, USP_VoucherBalanceCheck, the _H_ history copies) or - when there are no voucher
     * lines - usp_ContractorWagesVoucherDeleteByWagesId; last, usp_WagesProportionateToStockEvaluation
     * for 68 / 806.
     */
    @SuppressWarnings("unchecked")
    private Integer setData(Map<String, Object> h, List<Map<String, Object>> details, Map<String, Object> vh, String proc) {
        int l2 = repo.setProc(proc, headerParams(h));
        if (l2 > 0) h.put("Id", l2);
        else l2 = toInt(h.get("Id"));
        for (Map<String, Object> d : details) {
            d.put("InvContractorWagesBillHeaderId", h.get("Id"));
            repo.setProc("Sp_InvContractorWagesBillDetail_Insert", detailParams(d));
        }
        int org = toInt(h.get("OrganizationId")), comp = toInt(h.get("CompanyId"));
        List<Map<String, Object>> lines = vh == null ? null : (List<Map<String, Object>>) vh.get("voucherDetailList");
        if (lines != null && !lines.isEmpty()) {
            int l0;
            int existing = repo.voucherHeadId(org, comp, toInt(h.get("DocumentTypeId")), toInt(h.get("Id")));
            if (existing != 0) vh.put("Id", existing);
            vh.put("DocumentTypeSrNo", h.get("Id"));
            l0 = repo.setProc(existing == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", voucherHeadParams(vh));
            if (l0 > 0) vh.put("Id", l0);
            for (Map<String, Object> vd : lines) {
                vd.put("VoucherHeadId", vh.get("Id"));
                repo.setProc("Sp_VoucherDetail_Insert", voucherDetailParams(vd));
            }
            Map<String, Object> p = ProductionWagesBillRepository.params();
            p.put("@OrganizationId", org);
            p.put("@CompanyId", comp);
            p.put("@Id", vh.get("Id"));
            repo.command("USP_VoucherBalanceCheck", p);
            int hId = repo.setProc("Sp_VoucherHead_H_Insert", voucherHeadParams(vh));
            for (Map<String, Object> vd : lines) {
                vd.put("VoucherHeadId", vh.get("Id"));
                vd.put("DocumentTypeIdRef", hId);
                repo.setProc("Sp_VoucherDetail_H_Insert", voucherDetailParams(vd));
            }
        } else {
            Map<String, Object> p = ProductionWagesBillRepository.params();
            p.put("@OrganizationId", org);
            p.put("@CompanyId", comp);
            p.put("@DocumentTypeId", h.get("DocumentTypeId"));
            p.put("@Id", h.get("Id"));
            repo.command("usp_ContractorWagesVoucherDeleteByWagesId", p);
        }
        int refType = toInt(h.get("RefDocumentTypeId"));
        if (refType == 806 || refType == 68) {
            Map<String, Object> p = ProductionWagesBillRepository.params();
            p.put("@OrganizationId", org);
            p.put("@CompanyId", comp);
            p.put("@RefDocumentTypeId", refType);
            p.put("@RefDocNoId", h.get("RefDocNoId"));
            repo.command("[dbo].[usp_WagesProportionateToStockEvaluation]", p);
        }
        return l2;
    }

    /** Sp_InvContractorWagesBillHeader_Insert / _Update: the model properties the procedure declares. */
    private static Map<String, Object> headerParams(Map<String, Object> h) {
        return pick(h, new String[]{"Id", "DocumentTypeId", "DocNo", "DocDate", "RefDocumentTypeId", "RefDocNoId",
                "RefDocNo", "WeightTotal", "QtyTotal", "ScaleSlipNo", "OtherRemarks", "EntryDate", "EntryUser",
                "ModifyDate", "ModifyUser", "IsAproved", "ApprovedDate", "ApprovedUserId", "OrganizationId",
                "CompanyId", "BranchesId", "ProjectsId", "RefDocument", "FinancialYearId", "JobOrderId",
                "StockPartyId"});
    }

    /** InvContractorWagesBillDetail value-type defaults (the model has no constructor body). */
    private static Map<String, Object> detailDefaults() {
        Map<String, Object> d = new LinkedHashMap<>();
        for (String k : new String[]{"Id", "InvContractorWagesBillHeaderId", "ContractorId", "ItemId", "JobLotId",
                "InvPackingTypeId", "WbTransactionsIdDt", "InvConractorWagesAccountsId", "WareHouseFromId",
                "WareHouseToId", "WagesTypeId", "RefDocumentTypeId", "JobOrderId", "RefLineId",
                "InvContractorWagesScheduleId", "PurchaseGLAC"}) d.put(k, 0);
        for (String k : new String[]{"Weight", "PackSize", "Qty", "WageRate", "WagesAmount", "BillQty", "WeightCut",
                "BillWeight", "RateAddLess", "RefDocQty", "RefDocWeight"}) d.put(k, 0d);
        d.put("FreeOfCost", false);
        d.put("IsCompany", false);
        return d;
    }

    private static Map<String, Object> detailParams(Map<String, Object> d) {
        return pick(d, new String[]{"Id", "InvContractorWagesBillHeaderId", "ContractorId", "ItemId", "Crop",
                "JobLotId", "InvPackingTypeId", "WbTransactionsIdDt", "InvConractorWagesAccountsId", "Weight",
                "PackSize", "Qty", "WageRate", "WagesAmount", "RemarksDetail", "WareHouseFromId", "WareHouseToId",
                "BillQty", "WeightCut", "BillWeight", "RefDocDate", "WagesTypeId", "FreeOfCost", "RefDocumentTypeId",
                "JobOrderId", "IsCompany", "RateAddLess", "RefDocQty", "RefDocWeight", "RefLineId",
                "InvContractorWagesScheduleId"});
    }

    private static Map<String, Object> voucherHeadDefaults() {
        Map<String, Object> v = new LinkedHashMap<>();
        for (String k : new String[]{"IncludeWHT", "IsApproved", "PostState", "InclusiveTax", "IsUploaded", "CustomAccounts"}) v.put(k, false);
        for (String k : new String[]{"BillAmount", "ExchangeCurrencyRate", "FcAmount", "VoucherAmount", "CostCenterAmount",
                "AdvanceTaxAmount", "OtherChargesAmount"}) v.put(k, 0d);
        for (String k : new String[]{"AgainstAccountId", "BranchId", "CheqId", "ChequePrintId", "CompanyId", "DocumentTypeId",
                "DocumentTypeSrNo", "DueDays", "EntryUser", "FinancialYearId", "Id", "ModifyUser", "MultiCurrencyId",
                "OrganizationId", "PostUser", "ProjectId", "RefAccountId", "RefDocNoId", "VoucherCode", "ActionId",
                "RefDocumentTypeId", "FixedAssetEntryTypeId", "BaseDocumentTypeId", "AdvanceTaxAccountId",
                "OtherChargesAccountId"}) v.put(k, 0);
        return v;
    }

    private static final String[] VH = {"Id", "DocumentTypeId", "DocumentTypeSrNo", "RefDocNoId", "VoucherCode",
            "VoucherDate", "Remarks", "RemarksOtherLingo", "VoucherAmount", "FinancialYearId", "RefAccountId",
            "AgainstAccountId", "MultiCurrencyId", "ConversionFormula", "ExchangeCurrencyRate", "FcAmount", "CheqId",
            "ChequeNo", "ChequeDate", "PayTitle", "BankBranch", "ChequePrintId", "Source", "DrCrNoteType", "IsApproved",
            "EntryDate", "EntryUser", "ModifyDate", "ModifyUser", "PostDate", "PostUser", "PostState", "OrganizationId",
            "CompanyId", "IncludeWHT", "BranchId", "ProjectId", "ManualBillNo", "BillAmount", "DueDate", "DueDays",
            "ActionId", "CostCenterAmount", "AttachmentsValues", "RefDocumentTypeId", "CustomAttachmentsValues",
            "CustomAccounts", "BaseDocumentTypeId", "AdvanceTaxAccountId", "AdvanceTaxAmount", "OtherChargesAccountId",
            "OtherChargesAmount", "IsUploaded", "InclusiveTax", "FixedAssetEntryTypeId"};

    private static Map<String, Object> voucherHeadParams(Map<String, Object> v) { return pick(v, VH); }

    private static Map<String, Object> voucherDetailDefaults() {
        Map<String, Object> v = new LinkedHashMap<>();
        for (String k : new String[]{"Adjustment", "AdvanceAmount", "Commission", "CreditAmount", "DCurrencyAmount",
                "DebitAmount", "TaxAmount", "DExchangeCurrencyRate", "Expenses", "ExTax", "Freight", "ItemAmount", "ItemRate",
                "Journal", "QtyIn", "QtyOut", "RateCut", "RateCutAmount", "SaleTax", "TaxesTotalAmount", "TaxPrcnt",
                "WeightIn", "WeightOut", "WhtHolding", "ItemCgsRate", "TotalDebitAmount", "TotalCreditAmount",
                "ThirdCurrencyFcyExchangeRate", "ThirdCurrencyHcyExchangeRate", "ThirdCurrencyAmount",
                "ThirdCurrencyReceiverExchangeRate", "ThirdCurrencyReceiverFcyAmount", "SBRTaxAmount", "DiscountPercent",
                "DiscountAmount", "BaseFcyExchangeRate", "BaseFcyAmount"}) v.put(k, 0d);
        for (String k : new String[]{"ThirdCurrencyId", "AccountId", "AgainstAccountId", "DMultiCurrencyId", "DocumentTypeIdRef",
                "GpNo", "Id", "InvoiceNoRefId", "ItemId", "JobLotId", "OrderNo", "SupplierCustomerId", "EmployeeId",
                "SubsidiaryTypeId", "SubsidiaryAccountId", "SubsidiaryAgainstTypeId", "SubsidiaryAgainstAccountId",
                "TaxTypeId", "ActionId", "VoucherHeadId", "RefDocumentTypeId", "RefDocNoId", "RefDocNoDetailId",
                "RefDocSubIdNo", "LineId", "InstrumentTypeId", "SubNo", "SortNo", "IsCGS", "PaymentTypeId", "ChequeTypeId",
                "BranchesId", "CostCenterId", "ReferenceAccountId", "LocationTypeId", "BaseFcyId"}) v.put(k, 0);
        return v;
    }

    private static final String[] VD = {"Id", "VoucherHeadId", "AccountId", "AgainstAccountId", "Comments",
            "CommentsOtherLingo", "DebitAmount", "CreditAmount", "JobLotId", "RefInvoiceNo", "TaxesTotalAmount",
            "TaxesRemarks", "IsTaxable", "TaxTypeId", "TaxPrcnt", "DCheqDate", "CheqNoDetail", "DocumentTypeIdRef",
            "InvoiceNoRefId", "ItemId", "OrderNo", "GpNo", "VehicleNo", "GpDate", "QtyIn", "QtyOut", "WeightIn",
            "WeightOut", "SupplierCustomerId", "ItemRate", "RateCut", "RateCutAmount", "ItemAmount", "Expenses", "Freight",
            "Journal", "Commission", "DMultiCurrencyId", "DConversionFormula", "DExchangeCurrencyRate", "DCurrencyAmount",
            "PaymentType", "AdvanceAmount", "WhtHolding", "SaleTax", "ExTax", "Adjustment", "ActionId", "LineId",
            "RefDocumentTypeId", "RefDocNoId", "RefDocNoDetailId", "RefDocSubIdNo", "ItemCgsRate", "TotalCreditAmount",
            "TotalDebitAmount", "SubNo", "PayeeTitle", "SubsidiaryTypeId", "EmployeeId", "SubsidiaryAccountId", "IsCGS",
            "SubsidiaryAgainstTypeId", "SubsidiaryAgainstAccountId", "ThirdCurrencyId", "ThirdCurrencyFcyExchangeRate",
            "ThirdCurrencyHcyExchangeRate", "ThirdCurrencyAmount", "ThirdCurrencyReceiverExchangeRate",
            "ThirdCurrencyReceiverFcyAmount", "SortNo", "InstrumentTypeId", "ChequeTypeId", "BranchesId", "CostCenterId",
            "SBRTaxAmount", "DiscountPercent", "DiscountAmount", "ReferenceAccountId", "LocationTypeId", "PaymentTypeId",
            "TaxAmount", "BaseFcyId", "BaseFcyExchangeRate", "BaseFcyAmount"};

    private static Map<String, Object> voucherDetailParams(Map<String, Object> v) { return pick(v, VD); }

    /** "@Name" -> value for each listed property that has a value (a null is not sent). */
    private static Map<String, Object> pick(Map<String, Object> src, String[] names) {
        Map<String, Object> p = ProductionWagesBillRepository.params();
        for (String n : names) {
            Object v = src.get(n);
            if (v != null) p.put("@" + n, v);
        }
        return p;
    }

    // =================================================================================== helpers

    private boolean cfg(int org, int comp, String name) { return toBool(repo.config(org, comp, name)); }

    private boolean cfgOnQty(int org, int comp) {
        String q = repo.config(org, comp, "WagesAmountCalculateOnQty");
        return !q.isEmpty() && toBool(q);
    }

    /**
     * A Regular Wages cell's .Text. DetailGridSettings() gives "#,##0.####" to Quantity, BillQty,
     * Weight, BillWeight, RateAddLess, Amount and Rate/RateWithoutAddLess/WeightCut too, but only the
     * typed (double) DataColumns of dtdetail (Load:354-386) are IFormattable - Rate,
     * RateWithoutAddLess, WeightCut and PackSize are untyped (string) columns, so Janus shows their
     * text unformatted.
     */
    private static String regText(Map<String, Object> r, String key) {
        Object v = r.get(key);
        if (v == null || str(v).isEmpty()) return "";
        switch (key) {
            case "Quantity": case "BillQty": case "Weight": case "BillWeight": case "RateAddLess": case "Amount":
                return fmt(toDouble(v), 4);                          // "#,##0.####"
            default:
                return rawText(r, key);
        }
    }

    /** A cell's .Text with no FormatString - the value's ToString(). */
    private static String rawText(Map<String, Object> r, String key) {
        Object v = r.get(key);
        if (v == null) return "";
        if (v instanceof Number) {
            double d = ((Number) v).doubleValue();
            if (v instanceof Integer || v instanceof Long) return String.valueOf(((Number) v).longValue());
            return netG(d);
        }
        return String.valueOf(v);
    }

    /** A value-list column's .Text: the display text the page read from the list. */
    private static String text(Map<String, Object> r, String key, String textKey) {
        Object t = r.get(textKey);
        if (t != null) return String.valueOf(t);
        return rawText(r, key);
    }

    /** .NET custom format "#,##0.###[#]" then read back - rounds half away from zero. */
    private static String fmt(double d, int dec) {
        BigDecimal b = BigDecimal.valueOf(d).setScale(dec, RoundingMode.HALF_UP).stripTrailingZeros();
        return b.toPlainString();
    }

    /** Math.Round(x, digits) - .NET rounds half to even. */
    static double round(double x, int digits) {
        if (Double.isNaN(x) || Double.isInfinite(x)) return x;
        return new BigDecimal(x).setScale(digits, RoundingMode.HALF_EVEN).doubleValue();
    }

    /** .NET Framework double.ToString() ("G", 15 significant digits). */
    static String netG(double d) {
        if (Double.isNaN(d)) return "NaN";
        if (Double.isInfinite(d)) return d > 0 ? "Infinity" : "-Infinity";
        if (d == 0d) return "0";
        BigDecimal b = new BigDecimal(d).round(new MathContext(15, RoundingMode.HALF_EVEN)).stripTrailingZeros();
        double a = Math.abs(d);
        if (a >= 1e15 || a < 1e-5) {
            String e = String.format(Locale.ROOT, "%.14E", d);
            String[] parts = e.split("E");
            String mant = parts[0].contains(".") ? parts[0].replaceAll("0+$", "").replaceAll("\\.$", "") : parts[0];
            int exp = Integer.parseInt(parts[1]);
            return mant + "E" + (exp < 0 ? "-" : "+") + String.format(Locale.ROOT, "%02d", Math.abs(exp));
        }
        return b.toPlainString();
    }

    private static final DateTimeFormatter DMY = DateTimeFormatter.ofPattern("dd-MMM-yyyy", Locale.ENGLISH);

    /** Conversion.ToDateTime of a cell: "dd-MMM-yyyy" (as the grid stores it) or ISO. */
    static LocalDateTime dateOf(Object v) {
        if (v == null) return LocalDate.now().atStartOfDay();
        if (v instanceof LocalDateTime) return (LocalDateTime) v;
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return LocalDate.now().atStartOfDay();
        try {
            if (s.matches("\\d{1,2}-[A-Za-z]{3}-\\d{4}")) {
                String t = s.length() == 10 ? s : "0" + s;
                t = t.substring(0, 3) + t.substring(3, 4).toUpperCase(Locale.ROOT) + t.substring(4, 6).toLowerCase(Locale.ROOT) + t.substring(6);
                return LocalDate.parse(t, DMY).atStartOfDay();
            }
            if (s.length() <= 10) return LocalDate.parse(s).atTime(LocalTime.MIDNIGHT);
            return LocalDateTime.parse(s.replace(' ', 'T').replaceAll("Z$", "").replaceAll("\\.\\d+$", ""));
        } catch (Exception e) {
            throw new IllegalArgumentException("Invalid date: " + s);
        }
    }

    private static LocalDateTime dateOrNull(String s) {
        if (s == null || s.trim().isEmpty()) return null;
        return dateOf(s);
    }
}
