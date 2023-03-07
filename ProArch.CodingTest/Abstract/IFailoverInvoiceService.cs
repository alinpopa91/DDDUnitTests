using ProArch.CodingTest.Invoices;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProArch.CodingTest.Abstract
{
    public interface IFailoverInvoiceService
    {
        FailoverInvoiceCollection GetInvoices(int supplierId);
    }
}
