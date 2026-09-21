package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryPosItemRequest;
import com.mst.models.dto.InventoryGeneralItemRequest;
import com.mst.repositories.DesktopInventoryItemRepository;
import java.io.*;
import java.nio.charset.StandardCharsets;
import java.sql.Timestamp;
import java.util.*;
import javax.imageio.ImageIO;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

@Service
public class DesktopInventoryItemFileService {
    private final DesktopAttachmentStore store;private final JdbcTemplate jdbc;private final DesktopInventoryItemRepository repo;
    public DesktopInventoryItemFileService(DesktopAttachmentStore store,JdbcTemplate jdbc,DesktopInventoryItemRepository repo){this.store=store;this.jdbc=jdbc;this.repo=repo;}
    public List<Map<String,Object>> prepareImages(UserAccount u,InventoryGeneralItemRequest r){
        List<Map<String,Object>> existing=r.Id>0?jdbc.queryForList("EXEC dbo.Sp_Item_GetAllMethod @Id=?, @Activity=?",r.Id,"ItemImagesByItemId"):List.of();
        if(r.Id>0)repo.record(u,r.Id);
        if(r.images==null)return existing;
        if(r.images.size()>4)throw new IllegalArgumentException("The desktop form supports four item images");
        if(!r.images.isEmpty()&&r.imageDate==null)throw new IllegalArgumentException("Select the image effected date");
        var slots=new HashSet<Integer>();var rows=new ArrayList<Map<String,Object>>();
        for(var image:r.images){
            if(image==null||image.sortNo<1||image.sortNo>4||!slots.add(image.sortNo))throw new IllegalArgumentException("Select one image for each slot");
            String fileName,path;
            if(image.upload!=null){byte[] bytes=decode(image.upload);imageType(bytes);if(image.upload.name.length()>150)throw new IllegalArgumentException("Image filename exceeds 150 characters");fileName=image.upload.name;path=store.store(u,fileName,bytes);}
            else{var old=existing.stream().filter(row->((Number)row.get("Id")).intValue()==image.id).findFirst().orElseThrow(()->new IllegalArgumentException("Image does not belong to this item"));fileName=Objects.toString(old.get("FileName"),"");path=Objects.toString(old.get("ImagePath"),"");}
            rows.add(new LinkedHashMap<>(Map.of("Id",0,"ImagePath",path,"FileName",fileName,"SortNo",image.sortNo,"EffetedDate",Timestamp.valueOf(r.imageDate),"IsProfileImage",image.sortNo==1,"IsActive",false)));
        }
        return rows;
    }
    public void persist(UserAccount u,int item,int type,InventoryGeneralItemRequest r){
        if(r.files==null||r.removeAttachmentIds==null||r.files.size()>10)throw new IllegalArgumentException("At most ten attachments may be uploaded at once");
        var existing=attachments(u,item);
        for(Integer removed:new LinkedHashSet<>(r.removeAttachmentIds)){
            if(removed==null||existing.stream().noneMatch(a->((Number)a.get("Id")).intValue()==removed))throw new IllegalArgumentException("Attachment does not belong to this item");
            jdbc.update("EXEC dbo.Sp_DMSAttachments_GetAllMethod @Id=?, @Activity=?",removed,"AttachmentDeleteById");
            // Keep existing physical objects: legacy filenames can be shared across forms.
            // Only files created by the current failed transaction are physically cleaned up.
        }
        for(var file:r.files){byte[] bytes=decode(file);String stored=store.store(u,file.name,bytes);Timestamp now=new Timestamp(System.currentTimeMillis());
            Object[] values={0,type,0,0,item,file.name,now,u.getId(),now,u.getId(),u.getOrganizationId(),u.getCompanyId(),0,"InvDefrmAddItem",false,stored,bytes.length/1048576d,0};
            jdbc.execute("EXEC dbo.Proc_DMSAttachments_Insert @Id=?, @RefAccountId=?, @DMSFoldersLabelsId=?, @RefDocumentTypeId=?, @RefDocumentNo=?, @Attachment=?, @EntryDate=?, @EntryUser=?, @ModifyDate=?, @ModifyUser=?, @OrganizationId=?, @CompanyId=?, @BranchId=?, @ScreenName=?, @DetailWiseAttachment=?, @UploadedFileCustomName=?, @UploadedFileSizeMb=?, @LineId=?",(org.springframework.jdbc.core.PreparedStatementCallback<Void>) s->{for(int i=0;i<values.length;i++)s.setObject(i+1,values[i]);boolean result=s.execute();while(true){if(result){try(var rs=s.getResultSet()){while(rs.next()){/* drain SCOPE_IDENTITY and any subsequent SQL errors */}}}else if(s.getUpdateCount()==-1)break;result=s.getMoreResults();}return null;});
        }
    }
    private List<Map<String,Object>> attachments(UserAccount u,int item){repo.record(u,item);return jdbc.queryForList("EXEC dbo.Sp_DMSAttachments_GetAllMethod @ScreenName=?, @Id=?, @Activity=?","InvDefrmAddItem",item,"ReadById").stream().filter(r->Objects.equals(r.get("OrganizationId"),u.getOrganizationId())&&Objects.equals(r.get("CompanyId"),u.getCompanyId())).toList();}
    public Download download(UserAccount u,int item,int attachment){var row=attachments(u,item).stream().filter(r->((Number)r.get("Id")).intValue()==attachment).findFirst().orElseThrow(()->new org.springframework.web.server.ResponseStatusException(org.springframework.http.HttpStatus.NOT_FOUND,"Attachment not found"));String stored=Objects.toString(row.get("UploadedFileCustomName"),"");if(stored.isBlank())stored=Objects.toString(row.get("Attachment"),"");return new Download(basename(Objects.toString(row.get("Attachment"),stored)),store.read(u,basename(stored)),"application/octet-stream");}
    public Download image(UserAccount u,int item,int image){repo.record(u,item);var row=jdbc.queryForList("EXEC dbo.Sp_Item_GetAllMethod @Id=?, @Activity=?",item,"ItemImagesByItemId").stream().filter(r->((Number)r.get("Id")).intValue()==image).findFirst().orElseThrow(()->new org.springframework.web.server.ResponseStatusException(org.springframework.http.HttpStatus.NOT_FOUND,"Image not found"));String path=basename(Objects.toString(row.get("ImagePath"),""));byte[] bytes=store.read(u,path);return new Download(basename(Objects.toString(row.get("FileName"),path)),bytes,imageType(bytes));}
    public static byte[] decode(InventoryPosItemRequest.Upload file){if(file==null)throw new IllegalArgumentException("Invalid attachment");DesktopAttachmentStore.validateName(file.name);if(file.base64==null||file.base64.length()>7*1024*1024)throw new IllegalArgumentException("Attachment exceeds 5 MB");byte[] bytes;try{bytes=Base64.getDecoder().decode(file.base64);}catch(IllegalArgumentException ex){throw new IllegalArgumentException("Invalid attachment content");}if(bytes.length==0||bytes.length>DesktopAttachmentStore.MAX_BYTES)throw new IllegalArgumentException("Attachment must be between 1 byte and 5 MB");return bytes;}
    public static String imageType(byte[] bytes){try(var stream=ImageIO.createImageInputStream(new ByteArrayInputStream(bytes))){var readers=ImageIO.getImageReaders(stream);if(!readers.hasNext())throw new IllegalArgumentException("Select a valid product image");var reader=readers.next();try{reader.setInput(stream);if((long)reader.getWidth(0)*reader.getHeight(0)>20_000_000)throw new IllegalArgumentException("Product image dimensions are too large");return switch(reader.getFormatName().toLowerCase(Locale.ROOT)){case "jpg","jpeg"->"image/jpeg";case "png"->"image/png";case "gif"->"image/gif";case "bmp"->"image/bmp";default->throw new IllegalArgumentException("Unsupported product image type");};}finally{reader.dispose();}}catch(IOException ex){throw new IllegalArgumentException("Select a valid product image");}}
    private static String basename(String name){String normalized=name.replace('\\','/');String result=normalized.substring(normalized.lastIndexOf('/')+1);DesktopAttachmentStore.validateName(result);return result;}
    public record Download(String name,byte[] bytes,String type){}
}
