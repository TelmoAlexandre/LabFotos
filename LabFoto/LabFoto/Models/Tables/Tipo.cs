using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LabFoto.Models.Tables
{
    public class Tipo
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "É necessário preencher o nome.")]
        public string Nome { get; set; }

        //Ligacao 1-N com os TiposServico_PedidosServico
        public virtual ICollection<Servico_Tipo> Tipos_Servicos { get; set; }
    }
}
