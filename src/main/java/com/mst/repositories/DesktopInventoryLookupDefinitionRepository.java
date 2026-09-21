package com.mst.repositories;
import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryLookupDefinitionRequest;
import java.sql.Timestamp;
import java.util.*;
import org.springframework.http.HttpStatus;
import org.springframework.jdbc.core.*;
import org.springframework.stereotype.Repository;
import org.springframework.web.server.ResponseStatusException;
@Repository
public class DesktopInventoryLookupDefinitionRepository {
 private final JdbcTemplate jdbc;
 public DesktopInventoryLookupDefinitionRepository(JdbcTemplate jdbc){this.jdbc=jdbc;}
 public List<Map<String,Object>> types(UserAccount u){return jdbc.queryForList("EXEC dbo.Proc_InvLookupType_ReadAll @OrganizationId=?, @CompanyId=?",u.getOrganizationId(),u.getCompanyId());}
 public List<Map<String,Object>> history(UserAccount u){return jdbc.queryForList("EXEC dbo.Sp_InvLookup_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity='ReadAll'",u.getOrganizationId(),u.getCompanyId());}
 public int code(UserAccount u,int type){if(types(u).stream().noneMatch(r->num(r.get("Id"))==type))throw new IllegalArgumentException("Select a valid lookup type");var rows=jdbc.queryForList("EXEC dbo.Proc_InvLookUpType_GenerateCodeByLookUpType @OrganizationId=?, @CompanyID=?, @InvLookupTypeId=?",u.getOrganizationId(),u.getCompanyId(),type);return rows.isEmpty()?0:num(rows.get(0).get("Code"));}
 public Map<String,Object> record(UserAccount u,int id){var rows=jdbc.queryForList("EXEC dbo.Proc_InvLookup_ReadById @Id=?",id);if(rows.size()!=1)throw new ResponseStatusException(HttpStatus.NOT_FOUND,"Lookup not found");var r=rows.get(0);r.put("CanEdit",num(r.get("OrganizationId"))==u.getOrganizationId()&&num(r.get("CompanyId"))==u.getCompanyId());return r;}
 public Map<String,Object> save(UserAccount u,InventoryLookupDefinitionRequest r){if(r.id>0&&!Boolean.TRUE.equals(record(u,r.id).get("CanEdit")))throw new org.springframework.security.access.AccessDeniedException("This shared lookup belongs to another company and cannot be edited here");if(types(u).stream().noneMatch(row->num(row.get("Id"))==r.typeId))throw new IllegalArgumentException("Select a valid lookup type");var now=new Timestamp(System.currentTimeMillis());Object[] values={r.id,r.typeId,r.code,r.name,u.getId(),now,u.getId(),now,null,0,false,u.getOrganizationId(),u.getCompanyId()};int id=jdbc.execute("EXEC dbo.Proc_InvLookup_"+(r.id==0?"Insert":"Update")+" @Id=?, @InvLookupTypeId=?, @Code=?, @LookupName=?, @EntryUser=?, @EntryDate=?, @ModifyUser=?, @ModifyDate=?, @PostDate=?, @PostUser=?, @PostState=?, @OrganizationId=?, @CompanyId=?",(PreparedStatementCallback<Integer>) statement->{for(int i=0;i<values.length;i++){if(i==8)statement.setNull(i+1,java.sql.Types.TIMESTAMP);else statement.setObject(i+1,values[i]);}boolean result=statement.execute();int saved=r.id;while(true){if(result){try(var rows=statement.getResultSet()){while(rows.next())if(rows.getObject(1) instanceof Number n)saved=n.intValue();}}else if(statement.getUpdateCount()==-1)break;result=statement.getMoreResults();}return saved;});return record(u,id);}
 private static int num(Object value){return value==null?0:((Number)value).intValue();}
}
