package com.accountbook.erp.controller;

import com.accountbook.erp.model.BankDefinition;
import com.accountbook.erp.model.ScreenCard;
import com.accountbook.erp.model.SubModuleCard;
import com.accountbook.erp.service.AccountsService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/api/accounts")
public class AccountsApiController {

    @Autowired
    private AccountsService accountsService;

    @GetMapping("/submodules")
    public ResponseEntity<List<SubModuleCard>> getSubModules() {
        return ResponseEntity.ok(accountsService.getSubModules());
    }

    @GetMapping("/screens")
    public ResponseEntity<List<ScreenCard>> getScreens(@RequestParam(required = false, defaultValue = "Banking Managment") String submodule) {
        return ResponseEntity.ok(accountsService.getScreenCards(submodule));
    }

    @PostMapping("/banks")
    public ResponseEntity<Map<String, Object>> saveBank(@RequestBody BankDefinition bank) {
        BankDefinition savedBank = accountsService.saveBankDefinition(bank);
        Map<String, Object> response = new HashMap<>();
        response.put("status", "success");
        response.put("message", "Bank Definition Saved Successfully!");
        response.put("data", savedBank);
        return ResponseEntity.ok(response);
    }

    @PutMapping("/screens/{screenId}/favorite")
    public ResponseEntity<Map<String, Object>> toggleFavorite(@PathVariable String screenId, @RequestBody Map<String, Boolean> body) {
        Map<String, Object> response = new HashMap<>();
        response.put("status", "success");
        response.put("screenId", screenId);
        response.put("isFavorite", body.getOrDefault("isFavorite", false));
        return ResponseEntity.ok(response);
    }
}
