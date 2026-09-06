package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;

import com.mst.models.AccountsCustomGroup;

public interface IAccountsCustomGroupRepository extends JpaRepository<AccountsCustomGroup, Integer> {

	List<AccountsCustomGroup> findByChartOfAccountId(Integer chartOfAccountId);

	AccountsCustomGroup findByChartOfAccountIdAndAcLookUpsId(Integer chartOfAccountId, Integer acLookUpsId);

	void deleteByChartOfAccountIdAndAcLookUpsId(Integer chartOfAccountId, Integer acLookUpsId);

	long countByAcLookUpsId(Integer acLookUpsId);

	@Query("select coalesce(max(g.sortNo), 0) from AccountsCustomGroup g")
	int findMaxSortNo();
}
