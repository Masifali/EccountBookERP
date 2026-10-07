/* Incentive Policy - Architecture.WinApp.WholeSale.frmIncentivePolicy (screen 7). Event-for-event port of frmIncentivePolicy.cs. */
(function ($) {
    'use strict';
    var API = '/accounts/incentive-policy';
    var RecId = 0, updateDetailIndex = -1;
    var dtdetail = [];                          // the DataTable behind grdDetails
    var timeFrom = '00:00:00', timeTo = '00:00:00';   // DateTimePicker.Value keeps the time of day

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
            if (method === 'POST') { o.contentType = 'application/json'; o.data = JSON.stringify(data); } else o.data = data || {};
            $.ajax(o).done(function (r) { if (r && r.success === false) reject(new Error(r.message || 'Error')); else resolve(r); })
                .fail(function (x) { reject(new Error(failText(x))); });
        });
    }
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function nowParts() { var d = new Date(); return { date: d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()), time: pad(d.getHours()) + ':' + pad(d.getMinutes()) + ':' + pad(d.getSeconds()) }; }
    /* Conversion.ToDouble(string) */
    function toDouble(s) { var t = String(s == null ? '' : s).trim(); if (t === '') return 0; var n = Number(t); return isFinite(n) ? n : 0; }
    /* Conversion.ToInt(string) = Convert.ToInt32(string): digits only, otherwise the exception becomes 0 */
    function toInt(s) { var t = String(s == null ? '' : s).trim(); if (!/^[+-]?\d+$/.test(t)) return 0; var n = parseInt(t, 10); return (n > 2147483647 || n < -2147483648) ? 0 : n; }

    /* ---------------------------------------------------------------- combos */
    function fill(sel, rows, keyV, keyT) { if (rows && rows.length > 0) AccF.fillSelect(sel, rows.map(function (r) { return { V: CJG.pick(r, keyV), T: CJG.pick(r, keyT) }; }), 'V', 'T', ''); }
    function comboVal(sel) { var v = $(sel).val(); return v == null ? '' : String(v); }
    function comboText(sel) { return comboVal(sel) === '' ? '' : $(sel + ' option:selected').text(); }
    function setCombo(sel, v) {
        var s = v == null ? '' : String(v);
        if (!$(sel).find('option').filter(function () { return this.value === s; }).length) s = '';
        $(sel).val(s).trigger('change');
    }
    function clearCombo(sel) { $(sel).val('').trigger('change'); }
    function applyCombos(r) {
        fill('#cmbAdvoiceDuration', r.advoiceDuration, 'Id', 'LookUpName');
        fill('#cmbTargetBaseOn', r.targetBaseOn, 'Id', 'LookUpName');
        fill('#cmbItemName', r.items, 'Id', 'ItemName');
        fill('#cmbIncentiveType', r.incentiveType, 'Id', 'LookUpName');
        fill('#cmbIncentiveCalcOn', r.incentiveCalcOn, 'Id', 'LookUpName');
    }

    /* ---------------------------------------------------------------- grids */
    var grdDetails = CJG.create({
        table: '#grdDetails', nav: '#navDetails', autoResize: true,
        columns: [{ key: 'TargetBaseTypeId', hidden: true }, { key: 'TargetBaseType', caption: 'TargetBaseType', width: 140 },
                  { key: 'ItemId', hidden: true }, { key: 'ItemName', caption: 'ItemName', width: 260 },
                  { key: 'IncentiveTypeId', hidden: true }, { key: 'IncentiveType', caption: 'IncentiveType', width: 120 },
                  { key: 'TargetRangeFrom', caption: 'TargetRangeFrom', width: 100, num: true }, { key: 'TargetRangeTo', caption: 'TargetRangeTo', width: 100, num: true },
                  { key: 'IncentiveRate', caption: 'IncentiveRate', width: 100, num: true }, { key: 'IncentiveCalcOn', caption: 'IncentiveCalcOn', width: 120 },
                  { key: 'RemarksDetail', caption: 'RemarksDetail', width: 250 },
                  { key: 'Delete', caption: 'X', button: 'X', width: 20 }],
        onButton: function (col, row) { if (col === 'Delete') grdDetails_Delete(row); },
        onDblClick: function (row) { grdDetails_DoubleClick(row); }
    });
    function bindDetails() { grdDetails.setRows(dtdetail); }
    var grdHistory = CJG.create({
        table: '#DataGridHistory', nav: '#navHistory', autoResize: true,
        columns: [{ key: 'Id', hidden: true }, { key: 'PolicyDateFrom', caption: 'PolicyDateFrom', width: 110 }, { key: 'PolicyDateTo', caption: 'PolicyDateTo', width: 110 },
                  { key: 'PolicyDescription', caption: 'PolicyDescription', width: 300 }, { key: 'CrAdvoiceDuration', caption: 'CrAdvoiceDuration', width: 150 },
                  { key: 'Remarks', caption: 'Remarks', width: 300 },
                  { key: 'Edit', caption: 'Edit', button: 'Edit', width: 50 }, { key: 'Detail', caption: 'Detail', button: 'Detail', width: 50 }],
        onButton: function (col, row, i, btn) { DataGridHistory_ColumnButtonClick(col, row, btn); },
        onDblClick: function (row) { DataGridHistory_DoubleClick(row); }
    });
    var grdHistoryDetail = CJG.create({
        table: '#grdHistoryDetail', nav: '#navHistoryDetail', autoResize: true,
        columns: [{ key: 'TargetBaseType', caption: 'TargetBaseType', width: 140 }, { key: 'ItemName', caption: 'ItemName', width: 260 }, { key: 'IncentiveType', caption: 'IncentiveType', width: 120 },
                  { key: 'TargetRangeFrom', caption: 'TargetRangeFrom', width: 100, num: true }, { key: 'TargetRangeTo', caption: 'TargetRangeTo', width: 100, num: true },
                  { key: 'IncentiveRate', caption: 'IncentiveRate', width: 100, num: true }, { key: 'IncentiveCalcOn', caption: 'IncentiveCalcOn', width: 120 },
                  { key: 'RemarksDetail', caption: 'RemarksDetail', width: 250 }]
    });

    /* grdDetails_ColumnButtonClick ("Delete") */
    function grdDetails_Delete(row) {
        var i = dtdetail.indexOf(row);
        if (i >= 0) dtdetail.splice(i, 1);
        bindDetails();
    }

    /* ---------------------------------------------------------------- tabs */
    function selectTab(i) {
        $('#pageForm').toggleClass('on', i === 0); $('#pageHistory').toggleClass('on', i === 1);
        $('#tabF').toggleClass('on', i === 0); $('#tabH').toggleClass('on', i === 1);
        /* tabControl1_SelectedIndexChanged: SelectedIndex == 1 -> HistoryFill() */
        if (i === 1) historyFill();
        else { grdDetails.rerender(); }
    }
    function tabIndex() { return $('#pageHistory').hasClass('on') ? 1 : 0; }
    function historyFill() {
        return call('GET', '/history').then(function (r) {
            if (r.rows && r.rows.length > 0) grdHistory.setRows(r.rows);        // else the grid is left as it is
        }).catch(function (e) { say(e.message); });
    }

    /* ---------------------------------------------------------------- Reset / ResetDetail / save-update mode */
    function setSaveMode(isNew) { $('#btnsave').prop('hidden', !isNew); $('#btnUpdate').prop('hidden', isNew); }
    function reset() {
        var n = nowParts();
        $('#txtPolicyFrom').val(n.date); $('#txtPolicyTo').val(n.date); timeFrom = timeTo = n.time;
        $('#txtPolicyDescription').val(''); clearCombo('#cmbAdvoiceDuration'); $('#txtRemarks').val('');
        RecId = 0; dtdetail = []; bindDetails();
        setSaveMode(true);
        $('#txtPolicyFrom').focus();
    }
    function resetDetail() {
        clearCombo('#cmbTargetBaseOn'); clearCombo('#cmbItemName'); $('#txtRangeFrom').val(''); $('#txtRangeTo').val('');
        clearCombo('#cmbIncentiveType'); $('#txtIncentiveRate').val(''); clearCombo('#cmbIncentiveCalcOn'); $('#txtDetailRemarks').val('');
    }
    function btnnew_Click() { reset(); resetDetail(); }

    /* ---------------------------------------------------------------- Save / Update */
    function formValidation() {
        if ($('#txtPolicyDescription').val() === '') { say('Policy Description Required'); $('#txtPolicyDescription').focus(); return false; }
        if (comboVal('#cmbAdvoiceDuration') === '') { say('Cr Advoice Duration Required'); AccF.focus('#cmbAdvoiceDuration'); return false; }
        return true;
    }
    function btnsave_Click() {
        if (isBusy('#btnsave') || isBusy('#btnUpdate')) return Promise.resolve();
        if (!window.confirm(RecId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return Promise.resolve();   // MessageBox YesNo
        if (!formValidation()) return Promise.resolve();
        if (dtdetail.length <= 0) { say('Grid record not found'); return Promise.resolve(); }
        busy('#btnsave', true); busy('#btnUpdate', true);
        var body = {
            id: RecId, dateFrom: $('#txtPolicyFrom').val() + ' ' + timeFrom, dateTo: $('#txtPolicyTo').val() + ' ' + timeTo,
            description: $('#txtPolicyDescription').val(), crAdvoiceTerms: comboVal('#cmbAdvoiceDuration'), remarks: $('#txtRemarks').val(),
            detail: dtdetail.map(function (r) {
                return { TargetBaseTypeId: r.TargetBaseTypeId, ItemId: r.ItemId, IncentiveTypeId: r.IncentiveTypeId, TargetRangeFrom: r.TargetRangeFrom,
                         TargetRangeTo: r.TargetRangeTo, IncentiveRate: r.IncentiveRate, IncentiveCalcOn: r.IncentiveCalcOn, RemarksDetail: r.RemarksDetail == null ? '' : String(r.RemarksDetail) };
            })
        };
        var wasNew = RecId === 0;
        return call('POST', '/save', body).then(function () {
            say(wasNew ? 'Receod Save Successfully' : 'Receod Update Successfully');
            reset();
        }).catch(function (e) { say(e.message); })
          .then(function () { busy('#btnsave', false); busy('#btnUpdate', false); });
    }

    /* ---------------------------------------------------------------- ReadById */
    function dateOf(s) { return s ? String(s).substring(0, 10) : ''; }
    function timeOf(s) { return s && String(s).length >= 19 ? String(s).substring(11, 19) : '00:00:00'; }
    function readById(id) {
        RecId = id;
        return call('GET', '/' + id).then(function (r) {
            var h = r.header;
            if (!h) return;
            setSaveMode(false);
            selectTab(0);
            $('#txtPolicyFrom').val(dateOf(h.SchemeDateFrom)); timeFrom = timeOf(h.SchemeDateFrom);
            $('#txtPolicyTo').val(dateOf(h.SchemeDateTo)); timeTo = timeOf(h.SchemeDateTo);
            $('#txtPolicyDescription').val(h.SchemeDescription || '');
            setCombo('#cmbAdvoiceDuration', h.CrAdvoiceTerms);
            $('#txtRemarks').val(h.RemarksHeader || '');
            dtdetail = (r.details || []).map(function (d) {
                return { TargetBaseTypeId: d.TargetBaseTypeId, TargetBaseType: d.TargetBaseType, ItemId: d.ItemId, ItemName: d.ItemName,
                         IncentiveTypeId: d.IncentiveTypeId, IncentiveType: d.IncentiveType, TargetRangeFrom: d.TargetRangeFrom, TargetRangeTo: d.TargetRangeTo,
                         IncentiveRate: d.IncentiveRate, IncentiveCalcOn: d.IncentiveCalcTypeId, RemarksDetail: d.RemarksDetail };
            });
            bindDetails();
        }).catch(function (e) { say(e.message); });
    }
    function DataGridHistory_DoubleClick(row) {
        if (!row) { say('Object reference not set to an instance of an object.'); return; }
        RecId = parseInt(row.Id, 10) || 0; readById(RecId);
    }
    function DataGridHistory_ColumnButtonClick(col, row, btn) {
        if (!row) return;
        var id = parseInt(row.Id, 10) || 0;
        if (col === 'Edit') { RecId = id; readById(id); }
        if (col === 'Detail') {
            busy(btn, true);
            call('GET', '/' + id).then(function (r) {
                if (!r.header) return;
                grdHistoryDetail.setRows((r.details || []).map(function (d) {
                    return { TargetBaseType: d.TargetBaseType, ItemName: d.ItemName, IncentiveType: d.IncentiveType, TargetRangeFrom: d.TargetRangeFrom,
                             TargetRangeTo: d.TargetRangeTo, IncentiveRate: d.IncentiveRate, IncentiveCalcOn: d.IncentiveCalcTypeId, RemarksDetail: d.RemarksDetail };
                }));
            }).catch(function (e) { say(e.message); }).then(function () { busy(btn, false); });
        }
    }

    /* ---------------------------------------------------------------- detail entry */
    function detailValidation() {
        if (comboVal('#cmbTargetBaseOn') === '') { say('Target Base On Required'); AccF.focus('#cmbTargetBaseOn'); return false; }
        if (comboVal('#cmbItemName') === '') { say('Item Required'); AccF.focus('#cmbItemName'); return false; }
        if (toDouble($('#txtRangeFrom').val()) === 0) { say('Range From Required'); $('#txtRangeFrom').focus(); return false; }
        if (toDouble($('#txtRangeTo').val()) === 0) { say('Range To Required'); $('#txtRangeTo').focus(); return false; }
        if (comboVal('#cmbIncentiveType') === '') { say('Incentive Type Required'); AccF.focus('#cmbIncentiveType'); return false; }
        if (toDouble($('#txtIncentiveRate').val()) === 0) { say('Incentive Rate Required'); $('#txtIncentiveRate').focus(); return false; }
        if (comboVal('#cmbIncentiveCalcOn') === '') { say('Incentive Calc On Required'); AccF.focus('#cmbIncentiveCalcOn'); return false; }
        return true;
    }
    /* btnplus_Click -> AddtoGrid(): note the desktop stores the combo TEXT of Incentive Calc On and ToInt() of the ranges */
    function addToGrid() {
        if (!detailValidation()) return;
        dtdetail.push({
            TargetBaseTypeId: comboVal('#cmbTargetBaseOn'), TargetBaseType: comboText('#cmbTargetBaseOn'),
            ItemId: comboVal('#cmbItemName'), ItemName: comboText('#cmbItemName'),
            IncentiveTypeId: comboVal('#cmbIncentiveType'), IncentiveType: comboText('#cmbIncentiveType'),
            TargetRangeFrom: toInt($('#txtRangeFrom').val()), TargetRangeTo: toInt($('#txtRangeTo').val()),
            IncentiveRate: toDouble($('#txtIncentiveRate').val()), IncentiveCalcOn: comboText('#cmbIncentiveCalcOn'),
            RemarksDetail: $('#txtDetailRemarks').val()
        });
        bindDetails();
        resetDetail();
        AccF.focus('#cmbTargetBaseOn');
    }
    function setDetailButtons(editing) {
        $('#btnplus').prop('hidden', editing); $('#btnUpdateDetail').prop('hidden', !editing); $('#btnCancelUpdateDetial').prop('hidden', !editing);
    }
    /* grdDetails_DoubleClick */
    function grdDetails_DoubleClick(row) {
        if (!row) return;
        updateDetailIndex = dtdetail.indexOf(row);
        setCombo('#cmbTargetBaseOn', row.TargetBaseTypeId);
        setCombo('#cmbItemName', row.ItemId);
        $('#txtRangeFrom').val(row.TargetRangeFrom == null ? '' : String(row.TargetRangeFrom));
        $('#txtRangeTo').val(row.TargetRangeTo == null ? '' : String(row.TargetRangeTo));
        setCombo('#cmbIncentiveType', row.IncentiveTypeId);
        $('#txtIncentiveRate').val(row.IncentiveRate == null ? '' : String(row.IncentiveRate));
        setCombo('#cmbIncentiveCalcOn', row.IncentiveCalcOn);
        $('#txtDetailRemarks').val(row.RemarksDetail == null ? '' : String(row.RemarksDetail));
        setDetailButtons(true);
    }
    /* btnUpdateDetail_Click */
    function updateDetail() {
        if (!detailValidation()) return;
        var r = dtdetail[updateDetailIndex];
        if (!r) { say('There is no row at position ' + updateDetailIndex + '.'); return; }
        r.TargetBaseTypeId = comboVal('#cmbTargetBaseOn'); r.TargetBaseType = comboText('#cmbTargetBaseOn');
        r.ItemId = comboVal('#cmbItemName'); r.ItemName = comboText('#cmbItemName');
        r.IncentiveTypeId = comboVal('#cmbIncentiveType'); r.IncentiveType = comboText('#cmbIncentiveType');
        /* the double columns take the typed text as is (DataTable parses it) */
        var cols = [['TargetRangeFrom', '#txtRangeFrom'], ['TargetRangeTo', '#txtRangeTo'], ['IncentiveRate', '#txtIncentiveRate']];
        for (var k = 0; k < cols.length; k++) {
            var t = $(cols[k][1]).val(), n = Number(String(t).trim());
            if (String(t).trim() === '' || !isFinite(n)) { say('Couldn\'t store <' + t + '> in ' + cols[k][0] + ' Column.  Expected type is Double.'); return; }
            r[cols[k][0]] = n;
        }
        r.IncentiveCalcOn = comboVal('#cmbIncentiveCalcOn');
        r.RemarksDetail = $('#txtDetailRemarks').val();
        bindDetails();
        setDetailButtons(false);
        resetDetail();
        AccF.focus('#cmbTargetBaseOn');
    }
    /* btnCancelUpdateDetial_Click */
    function cancelUpdateDetail() { setDetailButtons(false); resetDetail(); }

    function closeForm() { window.location.href = '/accounts/dashboard'; }
    function comboOpen() { return $('.dtcombo-pop').filter(function () { return this.style.display === 'block'; }).length > 0; }

    /* ---------------------------------------------------------------- wiring */
    $('#btnnew').on('click', btnnew_Click);
    $('#btnsave, #btnUpdate').on('click', btnsave_Click);          // btnUpdate_Click -> btnsave_Click
    $('#btnplus').on('click', addToGrid);
    $('#btnUpdateDetail').on('click', updateDetail);
    $('#btnCancelUpdateDetial').on('click', cancelUpdateDetail);
    $('#tabsBar').on('click', '.tab', function () { selectTab(parseInt($(this).data('i'), 10)); });

    /* FrmExportSalesContract_KeyDown (KeyPreview) */
    $(document).on('keydown', function (e) {
        var k = e.key, K = (k || '').toUpperCase(), ctl = e.ctrlKey && !e.altKey;
        if (k === 'Enter' && !e.ctrlKey && !e.altKey && !e.shiftKey && !comboOpen() && !$(e.target).is('button, a, tr.flt input, textarea')) {
            /* SendKeys.Send("{TAB}") */
            var f = $('#pages .page.on').find('input:visible:not([readonly]):not([type=checkbox]), .dtcombo-input:visible, button:visible:enabled').toArray();
            var i = f.indexOf(e.target); if (i >= 0 && i < f.length - 1) { e.preventDefault(); f[i + 1].focus(); }
        }
        if (ctl && K === 'N' && tabIndex() === 0) { e.preventDefault(); btnnew_Click(); }
        if (ctl && K === 'S' && !$('#btnsave').prop('hidden') && !$('#btnsave').prop('disabled')) { e.preventDefault(); btnsave_Click(); }
        if (ctl && K === 'U' && !$('#btnUpdate').prop('hidden') && !$('#btnUpdate').prop('disabled')) { e.preventDefault(); btnsave_Click(); }
        if (ctl && K === 'T') { e.preventDefault(); selectTab(tabIndex() === 1 ? 0 : 1); }
        if ((ctl && K === 'E') || (k === 'Escape' && !comboOpen())) { e.preventDefault(); closeForm(); }
    });

    /* FrmExportSalesContract_Load */
    $(function () {
        var n = nowParts();
        $('#txtPolicyFrom').val(n.date); $('#txtPolicyTo').val(n.date); timeFrom = timeTo = n.time;
        bindDetails();                                   // grdDetails.DataSource = dtdetail; RetrieveStructure(); grdSettings()
        setSaveMode(true);
        call('GET', '/load').then(function (r) { applyCombos(r); }).catch(function (e) { say(e.message); });
        $('#txtPolicyFrom').focus();
    });
})(window.jQuery);
