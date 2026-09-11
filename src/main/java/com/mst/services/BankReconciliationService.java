package com.mst.services;

import com.mst.security.CurrentUserContext;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.BankReconciliation;
import com.mst.repositories.IBankReconciliationRepository;
import com.mst.serviceInterface.IBankReconciliationService;

@Service
public class BankReconciliationService implements IBankReconciliationService {

	@Autowired
	private IBankReconciliationRepository repository;

	@Autowired
	private CurrentUserContext currentUserContext;

	@Override
	@Transactional
	public List<BankReconciliation> saveList(List<BankReconciliation> items) {
		if (items == null || items.isEmpty()) {
			return new ArrayList<>();
		}

		int maxId = repository.findMaxId();
		List<BankReconciliation> savedList = new ArrayList<>();

		for (int i = 0; i < items.size(); i++) {
			BankReconciliation item = items.get(i);

			if (item.getBankAccountId() == null || item.getBankAccountId() <= 0) {
				throw new IllegalArgumentException("Please select an account title.");
			}

			if (item.getParticulars() == null || item.getParticulars().trim().isEmpty()) {
				throw new IllegalArgumentException("Particulars cannot be empty in Row " + (i + 1));
			}

			BigDecimal debit = item.getDebit() != null ? item.getDebit() : BigDecimal.ZERO;
			BigDecimal credit = item.getCredit() != null ? item.getCredit() : BigDecimal.ZERO;

			boolean debitValid = debit.compareTo(BigDecimal.ZERO) > 0;
			boolean creditValid = credit.compareTo(BigDecimal.ZERO) > 0;

			if ((!debitValid && !creditValid) || (debitValid && creditValid)) {
				throw new IllegalArgumentException(
						"Either Debit or Credit must be greater than zero but not both In Row No " + (i + 1));
			}

			boolean isNew = item.getId() == null || item.getId() == 0;
			if (isNew) {
				maxId++;
				item.setId(maxId);
				item.setActionId(1); // 1 = Insert
				item.setEntryDate(LocalDateTime.now());
				item.setEntryUserId(currentUserContext.currentUserId());
			} else {
				item.setActionId(2); // 2 = Update
				item.setModifyDate(LocalDateTime.now());
				item.setModifyUserId(currentUserContext.currentUserId());
			}

			item.setDocumentTypeId(920); // Bank Reconciliation Upload Excel
			item.setOrganizationId(currentUserContext.currentOrganizationId());
			item.setCompanyId(currentUserContext.currentCompanyId());
			if (item.getBranchesId() == null) {
				item.setBranchesId(currentUserContext.currentBranchId());
			}
			if (item.getProjectsId() == null) {
				item.setProjectsId(item.getBranchesId());
			}
			if (item.getFinancialYearId() == null) {
				item.setFinancialYearId(currentUserContext.currentFinancialYearId());
			}
			if (item.getIsReconciled() == null) {
				item.setIsReconciled(false);
			}

			savedList.add(repository.save(item));
		}

		return savedList;
	}

	@Override
	public List<BankReconciliation> getHistory(Integer bankAccountId, LocalDate fromDate, LocalDate toDate) {
		return repository.findHistory(bankAccountId, fromDate, toDate);
	}

	@Override
	@Transactional
	public void deleteById(Integer id) {
		if (id != null && repository.existsById(id)) {
			repository.deleteById(id);
		}
	}
}
