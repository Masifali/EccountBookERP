package com.mst.services;

import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.ConnectionCallback;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Order Management pop-up (open orders for one item).
 *
 * Ported from Architecture.WinApp.Dashboard\OrderManagemantPopUpForPurchaseSale.cs (628 lines),
 * the form the Order Management Dashboard opens when an item name is clicked (:117-133 of the
 * dashboard's own header). InvoiceType 1 is Purchase, 2 is Sales.
 *
 * ONE form, TWO completely different reads, and they are NOT symmetric:
 *
 *   InvoiceType 1 - PurchaseOrder.PurchaseOrderHistory (0427_...Purchase.PurchaseOrder.cs)
 *       [fed].[usp_PurchaseOrder_History_Rpt]
 *           @OrganizationId, @CompanyId            always
 *           @DocumentTypeId = 41                   set by the form (:73)
 *           @ItemId                                the clicked item
 *           @Status = 'Open'                       set by the form (:69)
 *           @IsApproved = 1                        SENT, because the form leaves ApprovedFilter
 *                                                  unset and the BLL guards on
 *                                                  `ApprovedFilter != "All"`
 *       @UserId is NOT sent: the form assigns EntryUser, and this BLL only reads UserId.
 *       @AppId is not a parameter of this procedure at all.
 *
 *   InvoiceType 2 - SaleOrder.SaleOrderHistory (0596_Architecture.BLL.Inventory.SaleOrder.cs)
 *       Sp_SalesSaleOrder_RiceAndPaddyRegister_Rpt      returns TWO result sets
 *           @OrganizationId, @CompanyId, @AppId    always
 *           @CustomerUserId = the signed-in user   always (the BLL maps EntryUser here)
 *           @ItemId                                the clicked item
 *           @Status = 'Open'
 *           @IsApproved                            NOT SENT - the form sets
 *                                                  ApprovedFilter = "All" (:91), which switches
 *                                                  that guard off
 *       Tables[0] is the grid; Tables[1] is the company logo, used only by
 *       ReportHelper.ReplaceCompLogoColumn for printing, so it is not needed here.
 *
 * So the Purchase side is restricted to approved orders and the Sales side is not. That asymmetry
 * is the desktop's, and it is reproduced rather than tidied up.
 *
 * SECOND ASYMMETRY (:251): the Sales grid derives its balances on the client -
 *     BalQty    = OrderItemQty - DispatchQty
 *     BalWeight = NetWeight    - DispatchWeight
 * while the Purchase grid takes BalQty and BalWeight straight from the procedure (:141). Kept.
 *
 * Read-only: the pop-up lists orders and prints slips; it saves nothing.
 */
@Service
public class OrderManagementPopupService {

    private static final Logger LOG = LoggerFactory.getLogger(OrderManagementPopupService.class);

    /** The form sets DocumentTypeId 41 for the purchase read (:73). */
    private static final int PURCHASE_DOCUMENT_TYPE_ID = 41;

    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private CurrentUserContext currentUserContext;

    /**
     * OrderManagemantPopUpForPurchaseSale_Load, :56-110.
     *
     * @param invoiceType 1 = Purchase, 2 = Sales
     * @param itemId      the clicked item; the dashboard already refuses a click when it is <= 0
     */
    public Map<String, Object> orders(int invoiceType, int itemId) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("invoiceType", invoiceType);
        out.put("itemId", itemId);
        /* :113 of the dashboard - no item, nothing happens. */
        if (itemId <= 0) {
            out.put("rows", new ArrayList<>());
            return out;
        }
        try {
            out.put("rows", invoiceType == 1 ? purchaseRows(itemId) : saleRows(itemId));
        } catch (Exception e) {
            LOG.error("Order management pop-up failed for invoiceType {} item {}", invoiceType, itemId, e);
            out.put("error", e.getMessage());
            out.put("rows", new ArrayList<>());
        }
        return out;
    }

    /** GridFillForPuschaseOrder, :112-153 - the desktop's own column names and order. */
    private List<Map<String, Object>> purchaseRows(int itemId) {
        List<String> names = new ArrayList<>();
        List<Object> args = new ArrayList<>();
        names.add("@OrganizationId"); args.add(currentUserContext.currentOrganizationId());
        names.add("@CompanyId");      args.add(currentUserContext.currentCompanyId());
        names.add("@DocumentTypeId"); args.add(PURCHASE_DOCUMENT_TYPE_ID);
        names.add("@ItemId");         args.add(itemId);
        names.add("@IsApproved");     args.add(1);
        names.add("@Status");         args.add("Open");

        List<Map<String, Object>> raw = jdbcTemplate.queryForList(
                exec("[fed].[usp_PurchaseOrder_History_Rpt]", names), args.toArray());

        List<Map<String, Object>> rows = new ArrayList<>(raw.size());
        for (Map<String, Object> r : raw) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id",           col(r, "Id"));
            m.put("DocDate",      col(r, "DocDate"));
            m.put("DocNo",        col(r, "DocNo"));
            m.put("SupCustId",    col(r, "OrderSupCustId"));      // OrderSupCustId -> SupCustId
            m.put("SupplierName", col(r, "SupplierName"));
            m.put("DeliveryTerm", col(r, "DeliveryTerm"));
            m.put("PurchaseGLAC", col(r, "PurchaseGLAC"));
            m.put("ItemId",       col(r, "OrderItemId"));         // OrderItemId -> ItemId
            m.put("ItemName",     col(r, "ItemName"));
            m.put("Crop",         col(r, "Crop"));
            m.put("PackUom",      col(r, "UOMCodeItm"));          // UOMCodeItm -> PackUom
            m.put("Qty",          col(r, "OrderItemQty"));
            m.put("RcvdQty",      col(r, "ReceivedQty"));
            m.put("RejQty",       col(r, "RejQty"));
            m.put("BalQty",       col(r, "BalQty"));              // from the procedure
            m.put("Weight",       col(r, "NetWeight"));
            m.put("RcvdWeight",   col(r, "ReceivedWeight"));
            m.put("BalWeight",    col(r, "BalWeight"));           // from the procedure
            m.put("Rate",         col(r, "OrderItemRate"));
            m.put("ExpiryDate",   col(r, "OrderExpiryDate"));
            rows.add(m);
        }
        return rows;
    }

    /** GridFillForSaleOrder, :225-262. */
    private List<Map<String, Object>> saleRows(int itemId) {
        List<String> names = new ArrayList<>();
        List<Object> args = new ArrayList<>();
        names.add("@OrganizationId");  args.add(currentUserContext.currentOrganizationId());
        names.add("@CompanyId");       args.add(currentUserContext.currentCompanyId());
        int appId = 0;
        try { appId = currentUserContext.currentAppId(); } catch (Exception ignored) { }
        names.add("@AppId");           args.add(appId);
        names.add("@CustomerUserId");  args.add(currentUserContext.currentUserId());
        names.add("@ItemId");          args.add(itemId);
        names.add("@Status");          args.add("Open");
        /* @IsApproved deliberately omitted - ApprovedFilter is "All" on this path (:91). */

        List<List<Map<String, Object>>> sets = callMultiSet(
                exec("Sp_SalesSaleOrder_RiceAndPaddyRegister_Rpt", names), args);
        List<Map<String, Object>> raw = sets.isEmpty() ? new ArrayList<>() : sets.get(0);

        List<Map<String, Object>> rows = new ArrayList<>(raw.size());
        for (Map<String, Object> r : raw) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id",             col(r, "Id"));
            m.put("DocDate",        col(r, "DocDate"));
            m.put("DocNo",          col(r, "DocNo"));
            m.put("CustomerName",   col(r, "SupplierName"));      // the Sales grid renames it
            m.put("SupCustId",      col(r, "OrderSupCustId"));
            m.put("ItemId",         col(r, "OrderItemId"));
            m.put("SaleGLAC",       col(r, "SaleGLAC"));
            m.put("DeliveryTerm",   col(r, "DeliveryTerm"));
            m.put("ItemName",       col(r, "ItemName"));
            m.put("Crop",           col(r, "Crop"));
            m.put("PackUom",        col(r, "UOMCodeItm"));

            double qty      = dbl(col(r, "OrderItemQty"));
            double dispQty  = dbl(col(r, "DispatchQty"));
            double weight   = dbl(col(r, "NetWeight"));
            double dispWt   = dbl(col(r, "DispatchWeight"));
            m.put("Qty",            col(r, "OrderItemQty"));
            m.put("DispatchQty",    col(r, "DispatchQty"));
            /* :251 - derived here, not taken from the procedure. */
            m.put("BalQty",         qty - dispQty);
            m.put("Weight",         col(r, "NetWeight"));
            m.put("DispatchWeight", col(r, "DispatchWeight"));
            m.put("BalWeight",      weight - dispWt);
            m.put("Rate",           col(r, "OrderItemRate"));
            m.put("ExpiryDate",     col(r, "OrderExpiryDate"));
            rows.add(m);
        }
        return rows;
    }

    // ---------------------------------------------------------------- helpers

    /** The sale procedure returns two sets; queryForList would silently keep only the first. */
    private List<List<Map<String, Object>>> callMultiSet(final String sql, final List<Object> args) {
        return jdbcTemplate.execute((ConnectionCallback<List<List<Map<String, Object>>>>) (Connection con) -> {
            List<List<Map<String, Object>>> sets = new ArrayList<>();
            try (PreparedStatement ps = con.prepareStatement(sql)) {
                for (int i = 0; i < args.size(); i++) ps.setObject(i + 1, args.get(i));
                boolean hasResults = ps.execute();
                while (true) {
                    if (hasResults) {
                        try (ResultSet rs = ps.getResultSet()) { sets.add(readRows(rs)); }
                    } else if (ps.getUpdateCount() == -1) {
                        break;
                    }
                    hasResults = ps.getMoreResults();
                    if (!hasResults && ps.getUpdateCount() == -1) break;
                }
            }
            return sets;
        });
    }

    private static List<Map<String, Object>> readRows(ResultSet rs) throws java.sql.SQLException {
        List<Map<String, Object>> rows = new ArrayList<>();
        ResultSetMetaData md = rs.getMetaData();
        int n = md.getColumnCount();
        while (rs.next()) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (int i = 1; i <= n; i++) {
                String label = md.getColumnLabel(i);
                if (label == null || label.isEmpty()) label = md.getColumnName(i);
                m.put(label, rs.getObject(i));
            }
            rows.add(m);
        }
        return rows;
    }

    private static String exec(String proc, List<String> names) {
        StringBuilder b = new StringBuilder("EXEC ").append(proc).append(' ');
        for (int i = 0; i < names.size(); i++) {
            if (i > 0) b.append(", ");
            b.append(names.get(i)).append("=?");
        }
        return b.toString();
    }

    private static Object col(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    private static double dbl(Object o) {
        if (o == null) return 0.0;
        if (o instanceof Number) return ((Number) o).doubleValue();
        try { return Double.parseDouble(String.valueOf(o).trim()); } catch (Exception e) { return 0.0; }
    }
}
