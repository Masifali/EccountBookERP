package com.mst.controllers;

import com.mst.models.dto.OpeningStockStoreDto;
import com.mst.services.OpeningStockStoreService;
import com.mst.services.OpeningStockStoreUploadService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Collections;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Store Management, ModuleId 24 — screen 341 "Opening Stock Store",
 * {@code Architecture.WinApp.Inventory_Definition.frmStoreOpeningStockBalancing} (store path,
 * BaseDocumentTypeId 1), ScreenName "frmStoreOpeningStockBalancing", DocumentTypeId 39,
 * at /store/opening-stock-store. BLL 0253 / DAL 0222 / Model 0201 InvStockOpeningBalanceHeader.
 *
 * DESKTOP BEHAVIOUR REPRODUCED and DEVIATIONS: see {@link OpeningStockStoreService}.
 */
@Controller
public class OpeningStockStoreController {

    private static final String API = "/api/store/opening-stock-store";

    private final OpeningStockStoreService service;
    private final OpeningStockStoreUploadService upload;
    public OpeningStockStoreController(OpeningStockStoreService service, OpeningStockStoreUploadService upload) {
        this.service = service;
        this.upload = upload;
    }

    /** btnAttachment — the opened document's attachments. */
    @GetMapping(API + "/{id:[0-9]+}/attachments")
    @ResponseBody
    public ResponseEntity<?> attachments(@PathVariable int id) {
        return run(() -> service.attachments(id), "Could not load the attachments.");
    }

    @GetMapping(API + "/{id:[0-9]+}/attachments/{attachment:[0-9]+}")
    @ResponseBody
    public ResponseEntity<?> download(@PathVariable int id, @PathVariable int attachment) {
        try {
            var file = service.download(id, attachment);
            return ResponseEntity.ok()
                    .header("Content-Disposition", org.springframework.http.ContentDisposition.attachment()
                            .filename(file.name(), java.nio.charset.StandardCharsets.UTF_8).build().toString())
                    .contentType(org.springframework.http.MediaType.APPLICATION_OCTET_STREAM)
                    .body(file.bytes());
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(fail(msg(e, "Download failed.")));
        }
    }

    /** Upload Opening → frmItemDefineExcelSheetUpload btnUpload (read the "Items" sheet). */
    @PostMapping(value = API + "/upload-opening/preview", consumes = org.springframework.http.MediaType.MULTIPART_FORM_DATA_VALUE)
    @ResponseBody
    public ResponseEntity<?> uploadPreview(@RequestParam("file") org.springframework.web.multipart.MultipartFile file) {
        try (java.io.InputStream in = file.getInputStream()) {
            return ResponseEntity.ok(upload.preview(file.getOriginalFilename(), in));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(fail(msg(e, "The workbook could not be read.")));
        }
    }

    /** Upload Opening → frmItemDefineExcelSheetUpload btnSave (the grid rows). */
    @PostMapping(API + "/upload-opening/save")
    @ResponseBody
    public ResponseEntity<?> uploadSave(@RequestBody Map<String, List<Map<String, String>>> body) {
        try {
            return ResponseEntity.ok(upload.save(body == null ? null : body.get("rows")));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            Throwable r = e;
            while (r.getCause() != null && r.getCause() != r) r = r.getCause();
            return ResponseEntity.badRequest().body(fail(msg(r, "Upload failed.")));
        }
    }

    @GetMapping("/store/opening-stock-store")
    public String page(Model model) {
        model.addAttribute("activeMenu", "store");
        return "store/opening_stock_store";
    }

    /** Form_Load + InitializeComponentMethod. */
    @GetMapping(API + "/lookups")
    @ResponseBody
    public ResponseEntity<?> lookups() { return run(service::lookups, "Could not load the screen."); }

    /** btnRefresh_Click — the global lists again. */
    @GetMapping(API + "/globals")
    @ResponseBody
    public ResponseEntity<?> globals() { return run(service::globals, "Refresh failed."); }

    /** FormReset — DocumentNoDbCall. */
    @GetMapping(API + "/next-doc-no")
    @ResponseBody
    public ResponseEntity<?> nextDocNo() {
        return run(() -> Collections.singletonMap("docNo", service.nextDocNo()), "Could not generate the document number.");
    }

    /** BtnRefreshHistory_Click — HistoryComboBind(HistoryComboDbCall()). */
    @GetMapping(API + "/history-combos")
    @ResponseBody
    public ResponseEntity<?> historyCombos() { return run(service::historyCombos, "Could not load the history filters."); }

    @GetMapping(API + "/history")
    @ResponseBody
    public ResponseEntity<?> history(@RequestParam(required = false) String fromDate,
                                     @RequestParam(required = false) String toDate,
                                     @RequestParam(required = false) String fromDocNo,
                                     @RequestParam(required = false) String toDocNo,
                                     @RequestParam(defaultValue = "0") int accountId,
                                     @RequestParam(defaultValue = "0") int itemStockAccountId,
                                     @RequestParam(defaultValue = "0") int itemId) {
        return run(() -> service.history(fromDate, toDate, fromDocNo, toDocNo, accountId, itemStockAccountId, itemId),
                "History failed.");
    }

    @GetMapping(API + "/{id:[0-9]+}")
    @ResponseBody
    public ResponseEntity<?> load(@PathVariable int id) {
        try {
            Map<String, Object> m = service.load(id);
            if (m == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("Record not Found"));
            return ResponseEntity.ok(m);
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, "Could not open that document.")));
        }
    }

    /** 416-Print — the register procedure's rows (the Crystal layout is not ported). */
    @GetMapping(API + "/{id:[0-9]+}/slip")
    @ResponseBody
    public ResponseEntity<?> slip(@PathVariable int id) {
        try {
            List<Map<String, Object>> rows = service.slip(id);
            if (rows == null || rows.isEmpty()) return ResponseEntity.status(HttpStatus.NOT_FOUND).body(fail("No Record Found For Display"));
            return ResponseEntity.ok(rows);
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e, "Print failed.")));
        }
    }

    @PostMapping(API + "/save")
    @ResponseBody
    public ResponseEntity<?> save(@RequestBody OpeningStockStoreDto dto) {
        try {
            return ResponseEntity.ok(service.save(dto));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (IllegalStateException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            Throwable r = e;
            while (r.getCause() != null && r.getCause() != r) r = r.getCause();
            return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(fail(msg(r, "Save failed.")));
        }
    }

    private interface Call { Object get() throws Exception; }

    private static ResponseEntity<?> run(Call c, String fallback) {
        try {
            return ResponseEntity.ok(c.get());
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(fail(msg(e, fallback)));
        }
    }

    private static String msg(Throwable e, String fallback) {
        return (e.getMessage() == null || e.getMessage().trim().isEmpty()) ? fallback : e.getMessage();
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
