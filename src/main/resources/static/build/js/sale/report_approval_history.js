(() => {
  'use strict';
  let dialog, table, message, reload, runRequest;
  const text=value=>value==null?'':typeof value==='boolean'?(value?'True':'False'):String(value);
  function date(value){if(!value)return '';const raw=text(value),d=new Date(raw);return Number.isNaN(d.getTime())?raw:d.toLocaleDateString('en-GB');}
  function create(){
    dialog=document.createElement('dialog');dialog.className='approval-history';dialog.setAttribute('aria-label','Approval History');
    const heading=document.createElement('h2');heading.textContent='Approval History';
    const toolbar=document.createElement('div');toolbar.className='toolbar';
    const refresh=document.createElement('button');refresh.type='button';refresh.textContent='Refresh';
    const close=document.createElement('button');close.type='button';close.textContent='Close';close.addEventListener('click',()=>dialog.close());
    refresh.addEventListener('click',()=>runRequest(load));toolbar.append(refresh,close);
    message=document.createElement('div');message.className='approval-message';message.setAttribute('role','status');
    const grid=document.createElement('div');grid.className='grid';table=document.createElement('table');table.createTHead();table.createTBody();grid.append(table);
    dialog.append(heading,toolbar,message,grid);document.body.append(dialog);
    dialog.addEventListener('keydown',event=>{if(event.ctrlKey&&event.key.toLowerCase()==='n'){event.preventDefault();runRequest(load);}if(event.ctrlKey&&event.key.toLowerCase()==='e'){event.preventDefault();dialog.close();}});
  }
  async function load(){
    const buttons=Array.from(dialog.querySelectorAll('button')).map(button=>[button,button.disabled]);buttons.forEach(([button])=>{button.disabled=true;button.setAttribute('aria-busy','true');});
    message.textContent='Loading approval history...';table.tHead.replaceChildren();table.tBodies[0].replaceChildren();
    try {
      const data=await reload(),keys=data.columns.filter(key=>key!=='Id'),head=document.createElement('tr');
      const widths={DocumentName:130,UserName:130,UserComments:200,ApprovalDate:100,RejectedDate:100,IsFinalApprover:120,IsMandatory:100};
      for(const key of keys){const th=document.createElement('th');th.textContent=key.replace(/_/g,' ').replace(/([a-z])([A-Z])/g,'$1 $2');if(widths[key])th.style.minWidth=widths[key]+'px';head.append(th);}table.tHead.append(head);
      for(const row of data.rows){const tr=document.createElement('tr');for(const key of keys){const td=document.createElement('td');td.textContent=['ApprovalDate','RejectedDate'].includes(key)?date(row[key]):text(row[key]);
        if(key==='Approved_Status'&&row[key]==='Approved')td.style.color='darkgreen';if(key==='Rejected_Status'&&row[key]==='Rejected')td.style.color='red';if(key==='UserComments')td.className='approval-comments';tr.append(td);}table.tBodies[0].append(tr);}
      message.textContent=data.rows.length?`${data.rows.length} records`:'No approval history found.';
    }catch(error){message.textContent=error.message||text(error);throw error;}
    finally{buttons.forEach(([button,disabled])=>{button.disabled=disabled;button.removeAttribute('aria-busy');});}
  }
  window.SaleApprovalHistory={async open(fetchRows,run){if(!dialog)create();reload=fetchRows;runRequest=run;if(!dialog.open)dialog.showModal();await load();}};
})();
