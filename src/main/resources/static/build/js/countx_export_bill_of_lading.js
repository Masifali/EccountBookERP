/* ============================================================================================
 * countx_export_bill_of_lading.js - EximBillOfLading.cs (Architecture.WinApp.Export), screen 213
 * "Export Bill Of Lading". Form | History tabs; every button, cmbInvoiceNo_Leave, history Show / New /
 * Refresh, grid double-click / Print / Edit buttons, the attachments link and the KeyDown shortcuts of the
 * desktop form have their counterpart here with the desktop's texts; data from /api/export/bill-of-lading.
 * Prints: 505-Print -> seeded contract 505-eximbilloflading-slip (invoiceId); grid Print ->
 * CommonServices.ExportInvoiceSlip_520 -> 530-eximbillofladingslip (invoiceId).
 * ============================================================================================ */
(function (global) {
    'use strict';

    var F = global.ExportF, $id = F.$id, box = F.box, ask = F.ask, str = F.str, netI = F.netI, netD = F.netD, val = F.val, setText = F.setText,
        setVal = F.setVal, bind = F.bind, busy = F.busy, getJson = F.getJson, postJson = F.postJson, show = F.show, focus = F.focus;
    var API = '/api/export/bill-of-lading';

    var PERM = { Save: true, Update: true, Print: true };
    var INVOICES = [], SUPPLIERS = [], BANKS = [], HIST_INV = [];
    var S = { recId: 0, updateMode: false };
    var HIST = { rows: [], cur: -1 };

    function bindInvoices() { bind('cmbInvoiceNo', INVOICES, 'Id', 'Name', []); }
    function bindSuppliers() { bind('cmbNotifyParty', SUPPLIERS, 'Id', 'Name', []); bind('cmbImporter', SUPPLIERS, 'Id', 'Name', []); }
    function bindBanks() { bind('cmbTotheOrder', BANKS, 'Id', 'Name', []); }
    function bindHistInv() { bind('CmbInvoicenoHistory', HIST_INV, 'Id', 'Name', []); }

    /** EximShipmentInfo_Load. */
    function load() {
        return getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM = { Save: d.permissions.Save !== false, Update: d.permissions.Update !== false, Print: d.permissions.Print !== false };
            $id('btnsave').disabled = !PERM.Save; $id('btnUpdate').disabled = !PERM.Update; $id('Print').disabled = !PERM.Print;
            ['invoices', 'suppliers', 'banks', 'historyInvoices'].forEach(function (k) { if (d[k + 'Error']) box(d[k + 'Error']); });
            INVOICES = d.invoices || []; bindInvoices();
            SUPPLIERS = d.suppliers || []; bindSuppliers();
            BANKS = d.banks || []; bindBanks();
            HIST_INV = d.historyInvoices || []; bindHistInv();
            /* DefaultDaysToLessFromHistoryFromDate is never read by this form -> today - 3 */
            setText('FromDateHistory', F.daysAgo(3)); setText('ToDateHistory', F.today());
            setText('txtBLDate', F.today()); setText('txtBookingDate', F.today()); setText('txtEformDate', F.today());
            $id('blFooterInfo').textContent = 'EximBillOfLading';
            focus('cmbInvoiceNo');
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }
    /** BtnRefresh_Click: bindInvoiceNo + bindSupplier. */
    function refresh(btn) {
        return busy(btn, function () {
            return getJson(API + '/refresh').then(function (d) {
                INVOICES = d.invoices || []; bindInvoices();
                SUPPLIERS = d.suppliers || []; bindSuppliers();
            }).catch(function (e) { box(e.message); });
        });
    }

    /** cmbInvoiceNo_Leave: cmbinvoiceLeaveComboFills + ReadBookingCroByInvoiceId. */
    function invoiceLeave() {
        var id = netI(val('cmbInvoiceNo'));
        if (id <= 0) return Promise.resolve();
        return getJson(API + '/invoice-leave?invoiceId=' + id).then(function (d) {
            d = d || {};
            var lc = $id('cmblccontract'), cc = $id('cmbcurrencycode');
            if (d.found) {
                setText('txtdeliveryterm', d.DeliveryTerm);
                lc.innerHTML = '<option value="0"></option>' + (d.lcOrders || []).map(function (r) { return '<option value="' + F.esc(r.Id) + '">' + F.esc(r.Name) + '</option>'; }).join('');
                if ((d.lcOrders || []).length) lc.selectedIndex = 1;
                cc.innerHTML = '<option value="0"></option>' + (d.currencies || []).map(function (r) { return '<option value="' + F.esc(r.Id) + '">' + F.esc(r.Name) + '</option>'; }).join('');
                if ((d.currencies || []).length) cc.selectedIndex = 1;
                setText('txtGrossWeight', d.GrossWeight); setText('txtNetWeight', d.NetWeight); setText('txtNoOfContainer', d.NoOfContainers);
                F.setValOrAdd('cmbImporter', d.SupplierCustomerId, '');
                setText('txtEformNo', d.EFormNo); setText('txtEformDate', d.EFormDate || F.today());
                F.setValOrAdd('cmbNotifyParty', d.NotifyPartyId, '');
            } else {
                setText('txtGrossWeight', ''); setText('txtNetWeight', ''); setText('txtNoOfContainer', '');
                setVal('cmbImporter', '0'); setText('txtEformNo', ''); setText('txtEformDate', F.today());
            }
            var b = d.booking || {};
            if (b.found) { setText('txtBookingNo', b.BookingCroNo); setText('txtBookingDate', b.BookingDate); setText('txtVesselName', b.VesselName); setText('txtVoyageNo', b.VoyageNo); }
            else { setText('txtBookingNo', ''); setText('txtBookingDate', F.today()); setText('txtVesselName', ''); setText('txtVoyageNo', ''); }
        }).catch(function (e) { box(e.message); });
    }

    /** FormValidation(). */
    function formValidation() {
        if (!F.hasSel('cmbInvoiceNo')) { box('Invoice  Field Required'); focus('cmbInvoiceNo'); return false; }
        if (!val('txtBookingNo').trim()) { box('Booking No  Field Required'); focus('txtBookingNo'); return false; }
        if (!val('txtBLNumber').trim()) { box('Bl Number  Field Required'); focus('txtBLNumber'); return false; }
        if (!val('txtVoyageNo').trim()) { box('Voyage No  Field Required'); focus('txtVoyageNo'); return false; }
        if (!F.hasSel('cmbFreight')) { box('Freight Type Field Required'); focus('cmbFreight'); return false; }
        if (!F.hasSel('cmbcarier')) { box('Carrier Type  Field Required'); focus('cmbcarier'); return false; }
        return true;
    }
    /** Insert(). */
    function insert(btn) {
        return busy(btn, function () {
            if (!formValidation()) return Promise.resolve();
            if (!ask(S.recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return Promise.resolve();
            return postJson(API + '/save', {
                recId: S.recId, updateMode: S.updateMode, invoiceId: netI(val('cmbInvoiceNo')),
                orderOfId: netI(val('cmbTotheOrder')), toTheOrderOfName: val('txtTotheOrder'),
                notifyPartyId: netI(val('cmbNotifyParty')), notifyPartyName: val('txtNotifyParty'),
                importerId: netI(val('cmbImporter')), importerName: val('txtImporter'),
                blNumber: val('txtBLNumber'), blDate: val('txtBLDate'), vesselName: val('txtVesselName'), voyageNo: val('txtVoyageNo'),
                freightId: netI(val('cmbFreight')), freightText: F.selText('cmbFreight'), carierId: netI(val('cmbcarier')), carierText: F.selText('cmbcarier'),
                bookingNo: val('txtBookingNo'), bookingDate: val('txtBookingDate'), fobValue: netD(val('txtFOBValue')),
                goodsDesc: val('txtGoodsDesc'), remarks: val('txtRemarks')
            }).then(function (d) {
                box((d && d.message) || (S.updateMode ? 'Update SuccessFully' : 'Save SuccessFully'));
                return formReset();
            }).catch(function (e) { box(e.message); });
        });
    }
    function save(btn) { S.recId = 0; return insert(btn); }
    /** btnUpdate_Click. */
    function update(btn) {
        if (S.recId === 0) { box('Record Not Update because RecId Not Found'); return Promise.resolve(); }
        return insert(btn);
    }
    /** ReadById(Id). */
    function readById(id) {
        return getJson(API + '/by-id?id=' + encodeURIComponent(id)).then(function (r) {
            S.recId = netI(r.Id);
            F.setEnabled('cmbInvoiceNo', false);
            tab('tabPage1');
            if (!F.findRow(INVOICES, r.ExImInvoiceId)) { INVOICES.push({ Id: netI(r.ExImInvoiceId), Name: str(r.ExImInvoiceNo) }); bindInvoices(); }
            setVal('cmbInvoiceNo', r.ExImInvoiceId);
            F.setValOrAdd('cmbTotheOrder', r.OrderOfId, ''); setText('txtTotheOrder', r.ToTheOrderOfName);
            F.setValOrAdd('cmbNotifyParty', r.NotifyPartyId, ''); setText('txtNotifyParty', r.NotifyPartyName);
            F.setValOrAdd('cmbImporter', r.ImporterId, ''); setText('txtImporter', r.ImporterName);
            setText('txtBLNumber', r.BLNumber); setText('txtBLDate', r.BLDate || F.today());
            setText('txtVesselName', r.VesselNo); setText('txtVoyageNo', r.VoyageNo);
            setByText('cmbFreight', r.FreightType); setByText('cmbcarier', r.CarierType);
            setText('txtBookingNo', r.BookingNo); setText('txtBookingDate', r.BookingDate || F.today());
            setText('txtFOBValue', str(r.FobValue)); setText('txtGoodsDesc', r.ItemsGoodsDesc); setText('txtRemarks', r.Remarks);
            show('btnsave', false); show('btnUpdate', true);
            S.updateMode = true;
        }).catch(function (e) { box(e.message); });
    }
    /* UltraCombo.Text = "Freight PrePaid" selects the row with that text; unknown text leaves the combo empty. */
    function setByText(id, text) {
        var s = $id(id), found = '0';
        Array.prototype.forEach.call(s.options, function (o) { if (o.textContent.trim() === str(text).trim() && o.value !== '0') found = o.value; });
        setVal(id, found);
    }
    /** FormReset(). */
    function formReset() {
        F.setEnabled('cmbInvoiceNo', true);
        setText('txtBLDate', F.today()); setText('txtBLNumber', ''); setText('txtBookingDate', F.today()); setText('txtBookingNo', '');
        $id('cmbcurrencycode').innerHTML = ''; $id('cmblccontract').innerHTML = '';
        setText('txtEformDate', F.today()); setText('txtEformNo', ''); setText('txtEformValue', ''); setText('txtFOBValue', ''); setText('txtGoodsDesc', '');
        setText('txtGrossWeight', ''); setText('txtImporter', ''); setText('txtNetWeight', ''); setText('txtNoOfContainer', ''); setText('txtNotifyParty', '');
        setText('txtdeliveryterm', ''); setText('txtRemarks', ''); setText('txtTotheOrder', ''); setText('txtVesselName', ''); setText('txtVoyageNo', '');
        ['cmbFreight', 'cmbImporter', 'cmbInvoiceNo', 'cmbNotifyParty', 'cmbTotheOrder', 'cmbcarier'].forEach(function (id) { setVal(id, '0'); });
        show('btnsave', true); show('btnUpdate', false);
        S.updateMode = false; S.recId = 0;
        return getJson(API + '/refresh').then(function (d) { INVOICES = d.invoices || []; bindInvoices(); })
            .then(function () { return historyShow(null); }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ history

    var COLS = [
        { key: 'InvoiceNo' }, { key: 'BookingNo' }, { key: 'BookingDate', fmt: 'date' }, { key: 'BLNumber' }, { key: 'BLDate', fmt: 'date' },
        { key: 'VesselNo' }, { key: 'VoyageNo' }, { key: 'FrieghtType' }, { key: 'CarierType' }, { key: 'FobValue', fmt: 'num' },
        { key: 'EntryDate', fmt: 'datetime', mmm: false }, { key: 'EntryUser' }, { key: 'ModifyDate', fmt: 'datetime', mmm: false }, { key: 'ModifyUser' },
        { key: 'NoOfAttachments', fmt: 'int', link: true }];
    function histRender() {
        F.drawGrid('histBody', null, HIST.rows, COLS, HIST.cur, function (r, i) {
            return '<td class="win-cell-btn"><button type="button" class="win-edit" data-btn="Print" data-i="' + i + '"' + (PERM.Print ? '' : ' disabled') + '>Print</button></td>' +
                '<td class="win-cell-btn"><button type="button" class="win-edit" data-btn="Edit" data-i="' + i + '"' + (PERM.Update ? '' : ' disabled') + '>Edit</button></td>';
        });
        show('histEmpty', HIST.rows.length === 0);
    }
    /** btnShowHistory_Click -> BindGrid. */
    function historyShow(btn) {
        var run = function () {
            return postJson(API + '/history', {
                dateBy: (document.querySelector('input[name="dateBy"]:checked') || {}).value || 'doc',
                fromChecked: F.checked('FromDateHistoryChk'), toChecked: F.checked('ToDateHistoryChk'),
                fromDate: val('FromDateHistory'), toDate: val('ToDateHistory'), invoiceId: netI(val('CmbInvoicenoHistory'))
            }).then(function (rows) { HIST.rows = rows || []; HIST.cur = -1; histRender(); }).catch(function (e) { box(e.message); });
        };
        return btn ? busy(btn, run) : run();
    }
    /** btnResetHistory_Click. */
    function historyReset() {
        setText('FromDateHistory', F.daysAgo(3)); setText('ToDateHistory', F.today());
        setVal('CmbInvoicenoHistory', '0');
        HIST.rows = []; HIST.cur = -1; histRender();
    }
    /** btnRefreshHistory_Click -> HistoryCombosFill. */
    function historyRefresh(btn) {
        return busy(btn, function () { return getJson(API + '/history-invoices').then(function (rows) { HIST_INV = rows || []; bindHistInv(); }).catch(function (e) { box(e.message); }); });
    }
    /** Print_Click -> CommonServices.ExImBillOfLading_Slip(cmbInvoiceNo.Value): 505-ExImBillOfLading_Slip.rpt. */
    function print() {
        if (!PERM.Print) return;
        var id = netI(val('cmbInvoiceNo'));
        if (id === 0) { box('No Record Found For Display'); return; }
        F.printSeeded('505-eximbilloflading-slip', { invoiceId: id });
    }
    /** GenerateGrdSlip(EximInvoiceId) -> CommonServices.ExportInvoiceSlip_520: 530-EximBillOfLadingSlip.rpt (+ 529 container sub-report). */
    function printRow(i) {
        var r = HIST.rows[i]; if (!r) return;
        if (netI(r.EximInvoiceId) === 0) { box('No Record Found For Display'); return; }
        F.printSeeded('530-eximbillofladingslip', { invoiceId: r.EximInvoiceId });
    }
    function attachment() { box('Attachments (DMS popup) are not part of the web port.'); }
    function attachmentsLink(i) { var r = HIST.rows[i]; if (r) box('Attachments of record ' + r.Id + ': ' + netI(r.NoOfAttachments)); }
    function shortcuts() {
        F.shortcutPopup([['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'],
            ['Ctrl+F5', 'For Focus on Booking Combo'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
            ['Ctrl+Enter', 'For Update Record When Focus On Any Grid '], ['Ctrl+Up', 'For Focus On Booking Number '], ['Ctrl+Down', 'For Focus On grid '],
            ['Ctrl+Space', "To Call Function's On Button Or Link When Focus On Any Grid "]]);
    }
    function tab(id) { F.innerTab('main', id, 'btnBlFooterHistory', function (p, onHist) { focus(onHist ? 'FromDateHistory' : 'cmbInvoiceNo'); }); }
    function toggleHistory() { tab(F.activeInner('main') === 'tabHistory' ? 'tabPage1' : 'tabHistory'); }

    document.addEventListener('DOMContentLoaded', function () {
        F.wireTabs(function (p, onHist) { focus(onHist ? 'FromDateHistory' : 'cmbInvoiceNo'); }, 'btnBlFooterHistory');
        F.on('cmbInvoiceNo', 'change', invoiceLeave);
        F.wireGrid('histBody', {
            select: function (i) { HIST.cur = i; },
            open: function (i) { var r = HIST.rows[i]; if (r) readById(r.Id); },
            btn: function (name, i) { if (name === 'Print') printRow(i); else if (name === 'Edit') readById(HIST.rows[i].Id); },
            link: function (k, i) { attachmentsLink(i); }
        });
        /* EximShipmentInfo_KeyDown */
        document.addEventListener('keydown', function (e) {
            if (F.enterMoves(e)) return;
            var k = (e.key || '').toLowerCase(), onForm = F.activeInner('main') !== 'tabHistory';
            if (e.ctrlKey && k === 's' && onForm && !S.updateMode) { e.preventDefault(); save($id('btnsave')); }
            if (e.ctrlKey && k === 'n') { e.preventDefault(); formReset(); }
            if (e.ctrlKey && k === 't') { e.preventDefault(); toggleHistory(); }
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); F.cancel(); }
            if (e.ctrlKey && k === 'u' && S.updateMode) { e.preventDefault(); update($id('btnUpdate')); }
            if (e.ctrlKey && k === 'p' && PERM.Print) { e.preventDefault(); print(); }
            if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh($id('BtnRefresh')); }
            if (e.ctrlKey && (e.key === 'F5' || e.key === 'ArrowUp')) { e.preventDefault(); focus('txtBookingNo'); }
            if (e.ctrlKey && e.key === 'F10') { e.preventDefault(); attachment(); }
            if (e.ctrlKey && e.key === 'Enter' && PERM.Update && HIST.cur >= 0) { e.preventDefault(); readById(HIST.rows[HIST.cur].Id); }
            if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcuts(); }
            if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); var tr = $id('histBody').querySelector('tr'); if (tr) tr.scrollIntoView(); }
            if (e.ctrlKey && e.key === ' ' && HIST.cur >= 0) { e.preventDefault(); if (PERM.Update) readById(HIST.rows[HIST.cur].Id); }
        });
        load();
    });

    global.ExportBL = { formReset: formReset, refresh: refresh, save: save, update: update, attachment: attachment, print: print, shortcuts: shortcuts,
        historyReset: historyReset, historyRefresh: historyRefresh, historyShow: historyShow, toggleHistory: toggleHistory };
}(window));
