using Moq;
using NUnit.Framework;
using ProArch.CodingTest.Abstract;
using ProArch.CodingTest.Summary;
using System;
using System.Linq;

namespace ProArch.Tests
{
    public sealed class SpendServiceWithExternalSupplierUnitTest
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
        public void ExternalSupplierWithNoFailureFromExternal()
        {
            _supplierServiceMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(InvoiceData.GetSingleExternalSupplier());
            _externalInvoiceProviderServiceMock.Setup(a => a.GetInvoices(It.IsAny<string>()))
                .Returns(InvoiceData.GetOnlyExternalFromExternalProvider());

            var result = _spendService.GetTotalSpend(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Name, "Alin's EXTERNAL Shop");
            Assert.AreEqual(result.Years.Count(), 2);
            Assert.AreEqual(result.Years[0].Year, 2020);
            Assert.AreEqual(result.Years[0].TotalSpend, 10);
            Assert.AreEqual(result.Years[1].Year, 2021);
            Assert.AreEqual(result.Years[1].TotalSpend, 15);
        }

        [Test]
        public void ExternalSupplierWithFailoverFromExternal()
        {
            _supplierServiceMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(InvoiceData.GetSingleExternalSupplier());
            _externalInvoiceProviderServiceMock.Setup(a => a.GetInvoices(It.IsAny<string>()))
                .Throws(new TimeoutException());

            _failOverInvoiceServiceMock.Setup(x => x.GetInvoices(It.IsAny<int>()))
                .Returns(InvoiceData.GetFailoverInvoiceCollection());

            var result = _spendService.GetTotalSpend(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Name, "Alin's EXTERNAL Shop");
            Assert.AreEqual(result.Years.Count(), 2);
            Assert.AreEqual(result.Years[0].Year, 2020);
            Assert.AreEqual(result.Years[0].TotalSpend, 10);
            Assert.AreEqual(result.Years[1].Year, 2021);
            Assert.AreEqual(result.Years[1].TotalSpend, 15);
        }

        [Test]
        public void ExternalSupplierWithObsoleteFailoverFromExternal()
        {
            _supplierServiceMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(InvoiceData.GetSingleExternalSupplier());
            _externalInvoiceProviderServiceMock.Setup(a => a.GetInvoices(It.IsAny<string>()))
                .Throws(new TimeoutException());

            _failOverInvoiceServiceMock.Setup(x => x.GetInvoices(It.IsAny<int>()))
                .Returns(InvoiceData.GetVeryOldFailoverInvoiceCollection());

            Assert.Throws<ApplicationException>(() => _spendService.GetTotalSpend(1));
        }

    }
}
