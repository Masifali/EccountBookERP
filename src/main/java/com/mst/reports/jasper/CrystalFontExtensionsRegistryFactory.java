package com.mst.reports.jasper;

import net.sf.jasperreports.engine.DefaultJasperReportsContext;
import net.sf.jasperreports.engine.JRPropertiesMap;
import net.sf.jasperreports.engine.fonts.FontFamily;
import net.sf.jasperreports.engine.fonts.SimpleFontFamily;
import net.sf.jasperreports.engine.fonts.SimpleFontFace;
import net.sf.jasperreports.extensions.ExtensionsRegistry;
import net.sf.jasperreports.extensions.ExtensionsRegistryFactory;

import java.nio.file.Files;
import java.nio.file.Path;
import java.util.ArrayList;
import java.util.List;

/** Embeds installed desktop fonts so PDF text matches the Crystal design and AWT metrics. */
public class CrystalFontExtensionsRegistryFactory implements ExtensionsRegistryFactory {
    private static final String[][] FAMILIES = {
        {"Calibri", "calibri.ttf", "calibrib.ttf", "calibrii.ttf", "calibriz.ttf"},
        {"Calibri Light", "calibril.ttf", "calibrib.ttf", "calibrili.ttf", "calibriz.ttf"},
        {"Times New Roman", "times.ttf", "timesbd.ttf", "timesi.ttf", "timesbi.ttf"},
        {"Arial", "arial.ttf", "arialbd.ttf", "ariali.ttf", "arialbi.ttf"},
        {"Arial Narrow", "arialn.ttf", "arialnb.ttf", "arialni.ttf", "arialnbi.ttf"},
        {"Tahoma", "tahoma.ttf", "tahomabd.ttf", null, null},
        {"tahoma", "tahoma.ttf", "tahomabd.ttf", null, null},
        {"Segoe UI", "segoeui.ttf", "segoeuib.ttf", "segoeuii.ttf", "segoeuiz.ttf"},
        {"Courier New", "cour.ttf", "courbd.ttf", "couri.ttf", "courbi.ttf"}
    };

    @Override
    public ExtensionsRegistry createRegistry(String id, JRPropertiesMap properties) {
        List<Path> directories = new ArrayList<>();
        String configured = System.getProperty("mst.jasper.fonts.dir", System.getenv("JASPER_FONTS_DIR"));
        if (configured != null && !configured.isBlank()) directories.add(Path.of(configured));
        String windows = System.getenv("WINDIR");
        if (windows != null) directories.add(Path.of(windows, "Fonts"));
        List<FontFamily> families = new ArrayList<>();
        for (String[] spec : FAMILIES) {
            Path normal = find(directories, spec[1]);
            if (normal == null) continue;
            SimpleFontFamily family = new SimpleFontFamily(DefaultJasperReportsContext.getInstance());
            family.setName(spec[0]);
            family.setNormalFace(face(normal));
            Path bold = find(directories, spec[2]), italic = find(directories, spec[3]);
            Path boldItalic = find(directories, spec[4]);
            if (bold != null) family.setBoldFace(face(bold));
            if (italic != null) family.setItalicFace(face(italic));
            if (boldItalic != null) family.setBoldItalicFace(face(boldItalic));
            family.setPdfEncoding("Identity-H");
            family.setPdfEmbedded(true);
            families.add(family);
        }
        return new ExtensionsRegistry() {
            @Override public <T> List<T> getExtensions(Class<T> type) {
                return type == FontFamily.class ? families.stream().map(type::cast).toList() : List.of();
            }
        };
    }

    private static Path find(List<Path> directories, String filename) {
        if (filename == null) return null;
        return directories.stream().map(p -> p.resolve(filename)).filter(Files::isRegularFile).findFirst().orElse(null);
    }

    private static SimpleFontFace face(Path path) {
        SimpleFontFace face = new SimpleFontFace(DefaultJasperReportsContext.getInstance());
        // Loading here would recurse into extension discovery through the repository service.
        face.setTtf(path.toString(), false);
        return face;
    }
}
