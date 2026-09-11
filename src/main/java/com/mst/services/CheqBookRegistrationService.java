package com.mst.services;

import com.mst.security.CurrentUserContext;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import java.util.stream.Collectors;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.ChartofAccount;
import com.mst.models.CheqBookDetail;
import com.mst.models.CheqBookHeader;
import com.mst.repositories.IChartofAccountRepository;
import com.mst.repositories.ICheqBookDetailRepository;
import com.mst.repositories.ICheqBookHeaderRepository;

@Service
public class CheqBookRegistrationService {

	@Autowired
	private ICheqBookHeaderRepository headerRepository;

	@Autowired
	private ICheqBookDetailRepository detailRepository;

	@Autowired
	private IChartofAccountRepository chartOfAccountRepository;

	@Autowired
	private CurrentUserContext currentUserContext;

	public List<ChartofAccount> getBankAccounts() {
		List<ChartofAccount> accounts = chartOfAccountRepository.findAllByOrderByAccountCode();
		return accounts.stream()
				.filter(a -> a.getAccountGroup() != null && "Detail".equalsIgnoreCase(a.getAccountGroup().trim()))
				.collect(Collectors.toList());
	}

	public Integer getNextDocNo() {
		Integer max = headerRepository.findMaxDocNo();
		return (max != null ? max : 0) + 1;
	}

	public List<CheqBookHeader> getAllHeaders() {
		List<CheqBookHeader> headers = headerRepository.findAllByOrderByIdDesc();
		List<ChartofAccount> allAccounts = chartOfAccountRepository.findAll();
		Map<Integer, String> accountMap = new HashMap<>();
		for (ChartofAccount a : allAccounts) {
			if (a.getId() != null) {
				accountMap.put(a.getId(), a.getAccountCode() + " - " + a.getAccountTitle());
			}
		}

		for (CheqBookHeader h : headers) {
			Integer accId = h.getBankId() != null ? h.getBankId() : h.getChartOfAccountId();
			if (accId != null && accountMap.containsKey(accId)) {
				h.setBankName(accountMap.get(accId));
				h.setAccountTitle(accountMap.get(accId));
			} else {
				h.setBankName("Account #" + accId);
				h.setAccountTitle("Account #" + accId);
			}
		}
		return headers;
	}

	public List<CheqBookDetail> getDetailsByHeaderId(Long headerId) {
		if (headerId == null) {
			return new ArrayList<>();
		}
		return detailRepository.findByCheqBookHeaderIdOrderByIdAsc(headerId);
	}

	@Transactional
	public String saveChequeBook(CheqBookHeader header) {
		if (header == null) {
			return "Invalid cheque book data.";
		}
		if (header.getBankId() == null || header.getBankId() <= 0) {
			return "Please select a valid Bank Account.";
		}
		if (header.getCbSrFrom() == null || header.getCbSrFrom().trim().isEmpty()) {
			return "Serial From is required.";
		}
		if (header.getCbSrTo() == null || header.getCbSrTo().trim().isEmpty()) {
			return "Serial To is required.";
		}

		long srFrom, srTo;
		try {
			srFrom = Long.parseLong(header.getCbSrFrom().trim());
			srTo = Long.parseLong(header.getCbSrTo().trim());
		} catch (NumberFormatException e) {
			return "Serial From and Serial To must be numeric numbers.";
		}

		if (srFrom > srTo) {
			return "Serial From cannot be greater than Serial To!";
		}
		if (srFrom == srTo) {
			return "Serial From and Serial To cannot be the same!";
		}
		long diff = srTo - srFrom;
		if (diff >= 500) {
			return "Cheques Quantity cannot exceed 500.";
		}

		if (header.getDocNo() == null || header.getDocNo() <= 0) {
			header.setDocNo(getNextDocNo());
		}
		if (header.getDocDate() == null) {
			header.setDocDate(LocalDate.now());
		}
		header.setChartOfAccountId(header.getBankId());
		header.setEntryUser(currentUserContext.currentUserId());
		header.setEntryDate(LocalDateTime.now());
		header.setCompanyId(currentUserContext.currentCompanyId());
		header.setOrganizationId(currentUserContext.currentOrganizationId());

		CheqBookHeader savedHeader = headerRepository.save(header);

		String prefix = header.getCbPrefix() != null ? header.getCbPrefix().trim() : "";
		int padLength = header.getCbSrFrom().trim().length();

		List<CheqBookDetail> detailsList = new ArrayList<>();
		for (long i = srFrom; i <= srTo; i++) {
			CheqBookDetail cd = new CheqBookDetail();
			cd.setCheqBookHeaderId(savedHeader.getId());

			String numStr = String.valueOf(i);
			if (padLength > numStr.length()) {
				numStr = String.format("%0" + padLength + "d", i);
			}

			cd.setCheqNo(prefix + numStr);
			cd.setCheqStatus("Blank");
			cd.setOtherRemarks(header.getRemarks());
			cd.setCheqCancelStatus(false);

			detailsList.add(cd);
		}

		detailRepository.saveAll(detailsList);

		return "SUCCESS";
	}

	@Transactional
	public boolean updateChequeStatus(Long detailId, String status, String remarks) {
		Optional<CheqBookDetail> opt = detailRepository.findById(detailId);
		if (opt.isPresent()) {
			CheqBookDetail d = opt.get();
			d.setCheqStatus(status);
			if (remarks != null && !remarks.trim().isEmpty()) {
				d.setOtherRemarks(remarks);
			}
			if ("Cancelled".equalsIgnoreCase(status)) {
				d.setCheqCancelStatus(true);
			} else if ("Blank".equalsIgnoreCase(status)) {
				d.setCheqCancelStatus(false);
			}
			d.setChequeStatusDate(LocalDateTime.now());
			d.setStatusChangeUserId(currentUserContext.currentUserId());
			detailRepository.save(d);
			return true;
		}
		return false;
	}
}
