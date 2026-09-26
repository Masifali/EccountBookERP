package com.mst.repositories;

import com.mst.models.dto.InventoryOpeningRequest;
import java.math.BigDecimal;
import java.nio.file.*;
import java.time.LocalDate;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.jdbc.datasource.DataSourceTransactionManager;
import org.springframework.transaction.support.TransactionTemplate;
import static org.junit.jupiter.api.Assertions.*;

/** Opt-in isolated test record. Always rolled back, including every stock/account/audit side effect. */
@EnabledIfSystemProperty(named="inventory.rollback",matches="true")
class InventoryOpeningRollbackTest {
    private static int id(Object value){return ((Number)value).intValue();}
    @Test void insertLoadUpdateAndAccountingStockEffectsRollbackTogether() throws Exception {
        var jdbc=InventoryOpeningLiveTest.jdbc();var user=InventoryOpeningLiveTest.user(jdbc);var repo=new InventoryOpeningRepository(jdbc);var writer=new InventoryOpeningWriter(jdbc);
        var years=repo.years(user);int year=id(years.get(0).get("Id"));LocalDate date=LocalDate.parse(years.get(0).get("Start_Period").toString().substring(0,10)).minusDays(1);
        // Use a real company record as the source of valid relationships; never modify that record.
        var rows=repo.history(user,year,new InventoryOpeningRequest.History());assertFalse(rows.isEmpty(),"No desktop opening record is available as a relationship fixture");
        var source=rows.stream().filter(x->id(x.get("ProjectsId"))>0&&id(x.get("JobLotId"))>0).findFirst().orElseThrow();
        InventoryOpeningRequest r=new InventoryOpeningRequest();r.projectsId=id(source.get("ProjectsId"));r.warehouseId=id(source.get("WarehouseId"));r.itemId=id(source.get("ItemId"));r.itemUomSch=id(source.get("ItemUomSch"));r.jobLotId=id(source.get("JobLotId"));r.packingTypeId=id(source.get("PackingTypeId"));r.stockCrGLAcId=id(source.get("StockCrGLAcId"));r.rateUomSch=id(source.get("RateUomSch"));r.cropYear=Objects.toString(source.get("CropYear"));r.transactionType="Stocks";r.remarks="CODEX_INVENTORY_ROLLBACK_"+UUID.randomUUID();r.qty=BigDecimal.ONE;r.itemRate=new BigDecimal(source.get("ItemRate").toString());
        var units=repo.uoms(user,r.itemId);BigDecimal pack=new BigDecimal(units.stream().filter(x->id(x.get("Id"))==r.itemUomSch).findFirst().orElseThrow().get("Equivalent").toString());BigDecimal rate=new BigDecimal(units.stream().filter(x->id(x.get("Id"))==r.rateUomSch).findFirst().orElseThrow().get("Equivalent").toString());r.weightKgs=pack;r.itemAmount=r.itemRate.multiply(pack).divide(rate,3,java.math.RoundingMode.HALF_UP);
        assertTrue(r.itemAmount.signum()!=0);int[] created={0};int[] voucher={0};var tx=new TransactionTemplate(new DataSourceTransactionManager(jdbc.getDataSource()));tx.setIsolationLevel(org.springframework.transaction.TransactionDefinition.ISOLATION_SERIALIZABLE);tx.setTimeout(60);
        tx.execute(status->{try{
            created[0]=writer.save(user,year,date,repo.nextCode(user,year),r,null);r.id=created[0];var loaded=repo.record(user,r.id);
            assertEquals(r.remarks,loaded.get("Remarks"));assertEquals(user.getId(),id(loaded.get("EntryUserId")));assertEquals(user.getBranchesId(),id(loaded.get("BranchesId")));
            voucher[0]=jdbc.queryForObject("SELECT Id FROM VoucherHead WHERE OrganizationId=? AND CompanyId=? AND DocumentTypeId=40 AND DocumentTypeSrNo=?",Integer.class,user.getOrganizationId(),user.getCompanyId(),r.id);
            assertEquals(2,jdbc.queryForObject("SELECT COUNT(*) FROM VoucherDetail WHERE VoucherHeadId=?",Integer.class,voucher[0]));
            assertEquals(0d,jdbc.queryForObject("SELECT SUM(DebitAmount-CreditAmount) FROM VoucherDetail WHERE VoucherHeadId=?",Double.class,voucher[0]),0.000001);
            for(String table:List.of("InventoryTransactions","InventoryStockEvalautionDetail")){var stocks=jdbc.queryForList("SELECT StockWeightIn,QtyIn FROM "+table+" WHERE OrganizationId=? AND CompanyId=? AND RefDocumentTypeId=40 AND RefDocIdNo=?",user.getOrganizationId(),user.getCompanyId(),r.id);assertEquals(1,stocks.size());assertEquals(r.weightKgs.doubleValue(),((Number)stocks.get(0).get("StockWeightIn")).doubleValue(),0.000001);}
            r.qty=BigDecimal.valueOf(2);r.weightKgs=pack.multiply(r.qty);r.itemAmount=r.itemAmount.multiply(r.qty);r.remarks+="_UPDATED";
            assertEquals(r.id,writer.save(user,year,date,id(loaded.get("DocNo")),r,loaded));var updated=repo.record(user,r.id);assertEquals(r.remarks,updated.get("Remarks"));assertEquals(user.getId(),id(updated.get("ModifyUserId")));assertEquals(2,jdbc.queryForObject("SELECT COUNT(*) FROM VoucherDetail WHERE VoucherHeadId=?",Integer.class,voucher[0]));
            assertEquals(2,jdbc.queryForObject("SELECT COUNT(*) FROM VoucherHead_H WHERE OrganizationId=? AND CompanyId=? AND DocumentTypeId=40 AND DocumentTypeSrNo=?",Integer.class,user.getOrganizationId(),user.getCompanyId(),r.id));
            return null;
        }finally{status.setRollbackOnly();}});
        assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM InvStockOpeningBalanceHeader WHERE Remarks LIKE ?",Integer.class,r.remarks.replace("_UPDATED","")+"%"));assertEquals(0,jdbc.queryForObject("SELECT COUNT(*) FROM VoucherHead WHERE Id=?",Integer.class,voucher[0]));
        Files.writeString(Path.of("migration/inventory/evidence/opening-stock-rollback.txt"),"Controlled transaction on GoldenAcedb\nUser="+user.getId()+" Organization="+user.getOrganizationId()+" Company="+user.getCompanyId()+" Branch="+user.getBranchesId()+" Year="+year+" Date="+date+"\nInsert/load/update: verified. Two balanced voucher lines, stock transaction, stock evaluation and two voucher history snapshots verified. Transaction rolled back; test opening and voucher absent afterward. No normal business row changed. Delete is not exposed by this desktop form. Native UI comparison is still pending.\n");
    }
}
