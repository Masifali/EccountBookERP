package com.mst.controllers;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import com.mst.models.AcLookUp;

import com.mst.serviceInterface.IAccountCustomGroupService;

/**
 * Controller for the Account Custom Group screen. Serves the HTML page and provides JSON API endpoints.
 */
@Controller
@RequestMapping("/accounts")
public class AccountCustomGroupRestController {

    @Autowired
    private IAccountCustomGroupService accountCustomGroupService;

    // ---------------------------------------------------------------------
    // UI page
    

    // ---------------------------------------------------------------------
    // API endpoints (JSON)
    @GetMapping("/api/custom-group/groups")
    public ResponseEntity<List<AcLookUp>> getAllGroups() {
        return ResponseEntity.ok(accountCustomGroupService.getAllGroups());
    }

    @GetMapping("/api/custom-group/group/{id}")
    public ResponseEntity<AcLookUp> getGroup(@PathVariable int id) {
        AcLookUp group = accountCustomGroupService.getGroupById(id);
        return group != null ? ResponseEntity.ok(group) : ResponseEntity.notFound().build();
    }

    @PostMapping("/api/custom-group")
    public ResponseEntity<AcLookUp> addOrUpdateGroup(@RequestBody AcLookUp group) {
        AcLookUp saved = accountCustomGroupService.addOrUpdateGroup(group);
        return ResponseEntity.ok(saved);
    }

    @DeleteMapping("/api/custom-group/{id}")
    public ResponseEntity<Void> deleteGroup(@PathVariable int id) {
        accountCustomGroupService.deleteGroup(id);
        return ResponseEntity.noContent().build();
    }

    @GetMapping("/api/custom-group/account-types")
    public ResponseEntity<?> getAccountTypes() {
        return ResponseEntity.ok(accountCustomGroupService.getAccountTypes());
    }

    @GetMapping("/api/custom-group/parent-accounts")
    public ResponseEntity<?> getParentAccounts() {
        return ResponseEntity.ok(accountCustomGroupService.getParentAccounts());
    }

    @GetMapping("/api/custom-group/allocated/{groupId}")
    public ResponseEntity<?> getAllocated(@PathVariable int groupId) {
        return ResponseEntity.ok(accountCustomGroupService.getAllocatedAccounts(groupId));
    }

    @GetMapping("/api/custom-group/unallocated/{groupId}")
    public ResponseEntity<?> getUnallocated(@PathVariable int groupId) {
        return ResponseEntity.ok(accountCustomGroupService.getUnAllocatedAccounts(groupId));
    }

    @PostMapping("/api/custom-group/allocate")
    public ResponseEntity<Void> allocate(@RequestParam int groupId, @RequestBody List<Integer> accountIds) {
        accountCustomGroupService.allocateAccounts(groupId, accountIds);
        return ResponseEntity.ok().build();
    }

    @PostMapping("/api/custom-group/unallocate")
    public ResponseEntity<Void> unallocate(@RequestParam int allocationId) {
        accountCustomGroupService.unAllocateAccounts(allocationId);
        return ResponseEntity.ok().build();
    }
}
