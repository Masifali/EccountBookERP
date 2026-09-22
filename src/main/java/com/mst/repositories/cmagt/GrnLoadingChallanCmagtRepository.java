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
        masterParams.addValue("deliveryStartDate", parseDate(dto.getDeliveryStartDate()));
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
        masterParams.addValue("approvalRemarks", ns(dto.getApprovalRemarks()));
        masterParams.addValue("attachmentsValues", ns(dto.getAttachmentsValues()));
        masterParams.addValue("biltyNo", ns(dto.getBiltyNo()));
        masterParams.addValue("customAttachmentsValues", ns(dto.getCustomAttachmentsValues()));
        masterParams.addValue("remarksHeader", ns(dto.getRemarksHeader()));
        masterParams.addValue("supplierRefDocNo", ns(dto.getSupplierRefDocNo()));
        masterParams.addValue("transporterName", ns(dto.getTransporterName()));
        masterParams.addValue("vehicleNo", ns(dto.getVehicleNo()));

        Map<String, Object> masterOut = masterCall.execute(masterParams);
        Integer masterId = extractReturnedId(masterOut);
        if (masterId == null || masterId <= 0) {
            masterId = dto.getGrnSupplierLoadingMasterId();
        }

        /* SetData assigns the returned master id onto every child before inserting it. */
        for (GrnLoadingChallanCmagtDto.DetailDto d : dto.getGrnSupplierLoadingDetailList()) {
            d.setGrnSupplierLoadingMasterId(masterId);
            /* BLL: actionTypeId = detailId <= 0 ? 1 : 2 (btnSave_Click :2680). */
            d.setActionTypeId((d.getGrnSupplierLoadingDetailId() == null
                    || d.getGrnSupplierLoadingDetailId() <= 0) ? 1 : 2);
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

    public List<Map<String, Object>> getHistory(Integer companyId, Integer organizationId, String fromDate, String toDate) {
        SimpleJdbcCall call = new SimpleJdbcCall(jdbcTemplate)
                .withSchemaName("cmagt")
                .withProcedureName("usp_grnSupplierLoadingMaster_GetAllMethod");

        MapSqlParameterSource params = new MapSqlParameterSource();
        params.addValue("Activity", "ReadBySearch_grnSupplierLoadingMaster");
        /* The controller passes the session's own values. The ': 1' fallbacks that
           used to sit here would have quietly widened a read to company 1 if one
           ever arrived null, hiding the fault instead of surfacing it. */
        params.addValue("CompanyId", companyId);
        params.addValue("OrganizationId", organizationId);
        params.addValue("DocumentTypeId", 1054);
        params.addValue("FromDate", parseDate(fromDate));
        params.addValue("ToDate", parseDate(toDate));

        Map<String, Object> out = call.execute(params);
        return extractList(out);
    }

    public Map<String, Object> getById(Integer id) {
        Map<String, Object> result = new HashMap<>();

        SimpleJdbcCall headerCall = new SimpleJdbcCall(jdbcTemplate)
                .withSchemaName("cmagt")
                .withProcedureName("usp_grnSupplierLoadingMaster_GetAllMethod");

        MapSqlParameterSource headerParams = new MapSqlParameterSource();
        headerParams.addValue("Activity", "ReadById_grnSupplierLoadingMaster");
        headerParams.addValue("Id", id);

        Map<String, Object> headerOut = headerCall.execute(headerParams);
        List<Map<String, Object>> headers = extractList(headerOut);

        if (headers != null && !headers.isEmpty()) {
            Map<String, Object> header = new HashMap<>(headers.get(0));

            SimpleJdbcCall detCall = new SimpleJdbcCall(jdbcTemplate)
                    .withSchemaName("cmagt")
                    .withProcedureName("usp_grnSupplierLoadingMaster_GetAllMethod");
            MapSqlParameterSource detParams = new MapSqlParameterSource();
            detParams.addValue("Activity", "ReadDetailByHeaderId");
            detParams.addValue("Id", id);
            header.put("grnSupplierLoadingDetailList", extractList(detCall.execute(detParams)));

            result.put("status", "SUCCESS");
            result.put("data", header);
        } else {
            result.put("status", "ERROR");
            result.put("message", "Record not found");
        }
        return result;
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

    private Integer extractReturnedId(Map<String, Object> out) {
        for (Object val : out.values()) {
            if (val instanceof Integer) return (Integer) val;
            if (val instanceof Number) return ((Number) val).intValue();
        }
        return null;
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
