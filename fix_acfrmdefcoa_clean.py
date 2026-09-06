import os

path = r"d:\CShapEccorErp\src\AcfrmDefCoa.cs"
with open(path, "r", encoding="utf-8", errors="ignore") as f:
    code = f.read()

aliases = """using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
using GroupTotals = Janus.Windows.GridEX.GroupTotals;
using FilterMode = Janus.Windows.GridEX.FilterMode;
using FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle;
using EditType = Janus.Windows.GridEX.EditType;
using FilterEditType = Janus.Windows.GridEX.FilterEditType;
using VisualStyle = Janus.Windows.GridEX.VisualStyle;
using ScrollableBase = Janus.Windows.GridEX.ScrollableBase;
using HierarchicalMode = Janus.Windows.GridEX.HierarchicalMode;
using SelfReferencingAggregationMode = Janus.Windows.GridEX.SelfReferencingAggregationMode;
using ConditionOperator = Janus.Windows.GridEX.ConditionOperator;
using COAAllocation = Architecture.Model.Accounts.COAAllocation;
using MultiCurrency = global::Architecture.Model.MultiCurrency;
using AcLookUps = global::Architecture.Model.Accounts.AcLookUps;
using AccountNotes = global::Architecture.Model.Accounts.AccountNotes;
using CustomerGroup = global::Architecture.Model.Inventory.CustomerGroup;
using AccountTypes = global::Architecture.Model.Accounts.AccountTypes;
"""

if "using MultiCurrency =" not in code:
    code = code.replace("namespace Architecture.WinApp\n{\n", "namespace Architecture.WinApp\n{\n" + aliases)
    code = code.replace("namespace Architecture.WinApp\r\n{\r\n", "namespace Architecture.WinApp\r\n{\r\n" + aliases)

# BLL Method replacements
code = code.replace("COAAllocation.GetParentCodeByChartofAccountId", "Architecture.BLL.Accounts.COAAllocation.GetParentCodeByChartofAccountId")
code = code.replace("COAAllocation.Get3rdLevelGroupAccounts", "Architecture.BLL.Accounts.COAAllocation.Get3rdLevelGroupAccounts")
code = code.replace("ChartofAccount.ReadAllAccountgroup", "Architecture.BLL.Accounts.ChartofAccount.ReadAllAccountgroup")
code = code.replace("MultiCurrency.GetAll", "Architecture.BLL.MultiCurrency.GetAll")
code = code.replace("AcLookUps.GetByProfileTypeId", "Architecture.BLL.Accounts.AcLookUps.GetByProfileTypeId")
code = code.replace("CustomerGroup.GetAll", "Architecture.BLL.Inventory.CustomerGroup.GetAll")
code = code.replace("CommonServices.", "Architecture.WinApp.CommonServices.")

with open(path, "w", encoding="utf-8") as f:
    f.write(code)

print("Updated AcfrmDefCoa.cs model aliases and BLL calls!")
