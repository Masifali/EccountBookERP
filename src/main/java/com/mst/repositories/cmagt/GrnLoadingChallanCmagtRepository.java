package com.mst.repositories.cmagt;

import com.mst.models.cmagt.dto.GrnLoadingChallanCmagtDto;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.namedparam.MapSqlParameterSource;
import org.springframework.jdbc.core.simple.SimpleJdbcCall;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.stereotype.Repository;

import java.math.BigDecimal;
import java.text.SimpleDateFormat;
import java.util.*;

@Repository
public class GrnLoadingChallanCmagtRepository {

    @Autowired
    private JdbcTemplate jdbcTemplate;

    private static final SimpleDateFormat DATE_FORMAT = new SimpleDateFormat("yyyy-MM-dd");

    /**
     * DAL grnSupplierLoadingMaster.SetData - ONE transaction over the master and all three
     * child collections, exactly as the desktop does it. @Transactional gives the same
     * all-or-nothing boundary: a challan must not survive with a master row and no details.
     *
     * The desktop's own guard comes first: SetData throws "Detail list not found" when the
     * detail list is empty, so an empty save is refused here rather than writing a headless
     * master row.
     *
     * Every parameter of every procedure is passed. SimpleJdbcCall applies the procedure's
     * own default for anything omitted, which is how the previous 15-of-43 version reported
     * success while storing no freight, weights, transporter, cities, delivery term or
     * vehicle type.
     */
    @Transactional
    public Map<String, Object> saveOrUpdate(GrnLoadingChallanCmagtDto dto) {
        Map<String, Object> result = new HashMap<>();

        if (dto.getGrnSupplierLoadingDetailList() == null
                || dto.getGrnSupplierLoadingDetailList().isEmpty()) {
            throw new IllegalArgumentException("Detail list not found");
        }

        SimpleJdbcCall masterCall = new SimpleJdbcCall(jdbcTemplate)
                .withSchemaName("cmagt")
                .withProcedureName("USP_grnSupplierLoadingMaster_InsertAndUpdate");

        MapSqlParameterSource masterParams = new MapSqlParameterSource();
        masterParams.addValue("isApproved", nb(dto.isApproved()));
        masterParams.addValue("isSupplierOtherChargesAllowed", nb(dto.isSupplierOtherChargesAllowed()));
        masterParams.addValue("approvedDate", parseDate(dto.getApprovedDate()));
        masterParams.addValue("biltyDate", parseDate(dto.getBiltyDate()));
        /* Nullable DateTime? on the model; frmGrnLoadingChallanCmagt.Insert() never assigns it, so
           the desktop sends NULL. parseDate() would have stamped today. */
        masterParams.addValue("deliveryStartDate", parseDateOrNull(dto.getDeliveryStartDate()));
        masterParams.addValue("docDate", parseDate(dto.getDocDate()));
        masterParams.addValue("entryDate", parseDate(dto.getEntryDate()));
        masterParams.addValue("modifyDate", parseDate(dto.getModifyDate()));
        masterParams.addValue("biltyFreight", nd(dto.getBiltyFreight()));
        masterParams.addValue("biltyQty", nd(dto.getBiltyQty()));
        masterParams.addValue("loadWeight", nd(dto.getLoadWeight()));
        masterParams.addValue("otherAdLesCharges", nd(dto.getOtherAdLesCharges()));
        masterParams.addValue("scaleNetWeight", nd(dto.getScaleNetWeight()));
        masterParams.addValue("tareWeight", nd(dto.getTareWeight()));
        masterParams.addValue("totalFreight", nd(dto.getTotalFreight()));
        masterParams.addValue("actionId", ni(dto.getActionId()));
        masterParams.addValue("approvedUserId", ni(dto.getApprovedUserId()));
        masterParams.addValue("branchId", ni(dto.getBranchId()));
        masterParams.addValue("commissionAgentId", ni(dto.getCommissionAgentId()));
        masterParams.addValue("companyId", ni(dto.getCompanyId()));
        masterParams.addValue("deliveryDays", ni(dto.getDeliveryDays()));
        masterParams.addValue("deliveryTermId", ni(dto.getDeliveryTermId()));
        masterParams.addValue("docNo", ni(dto.getDocNo()));
        masterParams.addValue("documentTypeId", ni(dto.getDocumentTypeId()));
        masterParams.addValue("entryUserId", ni(dto.getEntryUserId()));
        masterParams.addValue("financialYearId", ni(dto.getFinancialYearId()));
        masterParams.addValue("grnSupplierLoadingMasterId", ni(dto.getGrnSupplierLoadingMasterId()));
        masterParams.addValue("loadingCityId", ni(dto.getLoadingCityId()));
        masterParams.addValue("modifyUserId", ni(dto.getModifyUserId()));
        masterParams.addValue("organizationId", ni(dto.getOrganizationId()));
        masterParams.addValue("projectId", ni(dto.getProjectId()));
        masterParams.addValue("supplierId", ni(dto.getSupplierId()));
        masterParams.addValue("transporterId", ni(dto.getTransporterId()));
        masterParams.addValue("unloadingCityId", ni(dto.getUnloadingCityId()));
        masterParams.addValue("vehicleTypeId", ni(dto.getVehicleTypeId()));
        masterParams.addValue("approvalRemarks", dto.getApprovalRemarks());
        masterParams.addValue("attachmentsValues", dto.getAttachmentsValues());
        masterParams.addValue("biltyNo", ns(dto.getBiltyNo()));
        masterParams.addValue("customAttachmentsValues", dto.getCustomAttachmentsValues());
        masterParams.addValue("remarksHeader", ns(dto.getRemarksHeader()));
        masterParams.addValue("supplierRefDocNo", dto.getSupplierRefDocNo());
        masterParams.addValue("transporterName", dto.getTransporterName());
        masterParams.addValue("vehicleNo", ns(dto.getVehicleNo()));

        Map<String, Object> masterOut = masterCall.execute(masterParams);
        Integer masterId = extractReturnedId(masterOut);
        if (masterId == null || masterId <= 0) {
            masterId = dto.getGrnSupplierLoadingMasterId();
        }
        if (masterId == null || masterId <= 0) {
            /* Never write children against master 0 - the DAL would not either, because
               ExecuteScalar always yields the id the procedure SELECTs. */
            throw new IllegalStateException("Master id was not returned by USP_grnSupplierLoadingMaster_InsertAndUpdate");
        }

        /* SetData assigns the returned master id onto every child before inserting it. */
        for (GrnLoadingChallanCmagtDto.DetailDto d : dto.getGrnSupplierLoadingDetailList()) {
            d.setGrnSupplierLoadingMasterId(masterId);
            /* BLL: actionTypeId = detailId <= 0 ? 1 : 2 (btnSave_Click :2680). */
            /* Rows the operator removed from a saved challan arrive with actionTypeId 3
               (DeleteDetailRow :2142 -> lstRemoveRecordDetail, added in Insert() :2593) and
               must stay 3 - recomputing them to 2 re-saved a deleted row instead of
               soft-deleting it. */
            if (d.getActionTypeId() == null || d.getActionTypeId() != 3) {
                d.setActionTypeId((d.getGrnSupplierLoadingDetailId() == null
                        || d.getGrnSupplierLoadingDetailId() <= 0) ? 1 : 2);
            } else if (d.getGrnSupplierLoadingDetailId() == null || d.getGrnSupplierLoadingDetailId() <= 0) {
                continue;   /* an unsaved row that was removed never reaches the procedure */
            }
            MapSqlParameterSource p = new MapSqlParameterSource();
            p.addValue("addLessWeight", nd(d.getAddLessWeight()));
            p.addValue("ebwPerUnit", nd(d.getEbwPerUnit()));
            p.addValue("ebwTotal", nd(d.getEbwTotal()));
            p.addValue("loadingQty", nd(d.getLoadingQty()));
            p.addValue("netBillWeight", nd(d.getNetBillWeight()));
            p.addValue("wbGrossWeight", nd(d.getWbGrossWeight()));
            p.addValue("actionTypeId", ni(d.getActionTypeId()));
            p.addValue("buyerId", ni(d.getBuyerId()));
            p.addValue("cropYearId", ni(d.getCropYearId()));
            p.addValue("DeliverToAddressId", ni(d.getDeliverToAddressId()));
            p.addValue("DeliverToPartyId", ni(d.getDeliverToPartyId()));
            p.addValue("grnSupplierLoadingDetailId", ni(d.getGrnSupplierLoadingDetailId()));
            p.addValue("grnSupplierLoadingMasterId", ni(d.getGrnSupplierLoadingMasterId()));
            p.addValue("inquiryBookingDetailId", ni(d.getInquiryBookingDetailId()));
            p.addValue("inquiryBookingMasterId", ni(d.getInquiryBookingMasterId()));
            p.addValue("inventoryParentCategoryId", ni(d.getInventoryParentCategoryId()));
            p.addValue("itemId", ni(d.getItemId()));
            p.addValue("loadingCityId", ni(d.getLoadingCityId()));
            p.addValue("modifyUserId", ni(d.getModifyUserId()));
            p.addValue("packingTypeId", ni(d.getPackingTypeId()));
            p.addValue("packUomId", ni(d.getPackUomId()));
            p.addValue("purchaseOrderDetailId", ni(d.getPurchaseOrderDetailId()));
            p.addValue("purchaseOrderMasterId", ni(d.getPurchaseOrderMasterId()));
            p.addValue("saleOrderDetailId", ni(d.getSaleOrderDetailId()));
            p.addValue("saleOrderMasterId", ni(d.getSaleOrderMasterId()));
            p.addValue("EBWeightDeductionTermId", ni(d.getEBWeightDeductionTermId()));
            p.addValue("purchaseOrderSaleOrderMappingId", ni(d.getPurchaseOrderSaleOrderMappingId()));
            p.addValue("sortNo", ni(d.getSortNo()));
            p.addValue("supplierOfferDetailId", ni(d.getSupplierOfferDetailId()));
            p.addValue("supplierOfferId", ni(d.getSupplierOfferId()));
            p.addValue("unloadingCityId", ni(d.getUnloadingCityId()));
            p.addValue("cropYear", ns(d.getCropYear()));
            p.addValue("remarks", ns(d.getRemarks()));
            p.addValue("warningRemarks", ns(d.getWarningRemarks()));
            p.addValue("DeliverToAddress", ns(d.getDeliverToAddress()));
            new SimpleJdbcCall(jdbcTemplate).withSchemaName("cmagt")
                    .withProcedureName("USP_grnSupplierLoadingDetail_Insert").execute(p);
        }

        if (dto.getGrnSupplierLoadingEmptyBagDetailList() != null) {
            for (GrnLoadingChallanCmagtDto.EmptyBagDto d : dto.getGrnSupplierLoadingEmptyBagDetailList()) {
                d.setGrnSupplierLoadingMasterId(masterId);
                MapSqlParameterSource p = new MapSqlParameterSource();
                p.addValue("Rate", nd(d.getRate()));
                p.addValue("weightCutKg", nd(d.getWeightCutKg()));
                p.addValue("grnSupplierLoadingEmptyBagDetailId", ni(d.getGrnSupplierLoadingEmptyBagDetailId()));
                p.addValue("grnSupplierLoadingMasterId", ni(d.getGrnSupplierLoadingMasterId()));
                p.addValue("purchaseOrderMasterId", ni(d.getPurchaseOrderMasterId()));
                p.addValue("purchaseOrderEmptyBagDetailId", ni(d.getPurchaseOrderEmptyBagDetailId()));
                p.addValue("PackingTypeId", ni(d.getPackingTypeId()));
                p.addValue("emptyBagPackingMaterialItemId", ni(d.getEmptyBagPackingMaterialItemId()));
                p.addValue("sortNo", ni(d.getSortNo()));
                p.addValue("remarks", ns(d.getRemarks()));
                new SimpleJdbcCall(jdbcTemplate).withSchemaName("cmagt")
                        .withProcedureName("USP_grnSupplierLoadingEmptyBagDetail_Insert").execute(p);
            }
        }

        if (dto.getGrnSupplierLoadingExpenseDetailList() != null) {
            for (GrnLoadingChallanCmagtDto.ExpenseDto d : dto.getGrnSupplierLoadingExpenseDetailList()) {
                d.setGrnSupplierLoadingMasterId(masterId);
                MapSqlParameterSource p = new MapSqlParameterSource();
                p.addValue("Qty", nd(d.getQty()));
                p.addValue("amount", no(d.getAmount()));
                p.addValue("rate", no(d.getRate()));
                p.addValue("grnSupplierLoadingExpenseDetailId", ni(d.getGrnSupplierLoadingExpenseDetailId()));
                p.addValue("grnSupplierLoadingMasterId", ni(d.getGrnSupplierLoadingMasterId()));
                p.addValue("purchaseOrderMasterId", ni(d.getPurchaseOrderMasterId()));
                p.addValue("purchaseOrderExpenseDetailId", ni(d.getPurchaseOrderExpenseDetailId()));
                p.addValue("ItemId", ni(d.getItemId()));
                p.addValue("sortNo", ni(d.getSortNo()));
                p.addValue("remarks", ns(d.getRemarks()));
                new SimpleJdbcCall(jdbcTemplate).withSchemaName("cmagt")
                        .withProcedureName("USP_grnSupplierLoadingExpenseDetail_Insert").execute(p);
            }
        }

        result.put("status", "SUCCESS");
        result.put("id", masterId);
        result.put("grnSupplierLoadingMasterId", masterId);
        return result;
    }

    /* Null-safe coercions. A null never becomes 1 or any other invented id - it becomes the
       type's zero, and tenancy/user are guaranteed non-null by the service. */
    private static int ni(Integer v) { return v == null ? 0 : v; }
    private static boolean nb(Boolean v) { return v != null && v; }
    private static String ns(String v) { return v == null ? "" : v; }
    private static BigDecimal nd(BigDecimal v) { return v == null ? BigDecimal.ZERO : v; }
    private static double no(Double v) { return v == null ? 0d : v; }

    /**
     * "Load Purchase Order" - frmGrnLoadingChallanCmagt:4292 opens
     * frmLoadPurchaseOrderForGrnLoading, whose grid is filled by :309
     *   purchaseOrderMaster.PendingDataLoaderForGrn (BLL 0490)
     *   -> [cmagt].[USP_purchaseOrderMaster_PendingDataLoaderForGrn]
     *
     * Four parameters are ALWAYS sent (OrganizationId, CompanyId, BranchesId,
     * FinancialYearId); the rest are guarded in the BLL and omitted when unset, so they are
     * omitted here too - a guarded parameter sent as NULL is not the same call.
     *
     * This is how a GRN detail row gets buyerId, the deliver-to party and address, the
     * source purchase-order ids, the item, pack UOM, crop year and the empty-bag weight
     * terms. They are carried from the source document, not typed, which is why the detail
     * grid alone could never populate the procedure's 35 parameters.
     */
    public List<Map<String, Object>> pendingPurchaseOrdersForGrn(
            int organizationId, int companyId, int branchId, int financialYearId,
            String fromDate, String toDate, Integer fromDocNo, Integer toDocNo,
            Integer recId, Integer commissionAgentId, Integer supplierId, Integer itemId,
            Integer deliveryToPartyId, String shipToAddress) {

        MapSqlParameterSource p = new MapSqlParameterSource();
        p.addValue("OrganizationId", organizationId);
        p.addValue("CompanyId", companyId);
        p.addValue("BranchesId", branchId);
        p.addValue("FinancialYearId", financialYearId);
        if (notBlank(fromDate))                          p.addValue("FromDate", parseDate(fromDate));
        if (notBlank(toDate))                            p.addValue("ToDate", parseDate(toDate));
        if (fromDocNo != null && fromDocNo != 0)         p.addValue("FromDocNo", fromDocNo);
        if (toDocNo != null && toDocNo != 0)             p.addValue("ToDocNo", toDocNo);
        if (recId != null && recId != 0)                 p.addValue("RecId", recId);
        if (commissionAgentId != null && commissionAgentId != 0) p.addValue("CommissionAgentId", commissionAgentId);
        if (supplierId != null && supplierId != 0)       p.addValue("supplierId", supplierId);
        if (itemId != null && itemId != 0)               p.addValue("ItemId", itemId);
        if (deliveryToPartyId != null && deliveryToPartyId != 0) p.addValue("DeliveryToPartyId", deliveryToPartyId);
        if (notBlank(shipToAddress))                     p.addValue("ShipToAddress", shipToAddress);

        return pendingLoaderTables(p).get("pendingOrders");
    }

    /**
     * The SAME call, returning EVERY result set.
     *
     * BLL purchaseOrderMaster.PendingDataLoaderForGrn returns a DataSet, and
     * frmGrnLoadingChallanCmagt.cs:910-915 copies FIVE tables out of it:
     *
     *   Tables[0] -> dtPendingOrdersData           pending PO rows; feeds the Order No combo
     *                                              (OrderNoBind:923) and the PO-scoped item
     *                                              combo, which is keyed on MappingId and
     *                                              carries OrderDetailId / SoId / SoDetailId /
     *                                              SoNo / BuyerId / DeliveryToPartyId /
     *                                              ShipToAddressId / ShipToAddress
     *   Tables[1] -> dtSaleOrdersPurchaseOrderWise sale orders per purchase order
     *   Tables[2] -> dtEmptyBagDataOrderWise       empty-bag rows per order
     *   Tables[3] -> dtExpenseDataOrderWise        expense rows per order
     *   Tables[4] -> dtEmptyBagCutDataOrderWise    empty-bag weight-cut rows per order
     *
     * extractList() returned the FIRST list it found and dropped the other four, so the
     * screen could never populate its empty-bag, weight-cut or expense grids from a chosen
     * purchase order - the linkage the whole document depends on. All five are returned now,
     * under the desktop's own names.
     */
    public Map<String, List<Map<String, Object>>> pendingPurchaseOrderTables(
            int organizationId, int companyId, int branchId, int financialYearId,
            String fromDate, String toDate, Integer fromDocNo, Integer toDocNo,
            Integer recId, Integer commissionAgentId, Integer supplierId, Integer itemId,
            Integer deliveryToPartyId, String shipToAddress) {

        MapSqlParameterSource p = new MapSqlParameterSource();
        p.addValue("OrganizationId", organizationId);
        p.addValue("CompanyId", companyId);
        p.addValue("BranchesId", branchId);
        p.addValue("FinancialYearId", financialYearId);
        if (notBlank(fromDate))                          p.addValue("FromDate", parseDate(fromDate));
        if (notBlank(toDate))                            p.addValue("ToDate", parseDate(toDate));
        if (fromDocNo != null && fromDocNo != 0)         p.addValue("FromDocNo", fromDocNo);
        if (toDocNo != null && toDocNo != 0)             p.addValue("ToDocNo", toDocNo);
        if (recId != null && recId != 0)                 p.addValue("RecId", recId);
        if (commissionAgentId != null && commissionAgentId != 0) p.addValue("CommissionAgentId", commissionAgentId);
        if (supplierId != null && supplierId != 0)       p.addValue("supplierId", supplierId);
        if (itemId != null && itemId != 0)               p.addValue("ItemId", itemId);
        if (deliveryToPartyId != null && deliveryToPartyId != 0) p.addValue("DeliveryToPartyId", deliveryToPartyId);
        if (notBlank(shipToAddress))                     p.addValue("ShipToAddress", shipToAddress);
        return pendingLoaderTables(p);
    }

    /**
     * SimpleJdbcCall returns each result set under a generated key (#result-set-1, ...), and
     * iteration order of the returned map is not guaranteed to be the procedure's order, so
     * the keys are sorted before they are mapped onto the desktop's five names. Fewer tables
     * than expected yields fewer entries rather than a wrong assignment.
     */
    @SuppressWarnings("unchecked")
    private Map<String, List<Map<String, Object>>> pendingLoaderTables(MapSqlParameterSource p) {
        Map<String, Object> out = new SimpleJdbcCall(jdbcTemplate)
                .withSchemaName("cmagt")
                .withProcedureName("USP_purchaseOrderMaster_PendingDataLoaderForGrn")
                .execute(p);

        java.util.List<String> keys = new java.util.ArrayList<>();
        for (Map.Entry<String, Object> e : out.entrySet()) {
            if (e.getValue() instanceof List) keys.add(e.getKey());
        }
        java.util.Collections.sort(keys);

        String[] names = { "pendingOrders", "saleOrdersByPurchaseOrder", "emptyBagsByOrder",
                           "expensesByOrder", "emptyBagCutByOrder" };
        Map<String, List<Map<String, Object>>> result = new java.util.LinkedHashMap<>();
        for (String n : names) result.put(n, Collections.emptyList());
        for (int i = 0; i < keys.size() && i < names.length; i++) {
            result.put(names[i], (List<Map<String, Object>>) out.get(keys.get(i)));
        }
        return result;
    }

    private static boolean notBlank(String v) { return v != null && !v.trim().isEmpty(); }

    /**
     * BLL grnSupplierLoadingMaster.FormHistory -> usp_grnSupplierLoadingMaster_GetAllMethod
     * @Activity='FormHistory'. The old call used 'ReadBySearch_grnSupplierLoadingMaster', an
     * Activity the procedure has no branch for, so the history grid was always empty.
     *
     * Always sent: OrganizationId, CompanyId, BranchesId, FinancialYearId, CanViewAllRecord.
     * EntryUserId only when !CanViewAllRecord; FromDate / ToDate only when set. The BLL sends
     * no DocumentTypeId on this path.
     */
    public List<Map<String, Object>> formHistory(int organizationId, int companyId, int branchId,
                                                 int financialYearId, boolean canViewAllRecord,
                                                 int entryUserId, String fromDate, String toDate) {
        MapSqlParameterSource params = new MapSqlParameterSource();
        params.addValue("OrganizationId", organizationId);
        params.addValue("CompanyId", companyId);
        params.addValue("BranchesId", branchId);
        params.addValue("FinancialYearId", financialYearId);
        params.addValue("CanViewAllRecord", canViewAllRecord ? 1 : 0);
        if (!canViewAllRecord) params.addValue("EntryUserId", entryUserId);
        if (notBlank(fromDate)) params.addValue("FromDate", parseDateOrNull(fromDate));
        if (notBlank(toDate))   params.addValue("ToDate", parseDateOrNull(toDate));
        params.addValue("Activity", "FormHistory");
        return extractList(getAll().execute(params));
    }

    private SimpleJdbcCall getAll() {
        return new SimpleJdbcCall(jdbcTemplate)
                .withSchemaName("cmagt")
                .withProcedureName("usp_grnSupplierLoadingMaster_GetAllMethod");
    }

    /**
     * ReadDetailByHeaderId selects DeliverToAddress TWICE - first the CASE (AddressTitle when an
     * address id is set, else the stored text), then sta.AddressLine1. The desktop's reader
     * binds by name and SqlDataReader resolves a duplicate name to the FIRST column; Spring's
     * ColumnMapRowMapper keeps the LAST, so an Update after a load wrote AddressLine1 (or NULL)
     * back over the stored address. This mapper keeps the first occurrence, as the desktop does.
     */
    private static final org.springframework.jdbc.core.RowMapper<Map<String, Object>> FIRST_COLUMN_WINS =
            (rs, rowNum) -> {
                java.sql.ResultSetMetaData md = rs.getMetaData();
                int n = md.getColumnCount();
                Map<String, Object> m = new org.springframework.util.LinkedCaseInsensitiveMap<>(n);
                for (int i = 1; i <= n; i++) {
                    String k = org.springframework.jdbc.support.JdbcUtils.lookupColumnName(md, i);
                    if (!m.containsKey(k)) {
                        m.put(k, org.springframework.jdbc.support.JdbcUtils.getResultSetValue(rs, i));
                    }
                }
                return m;
            };

    private List<Map<String, Object>> readActivity(String activity, int id) {
        MapSqlParameterSource p = new MapSqlParameterSource();
        p.addValue("Id", id);
        p.addValue("Activity", activity);
        return extractList(getAll().returningResultSet("rows", FIRST_COLUMN_WINS).execute(p));
    }

    /**
     * BLL ReadById -> DAL GetData: @Activity='ReadById' for the header, then per header
     * 'ReadDetailByHeaderId', 'grnSupplierLoadingExpensesDetailByHeaderId' and
     * 'grnSupplierLoadingEmptyBagDetailByHeaderId'. The old call used
     * 'ReadById_grnSupplierLoadingMaster', which the procedure does not handle, so every open
     * from history answered "Record not found".
     *
     * ReadById has no tenancy filter in the procedure, so the header's organization and
     * company are checked against the session here. The form refuses a record with no detail
     * rows ("Record Not Found", ReadById :2818).
     */
    public Map<String, Object> getById(Integer id, int organizationId, int companyId) {
        Map<String, Object> result = new HashMap<>();
        List<Map<String, Object>> headers = (id == null || id <= 0)
                ? Collections.emptyList() : readActivity("ReadById", id);
        Map<String, Object> header = headers.isEmpty() ? null : new HashMap<>(headers.get(0));
        if (header != null && !(sameInt(header.get("organizationId"), organizationId)
                && sameInt(header.get("companyId"), companyId))) {
            header = null;
        }
        List<Map<String, Object>> details = header == null
                ? Collections.emptyList() : readActivity("ReadDetailByHeaderId", id);
        if (header == null || details.isEmpty()) {
            result.put("status", "ERROR");
            result.put("message", "Record Not Found");
            return result;
        }
        header.put("grnSupplierLoadingDetailList", details);
        header.put("grnSupplierLoadingExpenseDetailList",
                readActivity("grnSupplierLoadingExpensesDetailByHeaderId", id));
        header.put("grnSupplierLoadingEmptyBagDetailList",
                readActivity("grnSupplierLoadingEmptyBagDetailByHeaderId", id));
        result.put("status", "SUCCESS");
        result.put("data", header);
        return result;
    }

    /** True when the saved header belongs to this organization and company. */
    public boolean belongsTo(int id, int organizationId, int companyId) {
        List<Map<String, Object>> h = readActivity("ReadById", id);
        return !h.isEmpty() && sameInt(h.get(0).get("organizationId"), organizationId)
                && sameInt(h.get(0).get("companyId"), companyId);
    }

    /**
     * BLL DeleteByID: @EntryUserId, @Id, @Activity='DeleteById' in its own transaction.
     * The procedure refuses approved records and records already dispatched in a GDN.
     */
    @Transactional
    public void deleteById(int entryUserId, int id) {
        MapSqlParameterSource p = new MapSqlParameterSource();
        p.addValue("EntryUserId", entryUserId);
        p.addValue("Id", id);
        p.addValue("Activity", "DeleteById");
        getAll().execute(p);
    }

    /** BLL GenerateCode: org, company, branch, year, DocumentTypeId, @Activity='GenerateCode'. */
    public int generateCode(int organizationId, int companyId, int branchId,
                            int financialYearId, int documentTypeId) {
        MapSqlParameterSource p = new MapSqlParameterSource();
        p.addValue("OrganizationId", organizationId);
        p.addValue("CompanyId", companyId);
        p.addValue("BranchesId", branchId);
        p.addValue("FinancialYearId", financialYearId);
        p.addValue("DocumentTypeId", documentTypeId);
        p.addValue("Activity", "GenerateCode");
        List<Map<String, Object>> rows = extractList(getAll().execute(p));
        if (rows.isEmpty()) return 0;
        for (Map.Entry<String, Object> e : rows.get(0).entrySet()) {
            if ("DocNo".equalsIgnoreCase(e.getKey()) && e.getValue() instanceof Number) {
                return ((Number) e.getValue()).intValue();
            }
        }
        return 0;
    }

    private static boolean sameInt(Object v, int expected) {
        return v instanceof Number && ((Number) v).intValue() == expected;
    }

    @SuppressWarnings("unchecked")
    private List<Map<String, Object>> extractList(Map<String, Object> out) {
        for (Object val : out.values()) {
            if (val instanceof List) {
                return (List<Map<String, Object>>) val;
            }
        }
        return Collections.emptyList();
    }

    /**
     * The DAL reads the new id with ExecuteScalar - first column of the first row of the
     * first result set, which here is the procedure's closing SELECT @grnSupplierLoadingMasterId.
     * SimpleJdbcCall returns that as "#result-set-1" -> List<Map>, never as a bare Number
     * (the RETURN_VALUE is bypassed for procedures on SQL Server), so the old loop found
     * nothing and every INSERT fell back to masterId 0: the children were written against
     * master 0. Read the scalar the way ExecuteScalar does.
     */
    @SuppressWarnings("unchecked")
    private Integer extractReturnedId(Map<String, Object> out) {
        java.util.List<String> keys = new java.util.ArrayList<>();
        for (Map.Entry<String, Object> e : out.entrySet()) {
            if (e.getValue() instanceof List) keys.add(e.getKey());
        }
        java.util.Collections.sort(keys);
        for (String k : keys) {
            List<Map<String, Object>> rows = (List<Map<String, Object>>) out.get(k);
            if (rows == null || rows.isEmpty()) continue;
            for (Object v : rows.get(0).values()) {
                if (v instanceof Number) return ((Number) v).intValue();
                if (v != null) {
                    try { return Integer.valueOf(v.toString().trim()); } catch (NumberFormatException ignored) { }
                }
                break;
            }
            break;
        }
        for (Object val : out.values()) {
            if (val instanceof Number) return ((Number) val).intValue();
        }
        return null;
    }

    private Date parseDateOrNull(String dateStr) {
        if (dateStr == null || dateStr.trim().isEmpty()) return null;
        try {
            return DATE_FORMAT.parse(dateStr.length() > 10 ? dateStr.substring(0, 10) : dateStr);
        } catch (Exception e) {
            return null;
        }
    }

    private Date parseDate(String dateStr) {
        if (dateStr == null || dateStr.trim().isEmpty()) return new Date();
        try {
            return DATE_FORMAT.parse(dateStr);
        } catch (Exception e) {
            return new Date();
        }
    }
}
