package com.mst.services;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.ObjectProvider;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.context.event.ContextRefreshedEvent;
import org.springframework.context.event.EventListener;
import org.springframework.stereotype.Component;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.method.HandlerMethod;
import org.springframework.web.servlet.mvc.method.RequestMappingInfo;
import org.springframework.web.servlet.mvc.method.annotation.RequestMappingHandlerMapping;

import java.util.HashMap;
import java.util.HashSet;
import java.util.Map;
import java.util.Set;

/**
 * Where a desktop screen lives on the web.
 *
 * The desktop resolves a menu item by its own TargetUrl - a .NET type name such as
 * "Architecture.WinApp.Contractor_Wages.frmwagesBillHeader" (DashboardNew.cs :1553,
 * MenuItem_Click). That string means nothing to a browser, so the web hub has to map a screen
 * onto a page of this application.
 *
 * It is NOT mapped by guessing a URL. This index reads the routes Spring has actually registered
 * - so every URL it hands back is a page that exists - and matches a screen to one only on exact
 * equality after normalising (lower-case, letters and digits only):
 *
 *     ScreenName "LabourWages"  ->  "labourwages"  ->  /accounts/vouchers/labour-wages
 *
 * A screen that matches nothing resolves to null and the caller shows the not-built placeholder
 * naming its desktop form. A normalised key that two different routes both claim is dropped as
 * ambiguous rather than being resolved arbitrarily, because the brief requires that different
 * URLs never open the same or an unrelated page.
 *
 * The index therefore grows by itself as screens are ported; there is no hand-kept list to drift.
 */
@Component
public class ScreenRouteIndex {

    private static final Logger LOG = LoggerFactory.getLogger(ScreenRouteIndex.class);

    /* An ObjectProvider, not the mapping itself: RequestMappingHandlerMapping is built while the
       web layer initialises, and asking for it by field injection can put this bean in the middle
       of that. The provider is resolved on the ContextRefreshedEvent instead, by which time the
       mapping is finished. */
    @Autowired
    @Qualifier("requestMappingHandlerMapping")
    private ObjectProvider<RequestMappingHandlerMapping> handlerMappingProvider;

    /** normalised last path segment -> the single GET route that owns it */
    private final Map<String, String> byLastSegment = new HashMap<>();
    /** normalised full path (all segments joined) -> the single GET route that owns it */
    private final Map<String, String> byFullPath = new HashMap<>();

    private volatile boolean built = false;

    @EventListener(ContextRefreshedEvent.class)
    public synchronized void build() {
        if (built) return;
        Set<String> ambiguousLast = new HashSet<>();
        Set<String> ambiguousFull = new HashSet<>();
        try {
            RequestMappingHandlerMapping handlerMapping = handlerMappingProvider.getIfAvailable();
            if (handlerMapping == null) {
                LOG.warn("No RequestMappingHandlerMapping available; every screen will show as not built");
                built = true;
                return;
            }
            for (Map.Entry<RequestMappingInfo, HandlerMethod> e
                    : handlerMapping.getHandlerMethods().entrySet()) {
                RequestMappingInfo info = e.getKey();

                Set<RequestMethod> methods = info.getMethodsCondition().getMethods();
                if (!methods.isEmpty() && !methods.contains(RequestMethod.GET)) continue;

                for (String pattern : patternsOf(info)) {
                    /* Only plain page routes: no path variables, no wildcards, and nothing under
                       /api - those are data endpoints, not pages a card may open. */
                    if (pattern == null || pattern.isEmpty()) continue;
                    if (pattern.indexOf('{') >= 0 || pattern.indexOf('*') >= 0) continue;
                    if (pattern.startsWith("/api")) continue;
                    if ("/".equals(pattern)) continue;

                    String last = pattern.substring(pattern.lastIndexOf('/') + 1);
                    String kLast = norm(last);
                    if (!kLast.isEmpty()) {
                        String prev = byLastSegment.put(kLast, pattern);
                        if (prev != null && !prev.equals(pattern)) ambiguousLast.add(kLast);
                    }

                    String kFull = norm(pattern);
                    if (!kFull.isEmpty()) {
                        String prev = byFullPath.put(kFull, pattern);
                        if (prev != null && !prev.equals(pattern)) ambiguousFull.add(kFull);
                    }
                }
            }
            for (String k : ambiguousLast) byLastSegment.remove(k);
            for (String k : ambiguousFull) byFullPath.remove(k);
            built = true;
            LOG.info("Screen route index built: {} unique page routes ({} ambiguous names dropped)",
                     byLastSegment.size(), ambiguousLast.size());
        } catch (Exception ex) {
            LOG.error("Screen route index could not be built; screens will show as not built", ex);
            built = true;   // do not retry on every request
        }
    }

    @SuppressWarnings("unchecked")
    private static Iterable<String> patternsOf(RequestMappingInfo info) {
        /* Spring 5.3 exposes either PatternsRequestCondition (Ant) or
           PathPatternsRequestCondition, depending on how the app is configured. */
        try {
            if (info.getPatternsCondition() != null) {
                return info.getPatternsCondition().getPatterns();
            }
        } catch (Throwable ignored) { /* fall through */ }
        try {
            Object c = info.getPathPatternsCondition();
            if (c != null) {
                java.util.List<String> out = new java.util.ArrayList<>();
                for (Object p : (Set<Object>) c.getClass().getMethod("getPatterns").invoke(c)) {
                    out.add(String.valueOf(p));
                }
                return out;
            }
        } catch (Throwable ignored) { /* fall through */ }
        return java.util.Collections.emptyList();
    }

    /**
     * The web route for a screen, or null when this application has no page for it.
     *
     * @param screenName    ScreenDefinition.ScreenName
     * @param targetUrl     ScreenDefinition.TargetUrl (the desktop .NET type name)
     */
    public String routeFor(String screenName, String targetUrl) {
        if (!built) build();

        String cls = targetUrl == null ? "" : targetUrl.trim();
        if (cls.contains(".")) cls = cls.substring(cls.lastIndexOf('.') + 1);

        for (String candidate : new String[] { screenName, cls, stripFormPrefix(cls),
                                               stripFormPrefix(screenName) }) {
            String k = norm(candidate);
            /* A very short name is not evidence of anything - "Lab", "POS", "GRN" would collide
               with unrelated routes, and the brief forbids a link that opens an unrelated page.
               Below this length the screen is reported as not built instead. */
            if (k.length() < 5) continue;
            String r = byLastSegment.get(k);
            if (r != null) return r;
            r = byFullPath.get(k);
            if (r != null) return r;
        }
        return null;
    }

    /** "frmwagesBillHeader" -> "wagesBillHeader"; the desktop's form-class prefix carries no meaning here. */
    private static String stripFormPrefix(String s) {
        if (s == null) return null;
        String t = s.trim();
        if (t.regionMatches(true, 0, "frm", 0, 3) && t.length() > 3) return t.substring(3);
        if (t.regionMatches(true, 0, "btn", 0, 3) && t.length() > 3) return t.substring(3);
        return t;
    }

    private static String norm(String s) {
        if (s == null) return "";
        StringBuilder b = new StringBuilder(s.length());
        for (int i = 0; i < s.length(); i++) {
            char c = s.charAt(i);
            if (Character.isLetterOrDigit(c)) b.append(Character.toLowerCase(c));
        }
        return b.toString();
    }

    /** For diagnostics: how many page routes this application registered. */
    public int size() { if (!built) build(); return byLastSegment.size(); }
}
