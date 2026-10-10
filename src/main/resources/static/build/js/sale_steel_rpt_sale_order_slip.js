/* ============================================================================================
 * Screen 560 frmSaleOrderSlipAndRegister - Architecture.WinApp.Inventory_Reports.frmSaleOrderSlipAndRegister (.cs, Steel BLL)
 * Page: templates/sale/steel/rpt_sale_order_slip_register_st.html (route /sale/reports/steel/sale-order-slip-register).
 *   frmSaleOrderSlipAndRegister_Load :72   combobind, datFromDate = Start_Period, ApprovedFill (Rows[2] = All), OrderStatusFill (Rows[0] = Open), gridHisory
 *   combobind :95          InvSaleInvoice.GetDataForDropDownFromSaleInvoiceForSteel ([ST].[USP_GetDataForDropDownFromSaleInvoice] Org, Company): rows of Activity
 *                          Customer -> cmbPartyName, Item -> cmbItemName, ParentCategory, ItemCategory, ItemType (BindDDL ZeroIndex false, ReferenceName)
 *   ApprovedFill :150      1 UnApporve, 2 Approve, 3 All.   OrderStatusFill :171  1 Open, 2 Cancel, 3 Complete
 *   gridHisory :209        SaleOrder.InvSaleOrderSlipRegisterForSteel = [ST].[USP_SaleOrderSlipAndRegister]; Status = the status combo TEXT; @IsApproved sent unless the
 *                          Approved text is "All" (false for UnApporve, true for Approve); 55 column table
 *   grdSetting :326        Id, PoDId, DocumentTypeId, OrderItemId, OrderSupCustId hidden; DocNo and NoOfAttachments are links; GridAutoAdjustment
 *   SaleRegisterGridHistory_LinkClicked :349  NoOfAttachments -> attachments of (Id, DocumentTypeId); DocNo -> SaleOrderSlip_1511 (1511-SaleOrderSlipAndRegister.rpt)
 *   Reset / btnNew :385    combobind again, doc numbers cleared.   btnRegister :405  "Record Not Found For Display" else 1509-SaleOrderSlipAndRegister.rpt over dt
 *   txtFromDocNo / txtToDocNo KeyPress: digits only.   KeyDown: Ctrl+E / Esc close, Ctrl+N refresh, Ctrl+P register
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, S = w.SaRpt1, el = A.el;
    var API = '/api/sale/steel-reports/sale-order-slip-register';
    var PRINT = '/sale/reports/steel/print/';
    var lastArgs = null, hasRows = false, seq = 0;

    var grid = new S.Grid({ tableId: 'results', gridId: 'grid', navId: 'nav', navTextId: 'navText', headerLines: 2, autosize: true, onLink: onLink });

    function cols() {
        var D = 'dd-MMM-yy', R = 'r';
        return [
            { key: 'Id', caption: 'Id', hidden: true }, { key: 'PoDId', caption: 'PoDId', hidden: true }, { key: 'DocumentTypeId', caption: 'DocumentTypeId', hidden: true },
            { key: 'DocumentTypeDescription', caption: 'DocumentTypeDescription' }, { key: 'DocNo', caption: 'DocNo', link: true, align: R },
            { key: 'DocDate', caption: 'DocDate', date: D }, { key: 'OrderCategory', caption: 'OrderCategory' }, { key: 'CatagorySrNo', caption: 'CatagorySrNo', align: R },
            { key: 'SupplierName', caption: 'SupplierName' }, { key: 'OrderDueDate', caption: 'OrderDueDate', date: D }, { key: 'OrderDueDays', caption: 'OrderDueDays', align: R },
            { key: 'OrderStatus', caption: 'OrderStatus' }, { key: 'CommissionAgent', caption: 'CommissionAgent' }, { key: 'CommissionType', caption: 'CommissionType' },
            { key: 'CommRate', caption: 'CommRate', align: R }, { key: 'CommAmount', caption: 'CommAmount', align: R }, { key: 'CommRemarks', caption: 'CommRemarks' },
            { key: 'TermsDescription', caption: 'TermsDescription' }, { key: 'OrderExpiryDate', caption: 'OrderExpiryDate', date: D }, { key: 'DeliveryTerm', caption: 'DeliveryTerm' },
            { key: 'DeliveryStartDate', caption: 'DeliveryStartDate', date: D }, { key: 'DeliveryDays', caption: 'DeliveryDays', align: R }, { key: 'DeliveryRemarks', caption: 'DeliveryRemarks' },
            { key: 'IsApproved', caption: 'IsApproved' }, { key: 'EntryUser', caption: 'EntryUser' }, { key: 'EntryDate', caption: 'EntryDate', date: D },
            { key: 'ModifyUser', caption: 'ModifyUser' }, { key: 'ModifyDate', caption: 'ModifyDate', date: D }, { key: 'ApprovedUser', caption: 'ApprovedUser' },
            { key: 'ApprovedDate', caption: 'ApprovedDate', date: D }, { key: 'ItemCode', caption: 'ItemCode', align: R }, { key: 'ItemName', caption: 'ItemName' },
            { key: 'PackUom', caption: 'PackUom' }, { key: 'OrderItemQty', caption: 'OrderItemQty', align: R }, { key: 'DispatchQty', caption: 'DispatchQty', align: R },
            { key: 'BalQty', caption: 'BalQty', align: R }, { key: 'NetWeight', caption: 'NetWeight', align: R }, { key: 'DispatchWeight', caption: 'DispatchWeight', align: R },
            { key: 'BalWeight', caption: 'BalWeight', align: R }, { key: 'OrderItemRate', caption: 'OrderItemRate', align: R }, { key: 'RateUom', caption: 'RateUom' },
            { key: 'EquivalentRate', caption: 'EquivalentRate', align: R }, { key: 'ItemAmount', caption: 'ItemAmount', align: R }, { key: 'CityName', caption: 'CityName' },
            { key: 'JobLotCode', caption: 'JobLotCode' }, { key: 'OrderItemId', caption: 'OrderItemId', hidden: true }, { key: 'SaleGLAC', caption: 'SaleGLAC', align: R },
            { key: 'OrderSupCustId', caption: 'OrderSupCustId', hidden: true }, { key: 'PackTypeDesc', caption: 'PackTypeDesc' }, { key: 'CurrencyCode', caption: 'CurrencyCode' },
            { key: 'ExchangeRate', caption: 'ExchangeRate', align: R }, { key: 'FcyAmount', caption: 'FcyAmount', align: R }, { key: 'ItemNetAmount', caption: 'ItemNetAmount', align: R },
            { key: 'RemarksHeader', caption: 'RemarksHeader' }, { key: 'NoOfAttachments', caption: 'NoOfAttachments', link: true, align: R }
        ];
    }

    // ------------------------------------------------------------------ lists
    function combobind(data) {
        S.fill(el('cmbPartyName'), data.parties, false);
        S.fill(el('cmbItemName'), data.items, false);
        S.fill(el('cmbParentCategory'), data.parentCategories, false);
        S.fill(el('cmbItemCategory'), data.itemCategories, false);
        S.fill(el('cmbItemType'), data.itemTypes, false);
    }
    function loadLists() { return A.getJson(API + '/lookups').then(function (data) { combobind(data); return data; }); }
    function approvedFill() { S.fill(el('cmbIsApproved'), [{ Id: 1, name: 'UnApporve' }, { Id: 2, name: 'Approve' }, { Id: 3, name: 'All' }], false); S.activate('cmbIsApproved', 2, false); }
    function statusFill() { S.fill(el('cmbStatus'), [{ Id: 1, name: 'Open' }, { Id: 2, name: 'Cancel' }, { Id: 3, name: 'Complete' }], false); S.activate('cmbStatus', 0, false); }

    // ------------------------------------------------------------------ Show
    function gridHisory() {
        var b = el('btnShow'); if (b.disabled) return Promise.resolve();
        var a = { fromDate: el('datFromDate').value, toDate: el('datToDate').value, fromDocNo: A.toIntText(el('txtFromDocNo').value), toDocNo: A.toIntText(el('txtToDocNo').value),
                  supplierCustomerId: S.selInt('cmbPartyName'), itemId: S.selInt('cmbItemName'), itemCategoryId: S.selInt('cmbItemCategory'),
                  parentCategoryId: S.selInt('cmbParentCategory'), itemTypeId: S.selInt('cmbItemType'), status: S.selText('cmbStatus'),
                  approvedId: S.selInt('cmbIsApproved'), approvedFilter: S.selText('cmbIsApproved') };
        var token = ++seq;
        A.busy(b, true);
        return A.getJson(API + '/rows?' + S.qs(a)).then(function (rows) {
            if (token !== seq) return;
            lastArgs = a; hasRows = !!(rows && rows.length);
            if (!hasRows) { grid.clear(); return; }
            grid.setData(cols(), rows);
        }).catch(function (e) { alert(e.message); }).then(function () { if (token === seq) A.busy(b, false); });
    }

    // ------------------------------------------------------------------ links / attachments
    function onLink(row, col) {
        if (!row || !col) return;
        var id = A.toInt(A.ci(row, 'Id'));
        if (col.key === 'NoOfAttachments') {
            A.getJson(API + '/attachments?' + S.qs({ id: id, documentTypeId: A.toInt(A.ci(row, 'DocumentTypeId')) })).then(function (list) {
                if (!list || !list.length) return;                                  // no attachments: the desktop shows nothing
                var tb = el('attRows'); tb.innerHTML = '';
                list.forEach(function (r) {
                    var tr = tb.insertRow();
                    tr.insertCell().textContent = r.AttachmentName == null ? '' : r.AttachmentName;
                    tr.insertCell().textContent = r.CustomName == null ? '' : r.CustomName;
                    tr.insertCell().textContent = S.fmtDate(r.EntryDate, 'dd-MMM-yy');
                });
                var dlg = el('attDialog'); if (dlg.showModal) dlg.showModal(); else dlg.setAttribute('open', '');
            }).catch(function (e) { alert(e.message); });
        } else if (col.key === 'DocNo') {
            A.openPdf(PRINT + 'sale-order-slip?id=' + id).catch(function (e) { alert(e.message); });
        }
    }
    el('closeAtt').addEventListener('click', function () { el('attDialog').close(); });

    // ------------------------------------------------------------------ toolbar
    function reset() {                                                              // Reset(): combobind, doc numbers cleared
        el('txtFromDocNo').value = ''; el('txtToDocNo').value = '';
        loadLists().catch(function (e) { alert(e.message); });
    }
    function register() {                                                           // btnRegister_Click
        if (!hasRows || !lastArgs) { alert('Record Not Found For Display'); return; }
        A.openPdf(PRINT + 'sale-order-slip-register?' + S.qs(lastArgs)).catch(function (e) { alert(e.message); });
    }
    function closeForm() { w.location.href = '/sale/reports'; }

    el('btnShow').addEventListener('click', gridHisory);
    el('reset').addEventListener('click', reset);
    el('register').addEventListener('click', register);
    S.digitsOnly('txtFromDocNo'); S.digitsOnly('txtToDocNo');
    S.keys({ n: reset, p: register, e: closeForm }, closeForm);

    // ------------------------------------------------------------------ start (Load)
    el('datFromDate').value = A.today(); el('datToDate').value = A.today();
    grid.render();
    A.getJson(API + '/lookups').then(function (data) {
        combobind(data);                                                            // combobind
        if (data.yearStart) el('datFromDate').value = S.isoDay(data.yearStart);     // datFromDate = ActiveYr.Start_Period
        approvedFill(); statusFill();
        return gridHisory();
    }).catch(function (e) { alert(e.message); });
}(window, document));
