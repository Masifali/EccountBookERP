package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryItemTaxScheduleRequest;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.*;
import org.springframework.http.HttpStatus;
import org.springframework.jdbc.core.*;
import org.springframework.stereotype.Repository;
import org.springframework.web.server.ResponseStatusException;

@Repository
public class DesktopInventoryItemTaxScheduleRepository {
    private final JdbcTemplate jdbc;
    public DesktopInventoryItemTaxScheduleRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    public List<Map<String,Object>> items(UserAccount u) {
        var rows = jdbc.queryForList("EXEC dbo.USP_Item_AllItemsWithModal @OrganizationId=?, @CompanyId=?", u.getOrganizationId(), u.getCompanyId());
        if (rows.isEmpty()) return rows;
        var ids = new HashSet<Integer>();
        rows.forEach(row -> ids.add(number(row.get("Id"))));
        for (var row : jdbc.queryForList("EXEC lgstcm.USP_Item_AllServiesItems @OrganizationId=?, @CompanyId=?", u.getOrganizationId(), u.getCompanyId())) {
            if (ids.add(number(row.get("Id")))) rows.add(row);
        }
        return rows;
    }

    public List<Map<String,Object>> taxes(UserAccount u) {
        return jdbc.queryForList("EXEC dbo.Sp_TaxesTypes_GetAllMethod @OrganizationId=?, @CompanyId=?, @Type=2, @Activity='ReadByCombo'", u.getOrganizationId(), u.getCompanyId());
    }

    public Map<String,Object> lookups(UserAccount u) {
        var result = new LinkedHashMap<String,Object>();
        result.put("items", items(u));
        result.put("taxes", taxes(u));
        result.put("filters", jdbc.queryForList("EXEC dbo.Usp_AllComboAgainstItemTaxSchedule @OrganizationId=?, @CompanyId=?", u.getOrganizationId(), u.getCompanyId()));
        return result;
    }

    public List<Map<String,Object>> history(UserAccount u, int category, int type, int item, int tax, LocalDate date, Boolean active) {
        StringBuilder sql = new StringBuilder("EXEC dbo.Sp_ItemTaxSchedule_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity='ReadByOrganizationCompanyId'");
        var values = new ArrayList<Object>(List.of(u.getOrganizationId(), u.getCompanyId()));
        String[] names = {"ItemCategoryId", "ItemTypeId", "ItemId", "TaxTypeId"};
        int[] filters = {category, type, item, tax};
        for (int i=0; i<filters.length; i++) if (filters[i]>0) { sql.append(", @").append(names[i]).append("=?"); values.add(filters[i]); }
        if (date!=null) { sql.append(", @EffectedDate=?"); values.add(java.sql.Date.valueOf(date)); }
        if (active!=null) { sql.append(", @Active=?"); values.add(active); }
        return jdbc.queryForList(sql.toString(), values.toArray());
    }

    public Map<String,Object> record(UserAccount u, int id) {
        var rows = jdbc.queryForList("EXEC dbo.Sp_ItemTaxSchedule_GetAllMethod @Id=?, @Activity='ReadById'", id);
        if (rows.size()!=1 || number(rows.get(0).get("OrganizationId"))!=u.getOrganizationId() || number(rows.get(0).get("CompanyId"))!=u.getCompanyId())
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Item tax schedule not found in this company");
        return rows.get(0);
    }

    public Map<String,Object> save(UserAccount u, InventoryItemTaxScheduleRequest r) {
        if (r.id>0) record(u,r.id);
        var now = new Timestamp(System.currentTimeMillis());
        Object[] values = {u.getCompanyId(), Timestamp.valueOf(r.effectedDate), now, u.getId(), r.id, r.active, r.itemId, now, u.getId(), u.getOrganizationId(), r.taxTypeId};
        String sql = "EXEC dbo.Sp_ItemTaxSchedule_"+(r.id==0?"Insert":"Update")+" @CompanyId=?, @EffectedDate=?, @EntryDate=?, @EntryUser=?, @Id=?, @IsActive=?, @ItemId=?, @ModifyDate=?, @ModifyUser=?, @OrganizationId=?, @TaxTypeId=?";
        int id = jdbc.execute(sql, (PreparedStatementCallback<Integer>) statement -> {
            for (int i=0;i<values.length;i++) statement.setObject(i+1, values[i]);
            boolean result=statement.execute(); int saved=r.id;
            while (true) {
                if (result) { try (var rows=statement.getResultSet()) { while(rows.next()) if(rows.getObject(1) instanceof Number n && n.intValue()>0) saved=n.intValue(); } }
                else if (statement.getUpdateCount()==-1) break;
                result=statement.getMoreResults();
            }
            return saved;
        });
        return record(u,id);
    }
    private static int number(Object value) { return value==null?0:((Number)value).intValue(); }
}
