package com.mst.services;

import com.mst.models.*;
import java.io.*;
import java.time.*;
import java.time.format.DateTimeFormatter;
import java.util.*;
import org.apache.poi.hssf.usermodel.HSSFWorkbook;
import org.apache.poi.ss.usermodel.*;
import org.springframework.stereotype.Service;

/** Native grid Excel export, regenerated from authorized database rows. */
@Service
public class PackingMaterialReportExportService {
    public record Request(PackingReportFilter filter,String search,Map<String,String> columnFilters,String sortKey,int sortDirection) {}
    private final PackingMaterialReportsService reports;
    public PackingMaterialReportExportService(PackingMaterialReportsService reports){this.reports=reports;}
    public byte[] export(String report,Request request) throws IOException {
        if(request==null||request.filter()==null)throw new IllegalArgumentException("Load the report before exporting");
        var columns=PackingReportColumns.forReport(report,request.filter().mode());
        var filters=request.columnFilters()==null?Map.<String,String>of():request.columnFilters();
        var keys=columns.stream().map(PackingReportColumns.Column::key).toList();
        if(!keys.containsAll(filters.keySet())||(request.sortKey()!=null&&!keys.contains(request.sortKey())))throw new IllegalArgumentException("Invalid grid column");
        String search=Objects.toString(request.search(),"").toLowerCase(Locale.ROOT);
        var rows=new ArrayList<>(reports.data(report,request.filter()).stream().filter(row->
            columns.stream().anyMatch(c->text(row.get(c.key()),c,report).toLowerCase(Locale.ROOT).contains(search))&&
            columns.stream().allMatch(c->{String filter=filters.get(c.key());return filter==null||filter.isEmpty()||
                (c.type().equals("d")?Objects.toString(row.get(c.key()),"").startsWith(filter):text(row.get(c.key()),c,report).toLowerCase(Locale.ROOT).contains(filter.toLowerCase(Locale.ROOT)));})).toList());
        if(rows.isEmpty())throw new IllegalArgumentException("No visible records to export");
        if(rows.size()>65533)throw new IllegalArgumentException("Limit the report to 65,533 rows for Excel export");
        if(request.sortKey()!=null)rows.sort((a,b)->{
            Object av=a.get(request.sortKey()),bv=b.get(request.sortKey());
            int result=av instanceof Number an&&bv instanceof Number bn?Double.compare(an.doubleValue(),bn.doubleValue()):Objects.toString(av,"").compareToIgnoreCase(Objects.toString(bv,""));
            return (request.sortDirection()<0?-1:1)*result;
        });
        try(var book=new HSSFWorkbook();var out=new ByteArrayOutputStream()) {
            var sheet=book.createSheet("Report");var font=book.createFont();font.setFontName("Verdana");font.setFontHeightInPoints((short)9);
            var regular=book.createCellStyle();regular.setFont(font);var heading=book.createCellStyle();heading.cloneStyleFrom(regular);
            heading.setFillForegroundColor(IndexedColors.GREY_25_PERCENT.getIndex());heading.setFillPattern(FillPatternType.SOLID_FOREGROUND);
            var date=book.createCellStyle();date.cloneStyleFrom(regular);date.setDataFormat(book.createDataFormat().getFormat("dd-mmm-yy"));
            var numbers=new HashMap<String,CellStyle>();
            var header=sheet.createRow(0);
            for(int i=0;i<columns.size();i++){var c=columns.get(i);var cell=header.createCell(i);cell.setCellValue(c.title());cell.setCellStyle(heading);sheet.setColumnWidth(i,Math.min(60*256,Math.max(10*256,c.width()*256/7)));}
            int rowNo=1;
            for(var row:rows){var target=sheet.createRow(rowNo++);for(int i=0;i<columns.size();i++){
                var c=columns.get(i);var cell=target.createCell(i);cell.setCellStyle(regular);Object value=row.get(c.key());if(value==null)continue;
                if(c.type().equals("d")||c.type().equals("expiry")){cell.setCellValue(LocalDate.parse(value.toString().substring(0,10)).atStartOfDay());cell.setCellStyle(date);}
                else if(value instanceof Boolean flag)cell.setCellValue(flag);
                else if(c.type().startsWith("n")&&value instanceof Number n){cell.setCellValue(n.doubleValue());cell.setCellStyle(numbers.computeIfAbsent(c.type(),key->{
                    var style=book.createCellStyle();style.cloneStyleFrom(regular);boolean fixed=report.equals("stock-with-supplier")||report.equals("requirement-planning-detail");
                    String pattern="#,##0."+(fixed?"0":"#").repeat(Integer.parseInt(key.substring(1)));style.setDataFormat(book.createDataFormat().getFormat(pattern+(fixed?";("+pattern+")":"")));return style;}));}
                // Text cells remain strings, including values starting with '='. Never create formulas from database text.
                else cell.setCellValue(value.toString());
            }}
            sheet.createFreezePane(0,1);sheet.setAutoFilter(new org.apache.poi.ss.util.CellRangeAddress(0,rowNo-1,0,columns.size()-1));
            sheet.getPrintSetup().setLandscape(true);book.write(out);return out.toByteArray();
        }
    }
    private static String text(Object value,PackingReportColumns.Column c,String report) {
        if(value==null)return "";String s=value.toString();
        if(c.type().equals("d")||c.type().equals("expiry"))return LocalDate.parse(s.substring(0,10)).format(DateTimeFormatter.ofPattern("dd-MMM-yy",Locale.US));
        if(c.type().startsWith("n")&&value instanceof Number n){boolean fixed=report.equals("stock-with-supplier")||report.equals("requirement-planning-detail");var format=java.text.NumberFormat.getNumberInstance(Locale.US);format.setMaximumFractionDigits(Integer.parseInt(c.type().substring(1)));format.setMinimumFractionDigits(fixed?Integer.parseInt(c.type().substring(1)):0);String amount=format.format(Math.abs(n.doubleValue()));return n.doubleValue()<0?(fixed?"("+amount+")":"-"+amount):amount;}
        return s;
    }
}
