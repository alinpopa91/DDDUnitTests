using Moq;
using NUnit.Framework;
using ProArch.CodingTest.Abstract;
using ProArch.CodingTest.Summary;
using System.Linq;

namespace ProArch.Tests
{
    public sealed class SpendServiceWithInternalSupplierUnitTest
    {
        SpendService _spendService;

        Mock<ISupplierService> _supplierServiceMock;
        Mock<IInvoiceRepository> _invoiceRepositoryMock;
        Mock<IFailoverInvoiceService> _failOverInvoiceServiceMock;
        Mock<IExternalInvoiceProviderService> _externalInvoiceProviderServiceMock;

        [SetUp]
        public void Setup()
        {
            _supplierServiceMock = new Mock<ISupplierService>();
            _invoiceRepositoryMock = new Mock<IInvoiceRepository>();
            _failOverInvoiceServiceMock = new Mock<IFailoverInvoiceService>();
            _externalInvoiceProviderServiceMock = new Mock<IExternalInvoiceProviderService>();

            _spendService = new SpendService(_supplierServiceMock.Object, _invoiceRepositoryMock.Object, 
                _failOverInvoiceServiceMock.Object, _externalInvoiceProviderServiceMock.Object);
        }

        [Test]
        public void InternalSupplierWith4Invoices()
        {
            _supplierServiceMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(InvoiceData.GetSingleInternalSupplier());
            _invoiceRepositoryMock.Setup(x => x.Get()).Returns(InvoiceData.GetOnlyInternalInvoices());

            var result = _spendService.GetTotalSpend(1);

            Assert.IsNotNull(result);
        }

        [Test]
        public void InternalSupplierWith4InvoicesWith2Years()
        {
            _supplierServiceMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(InvoiceData.GetSingleInternalSupplier());
            _invoiceRepositoryMock.Setup(x => x.Get()).Returns(InvoiceData.GetOnlyInternalInvoicesWith2Years());

            var result = _spendService.GetTotalSpend(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Years.Count(), 2);
            Assert.AreEqual(result.Years[0].Year, 2020);
            Assert.AreEqual(result.Years[0].TotalSpend, 700);
            Assert.AreEqual(result.Years[1].Year, 2023);
            Assert.AreEqual(result.Years[1].TotalSpend, 600);
        }

        [Test]
        public void InternalSupplierWithInvoicesFrom2Suppliers()
        {
            _supplierServiceMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(InvoiceData.GetSingleInternalSupplier());
            _invoiceRepositoryMock.Setup(x => x.Get()).Returns(InvoiceData.GetOnlyInternalInvoices2Suppliers());

            var result = _spendService.GetTotalSpend(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Years.Count(), 2);
            Assert.AreEqual(result.Years[0].Year, 2020);
            Assert.AreEqual(result.Years[0].TotalSpend, 20);
            Assert.AreEqual(result.Years[1].Year, 2023);
            Assert.AreEqual(result.Years[1].TotalSpend, 100);
        }
    }
}
