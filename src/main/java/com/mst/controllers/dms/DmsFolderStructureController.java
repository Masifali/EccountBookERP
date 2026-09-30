package com.mst.controllers.dms;

import com.mst.controllers.hrm.HrmApi;
import com.mst.services.dms.DmsFolderStructureService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Map;

/**
 * Upload documents - 473 /upload-documents/folder-structure (DMS_FolderHierarchy.CreateFolderStructure).
 * API /api/upload-documents/folder-structure/{setup|tree|documents|add-folder|delete-folder|delete-file}.
 * Port notes: {@link DmsFolderStructureService}.
 */
@Controller
public class DmsFolderStructureController {

    private static final String API = "/api/upload-documents/folder-structure";

    @Autowired private DmsFolderStructureService service;

    @GetMapping("/upload-documents/folder-structure")
    public String page(Model model) { model.addAttribute("activeMenu", "apps"); return "upload_documents/folder-structure"; }

    @GetMapping(API + "/setup") @ResponseBody
    public ResponseEntity<?> setup() { return HrmApi.run(() -> service.setup()); }

    @GetMapping(API + "/tree") @ResponseBody
    public ResponseEntity<?> tree() { return HrmApi.run(() -> service.tree()); }

    @GetMapping(API + "/documents") @ResponseBody
    public ResponseEntity<?> documents(@RequestParam("folderId") long folderId) { return HrmApi.run(() -> service.documents(folderId)); }

    @PostMapping(API + "/add-folder") @ResponseBody
    public ResponseEntity<?> addFolder(@RequestBody Map<String, Object> b) {
        return HrmApi.run(() -> service.addSubFolder(num(b.get("parentId")), b.get("name") == null ? "" : String.valueOf(b.get("name"))));
    }

    @PostMapping(API + "/delete-folder") @ResponseBody
    public ResponseEntity<?> deleteFolder(@RequestBody Map<String, Object> b) { return HrmApi.run(() -> service.deleteFolder(num(b.get("folderId")))); }

    @PostMapping(API + "/delete-file") @ResponseBody
    public ResponseEntity<?> deleteFile(@RequestBody Map<String, Object> b) {
        return HrmApi.run(() -> service.deleteFile(num(b.get("folderId")), (int) num(b.get("docFilePathId"))));
    }

    private static long num(Object v) {
        if (v instanceof Number) return ((Number) v).longValue();
        try { return v == null ? 0L : Long.parseLong(String.valueOf(v).trim()); } catch (NumberFormatException e) { return 0L; }
    }
}
