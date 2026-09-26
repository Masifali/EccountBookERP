/* ============================================================================================
 * Screen 349 "Department Request To Consumable Store" —
 * Architecture.WinApp.StoreManagement.DepartmentRequestToConsumableStore,
 * ScreenName "DepartmentRequestToConsumableStore", DocumentTypeId 1615.
 *
 *   EccountBook_Load:225       rights, ItemSearchByCode, GenerateCode, the five combo fills
 *   cmbItem_Leave:1346         ItemUOMFill (retains the previous UOM by Id) + BalanceStock
 *   Add_Click:1213 / btnUpdateDetail_Click:1275 / grdDetail_DoubleClick:1310 / ColumnButtonClick:1164
 *   Insert():700               save and update; Reset():613 afterwards; slip when Preview is ticked
 *   HistoryGridFill:928        history (tab / Load All); DataGridHistory_SelectionChanged:1112 its detail
 * ============================================================================================ */
(function () {
    'use strict';
    var C = window.StoreCommon, $id = C.$id, esc = C.esc, num = C.num, intOf = C.intOf;
    var api = '/api/store/department-request-to-consumable';
    var SELECT_ANY = '...Select Any Value...';     // DropDownBind.BindDDL / BindDDLNew default row
    var SELECT_DEPT = '-- Select --';              // InfragisticsHelper.InsertDefaultRow

    var look = { rights: {}, items: [], assets: [], departments: [], workOrders: [], projects: [] };
    var recId = 0;
    var isApproved = false;
    var table = [];                 // dtdetail
    var removed = [];               // lstRemoveRecord
    var updateDetailIndex = 0;      // int field, 0 until an edit
    var uomSeq = 0, stockSeq = 0, histSeq = 0;

    function rt() { return look.rights || {}; }
    function selText(id) { var e = $id(id); return e.selectedIndex >= 0 && e.options[e.selectedIndex] ? e.options[e.selectedIndex].textContent : ''; }
    /** UltraCombo.Text = "" — nothing selected (Value null → Conversion.ToInt = 0). */
    function clearCombo(id) { $id(id).selectedIndex = -1; }
    /** UltraCombo.Value = v — no matching row → nothing selected. */
    function setVal(id, v) {
        var e = $id(id), s = String(intOf(v));
        e.value = s;
        if (e.value !== s) e.selectedIndex = -1;
    }
    /** UltraCombo.Text = t with LimitToList — the first row showing that text, else nothing. */
    function setText(id, t) {
        var e = $id(id), opts = Array.prototype.slice.call(e.options);
        var o = opts.find(function (x) { return x.textContent === String(t); });
        if (o) e.value = o.value; else e.selectedIndex = -1;
    }
    function bind(id, rows, valueKey, textKey, defaultText) {
        var h = defaultText === null ? '' : '<option value="0">' + esc(defaultText) + '</option>';
        (rows || []).forEach(function (r) { h += '<option value="' + esc(r[valueKey]) + '">' + esc(r[textKey]) + '</option>'; });
        $id(id).innerHTML = h;
    }
    function fmt2(v) { return Number(num(v)).toLocaleString('en-US', { maximumFractionDigits: 2 }); }   // "#,##0.##"
    function dateText(v) { return C.gridDate(C.isoDay(v)); }

    /* ------------------------------------------------------------------ combos */

    /** DeprtmentFill:318 — BindAndRetainSelection, "-- Select --"; no rows → cleared. */
    function departmentFill() {
        ['cmbDepartmentFrom', 'cmbDepartmentTo'].forEach(function (id) {
            var prev = intOf($id(id).value);
            if (!look.departments.length) { $id(id).innerHTML = ''; return; }
            bind(id, look.departments, 'Id', 'Description', SELECT_DEPT);
            var found = look.departments.some(function (d) { return d.Id === prev; });
            $id(id).value = found ? String(prev) : '0';                         // else Rows[0] (default row) activated
        });
    }
    /** ProjectFill:334 — only when rows exist; Rows[1].Activate() picks the first project. Load only. */
    function projectFill() {
        if (!look.projects.length) return;
        bind('cmbProject', look.projects, 'Id', 'ProjectName', SELECT_ANY);
        $id('cmbProject').value = String(look.projects[0].Id);
    }
    /** ItemNameFill:352 / rdbtnItemName_CheckedChanged:508 — display ItemName or ItemCode. */
    function itemBind(keepId) {
        var byName = $id('rdbtnItemName').checked;
        bind('cmbItem', look.items, 'Id', byName ? 'ItemName' : 'ItemCode', SELECT_ANY);
        $id('cmbItem').value = '0';
        if (keepId > 0) setVal('cmbItem', keepId);
    }
    function itemNameFill() {
        var id = intOf($id('cmbItem').value);
        if (!look.items.length) { $id('cmbItem').innerHTML = ''; return; }    // dtitem (the DataSource) was cleared
        itemBind(0);
        if (id > 0) {
            if (look.items.some(function (x) { return x.Id === id; })) setVal('cmbItem', id);
            else clearCombo('cmbItem');
        }
    }
    function simpleFill(id, rows, textKey) {                                  // FixedAssest:440 / WorkOrderFill:478
        var prev = intOf($id(id).value);
        if (!rows.length) return;                                             // returns before binding
        bind(id, rows, 'Id', textKey, SELECT_ANY);
        $id(id).value = '0';
        if (prev > 0) {
            if (rows.some(function (x) { return x.Id === prev; })) $id(id).value = String(prev);
            else clearCombo(id);
        }
    }
    function selectedItem() { var id = intOf($id('cmbItem').value); return look.items.find(function (x) { return x.Id === id; }) || null; }

    /** ItemUOMFill:397 — per item from the server; keeps the previous UOM Id when the new list has it. */
    function itemUomFill() {
        var prev = intOf($id('cmbItemUOM').value);
        var itemId = intOf($id('cmbItem').value);
        var seq = ++uomSeq;
        return C.getJson(api + '/uoms' + C.qs({ itemId: itemId })).then(function (list) {
            if (seq !== uomSeq) return;
            list = list || [];
            if (!list.length) { $id('cmbItemUOM').innerHTML = ''; return; }    // DataSource = null
            bind('cmbItemUOM', list, 'Id', 'UOMCode', SELECT_ANY);
            $id('cmbItemUOM').value = '0';
            if (prev > 0) {
                if (list.some(function (u) { return u.Id === prev; })) $id('cmbItemUOM').value = String(prev);
                else clearCombo('cmbItemUOM');
            }
        }).catch(function (e) { alert(e.message); });
    }

    /** BalanceStock:1352 — label = stock formatted #,##0.##, made visible. */
    function balanceStock() {
        var seq = ++stockSeq;
        return C.getJson(api + '/stock' + C.qs({ itemId: intOf($id('cmbItem').value), docDate: $id('DocDate').value }))
            .then(function (r) {
                if (seq !== stockSeq) return;
                var l = $id('lblStockQty');
                l.textContent = fmt2(r ? r.balQty : 0);
                l.classList.remove('is-hidden');
            }).catch(function (e) { alert(e.message); });
    }

    function itemLeave() { itemUomFill(); return balanceStock(); }              // cmbItem_Leave:1346

    function searchModeChanged() {                                            // rdbtnItemName_CheckedChanged:508
        if (!look.items.length) return;
        var id = intOf($id('cmbItem').value);
        itemBind(0);
        setVal('cmbItem', id);
        $id('cmbItem').focus();
    }

    function barcode() {                                                      // txtBarcodeReader_KeyDown:1426
        C.getJson(api + '/barcode' + C.qs({ barcode: $id('txtBarcodeReader').value.trim() })).then(function (r) {
            setVal('cmbItem', r.itemId);
            itemLeave();
            $id('txtBarcodeReader').value = '';
            $id('cmbItemUOM').focus();
        }).catch(function (e) { alert(e.message); });
    }

    /* CommonServices.OnlytextNumberFunction — digits and control keys only. */
    function numberKey(e) {
        var ch = e.key;
        if (ch.length !== 1 || /[0-9]/.test(ch)) return true;
        e.preventDefault(); return false;
    }

    /* ------------------------------------------------------------------ entry bar */

    function formValidationDetail() {                                         // FormValidationDetail:584
        if (intOf($id('cmbItem').value) === 0) { alert('Item Field is Required'); $id('cmbItem').focus(); return false; }
        if (intOf($id('cmbItemUOM').value) === 0) { alert('UOM Field is Required'); $id('cmbItemUOM').focus(); return false; }
        if (intOf($id('CmbAssetName').value) === 0) { alert('AssetName Field is Required'); $id('CmbAssetName').focus(); return false; }
        var q = $id('txtRequestedQty').value.trim();
        if (q === '' || num(q) === 0) { alert('RequestedQty Field is Required'); $id('txtRequestedQty').focus(); return false; }
        return true;
    }
    /** The dtdetail column is typed double: text that is not a number throws on Rows.Add. */
    function qtyValue() {
        var t = $id('txtRequestedQty').value.trim(), v = Number(t);
        if (t === '' || isNaN(v)) throw new Error("Input string was not in a correct format.Couldn't store <" + t + "> in RequestedQty Column.  Expected type is Double.");
        return v;
    }

    function add() {                                                          // Add_Click:1213
        try {
            if (!formValidationDetail()) return;
            var itemId = intOf($id('cmbItem').value);
            if (table.some(function (r) { return intOf(r.ItemId) === itemId; })) { alert('Duplicate Item Not Add in Grid'); return; }
            var it = selectedItem() || {};
            table.push({
                Id: 0, ItemId: itemId, Item: it.ItemName || '', ItemCode: it.ItemCode || '',
                ItemUOMId: intOf($id('cmbItemUOM').value), UOM: selText('cmbItemUOM'),
                AssetId: intOf($id('CmbAssetName').value), AssetName: selText('CmbAssetName'),
                RequestedQty: qtyValue()
            });
            render();
            resetDetail();
        } catch (e) { alert(e.message); }
    }

    /** btnUpdateDetail_Click:1275 — ItemUOMId and AssetId of the row are NOT rewritten (desktop note 5). */
    function updateDetail() {
        try {
            if (!formValidationDetail()) return;
            var itemId = intOf($id('cmbItem').value);
            if (table.some(function (r, i) { return intOf(r.ItemId) === itemId && i !== updateDetailIndex; })) { alert('Duplicate Item Not Add in Grid'); return; }
            var r = table[updateDetailIndex];
            if (!r) throw new Error('There is no row at position ' + updateDetailIndex + '.');
            var it = selectedItem() || {};
            r.ItemId = itemId; r.Item = it.ItemName || ''; r.ItemCode = it.ItemCode || '';
            r.UOM = selText('cmbItemUOM');
            r.AssetName = selText('CmbAssetName');
            r.RequestedQty = qtyValue();
            render();
            $id('Add').classList.remove('is-hidden');
            $id('btnUpdateDetail').classList.add('is-hidden');
            $id('btnCancelUpdateDetial').classList.add('is-hidden');
            resetDetail();
        } catch (e) { alert(e.message); }
    }

    function edit(i) {                                                        // grdDetail_DoubleClick:1310
        var r = table[i];
        if (!r) return;
        updateDetailIndex = i;
        setVal('cmbItem', r.ItemId);
        var uom = r.UOM, asset = r.AssetName, qty = r.RequestedQty;
        $id('Add').classList.add('is-hidden');
        $id('btnUpdateDetail').classList.remove('is-hidden');
        $id('btnCancelUpdateDetial').classList.remove('is-hidden');
        setText('CmbAssetName', asset);                                       // :1321 Text = row AssetName
        $id('txtRequestedQty').value = String(qty);                          // :1322
        itemUomFill().then(function () { setText('cmbItemUOM', uom); });     // :1319-1320 ItemUOMFill, then Text = row UOM
    }

    function deleteRow(i) {                                                   // grdDetail_ColumnButtonClick:1164 "Delete"
        var r = table[i];
        if (!r) return;
        if (intOf(r.Id) > 0) {
            if (!confirm('Are you sure to Delete?')) return;
            removed.push({ Id: intOf(r.Id), ItemId: intOf(r.ItemId), ItemUOMId: intOf(r.ItemUOMId), AssetId: intOf(r.AssetId),
                WorkOrderId: intOf($id('cmbworkno').value), RequestedQty: num(r.RequestedQty) });
        }
        table.splice(i, 1);
        render();
    }

    function resetDetail() {                                                  // ResetDetail:648
        clearCombo('cmbItem');
        clearCombo('cmbItemUOM');
        $id('txtRequestedQty').value = '';
        clearCombo('CmbAssetName');
        $id('cmbItem').focus();
    }
    function cancelDetail() { resetDetail(); }                                 // btnCancelUpdateDetial_Click:1334 — buttons stay

    /* ------------------------------------------------------------------ grid */

    function render() {                                                       // grdSettings:1243 — X, Edit, then dtdetail's visible columns
        var t = $id('grdDetail');
        var cols = ['Item', 'ItemCode', 'UOM', 'AssetName', 'RequestedQty'];
        t.tHead.innerHTML = '<tr><th>X</th><th>Edit</th>' + cols.map(function (c) { return '<th>' + c + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = table.map(function (r, i) {
            return '<tr ondblclick="DeptReqConsumable.edit(' + i + ')">' +
                '<td><button type="button" class="dr-cellbtn" onclick="DeptReqConsumable.deleteRow(' + i + ')">X</button></td>' +
                '<td><button type="button" class="dr-cellbtn" onclick="DeptReqConsumable.edit(' + i + ')">Edit</button></td>' +
                cols.map(function (c) { return c === 'RequestedQty' ? '<td class="num">' + esc(r[c]) + '</td>' : '<td>' + esc(r[c]) + '</td>'; }).join('') + '</tr>';
        }).join('');
    }

    /* ------------------------------------------------------------------ save / open / delete */

    function formValidation() {                                               // FormValidation:549
        if (intOf($id('cmbProject').value) === 0) { alert('Project Field is Required'); $id('cmbProject').focus(); return false; }
        var d = $id('txtdocno').value.trim();
        if (d === '' || intOf(d) === 0) { alert('DocNo Field is Required'); $id('txtdocno').focus(); return false; }
        if (intOf($id('cmbworkno').value) === 0) { alert('Work No Field is Required'); $id('cmbworkno').focus(); return false; }
        if (intOf($id('cmbDepartmentFrom').value) === 0) { alert('Department From Field is Required'); $id('cmbDepartmentFrom').focus(); return false; }
        if (intOf($id('cmbDepartmentTo').value) === 0) { alert('Department To Field is Required'); $id('cmbDepartmentTo').focus(); return false; }
        return true;
    }

    function save() { recId = 0; insert(); }                                  // btnSave_Click:797 (RecId = 0)
    function update() {                                                       // btnUpdate_Click:810
        if (isApproved) { alert('Record not Update because record has approved'); return; }
        if (recId === 0) { alert('RecId Not Found'); return; }
        insert();
    }
    function insert() {                                                       // Insert():700
        if (!formValidation()) return;
        if (recId > 0) { if (!confirm('Are you sure to Update?')) return; }
        else if (!confirm('Are you sure to Save?')) return;
        if (!table.length) { alert('Grid record not found'); return; }        // :741
        var print = $id('ChkBok').checked;
        var workOrderId = intOf($id('cmbworkno').value);
        C.postJson(api + '/save', {
            Id: recId, DocDate: $id('DocDate').value,
            ProjectId: intOf($id('cmbProject').value), WorkOrderId: workOrderId,
            FromDepartmentId: intOf($id('cmbDepartmentFrom').value), ToDepartmentId: intOf($id('cmbDepartmentTo').value),
            RemarksHeader: $id('txtremarks').value,
            rows: table.map(function (r) {
                return { Id: intOf(r.Id), ItemId: intOf(r.ItemId), ItemUOMId: intOf(r.ItemUOMId), AssetId: intOf(r.AssetId),
                    RequestedQty: num(r.RequestedQty) };
            }),
            removed: recId > 0 ? removed : []
        }).then(function (res) {
            alert(res.message);
            var id = res.id;
            return reset().then(function () { if (print) printId(id); });      // :785 Reset, :786 Slip(success)
        }).catch(function (e) { alert(e.message); });
    }

    function del() {                                                          // btnDelete_Click:904
        if (recId <= 0) { alert('No Record Found For Deletion'); return; }
        if (!confirm('Are you sure to Delete?')) return;
        C.postJson(api + '/' + recId + '/delete', {}).then(function (res) {
            alert(res.message);
            return reset();
        }).catch(function (e) { alert(e.message); });
    }

    function printId(id) { C.printSlip(api + '/' + intOf(id) + '/slip', 'Department Request To Consumable Store Slip'); }

    function readById(id) {                                                   // ReadById:830 — the entry bar is left as it is
        return C.getJson(api + '/' + id).then(function (h) {
            recId = h.Id;
            removed = [];                                                     // :835 lstRemoveRecord.Clear()
            setVal('cmbProject', h.ProjectId);
            $id('txtdocno').value = h.DocNo;
            $id('DocDate').value = C.isoDay(h.DocDate);
            tab('tabForm');
            setVal('cmbDepartmentFrom', h.FromDepartmentId);
            setVal('cmbDepartmentTo', h.ToDepartmentId);
            $id('txtremarks').value = h.RemarksHeader || '';
            isApproved = !!h.IsApproved;
            table = h.rows || [];
            if (table.length) setVal('cmbworkno', h.WorkOrderId);             // :850 detail[0].WorkOrderId
            render();
            $id('btnSave').classList.add('is-hidden');
            $id('btnUpdate').classList.remove('is-hidden');
            $id('btnDelete').classList.remove('is-hidden');
        }).catch(function (e) { alert(e.message); });
    }

    function docNoLeave() {                                                   // txtdocno_Leave:873
        var t = $id('txtdocno').value.trim();
        if (t === '') return;
        C.getJson(api + '/id-by-docno' + C.qs({ docNo: intOf(t) })).then(function (r) {
            if (r && intOf(r.id) > 0) readById(intOf(r.id));
        }).catch(function (e) { alert(e.message); });
    }

    /* ------------------------------------------------------------------ history */

    function historyGridFill() {                                              // HistoryGridFill:928
        return C.getJson(api + '/history').then(function (list) {
            if (!list || !list.length) return;                                // :941 — the grid keeps what it showed
            var r = rt(), t = $id('DataGridHistory');
            var cols = ['DocDate', 'DocNo', 'WorkOrderNo', 'DepartmentNameFrom', 'ToDepartmentName', 'RemarksHeader',
                'EntryUser', 'EntryDate', 'ModifyUser', 'ModifyDate', 'NoOfAttachments'];
            t.tHead.innerHTML = '<tr>' + (r.print ? '<th>Print</th>' : '') + (r.update ? '<th>Edit</th>' : '') +
                cols.map(function (c) { return '<th>' + c + '</th>'; }).join('') + '</tr>';
            t.tBodies[0].innerHTML = list.map(function (h) {
                var cells = cols.map(function (c) {
                    var v = h[c];
                    if (c === 'DocDate' || c === 'EntryDate' || c === 'ModifyDate') return '<td>' + esc(dateText(v)) + '</td>';
                    if (c === 'DocNo') return r.update
                        ? '<td class="num"><span class="dr-link" onclick="event.stopPropagation();DeptReqConsumable.historyOpen(' + h.Id + ')">' + esc(v) + '</span></td>'
                        : '<td class="num">' + esc(v) + '</td>';
                    if (c === 'NoOfAttachments') return '<td class="num">' + esc(v) + '</td>';
                    return '<td>' + esc(v) + '</td>';
                }).join('');
                return '<tr data-id="' + h.Id + '" onclick="DeptReqConsumable.historyDetail(this)" ondblclick="DeptReqConsumable.historyOpen(' + h.Id + ')">' +
                    (r.print ? '<td><button type="button" class="dr-cellbtn" onclick="event.stopPropagation();DeptReqConsumable.printId(' + h.Id + ')">Print</button></td>' : '') +
                    (r.update ? '<td><button type="button" class="dr-cellbtn" onclick="event.stopPropagation();DeptReqConsumable.historyOpen(' + h.Id + ')">Edit</button></td>' : '') +
                    cells + '</tr>';
            }).join('');
            var first = t.tBodies[0].rows[0];                                 // the bound grid's current row → SelectionChanged
            if (first) historyDetail(first);
        }).catch(function (e) { alert(e.message); });
    }
    function loadAll() { return historyGridFill(); }                           // toolStripButton1_Click:1100 (HistoryGridFill())
    function historyOpen(id) {                                                // DataGridHistory_DoubleClick:1037 / Edit column :1069
        if (!rt().update) return;
        readById(id);
    }
    function historyDetail(tr) {                                              // DataGridHistory_SelectionChanged:1112
        document.querySelectorAll('#DataGridHistory tbody tr').forEach(function (x) { x.classList.remove('is-selected'); });
        tr.classList.add('is-selected');
        var seq = ++histSeq;                                                  // only the last selected row paints the detail
        C.getJson(api + '/' + tr.getAttribute('data-id')).then(function (h) {
            if (seq !== histSeq) return;
            var cols = [['Item', 'ItemName'], ['ItemCode', 'ItemCode'], ['UOM', 'UOM'], ['AssetName', 'AssetName'], ['RequestedQty', 'RequestedQty']];
            var t = $id('DataGridHistoryDetail'), total = 0;
            t.tHead.innerHTML = '<tr>' + cols.map(function (c) { return '<th>' + c[1] + '</th>'; }).join('') + '</tr>';
            t.tBodies[0].innerHTML = (h.rows || []).map(function (r) {
                total += num(r.RequestedQty);
                return '<tr>' + cols.map(function (c) {
                    return c[0] === 'RequestedQty' ? '<td class="num">' + esc(fmt2(r[c[0]])) + '</td>' : '<td>' + esc(r[c[0]]) + '</td>';
                }).join('') + '</tr>';
            }).join('');
            t.tFoot.innerHTML = '<tr><td colspan="4"></td><td class="num">' + esc(fmt2(total)) + '</td></tr>';   // AggregateFunction Sum
        }).catch(function (e) { alert(e.message); });
    }

    function tab(id) {                                                        // tabControl1_SelectedIndexChanged:1022
        var was = document.querySelector('.win-tab.is-active');
        var changed = !was || was.getAttribute('data-tab') !== id;
        document.querySelectorAll('.win-tab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-tab') === id); });
        document.querySelectorAll('.win-tab-panel').forEach(function (p) { p.classList.toggle('is-active', p.id === id); });
        if (changed && id === 'tabHistory') historyGridFill();                // HistoryGridFill(50)
    }

    /* ------------------------------------------------------------------ load / reset / refresh */

    function applyLists(l) {
        look.departments = l.departments || []; look.assets = l.assets || [];
        look.items = l.items || []; look.workOrders = l.workOrders || [];
        departmentFill();
        simpleFill('CmbAssetName', look.assets, 'AssetName');
        itemNameFill();
        simpleFill('cmbworkno', look.workOrders, 'WorkOrderNo');
    }

    function init() {                                                         // EccountBook_Load:225
        return C.getJson(api + '/lookups').then(function (l) {
            look.rights = l.rights || {};
            var r = look.rights;
            $id('btnSave').disabled = !r.save; $id('btnUpdate').disabled = !r.update;
            $id('btnDelete').disabled = !r.delete; $id('btnPrint1615').disabled = !r.print;
            $id('ChkBok').disabled = !r.print; $id('ChkBok').checked = !!r.print;
            if (l.itemSearchByCode) $id('rdbtnItemCode').checked = true; else $id('rdbtnItemName').checked = true;
            $id('lblStockQty').classList.add('is-hidden');
            render();
            $id('txtdocno').value = l.docNo;                                  // GenerateCode:293
            look.departments = l.departments || []; departmentFill();         // :277
            look.projects = l.projects || []; projectFill();                  // :278
            look.assets = l.assets || []; simpleFill('CmbAssetName', look.assets, 'AssetName');   // :279
            look.items = l.items || []; itemNameFill();                       // :280
            look.workOrders = l.workOrders || []; simpleFill('cmbworkno', look.workOrders, 'WorkOrderNo');   // :281
            $id('DocDate').focus();
            $id('btnSave').classList.remove('is-hidden');
            $id('btnUpdate').classList.add('is-hidden');
            $id('btnDelete').classList.add('is-hidden');
        }).catch(function (e) { alert(e.message); });
    }

    function reset() {                                                        // Reset():613
        recId = 0; isApproved = false;
        clearCombo('cmbDepartmentFrom'); clearCombo('cmbDepartmentTo');
        $id('txtremarks').value = ''; $id('txtdocno').value = '';
        clearCombo('cmbItem'); clearCombo('cmbItemUOM');
        $id('txtRequestedQty').value = '';
        clearCombo('CmbAssetName');
        $id('DocDate').value = C.today();                                     // :629 DateTime.Now
        clearCombo('cmbworkno');
        table = []; render();
        $id('btnUpdateDetail').classList.add('is-hidden');
        $id('btnCancelUpdateDetial').classList.add('is-hidden');
        $id('Add').classList.remove('is-hidden');
        $id('btnSave').classList.remove('is-hidden');
        $id('btnUpdate').classList.add('is-hidden');
        $id('btnDelete').classList.add('is-hidden');
        return C.getJson(api + '/doc-no').then(function (r) {                // :639 GenerateCode
            if (recId === 0) $id('txtdocno').value = r.docNo;
            $id('DocDate').focus();
        }).catch(function (e) { alert(e.message); });
    }

    function refresh() {                                                      // btnRefresh_Click:671 — no projects
        return C.getJson(api + '/lists').then(applyLists).catch(function (e) { alert(e.message); });
    }

    /* button2_Click:1378 (wired to BtnDepartmentDefine) — new Define_Department(UserAccount).Show().
       Desktop behaviour: this page's Department From / To combos are NOT the Department table — they are
       the active warehouses of WareHouseTypeId 4 (DeprtmentFill:318), so a department defined there never
       appears in them, not even after Refresh. Nothing is re-filled when the dialog closes. */
    function defineDepartment() {
        window.StoreDefine.openDepartment(null, { host: 'DepartmentRequestToConsumableStore' });
    }
    /* button1_Click:1390 — new frmLookUpDefineAsset(UserAccount).Show(). The desktop picks new assets up
       only on btnRefresh_Click:671 (FixedAssest:440). Web deviation: when the dialog closes, only the
       Asset combo is re-filled from this page's own /lists (Sp_FixedAssetsRegister_GetAllMethod ReadAll),
       keeping the selection (simpleFill = FixedAssest:440, incl. its "no rows → leave as is"). */
    function defineAsset() {
        window.StoreDefine.openAsset(function () {
            C.getJson(api + '/lists').then(function (l) {
                look.assets = l.assets || [];
                simpleFill('CmbAssetName', look.assets, 'AssetName');
            }).catch(function (e) { alert(e.message); });
        }, { host: 'DepartmentRequestToConsumableStore' });
    }

    window.DeptReqConsumable = {
        reset: reset, save: save, update: update, del: del, refresh: refresh, tab: tab,
        defineDepartment: defineDepartment, defineAsset: defineAsset,
        print: function () { printId(recId); }, printId: printId,               // printToolStripButton_Click:1402
        searchModeChanged: searchModeChanged, itemLeave: itemLeave, barcode: barcode, numberKey: numberKey,
        add: add, updateDetail: updateDetail, cancelDetail: cancelDetail, edit: edit, deleteRow: deleteRow,
        docNoLeave: docNoLeave, loadAll: loadAll, historyDetail: historyDetail, historyOpen: historyOpen
    };

    document.addEventListener('DOMContentLoaded', function () {
        $id('DocDate').value = C.today();                                     // designer: DateTime.Now
        init();
    });
})();
