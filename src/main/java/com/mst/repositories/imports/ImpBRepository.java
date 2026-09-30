package com.mst.repositories.imports;

import com.mst.models.UserAccount;
import com.mst.models.hrm.DesktopModel;
import com.mst.repositories.hrm.HrmProcRepository;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.time.LocalDateTime;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.function.Supplier;

/**
 * DAL of the four Import screens ported here (782 Proforma Invoice, 783 Import Invoice, 784 Invoice
 * Packing Detail, 785 Shipment Booking) and of the two serial dialogs they open (frmProformaDocumentSerial,
 * frmMasterDocumentSerial). Every call is the desktop BLL's own procedure with the parameters the BLL adds,
 * guarded exactly as the BLL guards them (a guarded parameter is left out, never sent as NULL). Tenancy is
 * always the signed-in user's. Nothing here creates or changes a table, column or procedure.
 *
 * Procedures (all checked in procdure.utf8.sql):
 *   [ImEx].[usp_Get_InvoiceMasterNoForShipmentBooking]  ShipmentBooking.GetInvoiceNoForBookingInfo
 *   [ImEx].[usp_Get_ShipmentBooking] 'ID' / 'SEARCH'      ShipmentBooking.GetByID / History
 *   [ImEx].[usp_Set_ShipmentBooking] INSERT / UPDATE      ShipmentBooking.Save
 *   [ImEx].[USP_GetDataForDropDownFrom_ShipmentBooking]    ShipmentBooking.GetDataForDropDownFromShipmentBookingInfo
 *   [ImEx].[USP_GetDataForDropDownFrom_ImportRelated]      invoiceMaster.GetDataForDropDownFrom_ImportRelated
 *   [ImEx].[USP_GetDataForDropDownFrom_Invoice]            invoiceMaster.GetDataForDropDownFrom_Invoice
 *   [ImEx].[usp_Get_InvoiceMasterNoForPackingList]         invoicePackingList.GetInvoiceNo
 *   [ImEx].[usp_Get_invoicePackingList] 'ByInvoiceMasterID' invoicePackingList.GetById
 *   [ImEx].[usp_Set_invoicePackingList] INSERT / UPDATE    invoicePackingListProvider.Set
 *   [ImEx].[usp_Get_invoiceMaster] 'SEARCH' / 'ID'         invoiceMaster.SEARCHHistory / ReadById
 *   [ImEx].[usp_Get_invoiceDetail|InvoicePaymentTerm|invoiceOtherItem] 'SEARCH'   invoiceMasterProvider.GetAll
 *   [ImEx].[usp_Set_invoiceMaster|invoiceDetail|InvoicePaymentTerm|invoiceOtherItem] invoiceMasterProvider.Set
 *   [ImEx].[usp_Get_invoiceMasterGenerateDocNo]            invoiceMaster.GenerateCode
 *   [ImEx].[usp_Get_ProformaMasterGenerateDocNo]           ProformaMaster.GenerateCode
 *   [ImEx].[usp_Get_ProformaMaster] 'ID' / 'SEARCH'        ProformaMaster.GetByID / SearchHistory
 *   [ImEx].[usp_Get_ProformaDetail] 'SEARCH'               DAL ProformaMaster.GetDate
 *   [ImEx].[usp_Set_ProformaMaster] / [usp_Set_ProformaDetail]   DAL ProformaMaster.SetDate
 *   [ImEx].[USP_GetDataForDropDownFrom_ProformaMaster]     ProformaMaster.GetDataForDropDownFrom_ProformaMaster
 *   [ImEx].[usp_Get_ProformaMasterNoFromMasterDocumentSerial]  ProformaMaster.GetProformaNoFromMasterDocumentSerial
 *   [ImEx].[usp_Get_masterDocumentSerial] 'SEARCH' / 'ComboForInvoice'   masterDocumentSerial.SEARCHHistory / ComboForInvoice
 *   [ImEx].[usp_Set_masterDocumentSerial] Insert / UPDATE  masterDocumentSerialProvider.Set
 *   Sp_SupplierCustomer_GetAllMethod 'ReadByOrganizationCompanyIdForExport' / 'GetSupplierustomerByCustomerGroupId'
 *   SP_Country_ReadMethod 'GetAll', Sp_Bank_GetAllMethod 'ReadAll', Sp_Item_GetAllMethod 'ReadAllForComboTwoColumns' /
 *   'GetItemByItemTypeId', Sp_UOMSchedule_GetAllMethod 'ReadByItemID', [dbo].[USP_GetJobLotsAllocatedToBranch],
 *   usp_GetFINoForInvoice, Sp_ExImEFormRegistration_GetAllMethod 'GetFinancialInstrumentsBalance',
 *   usp_getExportCompaniesByCompanyId, Sp_DMSAttachments_GetAllMethod 'ReadById' / 'DeleteById' / 'RemoveByIdAndName',
 *   Proc_DMSAttachments_Insert.
 */
@Repository
public class ImpBRepository {

    public static final String IMEX = "[ImEx].";

    private final HrmProcRepository proc;

    public ImpBRepository(HrmProcRepository proc) { this.proc = proc; }

    public <T> T tx(Supplier<T> work) { return proc.tx(work); }

    public List<Map<String, Object>> rows(String p, Map<String, Object> params) { return proc.rows(p, params); }

    public int set(String p, DesktopModel m, String activity) { return proc.set(p, m, activity); }

    public int set(String p, DesktopModel m) { return proc.set(p, m); }

    public int set(String p, Map<String, Object> params) { return proc.set(p, params); }

    private static Map<String, Object> tenancy(UserAccount u) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("OrganizationId", u.getOrganizationId());
        m.put("CompanyId", u.getCompanyId());
        return m;
    }

    private static Map<String, Object> kv(Map<String, Object> m, Object... kv) {
        for (int i = 0; i + 1 < kv.length; i += 2) m.put(String.valueOf(kv[i]), kv[i + 1]);
        return m;
    }

    private static Timestamp ts(LocalDateTime d) { return d == null ? null : Timestamp.valueOf(d); }

    // ================================================================== shared lookups

    /** invoiceMaster.GetDataForDropDownFrom_ImportRelated (Activity not set by any of these forms). */
    public List<Map<String, Object>> importRelated(UserAccount u) {
        return rows(IMEX + "[USP_GetDataForDropDownFrom_ImportRelated]", tenancy(u));
    }

    /** invoiceMaster.GetDataForDropDownFrom_Invoice (no Activity). */
    public List<Map<String, Object>> invoiceDropDown(UserAccount u) {
        return rows(IMEX + "[USP_GetDataForDropDownFrom_Invoice]", tenancy(u));
    }

    /** SupplierCustomer.ReadByOrganizationCompanyIdForExport. */
    public List<Map<String, Object>> suppliersForExport(UserAccount u) {
        return rows("Sp_SupplierCustomer_GetAllMethod", kv(tenancy(u), "Activity", "ReadByOrganizationCompanyIdForExport"));
    }

    /**
     * SupplierCustomer.GetSupplierustomerByCustomerGroupId(ReportsParameters): Org, Company, @ParentId when != 0,
     * @CustomerGroupIds when not blank, @Activity. (CommonServices.GetSupplierustomerByCustomerGroupId(ids, parentId).)
     */
    public List<Map<String, Object>> suppliersByGroup(UserAccount u, String groupIds, int parentId) {
        Map<String, Object> p = tenancy(u);
        if (parentId != 0) p.put("ParentId", parentId);
        if (groupIds != null && !groupIds.isEmpty()) p.put("CustomerGroupIds", groupIds);
        p.put("Activity", "GetSupplierustomerByCustomerGroupId");
        return rows("Sp_SupplierCustomer_GetAllMethod", p);
    }

    /** country.GetAll(new Country{Org, Company}) - SP_Country_ReadMethod @MethodType GetAll. */
    public List<Map<String, Object>> countries(UserAccount u) {
        return rows("SP_Country_ReadMethod", kv(tenancy(u), "MethodType", "GetAll"));
    }

    /** Bank.GetAll - Sp_Bank_GetAllMethod @Activity ReadAll. */
    public List<Map<String, Object>> banks(UserAccount u) {
        return rows("Sp_Bank_GetAllMethod", kv(tenancy(u), "Activity", "ReadAll"));
    }

    /** CommonServices.ItemGetForComboServiceBind() -> Item.GetAllbyCombobind: ReadAllForComboTwoColumns (no category). */
    public List<Map<String, Object>> itemsForCombo(UserAccount u) {
        return rows("Sp_Item_GetAllMethod", kv(tenancy(u), "Activity", "ReadAllForComboTwoColumns"));
    }

    /** CommonServices.GetItemByItemTypeId(ids) -> Item.GetItemByItemTypeId: @LookupTypeIds always. */
    public List<Map<String, Object>> itemsByType(UserAccount u, String ids) {
        return rows("Sp_Item_GetAllMethod", kv(tenancy(u), "LookupTypeIds", ids, "Activity", "GetItemByItemTypeId"));
    }

    /** CommonServices.GetUomScheduleByItemId -> UOMSchedule.SearchByObject: 'ReadByItemID'. */
    public List<Map<String, Object>> uoms(UserAccount u, int itemId) {
        return rows("Sp_UOMSchedule_GetAllMethod", kv(tenancy(u), "ItemId", itemId, "Activity", "ReadByItemID"));
    }

    /** JobLotsAllocationToBranch.GetJobLotsAllocatedToBranchByBranchId: @BranchId when != 0. */
    public List<Map<String, Object>> jobLotsOfBranch(UserAccount u) {
        Map<String, Object> p = tenancy(u);
        int b = u.getBranchesId() == null ? 0 : u.getBranchesId();
        if (b != 0) p.put("BranchId", b);
        return rows("[dbo].[USP_GetJobLotsAllocatedToBranch]", p);
    }

    /** ExImEFormRegistration.GetFINoForInvoice: @SupplierCustomerId when != 0 (RecId never set by the form). */
    public List<Map<String, Object>> financialInstruments(UserAccount u, int supplierCustomerId) {
        Map<String, Object> p = tenancy(u);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        return rows("usp_GetFINoForInvoice", p);
    }

    /** ExImEFormRegistration.GetFinancialInstrumentsBalance - rows[0][0] or 0. */
    public List<Map<String, Object>> financialInstrumentBalance(UserAccount u, int documentTypeId, int id) {
        return rows("Sp_ExImEFormRegistration_GetAllMethod",
                kv(tenancy(u), "DocumentTypeId", documentTypeId, "Id", id, "Activity", "GetFinancialInstrumentsBalance"));
    }

    /** ExImLcOrder.GetExportCompaniesByCompany. */
    public List<Map<String, Object>> exportCompanies(UserAccount u) {
        return rows("usp_getExportCompaniesByCompanyId", tenancy(u));
    }

    // ================================================================== attachments (DMSAttachments)

    /** DMSAttachments.GetByID(id, screenName): 'ReadById' (@ScreenName, @Id). */
    public List<Map<String, Object>> attachments(String screenName, long id) {
        return rows("[dbo].[Sp_DMSAttachments_GetAllMethod]", kv(new LinkedHashMap<>(), "ScreenName", screenName, "Id", id, "Activity", "ReadById"));
    }

    /** DAL SetData: Sp_DMSAttachments_GetAllMethod @ScreenName, @Id, 'DeleteById' before the list is re-inserted. */
    public void deleteAttachments(String screenName, long id) {
        rows("[dbo].[Sp_DMSAttachments_GetAllMethod]", kv(new LinkedHashMap<>(), "ScreenName", screenName, "Id", id, "Activity", "DeleteById"));
    }

    /** DAL SetData: GenericProvider.SetProc(sqlTrn, DMSAttachments, "Proc_DMSAttachments_Insert") - all 18 properties. */
    public void insertAttachment(UserAccount u, String screenName, long refDocNo, int refAccountId, int refDocTypeId,
                                 String attachment, String customName, double sizeMb, long entryUser, long modifyUser) {
        LocalDateTime now = LocalDateTime.now();
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("RefDocumentNo", (int) refDocNo);
        p.put("CompanyId", u.getCompanyId());
        p.put("ModifyUser", (int) modifyUser);
        p.put("Attachment", attachment);
        p.put("BranchId", 0);
        p.put("Id", 0);
        p.put("EntryDate", ts(now));
        p.put("RefDocumentTypeId", refDocTypeId);
        p.put("RefAccountId", refAccountId);
        p.put("DMSFoldersLabelsId", 0);
        p.put("ModifyDate", ts(now));
        p.put("EntryUser", (int) entryUser);
        p.put("ScreenName", screenName);
        p.put("UploadedFileCustomName", customName);
        p.put("UploadedFileSizeMb", java.math.BigDecimal.valueOf(sizeMb));
        p.put("DetailWiseAttachment", false);
        p.put("LineId", 0);
        proc.scalar("[dbo].[Proc_DMSAttachments_Insert]", p);
    }

    /** DMSAttachments.RemoveByIdAndNames(Id, ScreenName, RefDocumentTypeId, 0): 'RemoveByIdAndName'. */
    public void removeAttachmentsByIdAndName(long id, String screenName, int refDocumentTypeId) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("Id", (int) id);
        p.put("ScreenName", screenName);
        if (refDocumentTypeId != 0) p.put("RefDocumentTypeId", refDocumentTypeId);
        p.put("Activity", "RemoveByIdAndName");
        proc.scalar("[dbo].[Sp_DMSAttachments_GetAllMethod]", p);
    }

    // ================================================================== 785 Shipment Booking

    /** ShipmentBooking.GetInvoiceNoForBookingInfo(Org, Company, 902, ActiveYr, RecId): @RecId only when != 0. */
    public List<Map<String, Object>> bookingInvoices(UserAccount u, int financialYearId, long recId) {
        Map<String, Object> p = kv(tenancy(u), "DocumentTypeId", 902, "FinancialYearId", financialYearId);
        if (recId != 0) p.put("RecId", recId);
        return rows(IMEX + "[usp_Get_InvoiceMasterNoForShipmentBooking]", p);
    }

    /** ShipmentBooking.GetByID: @ShipmentBookingId, @Activity 'ID'. */
    public List<Map<String, Object>> booking(long id) {
        return rows(IMEX + "[usp_Get_ShipmentBooking]", kv(new LinkedHashMap<>(), "ShipmentBookingId", id, "Activity", "ID"));
    }

    /** ShipmentBooking.GetDataForDropDownFromShipmentBookingInfo (Activity not set by the form). */
    public List<Map<String, Object>> bookingHistoryCombos(UserAccount u) {
        return rows(IMEX + "[USP_GetDataForDropDownFrom_ShipmentBooking]", tenancy(u));
    }

    /** ShipmentBooking.History - parameters exactly as the BLL adds them (see ImpBShipmentBookingService.history). */
    public List<Map<String, Object>> bookingHistory(Map<String, Object> p) {
        return rows(IMEX + "[usp_Get_ShipmentBooking]", p);
    }

    // ================================================================== 784 Invoice Packing Detail

    /** invoicePackingList.GetInvoiceNo(Org, Company, 902, ActiveYr, RecId): @RecId only when != 0. */
    public List<Map<String, Object>> packingInvoices(UserAccount u, int financialYearId, long recId) {
        Map<String, Object> p = kv(tenancy(u), "DocumentTypeId", 902, "FinancialYearId", financialYearId);
        if (recId != 0) p.put("RecId", recId);
        return rows(IMEX + "[usp_Get_InvoiceMasterNoForPackingList]", p);
    }

    /** invoicePackingList.GetById: GenericProvider.GetAll 'ByInvoiceMasterID' @InvoiceMasterId. */
    public List<Map<String, Object>> packingByInvoice(long invoiceMasterId) {
        return rows(IMEX + "[usp_Get_invoicePackingList]", kv(new LinkedHashMap<>(), "Activity", "ByInvoiceMasterID", "InvoiceMasterId", invoiceMasterId));
    }

    // ================================================================== 783 Import Invoice / invoice history

    /** invoiceMaster.GenerateCode: Org, Company, FinancialYearId, DocumentTypeId, @BranchesId when != 0. */
    public int invoiceDocNo(UserAccount u, int financialYearId, int documentTypeId) {
        Map<String, Object> p = kv(tenancy(u), "FinancialYearId", financialYearId, "DocumentTypeId", documentTypeId);
        int b = u.getBranchesId() == null ? 0 : u.getBranchesId();
        if (b != 0) p.put("BranchesId", b);
        List<Map<String, Object>> r = rows(IMEX + "[usp_Get_invoiceMasterGenerateDocNo]", p);
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    /** invoiceMaster.SEARCHHistory - usp_Get_invoiceMaster 'SEARCH' with the BLL's guarded parameters. */
    public List<Map<String, Object>> invoiceSearch(Map<String, Object> p) {
        p.put("Activity", "SEARCH");
        return rows(IMEX + "[usp_Get_invoiceMaster]", p);
    }

    /** invoiceMaster.ReadById -> invoiceMasterProvider.GetAll("ID", @invoiceMasterID). */
    public List<Map<String, Object>> invoice(long id) {
        return rows(IMEX + "[usp_Get_invoiceMaster]", kv(new LinkedHashMap<>(), "Activity", "ID", "invoiceMasterID", id));
    }

    /** invoiceMasterProvider.GetAll: each child list is GetAll<child>("SEARCH", same @invoiceMasterID). */
    public List<Map<String, Object>> invoiceChildren(String childProc, long id) {
        return rows(IMEX + "[" + childProc + "]", kv(new LinkedHashMap<>(), "Activity", "SEARCH", "invoiceMasterID", id));
    }

    // ================================================================== 782 Proforma Invoice

    /** ProformaMaster.GenerateCode: Org, Company, documentTypeId, FinancialYearId, BranchesId (always). */
    public int proformaDocNo(UserAccount u, int financialYearId) {
        Map<String, Object> p = kv(tenancy(u), "documentTypeId", 900, "FinancialYearId", financialYearId,
                "BranchesId", u.getBranchesId() == null ? 0 : u.getBranchesId());
        List<Map<String, Object>> r = rows(IMEX + "[usp_Get_ProformaMasterGenerateDocNo]", p);
        if (r.isEmpty()) throw new IllegalStateException("There is no row at position 0.");
        return toInt(r.get(0).get("DocNo"));
    }

    /** ProformaMaster.GetProformaNoFromMasterDocumentSerial: @ProformaMasterId when RecId != 0, @IsForInvoice when SkipZero != 0. */
    public List<Map<String, Object>> proformaNos(UserAccount u, long recId, int isForInvoice) {
        Map<String, Object> p = tenancy(u);
        if (recId != 0) p.put("ProformaMasterId", recId);
        if (isForInvoice != 0) p.put("IsForInvoice", isForInvoice);
        return rows(IMEX + "[usp_Get_ProformaMasterNoFromMasterDocumentSerial]", p);
    }

    /** ProformaMaster.GetByID: @proformaMasterId, 'ID'. */
    public List<Map<String, Object>> proforma(long id) {
        return rows(IMEX + "[usp_Get_ProformaMaster]", kv(new LinkedHashMap<>(), "proformaMasterId", id, "Activity", "ID"));
    }

    /** DAL ProformaMaster.GetDate: usp_Get_ProformaDetail @proformaMasterId, 'SEARCH'. */
    public List<Map<String, Object>> proformaDetails(long id) {
        return rows(IMEX + "[usp_Get_ProformaDetail]", kv(new LinkedHashMap<>(), "proformaMasterId", id, "Activity", "SEARCH"));
    }

    /** ProformaMaster.SearchHistory - parameters as the BLL adds them. */
    public List<Map<String, Object>> proformaSearch(Map<String, Object> p) {
        p.put("Activity", "SEARCH");
        return rows(IMEX + "[usp_Get_ProformaMaster]", p);
    }

    /** ProformaMaster.GetDataForDropDownFrom_ProformaMaster (no Activity). */
    public List<Map<String, Object>> proformaDropDown(UserAccount u) {
        return rows(IMEX + "[USP_GetDataForDropDownFrom_ProformaMaster]", tenancy(u));
    }

    // ================================================================== masterDocumentSerial (the two Define dialogs)

    /** masterDocumentSerial.SEARCHHistory: GetAllDataTableCore 'SEARCH', @IsPerforma when ActionId != 0. */
    public List<Map<String, Object>> serialSearch(UserAccount u, int isPerforma) {
        Map<String, Object> p = kv(new LinkedHashMap<>(), "Activity", "SEARCH");
        p.putAll(tenancy(u));
        if (isPerforma != 0) p.put("IsPerforma", isPerforma);
        return rows(IMEX + "[usp_Get_masterDocumentSerial]", p);
    }

    /** masterDocumentSerial.ComboForInvoice: 'ComboForInvoice', @invoiceMasterId when RecId != 0 (ActionId never set). */
    public List<Map<String, Object>> serialComboForInvoice(UserAccount u, long recId) {
        Map<String, Object> p = kv(new LinkedHashMap<>(), "Activity", "ComboForInvoice");
        p.putAll(tenancy(u));
        if (recId != 0) p.put("invoiceMasterId", recId);
        return rows(IMEX + "[usp_Get_masterDocumentSerial]", p);
    }

    public static int toInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        try { return Integer.parseInt(String.valueOf(v).trim()); } catch (NumberFormatException e) { return 0; }
    }
}
