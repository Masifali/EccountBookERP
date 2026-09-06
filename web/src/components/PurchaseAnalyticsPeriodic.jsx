import React, { useState } from 'react';
import { 
  FileSpreadsheet, Download, Printer, RefreshCw, X, Calendar, Filter, DollarSign, Package, TrendingUp 
} from 'lucide-react';

export default function PurchaseAnalyticsPeriodic({ onClose, screenTitle }) {
  const [fromDate, setFromDate] = useState('2026-04-01');
  const [toDate, setToDate] = useState('2026-09-03');
  const [branch, setBranch] = useState('All Branches');

  const reportData = [
    { id: 'PO-2026-001', date: '2026-08-15', supplier: 'Al-Madina Rice Traders', item: 'Super Kernel Basmati Paddy', qty: '1,200 Bags', rate: '3,450', total: '4,140,000', status: 'Completed' },
    { id: 'PO-2026-002', date: '2026-08-18', supplier: 'Kisan Agri Inputs', item: 'IRRI-6 Rice Paddy', qty: '1,500 Bags', rate: '2,800', total: '4,200,000', status: 'Completed' },
    { id: 'PO-2026-003', date: '2026-08-22', supplier: 'Punjab Grain Corporation', item: 'PK-386 Basmati Paddy', qty: '800 Bags', rate: '3,200', total: '2,560,000', status: 'Completed' },
    { id: 'PO-2026-004', date: '2026-08-28', supplier: 'Tariq & Sons Rice Mill', item: 'Super Kernel Basmati Raw', qty: '600 Bags', rate: '3,900', total: '2,340,000', status: 'Pending Delivery' },
    { id: 'PO-2026-005', date: '2026-09-01', supplier: 'United Milling Co.', item: 'Steam Basmati Paddy', qty: '400 Bags', rate: '4,025', total: '1,610,000', status: 'Completed' }
  ];

  const exportCSV = () => {
    const csvContent = "data:text/csv;charset=utf-8," 
      + ["Order No,Date,Supplier,Item,Quantity,Rate (PKR),Total (PKR),Status"]
      .concat(reportData.map(e => `${e.id},${e.date},${e.supplier},${e.item},${e.qty},${e.rate},${e.total},${e.status}`))
      .join("\n");
    const encodedUri = encodeURI(csvContent);
    const link = document.createElement("a");
    link.setAttribute("href", encodedUri);
    link.setAttribute("download", `${screenTitle || 'Purchase_Analytics'}_Report.csv`);
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  };

  return (
    <div className="modal-overlay">
      <div className="modal-window">
        <div className="modal-header">
          <div style={{ display: 'flex', alignItems: 'center', gap: '10px' }}>
            <FileSpreadsheet size={20} />
            <span>{screenTitle || 'Purchase Analytics (Periodic)'}</span>
          </div>
          <button className="modal-close-btn" onClick={onClose} title="Close Window">
            <X size={20} />
          </button>
        </div>

        <div className="modal-body">
          {/* Controls Filter Bar */}
          <div style={{ display: 'flex', flexWrap: 'wrap', gap: '14px', alignItems: 'center', justifyContent: 'space-between', background: '#f8fafc', padding: '14px', borderRadius: '6px', border: '1px solid #e2e8f0' }}>
            <div style={{ display: 'flex', gap: '12px', alignItems: 'center', flexWrap: 'wrap' }}>
              <div style={{ display: 'flex', alignItems: 'center', gap: '6px', fontSize: '13px', fontWeight: 600 }}>
                <Calendar size={16} style={{ color: '#0a6e6e' }} />
                <span>From:</span>
                <input type="date" value={fromDate} onChange={(e) => setFromDate(e.target.value)} className="form-input" style={{ width: '140px', padding: '4px 8px' }} />
              </div>

              <div style={{ display: 'flex', alignItems: 'center', gap: '6px', fontSize: '13px', fontWeight: 600 }}>
                <span>To:</span>
                <input type="date" value={toDate} onChange={(e) => setToDate(e.target.value)} className="form-input" style={{ width: '140px', padding: '4px 8px' }} />
              </div>

              <div style={{ display: 'flex', alignItems: 'center', gap: '6px', fontSize: '13px', fontWeight: 600 }}>
                <Filter size={16} style={{ color: '#0a6e6e' }} />
                <span>Branch:</span>
                <select value={branch} onChange={(e) => setBranch(e.target.value)} className="form-select" style={{ width: '160px', padding: '4px 8px' }}>
                  <option value="All Branches">All Branches</option>
                  <option value="Main Plant - Gujranwala">Main Plant - Gujranwala</option>
                  <option value="Rice Mill Unit 2 - Kamoke">Rice Mill Unit 2 - Kamoke</option>
                </select>
              </div>
            </div>

            <div style={{ display: 'flex', gap: '8px' }}>
              <button onClick={exportCSV} className="btn-primary" style={{ width: 'auto', padding: '6px 14px', display: 'flex', alignItems: 'center', gap: '6px', fontSize: '13px' }}>
                <Download size={15} /> Export CSV
              </button>
              <button onClick={() => window.print()} style={{ background: '#64748b', color: 'white', border: 'none', padding: '6px 14px', borderRadius: '4px', cursor: 'pointer', fontSize: '13px', fontWeight: 600, display: 'flex', alignItems: 'center', gap: '6px' }}>
                <Printer size={15} /> Print
              </button>
            </div>
          </div>

          {/* Summary KPI Cards */}
          <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(200px, 1fr))', gap: '14px', marginTop: '16px' }}>
            <div style={{ background: '#f0fdf4', border: '1px solid #bbf7d0', borderRadius: '6px', padding: '14px' }}>
              <div style={{ fontSize: '12px', color: '#166534', fontWeight: 600 }}>Total Purchase Amount</div>
              <div style={{ fontSize: '20px', fontWeight: 800, color: '#15803d', marginTop: '4px' }}>Rs. 14,850,000</div>
            </div>

            <div style={{ background: '#eff6ff', border: '1px solid #bfdbfe', borderRadius: '6px', padding: '14px' }}>
              <div style={{ fontSize: '12px', color: '#1e40af', fontWeight: 600 }}>Total Paddy Quantity</div>
              <div style={{ fontSize: '20px', fontWeight: 800, color: '#1d4ed8', marginTop: '4px' }}>4,500 Bags</div>
            </div>

            <div style={{ background: '#fefce8', border: '1px solid #fef08a', borderRadius: '6px', padding: '14px' }}>
              <div style={{ fontSize: '12px', color: '#854d0e', fontWeight: 600 }}>Average Purchase Rate</div>
              <div style={{ fontSize: '20px', fontWeight: 800, color: '#a16207', marginTop: '4px' }}>Rs. 3,300 / Bag</div>
            </div>
          </div>

          {/* Data Table */}
          <div className="data-table-container">
            <table className="data-table">
              <thead>
                <tr>
                  <th>Order No</th>
                  <th>Date</th>
                  <th>Supplier Name</th>
                  <th>Item / Product</th>
                  <th>Quantity</th>
                  <th>Rate (PKR)</th>
                  <th>Total Amount</th>
                  <th>Status</th>
                </tr>
              </thead>
              <tbody>
                {reportData.map((row) => (
                  <tr key={row.id}>
                    <td style={{ fontWeight: 700, color: '#0a6e6e' }}>{row.id}</td>
                    <td>{row.date}</td>
                    <td style={{ fontWeight: 600 }}>{row.supplier}</td>
                    <td>{row.item}</td>
                    <td>{row.qty}</td>
                    <td>{row.rate}</td>
                    <td style={{ fontWeight: 700 }}>Rs. {row.total}</td>
                    <td>
                      <span style={{
                        padding: '3px 8px',
                        borderRadius: '12px',
                        fontSize: '11px',
                        fontWeight: 700,
                        backgroundColor: row.status === 'Completed' ? '#dcfce7' : '#fef3c7',
                        color: row.status === 'Completed' ? '#15803d' : '#b45309'
                      }}>
                        {row.status}
                      </span>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  );
}
