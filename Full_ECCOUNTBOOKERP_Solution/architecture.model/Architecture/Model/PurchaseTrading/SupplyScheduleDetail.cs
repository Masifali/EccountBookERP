using System;

namespace Architecture.Model.PurchaseTrading;

public class SupplyScheduleDetail
{
	public DateTime MonthDesc { get; set; }

	public double Day1 { get; set; }

	public double Day10 { get; set; }

	public double Day11 { get; set; }

	public double Day12 { get; set; }

	public double Day13 { get; set; }

	public double Day14 { get; set; }

	public double Day15 { get; set; }

	public double Day16 { get; set; }

	public double Day17 { get; set; }

	public double Day18 { get; set; }

	public double Day19 { get; set; }

	public double Day2 { get; set; }

	public double Day20 { get; set; }

	public double Day21 { get; set; }

	public double Day22 { get; set; }

	public double Day23 { get; set; }

	public double Day24 { get; set; }

	public double Day25 { get; set; }

	public double Day26 { get; set; }

	public double Day27 { get; set; }

	public double Day28 { get; set; }

	public double Day29 { get; set; }

	public double Day3 { get; set; }

	public double Day30 { get; set; }

	public double Day31 { get; set; }

	public double Day4 { get; set; }

	public double Day5 { get; set; }

	public double Day6 { get; set; }

	public double Day7 { get; set; }

	public double Day8 { get; set; }

	public double Day9 { get; set; }

	public double TotalQty { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ItemUOMId { get; set; }

	public int OrderId { get; set; }

	public int SupplyScheduleId { get; set; }

	public virtual int PurchaseOrderNo { get; set; }

	public virtual string UOMCode { get; set; }

	public virtual string ItemName { get; set; }

	public virtual double UOMEquivalent { get; set; }
}
