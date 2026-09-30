package com.mst.repositories;

import com.mst.models.SaleGdnReportFilter;
import com.mst.models.UserAccount;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

/** frmGDNHistory -> Architecture.BLL.Inventory.InvGdn; no schema or procedure changes. */
@Repository
public class SaleGdnReportRepository {
    private final JdbcTemplate jdbc;
    public SaleGdnReportRepository(JdbcTemplate jdbc){this.jdbc=jdbc;}

    public List<Map<String,Object>> branches(UserAccount u){
        return jdbc.queryForList("EXEC dbo.USP_GetBranchsAllocatedToUserFromGdn @OrganizationId=?,@CompanyId=?,@UserId=?,@DocumentTypeId=86",
                u.getOrganizationId(),u.getCompanyId(),u.getId());
    }
    public List<Map<String,Object>> lookups(UserAccount u,String branches){
        String sql="EXEC dbo.USP_GetDataForDropDownFromGdn @OrganizationId=?,@CompanyId=?";
        if(branches==null||branches.isBlank())return jdbc.queryForList(sql,u.getOrganizationId(),u.getCompanyId());
        return jdbc.queryForList(sql+",@BranchesIds=?",u.getOrganizationId(),u.getCompanyId(),branches);
    }
    public String yearStart(UserAccount u,int year){
        var rows=jdbc.queryForList("EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?,@CompanyId=?",u.getOrganizationId(),u.getCompanyId());
        return rows.stream().filter(r->number(r.get("Id"))==year).map(r->String.valueOf(r.get("Start_Period")).substring(0,10)).findFirst().orElse("");
    }
    public List<Map<String,Object>> history(UserAccount u,SaleGdnReportFilter f,String branches){
        // BLL InvGdn_History:506-674 omits every unset numeric filter; dates and branches come from the form.
        StringBuilder sql=new StringBuilder("EXEC dbo.Sp_InvGdn_History @OrganizationId=?,@CompanyId=?,@GrnDateF=?,@GrnDateT=?");
        List<Object> args=new ArrayList<>(Arrays.asList(u.getOrganizationId(),u.getCompanyId(),f.fromDate().toString(),f.toDate().toString()));
        add(sql,args,"GrnNoF",f.fromNo());add(sql,args,"GrnNoT",f.toNo());add(sql,args,"SupplierCustomerId",f.customerId());
        add(sql,args,"InventoryParentCategoriesIds",f.parentCategoryId());add(sql,args,"ItemTypeId",f.itemTypeId());
        add(sql,args,"ItemClassGroupId",f.itemClassId());add(sql,args,"ItemCategoryId",f.categoryId());add(sql,args,"ItemId",f.itemId());
        add(sql,args,"CropYear",f.cropYearId());add(sql,args,"JobLotId",f.jobLotId());add(sql,args,"WarehouseId",f.warehouseId());
        add(sql,args,"OrderNoFrom",f.orderNoFrom());add(sql,args,"OrderNoTo",f.orderNoTo());
        sql.append(",@BranchesIds=?");args.add(branches);
        return jdbc.queryForList(sql.toString(),args.toArray()).stream().map(SaleGdnReportRepository::displayRow).toList();
    }
    private static void add(StringBuilder sql,List<Object> args,String key,int value){if(value!=0){sql.append(",@").append(key).append("=?");args.add(value);}}
    public static Map<String,Object> displayRow(Map<String,Object> source){
        // The form's dtGrid column order and aliases at frmGDNHistory:446-483.
        String[][] fields={{"Id","Id"},{"BranchName","BranchName"},{"DocDate","DocDate"},{"DocNo","DocNo"},
            {"PartyName","CompanyName"},{"PartyReference","SupplierReference"},{"GpNo","GpNo"},{"GPDate","GpDate"},
            {"VehicleNo","VehicleNo"},{"BiltyNo","BiltyNo"},{"ReferenceDocNo","ReferenceDocNo"},{"OrderNo","SoOrderNo"},
            {"OrderDate","SoDate"},{"ItemId","ItemId"},{"ItemCode","ItemCode"},{"ItemName","ItemName"},
            {"PackingType","PackTypeDesc"},{"CropYear","CropYear"},{"JobLot","JobLotDescription"},{"ItemQty","ItemQty"},
            {"PackUom","UOMCode"},{"GrossWeight","GrossWeight"},{"EBWPerUnit","EBWPerUnit"},{"EBWTotal","EBWTotal"},
            {"WtCutTotal","WtCutTotal"},{"AdLsWeight","AdLsWeight"},{"StockWeight","StockWeight"},{"NetBillWeight","NetBillWeight"},
            {"WareHouseName","WareHouseName"},{"LabReportReference","LabReportRef"},{"FreightAmount","NetPaid"},
            {"CityName","AreaCity"},{"EntryUser","UserNameEusr"},{"RemarksDetail","CommentsDetail"}};
        Map<String,Object> row=new LinkedHashMap<>();for(var field:fields)row.put(field[0],source.get(field[1]));return row;
    }
    private static int number(Object value){return value instanceof Number n?n.intValue():0;}
}
