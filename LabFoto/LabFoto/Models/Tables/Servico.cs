using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LabFoto.Models.Tables
{
    public class Servico
    {
        public Servico()
        {
            Servicos_Tipos = new Collection<Servico_Tipo>();
        }

        [Key]
        public string ID { get; set; }

        [Required(ErrorMessage = "É necessário preencher o nome do Serviço"), StringLength(255), Display(Prompt = "Nome do Serviço")]
        public string Nome { get; set; }

        [DataType(DataType.Date), Display(Name = "Data de Criação")]
        public DateTime DataDeCriacao { get; set; }

        [Required(ErrorMessage = "É necessário preencher a Identificação/Obra"), Display(Name = "Identificação/Obra", Prompt = "Identificador adicional/Obra")]
        public string IdentificacaoObra { get; set; }

        [StringLength(4096), Display(Name = "Observações", Prompt = "Observações adicionais")]
        public string Observacoes { get; set; }

        [Display(Name = "Horas de Estúdio")]
        public float? HorasEstudio { get; set; }

        [Display(Name = "Horas de Pos-Produção")]
        public float? HorasPosProducao { get; set; }

        [DataType(DataType.Date), Display(Name = "Data de Entrega")]
        public DateTime? DataEntrega { get; set; }

        public float? Total { get; set; }

        // Ligacao 1 para N com TiposServicos_PedidosServico
        [Display(Name = "Tipo de serviço")]
        public virtual ICollection<Servico_Tipo> Servicos_Tipos { get; set; }

        // Ligacao 1 para N com PedidosServicos_ServicosSolicitados
        [Display(Name = "Serviços solicitados")]
        public virtual ICollection<Servico_ServicoSolicitado> Servicos_ServicosSolicitados { get; set; }

        // Ligacao 1 para N com PedidosServicos_DataExecucaoServico
        [Display(Name = "Datas de execução")]
        public virtual ICollection<Servico_DataExecucao> Servicos_DataExecucao { get; set; }

        public virtual ICollection<Galeria> Galerias { get; set; }
        public virtual ICollection<Partilhavel> Partilhaveis { get; set; }

        //Chave Forasteira para Requerentes
        [ForeignKey("Requerente")]
        [Display(Name = "Requerente")]
        public string RequerenteFK { get; set; }
        public virtual Requerente Requerente { get; set; }
    }
}
