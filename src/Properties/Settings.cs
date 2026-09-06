namespace Architecture.WinApp.Properties
{
using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

[CompilerGenerated]
[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.14.0.0")]
internal sealed class Settings : ApplicationSettingsBase
{
	private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());

	public static Settings Default { get { return defaultInstance; } }

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string PanelColor
	{
		get
		{
			return (string)this["PanelColor"];
		}
		set
		{
			this["PanelColor"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string ButtonColor
	{
		get
		{
			return (string)this["ButtonColor"];
		}
		set
		{
			this["ButtonColor"] = value;
		}
	}
}

}
