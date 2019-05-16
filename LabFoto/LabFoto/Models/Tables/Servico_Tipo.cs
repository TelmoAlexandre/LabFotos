using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LabFoto.Models.Tables
{
    public class Servico_Tipo
    {
    
        //Chave Forasteira para Tipos de Servicos
        [ForeignKey("Tipo")]
        [Display(Name = "Tipo")]
        public int TipoFK { get; set; }
        public virtual Tipo Tipo { get; set; }

        //Chave Forasteira para Pedidos de Servico
        [ForeignKey("Servico")]
        [Display(Name = "Serviço")]
        public int ServicoFK { get; set; }
        public virtual Servico Servico { get; set; }
    }
}
