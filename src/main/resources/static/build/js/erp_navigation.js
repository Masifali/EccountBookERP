(function () {
    'use strict';
    var navigation = document.getElementById('erp-navigation');
    if (!navigation) return;
    // Hosted forms use their outer page's navigation. This also handles browsers
    // that do not send Sec-Fetch-Dest, before any sidebar space is reserved.
    if (window.self !== window.top) {
        navigation.remove();
        return;
    }
    var toggle = document.getElementById('erp-navigation-toggle');
    var body = document.body;
    var smallScreen = window.matchMedia('(max-width: 767px)');
    body.classList.add('erp-with-navigation');

    function expand(open, remember) {
        body.classList.toggle('erp-navigation-expanded', open);
        var sidebar = document.getElementById('erp-module-sidebar');
        sidebar.inert = smallScreen.matches && !open;
        sidebar.setAttribute('aria-hidden', String(smallScreen.matches && !open));
        toggle.setAttribute('aria-expanded', String(open));
        var label = (open ? 'Collapse' : 'Expand') + ' module sidebar';
        toggle.setAttribute('aria-label', label);
        toggle.title = label;
        if (remember && !smallScreen.matches) {
            try { localStorage.setItem('erp.sidebar.expanded', String(open)); } catch (ignored) { }
        }
    }
    var saved = false;
    try { saved = localStorage.getItem('erp.sidebar.expanded') === 'true'; } catch (ignored) { }
    expand(!smallScreen.matches && saved, false);
    toggle.addEventListener('click', function () {
        expand(!body.classList.contains('erp-navigation-expanded'), true);
    });
    navigation.querySelector('.erp-navigation-shade').addEventListener('click', function () {
        expand(false, false);
        toggle.focus();
    });
    document.addEventListener('keydown', function (event) {
        if (event.key === 'Escape' && body.classList.contains('erp-navigation-expanded')) {
            expand(false, false);
            toggle.focus();
        }
    });
    smallScreen.addEventListener('change', function () { expand(false, false); });

    /* user menu: click opens / closes, outside click or Escape closes, arrows move between items */
    var userBtn = document.getElementById('erp-username');
    var userMenu = document.getElementById('erp-user-dropdown');
    if (userBtn && userMenu) {
        var items = function () { return Array.prototype.slice.call(userMenu.querySelectorAll('a[role="menuitem"]')); };
        var setOpen = function (open, focusFirst) {
            userMenu.hidden = !open;
            userBtn.setAttribute('aria-expanded', String(open));
            if (open && focusFirst) { var f = items()[0]; if (f) f.focus(); }
        };
        userBtn.addEventListener('click', function (e) { e.stopPropagation(); setOpen(userMenu.hidden, false); });
        userBtn.addEventListener('keydown', function (e) {
            if (e.key === 'ArrowDown') { e.preventDefault(); setOpen(true, true); }
        });
        userMenu.addEventListener('keydown', function (e) {
            var list = items(), i = list.indexOf(document.activeElement);
            if (e.key === 'ArrowDown') { e.preventDefault(); (list[i + 1] || list[0]).focus(); }
            else if (e.key === 'ArrowUp') { e.preventDefault(); (list[i - 1] || list[list.length - 1]).focus(); }
            else if (e.key === 'Escape') { e.preventDefault(); e.stopPropagation(); setOpen(false); userBtn.focus(); }
        });
        document.addEventListener('click', function (e) { if (!userMenu.hidden && !userMenu.contains(e.target)) setOpen(false); });
        document.addEventListener('keydown', function (e) { if (e.key === 'Escape' && !userMenu.hidden) setOpen(false); });
    }

    function updateExistingBackLinks() {
        var parent = navigation.getAttribute('data-parent-url');
        if (!parent) return;
        var parentTitle = navigation.getAttribute('data-parent-title');
        document.querySelectorAll('a').forEach(function (link) {
            if (navigation.contains(link)) return;
            var text = link.textContent.trim();
            if (!/^back(?:\s+to\b|\s*$)/i.test(text)) return;
            var inlineAction = link.getAttribute('onclick');
            // Keep custom handlers, such as checks for unsaved changes, intact.
            if (inlineAction && !/^\s*(?:window\.)?history\.(?:back\(\)|go\(-1\))\s*;?\s*(?:return false;?)?\s*$/.test(inlineAction)) return;
            link.setAttribute('href', parent);
            link.removeAttribute('onclick');
            link.textContent = '\u2190 Back to ' + parentTitle;
            link.title = 'Back to ' + parentTitle;
        });
    }
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', updateExistingBackLinks);
    else updateExistingBackLinks();
}());
