using System;
using System.Collections.Generic;

namespace Architecture.Model.POS;

public class WsRmSalesOrder
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DeliveryDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DeliveryTermsId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int DueDays { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrderType { get; set; }

	public int OrganizationId { get; set; }

	public int PaymentTermsId { get; set; }

	public int SalesManId { get; set; }

	public int SupplierId { get; set; }

	public int SupplierIdShipTo { get; set; }

	public string RefCellNo { get; set; }

	public string RefDocNo { get; set; }

	public string RemarksHeader { get; set; }

	public string Status { get; set; }

	public virtual List<WsRmSalesOrderDetail> WsRmSalesOrderDetailsList { get; set; }
}
