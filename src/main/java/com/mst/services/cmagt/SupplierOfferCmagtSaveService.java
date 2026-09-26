package com.mst.services.cmagt;

import com.mst.models.cmagt.dto.PurchaseOrderMasterCmagtDto;
import com.mst.repositories.cmagt.PurchaseOrderMasterCmagtRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.util.HashMap;
import java.util.Map;

/**
 * Supplier Offer (CMAGT) write path — frmSupplierOfferCmagt.
 *
 * DocumentTypeId is 1051, read from the form itself (:546), NOT inferred from the screen name.
 * Purchase Order Cmagt is 1052 and runs through the very same model, BLL, DAL and procedures,
 * which is why the repository is shared and only this value differs.
 *
 * Replaces a write path that targeted [cmagt].[USP_SupplierOfferMaster_InsertAndUpdate] — a
 * procedure family belonging to a BLL/DAL/model that no desktop form constructs.
 */
@Service
public class SupplierOfferCmagtSaveService {

    /** frmSupplierOfferCmagt:546. */
    public static final int DOCUMENT_TYPE_ID = 1051;

    @Autowired private PurchaseOrderMasterCmagtRepository repository;
    @Autowired private PurchaseOrderMasterCmagtValidator validator;
    @Autowired private CurrentUserContext currentUserContext;

    /** frmSupplierOfferCmagt — the screen name the grant grid is keyed on. */
    public static final String DESKTOP_SCREEN_NAME = "frmSupplierOfferCmagt";
    private static final String RIGHT_CAN_VIEW_ALL_RECORDS = "CanView AllRecord";

    @Autowired private com.mst.repositories.cmagt.SaleOrderCmagtRepository rightsRepo;

    /**
     * BLL FormHistory sends @CanViewAllRecord, and the desktop then sets EntryUser to the
     * current user when the right is absent (InvoiceHistoryService documents the same pattern):
     *   obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
     *   if (!obj.CanViewAllRecord) obj.EntryUser = UserAccount.ID;
     *
     * Read exactly as SaleOrderCmagtService.canViewAllRecords does, against this screen's own
     * name. An unreadable grant grid must not become an implicit grant.
     */
    public boolean canViewAllRecords() {
        String role = currentUserContext.currentRoleName();
        if ("Admin".equalsIgnoreCase(role) || "Administrator".equalsIgnoreCase(role)) return true;
        try {
            for (Map<String, Object> r : rightsRepo.userRightsForScreen(
                    currentUserContext.currentUserId(), DESKTOP_SCREEN_NAME, role,
                    currentUserContext.currentCompanyId())) {
                Object name = r.get("RightName");
                if (name == null) {
                    for (Map.Entry<String, Object> e : r.entrySet())
                        if (e.getKey().equalsIgnoreCase("RightName")) { name = e.getValue(); break; }
                }
                if (name != null && RIGHT_CAN_VIEW_ALL_RECORDS.equalsIgnoreCase(name.toString().trim())) {
                    Object v = r.get("Value");
                    if (v == null) {
                        for (Map.Entry<String, Object> e : r.entrySet())
                            if (e.getKey().equalsIgnoreCase("Value")) { v = e.getValue(); break; }
                    }
                    if (v instanceof Boolean) return (Boolean) v;
                    if (v instanceof Number)  return ((Number) v).intValue() != 0;
                    return v != null && ("1".equals(v.toString().trim())
                            || "true".equalsIgnoreCase(v.toString().trim()));
                }
            }
        } catch (Exception ignored) { }
        return false;
    }

    /** ReadById plus every child collection the DAL loads, in the DAL's own order. */
    public Map<String, Object> getById(int id) {
        Map<String, Object> out = new java.util.LinkedHashMap<>();
        java.util.List<Map<String, Object>> head = repository.readById(id);
        if (head.isEmpty()) {
            out.put("status", "ERROR");
            out.put("message", "Record not found");
            return out;
        }
        Map<String, Object> h = new java.util.LinkedHashMap<>(head.get(0));
        h.put("purchaseOrderDetailList",                repository.detailByHeaderId(id));
        h.put("purchaseOrderSupplierExpenseDetailList", repository.supplierExpenseByHeaderId(id));
        h.put("purchaseOrderEmptyBagDetailList",        repository.emptyBagByHeaderId(id));
        h.put("purchaseOrderCommissionDetailList",      repository.commissionByHeaderId(id));
        h.put("purchaseOrderPaymentDetailList",         repository.paymentByHeaderId(id));
        h.put("purchaseOrderSaleOrderMappingList",      repository.saleOrderMappingByHeaderId(id));
        h.put("supplierOfferBuyerInquiryMappingDetailList",
                repository.offerInquiryMappingByHeaderId(id, DOCUMENT_TYPE_ID));
        out.put("status", "SUCCESS");
        out.put("data", h);
        return out;
    }

    /** DocumentNoFill — GenerateCode, five unconditional parameters. */
    public int generateNextDocNo() {
        return repository.generateCode(
                currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(),
                currentUserContext.currentBranchId(),
                currentUserContext.currentFinancialYearId(),
                DOCUMENT_TYPE_ID);
    }

    public java.util.List<Map<String, Object>> formHistory(String fromDate, String toDate) {
        boolean all = canViewAllRecords();
        return repository.formHistory(
                currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(),
                currentUserContext.currentBranchId(),
                currentUserContext.currentFinancialYearId(),
                DOCUMENT_TYPE_ID,
                all,
                all ? null : currentUserContext.currentUserId(),
                fromDate, toDate);
    }

    /** DeleteByID — the desktop's own delete, not a JPA deleteById. */
    public Map<String, Object> deleteById(int id) {
        repository.deleteById(currentUserContext.currentUserId(), id);
        Map<String, Object> out = new java.util.LinkedHashMap<>();
        out.put("status", "SUCCESS");
        return out;
    }

    @Transactional
    public Map<String, Object> saveOrUpdate(PurchaseOrderMasterCmagtDto dto) {

        /* Tenancy and authorship are server authority — never the request body, never a
           fallback of 1. The desktop reads UserAccount and clsGlobalVariables.ActiveYr. */
        dto.setOrganizationId(currentUserContext.currentOrganizationId());
        dto.setCompanyId(currentUserContext.currentCompanyId());
        dto.setBranchId(currentUserContext.currentBranchId());
        dto.setFinancialYearId(currentUserContext.currentFinancialYearId());
        dto.setEntryUserId(currentUserContext.currentUserId());
        dto.setModifyUserId(currentUserContext.currentUserId());
        dto.setDocumentTypeId(DOCUMENT_TYPE_ID);

        /* BLL purchaseOrderMaster.Save: actionId = purchaseOrderMasterId == 0 ? 1 : 2.
           Derived from the record's own state, not from anything the browser sends. */
        Integer id = dto.getPurchaseOrderMasterId();
        dto.setActionId((id == null || id == 0) ? 1 : 2);

        String today = java.time.LocalDate.now().toString();
        /* Insert():3043/3045/3047 - entryDate, modifyDate and approvedDate are all DateTime.Now
           on EVERY save (insert and update alike), whatever the page sent; isApproved is always
           false (:3048) and statusId is always 1 (:3029). Server authority, not the request. */
        dto.setEntryDate(today);
        dto.setModifyDate(today);
        dto.setApprovedDate(today);
        dto.setIsApproved(Boolean.FALSE);
        dto.setStatusId(1);

        /* frmSupplierOfferCmagt formvalidation() + Insert()'s per-row refusals, reproduced
           server-side. validateMappedWeight was only one of them; the other twenty-odd were
           enforced by the page alone, which a crafted POST does not run. */
        validator.validate(dto);

        return repository.saveOrUpdate(dto);
    }

    /**
     * btnSave_Click:3338 — for each offer line, the Buyer-Inquiry mapping rows for that item
     * may not sum to more than the line's own weight:
     *
     *   sumOfWeight = mapping.Where(x =&gt; x.itemId == detail.itemId).Sum(x =&gt; x.itemNetWeight)
     *   if (sumOfWeight &gt; detail.itemWeight) refuse
     *
     * Enforced here rather than only in the page, so a crafted request cannot bypass it.
     */
    @SuppressWarnings("unused")
    private void validateMappedWeight(PurchaseOrderMasterCmagtDto dto) {
        if (dto.getSupplierOfferBuyerInquiryMappingDetailList() == null
                || dto.getSupplierOfferBuyerInquiryMappingDetailList().isEmpty()) return;

        Map<Integer, BigDecimal> mapped = new HashMap<>();
        for (PurchaseOrderMasterCmagtDto.OfferInquiryMappingDto r
                : dto.getSupplierOfferBuyerInquiryMappingDetailList()) {
            Integer itemId = r.getItemId();
            if (itemId == null) continue;
            BigDecimal w = r.getItemNetWeight() == null ? BigDecimal.ZERO : r.getItemNetWeight();
            mapped.merge(itemId, w, BigDecimal::add);
        }
        for (PurchaseOrderMasterCmagtDto.DetailDto det : dto.getPurchaseOrderDetailList()) {
            Integer itemId = det.getItemId();
            if (itemId == null) continue;
            BigDecimal sum = mapped.get(itemId);
            if (sum == null) continue;
            BigDecimal offer = det.getItemWeight() == null ? BigDecimal.ZERO : det.getItemWeight();
            if (sum.compareTo(offer) > 0) {
                throw new IllegalArgumentException("Mapped weight (" + sum
                        + ") cannot be greater than Offer weight (" + offer
                        + ") for Item " + itemId);
            }
        }
    }
}
