package com.mst.repositories.fixedassets;

import com.mst.models.UserAccount;
import com.mst.models.fixedassets.FaAssetRegister;
import com.mst.repositories.hrm.HrmProcRepository;
import org.springframework.stereotype.Repository;

import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Procedure calls of the Fixed Assets screens that no existing repository already makes.
 * Each method names the desktop BLL method it reproduces; parameters are sent exactly when the BLL adds them.
 * All procedures / parameters checked in procdure.utf8.sql.
 *
 * 353 frmAssetsRegister - BLL 0423 AssetSchema.Category, BLL 0424 AssetSchema.AssetRegister, DAL 0479:
 *   [asset].[USP_Category_GetAllMethod]      @OrganizationId @CompanyId @Activity @Id
 *   [asset].[USP_AssetRegister_GetAllMethod] @OrganizationId @CompanyId @Activity @Id @CategoryId
 *                                            @EntryFromDate @EntryToDate @ModifyFromDate @ModifyToDate
 *   [Asset].[USP_AssetRegister_InsertAndUpdate] (44 params; model 0738 via SetProc)
 * 918 frmFixedAssetPurchase - the lookups DesktopVoucherSupport does not have (BLL 0654 VoucherHead,
 *   CommonServices.ProjectServiceBind / StaticColumnsService, AccountsAllocateToCustomAccountGroupByDocumentType,
 *   TaxesTypes, TaxScheduleMain):
 *   usp_getLocationType (no params), Sp_Projects_GetAllMethod @OrganizationId @CompanyId @MethodType,
 *   usp_getCustomAccountGroupsByDocumentType @DocumentTypeId, usp_getCustomAccountsByGroupId @CompanyId @CustomGroupId,
 *   Sp_TaxesTypes_GetAllMethod @OrganizationId @CompanyId @Type @Activity, SpStaticColumnNames @Activity,
 *   Sp_Vouchers_GetMethods (GetMultiCurrencyAndLastRate), Sp_TaxSchedule_GetAllMehtod (GetTaxPercentInTaxSchedule).
 */
@Repository
public class FaRepository {

    private static final String CAT = "[asset].[USP_Category_GetAllMethod]";
    private static final String REG = "[asset].[USP_AssetRegister_GetAllMethod]";

    private final HrmProcRepository db;

    public FaRepository(HrmProcRepository db) { this.db = db; }

    // ================================================================== 353 Assets Register

    /** Category.GetCategoryForComboBind(org, company): Id, categoryDescription, categoryCode, GL ids, method, life, rate. */
    public List<Map<String, Object>> categories(UserAccount u) {
        return db.rows(CAT, "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetCategoryForComboBind");
    }

    /** Category.GetAssetItemForComboBind(org, company, categoryId): @Id only when != 0. */
    public List<Map<String, Object>> assetItems(UserAccount u, int categoryId) {
        return db.rows(CAT, "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Id", categoryId != 0 ? categoryId : null, "Activity", "GetAssetItemForComboBind");
    }

    /** AssetRegister.GetAssetDepartment(org, company): Id, DepartmentName. */
    public List<Map<String, Object>> departments(UserAccount u) {
        return db.rows(REG, "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetAssetDepartment");
    }

    /** AssetRegister.GetAssetStatus(): Id, StatusName. */
    public List<Map<String, Object>> statuses() {
        return db.rows(REG, "Activity", "GetAssetStatus");
    }

    /** AssetRegister.GetAssetCondition(): Id, ConditionName. */
    public List<Map<String, Object>> conditions() {
        return db.rows(REG, "Activity", "GetAssetCondition");
    }

    /** AssetRegister.GenerateSerailNoByCategoryId(org, company, categoryId): SerialNo ("000000" format). */
    public String generateSerialNo(UserAccount u, int categoryId) {
        List<Map<String, Object>> r = db.rows("[asset].[USP_AssetRegister_GetAllMethod]", "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "CategoryId", categoryId, "Activity", "GenerateSerailNoByCategoryId");
        if (r.isEmpty() || r.get(0).get("SerialNo") == null) return null;
        return String.valueOf(r.get(0).get("SerialNo"));
    }

    /**
     * AssetRegister.FormHistory(ReportsParameters): @OrganizationId @CompanyId, [@Id when != 0 - the form never
     * sets it], the four date bounds only when set (Conversion.CheckDateTimeNull), @Activity='FormHistory'.
     * obj.ItemCategoryId is set by the form but the BLL never sends it.
     */
    public List<Map<String, Object>> history(UserAccount u, Object entryFrom, Object entryTo, Object modifyFrom, Object modifyTo) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("EntryFromDate", entryFrom);
        p.put("EntryToDate", entryTo);
        p.put("ModifyFromDate", modifyFrom);
        p.put("ModifyToDate", modifyTo);
        p.put("Activity", "FormHistory");
        return db.rows(REG, p);
    }

    /** AssetRegister.GetById(id): @Id, @Activity='ReadById' (GetAllDetail -> [0]). */
    public List<Map<String, Object>> assetById(long id) {
        return db.rows(REG, "Id", id, "Activity", "ReadById");
    }

    /** AssetRegister.Save -> DAL 0479 SetData(obj, "[Asset].[USP_AssetRegister_InsertAndUpdate]"). */
    public int saveAsset(FaAssetRegister m) {
        return db.set("[Asset].[USP_AssetRegister_InsertAndUpdate]", m);
    }

    // ============================================================ 918 Fixed Asset Purchase

    /** VoucherHead.GetLocationType(): usp_getLocationType, no parameters. */
    public List<Map<String, Object>> locationTypes() {
        return db.rows("usp_getLocationType", new LinkedHashMap<>());
    }

    /** CommonServices.ProjectServiceBind() -> Projects.GetAlldt: Sp_Projects_GetAllMethod @MethodType='GetAll'. */
    public List<Map<String, Object>> costCenters(UserAccount u) {
        return db.rows("Sp_Projects_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "MethodType", "GetAll");
    }

    /** AccountsAllocateToCustomAccountGroupByDocumentType.GetCustomAccountGroupsByDocumentType(130, null). */
    public List<Map<String, Object>> customGroups(int documentTypeId) {
        return db.rows("[dbo].[usp_getCustomAccountGroupsByDocumentType]", "DocumentTypeId", documentTypeId);
    }

    /** AccountsAllocateToCustomAccountGroupByDocumentType.GetCustomAccountsByGroup(CompanyId, CustomGroupId). */
    public List<Map<String, Object>> customAccounts(UserAccount u, int customGroupId) {
        return db.rows("[dbo].[usp_getCustomAccountsByGroupId]", "CompanyId", u.getCompanyId(), "CustomGroupId", customGroupId);
    }

    /** TaxesTypes.GetForComboBind(Type = 2): @OrganizationId @CompanyId @Type @Activity='ReadByCombo'. */
    public List<Map<String, Object>> taxTypes(UserAccount u) {
        return db.rows("Sp_TaxesTypes_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Type", 2, "Activity", "ReadByCombo");
    }

    /** CommonServices.StaticColumnsService(activity) -> SpStaticColumnNames @Activity ("FixedAssetEntryType": 1 Purchase, 2 Opening). */
    public List<Map<String, Object>> staticColumns(String activity) {
        return db.rows("SpStaticColumnNames", "Activity", activity);
    }

    /** VoucherHead.GetLastExchangeRateAndCurrencyOfVoucher: @DocumentTypeIds, @DMultiCurrencyIds, 'GetMultiCurrencyAndLastRate'. */
    public List<Map<String, Object>> lastExchangeRate(UserAccount u, String documentTypeIds, String currencyIds) {
        return db.rows("Sp_Vouchers_GetMethods", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeIds", documentTypeIds, "DMultiCurrencyIds", currencyIds, "Activity", "GetMultiCurrencyAndLastRate");
    }

    /** TaxScheduleMain.ReadTaxSchedule: @EffectedDate @TaxNameId, 'GetTaxPercentInTaxSchedule'. */
    public List<Map<String, Object>> taxSchedule(UserAccount u, Object effectedDate, int taxNameId) {
        return db.rows("Sp_TaxSchedule_GetAllMehtod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "EffectedDate", effectedDate, "TaxNameId", taxNameId, "Activity", "GetTaxPercentInTaxSchedule");
    }
}
