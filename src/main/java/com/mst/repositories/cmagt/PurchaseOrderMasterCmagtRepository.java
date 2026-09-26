package com.mst.repositories.cmagt;

import com.mst.models.cmagt.dto.PurchaseOrderMasterCmagtDto;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.namedparam.MapSqlParameterSource;
import org.springframework.jdbc.core.simple.SimpleJdbcCall;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * DAL Architecture.DAL.CommissionAgent.purchaseOrderMaster.SetData.
 *
 * Serves Supplier Offer (DocumentTypeId 1051) and Purchase Order Cmagt (1052) — the desktop
 * runs both through this one model, BLL, DAL and procedure set, so the data contract is shared
 * and only the document type differs. Each service supplies its own.
 *
 * Every parameter of every procedure is passed. SimpleJdbcCall applies the procedure's own
 * default for anything omitted, which is how under-filled writes report success while storing
 * nothing.
 */
@Repository
public class PurchaseOrderMasterCmagtRepository {

    @Autowired
    private JdbcTemplate jdbcTemplate;

    private static final SimpleDateFormat DATE_FORMAT = new SimpleDateFormat("yyyy-MM-dd");

    /**
     * One transaction over the master and all seven child collections, as SetData does.
     * SetData's own first guard is reproduced: it throws "Detail list not found" when the
     * detail list is empty, so an empty save is refused rather than writing a headless master.
     */
    @Transactional
    public Map<String, Object> saveOrUpdate(PurchaseOrderMasterCmagtDto dto) {
        if (dto.getPurchaseOrderDetailList() == null || dto.getPurchaseOrderDetailList().isEmpty()) {
            throw new IllegalArgumentException("Detail list not found");
        }

        MapSqlParameterSource m = new MapSqlParameterSource();
        m.addValue("isApproved", nb(dto.isApproved()));
        m.addValue("isSupplierOtherChargesAllowed", nb(dto.isSupplierOtherChargesAllowed()));
        m.addValue("isWhtApplied", nb(dto.isWhtApplied()));
        m.addValue("approvedDate", parseDate(dto.getApprovedDate()));
        m.addValue("deliveryStartDate", parseDate(dto.getDeliveryStartDate()));
        m.addValue("docDate", parseDate(dto.getDocDate()));
        m.addValue("entryDate", parseDate(dto.getEntryDate()));
        m.addValue("modifyDate", parseDate(dto.getModifyDate()));
        m.addValue("ValidityDate", parseDate(dto.getValidityDate()));
        m.addValue("fcyAmount", nd(dto.getFcyAmount()));
        m.addValue("fcyFxRate", nd(dto.getFcyFxRate()));
        m.addValue("actionId", ni(dto.getActionId()));
        m.addValue("approvedUserId", ni(dto.getApprovedUserId()));
        m.addValue("branchId", ni(dto.getBranchId()));
        m.addValue("commissionAgentId", ni(dto.getCommissionAgentId()));
        m.addValue("companyId", ni(dto.getCompanyId()));
        m.addValue("deliveryDays", ni(dto.getDeliveryDays()));
        m.addValue("deliveryTermId", ni(dto.getDeliveryTermId()));
        m.addValue("docNo", ni(dto.getDocNo()));
        m.addValue("documentTypeId", ni(dto.getDocumentTypeId()));
        m.addValue("EBWeightDeductionTermId", ni(dto.getEBWeightDeductionTermId()));
        m.addValue("entryUserId", ni(dto.getEntryUserId()));
        m.addValue("fcyId", ni(dto.getFcyId()));
        m.addValue("financialYearId", ni(dto.getFinancialYearId()));
        m.addValue("modifyUserId", ni(dto.getModifyUserId()));
        m.addValue("organizationId", ni(dto.getOrganizationId()));
        m.addValue("projectId", ni(dto.getProjectId()));
        m.addValue("purchaseOrderMasterId", ni(dto.getPurchaseOrderMasterId()));
        m.addValue("statusId", ni(dto.getStatusId()));
        m.addValue("supplierId", ni(dto.getSupplierId()));
        m.addValue("approvalRemarks", ns(dto.getApprovalRemarks()));
        m.addValue("attachmentsValues", ns(dto.getAttachmentsValues()));
        m.addValue("customAttachmentsValues", ns(dto.getCustomAttachmentsValues()));
        m.addValue("remarksHeader", ns(dto.getRemarksHeader()));
        m.addValue("paymentScheduleDescription", ns(dto.getPaymentScheduleDescription()));
        m.addValue("purchaseOrderEmptyBagDetailDescription", ns(dto.getPurchaseOrderEmptyBagDetailDescription()));
        m.addValue("purchaseOrderEmptyBagDetailDescriptionII", ns(dto.getPurchaseOrderEmptyBagDetailDescriptionII()));
        m.addValue("purchaseOrderSupplierExpenseDetailDescription", ns(dto.getPurchaseOrderSupplierExpenseDetailDescription()));
        m.addValue("purchaseOrderCommissionDetailDescription", ns(dto.getPurchaseOrderCommissionDetailDescription()));
        m.addValue("ShipToAddress", ns(dto.getShipToAddress()));
        m.addValue("shipToAddressId", ni(dto.getShipToAddressId()));
        m.addValue("DeliveryToPartyId", ni(dto.getDeliveryToPartyId()));

        Map<String, Object> out = new SimpleJdbcCall(jdbcTemplate)
                .withSchemaName("cmagt")
                .withProcedureName("USP_purchaseOrderMaster_InsertAndUpdate")
                .execute(m);

        /* DAL SetData:54-55 - num = Convert.ToInt32(SetProc(...)), i.e. ExecuteScalar: the first
           column of the first row of the FIRST RESULT SET. The procedure has no SET NOCOUNT ON,
           so SimpleJdbcCall's map also carries '#update-count-N' Integers (the INSERT's row
           count = 1, the UPDATE's, the child DELETEs'); the old "first Number in the map" read
           could pick one of those and file every child row under master id 1. Only result
           sets are read now. */
        Integer masterId = firstScalar(out);
        if (masterId == null || masterId <= 0) masterId = ni(dto.getPurchaseOrderMasterId());
        dto.setPurchaseOrderMasterId(masterId);

        if (dto.getPurchaseOrderDetailList() != null) {
            for (PurchaseOrderMasterCmagtDto.DetailDto r : dto.getPurchaseOrderDetailList()) {
                r.setPurchaseOrderMasterId(masterId);
                /* Insert():3020 - a grid row is actionTypeId = detailId <= 0 ? 1 : 2. A row the
                   operator removed arrives as 3 (DeleteDetailRow:1723, lstRemoveRecordDetail) and
                   must stay 3: forcing it to 2 made the procedure UPDATE the row it had been
                   asked to soft-delete, so a deleted line survived every save. */
                if (r.getActionTypeId() == null || r.getActionTypeId() != 3) {
                    r.setActionTypeId((r.getPurchaseOrderDetailId() == null
                            || r.getPurchaseOrderDetailId() <= 0) ? 1 : 2);
                }
                MapSqlParameterSource p = new MapSqlParameterSource();
                p.addValue("isApproved", nb(r.isApproved()));
                p.addValue("fcyAmount", nd(r.getFcyAmount()));
                p.addValue("ItemAmount", nd(r.getItemAmount()));
                p.addValue("itemQty", nd(r.getItemQty()));
                p.addValue("itemRate", nd(r.getItemRate()));
                p.addValue("itemWeight", nd(r.getItemWeight()));
                p.addValue("TaxAmount", nd(r.getTaxAmount()));
                p.addValue("TaxPercent", nd(r.getTaxPercent()));
                p.addValue("TotalAmount", nd(r.getTotalAmount()));
                p.addValue("actionTypeId", ni(r.getActionTypeId()));
                p.addValue("cropYearId", ni(r.getCropYearId()));
                p.addValue("inquiryBookingDetailId", ni(r.getInquiryBookingDetailId()));
                p.addValue("inquiryBookingMasterId", ni(r.getInquiryBookingMasterId()));
                p.addValue("inventoryParentCategoryId", ni(r.getInventoryParentCategoryId()));
                p.addValue("itemId", ni(r.getItemId()));
                p.addValue("packingTypeId", ni(r.getPackingTypeId()));
                p.addValue("packUomId", ni(r.getPackUomId()));
                p.addValue("purchaseOrderDetailId", ni(r.getPurchaseOrderDetailId()));
                p.addValue("purchaseOrderMasterId", ni(r.getPurchaseOrderMasterId()));
                p.addValue("rateUomId", ni(r.getRateUomId()));
                p.addValue("buyerId", ni(r.getBuyerId()));
                p.addValue("saleOrderDetailId", ni(r.getSaleOrderDetailId()));
                p.addValue("saleOrderMasterId", ni(r.getSaleOrderMasterId()));
                p.addValue("sortNo", ni(r.getSortNo()));
                p.addValue("supplierOfferDetailId", ni(r.getSupplierOfferDetailId()));
                p.addValue("supplierOfferId", ni(r.getSupplierOfferId()));
                p.addValue("TaxNameId", ni(r.getTaxNameId()));
                p.addValue("cropYear", ns(r.getCropYear()));
                p.addValue("qualitySpecification", ns(r.getQualitySpecification()));
                p.addValue("remarks", ns(r.getRemarks()));
                Integer newDetailId = firstScalar(new SimpleJdbcCall(jdbcTemplate).withSchemaName("cmagt")
                        .withProcedureName("USP_purchaseOrderDetail_Insert").execute(p));
                /* DAL SetData:59-60 - the insert branch SELECTs the new detail id; it is kept on
                   the row because the two mapping collections below link to it by item. */
                if (newDetailId != null && newDetailId > 0) r.setPurchaseOrderDetailId(newDetailId);
            }
        }
        if (dto.getPurchaseOrderPaymentDetailList() != null) {
            for (PurchaseOrderMasterCmagtDto.PaymentDto r : dto.getPurchaseOrderPaymentDetailList()) {
                r.setPurchaseOrderMasterId(masterId);
                MapSqlParameterSource p = new MapSqlParameterSource();
                p.addValue("DueDate", parseDate(r.getDueDate()));
                p.addValue("dueAmount", nd(r.getDueAmount()));
                p.addValue("pctOfTotal", nd(r.getPctOfTotal()));
                p.addValue("BaseDueDateTypeId", ni(r.getBaseDueDateTypeId()));
                p.addValue("DueDays", ni(r.getDueDays()));
                p.addValue("PaymentTermId", ni(r.getPaymentTermId()));
                p.addValue("purchaseOrderMasterId", ni(r.getPurchaseOrderMasterId()));
                p.addValue("purchaseOrderPaymentDetailId", ni(r.getPurchaseOrderPaymentDetailId()));
                p.addValue("sortNo", ni(r.getSortNo()));
                p.addValue("remarks", ns(r.getRemarks()));
                new SimpleJdbcCall(jdbcTemplate).withSchemaName("cmagt")
                        .withProcedureName("USP_purchaseOrderPaymentDetail_Insert").execute(p);
            }
        }
        if (dto.getPurchaseOrderEmptyBagDetailList() != null) {
            for (PurchaseOrderMasterCmagtDto.EmptyBagDto r : dto.getPurchaseOrderEmptyBagDetailList()) {
                r.setPurchaseOrderMasterId(masterId);
                MapSqlParameterSource p = new MapSqlParameterSource();
                p.addValue("Rate", nd(r.getRate()));
                p.addValue("weightCutKg", nd(r.getWeightCutKg()));
                p.addValue("PackingTypeId", ni(r.getPackingTypeId()));
                p.addValue("purchaseOrderEmptyBagDetailId", ni(r.getPurchaseOrderEmptyBagDetailId()));
                p.addValue("purchaseOrderMasterId", ni(r.getPurchaseOrderMasterId()));
                p.addValue("saleOrderEmptyBagDetailId", ni(r.getSaleOrderEmptyBagDetailId()));
                p.addValue("saleOrderMasterId", ni(r.getSaleOrderMasterId()));
                p.addValue("entryTypeId", ni(r.getEntryTypeId()));
                p.addValue("emptyBagPackingMaterialItemId", ni(r.getEmptyBagPackingMaterialItemId()));
                p.addValue("sortNo", ni(r.getSortNo()));
                p.addValue("remarks", ns(r.getRemarks()));
                new SimpleJdbcCall(jdbcTemplate).withSchemaName("cmagt")
                        .withProcedureName("USP_purchaseOrderEmptyBagDetail_Insert").execute(p);
            }
        }
        if (dto.getPurchaseOrderSupplierExpenseDetailList() != null) {
            for (PurchaseOrderMasterCmagtDto.SupplierExpenseDto r : dto.getPurchaseOrderSupplierExpenseDetailList()) {
                r.setPurchaseOrderMasterId(masterId);
                MapSqlParameterSource p = new MapSqlParameterSource();
                p.addValue("Qty", nd(r.getQty()));
                p.addValue("amount", no(r.getAmount()));
                p.addValue("rate", no(r.getRate()));
                p.addValue("ItemId", ni(r.getItemId()));
                p.addValue("purchaseOrderMasterId", ni(r.getPurchaseOrderMasterId()));
                p.addValue("purchaseOrderSupplierExpenseDetailId", ni(r.getPurchaseOrderSupplierExpenseDetailId()));
                p.addValue("sortNo", ni(r.getSortNo()));
                p.addValue("saleOrderMasterId", ni(r.getSaleOrderMasterId()));
                p.addValue("saleOrderBuyerOtherExpenseDetailId", ni(r.getSaleOrderBuyerOtherExpenseDetailId()));
                p.addValue("remarks", ns(r.getRemarks()));
                new SimpleJdbcCall(jdbcTemplate).withSchemaName("cmagt")
                        .withProcedureName("USP_purchaseOrderSupplierExpenseDetail_Insert").execute(p);
            }
        }
        if (dto.getPurchaseOrderCommissionDetailList() != null) {
            for (PurchaseOrderMasterCmagtDto.CommissionDto r : dto.getPurchaseOrderCommissionDetailList()) {
                r.setPurchaseOrderMasterId(masterId);
                MapSqlParameterSource p = new MapSqlParameterSource();
                p.addValue("commissionAmount", nd(r.getCommissionAmount()));
                p.addValue("commissionRate", nd(r.getCommissionRate()));
                p.addValue("agentTypeId", ni(r.getAgentTypeId()));
                p.addValue("commissionAgentId", ni(r.getCommissionAgentId()));
                p.addValue("commissionTypeId", ni(r.getCommissionTypeId()));
                p.addValue("purchaseOrderCommissionDetailId", ni(r.getPurchaseOrderCommissionDetailId()));
                p.addValue("purchaseOrderMasterId", ni(r.getPurchaseOrderMasterId()));
                p.addValue("rateUomId", ni(r.getRateUomId()));
                p.addValue("sortNo", ni(r.getSortNo()));
                p.addValue("commissionRemarks", ns(r.getCommissionRemarks()));
                new SimpleJdbcCall(jdbcTemplate).withSchemaName("cmagt")
                        .withProcedureName("USP_purchaseOrderCommissionDetail_Insert").execute(p);
            }
        }
        if (dto.getPurchaseOrderSaleOrderMappingList() != null) {
            for (PurchaseOrderMasterCmagtDto.SaleOrderMappingDto r : dto.getPurchaseOrderSaleOrderMappingList()) {
                /* DAL SetData:84-89 - the header id and the detail id are assigned HERE, from the
                   rows just written. They were never set, so a new order's mappings went in with
                   purchaseOrderMasterId 0 and no purchaseOrderDetailId. */
                r.setPurchaseOrderMasterId(masterId);
                Integer linkedDetailId = detailIdForItem(dto, r.getItemId());
                if (linkedDetailId != null) r.setPurchaseOrderDetailId(linkedDetailId);
                MapSqlParameterSource p = new MapSqlParameterSource();
                p.addValue("itemNetWeight", nd(r.getItemNetWeight()));
                p.addValue("itemQty", nd(r.getItemQty()));
                p.addValue("buyerId", ni(r.getBuyerId()));
                p.addValue("itemId", ni(r.getItemId()));
                p.addValue("purchaseOrderDetailId", ni(r.getPurchaseOrderDetailId()));
                p.addValue("purchaseOrderMasterId", ni(r.getPurchaseOrderMasterId()));
                p.addValue("purchaseOrderSaleOrderMappingId", ni(r.getPurchaseOrderSaleOrderMappingId()));
                p.addValue("saleOrderDetailId", ni(r.getSaleOrderDetailId()));
                p.addValue("saleOrderMasterId", ni(r.getSaleOrderMasterId()));
                p.addValue("sortNo", ni(r.getSortNo()));
                p.addValue("supplierId", ni(r.getSupplierId()));
                p.addValue("actionTypeId", ni(r.getActionTypeId()));
                p.addValue("Remarks", ns(r.getRemarks()));
                new SimpleJdbcCall(jdbcTemplate).withSchemaName("cmagt")
                        .withProcedureName("USP_purchaseOrderSaleOrderMapping_Insert").execute(p);
            }
        }
        if (dto.getSupplierOfferBuyerInquiryMappingDetailList() != null) {
            for (PurchaseOrderMasterCmagtDto.OfferInquiryMappingDto r : dto.getSupplierOfferBuyerInquiryMappingDetailList()) {
                /* DAL SetData:97-102 - same assignment as the sale-order mapping above. */
                r.setPurchaseOrderMasterId(masterId);
                Integer linkedDetailId = detailIdForItem(dto, r.getItemId());
                if (linkedDetailId != null) r.setPurchaseOrderDetailId(linkedDetailId);
                MapSqlParameterSource p = new MapSqlParameterSource();
                p.addValue("itemNetWeight", nd(r.getItemNetWeight()));
                p.addValue("itemQty", nd(r.getItemQty()));
                p.addValue("actionTypeId", ni(r.getActionTypeId()));
                p.addValue("buyerId", ni(r.getBuyerId()));
                p.addValue("inquiryBookingDetailId", ni(r.getInquiryBookingDetailId()));
                p.addValue("inquiryBookingMasterId", ni(r.getInquiryBookingMasterId()));
                p.addValue("itemId", ni(r.getItemId()));
                p.addValue("purchaseOrderDetailId", ni(r.getPurchaseOrderDetailId()));
                p.addValue("purchaseOrderMasterId", ni(r.getPurchaseOrderMasterId()));
                p.addValue("sortNo", ni(r.getSortNo()));
                p.addValue("supplierId", ni(r.getSupplierId()));
                p.addValue("SupplierOfferBuyerInquiryMappingDetailId", ni(r.getSupplierOfferBuyerInquiryMappingDetailId()));
                p.addValue("supplierOfferDetailId", ni(r.getSupplierOfferDetailId()));
                p.addValue("supplierOfferMasterId", ni(r.getSupplierOfferMasterId()));
                p.addValue("Remarks", ns(r.getRemarks()));
                new SimpleJdbcCall(jdbcTemplate).withSchemaName("cmagt")
                        .withProcedureName("USP_SupplierOfferBuyerInquiryMappingDetail_Insert").execute(p);
            }
        }

        Map<String, Object> result = new HashMap<>();
        result.put("status", "SUCCESS");
        result.put("id", masterId);
        result.put("purchaseOrderMasterId", masterId);
        return result;
    }

    /* ==========================================================================================
     * READ SIDE — DAL purchaseOrderMaster.GetData, all through
     * [cmagt].[USP_purchaseOrderMaster_GetAllMethod] with an @Activity per collection.
     * The six child activities below are the DAL's own strings, including the odd
     * "...purchaseOrderCommissionDetailId" spelling, which is the activity name, not a typo to
     * correct. The seventh collection uses a DIFFERENT procedure and also passes
     * @DocumentTypeId, so it is not folded in with the others.
     * ========================================================================================= */

    private static final String P_GET = "USP_purchaseOrderMaster_GetAllMethod";

    private List<Map<String, Object>> get(MapSqlParameterSource p) {
        Map<String, Object> out = new SimpleJdbcCall(jdbcTemplate)
                .withSchemaName("cmagt").withProcedureName(P_GET).execute(p);
        return firstList(out);
    }

    /** BLL ReadById — @Id, @Activity='ReadById'. */
    public List<Map<String, Object>> readById(int id) {
        MapSqlParameterSource p = new MapSqlParameterSource();
        p.addValue("Id", id);
        p.addValue("Activity", "ReadById");
        return get(p);
    }

    private List<Map<String, Object>> child(int headerId, String activity) {
        MapSqlParameterSource p = new MapSqlParameterSource();
        p.addValue("Id", headerId);
        p.addValue("Activity", activity);
        return get(p);
    }

    public List<Map<String, Object>> detailByHeaderId(int id)            { return child(id, "ReadByHeaderId_purchaseOrderDetail"); }
    public List<Map<String, Object>> supplierExpenseByHeaderId(int id)   { return child(id, "ReadByHeaderId_purchaseOrderSupplierExpenseDetail"); }
    public List<Map<String, Object>> emptyBagByHeaderId(int id)          { return child(id, "ReadByHeaderId_purchaseOrderEmptyBagDetail"); }
    public List<Map<String, Object>> commissionByHeaderId(int id)        { return child(id, "ReadByHeaderId_purchaseOrderCommissionDetailId"); }
    public List<Map<String, Object>> paymentByHeaderId(int id)           { return child(id, "ReadByHeaderId_purchaseOrderPaymentDetail"); }
    public List<Map<String, Object>> saleOrderMappingByHeaderId(int id)  { return child(id, "ReadByHeaderId_purchaseOrderSaleOrderMapping"); }

    /** A different procedure, and it also takes @DocumentTypeId (DAL p8). */
    public List<Map<String, Object>> offerInquiryMappingByHeaderId(int id, int documentTypeId) {
        MapSqlParameterSource p = new MapSqlParameterSource();
        p.addValue("Id", id);
        p.addValue("DocumentTypeId", documentTypeId);
        return firstList(new SimpleJdbcCall(jdbcTemplate).withSchemaName("cmagt")
                .withProcedureName("USP_SupplierOfferBuyerInquiryMappingDetail_ReadById").execute(p));
    }

    /** BLL GenerateCode — five parameters, all unconditional. */
    public int generateCode(int orgId, int companyId, int branchId, int financialYearId, int documentTypeId) {
        MapSqlParameterSource p = new MapSqlParameterSource();
        p.addValue("OrganizationId", orgId);
        p.addValue("CompanyId", companyId);
        p.addValue("BranchesId", branchId);
        p.addValue("FinancialYearId", financialYearId);
        p.addValue("DocumentTypeId", documentTypeId);
        p.addValue("Activity", "GenerateCode");
        List<Map<String, Object>> rows = get(p);
        if (rows.isEmpty()) return 0;
        for (Object v : rows.get(0).values()) {
            if (v instanceof Number) return ((Number) v).intValue();
        }
        return 0;
    }

    /**
     * BLL FormHistory. Six parameters are always sent — including @CanViewAllRecord, which is
     * the permission filter deciding whether the operator sees other users' documents — and
     * @EntryUserId, @FromDate and @ToDate are guarded, so they are omitted when unset rather
     * than sent as NULL.
     */
    public List<Map<String, Object>> formHistory(int orgId, int companyId, int branchId,
                                                 int financialYearId, int documentTypeId,
                                                 boolean canViewAllRecord, Integer entryUserId,
                                                 String fromDate, String toDate) {
        return formHistory(orgId, companyId, branchId, financialYearId, documentTypeId,
                canViewAllRecord, entryUserId, fromDate, toDate, null);
    }

    /**
     * The same call with the BLL's optional filters. Keys are the procedure's own parameter
     * names (EntryFromDate, EntryToDate, ModifyFromDate, ModifyToDate, ValidityDateFrom,
     * ValidityDateTo, FromDocNo, ToDocNo, Id, CommissionAgentId, SupplierId, ItemId,
     * ParentItemIds, DeliveryToPartyId, ShipToAddress). As in BLL FormHistory, an int is sent
     * only when non-zero, a string only when non-empty, a date only when it parses.
     */
    public List<Map<String, Object>> formHistory(int orgId, int companyId, int branchId,
                                                 int financialYearId, int documentTypeId,
                                                 boolean canViewAllRecord, Integer entryUserId,
                                                 String fromDate, String toDate,
                                                 Map<String, Object> filters) {
        MapSqlParameterSource p = new MapSqlParameterSource();
        /* BLL FormHistory adds @Activity='FormHistory' last. It was never sent, so the
           procedure matched none of its IF @Activity branches and the history grid was
           always empty. */
        p.addValue("Activity", "FormHistory");
        if (filters != null) {
            for (Map.Entry<String, Object> e : filters.entrySet()) {
                Object v = e.getValue();
                if (v == null) continue;
                String k = e.getKey();
                if (k.endsWith("Date") || k.startsWith("ValidityDate")) {
                    Date d = parseDate(String.valueOf(v));
                    if (d != null) p.addValue(k, d);
                } else if (v instanceof Number) {
                    if (((Number) v).intValue() != 0) p.addValue(k, ((Number) v).intValue());
                } else if (!String.valueOf(v).trim().isEmpty()) {
                    p.addValue(k, String.valueOf(v));
                }
            }
        }
        p.addValue("OrganizationId", orgId);
        p.addValue("CompanyId", companyId);
        p.addValue("BranchesId", branchId);
        p.addValue("FinancialYearId", financialYearId);
        p.addValue("DocumentTypeId", documentTypeId);
        p.addValue("CanViewAllRecord", canViewAllRecord ? 1 : 0);   // @CanViewAllRecord INT
        if (entryUserId != null && entryUserId != 0) p.addValue("EntryUserId", entryUserId);
        Date f = parseDate(fromDate), t = parseDate(toDate);
        if (f != null) p.addValue("FromDate", f);
        if (t != null) p.addValue("ToDate", t);
        return get(p);
    }

    /** BLL DeleteByID — @EntryUserId, @Id, @Activity='DeleteById'. */
    public void deleteById(int entryUserId, int id) {
        MapSqlParameterSource p = new MapSqlParameterSource();
        p.addValue("EntryUserId", entryUserId);
        p.addValue("Id", id);
        p.addValue("Activity", "DeleteById");
        new SimpleJdbcCall(jdbcTemplate).withSchemaName("cmagt")
                .withProcedureName(P_GET).execute(p);
    }

    @SuppressWarnings("unchecked")
    private List<Map<String, Object>> firstList(Map<String, Object> out) {
        for (Object v : out.values()) if (v instanceof List) return (List<Map<String, Object>>) v;
        return java.util.Collections.emptyList();
    }

    /**
     * ExecuteScalar semantics over a SimpleJdbcCall result map: the first column of the first
     * row of the lowest-numbered '#result-set-N'. Update counts ('#update-count-N', Integers)
     * are ignored - they are row counts, not ids. Null when no result set carried a row.
     */
    @SuppressWarnings("unchecked")
    private static Integer firstScalar(Map<String, Object> out) {
        if (out == null) return null;
        List<Map<String, Object>> first = null;
        int best = Integer.MAX_VALUE;
        for (Map.Entry<String, Object> e : out.entrySet()) {
            if (!(e.getValue() instanceof List)) continue;
            String k = e.getKey();
            int idx = Integer.MAX_VALUE - 1;
            if (k != null && k.startsWith("#result-set-")) {
                try { idx = Integer.parseInt(k.substring("#result-set-".length())); }
                catch (NumberFormatException ignored) { }
            }
            if (idx < best) { best = idx; first = (List<Map<String, Object>>) e.getValue(); }
        }
        if (first == null || first.isEmpty() || first.get(0) == null || first.get(0).isEmpty()) return null;
        Object v = first.get(0).values().iterator().next();
        if (v instanceof Number) return ((Number) v).intValue();
        if (v == null) return null;
        try { return Integer.valueOf(String.valueOf(v).trim()); }
        catch (NumberFormatException e) { return null; }
    }

    /** DAL SetData:85/98 - purchaseOrderDetailList.FirstOrDefault(r => r.itemId == itemId). */
    private static Integer detailIdForItem(PurchaseOrderMasterCmagtDto dto, Integer itemId) {
        if (dto.getPurchaseOrderDetailList() == null) return null;
        int want = ni(itemId);
        for (PurchaseOrderMasterCmagtDto.DetailDto d : dto.getPurchaseOrderDetailList()) {
            if (ni(d.getItemId()) == want) return ni(d.getPurchaseOrderDetailId());
        }
        return null;
    }

    /* Null-safe coercions. A null never becomes 1 or any other invented id. */
    private static int ni(Integer v) { return v == null ? 0 : v; }
    private static boolean nb(Boolean v) { return v != null && v; }
    private static String ns(String v) { return v == null ? "" : v; }
    private static BigDecimal nd(BigDecimal v) { return v == null ? BigDecimal.ZERO : v; }
    private static double no(Double v) { return v == null ? 0d : v; }

    private Date parseDate(String s) {
        if (s == null || s.trim().isEmpty()) return null;
        try { return DATE_FORMAT.parse(s.length() > 10 ? s.substring(0, 10) : s); }
        catch (Exception e) { return null; }
    }
}
