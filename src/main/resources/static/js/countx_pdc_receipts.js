/* Pdc Receipts - Architecture.WinApp.Account_Definition.AcfrmDefPdcManagment (screen 45, DocumentTypeId 16).
   Event-for-event port of AcfrmDefPdcManagment.cs. */
(function ($) {
    'use strict';
    var API = '/accounts/pdc-receipts';
    var RecId = 0, VoucherHeadId = 0, uidSeq = 0, updateRow = null;
    var table = [];                 // the DataTable behind grdfrm
    var attSaved = [];              // Attachment.lst rows already stored (ReadById)
    var attNew = [];                // files picked in the Attachment form since the last save
    var docTime = '00:00:00';       // DateTimePicker.Value keeps the time of day
    var lastFilter = null, reportCount = 0;      // dtvoucher: the table of the last "Show"
    var lastDocLookup = '';

    function busy(btn, on) { var $b = $(btn); if (on) $b.prop('disabled', true).addClass('btn-busy'); else $b.removeClass('btn-busy').prop('disabled', false); }
    function isBusy(btn) { return $(btn).hasClass('btn-busy'); }
    function say(m) { if (m != null && m !== '') window.alert(m); }
    function failText(x) {
        try { var j = x.responseJSON || JSON.parse(x.responseText); if (j && j.message) return j.message; } catch (e) { /* ignore */ }
        return x && x.status ? 'HTTP ' + x.status : 'Request failed';
    }
    function call(method, path, data) {
        return new Promise(function (resolve, reject) {
            var o = { url: API + path, type: method, dataType: 'json', cache: false };
            if (method === 'POST') { o.contentType = 'application/json'; o.data = JSON.stringify(data == null ? {} : data); } else o.data = data || {};
            $.ajax(o).done(function (r) { if (r && r.success === false) reject(new Error(r.message || 'Error')); else resolve(r); })
                .fail(function (x) { reject(new Error(failText(x))); });
        });
    }
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function nowParts() { var d = new Date(); return { date: d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()), time: pad(d.getHours()) + ':' + pad(d.getMinutes()) + ':' + pad(d.getSeconds()) }; }
    /* Conversion.ToDouble (string) */
    function toDouble(s) { var t = String(s == null ? '' : s).trim(); if (t === '') return 0; var n = Number(t); return isFinite(n) ? n : 0; }
    /* Conversion.ToInt = Convert.ToInt32(string): digits only, otherwise 0 */
    function toInt(s) { var t = String(s == null ? '' : s).trim(); if (!/^[+-]?\d+$/.test(t)) return 0; var n = parseInt(t, 10); return (n > 2147483647 || n < -2147483648) ? 0 : n; }
    /* "#,#" : thousands separators, no decimals, zero shows nothing */
    function fmtHashInt(v) {
        var n = Math.round(toDouble(v)); if (n === 0) return '';
        return (n < 0 ? '-' : '') + String(Math.abs(n)).replace(/\B(?=(\d{3})+(?!\d))/g, ',');
    }
    /* "0,0" */
    function fmtZeroComma(v) {
        var n = Math.round(toDouble(v));
        return (n < 0 ? '-' : '') + String(Math.abs(n)).replace(/\B(?=(\d{3})+(?!\d))/g, ',');
    }
    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    /* DateTime.ToString("dd-MMM-yy") of 'yyyy-MM-dd...' */
    function fmtDMY(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v == null ? '' : v));
        return m ? m[3] + '-' + MON[+m[2] - 1] + '-' + m[1].substring(2) : (v == null ? '' : String(v));
    }
    /* 'yyyy-MM-dd HH:mm:ss' -> the DataTable cell text of a DateTime column */
    function fmtDt(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})(?:[ T](\d{2}):(\d{2}):(\d{2}))?/.exec(String(v == null ? '' : v));
        if (!m) return v == null ? '' : String(v);
        return (+m[2]) + '/' + (+m[3]) + '/' + m[1] + ' ' + (m[4] ? (+m[4] % 12 === 0 ? 12 : +m[4] % 12) : 12) + ':' + (m[5] || '00') + ':' + (m[6] || '00') + ' ' + ((m[4] ? +m[4] : 0) >= 12 ? 'PM' : 'AM');
    }
    function isDtText(v) { return /^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}$/.test(String(v)); }
    function sizeMb(bytes) { return Math.round(bytes / 1048576 * 100) / 100; }

    /* ---------------------------------------------------------------- combos (DDL.BindDDL ..., ZeroIndex: false) */
    function fill(sel, rows, keyV, keyT) {
        if (rows && rows.length > 0) { AccF.fillSelect(sel, rows.map(function (r) { return { V: CJG.pick(r, keyV), T: CJG.pick(r, keyT) }; }), 'V', 'T', null); clearCombo(sel); }
    }
    function comboVal(sel) { var v = $(sel).val(); return v == null ? '' : String(v); }
    function comboText(sel) { return comboVal(sel) === '' ? '' : $(sel + ' option:selected').text(); }
    function setCombo(sel, v) {
        var s = v == null ? '' : String(v);
        if (!$(sel).find('option').filter(function () { return this.value === s; }).length) s = '';
        $(sel).val(s).trigger('change');
    }
    function clearCombo(sel) { $(sel).val('').trigger('change'); }
    function activateRow(sel, i) {
        var o = $(sel).find('option').eq(i);
        if (o.length) $(sel).val(o.val()).trigger('change');
    }
    function setDocNo(n) { $('#txtdocno').val(String(n)); lastDocLookup = String(n); }
    function applyLoad(r) {
        if (r.banks && r.banks.length > 0) fill('#cmbbank', r.banks, 'Id', 'BankName');
        if (r.accounts && r.accounts.length > 0) {
            fill('#CmbAccountTitleCredit', r.accounts, 'Id', 'AccountTitle'); fill('#CmbAccountTitleDebit', r.accounts, 'Id', 'AccountTitle');
            fill('#CmbAccountTitle', r.accounts, 'Id', 'AccountTitle'); fill('#cmbDebitAccountrpt', r.accounts, 'Id', 'AccountTitle');
        }
        if (r.docNo != null && r.docNo > 0) setDocNo(r.docNo);          // DocumentNoFill: only when Code > 0
        if (r.rights) applyRights(r.rights);
    }
    function applyRights(rt) {
        $('#btnsave').prop('disabled', !rt.canSave); $('#btnUpdate').prop('disabled', !rt.canUpdate); $('#btnDelete').prop('disabled', !rt.canDelete);
    }
    /* CheqStatusReprots: Pending / Clear / All, "All" active */
    function statusFill() {
        AccF.fillSelect('#CmbChequeStatusReprot', [{ V: 1, T: 'Pending' }, { V: 2, T: 'Clear' }, { V: 3, T: 'All' }], 'V', 'T', null);
        activateRow('#CmbChequeStatusReprot', 2);
    }

    /* ---------------------------------------------------------------- grids */
    function rowOf(r) { for (var i = 0; i < table.length; i++) if (table[i].__uid === r.__uid) return table[i]; return null; }
    var DEL_ICON = '<svg viewBox="0 0 16 16" width="14" height="14"><path d="M3 4h10l-1 10H4z" fill="#fff" stroke="#b33"/><path d="M2 4h12M6 2h4" stroke="#b33"/></svg>';
    var grdfrm = CJG.create({
        table: '#grdfrm', nav: '#navFrm', autoResize: true,
        columns: [{ key: 'Id', hidden: true }, { key: 'BankId', hidden: true }, { key: 'BankName', caption: 'BankName', width: 120 },
                  { key: 'CheqDate', caption: 'CheqDate', width: 90, format: function (v) { return fmtDMY(v); } },
                  { key: 'CheqNo', caption: 'CheqNo', width: 100 },
                  { key: 'CheqAmount', caption: 'CheqAmount', width: 100, num: true, sum: true, format: function (v) { return fmtHashInt(v); }, totalFormat: function (t) { return fmtHashInt(t); } },
                  { key: 'Comments', caption: 'Comments', width: 150 }, { key: 'ChequeStatus', caption: 'ChequeStatus', width: 90 },
                  { key: 'Delete', caption: 'Delete', button: 'Delete', width: 35, buttonHtml: DEL_ICON, buttonTitle: 'Delete' }],
        onButton: function (col, row) { if (col === 'Delete') grdfrm_Delete(row); },
        onDblClick: function (row) { grdfrm_DoubleClick(row); }
    });
    function bindFrm() { grdfrm.setRows(table); }
    var rptCols = [];
    var grdReprot = CJG.create({ table: '#grdReprot', nav: '#navReprot', autoResize: false, columns: rptCols });
    var histCols = [];
    var grdHistory = CJG.create({
        table: '#grdHistory', nav: '#navHistory', autoResize: false, columns: histCols,
        onLink: function (col, row) { grdHistory_LinkClicked(col, row); },
        onDblClick: function (row) { grdHistory_DoubleClick(row); }
    });

    /* GridSettings (grdReprot) */
    function reportGrid(rows) {
        rptCols.length = 0;
        var hide = { Id: 1, Remarks: 1, MGLAccountCrId: 1, MGLAccountDrId: 1, AccountTitleDebit: 1, PaidOnDate: 1 };
        Object.keys(rows[0]).forEach(function (k) {
            if (hide[k]) rptCols.push({ key: k, hidden: true });
            else if (k === 'CheqAmount') rptCols.push({ key: k, caption: k, width: 90, num: true, sum: true, format: function (v) { return fmtZeroComma(v); }, totalFormat: function (t) { return fmtZeroComma(t); } });
            else rptCols.push({ key: k, caption: k, width: 100, format: function (v) { return isDtText(v) ? fmtDt(v) : (v == null ? '' : String(v)); } });
        });
        grdReprot.setRows(rows);
    }

    /* ---------------------------------------------------------------- tabs */
    function selectTab(i) {
        $('#pageForm').toggleClass('on', i === 0); $('#pageHistory').toggleClass('on', i === 1);
        $('#tabF').toggleClass('on', i === 0); $('#tabH').toggleClass('on', i === 1);
        /* tabControl1_SelectedIndexChanged: SelectedIndex == 1 -> HistoryFill(50) */
        if (i === 1) historyFill(50); else { grdfrm.rerender(); grdReprot.rerender(); }
    }
    function historyFill(n) {
        return call('GET', '/history', { n: n || 0 }).then(function (r) {
            var rows = r.rows || [];
            if (rows.length > 0) {
                histCols.length = 0;
                Object.keys(rows[0]).forEach(function (k) {
                    if (k === 'Id' || k === 'RecordNo') histCols.push({ key: k, hidden: true });
                    else if (k === 'NoOfAttachments') histCols.push({ key: k, caption: k, width: 100, link: true, num: true });
                    else histCols.push({ key: k, caption: k, width: (k === 'CreditAccount' || k === 'DebitAccount') ? 220 : k === 'RemarkHeader' ? 350 : 110,
                        format: function (v) { return isDtText(v) ? fmtDt(v) : (v == null ? '' : String(v)); } });
                });
                grdHistory.setRows(rows);
            } else grdHistory.clear();              // grdHistory.ClearStructure()
        }).catch(function (e) { say(e.message); });
    }

    /* ---------------------------------------------------------------- Reset / ResetDetail / New */
    function setSaveMode(isNew) { $('#btnsave').prop('hidden', !isNew); $('#btnUpdate').prop('hidden', isNew); }
    function setAddMode(updating) { $('#btnAdd').text(updating ? 'Update' : 'Add'); $('#btnCancel').prop('hidden', !updating); }
    function reset() {
        attSaved = []; attNew = [];
        RecId = 0; VoucherHeadId = 0; updateRow = null;
        $('#txtremarks').val('');
        setSaveMode(true);
        setAddMode(false);
        table = []; bindFrm();
        AccF.focus('#txtdocdate');
        return call('GET', '/code').then(function (r) { if (r.docNo > 0) setDocNo(r.docNo); }).catch(function (e) { say(e.message); });
    }
    function resetDetail() {
        $('#txtcheqamount').val(''); $('#txtcheqno').val(''); $('#txtComments').val('');
        AccF.focus('#cmbbank');
    }
    function resetReport() {
        clearCombo('#CmbAccountTitle');
        $('#ChkCheqFromdate, #ChkCheqToDate, #ChkFromdate, #ChkTodate').prop('checked', false);
        grdReprot.clear();
    }
    function btnnew_Click() {
        reset(); resetReport();
        clearCombo('#CmbAccountTitleCredit'); clearCombo('#CmbAccountTitleDebit'); clearCombo('#cmbbank');
    }

    /* ---------------------------------------------------------------- detail add / update / cancel / edit / delete */
    function formValidationDetail() {
        if (comboVal('#cmbbank') === '') { say('Bank Field Required'); AccF.focus('#cmbbank'); return false; }
        var no = $('#txtcheqno').val().trim();
        if (no === '' || no === '0') { say('Cheq No Field Required'); $('#txtcheqno').focus(); return false; }
        if (toDouble($('#txtcheqamount').val().trim()) === 0) { say('CheqAmount Field Required'); $('#txtcheqamount').focus(); return false; }
        return true;
    }
    function cheqDateValue() { return $('#txtcheqdate').val() || nowParts().date; }
    /* btnAdd_Click: the validation is skipped for the bank "CASH" */
    function btnAdd_Click() {
        if (comboText('#cmbbank') === 'CASH' || formValidationDetail()) {
            var amtText = $('#txtcheqamount').val();
            if (String(amtText).trim() === '') { say("Couldn't store <> in CheqAmount Column.  Expected type is Double."); return; }   // DataColumn(typeof(double))
            if ($('#btnAdd').text() === 'Add') {
                table.push({ Id: 0, BankId: comboVal('#cmbbank'), BankName: comboText('#cmbbank'), CheqDate: cheqDateValue(), CheqNo: $('#txtcheqno').val(),
                    CheqAmount: toDouble(amtText), Comments: $('#txtComments').val(), ChequeStatus: 'Pending', __uid: ++uidSeq });
            } else if (updateRow) {
                updateRow.BankId = comboVal('#cmbbank'); updateRow.BankName = comboText('#cmbbank'); updateRow.CheqDate = cheqDateValue();
                updateRow.CheqNo = $('#txtcheqno').val(); updateRow.CheqAmount = toDouble(amtText); updateRow.Comments = $('#txtComments').val();
                setAddMode(false);
            }
            bindFrm();
            resetDetail();
        }
    }
    function btnCancel_Click() { setAddMode(false); resetDetail(); }
    /* grdfrm_DoubleClick */
    function grdfrm_DoubleClick(r) {
        var row = rowOf(r); if (!row) return;
        updateRow = row;
        setCombo('#cmbbank', row.BankId);
        $('#txtcheqdate').val(String(row.CheqDate).substring(0, 10));
        $('#txtcheqno').val(row.CheqNo == null ? '' : String(row.CheqNo));
        $('#txtcheqamount').val(row.CheqAmount == null ? '' : String(row.CheqAmount));
        $('#txtComments').val(row.Comments == null ? '' : String(row.Comments));
        setAddMode(true);
        AccF.focus('#cmbbank');
    }
    /* grdfrm_ColumnButtonClick ("Delete") */
    function grdfrm_Delete(r) {
        if (!window.confirm('Are you sure to delete?')) return;
        var row = rowOf(r); if (!row) return;
        function drop() { table.splice(table.indexOf(row), 1); bindFrm(); }
        if (!$('#btnsave').prop('hidden') && !$('#btnsave').prop('disabled')) { drop(); return; }
        /* not in save mode: PdcCheqDeleteByPdcInventoryId (a row that was never saved has Id 0 and nothing to delete) */
        if (toInt(row.Id) === 0) { drop(); return; }
        call('POST', '/cheque/' + toInt(row.Id) + '/delete').then(drop).catch(function (e) { window.alert(e.message); });
    }

    /* ---------------------------------------------------------------- formvalidation / Insert (Save / Update) */
    function formValidation() {
        var doc = $('#txtdocno').val().trim();
        if (doc === '' || doc === '0') { say('Doc No field required'); $('#txtdocno').focus(); return false; }
        if (comboVal('#CmbAccountTitleCredit') === '') { say('Please Select Party Account'); AccF.focus('#CmbAccountTitleCredit'); return false; }
        if (comboVal('#CmbAccountTitleDebit') === '') { say('Please Select Debit Account'); AccF.focus('#CmbAccountTitleDebit'); return false; }
        return true;
    }
    function plainRow(row) { var d = {}; for (var k in row) if (k.indexOf('__') !== 0) d[k] = row[k]; return d; }
    /* a window opened inside the click, so the browser lets the report appear after the asynchronous call */
    function openBlank() { try { return window.open('', '_blank'); } catch (e) { return null; } }
    function showIn(w, url) { if (w && !w.closed) w.location.href = url; else window.open(url, '_blank'); }
    function dropWin(w) { try { if (w && !w.closed) w.close(); } catch (e) { /* ignore */ } }
    function insert(btn) {
        if (isBusy(btn)) return;
        if (!formValidation()) return;
        var updating = RecId > 0;
        if (!window.confirm(updating ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        if (table.length === 0) { say('grid record not found'); return; }
        var req = {
            id: RecId, docDate: ($('#txtdocdate').val() || nowParts().date) + ' ' + docTime, docNo: $('#txtdocno').val().trim(), remarks: $('#txtremarks').val(),
            creditId: comboVal('#CmbAccountTitleCredit'), debitId: comboVal('#CmbAccountTitleDebit'),
            rows: table.map(plainRow),
            keepAttachments: updating ? attSaved.map(function (a) { return a.Id; }) : [],
            addAttachments: attNew.map(function (a) { return { name: a.name, base64: a.base64 }; })
        };
        var w1 = $('#ChkPrint1').prop('checked') ? openBlank() : null, w2 = $('#ChkPrint2').prop('checked') ? openBlank() : null;
        busy(btn, true);
        call('POST', '/save', req).then(function (r) {
            busy(btn, false);
            say(updating ? 'Record Update Successfully' : 'Record Save Successfully');
            if ($('#ChkPrint1').prop('checked')) slipPrint(r.id, w1);
            if ($('#ChkPrint2').prop('checked')) voucher118(r.id, w2);
            return reset();
        }).catch(function (e) { busy(btn, false); dropWin(w1); dropWin(w2); window.alert(e.message); });
    }
    function btnsave_Click() { RecId = 0; insert('#btnsave'); }
    /* btnUpdate_Click: GetIdByDocNo must agree with RecId */
    function btnUpdate_Click() {
        if (isBusy('#btnUpdate')) return;
        busy('#btnUpdate', true);
        call('GET', '/lookup', { docNo: toInt($('#txtdocno').val().trim()) }).then(function (r) {
            busy('#btnUpdate', false);
            var id = toInt(r.id);
            if (id > 0) {
                if (RecId === id) { insert('#btnUpdate'); return; }
                throw new Error('RecId Not match against this DocNo');
            }
            throw new Error('Record Not Found For Update');
        }).catch(function (e) { busy('#btnUpdate', false); say(e.message); });
    }
    /* btnDelete_Click */
    function btnDelete_Click() {
        if (isBusy('#btnDelete')) return;
        if (!window.confirm('Are you sure to Delete?')) return;
        if (RecId <= 0) { window.alert('Record Not Found For Delete'); return; }
        busy('#btnDelete', true);
        call('POST', '/' + RecId + '/delete').then(function () {
            busy('#btnDelete', false);
            say('Delete Record Seccessfully');
            return reset();
        }).catch(function (e) { busy('#btnDelete', false); window.alert(e.message); });
    }
    function toolStripButton1_Click() {
        busy('#toolStripButton1', true);
        /* bankfill, AccountsFill, CreditAccountFill, CheqStatusFill (the status combo is invisible); the document number is not refilled */
        call('GET', '/load').then(function (r) { r.docNo = 0; applyLoad(r); }).catch(function (e) { say(e.message); }).then(function () { busy('#toolStripButton1', false); });
    }

    /* ---------------------------------------------------------------- ReadById / history */
    function readById(id) {
        RecId = toInt(id);
        return call('GET', '/' + RecId).then(function (r) {
            var h = r.header || {};
            selectTab(0);
            var m = /^(\d{4}-\d{2}-\d{2})[ T](\d{2}:\d{2}:\d{2})/.exec(String(h.DocDate == null ? '' : h.DocDate));
            if (m) { $('#txtdocdate').val(m[1]); docTime = m[2]; }
            setDocNo(h.DocNo == null ? '' : h.DocNo);
            setCombo('#CmbAccountTitleCredit', h.MGLAccountCrId); setCombo('#CmbAccountTitleDebit', h.MGLAccountDrId);
            $('#txtremarks').val(h.RemarkHeader == null ? '' : h.RemarkHeader);
            table = (r.details || []).map(function (d) {
                return { Id: toInt(d.Id), BankId: d.BankId, BankName: d.BankName, CheqDate: String(d.CheqDate == null ? '' : d.CheqDate).substring(0, 10), CheqNo: d.CheqNo,
                    CheqAmount: toDouble(d.CheqAmount), Comments: d.Remarks == null ? '' : String(d.Remarks), ChequeStatus: d.CheqStatus == null ? '' : String(d.CheqStatus), __uid: ++uidSeq };
            });
            VoucherHeadId = toInt(r.voucherHeadId);
            bindFrm();
            attSaved = (r.attachments || []).map(function (a) { return { Id: a.Id, name: a.Attachment, size: a.UploadedFileSizeMb, date: a.EntryDate }; });
            attNew = [];
            setSaveMode(false);
        }).catch(function (e) { say(e.message); });
    }
    function grdHistory_DoubleClick(row) { RecId = toInt(row.Id); readById(RecId); }

    /* txtdocno leave: GetIdByDocNo, then ReadById when the number exists (only after the number was edited) */
    function txtdocno_Leave() {
        var v = $('#txtdocno').val().trim();
        if (v === lastDocLookup) return;
        lastDocLookup = v;
        call('GET', '/lookup', { docNo: toInt(v) }).then(function (r) {
            RecId = toInt(r.id);
            if (RecId > 0) return readById(RecId);
        }).catch(function (e) { say(e.message); });
    }

    /* ---------------------------------------------------------------- Attachment form (AT.Show) / AttachmentView */
    function cardHtml(name, meta, extra, delAttr) {
        var ext = (String(name).split('.').pop() || '').toUpperCase().substring(0, 4);
        return '<div class="card" ' + extra + '><div class="pv">' + CJG.esc(ext) + '</div><div class="nm">' + CJG.esc(name) + '</div><div class="mt">' + CJG.esc(meta) + '</div>'
            + (delAttr ? '<button type="button" class="del" ' + delAttr + '>Delete</button>' : '') + '</div>';
    }
    function renderAtt() {
        var h = '';
        attSaved.forEach(function (a, i) { h += cardHtml(a.name, (a.size == null ? '' : a.size + ' MB') + ' ' + (a.date ? fmtDt(a.date) : ''), 'data-s="' + i + '"', 'data-s="' + i + '"'); });
        attNew.forEach(function (a, i) { h += cardHtml(a.name, sizeMb(a.size) + ' MB (new)', 'data-n="' + i + '"', 'data-n="' + i + '"'); });
        $('#attCards').html(h);
        $('#attNote').text(attSaved.length + attNew.length + ' file(s)');
    }
    function toolStripButton8_Click() { renderAtt(); $('#ovAtt').addClass('on'); }
    function renderView(rows) {
        var h = '', mb = 0;
        rows.forEach(function (a) {
            mb += toDouble(a.UploadedFileSizeMb);
            h += cardHtml(a.Attachment, (a.UploadedFileSizeMb == null ? '' : a.UploadedFileSizeMb + ' MB'), 'data-id="' + a.Id + '"', '');
        });
        $('#viewCards').html(h); $('#vFiles').text(rows.length); $('#vSize').text(Math.round(mb * 100) / 100);
    }
    var viewId = 0;
    function grdHistory_LinkClicked(col, row) {
        if (col !== 'NoOfAttachments') return;
        viewId = toInt(row.Id);
        call('GET', '/' + viewId + '/attachments').then(function (r) { renderView(r.rows || []); $('#ovView').addClass('on'); }).catch(function (e) { say(e.message); });
    }
    function pickFiles(files) {
        var list = Array.prototype.slice.call(files || []);
        if (attNew.length + list.length > 10) { say('Select at most ten files at once'); return; }
        list.forEach(function (f) {
            if (f.size === 0 || f.size > 5 * 1024 * 1024) { say(f.name + ': Attachment must be between 1 byte and 5 MB'); return; }
            var fr = new FileReader();
            fr.onload = function () {
                var s = String(fr.result), i = s.indexOf(',');
                attNew.push({ name: f.name, size: f.size, base64: i >= 0 ? s.substring(i + 1) : '' });
                renderAtt();
            };
            fr.readAsDataURL(f);
        });
    }

    /* ---------------------------------------------------------------- prints / report block */
    /* count check ("Record Not Found For Display"), then the PDF */
    function printKind(kind, params, btn, w) {
        if (btn && isBusy(btn)) { dropWin(w); return; }
        if (btn) busy(btn, true);
        return call('GET', '/print/' + kind + '/count', params).then(function (r) {
            if (!r.count) { dropWin(w); say('Record Not Found For Display'); return; }
            showIn(w, API + '/print/' + kind + '?' + $.param(params));
        }).catch(function (e) { dropWin(w); say(e.message); }).then(function () { if (btn) busy(btn, false); });
    }
    /* SlipPrint(PrintId) */
    function slipPrint(id, w) { return printKind('slip', { id: id }, null, w || openBlank()); }
    /* CommonServices.VoucherReport_118(VoucherHeadIdGet(id, 16)) */
    function voucher118(id, w) {
        return call('GET', '/' + id + '/voucher').then(function (r) {
            var vid = toInt(r.voucherHeadId);
            if (vid === 0) { dropWin(w); say('VoucherId Not Found'); return; }
            showIn(w, '/reports/print/by-key/acc-118?id=' + vid);
        }).catch(function (e) { dropWin(w); say(e.message); });
    }
    function toolStripButton7_Click() {
        var w = openBlank();
        if (VoucherHeadId === 0) { dropWin(w); say('VoucherId Not Found'); return; }
        showIn(w, '/reports/print/by-key/acc-118?id=' + VoucherHeadId);
    }
    function btnPrint_Click() { slipPrint(RecId); }
    function dateOrToday(sel) { return $(sel).val() || nowParts().date; }
    function filterFromControls() {
        var f = { accountId: comboVal('#CmbAccountTitle'), debitId: comboVal('#cmbDebitAccountrpt'), status: comboText('#CmbChequeStatusReprot').trim() };
        if ($('#ChkFromdate').prop('checked')) f.fromDate = dateOrToday('#datFromDate');
        if ($('#ChkTodate').prop('checked')) f.toDate = dateOrToday('#datToDate');
        if ($('#ChkCheqFromdate').prop('checked')) f.cheqFrom = dateOrToday('#datCheqFromDate');
        if ($('#ChkCheqToDate').prop('checked')) f.cheqTo = dateOrToday('#datCheqToDate');
        return f;
    }
    /* btnshow_Click */
    function btnShow_Click() {
        if (isBusy('#btnShow')) return;
        var f = filterFromControls();
        busy('#btnShow', true);
        call('POST', '/report', f).then(function (r) {
            var rows = r.rows || [];
            lastFilter = f; reportCount = rows.length;
            if (rows.length > 0) reportGrid(rows); else grdReprot.setRows([]);
        }).catch(function (e) { say(e.message); }).then(function () { busy('#btnShow', false); });
    }
    /* btnRegister_Click / btnPdcRegisterGuriList_Click: the table of the last Show */
    function registerPrint(kind, btn) {
        if (!lastFilter || reportCount === 0) { say('Record Not Found For Display'); return; }
        printKind(kind, lastFilter, btn, openBlank());
    }
    /* btnPdcSummery_Click: fresh filters, no debit account */
    function btnPdcSummery_Click() {
        var f = filterFromControls(); delete f.debitId;
        printKind('summary', f, '#btnPdcSummery', openBlank());
    }

    /* ---------------------------------------------------------------- keys */
    function enterTab(e) {
        var t = e.target;
        if (!t || t.tagName !== 'INPUT' || t.type === 'checkbox' || t.type === 'file') return false;
        var els = $('#pageForm').find('input:visible:not(:disabled):not([type=checkbox]):not([type=file]), button:visible:not(:disabled)').toArray();
        var i = els.indexOf(t);
        if (i >= 0 && i < els.length - 1) { e.preventDefault(); els[i + 1].focus(); return true; }
        return false;
    }
    function digitsOnly(e) {
        if (e.ctrlKey || e.metaKey || e.altKey || e.key.length > 1) return;
        if (!/\d/.test(e.key)) e.preventDefault();
    }

    /* ---------------------------------------------------------------- wiring */
    $(function () {
        var n = nowParts();
        $('#txtdocdate, #txtcheqdate, #datFromDate, #datToDate, #datCheqFromDate, #datCheqToDate').val(n.date); docTime = n.time;
        $('#tabF').on('click', function () { selectTab(0); });
        $('#tabH').on('click', function () { selectTab(1); });
        $('#btnnew').on('click', btnnew_Click);
        $('#btnsave').on('click', btnsave_Click);
        $('#btnUpdate').on('click', btnUpdate_Click);
        $('#btnDelete').on('click', btnDelete_Click);
        $('#btnPrint').on('click', btnPrint_Click);
        $('#toolStripButton7').on('click', toolStripButton7_Click);
        $('#toolStripButton8').on('click', toolStripButton8_Click);
        $('#toolStripButton1').on('click', toolStripButton1_Click);
        $('#btnRegister').on('click', function () { registerPrint('register', '#btnRegister'); });
        $('#btnPdcRegisterGuriList').on('click', function () { registerPrint('guri', '#btnPdcRegisterGuriList'); });
        $('#btnPdcSummery').on('click', btnPdcSummery_Click);
        $('#btnShow').on('click', btnShow_Click);
        $('#toolStripButton10').on('click', function () { historyFill(0); });
        $('#btnAdd').on('click', btnAdd_Click);
        $('#btnCancel').on('click', btnCancel_Click);
        $('#button1').on('click', function () { window.open('/master-data/pdc-bank', '_blank'); });     // new PdcBank(UserAccount).Show()
        $('#txtdocno').on('keydown', digitsOnly).on('blur', txtdocno_Leave);
        $('#txtcheqamount').on('keydown', digitsOnly);
        $('#attChoose').on('click', function () { $('#attFile').val('').trigger('click'); });
        $('#attFile').on('change', function () { pickFiles(this.files); });
        $('#attClose').on('click', function () { $('#ovAtt').removeClass('on'); });
        $('#viewClose').on('click', function () { $('#ovView').removeClass('on'); });
        $('#attCards').on('click', '.del', function (e) {
            e.stopPropagation();
            var s = $(this).data('s'), nn = $(this).data('n');
            if (s != null) attSaved.splice(+s, 1); else if (nn != null) attNew.splice(+nn, 1);
            renderAtt();
        }).on('click', '.card', function () {
            var s = $(this).data('s');
            if (s != null && RecId > 0 && attSaved[+s]) window.open(API + '/' + RecId + '/attachments/' + attSaved[+s].Id, '_blank');
        });
        $('#viewCards').on('click', '.card', function () { window.open(API + '/' + viewId + '/attachments/' + $(this).data('id'), '_blank'); });
        /* AcfrmDefPdcManagment_KeyDown */
        $(document).on('keydown', function (e) {
            if ($('.ov.on').length) return;
            if (e.key === 'Enter' && !e.ctrlKey && !e.altKey) { if (enterTab(e)) return; }
            if (e.ctrlKey && !e.altKey) {
                var k = e.key.toLowerCase();
                if (k === 's' && !$('#btnsave').prop('hidden') && !$('#btnsave').prop('disabled')) { e.preventDefault(); $('#btnsave').click(); }
                else if (k === 'n') { e.preventDefault(); btnnew_Click(); }
                else if (k === 'u' && !$('#btnUpdate').prop('hidden') && !$('#btnUpdate').prop('disabled')) { e.preventDefault(); $('#btnUpdate').click(); }
                else if (k === 'e') { e.preventDefault(); if (window.history.length > 1) window.history.back(); }
            } else if (e.key === 'Escape' && window.history.length > 1) { window.history.back(); }
        });
        /* AcfrmDefPdcManagment_Load */
        statusFill();
        setSaveMode(true); setAddMode(false);
        bindFrm();
        call('GET', '/load').then(function (r) { applyLoad(r); $('#txtdocdate').focus(); }).catch(function (e) { say(e.message); });
    });
})(window.jQuery);
