package com.mst.models;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.Table;
import javax.persistence.UniqueConstraint;

import lombok.Data;

/**
 * Master list of screens that can be rights-controlled - the lookup table
 * {@code MstUserRight} joins against on this port's side (its ScreenId FK;
 * ScreenName/ScreenAlias/ModuleDescription/TargetUrl/MenuControllName are the
 * columns MstUserRight pulls from this table via that join - see UserRight.java).
 *
 * NOTE: the real desktop's own screen master is dbo.ScreenDefinition (confirmed via
 * goldenAce5_25t.sql - Id, ScreenName, ScreenAlias, ModuleId (numeric FK into
 * AppModules), IsCreateSMSTemplate, ReferencedTableName/Id, MenuControllName,
 * TargetUrl, IsActive, SortNo, HasSpecialRight, AppId, CompanyIds), and it already
 * has ~900 real rows describing every WinForms screen across the whole desktop app.
 * This entity is deliberately NOT mapped onto that table: the real desktop's
 * authorization model is a numeric (UserId, ScreenId, RightId)-&gt;Value grid keyed
 * off a separate ScreenRights table (one row per (screen, right-name) combination,
 * with scattered large Ids - RightId isn't a small fixed enum), which doesn't carry
 * the string "authority code" this port's Spring-Security-based menu gating
 * (fixed_sidebar.html's hasAuthority(...) checks) needs. Rather than force this
 * port's authorization onto that shape (or worse, insert synthetic rows into the
 * real, already-populated ScreenDefinition table), this is its own small,
 * additively-new table (MstScreen) that exists alongside the real schema - see
 * the one-time CREATE TABLE script provided for it, since ddl-auto=none means
 * Hibernate won't create it automatically.
 *
 * The desktop's real Screens master seeds one row per WinForms screen across ~80
 * modules (~900 rows). Reproducing that whole tree here is out of scope; ScreenSeeder
 * seeds one row per screen this Java port has actually built so far, and grows as
 * more screens are ported.
 *
 * realScreenDefinitionId (added after the initial rollout): when set, this is the
 * real dbo.ScreenDefinition.Id this screen corresponds to, and
 * CustomUserDetailsService gates it through the REAL rights chain instead - real
 * dbo.CompanyRights (tenant-level: is this screen even enabled for the user's
 * company) and real dbo.tblUserRights/dbo.ScreenRights (has THIS user actually been
 * granted the "View" right on it) - exactly like the desktop, with no
 * hardcoded role-based bypass (see RealUserRight's Javadoc for why that matters).
 * Screens not yet linked to a confirmed real ScreenDefinition.Id keep using this
 * port's own MstUserRight grid as a fallback, until they're mapped too.
 */
@Entity
@Table(name = "MstScreen", uniqueConstraints = @UniqueConstraint(columnNames = "TargetUrl"))
@Data
public class Screen {

	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "Id")
	private Integer id;

	@Column(name = "ScreenName", nullable = false, length = 150)
	private String screenName;

	@Column(name = "ScreenAlias", length = 60)
	private String screenAlias;

	/** This port's own module/category grouping this screen renders under (e.g. "Accounts"). */
	@Column(name = "ModuleDescription", length = 100)
	private String moduleDescription;

	@Column(name = "TargetUrl", nullable = false, length = 200)
	private String targetUrl;

	@Column(name = "MenuControllName", length = 100)
	private String menuControllName;

	/**
	 * The exact Spring Security authority string fixed_sidebar.html's
	 * hasAuthority(...) check uses for this screen's own menu item (e.g.
	 * "INVENTORY_BRANDS") - see CustomUserDetailsService.
	 */
	@Column(name = "AuthorityCode", length = 60)
	private String authorityCode;

	/** The authority that gates this screen's whole parent section (e.g. "INVENTORY"). */
	@Column(name = "SectionAuthorityCode", length = 60)
	private String sectionAuthorityCode;

	/**
	 * The real dbo.ScreenDefinition.Id this screen corresponds to, when confirmed -
	 * null means "not yet mapped", in which case CustomUserDetailsService falls back
	 * to this port's own MstUserRight grid for it. See the class Javadoc above.
	 */
	@Column(name = "RealScreenDefinitionId")
	private Integer realScreenDefinitionId;

	/**
	 * The real dbo.AppModules.Id this screen's Accounts sub-module is, per
	 * goldenAce5_25t.sql (1=Account Definition, 2=Accounts Transaction,
	 * 3=Account Reports, 2032=Banking Managment - real spelling, missing the second
	 * "e"). Used only to group this port's own built screens into the same 4 tiles
	 * the real desktop's Accounts home screen shows (see AccountsHomeController) -
	 * null for screens outside the Accounts module (Inventory, User Management, ...).
	 */
	@Column(name = "RealModuleId")
	private Integer realModuleId;
}
