using System;

namespace Architecture.Model.Inventory;

public class ItemImage
{
	public int Id { get; set; }

	public int ItemId { get; set; }

	public string ImagePath { get; set; }

	public string FileName { get; set; }

	public DateTime EffetedDate { get; set; }

	public bool IsActive { get; set; }

	public int SortNo { get; set; }

	public bool IsProfileImage { get; set; }
}
