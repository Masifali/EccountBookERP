/**
 * Payables And Payment Schedule JS Module (ERP 1003 Payables & PaymentSchedule)
 * Matches exact desktop WinForms theme, button disability management, Select2 dropdowns,
 * summary grid rendering, parent group rendering with subtotals & clickable voucher popups.
 */

$(document).ready(function() {
    initDefaults();
    initSelect2();
    loadLookups();
    loadReport();
});

function initDefaults() {
    const today = ReportLoading.localDate();
    const firstDayMonth = ReportLoading.localDate(new Date(new Date().getFullYear(), new Date().getMonth(), 1));
    
    $('#txtDateFrom').val(firstDayMonth);
    $('#txtDateTo').val(today);
    $('#txtDueFrom').val(today);
    $('#txtDueUpTo').val(today);
    $('#txtPurchaseFrom').val(firstDayMonth);
    $('#txtPurchaseTo').val(today);
    $('#txtIntervalDays').val('30');
    $('#txtAgingDays').val('30');

    // Sync Checkbox state with input disability
    $('#chkFromDate').on('change', function() { $('#txtDateFrom').prop('disabled', !this.checked); });
    $('#chkToDate').on('change', function() { $('#txtDateTo').prop('disabled', !this.checked); });
    $('#chkDueFrom').on('change', function() { $('#txtDueFrom').prop('disabled', !this.checked); });
    $('#chkDueUpTo').on('change', function() { $('#txtDueUpTo').prop('disabled', !this.checked); });
    $('#chkPurchaseFrom').on('change', function() { $('#txtPurchaseFrom').prop('disabled', !this.checked); });
    $('#chkPurchaseTo').on('change', function() { $('#txtPurchaseTo').prop('disabled', !this.checked); });
}

function initSelect2() {
    $('#cmbCustomGroup, #cmbCustomerGroup').select2({
        width: '100%',
        dropdownAutoWidth: true
    });

    $('#cmbControlAccount').select2({
        width: '100%',
        placeholder: "Account Title",
        allowClear: true,
        closeOnSelect: false,
        templateResult: function(data) {
            if (!data.id) return data.text;
            let selectedVals = $('#cmbControlAccount').val() || [];
            let isChecked = Array.isArray(selectedVals) ? selectedVals.includes(data.id.toString()) : (selectedVals == data.id);
            return $(`<span><input type="checkbox" ${isChecked ? 'checked' : ''} style="margin-right: 6px; vertical-align: middle;"> ${data.text}</span>`);
        },
        templateSelection: function(data) {
            if (!data.id) return data.text;
            let selectedVals = $('#cmbControlAccount').val() || [];
            if (!Array.isArray(selectedVals) || selectedVals.length <= 1) return data.text;
            return `${selectedVals.length} Accounts Selected`;
        }
    });
}

function loadLookups() {
    $.ajax({
        url: '/api/accounts/payables-payment-schedule/lookups',
        type: 'GET',
        dataType: 'json',
        success: function(res) {
            if (!res) return;

            // Populate Control Accounts (Level 3) with checkbox multi-select UI
            let $ctrl = $('#cmbControlAccount').empty();
            if (res.controls) {
                res.controls.forEach(item => {
                    let id = item.id || item.Id || item.AccountCode || item.accountCode;
                    let title = item.AccountTitle || item.accountTitle || item.AccountsTypeTitle || item.AccountsTypeDesc || '';
                    if (id && title) {
                        $ctrl.append(new Option(title, id));
                    }
                });
            }

            // Populate Custom Groups (Default: Custom Group)
            let $cg = $('#cmbCustomGroup').empty().append('<option value="0">Custom Group</option>');
            if (res.customGroups) {
                res.customGroups.forEach(item => {
                    let id = item.Id || item.id;
                    let title = item.AcLookUpsDescription || item.acLookUpsDescription || item.AcLookUpName || item.groupName || item.GroupName || item.name || item.Description || item.description;
                    if (id && title) $cg.append(new Option(title, id));
                });
            }

            // Populate Customer Groups (Default: Customer Group)
            let $custG = $('#cmbCustomerGroup').empty().append('<option value="0">Customer Group</option>');
            if (res.customerGroups) {
                res.customerGroups.forEach(item => {
                    let id = item.Id || item.id;
                    let name = item.Description || item.description || item.GroupName || item.groupName || item.name;
                    if (id && name) $custG.append(new Option(name, id));
                });
            }
        },
        error: function(err) {
            console.error('Error loading lookups:', err);
        }
    });
}

function setLoading(isLoading) {
    const $btn = $('#btnShow');
    if (isLoading) {
        $btn.prop('disabled', true).addClass('disabled').html('<i class="fa fa-spinner fa-spin"></i> Loading...');
    } else {
        $btn.prop('disabled', false).removeClass('disabled').html('Show');
    }
}

function loadReport() {
    if (loadReport.pending) return;
    loadReport.pending = true;
    setLoading(true);

    let ctrlVal = $('#cmbControlAccount').val();
    let controlAccountId = Array.isArray(ctrlVal) ? ctrlVal.join(',') : (ctrlVal || '0');

    const params = {
        fromDate: $('#chkFromDate').is(':checked') ? $('#txtDateFrom').val() : '',
        toDate: $('#chkToDate').is(':checked') ? $('#txtDateTo').val() : '',
        dueFrom: $('#chkDueFrom').is(':checked') ? $('#txtDueFrom').val() : '',
        dueUpTo: $('#chkDueUpTo').is(':checked') ? $('#txtDueUpTo').val() : '',
        purchaseFrom: $('#chkPurchaseFrom').is(':checked') ? $('#txtPurchaseFrom').val() : '',
        purchaseTo: $('#chkPurchaseTo').is(':checked') ? $('#txtPurchaseTo').val() : '',
        intervalDays: $('#txtIntervalDays').val() || 30,
        agingDays: $('#txtAgingDays').val() || 30,
        controlAccountId: controlAccountId,
        customGroupId: $('#cmbCustomGroup').val() || 0,
        customerGroupId: $('#cmbCustomerGroup').val() || 0
    };

    $.ajax({
        url: '/api/accounts/payables-payment-schedule',
        type: 'GET',
        data: params,
        dataType: 'json',
        success: function(res) {
            if (!res) {
                renderEmptyTable();
                renderSummaryMatrix([]);
                return;
            }

            window.cachedVouchers = res.vouchers || [];
            renderSummaryMatrix(res.summaryMatrix || []);
            renderGridData(res.data || []);
        },
        error: function(xhr, status, error) {
            console.error('Failed to fetch Payables Payment Schedule:', error);
            renderEmptyTable();
            document.querySelector('#mainTbody td').textContent = 'Unable to load the payment schedule (' + xhr.status + '). Please retry.';
        },
        complete: function() {
            loadReport.pending=false;setLoading(false);
        }
    });
}

function renderSummaryMatrix(matrix) {
    const table=document.getElementById('summaryTbody').closest('table');
    const first=matrix?.[0]||{};
    const headings=['Description','Amount',first.IstIntervale||'Interval 1',first.ScnInterval||'Interval 2',first.TrdIntarval||'Interval 3',first.Above||'Above'];
    table.tHead.replaceChildren();const head=table.tHead.insertRow();
    headings.forEach(title=>{const th=document.createElement('th');th.textContent=title;head.append(th);});
    const body=table.tBodies[0];body.replaceChildren();
    (matrix||[]).forEach(row=>{const tr=body.insertRow();['Description','Amount','Value_1','Value_2','Value_3','Value_4'].forEach((key,index)=>{const td=tr.insertCell();td.textContent=index?formatNum(row[key]||0):row[key]||'';if(index)td.style.textAlign='right';});});
}
function renderGridData(rows) {
    const $tbody = $('#mainTbody').empty();

    if (!rows || rows.length === 0) {
        renderEmptyTable();
        return;
    }

    // Group rows by ParentAccount
    const groups = {};
    rows.forEach(r => {
        const parent = r.ParentAccount || 'TRADE SUPPLIERS';
        if (!groups[parent]) groups[parent] = [];
        groups[parent].push(r);
    });

    let grandOp = 0, grandDr = 0, grandCr = 0, grandCl = 0, grandDue = 0, grandNotYet = 0, grandPayToday = 0, grandShort = 0, grandLastAmt = 0;

    Object.keys(groups).forEach(parentName => {
        // Render Group Header
        $tbody.append(`
            <tr class="group-header-row">
                <td colspan="14" style="background-color: #e2e8f0; font-weight: bold; color: #0f172a; padding: 4px 8px;">
                    <i class="fa fa-folder-open-o" style="color:#00796B;"></i> Parent Account Title: ${escapeHtml(parentName)}
                </td>
            </tr>
        `);

        let grpOp = 0, grpDr = 0, grpCr = 0, grpCl = 0, grpDue = 0, grpNotYet = 0, grpPayToday = 0, grpShort = 0, grpLastAmt = 0;

        groups[parentName].forEach(r => {
            let accId = r.AccountId || 0;
            let code = r.AccountCode || '';
            let title = r.AccountTitle || '';
            let accType = r.AccountType || 'AP/AR';
            let op = parseNum(r.Opening);
            let dr = parseNum(r.CurrDebit);
            let cr = parseNum(r.CurrCredit);
            let cl = parseNum(r.Closing);
            let due = parseNum(r.DueBalance);
            let notYet = parseNum(r.NotYetDue);
            let payToday = parseNum(r.PayToday);
            let shortEx = parseNum(r['Short/Excess']);
            let incDec = (shortEx < 0 ? 'Increase' : 'Decrease');
            let lastDate = r.LastPaymentDate || '';
            let lastAmt = parseNum(r.LastPaymentAmount);

            grpOp += op; grpDr += dr; grpCr += cr; grpCl += cl;
            grpDue += due; grpNotYet += notYet; grpPayToday += payToday;
            grpShort += shortEx; grpLastAmt += lastAmt;

            $tbody.append(`
                <tr>
                    <td style="text-align: center;"><a href="javascript:void(0)" onclick="openVoucherModal(${Number(accId) || 0})" style="color: #2563eb; font-weight: bold; text-decoration: underline;">${escapeHtml(code)}</a></td>
                    <td>${escapeHtml(title)}</td>
                    <td style="text-align: center;">${escapeHtml(accType)}</td>
                    <td style="text-align: right;">${formatParen(op)}</td>
                    <td style="text-align: right;">${formatParen(dr)}</td>
                    <td style="text-align: right;">${formatParen(cr)}</td>
                    <td style="text-align: right;">${formatParen(cl)}</td>
                    <td style="text-align: right; font-weight: 600;">${formatNum(due)}</td>
                    <td style="text-align: right;">${formatNum(notYet)}</td>
                    <td style="text-align: right;">${formatNum(payToday)}</td>
                    <td style="text-align: right;">${formatParen(shortEx)}</td>
                    <td style="text-align: center; font-weight: 600; color: ${incDec === 'Increase' ? '#15803d' : '#b91c1c'};">${escapeHtml(incDec)}</td>
                    <td style="text-align: center;">${escapeHtml(lastDate)}</td>
                    <td style="text-align: right;">${formatNum(lastAmt)}</td>
                </tr>
            `);
        });

        grandOp += grpOp; grandDr += grpDr; grandCr += grpCr; grandCl += grpCl;
        grandDue += grpDue; grandNotYet += grpNotYet; grandPayToday += grpPayToday;
        grandShort += grpShort; grandLastAmt += grpLastAmt;

        // Render Group Subtotal
        $tbody.append(`
            <tr class="group-subtotal-row" style="background-color: #f1f5f9; font-weight: bold;">
                <td colspan="3" style="text-align: right; color: #475569;">Subtotal:</td>
                <td style="text-align: right; color: #00796B;">${formatParen(grpOp)}</td>
                <td style="text-align: right; color: #00796B;">${formatParen(grpDr)}</td>
                <td style="text-align: right; color: #00796B;">${formatParen(grpCr)}</td>
                <td style="text-align: right; color: #00796B;">${formatParen(grpCl)}</td>
                <td style="text-align: right; color: #00796B;">${formatNum(grpDue)}</td>
                <td style="text-align: right;">${formatNum(grpNotYet)}</td>
                <td style="text-align: right;">${formatNum(grpPayToday)}</td>
                <td style="text-align: right; color: #00796B;">${formatParen(grpShort)}</td>
                <td></td>
                <td></td>
                <td style="text-align: right; color: #00796B;">${formatNum(grpLastAmt)}</td>
            </tr>
        `);
    });

    // Render Grand Total Row
    $tbody.append(`
        <tr class="grand-total-row" style="background-color: #00796B; color: white; font-weight: bold; font-size: 11px;">
            <td colspan="3" style="text-align: right;">Grand Total:</td>
            <td style="text-align: right;">${formatParen(grandOp)}</td>
            <td style="text-align: right;">${formatParen(grandDr)}</td>
            <td style="text-align: right;">${formatParen(grandCr)}</td>
            <td style="text-align: right;">${formatParen(grandCl)}</td>
            <td style="text-align: right;">${formatNum(grandDue)}</td>
            <td style="text-align: right;">${formatNum(grandNotYet)}</td>
            <td style="text-align: right;">${formatNum(grandPayToday)}</td>
            <td style="text-align: right;">${formatParen(grandShort)}</td>
            <td></td>
            <td></td>
            <td style="text-align: right;">${formatNum(grandLastAmt)}</td>
        </tr>
    `);
}

function renderEmptyTable() {
    $('#mainTbody').html(`
        <tr>
            <td colspan="14" style="text-align: center; padding: 20px; color: #64748b; font-size: 12px;">
                No payment schedule data found for selected filter criteria.
            </td>
        </tr>
    `);
}

function openVoucherModal(accountId) {
    if(!Number(accountId))return;
    location.href='/accounts/reports/general-ledger?'+new URLSearchParams({accountId,fromDate:$('#txtDateFrom').val(),toDate:$('#txtDateTo').val()});
}
function openHistoryModal() {
    $('#historyModal').modal('show');
}

function openShortcutModal() {
    $('#shortcutModal').modal('show');
}

function resetFilters() {
    initDefaults();
    $('.select2').val('0').trigger('change');
    loadReport();
}

function parseNum(v) {
    if (!v) return 0;
    let n = parseFloat(v);
    return isNaN(n) ? 0 : n;
}

function formatNum(v) {
    let n = parseNum(v);
    return n.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 0 });
}

function formatParen(v) {
    let n = parseNum(v);
    if (n < 0) {
        return `(${Math.abs(n).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 0 })})`;
    }
    return n.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 0 });
}

function escapeHtml(str) {
    if (!str) return '';
    return String(str).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');
}
