/* ============================================================================================
 * Screen 340 "Purchase Demand" — Architecture.WinApp.StoreManagement.frmPurchaseDemand,
 * ScreenName "frmPurchaseDemand" (the Store path, ParentCategoryId 8), DocumentTypeId 141.
 * Server side and the numbered behaviour notes / deviations: PurchaseDemandService.java.
 *
 *   Form_Load:286          combos, doc no, print-preview from rights, history dates (first load only)
 *   cmbItem_Leave:641      UOM list, last store purchase (doc type 64), outstanding + stock
 *   Add_Click:1038         FormValidationOfDetailPortion, duplicate item, FillDetailRow
 *   DoubleClick:1090       edit a row (condition / outstanding NOT restored — B6)
 *   txtDocdate_ValueChanged:1537  outstanding + stock recomputed for every row and the entry bar
 *   Insert():739           FormValidation, confirm, deleted-rows warning, save, reset, slip
 * ============================================================================================ */
(function () {
    'use strict';
    var C = window.StoreCommon, $id = C.$id, esc = C.esc, num = C.num, intOf = C.intOf;
    var api = '/api/store/purchase-demand';

    var look = { rights: {}, items: [], uoms: [], itemConditions: [], departments: [], assets: [], jobLots: [], parentCategories: [] };
    var RECID = 0;
    var table = [];                 // dtgrddetail
    var updateDetailIndex = -1;
    var detailIdsToDelete = [];     // HashSet<int> DetailIdsToDelete
    var loaded = false;             // Form_Load done

    var MONTHS = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

    /* .NET custom format "#,##.##" — 0 prints as "", 0.5 as ".5", 1234.5 as "1,234.5". */
    function fmtHash(v) {
        var n = num(v), neg = n < 0, a = Math.round(Math.abs(n) * 100) / 100;
        if (a === 0) return '';
        var ip = Math.floor(a), fp = Math.round((a - ip) * 100);
        var s = ip > 0 ? ip.toLocaleString('en-US') : '';
        if (fp > 0) s += '.' + String(fp).padStart(2, '0').replace(/0+$/, '');
        return (neg ? '-' : '') + s;
    }
    /* Conversion.ToDateTime(text) then "dd-MMM-yyyy", or "" for an unparseable text / 1900. */
    function lastDateText(t) {
        var m = /^(\d{2})-([A-Za-z]{3})-(\d{4})$/.exec(String(t || '').trim());
        if (!m || MONTHS.indexOf(m[2]) < 0 || m[3] === '1900') return '';
        return m[0];
    }
    function selText(id) { var e = $id(id); return e.selectedOptions && e.selectedOptions[0] && e.value !== '0' ? e.selectedOptions[0].textContent.trim() : ''; }
    function byName() { return $id('rdSearchByName').checked; }

    /* ------------------------------------------------------------------ combos */

    function itemBind() {                                                       // ItemNameBind:535 (retains the item)
        C.fillSelect('cmbItem', look.items, 'Id', byName() ? 'ItemName' : 'ItemCode');
    }
    function selectedItem() { var id = intOf($id('cmbItem').value); return look.items.find(function (x) { return x.Id === id; }) || null; }

    function uomBind(itemId) {                                                  // ItemUomFill:570 — keeps the UOM by its text
        var el = $id('cmbUOM'), keep = selText('cmbUOM');
        var list = look.uoms.filter(function (u) { return u.ItemId === itemId; });
        C.fillSelect(el, list, 'Id', 'UOMCode');
        var hit = list.find(function (u) { return keep && u.UOMCode === keep; });
        el.value = hit ? String(hit.Id) : '0';
    }

    function lastPurchase(itemId) {                                             // UpdateLastPurchaseInfo:667
        return C.getJson(api + '/last-purchase' + C.qs({ itemId: itemId })).then(function (r) {
            $id('txtLastDate').value = r.LastDate; $id('txtLastQty').value = r.LastQty; $id('txtLastRate').value = r.LastRate;
        }).catch(function (e) { alert(e.message); });
    }
    function outstanding(itemId, conditionId) {
        return C.getJson(api + '/outstanding' + C.qs({ itemId: itemId, docDate: $id('txtDocdate').value, itemConditionId: conditionId, recId: RECID }));
    }
    function barOutstanding() {                                                 // UpdateOutstandingDemandAndStockQty:705
        return outstanding(intOf($id('cmbItem').value), intOf($id('CmbItemCondition').value)).then(function (r) {
            $id('txtOutstandingDemandAndPOQty').value = fmtHash(r.OutstandingDemandAndPOQty);
            $id('txtAvaialableStockQty').value = fmtHash(r.AvaialableStockQty);
        }).catch(function (e) { alert(e.message); });
    }
    function itemLeave() {                                                      // cmbItem_Leave:641
        var itemId = intOf($id('cmbItem').value);
        uomBind(itemId);
        return Promise.all([lastPurchase(itemId), barOutstanding()]);
    }
    function conditionLeave() { return barOutstanding(); }                     // CmbItemCondition_Leave:655
    function searchModeChanged() { itemBind(); }                                // rdSearchByName_CheckedChanged:727

    function barcodeKey(e) {                                                    // txtBarcodeReader_KeyDown:1982
        if (e.key !== 'Enter') return;
        e.preventDefault();
        C.getJson(api + '/barcode' + C.qs({ no: $id('txtBarcodeReader').value.trim() })).then(function (r) {
            var id = intOf(r.itemId);
            $id('cmbItem').value = look.items.some(function (x) { return x.Id === id; }) ? String(id) : '0';
            itemLeave();
            $id('txtBarcodeReader').value = '';
            $id('cmbUOM').focus();
        }).catch(function (e2) { alert(e2.message); });
    }
    function decimalKey(e, box) {                                               // OnlytextdecimelFunction
        if (e.key.length !== 1) return true;
        if (e.key === '.') return box.value.indexOf('.') < 0;
        return /\d/.test(e.key);
    }
    function digitKey(e) { return e.key.length !== 1 || /\d/.test(e.key); }     // OnlytextNumberFunction

    /* ------------------------------------------------------------------ detail grid */

    var COLS = [['Department', 'Department'], ['RequistionBy', 'RequistionBy'], ['ItemCode', 'ItemCode'], ['ItemName', 'ItemName'],
        ['ItemUOM', 'UOM'], ['ItemCondition', 'ItemCondition'], ['RequiredQty', 'RequiredQty', 1], ['ApprovedQty', 'ApprovedQty', 1],
        ['ApprovalStatus', 'ApprovalStatus'], ['JobLot', 'JobLot'], ['AssetsRef', 'AssetsRef'], ['Remarks', 'Remarks'],
        ['LastDate', 'Last Purchase Date'], ['LastQty', 'Last Purchase Qty', 1], ['LastRate', 'Last Purchase Rate', 1],
        ['OutstandingDemandAndPOQty', 'OutstandingDemandAndPOQty', 1], ['AvaialableStockQty', 'AvaialableStockQty', 1]];

    function cellHtml(r, c) {
        var v = r[c[0]];
        if (c[0] === 'ApprovalStatus') return '<td style="text-align:center"><input type="checkbox" disabled' + (v ? ' checked' : '') + '></td>';
        return '<td' + (c[2] ? ' class="num"' : '') + '>' + esc(v === null || v === undefined ? '' : v) + '</td>';
    }

    function render() {                                                         // gridsettings:1128
        var t = $id('grdPurchaseDemand'), upd = !!(look.rights || {}).update;
        t.tHead.innerHTML = '<tr>' + (upd ? '<th></th><th>Edit</th>' : '') + COLS.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = table.map(function (r, i) {
            return '<tr ondblclick="PurDemand.edit(' + i + ')"' + (i === updateDetailIndex ? ' class="is-selected"' : '') + '>' +
                (upd ? '<td><button type="button" class="cx-x" onclick="PurDemand.del(' + i + ')">X</button></td>' +
                    '<td><button type="button" class="cx-link" onclick="PurDemand.edit(' + i + ')">Edit</button></td>' : '') +
                COLS.map(function (c) {
                    if (c[0] === 'Remarks') {                                   // the one editable column
                        return '<td><input type="text" value="' + esc(r.Remarks || '') + '" oninput="PurDemand.remarksCell(' + i + ', this.value)" ondblclick="event.stopPropagation()"></td>';
                    }
                    return cellHtml(r, c);
                }).join('') + '</tr>';
        }).join('');
    }
    function remarksCell(i, v) { if (table[i]) table[i].Remarks = v; }

    function validateDetail() {                                                 // FormValidationOfDetailPortion:1385
        if (intOf($id('CmbParentCategory').value) === 0) { alert('Parent Category Field Required'); return false; }
        if (intOf($id('cmbDepartment').value) === 0) { alert('Department Field Required'); $id('cmbDepartment').focus(); return false; }
        if ($id('txtRequisition').value.trim() === '') { alert('Requisition By Field Required'); $id('txtRequisition').focus(); return false; }
        if (intOf($id('cmbItem').value) === 0) { alert(' Item Field Required'); $id('cmbItem').focus(); return false; }
        if ($id('txtRequiredQty').value === '') { alert('Required Qty Field Required'); $id('cmbUOM').focus(); return false; }   // B4
        if (intOf($id('cmbLot').value) === 0) { alert(' JobLot Field Required'); $id('cmbLot').focus(); return false; }
        if (intOf($id('cmbAssetsRef').value) === 0) { alert('Asset Field Required'); $id('cmbAssetsRef').focus(); return false; }
        return true;
    }
    function isDuplicate(itemId, skip) {                                        // IsDuplicateItem:999
        return table.some(function (r, i) { return i !== skip && intOf(r.ItemId) === itemId; });
    }
    function fillRow(row) {                                                     // FillDetailRow:1011
        var it = selectedItem();
        row.Department = selText('cmbDepartment');
        row.DepartmentId = intOf($id('cmbDepartment').value);
        row.RequistionBy = $id('txtRequisition').value;
        row.ItemId = intOf($id('cmbItem').value);
        row.ItemCode = it ? it.ItemCode : '';
        row.ItemName = it ? it.ItemName : '';
        row.ItemUOM = selText('cmbUOM');                                        // B7 — the code
        row.ItemUOMId = intOf($id('cmbUOM').value);
        row.ItemConditionId = intOf($id('CmbItemCondition').value);
        row.ItemCondition = selText('CmbItemCondition');
        row.RequiredQty = num($id('txtRequiredQty').value);
        row.ApprovedQty = num($id('txtRequiredQty').value);                     // B3
        row.JobLot = selText('cmbLot');
        row.JobLotId = intOf($id('cmbLot').value);
        row.AssetsRef = selText('cmbAssetsRef');
        row.AssetsRefId = intOf($id('cmbAssetsRef').value);
        row.Remarks = $id('txtRemarksDetail').value;
        row.LastDate = lastDateText($id('txtLastDate').value);
        row.LastQty = num($id('txtLastQty').value);
        row.LastRate = num($id('txtLastRate').value);
        row.OutstandingDemandAndPOQty = num($id('txtOutstandingDemandAndPOQty').value);
        row.AvaialableStockQty = num($id('txtAvaialableStockQty').value);
    }
    function add() {                                                            // Add_Click:1038
        if (!validateDetail()) return;
        if (isDuplicate(intOf($id('cmbItem').value), -1)) { alert('Duplicate Item Not Allowed in Grid'); return; }
        var row = { Id: 0, ApprovalStatus: false };
        fillRow(row);
        table.push(row);
        resetDetail();
        render();
    }
    function updateDetail() {                                                   // btnUpdateDetail_Click:1066
        if (!validateDetail()) return;
        if (isDuplicate(intOf($id('cmbItem').value), updateDetailIndex)) { alert('Duplicate Item Not Allowed in Grid'); return; }
        fillRow(table[updateDetailIndex]);
        resetDetail();
        render();
    }
    function edit(i) {                                                          // grdPurchaseDemand_DoubleClick:1090
        var r = table[i];
        if (!r) return;
        if (r.ApprovalStatus) { alert('You cannot Update Approved Row'); return; }
        updateDetailIndex = i;
        $id('cmbDepartment').value = String(intOf(r.DepartmentId));
        $id('txtRequisition').value = r.RequistionBy || '';
        $id('cmbLot').value = String(intOf(r.JobLotId));
        $id('cmbItem').value = String(intOf(r.ItemId));
        uomBind(intOf(r.ItemId));
        $id('cmbUOM').value = String(intOf(r.ItemUOMId));
        $id('txtRequiredQty').value = r.RequiredQty === null || r.RequiredQty === undefined ? '' : String(r.RequiredQty);
        $id('cmbAssetsRef').value = String(intOf(r.AssetsRefId));
        $id('txtRemarksDetail').value = r.Remarks || '';
        $id('txtLastDate').value = lastDateText(r.LastDate);
        $id('txtLastQty').value = r.LastQty === null || r.LastQty === undefined ? '' : String(r.LastQty);
        $id('txtLastRate').value = r.LastRate === null || r.LastRate === undefined ? '' : String(r.LastRate);
        /* B6 — Item Condition, Outstanding and Available Stock are not restored. */
        $id('Add').classList.add('is-hidden');
        $id('btnUpdateDetail').classList.remove('is-hidden');
        $id('btnCancelDetail').classList.remove('is-hidden');
        render();
        $id('cmbDepartment').focus();
    }
    function del(i) {                                                           // DeleteDetailRow:958
        var r = table[i];
        if (!r) return;
        if (r.ApprovalStatus) { alert('You cannot Update Approved Row'); return; }
        if (updateDetailIndex !== -1) { alert('Reset Detail First'); return; }
        if (intOf(r.Id) > 0) {
            if (!confirm('Are you sure to Delete?')) return;
            if (detailIdsToDelete.indexOf(intOf(r.Id)) < 0) detailIdsToDelete.push(intOf(r.Id));
        }
        table.splice(i, 1);                                                     // D7 — one row
        render();
    }
    function resetDetail() {                                                    // resetDetail:1469
        $id('cmbUOM').value = '0';
        $id('txtRequiredQty').value = '';
        $id('txtRemarksDetail').value = '';
        $id('cmbLot').value = '0';
        $id('cmbAssetsRef').value = '0';
        $id('Add').classList.remove('is-hidden');
        $id('btnUpdateDetail').classList.add('is-hidden');
        $id('btnCancelDetail').classList.add('is-hidden');
        updateDetailIndex = -1;
        render();
        $id('cmbDepartment').focus();
    }

    function docDateChanged() {                                                 // txtDocdate_ValueChanged:1537
        Promise.all(table.map(function (r) {
            return outstanding(intOf(r.ItemId), intOf(r.ItemConditionId)).then(function (x) {
                r.OutstandingDemandAndPOQty = num(x.OutstandingDemandAndPOQty);
                r.AvaialableStockQty = num(x.AvaialableStockQty);
            });
        })).then(function () { render(); return barOutstanding(); }).catch(function (e) { alert(e.message); });
    }

    /* ------------------------------------------------------------------ save / open / print */

    function save() { RECID = 0; insert(); }                                    // btnSave_Click:860
    function update() { insert(); }                                             // btnUpdate_Click:873
    function insert() {                                                         // Insert():739
        if (intOf($id('CmbParentCategory').value) === 0) { alert('Parent Category Field Required'); return; }   // FormValidation:1370
        var d = $id('txtdocnumber').value.trim();
        if (d === '' || intOf(d) === 0) { alert('Doc No Field Required'); $id('txtdocnumber').focus(); return; }
        if (!confirm(RECID > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        if (detailIdsToDelete.length && !confirm('You have deleted some rows from the detail data.\nAre you sure you want to proceed?')) return;
        if (!table.length) { alert('Grid Record not found'); return; }
        var preview = $id('chkPrintPreview').checked;
        C.postJson(api + '/save', {
            Id: RECID, DocNo: intOf(d), DocDate: $id('txtDocdate').value, RemarksHeader: $id('txtRemarks').value,
            DetailIdsToDelete: detailIdsToDelete.join(','), rows: table
        }).then(function (res) {
            alert(res.message);
            var id = res.id;
            reset().then(function () { if (preview) printId(id); });
        }).catch(function (e) { alert(e.message); });
    }
    function printId(id) {                                                      // PurchaseDemandSlip454
        if (!id) { alert('No Record Found For Display'); return; }
        C.printSlip(api + '/' + id + '/slip', 'Purchase Demand Slip (454)');
    }

    function readById(id) {                                                     // ReadById:885
        C.getJson(api + '/' + id).then(function (h) {
            RECID = h.Id;
            detailIdsToDelete = [];                                             // D9 (the desktop keeps them — see service Javadoc)
            $id('btnSave').classList.add('is-hidden'); $id('btnUpdate').classList.remove('is-hidden');
            tab('tabForm');
            $id('txtdocnumber').value = h.DocNo;
            var before = $id('txtDocdate').value;
            $id('txtDocdate').value = C.isoDay(h.DocDate);
            $id('txtRemarks').value = h.RemarksHeader || '';
            $id('lblRecId').textContent = 'Record #' + RECID;
            table = h.rows || [];
            render();
            if (before !== $id('txtDocdate').value) barOutstanding();           // ValueChanged fires on the entry bar
        }).catch(function (e) { alert(e.message); });
    }

    /* ------------------------------------------------------------------ history */

    function showHistory() {                                                    // HistoryFill:1612
        var dt = document.querySelector('input[name="histDate"]:checked');
        C.getJson(api + '/history' + C.qs({
            dateType: dt ? dt.value : 'doc',
            fromDate: $id('chkFromDate').checked ? $id('FromDateHistory').value : '',
            toDate: $id('chkToDate').checked ? $id('ToDateHistory').value : '',
            fromDocNo: intOf($id('txtFromDocNoHistory').value), toDocNo: intOf($id('txtToDocNoHistory').value)
        })).then(function (list) {
            var t = $id('DatagridHistory');
            t.tHead.innerHTML = '<tr><th>Edit</th><th>Slip</th><th>DocDate</th><th>DocNo</th><th>Status</th><th>RequiredQty</th><th>RequestBy</th>' +
                '<th>EntryUser</th><th>EntryDate</th><th>ModifyUser</th><th>ModifyDate</th><th>NoOfAttachments</th><th>RemarksHeader</th></tr>';
            t.tBodies[0].innerHTML = (list || []).map(function (h) {
                return '<tr data-id="' + h.Id + '" onclick="PurDemand.historyDetail(this)" ondblclick="PurDemand.open(' + h.Id + ')">' +
                    '<td><button type="button" class="cx-link" onclick="event.stopPropagation();PurDemand.open(' + h.Id + ')">Edit</button></td>' +
                    '<td><button type="button" class="cx-link" onclick="event.stopPropagation();PurDemand.printId(' + h.Id + ')">Slip</button></td>' +
                    '<td>' + esc(C.gridDate(h.DocDate)) + '</td><td>' + esc(h.DocNo) + '</td><td>' + esc(h.Status) + '</td><td class="num">' + esc(h.RequiredQty) +
                    '</td><td>' + esc(h.RequestBy) + '</td><td>' + esc(h.EntryUser) + '</td><td>' + esc(C.gridDateTime(h.EntryDate)) + '</td><td>' +
                    esc(h.ModifyUser) + '</td><td>' + esc(C.gridDateTime(h.ModifyDate)) + '</td><td class="num">' + esc(h.NoOfAttachments) + '</td><td>' +
                    esc(h.RemarksHeader) + '</td></tr>';
            }).join('');
            $id('grdDetail').tHead.innerHTML = ''; $id('grdDetail').tBodies[0].innerHTML = '';
        }).catch(function (e) { alert(e.message); });
    }
    function historyDetail(tr) {                                                // DatagridHistory_SelectionChanged:1835
        document.querySelectorAll('#DatagridHistory tbody tr').forEach(function (x) { x.classList.remove('is-selected'); });
        tr.classList.add('is-selected');
        C.getJson(api + '/' + tr.getAttribute('data-id')).then(function (h) {
            var t = $id('grdDetail'), rows = h.rows || [];
            if (!rows.length) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; return; }
            t.tHead.innerHTML = '<tr>' + COLS.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('') + '</tr>';
            t.tBodies[0].innerHTML = rows.map(function (r) { return '<tr>' + COLS.map(function (c) { return cellHtml(r, c); }).join('') + '</tr>'; }).join('');
        }).catch(function (e) { alert(e.message); });
    }
    function newHistory() {                                                     // btnNewHistory_Click:1595
        $id('FromDateHistory').value = C.today(); $id('ToDateHistory').value = C.today();
        $id('txtFromDocNoHistory').value = ''; $id('txtToDocNoHistory').value = '';
        ['DatagridHistory', 'grdDetail'].forEach(function (id) { $id(id).tHead.innerHTML = ''; $id(id).tBodies[0].innerHTML = ''; });
    }

    function tab(id) {
        document.querySelectorAll('.win-tab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-tab') === id); });
        document.querySelectorAll('.win-tab-panel').forEach(function (p) { p.classList.toggle('is-active', p.id === id); });
    }

    /* ------------------------------------------------------------------ load / reset / refresh */

    /* Form_Load:286 on the first call; btnRefresh_Click:1490 afterwards (rebind, keep selections). */
    function init() {
        return C.getJson(api + '/lookups').then(function (l) {
            look = l;
            var r = l.rights || {};
            $id('btnSave').disabled = !r.save; $id('btnUpdate').disabled = !r.update; $id('btnPrint').disabled = !r.print;
            if (!loaded) {
                $id('chkPrintPreview').checked = !!r.print;                     // :309-310
                $id('chkPrintPreview').disabled = !r.print;
                if (l.itemSearchByCode) $id('rdSearchByCode').checked = true; else $id('rdSearchByName').checked = true;   // :315
                var days = intOf(l.defaultDaysToLessFromHistoryFromDate) > 0 ? intOf(l.defaultDaysToLessFromHistoryFromDate) : 3;   // :360
                var f = new Date(); f.setDate(f.getDate() - days);
                $id('FromDateHistory').value = f.getFullYear() + '-' + String(f.getMonth() + 1).padStart(2, '0') + '-' + String(f.getDate()).padStart(2, '0');
                $id('ToDateHistory').value = C.today();
                $id('txtdocnumber').value = l.docNo > 0 ? l.docNo : '';          // GenerateDocNumber:388
            }
            if (!r.view) { $id('rightsNote').textContent = 'You do not have the View right for Purchase Demand.'; $id('rightsNote').classList.remove('is-hidden'); }
            C.fillSelect('cmbDepartment', l.departments, 'Id', 'Name');
            C.fillSelect('CmbParentCategory', l.parentCategories, 'Id', 'Description', false);   // D8 — its single row
            itemBind();
            C.fillSelect('CmbItemCondition', l.itemConditions, 'Id', 'Description');
            C.fillSelect('cmbLot', l.jobLots, 'Id', 'Name');
            C.fillSelect('cmbAssetsRef', l.assets, 'Id', 'Name');
            /* Default rows as the desktop binders add them: DropDownBind.BindDDL/BindDDLNew(ZeroIndex) →
               "...Select Any Value..." (DepartmentFill:449, combojoblotfill:593, AssetsFill:407);
               InfragisticsHelper.BindAndRetainSelection(insertDefaultRow) → "-- Select --" (ItemConditionBindFromGlobal:560). */
            defRow('cmbDepartment', '...Select Any Value...');
            defRow('cmbLot', '...Select Any Value...');
            defRow('cmbAssetsRef', '...Select Any Value...');
            defRow('CmbItemCondition', '-- Select --');
            if (!loaded) ['cmbDepartment', 'cmbLot', 'cmbAssetsRef', 'CmbItemCondition'].forEach(function (id) { $id(id).value = '0'; });
            loaded = true;
            render();
            return l;
        }).catch(function (e) { alert(e.message); });
    }
    function defRow(id, text) {
        var el = $id(id); if (!el || !el.options.length || el.options[0].value !== '0') return;
        el.options[0].textContent = text;
    }
    function reset() {                                                          // reset():1445 (doc date kept)
        detailIdsToDelete = [];
        RECID = 0;
        $id('btnSave').classList.remove('is-hidden'); $id('btnUpdate').classList.add('is-hidden');
        $id('txtRemarks').value = '';
        table = [];
        $id('lblRecId').textContent = '';
        resetDetail();
        return C.getJson(api + '/lookups').then(function (l) {                  // GenerateDocNumber:1461
            $id('txtdocnumber').value = l.docNo > 0 ? l.docNo : $id('txtdocnumber').value;
        }).catch(function (e) { alert(e.message); });
    }
    function newDoc() { resetDetail(); return reset(); }                        // btnNew_Click:1432
    function refresh() { return init(); }                                       // btnRefresh_Click:1490

    /* button2_Click:1946 / btnAssetRefDefine_Click:1958 — new Define_Department / frmLookUpDefineAsset(UserAccount).Show().
       The desktop picks new rows up only on btnRefresh_Click:1490 (DepartmentFill:441 / FixedAssest:399).
       Web deviation: when the dialog closes, only that combo is re-filled from this page's own /lookups
       (same source), keeping the selection; a kept Id no longer in the list is cleared (DepartmentFill:467). */
    function refillCombo(id, key) {
        return C.getJson(api + '/lookups').then(function (l) {
            var keep = intOf($id(id).value);
            look[key] = l[key];
            C.fillSelect(id, l[key], 'Id', 'Name');
            defRow(id, '...Select Any Value...');
            var list = l[key] || [];
            $id(id).value = keep > 0 && list.some(function (x) { return intOf(x.Id) === keep; }) ? String(keep) : '0';
        }).catch(function (e) { alert(e.message); });
    }
    function defineDepartment() {
        window.StoreDefine.openDepartment(function () { refillCombo('cmbDepartment', 'departments'); }, { host: 'frmPurchaseDemand' });
    }
    function defineAsset() {
        window.StoreDefine.openAsset(function () { refillCombo('cmbAssetsRef', 'assets'); }, { host: 'frmPurchaseDemand' });
    }

    window.PurDemand = {
        newDoc: newDoc, refresh: refresh, save: save, update: update, tab: tab, open: readById,
        defineDepartment: defineDepartment, defineAsset: defineAsset,
        print: function () { printId(RECID); }, printId: printId,
        itemLeave: itemLeave, conditionLeave: conditionLeave, searchModeChanged: searchModeChanged,
        barcodeKey: barcodeKey, decimalKey: decimalKey, digitKey: digitKey,
        add: add, updateDetail: updateDetail, resetDetail: resetDetail, edit: edit, del: del, remarksCell: remarksCell,
        docDateChanged: docDateChanged, showHistory: showHistory, historyDetail: historyDetail, newHistory: newHistory
    };

    document.addEventListener('DOMContentLoaded', function () {
        $id('txtDocdate').value = C.today();                                    // designer default: DateTime.Now
        init().then(function () { resetDetail(); });
    });
})();
