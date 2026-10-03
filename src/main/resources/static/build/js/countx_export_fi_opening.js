/* ============================================================================================
 * countx_export_fi_opening.js - frmFIOpening.cs (Architecture.WinApp.Export) "FI Opening Balances (Balance
 * Advance Payment)", pop-up of 952 GD / Bank Invoice Mapping. Data from /api/export/fi-opening.
 *
 * Desktop behaviour kept: the toolbar Save always inserts (RecId = 0); Update saves the opened row; only the
 * Exchange Rate's TextChanged recomputes Lcy Amount (FI Amount x rate, "#,#.####" - so 0 shows empty); opening a
 * history row fills Opening Date, FI Date, FI Number, FI Amount, Fcy, Exchange Rate, Lcy Amount and Remarks but
 * NOT the expiry date, bank or consignee (they keep what the form showed and are saved as shown); New clears the
 * boxes and combos but keeps the three dates. The pickers carry the time of day they were opened with (as a
 * DateTimePicker's Value does). ?id= opens that opening.
 * ============================================================================================ */
(function (global) {
    'use strict';
    var Q = global.ExportQ;
    var API = '/api/export/fi-opening';
    var box = Q.box, netI = Q.netI, netD = Q.netD, str = Q.str;

    var S = { history: [], currencies: [], banks: [], consignees: [], recId: 0, cur: -1, time: {} };
    var COLS = [{ key: 'OpeningDate', fmt: 'datetime', mmm: false }, { key: 'FIDate', fmt: 'datetime', mmm: false }, { key: 'FINumber', link: true },
        { key: 'FIAmount', fmt: 'num', sum: true }, { key: 'ExpiryDate', fmt: 'dmy' }, { key: 'FcyCode' }, { key: 'ExchangeRate', fmt: 'rate' },
        { key: 'LcyAmount', fmt: 'num', sum: true }, { key: 'BankName' }, { key: 'Consignee' }, { key: 'Remarks' }, { key: 'EntryUser' },
        { key: 'EntryDate', fmt: 'datetime', mmm: false }, { key: 'ModifyUser' }, { key: 'ModifyDate', fmt: 'datetime', mmm: false }];

    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function nowTime() { var d = new Date(); return pad(d.getHours()) + ':' + pad(d.getMinutes()) + ':' + pad(d.getSeconds()); }
    /* DateTimePicker.Value: the picked date with the time of day the picker holds. */
    function pickerValue(id) { var v = Q.val(id) || Q.today(); return v + 'T' + (S.time[id] || nowTime()); }
    function setPicker(id, iso) {
        var s = str(iso);
        Q.setText(id, Q.isoDate(s) || Q.today());
        var m = /T(\d{2}:\d{2}(:\d{2})?)/.exec(s);
        S.time[id] = m ? (m[1].length === 5 ? m[1] + ':00' : m[1]) : '00:00:00';
    }
    /* "#,#.####": no digits for 0, no leading zero, up to four decimals. */
    function fmtHash(v) {
        var n = netD(v); if (n === 0) return '';
        var s = Math.abs(n).toFixed(4).replace(/\.?0+$/, '');
        var p = s.split('.');
        p[0] = p[0] === '0' ? '' : p[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        return (n < 0 ? '-' : '') + p.join('.');
    }

    function render() {
        var rows = S.history.map(function (r) {
            var c = {}; for (var k in r) if (Object.prototype.hasOwnProperty.call(r, k)) c[k] = r[k];
            /* ExpiryDate "dd-MM-yyyy" */
            var d = Q.isoDate(r.ExpiryDate); c.ExpiryDateTxt = d ? d.substring(8, 10) + '-' + d.substring(5, 7) + '-' + d.substring(0, 4) : '';
            return c;
        });
        var cols = COLS.map(function (c) { return c.key === 'ExpiryDate' ? { key: 'ExpiryDateTxt' } : c; });
        Q.drawGrid('histBody', 'histFoot', rows, cols, S.cur);
        Q.show('histEmpty', S.history.length === 0);
    }
    function bindCombos() {
        Q.bind('CmbFcyCode', S.currencies, 'Id', 'CurrencyCode', ['CurrencyName', 'CurrencyRate', 'CurrencySymbol']);
        Q.bind('CmbBankName', S.banks, 'Id', 'BranchName', ['BankIBANNo']);
        Q.bind('CmbConsigneeName', S.consignees, 'Id', 'CompanyName', ['GlAccountCurrency']);
    }
    function applyCombos(d) {
        ['currencies', 'banks', 'consignees'].forEach(function (k) { if (d[k + 'Error']) box(d[k + 'Error']); });
        S.currencies = d.currencies || []; S.banks = d.banks || []; S.consignees = d.consignees || [];
        bindCombos();
    }
    function modeSave() { Q.show('btnSave', true); Q.show('btnupdate', false); }
    function modeUpdate() { Q.show('btnSave', false); Q.show('btnupdate', true); }

    /* InitializeComponentMethod. */
    function load() {
        var t = nowTime();
        ['datOpeningDate', 'datFIDate', 'datFiExpiryDate'].forEach(function (id) { Q.setText(id, Q.today()); S.time[id] = t; });
        return Q.getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.historyError) box('Error occurred during database call.');
            S.history = d.history || []; S.cur = -1; render();
            applyCombos(d);
            modeSave();
            var id = netI(Q.qs('id'));
            if (id > 0) { for (var i = 0; i < S.history.length; i++) if (netI(S.history[i].Id) === id) { S.cur = i; render(); readById(i); break; } }
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }

    /* ValidateInputs. */
    function validate() {
        if (!Q.val('txtFiNumber').trim()) { box('FI Number field is required'); Q.focus('txtFiNumber'); return false; }
        if (netD(Q.val('txtFiAmount')) === 0) { box('FIAmount field is required'); Q.focus('txtFiAmount'); return false; }
        if (!Q.hasSel('CmbFcyCode')) { box('Fcy Code field is required'); Q.focus('CmbFcyCode'); return false; }
        if (netD(Q.val('txtExchangeRate')) === 0) { box('Exchange Rate field is required'); Q.focus('txtExchangeRate'); return false; }
        if (netD(Q.val('txtLlcyAmount')) === 0) { box('LcyAmount field is required'); Q.focus('txtLlcyAmount'); return false; }
        return true;
    }
    function reloadHistory() {
        return Q.getJson(API + '/history').then(function (d) {
            if (d && d.historyError) { box(d.historyError); return; }
            S.history = (d && d.history) || []; S.cur = -1; render();
        }).catch(function (e) { box(e.message); });
    }
    /* Insert(). */
    function insert(btn) {
        return Q.busy(btn, function () {
            if (!validate()) return Promise.resolve();
            if (!Q.ask(S.recId > 0 ? 'Are you sure you want to update?' : 'Are you sure you want to save?')) return Promise.resolve();
            return Q.postJson(API + '/save', {
                recId: S.recId, openingDate: pickerValue('datOpeningDate'), fiDate: pickerValue('datFIDate'), expiryDate: pickerValue('datFiExpiryDate'),
                fiNumber: Q.val('txtFiNumber'), fiAmount: Q.val('txtFiAmount'), fcyId: netI(Q.val('CmbFcyCode')), exchangeRate: Q.val('txtExchangeRate'),
                lcyAmount: Q.val('txtLlcyAmount'), remarks: Q.val('txtRemarks'), bankId: netI(Q.val('CmbBankName')), consigneeId: netI(Q.val('CmbConsigneeName'))
            }).then(function (r) {
                box((r && r.message) || '');
                resetForm();
                return reloadHistory();
            });
        });
    }
    function toolSave(btn) { S.recId = 0; return insert(btn); }
    function toolUpdate(btn) { return insert(btn); }

    /* txtExchangeRate_TextChanged. */
    function exchangeChanged() { Q.setText('txtLlcyAmount', fmtHash(netD(Q.val('txtFiAmount').trim()) * netD(Q.val('txtExchangeRate').trim()))); }

    /* ReadById: from the grid's current row. */
    function readById(i) {
        var r = S.history[i]; if (!r) return;
        S.recId = netI(r.Id);
        setPicker('datOpeningDate', r.OpeningDate);
        setPicker('datFIDate', r.FIDate);
        Q.setText('txtFiNumber', str(r.FINumber));
        Q.setText('txtFiAmount', str(r.FIAmount));
        if (netI(r.FcyId) > 0) Q.setValOrAdd('CmbFcyCode', r.FcyId, r.FcyCode);
        Q.setText('txtExchangeRate', str(r.ExchangeRate)); exchangeChanged();
        Q.setText('txtLlcyAmount', str(r.LcyAmount));
        Q.setText('txtRemarks', str(r.Remarks));
        modeUpdate();
        Q.focus('datOpeningDate');
    }
    /* ResetForm (dates kept). */
    function resetForm() {
        S.recId = 0;
        ['txtFiAmount', 'txtFiNumber', 'txtRemarks', 'txtExchangeRate', 'txtLlcyAmount'].forEach(function (id) { Q.setText(id, ''); });
        Q.setVal('CmbFcyCode', '0'); Q.setVal('CmbBankName', '0'); Q.setVal('CmbConsigneeName', '0');
        modeSave();
    }
    /* btnRefresh_Click: global parties, currencies, banks, consignees re-read. */
    function refresh(btn) {
        return Q.busy(btn, function () {
            return Q.getJson(API + '/combos').then(function (d) { applyCombos(d || {}); });
        });
    }
    function shortcuts() {
        Q.shortcutPopup([['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
            ['Ctrl+F5', 'For Focus on ChargesCode'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'],
            ['Ctrl+ArrowUp', 'For Focus On ChargesCode'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record']]);
    }
    function history(btn) {
        return Q.busy(btn, function () {
            return reloadHistory().then(function () { var b = Q.$id('historyBar'); if (b) b.scrollIntoView({ behavior: 'smooth' }); });
        });
    }

    document.addEventListener('DOMContentLoaded', function () {
        Q.wireTabs(null, null);
        Q.wireGrid('histBody', { select: function (i) { S.cur = i; }, open: readById });
        Q.$id('grdfrm').addEventListener('keydown', function (e) {
            if (e.ctrlKey && e.key === 'Enter' && S.cur >= 0) { e.preventDefault(); e.stopPropagation(); readById(S.cur); }
        });
        Q.on('txtExchangeRate', 'input', exchangeChanged);
        /* DefineExportCharges_KeyDown (the handler name the desktop form kept). */
        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && e.altKey) { if (k === 'control' || k === 'alt') { shortcuts(); } return; }
            if (e.ctrlKey && k === 's') { e.preventDefault(); if (!Q.$id('btnSave').classList.contains('is-hidden')) toolSave(Q.$id('btnSave')); }
            else if (e.ctrlKey && k === 'u') { e.preventDefault(); if (!Q.$id('btnupdate').classList.contains('is-hidden')) toolUpdate(Q.$id('btnupdate')); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); resetForm(); }
            else if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); Q.cancel(); }
            else if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); Q.focus('datOpeningDate'); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); Q.$id('grdfrm').focus(); }
        });
        load();
    });

    global.ExportFiOpening = { resetForm: resetForm, toolSave: toolSave, toolUpdate: toolUpdate, refresh: refresh, shortcuts: shortcuts, history: history };
}(window));
