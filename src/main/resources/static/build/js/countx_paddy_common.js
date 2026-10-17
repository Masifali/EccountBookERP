/* Shared helpers of the Paddy Purchase (module 48) screens. No dependency except countx_purchase_request.js (PurchaseRequest). */
(function (global) {
    'use strict';
    const PP = {};
    PP.$ = id => document.getElementById(id);
    PP.num = v => { const n = Number(String(v == null ? '' : v).replace(/,/g, '').trim()); return isNaN(n) ? 0 : n; };
    PP.esc = v => String(v == null ? '' : v).replace(/[&<>"']/g, c => ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c]));
    PP.localStamp = (d = new Date()) => new Date(d.getTime() - d.getTimezoneOffset() * 60000).toISOString();
    PP.today = () => PP.localStamp().slice(0, 10);
    PP.error = e => alert((e && e.message) || 'Request failed. Please retry.');
    const MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    PP.fmtDate = (v, withTime) => {
        if (v == null || v === '') return '';
        const d = typeof v === 'number' ? new Date(v) : new Date(String(v).replace(' ', 'T'));
        if (isNaN(d)) return PP.esc(v);
        const p = n => String(n).padStart(2, '0');
        let s = p(d.getDate()) + '-' + MON[d.getMonth()] + '-' + d.getFullYear();
        if (withTime || d.getHours() || d.getMinutes()) s += ' ' + p(d.getHours()) + ':' + p(d.getMinutes());
        return s;
    };
    PP.fmtNum = (v, dec) => { const n = Number(v); if (v === null || v === '' || isNaN(n)) return v == null ? '' : PP.esc(v); return n.toLocaleString('en-US', { maximumFractionDigits: dec == null ? 3 : dec }); };
    PP.fetch = async function (url, options) {
        const run = async () => {
            const response = await fetch(url, options);
            const text = await response.text();
            let data = null;
            try { data = text ? JSON.parse(text) : null; } catch (_) { throw Error('The session expired or the server returned an invalid response. Please sign in and retry.'); }
            if (!response.ok || (data && data.success === false && !data.confirmKey && data.message && !Array.isArray(data))) throw Error((data && (data.message || data.detail)) || 'Request failed (' + response.status + ')');
            return data;
        };
        return global.PurchaseRequest ? global.PurchaseRequest.track(run) : run();
    };
    PP.post = (url, body) => PP.fetch(url, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(body) });
    PP.withButton = async (btn, work) => { try { return global.PurchaseRequest ? await global.PurchaseRequest.run(btn, work) : await work(); } catch (e) { PP.error(e); } };

    /* DropDownBind.BindDDL / BindDDLNew: the select keeps one empty option (value '' = no ActiveRow); zeroIndex adds the "...Select Any Value..." caption text. */
    PP.bind = function (id, list, valKey, textKey, opts) {
        const sel = PP.$(id); if (!sel) return;
        opts = opts || {};
        sel.innerHTML = '<option value="">' + (opts.zeroIndex ? '...Select Any Value...' : '') + '</option>';
        (list || []).forEach(item => { const o = document.createElement('option'); o.value = item[valKey]; o.textContent = item[textKey]; sel.appendChild(o); });
        if (opts.activateFirst && sel.options.length > 1) sel.selectedIndex = 1;
    };
    PP.selText = id => { const s = PP.$(id); return s && s.selectedIndex >= 0 && s.value !== '' ? s.options[s.selectedIndex].textContent.trim() : ''; };
    PP.setSel = (id, value) => { const s = PP.$(id); if (!s) return; s.value = value == null ? '' : String(value); };
    PP.setSelText = (id, text) => { const s = PP.$(id); if (!s) return; const o = Array.from(s.options).find(x => x.textContent.trim() === String(text || '')); s.value = o ? o.value : ''; };

    /* CommonServices.OnlytextdecimelFunction / OnlytextNumberFunction (KeyPress): data-guard="decimal" | "digits" */
    PP.installGuards = function () {
        document.addEventListener('keypress', e => {
            const t = e.target; if (!t || !t.dataset || !t.dataset.guard || e.ctrlKey || e.metaKey || e.key.length > 1) return;
            if (t.dataset.guard === 'digits' && !/\d/.test(e.key)) e.preventDefault();
            if (t.dataset.guard === 'decimal' && !/[\d.]/.test(e.key)) e.preventDefault();
            if (t.dataset.guard === 'decimal' && e.key === '.' && t.value.indexOf('.') > -1) e.preventDefault();
        });
        document.addEventListener('paste', e => { const t = e.target; if (t && t.dataset && t.dataset.guard) { const x = (e.clipboardData || global.clipboardData).getData('text'); if (!(t.dataset.guard === 'digits' ? /^\d*$/ : /^\d*\.?\d*$/).test(x)) e.preventDefault(); } });
    };
    /* KeyDown Return -> SendKeys "{TAB}" */
    PP.enterToTab = function (root) {
        root.addEventListener('keydown', e => {
            if (e.key !== 'Enter' || e.ctrlKey || e.altKey) return;
            const t = e.target; if (!t || t.tagName === 'BUTTON' || t.tagName === 'TEXTAREA' || t.type === 'button' || (t.closest && t.closest('.dtcombo-pop'))) return;
            const list = Array.from(root.querySelectorAll('input:not([type=hidden]):not([disabled]),select:not([disabled]),.dtcombo-input,button.pp-btn:not([disabled])')).filter(x => x.offsetParent !== null && !x.closest('.dtcombo-pop') && x.tabIndex >= 0);
            const i = list.indexOf(t); if (i < 0) return;
            e.preventDefault(); (list[i + 1] || list[0]).focus();
        });
    };

    /* Janus-like grid from rows: opts {hide:[], caption:{}, buttons:[{key,label,onclick}], pre:[buttons first], numeric:[], link:[], dateCols:[], frozen:n, onRow, onDbl, totals:{key:fmt}} */
    PP.renderGrid = function (tableId, rows, opts) {
        opts = opts || {};
        const table = PP.$(tableId), head = table.querySelector('thead'), body = table.querySelector('tbody');
        let foot = table.querySelector('tfoot'); if (!foot) { foot = document.createElement('tfoot'); table.appendChild(foot); }
        if (!rows || !rows.length) { head.innerHTML = ''; body.innerHTML = ''; foot.innerHTML = ''; table._rows = []; return; }
        const hide = new Set(opts.hide || []);
        const keys = Object.keys(rows[0]).filter(k => !hide.has(k));
        const pre = opts.pre || [], post = opts.post || [];
        const fz = n => (opts.frozen && n < opts.frozen) ? ' class="frozen" style="left:' + (n * 52) + 'px"' : '';
        let h = '<tr>';
        pre.forEach((b, i) => { h += '<th' + fz(i) + '>' + PP.esc(b.label) + '</th>'; });
        keys.forEach(k => { h += '<th>' + PP.esc((opts.caption || {})[k] || k) + '</th>'; });
        post.forEach(b => { h += '<th>' + PP.esc(b.label) + '</th>'; });
        head.innerHTML = h + '</tr>';
        const isNum = k => (opts.numeric || []).includes(k) || (typeof rows[0][k] === 'number' && !(opts.dateCols || []).includes(k));
        body.innerHTML = rows.map((r, ri) => {
            let t = '<tr data-i="' + ri + '">';
            pre.forEach((b, i) => { t += '<td' + fz(i) + '><button type="button" class="tool-btn" data-act="' + b.key + '">' + PP.esc(b.label) + '</button></td>'; });
            keys.forEach(k => {
                const v = r[k]; let c;
                if ((opts.dateCols || []).includes(k)) c = PP.fmtDate(v);
                else if ((opts.link || []).includes(k)) c = (Number(v) > 0 || (opts.linkAlways || []).includes(k)) ? '<span class="code-link" data-link="' + k + '">' + PP.esc(v) + '</span>' : PP.esc(v);
                else if (typeof v === 'boolean') c = '<input type="checkbox" disabled' + (v ? ' checked' : '') + '>';
                else if (typeof v === 'number') c = PP.fmtNum(v, (opts.dec || {})[k]);
                else c = PP.esc(v);
                t += '<td' + (isNum(k) ? ' class="n"' : '') + '>' + c + '</td>';
            });
            post.forEach(b => { t += '<td><button type="button" class="tool-btn" data-act="' + b.key + '">' + PP.esc(b.label) + '</button></td>'; });
            return t + '</tr>';
        }).join('');
        const tot = opts.totals || {};
        if (Object.keys(tot).length) {
            let f = '<tr>'; pre.forEach(() => { f += '<td></td>'; });
            keys.forEach(k => { f += '<td>' + (tot[k] ? PP.fmtNum(rows.reduce((s, r) => s + PP.num(r[k]), 0), tot[k] === true ? 3 : tot[k]) : '') + '</td>'; });
            post.forEach(() => { f += '<td></td>'; });
            foot.innerHTML = f + '</tr>';
        } else foot.innerHTML = '';
        table._rows = rows;
        body.onclick = e => {
            const tr = e.target.closest('tr'); if (!tr) return; const i = Number(tr.dataset.i); const row = rows[i];
            body.querySelectorAll('tr.selected').forEach(x => x.classList.remove('selected')); tr.classList.add('selected'); table._current = row;
            const act = e.target.closest('[data-act]'); const lnk = e.target.closest('[data-link]');
            if (act) { e.stopPropagation(); [].concat(pre, post).find(b => b.key === act.dataset.act).onclick(row, act); return; }
            if (lnk && opts.onLink) { e.stopPropagation(); opts.onLink(lnk.dataset.link, row); return; }
            if (opts.onRow) opts.onRow(row, tr);
        };
        body.ondblclick = e => { const tr = e.target.closest('tr'); if (tr && opts.onDbl && !e.target.closest('[data-act]')) opts.onDbl(rows[Number(tr.dataset.i)]); };
    };
    /* UltraCombo.Leave: focus leaves the combo's wrap (the popup lives inside the wrap) */
    PP.onComboLeave = function (selectId, fn) {
        document.addEventListener('focusout', e => {
            const w = e.target && e.target.closest && e.target.closest('.dtcombo-wrap'); const sel = PP.$(selectId);
            if (w && sel && w.contains(sel) && !(e.relatedTarget && w.contains(e.relatedTarget))) fn();
        });
    };
    PP.confirm = msg => global.confirm(msg);
    global.PP = PP;
}(window));
