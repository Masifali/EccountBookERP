package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.IUserAccountRepository;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Form History for the eight Sale / Purchase invoice screens.
 *
 * Every one of these desktop forms carries a grdHistory grid (94-145 references each) and the web
 * screens had none at all, so none of them could find a document once it had been saved.
 *
 * ---------------------------------------------------------------------------------------------
 * ONE SERVICE, TWO CONTRACTS - THEY ARE NOT INTERCHANGEABLE
 * ---------------------------------------------------------------------------------------------
 *   Purchase  InvPurchaseInvoice.FormHistory  (BLL 0581) -> [Sp_InvPurchaseInvoice_GetAllMethod]
 *   Sale      InvSaleInvoice.FormHistory      (BLL 0580) -> [Sp_InvSaleInvoice_GetAllMethod]
 *
 * They look alike and are not:
 *
 *   | parameter        | Purchase            | Sale                        |
 *   |------------------|---------------------|-----------------------------|
 *   | doc no from/to   | @FromDocNo/@ToDocNo | @DocNoFrom/@DocNoTo         |
 *   | @BranchesIds     | only when non-empty | ALWAYS sent                 |
 *   | @PaymentTermId   | not a parameter     | only when != 0              |
 *   | @ScreenName      | not a parameter     | ALWAYS sent                 |
 *
 * Forcing them through one generic call would send the wrong parameter names to one of the two
 * procedures, so the two are built separately below.
 *
 * ---------------------------------------------------------------------------------------------
 * AUTHORIZATION - DERIVED SERVER-SIDE, NEVER ACCEPTED FROM THE CALLER
 * ---------------------------------------------------------------------------------------------
 * The desktop sets, for both:
 *
 *     obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
 *     if (!obj.CanViewAllRecord) obj.EntryUser = UserAccount.ID;
 *                                        (InvfrmPurchaseInvoice.cs:4943-4947)
 *
 * so a user without that right sees ONLY their own documents. Both values are therefore computed
 * here from the security context and the grant grid. Neither is a request parameter: accepting a
 * client-supplied "view all" flag, or a client-supplied user id, would let any caller read every
 * other user's invoices.
 *
 * The right is read exactly as SaleOrderCmagtService.canViewAllRecords does - Admin/Administrator
 * short-circuits to true, otherwise the "CanView AllRecord" row of
 * Sp_tblUserRights_GetAllMethod @Activity='GetByUserId' for THIS screen. It is computed per call
 * and never cached in a field: this is a singleton bean, and a cached answer would leak one
 * user's permission to the next request.
 *
 * ---------------------------------------------------------------------------------------------
 * DATE MODE IS A RADIO, AND EACH END IS A CHECKBOX
 * ---------------------------------------------------------------------------------------------
 * The desktop offers four mutually exclusive date modes - document / entry / modify / approved -
 * and within the chosen mode each end only applies when its own checkbox is ticked
 * (InvfrmPurchaseInvoice.cs:4948-4975). So "doc date from 1 Jan, no to-date" is a real query, and
 * an unticked end must be OMITTED rather than defaulted to today or to a sentinel date. dateMode
 * selects which PAIR of parameters a blank-tolerant from/to lands in.
 */
@Service
public class InvoiceHistoryService {

    private static final Logger LOG = LoggerFactory.getLogger(InvoiceHistoryService.class);

    /** Sp_tblUserRights_GetAllMethod's row for the right the desktop reads. */
    private static final String RIGHT_CAN_VIEW_ALL_RECORDS = "CanView AllRecord";

    private static final String SQL_USER_RIGHTS_FOR_SCREEN =
            "EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?, @ScreenName=?, @RightName=?, "
            + "@CompanyId=?, @Activity=?";

    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private CurrentUserContext currentUserContext;

    /**
     * The eight screens, each with the DocumentTypeId its desktop form declares and the desktop
     * form name, which the Sale procedure also wants as @ScreenName.
     *
     * DocumentTypeId is taken from the form, NOT from the caller - it is what separates a Purchase
     * Invoice from a Direct Purchase Invoice in the same table, so letting a request choose it
     * would let one screen read another's documents.
     */
    public enum Screen {
        PURCHASE_INVOICE            (56,  "InvfrmPurchaseInvoice",             false),
        PURCHASE_INVOICE_DIRECT     (57,  "InvfrmPurchasedirectInvoice",       false),
        PURCHASE_INVOICE_GRN_DIRECT (138, "frmPurchaseInvoiceAgaintGrnDirect", false),
        /* CORRECTION: this was first mapped to InvfrmSaleInvoiceReturn / 98, which is the SALE
           Invoice Return form. The Purchase Invoice Return screen's desktop form is
           InvfrmInvPurchaseInvoiceReturn - DocumentTypeId 59 (:1261, :2488, :3086), 126
           grdHistory references, and it calls InvPurchaseInvoice.FormHistory, so the Purchase
           procedure is right. Its ScreenName, which the grant-grid lookup needs, is set at :2516
           to "InvfrmPurchaseReturn" - NOT the class name, so it cannot be inferred from the file. */
        PURCHASE_INVOICE_RETURN     (59,  "InvfrmPurchaseReturn",              false),
        SALE_INVOICE                (95,  "InvfrmSaleInvoice",                 true),
        SALE_INVOICE_DIRECT         (99,  "InvfrmInvSaleInvoiceDirect",        true),
        SALE_INVOICE_GDN_NO_WB      (171, "frmSaleInvoiceAgainstGdnWithoutWb", true),
        /* A SALE form that uses the PURCHASE procedure. InvfrmSaleInvoiceReturn is filed under
           Architecture.WinApp.Sale and its history combo is captioned "Customer Name" (:758), but
           its history call is InvPurchaseInvoice.FormHistory(obj) at :3404 - not InvSaleInvoice,
           which the other three sale screens use (InvfrmSaleInvoice.cs:4926,
           InvfrmInvSaleInvoiceDirect.cs:4546, frmSaleInvoiceAgainstGdnWithoutWb.cs:2764).
           So `sale` is FALSE here: it selects the procedure, not the module. Guessing it from the
           folder would send @DocNoFrom/@DocNoTo and @ScreenName to a procedure that takes
           @FromDocNo/@ToDocNo and no @ScreenName.
           DocumentTypeId 98 (:2269, :3337, :4055); ScreenName from base.Name at :9551. */
        SALE_INVOICE_RETURN         (98,  "InvfrmSaleInvoiceReturn",           false);

        public final int documentTypeId;
        public final String desktopScreenName;
        /** true -> Sp_InvSaleInvoice_GetAllMethod, false -> Sp_InvPurchaseInvoice_GetAllMethod. */
        public final boolean sale;

        Screen(int documentTypeId, String desktopScreenName, boolean sale) {
            this.documentTypeId = documentTypeId;
            this.desktopScreenName = desktopScreenName;
            this.sale = sale;
        }

        public static Screen of(String key) {
            if (key == null) return null;
            String k = key.trim().replace('-', '_').toUpperCase();
            for (Screen s : values()) if (s.name().equals(k)) return s;
            return null;
        }
    }

    /** One history query. Blank dates and zero ids mean "omit", exactly as the BLLs do. */
    public static final class Filter {
        public String dateMode = "doc";   // doc | entry | modify | approved
        public String fromDate;
        public String toDate;
        public int fromDocNo;
        public int toDocNo;
        public int supplierCustomerId;
        public int paymentTermId;         // Sale only
        public String branchesIds;        // CSV of BranchId
        public int noOfRecords;
    }

    public Map<String, Object> history(Screen screen, Filter f) {
        Map<String, Object> out = new LinkedHashMap<>();
        if (screen == null) {
            out.put("success", false);
            out.put("message", "Unknown history screen.");
            out.put("rows", new ArrayList<>());
            return out;
        }
        boolean canViewAll = canViewAllRecords(screen.desktopScreenName);
        try {
            List<String> names = new ArrayList<>();
            List<Object> args = new ArrayList<>();

            names.add("@OrganizationId"); args.add(currentUserContext.currentOrganizationId());
            names.add("@CompanyId");      args.add(currentUserContext.currentCompanyId());
            names.add("@DocumentTypeId"); args.add(screen.documentTypeId);
            names.add("@CanViewAllRecord"); args.add(canViewAll ? 1 : 0);

            /* Both BLLs guard this one on != 0. */
            int yearId = 0;
            try { yearId = currentUserContext.currentFinancialYearId(); } catch (Exception ignored) { }
            if (yearId != 0) { names.add("@FinancialYearId"); args.add(yearId); }

            /* The desktop's own restriction: without the right, only your own documents. */
            names.add("@EntryUser");
            args.add(canViewAll ? 0 : currentUserContext.currentUserId());

            addDateMode(names, args, f);

            if (f.fromDocNo != 0) {
                names.add(screen.sale ? "@DocNoFrom" : "@FromDocNo"); args.add(f.fromDocNo);
            }
            if (f.toDocNo != 0) {
                names.add(screen.sale ? "@DocNoTo" : "@ToDocNo"); args.add(f.toDocNo);
            }
            if (f.supplierCustomerId != 0) {
                names.add("@SupplierCustomerId"); args.add(f.supplierCustomerId);
            }
            if (f.noOfRecords != 0) { names.add("@NoOfRecords"); args.add(f.noOfRecords); }

            if (screen.sale) {
                /* Sale only, and only when set (BLL 0580 guards on PaymenetTermId != 0). */
                if (f.paymentTermId != 0) { names.add("@PaymentTermId"); args.add(f.paymentTermId); }
                /* Sale sends both of these unconditionally. */
                names.add("@BranchesIds"); args.add(f.branchesIds == null ? "" : f.branchesIds);
                names.add("@ScreenName");  args.add(screen.desktopScreenName);
            } else if (f.branchesIds != null && !f.branchesIds.trim().isEmpty()) {
                /* Purchase omits it when empty. */
                names.add("@BranchesIds"); args.add(f.branchesIds.trim());
            }

            names.add("@Activity"); args.add("FormHistory");

            String proc = screen.sale
                    ? "[Sp_InvSaleInvoice_GetAllMethod]"
                    : "[Sp_InvPurchaseInvoice_GetAllMethod]";

            List<Map<String, Object>> rows =
                    jdbcTemplate.queryForList(exec(proc, names), args.toArray());

            out.put("success", true);
            out.put("canViewAllRecords", canViewAll);
            out.put("documentTypeId", screen.documentTypeId);
            out.put("rows", rows);
        } catch (Exception e) {
            LOG.error("Form history failed for {}", screen, e);
            out.put("success", false);
            out.put("message", e.getMessage());
            out.put("rows", new ArrayList<>());
        }
        return out;
    }

    /**
     * The four date modes, mutually exclusive, each end optional.
     *
     * InvfrmPurchaseInvoice.cs:4948-4975 - drdocdate / rdentrydate / rdmodifydate / rdapproveddate,
     * and inside the chosen one FromDateHistory.Checked / ToDateHistory.Checked decide whether each
     * end is set at all. A blank end is omitted; it is never turned into today's date.
     */
    private static void addDateMode(List<String> names, List<Object> args, Filter f) {
        String from = trimToNull(f.fromDate);
        String to   = trimToNull(f.toDate);
        if (from == null && to == null) return;

        String mode = f.dateMode == null ? "doc" : f.dateMode.trim().toLowerCase();
        String fromParam;
        String toParam;
        switch (mode) {
            case "entry":    fromParam = "@EntryFromDate";    toParam = "@EntryToDate";    break;
            case "modify":   fromParam = "@ModifyFromDate";   toParam = "@ModifyToDate";   break;
            case "approved": fromParam = "@ApprovedFromDate"; toParam = "@ApprovedToDate"; break;
            default:         fromParam = "@FromDate";         toParam = "@ToDate";         break;
        }
        if (from != null) { names.add(fromParam); args.add(from); }
        if (to   != null) { names.add(toParam);   args.add(to); }
    }

    /**
     * True when this user may see other users' documents on this screen.
     *
     * Computed per call, never cached: this is a singleton bean and a cached answer would leak one
     * user's permission to another request. Any failure returns FALSE - the restrictive answer -
     * so a lookup problem narrows what is visible instead of widening it.
     */
    private boolean canViewAllRecords(String desktopScreenName) {
        String role = currentRoleName();
        if ("Admin".equalsIgnoreCase(role) || "Administrator".equalsIgnoreCase(role)) return true;
        try {
            List<Map<String, Object>> rights = jdbcTemplate.queryForList(
                    SQL_USER_RIGHTS_FOR_SCREEN,
                    currentUserContext.currentUserId(),
                    desktopScreenName,
                    role == null ? "" : role,
                    currentUserContext.currentCompanyId(),
                    "GetByUserId");
            for (Map<String, Object> r : rights) {
                Object name = ci(r, "RightName");
                if (name != null
                        && RIGHT_CAN_VIEW_ALL_RECORDS.equalsIgnoreCase(name.toString().trim())) {
                    return toBool(ci(r, "Value"));
                }
            }
        } catch (Exception e) {
            LOG.warn("Could not read CanView AllRecord for {}; restricting to own records",
                    desktopScreenName, e);
        }
        return false;
    }

    /**
     * RoleName, now taken from the login itself.
     *
     * The desktop reads clsGlobalVariables.UserAccount.RoleName - a column Sp_UserAccount_Login
     * returns, which is NOT a column of dbo.UserAccount and so cannot be recovered by reloading
     * the user. DesktopLoginAuthenticationProvider carries it on the principal;
     * CurrentUserContext.currentRoleName() reads it there and falls back to
     * UserGroup.UserGroupRole, which is what this method used to do on its own.
     */
    private String currentRoleName() {
        return currentUserContext.currentRoleName();
    }

    // ---------------------------------------------------------------- helpers

    private static String exec(String proc, List<String> names) {
        StringBuilder b = new StringBuilder("EXEC ").append(proc).append(' ');
        for (int i = 0; i < names.size(); i++) {
            if (i > 0) b.append(", ");
            b.append(names.get(i)).append("=?");
        }
        return b.toString();
    }

    private static String trimToNull(String s) {
        if (s == null) return null;
        String t = s.trim();
        return t.isEmpty() ? null : t;
    }

    private static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet())
            if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    private static boolean toBool(Object o) {
        if (o == null) return false;
        if (o instanceof Boolean) return (Boolean) o;
        if (o instanceof Number) return ((Number) o).intValue() != 0;
        String s = String.valueOf(o).trim();
        return "1".equals(s) || "true".equalsIgnoreCase(s);
    }
}
