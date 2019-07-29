using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LabFoto.Models.Tables
{
    public class Fotografia
    {
        [Key]
        public int ID { get; set; }
        public string Nome { get; set; }
        public string Formato { get; set; }
        public string DownloadUrl { get; set; }
        public string Thumbnail_Small { get; set; }
        public string Thumbnail_Medium { get; set; }
        public string Thumbnail_Large { get; set; }
        public string ItemId { get; set; }

        //Chave Forasteira para Fotografia de origem
        [ForeignKey("Fotografia")]
        [Display(Name = "Fotografia de Origem")]
        public int FotografiaOrigemFK { get; set; }
        public Fotografia FotografiaOrigem { get; set; }

        //Chave Forasteira para ContaOnedrive
        [ForeignKey("ContaOnedrive")]
        [Display(Name = "Conta da Onedrive")]
        public int ContaOnedriveFK { get; set; }
        public ContaOnedrive ContaOnedrive { get; set; }

        //Chave Forasteira para Galeria
        [ForeignKey("Galeria")]
        public string GaleriaFK { get; set; }
        public Galeria Galeria { get; set; }
    }
}
