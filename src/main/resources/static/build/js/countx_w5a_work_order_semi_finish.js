/* ============================================================================================
 * Screen 781 "Work Order For Semi Finish" - Architecture.WinApp.Mfg.frmWorkOrderForSemiFinish (base.Name "frmWorkOrderForSemiFinish", DocumentTypeId 1621).
 * Desktop line numbers (.cs): _Load:264, DocumentNoFill:335, GetItemCategory:362, ItemModalBind:423, ItemDetailBind:494, CastingType:555, FillItemFinishWeight:589,
 * CmbCategory_Leave:607, cmbItem_Leave:620, ItemUomBind:640, bindvarientunit:676, CmbSampleModal_Leave:777, FormValidation:794, FormValidationOperationDetail:823,
 * BtnAddOp_Click:852, BtnUpdateOp_Click:871, grdDetail_DoubleClick:913, DeleteOperationDetailRow:1023, FromReset:1074, FromResetDetail:1105, btnRefresh_Click:1129,
 * Insert:1218, ReadById:1375, btnDelete_Click:1428, btnSaveAs_Click:1452, btnPrint_Click:1513, HistoryGridFill:1561, DatagridHistory_ColumnButtonClick:1712,
 * DataGridHistory_SaveAs:1762, DatagridHistoryDetailbind:1796, key handler:1898, MakeShortCutKeys:2019.
 * API /production/mfg/work-order-semi-finish/api. Not ported: Attachment dialog, Define Variant / Define Item pop-ups. Order No combo is never bound in the desktop.
 * ============================================================================================ */
(function () {
    'use strict';
    var api = '/production/mfg/work-order-semi-finish/api';
    var $ = W3B.$;
    var look = { categories: [], modals: [], items: [], castingTypes: [] };
    var rights = {};
    var recId = 0, updateIdx = -1, updateMode = false, saveAsMode = false;
    var rows = [], removed = [], hist = [];

    function applyRights() {
        $('btnSave').disabled = !rights.save; $('btnSaveAs').disabled = !rights.save; $('btnUpdate').disabled = !rights.update;
        $('btnPrint').disabled = !rights.print; $('btnDelete').disabled = !rights['delete'];
        var m = [];
        if (!rights.save) m.push('Save'); if (!rights.update) m.push('Update'); if (!rights['delete']) m.push('Delete'); if (!rights.print) m.push('Print');
        var n = $('rightsNote');
        n.textContent = m.length ? 'Your account does not have the ' + m.join(', ') + ' right on this screen, so those buttons are off.' : '';
        n.classList.toggle('is-hidden', !m.length);
    }
    function mode() {                                          // FromReset:1074 / ReadById:1375 / DataGridHistory_SaveAs:1762 button visibility
        W3B.show('btnSave', !updateMode && !saveAsMode); W3B.show('btnUpdate', updateMode && !saveAsMode);
        W3B.show('btnDelete', updateMode && !saveAsMode); W3B.show('btnSaveAs', saveAsMode);
    }

    // ----------------------------------------------------------------------------- combos
    function fillOpts(id, list, zero, keep) {
        var el = $(id); if (!el) return;
        var h = zero ? '<option value="0">...Select Any Value...</option>' : '<option value=""></option>';
        (list || []).forEach(function (r) { h += '<option value="' + W3B.esc(r.Id) + '">' + W3B.esc(r.Name) + '</option>'; });
        el.innerHTML = h;
        if (keep === undefined || keep === null) { el.selectedIndex = 0; return; }
        el.value = String(keep);
        if (el.value !== String(keep)) el.selectedIndex = 0;
    }
    function byCode() { return $('rdbtnItemCode').checked; }
    function bindCategories() {                                // GetItemCategory:362
        var prev = W3B.int($('cmbCategory').value);
        fillOpts('cmbCategory', look.categories, false, look.categories.some(function (c) { return c.Id === prev; }) ? prev : null);
    }
    function bindItems(categoryId, modalId) {                  // ItemDetailBind:494
        var prev = W3B.int($('cmbItem').value), list = look.items;
        if (modalId > 0) list = look.items.filter(function (i) { return i.ItemModalId === modalId; });
        else if (categoryId > 0) list = look.items.filter(function (i) { return i.ItemCategoryId === categoryId; });
        if (!list.length) { fillOpts('cmbItem', [], false, null); return; }
        var rowsOut = list.map(function (i) { return { Id: i.Id, Name: byCode() ? i.ItemCode : i.ItemName }; });
        fillOpts('cmbItem', rowsOut, true, prev > 0 && list.some(function (i) { return i.Id === prev; }) ? prev : 0);
    }
    function bindModals(categoryId, itemId) {                  // ItemModalBind:423
        var prev = W3B.int($('cmbSampleModal').value), list = look.modals;
        if (itemId > 0) list = look.modals.filter(function (m) { return m.ItemId === itemId; });
        else if (categoryId > 0) list = look.modals.filter(function (m) { return m.ItemCategoryId === categoryId; });
        var seen = {}, out = [];
        list.forEach(function (m) { if (!seen[m.Id]) { seen[m.Id] = 1; out.push(m); } });
        if (!out.length) { fillOpts('cmbSampleModal', [], false, null); return; }
        fillOpts('cmbSampleModal', out, true, prev > 0 && out.some(function (m) { return m.Id === prev; }) ? prev : 0);
    }
    function bindCasting() {                                   // CastingType:555
        var prev = W3B.int($('cmbCastingTypeDetail').value);
        fillOpts('cmbCastingTypeDetail', look.castingTypes, true, look.castingTypes.some(function (c) { return c.Id === prev; }) ? prev : 0);
    }
    function bindAll() { bindCategories(); bindModals(0, 0); bindItems(0, 0); bindCasting(); }

    function fmt2(v) { return v ? W3B.fmt(v, 2) : ''; }          // "#,##.##" prints nothing for 0
    function fillWeight() {                                    // FillItemFinishWeight:589
        var id = W3B.int($('cmbItem').value), w = 0;
        if (id > 0) {
            var it = look.items.filter(function (i) { return i.Id === id; })[0];
            if (it) w = W3B.int($('cmbCastingTypeDetail').value) === 1 ? it.SandWeight : it.DyeWeight;
        }
        $('txtItemWeight').value = fmt2(w);
        $('txtCastingWeight').value = fmt2(W3B.num($('txtQtyDetaill').value) * w);
    }
    function qtyChanged() {                                    // txtQtyDetaill_TextChanged:731
        $('txtCastingWeight').value = fmt2(W3B.num($('txtQtyDetaill').value) * W3B.num($('txtItemWeight').value));
    }
    function categoryLeave() { var c = W3B.int($('cmbCategory').value); bindItems(c, 0); bindModals(c, 0); }              // CmbCategory_Leave:607
    function itemBy() {                                        // rdbtnItemName_CheckedChanged:745
        var id = W3B.int($('cmbItem').value);
        var rowsOut = look.items.map(function (i) { return { Id: i.Id, Name: byCode() ? i.ItemCode : i.ItemName }; });
        if (look.items.length) fillOpts('cmbItem', rowsOut, true, id > 0 ? id : 0);
    }
    function itemLeave() {                                     // cmbItem_Leave:620 = ItemUomBind + bindvarientunit + ItemModalBind + category + weight
        var id = W3B.int($('cmbItem').value);
        var p = id > 0 ? W3B.getJson(api + '/item-info?itemId=' + id) : Promise.resolve({ uoms: [], variants: [] });
        return p.then(function (d) {
            var prevUom = W3B.int($('cmbUomDetail').value), prevVar = W3B.selText('cmbVariantDescription');
            var uoms = d.uoms || [], vars = d.variants || [];
            fillOpts('cmbUomDetail', uoms, false, uoms.length ? (uoms.some(function (u) { return u.Id === prevUom; }) ? prevUom : uoms[0].Id) : null);
            var match = vars.filter(function (v) { return v.Name === prevVar; })[0];
            fillOpts('cmbVariantDescription', vars, true, match ? match.Id : 0);
            bindModals(0, id);
            if (id > 0) {
                var it = look.items.filter(function (i) { return i.Id === id; })[0];
                if (it) W3B.setCombo('cmbCategory', it.ItemCategoryId);
            }
            fillWeight();
        }).catch(W3B.fail);
    }
    function modalLeave() {                                    // CmbSampleModal_Leave:777
        var id = W3B.int($('cmbSampleModal').value);
        if (id > 0) {
            var m = look.modals.filter(function (x) { return x.Id === id; })[0];
            if (m) W3B.setCombo('cmbCategory', m.ItemCategoryId);
        }
        bindItems(0, id);
    }

    // ----------------------------------------------------------------------------- detail entry + grid
    function validateDetail() {                                // FormValidationOperationDetail:823
        if (!W3B.int($('cmbItem').value)) { W3B.box('Item / Modal Field is Required'); W3B.focus('cmbItem'); return false; }
        if (!W3B.int($('cmbUomDetail').value)) { W3B.box('Uom Field is Required'); W3B.focus('cmbUomDetail'); return false; }
        if (!W3B.int($('cmbCastingTypeDetail').value)) { W3B.box('Casting Type Field is Required'); W3B.focus('cmbCastingTypeDetail'); return false; }
        var q = $('txtQtyDetaill').value.trim();
        if (q === '' || q === '0') { W3B.box('Qty Field is Required'); W3B.focus('txtQtyDetaill'); return false; }
        return true;
    }
    function entryRow(base) {
        var id = W3B.int($('cmbItem').value), it = look.items.filter(function (i) { return i.Id === id; })[0] || {};
        base.ItemId = id; base.ItemCode = it.ItemCode || ''; base.ItemName = it.ItemName || '';
        base.UomId = W3B.int($('cmbUomDetail').value); base.Uom = W3B.selText('cmbUomDetail');
        base.VariantId = W3B.int($('cmbVariantDescription').value); base.VariantDescription = W3B.selText('cmbVariantDescription');
        base.CastingTypeId = W3B.int($('cmbCastingTypeDetail').value); base.CastingType = W3B.selText('cmbCastingTypeDetail');
        base.Qty = W3B.num($('txtQtyDetaill').value); base.WeightPerQty = W3B.num($('txtItemWeight').value); base.CastingWeight = W3B.num($('txtCastingWeight').value);
        return base;
    }
    function addOp() {                                         // BtnAddOp_Click:852
        if (!validateDetail()) return;
        rows.push(entryRow({ Id: 0, OrderId: 0, OrderNo: 0 }));
        detailReset(); render();
    }
    function updateOp() {                                      // BtnUpdateOp_Click:871 (OrderId / OrderNo come from the unbound order combo = 0)
        if (!validateDetail()) return;
        var r = rows[updateIdx]; if (!r) return;
        r.OrderId = 0; r.OrderNo = 0; entryRow(r);
        detailReset(); render();
    }
    function cancelOp() { detailReset(); render(); }           // BtnCancelOp_Click:901
    function detailReset() {                                   // FromResetDetail:1105
        updateIdx = -1;
        ['cmbItem', 'cmbUomDetail', 'cmbVariantDescription', 'cmbCastingTypeDetail'].forEach(function (i) { W3B.clearCombo(i); });
        ['txtItemWeight', 'txtCastingWeight', 'txtQtyDetaill'].forEach(function (i) { $(i).value = ''; });
        W3B.show('BtnAddOp', true); W3B.show('BtnCancelOp', false); W3B.show('BtnUpdateOp', false);
    }
    function editRow(i) {                                      // grdDetail_DoubleClick:913
        var r = rows[i]; if (!r) return;
        updateIdx = i;
        W3B.clearCombo('cmbCategory'); categoryLeave();
        $('cmbItem').value = String(r.ItemId);
        itemLeave().then(function () {
            W3B.setCombo('cmbUomDetail', r.UomId); W3B.setCombo('cmbVariantDescription', r.VariantId); W3B.setCombo('cmbCastingTypeDetail', r.CastingTypeId);
            fillWeight();
            $('txtQtyDetaill').value = W3B.fmt(r.Qty, 3);
            W3B.show('BtnAddOp', false); W3B.show('BtnUpdateOp', true); W3B.show('BtnCancelOp', true);
            render(); W3B.focus('cmbItem');
        });
    }
    function delRow(i) {                                       // DeleteOperationDetailRow:1023
        var r = rows[i]; if (!r) return;
        if (updateIdx !== -1) { W3B.box('Please Reset the Detail first..'); return; }
        if (r.Id !== 0) {
            if (!W3B.ask('Are you sure to Delete?')) return;
            removed.push({ Id: r.Id, OrderId: r.OrderId, ItemId: r.ItemId, UomId: r.UomId, VariantId: r.VariantId, CastingTypeId: r.CastingTypeId, Qty: r.Qty });
        }
        rows.splice(i, 1); render();
    }
    function render() {                                        // grdDetailSetting:943
        $('grdDetail').innerHTML = W3B.tableHtml([
            { h: 'X', html: true, cls: 'w3-btn', w: 25, f: function (r, i) { return '<button type="button" class="rc-x" data-del="' + i + '">X</button>'; } },
            { h: 'Edit', html: true, cls: 'w3-btn', w: 40, f: function (r, i) { return '<button type="button" class="rc-cellbtn" data-edit="' + i + '">Edit</button>'; } },
            { h: 'OrderNo', k: 'OrderNo', cls: 'num' }, { h: 'ItemCode', k: 'ItemCode' }, { h: 'ItemName', k: 'ItemName' }, { h: 'Uom', k: 'Uom' },
            { h: 'Modal Description', k: 'VariantDescription' }, { h: 'CastingType', k: 'CastingType' },
            { h: 'Qty', cls: 'num', f: function (r) { return W3B.fmt(r.Qty, 3); } },
            { h: 'WeightPerQty', cls: 'num', f: function (r) { return W3B.fmt(r.WeightPerQty, 3); } },
            { h: 'CastingWeight', cls: 'num', f: function (r) { return W3B.fmt(r.CastingWeight, 3); } }
        ], rows, { empty: 'No records', rowClass: function (r, i) { return i === updateIdx ? 'is-current' : ''; } });
    }
    document.addEventListener('click', function (e) {
        var t = e.target.closest ? e.target : null; if (!t) return;
        var d = t.closest('button[data-del]'); if (d) { delRow(+d.dataset.del); return; }
        var ed = t.closest('button[data-edit]'); if (ed) { editRow(+ed.dataset.edit); return; }
        var ph = t.closest('button[data-hprint]'); if (ph) { printId(hist[+ph.dataset.hprint].Id); return; }
        var eh = t.closest('button[data-hedit]'); if (eh) { histEdit(hist[+eh.dataset.hedit].Id); return; }
        var sa = t.closest('button[data-hsaveas]'); if (sa) { histSaveAs(hist[+sa.dataset.hsaveas].Id); return; }
        var tr = t.closest('#DatagridHistory tbody tr[data-i]');                               // DatagridHistory_SelectionChanged:1779
        if (tr) historyDetail(hist[+tr.dataset.i].Id);
    });
    document.addEventListener('dblclick', function (e) {
        var tr = e.target.closest ? e.target.closest('#grdDetail tbody tr[data-i]') : null;
        if (tr) { editRow(+tr.dataset.i); return; }
        var hr = e.target.closest ? e.target.closest('#DatagridHistory tbody tr[data-i]') : null;    // DatagridHistory_DoubleClick:1695
        if (hr) histEdit(hist[+hr.dataset.i].Id);
    });

    // ----------------------------------------------------------------------------- toolbar
    function resetForm() {                                     // FromReset:1074
        recId = 0; updateIdx = -1; removed = []; updateMode = false; saveAsMode = false;
        $('txtdocdate').value = W3B.today(); $('txtWorkOrderNo').value = ''; $('txtRemarksHeader').value = '';
        detailReset(); rows = []; render(); mode();
        return W3B.getJson(api + '/doc-no').then(function (d) { $('txtdocno').value = d.docNo; });
    }
    function newClick() { resetForm().catch(W3B.fail); }
    function refresh() {                                       // btnRefresh_Click:1129
        W3B.getJson(api + '/refresh').then(function (d) { look.categories = d.categories; look.modals = d.modals; look.items = d.items; look.castingTypes = d.castingTypes; bindAll(); }).catch(W3B.fail);
    }
    function validateHeader() {                                // FormValidation:794
        var dn = $('txtdocno').value.trim();
        if (dn === '' || dn === '0') { W3B.box('doc No Field Required'); return false; }
        var wo = $('txtWorkOrderNo').value.trim();
        if (wo === '' || wo === '0') { W3B.box('WorkOrder No Field Required'); W3B.focus('txtWorkOrderNo'); return false; }
        if ($('txtStartdate').value > $('txtEndDate').value) { W3B.box('Start date cannot be greater than End Date'); W3B.focus('txtStartdate'); return false; }
        if ($('txtDeadLineDateMain').value < $('txtEndDate').value) { W3B.box('Dead Line date cannot be less than End Date'); W3B.focus('txtStartdate'); return false; }
        return true;
    }
    function insert() {                                        // Insert:1218
        if (!validateHeader()) return;
        if (!rows.length) { W3B.box('Detail Grid Record Not Found...\nGrid Should have atleast one record'); return; }
        if (!W3B.ask(recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var b = { id: recId, docNo: $('txtdocno').value, docDate: $('txtdocdate').value, workOrderNo: $('txtWorkOrderNo').value,
            startDate: $('txtStartdate').value, endDate: $('txtEndDate').value, deadLine: $('txtDeadLineDateMain').value,
            batchDesc: $('txtBatchDesc').value, remarksHeader: $('txtRemarksHeader').value, rows: rows, removed: removed };
        W3B.postJson(api + '/save', b).then(function (d) {
            W3B.box(d.message);
            var preview = $('chkPreview').checked && rights.print;
            return resetForm().then(function () { if (preview) printId(d.id); });
        }).catch(W3B.fail);
    }
    function save() { recId = 0; insert(); }                   // btnsave_Click:1350
    function update() { if (updateMode) insert(); }            // btnUpdate_Click:1363
    function saveAs() { recId = 0; insert(); }                 // btnSaveAs_Click:1452
    function del() {                                           // btnDelete_Click:1428
        if (recId <= 0) { W3B.box('No record found to Delete'); return; }
        if (!W3B.ask('Are you sure to Delete?')) return;
        W3B.postForm(api + '/delete?id=' + recId).then(function (d) { W3B.box(d.message); newClick(); }).catch(W3B.fail);
    }
    function printId(id) {                                     // WorkOrderSlip:1525 -> CommonServices.WorkOrderSemiFinish_Slip1621
        if (!(id > 0)) { W3B.box('No Record Found For Display'); return; }
        window.open('/reports/print/1621-work-order-semi-finish-slip?id=' + id, '_blank');
    }
    function print() { printId(recId); }                       // btnPrint_Click:1513
    function keys() {                                          // MakeShortCutKeys:2019
        W3B.box(['Ctrl+S : For Save When on Entry form and For Show Data when on History Form', 'Ctrl+U : For Update', 'Ctrl+Shift+Delete : For Delete',
            'Ctrl+E : For Close', 'Ctrl+R : For Refresh', 'Ctrl+N : For New', 'Ctrl+P : For Print', 'Ctrl+F12 : For SaveAs', 'Ctrl+T : For Tab Transfer'].join('\n'));
    }

    function load(id) {                                        // ReadById:1375
        return W3B.getJson(api + '/read/' + id).then(function (d) {
            recId = d.id; removed = []; updateIdx = -1; updateMode = true; saveAsMode = false;
            $('txtdocdate').value = d.docDate; $('txtdocno').value = d.docNo; $('txtWorkOrderNo').value = d.workOrderNo;
            $('txtStartdate').value = d.startDate; $('txtEndDate').value = d.endDate; $('txtDeadLineDateMain').value = d.deadLine;
            $('txtRemarksHeader').value = d.remarksHeader; $('txtBatchDesc').value = d.batchDesc;
            rows = d.details || []; detailReset(); render(); mode(); tab('form');
        }).catch(W3B.fail);
    }

    // ----------------------------------------------------------------------------- history
    function tab(t) {
        $('panelForm').classList.toggle('is-active', t === 'form'); $('panelHist').classList.toggle('is-active', t === 'hist');
        $('tabForm').classList.toggle('is-active', t === 'form'); $('tabHist').classList.toggle('is-active', t === 'hist');
    }
    function history() {                                       // HistoryGridFill:1561
        $('grdhistoryDetail').innerHTML = '';
        W3B.getJson(api + '/history' + W3B.qs({ fromDate: $('chkFrom').checked ? $('txtFromdateHistory').value : '', toDate: $('chkTo').checked ? $('txtToDateHistory').value : '',
            fromDocNo: W3B.int($('txtFromNoHistory').value), toDocNo: W3B.int($('txtToDocNoHistory').value) })).then(function (r) {
            hist = r;
            $('DatagridHistory').innerHTML = W3B.tableHtml([
                { h: 'Print', html: true, cls: 'w3-btn', w: 50, f: function (x, i) { return '<button type="button" class="rc-cellbtn" data-hprint="' + i + '">Print</button>'; } },
                { h: 'Edit', html: true, cls: 'w3-btn', w: 40, f: function (x, i) { return '<button type="button" class="rc-cellbtn" data-hedit="' + i + '">Edit</button>'; } },
                { h: 'SaveAs', html: true, cls: 'w3-btn', w: 60, f: function (x, i) { return '<button type="button" class="rc-cellbtn" data-hsaveas="' + i + '">SaveAs</button>'; } },
                { h: 'DocNo', k: 'DocNo' }, { h: 'DocDate', f: function (x) { return W3B.fmtDMY(x.DocDate); } }, { h: 'ManualDocCode', k: 'ManualDocCode' },
                { h: 'StartDate', f: function (x) { return W3B.fmtDMY(x.StartDate); } }, { h: 'EndDate', f: function (x) { return W3B.fmtDMY(x.EndDate); } },
                { h: 'DeadLine', f: function (x) { return W3B.fmtDMY(x.DeadLine); } }, { h: 'BatchDesc', k: 'BatchDesc' }, { h: 'RemarksHeader', k: 'RemarksHeader' },
                { h: 'EntryUserName', k: 'EntryUserName' }, { h: 'EntryDate', f: function (x) { return W3B.fmtStamp(x.EntryDate); } },
                { h: 'ModifyUserName', k: 'ModifyUserName' }, { h: 'ModifyDate', f: function (x) { return W3B.fmtStamp(x.ModifyDate); } },
                { h: 'IsApproved', bool: true, k: 'IsApproved', cls: 'ctr' }, { h: 'NoOfAttachments', k: 'NoOfAttachments', cls: 'num' }
            ], r, { empty: 'No records' });
        }).catch(W3B.fail);
    }
    function historyDetail(id) {                               // DatagridHistoryDetailbind:1796
        W3B.getJson(api + '/history-detail/' + id).then(function (d) {
            $('grdhistoryDetail').innerHTML = W3B.tableHtml([
                { h: 'OrderNo', k: 'OrderNo', cls: 'num' }, { h: 'ItemName', k: 'ItemName' }, { h: 'Uom', k: 'Uom' }, { h: 'VariantDescription', k: 'VariantDescription' },
                { h: 'CastingType', k: 'CastingType' }, { h: 'Qty', cls: 'num', f: function (r) { return W3B.fmt(r.Qty, 3); } },
                { h: 'WeightPerQty', cls: 'num', f: function (r) { return W3B.fmt(r.WeightPerQty, 3); } },
                { h: 'CastingWeight', cls: 'num', f: function (r) { return W3B.fmt(r.CastingWeight, 3); } }
            ], d, { empty: 'No records' });
        }).catch(W3B.fail);
    }
    function histEdit(id) {                                    // :1695 / :1712
        if (!rights.update) { W3B.box("you don't have update rights..."); return; }
        load(id);
    }
    function histSaveAs(id) {                                  // DataGridHistory_SaveAs:1762
        load(id).then(function () { updateMode = false; saveAsMode = true; $('txtdocdate').value = W3B.today(); mode(); });
    }

    window.Sf = { newClick: newClick, refresh: refresh, save: save, saveAs: saveAs, update: update, del: del, print: print, keys: keys, tab: tab,
        categoryLeave: categoryLeave, modalLeave: modalLeave, itemLeave: itemLeave, itemBy: itemBy, castingLeave: fillWeight, qtyChanged: qtyChanged,
        addOp: addOp, updateOp: updateOp, cancelOp: cancelOp, history: history };

    function on(id) { var b = $(id); return !b.classList.contains('is-hidden') && !b.disabled; }
    W3B.keys({
        n: function () { if ($('panelForm').classList.contains('is-active')) newClick(); else $('grdhistoryDetail').innerHTML = ''; },
        s: function () { if ($('panelForm').classList.contains('is-active')) { if (on('btnSave')) save(); } else history(); },
        u: function () { if (on('btnUpdate')) update(); },
        p: function () { if (on('btnPrint')) print(); },
        r: refresh, f12: function () { if (on('btnSaveAs')) saveAs(); },
        'shift+delete': function () { if (on('btnDelete')) del(); },
        t: function () { tab($('panelForm').classList.contains('is-active') ? 'hist' : 'form'); },
        e: function () { window.location.href = '/production'; }, escape: function () { window.location.href = '/production'; }
    });

    W3B.getJson(api + '/lookups').then(function (d) {
        rights = d.rights || {}; applyRights();
        look.categories = d.categories; look.modals = d.modals; look.items = d.items; look.castingTypes = d.castingTypes;
        bindAll();
        var t = W3B.today();
        ['txtdocdate', 'txtStartdate', 'txtEndDate', 'txtDeadLineDateMain', 'txtFromdateHistory', 'txtToDateHistory'].forEach(function (i) { $(i).value = t; });
        $('chkFrom').checked = true; $('chkTo').checked = true;
        $('txtdocno').value = d.docNo; render(); mode();
    }).catch(W3B.fail);
}());
