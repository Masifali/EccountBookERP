package com.mst.controllers;

import java.util.List;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import com.mst.services.VoucherDesktopConfigService;

/**
 * Configuration-driven lookups shared by the Accounts voucher pages (CPV/BPV/CRV/BRV/Contra/
 * Journal/Expense). See VoucherDesktopConfigService for the desktop source of each.
 */
@RestController
@RequestMapping("/accounts/api/vouchers")
public class VoucherDesktopConfigController {

	@Autowired
	private VoucherDesktopConfigService service;

	/** The desktop DefaultConfigurations() flags the voucher forms read on Load. */
	@GetMapping("/desktop-config")
	public ResponseEntity<Map<String, Object>> desktopConfig() {
		return ResponseEntity.ok(service.voucherFlags());
	}

	/** DetailAccountFill()/DebitAccountTitleFill() of the named form. */
	@GetMapping("/detail-accounts")
	public ResponseEntity<?> detailAccounts(@RequestParam("form") String form,
			@RequestParam(value = "query", defaultValue = "") String query) {
		List<Map<String, Object>> rows;
		try {
			rows = service.detailAccounts(form);
		} catch (IllegalArgumentException e) {
			return ResponseEntity.badRequest().body(Map.of("message", e.getMessage()));
		}
		String q = query.trim().toLowerCase();
		if (q.isEmpty()) return ResponseEntity.ok(rows);
		return ResponseEntity.ok(rows.stream().filter(r ->
				String.valueOf(r.get("accountTitle")).toLowerCase().contains(q)
				|| String.valueOf(r.get("accountCode")).toLowerCase().contains(q)).toList());
	}

	/** CheqNoFill(): outstanding cheques of the selected bank (CheqBookHeader.OutstandingCheqNo). */
	@GetMapping("/outstanding-cheques")
	public ResponseEntity<List<Map<String, Object>>> outstandingCheques(@RequestParam("bankId") int bankId,
			@RequestParam(value = "recId", defaultValue = "0") int recId) {
		return ResponseEntity.ok(service.outstandingCheques(bankId, recId));
	}
}
