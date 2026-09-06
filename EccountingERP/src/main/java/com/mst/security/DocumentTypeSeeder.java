package com.mst.security;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.CommandLineRunner;
import org.springframework.core.annotation.Order;
import org.springframework.stereotype.Component;

import com.mst.models.DocumentType;
import com.mst.repositories.IDocumentTypeRepository;

/**
 * Seeds the handful of real dbo.DocumentType rows this port's Accounts Transaction
 * vouchers need (see VoucherController) - exact Id/Code/Description/ScreenName read
 * straight from goldenAce5_25t.sql's own DocumentType INSERTs, not invented. The
 * real table has 393 rows across the whole ERP (every Purchase/Sale/Production
 * voucher too); only the core accounting ones are seeded here, guarded per-row
 * (existsById) so this never touches/duplicates the real GoldenAcedb DocumentType
 * table if these rows are ever present there for real.
 */
@Component
@Order(2)
public class DocumentTypeSeeder implements CommandLineRunner {

	@Autowired
	private IDocumentTypeRepository documentTypeRepository;

	@Override
	public void run(String... args) {
		seed(1, "CPV", "CASH PAYMENT VOUCHER", "frmCashPaymentVoucher");
		seed(2, "BPV", "BANK PAYMENT VOUCHER", "frmBankPaymentVoucher");
		seed(3, "CRV", "CASH RECEIPTS VOUCHER", "frmCashReceiptVoucher");
		seed(4, "BRV", "BANK RECEIPTS VOUCHER", "frmBankReceiptVoucher");
		seed(5, "JV", "JOURNAL VOUCHER", "VoucherEntry");
		seed(10, "CONTRA", "CONTRA VOUCHER", "ContraVoucher");
		seed(26, "EV", "EXPENSE VOUCHER", "ExpenseVoucher");
		seed(34, "PRV", "Party Receipt Voucher", "frmPartyReceiptVoucher");
		seed(35, "PPV", "Party Payment Voucher", "frmPartyPaymentVoucher");
	}

	private void seed(int id, String code, String description, String screenName) {
		if (documentTypeRepository.existsById(id)) {
			return;
		}
		DocumentType type = new DocumentType();
		type.setId(id);
		type.setDocumentTypeCode(code);
		type.setDocumentTypeDescription(description);
		type.setScreenName(screenName);
		type.setStatus(true);
		documentTypeRepository.save(type);
	}
}
