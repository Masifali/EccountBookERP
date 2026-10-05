package com.mst.reports.jasper;

import net.sf.jasperreports.engine.*;
import net.sf.jasperreports.engine.fonts.FontFamily;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.Assumptions;
import org.apache.pdfbox.pdmodel.PDDocument;
import org.apache.pdfbox.text.PDFTextStripper;
import java.io.ByteArrayInputStream;
import java.nio.charset.StandardCharsets;
import java.nio.file.*;
import java.util.Map;
import java.util.HashMap;
import static org.junit.jupiter.api.Assertions.*;

class CrystalFontExtensionsRegistryFactoryTest {
    @Test void registryCanBeCreatedWithoutRecursingIntoFontLoading() {
        var registry = new CrystalFontExtensionsRegistryFactory().createRegistry("test", new JRPropertiesMap());
        assertNotNull(registry.getExtensions(FontFamily.class));
        assertTrue(registry.getExtensions(String.class).isEmpty());
        for (var family : registry.getExtensions(FontFamily.class))
            assertNotNull(family.getNormalFace().getFont(), "Local font metrics must be loaded without relying on an installed AWT family");
    }

    @Test void installedCalibriIsEmbeddedInPdfAndTextRemainsReadable() throws Exception {
        String windows = System.getenv("WINDIR");
        Assumptions.assumeTrue(windows != null && Files.isRegularFile(Path.of(windows, "Fonts", "calibri.ttf")));
        String source = """
            <jasperReport xmlns="http://jasperreports.sourceforge.net/jasperreports"
              name="font_embedding_test" pageWidth="250" pageHeight="100" columnWidth="230"
              leftMargin="10" rightMargin="10" topMargin="10" bottomMargin="10">
              <detail><band height="30"><staticText>
                <reportElement x="0" y="0" width="230" height="25" forecolor="#008080"/>
                <textElement><font fontName="Calibri" size="12"/></textElement>
                <text><![CDATA[Sample ledger]]></text>
              </staticText></band></detail>
            </jasperReport>
            """;
        var report = JasperCompileManager.compileReport(new ByteArrayInputStream(source.getBytes(StandardCharsets.UTF_8)));
        byte[] pdf = JasperExportManager.exportReportToPdf(JasperFillManager.fillReport(report, new HashMap<>(), new JREmptyDataSource(1)));
        try (PDDocument document = PDDocument.load(pdf)) {
            assertTrue(new PDFTextStripper().getText(document).contains("Sample ledger"));
            boolean embedded = false;
            for (var name : document.getPage(0).getResources().getFontNames()) {
                var font = document.getPage(0).getResources().getFont(name);
                embedded |= font.getName().contains("Calibri") && font.isEmbedded();
            }
            assertTrue(embedded, "Calibri must be embedded instead of silently replaced with Helvetica");
        }
    }
}
