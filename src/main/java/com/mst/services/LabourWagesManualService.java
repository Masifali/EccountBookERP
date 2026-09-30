package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.reports.ReportDataService;
import com.mst.repositories.ContractorWagesBillWriter;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Labour Wages Manual - screen 187, frmWagesBillManual, DocumentTypeId 810.
 *
 * Ported method by method from
 *   Architecture.WinApp.Contractor_Wages\frmWagesBillManual.cs   (logic :153-2666)
 *   Architecture.WinApp.Common\CommonServices.cs                 (the helpers it calls)
 *   Architecture.BLL/DAL.ContractorWages.InvContractorWagesBillHeader (GenerateCode, GetByID,
 *   DocumentTypeForManualWages, FormHistoryNew, Save, ContractorWagesBill_SlipandRegister)
 *
 * Every procedure and activity below was checked against procdure.sql:
 *
 *   rights           Sp_tblUserRights_GetAllMethod 'GetByUserId'  ScreenName 'frmWagesBillManual'
 *   ERP feature 8    USP_GetERPFeaturesByCompanyId               (Party_ProcessingFeature)
 *   config           Sp_ConfigrationsAllocation_GetAllMethod 'GetConfigurationByOrgCompandConfigDescription'
 *   contractors      Sp_SupplierCustomer_GetAllMethod 'ReadByOrganizationCompanyIdForContractorWages'
 *   stock party      Sp_SupplierCustomer_GetAllMethod 'GetSupplierustomerForPartyProcessing'
 *   wages accounts   Sp_InvConractorWagesAccounts_GetAllMethod 'GetWagesItemsByWagesTypeIds' @WagesLookupIds='50' @ActionId=1
 *   items            Sp_Item_GetAllMethod 'ReadAllItems'
 *   packing          Sp_InvPackingType_GetAllMethod 'ReadAll'
 *   warehouses       Sp_InvWareHouse_GetAllMethod 'ReadByOrganizationCompanyId'
 *   crop years       Sp_InvCropYear_GetAllMethod 'ReadAll'
 *   job lots         SP_JobLot_ReadMethod 'GetAll'
 *   job orders       Sp_InvProductionJobOrder_GetAllMethod 'GetJobOrderNoForInvFoodProduction'
 *   document types   Sp_InvContractorWagesBillHeader_GetAllMethod 'DocumentTypeForManualWages'
 *   doc no           Sp_InvContractorWagesBillHeader_GetAllMethod 'GenerateCode' @RefDocumentTypeId=810
 *   read             Sp_InvContractorWagesBillHeader_GetAllMethod 'ReadById' + 'ReadDetailByHeaderId'
 *   free of cost     USP_CheckItemsFreeofcostforWages
 *   rate             Sp_InvContractorWagesSchedule_GetAllMethod 'GetWagesScheduleRateByEffectiveDateWagesAccountIdandPackSize'
 *   history          USP_ContractorWagesBillHeader_FormHistory
 *   slip 004         Sp_InvContractorWagesBillHeader_SlipandRegister (registry key wages-004)
 *   voucher 118      Sp_Vouchers_GetMethods 'GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId' -> acc-118
 *   save             ContractorWagesBillWriter (DAL SetData + BLL MakeVoucher)
 *
 * DESKTOP QUIRKS KEPT (see the project doc for each one):
 *   Q1  Insert: PackSize is Conversion.ToInt(cell TEXT) - a fractional pack size (12.5) is saved as 0.
 *   Q2  Insert: Qty is refused when Conversion.ToInt(value) == 0 - Convert.ToInt32(double) rounds
 *       half to even, so 0.4 or 0.5 is "Qty Field required".
 *   Q3  Insert: WageRate is parsed back from the Rate cell's TEXT, i.e. the rate rounded to the
 *       rate format's decimals.
 *   Q4  Insert: IsCompany is forced true for every row when ERP feature 8 is off.
 *   Q5  Slip 004 always sends @FreeOfCost = 0 (the DAL's ApprovedFilter is null, so the IsApproved
 *       false is sent) - free-of-cost rows never print.
 *   Q6  History detail and read use the unscoped ReadById (the web adds the tenancy guard below).
 *
 * WEB-ONLY GUARDS (not in the desktop, with reason):
 *   G1  A read / update / print of an id checks the header's OrganizationId and CompanyId against
 *       the session (the desktop only ever got ids from its own grid).
 *   G2  Save and Update re-check the Save / Update right on the server.
 */
@Service
public class LabourWagesManualService {

    private static final Logger LOG = LoggerFactory.getLogger(LabourWagesManualService.class);

    public static final int DOCUMENT_TYPE_ID = 810;
    public static final String SCREEN_NAME = "frmWagesBillManual";
    private static final int PARTY_PROCESSING_FEATURE = 8;
    private static final String PROC = "Sp_InvContractorWagesBillHeader_GetAllMethod";

    @Autowired private JdbcTemplate jdbc;
    @Autowired private CurrentUserContext ctx;
    @Autowired private ContractorWagesBillWriter writer;
    @Autowired private ReportDataService reportData;

    // ============================================================================ load

    /** frmWagesBillManualLoad :171 - rights, feature 8, the config, every combo and the doc no. */
    public Map<String, Object> load() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("canSave", hasRight("Save"));
        out.put("canUpdate", hasRight("Update"));
        out.put("canPrint", hasRight("Print"));
        out.put("partyProcessing", erpFeature(u, PARTY_PROCESSING_FEATURE));
        out.putAll(lists(u, true));
        out.put("docNo", generateDocNo());
        return out;
    }

    /**
     * btnRefresh_Click :1332 - config, contractors (twice in the desktop), wages accounts, items,
     * packing, warehouses, crop, job lot, document types. Stock party is NOT refreshed.
     */
    public Map<String, Object> refresh() {
        return lists(ctx.requireAccountingUser(), false);
    }

    private Map<String, Object> lists(UserAccount u, boolean withStockParty) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("wagesAmountCalculateOnQty", toBool(config(u, "WagesAmountCalculateOnQty")));   // :686
        out.put("amountDecimals", amountDecimalsRaw(u));
        out.put("rateDecimals", rateDecimals(u));
        out.put("contractors", pick(rows("Sp_SupplierCustomer_GetAllMethod", orgComp(u,
                "ReadByOrganizationCompanyIdForContractorWages")), "Id", "CompanyName"));       // :320
        out.put("wagesAccounts", pick(wagesAccounts(u), "Id", "WagesAccountName"));             // :653
        List<Map<String, Object>> items = new ArrayList<>();                                     // :356
        for (Map<String, Object> r : rows("Sp_Item_GetAllMethod", orgComp(u, "ReadAllItems"))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ci(r, "Id"));
            m.put("ItemName", ci(r, "ItemName"));
            m.put("ItemCode", ci(r, "ItemCodeNew"));
            items.add(m);
        }
        out.put("items", items);
        LinkedHashMap<String, Object> pk = new LinkedHashMap<>();                                // :405
        pk.put("@Activity", "ReadAll");
        out.put("packingTypes", pick(rows("Sp_InvPackingType_GetAllMethod", pk), "Id", "PackTypeDesc"));
        out.put("warehouses", pick(rows("Sp_InvWareHouse_GetAllMethod",
                orgComp(u, "ReadByOrganizationCompanyId")), "Id", "WareHouseName"));            // :438
        out.put("cropYears", pick(rows("Sp_InvCropYear_GetAllMethod", orgComp(u, "ReadAll")), "Id", "CropYear")); // :486
        out.put("jobLots", pick(rows("SP_JobLot_ReadMethod", orgComp(u, "GetAll")), "Id", "JobLotDescription"));  // :566
        out.put("documentTypes", documentTypes());                                               // :519
        if (withStockParty) {
            out.put("stockParties", pick(rows("Sp_SupplierCustomer_GetAllMethod",
                    orgComp(u, "GetSupplierustomerForPartyProcessing")), "Id", "CompanyName")); // :270
        }
        return out;
    }

    /** CommonServices.GetWagesAccount("50", 0, 1): @WagesLookupIds when not empty, @WagesActivityId / @ActionId when > 0. */
    private List<Map<String, Object>> wagesAccounts(UserAccount u) {
        LinkedHashMap<String, Object> p = new LinkedHashMap<>();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@WagesLookupIds", "50");
        p.put("@ActionId", 1);
        p.put("@Activity", "GetWagesItemsByWagesTypeIds");
        return rows("Sp_InvConractorWagesAccounts_GetAllMethod", p);
    }

    /** DocumentTypeFill :519 -> BLL DocumentTypeForManualWages: @BranchesId only when != 0. */
    public List<Map<String, Object>> documentTypes() {
        UserAccount u = ctx.requireAccountingUser();
        LinkedHashMap<String, Object> p = new LinkedHashMap<>();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        int branch = nz(u.getBranchesId());
        if (branch != 0) p.put("@BranchesId", branch);
        p.put("@Activity", "DocumentTypeForManualWages");
        return pick(rows(PROC, p), "Id", "DocumentType");
    }

    /** JobOrderNoFill :599 - Org, Company, FinancialYearId (ActiveYr) -> Id / PlanCode. */
    public List<Map<String, Object>> jobOrders() {
        UserAccount u = ctx.requireAccountingUser();
        LinkedHashMap<String, Object> p = new LinkedHashMap<>();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        int fy = financialYear();
        if (fy != 0) p.put("@FinancialYearId", fy);
        p.put("@Activity", "GetJobOrderNoForInvFoodProduction");
        return pick(rows("Sp_InvProductionJobOrder_GetAllMethod", p), "Id", "PlanCode");
    }

    /** GenerateDocNo :299 - RefDocumentTypeId 810; the text box is only set when code > 0. */
    public int generateDocNo() {
        UserAccount u = ctx.requireAccountingUser();
        LinkedHashMap<String, Object> p = new LinkedHashMap<>();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@RefDocumentTypeId", DOCUMENT_TYPE_ID);
        p.put("@FinancialYearId", financialYear());
        p.put("@Activity", "GenerateCode");
        List<Map<String, Object>> r = rows(PROC, p);
        return r.isEmpty() ? 0 : toInt(ci(r.get(0), "DocNo"));
    }

    // ============================================================================ rate helpers

    /** CommonServices.CheckItemsFreeofcostforWages - true is "Free Of Cost". */
    public boolean freeOfCost(String docDate, int refDocumentTypeId, int itemId, int wagesAccountId) {
        UserAccount u = ctx.requireAccountingUser();
        List<Map<String, Object>> r = jdbc.queryForList(
                "EXEC dbo.USP_CheckItemsFreeofcostforWages @OrganizationId=?, @CompanyId=?, "
              + "@ItemId=?, @DocDate=?, @RefDocumentTypeId=?, @WagesAccountId=?",
                u.getOrganizationId(), u.getCompanyId(), itemId, dateTime(docDate), refDocumentTypeId, wagesAccountId);
        return !r.isEmpty() && toBool(ci(r.get(0), "IsFreeocCost"));
    }

    /** CommonServices.GetWagesRate - {rate, scheduleId}; a miss is 0 / 0. */
    public Map<String, Object> wagesRate(String docDate, double packSize, int wagesAccountId, int contractorId) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        double rate = 0;
        int scheduleId = 0;
        List<Map<String, Object>> r = jdbc.queryForList(
                "EXEC dbo.Sp_InvContractorWagesSchedule_GetAllMethod @OrganizationId=?, @CompanyId=?, "
              + "@InvConractorWagesAccountsId=?, @ContractorId=?, @EffectedDate=?, @PackUomFrom=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), wagesAccountId, contractorId, dateTime(docDate), packSize,
                "GetWagesScheduleRateByEffectiveDateWagesAccountIdandPackSize");
        if (!r.isEmpty()) {
            rate = toDouble(ci(r.get(0), "WageRate"));
            scheduleId = toInt(ci(r.get(0), "Id"));
        }
        out.put("rate", rate);
        out.put("scheduleId", scheduleId);
        return out;
    }

    // ============================================================================ read

    /**
     * ReadById :1577 / HistoryDetailGridBind :2033 -> GetByID. "Record Not found" when the header
     * is missing or has no detail rows (the form checks invContractWagesBillDateil.Count > 0).
     */
    public Map<String, Object> getById(int id) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> head = header(u, id);
        if (head == null) throw new IllegalArgumentException("Record Not found");
        LinkedHashMap<String, Object> p = new LinkedHashMap<>();
        p.put("@Id", id);
        p.put("@Activity", "ReadDetailByHeaderId");
        List<Map<String, Object>> details = rows(PROC, p);
        if (details.isEmpty()) throw new IllegalArgumentException("Record Not found");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", head);
        out.put("details", details);
        return out;
    }

    /** ReadById header row, scoped to this session's organization and company (G1). */
    private Map<String, Object> header(UserAccount u, int id) {
        if (id <= 0) return null;
        LinkedHashMap<String, Object> p = new LinkedHashMap<>();
        p.put("@Id", id);
        p.put("@Activity", "ReadById");
        List<Map<String, Object>> r = rows(PROC, p);
        if (r.isEmpty()) return null;
        Map<String, Object> h = r.get(0);
        if (toInt(ci(h, "OrganizationId")) != nz(u.getOrganizationId())
                || toInt(ci(h, "CompanyId")) != nz(u.getCompanyId())) return null;
        return h;
    }

    // ============================================================================ history

    /**
     * bindHistory :1865 -> FormHistoryNew: @OrganizationId, @CompanyId, @DocumentTypeId=810 always;
     * @FinancialYearId, @BranchesId, @RefDocumentTypeId when non-zero; @FromDate / @ToDate when the
     * picker is checked; @FromDocNo / @ToDocNo when non-zero. Rows are re-shaped as the form does.
     */
    public List<Map<String, Object>> history(String fromDate, String toDate, String fromDocNo, String toDocNo,
                                             int refDocumentTypeId) {
        UserAccount u = ctx.requireAccountingUser();
        LinkedHashMap<String, Object> p = new LinkedHashMap<>();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@DocumentTypeId", DOCUMENT_TYPE_ID);
        int fy = financialYear();
        if (fy != 0) p.put("@FinancialYearId", fy);
        int branch = nz(u.getBranchesId());
        if (branch != 0) p.put("@BranchesId", branch);
        Object from = dateTime(fromDate), to = dateTime(toDate);
        if (from != null) p.put("@FromDate", from);
        if (to != null) p.put("@ToDate", to);
        int fdn = netToInt(fromDocNo), tdn = netToInt(toDocNo);
        if (fdn != 0) p.put("@FromDocNo", fdn);
        if (tdn != 0) p.put("@ToDocNo", tdn);
        if (refDocumentTypeId != 0) p.put("@RefDocumentTypeId", refDocumentTypeId);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows("[dbo].[USP_ContractorWagesBillHeader_FormHistory]", p)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("RecordNo", ci(r, "RecordNo"));
            m.put("Id", ci(r, "Id"));
            m.put("DocumentTypeId", ci(r, "DocumentTypeId"));
            m.put("RefDocumentTypeId", ci(r, "RefDocumentTypeId"));
            m.put("DocumentType", ci(r, "DocumentTypeDescription"));
            m.put("StockParty", ci(r, "StockParty"));
            m.put("DocDate", ci(r, "DocDate"));
            m.put("DocNo", ci(r, "DocNo"));
            m.put("QtyTotal", ci(r, "QtyTotal"));
            m.put("WeightTotal", ci(r, "WeightTotal"));
            m.put("WagesAmount", ci(r, "WagesAmount"));
            m.put("Remarks", ci(r, "OtherRemarks"));
            m.put("EntryDate", ci(r, "EntryDate"));
            m.put("EntryUser", ci(r, "UserName"));
            m.put("ModifyDate", ci(r, "ModifyDate"));
            m.put("ModifyUser", ci(r, "ModifyUser"));
            out.add(m);
        }
        return out;
    }

    // ============================================================================ prints

    /** CommonServices.ContractorWagesBillManualSlip004 :7531 - the checks before the viewer opens. */
    public Map<String, Object> slipCheck(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (id <= 0 || header(u, id) == null) throw new IllegalArgumentException("No Record Found For Display");
        Map<String, Object> args = new LinkedHashMap<>();
        args.put("id", id);
        Object rows = reportData.run("wages-004", args).get("rows");
        if (!(rows instanceof List) || ((List<?>) rows).isEmpty())
            throw new IllegalArgumentException("No Record Found For Display");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("key", "wages-004");
        out.put("args", args);
        return out;
    }

    /**
     * History "Voucher" :2016 -> VoucherReport_118(VoucherHeadIdGet(Id, 810), 810):
     * 0 -> "VoucherId Not Found"; no rows -> "No Record Found For Display".
     */
    public Map<String, Object> voucherCheck(int id) {
        UserAccount u = ctx.requireAccountingUser();
        int vh = 0;
        if (id > 0 && header(u, id) != null) {
            List<Map<String, Object>> r = jdbc.queryForList(
                    "EXEC dbo.Sp_Vouchers_GetMethods @Activity=?, @OrganizationId=?, @CompanyId=?, "
                  + "@DocumentTypeId=?, @DocumentTypeSrNo=?",
                    "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId",
                    u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, id);
            if (!r.isEmpty()) vh = toInt(ci(r.get(0), "Id"));
        }
        if (vh == 0) throw new IllegalArgumentException("VoucherId Not Found");
        if (vh < 0) throw new IllegalArgumentException("Record Not Found For Display because VoucherHeadId not found");
        Map<String, Object> args = new LinkedHashMap<>();
        args.put("id", vh);
        args.put("documentTypeId", DOCUMENT_TYPE_ID);
        Object rows = reportData.run("acc-118", args).get("rows");
        if (!(rows instanceof List) || ((List<?>) rows).isEmpty())
            throw new IllegalArgumentException("No Record Found For Display");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("key", "acc-118");
        out.put("args", args);
        return out;
    }

    // ============================================================================ save

    /**
     * Insert :1377 from the point after the confirm (the page shows the confirm first, exactly
     * where the desktop does). Header, rows and every per-row message are the form's own.
     */
    @Transactional
    public Map<String, Object> save(Map<String, Object> body) {
        UserAccount u = ctx.requireAccountingUser();
        int recId = toInt(body.get("id"));
        boolean isUpdate = recId > 0;

        if (isUpdate ? !hasRight("Update") : !hasRight("Save"))                                   // G2
            throw new IllegalStateException("You do not have the " + (isUpdate ? "Update" : "Save")
                    + " right on Labour Wages Manual.");
        if (isUpdate && header(u, recId) == null) throw new IllegalArgumentException("Record Not found");  // G1

        @SuppressWarnings("unchecked")
        List<Map<String, Object>> rows = body.get("details") instanceof List
                ? (List<Map<String, Object>>) body.get("details") : new ArrayList<>();
        if (rows.isEmpty()) throw new IllegalArgumentException("Regular Wages Grid... Record Not Found");  // :1383

        // FormValidation :1181
        String docNoText = str(body.get("docNo"));
        int refDocTypeId = toInt(body.get("refDocumentTypeId"));
        int jobOrderId = toInt(body.get("jobOrderId"));
        if (docNoText.isEmpty() || "0".equals(docNoText)) throw new IllegalArgumentException("Document Number Field Required");
        if (str(body.get("refDocument")).isEmpty() || refDocTypeId == 0) throw new IllegalArgumentException("Document Type Field Required");
        if ((refDocTypeId == 112 || refDocTypeId == 80) && jobOrderId == 0)
            throw new IllegalArgumentException("JobOrder Field Required when Document Type is Production");

        boolean partyProcessing = erpFeature(u, PARTY_PROCESSING_FEATURE);
        boolean onQty = toBool(config(u, "WagesAmountCalculateOnQty"));
        int amountDigits = amountDecimalsRaw(u);
        int rateDigits = rateDecimals(u);

        // :1402-1416 - a third-party row needs a stock party
        boolean allCompany = true;
        for (Map<String, Object> r : rows) if (!toBool(r.get("isCompany"))) { allCompany = false; break; }
        int stockPartyId = toInt(body.get("stockPartyId"));
        if (!allCompany && stockPartyId == 0)
            throw new IllegalArgumentException("StockParty Field required For third Party Rows In Regular Wages Grid...");

        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        int userId = nz(u.getId());
        Map<String, Object> h = ContractorWagesBillWriter.headerParams();
        h.put("Id", recId);
        h.put("CompanyId", u.getCompanyId());
        h.put("OrganizationId", u.getOrganizationId());
        h.put("BranchesId", nz(u.getBranchesId()));
        h.put("FinancialYearId", financialYear());
        h.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        h.put("EntryUser", userId);
        h.put("ModifyUser", userId);
        h.put("ModifyDate", now);
        h.put("EntryDate", now);
        if (toBool(body.get("isApproved"))) {
            h.put("IsAproved", true);
            h.put("ApprovedDate", now);
            h.put("ApprovedUserId", userId);
        } else {
            h.put("IsAproved", false);
        }
        h.put("DocNo", netToInt(docNoText));
        h.put("DocDate", dateTime(str(body.get("docDate"))));
        h.put("RefDocumentTypeId", refDocTypeId);
        h.put("JobOrderId", jobOrderId);
        h.put("RefDocument", str(body.get("refDocument")));
        h.put("OtherRemarks", body.get("otherRemarks") == null ? "" : String.valueOf(body.get("otherRemarks")));
        h.put("StockPartyId", stockPartyId);

        double qtyTotal = 0, weightTotal = 0;
        List<Map<String, Object>> details = new ArrayList<>();
        for (int i = 0; i < rows.size(); i++) {
            Map<String, Object> r = rows.get(i);
            String rowNo = String.valueOf(i + 1);
            Map<String, Object> d = ContractorWagesBillWriter.detailParams();
            d.put("Id", toInt(r.get("id")));
            d.put("RefDocDate", dateTime(str(r.get("date"))));
            int contractorId = toInt(r.get("contractorId"));
            if (contractorId == 0 || str(r.get("contractorText")).isEmpty())
                throw new IllegalArgumentException("ContractorAccount Field required In Regular Wages Grid... and Row No is " + rowNo);
            d.put("ContractorId", contractorId);
            int wagesAccountId = toInt(r.get("wagesAccountId"));
            if (wagesAccountId == 0)
                throw new IllegalArgumentException("WagesAccount Field required In Regular Wages Grid... and Row No is " + rowNo);
            d.put("InvConractorWagesAccountsId", wagesAccountId);
            d.put("WagesAccountName", str(r.get("wagesAccountText")));
            d.put("ItemName", "");                                   // wd.ItemName is never set on this form

            double packValue = toDouble(r.get("packSize"));
            String packText = netDoubleText(r.get("packSize"));
            if (packValue == 0.0 || packText.isEmpty())
                throw new IllegalArgumentException("PackSize Field required In Regular Wages Grid... and Row No is " + rowNo);
            int packSize = netToInt(packText);                                                    // Q1
            d.put("PackSize", packSize);

            double qtyValue = toDouble(r.get("qty"));
            if (bankersInt(qtyValue) == 0 || netDoubleText(r.get("qty")).isEmpty())                // Q2
                throw new IllegalArgumentException("Qty Field required In Regular Wages Grid... and Row No is " + rowNo);
            d.put("Qty", qtyValue);
            d.put("BillQty", qtyValue);
            qtyTotal += qtyValue;

            double weightValue = toDouble(r.get("billWeight"));
            if (weightValue == 0.0 || netDoubleText(r.get("billWeight")).isEmpty())
                throw new IllegalArgumentException("BillWeight Field required In Regular Wages Grid... and Row No is " + rowNo);
            /* wd.Weight is parsed from the cell text ("##,#.####"); BillWeight is the value. */
            double weightFromText = roundHalfUp(weightValue, 4);
            d.put("Weight", weightFromText);
            d.put("BillWeight", weightValue);
            weightTotal += weightFromText;

            boolean foc = "Free Of Cost".equals(str(r.get("wagesType")));
            double wageRate = 0, amount = 0;
            if (!foc) {
                wageRate = roundHalfUp(toDouble(r.get("rate")), rateDigits);                     // Q3
                if (wageRate == 0.0)
                    throw new IllegalArgumentException("Rate Field required In Wages Grid... and Row No is " + rowNo);
                if (amountDigits < 0 || amountDigits > 15)
                    throw new IllegalArgumentException("Rounding digits must be between 0 and 15, inclusive.\r\nParameter name: digits");
                if (onQty) {
                    amount = roundHalfUp(qtyValue * wageRate, amountDigits);
                } else {
                    if (packSize == 0)
                        throw new IllegalArgumentException("Row No " + rowNo + ": Pack Size " + packText
                                + " is saved as 0 (Conversion.ToInt of the cell text), so Weight / PackSize has no value.");
                    amount = roundHalfUp(weightFromText / packSize * wageRate, amountDigits);
                }
            }
            d.put("WageRate", wageRate);
            d.put("WagesAmount", amount);

            d.put("ItemId", toInt(r.get("itemId")));
            d.put("RefDocumentTypeId", refDocTypeId);
            d.put("JobOrderId", jobOrderId);
            d.put("Crop", str(r.get("cropText")));
            d.put("InvPackingTypeId", toInt(r.get("packingTypeId")));
            d.put("JobLotId", toInt(r.get("jobLotId")));
            d.put("WareHouseFromId", toInt(r.get("wareHouseFromId")));
            d.put("WareHouseToId", toInt(r.get("wareHouseToId")));
            boolean isCompany = toBool(r.get("isCompany"));
            int scheduleId = toInt(r.get("wagesScheduleId"));
            if (foc) {
                d.put("WagesTypeId", 1);
                d.put("FreeOfCost", true);
                scheduleId = 0;
            } else {
                d.put("WagesTypeId", 2);
                d.put("FreeOfCost", false);
            }
            d.put("InvContractorWagesScheduleId", scheduleId);
            if (!partyProcessing) isCompany = true;                                               // Q4
            d.put("IsCompany", isCompany);
            details.add(d);
        }
        h.put("QtyTotal", qtyTotal);
        h.put("WeightTotal", weightTotal);

        int code = writer.save(h, details, isUpdate);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", code);
        out.put("message", (isUpdate ? "Updated SuccessFully  [" : "Saved SuccessFully  [") + h.get("DocNo") + "]");
        return out;
    }

    // ============================================================================ rights / config

    private boolean hasRight(String rightName) {
        String role = ctx.currentRoleName();
        if ("Admin".equals(role)) return true;
        try {
            for (Map<String, Object> r : jdbc.queryForList(
                    "EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?, @ScreenName=?, @RightName=?, "
                  + "@CompanyId=?, @Activity=?",
                    ctx.currentUserId(), SCREEN_NAME, role == null ? "" : role, ctx.currentCompanyId(), "GetByUserId")) {
                Object name = ci(r, "RightName");
                if (name != null && rightName.equals(String.valueOf(name).trim())) return toBool(ci(r, "Value"));
            }
        } catch (Exception e) {
            LOG.warn("Could not read the '{}' right for {}; denying", rightName, SCREEN_NAME, e);
        }
        return false;
    }

    private boolean erpFeature(UserAccount u, int id) {
        try {
            for (Map<String, Object> r : jdbc.queryForList("EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?, @CompanyId=?",
                    u.getOrganizationId(), u.getCompanyId())) {
                if (toInt(ci(r, "Id")) == id) return true;
            }
        } catch (Exception e) {
            LOG.warn("ERP features could not be read; feature {} treated as off", id, e);
        }
        return false;
    }

    private String config(UserAccount u, String name) {
        LinkedHashMap<String, Object> p = new LinkedHashMap<>();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@ConfigDescription", name);
        p.put("@Activity", "GetConfigurationByOrgCompandConfigDescription");
        List<Map<String, Object>> r = rows("dbo.Sp_ConfigrationsAllocation_GetAllMethod", p);
        if (r.isEmpty()) return "";
        Object v = ci(r.get(0), "ConfigKey");
        return v == null ? "" : String.valueOf(v).trim();
    }

    /** clsGlobalVariables.DefaultNoofDecimalPointsForAmount = Conversion.ToInt(config) - used as is by Math.Round. */
    private int amountDecimalsRaw(UserAccount u) {
        return netToInt(config(u, "Default NoofDecimal Points For Amount"));
    }

    /** DecimalRateFormate "#,#0." + 1..4 zeros; 0 gives "00"; anything else no decimals. */
    private int rateDecimals(UserAccount u) {
        int n = netToInt(config(u, "Default NoofDecimal Points For Rate"));
        if (n == 0) return 2;
        return (n >= 1 && n <= 4) ? n : 0;
    }

    private int financialYear() {
        try { return ctx.currentFinancialYearId(); } catch (Exception e) { return 0; }
    }

    // ============================================================================ jdbc helpers

    private LinkedHashMap<String, Object> orgComp(UserAccount u, String activity) {
        LinkedHashMap<String, Object> p = new LinkedHashMap<>();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@Activity", activity);
        return p;
    }

    private List<Map<String, Object>> rows(String proc, LinkedHashMap<String, Object> p) {
        StringBuilder sql = new StringBuilder("EXEC ").append(proc);
        List<Object> args = new ArrayList<>();
        boolean first = true;
        for (Map.Entry<String, Object> e : p.entrySet()) {
            sql.append(first ? " " : ", ").append(e.getKey()).append("=?");
            args.add(e.getValue());
            first = false;
        }
        return jdbc.queryForList(sql.toString(), args.toArray());
    }

    private static List<Map<String, Object>> pick(List<Map<String, Object>> src, String id, String text) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : src) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put(id, ci(r, id));
            m.put(text, ci(r, text));
            out.add(m);
        }
        return out;
    }

    static Object ci(Map<String, Object> row, String key) {
        if (row == null) return null;
        if (row.containsKey(key)) return row.get(key);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey().equalsIgnoreCase(key)) return e.getValue();
        return null;
    }

    // ============================================================================ Conversion.*

    private static int nz(Integer i) { return i == null ? 0 : i; }

    private static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }

    /** Conversion.ToInt(object) - numbers round half to even; text must be a whole number. */
    static int toInt(Object o) {
        if (o == null) return 0;
        if (o instanceof Boolean) return ((Boolean) o) ? 1 : 0;
        if (o instanceof Number) return bankersInt(((Number) o).doubleValue());
        return netToInt(String.valueOf(o));
    }

    /** Convert.ToInt32(string): whole numbers only (with sign / thousands separators); anything else 0. */
    static int netToInt(String s) {
        if (s == null) return 0;
        String t = s.trim().replace(",", "");
        if (!t.matches("[+-]?\\d+")) return 0;
        try { return Integer.parseInt(t.startsWith("+") ? t.substring(1) : t); } catch (Exception e) { return 0; }
    }

    static int bankersInt(double d) {
        if (Double.isNaN(d) || Double.isInfinite(d)) return 0;
        return (int) Math.rint(d);
    }

    static double toDouble(Object o) {
        if (o == null) return 0;
        if (o instanceof Number) return ((Number) o).doubleValue();
        try { return Double.parseDouble(String.valueOf(o).trim().replace(",", "")); } catch (Exception e) { return 0; }
    }

    static boolean toBool(Object o) {
        if (o == null) return false;
        if (o instanceof Boolean) return (Boolean) o;
        if (o instanceof Number) return ((Number) o).intValue() != 0;
        String s = String.valueOf(o).trim();
        return "true".equalsIgnoreCase(s) || "1".equals(s);
    }

    /** A double cell's Text as .NET prints it (no format string): "50", "12.5", "" for null. */
    private static String netDoubleText(Object o) {
        if (o == null || String.valueOf(o).trim().isEmpty()) return "";
        double d = toDouble(o);
        if (d == Math.rint(d) && Math.abs(d) < 1e15) return String.valueOf((long) d);
        return BigDecimal.valueOf(d).stripTrailingZeros().toPlainString();
    }

    /** Math.Round(x, n, AwayFromZero) and .NET's "0.00" text rounding. */
    static double roundHalfUp(double v, int n) {
        if (Double.isNaN(v) || Double.isInfinite(v)) return v;
        return new BigDecimal(Double.toString(v)).setScale(Math.max(0, n), RoundingMode.HALF_UP).doubleValue();
    }

    /** A picker value: "yyyy-MM-dd" gets the current time of day, as a DateTimePicker's Value has. */
    private static Timestamp dateTime(String s) {
        if (s == null || s.trim().isEmpty()) return null;
        String t = s.trim();
        try {
            if (t.length() > 10) return Timestamp.valueOf(LocalDateTime.parse(t.length() > 19 ? t.substring(0, 19) : t));
            return Timestamp.valueOf(LocalDateTime.of(LocalDate.parse(t), LocalTime.now().withNano(0)));
        } catch (Exception e) {
            return null;
        }
    }
}
