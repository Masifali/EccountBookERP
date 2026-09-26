package com.mst.repositories;

import com.mst.models.dto.PurchaseOrderFullDto;
import com.mst.repositories.support.ProcExec;
import java.sql.Types;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.SqlParameterValue;
import org.springframework.stereotype.Repository;

/** PurchsaeOrder.cs:3353-3401; Inventory.PurchaseOrderDetail's non-virtual CLR properties. */
@Repository
public class PurchaseOrderDetailRepository {
    private final JdbcTemplate jdbc;
    public PurchaseOrderDetailRepository(JdbcTemplate jdbc) { this.jdbc=jdbc; }
    public int save(int orderId, PurchaseOrderFullDto.PurchaseOrderDetailItemDto line, PurchaseOrderFullDto order) {
        Map<String,Object> p=new LinkedHashMap<>();
        for (String key : List.of("Amount","BagPrice","BagWeight","NetWeight","OrderItemQty","OrderItemRate",
                "RetailRate","TaxAmount","TaxPercent","TotalAmount","WeightPerQty")) p.put(key,0d);
        for (String key : List.of("Id","JobLotId","LineId","OrderItemId","OrderItemRateUOMId","OrderItemUOMId",
                "PurchaseOrderId","TaxNameId","RefDocumentTypeId","RefDocId","RefDocInvoiceId","CityId")) p.put(key,0);
        for (String key : List.of("AmountCalcType","CityArea","Crop","LabSampleNo","Moisture","OrderRemarks","TaxableStatus"))
            p.put(key,new SqlParameterValue(Types.NVARCHAR,null));
        p.put("InvLabSampleAnalysisHeaderId",n(line.getLabSampleId()));
        p.put("LabAnalysisStandardScheduleId",n(line.getLabAnalysisStandardScheduleId()));
        p.put("CurrencyId",n(order.getCurrencyId())); p.put("ExchangeRate",d(order.getExchangeRate())); p.put("FcyAmount",d(line.getFcyAmount()));
        p.put("PackingDate",new SqlParameterValue(Types.TIMESTAMP,null)); p.put("ExpiryDate",new SqlParameterValue(Types.TIMESTAMP,null));
        p.put("CropYearId",n(line.getCropYearId())); p.put("LeadTime",0); p.put("WeightCapacity",new SqlParameterValue(Types.NVARCHAR,null));
        p.put("Id",n(line.getPurchaseOrderDetailId())); p.put("PurchaseOrderId",orderId);
        p.put("OrderItemId",n(line.getItemId())); p.put("OrderItemUOMId",n(line.getPackUomId()));
        p.put("OrderItemRateUOMId",n(line.getRateUomId())); p.put("OrderItemQty",d(line.getItemQty()));
        p.put("NetWeight",d(line.getItemWeight())); p.put("OrderItemRate",d(line.getItemRate())); p.put("Amount",d(line.getItemAmount()));
        p.put("JobLotId",n(line.getJobLotId())); p.put("CityId",n(line.getLoadingLocationCityId()));
        p.put("CityArea",Objects.toString(line.getLoadingLocationCityName(),"")); p.put("Crop",Objects.toString(line.getCropYear(),""));
        p.put("LabSampleNo",Objects.toString(line.getLabSampleNo(),"")); p.put("OrderRemarks",Objects.toString(line.getRemarks(),""));
        p.put("Moisture",Objects.toString(line.getMoisturePercent(),""));
        String sql="EXEC dbo.Sp_PurchaseOrderDetail_Insert "+String.join(",",p.keySet().stream().map(key->"@"+key+"=?").toList());
        Integer id=ProcExec.call(jdbc,sql,p.values().toArray());
        if (n(line.getPurchaseOrderDetailId())>0) return line.getPurchaseOrderDetailId();
        if (id==null || id<=0) throw new IllegalStateException("Purchase Order detail save returned no Id");
        return id;
    }
    private static int n(Integer n) { return n==null?0:n; }
    private static double d(Double n) { return n==null?0d:n; }
}
