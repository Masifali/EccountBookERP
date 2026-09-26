'use strict';

(function () {
    const el = id => document.getElementById(id);
    let lookup = {}, result = { rows: [], overdue: [], notYetDue: [], totals: {} }, sequence = 0, request = null;

    function formatNumber(val) {
        if (val === null || val === undefined || val === '') return '0';
        let num = Number(val);
        if (isNaN(num)) return val;
        const absStr = Math.abs(num).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 2 });
        return num < 0 ? `(${absStr})` : absStr;
    }

    function status(text) {
        const s = el('lblStatus');
        if (s) s.textContent = text;
    }

    function setLoading(isLoading) {
        const buttons = document.querySelectorAll('.btn-erp, #btnShow, #btnRefresh, #btnNew, #btnApplySorting');
        buttons.forEach(btn => {
            if (isLoading) {
                btn.setAttribute('disabled', 'disabled');
                btn.classList.add('disabled');
            } else {
                btn.removeAttribute('disabled');
                btn.classList.remove('disabled');
            }
        });
    }

    async function api(path, options) {
        const r = await fetch('/api/accounts/receivables-aging' + path, options);
        if (!r.ok) throw new Error('Receivables Aging request failed (' + r.status + ').');
        return r.json();
    }

    function bindSelect(id, data, labelKey, valueKey = 'Id', placeholder = '-- Select --') {
        const $elem = $('#' + id);
        const currentVal = $elem.val();
        const nativeElem = el(id);
        if (!nativeElem) return;

        nativeElem.replaceChildren(new Option(placeholder, '0'));
        if (Array.isArray(data)) {
            data.forEach(item => {
                const text = item[labelKey] ?? item.name ?? item.accountTitle ?? item.cityName ?? '';
                const val = item[valueKey] ?? item.id ?? item.ChartOfAccountId ?? '';
                if (text && val !== '') {
                    nativeElem.add(new Option(text, val));
                }
            });
        }
        if (currentVal && $elem.find(`option[value="${currentVal}"]`).length) {
            $elem.val(currentVal).trigger('change.select2');
        } else {
            $elem.val('0').trigger('change.select2');
        }
    }

    function filterAccounts() {
        const parentId = Number($('#cmbControlAccount').val() || 0);
        const seen = new Set();
        const rawAccounts = lookup.accounts || [];
        const filtered = rawAccounts.filter(r => {
            const isReceivable = Number(r.AccountClass || 2) === 2 || [1, 4, 12, 16].includes(Number(r.AccountTypeId));
            const matchesParent = !parentId || Number(r.ParentCodeId) === parentId;
            const notSeen = !seen.has(r.ChartOfAccountId);
            if (notSeen) seen.add(r.ChartOfAccountId);
            return isReceivable && matchesParent;
        });
        bindSelect('cmbAccountTitle', filtered, 'AccountTitle', 'ChartOfAccountId', '-- Select Account --');
    }

    async function refreshLookups() {
        setLoading(true);
        status('Loading dropdown criteria...');
        try {
            lookup = await api('/lookups');

            const controls = (lookup.controls || []).filter(r => Number(r.TypeNo || 2) === 2 || Number(r.TypeNo || 1) === 1);
            bindSelect('cmbControlAccount', controls.length ? controls : lookup.controls, 'AccountTitle', 'id', '-- Select Control Account --');
            bindSelect('cmbCustomGroup', lookup.customGroups || [], 'AcLookUpsDescription', 'Id', '-- Select Custom Group --');
            bindSelect('cmbCostCenter', lookup.costCenters || [], 'CostCenterName', 'Id', '-- Select Cost Center --');
            bindSelect('cmbCity', lookup.cities || [], 'cityName', 'id', '-- Select City --');
            bindSelect('cmbBranches', lookup.branches || [], 'BranchName', 'Id', '-- Select Branch --');

            filterAccounts();
            status('Ready. Choose filters and click Show.');
        } catch (e) {
            status('Error loading lookups: ' + e.message);
        } finally {
            setLoading(false);
        }
    }

    const fields = [
        ['AccountCode', 'Account Code', 110],
        ['AccountTitle', 'Account Title', 260],
        ['AccountType', 'Account Type', 70],
        ['CurrentBalance', 'Current Balance', 120],
        ['OverDue', 'Over Due', 120],
        ['NotYetDue', 'Not Yet Due', 100],
        ['IstIntervale', '1 30', 110],
        ['ScnInterval', '31 60', 110],
        ['TrdIntarval', '61 90', 110],
        ['Above', '90 Above', 110]
    ];

    function openVoucherModal(row, isOverdue) {
        const modal = el('dialogVoucherDetails');
        if (!modal) return;

        const list = (isOverdue ? result.overdue : result.notYetDue).filter(r => String(r.AccountId) === String(row.AccountId));

        el('voucherModalTitle').textContent = `Receivable Aging Details (${isOverdue ? 'Over Due' : 'Not Yet Due'}) - ${row.AccountTitle || ''}`;
        const tbody = el('tbodyVoucherDetails');
        tbody.replaceChildren();

        if (!list.length) {
            const tr = tbody.insertRow();
            const td = tr.insertCell();
            td.colSpan = 8;
            td.style.textAlign = 'center';
            td.style.padding = '12px';
            td.textContent = 'No detailed voucher records found for this account.';
        } else {
            list.forEach(item => {
                const tr = tbody.insertRow();
                tr.insertCell().textContent = item.DocumentTypeDescription || item.DocumentType || 'Voucher';

                const tdVoucher = tr.insertCell();
                const aVoucher = document.createElement('a');
                aVoucher.href = '#';
                aVoucher.style.fontWeight = 'bold';
                aVoucher.style.color = '#00796B';
                aVoucher.textContent = item.VoucherCode || item.VoucherNo || 'View Voucher';
                aVoucher.onclick = (e) => {
                    e.preventDefault();
                    alert(`Voucher Details: ${item.VoucherCode || ''}\nDate: ${item.VoucherDate || ''}\nAmount: ${item.Amount || 0}`);
                };
                tdVoucher.append(aVoucher);

                tr.insertCell().textContent = String(item.VoucherDate || '').slice(0, 10);
                tr.insertCell().textContent = item.DueDays || '0';
                tr.insertCell().textContent = String(item.DueDate || '').slice(0, 10);
                if (isOverdue) {
                    tr.insertCell().textContent = item.OverDueBy || '0';
                } else {
                    tr.insertCell().textContent = '-';
                }
                tr.insertCell().textContent = item.AccountTitle || row.AccountTitle || '';

                const tdAmt = tr.insertCell();
                tdAmt.style.textAlign = 'right';
                tdAmt.style.fontWeight = 'bold';
                tdAmt.textContent = formatNumber(item.Amount || 0);
            });
        }
        modal.showModal();
    }

    function renderTable() {
        const rowsData = result.rows || [];
        const first = rowsData[0] || {};

        // Update interval headers & summary cards if captions returned
        const captions = ['FirstIntervalCaption', 'SecondIntervalCaption', 'ThirdIntervalCaption', 'AboveIntervalCaption'];
        captions.forEach((capKey, idx) => {
            if (first[capKey]) {
                fields[6 + idx][1] = first[capKey];
                const cardHead = el(`cardHeader_${idx + 1}`);
                if (cardHead) cardHead.textContent = first[capKey];
            }
        });

        // Fill metric summary tiles
        if (result.totals) {
            el('tile1_30').textContent = formatNumber(result.totals.IstIntervale || 0);
            el('tile31_60').textContent = formatNumber(result.totals.ScnInterval || 0);
            el('tile61_90').textContent = formatNumber(result.totals.TrdIntarval || 0);
            el('tile90_Above').textContent = formatNumber(result.totals.Above || 0);

            el('lblCurrBalance').textContent = formatNumber(result.totals.CurrentBalance || 0);
            el('lblOverDue').textContent = formatNumber(result.totals.OverDue || 0);
            el('lblNotYetDue').textContent = formatNumber(result.totals.NotYetDue || 0);
        }

        const tbody = el('tbodyReport');
        tbody.replaceChildren();

        const term = (el('txtSearch').value || '').toLowerCase().trim();
        const checkedSorts = [...document.querySelectorAll('.chk-sort:checked')].map(c => c.value);
        const direction = Number(document.querySelector('[name=sortDirection]:checked')?.value || 1);

        let filteredRows = rowsData.filter(r => {
            if (!term) return true;
            return fields.some(([k]) => String(r[k] ?? '').toLowerCase().includes(term));
        });

        if (checkedSorts.length) {
            filteredRows.sort((a, b) => {
                for (const k of checkedSorts) {
                    let diff = 0;
                    if (['CurrentBalance', 'OverDue', 'NotYetDue', 'IstIntervale', 'ScnInterval', 'TrdIntarval', 'Above'].includes(k)) {
                        diff = Number(a[k] || 0) - Number(b[k] || 0);
                    } else {
                        diff = String(a[k] ?? '').localeCompare(String(b[k] ?? ''));
                    }
                    if (diff !== 0) return diff * direction;
                }
                return 0;
            });
        }

        let currentGroup = null;
        let groupTotals = { CurrentBalance: 0, OverDue: 0, NotYetDue: 0, IstIntervale: 0, ScnInterval: 0, TrdIntarval: 0, Above: 0 };
        let grandTotals = { CurrentBalance: 0, OverDue: 0, NotYetDue: 0, IstIntervale: 0, ScnInterval: 0, TrdIntarval: 0, Above: 0 };

        function addSubtotalRow(grpName) {
            if (!grpName) return;
            const tr = tbody.insertRow();
            tr.style.backgroundColor = '#e6f2ff';
            tr.style.fontWeight = 'bold';

            const tdLabel = tr.insertCell();
            tdLabel.colSpan = 3;
            tdLabel.style.textAlign = 'right';
            tdLabel.textContent = `Subtotal (${grpName}):`;

            ['CurrentBalance', 'OverDue', 'NotYetDue', 'IstIntervale', 'ScnInterval', 'TrdIntarval', 'Above'].forEach(k => {
                const td = tr.insertCell();
                td.style.textAlign = 'right';
                td.textContent = formatNumber(groupTotals[k]);
            });

            // Reset group totals
            Object.keys(groupTotals).forEach(k => groupTotals[k] = 0);
        }

        filteredRows.forEach(row => {
            const grp = row.ParentAccount || 'TRADE CUSTOMERS';
            if (currentGroup !== grp) {
                if (currentGroup !== null) {
                    addSubtotalRow(currentGroup);
                }
                currentGroup = grp;
                const trGrp = tbody.insertRow();
                trGrp.className = 'group-header-row';
                trGrp.style.backgroundColor = '#cce5ff';
                trGrp.style.fontWeight = 'bold';
                trGrp.style.color = '#004d40';

                const tdGrp = trGrp.insertCell();
                tdGrp.colSpan = fields.length;
                tdGrp.textContent = `Parent Account: ${grp}`;
            }

            const tr = tbody.insertRow();
            tr.tabIndex = 0;
            tr.onclick = () => {
                tbody.querySelectorAll('.selected-row').forEach(x => x.classList.remove('selected-row'));
                tr.classList.add('selected-row');
            };

            fields.forEach(([k]) => {
                const td = tr.insertCell();
                const val = row[k];

                if (k === 'AccountCode') {
                    const a = document.createElement('a');
                    a.style.color = '#00796B';
                    a.style.fontWeight = 'bold';
                    a.style.textDecoration = 'underline';
                    a.textContent = val || '';
                    a.href = `/accounts/reports/general-ledger?accountId=${row.AccountId || 0}&fromDate=${String(row.StartDate || '').slice(0, 10)}&toDate=${el('dtpAsOnDate').value}&branchId=${row.BranchesId || 0}`;
                    td.append(a);
                } else if (k === 'OverDue' || k === 'NotYetDue') {
                    td.style.textAlign = 'right';
                    const numVal = Number(val || 0);
                    if (numVal !== 0) {
                        const btn = document.createElement('button');
                        btn.type = 'button';
                        btn.className = 'btn-link-erp';
                        btn.style.background = 'none';
                        btn.style.border = 'none';
                        btn.style.color = '#00796B';
                        btn.style.textDecoration = 'underline';
                        btn.style.cursor = 'pointer';
                        btn.style.fontWeight = 'bold';
                        btn.textContent = formatNumber(val);
                        btn.onclick = (e) => {
                            e.stopPropagation();
                            openVoucherModal(row, k === 'OverDue');
                        };
                        td.append(btn);
                    } else {
                        td.textContent = '0';
                    }
                } else if (['CurrentBalance', 'IstIntervale', 'ScnInterval', 'TrdIntarval', 'Above'].includes(k)) {
                    td.style.textAlign = 'right';
                    td.textContent = formatNumber(val);
                } else {
                    td.textContent = val ?? '';
                }
            });

            // Accumulate totals
            ['CurrentBalance', 'OverDue', 'NotYetDue', 'IstIntervale', 'ScnInterval', 'TrdIntarval', 'Above'].forEach(k => {
                const n = Number(row[k] || 0);
                groupTotals[k] += n;
                grandTotals[k] += n;
            });
        });

        if (currentGroup !== null) {
            addSubtotalRow(currentGroup);
        }

        // Grand Total row
        if (filteredRows.length > 0) {
            const trGrand = tbody.insertRow();
            trGrand.style.backgroundColor = '#00796B';
            trGrand.style.color = '#ffffff';
            trGrand.style.fontWeight = 'bold';

            const tdGrandLabel = trGrand.insertCell();
            tdGrandLabel.colSpan = 3;
            tdGrandLabel.style.textAlign = 'right';
            tdGrandLabel.textContent = 'Grand Total:';

            ['CurrentBalance', 'OverDue', 'NotYetDue', 'IstIntervale', 'ScnInterval', 'TrdIntarval', 'Above'].forEach(k => {
                const td = trGrand.insertCell();
                td.style.textAlign = 'right';
                td.style.color = '#ffffff';
                td.textContent = formatNumber(grandTotals[k]);
            });
        }

        status(`${filteredRows.length} customer account records displayed.`);
    }

    async function loadReport() {
        const token = ++sequence;
        if (request) request.abort();
        request = new AbortController();

        setLoading(true);
        status('Fetching Receivables Aging Report...');

        try {
            const mode = document.querySelector('[name=modeRadio]:checked')?.value || 1;
            const params = new URLSearchParams({
                asOnDate: el('dtpAsOnDate').value,
                agingDays: el('txtAgingDays').value || 30,
                reportId: mode,
                parentId: $('#cmbControlAccount').val() || 0,
                accountId: $('#cmbAccountTitle').val() || 0,
                customGroupId: $('#cmbCustomGroup').val() || 0,
                costCenterId: $('#cmbCostCenter').val() || 0,
                branches: [$('#cmbBranches').val()].flat().filter(Boolean).join(',')
            });

            const data = await api('?' + params.toString(), { signal: request.signal });
            if (token !== sequence) return;

            result = data;
            renderTable();
        } catch (e) {
            if (e.name !== 'AbortError') {
                status('Error: ' + e.message);
            }
        } finally {
            setLoading(false);
        }
    }

    function initEvents() {
        el('btnShow').onclick = loadReport;

        el('btnRefresh').onclick = () => {
            refreshLookups().then(loadReport);
        };

        el('btnNew').onclick = () => {
            el('txtAgingDays').value = '30';
            $('#cmbControlAccount').val('0').trigger('change');
            $('#cmbCustomGroup').val('0').trigger('change');
            $('#cmbCostCenter').val('0').trigger('change');
            $('#cmbCity').val('0').trigger('change');
            $('#cmbBranches').val('0').trigger('change');
            filterAccounts();
            loadReport();
        };

        el('btnApplySorting').onclick = () => {
            setLoading(true);
            setTimeout(() => {
                renderTable();
                setLoading(false);
            }, 100);
        };

        el('txtSearch').oninput = renderTable;
        $('#cmbControlAccount').on('change', filterAccounts);

        const closeVoucherModalBtn = el('btnCloseVoucherModal');
        if (closeVoucherModalBtn) {
            closeVoucherModalBtn.onclick = () => el('dialogVoucherDetails').close();
        }

        const btnShortcuts = el('btnShortcuts');
        if (btnShortcuts) {
            btnShortcuts.onclick = () => el('dialogShortcuts').showModal();
        }
        const btnCloseShortcuts = el('btnCloseShortcuts');
        if (btnCloseShortcuts) {
            btnCloseShortcuts.onclick = () => el('dialogShortcuts').close();
        }

        const btnHistory = el('btnHistory');
        if (btnHistory) {
            btnHistory.onclick = () => {
                el('dialogHistory').showModal();
            };
        }
        const btnCloseHistory = el('btnCloseHistory');
        if (btnCloseHistory) {
            btnCloseHistory.onclick = () => el('dialogHistory').close();
        }

        el('btnPrint').onclick = () => window.print();

        document.addEventListener('keydown', e => {
            if (e.ctrlKey) {
                const key = e.key.toLowerCase();
                if (key === 's') {
                    e.preventDefault();
                    loadReport();
                } else if (key === 'n') {
                    e.preventDefault();
                    el('btnNew').click();
                } else if (key === 'r') {
                    e.preventDefault();
                    refreshLookups();
                } else if (key === 'p') {
                    e.preventDefault();
                    window.print();
                }
            }
        });
    }

    $(document).ready(function () {
        const today = new Date().toISOString().split('T')[0];
        el('dtpAsOnDate').value = today;

        $('.select2-elem').select2({ placeholder: '-- Select --', allowClear: true });

        initEvents();
        refreshLookups().then(loadReport);
    });
})();
