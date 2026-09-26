package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryItemLookupRequest;
import java.sql.Timestamp;
import java.util.*;
import org.springframework.jdbc.core.*;
import org.springframework.stereotype.Repository;
import org.springframework.http.HttpStatus;
import org.springframework.web.server.ResponseStatusException;

@Repository
public class DesktopInventoryItemLookupRepository {
    public enum Kind { CROP_YEAR, UOM_GROUP }
    private final JdbcTemplate jdbc;
    public DesktopInventoryItemLookupRepository(JdbcTemplate jdbc){this.jdbc=jdbc;}
    private String procedure(Kind kind){return kind==Kind.CROP_YEAR?"Sp_InvCropYear":"Sp_ItemGroup";}
    public List<Map<String,Object>> history(UserAccount u,Kind kind){return jdbc.queryForList("EXEC dbo."+procedure(kind)+"_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity='ReadAll'",u.getOrganizationId(),u.getCompanyId());}
    public Map<String,Object> record(UserAccount u,Kind kind,int id){
        var rows=jdbc.queryForList("EXEC dbo."+procedure(kind)+"_GetAllMethod @"+(kind==Kind.CROP_YEAR?"Id":"GroupId")+"=?, @Activity='ReadById'",id);
        if(rows.size()!=1||!Objects.equals(rows.get(0).get("OrganizationId"),u.getOrganizationId())||!Objects.equals(rows.get(0).get("CompanyId"),u.getCompanyId()))throw new ResponseStatusException(HttpStatus.NOT_FOUND,"Record not found in this company");
        return rows.get(0);
    }
    public Map<String,Object> save(UserAccount u,Kind kind,InventoryItemLookupRequest r){
        if(r.id>0)record(u,kind,r.id);
        String sql="EXEC dbo."+procedure(kind)+(r.id==0?"_Insert":"_Update");Object[] values;
        if(kind==Kind.CROP_YEAR){sql+=" @Id=?, @CropYear=?, @DefaultKey=?, @OrganizationId=?, @CompanyId=?, @EntryUserId=?, @EntryDate=?, @ModifyUserId=?, @ModifyDate=?";var now=new Timestamp(System.currentTimeMillis());values=new Object[]{r.id,r.name,r.defaultKey,u.getOrganizationId(),u.getCompanyId(),u.getId(),now,u.getId(),now};}
        else{sql+=" @GroupId=?, @ItemGroupName=?, @OrganizationId=?, @CompanyId=?";values=new Object[]{r.id,r.name.trim(),u.getOrganizationId(),u.getCompanyId()};}
        int id=jdbc.execute(sql,(PreparedStatementCallback<Integer>) statement->{
            for(int index=0;index<values.length;index++)statement.setObject(index+1,values[index]);int resultId=r.id;boolean result=statement.execute();
            while(true){if(result){try(var rows=statement.getResultSet()){while(rows.next())if(rows.getObject(1) instanceof Number n&&n.intValue()>0)resultId=n.intValue();}}else if(statement.getUpdateCount()==-1)break;result=statement.getMoreResults();}return resultId;
        });
        if(id<=0)throw new IllegalStateException("Save returned no record ID");return record(u,kind,id);
    }
}
