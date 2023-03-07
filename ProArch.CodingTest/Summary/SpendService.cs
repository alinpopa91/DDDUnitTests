using ProArch.CodingTest.External;
using ProArch.CodingTest.Invoices;
using ProArch.CodingTest.Suppliers;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
using ProArch.CodingTest.Abstract;

namespace ProArch.CodingTest.Summary
{
    public sealed class SpendService
    {
        private readonly ISupplierService _supplierService;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IFailoverInvoiceService _failoverInvoiceService;
        private readonly IExternalInvoiceProviderService _externalInvoiceProviderService;

        public SpendService(ISupplierService supplierService, IInvoiceRepository invoiceRepository, 
            IFailoverInvoiceService failoverInvoiceService, IExternalInvoiceProviderService externalInvoiceProviderService)
        {
            _supplierService = supplierService;
            _invoiceRepository = invoiceRepository;
            _failoverInvoiceService = failoverInvoiceService;
            _externalInvoiceProviderService = externalInvoiceProviderService;
        }

        public SpendSummary GetTotalSpend(int supplierId)
        {
            var supplier = _supplierService.GetById(supplierId);

            SpendSummary result = new SpendSummary()
            {
                Name = supplier.Name,
                Years = new List<SpendDetail>()
            };

            if (supplier.IsExternal)
            {
                ExternalInvoice[] invoices = null;
                var tryGetInvoices = TryGetExternalInvoices(supplierId);
                if (tryGetInvoices.Item2 == false)
                {
                    Task.Delay(60000);
                    tryGetInvoices = TryGetExternalInvoices(supplierId);
                }

                invoices = tryGetInvoices.Item1;
                result.Years = GenerateExternalInvoicesByYear(invoices);
            }
            else
            {
                var invoices = _invoiceRepository.Get();
                result.Years = GenerateInternalInvoicesByYear(invoices, supplierId);
            }

            return result;
        }


        private Tuple<ExternalInvoice[], bool> TryGetExternalInvoices(int supplierId)
        {
            ExternalInvoice[] result = null;
            bool receivedFromExternalService = false;
            var retryTimes = 3;

            for (int i = 0; i < retryTimes; i++)
            {
                try
                {
                    result = _externalInvoiceProviderService.GetInvoices(supplierId.ToString());
                    receivedFromExternalService = true;
                    break;
                }
                catch (Exception ex) { }
            }

            if (receivedFromExternalService == false)
            {
                var failOverData = _failoverInvoiceService.GetInvoices(supplierId);
                if (failOverData.Timestamp < DateTime.Now.AddMonths(-1))
                {
                    throw new ApplicationException("Data is too old");
                }
                result = failOverData.Invoices;
            }

            return new Tuple<ExternalInvoice[], bool>(result, receivedFromExternalService);
        }

        private List<SpendDetail> GenerateExternalInvoicesByYear(ExternalInvoice[] externalInvoices)
        {
            var result = new List<SpendDetail>();

            List<int> years = externalInvoices.Select(a => a.Year).Distinct().OrderBy(p => p).ToList();

            SpendDetail spendDetail;
            foreach (var year in years)
            {
                spendDetail = new SpendDetail
                {
                    Year = year,
                    TotalSpend = externalInvoices.Where(y => y.Year == year).Sum(a => a.TotalAmount)
                };

                result.Add(spendDetail);
            }

            return result;
        }

        private List<SpendDetail> GenerateInternalInvoicesByYear(IQueryable<Invoice> invoices, int supplierId)
        {
            var result = new List<SpendDetail>();

            List<int> years = invoices.Where(a => a.SupplierId == supplierId).Select(a => a.InvoiceDate.Year).Distinct().OrderBy(p => p).ToList();

            SpendDetail spendDetail;
            foreach (var year in years)
            {
                spendDetail = new SpendDetail
                {
                    Year = year,
                    TotalSpend = invoices
                    .Where(y => y.InvoiceDate >= new DateTime(year, 1, 1, 0 , 0, 1) 
                    && y.InvoiceDate <= new DateTime(year, 12, 31, 23, 59, 59)
                    && y.SupplierId == supplierId)
                    .Sum(a => a.Amount)
                };

                result.Add(spendDetail);
            }

            return result;
        }
    }
}
