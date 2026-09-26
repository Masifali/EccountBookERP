'use strict';
window.TradeGrid = (() => {
    const filters = new Map();
    let groupedBy = '', selected = null;
    const signed = new Set(['Opening', 'Closing', 'Increase/Decrease']);
    function render(config) {
        const {table, columns, rows, numeric, decimals, term, sort, direction, accountClass, ledger, date, status} = config;
        let bar = document.getElementById('tradeGrouping');
        if (!bar) {
            bar = document.createElement('div'); bar.id = 'tradeGrouping';
            bar.style.cssText = 'min-height:26px;padding:3px 6px;background:#eee;border-bottom:1px solid #aaa';
            table.parentElement.before(bar);
            bar.ondragover = e => e.preventDefault();
            bar.ondrop = e => {e.preventDefault();groupedBy = e.dataTransfer.getData('text/plain');render(config)};
        }
        bar.replaceChildren();
        const groupLabel = document.createElement('span');
        groupLabel.textContent = groupedBy ? 'Grouped by ' + (columns.find(c => c[0] === groupedBy)?.[1] || groupedBy) + ' ' : 'Drag a column header here to group by that column';
        bar.append(groupLabel);
        if (groupedBy) {const clear = document.createElement('button');clear.textContent = 'Remove grouping';clear.onclick = () => {groupedBy='';render(config)};bar.append(clear)}
        table.tHead.replaceChildren(); table.tBodies[0].replaceChildren();
        const footer = table.tFoot || table.createTFoot();footer.replaceChildren();
        const header = table.tHead.insertRow(), filterRow = table.tHead.insertRow();
        columns.forEach(([key, label, width]) => {
            const th = document.createElement('th');th.style.width = th.style.minWidth = width + 'px';
            th.textContent = accountClass === 2 ? ({LastRcvdDate:'Last Received Date',LastRcvdAmount:'Last Received Amount',RcvdDays:'Received Days'}[key] || label) : label;
            th.draggable = true;th.ondragstart = e => e.dataTransfer.setData('text/plain',key);header.append(th);
            const cell = document.createElement('th');cell.style.top='38px';cell.style.height='26px';
            const input = document.createElement('input');input.setAttribute('aria-label','Filter '+label);input.style.cssText='width:100%;min-width:0;height:22px';input.value=filters.get(key)||'';
            input.oninput = () => {const position=input.selectionStart;filters.set(key,input.value);render(config);const next=table.tHead.rows[1].cells[columns.findIndex(c=>c[0]===key)].firstChild;next.focus();next.setSelectionRange(position,position)};
            cell.append(input);filterRow.append(cell);
        });
        const visible = rows.filter(row => columns.some(([key]) => String(row[key]??'').toLowerCase().includes(term.toLowerCase())) && [...filters].every(([key,value])=>String(row[key]??'').toLowerCase().includes(value.toLowerCase())));
        visible.sort((a,b)=>{for(const key of sort){const difference=numeric.has(key)?Number(a[key]||0)-Number(b[key]||0):String(a[key]??'').localeCompare(String(b[key]??''));if(difference)return difference*direction}return 0});
        const totals = (section, records, label) => {
            const tr=section.insertRow();tr.className='trade-total';
            columns.forEach(([key],index)=>{const td=tr.insertCell();if(numeric.has(key)){td.className='num';td.textContent=ReportDecimal.format(ReportDecimal.sum(records.map(r=>r[key]??'0')),decimals,signed.has(key))}else if(index===0)td.textContent=label});
        };
        const appendRow = row => {
            const tr=table.tBodies[0].insertRow();tr.tabIndex=0;tr.classList.toggle('selected',selected===row);
            const select=()=>{selected=row;table.tBodies[0].querySelectorAll('.selected').forEach(r=>r.classList.remove('selected'));tr.classList.add('selected')};
            tr.onclick=select;
            tr.onkeydown=e=>{if(e.key==='ArrowDown'||e.key==='ArrowUp'){e.preventDefault();let next=e.key==='ArrowDown'?tr.nextElementSibling:tr.previousElementSibling;while(next&&next.tabIndex!==0)next=e.key==='ArrowDown'?next.nextElementSibling:next.previousElementSibling;if(next){next.focus();next.click()}}else if(e.ctrlKey&&e.code==='Space'){e.preventDefault();location.href=ledger(row)}};
            columns.forEach(([key])=>{const td=tr.insertCell();if(key==='AccountCode'){const a=document.createElement('a');a.textContent=row[key]??'';a.href=ledger(row);td.append(a)}else if(numeric.has(key)){td.className='num';td.textContent=ReportDecimal.format(row[key]??'0',decimals,signed.has(key))}else if(key==='LastBillDate')td.textContent=Number(row.LastBillsAmount)>0?date(row[key]):'';else if(key==='LastRcvdDate')td.textContent=Number(row.LastRcvdAmount)>0?date(row[key]):'';else td.textContent=row[key]??''});
        };
        if(groupedBy&&columns.some(c=>c[0]===groupedBy)) {
            const groups=new Map();visible.forEach(r=>{const key=String(r[groupedBy]??'');if(!groups.has(key))groups.set(key,[]);groups.get(key).push(r)});
            groups.forEach((records,key)=>{const tr=table.tBodies[0].insertRow();const td=tr.insertCell();td.colSpan=columns.length;td.style.background='#eee';td.textContent=key;records.forEach(appendRow);totals(table.tBodies[0],records,'Subtotal')});
        } else visible.forEach(appendRow);
        totals(footer,visible,'Total');status(visible.length+' records');
    }
    return {render};
})();
