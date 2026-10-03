package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.saleinvoice.SaleInvoiceModels;
import com.mst.repositories.support.DesktopProc;
import com.mst.repositories.support.ProcExec;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.SqlParameterValue;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.lang.reflect.Field;
import java.lang.reflect.Modifier;
import java.math.BigDecimal;
import java.sql.Timestamp;
import java.sql.Types;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Data layer of 202 "Commercial Invoice" (Against Pre Invoice Transfer) - Architecture.WinApp.Export
 * .CommiercialInvoiceAgainstPreInvoiceTransfer (ScreenName = form Name "CommiercialInvoiceAgainstPreInvoiceTransfer",
 * table ExImInvoice with DocumentTypeId 211) and its loader popup LoadForwardingForCommercialInvoice.
 * One method per desktop BLL call, with the desktop's procedure and parameters (verified in procdure_index.csv /
 * procdure.utf8.sql, 03-Oct-2026):
 *
 *   BLL 0467 ExImInvoice
 *     GenerateCode              Sp_ExImInvoice_GetAllMethod @OrganizationId @CompanyId @DocumentTypeId=211 [@FinancialYearId] [@BranchesId] 'GenerateDocNo'
 *     GetPreInvoicenumberForCommercialInvoice  UPS_GetPreInvoicenumberForCommercialInvoice @OrganizationId @CompanyId @DocumentTypeId=209 [@SupplierCustomerId]
 *     GetPreInvoiceDataByPreInvoiceId          UPS_GetPreInvoiceHeaderDataForCommercialInvoice @ExImInvoiceId
 *     GetLastCommissionDebitAccountId          Sp_ExImInvoice_GetAllMethod @OrganizationId @CompanyId 'GetLastCommissionDebitAccountId' (Rows[0][0])
 *     FormHistory               USP_ExImInvoice_FormHistory @OrganizationId @CompanyId @FinancialYearId [dates] [@SupplierCustomerId]
 *                               @DocumentTypeIds='211' [@DocNoFrom] [@DocNoTo] (ApprovedFilter "All" -> no @IsApproved)
 *     GetByID                   Sp_ExImInvoice_GetAllMethod @Id 'ReadById' + 'ReadExImInvoicePackingDetailByHeaderId',
 *                               'ReadExImInvoicePaymentTermsDetailByHeaderId', 'ReadExImInvoiceOtherItemsByHeaderId' (DAL GetData)
 *     GetPreInvoicesAndForwardingDateForCommercialInvoices  USP_GetPreInvoicesAndForwardingDateForCommercialInvoices
 *                               @OrganizationId @CompanyId @BranchesId @FinancialYearId [@ContractId] [@SupplierCustomerId] [@FcurrencyId]
 *                               [@ItemId] [@PackingExpiryDate] [@ProductionNo] [@SkipZero]   (loader)
 *     Save -> DAL SetData       Sp_ExImInvoice_Insert | Sp_ExImInvoice_Update (every non-virtual ExImInvoice property),
 *                               Sp_ExImInvoicePackingDetail_Insert (LineId 1..n, removed rows first), Sp_ExImInvoicePaymentTermsDetail_Insert,
 *                               Sp_ExImInvoiceOtherItems_Insert, then the voucher of MakeVoucherForExImInvoice (always built for 211):
 *                               Sp_Vouchers_GetMethods 'GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId', Sp_VoucherHead_Insert |
 *                               Sp_VoucherHead_Update, Sp_VoucherDetail_Insert, USP_VoucherBalanceCheck, Sp_VoucherHead_H_Insert,
 *                               Sp_VoucherDetail_H_Insert - one transaction, rolled back on any error.
 *   BLL 0465 ExImForwarding (loader)  USP_GetDataForDropDownFromGoodsForwarding @OrganizationId @CompanyId [@BranchesId],
 *                               USP_getPackingExpiryFromForwarding / USP_getProductionNoFromForwarding @OrganizationId @CompanyId
 *   BLL 0469 ExImLcOrder       Sp_ExImLcOrder_GetAllMethod @OrganizationId @CompanyId @Id 'GetOtherItemByContractId';
 *                               [dbo].[USP_GetDataForDropDownFromExportInvoice] @OrganizationId @CompanyId (history combo)
 *   BLL 0039 GenerateExportInvoiceNos  [dbo].[USP_ExportInvoiceNos_GetAllMethod] @OrganizationId @CompanyId [@InvoiceNo]
 *                               'GetFinalInvoiceNosForCommercialInvoice';  BLL 0018 ExportPrefixType  usp_ExportInvoiceNosPrefix_History
 *   ExImEFormRegistration.GetFINoForInvoice  usp_GetFINoForInvoice @OrganizationId @CompanyId
 *   Lookups: Sp_SupplierCustomer_GetAllMethod 'ReadByOrganizationCompanyIdForExport', Sp_ExImDeliveryTerm_GetAllMethod 'ReadAll',
 *     Sp_ExImLcPaymentTerm_GetAllMethod 'ReadAll', Sp_SeaPorts_GetAllMethod 'ReadByCompanyNOrganizationId', Sp_MultiCurrency_GetAllMethod
 *     'ReadAll', Sp_Bank_GetAllMethod 'ReadAll', Sp_InvPackingType_GetAllMethod 'ReadAll', Sp_InvCropYear_GetAllMethod 'ReadAll',
 *     Sp_COAAllocation_GetAllMethod 'GetAccountTitleByAccountTypeIds' ('11,12,13,14,20,21'), Sp_UOMSchedule_GetAllMethod 'ReadByItemID',
 *     Sp_Branches_GetAllMethod 'GetAll', Sp_Projects_GetAllMethod @MethodType='GetAll', Sp_ConfigrationsAllocation_GetAllMethod.
 *   Voucher look-ups: Sp_SupplierCustomer_GetAllMethod 'GetGlAccountIdandCompanyNameBySupplierCustomerId',
 *     Sp_Item_GetAllMethod 'GetItemGlIdsandItemName'.
 * No table, column or procedure is created or changed. Tenancy is always the session user's.
 */
@Repository
public class ExportCommercialInvoiceTransferRepository {

    public static final int DOCUMENT_TYPE_ID = 211;
    private static final String INV = "Sp_ExImInvoice_GetAllMethod";

    private final JdbcTemplate jdbc;

    public ExportCommercialInvoiceTransferRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    private static Map<String, Object> oc(UserAccount u) { return params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()); }
    private List<Map<String, Object>> rows(String proc, Map<String, Object> p) { return DesktopProc.rows(jdbc, proc, p); }
    private List<Map<String, Object>> rowsA(UserAccount u, String proc, String key, String activity) { Map<String, Object> p = oc(u); p.put(key, activity); return rows(proc, p); }

    private static int num(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        try { return o == null ? 0 : (int) Math.round(Double.parseDouble(String.valueOf(o).trim())); } catch (NumberFormatException e) { return 0; }
    }

    // ================================================================== configuration

    public String config(UserAccount u, String description) {
        Map<String, Object> p = oc(u);
        p.put("ConfigDescription", description);
        p.put("Activity", "GetConfigurationByOrgCompandConfigDescription");
        List<Map<String, Object>> r = rows("Sp_ConfigrationsAllocation_GetAllMethod", p);
        if (r.isEmpty()) return "";
        Object v = r.get(0).get("ConfigKey");
        return v == null ? "" : String.valueOf(v).trim();
    }

    // ================================================================== numbering / combos

    public int generateDocNo(UserAccount u, int financialYearId) {
        Map<String, Object> p = oc(u);
        p.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        if (u.getBranchesId() != null && u.getBranchesId() != 0) p.put("BranchesId", u.getBranchesId());
        p.put("Activity", "GenerateDocNo");
        List<Map<String, Object>> r = rows(INV, p);
        return r.isEmpty() ? 0 : num(r.get(0).get("DocNo"));
    }

    public List<Map<String, Object>> preInvoices(UserAccount u, int supplierCustomerId) {
        Map<String, Object> p = oc(u);
        p.put("DocumentTypeId", 209);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        return rows("UPS_GetPreInvoicenumberForCommercialInvoice", p);
    }

    public List<Map<String, Object>> preInvoiceHeader(int preInvoiceId) {
        return rows("UPS_GetPreInvoiceHeaderDataForCommercialInvoice", params("ExImInvoiceId", preInvoiceId));
    }

    public List<Map<String, Object>> prefixTypes() { return rows("usp_ExportInvoiceNosPrefix_History", params()); }

    public List<Map<String, Object>> finalInvoiceNos(UserAccount u, String invoiceNo) {
        Map<String, Object> p = oc(u);
        if (invoiceNo != null && !invoiceNo.isEmpty()) p.put("InvoiceNo", invoiceNo);
        p.put("Activity", "GetFinalInvoiceNosForCommercialInvoice");
        return rows("[dbo].[USP_ExportInvoiceNos_GetAllMethod]", p);
    }

    public List<Map<String, Object>> customers(UserAccount u) { return rowsA(u, "Sp_SupplierCustomer_GetAllMethod", "Activity", "ReadByOrganizationCompanyIdForExport"); }
    public List<Map<String, Object>> deliveryTerms() { return rows("Sp_ExImDeliveryTerm_GetAllMethod", params("Activity", "ReadAll")); }
    public List<Map<String, Object>> paymentTerms(UserAccount u) { return rows("Sp_ExImLcPaymentTerm_GetAllMethod", params("Activity", "ReadAll", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId())); }
    public List<Map<String, Object>> seaPorts(UserAccount u) { return rowsA(u, "Sp_SeaPorts_GetAllMethod", "Activity", "ReadByCompanyNOrganizationId"); }
    public List<Map<String, Object>> currencies(UserAccount u) { return rowsA(u, "Sp_MultiCurrency_GetAllMethod", "Activity", "ReadAll"); }
    public List<Map<String, Object>> banks(UserAccount u) { return rowsA(u, "Sp_Bank_GetAllMethod", "Activity", "ReadAll"); }
    public List<Map<String, Object>> packingTypes() { return rows("Sp_InvPackingType_GetAllMethod", params("Activity", "ReadAll")); }
    public List<Map<String, Object>> cropYears(UserAccount u) { return rowsA(u, "Sp_InvCropYear_GetAllMethod", "Activity", "ReadAll"); }
    public List<Map<String, Object>> branches(UserAccount u) { return rowsA(u, "Sp_Branches_GetAllMethod", "Activity", "GetAll"); }
    public List<Map<String, Object>> projects(UserAccount u) { return rowsA(u, "Sp_Projects_GetAllMethod", "MethodType", "GetAll"); }

    /** CommonServices.CoaAllocationAccountTitleByAccountTypeIds(ids). */
    public List<Map<String, Object>> coaByAccountTypes(UserAccount u, String accountTypeIds) {
        Map<String, Object> p = oc(u);
        p.put("AppId", u.getAppId() == null ? 0 : u.getAppId());
        if (accountTypeIds != null && !accountTypeIds.isEmpty()) p.put("AccountTypeIds", accountTypeIds);
        if (u.getId() != null && u.getId() != 0) p.put("UserId", u.getId());
        p.put("Activity", "GetAccountTitleByAccountTypeIds");
        return rows("Sp_COAAllocation_GetAllMethod", p);
    }

    public int lastCommissionDebitAccountId(UserAccount u) {
        List<Map<String, Object>> r = rowsA(u, INV, "Activity", "GetLastCommissionDebitAccountId");
        if (r.isEmpty() || r.get(0).isEmpty()) return 0;
        return num(r.get(0).values().iterator().next());
    }

    public List<Map<String, Object>> uomSchedule(UserAccount u, int itemId) {
        Map<String, Object> p = oc(u);
        p.put("ItemId", itemId);
        p.put("Activity", "ReadByItemID");
        return rows("Sp_UOMSchedule_GetAllMethod", p);
    }

    /** ExImLcOrder.GetOtherItemByContractId (BindOtherItems never sets obj.Id: @Id = 0). */
    public List<Map<String, Object>> otherItemsByContract(UserAccount u) {
        Map<String, Object> p = oc(u);
        p.put("Id", 0);
        p.put("Activity", "GetOtherItemByContractId");
        return rows("Sp_ExImLcOrder_GetAllMethod", p);
    }

    /** ExImEFormRegistration.GetFINoForInvoice (no customer, no RecId). */
    public List<Map<String, Object>> fiNos(UserAccount u) { return rows("usp_GetFINoForInvoice", oc(u)); }

    // ================================================================== history / read

    public List<Map<String, Object>> historyDropDowns(UserAccount u) { return rows("[dbo].[USP_GetDataForDropDownFromExportInvoice]", oc(u)); }

    public List<Map<String, Object>> formHistory(UserAccount u, int financialYearId, Map<String, Timestamp> dates, int supplierCustomerId,
                                                 int fromDocNo, int toDocNo) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "FinancialYearId", financialYearId);
        for (String k : new String[] { "FromDate", "ToDate", "EntryFromDate", "EntryToDate", "ModifyFromDate", "ModifyToDate", "ApprovedFromDate", "ApprovedToDate" })
            if (dates.get(k) != null) p.put(k, dates.get(k));
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        p.put("DocumentTypeIds", String.valueOf(DOCUMENT_TYPE_ID));
        if (fromDocNo != 0) p.put("DocNoFrom", fromDocNo);
        if (toDocNo != 0) p.put("DocNoTo", toDocNo);
        return rows("USP_ExImInvoice_FormHistory", p);
    }

    public Map<String, Object> header(int id) {
        List<Map<String, Object>> r = rows(INV, params("Id", id, "Activity", "ReadById"));
        return r.isEmpty() ? null : r.get(0);
    }

    public List<Map<String, Object>> child(int id, String activity) { return rows(INV, params("Id", id, "Activity", activity)); }

    // ================================================================== loader (LoadForwardingForCommercialInvoice)

    public List<Map<String, Object>> forwardingDropDowns(UserAccount u) {
        Map<String, Object> p = oc(u);
        if (u.getBranchesId() != null && u.getBranchesId() != 0) p.put("BranchesId", u.getBranchesId());
        return rows("USP_GetDataForDropDownFromGoodsForwarding", p);
    }

    public List<Map<String, Object>> packingExpiryDates(UserAccount u) { return rows("USP_getPackingExpiryFromForwarding", oc(u)); }
    public List<Map<String, Object>> productionNos(UserAccount u) { return rows("USP_getProductionNoFromForwarding", oc(u)); }

    public List<Map<String, Object>> loaderData(UserAccount u, int financialYearId, int contractId, int supplierCustomerId, int fcyId, int itemId,
                                                String packingExpiryDate, String productionNo, int skipZero) {
        Map<String, Object> p = oc(u);
        p.put("BranchesId", u.getBranchesId() == null ? 0 : u.getBranchesId());
        p.put("FinancialYearId", financialYearId);
        if (contractId != 0) p.put("ContractId", contractId);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        if (fcyId != 0) p.put("FcurrencyId", fcyId);
        if (itemId != 0) p.put("ItemId", itemId);
        if (packingExpiryDate != null && !packingExpiryDate.isEmpty()) p.put("PackingExpiryDate", packingExpiryDate);
        if (productionNo != null && !productionNo.isEmpty()) p.put("ProductionNo", productionNo);
        if (skipZero != 0) p.put("SkipZero", skipZero);
        return rows("USP_GetPreInvoicesAndForwardingDateForCommercialInvoices", p);
    }

    // ================================================================== voucher look-ups (BLL MakeVoucherForExImInvoice)

    public List<Map<String, Object>> supplierCustomerGl(UserAccount u) { return rowsA(u, "Sp_SupplierCustomer_GetAllMethod", "Activity", "GetGlAccountIdandCompanyNameBySupplierCustomerId"); }
    public List<Map<String, Object>> itemGl(UserAccount u) { return rowsA(u, "Sp_Item_GetAllMethod", "Activity", "GetItemGlIdsandItemName"); }

    private int voucherHeadId(int org, int company, int documentTypeId, int id) {
        List<Map<String, Object>> r = rows("Sp_Vouchers_GetMethods", params("Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId",
                "OrganizationId", org, "CompanyId", company, "DocumentTypeId", documentTypeId, "DocumentTypeSrNo", id));
        if (r.isEmpty()) return 0;
        Object v = r.get(0).get("Id");
        return num(v);
    }

    // ================================================================== save (DAL ExImInvoice.SetData)

    @Transactional(rollbackFor = Exception.class)
    public int save(Map<String, Object> header, List<Map<String, Object>> packing, List<Map<String, Object>> paymentTerms,
                    List<Map<String, Object>> otherItems, SaleInvoiceModels.VoucherHead vh, List<SaleInvoiceModels.VoucherDetail> details) {
        int org = num(header.get("OrganizationId")), company = num(header.get("CompanyId"));
        int documentTypeId = num(header.get("DocumentTypeId"));
        boolean insert = num(header.get("Id")) == 0;
        Integer v = DesktopProc.scalar(jdbc, insert ? "Sp_ExImInvoice_Insert" : "Sp_ExImInvoice_Update", header);
        int num3 = v == null ? 0 : v;
        int id;
        if (num3 > 0) id = num3; else { id = num(header.get("Id")); num3 = id; }
        int line = 0;
        for (Map<String, Object> d : packing) { d.put("LineId", ++line); d.put("ExImInvoiceId", id); DesktopProc.scalar(jdbc, "Sp_ExImInvoicePackingDetail_Insert", d); }
        for (Map<String, Object> d : paymentTerms) { d.put("ExImInvoiceId", id); DesktopProc.scalar(jdbc, "Sp_ExImInvoicePaymentTermsDetail_Insert", d); }
        for (Map<String, Object> d : otherItems) { d.put("ExImInvoiceId", id); DesktopProc.scalar(jdbc, "Sp_ExImInvoiceOtherItems_Insert", d); }
        if (vh != null && !details.isEmpty()) {
            int existing = voucherHeadId(org, company, documentTypeId, id);
            if (existing > 0) vh.Id = existing;
            vh.DocumentTypeSrNo = id;
            int n = setModel(existing == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", vh);
            if (n > 0) vh.Id = n;
            for (SaleInvoiceModels.VoucherDetail d : details) { d.VoucherHeadId = vh.Id; setModel("Sp_VoucherDetail_Insert", d); }
            if (vh.Id > 0) ProcExec.run(jdbc, "EXEC dbo.USP_VoucherBalanceCheck @OrganizationId=?, @CompanyId=?, @Id=?", org, company, vh.Id);
            int documentTypeIdRef = setModel("Sp_VoucherHead_H_Insert", vh);
            for (SaleInvoiceModels.VoucherDetail d : details) { d.VoucherHeadId = vh.Id; d.DocumentTypeIdRef = documentTypeIdRef; setModel("Sp_VoucherDetail_H_Insert", d); }
        }
        return num3;
    }

    /**
     * GenericProvider.SetProc over a model object: every public non-static field not marked @NotParam becomes @Name;
     * a null is not sent (a non-nullable DateTime left unset fails as "SqlDateTime overflow" does on the desktop).
     */
    private int setModel(String proc, Object model) {
        StringBuilder sql = new StringBuilder("EXEC ").append(proc.contains(".") ? proc : "dbo." + proc).append(' ');
        List<Object> args = new ArrayList<>();
        boolean first = true;
        for (Field f : model.getClass().getFields()) {
            if (Modifier.isStatic(f.getModifiers())) continue;
            if (f.isAnnotationPresent(SaleInvoiceModels.NotParam.class)) continue;
            Object val;
            try { val = f.get(model); } catch (IllegalAccessException e) { continue; }
            if (val == null) {
                if (f.getType() == LocalDateTime.class && !f.isAnnotationPresent(SaleInvoiceModels.Nullable.class))
                    throw new IllegalStateException("SqlDateTime overflow: " + model.getClass().getSimpleName() + "." + f.getName() + " was not set");
                continue;
            }
            Object bound;
            if (val instanceof LocalDateTime) bound = new SqlParameterValue(Types.TIMESTAMP, Timestamp.valueOf((LocalDateTime) val));
            else if (val instanceof BigDecimal) bound = new SqlParameterValue(Types.DECIMAL, val);
            else if (val instanceof Double) bound = new SqlParameterValue(Types.DOUBLE, val);
            else if (val instanceof Boolean) bound = new SqlParameterValue(Types.BIT, val);
            else if (val instanceof Integer) bound = new SqlParameterValue(Types.INTEGER, val);
            else bound = new SqlParameterValue(Types.NVARCHAR, String.valueOf(val));
            sql.append(first ? "" : ", ").append('@').append(f.getName()).append("=?");
            args.add(bound);
            first = false;
        }
        Integer r = ProcExec.call(jdbc, sql.toString(), args.toArray());
        return r == null ? 0 : r;
    }
}
