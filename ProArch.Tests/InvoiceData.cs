using ProArch.CodingTest.External;
using ProArch.CodingTest.Invoices;
using ProArch.CodingTest.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProArch.Tests
{
    public static class InvoiceData
    {
        public static Supplier GetSingleInternalSupplier()
        {
            Supplier supplier = new Supplier
            {
                Id = 1,
                Name = "Alin's Shop",
                IsExternal = false
            };

            return supplier;
        }

        public static IQueryable<Invoice> GetOnlyInternalInvoices()
        {
            IQueryable<Invoice> invoices = new List<Invoice>
            {
                new Invoice { SupplierId = 1, InvoiceDate = new DateTime(2023, 2, 12), Amount = 150 },
                new Invoice { SupplierId = 1, InvoiceDate = new DateTime(2023, 2, 11), Amount = 250 },
                new Invoice { SupplierId = 1, InvoiceDate = new DateTime(2023, 2, 10), Amount = 450 },
            }.AsQueryable();

            return invoices;
        }

        public static IQueryable<Invoice> GetOnlyInternalInvoicesWith2Years()
        {
            IQueryable<Invoice> invoices = new List<Invoice>
            {
                new Invoice { SupplierId = 1, InvoiceDate = new DateTime(2023, 2, 12), Amount = 150 },
                new Invoice { SupplierId = 1, InvoiceDate = new DateTime(2020, 2, 11), Amount = 250 },
                new Invoice { SupplierId = 1, InvoiceDate = new DateTime(2023, 2, 10), Amount = 450 },
                new Invoice { SupplierId = 1, InvoiceDate = new DateTime(2020, 2, 10), Amount = 450 },
            }.AsQueryable();

            return invoices;
        }

        public static IQueryable<Invoice> GetOnlyInternalInvoices2Suppliers()
        {
            IQueryable<Invoice> invoices = new List<Invoice>
            {
                new Invoice { SupplierId = 1, InvoiceDate = new DateTime(2020, 2, 12), Amount = 10 },
                new Invoice { SupplierId = 1, InvoiceDate = new DateTime(2020, 2, 11), Amount = 10 },
                new Invoice { SupplierId = 1, InvoiceDate = new DateTime(2023, 2, 10), Amount = 50 },
                new Invoice { SupplierId = 1, InvoiceDate = new DateTime(2023, 2, 10), Amount = 50 },
                new Invoice { SupplierId = 2, InvoiceDate = new DateTime(2023, 2, 10), Amount = 450 },
                new Invoice { SupplierId = 2, InvoiceDate = new DateTime(2020, 2, 10), Amount = 450 },
            }.AsQueryable();

            return invoices;
        }

        public static Supplier GetSingleExternalSupplier()
        {
            Supplier supplier = new Supplier
            {
                Id = 1,
                Name = "Alin's EXTERNAL Shop",
                IsExternal = true
            };

            return supplier;
        }

        public static ExternalInvoice[] GetOnlyExternalFromExternalProvider()
        {
            ExternalInvoice[] invoices = new ExternalInvoice[]
            {
                new ExternalInvoice { Year = 2020, TotalAmount = 10 },
                new ExternalInvoice { Year = 2021, TotalAmount = 15 },
            };

            return invoices;
        }

        public static FailoverInvoiceCollection GetFailoverInvoiceCollection()
        {
            FailoverInvoiceCollection result = new FailoverInvoiceCollection();
            result.Timestamp = DateTime.Now.AddHours(-2);
            result.Invoices = GetOnlyExternalFromExternalProvider();

            return result;
        }

        public static FailoverInvoiceCollection GetVeryOldFailoverInvoiceCollection()
        {
            FailoverInvoiceCollection result = new FailoverInvoiceCollection();
            result.Timestamp = DateTime.Now.AddYears(-2);
            result.Invoices = GetOnlyExternalFromExternalProvider();

            return result;
        }
    }
}
