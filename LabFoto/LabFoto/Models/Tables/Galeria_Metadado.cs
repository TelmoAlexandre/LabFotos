using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LabFoto.Models.Tables
{
    public class Galeria_Metadado
    {

        //Chave Forasteira para ServicosSolicitados
        [ForeignKey("Metadado")]
        public int MetadadoFK { get; set; }
        public virtual Metadado Metadado { get; set; }

        //Chave Forasteira para Pedidos de Servico
        [ForeignKey("Galeria")]
        public string GaleriaFK { get; set; }
        public virtual Galeria Galeria { get; set; }
    }
}
