(() => {
  'use strict';

  const $ = id => document.getElementById(id);
  const api = '/sale/reports/outward-gate-pass-report/api';
  const text = value => value == null ? '' : String(value);
  const hidden = new Set(['Id', 'WeighBridgeId']);
  const dates = new Set(['GpDate', 'EntryDate', 'ModifyDate', 'PostDate']);
  const times = new Set(['InDateTime', 'OutDateTime']);
  const onlyPending = new URLSearchParams(location.search).get('onlyPending') === 'true';
  let busy = false, columns = [], yearStart = '', currentRows = [], loaded = false, displayedFilter = null, groupKey = '';

  const localDate = date => `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(2, '0')}-${String(date.getDate()).padStart(2, '0')}`;

  function syncButtons() { $('printButton').disabled = busy; }

  function dateText(value, withTime) {
    if (!value) return '';
    const raw = text(value);
    const date = new Date(/^\d{4}-\d{2}-\d{2}$/.test(raw) ? `${raw}T00:00:00` : raw);
    if (Number.isNaN(date.getTime())) return raw;
    let result = `${String(date.getDate()).padStart(2, '0')}-${String(date.getMonth() + 1).padStart(2, '0')}-${String(date.getFullYear()).slice(-2)}`;
    if (withTime) result += ` ${String(date.getHours() % 12 || 12).padStart(2, '0')}:${String(date.getMinutes()).padStart(2, '0')} ${date.getHours() < 12 ? 'AM' : 'PM'}`;
    return result;
  }

  async function run(action) {
    if (busy) return;
    busy = true;
    $('loader').hidden = false;
    $('message').textContent = '';
    const controls = Array.from(document.querySelectorAll('input,select,button')).map(element => [element, element.disabled]);
    controls.forEach(([element]) => { element.disabled = true; element.setAttribute('aria-busy', 'true'); });
    try {
      await new Promise(resolve => requestAnimationFrame(resolve));
      await action();
    }
    catch (error) { $('message').textContent = error.message || text(error); }
    finally {
      controls.forEach(([element, disabled]) => { element.disabled = disabled; element.removeAttribute('aria-busy'); });
      busy = false;
      syncButtons();
      $('loader').hidden = true;
    }
  }

  async function request(path, body) {
    const headers = { Accept: 'application/json' };
    const csrf = document.querySelector('meta[name="_csrf"]');
    const csrfHeader = document.querySelector('meta[name="_csrf_header"]');
    if (csrf && csrfHeader) headers[csrfHeader.content] = csrf.content;
    const options = { headers };
    if (body) {
      options.method = 'POST';
      headers['Content-Type'] = 'application/json';
      options.body = JSON.stringify(body);
    }
    const response = await fetch(api + path, options);
    if (response.redirected && /\/login(?:[?#]|$)/.test(response.url)) throw new Error('Please sign in again.');
    const result = await response.json().catch(() => { throw new Error('The server did not return report data.'); });
    if (!response.ok) throw new Error(result.message || result.detail || `Report request failed (${response.status})`);
    return result;
  }

  function branchIds() { return Array.from($('branchIds').selectedOptions, option => Number(option.value)); }
  function refreshSelect(select) { if (window.jQuery) window.jQuery(select).trigger('change.select2'); }

  function fill(id, rows, activity) {
    const select = $(id);
    select.replaceChildren(new Option('...Select Any Value...', ''));
    for (const row of rows.filter(item => item.Activity === activity)) {
      select.add(new Option(text(row.ReferenceName), text(id === 'gatePassType' ? row.ReferenceName : row.Id)));
    }
    refreshSelect(select);
  }

  async function lookups(filterByBranches = false) {
    fill('customerId', [], 'Customer');
    fill('gatePassType', [], 'GatePassType');
    const selectedBranches = filterByBranches ? branchIds() : [];
    const query = selectedBranches.length ? '?branchIds=' + encodeURIComponent(selectedBranches.join(',')) : '';
    const rows = await request('/lookups' + query);
    fill('customerId', rows, 'Customer');
    fill('gatePassType', rows, 'GatePassType');
  }

  function period() {
    const type = Number($('dateType').value), today = new Date(), from = new Date();
    if (type === 1) $('fromDate').value = localDate(today);
    if (type === 2) { from.setDate(from.getDate() - 7); $('fromDate').value = localDate(from); }
    if (type === 3) { from.setDate(1); $('fromDate').value = localDate(from); $('toDate').value = localDate(today); }
    if (type === 4) { from.setMonth(0, 1); $('fromDate').value = localDate(from); $('toDate').value = localDate(today); }
    if (type === 5 && yearStart) $('fromDate').value = yearStart;
  }

  function reportFilter() {
    return {
      fromDate: $('fromDate').value,
      toDate: $('toDate').value,
      fromNo: Number($('fromNo').value) || 0,
      toNo: Number($('toNo').value) || 0,
      customerId: Number($('customerId').value) || 0,
      gatePassType: $('gatePassType').value,
      status: $('status').value,
      branchIds: branchIds(),
      onlyPending
    };
  }

  function caption(key) {
    return ({ GpSrNo: 'GP No', GpTypeSrNo: 'GP Type Sr No', GpType: 'GP Type', DeliveryOrderNo: 'DO No', JobLotDescription: 'Job Lot', GpVarietyName: 'Variety Name' })[key]
      || key.replace(/([a-z])([A-Z])/g, '$1 $2');
  }

  function makeRow(row, keys) {
    const tr = document.createElement('tr');
    for (const key of keys) {
      const td = document.createElement('td');
      if (key === 'GpSrNo' && Number(row.Id) > 0 && Number(row.DocumentTypeId) > 0) {
        const link = document.createElement('a');
        link.href = '#';
        link.textContent = text(row[key]);
        link.addEventListener('click', event => {
          event.preventDefault();
          run(async () => {
            if (!window.DocLink) throw new Error('Document navigation is unavailable.');
            await window.DocLink.open(Number(row.DocumentTypeId), Number(row.Id));
          });
        });
        td.append(link);
      } else {
        td.textContent = dates.has(key) || times.has(key) ? dateText(row[key], times.has(key)) : text(row[key]);
      }
      if (typeof row[key] === 'number') td.classList.add('numeric');
      tr.append(td);
    }
    return tr;
  }

  function render(rows, hasLoaded = loaded) {
    const table = $('reportTable'), keys = columns.filter(key => !hidden.has(key));
    currentRows = rows;
    loaded = hasLoaded;
    table.tHead.replaceChildren();
    table.tBodies[0].replaceChildren();
    table.tFoot.replaceChildren();
    $('recordCount').textContent = `${rows.length} records`;
    syncButtons();
    if (!loaded) return;

    const header = document.createElement('tr');
    for (const key of keys) {
      const th = document.createElement('th');
      th.textContent = caption(key);
      th.draggable = true;
      th.dataset.column = key;
      th.addEventListener('dragstart', event => {
        event.dataTransfer.setData('text/plain', key);
        event.dataTransfer.effectAllowed = 'move';
      });
      header.append(th);
    }
    table.tHead.append(header);
    if (!rows.length) return;

    const body = table.tBodies[0];
    const appendDataRow = row => body.append(makeRow(row, keys));
    if (groupKey && keys.includes(groupKey)) {
      const groups = new Map();
      rows.forEach(row => {
        const value = text(row[groupKey]);
        if (!groups.has(value)) groups.set(value, []);
        groups.get(value).push(row);
      });
      let index = 0;
      for (const [value, members] of groups) {
        const group = document.createElement('tr'), cell = document.createElement('td'), groupId = String(index++);
        group.className = 'gb-group';
        group.dataset.groupId = groupId;
        group.tabIndex = 0;
        cell.colSpan = keys.length;
        cell.textContent = `${caption(groupKey)}: ${value || '(blank)'} (${members.length})`;
        group.append(cell);
        const groupRows = [];
        const toggle = () => {
          const closed = group.classList.toggle('gb-group-closed');
          groupRows.forEach(element => element.classList.toggle('gb-collapsed', closed));
        };
        group.addEventListener('click', toggle);
        group.addEventListener('keydown', event => { if (event.key === 'Enter') { event.preventDefault(); toggle(); } });
        body.append(group);
        members.forEach(member => {
          const element = makeRow(member, keys);
          element.dataset.memberGroup = groupId;
          element.dataset.rowIndex = String(rows.indexOf(member));
          body.append(element);
          groupRows.push(element);
        });
      }
    } else {
      rows.forEach(appendDataRow);
    }

  }

  async function show() {
    const filter = reportFilter();
    if (!filter.branchIds.length) throw new Error('Select Branch First');
    if (!filter.fromDate || !filter.toDate) throw new Error('From Date and To Date are required');
    displayedFilter = null;
    render([], false);
    currentRows = await request('/rows', filter);
    displayedFilter = filter;
    render(currentRows, true);
  }

  async function printRegister() {
    if (!loaded || !currentRows.length || !displayedFilter) throw new Error('Press Show to load the report before printing.');
    const filter = displayedFilter;
    const headers = { 'Content-Type': 'application/json', Accept: 'application/pdf' };
    const csrf = document.querySelector('meta[name="_csrf"]'), csrfHeader = document.querySelector('meta[name="_csrf_header"]');
    if (csrf && csrfHeader) headers[csrfHeader.content] = csrf.content;
    const preview = window.open('about:blank', '_blank');
    if (!preview) throw new Error('Allow pop-ups to open the report.');
    preview.document.title = 'Preparing Outward Gate Pass Register';
    preview.document.body.textContent = 'Preparing report…';
    const body = {
      supplierCustomerId: filter.customerId || null,
      fromDate: filter.fromDate,
      toDate: filter.toDate,
      fromDocNo: filter.fromNo || null,
      toDocNo: filter.toNo || null,
      gatepassType: filter.gatePassType || null,
      status: filter.status || null,
      branchesIds: filter.branchIds.join(','),
      pendingForView: filter.onlyPending ? 'true' : null
    };
    try {
      const response = await fetch('/reports/print/291-gate-pass-outward-register', { method: 'POST', headers, body: JSON.stringify(body) });
      if (!response.ok || !response.headers.get('Content-Type')?.includes('application/pdf')) {
        const error = await response.text();
        let message;
        try { message = JSON.parse(error).message; } catch { if (response.headers.get('Content-Type')?.includes('text/plain')) message = error; }
        throw new Error(message || 'Unable to print the register. Please check your session and report filters.');
      }
      const url = URL.createObjectURL(await response.blob());
      preview.location.href = url;
      setTimeout(() => URL.revokeObjectURL(url), 300000);
    } catch (error) {
      preview.close();
      throw error;
    }
  }

  function newReport() {
    $('fromNo').value = '';
    $('toNo').value = '';
    $('status').value = '';
    refreshSelect($('status'));
    return show();
  }

  $('reportForm').addEventListener('submit', event => { event.preventDefault(); run(show); });
  $('refreshButton').addEventListener('click', () => run(() => lookups(true)));
  $('newButton').addEventListener('click', () => run(newReport));
  $('printButton').addEventListener('click', () => run(printRegister));
  $('shortcutsButton').addEventListener('click', () => $('shortcutsDialog').showModal());

  $('groupStrip').addEventListener('dragover', event => { event.preventDefault(); $('groupStrip').classList.add('drag-over'); });
  $('groupStrip').addEventListener('dragleave', () => $('groupStrip').classList.remove('drag-over'));
  $('groupStrip').addEventListener('drop', event => {
    event.preventDefault();
    $('groupStrip').classList.remove('drag-over');
    const key = event.dataTransfer.getData('text/plain');
    if (!columns.includes(key) || hidden.has(key)) return;
    groupKey = key;
    $('groupStrip').firstChild.textContent = 'Grouped by ' + caption(key);
    $('clearGrouping').hidden = false;
    render(currentRows, loaded);
  });
  $('clearGrouping').addEventListener('click', () => {
    groupKey = '';
    $('groupStrip').firstChild.textContent = 'Drag a column header here to group by that column.';
    $('clearGrouping').hidden = true;
    render(currentRows, loaded);
  });

  if (window.jQuery && window.jQuery.fn.select2) {
    window.jQuery('.searchable').attr('data-dtcombo-skip', '').select2({ width: '100%' });
    window.jQuery('#dateType').on('change', period);
  } else {
    $('dateType').addEventListener('change', period);
  }

  document.addEventListener('keydown', event => {
    if (busy || $('shortcutsDialog').open || !event.ctrlKey) return;
    const key = event.key.toLowerCase();
    if (['s', 'r', 'n', 'p'].includes(key)) {
      event.preventDefault();
      run(key === 's' ? show : key === 'r' ? lookups : key === 'n' ? newReport : printRegister);
    } else if (key === 'e') {
      event.preventDefault();
      location.href = '/sale/reports';
    } else if (event.altKey) {
      event.preventDefault();
      $('shortcutsDialog').showModal();
    } else if (event.key === 'ArrowDown') {
      event.preventDefault();
      $('reportGrid').focus();
    } else if (event.key === 'ArrowUp') {
      event.preventDefault();
      $('dateType').focus();
    }
  });

  run(async () => {
    const data = await request('/initial');
    columns = data.columns;
    yearStart = data.yearStart;
    if (onlyPending) $('outwardGatePassReport').querySelector('h1').textContent += ' (Pending)';
    const seen = new Set();
    for (const row of data.branches) {
      if (seen.has(row.BranchId)) continue;
      seen.add(row.BranchId);
      $('branchIds').add(new Option(text(row.BranchName), text(row.BranchId), false, Number(row.BranchId) === Number(data.branchId)));
    }
    refreshSelect($('branchIds'));
    $('toDate').value = localDate(new Date());
    period();
    render([], false);
    await lookups();
    if (onlyPending) await show();
  });
})();
