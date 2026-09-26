package com.mst.repositories;

import com.mst.reports.CrystalPrintProperties;
import org.springframework.stereotype.Repository;
import java.io.IOException;
import java.nio.file.*;
import java.util.*;
import java.util.stream.Collectors;

/** File inventory only: preserves the existing database and original Crystal templates. */
@Repository
public class ReportTemplateRepository {
    private final CrystalPrintProperties properties;
    public ReportTemplateRepository(CrystalPrintProperties properties) { this.properties = properties; }
    private Path root() {
        String value=properties.getTemplateRoot();
        return value==null || value.isBlank()?null:Paths.get(value).toAbsolutePath().normalize();
    }
    public boolean seeded() { Path root=root(); return root!=null && Files.isDirectory(root); }
    private List<Path> files() {
        Path root=root(); if(root==null || !Files.isDirectory(root)) return Collections.emptyList();
        try(var paths=Files.walk(root)) {
            return paths.filter(p->Files.isRegularFile(p,LinkOption.NOFOLLOW_LINKS))
                .filter(p->p.getFileName().toString().toLowerCase(Locale.ROOT).endsWith(".rpt"))
                .sorted(Comparator.<Path>comparingInt(p->root.relativize(p).getNameCount())
                    .thenComparing(p->root.relativize(p).toString(),String.CASE_INSENSITIVE_ORDER))
                .collect(Collectors.toList());
        } catch(IOException e) { throw new IllegalStateException("Cannot read the configured report template folder",e); }
    }
    public String relativePathOf(String name) {
        if(name==null || name.isBlank() || name.contains("/") || name.contains("\\")) return null;
        Path root=root();
        return files().stream().filter(p->p.getFileName().toString().equalsIgnoreCase(name.trim()))
            .map(p->root.relativize(p).toString()).findFirst().orElse(null);
    }
    public List<Map<String,Object>> copiesOf(String name) {
        List<Map<String,Object>> result=new ArrayList<>(); Path root=root();
        for(Path p:files()) if(p.getFileName().toString().equalsIgnoreCase(name)) {
            try { Map<String,Object> row=new LinkedHashMap<>(); row.put("RelativePath",root.relativize(p).toString());
                row.put("FileBytes",Files.size(p));row.put("IsPreferred",result.isEmpty());result.add(row);
            } catch(IOException e){throw new IllegalStateException("Cannot inspect report template",e);}
        }
        return result;
    }
    public int count() { return files().size(); }
}
