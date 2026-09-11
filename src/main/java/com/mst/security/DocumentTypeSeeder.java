package com.mst.security;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.CommandLineRunner;
import org.springframework.core.annotation.Order;
import org.springframework.stereotype.Component;

import com.mst.models.DocumentType;
import com.mst.repositories.IDocumentTypeRepository;
import lombok.extern.slf4j.Slf4j;

@Slf4j
// Not registered: legacy ERP reference data must not be seeded at startup.
@Order(2)
public class DocumentTypeSeeder implements CommandLineRunner {

	@Autowired
	private IDocumentTypeRepository documentTypeRepository;

	@Override
	public void run(String... args) {
		try {
			seed(1, "CPV", "CASH PAYMENT VOUCHER", "frmCashPaymentVoucher");
			seed(2, "BPV", "BANK PAYMENT VOUCHER", "frmBankPaymentVoucher");
			seed(3, "CRV", "CASH RECEIPTS VOUCHER", "frmCashReceiptVoucher");
			seed(4, "BRV", "BANK RECEIPTS VOUCHER", "frmBankReceiptVoucher");
			seed(5, "JV", "JOURNAL VOUCHER", "VoucherEntry");
			seed(10, "CONTRA", "CONTRA VOUCHER", "ContraVoucher");
			seed(26, "EV", "EXPENSE VOUCHER", "ExpenseVoucher");
			seed(34, "PRV", "Party Receipt Voucher", "frmPartyReceiptVoucher");
			seed(35, "PPV", "Party Payment Voucher", "frmPartyPaymentVoucher");
		} catch (Throwable t) {
			log.warn("DocumentTypeSeeder skipped: {}", t.getMessage());
		}
	}

	private void seed(int id, String code, String description, String screenName) {
		try {
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
		} catch (Throwable t) {
			log.warn("Failed to seed document type {}: {}", code, t.getMessage());
		}
	}
}
