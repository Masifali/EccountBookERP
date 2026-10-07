package com.mst.repositories.cmagt;

import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/** DDL calls for the desktop frmCommissionAgentOrder (DocumentTypeId 159). */
@Repository
public class CommissionAgentOrder159Repository {
    private final JdbcTemplate jdbc;
    public CommissionAgentOrder159Repository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    public List<Map<String,Object>> call(String procedure, Map<String,Object> p) {
        StringBuilder sql=new StringBuilder("EXEC ").append(procedure);List<Object> args=new ArrayList<>();boolean first=true;
        for(var e:p.entrySet()){sql.append(first?" ":", ").append('@').append(e.getKey()).append("=?");args.add(e.getValue());first=false;}
        return jdbc.queryForList(sql.toString(),args.toArray());
    }

    public Object saveHeader(String procedure, LinkedHashMap<String,Object> p, boolean insert) {
        if(insert){List<Map<String,Object>> rows=call(procedure,p);if(rows.isEmpty()||rows.get(0).isEmpty())throw new IllegalStateException("The order number was not returned.");return rows.get(0).values().iterator().next();}
        call(procedure,p);return p.get("Id");
    }

    public void saveDetail(LinkedHashMap<String,Object> p) { call("[dbo].[Sp_InvCommAgentOrderDetail_Insert]",p); }
}
