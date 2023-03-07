using ProArch.CodingTest.Abstract;

namespace ProArch.CodingTest.Invoices
{
    public sealed class FailoverInvoiceService : IFailoverInvoiceService
    {
        public FailoverInvoiceCollection GetInvoices(int supplierId)
        {
            return new FailoverInvoiceCollection();
        }
    }
}
