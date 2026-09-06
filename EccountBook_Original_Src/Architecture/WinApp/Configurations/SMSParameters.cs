using System;

namespace Architecture.WinApp.Configurations;

public class SMSParameters
{
	public string AccountCode { get; set; }

	public string AccountTitle { get; set; }

	public string DocumentNo { get; set; }

	public DateTime DocumentDate { get; set; }

	public string OtherDocNo { get; set; }

	public string Remarks { get; set; }

	public double Amount { get; set; }

	public double Quantity { get; set; }

	public string ChequeNo { get; set; }

	public DateTime ChequeDate { get; set; }

	public string CompanyName { get; set; }

	public string CellNo { get; set; }

	public string SCS { get; set; }

	public string GpNo { get; set; }

	public DateTime GpDate { get; set; }

	public string VehicleNo { get; set; }

	public string BilityNo { get; set; }

	public string ContainerNo { get; set; }

	public string SealNo { get; set; }

	public string DriverName { get; set; }

	public string DriverCellNo { get; set; }

	public double NetWeight { get; set; }
}
