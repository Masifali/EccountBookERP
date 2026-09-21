package com.mst.services;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import com.mst.models.Bank;
import com.mst.repositories.IBankRepository;
import com.mst.security.CurrentUserContext;
import com.mst.serviceInterface.IDefineBankService;

@Service("defineBankService")
public class DefineBankService implements IDefineBankService {

	@Autowired
	private IBankRepository bankRepository;

	@Autowired
	private JdbcTemplate jdbcTemplate;

	@Autowired
	private CurrentUserContext currentUserContext;

	@Override
	public List<Map<String, Object>> getBankList() {
		String sql = "SELECT b.Id as id, b.BranchCode as branchCode, b.BranchName as branchName, " +
				"b.BranchAddress as branchAddress, b.BranchCity as branchCity, b.CountryId as countryId, " +
				"b.Contact1Tel as contact1Tel, b.Contact2Tel as contact2Tel, b.Contact3Mobile as contact3Mobile, " +
				"b.emailPrimery as emailPrimery, b.emailAlternate as emailAlternate, b.IsHomeland as isHomeland, " +
				"b.OtherInfo as otherInfo, b.ChartOfAccountId as chartOfAccountId, b.ChequeTemplete as chequeTemplete, " +
				"b.BankAccountNo as bankAccountNo, b.BankIBANNo as bankIBANNo, b.BankAccountTitle as bankAccountTitle, " +
				"coa.AccountTitle as glAccountTitle, c.CityName as cityName " +
				"FROM Bank b " +
				"LEFT JOIN ChartofAccount coa ON b.ChartOfAccountId = coa.Id " +
				"LEFT JOIN City c ON b.BranchCity = c.Id " +
				"ORDER BY b.Id DESC";
		try {
			return jdbcTemplate.queryForList(sql);
		} catch (Exception e) {
			return new ArrayList<>();
		}
	}

	@Override
	public Bank getById(int id) {
		return bankRepository.findById(id).orElse(null);
	}

	@Override
	public Bank saveBank(Bank bank) {
		if (bank.getId() == null || bank.getId() <= 0) {
			int maxId = bankRepository.findMaxId();
			bank.setId(maxId + 1);
			bank.setEntryDate(LocalDateTime.now());
			bank.setEntryUser(1);
		} else {
			bank.setModifyDate(LocalDateTime.now());
			bank.setModifyUser(1);
		}
		if (bank.getOrganizationId() == null) bank.setOrganizationId(currentUserContext.currentOrganizationId());
		if (bank.getCompanyId() == null) bank.setCompanyId(currentUserContext.currentCompanyId());

		return bankRepository.save(bank);
	}

	@Override
	public void deleteBank(int id) {
		bankRepository.deleteById(id);
	}
}
