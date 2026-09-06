package com.mst.security;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.CommandLineRunner;
import org.springframework.core.annotation.Order;
import org.springframework.stereotype.Component;

import com.mst.models.Screen;
import com.mst.repositories.IScreenRepository;

/**
 * Seeds the Screen master (see Screen.java) with one row per screen this Java port
 * has actually built so far, so User Rights has something to assign. authorityCode /
 * sectionAuthorityCode are the exact strings fixed_sidebar.html's hasAuthority(...)
 * checks already use for that menu item / its parent section - keep them in sync
 * with the sidebar. Add a line here whenever a new screen/controller is ported.
 *
 * realScreenDefinitionId links a row to the real dbo.ScreenDefinition.Id it
 * corresponds to, confirmed via goldenAce5_25t.sql - CustomUserDetailsService uses
 * this to gate the screen through the REAL CompanyRights/tblUserRights/ScreenRights
 * chain instead of this port's own MstUserRight fallback (see Screen.java's
 * Javadoc). Only screens whose real Id has actually been confirmed get one here -
 * the rest stay null (fallback) until mapped.
 *
 * realModuleId is the real dbo.AppModules.Id for the screen's Accounts sub-module
 * (1=Account Definition, 2=Accounts Transaction, 3=Account Reports,
 * 2032=Banking Managment) - AccountsHomeController groups this port's own built
 * screens by it to render the same 4 tiles the real desktop's Accounts home screen
 * shows. Null for screens outside the Accounts module.
 *
 * seed() UPSERTS by TargetUrl rather than skipping existing rows outright, so this
 * can also backfill new columns onto a row that was already seeded before they
 * existed, without ever touching a row's real desktop counterpart (this table is
 * additively-new, not the real ScreenDefinition).
 */
@Component
@Order(1)
public class ScreenSeeder implements CommandLineRunner {

	@Autowired
	private IScreenRepository screenRepository;

	@Override
	public void run(String... args) {
		// Account Definition (real AppModules.Id=1) - real ScreenDefinition.Id confirmed via goldenAce5_25t.sql.
		seed("Chart Of Account Definition", "Accounts", "/accounts/chart_of_accounts", "CHART_OF_ACCOUNT", "ACCOUNTS", 9, 1);
		seed("Account Custom Group", "Accounts", "/accounts/custom_group", "ACCOUNT_CUSTOM_GROUP", "ACCOUNTS", 1, 1);
		seed("Account Opening Balance", "Accounts", "/accounts/opening_balance", "ACCOUNT_OPENING_BALANCE", "ACCOUNTS", 10, 1);
		seed("Cheque Book Registration", "Accounts", "/accounts/cheque_book", "CHEQUE_BOOK_REGISTRATION", "ACCOUNTS", 42, 1);
		seed("Define Supplier / Customer", "Accounts", "/accounts/supplier", "DEFINE_SUPPLIER", "ACCOUNTS", 11, 1);

		// Accounts Transaction (real AppModules.Id=2) - not yet mapped to individual real ScreenDefinition.Ids.
		seed("Cash Payment Voucher", "Accounts", "/accounts/vouchers/cash-payment", "VOUCHER_CASH_PAYMENT", "ACCOUNTS", null, 2);
		seed("Bank Payment Voucher", "Accounts", "/accounts/vouchers/bank-payment", "VOUCHER_BANK_PAYMENT", "ACCOUNTS", null, 2);
		seed("Cash Receipt Voucher", "Accounts", "/accounts/vouchers/cash-receipt", "VOUCHER_CASH_RECEIPT", "ACCOUNTS", null, 2);
		seed("Bank Receipt Voucher", "Accounts", "/accounts/vouchers/bank-receipt", "VOUCHER_BANK_RECEIPT", "ACCOUNTS", null, 2);
		seed("Journal Voucher", "Accounts", "/accounts/vouchers/journal", "VOUCHER_JOURNAL", "ACCOUNTS", null, 2);
		seed("Contra Voucher", "Accounts", "/accounts/vouchers/contra", "VOUCHER_CONTRA", "ACCOUNTS", null, 2);
		seed("Expense Voucher", "Accounts", "/accounts/vouchers/expense", "VOUCHER_EXPENSE", "ACCOUNTS", null, 2);
		seed("Party Receipt Voucher", "Accounts", "/accounts/vouchers/party-receipt", "VOUCHER_PARTY_RECEIPT", "ACCOUNTS", null, 2);
		seed("Party Payment Voucher", "Accounts", "/accounts/vouchers/party-payment", "VOUCHER_PARTY_PAYMENT", "ACCOUNTS", null, 2);

		// Outside the Accounts module - not shown on the Accounts home dashboard.
		seed("Brands", "Inventory", "/inventory/brands", "INVENTORY_BRANDS", "INVENTORY", null, null);
		seed("Item Groups", "Inventory", "/inventory/item_groups", "INVENTORY_ITEM_GROUPS", "INVENTORY", null, null);
		seed("Product Types", "Inventory", "/inventory/product_types", "INVENTORY_PRODUCT_TYPES", "INVENTORY", null, null);
		seed("Warehouses", "Inventory", "/inventory/warehouses", "INVENTORY_WAREHOUSES", "INVENTORY", null, null);
		seed("Racks", "Inventory", "/inventory/racks", "INVENTORY_RACKS", "INVENTORY", null, null);
		seed("Item Categories", "Inventory", "/inventory/item_categories", "INVENTORY_ITEM_CATEGORIES", "INVENTORY", null, null);
		seed("Item Types", "Inventory", "/inventory/item_types", "INVENTORY_ITEM_TYPES", "INVENTORY", null, null);
		seed("Items", "Inventory", "/inventory/items", "INVENTORY_ITEMS", "INVENTORY", null, null);
		seed("User Registration", "User Management", "/user-management/users", "ADD_NEW_USER", "UTILITIES", null, null);
		seed("User Roles", "User Management", "/user-management/roles", "USER_ROLES", "UTILITIES", null, null);
		seed("User Rights", "User Management", "/user-management/rights", "USER_RIGHTS", "UTILITIES", null, null);
	}

	private void seed(String name, String module, String url, String authorityCode, String sectionAuthorityCode,
			Integer realScreenDefinitionId, Integer realModuleId) {
		Screen screen = screenRepository.findByTargetUrl(url);
		if (screen == null) {
			screen = new Screen();
			screen.setTargetUrl(url);
		}
		screen.setScreenName(name);
		screen.setModuleDescription(module);
		screen.setScreenAlias(name);
		screen.setAuthorityCode(authorityCode);
		screen.setSectionAuthorityCode(sectionAuthorityCode);
		if (realScreenDefinitionId != null) {
			screen.setRealScreenDefinitionId(realScreenDefinitionId);
		}
		if (realModuleId != null) {
			screen.setRealModuleId(realModuleId);
		}
		screenRepository.save(screen);
	}
}
