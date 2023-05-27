using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaleEntities.viewmodel
{
    public class loginModel
    {
        [DisplayName("Kullanıcı Adınız"),Required(ErrorMessage ="Kullanıcı Adı boş geçilemez"), StringLength(30)]
        public string KullaniciAdi { get; set; }

        [DisplayName("Şifreniz"),Required(ErrorMessage = "Şifre boş geçilemez"), StringLength(30)]
        public string Sifre { get; set; }
    }
}
