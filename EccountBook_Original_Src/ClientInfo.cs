using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;

public class ClientInfo
{
	public static string GetMachineName()
	{
		return Environment.MachineName;
	}

	public static string GetIPAddress()
	{
		NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
		foreach (NetworkInterface ni in allNetworkInterfaces)
		{
			if (ni.OperationalStatus != OperationalStatus.Up || (ni.NetworkInterfaceType != NetworkInterfaceType.Wireless80211 && ni.NetworkInterfaceType != NetworkInterfaceType.Ethernet))
			{
				continue;
			}
			string desc = ni.Description.ToLower();
			if (desc.Contains("tailscale") || desc.Contains("virtual") || desc.Contains("vmware") || desc.Contains("hyper-v") || desc.Contains("loopback"))
			{
				continue;
			}
			foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
			{
				if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
				{
					string address = ip.Address.ToString();
					if (!address.StartsWith("169.254."))
					{
						return address;
					}
				}
			}
		}
		return string.Empty;
	}

	public static string GetMacAddress()
	{
		NetworkInterface adapter = GetActiveAdapter();
		if (adapter == null)
		{
			return string.Empty;
		}
		return string.Join(":", from b in adapter.GetPhysicalAddress().GetAddressBytes()
			select b.ToString("X2"));
	}

	private static NetworkInterface GetActiveAdapter()
	{
		return (from n in NetworkInterface.GetAllNetworkInterfaces()
			where n.OperationalStatus == OperationalStatus.Up && (n.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || n.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
			select n).Where(delegate(NetworkInterface n)
		{
			string text = (n.Description + " " + n.Name).ToLower();
			return !text.Contains("tailscale") && !text.Contains("vmware") && !text.Contains("virtual") && !text.Contains("hyper-v") && !text.Contains("loopback") && !text.Contains("bluetooth");
		}).FirstOrDefault();
	}
}
