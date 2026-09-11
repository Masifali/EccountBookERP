package com.mst.services;

import com.mst.security.CurrentUserContext;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.mst.messages.OpeningBalanceRow;
import com.mst.models.AccountOpeningBalance;
import com.mst.models.ChartofAccount;
import com.mst.repositories.IAccountOpeningBalanceRepository;
import com.mst.serviceInterface.IAccountOpeningBalanceService;
import com.mst.serviceInterface.IChartofAccountService;

/**
 * Ditto of the desktop's Architecture.BLL.Accounts.AccountsOpeningBalances, backing the
 * AcfrmOpeningBalance.cs "Save All" grid - one upserted dbo.AccountsOpeningBalances row
 * per Detail account, keyed by AccountCode (matches the real table: one row expected per
 * account, not a running history).
 */
@Service
public class AccountOpeningBalanceService implements IAccountOpeningBalanceService {

	@Autowired
	private IAccountOpeningBalanceRepository openingBalanceRepository;
	@Autowired
	private IChartofAccountService chartofAccountService;
	@Autowired
	private CurrentUserContext currentUserContext;

	@Override
	public List<OpeningBalanceRow> listRows() {
		List<OpeningBalanceRow> rows = new ArrayList<>();
		for (ChartofAccount account : chartofAccountService.getDetailAccounts()) {
			AccountOpeningBalance existing = openingBalanceRepository.findByAccountCode(account.getAccountCode());
			OpeningBalanceRow row = new OpeningBalanceRow();
			row.setAccountCode(account.getAccountCode());
			row.setAccountTitle(account.getAccountTitle());
			row.setOpeningDebit(existing != null && existing.getYearObDebit() != null
					? BigDecimal.valueOf(existing.getYearObDebit()) : BigDecimal.ZERO);
			row.setOpeningCredit(existing != null && existing.getYearObCredit() != null
					? BigDecimal.valueOf(existing.getYearObCredit()) : BigDecimal.ZERO);
			rows.add(row);
		}
		return rows;
	}

	@Override
	public void save(String accountCode, BigDecimal debit, BigDecimal credit) {
		ChartofAccount account = chartofAccountService.getByCode(accountCode);
		if (account == null) {
			return;
		}

		AccountOpeningBalance balance = openingBalanceRepository.findByAccountCode(accountCode);
		boolean isNew = balance == null;
		if (isNew) {
			balance = new AccountOpeningBalance();
			balance.setId(openingBalanceRepository.findMaxId() + 1);
			balance.setChartOfAccountId(account.getId());
			balance.setAccountCode(accountCode);
			balance.setOrganizationId(currentUserContext.currentOrganizationId());
			balance.setCompanyId(currentUserContext.currentCompanyId());
			balance.setEntryDate(LocalDateTime.now());
			balance.setEntryUser(currentUserContext.currentUserId());
		} else {
			balance.setModifyDate(LocalDateTime.now());
			balance.setModifyUser(currentUserContext.currentUserId());
		}

		balance.setChartOfAccountTitle(account.getAccountTitle());
		balance.setYearObDebit(debit != null ? debit.doubleValue() : 0d);
		balance.setYearObCredit(credit != null ? credit.doubleValue() : 0d);
		balance.setFinancialYearId(account.getFinancialYearId());

		openingBalanceRepository.save(balance);
	}

	@Override
	public BigDecimal getOpeningDebit(String accountCode) {
		AccountOpeningBalance existing = openingBalanceRepository.findByAccountCode(accountCode);
		return existing != null && existing.getYearObDebit() != null
				? BigDecimal.valueOf(existing.getYearObDebit())
				: BigDecimal.ZERO;
	}
}
