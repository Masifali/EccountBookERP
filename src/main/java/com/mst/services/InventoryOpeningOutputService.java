package com.mst.services;
import com.mst.models.dto.InventoryOpeningRequest;
import com.mst.security.*;
import java.io.*;
import java.util.*;
import org.apache.poi.hssf.usermodel.HSSFWorkbook;
import org.apache.poi.ss.usermodel.*;
import org.springframework.stereotype.Service;
@Service
public class InventoryOpeningOutputService {
 public static class Request {public InventoryOpeningRequest.History filters;public List<Integer> ids;public List<String> columns;public String title="Opening Stock Balancing History";}
 private static final Map<String,String> COLUMNS;
 static {var map=new LinkedHashMap<String,String>();String[][] entries={{"DocNo","Doc No"},{"DocDate","Doc Date"},{"ItemName","Item"},{"PackUom","Item UOM"},{"CropYear","Crop"},{"JobLotDescription","Job Lot"},{"WareHouseName","Warehouse"},{"PackTypeDesc","Packing Type"},{"AccountTitle","Stock Credit Account"},{"Qty","Qty"},{"WeightKgs","Weight"},{"ItemRate","Rate"},{"RateUom","Rate UOM"},{"ItemAmount","Amount"},{"Remarks","Remarks"},{"IssueWeight","Issue Weight"},{"EntryDate","Entry Date"},{"EntryUser","Entry User"},{"ModifyDate","Modify Date"},{"ModifyUser","Modify User"},{"NoOfAttachments","Attachments"}};for(var e:entries)map.put(e[0],e[1]);COLUMNS=Collections.unmodifiableMap(map);}
 private final InventoryOpeningService opening;private final CurrentUserContext context;private final DesktopReportRights rights;
 public InventoryOpeningOutputService(InventoryOpeningService opening,CurrentUserContext context,DesktopReportRights rights){this.opening=opening;this.context=context;this.rights=rights;}
 public void printRight(){var u=context.requireAccountingUser();rights.require(u,92,"View");rights.require(u,92,"Grid Print");}
 public byte[] export(Request r){
  var u=context.requireAccountingUser();rights.require(u,92,"View");rights.require(u,92,"Grid Export");
  if(r==null||r.filters==null||r.ids==null||r.ids.isEmpty()||r.ids.size()>65534)throw new IllegalArgumentException("Show between 1 and 65,534 history rows before exporting");
  if(r.columns==null||r.columns.isEmpty()||r.columns.size()>COLUMNS.size()||new HashSet<>(r.columns).size()!=r.columns.size()||!COLUMNS.keySet().containsAll(r.columns))throw new IllegalArgumentException("Select valid history columns");
  if(r.title==null||r.title.length()>150)throw new IllegalArgumentException("Grid title cannot exceed 150 characters");
  var allowed=new HashMap<Integer,Map<String,Object>>();for(var row:opening.history(r.filters))allowed.put(((Number)row.get("Id")).intValue(),row);
  if(new HashSet<>(r.ids).size()!=r.ids.size()||!allowed.keySet().containsAll(r.ids))throw new IllegalArgumentException("History changed; reload it before exporting");
  try(var workbook=new HSSFWorkbook();var output=new ByteArrayOutputStream()){
   var sheet=workbook.createSheet("Opening Stock");var font=workbook.createFont();font.setFontName("Verdana");font.setFontHeightInPoints((short)8);var basic=workbook.createCellStyle();basic.setFont(font);var numeric=workbook.createCellStyle();numeric.cloneStyleFrom(basic);numeric.setDataFormat(workbook.createDataFormat().getFormat("#,##0.###"));var day=workbook.createCellStyle();day.cloneStyleFrom(basic);day.setDataFormat(workbook.createDataFormat().getFormat("dd/mm/yyyy"));var timestamp=workbook.createCellStyle();timestamp.cloneStyleFrom(basic);timestamp.setDataFormat(workbook.createDataFormat().getFormat("dd-mmm-yyyy hh:mm AM/PM"));var header=workbook.createCellStyle();header.cloneStyleFrom(basic);header.setFillForegroundColor(IndexedColors.GREY_25_PERCENT.getIndex());header.setFillPattern(FillPatternType.SOLID_FOREGROUND);
   sheet.createRow(0).createCell(0).setCellValue(r.title);var headings=sheet.createRow(1);for(int i=0;i<r.columns.size();i++){var c=headings.createCell(i);c.setCellValue(COLUMNS.get(r.columns.get(i)));c.setCellStyle(header);}
   int index=2;for(int id:r.ids){var source=allowed.get(id);var row=sheet.createRow(index++);for(int i=0;i<r.columns.size();i++){String key=r.columns.get(i);Object value=source.get(key);var cell=row.createCell(i);cell.setCellStyle(basic);if(value==null)continue;if(value instanceof java.util.Date date){cell.setCellValue(date);cell.setCellStyle(key.equals("DocDate")?day:timestamp);}else if(value instanceof Number number){cell.setCellValue(number.doubleValue());cell.setCellStyle(numeric);}else cell.setCellValue(value.toString());}}
   for(int i=0;i<r.columns.size();i++){sheet.autoSizeColumn(i);sheet.setColumnWidth(i,Math.min(60*256,Math.max(8*256,sheet.getColumnWidth(i))));}sheet.createFreezePane(0,2);sheet.setRepeatingRows(new org.apache.poi.ss.util.CellRangeAddress(1,1,-1,-1));sheet.getPrintSetup().setLandscape(true);sheet.setFitToPage(true);sheet.getPrintSetup().setFitWidth((short)1);sheet.getPrintSetup().setFitHeight((short)0);workbook.write(output);return output.toByteArray();
  }catch(IOException ex){throw new IllegalStateException("The Excel export could not be generated",ex);}
 }
}
