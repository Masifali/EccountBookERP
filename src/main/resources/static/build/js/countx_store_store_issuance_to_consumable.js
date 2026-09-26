/* ============================================================================================
 * Screen 347 "Store Issuance To Consumable Store" — Architecture.WinApp.StoreManagement.
 * frmStoreIssuanceToCosumableStore (ScreenName = base.Name "frmStoreIssuanceToCosumableStore"),
 * DocumentTypeId 1616, with its loader LoadDepRequestToConsumableStore (DocumentTypeId 1615).
 *
 *   frmGSIssuance_Load:168        rights, BranchFill, ProjectFill, GenerateDocNo  (init)
 *   btnRefresh_Click:547          BranchFill, ProjectFill, GenerateDocNo          (refresh)
 *   btnnew_Click / formReset:566  (newClick / formReset)
 *   Insert():296                  precheck (grid, voucher, FormValidation) → confirm → save
 *   ReadById:471                  (readById)
 *   grdDetail_ColumnButtonClick   "X" → lstRemoveRecord (never cleared, service note 4)
 *   btnLoadRequest_Click:936      a NEW loader per click (fresh filters) → LoadInGridDetail:952
 *   tabControl SelectedIndexChanged:985  History tab → HistoryFill(50); Load All → HistoryFill()
 *   DataGridHistory SelectionChanged:1128 / DoubleClick:1078 / ColumnButtonClick:1094
 * ============================================================================================ */
(function () {
    'use strict';
    var C = window.StoreCommon, $id = C.$id, esc = C.esc;
    var api = '/api/store/issuance-to-consumable-store';
    var DEFAULT_TEXT = '...Select Any Value...';          // DropDownBind.BindDDLNew default row

    var look = { rights: {}, branches: [], projects: [], financialYearStart: null };
    var recId = 0;                 // RECID
    var rows = [];                 // dtgrddetail
    var removed = [];              // lstRemoveRecord — a field initialised in the constructor only
    var historyRows = [], historySel = -1;
    var loader = null;             // the open LoadDepRequestToConsumableStore instance

    // ------------------------------------------------------------------------------ helpers

    function num(v) {
        var s = String(v === null || v === undefined ? '' : v).replace(/,/g, '').trim();
        if (!s) return null;
        var n = Number(s);
        return isNaN(n) ? null : n;
    }
    function fmt(v, places) {
        var n = Number(v) || 0;
        return n.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: places });
    }
    function addDays(iso, days) {
        var p = iso.split('-'), d = new Date(+p[0], +p[1] - 1, +p[2]);
        d.setDate(d.getDate() + days);
        return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0');
    }
    function selText(el) {
        if (!el || el.selectedIndex < 0 || !el.options.length) return '';
        return el.options[el.selectedIndex].text;
    }
    function fail(e) { alert(e && e.message ? e.message : String(e)); }

    /** BindDDLNew(dt, cmb, "Id", <name>, <caption>, true) then Rows[1].Activate() — only when rows exist. */
    function bindDdlNew(id, list) {
        var el = $id(id);
        if (!list || !list.length) return;                      // "if (dt.Rows.Count > 0)"
        var h = '<option value="0">' + esc(DEFAULT_TEXT) + '</option>';
        list.forEach(function (r) { h += '<option value="' + esc(r.Id) + '">' + esc(r.Name) + '</option>'; });
        el.innerHTML = h;
        el.selectedIndex = 1;                                   // Rows[1].Activate()
    }

    // ------------------------------------------------------------------------------ load

    function init() {
        $id('txtDocdate').value = C.today();
        renderGrid();
        C.getJson(api + '/lookups').then(function (d) {
            look = d;
            var r = d.rights || {};
            /* :176-181 — set once, in Form_Load only. */
            $id('btnsave').disabled = !r.save;
            $id('btnUpdate').disabled = !r.update;
            $id('Print').disabled = !r.print;
            $id('checkBox1').checked = !!r.print;
            $id('checkBox1').disabled = !r.print;
            bindDdlNew('cmbBranch', d.branches);
            bindDdlNew('cmbProject', d.projects);
            if (d.docNo > 0) $id('txtDocNo').value = d.docNo;   // GenerateDocNo: only when code > 0
        }).catch(fail);
        document.addEventListener('keydown', function (e) {
            if (e.key === 'Escape' && loader) { e.preventDefault(); loaderClose(true); }
        });
    }

    function refresh() {
        C.getJson(api + '/refresh').then(function (d) {
            bindDdlNew('cmbBranch', d.branches);
            bindDdlNew('cmbProject', d.projects);
            if (d.docNo > 0) $id('txtDocNo').value = d.docNo;
        }).catch(fail);
    }

    function generateDocNo() {
        return C.getJson(api + '/doc-no').then(function (d) {
            if (d && d.docNo > 0) $id('txtDocNo').value = d.docNo;
        }).catch(fail);
    }

    /** formReset:566 — Branch / Project / Preview / lstRemoveRecord are NOT touched. */
    function formReset() {
        recId = 0;
        $id('btnsave').classList.remove('is-hidden');
        $id('btnUpdate').classList.add('is-hidden');
        rows = [];
        renderGrid();
        $id('txtDocdate').value = C.today();                    // txtDocdate.Value = DateTime.Now
        $id('txtRemarks').value = '';
        $id('txtDocNo').focus();
        generateDocNo();
    }

    // ------------------------------------------------------------------------------ grid

    /* Visible columns after DetailGridSettings:666 — "X" at position 0, then dtgrddetail order. */
    var COLS = [
        { key: 'DepartmentRequestNo', cap: 'Request No', cls: 'ctr' },
        { key: 'DepartmentFrom', cap: 'DepartmentFrom' },
        { key: 'DepartmentTo', cap: 'DepartmentTo' },
        { key: 'WorkOrder', cap: 'WorkOrder' },
        { key: 'Item', cap: 'Item' },
        { key: 'ItemCode', cap: 'ItemCode' },
        { key: 'Unit', cap: 'Unit' },
        { key: 'Asset', cap: 'Asset' },
        { key: 'IssueQty', cap: 'IssueQty', edit: 'qty' },
        { key: 'Remarks', cap: 'Remarks', edit: 'rem' }
    ];

    function renderGrid() {
        var t = $id('grdDetail');
        t.tHead.innerHTML = '<tr><th style="width:30px">X</th>' + COLS.map(function (c) { return '<th>' + esc(c.cap) + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = rows.map(function (r, i) {
            return '<tr data-i="' + i + '"><td class="ctr"><button type="button" class="cx-x" data-x="' + i + '">X</button></td>' +
                COLS.map(function (c) {
                    var v = r[c.key];
                    if (c.edit === 'qty') return '<td class="num"><input type="text" class="qty" data-q="' + i + '" value="' + esc(v === null || v === undefined ? '' : v) + '"></td>';
                    if (c.edit === 'rem') return '<td><input type="text" class="rem" data-r="' + i + '" value="' + esc(v) + '"></td>';
                    return '<td' + (c.cls ? ' class="' + c.cls + '"' : '') + '>' + esc(v) + '</td>';
                }).join('') + '</tr>';
        }).join('');
        t.tBodies[0].querySelectorAll('[data-x]').forEach(function (b) {
            b.onclick = function () { removeRow(parseInt(b.getAttribute('data-x'), 10)); };
        });
        t.tBodies[0].querySelectorAll('[data-q]').forEach(function (inp) {
            inp.onchange = function () { rows[parseInt(inp.getAttribute('data-q'), 10)].IssueQty = num(inp.value); total(); };
        });
        t.tBodies[0].querySelectorAll('[data-r]').forEach(function (inp) {
            inp.oninput = function () { rows[parseInt(inp.getAttribute('data-r'), 10)].Remarks = inp.value; };
        });
        total();
    }

    /** IssueQty AggregateFunction Sum, "#,##0.##" (:711-714). */
    function total() {
        var s = rows.reduce(function (a, r) { return a + (Number(r.IssueQty) || 0); }, 0);
        $id('grdDetail').tFoot.innerHTML = rows.length
            ? '<tr><td></td>' + COLS.map(function (c) { return '<td class="num">' + (c.key === 'IssueQty' ? fmt(s, 2) : '') + '</td>'; }).join('') + '</tr>'
            : '';
    }

    /** grdDetail_ColumnButtonClick:760. */
    function removeRow(i) {
        var r = rows[i];
        if (!r) return;
        if ((r.ItemId | 0) > 0) {
            if (!confirm('Are you sure to Delete?')) return;
            removed.push(Object.assign({}, r));                 // lstRemoveRecord.Add(d1)
        }
        rows.splice(i, 1);
        renderGrid();
    }

    // ------------------------------------------------------------------------------ save

    function body() {
        return {
            Id: recId,
            DocDate: $id('txtDocdate').value,
            Remarks: $id('txtRemarks').value,
            BranchesId: parseInt($id('cmbBranch').value || '0', 10) || 0,
            BranchText: selText($id('cmbBranch')),
            ProjectsId: parseInt($id('cmbProject').value || '0', 10) || 0,
            ProjectText: selText($id('cmbProject')),
            rows: rows.map(rowDto),
            removed: removed.map(rowDto)
        };
    }
    function rowDto(r) {
        return {
            Id: r.Id | 0, DepartmentRequestId: r.DepartmentRequestId | 0,
            DepartmentRequestDetailId: r.DepartmentRequestDetailId | 0, DepartmentRequestNo: r.DepartmentRequestNo | 0,
            LocationId: r.LocationId | 0, DepartmentFromId: r.DepartmentFromId | 0, DepartmentFrom: r.DepartmentFrom,
            DepartmentToId: r.DepartmentToId | 0, DepartmentTo: r.DepartmentTo, WorkOrderId: r.WorkOrderId | 0,
            WorkOrder: r.WorkOrder, ItemId: r.ItemId | 0, Item: r.Item, ItemCode: r.ItemCode, UnitId: r.UnitId | 0,
            Unit: r.Unit, AssetId: r.AssetId | 0, Asset: r.Asset,
            IssueQty: r.IssueQty === null || r.IssueQty === undefined ? null : Number(r.IssueQty),
            Remarks: r.Remarks === null || r.Remarks === undefined ? '' : String(r.Remarks)
        };
    }

    function save() { recId = 0; insert(); }             // btnsave_Click:446
    function update() { insert(); }                       // btnUpdate_Click:459

    function insert() {
        var b = body();
        var printId = recId;                              // PrintMethod(RECID) after the save (:436)
        C.postJson(api + '/precheck', b).then(function () {
            if (!confirm(recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return null;
            return C.postJson(api + '/save', b).then(function (res) {
                alert(res.message);
                if ($id('checkBox1').checked) printMethod(printId);
                formReset();
            });
        }).catch(fail);
    }

    // ------------------------------------------------------------------------------ print

    /** PrintMethod → CommonServices.StoreIssuanceSlip1616 (data rows; the .rpt layout is not ported). */
    function printMethod(id) {
        if (!id) { alert('Record Id Not Found'); return; }
        C.printSlip(api + '/' + id + '/slip', '1616-StoreIssuanceToConsumableStore_Slip');
    }
    function print() { printMethod(recId); }

    // ------------------------------------------------------------------------------ read

    function mapRow(r) {
        return {
            Id: r.Id, DepartmentRequestId: r.DepartmentRequestId, DepartmentRequestDetailId: r.DepartmentRequestDetailId,
            DepartmentRequestNo: r.DepartmentRequestNo, LocationId: r.LocationId, DepartmentFromId: r.DepartmentFromId,
            DepartmentFrom: r.DepartmentFrom, DepartmentToId: r.DepartmentToId, DepartmentTo: r.DepartmentTo,
            WorkOrderId: r.WorkOrderId, WorkOrder: r.WorkOrder, ItemId: r.ItemId, Item: r.Item, ItemCode: r.ItemCode,
            UnitId: r.UnitId, Unit: r.Unit, AssetId: r.AssetId, Asset: r.Asset, IssueQty: r.IssueQty, Remarks: r.Remarks
        };
    }

    /** ReadById:471. */
    function readById(id) {
        C.getJson(api + '/' + id).then(function (h) {
            recId = h.Id;
            tab('tabForm', true);                             // SelectedIndex = 0
            $id('btnsave').classList.add('is-hidden');
            $id('btnUpdate').classList.remove('is-hidden');
            $id('txtDocdate').value = C.isoDay(h.DocDate);
            $id('txtDocNo').value = h.DocNo;
            $id('cmbBranch').value = String(h.BranchesId);
            $id('cmbProject').value = String(h.ProjectsId);
            $id('txtRemarks').value = h.Remarks || '';
            rows = (h.rows || []).map(mapRow);
            renderGrid();
        }).catch(function (e) {
            /* :501-506 — GetByID returned null: clear the grid and formReset, no message. */
            if (e && e.message === 'Record Not Found') { rows = []; formReset(); return; }
            fail(e);
        });
    }

    // ------------------------------------------------------------------------------ tabs / history

    function tab(name, silent) {
        /* SelectedIndexChanged:985 fires only when the tab actually changes. */
        var changed = !$id(name).classList.contains('is-active');
        document.querySelectorAll('.win-tab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-tab') === name); });
        $id('tabForm').classList.toggle('is-active', name === 'tabForm');
        $id('tabHistory').classList.toggle('is-active', name === 'tabHistory');
        if (name === 'tabHistory' && !silent && changed) historyFill(50);   // :989
    }
    function loadAll() { historyFill(0); }

    /** HistoryFill:1012. */
    function historyFill(noOfRecords) {
        C.getJson(api + '/history' + C.qs({ noOfRecords: noOfRecords || '' })).then(function (list) {
            if (!list || !list.length) return;                 // :1017 grid left as it was
            historyRows = list;
            historySel = -1;
            renderHistory();
        }).catch(fail);
    }

    function renderHistory() {
        var r = look.rights || {};
        var cols = [];
        if (r.print) cols.push({ key: 'Print', btn: 'Print' });           // Position 0
        if (r.update) cols.push({ key: 'Edit', btn: 'Edit' });            // Position 0 / 1 when both
        if (r.print && r.update) cols = [{ key: 'Print', btn: 'Print' }, { key: 'Edit', btn: 'Edit' }];
        cols = cols.concat([
            { key: 'RecordNo', cls: 'ctr' }, { key: 'DocNo', cls: 'ctr', open: true }, { key: 'DocDate', date: true },
            { key: 'Remarks' }, { key: 'NoOfAttachments', link: true }
        ]);
        var t = $id('DataGridHistory');
        t.tHead.innerHTML = '<tr>' + cols.map(function (c) { return '<th>' + esc(c.key) + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = historyRows.map(function (h, i) {
            return '<tr data-i="' + i + '"' + (i === historySel ? ' class="is-selected"' : '') + '>' + cols.map(function (c) {
                if (c.btn) return '<td class="ctr"><button type="button" class="cx-link" data-b="' + c.btn + '" data-i="' + i + '">' + c.btn + '</button></td>';
                if (c.date) return '<td>' + esc(C.gridDate(h.DocDate)) + '</td>';
                if (c.link) return '<td class="lnk" data-att="' + i + '" title="Attachments are not ported to the web page">' + esc(h.NoOfAttachments) + '</td>';
                if (c.open && r.update) return '<td class="lnk" data-open="' + i + '">' + esc(h[c.key]) + '</td>';
                return '<td' + (c.cls ? ' class="' + c.cls + '"' : '') + '>' + esc(h[c.key]) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        t.tBodies[0].querySelectorAll('tr[data-i]').forEach(function (tr) {
            var i = parseInt(tr.getAttribute('data-i'), 10);
            tr.onclick = function () { selectHistory(i); };
            tr.ondblclick = function () { if (r.update) readById(historyRows[i].Id); };   // :1078
        });
        t.tBodies[0].querySelectorAll('[data-b]').forEach(function (b) {
            b.onclick = function (e) {
                e.stopPropagation();
                var id = historyRows[parseInt(b.getAttribute('data-i'), 10)].Id;
                if (b.getAttribute('data-b') === 'Print') printMethod(id);            // :1098
                else readById(id);                                                     // :1102
            };
        });
        t.tBodies[0].querySelectorAll('[data-open]').forEach(function (td) {
            td.onclick = function (e) { e.stopPropagation(); readById(historyRows[parseInt(td.getAttribute('data-open'), 10)].Id); };
        });
        t.tBodies[0].querySelectorAll('[data-att]').forEach(function (td) {
            td.onclick = function (e) { e.stopPropagation(); alert('Attachments are not ported to the web page.'); };
        });
    }

    /** DataGridHistory_SelectionChanged:1128 — GetByID → the detail grid. */
    function selectHistory(i) {
        if (historySel === i) return;
        historySel = i;
        $id('DataGridHistory').tBodies[0].querySelectorAll('tr').forEach(function (tr, k) { tr.classList.toggle('is-selected', k === i); });
        C.getJson(api + '/' + historyRows[i].Id).then(function (h) {
            var dcols = ['DepartmentRequestNo', 'DepartmentFrom', 'DepartmentTo', 'WorkOrder', 'Item', 'ItemCode', 'Unit', 'Asset', 'IssueQty', 'Remarks'];
            var t = $id('grddetailHistory'), list = h.rows || [];
            t.tHead.innerHTML = '<tr>' + dcols.map(function (c) { return '<th>' + c + '</th>'; }).join('') + '</tr>';
            t.tBodies[0].innerHTML = list.map(function (r) {
                return '<tr>' + dcols.map(function (c) {
                    if (c === 'IssueQty') return '<td class="num">' + esc(fmt(r.IssueQty, 2)) + '</td>';
                    if (c === 'DepartmentRequestNo') return '<td class="ctr">' + esc(r[c]) + '</td>';
                    return '<td>' + esc(r[c]) + '</td>';
                }).join('') + '</tr>';
            }).join('');
            var s = list.reduce(function (a, r) { return a + (Number(r.IssueQty) || 0); }, 0);
            t.tFoot.innerHTML = '<tr>' + dcols.map(function (c) { return '<td class="num">' + (c === 'IssueQty' ? fmt(s, 2) : '') + '</td>'; }).join('') + '</tr>';
        }).catch(function (e) {
            if (e && e.message === 'Record Not Found') return;          // :1142 "if (po != null)"
            fail(e);
        });
    }

    // ------------------------------------------------------------------------------ shortcut keys

    function shortcutKeys() {
        alert(['Ctrl+S  For Save', 'Ctrl+U  For Update', 'Ctrl+E  For Close', 'Ctrl+R  For Refresh', 'Ctrl+N  For New',
            'Ctrl+P  For Print', 'Ctrl+L  To Load Request', 'Ctrl+F5  For Focus on DocDate', 'Ctrl+F10  For Open Attachments',
            'Ctrl+T  For Tab Transfer', 'Ctrl+alt  To Show ShortCut Keys Form', 'Ctrl+ArrowDown  For Focus On Detail Grid',
            'Ctrl+ArrowUp  For Focus On DocDate in Main Box', 'Ctrl+Enter  When Focus On Any Grid For Update Record',
            'Ctrl+Space  When Focus On Any Grid To Call Function\'s On Button Or Link'].join('\n'));
    }

    // ============================================================================== LOADER
    // LoadDepRequestToConsumableStore, DocumentTypeId = 1615: a new instance per click.

    function openLoader() {
        loader = { dtGridFromDb: [], shown: [], dtLoader: [], lookups: null };
        ['cmbDepartmentFrom', 'cmbDepartmentTo', 'cmbItem', 'cmbFromWStation', 'CmbToWorkStation', 'CmbWorkOrder'].forEach(function (id) { $id(id).innerHTML = ''; });
        $id('txtGrnNoFrom').value = '';
        $id('txtGrnNoTo').value = '';
        $id('Todate').value = C.today();                                    // designer default: Now
        $id('FromDate').value = C.today();
        renderLoaderGrid([]);
        C.openModal('dlgLoader');
        /* LoadInvoices_Load:123 — the 1615 labels/combos are visible; AllCombobind; Datetypefill;
           FromDate = ActiveYr.Start_Period; PendingDepRequestLoad. */
        allComboBind().then(function () {
            dateTypeFill();
            if (look.financialYearStart) $id('FromDate').value = look.financialYearStart;
            if ((parseInt($id('cmbperemeter').value, 10) || 0) === 0) $id('cmbperemeter').selectedIndex = 0;
            return loaderSearch();
        }).catch(fail);
    }

    /** AllCombobind:153 — BindAndRetainSelection, no default row, no row activated. */
    function allComboBind() {
        return C.getJson(api + '/loader/lookups').then(function (d) {
            loader.lookups = d;
            bindRetain('cmbItem', d.Item);
            bindRetain('cmbDepartmentTo', d.DepartmentTo);
            bindRetain('cmbDepartmentFrom', d.DepartmentFrom);
            bindRetain('CmbToWorkStation', d.WorkStationTo);           // DocumentTypeId == 1615
            bindRetain('cmbFromWStation', d.WorkStationFrom);
            bindRetain('CmbWorkOrder', d.WorkOrderNo);
        });
    }
    function bindRetain(id, list) {
        var el = $id(id), prev = el.value;
        if (!list || !list.length) { el.innerHTML = ''; return; }            // Text = "", DataSource = null
        var h = '<option value="0"></option>';                               // "no selection" (empty text)
        list.forEach(function (r) { h += '<option value="' + esc(r.Id) + '">' + esc(r.Name) + '</option>'; });
        el.innerHTML = h;
        el.value = list.some(function (r) { return String(r.Id) === prev; }) ? prev : '0';
    }

    /** Datetypefill:239 — BindComboDateType (default row "Select..." 0) then Rows[2] ("This Week"). */
    function dateTypeFill() {
        var el = $id('cmbperemeter');
        el.innerHTML = '<option value="0">Select...</option><option value="1">This Day</option><option value="2">This Week</option>' +
            '<option value="3">This Month</option><option value="4">This Year</option><option value="5">Financial Year</option>';
        el.selectedIndex = 2;
        peremeterChanged();
    }

    /** cmbperemeter_ValueChanged:252. */
    function peremeterChanged() {
        var v = parseInt($id('cmbperemeter').value, 10) || 0, today = C.today();
        if (v === 1) $id('FromDate').value = today;
        else if (v === 2) $id('FromDate').value = addDays(today, -7);
        else if (v === 3) {
            var n = new Date();                                            // DateTime.UtcNow
            $id('FromDate').value = n.getUTCFullYear() + '-' + String(n.getUTCMonth() + 1).padStart(2, '0') + '-01';
            $id('Todate').value = today;
        } else if (v === 4) {
            $id('FromDate').value = new Date().getFullYear() + '-01-01';
            $id('Todate').value = today;
        } else if (v === 5) {
            if (look.financialYearStart) $id('FromDate').value = look.financialYearStart;
        }
    }

    function digits(id) { var s = $id(id).value.replace(/\D/g, ''); return s ? parseInt(s, 10) : 0; }
    function comboVal(id) { return parseInt($id(id).value || '0', 10) || 0; }

    /** PendingDepRequestLoad:301. */
    function loaderSearch() {
        return C.getJson(api + '/loader/pending' + C.qs({
            fromDate: $id('FromDate').value, toDate: $id('Todate').value,
            docNoFrom: digits('txtGrnNoFrom') || '', docNoTo: digits('txtGrnNoTo') || '',
            departmentFromId: comboVal('cmbDepartmentFrom') || '', departmentToId: comboVal('cmbDepartmentTo') || '',
            workStationFromId: comboVal('cmbFromWStation') || '', workStationToId: comboVal('CmbToWorkStation') || '',
            workOrderId: comboVal('CmbWorkOrder') || '', itemId: comboVal('cmbItem') || ''
        })).then(function (list) {
            loader.dtGridFromDb = list || [];
            renderLoaderGrid(list && list.length ? list : []);             // else grd.DataSource = null
        }).catch(fail);
    }

    /* grdSettings:370 — hidden: Id, FromDepartmentId, ToDepartmentId, DocumentTypeId, AssetId, WorkOrderId,
       DetailId, ItemId, ItemUOMId; WorkOrderNo visible for 1615; "Select" selector at position 0. */
    var LCOLS = [
        { key: 'DocDate', date: true }, { key: 'DocNo', cls: 'ctr' }, { key: 'FromDepartment' }, { key: 'ToDepartment' },
        { key: 'WorkOrderNo' }, { key: 'ItemName' }, { key: 'ItemCode' }, { key: 'UOM' }, { key: 'AssetName' },
        { key: 'RequestedQty', qty: true }, { key: 'IssuedQty', qty: true }, { key: 'QtyBal', qty: true }, { key: 'QtyInStock', qty: true }
    ];
    function renderLoaderGrid(list) {
        loader.shown = list;
        var t = $id('grd');
        t.tHead.innerHTML = '<tr><th style="width:30px"><input type="checkbox" id="ldAll"></th>' + LCOLS.map(function (c) { return '<th>' + c.key + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = list.map(function (r, i) {
            return '<tr><td class="ctr"><input type="checkbox" data-c="' + i + '"></td>' + LCOLS.map(function (c) {
                if (c.date) return '<td>' + esc(C.gridDate(r.DocDate)) + '</td>';
                if (c.qty) return '<td class="num">' + esc(fmt(r[c.key], 3)) + '</td>';
                return '<td' + (c.cls ? ' class="' + c.cls + '"' : '') + '>' + esc(r[c.key]) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        t.tFoot.innerHTML = list.length ? '<tr><td></td>' + LCOLS.map(function (c) {
            if (!c.qty) return '<td></td>';
            return '<td class="num">' + fmt(list.reduce(function (a, r) { return a + (Number(r[c.key]) || 0); }, 0), 3) + '</td>';
        }).join('') + '</tr>' : '';
        var all = $id('ldAll');
        all.onchange = function () { t.tBodies[0].querySelectorAll('[data-c]').forEach(function (c) { c.checked = all.checked; }); };
    }

    /** btnReset_Click:402 — focus FromDate, grd.DataSource = null. */
    function loaderNew() { $id('FromDate').focus(); renderLoaderGrid([]); }

    /** btnRefresh_Click:413 — AllCombobind, Datetypefill. */
    function loaderRefresh() { allComboBind().then(dateTypeFill).catch(fail); }

    /** btnLoadOnInvoice_Click_1:419 (DocumentTypeId 1615: no same-document check). */
    function loaderLoad() {
        var checked = [];
        $id('grd').tBodies[0].querySelectorAll('[data-c]').forEach(function (c) {
            if (c.checked) checked.push(loader.shown[parseInt(c.getAttribute('data-c'), 10)]);
        });
        if (!checked.length) { alert('Check the row first'); return; }
        var ids = {};
        checked.forEach(function (r) { ids[String(r.DetailId)] = 1; });
        loader.dtLoader = loader.dtGridFromDb.filter(function (r) { return ids[String(r.DetailId)]; });
        loaderClose(false);
    }

    function loaderShortcutKeys() {
        alert(['Ctrl+E  For Close', 'Ctrl+N  For New', 'Ctrl+R  For Refresh', 'Ctrl+L  To Press Load Button', 'Ctrl+S  For Search',
            'Ctrl+alt  To Show ShortCut Keys Form', 'Ctrl+Space  When Focus On Any Grid To Call Function\'s On Button Or Link'].join('\n'));
    }

    /** Hide(); escape (Ctrl+E / Esc) sets dtLoader = null first (:476). Then LoadInGridDetail. */
    function loaderClose(escape) {
        if (!loader) return;
        var dt = escape ? null : loader.dtLoader;
        C.closeModal('dlgLoader');
        loader = null;
        loadInGridDetail(dt);
    }

    /**
     * LoadInGridDetail:952. Rows already in the grid (by DepartmentRequestDetailId) are skipped.
     * DEVIATION (service deviation 6): the desktop reads row["UOM"] here, a column the procedure rows
     * do not have, and fails on the first row; the Unit cell is filled from UOMCode (sent as "UOM").
     */
    function loadInGridDetail(dt) {
        if (dt && dt.length) {
            var existing = {};
            rows.forEach(function (r) { existing[r.DepartmentRequestDetailId | 0] = 1; });
            dt.forEach(function (r) {
                var detailId = r.DetailId | 0;
                if (existing[detailId]) return;
                existing[detailId] = 1;
                rows.push({
                    Id: 0, DepartmentRequestId: r.Id, DepartmentRequestDetailId: detailId, DepartmentRequestNo: r.DocNo,
                    LocationId: 0, DepartmentFromId: r.FromDepartmentId, DepartmentFrom: r.FromDepartment,
                    DepartmentToId: r.ToDepartmentId, DepartmentTo: r.ToDepartment, WorkOrderId: r.WorkOrderId,
                    WorkOrder: r.WorkOrderNo, ItemId: r.ItemId, Item: r.ItemName, ItemCode: r.ItemCode,
                    UnitId: r.ItemUOMId, Unit: r.UOM, AssetId: r.AssetId, Asset: r.AssetName,
                    IssueQty: r.QtyBal, Remarks: ''                                   // BalQty
                });
            });
            renderGrid();
        } else if (!rows.length) {
            formReset();
        }
    }

    document.addEventListener('DOMContentLoaded', init);

    window.SiCons = {
        newClick: formReset, refresh: refresh, save: save, update: update, print: print,
        openLoader: openLoader, shortcutKeys: shortcutKeys, tab: tab, loadAll: loadAll,
        peremeterChanged: peremeterChanged, loaderSearch: loaderSearch, loaderNew: loaderNew,
        loaderRefresh: loaderRefresh, loaderLoad: loaderLoad, loaderShortcutKeys: loaderShortcutKeys,
        loaderClose: loaderClose
    };
})();
