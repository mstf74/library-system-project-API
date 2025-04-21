using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.GenericRepo
{
    public  interface IGenericRepo<T>
    {
        IEnumerable<T> getAll();
        IEnumerable<T> getAllFilter(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IQueryable<T>> include = null);
        T getById(int id);
        bool add(T item);
        bool update(T item);
        bool remove(int id);
        EntityEntry CheckState(T item);
        public bool SaveChanges();

    }
}
