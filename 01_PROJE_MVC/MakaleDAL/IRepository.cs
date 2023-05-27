using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MakaleDAL
{
    public interface IRepository<T>
    {
        int Insert(T nesne);
        int Delete(T nesne);
        int Update(T nesne);
        List<T> Liste();
        IQueryable<T> ListQueryable();
        List<T> Liste(Expression<Func<T,bool>> kosul);
        T Find(Expression<Func<T, bool>> kosul);
    }
}