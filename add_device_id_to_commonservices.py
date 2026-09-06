path_cs = r"d:\CShapEccorErp\src\CommonServices.cs"
with open(path_cs, "r", encoding="utf-8", errors="ignore") as f:
    code = f.read()

# Add GetEncryptedUniqueDeviceId implementation before closing brace of class
device_id_code = """
	public static string GetEncryptedUniqueDeviceId()
	{
		try
		{
			string motherboard = GetMotherboardSerial();
			string cpu = GetCpuId();
			string disk = GetPrimaryDiskSerial();
			return global::Architecture.Common.Encryption.EncryptString(motherboard + "-" + cpu + "-" + disk);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static string GetMacAddress()
	{
		return "00:00:00:00:00:00";
	}

	private static string GetMotherboardSerial()
	{
		return "UNKNOWN_MB";
	}

	private static string GetCpuId()
	{
		return "UNKNOWN_CPU";
	}

	private static string GetPrimaryDiskSerial()
	{
		return "UNKNOWN_DISK";
	}
}
}
"""

code = code.rstrip()
if code.endswith("}\n}\n") or code.endswith("}\n}"):
    # Replace last closing braces with device_id_code
    code = code[:code.rfind("}")]
    code = code[:code.rfind("}")]
    code += device_id_code

with open(path_cs, "w", encoding="utf-8") as f:
    f.write(code)

print("Added GetEncryptedUniqueDeviceId to CommonServices.cs!")
