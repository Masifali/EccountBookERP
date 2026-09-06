import React from 'react';
import { Menu, Home, Star, Clock, BarChart3, Layers } from 'lucide-react';

export default function Sidebar({ activeView, setActiveView }) {
  return (
    <aside className="sidebar-nav">
      <button className="sidebar-btn" title="Toggle Navigation Menu">
        <Menu size={20} />
      </button>

      <button 
        className={`sidebar-btn ${activeView === 'modules' ? 'active' : ''}`}
        onClick={() => setActiveView('modules')}
        title="ERP Modules"
      >
        <div style={{ fontWeight: 800, fontSize: '18px', fontFamily: 'serif' }}>e</div>
      </button>

      <button 
        className={`sidebar-btn ${activeView === 'analytics' ? 'active' : ''}`}
        onClick={() => setActiveView('analytics')}
        title="Executive Analytics"
      >
        <BarChart3 size={20} />
      </button>

      <button 
        className={`sidebar-btn ${activeView === 'dashboard' ? 'active' : ''}`}
        onClick={() => setActiveView('dashboard')}
        title="Home Dashboard"
      >
        <Home size={20} />
      </button>

      <button 
        className={`sidebar-btn ${activeView === 'favorites' ? 'active' : ''}`}
        onClick={() => setActiveView('favorites')}
        title="Favorite Screens"
      >
        <Star size={20} />
      </button>

      <button 
        className="sidebar-btn" 
        title="Recent History Log"
      >
        <Clock size={20} />
      </button>
    </aside>
  );
}
