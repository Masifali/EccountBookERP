using System;

namespace Architecture.Model;

public class GridDetail
{
	public long GridDetailId { get; set; }

	public int GridId { get; set; }

	public string ControlType { get; set; }

	public string DataPropertyName { get; set; }

	public string ValueType { get; set; }

	public string HeaderText { get; set; }

	public int MaxInputLength { get; set; }

	public int LineSno { get; set; }

	public int ColSeq { get; set; }

	public int ColWidth { get; set; }

	public int? LookUpConfigId { get; set; }

	public bool IsShow { get; set; }

	public bool IsReadOnly { get; set; }

	public bool IsResizeable { get; set; }

	public bool IsSelectAll { get; set; }

	public bool IsMandotory { get; set; }

	public string CellFormat { get; set; }

	public string CellAlignment { get; set; }

	public DateTime CreatedOn { get; set; }

	public long? CreatedUserId { get; set; }

	public DateTime LastModifiedOn { get; set; }

	public long? LastModifiedUserId { get; set; }

	public long? AppUserLogId { get; set; }

	public byte[] RowVersion { get; set; }

	public int AppActionId { get; set; }
}
