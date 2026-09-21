package com.mst.services;

import com.mst.repositories.ContractorWagesBillWriter;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Labour Wages - Contractor Wages Bill.
 * Ported from Architecture.WinApp.Contractor_Wages\frmwagesBillHeader.cs (5,407 lines),
 * Architecture.BLL.ContractorWages.InvContractorWagesBillHeader and its DAL.
 *
 * DocumentTypeId is 101 (form :479). The bill is always raised AGAINST another document, so
 * RefDocumentTypeId varies (46, 68, 80, 86, 112, 143, 205, 210, 806 ...) - which is why the desktop
 * has a "Pending Data For Load" tab.
 *
 * Procedure contract, read out of the BLL rather than guessed:
 *
 *   Insert / Update   Sp_InvContractorWagesBillHeader_Insert | _Update   (BLL :358-370)
 *   Detail insert     Sp_InvContractorWagesBillDetail_Insert             (DAL SetData)
 *   Generate code     Sp_InvContractorWagesBillHeader_GetAllMethod @Activity='GenerateCode'
 *   Read one          ... @Activity='ReadById'   + @Activity='ReadDetailByHeaderId' for the rows
 *   Id by doc no      ... @Activity='GetIdbyDocNo'
 *   Form history      ... @Activity='FormHistory'
 *   Document types    ... @Activity='DocumentTypeForManualWages'
 *
 * The voucher posting, the audit copies and the stock-evaluation call all live in
 * ContractorWagesBillWriter, which reproduces the desktop DAL's sequence step for step.
 */
@Service
public class LabourWagesService {

    private static final Logger LOG = LoggerFactory.getLogger(LabourWagesService.class);

    /** frmwagesBillHeader.cs :479 */
    public static final int DOCUMENT_TYPE_ID = 101;

    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private ContractorWagesBillWriter writer;

    private static final String PROC = "Sp_InvContractorWagesBillHeader_GetAllMethod";

    // ---------------------------------------------------------------- reads

    /** GenerateCode(), BLL :376-418. */
    public int generateDocNo(int refDocumentTypeId) {
        try {
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                    "EXEC " + PROC + " @OrganizationId=?, @CompanyId=?, @RefDocumentTypeId=?, "
                  + "@FinancialYearId=?, @Activity=?",
                    currentUserContext.currentOrganizationId(),
                    currentUserContext.currentCompanyId(),
                    refDocumentTypeId,
                    currentUserContext.currentFinancialYearId(),
                    "GenerateCode");
            if (!rows.isEmpty()) {
                Object v = rows.get(0).values().iterator().next();
                if (v instanceof Number) return ((Number) v).intValue();
            }
        } catch (Exception e) {
            LOG.error("Wages bill code generation failed for ref doc type {}", refDocumentTypeId, e);
        }
        return 0;
    }

    /** DocumentTypeForManualWages(), BLL :533-567 - the Reference Document Type combo. */
    public List<Map<String, Object>> getDocumentTypes() {
        try {
            return jdbcTemplate.queryForList(
                    "EXEC [dbo]." + PROC + " @OrganizationId=?, @CompanyId=?, @BranchesId=?, @Activity=?",
                    currentUserContext.currentOrganizationId(),
                    currentUserContext.currentCompanyId(),
                    currentUserContext.currentBranchId(),
                    "DocumentTypeForManualWages");
        } catch (Exception e) {
            LOG.error("Wages bill document type list failed", e);
            return Collections.emptyList();
        }
    }

    /**
     * GetByID(), BLL :421-443 plus the DAL's second pass: the header comes back from
     * @Activity='ReadById' and its rows from @Activity='ReadDetailByHeaderId' with the same @Id.
     */
    public Map<String, Object> getById(int id) {
        Map<String, Object> out = new LinkedHashMap<>();
        try {
            List<Map<String, Object>> head = jdbcTemplate.queryForList(
                    "EXEC " + PROC + " @Id=?, @Activity=?", id, "ReadById");
            if (head.isEmpty()) return Collections.emptyMap();
            out.put("header", head.get(0));
            out.put("details", jdbcTemplate.queryForList(
                    "EXEC " + PROC + " @Id=?, @Activity=?", id, "ReadDetailByHeaderId"));
        } catch (Exception e) {
            LOG.error("Wages bill read failed for id {}", id, e);
            return Collections.emptyMap();
        }
        return out;
    }

    /** GetContractorWagesBillHeaderIdByDocNo(), BLL :445-488 - the clickable doc-no lookup. */
    public Integer getIdByDocNo(int refDocumentTypeId, int docNo) {
        try {
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                    "EXEC " + PROC + " @OrganizationId=?, @CompanyId=?, @RefDocumentTypeId=?, "
                  + "@DocNo=?, @Activity=?",
                    currentUserContext.currentOrganizationId(),
                    currentUserContext.currentCompanyId(),
                    refDocumentTypeId, docNo, "GetIdbyDocNo");
            if (!rows.isEmpty()) {
                Object v = rows.get(0).values().iterator().next();
                if (v instanceof Number) return ((Number) v).intValue();
            }
        } catch (Exception e) {
            LOG.error("Wages bill id-by-docno failed for {}/{}", refDocumentTypeId, docNo, e);
        }
        return null;
    }

    /** FormHistory(), BLL :569-611 - the footer History button's grid. */
    public List<Map<String, Object>> getFormHistory(Integer noOfRecords) {
        try {
            return jdbcTemplate.queryForList(
                    "EXEC " + PROC + " @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, "
                  + "@FinancialYearId=?, @NoOfRecords=?, @Activity=?",
                    currentUserContext.currentOrganizationId(),
                    currentUserContext.currentCompanyId(),
                    DOCUMENT_TYPE_ID,
                    currentUserContext.currentFinancialYearId(),
                    (noOfRecords == null || noOfRecords <= 0) ? null : noOfRecords,
                    "FormHistory");
        } catch (Exception e) {
            LOG.error("Wages bill form history failed", e);
            return Collections.emptyList();
        }
    }

    /** GetHistory(), BLL :618+ - the History tab's date / bill-serial filters. */
    public List<Map<String, Object>> getHistory(String fromDate, String toDate,
                                                Integer fromDocNo, Integer toDocNo) {
        try {
            return jdbcTemplate.queryForList(
                    "EXEC " + PROC + " @OrganizationId=?, @CompanyId=?, @BillDateFrom=?, "
                  + "@BillDateTo=?, @BillSrFrom=?, @BillSrTo=?, @DocumentTypeId=?, @Activity=?",
                    currentUserContext.currentOrganizationId(),
                    currentUserContext.currentCompanyId(),
                    date(fromDate), date(toDate),
                    (fromDocNo == null || fromDocNo == 0) ? null : fromDocNo,
                    (toDocNo == null || toDocNo == 0) ? null : toDocNo,
                    DOCUMENT_TYPE_ID, "GetHistory");
        } catch (Exception e) {
            LOG.error("Wages bill history failed", e);
            return Collections.emptyList();
        }
    }

    // ---------------------------------------------------------------- grid lookups

    /**
     * The two editable columns in both wages grids are Contractor Name (SupplierId) and
     * Labour / Wages Activity (WagesId). Everything else on a row - Item, Job Lot, Packing Type,
     * Move From, Crop, Pack Size, Date - is EditType 0 on the desktop grid (:735-745): it arrives
     * with the loaded reference document, it is not chosen from a list. So only these two are
     * fetched, exactly as the form does:
     *
     *   Contractor  Sp_SupplierCustomer_GetAllMethod
     *               @Activity='ReadByOrganizationCompanyIdForContractorWages'   (form :506)
     *   Activity    [dbo].[USP_WagesTypeIdsAgainstDocumentType_GetAll] gives the WagesActivityIds
     *               allowed for THIS reference document type (form :540-544), and those ids then
     *               filter Sp_InvConractorWagesAccounts_GetAllMethod
     *               @Activity='GetWagesItemsByWagesTypeIds', @ActionId=1   (form :545)
     *
     * The activity list is therefore per reference-document-type, not a single global list.
     */
    public Map<String, Object> getGridLookups(int refDocumentTypeId) {
        Map<String, Object> out = new LinkedHashMap<>();
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();

        List<Map<String, Object>> contractors = new ArrayList<>();
        try {
            contractors = jdbcTemplate.queryForList(
                    "EXEC Sp_SupplierCustomer_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                    orgId, compId, "ReadByOrganizationCompanyIdForContractorWages");
        } catch (Exception e) {
            LOG.error("Contractor list failed", e);
        }

        String wagesActivityIds = null;
        try {
            for (Map<String, Object> r : jdbcTemplate.queryForList(
                    "EXEC [dbo].[USP_WagesTypeIdsAgainstDocumentType_GetAll]")) {
                if (asInt(r.get("DocumentTypeId")) == refDocumentTypeId) {
                    Object v = r.get("WagesActivityIds");
                    wagesActivityIds = v == null ? null : String.valueOf(v);
                    break;
                }
            }
        } catch (Exception e) {
            LOG.error("Wages type ids by document type failed", e);
        }

        List<Map<String, Object>> activities = new ArrayList<>();
        try {
            activities = jdbcTemplate.queryForList(
                    "EXEC Sp_InvConractorWagesAccounts_GetAllMethod @OrganizationId=?, @CompanyId=?, "
                  + "@WagesLookupIds=?, @ActionId=?, @Activity=?",
                    orgId, compId, wagesActivityIds, 1, "GetWagesItemsByWagesTypeIds");
        } catch (Exception e) {
            LOG.error("Wages activity list failed for ref doc type {}", refDocumentTypeId, e);
        }

        out.put("contractors", contractors);
        out.put("wagesActivities", activities);
        out.put("wagesActivityIds", wagesActivityIds);
        return out;
    }

    /**
     * CommonServices.GetWagesRate (form :977) - the schedule rate in force for a
     * date / pack size / wages account / contractor. Same procedure the Contractor-Wise schedule
     * screen uses. A miss returns null; nothing is defaulted.
     */
    public Map<String, Object> getWagesRate(String docDate, double packSize,
                                            int wagesAccountId, int contractorId) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("wagesRate", null);
        out.put("scheduleId", null);
        try {
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                    "EXEC Sp_InvContractorWagesSchedule_GetAllMethod @OrganizationId=?, @CompanyId=?, "
                  + "@InvConractorWagesAccountsId=?, @ContractorId=?, @EffectedDate=?, @PackUomFrom=?, @Activity=?",
                    currentUserContext.currentOrganizationId(),
                    currentUserContext.currentCompanyId(),
                    wagesAccountId, contractorId, date(docDate), packSize,
                    "GetWagesScheduleRateByEffectiveDateWagesAccountIdandPackSize");
            if (!rows.isEmpty()) {
                out.put("wagesRate", rows.get(0).get("WageRate"));
                out.put("scheduleId", rows.get(0).get("Id"));
            }
        } catch (Exception e) {
            LOG.error("Wages rate lookup failed", e);
        }
        return out;
    }

    // ---------------------------------------------------------------- save

    /**
     * Insert()/Update() on the form. The validation messages below are the form's own, in its own
     * order (:2572-2600), and the confirms are "Are you sure to Save?" / "Are you sure to Update?"
     * (:2724, :2730). The success text carries the doc number, as :3009/:3013 do.
     *
     * NOTE: a save here posts a GL voucher through ContractorWagesBillWriter. It has not been
     * exercised against a database from this session, so it must first be run on the approved test
     * data - not on a live bill.
     */
    @Transactional
    public Map<String, Object> save(Map<String, Object> body) {
        Map<String, Object> res = new HashMap<>();

        int id = asInt(body.get("id"));
        int refDocumentTypeId = asInt(body.get("refDocumentTypeId"));
        int refDocNo = asInt(body.get("refDocNo"));
        int refDocNoId = asInt(body.get("refDocNoId"));
        int docNo = asInt(body.get("docNo"));
        String docDate = str(body.get("docDate"));

        @SuppressWarnings("unchecked")
        List<Map<String, Object>> rows = body.get("details") instanceof List
                ? (List<Map<String, Object>>) body.get("details") : new ArrayList<>();

        // FormValidation, form :2572-2600 - verbatim, same order.
        String err = null;
        if (docNo == 0)                 err = "document Number Field Required";
        else if (refDocumentTypeId == 0) err = "ReferenceDocType Field Required";
        else if (refDocNo == 0)          err = "Doc No Field Required";
        else if (refDocNoId == 0)        err = "DocNoId Field Required";
        else if (rows.isEmpty())         err = "First row contains No values Of Contractor And Wages Account.";
        if (err != null) {
            res.put("success", false);
            res.put("message", err);
            return res;
        }

        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        int userId = currentUserContext.currentUserId();
        Timestamp now = new Timestamp(System.currentTimeMillis());
        boolean isUpdate = id > 0;

        double weightTotal = 0, qtyTotal = 0;
        for (Map<String, Object> r : rows) {
            weightTotal += asDouble(r.get("weight"));
            qtyTotal += asDouble(r.get("qty"));
        }

        Map<String, Object> h = ContractorWagesBillWriter.headerParams();
        h.put("Id", id);
        h.put("OrganizationId", orgId);
        h.put("CompanyId", compId);
        h.put("BranchesId", currentUserContext.currentBranchId());
        h.put("FinancialYearId", currentUserContext.currentFinancialYearId());
        h.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        h.put("DocNo", docNo);
        h.put("DocDate", date(docDate));
        h.put("RefDocumentTypeId", refDocumentTypeId);
        h.put("RefDocNo", refDocNo);
        h.put("RefDocNoId", refDocNoId);
        h.put("RefDocument", str(body.get("refDocument")));
        h.put("StockPartyId", asInt(body.get("stockPartyId")));
        h.put("JobOrderId", asInt(body.get("jobOrderId")));
        h.put("ScaleSlipNo", asInt(body.get("scaleSlipNo")));
        h.put("ProjectsId", asInt(body.get("projectsId")));
        h.put("OtherRemarks", str(body.get("otherRemarks")));
        h.put("WeightTotal", weightTotal);
        h.put("QtyTotal", qtyTotal);
        h.put("EntryUser", userId);
        h.put("ModifyUser", userId);
        h.put("EntryDate", now);
        h.put("ModifyDate", now);

        List<Map<String, Object>> details = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> d = ContractorWagesBillWriter.detailParams();
            d.put("Id", asInt(r.get("id")));
            d.put("ContractorId", asInt(r.get("contractorId")));
            d.put("InvConractorWagesAccountsId", asInt(r.get("wagesAccountId")));
            d.put("InvContractorWagesScheduleId", asInt(r.get("scheduleId")));
            d.put("ItemId", asInt(r.get("itemId")));
            d.put("JobLotId", asInt(r.get("jobLotId")));
            d.put("InvPackingTypeId", asInt(r.get("packingTypeId")));
            d.put("WagesTypeId", asInt(r.get("wagesTypeId")));
            d.put("WareHouseFromId", asInt(r.get("warehouseFromId")));
            d.put("WareHouseToId", asInt(r.get("warehouseToId")));
            d.put("WbTransactionsIdDt", asInt(r.get("wbTransactionsIdDt")));
            d.put("JobOrderId", asInt(r.get("jobOrderId")));
            d.put("RefDocumentTypeId", refDocumentTypeId);
            d.put("RefLineId", asInt(r.get("refLineId")));
            d.put("RefDocDate", date(str(r.get("refDocDate"))));
            d.put("RefDocQty", asDouble(r.get("refDocQty")));
            d.put("RefDocWeight", asDouble(r.get("refDocWeight")));
            d.put("PackSize", asDouble(r.get("packSize")));
            d.put("Qty", asDouble(r.get("qty")));
            d.put("BillQty", asDouble(r.get("billQty")));
            d.put("Weight", asDouble(r.get("weight")));
            d.put("BillWeight", asDouble(r.get("billWeight")));
            d.put("WeightCut", asDouble(r.get("weightCut")));
            d.put("WageRate", asDouble(r.get("wageRate")));
            d.put("RateAddLess", asDouble(r.get("rateAddLess")));
            d.put("WagesAmount", asDouble(r.get("wagesAmount")));
            d.put("FreeOfCost", asBool(r.get("freeOfCost")));
            d.put("IsCompany", asBool(r.get("isCompany")));
            d.put("Crop", str(r.get("crop")));
            d.put("RemarksDetail", str(r.get("remarksDetail")));
            /* Carried for the voucher comment only - MakeVoucher reads WagesAccountName and
               ItemName off the detail row (BLL :224). They are not procedure parameters, so the
               writer strips them: detailParams() has no such keys. */
            d.put("WagesAccountName", str(r.get("wagesAccountName")));
            d.put("ItemName", str(r.get("itemName")));
            details.add(d);
        }

        try {
            /* The rows go to the writer complete - it needs WagesAccountName and ItemName for the
               voucher comment and drops them itself before the detail procedure call. */
            int headerId = writer.save(h, details, isUpdate);
            res.put("success", true);
            res.put("id", headerId);
            res.put("message", (isUpdate ? "Updated SuccessFully  [" : "Saved SuccessFully  [") + docNo + "]");
        } catch (Exception e) {
            LOG.error("Wages bill save failed", e);
            res.put("success", false);
            res.put("message", e.getMessage());
        }
        return res;
    }


    // ---------------------------------------------------------------- config flags

    /**
     * GlobalVariables_Helper.GetConfigValueFromGlobal(name) (Helper :11-15) reads
     * clsGlobalVariables.configrationsAllocation and returns that row's ConfigKey. The same value
     * is read here straight from the two tables the desktop's own config screen uses -
     * ConfigrationsAllocation joined to ConfigrationsDefinition on ConfigDescription - scoped to
     * this organization and company. Read-only; nothing is defaulted into the database.
     *
     * The form depends on these (frmwagesBillHeader.cs :330, :553-566, and the caps at :893-960):
     *
     *   WagesAmountCalculateOnQty   Amount = Qty * Rate when true, else (BillWeight / PackSize) * Rate
     *   EnableAddLessOnWagesRegular shows the RateAddLess / RateWithoutAddLess columns
     *   PercentageForRateAddLess    the cap |RateAddLess| may not exceed, as a % of the schedule rate
     *   ContractorWageComparisonbyActivityFor{GRN,GDN,Forwarding,StockTransfer,
     *                                        ProductionInput,ProductionConsumption}
     *                               each one exempts its own reference document type from the
     *                               "cannot exceed the document's total" caps
     */
    private static final String[] CONFIG_NAMES = {
            "WagesAmountCalculateOnQty",
            "EnableAddLessOnWagesRegular",
            "PercentageForRateAddLess",
            "ContractorWageComparisonbyActivityForGRN",
            "ContractorWageComparisonbyActivityForGDN",
            "ContractorWageComparisonbyActivityForForwarding",
            "ContractorWageComparisonbyActivityForStockTransfer",
            "ContractorWageComparisonbyActivityForProductionInput",
            "ContractorWageComparisonbyActivityForProductionConsumption"
    };

    public Map<String, Object> getConfigFlags() {
        Map<String, Object> out = new LinkedHashMap<>();
        for (String n : CONFIG_NAMES) out.put(n, "");
        try {
            for (Map<String, Object> r : jdbcTemplate.queryForList(
                    "SELECT D.ConfigDescription, A.ConfigKey "
                  + "FROM ConfigrationsAllocation A "
                  + "JOIN ConfigrationsDefinition D ON A.ConfigrationsDefinitionId = D.Id "
                  + "WHERE A.OrganizationId = ? AND A.CompanyId = ? AND ISNULL(A.IsActive, 1) = 1",
                    currentUserContext.currentOrganizationId(),
                    currentUserContext.currentCompanyId())) {
                String name = str(r.get("ConfigDescription"));
                if (out.containsKey(name)) out.put(name, str(r.get("ConfigKey")));
            }
        } catch (Exception e) {
            LOG.error("Wages config flags read failed", e);
        }
        return out;
    }

    // ---------------------------------------------------------------- pending data

    /**
     * The "Pending Data For Load" grid. The desktop takes one of two paths (form :387-434):
     *
     *   opened standalone  ->  PendingTicket()      (:2040)
     *                          InvGrn.GetAllPendingRecordsForConractorWages, BLL :2240-2284
     *                          [dbo].[USP_GetAllPendingRecordsForConractorWages]
     *                          @OrganizationId, @CompanyId, and @DocumentTypeId / @Id / @BranchesId
     *                          ONLY when each is non-zero - the BLL omits them otherwise, and
     *                          omitting is not the same as passing NULL.
     *
     *   opened from a doc  ->  PendingGrnAndGdn()   (:1974)
     *                          InvGrn.GetPendingGrnAndGdnForConractorWagesByRefIds, BLL :1514-1557
     *                          Sp_InvContractorWagesBillHeader_GetAllMethod
     *                          @OrganizationId, @CompanyId, @DocumentTypeId, @Id (always, even 0),
     *                          @ReqType only when non-empty,
     *                          @Activity='GetPendingGrnAndGdnForConractorWagesByRefIds'
     *
     * RefLineId is not a column of either result: both methods number the rows 1..n per Id as they
     * copy them into the grid's own table (:2004-2008 / :2071-2075), so that is done here too.
     */
    public List<Map<String, Object>> getPending(int refDocumentTypeId, int refDocId, String reqType) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        List<Map<String, Object>> rows;
        try {
            if (refDocumentTypeId > 0 && refDocId > 0) {
                List<String> names = new ArrayList<>();
                List<Object> args = new ArrayList<>();
                names.add("@OrganizationId"); args.add(orgId);
                names.add("@CompanyId");      args.add(compId);
                names.add("@DocumentTypeId"); args.add(refDocumentTypeId);
                names.add("@Id");             args.add(refDocId);
                if (nz(reqType)) { names.add("@ReqType"); args.add(reqType.trim()); }
                names.add("@Activity");       args.add("GetPendingGrnAndGdnForConractorWagesByRefIds");
                rows = jdbcTemplate.queryForList(exec(PROC, names), args.toArray());
            } else {
                List<String> names = new ArrayList<>();
                List<Object> args = new ArrayList<>();
                names.add("@OrganizationId"); args.add(orgId);
                names.add("@CompanyId");      args.add(compId);
                if (refDocumentTypeId != 0) { names.add("@DocumentTypeId"); args.add(refDocumentTypeId); }
                if (refDocId != 0)          { names.add("@Id");             args.add(refDocId); }
                int branchId = currentUserContext.currentBranchId();
                if (branchId != 0)          { names.add("@BranchesId");     args.add(branchId); }
                rows = jdbcTemplate.queryForList(
                        exec("[dbo].[USP_GetAllPendingRecordsForConractorWages]", names), args.toArray());
            }
        } catch (Exception e) {
            LOG.error("Pending wages documents failed for ref doc type {} id {}",
                      refDocumentTypeId, refDocId, e);
            return Collections.emptyList();
        }

        /* RefLineId: 1..n within each Id, in the order the procedure returned them (:2004-2008). */
        Map<String, Integer> seen = new HashMap<>();
        List<Map<String, Object>> out = new ArrayList<>(rows.size());
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>(r);
            String key = String.valueOf(r.get("Id"));
            int n = seen.merge(key, 1, Integer::sum);
            m.put("RefLineId", n);
            out.add(m);
        }
        return out;
    }

    /**
     * Has this reference document already been billed?  GetIdByRefDocTypeIdAndRefDocId,
     * BLL :816-864 - @OrganizationId, @CompanyId, @RefDocumentTypeId, @RefDocNoId,
     * @FinancialYearId, @ReqType only when non-empty, @Activity='GetIdByRefDocTypeIdAndRefDocId'.
     * A hit switches the form from Save to Update (:2185-2203).
     */
    public Integer getIdByRefDoc(int refDocumentTypeId, int refDocNoId, String reqType) {
        try {
            List<String> names = new ArrayList<>();
            List<Object> args = new ArrayList<>();
            names.add("@OrganizationId");    args.add(currentUserContext.currentOrganizationId());
            names.add("@CompanyId");         args.add(currentUserContext.currentCompanyId());
            names.add("@RefDocumentTypeId"); args.add(refDocumentTypeId);
            names.add("@RefDocNoId");        args.add(refDocNoId);
            names.add("@FinancialYearId");   args.add(currentUserContext.currentFinancialYearId());
            if (nz(reqType)) { names.add("@ReqType"); args.add(reqType.trim()); }
            names.add("@Activity");          args.add("GetIdByRefDocTypeIdAndRefDocId");
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(exec(PROC, names), args.toArray());
            if (!rows.isEmpty()) {
                Object v = rows.get(0).values().iterator().next();
                if (v instanceof Number) return ((Number) v).intValue();
            }
        } catch (Exception e) {
            LOG.error("Wages bill id-by-ref-doc failed for {}/{}", refDocumentTypeId, refDocNoId, e);
        }
        return null;
    }

    /**
     * The rows of the chosen reference document - what LoadDataForWages() (:2121) turns into the
     * Regular / Other wages grids.
     *
     *   InvGrn.GetGrnDetialForContractorWages, BLL :1334-1377
     *   Sp_InvContractorWagesBillHeader_GetAllMethod
     *     @OrganizationId, @CompanyId, @Id, @DocumentTypeId, @ReqType (only when non-empty),
     *     @Activity='GetGrnGdnTransferDetailForContractorWagesByDocumentTypeIdAndId'
     *
     * Each returned row also carries whether its item is free of cost on that date
     * (USP_CheckItemsFreeofcostforWages, BLL :234-281) because the form asks per row, at :2212 and
     * :2221, before deciding "Free Of Cost" or "Regular" - and a free-of-cost row is rated 0.
     *
     * The wages-activity id list for this document type is returned alongside, because
     * accountName(DocumentTypeId) (:523-550) picks it the same way: the defaults keyed on the
     * reference document type, then overridden by USP_WagesTypeIdsAgainstDocumentType_GetAll when
     * it has a row for that type.
     */
    public Map<String, Object> loadRefDoc(int refDocumentTypeId, int refDocId, String reqType) {
        Map<String, Object> out = new LinkedHashMap<>();
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();

        List<Map<String, Object>> detail = new ArrayList<>();
        try {
            List<String> names = new ArrayList<>();
            List<Object> args = new ArrayList<>();
            names.add("@OrganizationId"); args.add(orgId);
            names.add("@CompanyId");      args.add(compId);
            names.add("@Id");             args.add(refDocId);
            names.add("@DocumentTypeId"); args.add(refDocumentTypeId);
            if (nz(reqType)) { names.add("@ReqType"); args.add(reqType.trim()); }
            names.add("@Activity");
            args.add("GetGrnGdnTransferDetailForContractorWagesByDocumentTypeIdAndId");
            detail = jdbcTemplate.queryForList(exec(PROC, names), args.toArray());
        } catch (Exception e) {
            LOG.error("Reference document detail failed for {}/{}", refDocumentTypeId, refDocId, e);
            out.put("error", e.getMessage());
        }

        /* Free-of-cost is per item AND per date, so it is asked once per distinct (date, item)
           rather than once per row - same answer, fewer round trips. WagesAccountId is 0 here,
           exactly as the form passes it at :2212 before an activity has been chosen. */
        Map<String, Boolean> foc = new LinkedHashMap<>();
        List<Map<String, Object>> rows = new ArrayList<>(detail.size());
        for (Map<String, Object> r : detail) {
            Map<String, Object> m = new LinkedHashMap<>(r);
            Object dd = r.get("DocDate");
            String docDate = dd == null ? "" : String.valueOf(dd);
            int itemId = asInt(r.get("ItemId"));
            String key = docDate + "|" + itemId;
            Boolean f = foc.get(key);
            if (f == null) {
                f = checkFreeOfCost(docDate, refDocumentTypeId, itemId, 0);
                foc.put(key, f);
            }
            m.put("IsFreeOfCost", f);
            rows.add(m);
        }
        out.put("detail", rows);
        out.put("wagesActivityIds", wagesActivityIdsFor(refDocumentTypeId));
        return out;
    }

    /** USP_CheckItemsFreeofcostforWages, BLL :234-281. Returns IsFreeocCost (the column's own spelling). */
    public boolean checkFreeOfCost(String docDate, int refDocumentTypeId, int itemId, int wagesAccountId) {
        try {
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                    "EXEC USP_CheckItemsFreeofcostforWages @OrganizationId=?, @CompanyId=?, "
                  + "@ItemId=?, @DocDate=?, @RefDocumentTypeId=?, @WagesAccountId=?",
                    currentUserContext.currentOrganizationId(),
                    currentUserContext.currentCompanyId(),
                    itemId, date(docDate), refDocumentTypeId, wagesAccountId);
            if (!rows.isEmpty()) return asBool(rows.get(0).get("IsFreeocCost"));
        } catch (Exception e) {
            LOG.error("Free-of-cost check failed for item {}", itemId, e);
        }
        return false;
    }

    /**
     * accountName(DocumentTypeId), form :523-550. The defaults below are the form's own, keyed on
     * the reference document type, and USP_WagesTypeIdsAgainstDocumentType_GetAll overrides them
     * when it has a row for that type. Nothing is invented: a type with neither a default nor a
     * row yields null, which is what the form passes on.
     */
    private String wagesActivityIdsFor(int refDocumentTypeId) {
        String ids = null;
        switch (refDocumentTypeId) {
            case 46: case 80: case 143:  ids = "34,36,44"; break;
            case 86: case 112: case 210: ids = "33,36,44"; break;
            case 68: case 806:           ids = "33,34";    break;
            default: break;
        }
        try {
            for (Map<String, Object> r : jdbcTemplate.queryForList(
                    "EXEC [dbo].[USP_WagesTypeIdsAgainstDocumentType_GetAll]")) {
                if (asInt(r.get("DocumentTypeId")) == refDocumentTypeId) {
                    Object v = r.get("WagesActivityIds");
                    if (v != null) ids = String.valueOf(v);
                    break;
                }
            }
        } catch (Exception e) {
            LOG.error("Wages type ids by document type failed", e);
        }
        return ids;
    }

    /** Builds "EXEC <proc> @A=?, @B=?" for a parameter list the BLL decided at run time. */
    private static String exec(String proc, List<String> names) {
        StringBuilder b = new StringBuilder("EXEC ").append(proc).append(' ');
        for (int i = 0; i < names.size(); i++) {
            if (i > 0) b.append(", ");
            b.append(names.get(i)).append("=?");
        }
        return b.toString();
    }

    private static boolean nz(String s) { return s != null && !s.trim().isEmpty(); }

    // ---------------------------------------------------------------- helpers

    private static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }

    private static int asInt(Object o) {
        if (o == null) return 0;
        if (o instanceof Number) return ((Number) o).intValue();
        try { return (int) Double.parseDouble(String.valueOf(o).trim()); } catch (Exception e) { return 0; }
    }

    private static double asDouble(Object o) {
        if (o == null) return 0d;
        if (o instanceof Number) return ((Number) o).doubleValue();
        try { return Double.parseDouble(String.valueOf(o).trim()); } catch (Exception e) { return 0d; }
    }

    private static boolean asBool(Object o) {
        if (o == null) return false;
        if (o instanceof Boolean) return (Boolean) o;
        String s = String.valueOf(o).trim();
        return "1".equals(s) || "true".equalsIgnoreCase(s);
    }

    private static Object date(String s) {
        if (s == null || s.trim().isEmpty()) return null;
        try { return java.sql.Date.valueOf(LocalDate.parse(s.trim().substring(0, 10))); }
        catch (Exception e) { return null; }
    }
}
