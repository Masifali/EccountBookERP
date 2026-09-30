package com.mst.repositories.logistics;

import com.mst.models.UserAccount;
import com.mst.models.hrm.DesktopModel;
import com.mst.repositories.hrm.HrmProcRepository;
import org.springframework.stereotype.Repository;

import java.time.LocalDateTime;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.function.Supplier;

/**
 * DAL of the four Logistics transaction screens ported by this group (LgsB):
 *
 *   949 Logistic Purchase Order         frmLogisticPurchaseOrder            BLL/DAL lgstcm.PurchaseOrderHeader
 *   955 Logistic Purchase Services Bill frmLogisticPurchaseServicesBill     BLL/DAL lgstcm.ServicesBillHeader
 *   965 Freight Voucher (Export)        frmFreightVoucherExport             BLL/DAL lgstcm.FreightVoucherOutward
 *   967 Freight Voucher Export (Multi)  frmFreightVoucherExportMultiVehicles BLL/DAL lgstcm.FreightVoucherOutward
 *
 * Every method is one desktop BLL / DAL call: same procedure, same parameters, and a parameter the BLL
 * guards ("if (x != 0)") is omitted when unset (a null value is never sent). Only procedures that exist
 * in the database are called; nothing is created or changed in the schema.
 */
@Repository
public class LgsBRepository {

    private final HrmProcRepository db;

    public LgsBRepository(HrmProcRepository db) { this.db = db; }

    public <T> T tx(Supplier<T> work) { return db.tx(work); }

    // ================================================================== plumbing

    /** Ordered parameter map; a null value is dropped by DesktopProc (never sent). */
    public static Map<String, Object> p(Object... kv) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (int i = 0; i + 1 < kv.length; i += 2) m.put(String.valueOf(kv[i]), kv[i + 1]);
        return m;
    }

    /** "if (x != 0) list.Add(...)" - the value, or null so the parameter is not sent. */
    private static Integer nz(int v) { return v != 0 ? v : null; }

    private static String nzs(String s) { return s == null || s.isEmpty() ? null : s; }

    public List<Map<String, Object>> rows(String proc, Map<String, Object> params) { return db.rows(proc, params); }

    /** GenericProvider.SetProc read with Convert.ToInt32 (strict). */
    public int set(String proc, DesktopModel m) { return db.set(proc, m); }

    /** GenericProvider.SetProc whose result the DAL ignores / ExecuteNonQuery (lenient). */
    public void exec(String proc, Map<String, Object> params) { db.scalar(proc, params); }

    public void exec(String proc, DesktopModel m) { db.scalar(proc, m.toParams()); }

    // ================================================================== globals (clsGlobalVariables caches)

    /** GlobalServicesMethods.getGlobalSupplierCustomer: USP_GetVendorsAndCustomersWithCityName @OrganizationId, @CompanyId. */
    public List<Map<String, Object>> suppliers(UserAccount u) {
        return db.rows("USP_GetVendorsAndCustomersWithCityName", p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** GlobalServicesMethods.Item_AllServiesItems: [lgstcm].[USP_Item_AllServiesItems]. */
    public List<Map<String, Object>> serviceItems(UserAccount u) {
        return db.rows("[lgstcm].[USP_Item_AllServiesItems]", p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** GlobalServicesMethods.GetAllMultiCurrency: Sp_MultiCurrency_GetAllMethod 'ReadAll'. */
    public List<Map<String, Object>> currencies(UserAccount u) {
        return db.rows("Sp_MultiCurrency_GetAllMethod", p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    /** GlobalServicesMethods.getGlobalAllCity: [dbo].[USP_City_GetAllWithCountryAndTehsil]. */
    public List<Map<String, Object>> cities(UserAccount u) {
        return db.rows("[dbo].[USP_City_GetAllWithCountryAndTehsil]", p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** GlobalServicesMethods.GetGlobalAllAccountsWithCustomGroup: [dbo].[USP_GETAllAccountsFromCustomGroups]. */
    public List<Map<String, Object>> accounts(UserAccount u) {
        return db.rows("[dbo].[USP_GETAllAccountsFromCustomGroups]", p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** GlobalVariables_Helper.GetConfigValueFromGlobal(description): the ConfigKey text, "" when absent. */
    public String config(UserAccount u, String description) {
        List<Map<String, Object>> r = db.rows("Sp_ConfigrationsAllocation_GetAllMethod", p("OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        if (r.isEmpty() || r.get(0).get("ConfigKey") == null) return "";
        return String.valueOf(r.get(0).get("ConfigKey")).trim();
    }

    /** CommonServices.GetERPFeatureById: the id is in USP_GetERPFeaturesByCompanyId. */
    public boolean feature(UserAccount u, int featureId) {
        for (Map<String, Object> r : db.rows("USP_GetERPFeaturesByCompanyId", p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            Object id = r.get("Id");
            if (id instanceof Number && ((Number) id).intValue() == featureId) return true;
        }
        return false;
    }

    /** CommonServices.SetRightsValueInRightsObject: Sp_tblUserRights_GetAllMethod 'GetByUserId' rows (RightName, Value). */
    public List<Map<String, Object>> userRights(UserAccount u, String screenName, String roleName) {
        return db.rows("Sp_tblUserRights_GetAllMethod", p("UserId", u.getId(), "ScreenName", screenName, "RightName", roleName == null ? "" : roleName,
                "CompanyId", u.getCompanyId(), "Activity", "GetByUserId"));
    }

    /** clsGlobalVariables.ActiveYr (Start_Period): Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId. */
    public List<Map<String, Object>> financialYears(UserAccount u) {
        return db.rows("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** LookUps.LookUpAllServices(org, comp, null): [lgstcm].[USP_LookAllServices] (no @Activity). */
    public List<Map<String, Object>> lookUpAllServices(UserAccount u) {
        return db.rows("[lgstcm].[USP_LookAllServices]", p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** SeaPorts.Getall: Sp_SeaPorts_GetAllMethod 'ReadByCompanyNOrganizationId'. */
    public List<Map<String, Object>> seaPorts(UserAccount u) {
        return db.rows("Sp_SeaPorts_GetAllMethod", p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadByCompanyNOrganizationId"));
    }

    /** CommonServices.GetTaxTypeIdAndPercentByItemId -> ItemTaxSchedule 'GetItemTaxScheduleForItemId'. */
    public List<Map<String, Object>> taxForItem(UserAccount u, int itemId, LocalDateTime date) {
        return db.rows("Sp_ItemTaxSchedule_GetAllMethod", p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ItemId", itemId, "EffectedDate", date, "Activity", "GetItemTaxScheduleForItemId"));
    }

    /** CommonServices.GetTaxScheduleDetailbyItemIds -> ItemTaxSchedule 'GetTaxScheduleDetailbyItemIds'. */
    public List<Map<String, Object>> taxForItems(UserAccount u, String itemIds, LocalDateTime date) {
        return db.rows("Sp_ItemTaxSchedule_GetAllMethod", p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ItemIds", itemIds, "EffectedDate", date, "Activity", "GetTaxScheduleDetailbyItemIds"));
    }

    /** ExImInvoice.getExportInvoicePendingAndAll: usp_getExportInvoicePendingAndAll, @ActionId / @RecId when set. */
    public List<Map<String, Object>> exportInvoices(UserAccount u, int actionId, int recId) {
        return db.rows("usp_getExportInvoicePendingAndAll", p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ActionId", nz(actionId), "RecId", recId > 0 ? recId : null));
    }

    /** ExImInvoice.GetInvoiceInformationByInvoiceIdForServices: Sp_ExImInvoice_GetAllMethod. */
    public List<Map<String, Object>> invoiceInfo(UserAccount u, int invoiceId) {
        return db.rows("Sp_ExImInvoice_GetAllMethod", p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Id", invoiceId, "Activity", "GetInvoiceInformationByInvoiceIdForServices"));
    }

    /** CommonServices.VoucherHeadIdGet(id, documentTypeId): Sp_Vouchers_GetMethods 'GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId'. */
    public int voucherHeadId(UserAccount u, int documentTypeId, int id) {
        List<Map<String, Object>> r = db.rows("Sp_Vouchers_GetMethods", p("Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId",
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeId", documentTypeId, "DocumentTypeSrNo", id));
        if (r.isEmpty()) return 0;
        Object v = r.get(0).get("Id");
        if (v == null && !r.get(0).isEmpty()) v = r.get(0).values().iterator().next();
        return v instanceof Number ? ((Number) v).intValue() : 0;
    }

    // ================================================================== 949 Purchase Order (BLL 0389 / DAL 0346)

    /** PurchaseOrderHeader.GenerateCode. */
    public int poGenerateCode(UserAccount u, int fy, int documentTypeId) {
        List<Map<String, Object>> r = db.rows("[lgstcm].[USP_PurchaseOrderHeader_GetAllMethod]", p("OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "BranchesId", u.getBranchesId(), "FinancialYearId", fy, "DocumentTypeId", documentTypeId, "Activity", "GenerateCode"));
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocumentNo"));
    }

    /** PurchaseOrderHeader.ReadById -> DAL GetData: header 'ReadById'. */
    public List<Map<String, Object>> poHeader(int id) {
        return db.rows("[lgstcm].[USP_PurchaseOrderHeader_GetAllMethod]", p("Id", id, "Activity", "ReadById"));
    }

    /** DAL GetData: 'ReadByHeaderId_PurchaseOrderDetail'. */
    public List<Map<String, Object>> poDetail(int id) {
        return db.rows("[lgstcm].[USP_PurchaseOrderHeader_GetAllMethod]", p("Id", id, "Activity", "ReadByHeaderId_PurchaseOrderDetail"));
    }

    /** PurchaseOrderHeader.DeleteByID (ExecuteNonQuery). */
    public void poDelete(int entryUserId, int id) {
        exec("[lgstcm].[USP_PurchaseOrderHeader_GetAllMethod]", p("EntryUserId", entryUserId, "Id", id, "Activity", "DeleteById"));
    }

    /** PurchaseOrderHeader.GetDataForDropDownFromPurchaseOrderHeader (no @Activity from these callers). */
    public List<Map<String, Object>> poDropDown(UserAccount u) {
        return db.rows("[lgstcm].[USP_GetDataForDropDownFromPurchaseOrderHeader]", p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** PurchaseOrderHeader.LoadPendingSaleOrderOnInvoice: [lgstcm].[USP_PurchaseOrder_PendingDataForServicesBill]. */
    public List<Map<String, Object>> poPendingForServicesBill(UserAccount u, int fy, int documentTypeId, LocalDateTime from, LocalDateTime to,
                                                              int fromDocNo, int toDocNo, int serviceTypeId, int billToPartyId, int serviceProviderId, int itemId) {
        return db.rows("[lgstcm].[USP_PurchaseOrder_PendingDataForServicesBill]", p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchesId", u.getBranchesId(), "DocumentTypeId", documentTypeId, "FinancialYearId", fy, "FromDate", from, "ToDate", to,
                "FromDocNo", nz(fromDocNo), "ToDocNo", nz(toDocNo), "ServiceTypeId", nz(serviceTypeId), "BillToPartyId", nz(billToPartyId),
                "ServiceProviderId", nz(serviceProviderId), "ItemId", nz(itemId)));
    }

    // ================================================================== Agreement loader (frmLoadLogisticAgreementForPO, BLL 0390)

    /** AgreementHeader.GetDataForDropDownFromAgreementHeader (no @Activity). */
    public List<Map<String, Object>> agreementDropDown(UserAccount u) {
        return db.rows("[lgstcm].[USP_GetDataForDropDownFromAgreementHeader]", p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** AgreementHeader.Agreement_DataForPurchaseOrder: [lgstcm].[USP_Agreement_DataForPurchaseOrder] (DataSet Tables[0]). */
    public List<Map<String, Object>> agreementForPo(UserAccount u, int fy, LocalDateTime from, LocalDateTime to, int fromDocNo, int toDocNo,
                                                    int serviceTypeId, int serviceProviderId, int brokerAgentId, int itemId) {
        return db.rows("[lgstcm].[USP_Agreement_DataForPurchaseOrder]", p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchesId", u.getBranchesId(), "FinancialYearId", fy, "DocumentTypeId", 1301, "FromDate", from, "ToDate", to,
                "FromDocNo", nz(fromDocNo), "ToDocNo", nz(toDocNo), "ServiceTypeId", nz(serviceTypeId), "ServiceProviderId", nz(serviceProviderId),
                "BrokerAgentId", nz(brokerAgentId), "ItemId", nz(itemId)));
    }

    // ================================================================== 955 Services Bill (BLL 0388 / DAL 0345)

    public int sbGenerateCode(UserAccount u, int fy, int documentTypeId) {
        List<Map<String, Object>> r = db.rows("[lgstcm].[USP_ServicesBillHeader_GetAllMethod]", p("OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "BranchesId", u.getBranchesId(), "FinancialYearId", fy, "DocumentTypeId", documentTypeId, "Activity", "GenerateCode"));
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocumentNo"));
    }

    public List<Map<String, Object>> sbHeader(int id) {
        return db.rows("[lgstcm].[USP_ServicesBillHeader_GetAllMethod]", p("Id", id, "Activity", "ReadById"));
    }

    public List<Map<String, Object>> sbDetail(int id) {
        return db.rows("[lgstcm].[USP_ServicesBillHeader_GetAllMethod]", p("Id", id, "Activity", "ReadByHeaderId_ServicesBillDetail"));
    }

    public List<Map<String, Object>> sbFreightDetail(int id) {
        return db.rows("[lgstcm].[USP_ServicesBillHeader_GetAllMethod]", p("Id", id, "Activity", "ReadByHeaderId_ServicesBillFreightVoucherDetail"));
    }

    public void sbDelete(int entryUserId, int id) {
        exec("[lgstcm].[USP_ServicesBillHeader_GetAllMethod]", p("EntryUserId", entryUserId, "Id", id, "Activity", "DeleteById"));
    }

    /** ServicesBillHeader.GetDataForDropDownFromServicesBillHeader (@DocumentTypeIds, no @Activity). */
    public List<Map<String, Object>> sbDropDown(UserAccount u, String documentTypeIds) {
        return db.rows("[lgstcm].[USP_GetDataForDropDownFromServicesBillHeader]", p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeIds", nzs(documentTypeIds)));
    }

    /** CommonServies.GetSupplierCustomerListForFinancialEffects: Sp_SupplierCustomer_GetAllMethod. */
    public List<Map<String, Object>> supplierGlAccounts(UserAccount u) {
        return db.rows("Sp_SupplierCustomer_GetAllMethod", p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Activity", "GetGlAccountIdandCompanyNameBySupplierCustomerId"));
    }

    /** CommonServies.GetItemListForFinancialEffects: Sp_Item_GetAllMethod 'GetItemGlIdsandItemName'. */
    public List<Map<String, Object>> itemGlAccounts(UserAccount u) {
        return db.rows("Sp_Item_GetAllMethod", p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetItemGlIdsandItemName"));
    }

    // ================================================================== 965 / 967 Freight Voucher Outward (BLL 0387 / DAL 0344)

    public int fvGenerateCode(UserAccount u, int fy, int documentTypeId) {
        List<Map<String, Object>> r = db.rows("[lgstcm].[USP_FreightVoucherOutward_GetAllMethod]", p("OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "BranchesId", u.getBranchesId(), "FinancialYearId", fy, "DocumentTypeId", documentTypeId, "Activity", "GenerateCode"));
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    public int fvGenerateMasterDocNo(UserAccount u, int fy, int documentTypeId) {
        List<Map<String, Object>> r = db.rows("[lgstcm].[USP_FreightVoucherOutward_GetAllMethod]", p("OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "BranchesId", u.getBranchesId(), "FinancialYearId", fy, "DocumentTypeId", documentTypeId, "Activity", "GenerateMasterDocNo"));
        return r.isEmpty() ? 0 : toInt(r.get(0).get("MasterDocNo"));
    }

    /** FreightVoucherOutward.ReadById: @Id, 'ReadById'. */
    public List<Map<String, Object>> fvById(int id) {
        return db.rows("[lgstcm].[USP_FreightVoucherOutward_GetAllMethod]", p("Id", id, "Activity", "ReadById"));
    }

    /** FreightVoucherOutward.ReadByMasterId: @MasterId, 'ReadById'. */
    public List<Map<String, Object>> fvByMaster(int masterId) {
        return db.rows("[lgstcm].[USP_FreightVoucherOutward_GetAllMethod]", p("MasterId", masterId, "Activity", "ReadById"));
    }

    public List<Map<String, Object>> fvPaymentDetail(int id) {
        return db.rows("[lgstcm].[USP_FreightVoucherOutward_GetAllMethod]", p("Id", id, "Activity", "ReadByHeaderId_FreightVoucherOutwardPaymentDetail"));
    }

    public List<Map<String, Object>> fvExpenseDetail(int id) {
        return db.rows("[lgstcm].[USP_FreightVoucherOutward_GetAllMethod]", p("Id", id, "Activity", "ReadByHeaderId_FreightVoucherOutwardExpenseDetail"));
    }

    public void fvDelete(int entryUserId, int id) {
        exec("[lgstcm].[USP_FreightVoucherOutward_GetAllMethod]", p("EntryUserId", entryUserId, "Id", id, "Activity", "DeleteById"));
    }

    public void fvDeleteByMaster(int entryUserId, int masterDocId) {
        exec("[lgstcm].[USP_FreightVoucherOutward_DeleteByMasterDocID]", p("EntryUserId", entryUserId, "MasterDocId", masterDocId));
    }

    /** FreightVoucherOutward.FreightVoucherOutward_GetVoucherHeadIds. */
    public List<Map<String, Object>> fvVoucherHeadIds(UserAccount u, int fy, int documentTypeId, int id, int masterId) {
        return db.rows("[lgstcm].[USP_FreightVoucherOutward_GetVoucherHeadIds]", p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchesId", u.getBranchesId(), "FinancialYearId", fy, "DocumentTypeId", documentTypeId,
                "Id", id > 0 ? id : null, "MasterId", masterId > 0 ? masterId : null));
    }

    /** FreightVoucherOutward.GetDataForDropDownFromFreightVoucherOutward (@DocumentTypeIds / @Activity when set). */
    public List<Map<String, Object>> fvDropDown(UserAccount u, String documentTypeIds, String activity) {
        return db.rows("[lgstcm].[USP_GetDataForDropDownFromFreightVoucherOutward]", p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeIds", nzs(documentTypeIds), "Activity", nzs(activity)));
    }

    /** FreightVoucherOutward.GatePassOutward_PendingForFreightVoucherOutward: [dbo].[USP_GatePassOutward_PendingForFreightVoucherOutward]. */
    public List<Map<String, Object>> gpPending(UserAccount u, int fy, int recId, LocalDateTime from, LocalDateTime to, int fromDocNo, int toDocNo,
                                               int supplierCustomerId, int transporterId, int actionId, boolean masterDoc) {
        return db.rows("[dbo].[USP_GatePassOutward_PendingForFreightVoucherOutward]", p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FinancialYearId", fy, "BranchesId", u.getBranchesId(), "RecId", nz(recId), "FromDate", from, "ToDate", to,
                "FromDocNo", nz(fromDocNo), "ToDocNo", nz(toDocNo), "SupplierCustomerId", nz(supplierCustomerId), "TransporterId", nz(transporterId),
                "ActionId", nz(actionId), "IsMasterDoc", masterDoc ? 1 : null));
    }

    /** FreightVoucherOutward.PurchaseOrderHeader_PendingForFreightVoucherOutward. */
    public List<Map<String, Object>> poPendingForFreight(UserAccount u, int fy, int billToPartyId, int recId, int headerId) {
        return db.rows("[lgstcm].[USP_PurchaseOrderHeader_PendingForFreightVoucherOutward]", p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FinancialYearId", fy, "BranchesId", u.getBranchesId(), "BillToPartyId", billToPartyId, "RecId", nz(recId), "IsMasterDoc", nz(headerId)));
    }

    /** FreightVoucherOutward.FreightVoucherOutward_PendingForLogisticServicesBill. */
    public List<Map<String, Object>> fvPendingForServicesBill(UserAccount u, int fy, int exImInvoiceId, int supplierCustomerId, int transporterId,
                                                              int actionId, String ids) {
        return db.rows("[lgstcm].[USP_FreightVoucherOutward_PendingForLogisticServicesBill]", p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FinancialYearId", fy, "BranchesId", u.getBranchesId(), "ExImInvoiceId", exImInvoiceId, "SupplierCustomerId", nz(supplierCustomerId),
                "TransporterId", nz(transporterId), "ActionId", nz(actionId), "FreightVoucherIds", nzs(ids)));
    }

    /** LogiticOtherChargesItems.FormHistory: [lgstcm].[USP_LogiticOtherChargesItems_GetAllMethod] 'FormHistory'. */
    public List<Map<String, Object>> otherChargesItems(UserAccount u) {
        return db.rows("[lgstcm].[USP_LogiticOtherChargesItems_GetAllMethod]", p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "FormHistory"));
    }

    /** VoucherHead.GetInstrumentTypes: usp_getInstrumentType. */
    public List<Map<String, Object>> instrumentTypes() { return db.rows("usp_getInstrumentType", p()); }

    /** VoucherHead.ReadByCurrentBalanceByDateAndAccountId. */
    public List<Map<String, Object>> currentBalance(UserAccount u, int fy, int accountId, LocalDateTime date) {
        return db.rows("Sp_Vouchers_GetMethods", p("Activity", "ReadByCurrentBalanceByDateAndAccountId", "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "FinancialYearId", fy, "RefAccountId", accountId, "VoucherDate", date));
    }

    /** CheqBookHeader.OutstandingCheqNo: @RecId when != 0. */
    public List<Map<String, Object>> outstandingCheques(UserAccount u, int bankId, int recId) {
        return db.rows("SP_CheqBookHeader_GetAllMethod", p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "BankId", bankId,
                "RecId", nz(recId), "MethodType", "OutstandingCheqNo"));
    }

    /**
     * GatePassOutward.GetDataForDropDownFromGPO(CompanyId, OrganizationId, "Customer"): [dbo].[USP_GetDataForDropDownFromOutWardGP].
     * The desktop calls it with the organization and company arguments swapped; the signed-in user's own
     * organization / company are sent to the matching parameters here (see the service).
     */
    public List<Map<String, Object>> gpoCustomers(UserAccount u) {
        return db.rows("[dbo].[USP_GetDataForDropDownFromOutWardGP]", p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "Customer"));
    }

    // ================================================================== history (BLL FormHistory)

    /** PurchaseOrderHeader / ServicesBillHeader / FreightVoucherOutward.FormHistory - the map is built by the service. */
    public List<Map<String, Object>> formHistory(String proc, Map<String, Object> params) { return db.rows(proc, params); }

    static int toInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        try { return new java.math.BigDecimal(String.valueOf(v).trim()).intValue(); } catch (RuntimeException e) { return 0; }
    }
}
