path_login = r"d:\CShapEccorErp\src\LoginNew.cs"
with open(path_login, "r", encoding="utf-8", errors="ignore") as f:
    code = f.read()

code = code.replace("global::Architecture.Common.CommonServices.GetEncryptedUniqueDeviceId()", "global::Architecture.WinApp.Common.CommonServices.GetEncryptedUniqueDeviceId()")
with open(path_login, "w", encoding="utf-8") as f:
    f.write(code)

path_cs = r"d:\CShapEccorErp\src\CommonServices.cs"
with open(path_cs, "r", encoding="utf-8", errors="ignore") as f:
    c = f.read()

c = c.replace("global::Architecture.Common.Encryption.EncryptString(motherboard + \"-\" + cpu + \"-\" + disk)", "global::Architecture.Common.AESEncryption.EncryptText(motherboard + \"-\" + cpu + \"-\" + disk, \"4693024482831831\")")
with open(path_cs, "w", encoding="utf-8") as f:
    f.write(c)

print("Updated Encryption to AESEncryption and CommonServices namespace call in LoginNew!")
