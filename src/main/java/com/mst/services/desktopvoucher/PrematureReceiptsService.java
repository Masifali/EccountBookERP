package com.mst.services.desktopvoucher;

import com.mst.models.dto.ContraVoucherDto;
import com.mst.repositories.support.DesktopProc;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.services.desktopvoucher.DesktopVoucherSupport.*;

/**
 * Screen 25 "Premature Receipts" - Architecture.WinApp.Account_Definition.
 * AccountsPrematurePaymentsReceipts (DocumentTypeId 159). There was no web page for it.
 *
 * BLL 0634 AccountsPrematurePaymentsReceipts / DAL 0567 AccountsPrematureReceipts:
 *   Save            -> per grid row Sp_AccountsPrematurePaymentsReceipts_Insert | _Update (Id > 0)
 *   UpdateRecord    -> SetDataForUpdate: re-read the slip (ReadByTrackingNo); on "Confirm" post a
 *                      CRV/BRV/JV through Sp_VoucherHead_Insert + Sp_VoucherDetail_Insert (+ cost
 *                      centres), then RecordStatusUpdateById for every row of the slip
 *   GenerateCode, GetPendingRecords, FormHistory, ReadByMultiParams, GetDataForDropDown.
 *
 * Desktop behaviours reproduced deliberately (not corrected):
 *  - Save never deletes: a row removed from the grid while editing a slip stays in the table.
 *  - A NEW row is inserted with DocNo 0 (the form only copies DocNo for rows that have an Id).
 *  - The Confirm voucher: VoucherCode = the slip's DocNo; LineId is not advanced after the credit
 *    line (the next pair's debit repeats it); Remarks is "text + ' ' + Remarks" (the generated
 *    text twice when the slip has no remarks, a leading space otherwise); no USP_VoucherBalanceCheck,
 *    no history mirror, no approval row (DAL 0567 calls none of them); the cost-centre rows are
 *    written with VoucherHeaderId = voucherHead.Id and VoucherDetailId = detail.Id, both of which
 *    the DAL never assigns (0).
 *  - The bulk "Confirm Vouchers" button sends DocumentTypeId 0 (RefDocumentTypeId 0 on the voucher);
 *    the grid's Confirm/Cancel buttons send 159.
 * Not ported: DMS attachments (the Confirm path also copies/renames attachment files on disk).
 */
@Service
public class PrematureReceiptsService {

    public static final int DOC_TYPE = 159;
    public static final String SCREEN = "AccountsPrematurePaymentsReceipts";
    private static final String PROC = "Sp_AccountsPrematurePaymentsReceipts_GetAllMethod";

    @Autowired private DesktopVoucherSupport s;
    @Autowired private JdbcTemplate jdbc;

    /** IsBookingOffice_App = AppId == 5; IsCustomerPortal_App = AppId 4 or 6. */
    private boolean bookingOffice() { return s.appId() == 5; }
    private boolean customerPortal() { int a = s.appId(); return a == 4 || a == 6; }

    // ================================================================================= lookups

    public Map<String, Object> lookups() {
        Map<String, Object> m = new LinkedHashMap<>();
        int app = s.appId();
        boolean booking = app == 5, portal = app == 4 || app == 6;
        m.put("rights", s.rights(SCREEN));
        m.put("appId", app);
        m.put("bookingOffice", booking);
        m.put("customerPortal", portal);
        /* CostCenterFill() - only for the booking-office app */
        m.put("costCenters", booking ? costCenters() : new ArrayList<>());
        m.put("docNo", generateCode());
        /* VoucherTypeFill(): CRV 3, BRV 4, and JV 5 unless booking office / customer portal */
        List<Map<String, Object>> vt = new ArrayList<>();
        vt.add(kv(3, "CRV")); vt.add(kv(4, "BRV"));
        if (!booking && app != 4 && app != 6) vt.add(kv(5, "JV"));
        m.put("voucherTypes", vt);
        m.put("banks", DesktopProc.rows(jdbc, "Sp_PdcBankName_GetAllMethod",
                DesktopProc.params("OrganizationId", s.orgId(), "CompanyId", s.companyId(), "Activity", "ReadAll")));
        List<Map<String, Object>> st = new ArrayList<>(); st.add(kv(1, "Pending"));
        m.put("statuses", st);
        /* OtherPartiesFill() -> CommonServices.GetOtherPartiesForPrematureReciept */
        m.put("representatives", DesktopProc.rows(jdbc, "[Sp_OtherParties_GetAllMethod]",
                DesktopProc.params("OrganizationId", s.orgId(), "CompanyId", s.companyId(),
                        "DocumentTypeId", DOC_TYPE, "Activity", "ReadAll")));
        int dd = toInt(s.config("DefaultDaysToLessFromHistoryFromDate"));
        m.put("historyFromDaysBack", dd > 0 ? dd : 3);
        /* CommonServices.GetDecimalConfiguration() - CommasApplyWhileTypingOnVouchers trims the typed
           decimals to DefaultNoofDecimalPointsForAmount (txtSlipAmount / txtAmount TextChanged). */
        m.put("decimals", s.decimals());
        return m;
    }

    private static Map<String, Object> kv(int id, String name) {
        Map<String, Object> r = new LinkedHashMap<>(); r.put("Id", id); r.put("Name", name); return r;
    }

    /** UsersWithCostCenter.UsersWithCostCenter_AllocatedData (BLL 0516:74), AppId unset. */
    public List<Map<String, Object>> costCenters() {
        int uid = s.userId();
        return DesktopProc.rows(jdbc, "[dbo].[USP_UsersWithCostCenter_AllocatedData]",
                DesktopProc.params("OrganizationId", s.orgId(), "CompanyId", s.companyId(),
                        "UserId", uid > 0 ? uid : null));
    }

    /** SenderPartiesBind(costCenterId): CoaAllocationAccountTitleByAccountTypeIds("3,6,8", null, cc). */
    public List<Map<String, Object>> senders(int costCenterId) {
        return s.coaAccountTitleByAccountTypeIds("3,6,8", costCenterId);
    }

    /** ReceiverAccountBind(): BRV -> "15", CRV -> "2", JV -> "3,6,8". */
    public List<Map<String, Object>> receivers(int voucherTypeId) {
        if (voucherTypeId == 4) return s.coaAccountTitleByAccountTypeIds("15", 0);
        if (voucherTypeId == 3) return s.coaAccountTitleByAccountTypeIds("2", 0);
        if (voucherTypeId == 5) return s.coaAccountTitleByAccountTypeIds("3,6,8", 0);
        return new ArrayList<>();
    }

    /** BLL GenerateCode (@SenderGlId only when ChartOfAccountIdSender != 0 - never, here). */
    public int generateCode() {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, PROC,
                DesktopProc.params("OrganizationId", s.orgId(), "CompanyId", s.companyId(),
                        "FinancialYearId", s.yearId(), "DocumentTypeId", DOC_TYPE, "Activity", "GenerateCode"));
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    /** GridFill() -> GetPendingRecords (UserId = EntryUser = the user). */
    public List<Map<String, Object>> pending(int costCenterId) {
        return DesktopProc.rows(jdbc, PROC,
                DesktopProc.params("OrganizationId", s.orgId(), "CompanyId", s.companyId(),
                        "FinancialYearId", s.yearId(), "UserId", s.userId(), "AppId", s.appId(),
                        "CostCenterId", costCenterId != 0 ? costCenterId : null, "Activity", "GetPendingRecords"));
    }

    /** HistoryCombosFill() -> GetDataForDropDown (no @Activity). */
    public Map<String, Object> historyCombos(int costCenterId) {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "USP_GetDataForDropDownFromAccountsPrematurePaymentsReceipts",
                DesktopProc.params("OrganizationId", s.orgId(), "CompanyId", s.companyId(), "UserId", s.userId(),
                        "AppId", s.appId(), "CostCenterId", costCenterId != 0 ? costCenterId : null));
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> vt = new ArrayList<>(), bank = new ArrayList<>(), snd = new ArrayList<>(), rcv = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            String a = str(r.get("Activity"));
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("Id", r.get("Id")); x.put("Name", r.get("ReferenceName"));
            if ("VoucherType".equals(a)) vt.add(x);
            else if ("SenderBank".equals(a)) bank.add(x);
            else if ("SenderAc".equals(a)) snd.add(x);
            else if ("ReceiverAc".equals(a)) rcv.add(x);
        }
        out.put("voucherTypes", vt); out.put("banks", bank); out.put("senders", snd); out.put("receivers", rcv);
        return out;
    }

    /** HistoryFill() -> FormHistory. */
    public List<Map<String, Object>> history(Map<String, String> q) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", s.orgId());
        p.put("CompanyId", s.companyId());
        p.put("FinancialYearId", s.yearId());
        p.put("UserId", s.userId());
        p.put("AppId", s.appId());
        int snd = toInt(q.get("senderAcId")), rcv = toInt(q.get("receiverAcId")), vt = toInt(q.get("voucherTypeId"));
        if (snd != 0) p.put("SenderAcId", snd);
        if (rcv != 0) p.put("ReceiverAcId", rcv);
        if (vt != 0) p.put("VoucherTypeId", vt);
        p.put("FromDate", nullIfEmpty(q.get("fromDate")));
        p.put("ToDate", nullIfEmpty(q.get("toDate")));
        int fd = toInt(q.get("fromDocNo")), td = toInt(q.get("toDocNo")), bank = toInt(q.get("bankId")), cc = toInt(q.get("costCenterId"));
        if (fd != 0) p.put("FromDocNo", fd);
        if (td != 0) p.put("ToDocNo", td);
        if (bank != 0) p.put("BankId", bank);
        if (cc != 0) p.put("CostCenterId", cc);
        p.put("Activity", "FormHistory");
        return DesktopProc.rows(jdbc, PROC, p);
    }

    /** grdPendingRecord "Edit" -> ReadByMultiParams. */
    public List<Map<String, Object>> readByMultiParams(Map<String, Object> q) {
        return readByTrackingNo(str(q.get("trackingNo")), isoDay(q.get("docDate")), toInt(q.get("voucherTypeId")),
                toInt(q.get("receiverAcId")), toDouble(q.get("slipAmount")), toInt(q.get("costCenterId")), false);
    }

    private List<Map<String, Object>> readByTrackingNo(String tracking, String docDate, int voucherTypeId,
                                                       int receiverAcId, double slipAmount, int costCenterId,
                                                       boolean dalForm) {
        Map<String, Object> p = new LinkedHashMap<>();
        if (dalForm) {
            /* DAL 0567 SetDataForUpdate always sends VoucherTypeId, ReceiverAcId, FromDate, SlipAmount. */
            p.put("VoucherTypeId", voucherTypeId);
            p.put("ReceiverAcId", receiverAcId);
            if (costCenterId != 0) p.put("CostCenterId", costCenterId);
            p.put("FromDate", docDate);
            p.put("TrackingSlipRef", tracking);
            p.put("SlipAmount", slipAmount);
        } else {
            p.put("TrackingSlipRef", tracking);
            p.put("FromDate", docDate);
            if (voucherTypeId != 0) p.put("VoucherTypeId", voucherTypeId);
            if (receiverAcId != 0) p.put("ReceiverAcId", receiverAcId);
            if (slipAmount != 0d) p.put("SlipAmount", slipAmount);
            if (costCenterId != 0) p.put("CostCenterId", costCenterId);
        }
        p.put("Activity", "ReadByTrackingNo");
        return DesktopProc.rows(jdbc, PROC, p);
    }

    // ==================================================================================== save

    /** Insert() (form) -> AccountsPrematurePaymentsReceipts.Save -> DAL SetData. */
    @Transactional
    public Map<String, Object> save(Map<String, Object> body) {
        boolean update = "update".equals(str(body.get("mode")));
        Map<String, Boolean> rights = s.rights(SCREEN);
        if (update && !Boolean.TRUE.equals(rights.get("update"))) throw new SecurityException("You do not have the Update right for this screen.");
        if (!update && !Boolean.TRUE.equals(rights.get("save"))) throw new SecurityException("You do not have the Save right for this screen.");
        List<Map<String, Object>> rows = PartyVoucherService.rows(body.get("rows"));
        if (rows.isEmpty()) throw new IllegalArgumentException("grid record not found");

        boolean booking = bookingOffice(), portal = customerPortal();
        int costCenterId = toInt(body.get("costCenterId"));
        Map<String, Object> first = rows.get(0);
        BigDecimal total = BigDecimal.ZERO;
        double slip = 0d;
        String now = now();
        List<Map<String, Object>> toWrite = new ArrayList<>();
        for (int i = 0; i < rows.size(); i++) {
            Map<String, Object> r = rows.get(i);
            validateRow(r, booking, portal);
            if (!str(r.get("trakingNo")).equals(str(first.get("trakingNo"))))
                throw new IllegalArgumentException("TrackingSlipNo must be Same please check.Row #. " + (i + 1));
            if (toInt(r.get("voucherTypeId")) != toInt(first.get("voucherTypeId")))
                throw new IllegalArgumentException("VoucherType must be same please check.Row #. " + (i + 1));
            if (toInt(r.get("bankId")) != toInt(first.get("bankId")))
                throw new IllegalArgumentException("BankName must be same please check.Row #. " + (i + 1));
            if (toInt(r.get("representativeAcId")) != toInt(first.get("representativeAcId")))
                throw new IllegalArgumentException("RepresentativeAc must be same please check.Row #. " + (i + 1));
            if (toInt(r.get("receiverAcId")) != toInt(first.get("receiverAcId")))
                throw new IllegalArgumentException("ReceiverAccount must be same please check.Row #. " + (i + 1));

            /* Model AccountsPrematureReceipts, non-virtual properties (model 1155). */
            Map<String, Object> d = new LinkedHashMap<>();
            int id = toInt(r.get("id"));
            d.put("IsApproved", Boolean.FALSE);
            d.put("ApprovedDate", now);
            if (id > 0) {
                d.put("DocDate", isoDay(r.get("docDate")));
            } else {
                d.put("DocDate", isoDay(body.get("docDate")));
            }
            d.put("EnteryDate", now);
            d.put("ModifyDate", now);
            d.put("ChequeDate", isoDay(r.get("chequeDate")));
            BigDecimal amount = BigDecimal.valueOf(toDouble(r.get("amount")));
            d.put("Amount", amount);
            double rowSlip = id > 0 ? toDouble(r.get("slipAmount")) : toDouble(body.get("slipAmount"));
            d.put("SlipAmount", rowSlip);
            d.put("ApprovalUserId", s.userId());
            d.put("BankId", toInt(r.get("bankId")));
            d.put("BranchId", 0);
            d.put("ChartOfAccountIdReceiver", toInt(r.get("receiverAcId")));
            d.put("ChartOfAccountIdSender", toInt(r.get("senderAcId")));
            d.put("CompanyId", s.companyId());
            d.put("DocNo", id > 0 ? toInt(r.get("docNo")) : 0);
            d.put("DocumentTypeId", DOC_TYPE);
            d.put("EnteryUserId", s.userId());
            d.put("Id", id);
            d.put("ModifyUserId", s.userId());
            d.put("OrganizationId", s.orgId());
            d.put("ProjectId", 0);
            d.put("RefDocumentTypeId", 0);
            d.put("SupplierCustomerIdSalesMan", toInt(r.get("representativeAcId")));
            d.put("VouchersId", toInt(r.get("voucherTypeId")));
            d.put("SenderPartyId", 0);
            d.put("EntryStatus", str(r.get("status")));
            d.put("RemarksHeader", str(r.get("remarks")));
            d.put("TrackingSlipRef", id > 0 ? str(r.get("trakingNo")) : str(body.get("trackingNo")));
            d.put("ChequeNo", str(r.get("chequeNo")));
            d.put("AttachmentsValues", null);
            d.put("CustomAttachmentsValues", null);
            d.put("FinancialYearId", s.yearId());
            d.put("CostCenterId", costCenterId);
            total = total.add(amount);
            slip = rowSlip;
            toWrite.add(d);
        }
        /* TotalGridAmount != SlipAmount (the last row's). Compared as decimals, not doubles. */
        if (total.compareTo(BigDecimal.valueOf(slip)) != 0) throw new IllegalArgumentException("SlipAmount not equal to Grid Total Amount");

        for (Map<String, Object> d : toWrite) {
            DesktopProc.setProc(jdbc, toInt(d.get("Id")) > 0
                    ? "Sp_AccountsPrematurePaymentsReceipts_Update" : "Sp_AccountsPrematurePaymentsReceipts_Insert", d);
        }
        Map<String, Object> res = new LinkedHashMap<>();
        res.put("success", true);
        res.put("message", update ? "Record Update Successfully" : "Record Save Successfully");
        res.put("docNo", generateCode());
        return res;
    }

    /** formvalidation() - run at row-add time on the desktop. */
    private void validateRow(Map<String, Object> r, boolean booking, boolean portal) {
        String tn = str(r.get("trakingNo")).trim();
        if (tn.isEmpty() || "0".equals(tn)) throw new IllegalArgumentException("Tracking No field required");
        int vt = toInt(r.get("voucherTypeId"));
        if (vt == 0) throw new IllegalArgumentException("Please Select Voucher type");
        if (vt == 5 && toInt(r.get("senderAcId")) == toInt(r.get("receiverAcId")))
            throw new IllegalArgumentException("SenderAccount cannot be equal to ReceiverAccount Please Check!");
        if (toInt(r.get("bankId")) == 0) throw new IllegalArgumentException("Sender Bank Field Required");
        if (toInt(r.get("representativeAcId")) == 0) throw new IllegalArgumentException("Please Select Represented Account");
        if (toInt(r.get("senderAcId")) == 0) throw new IllegalArgumentException("Please Select Sender Account");
        if (!booking && !portal && toInt(r.get("receiverAcId")) == 0) throw new IllegalArgumentException("Please Select Receiver Account");
        if (vt == 4 && str(r.get("chequeNo")).isEmpty()) throw new IllegalArgumentException("ChequeNo Field Required");
        if (toDouble(r.get("amount")) == 0d) throw new IllegalArgumentException("Amount Field is Required");
        if (str(r.get("status")).isEmpty()) throw new IllegalArgumentException("Please Select Status");
    }

    // ======================================================================= confirm / cancel

    /**
     * AccountsPrematurePaymentsReceipts.UpdateRecord -> DAL 0567 SetDataForUpdate, one transaction
     * per call (the bulk button calls it once per checked row, each committing on its own).
     *
     * @param action "Confirm" | "Cancel"
     * @param documentTypeId 159 from the grid buttons, 0 from btnConfirmVoucher
     */
    @Transactional
    public void updateRecord(Map<String, Object> row, String action, int documentTypeId) {
        String tracking = str(row.get("trakingNo"));
        int voucherTypeId = toInt(row.get("vouchersId"));
        int receiverAcId = toInt(row.get("receiverAcId"));
        String docDate = isoDay(row.get("docDate"));
        double slipAmount = toDouble(row.get("slipAmount"));
        int costCenterId = toInt(row.get("costCenterId"));
        int userId = s.userId();

        List<Map<String, Object>> dt = readByTrackingNo(tracking, docDate, voucherTypeId, receiverAcId, slipAmount, costCenterId, true);
        if (dt.isEmpty()) throw new IllegalArgumentException("No data found!");
        int vDocType = toInt(dt.get(0).get("VoucherTypeId"));
        if (distinct(dt, "Status").size() > 1) throw new IllegalArgumentException("EntryStatus column contains multiple different values.");

        if ("CONFIRM".equals(action.toUpperCase())) {
            if (distinct(dt, "VoucherTypeId").size() > 1) throw new IllegalArgumentException("VoucherTypeId column contains multiple different values.");
            if (distinct(dt, "CostCenterId").size() > 1) throw new IllegalArgumentException("Can't Confirm Multiple CostCenters Rows.");
            if ((vDocType == 3 || vDocType == 4) && distinct(dt, "ReceiverAcId").size() > 1)
                throw new IllegalArgumentException("ReceiverAcId must be the same for VoucherTypeId CASH and BANK.");
            double firstSlip = toDouble(dt.get(0).get("SlipAmount"));
            for (Map<String, Object> r : dt) {
                if (toDouble(r.get("SlipAmount")) != firstSlip) throw new IllegalArgumentException("The Slip Amount of All rows are not same...");
            }
            BigDecimal sum = BigDecimal.ZERO;
            for (Map<String, Object> r : dt) sum = sum.add(new BigDecimal(str(r.get("Amount")).isEmpty() ? "0" : str(r.get("Amount"))));

            Map<String, Object> f = dt.get(0);
            ContraVoucherDto.Head h = new ContraVoucherDto.Head();
            h.DocumentTypeId = vDocType;
            h.RefDocNoId = 0;                                   // obj.Id: UpdateRecord's caller never sets it
            h.RefDocumentTypeId = documentTypeId;
            h.VoucherDate = isoDay(f.get("DocDate"));
            h.VoucherCode = toInt(f.get("DocNo"));
            h.RefAccountId = vDocType != 5 ? toInt(f.get("ReceiverAcId")) : 0;
            h.ManualBillNo = str(f.get("TrakingNo"));
            if (dt.size() == 1) {
                h.AgainstAccountId = toInt(f.get("SenderAcId"));
                h.ChequeNo = str(f.get("ChequeNo"));
                h.ChequeDate = isoDay(f.get("ChequeDate"));
            }
            h.Remarks = str(f.get("Remarks"));
            String now = now();
            h.EntryDate = now;
            h.ModifyDate = now;
            h.VoucherAmount = sum.doubleValue();
            h.EntryUser = userId;
            h.ModifyUser = userId;
            h.OrganizationId = s.orgId();
            h.CompanyId = s.companyId();
            h.FinancialYearId = s.yearId();
            h.ActionId = 1;
            String text2 = "";
            if (h.Remarks.isEmpty()) {
                text2 = h.Remarks = (vDocType != 3 ? "Amount Received From " : "Cash Received From ")
                        + str(f.get("SenderAc")) + " In " + str(f.get("ReceiverAc"));
            }
            h.Remarks = text2 + " " + h.Remarks;
            if (vDocType != 3 && vDocType != 4 && vDocType != 5) throw new IllegalArgumentException("DocumentTypeId not found...");

            List<ContraVoucherDto.Detail> details = new ArrayList<>();
            List<ContraVoucherDto.CostCentre> centres = new ArrayList<>();
            int lineId = 1, sortNo = 1;
            for (Map<String, Object> r : dt) {
                int cc = toInt(r.get("CostCenterId"));
                String text4 = (vDocType != 3 ? "Amount Received From " : "Cash Received From ")
                        + str(r.get("SenderAc")) + " In " + str(r.get("ReceiverAc"));
                double amt = toDouble(r.get("Amount"));
                ContraVoucherDto.Detail dr = new ContraVoucherDto.Detail();
                dr.LineId = lineId; dr.SortNo = sortNo;
                dr.AccountId = toInt(r.get("ReceiverAcId")); dr.AgainstAccountId = toInt(r.get("SenderAcId"));
                dr.Comments = text4; dr.CheqNoDetail = str(r.get("ChequeNo")); dr.DCheqDate = isoDay(r.get("ChequeDate"));
                dr.DebitAmount = amt; dr.IsTaxable = "False"; dr.PaymentType = "Regular"; dr.CostCenterId = cc;
                details.add(dr);
                lineId++;
                ContraVoucherDto.Detail cr = new ContraVoucherDto.Detail();
                cr.LineId = lineId; cr.SortNo = sortNo;
                cr.AccountId = toInt(r.get("SenderAcId")); cr.AgainstAccountId = toInt(r.get("ReceiverAcId"));
                cr.Comments = text4; cr.CheqNoDetail = str(r.get("ChequeNo")); cr.DCheqDate = isoDay(r.get("ChequeDate"));
                cr.CreditAmount = amt; cr.IsTaxable = "False"; cr.PaymentType = "Regular"; cr.CostCenterId = cc;
                details.add(cr);
                if (cc > 0) {
                    ContraVoucherDto.CostCentre c = new ContraVoucherDto.CostCentre();
                    c.SortNo = sortNo; c.costPrcent = new BigDecimal("100"); c.costAmount = amt; c.CostCenterId = cc;
                    centres.add(c);
                }
                sortNo++;
            }
            int headId = DesktopProc.setProc(jdbc, "Sp_VoucherHead_Insert", fields(h));
            for (ContraVoucherDto.Detail d : details) {
                d.VoucherHeadId = headId;
                DesktopProc.setProc(jdbc, "Sp_VoucherDetail_Insert", fields(d));   // returned id is not kept (DAL 0567)
            }
            for (ContraVoucherDto.CostCentre c : centres) {
                c.VoucherHeaderId = h.Id == null ? 0 : h.Id;                         // voucherHead.Id: never assigned
                for (ContraVoucherDto.Detail d : details) {
                    if (d.SortNo != null && d.SortNo.equals(c.SortNo)) { c.VoucherDetailId = d.Id; break; }
                }
                DesktopProc.setProc(jdbc, "USP_voucherCostCenterDetail_Insert", fields(c));
            }
        }
        for (Map<String, Object> r : dt) {
            DesktopProc.scalar(jdbc, PROC, DesktopProc.params("OrganizationId", s.orgId(), "CompanyId", s.companyId(),
                    "EntryUserId", userId, "EntryStatus", action, "Id", toInt(r.get("Id")), "Activity", "RecordStatusUpdateById"));
        }
    }

    private static Set<String> distinct(List<Map<String, Object>> dt, String col) {
        Set<String> set = new LinkedHashSet<>();
        for (Map<String, Object> r : dt) set.add(str(r.get(col)));
        return set;
    }
}
