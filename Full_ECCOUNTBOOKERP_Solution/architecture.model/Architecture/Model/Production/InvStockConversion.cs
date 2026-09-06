using System;
using System.Collections.Generic;
using Architecture.Model.ContractorWages;
using Architecture.Model.Inventory;

namespace Architecture.Model.Production;

public class InvStockConversion
{
	public bool PostState { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime? PostDate { get; set; }

	public int BranchedId { get; set; }

	public int CompanyId { get; set; }

	public int DocSrNo { get; set; }

	public int ConversionTypeId { get; set; }

	public int GainLossId { get; set; }

	public int DocTypeId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int PostUser { get; set; }

	public int ProjectsId { get; set; }

	public string DocManualRef { get; set; }

	public string ProductionNo { get; set; }

	public string Remarks { get; set; }

	public int ActionId { get; set; }

	public int parentCategoryId { get; set; }

	public int FinancialYearId { get; set; }

	public int EBDepartmentId { get; set; }

	public int DifferenceAccountId { get; set; }

	public virtual string InputDetailRowsRemoveIds { get; set; }

	public virtual List<InvStockConversionDetail> invStockConversionDetails { get; set; }

	public virtual List<InvStockConversionPackingMaterial> invStockConversionPackings { get; set; }

	public virtual List<InvStockConversionAddExpense> invStockConversionAddExpenses { get; set; }

	public virtual List<InvContractorWagesBillHeader> InvContractorWagesBillHeaderList { get; set; }

	public virtual List<InventoryStockEvalautionDetail> InventoryStockEvalautionDetailslist { get; set; }

	public InvStockConversion()
	{
		invStockConversionDetails = new List<InvStockConversionDetail>();
		invStockConversionPackings = new List<InvStockConversionPackingMaterial>();
		invStockConversionAddExpenses = new List<InvStockConversionAddExpense>();
		InvContractorWagesBillHeaderList = new List<InvContractorWagesBillHeader>();
		InventoryStockEvalautionDetailslist = new List<InventoryStockEvalautionDetail>();
	}
}
