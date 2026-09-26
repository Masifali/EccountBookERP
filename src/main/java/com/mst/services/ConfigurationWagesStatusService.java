package com.mst.services;
import com.mst.models.dto.WagesDocumentStatusDto;
import com.mst.repositories.support.ProcExec;
import com.mst.security.CurrentUserContext;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.http.HttpStatus;
import org.springframework.web.server.ResponseStatusException;
import java.sql.Timestamp;
import java.util.*;

@Service
public class ConfigurationWagesStatusService {
 private final JdbcTemplate jdbc;
 private final CurrentUserContext context;
 public ConfigurationWagesStatusService(JdbcTemplate jdbc,CurrentUserContext context){this.jdbc=jdbc;this.context=context;}
 public List<Map<String,Object>> load(){
  context.requireAccountingUser();
  // Original BLL supplies no organization/company parameters to this global catalogue.
  return jdbc.queryForList("EXEC dbo.USP_GetRefDocumentsForWages");
 }
 public void update(List<WagesDocumentStatusDto> rows){
  if(rows==null || rows.isEmpty()) throw new ResponseStatusException(HttpStatus.BAD_REQUEST,"No document statuses to update");
  var originals=load();var known=new HashMap<Integer,Integer>();
  for(var row:originals)known.put(((Number)row.get("Id")).intValue(),((Number)row.get("RefDocumentTypeId")).intValue());
  var seen=new HashSet<Integer>();
  for(var row:rows)if(row==null || row.isActive()==null || !seen.add(row.id()) || !Objects.equals(known.get(row.id()),row.refDocumentTypeId()))
   throw new ResponseStatusException(HttpStatus.BAD_REQUEST,"The document status list changed. Refresh and retry.");
  // Preserve the desktop BLL's row-by-row original procedure calls and parameter contract.
  for(var row:rows)ProcExec.call(jdbc,"EXEC dbo.USP_ContractorWagesRefDocumentStatus_Insert @OrganizationId=?,@CompanyId=?,@RefDocumentTypeId=?,@Id=?,@ModifyDate=?,@ModifyUserId=?,@IsActive=?",
   context.currentOrganizationId(),context.currentCompanyId(),row.refDocumentTypeId(),row.id(),new Timestamp(System.currentTimeMillis()),context.currentUserId(),row.isActive());
 }
}
