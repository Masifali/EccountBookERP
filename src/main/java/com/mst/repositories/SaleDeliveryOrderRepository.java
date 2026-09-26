package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.dto.SaleDeliveryOrderRequest;
import java.sql.Timestamp;
import java.time.LocalDateTime;
import java.util.*;
import org.springframework.http.HttpStatus;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.PreparedStatementCallback;
import org.springframework.stereotype.Repository;
import org.springframework.web.server.ResponseStatusException;

@Repository
public class SaleDeliveryOrderRepository {
    public static final int DOCUMENT_TYPE_ID = 84;
    private final JdbcTemplate jdbc;

    public SaleDeliveryOrderRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    public int nextCode(UserAccount u, int financialYearId) {
        String sql="EXEC dbo.Sp_InvDeliveryOrder_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@Activity='GenerateCode'";
        List<Object> args=new ArrayList<>(Arrays.asList(u.getOrganizationId(),u.getCompanyId(),DOCUMENT_TYPE_ID));
        if(financialYearId!=0){sql+=",@FinancialYearId=?";args.add(financialYearId);}
        if(u.getBranchesId()!=0){sql+=",@BranchesId=?";args.add(u.getBranchesId());}
        var rows=jdbc.queryForList(sql,args.toArray());
        return rows.isEmpty() ? 0 : number(rows.get(0).get("DocNo"));
    }

    public List<Map<String,Object>> pendingOrders(UserAccount u, int financialYearId) {
        return jdbc.queryForList("EXEC dbo.USP_GetPendingSaleOrderByDoAndGdn @OrganizationId=?, @CompanyId=?, @FinancialYearId=?, @BranchesIds=?",
                u.getOrganizationId(), u.getCompanyId(), financialYearId, String.valueOf(u.getBranchesId()));
    }

    public List<Map<String,Object>> orderLines(UserAccount u, int orderId) {
        Integer count = jdbc.queryForObject("SELECT COUNT(*) FROM dbo.SaleOrder WHERE Id=? AND OrganizationId=? AND CompanyId=?", Integer.class,
                orderId, u.getOrganizationId(), u.getCompanyId());
        if (count == null || count == 0) throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Sale order not found in this company");
        List<Integer> ids = jdbc.queryForList("SELECT Id FROM dbo.SaleOrderDetail WHERE SaleOrderId=? AND ActionTypeId<>3 AND ISNULL(IsCanceled,0)=0 ORDER BY Id", Integer.class, orderId);
        if (ids.isEmpty()) return List.of();
        String detailIds = String.join(",", ids.stream().map(String::valueOf).toList());
        List<Map<String,Object>> lines = jdbc.queryForList("EXEC dbo.USP_LoadSaleOrderForDeliveryOrderInDetail @OrganizationId=?, @CompanyId=?, @GdnIds=?",
                u.getOrganizationId(), u.getCompanyId(), detailIds);
        for (Map<String,Object> line : lines) {
            int detailId = number(line.get("SaleOrderDetailId"));
            var labels = jdbc.queryForMap("SELECT d.OrderItemRateUOMId RateUomId,w.WareHouseName,j.JobLotDescription,p.PackTypeDesc FROM dbo.SaleOrderDetail d LEFT JOIN dbo.InvWareHouse w ON w.Id=d.WarehouseId LEFT JOIN dbo.JobLot j ON j.Id=d.JobLotId LEFT JOIN dbo.InvPackingType p ON p.Id=d.PackingTypeID WHERE d.Id=?", detailId);
            line.putAll(labels);
        }
        return lines;
    }

    public List<Map<String,Object>> history(UserAccount u, int financialYearId, boolean canViewAll) {
        // DeliveryOrder.FillHistoryGrid -> BLL InvDeliveryOrder.FormHistoryNew.
        // The BLL does not forward BranchesId; the form leaves NoOfRecords at zero (omitted).
        String sql="EXEC dbo.SP_DeliveryOrderFormHistory @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@CanViewAllRecord=?,@DeliveryOrderType='Local'";
        List<Object> args=new ArrayList<>(Arrays.asList(u.getOrganizationId(),u.getCompanyId(),DOCUMENT_TYPE_ID,canViewAll));
        if(financialYearId!=0){sql+=",@FinancialYearId=?";args.add(financialYearId);}
        if(!canViewAll){sql+=",@EntryUser=?";args.add(u.getId());}
        var rows=jdbc.queryForList(sql,args.toArray());
        // Keep the procedure's columns and provide aliases consumed by the existing Java page.
        for(var row:rows){
            row.put("SupplierCustomer",row.get("CustomerName"));row.put("DoQty",row.get("LoadingQty"));
            row.put("DoWeight",row.get("LoadingWeight"));row.put("IsApproved",row.get("ApprovalStatus"));
            row.put("EntryUserName",row.get("EntryUser"));
        }
        return rows;
    }

    public boolean canViewAllRecords(UserAccount u,String role) {
        if("Admin".equalsIgnoreCase(role))return true;
        return jdbc.queryForList("EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?,@ScreenName='DeliveryOrder',@RightName=?,@CompanyId=?,@Activity='GetByUserId'",
                u.getId(),role==null?"":role,u.getCompanyId()).stream()
                .anyMatch(r->"CanView AllRecord".equalsIgnoreCase(Objects.toString(r.get("RightName"),"").trim())
                        && (Boolean.TRUE.equals(r.get("Value"))||"1".equals(Objects.toString(r.get("Value"),""))||"true".equalsIgnoreCase(Objects.toString(r.get("Value"),""))));
    }

    public Map<String,Object> record(UserAccount u, int id) {
        var heads = jdbc.queryForList("EXEC dbo.Sp_InvDeliveryOrder_GetAllMethod @Id=?,@Activity='ReadById'",id);
        if (heads.size() != 1 || number(heads.get(0).get("OrganizationId"))!=u.getOrganizationId()
                || number(heads.get(0).get("CompanyId"))!=u.getCompanyId()
                || number(heads.get(0).get("DocumentTypeId"))!=DOCUMENT_TYPE_ID)
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Delivery order not found in this company");
        Map<String,Object> result = new LinkedHashMap<>(heads.get(0));
        boolean export="Export".equals(result.get("DeliveryOrderType"));
        result.put("lines", jdbc.queryForList("EXEC dbo.Sp_InvDeliveryOrder_GetAllMethod @Id=?, @Activity=?", id,
                export?"ReadByIdDetailIdExport":"ReadByIdDetailId"));
        result.put("expenses", jdbc.queryForList("EXEC dbo.Sp_InvDeliveryOrder_GetAllMethod @Id=?, @Activity=?", id,
                export?"ReadInvDeliveryOrderExpensesByHeaderIdForExport":"ReadInvDeliveryOrderExpensesByHeaderId"));
        return result;
    }

    /** DeliveryOrder.cs:530 BranchList = CommonServices.BrancheServiceBind() = Branches.GetAll:
        Sp_Branches_GetAllMethod @OrganizationId,@CompanyId,@Activity='GetAll' - every branch of the company, not only the
        user's allocated ones. The form then sets the combo to UserAccount.BranchesId (:654) and on edit to the record's
        BranchesId (:2038). BranchId is repeated from Id for the page script; UserBranch marks the default row. */
    public List<Map<String,Object>> branches(UserAccount u) {
        List<Map<String,Object>> out = new ArrayList<>();
        for (Map<String,Object> r : jdbc.queryForList("EXEC dbo.Sp_Branches_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity='GetAll'",
                u.getOrganizationId(), u.getCompanyId())) {
            Map<String,Object> x = new LinkedHashMap<>(r);
            x.put("BranchId", r.get("Id"));
            Object id = r.get("Id");
            x.put("UserBranch", id instanceof Number && ((Number) id).intValue() == u.getBranchesId());
            out.add(x);
        }
        return out;
    }

    /** DeliveryOrder.cs:527 VehicleType.GetAll() - EXEC Sp_VehicleType_GetAllMethod with no parameters (the BLL builds an
        @Activity list and never passes it, 0611:13-23). Was a raw SELECT on dbo.VehicleType. */
    public List<Map<String,Object>> vehicleTypes(UserAccount u) {
        return jdbc.queryForList("EXEC dbo.Sp_VehicleType_GetAllMethod");
    }

    public Map<String,Object> save(UserAccount u, int financialYearId, SaleDeliveryOrderRequest r) {
        Map<String,Object> old = r.id > 0 ? record(u, r.id) : null;
        Map<Integer,SaleDeliveryOrderRequest.Line> savedLines=savedLines(old);
        String orderType=text(r.deliveryOrderType);
        if(orderType.isEmpty())orderType=old==null?"Local":Objects.toString(old.get("DeliveryOrderType"),"Local");
        // Export uses its own desktop form and model mappings; this local form cannot rewrite it.
        if(old!=null&&"Export".equals(old.get("DeliveryOrderType")))
            throw new IllegalArgumentException("Open this record in the Export Delivery Order form");
        if(!"Local".equals(orderType)&&!"StockTransfer".equals(orderType))
            throw new IllegalArgumentException("Select a valid delivery order type");
        boolean stockTransfer="StockTransfer".equals(orderType);
        int branchId=r.branchesId!=null?r.branchesId:old==null?u.getBranchesId():number(old.get("BranchesId"));
        validate(r,savedLines,stockTransfer,branchId);
        var branchList=branches(u);
        if(branchList.stream().noneMatch(branch->number(branch.get("Id"))==branchId)
                ||(stockTransfer&&branchList.stream().noneMatch(branch->number(branch.get("Id"))==r.toBranchId)))
            throw new IllegalArgumentException("Select a branch from this company");
        List<SaleDeliveryOrderRequest.Expense> expenses=expensesForSave(r,old);
        validateExpenses(r,expenses);
        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        java.math.BigDecimal totalQty=java.math.BigDecimal.ZERO;
        double netWeight=0,packingWeight=0,grossWeight=0;
        for(var line:r.lines){
            totalQty=totalQty.add(new java.math.BigDecimal(line.quantity,new java.math.MathContext(15,java.math.RoundingMode.HALF_EVEN)));
            netWeight+=line.weight;packingWeight+=line.packingWeight;grossWeight+=line.grossWeight;
        }
        String procedure = r.id > 0 ? "Sp_InvDeliveryOrder_Update" : "Sp_InvDeliveryOrder_Insert";
        String sql = "EXEC dbo." + procedure + " @Id=?, @DocumentTypeId=?, @DocNo=?, @DocDate=?, @DoTotalQty=?, @LoadingInstructions=?, @IsApproved=?, @EntryDate=?, @EntryUser=?, @ModifyDate=?, @ModifyUser=?, @OrganizationId=?, @CompanyId=?, @BranchesId=?, @GrossWeight=?, @NetWeight=?, @DeliveryOrderType=?, @VehicleNo=?, @VehicleType=?, @FinancialYearId=?, @ActionId=?, @ToBranchId=?, @PackingWeight=?, @ScreenName=?, @SaleTypeId=?, @FromBranchId=0, @TransporterId=?, @IsStockReserved=?,@AttachmentsValues=?,@CustomAttachmentsValues=?";
        // Unset non-nullable properties in the native Local/StockTransfer header model are zero, not SQL NULL.
        sql+=",@ApprovedUser=0,@ProjectsId=0,@EximInvoiceId=0,@OtherWeight=0,@LoadingportId=0,@DepartmentFromId=0,@DepartmentToId=0,@RequestedByLookUpId=0,@ApprovedByLookUpId=0";
        Object docNo = old == null ? 0 : old.get("DocNo");
        int savedId = execute(sql, new Object[]{r.id, DOCUMENT_TYPE_ID, docNo, r.docDate, totalQty, Objects.toString(r.loadingInstructions,""), old!=null&&Boolean.TRUE.equals(old.get("IsApproved")),
                now, u.getId(), now, u.getId(), u.getOrganizationId(), u.getCompanyId(), branchId,
                grossWeight, netWeight, orderType, text(r.vehicleNo), text(r.vehicleType), financialYearId, r.id > 0 ? 2 : 1, r.toBranchId, packingWeight,
                "DeliveryOrder", r.saleTypeId, r.transporterId, r.stockReserved,
                old==null?null:old.get("AttachmentsValues"),old==null?null:old.get("CustomAttachmentsValues")}, r.id);
        if (savedId <= 0) throw new IllegalStateException("Delivery order save returned no record ID");
        for (Integer removedId : new LinkedHashSet<>(r.removedLineIds)) if (removedId != null && removedId > 0)
            saveLine(savedId,savedLines.get(removedId),3,stockTransfer);
        for (SaleDeliveryOrderRequest.Line line : r.lines) saveLine(savedId,line,line.id > 0 ? 2 : 1,stockTransfer);
        // Sp_InvDeliveryOrder_Update removes the old expense rows; the desktop DAL reinserts the entire grid.
        for(var expense:expenses)
            execute("EXEC dbo.Sp_InvDeliveryOrderExpense_Insert @Id=?,@SaleOrderId=?,@SaleOrderCustomerExpId=?,@InvDeliveryOrderId=?,@ItemId=?,@Qty=?,@Remarks=?,@ExImInvoiceId=0,@InvoiceOtherItemDetailId=0,@WeightPerQty=0,@NetWeight=0",
                    new Object[]{expense.id,expense.saleOrderId,expense.saleOrderCustomerExpId,savedId,expense.itemId,expense.quantity,Objects.toString(expense.remarks,"")},0);
        execute("EXEC [DAW].[USp_DocumentApprovalDetail_Insert] @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@Id=?,@LimitAmount=?",
                new Object[]{u.getOrganizationId(),u.getCompanyId(),DOCUMENT_TYPE_ID,savedId,java.math.BigDecimal.ZERO},0);
        return record(u, savedId);
    }

    private List<SaleDeliveryOrderRequest.Expense> expensesForSave(SaleDeliveryOrderRequest r,Map<String,Object> old){
        if(r.expenses!=null)return r.expenses;
        List<SaleDeliveryOrderRequest.Expense> expenses=new ArrayList<>();
        if(old!=null&&old.get("expenses") instanceof List<?> rows)for(Object row:rows){
            if(!(row instanceof Map<?,?> x))continue;
            var expense=new SaleDeliveryOrderRequest.Expense();
            expense.id=number(x.get("Id"));expense.saleOrderId=number(x.get("SaleOrderId"));
            expense.saleOrderCustomerExpId=number(x.get("SaleOrderCustomerExpId"));expense.itemId=number(x.get("ItemId"));
            expense.quantity=x.get("Qty") instanceof Number n?n.doubleValue():0;
            expense.remarks=Objects.toString(x.get("Remarks"),"");expenses.add(expense);
        }
        return expenses;
    }

    private void validateExpenses(SaleDeliveryOrderRequest r,List<SaleDeliveryOrderRequest.Expense> expenses){
        int row=0;
        for(var expense:expenses){
            row++;
            if(expense==null)throw new IllegalArgumentException("Expense row "+row+" is required");
            if(expense.saleOrderId>0&&r.lines.stream().noneMatch(line->line.saleOrderId==expense.saleOrderId))
                throw new IllegalArgumentException("Order Id in row No "+row+" in Expense Grid does not exist in Detail Grid");
        }
    }

    public void delete(UserAccount u, int id) {
        Map<String,Object> row = record(u, id);
        if (Boolean.TRUE.equals(row.get("IsApproved"))) throw new IllegalArgumentException("Record has been approved");
        execute("EXEC dbo.USP_RecoredRemoveByOrgCompDocAndByID @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @Id=?, @UserId=?",
                new Object[]{u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, id, u.getId()}, id);
    }

    private void saveLine(int headerId, SaleDeliveryOrderRequest.Line x, int action, boolean stockTransfer) {
        // GenericProvider.SetProc sends non-virtual InvDeliveryOrderdetail properties only.
        // Rate/RateUom are virtual display fields; the original procedure resolves the sale-order price.
        Map<String,Object> p=new LinkedHashMap<>();
        p.put("Id",x.id);p.put("InvDeliveryOrderId",headerId);p.put("ActionTypeId",action);
        p.put("SupplierCustomerId",x.supplierCustomerId);p.put("SaleOrderId",stockTransfer?0:x.saleOrderId);
        p.put("SaleOrderDetailId",stockTransfer?0:x.saleOrderDetailId);
        p.put("DeliveryScheduleId",stockTransfer?0:number(x.deliveryScheduleId));
        p.put("DeliveryScheduleDetailId",stockTransfer?0:number(x.deliveryScheduleDetailId));
        p.put("ItemId",x.itemId);p.put("PackUomId",x.packUomId);p.put("InvPackingTypeId",x.packingTypeId);
        p.put("DoQty",x.quantity);p.put("LoadingQty",x.quantity);p.put("DoWeight",x.weight);p.put("LoadingWeight",x.weight);
        p.put("WarehouseId",x.warehouseId);p.put("JobLotId",x.jobLotId);p.put("CropYearId",x.cropYearId);
        p.put("LoadingRemarks",Objects.toString(x.remarks,""));p.put("GrossWeight",x.grossWeight);
        p.put("PackingWeight",x.packingUnit);p.put("OuterEbTotal",x.packingWeight);
        // Deleted rows use FillDetailListCommonForInsertAndDelete only; Insert sets this total on live rows.
        p.put("TotalPackingWeight",action==3?0d:x.packingWeight);
        p.put("RefPartyId",x.refPartyId);p.put("RefDocumentTypeId",x.refDocumentTypeId);
        p.put("RefDocIdNo",x.refDocIdNo);p.put("RefDocSubIdNo",x.refDocSubIdNo);
        for(String field:List.of("StockWeight","OtherWeight","InnerQty","InnerUomId","InnerEbUnit","InnerEbTotal","AccessWtSet"))p.put(field,0d);
        for(String field:List.of("CastingTypeId","ItemVariantId","InvoiceDetailId","WareHouseToId","ToJobLotId","ExImInvoiceId",
                "BagTypeId","ContainerId","DeliveryTypeId","AssetId","ThirdPartyAnalysisSubId","ThirdPartyAnalysisId"))p.put(field,0);
        p.put("IsAssetItem",false);
        p.put("ContainerRemarks",null);p.put("InspectionRemarks",null);p.put("ItemDiscription",null);
        execute("EXEC dbo.Sp_InvDeliveryOrderDetail_Insert "+String.join(",",p.keySet().stream().map(key->"@"+key+"=?").toList()),p.values().toArray(),x.id);
    }

    private Map<Integer,SaleDeliveryOrderRequest.Line> savedLines(Map<String,Object> old) {
        Map<Integer,SaleDeliveryOrderRequest.Line> lines=new LinkedHashMap<>();
        if(old!=null&&old.get("lines") instanceof List<?> rows)for(Object row:rows){
            if(!(row instanceof Map<?,?> m))continue;
            var x=new SaleDeliveryOrderRequest.Line();
            x.id=number(m.get("Id"));x.supplierCustomerId=number(m.get("SupplierCustomerId"));
            x.saleOrderId=number(m.get("SaleOrderId"));x.saleOrderDetailId=number(m.get("SaleOrderDetailId"));
            x.deliveryScheduleId=number(m.get("DeliveryScheduleId"));x.deliveryScheduleDetailId=number(m.get("DeliveryScheduleDetailId"));
            x.itemId=number(m.get("ItemId"));x.packUomId=number(m.get("PackUomId"));x.packingTypeId=number(m.get("InvPackingTypeId"));
            x.warehouseId=number(m.get("WarehouseId"));x.jobLotId=number(m.get("JobLotId"));x.cropYearId=number(m.get("CropYearId"));
            x.refPartyId=number(m.get("RefPartyId"));x.refDocumentTypeId=number(m.get("RefDocumentTypeId"));
            x.refDocIdNo=number(m.get("RefDocIdNo"));x.refDocSubIdNo=number(m.get("RefDocSubIdNo"));
            x.quantity=decimal(m.get("LoadingQty"));x.weight=decimal(m.get("LoadingWeight"));
            x.packingUnit=decimal(m.get("PackingWeight"));x.packingWeight=decimal(m.get("OuterEbTotal"));x.grossWeight=decimal(m.get("GrossWeight"));
            x.remarks=Objects.toString(m.get("LoadingRemarks"),"");lines.put(x.id,x);
        }
        return lines;
    }

    private void validate(SaleDeliveryOrderRequest r,Map<Integer,SaleDeliveryOrderRequest.Line> savedLines,boolean stockTransfer,int branchId) {
        if (r.docDate == null) throw new IllegalArgumentException("Document date is required");
        if(branchId<=0)throw new IllegalArgumentException("Branch From is required");
        if(stockTransfer&&(r.toBranchId<=0||r.toBranchId==branchId))
            throw new IllegalArgumentException("Select a Branch To different from Branch From");
        if (r.saleTypeId != 1 && r.saleTypeId != 2) throw new IllegalArgumentException("Select a valid sale type");
        if (r.lines == null || r.lines.isEmpty()) throw new IllegalArgumentException("Grid record not found");
        if(r.lines.stream().anyMatch(Objects::isNull))throw new IllegalArgumentException("Detail row is required");
        if(r.removedLineIds==null)r.removedLineIds=new ArrayList<>();
        if(r.id==0&&(r.lines.stream().anyMatch(x->x.id>0)||r.removedLineIds.stream().anyMatch(id->id!=null&&id>0)))
            throw new IllegalArgumentException("Record cannot be inserted because ActionTypeId not equal to 1");
        Set<Integer> removedIds=new HashSet<>(r.removedLineIds),updatedIds=new HashSet<>();
        for(Integer id:removedIds)if(id!=null&&id>0&&!savedLines.containsKey(id))
            throw new IllegalArgumentException("Deleted detail row does not belong to this delivery order");
        for (SaleDeliveryOrderRequest.Line x : r.lines) {
            if(x.id>0&&(!savedLines.containsKey(x.id)||removedIds.contains(x.id)||!updatedIds.add(x.id)))
                throw new IllegalArgumentException("Invalid or duplicate detail row for this delivery order");
            var saved=savedLines.get(x.id);
            if(x.deliveryScheduleId==null)x.deliveryScheduleId=saved==null?0:saved.deliveryScheduleId;
            if(x.deliveryScheduleDetailId==null)x.deliveryScheduleDetailId=saved==null?0:saved.deliveryScheduleDetailId;
            if (!stockTransfer&&(x.saleOrderId <= 0 || x.saleOrderDetailId <= 0)) throw new IllegalArgumentException("Sale order number is required on every row");
            if (x.supplierCustomerId <= 0 || x.itemId <= 0 || x.packUomId <= 0 || x.packingTypeId <= 0 || x.warehouseId <= 0 || x.jobLotId <= 0 || x.cropYearId <= 0)
                throw new IllegalArgumentException("Customer, item, UOM, packing type, warehouse, crop and job lot are required on every row");
            if (x.quantity <= 0 || x.weight <= 0 || x.grossWeight <= 0) throw new IllegalArgumentException("Quantity, weight and gross weight must be greater than zero");
        }
        boolean scheduleExists=savedLines.values().stream().anyMatch(x->number(x.deliveryScheduleDetailId)>0)
                ||r.lines.stream().anyMatch(x->number(x.deliveryScheduleId)>0||number(x.deliveryScheduleDetailId)>0);
        if(!stockTransfer&&scheduleExists&&r.lines.stream().anyMatch(x->number(x.deliveryScheduleId)<=0||number(x.deliveryScheduleDetailId)<=0))
            throw new IllegalArgumentException("Delivery schedule and schedule detail are required on every row");
    }

    private int execute(String sql, Object[] values, int fallback) {
        return jdbc.execute(sql, (PreparedStatementCallback<Integer>) statement -> {
            for (int i=0;i<values.length;i++) statement.setObject(i+1, values[i]);
            int saved=fallback; boolean result=statement.execute();
            while (true) {
                if (result) try (var rows=statement.getResultSet()) { while(rows.next()) if(rows.getObject(1) instanceof Number n && n.intValue()>0) saved=n.intValue(); }
                else if (statement.getUpdateCount()==-1) break;
                result=statement.getMoreResults();
            }
            return saved;
        });
    }

    private static double decimal(Object value) { return value instanceof Number n?n.doubleValue():value==null?0:Double.parseDouble(value.toString()); }
    private static String text(String value) { return value == null ? "" : value.trim(); }
    private static int number(Object value) { return value == null ? 0 : Integer.parseInt(value.toString()); }
}
