using System;
using System.Collections.Generic;
using System.Text;
using LabFoto.Models;
using LabFoto.Models.Tables;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LabFoto.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // n-m ---> Servicos-ServicosSolicitados
            modelBuilder.Entity<Servico_ServicoSolicitado>()
                .HasKey(sc => new { sc.ServicoSolicitadoFK, sc.ServicoFK });
            modelBuilder.Entity<Servico_ServicoSolicitado>()
                .HasOne(sss => sss.Servico)
                .WithMany(s => s.Servicos_ServicosSolicitados)
                .HasForeignKey(sss => sss.ServicoFK);
            modelBuilder.Entity<Servico_ServicoSolicitado>()
                .HasOne(sss => sss.ServicoSolicitado)
                .WithMany(ss => ss.ServicosSolicitados_Servicos)
                .HasForeignKey(sss => sss.ServicoSolicitadoFK);

            // n-m ---> Servicos_DatasExecucao
            modelBuilder.Entity<Servico_DataExecucao>()
                .HasKey(sc => new { sc.DataExecucaoFK, sc.ServicoFK });
            modelBuilder.Entity<Servico_DataExecucao>()
                .HasOne(sde => sde.Servico)
                .WithMany(s => s.Servicos_DataExecucao)
                .HasForeignKey(sde => sde.ServicoFK);
            modelBuilder.Entity<Servico_DataExecucao>()
                .HasOne(sde => sde.DataExecucao)
                .WithMany(d => d.DatasExecucao_Servicos)
                .HasForeignKey(sde => sde.DataExecucaoFK);

            // n-m ---> Servicos_Tipos
            modelBuilder.Entity<Servico_Tipo>()
                .HasKey(sc => new { sc.TipoFK, sc.ServicoFK });
            modelBuilder.Entity<Servico_Tipo>()
                .HasOne(st => st.Servico)
                .WithMany(s => s.Servicos_Tipos)
                .HasForeignKey(st => st.ServicoFK);
            modelBuilder.Entity<Servico_Tipo>()
                .HasOne(st => st.Tipo)
                .WithMany(s => s.Tipos_Servicos)
                .HasForeignKey(st => st.TipoFK);

            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "efbd71e2-da58-467d-b5a7-38c0fdaeb8c1",
                Name = "Admin",
                NormalizedName = "Admin".ToUpper()
            });

            var hasher = new PasswordHasher<IdentityUser>();

            modelBuilder.Entity<Utilizador>().HasData(
                new Utilizador
                {
                    Id = "73a9eaf0-43f6-43a6-bf98-f0bb4e8a93b7",
                    UserName = "admin1@admin1.com",
                    NormalizedUserName = "admin1@admin1.com".ToUpper(),
                    Email = "admin1@admin1.com",
                    NormalizedEmail = "admin1@admin1.com".ToUpper(),
                    EmailConfirmed = false,
                    AccessFailedCount = 0,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    PasswordHash = hasher.HashPassword(null, "123Qwe!"),
                    SecurityStamp = string.Empty,
                    Nome = "Admin1"
                }, 
                new Utilizador
                    {
                        Id = "fcbbb3e1-e6ce-43b3-922d-f7342c59e5f1",
                        UserName = "user1@user1.com",
                        NormalizedUserName = "user1@user1.com".ToUpper(),
                        Email = "user1@user1.com",
                        NormalizedEmail = "user1@user1.com".ToUpper(),
                        EmailConfirmed = false,
                        AccessFailedCount = 0,
                        PhoneNumberConfirmed = false,
                        TwoFactorEnabled = false,
                        PasswordHash = hasher.HashPassword(null, "123Qwe!"),
                        SecurityStamp = string.Empty,
                        Nome = "User1"
                    }

            );

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "efbd71e2-da58-467d-b5a7-38c0fdaeb8c1",
                    UserId = "73a9eaf0-43f6-43a6-bf98-f0bb4e8a93b7"
                }, 
                new IdentityUserRole<string>
                {
                    RoleId = "efbd71e2-da58-467d-b5a7-38c0fdaeb8c1",
                    UserId = "fcbbb3e1-e6ce-43b3-922d-f7342c59e5f1"
                }
            );

            modelBuilder.Entity<Requerente>().HasData(
                new Requerente
                {
                    ID = 1,
                    Nome = "Fernando",
                    Email = "fernando@ipt.pt",
                    Telemovel = "123456789",
                    Responsavel = "Fernando"
                },
                new Requerente
                {
                    ID = 2,
                    Nome = "Maria",
                    Email = "maria@ipt.pt",
                    Telemovel = "987654321",
                    Responsavel = "Maria"
                },
                new Requerente
                {
                    ID = 3,
                    Nome = "João",
                    Email = "joao@ipt.pt",
                    Telemovel = "987654321",
                    Responsavel = "Maria"
                },
                new Requerente
                {
                    ID = 4,
                    Nome = "José",
                    Email = "jose@ipt.pt",
                    Telemovel = "123789456",
                    Responsavel = "José"
                },
                new Requerente
                {
                    ID = 5,
                    Nome = "Manuel",
                    Email = "manuel@ipt.pt",
                    Telemovel = "987321546",
                    Responsavel = "Manuel"
                }
            );

            modelBuilder.Entity<Servico>().HasData(
                new Servico
                {
                    ID = 1,
                    Nome = "Sanefa",
                    DataDeCriacao = new DateTime(2018, 6, 25),
                    IdentificacaoObra = "Tira larga, de tecido ou de madeira, que se dispõe transversalmente como ornato na parte superior de uma cortina.",
                    Observacoes = "Sanefa degradada na parte superior",
                    HorasEstudio = 2,
                    HorasPosProducao = 8,
                    DataEntrega = new DateTime(2018, 6, 27),
                    Total = 40,
                    RequerenteFK = 1,
                    Hide = false
                },
                new Servico
                {
                    ID = 2,
                    Nome = "Dia da árvore",
                    DataDeCriacao = new DateTime(2019, 3, 21),
                    IdentificacaoObra = "Plantação de árvores na escola.",
                    Observacoes = "Presença do presidente nas plantações.",
                    HorasEstudio = 6,
                    HorasPosProducao = 17,
                    DataEntrega = new DateTime(2019, 3, 30),
                    Total = 65,
                    RequerenteFK = 2,
                    Hide = false
                },
                new Servico
                {
                    ID = 3,
                    Nome = "Vaso talha prata dourada",
                    DataDeCriacao = new DateTime(2019, 4, 10),
                    IdentificacaoObra = "Vasos/Jarrões a parecem integrados em retábulos,tronos, mesas de altar em várias igrejas em Tomar.",
                    Observacoes = "",
                    HorasEstudio = 4,
                    HorasPosProducao = 10,
                    DataEntrega = new DateTime(2019, 4, 15),
                    Total = 45,
                    RequerenteFK = 1,
                    Hide = false
                },
                new Servico
                {
                    ID = 4,
                    Nome = "Conjunto de Cadeiras",
                    DataDeCriacao = new DateTime(2017, 9, 11),
                    IdentificacaoObra = "Seis cadeiras em madeira de cerejeira com acabamento em verniz",
                    Observacoes = "",
                    HorasEstudio = 20,
                    HorasPosProducao = 43,
                    DataEntrega = new DateTime(2017, 9, 20),
                    Total = 125,
                    RequerenteFK = 1,
                    Hide = false
                },
                new Servico
                {
                    ID = 5,
                    Nome = "Festa do Bodo",
                    DataDeCriacao = new DateTime(2018, 7, 13),
                    IdentificacaoObra = "Festa tradicional da aldeia de Azinhaga",
                    Observacoes = "Muito bom",
                    HorasEstudio = 3,
                    HorasPosProducao = 4,
                    DataEntrega = null,
                    Total = 0,
                    RequerenteFK = 3,
                    Hide = false
                },
                new Servico
                {
                    ID = 6,
                    Nome = "Feira do Cavalo, Golegã",
                    DataDeCriacao = new DateTime(2019, 11, 24),
                    IdentificacaoObra = "Fotos da tabanca do Maltez",
                    Observacoes = "Grande festa, aprovado",
                    HorasEstudio = 0,
                    HorasPosProducao = 0,
                    DataEntrega = null,
                    Total = 0,
                    RequerenteFK = 4,
                    Hide = false
                },
                new Servico
                {
                    ID = 7,
                    Nome = "Estátua do Cavalo queimado",
                    DataDeCriacao = new DateTime(2018, 1, 6),
                    IdentificacaoObra = "Um cavalo queimado no matadouro",
                    Observacoes = "",
                    HorasEstudio = 6,
                    HorasPosProducao = 2,
                    DataEntrega = new DateTime(2018, 2, 21),
                    Total = 0,
                    RequerenteFK = 2,
                    Hide = false
                },
                new Servico
                {
                    ID = 8,
                    Nome = "Exposição de Arte contemporânea",
                    DataDeCriacao = new DateTime(2019, 3, 12),
                    IdentificacaoObra = "Exposição na galeria de Sintra",
                    Observacoes = "",
                    HorasEstudio = 4,
                    HorasPosProducao = 2,
                    DataEntrega = null,
                    Total = 0,
                    RequerenteFK = 5,
                    Hide = false
                }
            );

            modelBuilder.Entity<Tipo>().HasData(
                new Tipo
                {
                    ID = 1,
                    Nome = "Académico"
                },
                new Tipo
                {
                    ID = 2,
                    Nome = "Investigação"
                },
                new Tipo
                {
                    ID = 3,
                    Nome = "Serviço Exterior"
                },
                new Tipo
                {
                    ID = 4,
                    Nome = "Pessoal"
                }
            );

            modelBuilder.Entity<Servico_Tipo>().HasData(
                new Servico_Tipo
                {
                    ServicoFK = 1,
                    TipoFK = 1
                },
                new Servico_Tipo
                {
                    ServicoFK = 2,
                    TipoFK = 2
                },
                new Servico_Tipo
                {
                    ServicoFK = 3,
                    TipoFK = 1
                },
                new Servico_Tipo
                {
                    ServicoFK = 4,
                    TipoFK = 3
                },
                new Servico_Tipo
                {
                    ServicoFK = 5,
                    TipoFK = 2
                },
                new Servico_Tipo
                {
                    ServicoFK = 6,
                    TipoFK = 1
                },
                new Servico_Tipo
                {
                    ServicoFK = 6,
                    TipoFK = 3
                },
                new Servico_Tipo
                {
                    ServicoFK = 7,
                    TipoFK = 4
                },
                new Servico_Tipo
                {
                    ServicoFK = 8,
                    TipoFK = 4
                }
            );

            modelBuilder.Entity<ServicoSolicitado>().HasData(
                new ServicoSolicitado
                {
                    ID = 1,
                    Nome = "Luz Visível"
                },
                new ServicoSolicitado
                {
                    ID = 2,
                    Nome = "Luz U.V"
                },
                new ServicoSolicitado
                {
                    ID = 3,
                    Nome = "Rasante"
                },
                new ServicoSolicitado
                {
                    ID = 4,
                    Nome = "Infra-red"
                },
                new ServicoSolicitado
                {
                    ID = 5,
                    Nome = "Luz Trasmitida"
                }
            );

            modelBuilder.Entity<Servico_ServicoSolicitado>().HasData(
                new Servico_ServicoSolicitado
                {
                    ServicoFK = 1,
                    ServicoSolicitadoFK = 3,
                },
                new Servico_ServicoSolicitado
                {
                    ServicoFK = 1,
                    ServicoSolicitadoFK = 2,
                },
                new Servico_ServicoSolicitado
                {
                    ServicoFK = 2,
                    ServicoSolicitadoFK = 4,
                },
                new Servico_ServicoSolicitado
                {
                    ServicoFK = 2,
                    ServicoSolicitadoFK = 1,
                },
                new Servico_ServicoSolicitado
                {
                    ServicoFK = 3,
                    ServicoSolicitadoFK = 5,
                },
                new Servico_ServicoSolicitado
                {
                    ServicoFK = 4,
                    ServicoSolicitadoFK = 1,
                },
                new Servico_ServicoSolicitado
                {
                    ServicoFK = 4,
                    ServicoSolicitadoFK = 2,
                },
                new Servico_ServicoSolicitado
                {
                    ServicoFK = 4,
                    ServicoSolicitadoFK = 3,
                },
                new Servico_ServicoSolicitado
                {
                    ServicoFK = 4,
                    ServicoSolicitadoFK = 4,
                },
                new Servico_ServicoSolicitado
                {
                    ServicoFK = 5,
                    ServicoSolicitadoFK = 2,
                },
                new Servico_ServicoSolicitado
                {
                    ServicoFK = 5,
                    ServicoSolicitadoFK = 5,
                },
                new Servico_ServicoSolicitado
                {
                    ServicoFK = 6,
                    ServicoSolicitadoFK = 2,
                },
                new Servico_ServicoSolicitado
                {
                    ServicoFK = 6,
                    ServicoSolicitadoFK = 4,
                },
                new Servico_ServicoSolicitado
                {
                    ServicoFK = 7,
                    ServicoSolicitadoFK = 5,
                },
                new Servico_ServicoSolicitado
                {
                    ServicoFK = 7,
                    ServicoSolicitadoFK = 2,
                },
                new Servico_ServicoSolicitado
                {
                    ServicoFK = 7,
                    ServicoSolicitadoFK = 3,
                },
                new Servico_ServicoSolicitado
                {
                    ServicoFK = 8,
                    ServicoSolicitadoFK = 1,
                },
                new Servico_ServicoSolicitado
                {
                    ServicoFK = 8,
                    ServicoSolicitadoFK = 3,
                }
            );

            modelBuilder.Entity<DataExecucao>().HasData(
                new DataExecucao
                {
                    ID = 1,
                    Data = new DateTime(2019, 6, 12)
                },
                new DataExecucao
                {
                    ID = 2,
                    Data = new DateTime(2019, 2, 24)
                },
                new DataExecucao
                {
                    ID = 3,
                    Data = new DateTime(2018, 9, 16)
                },
                new DataExecucao
                {
                    ID = 4,
                    Data = new DateTime(2019, 5, 7)
                },
                new DataExecucao
                {
                    ID = 5,
                    Data = new DateTime(2019, 8, 30)
                },
                new DataExecucao
                {
                    ID = 6,
                    Data = new DateTime(2018, 2, 27)
                },
                new DataExecucao
                {
                    ID = 7,
                    Data = new DateTime(2018, 1, 11)
                },
                new DataExecucao
                {
                    ID = 8,
                    Data = new DateTime(2019, 12, 15)
                },
                new DataExecucao
                {
                    ID = 9,
                    Data = new DateTime(2018, 3, 4)
                },
                new DataExecucao
                {
                    ID = 10,
                    Data = new DateTime(2018, 7, 21)
                },
                new DataExecucao
                {
                    ID = 11,
                    Data = new DateTime(2019, 8, 1)
                }
            );

            modelBuilder.Entity<Servico_DataExecucao>().HasData(
                new Servico_DataExecucao {
                    ServicoFK = 1,
                    DataExecucaoFK = 1
                },
                new Servico_DataExecucao
                {
                    ServicoFK = 2,
                    DataExecucaoFK = 2
                },
                new Servico_DataExecucao
                {
                    ServicoFK = 3,
                    DataExecucaoFK = 3
                },
                new Servico_DataExecucao
                {
                    ServicoFK = 4,
                    DataExecucaoFK = 4
                },
                new Servico_DataExecucao
                {
                    ServicoFK = 5,
                    DataExecucaoFK = 5
                },
                new Servico_DataExecucao
                {
                    ServicoFK = 6,
                    DataExecucaoFK = 6
                },
                new Servico_DataExecucao
                {
                    ServicoFK = 7,
                    DataExecucaoFK = 7
                },
                new Servico_DataExecucao
                {
                    ServicoFK = 8,
                    DataExecucaoFK = 8
                },
                new Servico_DataExecucao
                {
                    ServicoFK = 3,
                    DataExecucaoFK = 9
                },
                new Servico_DataExecucao
                {
                    ServicoFK = 3,
                    DataExecucaoFK = 10
                },
                new Servico_DataExecucao
                {
                    ServicoFK = 3,
                    DataExecucaoFK = 11
                },
                new Servico_DataExecucao
                {
                    ServicoFK = 8,
                    DataExecucaoFK = 11
                },
                new Servico_DataExecucao
                {
                    ServicoFK = 8,
                    DataExecucaoFK = 10
                },
                new Servico_DataExecucao
                {
                    ServicoFK = 7,
                    DataExecucaoFK = 5
                },
                new Servico_DataExecucao
                {
                    ServicoFK = 7,
                    DataExecucaoFK = 2
                },
                new Servico_DataExecucao
                {
                    ServicoFK = 5,
                    DataExecucaoFK = 8
                }
            );
        }

        public DbSet<Utilizador> Utilizador { get; set; }

        public DbSet<DataExecucao> DataExecucao { get; set; }

        public DbSet<Servico> Servicos { get; set; }

        public DbSet<Servico_DataExecucao> Servicos_DatasExecucao { get; set; }

        public DbSet<Servico_ServicoSolicitado> Servicos_ServicosSolicitados { get; set; }

        public DbSet<Requerente> Requerentes { get; set; }

        public DbSet<ServicoSolicitado> ServicosSolicitados { get; set; }

        public DbSet<Tipo> Tipos { get; set; }

        public DbSet<Servico_Tipo> Servicos_Tipos { get; set; }
    }
}
