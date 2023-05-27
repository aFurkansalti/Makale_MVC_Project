using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaleDAL
{
    public class Singelton
    {
        public static DatabaseContext db;
        public Singelton()
        {
            if (db == null)
               db = new DatabaseContext();
        }
    }
}
