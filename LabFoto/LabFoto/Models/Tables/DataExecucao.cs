using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LabFoto.Models.Tables
{
    public class DataExecucao
    {

        [Key]
        public int ID { get; set; }

        [DataType(DataType.Date)]
        public DateTime Data { get; set; }

        //Ligacao 1-N com os PedidosServicos_DataExecucaoServicos
        public virtual ICollection<Servico_DataExecucao> DatasExecucao_Servicos { get; set; }
    }
}
