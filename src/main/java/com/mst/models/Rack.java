package com.mst.models;

import java.math.BigDecimal;
import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import javax.persistence.Table;

import lombok.Data;

/**
 * Ditto of the real GoldenAcedb dbo.invWarehouseRack table (desktop:
 * Inventory_Definition/frmRackDefine.cs). Id is not a SQL Server IDENTITY column
 * (unlike its parent InvWareHouse), so RackService assigns the next one itself. The
 * real table has no separate "rack code" - just rackName and a sortNo used to order
 * racks within a warehouse.
 */
@Entity
@Table(name = "invWarehouseRack")
@Data
public class Rack {

	@Id
	@Column(name = "Id")
	private Integer id;

	@ManyToOne(fetch = FetchType.LAZY)
	@JoinColumn(name = "invWarehouseId", nullable = false)
	private Warehouse warehouse;

	@Column(name = "rackName", nullable = false, length = 150)
	private String rackName;

	@Column(name = "sortNo", nullable = false)
	private Integer sortNo = 0;

	@Column(name = "EntryDate")
	private LocalDateTime entryDate;

	@Column(name = "EntryUserId")
	private Integer entryUserId;

	@Column(name = "ModifyDate")
	private LocalDateTime modifyDate;

	@Column(name = "ModifyUserId")
	private Integer modifyUserId;

	@Column(name = "isActive")
	private Boolean isActive = true;

	// ---- Added by batch C1_joblot_warehouse. ----
	/**
	 * Read-only mirror of the invWarehouseId FK column (the column itself is owned by the
	 * {@code warehouse} relation above), so JPQL can use {@code r.invWarehouseId} directly.
	 */
	@Column(name = "invWarehouseId", insertable = false, updatable = false)
	private Integer invWarehouseId;

	// The columns below are listed for dbo.invWarehouseRack in the batch registry but are NOT
	// referenced by any procedure in this batch (they look like InvGrnDetail columns that the
	// registry attributed to this table). Verify they exist on dbo.invWarehouseRack before use.

	@Column(name = "PurchaseOrderId")
	private Integer purchaseOrderId;

	@Column(name = "PurchaseOrderDetailId")
	private Integer purchaseOrderDetailId;

	@Column(name = "PurchaseOrder")
	private String purchaseOrder;

	@Column(name = "PurchaseOrderDate")
	private LocalDateTime purchaseOrderDate;

	@Column(name = "PurchaseOrderQty")
	private BigDecimal purchaseOrderQty;

	@Column(name = "InvPurchasedemondId")
	private Integer invPurchasedemondId;

	@Column(name = "PurchaseDemondDetailId")
	private Integer purchaseDemondDetailId;

	@Column(name = "PurchaseDemandNo")
	private Integer purchaseDemandNo;

	@Column(name = "PurchaseDemandDocDate")
	private LocalDateTime purchaseDemandDocDate;

	@Column(name = "PurchaseDemandQty")
	private BigDecimal purchaseDemandQty;

	@Column(name = "DeliveryChallanId")
	private Integer deliveryChallanId;

	@Column(name = "DeliveryChallanDetailId")
	private Integer deliveryChallanDetailId;

	@Column(name = "DeliveryChallanNo")
	private Integer deliveryChallanNo;

	@Column(name = "DeliveryChallanDate")
	private LocalDateTime deliveryChallanDate;

	@Column(name = "DeliveryChallanQty")
	private BigDecimal deliveryChallanQty;

	@Column(name = "PurchasePreBillId")
	private Integer purchasePreBillId;

	@Column(name = "PurchasePreBillDetailId")
	private Integer purchasePreBillDetailId;

	@Column(name = "PurchasePreBillNo")
	private Integer purchasePreBillNo;

	@Column(name = "PurchasePreBillQty")
	private BigDecimal purchasePreBillQty;

	@Column(name = "UsedDemandQtyInPreBill")
	private BigDecimal usedDemandQtyInPreBill;

	@Column(name = "UsedDemandQtyInGrn")
	private BigDecimal usedDemandQtyInGrn;

	@Column(name = "UsedOrderQtyInPreBill")
	private BigDecimal usedOrderQtyInPreBill;

	@Column(name = "UsedOrderQtyInGrn")
	private BigDecimal usedOrderQtyInGrn;

	@Column(name = "UsedDeliveryChallanQtyGrn")
	private BigDecimal usedDeliveryChallanQtyGrn;

	@Column(name = "InvRevExpItemId")
	private Integer invRevExpItemId;

	@Column(name = "Qty")
	private BigDecimal qty;

	@Column(name = "Rate")
	private BigDecimal rate;

	@Column(name = "Amount")
	private BigDecimal amount;

	@Column(name = "Remarks")
	private String remarks;

	@Column(name = "VoucherHeadId")
	private Integer voucherHeadId;

	@Column(name = "DocumentTypeId")
	private Integer documentTypeId;

	@Column(name = "NoOfAttachments")
	private Integer noOfAttachments;

	@Column(name = "LabAnalisysNo")
	private Integer labAnalisysNo;

	@Column(name = "LabPurchaseId")
	private Integer labPurchaseId;

	@Column(name = "InvGrnId")
	private Integer invGrnId;

	@Column(name = "InvGrnDetailId")
	private Integer invGrnDetailId;

	@Column(name = "RefDocNo")
	private Integer refDocNo;

	@Column(name = "RefDocInvoiceNo")
	private Integer refDocInvoiceNo;

	@Column(name = "RefDocId")
	private Integer refDocId;

	@Column(name = "RefDocumentTypeId")
	private Integer refDocumentTypeId;

	@Column(name = "RefInvoiceQty")
	private BigDecimal refInvoiceQty;

	@Column(name = "RefInvoiceWeight")
	private BigDecimal refInvoiceWeight;

	@Column(name = "RefDocSubId")
	private Integer refDocSubId;

	public Integer getId() { return id; }
	public void setId(Integer id) { this.id = id; }
	public Warehouse getWarehouse() { return warehouse; }
	public void setWarehouse(Warehouse warehouse) { this.warehouse = warehouse; }
	public String getRackName() { return rackName; }
	public void setRackName(String rackName) { this.rackName = rackName; }
	public Integer getSortNo() { return sortNo; }
	public void setSortNo(Integer sortNo) { this.sortNo = sortNo; }
	public LocalDateTime getEntryDate() { return entryDate; }
	public void setEntryDate(LocalDateTime entryDate) { this.entryDate = entryDate; }
	public Integer getEntryUserId() { return entryUserId; }
	public void setEntryUserId(Integer entryUserId) { this.entryUserId = entryUserId; }
	public LocalDateTime getModifyDate() { return modifyDate; }
	public void setModifyDate(LocalDateTime modifyDate) { this.modifyDate = modifyDate; }
	public Integer getModifyUserId() { return modifyUserId; }
	public void setModifyUserId(Integer modifyUserId) { this.modifyUserId = modifyUserId; }
	public Boolean getIsActive() { return isActive; }
	public void setIsActive(Boolean isActive) { this.isActive = isActive; }
	public Integer getInvWarehouseId() { return invWarehouseId; }
	public void setInvWarehouseId(Integer invWarehouseId) { this.invWarehouseId = invWarehouseId; }
	public Integer getPurchaseOrderId() { return purchaseOrderId; }
	public void setPurchaseOrderId(Integer purchaseOrderId) { this.purchaseOrderId = purchaseOrderId; }
	public Integer getPurchaseOrderDetailId() { return purchaseOrderDetailId; }
	public void setPurchaseOrderDetailId(Integer purchaseOrderDetailId) { this.purchaseOrderDetailId = purchaseOrderDetailId; }
	public String getPurchaseOrder() { return purchaseOrder; }
	public void setPurchaseOrder(String purchaseOrder) { this.purchaseOrder = purchaseOrder; }
	public LocalDateTime getPurchaseOrderDate() { return purchaseOrderDate; }
	public void setPurchaseOrderDate(LocalDateTime purchaseOrderDate) { this.purchaseOrderDate = purchaseOrderDate; }
	public BigDecimal getPurchaseOrderQty() { return purchaseOrderQty; }
	public void setPurchaseOrderQty(BigDecimal purchaseOrderQty) { this.purchaseOrderQty = purchaseOrderQty; }
	public Integer getInvPurchasedemondId() { return invPurchasedemondId; }
	public void setInvPurchasedemondId(Integer invPurchasedemondId) { this.invPurchasedemondId = invPurchasedemondId; }
	public Integer getPurchaseDemondDetailId() { return purchaseDemondDetailId; }
	public void setPurchaseDemondDetailId(Integer purchaseDemondDetailId) { this.purchaseDemondDetailId = purchaseDemondDetailId; }
	public Integer getPurchaseDemandNo() { return purchaseDemandNo; }
	public void setPurchaseDemandNo(Integer purchaseDemandNo) { this.purchaseDemandNo = purchaseDemandNo; }
	public LocalDateTime getPurchaseDemandDocDate() { return purchaseDemandDocDate; }
	public void setPurchaseDemandDocDate(LocalDateTime purchaseDemandDocDate) { this.purchaseDemandDocDate = purchaseDemandDocDate; }
	public BigDecimal getPurchaseDemandQty() { return purchaseDemandQty; }
	public void setPurchaseDemandQty(BigDecimal purchaseDemandQty) { this.purchaseDemandQty = purchaseDemandQty; }
	public Integer getDeliveryChallanId() { return deliveryChallanId; }
	public void setDeliveryChallanId(Integer deliveryChallanId) { this.deliveryChallanId = deliveryChallanId; }
	public Integer getDeliveryChallanDetailId() { return deliveryChallanDetailId; }
	public void setDeliveryChallanDetailId(Integer deliveryChallanDetailId) { this.deliveryChallanDetailId = deliveryChallanDetailId; }
	public Integer getDeliveryChallanNo() { return deliveryChallanNo; }
	public void setDeliveryChallanNo(Integer deliveryChallanNo) { this.deliveryChallanNo = deliveryChallanNo; }
	public LocalDateTime getDeliveryChallanDate() { return deliveryChallanDate; }
	public void setDeliveryChallanDate(LocalDateTime deliveryChallanDate) { this.deliveryChallanDate = deliveryChallanDate; }
	public BigDecimal getDeliveryChallanQty() { return deliveryChallanQty; }
	public void setDeliveryChallanQty(BigDecimal deliveryChallanQty) { this.deliveryChallanQty = deliveryChallanQty; }
	public Integer getPurchasePreBillId() { return purchasePreBillId; }
	public void setPurchasePreBillId(Integer purchasePreBillId) { this.purchasePreBillId = purchasePreBillId; }
	public Integer getPurchasePreBillDetailId() { return purchasePreBillDetailId; }
	public void setPurchasePreBillDetailId(Integer purchasePreBillDetailId) { this.purchasePreBillDetailId = purchasePreBillDetailId; }
	public Integer getPurchasePreBillNo() { return purchasePreBillNo; }
	public void setPurchasePreBillNo(Integer purchasePreBillNo) { this.purchasePreBillNo = purchasePreBillNo; }
	public BigDecimal getPurchasePreBillQty() { return purchasePreBillQty; }
	public void setPurchasePreBillQty(BigDecimal purchasePreBillQty) { this.purchasePreBillQty = purchasePreBillQty; }
	public BigDecimal getUsedDemandQtyInPreBill() { return usedDemandQtyInPreBill; }
	public void setUsedDemandQtyInPreBill(BigDecimal usedDemandQtyInPreBill) { this.usedDemandQtyInPreBill = usedDemandQtyInPreBill; }
	public BigDecimal getUsedDemandQtyInGrn() { return usedDemandQtyInGrn; }
	public void setUsedDemandQtyInGrn(BigDecimal usedDemandQtyInGrn) { this.usedDemandQtyInGrn = usedDemandQtyInGrn; }
	public BigDecimal getUsedOrderQtyInPreBill() { return usedOrderQtyInPreBill; }
	public void setUsedOrderQtyInPreBill(BigDecimal usedOrderQtyInPreBill) { this.usedOrderQtyInPreBill = usedOrderQtyInPreBill; }
	public BigDecimal getUsedOrderQtyInGrn() { return usedOrderQtyInGrn; }
	public void setUsedOrderQtyInGrn(BigDecimal usedOrderQtyInGrn) { this.usedOrderQtyInGrn = usedOrderQtyInGrn; }
	public BigDecimal getUsedDeliveryChallanQtyGrn() { return usedDeliveryChallanQtyGrn; }
	public void setUsedDeliveryChallanQtyGrn(BigDecimal usedDeliveryChallanQtyGrn) { this.usedDeliveryChallanQtyGrn = usedDeliveryChallanQtyGrn; }
	public Integer getInvRevExpItemId() { return invRevExpItemId; }
	public void setInvRevExpItemId(Integer invRevExpItemId) { this.invRevExpItemId = invRevExpItemId; }
	public BigDecimal getQty() { return qty; }
	public void setQty(BigDecimal qty) { this.qty = qty; }
	public BigDecimal getRate() { return rate; }
	public void setRate(BigDecimal rate) { this.rate = rate; }
	public BigDecimal getAmount() { return amount; }
	public void setAmount(BigDecimal amount) { this.amount = amount; }
	public String getRemarks() { return remarks; }
	public void setRemarks(String remarks) { this.remarks = remarks; }
	public Integer getVoucherHeadId() { return voucherHeadId; }
	public void setVoucherHeadId(Integer voucherHeadId) { this.voucherHeadId = voucherHeadId; }
	public Integer getDocumentTypeId() { return documentTypeId; }
	public void setDocumentTypeId(Integer documentTypeId) { this.documentTypeId = documentTypeId; }
	public Integer getNoOfAttachments() { return noOfAttachments; }
	public void setNoOfAttachments(Integer noOfAttachments) { this.noOfAttachments = noOfAttachments; }
	public Integer getLabAnalisysNo() { return labAnalisysNo; }
	public void setLabAnalisysNo(Integer labAnalisysNo) { this.labAnalisysNo = labAnalisysNo; }
	public Integer getLabPurchaseId() { return labPurchaseId; }
	public void setLabPurchaseId(Integer labPurchaseId) { this.labPurchaseId = labPurchaseId; }
	public Integer getInvGrnId() { return invGrnId; }
	public void setInvGrnId(Integer invGrnId) { this.invGrnId = invGrnId; }
	public Integer getInvGrnDetailId() { return invGrnDetailId; }
	public void setInvGrnDetailId(Integer invGrnDetailId) { this.invGrnDetailId = invGrnDetailId; }
	public Integer getRefDocNo() { return refDocNo; }
	public void setRefDocNo(Integer refDocNo) { this.refDocNo = refDocNo; }
	public Integer getRefDocInvoiceNo() { return refDocInvoiceNo; }
	public void setRefDocInvoiceNo(Integer refDocInvoiceNo) { this.refDocInvoiceNo = refDocInvoiceNo; }
	public Integer getRefDocId() { return refDocId; }
	public void setRefDocId(Integer refDocId) { this.refDocId = refDocId; }
	public Integer getRefDocumentTypeId() { return refDocumentTypeId; }
	public void setRefDocumentTypeId(Integer refDocumentTypeId) { this.refDocumentTypeId = refDocumentTypeId; }
	public BigDecimal getRefInvoiceQty() { return refInvoiceQty; }
	public void setRefInvoiceQty(BigDecimal refInvoiceQty) { this.refInvoiceQty = refInvoiceQty; }
	public BigDecimal getRefInvoiceWeight() { return refInvoiceWeight; }
	public void setRefInvoiceWeight(BigDecimal refInvoiceWeight) { this.refInvoiceWeight = refInvoiceWeight; }
	public Integer getRefDocSubId() { return refDocSubId; }
	public void setRefDocSubId(Integer refDocSubId) { this.refDocSubId = refDocSubId; }
}
