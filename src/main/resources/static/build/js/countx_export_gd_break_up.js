/* ============================================================================================
 * countx_export_gd_break_up.js - frmGdBreakUpByInvoice.cs (Architecture.WinApp.Export), screen 881
 * "Gd Break Up By Invoice". Two main tabs (GD BreakUp | Advance Utilized By GD), each with Form and
 * History. Every button, TextChanged, Leave, double-click and grid button of the desktop form has its
 * counterpart here, with the desktop's messages and order; all data comes from /api/export/gd-break-up
 * (ExportModuleController -> ExportGdBreakUpService -> the desktop's own procedures).
 *
 * Button contract on every action: disabled + spinner while the request runs, duplicates ignored,
 * re-enabled on success and on failure (busy()).
 * ============================================================================================ */
(function (global) {
    'use strict';

    var API = '/api/export/gd-break-up';

    function $id(id) { return document.getElementById(id); }
    function box(m) { global.alert(m); }
    function ask(m) { return global.confirm(m); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function str(v) { return v === null || v === undefined ? '' : String(v); }
    /* Conversion.ToInt / ToDouble - text with thousands separators parses, anything else is 0. */
    function netI(v) { var n = parseInt(String(v === null || v === undefined ? '' : v).replace(/,/g, ''), 10); return isFinite(n) ? n : 0; }
    function netD(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isFinite(n) ? n : 0; }
    /* ToString("#,##0.###") and the grid formats. */
    function fmt(v, dec) {
        var n = netD(v);
        if (dec === undefined) dec = 3;
        var s = n.toFixed(dec);
        if (dec > 0) s = s.replace(/\.?0+$/, '');
        var parts = s.split('.');
        parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        return parts.join('.');
    }
    function fmt2(v) { return fmt(v, 2); }   /* stringFormatsingle for "Amount" / "Freight" columns */
    function fmt4(v) { return fmt(v, 4); }   /* DecimalRateFormate for "Rate" columns */
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function today() { var d = new Date(); return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function daysAgo(n) { var d = new Date(); d.setDate(d.getDate() - n); return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function isoDate(v) {
        var s = str(v).trim();
        if (!s) return '';
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(s);
        if (m) return m[1] + '-' + m[2] + '-' + m[3];
        var d = new Date(s);
        return isNaN(d.getTime()) ? '' : d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate());
    }
    /* DateTime.ToShortDateString() under the desktop's culture: M/d/yyyy. */
    function shortDate(v) {
        var s = isoDate(v); if (!s) return '';
        return parseInt(s.substring(5, 7), 10) + '/' + parseInt(s.substring(8, 10), 10) + '/' + s.substring(0, 4);
    }
    function busy(btn, fn) {
        var b = (typeof btn === 'string') ? $id(btn) : btn;
        if (b) {
            if (b.disabled || b.classList.contains('is-busy')) return Promise.resolve();
            b.disabled = true; b.classList.add('is-busy');
        }
        var done = function () { if (b) { b.disabled = false; b.classList.remove('is-busy'); } };
        var p;
        try { p = fn(); } catch (e) { done(); box(e.message); return Promise.resolve(); }
        if (p && typeof p.then === 'function') return p.then(done, function (e) { done(); if (e && e.message) box(e.message); });
        done();
        return Promise.resolve(p);
    }
    function parse(r) {
        return r.text().then(function (t) {
            var b = null;
            try { b = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
            if (!r.ok) throw new Error((b && b.message) || ('Request failed (' + r.status + ')'));
            return b;
        });
    }
    function getJson(url) { return fetch(url, { headers: { 'Accept': 'application/json' }, credentials: 'same-origin' }).then(parse); }
    function postJson(url, data) {
        var h = { 'Accept': 'application/json', 'Content-Type': 'application/json' };
        var t = document.querySelector('meta[name="_csrf"]'), n = document.querySelector('meta[name="_csrf_header"]');
        if (t && n && t.getAttribute('content') && n.getAttribute('content')) h[n.getAttribute('content')] = t.getAttribute('content');
        return fetch(url, { method: 'POST', headers: h, credentials: 'same-origin', body: JSON.stringify(data) }).then(parse);
    }
    function refreshCombos() { if (global.DesktopCombo) global.DesktopCombo.refresh(); }
    function show(id, on) { var e = $id(id); if (e) e.classList.toggle('is-hidden', !on); }
    function col(row, name) {
        if (!row) return null;
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        var l = name.toLowerCase();
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === l) return row[k];
        return null;
    }
    /**
     * InfragisticsHelper.BindAndRetainSelection(cmb, dt, valueMember, displayMember, caption, AllColumns, keep,
     * insertDefaultRow:false): options from the rows, extra columns for the popup, the previous value kept
     * when it is still in the list. The empty first option is the combo with no active row.
     */
    function bind(id, rows, valueCol, textCol, extraCols) {
        var s = $id(id); if (!s) return;
        var keep = s.value;
        var h = '<option value="0"></option>';
        (rows || []).forEach(function (r) {
            var extra = (extraCols || []).map(function (k) { return str(col(r, k)).replace(/\|/g, '/'); }).join('|');
            h += '<option value="' + esc(col(r, valueCol)) + '" data-extra="' + esc(extra) + '">' + esc(col(r, textCol)) + '</option>';
        });
        s.innerHTML = h;
        s.value = keep;
        if (s.value !== keep) s.value = '0';
        refreshCombos();
    }
    function setVal(id, v) {
        var s = $id(id); if (!s) return;
        s.value = str(v);
        if (s.value !== str(v)) s.value = '0';
        refreshCombos();
    }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setText(id, v) { var e = $id(id); if (e) e.value = str(v); }
    function hasSel(id) { var s = $id(id); return !!s && s.value !== '0' && s.value !== ''; }
    function selText(id) { var s = $id(id); return s && s.selectedIndex >= 0 ? s.options[s.selectedIndex].textContent.trim() : ''; }
    function findRow(rows, id) { for (var i = 0; i < rows.length; i++) if (netI(col(rows[i], 'Id')) === netI(id)) return rows[i]; return null; }
    function setEnabled(id, on) { var e = $id(id); if (e) { e.disabled = !on; refreshCombos(); } }
    function focus(id) { var e = $id(id); if (!e) return; var w = e.closest && e.closest('.dtcombo-wrap'); var t = w ? w.querySelector('.dtcombo-input') : e; try { (t || e).focus(); } catch (x) { /* ignore */ } }
    function cancel() {
        global.close();
        setTimeout(function () { if (!global.closed) { if (history.length > 1) history.back(); else location.href = '/export'; } }, 150);
    }

    // ------------------------------------------------------------------------------ state

    var PERM = { Save: true, Update: true };
    var CFG = { defaultDaysToLessFromHistoryFromDate: 0, allowOneRowPerInvoiceOnGD: false };
    var INVOICES = [], BANKS = [], HIST_BANKS = [];
    var GD = { rows: [], removed: [], updateIndex: -1, recId: 0 };
    var HIST = [];
    var ADV_INVOICES = [], FIS = [], GDS = [];
    var ADV = { rows: [], removedIds: '', updateIndex: -1 };
    var ADV_HIST = [];
    var CUR = { gd: -1, hist: -1, adv: -1, advHist: -1 };

    // ------------------------------------------------------------------------------ tabs

    function mainTab(name) {
        document.querySelectorAll('.ex-main-tab').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-main') === name); });
        document.querySelectorAll('.ex-main-panel').forEach(function (p) { p.classList.toggle('is-active', p.id === name); });
        if (name === 'mainGd') focus('CmbInvoiceGdBreakUp');
    }
    function innerTab(group, panelId) {
        var tabs = document.querySelector('.win-tabs[data-tabs="' + group + '"]');
        if (!tabs) return;
        tabs.querySelectorAll('.win-tab').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab') === panelId); });
        var host = tabs.parentNode;
        Array.prototype.forEach.call(host.children, function (p) {
            if (p.classList.contains('win-tab-panel')) p.classList.toggle('is-active', p.id === panelId);
        });
        var footBtn = $id(group === 'gd' ? 'btnGdFooterHistory' : 'btnAdvFooterHistory');
        var onHist = /History$/.test(panelId);
        if (footBtn) {
            footBtn.querySelector('span').textContent = onHist ? 'Form' : 'History';
            footBtn.querySelector('i').className = onHist ? 'fa fa-file-text-o' : 'fa fa-history';
        }
        /* tabControl1_SelectedIndexChanged / tabControl3_SelectedIndexChanged */
        if (group === 'gd') { if (onHist) focus('FromDateGdBreakupHistory'); else focus('CmbInvoiceGdBreakUp'); }
        if (group === 'adv' && onHist) advHistoryLoad();
    }
    function activeInner(group) {
        var tabs = document.querySelector('.win-tabs[data-tabs="' + group + '"] .win-tab.is-active');
        return tabs ? tabs.getAttribute('data-tab') : '';
    }
    function toggleHistory(group) {
        var cur = activeInner(group);
        innerTab(group, group === 'gd' ? (cur === 'gdHistory' ? 'gdForm' : 'gdHistory') : (cur === 'advHistory' ? 'advForm' : 'advHistory'));
    }

    // ------------------------------------------------------------------------------ load

    /** InitializeComponentMethod. */
    function load() {
        return getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM = { Save: d.permissions.Save !== false, Update: d.permissions.Update !== false };
            if (d.config) CFG = d.config;
            $id('BtnSaveGdBreakUp').disabled = !PERM.Save;
            $id('BtnSaveAdvanceAgaintGD').disabled = !PERM.Save;
            ['invoices', 'banks', 'advInvoices', 'fis', 'historyBanks'].forEach(function (k) { if (d[k + 'Error']) box(d[k + 'Error']); });
            INVOICES = d.invoices || []; bindInvoices();
            BANKS = d.banks || []; bindBanks();
            ADV_INVOICES = d.advInvoices || []; bindAdvInvoices();
            FIS = d.fis || []; bindFis();
            HIST_BANKS = d.historyBanks || []; bindHistoryBanks();
            var days = netI(CFG.defaultDaysToLessFromHistoryFromDate);
            setText('FromDateGdBreakupHistory', daysAgo(days > 0 ? days : 3));
            setText('ToDateGdBreakupHistory', today());
            gdRender(); advRender();
            $id('gdFooterInfo').textContent = 'frmGdBreakUpByInvoice  -  Document Type 224';
            focus('CmbInvoiceGdBreakUp');
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }

    // ============================================================================== GD BreakUp - combos

    /* InvoicesNoBindGdBreakUp: Id / InvoiceNo / BankInvoiceAmount, all columns shown. */
    function bindInvoices() { bind('CmbInvoiceGdBreakUp', INVOICES, 'Id', 'InvoiceNo', ['BankInvoiceAmount']); invoiceChanged(); }
    /* BankBind: Home Country banks, Id / BranchName / BankIBANNo. */
    function bindBanks() { bind('CmbBankNameGdBreakUp', BANKS, 'Id', 'BranchName', ['BankIBANNo']); }
    function bindHistoryBanks() { bind('CmbBankGdBankHistory', HIST_BANKS, 'Id', 'name', []); }

    /* CmbInvoiceGdBreakUp_TextChanged: Invoice Balance = the active row's third cell (BankInvoiceAmount), else "0". */
    function invoiceChanged() {
        var r = findRow(INVOICES, val('CmbInvoiceGdBreakUp'));
        setText('txtInvoiceBalanceGdBreakUp', r ? str(col(r, 'BankInvoiceAmount')) : '0');
    }
    /* Edit mode: the desktop sets Value and Text straight on the combo, so the invoice shows even when the
       list (re-read with GdBreakUpRecId) does not carry it. The same here: the option is added if missing. */
    function setInvoice(id, text) {
        var s = $id('CmbInvoiceGdBreakUp');
        if (netI(id) > 0 && !findRow(INVOICES, id)) {
            INVOICES.push({ Id: netI(id), InvoiceNo: text, BankInvoiceAmount: 0 });
            bind('CmbInvoiceGdBreakUp', INVOICES, 'Id', 'InvoiceNo', ['BankInvoiceAmount']);
        }
        s.value = str(netI(id));
        refreshCombos();
        invoiceChanged();
    }

    // ============================================================================== GD BreakUp - calculations

    /* CalculateFobValue: FOB = GD Value - Freight  ("#,##0.###"). */
    function calcFob() { setText('txtFobValueGdBreakUp', fmt(netD(val('txtGDValueGdBreakUp')) - netD(val('txtFreightGdBreakUp')))); }
    /* CalculateFTTAmount: FTT % > 100 -> back to 10 with a warning; FTT Amount = FOB * FTT % / 100. */
    function calcFtt() {
        if (netD(val('txtFTTPrcntGdBreakUp')) > 100) {
            setText('txtFTTPrcntGdBreakUp', '10');
            box('Percent Cant be Greater Than 100');
        }
        setText('txtFttAmountGdBreakUp', fmt(netD(val('txtFobValueGdBreakUp')) * netD(val('txtFTTPrcntGdBreakUp')) / 100));
    }
    /* CalculateNetAmount: GD Value > 0 -> GD Value - FTT Amount, else 0. */
    function calcNet() {
        var gd = netD(val('txtGDValueGdBreakUp'));
        setText('txtNetAmountGdBreakUp', gd > 0 ? fmt(gd - netD(val('txtFttAmountGdBreakUp'))) : fmt(0));
    }
    /* The TextChanged chains: setting FOB re-fires FTT, setting FTT Amount re-fires Net (WinForms raises
       TextChanged on programmatic sets), so every entry point ends at Net Amount. */
    function onGdValue() { calcFob(); calcFtt(); calcNet(); }         /* txtGDValue_TextChanged */
    function onFreight() { calcFob(); calcFtt(); calcNet(); }         /* txtFreight_TextChanged */
    function onFttPercent() { calcFtt(); calcNet(); }                 /* txtFTTPrcnt_TextChanged */

    // ============================================================================== GD BreakUp - grid

    var GD_COLS = ['PartyInvoiceNo', 'GDNo', 'GDDate', 'InvoiceNo', 'InvoiceDate', 'BankName', 'DueDays', 'ExchangeRate', 'GDValue',
        'Freight', 'FOBValue', 'FTTPercent', 'FttAmount', 'NetAmount', 'CustomDutyPercent', 'CustomDutyAmount', 'SalesTaxPercent',
        'SalesTaxAmount', 'AdditionalCustomDutyPercent', 'AdditionalCustomDutyAmount', 'IncomeTaxPercent', 'IncomeTaxAmount', 'Remarks'];
    /* GridColumnSettings: doubles right-aligned with a Sum total - "Amount"/"Freight" in stringFormatsingle,
       "Rate" in DecimalRateFormate and no total, everything else "#,##0.###"; ints plain. */
    function cellFmt(name, v) {
        if (name === 'DueDays') return { v: str(netI(v)), num: true };
        if (/Rate/.test(name)) return { v: fmt4(v), num: true, noSum: true };
        if (/Amount|Freight/.test(name)) return { v: fmt2(v), num: true, sum: true };
        if (/Value|Percent/.test(name)) return { v: fmt(v), num: true, sum: true };
        if (/Date/.test(name)) return { v: shortDate(v) };
        return { v: str(v) };
    }
    function drawGrid(bodyId, footId, rows, cols, cur, leading, linkCol) {
        var body = $id(bodyId), foot = $id(footId);
        var sums = {};
        var hasLead = !!leading && rows.length > 0 && leading(rows[0], 0) !== '';
        body.innerHTML = rows.map(function (r, i) {
            var h = '<tr data-i="' + i + '"' + (i === cur ? ' class="is-current"' : '') + '>' + (leading ? leading(r, i) : '');
            cols.forEach(function (c) {
                var f = cellFmt(c, col(r, c));
                if (f.sum) sums[c] = (sums[c] || 0) + netD(col(r, c));
                var inner = esc(f.v);
                if (linkCol && c === linkCol && f.v !== '') inner = '<a class="win-code" data-i="' + i + '" href="javascript:void(0)">' + esc(f.v) + '</a>';
                h += '<td' + (f.num ? ' class="num"' : '') + '>' + inner + '</td>';
            });
            return h + '</tr>';
        }).join('');
        if (foot) {
            if (!rows.length) { foot.innerHTML = ''; return; }
            var t = '<tr>' + (hasLead ? '<td class="lbl">&Sigma;</td>' : '');
            cols.forEach(function (c, idx) {
                var f = cellFmt(c, 0);
                var v = f.sum ? cellFmt(c, sums[c] || 0).v : '';
                var cell = esc(v);
                if (idx === 0 && !hasLead) cell = '&Sigma;';
                t += '<td' + (f.num ? '' : ' class="lbl"') + '>' + cell + '</td>';
            });
            foot.innerHTML = t + '</tr>';
        }
    }
    function gdRender() {
        drawGrid('gdBody', 'gdFoot', GD.rows, GD_COLS, CUR.gd, function (r, i) {
            return '<td class="win-cell-btn"><button type="button" class="win-x" data-del="' + i + '" title="Delete">X</button></td>';
        }, 'GDNo');
    }

    /* GdBreakUpFormValidation - FormHelper.ValidateControls, in the desktop's order. */
    function gdFormValidation() {
        if (!hasSel('CmbInvoiceGdBreakUp')) { box('Invoice No field is required'); focus('CmbInvoiceGdBreakUp'); return false; }
        if (!val('txtGDNoGdBreakUp').trim()) { box('GD No field is required'); focus('txtGDNoGdBreakUp'); return false; }
        if (!val('txtBankInvoiceNoGDBreakUp').trim()) { box('Bank Invoice No field is required'); focus('txtBankInvoiceNoGDBreakUp'); return false; }
        if (!/^\s*[+-]?\d+\s*$/.test(val('txtDueDaysGdBreakUp')) || netI(val('txtDueDaysGdBreakUp')) === 0) { box('Due Days must be a non-zero number'); focus('txtDueDaysGdBreakUp'); return false; }
        if (netD(val('txtGDValueGdBreakUp')) === 0) { box('GD Value must be a non-zero number'); focus('txtGDValueGdBreakUp'); return false; }
        if (netD(val('txtFobValueGdBreakUp')) === 0) { box('FOB Value must be a non-zero number'); focus('txtFobValueGdBreakUp'); return false; }
        if (netD(val('txtNetAmountGdBreakUp')) === 0) { box('Net Amount must be a non-zero number'); focus('txtNetAmountGdBreakUp'); return false; }
        return true;
    }
    function gdRowFromFields(base) {
        var r = base || {};
        r.Id = r.Id || 0;
        r.InvoiceId = netI(val('CmbInvoiceGdBreakUp'));
        r.PartyInvoiceNo = selText('CmbInvoiceGdBreakUp');
        r.GDNo = val('txtGDNoGdBreakUp').trim();
        r.GDDate = val('datGDDateGdBreakUp') || today();
        r.InvoiceNo = val('txtBankInvoiceNoGDBreakUp').trim();
        r.InvoiceDate = val('datInvoiceDateGdBreakUp') || today();
        r.OtherAmount = netD(val('txtInvoiceBalanceGdBreakUp'));
        r.BankId = netI(val('CmbBankNameGdBreakUp'));
        r.BankName = selText('CmbBankNameGdBreakUp');
        r.DueDays = netI(val('txtDueDaysGdBreakUp'));
        r.ExchangeRate = netD(val('txtExchangeRateGdBreakUp'));
        r.GDValue = netD(val('txtGDValueGdBreakUp'));
        r.Freight = netD(val('txtFreightGdBreakUp'));
        r.FOBValue = netD(val('txtFobValueGdBreakUp'));
        r.FTTPercent = netD(val('txtFTTPrcntGdBreakUp'));
        r.FttAmount = netD(val('txtFttAmountGdBreakUp'));
        r.NetAmount = netD(val('txtNetAmountGdBreakUp'));
        r.CustomDutyPercent = netD(val('txtCustomDutyPrcntGdBreakUp'));
        r.CustomDutyAmount = netD(val('txtCustomDutyAmountGdBreakUp'));
        r.SalesTaxPercent = netD(val('txtSaleTaxtPrcntGdBreakUp'));
        r.SalesTaxAmount = netD(val('txtSaleTaxAmountGdBreakUp'));
        r.AdditionalCustomDutyPercent = netD(val('txtAdditionalDutyPrcntGdBreakUp'));
        r.AdditionalCustomDutyAmount = netD(val('txtAdditionalDutyAmountGdBreakUp'));
        r.IncomeTaxPercent = netD(val('txtIncomeTaxPrcntGdBreakUp'));
        r.IncomeTaxAmount = netD(val('txtIncomeTaxAmountGdBreakUp'));
        r.Remarks = val('txtRemarksGdBreakUp').trim();
        return r;
    }

    /* btnAddGDBreakUpDetail_Click. */
    function gdAddRow() {
        try {
            if (!gdFormValidation()) return;
            var inv = netI(val('CmbInvoiceGdBreakUp'));
            if (GD.rows.length > 0) {
                for (var i = 0; i < GD.rows.length; i++) {
                    if (netI(GD.rows[i].InvoiceId) !== inv) throw new Error('You can only add data against one invoice in the grid');
                }
                if (CFG.allowOneRowPerInvoiceOnGD) throw new Error('You can only one Row Per invoice in the grid');
            }
            if (CFG.allowOneRowPerInvoiceOnGD && netD(val('txtGDValueGdBreakUp')) !== netD(val('txtInvoiceBalanceGdBreakUp')))
                throw new Error('GD Value Should be Equal To Invoice Balance');
            GD.rows.push(gdRowFromFields({ Id: 0 }));
            CUR.gd = -1;
            gdRender();
            gdResetDetails();
            setEnabled('CmbInvoiceGdBreakUp', false);   /* CmbInvoiceGdBreakUp.ReadOnly = true */
        } catch (e) { box(e.message); }
    }
    /* grdGdBreakUp_DoubleClick. */
    function gdEditRow(i) {
        var item = GD.rows[i]; if (!item) return;
        GD.updateIndex = i;
        getJson(API + '/invoices?recId=' + GD.recId).then(function (rows) { INVOICES = rows || []; bindInvoices(); })
            .catch(function (e) { box(e.message); })
            .then(function () {
                setInvoice(item.InvoiceId, item.PartyInvoiceNo);
                setText('txtGDNoGdBreakUp', item.GDNo);
                setText('datGDDateGdBreakUp', isoDate(item.GDDate));
                setText('txtBankInvoiceNoGDBreakUp', item.InvoiceNo);
                setText('datInvoiceDateGdBreakUp', isoDate(item.InvoiceDate));
                setVal('CmbBankNameGdBreakUp', item.BankId);
                setText('txtRemarksGdBreakUp', item.Remarks);
                setText('txtDueDaysGdBreakUp', str(item.DueDays));
                setText('txtExchangeRateGdBreakUp', str(item.ExchangeRate));
                setText('txtGDValueGdBreakUp', fmt(item.GDValue));
                setText('txtFreightGdBreakUp', fmt(item.Freight));
                setText('txtFobValueGdBreakUp', fmt(item.FOBValue));
                setText('txtFTTPrcntGdBreakUp', netD(item.FTTPercent) > 0 ? fmt(item.FTTPercent) : '10');
                setText('txtFttAmountGdBreakUp', fmt(item.FttAmount));
                setText('txtNetAmountGdBreakUp', fmt(item.NetAmount));
                setText('txtCustomDutyPrcntGdBreakUp', fmt(item.CustomDutyPercent));
                setText('txtCustomDutyAmountGdBreakUp', fmt(item.CustomDutyAmount));
                setText('txtSaleTaxtPrcntGdBreakUp', fmt(item.SalesTaxPercent));
                setText('txtSaleTaxAmountGdBreakUp', fmt(item.SalesTaxAmount));
                setText('txtAdditionalDutyPrcntGdBreakUp', fmt(item.AdditionalCustomDutyPercent));
                setText('txtAdditionalDutyAmountGdBreakUp', fmt(item.AdditionalCustomDutyAmount));
                setText('txtIncomeTaxPrcntGdBreakUp', fmt(item.IncomeTaxPercent));
                setText('txtIncomeTaxAmountGdBreakUp', fmt(item.IncomeTaxAmount));
                show('btnAddGDBreakUpDetail', false); show('btnUpdateGDBreakUpDetail', true); show('btnCancelGDBreakUpDetail', true);
                focus('CmbInvoiceGdBreakUp');
            });
    }
    /* btnUpdateGDBreakUpDetail_Click. */
    function gdUpdateRow() {
        try {
            if (!gdFormValidation()) return;
            if (GD.updateIndex < 0 || !GD.rows[GD.updateIndex]) return;
            var inv = netI(val('CmbInvoiceGdBreakUp'));
            for (var i = 0; i < GD.rows.length; i++) {
                if (i !== GD.updateIndex && netI(GD.rows[i].InvoiceId) !== inv) throw new Error('You can only add data against one invoice in the grid');
            }
            if (CFG.allowOneRowPerInvoiceOnGD && netD(val('txtGDValueGdBreakUp')) !== netD(val('txtInvoiceBalanceGdBreakUp')))
                throw new Error('GD Value Should be Equal To Invoice Balance');
            var cur = GD.rows[GD.updateIndex];
            GD.rows[GD.updateIndex] = gdRowFromFields({ Id: cur.Id });
            gdRender();
            gdResetDetails();
            focus('CmbInvoiceGdBreakUp');
        } catch (e) { box(e.message); }
    }
    function gdCancelRow() { gdResetDetails(); }
    /* DeleteDetailRowGdBreakUp: a saved row asks, is remembered with ActionTypeId 3 and removed; a new row is just removed. */
    function gdDeleteRow(i) {
        var r = GD.rows[i]; if (!r) return;
        if (netI(r.Id) > 0) {
            if (!ask('Are you sure to Delete?')) return;
            var vd = {}; for (var k in r) if (Object.prototype.hasOwnProperty.call(r, k)) vd[k] = r[k];
            GD.removed.push(vd);
        }
        GD.rows.splice(i, 1);
        if (GD.updateIndex === i) gdResetDetails();
        CUR.gd = -1;
        gdRender();
    }
    /* ResetGdBreakUpDetails. */
    function gdResetDetails() {
        GD.updateIndex = -1;
        setText('txtGDNoGdBreakUp', ''); setText('txtBankInvoiceNoGDBreakUp', '');
        setVal('CmbBankNameGdBreakUp', '0');
        setText('txtRemarksGdBreakUp', ''); setText('txtDueDaysGdBreakUp', ''); setText('txtFreightGdBreakUp', '');
        setText('txtGDValueGdBreakUp', ''); setText('txtFobValueGdBreakUp', '');
        setText('txtFTTPrcntGdBreakUp', '0');
        setText('txtFttAmountGdBreakUp', ''); setText('txtNetAmountGdBreakUp', '');
        ['txtCustomDutyPrcntGdBreakUp', 'txtCustomDutyAmountGdBreakUp', 'txtSaleTaxtPrcntGdBreakUp', 'txtSaleTaxAmountGdBreakUp',
         'txtAdditionalDutyPrcntGdBreakUp', 'txtAdditionalDutyAmountGdBreakUp', 'txtIncomeTaxPrcntGdBreakUp', 'txtIncomeTaxAmountGdBreakUp'].forEach(function (id) { setText(id, ''); });
        show('btnAddGDBreakUpDetail', true); show('btnUpdateGDBreakUpDetail', false); show('btnCancelGDBreakUpDetail', false);
        focus('txtGDNoGdBreakUp');
    }
    /* FormReset. */
    function gdFormReset() {
        gdResetDetails();
        GD.rows = []; GD.removed = []; CUR.gd = -1;
        gdRender();
        setEnabled('CmbInvoiceGdBreakUp', true);
        GD.recId = 0;
        return getJson(API + '/invoices?recId=0').then(function (rows) { INVOICES = rows || []; bindInvoices(); setVal('CmbInvoiceGdBreakUp', '0'); invoiceChanged(); })
            .catch(function (e) { box(e.message); });
    }
    /* BtnNewGdBreakUp_Click. */
    function gdNew() { return gdFormReset(); }
    /* BtnRefreshGdBreakUp_Click. */
    function gdRefresh(btn) {
        return busy(btn, function () {
            return getJson(API + '/refresh?recId=' + GD.recId).then(function (d) {
                d = d || {};
                if (d.config) CFG = d.config;
                INVOICES = d.invoices || []; bindInvoices();
                BANKS = d.banks || []; bindBanks();
            });
        });
    }
    /* BtnSaveGdBreakUp_Click -> Insert(). */
    function gdSave(btn) {
        return busy(btn, function () {
            if (GD.rows.length === 0) { innerTab('gd', 'gdForm'); box('GDBreakUp Detail Record Not Found'); return Promise.resolve(); }
            if (!ask('Are you sure to Save?')) return Promise.resolve();
            return postJson(API + '/save', { rows: GD.rows, removed: GD.removed }).then(function (d) {
                box((d && d.message) || 'Record Save Successfully');
                return gdFormReset();
            }).catch(function (e) { box(e.message); });
        });
    }
    /* ReadyByIdGdBreak(InvoiceId) - from the History grid. */
    function gdReadById(invoiceId) {
        GD.recId = netI(invoiceId);
        return getJson(API + '/by-invoice?invoiceId=' + GD.recId).then(function (d) {
            d = d || {};
            innerTab('gd', 'gdForm');
            INVOICES = d.invoices || []; bindInvoices();
            GD.rows = d.rows || []; GD.removed = []; CUR.gd = -1;
            gdRender();
        }).catch(function (e) { box(e.message); });
    }

    // ============================================================================== GD BreakUp - history

    var HIST_COLS = ['InvoiceNo', 'GDNO', 'GDDate', 'BankInvoiceNo', 'BankInvoiceDate', 'BankName', 'DueDays', 'ExchangeRate', 'GDValue',
        'Freight', 'FobValue', 'FTTPercent', 'FTTAmount', 'NetAmount', 'Remarks'];
    function histRender() {
        var editable = PERM.Update;
        show('gdHistEditTh', editable);
        drawGrid('gdHistBody', 'gdHistFoot', HIST, HIST_COLS, CUR.hist, function (r, i) {
            return editable ? '<td class="win-cell-btn"><button type="button" class="win-edit" data-edit="' + i + '">Edit</button></td>' : '';
        }, 'InvoiceNo');
        show('gdHistEmpty', HIST.length === 0);
    }
    /* btnShowGdBreakUpHistory_Click -> FillGDFormHistory. */
    function gdHistoryShow(btn) {
        return busy(btn, function () {
            var q = [];
            if ($id('FromDateGdBreakupHistoryChk').checked && val('FromDateGdBreakupHistory')) q.push('fromDate=' + encodeURIComponent(val('FromDateGdBreakupHistory')));
            if ($id('ToDateGdBreakupHistoryChk').checked && val('ToDateGdBreakupHistory')) q.push('toDate=' + encodeURIComponent(val('ToDateGdBreakupHistory')));
            q.push('bankId=' + netI(val('CmbBankGdBankHistory')));
            return getJson(API + '/history?' + q.join('&')).then(function (rows) { HIST = rows || []; CUR.hist = -1; histRender(); })
                .catch(function (e) { box(e.message); });
        });
    }
    /* btnResetGdBreakupHistory_Click. */
    function gdHistoryReset() {
        setText('FromDateGdBreakupHistory', today()); setText('ToDateGdBreakupHistory', today());
        setVal('CmbBankGdBankHistory', '0');
        HIST = []; CUR.hist = -1; histRender(); show('gdHistEmpty', false);
    }
    /* btnRefreshGdBreakupHistory_Click. */
    function gdHistoryRefresh(btn) {
        return busy(btn, function () {
            return getJson(API + '/history-banks').then(function (rows) { HIST_BANKS = rows || []; bindHistoryBanks(); }).catch(function (e) { box(e.message); });
        });
    }

    // ============================================================================== Advance Utilized By GD

    function bindAdvInvoices() { bind('CmbInvoiceAdvanceUtilize', ADV_INVOICES, 'Id', 'InvoiceNo', []); }
    /* FinancialInstrumentBindAdvanceUtilize: EFormNo (400 wide) + BalFIAmount shown; PaymenttermId / DocumentTypeId hidden. */
    function bindFis() { bind('CmbFIAdvanceUtlize', FIS, 'Id', 'EFormNo', ['BalFIAmount']); fiChanged(); }
    /* GDsNoBindAdvanceUtilize: every column of the procedure's result. */
    function bindGds() { bind('CmbGdNoAdvanceUtilize', GDS, 'Id', 'GDNO', ['GDValue', 'UtilizeAmount', 'GDBalance', 'BankInvoiceNo', 'GdStepStatus']); gdNoChanged(); }
    /* CmbGdNoAdvanceUtilize_TextChanged: GD Value = the active row's GDBalance. */
    function gdNoChanged() { var r = findRow(GDS, val('CmbGdNoAdvanceUtilize')); setText('txtGdValueAdvanceUtlize', r ? str(col(r, 'GDBalance')) : ''); }
    /* CmbFIAdvanceUtlize_TextChanged / Leave: Balance = the active row's BalFIAmount. */
    function fiChanged() { var r = findRow(FIS, val('CmbFIAdvanceUtlize')); setText('txtFIBalanceAdvanceUtlize', r ? str(col(r, 'BalFIAmount')) : ''); }

    /* CmbInvoiceAdvanceUtilize_Leave: the GDs for the invoice; with an empty grid, the saved rows for it too. */
    function advInvoiceLeave() {
        var inv = netI(val('CmbInvoiceAdvanceUtilize'));
        var r = findRow(ADV_INVOICES, inv);
        var docType = r ? netI(col(r, 'DocumentTypeId')) : 0;
        return getJson(API + '/advance/by-invoice?invoiceId=' + inv + '&documentTypeId=' + docType).then(function (d) {
            d = d || {};
            GDS = d.gds || []; bindGds();
            if (inv > 0 && ADV.rows.length === 0) { ADV.rows = d.rows || []; CUR.adv = -1; advRender(); }
        }).catch(function (e) { box(e.message); });
    }
    var ADV_COLS = ['InvoiceNo', 'GDNo', 'FINo', 'UtilizeAmount'];
    function advRender() {
        drawGrid('advBody', 'advFoot', ADV.rows, ADV_COLS, CUR.adv, function (r, i) {
            return '<td class="win-cell-btn"><button type="button" class="win-x" data-del="' + i + '" title="Delete">X</button></td>';
        }, 'GDNo');
    }
    /* AdvanceUtilizeFormValidation. */
    function advFormValidation() {
        if (!hasSel('CmbGdNoAdvanceUtilize')) { box('GD No field is required'); focus('CmbGdNoAdvanceUtilize'); return false; }
        if (!hasSel('CmbFIAdvanceUtlize')) { box('FI No field is required'); focus('CmbFIAdvanceUtlize'); return false; }
        if (netD(val('txtAdvanceUtlize')) === 0) { box('Utilize Amount must be a non-zero number'); focus('txtAdvanceUtlize'); return false; }
        return true;
    }
    /* btnAddAdvanceUtlize_Click: the row's invoice comes from the GD row (ExImInvoiceId / BankInvoiceNo). */
    function advAddRow() {
        try {
            if (!advFormValidation()) return;
            var g = findRow(GDS, val('CmbGdNoAdvanceUtilize')) || {};
            ADV.rows.push({
                Id: 0, InvoiceId: netI(col(g, 'ExImInvoiceId')), InvoiceNo: str(col(g, 'BankInvoiceNo')),
                GDId: netI(val('CmbGdNoAdvanceUtilize')), GDNo: selText('CmbGdNoAdvanceUtilize'), DocumentTypeId: netI(col(g, 'DocumentTypeId')),
                FIId: netI(val('CmbFIAdvanceUtlize')), FINo: selText('CmbFIAdvanceUtlize'), UtilizeAmount: netD(val('txtAdvanceUtlize'))
            });
            CUR.adv = -1; advRender();
            advResetDetails();
        } catch (e) { box(e.message); }
    }
    /* grdAdvanceUtlize_DoubleClick. */
    function advEditRow(i) {
        var item = ADV.rows[i]; if (!item) return;
        ADV.updateIndex = i;
        setVal('CmbInvoiceAdvanceUtilize', item.InvoiceId);
        if (!findRow(GDS, item.GDId)) { GDS = GDS.concat([{ Id: item.GDId, GDNO: item.GDNo, GDBalance: '', ExImInvoiceId: item.InvoiceId, BankInvoiceNo: item.InvoiceNo, DocumentTypeId: item.DocumentTypeId }]); bindGds(); }
        setVal('CmbGdNoAdvanceUtilize', item.GDId); gdNoChanged();
        if (!findRow(FIS, item.FIId)) { FIS = FIS.concat([{ Id: item.FIId, EFormNo: item.FINo, BalFIAmount: '' }]); bindFis(); }
        setVal('CmbFIAdvanceUtlize', item.FIId); fiChanged();
        setText('txtAdvanceUtlize', str(item.UtilizeAmount));
        show('btnAddAdvanceAgaintGdDetail', false); show('btnUpdateAdvanceAgaintGdDetail', true); show('btnCancelAdvanceAgaintGdDetail', true);
        focus('CmbInvoiceAdvanceUtilize');
    }
    /* btnUpdateAdvanceUtilize_Click: here the invoice comes from the Invoice combo (as the desktop writes it). */
    function advUpdateRow() {
        try {
            if (!advFormValidation()) return;
            var r = ADV.rows[ADV.updateIndex]; if (!r) return;
            r.InvoiceId = netI(val('CmbInvoiceAdvanceUtilize')); r.InvoiceNo = selText('CmbInvoiceAdvanceUtilize');
            r.GDId = netI(val('CmbGdNoAdvanceUtilize')); r.GDNo = selText('CmbGdNoAdvanceUtilize');
            r.FIId = netI(val('CmbFIAdvanceUtlize')); r.FINo = selText('CmbFIAdvanceUtlize');
            r.UtilizeAmount = netD(val('txtAdvanceUtlize'));
            advRender();
            show('btnAddAdvanceAgaintGdDetail', true); show('btnUpdateAdvanceAgaintGdDetail', false); show('btnCancelAdvanceAgaintGdDetail', false);
            advResetDetails();
            focus('CmbInvoiceAdvanceUtilize');
        } catch (e) { box(e.message); }
    }
    /* btnCancelAdvanceUtilize_Click. */
    function advCancelRow() {
        ADV.updateIndex = -1;
        show('btnAddAdvanceAgaintGdDetail', true); show('btnUpdateAdvanceAgaintGdDetail', false); show('btnCancelAdvanceAgaintGdDetail', false);
    }
    /* grdAdvanceUtlize_ColumnButtonClick "Delete". */
    function advDeleteRow(i) {
        var r = ADV.rows[i]; if (!r) return;
        if (netI(r.Id) > 0) {
            if (!ask('Are you sure to Delete?')) return;
            ADV.removedIds = ADV.removedIds + ',' + str(r.Id);
        }
        ADV.rows.splice(i, 1);
        if (ADV.updateIndex === i) advCancelRow();
        CUR.adv = -1; advRender();
    }
    /* ResetGdAdvanceUtilizeDetails: amount cleared, focus GD No, FI list re-read. */
    function advResetDetails() {
        ADV.updateIndex = -1;
        setText('txtAdvanceUtlize', '');
        focus('CmbGdNoAdvanceUtilize');
        return getJson(API + '/advance/setup').then(function (d) { FIS = (d && d.fis) || []; bindFis(); }).catch(function (e) { box(e.message); });
    }
    /* BtnNewAdvanceAgaintGD_Click. */
    function advNew() {
        ADV.removedIds = '';
        setVal('CmbInvoiceAdvanceUtilize', '0'); setVal('CmbFIAdvanceUtlize', '0');
        setText('txtGdValueAdvanceUtlize', ''); setText('txtFIBalanceAdvanceUtlize', '');
        GDS = []; bindGds();
        show('btnAddAdvanceAgaintGdDetail', true); show('btnUpdateAdvanceAgaintGdDetail', false); show('btnCancelAdvanceAgaintGdDetail', false);
        ADV.rows = []; CUR.adv = -1; advRender();
        return advResetDetails();
    }
    /* BtnRefreshAdvanceAgaintGD_Click. */
    function advRefresh(btn) {
        return busy(btn, function () {
            return getJson(API + '/advance/setup').then(function (d) {
                d = d || {};
                ADV_INVOICES = d.advInvoices || []; bindAdvInvoices();
                FIS = d.fis || []; bindFis();
            }).catch(function (e) { box(e.message); });
        });
    }
    /* BtnSaveAdvanceAgaintGD_Click. */
    function advSave(btn) {
        return busy(btn, function () {
            if (ADV.rows.length <= 0) { box('Advance Utilize Grid Record not found'); return Promise.resolve(); }
            if (!ask('Are you sure to Save?')) return Promise.resolve();
            return postJson(API + '/advance/save', { rows: ADV.rows, removedIds: ADV.removedIds }).then(function (d) {
                box((d && d.message) || 'Record Save Successfully');
                ADV.rows = []; CUR.adv = -1; advRender();
                ADV.removedIds = '';
            }).catch(function (e) { box(e.message); });
        });
    }
    /* tabControl3_SelectedIndexChanged (History). */
    function advHistoryLoad() {
        return getJson(API + '/advance/history').then(function (rows) {
            ADV_HIST = rows || []; CUR.advHist = -1;
            drawGrid('advHistBody', 'advHistFoot', ADV_HIST, ADV_COLS, CUR.advHist, null, 'GDNo');
            show('advHistEmpty', ADV_HIST.length === 0);
        }).catch(function (e) { box(e.message); });
    }
    /* grdAdvanceHistory_DoubleClick: back to the Form tab with that one row in the grid. */
    function advHistoryPick(i) {
        var r = ADV_HIST[i]; if (!r) return;
        innerTab('adv', 'advForm');
        var c = {}; for (var k in r) if (Object.prototype.hasOwnProperty.call(r, k)) c[k] = r[k];
        ADV.rows = [c]; CUR.adv = -1; advRender();
    }

    // ============================================================================== wiring

    function wireGrid(bodyId, handlers) {
        var gb = $id(bodyId);
        gb.addEventListener('click', function (e) {
            var del = e.target.closest('button[data-del]');
            if (del) { handlers.del(+del.getAttribute('data-del')); return; }
            var ed = e.target.closest('button[data-edit]');
            if (ed) { handlers.open(+ed.getAttribute('data-edit')); return; }
            var lk = e.target.closest('a.win-code');
            if (lk) { handlers.open(+lk.getAttribute('data-i')); return; }
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            handlers.select(+tr.getAttribute('data-i'));
            gb.querySelectorAll('tr[data-i]').forEach(function (x) { x.classList.toggle('is-current', x === tr); });
        });
        gb.addEventListener('dblclick', function (e) {
            var tr = e.target.closest('tr[data-i]'); if (tr) handlers.open(+tr.getAttribute('data-i'));
        });
    }
    function on(id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); }

    document.addEventListener('DOMContentLoaded', function () {
        document.querySelectorAll('.ex-main-tab').forEach(function (b) { b.addEventListener('click', function () { mainTab(b.getAttribute('data-main')); }); });
        document.querySelectorAll('.win-tabs .win-tab').forEach(function (b) {
            b.addEventListener('click', function () { innerTab(b.closest('.win-tabs').getAttribute('data-tabs'), b.getAttribute('data-tab')); });
        });
        document.querySelectorAll('button[data-fullscreen]').forEach(function (b) {
            b.addEventListener('click', function () { var bx = $id(b.getAttribute('data-fullscreen')); if (bx) bx.classList.toggle('ex-fullscreen'); });
        });
        on('CmbInvoiceGdBreakUp', 'change', invoiceChanged);
        on('txtGDValueGdBreakUp', 'input', onGdValue);
        on('txtFreightGdBreakUp', 'input', onFreight);
        on('txtFTTPrcntGdBreakUp', 'input', onFttPercent);
        on('txtFTTPrcntGdBreakUp', 'change', onFttPercent);
        on('CmbInvoiceAdvanceUtilize', 'change', advInvoiceLeave);
        on('CmbGdNoAdvanceUtilize', 'change', gdNoChanged);
        on('CmbFIAdvanceUtlize', 'change', fiChanged);
        wireGrid('gdBody', { del: gdDeleteRow, open: gdEditRow, select: function (i) { CUR.gd = i; } });
        wireGrid('gdHistBody', { del: function () {}, open: function (i) { var r = HIST[i]; if (r) gdReadById(col(r, 'ExImInvoiceId')); }, select: function (i) { CUR.hist = i; } });
        wireGrid('advBody', { del: advDeleteRow, open: advEditRow, select: function (i) { CUR.adv = i; } });
        wireGrid('advHistBody', { del: function () {}, open: advHistoryPick, select: function (i) { CUR.advHist = i; } });
        /* ImProformaInvoice_KeyDown: Enter moves on, Ctrl+E / Esc close, Ctrl+N history reset, Ctrl+R history
           refresh, Ctrl+T next tab, Ctrl+Down / Ctrl+Up on the history tab. Ctrl+Enter / Ctrl+Space on a history row = Edit. */
        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase();
            var onGd = $id('mainGd').classList.contains('is-active');
            var onHist = onGd ? activeInner('gd') === 'gdHistory' : activeInner('adv') === 'advHistory';
            if (e.key === 'Enter' && !e.ctrlKey && e.target && /^(INPUT|SELECT)$/.test(e.target.tagName) && e.target.type !== 'checkbox') {
                var f = Array.prototype.filter.call(document.querySelectorAll('input:not([type=hidden]):not(:disabled), select:not(:disabled), button:not(:disabled), textarea'), function (x) { return x.offsetParent !== null; });
                var i = f.indexOf(e.target);
                if (i >= 0 && i + 1 < f.length) { e.preventDefault(); f[i + 1].focus(); }
                return;
            }
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); cancel(); return; }
            if (e.ctrlKey && k === 't') {
                e.preventDefault();
                if (onGd) innerTab('gd', activeInner('gd') === 'gdForm' ? 'gdHistory' : 'gdForm');
                else innerTab('adv', activeInner('adv') === 'advForm' ? 'advHistory' : 'advForm');
                return;
            }
            if (onGd && onHist) {
                if (e.ctrlKey && k === 'n') { e.preventDefault(); gdHistoryReset(); }
                if (e.ctrlKey && k === 'r') { e.preventDefault(); gdHistoryRefresh($id('btnRefreshGdBreakupHistory')); }
                if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); var tr = $id('gdHistBody').querySelector('tr'); if (tr) tr.scrollIntoView(); }
                if (e.ctrlKey && e.key === 'ArrowUp') { e.preventDefault(); focus('FromDateGdBreakupHistory'); }
                if (e.ctrlKey && (e.key === 'Enter' || e.key === ' ') && CUR.hist >= 0) { e.preventDefault(); var r = HIST[CUR.hist]; if (r) gdReadById(col(r, 'ExImInvoiceId')); }
            } else if (onGd && e.ctrlKey && k === 'r') { e.preventDefault(); gdRefresh($id('BtnRefreshGdBreakUp')); }
        });
        load();
    });

    global.ExportGd = {
        gdNew: gdNew, gdSave: gdSave, gdRefresh: gdRefresh, gdAddRow: gdAddRow, gdUpdateRow: gdUpdateRow, gdCancelRow: gdCancelRow,
        gdHistoryShow: gdHistoryShow, gdHistoryReset: gdHistoryReset, gdHistoryRefresh: gdHistoryRefresh,
        advNew: advNew, advSave: advSave, advRefresh: advRefresh, advAddRow: advAddRow, advUpdateRow: advUpdateRow, advCancelRow: advCancelRow,
        toggleHistory: toggleHistory
    };
}(window));
