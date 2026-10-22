package com.mst.repositories;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.ChartofAccount;
import com.mst.projections.AccountTitleByAccountTypeIdsRow;
import com.mst.projections.AccountsInfoByItemCategoryRow;
import com.mst.projections.CoaAccountTypeIdRow;
import com.mst.projections.CoaAvailableForCustomGroupRow;
import com.mst.projections.CoaByCustomGroupIdRow;
import com.mst.projections.CoaCompleteDetailRow;
import com.mst.projections.CoaDetailAccountRow;
import com.mst.projections.CoaDetailByAccountIdRow;
import com.mst.projections.CoaGeneratedCodeRow;
import com.mst.projections.CoaLevelIdsRow;
import com.mst.projections.CoaMobileAppRow;
import com.mst.projections.CoaNewCodeByParentCodeRow;
import com.mst.projections.CoaParentNoteRow;
import com.mst.projections.CoaReadAllAccountGroupRow;
import com.mst.projections.CoaReadByFinYearRow;
import com.mst.projections.CoaReadSpecificAccountsRow;

/**
 * dbo.ChartofAccount - Proc_ChartofAccount_Insert, Sp_ChartofAccount_GetAllMethodFromCOA (@CoaType branches),
 * USP_Accounts_GetAccountTitleByAccountTypeIds and usp_getAccountsInfoByItemCategoryId.
 *
 * Pre-insert logic of Proc_ChartofAccount_Insert (callers reproduce it around save()):
 *  1. AccountCode / AccountClass / AccountType are taken from dbo.GenerateNewAccountCodeByParentAccountCode
 *     (OrganizationId, CompanyId, ParentAccountCode) -> {@link #generateNewAccountCodeByParentAccountCode}.
 *  2. Id = ISNULL(MAX(Id),0)+1 -> {@link #maxId()}.
 *  3. Error if (OrganizationId, CompanyId, AccountCode) already exists -> {@link #countByOrgCompanyAndAccountCode}.
 *  4. If AccountGroup='Group' and the latest sibling under ParentAccountCode is 'Detail' -> error ("Please select type Detail first")
 *     -> {@link #lastAccountGroupUnderParent}.
 *  5. Account_Level 4/5: PLNoteId, BSNoteId, AccountTypeId, AccountClass copied from the parent row -> {@link #parentNoteAndType}.
 *  6. Account_Level 3 with PLNoteId=0 and BSNoteId=0 -> error.
 *  7. Lvl01Id..Lvl04Id are derived from Account_Level / ParentCodeId (level 2: Lvl01=Parent, Lvl02=Id; level 3: Lvl01 from parent,
 *     Lvl02=Parent, Lvl03=Id; level 4: Lvl01/02 from parent, Lvl03=Parent, Lvl04=Id; level 5 Detail: Lvl01/02/03 from parent,
 *     Lvl04=Parent) -> {@link #parentLevelIds}; missing ids raise an error.
 *  8. IsActive = 1; UserLogId defaults to UserAccount.LastUserLogId of EntryUser -> {@link #lastUserLogIdOfUser}.
 *  9. After the insert: SELECT @Id; EXEC USP_UserAudit_Insert ('Create Account ...', ScreenName 'AcfrmDefCoa', ActionId 1)
 *     -> UserAuditQueryRepository#uspUserAuditInsert; if @CustomGroupId > 0 a row is added to AccountsCustomGroups with
 *     SortNo = MAX(SortNo)+1 -> AccountsCustomGroupRepository#maxSortNo() + save().
 * ModifyDate / ModifyUser are NOT written by the insert proc.
 */
@Repository
public interface ChartofAccountRepository extends JpaRepository<ChartofAccount, Integer> {

    // =====================================================================================
    // Proc_ChartofAccount_Insert
    // =====================================================================================

    /** Proc_ChartofAccount_Insert - KEEP-EXEC: full procedural insert (code generation, validations, audit, custom group). All parameters default to NULL. */
    // KEEP-EXEC: multi-table procedural insert (RAISERROR validations, GenerateNewAccountCodeByParentAccountCode, USP_UserAudit_Insert, AccountsCustomGroups)
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.Proc_ChartofAccount_Insert @Id = :id, @AccountCode = :accountCode, @AccountTitle = :accountTitle, "
         + "@AccountTitleOtherLingo = :accountTitleOtherLingo, @AccountClass = :accountClass, @AccountTypeId = :accountTypeId, "
         + "@AccountGroup = :accountGroup, @Account_Level = :account_Level, @PLNoteId = :plNoteId, @BSNoteId = :bsNoteId, "
         + "@ParentAccountCode = :parentAccountCode, @OtherErpCode = :otherErpCode, @QrCode = :qrCode, @FinancialYearId = :financialYearId, "
         + "@IsActive = :isActive, @EntryDate = :entryDate, @EntryUser = :entryUser, @ModifyDate = :modifyDate, @ModifyUser = :modifyUser, "
         + "@PostDate = :postDate, @PostUser = :postUser, @PostState = :postState, @OrganizationId = :organizationId, @CompanyId = :companyId, "
         + "@ParentCodeId = :parentCodeId, @BranchId = :branchId, @CustomGroupId = :customGroupId, @CustomerGroupId = :customerGroupId, "
         + "@CityId = :cityId, @Lvl01Id = :lvl01Id, @Lvl02Id = :lvl02Id, @Lvl03Id = :lvl03Id, @Lvl04Id = :lvl04Id, "
         + "@CurrencyId = :currencyId, @ContactNo = :contactNo, @UserLogId = :userLogId", nativeQuery = true)
    void procChartofAccountInsert(@Param("id") Integer id, @Param("accountCode") String accountCode, @Param("accountTitle") String accountTitle,
            @Param("accountTitleOtherLingo") String accountTitleOtherLingo, @Param("accountClass") Integer accountClass,
            @Param("accountTypeId") Integer accountTypeId, @Param("accountGroup") String accountGroup, @Param("account_Level") Integer account_Level,
            @Param("plNoteId") Integer plNoteId, @Param("bsNoteId") Integer bsNoteId, @Param("parentAccountCode") String parentAccountCode,
            @Param("otherErpCode") String otherErpCode, @Param("qrCode") String qrCode, @Param("financialYearId") Integer financialYearId,
            @Param("isActive") Boolean isActive, @Param("entryDate") LocalDateTime entryDate, @Param("entryUser") Integer entryUser,
            @Param("modifyDate") LocalDateTime modifyDate, @Param("modifyUser") Integer modifyUser, @Param("postDate") LocalDateTime postDate,
            @Param("postUser") Integer postUser, @Param("postState") Boolean postState, @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("parentCodeId") Integer parentCodeId, @Param("branchId") Integer branchId,
            @Param("customGroupId") Integer customGroupId, @Param("customerGroupId") Integer customerGroupId, @Param("cityId") Integer cityId,
            @Param("lvl01Id") Integer lvl01Id, @Param("lvl02Id") Integer lvl02Id, @Param("lvl03Id") Integer lvl03Id, @Param("lvl04Id") Integer lvl04Id,
            @Param("currencyId") Integer currencyId, @Param("contactNo") String contactNo, @Param("userLogId") Long userLogId);

    /** Proc_ChartofAccount_Insert step 2: SELECT ISNULL(MAX(CONVERT(int,Id)),0) FROM ChartofAccount (caller adds 1). */
    @Query("select coalesce(max(e.id),0) from ChartofAccount e")
    Integer maxId();

    /** Proc_ChartofAccount_Insert step 1 / Sp_ChartofAccount_GetAllMethodFromCOA 'NewCodeByParentCode': table-valued function dbo.GenerateNewAccountCodeByParentAccountCode. */
    @Query(value = "SELECT AccountCode, AccountClass, AccountType, Account_Level, ParentAccountCode, ParentCodeTitle "
         + "FROM dbo.GenerateNewAccountCodeByParentAccountCode(:organizationId, :companyId, :parentAccountCode)", nativeQuery = true)
    List<CoaGeneratedCodeRow> generateNewAccountCodeByParentAccountCode(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("parentAccountCode") String parentAccountCode);

    /** Proc_ChartofAccount_Insert step 3: duplicate AccountCode check (count > 0 -> "AccountCode already exist"). */
    @Query("SELECT COUNT(c) FROM ChartofAccount c WHERE c.organizationId = :organizationId AND c.companyId = :companyId AND c.accountCode = :accountCode")
    Long countByOrgCompanyAndAccountCode(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("accountCode") String accountCode);

    /** Proc_ChartofAccount_Insert step 4: SELECT TOP 1 AccountGroup ... WHERE ParentAccountCode = @ParentAccountCode ORDER BY Id DESC. */
    @Query(value = "SELECT TOP 1 AccountGroup FROM ChartofAccount WHERE OrganizationId = :organizationId AND CompanyId = :companyId "
         + "AND ParentAccountCode = :parentAccountCode ORDER BY Id DESC", nativeQuery = true)
    List<String> lastAccountGroupUnderParent(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("parentAccountCode") String parentAccountCode);

    /** Proc_ChartofAccount_Insert step 5: PLNoteId, BSNoteId, AccountTypeId, AccountClass of the parent row (Account_Level 4/5). */
    @Query("SELECT c.plNoteId AS plNoteId, c.bsNoteId AS bsNoteId, c.accountTypeId AS accountTypeId, c.accountClass AS accountClass "
         + "FROM ChartofAccount c WHERE c.id = :parentCodeId")
    List<CoaParentNoteRow> parentNoteAndType(@Param("parentCodeId") Integer parentCodeId);

    /** Proc_ChartofAccount_Insert step 7: Lvl01Id, Lvl02Id, Lvl03Id of the parent row. */
    @Query("SELECT c.lvl01Id AS lvl01Id, c.lvl02Id AS lvl02Id, c.lvl03Id AS lvl03Id FROM ChartofAccount c WHERE c.id = :parentCodeId")
    List<CoaLevelIdsRow> parentLevelIds(@Param("parentCodeId") Integer parentCodeId);

    /** Proc_ChartofAccount_Insert step 8: SELECT LastUserLogId FROM UserAccount WHERE ID = @EntryUser (column not on the UserAccount entity). */
    @Query(value = "SELECT LastUserLogId FROM UserAccount WHERE ID = :entryUser", nativeQuery = true)
    List<Long> lastUserLogIdOfUser(@Param("entryUser") Integer entryUser);

    // =====================================================================================
    // Sp_ChartofAccount_GetAllMethodFromCOA
    // =====================================================================================

    /** Sp_ChartofAccount_GetAllMethodFromCOA @CoaType='NewCodeByParentCode' (the proc also reads CustomerGroupId of the row whose AccountCode = @ParentCode; result columns kept). */
    // KEEP-EXEC: result is built from scalar variables filled by dbo.GenerateNewAccountCodeByParentAccountCode
    @Query(value = "EXEC dbo.Sp_ChartofAccount_GetAllMethodFromCOA @OrganizationId = :organizationId, @CompanyId = :companyId, "
         + "@AccountCode = :accountCode, @CoaType = 'NewCodeByParentCode'", nativeQuery = true)
    List<CoaNewCodeByParentCodeRow> newCodeByParentCode(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("accountCode") String accountCode);

    /** Sp_ChartofAccount_GetAllMethodFromCOA @CoaType='ReadByFinancialYearIdNOrganizationIdNCompanyIdNParentAccountCode' (native: AccountNotes, ChartOfAccountMultiLingo). @LanguageId default NULL. */
    @Query(value = "SELECT DISTINCT coa.Id, "
         + "CASE WHEN :languageId > 0 THEN CASE WHEN L.AccountTitle IS NULL THEN coa.AccountTitle ELSE L.AccountTitle END ELSE coa.AccountTitle END AS AccountTitle, "
         + "coa.AccountCode, (coa.AccountCode) AS AccountCodeBigInt, coa.AccountClass, coa.AccountTypeId, coa.AccountGroup, coa.Account_Level, "
         + "coa.PLNoteId, coa.BSNoteId, coa.ParentAccountCode, parent.AccountTitle AS ParentAccountTitle, coa.AccountTitleOtherLingo, "
         + "coa.OtherErpCode, coa.QrCode, CASE WHEN coa.AccountGroup = 'Group' THEN 1 ELSE coaa.IsActive END AS IsActive, "
         + "coa.EntryDate, coa.EntryUser, coa.ModifyDate, coa.ModifyUser, coa.PostDate, coa.PostUser, coa.PostState, coa.OrganizationId, "
         + "coa.CompanyId, coa.FinancialYearId, AccountTypes.AccountType, AccountNotes_1.NoteTitle AS PLNote, AccountNotes.NoteTitle AS BsNote, "
         + "CASE WHEN coa.AccountClass = 2 THEN 'Assets' WHEN coa.AccountClass = 3 THEN 'Liability' WHEN coa.AccountClass = 1 THEN 'Capital' "
         + "WHEN coa.AccountClass = 5 THEN 'Revenue' WHEN coa.AccountClass = 4 THEN 'Expense' END AS AccountClassName "
         + "FROM ChartofAccount coa "
         + "LEFT JOIN ChartofAccount parent ON parent.Id = coa.ParentCodeId "
         + "LEFT JOIN AccountNotes ON coa.BSNoteId = AccountNotes.Id "
         + "LEFT JOIN AccountNotes AS AccountNotes_1 ON coa.PLNoteId = AccountNotes_1.Id "
         + "LEFT JOIN AccountTypes ON coa.AccountTypeId = AccountTypes.Id "
         + "LEFT JOIN COAAllocation coaa ON coa.Id = coaa.ChartofAccountId AND coa.CompanyId = coaa.CompanyId "
         + "LEFT JOIN ChartOfAccountMultiLingo L WITH (NOLOCK) ON coa.Id = L.ChartOfAccountId AND L.MultiLanguagesId = :languageId "
         + "WHERE coa.OrganizationId = :organizationId AND coa.CompanyId = :companyId "
         + "ORDER BY AccountCodeBigInt ASC", nativeQuery = true)
    List<CoaReadByFinYearRow> readByFinancialYearIdNOrganizationIdNCompanyIdNParentAccountCode(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("languageId") Integer languageId);

    /** Sp_ChartofAccount_GetAllMethodFromCOA @CoaType='ReadAllAccountGroup' (native: vAccountClasses, ChartOfAccountMultiLingo, fnSplitString). Optional filters default NULL. */
    @Query(value = "SELECT c.Id, c.AccountCode, "
         + "CASE WHEN :languageId > 0 THEN CASE WHEN L.AccountTitle IS NULL THEN c.AccountTitle ELSE L.AccountTitle END ELSE c.AccountTitle END AS AccountTitle, "
         + "c.AccountClass, c.AccountTypeId, c.AccountGroup, c.Account_Level, c.PLNoteId, c.BSNoteId, c.ParentAccountCode, c.AccountTitleOtherLingo, "
         + "c.OtherErpCode, c.QrCode, c.IsActive, c.FinancialYearId, c.EntryDate, c.EntryUser, c.ModifyDate, c.ModifyUser, c.PostDate, c.PostUser, "
         + "c.PostState, c.OrganizationId, c.CompanyId, Parent.AccountTitle AS ParentAccountTitle, AC.ClassName, AT.AccountType "
         + "FROM ChartofAccount c WITH (NOLOCK) "
         + "INNER JOIN vAccountClasses AC WITH (NOLOCK) ON AC.ClassId = c.AccountClass "
         + "LEFT JOIN ChartofAccount Parent WITH (NOLOCK) ON Parent.Id = c.ParentCodeId "
         + "LEFT JOIN AccountTypes AT ON AT.Id = c.AccountTypeId "
         + "LEFT JOIN ChartOfAccountMultiLingo L WITH (NOLOCK) ON c.Id = L.ChartOfAccountId AND L.MultiLanguagesId = :languageId "
         + "WHERE (c.AccountGroup = 'Group') AND c.OrganizationId = :organizationId AND c.CompanyId = :companyId "
         + "AND (:account_Level IS NULL OR c.Account_Level = :account_Level) "
         + "AND (:accountTypeId IS NULL OR c.AccountTypeId = :accountTypeId) "
         + "AND (:accountClassId IS NULL OR c.AccountClass = :accountClassId) "
         + "AND (:accountClassIds IS NULL OR c.AccountClass IN (SELECT data FROM dbo.fnSplitString(:accountClassIds, ','))) "
         + "AND (:accountTypeIds IS NULL OR c.AccountTypeId IN (SELECT data FROM dbo.fnSplitString(:accountTypeIds, ',')))", nativeQuery = true)
    List<CoaReadAllAccountGroupRow> readAllAccountGroup(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("languageId") Integer languageId, @Param("account_Level") Integer account_Level, @Param("accountTypeId") Integer accountTypeId,
            @Param("accountClassId") Integer accountClassId, @Param("accountClassIds") String accountClassIds, @Param("accountTypeIds") String accountTypeIds);

    /** Sp_ChartofAccount_GetAllMethodFromCOA @CoaType='ReadSpecificAccounts' (ORDER BY AccountCode). */
    @Query("SELECT c.id AS id, c.accountCode AS accountCode, c.accountTitle AS accountTitle, c.accountClass AS accountClass, "
         + "c.accountTypeId AS accountTypeId, c.accountGroup AS accountGroup, c.accountLevel AS account_Level, c.plNoteId AS plNoteId, "
         + "c.bsNoteId AS bsNoteId, c.parentAccountCode AS parentAccountCode, a.isActive AS isActive, c.entryDate AS entryDate, "
         + "c.entryUser AS entryUser, c.modifyDate AS modifyDate, c.modifyUser AS modifyUser, c.postDate AS postDate, c.postUser AS postUser, "
         + "c.postState AS postState, c.organizationId AS organizationId, a.companyId AS companyId, c.financialYearId AS financialYearId "
         + "FROM ChartofAccount c INNER JOIN COAAllocation a ON c.id = a.chartofAccountId "
         + "WHERE c.organizationId = :organizationId AND a.companyId = :companyId AND c.accountGroup = :accountGroup "
         + "ORDER BY c.accountCode")
    List<CoaReadSpecificAccountsRow> readSpecificAccounts(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("accountGroup") String accountGroup);

    /** Sp_ChartofAccount_GetAllMethodFromCOA @CoaType='DetailAccount' (RAISERROR when @UsersId is 0/NULL; #TmpCostCenterParties for @AppId 5; portal GL filter for @AppId 4/6). */
    // KEEP-EXEC: temp tables (#TmpCostCenterParties, #tmpAllcoateAccounts) + RAISERROR
    @Query(value = "EXEC dbo.Sp_ChartofAccount_GetAllMethodFromCOA @OrganizationId = :organizationId, @CompanyId = :companyId, "
         + "@LanguageId = :languageId, @UsersId = :usersId, @CostCenterId = :costCenterId, @AppId = :appId, @CoaType = 'DetailAccount'", nativeQuery = true)
    List<CoaDetailAccountRow> detailAccount(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("languageId") Integer languageId, @Param("usersId") Integer usersId, @Param("costCenterId") Integer costCenterId,
            @Param("appId") Integer appId);

    /** Sp_ChartofAccount_GetAllMethodFromCOA @CoaType='GetAccountsAvailableForAcCustomGroup' (Detail accounts not yet in any AccountsCustomGroups row; ORDER BY AccountTitle). */
    @Query("SELECT a.chartofAccountId AS id, c.accountCode AS accountCode, c.accountTitle AS accountTitle, c.accountTypeId AS accountTypeId "
         + "FROM COAAllocation a INNER JOIN ChartofAccount c ON a.chartofAccountId = c.id AND a.companyId = c.companyId "
         + "LEFT JOIN AccountsCustomGroup acg ON c.id = acg.chartOfAccountId "
         + "WHERE c.organizationId = :organizationId AND c.companyId = :companyId AND c.accountGroup = 'Detail' AND acg.chartOfAccountId IS NULL "
         + "ORDER BY c.accountTitle")
    List<CoaAvailableForCustomGroupRow> getAccountsAvailableForAcCustomGroup(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId);

    /** Sp_ChartofAccount_GetAllMethodFromCOA @CoaType='GetAllAccountsByCustomGroupId' (ORDER BY AccountTitle). */
    @Query("SELECT acg.acLookUpsId AS acLookUpsId, acg.chartOfAccountId AS chartOfAccountId, c.accountCode AS accountCode, c.accountTitle AS accountTitle "
         + "FROM AccountsCustomGroup acg INNER JOIN ChartofAccount c ON acg.chartOfAccountId = c.id "
         + "WHERE c.organizationId = :organizationId AND c.companyId = :companyId AND acg.acLookUpsId = :acLookUpsId "
         + "ORDER BY c.accountTitle")
    List<CoaByCustomGroupIdRow> getAllAccountsByCustomGroupId(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("acLookUpsId") Integer acLookUpsId);

    /** Sp_ChartofAccount_GetAllMethodFromCOA @CoaType='DeleteAccountsFromCustomGroup' (DELETE AccountsCustomGroups WHERE AcLookUpsId=@AcLookUpsId AND ChartOfAccountId=@Id). */
    @Modifying
    @Transactional
    @Query("DELETE FROM AccountsCustomGroup g WHERE g.acLookUpsId = :acLookUpsId AND g.chartOfAccountId = :id")
    int deleteAccountsFromCustomGroup(@Param("acLookUpsId") Integer acLookUpsId, @Param("id") Integer id);

    /** Sp_ChartofAccount_GetAllMethodFromCOA @CoaType='GetAccountCodeForAcTitle' (native: fnSplitString over @ParentAccountCode which carries a CSV of AccountTitles). */
    @Query(value = "SELECT AccountCode FROM ChartofAccount C WITH (NOLOCK) INNER JOIN COAAllocation COA WITH (NOLOCK) ON C.Id = COA.ChartofAccountId "
         + "WHERE (AccountGroup = 'Group') AND OrganizationId = :organizationId AND COA.CompanyId = :companyId AND FinancialYearId = :financialYearId "
         + "AND AccountTitle IN (SELECT data FROM dbo.fnSplitString(:parentAccountCode, ','))", nativeQuery = true)
    List<String> getAccountCodeForAcTitle(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("financialYearId") Integer financialYearId, @Param("parentAccountCode") String parentAccountCode);

    /** Sp_ChartofAccount_GetAllMethodFromCOA @CoaType='GetAccountTypeId' (@Id = COAAllocation.ChartofAccountId; detail + active only). */
    @Query("SELECT c.accountTypeId AS accountTypeId, c.customerGroupId AS customerGroupId "
         + "FROM COAAllocation a INNER JOIN ChartofAccount c ON a.chartofAccountId = c.id "
         + "WHERE c.organizationId = :organizationId AND a.companyId = :companyId AND a.chartofAccountId = :id "
         + "AND c.accountGroup = 'detail' AND a.isActive = true")
    List<CoaAccountTypeIdRow> getAccountTypeId(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("id") Integer id);

    /**
     * Sp_ChartofAccount_GetAllMethodFromCOA @CoaType='GetAllDetailAccountsForMobileApp' (native: OFFSET/FETCH paging, ChartOfAccountMultiLingo).
     * Callers normalise paging as the proc does: PageNumber <= 0 -> 1, PageSize <= 0 -> 1, skipRows = (PageNumber-1)*PageSize.
     */
    @Query(value = "SELECT coa.ChartofAccountId AS Id, c.AccountCode, "
         + "CASE WHEN :languageId > 0 THEN CASE WHEN L.AccountTitle IS NULL THEN c.AccountTitle ELSE L.AccountTitle END ELSE c.AccountTitle END AS AccountTitle, "
         + "AccountTypeId "
         + "FROM ChartofAccount c WITH (NOLOCK) "
         + "INNER JOIN COAAllocation coa WITH (NOLOCK) ON c.Id = coa.ChartofAccountId "
         + "LEFT JOIN ChartOfAccountMultiLingo L WITH (NOLOCK) ON c.Id = L.ChartOfAccountId AND L.MultiLanguagesId = :languageId "
         + "WHERE c.OrganizationId = :organizationId AND coa.CompanyId = :companyId AND c.AccountGroup = 'Detail' "
         + "AND (:keyword IS NULL OR c.AccountTitle LIKE '%' + :keyword + '%') "
         + "ORDER BY Id ASC OFFSET :skipRows ROWS FETCH NEXT :pageSize ROWS ONLY", nativeQuery = true)
    List<CoaMobileAppRow> getAllDetailAccountsForMobileApp(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("languageId") Integer languageId, @Param("keyword") String keyword, @Param("skipRows") Integer skipRows,
            @Param("pageSize") Integer pageSize);

    /** Sp_ChartofAccount_GetAllMethodFromCOA @CoaType='GetCoaDetailByAccountId' (native: vAccountClasses). */
    @Query(value = "SELECT DISTINCT c.AccountCode, tp.AccountType, cl.ClassName FROM ChartofAccount c "
         + "INNER JOIN AccountTypes tp ON c.AccountTypeId = tp.Id INNER JOIN vAccountClasses cl ON c.AccountClass = cl.ClassId "
         + "WHERE c.OrganizationId = :organizationId AND c.CompanyId = :companyId AND c.Id = :id", nativeQuery = true)
    List<CoaDetailByAccountIdRow> getCoaDetailByAccountId(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("id") Integer id);

    /** Sp_ChartofAccount_GetAllMethodFromCOA @CoaType='GetCompleteCoaDetailByAccountId' (native: c.* + AccountType + ClassName from vAccountClasses). */
    @Query(value = "SELECT DISTINCT c.*, tp.AccountType, cl.ClassName FROM ChartofAccount c "
         + "INNER JOIN AccountTypes tp ON c.AccountTypeId = tp.Id INNER JOIN vAccountClasses cl ON c.AccountClass = cl.ClassId "
         + "WHERE c.OrganizationId = :organizationId AND c.CompanyId = :companyId AND c.Id = :id", nativeQuery = true)
    List<CoaCompleteDetailRow> getCompleteCoaDetailByAccountId(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("id") Integer id);

    // =====================================================================================
    // USP_Accounts_GetAccountTitleByAccountTypeIds
    // =====================================================================================

    /** USP_Accounts_GetAccountTitleByAccountTypeIds (single branch). RAISERROR when @UserId is 0/NULL; @AccountTypeIds/@AccountTypeIdsNot CSV; @AppId/@CostCenterId default NULL. */
    // KEEP-EXEC: #TmpCostCenterParties / #TmpAccounts temp tables, TmpShowHideStatusWithAccounts status variable, RAISERROR
    @Query(value = "EXEC dbo.USP_Accounts_GetAccountTitleByAccountTypeIds @OrganizationId = :organizationId, @CompanyId = :companyId, "
         + "@UserId = :userId, @AccountTypeIds = :accountTypeIds, @AccountTypeIdsNot = :accountTypeIdsNot, @AppId = :appId, "
         + "@CostCenterId = :costCenterId", nativeQuery = true)
    List<AccountTitleByAccountTypeIdsRow> uspAccountsGetAccountTitleByAccountTypeIds(@Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId, @Param("userId") Integer userId, @Param("accountTypeIds") String accountTypeIds,
            @Param("accountTypeIdsNot") String accountTypeIdsNot, @Param("appId") Integer appId, @Param("costCenterId") Integer costCenterId);

    // =====================================================================================
    // usp_getAccountsInfoByItemCategoryId
    // =====================================================================================

    /**
     * usp_getAccountsInfoByItemCategoryId @Activity='Stock': the ItemCategory's InventoryAccountId row, only when it is a
     * level-3 Group account. The proc RAISERRORs 'The Category Does Not Have Group Account Allocated' otherwise - here the
     * caller treats an empty list as that error.
     */
    @Query("SELECT c.id AS parentCodeId, c.accountCode AS parentAccountCode, c.accountClass AS accountClass, c.accountTypeId AS accountTypeId, "
         + "c.bsNoteId AS bsNoteId, c.plNoteId AS plNoteId, c.accountLevel AS account_Level "
         + "FROM ItemCategory ic INNER JOIN ChartofAccount c ON c.id = ic.inventoryAccountId "
         + "WHERE ic.id = :itemCategoryId AND c.accountLevel = 3 AND c.accountGroup = 'Group'")
    List<AccountsInfoByItemCategoryRow> getAccountsInfoByItemCategoryIdStock(@Param("itemCategoryId") Integer itemCategoryId);

    /** usp_getAccountsInfoByItemCategoryId @Activity='Sale': same as Stock but for the ItemCategory's RevenueAccountId (empty list = proc's RAISERROR). */
    @Query("SELECT c.id AS parentCodeId, c.accountCode AS parentAccountCode, c.accountClass AS accountClass, c.accountTypeId AS accountTypeId, "
         + "c.bsNoteId AS bsNoteId, c.plNoteId AS plNoteId, c.accountLevel AS account_Level "
         + "FROM ItemCategory ic INNER JOIN ChartofAccount c ON c.id = ic.revenueAccountId "
         + "WHERE ic.id = :itemCategoryId AND c.accountLevel = 3 AND c.accountGroup = 'Group'")
    List<AccountsInfoByItemCategoryRow> getAccountsInfoByItemCategoryIdSale(@Param("itemCategoryId") Integer itemCategoryId);
}
