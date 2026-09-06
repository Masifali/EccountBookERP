/**
 * Accounts Dashboard GUI Controller for all 7 Banking Screens
 */
document.addEventListener('DOMContentLoaded', () => {
    initClock();
    initEventListeners();
    loadSubmodules();
    loadScreenCards('Banking Managment');
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

// Event Listeners Setup
function initEventListeners() {
    const searchSelect = document.getElementById('screenSearchSelect');
    if (searchSelect) {
        searchSelect.addEventListener('change', (e) => {
            const val = e.target.value;
            if (val) openScreenModal(val);
        });
    }
}

// Load Submodules into top cards
async function loadSubmodules() {
    const submodules = await AccountsApi.getSubModules();
    const container = document.getElementById('submodulesContainer');
    if (!container) return;

    container.innerHTML = submodules.map(sub => `
        <div class="module-card ${sub.title === 'Banking Managment' ? 'selected' : ''}" 
             data-title="${sub.title}" 
             onclick="onSubmoduleClick(this, '${sub.title}')">
            <div class="module-card-banner">${sub.count}</div>
            <div class="module-card-body">
                <svg class="module-icon" fill="none" stroke="currentColor" stroke-width="1.8" viewBox="0 0 24 24">
                    <path d="M3 21h18M3 7v14M21 7v14M6 11h2M6 15h2M11 11h2M11 15h2M16 11h2M16 15h2M4 7l8-4 8 4"/>
                </svg>
                <div class="module-title">${sub.title}</div>
            </div>
        </div>
    `).join('');
}

// Submodule Selection Click Handler
function onSubmoduleClick(cardEl, submoduleTitle) {
    document.querySelectorAll('.module-card').forEach(c => c.classList.remove('selected'));
    cardEl.classList.add('selected');
    loadScreenCards(submoduleTitle);
}

// Load Screen Cards Grid
async function loadScreenCards(submoduleTitle) {
    const cards = await AccountsApi.getScreenCards(submoduleTitle);
    const container = document.getElementById('screensGrid');
    if (!container) return;

    container.innerHTML = cards.map(card => `
        <div class="screen-card" onclick="openScreenModal('${card.title}')">
            <svg class="star-icon ${card.isFav ? 'filled' : ''}" 
                 onclick="toggleStar(event, this, '${card.id}')" 
                 fill="${card.isFav ? 'currentColor' : 'none'}" 
                 stroke="currentColor" stroke-width="1.8" viewBox="0 0 24 24">
                <path d="M12 2l3.09 6.26L22 9.27l-5 4.87 1.18 6.88L12 17.77l-6.18 3.25L7 14.14 2 9.27l6.91-1.01L12 2z"/>
            </svg>
            <div class="screen-card-title">${card.title}</div>
        </div>
    `).join('');
}

// Favorite Star Toggle
async function toggleStar(event, svgEl, screenId) {
    event.stopPropagation();
    const isCurrentlyFav = svgEl.classList.contains('filled');
    const newFavState = !isCurrentlyFav;

    if (newFavState) {
        svgEl.classList.add('filled');
        svgEl.setAttribute('fill', 'currentColor');
    } else {
        svgEl.classList.remove('filled');
        svgEl.setAttribute('fill', 'none');
    }

    await AccountsApi.toggleFavorite(screenId, newFavState);
}

// Open Modal Window with Specific Screen UI Implementation
function openScreenModal(screenTitle) {
    const titleEl = document.getElementById('modalTitle');
    const modalEl = document.getElementById('screenModal');
    const bodyEl = document.getElementById('modalBodyContent');

    if (titleEl) titleEl.textContent = screenTitle;

    if (bodyEl) {
        switch (screenTitle) {
            case 'Define Bank':
                bodyEl.innerHTML = getDefineBankForm();
                break;
            case 'Bank Payment Voucher':
                bodyEl.innerHTML = getBankPaymentVoucherForm();
                break;
            case 'Bank Receipt Voucher':
                bodyEl.innerHTML = getBankReceiptVoucherForm();
                break;
            case 'Bank Balance Entry':
                bodyEl.innerHTML = getBankBalanceEntryForm();
                break;
            case 'Cheque Printing':
                bodyEl.innerHTML = getChequePrintingForm();
                break;
            case 'Bank Balances Report':
                bodyEl.innerHTML = getBankBalancesReportView();
                break;
            case 'General Ledger Report':
                bodyEl.innerHTML = getGeneralLedgerReportView();
                break;
            default:
                bodyEl.innerHTML = getDefineBankForm();
        }
    }

    if (modalEl) modalEl.classList.add('active');
}

// Close Modal Window
function closeModal() {
    const modalEl = document.getElementById('screenModal');
    if (modalEl) modalEl.classList.remove('active');
}

// --- Screen Forms HTML Generators ---

function getDefineBankForm() {
    return `
        <form onsubmit="event.preventDefault(); alert('Define Bank Saved!'); closeModal();">
            <div class="form-grid">
                <div class="form-group">
                    <label class="form-label">Bank Code / ID</label>
                    <input type="text" class="form-input" value="BNK-2026-001" readonly>
                </div>
                <div class="form-group">
                    <label class="form-label">Bank Name</label>
                    <input type="text" class="form-input" placeholder="e.g. Meezan Bank Limited" required>
                </div>
                <div class="form-group">
                    <label class="form-label">Account Number</label>
                    <input type="text" class="form-input" placeholder="e.g. 0102010349281" required>
                </div>
                <div class="form-group">
                    <label class="form-label">Branch Name / Code</label>
                    <input type="text" class="form-input" placeholder="e.g. Main Branch - Gujranwala">
                </div>
            </div>
            <button type="submit" class="btn-submit">Save Bank Definition</button>
        </form>
    `;
}

function getBankPaymentVoucherForm() {
    return `
        <form onsubmit="event.preventDefault(); alert('Bank Payment Voucher Posted!'); closeModal();">
            <div class="form-grid">
                <div class="form-group">
                    <label class="form-label">Voucher Number</label>
                    <input type="text" class="form-input" value="BPV-2026-089" readonly>
                </div>
                <div class="form-group">
                    <label class="form-label">Voucher Date</label>
                    <input type="date" class="form-input" value="2026-09-03" required>
                </div>
                <div class="form-group">
                    <label class="form-label">Paid From Bank</label>
                    <select class="form-input">
                        <option>Meezan Bank Ltd - A/C 0102010349281</option>
                        <option>Habib Bank Ltd - A/C 4829104810231</option>
                    </select>
                </div>
                <div class="form-group">
                    <label class="form-label">Debit Account Title</label>
                    <input type="text" class="form-input" placeholder="e.g. Vendor / Supplier Account" required>
                </div>
                <div class="form-group">
                    <label class="form-label">Payment Amount (PKR)</label>
                    <input type="number" class="form-input" placeholder="0.00" required>
                </div>
                <div class="form-group">
                    <label class="form-label">Cheque / Instrument No.</label>
                    <input type="text" class="form-input" placeholder="e.g. CHQ-991823">
                </div>
            </div>
            <button type="submit" class="btn-submit">Post Payment Voucher</button>
        </form>
    `;
}

function getBankReceiptVoucherForm() {
    return `
        <form onsubmit="event.preventDefault(); alert('Bank Receipt Voucher Posted!'); closeModal();">
            <div class="form-grid">
                <div class="form-group">
                    <label class="form-label">Voucher Number</label>
                    <input type="text" class="form-input" value="BRV-2026-114" readonly>
                </div>
                <div class="form-group">
                    <label class="form-label">Receipt Date</label>
                    <input type="date" class="form-input" value="2026-09-03" required>
                </div>
                <div class="form-group">
                    <label class="form-label">Deposited To Bank</label>
                    <select class="form-input">
                        <option>Meezan Bank Ltd - A/C 0102010349281</option>
                        <option>Bank Alfalah Ltd - A/C 8829104810111</option>
                    </select>
                </div>
                <div class="form-group">
                    <label class="form-label">Credit Account (Customer/Party)</label>
                    <input type="text" class="form-input" placeholder="e.g. Al-Madina Grain Traders" required>
                </div>
                <div class="form-group">
                    <label class="form-label">Received Amount (PKR)</label>
                    <input type="number" class="form-input" placeholder="0.00" required>
                </div>
                <div class="form-group">
                    <label class="form-label">Online Ref / Pay Order No</label>
                    <input type="text" class="form-input" placeholder="e.g. FT262468192">
                </div>
            </div>
            <button type="submit" class="btn-submit">Post Receipt Voucher</button>
        </form>
    `;
}

function getBankBalanceEntryForm() {
    return `
        <form onsubmit="event.preventDefault(); alert('Opening Balance Entry Updated!'); closeModal();">
            <div class="form-grid">
                <div class="form-group">
                    <label class="form-label">Bank Account</label>
                    <select class="form-input">
                        <option>Meezan Bank Ltd (0102010349281)</option>
                        <option>Habib Bank Ltd (4829104810231)</option>
                    </select>
                </div>
                <div class="form-group">
                    <label class="form-label">As of Date</label>
                    <input type="date" class="form-input" value="2026-04-01">
                </div>
                <div class="form-group">
                    <label class="form-label">Opening Debit Balance</label>
                    <input type="number" class="form-input" value="4500000.00">
                </div>
                <div class="form-group">
                    <label class="form-label">Opening Credit Balance</label>
                    <input type="number" class="form-input" value="0.00">
                </div>
            </div>
            <button type="submit" class="btn-submit">Save Balance Entry</button>
        </form>
    `;
}

function getChequePrintingForm() {
    return `
        <form onsubmit="event.preventDefault(); alert('Cheque Sent to Printer!'); closeModal();">
            <div class="form-grid">
                <div class="form-group">
                    <label class="form-label">Select Bank</label>
                    <select class="form-input">
                        <option>Meezan Bank Ltd</option>
                        <option>Habib Bank Ltd</option>
                    </select>
                </div>
                <div class="form-group">
                    <label class="form-label">Payee Name</label>
                    <input type="text" class="form-input" placeholder="e.g. Kisan Agri Inputs Ltd." required>
                </div>
                <div class="form-group">
                    <label class="form-label">Cheque Date</label>
                    <input type="date" class="form-input" value="2026-09-03">
                </div>
                <div class="form-group">
                    <label class="form-label">Cheque Amount (PKR)</label>
                    <input type="number" class="form-input" placeholder="2500000.00" required>
                </div>
            </div>
            <button type="submit" class="btn-submit">Print Cheque</button>
        </form>
    `;
}

function getBankBalancesReportView() {
    return `
        <div style="font-size:13px;">
            <table style="width:100%; border-collapse:collapse; text-align:left; border:1px solid #cbd5e1;">
                <thead>
                    <tr style="background:#f1f5f9; border-bottom:2px solid #cbd5e1;">
                        <th style="padding:8px;">Bank Name</th>
                        <th style="padding:8px;">Account No</th>
                        <th style="padding:8px;">Branch</th>
                        <th style="padding:8px;">Current Balance (PKR)</th>
                    </tr>
                </thead>
                <tbody>
                    <tr style="border-bottom:1px solid #e2e8f0;">
                        <td style="padding:8px; font-weight:700; color:#0e6863;">Meezan Bank Ltd</td>
                        <td style="padding:8px;">0102010349281</td>
                        <td style="padding:8px;">Main Branch</td>
                        <td style="padding:8px; font-weight:700;">Rs. 14,850,000.00</td>
                    </tr>
                    <tr style="border-bottom:1px solid #e2e8f0;">
                        <td style="padding:8px; font-weight:700; color:#0e6863;">Habib Bank Ltd</td>
                        <td style="padding:8px;">4829104810231</td>
                        <td style="padding:8px;">Kamoke Branch</td>
                        <td style="padding:8px; font-weight:700;">Rs. 8,210,000.00</td>
                    </tr>
                </tbody>
            </table>
            <button onclick="closeModal()" class="btn-submit" style="margin-top:14px;">Close Report</button>
        </div>
    `;
}

function getGeneralLedgerReportView() {
    return `
        <div style="font-size:13px;">
            <div style="display:flex; justify-content:space-between; margin-bottom:12px; font-weight:600;">
                <span>Account: Bank Master Ledger</span>
                <span>Period: Apr 2026 - Sep 2026</span>
            </div>
            <table style="width:100%; border-collapse:collapse; text-align:left; border:1px solid #cbd5e1;">
                <thead>
                    <tr style="background:#f1f5f9; border-bottom:2px solid #cbd5e1;">
                        <th style="padding:8px;">Date</th>
                        <th style="padding:8px;">Voucher No</th>
                        <th style="padding:8px;">Description</th>
                        <th style="padding:8px;">Debit</th>
                        <th style="padding:8px;">Credit</th>
                        <th style="padding:8px;">Balance</th>
                    </tr>
                </thead>
                <tbody>
                    <tr style="border-bottom:1px solid #e2e8f0;">
                        <td style="padding:8px;">2026-08-15</td>
                        <td style="padding:8px; font-weight:700; color:#0e6863;">BPV-2026-001</td>
                        <td style="padding:8px;">Paddy Purchase Payment</td>
                        <td style="padding:8px;">-</td>
                        <td style="padding:8px;">4,140,000.00</td>
                        <td style="padding:8px; font-weight:700;">10,710,000.00</td>
                    </tr>
                    <tr style="border-bottom:1px solid #e2e8f0;">
                        <td style="padding:8px;">2026-08-20</td>
                        <td style="padding:8px; font-weight:700; color:#0e6863;">BRV-2026-004</td>
                        <td style="padding:8px;">Customer Receipt</td>
                        <td style="padding:8px;">4,200,000.00</td>
                        <td style="padding:8px;">-</td>
                        <td style="padding:8px; font-weight:700;">14,910,000.00</td>
                    </tr>
                </tbody>
            </table>
            <button onclick="closeModal()" class="btn-submit" style="margin-top:14px;">Close Ledger</button>
        </div>
    `;
}
