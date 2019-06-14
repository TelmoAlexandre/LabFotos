using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LabFoto.Models.Tables
{
    public class Servico_DataExecucao
    {

        //Chave Forasteira para Datas de Execucao
        [ForeignKey("DataExecucao")]
        [Display(Name = "Data de Execução")]
        public int DataExecucaoFK { get; set; }
        public virtual DataExecucao DataExecucao { get; set; }

        //Chave Forasteira para Pedidos de Servico
        [ForeignKey("Servico")]
        [Display(Name = "Serviço")]
        public string ServicoFK { get; set; }
        public virtual Servico Servico { get; set; }
    }
}
