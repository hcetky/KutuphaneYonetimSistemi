using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KKU_Kutuphane
{
    internal class Class
    {
    }

    public class Kitap
    {
        public int Id { get; set; }
        public string KitapAdi { get; set; }
        public string Yazar { get; set; }
        public DateTime TeslimTarihi { get; set; } // TeslimTarihi'ni DateTime olarak değiştiriyoruz.
    }

}
