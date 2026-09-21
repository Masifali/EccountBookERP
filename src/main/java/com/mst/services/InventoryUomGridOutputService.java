package com.mst.services;
import com.mst.security.*;
import java.io.*;
import java.math.BigDecimal;
import java.util.*;
import org.apache.poi.hssf.usermodel.HSSFWorkbook;
import org.apache.poi.ss.usermodel.*;
import org.springframework.stereotype.Service;
/** CtrlGrdBar outputs for the UOM form, whose source authorizes all controls through View115. */
@Service
public class InventoryUomGridOutputService {
 public static class Request {public List<Integer> ids;public List<String> columns;public String title="Item UOM Schedule";}
 private static final Map<String,String> COLUMNS;
 static {var c=new LinkedHashMap<String,String>();String[][] names={{"ItemName","Item"},{"UOMDescription","Schedule Unit"},{"Equivalent","Equivalent"},{"QtyEquivalent","QtyEquivalent"},{"BaseRateUom","BaseRateUom"},{"BasePackUom","BasePackUom"},{"BaseSecondaryUom","BaseSecondaryUom"},{"Active","Active"}};for(var pair:names)c.put(pair[0],pair[1]);COLUMNS=Collections.unmodifiableMap(c);}
 private final InventoryUomScheduleService service;private final CurrentUserContext context;private final DesktopReportRights rights;
 public InventoryUomGridOutputService(InventoryUomScheduleService service,CurrentUserContext context,DesktopReportRights rights){this.service=service;this.context=context;this.rights=rights;}
 public void authorize(){rights.require(context.requireAccountingUser(),115,"View");}
 public byte[] export(Request r){authorize();if(r==null||r.ids==null||r.ids.isEmpty()||r.ids.size()>65533||new HashSet<>(r.ids).size()!=r.ids.size())throw new IllegalArgumentException("Choose between 1 and 65,533 distinct history records");if(r.columns==null||r.columns.isEmpty()||new HashSet<>(r.columns).size()!=r.columns.size()||!COLUMNS.keySet().containsAll(r.columns))throw new IllegalArgumentException("Choose valid visible columns");if(r.title==null||r.title.length()>150)throw new IllegalArgumentException("Grid title cannot exceed150 characters");var allowed=new HashMap<Integer,Map<String,Object>>();for(var row:service.history(null))allowed.put(((Number)row.get("Id")).intValue(),row);if(!allowed.keySet().containsAll(r.ids))throw new IllegalArgumentException("History changed; reload before exporting");
  try(var book=new HSSFWorkbook();var output=new ByteArrayOutputStream()){var sheet=book.createSheet("UOM Schedule");var font=book.createFont();font.setFontName("Verdana");font.setFontHeightInPoints((short)8);var plain=book.createCellStyle();plain.setFont(font);var numeric=book.createCellStyle();numeric.cloneStyleFrom(plain);numeric.setDataFormat(book.createDataFormat().getFormat("#,##0.####"));var header=book.createCellStyle();header.cloneStyleFrom(plain);header.setFillForegroundColor(IndexedColors.GREY_25_PERCENT.getIndex());header.setFillPattern(FillPatternType.SOLID_FOREGROUND);sheet.createRow(0).createCell(0).setCellValue(r.title);var headings=sheet.createRow(1);for(int col=0;col<r.columns.size();col++){var cell=headings.createCell(col);cell.setCellValue(COLUMNS.get(r.columns.get(col)));cell.setCellStyle(header);}int index=2;var sums=new HashMap<String,BigDecimal>();for(int id:r.ids){var data=allowed.get(id);var row=sheet.createRow(index++);for(int col=0;col<r.columns.size();col++){String key=r.columns.get(col);Object value=data.get(key);var cell=row.createCell(col);cell.setCellStyle(plain);if(value instanceof Boolean flag)cell.setCellValue(flag);else if(value instanceof Number number){cell.setCellValue(number.doubleValue());cell.setCellStyle(numeric);if(key.endsWith("Equivalent"))sums.merge(key,new BigDecimal(value.toString()),BigDecimal::add);}else if(value!=null)cell.setCellValue(value.toString());}}
   var total=sheet.createRow(index);for(int col=0;col<r.columns.size();col++){var cell=total.createCell(col);String key=r.columns.get(col);cell.setCellStyle(header);if(sums.containsKey(key)){cell.setCellValue(sums.get(key).doubleValue());cell.setCellStyle(numeric);}else if(col==0)cell.setCellValue("Total");sheet.autoSizeColumn(col);sheet.setColumnWidth(col,Math.min(60*256,Math.max(8*256,sheet.getColumnWidth(col))));}sheet.createFreezePane(0,2);sheet.getPrintSetup().setLandscape(true);sheet.setFitToPage(true);sheet.getPrintSetup().setFitWidth((short)1);sheet.getPrintSetup().setFitHeight((short)0);book.write(output);return output.toByteArray();
  }catch(IOException ex){throw new IllegalStateException("The workbook could not be created",ex);}
 }
 public byte[] print(Request request){
  byte[] spreadsheet=export(request);
  try(var book=new HSSFWorkbook(new ByteArrayInputStream(spreadsheet));var output=new ByteArrayOutputStream()){
   var document=new com.itextpdf.text.Document(com.itextpdf.text.PageSize.A4.rotate(),24,24,24,24);com.itextpdf.text.pdf.PdfWriter.getInstance(document,output);document.open();
   var base=com.itextpdf.text.pdf.BaseFont.createFont("C:/Windows/Fonts/verdana.ttf",com.itextpdf.text.pdf.BaseFont.IDENTITY_H,com.itextpdf.text.pdf.BaseFont.EMBEDDED);var font=new com.itextpdf.text.Font(base,8);document.add(new com.itextpdf.text.Paragraph(request.title,new com.itextpdf.text.Font(base,12,com.itextpdf.text.Font.BOLD)));document.add(new com.itextpdf.text.Paragraph(" "));
   var table=new com.itextpdf.text.pdf.PdfPTable(request.columns.size());table.setWidthPercentage(100);table.setHeaderRows(1);var sheet=book.getSheetAt(0);var format=new DataFormatter(java.util.Locale.US);for(int row=1;row<=sheet.getLastRowNum();row++){for(int col=0;col<request.columns.size();col++){var source=sheet.getRow(row).getCell(col);var cell=new com.itextpdf.text.pdf.PdfPCell(new com.itextpdf.text.Phrase(format.formatCellValue(source),font));cell.setPadding(3);cell.setBorderColor(com.itextpdf.text.BaseColor.LIGHT_GRAY);if(row==1||row==sheet.getLastRowNum())cell.setBackgroundColor(new com.itextpdf.text.BaseColor(240,240,240));if(source.getCellType()==CellType.NUMERIC)cell.setHorizontalAlignment(com.itextpdf.text.Element.ALIGN_RIGHT);table.addCell(cell);}}document.add(table);document.close();return output.toByteArray();
  }catch(Exception ex){throw new IllegalStateException("The grid print preview could not be generated",ex);}
 }

}
