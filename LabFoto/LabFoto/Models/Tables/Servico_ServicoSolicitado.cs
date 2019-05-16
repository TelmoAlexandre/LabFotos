using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LabFoto.Models.Tables
{
    public class Servico_ServicoSolicitado
    {

        //Chave Forasteira para ServicosSolicitados
        [ForeignKey("ServicoSolicitado")]
        [Display(Name = "Serviço Solicitado")]
        public int ServicoSolicitadoFK { get; set; }
        public virtual ServicoSolicitado ServicoSolicitado { get; set; }

        //Chave Forasteira para Pedidos de Servico
        [ForeignKey("Servico")]
        [Display(Name = "Serviço")]
        public int ServicoFK { get; set; }
        public virtual Servico Servico { get; set; }
    }
}
