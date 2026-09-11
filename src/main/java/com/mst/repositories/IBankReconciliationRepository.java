package com.mst.repositories;

import java.time.LocalDate;
import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

import com.mst.models.BankReconciliation;

public interface IBankReconciliationRepository extends JpaRepository<BankReconciliation, Integer> {

	@Query("select coalesce(max(b.id), 0) from BankReconciliation b")
	int findMaxId();

	List<BankReconciliation> findByBankAccountIdAndOrganizationIdAndCompanyId(Integer bankAccountId, Integer organizationId, Integer companyId);

	@Query("select b from BankReconciliation b where b.bankAccountId = :bankAccountId " +
	       "and (:fromDate is null or b.transactionDate >= :fromDate) " +
	       "and (:toDate is null or b.transactionDate <= :toDate) " +
	       "order by b.transactionDate desc, b.id desc")
	List<BankReconciliation> findHistory(
			@Param("bankAccountId") Integer bankAccountId,
			@Param("fromDate") LocalDate fromDate,
			@Param("toDate") LocalDate toDate);
}
