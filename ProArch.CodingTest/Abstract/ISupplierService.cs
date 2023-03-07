using ProArch.CodingTest.Suppliers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProArch.CodingTest.Abstract
{
    public interface ISupplierService
    {
        Supplier GetById(int id);
    }
}
