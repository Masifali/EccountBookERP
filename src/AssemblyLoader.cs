using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Costura
{

[CompilerGenerated]
internal static class AssemblyLoader
{
	private static object nullCacheLock = new object();

	private static Dictionary<string, bool> nullCache = new Dictionary<string, bool>();

	private static Dictionary<string, string> assemblyNames = new Dictionary<string, string>();

	private static Dictionary<string, string> symbolNames = new Dictionary<string, string>();

	private static int isAttached;

	private static string CultureToString(CultureInfo culture)
	{
		if (culture == null)
		{
			return string.Empty;
		}
		return culture.Name;
	}

	private static Assembly ReadExistingAssembly(AssemblyName name)
	{
		AppDomain currentDomain = AppDomain.CurrentDomain;
		Assembly[] assemblies = currentDomain.GetAssemblies();
		Assembly[] array = assemblies;
		foreach (Assembly assembly in array)
		{
			AssemblyName name2 = assembly.GetName();
			if (string.Equals(name2.Name, name.Name, StringComparison.InvariantCultureIgnoreCase) && string.Equals(CultureToString(name2.CultureInfo), CultureToString(name.CultureInfo), StringComparison.InvariantCultureIgnoreCase))
			{
				return assembly;
			}
		}
		return null;
	}

	private static string GetAssemblyResourceName(AssemblyName requestedAssemblyName)
	{
		string name = requestedAssemblyName.Name.ToLowerInvariant();
		if (requestedAssemblyName.CultureInfo != null && !string.IsNullOrEmpty(requestedAssemblyName.CultureInfo.Name))
		{
			name = (CultureToString(requestedAssemblyName.CultureInfo) + "." + name).ToLowerInvariant();
		}
		return name;
	}

	private static void CopyTo(Stream source, Stream destination)
	{
		byte[] array = new byte[81920];
		int count;
		while ((count = source.Read(array, 0, array.Length)) != 0)
		{
			destination.Write(array, 0, count);
		}
	}

	private static Stream LoadStream(string fullName)
	{
		Assembly executingAssembly = Assembly.GetExecutingAssembly();
		if (fullName.EndsWith(".compressed"))
		{
			using (Stream stream = executingAssembly.GetManifestResourceStream(fullName))
			{
				using DeflateStream source = new DeflateStream(stream, CompressionMode.Decompress);
				MemoryStream memoryStream = new MemoryStream();
				CopyTo(source, memoryStream);
				memoryStream.Position = 0L;
				return memoryStream;
			}
		}
		return executingAssembly.GetManifestResourceStream(fullName);
	}

	private static Stream LoadStream(Dictionary<string, string> resourceNames, string name)
	{
		if (resourceNames.TryGetValue(name, out var value))
		{
			return LoadStream(value);
		}
		return null;
	}

	private static byte[] ReadStream(Stream stream)
	{
		byte[] data = new byte[stream.Length];
		stream.Read(data, 0, data.Length);
		return data;
	}

	private static Assembly ReadFromEmbeddedResources(Dictionary<string, string> assemblyNames, Dictionary<string, string> symbolNames, AssemblyName requestedAssemblyName)
	{
		string name = GetAssemblyResourceName(requestedAssemblyName);
		byte[] assemblyData;
		using (Stream stream = LoadStream(assemblyNames, name))
		{
			if (stream == null)
			{
				return null;
			}
			assemblyData = ReadStream(stream);
		}
		using (Stream stream2 = LoadStream(symbolNames, name))
		{
			if (stream2 != null)
			{
				byte[] rawSymbolStore = ReadStream(stream2);
				return Assembly.Load(assemblyData, rawSymbolStore);
			}
		}
		return Assembly.Load(assemblyData);
	}

	public static Assembly ResolveAssembly(object sender, ResolveEventArgs e)
	{
		string assemblyNameAsString = e.Name;
		AssemblyName assemblyName = new AssemblyName(assemblyNameAsString);
		lock (nullCacheLock)
		{
			if (nullCache.ContainsKey(assemblyNameAsString))
			{
				return null;
			}
		}
		Assembly assembly = ReadExistingAssembly(assemblyName);
		if ((object)assembly != null)
		{
			return assembly;
		}
		assembly = ReadFromEmbeddedResources(assemblyNames, symbolNames, assemblyName);
		if ((object)assembly == null)
		{
			lock (nullCacheLock)
			{
				nullCache[assemblyNameAsString] = true;
			}
			if ((assemblyName.Flags & AssemblyNameFlags.Retargetable) != 0)
			{
				assembly = Assembly.Load(assemblyName);
			}
		}
		return assembly;
	}

	static AssemblyLoader()
	{
		assemblyNames.Add("aforge", "costura.aforge.dll.compressed");
		assemblyNames.Add("aforge.video.directshow", "costura.aforge.video.directshow.dll.compressed");
		assemblyNames.Add("aforge.video", "costura.aforge.video.dll.compressed");
		assemblyNames.Add("architecture.bll", "costura.architecture.bll.dll.compressed");
		symbolNames.Add("architecture.bll", "costura.architecture.bll.pdb.compressed");
		assemblyNames.Add("architecture.common", "costura.architecture.common.dll.compressed");
		symbolNames.Add("architecture.common", "costura.architecture.common.pdb.compressed");
		assemblyNames.Add("architecture.dal", "costura.architecture.dal.dll.compressed");
		symbolNames.Add("architecture.dal", "costura.architecture.dal.pdb.compressed");
		assemblyNames.Add("architecture.model", "costura.architecture.model.dll.compressed");
		symbolNames.Add("architecture.model", "costura.architecture.model.pdb.compressed");
		assemblyNames.Add("architecture.psdsms", "costura.architecture.psdsms.exe.compressed");
		symbolNames.Add("architecture.psdsms", "costura.architecture.psdsms.pdb.compressed");
		assemblyNames.Add("architecture.reports", "costura.architecture.reports.exe.compressed");
		symbolNames.Add("architecture.reports", "costura.architecture.reports.pdb.compressed");
		assemblyNames.Add("architecture.webcamera", "costura.architecture.webcamera.dll.compressed");
		symbolNames.Add("architecture.webcamera", "costura.architecture.webcamera.pdb.compressed");
		assemblyNames.Add("architecture.webhelper", "costura.architecture.webhelper.dll.compressed");
		symbolNames.Add("architecture.webhelper", "costura.architecture.webhelper.pdb.compressed");
		assemblyNames.Add("architecture.zktfingerprint", "costura.architecture.zktfingerprint.dll.compressed");
		symbolNames.Add("architecture.zktfingerprint", "costura.architecture.zktfingerprint.pdb.compressed");
		assemblyNames.Add("axinterop.wmplib", "costura.axinterop.wmplib.dll.compressed");
		assemblyNames.Add("circularprogressbar", "costura.circularprogressbar.dll.compressed");
		assemblyNames.Add("crystaldecisions.crystalreports.design", "costura.crystaldecisions.crystalreports.design.dll.compressed");
		assemblyNames.Add("crystaldecisions.crystalreports.engine", "costura.crystaldecisions.crystalreports.engine.dll.compressed");
		assemblyNames.Add("crystaldecisions.reportappserver.clientdoc", "costura.crystaldecisions.reportappserver.clientdoc.dll.compressed");
		assemblyNames.Add("crystaldecisions.reportappserver.commlayer", "costura.crystaldecisions.reportappserver.commlayer.dll.compressed");
		assemblyNames.Add("crystaldecisions.reportappserver.commoncontrols", "costura.crystaldecisions.reportappserver.commoncontrols.dll.compressed");
		assemblyNames.Add("crystaldecisions.reportappserver.commonobjectmodel", "costura.crystaldecisions.reportappserver.commonobjectmodel.dll.compressed");
		assemblyNames.Add("crystaldecisions.reportappserver.controllers", "costura.crystaldecisions.reportappserver.controllers.dll.compressed");
		assemblyNames.Add("crystaldecisions.reportappserver.cubedefmodel", "costura.crystaldecisions.reportappserver.cubedefmodel.dll.compressed");
		assemblyNames.Add("crystaldecisions.reportappserver.datadefmodel", "costura.crystaldecisions.reportappserver.datadefmodel.dll.compressed");
		assemblyNames.Add("crystaldecisions.reportappserver.datasetconversion", "costura.crystaldecisions.reportappserver.datasetconversion.dll.compressed");
		assemblyNames.Add("crystaldecisions.reportappserver.objectfactory", "costura.crystaldecisions.reportappserver.objectfactory.dll.compressed");
		assemblyNames.Add("crystaldecisions.reportappserver.prompting", "costura.crystaldecisions.reportappserver.prompting.dll.compressed");
		assemblyNames.Add("crystaldecisions.reportappserver.reportdefmodel", "costura.crystaldecisions.reportappserver.reportdefmodel.dll.compressed");
		assemblyNames.Add("crystaldecisions.reportappserver.xmlserialize", "costura.crystaldecisions.reportappserver.xmlserialize.dll.compressed");
		assemblyNames.Add("crystaldecisions.reportsource", "costura.crystaldecisions.reportsource.dll.compressed");
		assemblyNames.Add("crystaldecisions.shared", "costura.crystaldecisions.shared.dll.compressed");
		assemblyNames.Add("crystaldecisions.web", "costura.crystaldecisions.web.dll.compressed");
		assemblyNames.Add("crystaldecisions.windows.forms", "costura.crystaldecisions.windows.forms.dll.compressed");
		assemblyNames.Add("dotnetzip", "costura.dotnetzip.dll.compressed");
		symbolNames.Add("dotnetzip", "costura.dotnetzip.pdb.compressed");
		assemblyNames.Add("edsdklib", "costura.edsdklib.dll.compressed");
		symbolNames.Add("edsdklib", "costura.edsdklib.pdb.compressed");
		assemblyNames.Add("epplus", "costura.epplus.dll.compressed");
		assemblyNames.Add("epplus.interfaces", "costura.epplus.interfaces.dll.compressed");
		assemblyNames.Add("epplus.system.drawing", "costura.epplus.system.drawing.dll.compressed");
		assemblyNames.Add("exceldatareader.dataset", "costura.exceldatareader.dataset.dll.compressed");
		symbolNames.Add("exceldatareader.dataset", "costura.exceldatareader.dataset.pdb.compressed");
		assemblyNames.Add("exceldatareader", "costura.exceldatareader.dll.compressed");
		symbolNames.Add("exceldatareader", "costura.exceldatareader.pdb.compressed");
		assemblyNames.Add("gsmcommserver", "costura.gsmcommserver.dll.compressed");
		assemblyNames.Add("gsmcommshared", "costura.gsmcommshared.dll.compressed");
		assemblyNames.Add("gsmcommunication", "costura.gsmcommunication.dll.compressed");
		assemblyNames.Add("icsharpcode.sharpziplib", "costura.icsharpcode.sharpziplib.dll.compressed");
		symbolNames.Add("icsharpcode.sharpziplib", "costura.icsharpcode.sharpziplib.pdb.compressed");
		assemblyNames.Add("infragistics2.documents.v10.1", "costura.infragistics2.documents.v10.1.dll.compressed");
		assemblyNames.Add("infragistics2.excel.v10.1", "costura.infragistics2.excel.v10.1.dll.compressed");
		assemblyNames.Add("infragistics2.shared.v10.1", "costura.infragistics2.shared.v10.1.dll.compressed");
		assemblyNames.Add("infragistics2.win.appstylistsupport.v10.1", "costura.infragistics2.win.appstylistsupport.v10.1.dll.compressed");
		assemblyNames.Add("infragistics2.win.misc.v10.1", "costura.infragistics2.win.misc.v10.1.dll.compressed");
		assemblyNames.Add("infragistics2.win.supportdialogs.v10.1", "costura.infragistics2.win.supportdialogs.v10.1.dll.compressed");
		assemblyNames.Add("infragistics2.win.ultrachart.v10.1.design", "costura.infragistics2.win.ultrachart.v10.1.design.dll.compressed");
		assemblyNames.Add("infragistics2.win.ultragauge.v10.1.design", "costura.infragistics2.win.ultragauge.v10.1.design.dll.compressed");
		assemblyNames.Add("infragistics2.win.ultrawincalcmanager.v10.1", "costura.infragistics2.win.ultrawincalcmanager.v10.1.dll.compressed");
		assemblyNames.Add("infragistics2.win.ultrawincalcmanager.v10.1.formulabuilder", "costura.infragistics2.win.ultrawincalcmanager.v10.1.formulabuilder.dll.compressed");
		assemblyNames.Add("infragistics2.win.ultrawinchart.v10.1", "costura.infragistics2.win.ultrawinchart.v10.1.dll.compressed");
		assemblyNames.Add("infragistics2.win.ultrawindatasource.v10.1", "costura.infragistics2.win.ultrawindatasource.v10.1.dll.compressed");
		assemblyNames.Add("infragistics2.win.ultrawindock.v10.1", "costura.infragistics2.win.ultrawindock.v10.1.dll.compressed");
		assemblyNames.Add("infragistics2.win.ultrawineditors.v10.1", "costura.infragistics2.win.ultrawineditors.v10.1.dll.compressed");
		assemblyNames.Add("infragistics2.win.ultrawinexplorerbar.v10.1", "costura.infragistics2.win.ultrawinexplorerbar.v10.1.dll.compressed");
		assemblyNames.Add("infragistics2.win.ultrawinganttview.v10.1", "costura.infragistics2.win.ultrawinganttview.v10.1.dll.compressed");
		assemblyNames.Add("infragistics2.win.ultrawingauge.v10.1", "costura.infragistics2.win.ultrawingauge.v10.1.dll.compressed");
		assemblyNames.Add("infragistics2.win.ultrawingrid.documentexport.v10.1", "costura.infragistics2.win.ultrawingrid.documentexport.v10.1.dll.compressed");
		assemblyNames.Add("infragistics2.win.ultrawingrid.excelexport.v10.1", "costura.infragistics2.win.ultrawingrid.excelexport.v10.1.dll.compressed");
		assemblyNames.Add("infragistics2.win.ultrawingrid.v10.1", "costura.infragistics2.win.ultrawingrid.v10.1.dll.compressed");
		assemblyNames.Add("infragistics2.win.ultrawininkprovider.ink17.v10.1", "costura.infragistics2.win.ultrawininkprovider.ink17.v10.1.dll.compressed");
		assemblyNames.Add("infragistics2.win.ultrawininkprovider.v10.1", "costura.infragistics2.win.ultrawininkprovider.v10.1.dll.compressed");
		assemblyNames.Add("infragistics2.win.ultrawinlistbar.v10.1", "costura.infragistics2.win.ultrawinlistbar.v10.1.dll.compressed");
		assemblyNames.Add("infragistics2.win.ultrawinlistview.v10.1", "costura.infragistics2.win.ultrawinlistview.v10.1.dll.compressed");
		assemblyNames.Add("infragistics2.win.ultrawinmaskededit.v10.1", "costura.infragistics2.win.ultrawinmaskededit.v10.1.dll.compressed");
		assemblyNames.Add("infragistics2.win.ultrawinprintpreviewdialog.v10.1", "costura.infragistics2.win.ultrawinprintpreviewdialog.v10.1.dll.compressed");
		assemblyNames.Add("infragistics2.win.ultrawinschedule.v10.1", "costura.infragistics2.win.ultrawinschedule.v10.1.dll.compressed");
		assemblyNames.Add("infragistics2.win.ultrawinspellchecker.v10.1", "costura.infragistics2.win.ultrawinspellchecker.v10.1.dll.compressed");
		assemblyNames.Add("infragistics2.win.ultrawinstatusbar.v10.1", "costura.infragistics2.win.ultrawinstatusbar.v10.1.dll.compressed");
		assemblyNames.Add("infragistics2.win.ultrawintabbedmdi.v10.1", "costura.infragistics2.win.ultrawintabbedmdi.v10.1.dll.compressed");
		assemblyNames.Add("infragistics2.win.ultrawintabcontrol.v10.1", "costura.infragistics2.win.ultrawintabcontrol.v10.1.dll.compressed");
		assemblyNames.Add("infragistics2.win.ultrawintoolbars.v10.1", "costura.infragistics2.win.ultrawintoolbars.v10.1.dll.compressed");
		assemblyNames.Add("infragistics2.win.ultrawintree.v10.1", "costura.infragistics2.win.ultrawintree.v10.1.dll.compressed");
		assemblyNames.Add("infragistics2.win.v10.1.design", "costura.infragistics2.win.v10.1.design.dll.compressed");
		assemblyNames.Add("infragistics2.win.v10.1", "costura.infragistics2.win.v10.1.dll.compressed");
		assemblyNames.Add("interop.zkemkeeper", "costura.interop.zkemkeeper.dll.compressed");
		assemblyNames.Add("janus.data.v3", "costura.janus.data.v3.dll.compressed");
		assemblyNames.Add("janus.windows.buttonbar.v3", "costura.janus.windows.buttonbar.v3.dll.compressed");
		assemblyNames.Add("janus.windows.calendarcombo.v3", "costura.janus.windows.calendarcombo.v3.dll.compressed");
		assemblyNames.Add("janus.windows.common.v3", "costura.janus.windows.common.v3.dll.compressed");
		assemblyNames.Add("janus.windows.explorerbar.v3", "costura.janus.windows.explorerbar.v3.dll.compressed");
		assemblyNames.Add("janus.windows.filtereditor.v3", "costura.janus.windows.filtereditor.v3.dll.compressed");
		assemblyNames.Add("janus.windows.gridex.v3", "costura.janus.windows.gridex.v3.dll.compressed");
		assemblyNames.Add("janus.windows.ribbon.v3", "costura.janus.windows.ribbon.v3.dll.compressed");
		assemblyNames.Add("janus.windows.schedule.v3", "costura.janus.windows.schedule.v3.dll.compressed");
		assemblyNames.Add("janus.windows.timeline.v3", "costura.janus.windows.timeline.v3.dll.compressed");
		assemblyNames.Add("janus.windows.ui.v3", "costura.janus.windows.ui.v3.dll.compressed");
		assemblyNames.Add("libzkfpcsharp", "costura.libzkfpcsharp.dll.compressed");
		assemblyNames.Add("log4net", "costura.log4net.dll.compressed");
		assemblyNames.Add("microsoft.practices.enterpriselibrary.common", "costura.microsoft.practices.enterpriselibrary.common.dll.compressed");
		assemblyNames.Add("microsoft.practices.enterpriselibrary.data", "costura.microsoft.practices.enterpriselibrary.data.dll.compressed");
		assemblyNames.Add("newtonsoft.json", "costura.newtonsoft.json.dll.compressed");
		assemblyNames.Add("pduconverter", "costura.pduconverter.dll.compressed");
		assemblyNames.Add("qrcoder", "costura.qrcoder.dll.compressed");
		assemblyNames.Add("system.buffers", "costura.system.buffers.dll.compressed");
		assemblyNames.Add("system.configuration.configurationmanager", "costura.system.configuration.configurationmanager.dll.compressed");
		assemblyNames.Add("system.data.sqlserverce", "costura.system.data.sqlserverce.dll.compressed");
		assemblyNames.Add("system.memory", "costura.system.memory.dll.compressed");
		assemblyNames.Add("system.net.http.formatting", "costura.system.net.http.formatting.dll.compressed");
		assemblyNames.Add("system.numerics.vectors", "costura.system.numerics.vectors.dll.compressed");
		assemblyNames.Add("system.runtime.compilerservices.unsafe", "costura.system.runtime.compilerservices.unsafe.dll.compressed");
		assemblyNames.Add("system.security.accesscontrol", "costura.system.security.accesscontrol.dll.compressed");
		assemblyNames.Add("system.security.permissions", "costura.system.security.permissions.dll.compressed");
		assemblyNames.Add("system.security.principal.windows", "costura.system.security.principal.windows.dll.compressed");
		assemblyNames.Add("system.threading.tasks.extensions", "costura.system.threading.tasks.extensions.dll.compressed");
		assemblyNames.Add("webcam_capture", "costura.webcam_capture.dll.compressed");
		assemblyNames.Add("whatsappapi", "costura.whatsappapi.dll.compressed");
		assemblyNames.Add("winformanimation", "costura.winformanimation.dll.compressed");
		assemblyNames.Add("zen.barcode.core", "costura.zen.barcode.core.dll.compressed");
	}

	public static void Attach(bool subscribe)
	{
		if (Interlocked.Exchange(ref isAttached, 1) == 1 || !subscribe)
		{
			return;
		}
		AppDomain currentDomain = AppDomain.CurrentDomain;
		currentDomain.AssemblyResolve += delegate(object sender, ResolveEventArgs e)
		{
			string ResolveAssemblyassemblyNameAsString = e.Name;
			AssemblyName ResolveAssemblyassemblyName = new AssemblyName(ResolveAssemblyassemblyNameAsString);
			lock (nullCacheLock)
			{
				if (nullCache.ContainsKey(ResolveAssemblyassemblyNameAsString))
				{
					return (Assembly)null;
				}
			}
			Assembly ResolveAssemblyassembly = ReadExistingAssembly(ResolveAssemblyassemblyName);
			if ((object)ResolveAssemblyassembly != null)
			{
				return ResolveAssemblyassembly;
			}
			ResolveAssemblyassembly = ReadFromEmbeddedResources(assemblyNames, symbolNames, ResolveAssemblyassemblyName);
			if ((object)ResolveAssemblyassembly == null)
			{
				lock (nullCacheLock)
				{
					nullCache[ResolveAssemblyassemblyNameAsString] = true;
				}
				if ((ResolveAssemblyassemblyName.Flags & AssemblyNameFlags.Retargetable) != 0)
				{
					ResolveAssemblyassembly = Assembly.Load(ResolveAssemblyassemblyName);
				}
			}
			return ResolveAssemblyassembly;
		};
	}
}

}
