package com.mst.controllers;

import java.time.LocalDate;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import com.mst.models.BankReconciliation;
import com.mst.models.ChartofAccount;
import com.mst.repositories.IChartofAccountRepository;
import com.mst.serviceInterface.IBankReconciliationService;

@RestController
@RequestMapping("/api/bank-reconciliation")
public class BankReconciliationController {

	@Autowired
	private IBankReconciliationService service;

	@Autowired
	private IChartofAccountRepository chartofAccountRepository;

	@GetMapping("/bank-accounts")
	public ResponseEntity<List<ChartofAccount>> getBankAccounts() {
		List<ChartofAccount> list = chartofAccountRepository.findBankAccounts();
		return ResponseEntity.ok(list);
	}

	@PostMapping("/save-list")
	public ResponseEntity<?> saveList(@RequestBody List<BankReconciliation> items) {
		try {
			List<BankReconciliation> saved = service.saveList(items);
			return ResponseEntity.ok(saved);
		} catch (IllegalArgumentException e) {
			return ResponseEntity.badRequest().body(e.getMessage());
		} catch (Exception e) {
			return ResponseEntity.internalServerError().body("Error saving bank reconciliation: " + e.getMessage());
		}
	}

	@GetMapping("/history")
	public ResponseEntity<List<BankReconciliation>> getHistory(
			@RequestParam(value = "bankAccountId", required = false) Integer bankAccountId,
			@RequestParam(value = "fromDate", required = false) @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate fromDate,
			@RequestParam(value = "toDate", required = false) @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate toDate) {
		List<BankReconciliation> history = service.getHistory(bankAccountId, fromDate, toDate);
		return ResponseEntity.ok(history);
	}

	@DeleteMapping("/{id}")
	public ResponseEntity<Void> deleteById(@PathVariable("id") Integer id) {
		service.deleteById(id);
		return ResponseEntity.ok().build();
	}
}

