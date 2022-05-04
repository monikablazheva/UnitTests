using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer
{
    public interface IDBFiltration<T>
    {
        IEnumerable<T> Find(object[] args);

        IEnumerable<T> GroupBy(object[] args);

        IEnumerable<T> OrderBy(object[] args);

    }
}
