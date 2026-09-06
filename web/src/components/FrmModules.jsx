import React from 'react';
import { 
  Building2, Wallet, ShoppingCart, ShieldCheck, Users, 
  TrendingUp, Factory, Warehouse, PackageCheck, Receipt, 
  Scale, Database, Briefcase, Boxes 
} from 'lucide-react';

export default function FrmModules({ onSelectModule }) {
  const modulesList = [
    { id: 'dashboard', title: 'DashBoard', count: '', icon: Building2 },
    { id: 'accounts', title: 'Accounts', count: '14', icon: Wallet },
    { id: 'purchase', title: 'Purchase', count: '12', icon: ShoppingCart },
    { id: 'quality', title: 'Quality Control', count: '8', icon: ShieldCheck },
    { id: 'wages', title: 'Contractor Wages', count: '5', icon: Users },
    { id: 'sale', title: 'Sale', count: '15', icon: TrendingUp },
    { id: 'production', title: 'Production', count: '10', icon: Factory },
    { id: 'store', title: 'Store Management', count: '9', icon: Warehouse },
    { id: 'inventory', title: 'Inventory', count: '18', icon: PackageCheck },
    { id: 'taxation', title: 'Taxation', count: '6', icon: Receipt },
    { id: 'weighbridge', title: 'Weigh Bridge', count: '4', icon: Scale },
    { id: 'masterdata', title: 'Master Data Definition', count: '11', icon: Database },
    { id: 'commission', title: 'Commission Trading', count: '7', icon: Briefcase },
    { id: 'packing', title: 'Packing Material', count: '6', icon: Boxes }
  ];

  return (
    <div className="modules-view">
      <div className="dashboard-header-panel">
        <div>
          <div className="screen-breadcrumbs">GOLDEN ACE FOODS</div>
          <h1 className="dashboard-title">DashBoard</h1>
        </div>
      </div>

      <div className="modules-grid-container">
        {modulesList.map((item) => {
          const IconComp = item.icon;
          return (
            <div 
              key={item.id} 
              className="module-card"
              onClick={() => onSelectModule && onSelectModule(item)}
            >
              <div className="module-card-header">
                {item.count ? item.count : ''}
              </div>
              <div className="module-card-body">
                <IconComp size={42} className="module-card-icon" />
                <div className="module-card-title">{item.title}</div>
              </div>
            </div>
          );
        })}
      </div>
    </div>
  );
}
