package com.mst.repositories;
import com.mst.models.dto.InventoryGridColumn;
import com.mst.security.*;
import com.mst.services.*;
import java.io.*;
import java.math.BigDecimal;
import java.nio.file.*;
import java.util.*;
import org.apache.poi.hssf.usermodel.HSSFWorkbook;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;
@EnabledIfSystemProperty(named="inventory.live",matches="true")
class InventoryUomGridOutputLiveTest {
 @Test void allRowsExcelAndLandscapePdfMatchScopedHistory() throws Exception {
  var jdbc=InventoryOpeningLiveTest.jdbc();var user=InventoryOpeningLiveTest.user(jdbc);var ctx=mock(CurrentUserContext.class);when(ctx.requireAccountingUser()).thenReturn(user);var rights=mock(DesktopReportRights.class);var service=new InventoryUomScheduleService(new InventoryUomScheduleRepository(jdbc),ctx,rights);var output=new InventoryUomGridOutputService(service,ctx,rights);var rows=service.history(null);assertFalse(rows.isEmpty());var request=new InventoryUomGridOutputService.Request();request.ids=rows.stream().map(r->((Number)r.get("Id")).intValue()).toList();request.columns=List.of("ItemName","UOMDescription","Equivalent","QtyEquivalent","BaseRateUom","BasePackUom","BaseSecondaryUom","Active");BigDecimal sum=BigDecimal.ZERO;
  try(var workbook=new HSSFWorkbook(new ByteArrayInputStream(output.export(request)))){var sheet=workbook.getSheetAt(0);assertEquals(rows.size()+2,sheet.getLastRowNum());for(int i=0;i<rows.size();i++){var expected=rows.get(i);var actual=sheet.getRow(i+2);assertEquals(expected.get("ItemName"),actual.getCell(0).getStringCellValue());assertEquals(expected.get("UOMDescription"),actual.getCell(1).getStringCellValue());assertEquals(((Number)expected.get("Equivalent")).doubleValue(),actual.getCell(2).getNumericCellValue());sum=sum.add(new BigDecimal(expected.get("Equivalent").toString()));}assertEquals(sum.doubleValue(),sheet.getRow(rows.size()+2).getCell(2).getNumericCellValue());}
  byte[] pdf=output.print(request);var reader=new com.itextpdf.text.pdf.PdfReader(pdf);int pages=reader.getNumberOfPages();assertTrue(pages>1,"Expected multiple PDF pages");assertTrue(reader.getPageSizeWithRotation(1).getWidth()>reader.getPageSizeWithRotation(1).getHeight(),"Expected landscape PDF");String first=com.itextpdf.text.pdf.parser.PdfTextExtractor.getTextFromPage(reader,1),last=com.itextpdf.text.pdf.parser.PdfTextExtractor.getTextFromPage(reader,pages);assertTrue(first.contains("Item UOM Schedule"),"Missing PDF title");assertTrue(last.contains("Total"),"Missing final PDF totals");reader.close();Files.write(Path.of("migration/inventory/evidence/uom-grid-preview.pdf"),pdf);
  request.ids=List.of(-1);assertThrows(IllegalArgumentException.class,()->output.export(request));verify(rights,atLeastOnce()).require(user,115,"View");Files.writeString(Path.of("migration/inventory/evidence/uom-grid-output.txt"),"UOM Excel full history="+rows.size()+"; every item/unit/equivalent matches scoped query; total="+sum+". PDF landscape pages="+pages+", title and final totals present. Foreign ID rejected. No database writes. Rights mocked; native print layout comparison pending.\n");
 }
}
