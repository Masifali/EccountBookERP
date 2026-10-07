package com.mst.services;

import java.lang.reflect.Field;
import java.lang.reflect.Modifier;
import java.math.BigDecimal;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.Base64;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.Set;

import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.PreparedStatementCallback;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.dto.ContraVoucherDto;
import com.mst.repositories.AccountsGroupDSupport;
import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;

/**
 * Screen 45 "Pdc Receipts" - Architecture.WinApp.Account_Definition.AcfrmDefPdcManagment (form Name "AcfrmDefPdcManagment",
 * DocumentTypeId 16), call for call. BLL/DAL read from recovered_source/projects: BLL 0649 PdcInventory, DAL 0580 PdcInventory,
 * models 1183 / 1184, BLL 0075 PdcBankName, BLL 0140 GeneralReprots, GenericProvider.SetProc (every non-virtual model property is
 * a parameter, a CLR null is omitted).
 *
 * <ul>
 * <li>bankfill: PdcBankName.Getall -> Sp_PdcBankName_GetAllMethod @OrganizationId @CompanyId @Activity='ReadAll' (Id, BankName).</li>
 * <li>AccountsFill / CreditAccountFill: CommonServices.CoaAllocationGetAllServiceBind -> Sp_COAAllocation_GetAllMethod
 *     @OrganizationId @CompanyId @UserId(when != 0) @Activity='COAAllocationSearch', AccountTypeId not in (2, 11, 15).</li>
 * <li>DocumentNoFill: Sp_PdcInventory_GetAllMethod 'GenerateCode' (@DocumentTypeId 16, @FinancialYearId when != 0) -> DocNo of row 0, used when &gt; 0.</li>
 * <li>Save (BLL.Save / DAL.SetDate, one transaction): Sp_PdcInventoryHeader_Insert / _Update, SP_PdcInventory_Insert per grid row,
 *     the voucher (Sp_VoucherHead_Insert/_Update + Sp_VoucherDetail_Insert; the desktop DAL does not call the balance check, the _H
 *     mirrors or the approval procedure, so neither does this port), attachments (DeleteById + Proc_DMSAttachments_Insert).</li>
 * <li>ReadById: 'ReadByIdHeader' + 'ReadByID'. History: 'FormHistory'. Doc lookup: 'GetIdByDocNo'. Row delete: 'DeleteByPdcInventoryId'.
 *     Delete: InvPurchaseInvoice.RemoveByID -> Sp_InvoicesVouchersandStocksDelete (DocumentTypeId 16).</li>
 * <li>Reports: Sp_PdcInventory_SlipAndRegister_Rpt (125 / 127), Sp_PdcInventory_ReceiptsSummery_Rpt (126), Sp_PdcInventoryHeader_rpt (123).</li>
 * </ul>
 */
@Service
public class PdcReceiptDesktopService {

    public static final String SCREEN_NAME = "AcfrmDefPdcManagment";
    public static final int DOCUMENT_TYPE_ID = 16;

    private static final DateTimeFormatter DT = DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss", Locale.ENGLISH);

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;
    private final AccountsGroupDSupport s;
    private final DesktopAttachmentStore store;

    public PdcReceiptDesktopService(JdbcTemplate jdbc, CurrentUserContext ctx, AccountsGroupDSupport s, DesktopAttachmentStore store) {
        this.jdbc = jdbc;
        this.ctx = ctx;
        this.s = s;
        this.store = store;
    }

    /** A desktop validation or database message, shown as a plain MessageBox text. */
    public static class Refusal extends RuntimeException {
        private static final long serialVersionUID = 1L;
        public Refusal(String m) { super(m); }
    }

    // ------------------------------------------------------------------------------------------------- DDL

    /** bankfill. */
    public List<Map<String, Object>> banks() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_PdcBankName_GetAllMethod", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(), "Activity", "ReadAll"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", r.get("Id"));
            o.put("BankName", r.get("BankName"));
            out.add(o);
        }
        return out;
    }

    /** AccountsFill + CreditAccountFill: the COA allocation list without cash (2), 11 and bank (15) accounts. */
    public List<Map<String, Object>> accounts() {
        Map<String, Object> p = DesktopProc.params("OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId());
        int uid = ctx.currentUserId();
        if (uid != 0) p.put("UserId", uid);
        p.put("Activity", "COAAllocationSearch");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_COAAllocation_GetAllMethod", p)) {
            int t = netInt(r.get("AccountTypeId"));
            if (t == 2 || t == 11 || t == 15) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", r.get("Id"));
            o.put("AccountTitle", r.get("AccountTitle"));
            out.add(o);
        }
        return out;
    }

    /** DocumentNoFill / GetGenerateCode: 0 when the procedure returns no row (the page then keeps the box as it is). */
    public int generateCode() {
        Map<String, Object> p = DesktopProc.params("OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                "DocumentTypeId", DOCUMENT_TYPE_ID);
        int fy = ctx.currentFinancialYearId();
        if (fy != 0) p.put("FinancialYearId", fy);
        p.put("Activity", "GenerateCode");
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_PdcInventory_GetAllMethod", p);
        return rows.isEmpty() ? 0 : netInt(rows.get(0).get("DocNo"));
    }

    /** AcfrmDefPdcManagment_Load / toolStripButton1_Click (Refresh): the fills plus the user's rights. */
    public Map<String, Object> load() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("banks", banks());
        out.put("accounts", accounts());
        out.put("docNo", generateCode());
        out.put("rights", s.rights(SCREEN_NAME));
        return out;
    }

    // ------------------------------------------------------------------------------------------------- read

    /** GetByID(Id)[0]: the header must belong to the signed-in company and be a PDC receipt (the desktop reaches ids only through its own history). */
    private Map<String, Object> header(int id) {
        List<Map<String, Object>> own = jdbc.queryForList(
                "SELECT Id FROM dbo.PdcInventoryHeader WHERE Id = ? AND OrganizationId = ? AND CompanyId = ? AND DocumentTypeId = ?",
                id, ctx.currentOrganizationId(), ctx.currentCompanyId(), DOCUMENT_TYPE_ID);
        if (own.isEmpty()) throw new Refusal("Index was out of range. Must be non-negative and less than the size of the collection.");
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_PdcInventory_GetAllMethod", DesktopProc.params("Id", id, "Activity", "ReadByIdHeader"));
        if (rows.isEmpty()) throw new Refusal("Index was out of range. Must be non-negative and less than the size of the collection.");
        return rows.get(0);
    }

    /** DAL.GetDate: the child rows 'ReadByID'. */
    private List<Map<String, Object>> children(int id) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : DesktopProc.rows(jdbc, "Sp_PdcInventory_GetAllMethod", DesktopProc.params("Id", id, "Activity", "ReadByID"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (String k : new String[] { "Id", "BankId", "BankName", "CheqNo", "CheqAmount", "CheqDate", "Remarks", "CheqStatus" }) o.put(k, norm(d.get(k)));
            out.add(o);
        }
        return out;
    }

    /** CommonServices.VoucherHeadIdGet(id, 16). */
    public int voucherHeadId(int id) {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_Vouchers_GetMethods", DesktopProc.params(
                "Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId",
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                "DocumentTypeId", DOCUMENT_TYPE_ID, "DocumentTypeSrNo", id));
        return rows.isEmpty() ? 0 : netInt(rows.get(0).get("Id"));
    }

    /** DMSAttachments.GetByID(id, base.Name). */
    public List<Map<String, Object>> attachments(int id) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> a : DesktopProc.rows(jdbc, "Sp_DMSAttachments_GetAllMethod", DesktopProc.params(
                "ScreenName", SCREEN_NAME, "Id", id, "Activity", "ReadById"))) {
            if (netInt(a.get("OrganizationId")) != ctx.currentOrganizationId() || netInt(a.get("CompanyId")) != ctx.currentCompanyId()) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", a.get("Id"));
            o.put("Attachment", a.get("Attachment"));
            o.put("UploadedFileCustomName", a.get("UploadedFileCustomName"));
            o.put("UploadedFileSizeMb", norm(a.get("UploadedFileSizeMb")));
            o.put("EntryDate", norm(a.get("EntryDate")));
            o.put("EntryUserName", a.get("EntryUserName"));
            out.add(o);
        }
        return out;
    }

    /** ReadById: header, grid rows, voucher head id, attachments. */
    public Map<String, Object> getById(int id) {
        Map<String, Object> h = header(id);
        Map<String, Object> head = new LinkedHashMap<>();
        for (String k : new String[] { "Id", "DocDate", "DocNo", "MGLAccountCrId", "MGLAccountDrId", "RemarkHeader" }) head.put(k, norm(h.get(k)));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", head);
        out.put("details", children(id));
        out.put("voucherHeadId", voucherHeadId(id));
        out.put("attachments", attachments(id));
        return out;
    }

    /** GetIdByDocNo (txtdocno leave / Update button): 0 when there is none. */
    public int idByDocNo(int docNo) {
        Map<String, Object> p = DesktopProc.params("OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                "DocumentTypeId", DOCUMENT_TYPE_ID, "DocNo", docNo);
        int fy = ctx.currentFinancialYearId();
        if (fy != 0) p.put("FinancialYearId", fy);
        p.put("Activity", "GetIdByDocNo");
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_PdcInventory_GetAllMethod", p);
        return rows.isEmpty() ? 0 : netInt(rows.get(0).get("Id"));
    }

    /** HistoryFill(NoOfRecords): FormHistory; the user's own records only without the "CanView AllRecord" right. */
    public List<Map<String, Object>> history(int noOfRecords) {
        boolean all = Boolean.TRUE.equals(s.rights(SCREEN_NAME).get("canViewAll"));
        Map<String, Object> p = DesktopProc.params("OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                "DocumentTypeId", DOCUMENT_TYPE_ID, "CanViewAllRecord", all);
        int fy = ctx.currentFinancialYearId();
        if (fy != 0) p.put("FinancialYearId", fy);
        if (!all) p.put("EntryUser", ctx.currentUserId());
        if (noOfRecords != 0) p.put("NoOfRecords", noOfRecords);
        p.put("Activity", "FormHistory");
        return gridRows("Sp_PdcInventory_GetAllMethod", p);
    }

    /**
     * The first result set as a DataTable shows it to the grid: columns in the procedure's own order (DesktopProc.rows sorts them by name),
     * dates / decimals made JSON friendly, binary columns (the company logo of the register) left out.
     */
    private List<Map<String, Object>> gridRows(String proc, Map<String, Object> params) {
        List<Object> args = new ArrayList<>();
        StringBuilder sb = new StringBuilder("EXEC dbo.").append(proc);
        boolean first = true;
        for (Map.Entry<String, Object> e : params.entrySet()) {
            if (e.getValue() == null) continue;
            sb.append(first ? " " : ", ").append('@').append(e.getKey()).append("=?");
            args.add(e.getValue());
            first = false;
        }
        return jdbc.execute(sb.toString(), (PreparedStatementCallback<List<Map<String, Object>>>) ps -> {
            for (int i = 0; i < args.size(); i++) ps.setObject(i + 1, args.get(i));
            List<Map<String, Object>> out = new ArrayList<>();
            boolean taken = false;
            boolean isRs = ps.execute();
            while (true) {
                if (isRs) {
                    try (ResultSet rs = ps.getResultSet()) {
                        if (!taken) {
                            ResultSetMetaData md = rs.getMetaData();
                            int n = md.getColumnCount();
                            while (rs.next()) {
                                Map<String, Object> row = new LinkedHashMap<>();
                                for (int c = 1; c <= n; c++) {
                                    String label = md.getColumnLabel(c);
                                    if (label == null || label.isEmpty()) label = "Column" + c;
                                    Object v = rs.getObject(c);
                                    if (v instanceof byte[]) continue;
                                    if (!row.containsKey(label)) row.put(label, norm(v));
                                }
                                out.add(row);
                            }
                            taken = true;
                        } else {
                            while (rs.next()) { /* drain */ }
                        }
                    }
                } else if (ps.getUpdateCount() == -1) {
                    break;
                }
                isRs = ps.getMoreResults();
            }
            return out;
        });
    }

    // ------------------------------------------------------------------------------------------------- save

    public static class Upload {
        public String name;
        public String base64;
    }

    public static class SaveRequest {
        public int id;
        public String docDate;
        public String docNo;
        public String remarks;
        public Object creditId;
        public Object debitId;
        public List<Map<String, Object>> rows;
        public List<Integer> keepAttachments;
        public List<Upload> addAttachments;
    }

    /**
     * Insert() after the confirm. formvalidation and "grid record not found" are repeated with the desktop's texts.
     * Returns {id, docNo}; id is the header id the DAL returns (it drives the slip / voucher previews).
     */
    @Transactional
    public Map<String, Object> save(SaveRequest r) {
        int org = ctx.currentOrganizationId(), company = ctx.currentCompanyId(), user = ctx.currentUserId();
        int fy = ctx.currentFinancialYearId();
        String docNoText = r.docNo == null ? "" : r.docNo.trim();
        if (docNoText.isEmpty() || docNoText.equals("0")) throw new Refusal("Doc No field required");
        if (r.creditId == null || String.valueOf(r.creditId).trim().isEmpty()) throw new Refusal("Please Select Party Account");
        if (r.debitId == null || String.valueOf(r.debitId).trim().isEmpty()) throw new Refusal("Please Select Debit Account");
        if (r.rows == null || r.rows.isEmpty()) throw new Refusal("grid record not found");
        if (r.id < 0) throw new Refusal("Invalid record");
        s.requireRight(SCREEN_NAME, r.id > 0 ? "canUpdate" : "canSave", "You do not have the right to " + (r.id > 0 ? "update" : "save") + " this record");

        int cr = netInt(r.creditId), dr = netInt(r.debitId);
        Set<Integer> accountIds = new HashSet<>();
        for (Map<String, Object> a : accounts()) accountIds.add(netInt(a.get("Id")));
        if (!accountIds.contains(cr)) throw new Refusal("Please Select Party Account");
        if (!accountIds.contains(dr)) throw new Refusal("Please Select Debit Account");
        Map<Integer, String> bankNames = new LinkedHashMap<>();
        for (Map<String, Object> b : banks()) bankNames.put(netInt(b.get("Id")), str(b.get("BankName")));

        int recId = r.id;
        if (recId > 0) header(recId);                                      // belongs to the signed-in company

        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        Timestamp docDate = ts(r.docDate);
        int docNo = netInt(docNoText);
        int num = DesktopProc.setProc(jdbc, recId == 0 ? "Sp_PdcInventoryHeader_Insert" : "Sp_PdcInventoryHeader_Update", DesktopProc.params(
                "Id", recId,
                "DocDate", docDate,
                "DocNo", docNo,
                "DocumentTypeId", DOCUMENT_TYPE_ID,
                "MGLAccountCrId", cr,
                "MGLAccountDrId", dr,
                "RemarkHeader", r.remarks == null ? "" : r.remarks,
                "EntryDate", now,
                "EntryUser", user,
                "ModifyDate", now,
                "ModifyUser", user,
                "IsApproved", Boolean.FALSE,
                "ApprovedUserId", 0,
                "OrganizationId", org,
                "CompanyId", company,
                "BranchId", 0,
                "ProjectId", 0,
                "FinancialYearId", fy));
        int hid = num > 0 ? num : recId;                                  // DAL: num3 > 0 ? obj.Id = num3 : num3 = obj.Id

        /* the grid rows (PdcInventory objects) in grid order */
        List<Map<String, Object>> items = new ArrayList<>();
        for (Map<String, Object> row : r.rows) {
            int bankId = netInt(row.get("BankId"));
            if (!bankNames.containsKey(bankId)) throw new Refusal("Bank Field Required");
            Map<String, Object> it = new LinkedHashMap<>();
            it.put("BankId", bankId);
            it.put("BankName", bankNames.get(bankId));
            it.put("CheqNo", str(row.get("CheqNo")).trim());
            it.put("CheqAmount", netDouble(row.get("CheqAmount")));
            it.put("CheqDate", ts(str(row.get("CheqDate"))));
            it.put("CheqStatus", str(row.get("ChequeStatus")));
            it.put("Remarks", str(row.get("Comments")));
            items.add(it);
        }
        for (Map<String, Object> it : items) {
            DesktopProc.setProc(jdbc, "SP_PdcInventory_Insert", DesktopProc.params(
                    "Id", 0,
                    "DocDate", docDate,
                    "PdcInventoryHeaderId", hid,
                    "DocNo", docNo,
                    "DocumentTypeId", DOCUMENT_TYPE_ID,
                    "BankId", it.get("BankId"),
                    "CheqNo", it.get("CheqNo"),
                    "CheqId", 0,
                    "CheqAmount", it.get("CheqAmount"),
                    "CheqDate", it.get("CheqDate"),
                    "CheqStatus", it.get("CheqStatus"),
                    "Remarks", it.get("Remarks"),
                    "GlCreditAcId", cr,
                    "GlDebitAcId", dr,
                    "GlVoucherRefId", 0,
                    "EntryDate", now,
                    "EntryUser", user,
                    "ModifyDate", now,
                    "ModifyUser", user,
                    "IsApproved", Boolean.FALSE,
                    "ApprovedUserId", 0,
                    "OrganizationId", org,
                    "CompanyId", company,
                    "BranchId", 0,
                    "ProjectId", 0,
                    "ActionId", 0));
        }

        saveVoucher(hid, docDate, docNo, cr, dr, items, now, org, company, user, fy);
        saveAttachments(r, hid, cr, org, company, user, now);

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", hid);
        out.put("docNo", docNo);
        return out;
    }

    /** DAL.SetDate: the receipt voucher (VoucherAmount stays 0, as in the desktop). */
    private void saveVoucher(int hid, Timestamp docDate, int docNo, int cr, int dr, List<Map<String, Object>> items, Timestamp now,
                             int org, int company, int user, int fy) {
        int existing = voucherHeadId(hid);
        ContraVoucherDto.Head head = new ContraVoucherDto.Head();
        head.Id = existing;
        head.DocumentTypeSrNo = hid;
        head.VoucherDate = docDate.toLocalDateTime().format(DT);
        head.VoucherCode = docNo;
        head.DocumentTypeId = DOCUMENT_TYPE_ID;
        head.RefAccountId = cr;
        head.AgainstAccountId = dr;
        head.CheqId = hid;
        head.VoucherAmount = 0d;
        head.EntryDate = now.toLocalDateTime().format(DT);
        head.ModifyDate = now.toLocalDateTime().format(DT);
        head.EntryUser = user;
        head.ModifyUser = user;
        head.OrganizationId = org;
        head.CompanyId = company;
        head.FinancialYearId = fy;

        int cashAc = 0;
        String cfg = s.config("CashInHandAc");
        if (!cfg.isEmpty()) cashAc = netInt(cfg);
        String title = "";
        List<Map<String, Object>> coa = DesktopProc.rows(jdbc, "Sp_ChartofAccount_GetAllMethodFromCOA", DesktopProc.params(
                "OrganizationId", org, "CompanyId", company, "Id", cr, "CoaType", "GetCompleteCoaDetailByAccountId"));
        if (!coa.isEmpty()) title = str(coa.get(0).get("AccountTitle"));

        List<ContraVoucherDto.Detail> details = new ArrayList<>();
        for (Map<String, Object> it : items) {
            String bank = str(it.get("BankName"));
            String comment = "Cheque # '" + it.get("CheqNo") + "' Received From '" + title + "' To Bank '" + bank + "' " + it.get("Remarks");
            double amt = netDouble(it.get("CheqAmount"));
            String cheqDate = ((Timestamp) it.get("CheqDate")).toLocalDateTime().format(DT);
            if (cashAc > 0 && "CASH".equals(bank)) {
                ContraVoucherDto.Detail d1 = new ContraVoucherDto.Detail();
                d1.AccountId = dr; d1.AgainstAccountId = cashAc; d1.Comments = comment; d1.CreditAmount = amt;
                details.add(d1);
                ContraVoucherDto.Detail d2 = new ContraVoucherDto.Detail();
                d2.AccountId = cashAc; d2.AgainstAccountId = dr; d2.Comments = comment; d2.DebitAmount = amt;
                details.add(d2);
            }
            ContraVoucherDto.Detail d3 = new ContraVoucherDto.Detail();
            d3.AccountId = cr; d3.AgainstAccountId = dr; d3.Comments = comment; d3.CheqNoDetail = str(it.get("CheqNo")); d3.DCheqDate = cheqDate; d3.CreditAmount = amt;
            details.add(d3);
            ContraVoucherDto.Detail d4 = new ContraVoucherDto.Detail();
            d4.AccountId = dr; d4.AgainstAccountId = cr; d4.Comments = comment; d4.CheqNoDetail = str(it.get("CheqNo")); d4.DCheqDate = cheqDate; d4.DebitAmount = amt;
            details.add(d4);
        }
        int num = DesktopProc.setProc(jdbc, existing != 0 ? "Sp_VoucherHead_Update" : "Sp_VoucherHead_Insert", fields(head));
        int vid = num > 0 ? num : head.Id;
        for (ContraVoucherDto.Detail d : details) {
            d.VoucherHeadId = vid;
            DesktopProc.setProc(jdbc, "Sp_VoucherDetail_Insert", fields(d));
        }
    }

    /** GenericProvider.SetProc: one parameter per property, a null is not sent. */
    static Map<String, Object> fields(Object o) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (Field f : o.getClass().getFields()) {
            if (Modifier.isStatic(f.getModifiers()) || List.class.isAssignableFrom(f.getType())) continue;
            try {
                Object v = f.get(o);
                if (v != null) m.put(f.getName(), v);
            } catch (IllegalAccessException e) {
                /* a public field is always readable */
            }
        }
        return m;
    }

    /** DAL: only when AttachmentsList is not empty - DeleteById, then one Proc_DMSAttachments_Insert per file; Insert(): RemoveByIdAndNames when all were removed. */
    private void saveAttachments(SaveRequest r, int hid, int cr, int org, int company, int user, Timestamp now) {
        List<Map<String, Object>> existing = r.id > 0 ? attachments(hid) : new ArrayList<Map<String, Object>>();
        List<Map<String, Object>> keep = new ArrayList<>();
        if (r.id > 0 && r.keepAttachments != null && !r.keepAttachments.isEmpty()) {
            Set<Integer> want = new HashSet<>(r.keepAttachments);
            for (Map<String, Object> a : existing) if (want.contains(netInt(a.get("Id")))) keep.add(a);
        }
        List<Upload> add = r.addAttachments == null ? new ArrayList<Upload>() : r.addAttachments;
        if (keep.isEmpty() && add.isEmpty()) {
            /* AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0 */
            if (r.id > 0 && !existing.isEmpty()) {
                DesktopProc.rows(jdbc, "Sp_DMSAttachments_GetAllMethod", DesktopProc.params(
                        "Id", hid, "ScreenName", SCREEN_NAME, "RefDocumentTypeId", DOCUMENT_TYPE_ID, "Activity", "RemoveByIdAndName"));
            }
            return;
        }
        if (add.size() > 10) throw new Refusal("Select at most ten files at once");
        DesktopProc.rows(jdbc, "Sp_DMSAttachments_GetAllMethod", DesktopProc.params(
                "ScreenName", SCREEN_NAME, "Id", hid, "Activity", "DeleteById"));
        for (Map<String, Object> a : keep) insertAttachment(hid, cr, org, company, user, now, str(a.get("Attachment")),
                str(a.get("UploadedFileCustomName")), netDouble(a.get("UploadedFileSizeMb")));
        for (Upload up : add) {
            byte[] bytes = decode(up);
            String stored = store.store(ctx.requireAccountingUser(), up.name, bytes);
            insertAttachment(hid, cr, org, company, user, now, up.name, stored, bytes.length / 1048576d);
        }
    }

    private void insertAttachment(int hid, int cr, int org, int company, int user, Timestamp now, String name, String stored, double mb) {
        DesktopProc.setProc(jdbc, "Proc_DMSAttachments_Insert", DesktopProc.params(
                "Id", 0,
                "RefAccountId", cr,
                "DMSFoldersLabelsId", 0,
                "RefDocumentTypeId", DOCUMENT_TYPE_ID,
                "RefDocumentNo", hid,
                "Attachment", name,
                "EntryDate", now,
                "EntryUser", user,
                "ModifyDate", now,
                "ModifyUser", user,
                "OrganizationId", org,
                "CompanyId", company,
                "BranchId", 0,
                "ScreenName", SCREEN_NAME,
                "DetailWiseAttachment", Boolean.FALSE,
                "UploadedFileCustomName", stored == null || stored.isEmpty() ? null : stored,
                "UploadedFileSizeMb", mb,
                "LineId", 0));
    }

    private static byte[] decode(Upload up) {
        if (up == null || up.name == null) throw new Refusal("Invalid attachment");
        DesktopAttachmentStore.validateName(up.name);
        if (up.base64 == null || up.base64.length() > 7 * 1024 * 1024) throw new Refusal("Attachment exceeds 5 MB");
        byte[] b;
        try {
            b = Base64.getDecoder().decode(up.base64);
        } catch (IllegalArgumentException e) {
            throw new Refusal("Invalid attachment content");
        }
        if (b.length == 0 || b.length > DesktopAttachmentStore.MAX_BYTES) throw new Refusal("Attachment must be between 1 byte and 5 MB");
        return b;
    }

    public static class Download {
        public final String name;
        public final byte[] bytes;
        public Download(String name, byte[] bytes) { this.name = name; this.bytes = bytes; }
    }

    /** Opening one attached file of a PDC receipt. */
    public Download download(int docId, int attachmentId) {
        header(docId);
        for (Map<String, Object> a : attachments(docId)) {
            if (netInt(a.get("Id")) != attachmentId) continue;
            String original = base(str(a.get("Attachment")));
            String stored = str(a.get("UploadedFileCustomName"));
            return new Download(original, store.read(ctx.requireAccountingUser(), base(stored.isEmpty() ? original : stored)));
        }
        throw new Refusal("Attachment not found");
    }

    private static String base(String v) {
        String n = v.replace('\\', '/');
        n = n.substring(n.lastIndexOf('/') + 1);
        DesktopAttachmentStore.validateName(n);
        return n;
    }

    // ------------------------------------------------------------------------------------------------- deletes

    /** grdfrm_ColumnButtonClick (not in save mode): PdcCheqDeleteByPdcInventoryId. The procedure deletes by Id alone, so the cheque is first proved to be this company's. */
    @Transactional
    public void deleteCheque(int pdcInventoryId) {
        s.requireRight(SCREEN_NAME, "canDelete", "You do not have the right to delete");
        List<Map<String, Object>> own = jdbc.queryForList(
                "SELECT Id FROM dbo.PdcInventory WHERE Id = ? AND OrganizationId = ? AND CompanyId = ? AND DocumentTypeId = ?",
                pdcInventoryId, ctx.currentOrganizationId(), ctx.currentCompanyId(), DOCUMENT_TYPE_ID);
        if (own.isEmpty()) throw new Refusal("Record Not Found For Delete");
        DesktopProc.rows(jdbc, "Sp_PdcInventory_GetAllMethod", DesktopProc.params(
                "Id", pdcInventoryId, "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                "Activity", "DeleteByPdcInventoryId"));
    }

    /** btnDelete_Click: InvPurchaseInvoice.RemoveByID -> Sp_InvoicesVouchersandStocksDelete (DocumentTypeId 16). */
    @Transactional
    public void delete(int id) {
        s.requireRight(SCREEN_NAME, "canDelete", "You do not have the right to delete");
        if (id <= 0) throw new Refusal("Record Not Found For Delete");
        header(id);
        DesktopProc.rows(jdbc, "Sp_InvoicesVouchersandStocksDelete", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                "Id", id, "DocumentTypeId", DOCUMENT_TYPE_ID, "UserId", ctx.currentUserId()));
    }

    // ------------------------------------------------------------------------------------------------- reports

    public static class ReportFilter {
        public Object accountId;
        public Object debitId;
        public String fromDate;
        public String toDate;
        public String cheqFrom;
        public String cheqTo;
        public String status;
    }

    private Map<String, Object> reportParams(ReportFilter f, boolean withDebit) {
        Map<String, Object> p = DesktopProc.params("OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId());
        int acc = netInt(f.accountId);
        if (acc != 0) p.put("GLCrAccountId", acc);
        if (f.fromDate != null && !f.fromDate.trim().isEmpty()) p.put("DateFrom", ts(f.fromDate));
        if (f.toDate != null && !f.toDate.trim().isEmpty()) p.put("DateTo", ts(f.toDate));
        if (f.cheqFrom != null && !f.cheqFrom.trim().isEmpty()) p.put("DateFromCheq", ts(f.cheqFrom));
        if (f.cheqTo != null && !f.cheqTo.trim().isEmpty()) p.put("DateToCheq", ts(f.cheqTo));
        int dr = netInt(f.debitId);
        if (withDebit && dr != 0) p.put("GLDrAccountId", dr);
        String status = f.status == null ? "" : f.status.trim();
        if (!"All".equals(status)) p.put("CheqStatus", status);
        return p;
    }

    /** btnshow_Click / 125 / 127: GeneralReprots.PdcReceiptsRegister -> Sp_PdcInventory_SlipAndRegister_Rpt. */
    public List<Map<String, Object>> register(ReportFilter f) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_PdcInventory_SlipAndRegister_Rpt", reportParams(f, true))) out.add(plain(r));
        return out;
    }

    /** btnshow_Click: the same table as grdReprot shows it. */
    public List<Map<String, Object>> registerGrid(ReportFilter f) {
        return gridRows("Sp_PdcInventory_SlipAndRegister_Rpt", reportParams(f, true));
    }

    /** 126-PdcSummery: GeneralReprots.PdcReceiptsRegisterSummery -> Sp_PdcInventory_ReceiptsSummery_Rpt (no debit account). */
    public List<Map<String, Object>> summary(ReportFilter f) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_PdcInventory_ReceiptsSummery_Rpt", reportParams(f, false))) out.add(plain(r));
        return out;
    }

    /** SlipPrint(id): GeneralReprots.PdcInventorySlip -> Sp_PdcInventoryHeader_rpt (@Id only when != 0). */
    public List<Map<String, Object>> slip(int id) {
        Map<String, Object> p = DesktopProc.params("OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId());
        if (id != 0) {
            header(id);
            p.put("Id", id);
        }
        p.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_PdcInventoryHeader_rpt", p)) out.add(r);
        return out;
    }

    // ------------------------------------------------------------------------------------------------- helpers

    /** Conversion.ToInt: null / "" / a non-integer string -> 0. */
    static int netInt(Object v) {
        if (v == null) return 0;
        try {
            if (v instanceof Number) {
                double d = ((Number) v).doubleValue();
                if (Double.isNaN(d) || Double.isInfinite(d)) return 0;
                double x = Math.rint(d);
                if (x > Integer.MAX_VALUE || x < Integer.MIN_VALUE) return 0;
                return (int) x;
            }
            String t = String.valueOf(v).trim();
            if (!t.matches("[+-]?\\d+")) return 0;
            return Integer.parseInt(t);
        } catch (RuntimeException e) {
            return 0;
        }
    }

    static double netDouble(Object v) {
        if (v == null) return 0.0;
        try {
            double d = v instanceof Number ? ((Number) v).doubleValue() : Double.parseDouble(String.valueOf(v).trim());
            return Double.isNaN(d) || Double.isInfinite(d) ? 0.0 : d;
        } catch (RuntimeException e) {
            return 0.0;
        }
    }

    private static String str(Object v) { return v == null ? "" : String.valueOf(v); }

    /** 'yyyy-MM-dd' or 'yyyy-MM-dd HH:mm:ss' / 'T' form -> Timestamp (a date alone gets 00:00:00). */
    static Timestamp ts(String v) {
        if (v == null || v.trim().isEmpty()) throw new Refusal("Invalid date");
        try {
            String t = v.trim().replace('T', ' ');
            if (t.length() == 10) return Timestamp.valueOf(LocalDate.parse(t).atTime(LocalTime.MIDNIGHT));
            if (t.length() == 16) t = t + ":00";
            return Timestamp.valueOf(LocalDateTime.parse(t.substring(0, 19), DT));
        } catch (RuntimeException e) {
            throw new Refusal("Invalid date");
        }
    }

    /** JSON friendly value: Timestamp -> yyyy-MM-dd HH:mm:ss, BigDecimal -> plain number string. */
    static Object norm(Object v) {
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().format(DT);
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().atStartOfDay().format(DT);
        if (v instanceof BigDecimal) return ((BigDecimal) v).stripTrailingZeros().toPlainString();
        return v;
    }

    static Map<String, Object> plain(Map<String, Object> row) {
        Map<String, Object> o = new LinkedHashMap<>();
        for (Map.Entry<String, Object> e : row.entrySet()) o.put(e.getKey(), norm(e.getValue()));
        return o;
    }
}
