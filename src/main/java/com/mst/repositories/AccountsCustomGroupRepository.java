package com.mst.repositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

import com.mst.models.AccountsCustomGroup;

/**
 * dbo.AccountsCustomGroups - the trailing step of Proc_ChartofAccount_Insert: when @CustomGroupId > 0 the proc inserts
 * (AcLookUpsId=@CustomGroupId, ChartOfAccountId=@Id, SortNo=MAX(SortNo)+1, EntryDate, EntryUserId=@EntryUser, organizationId, companyId)
 * -> {@link #maxSortNo()} + 1 and save(). The DELETE branch of Sp_ChartofAccount_GetAllMethodFromCOA ('DeleteAccountsFromCustomGroup')
 * lives in ChartofAccountRepository#deleteAccountsFromCustomGroup.
 */
@Repository
public interface AccountsCustomGroupRepository extends JpaRepository<AccountsCustomGroup, Integer> {

    /** Proc_ChartofAccount_Insert: SELECT ISNULL(MAX(CONVERT(int,SortNo)),0) FROM AccountsCustomGroups (caller adds 1). */
    @Query("select coalesce(max(e.sortNo),0) from AccountsCustomGroup e")
    Integer maxSortNo();
}
