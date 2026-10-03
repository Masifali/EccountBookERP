/* ============================================================================================
 * countx_export_gd_break_up_manual.js - GdBreakUpManual.cs (Architecture.WinApp.Export), screen 198
 * "GD Break Up Manual". Every desktop event has its counterpart here with the desktop's texts;
 * data through /api/export/gd-break-up-manual (ExportBankGdController -> ExportGdBreakUpManualService).
 * ============================================================================================ */
(function (global) {
    'use strict';
    var X = global.ExBG, $id = X.$id, box = X.box, ask = X.ask, val = X.val, setText = X.setText, netD = X.netD, netI = X.netI, fmt = X.fmt, str = X.str, col = X.col;
    var API = '/api/export/gd-break-up-manual';

    var PERM = { Save: true, Update: true };
    var BANKS = [], HIST = [], RecId = 0, CUR = -1;

    // ------------------------------------------------------------------ load / bind
    /** GdBreakUpManual_Load: BankBind + HistoryBind. */
    function load() {
        return X.getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM = { Save: d.permissions.Save !== false, Update: d.permissions.Update !== false };
            $id('btnsave').disabled = !PERM.Save;
            $id('btnupdate').disabled = !PERM.Update;
            ['banks', 'history'].forEach(function (k) { if (d[k + 'Error']) box(d[k + 'Error']); });
            BANKS = d.banks || []; X.bind('CmbBankName', BANKS, 'Id', 'BranchName', []);
            HIST = d.history || []; render();
            setText('datGDDateGDBreakUp', X.today()); setText('datInvoiceDateGdBreakUp', X.today());
            $id('gbmFooterInfo').textContent = 'GdBreakUpManual  -  Document Type 3';
            X.focus('txtGDNoBreakUp');
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }
    /* grdSettingsGdBreakUp: doubles right aligned "#,##0.###"; GDValue / Freight / FOBValue / FttAmount / NetAmount summed. */
    var COLS = [
        'BankInvoiceNo', { key: 'BankInvoiceDate', fmt: X.shortDate }, { key: 'GDNO', link: true }, { key: 'GDDate', fmt: X.shortDate },
        { key: 'GDValue', num: true, sum: true, fmt: fmt }, 'BankName', { key: 'DueDays', num: true, fmt: function (v) { return str(netI(v)); } },
        { key: 'ExchangeRate', num: true, fmt: fmt }, { key: 'Freight', num: true, sum: true, fmt: fmt }, { key: 'FobValue', num: true, sum: true, fmt: fmt },
        { key: 'FTTPercent', num: true, fmt: fmt }, { key: 'FttAmount', num: true, sum: true, fmt: fmt }, { key: 'NetAmount', num: true, sum: true, fmt: fmt },
        'Remarks', { key: 'EntryDate', fmt: X.ddMMyyyyHm }, 'EntryUser', { key: 'ModifyDate', fmt: X.ddMMyyyyHm }, 'ModifyUser'
    ];
    function render() { X.drawGrid('gbmBody', 'gbmFoot', HIST, COLS, { cur: CUR, empty: 'gbmEmpty' }); }

    // ------------------------------------------------------------------ calculations (TextChanged chain)
    /* CalculateFobValue: Freight > GD Value -> exception "Freight Cant be Greater Than GD Value" (txtGDValue_TextChanged /
       txtFreight_TextChanged rethrow it; the desktop shows it as an unhandled-exception box); FOB = GD Value - Freight. */
    function calcFob() {
        if (netD(val('txtFreight')) > netD(val('txtGDValue'))) throw new Error('Freight Cant be Greater Than GD Value');
        setText('txtFobValue', fmt(netD(val('txtGDValue')) - netD(val('txtFreight'))));
    }
    /* CalculateFTTAmount: FTT % > 100 -> "10" with a warning; FTT Amount = FOB * FTT % / 100. */
    function calcFtt() {
        if (netD(val('txtFTTPrcnt')) > 100) { setText('txtFTTPrcnt', '10'); box('Percent Cant be Greater Than 100'); }
        setText('txtFttAmount', fmt(netD(val('txtFobValue')) * netD(val('txtFTTPrcnt')) / 100));
    }
    /* CalculateNetAmount: both GD Value and FTT Amount > 0 -> GD Value - FTT Amount, else "0". */
    function calcNet() {
        var gd = netD(val('txtGDValue')), ftt = netD(val('txtFttAmount'));
        setText('txtNetAmount', gd > 0 && ftt > 0 ? fmt(gd - ftt) : fmt(0));
    }
    /* WinForms raises TextChanged on programmatic sets: FOB -> FTT -> Net; each entry point ends at Net Amount. */
    function onGdValue() { try { calcFob(); calcFtt(); calcNet(); } catch (e) { box(e.message); } }
    function onFreight() { try { calcFob(); calcFtt(); calcNet(); } catch (e) { box(e.message); } }
    function onFttPercent() { try { calcFtt(); calcNet(); } catch (e) { box(e.message); } }

    // ------------------------------------------------------------------ form
    /* Reset: Save visible, Update hidden, RecId 0, fields cleared (FTT % back to 10), focus GD No, HistoryBind. */
    function reset() {
        X.show('btnsave', true); X.show('btnupdate', false);
        RecId = 0; CUR = -1;
        X.setVal('CmbBankName', '0');
        ['txtGDNoBreakUp', 'txtInvoiceNoGDBreakUp', 'txtFreight', 'txtGDValue', 'txtExchangeRate', 'txtDueDays', 'txtFobValue', 'txtFttAmount', 'txtNetAmount', 'txtRemarks'].forEach(function (id) { setText(id, ''); });
        setText('txtFTTPrcnt', '10');
        X.focus('txtGDNoBreakUp');
        return X.getJson(API + '/history').then(function (rows) { HIST = rows || []; render(); }).catch(function (e) { box(e.message); });
    }
    /* GdBreakUpFormValidation - desktop order and texts. */
    function formValidation() {
        if (!val('txtGDNoBreakUp')) { box('GdNo field is required'); X.focus('txtGDNoBreakUp'); return false; }
        if (!val('txtInvoiceNoGDBreakUp')) { box('BankInvoiceNo field is required'); X.focus('txtInvoiceNoGDBreakUp'); return false; }
        if (netI(val('txtDueDays').trim()) <= 0) { box('Due Days field is Required'); X.focus('txtDueDays'); return false; }
        if (netD(val('txtGDValue').trim()) <= 0) { box('GD Value field is Required'); X.focus('txtGDValue'); return false; }
        if (netD(val('txtFobValue').trim()) <= 0) { box('FOb Value field is Required'); X.focus('txtFobValue'); return false; }
        if (netD(val('txtNetAmount').trim()) <= 0) { box('Net Amount field is Required'); X.focus('txtNetAmount'); return false; }
        return true;
    }
    /* Insert(): the confirmation comes BEFORE the validation on the desktop. */
    function insert(btn) {
        return X.busy(btn, function () {
            if (!ask(RecId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return Promise.resolve();
            if (!formValidation()) return Promise.resolve();
            var body = {
                recId: RecId, gdNo: val('txtGDNoBreakUp').trim(), gdDate: val('datGDDateGDBreakUp') || X.today(),
                bankInvoiceNo: val('txtInvoiceNoGDBreakUp').trim(), bankInvoiceDate: val('datInvoiceDateGdBreakUp') || X.today(),
                bankId: netI(val('CmbBankName')), dueDays: netI(val('txtDueDays')), exchangeRate: netD(val('txtExchangeRate')),
                gdValue: netD(val('txtGDValue')), freight: netD(val('txtFreight')), fobValue: netD(val('txtFobValue')),
                fttPercent: netD(val('txtFTTPrcnt')), fttAmount: netD(val('txtFttAmount')), netAmount: netD(val('txtNetAmount')),
                remarks: val('txtRemarks')
            };
            return X.postJson(API + '/save', body).then(function (d) {
                box((d && d.message) || (RecId === 0 ? 'Save SuccessFully' : 'Update SuccessFully'));
                return reset();
            }).catch(function (e) { box(e.message); });
        });
    }
    /* btnsave_Click: RecId = 0 then Insert. */
    function save(btn) { RecId = 0; return insert(btn); }
    /* btnupdate_Click: "RecId not found" when nothing was loaded. */
    function update(btn) { if (RecId === 0) { box('RecId not found'); return Promise.resolve(); } return insert(btn); }
    /* grdGdBreakUp_DoubleClick -> GetByID into the entry, Update mode. */
    function open(i) {
        var r = HIST[i]; if (!r) return;
        X.show('btnsave', false); X.show('btnupdate', true);
        RecId = netI(col(r, 'Id'));
        return X.getJson(API + '/by-id?id=' + RecId).then(function (o) {
            o = o || {};
            setText('txtGDNoBreakUp', o.GDNO); setText('datGDDateGDBreakUp', X.isoDate(o.GDDate));
            setText('txtInvoiceNoGDBreakUp', o.BankInvoiceNo); setText('datInvoiceDateGdBreakUp', X.isoDate(o.BankInvoiceDate));
            X.setVal('CmbBankName', o.BankId);
            setText('txtDueDays', str(o.DueDays)); setText('txtExchangeRate', str(o.ExchangeRate));
            setText('txtGDValue', fmt(o.GDValue)); setText('txtFreight', fmt(o.Freight)); setText('txtFobValue', fmt(o.FobValue));
            setText('txtFTTPrcnt', netD(o.CommPercent) > 0 ? fmt(o.CommPercent) : '10');
            setText('txtFttAmount', fmt(o.CommAmount)); setText('txtNetAmount', fmt(o.NetToBeRealized));
            setText('txtRemarks', o.Remarks);
            X.focus('txtGDNoBreakUp');
        }).catch(function (e) { box(e.message); });
    }
    function historyRefresh(btn) {
        return X.busy(btn, function () {
            return X.getJson(API + '/history').then(function (rows) { HIST = rows || []; render(); $id('grdGdBreakUpBox').scrollIntoView({ behavior: 'smooth' }); }).catch(function (e) { box(e.message); });
        });
    }

    document.addEventListener('DOMContentLoaded', function () {
        X.wireTabs(function () { /* one tab */ });
        X.on('txtGDValue', 'input', onGdValue);
        X.on('txtFreight', 'input', onFreight);
        X.on('txtFTTPrcnt', 'input', onFttPercent);
        X.on('txtFTTPrcnt', 'change', onFttPercent);
        X.wireGrid('gbmBody', { open: open, select: function (i) { CUR = i; } });
        /* GdBreakUpManual_KeyDown */
        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase();
            if (X.enterMovesOn(e)) return;
            if (e.ctrlKey && k === 's') { e.preventDefault(); insert($id(RecId === 0 ? 'btnsave' : 'btnupdate')); return; }
            if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); return; }
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); X.cancel(); return; }
            if (e.ctrlKey && k === 'u') { e.preventDefault(); update($id('btnupdate')); }
        });
        load();
    });

    global.ExportGdBreakUpManual = { reset: reset, save: save, update: update, historyRefresh: historyRefresh };
}(window));
