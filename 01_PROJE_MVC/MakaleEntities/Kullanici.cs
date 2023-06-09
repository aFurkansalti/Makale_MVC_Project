﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace MakaleEntities
{
    [Table("Kullanici")]
    public class Kullanici:BaseClass
    {
        [StringLength(30), DisplayName("Ad")]
        public string Adi { get; set; }


        [StringLength(30), DisplayName("Soyadı")]
        public string SoyAdi { get; set; }


        [Required, StringLength(30), DisplayName("Kullanıcı Adı")]
        public string KullaniciAdi { get; set; }


        [Required, StringLength(30), DisplayName("E-posta")]
        public string Email { get; set; }


        [Required, StringLength(20), DisplayName("Şifre")]
        public string Sifre { get; set; }

        [StringLength(30), ScaffoldColumn(false)]
        public string ProfilResimDosyaAdi { get; set; } 
        public bool Aktif { get; set; }

        [Required, ScaffoldColumn(false)]
        public Guid AktifGuid { get; set;}

        public bool Admin { get; set; }


        public virtual List<Makale> Makaleler { get; set; }
        public virtual List<Yorum> Yorumlar { get; set; }
        public virtual List<Begeni> Begeniler { get; set; }
    }
}
