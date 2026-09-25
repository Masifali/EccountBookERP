package com.mst.services.cmagt;

import com.mst.models.cmagt.dto.*;
import com.mst.repositories.cmagt.SaleOrderCmagtRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.*;

/**
 * Commission Trading - "Sale Order / Deal With Buyer" (DocumentTypeId = 1053).
 *
 * This is a port of Architecture.WinApp.Cmagt/frmSaleOrderCmagt.cs. Every validation
 * message, calculation and ordering decision below was taken from that file; the desktop's
 * own wording is preserved verbatim (including its typos) so the two screens behave
 * identically and users see the same text.
 *
 * Save shape, from Architecture.DAL.CommissionAgent.saleOrderMaster.SetData:
 *
 *   BEGIN TRANSACTION
 *     if saleOrderDetailList empty -> "Detail list not found"
 *     id = USP_saleOrderMaster_InsertAndUpdate(header)
 *     for each detail row       -> USP_saleOrderDetail_Insert           (all rows, incl. soft deletes)
 *     for each payment row      -> USP_saleOrderPaymentDetail_Insert
 *     for each empty-bag row    -> USP_saleOrderEmptyBagDetail_Insert
 *     for each expense row      -> USP_saleOrderBuyerExpenseDetail_Insert
 *     for each commission row   -> USP_saleOrderCommissionDetail_Insert
 *   COMMIT   (ROLLBACK + rethrow on any exception)
 */
@Service
public class SaleOrderCmagtService {

    /**
     * Architecture.Model.CommissionAgent.DocumentTypeEnum:
     *   BuyerInquiry = 1050, SupplierOffer = 1051, PurchaseOrder = 1052,
     *   SaleOrder = 1053, GrnLoading = 1054, GdnDispatched = 1055.
     * Cross-confirmed against frmSaleOrderCmagt.cs line 484: DocumentTypeId = 1053.
     */
    public static final int DOCUMENT_TYPE_ID = 1053;

    /**
     * The screen key the desktop resolves its rights against.
     * frmSaleOrderCmagt.cs line 487: {@code ScreenName = "frmSaleOrderCmagt";}
     * It is matched against ScreenDefinition.ScreenName inside Sp_tblUserRights_GetAllMethod.
     */
    public static final String DESKTOP_SCREEN_NAME = "frmSaleOrderCmagt";

    /**
     * The right's exact name in dbo.ScreenRights - note the space, which is in the real data:
     * CommonServices.SetRightsValueInRightsObject compares
     * {@code lstRights[i].RightName == "CanView AllRecord"}.
     */
    private static final String RIGHT_CAN_VIEW_ALL_RECORDS = "CanView AllRecord";

    private static final DateTimeFormatter ISO = DateTimeFormatter.ISO_LOCAL_DATE;

    @Autowired
    private SaleOrderCmagtRepository repo;

    @Autowired
    private CurrentUserContext currentUserContext;


    // =======================================================================
    // AUTHORIZATION
    //
    // Reproduces the desktop rule exactly, rather than any constant:
    //
    //   frmSaleOrderCmagt.cs:566   formright = CommonServices.SetRightsValueInRightsObject(ScreenName)
    //   frmSaleOrderCmagt.cs:3310  obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights
    //
    // SetRightsValueInRightsObject grants everything when
    // clsGlobalVariables.UserAccount.RoleName is "Admin", and otherwise reads the grant grid
    // via tblUserRights.GetByUserId -> Sp_tblUserRights_GetAllMethod
    // (@UserId, @ScreenName, @RightName = role, @CompanyId, @Activity='GetByUserId'), taking
    // Value from the row whose RightName is "CanView AllRecord".
    //
    // That procedure's GetByUserId branch is SELECT-only and filters
    // `ScreenDefinition.ScreenName = @ScreenName`, so it is safe to call for a read check.
    // Its Admin branch ('Admin' or 'Administrator') projects CONVERT(bit,1) for every right
    // on the screen; the other branch INNER JOINs tblUserRights and projects
    // ISNULL(UR.Value,0).
    //
    // The user id, company id and role all come from the server-side security context. A
    // client cannot supply or widen any of them.
    // =======================================================================

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

    /**
     * True when this user may see other users' Sale Orders on this screen.
     *
     * Computed per call - deliberately not cached in a field, because this is a singleton bean
     * and a cached answer would leak one user's permission to another request.
     */
    public boolean canViewAllRecords() {
        String role = currentRoleName();
        if ("Admin".equalsIgnoreCase(role) || "Administrator".equalsIgnoreCase(role)) {
            return true;
        }
        try {
            List<Map<String, Object>> rights = repo.userRightsForScreen(
                    currentUserContext.currentUserId(),
                    DESKTOP_SCREEN_NAME,
                    role,
                    currentUserContext.currentCompanyId());
            for (Map<String, Object> r : rights) {
                Object name = ci(r, "RightName");
                if (name != null && RIGHT_CAN_VIEW_ALL_RECORDS.equalsIgnoreCase(name.toString().trim())) {
                    return toBool(ci(r, "Value"));
                }
            }
        } catch (Exception ignored) {
            // An unreadable grant grid must not become an implicit grant.
        }
        return false;
    }

    // =======================================================================
    // Document numbering
    // =======================================================================

    /** BLL saleOrderMaster.GenerateCode - @Activity = 'GenerateCode', reads the DocNo column. */
    public int generateNextDocNo() {
        return repo.generateCode(
                currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(),
                currentUserContext.currentBranchId(),
                currentUserContext.currentFinancialYearId(),
                DOCUMENT_TYPE_ID);
    }

    // =======================================================================
    // Master lookups - every dropdown on the screen
    // =======================================================================

    public Map<String, Object> getMasterLookups() {
        int orgId = currentUserContext.currentOrganizationId();
        int companyId = currentUserContext.currentCompanyId();
        int userId = currentUserContext.currentUserId();

        Map<String, Object> m = new LinkedHashMap<>();

        /*
         * Each lookup is fetched independently. If one fails, that dropdown comes back EMPTY
         * and the reason is reported in "lookupErrors" so the screen can say so out loud -
         * a lookup is never back-filled with invented placeholder rows, and a failure is
         * never hidden.
         */
        Map<String, String> errors = new LinkedHashMap<>();

        // The parties list feeds five combos on this screen - Commission Agent / Broker,
        // Buyer / Bill To Party, Deliver / Ship To Party, Sub_Commission Agent Account and
        // Sub_Brokery Agent Account - exactly as SupplierBind() binds one shared dtSupplier
        // DataTable to all of them on the desktop.
        m.put("parties", lookup(errors, "parties", () -> loadParties(orgId, companyId)));

        // [cmagt].[USP_AllComboServices] returns one table whose Activity column splits it
        // into the four sets BindViewCombos() consumes.
        List<Map<String, Object>> combo =
                lookup(errors, "comboServices", () -> repo.allComboServices(orgId, companyId));
        m.put("commissionTypes", filterByActivity(combo, "CommissionType"));
        m.put("commissionRateUoms", filterByActivity(combo, "CommissionRateUom"));
        m.put("paymentBaseDates", filterByActivity(combo, "PaymentBaseDate"));
        m.put("allocatedPackingTypes", filterByActivity(combo, "AllocatedPackingType"));

        m.put("deliveryTerms",   lookup(errors, "deliveryTerms",   () -> repo.deliveryTerms()));
        m.put("paymentTerms",    lookup(errors, "paymentTerms",    () -> repo.paymentTerms(orgId, companyId)));
        m.put("branches",        lookup(errors, "branches",        () -> repo.branches(orgId, companyId, userId)));
        m.put("shipToAddresses", lookup(errors, "shipToAddresses", () -> repo.shipToAddresses(orgId, companyId)));
        m.put("otherItems",      lookup(errors, "otherItems",      () -> repo.otherItems(orgId, companyId)));
        m.put("cities",          lookup(errors, "cities",          () -> repo.cities(orgId, companyId)));
        m.put("cropYears",       lookup(errors, "cropYears",       () -> repo.cropYears(orgId, companyId)));
        m.put("packingTypes",    lookup(errors, "packingTypes",    () -> repo.packingTypes()));

        // Parent Item is not a table of its own on the desktop: ParentCategoryBindFromGlobal()
        // derives it by de-duplicating the item master on
        // (InventoryParentCategoriesId, InvParentCateDescription). Reproduced exactly.
        List<Map<String, Object>> items = lookup(errors, "items", () -> repo.items(orgId, companyId));
        m.put("items", items);
        m.put("parentItems", deriveParentItems(items));

        m.put("lookupErrors", errors);
        return m;
    }

    /** Runs one lookup; on failure records why and yields an empty list, never fake rows. */
    private List<Map<String, Object>> lookup(Map<String, String> errors, String name,
                                             java.util.function.Supplier<List<Map<String, Object>>> supplier) {
        try {
            List<Map<String, Object>> rows = supplier.get();
            return rows == null ? new ArrayList<>() : rows;
        } catch (Exception ex) {
            errors.put(name, rootMessage(ex));
            return new ArrayList<>();
        }
    }

    /**
     * Reproduces frmSaleOrderCmagt.SupplierDtFillFromGlobal().
     *
     * The desktop builds ONE DataTable from clsGlobalVariables.globalAllSupplierCustomer and
     * binds it to all five party combos on this screen. Two behaviours matter and are ported
     * exactly:
     *
     *  1. The "Business Name / Nick Name" radio pair switches the displayed name between
     *     CompanyName and NickName. Both are returned here as DisplayCompany / DisplayNick so
     *     the radio can switch instantly client-side without another round trip, which is what
     *     the desktop's in-memory rebind does.
     *  2. A sub-party (IsSubSupCust with a ParentsSupCustId that resolves) is displayed as
     *     "<parent name> / <own name>" - under whichever of the two naming modes is active.
     */
    private List<Map<String, Object>> loadParties(int orgId, int companyId) {
        List<Map<String, Object>> rows = repo.parties(orgId, companyId);

        Map<Integer, Map<String, Object>> byId = new HashMap<>();
        for (Map<String, Object> r : rows) {
            Object id = r.get("Id");
            if (id instanceof Number) byId.put(((Number) id).intValue(), r);
        }

        List<Map<String, Object>> out = new ArrayList<>(rows.size());
        for (Map<String, Object> r : rows) {
            String company = nzs(r.get("CompanyName"));
            String nick = nzs(r.get("NickName"));

            boolean isSub = toBool(r.get("IsSubSupCust"));
            int parentId = toInt(r.get("ParentsSupCustId"));
            if (isSub && parentId > 0) {
                Map<String, Object> parent = byId.get(parentId);
                if (parent != null) {
                    company = nzs(parent.get("CompanyName")) + " / " + company;
                    nick = nzs(parent.get("NickName")) + " / " + nick;
                }
            }

            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", r.get("Id"));
            o.put("DisplayCompany", company);
            o.put("DisplayNick", nick);
            o.put("PartyCode", r.get("PartyCode"));
            o.put("GlAccountId", r.get("GlAccountId"));
            o.put("CityId", r.get("CityId"));
            o.put("CityName", r.get("CityName"));
            o.put("MobilePersonal", r.get("MobilePersonal"));
            o.put("PartyTypeId", r.get("PartyTypeId"));
            out.add(o);
        }
        return out;
    }

    private static boolean toBool(Object o) {
        if (o instanceof Boolean) return (Boolean) o;
        if (o instanceof Number) return ((Number) o).intValue() != 0;
        return o != null && "true".equalsIgnoreCase(o.toString().trim());
    }

    private static int toInt(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null) return 0;
        try { return Integer.parseInt(o.toString().trim()); } catch (NumberFormatException e) { return 0; }
    }

    private static List<Map<String, Object>> filterByActivity(List<Map<String, Object>> rows, String activity) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (rows == null) return out;
        for (Map<String, Object> r : rows) {
            Object a = r.get("Activity");
            if (a != null && activity.equalsIgnoreCase(a.toString().trim())) {
                Map<String, Object> o = new LinkedHashMap<>();
                o.put("Id", r.get("Id"));
                o.put("Name", r.get("ReferenceName"));
                out.add(o);
            }
        }
        return out;
    }

    private static List<Map<String, Object>> deriveParentItems(List<Map<String, Object>> items) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (items == null) return out;
        Set<String> seen = new HashSet<>();
        for (Map<String, Object> it : items) {
            Object desc = it.get("InvParentCateDescription");
            Object id = it.get("InventoryParentCategoriesId");
            if (desc == null || desc.toString().trim().isEmpty()) continue;
            if (!seen.add(desc.toString())) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", id);
            o.put("Description", desc);
            out.add(o);
        }
        return out;
    }

    // =======================================================================
    // UOM schedule - the conversion factor contract
    //
    // Traced end to end rather than inferred:
    //
    //   frmSaleOrderCmagt.CmbItemName_Leave
    //     -> CommonBindings.ItemUomFromGlobalBind(itemId, CmbPackUom, CmbRateUom)
    //     -> CommonServices.dtUomFromGloablUomScheduleByItemId(itemId, addBaseSecondaryUom: true)
    //
    // That last method builds the DataTable the two combos are actually bound to, with this
    // exact column order:
    //     0 Id (int)   1 UOMCode (string)   2 Equivalent (double)
    //     3 BaseRateUom (bool)   4 BasePackUom (bool)   5 BaseSecondaryUom (bool)
    //
    // So the desktop's positional read, SelectedRow.Cells[2].Value in CalculateWeight and
    // CalculateAmount, is the **Equivalent** column - confirmed at the binding site, not
    // guessed from a procedure's SELECT list. QtyEquivalent is NOT in that table and is NOT
    // the factor. Sp_UOMSchedule_GetAllMethod @Activity='ReadByItemID', which this screen
    // calls, returns a column of the same name and meaning.
    //
    // The browser is handed a stable, self-describing contract (uomId / uomCode / equivalent
    // / equivalentAvailable / basePackUom / baseRateUom) so it never has to guess at raw
    // database column names.
    // =======================================================================

    public List<Map<String, Object>> getItemUomSchedule(int itemId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.uomScheduleByItem(currentUserContext.currentOrganizationId(), itemId)) {
            BigDecimal eq = decimalOrNull(r.get("Equivalent"));
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("uomId", toInt(r.get("Id")));
            o.put("uomCode", nzs(r.get("UOMCode")));
            o.put("equivalent", eq);
            // A usable factor must be present AND strictly positive: Weight multiplies by it
            // and Amount divides by it, so zero is not a legitimate factor in either direction.
            o.put("equivalentAvailable", eq != null && eq.signum() > 0);
            o.put("basePackUom", toBool(r.get("BasePackUom")));
            o.put("baseRateUom", toBool(r.get("BaseRateUom")));
            o.put("baseSecondaryUom", toBool(r.get("BaseSecondaryUom")));
            out.add(o);
        }
        return out;
    }

    /**
     * Per-invocation lookup cache so one save does not re-query the schedule for every row.
     *
     * This is deliberately NOT an instance field. A @Service is a singleton, so an instance
     * field here would be shared mutable state across concurrent requests - one user's save
     * could read another user's cached UOM rows, and two saves could corrupt the map. The
     * context is created inside doSave and passed down.
     */
    private static final class CalcContext {
        final Map<Integer, List<Map<String, Object>>> uom = new HashMap<>();
        final Map<Integer, Map<Integer, BigDecimal>> tax = new HashMap<>();
        /** CmbItemName_Leave -> TaxTypeDbCall(ItemId, datDocDate.Value): the schedule is read
         *  as of the DOCUMENT date, never today. */
        final String docDate;
        CalcContext(String docDate) {
            this.docDate = (docDate == null || docDate.trim().length() < 10)
                    ? LocalDate.now().format(ISO) : docDate.trim().substring(0, 10);
        }
    }

    private List<Map<String, Object>> uomRowsFor(CalcContext ctx, int itemId) {
        return ctx.uom.computeIfAbsent(itemId,
                id -> repo.uomScheduleByItem(currentUserContext.currentOrganizationId(), id));
    }

    /**
     * Resolves a conversion factor, or refuses. An absent schedule row, a null Equivalent or a
     * non-positive Equivalent are all treated as "the required factor is unavailable" - they are
     * NOT silently turned into 0 or 1. The desktop would display a transient 0 here, but that 0
     * can never reach the database: FormValidationDetail blocks the row from being added and
     * Insert()'s ValidateField blocks the save. This method makes that refusal explicit and
     * specific instead of letting a zero look like a legitimate amount.
     */
    private BigDecimal requireUomFactor(CalcContext ctx, int itemId, int uomId, String which, int rowIndex) {
        if (uomId <= 0) {
            throw new ValidationException(which + " is required in Detail Grid at row No: " + (rowIndex + 1), "detail");
        }
        for (Map<String, Object> r : uomRowsFor(ctx, itemId)) {
            if (toInt(r.get("Id")) == uomId) {
                BigDecimal eq = decimalOrNull(r.get("Equivalent"));
                if (eq == null) {
                    throw new ValidationException(which + " conversion factor (Equivalent) is missing in the "
                            + "UOM schedule for this item - row#" + (rowIndex + 1)
                            + ". The row cannot be calculated or saved until it is set up.", "detail");
                }
                if (eq.signum() <= 0) {
                    throw new ValidationException(which + " conversion factor (Equivalent) is " + eq.toPlainString()
                            + " in the UOM schedule for this item - row#" + (rowIndex + 1)
                            + ". A usable factor must be greater than zero.", "detail");
                }
                return eq;
            }
        }
        throw new ValidationException(which + " is not in this item's UOM schedule - row#" + (rowIndex + 1)
                + ". The row cannot be calculated or saved.", "detail");
    }

    /**
     * Re-computes one detail row server-side, exactly as the desktop does, and refuses to accept
     * a browser-supplied figure that disagrees. The browser's arithmetic is a convenience for the
     * user, never the authority for what is written.
     *
     *   CalculateWeight   Weight = Qty * packEquivalent                     (multiply)
     *   CalculateAmount   Amount = Weight / rateEquivalent * Rate           (divide, then multiply)
     *   CalculateTaxAmount TaxAmount = Amount * TaxPercent / 100
     *                      TotalAmount = Amount + TaxAmount
     *
     * Every desktop result is written back through ToString("#,##0.###") and re-parsed by
     * FillCustomerDetailRow, so each persisted figure carries at most 3 decimals. Math.Round in
     * CalculateTaxAmount is explicitly MidpointRounding.AwayFromZero, which for these
     * non-negative values is HALF_UP. Both are reproduced here in BigDecimal.
     */
    private void recomputeAndVerifyRow(CalcContext ctx, SaleOrderCmagtDetailDto d, int rowIndex) {
        int itemId = iv(d.getItemId());
        BigDecimal packEq = requireUomFactor(ctx, itemId, iv(d.getPackUomId()), "Pack Uom", rowIndex);
        BigDecimal rateEq = requireUomFactor(ctx, itemId, iv(d.getRateUomId()), "Rate Uom", rowIndex);

        BigDecimal qty = nz(d.getItemQty());
        BigDecimal rate = nz(d.getItemRate());

        // All four figures come from the shared, unit-tested arithmetic - see
        // SaleOrderCmagtCalc, which carries the desktop formulas and rounding.
        BigDecimal weight = SaleOrderCmagtCalc.weight(qty, packEq);
        BigDecimal amount = SaleOrderCmagtCalc.amount(weight, rateEq, rate);

        BigDecimal taxPercent = nz(d.getTaxPercent());
        if (iv(d.getTaxNameId()) > 0) {
            BigDecimal declared = taxPercentFor(ctx, itemId, iv(d.getTaxNameId()));
            if (declared != null) taxPercent = declared;      // the schedule wins over the client
        }
        BigDecimal taxAmount = SaleOrderCmagtCalc.taxAmount(amount, taxPercent);
        BigDecimal totalAmount = SaleOrderCmagtCalc.totalAmount(amount, taxAmount);

        disagree(d.getItemWeight(), weight, "Weight", rowIndex);
        disagree(d.getItemAmount(), amount, "Amount", rowIndex);
        disagree(d.getTaxAmount(), taxAmount, "Tax Amount", rowIndex);
        disagree(d.getTotalAmount(), totalAmount, "Tax + Amount", rowIndex);

        // Persist the server's own figures, not the browser's.
        d.setItemWeight(weight);
        d.setItemAmount(amount);
        d.setTaxPercent(taxPercent);
        d.setTaxAmount(taxAmount);
        d.setTotalAmount(totalAmount);
        d.setPackUomEquivalent(packEq.doubleValue());
        d.setRateUomEquivalent(rateEq.doubleValue());
    }

    /** Both sides round to 3 decimals, so anything beyond 0.001 is a real disagreement. */
    private static void disagree(BigDecimal submitted, BigDecimal computed, String field, int rowIndex) {
        BigDecimal s = nz(submitted);
        if (s.subtract(computed).abs().compareTo(new BigDecimal("0.001")) > 0) {
            throw new ValidationException(field + " submitted as " + s.toPlainString()
                    + " but recalculates to " + computed.toPlainString() + " at row#" + (rowIndex + 1)
                    + ". The row was not saved.", "detail");
        }
    }

    /** The Tax% the item's own tax schedule declares, or null when it cannot be resolved. */
    private BigDecimal taxPercentFor(CalcContext ctx, int itemId, int taxNameId) {
        Map<Integer, BigDecimal> byTax = ctx.tax.computeIfAbsent(itemId, id -> {
            Map<Integer, BigDecimal> m = new HashMap<>();
            try {
                for (Map<String, Object> r : repo.itemTaxSchedule(
                        currentUserContext.currentOrganizationId(),
                        currentUserContext.currentCompanyId(),
                        id, ctx.docDate)) {
                    m.put(toInt(r.get("TaxNameId")), decimalOrNull(r.get("TaxPercent")));
                }
            } catch (Exception ignored) {
                // Unresolvable schedule: fall back to the submitted percent, which is still
                // validated by the required-field checks below.
            }
            return m;
        });
        return byTax.get(taxNameId);
    }

    private static BigDecimal decimalOrNull(Object o) {
        if (o == null) return null;
        if (o instanceof BigDecimal) return (BigDecimal) o;
        if (o instanceof Number) return new BigDecimal(o.toString());
        String s = o.toString().trim().replace(",", "");
        if (s.isEmpty()) return null;
        try { return new BigDecimal(s); } catch (NumberFormatException e) { return null; }
    }

    /**
     * Sp_ItemTaxSchedule_GetAllMethod @Activity='GetItemTaxScheduleForItemId'.
     * Returns TaxNameId / TaxName / TaxPercent, which is what CmbTaxName binds to and what
     * CalculateTaxAmount() reads its percentage from.
     */
    public List<Map<String, Object>> getItemTaxSchedule(int itemId, String docDate) {
        return repo.itemTaxSchedule(
                currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(),
                itemId,
                (docDate == null || docDate.isEmpty()) ? LocalDate.now().format(ISO) : docDate);
    }

    // =======================================================================
    // Load one order
    // =======================================================================

    /**
     * Replicates Architecture.DAL.CommissionAgent.saleOrderMaster.GetData: one header row
     * plus the same five child activities the desktop DAL loads for it.
     */
    /**
     * Case-insensitive column lookup across candidate names.
     *
     * This exists because the desktop gets case-insensitivity for free - ADO.NET DataTable
     * indexers ignore case, so HistoryFill() can read row["docNo"] from a column the procedure
     * actually names DocNo. JSON and JavaScript are case-sensitive, so the same spelling that
     * works on the desktop silently yields undefined on the web. The procedures are genuinely
     * inconsistent about this: ReadById projects VS.status while FormHistory projects vs.Status,
     * and the header column is BuyerReferenceNo though the model property is buyerReferenceNo.
     * Rather than depend on any of that, the browser is handed explicit, stable names.
     */
    static Object ci(Map<String, Object> row, String... names) {
        if (row == null) return null;
        for (String n : names) {
            if (row.containsKey(n)) return row.get(n);
        }
        for (String n : names) {
            for (Map.Entry<String, Object> e : row.entrySet()) {
                if (e.getKey().equalsIgnoreCase(n)) return e.getValue();
            }
        }
        return null;
    }

    /** Header row -> stable contract, keyed by the desktop model's own property names. */
    static Map<String, Object> normalizeHeader(Map<String, Object> r) {
        Map<String, Object> h = new LinkedHashMap<>();
        h.put("saleOrderMasterId", ci(r, "saleOrderMasterId"));
        h.put("docNo", ci(r, "docNo", "DocNo"));
        h.put("docDate", ci(r, "docDate", "DocDate"));
        h.put("documentTypeId", ci(r, "documentTypeId", "DocumentTypeId"));
        h.put("commissionAgentId", ci(r, "commissionAgentId"));
        h.put("commissionAgentName", ci(r, "CommissionAgentName"));
        h.put("buyerId", ci(r, "buyerId"));
        h.put("buyerName", ci(r, "BuyerName"));
        h.put("deliveryToPartyId", ci(r, "DeliveryToPartyId"));
        h.put("deliveryToPartyName", ci(r, "DeliveryToPartyName"));
        h.put("deliveryTermId", ci(r, "deliveryTermId"));
        h.put("deliveryTerm", ci(r, "DeliveryTerm"));
        h.put("deliveryStartDate", ci(r, "deliveryStartDate"));
        h.put("deliveryDays", ci(r, "deliveryDays"));
        h.put("validityDate", ci(r, "ValidityDate"));
        h.put("isWhtApplied", ci(r, "isWhtApplied"));
        h.put("isBuyerOtherChargesAllowed", ci(r, "isbuyerOtherChargesAllowed"));
        h.put("ebWeightDeductionTermId", ci(r, "EBWeightDeductionTermId"));
        h.put("statusId", ci(r, "statusId", "StatusId"));
        h.put("status", ci(r, "status", "Status"));
        h.put("remarksHeader", ci(r, "remarksHeader"));
        // The procedure names this column BuyerReferenceNo; the model property is buyerReferenceNo.
        h.put("buyerReferenceNo", ci(r, "BuyerReferenceNo", "buyerReferenceNo"));
        h.put("shipToAddress", ci(r, "ShipToAddress"));
        h.put("shipToAddressId", ci(r, "shipToAddressId"));
        h.put("paymentScheduleDescription", ci(r, "paymentScheduleDescription"));
        h.put("organizationId", ci(r, "organizationId"));
        h.put("companyId", ci(r, "companyId"));
        h.put("branchId", ci(r, "branchId"));
        h.put("financialYearId", ci(r, "financialYearId"));
        h.put("entryUserId", ci(r, "entryUserId"));
        h.put("entryDate", ci(r, "entryDate", "EntryDate"));
        h.put("modifyUserId", ci(r, "modifyUserId"));
        h.put("modifyDate", ci(r, "modifyDate", "ModifyDate"));
        h.put("isApproved", ci(r, "isApproved", "IsApproved"));
        h.put("actionId", ci(r, "actionId"));
        h.put("revisionNo", ci(r, "revisionNo"));
        return h;
    }

    /**
     * Loads one order, enforcing scope server-side.
     *
     * The ReadById branch of USP_saleOrderMaster_GetAllMethod filters on
     * `[saleOrderMasterId] = @Id` ONLY - it applies no organization, company or user filter of
     * its own. On the desktop that is safe because the whole client runs inside one
     * authenticated session's context; over HTTP it is not, because the id is supplied by the
     * caller. Without the checks below any signed-in user could read any Sale Order, including
     * another tenant's, by changing the number in the URL.
     *
     * Hiding the row or the link in the browser is not a control; this is the control.
     */
    public Map<String, Object> getSaleOrderById(int id) {
        Map<String, Object> h = scopedHeader(id);
        if (h == null) return null;

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", h);
        out.put("detail", repo.readDetailByHeaderId(id));
        out.put("expenses", repo.readExpensesByHeaderId(id));
        out.put("emptyBags", repo.readEmptyBagsByHeaderId(id));
        out.put("commissions", repo.readCommissionsByHeaderId(id));
        out.put("payments", repo.readPaymentsByHeaderId(id));
        return out;
    }

    /**
     * The stored header of one order, or null when it does not exist (ReadById filters
     * ActionId <> 3). Throws AccessDeniedException when it belongs to another organization /
     * company, or to another user and the caller lacks "CanView AllRecord".
     *
     * Used by the read, and - since 2026-09-25 - by Update and Delete as well. Both
     * USP_saleOrderMaster_InsertAndUpdate (ActionId 2) and the DeleteById activity act on
     * `saleOrderMasterId = @Id` alone, and the Update branch even rewrites organizationId /
     * companyId to the caller's, so without this check any signed-in user could overwrite or
     * delete another tenant's order by posting its id.
     */
    private Map<String, Object> scopedHeader(int id) {
        List<Map<String, Object>> header = repo.readHeaderById(id);
        if (header == null || header.isEmpty()) return null;

        Map<String, Object> h = normalizeHeader(header.get(0));

        // Tenant isolation - always, regardless of any right.
        int ctxOrg = currentUserContext.currentOrganizationId();
        int ctxCompany = currentUserContext.currentCompanyId();
        int rowOrg = toInt(h.get("organizationId"));
        int rowCompany = toInt(h.get("companyId"));
        if (rowOrg != ctxOrg || rowCompany != ctxCompany) {
            throw new org.springframework.security.access.AccessDeniedException(
                    "This Sale Order belongs to another organization or company.");
        }

        // Ownership - only a user holding "CanView AllRecord" on frmSaleOrderCmagt may open
        // a document entered by someone else. This mirrors the scope the same right gives
        // History, so the listing and the direct load cannot disagree.
        if (!canViewAllRecords()) {
            int owner = toInt(h.get("entryUserId"));
            if (owner != currentUserContext.currentUserId()) {
                throw new org.springframework.security.access.AccessDeniedException(
                        "You do not have permission to open Sale Orders entered by another user.");
            }
        }
        return h;
    }

    // =======================================================================
    // History
    // =======================================================================

    /**
     * BLL saleOrderMaster.FormHistory. The desktop offers three mutually exclusive date
     * modes on one From/To pair - Doc Date, Entry Date, Modify Date - so only one of the
     * three parameter pairs is ever populated.
     */
    public List<Map<String, Object>> getHistory(String dateMode, String fromDate, String toDate,
                                                String validityFrom, String validityTo,
                                                Integer commissionAgentId, Integer buyerId,
                                                Integer itemId, String parentItemIds,
                                                Integer deliveryToPartyId, String shipToAddress) {
        String docFrom = null, docTo = null, entryFrom = null, entryTo = null,
               modFrom = null, modTo = null;

        if ("entrydate".equalsIgnoreCase(dateMode)) {
            entryFrom = fromDate; entryTo = toDate;
        } else if ("modifydate".equalsIgnoreCase(dateMode)) {
            modFrom = fromDate; modTo = toDate;
        } else {
            docFrom = fromDate; docTo = toDate;           // Doc Date is the desktop default
        }

        List<Map<String, Object>> raw = repo.formHistory(
                currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(),
                currentUserContext.currentBranchId(),
                currentUserContext.currentFinancialYearId(),
                // The real right, resolved server-side from the grant grid - see
                // canViewAllRecords(). Organization / Company / Branch / FinancialYear
                // isolation comes from the trusted context above and is never client-supplied.
                canViewAllRecords(),
                currentUserContext.currentUserId(),
                docFrom, docTo, entryFrom, entryTo, modFrom, modTo,
                null, null,
                validityFrom, validityTo,
                null, null, null,
                commissionAgentId, buyerId, itemId,
                parentItemIds, deliveryToPartyId, shipToAddress);

        // FormHistory's final SELECT projects DocNo / DocDate / BuyerName / Status with capitals,
        // while ReadById projects some of the same values in lower case. Normalising here means
        // the grid - and the clickable document code in particular - cannot silently render blank
        // because of a case difference between two branches of the same procedure.
        List<Map<String, Object>> out = new ArrayList<>(raw.size());
        for (Map<String, Object> r : raw) {
            out.add(normalizeHistoryRow(r));
        }
        return out;
    }

    /**
     * History row -> stable contract.
     *
     * `id` (saleOrderMasterId) and `docNo` are deliberately separate values: the id is what the
     * link resolves against, docNo is only what the user reads. They are never interchangeable -
     * docNo is per financial year/branch and is not unique across the table.
     */
    static Map<String, Object> normalizeHistoryRow(Map<String, Object> r) {
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("id", ci(r, "saleOrderMasterId"));
        o.put("documentTypeId", ci(r, "DocumentTypeId", "documentTypeId"));
        o.put("docNo", ci(r, "DocNo", "docNo"));
        o.put("docDate", ci(r, "DocDate", "docDate"));
        o.put("commissionAgent", ci(r, "CommissionAgentName"));
        o.put("buyerName", ci(r, "BuyerName", "buyerName"));
        o.put("deliverToParty", ci(r, "DeliveryToPartyName"));
        o.put("deliverToAddress", ci(r, "ShipToAddress"));
        o.put("deliveryTerm", ci(r, "DeliveryTerm"));
        o.put("deliveryStartDate", ci(r, "deliveryStartDate"));
        o.put("deliveryDays", ci(r, "deliveryDays"));
        o.put("validityDate", ci(r, "ValidityDate"));
        o.put("whtApplied", ci(r, "isWhtApplied"));
        o.put("status", ci(r, "Status", "status"));
        o.put("entryUser", ci(r, "EntryUserName"));
        o.put("entryDate", ci(r, "EntryDate", "entryDate"));
        o.put("modifyUser", ci(r, "ModifyUserName"));
        o.put("modifyDate", ci(r, "ModifyDate", "modifyDate"));
        o.put("noOfAttachments", ci(r, "NoOfAttachments"));
        return o;
    }

    // =======================================================================
    // Delete (whole-order soft delete)
    // =======================================================================

    public Map<String, Object> deleteSaleOrder(int id) {
        Map<String, Object> r = new LinkedHashMap<>();
        if (id <= 0) {
            r.put("success", false);
            r.put("message", "No record found to Delete");
            return r;
        }
        try {
            // Same tenant / ownership scope as the read - see scopedHeader().
            if (scopedHeader(id) == null) {
                r.put("success", false);
                r.put("message", "No record found to Delete");
                return r;
            }
            repo.deleteById(currentUserContext.currentUserId(), id);
            r.put("success", true);
            r.put("message", "Delete Record Successfully");
        } catch (Exception ex) {
            r.put("success", false);
            r.put("message", rootMessage(ex));
        }
        return r;
    }

    // =======================================================================
    // Save / Update
    // =======================================================================

    @Transactional
    public Map<String, Object> saveSaleOrder(SaleOrderCmagtDto dto) {
        Map<String, Object> result = new LinkedHashMap<>();
        try {
            int savedId = doSave(dto);
            result.put("success", true);
            result.put("id", savedId);
            result.put("message", dto.getSaleOrderMasterId() != null && dto.getSaleOrderMasterId() > 0
                    ? "Update Successfully" : "Save Successfully");
        } catch (ValidationException ve) {
            // A rejected form must not leave a half-written order behind.
            markRollback();
            result.put("success", false);
            result.put("message", ve.getMessage());
            if (ve.tab != null) result.put("tab", ve.tab);
        } catch (Exception ex) {
            markRollback();
            result.put("success", false);
            result.put("message", rootMessage(ex));
        }
        return result;
    }

    private void markRollback() {
        try {
            org.springframework.transaction.interceptor.TransactionAspectSupport
                    .currentTransactionStatus().setRollbackOnly();
        } catch (Exception ignored) {
            // no active transaction - nothing was written
        }
    }

    private int doSave(SaleOrderCmagtDto h) {
        // ---- formvalidation() -------------------------------------------------
        BigDecimal paymentGridSum = BigDecimal.ZERO;
        for (SaleOrderCmagtPaymentDto p : nz(h.getSaleOrderPaymentDetailList())) {
            paymentGridSum = paymentGridSum.add(nz(p.getDueAmount()));
        }

        if (iv(h.getCommissionAgentId()) == 0) throw new ValidationException("Please Select Commission Agent / Broker");
        if (iv(h.getBuyerId()) == 0)           throw new ValidationException("Please Select Buyer Name");
        if (iv(h.getDeliveryToPartyId()) == 0) throw new ValidationException("Please Select Deliver / Ship to Party");

        if (paymentGridSum.compareTo(BigDecimal.ZERO) == 0) {
            if (iv(h.getHeaderPaymentTermId()) == 0) throw new ValidationException("Please Select Payment Term");
            if (iv(h.getHeaderPaymentTermId()) == 2 && iv(h.getHeaderDueDays()) == 0)
                throw new ValidationException("Due Days field is required");
        }
        if (iv(h.getDeliveryTermId()) == 0) throw new ValidationException("Please Select Delivery Term");
        if (iv(h.getDeliveryDays()) == 0)   throw new ValidationException("Delivery Days field is required");

        // ---- server-owned context (never client supplied) ----------------------
        String now = LocalDateTime.now().toString();
        boolean isUpdate = iv(h.getSaleOrderMasterId()) > 0;

        // Update may only touch an order the caller could have opened (tenant + ownership),
        // and only that order's own detail rows: USP_saleOrderDetail_Insert updates / soft-
        // deletes by saleOrderDetailId alone and re-points the row at @saleOrderMasterId.
        Set<Integer> ownDetailIds = new HashSet<>();
        if (isUpdate) {
            Map<String, Object> stored = scopedHeader(iv(h.getSaleOrderMasterId()));
            if (stored == null) throw new ValidationException("Record not update because Id not found");
            // txtDocNo is loaded from the record and read-only on the desktop, so Update always
            // re-sends the stored number; never let a posted value renumber the document.
            if (toInt(stored.get("docNo")) > 0) h.setDocNo(toInt(stored.get("docNo")));
            for (Map<String, Object> r : repo.readDetailByHeaderId(iv(h.getSaleOrderMasterId()))) {
                ownDetailIds.add(toInt(ci(r, "saleOrderDetailId")));
            }
        }

        h.setDocumentTypeId(DOCUMENT_TYPE_ID);
        h.setStatusId(1);                                  // desktop hardcodes statusId = 1
        h.setActionId(isUpdate ? 2 : 1);                   // BLL Save(): 1 insert, 2 update
        h.setOrganizationId(currentUserContext.currentOrganizationId());
        h.setCompanyId(currentUserContext.currentCompanyId());
        h.setBranchId(currentUserContext.currentBranchId());
        h.setFinancialYearId(currentUserContext.currentFinancialYearId());
        h.setEntryUserId(currentUserContext.currentUserId());
        h.setModifyUserId(currentUserContext.currentUserId());
        h.setEntryDate(now);
        h.setModifyDate(now);
        h.setApprovedDate(now);
        h.setIsApproved(false);
        h.setApprovedUserId(0);

        if (isBlank(h.getDocDate())) h.setDocDate(LocalDate.now().format(ISO));
        if (isBlank(h.getDeliveryStartDate())) h.setDeliveryStartDate(h.getDocDate());
        if (isBlank(h.getValidityDate())) h.setValidityDate(h.getDocDate());

        // ---- Detail rows -------------------------------------------------------
        List<SaleOrderCmagtDetailDto> detailToSave = new ArrayList<>();

        // Rows the user removed during an edit session go in FIRST, pre-marked
        // actionTypeId = 3, so the row procedure performs its soft delete. The desktop
        // does exactly this with lstRemoveRecordDetail; deletion is never "omit the row".
        if (isUpdate) {
            for (SaleOrderCmagtDetailDto d : nz(h.getRemovedDetailRows())) {
                requireOwnDetail(ownDetailIds, iv(d.getSaleOrderDetailId()));
                d.setActionTypeId(3);
                detailToSave.add(d);
            }
        }

        // One lookup cache for this save only - never shared between requests.
        CalcContext calc = new CalcContext(h.getDocDate());

        BigDecimal detailSumAmount = BigDecimal.ZERO;
        int rowIndex = 0;
        for (SaleOrderCmagtDetailDto d : nz(h.getSaleOrderDetailList())) {
            d.setSaleOrderDetailId(isUpdate ? iv(d.getSaleOrderDetailId()) : 0);
            d.setActionTypeId(iv(d.getSaleOrderDetailId()) <= 0 ? 1 : 2);
            if (iv(d.getSaleOrderDetailId()) > 0) requireOwnDetail(ownDetailIds, iv(d.getSaleOrderDetailId()));

            // frmSaleOrderCmagt.Insert():2542-2558, in the desktop's own order.
            requireField(iv(d.getInventoryParentCategoryId()), "Parent Category", rowIndex);
            requireField(iv(d.getItemId()), "Item Name", rowIndex);
            requireField(iv(d.getCropYearId()), "Crop Year", rowIndex);
            requireField(iv(d.getPackingTypeId()), "Packing Type", rowIndex);
            requireField(iv(d.getPackUomId()), "Pack Uom", rowIndex);
            requireField(nz(d.getItemQty()), "Qty", rowIndex);
            requireField(nz(d.getItemWeight()), "Weight", rowIndex);
            requireField(nz(d.getItemRate()), "Rate", rowIndex);
            requireField(iv(d.getRateUomId()), "Rate Uom", rowIndex);
            requireField(nz(d.getItemAmount()), "Amount", rowIndex);

            // Authoritative server-side recalculation. It refuses the row outright when a
            // required conversion factor is unavailable, rather than letting that become a
            // zero that looks like a legitimate amount, and it rejects any browser figure
            // that disagrees with the server's own arithmetic.
            recomputeAndVerifyRow(calc, d, rowIndex);

            if (iv(d.getTaxNameId()) > 0
                    || nz(d.getTaxPercent()).compareTo(BigDecimal.ZERO) > 0
                    || nz(d.getTaxAmount()).compareTo(BigDecimal.ZERO) > 0) {
                requireField(iv(d.getTaxNameId()), "Tax Name", rowIndex);
                requireField(nz(d.getTaxPercent()), "Tax Percent", rowIndex);
                requireField(nz(d.getTaxAmount()), "Tax Amount", rowIndex);
            }
            requireField(nz(d.getTotalAmount()), "Total Amount", rowIndex);

            // The desktop back-fills an empty header remark from the first detail remark.
            if (isBlank(h.getRemarksHeader()) && !isBlank(d.getRemarks())) {
                h.setRemarksHeader(d.getRemarks());
            }

            detailSumAmount = detailSumAmount.add(nz(d.getTotalAmount()));
            detailToSave.add(d);
            rowIndex++;
        }

        // DAL SetData throws before touching the header when there is nothing to save.
        if (detailToSave.isEmpty()) throw new ValidationException("Detail list not found");

        // ---- Buyer Other Charges ----------------------------------------------
        List<SaleOrderCmagtExpenseDto> expenses = new ArrayList<>();
        for (SaleOrderCmagtExpenseDto x : nz(h.getSaleOrderBuyerExpenseDetailList())) {
            if (iv(x.getItemId()) == 0 || dv(x.getAmount()) <= 0d) continue;   // desktop's row filter
            if (isBlank(x.getRemarks()) || "0".equals(x.getRemarks().trim())) {
                x.setRemarks("Expense : " + nzs(x.getOtherItemName()).trim()
                        + "  Qty" + trimNum(nz(x.getQty()))
                        + "  @" + trimNum(BigDecimal.valueOf(dv(x.getRate()))));
            }
            expenses.add(x);
        }
        h.setSaleOrderbuyerExpenseDetailDescription(joinExpenseDescription(expenses));

        // ---- Empty Bags Weight Deduction Policy --------------------------------
        List<SaleOrderCmagtEmptyBagDto> emptyBags = new ArrayList<>(nz(h.getSaleOrderEmptyBagDetailList()));
        boolean bagFocNoWeightCut = iv(h.getEBWeightDeductionTermId()) == 3;
        if (!bagFocNoWeightCut && !emptyBags.isEmpty()) {
            Map<Integer, Map<String, Object>> packTypes = packingTypesById();
            for (SaleOrderCmagtEmptyBagDto e : emptyBags) {
                if (nz(e.getWeightCutKg()).compareTo(BigDecimal.ZERO) <= 0) {
                    // Desktop message preserved verbatim, typo included.
                    throw new ValidationException("WeightCut filed required In Empty bags Grid...", "emptybags");
                }
                // The desktop validates rows of any other packing type against packing type 2.
                int pid = iv(e.getPackingTypeId());
                int validationTypeId = (pid == 1 || pid == 2 || pid == 5) ? pid : 2;
                Map<String, Object> pt = packTypes.get(validationTypeId);
                if (pt != null) {
                    double min = dv(asDouble(pt.get("MinEbWeight")));
                    double max = dv(asDouble(pt.get("MaxEbWeight")));
                    double wc = nz(e.getWeightCutKg()).doubleValue();
                    if (wc < min || wc > max) {
                        throw new ValidationException(
                                "Weight Cut Should be in Range of: " + trimD(min) + " to " + trimD(max) + "\n"
                                        + "For Packing Type:" + nzs(pt.get("PackTypeDesc")), "emptybags");
                    }
                }
            }
        }
        h.setSaleOrderEmptyBagDetailDescription(joinEmptyBagDescription(emptyBags));

        // ---- Sub_Commission Agent (agentTypeId 1) / Sub_Brokery Agent (2) -------
        List<SaleOrderCmagtCommissionDto> commissions = new ArrayList<>();
        for (SaleOrderCmagtCommissionDto c : nz(h.getSaleOrderCommissionDetailList())) {
            // Desktop adds a commission row only when BOTH an account is chosen and the
            // computed amount is greater than zero.
            if (nz(c.getCommissionAmount()).compareTo(BigDecimal.ZERO) > 0 && iv(c.getCommissionAgentId()) > 0) {
                commissions.add(c);
            }
        }

        // ---- Payment Schedule ---------------------------------------------------
        List<SaleOrderCmagtPaymentDto> payments = new ArrayList<>();
        BigDecimal paymentDetailAmount = BigDecimal.ZERO;
        BigDecimal pctOfTotal = BigDecimal.ZERO;

        if (paymentGridSum.compareTo(BigDecimal.ZERO) > 0) {
            int pRow = 0;
            for (SaleOrderCmagtPaymentDto p : nz(h.getSaleOrderPaymentDetailList())) {
                if (iv(p.getPaymentTermId()) <= 0)
                    throw new ValidationException("Payment Term Required in row#" + (pRow + 1), "payment");
                if (iv(p.getPaymentTermId()) == 2 && iv(p.getDueDays()) <= 0)
                    throw new ValidationException("Due Days Required In case Of Credit row in row#" + (pRow + 1), "payment");

                // DueDate is derived for BaseDueDateType 1, taken as-entered for 4, else null.
                int baseType = iv(p.getBaseDueDateTypeId());
                if (baseType == 1) {
                    p.setDueDate(LocalDate.parse(h.getDocDate().substring(0, 10))
                            .plusDays(iv(p.getDueDays())).format(ISO));
                } else if (baseType != 4) {
                    p.setDueDate(null);
                }

                pctOfTotal = pctOfTotal.add(nz(p.getPctOfTotal()));
                paymentDetailAmount = paymentDetailAmount.add(nz(p.getDueAmount()));
                p.setSortNo(pRow + 1);
                payments.add(p);
                pRow++;
            }
        } else {
            // No grid rows: synthesise the implicit single 100% schedule row from the
            // header's own Payment Term / Due Days controls.
            SaleOrderCmagtPaymentDto p = new SaleOrderCmagtPaymentDto();
            p.setPaymentTermId(iv(h.getHeaderPaymentTermId()));
            p.setDueDays(iv(h.getHeaderDueDays()));
            p.setBaseDueDateTypeId(1);
            p.setDueDate(LocalDate.parse(h.getDocDate().substring(0, 10))
                    .plusDays(iv(h.getHeaderDueDays())).format(ISO));
            p.setPctOfTotal(new BigDecimal("100"));
            p.setDueAmount(detailSumAmount);
            p.setSortNo(1);
            payments.add(p);
            paymentDetailAmount = detailSumAmount;
            pctOfTotal = new BigDecimal("100");
        }

        h.setPaymentScheduleDescription(joinPaymentDescription(payments));

        if (paymentDetailAmount.compareTo(detailSumAmount) != 0) {
            throw new ValidationException("Payment Detail Amount" + fmt4(paymentDetailAmount)
                    + " Not Equal to Total Amount" + fmt4(detailSumAmount), "payment");
        }
        pctOfTotal = pctOfTotal.setScale(4, RoundingMode.HALF_UP);
        if (new BigDecimal("100").subtract(pctOfTotal).abs().compareTo(new BigDecimal("0.01")) > 0) {
            throw new ValidationException("Payment Detail Total% not near to 100", "payment");
        }

        // ---- Write, in the DAL's own order -------------------------------------
        int masterId = repo.saveMaster(h);
        if (masterId <= 0) masterId = iv(h.getSaleOrderMasterId());
        if (masterId <= 0) throw new ValidationException("Sale Order header was not saved");

        int sort = 1;
        for (SaleOrderCmagtDetailDto d : detailToSave) {
            d.setSaleOrderMasterId(masterId);
            if (iv(d.getSortNo()) == 0) d.setSortNo(sort);
            sort++;
            repo.saveDetailRow(d);
        }
        for (SaleOrderCmagtPaymentDto p : payments) {
            p.setSaleOrderMasterId(masterId);
            repo.savePaymentRow(p);
        }
        int ebSort = 1;
        for (SaleOrderCmagtEmptyBagDto e : emptyBags) {
            e.setSaleOrderMasterId(masterId);
            if (iv(e.getSortNo()) == 0) e.setSortNo(ebSort);
            ebSort++;
            repo.saveEmptyBagRow(e);
        }
        int xSort = 1;
        for (SaleOrderCmagtExpenseDto x : expenses) {
            x.setSaleOrderMasterId(masterId);
            if (iv(x.getSortNo()) == 0) x.setSortNo(xSort);
            xSort++;
            repo.saveExpenseRow(x);
        }
        int cSort = 1;
        for (SaleOrderCmagtCommissionDto c : commissions) {
            c.setSaleOrderMasterId(masterId);
            if (iv(c.getSortNo()) == 0) c.setSortNo(cSort);
            cSort++;
            repo.saveCommissionRow(c);
        }

        return masterId;
    }

    // =======================================================================
    // Description strings - byte-for-byte the desktop's own formats
    // =======================================================================

    private static String joinExpenseDescription(List<SaleOrderCmagtExpenseDto> list) {
        StringJoiner j = new StringJoiner(", ");
        for (SaleOrderCmagtExpenseDto x : list) {
            j.add("[Item:" + iv(x.getItemId()) + ":" + nzs(x.getOtherItemName())
                    + ", Qty:" + trimNum(nz(x.getQty()))
                    + ",Rate:" + trimD(dv(x.getRate()))
                    + ",Amount:" + trimD(dv(x.getAmount())) + "]");
        }
        return j.toString();
    }

    private static String joinEmptyBagDescription(List<SaleOrderCmagtEmptyBagDto> list) {
        StringJoiner j = new StringJoiner(", ");
        for (SaleOrderCmagtEmptyBagDto e : list) {
            j.add("[PackingType:" + iv(e.getPackingTypeId()) + ":" + nzs(e.getPackingType())
                    + ", WeightCut:" + trimNum(nz(e.getWeightCutKg()))
                    + ",Rate:" + trimNum(nz(e.getRate())) + "]");
        }
        return j.toString();
    }

    private static String joinPaymentDescription(List<SaleOrderCmagtPaymentDto> list) {
        StringJoiner j = new StringJoiner(", ");
        for (SaleOrderCmagtPaymentDto p : list) {
            j.add("[Term:" + iv(p.getPaymentTermId()) + ":" + nzs(p.getPaymentTerm())
                    + ",DueDays:" + iv(p.getDueDays())
                    + ",%OfTotal:" + trimNum(nz(p.getPctOfTotal()))
                    + ",DueAmount:" + trimNum(nz(p.getDueAmount()))
                    + ",BaseDateType:" + iv(p.getBaseDueDateTypeId()) + "]");
        }
        return j.toString();
    }

    // =======================================================================
    // Helpers
    // =======================================================================

    private Map<Integer, Map<String, Object>> packingTypesById() {
        Map<Integer, Map<String, Object>> m = new HashMap<>();
        for (Map<String, Object> r : repo.packingTypes()) {
            Object id = r.get("Id");
            if (id instanceof Number) m.put(((Number) id).intValue(), r);
        }
        return m;
    }

    /**
     * FormHelper.ValidateField (FormHelper.cs:503-508), message verbatim:
     * "{fieldName} is required in {GridName} at row No: {rowIndex + 1}", GridName "Detail Grid".
     * int fails only on == 0; decimal fails on <= 0.
     */
    private static void requireField(int value, String field, int rowIndex) {
        if (value == 0) throw new ValidationException(field + " is required in Detail Grid at row No: " + (rowIndex + 1), "detail");
    }

    private static void requireField(BigDecimal value, String field, int rowIndex) {
        if (value == null || value.compareTo(BigDecimal.ZERO) <= 0)
            throw new ValidationException(field + " is required in Detail Grid at row No: " + (rowIndex + 1), "detail");
    }

    /** A posted saleOrderDetailId must be one of this order's own live detail rows. */
    private static void requireOwnDetail(Set<Integer> ownDetailIds, int detailId) {
        if (detailId <= 0 || !ownDetailIds.contains(detailId)) {
            throw new ValidationException("Detail row " + detailId + " does not belong to this Sale Order.", "detail");
        }
    }

    private static <T> List<T> nz(List<T> l) { return l == null ? Collections.emptyList() : l; }
    private static BigDecimal nz(BigDecimal v) { return v == null ? BigDecimal.ZERO : v; }
    private static int iv(Integer v) { return v == null ? 0 : v; }
    private static double dv(Double v) { return v == null ? 0d : v; }
    private static String nzs(Object v) { return v == null ? "" : v.toString(); }
    private static boolean isBlank(String v) { return v == null || v.trim().isEmpty(); }

    private static Double asDouble(Object o) {
        if (o instanceof Number) return ((Number) o).doubleValue();
        if (o == null) return 0d;
        try { return Double.valueOf(o.toString()); } catch (NumberFormatException e) { return 0d; }
    }

    private static String trimNum(BigDecimal v) { return v.stripTrailingZeros().toPlainString(); }
    private static String trimD(double d) { return BigDecimal.valueOf(d).stripTrailingZeros().toPlainString(); }

    private static String fmt4(BigDecimal v) {
        return v.setScale(4, RoundingMode.HALF_UP).stripTrailingZeros().toPlainString();
    }

    /** Surfaces the real SQL Server message rather than a wrapped Spring exception chain. */
    private static String rootMessage(Throwable t) {
        Throwable c = t;
        while (c.getCause() != null && c.getCause() != c) c = c.getCause();
        String m = c.getMessage();
        return (m == null || m.trim().isEmpty()) ? t.toString() : m;
    }

    /** A rejected form, carrying the tab the desktop would have switched to. */
    public static class ValidationException extends RuntimeException {
        public final String tab;
        public ValidationException(String message) { this(message, null); }
        public ValidationException(String message, String tab) { super(message); this.tab = tab; }
    }
}
