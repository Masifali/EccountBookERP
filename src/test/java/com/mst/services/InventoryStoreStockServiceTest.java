package com.mst.services;
import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryStoreStockRequest;
import com.mst.repositories.InventoryStoreStockRepository;
import com.mst.security.*;
import java.math.BigDecimal;
import java.time.LocalDate;
import java.util.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;
class InventoryStoreStockServiceTest {
 @Test void rateChangeUsesDesktopMidpointRoundingAndConfigurationGate(){
  var repo=mock(InventoryStoreStockRepository.class);var context=mock(CurrentUserContext.class);var rights=mock(DesktopReportRights.class);var u=new UserAccount();when(context.requireAccountingUser()).thenReturn(u);when(repo.branches(u)).thenReturn(List.of(Map.of("BranchId",58)));when(repo.manual(u)).thenReturn(true);when(repo.years(u)).thenReturn(List.of(Map.of("Id",1)));var service=new InventoryStoreStockService(repo,context,rights);
  var r=new InventoryStoreStockRequest();r.fromDate=LocalDate.of(2026,1,1);r.toDate=LocalDate.of(2026,9,16);r.branchIds=List.of(58);r.parentIds=List.of(7,8);var edit=new InventoryStoreStockRequest.Rate();edit.itemId=1;edit.packUomId=2;edit.packingTypeId=3;edit.conditionId=4;r.rates=List.of(edit);
  Map<String,Object> row=new HashMap<>(Map.of("ItemId",1,"PackUomId",2,"PackingTypeId",3,"ItemConditionId",4,"BalWeight","100","ManualRate","0"));when(repo.load(u,r)).thenReturn(List.of(row));
  edit.rate=new BigDecimal("0.6");service.update(r);verify(repo).saveRate(u,1,r,row,new BigDecimal("0.6"));clearInvocations(repo);
  row.put("ManualRate","1.5");edit.rate=new BigDecimal("2.5");assertThrows(IllegalArgumentException.class,()->service.update(r));verify(repo,never()).saveRate(any(),anyInt(),any(),any(),any());
  edit.rate=new BigDecimal("3.5");service.update(r);verify(repo).saveRate(u,1,r,row,new BigDecimal("3.5"));clearInvocations(repo);
  edit.rate=new BigDecimal("4294967297");assertThrows(IllegalArgumentException.class,()->service.update(r));verify(repo,never()).saveRate(any(),anyInt(),any(),any(),any());
  when(repo.manual(u)).thenReturn(false);edit.rate=new BigDecimal("5");assertThrows(IllegalArgumentException.class,()->service.update(r));verify(repo,never()).saveRate(any(),anyInt(),any(),any(),any());
 }
}
