package com.mst.services.sale.pcc;

/** .NET double.ToString() used in the CGS remarks of the pcc DAL (delegates to the shared "G15" formatter). */
final class SalePccInvoiceFinancialFormat {
    private SalePccInvoiceFinancialFormat() { }
    static String g(double v) { return com.mst.services.SaleInvoiceFinancialDirect.g(v); }
}
