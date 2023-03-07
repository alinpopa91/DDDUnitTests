using ProArch.CodingTest.Abstract;

namespace ProArch.CodingTest.Suppliers
{
    public sealed class SupplierService : ISupplierService
    {
        public Supplier GetById(int id)
        {
            return new Supplier();
        }
    }
}
