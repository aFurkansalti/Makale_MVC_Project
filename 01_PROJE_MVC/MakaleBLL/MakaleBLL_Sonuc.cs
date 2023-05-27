using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaleBLL
{
    public class MakaleBLL_Sonuc<T> where T : class
    {
        public T nesne { get; set; }
        public List<string> hatalar { get; set; }

        public MakaleBLL_Sonuc()
        {
            hatalar = new List<string>();
        }

    }
}
