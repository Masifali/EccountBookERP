package com.mst.models;

import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.Table;
import javax.persistence.Transient;

import lombok.Data;

@Entity
@Table(name = "CheqBookDetail")
@Data
public class CheqBookDetail {

	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	private Long id;

	@Column(name = "cheq_book_header_id")
	private Long cheqBookHeaderId;

	@Column(name = "cheq_no", length = 50)
	private String cheqNo;

	@Column(name = "cheq_status", length = 50)
	private String cheqStatus = "Blank";

	@Column(name = "other_remarks", length = 255)
	private String otherRemarks;

	@Column(name = "cheq_cancel_status")
	private Boolean cheqCancelStatus = false;

	@Column(name = "cheque_status_date")
	private LocalDateTime chequeStatusDate;

	@Column(name = "status_change_user_id")
	private Integer statusChangeUserId;

	@Transient
	private String bankName;
}
