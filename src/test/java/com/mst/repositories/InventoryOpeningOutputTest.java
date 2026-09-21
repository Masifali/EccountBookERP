package com.mst.repositories;
import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryOpeningRequest;
import com.mst.security.*;
import com.mst.services.*;
import java.io.*;
import java.util.*;
import org.apache.poi.hssf.usermodel.HSSFWorkbook;
import org.apache.poi.ss.usermodel.CellType;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;
class InventoryOpeningOutputTest {
 @Test void exportPreservesSelectedRowsColumnsTypesAndRejectsForeignIds() throws Exception {
  var ctx=mock(CurrentUserContext.class);var user=new UserAccount();when(ctx.requireAccountingUser()).thenReturn(user);var rights=mock(DesktopReportRights.class);var opening=mock(InventoryOpeningService.class);var service=new InventoryOpeningOutputService(opening,ctx,rights);var r=new InventoryOpeningOutputService.Request();r.filters=new InventoryOpeningRequest.History();r.ids=List.of(8,7);r.columns=List.of("Remarks","ItemAmount","DocNo");
  when(opening.history(r.filters)).thenReturn(List.of(Map.of("Id",7,"Remarks","first","ItemAmount",12.5,"DocNo","00007"),Map.of("Id",8,"Remarks","=HYPERLINK(\"https://invalid\")","ItemAmount",18.25,"DocNo","00008")));
  try(var book=new HSSFWorkbook(new ByteArrayInputStream(service.export(r)))){var sheet=book.getSheetAt(0);assertEquals(3,sheet.getLastRowNum());assertEquals("Remarks",sheet.getRow(1).getCell(0).getStringCellValue());assertEquals(CellType.STRING,sheet.getRow(2).getCell(0).getCellType());assertEquals("=HYPERLINK(\"https://invalid\")",sheet.getRow(2).getCell(0).getStringCellValue());assertEquals(18.25,sheet.getRow(2).getCell(1).getNumericCellValue());assertEquals("00008",sheet.getRow(2).getCell(2).getStringCellValue());assertEquals("first",sheet.getRow(3).getCell(0).getStringCellValue());assertTrue(sheet.getPrintSetup().getLandscape());}
  r.ids=List.of(999);assertThrows(IllegalArgumentException.class,()->service.export(r));r.ids=List.of(7,7);assertThrows(IllegalArgumentException.class,()->service.export(r));r.ids=List.of(7);r.columns=List.of("OrganizationId");assertThrows(IllegalArgumentException.class,()->service.export(r));verify(rights,atLeastOnce()).require(user,92,"Grid Export");
 }
 @Test @org.junit.jupiter.api.condition.EnabledIfSystemProperty(named="inventory.live",matches="true") void exportMatchesGoldenAcedbHistory() throws Exception {
  var jdbc=InventoryOpeningLiveTest.jdbc();var user=InventoryOpeningLiveTest.user(jdbc);var repo=new InventoryOpeningRepository(jdbc);var ctx=mock(CurrentUserContext.class);when(ctx.requireAccountingUser()).thenReturn(user);var rights=mock(DesktopReportRights.class);var opening=new InventoryOpeningService(repo,mock(InventoryOpeningWriter.class),ctx,rights,mock(InventoryOpeningAttachmentService.class));var filters=new InventoryOpeningRequest.History();var rows=opening.history(filters);assertFalse(rows.isEmpty());var request=new InventoryOpeningOutputService.Request();request.filters=filters;request.columns=List.of("DocNo","ItemName","WeightKgs","ItemAmount","DocDate");request.ids=rows.stream().map(r->((Number)r.get("Id")).intValue()).toList();
  try(var book=new HSSFWorkbook(new ByteArrayInputStream(new InventoryOpeningOutputService(opening,ctx,rights).export(request)))){var sheet=book.getSheetAt(0);assertEquals(rows.size()+1,sheet.getLastRowNum());for(int i=0;i<rows.size();i++){var expected=rows.get(i);var actual=sheet.getRow(i+2);assertEquals(((Number)expected.get("DocNo")).doubleValue(),actual.getCell(0).getNumericCellValue());assertEquals(expected.get("ItemName"),actual.getCell(1).getStringCellValue());assertEquals(((Number)expected.get("WeightKgs")).doubleValue(),actual.getCell(2).getNumericCellValue());assertEquals(((Number)expected.get("ItemAmount")).doubleValue(),actual.getCell(3).getNumericCellValue());assertEquals(((java.util.Date)expected.get("DocDate")).getTime(),actual.getCell(4).getDateCellValue().getTime());}}
  java.nio.file.Files.writeString(java.nio.file.Path.of("migration/inventory/evidence/opening-grid-export.txt"),"Opening history Excel export: "+rows.size()+" GoldenAcedb rows; document code, item, weight, amount and date match the scoped original-procedure result. Selected columns and row order verified. No database writes. Rights mocked in this test; browser/native output comparison remains pending.\n");
 }
 @Test void deniedOutputNeverQueriesHistory(){var ctx=mock(CurrentUserContext.class);var user=new UserAccount();when(ctx.requireAccountingUser()).thenReturn(user);var rights=mock(DesktopReportRights.class);var opening=mock(InventoryOpeningService.class);var service=new InventoryOpeningOutputService(opening,ctx,rights);doThrow(new org.springframework.security.access.AccessDeniedException("Denied")).when(rights).require(user,92,"Grid Export");assertThrows(org.springframework.security.access.AccessDeniedException.class,()->service.export(new InventoryOpeningOutputService.Request()));verifyNoInteractions(opening);doThrow(new org.springframework.security.access.AccessDeniedException("Denied")).when(rights).require(user,92,"Grid Print");assertThrows(org.springframework.security.access.AccessDeniedException.class,service::printRight);}
}
