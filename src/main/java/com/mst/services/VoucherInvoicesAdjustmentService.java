package com.mst.services;

import com.mst.models.dto.VoucherInvoicesAdjustmentDto;
import com.mst.models.dto.VoucherInvoicesAdjustmentLineDto;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.dao.EmptyResultDataAccessException;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * Voucher Invoices Adjustment - ditto Architecture.WinApp.Account_Definition.AdjustmentVouchers.
 * frmInvoicesAdjustmentVoucher.cs, backed by the real Architecture.BLL.Accounts.
 * VoucherInvoicesAdjustment BLL and its own dedicated [dbo].[VoucherInvoicesAdjustment] table -
 * a completely separate subsystem from the shared VoucherHead/VoucherDetail pipeline every other
 * voucher in this app uses (an earlier pass of the project's progress notes wrongly assumed this
 * screen would turn out to share that generic model the way Payment By Invoice did; reading the
 * real Save_Click/Insert()/BLL/DAL/proc source directly confirmed it does not).
 *
 * TransactionTypeId is hardcoded to 1 everywhere in this service, exactly as real desktop's own
 * frmInvoicesAdjustmentVoucher constructor does ("TransactionTypeId = 1;") - this screen only ever
 * adjusts outstanding Payment-side vouchers (CPV/BPV/JV/Bill Payables/etc.) against outstanding
 * Purchase Invoices. TransactionTypeId 2 (Receipts) and 3 (FCY Export Receipts) belong to the two
 * sibling desktop forms frmReceiptInvoicesAdjustmentVoucher.cs and
 * frmExportReceiptInvoicesAdjustmentVoucher.cs, which are out of scope for this module.
 *
 * Every stored procedure name and parameter below was read directly out of the real SQL Server
 * object script (USP_VoucherInvoicesAdjustment_InsertUpdateDelete / _ReadByVoucherHeadId /
 * _FormHistory / _PartyDropDown, USP_Vouchers_OutstandingForAdjustment,
 * USP_PurchaseInvoice_GetDataForAdjustment) - none of it guessed. No schema, column, or stored
 * procedure was changed to build this.
 */
@Service
public class VoucherInvoicesAdjustmentService {

    private static final int TRANSACTION_TYPE_ID = 1;
    private static final String SCREEN_NAME = "frmInvoicesAdjustmentVoucher";

    @Autowired
    private JdbcTemplate jdbcTemplate;

    @Autowired
    private CurrentUserContext currentUserContext;

    /** ditto InitializeComponentMethod()'s Vouchers_OutstandingForAdjustment(OrgId, CompId, 1, 0)
     *  call (unfiltered, on page load) and btnShowRecords_Click()'s per-account re-fetch. */
    public List<Map<String, Object>> getOutstandingForAdjustment(Integer accountId) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        try {
            if (accountId != null && accountId > 0) {
                return jdbcTemplate.queryForList(
                        "EXEC USP_Vouchers_OutstandingForAdjustment @OrganizationId=?, @CompanyId=?, @TransTypeId=?, @AccountId=?",
                        orgId, compId, TRANSACTION_TYPE_ID, accountId);
            }
            return jdbcTemplate.queryForList(
                    "EXEC USP_Vouchers_OutstandingForAdjustment @OrganizationId=?, @CompanyId=?, @TransTypeId=?",
                    orgId, compId, TRANSACTION_TYPE_ID);
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    /** ditto LoadInvoiceData()/PurchaseInvoice_GetDataForAdjustment(). */
    public List<Map<String, Object>> getPurchaseInvoicesForAdjustment(int supplierCustomerId, Integer partyGlId) {
        int orgId = currentUserContext.currentOrganizationId();
        int compId = currentUserContext.currentCompanyId();
        try {
            if (partyGlId != null && partyGlId > 0) {
                return jdbcTemplate.queryForList(
                        "EXEC USP_PurchaseInvoice_GetDataForAdjustment @OrganizationId=?, @CompanyId=?, @SupplierCustomerId=?, @PartyGlId=?",
                        orgId, compId, supplierCustomerId, partyGlId);
            }
            return jdbcTemplate.queryForList(
                    "EXEC USP_PurchaseInvoice_GetDataForAdjustment @OrganizationId=?, @CompanyId=?, @SupplierCustomerId=?",
                    orgId, compId, supplierCustomerId);
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    /** ditto ReadById()/GetByVoucherHeadId() - reloads a previously-saved adjustment voucher's
     *  own detail rows (each one still a real VoucherInvoicesAdjustment row, ActionTypeId<>3). */
    public Map<String, Object> getByVoucherHeadId(int voucherHeadId, int paymentTypeId, Integer supplierCustomerId, Integer partyGlId) {
        Map<String, Object> result = new HashMap<>();
        try {
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                    "EXEC USP_VoucherInvoicesAdjustment_ReadByVoucherHeadId @TransactionTypeId=?, @VoucherHeadId=?, @PaymentTypeId=?, @SupplierCustomerId=?, @PartyGlId=?",
                    TRANSACTION_TYPE_ID, voucherHeadId, paymentTypeId, supplierCustomerId, partyGlId);
            result.put("rows", rows);
        } catch (Exception e) {
            result.put("rows", Collections.emptyList());
        }
        return result;
    }

    /** ditto HistoryComboBind()/PartyDropDown(). */
    public List<Map<String, Object>> getPartyDropDown() {
        try {
            return jdbcTemplate.queryForList(
                    "EXEC USP_VoucherInvoicesAdjustment_PartyDropDown @TransactionTypeId=?", TRANSACTION_TYPE_ID);
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    /** ditto FillHistory(). Note: USP_VoucherInvoicesAdjustment_FormHistory's real signature (read
     *  directly from its CREATE PROCEDURE body) has no @EntryUser/@CanViewAllRecord parameter and
     *  no approved-status filter at all - unlike USP_VoucherFormHistory (used by every other
     *  voucher's History tab), this proc simply does not support restricting by entry user, so
     *  desktop's own PI.EntryUser assignment on this screen is effectively a no-op the stored
     *  procedure ignores. Not fixed here - the underlying proc's shape can't be changed and this
     *  Java port must call it exactly as it is, not as desktop's C# object model implies. */
    public List<Map<String, Object>> getFormHistory(String dateType, LocalDate fromDate, LocalDate toDate,
            Integer fromDocNo, Integer toDocNo, Integer supplierCustomerId, Integer partyGlId) {
        StringBuilder sql = new StringBuilder("EXEC USP_VoucherInvoicesAdjustment_FormHistory @TransactionTypeId=?");
        List<Object> params = new ArrayList<>();
        params.add(TRANSACTION_TYPE_ID);

        if (fromDocNo != null && fromDocNo != 0) { sql.append(", @VoucherNoFrom=?"); params.add(fromDocNo); }
        if (toDocNo != null && toDocNo != 0) { sql.append(", @VoucherNoTo=?"); params.add(toDocNo); }

        if ("entrydate".equalsIgnoreCase(dateType)) {
            if (fromDate != null) { sql.append(", @EntryFromDate=?"); params.add(fromDate); }
            if (toDate != null) { sql.append(", @EntryToDate=?"); params.add(toDate); }
        } else if ("modifydate".equalsIgnoreCase(dateType)) {
            if (fromDate != null) { sql.append(", @ModifyFromDate=?"); params.add(fromDate); }
            if (toDate != null) { sql.append(", @ModifyToDate=?"); params.add(toDate); }
        } else {
            // default (and explicit "docdate"): ditto desktop's drdocdate radio default
            if (fromDate != null) { sql.append(", @VoucherDateFrom=?"); params.add(fromDate); }
            if (toDate != null) { sql.append(", @VoucherDateTo=?"); params.add(toDate); }
        }

        if (supplierCustomerId != null && supplierCustomerId > 0 && partyGlId != null && partyGlId > 0) {
            sql.append(", @SupplierCustomerId=?, @PartyGlId=?");
            params.add(supplierCustomerId);
            params.add(partyGlId);
        }

        try {
            return jdbcTemplate.queryForList(sql.toString(), params.toArray());
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    /** ditto Insert() (shared by both BtnSave_Click and btnUpdate_Click in real desktop): only
     *  rows with an existing Id OR a positive AdjustmentAmount are saved (FormValidation's own
     *  qualifying-row filter), each one posted through USP_VoucherInvoicesAdjustment_
     *  InsertUpdateDelete individually - exactly like the real DAL's SetData() method, which loops
     *  obj.VoucherInvoicesAdjustmentList and calls the proc once per row rather than as a single
     *  batched/TVP call. */
    @Transactional
    public Map<String, Object> save(VoucherInvoicesAdjustmentDto dto) {
        Map<String, Object> resp = new HashMap<>();
        try {
            if (dto.getVoucherHeadId() == null || dto.getVoucherHeadId() <= 0) {
                throw new IllegalArgumentException("Voucher No Field is Required");
            }
            if (dto.getVoucherAmount() == null || dto.getVoucherAmount() <= 0) {
                throw new IllegalArgumentException("VoucherAmount Field is Required");
            }
            if (dto.getPaymentTypeId() == null || dto.getPaymentTypeId() <= 0) {
                throw new IllegalArgumentException("Payment Type Field is Required");
            }
            if (dto.getSupplierCustomerId() == null || dto.getSupplierCustomerId() <= 0) {
                throw new IllegalArgumentException("Party Field is Required");
            }

            boolean editing = dto.getRecId() != null && dto.getRecId() > 0;

            List<VoucherInvoicesAdjustmentLineDto> qualifying = new ArrayList<>();
            double totalAdjustment = 0;
            if (dto.getLines() != null) {
                for (VoucherInvoicesAdjustmentLineDto l : dto.getLines()) {
                    boolean idPositive = l.getId() != null && l.getId() > 0;
                    double adj = l.getAdjustmentAmount() != null ? l.getAdjustmentAmount() : 0;
                    if (idPositive || adj > 0) {
                        qualifying.add(l);
                        totalAdjustment += adj;
                    }
                }
            }
            if (qualifying.isEmpty()) {
                throw new IllegalArgumentException("At least enter one row in detail");
            }

            // ditto Insert()'s own cross-check: PaymentTypeId==1 is an Advance-payment voucher
            // (grid total may under-adjust, never over-adjust); every other PaymentTypeId requires
            // an exact match to the voucher's own amount.
            if (dto.getPaymentTypeId() == 1) {
                if (Math.round(totalAdjustment) > Math.round(dto.getVoucherAmount())) {
                    throw new IllegalArgumentException("Total adjustment Amount " + Math.round(totalAdjustment)
                            + " In Grid cannot be greater then to voucher amount " + Math.round(dto.getVoucherAmount()));
                }
            } else if (Math.round(totalAdjustment) != Math.round(dto.getVoucherAmount())) {
                throw new IllegalArgumentException("Total adjustment Amount " + Math.round(totalAdjustment)
                        + " In Grid Should be equal to voucher amount " + Math.round(dto.getVoucherAmount()));
            }

            int userId = currentUserContext.currentUserId();
            Timestamp now = new Timestamp(System.currentTimeMillis());

            for (VoucherInvoicesAdjustmentLineDto l : qualifying) {
                int lineId = (editing && l.getId() != null) ? l.getId() : 0;
                int actionTypeId = lineId > 0 ? 2 : 1;
                double adjustmentAmount;
                double advanceAmount;
                double rawAmount = l.getAdjustmentAmount() != null ? l.getAdjustmentAmount() : 0;
                if (dto.getPaymentTypeId() == 1) {
                    advanceAmount = rawAmount;
                    adjustmentAmount = 0;
                } else {
                    adjustmentAmount = rawAmount;
                    advanceAmount = 0;
                }
                callInsertUpdateDelete(lineId, dto, l, actionTypeId, adjustmentAmount, advanceAmount, userId, now);
            }

            // ditto lstRemoveDetailRecord merge: only meaningful when editing an existing voucher
            // (a brand-new voucher's removed rows simply never existed on the server to begin with).
            if (editing && dto.getRemovedLineIds() != null) {
                for (Integer removedId : dto.getRemovedLineIds()) {
                    if (removedId == null || removedId <= 0) continue;
                    callInsertUpdateDelete(removedId, dto, null, 3, 0, 0, userId, now);
                }
            }

            resp.put("success", true);
            resp.put("voucherHeadId", dto.getVoucherHeadId());
            resp.put("message", editing
                    ? "Record Update Successfully [" + dto.getVoucherCode() + "]"
                    : "Record Saved Successfully [" + dto.getVoucherCode() + "]");
        } catch (Exception e) {
            resp.put("success", false);
            resp.put("message", e.getMessage());
        }
        return resp;
    }

    /** ditto btnDelete_Click(): marks every currently-loaded saved detail row ActionTypeId=3 -
     *  a whole-voucher-adjustment delete, not a per-line one (per-line deletes are handled inline
     *  by save()'s removedLineIds, matching DeleteDetailRow()'s own deferred-to-next-Save design). */
    @Transactional
    public Map<String, Object> deleteAll(VoucherInvoicesAdjustmentDto dto) {
        Map<String, Object> resp = new HashMap<>();
        try {
            if (dto.getRecId() == null || dto.getRecId() <= 0) {
                throw new IllegalArgumentException("Record Id not found for deletion...");
            }
            int userId = currentUserContext.currentUserId();
            Timestamp now = new Timestamp(System.currentTimeMillis());
            boolean any = false;
            if (dto.getLines() != null) {
                for (VoucherInvoicesAdjustmentLineDto l : dto.getLines()) {
                    if (l.getId() != null && l.getId() > 0) {
                        callInsertUpdateDelete(l.getId(), dto, l, 3, 0, 0, userId, now);
                        any = true;
                    }
                }
            }
            if (!any) {
                throw new IllegalArgumentException("No saved detail rows found to delete");
            }
            resp.put("success", true);
            resp.put("message", "Deleted Successfully");
        } catch (Exception e) {
            resp.put("success", false);
            resp.put("message", e.getMessage());
        }
        return resp;
    }

    /** One EXEC of USP_VoucherInvoicesAdjustment_InsertUpdateDelete per detail row, matching the
     *  real DAL's per-row loop. Parameter list, order, and types are copied verbatim from the
     *  stored procedure's own CREATE PROCEDURE signature. DocumentTypeId/ExchangeRate/
     *  PaymentTermId/FcyId/ExImInvoicePaymentTermsDetailId are always sent NULL here because
     *  frmInvoicesAdjustmentVoucher.cs's own Insert() never sets those five C# properties either -
     *  they only get real values on the sibling FCY/Export-Receipts form (TransactionTypeId=3),
     *  out of scope for this module. The proc itself returns the affected Id via a trailing
     *  "SELECT @Id" for the Insert/Update branches only (no result set at all for Delete), so
     *  Insert/Update go through queryForObject and Delete goes through update(). */
    private void callInsertUpdateDelete(Integer id, VoucherInvoicesAdjustmentDto dto, VoucherInvoicesAdjustmentLineDto line,
            int actionTypeId, double adjustmentAmount, double advanceAmount, int userId, Timestamp now) {
        String sql = "EXEC USP_VoucherInvoicesAdjustment_InsertUpdateDelete " +
                "@Id=?, @TransactionTypeId=?, @PartyGlId=?, @SupplierCustomerId=?, @VoucherCode=?, @VoucherDate=?, " +
                "@VoucherHeadId=?, @RefDocumentTypeId=?, @RefDocNoId=?, @InvoiceAmount=?, @AdjustmentAmount=?, @AdvanceAmount=?, " +
                "@DueDate=?, @Remarks=?, @EntryDate=?, @EntryUserId=?, @ModifyDate=?, @ModifyUserId=?, @ActionTypeId=?, " +
                "@ScreenName=?, @PaymentTypeId=?, @VoucherAmount=?, @DocumentTypeId=?, @ExchangeRate=?, @PaymentTermId=?, " +
                "@FcyId=?, @ExImInvoicePaymentTermsDetailId=?";
        Object[] params = new Object[]{
                (id != null && id > 0) ? id : null,
                TRANSACTION_TYPE_ID,
                dto.getPartyGlId(),
                dto.getSupplierCustomerId(),
                dto.getVoucherCode(),
                dto.getVoucherDate(),
                dto.getVoucherHeadId(),
                line != null ? line.getRefDocumentTypeId() : null,
                line != null ? line.getRefDocNoId() : null,
                (line != null && line.getInvoiceAmount() != null) ? line.getInvoiceAmount() : 0.0,
                adjustmentAmount,
                advanceAmount,
                dto.getVoucherDate(), // DueDate = txtVoucherDate.Value in the real Insert()
                line != null ? line.getRemarks() : null,
                now,
                userId,
                now,
                userId,
                actionTypeId,
                SCREEN_NAME,
                dto.getPaymentTypeId(),
                dto.getVoucherAmount(),
                null, // DocumentTypeId - not used by this form's TransactionTypeId=1 path
                null, // ExchangeRate
                null, // PaymentTermId
                null, // FcyId
                null  // ExImInvoicePaymentTermsDetailId
        };
        if (actionTypeId == 3) {
            jdbcTemplate.update(sql, params);
        } else {
            try {
                jdbcTemplate.queryForObject(sql, Integer.class, params);
            } catch (EmptyResultDataAccessException ex) {
                // The proc's trailing "SELECT @Id" returned no row in some edge case - the write
                // itself still committed (it happens before the SELECT), so this is not an error,
                // matching desktop which never surfaces this scenario either.
            }
        }
    }
}
