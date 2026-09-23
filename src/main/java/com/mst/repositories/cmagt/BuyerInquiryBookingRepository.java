package com.mst.repositories.cmagt;

import com.mst.repositories.support.ProcExec;

import com.mst.models.cmagt.dto.BuyerInquiryBookingDto;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.sql.Types;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

@Repository
public class BuyerInquiryBookingRepository {

    @Autowired
    private JdbcTemplate jdbc;

    private static final String SQL_MASTER_SAVE =
            "EXEC [cmagt].[USP_inquiryBookingMaster_InsertAndUpdate] "
            + "@isApproved=?, @approvedDate=?, @deliveryStartDate=?, @entryDate=?, @expiryDate=?, "
            + "@inquiryBookingDate=?, @modifyDate=?, @validityDate=?, @actionId=?, @analysisGroupId=?, "
            + "@approvedUserId=?, @branchId=?, @buyerId=?, @commissionAgentId=?, @inquiryStatusId=?, "
            + "@companyId=?, @deliveryDays=?, @deliveryTermId=?, @documentTypeId=?, @entryUserId=?, "
            + "@financialYearId=?, @inquiryBookingMasterId=?, @inquiryBookingNo=?, @modifyUserId=?, "
            + "@organizationId=?, @projectId=?, @validityDays=?, @ShipToAddress=?, @shipToAddressId=?, "
            + "@DeliveryToPartyId=?, @remarksHeader=?, @approvalRemarks=?, @attachmentsValues=?, "
            + "@customAttachmentsValues=?, @QualitySpecifications=?";

    private static final String SQL_DETAIL_SAVE =
            "EXEC [cmagt].[USP_inquiryBookingDetail_Insert] "
            + "@buyerAmount=?, @buyerRate=?, @itemQty=?, @itemWeight=?, @SupplierAmount=?, "
            + "@supplierRate=?, @actionTypeId=?, @cropYearId=?, @inquiryBookingDetailId=?, "
            + "@inquiryBookingMasterId=?, @inventoryParentCategoryId=?, @itemId=?, @packingTypeId=?, "
            + "@packUomId=?, @rateUomId=?, @sortNo=?, @cropYear=?, @remarks=?, @QualitySpecifications=?";

    private static final String SQL_PARTY_DETAIL_SAVE =
            "EXEC [cmagt].[USP_InquiryBookingPartyDetail_Insert] "
            + "@inquiryBookingPartyDetailId=?, @inquiryBookingMasterId=?, @SubPartyId=?, @itemQty=?, "
            + "@Rate=?, @rateUomId=?, @Amount=?, @sortNo=?, @remarks=?, @actionTypeId=?";

    /**
     * [cmagt].[USP_inquiryBookingPaymentSchedule_Insert] - the 9 non-virtual properties of
     * Architecture.Model.CommissionAgent.InquiryBookingPaymentSchedule, declaration order.
     *
     * This procedure was NEVER CALLED. The DAL writes it for every inquiry
     * (0544:61-65), so every document saved from the web was missing its payment schedule
     * while the save reported success.
     */
    private static final String SQL_PAYMENT_SCHEDULE_SAVE =
            "EXEC [cmagt].[USP_inquiryBookingPaymentSchedule_Insert] "
            + "@dueBaseDate=?, @dueAmount=?, @pctOfTotal=?, @dueDays=?, @inquiryBookingMasterId=?, "
            + "@inquiryBookingPaymentScheduleId=?, @paymentTermId=?, @sortNo=?, @remarks=?";

    /**
     * [cmagt].[USP_inquiryBookingQualitySpecification_Insert] - the 7 non-virtual properties of
     * Architecture.Model.CommissionAgent.InquiryBookingQualitySpecification.
     *
     * Also never called. qualityParameter is virtual on the desktop model and is therefore NOT
     * a parameter - SetProc skips virtual properties.
     */
    private static final String SQL_QUALITY_SPEC_SAVE =
            "EXEC [cmagt].[USP_inquiryBookingQualitySpecification_Insert] "
            + "@rangeFrom=?, @rangeTo=?, @inquiryBookingMasterId=?, "
            + "@inquiryBookingQualitySpecificationId=?, @qualityParameterId=?, @sortNo=?, @remarks=?";

    private static final String SQL_READ_BY_ID =
            "EXEC [cmagt].[USP_inquiryBookingMaster_GetAllMethod] @Id=?, @Activity=?";

    private static final String SQL_GENERATE_CODE =
            "EXEC [cmagt].[USP_inquiryBookingMaster_GetAllMethod] @OrganizationId=?, @CompanyId=?, "
            + "@BranchesId=?, @FinancialYearId=?, @DocumentTypeId=?, @Activity=?";

    private static final String SQL_DELETE_BY_ID =
            "EXEC [cmagt].[USP_inquiryBookingMaster_GetAllMethod] @EntryUserId=?, @Id=?, @Activity=?";

    /**
     * [cmagt].[USP_inquiryBookingMaster_FormHistory].
     *
     * FIVE parameters are unconditional; the other eighteen are GUARDED in the BLL
     * (0492_Architecture.BLL...InquiryBookingMaster.cs:128-340) and are OMITTED when unset,
     * never sent as NULL:
     *
     *     @EntryUserId       only when !CanViewAllRecord
     *     @FromDate @ToDate @EntryFromDate @EntryToDate @ModifyFromDate @ModifyToDate
     *     @ApprovedFromDate @ApprovedToDate @ValidityDateFrom @ValidityDateTo
     *                        only when the date is not null
     *     @BuyerRateFrom @BuyerRateTo @FromDocNo @ToDocNo
     *                        only when the number is not 0
     *     @Id @CommissionAgentId @BuyerId @ItemId
     *                        only when the id is not 0
     *     @ParentItemIds     only when the string is non-empty
     *
     * The previous version sent all twenty-three every time, with NULL for the unset ones. A
     * procedure that branches on a parameter's PRESENCE does not treat NULL the same as absent -
     * this is the same defect class as the GDN under-filled write. Worse here: @EntryUserId was
     * sent unconditionally, so a user who DOES hold CanViewAllRecord still had their own id
     * pushed into the filter and saw only their own documents.
     *
     * The statement is therefore built per call from the parameters that actually apply.
     */
    private static final String FORM_HISTORY_PROC = "[cmagt].[USP_inquiryBookingMaster_FormHistory]";

    public int saveMaster(BuyerInquiryBookingDto h) {
        return scalarInt(SQL_MASTER_SAVE,
                bool(h.getIsApproved()),
                ts(h.getApprovedDate()),
                ts(h.getDeliveryStartDate()),
                ts(h.getEntryDate()),
                ts(h.getExpiryDate()),
                ts(h.getInquiryBookingDate()),
                ts(h.getModifyDate()),
                ts(h.getValidityDate()),
                i(h.getActionId()),
                i(h.getAnalysisGroupId()),
                i(h.getApprovedUserId()),
                i(h.getBranchId()),
                i(h.getBuyerId()),
                i(h.getCommissionAgentId()),
                i(h.getInquiryStatusId()),
                i(h.getCompanyId()),
                i(h.getDeliveryDays()),
                i(h.getDeliveryTermId()),
                i(h.getDocumentTypeId()),
                i(h.getEntryUserId()),
                i(h.getFinancialYearId()),
                i(h.getInquiryBookingMasterId()),
                i(h.getInquiryBookingNo()),
                i(h.getModifyUserId()),
                i(h.getOrganizationId()),
                i(h.getProjectId()),
                i(h.getValidityDays()),
                s(h.getShipToAddress()),
                i(h.getShipToAddressId()),
                i(h.getDeliveryToPartyId()),
                s(h.getRemarksHeader()),
                s(h.getApprovalRemarks()),
                s(h.getAttachmentsValues()),
                s(h.getCustomAttachmentsValues()),
                s(h.getQualitySpecifications())
        );
    }

    public void saveDetailRow(BuyerInquiryBookingDto.BuyerInquiryDetailDto d) {
        scalarInt(SQL_DETAIL_SAVE,
                num(d.getBuyerAmount()),
                num(d.getBuyerRate()),
                num(d.getItemQty()),
                num(d.getItemWeight()),
                num(d.getSupplierAmount()),
                num(d.getSupplierRate()),
                i(d.getActionTypeId()),
                i(d.getCropYearId()),
                i(d.getInquiryBookingDetailId()),
                i(d.getInquiryBookingMasterId()),
                i(d.getInventoryParentCategoryId()),
                i(d.getItemId()),
                i(d.getPackingTypeId()),
                i(d.getPackUomId()),
                i(d.getRateUomId()),
                i(d.getSortNo()),
                s(d.getCropYear()),
                s(d.getRemarks()),
                s(d.getQualitySpecifications())
        );
    }

    public void savePartyDetailRow(BuyerInquiryBookingDto.BuyerInquiryPartyDetailDto p) {
        scalarInt(SQL_PARTY_DETAIL_SAVE,
                i(p.getInquiryBookingPartyDetailId()),
                i(p.getInquiryBookingMasterId()),
                i(p.getSubPartyId()),
                num(p.getItemQty()),
                num(p.getRate()),
                i(p.getRateUomId()),
                num(p.getAmount()),
                i(p.getSortNo()),
                s(p.getRemarks()),
                i(p.getActionTypeId())
        );
    }

    public void savePaymentScheduleRow(BuyerInquiryBookingDto.BuyerInquiryPaymentScheduleDto p) {
        scalarInt(SQL_PAYMENT_SCHEDULE_SAVE,
                ts(p.getDueBaseDate()),
                num(p.getDueAmount()),
                num(p.getPctOfTotal()),
                i(p.getDueDays()),
                i(p.getInquiryBookingMasterId()),
                i(p.getInquiryBookingPaymentScheduleId()),
                i(p.getPaymentTermId()),
                i(p.getSortNo()),
                s(p.getRemarks())
        );
    }

    public void saveQualitySpecificationRow(BuyerInquiryBookingDto.BuyerInquiryQualitySpecificationDto q) {
        scalarInt(SQL_QUALITY_SPEC_SAVE,
                num(q.getRangeFrom()),
                num(q.getRangeTo()),
                i(q.getInquiryBookingMasterId()),
                i(q.getInquiryBookingQualitySpecificationId()),
                i(q.getQualityParameterId()),
                i(q.getSortNo()),
                s(q.getRemarks())
        );
    }

    public List<Map<String, Object>> readHeaderById(int id) {
        return jdbc.queryForList(SQL_READ_BY_ID, id, "ReadById");
    }

    public List<Map<String, Object>> readDetailByHeaderId(int id) {
        return jdbc.queryForList(SQL_READ_BY_ID, id, "ReadByHeaderId_inquiryBookingDetail");
    }

    public List<Map<String, Object>> readPartyDetailByHeaderId(int id) {
        return jdbc.queryForList(SQL_READ_BY_ID, id, "ReadByHeaderId_InquiryBookingPartyDetail");
    }

    /** DAL ReadById child #2 - activity spelling is the DAL's own (0544:145). */
    public List<Map<String, Object>> readPaymentScheduleByHeaderId(int id) {
        return jdbc.queryForList(SQL_READ_BY_ID, id, "ReadByHeaderId_inquiryBookingPaymentSchedule");
    }

    /** DAL ReadById child #3 (0544:151). */
    public List<Map<String, Object>> readQualitySpecificationByHeaderId(int id) {
        return jdbc.queryForList(SQL_READ_BY_ID, id, "ReadByHeaderId_inquiryBookingQualitySpecification");
    }

    public int generateCode(int orgId, int companyId, int branchId, int financialYearId, int documentTypeId) {
        List<Map<String, Object>> rows = jdbc.queryForList(SQL_GENERATE_CODE,
                orgId, companyId, branchId, financialYearId, documentTypeId, "GenerateCode");
        if (rows.isEmpty()) return 1;
        Object v = rows.get(0).get("DocumentNo");
        if (v == null) v = rows.get(0).get("DocNo");
        return (v instanceof Number) ? ((Number) v).intValue() : 1;
    }

    public void deleteById(int entryUserId, int id) {
        ProcExec.call(jdbc, SQL_DELETE_BY_ID, entryUserId, id, "DeleteById");
    }

    public List<Map<String, Object>> formHistory(int orgId, int companyId, int branchId, int financialYearId,
                                                 boolean canViewAllRecords, int entryUserId,
                                                 String fromDate, String toDate,
                                                 Integer fromDocNo, Integer toDocNo, Integer id,
                                                 Integer commissionAgentId, Integer buyerId, Integer itemId) {
        List<String> names = new ArrayList<>();
        List<Object> args = new ArrayList<>();

        /* always, in the BLL's own order */
        add(names, args, "@OrganizationId",   orgId);
        add(names, args, "@CompanyId",        companyId);
        add(names, args, "@BranchesId",       branchId);
        add(names, args, "@FinancialYearId",  financialYearId);
        add(names, args, "@CanViewAllRecord", canViewAllRecords);

        /* guarded - omitted, not nulled */
        if (!canViewAllRecords)              add(names, args, "@EntryUserId", entryUserId);
        Object f = dateOrNull(fromDate);
        if (f != null)                       add(names, args, "@FromDate", f);
        Object t = dateOrNull(toDate);
        if (t != null)                       add(names, args, "@ToDate", t);
        if (nonZero(fromDocNo))              add(names, args, "@FromDocNo", fromDocNo);
        if (nonZero(toDocNo))                add(names, args, "@ToDocNo", toDocNo);
        if (nonZero(id))                     add(names, args, "@Id", id);
        if (nonZero(commissionAgentId))      add(names, args, "@CommissionAgentId", commissionAgentId);
        if (nonZero(buyerId))                add(names, args, "@BuyerId", buyerId);
        if (nonZero(itemId))                 add(names, args, "@ItemId", itemId);

        StringBuilder sql = new StringBuilder("EXEC ").append(FORM_HISTORY_PROC).append(' ');
        for (int k = 0; k < names.size(); k++) {
            if (k > 0) sql.append(", ");
            sql.append(names.get(k)).append("=?");
        }
        return jdbc.queryForList(sql.toString(), args.toArray());
    }

    private static void add(List<String> names, List<Object> args, String name, Object value) {
        names.add(name);
        args.add(value);
    }

    private static boolean nonZero(Integer v) { return v != null && v != 0; }


    /**
     * Same defect class as PurchaseOrderHeaderRepository.exec - see the note there.
     *
     * This used jdbc.query(sql, ResultSetExtractor), which goes through executeQuery() and so
     * REQUIRES the statement to produce a result set. These *_InsertAndUpdate procedures return
     * the id on their INSERT path; whether they also return one on their UPDATE path has not
     * been verified, and Sp_PurchaseOrder_Update - the same shape - returns nothing, which made
     * every Purchase Order Update fail with "The statement did not return a result set."
     *
     * ProcExec walks the whole result/update-count chain and returns the first scalar it finds
     * or null, which is what the desktop's GenericProvider.SetProc does via ExecuteScalar().
     * It behaves identically when a result set IS returned, so this is safe for the insert path
     * that already works, and removes the latent failure on the update path.
     *
     * NOT RUNTIME TESTED on these two screens.
     */
    private int scalarInt(String sql, Object... args) {
        Integer v = com.mst.repositories.support.ProcExec.call(jdbc, sql, args);
        return v == null ? 0 : v;
    }

    private static Object s(String v) { return v == null ? "" : v; }
    private static Object i(Integer v) { return v == null ? 0 : v; }
    private static Object num(java.math.BigDecimal v) { return v == null ? java.math.BigDecimal.ZERO : v; }
    private static Object bool(Boolean v) { return v != null && v; }

    private static Object ts(String iso) {
        LocalDateTime t = parse(iso);
        return Timestamp.valueOf(t == null ? LocalDateTime.now() : t);
    }

    private static Object intOrNull(Integer v) {
        return (v == null || v == 0)
                ? new org.springframework.jdbc.core.SqlParameterValue(Types.INTEGER, null)
                : v;
    }

    private static Object dateOrNull(String iso) {
        LocalDateTime t = parse(iso);
        return t == null ? new org.springframework.jdbc.core.SqlParameterValue(Types.TIMESTAMP, null)
                : Timestamp.valueOf(t);
    }

    private static LocalDateTime parse(String iso) {
        if (iso == null || iso.trim().isEmpty()) return null;
        String v = iso.trim();
        try {
            if (v.length() <= 10) return LocalDate.parse(v).atStartOfDay();
            return LocalDateTime.parse(v.replace(' ', 'T').substring(0, Math.min(19, v.length())));
        } catch (Exception ex) {
            return null;
        }
    }
}
