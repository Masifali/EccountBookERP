/* ============================================================================================
 * Screen 280 "Production Against (Job Order)" - the shell (FoodProductionWithValues.cs).
 *
 *   frmFoodProduction_Load:452          the switches, rights and the tab strip (from /shell)
 *   tabControl1_SelectedIndexChanged:839 hosted forms in PanelOtherForms, framed here:
 *        Input / Output / PackingMaterial / Overhead  FormHelper.OpenOrGetFormInPanel - created once,
 *                                                     REUSED: one iframe each, kept alive, shown/hidden
 *        Summary / Settlement                          FormHelper.OpenFormInPanel(new form) - a NEW
 *                                                     iframe on every selection
 *        index 2 (Consumption)                         GenerateDocConsumption when Save is visible and
 *                                                     enabled, ConsumptionDetailComboBind, focus date
 *        index 5                                       focus txtfromDateTransactionHistory
 *   frmFoodProduction_KeyDown:747       Ctrl+NumPad1..7 select tabs 0..6 BY POSITION; Ctrl+E / Esc close;
 *                                       the per-tab shortcuts are registered by the tab scripts.
 *   SettlementForm_OnOverHeadReadById:3331 / _OnPackingMaterialReadById:3317
 *                                       window.P280OpenTab(name, id)
 * ============================================================================================ */
(function (global) {
    'use strict';

    var API = '/api/production/production-against-job-order';
    var doc = global.document;

    function $(id) { return doc.getElementById(id); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }

    function csrf(h) {
        var t = doc.querySelector('meta[name="_csrf"]'), n = doc.querySelector('meta[name="_csrf_header"]');
        if (t && n && t.getAttribute('content')) h[n.getAttribute('content')] = t.getAttribute('content');
        return h;
    }
    function handle(r) {
        return r.text().then(function (t) {
            var body = null;
            try { body = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
            if (!r.ok) throw new Error((body && body.message) || t || ('Request failed (' + r.status + ')'));
            return body;
        });
    }
    function getJson(url) {
        return fetch(url, { headers: { Accept: 'application/json' }, credentials: 'same-origin' }).then(handle);
    }
    function postJson(url, data) {
        return fetch(url, {
            method: 'POST', credentials: 'same-origin',
            headers: csrf({ 'Content-Type': 'application/json', Accept: 'application/json' }),
            body: JSON.stringify(data || {})
        }).then(handle);
    }
    function qs(o) {
        var a = [];
        Object.keys(o).forEach(function (k) {
            var v = o[k];
            if (v === null || v === undefined) return;
            a.push(encodeURIComponent(k) + '=' + encodeURIComponent(v));
        });
        return a.length ? '?' + a.join('&') : '';
    }

    /* The button contract: disabled at once with a spinner, duplicates ignored, re-enabled on
       success AND failure (restoring whatever enabled state the rights gave it). */
    function busy(btn, fn) {
        var b = (typeof btn === 'string') ? $(btn) : btn;
        if (b) {
            if (b.classList.contains('is-busy')) return Promise.resolve();
            b.dataset.wasDisabled = b.disabled ? '1' : '';
            b.disabled = true;
            b.classList.add('is-busy');
        }
        var done = function () { if (b) { b.classList.remove('is-busy'); b.disabled = b.dataset.wasDisabled === '1'; } };
        var p;
        try { p = Promise.resolve(fn()); } catch (e) { done(); return Promise.reject(e); }
        return p.then(function (v) { done(); return v; }, function (e) { done(); throw e; });
    }

    /* MessageBox.Show(ex.Message) */
    function box(m) { global.alert(m); }

    var shell = null;
    var active = -1;
    var frames = {};                     /* tab name -> iframe (the reused ones) */
    var keyHandlers = [];
    var selectHooks = [];

    var REUSED = { 'Input': 1, 'Output': 1, 'PackingMaterial': 1, 'Overhead': 1 };

    function renderTabs() {
        var strip = $('p280Tabs');
        strip.innerHTML = '';
        shell.tabs.forEach(function (t, i) {
            var b = doc.createElement('div');
            b.className = 'p280-tab';
            b.setAttribute('role', 'tab');
            b.textContent = t.name;
            b.onclick = function () { selectTab(i); };
            strip.appendChild(b);
        });
    }

    function frameFor(t, fresh) {
        var host = $('p280Frames');
        var f = REUSED[t.name] ? frames[t.name] : null;
        if (!REUSED[t.name]) {
            /* OpenFormInPanel with a NEW form: the previous one is discarded. */
            var old = frames[t.name];
            if (old && old.parentNode) old.parentNode.removeChild(old);
            f = null;
        }
        if (!f || fresh) {
            f = doc.createElement('iframe');
            f.title = t.name;
            f.setAttribute('data-tab', t.name);
            f.__loaded = false;
            f.addEventListener('load', function () {
                f.__loaded = true;
                forwardKeys(f);
            });
            f.src = t.route;
            host.appendChild(f);
            frames[t.name] = f;
        }
        return f;
    }

    /* The desktop form's KeyPreview sees keys typed inside the hosted forms too; the tab shortcuts
       (Ctrl+NumPad1..7) and Ctrl+E are forwarded from each framed page to the shell. */
    function forwardKeys(f) {
        try {
            var d = f.contentWindow.document;
            if (d.__p280fwd) return;
            d.__p280fwd = true;
            d.addEventListener('keydown', function (e) {
                if (e.ctrlKey && (/^Numpad[1-7]$/.test(e.code) || e.key === 'e' || e.key === 'E')) {
                    e.preventDefault();
                    onKey(e);
                }
            });
        } catch (e) { /* cross-origin - nothing to forward */ }
    }

    function selectTab(i) {
        if (!shell || i < 0 || i >= shell.tabs.length) return;
        var prev = active;
        active = i;
        Array.prototype.forEach.call($('p280Tabs').children, function (c, k) { c.classList.toggle('active', k === i); });
        var t = shell.tabs[i];
        $('pageConsumption').classList.toggle('on', t.name === 'Consumption');
        $('pageTransactionHistory').classList.toggle('on', t.name === 'Transaction History');
        Object.keys(frames).forEach(function (k) { frames[k].classList.remove('on'); });
        if (t.name !== 'Consumption' && t.name !== 'Transaction History') {
            if (t.route) {
                var f = frameFor(t, false);
                f.classList.add('on');
            } else {
                message('The ' + t.name + ' tab (' + t.desktopForm + ') has no page.');
            }
        }
        /* tabControl1_SelectedIndexChanged:899-911 - by POSITION, as the desktop tests SelectedIndex. */
        selectHooks.forEach(function (h) { try { h(i, t, prev); } catch (e) { box(e.message); } });
    }

    function message(text) {
        var m = $('p280Message');
        if (!text) { m.style.display = 'none'; return; }
        m.textContent = text;
        m.style.display = 'block';
    }

    /** Calls fn(contentWindow) once the framed page has loaded and exposes `member`. */
    function whenReady(f, member, fn) {
        var tries = 0;
        (function poll() {
            var w = null;
            try { w = f.contentWindow; } catch (e) { w = null; }
            if (f.__loaded && w && typeof w[member] === 'function') { fn(w); return; }
            if (++tries > 100) { box('The ' + f.title + ' page did not become ready (' + member + ').'); return; }
            setTimeout(poll, 150);
        }());
    }

    /* SettlementForm_OnPackingMaterialReadById:3317 (SelectedIndex = 3) and
       SettlementForm_OnOverHeadReadById:3331 (SelectedIndex = 4): select the tab, then its
       form's first inner tab and ReadById - the hosted page's P280ReadById does both. */
    global.P280OpenTab = function (tabName, id) {
        var idx = tabName === 'PackingMaterial' ? 3 : (tabName === 'Overhead' ? 4 : -1);
        if (idx < 0) {
            shell.tabs.forEach(function (t, k) { if (t.name === tabName) idx = k; });
        }
        selectTab(idx);
        var t = shell.tabs[idx];
        if (!t || !frames[t.name]) return;
        whenReady(frames[t.name], 'P280ReadById', function (w) {
            try { w.P280ReadById(id); } catch (e) { box(e.message); }
        });
    };

    /* A hosted form's Ctrl+E / Esc. On the desktop the shell has KeyPreview = true, so the same key
       first reaches frmFoodProduction_KeyDown:781, which Close()s the WHOLE screen - the hosted
       form goes with it. P280CloseTab therefore closes the shell, not just the tab. */
    global.P280CloseTab = function (tabName) {
        global.location.href = '/production';
    };

    /* DatagridHistory_ColumnButtonClick:1106 - Input: SelectedIndex 0, CmbJobOrderNo disabled,
       ReadByIdInput(Id); Output: SelectedIndex 1, CmbJobOrderNoOutput disabled, ReadByIdOutPut. */
    function openHosted(index, id) {
        selectTab(index);
        var t = shell.tabs[index];
        if (!t || !frames[t.name]) return;
        whenReady(frames[t.name], 'P280ReadById', function (w) {
            try { w.P280ReadById(id, { disableJobOrder: true }); } catch (e) { box(e.message); }
        });
    }

    function modalOpen() { return !!doc.querySelector('.p280-modal.open, .rk-modal.is-open'); }

    function onKey(e) {
        /* frmFoodProduction_KeyDown:751-783 */
        if (e.ctrlKey && /^Numpad[1-7]$/.test(e.code)) {
            e.preventDefault();
            var n = parseInt(e.code.slice(6), 10) - 1;
            selectTab(n);
            if (n === 2) { var d = $('txtDocDateConsumption'); if (d) d.focus(); }
            if (n === 5) { var g = $('DatagridHistoryHost'); if (g) g.focus(); }
            return;
        }
        if ((e.ctrlKey && (e.key === 'e' || e.key === 'E')) || e.key === 'Escape') {
            if (e.key === 'Escape' && modalOpen()) return;
            e.preventDefault();
            global.location.href = '/production';
            return;
        }
        keyHandlers.forEach(function (h) { try { h(e, active); } catch (x) { box(x.message); } });
    }

    doc.addEventListener('keydown', function (e) {
        /* A loader form open on top is its own window on the desktop: its keys are its own. */
        if (e.defaultPrevented || modalOpen()) return;
        onKey(e);
    });

    global.P280 = {
        api: API,
        $: $, esc: esc, getJson: getJson, postJson: postJson, qs: qs, busy: busy, box: box,
        shell: function () { return shell; },
        activeIndex: function () { return active; },
        selectTab: selectTab,
        openHosted: openHosted,
        onKey: function (fn) { keyHandlers.push(fn); },
        onSelect: function (fn) { selectHooks.push(fn); },
        ready: null
    };

    global.P280.ready = new Promise(function (resolve, reject) {
        doc.addEventListener('DOMContentLoaded', function () {
            getJson(API + '/shell').then(function (d) {
                shell = d;
                renderTabs();
                resolve(d);
                /* :607 - tabControl1.SelectedIndex = 0 at the end of Load. */
                selectTab(0);
            }).catch(function (e) {
                message('The screen could not read its configuration: ' + e.message);
                reject(e);
            });
        });
    });
}(window));
