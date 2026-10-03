/* ============================================================================================
 * countx_export_gd_bank_request.js - GdBankRequest.cs (Architecture.WinApp.Export), screen 197
 * "Gd Bank Request". dtDetail rows: Id, SubCode, AmountRcvd, GdId, RefDocumentTypeId, GdNo, GdValue,
 * FobValue, GdBalance, Realized, FTT, Status, Remarks. The desktop's Calculation /
 * CalculateRealizedFromGrid / CheckValidation state machine (RemaingOfRecvAmount,
 * UniqueSubCodeForOneEntry, txtAmountRcvd.Enabled) is reproduced verbatim, including its quirks.
 * Data through /api/export/gd-bank-request (ExportBankGdController -> ExportGdBankRequestService).
 * ============================================================================================ */
(function (global) {
    'use strict';
    var X = global.ExBG, $id = X.$id, box = X.box, ask = X.ask, val = X.val, setText = X.setText, netD = X.netD, netI = X.netI, fmt = X.fmt, str = X.str, col = X.col;
    var API = '/api/export/gd-bank-request';

    var PERM = { Save: true, Update: true, Print: true };
    var BANKS = [], HIST_BANKS = [], GDS = [];
    var DT = [];                       /* dtDetail */
    var REMOVED = [];                  /* LstRemoveRecordDetail */
    var RecId = 0, DefaultDays = 0, RemaingOfRecvAmount = 0, UniqueSubCodeForOneEntry = 1, updateDetailIndex = -1;
    var CUR = -1, HIST = [], HIST_CUR = -1, HIST_DET = [];
    var rcvdEnabled = true;            /* txtAmountRcvd.Enabled */

    function setRcvdEnabled(on) { rcvdEnabled = on; $id('txtAmountRcvd').disabled = !on; }
    function currentRow() { return CUR >= 0 ? DT[CUR] : (updateDetailIndex >= 0 ? DT[updateDetailIndex] : null); }
    function gdRow() { return X.findRow(GDS, val('CmbGdNo')); }
    function sumWhere(colName, pred) { return X.sum(DT, colName, pred); }
    function countWhere(pred) { var n = 0; DT.forEach(function (r, i) { if (pred(r, i)) n++; }); return n; }
    /* GetOrdinal */
    function ordinal(n) {
        if (n <= 0) return String(n);
        var m = n % 100;
        if (m - 11 >= 0 && m - 11 <= 2) return n + 'th';
        switch (n % 10) { case 1: return n + 'st'; case 2: return n + 'nd'; case 3: return n + 'rd'; default: return n + 'th'; }
    }

    // ------------------------------------------------------------------ load / combos
    /** GdBankRequest_Load. */
    function load() {
        return X.getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM = { Save: d.permissions.Save !== false, Update: d.permissions.Update !== false, Print: d.permissions.Print !== false };
            $id('btnSave').disabled = !PERM.Save; $id('btnUpdate').disabled = !PERM.Update; $id('BtnPrint').disabled = !PERM.Print;
            ['docNo', 'banks', 'historyBanks'].forEach(function (k) { if (d[k + 'Error']) box(d[k + 'Error']); });
            if (netI(d.docNo) > 0) setText('txtDocNo', d.docNo);
            BANKS = d.banks || []; X.bind('CmbBank', BANKS, 'Id', 'Name', []);
            HIST_BANKS = d.historyBanks || []; X.bind('CmbBankHistory', HIST_BANKS, 'Id', 'Name', []);
            DefaultDays = netI(d.defaultDaysToLessFromHistoryFromDate);
            setText('FromDateHistory', X.daysAgo(DefaultDays > 0 ? DefaultDays : 3)); setText('ToDateHistory', X.today());
            setText('DocDate', X.today());
            render();
            $id('gbrFooterInfo').textContent = 'GdBankRequest';
            X.focus('DocDate');
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }
    /* GetGdsAgainstBank(BankId) -> CmbGdNo (Id / GdNo / InvoiceNo / GdValue / GdBalance / Status / FobValue / RefDocumentTypeId). */
    function gdsForBank(bankId) {
        return X.getJson(API + '/gds?bankId=' + bankId + '&recId=' + RecId).then(function (rows) {
            GDS = rows || []; X.bind('CmbGdNo', GDS, 'Id', 'GdNo', ['InvoiceNo', 'GdValue', 'GdBalance', 'Status', 'FobValue']);
        }).catch(function (e) { box(e.message); });
    }
    /* CmbBank_Leave. */
    function bankLeave() {
        if (netI(val('CmbBank')) > 0) return gdsForBank(netI(val('CmbBank')));
        GDS = []; X.bind('CmbGdNo', GDS, 'Id', 'GdNo', []);
        return Promise.resolve();
    }
    /* txtAmountRcvd_Leave. */
    function amountLeave() {
        if (netI(val('CmbGdNo')) > 0) { RemaingOfRecvAmount = netD(val('txtAmountRcvd')); calculation(); }
    }
    /* CmbGdNo_Leave. */
    function gdLeave() {
        var g = gdRow(); if (!g || netI(val('CmbGdNo')) <= 0) return;
        setText('txtInvoiceNo', str(col(g, 'InvoiceNo')));
        setText('txtGdValue', fmt(col(g, 'GdValue')));
        setText('txtFobValue', fmt(col(g, 'FobValue')));
        setText('txtGdValueBalance', fmt(col(g, 'GdBalance')));
        if (rcvdEnabled && !(netD(val('txtRealizedAmount')) > 0)) setText('txtRealizedAmount', fmt(col(g, 'GdBalance')));
        setText('txtStatus', ordinal(netI(col(g, 'Status')) + 1) + ' Part');
        if (netD(val('txtAmountRcvd')) > 0) calculation();
    }

    // ------------------------------------------------------------------ the desktop's arithmetic
    /* Calculation() */
    function calculation() {
        try {
            var g = gdRow(); if (!g) return;
            var gdId = netI(val('CmbGdNo'));
            var RealizedAmount = 0, rcvdAmount = 0;
            if (DT.length > 0) {
                if (!rcvdEnabled || updateDetailIndex > -1) {
                    calculateRealizedFromGrid();
                    if (updateDetailIndex > -1) RemaingOfRecvAmount = netD(val('txtRealizedAmount'));
                    rcvdAmount = RemaingOfRecvAmount;
                } else rcvdAmount = netD(val('txtAmountRcvd'));
            } else rcvdAmount = netD(val('txtAmountRcvd'));
            var cur = currentRow();
            var idExists = updateDetailIndex <= -1
                ? DT.some(function (r) { return netI(r.GdId) === gdId; })
                : DT.some(function (r, i) { return i !== CUR && netI(r.GdId) === gdId; });
            if (idExists) {
                var SumOfSameGd = sumWhere('Realized', function (r) { return netI(r.GdId) === gdId; });
                var SumOfFttOfSameGd = sumWhere('FTT', function (r) { return netI(r.GdId) === gdId; });
                var Count = countWhere(function (r) { return netI(r.GdId) === gdId; });
                if (updateDetailIndex > -1) {
                    if (SumOfSameGd + SumOfFttOfSameGd === netD(col(g, 'GdBalance'))) return;
                    SumOfSameGd -= netD(cur ? cur.Realized : 0);
                    if (Count > 0) setText('txtStatus', ordinal(Count) + ' Part');
                } else {
                    if (SumOfSameGd + SumOfFttOfSameGd === netD(col(g, 'GdBalance'))) return;
                    if (Count > 0) setText('txtStatus', ordinal(Count + 1) + ' Part');
                }
                RealizedAmount = netD(val('txtGdValueBalance')) - SumOfSameGd;
            } else {
                RealizedAmount = netD(col(g, 'GdBalance'));
            }
            var Ftt = 0, Realized = 0;
            if (rcvdAmount >= RealizedAmount) {
                Ftt = netD(val('txtFobValue')) * 10 / 100;
                setText('txtFttAmount', fmt(Ftt));
                if (Ftt > rcvdAmount) box('Remaining Or Rcvd Amount Is Less than Ftt Amount.');
                Realized = RealizedAmount - Ftt;
                setText('txtRealizedAmount', fmt(Realized));
                setText('txtStatus', 'Final');
            } else {
                setText('txtRealizedAmount', fmt(rcvdAmount));
                setText('txtFttAmount', '0');
            }
        } catch (e) { box(e.message); }
    }
    /* CalculateRealizedFromGrid() */
    function calculateRealizedFromGrid() {
        if (DT.length <= 0) return;
        var Remaining1 = 0;
        var cur = currentRow();
        if (updateDetailIndex > -1 && cur) {
            var sc = netI(cur.SubCode);
            if (countWhere(function (r) { return netI(r.SubCode) === sc; }) > 1 && netD(cur.AmountRcvd) !== netD(val('txtAmountRcvd'))) {
                box('You Are Changing Rcvd Amount and there are multiple rows are present in grid against this detail entry. You can delete The Entire entry against it');
                setText('txtAmountRcvd', str(cur.AmountRcvd));
            }
            var minusCurrentRow = sumWhere('Realized', function (r) { return netI(r.SubCode) === sc; }) - netD(cur.Realized);
            Remaining1 = netD(val('txtAmountRcvd')) - minusCurrentRow;
        } else {
            var scu = subCodeWithUnproportionateAmount();
            var subCodeFilter = scu !== 0 ? scu : UniqueSubCodeForOneEntry;
            var Realized1 = sumWhere('Realized', function (r) { return netI(r.SubCode) === subCodeFilter; });
            Remaining1 = netD(val('txtAmountRcvd')) - Realized1;
        }
        setText('txtRealizedAmount', fmt(Remaining1));
        RemaingOfRecvAmount = Remaining1;
    }
    /* rows with AmountRcvd == txtAmountRcvd grouped by SubCode where Sum(Realized) != txtAmountRcvd -> first key, else 0. */
    function subCodeWithUnproportionateAmount() {
        var amt = netD(val('txtAmountRcvd')), seen = [], i;
        for (i = 0; i < DT.length; i++) {
            if (netD(DT[i].AmountRcvd) !== amt) continue;
            var sc = netI(DT[i].SubCode);
            if (seen.indexOf(sc) >= 0) continue;
            seen.push(sc);
            var s = sumWhere('Realized', function (r) { return netD(r.AmountRcvd) === amt && netI(r.SubCode) === sc; });
            if (s !== amt) return sc;
        }
        return 0;
    }
    /* CheckValidation() */
    function checkValidation() {
        var RcvdAmount = netD(val('txtAmountRcvd'));
        var gdBalance = netD(val('txtGdValueBalance'));
        var ftt = netD(val('txtFttAmount'));
        var status = false;
        if (ftt > 0) {
            if (rcvdEnabled) {
                if (updateDetailIndex > -1) RemaingOfRecvAmount -= netD(val('txtRealizedAmount'));
                else RemaingOfRecvAmount = RcvdAmount - netD(val('txtRealizedAmount'));
            } else RemaingOfRecvAmount -= netD(val('txtRealizedAmount'));
            setText('txtRealizedAmount', fmt(RemaingOfRecvAmount));
            status = true;
        }
        if (!status) {
            if (RemaingOfRecvAmount < gdBalance) {
                setRcvdEnabled(true);
                setText('txtAmountRcvd', ''); setText('txtRealizedAmount', ''); setText('txtStatus', ''); setText('txtRemarksDetail', '');
                X.focus('txtAmountRcvd');
            } else {
                setRcvdEnabled(true);
                X.setEnabled('CmbGdNo', true);
                setText('txtAmountRcvd', ''); X.setVal('CmbGdNo', '0');
                setText('txtGdValue', ''); setText('txtInvoiceNo', ''); setText('txtGdValueBalance', ''); setText('txtRealizedAmount', ''); setText('txtStatus', ''); setText('txtRemarksDetail', '');
            }
        } else {
            setRcvdEnabled(false);
            X.setEnabled('CmbGdNo', true);
            X.setVal('CmbGdNo', '0');
            setText('txtGdValue', ''); setText('txtInvoiceNo', ''); setText('txtFobValue', ''); setText('txtGdValueBalance', ''); setText('txtStatus', ''); setText('txtFttAmount', ''); setText('txtRemarksDetail', '');
            X.focus('CmbGdNo');
        }
    }

    // ------------------------------------------------------------------ grid
    var COLS = [
        { key: 'SubCode', num: true, fmt: function (v) { return str(netI(v)); } },
        { key: '_x', html: function (v, r, i) { return '<button type="button" class="win-x" data-del="' + i + '" title="Delete">X</button>'; }, cls: 'win-cell-btn' },
        { key: 'AmountRcvd', num: true, fmt: fmt }, { key: 'GdNo', link: true }, { key: 'GdValue', num: true, fmt: fmt }, { key: 'FobValue', num: true, fmt: fmt },
        { key: 'GdBalance', num: true, fmt: fmt }, { key: 'Realized', num: true, sum: true, fmt: fmt }, { key: 'FTT', num: true, sum: true, fmt: function (v) { return fmt(v, 2); } },
        'Status', 'Remarks'
    ];
    function render() { X.drawGrid('gbrBody', 'gbrFoot', DT, COLS, { cur: CUR }); }

    /* FormValidationDetail */
    function detailValidation() {
        if (!(netD(val('txtAmountRcvd')) > 0)) { box('Rcvd Amount field required'); X.focus('txtAmountRcvd'); return false; }
        if (netI(val('CmbGdNo')) === 0) { box('Gd No field required'); X.focus('CmbGdNo'); return false; }
        if (!val('txtGdValue').trim() || !(netD(val('txtGdValue')) > 0)) { box('Gd Value field required'); X.focus('txtGdValue'); return false; }
        if (!val('txtInvoiceNo')) { box('Invoice No field required'); X.focus('txtInvoiceNo'); return false; }
        if (!val('txtRealizedAmount').trim() || !(netD(val('txtRealizedAmount')) > 0)) { box('Realized Amount field required'); X.focus('txtRealizedAmount'); return false; }
        if (!val('txtStatus')) { box('Status field required'); X.focus('txtStatus'); return false; }
        return true;
    }
    /* btnAdd_Click */
    function addRow() {
        try {
            if (!detailValidation()) return;
            var g = gdRow() || {}, gdId = netI(val('CmbGdNo'));
            if (DT.some(function (r) { return netI(r.GdId) === gdId; })) {
                var s1 = sumWhere('Realized', function (r) { return netI(r.GdId) === gdId; });
                var s2 = sumWhere('FTT', function (r) { return netI(r.GdId) === gdId; });
                if (s1 + s2 === netD(col(g, 'GdBalance'))) { box('Please Select Another Gd Because This Gd is Completed in Detail'); return; }
            }
            var scu = DT.length > 0 ? subCodeWithUnproportionateAmount() : 0;
            DT.push({
                Id: 0, SubCode: scu !== 0 ? scu : UniqueSubCodeForOneEntry, AmountRcvd: netD(val('txtAmountRcvd')), GdId: gdId,
                RefDocumentTypeId: netI(col(g, 'RefDocumentTypeId')), GdNo: X.selText('CmbGdNo'), GdValue: netD(col(g, 'GdValue')),
                FobValue: netD(col(g, 'FobValue')), GdBalance: netD(col(g, 'GdBalance')), Realized: netD(val('txtRealizedAmount')),
                FTT: netD(val('txtFttAmount')), Status: val('txtStatus'), Remarks: val('txtRemarksDetail')
            });
            CUR = DT.length - 1;
            render();
            checkValidation();
            if (rcvdEnabled) { UniqueSubCodeForOneEntry = Math.max.apply(null, DT.map(function (r) { return netI(r.SubCode); })) + 1; }
            X.setEnabled('CmbBank', false);
        } catch (e) { box(e.message); }
    }
    /* btnDetailUpdate_Click */
    function updateRow() {
        try {
            if (!detailValidation()) return;
            var item = DT[updateDetailIndex]; if (!item) return;
            var sc = netI(item.SubCode);
            var same = DT.filter(function (r) { return netI(r.SubCode) === sc; });
            if (same.length > 1 && netD(same[0].AmountRcvd) !== netD(val('txtAmountRcvd'))) {
                box('You Are Changing Rcvd Amount and there are multiple rows are present in grid against this detail entry. You can delete The Entire entry against it');
                return;
            }
            var g = gdRow() || {};
            item.AmountRcvd = netD(val('txtAmountRcvd')); item.GdId = netI(val('CmbGdNo')); item.RefDocumentTypeId = netI(col(g, 'RefDocumentTypeId'));
            item.GdNo = X.selText('CmbGdNo'); item.Realized = netD(val('txtRealizedAmount')); item.FTT = netD(val('txtFttAmount'));
            item.Status = val('txtStatus'); item.Remarks = val('txtRemarksDetail');
            render();
            checkValidation();
            if (rcvdEnabled) { UniqueSubCodeForOneEntry = Math.max.apply(null, DT.map(function (r) { return netI(r.SubCode); })) + 1; }
            updateDetailIndex = -1;
            X.show('btnAdd', true); X.show('btnDetailUpdate', false); X.show('btnDetailCancel', false);
        } catch (e) { box(e.message); }
    }
    /* btnDetailCancel_Click */
    function cancelRow() {
        if (rcvdEnabled) setText('txtAmountRcvd', '');
        X.setVal('CmbGdNo', '0');
        ['txtGdValue', 'txtFobValue', 'txtInvoiceNo', 'txtGdValueBalance', 'txtRealizedAmount', 'txtStatus', 'txtRemarksDetail'].forEach(function (id) { setText(id, ''); });
        updateDetailIndex = -1;
        X.show('btnAdd', true); X.show('btnDetailUpdate', false); X.show('btnDetailCancel', false);
    }
    /* grdDetail_DoubleClick */
    function editRow(i) {
        var item = DT[i]; if (!item) return;
        CUR = i; updateDetailIndex = i;
        setText('txtAmountRcvd', str(item.AmountRcvd));
        X.setVal('CmbGdNo', item.GdId);
        gdLeave();
        setText('txtRealizedAmount', fmt(item.Realized)); setText('txtFttAmount', fmt(item.FTT));
        setText('txtStatus', item.Status); setText('txtRemarksDetail', item.Remarks);
        X.show('btnAdd', false); X.show('btnDetailUpdate', true); X.show('btnDetailCancel', true);
        if (!rcvdEnabled && DT.filter(function (r) { return netI(r.SubCode) === netI(item.SubCode); }).length === 1) setRcvdEnabled(true);
        render();
    }
    /* grdDetail_ColumnButtonClick "Delete": the whole SubCode goes; saved rows are remembered with ActionTypeId 3. */
    function deleteRow(i) {
        var item = DT[i]; if (!item) return;
        if (!ask('Are you sure! You Want To Delete.Because Entire Entry Will be delete Against this?')) return;
        var sc = netI(item.SubCode), DetailId = netI(item.Id);
        if (DetailId > 0) {
            DT.filter(function (r) { return netI(r.SubCode) === sc; }).forEach(function (r) { REMOVED.push(X.copy(r)); });
        }
        DT = DT.filter(function (r) { return netI(r.SubCode) !== sc; });
        CUR = -1;
        if (updateDetailIndex >= 0) updateDetailIndex = -1;
        render();
        if (DT.length > 0) X.setEnabled('CmbBank', true);
    }

    // ------------------------------------------------------------------ form
    /* Reset */
    function reset() {
        REMOVED = []; RecId = 0; UniqueSubCodeForOneEntry = 1; updateDetailIndex = -1; CUR = -1;
        setText('txtDocNo', '');
        X.setEnabled('CmbBank', true); X.setVal('CmbBank', '0');
        setText('DocDate', X.today()); setText('txtRemarks', '');
        setRcvdEnabled(true); setText('txtAmountRcvd', '');
        GDS = []; X.bind('CmbGdNo', GDS, 'Id', 'GdNo', []);
        ['txtGdValue', 'txtInvoiceNo', 'txtFttAmount', 'txtRealizedAmount', 'txtStatus', 'txtRemarksDetail', 'txtFobValue', 'txtGdValueBalance'].forEach(function (id) { setText(id, ''); });
        X.show('btnSave', true); X.show('btnUpdate', false);
        X.show('btnAdd', true); X.show('btnDetailUpdate', false); X.show('btnDetailCancel', false);
        X.focus('DocDate');
        DT = []; render();
        return X.getJson(API + '/generate-code').then(function (d) { if (d && netI(d.docNo) > 0) setText('txtDocNo', d.docNo); }).catch(function (e) { box(e.message); });
    }
    /* Insert() */
    function insert(btn) {
        return X.busy(btn, function () {
            /* formvalidation */
            if (!val('txtDocNo') || netI(val('txtDocNo')) === 0) { box('Doc No Is Required'); X.focus('txtDocNo'); return Promise.resolve(); }
            if (netI(val('CmbBank')) <= 0) { box('Bank Is Required'); X.focus('CmbBank'); return Promise.resolve(); }
            if (!rcvdEnabled) { box('You Cant Save Record Beacuse Bank Recieved Amount Is not Completly Proportionated'); return Promise.resolve(); }
            if (DT.length <= 0) { box('Grid Record not found'); return Promise.resolve(); }
            var seen = [];
            for (var i = 0; i < DT.length; i++) {
                var sc = netI(DT[i].SubCode);
                if (seen.indexOf(sc) >= 0) continue;
                seen.push(sc);
                var sum = sumWhere('Realized', function (r) { return netI(r.SubCode) === sc; });
                if (netD(DT[i].AmountRcvd) !== sum) { box('Amount Received ' + str(DT[i].AmountRcvd) + ' not Equal To Total Realized ' + sum); return Promise.resolve(); }
            }
            if (!ask(RecId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return Promise.resolve();
            var body = {
                recId: RecId, docNo: netI(val('txtDocNo')), docDate: val('DocDate') || X.today(), bankId: netI(val('CmbBank')), remarks: val('txtRemarks'),
                amountRcvdEnabled: rcvdEnabled, rows: DT, removed: REMOVED
            };
            return X.postJson(API + '/save', body).then(function (d) {
                box((d && d.message) || (RecId === 0 ? 'Save SuccessFully' : 'Update SuccessFully'));
                return reset().then(function () { return X.getJson(API + '/history-banks').then(function (rows) { HIST_BANKS = rows || []; X.bind('CmbBankHistory', HIST_BANKS, 'Id', 'Name', []); }); });
            }).catch(function (e) { box(e.message); });
        });
    }
    function save(btn) { RecId = 0; return insert(btn); }
    function update(btn) { return insert(btn); }
    /* btnRefresh_Click -> BankFill */
    function refresh(btn) { return X.busy(btn, function () { return X.getJson(API + '/banks').then(function (rows) { BANKS = rows || []; X.bind('CmbBank', BANKS, 'Id', 'Name', []); }).catch(function (e) { box(e.message); }); }); }
    /* ReadById(ID) */
    function readById(id) {
        return X.getJson(API + '/by-id?id=' + id).then(function (o) {
            o = o || {};
            RecId = netI(o.Id);
            X.show('btnSave', false); X.show('btnUpdate', true);
            X.innerTab('gbr', 'gbrForm', 'btnGbrFooterHistory');
            setText('DocDate', X.isoDate(o.DocDate)); setText('txtDocNo', str(o.DocNo));
            X.setVal('CmbBank', o.BankId);
            return bankLeave().then(function () {
                X.setEnabled('CmbBank', false);
                setText('txtRemarks', o.RemarksHeader);
                DT = (o.details || []).map(function (d) { return X.copy(d); });
                CUR = -1; render();
            });
        }).catch(function (e) { box(e.message); });
    }
    /* BtnPrint_Click -> CommonServices.GDBreakUpBankRequest_Slip560(RecId). */
    function print(btn) {
        if (RecId <= 0) { box('Record Not Found'); return Promise.resolve(); }
        return X.print('560-gdbreakupbankrequest-slip', { id: RecId }, btn);
    }
    function attachment() { box('Attachments (Attachment form) are not part of the web port.'); }
    function shortcuts() {
        X.shortcutPopup([['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'],
            ['Ctrl+F5', 'For Focus on Doc Date'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
            ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus in Detail Entry'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'],
            ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }

    // ------------------------------------------------------------------ history
    var HCOLS = [
        { key: '_e', html: function (v, r, i) { return '<button type="button" class="win-edit" data-edit="' + i + '">Edit</button>'; }, cls: 'win-cell-btn' },
        { key: '_s', html: function (v, r, i) { return '<button type="button" class="win-edit" data-act="slip" data-i="' + i + '">Slip</button>'; }, cls: 'win-cell-btn' },
        { key: 'DocNo', num: true, link: true, fmt: function (v) { return str(netI(v)); } }, { key: 'DocDate', fmt: X.shortDate }, 'BankName',
        { key: 'EntryDate', fmt: X.shortDate }, 'EntryUser', { key: 'ModifyDate', fmt: X.shortDate }, 'ModifyUser',
        { key: 'NoOfAttachments', num: true, fmt: function (v) { return str(netI(v)); } }, 'Remarks'
    ];
    var HDCOLS = [
        { key: 'SubCode', num: true, fmt: function (v) { return str(netI(v)); } }, { key: 'AmountRcvd', num: true, fmt: fmt }, 'GdNo',
        { key: 'GdValue', num: true, fmt: fmt }, { key: 'FobValue', num: true, fmt: fmt }, { key: 'Realized', num: true, sum: true, fmt: fmt }, 'Status', 'Remarks'
    ];
    function histRender() { X.drawGrid('gbrHistBody', 'gbrHistFoot', HIST, HCOLS, { cur: HIST_CUR, empty: 'gbrHistEmpty' }); }
    /* HistoryFill */
    function historyShow(btn) {
        return X.busy(btn, function () {
            var kind = (document.querySelector('input[name="gbrDateKind"]:checked') || {}).value || 'doc';
            var body = { dateKind: kind, fromChecked: $id('FromDateHistoryChk').checked, fromDate: val('FromDateHistory'), toChecked: $id('ToDateHistoryChk').checked, toDate: val('ToDateHistory'),
                fromDocNo: netI(val('FromDocNo')), toDocNo: netI(val('ToDocNo')), bankId: netI(val('CmbBankHistory')) };
            return X.postJson(API + '/history', body).then(function (rows) { HIST = rows || []; HIST_CUR = -1; histRender(); HIST_DET = []; X.drawGrid('gbrHistDetBody', 'gbrHistDetFoot', HIST_DET, HDCOLS, {}); })
                .catch(function (e) { box(e.message); });
        });
    }
    /* DataGridHistory_SelectionChanged -> GetByID detail list. */
    function histSelect(i) {
        HIST_CUR = i; var r = HIST[i]; if (!r) return;
        X.getJson(API + '/history-detail?id=' + netI(col(r, 'Id'))).then(function (rows) { HIST_DET = rows || []; X.drawGrid('gbrHistDetBody', 'gbrHistDetFoot', HIST_DET, HDCOLS, {}); }).catch(function (e) { box(e.message); });
    }
    /* btnNewHistory_Click */
    function historyReset() {
        setText('FromDateHistory', X.daysAgo(7)); setText('ToDateHistory', X.today());
        setText('FromDocNo', ''); setText('ToDocNo', ''); X.setVal('CmbBankHistory', '0');
        HIST = []; HIST_CUR = -1; histRender(); X.show('gbrHistEmpty', false);
        HIST_DET = []; X.drawGrid('gbrHistDetBody', 'gbrHistDetFoot', HIST_DET, HDCOLS, {});
    }
    /* btnRefreshHistory_Click */
    function historyRefresh(btn) { return X.busy(btn, function () { return X.getJson(API + '/history-banks').then(function (rows) { HIST_BANKS = rows || []; X.bind('CmbBankHistory', HIST_BANKS, 'Id', 'Name', []); }).catch(function (e) { box(e.message); }); }); }
    /* tabControl1_SelectedIndexChanged: History tab -> dates = today-7 .. today and HistoryFill. */
    function tab(group, panelId) {
        X.innerTab(group, panelId, 'btnGbrFooterHistory', function (p, onHist) {
            if (onHist) { setText('FromDateHistory', X.daysAgo(7)); setText('ToDateHistory', X.today()); historyShow($id('btnShow')); X.focus('FromDateHistory'); }
            else X.focus('DocDate');
        });
    }
    function toggleHistory() { tab('gbr', X.activeInner('gbr') === 'gbrHistory' ? 'gbrForm' : 'gbrHistory'); }

    document.addEventListener('DOMContentLoaded', function () {
        X.wireTabs(tab);
        X.on('CmbBank', 'change', bankLeave);
        X.on('txtAmountRcvd', 'change', amountLeave);
        X.on('CmbGdNo', 'change', gdLeave);
        X.wireGrid('gbrBody', { del: deleteRow, open: editRow, select: function (i) { CUR = i; } });
        X.wireGrid('gbrHistBody', {
            open: function (i) { var r = HIST[i]; if (r) { reset().then(function () { readById(netI(col(r, 'Id'))); }); } },
            select: histSelect,
            act: function (a, i) { var r = HIST[i]; if (r && a === 'slip') X.print('560-gdbreakupbankrequest-slip', { id: netI(col(r, 'Id')) }); }
        });
        /* GdBankRequest_KeyDown */
        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase();
            if (X.enterMovesOn(e)) return;
            var onForm = X.activeInner('gbr') === 'gbrForm';
            if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcuts(); return; }
            if (e.ctrlKey && k === 's' && !$id('btnSave').classList.contains('is-hidden') && !$id('btnSave').disabled) { e.preventDefault(); save($id('btnSave')); return; }
            if (e.ctrlKey && k === 'u' && !$id('btnUpdate').classList.contains('is-hidden') && !$id('btnUpdate').disabled) { e.preventDefault(); update($id('btnUpdate')); return; }
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); X.cancel(); return; }
            if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); return; }
            if (e.ctrlKey && k === 't') { e.preventDefault(); toggleHistory(); return; }
            if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh($id('btnRefresh')); return; }
            if (e.ctrlKey && k === 'p' && PERM.Print) { e.preventDefault(); print($id('BtnPrint')); return; }
            if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); X.focus('DocDate'); return; }
            if (e.ctrlKey && e.key === 'F10') { e.preventDefault(); attachment(); return; }
            if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); var tr = $id(onForm ? 'gbrBody' : 'gbrHistBody').querySelector('tr'); if (tr) tr.scrollIntoView(); return; }
            if (e.ctrlKey && e.key === 'ArrowUp') { e.preventDefault(); X.focus(rcvdEnabled ? 'txtAmountRcvd' : 'CmbGdNo'); return; }
            if (e.ctrlKey && e.key === 'Enter') { e.preventDefault(); if (onForm && CUR >= 0) editRow(CUR); else if (!onForm && HIST_CUR >= 0) { var r = HIST[HIST_CUR]; reset().then(function () { readById(netI(col(r, 'Id'))); }); } }
        });
        load();
    });

    global.ExportGdBankRequest = {
        reset: reset, refresh: refresh, save: save, update: update, print: print, attachment: attachment, shortcuts: shortcuts,
        addRow: addRow, updateRow: updateRow, cancelRow: cancelRow,
        historyShow: historyShow, historyReset: historyReset, historyRefresh: historyRefresh, toggleHistory: toggleHistory
    };
}(window));
