/**
 * REST API Client for AccountBookERP Accounts Module
 */
const AccountsApi = {
    baseUrl: '/api/accounts',

    /**
     * Fetch all submodules (e.g. Account Definition, Accounts Transaction, Account Reports, Banking Management)
     */
    async getSubModules() {
        try {
            const res = await fetch(`${this.baseUrl}/submodules`);
            if (!res.ok) throw new Error('Failed to fetch submodules');
            return await res.json();
        } catch (err) {
            console.warn('Falling back to local submodule data:', err.message);
            return [
                { id: 1, title: 'Account Definition', count: 8, icon: 'bank' },
                { id: 2, title: 'Accounts Transaction', count: 18, icon: 'bank' },
                { id: 3, title: 'Account Reports', count: 27, icon: 'bank' },
                { id: 4, title: 'Banking Managment', count: 7, icon: 'bank' }
            ];
        }
    },

    /**
     * Fetch screen cards by submodule title or ID
     */
    async getScreenCards(submoduleTitle = 'Banking Managment') {
        try {
            const res = await fetch(`${this.baseUrl}/screens?submodule=${encodeURIComponent(submoduleTitle)}`);
            if (!res.ok) throw new Error('Failed to fetch screen cards');
            return await res.json();
        } catch (err) {
            console.warn('Falling back to local screen cards data:', err.message);
            return [
                { id: 'scr_1', title: 'Define Bank', targetUrl: '/accounts/bank/define', isFav: false },
                { id: 'scr_2', title: 'Bank Payment Voucher', targetUrl: '/accounts/bank/payment-voucher', isFav: false },
                { id: 'scr_3', title: 'Bank Receipt Voucher', targetUrl: '/accounts/bank/receipt-voucher', isFav: false },
                { id: 'scr_4', title: 'Bank Balance Entry', targetUrl: '/accounts/bank/balance-entry', isFav: false },
                { id: 'scr_5', title: 'Cheque Printing', targetUrl: '/accounts/bank/cheque-printing', isFav: false },
                { id: 'scr_6', title: 'Bank Balances Report', targetUrl: '/accounts/bank/balances-report', isFav: false },
                { id: 'scr_7', title: 'General Ledger Report', targetUrl: '/accounts/reports/general-ledger', isFav: false }
            ];
        }
    },

    /**
     * Save bank definition form
     */
    async saveBankDefinition(bankData) {
        try {
            const res = await fetch(`${this.baseUrl}/banks`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(bankData)
            });
            return await res.json();
        } catch (err) {
            console.warn('API offline - simulated bank definition saved:', bankData);
            return { status: 'success', message: 'Bank Definition Saved Successfully!', data: bankData };
        }
    },

    /**
     * Toggle screen favorite status
     */
    async toggleFavorite(screenId, isFavorite) {
        try {
            const res = await fetch(`${this.baseUrl}/screens/${screenId}/favorite`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ isFavorite })
            });
            return await res.json();
        } catch (err) {
            return { status: 'success', screenId, isFavorite };
        }
    }
};
