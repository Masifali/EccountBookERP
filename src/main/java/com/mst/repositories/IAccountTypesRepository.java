package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;

import com.mst.models.AccountTypes;

/** Ditto of Architecture.BLL.Accounts.AccountTypes::GetAll (Proc_AccountTypes_ReadAll). */
public interface IAccountTypesRepository extends JpaRepository<AccountTypes, Integer> {
	List<AccountTypes> findAllByOrderById();
}
