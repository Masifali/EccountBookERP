package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryUomScheduleRequest;
import java.sql.*;
import java.util.*;
import org.springframework.jdbc.core.*;
import org.springframework.stereotype.Repository;
import org.springframework.web.server.ResponseStatusException;
import org.springframework.http.HttpStatus;

@Repository
public class InventoryUomScheduleRepository {
    private final JdbcTemplate jdbc;
    public InventoryUomScheduleRepository(JdbcTemplate jdbc) { this.jdbc=jdbc; }
    public List<Map<String,Object>> items(UserAccount u) {
        return jdbc.execute("{call dbo.Sp_Item_GetAllMethod(?,?,?)}",(CallableStatementCallback<List<Map<String,Object>>>) s->{
            s.setInt(1,u.getOrganizationId());s.setInt(2,u.getCompanyId());s.setNString(3,"ReadAllItemsWithPackingAndStore");
            try(var rs=s.executeQuery()){return new RowMapperResultSetExtractor<Map<String,Object>>(new ColumnMapRowMapper()).extractData(rs);}
        });
    }
    public List<Map<String,Object>> units(UserAccount u) {
        // Desktop UOM.Getall: organization-wide units; procedure intentionally ignores CompanyId.
        return jdbc.queryForList("EXEC dbo.Sp_UOM_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),"ReadByOrganizationCompanyId");
    }
    public List<Map<String,Object>> history(UserAccount u,Integer item) {
        if(item!=null&&item>0)return jdbc.queryForList("EXEC dbo.usp_getAllUomsByCompanyId @OrganizationId=?, @CompanyId=?, @ItemId=?",u.getOrganizationId(),u.getCompanyId(),item);
        return jdbc.queryForList("EXEC dbo.usp_getAllUomsByCompanyId @OrganizationId=?, @CompanyId=?",u.getOrganizationId(),u.getCompanyId());
    }
    public Map<String,Object> record(UserAccount u,int id) {
        var rows=jdbc.queryForList("EXEC dbo.Sp_UOMSchedule_GetAllMethod @Id=?, @Activity=?",id,"ReadById");
        if(rows.size()!=1||!Objects.equals(rows.get(0).get("OrganizationId"),u.getOrganizationId())||!Objects.equals(rows.get(0).get("CompanyId"),u.getCompanyId()))
            throw new ResponseStatusException(HttpStatus.NOT_FOUND,"UOM schedule not found in this company");
        return rows.get(0);
    }
    public int save(UserAccount u,InventoryUomScheduleRequest r) {
        String proc=r.id==0?"Sp_UOMSchedule_Insert":"Sp_UOMSchedule_Update";
        // Every non-virtual model property is passed, just as GenericProvider.SetProc does.
        Object[] values={u.getCompanyId(),new Timestamp(System.currentTimeMillis()),u.getId(),r.equivalent,r.id,r.itemId,new Timestamp(System.currentTimeMillis()),u.getId(),u.getOrganizationId(),r.scheduleUnitId,r.qtyEquivalent,r.baseRateUom,r.basePackUom,r.baseSecondaryUom,r.active};
        return jdbc.execute("EXEC dbo."+proc+" @CompanyId=?, @EntryDate=?, @EntryUser=?, @Equivalent=?, @Id=?, @ItemId=?, @ModifyDate=?, @ModifyUser=?, @OrganizationId=?, @ScheduleUnitId=?, @QtyEquivalent=?, @BaseRateUom=?, @BasePackUom=?, @BaseSecondaryUom=?, @Active=?",(PreparedStatementCallback<Integer>) s->{
            for(int i=0;i<values.length;i++)s.setObject(i+1,values[i]);
            boolean result=s.execute();int id=r.id;
            // Drain all results so SQL errors after an update count cannot become false success.
            while(true){if(result){try(var rs=s.getResultSet()){if(rs.next())id=rs.getInt(1);}}else if(s.getUpdateCount()==-1)break;result=s.getMoreResults();}
            if(id<=0)throw new IllegalStateException("The database did not return a saved UOM schedule");
            return id;
        });
    }
}
