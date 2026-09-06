import React, { useState, useEffect } from 'react';
import { Search, User, Bell, ChevronDown } from 'lucide-react';

export default function HeaderBar({ user, activeCompany, financialYear, onSearchSelect }) {
  const [timeStr, setTimeStr] = useState('');

  useEffect(() => {
    const updateTime = () => {
      const now = new Date();
      setTimeStr(now.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit', second: '2-digit', hour12: true }).toLowerCase());
    };
    updateTime();
    const interval = setInterval(updateTime, 1000);
    return () => clearInterval(interval);
  }, []);

  return (
    <header className="header-bar">
      <div className="header-left">
        <select 
          className="header-search-box"
          onChange={(e) => onSearchSelect && onSearchSelect(e.target.value)}
          defaultValue=""
        >
          <option value="" disabled>...Select Any Value...</option>
          <option value="Accounts">Accounts Dashboard</option>
          <option value="Purchase Analytics (Periodic)">Purchase Analytics (Periodic)</option>
          <option value="Receivables Receipts Schedule">Receivables Receipts Schedule</option>
          <option value="Accounts Posting (Dashboard)">Accounts Posting (Dashboard)</option>
          <option value="Banking Management">Banking Management</option>
        </select>

        <div className="header-user-info">
          {user ? `${user.name} (${user.role})` : 'Numan Rasheed (Admin)'}
        </div>

        <div className="header-company-title">
          {activeCompany || 'Golden Ace Rice Mills (Pvt) Ltd.'}
        </div>
      </div>

      <div className="header-right">
        <div className="header-clock">
          {timeStr || '5:36:23 pm'}
        </div>

        <div className="header-fy-badge">
          {financialYear || '2026-27'}
        </div>

        <div className="header-version-alert">
          Current Version: 1.0.0.1388
        </div>
      </div>
    </header>
  );
}
