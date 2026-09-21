/* Hold loading through parsing, rendering and dependent requests, not merely response headers. */
window.InventoryRequest = (() => {
    async function execute(button, label, action) {
        if(button?.disabled)return;
        const original=button?.innerHTML;
        const finish=ReportLoading.begin();
        if(button)button.textContent=label;
        try { return await action(); }
        finally { if(button)button.innerHTML=original;finish(); }
    }
    async function json(url,options={}) {
        const abort=new AbortController(),timer=setTimeout(()=>abort.abort(),120000);
        try {
            const response=await fetch(url,{...options,headers:{Accept:'application/json',...options.headers},signal:abort.signal});
            if(!(response.headers.get('content-type')||'').includes('application/json')) {
                const login=response.redirected && /\/login(?:[/?#]|$)/.test(response.url);
                throw Error(login||response.status===401 ? 'Your session has expired. Please sign in again.' : `The server returned an unexpected response (${response.status}). Please retry.`);
            }
            const body=await response.json();
            if(!response.ok)throw Error(body.message||body.detail||(response.status===403?'You do not have permission for this action.':`Request failed (${response.status}).`));
            return body;
        }catch(error){if(error.name==='AbortError')throw Error('The request timed out. Please retry.');throw error;}
        finally{clearTimeout(timer);}
    }
    return {execute,json};
})();
