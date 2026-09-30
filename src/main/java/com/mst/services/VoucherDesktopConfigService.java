package com.mst.services;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import com.mst.models.ConfigrationsAllocation;
import com.mst.security.CurrentUserContext;
import com.mst.serviceInterface.IConfigurationService;

/**
 * The Configuration-screen settings the desktop Accounts voucher forms read, and the three
 * lookups that depend on them. One place, so every voucher page reads the same keys the same way.
 *
 * Sources (recovered_source/ECCOUNTBOOKERP):
 *   PaymentVoucherNew.cs      DefaultConfigurations():3294, DetailAccountFill():2097, CheqNoFill():2205,
 *                             BuildRemarks():1102
 *   ReceiptsVoucherNew.cs     :1091-1092, DetailAccountFill():1785, Insert():668-735
 *   ContraVoucher.cs          :1318, :1838-1891, Insert():535-620
 *   VoucherEntry.cs           DebitAccountTitleFill():496, :1281-1282
 *   VouchersWithTax/JournalVoucher.cs   DebitAccountTitleFill():614, :1508
 *   VouchersWithTax/ExpenseVoucherNew.cs  :505, :523-525, account fill :824, Insert() :2369/:2389/:2536
 *
 * Config values are read exactly as each form reads them: Conversion.ToBool (true for "True"/"1"),
 * except ExpenseAccountAllowOnPaymentVoucher, which PaymentVoucherNew compares with
 * ConfigKey.ToLower() == "true" (so "1" is false there).
 */
@Service
public class VoucherDesktopConfigService {

	/** ERPFeatures seed: 6 = MultiCurrency (MultiCurrencyFeatureVisibilty = GetERPFeatureById(6)). */
	private static final int ERP_FEATURE_MULTI_CURRENCY = 6;
	/** ERPFeatures seed: 17 = Branch feature (BranchFeature = GetERPFeatureById(17)). */
	private static final int ERP_FEATURE_BRANCH = 17;

	@Autowired private JdbcTemplate jdbcTemplate;
	@Autowired private CurrentUserContext currentUserContext;
	@Autowired private IConfigurationService configurationService;

	// ------------------------------------------------------------------ config reads

	public Map<String, ConfigrationsAllocation> configMap() {
		Map<String, ConfigrationsAllocation> m = configurationService.getHistoryMap();
		return m == null ? new LinkedHashMap<>() : m;
	}

	public static String key(Map<String, ConfigrationsAllocation> m, String description) {
		ConfigrationsAllocation c = m.get(description);
		return c == null ? null : c.getConfigKey();
	}

	/** Conversion.ToBool: "True"/"true"/"1" are true; null, "" and anything else false. */
	public static boolean toBool(String v) {
		if (v == null) return false;
		String t = v.trim();
		return t.equalsIgnoreCase("true") || t.equals("1");
	}

	/** Conversion.ToDouble: unparseable or empty is 0. */
	public static double toDouble(String v) {
		if (v == null) return 0.0;
		try { return Double.parseDouble(v.trim()); } catch (NumberFormatException e) { return 0.0; }
	}

	/** Active ERPConfigurations feature ids of the session company (USP_GetERPFeaturesByCompanyId). */
	public List<Integer> erpFeatureIds() {
		List<Integer> ids = new ArrayList<>();
		for (Map<String, Object> r : jdbcTemplate.queryForList(
				"EXEC USP_GetERPFeaturesByCompanyId @OrganizationId=?, @CompanyId=?",
				currentUserContext.currentOrganizationId(), currentUserContext.currentCompanyId())) {
			ids.add(toInt(ci(r, "Id")));
		}
		return ids;
	}

	public boolean erpFeature(int featureId) {
		List<Map<String, Object>> rows = jdbcTemplate.queryForList(
				"EXEC USP_GetERPFeaturesByCompanyId @OrganizationId=?, @CompanyId=?",
				currentUserContext.currentOrganizationId(), currentUserContext.currentCompanyId());
		for (Map<String, Object> r : rows) {
			Object id = ci(r, "Id");
			if (id instanceof Number && ((Number) id).intValue() == featureId) return true;
		}
		return false;
	}

	/**
	 * Every flag the voucher pages need, as the desktop forms compute them on Load /
	 * DefaultConfigurations(). The page gets values, never the raw table.
	 */
	public Map<String, Object> voucherFlags() {
		Map<String, ConfigrationsAllocation> m = configMap();
		Map<String, Object> f = new LinkedHashMap<>();
		// PaymentVoucherNew / ContraVoucher - DefaultConfigurations()
		f.put("autoRemarksForPaymentThroughBank", toBool(key(m, "AutoRemarksForPaymentThroughBank")));
		f.put("autoRemarksIncludeHeaderRemarks", toBool(key(m, "AutoRemarksIncludeHeaderRemarks")));
		f.put("autoRemarksIncludeChequeDateAndNumber", toBool(key(m, "AutoRemarksIncludeChequeDateAndNumber")));
		f.put("autoRemarksIncludeChequeNumber", toBool(key(m, "AutoRemarksIncludeChequeNumber")));
		f.put("autoRemarksIncludePayeeTitle", toBool(key(m, "AutoRemarksIncludePayeeTitle")));
		f.put("autoRemarksIncludeDetailRemarks", toBool(key(m, "AutoRemarksIncludeDetailRemarks")));
		f.put("chequeBookEnabled", toBool(key(m, "CheqBook Enabled")));
		f.put("chequeNoCompulsoryOnBpv", toBool(key(m, "ChequeNoCompulsoryOnBpv")));
		f.put("chequePostingSerialWise", toBool(key(m, "Cheque Posting Serial Wise")));
		f.put("chequePrintingEnable", toBool(key(m, "ChequePrintingEnable")));
		String exp = key(m, "ExpenseAccountAllowOnPaymentVoucher");
		f.put("expenseAccountAllowOnPaymentVoucher", exp != null && exp.toLowerCase().equals("true"));
		// ReceiptsVoucherNew :1091
		f.put("autoRemarksForReceiptsThroughBank", toBool(key(m, "AutoRemarksForReceiptsThroughBank")));
		// Every voucher form
		f.put("inventoryRelatedAccountsShowInVouchers", toBool(key(m, "InventoryRelatedAccountsShowInVouchers")));
		// VoucherEntry :1282
		f.put("allowExportPartiesOnJV", toBool(key(m, "AllowExportPartiesOnJV")));
		// ExpenseVoucherNew :524-525
		f.put("amountLimitForExpenseVoucher", toDouble(key(m, "AmountLimitForExpenseVoucher")));
		String applyActual = key(m, "Apply On Actual Expenses");
		f.put("applyOnActualExpenses", applyActual != null && applyActual.trim().equalsIgnoreCase("true"));
		f.put("monthlyBudget", key(m, "MonthlyBudget"));
		f.put("multiCurrencyFeature", erpFeature(ERP_FEATURE_MULTI_CURRENCY));
		f.put("branchFeature", erpFeature(ERP_FEATURE_BRANCH));
		return f;
	}

	// ------------------------------------------------------------------ detail-account lists

	/**
	 * The account list each form binds to its detail-account combo, rebuilt from the same
	 * USP_GETAllAccountsFromCustomGroups rows (clsGlobalVariables.AllAccountsWithCustomGroupId) and
	 * filtered exactly as DatatableHelper.GetAccountsFromGlobalByTypeIds does:
	 * include types -> exclude types -> exclude PLNoteIds -> dedup by ChartOfAccountId.
	 *
	 * @param form payment | receipt | journal | journalEntry | expense
	 */
	public List<Map<String, Object>> detailAccounts(String form) {
		Map<String, ConfigrationsAllocation> m = configMap();
		boolean inv = toBool(key(m, "InventoryRelatedAccountsShowInVouchers"));
		boolean mc = erpFeature(ERP_FEATURE_MULTI_CURRENCY);
		int t22 = mc ? 0 : 22;
		int[] with = null, without = null, withoutPlNote = inv ? null : new int[] { 2 };

		switch (form == null ? "" : form) {
			case "payment": { // PaymentVoucherNew.DetailAccountFill():2097
				String exp = key(m, "ExpenseAccountAllowOnPaymentVoucher");
				boolean allowExp = exp != null && exp.toLowerCase().equals("true");
				if (allowExp) {
					without = inv ? new int[] { 2, 15, t22 } : new int[] { 2, 4, 12, 15, t22 };
				} else {
					without = inv ? new int[] { 2, 11, 12, 13, 14, 15, 20, 21, t22, 23 }
								  : new int[] { 2, 4, 12, 11, 13, 14, 15, 20, 21, t22, 23 };
				}
				break;
			}
			case "receipt": // ReceiptsVoucherNew.DetailAccountFill():1785
				without = inv ? new int[] { 2, 15, t22 } : new int[] { 2, 4, 12, 15, t22 };
				break;
			case "journal": // VouchersWithTax/JournalVoucher.DebitAccountTitleFill():614
				without = inv ? new int[] { 0 } : new int[] { 4, 12 };
				break;
			case "journalEntry": { // VoucherEntry.DebitAccountTitleFill():496
				boolean exportJv = toBool(key(m, "AllowExportPartiesOnJV"));
				int x = (!mc && !exportJv) ? 22 : 0;
				without = inv ? new int[] { x } : new int[] { 4, 12, x };
				break;
			}
			case "expense": // VouchersWithTax/ExpenseVoucherNew account fill :824 (include list, no PL note filter)
				with = inv ? new int[] { 11, 12, 13, 14, 20, 21, 22, 23 } : new int[] { 11, 13, 14, 20, 21, 22, 23 };
				withoutPlNote = null;
				break;
			default:
				throw new IllegalArgumentException("Unknown voucher form: " + form);
		}
		return filterGlobalAccounts(with, without, withoutPlNote);
	}

	private List<Map<String, Object>> filterGlobalAccounts(int[] with, int[] without, int[] withoutPlNote) {
		List<Map<String, Object>> rows = jdbcTemplate.queryForList(
				"EXEC USP_GETAllAccountsFromCustomGroups @OrganizationId=?, @CompanyId=?",
				currentUserContext.currentOrganizationId(), currentUserContext.currentCompanyId());
		Set<Integer> w = set(with), wo = set(without), pl = set(withoutPlNote);
		Set<Integer> seen = new HashSet<>();
		List<Map<String, Object>> out = new ArrayList<>();
		for (Map<String, Object> r : rows) {
			int type = toInt(ci(r, "AccountTypeId"));
			if (!w.isEmpty() && !w.contains(type)) continue;
			if (!wo.isEmpty() && wo.contains(type)) continue;
			if (!pl.isEmpty() && pl.contains(toInt(ci(r, "PLNoteId")))) continue;
			int id = toInt(ci(r, "ChartOfAccountId"));
			if (!seen.add(id)) continue;
			Map<String, Object> o = new LinkedHashMap<>();
			o.put("id", id);
			o.put("accountTitle", ci(r, "AccountTitle"));
			o.put("accountCode", ci(r, "AccountCode"));
			o.put("parentAccountTitle", ci(r, "ParentAccountTitle"));
			o.put("accountClass", ci(r, "AccountClassName"));
			o.put("className", ci(r, "AccountClassName"));
			o.put("accountTypeId", type);
			o.put("currencyId", ci(r, "CurrencyId"));
			o.put("currencyCode", ci(r, "CurrencyCode"));
			out.add(o);
		}
		return out;
	}

	// ------------------------------------------------------------------ cheque book

	/**
	 * CheqBookHeader.OutstandingCheqNo -> SP_CheqBookHeader_GetAllMethod @MethodType='OutstandingCheqNo'.
	 * @RecId only when editing (obj.Id = RecId != 0); @Id (ActionId) is never set by the voucher forms.
	 */
	public List<Map<String, Object>> outstandingCheques(int bankId, int recId) {
		StringBuilder sql = new StringBuilder(
				"EXEC SP_CheqBookHeader_GetAllMethod @OrganizationId=?, @CompanyId=?, @BankId=?");
		List<Object> args = new ArrayList<>(Arrays.asList(
				currentUserContext.currentOrganizationId(), currentUserContext.currentCompanyId(), bankId));
		if (recId != 0) { sql.append(", @RecId=?"); args.add(recId); }
		sql.append(", @MethodType='OutstandingCheqNo'");
		List<Map<String, Object>> out = new ArrayList<>();
		for (Map<String, Object> r : jdbcTemplate.queryForList(sql.toString(), args.toArray())) {
			Map<String, Object> o = new LinkedHashMap<>();
			o.put("id", ci(r, "Id"));
			o.put("cheqNo", ci(r, "CheqNo"));
			out.add(o);
		}
		return out;
	}

	// ------------------------------------------------------------------ helpers

	private static Set<Integer> set(int[] a) {
		Set<Integer> s = new HashSet<>();
		if (a != null) for (int i : a) s.add(i);
		return s;
	}

	static int toInt(Object v) {
		if (v instanceof Number) return ((Number) v).intValue();
		if (v == null) return 0;
		try { return Integer.parseInt(v.toString().trim()); } catch (NumberFormatException e) { return 0; }
	}

	static Object ci(Map<String, Object> row, String name) {
		if (row == null) return null;
		if (row.containsKey(name)) return row.get(name);
		for (Map.Entry<String, Object> e : row.entrySet())
			if (e.getKey() != null && e.getKey().equalsIgnoreCase(name)) return e.getValue();
		return null;
	}
}
