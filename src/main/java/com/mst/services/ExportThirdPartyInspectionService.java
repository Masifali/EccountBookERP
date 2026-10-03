package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportThirdPartyInspectionRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.time.format.DateTimeParseException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.ExportGdBreakUpRepository.ci;

/**
 * The BLL side of two desktop forms that edit ONE record (InvLabPreProductionExportLotInspectionHeader):
 *
 *   857 frmThirdPartyInspection             "Third Party Inspection"              (request side - Export dept.)
 *   858 frmLabAgainstThirdPartyInspection   "Lab Against Third Party Inspection"  (lab side - sampling)
 *
 * The screen id passed by the controller picks the desktop form's ScreenName (sent as @ScreenName -
 * the procedures branch on it: history dates by SampleTakenDate / Lab* user columns for the lab form)
 * and the form's own validations / save shape:
 *
 *   857: FormValidation incl. Result Status; RemoveDetailIds (saved sub-lots removed with X) sent to
 *        usp_..._DetailDeletebyIds; detail rows keep their Id; sampling SealNo is NOT copied into the
 *        model (desktop quirk - vd3.SealNo never set, so @SealNo is omitted); JobLotId always sent;
 *        PlaceOfInspection = Country Of Inspection text only when a country is chosen, else "";
 *        "Detail MTon (x) cannot be greater than Header MTon (y)." check.
 *   858: FormValidation without Result Status but with "'Stock Reserved Date' or  'Sample Taken Date' or
 *        'Stock Sealed Date' is Required"; RemoveDetailIds is passed as null (no delete call); SealNo sent;
 *        JobLotId only when > 0 (0 either way - the model default); PlaceOfInspection = combo text always;
 *        no detail-vs-header MTon check.
 *
 * Desktop quirks kept (documented here, not fixed):
 *   - Header InsertAndUpdate receives ReportDocNo = 0 and RevisionNo = 0 on every save (model defaults);
 *     on update the procedure writes them as sent.
 *   - Sp_InvLabGroupAnalysisStandards_GetAllMethod 'GetParametersFromGroupStandards' compares
 *     gs.CompanyId to @OrganizationId and gs.OrganizationId to @CompanyId (procedure's own text).
 *   - GetByID takes [0] of the header list: no row -> "Index was out of range..." as on the desktop.
 *
 * Rights: DesktopReportRights with the ScreenDefinition id (857 / 858); View to load, Save / Update /
 * Print as formrights; CanViewAllRecord for the History grid. Tenancy, financial year, branch and user
 * come from the session only.
 */
@Service
public class ExportThirdPartyInspectionService {

    public static final int SCREEN_TPI = 857;
    public static final int SCREEN_LAB = 858;

    @Autowired private ExportThirdPartyInspectionRepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private DesktopReportRights rights;

    private static String screenName(int screen) {
        return screen == SCREEN_LAB ? "frmLabAgainstThirdPartyInspection" : "frmThirdPartyInspection";
    }

    private static int check(int screen) {
        if (screen != SCREEN_TPI && screen != SCREEN_LAB) throw new IllegalArgumentException("Unknown screen");
        return screen;
    }

    private UserAccount user(int screen, String action) {
        UserAccount u = currentUserContext.requireAccountingUser();
        rights.require(u, check(screen), action);
        return u;
    }

    private boolean allowed(UserAccount u, int screen, String action) {
        try { rights.require(u, screen, action); return true; }
        catch (AccessDeniedException ex) { return false; }
    }

    private int fy() { return currentUserContext.currentFinancialYearId(); }

    // ================================================================= load

    /** InitializeComponentMethod - every DbCall of the Task.Run block, bound as the ContinueWith does. */
    public Map<String, Object> setup(int screen) {
        UserAccount u = user(screen, "View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, screen, "Save"));
        perm.put("Update", allowed(u, screen, "Update"));
        perm.put("Print", allowed(u, screen, "Print"));
        perm.put("CanViewAllRecord", allowed(u, screen, "CanViewAllRecord"));
        out.put("permissions", perm);
        out.put("screenName", screenName(screen));
        int days = asInt(repo.config(u, "DefaultDaysToLessFromHistoryFromDate"));
        out.put("defaultDaysToLessFromHistoryFromDate", days);
        out.put("defaultExportInspectionCountryOfOriginId", asInt(repo.config(u, "DefaultExportInspectionCountryOfOriginId")));
        out.putAll(combos(u, screen));
        out.put("trackingNos", trackingNos(u, screen, "not"));
        out.putAll(historyCombos(u));
        List<Map<String, Object>> mu = repo.mostUsed(u, fy());
        out.put("mostUsedRequestedById", mu.isEmpty() ? 0 : asInt(ci(mu.get(0), "RequestedById")));
        out.put("mostUsedSampleHandedOverId", mu.isEmpty() ? 0 : asInt(ci(mu.get(0), "SampleHandedOverId")));
        return out;
    }

    /** btnRefresh_Click: the lookups, customers (857), countries, analysis groups, farming n trade, job lots, items. */
    public Map<String, Object> refresh(int screen) {
        UserAccount u = user(screen, "View");
        return combos(u, screen);
    }

    private Map<String, Object> combos(UserAccount u, int screen) {
        Map<String, Object> out = new LinkedHashMap<>();
        /* ExportComboBindFromLookup: ExImLookUptypesId 8 Requested By (also Sample Handed Over), 11 Medium,
           12 Inspection Agency, 13 Sampling Responsibility, 14 Result Status, 15 Result Remarks. */
        List<Map<String, Object>> t8 = new ArrayList<>(), t11 = new ArrayList<>(), t12 = new ArrayList<>(),
                t13 = new ArrayList<>(), t14 = new ArrayList<>(), t15 = new ArrayList<>();
        for (Map<String, Object> r : repo.lookups(u)) {
            Map<String, Object> m = idName(asInt(ci(r, "Id")), text(ci(r, "LookUpName")));
            switch (asInt(ci(r, "ExImLookUptypesId"))) {
                case 8: t8.add(m); break;
                case 11: t11.add(m); break;
                case 12: t12.add(m); break;
                case 13: t13.add(m); break;
                case 14: t14.add(m); break;
                case 15: t15.add(m); break;
                default: break;
            }
        }
        out.put("requestedBy", t8);
        out.put("medium", t11);
        out.put("agency", t12);
        out.put("samplingResponsibility", t13);
        out.put("resultStatus", t14);
        out.put("resultRemarks", t15);
        if (screen == SCREEN_TPI) {
            List<Map<String, Object>> cust = new ArrayList<>();
            for (Map<String, Object> r : repo.contractCustomers(u)) cust.add(idName(asInt(ci(r, "Id")), text(ci(r, "name"))));
            out.put("customers", cust);
        }
        List<Map<String, Object>> countries = new ArrayList<>();
        for (Map<String, Object> r : repo.countries(u)) countries.add(idName(asInt(ci(r, "Id")), text(ci(r, "Description"))));
        out.put("countries", countries);
        List<Map<String, Object>> groups = new ArrayList<>();
        for (Map<String, Object> r : repo.analysisGroups(u)) groups.add(idName(asInt(ci(r, "Id")), text(ci(r, "AnalysisGroupDescription"))));
        out.put("analysisGroups", groups);
        List<Map<String, Object>> fnt = new ArrayList<>();
        for (Map<String, Object> r : repo.farmingNTrade()) {
            Map<String, Object> m = idName(asInt(ci(r, "Id")), text(ci(r, "FarmingNTrade")));
            m.put("ExImFarmingTypeId", asInt(ci(r, "ExImFarmingTypeId")));
            m.put("ExImTradeTypeId", asInt(ci(r, "ExImTradeTypeId")));
            fnt.add(m);
        }
        out.put("farmingNTrade", fnt);
        List<Map<String, Object>> lots = new ArrayList<>();
        for (Map<String, Object> r : repo.jobLots(u)) lots.add(idName(asInt(ci(r, "Id")), text(ci(r, "JobLotDescription"))));
        out.put("jobLots", lots);
        /* ItemDtsFillFromGlobal: getGlobalAllItems without ItemTypeOfTypeId 14 and 17. */
        List<Map<String, Object>> items = new ArrayList<>();
        for (Map<String, Object> r : repo.allItems(u)) {
            int tot = asInt(ci(r, "ItemTypeOfTypeId"));
            if (tot == 14 || tot == 17) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("ItemName", text(ci(r, "ItemName")));
            m.put("ItemCode", text(ci(r, "ItemCode")));
            m.put("ItemCategory", text(ci(r, "ItemCategory")));
            m.put("ItemType", text(ci(r, "ItemType")));
            items.add(m);
        }
        out.put("items", items);
        return out;
    }

    /** TackingDbCall + TackingNoBind: FormHistory with CanViewAllRecord true; ActionId 1 Complete, 2 Pending, All none. */
    public List<Map<String, Object>> trackingNos(int screen, String status) {
        return trackingNos(user(screen, "View"), screen, status);
    }

    private List<Map<String, Object>> trackingNos(UserAccount u, int screen, String status) {
        Map<String, Object> g = new LinkedHashMap<>();
        if ("complete".equals(status)) g.put("ActionId", 1);
        else if (!"all".equals(status)) g.put("ActionId", 2);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.formHistory(u, fy(), screenName(screen), true, g)) {
            Map<String, Object> m = idName(asInt(ci(r, "Id")), text(ci(r, "LotRefNo")));
            m.put("Status", text(ci(r, "SampleStatus")));
            out.add(m);
        }
        return out;
    }

    /** HistoryComboFill: ActivityType "Items" -> Item, "TrackingNo" -> Tracking No. */
    private Map<String, Object> historyCombos(UserAccount u) {
        List<Map<String, Object>> items = new ArrayList<>(), tracks = new ArrayList<>();
        for (Map<String, Object> r : repo.historyDropDowns(u)) {
            String a = text(ci(r, "ActivityType"));
            Map<String, Object> m = idName(asInt(ci(r, "Id")), text(ci(r, "name")));
            if ("Items".equals(a)) items.add(m);
            else if ("TrackingNo".equals(a)) tracks.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("historyItems", items);
        out.put("historyTrackingNos", tracks);
        return out;
    }

    /** BtnRefreshHistory_Click. */
    public Map<String, Object> historyComboRefresh(int screen) { return historyCombos(user(screen, "View")); }

    // ================================================================= cascades (857 detail panel)

    /** CmbCustomer_Leave -> ContractNoDbCall(CustomerId) with RecId. */
    public List<Map<String, Object>> contracts(int screen, int customerId, int recId) {
        UserAccount u = user(screen, "View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.pendingContracts(u, customerId, recId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("LcOrderNo", text(ci(r, "LcOrderNo")));
            m.put("LcOrderDate", iso(ci(r, "LcOrderDate")));
            m.put("ItemQty", asDouble(ci(r, "ItemQty")));
            m.put("NetWeight", asDouble(ci(r, "NetWeight")));
            m.put("WeightUsedInLab", asDouble(ci(r, "WeightUsedInLab")));
            m.put("BalanceWeight", asDouble(ci(r, "BalanceWeight")));
            m.put("Mton", asDouble(ci(r, "Mton")));
            m.put("MtonUsedInLab", asDouble(ci(r, "MtonUsedInLab")));
            m.put("BalanceMton", asDouble(ci(r, "BalanceMton")));
            out.add(m);
        }
        return out;
    }

    /** BindInvoiceNoByContactID -> CommonServices.GetInvoicesDataForShipmentAnalysis(contractId) (SupplierCustomerId 0). */
    public List<Map<String, Object>> invoices(int screen, int contractId) {
        UserAccount u = user(screen, "View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.invoicesForShipment(u, 0, contractId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("InvoiceId", asInt(ci(r, "InvoiceId")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("InvoiceDate", iso(ci(r, "InvoiceDate")));
            m.put("ContractScheduleId", asInt(ci(r, "ContractScheduleId")));
            out.add(m);
        }
        return out;
    }

    /** BindGetContractSchedulesByContactID(contractId, customerId) - only when both are > 0. */
    public List<Map<String, Object>> schedules(int screen, int contractId, int customerId) {
        UserAccount u = user(screen, "View");
        List<Map<String, Object>> out = new ArrayList<>();
        if (contractId <= 0 || customerId <= 0) return out;
        for (Map<String, Object> r : repo.schedulesForShipment(u, customerId, contractId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ContractScheduleId", asInt(ci(r, "ContractScheduleId")));
            m.put("ContractScheduleNo", text(ci(r, "ContractScheduleNo")));
            m.put("ExImInvoiceId", asInt(ci(r, "ExImInvoiceId")));
            m.put("CustomerContractNo", text(ci(r, "CustomerContractNo")));
            m.put("MTons", asDouble(ci(r, "MTons")));
            out.add(m);
        }
        return out;
    }

    /** GetContractsInformation: usp_ExportContract_BalanceByIds; BalanceMton = Mton - ShippedMton (computed by the form). */
    public List<Map<String, Object>> contractInfo(int screen, String contractIds) {
        UserAccount u = user(screen, "View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.contractBalance(u, contractIds == null ? "" : contractIds)) {
            Map<String, Object> m = new LinkedHashMap<>();
            double mton = asDouble(ci(r, "Mton")), shipped = asDouble(ci(r, "ShippedMton"));
            m.put("ContractId", asInt(ci(r, "ContractId")));
            m.put("ContractNo", text(ci(r, "ContractNo")));
            m.put("Mton", mton);
            m.put("ShippedMton", shipped);
            m.put("BalanceMton", mton - shipped);
            out.add(m);
        }
        return out;
    }

    /** CmbRequiredAnalysisGroup_Leave -> GetParametersFromGroupStandards(Id). */
    public List<Map<String, Object>> groupParameters(int screen, int groupId) {
        UserAccount u = user(screen, "View");
        List<Map<String, Object>> out = new ArrayList<>();
        if (groupId <= 0) return out;
        for (Map<String, Object> r : repo.groupParameters(u, groupId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ParameterId", asInt(ci(r, "InvLabAnalysisItemsId")));
            m.put("Parameter", text(ci(r, "AnalysisParameterDescription")));
            out.add(m);
        }
        return out;
    }

    /** ThirdPartyType.FormHistory(org, comp, 0, activeOnly) - ReadById adds all types, 858's refresh button only active ones. */
    public List<Map<String, Object>> thirdPartyTypes(int screen, boolean activeOnly) {
        UserAccount u = user(screen, "View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.thirdPartyTypes(u, activeOnly)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ThirdPartyTypeId", asInt(ci(r, "ThirdPartyTypeID")));
            m.put("ThirdPartyType", text(ci(r, "PartyTypeName")));
            out.add(m);
        }
        return out;
    }

    // ================================================================= ReadById

    /** InvLabPreProductionExportLotInspectionHeader.GetByID(ID): header + the three detail lists. */
    public Map<String, Object> readById(int screen, int id) {
        user(screen, "View");
        List<Map<String, Object>> h = repo.header(id);
        if (h.isEmpty()) throw new IllegalArgumentException("Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index");
        Map<String, Object> r = h.get(0);
        Map<String, Object> hd = new LinkedHashMap<>();
        for (String k : new String[] {"Id", "RequestedById", "RequestMediumId", "JobLotId", "ItemId", "InspectionAgencyId",
                "SamplingResponsibilityId", "CountryOfOriginId", "CountryOfInspectionId", "TransitDays", "ReportResultRemarksId",
                "RequiredAnalysisId", "SampleHandedOverId", "ExImFarmingNTradeId", "ExImFarmingTypeId", "ExImTradeTypeId"})
            hd.put(k, asInt(ci(r, k)));
        for (String k : new String[] {"LotRefNo", "RequestRefNo", "RequestedBy", "RequestMedium", "ExporterLotRefNo",
                "CourierTrackingNo", "InspectionRemarks", "ReportRefNo", "ReportStatus", "LabRemarks", "LotInstructionsOrRemarks",
                "LotCurrentStage", "PlaceOfInspection"})
            hd.put(k, text(ci(r, k)));
        hd.put("QtyKgs", asDouble(ci(r, "QtyKgs")));
        for (String k : new String[] {"RequestDate", "SampleDispatchedDate", "SampleATADestinationDate", "SampleETADestinationDate",
                "ReportDate", "DateOfInspection", "ResultDate", "StockReservedDate", "SampleTakenDate", "StockRSealedDate", "SampleHandedOverDate"})
            hd.put(k, iso(ci(r, k)));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", hd);
        out.put("details", detailRows(id));
        List<Map<String, Object>> params = new ArrayList<>();
        for (Map<String, Object> p : repo.parameterDetails(id)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(p, "Id")));
            m.put("ParameterId", asInt(ci(p, "ParameterId")));
            m.put("Remarks", text(ci(p, "Remarks")));
            params.add(m);
        }
        out.put("parameters", params);
        List<Map<String, Object>> samp = new ArrayList<>();
        for (Map<String, Object> s : repo.samplingDetails(id)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(s, "Id")));
            m.put("ThirdPartyTypeId", asInt(ci(s, "ThirdPartyTypeId")));
            m.put("ThirdPartyType", text(ci(s, "PartyTypeName")));
            m.put("NoOfSample", asInt(ci(s, "NoOfSample")));
            m.put("SampleWeight_Kg", asDouble(ci(s, "SampleWeight_Kg")));
            m.put("TotalWeight_Kg", asDouble(ci(s, "TotalWeight_Kg")));
            m.put("SealNo", text(ci(s, "SealNo")));
            m.put("Remarks", text(ci(s, "Remarks")));
            samp.add(m);
        }
        out.put("sampling", samp);
        return out;
    }

    private List<Map<String, Object>> detailRows(int id) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : repo.details(id)) {
            Map<String, Object> m = new LinkedHashMap<>();
            int ref = asInt(ci(d, "ReferredStatusId"));
            m.put("Id", asInt(ci(d, "Id")));
            m.put("Lot", text(ci(d, "SubLot")));
            m.put("CustomerId", asInt(ci(d, "SupplierCustomerId")));
            m.put("Customer", text(ci(d, "SupplierCustomer")));
            m.put("ContractId", asInt(ci(d, "ContractId")));
            m.put("ContractNo", text(ci(d, "ContractNo")));
            m.put("ContractScheduleId", asInt(ci(d, "ContractScheduleId")));
            m.put("ContractScheduleNo", text(ci(d, "ContractScheduleNo")));
            m.put("InvoiceId", asInt(ci(d, "InvoiceId")));
            m.put("InvoiceNo", text(ci(d, "InvoiceNo")));
            m.put("MTons", asDouble(ci(d, "Weight")));
            m.put("SealNoForBuyer", text(ci(d, "SealNoBuyer")));
            m.put("SealNoForShipper", text(ci(d, "SealNoShipper")));
            m.put("Remarks", text(ci(d, "SubRemarks")));
            m.put("ReferredStatusId", ref);
            m.put("ReferredStatus", ref == 1 ? "Referred" : "Not Referred");
            out.add(m);
        }
        return out;
    }

    /** BindDetailByHistoryHeader - the lower History grid (GetByID(Id).LotInspectionDetails). */
    public List<Map<String, Object>> historyDetail(int screen, int id) {
        user(screen, "View");
        if (id <= 0) return new ArrayList<>();
        if (repo.header(id).isEmpty()) throw new IllegalArgumentException("Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index");
        return detailRows(id);
    }

    /** CommonServices.GetNoofAttachmentsByScreenName(Id, ScreenName) - attachment name, custom name, entry date. */
    public List<Map<String, Object>> attachments(int screen, int id) {
        user(screen, "View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.attachments(id, screenName(screen))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("AttachmentName", text(ci(r, "Attachment")));
            m.put("CustomName", text(ci(r, "UploadedFileCustomName")));
            m.put("EntryDate", isoDateTime(ci(r, "EntryDate")));
            out.add(m);
        }
        return out;
    }

    // ================================================================= Insert

    /**
     * Insert(): FormValidation (the form's texts, in order), ValidateDuplicateWeightPerItemInSampling,
     * the model build, (857) the detail/header MTon check, then BLL Save (ActionId 1 new / 2 update;
     * EntryDate / ModifyDate / ApprovedDate = now). "Record Saved Successfully" / "Record Update Successfully".
     */
    public Map<String, Object> save(int screen, Map<String, Object> b) {
        int recId = asInt(b.get("recId"));
        if (screen == SCREEN_LAB && recId == 0) throw new IllegalArgumentException("RecId not Found...");
        UserAccount u = user(screen, recId > 0 ? "Update" : "Save");
        boolean lab = screen == SCREEN_LAB;
        formValidation(b, lab);
        List<Map<String, Object>> sampling = list(b.get("sampling"));
        validateDuplicateWeight(sampling);

        Map<String, Object> h = new LinkedHashMap<>();
        LocalDateTime now = LocalDateTime.now();
        int countryOfInspection = asInt(b.get("CountryOfInspectionId"));
        int transitDays = asInt(b.get("TransitDays"));
        LocalDate dispatched = checkedDate(b, "SampleDispatchedDate");
        /* model property order (Architecture.Model.Export.InvLabPreProductionExportLotInspectionHeader) */
        h.put("IsApproved", false);
        h.put("ApprovedDate", Timestamp.valueOf(now));
        h.put("ConfirmationDate", null);
        h.put("DateOfInspection", ts(checkedDate(b, "DateOfInspection")));
        h.put("ResultDate", ts(checkedDate(b, "ResultDate")));
        h.put("EntryDate", Timestamp.valueOf(now));
        h.put("ModifyDate", Timestamp.valueOf(now));
        h.put("ReportDate", ts(checkedDate(b, "ReportDate")));
        /* RequestDate = Conversion.ToDateTime(txtRequestLodgDate.Text) - the date only */
        LocalDate req = asDate(b.get("RequestDate"));
        h.put("RequestDate", req == null ? null : Timestamp.valueOf(req.atStartOfDay()));
        h.put("SampleATADestinationDate", ts(checkedDate(b, "SampleATADestinationDate")));
        h.put("SampleDispatchedDate", ts(dispatched));
        h.put("SampleETADestinationDate", dispatched == null ? null : ts(dispatched.plusDays(transitDays)));
        h.put("SampleHandedOverDate", ts(checkedDate(b, "SampleHandedOverDate")));
        h.put("SampleTakenDate", ts(checkedDate(b, "SampleTakenDate")));
        h.put("StockReservedDate", ts(checkedDate(b, "StockReservedDate")));
        h.put("StockRSealedDate", ts(checkedDate(b, "StockRSealedDate")));
        double qty = asDouble(b.get("QtyKgs"));
        h.put("QtyKgs", qty);
        h.put("ActionId", recId == 0 ? 1 : 2);
        h.put("ApprovedUserId", u.getId());
        h.put("BranchesId", u.getBranchesId());
        h.put("CompanyId", u.getCompanyId());
        h.put("CountryOfInspectionId", countryOfInspection);
        h.put("PlaceOfInspectionId", 0);
        h.put("CountryOfOriginId", asInt(b.get("CountryOfOriginId")));
        h.put("EntryUserId", u.getId());
        h.put("FinancialYearId", fy());
        h.put("Id", recId);
        h.put("InspectedByLabId", 0);
        h.put("InspectionAgencyId", asInt(b.get("InspectionAgencyId")));
        h.put("ItemId", asInt(b.get("ItemId")));
        int jobLot = asInt(b.get("JobLotId"));
        h.put("JobLotId", lab ? (jobLot > 0 ? jobLot : 0) : jobLot);
        h.put("LotCurrentStageId", 0);
        h.put("ModifyUserId", u.getId());
        h.put("OrganizationId", u.getOrganizationId());
        h.put("ProjectsId", u.getBranchesId());
        h.put("ReportDocNo", 0);
        h.put("ReportResultRemarksId", asInt(b.get("ReportResultRemarksId")));
        h.put("RequestedById", asInt(b.get("RequestedById")));
        h.put("RequestMediumId", asInt(b.get("RequestMediumId")));
        h.put("RequiredAnalysisId", asInt(b.get("RequiredAnalysisId")));
        h.put("RevisionNo", 0);
        h.put("SampleHandedOverId", asInt(b.get("SampleHandedOverId")));
        h.put("SamplingResponsibilityId", asInt(b.get("SamplingResponsibilityId")));
        h.put("TransitDays", transitDays);
        int fnt = asInt(b.get("ExImFarmingNTradeId"));
        h.put("ExImFarmingNTradeId", fnt > 0 ? fnt : 0);
        h.put("ExImFarmingTypeId", fnt > 0 ? asInt(b.get("ExImFarmingTypeId")) : 0);
        h.put("ExImTradeTypeId", fnt > 0 ? asInt(b.get("ExImTradeTypeId")) : 0);
        h.put("CourierTrackingNo", str(b.get("CourierTrackingNo")));
        h.put("ExporterLotRefNo", str(b.get("ExporterLotRefNo")).trim());
        h.put("InspectionRemarks", str(b.get("InspectionRemarks")));
        h.put("InspectionReportNo", null);
        h.put("LabRemarks", str(b.get("LabRemarks")));
        h.put("LotRefNo", str(b.get("LotRefNo")));
        String countryText = str(b.get("CountryOfInspectionText"));
        h.put("PlaceOfInspection", lab ? countryText : (countryOfInspection > 0 ? countryText : ""));
        h.put("ProductSpecification", str(b.get("ProductSpecification")).trim());
        h.put("ReportRefNo", str(b.get("ReportRefNo")).trim());
        h.put("ReportStatus", str(b.get("ReportStatus")));
        h.put("RequestedBy", str(b.get("RequestedBy")));
        h.put("RequestMedium", str(b.get("RequestMedium")));
        h.put("RequestRefNo", str(b.get("RequestRefNo")));
        h.put("RequiredAnalysis", null);
        h.put("LotInstructionsOrRemarks", str(b.get("LotInstructionsOrRemarks")).trim());
        h.put("ScreenName", screenName(screen));
        h.put("AttachmentsValues", "");
        h.put("CustomAttachmentsValues", "");

        List<Map<String, Object>> details = new ArrayList<>();
        BigDecimal rowMTon = BigDecimal.ZERO;
        for (Map<String, Object> r : list(b.get("details"))) {
            Map<String, Object> vd = new LinkedHashMap<>();
            BigDecimal w = BigDecimal.valueOf(asDouble(r.get("MTons")));
            vd.put("Weight", w);
            vd.put("ContractId", asInt(r.get("ContractId")));
            vd.put("ContractScheduleId", asInt(r.get("ContractScheduleId")));
            vd.put("InvoiceId", asInt(r.get("InvoiceId")));
            vd.put("Id", asInt(r.get("Id")));
            vd.put("InvLabPreProductionExportLotInspectionHeaderId", 0);
            vd.put("SupplierCustomerId", asInt(r.get("CustomerId")));
            vd.put("SealNoBuyer", str(r.get("SealNoForBuyer")));
            vd.put("SealNoShipper", str(r.get("SealNoForShipper")));
            vd.put("Status", null);
            vd.put("SubLot", str(r.get("Lot")));
            vd.put("SubRemarks", str(r.get("Remarks")));
            rowMTon = rowMTon.add(w);
            details.add(vd);
        }
        if (!lab && rowMTon.compareTo(BigDecimal.valueOf(qty)) > 0)
            throw new IllegalArgumentException("Detail MTon (" + rowMTon.stripTrailingZeros().toPlainString() + ") cannot be greater than Header MTon (" + num(qty) + ").");

        List<Map<String, Object>> params = new ArrayList<>();
        for (Map<String, Object> r : list(b.get("parameters"))) {
            if (!asBool(r.get("Checked"))) continue;      /* grdParameters.GetCheckedRows() */
            Map<String, Object> vd = new LinkedHashMap<>();
            vd.put("Id", asInt(r.get("Id")));
            vd.put("InvLabPreProductionExportLotInspectionHeaderId", 0);
            vd.put("ParameterId", asInt(r.get("ParameterId")));
            vd.put("RequiredAnalysisGroupId", 0);
            vd.put("Remarks", str(r.get("Remarks")));
            params.add(vd);
        }
        List<Map<String, Object>> samples = new ArrayList<>();
        for (Map<String, Object> r : sampling) {
            if (asInt(r.get("ThirdPartyTypeId")) <= 0 || asInt(r.get("NoOfSample")) <= 0 || asDouble(r.get("SampleWeight_Kg")) <= 0) continue;
            Map<String, Object> vd = new LinkedHashMap<>();
            vd.put("SampleWeight_Kg", asDouble(r.get("SampleWeight_Kg")));
            vd.put("TotalWeight_Kg", asDouble(r.get("TotalWeight_Kg")));
            vd.put("Id", asInt(r.get("Id")));
            vd.put("InvLabPreProductionExportLotInspectionHeaderId", 0);
            vd.put("NoOfSample", asInt(r.get("NoOfSample")));
            vd.put("ThirdPartyTypeId", asInt(r.get("ThirdPartyTypeId")));
            vd.put("Remarks", str(r.get("Remarks")));
            /* 857 never copies SealNo into the model (null -> not sent); 858 does. */
            vd.put("SealNo", lab ? str(r.get("SealNo")) : null);
            samples.add(vd);
        }
        String removeIds = lab ? null : str(b.get("removeDetailIds"));
        int id = repo.save(h, removeIds, details, params, samples);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", id);
        out.put("message", recId == 0 ? "Record Saved Successfully" : "Record Update Successfully");
        return out;
    }

    private static void formValidation(Map<String, Object> b, boolean lab) {
        if (str(b.get("LotRefNo")).trim().isEmpty()) throw new IllegalArgumentException("Lot Reference/tracking No is required.");
        if (asInt(b.get("RequestedById")) == 0) throw new IllegalArgumentException("Requested By is required.");
        if (asInt(b.get("RequestMediumId")) == 0) throw new IllegalArgumentException("Request Medium is required.");
        if (asInt(b.get("ItemId")) == 0) throw new IllegalArgumentException("Item selection is required.");
        if (asDouble(b.get("QtyKgs")) <= 0) throw new IllegalArgumentException("Quantity in M.Tons must be greater than 0.");
        if (asInt(b.get("InspectionAgencyId")) == 0) throw new IllegalArgumentException("Inspection Agency is required.");
        if (asInt(b.get("SamplingResponsibilityId")) == 0) throw new IllegalArgumentException("Sampling Responsibility is required.");
        if (asInt(b.get("CountryOfOriginId")) == 0) throw new IllegalArgumentException("Country of Origin is required.");
        if (!lab && asInt(b.get("ResultStatusId")) == 0) throw new IllegalArgumentException("Result Status is required.");
        LocalDate disp = checkedDate(b, "SampleDispatchedDate"), ata = checkedDate(b, "SampleATADestinationDate");
        LocalDate taken = checkedDate(b, "SampleTakenDate"), reserved = checkedDate(b, "StockReservedDate");
        LocalDate sealed = checkedDate(b, "StockRSealedDate"), handOver = checkedDate(b, "SampleHandedOverDate");
        if (disp != null && ata != null && !disp.isBefore(ata))
            throw new IllegalArgumentException("'Sample Dispatched Date' cannot be greater than or Equal 'Sample ATA Destination Date'");
        if (disp != null && taken != null && taken.isAfter(disp))
            throw new IllegalArgumentException("'Sample Dispatched Date' cannot be lesser than 'Sample Taken Date'");
        if (lab && reserved == null && taken == null && sealed == null)
            throw new IllegalArgumentException("'Stock Reserved Date' or  'Sample Taken Date' or 'Stock Sealed Date' is Required");
        if (reserved != null && taken != null && reserved.isAfter(taken))
            throw new IllegalArgumentException("'Stock Reserved Date' cannot be greater than 'Sample Taken Date'");
        if (sealed != null && taken != null && sealed.isBefore(taken))
            throw new IllegalArgumentException("'Sample Taken Date' cannot be greater than 'Stock Sealed Date'");
        if (sealed != null && handOver != null && sealed.isAfter(handOver))
            throw new IllegalArgumentException("'Stock Sealed Date' cannot be greater than 'Sample hand over Date'");
    }

    /** ValidateDuplicateWeightPerItemInSampling - same ThirdPartyType with the same SampleWeight_Kg (> 0) twice. */
    private static void validateDuplicateWeight(List<Map<String, Object>> rows) {
        Map<String, Integer> count = new LinkedHashMap<>();
        Map<String, String> names = new HashMap<>();
        Map<String, Double> weights = new HashMap<>();
        for (Map<String, Object> r : rows) {
            double w = asDouble(r.get("SampleWeight_Kg"));
            if (w <= 0) continue;
            String k = asInt(r.get("ThirdPartyTypeId")) + "|" + w;
            count.merge(k, 1, Integer::sum);
            names.putIfAbsent(k, str(r.get("ThirdPartyType")));
            weights.put(k, w);
        }
        List<String> msgs = new ArrayList<>();
        for (Map.Entry<String, Integer> e : count.entrySet())
            if (e.getValue() > 1) msgs.add("ThirdPartyType '" + names.get(e.getKey()) + "' has duplicate SampleWeight_Kg: " + num(weights.get(e.getKey())));
        if (!msgs.isEmpty()) throw new IllegalArgumentException("Duplicate SampleWeight_Kg values found:\n" + String.join("\n", msgs));
    }

    // ================================================================= history

    /**
     * HistoryFill: CanViewAllRecord from the rights (EntryUser when not), the date pair of the checked
     * radio (Request / Entry / Modify / Approved), @Id (Tracking No), @ItemId, @ActionId (1 Complete,
     * 2 Pending - the default, All none). Columns as dtcol; 858 shows the Lab* user columns.
     */
    public List<Map<String, Object>> history(int screen, Map<String, Object> f) {
        UserAccount u = user(screen, "View");
        boolean canViewAll = allowed(u, screen, "CanViewAllRecord");
        Map<String, Object> g = new LinkedHashMap<>();
        LocalDate from = asBool(f.get("fromChecked")) ? asDate(f.get("fromDate")) : null;
        LocalDate to = asBool(f.get("toChecked")) ? asDate(f.get("toDate")) : null;
        String by = str(f.get("dateBy"));
        String fk = "FromDate", tk = "ToDate";
        if ("entry".equals(by)) { fk = "EntryFromDate"; tk = "EntryToDate"; }
        else if ("modify".equals(by)) { fk = "ModifyFromDate"; tk = "ModifyToDate"; }
        else if ("approved".equals(by)) { fk = "ApprovedFromDate"; tk = "ApprovedToDate"; }
        if (from != null) g.put(fk, java.sql.Date.valueOf(from));
        if (to != null) g.put(tk, java.sql.Date.valueOf(to));
        int id = asInt(f.get("trackingId")), item = asInt(f.get("itemId"));
        if (id != 0) g.put("Id", id);
        if (item != 0) g.put("ItemId", item);
        String st = str(f.get("status"));
        if ("complete".equals(st)) g.put("ActionId", 1);
        else if (!"all".equals(st)) g.put("ActionId", 2);
        boolean lab = screen == SCREEN_LAB;
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.formHistory(u, fy(), screenName(screen), canViewAll, g)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("LotRefNo", text(ci(r, "LotRefNo")));
            m.put("RequestDate", iso(ci(r, "RequestDate")));
            m.put("RequestRefNo", text(ci(r, "RequestRefNo")));
            m.put("RequestedByName", text(ci(r, "RequestedByName")));
            m.put("RequestedBy", text(ci(r, "RequestedBy")));
            m.put("RequestMedium", text(ci(r, "RequestMedium")));
            m.put("ExporterLotRefNo", text(ci(r, "ExporterLotRefNo")));
            m.put("ItemName", text(ci(r, "ItemName")));
            m.put("MTon", asDouble(ci(r, "QtyKgs")));
            m.put("InspectionAgency", text(ci(r, "InspectionAgencyName")));
            m.put("CountryOfOrigin", text(ci(r, "CountryOfOrigin")));
            m.put("CountryOfInspection", text(ci(r, "CountryOfInspection")));
            m.put("PlaceOfInspection", text(ci(r, "PlaceOfInspection")));
            m.put("CourierTrackingNo", text(ci(r, "CourierTrackingNo")));
            m.put("SampleDispatchedDate", iso(ci(r, "SampleDispatchedDate")));
            m.put("TransitDays", asInt(ci(r, "TransitDays")));
            m.put("SampleETADestinationDate", iso(ci(r, "SampleETADestinationDate")));
            m.put("SampleATADestinationDate", iso(ci(r, "SampleATADestinationDate")));
            m.put("InspectionRemarks", text(ci(r, "InspectionRemarks")));
            m.put("ReportRefNo", text(ci(r, "ReportRefNo")));
            m.put("ReportDate", iso(ci(r, "ReportDate")));
            m.put("DateOfInspection", iso(ci(r, "DateOfInspection")));
            m.put("ReportStatus", text(ci(r, "ReportStatus")));
            m.put("LotCurrentStage", text(ci(r, "LotCurrentStage")));
            m.put("RequiredAnalysisGroup", text(ci(r, "RequiredAnalysisGroup")));
            m.put("StockReservedDate", iso(ci(r, "StockReservedDate")));
            m.put("SampleTakenDate", iso(ci(r, "SampleTakenDate")));
            m.put("StockRSealedDate", iso(ci(r, "StockRSealedDate")));
            m.put("SampleHandedOver", text(ci(r, "SampleHandedOver")));
            m.put("SampleHandedOverDate", iso(ci(r, "SampleHandedOverDate")));
            m.put("LabRemarks", text(ci(r, "LabRemarks")));
            m.put("SampleStatus", text(ci(r, "SampleStatus")));
            m.put("EntryUserName", text(ci(r, lab ? "LabEntryUserName" : "EntryUserName")));
            m.put("EntryDate", isoDateTime(ci(r, lab ? "LabEntryDate" : "EntryDate")));
            m.put("ModifyUserName", text(ci(r, lab ? "LabModifyUserName" : "ModifyUserName")));
            m.put("ModifyDate", isoDateTime(ci(r, lab ? "LabModifyDate" : "ModifyDate")));
            m.put("ApprovedUser", text(ci(r, lab ? "LabApprovedUser" : "ApprovedUser")));
            m.put("ApprovedDate", isoDateTime(ci(r, lab ? "LabApprovedDate" : "ApprovedDate")));
            m.put("NoOfAttachments", asInt(ci(r, "NoOfAttachments")));
            out.add(m);
        }
        return out;
    }

    /** Print rights gate for the 514 / 514_01 buttons (the PDF itself goes through ReportRegistry). */
    public Map<String, Object> printCheck(int screen, int id) {
        user(screen, "Print");
        if (id == 0) throw new IllegalArgumentException("No Record Found");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", id);
        return out;
    }

    // ================================================================= helpers

    private static Map<String, Object> idName(int id, String name) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", id);
        m.put("name", name);
        return m;
    }

    /** A ShowCheckBox DateTimePicker: the date only when its box is ticked. */
    private static LocalDate checkedDate(Map<String, Object> b, String key) {
        return asBool(b.get(key + "Checked")) ? asDate(b.get(key)) : null;
    }

    /** DateTimePicker.Value - the picked date with the current time of day. */
    private static Timestamp ts(LocalDate d) { return d == null ? null : Timestamp.valueOf(d.atTime(LocalTime.now().withNano(0))); }

    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> list(Object v) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (v instanceof List) for (Object o : (List<Object>) v) if (o instanceof Map) out.add((Map<String, Object>) o);
        return out;
    }

    static String num(double d) {
        if (d == Math.rint(d) && Math.abs(d) < 1e15) return String.valueOf((long) d);
        return BigDecimal.valueOf(d).setScale(10, RoundingMode.HALF_UP).stripTrailingZeros().toPlainString();
    }

    private static String str(Object v) { return v == null ? "" : String.valueOf(v); }
    private static String text(Object v) { return v == null ? "" : String.valueOf(v).trim(); }

    static int asInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        if (v instanceof Boolean) return (Boolean) v ? 1 : 0;
        String s = String.valueOf(v).trim().replace(",", "");
        if (s.isEmpty()) return 0;
        try { return Integer.parseInt(s); }
        catch (NumberFormatException e) {
            try { return (int) Double.parseDouble(s); } catch (NumberFormatException e2) { return 0; }
        }
    }

    static double asDouble(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).doubleValue();
        try { return Double.parseDouble(String.valueOf(v).trim().replace(",", "")); }
        catch (NumberFormatException e) { return 0; }
    }

    static boolean asBool(Object v) {
        if (v == null) return false;
        if (v instanceof Boolean) return (Boolean) v;
        String s = String.valueOf(v).trim().toLowerCase();
        return "true".equals(s) || "1".equals(s) || "on".equals(s);
    }

    static LocalDate asDate(Object v) {
        if (v == null) return null;
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return null;
        if (s.length() > 10) s = s.substring(0, 10);
        try { return LocalDate.parse(s); } catch (DateTimeParseException e) { return null; }
    }

    static String iso(Object v) {
        if (v == null) return "";
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().toLocalDate().toString();
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString();
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().toLocalDate().toString();
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).toLocalDate().toString();
        String s = String.valueOf(v).trim();
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }

    static String isoDateTime(Object v) {
        if (v == null) return "";
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().withNano(0).toString();
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().withNano(0).toString();
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).withNano(0).toString();
        return String.valueOf(v).trim();
    }

    /** Distinct contract ids of the grid rows (GetContractsInformation). */
    public static String contractIds(List<Map<String, Object>> rows) {
        Set<Integer> s = new LinkedHashSet<>();
        for (Map<String, Object> r : rows) { int c = asInt(r.get("ContractId")); if (c != 0) s.add(c); }
        StringBuilder sb = new StringBuilder();
        for (Integer i : s) { if (sb.length() > 0) sb.append(','); sb.append(i); }
        return sb.toString();
    }
}
