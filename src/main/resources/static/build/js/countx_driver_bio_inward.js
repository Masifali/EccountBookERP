/* frmDriverBio (Architecture.WinApp.Sale/frmDriverBio.cs) with RefDocumentTypeId = 51 - opened from Inward Gate Pass
   (BtnDriverForm_Click :5321 and, after Save with chkDriverInfoForm, with the saved gate pass: ?gatePassId=).
   Line references are to frmDriverBio.cs. Server: /purchase/driver-bio/api (DriverBioInwardService). */
(function () {
    'use strict';
    const API = '/purchase/driver-bio/api';
    const field = id => document.getElementById(id);
    const escapeHtml = v => String(v ?? '').replace(/[&<>"']/g, c => ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c]));
    const pad = n => String(n).padStart(2, '0');
    const isoLocal = (d = new Date()) => d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()) + 'T' + pad(d.getHours()) + ':' + pad(d.getMinutes()) + ':' + pad(d.getSeconds());
    const MONTHS = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    function parse(v) { if (!v) return null; const d = new Date(String(v).replace(' ', 'T')); return isNaN(d) ? null : d; }
    function fmtDate(v) { const d = parse(v); return d ? pad(d.getDate()) + '-' + MONTHS[d.getMonth()] + '-' + String(d.getFullYear()).slice(2) : escapeHtml(v ?? ''); }          // dd-MMM-yy
    function fmtDateTime(v) { const d = parse(v); if (!d) return escapeHtml(v ?? ''); let h = d.getHours(); const ap = h >= 12 ? 'PM' : 'AM'; h = h % 12 || 12;
        return pad(d.getDate()) + '-' + MONTHS[d.getMonth()] + '-' + d.getFullYear() + ' ' + pad(h) + ':' + pad(d.getMinutes()) + ' ' + ap; }                                      // dd-MMM-yyyy hh:mm tt

    let recId = 0, rights = {}, pendingRows = [], knownDrivers = [], historyDays = 3, gpDateTime = isoLocal();

    async function api(url, options) {
        return PurchaseRequest.track(async () => {
            const response = await fetch(url, options);
            const text = await response.text(); let data;
            try { data = text ? JSON.parse(text) : null; } catch (_) { throw Error('The session expired or the server returned an invalid response. Please sign in and retry.'); }
            if (!response.ok) throw Error(data?.message || data?.detail || 'Request failed (' + response.status + ')');
            return data;
        });
    }
    const showError = e => alert(e?.message || 'Request failed. Please retry.');
    const busy = (btn, work) => PurchaseRequest.run(btn, work).catch(showError);

    /* MaskedTextBox masks: txtCNIC "00000-0000000-0", txtDriverCellNo / txtAlternateCellNo "0092-300-0000000" */
    const MASKS = { txtCNIC: '00000-0000000-0', txtDriverCellNo: '0000-000-0000000', txtAlternateCellNo: '0000-000-0000000' };
    function maskText(value, mask) { const digits = String(value ?? '').replace(/\D/g, ''); let out = '', i = 0;
        for (const ch of mask) { if (i >= digits.length) break; out += ch === '0' ? digits[i++] : ch; } return out; }
    const CELL_DEFAULT = '0092-3';   // txtDriverCellNo.Text = "0092-3-"

    /* DriverFieldsDisableOrEnable(flag) */
    function driverFieldsEnabled(flag) { for (const id of ['txtCNIC', 'txtDriverCellNo', 'txtDriverName', 'txtAlternateCellNo', 'txtFatherName']) field(id).disabled = !flag; }

    /* GpNoBind(dtPending) + pendingGatepass() grid */
    function bindPending(rows) {
        pendingRows = rows || [];
        const sel = field('cmbGatePass'), keep = sel.value;
        sel.innerHTML = '<option value=""></option>' + pendingRows.map(r => `<option value="${Number(r.Id)}" data-vehicle="${escapeHtml(r.VehicleNo)}" data-date="${fmtDate(r.GpDate)}" data-bilty="${escapeHtml(r.BiltyNo)}" data-type="${escapeHtml(r.GatepassType)}">${escapeHtml(r.GpSrNo)}</option>`).join('');
        if (keep && Array.from(sel.options).some(o => o.value === keep)) sel.value = keep;
        const body = field('grdOutstandingGatePassBody');
        body.innerHTML = '';
        const num = v => `<td class="num">${escapeHtml(v ?? '')}</td>`, txt = v => `<td>${escapeHtml(v ?? '')}</td>`;
        pendingRows.forEach(r => {
            const tr = document.createElement('tr'); tr.tabIndex = -1;
            tr.innerHTML = `<td><button type="button" class="tool-btn" style="height:20px;padding:0 5px;">Load</button></td>` + txt(r.DocumentType) + txt(r.GatepassType)
                + num(r.GpSrNo) + `<td>${fmtDate(r.GpDate)}</td>` + txt(r.VehicleNo) + txt(r.BiltyNo) + txt(r.Container) + txt(r.Container1)
                + num(r.FirstWeight) + num(r.SecondWeight) + num(r.NetWeight);
            const load = () => { if (field('cmbGatePass').disabled) return; field('cmbGatePass').value = String(r.Id); return busy(null, gatePassChanged); };   // grdOutstandingGatePass_ColumnButtonClick "Load"
            tr.querySelector('button').onclick = e => { e.stopPropagation(); load(); };
            tr.onclick = () => select(tr);
            tr.onkeydown = e => { if (e.ctrlKey && e.key === ' ') { e.preventDefault(); load(); } else arrows(e, tr); };
            body.appendChild(tr);
        });
    }
    function select(tr) { tr.parentElement.querySelectorAll('tr.selected').forEach(r => r.classList.remove('selected')); tr.classList.add('selected'); }
    function arrows(e, tr) { if (e.key !== 'ArrowDown' && e.key !== 'ArrowUp') return; e.preventDefault(); const n = e.key === 'ArrowDown' ? tr.nextElementSibling : tr.previousElementSibling; if (n) { select(n); n.focus(); } }
    function focusGrid(bodyId) { const b = field(bodyId); const tr = b.querySelector('tr.selected') || b.querySelector('tr'); if (tr) { select(tr); tr.focus(); } }

    /* cmbGatePass_TextChanged (Leave): ReadByGpNoForExportDriverInformation with the GP number TEXT and DocumentTypeId 51.
       That activity reads GatePassOutward only, so for an inward pass it returns no row and the desktop clears the read-only
       fields and sets the date to Now - reproduced (the pending grid shows the pass's vehicle / bilty). */
    async function gatePassChanged() {
        const sel = field('cmbGatePass'), text = sel.selectedOptions[0]?.textContent.trim() || '';
        if (text === '' || text === '0') return;
        const rows = await api(API + '/gate-pass?gpSrNo=' + encodeURIComponent(parseInt(text, 10) || 0));
        if (rows.length) {
            const r = rows[0];
            field('txtBiltyNo').value = r.BiltyNo ?? ''; field('txtVehicleNo').value = r.VehicleNo ?? '';
            setGpDate(r.GpDate ? String(r.GpDate).replace(' ', 'T').slice(0, 19) : isoLocal());
            field('txtContainer').value = String(r.Container ?? '') + '0';          // desktop: Container + "0"
            field('txtSealNo').value = r.OtherRemarks ?? ''; field('txtNetWeight').value = r.FactoryWeight ?? '';
        } else {
            for (const id of ['txtBiltyNo', 'txtVehicleNo', 'txtContainer', 'txtSealNo', 'txtNetWeight']) field(id).value = '';
            setGpDate(isoLocal());
        }
        field('txtCNIC').focus();
    }
    function setGpDate(iso) { gpDateTime = iso.length === 10 ? iso + 'T00:00:00' : iso; field('txtGatepassDate').value = gpDateTime.slice(0, 10); }

    /* txtCNIC_Leave / txtDriverCellNo_Leave: globalAllDriverBioInfo.Find(exact text) */
    function driverLeave(kind) {
        const value = field(kind === 'cnic' ? 'txtCNIC' : 'txtDriverCellNo').value.trim();
        const d = knownDrivers.find(r => String(kind === 'cnic' ? r.cnicNo : r.cellNo ?? '') === value);
        if (d) {
            if (kind === 'cnic') field('txtDriverCellNo').value = d.cellNo ?? ''; else field('txtCNIC').value = d.cnicNo ?? '';
            field('txtDriverName').value = d.driverName ?? ''; field('txtAlternateCellNo').value = d.alternateCellNo ?? ''; field('txtFatherName').value = d.fatherName ?? '';
            driverFieldsEnabled(false);
        } else driverFieldsEnabled(true);
    }

    /* FormReset() */
    async function formReset() {
        recId = 0;
        field('btnsave').style.display = ''; field('btnUpdate').style.display = 'none';
        for (const id of ['txtBiltyNo', 'txtForwarderName', 'txtNetWeight', 'txtRemarks', 'txtSealNo', 'txtVehicleNo', 'txtContainer', 'txtCNIC', 'txtDriverName', 'txtFatherName']) field(id).value = '';
        field('txtDriverCellNo').value = CELL_DEFAULT; field('txtAlternateCellNo').value = CELL_DEFAULT;
        field('cmbGatePass').value = ''; field('cmbGatePass').disabled = false;
        driverFieldsEnabled(true);
        const data = await api(API + '/refresh');                               // pendingGatepass() + driverBiodata.ReadAll
        knownDrivers = data.knownDrivers || []; bindPending(data.pendingGatePasses);
        field('cmbGatePass').focus();
    }

    /* ReadById(Id) */
    async function readById(id) {
        const r = await api(API + '/' + Number(id));
        switchView('Form');
        recId = Number(r.Id) || Number(id);
        field('btnsave').style.display = 'none'; field('btnUpdate').style.display = '';
        const sel = field('cmbGatePass');
        // cmbGatePass.Value = GatePassOutwardId. A pass that already has driver info is not in the pending list, so the
        // desktop combo shows it blank; the web adds it so the saved GatePassOutwardId is kept on Update.
        if (!Array.from(sel.options).some(o => o.value === String(r.GatePassOutwardId))) sel.insertAdjacentHTML('beforeend', `<option value="${Number(r.GatePassOutwardId)}">${escapeHtml(r.GpSrNo)}</option>`);
        sel.value = String(r.GatePassOutwardId);
        setGpDate(r.GpDate ? String(r.GpDate).replace(' ', 'T').slice(0, 19) : isoLocal());
        field('txtVehicleNo').value = r.VehicleNo ?? ''; field('txtBiltyNo').value = r.BiltyNo ?? ''; field('txtContainer').value = r.Container ?? '';
        field('txtNetWeight').value = r.FactoryWeight ?? '';                        // txtSealNo keeps its text (txtSealNo.Text = txtSealNo.Text)
        field('txtForwarderName').value = r.ForwarderName ?? ''; field('txtCNIC').value = r.CnicNo ?? ''; field('txtDriverName').value = r.DriverName ?? '';
        field('txtDriverCellNo').value = r.DriverCellNo ?? ''; field('txtAlternateCellNo').value = r.AlternateCellNo ?? ''; field('txtFatherName').value = r.FatherName ?? '';
        if (Number(r.driverBiodataId) > 0) driverFieldsEnabled(false);
        field('txtRemarks').value = r.RemarksHeader ?? '';
        sel.disabled = true;
    }

    /* Insert(): FormValidation (the server repeats it), confirmation, Save/Update, message, FormReset, Preview print */
    async function insert(updating) {
        if (updating && !recId) throw Error('Record Not Update because RecId Not Found');
        const cnic = field('txtCNIC').value.trim();
        const checks = [
            [cnic === '-       -' || cnic.length < 15 || !/^\d{5}-\d{7}-\d$/.test(cnic), 'CNIC Field Required', 'txtCNIC'],
            [!/^\d{4}-\d{3}-\d{7}$/.test(field('txtDriverCellNo').value.trim()), 'Cell No. Field Required', 'txtDriverCellNo'],
            [!field('txtDriverName').value.trim(), 'Driver Name  Field Required', 'txtDriverName'],
            [!field('txtFatherName').value.trim(), 'Father Name  Field Required', 'txtFatherName'],
            [!field('txtForwarderName').value.trim(), 'Forwarder Name Field Required', 'txtForwarderName']];
        for (const [bad, message, id] of checks) if (bad) { alert(message); field(id).disabled = false; field(id).focus(); return; }
        if (!confirm(updating ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        const body = { id: updating ? recId : 0, gatePassOutwardId: Number(field('cmbGatePass').value) || 0, gpDate: gpDateTime,
            forwarderName: field('txtForwarderName').value, driverName: field('txtDriverName').value, fatherName: field('txtFatherName').value,
            cnicNo: field('txtCNIC').value, driverCellNo: field('txtDriverCellNo').value, alternateCellNo: field('txtAlternateCellNo').value,
            remarksHeader: field('txtRemarks').value };
        const data = await api(API + '/save', { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(body) });
        alert(data.message);
        await formReset();
        if (field('ChkBox').checked) generateReport(data.id);
    }

    /* GenerateReport(PrintId): Sp_GatePassOutwardDriverInfo_rpt -> 07_RptGatepassOutwardDriverInfoSlipA.rpt */
    function generateReport(id) {
        if (!(Number(id) > 0)) { alert('Not Record Found For Display'); return; }
        if (window.printRpt) return window.printRpt('07_RptGatepassOutwardDriverInfoSlipA.rpt', { id: Number(id) });
        alert('The print runtime is not loaded on this page.');
    }

    /* HistoryGridFill() */
    async function historyGridFill() {
        const q = new URLSearchParams({ dateField: document.querySelector('input[name="histDate"]:checked').value,
            fromDoc: parseInt(field('FromDocNoHistory').value, 10) || 0, toDoc: parseInt(field('ToDocNoHistory').value, 10) || 0 });
        if (field('chkFromDateHistory').checked && field('FromDateHistory').value) q.set('fromDate', field('FromDateHistory').value);
        if (field('chkToDateHistory').checked && field('ToDateHistory').value) q.set('toDate', field('ToDateHistory').value);
        const rows = await api(API + '/history?' + q);
        const body = field('DataGridHistoryBody'); body.innerHTML = '';
        const num = v => `<td class="num">${escapeHtml(v ?? '')}</td>`, txt = v => `<td>${escapeHtml(v ?? '')}</td>`;
        rows.forEach(r => {
            const id = Number(r.Id), tr = document.createElement('tr'); tr.tabIndex = -1;
            tr.innerHTML = `<td><button type="button" class="tool-btn" data-act="edit" style="height:20px;padding:0 5px;">Edit</button></td>`
                + `<td><button type="button" class="tool-btn" data-act="print" style="height:20px;padding:0 5px;">Print</button></td>`
                + txt(r.GpDocumentType) + txt(r.DriverCNIC) + txt(r.DriverName) + txt(r.FatherName) + txt(r.ForwarderName) + txt(r.DriverCellNo) + txt(r.AlternateCellNo)
                + txt(r.GatepassType) + `<td><a href="#" class="code-link" data-act="edit">${escapeHtml(r.GpSrNo)}</a></td>` + `<td>${fmtDate(r.GpDate)}</td>` + txt(r.VehicleNo) + txt(r.BiltyNo)
                + txt(r.Container) + txt(r.Container1) + num(r.FirstWeight) + num(r.SecondWeight) + num(r.NetWeight)
                + `<td>${fmtDateTime(r.EntryDate)}</td>` + txt(r.EntryUser) + `<td>${fmtDateTime(r.ModifyDate)}</td>` + txt(r.ModifyUser) + txt(r.RemarksHeader)
                + `<td class="num"><a href="#" class="code-link" data-act="attach">${escapeHtml(r.NoOfAttachments ?? 0)}</a></td>`;
            tr.onclick = e => {
                select(tr);
                const act = e.target.closest('[data-act]')?.dataset.act; if (!act) return;
                e.preventDefault();
                if (act === 'edit') busy(e.target.closest('button'), () => readById(id));              // DataGridHistory_ColumnButtonClick "Edit"
                if (act === 'print') generateReport(id);                                                  // "Print"
                if (act === 'attach') alert('Driver Bio attachments (CommonServices.GetNoofAttachmentsByScreenName, frmDriverBioForInWard) are not ported to the web yet.');
            };
            tr.ondblclick = () => busy(null, () => readById(id));                                        // DataGridHistory_DoubleClick
            tr.onkeydown = e => {
                if (e.ctrlKey && (e.key === 'Enter' || e.key === ' ')) { e.preventDefault(); busy(null, async () => { await formReset(); await readById(id); }); }   // DataGridHistory_KeyDown
                else arrows(e, tr);
            };
            body.appendChild(tr);
        });
        field('lblHistoryCount').textContent = rows.length;
    }

    /* BtnNewHistory_Click */
    function historyReset() {
        const from = new Date(); from.setDate(from.getDate() - 3);
        field('FromDateHistory').value = isoLocal(from).slice(0, 10); field('ToDateHistory').value = isoLocal().slice(0, 10);
        field('FromDocNoHistory').value = ''; field('ToDocNoHistory').value = '';
        field('DataGridHistoryBody').innerHTML = ''; field('lblHistoryCount').textContent = '0';
        field('drdocdate').checked = true;
    }

    function isHistory() { return field('viewHistory').style.display !== 'none'; }
    /* tabControl1_SelectedIndexChanged: History -> FromDateHistory.Focus(), Form -> txtCNIC.Focus() */
    function switchView(mode) {
        const h = mode === 'History';
        field('viewForm').style.display = h ? 'none' : ''; field('viewHistory').style.display = h ? '' : 'none';
        field('btnModeHistory').textContent = h ? 'Form' : 'History';
        (h ? field('FromDateHistory') : field('txtCNIC')).focus();
    }

    /* MakeShortCutKeys() */
    function shortcuts() {
        alert(['Ctrl+S  For Save in Form Tab and For Show History in History Tab', 'Ctrl+U  For Update', 'Ctrl+E  For Close', 'Ctrl+R  For Refresh', 'Ctrl+N  For New',
            'Ctrl+P  For Print', 'Alt+1  For Print', 'Ctrl+F5  For Focus on Gp No', 'Ctrl+F10  For Open Attachments', 'Ctrl+T  For Tab Transfer', 'Ctrl+alt  To Show ShortCut Keys Form',
            'Ctrl+ArrowDown  For Focus On Pending GP Grid when focus in form tab and for focus on history grid when in history tab',
            'Ctrl+ArrowUp  For Focus on CNIC in Pending GP Grid when in Form tab and For focus on FromDate in history tab',
            'Ctrl+Enter  When Focus On Any Grid For Update Record', 'Ctrl+Space  When Focus On Any Grid To Call Function\'s On Button Or Link'].join('\n'));
    }
    const attachmentsNotPorted = () => alert('Driver Bio attachments (the desktop Attachment form, ScreenName frmDriverBioForInWard) are not ported to the web yet.');

    /* frmDriverBio_KeyDown */
    document.addEventListener('keydown', e => {
        const k = e.key.toLowerCase();
        if (e.ctrlKey && e.altKey && (k === 'control' || k === 'alt')) { shortcuts(); return; }
        if (e.ctrlKey && k === 't') { e.preventDefault(); switchView(isHistory() ? 'Form' : 'History'); return; }
        if (e.key === 'Enter' && !e.ctrlKey && e.target.tagName === 'INPUT' && !['checkbox', 'radio'].includes(e.target.type)) {   // Keys.Return -> {TAB}
            const list = Array.from(document.querySelectorAll((isHistory() ? '#viewHistory' : '#viewForm') + ' input, ' + (isHistory() ? '#viewHistory' : '#viewForm') + ' select'))
                .filter(el => !el.disabled && el.offsetParent !== null && !el.classList.contains('dtcombo-native'));
            const i = list.indexOf(e.target); if (i >= 0 && list[i + 1]) { e.preventDefault(); list[i + 1].focus(); }
        }
        if (!isHistory()) {
            if (e.ctrlKey && k === 'n') { e.preventDefault(); busy(field('btnnew'), formReset); }
            if (e.ctrlKey && k === 's') { e.preventDefault(); if (field('btnsave').style.display !== 'none' && !field('btnsave').disabled) busy(field('btnsave'), () => insert(false)); }
            if (e.ctrlKey && k === 'r') { e.preventDefault(); busy(field('btnRefresh'), refresh); }
            if (e.ctrlKey && k === 'u') { e.preventDefault(); if (field('btnUpdate').style.display !== 'none' && !field('btnUpdate').disabled) busy(field('btnUpdate'), () => insert(true)); }
            if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); field('cmbGatePass').__dtcombo?.input.focus(); }
            if (e.ctrlKey && e.key === 'F10') { e.preventDefault(); attachmentsNotPorted(); }
            if (e.ctrlKey && e.key === 'ArrowDown' && !e.target.closest('tbody')) { e.preventDefault(); focusGrid('grdOutstandingGatePassBody'); }
            if (e.ctrlKey && e.key === 'ArrowUp') { e.preventDefault(); field('txtCNIC').focus(); }
            if (((e.ctrlKey && k === 'p') || (e.altKey && (e.key === '1'))) && rights.canPrint) { e.preventDefault(); generateReport(recId); }
        } else {
            if (e.ctrlKey && k === 's') { e.preventDefault(); busy(field('btnShowHistory'), historyGridFill); }
            if (e.ctrlKey && k === 'n') { e.preventDefault(); historyReset(); }
            if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); field('FromDateHistory').focus(); }
            if (e.ctrlKey && e.key === 'ArrowDown' && !e.target.closest('tbody')) { e.preventDefault(); focusGrid('DataGridHistoryBody'); }
            if (e.ctrlKey && e.key === 'ArrowUp') { e.preventDefault(); field('FromDateHistory').focus(); }
        }
    });

    /* btnRefresh_Click: driverBiodata.ReadAll + pendingGatepass() */
    async function refresh() { const data = await api(API + '/refresh'); knownDrivers = data.knownDrivers || []; bindPending(data.pendingGatePasses); }

    document.addEventListener('DOMContentLoaded', () => {
        if (window.DesktopCombo) DesktopCombo.define('driverGatePass', [   /* GpNoBind: Gp No, VehicleNo, GpDate, BiltyNo, GatePassType (Id hidden) */
            { caption: 'Gp No', flex: 1 }, { caption: 'Vehicle No', flex: 2, key: 'vehicle' }, { caption: 'Gp Date', flex: 2, key: 'date' },
            { caption: 'Bilty No', flex: 2, key: 'bilty' }, { caption: 'Gate Pass Type', flex: 2, key: 'type' }]);
        if (window.PurchaseChrome) {
            PurchaseChrome.footer({ isHistory, toggle: () => switchView(isHistory() ? 'Form' : 'History'), watch: '#viewHistory' });
            field('bottomModeBar').style.display = 'none';
            PurchaseChrome.fullscreen('#wrapPending', 'pending gate passes'); PurchaseChrome.fullscreen('#wrapHistory', 'history');
        }
        field('btnModeHistory').onclick = () => switchView(isHistory() ? 'Form' : 'History');
        field('btnnew').onclick = e => busy(e.currentTarget, formReset);
        field('btnRefresh').onclick = e => busy(e.currentTarget, refresh);
        field('btnsave').onclick = e => busy(e.currentTarget, () => insert(false));
        field('btnUpdate').onclick = e => busy(e.currentTarget, () => insert(true));
        field('Print').onclick = () => generateReport(recId);
        field('btnattachment').onclick = attachmentsNotPorted;
        field('BtnShortCutkeys').onclick = shortcuts;
        field('btnShowHistory').onclick = e => busy(e.currentTarget, historyGridFill);
        field('BtnNewHistory').onclick = historyReset;
        field('cmbGatePass').addEventListener('change', () => busy(null, gatePassChanged));    // Leave -> cmbGatePass_TextChanged
        field('txtGatepassDate').addEventListener('change', () => { const v = field('txtGatepassDate').value; if (v) gpDateTime = v + gpDateTime.slice(10); });
        for (const [id, mask] of Object.entries(MASKS)) field(id).addEventListener('input', () => { field(id).value = maskText(field(id).value, mask); });
        field('txtCNIC').addEventListener('blur', () => driverLeave('cnic'));
        field('txtDriverCellNo').addEventListener('blur', () => driverLeave('cell'));
        for (const id of ['FromDocNoHistory', 'ToDocNoHistory']) field(id).addEventListener('input', () => { field(id).value = field(id).value.replace(/\D/g, ''); });   // OnlytextNumberFunction
        historyReset(); setGpDate(isoLocal());
        field('txtDriverCellNo').value = CELL_DEFAULT; field('txtAlternateCellNo').value = CELL_DEFAULT;
        busy(null, async () => {
            const data = await api(API + '/initial');                        // frmDriverBio_Load
            rights = data;
            field('btnsave').disabled = !data.canSave; field('Print').disabled = !data.canPrint; field('btnUpdate').disabled = !data.canUpdate;
            knownDrivers = data.knownDrivers || []; historyDays = Number(data.historyDays) || 3;
            const from = new Date(); from.setDate(from.getDate() - historyDays); field('FromDateHistory').value = isoLocal(from).slice(0, 10);
            bindPending(data.pendingGatePasses);
            // InwardGatePass after Save: obj.cmbGatePass.Value = success; obj.cmbGatePass_TextChanged(); obj.Show()
            const pre = Number(new URLSearchParams(location.search).get('gatePassId'));
            if (pre > 0 && Array.from(field('cmbGatePass').options).some(o => o.value === String(pre))) { field('cmbGatePass').value = String(pre); await gatePassChanged(); }
            else field('cmbGatePass').__dtcombo?.input.focus();
        });
    });
})();
