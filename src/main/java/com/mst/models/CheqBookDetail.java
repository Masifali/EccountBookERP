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
	@Column(name = "Id")
	private Long id;

	@Column(name = "CheqBookHeaderId")
	private Long cheqBookHeaderId;

	@Column(name = "CheqNo", length = 50)
	private String cheqNo;

	@Column(name = "CheqStatus", length = 50)
	private String cheqStatus = "Blank";

	@Column(name = "OtherRemarks", length = 255)
	private String otherRemarks;

	@Column(name = "CheqCancelStatus")
	private Boolean cheqCancelStatus = false;

	@Column(name = "ChequeStatusDate")
	private LocalDateTime chequeStatusDate;

	@Column(name = "StatusChangeUserId")
	private Integer statusChangeUserId;

	@Transient
	private String bankName;
}
