/* 956 Sale Invoice (Upload) - Architecture.WinApp.DataSyncing.frmPendingSaleInvoiceForUpload.
   ":NNN" = line in frmPendingSaleInvoiceForUpload.cs. API /sale/api/sale-invoice-for-upload.
   Built on pr_common.js (window.PR): checked-list branch combo, GridEX-like grid with a row checkbox column,
   filter row and total row; DocLink (countx_doc_link.js) opens an invoice from its DocNo. */
(function () {
    'use strict';
    var $ = PR.$, API = '/sale/api/sale-invoice-for-upload';
    var st = { init: null, rows: [] };
    var branches = new PR.CheckList('CmbBranch');
    var grid = new PR.Grid('grd', 'lblCount');

    /** grdSettingsForGrnPending :437 - dtGrid order (:400-436), the 14 hidden columns, NoOfAttachments as a link,
        AddSelectButton("Select", 30, 0) = the row checkbox column in front. Widths stand in for
        Constants.InventoryConstants (not in the decompiled sources; PR.K as the other ported registers). Numeric
        columns: "#,##0.###" and Amount columns in the amount format, all summed in the total row
        (GridWrappingAndColumnSettings(grd, 3, 2) is not in the sources either). DocNo opens the invoice (DocLink). */
    function columns() {
        var K = PR.K, H = true;
        return [
            { key: 'BranchId', hidden: H }, { key: 'BranchName', hidden: H, width: K.BranchName },
            { key: 'DocumentTypeId', hidden: H }, { key: 'DocumentType', width: 150 },
            { key: 'Id', hidden: H }, { key: 'DocNo', width: K.DocNo, link: true },
            { key: 'DocDate', width: K.Date + 15, type: 'date' },
            { key: 'SupplierCustomerId', hidden: H }, { key: 'CustomerName', width: K.SupplierName },
            { key: 'PaymentTerm', width: K.Term }, { key: 'GpNo', width: K.GpNo }, { key: 'VehicleNo', width: K.VehicleNo },
            { key: 'ItemId', hidden: H }, { key: 'ItemCode', width: K.ItemCode }, { key: 'ItemName', width: K.ItemName },
            { key: 'CropYear', hidden: H, width: K.CropYear }, { key: 'JobLot', hidden: H, width: K.JobLot },
            { key: 'PackUom', width: K.Uom },
            { key: 'ItemQty', width: K.Qty, type: 'num', fmt: 'n3', agg: 'sum' },
            { key: 'NetBillWeight', width: K.Weight, type: 'num', fmt: 'n3', agg: 'sum' },
            { key: 'ItemRate', width: K.Rate, type: 'num', fmt: 'n3', agg: 'sum' },
            { key: 'RateUom', width: K.Uom },
            { key: 'ItemAmount', width: K.Amount, type: 'num', fmt: 'amt', agg: 'sum' },
            { key: 'BillAmount', width: K.Amount, type: 'num', fmt: 'amt', agg: 'sum' },
            { key: 'EntryDate', width: K.DateTime, type: 'datetime' }, { key: 'EntryUser', width: K.EntryUser },
            { key: 'ModifyDate', hidden: H, type: 'datetime' }, { key: 'ModifyUser', hidden: H },
            { key: 'ApprovalStatus', hidden: H }, { key: 'ApprovedDate', hidden: H, type: 'datetime' }, { key: 'ApprovedUser', hidden: H },
            { key: 'WareHouseName', hidden: H, width: K.WareHouseName },
            { key: 'CommissionAgent', width: K.SupplierName },
            { key: 'CommAmount', width: K.Amount, type: 'num', fmt: 'amt', agg: 'sum' },
            { key: 'NoOfAttachments', width: K.NoOfAttachments, type: 'int', link: true }];
    }

    // ------------------------------------------------------------------ busy buttons
    /** Disable the button at once, show the wait overlay, refuse a second click, re-enable on success and failure. */
    function busy(btn, fn) {
        if (btn && (btn.disabled || btn.getAttribute('aria-busy') === 'true')) return Promise.resolve();
        if (btn) { btn.disabled = true; btn.setAttribute('aria-busy', 'true'); }
        return PR.run(fn).then(function (v) {
            if (btn) { btn.disabled = false; btn.removeAttribute('aria-busy'); }
            return v;
        });
    }

    // ------------------------------------------------------------------ combos
    function branchItems(rows) { return (rows || []).map(function (b) { return { id: b.BranchId, name: b.BranchName }; }); }
    /** ComboBind :258 - BindAndRetainSelection(CmbCustomer, dt, "Id", "ReferenceName", "Customer Name", ..., insertDefaultRow: false). */
    function ComboBind(rows) { PR.fill('CmbCustomer', rows, { value: 'Id', text: 'ReferenceName' }); }

    // ------------------------------------------------------------------ search
    function actionId() { var r = document.querySelector('input[name="accounts"]:checked'); return r ? PR.int(r.value) : 0; }
    function filter() {
        return { branchIds: branches.ids(), fromDate: $('FromDate').value, toDate: $('Todate').value,
            fromDocNo: $('txtFromDocNo').value, toDocNo: $('TxtToDocNo').value,
            supplierCustomerId: PR.val('CmbCustomer'), actionId: actionId() };
    }
    /** BtnShow_Click :316 - PendingDataDbCall :329 then PendingDataGridBind :392. A failed call still clears the grid
        (dtRecordsFromDb was cleared first, :335), as does an empty result (grd.DataSource = null, :449). */
    function bind(rows) {
        st.rows = rows || [];
        if (!st.rows.length) { grid.clear(); return; }
        grid.show(columns(), st.rows, { selector: true, onLink: link });
    }
    function BtnShow_Click() {
        if (!branches.ids().length) {
            grid.clear(); st.rows = [];
            branches.el.querySelector('summary').focus();
            return Promise.reject(new Error('Select branch first'));
        }
        return PR.request(API + '/rows', filter()).then(function (res) { bind(res && res.rows); },
            function (e) { grid.clear(); st.rows = []; throw e; });
    }

    /** grd_LinkClicked :491 (NoOfAttachments) and the DocNo link (CommonServices.EditMethodFromLinked via DocLink). */
    function link(col, r) {
        if (col === 'NoOfAttachments') attachments(PR.int(r.Id), PR.int(r.DocumentTypeId));
        if (col === 'DocNo') DocLink.open(PR.int(r.DocumentTypeId), PR.int(r.Id), { message: PR.box });
    }
    /** CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId) - the AttachmentView list. */
    function attachments(id, documentTypeId) {
        var q = '?id=' + encodeURIComponent(id) + '&documentTypeId=' + encodeURIComponent(documentTypeId);
        return PR.run(function () {
            return PR.request(API + '/attachments' + q).then(function (rows) {
                if (!rows || !rows.length) return;
                return PR.dialog('Attachments', '<table class="pr-grid"><thead><tr><th style="width:220px">AttachmentName</th><th style="width:220px">CustomName</th><th style="width:130px">EntryDate</th></tr></thead><tbody>'
                    + rows.map(function (a) { return '<tr><td><a href="' + API + '/attachments/' + PR.esc(a.Id) + q + '">' + PR.esc(a.AttachmentName) + '</a></td><td>' + PR.esc(a.CustomName) + '</td><td>' + PR.esc(PR.dmyt(a.EntryDate)) + '</td></tr>'; }).join('') + '</tbody></table>');
            });
        });
    }

    // ------------------------------------------------------------------ upload
    /** BtnUploadInvoices_Click :525 - "Check any row first", "Are you sure to Save?", then the upload loop on the server;
        success -> "Record Uploaded Successfully" and BtnShow_Click (:563-564). */
    function BtnUploadInvoices_Click() {
        var checkedRows = grid.checked();
        if (!checkedRows.length) { PR.box('Check any row first'); return Promise.resolve(); }
        if (!window.confirm('Are you sure to Save?')) return Promise.resolve();
        var rows = checkedRows.map(function (r) { return { Id: r.Id, DocNo: r.DocNo, DocumentTypeId: r.DocumentTypeId }; });
        return PR.request(API + '/upload', { rows: rows }).then(function (res) {
            PR.box((res && res.message) || 'Record Uploaded Successfully');
            return BtnShow_Click();
        });
    }

    // ------------------------------------------------------------------ toolstrip
    /** btnNew_Click :258 - dates back to Start_Period / Now, grid cleared, FromDate focused (other filters kept). */
    function btnNew_Click() {
        if (st.init) { $('FromDate').value = String(st.init.yearStart || '').slice(0, 10); $('Todate').value = String(st.init.now || '').slice(0, 10); }
        grid.clear(); st.rows = [];
        $('FromDate').focus();
    }
    /** btnRefresh_Click :273 - BranchesDbCall, BranchesBind (the user's branch ticked again), ComboBind(ComboDBCall()). */
    function btnRefresh_Click() {
        return PR.request(API + '/refresh').then(function (d) {
            branches.set(branchItems(d.branches), d.branchId ? [d.branchId] : []);
            ComboBind(d.customers);
        });
    }
    /** MakeShortCutKeys :572 - the list exactly as the desktop shows it. */
    function MakeShortCutKeys() {
        PR.shortcuts([['Ctrl+E', 'For Close'], ['Ctrl+L', 'To Press Load Button'], ['Ctrl+S', 'For Search'],
            ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }

    /** Constructor: InitializeComponentCustom :118 + InitializeComponentMethod :137 (branches, the pending list of every branch,
        then the customers; BranchesBind, PendingDataGridBind, ComboBind). */
    function load() {
        return PR.request(API + '/init').then(function (d) {
            st.init = d;
            PR.cfg.amountDecimals = d.amountDecimals;
            $('FromDate').value = String(d.yearStart || '').slice(0, 10);   // ActiveYr.Start_Period
            $('Todate').value = String(d.now || '').slice(0, 10);          // DateTime.Now
            branches.set(branchItems(d.branches), d.branchId ? [d.branchId] : []);
            bind(d.rows);
            ComboBind(d.customers);
            $('FromDate').focus();
        });
    }

    $('BtnShow').addEventListener('click', function () { busy($('BtnShow'), BtnShow_Click); });
    $('BtnUploadInvoices').addEventListener('click', function () { busy($('BtnUploadInvoices'), BtnUploadInvoices_Click); });
    $('btnNew').addEventListener('click', btnNew_Click);
    $('btnRefresh').addEventListener('click', function () { busy($('btnRefresh'), btnRefresh_Click); });
    $('btnShortcutKeys').addEventListener('click', MakeShortCutKeys);
    PR.gridTools(grid, 'btnGridPrint', 'btnGridExport', function () { return 'Pending Sale Invoices For Upload'; });
    PR.fullscreen('btnFullscreen', 'gridSection');
    PR.enterAsTab();                                                        // KeyDown :633 Return -> {TAB}
    /** frmPendingSaleInvoiceForUpload_KeyDown :606. */
    document.addEventListener('keydown', function (e) {
        if (document.querySelector('dialog[open]')) return;
        var k = e.key.toLowerCase();
        if (k === 'escape' && !e.ctrlKey) { e.preventDefault(); location.href = '/sale/customer'; return; }   // :615 Escape -> Hide()
        if (e.ctrlKey && e.altKey) { e.preventDefault(); MakeShortCutKeys(); return; }
        if (!e.ctrlKey) return;
        var a = {
            e: function () { location.href = '/sale/customer'; },          // dtLoader = null; Hide()
            s: function () { busy($('BtnShow'), BtnShow_Click); },
            u: function () { busy($('BtnUploadInvoices'), BtnUploadInvoices_Click); },
            n: function () { }, r: function () { },                         // :631 - Ctrl+N / Ctrl+R do nothing
            f5: function () { $('FromDate').focus(); },
            arrowdown: function () { $('pfuWrap').focus(); },
            arrowup: function () { $('FromDate').focus(); }
        }[k];
        if (a) { e.preventDefault(); a(); }
    });
    PR.run(load);
})();
