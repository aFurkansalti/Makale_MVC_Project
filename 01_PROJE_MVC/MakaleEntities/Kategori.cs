﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.CodeDom;
using System.ComponentModel;

namespace MakaleEntities
{
    [Table("Kategori")]
    public class Kategori:BaseClass
    {
        [Required,StringLength(50), DisplayName("Kategori")]
        public string Baslik { get; set; }

        [StringLength(150), DisplayName("Açıklama")]
        public string Aciklama { get; set; }

        public virtual List<Makale> Makaleler { get; set; }

        public Kategori()
        {
            Makaleler = new List<Makale>();
        }

        // Cascading yapacaksak

    }
}
