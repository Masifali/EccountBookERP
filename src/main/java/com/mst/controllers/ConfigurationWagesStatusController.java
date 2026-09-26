package com.mst.controllers;
import com.mst.models.dto.WagesDocumentStatusDto;
import com.mst.services.ConfigurationWagesStatusService;
import org.springframework.web.bind.annotation.*;
import java.util.*;
@RestController
@RequestMapping("/api/configurations/wages-documents")
public class ConfigurationWagesStatusController {
 private final ConfigurationWagesStatusService service;
 public ConfigurationWagesStatusController(ConfigurationWagesStatusService service){this.service=service;}
 @GetMapping public List<Map<String,Object>> load(){return service.load();}
 @PostMapping public Map<String,Object> update(@RequestBody List<WagesDocumentStatusDto> rows){service.update(rows);return Map.of("success",true,"message","Record's Status Updated Successfully");}
}
