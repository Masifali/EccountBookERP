import base64
from Crypto.Cipher import AES
from Crypto.Util.Padding import pad, unpad

KEY = b"4693024482831831"

def encrypt(plain_text):
    cipher = AES.new(KEY, AES.MODE_CBC, KEY)
    ct_bytes = cipher.encrypt(pad(plain_text.encode('utf-8'), AES.block_size))
    return base64.b64encode(ct_bytes).decode('utf-8')

def decrypt(cipher_text):
    ct = base64.b64decode(cipher_text)
    cipher = AES.new(KEY, AES.MODE_CBC, KEY)
    pt = unpad(cipher.decrypt(ct), AES.block_size)
    return pt.decode('utf-8')

conn_str = r"Data Source=MUHAMMADASIF-PC\SQLEXPRESS;Initial Catalog=GoldenAcedb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;"
encrypted_str = encrypt(conn_str)

print("Original Connection String:")
print(conn_str)
print("\nEncrypted Connection String:")
print(encrypted_str)

print("\nVerification Decryption:")
print(decrypt(encrypted_str))
