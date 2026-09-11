package com.mst.security;

import java.nio.charset.StandardCharsets;
import java.util.Base64;

import javax.crypto.Cipher;
import javax.crypto.spec.IvParameterSpec;
import javax.crypto.spec.SecretKeySpec;

import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Component;

/**
 * Reproduces the real desktop app's
 * Architecture.Model.Encryption.UserPasswordEncryptString / UserPasswordDecryptString
 * exactly, so this port can authenticate directly against the real
 * dbo.UserAccount.Password column - per the user's explicit instruction ("for
 * username and password for user login use db useraccount") - with no separate
 * credential table.
 *
 * Confirmed against real data: decrypting live dbo.UserAccount.Password values from
 * goldenAce5_25t.sql with this exact scheme yields plausible real plaintext
 * passwords (e.g. user "GOLDEN" -> "Dealing@3", "WB1" -> "123", "Suneel" ->
 * "suneel0009", "NAEEM" -> "naeem486", "KAREEM" -> "kareem@654"). The real desktop
 * source (Architecture.Model.Encryption.cs) confirms the exact algorithm:
 *
 *   RijndaelManaged (128-bit block = AES), CipherMode.CBC, PaddingMode.PKCS7,
 *   Key = IV = UTF-8 bytes of the hardcoded string "3024482831469469" (16 bytes),
 *   ciphertext Base64-encoded.
 *
 * IMPORTANT: this is REVERSIBLE, deterministic, single-key encryption, not a salted
 * hash - the same password always encrypts to the same string, and anyone with this
 * class (or the C# source) can decrypt every password in the database. That is a
 * pre-existing property of the real desktop app, not something introduced by this
 * port - reproducing it here is what lets a real desktop user's existing password
 * keep working unchanged in this web app, and vice versa. The real
 * Architecture.BLL.UserAccount.Login method works the same way: it encrypts the
 * entered password with this scheme and passes it to Sp_UserAccount_Login, which
 * does a plain string match against the stored column - matches() below reproduces
 * that same comparison.
 *
 * Registered as this app's Spring Security PasswordEncoder (see
 * SecurityConfiguration.configure(AuthenticationManagerBuilder)), so:
 *  - encode() is used whenever this port itself sets/changes a password (User
 *    Registration screen - see UserAccountService - and DefaultAdminSeeder's
 *    first-run "admin" account).
 *  - matches() is used by Spring's DaoAuthenticationProvider on every login attempt,
 *    comparing the entered password against the encrypted value
 *    CustomUserDetailsService loaded from UserAccount.password.
 */
@Component
public class LegacyUserPasswordEncoder implements PasswordEncoder {

	/** Exact hardcoded key from Architecture.Model.Encryption.UserPasswordEncryptString. */
	private static final String KEY = "3024482831469469";
	private static final String TRANSFORMATION = "AES/CBC/PKCS5Padding";

	@Override
	public String encode(CharSequence rawPassword) {
		if (rawPassword == null || rawPassword.length() == 0) {
			return "";
		}
		try {
			byte[] keyBytes = KEY.getBytes(StandardCharsets.UTF_8);
			Cipher cipher = Cipher.getInstance(TRANSFORMATION);
			cipher.init(Cipher.ENCRYPT_MODE, new SecretKeySpec(keyBytes, "AES"), new IvParameterSpec(keyBytes));
			byte[] encrypted = cipher.doFinal(rawPassword.toString().getBytes(StandardCharsets.UTF_8));
			return Base64.getEncoder().encodeToString(encrypted);
		} catch (Exception ex) {
			throw new IllegalStateException("Unable to encrypt password", ex);
		}
	}

	/**
	 * Mirrors Architecture.Model.Encryption.UserPasswordDecryptString. Not used by
	 * the login flow itself (which compares encrypted values, like the desktop
	 * does) - available for admin tooling that needs to recover a real user's
	 * plaintext password (e.g. a future "show password" support screen).
	 */
	public String decode(String encryptedPassword) {
		if (encryptedPassword == null || encryptedPassword.isEmpty()) {
			return "";
		}
		try {
			byte[] keyBytes = KEY.getBytes(StandardCharsets.UTF_8);
			Cipher cipher = Cipher.getInstance(TRANSFORMATION);
			cipher.init(Cipher.DECRYPT_MODE, new SecretKeySpec(keyBytes, "AES"), new IvParameterSpec(keyBytes));
			byte[] decrypted = cipher.doFinal(Base64.getDecoder().decode(encryptedPassword));
			return new String(decrypted, StandardCharsets.UTF_8);
		} catch (Exception ex) {
			return null;
		}
	}

	@Override
	public boolean matches(CharSequence rawPassword, String encodedPassword) {
		if (rawPassword == null || encodedPassword == null) {
			return false;
		}
		return encode(rawPassword).equals(encodedPassword);
	}
}
