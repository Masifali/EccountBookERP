import React, { useState } from 'react';
import { 
  Building2, BookOpen, CreditCard, FileSpreadsheet, Landmark, Star, Search, Filter 
} from 'lucide-react';

export default function FrmMenue({ moduleData, onOpenScreen, favorites, onToggleFavorite }) {
  const [selectedSubModule, setSelectedSubModule] = useState(null);
  const [searchFilter, setSearchFilter] = useState('');

  const accountsSubModules = [
    { id: 'acc_def', title: 'Account Definition', count: '8', icon: BookOpen },
    { id: 'acc_trans', title: 'Accounts Transaction', count: '18', icon: CreditCard },
    { id: 'acc_rep', title: 'Account Reports', count: '27', icon: FileSpreadsheet },
    { id: 'acc_bank', title: 'Banking Management', count: '7', icon: Landmark }
  ];

  const defaultScreenCards = [
    { id: 'scr_1', title: 'Purchase Analytics (Periodic)', targetUrl: 'Purchase Analytics (Periodic)', category: 'Account Reports', isFav: true },
    { id: 'scr_2', title: 'Receivables Receipts Schedule', targetUrl: 'Receivables Receipts Schedule', category: 'Account Reports', isFav: true },
    { id: 'scr_3', title: 'Accounts Posting (Dashboard)', targetUrl: 'Accounts Posting (Dashboard)', category: 'Accounts Transaction', isFav: true },
    { id: 'scr_4', title: 'Purchase Analytics Report (Paddy)', targetUrl: 'Purchase Analytics Report (Paddy)', category: 'Account Reports', isFav: true },
    { id: 'scr_5', title: 'Chart of Accounts Definition', targetUrl: 'Chart of Accounts Definition', category: 'Account Definition', isFav: false },
    { id: 'scr_6', title: 'Bank Reconciliation Statement', targetUrl: 'Bank Reconciliation Statement', category: 'Banking Management', isFav: false },
    { id: 'scr_7', title: 'General Ledger Summary', targetUrl: 'General Ledger Summary', category: 'Account Reports', isFav: false },
    { id: 'scr_8', title: 'Voucher Entry System', targetUrl: 'Voucher Entry System', category: 'Accounts Transaction', isFav: false }
  ];

  const filteredScreens = defaultScreenCards.filter(scr => {
    const matchesSearch = scr.title.toLowerCase().includes(searchFilter.toLowerCase());
    const matchesSubModule = selectedSubModule ? scr.category === selectedSubModule.title : true;
    return matchesSearch && matchesSubModule;
  });

  return (
    <div className="menue-view">
      <div className="dashboard-header-panel">
        <div>
          <div className="screen-breadcrumbs">Screens</div>
          <h1 className="dashboard-title">{moduleData ? moduleData.title : 'Accounts'}</h1>
        </div>

        <div style={{ display: 'flex', gap: '10px', alignItems: 'center' }}>
          <div style={{ position: 'relative' }}>
            <Search size={16} style={{ position: 'absolute', left: '10px', top: '50%', transform: 'translateY(-50%)', color: '#94a3b8' }} />
            <input 
              type="text"
              placeholder="Search screens..."
              value={searchFilter}
              onChange={(e) => setSearchFilter(e.target.value)}
              style={{
                paddingLeft: '32px',
                paddingRight: '12px',
                paddingTop: '6px',
                paddingBottom: '6px',
                borderRadius: '4px',
                border: 'none',
                fontSize: '13px',
                width: '200px',
                outline: 'none'
              }}
            />
          </div>
          {selectedSubModule && (
            <button 
              onClick={() => setSelectedSubModule(null)}
              style={{
                background: 'rgba(255,255,255,0.2)',
                color: 'white',
                border: 'none',
                padding: '6px 12px',
                borderRadius: '4px',
                fontSize: '12px',
                cursor: 'pointer',
                fontWeight: 600
              }}
            >
              Show All
            </button>
          )}
        </div>
      </div>

      {/* Top 4 Sub-module Cards matching screenshot */}
      <div className="modules-grid-container" style={{ paddingBottom: '12px' }}>
        {accountsSubModules.map((sub) => {
          const IconComp = sub.icon;
          const isSelected = selectedSubModule?.id === sub.id;
          return (
            <div 
              key={sub.id}
              className="module-card"
              style={{
                borderColor: isSelected ? 'var(--star-gold)' : undefined,
                boxShadow: isSelected ? '0 0 0 2px var(--star-gold)' : undefined
              }}
              onClick={() => setSelectedSubModule(isSelected ? null : sub)}
            >
              <div className="module-card-header">
                {sub.count}
              </div>
              <div className="module-card-body">
                <IconComp size={40} className="module-card-icon" />
                <div className="module-card-title">{sub.title}</div>
              </div>
            </div>
          );
        })}
      </div>

      {/* Screen Cards / Favorites Grid matching screenshot */}
      <div className="screens-section">
        <div className="screens-section-title">
          {selectedSubModule ? `${selectedSubModule.title} Screens` : 'Quick Screens & Analytics'}
        </div>

        <div className="screens-grid">
          {filteredScreens.map((scr) => {
            const isFav = favorites ? favorites[scr.id] : scr.isFav;
            return (
              <div 
                key={scr.id}
                className="screen-card"
                onClick={() => onOpenScreen && onOpenScreen(scr)}
              >
                <button 
                  className={`star-favorite-btn ${isFav ? 'is-favorite' : ''}`}
                  onClick={(e) => {
                    e.stopPropagation();
                    onToggleFavorite && onToggleFavorite(scr.id);
                  }}
                  title={isFav ? "Remove from Favorites" : "Add to Favorites"}
                >
                  <Star size={18} fill={isFav ? "currentColor" : "none"} />
                </button>
                <div className="screen-card-title">
                  {scr.title}
                </div>
              </div>
            );
          })}
        </div>
      </div>
    </div>
  );
}
