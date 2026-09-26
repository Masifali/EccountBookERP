package com.mst.services.cmagt;

import com.mst.models.cmagt.dto.BuyerInquiryBookingDto;
import com.mst.repositories.cmagt.BuyerInquiryBookingRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.stream.Collectors;

/**
 * Buyer Inquiry Booking (CMAGT) - frmBuyerInquiryBooking.
 *
 * DocumentTypeId is 1050, read from the form itself (frmBuyerInquiryBooking.cs:349), never
 * inferred from the screen name. ScreenName is "frmBuyerInquiryBooking" (:352).
 *
 * ---------------------------------------------------------------------------------------------
 * WHAT THIS REPLACES
 * ---------------------------------------------------------------------------------------------
 * The previous version had four defects, each of which let a save report success while doing
 * less than the desktop does:
 *
 *  1. TWO OF THE FOUR CHILD COLLECTIONS WERE NEVER WRITTEN.
 *     The DAL writes four in one transaction (0544:56-75):
 *         [cmagt].[USP_inquiryBookingDetail_Insert]
 *         [cmagt].[USP_inquiryBookingPaymentSchedule_Insert]          <- never called
 *         [cmagt].[USP_inquiryBookingQualitySpecification_Insert]     <- never called
 *         [cmagt].[USP_InquiryBookingPartyDetail_Insert]
 *     Insert():1596-1604 builds a payment-schedule row for EVERY inquiry, so every web-saved
 *     document was missing it.
 *
 *  2. THE TRANSACTION COULD NOT ROLL BACK.
 *     saveRecord was @Transactional but caught Exception and returned {"success": false}.
 *     Nothing propagated, so Spring committed whatever had already been written - a master row
 *     with some of its children, reported to the operator as a failure.
 *
 *  3. TENANCY CAME FROM THE REQUEST BODY (see the controller).
 *
 *  4. NO SERVER-SIDE VALIDATION. Every refusal in formvalidation() (:1379-1490) and the
 *     quality-specification rules (:1547-1571) lived in the page only.
 */
@Service
public class BuyerInquiryBookingService {

    /** frmBuyerInquiryBooking.cs:349. */
    public static final int DOCUMENT_TYPE_ID = 1050;
    /** frmBuyerInquiryBooking.cs:352 - the name the grant grid is keyed on. */
    public static final String DESKTOP_SCREEN_NAME = "frmBuyerInquiryBooking";

    @Autowired private BuyerInquiryBookingRepository repository;
    @Autowired private CurrentUserContext currentUserContext;
    /** Only for the shared grant-grid read (Sp_tblUserRights_GetAllMethod) - same as Supplier Offer. */
    @Autowired private com.mst.repositories.cmagt.SaleOrderCmagtRepository rightsRepo;

    private static final String RIGHT_CAN_VIEW_ALL_RECORDS = "CanView AllRecord";

    /**
     * HistoryFill() (:2093) obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights, and the
     * BLL sends @EntryUserId only when the right is absent. Resolved per call against this
     * screen's own name, exactly as SaleOrderCmagtService / SupplierOfferCmagtSaveService do.
     * An unreadable grant grid must not become an implicit grant.
     */
    public boolean canViewAllRecords() {
        String role = currentUserContext.currentRoleName();
        if ("Admin".equalsIgnoreCase(role) || "Administrator".equalsIgnoreCase(role)) return true;
        try {
            for (Map<String, Object> r : rightsRepo.userRightsForScreen(
                    currentUserContext.currentUserId(), DESKTOP_SCREEN_NAME, role,
                    currentUserContext.currentCompanyId())) {
                Object name = ci(r, "RightName");
                if (name != null && RIGHT_CAN_VIEW_ALL_RECORDS.equalsIgnoreCase(name.toString().trim())) {
                    Object v = ci(r, "Value");
                    if (v instanceof Boolean) return (Boolean) v;
                    if (v instanceof Number)  return ((Number) v).intValue() != 0;
                    return v != null && ("1".equals(v.toString().trim())
                            || "true".equalsIgnoreCase(v.toString().trim()));
                }
            }
        } catch (Exception ignored) { }
        return false;
    }

    /**
     * ReadById / DeleteById / the UPDATE branch of InsertAndUpdate filter by id only (no org,
     * company or user predicate). Over HTTP the id comes from the caller, so tenancy and - without
     * "CanView AllRecord" - ownership are enforced here, the same scope History gives the user.
     */
    private void assertAccess(Map<String, Object> h) {
        if (toInt(ci(h, "organizationId")) != currentUserContext.currentOrganizationId()
                || toInt(ci(h, "companyId")) != currentUserContext.currentCompanyId()) {
            throw new org.springframework.security.access.AccessDeniedException(
                    "This Buyer Inquiry belongs to another organization or company.");
        }
        if (!canViewAllRecords() && toInt(ci(h, "entryUserId")) != currentUserContext.currentUserId()) {
            throw new org.springframework.security.access.AccessDeniedException(
                    "You do not have permission to open Buyer Inquiries entered by another user.");
        }
    }

    public int generateNextDocNo(int orgId, int companyId, int branchId, int yearId, int docTypeId) {
        return repository.generateCode(orgId, companyId, branchId, yearId, docTypeId);
    }

    /* ------------------------------------------------------------------------------- save */

    /**
     * Insert() (:1492-1720), in the desktop's own order.
     *
     * No try/catch around the writes. A failure must reach Spring so the whole document rolls
     * back; swallowing it is what produced half-written inquiries before.
     */
    @Transactional
    public Map<String, Object> saveRecord(BuyerInquiryBookingDto dto) {

        /* Server authority only - never the request body, never a fallback of 1. */
        dto.setOrganizationId(currentUserContext.currentOrganizationId());
        dto.setCompanyId(currentUserContext.currentCompanyId());
        dto.setBranchId(currentUserContext.currentBranchId());
        dto.setFinancialYearId(currentUserContext.currentFinancialYearId());
        dto.setEntryUserId(currentUserContext.currentUserId());
        dto.setModifyUserId(currentUserContext.currentUserId());
        dto.setDocumentTypeId(DOCUMENT_TYPE_ID);

        /* :1529 obj.inquiryStatusId = 1; :1546 obj.isApproved = false - both fixed by the form,
           never posted. A client must not be able to approve an inquiry by sending a flag. */
        dto.setInquiryStatusId(1);
        dto.setIsApproved(Boolean.FALSE);

        boolean isNew = dto.getInquiryBookingMasterId() == null || dto.getInquiryBookingMasterId() == 0;
        /* BLL Save:20 - actionId = inquiryBookingMasterId == 0 ? 1 : 2, from the record's state. */
        dto.setActionId(isNew ? 1 : 2);
        if (!isNew) {
            /* The UPDATE branch writes WHERE inquiryBookingMasterId = @id only. */
            List<Map<String, Object>> existing = repository.readHeaderById(dto.getInquiryBookingMasterId());
            if (existing == null || existing.isEmpty()) fail("Record not update because Id not found");
            assertAccess(existing.get(0));
        }

        String today = LocalDate.now().toString();
        if (dto.getEntryDate() == null || dto.getEntryDate().isEmpty()) dto.setEntryDate(today);
        dto.setModifyDate(today);
        /* :1545 obj.approvedDate = DateTime.Now - the desktop stamps it even though
           isApproved is false. Reproduced rather than "corrected". */
        if (dto.getApprovedDate() == null || dto.getApprovedDate().isEmpty()) dto.setApprovedDate(today);

        validate(dto);

        /* :1547-1575 - the quality grid contributes three things to the MASTER, and only when
           at least one row has both ends of its range filled in. */
        List<BuyerInquiryBookingDto.BuyerInquiryQualitySpecificationDto> validSpecs = validQualityRows(dto);
        if (!validSpecs.isEmpty()) {
            dto.setQualitySpecifications(
                    validSpecs.stream()
                              .map(x -> "[" + nz(x.getQualityParameter()) + ", "
                                            + plain(x.getRangeFrom()) + "," + plain(x.getRangeTo()) + "]")
                              .collect(Collectors.joining(", ")));
        } else {
            /* the desktop leaves analysisGroupId and QualitySpecifications at their CLR
               defaults when no row qualifies (:1571) - it does NOT clear an id the user chose
               elsewhere, because it never assigned one. */
            dto.setAnalysisGroupId(0);
            dto.setQualitySpecifications("");
        }

        if (isNew && (dto.getInquiryBookingNo() == null || dto.getInquiryBookingNo() == 0)) {
            dto.setInquiryBookingNo(repository.generateCode(
                    dto.getOrganizationId(), dto.getCompanyId(), dto.getBranchId(),
                    dto.getFinancialYearId(), dto.getDocumentTypeId()));
        }

        int masterId = repository.saveMaster(dto);
        if (masterId <= 0) masterId = dto.getInquiryBookingMasterId() == null ? 0 : dto.getInquiryBookingMasterId();
        if (masterId <= 0) {
            throw new IllegalStateException(
                    "[cmagt].[USP_inquiryBookingMaster_InsertAndUpdate] returned no id; "
                  + "nothing was written.");
        }

        /* The DAL's own order: detail, payment schedule, quality specification, party detail
           (0544:56-75). Each row's master id is stamped by the DAL, not by the client. */
        for (BuyerInquiryBookingDto.BuyerInquiryDetailDto d : safe(dto.getInquiryBookingDetailList())) {
            d.setInquiryBookingMasterId(masterId);
            if (d.getActionTypeId() == null || d.getActionTypeId() == 0) {
                d.setActionTypeId((d.getInquiryBookingDetailId() == null || d.getInquiryBookingDetailId() <= 0) ? 1 : 2);
            }
            repository.saveDetailRow(d);
        }

        for (BuyerInquiryBookingDto.BuyerInquiryPaymentScheduleDto p
                : paymentSchedule(dto, masterId)) {
            repository.savePaymentScheduleRow(p);
        }

        for (BuyerInquiryBookingDto.BuyerInquiryQualitySpecificationDto q : validSpecs) {
            q.setInquiryBookingMasterId(masterId);
            repository.saveQualitySpecificationRow(q);
        }

        for (BuyerInquiryBookingDto.BuyerInquiryPartyDetailDto pd : safe(dto.getInquiryBookingPartyDetailList())) {
            /* :1607 - a row counts when it names a sub-party OR carries an amount. */
            boolean counts = (pd.getSubPartyId() != null && pd.getSubPartyId() > 0)
                          || (pd.getAmount() != null && pd.getAmount().compareTo(BigDecimal.ZERO) > 0);
            if (!counts) continue;
            pd.setInquiryBookingMasterId(masterId);
            if (pd.getActionTypeId() == null || pd.getActionTypeId() == 0) {
                pd.setActionTypeId((pd.getInquiryBookingPartyDetailId() == null
                                 || pd.getInquiryBookingPartyDetailId() <= 0) ? 1 : 2);
            }
            repository.savePartyDetailRow(pd);
        }

        Map<String, Object> res = new LinkedHashMap<>();
        res.put("success", true);
        res.put("id", masterId);
        res.put("inquiryBookingNo", dto.getInquiryBookingNo());
        res.put("message", (isNew ? "Save Successfully" : "Update Successfully")
                         + " (Inquiry No: " + dto.getInquiryBookingNo() + ")");
        return res;
    }

    /**
     * :1596-1604 - the desktop builds exactly ONE payment-schedule row per inquiry, from the
     * header payment term:
     *     paymentTermId = CmbPaymentTerm, dueDays = txtDueDays,
     *     dueBaseDate   = inquiryBookingDate + dueDays,
     *     pctOfTotal    = 100,
     *     dueAmount     = the single detail row's buyerAmount.
     *
     * A schedule posted by the page is honoured as sent (so a future multi-row schedule works);
     * when none is posted, this reproduces the desktop's row rather than writing nothing.
     */
    private List<BuyerInquiryBookingDto.BuyerInquiryPaymentScheduleDto> paymentSchedule(
            BuyerInquiryBookingDto dto, int masterId) {

        List<BuyerInquiryBookingDto.BuyerInquiryPaymentScheduleDto> posted =
                safe(dto.getInquiryBookingPaymentScheduleList());
        if (!posted.isEmpty()) {
            for (BuyerInquiryBookingDto.BuyerInquiryPaymentScheduleDto p : posted) {
                p.setInquiryBookingMasterId(masterId);
            }
            return posted;
        }

        BuyerInquiryBookingDto.BuyerInquiryPaymentScheduleDto p =
                new BuyerInquiryBookingDto.BuyerInquiryPaymentScheduleDto();
        p.setInquiryBookingMasterId(masterId);
        p.setPaymentTermId(dto.getPaymentTermId() == null ? 0 : dto.getPaymentTermId());
        p.setDueDays(dto.getDueDays() == null ? 0 : dto.getDueDays());
        p.setPctOfTotal(new BigDecimal("100"));

        BigDecimal buyerAmount = BigDecimal.ZERO;
        for (BuyerInquiryBookingDto.BuyerInquiryDetailDto d : safe(dto.getInquiryBookingDetailList())) {
            if (d.getBuyerAmount() != null) buyerAmount = buyerAmount.add(d.getBuyerAmount());
        }
        p.setDueAmount(buyerAmount);

        String base = dto.getInquiryBookingDate();
        if (base != null && base.length() >= 10) {
            p.setDueBaseDate(LocalDate.parse(base.substring(0, 10))
                                      .plusDays(p.getDueDays()).toString());
        }
        return java.util.Collections.singletonList(p);
    }

    /* ------------------------------------------------------------------------- validation */

    /**
     * formvalidation() (:1379-1490) then the quality-grid rules (:1547-1571), in the desktop's
     * order and with its own message strings - including the "Paymrnt" typo.
     *
     * Server-side because a crafted POST does not run the page.
     */
    private void validate(BuyerInquiryBookingDto dto) {
        if (zero(dto.getValidityDays()))       fail("Validity Days field is required");
        if (zero(dto.getCommissionAgentId()))  fail("Please Select Commission Agent / Broker");
        if (zero(dto.getBuyerId()))            fail("Please Select Buyer");
        if (zero(dto.getDeliveryToPartyId()))  fail("Please Select DeliveryToParty");
        /* CmbPaymentTerm / txtDueDays are header CONTROLS but not master model properties, so
           they reach the server either as the UI-only header fields or, as the page sends them,
           inside the single payment-schedule row (:1598-1599). Read whichever is present. */
        Integer termId  = headerPaymentTermId(dto);
        Integer dueDays = headerDueDays(dto);
        if (zero(termId))                      fail("Please Select Paymrnt Term");
        if (termId == 2 && zero(dueDays))      fail("Due Days field is required");
        if (zero(dto.getDeliveryTermId()))     fail("Please Select Delivery Term");
        if (zero(dto.getDeliveryDays()))       fail("Delivery Days field is required");

        List<BuyerInquiryBookingDto.BuyerInquiryDetailDto> det = safe(dto.getInquiryBookingDetailList());
        if (det.isEmpty()) fail("Item Field Required");

        /* The desktop's entry strip is a SINGLE item, so these checks are per row here. */
        for (BuyerInquiryBookingDto.BuyerInquiryDetailDto d : det) {
            if (zero(d.getInventoryParentCategoryId())) fail("Parent Item Field Required");
            if (zero(d.getItemId()))                    fail("Item Field Required");
            if (!pos(d.getItemQty()))                   fail("Qty Field Required");
            if (!pos(d.getItemWeight()))                fail("Weight Field Required");
            if (zero(d.getPackingTypeId()))             fail("Packing Type Field is Required");
            if (zero(d.getPackUomId()))                 fail("Pack Uom Field is Required");
            if (zero(d.getRateUomId()))                 fail("Rate Uom Field is Required");
            if (zero(d.getCropYearId()))                fail("Please Select Crop Year");
            if (!pos(d.getBuyerRate()))                 fail("Buyer Target Price Field Required");
            /* formvalidation() :1480 - the last check, previously missing here. */
            if (!pos(d.getSupplierRate()))              fail("Target Purchase Price Field Required");
        }

        /* :1607-1615 - a sub-party row that counts must name its party and carry an amount. */
        List<BuyerInquiryBookingDto.BuyerInquiryPartyDetailDto> parties =
                safe(dto.getInquiryBookingPartyDetailList());
        for (int i = 0; i < parties.size(); i++) {
            BuyerInquiryBookingDto.BuyerInquiryPartyDetailDto pd = parties.get(i);
            boolean counts = (pd.getSubPartyId() != null && pd.getSubPartyId() > 0)
                          || (pd.getAmount() != null && pd.getAmount().compareTo(BigDecimal.ZERO) > 0);
            if (!counts) continue;
            /* a removed saved row (actionTypeId 3) is not re-validated - DeleteDetailRow :1171 */
            if (pd.getActionTypeId() != null && pd.getActionTypeId() == 3) continue;
            /* FormHelper.ValidateField(value, name, r.RowIndex) - its own wording (FormHelper.cs:507) */
            if (zero(pd.getSubPartyId()))
                fail("Sub Party Name is required in Detail Grid at row No: " + (i + 1));
            if (!pos(pd.getAmount()))
                fail("amount is required in Detail Grid at row No: " + (i + 1));
        }

        /* :1549-1568 - checked over EVERY posted row, before the valid ones are filtered out.
           A half-filled range is an error, not something to silently drop. */
        for (BuyerInquiryBookingDto.BuyerInquiryQualitySpecificationDto q
                : safe(dto.getInquiryBookingQualitySpecificationList())) {
            boolean fromEntered = pos(q.getRangeFrom()) || neg(q.getRangeFrom());
            boolean toEntered   = pos(q.getRangeTo())   || neg(q.getRangeTo());
            String name = nz(q.getQualityParameter());
            if (fromEntered ^ toEntered) {
                fail("Please enter both Range From and Range To for parameter: " + name);
            }
            if (fromEntered && toEntered && q.getRangeFrom().compareTo(q.getRangeTo()) > 0) {
                fail("Range From cannot be greater than Range To for parameter: " + name);
            }
            if (neg(q.getRangeFrom()) || neg(q.getRangeTo())) {
                fail("Negative values are not allowed for parameter: " + name);
            }
        }
    }

    private static Integer headerPaymentTermId(BuyerInquiryBookingDto dto) {
        if (dto.getPaymentTermId() != null && dto.getPaymentTermId() != 0) return dto.getPaymentTermId();
        List<BuyerInquiryBookingDto.BuyerInquiryPaymentScheduleDto> ps =
                safe(dto.getInquiryBookingPaymentScheduleList());
        return ps.isEmpty() ? 0 : ps.get(0).getPaymentTermId();
    }

    private static Integer headerDueDays(BuyerInquiryBookingDto dto) {
        if (dto.getDueDays() != null && dto.getDueDays() != 0) return dto.getDueDays();
        List<BuyerInquiryBookingDto.BuyerInquiryPaymentScheduleDto> ps =
                safe(dto.getInquiryBookingPaymentScheduleList());
        return ps.isEmpty() ? 0 : ps.get(0).getDueDays();
    }

    /** :1569 - only rows with BOTH ends above zero are written. */
    private List<BuyerInquiryBookingDto.BuyerInquiryQualitySpecificationDto> validQualityRows(
            BuyerInquiryBookingDto dto) {
        List<BuyerInquiryBookingDto.BuyerInquiryQualitySpecificationDto> out = new ArrayList<>();
        for (BuyerInquiryBookingDto.BuyerInquiryQualitySpecificationDto q
                : safe(dto.getInquiryBookingQualitySpecificationList())) {
            if (pos(q.getRangeFrom()) && pos(q.getRangeTo())) out.add(q);
        }
        return out;
    }

    /* ------------------------------------------------------------------------------- read */

    /** ReadById plus ALL FOUR child collections, in the DAL's own order (0544:139-157). */
    public Map<String, Object> getById(int id) {
        Map<String, Object> res = new LinkedHashMap<>();
        List<Map<String, Object>> header = repository.readHeaderById(id);
        if (header == null || header.isEmpty()) {
            res.put("success", false);
            res.put("message", "Record not found");
            return res;
        }
        assertAccess(header.get(0));
        res.put("success", true);
        res.put("header", header.get(0));
        res.put("details",          repository.readDetailByHeaderId(id));
        res.put("paymentSchedule",  repository.readPaymentScheduleByHeaderId(id));
        res.put("qualitySpecs",     repository.readQualitySpecificationByHeaderId(id));
        res.put("partyDetails",     repository.readPartyDetailByHeaderId(id));
        return res;
    }

    /** DeleteByID - @EntryUserId, @Id, @Activity='DeleteById'. Not a JPA delete. */
    public Map<String, Object> deleteRecord(int entryUserId, int id) {
        Map<String, Object> res = new LinkedHashMap<>();
        List<Map<String, Object>> header = repository.readHeaderById(id);
        if (header == null || header.isEmpty()) {
            res.put("success", false);
            res.put("message", "No record found to Delete");
            return res;
        }
        assertAccess(header.get(0));
        repository.deleteById(entryUserId, id);
        res.put("success", true);
        res.put("message", "Delete Record Successfully");
        return res;
    }

    /**
     * BLL FormHistory (0492:128-332). Tenancy and CanViewAllRecord come from the session; the
     * filters (all optional) come from the page: fromDate/toDate, entryFromDate/entryToDate,
     * modifyFromDate/modifyToDate, approvedFromDate/approvedToDate, validityDateFrom/To,
     * buyerRateFrom/To, fromDocNo/toDocNo, id, commissionAgentId, buyerId, itemId, parentItemIds.
     */
    public List<Map<String, Object>> getHistory(Map<String, Object> filters) {
        return repository.formHistory(
                currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(),
                currentUserContext.currentBranchId(),
                currentUserContext.currentFinancialYearId(),
                canViewAllRecords(),
                currentUserContext.currentUserId(),
                filters == null ? new LinkedHashMap<>() : filters);
    }

    /**
     * FilldtLastAnalysisByParentItem (:991) -> BLL GetByParentCategoryId_LastinquiryBookingQualitySpecification
     * (0492:334): @OrganizationId, @CompanyId, @Id = parent category, @Activity.
     */
    public List<Map<String, Object>> lastAnalysisByParentCategory(int parentCategoryId) {
        if (parentCategoryId <= 0) return new ArrayList<>();
        return repository.lastAnalysisByParentCategory(
                currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(),
                parentCategoryId);
    }

    /* ---------------------------------------------------------------------------- helpers */

    private static <T> List<T> safe(List<T> v) { return v == null ? new ArrayList<>() : v; }
    private static boolean zero(Integer v) { return v == null || v == 0; }
    private static boolean pos(BigDecimal v) { return v != null && v.compareTo(BigDecimal.ZERO) > 0; }
    private static boolean neg(BigDecimal v) { return v != null && v.compareTo(BigDecimal.ZERO) < 0; }
    private static String nz(String v) { return v == null ? "" : v; }
    private static String plain(BigDecimal v) {
        return v == null ? "0" : v.stripTrailingZeros().toPlainString();
    }
    private static void fail(String message) { throw new IllegalArgumentException(message); }
    private static Object ci(Map<String, Object> m, String key) {
        if (m == null) return null;
        if (m.containsKey(key)) return m.get(key);
        for (Map.Entry<String, Object> e : m.entrySet()) if (e.getKey().equalsIgnoreCase(key)) return e.getValue();
        return null;
    }
    private static int toInt(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null) return 0;
        try { return Integer.parseInt(o.toString().trim()); } catch (NumberFormatException e) { return 0; }
    }
}
