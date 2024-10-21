using DataAcessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.GenericRepo
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        protected Context _context;
        protected DbSet<T> Dbset;
        public GenericRepo(Context context)
        {
            _context = context;
            Dbset = context.Set<T>();
        }
        public IEnumerable<T> getAll()
        {
            return Dbset.ToList();
        }
        public T getById(int id)
        {
            return Dbset.Find(id);
        }
        // add,delete,update functions return bool so it tells if the operation is done.
        public bool add(T item)
        {
            try
            {
                _context.Add(item);
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
            var added = _context.SaveChanges();
            return added > 0;
        }

        public bool update(T item)
        {
            #region enhancedUpdate
            //try
            //{
            //    var old = Dbset.Find(id);

            //    if (old == null)
            //        throw new Exception("Entity not found");
            //    _context.Entry(old).CurrentValues.SetValues(item);
            //    var check = _context.SaveChanges();
            //    return check > 0;

            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}

            #endregion            //try
            try{
                _context.Attach(item);
                _context.Entry(item).State = EntityState.Modified;
            }
            catch (Exception ex) 
            {
                throw new Exception(message: ex.Message);
            }
            var updated = _context.SaveChanges();
            return updated > 0;
        }


        public bool remove(int id)
        {
            var item = Dbset.Find(id);
            if (item != null)
            {
                _context.Remove(item);
                var deleted = _context.SaveChanges();
                return deleted > 0;
            }
            return false;
            
        }
        /*a get all function that let the filteration to accure in the server , to avoid bringing all the data in the memory,
         it can be used as : getAllFilter(//the filter: =>e.Age<30 , //if i need to use include: e=>e.Include(e.Department)),
        or i just can use it empty as getAllFilter() and it will bring all the rows
         */
        public IEnumerable<T> getAllFilter(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IQueryable<T>> include = null)
        {
            IQueryable<T> query = Dbset;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (include != null)
            {
                query = include(query);
            }
            return query.ToList();
        }
        
    }
}
