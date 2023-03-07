using ProArch.CodingTest.External;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProArch.CodingTest.Abstract
{
    public interface IExternalInvoiceProviderService
    {
        ExternalInvoice[] GetInvoices(string supplierId);
    }
}
