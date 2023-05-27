using MakaleCommon;
using MakaleEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MakaleDAL
{
    public class Repository<T> : Singelton, IRepository<T> where T : class
    {

        private DbSet<T> dbset;
        public Repository()
        {
            dbset = db.Set<T>();
        }
        public List<T> Liste()
        {
            return dbset.ToList();
        }

        public IQueryable<T> ListQueryable()
        {
            return dbset.AsQueryable();
        }

        public List<T> Liste(Expression<Func<T, bool>> kosul)
        {
            return dbset.Where(kosul).ToList();
        }

        public int Insert(T nesne)
        {
            dbset.Add(nesne);

            // baseclass tan kaıltım almışmı diye kontrol ediyoruz
            if (nesne is BaseClass)
            {
                // her kayıtta aşağıdaki işlemler tekrar edeceği için burada tek sefer de yazıyoruz o yüzden burada kullandık
                BaseClass obj = nesne as BaseClass;

                DateTime tarih = DateTime.Now;
                obj.KayitTarihi = tarih;
                obj.DegistirmeTarihi = tarih;
                obj.DegistirenKullanici = Uygulama.login;

            }

            return db.SaveChanges();
        }
        public int Delete(T nesne)
        {
            dbset.Remove(nesne);
            return db.SaveChanges();
        }

        public int Update(T nesne)
        {
            if (nesne is BaseClass)
            {               
                BaseClass obj = nesne as BaseClass;

                obj.DegistirmeTarihi = DateTime.Now;
                obj.DegistirenKullanici = Uygulama.login;

            }

            return db.SaveChanges();
        }

        // bir koşula bağlı olarak arama yapmak için
        public T Find(Expression<Func<T, bool>> kosul)
        {
            return dbset.FirstOrDefault(kosul);
        }
    }
}
