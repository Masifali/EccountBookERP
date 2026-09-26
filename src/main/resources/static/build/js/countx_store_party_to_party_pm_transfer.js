/* ============================================================================================
 * Screen 335 — Packing Material Transfer (Party to Party)
 * Desktop: Architecture.WinApp.StoreManagement.PartyToPartyPackingMaterialTransfer
 * (PartyToPartyPackingMaterialTransfer.cs), DocumentTypeId 125.
 * Line references (:NNN) are to that file. Business rules are re-checked on the server.
 * ============================================================================================ */
(function () {
    'use strict';

    var C = window.StoreCommon;
    var $ = C.$id;
    var API = '/api/store/party-to-party-pm-transfer';

    var look = null;           // lookups (rights, lists)
    var recId = 0;             // RecId — only btnSave_Click (:632) and ReadById (:865) change it
    var table = [];            // "table" (:257-270)
    var gridStructured = false;// false after Reset:805 ClearStructure
    var gridSettingsApplied = false; // Form_Load:272 RetrieveStructure without grdSettings
    var updateDetailIndex = -1;
    var uoms = [];
    var historyRows = [];
    var historyCurrent = -1;

    var FORM_ALL_COLS = ['Id', 'ItemId', 'ItemCode', 'ItemName', 'PackUomId', 'PackUom', 'ItemConditionId',
        'ItemCondition', 'Qty', 'Remarks', 'SupplierFromId', 'SupplierFrom', 'SupplierToId', 'SupplierTo'];
    /* grdSettings:462 — Id, ItemId, PackUomId, ItemConditionId, SupplierFromId, SupplierToId hidden. */
    var FORM_COLS = ['ItemCode', 'ItemName', 'PackUom', 'ItemCondition', 'Qty', 'Remarks', 'SupplierFrom', 'SupplierTo'];

    // ------------------------------------------------------------------------------ helpers
    function fmt(v, dec) {
        var n = C.num(v);
        return n.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: dec });
    }
    function esc(s) { return C.esc(s); }
    function val(id) { var e = $(id); return e ? e.value : ''; }
    function selVal(id) { var e = $(id); return (e && e.selectedIndex >= 0) ? C.intOf(e.value) : 0; }
    function selText(id) { var e = $(id); return (e && e.selectedIndex >= 0) ? e.options[e.selectedIndex].text.trim() : ''; }
    function clearCombo(id) { var e = $(id); if (e) e.selectedIndex = -1; }                  // .Text = string.Empty
    function hasOption(el, v) {
        for (var i = 0; i < el.options.length; i++) if (String(el.options[i].value) === String(v)) return true;
        return false;
    }
    function setCombo(id, v) {                                                                  // combo.Value = v
        var e = $(id);
        if (!e) return;
        if (hasOption(e, v)) e.value = String(v); else e.selectedIndex = -1;
    }
    function show(id, on) { var e = $(id); if (e) e.classList.toggle('is-hidden', !on); }
    function addDays(iso, d) {
        var p = iso.split('-'), dt = new Date(+p[0], +p[1] - 1, +p[2]);
        dt.setDate(dt.getDate() + d);
        return dt.getFullYear() + '-' + String(dt.getMonth() + 1).padStart(2, '0') + '-' + String(dt.getDate()).padStart(2, '0');
    }
    function msg(t) { alert(t); }

    // ------------------------------------------------------------------------------ combos
    /** ItemBind:288 / rdSearchByName_CheckedChanged:958 — DropDownBind.BindDDL(..., ZeroIndex: true). */
    function bindItems(mode, keepId) {
        var el = $('cmbitem');
        var byName = $('rdSearchByName').checked;
        var disp = byName ? 'ItemName' : 'ItemCode';
        var items = (look && look.items) || [];
        if (!items.length) {                                                                    // :316-320
            el.innerHTML = '';
            return;
        }
        var h = '<option value="0">...Select Any Value...</option>';
        items.forEach(function (r) { h += '<option value="' + esc(r.Id) + '">' + esc(r[disp]) + '</option>'; });
        el.innerHTML = h;
        if (mode === 'search') {
            el.value = String(keepId || 0);                                                     // :977 cmbitem.Value = ItemId
            if (!hasOption(el, keepId || 0)) el.selectedIndex = -1;
        } else {
            /* :314 SetComboValue(cmbitem, "Id", Id, dtitem): 0 or not found → Text = "" */
            if (keepId && hasOption(el, keepId)) el.value = String(keepId); else el.selectedIndex = -1;
        }
    }

    /** ItemConditionBindFromGlobal:328 — BindAndRetainSelection(..., insertDefaultRow: true). */
    function bindConditions() {
        var el = $('CmbItemcondition');
        var prev = selVal('CmbItemcondition');
        var rows = (look && look.itemConditions) || [];
        if (!rows.length) { el.innerHTML = ''; return; }
        var h = '<option value="0">-- Select --</option>';
        rows.forEach(function (r) { h += '<option value="' + esc(r.Id) + '">' + esc(r.Description) + '</option>'; });
        el.innerHTML = h;
        /* RetainComboSelection: previous in the source → it; otherwise ActivateRow → row 0. */
        if (prev && rows.some(function (r) { return C.intOf(r.Id) === prev; })) el.value = String(prev);
        else el.selectedIndex = 0;
    }

    /** SupplierFrombind:366 — both combos retain cmbSupplierFrom's previous value (:385-386). */
    function bindParties() {
        var rows = (look && look.parties) || [];
        if (!rows.length) return;                                                               // :371
        var prev = selVal('cmbSupplierFrom');
        ['cmbSupplierFrom', 'cmbSupplierTo'].forEach(function (id) {
            var el = $(id), h = '';
            rows.forEach(function (r) { h += '<option value="' + esc(r.Id) + '">' + esc(r.CompanyName) + '</option>'; });
            el.innerHTML = h;
            if (prev && rows.some(function (r) { return C.intOf(r.Id) === prev; })) el.value = String(prev);
            else el.selectedIndex = -1;                                                         // ActivateRow: false → Text = ""
        });
    }

    /** cmbitem_Leave:395 — the item's UOMs from the global schedule; left untouched when it has none. */
    function itemLeave() {
        var itemId = selVal('cmbitem');
        var list = uoms.filter(function (u) { return C.intOf(u.ItemId) === itemId; });
        if (!list.length) return;
        var el = $('cmbItemUom'), h = '';
        list.forEach(function (u) { h += '<option value="' + esc(u.Id) + '">' + esc(u.UOMCode) + '</option>'; });
        el.innerHTML = h;
        el.selectedIndex = -1;                                                                  // BindDDL ZeroIndex false: no value
    }

    function applyGlobals(g) {
        look.items = g.items || [];
        look.itemConditions = g.itemConditions || [];
        look.parties = g.parties || [];
        uoms = g.uoms || [];
    }

    // ------------------------------------------------------------------------------ form grid
    function renderForm() {
        var t = $('grdPartyPmTransfer');
        var thead = t.querySelector('thead'), tbody = t.querySelector('tbody'), tfoot = t.querySelector('tfoot');
        if (!gridStructured) { thead.innerHTML = ''; tbody.innerHTML = ''; tfoot.innerHTML = ''; return; }
        var cols = gridSettingsApplied ? FORM_COLS : FORM_ALL_COLS;
        thead.innerHTML = '<tr>' + cols.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('') + '</tr>';
        tbody.innerHTML = table.map(function (r, i) {
            return '<tr data-i="' + i + '">' + cols.map(function (c) {
                if (c === 'Qty') return '<td class="num">' + esc(fmt(r.Qty, 3)) + '</td>';
                return '<td>' + esc(r[c] === null || r[c] === undefined ? '' : r[c]) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        if (gridSettingsApplied) {
            var sum = table.reduce(function (a, r) { return a + C.num(r.Qty); }, 0);
            tfoot.innerHTML = '<tr>' + cols.map(function (c) {
                return c === 'Qty' ? '<td class="num">' + esc(fmt(sum, 3)) + '</td>' : '<td></td>';
            }).join('') + '</tr>';
        } else {
            tfoot.innerHTML = '';
        }
        tbody.querySelectorAll('tr[data-i]').forEach(function (tr) {
            tr.addEventListener('dblclick', function () { gridDoubleClick(parseInt(tr.getAttribute('data-i'), 10)); });
        });
    }

    /** grdPartyPmTransfer_DoubleClick:1098. */
    function gridDoubleClick(i) {
        var r = table[i];
        if (!r) return;
        updateDetailIndex = i;
        setCombo('cmbitem', C.intOf(r.ItemId));
        itemLeave();                                                                            // :1110
        setCombo('cmbItemUom', C.intOf(r.PackUomId));
        $('txtItemQty').value = String(r.Qty === null || r.Qty === undefined ? '' : r.Qty);
        setCombo('cmbSupplierFrom', C.intOf(r.SupplierFromId));
        setCombo('cmbSupplierTo', C.intOf(r.SupplierToId));
        $('txtRemarksDetail').value = r.Remarks == null ? '' : String(r.Remarks);
        /* CmbItemcondition is NOT set from the row (desktop behaviour 6). */
        show('BtnAdd', false);
        show('btnUpdateDetail', true);
        show('btnCancelUpdateDetial', true);
        $('cmbitem').focus();
    }

    // ------------------------------------------------------------------------------ detail entry
    /** FormValidationDetail:499 — same order, same messages. */
    function validateDetail() {
        if ($('cmbitem').selectedIndex < 0 || selVal('cmbitem') === 0) { msg('ItemName Field is Required'); $('cmbitem').focus(); return false; }
        if ($('cmbItemUom').selectedIndex < 0) { msg('ItemUOM Field is Required'); $('cmbItemUom').focus(); return false; }
        if (C.num(val('txtItemQty').trim()) === 0) { msg('ItemQty Field is Required'); $('txtItemQty').focus(); return false; }
        if ($('CmbItemcondition').selectedIndex < 0 || selVal('CmbItemcondition') === 0) { msg('Item Condition Field is Required'); $('CmbItemcondition').focus(); return false; }
        if ($('cmbSupplierFrom').selectedIndex < 0 || selVal('cmbSupplierFrom') === 0) { msg('SupplierFrom value Field is Required'); $('cmbSupplierFrom').focus(); return false; }
        if ($('cmbSupplierTo').selectedIndex < 0 || selVal('cmbSupplierTo') === 0) { msg('SupplierTo Field is Required'); $('cmbSupplierTo').focus(); return false; }
        if (selVal('cmbSupplierFrom') === selVal('cmbSupplierTo')) { msg("Supplier From && Supplier To can't be same"); $('cmbSupplierTo').focus(); return false; }
        return true;
    }

    function selectedItem() {
        var id = selVal('cmbitem');
        return ((look && look.items) || []).find(function (r) { return C.intOf(r.Id) === id; }) || {};
    }

    /** Add_Click:425. */
    function add() {
        if (!validateDetail()) return;
        var it = selectedItem();
        table.push({
            Id: 0,
            ItemId: selVal('cmbitem'),
            ItemCode: it.ItemCode || '',
            ItemName: it.ItemName || '',
            PackUomId: selVal('cmbItemUom'),
            PackUom: selText('cmbItemUom'),
            ItemConditionId: selVal('CmbItemcondition'),
            ItemCondition: selText('CmbItemcondition'),
            Qty: val('txtItemQty').trim(),
            Remarks: val('txtRemarksDetail').trim(),
            SupplierFromId: selVal('cmbSupplierFrom'),
            SupplierFrom: selText('cmbSupplierFrom'),
            SupplierToId: selVal('cmbSupplierTo'),
            SupplierTo: selText('cmbSupplierTo')
        });
        gridStructured = true;
        gridSettingsApplied = true;                                                             // :432-434
        renderForm();
        resetDetail(true);
    }

    /** btnUpdateDetail_Click:1052 — ItemConditionId / ItemCondition are not written back. */
    function updateDetail() {
        if (!validateDetail()) return;
        var r = table[updateDetailIndex];
        if (r) {
            var it = selectedItem();
            r.ItemId = selVal('cmbitem');
            r.ItemCode = it.ItemCode || '';
            r.ItemName = it.ItemName || '';
            r.PackUomId = selVal('cmbItemUom');
            r.PackUom = selText('cmbItemUom');
            r.Qty = val('txtItemQty');
            r.SupplierFromId = selVal('cmbSupplierFrom');
            r.SupplierFrom = selText('cmbSupplierFrom');
            r.SupplierToId = selVal('cmbSupplierTo');
            r.SupplierTo = selText('cmbSupplierTo');
            r.Remarks = val('txtRemarksDetail');
        }
        renderForm();
        show('BtnAdd', true);
        show('btnUpdateDetail', false);
        show('btnCancelUpdateDetial', false);
        resetDetail(true);
    }

    /** btnCancelUpdateDetial_Click:1083. */
    function cancelUpdateDetail() {
        show('BtnAdd', true);
        show('btnUpdateDetail', false);
        show('btnCancelUpdateDetial', false);
        resetDetail(true);
    }

    /** ResetDetai:444 — Item Condition is left as it is. */
    function resetDetail(focus) {
        clearCombo('cmbItemUom');
        $('txtItemQty').value = '';
        $('txtRemarksDetail').value = '';
        clearCombo('cmbSupplierFrom');
        clearCombo('cmbitem');
        clearCombo('cmbSupplierTo');
        if (focus) $('cmbitem').focus();
    }

    // ------------------------------------------------------------------------------ header
    /** Reset:796 — Doc Date and RecId are not touched. */
    function reset() {
        $('txtRemarksHeader').value = '';
        table = [];
        gridStructured = false;                                                                 // :805 ClearStructure
        renderForm();
        show('btnSave', true);
        show('btnUpdate', false);
        resetDetail(false);
        show('BtnAdd', true);
        show('btnUpdateDetail', false);
        show('btnCancelUpdateDetial', false);
        return C.getJson(API + '/next-doc-no').then(function (r) {                              // :802 GenerateCode
            $('txtVoucherCode').value = String(r.docNo);
        }).catch(function (e) { msg(e.message); });
    }

    /** ReadById:858. */
    function readById(id) {
        recId = C.intOf(id);                                                                    // :865
        return C.getJson(API + '/' + recId).then(function (d) {
            tab('tabForm');                                                                     // :869
            $('txtVoucherCode').value = String(d.DocNo);
            $('datDocDate').value = C.isoDay(d.DocDate);
            $('txtRemarksHeader').value = d.RemarksHeader || '';
            table = (d.rows || []).map(function (r) { return Object.assign({}, r); });
            gridStructured = true;
            gridSettingsApplied = true;
            renderForm();
            show('btnSave', false);
            show('btnUpdate', true);
        }).catch(function (e) { msg(e.message); });
    }

    /** Insert():546. */
    function insert() {
        if (!table.length) { msg('Please Insert Any Record In Grid'); return; }                 // :554
        if (C.intOf(val('txtVoucherCode').trim()) === 0) { msg('DocNo field is required'); $('txtVoucherCode').focus(); return; } // :490
        if (!confirm(recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var body = {
            Id: recId,
            DocDate: val('datDocDate'),
            RemarksHeader: val('txtRemarksHeader'),
            rows: table.map(function (r) {
                return {
                    Id: C.intOf(r.Id), ItemId: C.intOf(r.ItemId), ItemCode: r.ItemCode, ItemName: r.ItemName,
                    PackUomId: C.intOf(r.PackUomId), PackUom: r.PackUom,
                    ItemConditionId: C.intOf(r.ItemConditionId), ItemCondition: r.ItemCondition,
                    Qty: String(r.Qty === null || r.Qty === undefined ? '' : r.Qty), Remarks: r.Remarks == null ? '' : String(r.Remarks),
                    SupplierFromId: C.intOf(r.SupplierFromId), SupplierFrom: r.SupplierFrom,
                    SupplierToId: C.intOf(r.SupplierToId), SupplierTo: r.SupplierTo
                };
            })
        };
        C.postJson(API + '/save', body).then(function (res) {
            msg(res.message);                                                                   // :609 / :614
            return reset().then(function () {                                                   // :616
                if ($('ChkBox').checked) printSlip(recId);                                      // :617-619 (RecId, see behaviour 2)
            });
        }).catch(function (e) { msg(e.message); });
    }

    function printSlip(id) {
        C.printSlip(API + '/' + C.intOf(id) + '/slip', '419 - Party To Party Packing Material Slip');
    }

    // ------------------------------------------------------------------------------ history
    function historyMode() {
        if ($('rdpqtydoc').checked) return 'doc';
        if ($('rdpqtyentry').checked) return 'entry';
        if ($('rdpqtymodify').checked) return 'modify';
        if ($('rdapproved').checked) return 'approved';
        return 'none';
    }

    /** FormHistorybind:653. */
    function showHistory() {
        var q = {
            mode: historyMode(),
            fromDate: $('chkFromDateHistory').checked ? val('FromDateHistory') : '',
            toDate: $('chkToDateHistory').checked ? val('ToDateHistory') : '',
            fromDocNo: val('txtFromDocNoHistory'),
            toDocNo: val('txtToDocNoHistory')
        };
        C.getJson(API + '/history' + C.qs(q)).then(function (rows) {
            historyRows = rows || [];
            renderHistory();
            if (historyRows.length) selectHistory(0);
        }).catch(function (e) { msg(e.message); });
    }

    var HIST_COLS = [
        { key: 'Edit' }, { key: 'Print' },
        { key: 'DocNo', num: true }, { key: 'DocDate', date: true }, { key: 'EntryUser' },
        { key: 'EntryDate', stamp: true }, { key: 'ModifyUser' }, { key: 'ModifyDate', stamp: true },
        { key: 'ApprovedUser' }, { key: 'ApprovedDate', stamp: true }, { key: 'Remarks' }
    ];

    /**
     * Gridhistorysetting:743 — Edit / Print buttons first, Id hidden. No total row: DocNo is an int
     * column and GridEX_Helper.GridColumnSettings only formats/sums float/double/decimal columns
     * (IsColumnNumeric); int columns are centred and unformatted.
     */
    function renderHistory() {
        var t = $('grdhistory');
        var thead = t.querySelector('thead'), tbody = t.querySelector('tbody'), tfoot = t.querySelector('tfoot');
        historyCurrent = -1;
        if (!historyRows.length) { thead.innerHTML = ''; tbody.innerHTML = ''; tfoot.innerHTML = ''; return; }   // :734 ClearStructure
        thead.innerHTML = '<tr>' + HIST_COLS.map(function (c) { return '<th>' + esc(c.key) + '</th>'; }).join('') + '</tr>';
        tbody.innerHTML = historyRows.map(function (r, i) {
            return '<tr data-i="' + i + '">' + HIST_COLS.map(function (c) {
                if (c.key === 'Edit') return '<td><button type="button" class="pp-cellbtn" data-act="edit">Edit</button></td>';
                if (c.key === 'Print') return '<td><button type="button" class="pp-cellbtn" data-act="print">Print</button></td>';
                if (c.key === 'DocNo') return '<td style="text-align:center"><span class="pp-doclink" data-act="open">' + esc(r.DocNo == null ? '' : r.DocNo) + '</span></td>';
                if (c.date) return '<td>' + esc(C.gridDate(r[c.key])) + '</td>';
                if (c.stamp) return '<td>' + esc(r[c.key] ? C.gridDateTime(r[c.key], true) : '') + '</td>';
                return '<td>' + esc(r[c.key] == null ? '' : r[c.key]) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        tfoot.innerHTML = '';
        tbody.querySelectorAll('tr[data-i]').forEach(function (tr) {
            var i = parseInt(tr.getAttribute('data-i'), 10);
            tr.addEventListener('click', function (e) {
                var b = e.target.closest('[data-act]');
                selectHistory(i);
                if (!b) return;
                var id = historyRows[i].Id;
                var act = b.getAttribute('data-act');
                if (act === 'edit') {                                                           // :1147 Reset(); ReadById
                    reset().then(function () { return readById(id); });
                } else if (act === 'print') {                                                   // :1155
                    printSlip(id);
                } else if (act === 'open') {                                                    // the DoubleClick path
                    readById(id);
                }
            });
            tr.addEventListener('dblclick', function (e) {                                      // grdhistory_DoubleClick:845
                if (e.target.closest('[data-act]')) return;
                readById(historyRows[i].Id);
            });
        });
    }

    /** grdhistory_SelectionChanged:987 → BindHistoryDetailGrid:1005. */
    function selectHistory(i) {
        if (i === historyCurrent) return;
        historyCurrent = i;
        $('grdhistory').querySelectorAll('tbody tr').forEach(function (tr) {
            tr.classList.toggle('is-selected', parseInt(tr.getAttribute('data-i'), 10) === i);
        });
        var r = historyRows[i];
        if (!r) return;
        C.getJson(API + '/' + C.intOf(r.Id)).then(function (d) { renderDetail(d.rows || []); })
            .catch(function (e) { msg(e.message); });
    }

    var DETAIL_COLS = ['ItemCode', 'ItemName', 'PackUom', 'ItemCondition', 'Qty', 'SupplierFrom', 'SupplierTo', 'Remarks'];

    function renderDetail(rows) {
        var t = $('grddetail');
        var thead = t.querySelector('thead'), tbody = t.querySelector('tbody'), tfoot = t.querySelector('tfoot');
        if (rows === null) { thead.innerHTML = ''; tbody.innerHTML = ''; tfoot.innerHTML = ''; return; }
        thead.innerHTML = '<tr>' + DETAIL_COLS.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('') + '</tr>';
        tbody.innerHTML = rows.map(function (r) {
            return '<tr>' + DETAIL_COLS.map(function (c) {
                if (c === 'Qty') return '<td class="num">' + esc(fmt(r.Qty, 2)) + '</td>';     // "#,##0.##"
                return '<td>' + esc(r[c] == null ? '' : r[c]) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        var sum = rows.reduce(function (a, r) { return a + C.num(r.Qty); }, 0);
        tfoot.innerHTML = '<tr>' + DETAIL_COLS.map(function (c) {
            return c === 'Qty' ? '<td class="num">' + esc(fmt(sum, 2)) + '</td>' : '<td></td>';
        }).join('') + '</tr>';
    }

    /**
     * btnNewHistory_Click:940 — From = today − 3 (not the configured days). Assigning .Value to a
     * ShowCheckBox DateTimePicker sends DTM_SETSYSTEMTIME/GDT_VALID, which ticks its check box, so
     * both date filters come back checked.
     */
    function resetHistory() {
        var today = C.today();
        $('FromDateHistory').value = addDays(today, -3);
        $('ToDateHistory').value = today;
        $('chkFromDateHistory').checked = true;
        $('chkToDateHistory').checked = true;
        $('txtFromDocNoHistory').value = '';
        $('txtToDocNoHistory').value = '';
        historyRows = [];
        renderHistory();
        renderDetail(null);
        $('rdpqtydoc').checked = true;
    }

    // ------------------------------------------------------------------------------ tabs
    function tab(id) {
        ['tabForm', 'tabHistory'].forEach(function (t) {
            $(t).classList.toggle('is-active', t === id);
            var h = document.querySelector('.win-tab[data-tab="' + t + '"]');
            if (h) h.classList.toggle('is-active', t === id);
        });
    }

    // ------------------------------------------------------------------------------ key filters
    /** CommonServices.OnlytextdecimelFunction — digits, one '.', control keys. */
    function decimalOnly(e) {
        if (e.ctrlKey || e.metaKey || e.key.length !== 1) return;
        if (/[0-9]/.test(e.key)) return;
        if (e.key === '.' && e.target.value.indexOf('.') < 0) return;
        e.preventDefault();
    }
    /** CommonServices.OnlytextNumberFunction — digits and control keys. */
    function digitsOnly(e) {
        if (e.ctrlKey || e.metaKey || e.key.length !== 1) return;
        if (!/[0-9]/.test(e.key)) e.preventDefault();
    }

    // ------------------------------------------------------------------------------ load
    /** PartyToPartyPackingMaterialTransfer_Load:222. */
    function load() {
        var today = C.today();
        $('datDocDate').value = today;                                                          // DateTimePicker default: now
        $('ToDateHistory').value = today;
        $('txtItemQty').addEventListener('keydown', decimalOnly);
        $('txtFromDocNoHistory').addEventListener('keydown', digitsOnly);
        $('txtToDocNoHistory').addEventListener('keydown', digitsOnly);
        C.getJson(API + '/lookups').then(function (d) {
            look = d;
            uoms = d.uoms || [];
            var r = d.rights || {};
            $('btnSave').disabled = !r.save;                                                     // :230
            $('btnprint').disabled = !r.print;                                                   // :231
            $('btnUpdate').disabled = !r.update;                                                 // :232
            if (d.itemSearchByCode) $('rdSearchByCode').checked = true; else $('rdSearchByName').checked = true; // :246-253
            gridStructured = true;                                                              // :271-272 RetrieveStructure
            gridSettingsApplied = false;
            renderForm();
            bindItems('bind', 0);                                                               // :273
            bindConditions();                                                                   // :274
            bindParties();                                                                      // :275
            $('txtVoucherCode').value = String(d.docNo);                                        // :276
            show('btnSave', true);
            show('btnUpdate', false);
            var days = C.intOf(d.defaultDaysToLessFromHistoryFromDate);                         // :279-280
            $('FromDateHistory').value = addDays(d.today || today, days > 0 ? -days : -3);
        }).catch(function (e) { msg(e.message); });
    }

    window.P2PPm = {
        newClick: function () { reset(); },                                                     // btnNew_Click:824
        refresh: function () {                                                                  // btnRefresh_Click:829
            C.getJson(API + '/globals').then(function (g) {
                applyGlobals(g);
                bindItems('bind', selVal('cmbitem'));
                bindConditions();
                bindParties();
            }).catch(function (e) { msg(e.message); });
        },
        save: function () { recId = 0; insert(); },                                             // btnSave_Click:628
        update: function () { insert(); },                                                      // btnUpdate_Click:641
        print: function () { printSlip(recId); },                                               // btnprint_Click:1423
        add: add,
        updateDetail: updateDetail,
        cancelUpdateDetail: cancelUpdateDetail,
        itemLeave: itemLeave,
        searchModeChanged: function () {                                                        // rdSearchByName_CheckedChanged:958
            if (!look || !(look.items || []).length) return;
            var id = selVal('cmbitem');
            bindItems('search', id > 0 ? id : 0);
            $('cmbitem').focus();
        },
        showHistory: showHistory,
        resetHistory: resetHistory,
        tab: tab
    };

    document.addEventListener('DOMContentLoaded', load);
})();
