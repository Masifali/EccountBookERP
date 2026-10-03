/* ============================================================================================
 * countx_export_fcy_gd_utilization.js - frmGdUtilizationAgainstFcyReceipt.cs (Architecture.WinApp.Export),
 * screen 953 "Fcy Receipts Utilization Against Bank Invoice / GD". dtGrid rows: Id, FcyBankReceiptId,
 * RefDocumentTypeId(4), InvoiceId, InvoiceNo, GdRefDocTypeId, GDId, GDNo, GDValue, AdvanceUtilize,
 * UtilizedAmount, GDBalance, UtilizingAmount, Commission, FDBCNo, PrevSavedStepStatus, StepStatus,
 * BalanceAmount. grd_CellUpdated / TotalAmountAutoUtilizeInGrid reproduced verbatim. Data through
 * /api/export/fcy-receipt-gd-utilization (ExportBankGdController -> ExportFcyGdUtilizationService).
 * ============================================================================================ */
(function (global) {
    'use strict';
    var X = global.ExBG, $id = X.$id, box = X.box, ask = X.ask, val = X.val, setText = X.setText, netD = X.netD, netI = X.netI, fmt = X.fmt, str = X.str, col = X.col, esc = X.esc;
    var API = '/api/export/fcy-receipt-gd-utilization';

    var PERM = { Save: true, Update: true, Delete: true, Print: true };
    var PENDING = [], PEND_VIEW = [], PEND_SELECT = false, GRID = [], REMOVED = [], RecId = 0, Approved = false, DefaultDays = 0;
    var CUR = -1, HIST = [], HIST_CUR = -1, HIST_DET = [], DATE_TYPES = [[1, 'This Day'], [2, 'This Week'], [3, 'This Month'], [4, 'This Year'], [5, 'Financial Year']];

    function saveVisible() { return !$id('btnSave').classList.contains('is-hidden') && !$id('btnSave').disabled; }

    // ------------------------------------------------------------------ load
    function load() {
        return X.getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM = { Save: d.permissions.Save !== false, Update: d.permissions.Update !== false, Delete: d.permissions.Delete !== false, Print: d.permissions.Print !== false };
            $id('btnSave').disabled = !PERM.Save; $id('btnUpdate').disabled = !PERM.Update; $id('btnDelete').disabled = !PERM.Delete;
            $id('ChkPrintslip').checked = PERM.Print;
            if (d.pendingError) box(d.pendingError);
            DefaultDays = netI(d.defaultDaysToLessFromHistoryFromDate);
            fillPending(d.pending || []);
            setText('txtFromdateHistory', X.daysAgo(DefaultDays > 0 ? DefaultDays : 3)); setText('txtToDateHistory', X.today());
            setText('txtReceiptDate', X.today());
            render();
            $id('fcyFooterInfo').textContent = 'frmGdUtilizationAgainstFcyReceipt';
        }).catch(function () { box('Error occurred during database call.'); });
    }
    /* FillGrdPendingOrders + GetDistinctParties. */
    function fillPending(rows) {
        PENDING = rows; PEND_VIEW = PENDING; PEND_SELECT = false;
        var parties = [], seen = {};
        PENDING.forEach(function (r) { var id = netI(col(r, 'SupplierCustomerId')); if (!seen[id]) { seen[id] = 1; parties.push({ Id: id, Name: str(col(r, 'Customer')) }); } });
        X.bind('CmbAccountFilter', parties, 'Id', 'Name', []);
        renderPending();
    }
    var PCOLS = [
        { key: '_l', html: function (v, r, i) { return '<button type="button" class="win-edit" data-act="load" data-i="' + i + '">Load</button>'; }, cls: 'win-cell-btn' },
        { key: '_sel', html: function (v, r, i) { return PEND_SELECT ? '<input type="checkbox" data-cell="Select" ' + (r._sel ? 'checked' : '') + '/>' : ''; }, cls: 'ctr' },
        { key: 'ReceiptDate', fmt: X.shortDate }, { key: 'ReceiptCode', num: true }, 'Customer', 'PaymentTerm', 'InvoiceNo', 'BankName', 'FcyCode',
        { key: 'ExchangeRate', num: true, fmt: fmt }, { key: 'FcyAmount', num: true, sum: true, fmt: fmt }, { key: 'LcyAmount', num: true, sum: true, fmt: fmt }
    ];
    function renderPending() {
        X.show('fcyPendSelTh', PEND_SELECT);
        X.drawGrid('fcyPendBody', 'fcyPendFoot', PEND_VIEW, PCOLS, { empty: 'fcyPendEmpty' });
        if (!PEND_SELECT) $id('fcyPendBody').querySelectorAll('td:nth-child(2)').forEach(function (td) { td.classList.add('is-hidden'); });
    }
    /* grdPendingOrders_ColumnButtonClick "Load". */
    function loadReceipt(i) {
        var r = PEND_VIEW[i]; if (!r) return;
        if (!saveVisible()) { box('Please Reset the Form First to Load New Data'); return; }
        setText('txtReceiptDate', X.isoDate(col(r, 'ReceiptDate')));
        setText('txtReceiptNo', str(col(r, 'ReceiptCode')));
        setText('txtFcyAmount', fmt(col(r, 'FcyAmount'), 3));
        setText('txtExchangeRate', fmt(col(r, 'ExchangeRate'), 3));
        setText('txtLcyAmount', fmt(col(r, 'LcyAmount'), 3));
        return X.getJson(API + '/gds?fcyReceiptId=' + netI(col(r, 'Id'))).then(function (rows) { GRID = rows || []; CUR = -1; render(); }).catch(function (e) { box(e.message); });
    }
    /* btnShowRecords_Click: filter the pending grid by the chosen customer (adds the Select column), or show all and clear the detail grid. */
    function showRecords() {
        var acc = netI(val('CmbAccountFilter'));
        GRID = [];
        if (acc > 0) {
            var f = PENDING.filter(function (r) { return netI(col(r, 'SupplierCustomerId')) === acc; });
            if (f.length) { PEND_VIEW = f; PEND_SELECT = true; renderPending(); }
            else box('No pending vouchers found for the selected account.');
        } else {
            PEND_VIEW = PENDING; PEND_SELECT = false; renderPending(); render();
        }
    }
    /* BtnAutoUtilizedAllRowsOfPendingGrid_Click: only its validations exist on the desktop. */
    function autoUtilizeAllPending() {
        if (!saveVisible()) { box('Please Reset the Form First to Load New Data'); return; }
        if (PEND_VIEW.length === 0) { box('Pending Vouchers Detail have no Records...'); return; }
        if (PEND_VIEW.some(function (r) { return r._sel; })) return;
        box("Please Select Any row First.Press show Button If 'Select' Button not Appear");
    }

    // ------------------------------------------------------------------ detail grid
    function inp(name, r, i, cls) { return '<input type="text" class="num ' + (cls || '') + '" data-cell="' + name + '" value="' + esc(name === 'FDBCNo' ? str(r[name]) : fmt(r[name], 4)) + '"/>'; }
    var COLS = [
        'InvoiceNo', { key: 'GDNo', link: true }, { key: 'GDValue', num: true, fmt: fmt }, { key: 'AdvanceUtilize', num: true, fmt: fmt },
        { key: 'UtilizedAmount', num: true, fmt: fmt }, { key: 'GDBalance', num: true, fmt: fmt },
        { key: 'UtilizingAmount', num: true, sum: true, fmt: function (v, r) { return r ? '' : fmt(v, 4); }, html: function (v, r, i) { return inp('UtilizingAmount', r, i); }, cls: 'win-editable' },
        { key: 'Commission', num: true, html: function (v, r, i) { return inp('Commission', r, i); }, cls: 'win-editable' },
        { key: 'FDBCNo', html: function (v, r, i) { return '<input type="text" data-cell="FDBCNo" value="' + esc(str(r.FDBCNo)) + '"/>'; }, cls: 'win-editable' },
        'PrevSavedStepStatus', 'StepStatus', { key: 'BalanceAmount', num: true, fmt: fmt }
    ];
    function render() { X.drawGrid('fcyBody', 'fcyFoot', GRID, COLS, { cur: CUR }); }
    /* StepStatus rule shared by CellUpdated ("Last Part") and Auto Utilize ("Final Part"). */
    function stepStatus(item, BalanceAmount, lastLabel) {
        var Id = netI(item.Id), LastSavedStatus = str(item.PrevSavedStepStatus);
        if (LastSavedStatus === 'Final Part') return LastSavedStatus;
        if (!LastSavedStatus) return BalanceAmount === 0 ? lastLabel : '1 Part';
        var lastSavedNumber = parseInt(LastSavedStatus.split(/\s+/)[0], 10);
        if (isNaN(lastSavedNumber)) throw new Error('Input string was not in a correct format.');
        var newStatus = lastSavedNumber + ' Part';
        if (Id === 0) newStatus = (lastSavedNumber + 1) + ' Part';
        return BalanceAmount === 0 ? lastLabel : newStatus;
    }
    /* grd_CellUpdated */
    function cellUpdated(i, key, value) {
        try {
            var item = GRID[i]; if (!item) return;
            if (key === 'FDBCNo') { item.FDBCNo = value; return; }
            item[key] = netD(value);
            var UtilizingAmount = netD(item.UtilizingAmount), GDBalance = netD(item.GDBalance), Commission = netD(item.Commission);
            if (key === 'UtilizingAmount' && UtilizingAmount + Commission > GDBalance) {
                item.UtilizingAmount = GDBalance - Commission;
                box('UtilizingAmount:' + UtilizingAmount + ' can not be greater than GD Balance Amount:' + GDBalance);
            }
            if (key === 'Commission' && UtilizingAmount + Commission > GDBalance) {
                item.Commission = GDBalance - UtilizingAmount;
                box('Commission:' + Commission + ' + UtilizingAmount:' + UtilizingAmount + ' = ' + (Commission + UtilizingAmount) + ' can not be greater than GD Balance Amount:' + GDBalance);
            }
            /* the desktop computes the balance from the values read BEFORE the clamp */
            var BalanceAmount = GDBalance - UtilizingAmount - Commission;
            item.BalanceAmount = BalanceAmount;
            item.StepStatus = stepStatus(item, BalanceAmount, 'Last Part');
            CUR = i; render();
        } catch (e) { box(e.message); }
    }
    /* TotalAmountAutoUtilizeInGrid */
    function autoUtilize() {
        try {
            var totalAmount = netD(val('txtFcyAmount'));
            var totalGridAmount = X.sum(GRID, 'UtilizingAmount');
            if (totalAmount >= totalGridAmount) {
                totalAmount -= totalGridAmount;
                var rows = GRID.filter(function (r) { return netD(r.UtilizingAmount) === 0; });
                for (var k = 0; k < rows.length; k++) {
                    var row = rows[k];
                    var GDBalance = netD(row.GDBalance), commission = netD(row.Commission), balAmount = netD(row.BalanceAmount);
                    var applyAmount = Math.min(totalAmount, Math.min(balAmount, GDBalance));
                    row.UtilizingAmount = applyAmount;
                    var BalanceAmount = GDBalance - commission - applyAmount;
                    row.BalanceAmount = BalanceAmount;
                    row.StepStatus = stepStatus(row, BalanceAmount, 'Final Part');
                    totalAmount -= applyAmount;
                    if (totalAmount <= 0) break;
                }
            } else {
                var utilizedAmount = 0;
                GRID.forEach(function (row2) {
                    if (utilizedAmount >= totalAmount) { row2.UtilizingAmount = 0; row2.BalanceAmount = netD(row2.GDBalance); }
                    utilizedAmount += netD(row2.UtilizingAmount);
                });
            }
            render();
        } catch (e) { box('Error while auto-utilizing: ' + e.message); }
    }

    // ------------------------------------------------------------------ form
    /* Reset */
    function reset() {
        RecId = 0; Approved = false;
        X.show('btnSave', true); X.show('btnUpdate', false); X.show('btnDelete', false);
        setText('txtReceiptNo', ''); setText('txtReceiptDate', X.today()); setText('txtFcyAmount', ''); setText('txtLcyAmount', ''); setText('txtExchangeRate', '');
        REMOVED = []; GRID = []; CUR = -1; render();
        return X.getJson(API + '/pending').then(function (rows) { fillPending(rows || []); }).catch(function (e) { box(e.message); });
    }
    /* Insert(): "Grid Record Not Found" -> FormValidation -> confirm -> the server repeats the row checks. */
    function insert(btn) {
        return X.busy(btn, function () {
            if (GRID.length === 0) { box('Grid Record Not Found'); return Promise.resolve(); }
            var f = val('txtFcyAmount').trim();
            if (f === '' || f === '0') { box('Fcy Amount Field is Required'); X.focus('txtFcyAmount'); return Promise.resolve(); }
            if (!ask(RecId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return Promise.resolve();
            return X.postJson(API + '/save', { recId: RecId, fcyAmount: netD(f), rows: GRID, removed: REMOVED }).then(function (d) {
                box((d && d.message) || 'Record Saved Successfully of GD No [] ');
                return reset().then(function () { showRecords(); });
            }).catch(function (e) { box(e.message); });
        });
    }
    function save(btn) { RecId = 0; return insert(btn); }
    /* btnUpdate_Click */
    function update(btn) {
        if (RecId === 0) { box('Record Not found'); return Promise.resolve(); }
        if (Approved) { box('Record Not Update because Record has approved'); return Promise.resolve(); }
        return insert(btn);
    }
    /* btnDelete_Click: on the desktop it only shows "Deleted Successfully" and resets - nothing is deleted. */
    function deleteClick() {
        if (RecId === 0) { box('Record Id not found for deletion...'); return; }
        if (!ask('Are you sure to Delete?')) return;
        box('Deleted Successfully');
        reset();
    }
    function refresh() { /* btnRefresh_Click is empty on the desktop */ }
    /* ReadById(Id): Reset, then the desktop's DataTable column error (see the service). */
    function readById(id) {
        return reset().then(function () {
            RecId = id;
            return X.getJson(API + '/by-id?id=' + id).then(function (d) {
                if (d && netI(d.rows) > 0) {
                    X.innerTab('fcy', 'fcyForm', 'btnFcyFooterHistory');
                    X.show('btnSave', false); X.show('btnUpdate', true); X.show('btnDelete', true);
                    box(d.message);
                }
            });
        }).catch(function (e) { box(e.message); });
    }
    function shortcuts() {
        X.shortcutPopup([['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+Shift+Delete', 'For Delete'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
            ['Ctrl+P', 'For Print Slip 901'], ['Alt+1', 'For Print Slip 901'], ['Ctrl+F5', 'For Focus on DocDate'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'],
            ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+D', 'For Adding an row in Focused Grid'], ['Ctrl+Delete', 'For Deleting an row of Focused Grid'],
            ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowRight', 'For Change Focus from one Grid To another Grid'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }

    // ------------------------------------------------------------------ history
    var HCOLS = [
        { key: 'ReceiptDate', fmt: X.shortDate }, { key: 'ReceiptCode', link: true }, 'Customer', 'PaymentTerm', 'InvoiceNo', 'BankName', 'FcyCode',
        { key: 'ExchangeRate', num: true, fmt: fmt }, { key: 'FcyAmount', num: true, sum: true, fmt: fmt }, { key: 'LcyAmount', num: true, sum: true, fmt: fmt },
        { key: '_x', html: function (v, r, i) { return '<button type="button" class="win-x" data-del="' + i + '" title="Delete">X</button>'; }, cls: 'win-cell-btn' }
    ];
    var HDCOLS = ['GDNo', { key: 'GDValue', num: true, fmt: fmt }, 'InvoiceNo', 'FDBCNo', { key: 'RealizedAmount', num: true, sum: true, fmt: fmt }, { key: 'AgencyComm', num: true, sum: true, fmt: fmt }, 'StepStatus'];
    function histRender() { X.drawGrid('fcyHistBody', 'fcyHistFoot', HIST, HCOLS, { cur: HIST_CUR, empty: 'fcyHistEmpty' }); }
    /* FillHistory */
    function historyShow(btn) {
        return X.busy(btn, function () {
            var q = [];
            if ($id('txtFromdateHistoryChk').checked && val('txtFromdateHistory')) q.push('fromDate=' + val('txtFromdateHistory'));
            if ($id('txtToDateHistoryChk').checked && val('txtToDateHistory')) q.push('toDate=' + val('txtToDateHistory'));
            return X.getJson(API + '/history' + (q.length ? '?' + q.join('&') : '')).then(function (rows) {
                HIST = rows || []; HIST_CUR = -1; histRender();
                HIST_DET = []; X.drawGrid('fcyHistDetBody', 'fcyHistDetFoot', HIST_DET, HDCOLS, {});
            }).catch(function (e) { box(e.message); });
        });
    }
    /* grdHistory_SelectionChanged -> GetGdInfoAgainstReceipt (an empty result leaves the lower grid as it was). */
    function histSelect(i) {
        HIST_CUR = i; var r = HIST[i]; if (!r) return;
        X.getJson(API + '/utilization?id=' + netI(col(r, 'Id'))).then(function (rows) {
            if (rows && rows.length) { HIST_DET = rows; X.drawGrid('fcyHistDetBody', 'fcyHistDetFoot', HIST_DET, HDCOLS, {}); }
        }).catch(function (e) { box(e.message); });
    }
    /* DeleteRecordAgainstFcyReceipt */
    function histDelete(i) {
        var r = HIST[i]; if (!r) return;
        if (!ask('Are you sure to Delete?')) return;
        X.postJson(API + '/delete-by-receipt', { id: netI(col(r, 'Id')) }).then(function () {
            return historyShow($id('btnshow')).then(function () { return X.getJson(API + '/pending'); }).then(function (rows) { fillPending(rows || []); });
        }).catch(function (e) { box(e.message); });
    }
    /* cmbDateTypeHistory_ValueChanged */
    function dateTypeChanged() {
        var v = netI(val('cmbDateTypeHistory')), d = new Date();
        if (v === 1) setText('txtFromdateHistory', X.today());
        else if (v === 2) setText('txtFromdateHistory', X.daysAgo(7));
        else if (v === 3) { setText('txtFromdateHistory', d.getFullYear() + '-' + X.pad(d.getMonth() + 1) + '-01'); setText('txtToDateHistory', X.today()); }
        else if (v === 4) { setText('txtFromdateHistory', d.getFullYear() + '-01-01'); setText('txtToDateHistory', X.today()); }
        else if (v === 5) { setText('txtFromdateHistory', FY_START || X.today()); }
    }
    var FY_START = '';
    /* tabControl1_SelectedIndexChanged: first visit fills the Date Type combo and activates its first row. */
    function tab(group, panelId) {
        X.innerTab(group, panelId, 'btnFcyFooterHistory', function (p, onHist) {
            if (onHist && $id('cmbDateTypeHistory').options.length <= 1) {
                X.bind('cmbDateTypeHistory', DATE_TYPES.map(function (t) { return { Id: t[0], Parameters: t[1] }; }), 'Id', 'Parameters', []);
                X.setVal('cmbDateTypeHistory', 1); dateTypeChanged();
                X.focus('cmbDateTypeHistory');
            }
        });
    }
    function toggleHistory() { tab('fcy', X.activeInner('fcy') === 'fcyHistory' ? 'fcyForm' : 'fcyHistory'); }
    /* Resethistory: Date Type = 3 (This Month). */
    function historyReset() { X.setVal('cmbDateTypeHistory', 3); dateTypeChanged(); X.focus('cmbDateTypeHistory'); }
    function historyRefresh() { /* HistoryComboBind is empty on the desktop */ }

    document.addEventListener('DOMContentLoaded', function () {
        X.wireTabs(tab);
        X.on('cmbDateTypeHistory', 'change', dateTypeChanged);
        X.wireGrid('fcyBody', { select: function (i) { CUR = i; }, input: cellUpdated, open: function (i) { CUR = i; render(); } });
        X.wireGrid('fcyPendBody', {
            act: function (a, i) { if (a === 'load') loadReceipt(i); },
            input: function (i, cell, v, el) { if (cell === 'Select') PEND_VIEW[i]._sel = el.checked; }
        });
        X.wireGrid('fcyHistBody', { del: histDelete, select: histSelect, open: function (i) { var r = HIST[i]; if (r) readById(netI(col(r, 'Id'))); } });
        /* InvfrmPurchasedirectInvoice_KeyDown_1 */
        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase();
            if (X.enterMovesOn(e)) return;
            var onForm = X.activeInner('fcy') === 'fcyForm';
            if (e.ctrlKey && k === 't') { e.preventDefault(); toggleHistory(); return; }
            if (e.ctrlKey && k === 'e') { e.preventDefault(); X.cancel(); return; }
            if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcuts(); return; }
            if (onForm) {
                if (e.ctrlKey && k === 's' && saveVisible()) { e.preventDefault(); save($id('btnSave')); }
                if (e.ctrlKey && k === 'u' && !$id('btnUpdate').classList.contains('is-hidden') && !$id('btnUpdate').disabled) { e.preventDefault(); update($id('btnUpdate')); }
                if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); }
                if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); var tr = $id('fcyBody').querySelector('input'); if (tr) tr.focus(); }
                if (e.ctrlKey && e.key === 'ArrowRight') { e.preventDefault(); $id('grdPendingReceiptsBox').scrollIntoView(); }
                return;
            }
            if (e.ctrlKey && (e.key === 'F5' || e.key === 'ArrowDown' || e.key === 'ArrowUp' || e.key === 'ArrowRight')) { e.preventDefault(); var t = $id('fcyHistBody').querySelector('tr'); if (t) t.scrollIntoView(); }
            if (e.ctrlKey && e.key === 'Enter' && HIST_CUR >= 0 && PERM.Update) { e.preventDefault(); var r = HIST[HIST_CUR]; if (r) readById(netI(col(r, 'Id'))); }
        });
        load();
    });

    global.ExportFcyGd = {
        reset: reset, refresh: refresh, save: save, update: update, deleteClick: deleteClick, shortcuts: shortcuts, autoUtilize: autoUtilize,
        showRecords: showRecords, autoUtilizeAllPending: autoUtilizeAllPending,
        historyShow: historyShow, historyReset: historyReset, historyRefresh: historyRefresh, toggleHistory: toggleHistory
    };
}(window));
