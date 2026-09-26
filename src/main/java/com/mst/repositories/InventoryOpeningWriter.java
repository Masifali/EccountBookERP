package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryOpeningRequest;
import java.sql.*;
import java.time.LocalDate;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.PreparedStatementCallback;
import org.springframework.stereotype.Repository;

/** DAL SetData's sequence; caller owns the transaction. No direct table mutations. */
@Repository
public class InventoryOpeningWriter {
    private final JdbcTemplate jdbc;
    public InventoryOpeningWriter(JdbcTemplate jdbc){this.jdbc=jdbc;}
    public int save(UserAccount u,int year,LocalDate date,int code,InventoryOpeningRequest r,Map<String,Object> old) {
        Timestamp now=new Timestamp(System.currentTimeMillis());
        Map<String,Object> p=InventoryOpeningDefaults.opening();
        p.putAll(Map.of("Id",r.id,"OrganizationId",u.getOrganizationId(),"CompanyId",u.getCompanyId(),"FinancialYearId",year,"BranchesId",u.getBranchesId(),"DocumentTypeId",40,"DocNo",code,"DocDate",java.sql.Date.valueOf(date)));
        p.putAll(Map.of("ProjectsId",r.projectsId,"WarehouseId",r.warehouseId,"ItemId",r.itemId,"ItemUomSch",r.itemUomSch,"JobLotId",r.jobLotId,"PackingTypeId",r.packingTypeId,"StockCrGLAcId",r.stockCrGLAcId,"RateUomSch",r.rateUomSch));
        p.putAll(Map.of("Qty",r.qty.doubleValue(),"WeightKgs",r.weightKgs.doubleValue(),"ItemRate",r.itemRate.doubleValue(),"ItemAmount",r.itemAmount.doubleValue(),"CropYear",r.cropYear,"TransactionType",r.transactionType,"Remarks",Objects.toString(r.remarks,"")));
        p.putAll(Map.of("EntryUserId",r.id==0?u.getId():0,"ModifyUserId",r.id==0?0:u.getId(),"EntryDate",now,"ModifyDate",now,"ApprovedDate",now,"ApprovedUserId",u.getId()));
        // Preserve fields owned by other desktop forms and existing attachments on edit.
        if(old!=null)for(String key:List.of("AttachmentsValues","CustomAttachmentsValues","ItemVariantId","WorkOrderId","BatchNo","ProductionStageId","CastingTypeId","BaseDocumentTypeId","ItemConditionId","RackId","SecondaryUomId","SecondaryUomQty","SecondaryUomItemRate"))if(old.get(key)!=null)p.put(key,old.get(key));
        int id=scalar("Sp_InvStockOpeningBalanceHeader_"+(r.id==0?"Insert":"Update"),p,r.id);
        if(id<=0)throw new IllegalStateException("Opening stock did not return a record ID");
        Map<String,Object> v=InventoryOpeningDefaults.voucher();
        v.putAll(Map.of("OrganizationId",u.getOrganizationId(),"CompanyId",u.getCompanyId(),"FinancialYearId",year,"DocumentTypeId",40,"DocumentTypeSrNo",id,"RefDocNoId",r.id,"VoucherCode",code,"VoucherDate",java.sql.Date.valueOf(date)));
        v.putAll(Map.of("BranchId",u.getBranchesId(),"ProjectId",r.projectsId,"EntryUser",r.id==0?u.getId():0,"ModifyUser",r.id==0?0:u.getId(),"EntryDate",now,"ModifyDate",now,"ChequeDate",java.sql.Date.valueOf(LocalDate.now()),"DueDate",java.sql.Date.valueOf(date),"ManualBillNo",r.transactionType));
        v.put("Remarks",Objects.toString(r.remarks,""));v.put("RemarksOtherLingo","");
        int voucherId=0;
        if(r.id>0){List<Map<String,Object>> existing=jdbc.queryForList("EXEC dbo.Sp_Vouchers_GetMethods @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @DocumentTypeSrNo=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),40,id,"GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId");if(!existing.isEmpty())voucherId=((Number)existing.get(0).get("Id")).intValue();}
        v.put("Id",voucherId);
        voucherId=scalar("Sp_VoucherHead_"+(voucherId==0?"Insert":"Update"),v,voucherId);v.put("Id",voucherId);
        // Company access was checked through vItemAllocation, as in ReadAllItems.
        Map<String,Object> item=jdbc.queryForMap("SELECT ItemName,PurchaseGLAC FROM dbo.Item WHERE Id=? AND OrganizationId=?",r.itemId,u.getOrganizationId());
        List<Map<String,Object>> lots=jdbc.queryForList("EXEC dbo.SP_JobLot_ReadMethod @Id=?, @Activity=?",r.jobLotId,"GetById");
        int debit=lots.isEmpty()||!(lots.get(0).get("AccountId") instanceof Number)?0:((Number)lots.get(0).get("AccountId")).intValue();
        if(debit<=0)debit=((Number)item.get("PurchaseGLAC")).intValue();
        String comments=Objects.toString(r.remarks,"")+"   Item: "+item.get("ItemName")+",   Qty: "+r.qty.stripTrailingZeros().toPlainString()+",   Rate:"+r.itemRate.stripTrailingZeros().toPlainString()+",   Amount:"+r.itemAmount.stripTrailingZeros().toPlainString();
        List<Map<String,Object>> details=new ArrayList<>();
        for(int n=0;n<2;n++) {
            Map<String,Object> d=InventoryOpeningDefaults.detail();
            d.putAll(Map.of("VoucherHeadId",voucherId,"AccountId",n==0?debit:r.stockCrGLAcId,"AgainstAccountId",n==0?r.stockCrGLAcId:debit,"Comments",comments,"DebitAmount",n==0?r.itemAmount.doubleValue():0d,"CreditAmount",n==1?r.itemAmount.doubleValue():0d,"ItemRate",r.itemRate.doubleValue(),"QtyIn",r.qty.doubleValue(),"WeightIn",r.weightKgs.doubleValue(),"ItemAmount",r.itemAmount.doubleValue()));
            scalar("Sp_VoucherDetail_Insert",d,0);details.add(d);
        }
        int audit=scalar("Sp_VoucherHead_H_Insert",v,0);
        for(Map<String,Object> d:details){d.put("DocumentTypeIdRef",audit);scalar("Sp_VoucherDetail_H_Insert",d,0);}
        Map<String,Object> stock=InventoryOpeningDefaults.transactions();stock.putAll(Map.of("OrganizationId",u.getOrganizationId(),"CompanyId",u.getCompanyId(),"RefDocumentTypeId",40,"RefDocIdNo",id));scalar("Sp_InventoryTransactions_GetALLMethod",stock,0);
        stock=InventoryOpeningDefaults.evaluation();stock.putAll(Map.of("OrganizationId",u.getOrganizationId(),"CompanyId",u.getCompanyId(),"RefDocumentTypeId",40,"RefDocIdNo",id));scalar("Sp_InventoryStockEvalautionDetail_Update",stock,0);
        return id;
    }
    private int scalar(String procedure,Map<String,Object> values,int fallback) {
        String sql="EXEC dbo."+procedure+" "+String.join(", ",values.keySet().stream().map(k->"@"+k+"=?").toList());
        return jdbc.execute(sql,(PreparedStatementCallback<Integer>) statement->{
            int i=1;for(Object value:values.values())statement.setObject(i++,value);
            boolean result=statement.execute();Integer first=null;
            // Drain every result so an error after SELECT @Id cannot accidentally commit.
            while(true){if(result){try(ResultSet rs=statement.getResultSet()){if(first==null&&rs.next()&&rs.getObject(1) instanceof Number)first=((Number)rs.getObject(1)).intValue();}}else if(statement.getUpdateCount()==-1)break;result=statement.getMoreResults();}
            return first!=null&&first>0?first:fallback;
        });
    }
}
