package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * 215 "Proforma Invoice" - Architecture.WinApp.Export.ProformaInvoice (ExImLcOrder, DocumentTypeId 151).
 * BLL ExImLcOrder (0469) / DAL ExImLcOrder (0521), each call the desktop's own procedure and parameters:
 *
 *   Sp_ExImLcOrder_GetAllMethod  @OrganizationId @CompanyId @DocumentTypeId [@FinancialYearId] @Activity='GenerateDocNoCompanyIdOrganizationId'  (Rows[0]["DocNo"])
 *                                @Id @Activity='ReadById'                                                   ExImLcOrder.GetByID header
 *                                @Id @Activity='ReadByLcOrderHeaderId' (DocumentTypeId 201) /
 *                                    'ReadByLcOrderHeaderIdForSaleContracttExport' (any other type)          LcOrderDetail
 *                                @Id @Activity='ReadExImLcOrderOtherItemsByHeaderId'                         ExImLcOrderOtherItemslist
 *   [dbo].[USP_ExImLcOrder_FormHistory]  @OrganizationId @CompanyId @DocumentTypeIds @CanViewAllRecord [@FinancialYearId]
 *                                [@EntryUser when not CanViewAllRecord] [date pair] [@DocNoFrom] [@DocNoTo] [@SupplierCustomerId]
 *   Sp_ExImLcOrder_Insert / Sp_ExImLcOrder_Update  (the 89 non-virtual ExImLcOrder properties)
 *   Sp_ExImLcOrderPackingDetail_Insert  (34 properties, ExImLcOrderId = header id; removed rows first with ActionTypeId 3)
 *   Sp_ExImLcOrderOtherItems_Insert     (8 properties)
 *   - DAL ExImLcOrder.SetDate, one transaction.
 *
 * No table, column or procedure is created or changed.
 */
@Repository
public class ExportProformaInvoiceRepository {

    public static final int DOCUMENT_TYPE_ID = 151;

    private final JdbcTemplate jdbc;

    public ExportProformaInvoiceRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    /** ExImLcOrder.GenerateCode: Rows[0]["DocNo"] - an empty result throws, as on the desktop. */
    public int generateCode(UserAccount u, int financialYearId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOCUMENT_TYPE_ID);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        p.put("Activity", "GenerateDocNoCompanyIdOrganizationId");
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ExImLcOrder_GetAllMethod", p);
        if (r.isEmpty()) throw new IllegalStateException("There is no row at position 0.");
        Object v = r.get(0).get("DocNo");
        return v instanceof Number ? ((Number) v).intValue() : 0;
    }

    public Map<String, Object> header(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ExImLcOrder_GetAllMethod", params("Id", id, "Activity", "ReadById"));
        if (r.isEmpty()) throw new IllegalStateException("Index was out of range. Must be non-negative and less than the size of the collection.");
        return r.get(0);
    }

    public List<Map<String, Object>> detail(int id, int documentTypeId) {
        return DesktopProc.rows(jdbc, "Sp_ExImLcOrder_GetAllMethod", params("Id", id,
                "Activity", documentTypeId == 201 ? "ReadByLcOrderHeaderId" : "ReadByLcOrderHeaderIdForSaleContracttExport"));
    }

    public List<Map<String, Object>> otherItems(int id) {
        return DesktopProc.rows(jdbc, "Sp_ExImLcOrder_GetAllMethod", params("Id", id, "Activity", "ReadExImLcOrderOtherItemsByHeaderId"));
    }

    /** ExImLcOrder.FormHistory - parameters added exactly as the BLL adds them (the service builds the guarded part). */
    public List<Map<String, Object>> history(UserAccount u, boolean canViewAll, int financialYearId, Map<String, Object> guarded) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeIds", String.valueOf(DOCUMENT_TYPE_ID), "CanViewAllRecord", canViewAll);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        if (!canViewAll) p.put("EntryUser", u.getId());
        p.putAll(guarded);
        return DesktopProc.rows(jdbc, "[dbo].[USP_ExImLcOrder_FormHistory]", p);
    }

    /** DAL ExImLcOrder.SetDate for the lists this form fills (detail, other items). */
    @Transactional(rollbackFor = Exception.class)
    public int save(Map<String, Object> header, String proc, List<Map<String, Object>> details, List<Map<String, Object>> otherItems) {
        int num = DesktopProc.setProc(jdbc, proc, header);
        if (num > 0) header.put("Id", num);
        else num = ExportPreInvoiceRepository.asInt(header.get("Id"));
        int id = ExportPreInvoiceRepository.asInt(header.get("Id"));
        for (Map<String, Object> d : details) {
            d.put("ExImLcOrderId", id);
            DesktopProc.setProc(jdbc, "Sp_ExImLcOrderPackingDetail_Insert", d);
        }
        for (Map<String, Object> o : otherItems) {
            o.put("ExImLcOrderId", id);
            DesktopProc.setProc(jdbc, "Sp_ExImLcOrderOtherItems_Insert", o);
        }
        return num;
    }
}
