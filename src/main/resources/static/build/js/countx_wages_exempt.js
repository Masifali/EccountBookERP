/* ============================================================================================
 * Wages Exempt Item Schedule - WagesExemptItemSchedule.cs (Architecture.WinApp.Lookups).
 * Opened from Stock Conversion's "Wages Exempt" button (invfrmStockConversionProduction.cs:7545,
 * new WagesExemptItemSchedule(UserAccount).Show()) in a new window.
 *
 * Load (PackingChangePriceSchedule_Load:103): ItemBind, WagesAccountBind, DocumentType, BindHistory,
 * Save shown / Update hidden. Save / Update: Insert():250 with FormValiadation, the date order and the
 * overlap check against the history grid, the desktop's confirmations and messages. Line numbers are
 * src280/WagesExemptItemSchedule.cs.
 *
 * Reproduced as written:
 *  - Reset() does not clear RecId or the two dates; btnUpdate (and Ctrl+U) runs Insert() with whatever
 *    RecId was last read, even after New.
 *  - the form's KeyDown (Ctrl+U / Ctrl+S / Ctrl+N / Ctrl+E / Esc) is wired without KeyPreview, so it
 *    only fires while the form itself (no control) has the focus.
 *  - the DateTimePickers keep the time of day they were created with (DateTime.Now); ReadById sets them
 *    from the grid's short-date text, i.e. midnight. That value, time included, is what is saved.
 * ============================================================================================ */
(function () {
    'use strict';

    var api = '/api/production/wages-exempt-item-schedule';

    function $id(id) { return document.getElementById(id); }
    function box(m) { window.alert(m); }
    function ask(m) { return window.confirm(m); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function col(row, name) {
        if (!row) return null;
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        var l = name.toLowerCase();
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === l) return row[k];
        return null;
    }
    /** Conversion.ToInt. */
    function netI(v) {
        if (v === null || v === undefined || v === '') return 0;
        if (typeof v === 'number') return isFinite(v) ? Math.round(v) : 0;
        var s = String(v).trim();
        return /^[+-]?\d+$/.test(s) ? parseInt(s, 10) : 0;
    }
    function pad(n) { return String(n).padStart(2, '0'); }
    function datePart(v) {
        if (v === null || v === undefined || v === '') return '';
        var m = String(v).match(/^(\d{4})-(\d{2})-(\d{2})/);
        return m ? m[1] + '-' + m[2] + '-' + m[3] : '';
    }
    /** Conversion.ToDateTime(x).ToShortDateString() - dd/MM/yyyy; DBNull -> DateTime.MinValue. */
    function shortDate(v) {
        var d = datePart(v);
        if (!d) return '01/01/0001';
        var p = d.split('-');
        return p[2] + '/' + p[1] + '/' + p[0];
    }
    /** Conversion.ToDateTime of that short-date text back to yyyy-MM-dd (midnight). */
    function fromShort(s) {
        var m = String(s || '').match(/^(\d{2})\/(\d{2})\/(\d{4})$/);
        return m ? m[3] + '-' + m[2] + '-' + m[1] : '0001-01-01';
    }
    function nowTime() { var d = new Date(); return pad(d.getHours()) + ':' + pad(d.getMinutes()) + ':' + pad(d.getSeconds()); }
    function todayIso() { var d = new Date(); return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }

    function busy(btn, fn) {
        var b = (typeof btn === 'string') ? $id(btn) : btn;
        if (b) {
            if (b.disabled || b.classList.contains('is-busy')) return;
            b.disabled = true; b.classList.add('is-busy');
        }
        var done = function () { if (b) { b.disabled = false; b.classList.remove('is-busy'); } };
        var p;
        try { p = fn(); } catch (e) { done(); throw e; }
        if (p && typeof p.then === 'function') p.then(done, done); else done();
        return p;
    }
    function parse(r) {
        return r.text().then(function (t) {
            var body = null;
            try { body = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
            if (!r.ok) throw new Error((body && body.message) || ('Request failed (' + r.status + ')'));
            return body;
        });
    }
    function getJson(url) { return fetch(url, { headers: { 'Accept': 'application/json' }, credentials: 'same-origin' }).then(parse); }
    function postJson(url, body) {
        var h = { 'Accept': 'application/json', 'Content-Type': 'application/json' };
        var t = document.querySelector('meta[name="_csrf"]'), n = document.querySelector('meta[name="_csrf_header"]');
        if (t && n && t.getAttribute('content') && n.getAttribute('content')) h[n.getAttribute('content')] = t.getAttribute('content');
        return fetch(url, { method: 'POST', headers: h, credentials: 'same-origin', body: JSON.stringify(body) }).then(parse);
    }
    function refreshCombos() { if (window.DesktopCombo) window.DesktopCombo.refresh(); }
    function show(id, on) { var e = $id(id); if (e) e.classList.toggle('is-hidden', !on); }

    function fill(id, rows, valueKey, textKey) {
        var sel = $id(id);
        if (!sel) return;
        var html = '<option value="0"></option>';
        (rows || []).forEach(function (r) { html += '<option value="' + esc(col(r, valueKey)) + '">' + esc(col(r, textKey)) + '</option>'; });
        sel.innerHTML = html;
        sel.value = '0';
    }
    function hasSel(id) { var s = $id(id); return !!s && s.value !== '0' && s.value !== ''; }
    function selText(id) { var s = $id(id); return (!s || s.selectedIndex < 0 || s.value === '0' || s.value === '') ? '' : s.options[s.selectedIndex].textContent; }
    /** UltraCombo.Text = x: the first row whose display text is x becomes active; no match -> no row. */
    function setText(id, text) {
        var s = $id(id), t = String(text === null || text === undefined ? '' : text);
        if (!s) return;
        if (t !== '' && s.value !== '0' && s.selectedIndex >= 0 && s.options[s.selectedIndex].textContent === t) return;
        for (var i = 0; i < s.options.length; i++) if (t !== '' && s.options[i].value !== '0' && s.options[i].textContent === t) { s.selectedIndex = i; return; }
        s.value = '0';
    }
    function setValue(id, v) {
        var s = $id(id), x = String(v);
        if (!s) return;
        for (var i = 0; i < s.options.length; i++) if (s.options[i].value === x) { s.selectedIndex = i; return; }
        s.value = '0';
    }

    // ------------------------------------------------------------------------------ state

    var RecId = 0;
    var dtHistory = [];         // the procedure's rows
    var GRID = [];              // the dt bound to Grd (BindHistory:180)
    var CUR = -1;
    /* the pickers' time of day: DateTime.Now when the form was built, midnight once ReadById set them */
    var TIME = { from: nowTime(), to: nowTime() };

    // ------------------------------------------------------------------------------ Load

    /** PackingChangePriceSchedule_Load:103. Each bind has its own try/catch -> MessageBox. */
    function load() {
        document.body.style.backgroundColor = 'teal';
        $id('EffectiveDateFrom').value = todayIso();
        $id('EffectiveDateTo').value = todayIso();
        return getJson(api + '/setup').then(function (d) {
            d = d || {};
            /* ItemBind:136 / WagesAccountBind:121 / DocumentType:152 - bound only when rows came back. */
            if (d.itemsError) box(d.itemsError); else if ((d.items || []).length) fill('cmbItem', d.items, 'Id', 'ItemName');
            if (d.wagesAccountsError) box(d.wagesAccountsError); else if ((d.wagesAccounts || []).length) fill('cmbWagesAccount', d.wagesAccounts, 'Id', 'WagesAccountName');
            if (d.documentTypesError) box(d.documentTypesError); else if ((d.documentTypes || []).length) fill('cmbRefDocType', d.documentTypes, 'Id', 'DocumentType');
            refreshCombos();
        }).catch(function (e) { box(e.message); }).then(bindHistory).then(function () {
            saveMode(true);
            $id('EffectiveDateFrom').focus();
        });
    }
    function saveMode(save) {
        show('btnUpdate', !save); show('btnsave', save);
        show('btnUpdateF', !save); show('BtnSavef', save);
    }

    // ---------------------------------------------------------------------------- history

    /** BindHistory:166 - WagesExemptItemSchedule.FormHistory (Org, Company). */
    function bindHistory() {
        return getJson(api + '/history').then(function (rows) {
            dtHistory = rows || [];
            if (dtHistory.length > 0) {
                GRID = dtHistory.map(function (r) {
                    return { Id: col(r, 'Id'), EffectedFrom: shortDate(col(r, 'EffectedFrom')), EffectedTo: shortDate(col(r, 'EffectedTo')),
                        DocumentTypeId: col(r, 'RefDocumentTypeId'), DocumentType: col(r, 'DocumentTypeDescription'), ItemId: col(r, 'ItemId'),
                        ItemName: col(r, 'ItemName'), WagesAccountId: col(r, 'WagesAccountId'), WagesAccount: col(r, 'WagesAccountName'),
                        EntryDate: shortDate(col(r, 'EntryDate')), EntryUser: col(r, 'EntryUser') };
                });
            } else {
                GRID = [];                                  // Grd.ClearStructure()
            }
            CUR = -1;
            drawHistory();
        }).catch(function (e) { box(e.message); });
    }
    /* grdSettings:210 - Id, ItemId, DocumentTypeId, WagesAccountId hidden; EffectedFrom/To captioned
       "Date From" / "Date To"; an "Edit" button column at position 0, frozen. */
    var HCOLS = [['EffectedFrom', 'Date From', 80], ['EffectedTo', 'Date To', 80], ['DocumentType', 'DocumentType', 150],
                 ['ItemName', 'ItemName', 150], ['WagesAccount', 'WagesAccount', 100], ['EntryDate', 'EntryDate', 80], ['EntryUser', 'EntryUser', 100]];
    function drawHistory() {
        if (!GRID.length) { $id('histHead').innerHTML = ''; $id('gridHistory').innerHTML = ''; return; }
        $id('histHead').innerHTML = '<th style="width:40px;">Edit</th>' + HCOLS.map(function (c) { return '<th style="min-width:' + c[2] + 'px;">' + esc(c[1]) + '</th>'; }).join('');
        $id('gridHistory').innerHTML = GRID.map(function (r, i) {
            return '<tr data-i="' + i + '" tabindex="-1"' + (i === CUR ? ' class="is-current"' : '') + '><td><button type="button" class="we-edit" data-act="edit">Edit</button></td>'
                + HCOLS.map(function (c) { var v = r[c[0]]; return '<td>' + esc(v === null || v === undefined ? '' : v) + '</td>'; }).join('') + '</tr>';
        }).join('');
    }

    /** ReadById:232 (Grd_DoubleClick / the Edit button). */
    function readById(i) {
        try {
            var item = GRID[i];
            if (!item) return;
            RecId = netI(item.Id);
            $id('EffectiveDateFrom').value = fromShort(item.EffectedFrom); TIME.from = '00:00:00';
            $id('EffectiveDateTo').value = fromShort(item.EffectedTo); TIME.to = '00:00:00';
            setValue('cmbRefDocType', netI(item.DocumentTypeId));
            setText('cmbRefDocType', item.DocumentType);
            setValue('cmbItem', netI(item.ItemId));
            setText('cmbItem', item.ItemName);
            setText('cmbWagesAccount', item.WagesAccount);
            refreshCombos();
            saveMode(false);
        } catch (e) { box(e.message); }
    }

    // ------------------------------------------------------------------------ Save / Update

    /** FormValiadation:273 - ActiveRow null. */
    function formValiadation() {
        if (!hasSel('cmbRefDocType')) { box('Document Type Field Is Required'); $id('cmbRefDocType').focus(); return false; }
        if (!hasSel('cmbItem') && !hasSel('cmbWagesAccount')) { box('ItemName Or WagesAccount Field Is Required'); $id('cmbItem').focus(); return false; }
        return true;
    }
    function pickerDate(id) { return $id(id).value || '0001-01-01'; }

    /** Insert():300. */
    function insert(btn) {
        try {
            if (!formValiadation()) return;
            if (RecId > 0) { if (!ask('Are you sure to Update?')) return; }
            else if (!ask('Are you sure to Save?')) return;
            var from = pickerDate('EffectiveDateFrom'), to = pickerDate('EffectiveDateTo');
            if (to < from) throw new Error("Effective Date From Can't Be Greater Than Effective Date To");
            if (dtHistory.length > 0) {
                var itemId = netI($id('cmbItem').value), docId = netI($id('cmbRefDocType').value), wagesId = netI($id('cmbWagesAccount').value);
                GRID.forEach(function (item) {
                    if (RecId !== netI(item.Id) && itemId === netI(item.ItemId) && docId === netI(item.DocumentTypeId) && wagesId === netI(item.WagesAccountId)) {
                        var f = fromShort(item.EffectedFrom), t = fromShort(item.EffectedTo);
                        if (f <= from && from <= t) throw new Error('Effective Date From Of Item ' + selText('cmbItem') + ' Is Between The Range Of Already Defined ');
                        if (f <= to && to <= t) throw new Error('Effective Date To Of Item ' + selText('cmbItem') + ' Is Between The Range Of Already Defined ');
                    }
                });
            }
            var wasRec = RecId;
            return busy(btn, function () {
                return postJson(api + '/save', {
                    id: RecId,
                    effectedFrom: from + 'T' + TIME.from, effectedTo: to + 'T' + TIME.to,
                    refDocumentTypeId: netI($id('cmbRefDocType').value), itemId: netI($id('cmbItem').value),
                    wagesAccountId: netI($id('cmbWagesAccount').value)
                }).then(function (d) {
                    var id = netI(d && d.id);
                    if (wasRec === 0 && id > 0) box('Save Successfully');
                    else if (wasRec > 0) box('Update Successfully');
                    reset();
                    return bindHistory();
                }).catch(function (e) { box(e.message); });
            });
        } catch (e) { box(e.message); }
    }
    /** btnsave_Click:285 - RecId = 0, then Insert(). */
    function btnsave(btn) { RecId = 0; return insert(btn || 'btnsave'); }
    /** btnUpdate_Click:390 - Insert() with the RecId as it stands. */
    function btnUpdate(btn) { return insert(btn || 'btnUpdate'); }
    /** btnnew_Click:402. */
    function btnnew() { reset(); return busy('btnnew', bindHistory); }
    /** Reset:415 - RecId and the dates are left as they are. */
    function reset() {
        $id('EffectiveDateFrom').focus();
        setText('cmbRefDocType', ''); setText('cmbItem', ''); setText('cmbWagesAccount', '');
        refreshCombos();
        saveMode(true);
    }
    /** BtnCancel_Click:434 - Close(). */
    function cancel() {
        window.close();
        /* a window the script did not open cannot close itself; go back instead */
        setTimeout(function () { if (!window.closed) { if (history.length > 1) history.back(); else location.href = '/production'; } }, 150);
    }

    // ------------------------------------------------------------------------------ boot

    function boot() {
        var hb = $id('gridHistory');
        hb.addEventListener('click', function (e) {
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            var i = +tr.getAttribute('data-i');
            CUR = i;
            hb.querySelectorAll('tr[data-i]').forEach(function (x) { x.classList.toggle('is-current', x === tr); });
            if (e.target.closest('[data-act="edit"]')) readById(i);          // Grd_ColumnButtonClick:264
        });
        hb.addEventListener('dblclick', function (e) {                         // Grd_DoubleClick:222
            var tr = e.target.closest('tr[data-i]'); if (tr && !e.target.closest('button')) readById(+tr.getAttribute('data-i'));
        });
        /* The pickers keep their time of day when the date changes. */
        /* WagesExemptItemSchedule_KeyDown:455 - no KeyPreview: only while no control has the focus. */
        document.addEventListener('keydown', function (e) {
            var a = document.activeElement;
            if (a && a !== document.body && a !== document.documentElement) return;
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 'u') { e.preventDefault(); btnUpdate(); }
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); cancel(); }
            if (e.ctrlKey && k === 'n') { e.preventDefault(); btnnew(); }
            if (e.ctrlKey && k === 's') { e.preventDefault(); btnsave(); }
        });
        load();
    }

    window.WagesExempt = { btnnew: btnnew, btnsave: btnsave, btnUpdate: btnUpdate, cancel: cancel, readById: readById };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
    else boot();
}());
