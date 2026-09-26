package com.mst.repositories;

import com.mst.repositories.support.ProcExec;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

/** Preserve loaded grids that Sp_PurchaseOrder_Update deletes before the DAL writes them back. */
@Repository
public class PurchaseOrderSupplementRepository {
    private final JdbcTemplate jdbc;
    public PurchaseOrderSupplementRepository(JdbcTemplate jdbc) { this.jdbc=jdbc; }
    public record Snapshot(List<Map<String,Object>> lab, List<Map<String,Object>> dispatch) {}

    public Snapshot beforeUpdate(int id) {
        if (id<=0) return new Snapshot(List.of(),List.of());
        Integer advances=jdbc.queryForObject("SELECT COUNT(*) FROM dbo.AdvancePaymentAdjustment WHERE PurchaseOrderId=?",Integer.class,id);
        if (advances!=null && advances>0) throw new IllegalArgumentException(
                "This order has advance-payment adjustments. Update it in the desktop application until advance-payment editing is available here.");
        return new Snapshot(read(id,"ReadPurchaseOrderLabDeductionByHeaderId"),
                read(id,"ReadByHeaderId_PurchaseOrderSupplierDispatchDetail"));
    }
    private List<Map<String,Object>> read(int id,String activity) {
        return jdbc.queryForList("EXEC dbo.Sp_PurchaseOrder_GetAllMethod @Id=?,@Activity=?",id,activity);
    }
    public void restore(int id,Snapshot snapshot) {
        for (var row:snapshot.lab()) {
            write("Sp_PurchaseOrderLabDeduction_Insert",id,row,
                    List.of("DeductionValue","RangeFrom","RangeTo","StandardValue","WeightKgs","AnalysisParameterId",
                            "Id","InvLabAnalysisStandardDeductionPolicyHeaderId","ItemId","PurchaseOrderDetailId","PurchaseOrderId","DeductFrom"));
        }
        for (var row:snapshot.dispatch()) {
            write("USP_PurchaseOrderSupplierDispatchDetail_Insert",id,row,
                    List.of("Amount","ItemQty","ItemRate","NetWeight","CropYearId","Id","ItemId","ItemUOMId",
                            "PurchaseOrderId","RateUOMId","SupplierDispatchId","LoadingDetailId","PackingTypeId"));
        }
    }
    private void write(String proc,int id,Map<String,Object> row,List<String> names) {
        Object[] values=names.stream().map(name -> "PurchaseOrderId".equals(name) ? id : row.get(name)).toArray();
        ProcExec.call(jdbc,"EXEC dbo."+proc+" "+String.join(",",names.stream().map(name->"@"+name+"=?").toList()),values);
    }
}
