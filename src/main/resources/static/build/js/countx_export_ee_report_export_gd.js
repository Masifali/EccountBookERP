/* countx_export_ee_report_export_gd.js - EEReport_ExportGD.cs, screen 257 "EEReport". Data: /api/export/ee-report-export-gd.
 * Detail grid (Activity "Detail") is editable in FEReturnPageNo, ITRSNo, FEReturnMonth, Schedule, SalesTerm and EERemarks;
 * leaving an edited cell saves one EEStatement (grdfrm_CellUpdated -> USP_EEStatement_InsertAndUpdate). Summary is read-only. */
(function (global) {
    'use strict';
    var H = global.ExRptC, API = '/api/export/ee-report-export-gd';
    var YEAR_START = '', ROWS = [], CUR = -1, MODE = 'Detail', MONTHS = [], TERMS = [], SCHEDULES = [], CAN_SAVE = true;
    /* the month picked from the list in this session, by row index (FEReturnMonth cell Value = MonthId after a pick) */
    var PICKED = {};

    function inputCell(r, i, key, kind) {
        if (MODE !== 'Detail' || !CAN_SAVE) return H.esc(H.str(H.col(r, key)));
        var v = H.esc(H.str(H.col(r, key)));
        if (kind === 'num') return '<input type="text" class="num ee-edit" data-i="' + i + '" data-key="' + key + '" value="' + v + '" data-guard="integer"/>';
        if (kind === 'list') {
            var list = key === 'Schedule' ? SCHEDULES : TERMS;
            return '<input type="text" class="ee-edit" list="ee-list-' + key + '" data-i="' + i + '" data-key="' + key + '" value="' + v + '"/>';
        }
        return '<input type="text" class="ee-edit" data-i="' + i + '" data-key="' + key + '" value="' + v + '"/>';
    }
    /* FEReturnMonth: value list MonthId -> "MMM yyyy" (LimitToList); the stored text shows as-is until a pick. */
    function monthCell(r, i) {
        var cur = H.str(H.col(r, 'FEReturnMonth'));
        if (MODE !== 'Detail' || !CAN_SAVE) return H.esc(cur);
        var picked = PICKED[i];
        var h = '<select class="ee-edit" data-i="' + i + '" data-key="FEReturnMonth">';
        var inList = MONTHS.some(function (m) { return String(m.MonthId) === String(picked); });
        h += '<option value=""' + (!inList && !cur ? ' selected' : '') + '></option>';
        if (cur && !inList) h += '<option value="__raw__" selected>' + H.esc(cur) + '</option>';
        MONTHS.forEach(function (m) { h += '<option value="' + m.MonthId + '"' + (inList && String(m.MonthId) === String(picked) ? ' selected' : '') + '>' + H.esc(m.Month) + '</option>'; });
        return h + '</select>';
    }
    var DETAIL_COLS = [
        { key: 'Id', hidden: true }, { key: 'FcyBankReceiptId', hidden: true }, { key: 'Sr.No.', cap: 'Sr.No.' }, { key: 'GdRefDocTypeId', hidden: true }, { key: 'GDId', hidden: true },
        { key: 'GdNo', cap: 'Gd No' }, { key: 'Consignee', cap: 'Consignee' }, { key: 'Commodity', cap: 'Commodity' }, { key: 'HsCode', cap: 'Hs Code' }, { key: 'ContractNo', cap: 'Contract No' },
        { key: 'ContractDate', cap: 'Contract Date', kind: 'date' }, { key: 'ContractCurrency', cap: 'Contract Currency' }, { key: 'ContractAmount', cap: 'Contract Amount', kind: 'amt' },
        { key: 'DateOfShipment', cap: 'Date Of Shipment', kind: 'date' }, { key: 'DateOfNegotion', cap: 'Date Of Negotion', kind: 'date' }, { key: 'RealizedCurrency', cap: 'Realized Currency' },
        { key: 'RealizedAmount', cap: 'Realized Amount', kind: 'amt', sum: true }, { key: 'FTT/Comm', hidden: true }, { key: 'ExchangeRate', cap: 'Exchange Rate', kind: 'rate' },
        { key: 'RealizedAmountInPKR', cap: 'Realized Amount In PKR', kind: 'amt', sum: true }, { key: 'DateofRealization', cap: 'Date of Realization', kind: 'date' },
        { key: 'MultiCurrencyId', hidden: true }, { key: 'EEId', hidden: true },
        { key: 'FEReturnPageNo', cap: 'FE Return Page No', render: function (r, i) { return inputCell(r, i, 'FEReturnPageNo', 'num'); }, cls: function () { return MODE === 'Detail' && CAN_SAVE ? 'win-editable' : ''; } },
        { key: 'ITRSNo', cap: 'ITRS No', render: function (r, i) { return inputCell(r, i, 'ITRSNo', 'num'); }, cls: function () { return MODE === 'Detail' && CAN_SAVE ? 'win-editable' : ''; } },
        { key: 'FEReturnMonth', cap: 'FEReturnMonth', render: monthCell, cls: function () { return MODE === 'Detail' && CAN_SAVE ? 'win-editable' : ''; } },
        { key: 'Schedule', cap: 'Schedule', render: function (r, i) { return inputCell(r, i, 'Schedule', 'list'); }, cls: function () { return MODE === 'Detail' && CAN_SAVE ? 'win-editable' : ''; } },
        { key: 'SalesTerm', cap: 'SalesTerm', render: function (r, i) { return inputCell(r, i, 'SalesTerm', 'list'); }, cls: function () { return MODE === 'Detail' && CAN_SAVE ? 'win-editable' : ''; } },
        { key: 'Remarks', cap: 'Remarks' },
        { key: 'EERemarks', cap: 'EE Remarks', render: function (r, i) { return inputCell(r, i, 'EERemarks', 'text'); }, cls: function () { return MODE === 'Detail' && CAN_SAVE ? 'win-editable' : ''; } }
    ];
    var SUMMARY_COLS = DETAIL_COLS.filter(function (c) { return ['Id', 'FcyBankReceiptId', 'Sr.No.', 'EEId'].indexOf(c.key) < 0; })
        .map(function (c) { var x = {}; for (var k in c) x[k] = c[k]; delete x.render; delete x.cls; return x; });

    function render() {
        H.drawGrid('grdfrm', ROWS, MODE === 'Detail' ? DETAIL_COLS : SUMMARY_COLS, { cur: CUR });
        H.show('grdfrmEmpty', !ROWS.length);
        ['Schedule', 'SalesTerm'].forEach(function (k) {
            var dl = H.$id('ee-list-' + k);
            if (!dl) { dl = document.createElement('datalist'); dl.id = 'ee-list-' + k; document.body.appendChild(dl); }
            dl.innerHTML = (k === 'Schedule' ? SCHEDULES : TERMS).map(function (v) { return '<option value="' + H.esc(v) + '"></option>'; }).join('');
        });
    }
    function dateType() { H.applyDateType(H.val('cmbperemeter'), 'gdfrmdate', 'gdtodate', YEAR_START); }
    function bindCombos(d) { H.bind('CmbBank', d.banks || [], 'Id', 'Name'); H.bind('cmbrealizedbank', d.realizedBanks || [], 'Id', 'Name'); }
    function mode() { return H.checked('ChkDetail') ? 'Detail' : 'Summary'; }
    function filters() { return { fromDate: H.val('gdfrmdate'), toDate: H.val('gdtodate'), bankId: H.netI(H.val('CmbBank')), realizedBankId: H.netI(H.val('cmbrealizedbank')), mode: mode() }; }
    function show(btn) {
        return H.busy(btn, function () {
            return H.postJson(API + '/show', filters()).then(function (d) {
                d = d || {};
                MODE = d.mode || mode(); ROWS = d.rows || []; TERMS = d.salesTerms || []; SCHEDULES = d.schedules || []; CUR = -1; PICKED = {};
                render();
            });
        });
    }
    /** grdfrm_UpdatingCell + grdfrm_CellUpdated: numeric guard, then EEStatement.Save for the row. */
    function cellChanged(el) {
        var i = +el.getAttribute('data-i'), key = el.getAttribute('data-key'), r = ROWS[i]; if (!r) return;
        if ((key === 'ITRSNo' || key === 'FEReturnPageNo') && el.value !== '' && !/^-?\d+$/.test(el.value)) {
            H.box('Please Type Only Numeric Value');
            el.value = H.str(H.col(r, key));
            return;
        }
        var monthValue, monthText;
        if (key === 'FEReturnMonth') {
            if (el.value === '__raw__') { monthValue = H.str(H.col(r, 'FEReturnMonth')); monthText = monthValue; }
            else { monthValue = el.value; var m = MONTHS.filter(function (x) { return String(x.MonthId) === el.value; })[0]; monthText = m ? m.Month : ''; PICKED[i] = el.value; r.FEReturnMonth = monthText; }
        } else {
            r[key] = el.value;
            monthValue = PICKED[i] !== undefined ? PICKED[i] : H.str(H.col(r, 'FEReturnMonth'));   // ToInt(cell Value): 0 for the stored text
            monthText = H.str(H.col(r, 'FEReturnMonth'));
        }
        var body = { Id: H.col(r, 'Id'), FcyBankReceiptId: H.col(r, 'FcyBankReceiptId'), EEId: H.col(r, 'EEId'), FEReturnPageNo: H.col(r, 'FEReturnPageNo'), ITRSNo: H.col(r, 'ITRSNo'),
            FEReturnMonthValue: monthValue, FEReturnMonth: monthText, Schedule: H.col(r, 'Schedule'), SalesTerm: H.col(r, 'SalesTerm'), EERemarks: H.col(r, 'EERemarks') };
        el.disabled = true;
        H.postJson(API + '/save-row', body).then(function (d) {
            if (d && d.id && H.netI(H.col(r, 'EEId')) === 0) r.EEId = d.id;
            var v = H.str(H.col(r, key === 'FEReturnMonth' ? 'FEReturnMonth' : key));
            if (key === 'Schedule' && SCHEDULES.indexOf(v) < 0) SCHEDULES.push(v);
            if (key === 'SalesTerm' && TERMS.indexOf(v) < 0) TERMS.push(v);
        }).catch(function (e) { H.box(e.message); }).then(function () { el.disabled = false; });
    }
    /** frmStockReport_Load: ParameterFill (Rows[1] "This Week"), RealizedBankFill, BankFill. */
    function load() {
        return H.getJson(API + '/setup').then(function (d) {
            d = d || {};
            YEAR_START = d.yearStart || ''; MONTHS = d.months || [];
            if (d.permissions) CAN_SAVE = d.permissions.Save !== false;
            H.bindDateTypes('cmbperemeter', d.dateTypes);
            H.setText('gdfrmdate', H.today()); H.setText('gdtodate', H.today());
            H.setVal('cmbperemeter', 2); dateType();
            bindCombos(d);
            H.$id('footerInfo').textContent = 'EEReport_ExportGD  -  Prints 567 / 567_01 / 567_02';
            H.focus('cmbperemeter');
        }).catch(function (e) { H.box(e.message || 'Error occurred during database call.'); });
    }
    /** btnNew_Click: banks cleared, grid cleared, focus date type, Detail ticked. */
    function reset() {
        H.setVal('cmbrealizedbank', 0); H.setVal('CmbBank', 0);
        ROWS = []; CUR = -1; render();
        H.focus('cmbperemeter');
        H.setChecked('ChkDetail', true);
    }
    /** Btnreferesh_Click: ParameterFill (Rows[1] again), BankFill, RealizedBankFill. */
    function refresh(btn) {
        return H.busy(btn, function () {
            return H.getJson(API + '/combos').then(function (d) { H.setVal('cmbperemeter', 2); dateType(); bindCombos(d || {}); });
        });
    }
    /** The three print buttons: "Record Not Found For Display" without data; the same procedure with the shown Activity. */
    function print(btn, rpt) {
        if (!ROWS.length) { H.box('Record Not Found For Display'); return; }
        var f = filters();
        return H.print(rpt || '567-EEReport_ExportGD.rpt', { fromDate: f.fromDate, toDate: f.toDate, bankGlDrAc: f.bankId || undefined,
            exImFCBankReceiptsId: f.realizedBankId || undefined, activity: MODE }, btn);
    }
    function shortcuts() { H.shortcuts([]); }
    function toggleHistory() { var b = H.$id('grdfrmBox'); if (b) b.scrollIntoView({ behavior: 'smooth' }); H.focusGrid('grdfrm'); }

    document.addEventListener('DOMContentLoaded', function () {
        H.wireFullscreen();
        H.$id('cmbperemeter').addEventListener('change', dateType);
        H.wireGrid('grdfrm', { select: function (i) { CUR = i; } });
        H.$id('grdfrm').addEventListener('change', function (e) { var el = e.target.closest('.ee-edit'); if (el) cellChanged(el); });
        H.wireKeys({ shortcuts: shortcuts, handle: function (e, k) {
            if (!e.ctrlKey || e.target.closest('.ee-edit')) return;
            if (k === 's') { e.preventDefault(); show(H.$id('btnshow')); }
            else if (k === 'n') { e.preventDefault(); reset(); }
            else if (k === 'p') { e.preventDefault(); print(H.$id('btnPrint'), '567-EEReport_ExportGD.rpt'); }
            else if (e.key === 'F5' || e.key === 'ArrowUp') { e.preventDefault(); H.focus('cmbperemeter'); }
            else if (e.key === 'ArrowDown') { e.preventDefault(); H.focusGrid('grdfrm'); }
        } });
        load();
    });

    global.ExportEeReport = { show: show, reset: reset, refresh: refresh, print: print, shortcuts: shortcuts, toggleHistory: toggleHistory };
}(window));
