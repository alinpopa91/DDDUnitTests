using ProArch.CodingTest.Abstract;
using System.Collections.Generic;
using System.Linq;

namespace ProArch.CodingTest.Invoices
{
    public sealed class InvoiceRepository : IInvoiceRepository
    {
        public IQueryable<Invoice> Get()
        {
            return new List<Invoice>().AsQueryable();
        }
    }
}
