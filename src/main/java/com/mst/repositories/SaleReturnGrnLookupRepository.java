package com.mst.repositories;

import com.mst.security.CurrentUserContext;
import java.sql.Types;
import java.util.*;
import org.springframework.http.HttpStatus;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.SqlParameterValue;
import org.springframework.stereotype.Repository;
import org.springframework.web.server.ResponseStatusException;
import static com.mst.repositories.PurchaseGrnWriteRepository.number;

/**
 * SaleReturnGrn.cs (Grn (Sale Return), screen 866, DocumentTypeId 143) - the lookups that differ
 * from InvFrmGRN (46), each with the desktop's own procedure and parameters.
 *
 *   configs()          GetConfigurationsFromGlobal :670 + GetConfigurationsFromGlobalAndBindValuesInColumns :692
 *   wagesActive()      GlobalVariables_Helper.GetWagesRefDocumentsStatusById(143) :674
 *   bagConditions()    EmptyBagsTypeAndConditionDbCall :2649 - StaticColumnsService("EmptyBagsCondition")
 *   bagItems()         EmptyBagsGridComboBind :2661 - getGlobalAllItems where ItemTypeOfTypeId == 14
 *   historySuppliers() InitializeComponentMethod :559 / btnRefreshHistory_Click :3355 - USP_GetDataForDropDownFromGrn
 *   loadGatePass()     GatePassRecordFill :2968 - GatePassInward.LoadGpDataForSaleReturnGRN (USP_LoadGpDataForSaleReturnGRN)
 *   pendingGdn()       btnLoadGdn_Click :3996 frmLoadGdnForGrnSaleReturn - USP_InvGdn_GetPendingGdnForGrnReturn
 */
@Repository
public class SaleReturnGrnLookupRepository {
    public static final int TYPE = 143;
    public static final List<String> CONFIGS = List.of(
            "WagesCompulsoryOnSaleInvoiceReturn", "ItemSearchByCode", "DefaultDaysToLessFromHistoryFromDate",
            "EmptyBagsWeightCutEditableOnGRN", "AddLessWeightCutEditableOnGRN", "EmptyBagsInofrmationCompulsoryOnGRN",
            "BillWeightAndStockWeightDifferenceTolerance", "Job/Lot", "Default Crop Year", "Paking Type", "Warehouse", "FreightInwardAc");

    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;
    private final PurchaseGrnRecordRepository records;

    public SaleReturnGrnLookupRepository(JdbcTemplate jdbc, CurrentUserContext context, PurchaseGrnRecordRepository records) {
        this.jdbc = jdbc; this.context = context; this.records = records;
    }

    private int org() { return context.currentOrganizationId(); }
    private int company() { return context.currentCompanyId(); }

    /** clsGlobalVariables.configrationsAllocation / GetConfigValueFromGlobal - one call for all names the form reads. */
    public Map<String, String> configs() {
        Map<String, String> values = new LinkedHashMap<>();
        for (String name : CONFIGS) values.put(name, "");
        for (var row : jdbc.queryForList("EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?,@CompanyId=?,@ConfigDescription=?,@Activity='GetMultipleConfigurationsByConfigDescriptions'",
                org(), company(), String.join(",", CONFIGS))) {
            String name = Objects.toString(row.get("ConfigDescription"), "");
            for (String known : CONFIGS) if (known.equalsIgnoreCase(name)) values.put(known, Objects.toString(row.get("ConfigKey"), "").trim());
        }
        return values;
    }

    /** CommonServices.GetERPFeatureById(4) :551. */
    public boolean subsidiaryAccounts() {
        return jdbc.queryForList("EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?,@CompanyId=?", org(), company())
                .stream().anyMatch(r -> number(r.get("Id")) == 4);
    }

    /** GetWagesRefDocumentsStatusById(143): the start-up list USP_GetRefDocumentsForWages (id omitted). */
    public boolean wagesActive() {
        return jdbc.queryForList("EXEC [dbo].[USP_GetRefDocumentsForWages]").stream()
                .anyMatch(r -> number(r.get("RefDocumentTypeId")) == TYPE && PurchaseGrnFormRepository.flag(r.get("IsActive")));
    }

    /** StaticColumnsService("EmptyBagsCondition") - bound with value "Id", display "Type" (:2664). */
    public List<Map<String, Object>> bagConditions() {
        return jdbc.queryForList("EXEC dbo.SpStaticColumnNames @Activity='EmptyBagsCondition'");
    }

    /** getGlobalAllItems (USP_Item_AllItemsWithModal) filtered to ItemTypeOfTypeId 14 (:2661). */
    public List<Map<String, Object>> bagItems() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList("EXEC [dbo].[USP_Item_AllItemsWithModal] @OrganizationId=?,@CompanyId=?", org(), company()))
            if (number(r.get("ItemTypeOfTypeId")) == 14) out.add(r);
        return out;
    }

    /**
     * InvGrn.GetDataForDropDownFromGrn(org, company, "143", "Supplier", branch, 0) - BLL 0576:1978 omits
     * FinancialYearId when 0. Bound "Id" / "ReferenceName" (:662).
     */
    public List<Map<String, Object>> historySuppliers() {
        return jdbc.queryForList("EXEC [dbo].[USP_GetDataForDropDownFromGrn] @OrganizationId=?,@CompanyId=?,@DocumentTypeIds=?,@Activity=?,@BranchesIds=?",
                org(), company(), String.valueOf(TYPE), "Supplier", String.valueOf(context.currentBranchId()));
    }

    /**
     * GatePassRecordFill :2968 - GatePassInward.LoadGpDataForSaleReturnGRN with DocumentTypeId 51.
     * Returns null when the procedure yields no row (desktop then re-enables vehicle fields :3016).
     */
    public Map<String, Object> loadGatePass(int gatePassId) {
        records.requireRight(TYPE, "View");
        var rows = jdbc.queryForList("EXEC [dbo].[USP_LoadGpDataForSaleReturnGRN] @OrganizationId=?,@CompanyId=?,@BranchesId=?,@FinancialYearId=?,@DocumentTypeId=51,@Id=?",
                org(), company(), context.currentBranchId(), context.currentFinancialYearId(), gatePassId);
        return rows.isEmpty() ? null : rows.get(0);
    }

    /** GatepassDataDbCall :2775 - GetPendingInwardGatePassForSaleReturnGrn (BLL 0567:815), DocumentTypeId 51. Reset() re-reads it (:1755). */
    public List<Map<String, Object>> pendingGatePasses() {
        records.requireRight(TYPE, "View");
        return jdbc.queryForList("EXEC [dbo].[USP_PendingInwardGatePassForSaleReturnGrn] @OrganizationId=?,@CompanyId=?,@BranchesId=?,@FinancialYearId=?,@DocumentTypeId=51",
                org(), company(), context.currentBranchId(), context.currentFinancialYearId());
    }

    /** GpSrNo of an in-scope inward gate pass (desktop sends GpNo = CmbGpNo.Text :1289). */
    public int gatePassNumber(int gatePassId) {
        var rows = jdbc.queryForList("SELECT GpSrNo FROM dbo.GatePassInward WHERE Id=? AND OrganizationId=? AND CompanyId=? AND BranchesId=? AND FinancialYearId=? AND DocumentTypeId=51",
                gatePassId, org(), company(), context.currentBranchId(), context.currentFinancialYearId());
        if (rows.isEmpty()) throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Gate pass not found in the current company, branch and financial year");
        Object v = rows.get(0).get("GpSrNo");
        try { return v == null ? 0 : (int) Double.parseDouble(v.toString().trim()); } catch (NumberFormatException e) { return 0; }
    }

    /**
     * frmLoadGdnForGrnSaleReturn data. The loader form's source is not in the workspace; the procedure is
     * the one whose result set carries every column LoadInGridDetailFromGrn :4032 reads (Id, DetailId,
     * DocumentTypeId, GdnNo, BalQty, BalWeight, ItemEquivalent ...). Empty filters are omitted.
     */
    public List<Map<String, Object>> pendingGdn(String fromDate, String toDate, Integer customerId) {
        records.requireRight(TYPE, "View");
        return jdbc.queryForList("EXEC [dbo].[USP_InvGdn_GetPendingGdnForGrnReturn] @OrganizationId=?,@CompanyId=?,@FromDate=?,@ToDate=?,@SupplierCustomerId=?",
                org(), company(), date(fromDate), date(toDate),
                new SqlParameterValue(Types.INTEGER, customerId != null && customerId > 0 ? customerId : null));
    }

    private static SqlParameterValue date(String v) {
        return new SqlParameterValue(Types.DATE, v == null || v.isBlank() ? null : java.sql.Date.valueOf(v.trim().substring(0, 10)));
    }
}
