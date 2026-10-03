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
 * Data layer of 200 "Export Performa Invoice" - Architecture.WinApp.Export.ExImProformaInvoice
 * (BLL 0477 Architecture.BLL.Export.ExImProformaInvoice, DAL 0529, models 0871 / 0872; table dbo.ExImProformaInvoice
 * + dbo.ExImProformaInvoiceDetail, DocumentTypeId 200). Each method is one desktop BLL call with the desktop's own
 * procedure and parameters (verified in procdure_index.csv / procdure.utf8.sql, 03-Oct-2026):
 *
 *   Sp_ExImProformaInvoice_GetAllMethod  @OrganizationId @CompanyId @DocumentTypeId @Activity='GenerateDocNoByDocumentTypeId'  GenerateCode
 *                                        @OrganizationId @CompanyId @Activity='ReadByOrganizationCompanyId'                    GetData (history)
 *                                        @Id @Activity='ReadById' + per header @Id @Activity='ReadByIdProformaInvoiceHeaderId' GetByID (DAL GetDate)
 *   Sp_ExImProformaInvoice_Insert | Sp_ExImProformaInvoice_Update (28 params = every non-virtual model property; the
 *        Update proc deletes the old ExImProformaInvoiceDetail rows), then Sp_ExImProformaInvoiceDetail_Insert per row
 *        (14 params) - DAL SetDate, one transaction.
 *   Lookups: Sp_Branches_GetAllMethod 'GetAll' (CommonServices.BrancheServiceBind), Sp_Projects_GetAllMethod @MethodType='GetAll'
 *        (ProjectServiceBind), Sp_SupplierCustomer_GetAllMethod 'ReadByOrganizationCompanyIdForExport', Sp_SeaPorts_GetAllMethod
 *        'ReadByCompanyNOrganizationId', Sp_MultiCurrency_GetAllMethod 'ReadAll', Sp_Item_GetAllMethod
 *        'GetExportItemsByOrganizationCompanyId', Sp_ExImPackMaterilaType_GetAllMethod 'ReadAll', Sp_UOMSchedule_GetAllMethod
 *        @ItemId 'ReadByItemID' (UOMSchedule.SearchByObjectList).
 */
@Repository
public class ExportPerformaInvoiceRepository {

    private final JdbcTemplate jdbc;

    public ExportPerformaInvoiceRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    private Map<String, Object> oc(UserAccount u) { return params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()); }

    private List<Map<String, Object>> oc(UserAccount u, String proc, String activityKey, String activity) {
        Map<String, Object> p = oc(u);
        p.put(activityKey, activity);
        return DesktopProc.rows(jdbc, proc, p);
    }

    public List<Map<String, Object>> branches(UserAccount u) { return oc(u, "Sp_Branches_GetAllMethod", "Activity", "GetAll"); }
    public List<Map<String, Object>> projects(UserAccount u) { return oc(u, "Sp_Projects_GetAllMethod", "MethodType", "GetAll"); }
    public List<Map<String, Object>> customers(UserAccount u) { return oc(u, "Sp_SupplierCustomer_GetAllMethod", "Activity", "ReadByOrganizationCompanyIdForExport"); }
    public List<Map<String, Object>> seaPorts(UserAccount u) { return oc(u, "Sp_SeaPorts_GetAllMethod", "Activity", "ReadByCompanyNOrganizationId"); }
    public List<Map<String, Object>> currencies(UserAccount u) { return oc(u, "Sp_MultiCurrency_GetAllMethod", "Activity", "ReadAll"); }
    public List<Map<String, Object>> items(UserAccount u) { return oc(u, "Sp_Item_GetAllMethod", "Activity", "GetExportItemsByOrganizationCompanyId"); }
    public List<Map<String, Object>> packTypes(UserAccount u) { return oc(u, "Sp_ExImPackMaterilaType_GetAllMethod", "Activity", "ReadAll"); }

    public List<Map<String, Object>> uoms(UserAccount u, int itemId) {
        Map<String, Object> p = oc(u);
        p.put("ItemId", itemId);
        p.put("Activity", "ReadByItemID");
        return DesktopProc.rows(jdbc, "Sp_UOMSchedule_GetAllMethod", p);
    }

    /** ExImProformaInvoice.GenerateCode: Rows[0]["DocNo"]. */
    public int generateCode(UserAccount u, int documentTypeId) {
        Map<String, Object> p = oc(u);
        p.put("DocumentTypeId", documentTypeId);
        p.put("Activity", "GenerateDocNoByDocumentTypeId");
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ExImProformaInvoice_GetAllMethod", p);
        if (r.isEmpty()) throw new IllegalStateException("There is no row at position 0.");
        Object v = r.get(0).get("DocNo");
        return v instanceof Number ? ((Number) v).intValue() : 0;
    }

    /** ExImProformaInvoice.GetData. */
    public List<Map<String, Object>> history(UserAccount u) {
        return oc(u, "Sp_ExImProformaInvoice_GetAllMethod", "Activity", "ReadByOrganizationCompanyId");
    }

    /** GetByID header (DAL GetDate list[0]). */
    public Map<String, Object> header(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ExImProformaInvoice_GetAllMethod", params("Id", id, "Activity", "ReadById"));
        if (r.isEmpty()) throw new IllegalStateException("Index was out of range. Must be non-negative and less than the size of the collection.");
        return r.get(0);
    }

    public List<Map<String, Object>> detail(int id) {
        return DesktopProc.rows(jdbc, "Sp_ExImProformaInvoice_GetAllMethod", params("Id", id, "Activity", "ReadByIdProformaInvoiceHeaderId"));
    }

    /**
     * DAL ExImProformaInvoice.SetDate: header proc (Insert returns SCOPE_IDENTITY, Update returns nothing -> 0 -> the
     * record's own Id), each detail row with ExImProformaInvoiceHeaderId = header Id; the return value is the LAST
     * scalar (the last detail Id, or the header Id when the grid is empty) exactly as the desktop's "success".
     */
    @Transactional(rollbackFor = Exception.class)
    public int save(Map<String, Object> header, String proc, List<Map<String, Object>> details) {
        int num = DesktopProc.setProc(jdbc, proc, header);
        int id;
        if (num > 0) { id = num; } else { id = header.get("Id") instanceof Number ? ((Number) header.get("Id")).intValue() : 0; num = id; }
        for (Map<String, Object> d : details) {
            d.put("ExImProformaInvoiceHeaderId", id);
            num = DesktopProc.setProc(jdbc, "Sp_ExImProformaInvoiceDetail_Insert", d);
        }
        return num;
    }
}
