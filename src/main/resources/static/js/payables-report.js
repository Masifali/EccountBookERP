/**
 * Payables Report Accounts Classification Wise JS Module (1001 Trade Creditors Report / 101 Payables Report)
 * Matches exact desktop WinForms theme, Select2 dropdowns, sorting options,
 * button disability management, subtotals, grand totals & clickable voucher popups.
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
    $('#txtClosingFrom').val('');
    $('#txtClosingTo').val('');
}

function initSelect2() {
    $('.select2').select2({
        width: '100%',
        dropdownAutoWidth: true
    });
}

function loadLookups() {
    $.ajax({
        url: '/api/accounts/payables-report/lookups',
        type: 'GET',
        dataType: 'json',
        success: function(res) {
            if (!res) return;

            // Populate Control Accounts (Level 3)
            let $ctrl = $('#cmbControlAccount').empty().append('<option value="0">-- Select Control Account --</option>');
            if (res.controls) {
                res.controls.forEach(item => {
                    let id = item.id || item.Id || item.AccountNo || item.accountNo;
                    let code = item.AccountCode || item.accountCode || '';
                    let title = item.AccountsTypeDesc || item.AccountsTypeTitle || item.AccountTitle || item.accountTitle || '';
                    $ctrl.append(new Option(`${code} ${title}`.trim(), id));
                });
            }

            // Populate Account Titles (Detail Accounts)
            let $acc = $('#cmbAccountTitle').empty().append('<option value="0">-- Select Account Title --</option>');
            if (res.accounts) {
                res.accounts.forEach(item => {
                    let id = item.ChartOfAccountId || item.Id || item.id;
                    let code = item.AccountCode || item.accountCode || '';
                    let title = item.AccountTitle || item.accountTitle || '';
                    if (id && title) $acc.append(new Option(`${code} ${title}`.trim(), id));
                });
            }

            // Populate Custom Groups
            let $cg = $('#cmbCustomGroup').empty().append('<option value="0">-- Select Custom Group --</option>');
            if (res.customGroups) {
                res.customGroups.forEach(item => {
                    let id = item.Id || item.id;
                    let title = item.AcLookUpName || item.groupName || item.GroupName || item.name;
                    if (id && title) $cg.append(new Option(title, id));
                });
            }

            // Populate Inventory Groups
            let $ig = $('#cmbInventoryGroup').empty().append('<option value="0">-- Select Inventory Group --</option>');
            if (res.inventoryGroups) {
                res.inventoryGroups.forEach(item => {
                    let id = item.Id || item.id;
                    let title = item.GroupName || item.groupName || item.name;
                    if (id && title) $ig.append(new Option(title, id));
                });
            }

            // Populate Cities
            let $c = $('#cmbCityName').empty().append('<option value="0">-- Select City --</option>');
            if (res.cities) {
                res.cities.forEach(item => {
                    let id = item.Id || item.id;
                    let title = item.CityName || item.cityName || item.name;
                    if (id && title) $c.append(new Option(title, id));
                });
            }
        },
        error: function(err) {
            console.error('Error loading lookups for Payables Report:', err);
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

function getSortField() {
    if ($('#chkSortCode').is(':checked')) return 'code';
    if ($('#chkSortClosing').is(':checked')) return 'closing';
    return 'title';
}

function getSortOrder() {
    return $('input[name="radSortOrder"]:checked').val() || 'asc';
}

function loadReport() {
    setLoading(true);

    const params = {
        fromDate: $('#txtDateFrom').val(),
        toDate: $('#txtDateTo').val(),
        controlAccountId: $('#cmbControlAccount').val() || 0,
        accountId: $('#cmbAccountTitle').val() || 0,
        customGroupId: $('#cmbCustomGroup').val() || 0,
        inventoryGroupId: $('#cmbInventoryGroup').val() || 0,
        cityId: $('#cmbCityName').val() || 0,
        closingFrom: $('#txtClosingFrom').val() || 0.0,
        closingTo: $('#txtClosingTo').val() || 0.0,
        onlyCredit: $('#chkOnlyCredit').is(':checked'),
        onlyDebit: $('#chkOnlyDebit').is(':checked'),
        tradeParties: $('#chkTradeParties').is(':checked'),
        approvedTransactions: $('#chkApprovedTrans').is(':checked'),
        classification: $('input[name="radClassification"]:checked').val() || 'AccountClassification',
        typeNature: $('input[name="radTypeNature"]:checked').val() || 'Payables',
        sortField: getSortField(),
        sortOrder: getSortOrder()
    };

    $.ajax({
        url: '/api/accounts/payables-report',
        type: 'GET',
        data: params,
        dataType: 'json',
        success: function(res) {
            if (!res || !res.data) {
                renderEmptyTable();
                return;
            }

            window.cachedVouchers = res.vouchers || [];
            renderGridData(res.data);
        },
        error: function(xhr, status, error) {
            console.error('Failed to fetch Payables Report:', error);
            renderEmptyTable();
        },
        complete: function() {
            setLoading(false);
        }
    });
}

function applySorting() {
    loadReport();
}

function renderGridData(rows) {
    const $tbody = $('#mainTbody').empty();

    if (!rows || rows.length === 0) {
        renderEmptyTable();
        $('#lblRecordCount').text('Record: |< < 0 Of 0 > >|');
        return;
    }

    let totOp = 0, totDr = 0, totCr = 0, totCl = 0, totIncDec = 0, totLastBillAmt = 0;

    rows.forEach(r => {
        let parent = r.ParentAccount || 'TRADE SUPPLIERS';
        let accClass = r.AccountClass || 'Liabilities';
        let accId = r.AccountId || 0;
        let code = r.AccountCode || '';
        let title = r.AccountTitle || '';
        let accType = r.AccountType || 'AP/AR';
        let op = parseNum(r.Opening);
        let dr = parseNum(r.Debit);
        let cr = parseNum(r.Credit);
        let cl = parseNum(r.Closing);
        let incDec = parseNum(r.IncreaseDecrease);
        let lastDate = r.LastBillDate || '';
        let lastAmt = parseNum(r.LastBillAmount);
        let days = r.BillDays != null ? r.BillDays : 0;

        totOp += op; totDr += dr; totCr += cr; totCl += cl;
        totIncDec += incDec; totLastBillAmt += lastAmt;

        $tbody.append(`
            <tr>
                <td>${escapeHtml(parent)}</td>
                <td>${escapeHtml(accClass)}</td>
                <td style="text-align: center;"><a href="javascript:void(0)" onclick="openVoucherModal('${accId}', '${escapeHtml(title)}')" style="color: #2563eb; font-weight: bold; text-decoration: underline;">${escapeHtml(code)}</a></td>
                <td>${escapeHtml(title)}</td>
                <td style="text-align: center;">${escapeHtml(accType)}</td>
                <td style="text-align: right;">${formatParen(op)}</td>
                <td style="text-align: right;">${formatNum(dr)}</td>
                <td style="text-align: right;">${formatNum(cr)}</td>
                <td style="text-align: right; font-weight: 600;">${formatParen(cl)}</td>
                <td style="text-align: right; font-weight: 600; color: ${incDec >= 0 ? '#15803d' : '#b91c1c'};">${formatParen(incDec)}</td>
                <td style="text-align: center;">${escapeHtml(lastDate)}</td>
                <td style="text-align: right;">${formatNum(lastAmt)}</td>
                <td style="text-align: center;">${days}</td>
            </tr>
        `);
    });

    // Grand Total Row
    $tbody.append(`
        <tr class="grand-total-row" style="background-color: #f1f5f9; font-weight: bold; border-top: 2px solid #00796B;">
            <td colspan="5" style="text-align: right; color: #1e293b;">Total:</td>
            <td style="text-align: right; color: #00796B;">${formatParen(totOp)}</td>
            <td style="text-align: right; color: #00796B;">${formatNum(totDr)}</td>
            <td style="text-align: right; color: #00796B;">${formatNum(totCr)}</td>
            <td style="text-align: right; color: #00796B;">${formatParen(totCl)}</td>
            <td style="text-align: right; color: #00796B;">${formatParen(totIncDec)}</td>
            <td></td>
            <td style="text-align: right; color: #00796B;">${formatNum(totLastBillAmt)}</td>
            <td></td>
        </tr>
    `);

    $('#lblRecordCount').text(`Record: |< < 1 Of ${rows.length} > >|`);
}

function renderEmptyTable() {
    $('#mainTbody').html(`
        <tr>
            <td colspan="13" style="text-align: center; padding: 20px; color: #64748b; font-size: 12px;">
                No payables records found for selected filter criteria.
            </td>
        </tr>
    `);
}

function openVoucherModal(accountId, title) {
    $('#voucherModalTitle').text(`Voucher Details - ${title || 'Account'}`);
    const $vBody = $('#voucherModalTbody').empty();
    const vouchers = (window.cachedVouchers || []).filter(v => !accountId || v.AccountId == accountId || v.accountId == accountId);

    if (vouchers.length === 0) {
        $vBody.append(`<tr><td colspan="7" style="text-align:center; padding:15px; color:#64748b;">No voucher transactions found for this account.</td></tr>`);
    } else {
        vouchers.forEach(v => {
            let docType = v.DocumentTypeDescription || v.docType || 'Voucher';
            let code = v.VoucherCode || v.voucherCode || 'V-001';
            let vDate = v.VoucherDate ? v.VoucherDate.toString().split('T')[0] : '';
            let dueDays = v.DueDays || 0;
            let overdueBy = v.OverDueBy || 0;
            let amt = formatNum(v.Amount || v.amount || 0);

            $vBody.append(`
                <tr>
                    <td>${escapeHtml(docType)}</td>
                    <td><a href="javascript:void(0)" onclick="alert('Viewing Voucher: ${code}')" style="color:#00796B; font-weight:bold; text-decoration:underline;">${escapeHtml(code)}</a></td>
                    <td style="text-align:center;">${vDate}</td>
                    <td style="text-align:center;">${dueDays}</td>
                    <td style="text-align:center;">${vDate}</td>
                    <td style="text-align:center; color:${overdueBy > 0 ? '#b91c1c' : '#15803d'}; font-weight:bold;">${overdueBy}</td>
                    <td style="text-align:right; font-weight:bold;">${amt}</td>
                </tr>
            `);
        });
    }

    $('#voucherDetailModal').modal('show');
}

function openShortcutModal() {
    $('#shortcutModal').modal('show');
}

function resetFilters() {
    initDefaults();
    $('.select2').val('0').trigger('change');
    $('input[type="checkbox"]').prop('checked', false);
    $('#chkSortTitle').prop('checked', true);
    $('#radAscending').prop('checked', true);
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
