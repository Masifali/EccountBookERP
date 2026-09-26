package com.mst.security;

import com.mst.models.RealCompanyRight;
import com.mst.models.RealScreenDefinition;
import com.mst.models.RealScreenRight;
import com.mst.models.RealUserRight;
import com.mst.repositories.IRealCompanyRightRepository;
import com.mst.repositories.IRealScreenDefinitionRepository;
import com.mst.repositories.IRealScreenRightRepository;
import com.mst.repositories.IRealUserRightRepository;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

/**
 * Which screens a user may see, decided the way the desktop decides it - from the real tables,
 * with no table of this port's own.
 *
 * ---------------------------------------------------------------------------------------------
 * THE REAL CHAIN
 * ---------------------------------------------------------------------------------------------
 * <pre>
 *   dbo.ScreenDefinition   one row per WinForms screen (~900)
 *   dbo.CompanyRights      is this screen enabled for the user's company at all
 *   dbo.ScreenRights       one row per (screen, right name) - "View", "Save", "CanView AllRecord", ...
 *   dbo.tblUserRights      (UserId, ScreenId, RightId) -> Value : has THIS user been granted it
 * </pre>
 *
 * A screen is visible when its company right is active AND the user holds its "View" right. That
 * is what CommonServices reads and what the desktop menu obeys. There is no "admin sees
 * everything" shortcut for View - the desktop grants Admin a blanket Save/Update/Delete/Print and
 * CanView AllRecord, but not View, so an Admin with no View grant sees no menu item either.
 *
 * ---------------------------------------------------------------------------------------------
 * NO MstScreen, NO MstUserRight
 * ---------------------------------------------------------------------------------------------
 * The screens this port has built are listed in {@link SidebarScreenCatalog}, in code. This class
 * maps each of them onto a real ScreenDefinition.Id and then asks the real chain. Nothing is read
 * from or written to a table the desktop does not have.
 *
 * ---------------------------------------------------------------------------------------------
 * AN UNRESOLVED SCREEN IS HIDDEN, NOT ASSUMED
 * ---------------------------------------------------------------------------------------------
 * A catalog entry whose real ScreenDefinition.Id is not yet confirmed is matched by name against
 * the real ScreenDefinition - on ScreenName and on ScreenAlias, normalised, with the desktop's
 * "frm" class prefix stripped the same way ScreenRouteIndex strips it. When nothing matches, the
 * screen is <b>not granted</b> and the miss is logged by name.
 *
 * Hiding is the only safe default: granting an unresolved screen would hand out a menu item that
 * no rights row has ever authorised. The log names every miss so they can be confirmed and filled
 * into the catalog, rather than discovered by a user who cannot find a screen.
 */
@Service
public class DesktopScreenRightsService {

    private static final Logger LOG = LoggerFactory.getLogger(DesktopScreenRightsService.class);

    /** dbo.ScreenRights.RightName for the right that makes a menu item appear. */
    private static final String RIGHT_VIEW = "View";

    @Autowired private IRealScreenDefinitionRepository screenDefinitionRepository;
    @Autowired private IRealCompanyRightRepository companyRightRepository;
    @Autowired private IRealScreenRightRepository screenRightRepository;
    @Autowired private IRealUserRightRepository userRightRepository;

    /** catalog entry -> real ScreenDefinition.Id. Reference data, resolved once. */
    private volatile Map<String, Integer> resolvedByTargetUrl;

    /**
     * The real ScreenDefinition.Id behind each catalog entry, keyed by the entry's web route.
     *
     * Built once: ScreenDefinition is reference data that does not change while the app runs, and
     * re-reading ~900 rows on every login was one of the things that made sign-in slow.
     */
    private Map<String, Integer> resolved() {
        Map<String, Integer> cached = resolvedByTargetUrl;
        if (cached != null) return cached;
        synchronized (this) {
            if (resolvedByTargetUrl != null) return resolvedByTargetUrl;

            List<RealScreenDefinition> definitions;
            try {
                definitions = screenDefinitionRepository.findAll();
            } catch (Exception e) {
                LOG.error("Could not read dbo.ScreenDefinition - no screen will be granted", e);
                definitions = new ArrayList<>();
            }

            /* ALIAS FIRST, THEN NAME - and an ambiguous key is dropped, never guessed.
             *
             * Two reasons, both found in the real data:
             *
             *  - the same label genuinely belongs to two screens in different modules:
             *    "Bank Payment Voucher" is 29 under Accounts Transaction and 703 under Banking
             *    Managment. Neither is the "right" one in general; only the catalog knows which
             *    module it meant.
             *  - stripping the desktop's "frm" class prefix can collide two DIFFERENT screens:
             *    ScreenName "DayBook" (15) and "frmDayBook" (24) both normalise to "daybook",
             *    while their aliases - "Day Book" and "Day Book (Off Set)" - are distinct. So the
             *    alias is the better key, and the name is only a fallback.
             *
             * Picking the first match would gate a menu item on some OTHER screen's rights,
             * silently. ScreenRouteIndex drops a route key two controllers both claim; the same
             * rule applies here. */
            Map<String, Set<Integer>> aliasCandidates = new HashMap<>();
            Map<String, Set<Integer>> nameCandidates = new HashMap<>();
            for (RealScreenDefinition d : definitions) {
                if (d.getId() == null) continue;
                collect(aliasCandidates, d.getScreenAlias(), d.getId());
                collect(nameCandidates, d.getScreenName(), d.getId());
            }
            List<String> ambiguous = new ArrayList<>();
            Map<String, Integer> byAlias = unambiguous(aliasCandidates, ambiguous, "alias");
            Map<String, Integer> byName = unambiguous(nameCandidates, ambiguous, "name");

            Map<String, Integer> out = new HashMap<>();
            List<String> unresolved = new ArrayList<>();
            for (SidebarScreenCatalog.Entry entry : SidebarScreenCatalog.all()) {
                Integer id = entry.realScreenDefinitionId;
                if (id == null) id = byAlias.get(normalise(entry.screenName));
                if (id == null) id = byName.get(normalise(entry.screenName));
                if (id == null) unresolved.add(entry.screenName + " (" + entry.targetUrl + ")");
                else out.put(entry.targetUrl, id);
            }
            if (!ambiguous.isEmpty()) {
                LOG.info("{} screen names match more than one dbo.ScreenDefinition row and were "
                       + "not used for matching: {}", ambiguous.size(), ambiguous);
            }
            if (!unresolved.isEmpty()) {
                /* Named individually rather than counted, so the list is actionable. These screens
                   are hidden until their real ScreenDefinition.Id is confirmed. */
                LOG.warn("{} built screens have no confirmed dbo.ScreenDefinition row and are "
                       + "hidden from the menu: {}", unresolved.size(), unresolved);
            }
            resolvedByTargetUrl = out;
            return out;
        }
    }

    /** Keeps only the keys that name exactly one screen; the rest are reported, not guessed. */
    private static Map<String, Integer> unambiguous(Map<String, Set<Integer>> candidates,
                                                    List<String> ambiguousOut, String label) {
        Map<String, Integer> out = new HashMap<>();
        for (Map.Entry<String, Set<Integer>> c : candidates.entrySet()) {
            if (c.getValue().size() == 1) out.put(c.getKey(), c.getValue().iterator().next());
            else ambiguousOut.add(label + " '" + c.getKey() + "' -> " + c.getValue());
        }
        return out;
    }

    private static void collect(Map<String, Set<Integer>> map, String name, Integer id) {
        String key = normalise(name);
        if (key.isEmpty()) return;
        map.computeIfAbsent(key, k -> new HashSet<>()).add(id);
    }

    /**
     * Lower-case, letters and digits only, with a leading "frm" dropped - the same normalisation
     * ScreenRouteIndex uses, so "frmwagesBillHeader", "Wages Bill Header" and "WagesBillHeader"
     * all land on the same key. A fully-qualified name keeps only its last segment, because the
     * real ScreenName is sometimes "Architecture.WinApp.X.frmY".
     */
    static String normalise(String value) {
        if (value == null) return "";
        String t = value.trim();
        int dot = t.lastIndexOf('.');
        if (dot >= 0 && dot < t.length() - 1) t = t.substring(dot + 1);
        if (t.regionMatches(true, 0, "frm", 0, 3) && t.length() > 3) t = t.substring(3);
        StringBuilder b = new StringBuilder(t.length());
        for (int i = 0; i < t.length(); i++) {
            char c = t.charAt(i);
            if (Character.isLetterOrDigit(c)) b.append(Character.toLowerCase(c));
        }
        return b.toString();
    }

    /**
     * The catalog entries this user may see, for this company.
     *
     * Three reads, each over the whole id set rather than per screen - the per-screen version was
     * ninety-seven round trips on every login.
     */
    public List<SidebarScreenCatalog.Entry> viewableScreens(int userId, Integer companyId) {
        Map<String, Integer> ids = resolved();
        if (ids.isEmpty() || companyId == null || companyId <= 0) {
            if (companyId == null || companyId <= 0) {
                LOG.warn("User {} has no company; no screen can be gated by CompanyRights", userId);
            }
            return new ArrayList<>();
        }

        List<Integer> screenIds = new ArrayList<>(new HashSet<>(ids.values()));

        Set<Integer> companyEnabled = new HashSet<>();
        Map<Integer, Integer> viewRightByScreen = new HashMap<>();
        Set<Integer> grantedRightIds = new HashSet<>();
        try {
            for (RealCompanyRight r : companyRightRepository
                    .findByCompanyIdAndScreenIdInAndIsActiveTrue(companyId, screenIds)) {
                companyEnabled.add(r.getScreenId());
            }
            for (RealScreenRight r : screenRightRepository
                    .findByScreenIdInAndRightName(screenIds, RIGHT_VIEW)) {
                viewRightByScreen.put(r.getScreenId(), r.getId());
            }
            List<Integer> viewRightIds = new ArrayList<>(viewRightByScreen.values());
            if (!viewRightIds.isEmpty()) {
                for (RealUserRight r : userRightRepository
                        .findByUserIdAndCompanyIdAndRightIdInAndValueTrue(userId, companyId, viewRightIds)) {
                    grantedRightIds.add(r.getRightId());
                }
            }
        } catch (Exception e) {
            /* Reported, and the restrictive answer returned: a failed rights read must never be
               read as "grant everything". */
            LOG.error("Could not read the screen rights chain for user {} company {} - "
                    + "granting nothing", userId, companyId, e);
            return new ArrayList<>();
        }

        List<SidebarScreenCatalog.Entry> allowed = new ArrayList<>();
        for (SidebarScreenCatalog.Entry entry : SidebarScreenCatalog.all()) {
            Integer screenId = ids.get(entry.targetUrl);
            if (screenId == null) continue;                       // unresolved - hidden
            if (!companyEnabled.contains(screenId)) continue;     // not enabled for this company
            Integer viewRightId = viewRightByScreen.get(screenId);
            if (viewRightId == null || !grantedRightIds.contains(viewRightId)) continue;
            allowed.add(entry);
        }
        return allowed;
    }

    /** For diagnostics - which built screens still have no real ScreenDefinition behind them. */
    public List<String> unresolvedScreens() {
        Map<String, Integer> ids = resolved();
        List<String> out = new ArrayList<>();
        for (SidebarScreenCatalog.Entry e : SidebarScreenCatalog.all()) {
            if (!ids.containsKey(e.targetUrl)) out.add(e.screenName + " (" + e.targetUrl + ")");
        }
        return out;
    }
}
