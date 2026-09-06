using System;

namespace Architecture.Model.HRM.DeviceManagement;

public class hrmEmployeeBiometric
{
	public byte ActionTypeId { get; set; }

	public byte FingerId { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public long BiometricTypeProfileId { get; set; }

	public long AlteredById { get; set; }

	public long CreatedById { get; set; }

	public long EmployeeBiometricId { get; set; }

	public long EmployeeId { get; set; }

	public long UserLogId { get; set; }

	public string BiometricPassword { get; set; }

	public string EmployeeNo { get; set; }

	public string EyeTemplate { get; set; }

	public string FaceTemplate { get; set; }

	public string FingerTemplate { get; set; }
}
