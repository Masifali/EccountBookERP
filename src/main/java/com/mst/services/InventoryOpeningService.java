package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryOpeningRequest;
import com.mst.repositories.*;
import com.mst.security.*;
import java.math.BigDecimal;
import java.time.LocalDate;
import java.util.*;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.*;

@Service
public class InventoryOpeningService {
    private final InventoryOpeningRepository repo;
    private final InventoryOpeningWriter writer;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;
    private final InventoryOpeningAttachmentService files;
    public InventoryOpeningService(InventoryOpeningRepository repo,InventoryOpeningWriter writer,CurrentUserContext context,DesktopReportRights rights,InventoryOpeningAttachmentService files){this.repo=repo;this.writer=writer;this.context=context;this.rights=rights;this.files=files;}
    private UserAccount user(String action){UserAccount u=context.requireAccountingUser();rights.require(u,92,action);return u;}
    public Map<String,Object> lookups(){UserAccount u=user("View");Map<String,Object> r=repo.lookups(u);r.put("defaults",defaults(u));Map<String,Boolean> actions=new LinkedHashMap<>();for(String a:List.of("Save","Update","Print","Grid Print","Grid Export")){try{rights.require(u,92,a);actions.put(a,true);}catch(org.springframework.security.access.AccessDeniedException ex){actions.put(a,false);}}r.put("permissions",actions);return r;}
    public Map<String,Object> defaults(){return defaults(user("View"));}
    private Map<String,Object> year(UserAccount u){List<Map<String,Object>> years=repo.years(u);if(years.isEmpty())throw new IllegalStateException("No active financial year is allocated to this company");return years.get(0);}
    private Map<String,Object> defaults(UserAccount u){Map<String,Object> y=year(u);return Map.of("docNo",repo.nextCode(u,((Number)y.get("Id")).intValue()),"docDate",LocalDate.parse(y.get("Start_Period").toString().substring(0,10)).minusDays(1),"financialYearId",y.get("Id"));}
    public List<Map<String,Object>> history(InventoryOpeningRequest.History r){UserAccount u=user("View");return repo.history(u,((Number)year(u).get("Id")).intValue(),r);}
    public List<Map<String,Object>> historyChoices(){return repo.historyChoices(user("View"));}
    public Map<String,Object> record(int id){return repo.record(user("View"),id);}
    public List<Map<String,Object>> attachments(int id){return repo.attachments(user("View"),id);}
    public DesktopInventoryItemFileService.Download download(int id,int attachment){return files.download(user("View"),id,attachment);}
    public List<Map<String,Object>> uoms(int item){return repo.uoms(user("View"),item);}
    @Transactional(isolation=Isolation.SERIALIZABLE)
    public Map<String,Object> save(InventoryOpeningRequest r){
        UserAccount u=user(r.id==0?"Save":"Update");validate(r);
        if(u.getBranchesId()==null||u.getBranchesId()<=0)throw new IllegalStateException("No branch is allocated to this user");
        Map<String,Object> old=r.id==0?null:repo.record(u,r.id);
        Map<String,Object> choices=repo.lookups(u);
        selected(choices,"projects",r.projectsId);selected(choices,"warehouses",r.warehouseId);selected(choices,"items",r.itemId);selected(choices,"lots",r.jobLotId);selected(choices,"packingTypes",r.packingTypeId);selected(choices,"accounts",r.stockCrGLAcId);
        List<Map<String,Object>> units=repo.uoms(u,r.itemId);
        if(units.stream().noneMatch(x->number(x.get("Id"))==r.itemUomSch)||units.stream().noneMatch(x->number(x.get("Id"))==r.rateUomSch))throw new IllegalArgumentException("Select a UOM assigned to this item");
        @SuppressWarnings("unchecked") List<Map<String,Object>> crops=(List<Map<String,Object>>)choices.get("crops");
        if(crops.stream().noneMatch(x->r.cropYear.equals(Objects.toString(x.get("CropYear"),""))))throw new IllegalArgumentException("Select a crop from the company list");
        if(old!=null&&Boolean.TRUE.equals(old.get("IsApproved")))throw new IllegalArgumentException("Record Not Update because Record has approved");
        Map<String,Object> y=year(u);int yearId=old==null?number(y.get("Id")):number(old.get("FinancialYearId"));
        LocalDate date=LocalDate.parse((old==null?y.get("Start_Period"):old.get("DocDate")).toString().substring(0,10));if(old==null)date=date.minusDays(1);
        int code=old==null?repo.nextCode(u,yearId):number(old.get("DocNo"));
        var attachments=files.prepare(u,r);
        var header=old==null?new LinkedHashMap<String,Object>():new LinkedHashMap<>(old);
        header.put("AttachmentsValues",attachments.names());header.put("CustomAttachmentsValues",attachments.storedNames());
        int id=writer.save(u,yearId,date,code,r,header);
        files.persist(u,id,r.itemId,attachments);
        return repo.record(u,id);
    }
    private static int number(Object v){return v instanceof Number?((Number)v).intValue():0;}
    @SuppressWarnings("unchecked") private static void selected(Map<String,Object> choices,String key,int id){if(((List<Map<String,Object>>)choices.get(key)).stream().noneMatch(x->number(x.get("Id"))==id))throw new IllegalArgumentException("Selected "+key+" record is not available to this company/user");}
    public static void validate(InventoryOpeningRequest r){
        if(r.id<0)throw new IllegalArgumentException("Invalid record ID");
        required(r.projectsId,"Project");required(r.warehouseId,"WareHouseName");required(r.itemId,"Item");required(r.itemUomSch,"UOM");
        if(r.cropYear==null||r.cropYear.isBlank())throw new IllegalArgumentException("CropYear Field Required");
        required(r.jobLotId,"JobLot");required(r.stockCrGLAcId,"Stock Credit Account");nonzero(r.qty,"Quantity");nonzero(r.weightKgs,"Weight");nonzero(r.itemRate,"Rate");required(r.rateUomSch,"Rate UOM");nonzero(r.itemAmount,"Amount");required(r.packingTypeId,"PackingType");
        if(!List.of("Stocks","Sales").contains(Objects.toString(r.transactionType,"")))throw new IllegalArgumentException("Transaction Type Field Required");
        if(r.remarks!=null&&r.remarks.length()>500)throw new IllegalArgumentException("Remarks cannot exceed 500 characters");
    }
    private static void required(int id,String name){if(id<=0)throw new IllegalArgumentException(name+" Field Required");}
    private static void nonzero(BigDecimal n,String name){if(n==null||n.signum()==0||!Double.isFinite(n.doubleValue()))throw new IllegalArgumentException(name+" Field Required");}
}
