package com.mst.services;

import com.mst.security.CurrentUserContext;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.ChartofAccount;
import com.mst.models.DocumentType;
import com.mst.models.VoucherDetail;
import com.mst.models.VoucherHead;
import com.mst.models.dto.VoucherRequestDto;
import com.mst.repositories.IChartofAccountRepository;
import com.mst.repositories.IDocumentTypeRepository;
import com.mst.repositories.IVoucherDetailRepository;
import com.mst.repositories.IVoucherHeadRepository;
import com.mst.serviceInterface.IVoucherService;

@Service("voucherService")
public class VoucherService implements IVoucherService {

	@Autowired
	private JdbcTemplate jdbcTemplate;
	@Autowired
	private IVoucherHeadRepository voucherHeadRepository;
	@Autowired
	private IVoucherDetailRepository voucherDetailRepository;
	@Autowired
	private IDocumentTypeRepository documentTypeRepository;
	@Autowired
	private IChartofAccountRepository chartofAccountRepository;
	@Autowired
	private CurrentUserContext currentUserContext;

	@Override
	public int generateNextVoucherCode(int documentTypeId) {
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		int branchId = currentUserContext.currentBranchId();
		int finYearId = currentUserContext.currentFinancialYearId();
		try {
			List<Map<String, Object>> list = jdbcTemplate.queryForList(
				"EXEC Sp_Vouchers_GetMethods @Activity=?, @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @FinancialYearId=?, @BranchesId=?",
				"GenerateVoucherCodeByDocumentTypeId", orgId, compId, documentTypeId, finYearId, branchId
			);
			if (list != null && !list.isEmpty()) {
				Map<String, Object> row = list.get(0);
				Object val = row.get("VoucherCode");
				if (val == null) val = row.get("VoucherNo");
				if (val == null && !row.values().isEmpty()) val = row.values().iterator().next();
				if (val != null) {
					return Integer.parseInt(val.toString());
				}
			}
		} catch (Exception ex) {
			// Fallback to query MAX(VoucherCode) if procedure throws or returns empty
			try {
				String sql = "SELECT ISNULL(MAX(CAST(VoucherCode AS INT)), 0) + 1 FROM VoucherHead WHERE DocumentTypeId = ? AND OrganizationId = ? AND CompanyId = ?";
				Integer nextCode = jdbcTemplate.queryForObject(sql, Integer.class, documentTypeId, orgId, compId);
				return nextCode != null ? nextCode : 1;
			} catch (Exception e) {
				return 1;
			}
		}
		return 1;
	}

	@Override
	public List<VoucherHead> getVouchersByDocumentType(int documentTypeId) {
		return voucherHeadRepository.findByDocumentType_IdOrderByVoucherDateDescIdDesc(documentTypeId);
	}

	@Override
	public Map<String, Object> getVoucherById(int voucherHeadId) {
		Map<String, Object> result = new HashMap<>();
		VoucherHead head = voucherHeadRepository.findById(voucherHeadId).orElse(null);
		if (head == null) return null;

		List<VoucherDetail> details = voucherDetailRepository.findByVoucherHead_IdOrderByIdAsc(voucherHeadId);
		result.put("header", head);

		List<Map<String, Object>> detailMaps = new ArrayList<>();
		for (VoucherDetail detail : details) {
			Map<String, Object> dMap = new HashMap<>();
			dMap.put("id", detail.getId());
			dMap.put("accountId", detail.getAccount() != null ? detail.getAccount().getId() : null);
			dMap.put("accountTitle", detail.getAccount() != null ? detail.getAccount().getAccountTitle() : "");
			dMap.put("accountCode", detail.getAccount() != null ? detail.getAccount().getAccountCode() : "");
			dMap.put("debitAmount", detail.getDebitAmount() != null ? detail.getDebitAmount() : 0.0);
			dMap.put("creditAmount", detail.getCreditAmount() != null ? detail.getCreditAmount() : 0.0);
			dMap.put("comments", detail.getComments() != null ? detail.getComments() : "");
			// Was previously omitted here (every voucher's Edit-reload silently lost these columns,
			// e.g. Party Receipt Voucher's own ChequeNo column always came back blank on Edit) -
			// added now since PDC Payment's grid needs all of them to correctly re-populate on Edit.
			// Harmless additive fields for every other voucher's already-working Edit reload.
			dMap.put("jobLotId", detail.getJobLotId());
			dMap.put("costCenterId", detail.getCostCenterId());
			dMap.put("chequeNoDetail", detail.getCheqNoDetail());
			dMap.put("chequeDateDetail", detail.getDCheqDate());
			dMap.put("refInvoiceNo", detail.getRefInvoiceNo());
			dMap.put("supplierCustomerId", detail.getSupplierCustomerId());
			dMap.put("paymentType", detail.getPaymentType());
			dMap.put("commentsOtherLingo", detail.getCommentsOtherLingo());
			dMap.put("againstAccountId", detail.getAgainstAccountId());
			dMap.put("locationTypeId", detail.getLocationTypeId());
			// Payment By Invoice (DocumentTypeId=1/2 via this alternate entry screen) Edit-reload
			// fields - ditto PaymentByInvoiceVoucherNew.cs's HistoryGridFill(): OrderNo (InvoiceId)/
			// DocumentTypeIdRef/QtyIn/QtyOut/ItemAmount/WhtHolding for a real detail line, and
			// TaxTypeId/TaxPrcnt/TaxesTotalAmount for the one extra WHT row (read directly off that
			// row's own columns here, rather than gating on DebitAmount/CreditAmount>0 the way
			// desktop's own HistoryGridFill() does - a real, evidence-based pre-existing quirk in
			// that method that would otherwise leave WHT Credit Account/Tax Type/Tax % blank on every
			// Edit reload, since Insert() only ever sets DebitAmount on this one row, never
			// CreditAmount). Additive/harmless for every other voucher that never sends them.
			dMap.put("orderNo", detail.getOrderNo());
			dMap.put("documentTypeIdRef", detail.getDocumentTypeIdRef());
			dMap.put("qtyIn", detail.getQtyIn());
			dMap.put("qtyOut", detail.getQtyOut());
			dMap.put("itemAmount", detail.getItemAmount());
			dMap.put("whtHolding", detail.getWhtHolding());
			dMap.put("taxTypeId", detail.getTaxTypeId());
			dMap.put("taxPrcnt", detail.getTaxPrcnt());
			dMap.put("taxesTotalAmount", detail.getTaxesTotalAmount());
			dMap.put("isTaxable", detail.getIsTaxable());
			detailMaps.add(dMap);
		}
		result.put("details", detailMaps);
		return result;
	}

	@Override
	@Transactional
	public Map<String, Object> saveVoucher(VoucherRequestDto dto) {
		Map<String, Object> response = new HashMap<>();

		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		int branchId = currentUserContext.currentBranchId();
		int userId = currentUserContext.currentUserId();
		int finYearId = currentUserContext.currentFinancialYearId();

		DocumentType docType = documentTypeRepository.findById(dto.getDocumentTypeId()).orElse(null);
		if (docType == null) {
			response.put("success", false);
			response.put("message", "Invalid Document Type ID: " + dto.getDocumentTypeId());
			return response;
		}

		VoucherHead head;
		if (dto.getId() != null && dto.getId() > 0) {
			head = voucherHeadRepository.findById(dto.getId()).orElse(new VoucherHead());
			head.setModifyDate(LocalDateTime.now());
			head.setModifyUser(userId);
		} else {
			head = new VoucherHead();
			head.setEntryDate(LocalDateTime.now());
			head.setEntryUser(userId);
			head.setOrganizationId(orgId);
			head.setCompanyId(compId);
			head.setBranchId(branchId);
			head.setFinancialYearId(finYearId);
		}

		head.setDocumentType(docType);
		head.setVoucherCode(dto.getVoucherCode() != null && dto.getVoucherCode() > 0 ? dto.getVoucherCode() : generateNextVoucherCode(docType.getId()));
		head.setVoucherDate(dto.getVoucherDate() != null ? dto.getVoucherDate() : LocalDate.now());
		head.setRemarks(dto.getRemarks());
		head.setVoucherAmount(dto.getVoucherAmount());
		head.setRefAccountId(dto.getRefAccountId());
		head.setAgainstAccountId(dto.getAgainstAccountId());
		head.setChequeNo(dto.getChequeNo());
		head.setPayTitle(dto.getPayTitle());
		head.setManualBillNo(dto.getManualBillNo());
		head.setBillAmount(dto.getBillAmount());
		// Expense Voucher (DocumentTypeId=26) header fields - ditto ExpenseVoucherNew.cs's Insert().
		// Left null for every other voucher type that never sends them (no behavior change).
		if (dto.getProjectId() != null) head.setProjectId(dto.getProjectId());
		if (dto.getMultiCurrencyId() != null) head.setMultiCurrencyId(dto.getMultiCurrencyId());
		if (dto.getExchangeCurrencyRate() != null) head.setExchangeCurrencyRate(dto.getExchangeCurrencyRate());
		if (dto.getFcAmount() != null) head.setFcAmount(dto.getFcAmount());
		// PDC Payment (DocumentTypeId=24) header field - ditto PostDatedCheqPaymentVouchers.cs's
		// Insert(): vh.RemarksOtherLingo = Conversion.ToString((object)txtReceivedBy.Text) ("Received
		// By" - a real VoucherHead column reused for this unrelated purpose on this one form).
		if (dto.getRemarksOtherLingo() != null) head.setRemarksOtherLingo(dto.getRemarksOtherLingo());
		// CPV/BPV (DocumentTypeId=1/2) WHT header fields - ditto PaymentVoucherNew.cs's Insert():
		// vh.RefDocNoId (CmbAgainstAc, the "WHT Debit Ac" field) / vh.IncludeWHT
		// (ChkBoxWthHolding.Checked). The WHT "Credit Account" reuses head.AgainstAccountId, already
		// set above from dto.getAgainstAccountId() unconditionally for every voucher.
		if (dto.getRefDocNoId() != null) head.setRefDocNoId(dto.getRefDocNoId());
		if (dto.getIncludeWht() != null) head.setIncludeWHT(dto.getIncludeWht());
		// Payment By Invoice (DocumentTypeId=1/2 via this alternate entry screen) header fields -
		// ditto PaymentByInvoiceVoucherNew.cs's Insert(): vh.BranchId (combbranch - overridable per
		// voucher on this one form, unlike every other voucher which only ever uses the session's
		// own current branch, already set above) and vh.ChequeDate (CheqDate.Value - a real
		// VoucherHead column that was declared on this DTO from the start but never actually wired
		// here until now, a pre-existing gap; harmless for every other voucher that never sends it).
		if (dto.getBranchId() != null) head.setBranchId(dto.getBranchId());
		if (dto.getChequeDate() != null) head.setChequeDate(dto.getChequeDate().atStartOfDay());

		VoucherHead savedHead = voucherHeadRepository.save(head);

		// If updating, delete existing details
		if (dto.getId() != null && dto.getId() > 0) {
			voucherDetailRepository.deleteByVoucherHead_Id(savedHead.getId());
		}

		// Real desktop double-entry mirror - ditto ExpenseVoucherNew.cs's Insert() "vd"/"vd2" pair
		// (lines ~2417-2483): opt-in via mirrorCreditToRefAccount so CPV/BPV/CRV/BRV/JV/Contra (never
		// send this flag) keep their already-verified, already-committed single-row-per-line Save
		// behavior unchanged - each of those needs its own dedicated debit/credit-direction
		// verification pass before being opted in (tracked as a separate, explicitly flagged gap).
		boolean mirrorCredit = Boolean.TRUE.equals(dto.getMirrorCreditToRefAccount())
				&& dto.getRefAccountId() != null && dto.getRefAccountId() > 0;
		ChartofAccount refAccountCoa = mirrorCredit
				? chartofAccountRepository.findById(dto.getRefAccountId()).orElse(null)
				: null;
		mirrorCredit = mirrorCredit && refAccountCoa != null;

		if (dto.getDetails() != null) {
			for (VoucherRequestDto.VoucherDetailRowDto dDto : dto.getDetails()) {
				if (dDto.getAccountId() == null || dDto.getAccountId() == 0) continue;
				ChartofAccount coa = chartofAccountRepository.findById(dDto.getAccountId()).orElse(null);
				if (coa == null) continue;

				double lineDebit = dDto.getDebitAmount() != null ? dDto.getDebitAmount() : 0.0;
				double lineCredit = dDto.getCreditAmount() != null ? dDto.getCreditAmount() : 0.0;

				VoucherDetail vd = new VoucherDetail();
				vd.setVoucherHead(savedHead);
				vd.setAccount(coa);
				vd.setComments(dDto.getComments());
				vd.setDebitAmount(lineDebit);
				vd.setCreditAmount(lineCredit);
				vd.setJobLotId(dDto.getJobLotId());
				vd.setRefInvoiceNo(dDto.getRefInvoiceNo());
				vd.setSupplierCustomerId(dDto.getSupplierCustomerId());
				vd.setCostCenterId(dDto.getCostCenterId());
				// CheqNoDetail/DCheqDate were declared on VoucherDetailRowDto (chequeNoDetail/
				// chequeDateDetail) but never actually wired here - a pre-existing gap, fixed now
				// since Party Receipt/Payment's real desktop grid ("ChequeNo" column, see
				// frmPartyReceiptVoucher.cs's Insert(): detail.CheqNoDetail = r.Cells["ChequeNo"])
				// needs it. Harmless no-op for every voucher that never sends these fields.
				if (dDto.getChequeNoDetail() != null) vd.setCheqNoDetail(dDto.getChequeNoDetail());
				if (dDto.getChequeDateDetail() != null) vd.setDCheqDate(dDto.getChequeDateDetail());
				if (dto.getLocationTypeId() != null) vd.setLocationTypeId(dto.getLocationTypeId());
				// PDC Payment (DocumentTypeId=24) grid columns - ditto PostDatedCheqPaymentVouchers.cs's
				// Insert(): vd.PaymentType (PayTitle) / vd.CommentsOtherLingo (BankName). Harmless no-op
				// for every other voucher that never sends these fields.
				if (dDto.getPaymentType() != null) vd.setPaymentType(dDto.getPaymentType());
				if (dDto.getCommentsOtherLingo() != null) vd.setCommentsOtherLingo(dDto.getCommentsOtherLingo());
				// CPV/BPV (DocumentTypeId=1/2) per-line fields - ditto PaymentVoucherNew.cs's Insert()
				// "vd"/"vd2" pair, both of which set every one of these on every grid row. Harmless
				// no-op for every other voucher that never sends them.
				if (dDto.getPaymentTypeId() != null) vd.setPaymentTypeId(dDto.getPaymentTypeId());
				if (dDto.getInstrumentTypeId() != null) vd.setInstrumentTypeId(dDto.getInstrumentTypeId());
				if (dDto.getChequeTypeId() != null) vd.setChequeTypeId(dDto.getChequeTypeId());
				if (dDto.getPayeeTitle() != null) vd.setPayeeTitle(dDto.getPayeeTitle());
				if (dDto.getReferenceAccountId() != null) vd.setReferenceAccountId(dDto.getReferenceAccountId());
				// Payment By Invoice (DocumentTypeId=1/2 via this alternate entry screen) per-line
				// fields - ditto PaymentByInvoiceVoucherNew.cs's Insert(): vd.OrderNo/DocumentTypeIdRef/
				// QtyIn/QtyOut/ItemAmount/WhtHolding. Additive/harmless for every other voucher.
				if (dDto.getOrderNo() != null) vd.setOrderNo(dDto.getOrderNo());
				if (dDto.getDocumentTypeIdRef() != null) vd.setDocumentTypeIdRef(dDto.getDocumentTypeIdRef());
				if (dDto.getQtyIn() != null) vd.setQtyIn(dDto.getQtyIn());
				if (dDto.getQtyOut() != null) vd.setQtyOut(dDto.getQtyOut());
				if (dDto.getItemAmount() != null) vd.setItemAmount(dDto.getItemAmount());
				if (dDto.getWhtHolding() != null) vd.setWhtHolding(dDto.getWhtHolding());
				if (mirrorCredit) {
					vd.setAgainstAccountId(dto.getRefAccountId());
				} else if (dDto.getLineAgainstAccountId() != null) {
					// Payment By Invoice - ditto Insert(): vd.AgainstAccountId = combcreditac.Value (the
					// CPV/BPV cash/bank account being paid FROM), stamped on every grid line WITHOUT a
					// mirror row - single-entry style like PDC Payment below, but kept as its own
					// per-line field since this voucher ALSO uses the header-level dto.againstAccountId
					// for an unrelated purpose (its own WHT Credit Account).
					vd.setAgainstAccountId(dDto.getLineAgainstAccountId());
				} else if (dto.getAgainstAccountId() != null) {
					// PDC Payment (DocumentTypeId=24) - ditto PostDatedCheqPaymentVouchers.cs's Insert():
					// detail.AgainstAccountId = vh.DebitAccountId stamped on every grid line, WITHOUT the
					// double-entry mirror row (each PDC line already carries its own distinct
					// CreditAccount - see AccountId above - so no second row is created here).
					vd.setAgainstAccountId(dto.getAgainstAccountId());
				}
				vd.setEntryDate(LocalDateTime.now());
				voucherDetailRepository.save(vd);

				if (mirrorCredit) {
					VoucherDetail mirror = new VoucherDetail();
					mirror.setVoucherHead(savedHead);
					mirror.setAccount(refAccountCoa);
					mirror.setAgainstAccountId(dDto.getAccountId());
					mirror.setComments(dDto.getComments());
					mirror.setDebitAmount(lineCredit);
					mirror.setCreditAmount(lineDebit);
					mirror.setJobLotId(dDto.getJobLotId());
					mirror.setCostCenterId(dDto.getCostCenterId());
					if (dDto.getChequeNoDetail() != null) mirror.setCheqNoDetail(dDto.getChequeNoDetail());
					if (dDto.getChequeDateDetail() != null) mirror.setDCheqDate(dDto.getChequeDateDetail());
					if (dto.getLocationTypeId() != null) mirror.setLocationTypeId(dto.getLocationTypeId());
					// CPV/BPV - ditto PaymentVoucherNew.cs's Insert() "vd2": every one of these fields is
					// mirrored onto the credit row too, not just the primary debit row.
					if (dDto.getPaymentType() != null) mirror.setPaymentType(dDto.getPaymentType());
					if (dDto.getPaymentTypeId() != null) mirror.setPaymentTypeId(dDto.getPaymentTypeId());
					if (dDto.getInstrumentTypeId() != null) mirror.setInstrumentTypeId(dDto.getInstrumentTypeId());
					if (dDto.getChequeTypeId() != null) mirror.setChequeTypeId(dDto.getChequeTypeId());
					if (dDto.getPayeeTitle() != null) mirror.setPayeeTitle(dDto.getPayeeTitle());
					if (dDto.getReferenceAccountId() != null) mirror.setReferenceAccountId(dDto.getReferenceAccountId());
					mirror.setEntryDate(LocalDateTime.now());
					voucherDetailRepository.save(mirror);
				}
			}
		}

		// CPV/BPV (DocumentTypeId=1/2) WHT (Withholding Tax) GL posting - ditto PaymentVoucherNew.cs's
		// Insert() "vd3"/"vd4" pair, created ONCE per voucher (not per detail line) when
		// ChkBoxWthHolding.Checked: vd3 debits RefDocNoId (WHT Debit Ac) / credits AgainstAccountId
		// (WHT Credit Account) against the SAME tax amount reversed in vd4, i.e. a real, separate
		// double-entry pair for the withheld tax on top of the main grid lines above. The client
		// already computed taxAmount/taxPercent with the real Total() formula (Excluded Tax: gross-up
		// (Base+SRB+Disc)/(100-rate)*rate; Included Tax: gross-down Base*(100-rate)/100) - this just
		// persists the two rows desktop itself creates, never recomputes the tax math.
		if (Boolean.TRUE.equals(dto.getIncludeWht()) && dto.getTaxAmount() != null && dto.getTaxAmount() > 0.0
				&& dto.getRefDocNoId() != null && dto.getRefDocNoId() > 0
				&& dto.getAgainstAccountId() != null && dto.getAgainstAccountId() > 0) {
			ChartofAccount whtDebitCoa = chartofAccountRepository.findById(dto.getRefDocNoId()).orElse(null);
			ChartofAccount whtCreditCoa = chartofAccountRepository.findById(dto.getAgainstAccountId()).orElse(null);
			if (whtDebitCoa != null && whtCreditCoa != null) {
				double taxPercent = dto.getTaxPercent() != null ? dto.getTaxPercent() : 0.0;
				double taxAmount = dto.getTaxAmount();
				String taxComments = "Withholding Tax deducted under "
						+ (dto.getTaxTypeName() != null ? dto.getTaxTypeName() : "") + " at " + taxPercent + "% rate.";

				VoucherDetail vd3 = new VoucherDetail();
				vd3.setVoucherHead(savedHead);
				vd3.setAccount(whtDebitCoa);
				vd3.setAgainstAccountId(dto.getAgainstAccountId());
				vd3.setTaxTypeId(dto.getTaxTypeId());
				vd3.setIsTaxable("True");
				vd3.setComments(taxComments);
				vd3.setTaxPrcnt(taxPercent);
				vd3.setTaxesTotalAmount(taxAmount);
				vd3.setWhtHolding(taxAmount);
				vd3.setDebitAmount(taxAmount);
				vd3.setEntryDate(LocalDateTime.now());
				voucherDetailRepository.save(vd3);

				// Payment By Invoice (DocumentTypeId=1/2 via this alternate entry screen) sets
				// postSingleWhtDebitRow=true - ditto PaymentByInvoiceVoucherNew.cs's own Insert(),
				// whose "vd2" WHT row sets DebitAmount only, with NO matching credit-side row created
				// anywhere in that method (a real, asymmetric one-row posting, verified from source -
				// not a guess). CPV/BPV never send this flag, so their own real two-row vd3/vd4 pair
				// (verified separately from PaymentVoucherNew.cs) is unchanged.
				if (!Boolean.TRUE.equals(dto.getPostSingleWhtDebitRow())) {
					VoucherDetail vd4 = new VoucherDetail();
					vd4.setVoucherHead(savedHead);
					vd4.setAccount(whtCreditCoa);
					vd4.setAgainstAccountId(dto.getRefDocNoId());
					vd4.setTaxTypeId(dto.getTaxTypeId());
					vd4.setIsTaxable("True");
					vd4.setComments(taxComments);
					vd4.setTaxPrcnt(taxPercent);
					vd4.setTaxesTotalAmount(taxAmount);
					vd4.setWhtHolding(taxAmount);
					vd4.setCreditAmount(taxAmount);
					vd4.setEntryDate(LocalDateTime.now());
					voucherDetailRepository.save(vd4);
				}
			}
		}

		response.put("success", true);
		response.put("voucherHeadId", savedHead.getId());
		response.put("voucherCode", savedHead.getVoucherCode());
		response.put("message", "Voucher saved successfully!");
		return response;
	}

	@Override
	@Transactional
	public void deleteVoucher(int voucherHeadId) {
		voucherDetailRepository.deleteByVoucherHead_Id(voucherHeadId);
		voucherHeadRepository.deleteById(voucherHeadId);
	}

	@Override
	public List<Map<String, Object>> searchVouchers(int documentTypeId, String fromDate, String toDate, String query) {
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		try {
			String sql = "EXEC [dbo].[Sp_Vouchers_GetMethods] @Activity='Search', @DocumentTypeId=?, @OrganizationId=?, @CompanyId=?, @FromDate=?, @ToDate=?, @Query=?";
			return jdbcTemplate.queryForList(sql, documentTypeId, orgId, compId, fromDate, toDate, query);
		} catch (Exception ex) {
			return Collections.emptyList();
		}
	}

	@Override
	public void approveVouchers(List<Integer> voucherIds, boolean approve) {
		if (voucherIds == null || voucherIds.isEmpty()) return;
		int userId = currentUserContext.currentUserId();
		for (Integer vId : voucherIds) {
			try {
				String sql = "EXEC [dbo].[Sp_Accounts_ApprovedVoucherDashboardForUnPost] @VoucherHeadId=?, @IsApproved=?, @UserId=?";
				jdbcTemplate.update(sql, vId, approve, userId);
			} catch (Exception ex) {
				try {
					String directSql = "UPDATE VoucherHead SET IsApproved = ?, UnApproveUserId = ? WHERE Id = ?";
					jdbcTemplate.update(directSql, approve, userId, vId);
				} catch (Exception ignored) {}
			}
		}
	}

	@Override
	public List<Map<String, Object>> getDayBookApprovalVouchers() {
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		try {
			String sql = "EXEC [dbo].[Sp_Voucher_ReadDashboard] @OrganizationId=?, @CompanyId=?";
			return jdbcTemplate.queryForList(sql, orgId, compId);
		} catch (Exception ex) {
			return Collections.emptyList();
		}
	}

	// ==========================================================================================
	// CPV History / BPV History - real ditto-copy of Architecture.WinApp.Account_Definition.
	// VouchersWithTax.PaymentVoucherNew.cs's HistoryFillCpv()/HistoryFillBpv(). Both desktop methods
	// call the SAME shared BLL method (Architecture.BLL.Accounts.VoucherHead.VoucherFormHistory ->
	// EXEC USP_VoucherFormHistory, verified directly against the decoded stored procedure body) -
	// desktop itself shares ONE procedure across CPV/BPV/CRV/BRV/JV/Contra/Expense/Party Receipt/
	// Party Payment/PDC Payment history, differentiated only by @DocumentTypeName ("1" for CPV, "2"
	// for BPV, ...). Each voucher's own History tab still builds its OWN separate grid/column set from
	// the shared raw rows (CPV's grid has no ChequeNo column; BPV's does, per HistoryGridSettings()/
	// BankGridSettings()) - ditto-copied below as two distinct methods rather than one generic
	// endpoint/response shape reused across vouchers.
	// ==========================================================================================

	private static final String SQL_LEDGER_ACCOUNT_HEADER_DROPDOWN =
			"EXEC Sp_Vouchers_LedgerByJobLot_DropDownAndLists @OrganizationId=?, @CompanyId=?, @AppId=?, @UserId=?, @DocumentTypeIds=?, @ActivityType=?";

	/** Real "Account Title" History-filter dropdown for CPV/BPV, ditto ComboBindForCpvHistory()/
	 *  ComboBindForBpvHistory() -> Architecture.BLL.Reports.Accounts.VoucherReports.
	 *  LedgerByJobLotDropDownAndLists() -> Sp_Vouchers_LedgerByJobLot_DropDownAndLists (verified
	 *  against the decoded proc): only accounts that have actually been used as a VoucherHead.
	 *  RefAccountId on a real voucher of this exact DocumentTypeId are returned (@ActivityType=
	 *  'Account Header'), not every Chart of Account row - so the filter only ever offers accounts
	 *  the user could plausibly be looking for in this voucher's history. */
	private List<Map<String, Object>> getVoucherHistoryAccountFilterOptions(String documentTypeIds) {
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		int userId = currentUserContext.currentUserId();
		try {
			return jdbcTemplate.queryForList(SQL_LEDGER_ACCOUNT_HEADER_DROPDOWN,
					orgId, compId, 1, userId, documentTypeIds, "Account Header");
		} catch (Exception ex) {
			return Collections.emptyList();
		}
	}

	public List<Map<String, Object>> getCpvHistoryAccounts() {
		return getVoucherHistoryAccountFilterOptions("1");
	}

	public List<Map<String, Object>> getBpvHistoryAccounts() {
		return getVoucherHistoryAccountFilterOptions("2");
	}

	// CRV/BRV/Contra/Expense/Party Receipt/Party Payment Account-Title History filters - ditto
	// ComboBindForCrvHistory/BrvHistory/ComboBindForHistory(Contra/Expense)/ComboBindForHistory(Party
	// Receipt & Payment), each passing its own single DocumentTypeIds value (3/4/10/26/34/35) into the
	// same real Sp_Vouchers_LedgerByJobLot_DropDownAndLists proc used above. JV and PDC Payment have NO
	// such dropdown in desktop (JournalVoucher.cs's HistoryGridFill never sets vh.AccountId from a combo;
	// PostDatedCheqPaymentVouchers.cs's gridhistoryfill() has no filter UI at all) so no accounts endpoint
	// is added for those two.
	public List<Map<String, Object>> getCrvHistoryAccounts() {
		return getVoucherHistoryAccountFilterOptions("3");
	}

	public List<Map<String, Object>> getBrvHistoryAccounts() {
		return getVoucherHistoryAccountFilterOptions("4");
	}

	public List<Map<String, Object>> getContraHistoryAccounts() {
		return getVoucherHistoryAccountFilterOptions("10");
	}

	public List<Map<String, Object>> getExpenseHistoryAccounts() {
		return getVoucherHistoryAccountFilterOptions("26");
	}

	/** Expense Voucher's "Project" combo (captioned "Cost Center" in the desktop's own
	 *  DDL.BindDDLNew(dt, CmbProjectId, "Id", "ProjectName", "Cost Center", false) call) - ditto
	 *  CommonServices.ProjectServiceBind() -&gt; Architecture.BLL.Projects.GetAlldt() -&gt;
	 *  EXEC Sp_Projects_GetAllMethod @OrganizationId=?, @CompanyId=?, @MethodType='GetAll'
	 *  (verified directly against Architecture.BLL.Projects.cs). A required field per
	 *  ExpenseVoucherNew.cs's FormValidation(). */
	public List<Map<String, Object>> getProjects() {
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		try {
			return jdbcTemplate.queryForList(
					"EXEC Sp_Projects_GetAllMethod @OrganizationId=?, @CompanyId=?, @MethodType=?",
					orgId, compId, "GetAll");
		} catch (Exception ex) {
			return Collections.emptyList();
		}
	}

	/** Ditto Architecture.BLL.Accounts.VoucherHead.GetLocationType() -&gt;
	 *  EXEC usp_getLocationType (no parameters) - Expense Voucher's "Location Type" combo,
	 *  also a required FormValidation() field. */
	public List<Map<String, Object>> getLocationTypes() {
		try {
			return jdbcTemplate.queryForList("EXEC usp_getLocationType");
		} catch (Exception ex) {
			return Collections.emptyList();
		}
	}

	/** CPV/BPV (PaymentVoucherNew.cs) detail-grid "Payment Type" combo - ditto
	 *  Architecture.BLL.Account.lookUp.GetDataPaymentTypeForPayments() -&gt;
	 *  EXEC [Account].[USP_lookUp_GetAllMethod] @Activity='GetDataPaymentTypeForPayments'
	 *  (verified directly against 0622_Architecture.BLL.Account.lookUp.cs). Required per
	 *  FormValidationDetail(). */
	public List<Map<String, Object>> getPaymentTypes() {
		try {
			return jdbcTemplate.queryForList(
					"EXEC [Account].[USP_lookUp_GetAllMethod] @Activity=?", "GetDataPaymentTypeForPayments");
		} catch (Exception ex) {
			return Collections.emptyList();
		}
	}

	/** CPV/BPV (PaymentVoucherNew.cs) detail-grid "Financial Instrument" combo (BPV-only, required
	 *  when CmbVoucherType==2 per FormValidationDetail()) - ditto
	 *  Architecture.BLL.Accounts.VoucherHead.GetInstrumentTypes() -&gt; EXEC usp_getInstrumentType
	 *  (no parameters). */
	public List<Map<String, Object>> getFinancialInstrumentTypes() {
		try {
			return jdbcTemplate.queryForList("EXEC usp_getInstrumentType");
		} catch (Exception ex) {
			return Collections.emptyList();
		}
	}

	/** CPV/BPV (PaymentVoucherNew.cs) detail-grid "Cheque Type" combo (BPV-only, required when
	 *  Financial Instrument=Cheque per FormValidationDetail()) - ditto
	 *  Architecture.BLL.ChequePrinting.ChequeType.GetChequeType() -&gt; EXEC [dbo].[sp_ChequeType]
	 *  (no parameters - confirmed via PaymentVoucherNew.cs's own "using Architecture.BLL.
	 *  ChequePrinting;" import, disambiguating from the unrelated Architecture.BLL.BankSchema.
	 *  ChequeType class of the same name). */
	public List<Map<String, Object>> getChequeTypes() {
		try {
			return jdbcTemplate.queryForList("EXEC [dbo].[sp_ChequeType]");
		} catch (Exception ex) {
			return Collections.emptyList();
		}
	}

	/** CPV/BPV (PaymentVoucherNew.cs) WHT "Tax Type" combo - ditto
	 *  Architecture.BLL.Inventory.TaxesTypes.GetForComboBind(Type=1) -&gt;
	 *  EXEC Sp_TaxesTypes_GetAllMethod @OrganizationId=?, @CompanyId=?, @Type=1,
	 *  @Activity='ReadByCombo' (verified against 0604_Architecture.BLL.Inventory.TaxesTypes.cs). */
	public List<Map<String, Object>> getTaxTypes() {
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		try {
			return jdbcTemplate.queryForList(
					"EXEC Sp_TaxesTypes_GetAllMethod @OrganizationId=?, @CompanyId=?, @Type=?, @Activity=?",
					orgId, compId, 1, "ReadByCombo");
		} catch (Exception ex) {
			return Collections.emptyList();
		}
	}

	/** Payment By Invoice (PaymentByInvoiceVoucherNew.cs) "Company" combo - ditto
	 *  Architecture.WinApp.Common.CommonServices.CompanyServiceBind() -&gt; Architecture.BLL.
	 *  Company.GetAlldt() -&gt; EXEC Sp_Company_GetAllMethod @OrgCompanyTypeId=?,
	 *  @Activity='ReadByOrganizationId' (verified against 0062_Architecture.BLL.Company.cs). Real
	 *  desktop control lets the user pick among the org's companies; this port surfaces the list for
	 *  display only (every voucher's CompanyId is always the session's own current company - no
	 *  multi-company switch exists anywhere else in this Java app either, a documented scoping
	 *  decision, not a guess). */
	public List<Map<String, Object>> getCompanies() {
		int orgId = currentUserContext.currentOrganizationId();
		try {
			return jdbcTemplate.queryForList(
					"EXEC Sp_Company_GetAllMethod @OrgCompanyTypeId=?, @Activity=?", orgId, "ReadByOrganizationId");
		} catch (Exception ex) {
			return Collections.emptyList();
		}
	}

	/** Payment By Invoice (PaymentByInvoiceVoucherNew.cs) "Branch" combo - ditto
	 *  Architecture.WinApp.Common.CommonServices.BrancheServiceBind() -&gt; Architecture.BLL.
	 *  Branches.GetAll() -&gt; EXEC Sp_Branches_GetAllMethod @OrganizationId=?, @CompanyId=?,
	 *  @Activity='GetAll' (verified against 0058_Architecture.BLL.Branches.cs). Unlike every other
	 *  voucher (which only ever posts the session's own current BranchId), this form lets the user
	 *  pick a different branch per voucher - see VoucherRequestDto.branchId. */
	public List<Map<String, Object>> getBranches() {
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		try {
			return jdbcTemplate.queryForList(
					"EXEC Sp_Branches_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
					orgId, compId, "GetAll");
		} catch (Exception ex) {
			return Collections.emptyList();
		}
	}

	/** Payment By Invoice (PaymentByInvoiceVoucherNew.cs) "Invoice No" combo, populated per selected
	 *  Account Title - ditto Architecture.BLL.Accounts.VoucherHead.GetInvoiceNoByPaymentByInvoice()
	 *  -&gt; EXEC Sp_Vouchers_GetMethods @OrganizationId=?, @CompanyId=?, @FinancialYearId=?,
	 *  @RefAccountId=?, @Activity='GetInvoiceNoByPaymentByInvoice' (verified against
	 *  0654_Architecture.BLL.Accounts.VoucherHead.cs). */
	public List<Map<String, Object>> getInvoicesForPaymentByInvoice(int accountId) {
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		int finYearId = currentUserContext.currentFinancialYearId();
		try {
			return jdbcTemplate.queryForList(
					"EXEC Sp_Vouchers_GetMethods @OrganizationId=?, @CompanyId=?, @FinancialYearId=?, @RefAccountId=?, @Activity=?",
					orgId, compId, finYearId, accountId, "GetInvoiceNoByPaymentByInvoice");
		} catch (Exception ex) {
			return Collections.emptyList();
		}
	}

	/** Payment By Invoice (PaymentByInvoiceVoucherNew.cs) Invoice Amount/Total Paid/Balance Amount
	 *  auto-fill on Invoice No selection - ditto Architecture.BLL.Accounts.VoucherHead.
	 *  GetInvoiceWiseBalanceAmount() -&gt; EXEC Sp_Vouchers_GetMethods @OrganizationId=?,
	 *  @CompanyId=?, @FinancialYearId=?, @Id=?, @RefAccountId=?, @Activity='GetInvoiceWiseBalanceAmount'
	 *  (verified against the same file - note @Id is the InvoiceId, @RefAccountId the AccountId,
	 *  same real parameter order desktop uses). */
	public List<Map<String, Object>> getInvoiceBalanceForPaymentByInvoice(int accountId, int invoiceId) {
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		int finYearId = currentUserContext.currentFinancialYearId();
		try {
			return jdbcTemplate.queryForList(
					"EXEC Sp_Vouchers_GetMethods @OrganizationId=?, @CompanyId=?, @FinancialYearId=?, @Id=?, @RefAccountId=?, @Activity=?",
					orgId, compId, finYearId, invoiceId, accountId, "GetInvoiceWiseBalanceAmount");
		} catch (Exception ex) {
			return Collections.emptyList();
		}
	}

	/** Payment By Invoice (PaymentByInvoiceVoucherNew.cs) History tab - ditto
	 *  Architecture.BLL.Accounts.VoucherHead.PaymentByInvoiceVoucherNewHistory() -&gt;
	 *  EXEC Sp_Vouchers_GetMethods @Activity='PaymentByInvoiceVoucherNewHistory',
	 *  @OrganizationId=?, @CompanyId=?, @FinancialYearId=?, @DocumentTypeName='1,2'
	 *  (verified against 0654_Architecture.BLL.Accounts.VoucherHead.cs - desktop's own HistoryFill()
	 *  always passes DocumentTypeId=1, which that BLL method maps to the fixed string "1,2", so this
	 *  History tab shows every CPV+BPV voucher regardless of whether it was entered via this screen
	 *  or the regular Cash/Bank Payment Voucher forms - a real, intentional, evidence-based behavior,
	 *  not a bug. Desktop's own HistoryFill() has no filter-form UI beyond an optional VoucherCode
	 *  parameter it never actually exposes on screen, so this port also has no filter UI, ditto PDC
	 *  Payment's own no-filter History pattern). */
	public Map<String, Object> getPaymentByInvoiceHistory() {
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		int finYearId = currentUserContext.currentFinancialYearId();
		Map<String, Object> result = new HashMap<>();
		try {
			List<Map<String, Object>> rows = jdbcTemplate.queryForList(
					"EXEC Sp_Vouchers_GetMethods @Activity=?, @OrganizationId=?, @CompanyId=?, @FinancialYearId=?, @DocumentTypeName=?",
					"PaymentByInvoiceVoucherNewHistory", orgId, compId, finYearId, "1,2");
			result.put("rows", rows);
		} catch (Exception ex) {
			result.put("rows", Collections.emptyList());
		}
		return result;
	}

	public List<Map<String, Object>> getPartyReceiptHistoryAccounts() {
		return getVoucherHistoryAccountFilterOptions("34");
	}

	public List<Map<String, Object>> getPartyPaymentHistoryAccounts() {
		return getVoucherHistoryAccountFilterOptions("35");
	}

	/** Real EXEC USP_VoucherFormHistory call, ditto Architecture.BLL.Accounts.VoucherHead.
	 *  VoucherFormHistory(ReportsParameters) - only the parameters desktop actually conditionally
	 *  adds are included, exactly mirroring the C# method's own "if (...) list.Add(...)" branches
	 *  (an omitted date/filter parameter lets the real proc's own "@Param IS NULL OR ..." clauses
	 *  no-op it, same as desktop never setting that SqlParameter at all). dateType selects which one
	 *  of the proc's four independent date-range pairs (Doc/Entry/Modify/Approved date) the
	 *  from/toDate values apply to, ditto the desktop radio buttons (drdocdate/rdentrydate/
	 *  rdmodifydate/rdapproveddate for CPV, rdbpvdocdate/... for BPV). approvedStatus mirrors the
	 *  cmbApprovedStatusCpv/cmbApproveStatusBpv combo's three real options: "notapproved" (desktop's
	 *  own default - see StatusFillForBpv()'s Rows[0].Activate() selecting it first) passes
	 *  @IsApproved=0, "approved" passes @IsApproved=1, "all" omits @IsApproved entirely so every
	 *  voucher (approved or not) is returned, ditto the ApprovedFilter=="All" branch. */
	private List<Map<String, Object>> callVoucherFormHistory(String documentTypeName, String dateType,
			LocalDate fromDate, LocalDate toDate, Integer fromDocNo, Integer toDocNo, Integer accountId,
			String approvedStatus) {
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		int userId = currentUserContext.currentUserId();
		int finYearId = currentUserContext.currentFinancialYearId();

		// CanViewAllRecord: desktop reads the logged-in user's own "Can View All Record" screen right
		// (formright.DoHaveCanViewAllRecordRights) and, when false, restricts history to that user's
		// own EntryUser id. This dev deployment has no per-user login/rights table wired yet
		// (SecurityConfiguration permits every request - see CurrentUserContext's fallback IDs), so it
		// defaults to true (view every user's vouchers) rather than silently hiding real data behind an
		// unimplemented permission check. Revisit once per-user rights are wired up end to end.
		boolean canViewAllRecord = true;

		StringBuilder sql = new StringBuilder(
				"EXEC USP_VoucherFormHistory @OrganizationId=?, @CompanyId=?, @UserId=?, @DocumentTypeName=?, @CanViewAllRecord=?");
		List<Object> params = new ArrayList<>();
		params.add(orgId);
		params.add(compId);
		params.add(userId);
		params.add(documentTypeName);
		params.add(canViewAllRecord);

		if (finYearId != 0) {
			sql.append(", @FinancialYearId=?");
			params.add(finYearId);
		}
		if ("entrydate".equalsIgnoreCase(dateType)) {
			if (fromDate != null) { sql.append(", @EntryFromDate=?"); params.add(fromDate); }
			if (toDate != null) { sql.append(", @EntryToDate=?"); params.add(toDate); }
		} else if ("modifydate".equalsIgnoreCase(dateType)) {
			if (fromDate != null) { sql.append(", @ModifyFromDate=?"); params.add(fromDate); }
			if (toDate != null) { sql.append(", @ModifyToDate=?"); params.add(toDate); }
		} else if ("approveddate".equalsIgnoreCase(dateType)) {
			if (fromDate != null) { sql.append(", @ApprovedFromDate=?"); params.add(fromDate); }
			if (toDate != null) { sql.append(", @ApprovedToDate=?"); params.add(toDate); }
		} else {
			// default (and explicit "docdate"): ditto desktop's drdocdate/rdbpvdocdate radio default
			if (fromDate != null) { sql.append(", @FromDate=?"); params.add(fromDate); }
			if (toDate != null) { sql.append(", @ToDate=?"); params.add(toDate); }
		}
		if (fromDocNo != null && fromDocNo != 0) {
			sql.append(", @DocNoFrom=?");
			params.add(fromDocNo);
		}
		if (toDocNo != null && toDocNo != 0) {
			sql.append(", @DocNoTo=?");
			params.add(toDocNo);
		}
		if (accountId != null && accountId != 0) {
			sql.append(", @AccountId=?");
			params.add(accountId);
		}
		if (!canViewAllRecord) {
			sql.append(", @EntryUser=?");
			params.add(userId);
		}
		if (!"all".equalsIgnoreCase(approvedStatus)) {
			boolean isApproved = "approved".equalsIgnoreCase(approvedStatus);
			sql.append(", @IsApproved=?");
			params.add(isApproved);
		}
		sql.append(", @AppId=?");
		params.add(1); // Desktop_General - AppId=5 (Booking Office) has an extra CostCenter restriction this org doesn't use

		return jdbcTemplate.queryForList(sql.toString(), params.toArray());
	}

	/** Which optional columns a given voucher's own desktop HistoryFillXxx() method adds to its
	 *  DataTable, on top of the base set every one of them shares (Id/DocumentTypeId/VoucherDate/
	 *  VoucherCode/DocumentType(Code)/AccountTitle/VoucherAmount/Remarks/EntryUser/Attachment/
	 *  IsApproved - all present in every USP_VoucherFormHistory row regardless of voucher type).
	 *  Each voucher's own flag combination below was read directly out of that voucher's own
	 *  HistoryFillXxx()/HistoryGridFill() method's "dtHistory.Columns.Add(...)" list - never guessed -
	 *  so this is still each voucher building its own real grid, just without retyping the identical
	 *  base-column mapping 10 times over. */
	private static final class HistoryColumns {
		final boolean chequeNo;
		final boolean manualBillNo;
		final boolean againstAccount;
		final boolean accountTypeId;
		final boolean fcy; // FcyCode / ExchangeRate / FcyAmount
		final boolean entryDate;
		final boolean modifyApproved; // ModifyUser / ModifyDate / ApprovedUser / ApprovedDate

		private HistoryColumns(boolean chequeNo, boolean manualBillNo, boolean againstAccount,
				boolean accountTypeId, boolean fcy, boolean entryDate, boolean modifyApproved) {
			this.chequeNo = chequeNo;
			this.manualBillNo = manualBillNo;
			this.againstAccount = againstAccount;
			this.accountTypeId = accountTypeId;
			this.fcy = fcy;
			this.entryDate = entryDate;
			this.modifyApproved = modifyApproved;
		}

		// CPV HistoryFillCpv() / CRV HistoryFillCrv(): Id,DocumentTypeId,VoucherDate,VoucherCode,
		// DocumentType,AccountTitle,VoucherAmount,FcyCode,ExchangeRate,FcyAmount,Remarks,EntryUser,
		// EntryDate,ModifyUser,ModifyDate,ApprovedUser,ApprovedDate,Attachment - no ChequeNo.
		static final HistoryColumns CPV_CRV = new HistoryColumns(false, false, false, false, true, true, true);

		// BPV HistoryFillBpv() / BRV HistoryFillBrv(): same as above plus ChequeNo.
		static final HistoryColumns BPV_BRV = new HistoryColumns(true, false, false, false, true, true, true);

		// JV HistoryFill(): Id,VoucherCode,DocumentTypeId,DocumentTypeCode,VoucherDate,ManualBillNo,
		// AccountTitle,AgainstAccount,Remarks,VoucherAmount,FcyCode,ExchangeRate,FcyAmount,EntryUser,
		// EntryDate,ModifyUser,ModifyDate,ApprovedUser,ApprovedDate,CheqNo,Attachment - no AccountTypeId.
		static final HistoryColumns JV = new HistoryColumns(true, true, true, false, true, true, true);

		// Contra HistoryFill() / Expense HistoryFill(): same as JV plus AccountTypeId.
		static final HistoryColumns CONTRA_EXPENSE = new HistoryColumns(true, true, true, true, true, true, true);

        // Party Receipt / Party Payment HistoryGridFill(): Id,DocumentTypeId,VoucherDate,VoucherCode,
        // DocumentType,AccountTitle,VoucherAmount,Remarks,EntryUser,Attachment only - no Fcy trio, no
        // EntryDate, no Modify/Approved columns, no ChequeNo/ManualBillNo/AgainstAccount.
		static final HistoryColumns PARTY_RECEIPT_PAYMENT = new HistoryColumns(false, false, false, false, false, false, false);

		// PDC Payment CommonServices.VoucherFormHistory(): Id,VoucherCode,DocumentTypeId,
		// DocumentTypeCode,VoucherDate,ManualBillNo,AccountTitle,AgainstAccount,Remarks,VoucherAmount,
		// FcyCode,ExchangeRate,FcyAmount,UserName,CheqNo,Attachment - no AccountTypeId, no EntryDate, no
		// Modify/Approved columns (desktop's own wrapper never selects them for this voucher).
		static final HistoryColumns PDC_PAYMENT = new HistoryColumns(true, true, true, false, true, false, false);
	}

	private Map<String, Object> buildVoucherHistoryResponse(List<Map<String, Object>> rawRows, HistoryColumns cols) {
		Map<String, Object> response = new HashMap<>();
		List<Map<String, Object>> rows = new ArrayList<>();
		int totalVouchers = 0, totalApproved = 0, totalUnApproved = 0;
		if (rawRows != null && !rawRows.isEmpty()) {
			Map<String, Object> first = rawRows.get(0);
			totalVouchers = toIntSafe(first.get("TotalVouchers"));
			totalApproved = toIntSafe(first.get("TotalApprovedVoucher"));
			totalUnApproved = toIntSafe(first.get("TotalUnApprovedVoucher"));
			for (Map<String, Object> r : rawRows) {
				Map<String, Object> row = new HashMap<>();
				row.put("id", r.get("Id"));
				row.put("documentTypeId", r.get("DocumentTypeId"));
				row.put("voucherDate", r.get("VoucherDate"));
				row.put("voucherCode", r.get("VoucherCode"));
				row.put("documentType", r.get("DocumentTypeCode"));
				if (cols.manualBillNo) {
					row.put("manualBillNo", r.get("ManualBillNo"));
				}
				if (cols.accountTypeId) {
					row.put("accountTypeId", r.get("AccountTypeId"));
				}
				row.put("accountTitle", r.get("AccountTitle"));
				if (cols.againstAccount) {
					row.put("againstAccount", r.get("AgainstAccount"));
				}
				if (cols.chequeNo) {
					row.put("chequeNo", r.get("ChequeNo"));
				}
				row.put("voucherAmount", r.get("VoucherAmount"));
				if (cols.fcy) {
					row.put("fcyCode", r.get("CurrencyCode"));
					row.put("exchangeRate", r.get("ExchangeCurrencyRate"));
					row.put("fcyAmount", r.get("FcAmount"));
				}
				row.put("remarks", r.get("Remarks"));
				row.put("entryUser", r.get("UserName"));
				if (cols.entryDate) {
					row.put("entryDate", r.get("EntryDate"));
				}
				if (cols.modifyApproved) {
					row.put("modifyUser", r.get("ModifyUserName"));
					row.put("modifyDate", r.get("ModifyDate"));
					row.put("approvedUser", r.get("ApprovedUserName"));
					row.put("approvedDate", r.get("PostDate"));
				}
				row.put("attachment", r.get("NoOfAttachments"));
				row.put("isApproved", r.get("IsApproved"));
				rows.add(row);
			}
		}
		response.put("rows", rows);
		response.put("totalVouchers", totalVouchers);
		response.put("totalApprovedVoucher", totalApproved);
		response.put("totalUnApprovedVoucher", totalUnApproved);
		return response;
	}

	private static int toIntSafe(Object val) {
		if (val == null) return 0;
		try {
			return Integer.parseInt(val.toString());
		} catch (NumberFormatException e) {
			return 0;
		}
	}

	@Override
	public Map<String, Object> getCpvHistory(String dateType, LocalDate fromDate, LocalDate toDate,
			Integer fromDocNo, Integer toDocNo, Integer accountId, String approvedStatus) {
		List<Map<String, Object>> raw = callVoucherFormHistory("1", dateType, fromDate, toDate, fromDocNo, toDocNo, accountId, approvedStatus);
		return buildVoucherHistoryResponse(raw, HistoryColumns.CPV_CRV);
	}

	@Override
	public Map<String, Object> getBpvHistory(String dateType, LocalDate fromDate, LocalDate toDate,
			Integer fromDocNo, Integer toDocNo, Integer accountId, String approvedStatus) {
		List<Map<String, Object>> raw = callVoucherFormHistory("2", dateType, fromDate, toDate, fromDocNo, toDocNo, accountId, approvedStatus);
		return buildVoucherHistoryResponse(raw, HistoryColumns.BPV_BRV);
	}

	// ==========================================================================================
	// CRV/BRV/JV/Contra/Expense/Party Receipt/Party Payment/PDC Payment History - same real
	// EXEC USP_VoucherFormHistory ditto-copy as CPV/BPV above (desktop's own VoucherHead.
	// VoucherFormHistory() BLL call is genuinely shared across all of these forms - see
	// HistoryFillCrv/HistoryFillBrv in ReceiptsVoucherNew.cs, HistoryGridFill in JournalVoucher.cs,
	// HistoryFill in ContraVoucher.cs/ExpenseVoucherNew.cs, HistoryGridFill in
	// frmPartyReceiptVoucher.cs/frmPartyPaymentVoucher.cs, and CommonServices.VoucherFormHistory()
	// (itself just a thin wrapper around the same BLL call) used by
	// PostDatedCheqPaymentVouchers.cs's gridhistoryfill()) - each voucher still gets its own
	// distinct DocumentTypeName/column-set/filter behavior below, never a single generic response.
	// ==========================================================================================

	@Override
	public Map<String, Object> getCrvHistory(String dateType, LocalDate fromDate, LocalDate toDate,
			Integer fromDocNo, Integer toDocNo, Integer accountId, String approvedStatus) {
		List<Map<String, Object>> raw = callVoucherFormHistory("3", dateType, fromDate, toDate, fromDocNo, toDocNo, accountId, approvedStatus);
		return buildVoucherHistoryResponse(raw, HistoryColumns.CPV_CRV);
	}

	@Override
	public Map<String, Object> getBrvHistory(String dateType, LocalDate fromDate, LocalDate toDate,
			Integer fromDocNo, Integer toDocNo, Integer accountId, String approvedStatus) {
		List<Map<String, Object>> raw = callVoucherFormHistory("4", dateType, fromDate, toDate, fromDocNo, toDocNo, accountId, approvedStatus);
		return buildVoucherHistoryResponse(raw, HistoryColumns.BPV_BRV);
	}

	// JV's own HistoryGridFill() never reads an Account-Title combo into vh.AccountId - so accountId
	// is intentionally never wired from the JV History UI, ditto desktop having no such filter here.
	@Override
	public Map<String, Object> getJvHistory(String dateType, LocalDate fromDate, LocalDate toDate,
			Integer fromDocNo, Integer toDocNo, String approvedStatus) {
		List<Map<String, Object>> raw = callVoucherFormHistory("5", dateType, fromDate, toDate, fromDocNo, toDocNo, null, approvedStatus);
		return buildVoucherHistoryResponse(raw, HistoryColumns.JV);
	}

	@Override
	public Map<String, Object> getContraHistory(String dateType, LocalDate fromDate, LocalDate toDate,
			Integer fromDocNo, Integer toDocNo, Integer accountId, String approvedStatus) {
		List<Map<String, Object>> raw = callVoucherFormHistory("10", dateType, fromDate, toDate, fromDocNo, toDocNo, accountId, approvedStatus);
		return buildVoucherHistoryResponse(raw, HistoryColumns.CONTRA_EXPENSE);
	}

	@Override
	public Map<String, Object> getExpenseHistory(String dateType, LocalDate fromDate, LocalDate toDate,
			Integer fromDocNo, Integer toDocNo, Integer accountId, String approvedStatus) {
		List<Map<String, Object>> raw = callVoucherFormHistory("26", dateType, fromDate, toDate, fromDocNo, toDocNo, accountId, approvedStatus);
		return buildVoucherHistoryResponse(raw, HistoryColumns.CONTRA_EXPENSE);
	}

	@Override
	public Map<String, Object> getPartyReceiptHistory(String dateType, LocalDate fromDate, LocalDate toDate,
			Integer fromDocNo, Integer toDocNo, Integer accountId, String approvedStatus) {
		List<Map<String, Object>> raw = callVoucherFormHistory("34", dateType, fromDate, toDate, fromDocNo, toDocNo, accountId, approvedStatus);
		return buildVoucherHistoryResponse(raw, HistoryColumns.PARTY_RECEIPT_PAYMENT);
	}

	@Override
	public Map<String, Object> getPartyPaymentHistory(String dateType, LocalDate fromDate, LocalDate toDate,
			Integer fromDocNo, Integer toDocNo, Integer accountId, String approvedStatus) {
		List<Map<String, Object>> raw = callVoucherFormHistory("35", dateType, fromDate, toDate, fromDocNo, toDocNo, accountId, approvedStatus);
		return buildVoucherHistoryResponse(raw, HistoryColumns.PARTY_RECEIPT_PAYMENT);
	}

	// PDC Payment's own gridhistoryfill() has NO date/doc-no/account/approved-status filter UI at
	// all - it just calls CommonServices.VoucherFormHistory(canViewAll, "24", 0), which itself passes
	// only OrganizationId/CompanyId/UserId/FinancialYearId/Ids/PostState(/EntryUser when
	// !canViewAllRecord) into USP_VoucherFormHistory, i.e. every filter parameter below is
	// deliberately left null/"all" to reproduce that exact no-filter, show-everything-visible
	// behaviour - this is NOT an oversight, it is ditto-copying desktop's own simpler History tab.
	@Override
	public Map<String, Object> getPdcPaymentHistory() {
		List<Map<String, Object>> raw = callVoucherFormHistory("24", null, null, null, null, null, null, "all");
		return buildVoucherHistoryResponse(raw, HistoryColumns.PDC_PAYMENT);
	}
}
