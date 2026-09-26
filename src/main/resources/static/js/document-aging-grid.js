'use strict';
window.DocumentAgingGrid = (() => {
    const collapsed = new Set(), filters = new Map();
    let sortKey='', direction=1;
    function render({table,rows,columns,numbers,decimals,search,ledger,select,status}) {
        table.tHead.replaceChildren();table.tBodies[0].replaceChildren();
        const footer=table.tFoot||table.createTFoot();footer.replaceChildren();
        const header=table.tHead.insertRow(),filterRow=table.tHead.insertRow();
        const refresh=()=>render({table,rows,columns,numbers,decimals,search,ledger,select,status});
        const amount=(value,key)=>{
            const fixed=ReportDecimal.format(value??'0',key==='DueAmount'?decimals:(decimals||2),true);
            return key==='DueAmount'?fixed:fixed.replace(/(\.\d*?)0+(\)?)$/,'$1$2').replace(/\.(\)?)$/,'$1');
        };
        columns.forEach(([key,label,width],index)=>{
            const th=document.createElement('th');th.style.width=th.style.minWidth=width+'px';th.textContent=label;
            th.tabIndex=0;const sort=()=>{direction=sortKey===key?-direction:1;sortKey=key;refresh()};th.onclick=sort;th.onkeydown=e=>{if(e.key==='Enter')sort()};header.append(th);
            const cell=document.createElement('th');cell.style.top='38px';cell.style.height='26px';
            const input=document.createElement('input');input.style.cssText='width:100%;min-width:0;height:22px';input.setAttribute('aria-label','Filter '+label);input.value=filters.get(key)||'';
            input.oninput=()=>{const caret=input.selectionStart;filters.set(key,input.value);refresh();const next=table.tHead.rows[1].cells[index].firstChild;next.focus();next.setSelectionRange(caret,caret)};cell.append(input);filterRow.append(cell);
        });
        const visible=rows.filter(r=>Object.values(r).some(v=>String(v??'').toLowerCase().includes(search.toLowerCase()))&&[...filters].every(([key,value])=>String(r[key]??'').toLowerCase().includes(value.toLowerCase())));
        if(sortKey)visible.sort((a,b)=>direction*(numbers.has(sortKey)?Number(a[sortKey]||0)-Number(b[sortKey]||0):String(a[sortKey]??'').localeCompare(String(b[sortKey]??''))));
        const totals=(section,records,label)=>{const tr=section.insertRow();tr.className='aging-total';columns.forEach(([key],index)=>{const td=tr.insertCell();if(numbers.has(key)){td.className='amount';td.textContent=amount(ReportDecimal.sum(records.map(r=>r[key]??'0')),key)}else if(index===0)td.textContent=label});return tr};
        const groups=new Map();visible.forEach(r=>{const key=String(r.PartyName??'');if(!groups.has(key))groups.set(key,[]);groups.get(key).push(r)});
        groups.forEach((records,party)=>{
            const group=table.tBodies[0].insertRow();group.className='group';const cell=group.insertCell();cell.colSpan=columns.length;
            const toggle=document.createElement('button');toggle.textContent=(collapsed.has(party)?'+ ':'− ')+party;toggle.setAttribute('aria-expanded',String(!collapsed.has(party)));toggle.onclick=()=>{if(collapsed.has(party))collapsed.delete(party);else collapsed.add(party);refresh()};cell.append(toggle);
            records.forEach(row=>{
                const tr=table.tBodies[0].insertRow();tr.tabIndex=0;tr.hidden=collapsed.has(party);
                tr.onclick=()=>{table.querySelectorAll('.selected').forEach(r=>r.classList.remove('selected'));tr.classList.add('selected');select(row)};
                tr.onkeydown=e=>{if(e.ctrlKey&&e.code==='Space'){e.preventDefault();location.href=ledger(row)}else if(e.key==='ArrowDown'||e.key==='ArrowUp'){e.preventDefault();let next=e.key==='ArrowDown'?tr.nextElementSibling:tr.previousElementSibling;while(next&&(next.tabIndex!==0||next.hidden))next=e.key==='ArrowDown'?next.nextElementSibling:next.previousElementSibling;if(next){next.focus();next.click()}}};
                columns.forEach(([key])=>{const td=tr.insertCell();if(key==='AccountCode'){const a=document.createElement('a');a.textContent=row[key]??'';a.href=ledger(row);td.append(a)}else if(numbers.has(key)){td.className='amount';td.textContent=amount(row[key],key)}else{let value=row[key]??'';if(['InvoiceDate','PartyBillDate','InvoiceDueDate'].includes(key)){const match=/^(\d{4})-(\d{2})-(\d{2})/.exec(String(value));if(match)value=match[3]+'-'+['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'][Number(match[2])-1]+'-'+match[1]}td.textContent=value;if(key==='Remarks')td.className='remarks'}});
            });
            totals(table.tBodies[0],records,'Subtotal');
        });
        totals(footer,visible,'Total');status(visible.length+' documents');
    }
    return {render};
})();
