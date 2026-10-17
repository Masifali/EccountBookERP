/* ============================================================================================
 * Screen 594 "Work Order In Progress" - Architecture.WinApp.Mfg.frmWorkOrderInProgress (base.Name "frmWorkOrderInProgress", DocumentTypeId 1620).
 * Desktop line numbers (.cs): _Load:235, ItemFill:315, uomgetAll:356, CmbItemDetail_Leave:397, btnnew_Click:437, FormReset:450, DetailFormReset:471,
 * FillgrdWorkOrderMain:514, grdWorkorderMainSettings:550, grdWorkOrderMain_LinkClicked:648, grd_DoubleClick:679, BtnUpdateDetail_Click:723,
 * GridDetailSetting:759, btnsave_Click:852, Insert:871, ReadById:992, MakeShortCutKeys:1658, btnPrint_Click:1777.
 * API /production/mfg/work-order-in-progress/api. Not ported: Attachment dialog; History tab (the desktop removes it at load, :268).
 * ============================================================================================ */
(function () {
    'use strict';
    var api = '/production/mfg/work-order-in-progress/api';
    var $ = W3B.$;
    var rights = {}, uoms = [];
    var recId = 0, editIdx = -1;
    var main = [], det = [];

    function applyRights() {                                    // :241-:242
        $('btnUpdate').disabled = !rights.save; $('btnPrint').disabled = !rights.print;
        var m = [];
        if (!rights.save) m.push('Save'); if (!rights.print) m.push('Print');
        var n = $('rightsNote');
        n.textContent = m.length ? 'Your account does not have the ' + m.join(', ') + ' right on this screen, so those buttons are off.' : '';
        n.classList.toggle('is-hidden', !m.length);
    }
    function datePart(v) { var m = /^(\d{4}-\d{2}-\d{2})/.exec(String(v || '')); return m ? m[1] : ''; }
    function timePart(v) { var m = /[T ](\d{2}:\d{2}(?::\d{2})?)/.exec(String(v || '')); return m ? m[1] : ''; }
    function withDate(old, d) { return d === datePart(old) ? old : d; }                 // keep the loaded time-of-day when the day is unchanged
    function withTime(old, t, today) {
        if (!t) return old || null;
        var tt = t.length === 5 ? t + ':00' : t;
        return (datePart(old) || today) + 'T' + tt;
    }

    // ----------------------------------------------------------------------------- main grid (grdWorkOrderMain :550)
    function renderMain() {
        var total = main.reduce(function (s, r) { return s + W3B.num(r.ItemQty); }, 0);
        var cols = [
            { h: 'Doc No', k: 'DocNo' }, { h: 'Doc Date', f: function (r) { return W3B.fmtDMY(r.DocDate); } },
            { h: 'Work Order No', html: true, f: function (r, i) { return '<button type="button" class="rc-link" data-main="' + i + '">' + W3B.esc(r.WorkOrderNo) + '</button>'; } },
            { h: 'Bom Header No', k: 'BomHeaderNo' }, { h: 'Item Name', k: 'ItemName' }, { h: 'Item Code', k: 'ItemCode' }, { h: 'Item Uom Code', k: 'ItemUomCode' },
            { h: 'Item Qty', cls: 'num', f: function (r) { return W3B.fmt(r.ItemQty, 2); } },
            { h: 'Start Date', html: true, f: function (r, i) { return '<input type="date" class="cell txt" style="width:125px" data-m="StartDate" data-i="' + i + '" value="' + W3B.esc(datePart(r.StartDate)) + '">'; } },
            { h: 'End Date', html: true, f: function (r, i) { return '<input type="date" class="cell txt" style="width:125px" data-m="EndDate" data-i="' + i + '" value="' + W3B.esc(datePart(r.EndDate)) + '">'; } },
            { h: 'Dead Line', f: function (r) { return W3B.fmtDMY(r.DeadLine); } },
            { h: 'Remarks Header', html: true, f: function (r, i) { return '<input type="text" class="cell txt" style="width:200px" data-m="RemarksHeader" data-i="' + i + '" value="' + W3B.esc(r.RemarksHeader) + '">'; } }
        ];
        var h = W3B.tableHtml(cols, main, { empty: 'No records', rowClass: function (r) { return recId > 0 && r.Id === recId ? 'is-current' : ''; } });
        h += '<tfoot><tr><td colspan="7"></td><td>' + W3B.esc(W3B.fmt(total, 2)) + '</td><td colspan="4"></td></tr></tfoot>';      // ItemQty sum :583
        $('grdWorkOrderMain').innerHTML = h;
    }
    function loadMain() { return W3B.getJson(api + '/main').then(function (rows) { main = rows; renderMain(); }); }

    document.addEventListener('input', function (e) {
        var t = e.target; if (!t.dataset || t.dataset.m === undefined) return;
        var r = main[+t.dataset.i]; if (!r) return;
        if (t.dataset.m === 'RemarksHeader') r.RemarksHeader = t.value;
        else r[t.dataset.m] = t.value ? withDate(r[t.dataset.m], t.value) : null;
    });
    document.addEventListener('click', function (e) {
        var b = e.target.closest ? e.target.closest('button[data-main]') : null;
        if (!b) return;
        var r = main[+b.dataset.main]; if (!r) return;
        if (det.length > 0 && !W3B.ask('Are you sure to Reload Data For Detail?')) return;                       // :656
        det = [];
        readById(r.Id);
    });

    // ----------------------------------------------------------------------------- detail grid (grd :759)
    function renderDet() {
        var total = det.reduce(function (s, r) { return s + W3B.num(r.Qty); }, 0);
        var h = W3B.tableHtml([
            { h: 'Edit', html: true, cls: 'w3-btn', w: 50, f: function (r, i) { return '<button type="button" class="rc-cellbtn" data-edit="' + i + '">Edit</button>'; } },
            { h: 'ItemName', k: 'ItemName' }, { h: 'UOM', k: 'UOM' }, { h: 'Qty', cls: 'num', f: function (r) { return W3B.fmt(r.Qty, 0); } },
            { h: 'WorkStationName', k: 'WorkStationName' }, { h: 'OperationName', k: 'OperationName' },
            { h: 'StartDate', f: function (r) { return W3B.fmtDMY(r.StartDate); } }, { h: 'StartTime', f: function (r) { return timePart(r.StartTime); } },
            { h: 'CompleteDate', f: function (r) { return W3B.fmtDMY(r.CompleteDate); } }, { h: 'CompleteTime', f: function (r) { return timePart(r.CompleteTime); } },
            { h: 'Remarks', k: 'Remarks' }
        ], det, { empty: 'No records', rowClass: function (r, i) { return i === editIdx ? 'is-current' : ''; } });
        h += '<tfoot><tr><td colspan="3"></td><td>' + W3B.esc(W3B.fmt(total, 0)) + '</td><td colspan="7"></td></tr></tfoot>';      // Qty sum, format #,##0 :770
        $('grd').innerHTML = h;
    }
    document.addEventListener('click', function (e) {
        var b = e.target.closest ? e.target.closest('button[data-edit]') : null;
        if (b) editRow(+b.dataset.edit);
    });
    document.addEventListener('dblclick', function (e) {                    // grd_DoubleClick:679
        var tr = e.target.closest ? e.target.closest('#grd tbody tr[data-i]') : null;
        if (tr) editRow(+tr.dataset.i);
    });

    function uomOptions(itemId, keep) {                                      // CmbItemDetail_Leave:397
        var rows = uoms.filter(function (u) { return String(u.ItemId) === String(itemId); });
        if (!rows.length) return;                                            // desktop returns without rebinding when the item has no UOM
        W3B.fill('cmbItemUOM', rows, false, keep);
    }
    function itemLeave() { uomOptions($('CmbItemDetail').value, $('cmbItemUOM').value); }

    function editRow(i) {
        var r = det[i]; if (!r) return;
        editIdx = i;
        $('CmbItemDetail').value = String(r.ItemId);
        uomOptions(r.ItemId, String(r.UOMId));
        W3B.setCombo('cmbItemUOM', r.UOMId);
        $('txtQtyDetail').value = W3B.fmt(r.Qty, 3);
        $('txtOperation').value = r.OperationName || ''; $('txtWorkStation').value = r.WorkStationName || '';
        $('txtStartDate').value = datePart(r.StartDate); $('txtStartTime').value = timePart(r.StartTime).slice(0, 8);
        $('txtCompleteDate').value = datePart(r.CompleteDate); $('txtCompleteTime').value = timePart(r.CompleteTime).slice(0, 8);
        $('txtRemarksDetail').value = r.Remarks || '';
        W3B.show('BtnUpdateDetail', true); W3B.show('BtnCancelDetail', true);
        renderDet(); W3B.focus('txtStartDate');
    }
    function updateDetail() {                                                // BtnUpdateDetail_Click:723
        var r = det[editIdx]; if (!r) return;
        var today = W3B.today();
        r.ItemId = W3B.int($('CmbItemDetail').value); r.UOMId = W3B.int($('cmbItemUOM').value);
        r.Qty = W3B.num($('txtQtyDetail').value);
        r.WorkStationName = $('txtWorkStation').value; r.OperationName = $('txtOperation').value;      // ItemName / UOM texts are not refreshed (desktop :727-:738)
        var sd = $('txtStartDate').value, cd = $('txtCompleteDate').value;
        r.StartDate = sd ? withDate(r.StartDate, sd) : null;
        r.StartTime = withTime(r.StartTime, $('txtStartTime').value, today);
        r.CompleteDate = cd ? withDate(r.CompleteDate, cd) : null;
        r.CompleteTime = withTime(r.CompleteTime, $('txtCompleteTime').value, today);
        r.Remarks = $('txtRemarksDetail').value;
        detailReset(); renderDet(); W3B.focus('txtStartDate');
    }
    function cancelDetail() { detailReset(); renderDet(); }
    function detailReset() {                                                 // DetailFormReset:471
        editIdx = -1;
        W3B.clearCombo('CmbItemDetail'); W3B.clearCombo('cmbItemUOM');
        ['txtQtyDetail', 'txtWorkStation', 'txtOperation'].forEach(function (i) { $(i).value = ''; });
        W3B.show('BtnUpdateDetail', false); W3B.show('BtnCancelDetail', false);
    }

    // ----------------------------------------------------------------------------- toolbar
    function readById(id) {                                                  // ReadById:992
        if (id <= 0) { det = []; renderDet(); return; }
        recId = id;
        return W3B.getJson(api + '/read/' + id).then(function (d) {
            if (d.details && d.details.length) {
                main = d.main || main; det = d.details;                    // FillgrdWorkOrderMain() then the detail rows
            } else { det = []; }
            editIdx = -1; renderMain(); renderDet();
        }).catch(W3B.fail);
    }
    function formReset() { recId = 0; editIdx = -1; det = []; renderDet(); return loadMain(); }          // FormReset:450
    function newClick() { formReset().then(detailReset).catch(W3B.fail); }                                // btnnew_Click:437
    function refresh() {                                                     // btnRefresh_Click:489 = ItemFill + uomgetAll
        W3B.getJson(api + '/refresh').then(applyCombos).catch(W3B.fail);
    }
    function applyCombos(d) {
        var keep = $('CmbItemDetail').value;
        W3B.fill('CmbItemDetail', d.items, true, keep);
        uoms = (d.allUoms || []).map(function (u) { return { Id: u.Id, ItemId: u.ItemId, Name: u.UOMDescription || u.UOMCode }; });
    }
    function save() {                                                        // btnsave_Click:852 / Insert:871
        if (recId === 0) { W3B.box('Please Select Any Record first...'); return; }
        if (!main.length) { W3B.box('Grid WorkOrderMain Record Not Found...\nGrid Should have atleast one record'); return; }
        if (!det.length) { W3B.box('Grid Record Not Found...\nGrid Should have atleast one record'); return; }
        if (!W3B.ask('Are you sure to Update?')) return;
        var row = main.filter(function (r) { return r.Id === recId; })[0] || {};
        W3B.postJson(api + '/save', { id: recId, startDate: row.StartDate, endDate: row.EndDate, remarksHeader: row.RemarksHeader, details: det })
            .then(function (d) { W3B.box(d.message); return formReset().then(detailReset); }).catch(W3B.fail);
    }
    function print() {                                                       // btnPrint_Click:1777 -> WorkOrderInProgress_Slip1620
        if (recId <= 0) { W3B.box('No Record Found For Display'); return; }
        window.open('/reports/print/1620-work-order-in-progress-slip?id=' + recId, '_blank');
    }
    function keys() {                                                        // MakeShortCutKeys:1658
        W3B.box(['Ctrl+S / Ctrl+U : For Update', 'Ctrl+E : For Close', 'Ctrl+N : For New', 'Ctrl+P : For Print', 'Ctrl+F5 : For Focus on Main Grid',
            'Ctrl+Up : For Focus On StartDate in Detail Box', 'Ctrl+Down : For Focus On Detail Grid'].join('\n'));
    }

    window.Wip = { newClick: newClick, refresh: refresh, save: save, print: print, keys: keys, itemLeave: itemLeave, updateDetail: updateDetail, cancelDetail: cancelDetail };

    W3B.keys({
        n: newClick, s: save, u: save, p: print,
        e: function () { window.location.href = '/production'; }, escape: function () { window.location.href = '/production'; },
        arrowup: function () { W3B.focus('txtStartDate'); }, arrowdown: function () { var t = $('grd'); if (t) t.scrollIntoView(); }
    });

    W3B.getJson(api + '/lookups').then(function (d) {
        rights = d.rights || {}; applyRights(); applyCombos(d);
        main = d.main || []; renderMain(); renderDet();
    }).catch(W3B.fail);
}());
