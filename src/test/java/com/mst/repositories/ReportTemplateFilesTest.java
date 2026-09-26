package com.mst.repositories;
import com.mst.reports.CrystalPrintProperties;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.io.TempDir;
import java.nio.file.*;
import static org.junit.jupiter.api.Assertions.*;
class ReportTemplateFilesTest {
 @TempDir Path root;
 @Test void originalFilesResolveWithoutDatabase() throws Exception {
  Files.createDirectories(root.resolve("copies"));Files.writeString(root.resolve("203.rpt"),"original");Files.writeString(root.resolve("copies/203.rpt"),"copy");
  var props=new CrystalPrintProperties();props.setTemplateRoot(root.toString());var repo=new ReportTemplateRepository(props);
  assertTrue(repo.seeded());assertEquals(2,repo.count());assertEquals("203.rpt",repo.relativePathOf("203.RPT"));assertEquals(2,repo.copiesOf("203.rpt").size());
  assertNull(repo.relativePathOf("../203.rpt"));assertNull(repo.relativePathOf("missing.rpt"));assertEquals("original",Files.readString(root.resolve("203.rpt")));
 }
}
