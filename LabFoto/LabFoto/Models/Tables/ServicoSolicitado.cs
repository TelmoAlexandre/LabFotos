using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LabFoto.Models.Tables
{
    public class ServicoSolicitado
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "É necessário preencher o nome.")]
        public string Nome { get; set; }

        //Ligacao 1-N com o PedidosServico_ServicosSolicitados
        public virtual ICollection<Servico_ServicoSolicitado> ServicosSolicitados_Servicos { get; set; }
    }
}
