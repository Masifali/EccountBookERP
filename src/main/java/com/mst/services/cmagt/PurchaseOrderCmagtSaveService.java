package com.mst.services.cmagt;

import com.mst.models.cmagt.dto.PurchaseOrderMasterCmagtDto;
import com.mst.repositories.cmagt.PurchaseOrderMasterCmagtRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.Map;

/**
 * Purchase Order (CMAGT) — frmPurchaseOrderCmagt, DocumentTypeId 1052 (:548).
 *
 * WHY THIS REPLACES THE OLD PATH
 *   The old PurchaseOrderCmagtRepository already called the right procedure,
 *   [cmagt].[USP_purchaseOrderMaster_InsertAndUpdate] — but sent 24 of its 42 parameters and
 *   wrote only the detail rows. The other six child collections the DAL writes had no Java
 *   representation, and SimpleJdbcCall applied the procedure's own defaults for the eighteen
 *   missing master parameters, so the document reported success while storing nothing for them.
 *
 *   frmSupplierOfferCmagt (1051) and frmPurchaseOrderCmagt (1052) run through the SAME model,
 *   BLL, DAL and procedure set. That equivalence is proven at the contract level, so both use
 *   PurchaseOrderMasterCmagtRepository and differ only in the document type supplied here.
 */
@Service
public class PurchaseOrderCmagtSaveService {

    /** frmPurchaseOrderCmagt:548. Read from the form, never inferred from the screen name. */
    public static final int DOCUMENT_TYPE_ID = 1052;

    /** The screen name the grant grid is keyed on. */
    public static final String DESKTOP_SCREEN_NAME = "frmPurchaseOrderCmagt";
    private static final String RIGHT_CAN_VIEW_ALL_RECORDS = "CanView AllRecord";

    @Autowired private PurchaseOrderMasterCmagtRepository repository;
    @Autowired private PurchaseOrderMasterCmagtValidator validator;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private com.mst.repositories.cmagt.SaleOrderCmagtRepository rightsRepo;

    @Transactional
    public Map<String, Object> saveOrUpdate(PurchaseOrderMasterCmagtDto dto) {
        /* Server authority only — never the request body, never a fallback of 1. */
        dto.setOrganizationId(currentUserContext.currentOrganizationId());
        dto.setCompanyId(currentUserContext.currentCompanyId());
        dto.setBranchId(currentUserContext.currentBranchId());
        dto.setFinancialYearId(currentUserContext.currentFinancialYearId());
        dto.setEntryUserId(currentUserContext.currentUserId());
        dto.setModifyUserId(currentUserContext.currentUserId());
        dto.setDocumentTypeId(DOCUMENT_TYPE_ID);

        /* BLL purchaseOrderMaster.Save: actionId = purchaseOrderMasterId == 0 ? 1 : 2. */
        Integer id = dto.getPurchaseOrderMasterId();
        dto.setActionId((id == null || id == 0) ? 1 : 2);

        String today = java.time.LocalDate.now().toString();
        if (dto.getEntryDate() == null || dto.getEntryDate().isEmpty()) dto.setEntryDate(today);
        dto.setModifyDate(today);
        /* frmPurchaseOrderCmagt Insert() - obj.approvedDate = DateTime.Now. The desktop stamps it on EVERY save
           even though isApproved stays false, so the column is never null on a
           desktop-written row. Reproduced rather than 'corrected'. */
        if (dto.getApprovedDate() == null || dto.getApprovedDate().isEmpty())
            dto.setApprovedDate(today);

        /* frmPurchaseOrderCmagt formvalidation() + Insert()'s per-row refusals. This screen had
           NO server-side validation at all: every rule lived in the page, so a crafted POST
           reached the procedure unchecked. The validator is shared with Supplier Offer because
           the two forms' validation bodies were diffed and are identical. */
        /* Insert():3232 - each mapping-grid row is actionTypeId = mappingId <= 0 ? 1 : 2; rows
           the operator removed (DeleteSoMappingRow:2666) keep 3. A loaded row came back with
           whatever actionTypeId the table held (1 for a never-updated row), and re-sending that
           1 made the procedure INSERT a duplicate mapping on every Update. */
        if (dto.getPurchaseOrderSaleOrderMappingList() != null) {
            for (PurchaseOrderMasterCmagtDto.SaleOrderMappingDto m : dto.getPurchaseOrderSaleOrderMappingList()) {
                if (m.getActionTypeId() != null && m.getActionTypeId() == 3) continue;
                Integer mid = m.getPurchaseOrderSaleOrderMappingId();
                m.setActionTypeId((mid == null || mid <= 0) ? 1 : 2);
            }
        }

        validator.validate(dto);

        Map<String, Object> r = repository.saveOrUpdate(dto);
        r.put("success", true);   /* the page tests data.success; the repository never set it */
        return r;
    }

    /** ReadById plus every child collection the DAL loads. */
    public Map<String, Object> getById(int id) {
        Map<String, Object> out = new java.util.LinkedHashMap<>();
        List<Map<String, Object>> head = repository.readById(id);
        /* ReadById filters on @Id alone. A document of another company, or of the sibling
           Supplier Offer (1051, same table), is reported as not found rather than served. */
        if (!head.isEmpty() && !ownDocument(head.get(0))) head = java.util.Collections.emptyList();
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

    private boolean ownDocument(Map<String, Object> row) {
        Object c = pick(row, "companyId"), t = pick(row, "documentTypeId");
        return c instanceof Number && ((Number) c).intValue() == currentUserContext.currentCompanyId()
            && t instanceof Number && ((Number) t).intValue() == DOCUMENT_TYPE_ID;
    }

    public int generateNextDocNo() {
        return repository.generateCode(
                currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(),
                currentUserContext.currentBranchId(),
                currentUserContext.currentFinancialYearId(),
                DOCUMENT_TYPE_ID);
    }

    /**
     * FormHistory sends @CanViewAllRecord; without the right the desktop pins EntryUser to the
     * current user. Read against THIS screen's name. An unreadable grant grid returns false.
     */
    public boolean canViewAllRecords() {
        String role = currentUserContext.currentRoleName();
        if ("Admin".equalsIgnoreCase(role) || "Administrator".equalsIgnoreCase(role)) return true;
        try {
            for (Map<String, Object> r : rightsRepo.userRightsForScreen(
                    currentUserContext.currentUserId(), DESKTOP_SCREEN_NAME, role,
                    currentUserContext.currentCompanyId())) {
                Object name = pick(r, "RightName");
                if (name != null && RIGHT_CAN_VIEW_ALL_RECORDS.equalsIgnoreCase(name.toString().trim())) {
                    Object v = pick(r, "Value");
                    if (v instanceof Boolean) return (Boolean) v;
                    if (v instanceof Number)  return ((Number) v).intValue() != 0;
                    return v != null && ("1".equals(v.toString().trim())
                            || "true".equalsIgnoreCase(v.toString().trim()));
                }
            }
        } catch (Exception ignored) { }
        return false;
    }

    private static Object pick(Map<String, Object> row, String key) {
        if (row.containsKey(key)) return row.get(key);
        for (Map.Entry<String, Object> e : row.entrySet())
            if (e.getKey().equalsIgnoreCase(key)) return e.getValue();
        return null;
    }

    public List<Map<String, Object>> formHistory(String fromDate, String toDate) {
        return formHistory(fromDate, toDate, null);
    }

    /**
     * HistoryFill (frmPurchaseOrderCmagt.cs:3919-3993). The From/To pickers apply to the date
     * chosen by the radio - document date (drdocdate), entry date (rdentrydate) or modify date
     * (rdmodifydate) - and the remaining filters are passed through under the procedure's own
     * parameter names; anything else in {@code extra} is ignored.
     */
    public List<Map<String, Object>> formHistory(String fromDate, String toDate, Map<String, Object> extra) {
        boolean all = canViewAllRecords();
        Map<String, Object> filters = new java.util.LinkedHashMap<>();
        String dateType = extra == null ? null : (String) extra.get("dateType");
        String docFrom = fromDate, docTo = toDate;
        if ("entry".equalsIgnoreCase(dateType)) {
            filters.put("EntryFromDate", fromDate); filters.put("EntryToDate", toDate); docFrom = null; docTo = null;
        } else if ("modify".equalsIgnoreCase(dateType)) {
            filters.put("ModifyFromDate", fromDate); filters.put("ModifyToDate", toDate); docFrom = null; docTo = null;
        }
        if (extra != null) {
            for (String k : new String[] { "ValidityDateFrom", "ValidityDateTo", "CommissionAgentId",
                    "SupplierId", "ItemId", "ParentItemIds", "DeliveryToPartyId", "ShipToAddress" }) {
                if (extra.get(k) != null) filters.put(k, extra.get(k));
            }
        }
        return repository.formHistory(
                currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(),
                currentUserContext.currentBranchId(),
                currentUserContext.currentFinancialYearId(),
                DOCUMENT_TYPE_ID, all,
                all ? null : currentUserContext.currentUserId(),
                docFrom, docTo, filters);
    }

    public Map<String, Object> deleteById(int id) {
        List<Map<String, Object>> head = repository.readById(id);
        if (head.isEmpty() || !ownDocument(head.get(0))) {
            Map<String, Object> out = new java.util.LinkedHashMap<>();
            out.put("status", "ERROR");
            out.put("message", "No record found to Delete");
            return out;
        }
        repository.deleteById(currentUserContext.currentUserId(), id);
        Map<String, Object> out = new java.util.LinkedHashMap<>();
        out.put("status", "SUCCESS");
        return out;
    }
}
