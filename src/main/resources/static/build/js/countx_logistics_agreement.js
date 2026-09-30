/* ============================================================================================
 * countx_logistics_agreement.js - 939 "Logistics Agreement" (frmLogisticAgreement.cs, DocumentTypeId 1301).
 * Built on countx_hrm.js. Every handler names the desktop method it follows; the server repeats every check.
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/logistics/agreement';
    var P = {};
    window.LgsAG = P;

    if (window.DesktopCombo) {
        window.DesktopCombo.define('lgsaItem', [
            { caption: 'Item', flex: 4 }, { caption: 'Code / Name', flex: 2, key: 'alt' },
            { caption: 'MasterItem', flex: 2, key: 'master' }, { caption: 'ItemCategory', flex: 2, key: 'cat' }
        ]);
    }

    var RecId = 0, UpdateMode = false, updateDetailIndex = -1, rights = {}, cfg = {}, fyStart = null;
    var L = { items: [], parties: [], currencies: [], rateBases: [], serviceTypes: [] };
    var dtItem = [], removedRows = [];

    function roundAway(v, d) { var p = Math.pow(10, d), x = Math.abs(v) * p; var r = Math.floor(x + 0.5 + 1e-9) / p; return v < 0 ? -r : r; }
    /** clsGlobalVariables.DecimalFCYRateFormate. */
    function fcyFmt(v) { var d = HRM.int(cfg.FcyRateDecimals) || 4; return HRM.fmtNum(roundAway(HRM.num(v), d), d); }

    function fill(id, rows, valueKey, textKey, zero, attrs) {
        var sel = HRM.$(id); if (!sel) return;
        var keep = sel.value;
        var html = zero === false ? '' : '<option value="0">' + HRM.esc(zero || '') + '</option>';
        (rows || []).forEach(function (r) {
            var extra = '';
            Object.keys(attrs || {}).forEach(function (a) { extra += ' data-' + a + '="' + HRM.esc(HRM.col(r, attrs[a])) + '"'; });
            html += '<option value="' + HRM.esc(HRM.col(r, valueKey)) + '"' + extra + '>' + HRM.esc(HRM.col(r, textKey)) + '</option>';
        });
        sel.innerHTML = html;
        sel._rows = rows || [];
        if (keep && keep !== '0' && HRM.hasOption(id, keep)) sel.value = keep; else sel.value = '0';
        HRM.refreshCombos();
    }
    function rowById(list, id) { for (var i = 0; i < list.length; i++) if (HRM.int(list[i].Id) === HRM.int(id)) return list[i]; return null; }

    /** AllLookUpComboBind (ServiceType, RateBaseUom) + currencies + parties + items + GetConfigurationsFromGlobalandBind. */
    function bindLists(d) {
        L.serviceTypes = d.serviceTypes || []; L.rateBases = d.rateBases || []; L.currencies = d.currencies || [];
        L.parties = d.parties || []; L.items = d.items || [];
        cfg = d.config || cfg;
        var hadType = HRM.comboVal('CmbServiceType');
        fill('CmbServiceType', L.serviceTypes, 'Id', 'Name', '');                                   // ServiceTypeBind (Rows[0] when nothing kept)
        if ((!hadType || !HRM.comboVal('CmbServiceType')) && L.serviceTypes.length) HRM.setCombo('CmbServiceType', L.serviceTypes[0].Id);
        fill('CmbRateBaseType', L.rateBases, 'Id', 'Name', '', { eq: 'Equivalent' });
        ['CmbTransactionCurrency', 'CmbFcyCurrency', 'CmbTransactionCurrencyDetail'].forEach(function (c) { fill(c, L.currencies, 'Id', 'CurrencyCode', '', { rate: 'CurrencyRate' }); });
        var party = { code: 'PartyCode', city: 'CityName', mobile: 'MobileNo' };
        fill('CmbBrokerAgentName', L.parties, 'Id', 'CompanyName', '', party);
        fill('CmbServiceProvider', L.parties, 'Id', 'CompanyName', '', party);
        ItemCategoryOrMasterItemBind();
        ItemDtsFillFromGlobal(0);
        ItemNameBind();
        var cur = HRM.int(cfg.ForeignBaseCurrency);                                                   // GetConfigurationsFromGlobalandBind
        if (cur > 0) {
            HRM.setCombo('CmbFcyCurrency', cur);
            if (!(HRM.num(HRM.val('txtFcyExchangeRate')) > 0)) HRM.setVal('txtFcyExchangeRate', fcyFmt(cfg.FcyBaseCurrencyRate));
        }
    }
    function ItemCategoryOrMasterItemBind() {
        if (!L.items.length) return;
        var cat = HRM.checked('RadCategory'), seen = {}, rows = [];
        L.items.forEach(function (r) {
            var name = cat ? HRM.str(r.ItemCategory) : HRM.str(r.ServicesMasterItem);
            if (!name || seen[name]) return;
            seen[name] = 1;
            rows.push({ Id: cat ? HRM.int(r.ItemCategoryId) : HRM.int(r.ServicesMasterItemId), Description: name });
        });
        fill('CmbMasterItem', rows, 'Id', 'Description', '');
    }
    function ItemDtsFillFromGlobal(id) {
        id = HRM.int(id);
        var cat = HRM.checked('RadCategory'), mas = HRM.checked('RadMasterItem');
        dtItem = L.items.filter(function (r) { return id === 0 || (cat && id === HRM.int(r.ItemCategoryId)) || (mas && id === HRM.int(r.ServicesMasterItemId)); });
    }
    function ItemNameBind() {
        var byName = HRM.checked('RadItemNamePackListDetail');
        fill('CmbItemName', dtItem, 'Id', byName ? 'ItemName' : 'ItemCode', '', { alt: byName ? 'ItemCode' : 'ItemName', master: 'ServicesMasterItem', cat: 'ItemCategory' });
    }

    // ------------------------------------------------------------------------ amounts
    function rateBaseEq(id) { var r = rowById(L.rateBases, id); return r ? HRM.num(r.Equivalent) : 0; }
    function CalculateAmountsInDetail() {
        var rate = HRM.num(HRM.val('txtServiceRate')), eq = HRM.comboVal('CmbRateBaseType') > 0 ? rateBaseEq(HRM.comboVal('CmbRateBaseType')) : 0;
        var tcy = rate * eq;
        HRM.setVal('txtTcyAmount', fcyFmt(tcy));
        var ex = HRM.num(HRM.val('txtTransactionExchangeRate'));
        HRM.setVal('txtLcyAmountDetail', fcyFmt(ex > 0 ? tcy * ex : 0));
    }
    function amountInRow(r) {                                                     // CustomerDetail_AmountUpdateInRow
        var tcy = HRM.num(r.ServiceRate) * HRM.num(r.RateBaseEquivalent);
        r.TcyAmount = tcy;
        r.LcyAmount = tcy * HRM.num(HRM.val('txtTransactionExchangeRate'));
    }
    function CmbTransactionCurrency_ValueChanged() {
        HRM.setCombo('CmbTransactionCurrencyDetail', HRM.comboVal('CmbTransactionCurrency'));
        CalculateAmountsInDetail();
        grdCustomerDetail.rows().forEach(function (r) {                            // AmountUpdateInCustomerDetailGrid
            amountInRow(r);
            r.TransactionCurrencyId = HRM.comboVal('CmbTransactionCurrency');
            r.TransactionCurrency = HRM.comboText('CmbTransactionCurrency');
        });
        grdCustomerDetail.draw();
        var cur = rowById(L.currencies, HRM.comboVal('CmbTransactionCurrencyDetail'));
        if (cur && HRM.num(cur.CurrencyRate) > 0 && !(HRM.num(HRM.val('txtTransactionExchangeRate')) > 0)) HRM.setVal('txtTransactionExchangeRate', fcyFmt(cur.CurrencyRate));
    }

    // ------------------------------------------------------------------------ detail grid
    function btnCell(act, text) { return '<button type="button" class="win-btn-action hrm-cell-btn" data-act="' + act + '">' + text + '</button>'; }
    function amt(v) { return HRM.esc(v === null || v === undefined || v === '' ? '' : fcyFmt(v)); }
    function detailColumns(editable) {                                           // grdCustomerDetailSetting
        var cols = [];
        if (editable) {
            cols.push({ key: '__del', caption: '', width: 20, render: function () { return btnCell('del', 'X'); } });
            cols.push({ key: '__edit', caption: '', width: 40, render: function () { return btnCell('edit', 'Edit'); } });
        }
        cols.push(
            { key: 'Id', caption: 'Id', hidden: true },
            { key: 'PartyRefDocNo', caption: 'PartyRefDocNo', width: 120 }, { key: 'PartyRefDocDate', caption: 'PartyRefDocDate', type: 'date', width: 100 },
            { key: 'ItemId', caption: 'ItemId', hidden: true }, { key: 'ItemCode', caption: 'ItemCode', width: 100 }, { key: 'ItemName', caption: 'ItemName', width: 160 },
            { key: 'TransactionCurrencyId', caption: 'TransactionCurrencyId', hidden: true }, { key: 'TransactionCurrency', caption: 'TransactionCurrency', width: 80 },
            editable ? { key: 'ServiceRate', caption: 'ServiceRate', type: 'edit-num', width: 80 } : { key: 'ServiceRate', caption: 'ServiceRate', type: 'num', decimals: 4, width: 80 },
            { key: 'RateBaseId', caption: 'RateBaseId', hidden: true }, { key: 'RateBase', caption: 'RateBase', width: 120 },
            { key: 'RateBaseEquivalent', caption: 'RateBaseEquivalent', type: 'num', decimals: 2, width: 90 },
            { key: 'TcyAmount', caption: 'TcyAmount', align: 'right', width: 90, render: amt, sum: true },
            { key: 'LcyAmount', caption: 'LcyAmount', align: 'right', width: 90, render: amt, sum: true },
            editable ? { key: 'Remarks', caption: 'Remarks', type: 'edit', width: 180 } : { key: 'Remarks', caption: 'Remarks', width: 180 });
        return cols;
    }
    var grdCustomerDetail = new HRM.Grid('grdCustomerDetail', {
        columns: detailColumns(true), totals: true,
        onDouble: function (r, i) { grdCustomerDetail_DoubleClick(i); },
        onChange: function (r, k) { if (k === 'ServiceRate' || k === 'RateBaseEquivalent') amountInRow(r); grdCustomerDetail.draw(); }   // grdCustomerDetail_CellUpdated
    });
    var grdHistoryDetail = new HRM.Grid('grdHistoryDetail', { columns: detailColumns(false), totals: true });
    HRM.$('grdCustomerDetail').addEventListener('click', function (e) {
        var b = e.target.closest('button[data-act]'); if (!b) return;
        var tr = b.closest('tr[data-i]'); if (!tr) return;
        var i = +tr.getAttribute('data-i');
        grdCustomerDetail.select(i);
        if (b.getAttribute('data-act') === 'del') DeleteCustomerDetailRow(i); else grdCustomerDetail_DoubleClick(i);
    });
    HRM.$('grdCustomerDetail').addEventListener('keydown', function (e) {
        if (e.ctrlKey && !e.shiftKey && e.key === 'Delete' && grdCustomerDetail.currentIndex() >= 0) { e.preventDefault(); e.stopPropagation(); DeleteCustomerDetailRow(grdCustomerDetail.currentIndex()); }
    });

    function FormValidationCustomerDetail() {
        var rules = [
            [!HRM.comboVal('CmbServiceProvider'), 'Service Provider field is required', 'CmbServiceProvider'],
            [!HRM.val('txtPartyRefDocNo').trim(), 'Party Ref Doc No field is required', 'txtPartyRefDocNo'],
            [!HRM.comboVal('CmbItemName'), 'Item Name field is required', 'CmbItemName'],
            [HRM.num(HRM.val('txtServiceRate')) === 0, 'Service Rate must be a non-zero number', 'txtServiceRate'],
            [!HRM.comboVal('CmbRateBaseType'), 'Rate Base field is required', 'CmbRateBaseType'],
            [HRM.num(HRM.val('txtTcyAmount')) === 0, 'Tcy Amount must be a non-zero number', 'txtTcyAmount'],
            [HRM.num(HRM.val('txtLcyAmountDetail')) === 0, 'Lcy Amount must be a non-zero number', 'txtLcyAmountDetail']
        ];
        for (var i = 0; i < rules.length; i++) if (rules[i][0]) { HRM.box(rules[i][1]); HRM.focus(rules[i][2]); return false; }
        return true;
    }
    function FillCustomerDetailRow(dr) {
        var item = rowById(L.items, HRM.comboVal('CmbItemName'));
        dr.PartyRefDocNo = HRM.val('txtPartyRefDocNo');
        dr.PartyRefDocDate = HRM.val('txtPartyRefDocDate') || HRM.today();
        dr.ItemId = HRM.comboVal('CmbItemName');
        dr.ItemName = dr.ItemId > 0 && item ? HRM.str(item.ItemName) : '';
        dr.ItemCode = dr.ItemId > 0 && item ? HRM.str(item.ItemCode) : '';
        dr.TransactionCurrencyId = HRM.comboVal('CmbTransactionCurrencyDetail'); dr.TransactionCurrency = HRM.comboText('CmbTransactionCurrencyDetail');
        dr.ServiceRate = HRM.num(HRM.val('txtServiceRate'));
        dr.RateBaseId = HRM.comboVal('CmbRateBaseType'); dr.RateBase = HRM.comboText('CmbRateBaseType');
        dr.RateBaseEquivalent = dr.RateBaseId > 0 ? rateBaseEq(dr.RateBaseId) : 0;
        dr.TcyAmount = HRM.num(HRM.val('txtTcyAmount'));
        dr.LcyAmount = HRM.num(HRM.val('txtLcyAmountDetail'));
        dr.Remarks = HRM.val('txtRemarksDetail');
        return dr;
    }
    function ResetCustomerDetail() {
        HRM.setVal('txtPartyRefDocNo', '');
        HRM.setCombo('CmbItemName', 0);
        HRM.setVal('txtServiceRate', '');
        HRM.setCombo('CmbRateBaseType', 0);
        ['txtTcyAmount', 'txtLcyAmountDetail', 'txtRemarksDetail'].forEach(function (c) { HRM.setVal(c, ''); });
        updateDetailIndex = -1;
        HRM.show('BtnAddCustomer', true); HRM.show('BtnCancelCustomer', false); HRM.show('BtnUpdateCustmer', false);
        HRM.focus('txtPartyRefDocNo');
    }
    function grdCustomerDetail_DoubleClick(i) {
        var r = grdCustomerDetail.rows()[i]; if (!r) return;
        updateDetailIndex = i;
        HRM.setCombo('CmbMasterItem', 0);
        ItemDtsFillFromGlobal(0);
        ItemNameBind();
        HRM.setVal('txtPartyRefDocNo', HRM.str(r.PartyRefDocNo));
        HRM.setVal('txtPartyRefDocDate', HRM.day(r.PartyRefDocDate));
        HRM.setCombo('CmbItemName', r.ItemId);
        HRM.setCombo('CmbTransactionCurrencyDetail', r.TransactionCurrencyId);
        HRM.setVal('txtServiceRate', HRM.str(r.ServiceRate));
        HRM.setCombo('CmbRateBaseType', r.RateBaseId);
        HRM.setVal('txtTcyAmount', HRM.str(r.TcyAmount));
        HRM.setVal('txtLcyAmountDetail', HRM.str(r.LcyAmount));
        HRM.setVal('txtRemarksDetail', HRM.str(r.Remarks));
        HRM.show('BtnAddCustomer', false); HRM.show('BtnUpdateCustmer', true); HRM.show('BtnCancelCustomer', true);
    }
    function DeleteCustomerDetailRow(i) {
        var r = grdCustomerDetail.rows()[i]; if (!r) return;
        if (updateDetailIndex !== -1) { HRM.box('Please Reset the Detail first..'); return; }
        if (HRM.int(r.Id) !== 0) {
            if (!HRM.ask('Are you sure to Delete?')) return;
            removedRows.push(JSON.parse(JSON.stringify(r)));                     // lstRemoveRecordcustomerDetail (ActionTypeId 3)
        }
        grdCustomerDetail.remove(i);
    }

    // ------------------------------------------------------------------------ form
    function buttons(mode) {
        HRM.show('btnsave', mode === 'new'); HRM.show('btnSaveAs', mode === 'saveas');
        HRM.show('btnUpdate', mode === 'edit'); HRM.show('btnDelete', mode === 'edit');
    }
    function FromReset() {
        RecId = 0; removedRows = [];
        buttons('new');
        UpdateMode = false;
        HRM.setVal('txtRemarksHeader', '');
        ResetCustomerDetail();
        grdCustomerDetail.clear();
        HRM.focus('txtdocdate');
        return HRM.get(API + '/doc-no').then(function (d) { HRM.setVal('txtdocno', HRM.str(d && d.docNo)); }).catch(HRM.fail);
    }
    function validateHeader() {
        if (!grdCustomerDetail.rows().length) { HRM.box('Detail Record Not Found'); return false; }
        var rules = [
            [HRM.int(HRM.val('txtdocno')) === 0, 'Doc No must be a non-zero number', 'txtdocno'],
            [!HRM.comboVal('CmbServiceType'), 'Service Type field is required', 'CmbServiceType'],
            [!HRM.comboVal('CmbBrokerAgentName'), 'Bill To Party field is required', 'CmbBrokerAgentName'],
            [!HRM.comboVal('CmbServiceProvider'), 'Broker Agent Or Service Provider field is required', 'CmbServiceProvider'],
            [!HRM.comboVal('CmbTransactionCurrency'), 'Transaction Currency field is required', 'CmbTransactionCurrency'],
            [HRM.num(HRM.val('txtTransactionExchangeRate')) === 0, 'Tcy Exchange Rate must be a non-zero number', 'txtTransactionExchangeRate'],
            [!HRM.comboVal('CmbFcyCurrency'), 'Foreign Currency field is required', 'CmbFcyCurrency'],
            [HRM.num(HRM.val('txtFcyExchangeRate')) === 0, 'Fcy Rate must be a non-zero number', 'txtFcyExchangeRate']
        ];
        for (var i = 0; i < rules.length; i++) if (rules[i][0]) { HRM.box(rules[i][1]); HRM.focus(rules[i][2]); return false; }
        return true;
    }
    function Insert(btn) {
        if (!validateHeader()) return;
        if (!HRM.ask(RecId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return;
        var preview = HRM.checked('ChkPreview') && rights.print;
        return HRM.busy(btn, function () {
            return HRM.post(API + '/save', {
                id: RecId, docDate: HRM.val('txtdocdate'), serviceTypeId: HRM.comboVal('CmbServiceType'),
                brokerAgentId: HRM.comboVal('CmbBrokerAgentName'), serviceProviderId: HRM.comboVal('CmbServiceProvider'),
                transactionCurrencyId: HRM.comboVal('CmbTransactionCurrency'), transactionExchangeRate: HRM.val('txtTransactionExchangeRate'),
                fcyCurrencyId: HRM.comboVal('CmbFcyCurrency'), fcyExchangeRate: HRM.val('txtFcyExchangeRate'),
                effectiveDateFrom: HRM.val('txtEffectiveDateFrom'), effectiveDateTo: HRM.val('txtEffectiveDateTo'),
                isActive: HRM.checked('chkIsActive'), remarksHeader: HRM.val('txtRemarksHeader'),
                rows: grdCustomerDetail.rows(), removedRows: removedRows
            }).then(function (d) {
                HRM.box(d && d.message ? d.message : 'Data Save Successfully....');
                var id = HRM.int(d && d.id);
                return FromReset().then(function () { if (preview) Slip(id, null); });
            }).catch(HRM.fail);
        }, 'ag-save');
    }
    function ReadById(id, edit) {
        return HRM.loading(HRM.get(API + '/by-id', { id: id, edit: edit !== false })).then(function (o) {
            return FromReset().then(function () {
                RecId = HRM.int(o.Id);
                showTab('tabPage1');
                UpdateMode = true;
                buttons('edit');
                HRM.setVal('txtdocdate', HRM.day(o.DocumentDate));
                HRM.setVal('txtdocno', HRM.str(o.DocumentNo));
                HRM.setCombo('CmbServiceType', o.serviceTypeId);
                HRM.setCombo('CmbBrokerAgentName', o.BrokerAgentId);
                HRM.setCombo('CmbServiceProvider', o.serviceProviderId);
                HRM.setCombo('CmbTransactionCurrency', o.transactionCurrencyId);
                CmbTransactionCurrency_ValueChanged();
                HRM.setVal('txtTransactionExchangeRate', fcyFmt(o.transactionExchangeRate));
                HRM.setCombo('CmbFcyCurrency', o.fcyCurrencyId);
                HRM.setVal('txtFcyExchangeRate', fcyFmt(o.fcyExchangeRate));
                if (o.effectiveDateFrom) HRM.setVal('txtEffectiveDateFrom', HRM.day(o.effectiveDateFrom));
                if (o.effectiveDateTo) HRM.setVal('txtEffectiveDateTo', HRM.day(o.effectiveDateTo));
                HRM.check('chkIsActive', HRM.bool(o.isActive));
                HRM.setVal('txtRemarksHeader', HRM.str(o.RemarksHeader));
                grdCustomerDetail.set(o.rows || []);
                HRM.focus('txtdocdate');
            });
        }).catch(HRM.fail);
    }
    function SaveAs(id) {                                                         // DataGridHistory_SaveAs
        return ReadById(id, false).then(function () {
            if (RecId !== id) return;
            buttons('saveas');
            HRM.setVal('txtdocdate', HRM.today());
        });
    }
    function Slip(id, btn) {                                                      // AgreementHeader_Slip -> AgreementHeader_Slip_1301
        var win = window.CrystalPrint ? window.CrystalPrint.reserve() : null;
        return HRM.busy(btn, function () {
            return HRM.get(API + '/slip-check', { id: id }).then(function () {
                if (!window.CrystalPrint) { HRM.box('Report 1301 is not available.'); return; }
                return window.CrystalPrint.open('lgsa-1301', { id: id }, null, win);
            }).catch(function (e) { if (win) window.CrystalPrint.release(win); HRM.fail(e); });
        }, 'ag-print');
    }

    // ------------------------------------------------------------------------ history
    function stamp(v) { return HRM.esc(HRM.fmtDateTime(v)); }
    var grdHistory = new HRM.Grid('grdHistory', {
        columns: [                                                                 // HistoryGridSetting
            { key: '__edit', caption: 'Edit', width: 50, render: function () { return btnCell('edit', 'Edit'); } },
            { key: '__saveas', caption: 'Save As', width: 60, render: function () { return btnCell('saveas', 'Save As'); } },
            { key: '__print', caption: 'Print', width: 50, render: function () { return btnCell('print', 'Print'); } },
            { key: 'Id', caption: 'Id', hidden: true }, { key: 'DocumentTypeId', caption: 'DocumentTypeId', hidden: true },
            { key: 'DocumentNo', caption: 'DocumentNo', type: 'int' }, { key: 'DocumentDate', caption: 'DocumentDate', type: 'date' },
            { key: 'ServiceTypeId', caption: 'ServiceTypeId', hidden: true }, { key: 'ServiceType', caption: 'ServiceType', width: 150 },
            { key: 'BillToPartyId', caption: 'BillToPartyId', hidden: true }, { key: 'BillToPartyName', caption: 'BillToPartyName' },
            { key: 'ServiceProviderId', caption: 'ServiceProviderId', hidden: true }, { key: 'BrokerAgentOrServiceProviderName', caption: 'BrokerAgentOrServiceProviderName' },
            { key: 'TransactionCurrencyId', caption: 'TransactionCurrencyId', hidden: true }, { key: 'TcyCode', caption: 'TcyCode', width: 60 },
            { key: 'TransactionExchangeRate', caption: 'TransactionExchangeRate', type: 'num', decimals: 4, width: 90 },
            { key: 'FcyCurrencyId', caption: 'FcyCurrencyId', hidden: true }, { key: 'FcyCode', caption: 'FcyCode', width: 60 },
            { key: 'FcyExchangeRate', caption: 'FcyExchangeRate', type: 'num', decimals: 4, width: 90 },
            { key: 'EffectiveDateFrom', caption: 'EffectiveDateFrom', type: 'date' }, { key: 'EffectiveDateTo', caption: 'EffectiveDateTo', type: 'date' },
            { key: 'IsActive', caption: 'IsActive', type: 'check' }, { key: 'RemarksHeader', caption: 'RemarksHeader', width: 180 },
            { key: 'EntryDate', caption: 'EntryDate', render: stamp }, { key: 'EntryUserName', caption: 'EntryUserName' },
            { key: 'ModifyDate', caption: 'ModifyDate', render: stamp }, { key: 'ModifyUserName', caption: 'ModifyUserName' },
            { key: 'IsApproved', caption: 'IsApproved', type: 'check' }, { key: 'ApprovedDate', caption: 'ApprovedDate', render: stamp },
            { key: 'ApprovalUserName', caption: 'ApprovalUserName' }, { key: 'NoOfAttachments', caption: 'NoOfAttachments', type: 'code' }
        ],
        filterRow: true,
        onDouble: function (r) { ReadById(HRM.int(r.Id), true); },
        onCode: function () { HRM.box('Attachments (DMS) are not ported on the web yet.'); },
        onSelect: function (r) { GetDetailGrdByHeadId(HRM.int(r.Id)); }
    });
    HRM.$('grdHistory').addEventListener('click', function (e) {
        var b = e.target.closest('button[data-act]'); if (!b) return;
        var tr = b.closest('tr[data-i]'); if (!tr) return;
        var r = grdHistory.rows()[+tr.getAttribute('data-i')]; if (!r) return;
        var act = b.getAttribute('data-act');
        if (act === 'print') Slip(HRM.int(r.Id), b);
        else if (act === 'edit') ReadById(HRM.int(r.Id), true);
        else if (act === 'saveas') SaveAs(HRM.int(r.Id));
    });
    var detSeq = 0;
    function GetDetailGrdByHeadId(id) {
        var seq = ++detSeq;
        return HRM.get(API + '/history-detail', { id: id }).then(function (rows) { if (seq === detSeq) grdHistoryDetail.set(rows || []); }).catch(HRM.fail);
    }
    function HistoryComboBind(h) {
        fill('CmbServiceTypeHistory', (h && h.serviceTypes) || [], 'Id', 'Name', '');
        fill('CmbServiceProviderHistory', (h && h.serviceProviders) || [], 'Id', 'Name', '');
    }
    function Datetypefill() {
        HRM.fillFixed('cmbDateTypeHistory', [[0, 'Select...'], [1, 'This Day'], [2, 'This Week'], [3, 'This Month'], [4, 'This Year'], [5, 'Financial Year']]);
        HRM.setCombo('cmbDateTypeHistory', 0);
    }
    function cmbDateTypeHistory_ValueChanged() {
        var v = HRM.comboVal('cmbDateTypeHistory'), t = HRM.today();
        if (v === 1) HRM.setVal('FromDateHistory', t);
        else if (v === 2) HRM.setVal('FromDateHistory', HRM.addDays(t, -7));
        else if (v === 3) { HRM.setVal('FromDateHistory', HRM.firstOfMonth(t)); HRM.setVal('ToDateHistory', t); }
        else if (v === 4) { HRM.setVal('FromDateHistory', t.slice(0, 4) + '-01-01'); HRM.setVal('ToDateHistory', t); }
        else if (v === 5 && fyStart) HRM.setVal('FromDateHistory', HRM.day(fyStart));
    }
    function HistoryGridFill(btn) {
        var kind = HRM.checked('rdentrydate') ? 'entry' : HRM.checked('rdmodifydate') ? 'modify' : HRM.checked('rdapproveddate') ? 'approved' : 'doc';
        return HRM.busy(btn || 'btnshow', function () {
            return HRM.get(API + '/history', {
                dateKind: kind,
                fromDate: HRM.checked('chkFromDateHistory') ? HRM.val('FromDateHistory') : null,
                toDate: HRM.checked('chkToDateHistory') ? HRM.val('ToDateHistory') : null,
                fromDocNo: HRM.val('FromDocNoHistory'), toDocNo: HRM.val('ToDocNoHistory'),
                serviceTypeId: HRM.comboVal('CmbServiceTypeHistory'), serviceProviderId: HRM.comboVal('CmbServiceProviderHistory')
            }).then(function (rows) { grdHistory.set(rows || []); grdHistoryDetail.clear(); }).catch(HRM.fail);
        });
    }

    function showTab(id) {
        document.querySelectorAll('.hrm-tab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-tab') === id); });
        document.querySelectorAll('.hrm-tab-page').forEach(function (p) { p.classList.toggle('is-active', p.id === id); });
        HRM.focus(id === 'tabPage3' ? 'cmbDateTypeHistory' : 'txtdocdate');
    }
    document.querySelectorAll('.hrm-tab').forEach(function (t) { t.addEventListener('click', function () { showTab(t.getAttribute('data-tab')); }); });
    function historyTab() { return HRM.$('tabPage3').classList.contains('is-active'); }

    // ------------------------------------------------------------------------ handlers
    P.btnsave = function (btn) { RecId = 0; return Insert(btn || 'btnsave'); };
    P.btnUpdate = function (btn) { return Insert(btn || 'btnUpdate'); };
    P.btnSaveAs = function (btn) { RecId = 0; return Insert(btn || 'btnSaveAs'); };
    P.btnnew = function () { return FromReset(); };
    P.btnDelete = function (btn) {
        if (RecId <= 0) { HRM.box('No record found to Delete'); return; }
        if (!HRM.ask('Are you sure to Delete?')) return;
        return HRM.busy(btn || 'btnDelete', function () {
            return HRM.post(API + '/delete?id=' + RecId, {}).then(function (d) {
                HRM.box(d && d.message ? d.message : 'Delete Record Successfully');
                return FromReset();
            }).catch(HRM.fail);
        });
    };
    P.btnRefresh = function (btn) {
        return HRM.busy(btn || 'btnRefresh', function () { return HRM.get(API + '/refresh').then(bindLists).catch(HRM.fail); });
    };
    P.btnPrint = function (btn) { return Slip(RecId, btn || 'btnPrint'); };
    P.BtnAddCustomer = function () {
        if (!FormValidationCustomerDetail()) return;
        grdCustomerDetail.add(FillCustomerDetailRow({ Id: 0 }));
        ResetCustomerDetail();
    };
    P.BtnUpdateCustmer = function () {
        if (!FormValidationCustomerDetail()) return;
        var rows = grdCustomerDetail.rows();
        if (updateDetailIndex < 0 || updateDetailIndex >= rows.length) return;
        FillCustomerDetailRow(rows[updateDetailIndex]);
        grdCustomerDetail.draw();
        ResetCustomerDetail();
    };
    P.BtnCancelCustomer = function () { ResetCustomerDetail(); };
    P.btnHistoryRefresh = function () {
        HRM.setCombo('cmbDateTypeHistory', 0); HRM.setCombo('CmbServiceTypeHistory', 0); HRM.setCombo('CmbServiceProviderHistory', 0);
        grdHistory.clear(); grdHistoryDetail.clear();
        HRM.focus('cmbDateTypeHistory');
    };
    P.BtnRefreshHistory = function (btn) {
        return HRM.busy(btn || 'BtnRefreshHistory', function () {
            Datetypefill();
            return HRM.get(API + '/history-combos').then(HistoryComboBind).catch(HRM.fail);
        });
    };
    P.btnshow = function (btn) { return HistoryGridFill(btn); };
    P.attachment = function () { HRM.box('Attachments (DMS) are not ported on the web yet.'); };
    P.shortcuts = function () {
        HRM.box(['Ctrl+S  For Save When on Entry form and For Show Data when on History Form', 'Ctrl+U  For Update', 'Ctrl+Shift+Delete  For Delete',
            'Ctrl+E  For Close', 'Ctrl+R  For Refresh', 'Ctrl+N  For New', 'Ctrl+P  For Print', 'Ctrl+F5  For Focus on Doc Date', 'Ctrl+F10  For Open Attachments',
            'F1  For Combo Lookup on Current Column of any Focused Grid', 'Ctrl+T  For Tab Transfer', 'Ctrl+alt  To Show ShortCut Keys Form',
            'Ctrl+D  For Adding an row in Focused Grid', 'Ctrl+Delete  For Deleting an row of Focused Grid', 'Ctrl+ArrowDown  For Focus On Detail Grid',
            'Ctrl+ArrowUp  For Focus On on Doc Date', 'Ctrl+ArrowRight  to change focus from one grid to another',
            'Ctrl+Enter  When Focus On Any Grid For Update Record', 'Ctrl+Space  When Focus On Any Grid To Call Function\'s On Button Or Link'].join('\n'));
    };

    HRM.$('CmbTransactionCurrency').addEventListener('change', CmbTransactionCurrency_ValueChanged);
    HRM.$('txtServiceRate').addEventListener('input', CalculateAmountsInDetail);
    HRM.$('CmbRateBaseType').addEventListener('change', CalculateAmountsInDetail);
    HRM.$('CmbMasterItem').addEventListener('change', function () { ItemDtsFillFromGlobal(HRM.comboVal('CmbMasterItem')); ItemNameBind(); });
    ['RadMasterItem', 'RadCategory'].forEach(function (id) {
        HRM.$(id).addEventListener('change', function () { ItemCategoryOrMasterItemBind(); ItemDtsFillFromGlobal(HRM.comboVal('CmbMasterItem')); ItemNameBind(); });
    });
    ['RadItemNamePackListDetail', 'RadItemCodePackListDetail'].forEach(function (id) { HRM.$(id).addEventListener('change', ItemNameBind); });
    HRM.$('cmbDateTypeHistory').addEventListener('change', cmbDateTypeHistory_ValueChanged);

    HRM.keys({
        'ctrl+t': function () { showTab(historyTab() ? 'tabPage1' : 'tabPage3'); },
        'ctrl+e': HRM.close, 'esc': HRM.close,
        'ctrl+alt+control': P.shortcuts, 'ctrl+alt+alt': P.shortcuts,
        'ctrl+shift+delete': function () { var b = HRM.$('btnDelete'); if (!historyTab() && rights['delete'] && !b.disabled && HRM.visible('btnDelete')) P.btnDelete(); },
        'ctrl+s': function () {
            if (historyTab()) { P.btnshow(); return; }
            var b = HRM.$('btnsave'); if (HRM.visible('btnsave') && !b.disabled) P.btnsave();
        },
        'ctrl+u': function () { var b = HRM.$('btnUpdate'); if (!historyTab() && HRM.visible('btnUpdate') && !b.disabled && UpdateMode) P.btnUpdate(); },
        'ctrl+p': function () { var b = HRM.$('btnPrint'); if (!historyTab() && !b.disabled) P.btnPrint(); },
        'ctrl+n': function () { if (!historyTab()) P.btnnew(); },
        'ctrl+r': function () { if (!historyTab()) P.btnRefresh(); },
        'ctrl+f5': function () { if (!historyTab()) HRM.focus('txtdocdate'); },
        'ctrl+f10': function () { if (!historyTab()) P.attachment(); },
        'ctrl+f12': function () { var b = HRM.$('btnSaveAs'); if (!historyTab() && HRM.visible('btnSaveAs') && !b.disabled) P.btnSaveAs(); },
        'ctrl+arrowdown': function () { var tr = document.querySelector((historyTab() ? '#grdHistoryDetail' : '#grdCustomerDetail') + ' tbody tr[data-i]'); if (tr) tr.focus(); },
        'ctrl+arrowup': function () {
            if (!historyTab()) { HRM.focus('txtdocdate'); return; }
            var tr = document.querySelector('#grdHistory tbody tr[data-i]'); if (tr) tr.focus();
        },
        'ctrl+enter': function () {
            var a = document.activeElement;
            if (a && a.closest('#grdCustomerDetail') && grdCustomerDetail.currentIndex() >= 0) { grdCustomerDetail_DoubleClick(grdCustomerDetail.currentIndex()); return; }
            if (a && a.closest('#grdHistory') && grdHistory.current()) ReadById(HRM.int(grdHistory.current().Id), true);
        }
    });
    HRM.footer(function () { showTab('tabPage3'); });

    HRM.setVal('txtdocdate', HRM.today());
    HRM.setVal('txtPartyRefDocDate', HRM.today());
    HRM.setVal('txtEffectiveDateFrom', HRM.today());
    HRM.setVal('txtEffectiveDateTo', HRM.today());
    HRM.loading(HRM.get(API + '/setup')).then(function (d) {
        rights = d.rights || {};
        cfg = d.config || {};
        fyStart = d.financialYearStart;
        HRM.applyRights(rights, { save: ['btnsave', 'btnSaveAs'], update: 'btnUpdate', 'delete': 'btnDelete', print: 'btnPrint' });
        HRM.setVal('txtdocno', HRM.str(d.docNo));
        bindLists(d);
        HistoryComboBind(d.history);
        Datetypefill();
        var days = HRM.int(cfg.DefaultDaysToLessFromHistoryFromDate);
        HRM.setVal('FromDateHistory', HRM.addDays(HRM.today(), days > 0 ? -days : -3));
        HRM.setVal('ToDateHistory', HRM.today());
        buttons('new');
        HRM.focus('txtdocdate');
        var id = HRM.int(HRM.param('id'));
        if (id > 0) ReadById(id, true);
    }).catch(HRM.fail);
})();
