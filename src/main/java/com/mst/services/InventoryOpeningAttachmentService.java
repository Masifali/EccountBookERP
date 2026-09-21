package com.mst.services;
import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryOpeningRequest;
import com.mst.repositories.InventoryOpeningRepository;
import java.sql.Timestamp;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
@Service
public class InventoryOpeningAttachmentService {
 private static final String SCREEN="frmOpeningStockBlancing";
 private final DesktopAttachmentStore store;private final JdbcTemplate jdbc;private final InventoryOpeningRepository repo;
 public InventoryOpeningAttachmentService(DesktopAttachmentStore store,JdbcTemplate jdbc,InventoryOpeningRepository repo){this.store=store;this.jdbc=jdbc;this.repo=repo;}
 public record Prepared(List<Map<String,Object>> rows,String names,String storedNames){}
 public Prepared prepare(UserAccount u,InventoryOpeningRequest r){
  if(r.files==null||r.removeAttachmentIds==null||r.files.size()>10)throw new IllegalArgumentException("At most ten attachments may be uploaded at once");
  List<Map<String,Object>> existing=r.id>0?repo.attachments(u,r.id):List.of();
  var removed=new HashSet<>(r.removeAttachmentIds);for(Integer id:removed)if(id==null||existing.stream().noneMatch(row->Objects.equals(row.get("Id"),id)))throw new IllegalArgumentException("Attachment does not belong to this opening-stock record");
  var rows=new ArrayList<Map<String,Object>>();for(var row:existing)if(!removed.contains(row.get("Id")))rows.add(new LinkedHashMap<>(row));
  for(var upload:r.files){byte[] bytes=DesktopInventoryItemFileService.decode(upload);String saved=store.store(u,upload.name,bytes);rows.add(new LinkedHashMap<>(Map.of("Attachment",upload.name,"UploadedFileCustomName",saved,"UploadedFileSizeMb",bytes.length/1048576d)));}
  return new Prepared(rows,String.join(",",rows.stream().map(row->Objects.toString(row.get("Attachment"),"")).toList()),String.join(",",rows.stream().map(row->Objects.toString(row.get("UploadedFileCustomName"),"")).toList()));
 }
 public void persist(UserAccount u,int id,int item,Prepared attachments){
  repo.record(u,id);
  // The desktop delete procedure has no company predicate. Reject inconsistent ownership before calling it.
  int foreign=jdbc.queryForObject("SELECT COUNT(*) FROM DMSAttachments WITH (UPDLOCK,HOLDLOCK) WHERE ScreenName=? AND RefDocumentNo=? AND (OrganizationId<>? OR CompanyId<>? OR OrganizationId IS NULL OR CompanyId IS NULL)",Integer.class,SCREEN,id,u.getOrganizationId(),u.getCompanyId());
  if(foreign>0)throw new IllegalArgumentException("Opening-stock attachments include another company's record");
  jdbc.update("EXEC dbo.Sp_DMSAttachments_GetAllMethod @ScreenName=?,@Id=?,@Activity='DeleteById'",SCREEN,id);
  for(var row:attachments.rows){Timestamp now=new Timestamp(System.currentTimeMillis());Object[] values={item,40,id,row.get("Attachment"),now,u.getId(),now,u.getId(),u.getOrganizationId(),u.getCompanyId(),SCREEN,row.get("UploadedFileCustomName"),row.get("UploadedFileSizeMb")};
   jdbc.execute("EXEC dbo.Proc_DMSAttachments_Insert @Id=0,@RefAccountId=?,@DMSFoldersLabelsId=0,@RefDocumentTypeId=?,@RefDocumentNo=?,@Attachment=?,@EntryDate=?,@EntryUser=?,@ModifyDate=?,@ModifyUser=?,@OrganizationId=?,@CompanyId=?,@BranchId=0,@ScreenName=?,@DetailWiseAttachment=0,@UploadedFileCustomName=?,@UploadedFileSizeMb=?,@LineId=0",(org.springframework.jdbc.core.PreparedStatementCallback<Void>) statement->{for(int i=0;i<values.length;i++)statement.setObject(i+1,values[i]);boolean result=statement.execute();while(true){if(result){try(var rs=statement.getResultSet()){while(rs.next()){}}}else if(statement.getUpdateCount()==-1)break;result=statement.getMoreResults();}return null;});
  }
  // Existing physical objects may be shared. Remove their DMS association, retaining the object.
  // DesktopAttachmentStore only removes newly created objects on transaction rollback.
 }
 public DesktopInventoryItemFileService.Download download(UserAccount u,int id,int attachment){
  var row=repo.attachments(u,id).stream().filter(r->Objects.equals(r.get("Id"),attachment)).findFirst().orElseThrow(()->new org.springframework.web.server.ResponseStatusException(org.springframework.http.HttpStatus.NOT_FOUND,"Attachment not found"));String path=Objects.toString(row.get("UploadedFileCustomName"),"");if(path.isBlank())path=Objects.toString(row.get("Attachment"),"");String name=Objects.toString(row.get("Attachment"),path);return new DesktopInventoryItemFileService.Download(basename(name),store.read(u,basename(path)),"application/octet-stream");
 }
 private static String basename(String name){String value=name.replace('\\','/');value=value.substring(value.lastIndexOf('/')+1);DesktopAttachmentStore.validateName(value);return value;}
}
