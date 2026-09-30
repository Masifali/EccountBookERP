/* ============================================================================================
 * countx_fixedassets_common.js - small helpers shared by the Fixed Assets pages (dbo.App 9), on top of
 * countx_hrm.js (window.HRM). Exports window.FA.
 *
 *   FA.fillCols(id, rows, valueKey, textKey, extraKeys, opts)  UltraCombo with several visible columns:
 *        <select data-columns="Title|Code|...">, every option carries data-extra="code|..." (read by
 *        countx_prod_combo.js). opts.zero as HRM.fill (default '' slot), opts.keep keeps the current value.
 *   FA.tabs(onChange)            the desktop TabControl: [data-fa-tab] buttons switch [data-fa-page] panes.
 *   FA.showTab(name)
 *   FA.fmt(v, d, group)          .NET "#,##0.###"-style text: at most d decimals, trailing zeros dropped.
 *   FA.fixed(v, d)               "0.00" style (d decimals kept).
 *   FA.roundAway(v, d)           Math.Round(v, d, MidpointRounding.AwayFromZero).
 *   FA.roundEven(v, d)           Math.Round(v, d) (banker's rounding).
 *   FA.printPdf(url, body, btn)  POST a typed print body (/reports/print/...) and show the PDF in a new tab.
 *   FA.onLeave(id, fn)           UltraCombo.Leave for a searchable combo (a pick, or focus leaving its wrap).
 * ============================================================================================ */
(function (global) {
    'use strict';

    var FA = {};
    var doc = global.document;

    FA.fillCols = function (id, rows, valueKey, textKey, extraKeys, opts) {
        var sel = HRM.$(id); if (!sel) return;
        opts = opts || {};
        var keepV = opts.keep ? sel.value : null;
        var zero = opts.zero === undefined ? '' : opts.zero;
        var html = zero === false ? '' : '<option value="0">' + HRM.esc(zero) + '</option>';
        (rows || []).forEach(function (r) {
            var extra = (extraKeys || []).map(function (k) { return String(HRM.str(HRM.col(r, k))).replace(/\|/g, '/'); }).join('|');
            html += '<option value="' + HRM.esc(HRM.col(r, valueKey)) + '" data-extra="' + HRM.esc(extra) + '">' + HRM.esc(HRM.col(r, textKey)) + '</option>';
        });
        sel.innerHTML = html;
        if (keepV !== null && HRM.hasOption(id, keepV)) sel.value = keepV;
        else if (zero !== false) sel.value = '0';
        else sel.selectedIndex = sel.options.length ? 0 : -1;
        sel._rows = rows || [];
        HRM.refreshCombos();
    };

    FA.tabs = function (onChange) {
        doc.addEventListener('click', function (e) {
            var b = e.target.closest ? e.target.closest('[data-fa-tab]') : null;
            if (!b || b.disabled) return;
            FA.showTab(b.getAttribute('data-fa-tab'));
            if (onChange) onChange(b.getAttribute('data-fa-tab'));
        });
    };
    FA.showTab = function (name) {
        doc.querySelectorAll('[data-fa-tab]').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-fa-tab') === name); });
        doc.querySelectorAll('[data-fa-page]').forEach(function (p) { p.classList.toggle('is-active', p.getAttribute('data-fa-page') === name); });
    };
    FA.activeTab = function () {
        var t = doc.querySelector('[data-fa-tab].is-active');
        return t ? t.getAttribute('data-fa-tab') : null;
    };

    FA.roundAway = function (v, d) {
        var p = Math.pow(10, d || 0), x = HRM.num(v) * p;
        return (x < 0 ? -Math.round(-x + 1e-9) : Math.round(x + 1e-9)) / p;
    };
    FA.roundEven = function (v, d) {
        var p = Math.pow(10, d || 0), x = HRM.num(v) * p, r = Math.round(x);
        if (Math.abs(x % 1) === 0.5) r = 2 * Math.round(x / 2);
        return r / p;
    };
    FA.fmt = function (v, d, group) {
        var n = FA.roundAway(v, d === undefined ? 3 : d);
        return n.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: d === undefined ? 3 : d, useGrouping: group !== false });
    };
    FA.fixed = function (v, d) { return FA.roundAway(v, d || 0).toFixed(d || 0); };

    FA.printPdf = function (url, body, btn) {
        var win = global.open('about:blank', '_blank');
        var h = { 'Content-Type': 'application/json', 'Accept': 'application/pdf' };
        var t = doc.querySelector('meta[name="_csrf"]'), n = doc.querySelector('meta[name="_csrf_header"]');
        if (t && n && t.getAttribute('content') && n.getAttribute('content')) h[n.getAttribute('content')] = t.getAttribute('content');
        return HRM.busy(btn, function () {
            return fetch(url, { method: 'POST', credentials: 'same-origin', headers: h, body: JSON.stringify(body || {}) }).then(function (r) {
                var type = r.headers.get('Content-Type') || '';
                if (r.ok && type.indexOf('application/pdf') === 0) return r.blob().then(function (b) { if (win) win.location = URL.createObjectURL(b); });
                return r.text().then(function (x) { if (win) win.close(); HRM.box(x || ('Print failed (' + r.status + ')')); });
            }).catch(function (e) { if (win) win.close(); HRM.fail(e); });
        });
    };

    FA.onLeave = function (id, fn) {
        var sel = HRM.$(id); if (!sel) return;
        function fire() { fn(); }
        function inWrap() { var w = sel.closest('.dtcombo-wrap'); return !!(w && doc.activeElement && w.contains(doc.activeElement)); }
        /* a pick made without focus in the field (mouse in the popup) is a Leave too; with focus inside, focusout is */
        sel.addEventListener('change', function () { setTimeout(function () { if (!inWrap()) fire(); }, 0); });
        doc.addEventListener('focusout', function (e) {
            var w = e.target && e.target.closest ? e.target.closest('.dtcombo-wrap') : null;
            if (!w || !w.querySelector('#' + id)) return;
            if (e.relatedTarget && w.contains(e.relatedTarget)) return;
            setTimeout(fire, 0);
        });
    };

    /**
     * The green "+" forms the desktop opens with Form.Show() and that the web already has as dialogs
     * (countx_store_define_lookups.js, window.StoreDefine): 'openAssetCategory' (frmItemCatagoryStore
     * FormTypeId 3), 'openDepartment' (Define_Department), 'openFixedAssetItem' (frmDefineAssets).
     * The two store scripts are loaded on the first click only (after page load, so their page-level
     * DOMContentLoaded work never runs here). Their API checks the rights of the form itself.
     */
    var storeLoad = null;
    function loadScript(src) {
        return new Promise(function (res, rej) {
            var s = doc.createElement('script');
            s.src = src; s.onload = res; s.onerror = function () { rej(new Error('Could not load ' + src)); };
            doc.head.appendChild(s);
        });
    }
    FA.storeDialog = function (kind, onClose) {
        if (!storeLoad) {
            storeLoad = (global.StoreCommon ? Promise.resolve() : loadScript('/build/js/countx_store_common.js?v=20260928b'))
                .then(function () { return global.StoreDefine ? null : loadScript('/build/js/countx_store_define_lookups.js?v=20260928c'); });
        }
        return storeLoad.then(function () {
            if (!global.StoreDefine || typeof global.StoreDefine[kind] !== 'function') throw new Error('This form is not available.');
            global.StoreDefine[kind](onClose || null, {});
        }).catch(function (e) { storeLoad = null; HRM.fail(e); });
    };
    /** true while a store dialog is open (the page's own keys stay quiet then). */
    FA.dialogOpen = function () { return !!doc.querySelector('.sdl-overlay, .hrm-modal'); };

    global.FA = FA;
})(window);
