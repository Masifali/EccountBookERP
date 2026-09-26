package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.security.LegacyVpsCredentials;
import java.io.*;
import java.nio.file.*;
import java.time.Duration;
import java.util.*;
import org.apache.commons.net.ftp.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Component;
import org.springframework.transaction.support.*;

/** Original configuration-selected local/VPS storage, with rollback cleanup of NEW files only. */
@Component
public class DesktopAttachmentStore {
    public static final int MAX_BYTES=5*1024*1024;
    private final JdbcTemplate jdbc;private final LegacyVpsCredentials credentials;
    public DesktopAttachmentStore(JdbcTemplate jdbc,LegacyVpsCredentials credentials){this.jdbc=jdbc;this.credentials=credentials;}
    public String configuration(UserAccount u,String name){var rows=jdbc.queryForList("EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @ConfigDescription=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),name,"GetConfigurationByOrgCompandConfigDescription");return rows.isEmpty()?"":Objects.toString(rows.get(0).get("ConfigKey"),"");}
    private boolean remote(UserAccount u){return Set.of("true","1").contains(configuration(u,"IsVpsAttachmentsServiceOn").toLowerCase(Locale.ROOT));}
    public String store(UserAccount u,String original,byte[] bytes){
        validateName(original);if(bytes==null||bytes.length==0||bytes.length>MAX_BYTES)throw new IllegalArgumentException("Attachment must be between 1 byte and 5 MB");
        if(!TransactionSynchronizationManager.isSynchronizationActive())throw new IllegalStateException("Attachment upload requires a transaction");
        String extension=original.contains(".")?original.substring(original.lastIndexOf('.')+1):"";if(!extension.matches("[A-Za-z0-9]{0,16}"))throw new IllegalArgumentException("Unsupported file extension");
        String name="Inv_"+UUID.randomUUID()+(extension.isEmpty()?"":"."+extension);boolean remote=remote(u);Path root=remote?null:localRoot(u);
        try{if(remote)withFtp(ftp->{if(!ftp.storeFile(name,new ByteArrayInputStream(bytes)))throw new IOException();return null;});else Files.write(root.resolve(name),bytes,StandardOpenOption.CREATE_NEW);}
        catch(Exception ex){try{removeNew(remote,root,name);}catch(Exception ignored){}throw new IllegalStateException("Attachment upload failed; no database changes were saved");}
        TransactionSynchronizationManager.registerSynchronization(new TransactionSynchronization(){@Override public void afterCompletion(int status){if(status!=STATUS_COMMITTED)try{removeNew(remote,root,name);}catch(Exception ex){org.slf4j.LoggerFactory.getLogger(DesktopAttachmentStore.class).error("Rollback cleanup failed for newly uploaded attachment {}",name);}}});
        return name;
    }
    public byte[] read(UserAccount u,String stored){
        validateName(stored);
        try{if(remote(u))return withFtp(ftp->{var output=new LimitedOutput(MAX_BYTES);if(!ftp.retrieveFile(stored,output))throw new IOException();return output.toByteArray();});Path file=localRoot(u).resolve(stored).normalize();if(Files.size(file)>MAX_BYTES)throw new IOException();return Files.readAllBytes(file);}
        catch(Exception ex){throw new IllegalStateException("Attachment could not be read from desktop storage");}
    }
    /** Exact-name check used to verify cleanup of a generated upload, never a directory listing API. */
    public boolean containsGenerated(UserAccount u,String name){
        if(name==null||!name.matches("Inv_[0-9a-f-]{36}(\\.[A-Za-z0-9]{1,16})?"))throw new IllegalArgumentException("Invalid generated attachment name");
        try{if(!remote(u))return Files.exists(localRoot(u).resolve(name));return withFtp(ftp->{String[] matches=ftp.listNames(name);if(ftp.getReplyCode()==550)return false;if(!FTPReply.isPositiveCompletion(ftp.getReplyCode()))throw new IOException();return matches!=null&&matches.length>0;});}catch(Exception ex){throw new IllegalStateException("Could not verify uploaded attachment storage");}
    }
    private Path localRoot(UserAccount u){String configured=configuration(u,"Attachment Folder Path");if(configured.isBlank())throw new IllegalStateException("Attachment Folder Path Not Configure Properly");Path root=Path.of(configured).toAbsolutePath().normalize();if(!Files.isDirectory(root))throw new IllegalStateException("Attachment Folder Path Not Configure Properly");return root;}
    private void removeNew(boolean remote,Path root,String name) throws Exception {if(!name.matches("Inv_[0-9a-f-]{36}(\\.[A-Za-z0-9]{1,16})?"))throw new IllegalArgumentException("Invalid generated attachment name");if(remote)withFtp(ftp->{if(!ftp.deleteFile(name))throw new IOException();return null;});else Files.deleteIfExists(root.resolve(name));}
    private interface Transfer<T>{T run(FTPClient ftp)throws IOException;}
    private <T>T withFtp(Transfer<T> action)throws IOException {
        var secret=credentials.read();FTPClient ftp=new FTPClient();ftp.setConnectTimeout(10000);ftp.setDefaultTimeout(15000);ftp.setDataTimeout(Duration.ofSeconds(30));
        try{var endpoint=java.net.URI.create("ftp://"+secret.server());if(endpoint.getHost()==null||endpoint.getUserInfo()!=null)throw new IOException("Invalid VPS host configuration");ftp.connect(endpoint.getHost(),endpoint.getPort()>0?endpoint.getPort():21);if(!FTPReply.isPositiveCompletion(ftp.getReplyCode())||!ftp.login(secret.user(),secret.password()))throw new IOException("VPS login failed");ftp.enterLocalPassiveMode();if(!ftp.setFileType(FTP.BINARY_FILE_TYPE))throw new IOException();if(endpoint.getPath()!=null&&!endpoint.getPath().isBlank()&&!endpoint.getPath().equals("/")&&!ftp.changeWorkingDirectory(endpoint.getPath()))throw new IOException();return action.run(ftp);}finally{if(ftp.isConnected())try{ftp.disconnect();}catch(IOException ignored){}}
    }
    public static void validateName(String name){if(name==null||name.isBlank()||name.length()>500||name.contains("/")||name.contains("\\")||name.contains(":")||name.chars().anyMatch(c->c<32)||name.equals(".")||name.equals(".."))throw new IllegalArgumentException("Invalid attachment filename");}
    private static final class LimitedOutput extends ByteArrayOutputStream {private final int limit;LimitedOutput(int limit){this.limit=limit;}@Override public synchronized void write(byte[] b,int off,int len){if(count+len>limit)throw new IllegalArgumentException("Attachment exceeds 5 MB");super.write(b,off,len);}@Override public synchronized void write(int b){if(count>=limit)throw new IllegalArgumentException("Attachment exceeds 5 MB");super.write(b);}}
}
