/* 917 Export Voucher (Upload) - Architecture.WinApp.DataSyncing.frmPendingExportVoucherForUpload.
   ":NNN" = line in frmPendingExportVoucherForUpload.cs. API /accounts/api/export-voucher-for-upload.
   Built on pr_common.js (window.PR): GridEX-like grid with a row checkbox column, filter row and total row. */
(function () {
    'use strict';
    var $ = PR.$, API = '/accounts/api/export-voucher-for-upload';
    var st = { init: null, rows: [] };
    var grid = new PR.Grid('grd', 'lblCount');

    /** grdSettingsForGrnPending :222 - dtGrid order (:257-292); hidden: Id, DocumentTypeId, DocumentType, SupplierCustomerId, CropYear,
        ("WareHouseName" - the grid column is "Warehouse", so it stays visible), ApprovedUser, ApprovedDate, ApprovalStatus, ModifyUser,
        ModifyDate; NoOfAttachments is a link column; AddSelectButton("Select", 30, 0) = the row checkbox column in front; FrozenColumns = 1.
        Widths stand in for Constants.ExportConstants (not in the decompiled sources; PR.K as the other ported registers). */
    function columns() {
        var K = PR.K, H = true;
        return [
            { key: 'DocumentTypeId', hidden: H }, { key: 'DocumentType', hidden: H },
            { key: 'Id', hidden: H }, { key: 'DocNo', width: K.DocNo },
            { key: 'DocDate', width: K.Date + 15, type: 'date' },
            { key: 'SupplierCustomerId', hidden: H }, { key: 'CustomerName', width: K.SupplierName },
            { key: 'InvoiceNo', width: 100 }, { key: 'CreditAccount', width: 170 },
            { key: 'FcyCode', width: K.Currency - 10 },
            { key: 'ExchangeRate', width: K.Rate, type: 'num', fmt: 'n3' },
            { key: 'FcyAmount', width: K.Amount, type: 'num', fmt: 'amt', agg: 'sum' },
            { key: 'AddLessAmount', width: K.AddLess, type: 'num', fmt: 'amt', agg: 'sum' },
            { key: 'LcyAmount', width: K.Amount + 10, type: 'num', fmt: 'amt', agg: 'sum' },
            { key: 'ContractNo', width: 100 + 30 }, { key: 'ItemName', width: K.ItemName }, { key: 'Warehouse', width: K.String },
            { key: 'CropYear', hidden: H, width: K.CropYear }, { key: 'JobLot', width: K.JobLot }, { key: 'PackingType', width: K.PackingType },
            { key: 'ItemQty', width: K.Qty, type: 'num', fmt: 'n3', agg: 'sum' }, { key: 'PackUom', width: K.Uom },
            { key: 'NetBillWeight', width: K.Weight, type: 'num', fmt: 'n3', agg: 'sum' },
            { key: 'ItemRate', width: K.Rate, type: 'num', fmt: 'n3' }, { key: 'RateUom', width: K.Uom },
            { key: 'CostRate', width: K.Rate, type: 'num', fmt: 'n3' },
            { key: 'FcyAmountDetail', width: K.Amount, type: 'num', fmt: 'amt', agg: 'sum' },
            { key: 'LcyAmountDetail', width: K.Amount + 10, type: 'num', fmt: 'amt', agg: 'sum' },
            { key: 'EntryDate', width: K.DateTime, type: 'datetime' }, { key: 'EntryUser', width: K.EntryUser + 20 },
            { key: 'ModifyDate', hidden: H, type: 'datetime' }, { key: 'ModifyUser', hidden: H },
            { key: 'ApprovalStatus', hidden: H }, { key: 'ApprovedDate', hidden: H, type: 'datetime' }, { key: 'ApprovedUser', hidden: H },
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
    /** ComboBind :122 - BindAndRetainSelection(CmbBuyer / CmbContractNo / CmbCurrency, dt, "Id", "name", ..., insertDefaultRow: false). */
    function ComboBind(d) {
        PR.fill('CmbBuyer', (d && d.buyers) || [], { value: 'Id', text: 'name' });
        PR.fill('CmbContractNo', (d && d.contracts) || [], { value: 'Id', text: 'name' });
        PR.fill('CmbCurrency', (d && d.currencies) || [], { value: 'Id', text: 'name' });
    }

    // ------------------------------------------------------------------ search
    function filter() {
        return { fromDate: $('FromDate').value, toDate: $('Todate').value,
            fromDocNo: $('txtFromDocNo').value, toDocNo: $('TxtToDocNo').value,
            supplierCustomerId: PR.val('CmbBuyer'), contractId: PR.val('CmbContractNo'), currencyId: PR.val('CmbCurrency') };
    }
    /** BtnShow_Click :152 - PendingDataDbCall :163 then PendingDataGridBind :188. A failed call still clears the grid
        (dtRecordsFromDb was cleared first), as does an empty result (grd.DataSource = null). */
    function BtnShow_Click() {
        return PR.request(API + '/rows', filter()).then(function (res) {
            st.rows = (res && res.rows) || [];
            if (!st.rows.length) { grid.clear(); return; }
            grid.show(columns(), st.rows, { selector: true, onLink: link });
        }, function (e) { grid.clear(); st.rows = []; throw e; });
    }

    /** grd_LinkClicked :218 (NoOfAttachments) - CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId). */
    function link(col, r) {
        if (col === 'NoOfAttachments') attachments(PR.int(r.Id), PR.int(r.DocumentTypeId));
    }
    function attachments(id, documentTypeId) {
        var q = '?id=' + encodeURIComponent(id) + '&documentTypeId=' + encodeURIComponent(documentTypeId);
        return PR.run(function () {
            return PR.request(API + '/attachments' + q).then(function (rows) {
                if (!rows || !rows.length) return;
                return PR.dialog('Attachments', '<table class="pr-grid"><thead><tr><th style="width:220px">AttachmentName</th><th style="width:220px">CustomName</th><th style="width:130px">EntryDate</th></tr></thead><tbody>'
                    + rows.map(function (a) { return '<tr><td><a href="' + API + '/attachments/' + PR.esc(a.Id) + q + '">' + PR.esc(a.AttachmentName) + '</a></td><td>' + PR.esc(a.CustomName) + '</td><td>' + PR.esc(PR.dmyt(a.EntryDate)) + '</td></tr>'; }).join('')
                    + '</tbody></table>');
            });
        });
    }

    // ------------------------------------------------------------------ upload
    /** BtnUploadInvoices_Click :264 - "Check any row first", "Are you sure to Save?", then the upload loop on the server;
        success -> "Record Uploaded Successfully" and BtnShow_Click (:279-280). */
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
    /** btnNew_Click :132 - dates back to Start_Period / Now, grid cleared, FromDate focused (other filters kept). */
    function btnNew_Click() {
        if (st.init) { $('FromDate').value = String(st.init.yearStart || '').slice(0, 10); $('Todate').value = String(st.init.now || '').slice(0, 10); }
        grid.clear(); st.rows = [];
        $('FromDate').focus();
    }
    /** btnRefresh_Click :145 - ComboBind(ComboDBCall()). */
    function btnRefresh_Click() {
        return PR.request(API + '/refresh').then(ComboBind);
    }
    /** MakeShortCutKeys :298 - the list exactly as the desktop shows it. */
    function MakeShortCutKeys() {
        PR.shortcuts([['Ctrl+E', 'For Close'], ['Ctrl+L', 'To Press Load Button'], ['Ctrl+S', 'For Search'],
            ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }

    /** Constructor: InitializeComponentCustom :72 + InitializeComponentMethod :87. */
    function load() {
        return PR.request(API + '/init').then(function (d) {
            st.init = d;
            PR.cfg.amountDecimals = d.amountDecimals;
            $('FromDate').value = String(d.yearStart || '').slice(0, 10);   // ActiveYr.Start_Period
            $('Todate').value = String(d.now || '').slice(0, 10);          // DateTime.Now
            ComboBind(d);
            $('FromDate').focus();
        });
    }

    $('BtnShow').addEventListener('click', function () { busy($('BtnShow'), BtnShow_Click); });
    $('BtnUploadInvoices').addEventListener('click', function () { busy($('BtnUploadInvoices'), BtnUploadInvoices_Click); });
    $('btnNew').addEventListener('click', btnNew_Click);
    $('btnRefresh').addEventListener('click', function () { busy($('btnRefresh'), btnRefresh_Click); });
    $('btnShortcutKeys').addEventListener('click', MakeShortCutKeys);
    PR.digitsOnly(['txtFromDocNo', 'TxtToDocNo']);                         // txtFromDocNo_KeyPress :349 (OnlytextNumberFunction)
    PR.gridTools(grid, 'btnGridPrint', 'btnGridExport', function () { return 'Pending Export Vouchers For Upload'; });
    PR.fullscreen('btnFullscreen', 'gridSection');
    PR.enterAsTab();                                                        // KeyDown :310 Return -> {TAB}
    /** frmPendingSaleInvoiceForUpload_KeyDown :306 (the handler's name is the sale form's; it is this form's KeyDown). */
    document.addEventListener('keydown', function (e) {
        if (document.querySelector('dialog[open]')) return;
        var k = e.key.toLowerCase();
        if (e.ctrlKey && e.altKey) { e.preventDefault(); MakeShortCutKeys(); return; }
        if (!e.ctrlKey) return;
        var a = {
            e: function () { location.href = '/accounts'; },                   // dtLoader = null; Hide()
            s: function () { busy($('BtnShow'), BtnShow_Click); },
            u: function () { busy($('BtnUploadInvoices'), BtnUploadInvoices_Click); },
            n: function () { }, r: function () { },                         // Ctrl+N / Ctrl+R do nothing
            f5: function () { $('FromDate').focus(); },
            arrowdown: function () { $('pfuWrap').focus(); },
            arrowup: function () { $('FromDate').focus(); }
        }[k];
        if (a) { e.preventDefault(); a(); }
    });
    PR.run(load);
})();
