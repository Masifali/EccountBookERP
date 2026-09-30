/* ============================================================================================
 * countx_logistics_rate_negotiation.js - 937 "Logistic Rate Negotiation and Comparisons"
 * (frmLogisticRateNegotiation.cs, DocumentTypeId 1300) and 941 "... For Transporter"
 * (frmLogisticRateNegotiationTransporter.cs, DocumentTypeId 1302). One script; <body data-page> picks the form.
 *
 * The transporter form has City From / City To in place of the ports, no vessel fields (routing type /
 * description, cargo cut-off, ETD, transit / free days), Service Type defaulted to 3, editable NoOfContainers /
 * MTon in the source-document grid with the invoice-balance checks, and no source-document / broker mismatch
 * checks on save. Every handler names the desktop method it follows; the server repeats every check.
 * ============================================================================================ */
(function () {
    'use strict';

    var PAGE = document.body.getAttribute('data-page');
    var TR = PAGE === 'rate-negotiation-transporter';
    var API = '/api/logistics/' + PAGE;
    var PRINT_KEY = TR ? 'lgsa-1302' : 'lgsa-1300';
    var P = {};
    window.LgsRN = P;

    if (window.DesktopCombo) {
        window.DesktopCombo.define('lgsaItem', [
            { caption: 'Item', flex: 4 }, { caption: 'Code / Name', flex: 2, key: 'alt' },
            { caption: 'MasterItem', flex: 2, key: 'master' }, { caption: 'ItemCategory', flex: 2, key: 'cat' }
        ]);
    }

    var RecId = 0, UpdateMode = false, updateDetailIndex = -1, rights = {}, cfg = {}, fyStart = null;
    var L = { items: [], parties: [], currencies: [], invoices: [], rateBases: [], paymentTerms: [], routingTypes: [], serviceTypes: [], dealStatuses: [] };
    var dtItem = [], removedRows = [], removedDocs = [];

    // ------------------------------------------------------------------------ number text
    function roundAway(v, d) { var p = Math.pow(10, d), x = Math.abs(v) * p; var r = Math.floor(x + 0.5 + 1e-9) / p; return v < 0 ? -r : r; }
    /** clsGlobalVariables.DecimalFCYRateFormate: "#,#0." + N zeros. */
    function fcyFmt(v) { var d = HRM.int(cfg.FcyRateDecimals) || 4; return HRM.fmtNum(roundAway(HRM.num(v), d), d); }

    // ------------------------------------------------------------------------ combos
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
    /** BindAndRetainSelection with ActivateRow (Rows[index]) when nothing is retained. */
    function fillActivate(id, rows, valueKey, textKey, attrs) {
        var had = HRM.comboVal(id);
        fill(id, rows, valueKey, textKey, '', attrs);
        if (!had || !HRM.comboVal(id)) { if (rows && rows.length) HRM.setCombo(id, HRM.col(rows[0], valueKey)); }
    }
    function rowById(list, id) { for (var i = 0; i < list.length; i++) if (HRM.int(list[i].Id) === HRM.int(id)) return list[i]; return null; }

    /** AllLookUpComboBind + PortsBind / CityDtFillFromGlobalAndBind + CurrencyBindFromGlobal + party / item binds. */
    function bindLists(d, first) {
        L.serviceTypes = d.serviceTypes || []; L.dealStatuses = d.dealStatuses || []; L.routingTypes = d.routingTypes || [];
        L.rateBases = d.rateBases || []; L.paymentTerms = d.paymentTerms || [];
        L.currencies = d.currencies || []; L.parties = d.parties || []; L.items = d.items || []; L.invoices = d.invoices || [];
        cfg = d.config || cfg;
        fillActivate('CmbServiceType', L.serviceTypes, 'Id', 'Name');                                 // ServiceTypeBind (Rows[0])
        if (TR && L.serviceTypes.length) HRM.setCombo('CmbServiceType', 3);                          // transporter: CmbServiceType.Value = 3
        fillActivate('CmbDealStatus', L.dealStatuses, 'Id', 'Name');                                  // DealStatusBind
        if (!TR) fill('CmbVesselRoutingType', L.routingTypes, 'Id', 'Name', '');                      // VesselRoutingTypeBind
        fill('CmbRateBaseType', L.rateBases, 'Id', 'Name', '', { eq: 'Equivalent' });               // RateBaseTypeTypeBind
        if (first) fillActivate('CmbPaymentTerm', L.paymentTerms, 'Id', 'Name');                      // PaymentTermsBind (default row, Rows[1])
        else fill('CmbPaymentTerm', L.paymentTerms, 'Id', 'Name', '');
        if (TR) { fill('CmbCityFrom', d.cities || [], 'Id', 'Description', ''); fill('CmbCityTo', d.cities || [], 'Id', 'Description', ''); }
        else { fill('CmbLoadingPort', d.ports || [], 'Id', 'PortName', ''); fill('CmbDestinationPort', d.ports || [], 'Id', 'PortName', ''); }
        ['CmbTransactionCurrency', 'CmbFcyCurrency', 'CmbTransactionCurrencyDetail'].forEach(function (c) { fill(c, L.currencies, 'Id', 'CurrencyCode', '', { rate: 'CurrencyRate' }); });
        var party = { code: 'PartyCode', city: 'CityName', mobile: 'MobileNo' };
        fill('CmbBrokerAgentName', L.parties, 'Id', 'CompanyName', '', party);
        fill('CmbServiceProvider', L.parties, 'Id', 'CompanyName', '', party);
        ItemCategoryOrMasterItemBind();
        ItemDtsFillFromGlobal(0);
        ItemNameBind();
        GetConfigurationsFromGlobalandBind();
        SourceDocumentGridcomboBind();
    }
    /** GetConfigurationsFromGlobalandBind: ForeignBaseCurrency / FcyBaseCurrencyRate. */
    function GetConfigurationsFromGlobalandBind() {
        var cur = HRM.int(cfg.ForeignBaseCurrency);
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

    // ------------------------------------------------------------------------ amounts / dates
    function rateBaseEq(id) { var r = rowById(L.rateBases, id); return r ? HRM.num(r.Equivalent) : 0; }
    /** CalculateAmountsInDetail(). */
    function CalculateAmountsInDetail() {
        var rate = HRM.num(HRM.val('txtServiceRate')), eq = HRM.comboVal('CmbRateBaseType') > 0 ? rateBaseEq(HRM.comboVal('CmbRateBaseType')) : 0;
        var tcy = rate * eq;
        HRM.setVal('txtTcyAmount', fcyFmt(tcy));
        var ex = HRM.num(HRM.val('txtTransactionExchangeRate'));
        HRM.setVal('txtLcyAmountDetail', fcyFmt(ex > 0 ? tcy * ex : 0));
    }
    /** CustomerDetail_AmountUpdateInRow(row): Tcy = rate x equivalent, Lcy = Tcy x exchange rate (not rounded). */
    function amountInRow(r) {
        var tcy = HRM.num(r.ServiceRate) * HRM.num(r.RateBaseEquivalent);
        r.TcyAmount = tcy;
        r.LcyAmount = tcy * HRM.num(HRM.val('txtTransactionExchangeRate'));
    }
    /** CmbTransactionCurrency_ValueChanged. */
    function CmbTransactionCurrency_ValueChanged() {
        HRM.setCombo('CmbTransactionCurrencyDetail', HRM.comboVal('CmbTransactionCurrency'));
        CalculateAmountsInDetail();
        grdCustomerDetail.rows().forEach(function (r) {                            // AmountUpdateInCustomerDetailGrid
            amountInRow(r);
            r.TransactionCurrencyId = HRM.comboVal('CmbTransactionCurrency');
            r.TransactionCurrency = HRM.comboText('CmbTransactionCurrency');
        });
        grdCustomerDetail.draw();
        SourceDocumentGridcomboBind();
        var cur = rowById(L.currencies, HRM.comboVal('CmbTransactionCurrencyDetail'));
        if (cur && HRM.num(cur.CurrencyRate) > 0 && !(HRM.num(HRM.val('txtTransactionExchangeRate')) > 0)) {
            HRM.setVal('txtTransactionExchangeRate', fcyFmt(cur.CurrencyRate));
        }
    }
    function docDate() { return HRM.val('txtdocdate') || HRM.today(); }
    function txtOfferValidityDays_TextChanged() { HRM.setVal('txtOfferValidityDate', HRM.addDays(docDate(), HRM.int(HRM.val('txtOfferValidityDays')))); }
    function txtOfferValidityDate_ValueChanged() { HRM.setVal('txtOfferValidityDays', String(HRM.daysBetween(docDate(), HRM.val('txtOfferValidityDate') || docDate()))); }

    // ------------------------------------------------------------------------ detail grid (grdCustomerDetail)
    function btnCell(act, text) { return '<button type="button" class="win-btn-action hrm-cell-btn" data-act="' + act + '">' + text + '</button>'; }
    function amt(v) { return HRM.esc(v === null || v === undefined || v === '' ? '' : fcyFmt(v)); }
    function detailColumns(editable) {
        var cols = [];
        if (editable) {
            cols.push({ key: '__del', caption: '', width: 20, render: function () { return btnCell('del', 'X'); } });
            cols.push({ key: '__edit', caption: '', width: 40, render: function () { return btnCell('edit', 'Edit'); } });
        }
        cols.push(
            { key: 'Id', caption: 'Id', hidden: true }, { key: 'BrokerAgentId', caption: 'BrokerAgentId', hidden: true },
            { key: 'BrokerAgent', caption: 'BrokerAgent', width: 150 },
            { key: 'ServiceProviderId', caption: 'ServiceProviderId', hidden: true }, { key: 'ServiceProviderName', caption: 'ServiceProviderName', width: 150 },
            { key: 'PaymentTermId', caption: 'PaymentTermId', hidden: true }, { key: 'PaymentTerm', caption: 'PaymentTerm', width: 100 },
            { key: 'DueDays', caption: 'DueDays', type: 'int', width: 70 });
        if (!TR) cols.push(
            { key: 'VesselRoutingTypeId', caption: 'VesselRoutingTypeId', hidden: true }, { key: 'VesselRoutingType', caption: 'VesselRoutingType', width: 120 },
            { key: 'VesselRoutingDescription', caption: 'VesselRoutingDescription' });
        cols.push(
            { key: 'ItemId', caption: 'ItemId', hidden: true }, { key: 'ItemCode', caption: 'ItemCode' }, { key: 'ItemName', caption: 'ItemName', width: 160 },
            { key: 'TransactionCurrencyId', caption: 'TransactionCurrencyId', hidden: true }, { key: 'TransactionCurrency', caption: 'TransactionCurrency', width: 80 },
            editable ? { key: 'ServiceRate', caption: 'ServiceRate', type: 'edit-num', width: 80 } : { key: 'ServiceRate', caption: 'ServiceRate', type: 'num', decimals: 4, width: 80 },
            { key: 'RateBaseId', caption: 'RateBaseId', hidden: true }, { key: 'RateBase', caption: 'RateBase', width: 120 },
            { key: 'RateBaseEquivalent', caption: 'RateBaseEquivalent', hidden: true },
            { key: 'TcyAmount', caption: 'TcyAmount', align: 'right', width: 90, render: amt, sum: true },
            { key: 'LcyAmount', caption: 'LcyAmount', align: 'right', width: 90, render: amt, sum: true },
            editable ? { key: 'OfferValidityDays', caption: 'OfferValidityDays', type: 'edit-num', width: 90 } : { key: 'OfferValidityDays', caption: 'OfferValidityDays', type: 'int', width: 90 },
            editable ? { key: 'OfferValidityDate', caption: 'OfferValidityDate', type: 'edit-date', width: 90 } : { key: 'OfferValidityDate', caption: 'OfferValidityDate', type: 'date', width: 90 });
        if (!TR) cols.push(
            { key: 'CargoCutOffDate', caption: 'CargoCutOffDate', type: 'date', width: 80 }, { key: 'EtdSealingDate', caption: 'EtdSealingDate', type: 'date', width: 80 },
            { key: 'TransitsTimeDays', caption: 'TransitsTimeDays', type: 'int', width: 90 }, { key: 'FreeDaysAtPOD', caption: 'FreeDaysAtPOD', type: 'int', width: 90 });
        cols.push(editable ? { key: 'Remarks', caption: 'Remarks', type: 'edit', width: 180 } : { key: 'Remarks', caption: 'Remarks', width: 180 });
        return cols;
    }
    var grdCustomerDetail = new HRM.Grid('grdCustomerDetail', {
        columns: detailColumns(true), totals: true,
        onDouble: function (r, i) { grdCustomerDetail_DoubleClick(i); },
        onChange: function (r, k) { grdCustomerDetail_CellUpdated(r, k); }
    });
    var grdHistoryDetail = new HRM.Grid('grdHistoryDetail', { columns: detailColumns(false), totals: true });

    /** grdCustomerDetail_CellUpdated. */
    function grdCustomerDetail_CellUpdated(r, key) {
        if (key === 'ServiceRate' || key === 'RateBaseEquivalent') amountInRow(r);
        else if (key === 'OfferValidityDays') r.OfferValidityDate = HRM.addDays(docDate(), HRM.int(r.OfferValidityDays));
        else if (key === 'OfferValidityDate') r.OfferValidityDays = HRM.daysBetween(docDate(), HRM.day(r.OfferValidityDate) || docDate());
        grdCustomerDetail.draw();
        SourceDocumentGridcomboBind();
    }
    HRM.$('grdCustomerDetail').addEventListener('click', function (e) {          // grdCustomerDetail_ColumnButtonClick
        var b = e.target.closest('button[data-act]'); if (!b) return;
        var tr = b.closest('tr[data-i]'); if (!tr) return;
        var i = +tr.getAttribute('data-i');
        grdCustomerDetail.select(i);
        if (b.getAttribute('data-act') === 'del') DeleteCustomerDetailRow(i);
        else grdCustomerDetail_DoubleClick(i);
    });

    function FormValidationCustomerDetail() {
        var rules = [
            [!HRM.comboVal('CmbBrokerAgentName'), 'Broker Agent field is required', 'CmbBrokerAgentName'],
            [!HRM.comboVal('CmbServiceProvider'), 'Service Provider field is required', 'CmbServiceProvider'],
            [!HRM.comboVal('CmbPaymentTerm'), 'PaymentTerm field is required', 'CmbPaymentTerm']
        ];
        if (!TR) rules.push([!HRM.comboVal('CmbVesselRoutingType'), 'Vessel Routing Type field is required', 'CmbVesselRoutingType']);
        rules.push(
            [!HRM.comboVal('CmbItemName'), 'Item Name field is required', 'CmbItemName'],
            [HRM.num(HRM.val('txtServiceRate')) === 0, 'Service Rate must be a non-zero number', 'txtServiceRate'],
            [!HRM.comboVal('CmbRateBaseType'), 'Rate Base field is required', 'CmbRateBaseType'],
            [HRM.num(HRM.val('txtTcyAmount')) === 0, 'Tcy Amount must be a non-zero number', 'txtTcyAmount'],
            [HRM.num(HRM.val('txtLcyAmountDetail')) === 0, 'Lcy Amount must be a non-zero number', 'txtLcyAmountDetail'],
            [HRM.int(HRM.val('txtOfferValidityDays')) === 0, 'Offer Validity Days must be a non-zero number', 'txtOfferValidityDays']);
        for (var i = 0; i < rules.length; i++) if (rules[i][0]) { HRM.box(rules[i][1]); HRM.focus(rules[i][2]); return false; }
        if (!TR && (HRM.val('txtCargoCutOffDate') || '') > (HRM.val('txtEtdSailing') || '')) {
            HRM.box('ETD/Sailing Date must be greater than or equal to Cargo Cut-Off Date.');
            HRM.focus('txtEtdSailing');
            return false;
        }
        return true;
    }
    /** FillCustomerDetailRow(dr) - note the desktop writes the HEADER remarks (txtRemarksHeader) into the row. */
    function FillCustomerDetailRow(dr) {
        var item = rowById(L.items, HRM.comboVal('CmbItemName'));
        dr.BrokerAgentId = HRM.comboVal('CmbBrokerAgentName'); dr.BrokerAgent = HRM.comboText('CmbBrokerAgentName');
        dr.ServiceProviderId = HRM.comboVal('CmbServiceProvider'); dr.ServiceProviderName = HRM.comboText('CmbServiceProvider');
        dr.PaymentTermId = HRM.comboVal('CmbPaymentTerm'); dr.PaymentTerm = HRM.comboText('CmbPaymentTerm');
        dr.DueDays = HRM.int(HRM.val('txtDueDays'));
        if (!TR) {
            dr.VesselRoutingTypeId = HRM.comboVal('CmbVesselRoutingType'); dr.VesselRoutingType = HRM.comboText('CmbVesselRoutingType');
            dr.VesselRoutingDescription = HRM.val('txtVesselRoutingDescription');
        }
        dr.ItemId = HRM.comboVal('CmbItemName');
        dr.ItemName = dr.ItemId > 0 && item ? HRM.str(item.ItemName) : '';
        dr.ItemCode = dr.ItemId > 0 && item ? HRM.str(item.ItemCode) : '';
        dr.TransactionCurrencyId = HRM.comboVal('CmbTransactionCurrencyDetail'); dr.TransactionCurrency = HRM.comboText('CmbTransactionCurrencyDetail');
        dr.ServiceRate = HRM.num(HRM.val('txtServiceRate'));
        dr.RateBaseId = HRM.comboVal('CmbRateBaseType'); dr.RateBase = HRM.comboText('CmbRateBaseType');
        dr.RateBaseEquivalent = dr.RateBaseId > 0 ? rateBaseEq(dr.RateBaseId) : 0;
        dr.TcyAmount = HRM.num(HRM.val('txtTcyAmount'));
        dr.LcyAmount = HRM.num(HRM.val('txtLcyAmountDetail'));
        dr.OfferValidityDays = HRM.int(HRM.val('txtOfferValidityDays'));
        dr.OfferValidityDate = HRM.val('txtOfferValidityDate') || docDate();
        if (!TR) {
            dr.CargoCutOffDate = HRM.val('txtCargoCutOffDate') || HRM.today();
            dr.EtdSealingDate = HRM.val('txtEtdSailing') || HRM.today();
            dr.TransitsTimeDays = HRM.int(HRM.val('txtTransitDays'));
            dr.FreeDaysAtPOD = HRM.int(HRM.val('txtFreeDaysAtPOD'));
        }
        dr.Remarks = HRM.val('txtRemarksHeader');
        return dr;
    }
    /** ResetCustomerDetail(). */
    function ResetCustomerDetail() {
        ['CmbBrokerAgentName', 'CmbServiceProvider', 'CmbPaymentTerm', 'CmbItemName', 'CmbRateBaseType'].forEach(function (c) { HRM.setCombo(c, 0); });
        ['txtServiceRate', 'txtTcyAmount', 'txtLcyAmountDetail', 'txtRemarksDetail'].forEach(function (c) { HRM.setVal(c, ''); });
        updateDetailIndex = -1;
        HRM.show('BtnAddCustomer', true); HRM.show('BtnCancelCustomer', false); HRM.show('BtnUpdateCustmer', false);
        HRM.focus('CmbBrokerAgentName');
    }
    /** grdCustomerDetail_DoubleClick: the row back into the entry boxes (and its remarks into the header remarks). */
    function grdCustomerDetail_DoubleClick(i) {
        var r = grdCustomerDetail.rows()[i]; if (!r) return;
        updateDetailIndex = i;
        HRM.setCombo('CmbMasterItem', 0);
        ItemDtsFillFromGlobal(0);
        ItemNameBind();
        HRM.setCombo('CmbBrokerAgentName', r.BrokerAgentId);
        HRM.setCombo('CmbServiceProvider', r.ServiceProviderId);
        HRM.setCombo('CmbPaymentTerm', r.PaymentTermId);
        HRM.setVal('txtDueDays', HRM.str(r.DueDays));
        if (!TR) { HRM.setCombo('CmbVesselRoutingType', r.VesselRoutingTypeId); HRM.setVal('txtVesselRoutingDescription', HRM.str(r.VesselRoutingDescription)); }
        HRM.setCombo('CmbItemName', r.ItemId);
        HRM.setCombo('CmbTransactionCurrencyDetail', r.TransactionCurrencyId);
        HRM.setVal('txtServiceRate', HRM.str(r.ServiceRate));
        HRM.setCombo('CmbRateBaseType', r.RateBaseId);
        HRM.setVal('txtTcyAmount', HRM.str(r.TcyAmount));
        HRM.setVal('txtLcyAmountDetail', HRM.str(r.LcyAmount));
        HRM.setVal('txtOfferValidityDays', HRM.str(r.OfferValidityDays));
        HRM.setVal('txtOfferValidityDate', HRM.day(r.OfferValidityDate));
        if (!TR) {
            HRM.setVal('txtCargoCutOffDate', HRM.day(r.CargoCutOffDate)); HRM.setVal('txtEtdSailing', HRM.day(r.EtdSealingDate));
            HRM.setVal('txtTransitDays', HRM.str(r.TransitsTimeDays)); HRM.setVal('txtFreeDaysAtPOD', HRM.str(r.FreeDaysAtPOD));
        }
        HRM.setVal('txtRemarksHeader', HRM.str(r.Remarks));
        HRM.show('BtnAddCustomer', false); HRM.show('BtnUpdateCustmer', true); HRM.show('BtnCancelCustomer', true);
    }
    /** DeleteCustomerDetailRow(r). */
    function DeleteCustomerDetailRow(i) {
        var r = grdCustomerDetail.rows()[i]; if (!r) return;
        if (updateDetailIndex !== -1) { HRM.box('Please Reset the Detail first..'); return; }
        if (HRM.int(r.Id) !== 0) {
            if (!HRM.ask('Are you sure to Delete?')) return;
            removedRows.push(JSON.parse(JSON.stringify(r)));                       // lstRemoveRecordcustomerDetail (ActionTypeId 3)
        }
        grdCustomerDetail.remove(i);
        SourceDocumentGridcomboBind();
    }

    // ------------------------------------------------------------------------ source documents (grdSourceDocument)
    var brokerOptions = [];
    /** FilldtBrokeragentFromDetailGrid: distinct broker agents of the detail grid. */
    function FilldtBrokeragentFromDetailGrid() {
        var seen = {}, out = [];
        grdCustomerDetail.rows().forEach(function (r) {
            var a = HRM.int(r.BrokerAgentId);
            if (seen[a]) return; seen[a] = 1;
            out.push({ BrokerAgentId: a, BrokerAgent: r.BrokerAgent, ServiceProviderId: HRM.int(r.ServiceProviderId), ServiceProviderName: r.ServiceProviderName });
        });
        brokerOptions = out;
    }
    function SourceDocumentGridcomboBind() {
        FilldtBrokeragentFromDetailGrid();
        if (grdSourceDocument) grdSourceDocument.draw();
    }
    var grdSourceDocument = new HRM.Grid('grdSourceDocument', {
        columns: [                                                                  // grdSourceDocumentSettings
            { key: '__del', caption: '', width: 20, render: function () { return btnCell('del', 'X'); } },
            { key: '__add', caption: '', width: 20, render: function () { return btnCell('add', '+'); } },
            { key: 'Id', caption: 'Id', hidden: true },
            { key: 'DocumentTypeId', caption: 'DocumentTypeId', hidden: true },
            { key: 'DocumentId', caption: 'Source Document No', type: 'select', width: 110,
                options: function () { return [[0, '']].concat(L.invoices.map(function (v) { return [v.Id, v.InvoiceNo]; })); } },
            TR ? { key: 'NoOfContainers', caption: 'NoOfContainers', type: 'edit-num', width: 70 } : { key: 'NoOfContainers', caption: 'NoOfContainers', type: 'int', width: 70 },
            TR ? { key: 'MTon', caption: 'MTon', type: 'edit-num', width: 70 } : { key: 'MTon', caption: 'MTon', type: 'num', decimals: 3, width: 70 },
            { key: 'BrokerAgentId', caption: 'Broker Agent', type: 'select', width: 110,
                options: function () { return [[0, '']].concat(brokerOptions.map(function (b) { return [b.BrokerAgentId, b.BrokerAgent]; })); } },
            { key: 'ServiceProviderId', caption: 'ServiceProviderId', hidden: true },
            { key: 'Remarks', caption: 'Remarks', type: 'edit', width: 120 }
        ],
        onChange: function (r, k, i) { grdSourceDocument_CellUpdated(r, k, i); }
    });
    function blankDoc() { return { Id: 0, DocumentTypeId: 0, DocumentId: 0, NoOfContainers: 0, MTon: 0, BrokerAgentId: 0, ServiceProviderId: 0, Remarks: '' }; }
    function AddRowInSourceDocumentGrid() { grdSourceDocument.add(blankDoc()); }
    function invoice(id) { for (var i = 0; i < L.invoices.length; i++) if (HRM.int(L.invoices[i].Id) === HRM.int(id)) return L.invoices[i]; return null; }
    /** GridEX_Helper.GetColumnSumWithoutIndex: the column's sum over the other rows of the same document. */
    function sumOthers(col, index, docId) {
        var s = 0;
        grdSourceDocument.rows().forEach(function (r, i) { if (i !== index && HRM.int(r.DocumentId) === HRM.int(docId)) s += HRM.num(r[col]); });
        return s;
    }
    /** grdSourceDocument_CellUpdated. */
    function grdSourceDocument_CellUpdated(r, key, index) {
        r[key] = key === 'DocumentId' || key === 'BrokerAgentId' ? HRM.int(r[key]) : r[key];
        var msg = null;
        if (key === 'DocumentId') {
            var docId = HRM.int(r.DocumentId), inv = invoice(docId);
            if (!TR) {
                var dup = grdSourceDocument.rows().some(function (o, i) { return i !== index && HRM.int(o.DocumentId) === docId; });
                if (dup) { r.DocumentId = 0; r.NoOfContainers = 0; r.MTon = 0; msg = 'This Document Already Exists in another row'; }
                else if (inv) { r.DocumentTypeId = HRM.int(inv.DocumentTypeId); r.NoOfContainers = HRM.int(inv.NoOfContainers); r.MTon = HRM.num(inv.MTon); }
            } else if (inv) {
                r.DocumentTypeId = HRM.int(inv.DocumentTypeId);
                var bc = HRM.int(inv.NoOfContainers) - HRM.int(sumOthers('NoOfContainers', index, docId));
                r.NoOfContainers = bc > 0 ? bc : 0;
                var bm = HRM.int(inv.MTon) - HRM.int(sumOthers('MTon', index, docId));
                r.MTon = bm > 0 ? bm : 0;
            }
        }
        if (TR && key === 'NoOfContainers') {
            var inv2 = invoice(r.DocumentId), tot = inv2 ? HRM.int(inv2.NoOfContainers) : 0, bal = tot - HRM.int(sumOthers('NoOfContainers', index, r.DocumentId));
            var n = HRM.int(r.NoOfContainers);
            if (n >= bal) { n = bal; msg = 'NoofContainers exceeds from Invoice NoofContainers'; }
            r.NoOfContainers = n;
        }
        if (TR && key === 'MTon') {
            var inv3 = invoice(r.DocumentId), totm = inv3 ? HRM.int(inv3.MTon) : 0, balm = totm - HRM.int(sumOthers('MTon', index, r.DocumentId));
            var m = HRM.int(r.MTon);
            if (m >= balm) { m = balm; msg = 'MTon exceeds from Invoice MTon'; }
            r.MTon = m;
        }
        if (key === 'BrokerAgentId') {
            for (var i = 0; i < brokerOptions.length; i++) if (brokerOptions[i].BrokerAgentId === HRM.int(r.BrokerAgentId)) { r.ServiceProviderId = brokerOptions[i].ServiceProviderId; break; }
        }
        grdSourceDocument.draw();
        if (msg) HRM.box(msg);
    }
    /** DeleteSourceDocumentDetailRow(r). */
    function DeleteSourceDocumentDetailRow(i) {
        var r = grdSourceDocument.rows()[i]; if (!r) return;
        if (HRM.int(r.Id) > 0) {
            if (!HRM.ask('Are you sure to Delete?')) return;
            removedDocs.push(JSON.parse(JSON.stringify(r)));                          // lstRemoveRecordSourceDocument (ActionTypeId 3)
        }
        grdSourceDocument.remove(i);
        if (!grdSourceDocument.rows().length) AddRowInSourceDocumentGrid();
    }
    HRM.$('grdSourceDocument').addEventListener('click', function (e) {          // grdSourceDocument_ColumnButtonClick
        var b = e.target.closest('button[data-act]'); if (!b) return;
        var tr = b.closest('tr[data-i]'); if (!tr) return;
        var i = +tr.getAttribute('data-i');
        if (b.getAttribute('data-act') === 'del') DeleteSourceDocumentDetailRow(i); else AddRowInSourceDocumentGrid();
    });
    HRM.$('grdSourceDocument').addEventListener('keydown', function (e) {        // grdSourceDocument_KeyDown
        if (!e.ctrlKey) return;
        var i = grdSourceDocument.currentIndex();
        if ((e.key || '').toLowerCase() === 'd') { e.preventDefault(); AddRowInSourceDocumentGrid(); }
        else if (e.key === 'Delete' && i >= 0) { e.preventDefault(); e.stopPropagation(); DeleteSourceDocumentDetailRow(i); }
    });
    HRM.$('grdCustomerDetail').addEventListener('keydown', function (e) {        // grdCustomerDetail_KeyDown Ctrl+Delete
        if (e.ctrlKey && !e.shiftKey && e.key === 'Delete' && grdCustomerDetail.currentIndex() >= 0) { e.preventDefault(); e.stopPropagation(); DeleteCustomerDetailRow(grdCustomerDetail.currentIndex()); }
    });

    // ------------------------------------------------------------------------ form
    function buttons(mode) {
        HRM.show('btnsave', mode === 'new'); HRM.show('btnSaveAs', mode === 'saveas');
        HRM.show('btnUpdate', mode === 'edit'); HRM.show('btnDelete', mode === 'edit');
    }
    function DocumentNoDbCall() { return HRM.get(API + '/doc-no').then(function (d) { HRM.setVal('txtdocno', HRM.str(d && d.docNo)); }).catch(HRM.fail); }
    /** FromReset(). */
    function FromReset() {
        RecId = 0; removedRows = []; removedDocs = [];
        buttons('new');
        UpdateMode = false;
        HRM.enable('CmbDealStatus', false);
        if (TR) { HRM.setCombo('CmbCityTo', 0); HRM.setCombo('CmbCityFrom', 0); }
        else { HRM.setCombo('CmbLoadingPort', 0); HRM.setCombo('CmbDestinationPort', 0); }
        HRM.setVal('txtRemarksHeader', '');
        ResetCustomerDetail();
        grdCustomerDetail.clear();
        grdSourceDocument.set([blankDoc()]);
        SourceDocumentGridcomboBind();
        HRM.focus('txtdocdate');
        return DocumentNoDbCall();
    }

    function validateHeader() {
        if (!grdCustomerDetail.rows().length) { HRM.box('Broker Agent Detail Record Not Found'); return false; }
        if (!TR && !grdSourceDocument.rows().length) { HRM.box('Source Document Detail Record Not Found'); return false; }
        var rules = [
            [HRM.int(HRM.val('txtdocno')) === 0, 'Doc No must be a non-zero number', 'txtdocno'],
            [!HRM.comboVal('CmbServiceType'), 'Service Type field is required', 'CmbServiceType']
        ];
        if (TR) rules.push([!HRM.comboVal('CmbCityFrom'), 'City From field is required', 'CmbCityFrom'], [!HRM.comboVal('CmbCityTo'), 'City To field is required', 'CmbCityTo']);
        else rules.push([!HRM.comboVal('CmbDestinationPort'), 'Destination Port field is required', 'CmbDestinationPort'], [!HRM.comboVal('CmbLoadingPort'), 'Loading Port field is required', 'CmbLoadingPort']);
        rules.push(
            [!HRM.comboVal('CmbTransactionCurrency'), 'Transaction Currency field is required', 'CmbTransactionCurrency'],
            [HRM.num(HRM.val('txtTransactionExchangeRate')) === 0, 'Tcy Exchange Rate must be a non-zero number', 'txtTransactionExchangeRate'],
            [!HRM.comboVal('CmbFcyCurrency'), 'Foreign Currency field is required', 'CmbFcyCurrency'],
            [HRM.num(HRM.val('txtFcyExchangeRate')) === 0, 'Fcy Rate must be a non-zero number', 'txtFcyExchangeRate']);
        for (var i = 0; i < rules.length; i++) if (rules[i][0]) { HRM.box(rules[i][1]); HRM.focus(rules[i][2]); return false; }
        return true;
    }

    /** Insert(). */
    function Insert(btn) {
        if (!validateHeader()) return;
        if (!HRM.ask(RecId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return;
        var preview = HRM.checked('ChkPreview') && rights.print;
        var body = {
            id: RecId, docDate: HRM.val('txtdocdate'), serviceTypeId: HRM.comboVal('CmbServiceType'),
            transactionCurrencyId: HRM.comboVal('CmbTransactionCurrency'), transactionExchangeRate: HRM.val('txtTransactionExchangeRate'),
            fcyCurrencyId: HRM.comboVal('CmbFcyCurrency'), fcyExchangeRate: HRM.val('txtFcyExchangeRate'),
            dealStatusId: HRM.comboVal('CmbDealStatus'), remarksHeader: HRM.val('txtRemarksHeader'),
            rows: grdCustomerDetail.rows(), removedRows: removedRows,
            sourceDocuments: grdSourceDocument.rows(), removedSourceDocuments: removedDocs
        };
        if (TR) { body.cityFromId = HRM.comboVal('CmbCityFrom'); body.cityToId = HRM.comboVal('CmbCityTo'); }
        else { body.destinationPortId = HRM.comboVal('CmbDestinationPort'); body.loadingPortId = HRM.comboVal('CmbLoadingPort'); }
        return HRM.busy(btn, function () {
            return HRM.post(API + '/save', body).then(function (d) {
                HRM.box(d && d.message ? d.message : 'Data Save Successfully....');
                var id = HRM.int(d && d.id);
                return FromReset().then(function () { if (preview) Slip(id, null); });
            }).catch(HRM.fail);
        }, 'rn-save');
    }

    /** ReadById(Id). edit=false for Save As (no Update-right check). */
    function ReadById(id, edit) {
        return HRM.loading(HRM.get(API + '/by-id', { id: id, edit: edit !== false })).then(function (o) {
            return FromReset().then(function () {
                RecId = HRM.int(o.Id);
                showTab('tabPage1');
                UpdateMode = true;
                buttons('edit');
                HRM.setVal('txtdocdate', HRM.day(o.documentDate));
                HRM.setVal('txtdocno', HRM.str(o.documentNo));
                HRM.setCombo('CmbServiceType', o.ServiceTypeId);
                var first = (o.rows || [])[0] || {};
                if (TR) { HRM.setCombo('CmbCityFrom', first.CityFromId); HRM.setCombo('CmbCityTo', first.CityToId); }
                else { HRM.setCombo('CmbDestinationPort', first.portOfDischargeId); HRM.setCombo('CmbLoadingPort', first.portOfLoadingId); }
                HRM.setCombo('CmbTransactionCurrency', o.transactionCurrencyId);
                CmbTransactionCurrency_ValueChanged();                                   // Value = ... fires ValueChanged
                HRM.setVal('txtTransactionExchangeRate', fcyFmt(o.transactionExchangeRate));
                HRM.setCombo('CmbFcyCurrency', o.fcyCurrencyId);
                HRM.setVal('txtFcyExchangeRate', fcyFmt(o.fcyExchangeRate));
                HRM.setCombo('CmbDealStatus', o.DealStatusId);
                HRM.setVal('txtRemarksHeader', HRM.str(o.RemarksHeader));
                HRM.enable('CmbDealStatus', HRM.bool(o.IsApproved));
                grdCustomerDetail.set(o.rows || []);
                var docs = o.sourceDocuments || [];
                grdSourceDocument.set(docs.length ? docs : [blankDoc()]);
                SourceDocumentGridcomboBind();
                HRM.focus('txtdocdate');
            });
        }).catch(HRM.fail);
    }
    /** DataGridHistory_SaveAs(CurrId). */
    function SaveAs(id) {
        return ReadById(id, false).then(function () {
            if (RecId !== id) return;
            buttons('saveas');
            HRM.setVal('txtdocdate', HRM.today());
        });
    }

    /** LogisticRateNegotiationSlip(PrintId). */
    function Slip(id, btn) {
        var win = window.CrystalPrint ? window.CrystalPrint.reserve() : null;
        return HRM.busy(btn, function () {
            return HRM.get(API + '/slip-check', { id: id }).then(function () {
                if (!window.CrystalPrint) { HRM.box('Report is not available.'); return; }
                return window.CrystalPrint.open(PRINT_KEY, { id: id }, null, win);
            }).catch(function (e) { if (win) window.CrystalPrint.release(win); HRM.fail(e); });
        }, 'rn-print');
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
            { key: 'DealStatusId', caption: 'DealStatusId', hidden: true }, { key: 'DealStatus', caption: 'DealStatus', width: 120 },
            { key: 'TransactionCurrencyId', caption: 'TransactionCurrencyId', hidden: true }, { key: 'TcyCode', caption: 'TcyCode', width: 60 },
            { key: 'TransactionExchangeRate', caption: 'TransactionExchangeRate', type: 'num', decimals: 4, width: 90 },
            { key: 'FcyCurrencyId', caption: 'FcyCurrencyId', hidden: true }, { key: 'FcyCode', caption: 'FcyCode', width: 60 },
            { key: 'FcyExchangeRate', caption: 'FcyExchangeRate', type: 'num', decimals: 4, width: 90 },
            { key: 'RemarksHeader', caption: 'RemarksHeader', width: 180 },
            { key: 'EntryDate', caption: 'EntryDate', render: stamp }, { key: 'EntryUserName', caption: 'EntryUserName' },
            { key: 'ModifyDate', caption: 'ModifyDate', render: stamp }, { key: 'ModifyUserName', caption: 'ModifyUserName' },
            { key: 'IsApproved', caption: 'IsApproved', type: 'check' }, { key: 'ApprovedDate', caption: 'ApprovedDate', render: stamp },
            { key: 'ApprovalUserName', caption: 'ApprovalUserName' },
            { key: 'NoOfAttachments', caption: 'NoOfAttachments', type: 'code' }
        ],
        filterRow: true,
        onDouble: function (r) { ReadById(HRM.int(r.Id), true); },                 // DatagridHistory_DoubleClick
        onCode: function () { HRM.box('Attachments (DMS) are not ported on the web yet.'); },
        onSelect: function (r) { GetDetailGrdByHeadId(HRM.int(r.Id)); }            // DatagridHistory_SelectionChanged
    });
    HRM.$('grdHistory').addEventListener('click', function (e) {                  // DatagridHistory_ColumnButtonClick
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
    /** HistoryComboBind (default row). */
    function HistoryComboBind(h) {
        fill('CmbServiceTypeHistory', (h && h.serviceTypes) || [], 'Id', 'Name', '');
        fill('CmbDealStatusHistory', (h && h.dealStatuses) || [], 'Id', 'Name', '');
    }
    /** Datetypefill -> InfragisticsHelper.BindComboDateType. */
    function Datetypefill() {
        HRM.fillFixed('cmbDateTypeHistory', [[0, 'Select...'], [1, 'This Day'], [2, 'This Week'], [3, 'This Month'], [4, 'This Year'], [5, 'Financial Year']]);
        HRM.setCombo('cmbDateTypeHistory', 0);
    }
    /** cmbDateTypeHistory_ValueChanged. */
    function cmbDateTypeHistory_ValueChanged() {
        var v = HRM.comboVal('cmbDateTypeHistory'), t = HRM.today();
        if (v === 1) HRM.setVal('FromDateHistory', t);
        else if (v === 2) HRM.setVal('FromDateHistory', HRM.addDays(t, -7));
        else if (v === 3) { HRM.setVal('FromDateHistory', HRM.firstOfMonth(t)); HRM.setVal('ToDateHistory', t); }
        else if (v === 4) { HRM.setVal('FromDateHistory', t.slice(0, 4) + '-01-01'); HRM.setVal('ToDateHistory', t); }
        else if (v === 5 && fyStart) HRM.setVal('FromDateHistory', HRM.day(fyStart));
    }
    /** HistoryGridFill(). */
    function HistoryGridFill(btn) {
        var kind = HRM.checked('rdentrydate') ? 'entry' : HRM.checked('rdmodifydate') ? 'modify' : HRM.checked('rdapproveddate') ? 'approved' : 'doc';
        return HRM.busy(btn || 'btnshow', function () {
            return HRM.get(API + '/history', {
                dateKind: kind,
                fromDate: HRM.checked('chkFromDateHistory') ? HRM.val('FromDateHistory') : null,
                toDate: HRM.checked('chkToDateHistory') ? HRM.val('ToDateHistory') : null,
                fromDocNo: HRM.val('FromDocNoHistory'), toDocNo: HRM.val('ToDocNoHistory'),
                serviceTypeId: HRM.comboVal('CmbServiceTypeHistory'), dealStatusId: HRM.comboVal('CmbDealStatusHistory')
            }).then(function (rows) { grdHistory.set(rows || []); grdHistoryDetail.clear(); }).catch(HRM.fail);
        });
    }

    // ------------------------------------------------------------------------ tabs
    function showTab(id) {
        document.querySelectorAll('.hrm-tab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-tab') === id); });
        document.querySelectorAll('.hrm-tab-page').forEach(function (p) { p.classList.toggle('is-active', p.id === id); });
        HRM.focus(id === 'tabPage3' ? 'cmbDateTypeHistory' : 'txtdocdate');      // tabControl1_SelectedIndexChanged
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
    P.btnRefresh = function (btn) {                                                // btnRefresh_Click
        return HRM.busy(btn || 'btnRefresh', function () {
            return HRM.get(API + '/refresh').then(function (d) { bindLists(d, false); }).catch(HRM.fail);
        });
    };
    P.btnPrint = function (btn) { return Slip(RecId, btn || 'btnPrint'); };
    P.BtnAddCustomer = function () {                                               // BtnAddCustomer_Click
        if (!FormValidationCustomerDetail()) return;
        grdCustomerDetail.add(FillCustomerDetailRow({ Id: 0 }));
        ResetCustomerDetail();
        SourceDocumentGridcomboBind();
    };
    P.BtnUpdateCustmer = function () {                                             // BtnUpdateCustmer_Click
        if (!FormValidationCustomerDetail()) return;
        var rows = grdCustomerDetail.rows();
        if (updateDetailIndex < 0 || updateDetailIndex >= rows.length) return;
        FillCustomerDetailRow(rows[updateDetailIndex]);
        grdCustomerDetail.draw();
        ResetCustomerDetail();
        SourceDocumentGridcomboBind();
    };
    P.BtnCancelCustomer = function () { ResetCustomerDetail(); };
    P.btnHistoryRefresh = function () {                                           // btnHistoryRefresh_Click (History "New")
        HRM.setCombo('cmbDateTypeHistory', 0); HRM.setCombo('CmbServiceTypeHistory', 0); HRM.setCombo('CmbDealStatusHistory', 0);
        grdHistory.clear(); grdHistoryDetail.clear();
        HRM.focus('cmbDateTypeHistory');
    };
    P.BtnRefreshHistory = function (btn) {                                        // BtnRefreshHistory_Click
        return HRM.busy(btn || 'BtnRefreshHistory', function () {
            Datetypefill();
            return HRM.get(API + '/history-combos').then(HistoryComboBind).catch(HRM.fail);
        });
    };
    P.btnshow = function (btn) { return HistoryGridFill(btn); };
    P.attachment = function () { HRM.box('Attachments (DMS) are not ported on the web yet.'); };
    P.referenceParty = function () { HRM.open('/accounts/supplier'); };          // BtnReferenceParty_Click -> supfrmDefineSupplier
    P.shortcuts = function () {                                                    // MakeShortCutKeys
        HRM.box(['Ctrl+S  For Save When on Entry form and For Show Data when on History Form', 'Ctrl+U  For Update', 'Ctrl+Shift+Delete  For Delete',
            'Ctrl+E  For Close', 'Ctrl+R  For Refresh', 'Ctrl+N  For New', 'Ctrl+P  For Print', 'Ctrl+F5  For Focus on Doc Date', 'Ctrl+F10  For Open Attachments',
            'F1  For Combo Lookup on Current Column of any Focused Grid', 'Ctrl+T  For Tab Transfer', 'Ctrl+alt  To Show ShortCut Keys Form',
            'Ctrl+D  For Adding an row in Focused Grid', 'Ctrl+Delete  For Deleting an row of Focused Grid', 'Ctrl+ArrowDown  For Focus On Detail Grid',
            'Ctrl+ArrowUp  For Focus On on Doc Date', 'Ctrl+ArrowRight  to change focus from one grid to another',
            'Ctrl+Enter  When Focus On Any Grid For Update Record', 'Ctrl+Space  When Focus On Any Grid To Call Function\'s On Button Or Link'].join('\n'));
    };

    // ------------------------------------------------------------------------ events
    HRM.$('CmbTransactionCurrency').addEventListener('change', CmbTransactionCurrency_ValueChanged);
    HRM.$('txtServiceRate').addEventListener('input', CalculateAmountsInDetail);                  // txtServiceRate_TextChanged
    HRM.$('CmbRateBaseType').addEventListener('change', CalculateAmountsInDetail);                // CmbRateBaseType_Leave / ValueChanged
    HRM.$('txtOfferValidityDays').addEventListener('input', txtOfferValidityDays_TextChanged);
    HRM.$('txtOfferValidityDate').addEventListener('change', txtOfferValidityDate_ValueChanged);
    HRM.$('CmbMasterItem').addEventListener('change', function () { ItemDtsFillFromGlobal(HRM.comboVal('CmbMasterItem')); ItemNameBind(); });
    ['RadMasterItem', 'RadCategory'].forEach(function (id) {
        HRM.$(id).addEventListener('change', function () { ItemCategoryOrMasterItemBind(); ItemDtsFillFromGlobal(HRM.comboVal('CmbMasterItem')); ItemNameBind(); });
    });
    ['RadItemNamePackListDetail', 'RadItemCodePackListDetail'].forEach(function (id) { HRM.$(id).addEventListener('change', ItemNameBind); });
    HRM.$('cmbDateTypeHistory').addEventListener('change', cmbDateTypeHistory_ValueChanged);

    HRM.keys({                                                                    // frmBillOfMaterial_Manufacturing_KeyDown
        'ctrl+t': function () { showTab(historyTab() ? 'tabPage1' : 'tabPage3'); },
        'ctrl+e': HRM.close, 'esc': HRM.close,
        'ctrl+alt+control': P.shortcuts, 'ctrl+alt+alt': P.shortcuts,
        'ctrl+shift+delete': function () { var b = HRM.$('btnDelete'); if (!historyTab() && rights['delete'] && !b.disabled && HRM.visible('btnDelete')) P.btnDelete(); },
        'ctrl+s': function () {
            if (historyTab()) { P.btnshow(); return; }
            var b = HRM.$('btnsave'); if (HRM.visible('btnsave') && !b.disabled) P.btnsave();
        },
        'ctrl+u': function () { var b = HRM.$('btnUpdate'); if (!historyTab() && HRM.visible('btnUpdate') && !b.disabled && UpdateMode) P.btnUpdate(); },
        'ctrl+p': function () { var b = HRM.$('btnPrint'); if (!historyTab() && HRM.visible('btnPrint') && !b.disabled) P.btnPrint(); },
        'ctrl+n': function () { if (!historyTab()) P.btnnew(); },
        'ctrl+r': function () { if (!historyTab()) P.btnRefresh(); },
        'ctrl+f5': function () { if (!historyTab()) HRM.focus('txtdocdate'); },
        'ctrl+f10': function () { if (!historyTab()) P.attachment(); },
        'ctrl+f12': function () { var b = HRM.$('btnSaveAs'); if (!historyTab() && HRM.visible('btnSaveAs') && !b.disabled) P.btnSaveAs(); },
        'ctrl+arrowdown': function () {
            var tr = document.querySelector((historyTab() ? '#grdHistoryDetail' : '#grdCustomerDetail') + ' tbody tr[data-i]'); if (tr) tr.focus();
        },
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

    // ------------------------------------------------------------------------ load
    HRM.setVal('txtdocdate', HRM.today());
    HRM.setVal('txtOfferValidityDate', HRM.today());
    if (!TR) { HRM.setVal('txtCargoCutOffDate', HRM.today()); HRM.setVal('txtEtdSailing', HRM.today()); }
    HRM.loading(HRM.get(API + '/setup')).then(function (d) {                    // InitializeComponentMethod
        rights = d.rights || {};
        cfg = d.config || {};
        fyStart = d.financialYearStart;
        HRM.applyRights(rights, { save: ['btnsave', 'btnSaveAs'], update: 'btnUpdate', 'delete': 'btnDelete', print: 'btnPrint' });
        HRM.setVal('txtdocno', HRM.str(d.docNo));
        bindLists(d, true);
        grdSourceDocument.set([blankDoc()]);                                        // AddRowInSourceDocumentGrid
        SourceDocumentGridcomboBind();
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
