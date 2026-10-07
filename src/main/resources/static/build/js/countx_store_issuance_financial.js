/* ==========================================================================================
 * Store Issuance (Financials) - desktop Architecture.WinApp.StoreManagement.StoreIssuanceFinancial (ScreenId 33)
 *   :99 frmGSIssuance_Load  :143 GenerateDocNo  :158 DetailAccountFill  :193 Insert  :336 btnUpdate_Click
 *   :395 formReset  :452 Print_Click  :477 form KeyDown  :515 ReadById  :586 DetailGridSettings
 *   :466 txtDocNo_TextChanged  :552 HistoryFill / HistoryGridSettings  :672 DataGridHistory_ColumnButtonClick
 * ==========================================================================================*/
(function () {
    'use strict';
    var C = window.StoreCommon;
    var API = '/api/store/store-issuance-financial';
    var S = { recId: 0, headId: 0, requestType: '', updateMode: false, canUpdate: false, rows: [], hist: [], tab: 1 };

    function $(id) { return document.getElementById(id); }
    function esc(v) { return C.esc(v); }
    function txt(v) { return v === null || v === undefined ? '' : String(v); }
    function nf(v) { var n = parseFloat(v); return isNaN(n) ? '0' : String(Math.round(n * 10000) / 10000); }
    function fail(e) { alert(e && e.message ? e.message : String(e)); }

    /* ---------------------------------------------------------------- tabs (tabControlGsIssuance: 0 Form, 1 History) */
    function selectTab(i) {
        S.tab = i;
        $('paneForm').classList.toggle('sif-hidden', i !== 0);
        $('paneHistory').classList.toggle('sif-hidden', i !== 1);
        $('tabForm').classList.toggle('is-on', i === 0);
        $('tabHistory').classList.toggle('is-on', i === 1);
        historyFill();                                           // tabControlGsIssuance_SelectedIndexChanged :552
    }

    /* ---------------------------------------------------------------- load */
    function setDocNo(n) { if (parseInt(n, 10) > 0) $('txtDocNo').value = String(n); }   // GenerateDocNo: only when code > 0

    function load() {
        C.getJson(API + '/setup').then(function (d) {
            S.canUpdate = !!(d.rights && d.rights.update);
            setDocNo(d.docNo);
            C.fillSelectCols('CmbAccountId', d.accounts, 'Id', 'AccountTitle', [], ['Account Title'], false);
            var a = $('CmbAccountId'); if (a.options.length) a.selectedIndex = 0;
            $('txtDocdate').value = d.today || C.today();
            renderDetail([]);
            selectTab(1);                                         // :104 SelectedIndex = 1
        }).catch(function (e) {
            var n = $('rightsNote'); if (n) { n.textContent = e.message; n.classList.remove('is-hidden'); }
            selectTab(1);
            fail(e);
        });
    }

    /* ---------------------------------------------------------------- History (HistoryFill :552) */
    function historyFill() {
        return C.getJson(API + '/history').then(function (rows) {
            S.hist = rows || [];
            renderHistory();
        }).catch(fail);
    }
    function renderHistory() {
        var cols = [['Edit', 'Edit'], ['DocDate', 'DocDate'], ['DocNo', 'DocNo'], ['Remarks', 'Remarks'], ['ItemName', 'ItemName'],
            ['Equivalent', 'Equivalent'], ['IssueQty', 'IssueQty'], ['WareHouseName', 'WareHouseName'], ['DepartmentName', 'DepartmentName'],
            ['AssetName', 'AssetName'], ['EntryDate', 'EntryDate'], ['EntryUser', 'EntryUser'], ['ModifyDate', 'ModifyDate'], ['ModifyUser', 'ModifyUser']];
        var t = $('DataGridHistory');
        if (!S.hist.length) { t.querySelector('thead').innerHTML = ''; t.querySelector('tbody').innerHTML = ''; return; }   // ClearStructure
        t.querySelector('thead').innerHTML = '<tr>' + cols.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('') + '</tr>';
        t.querySelector('tbody').innerHTML = S.hist.map(function (r, i) {
            return '<tr data-i="' + i + '">' + cols.map(function (c) {
                var k = c[0], v = r[k];
                if (k === 'Edit') return '<td class="btn"><button type="button" class="fx-btn sif-edit" data-i="' + i + '" style="padding:0 6px">Edit</button></td>';
                if (k === 'DocDate') return '<td>' + esc(C.gridDate(v)) + '</td>';
                if (k === 'EntryDate' || k === 'ModifyDate') return '<td>' + esc(C.gridDateTime(v, true)) + '</td>';
                if (k === 'Equivalent' || k === 'IssueQty') return '<td class="num">' + nf(v) + '</td>';
                if (k === 'DocNo') return '<td class="num">' + esc(txt(v)) + '</td>';
                return '<td>' + esc(txt(v)) + '</td>';
            }).join('') + '</tr>';
        }).join('');
    }

    /* ---------------------------------------------------------------- Edit (ReadById :515) */
    function edit(id) {
        return C.getJson(API + '/load' + C.qs({ id: id })).then(function (h) {
            S.recId = h.Id;
            S.headId = h.voucherHeadId || 0;
            S.requestType = h.RequestType;
            S.rows = h.rows || [];
            $('txtDocdate').value = C.isoDay(h.DocDate);
            $('txtDocNo').value = String(h.DocNo);
            $('txtRemarks').value = h.Remarks || '';
            var a = $('CmbAccountId'); a.value = String(h.refAccountId || 0);
            if (a.value !== String(h.refAccountId || 0) && a.options.length) a.selectedIndex = 0;
            renderDetail(S.rows);
            S.updateMode = true;
            $('btnUpdate').classList.toggle('sif-hidden', false);
            $('btnUpdate').disabled = !S.canUpdate;               // btnUpdate.Enabled = DoHaveUpdateRights
            selectTab(0);
        }).catch(fail);
    }

    /* ---------------------------------------------------------------- detail grid (DetailGridSettings :586) */
    function renderDetail(rows) {
        var deliv = S.requestType === 'DeliveryOrder';
        var cols = [['Item', 'Item'], ['Warehouse', 'Warehouse'], ['RackName', 'RackName'], ['ItemCondition', 'ItemCondition'], ['Unit', 'Unit'],
            ['IssueQty', 'IssueQty', 1], ['ItemRate', 'ItemRate', 1], ['ItemAmount', 'ItemAmount', 1]];
        if (!deliv) cols.push(['Department', 'Department'], ['Asset', 'Asset']);
        cols.push(['Remarks', 'Remarks']);
        if (deliv) cols.push(['SupplierName', 'SupplierName']);
        var t = $('grdDetail');
        if (!rows || !rows.length) { t.querySelector('thead').innerHTML = ''; t.querySelector('tbody').innerHTML = ''; t.querySelector('tfoot').innerHTML = ''; return; }
        t.querySelector('thead').innerHTML = '<tr>' + cols.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('') + '</tr>';
        t.querySelector('tbody').innerHTML = rows.map(function (r, i) {
            return '<tr data-i="' + i + '">' + cols.map(function (c) {
                var v = r[c[0]];
                if (c[0] === 'Remarks') return '<td><input type="text" class="sif-rem" data-i="' + i + '" value="' + esc(txt(v)) + '"></td>';
                if (c[2]) return '<td class="num">' + nf(v) + '</td>';
                if (c[0] === 'Unit') return '<td class="num">' + nf(v) + '</td>';
                return '<td>' + esc(txt(v)) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        var f = '';
        if (!deliv) {                                             // AggregateFunction.Sum on ItemAmount and IssueQty (TotalRow)
            var sIq = 0, sIa = 0;
            rows.forEach(function (r) { sIq += parseFloat(r.IssueQty) || 0; sIa += parseFloat(r.ItemAmount) || 0; });
            f = '<tr>' + cols.map(function (c) {
                if (c[0] === 'IssueQty') return '<td>' + nf(sIq) + '</td>';
                if (c[0] === 'ItemAmount') return '<td>' + nf(sIa) + '</td>';
                return '<td></td>';
            }).join('') + '</tr>';
        }
        t.querySelector('tfoot').innerHTML = f;
    }

    /* ---------------------------------------------------------------- New / reset (formReset :395) */
    function formReset() {
        S.recId = 0; S.headId = 0; S.requestType = ''; S.updateMode = false; S.rows = [];
        $('btnUpdate').classList.add('sif-hidden');
        renderDetail([]);
        $('txtRemarks').value = '';
        selectTab(1);
        C.getJson(API + '/doc-no').then(function (d) { setDocNo(d.docNo); }).catch(fail);
    }

    /* ---------------------------------------------------------------- Update (btnUpdate_Click :336 -> Insert :193) */
    function update() {
        if (S.recId === 0) { alert('Record Not Found For Update'); return Promise.resolve(); }
        if ($('txtDocNo').value.trim() === '') { alert('document Number Field Required'); $('txtDocNo').focus(); return Promise.resolve(); }
        if (!confirm('Are you sure to Update?')) return Promise.resolve();
        var body = { Id: S.recId, DocNo: $('txtDocNo').value.trim(), DocDate: $('txtDocdate').value, Remarks: $('txtRemarks').value,
            AccountId: $('CmbAccountId').value };
        var preview = $('ChkPrintPreview').checked;
        return C.withBusy('btnUpdate', function () {
            return C.postJson(API + '/update', body).then(function (d) {
                alert(d.message);
                var vid = d.voucherHeadId || 0;
                formReset();
                if (preview) printVoucher(vid);
            }).catch(fail);
        });
    }

    /* ---------------------------------------------------------------- Print (Print_Click :452 -> VoucherReport_118) */
    function printVoucher(headId) {
        if (!(headId > 0)) { alert('VoucherId Not Found'); return; }
        window.CrystalPrint.open('acc-118', { id: headId, documentTypeId: 451 });
    }

    /* ---------------------------------------------------------------- events */
    function wire() {
        $('tabForm').addEventListener('click', function () { selectTab(0); });
        $('tabHistory').addEventListener('click', function () { selectTab(1); });
        $('btnnew').addEventListener('click', formReset);
        $('btnUpdate').addEventListener('click', update);
        $('Print').addEventListener('click', function () { printVoucher(S.headId); });
        $('btnRefresh').addEventListener('click', function () {
            C.getJson(API + '/doc-no').then(function (d) { setDocNo(d.docNo); }).catch(fail);
        });
        $('btnStoreIssuance').addEventListener('click', function () { window.open('/store/store-issuance', '_blank'); });
        $('DataGridHistory').addEventListener('click', function (e) {
            var b = e.target.closest('.sif-edit'); if (!b) return;
            var r = S.hist[parseInt(b.getAttribute('data-i'), 10)];
            if (r) edit(r.Id);
        });

        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase();
            if (e.key === 'Enter' && !e.ctrlKey && !e.altKey) {
                var a = document.activeElement;
                if (a && ((a.tagName === 'INPUT' && a.type !== 'checkbox') || a.tagName === 'SELECT')) {
                    e.preventDefault();
                    var f = Array.prototype.filter.call(document.querySelectorAll('input,select,button'), function (x) { return !x.disabled && x.offsetParent !== null && x.tabIndex >= 0; });
                    var n = f[f.indexOf(a) + 1]; if (n) n.focus();
                }
            }
            if (e.ctrlKey && k === 'n') { e.preventDefault(); formReset(); }
            if (e.ctrlKey && k === 't') { e.preventDefault(); selectTab(S.tab === 1 ? 0 : 1); }
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); window.location.href = '/modules'; }
            if (e.ctrlKey && k === 'u') { e.preventDefault(); if (S.updateMode && S.canUpdate) update(); }
            if (e.ctrlKey && k === 'p') { e.preventDefault(); printVoucher(S.headId); }
        });
    }

    document.addEventListener('DOMContentLoaded', function () { wire(); load(); });
})();
