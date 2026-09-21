package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryItemLedgerRequest;
import java.sql.Date;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

/** Exact BLL contracts: InventoryStockEvalautionDetail.ItemEvaluationLedgerNew and StocksReport.Inventory_StockEvalautionDetail_DropDownAndLists. */
@Repository
public class InventoryItemLedgerRepository {
    private final JdbcTemplate jdbc;
    public InventoryItemLedgerRepository(JdbcTemplate jdbc) { this.jdbc=jdbc; }
    public List<Map<String,Object>> items(UserAccount u) {
        return jdbc.queryForList("EXEC dbo.USP_Inventory_StockEvalautionDetail_DropDownAndLists @OrganizationId=?, @CompanyId=?, @ActivityType=?",u.getOrganizationId(),u.getCompanyId(),"Items");
    }
    public List<Map<String,Object>> load(UserAccount u,InventoryItemLedgerRequest r) {
        return ReportValueSupport.decimalStrings(jdbc.queryForList("EXEC dbo.usp_ItemLedgerFromStockEvaluations @OrganizationId=?, @CompanyId=?, @ItemId=?, @FromDate=?, @ToDate=?",u.getOrganizationId(),u.getCompanyId(),r.getItemId(),Date.valueOf(r.getFromDate()),Date.valueOf(r.getToDate())));
    }
    public List<Map<String,Object>> financialYears(UserAccount u) {
        return jdbc.queryForList("EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?",u.getOrganizationId(),u.getCompanyId());
    }
}
