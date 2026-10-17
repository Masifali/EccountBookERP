package com.mst.services;

import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.sql.Date;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Order Management grids - three desktop forms that read the same procedure as the 1001 card dashboard
 * but show it as grids:
 *
 *   363 frmOrderManagement                      Architecture.WinApp.Dashboard\frmOrderManagement.cs (419 lines)
 *       Purchase / Sale tabs, one GridEX each (grdPurchaseOrders, grdSaleOrders), every row of that TranType.
 *   366 OrderManagementDashboardForPurchase     ...\OrderManagementDashboardForPurchase.cs (235 lines)
 *   367 OrderManagementDashboardForSale         ...\OrderManagementDashboardForSale.cs (235 lines)
 *       One flow panel of OrderManageMentGrids cards (DynamicCards\OrderManageMentGrids.cs), one card per CardCount.
 *
 * Procedure (BLL.Dashboard.SPU_Invetnory_OrderManagementDashBoards):
 *   SPU_Invetnory_OrderManagementDashBoards @OrganizationId, @CompanyId, @ToDate
 * The three forms pass DateTime.Now for @ToDate (frmOrderManagement :36, ForPurchase :73, ForSale :73).
 *
 * Columns read: CardCount, TranType, SortNo, ActivityDescription, ItemCategoryId, CategoryDescription, ItemId,
 * ItemName, OrderBalanceQty, OrderBalalanceWeight (misspelled by the procedure), StockBalanceQty, StockBalanceWeight,
 * AvgRate40Kg, OrderCount, NoOfParties, OrderAmount. TranType may also be "Export"; none of the three forms shows it.
 *
 * Read-only on the desktop: no save, update or delete.
 */
@Service
public class W4OrderManagementGridsService {

    private static final Logger LOG = LoggerFactory.getLogger(W4OrderManagementGridsService.class);

    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private CurrentUserContext currentUserContext;

    /** frmOrderManagement_Load :31-86 and GetDataAndGenerateCards (ForPurchase/ForSale :64-107). */
    public Map<String, Object> data() {
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> purchase = new ArrayList<>();
        List<Map<String, Object>> sales = new ArrayList<>();
        try {
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                    "EXEC SPU_Invetnory_OrderManagementDashBoards @OrganizationId=?, @CompanyId=?, @ToDate=?",
                    currentUserContext.currentOrganizationId(),
                    currentUserContext.currentCompanyId(),
                    Date.valueOf(LocalDate.now()));
            for (Map<String, Object> r : rows) {
                String tranType = str(col(r, "TranType"));
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("cardCount", asInt(col(r, "CardCount")));
                m.put("sortNo", asInt(col(r, "SortNo")));
                m.put("title", str(col(r, "ActivityDescription")));
                m.put("itemId", asInt(col(r, "ItemId")));
                m.put("itemName", str(col(r, "ItemName")));
                m.put("orderQty", num(col(r, "OrderBalanceQty")));
                /* "OrderBalalanceWeight" - spelled that way by the procedure, read as it comes. */
                m.put("orderWeight", num(col(r, "OrderBalalanceWeight")));
                m.put("stockQty", num(col(r, "StockBalanceQty")));
                m.put("stockWeight", num(col(r, "StockBalanceWeight")));
                m.put("avgRate40Kg", num(col(r, "AvgRate40Kg")));
                m.put("orderCount", num(col(r, "OrderCount")));
                m.put("orderAmount", num(col(r, "OrderAmount")));
                if ("Purchase".equals(tranType)) purchase.add(m);       // r.Field<string>("TranType") == "Purchase"
                else if ("Sales".equals(tranType)) sales.add(m);        // == "Sales"
            }
        } catch (Exception e) {
            LOG.error("Order management grids failed", e);
            out.put("error", e.getMessage());
        }
        out.put("purchase", purchase);
        out.put("sales", sales);
        return out;
    }

    private static Object col(Map<String, Object> row, String name) {
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    private static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }

    private static int asInt(Object o) {
        if (o == null) return 0;
        if (o instanceof Number) return ((Number) o).intValue();
        try { return (int) Double.parseDouble(String.valueOf(o).trim()); } catch (Exception e) { return 0; }
    }

    private static double num(Object o) {
        if (o == null) return 0d;
        if (o instanceof Number) return ((Number) o).doubleValue();
        try { return Double.parseDouble(String.valueOf(o).trim()); } catch (Exception e) { return 0d; }
    }
}
