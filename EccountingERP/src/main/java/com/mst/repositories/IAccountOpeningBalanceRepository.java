package com.mst.repositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;

import com.mst.models.AccountOpeningBalance;

public interface IAccountOpeningBalanceRepository extends JpaRepository<AccountOpeningBalance, Integer> {

	AccountOpeningBalance findByAccountCode(String accountCode);

	@Query("select coalesce(max(a.id), 0) from AccountOpeningBalance a")
	int findMaxId();
}
