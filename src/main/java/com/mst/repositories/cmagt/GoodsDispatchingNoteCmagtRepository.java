package com.mst.repositories.cmagt;

import com.mst.models.cmagt.dto.GoodsDispatchingNoteCmagtDto;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.namedparam.MapSqlParameterSource;
import org.springframework.jdbc.core.simple.SimpleJdbcCall;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.text.SimpleDateFormat;
import java.util.*;

/**
 * Goods Dispatching Note (DocumentTypeId 1055) - BLL 0487 gdnBuyerDispatchMaster / DAL 0539.
 *
 * Every call here is one the desktop makes, with the desktop's own @Activity strings:
 *   Save      -> [cmagt].[USP_gdnBuyerDispatchMaster_InsertAndUpdate] then, in the SAME
 *                transaction, USP_gdnBuyerDispatchDetail_Insert / ..EmptyBagDetail_Insert /
 *                ..ExpenseDetail_Insert per row (DAL 0539 SetData).
 *   ReadById  -> usp_gdnBuyerDispatchMaster_GetAllMethod 'ReadById' + 'ReadDetailByHeaderId' +
 *                'gdnBuyerDispatchExpensesDetailByHeaderId' + 'gdnBuyerDispatchEmptyBagDetailByHeaderId'
 *                (DAL 0539 GetData).
 *   History   -> 'FormHistory' (BLL 0487 FormHistory :127-298).
 *   Delete    -> 'DeleteById' with @EntryUserId, @Id in a transaction (BLL 0487 DeleteByID :102-124).
 *   DocNo     -> 'GenerateCode' (BLL 0487 GenerateCode :52-100).
 *
 * The previous version called 'ReadBySearch_gdnBuyerDispatchMaster' and
 * 'ReadById_gdnBuyerDispatchMaster' - neither exists in the procedure, so history was always
 * empty and every record read as "Record not found" - and never sent @actionId, so the master
 * procedure took neither its insert nor its update branch and wrote nothing.
 */
@Repository
public class GoodsDispatchingNoteCmagtRepository {

    private static final String SCHEMA = "cmagt";
    private static final String PROC_GET_ALL = "usp_gdnBuyerDispatchMaster_GetAllMethod";

    @Autowired
    private JdbcTemplate jdbcTemplate;

    /**
     * DAL 0539 SetData: one SqlTransaction over the master and all three child collections.
     * @Transactional is that boundary - any procedure error rolls the whole document back.
     * Callers must let the exception propagate out of this method for the rollback to happen.
     */
    @Transactional
    public int save(GoodsDispatchingNoteCmagtDto dto, boolean isInsert) {
        if (dto.getGdnBuyerDispatchDetailList() == null || dto.getGdnBuyerDispatchDetailList().isEmpty()) {
            throw new IllegalArgumentException("Detail list not found");   // SetData :30-33
        }

        Date now = new Date();
        MapSqlParameterSource m = new MapSqlParameterSource();
        m.addValue("gdnBuyerDispatchMasterId", ni(dto.getGdnBuyerDispatchMasterId()));
        m.addValue("docNo", ni(dto.getDocNo()));
        m.addValue("docDate", parseDate(dto.getDocDate()));
        m.addValue("documentTypeId", ni(dto.getDocumentTypeId()));
        m.addValue("commissionAgentId", ni(dto.getCommissionAgentId()));
        m.addValue("BuyerId", ni(dto.getBuyerId()));
        m.addValue("loadingCityId", ni(dto.getLoadingCityId()));
        m.addValue("unloadingCityId", ni(dto.getUnloadingCityId()));
        m.addValue("BuyerRefDocNo", dto.getBuyerRefDocNo());
        m.addValue("deliveryTermId", ni(dto.getDeliveryTermId()));
        m.addValue("isBuyerOtherChargesAllowed", false);             // model bool, never set by the form
        m.addValue("transporterId", ni(dto.getTransporterId()));
        m.addValue("transporterName", dto.getTransporterName());
        m.addValue("biltyFreight", nd(dto.getBiltyFreight()));
        m.addValue("otherAdLesCharges", nd(dto.getOtherAdLesCharges()));
        m.addValue("totalFreight", nd(dto.getTotalFreight()));
        m.addValue("vehicleTypeId", ni(dto.getVehicleTypeId()));
        m.addValue("vehicleNo", dto.getVehicleNo());
        m.addValue("biltyNo", dto.getBiltyNo());
        m.addValue("biltyDate", parseDate(dto.getBiltyDate()));
        m.addValue("biltyQty", nd(dto.getBiltyQty()));
        m.addValue("loadWeight", nd(dto.getLoadWeight()));
        m.addValue("tareWeight", nd(dto.getTareWeight()));
        m.addValue("scaleNetWeight", nd(dto.getScaleNetWeight()));
        m.addValue("remarksHeader", dto.getRemarksHeader());
        m.addValue("approvalRemarks", null);
        m.addValue("attachmentsValues", null);
        m.addValue("customAttachmentsValues", null);
        m.addValue("organizationId", ni(dto.getOrganizationId()));
        m.addValue("companyId", ni(dto.getCompanyId()));
        m.addValue("branchId", ni(dto.getBranchId()));
        m.addValue("projectId", 0);                                   // model int, CLR default
        m.addValue("financialYearId", ni(dto.getFinancialYearId()));
        m.addValue("entryUserId", ni(dto.getEntryUserId()));
        m.addValue("entryDate", now);                                 // :1890
        m.addValue("modifyUserId", ni(dto.getModifyUserId()));
        m.addValue("modifyDate", now);                                // :1892
        m.addValue("isApproved", false);                              // :1895
        m.addValue("approvedUserId", 0);
        m.addValue("approvedDate", now);                              // :1894
        m.addValue("actionId", isInsert ? 1 : 2);                     // BLL Save :19
        m.addValue("revisionNo", 0);
        m.addValue("DeliverToAddressId", ni(dto.getDeliverToAddressId()));
        m.addValue("DeliverToAddress", dto.getDeliverToAddress());
        m.addValue("BillWeight", nd(dto.getBillWeight()));
        m.addValue("FreightRemarks", dto.getFreightRemarks());
        m.addValue("DeliverToPartyId", ni(dto.getDeliverToPartyId()));
        m.addValue("warningRemarks", dto.getWarningRemarks());

        Integer returned = firstScalarOfLastResultSet(call("USP_gdnBuyerDispatchMaster_InsertAndUpdate").execute(m));
        /* SetData :57-58 - num > 0 ? num : obj.gdnBuyerDispatchMasterId */
        int masterId = (returned != null && returned > 0) ? returned : ni(dto.getGdnBuyerDispatchMasterId());
        if (masterId <= 0) {
            throw new IllegalStateException("Goods Dispatching Note was not saved - the procedure returned no id.");
        }

        for (GoodsDispatchingNoteCmagtDto.DetailDto d : dto.getGdnBuyerDispatchDetailList()) {
            MapSqlParameterSource p = new MapSqlParameterSource();
            p.addValue("gdnBuyerDispatchDetailId", ni(d.getGdnBuyerDispatchDetailId()));
            p.addValue("gdnBuyerDispatchMasterId", masterId);
            p.addValue("supplierOfferId", ni(d.getSupplierOfferId()));
            p.addValue("supplierOfferDetailId", ni(d.getSupplierOfferDetailId()));
            p.addValue("purchaseOrderMasterId", ni(d.getPurchaseOrderMasterId()));
            p.addValue("purchaseOrderDetailId", ni(d.getPurchaseOrderDetailId()));
            p.addValue("inquiryBookingMasterId", ni(d.getInquiryBookingMasterId()));
            p.addValue("inquiryBookingDetailId", ni(d.getInquiryBookingDetailId()));
            p.addValue("saleOrderMasterId", ni(d.getSaleOrderMasterId()));
            p.addValue("saleOrderDetailId", ni(d.getSaleOrderDetailId()));
            p.addValue("grnSupplierLoadingMasterId", ni(d.getGrnSupplierLoadingMasterId()));
            p.addValue("grnSupplierLoadingDetailId", ni(d.getGrnSupplierLoadingDetailId()));
            p.addValue("supplierId", ni(d.getSupplierId()));
            p.addValue("DeliverToAddressId", ni(d.getDeliverToAddressId()));
            p.addValue("DeliverToAddress", d.getDeliverToAddress());
            p.addValue("loadingCityId", ni(d.getLoadingCityId()));
            p.addValue("unloadingCityId", ni(d.getUnloadingCityId()));
            p.addValue("inventoryParentCategoryId", ni(d.getInventoryParentCategoryId()));
            p.addValue("itemId", ni(d.getItemId()));
            p.addValue("packUomId", ni(d.getPackUomId()));
            p.addValue("cropYearId", ni(d.getCropYearId()));
            p.addValue("cropYear", d.getCropYear());
            p.addValue("packingTypeId", ni(d.getPackingTypeId()));
            p.addValue("loadingQty", nd(d.getLoadingQty()));
            p.addValue("wbGrossWeight", nd(d.getWbGrossWeight()));
            p.addValue("ebwPerUnit", nd(d.getEbwPerUnit()));
            p.addValue("ebwTotal", nd(d.getEbwTotal()));
            p.addValue("addLessWeight", nd(d.getAddLessWeight()));
            p.addValue("netBillWeight", nd(d.getNetBillWeight()));
            p.addValue("sortNo", ni(d.getSortNo()));
            p.addValue("remarks", d.getRemarks());
            p.addValue("entryDate", now);
            p.addValue("entryUserId", ni(dto.getEntryUserId()));
            p.addValue("modifyDate", now);
            p.addValue("modifyUserId", ni(dto.getModifyUserId()));
            p.addValue("isApproved", false);
            p.addValue("approvedDate", null);
            p.addValue("approvedUserId", 0);
            p.addValue("actionTypeId", ni(d.getActionTypeId()));
            p.addValue("revisionNo", 0);
            p.addValue("EBWeightDeductionTermId", ni(d.getEbWeightDeductionTermId()));
            p.addValue("purchaseOrderSaleOrderMappingId", ni(d.getPurchaseOrderSaleOrderMappingId()));
            p.addValue("warningRemarks", d.getWarningRemarks());
            call("USP_gdnBuyerDispatchDetail_Insert").execute(p);
        }

        if (dto.getGdnBuyerDispatchEmptyBagDetailList() != null) {
            for (GoodsDispatchingNoteCmagtDto.EmptyBagDto e : dto.getGdnBuyerDispatchEmptyBagDetailList()) {
                MapSqlParameterSource p = new MapSqlParameterSource();
                p.addValue("gdnBuyerDispatchEmptyBagDetailId", ni(e.getGdnBuyerDispatchEmptyBagDetailId()));
                p.addValue("gdnBuyerDispatchMasterId", masterId);
                p.addValue("saleOrderEmptyBagDetailId", ni(e.getSaleOrderEmptyBagDetailId()));
                p.addValue("saleOrderMasterId", ni(e.getSaleOrderMasterId()));
                p.addValue("PackingTypeId", ni(e.getPackingTypeId()));
                p.addValue("weightCutKg", nd(e.getWeightCutKg()));
                p.addValue("Rate", nd(e.getRate()));
                p.addValue("sortNo", ni(e.getSortNo()));
                p.addValue("remarks", e.getRemarks());
                p.addValue("emptyBagPackingMaterialItemId", ni(e.getEmptyBagPackingMaterialItemId()));
                call("USP_gdnBuyerDispatchEmptyBagDetail_Insert").execute(p);
            }
        }

        if (dto.getGdnBuyerDispatchExpenseDetailList() != null) {
            for (GoodsDispatchingNoteCmagtDto.ExpenseDto x : dto.getGdnBuyerDispatchExpenseDetailList()) {
                MapSqlParameterSource p = new MapSqlParameterSource();
                p.addValue("gdnBuyerDispatchExpenseDetailId", ni(x.getGdnBuyerDispatchExpenseDetailId()));
                p.addValue("gdnBuyerDispatchMasterId", masterId);
                p.addValue("saleOrderExpenseDetailId", ni(x.getSaleOrderExpenseDetailId()));
                p.addValue("saleOrderMasterId", ni(x.getSaleOrderMasterId()));
                p.addValue("ItemId", ni(x.getItemId()));
                p.addValue("Qty", nd(x.getQty()));
                p.addValue("rate", x.getRate() == null ? 0d : x.getRate());
                p.addValue("amount", x.getAmount() == null ? 0d : x.getAmount());
                p.addValue("sortNo", ni(x.getSortNo()));
                p.addValue("remarks", x.getRemarks());
                call("USP_gdnBuyerDispatchExpenseDetail_Insert").execute(p);
            }
        }
        return masterId;
    }

    /** BLL 0487 FormHistory :127-298 - optional parameters are omitted when unset, as there. */
    public List<Map<String, Object>> formHistory(int organizationId, int companyId, int branchId,
                                                 int financialYearId, boolean canViewAllRecord,
                                                 int entryUserId, String fromDate, String toDate) {
        MapSqlParameterSource p = new MapSqlParameterSource();
        p.addValue("OrganizationId", organizationId);
        p.addValue("CompanyId", companyId);
        p.addValue("BranchesId", branchId);
        p.addValue("FinancialYearId", financialYearId);
        p.addValue("CanViewAllRecord", canViewAllRecord ? 1 : 0);
        if (!canViewAllRecord) p.addValue("EntryUserId", entryUserId);
        if (notBlank(fromDate)) p.addValue("FromDate", parseDate(fromDate));
        if (notBlank(toDate))   p.addValue("ToDate", parseDate(toDate));
        p.addValue("Activity", "FormHistory");
        return datesToText(firstList(call(PROC_GET_ALL).execute(p)));
    }

    /** DAL 0539 GetData: header by 'ReadById', then the three child reads by header id. */
    public Map<String, Object> readById(int id) {
        List<Map<String, Object>> headers = readActivity("ReadById", id);
        if (headers.isEmpty()) return null;
        Map<String, Object> header = new LinkedHashMap<>(headers.get(0));
        header.put("gdnBuyerDispatchDetailList", readActivity("ReadDetailByHeaderId", id));
        header.put("gdnBuyerDispatchExpenseDetailList", readActivity("gdnBuyerDispatchExpensesDetailByHeaderId", id));
        header.put("gdnBuyerDispatchEmptyBagDetailList", readActivity("gdnBuyerDispatchEmptyBagDetailByHeaderId", id));
        return datesToText(Collections.singletonList(header)).get(0);
    }

    /** BLL 0487 DeleteByID :102-124 - @EntryUserId, @Id, @Activity='DeleteById' in a transaction. */
    @Transactional
    public void deleteById(int entryUserId, int id) {
        MapSqlParameterSource p = new MapSqlParameterSource();
        p.addValue("EntryUserId", entryUserId);
        p.addValue("Id", id);
        p.addValue("Activity", "DeleteById");
        call(PROC_GET_ALL).execute(p);
    }

    /** BLL 0487 GenerateCode :52-100. */
    public int generateCode(int organizationId, int companyId, int branchId, int financialYearId, int documentTypeId) {
        MapSqlParameterSource p = new MapSqlParameterSource();
        p.addValue("OrganizationId", organizationId);
        p.addValue("CompanyId", companyId);
        p.addValue("BranchesId", branchId);
        p.addValue("FinancialYearId", financialYearId);
        p.addValue("DocumentTypeId", documentTypeId);
        p.addValue("Activity", "GenerateCode");
        Integer v = firstScalarOfLastResultSet(call(PROC_GET_ALL).execute(p));
        return v == null ? 0 : v;
    }

    private List<Map<String, Object>> readActivity(String activity, int id) {
        MapSqlParameterSource p = new MapSqlParameterSource();
        p.addValue("Id", id);
        p.addValue("Activity", activity);
        return firstList(call(PROC_GET_ALL).execute(p));
    }

    private SimpleJdbcCall call(String proc) {
        return new SimpleJdbcCall(jdbcTemplate).withSchemaName(SCHEMA).withProcedureName(proc);
    }

    /** Result sets come back as #result-set-1..N; the id is the procedure's final SELECT. */
    @SuppressWarnings("unchecked")
    private static Integer firstScalarOfLastResultSet(Map<String, Object> out) {
        List<String> keys = new ArrayList<>();
        for (Map.Entry<String, Object> e : out.entrySet()) {
            if (e.getValue() instanceof List) keys.add(e.getKey());
        }
        keys.sort(Comparator.comparingInt(GoodsDispatchingNoteCmagtRepository::resultSetOrdinal));
        for (int i = keys.size() - 1; i >= 0; i--) {
            List<Map<String, Object>> rows = (List<Map<String, Object>>) out.get(keys.get(i));
            if (rows != null && !rows.isEmpty() && !rows.get(0).isEmpty()) {
                Object v = rows.get(0).values().iterator().next();
                if (v instanceof Number) return ((Number) v).intValue();
                if (v != null) {
                    try { return Integer.parseInt(v.toString().trim()); } catch (NumberFormatException ignored) { }
                }
            }
        }
        return null;
    }

    private static int resultSetOrdinal(String key) {
        int i = key.lastIndexOf('-');
        try { return Integer.parseInt(key.substring(i + 1)); } catch (Exception e) { return Integer.MAX_VALUE; }
    }

    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> firstList(Map<String, Object> out) {
        List<String> keys = new ArrayList<>();
        for (Map.Entry<String, Object> e : out.entrySet()) {
            if (e.getValue() instanceof List) keys.add(e.getKey());
        }
        if (keys.isEmpty()) return new ArrayList<>();
        keys.sort(Comparator.comparingInt(GoodsDispatchingNoteCmagtRepository::resultSetOrdinal));
        List<Map<String, Object>> rows = (List<Map<String, Object>>) out.get(keys.get(0));
        return rows == null ? new ArrayList<>() : rows;
    }

    /** Dates are sent as local "yyyy-MM-dd'T'HH:mm:ss" text so the page's substring(0,10)
        shows the stored day rather than a UTC-shifted one. */
    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> datesToText(List<Map<String, Object>> rows) {
        SimpleDateFormat f = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> c = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : r.entrySet()) {
                Object v = e.getValue();
                if (v instanceof Date) v = f.format((Date) v);
                else if (v instanceof List) {
                    List<Map<String, Object>> inner = new ArrayList<>();
                    for (Object o : (List<Object>) v) if (o instanceof Map) inner.add((Map<String, Object>) o);
                    v = datesToText(inner);
                }
                c.put(e.getKey(), v);
            }
            out.add(c);
        }
        return out;
    }

    private static Date parseDate(String s) {
        if (!notBlank(s)) return new Date();
        try {
            return new SimpleDateFormat("yyyy-MM-dd").parse(s.trim().length() > 10 ? s.trim().substring(0, 10) : s.trim());
        } catch (Exception e) {
            throw new IllegalArgumentException("Invalid date: " + s);
        }
    }

    private static boolean notBlank(String v) { return v != null && !v.trim().isEmpty(); }
    private static int ni(Integer v) { return v == null ? 0 : v; }
    private static BigDecimal nd(BigDecimal v) { return v == null ? BigDecimal.ZERO : v; }
}
