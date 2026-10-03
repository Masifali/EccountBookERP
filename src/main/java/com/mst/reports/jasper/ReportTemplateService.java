package com.mst.reports.jasper;

import com.fasterxml.jackson.databind.ObjectMapper;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;

import java.io.ByteArrayInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;

/** Finds report source files. Compilation, filling and PDF export stay in the ERPPrint module controllers. */
@Service
public class ReportTemplateService {
    private static final String ROOT = "/jasper/converted/";
    private final Path folder;
    private final ObjectMapper json = new ObjectMapper();
    private final Map<String, Map<String, Object>> facts = new LinkedHashMap<>();

    public ReportTemplateService(@Value("${reports.jasper.folder:D:/CShapEccorErp/EccountingERP/src/main/resources/jasper/converted}") String folder)
            throws IOException {
        this.folder = Paths.get(folder).toAbsolutePath().normalize();
        for (String resource : List.of("/reports/print-contracts.json", "/reports/print-contracts-bll.json")) {
            try (InputStream in = getClass().getResourceAsStream(resource)) {
                if (in == null) continue;
                Map<?, ?> root = json.readValue(in, Map.class);
                for (Object item : (List<?>) root.get("prints")) {
                    @SuppressWarnings("unchecked") Map<String, Object> fact = (Map<String, Object>) item;
                    facts.putIfAbsent(String.valueOf(fact.get("template")).toLowerCase(Locale.ROOT), fact);
                }
            }
        }
    }

    public record Subreport(String name, String key, String file) { }

    public final class Source {
        private final Path directory;
        private final String main;
        private final List<Subreport> subreports;
        private final String generated;

        private Source(Path directory, String main, List<Subreport> subreports, String generated) {
            this.directory = directory;
            this.main = main;
            this.subreports = subreports;
            this.generated = generated;
        }

        public InputStream openMain() throws IOException {
            return generated == null ? open(main) : new ByteArrayInputStream(generated.getBytes(StandardCharsets.UTF_8));
        }

        public List<Subreport> subreports() { return subreports; }

        public boolean isGenerated() { return generated != null; }

        public InputStream openSubreport(Subreport subreport) throws IOException { return open(subreport.file()); }

        private InputStream open(String file) throws IOException {
            if (directory != null) {
                Path path = directory.resolve(file).normalize();
                if (!path.startsWith(directory)) throw new IOException("Report file is outside the configured report folder");
                if (Files.isRegularFile(path)) return Files.newInputStream(path);
            }
            InputStream stream = getClass().getResourceAsStream(ROOT + file);
            if (stream == null) throw new IOException("Report template not found: " + file);
            return stream;
        }
    }

    public Source source(String template, List<Map<String, Object>> rows) throws IOException {
        String base = CrystalJasperPrinter.baseName(template);
        String stem = CrystalJasperPrinter.stem(template);
        for (String name : List.of(base, stem)) {
            Path manifest = folder.resolve(name + ".manifest.json").normalize();
            if (!manifest.startsWith(folder)) throw new IOException("Invalid report template name");
            if (Files.isRegularFile(manifest)) {
                try (InputStream in = Files.newInputStream(manifest)) { return fromManifest(in, folder); }
            }
        }
        for (String name : List.of(base, stem)) {
            try (InputStream in = getClass().getResourceAsStream(ROOT + name + ".manifest.json")) {
                if (in != null) return fromManifest(in, null);
            }
        }
        Path saved = folder.resolve(base + ".jrxml").normalize();
        if (saved.startsWith(folder) && Files.isRegularFile(saved)) return new Source(folder, base + ".jrxml", List.of(), null);
        if (getClass().getResource(ROOT + base + ".jrxml") != null) return new Source(null, base + ".jrxml", List.of(), null);

        // Existing reports without a converted layout use a layout made from their procedure columns.
        Map<String, Object> fact = facts.getOrDefault(template.toLowerCase(Locale.ROOT), Map.of());
        GeneratedPrintTemplate.Meta meta = new GeneratedPrintTemplate.Meta();
        meta.name = stem;
        meta.title = GeneratedPrintTemplate.titleOf(template);
        meta.source = template;
        meta.landscape = "Landscape".equals(fact.get("orientation"));
        Object pictures = fact.get("images");
        if (pictures instanceof List && !((List<?>) pictures).isEmpty()) {
            meta.logoResource = "/jasper/print/images/" + ((Map<?, ?>) ((List<?>) pictures).get(0)).get("file");
        }
        return new Source(null, null, List.of(), GeneratedPrintTemplate.build(meta, rows, new LinkedHashMap<>()));
    }

    private Source fromManifest(InputStream in, Path directory) throws IOException {
        Map<?, ?> manifest = json.readValue(in, Map.class);
        List<Subreport> subreports = new ArrayList<>();
        if (manifest.get("subreports") instanceof List) {
            for (Object item : (List<?>) manifest.get("subreports")) {
                Map<?, ?> sub = (Map<?, ?>) item;
                subreports.add(new Subreport(String.valueOf(sub.get("name")), String.valueOf(sub.get("key")), String.valueOf(sub.get("file"))));
            }
        }
        return new Source(directory, String.valueOf(manifest.get("main")), subreports, null);
    }
}
