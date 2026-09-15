package com.mst.controllers;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.Collections;
import java.util.Map;

@RestController
@RequestMapping("/.well-known")
public class WellKnownRestController {

    @GetMapping("/appspecific/com.chrome.devtools.json")
    public ResponseEntity<Map<String, Object>> getChromeDevToolsJson() {
        return ResponseEntity.ok(Collections.emptyMap());
    }
}
