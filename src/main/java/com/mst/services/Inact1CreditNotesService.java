package com.mst.services;

import com.mst.models.dto.ContraVoucherDto;
import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import com.mst.services.desktopvoucher.DesktopVoucherSupport;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.time.LocalDate;
import java.time.format.DateTimeFormatter;
import java.util.*;

import static com.mst.services.desktopvoucher.DesktopVoucherSupport.toDouble;
import static com.mst.services.desktopvoucher.DesktopVoucherSupport.toInt;

/**
 * Screen 27 "Customer Incentive Credit Notes" = Architecture.WinApp.WholeSale.frmCreditNotes (DocumentTypeId 115).
 * Every method names the form / BLL method it reproduces: frmCreditNotes.cs, BLL 0148 WsRmCreditNoteHeader (MakeVoucher, Save,
 * GenerateCode, GetByID, FormHistory), DAL 0115 WsRmCreditNoteHeader.SetData, BLL 0147 CalculateIncentivesForCreditNotes.
 * The form never reaches ReadById (history double click is empty), so GetByID only serves the history "Detail" button.
 */
@Service
public class Inact1CreditNotesService {
    public static final int DOCUMENT_TYPE_ID = 115;
    private static final String HDR = "Sp_WsRmCreditNoteHeader_GetAllMethod";
    private static final String OUT_OF_RANGE = "Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index";
    private static final DateTimeFormatter DMY = DateTimeFormatter.ofPattern("dd-MMM-yyyy", Locale.ENGLISH);

    @Autowired private JdbcTemplate jdbc;
    @Autowired private CurrentUserContext ctx;
    @Autowired private DesktopVoucherSupport support;

    private static Object ci(Map<String, Object> r, String key) {
        if (r.containsKey(key)) return r.get(key);
        for (Map.Entry<String, Object> e : r.entrySet()) if (e.getKey().equalsIgnoreCase(key)) return e.getValue();
        return null;
    }

    private static String day(Object o) {
        if (o == null) return "";
        String s = o.toString();
        try { return LocalDate.parse(s.length() > 10 ? s.substring(0, 10) : s).format(DMY); } catch (Exception e) { return s; }
    }

    private static String iso(Object o, String field) {
        if (o == null || o.toString().isBlank()) throw new IllegalArgumentException(field + " Field is Required");
        String s = o.toString().trim();
        s = s.length() > 10 ? s.substring(0, 10) : s;
        if (s.matches("\\d{2}-[A-Za-z]{3}-\\d{4}")) return LocalDate.parse(s, DMY).toString();
        return LocalDate.parse(s).toString();
    }

    /** FrmExportSalesContract_Load: GenerateCode + DebitAccountFill (CoaAllocationAccountTitleByAccountTypeIds("11,13,20,21")). */
    public Map<String, Object> load() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("docNo", generateCode());
        m.put("debitAccounts", debitAccounts());
        return m;
    }

    /** DebitAccountFill (also btnRefresh_Click, which no toolbar button reaches). */
    public List<Map<String, Object>> debitAccounts() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : support.coaAccountTitleByAccountTypeIds("11,13,20,21", 0)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ci(r, "Id"));
            o.put("AccountTitle", ci(r, "AccountTitle"));
            out.add(o);
        }
        return out;
    }

    /** GenerateCode: @OrganizationId, @CompanyId, @DocumentTypeId = 115, @Activity = 'GenerateCode'; 0 leaves the box as it was. */
    public int generateCode() {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, HDR, DesktopProc.params("OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(), "DocumentTypeId", DOCUMENT_TYPE_ID, "Activity", "GenerateCode"));
        return rows.isEmpty() ? 0 : toInt(ci(rows.get(0), "DocNo"));
    }

    /** btnShowDetail_Click: USP_CalculateIncentivesForCreditNotes (@OrganizationId, @CompanyId, @FromDate, @ToDate); the 24 grid cells per row. */
    public List<Map<String, Object>> showDetail(Object from, Object to) {
        String f = iso(from, "From Date"), t = iso(to, "To Date");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_CalculateIncentivesForCreditNotes", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(), "FromDate", f, "ToDate", t))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("PolicyId", ci(r, "PolicyId"));
            o.put("SupplierCustomerId", ci(r, "GlAccountId"));
            o.put("SupplierCustomer", ci(r, "CustomerName"));
            o.put("ItemId", ci(r, "ItemId"));
            o.put("ItemName", ci(r, "ItemName"));
            o.put("TargetBaseTypeId", ci(r, "TargetBaseTypeId"));
            o.put("TargetBaseType", ci(r, "IncentiveBaseType"));
            o.put("TargetFrom", day(ci(r, "SchemeDateFrom")));
            o.put("TargetTo", day(ci(r, "SchemeDateTo")));
            o.put("TargetRangeFrom", ci(r, "TargetRangeFrom"));
            o.put("TargetRangeTo", ci(r, "TargetRangeTo"));
            o.put("IncentiveTypeId", ci(r, "IncentiveTypeId"));
            o.put("IncentiveType", ci(r, "IncentiveTypeName"));
            o.put("TotalInvoices", ci(r, "TotalInvoices"));
            o.put("TotalQty", ci(r, "TotalQty"));
            o.put("TotalWeight", ci(r, "TotalBillWeight"));
            o.put("TotalItemAmount", ci(r, "TotalItemAmount"));
            o.put("TotalNetAmount", ci(r, "TotalNetAmount"));
            o.put("AvgRate", ci(r, "AvgRate"));
            o.put("IncentiveRate", ci(r, "IncentiveRate"));
            o.put("IncentiveCalcOnId", ci(r, "IncentiveCalcTypeId"));
            o.put("IncentiveCalcOn", ci(r, "IncentiveCalType"));
            o.put("IncentiveAmount", ci(r, "TotalIncentive"));
            o.put("RemarksDetail", "");
            out.add(o);
        }
        return out;
    }

    /** HistoryFill: FormHistory (NoOfRecords 21, CanViewAllRecord false, EntryUser = UserAccount.ID). */
    public List<Map<String, Object>> history() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, HDR, DesktopProc.params("OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(), "DocumentTypeId", DOCUMENT_TYPE_ID, "NoOfRecords", 21, "CanViewAllRecord", false,
                "EntryUser", ctx.currentUserId(), "Activity", "FormHistory"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ci(r, "Id"));
            o.put("DocDate", day(ci(r, "DocDate")));
            o.put("DocNo", ci(r, "DocNo"));
            o.put("PeriodFrom", day(ci(r, "PeriodFrom")));
            o.put("PeriodTo", day(ci(r, "PeriodTo")));
            o.put("DebitAccount", ci(r, "AccountTitle"));
            o.put("IncentiveAmount", ci(r, "IncentiveAmount"));
            o.put("Remarks", ci(r, "RemarksHeader"));
            o.put("EntryUser", ci(r, "UserName"));
            out.add(o);
        }
        return out;
    }

    /** DataGridHistory_ColumnButtonClick ("Detail"): GetByID -> ReadById + ReadDetailByHeaderId; the ten columns of grdHistoryDetail. */
    public List<Map<String, Object>> historyDetail(int id) {
        List<Map<String, Object>> h = DesktopProc.rows(jdbc, HDR, DesktopProc.params("Activity", "ReadById", "Id", id));
        if (h.isEmpty()) throw new IllegalStateException(OUT_OF_RANGE);
        Map<String, Object> hr = h.get(0);
        if (toInt(ci(hr, "OrganizationId")) != ctx.currentOrganizationId() || toInt(ci(hr, "CompanyId")) != ctx.currentCompanyId()
                || toInt(ci(hr, "DocumentTypeId")) != DOCUMENT_TYPE_ID)
            throw new IllegalStateException(OUT_OF_RANGE);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, HDR, DesktopProc.params("Id", id, "Activity", "ReadDetailByHeaderId"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("SupplierCustomer", ci(r, "CompanyName"));
            o.put("TargetBaseType", ci(r, "TargetType"));
            o.put("TargetFrom", day(ci(r, "TargetFrom")));
            o.put("TargetTo", day(ci(r, "TargetTo")));
            o.put("TargetAchieved", ci(r, "TargetAcheived"));
            o.put("IncentiveType", ci(r, "IncentiveType"));
            o.put("IncentiveRate", ci(r, "InCentiveRate"));
            o.put("IncentiveCalcOn", ci(r, "IncentiveCalculationOn"));
            o.put("IncentiveAmount", ci(r, "IncentiveAmount"));
            o.put("RemarksDetail", ci(r, "RemarksDetail"));
            out.add(o);
        }
        return out;
    }

    private boolean ownsAccount(int id) {
        Integer n = jdbc.queryForObject("SELECT COUNT(1) FROM dbo.ChartofAccount WHERE Id = ? AND OrganizationId = ? AND CompanyId = ?",
                Integer.class, id, ctx.currentOrganizationId(), ctx.currentCompanyId());
        return n != null && n > 0;
    }

    /**
     * btnsave_Click: FormValidation (DocNo Required / Debit Account Required), the per-row checks in the form's order, then
     * WsRmCreditNoteHeader.Save = MakeVoucher + DAL SetData (one transaction): Sp_WsRmCreditNoteHeader_Insert|Update, Sp_WsRmCreditNoteDetail_Insert per row,
     * Sp_Vouchers_GetMethods GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId, Sp_VoucherHead_Insert|Update, Sp_VoucherDetail_Insert,
     * Sp_VoucherHead_H_Insert, Sp_VoucherDetail_H_Insert. The form's Update path (RecId > 0) is unreachable (ReadById is never called), so recId is not accepted.
     */
    @Transactional
    @SuppressWarnings("unchecked")
    public String save(Map<String, Object> b) {
        String docNoText = b.get("docNo") == null ? "" : b.get("docNo").toString();
        if (docNoText.isEmpty()) throw new IllegalArgumentException("DocNo Required");
        int debit = toInt(b.get("debitAccountId"));
        if (debit == 0) throw new IllegalArgumentException("Debit Account Required");
        int org = ctx.currentOrganizationId(), company = ctx.currentCompanyId(), user = ctx.currentUserId();
        String docDate = iso(b.get("docDate"), "Doc Date"), from = iso(b.get("fromDate"), "From Date"), to = iso(b.get("toDate"), "To Date");
        List<Map<String, Object>> rows = (List<Map<String, Object>>) b.get("rows");
        if (rows == null || rows.isEmpty()) throw new IllegalStateException("Grid record not found");
        if (!ownsAccount(debit)) throw new IllegalArgumentException("Debit Account Required");
        String now = DesktopVoucherSupport.now();
        int docNo = toInt(docNoText);
        String remarks = b.get("remarks") == null ? "" : b.get("remarks").toString();

        List<Map<String, Object>> details = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> d = new LinkedHashMap<>();
            if (toInt(r.get("TargetBaseTypeId")) == 0) throw new IllegalArgumentException("Target Type Required");
            d.put("TargetTypeId", toInt(r.get("TargetBaseTypeId")));
            d.put("TargetFrom", iso(r.get("TargetFrom"), "Target From"));
            d.put("TargetTo", iso(r.get("TargetTo"), "Target To"));
            if (toInt(r.get("IncentiveTypeId")) == 0) throw new IllegalArgumentException("Incentive Type Required");
            d.put("IncentiveTypeId", toInt(r.get("IncentiveTypeId")));
            if (toDouble(r.get("IncentiveRate")) == 0.0) throw new IllegalArgumentException("Incentive Rate Required");
            d.put("InCentiveRate", toDouble(r.get("IncentiveRate")));
            if (toInt(r.get("IncentiveCalcOnId")) == 0) throw new IllegalArgumentException("Incentive Calculation On Required");
            d.put("CalcOn", toInt(r.get("IncentiveCalcOnId")));
            if (toDouble(r.get("IncentiveAmount")) == 0.0) throw new IllegalArgumentException("Incentive Amount Required");
            d.put("IncentiveAmount", toDouble(r.get("IncentiveAmount")));
            if (toInt(r.get("SupplierCustomerId")) == 0) throw new IllegalArgumentException("Customer Required");
            d.put("CreditAccountId", toInt(r.get("SupplierCustomerId")));
            if (toInt(r.get("PolicyId")) == 0) throw new IllegalArgumentException("Customer Required");
            d.put("PolicyId", toInt(r.get("PolicyId")));
            d.put("RemarksDetail", r.get("RemarksDetail") == null ? "" : r.get("RemarksDetail").toString());
            if (!ownsAccount((Integer) d.get("CreditAccountId"))) throw new IllegalArgumentException("Customer Required");
            details.add(d);
        }

        /* MakeVoucher: CommonServies.GetActiveFinancialYear */
        int yearId = ctx.currentFinancialYearId();
        if (yearId <= 0) throw new IllegalStateException("Financial Year Not Found Please Check");
        ContraVoucherDto.Head vh = new ContraVoucherDto.Head();
        vh.DocumentTypeId = DOCUMENT_TYPE_ID;
        vh.VoucherCode = docNo;
        vh.VoucherDate = docDate;
        vh.Remarks = remarks;
        vh.RemarksOtherLingo = "";
        vh.ChequeDate = LocalDate.now().toString();
        vh.IncludeWHT = Boolean.FALSE;
        vh.BranchId = 0;
        vh.ProjectId = 0;
        vh.DueDate = now;
        vh.OrganizationId = org;
        vh.CompanyId = company;
        vh.FinancialYearId = yearId;
        vh.EntryUser = user;
        vh.EntryDate = now;
        vh.ModifyDate = now;
        vh.ModifyUser = user;
        List<ContraVoucherDto.Detail> vds = new ArrayList<>();
        for (Map<String, Object> d : details) {
            ContraVoucherDto.Detail dr = new ContraVoucherDto.Detail();
            dr.AccountId = debit;
            dr.AgainstAccountId = (Integer) d.get("CreditAccountId");
            dr.Comments = (String) d.get("RemarksDetail");
            dr.DebitAmount = (Double) d.get("IncentiveAmount");
            vds.add(dr);
            ContraVoucherDto.Detail cr = new ContraVoucherDto.Detail();
            cr.AccountId = (Integer) d.get("CreditAccountId");
            cr.AgainstAccountId = debit;
            cr.Comments = (String) d.get("RemarksDetail");
            cr.CreditAmount = (Double) d.get("IncentiveAmount");
            vds.add(cr);
        }

        /* DAL SetData */
        Map<String, Object> hp = DesktopProc.params("Id", 0L, "DocumentTypeId", DOCUMENT_TYPE_ID, "DocDate", docDate, "DocNo", docNo,
                "RemarksHeader", remarks, "DebitAccountId", (long) debit, "PeriodFrom", from, "PeriodTo", to,
                "OrganizationId", org, "CompanyId", company, "BranchesId", 0, "EntryUserId", user, "EntryDate", now,
                "ModifyUserId", user, "ModifyDate", now, "IsApproved", false, "ApprovedUserId", user, "ApprovedDate", now);
        int id = DesktopProc.setProc(jdbc, "Sp_WsRmCreditNoteHeader_Insert", hp);
        for (Map<String, Object> d : details) {
            DesktopProc.setProc(jdbc, "Sp_WsRmCreditNoteDetail_Insert", DesktopProc.params("Id", 0L, "WsRmCreditNoteHeaderId", (long) id,
                    "CreditAccountId", d.get("CreditAccountId"), "RemarksDetail", d.get("RemarksDetail"), "TargetTypeId", d.get("TargetTypeId"),
                    "TargetFrom", d.get("TargetFrom"), "TargetTo", d.get("TargetTo"), "TargetAcheived", 0d,
                    "IncentiveTypeId", d.get("IncentiveTypeId"), "InCentiveRate", d.get("InCentiveRate"), "CalcOn", d.get("CalcOn"),
                    "IncentiveAmount", d.get("IncentiveAmount"), "PolicyId", d.get("PolicyId")));
        }
        List<Map<String, Object>> existing = DesktopProc.rows(jdbc, "Sp_Vouchers_GetMethods", DesktopProc.params(
                "Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId", "OrganizationId", org, "CompanyId", company,
                "DocumentTypeId", DOCUMENT_TYPE_ID, "DocumentTypeSrNo", id));
        int vhid = 0;
        if (!existing.isEmpty()) { vhid = toInt(ci(existing.get(0), "Id")); vh.Id = vhid; }
        vh.DocumentTypeSrNo = id;
        vh.RefDocNoId = id;
        int num3 = DesktopProc.setProc(jdbc, vhid == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", DesktopVoucherSupport.fields(vh));
        if (num3 > 0) vh.Id = num3;
        for (ContraVoucherDto.Detail d : vds) {
            d.VoucherHeadId = vh.Id;
            DesktopProc.setProc(jdbc, "Sp_VoucherDetail_Insert", DesktopVoucherSupport.fields(d));
        }
        if (vds.isEmpty()) throw new IllegalStateException("Voucher Detail List Not Found");
        int ref = DesktopProc.setProc(jdbc, "Sp_VoucherHead_H_Insert", DesktopVoucherSupport.fields(vh));
        for (ContraVoucherDto.Detail d : vds) {
            d.VoucherHeadId = vh.Id;
            d.DocumentTypeIdRef = ref;
            DesktopProc.setProc(jdbc, "Sp_VoucherDetail_H_Insert", DesktopVoucherSupport.fields(d));
        }
        return "Receod Save Successfully";
    }
}
