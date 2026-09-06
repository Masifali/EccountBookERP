using System;

namespace Architecture.Model.Themes_ERP;

public class ThemeSettings
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int ScreenId { get; set; }

	public long Id { get; set; }

	public string Button_BackColor { get; set; }

	public string Button_FontColor { get; set; }

	public string HeaderPanel_BackColor { get; set; }

	public string HeaderPanel_ControlsBackColor { get; set; }

	public string HeaderPanel_ControlsFontColor { get; set; }

	public string HeaderPanel_FontColor { get; set; }

	public string ScreenName { get; set; }
}
