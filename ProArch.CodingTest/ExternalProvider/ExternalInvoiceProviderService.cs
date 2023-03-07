using ProArch.CodingTest.Abstract;
using ProArch.CodingTest.External;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProArch.CodingTest.ExternalProvider
{
    public sealed class ExternalInvoiceProviderService : IExternalInvoiceProviderService
    {
        public ExternalInvoice[] GetInvoices(string supplierId)
        {
            return ExternalInvoiceService.GetInvoices(supplierId);
        }
    }
}
