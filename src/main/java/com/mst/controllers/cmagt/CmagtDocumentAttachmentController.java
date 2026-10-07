package com.mst.controllers.cmagt;

import com.mst.services.cmagt.CmagtDocumentAttachmentService;
import java.nio.charset.StandardCharsets;
import java.util.List;
import java.util.Map;
import org.springframework.http.ContentDisposition;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/api/commission/attachments")
public class CmagtDocumentAttachmentController {
    private final CmagtDocumentAttachmentService service;

    public CmagtDocumentAttachmentController(CmagtDocumentAttachmentService service) {
        this.service = service;
    }

    @GetMapping("/{screen}/{id:[0-9]+}")
    public List<Map<String, Object>> list(@PathVariable String screen, @PathVariable int id) {
        return service.list(screen, id);
    }

    @PostMapping("/{screen}/{id:[0-9]+}")
    public List<Map<String, Object>> save(@PathVariable String screen, @PathVariable int id,
            @RequestBody CmagtDocumentAttachmentService.Request request) {
        return service.save(screen, id, request);
    }

    @GetMapping("/{screen}/{id:[0-9]+}/{attachmentId:[0-9]+}")
    public ResponseEntity<byte[]> download(@PathVariable String screen, @PathVariable int id,
            @PathVariable int attachmentId) {
        var file = service.download(screen, id, attachmentId);
        ContentDisposition disposition = ContentDisposition.attachment()
                .filename(file.name(), StandardCharsets.UTF_8)
                .build();
        return ResponseEntity.ok().contentType(MediaType.APPLICATION_OCTET_STREAM)
                .header(HttpHeaders.CONTENT_DISPOSITION, disposition.toString()).body(file.bytes());
    }
}
