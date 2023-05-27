using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaleEntities.viewmodel
{
    public class RegisterModel
    {
        [DisplayName("Kullanıcı Adınız"), Required(ErrorMessage = "Kullanıcı Adı boş geçilemez"), StringLength(30)]
        public string KullaniciAdi { get; set; }

        [DisplayName("E-postanız"), StringLength(30),EmailAddress(ErrorMessage ="{0} için geçerli bir Eposta adresi giriniz")]
        public string Email { get; set; }

        [DisplayName("Şifreniz"), Required(ErrorMessage = "Şifre boş geçilemez"), StringLength(20)]
        public string Sifre { get; set; }

        [DisplayName("Şifren tekrar"), StringLength(20),Compare(nameof(Sifre)), Required(ErrorMessage = "{0} ile Şifreniz uyuşmadı")]
        public string Sifre2 { get; set; }
    }
}
