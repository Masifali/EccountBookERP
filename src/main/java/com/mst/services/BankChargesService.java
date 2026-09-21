package com.mst.services;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import com.mst.models.BankCharges;
import com.mst.repositories.IBankChargesRepository;
import com.mst.security.CurrentUserContext;
import com.mst.serviceInterface.IBankChargesService;

@Service("bankChargesService")
public class BankChargesService implements IBankChargesService {

	@Autowired
	private IBankChargesRepository bankChargesRepository;

	@Autowired
	private JdbcTemplate jdbcTemplate;

	@Autowired
	private CurrentUserContext currentUserContext;

	@Override
	public List<Map<String, Object>> getBankChargesList() {
		String sql = "SELECT bc.Id as id, bc.BankAccountId as bankAccountId, coa.AccountTitle as bankAccountTitle, " +
				"bc.ChargeDate as chargeDate, bc.ChargeType as chargeType, bc.Amount as amount, " +
				"bc.TaxAmount as taxAmount, bc.TotalAmount as totalAmount, bc.RefNo as refNo, bc.Remarks as remarks " +
				"FROM BankCharges bc " +
				"LEFT JOIN ChartofAccount coa ON bc.BankAccountId = coa.Id " +
				"ORDER BY bc.Id DESC";
		try {
			return jdbcTemplate.queryForList(sql);
		} catch (Exception e) {
			return new ArrayList<>();
		}
	}

	@Override
	public BankCharges getById(int id) {
		return bankChargesRepository.findById(id).orElse(null);
	}

	@Override
	public BankCharges saveBankCharges(BankCharges bankCharges) {
		if (bankCharges.getId() == null || bankCharges.getId() <= 0) {
			int maxId = bankChargesRepository.findMaxId();
			bankCharges.setId(maxId + 1);
			bankCharges.setEntryDate(LocalDateTime.now());
		}
		if (bankCharges.getOrganizationId() == null) bankCharges.setOrganizationId(currentUserContext.currentOrganizationId());
		if (bankCharges.getCompanyId() == null) bankCharges.setCompanyId(currentUserContext.currentCompanyId());

		return bankChargesRepository.save(bankCharges);
	}

	@Override
	public void deleteBankCharges(int id) {
		bankChargesRepository.deleteById(id);
	}
}
