package com.mst.serviceInterface;

import java.time.LocalDate;
import java.util.List;

import com.mst.models.BankReconciliation;

public interface IBankReconciliationService {

	List<BankReconciliation> saveList(List<BankReconciliation> items);

	List<BankReconciliation> getHistory(Integer bankAccountId, LocalDate fromDate, LocalDate toDate);

	void deleteById(Integer id);
}
