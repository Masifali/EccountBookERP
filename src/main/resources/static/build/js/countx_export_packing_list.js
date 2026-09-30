/* ============================================================================================
 * countx_export_packing_list.js - frmExportInvoicePackingList.cs (Architecture.WinApp.Export), screen 882
 * "Export Invoice Packing List". Form | History. Every button, Leave, grid edit, F1 picker, grid button,
 * double-click and shortcut of the desktop form has its counterpart here, with the desktop's messages
 * and order; all data comes from /api/export/packing-list (ExportModuleController ->
 * ExportInvoicePackingListService -> the desktop's own procedures). Prints go through CrystalPrint
 * (/api/reports/exp-529A|B|C/print.pdf).
 *
 * Button contract on every action: disabled + spinner while the request runs, duplicates ignored,
 * re-enabled on success and on failure (busy()).
 * ============================================================================================ */
(function (global) {
    'use strict';

    var API = '/api/export/packing-list';
    var DOCUMENT_TYPE_ID = 204;

    function $id(id) { return document.getElementById(id); }
    function box(m) { global.alert(m); }
    function ask(m) { return global.confirm(m); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function str(v) { return v === null || v === undefined ? '' : String(v); }
    function netI(v) { var n = parseInt(String(v === null || v === undefined ? '' : v).replace(/,/g, ''), 10); return isFinite(n) ? n : 0; }
    function netD(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isFinite(n) ? n : 0; }
    function fmt(v, dec) {
        var n = netD(v);
        if (dec === undefined) dec = 3;
        var s = n.toFixed(dec);
        if (dec > 0) s = s.replace(/\.?0+$/, '');
        var parts = s.split('.');
        parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        return parts.join('.');
    }
    function fmt4(v) { return fmt(v, 4); }   /* ToString("#,##0.####") on the header */
    function fmt2(v) { return fmt(v, 2); }
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function today() { var d = new Date(); return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function daysAgo(n) { var d = new Date(); d.setDate(d.getDate() - n); return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    function isoDate(v) {
        var s = str(v).trim();
        if (!s) return '';
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(s);
        if (m) return m[1] + '-' + m[2] + '-' + m[3];
        var d = new Date(s);
        return isNaN(d.getTime()) ? '' : d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate());
    }
    /* "dd-MMM-yyyy" (the history DocDate). */
    function ddMMMyyyy(v) {
        var s = isoDate(v); if (!s) return '';
        return s.substring(8, 10) + '-' + MON[parseInt(s.substring(5, 7), 10) - 1] + '-' + s.substring(0, 4);
    }
    /* "dd-MM-yyyy hh:mm tt" (EntryDate / ModifyDate FormatString). */
    function ddMMyyyyHm(v) {
        var s = str(v).trim(); if (!s) return '';
        var m = /^(\d{4})-(\d{2})-(\d{2})[T ](\d{2}):(\d{2})/.exec(s);
        if (!m) return ddMMMyyyy(s);
        var h = parseInt(m[4], 10), tt = h >= 12 ? 'PM' : 'AM'; h = h % 12; if (h === 0) h = 12;
        return m[3] + '-' + m[2] + '-' + m[1] + ' ' + pad(h) + ':' + m[5] + ' ' + tt;
    }
    /* DateTime columns of a GridEX: short date. */
    function shortDate(v) {
        var s = isoDate(v); if (!s) return '';
        return parseInt(s.substring(5, 7), 10) + '/' + parseInt(s.substring(8, 10), 10) + '/' + s.substring(0, 4);
    }
    function busy(btn, fn) {
        var b = (typeof btn === 'string') ? $id(btn) : btn;
        if (b) {
            if (b.disabled || b.classList.contains('is-busy')) return Promise.resolve();
            b.disabled = true; b.classList.add('is-busy');
        }
        var done = function () { if (b) { b.disabled = false; b.classList.remove('is-busy'); } };
        var p;
        try { p = fn(); } catch (e) { done(); box(e.message); return Promise.resolve(); }
        if (p && typeof p.then === 'function') return p.then(done, function (e) { done(); if (e && e.message) box(e.message); });
        done();
        return Promise.resolve(p);
    }
    function parse(r) {
        return r.text().then(function (t) {
            var b = null;
            try { b = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
            if (!r.ok) throw new Error((b && b.message) || ('Request failed (' + r.status + ')'));
            return b;
        });
    }
    function getJson(url) { return fetch(url, { headers: { 'Accept': 'application/json' }, credentials: 'same-origin' }).then(parse); }
    function postJson(url, data) {
        var h = { 'Accept': 'application/json', 'Content-Type': 'application/json' };
        var t = document.querySelector('meta[name="_csrf"]'), n = document.querySelector('meta[name="_csrf_header"]');
        if (t && n && t.getAttribute('content') && n.getAttribute('content')) h[n.getAttribute('content')] = t.getAttribute('content');
        return fetch(url, { method: 'POST', headers: h, credentials: 'same-origin', body: JSON.stringify(data) }).then(parse);
    }
    function refreshCombos() { if (global.DesktopCombo) global.DesktopCombo.refresh(); }
    function show(id, on) { var e = $id(id); if (e) e.classList.toggle('is-hidden', !on); }
    function col(row, name) {
        if (!row) return null;
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        var l = name.toLowerCase();
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === l) return row[k];
        return null;
    }
    function bind(id, rows, valueCol, textCol) {
        var s = $id(id); if (!s) return;
        var keep = s.value;
        var h = '<option value="0"></option>';
        (rows || []).forEach(function (r) { h += '<option value="' + esc(col(r, valueCol)) + '">' + esc(col(r, textCol)) + '</option>'; });
        s.innerHTML = h;
        s.value = keep;
        if (s.value !== keep) s.value = '0';
        refreshCombos();
    }
    function setVal(id, v) { var s = $id(id); if (!s) return; s.value = str(v); if (s.value !== str(v)) s.value = '0'; refreshCombos(); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setText(id, v) { var e = $id(id); if (e) e.value = str(v); }
    function setEnabled(id, on) { var e = $id(id); if (e) { e.disabled = !on; refreshCombos(); } }
    function focus(id) { var e = $id(id); if (!e) return; var w = e.closest && e.closest('.dtcombo-wrap'); var t = w ? w.querySelector('.dtcombo-input') : e; try { (t || e).focus(); } catch (x) { /* ignore */ } }
    function radio(name) { var r = document.querySelector('input[name="' + name + '"]:checked'); return r ? r.value : ''; }
    function cancel() {
        global.close();
        setTimeout(function () { if (!global.closed) { if (history.length > 1) history.back(); else location.href = '/export'; } }, 150);
    }

    // ------------------------------------------------------------------------------ state

    var PERM = { Save: true, Update: true, Delete: true, Print: true };
    var CFG = { defaultDaysToLessFromHistoryFromDate: 0, itemSearchByCode: false };
    var INVOICES = [], HIST_CUSTOMERS = [], HIST_INVOICES = [];
    var S = { recId: 0, updateMode: false, invoiceId: 0, header: null, detail: [], rows: [], removed: [] };
    var HIST = [], HIST_DETAIL = [];
    var CUR = { pl: -1, hist: -1 };

    // ------------------------------------------------------------------------------ tabs

    function tab(panelId) {
        document.querySelectorAll('.win-tabs[data-tabs="pl"] .win-tab').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab') === panelId); });
        ['plForm', 'plHistory'].forEach(function (p) { $id(p).classList.toggle('is-active', p === panelId); });
        var onHist = panelId === 'plHistory';
        var fb = $id('btnPlFooterHistory');
        fb.querySelector('span').textContent = onHist ? 'Form' : 'History';
        fb.querySelector('i').className = onHist ? 'fa fa-file-text-o' : 'fa fa-history';
        /* tabControl1_SelectedIndexChanged */
        if (onHist) focus('FromDateHistory'); else focus('cmbInvoiceNo');
    }
    function onHistory() { return $id('plHistory').classList.contains('is-active'); }
    function toggleHistory() { tab(onHistory() ? 'plForm' : 'plHistory'); }

    // ------------------------------------------------------------------------------ load

    /** InitializeComponentMethod. */
    function load() {
        return getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM = { Save: d.permissions.Save !== false, Update: d.permissions.Update !== false, Delete: d.permissions.Delete !== false, Print: d.permissions.Print !== false };
            if (d.config) CFG = d.config;
            $id('btnsave').disabled = !PERM.Save;
            $id('btnupdate').disabled = !PERM.Update;
            $id('BtnDelete').disabled = !PERM.Delete;
            $id('btnPrint').disabled = !PERM.Print;
            $id('btnPrint529B').disabled = !PERM.Print;
            $id('btnPrint529C').disabled = !PERM.Print;
            show('exportCompanyWrap', !!d.allowExportMultiCompanies);
            if (d.invoicesError) box(d.invoicesError);
            if (d.historyCombosError) box(d.historyCombosError);
            INVOICES = d.invoices || []; bind('cmbInvoiceNo', INVOICES, 'Id', 'InvoiceNo');
            HIST_CUSTOMERS = d.historyCustomers || []; bind('CmbCustomerHistory', HIST_CUSTOMERS, 'Id', 'name');
            HIST_INVOICES = d.historyInvoices || []; bind('CmbInvoiceNoHistory', HIST_INVOICES, 'Id', 'name');
            /* At load the desktop has not yet read the configuration (GetConfigurationsFromGlobal runs on Refresh),
               so DefaultDaysToLessFromHistoryFromDate is 0 and From = today - 3. */
            setText('FromDateHistory', daysAgo(3));
            setText('ToDateHistory', today());
            $id('ChkPrintPreview').checked = true;
            render();
            $id('plFooterInfo').textContent = 'frmExportInvoicePackingList  -  Document Type 204';
            focus('cmbInvoiceNo');
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }

    // ------------------------------------------------------------------------------ header

    function fillHeader(h) {
        S.header = h || {};
        setText('CmbExportCompany', h.ExportCompany);
        setText('txtdocno', h.DocCode);
        setText('txtDocDate', isoDate(h.DocDate));
        setText('txtDocumentaryCreditNo', h.DocumentaryCreditNo);
        setText('txtDocumentaryCreditNoDate', isoDate(h.DocumentaryCreditNoIssueDate));
        setText('txtCustomer', h.Customer);
        setText('txtConsignee', h.Consignee);
        setText('txtCustomerContractsNos', h.CustomerContractNos);
        setText('txtCustomerContractsDates', h.ContractDates);
        setText('txtNotifyParty1', h.NotifyParty1Name);
        setText('txtNotifyParty2', h.NotifyParty2Name);
        setText('txtNotifyParty3', h.NotifyParty3Name);
        setText('txtImporterBank', h.ImporterBank);
        setText('txtExporterBank', h.ExporterBank);
        setText('txtLotRefHeader', h.LotNoRef);
        setText('txtGdNo', h.EFormNo);
        setText('txtGdDate', isoDate(h.EFormDate));
        setText('txtLoadingPort', h.LoadingPort);
        setText('txtDestinationPort', h.DestinationPort);
        setText('txtCareierType', h.CarierType);
        setText('txtDeliveryTerm', h.DeliveryTerm);
        setText('txtDeliveryRemarks', h.DeliveryRemarks);
        setText('txtTcpRegNo', h.TCPRegNo);
        setText('txtContinent', h.ContinentName);
        setText('txtOriginCountry', h.OriginCountry);
        setText('txtImporterCountry', h.ImporterCountry);
        setText('txtPlaceOfDelivery', h.PlaceOfDelivery);
        setText('CmbFcyCode', h.CurrencyCode);
        setText('txtFcyAmount', fmt4(h.FCurrencyAmount));
        setText('txtExRate', fmt4(h.ConversionRate));
        setText('txtLocalAmt', fmt4(h.EquivalentAmount));
        setText('txtaddlesscommnets', h.AddLessComments);
        setText('txtaddlessamount', str(netD(h.AddLessAmount)));
        setText('txtTotalNetAmount', fmt4(h.TotalAmount));
        setText('txtNoOfContainer', str(netI(h.NoOfContainers)));
        setText('txtGrossWeight', fmt4(h.GrossMTons));
        setText('txtNetWeight', fmt4(h.NetMTons));
        setText('txtCertificate1', h.Certificate1);
        setText('txtCertificateOfOrigion', h.Certificate2);
        setText('txtRemarks1', h.Remarks1);
        setText('txtRemarks2', h.Remarks2);
        setText('txtOtherRemarks1', h.OtherRemarks1);
        setText('txtOtherRemarks2', h.OtherRemarks2);
        setText('txtFcyAmountHeaderCustom', fmt4(h.FcyAmountCustom));
        setText('txtAddLessHeaderCustom', fmt4(h.AddLessAmountCustom));
        setText('txtTotalFcyAmountHeaderCustom', fmt4(h.TotalFcyAmountCustom));
        setText('txtLocalAmountHeaderCustom', fmt4(h.LocalAmountCustom));
    }
    function clearHeader() {
        ['CmbExportCompany', 'txtdocno', 'txtDocumentaryCreditNo', 'txtCustomer', 'txtConsignee', 'txtCustomerContractsNos', 'txtCustomerContractsDates',
         'txtNotifyParty1', 'txtNotifyParty2', 'txtNotifyParty3', 'txtImporterBank', 'txtExporterBank', 'txtLotRefHeader', 'txtGdNo', 'txtLoadingPort',
         'txtDestinationPort', 'txtCareierType', 'txtDeliveryTerm', 'txtDeliveryRemarks', 'txtTcpRegNo', 'txtContinent', 'txtOriginCountry', 'txtImporterCountry',
         'txtPlaceOfDelivery', 'CmbFcyCode', 'txtExRate', 'txtFcyAmount', 'txtLocalAmt', 'txtaddlessamount', 'txtaddlesscommnets', 'txtTotalNetAmount',
         'txtGrossWeight', 'txtNetWeight', 'txtCertificate1', 'txtCertificateOfOrigion', 'txtRemarks1', 'txtRemarks2', 'txtOtherRemarks1', 'txtOtherRemarks2',
         'txtDocDate', 'txtDocumentaryCreditNoDate', 'txtGdDate', 'txtFcyAmountHeaderCustom', 'txtAddLessHeaderCustom', 'txtTotalFcyAmountHeaderCustom', 'txtLocalAmountHeaderCustom']
            .forEach(function (id) { setText(id, ''); });
        S.header = null;
    }

    // ------------------------------------------------------------------------------ grid

    var COLS = ['ContainerNo', 'ItemCode', 'ItemName', 'ItemCommodityDetail', 'ItemPmDetail', 'PackageDate', 'ExpiryDate', 'LotNo',
        'ThirdPartyAnalysisNo', 'ThirdPartyAnalysisSubNo', 'OuterQty', 'OuterUom', 'InnerQty', 'InnerUom', 'NetWeight', 'GrossWeight', 'Remarks'];
    /* grdSettingsPackListDetaill: editableColumns - TextBox for the texts and Gross Weight, Calendar for the dates. */
    var EDIT = { ItemCommodityDetail: 'text', ItemPmDetail: 'text', PackageDate: 'date', ExpiryDate: 'date', LotNo: 'text', GrossWeight: 'num', Remarks: 'text' };
    var NUM = { OuterQty: 1, InnerQty: 1, NetWeight: 1, GrossWeight: 1 };
    function cellText(c, v) {
        if (NUM[c]) return fmt(v);
        if (c === 'PackageDate' || c === 'ExpiryDate') return shortDate(v);
        return str(v);
    }
    function drawRows(bodyId, footId, rows, cur, editable, withX) {
        var body = $id(bodyId), foot = $id(footId);
        var sums = {};
        body.innerHTML = rows.map(function (r, i) {
            var h = '<tr data-i="' + i + '"' + (i === cur ? ' class="is-current"' : '') + '>';
            if (withX) h += '<td class="win-cell-btn"><button type="button" class="win-x" data-del="' + i + '" title="Delete">X</button></td>';
            COLS.forEach(function (c) {
                var v = col(r, c);
                if (NUM[c]) sums[c] = (sums[c] || 0) + netD(v);
                var kind = editable ? EDIT[c] : null;
                if (kind === 'date') h += '<td class="win-editable"><input type="date" data-i="' + i + '" data-c="' + c + '" value="' + esc(isoDate(v)) + '"/></td>';
                else if (kind === 'num') h += '<td class="win-editable"><input type="text" class="num" data-guard="decimal" data-i="' + i + '" data-c="' + c + '" value="' + esc(str(v)) + '"/></td>';
                else if (kind === 'text') h += '<td class="win-editable"><input type="text" data-i="' + i + '" data-c="' + c + '" value="' + esc(str(v)) + '"' + (c === 'Remarks' ? ' style="min-width:340px;"' : '') + ' title="' + (c === 'ItemCommodityDetail' || c === 'ItemPmDetail' ? 'F1 = pick from the item&#39;s ' + (c === 'ItemPmDetail' ? 'packing' : 'commodity') + ' details' : '') + '"/></td>';
                else if (c === 'ContainerNo') h += '<td><a class="win-code" data-i="' + i + '" href="javascript:void(0)">' + esc(cellText(c, v)) + '</a></td>';
                else h += '<td' + (NUM[c] ? ' class="num"' : '') + '>' + esc(cellText(c, v)) + '</td>';
            });
            return h + '</tr>';
        }).join('');
        if (!rows.length) { foot.innerHTML = ''; return; }
        var t = '<tr>' + (withX ? '<td class="lbl">&Sigma;</td>' : '');
        COLS.forEach(function (c, idx) {
            var v = NUM[c] ? fmt(sums[c] || 0) : (idx === 0 && !withX ? 'Σ' : '');
            t += '<td' + (NUM[c] ? '' : ' class="lbl"') + '>' + esc(v) + '</td>';
        });
        foot.innerHTML = t + '</tr>';
    }
    function render() { drawRows('plBody', 'plFoot', S.rows, CUR.pl, true, true); }

    /* A cell edit writes straight into the row, as GridEX writes into the bound DataTable. */
    function onCellInput(e) {
        var inp = e.target.closest('input[data-c]'); if (!inp) return;
        var r = S.rows[+inp.getAttribute('data-i')]; if (!r) return;
        var c = inp.getAttribute('data-c');
        r[c] = EDIT[c] === 'num' ? netD(inp.value) : inp.value;
        if (EDIT[c] === 'num' && e.type === 'change') { inp.value = str(r[c]); renderFootOnly(); }
    }
    function renderFootOnly() {
        var sums = {}; S.rows.forEach(function (r) { for (var c in NUM) sums[c] = (sums[c] || 0) + netD(r[c]); });
        var foot = $id('plFoot'); if (!foot.firstChild) return;
        var t = '<tr><td class="lbl">&Sigma;</td>';
        COLS.forEach(function (c) { t += '<td' + (NUM[c] ? '' : ' class="lbl"') + '>' + (NUM[c] ? esc(fmt(sums[c] || 0)) : '') + '</td>'; });
        foot.innerHTML = t + '</tr>';
    }
    /* DeletePackListDetailRow: a saved row asks and is remembered with ActionTypeId 3; a new row is just removed. */
    function deleteRow(i) {
        var r = S.rows[i]; if (!r) return;
        if (netI(r.Id) > 0) {
            if (!ask('Are you sure to Delete?')) return;
            var vd = {}; for (var k in r) if (Object.prototype.hasOwnProperty.call(r, k)) vd[k] = r[k];
            S.removed.push(vd);
        }
        S.rows.splice(i, 1);
        CUR.pl = -1;
        render();
    }

    // ------------------------------------------------------------------------------ F1 picker (GrdPopUp)

    var PICK = { rows: [], onPick: null };
    function pickerOpen(title, rows, onPick) {
        PICK.rows = rows; PICK.onPick = onPick;
        $id('plPickerTitle').textContent = title;
        $id('plPickerSearch').value = '';
        pickerDraw('');
        show('plPicker', true);
        $id('plPickerSearch').focus();
    }
    function pickerDraw(q) {
        q = (q || '').toLowerCase();
        $id('plPickerBody').innerHTML = PICK.rows.map(function (r, i) {
            var t = str(col(r, 'Remarks')), h = str(col(r, 'HSCode'));
            if (q && (t + ' ' + h).toLowerCase().indexOf(q) < 0) return '';
            return '<tr data-p="' + i + '"><td>' + esc(t) + '</td><td>' + esc(h) + '</td></tr>';
        }).join('');
    }
    function pickerClose() { show('plPicker', false); PICK.onPick = null; }
    /* HandleCommodityOrPackingDetail(row, detailTypeId 1 | 2, columnKey): ItemCommodityDetailDbCall(itemId).Select("DetailTypeId=n"),
       then GrdPopUp(rows, "Id", "Remarks") and the picked ReturnName into the cell; "Please Select an Item First" / "No Record Found". */
    function handleCommodityOrPacking(rowIndex, detailTypeId, columnKey, inp) {
        var r = S.rows[rowIndex]; if (!r) return;
        var itemId = netI(r.ItemId);
        if (itemId <= 0) { box('Please Select an Item First'); return; }
        getJson(API + '/commodity-remarks?itemId=' + itemId + '&detailTypeId=' + detailTypeId).then(function (rows) {
            if (!rows || !rows.length) { box('No Record Found'); return; }
            pickerOpen(columnKey === 'ItemPmDetail' ? 'Pm Detail' : 'Commodity Detail', rows, function (picked) {
                r[columnKey] = str(col(picked, 'Remarks'));
                if (inp) inp.value = r[columnKey];
            });
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ ReadById / Insert / Delete / Reset

    /** cmbInvoiceNo_Leave: "Please Select an Invoice First" with nothing chosen, else ReadById(Id, History:false). */
    function invoiceLeave() {
        var id = netI(val('cmbInvoiceNo'));
        if (id === 0) { box('Please Select an Invoice First'); return Promise.resolve(); }
        return readById(id, false);
    }
    /** ReadById(ID, History). */
    function readById(id, history) {
        return getJson(API + '/by-id?id=' + id + '&history=' + (history ? 'true' : 'false')).then(function (d) {
            d = d || {};
            formResetLocal();
            tab('plForm');
            var h = d.header || {};
            if (history) {
                S.recId = id; S.updateMode = true;
                show('btnsave', false); show('btnupdate', true); show('BtnDelete', true);
                INVOICES = [{ Id: h.Id, InvoiceNo: h.InvoiceNo }];
                bind('cmbInvoiceNo', INVOICES, 'Id', 'InvoiceNo');
                setVal('cmbInvoiceNo', h.Id);
                setEnabled('cmbInvoiceNo', false);
            } else {
                S.recId = 0; S.updateMode = false;
                show('btnsave', true); show('btnupdate', false);
                setVal('cmbInvoiceNo', h.Id);
            }
            S.invoiceId = netI(h.Id);
            fillHeader(h);
            S.detail = d.detail || [];
            S.rows = d.rows || [];
            S.removed = [];
            CUR.pl = -1;
            render();
            var first = $id('plBody').querySelector('input');
            if (first) first.focus();
        }).catch(function (e) { box(e.message); });
    }
    /** FormReset without the invoice re-read (used inside ReadById, which re-binds itself). */
    function formResetLocal() {
        show('btnsave', true); show('btnupdate', false); show('BtnDelete', false);
        S.recId = 0; S.updateMode = false; S.invoiceId = 0;
        S.removed = [];
        clearHeader();
        S.rows = []; S.detail = []; CUR.pl = -1;
        render();
        setEnabled('cmbInvoiceNo', true);
    }
    /** FormReset: the above, then ExImInvoice_GetInvoicesRefferedInDo and focus on Doc Date. */
    function formReset() {
        formResetLocal();
        return getJson(API + '/refresh').then(function (d) {
            INVOICES = (d && d.invoices) || []; bind('cmbInvoiceNo', INVOICES, 'Id', 'InvoiceNo'); setVal('cmbInvoiceNo', '0');
            if (d && d.config) CFG = d.config;
        }).catch(function (e) { box(e.message); }).then(function () { focus('cmbInvoiceNo'); });
    }
    /** btnNew_Click. */
    function btnNew() { return formReset(); }
    /** btnRefresh_Click: configuration; the invoice list only while the combo is enabled. */
    function btnRefresh(btn) {
        return busy(btn, function () {
            return getJson(API + '/refresh').then(function (d) {
                d = d || {};
                if (d.config) CFG = d.config;
                if (!$id('cmbInvoiceNo').disabled) { INVOICES = d.invoices || []; bind('cmbInvoiceNo', INVOICES, 'Id', 'InvoiceNo'); }
            }).catch(function (e) { box(e.message); });
        });
    }
    /** CheckItemIds(dtdetail, dtPackingListDetail) - repeated here so the message comes before the confirm, as on the desktop. */
    function checkItemIds() {
        var dNet = 0, dGross = 0, pNet = 0, pGross = 0;
        S.detail.forEach(function (r) { dNet += netD(col(r, 'NetWeight')); dGross += netD(col(r, 'GrossWeight')); });
        S.rows.forEach(function (r) { pNet += netD(r.NetWeight); pGross += netD(r.GrossWeight); });
        if (dNet - pNet > 1.0) throw new Error('Validation failed: Total Net Weight in Detail (' + dNet + ' kg) does not match PackList (' + pNet + ' kg).');
        if (dGross - pGross > 1.0) throw new Error('Validation failed: Total Gross Weight in Detail (' + dGross + ' kg) does not match PackList (' + pGross + ' kg).');
    }
    /** Insert(). */
    function insert(btn) {
        return busy(btn, function () {
            try {
                if (S.rows.length === 0) throw new Error('Detail Record Not Found');
                if (netD(val('txtGrossWeight')) < netD(val('txtNetWeight'))) { box('Gross Weight must be equal to or greater than Net Weight. Thank you.'); focus('txtGrossWeight'); return Promise.resolve(); }
                checkItemIds();
            } catch (e) { box(e.message); return Promise.resolve(); }
            if (!ask(S.recId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return Promise.resolve();
            var invoiceId = S.recId > 0 ? S.recId : netI(val('cmbInvoiceNo'));
            /* the previews open in tabs reserved inside the click, so a popup blocker lets them through */
            var wins = [];
            if (global.CrystalPrint) {
                if ($id('ChkPrintPreview').checked) wins.push(['exp-529A', global.CrystalPrint.reserve()]);
                if ($id('ChkSlipB').checked) wins.push(['exp-529B', global.CrystalPrint.reserve()]);
                if ($id('chkSlipC').checked) wins.push(['exp-529C', global.CrystalPrint.reserve()]);
            }
            return postJson(API + '/save', { recId: S.recId, invoiceId: invoiceId, rows: S.rows, removed: S.removed }).then(function (d) {
                box((d && d.message) || (S.recId === 0 ? 'Save SuccessFully' : 'Update SuccessFully'));
                var printId = (d && d.id) || invoiceId;
                return formReset().then(function () {
                    wins.forEach(function (w) { global.CrystalPrint.open(w[0], { id: printId }, null, w[1]); });
                });
            }).catch(function (e) {
                wins.forEach(function (w) { global.CrystalPrint.release(w[1]); });
                box(e.message);
            });
        });
    }
    /** btnsave_Click: RecId = 0 then Insert. */
    function btnsave(btn) { S.recId = 0; return insert(btn); }
    /** btnupdate_Click: "Rec Id not found..." without one. */
    function btnupdate(btn) { if (S.recId === 0) { box('Rec Id not found...'); return Promise.resolve(); } return insert(btn); }
    /** BtnDelete_Click. */
    function btnDelete(btn) {
        return busy(btn, function () {
            if (S.recId === 0) { box('RecId not found'); return Promise.resolve(); }
            if (!ask('Are you sure to Delete?')) return Promise.resolve();
            return postJson(API + '/delete', { recId: netI(val('cmbInvoiceNo')) || S.recId }).then(function (d) {
                box((d && d.message) || 'Delete Record Successfully');
                return formReset();
            }).catch(function (e) { box(e.message); });
        });
    }
    /** btnPrint_Click / btnPrint529B_Click / btnPrint529C_Click: CommonServices.ExportPackingList529x(RecId). */
    function print(slip, btn) {
        if (!global.CrystalPrint) { box('Printing is not available on this page.'); return; }
        if (S.recId === 0) { box('No Record Found For Display'); return; }
        return global.CrystalPrint.open('exp-529' + slip, { id: S.recId }, btn);
    }
    /** MakeShortCutKeys - the desktop's ShortCutKeyPopUp table. */
    function shortcuts() {
        box(['Ctrl+S  For Save', 'Ctrl+U  For Update', 'Ctrl+E  For Close', 'Ctrl+R  For Refresh', 'Ctrl+N  For New', 'Ctrl+P  For Print',
             'Ctrl+F5  For Focus on Doc Date', 'Ctrl+T  For Tab Transfer', 'Ctrl+alt  To Show ShortCut Keys Form',
             'Ctrl+ArrowDown  For Focus On Selected Tab Detail Grid', 'Ctrl+ArrowUp  For Focus On First Entry of Detail Selected Tab',
             'Ctrl+Enter  When Focus On Any Grid For Update Record', 'Ctrl+Space  When Focus On Any Grid To Call Function\'s On Button Or Link',
             'F1  On Commodity / Pm Detail cell: pick from the item\'s details'].join('\n'));
    }

    // ------------------------------------------------------------------------------ history

    var HCOLS = ['InvoiceNo', 'DocCode', 'DocDate', 'CustomerName', 'NoOfContainers', 'GrossWeight', 'NetWeight', 'FcyAmount', 'AddLess', 'TotalAmount',
        'LoadingPort', 'DestinationPort', 'EntryUser', 'EntryDate', 'ModifyUser', 'ModifyDate', 'NoOfAttachments'];
    var HNUM = { NoOfContainers: 3, GrossWeight: 3, NetWeight: 3, FcyAmount: 2, AddLess: 3, TotalAmount: 2 };
    function hcell(c, v) {
        if (c === 'DocCode' || c === 'NoOfAttachments') return str(netI(v));
        if (HNUM[c]) return fmt(v, HNUM[c]);
        if (c === 'DocDate') return ddMMMyyyy(v);
        if (c === 'EntryDate' || c === 'ModifyDate') return ddMMyyyyHm(v);
        return str(v);
    }
    function histRender() {
        show('plHistEditTh', PERM.Update);
        show('plHistSlipTh', PERM.Print);
        var sums = {};
        $id('plHistBody').innerHTML = HIST.map(function (r, i) {
            var h = '<tr data-i="' + i + '"' + (i === CUR.hist ? ' class="is-current"' : '') + '>';
            if (PERM.Update) h += '<td class="win-cell-btn"><button type="button" class="win-edit" data-edit="' + i + '">Edit</button></td>';
            if (PERM.Print) h += '<td class="win-cell-btn"><button type="button" class="win-edit" data-slip="A" data-i="' + i + '">Slip</button></td>'
                + '<td class="win-cell-btn"><button type="button" class="win-edit" data-slip="B" data-i="' + i + '">SlipB</button></td>'
                + '<td class="win-cell-btn"><button type="button" class="win-edit" data-slip="C" data-i="' + i + '">SlipC</button></td>';
            HCOLS.forEach(function (c) {
                var v = col(r, c);
                if (HNUM[c]) sums[c] = (sums[c] || 0) + netD(v);
                var inner = esc(hcell(c, v));
                if (c === 'InvoiceNo') inner = '<a class="win-code" data-i="' + i + '" href="javascript:void(0)">' + inner + '</a>';
                h += '<td' + (HNUM[c] || c === 'DocCode' || c === 'NoOfAttachments' ? ' class="num"' : '') + '>' + inner + '</td>';
            });
            return h + '</tr>';
        }).join('');
        var lead = (PERM.Update ? 1 : 0) + (PERM.Print ? 3 : 0);
        var foot = $id('plHistFoot');
        if (!HIST.length) foot.innerHTML = '';
        else {
            var t = '<tr>' + (lead ? '<td class="lbl" colspan="' + lead + '">&Sigma;</td>' : '');
            HCOLS.forEach(function (c, idx) { t += '<td' + (HNUM[c] ? '' : ' class="lbl"') + '>' + (HNUM[c] ? esc(fmt(sums[c] || 0, HNUM[c])) : (idx === 0 && !lead ? 'Σ' : '')) + '</td>'; });
            foot.innerHTML = t + '</tr>';
        }
        show('plHistEmpty', HIST.length === 0);
        HIST_DETAIL = []; drawRows('plHistDetailBody', 'plHistDetailFoot', HIST_DETAIL, -1, false, false);
    }
    /** btnShowHistory_Click -> HistoryGridFill. */
    function historyShow(btn) {
        return busy(btn, function () {
            var f = {
                dateBy: radio('dateBy'),
                fromChecked: $id('FromDateHistoryChk').checked, fromDate: val('FromDateHistory'),
                toChecked: $id('ToDateHistoryChk').checked, toDate: val('ToDateHistory'),
                fromDocNo: netI(val('FromDocNoHistory')), toDocNo: netI(val('ToDocNoHistory')),
                customerId: netI(val('CmbCustomerHistory')), invoiceId: netI(val('CmbInvoiceNoHistory')),
                referred: radio('referred')
            };
            return postJson(API + '/history', f).then(function (rows) { HIST = rows || []; CUR.hist = -1; histRender(); })
                .catch(function (e) { box(e.message); });
        });
    }
    /** btnResetHistory_Click. */
    function historyReset() {
        setText('FromDateHistory', daysAgo(3)); setText('ToDateHistory', today());
        setText('FromDocNoHistory', ''); setText('ToDocNoHistory', '');
        setVal('CmbCustomerHistory', '0');
        HIST = []; CUR.hist = -1; histRender(); show('plHistEmpty', false);
    }
    /** btnRefreshHistory_Click. */
    function historyRefresh(btn) {
        return busy(btn, function () {
            return getJson(API + '/history-combos').then(function (d) {
                d = d || {};
                HIST_CUSTOMERS = d.historyCustomers || []; bind('CmbCustomerHistory', HIST_CUSTOMERS, 'Id', 'name');
                HIST_INVOICES = d.historyInvoices || []; bind('CmbInvoiceNoHistory', HIST_INVOICES, 'Id', 'name');
            }).catch(function (e) { box(e.message); });
        });
    }
    /** DataGridHistory_SelectionChanged -> GetDetailGrdByHeadId. */
    function historySelect(i) {
        CUR.hist = i;
        var r = HIST[i]; if (!r) return;
        getJson(API + '/detail?id=' + netI(col(r, 'Id'))).then(function (rows) {
            HIST_DETAIL = rows || [];
            drawRows('plHistDetailBody', 'plHistDetailFoot', HIST_DETAIL, -1, false, false);
        }).catch(function (e) { box(e.message); });
    }
    function historyOpen(i) {
        var r = HIST[i]; if (!r) return;
        if (!PERM.Update) { box('The user does not have Update rights for this report'); return; }
        readById(netI(col(r, 'Id')), true);
    }
    function historySlip(slip, i, btn) {
        var r = HIST[i]; if (!r || !global.CrystalPrint) return;
        global.CrystalPrint.open('exp-529' + slip, { id: netI(col(r, 'Id')) }, btn);
    }

    // ------------------------------------------------------------------------------ wiring

    function on(id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); }

    document.addEventListener('DOMContentLoaded', function () {
        document.querySelectorAll('.win-tabs[data-tabs="pl"] .win-tab').forEach(function (b) { b.addEventListener('click', function () { tab(b.getAttribute('data-tab')); }); });
        document.querySelectorAll('button[data-fullscreen]').forEach(function (b) {
            b.addEventListener('click', function () { var bx = $id(b.getAttribute('data-fullscreen')); if (bx) bx.classList.toggle('ex-fullscreen'); });
        });
        on('cmbInvoiceNo', 'change', invoiceLeave);
        var gb = $id('plBody');
        gb.addEventListener('input', onCellInput);
        gb.addEventListener('change', onCellInput);
        gb.addEventListener('click', function (e) {
            var del = e.target.closest('button[data-del]');
            if (del) { deleteRow(+del.getAttribute('data-del')); return; }
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            CUR.pl = +tr.getAttribute('data-i');
            gb.querySelectorAll('tr[data-i]').forEach(function (x) { x.classList.toggle('is-current', x === tr); });
        });
        /* grdPackListDetail_KeyDown: Enter moves on; F1 on Commodity / Pm Detail opens the picker; Ctrl+Space deletes the row. */
        gb.addEventListener('keydown', function (e) {
            var inp = e.target.closest('input[data-c]'); if (!inp) return;
            var i = +inp.getAttribute('data-i'), c = inp.getAttribute('data-c');
            if (e.key === 'F1') {
                e.preventDefault();
                if (c === 'ItemCommodityDetail') handleCommodityOrPacking(i, 1, 'ItemCommodityDetail', inp);
                else if (c === 'ItemPmDetail') handleCommodityOrPacking(i, 2, 'ItemPmDetail', inp);
                return;
            }
            if (e.ctrlKey && e.key === ' ') { e.preventDefault(); deleteRow(i); return; }
            if (e.key === 'Enter' && !e.ctrlKey) {
                e.preventDefault();
                var all = Array.prototype.slice.call(gb.querySelectorAll('input[data-c]'));
                var n = all.indexOf(inp);
                if (n >= 0 && n + 1 < all.length) all[n + 1].focus();
            }
        });
        /* the picker */
        on('plPickerSearch', 'input', function () { pickerDraw($id('plPickerSearch').value); });
        $id('plPickerBody').addEventListener('click', function (e) {
            var tr = e.target.closest('tr[data-p]'); if (!tr) return;
            var picked = PICK.rows[+tr.getAttribute('data-p')];
            if (PICK.onPick) PICK.onPick(picked);
            pickerClose();
        });
        on('plPickerSearch', 'keydown', function (e) {
            if (e.key === 'Escape') { e.preventDefault(); e.stopPropagation(); pickerClose(); }
            if (e.key === 'Enter') { var tr = $id('plPickerBody').querySelector('tr[data-p]'); if (tr) tr.click(); }
        });
        /* history grid: Edit / Slip / SlipB / SlipC buttons, the Invoice No link, selection -> lower grid, double-click -> Edit */
        var hb = $id('plHistBody');
        hb.addEventListener('click', function (e) {
            var ed = e.target.closest('button[data-edit]');
            if (ed) { historyOpen(+ed.getAttribute('data-edit')); return; }
            var sl = e.target.closest('button[data-slip]');
            if (sl) { historySlip(sl.getAttribute('data-slip'), +sl.getAttribute('data-i'), sl); return; }
            var lk = e.target.closest('a.win-code');
            if (lk) { historyOpen(+lk.getAttribute('data-i')); return; }
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            hb.querySelectorAll('tr[data-i]').forEach(function (x) { x.classList.toggle('is-current', x === tr); });
            historySelect(+tr.getAttribute('data-i'));
        });
        hb.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr) historyOpen(+tr.getAttribute('data-i')); });
        /* ImProformaInvoice_KeyDown */
        document.addEventListener('keydown', function (e) {
            if (!$id('plPicker').classList.contains('is-hidden')) return;
            var k = (e.key || '').toLowerCase();
            if (e.key === 'Enter' && !e.ctrlKey && e.target && /^(INPUT|SELECT)$/.test(e.target.tagName) && e.target.type !== 'checkbox' && !e.target.closest('#plBody')) {
                var f = Array.prototype.filter.call(document.querySelectorAll('input:not([type=hidden]):not(:disabled), select:not(:disabled), button:not(:disabled), textarea:not(:disabled)'), function (x) { return x.offsetParent !== null; });
                var i = f.indexOf(e.target);
                if (i >= 0 && i + 1 < f.length) { e.preventDefault(); f[i + 1].focus(); }
                return;
            }
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); cancel(); return; }
            if (e.ctrlKey && k === 't') { e.preventDefault(); toggleHistory(); return; }
            if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcuts(); return; }
            if (!onHistory()) {
                if (e.ctrlKey && k === 's' && !S.updateMode) { e.preventDefault(); btnsave($id('btnsave')); }
                if (e.ctrlKey && k === 'u' && S.updateMode) { e.preventDefault(); btnsave($id('btnupdate')); }   /* the desktop's Ctrl+U calls btnsave_Click too */
                if (e.ctrlKey && k === 'n') { e.preventDefault(); btnNew(); }
                if (e.ctrlKey && k === 'p' && PERM.Print) { e.preventDefault(); print('A', $id('btnPrint')); }
                if (e.ctrlKey && k === 'r') { e.preventDefault(); btnRefresh($id('btnRefresh')); }
                if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); focus('cmbInvoiceNo'); }
                if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); var fi = $id('plBody').querySelector('input'); if (fi) fi.focus(); }
                if (e.ctrlKey && e.key === 'ArrowUp') { e.preventDefault(); focus('cmbInvoiceNo'); }
                return;
            }
            if (e.ctrlKey && k === 's') { e.preventDefault(); historyShow($id('btnShowHistory')); }
            if (e.ctrlKey && k === 'n') { e.preventDefault(); historyReset(); }
            if (e.ctrlKey && k === 'r') { e.preventDefault(); historyRefresh($id('btnRefreshHistory')); }
            if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); focus('FromDateHistory'); }
            if (e.ctrlKey && e.key === 'ArrowUp') { e.preventDefault(); focus('FromDateHistory'); }
            if (e.ctrlKey && (e.key === 'Enter') && CUR.hist >= 0 && PERM.Update) { e.preventDefault(); historyOpen(CUR.hist); }
        });
        load();
    });

    global.ExportPl = {
        btnNew: btnNew, btnRefresh: btnRefresh, btnsave: btnsave, btnupdate: btnupdate, btnDelete: btnDelete, print: print, shortcuts: shortcuts,
        historyShow: historyShow, historyReset: historyReset, historyRefresh: historyRefresh, toggleHistory: toggleHistory, pickerClose: pickerClose
    };
}(window));
