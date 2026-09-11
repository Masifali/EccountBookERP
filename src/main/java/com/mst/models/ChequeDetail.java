package com.mst.models;

import java.math.BigDecimal;
import java.time.LocalDate;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import javax.persistence.Table;

import lombok.Data;

/**
 * "account" is the party/ledger account the cheque is for (e.g. the supplier being
 * paid) - a real {@link ChartofAccount} relation. "chequeInHandAccount" is the
 * bank/cheque-in-hand GL account's accountCode the cheque is drawn against, kept as
 * a plain String (like ChartofAccount.accountCode itself) rather than a relation,
 * matching how the desktop screen just needs it for filtering/display.
 *
 * Scope note: the desktop's "Cheque Book Registration" screen
 * (Account_Definition/AcfrmChequebookRegistration.cs) is actually a bigger feature on
 * top of this - registering a whole physical cheque book by bank account + serial
 * number range (txtserialfrom/txtserialto), which bulk-creates the individual cheque
 * rows. That bulk "register a book" step is NOT implemented here; this covers single
 * cheque records only (add/edit/delete one cheque at a time).
 */
@Entity
@Table(name = "cheque_detail")
@Data
public class ChequeDetail {

	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	private Long id;

	@ManyToOne(fetch = FetchType.LAZY)
	@JoinColumn(name = "account_id")
	private ChartofAccount account;

	@Column(nullable = false, length = 30)
	private String chequeInHandAccount;

	@Column(nullable = false, length = 50)
	private String chequeNo;

	@Column(nullable = false)
	private LocalDate chequeDate;

	private BigDecimal amount = BigDecimal.ZERO;

	@Column(length = 255)
	private String narration;

	@Column(nullable = false)
	private Boolean active = true;
}
