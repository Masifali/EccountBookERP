package com.mst.services;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.mst.models.dto.PurchaseInvoiceAttachmentsDto;
import com.mst.repositories.PurchaseInvoiceRecordRepository;
import com.mst.repositories.support.ProcExec;
import com.mst.security.CurrentUserContext;
import java.sql.Timestamp;
import java.util.*;
import java.util.stream.Collectors;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.web.server.ResponseStatusException;
import static org.springframework.http.HttpStatus.NOT_FOUND;
import static com.mst.services.PurchaseInvoiceFinancialRules.*;

/** Invoice form 3797-3801 and DAL 0434:54-58,160-191; original DMS procedures/storage. */
@Service
public class PurchaseInvoiceAttachmentService {
    private final JdbcTemplate jdbc;private final CurrentUserContext context;
    private final DesktopAttachmentStore store;private final PurchaseInvoiceRecordRepository records;
    public PurchaseInvoiceAttachmentService(JdbcTemplate jdbc,CurrentUserContext context,DesktopAttachmentStore store,PurchaseInvoiceRecordRepository records){this.jdbc=jdbc;this.context=context;this.store=store;this.records=records;}
    private String screen(int type){return switch(type){case 56->"InvfrmPurchaseInvoice";case 57->"InvfrmPurchasedirectInvoice";default->throw new IllegalArgumentException("Unsupported invoice attachment form");};}
    public List<Map<String,Object>> list(int id,int type){
        String name=screen(type);records.require(id,type);
        return jdbc.queryForList("EXEC dbo.Sp_DMSAttachments_GetAllMethod @ScreenName=?,@Id=?,@Activity='ReadById'",name,id).stream().map(PurchaseInvoiceFinancialRules::copy)
                .filter(r->i(r,"OrganizationId")==context.currentOrganizationId()&&i(r,"CompanyId")==context.currentCompanyId()&&i(r,"RefDocumentTypeId")==type).toList();
    }
    public record Prepared(List<Map<String,Object>> retained,List<Map<String,Object>> added,Set<Integer> removed){
        public String names(){return joined("Attachment");}public String storedNames(){return joined("UploadedFileCustomName");}
        private String joined(String key){return java.util.stream.Stream.concat(retained.stream(),added.stream()).map(r->Objects.toString(r.get(key),"")).collect(Collectors.joining(","));}
    }
    public Prepared prepare(int id,int type,Object supplied){
        screen(type);records.requireRight(type,id>0?"Update":"Save");
        var change=new ObjectMapper().convertValue(supplied,PurchaseInvoiceAttachmentsDto.class);
        if(change==null||change.files==null||change.removeAttachmentIds==null||change.files.size()>10)throw new IllegalArgumentException("Select at most ten files at once");
        var existing=id>0?list(id,type):List.<Map<String,Object>>of();var removed=new HashSet<>(change.removeAttachmentIds);
        for(Integer attachmentId:removed)if(attachmentId==null||existing.stream().noneMatch(r->i(r,"Id")==attachmentId))throw new IllegalArgumentException("Attachment does not belong to this invoice");
        var added=new ArrayList<Map<String,Object>>();
        for(var file:change.files){byte[] bytes=DesktopInventoryItemFileService.decode(file);String stored=store.store(context.requireAccountingUser(),file.name,bytes);added.add(Map.of("Attachment",file.name,"UploadedFileCustomName",stored,"UploadedFileSizeMb",bytes.length/1048576d));}
        return new Prepared(existing.stream().filter(r->!removed.contains(i(r,"Id"))).toList(),added,removed);
    }
    public void persist(int id,int type,int supplier,Prepared change){
        String name=screen(type);
        for(Integer removed:change.removed())ProcExec.call(jdbc,"EXEC dbo.Sp_DMSAttachments_GetAllMethod @Id=?,@Activity='AttachmentDeleteById'",removed);
        Timestamp now=new Timestamp(System.currentTimeMillis());
        for(var row:change.added())ProcExec.call(jdbc,"EXEC dbo.Proc_DMSAttachments_Insert @Id=?,@RefAccountId=?,@DMSFoldersLabelsId=?,@RefDocumentTypeId=?,@RefDocumentNo=?,@Attachment=?,@EntryDate=?,@EntryUser=?,@ModifyDate=?,@ModifyUser=?,@OrganizationId=?,@CompanyId=?,@BranchId=?,@ScreenName=?,@DetailWiseAttachment=?,@UploadedFileCustomName=?,@UploadedFileSizeMb=?,@LineId=?",0,supplier,0,type,id,row.get("Attachment"),now,context.currentUserId(),now,context.currentUserId(),context.currentOrganizationId(),context.currentCompanyId(),context.currentBranchId(),name,false,row.get("UploadedFileCustomName"),row.get("UploadedFileSizeMb"),0);
        // Shared legacy physical files remain intact; the store cleans only new files on rollback.
    }
    public record Download(String name,byte[] bytes){}
    public Download download(int id,int type,int attachment){
        var row=list(id,type).stream().filter(r->i(r,"Id")==attachment).findFirst().orElseThrow(()->new ResponseStatusException(NOT_FOUND,"Attachment not found"));
        String original=s(row,"Attachment"),stored=s(row,"UploadedFileCustomName");return new Download(baseName(original),store.read(context.requireAccountingUser(),baseName(stored.isBlank()?original:stored)));
    }
    private String baseName(String value){String name=value.replace('\\','/');name=name.substring(name.lastIndexOf('/')+1);DesktopAttachmentStore.validateName(name);return name;}
}
