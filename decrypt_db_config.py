import base64
from Crypto.Cipher import AES
import xml.etree.ElementTree as ET

# Key and IV from Architecture.Model.Encryption
KEY = b"4693024482831831"
IV  = b"4693024482831831"

def unpad(s):
    return s[:-ord(s[len(s)-1:])]

def decrypt_string(encrypted_b64):
    try:
        cipher = AES.new(KEY, AES.MODE_CBC, IV)
        encrypted_bytes = base64.b64decode(encrypted_b64)
        decrypted = cipher.decrypt(encrypted_bytes)
        # PKCS7 unpad
        pad_len = decrypted[-1]
        return decrypted[:-pad_len].decode('utf-8')
    except Exception as e:
        return f"[Decryption Error: {e}]"

# Let's test with the values from ECCOUNTBOOKERP.exe.config
tpss_db = "31shbmRUY1uONbSEm6Ae9AKjZy3p8Iuzoa17DpMwPE8uXTZkYX36xEraosDrCx+3xGUc0PUXlxroFgdYlbwSQzSO4vKkvpXCtQi16SLkF3qJCTuL+n/v3n+1TMZEp9cdAouaURZy/xpU1Nmo8Ke9a0QLz6OOqhOy6DQmEJrrKeE="
vps_name = "4VctcCutg6yDpje25dkF9A=="
vps_user = "v/DL1vHFEdEiDrmrRGNN9g=="
vps_pwd  = "Xe0KYuiObJCdxo82JNav2g=="

print("=== DECRYPTED DATABASE CONNECTION DETAILS ===")
print("TPSSDb Connection String:", decrypt_string(tpss_db))
print("VPS Server Name / IP:", decrypt_string(vps_name))
print("VPS User:", decrypt_string(vps_user))
print("VPS Password:", decrypt_string(vps_pwd))
