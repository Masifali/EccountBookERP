/* ============================================================================================
 * Screen 338 "Department Request" — Architecture.WinApp.StoreManagement.frmDepartmentRequest,
 * ScreenName "frmDepartmentRequest", DocumentTypeId 450.
 *
 *   InitializeComponentMethod:300  rights, Doc No, items / conditions / departments / assets,
 *                                  history dates — first load only
 *   cmbItem_Leave:1496             ItemUOMFill (keeps the previous UOM by TEXT) + BalanceStock
 *   CmbItemCondition_Leave:1907    BalanceStock
 *   Add_Click:1302 / btnUpdateDetail_Click:1391 / grdDetail_DoubleClick:1456 / DeleteDetailRow:1259
 *   Insert():711                   save and update; Reset():614 afterwards; slip when Preview is ticked
 *   HistoryGridFill:917            history; DataGridHistory_SelectionChanged:1143 its detail
 * ============================================================================================ */
(function () {
    'use strict';
    var C = window.StoreCommon, $id = C.$id, esc = C.esc, num = C.num, intOf = C.intOf;
    var api = '/api/store/department-request';

    var look = { rights: {}, items: [], uoms: [], itemConditions: [], departments: [], assets: [] };
    var recId = 0;
    var isApproved = false;
    var table = [];                 // dtdetail
    var removed = [];               // lstRemoveRecord
    var updateDetailIndex = -1;
    var stockSeq = 0;

    function sel(id) { var e = $id(id); return e.selectedOptions && e.selectedOptions[0] && intOf(e.value) !== 0 ? e.selectedOptions[0].textContent.trim() : ''; }
    function setVal(id, v) {                                                    // UltraCombo.Value = v: no match → nothing selected
        var e = $id(id), s = String(intOf(v));
        e.value = s;
        if (e.value !== s) e.value = '0';
    }
    function nowIso() { return C.today(); }
    function fmtQty(v) { return Number(num(v)).toLocaleString('en-US', { maximumFractionDigits: 2 }); }
    function rt() { return look.rights || {}; }

    /* ------------------------------------------------------------------ combos */

    function itemNameBind() {                                                   // ItemNameBind:438 — retains the Id
        var keep = intOf($id('cmbItem').value);
        C.fillSelect('cmbItem', look.items, 'Id', $id('rdbtnItemName').checked ? 'ItemName' : 'ItemCode');
        setVal('cmbItem', keep);
    }
    function selectedItem() { var id = intOf($id('cmbItem').value); return look.items.find(function (x) { return x.Id === id; }) || null; }
    function departmentFill() {                                                 // DeprtmentFill:384
        if (!look.departments.length) return;
        ['cmbDepartmentFrom', 'cmbDepartmentTo'].forEach(function (id) {
            var keep = intOf($id(id).value);
            C.fillSelect(id, look.departments, 'Id', 'DepartmentName');
            setVal(id, keep);
        });
    }
    function assetFill() {                                                      // FixedAssest:518
        if (!look.assets.length) return;
        var keep = intOf($id('CmbAssetName').value);
        C.fillSelect('CmbAssetName', look.assets, 'Id', 'AssetName');
        setVal('CmbAssetName', keep);
    }
    function conditionBind() {                                                  // ItemConditionBindFromGlobal:534 (Id 4 dropped server side)
        var keep = intOf($id('CmbItemCondition').value);
        C.fillSelect('CmbItemCondition', look.itemConditions, 'Id', 'Description');
        setVal('CmbItemCondition', keep);
    }

    /** ItemUOMFill:458 — retain by the previous TEXT, else the only UOM, else the Equivalent = 1 one. */
    function itemUomFill() {
        var prevText = sel('cmbItemUOM');
        var itemId = intOf($id('cmbItem').value);
        var list = look.uoms.filter(function (u) { return u.ItemId === itemId; });
        C.fillSelect('cmbItemUOM', list, 'Id', 'UOMCode');
        var el = $id('cmbItemUOM');
        el.value = '0';
        if (prevText) {
            var m = list.find(function (u) { return u.UOMCode === prevText; });
            if (m) el.value = String(m.Id);
        }
        if (intOf(el.value) !== 0) return;
        if (list.length === 1) el.value = String(list[0].Id);
        else if (list.length > 1) {
            var eq = list.find(function (u) { return num(u.Equivalent) === 1; });
            if (eq) el.value = String(eq.Id);
        }
    }

    /** BalanceStock:1509 — the label goes to "0" first and only becomes visible when a row comes back. */
    function balanceStock() {
        var lbl = $id('lblStockQty');
        lbl.textContent = '0';
        var itemId = intOf($id('cmbItem').value);
        if (itemId === 0) return Promise.resolve();
        var seq = ++stockSeq;
        return C.getJson(api + '/stock' + C.qs({ itemId: itemId, itemConditionId: intOf($id('CmbItemCondition').value), docDate: $id('DocDate').value }))
            .then(function (r) {
                if (seq !== stockSeq) return;
                if (r && r.found) { lbl.textContent = String(r.qtyInHand); lbl.classList.remove('is-hidden'); }
            }).catch(function (e) { alert(e.message); });
    }

    function itemLeave() { itemUomFill(); return balanceStock(); }                // cmbItem_Leave:1496

    /* rdbtnItemName_CheckedChanged:549 — rebinds (keeping the item) when items exist; no Leave. */
    function searchModeChanged() { if (look.items.length) { itemNameBind(); $id('cmbItem').focus(); } }

    function barcode() {                                                        // txtBarcodeReader_KeyDown:1571
        var code = $id('txtBarcodeReader').value.trim();
        C.getJson(api + '/barcode' + C.qs({ barcode: code })).then(function (r) {
            setVal('cmbItem', r.itemId);
            itemLeave();
            $id('txtBarcodeReader').value = '';
            $id('cmbItemUOM').focus();
        }).catch(function (e) { alert(e.message); });
    }

    /* CommonServices.OnlytextdecimelFunction / OnlytextNumberFunction */
    function decimalKey(e) {
        var ch = e.key;
        if (ch.length !== 1) return true;
        if (/[0-9]/.test(ch)) return true;
        if (ch === '.' && e.target.value.indexOf('.') < 0) return true;
        e.preventDefault(); return false;
    }
    function numberKey(e) {
        var ch = e.key;
        if (ch.length !== 1 || /[0-9]/.test(ch)) return true;
        e.preventDefault(); return false;
    }

    /* ------------------------------------------------------------------ entry bar */

    /** The DataTable column is typed double: text that is not a number throws on Rows.Add. */
    function qtyValue() {
        var t = $id('txtRequestedQty').value.trim();
        var v = Number(t);
        if (t === '' || isNaN(v)) throw new Error("Input string was not in a correct format.Couldn't store <" + t + "> in RequestedQty Column.  Expected type is Double.");
        return v;
    }

    function add() {                                                            // Add_Click:1302
        try {
            if (intOf($id('cmbItem').value) === 0) { alert('Item Field is Required'); $id('cmbItem').focus(); return; }
            if (intOf($id('cmbItemUOM').value) === 0) { alert('UOM Field is Required'); $id('cmbItemUOM').focus(); return; }
            if ($id('txtRequestedQty').value.trim() === '') { alert('RequestedQty Field is Required'); $id('txtRequestedQty').focus(); return; }
            if (intOf($id('CmbAssetName').value) === 0) { alert('AssetName Field is Required'); $id('CmbAssetName').focus(); return; }
            if (intOf($id('CmbItemCondition').value) === 0) { alert('Item Condition Field is Required'); $id('CmbItemCondition').focus(); return; }
            var itemId = intOf($id('cmbItem').value);
            if (table.some(function (r) { return intOf(r.ItemId) === itemId; })) { alert('Duplicate Item Not Add in Grid'); return; }
            var it = selectedItem() || {};
            table.push({
                Id: 0, ItemId: itemId, Item: it.ItemName || '', ItemCode: it.ItemCode || '',
                ItemConditionId: intOf($id('CmbItemCondition').value), ItemCondition: sel('CmbItemCondition'),
                ItemUOMId: intOf($id('cmbItemUOM').value), UOM: sel('cmbItemUOM'),
                AssetId: intOf($id('CmbAssetName').value), AssetName: sel('CmbAssetName'),
                RequestedQty: qtyValue()
            });
            render();
            resetDetail();
        } catch (e) { alert(e.message); }
    }

    function updateDetail() {                                                   // btnUpdateDetail_Click:1391
        try {
            if (intOf($id('cmbItem').value) === 0) { alert('Item Field is Required'); $id('cmbItem').focus(); return; }
            if (intOf($id('cmbItemUOM').value) === 0) { alert('UOM Field is Required'); $id('cmbItemUOM').focus(); return; }
            if ($id('txtRequestedQty').value.trim() === '') { alert('RequestedQty Field is Required'); $id('txtRequestedQty').focus(); return; }
            if (intOf($id('CmbAssetName').value) === 0) { alert('AssetName Field is Required'); $id('CmbAssetName').focus(); return; }
            if (intOf($id('CmbItemCondition').value) === 0) { alert('Item Condition Field is Required'); $id('CmbItemCondition').focus(); return; }
            var itemId = intOf($id('cmbItem').value);
            if (table.some(function (r, i) { return intOf(r.ItemId) === itemId && i !== updateDetailIndex; })) { alert('Duplicate Item Not Add in Grid'); return; }
            var r = table[updateDetailIndex];
            if (!r) throw new Error('There is no row at position ' + updateDetailIndex + '.');
            var qty = qtyValue(), it = selectedItem() || {};
            r.ItemId = itemId; r.Item = it.ItemName || ''; r.ItemCode = it.ItemCode || '';
            r.ItemConditionId = intOf($id('CmbItemCondition').value); r.ItemCondition = sel('CmbItemCondition');
            r.ItemUOMId = intOf($id('cmbItemUOM').value); r.UOM = sel('cmbItemUOM');
            r.AssetId = intOf($id('CmbAssetName').value); r.AssetName = sel('CmbAssetName');
            r.RequestedQty = qty;
            render();
            resetDetail();
        } catch (e) { alert(e.message); }
    }

    function edit(i) {                                                          // grdDetail_DoubleClick:1456 — no BalanceStock
        var r = table[i];
        if (!r) return;
        updateDetailIndex = i;
        setVal('cmbItem', r.ItemId);
        itemUomFill();
        var u = look.uoms.find(function (x) { return x.ItemId === intOf(r.ItemId) && x.UOMCode === r.UOM; });  // Text = row UOM
        $id('cmbItemUOM').value = u ? String(u.Id) : '0';
        setVal('CmbItemCondition', r.ItemConditionId);
        var a = look.assets.find(function (x) { return x.AssetName === r.AssetName; });                          // Text = row AssetName
        $id('CmbAssetName').value = a ? String(a.Id) : '0';
        $id('txtRequestedQty').value = String(r.RequestedQty);
        $id('cmbItem').focus();
        $id('Add').classList.add('is-hidden');
        $id('btnUpdateDetail').classList.remove('is-hidden');
        $id('btnCancelUpdateDetial').classList.remove('is-hidden');
    }

    function deleteRow(i) {                                                     // DeleteDetailRow:1259
        var r = table[i];
        if (!r) return;
        if (updateDetailIndex !== -1) { alert('Please Reset the Detail first...'); return; }
        if (intOf(r.Id) > 0) {
            if (!confirm('Are you sure to Delete?')) return;
            removed.push({ Id: intOf(r.Id), ItemId: intOf(r.ItemId), ItemUOMId: intOf(r.ItemUOMId), AssetId: intOf(r.AssetId),
                RequestedQty: num(r.RequestedQty), ItemConditionId: 0 });          // ItemConditionId never set (:1272)
        }
        table.splice(i, 1);
        render();
    }

    function resetDetail() {                                                    // ResetDetail:654 — Item Condition kept
        updateDetailIndex = -1;
        $id('cmbItem').value = '0';
        $id('cmbItemUOM').value = '0';
        $id('txtRequestedQty').value = '';
        $id('CmbAssetName').value = '0';
        $id('cmbItem').focus();
        $id('Add').classList.remove('is-hidden');
        $id('btnUpdateDetail').classList.add('is-hidden');
        $id('btnCancelUpdateDetial').classList.add('is-hidden');
    }

    /* ------------------------------------------------------------------ grid */

    function render() {                                                         // grdSettings:1358
        var t = $id('grdDetail');
        var cols = [['Item', 'Item'], ['ItemCode', 'Item Code'], ['ItemCondition', 'Item Condition'], ['UOM', 'UOM'],
            ['AssetName', 'Asset Name'], ['RequestedQty', 'Requested Qty', 'n']];
        t.tHead.innerHTML = '<tr><th>X</th><th>Edit</th>' + cols.map(function (c) { return '<th>' + c[1] + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = table.map(function (r, i) {
            return '<tr ondblclick="DeptRequest.edit(' + i + ')"><td><button type="button" class="cx-x" onclick="DeptRequest.deleteRow(' + i + ')">X</button></td>' +
                '<td><button type="button" class="cx-link" onclick="DeptRequest.edit(' + i + ')">Edit</button></td>' +
                cols.map(function (c) {
                    var v = r[c[0]];
                    return c[2] === 'n' ? '<td class="num">' + esc(fmtQty(v)) + '</td>' : '<td>' + esc(v) + '</td>';
                }).join('') + '</tr>';
        }).join('');
    }

    /* ------------------------------------------------------------------ save / open / delete */

    function save() { insert(); }                                               // btnSave_Click:822
    function update() {                                                         // btnUpdate_Click:834
        if (recId === 0) { alert('RecId Not Found'); return; }
        insert();
    }
    function insert() {                                                         // Insert():711
        if (!table.length) { alert('Grid record not found'); return; }
        var d = $id('txtdocno').value.trim();
        if (d === '' || intOf(d) === 0) { alert('DocNo Field is Required'); return; }
        if (intOf($id('cmbDepartmentFrom').value) === 0) { alert('Department From Field is Required'); $id('cmbDepartmentFrom').focus(); return; }
        if (intOf($id('cmbDepartmentTo').value) === 0) { alert('Department To Field is Required'); $id('cmbDepartmentTo').focus(); return; }
        if (recId > 0) {
            if (isApproved) { alert('Record not Update because record has approved'); return; }
            if (!confirm('Are you sure to Update?')) return;
        } else if (!confirm('Are you sure to Save?')) return;
        var print = $id('ChkBok').checked;
        C.postJson(api + '/save', {
            Id: recId, DocDate: $id('DocDate').value,
            FromDepartmentId: intOf($id('cmbDepartmentFrom').value), ToDepartmentId: intOf($id('cmbDepartmentTo').value),
            RemarksHeader: $id('txtremarks').value,
            rows: table.map(function (r) {
                return { Id: intOf(r.Id), ItemId: intOf(r.ItemId), ItemConditionId: intOf(r.ItemConditionId), ItemUOMId: intOf(r.ItemUOMId),
                    AssetId: intOf(r.AssetId), RequestedQty: num(r.RequestedQty) };
            }),
            removed: recId > 0 ? removed : []
        }).then(function (res) {
            alert(res.message);
            var id = res.id;
            return reset().then(function () { if (print) printId(id); });
        }).catch(function (e) { alert(e.message); });
    }

    function del() {                                                            // btnDelete_Click:893
        if (recId <= 0) { alert('No Record Found For Deletion'); return; }
        if (!confirm('Are you sure to Delete?')) return;
        C.postJson(api + '/' + recId + '/delete', {}).then(function (res) {
            alert(res.message);
            return reset();
        }).catch(function (e) { alert(e.message); });
    }

    function printId(id) { C.printSlip(api + '/' + intOf(id) + '/slip', 'Department Request Slip'); }

    function readById(id) {                                                     // ReadById:850 — the entry bar is left as it is
        return C.getJson(api + '/' + id).then(function (h) {
            recId = h.Id;
            removed = [];                                                       // deviation 4 (lstRemoveRecord is never cleared on the desktop)
            $id('btnSave').classList.add('is-hidden');
            $id('btnUpdate').classList.remove('is-hidden');
            $id('btnDelete').classList.remove('is-hidden');
            tab('tabForm');
            $id('txtdocno').value = h.DocNo;
            $id('DocDate').value = C.isoDay(h.DocDate);
            setVal('cmbDepartmentFrom', h.FromDepartmentId);
            setVal('cmbDepartmentTo', h.ToDepartmentId);
            $id('txtremarks').value = h.RemarksHeader || '';
            isApproved = !!h.IsApproved;
            table = h.rows || [];
            render();
            $id('lblRecId').textContent = 'Record #' + recId + (isApproved ? ' (Approved)' : '');
        }).catch(function (e) { alert(e.message); });
    }

    /* ------------------------------------------------------------------ history */

    function dateField() {
        if ($id('drdocdate').checked) return 'doc';
        if ($id('rdentrydate').checked) return 'entry';
        if ($id('rdmodifydate').checked) return 'modify';
        if ($id('rdapproveddate').checked) return 'approved';
        return 'none';
    }
    function showHistory() {                                                    // HistoryGridFill:917
        C.getJson(api + '/history' + C.qs({
            dateField: dateField(),
            fromDate: $id('chkFromDateHistory').checked ? $id('FromDateHistory').value : '',
            toDate: $id('chkToDateHistory').checked ? $id('ToDateHistory').value : '',
            docNoFrom: intOf($id('txtFromDocNoHistory').value), docNoTo: intOf($id('txtToDocNoHistory').value)
        })).then(function (list) {
            if (!list || !list.length) return;                                  // :977 — the grid keeps what it showed
            var r = rt(), t = $id('DataGridHistory');
            t.tHead.innerHTML = '<tr>' + (r.print ? '<th>Print</th>' : '') + (r.update ? '<th>Edit</th>' : '') +
                '<th>Doc Date</th><th>Doc No</th><th>Department Name From</th><th>To Department Name</th><th>Remarks Header</th>' +
                '<th>Entry User</th><th>Entry Date</th><th>Modify User</th><th>Modify Date</th><th>No Of Attachments</th></tr>';
            t.tBodies[0].innerHTML = list.map(function (h) {
                return '<tr data-id="' + h.Id + '" onclick="DeptRequest.historyDetail(this)" ondblclick="DeptRequest.historyOpen(' + h.Id + ')">' +
                    (r.print ? '<td><button type="button" class="cx-link" onclick="event.stopPropagation();DeptRequest.printId(' + h.Id + ')">Print</button></td>' : '') +
                    (r.update ? '<td><button type="button" class="cx-link" onclick="event.stopPropagation();DeptRequest.historyOpen(' + h.Id + ')">Edit</button></td>' : '') +
                    '<td>' + esc(C.gridDate(h.DocDate)) + '</td><td>' + esc(h.DocNo) + '</td><td>' + esc(h.DepartmentNameFrom) + '</td><td>' +
                    esc(h.ToDepartmentName) + '</td><td>' + esc(h.RemarksHeader) + '</td><td>' + esc(h.EntryUser) + '</td><td>' +
                    esc(C.gridDateTime(h.EntryDate)) + '</td><td>' + esc(h.ModifyUser) + '</td><td>' + esc(C.gridDateTime(h.ModifyDate)) +
                    '</td><td class="num">' + esc(h.NoOfAttachments) + '</td></tr>';
            }).join('');
        }).catch(function (e) { alert(e.message); });
    }
    function historyOpen(id) {                                                  // DataGridHistory_DoubleClick:1088 / Edit column :1108
        if (!rt().update) return;
        readById(id);
    }
    function historyDetail(tr) {                                                // DataGridHistory_SelectionChanged:1143
        document.querySelectorAll('#DataGridHistory tbody tr').forEach(function (x) { x.classList.remove('is-selected'); });
        tr.classList.add('is-selected');
        C.getJson(api + '/' + tr.getAttribute('data-id')).then(function (h) {
            var cols = [['Item', 'Item Name'], ['ItemCode', 'Item Code'], ['UOM', 'UOM'], ['ItemCondition', 'Item Condition'],
                ['AssetName', 'Asset Name'], ['RequestedQty', 'Requested Qty']];
            var t = $id('DataGridHistoryDetail'), total = 0;
            t.tHead.innerHTML = '<tr>' + cols.map(function (c) { return '<th>' + c[1] + '</th>'; }).join('') + '</tr>';
            t.tBodies[0].innerHTML = (h.rows || []).map(function (r) {
                total += num(r.RequestedQty);
                return '<tr>' + cols.map(function (c) {
                    return c[0] === 'RequestedQty' ? '<td class="num">' + esc(fmtQty(r[c[0]])) + '</td>' : '<td>' + esc(r[c[0]]) + '</td>';
                }).join('') + '</tr>';
            }).join('');
            t.tFoot.innerHTML = '<tr><td colspan="5"></td><td class="num"><b>' + esc(fmtQty(total)) + '</b></td></tr>';  // AggregateFunction Sum
        }).catch(function (e) { alert(e.message); });
    }
    function newHistory() {                                                     // BtnNewHistory_Click:1205
        $id('FromDateHistory').value = nowIso(); $id('ToDateHistory').value = nowIso();
        $id('txtFromDocNoHistory').value = ''; $id('txtToDocNoHistory').value = '';
        ['DataGridHistory', 'DataGridHistoryDetail'].forEach(function (id) {
            var t = $id(id); t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; if (t.tFoot) t.tFoot.innerHTML = '';
        });
        $id('drdocdate').checked = true;
    }

    function tab(id) {
        document.querySelectorAll('.win-tab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-tab') === id); });
        document.querySelectorAll('.win-tab-panel').forEach(function (p) { p.classList.toggle('is-active', p.id === id); });
    }

    /* ------------------------------------------------------------------ load / reset / refresh */

    function applyLists(l) {
        look.items = l.items || []; look.uoms = l.uoms || []; look.itemConditions = l.itemConditions || [];
        look.departments = l.departments || []; look.assets = l.assets || [];
        itemNameBind(); conditionBind(); departmentFill(); assetFill();
    }

    function init() {                                                           // InitializeComponentMethod:300
        return C.getJson(api + '/lookups').then(function (l) {
            look.rights = l.rights || {};
            var r = look.rights;
            $id('btnSave').disabled = !r.save; $id('btnUpdate').disabled = !r.update;
            $id('btnDelete').disabled = !r.delete; $id('btnPrint451').disabled = !r.print;
            $id('ChkBok').disabled = !r.print; $id('ChkBok').checked = !!r.print;
            if (!r.view) { $id('rightsNote').textContent = 'You do not have the View right for Department Request.'; $id('rightsNote').classList.remove('is-hidden'); }
            if (recId === 0) $id('txtdocno').value = l.docNo || '';
            if (l.itemSearchByCode) $id('rdbtnItemCode').checked = true; else $id('rdbtnItemName').checked = true;
            applyLists(l);
            render();
            var days = intOf(l.defaultDaysToLessFromHistoryFromDate) > 0 ? intOf(l.defaultDaysToLessFromHistoryFromDate) : 3;
            var f = new Date(); f.setDate(f.getDate() - days);
            $id('FromDateHistory').value = f.getFullYear() + '-' + String(f.getMonth() + 1).padStart(2, '0') + '-' + String(f.getDate()).padStart(2, '0');
            $id('ToDateHistory').value = nowIso();
            $id('DocDate').focus();
        }).catch(function (e) { alert(e.message); });
    }

    function reset() {                                                          // Reset():614
        recId = 0; isApproved = false;
        $id('cmbDepartmentFrom').value = '0'; $id('cmbDepartmentTo').value = '0';
        $id('txtremarks').value = ''; $id('txtdocno').value = '';
        $id('cmbItem').value = '0'; $id('cmbItemUOM').value = '0'; $id('txtRequestedQty').value = ''; $id('CmbAssetName').value = '0';
        $id('DocDate').value = nowIso();                                        // :631 DateTime.Now
        table = []; render();
        $id('btnSave').classList.remove('is-hidden'); $id('btnUpdate').classList.add('is-hidden'); $id('btnDelete').classList.add('is-hidden');
        updateDetailIndex = -1;
        $id('Add').classList.remove('is-hidden'); $id('btnUpdateDetail').classList.add('is-hidden'); $id('btnCancelUpdateDetial').classList.add('is-hidden');
        removed = [];
        $id('lblRecId').textContent = '';
        return C.getJson(api + '/doc-no').then(function (r) {                  // :640 DocumentNoDbCall
            if (recId === 0) $id('txtdocno').value = r.docNo;
            $id('DocDate').focus();
        }).catch(function (e) { alert(e.message); });
    }

    function refresh() {                                                        // btnRefresh_Click:679 — lists only
        return C.getJson(api + '/lookups').then(applyLists).catch(function (e) { alert(e.message); });
    }

    /* button2_Click:1547 — new Define_Department(UserAccount).Show(). The desktop picks new rows up only
       on btnRefresh_Click:679 (DepartmentDbCall + DeprtmentFill → both Department From / To combos).
       Web deviation: when the dialog closes, only the department lists are re-filled from this page's own
       /lookups (Sp_Department_GetAllMethod ReadAll), keeping both selections (DeprtmentFill:384). */
    function defineDepartment() {
        window.StoreDefine.openDepartment(function () {
            C.getJson(api + '/lookups').then(function (l) {
                look.departments = l.departments || [];
                departmentFill();
            }).catch(function (e) { alert(e.message); });
        }, { host: 'frmDepartmentRequest' });
    }

    window.DeptRequest = {
        reset: reset, save: save, update: update, del: del, refresh: refresh, tab: tab,
        defineDepartment: defineDepartment,
        print: function () { printId(recId); }, printId: printId,                 // printToolStripButton_Click:1559
        searchModeChanged: searchModeChanged, itemLeave: itemLeave, balanceStock: balanceStock, barcode: barcode,
        decimalKey: decimalKey, numberKey: numberKey,
        add: add, updateDetail: updateDetail, resetDetail: resetDetail, edit: edit, deleteRow: deleteRow,
        showHistory: showHistory, historyDetail: historyDetail, historyOpen: historyOpen, newHistory: newHistory
    };

    document.addEventListener('DOMContentLoaded', function () {
        $id('DocDate').value = nowIso();                                        // designer: DateTime.Now
        C.fillSelect('cmbItemUOM', [], 'Id', 'UOMCode');
        init();
    });
})();
