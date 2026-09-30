package com.mst.controllers;

import com.mst.models.dto.InventoryGeneralItemRequest;
import com.mst.services.DesktopInventoryItemService;
import java.util.*;
import org.springframework.http.*;
import org.springframework.web.bind.annotation.*;

@RestController
/** Screen 111 InvDefrmAddItem; with ?taxable=1 the same endpoints serve screen 180 DefineTaxItem (Architecture.WinApp.Tax_Definition),
 *  the desktop's taxable-item copy of the same form: IsTaxable = true on save and in FormHistory, category/type combos
 *  unfiltered, no Mother Item and no Party-Processing flags. */
@RequestMapping("/api/inventory/defined-items")
public class DesktopInventoryItemController {
    private final DesktopInventoryItemService service;
    public DesktopInventoryItemController(DesktopInventoryItemService service){this.service=service;}
    @GetMapping("/lookups") public Map<String,Object> lookups(@RequestParam(defaultValue="false") boolean taxable){return service.lookups(taxable);}
    @GetMapping("/history") public List<Map<String,Object>> history(@RequestParam(defaultValue="50") int count,@RequestParam(defaultValue="0") int category,@RequestParam(defaultValue="0") int type,@RequestParam(defaultValue="0") int parent,@RequestParam(defaultValue="false") boolean taxable){return service.history(taxable,count,category,type,parent);}
    @GetMapping("/{id:[0-9]+}") public Map<String,Object> record(@PathVariable int id,@RequestParam(defaultValue="false") boolean taxable){return service.record(taxable,id);}
    @GetMapping("/code") public Map<String,Object> code(@RequestParam String code,@RequestParam(defaultValue="false") boolean taxable){return service.byCode(taxable,code);}
    @GetMapping("/defaults") public Map<String,Object> defaults(@RequestParam int category,@RequestParam(defaultValue="0") int type,@RequestParam(defaultValue="false") boolean taxable){return service.defaults(taxable,category,type);}
    @PostMapping("/save") public Map<String,Object> save(@RequestBody InventoryGeneralItemRequest request,@RequestParam(defaultValue="false") boolean taxable){return service.save(taxable,request);}
    @PostMapping("/names") public Map<String,Object> names(@RequestBody List<InventoryGeneralItemRequest.Name> changes,@RequestParam(defaultValue="false") boolean taxable){return service.updateNames(taxable,changes);}
    @GetMapping("/{id:[0-9]+}/attachments/{attachment:[0-9]+}") public ResponseEntity<byte[]> attachment(@PathVariable int id,@PathVariable int attachment){var file=service.attachment(id,attachment);return ResponseEntity.ok().contentType(MediaType.APPLICATION_OCTET_STREAM).header("X-Content-Type-Options","nosniff").header(HttpHeaders.CONTENT_DISPOSITION,ContentDisposition.attachment().filename(file.name(),java.nio.charset.StandardCharsets.UTF_8).build().toString()).cacheControl(CacheControl.noStore()).body(file.bytes());}
    @GetMapping("/{id:[0-9]+}/images/{image:[0-9]+}") public ResponseEntity<byte[]> image(@PathVariable int id,@PathVariable int image){var file=service.image(id,image);return ResponseEntity.ok().contentType(MediaType.parseMediaType(file.type())).header("X-Content-Type-Options","nosniff").cacheControl(CacheControl.noStore()).body(file.bytes());}
    @ExceptionHandler(IllegalStateException.class) public ResponseEntity<Map<String,String>> unavailable(IllegalStateException ex){return error(409,ex.getMessage());}
    @ExceptionHandler(IllegalArgumentException.class) public ResponseEntity<Map<String,String>> invalid(IllegalArgumentException ex){return error(400,ex.getMessage());}
    @ExceptionHandler(org.springframework.security.access.AccessDeniedException.class) public ResponseEntity<Map<String,String>> forbidden(org.springframework.security.access.AccessDeniedException ex){return error(403,ex.getMessage());}
    @ExceptionHandler(org.springframework.dao.DataAccessException.class) public ResponseEntity<Map<String,String>> database(org.springframework.dao.DataAccessException ex){return error(409,Objects.toString(ex.getMostSpecificCause().getMessage(),"Unable to save the item"));}
    private ResponseEntity<Map<String,String>> error(int status,String message){return ResponseEntity.status(status).contentType(MediaType.APPLICATION_JSON).body(Map.of("message",Objects.toString(message,"Request failed")));}
}
