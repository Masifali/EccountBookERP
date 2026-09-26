package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryUnitRequest;
import java.sql.*;
import java.util.*;
import org.springframework.jdbc.core.*;
import org.springframework.stereotype.Repository;
import org.springframework.http.HttpStatus;
import org.springframework.web.server.ResponseStatusException;

/** UOM BLL0609/DAL0462: organization-wide units, global parent-unit catalog. */
@Repository
public class InventoryUnitRepository {
    private final JdbcTemplate jdbc;
    public InventoryUnitRepository(JdbcTemplate jdbc){this.jdbc=jdbc;}
    public List<Map<String,Object>> history(UserAccount u){return jdbc.queryForList("EXEC dbo.Sp_UOM_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),"ReadByOrganizationCompanyId");}
    public List<Map<String,Object>> parents(){return jdbc.queryForList("EXEC dbo.Sp_UOM_GetAllMethod @Activity=?","GetAllParentUOM");}
    public Map<String,Object> record(UserAccount u,int id){var rows=jdbc.queryForList("EXEC dbo.Sp_UOM_GetAllMethod @Id=?, @Activity=?",id,"ReadById");if(rows.size()!=1||!Objects.equals(rows.get(0).get("OrganizationId"),u.getOrganizationId()))throw new ResponseStatusException(HttpStatus.NOT_FOUND,"UOM not found in this organization");return rows.get(0);}
    public Map<String,Object> save(UserAccount u,InventoryUnitRequest r){
        var old=r.id>0?record(u,r.id):null;Object company=old==null?u.getCompanyId():old.get("CompanyId");
        Object[] values={company,new Timestamp(System.currentTimeMillis()),u.getId(),r.id,new Timestamp(System.currentTimeMillis()),u.getId(),u.getOrganizationId(),r.parentUomId,r.code.trim(),r.code.trim(),r.active,r.code.trim(),r.equivalent,r.qtyEquivalent};
        int id=jdbc.execute("EXEC dbo.Sp_UOM_"+(r.id==0?"Insert":"Update")+" @CompanyId=?, @EntryDate=?, @EntryUser=?, @Id=?, @ModifyDate=?, @ModifyUser=?, @OrganizationId=?, @ParentUOMId=?, @UOMCode=?, @UOMDescription=?, @UOMStatus=?, @UOMSymbol=?, @Equivalent=?, @QtyEquivalent=?",(PreparedStatementCallback<Integer>) s->{for(int i=0;i<values.length;i++)s.setObject(i+1,values[i]);boolean result=s.execute();int saved=r.id;while(true){if(result){try(var rs=s.getResultSet()){if(rs.next())saved=rs.getInt(1);}}else if(s.getUpdateCount()==-1)break;result=s.getMoreResults();}if(saved<=0)throw new IllegalStateException("UOM save returned no record ID");return saved;});
        return record(u,id);
    }
}
