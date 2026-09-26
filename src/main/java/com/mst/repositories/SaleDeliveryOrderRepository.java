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
        var rows = jdbc.queryForList("EXEC dbo.Sp_InvDeliveryOrder_GetAllMethod @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @FinancialYearId=?, @BranchesId=?, @Activity='GenerateCode'",
                u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, financialYearId, u.getBranchesId());
        return rows.isEmpty() ? 1 : number(rows.get(0).get("DocNo"));
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
        return jdbc.queryForList("EXEC dbo.Sp_InvDeliveryOrder_GetAllMethod @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @FinancialYearId=?, @BranchesId=?, @CanViewAllRecord=?, @EntryUser=?, @NoOfRecords=?, @Activity='FormHistory'",
                u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, financialYearId, u.getBranchesId(), canViewAll, u.getId(), 500);
    }

    public Map<String,Object> record(UserAccount u, int id) {
        var heads = jdbc.queryForList("SELECT * FROM dbo.InvDeliveryOrder WHERE Id=? AND OrganizationId=? AND CompanyId=? AND DocumentTypeId=? AND ActionId<>3",
                id, u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID);
        if (heads.size() != 1) throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Delivery order not found in this company");
        Map<String,Object> result = new LinkedHashMap<>(heads.get(0));
        result.put("lines", jdbc.queryForList("EXEC dbo.Sp_InvDeliveryOrder_GetAllMethod @Id=?, @Activity='ReadByIdDetailId'", id));
        result.put("expenses", jdbc.queryForList("EXEC dbo.Sp_InvDeliveryOrder_GetAllMethod @Id=?, @Activity='ReadInvDeliveryOrderExpensesByHeaderId'", id));
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
        validate(u, r);
        Map<String,Object> old = r.id > 0 ? record(u, r.id) : null;
        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        double totalQty = r.lines.stream().mapToDouble(x -> x.quantity).sum();
        double netWeight = r.lines.stream().mapToDouble(x -> x.weight).sum();
        double packingWeight = r.lines.stream().mapToDouble(x -> x.packingWeight).sum();
        double grossWeight = r.lines.stream().mapToDouble(x -> x.grossWeight).sum();
        String procedure = r.id > 0 ? "Sp_InvDeliveryOrder_Update" : "Sp_InvDeliveryOrder_Insert";
        String sql = "EXEC dbo." + procedure + " @Id=?, @DocumentTypeId=?, @DocNo=?, @DocDate=?, @DoTotalQty=?, @LoadingInstructions=?, @IsApproved=?, @EntryDate=?, @EntryUser=?, @ModifyDate=?, @ModifyUser=?, @OrganizationId=?, @CompanyId=?, @BranchesId=?, @GrossWeight=?, @NetWeight=?, @DeliveryOrderType=?, @VehicleNo=?, @VehicleType=?, @FinancialYearId=?, @ActionId=?, @ToBranchId=?, @PackingWeight=?, @ScreenName=?, @SaleTypeId=?, @FromBranchId=?, @TransporterId=?, @IsStockReserved=?";
        Object docNo = old == null ? 0 : old.get("DocNo");
        int savedId = execute(sql, new Object[]{r.id, DOCUMENT_TYPE_ID, docNo, r.docDate, totalQty, text(r.loadingInstructions), false,
                old == null ? now : old.get("EntryDate"), old == null ? u.getId() : old.get("EntryUser"), now, u.getId(), u.getOrganizationId(), u.getCompanyId(), u.getBranchesId(),
                grossWeight, netWeight, "Local", text(r.vehicleNo), text(r.vehicleType), financialYearId, r.id > 0 ? 2 : 1, r.toBranchId, packingWeight,
                "DeliveryOrder", r.saleTypeId, u.getBranchesId(), zeroToNull(r.transporterId), r.stockReserved}, r.id);
        if (savedId <= 0) throw new IllegalStateException("Delivery order save returned no record ID");
        for (Integer removedId : new LinkedHashSet<>(r.removedLineIds)) if (removedId != null && removedId > 0) saveLine(u, savedId, removedId, null, 3);
        for (SaleDeliveryOrderRequest.Line line : r.lines) saveLine(u, savedId, line.id, line, line.id > 0 ? 2 : 1);
        return record(u, savedId);
    }

    public void delete(UserAccount u, int id) {
        Map<String,Object> row = record(u, id);
        if (Boolean.TRUE.equals(row.get("IsApproved"))) throw new IllegalArgumentException("Record has been approved");
        execute("EXEC dbo.USP_RecoredRemoveByOrgCompDocAndByID @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @Id=?, @UserId=?",
                new Object[]{u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, id, u.getId()}, id);
    }

    private void saveLine(UserAccount u, int headerId, int lineId, SaleDeliveryOrderRequest.Line x, int action) {
        if (action == 3) {
            execute("EXEC dbo.Sp_InvDeliveryOrderDetail_Insert @Id=?, @InvDeliveryOrderId=?, @ActionTypeId=?, @ModifyUserId=?",
                    new Object[]{lineId, headerId, 3, u.getId()}, lineId);
            return;
        }
        execute("EXEC dbo.Sp_InvDeliveryOrderDetail_Insert @Id=?, @InvDeliveryOrderId=?, @SupplierCustomerId=?, @SaleOrderId=?, @ItemId=?, @PackUomId=?, @InvPackingTypeId=?, @DoQty=?, @DoWeight=?, @LoadingQty=?, @LoadingWeight=?, @WarehouseId=?, @LoadingRemarks=?, @JobLotId=?, @GrossWeight=?, @PackingWeight=?, @TotalPackingWeight=?, @SaleOrderDetailId=?, @RefPartyId=?, @RefDocumentTypeId=?, @RefDocIdNo=?, @RefDocSubIdNo=?, @CropYearId=?, @ActionTypeId=?, @EntryUserId=?, @ModifyUserId=?, @ItemRate=?, @RateUomId=?, @ItemAmount=?, @OuterEbTotal=?",
                new Object[]{lineId, headerId, x.supplierCustomerId, x.saleOrderId, x.itemId, x.packUomId, x.packingTypeId, x.quantity, x.weight, x.quantity, x.weight,
                        x.warehouseId, text(x.remarks), x.jobLotId, x.grossWeight, x.packingUnit, x.packingWeight, x.saleOrderDetailId, x.refPartyId, x.refDocumentTypeId,
                        x.refDocIdNo, x.refDocSubIdNo, x.cropYearId, action, u.getId(), u.getId(), x.rate, x.rateUomId,
                        x.rateUom > 0 ? x.weight / x.rateUom * x.rate : 0d, x.packingWeight}, lineId);
    }

    private void validate(UserAccount u, SaleDeliveryOrderRequest r) {
        if (r.docDate == null) throw new IllegalArgumentException("Document date is required");
        if (r.saleTypeId != 1 && r.saleTypeId != 2) throw new IllegalArgumentException("Select a valid sale type");
        if (r.lines == null || r.lines.isEmpty()) throw new IllegalArgumentException("Grid record not found");
        if (r.id > 0) record(u, r.id);
        for (SaleDeliveryOrderRequest.Line x : r.lines) {
            if (x.saleOrderId <= 0 || x.saleOrderDetailId <= 0) throw new IllegalArgumentException("Sale order number is required on every row");
            if (x.supplierCustomerId <= 0 || x.itemId <= 0 || x.packUomId <= 0 || x.packingTypeId <= 0 || x.warehouseId <= 0 || x.jobLotId <= 0 || x.cropYearId <= 0)
                throw new IllegalArgumentException("Customer, item, UOM, packing type, warehouse, crop and job lot are required on every row");
            if (x.quantity <= 0 || x.weight <= 0 || x.grossWeight <= 0) throw new IllegalArgumentException("Quantity, weight and gross weight must be greater than zero");
        }
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

    private static Integer zeroToNull(int value) { return value > 0 ? value : null; }
    private static String text(String value) { return value == null ? "" : value.trim(); }
    private static int number(Object value) { return value == null ? 0 : Integer.parseInt(value.toString()); }
}
