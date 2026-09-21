package com.mst.security.desktop;

import javax.crypto.Cipher;
import javax.crypto.SecretKeyFactory;
import javax.crypto.spec.IvParameterSpec;
import javax.crypto.spec.PBEKeySpec;
import javax.crypto.spec.SecretKeySpec;
import java.nio.charset.StandardCharsets;
import java.util.Arrays;
import java.util.Base64;

/**
 * Architecture.Common.LicenseKey.ReturnKey (common 0006), ported.
 *
 * <pre>
 *   password = "AMIR2SPSCS44602"
 *   salt     = { 73,118,97,110,32,77,101,100,118,101,100,101,118 }   // "Ivan Medvedev"
 *   Rfc2898DeriveBytes(password, salt)                                // PBKDF2-HMAC-SHA1, 1000 iterations
 *   aes.Key = GetBytes(32);  aes.IV = GetBytes(16);                   // the SAME stream, continued
 *   AES decrypt (CBC + PKCS7, the .NET defaults), then Encoding.Unicode -&gt; UTF-16LE
 * </pre>
 *
 * The subtlety worth naming: .NET's {@code GetBytes} is a <b>stream</b>. {@code GetBytes(32)}
 * followed by {@code GetBytes(16)} is bytes 0-31 then bytes 32-47 of one derivation, not two
 * separate derivations of 32 and 16 bytes. Deriving 48 bytes once and splitting them is the only
 * way to get the same key and IV; deriving twice produces a key that decrypts nothing.
 *
 * ---------------------------------------------------------------------------------------------
 * ABOUT THE EMBEDDED PASSWORD
 * ---------------------------------------------------------------------------------------------
 * It is carried over verbatim because it is the desktop's own value and the licence strings in
 * the database cannot be read without it. It is an obfuscation key for this application's own
 * licence text, shipped inside every copy of the desktop DLL - not a credential to any account or
 * service, and copying it here does not widen its exposure by one reader.
 *
 * That reasoning does NOT extend to the OTP mail account's app password, which is a live
 * credential to a third-party mailbox; see DesktopUserAccountBll.setAuthenticationCode.
 */
public final class DesktopLicenseKey {

    private DesktopLicenseKey() { }

    private static final String PASSWORD = "AMIR2SPSCS44602";
    private static final byte[] SALT = {
            73, 118, 97, 110, 32, 77, 101, 100, 118, 101, 100, 101, 118
    };
    /** Rfc2898DeriveBytes' default on .NET Framework. */
    private static final int ITERATIONS = 1000;

    /** @throws IllegalStateException with the C# message, "License Key Not Match..." */
    public static String returnKey(String key) {
        try {
            byte[] cipherText = Base64.getDecoder().decode(key);

            SecretKeyFactory factory = SecretKeyFactory.getInstance("PBKDF2WithHmacSHA1");
            byte[] derived = factory.generateSecret(
                    new PBEKeySpec(PASSWORD.toCharArray(), SALT, ITERATIONS, 48 * 8)).getEncoded();

            byte[] aesKey = Arrays.copyOfRange(derived, 0, 32);
            byte[] iv     = Arrays.copyOfRange(derived, 32, 48);

            Cipher cipher = Cipher.getInstance("AES/CBC/PKCS5Padding");
            cipher.init(Cipher.DECRYPT_MODE, new SecretKeySpec(aesKey, "AES"), new IvParameterSpec(iv));
            byte[] plain = cipher.doFinal(cipherText);

            /* Encoding.Unicode is UTF-16 LITTLE endian. UTF_16 would read a byte-order mark that
               is not there and get every character wrong. */
            return new String(plain, StandardCharsets.UTF_16LE);
        } catch (Exception e) {
            throw new IllegalStateException("License Key Not Match...", e);
        }
    }
}
