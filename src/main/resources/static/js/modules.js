/**
 * Modules Navigation Grid GUI Controller
 */
document.addEventListener('DOMContentLoaded', () => {
    initClock();
    loadModulesGrid();
});

// Live Digital Clock
function initClock() {
    function updateTime() {
        const now = new Date();
        const timeStr = now.toLocaleTimeString([], { 
            hour: '2-digit', 
            minute: '2-digit', 
            second: '2-digit', 
            hour12: true 
        }).toLowerCase();
        const clockEl = document.getElementById('liveClock');
        if (clockEl) clockEl.textContent = timeStr;
    }
    updateTime();
    setInterval(updateTime, 1000);
}

// 14 ERP Modules List matching screenshot
const erpModulesList = [
    { id: 'dashboard', title: 'DashBoard', count: '' },
    { id: 'accounts', title: 'Accounts', count: '14', url: '/accounts/dashboard' },
    { id: 'purchase', title: 'Purchase', count: '12', url: '/accounts/dashboard?module=Purchase' },
    { id: 'quality', title: 'Quality Control', count: '8', url: '/accounts/dashboard?module=Quality' },
    { id: 'wages', title: 'Contractor Wages', count: '5', url: '/accounts/dashboard?module=Wages' },
    { id: 'sale', title: 'Sale', count: '15', url: '/accounts/dashboard?module=Sale' },
    { id: 'production', title: 'Production', count: '10', url: '/accounts/dashboard?module=Production' },
    { id: 'store', title: 'Store Management', count: '9', url: '/accounts/dashboard?module=Store' },
    { id: 'inventory', title: 'Inventory', count: '18', url: '/accounts/dashboard?module=Inventory' },
    { id: 'taxation', title: 'Taxation', count: '6', url: '/accounts/dashboard?module=Taxation' },
    { id: 'weighbridge', title: 'Weigh Bridge', count: '4', url: '/accounts/dashboard?module=WeighBridge' },
    { id: 'masterdata', title: 'Master Data Definition', count: '11', url: '/accounts/dashboard?module=MasterData' },
    { id: 'commission', title: 'Commission Trading', count: '7', url: '/accounts/dashboard?module=Commission' },
    { id: 'packing', title: 'Packing Material', count: '6', url: '/accounts/dashboard?module=Packing' }
];

// Load 14 Modules Grid
function loadModulesGrid() {
    const container = document.getElementById('modulesGridContainer');
    if (!container) return;

    container.innerHTML = erpModulesList.map(mod => `
        <div class="module-card" onclick="onModuleClick('${mod.title}', '${mod.url || '/accounts/dashboard'}')">
            <div class="module-card-banner">${mod.count}</div>
            <div class="module-card-body">
                <svg class="module-icon" fill="none" stroke="currentColor" stroke-width="1.8" viewBox="0 0 24 24">
                    <path d="M3 21h18M3 7v14M21 7v14M6 11h2M6 15h2M11 11h2M11 15h2M16 11h2M16 15h2M4 7l8-4 8 4"/>
                </svg>
                <div class="module-title">${mod.title}</div>
            </div>
        </div>
    `).join('');
}

// Module Click Handler
function onModuleClick(title, url) {
    if (title === 'Accounts') {
        window.location.href = '/accounts/dashboard';
    } else {
        window.location.href = url || '/accounts/dashboard';
    }
}
