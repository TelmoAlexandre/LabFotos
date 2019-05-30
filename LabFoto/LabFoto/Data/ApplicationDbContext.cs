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

            // n-m ---> Galeria_Metadado
            modelBuilder.Entity<Galeria_Metadado>()
                .HasKey(sc => new { sc.MetadadoFK, sc.GaleriaFK });
            modelBuilder.Entity<Galeria_Metadado>()
                .HasOne(st => st.Galeria)
                .WithMany(s => s.Galerias_Metadados)
                .HasForeignKey(st => st.GaleriaFK);
            modelBuilder.Entity<Galeria_Metadado>()
                .HasOne(st => st.Metadado)
                .WithMany(s => s.Galerias_Metadados)
                .HasForeignKey(st => st.MetadadoFK);

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

            modelBuilder.Entity<Metadado>().HasData(
                new Metadado {
                    ID = 1,
                    Nome = "Metadado1"
                },
                new Metadado
                {
                    ID = 2,
                    Nome = "Metadado2"
                },
                new Metadado
                {
                    ID = 3,
                    Nome = "Metadado3"
                },
                new Metadado
                {
                    ID = 4,
                    Nome = "Metadado4"
                }
            );

            modelBuilder.Entity<Galeria>().HasData(
                new Galeria
                {
                    ID = 1,
                    Nome = "Galeria1",
                    ServicoFK = 6
                },
                new Galeria
                {
                    ID = 2,
                    Nome = "Galeria2",
                    ServicoFK = 1
                },
                new Galeria
                {
                    ID = 3,
                    Nome = "Galeria3",
                    ServicoFK = 1
                },
                new Galeria
                {
                    ID = 4,
                    Nome = "Galeria4",
                    ServicoFK = 1
                }
            );

            modelBuilder.Entity<Galeria_Metadado>().HasData(
                new Galeria_Metadado {
                    GaleriaFK = 1,
                    MetadadoFK = 1
                },
                new Galeria_Metadado
                {
                    GaleriaFK = 2,
                    MetadadoFK = 2
                },
                new Galeria_Metadado
                {
                    GaleriaFK = 3,
                    MetadadoFK = 3
                },
                new Galeria_Metadado
                {
                    GaleriaFK = 4,
                    MetadadoFK = 4
                }
            );

            modelBuilder.Entity<ContaOnedrive>().HasData(
                new ContaOnedrive {
                    ID = 1,
                    DriveId = "b!0812_G3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29-ASFmlYSqg3p9xBheG7",
                    AccessToken = "eyJ0eXAiOiJKV1QiLCJub25jZSI6IkFRQUJBQUFBQUFEQ29NcGpKWHJ4VHE5Vkc5dGUtN0ZYanFlQnJmd0RNOHRIbzdVaExaTGdSN25mcnlhbjZxQ1d3aDYxTWt1dkdWdWo0MzAtQjZzSmRfS1hpNnZqT0lBQU1iel84cGJ5VFpOeVJnMlBMVXM4TkNBQSIsImFsZyI6IlJTMjU2IiwieDV0IjoiSEJ4bDltQWU2Z3hhdkNrY29PVTJUSHNETmEwIiwia2lkIjoiSEJ4bDltQWU2Z3hhdkNrY29PVTJUSHNETmEwIn0.eyJhdWQiOiJodHRwczovL2dyYXBoLm1pY3Jvc29mdC5jb20iLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC8yMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAvIiwiaWF0IjoxNTU5MjMzNDg0LCJuYmYiOjE1NTkyMzM0ODQsImV4cCI6MTU1OTIzNzM4NCwiYWNjdCI6MCwiYWNyIjoiMSIsImFpbyI6IkFTUUEyLzhMQUFBQXNjNHl5bjlWM010SWJjUjBUZDUzZDdmcTVQQjBUU1d5QmE5bjVablJjQ289IiwiYW1yIjpbInB3ZCJdLCJhcHBfZGlzcGxheW5hbWUiOiJMYWJGb3RvIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJhcHBpZGFjciI6IjEiLCJmYW1pbHlfbmFtZSI6IkFsZXhhbmRyZSIsImdpdmVuX25hbWUiOiJUZWxtbyIsImlwYWRkciI6IjE4OC4yNTEuMjI2LjEzOCIsIm5hbWUiOiJUZWxtbyBPbGl2ZWlyYSBBbGV4YW5kcmUiLCJvaWQiOiJjNzJkZTU2Yi01ZGZlLTQzMjgtOTMwZC0wNGEzZDcyZjI1MzMiLCJvbnByZW1fc2lkIjoiUy0xLTUtMjEtMzEwMDY3Mzc0Ni00MjcwNTgzNjI2LTI0MjUyNDIwNDktMTc4MjkiLCJwbGF0ZiI6IjMiLCJwdWlkIjoiMTAwMzdGRkU5MDg5NDYzNCIsInNjcCI6IkZpbGVzLlJlYWQgRmlsZXMuUmVhZC5BbGwgRmlsZXMuUmVhZFdyaXRlIEZpbGVzLlJlYWRXcml0ZS5BbGwgcHJvZmlsZSBvcGVuaWQgZW1haWwiLCJzaWduaW5fc3RhdGUiOlsia21zaSJdLCJzdWIiOiJZdVJhQVBLRVlKeU94eXlFY21kUjJNZG5hTE85NDk1QTNkQ2dFWlQ5MzlBIiwidGlkIjoiMjFlOTBkZmMtNTRmMS00YjIxLThmM2ItN2ZiOTc5OGVkMmUwIiwidW5pcXVlX25hbWUiOiJhbHVubzE5MDg5QGlwdC5wdCIsInVwbiI6ImFsdW5vMTkwODlAaXB0LnB0IiwidXRpIjoiVV9GbXNLblI3a20tMWVPcUZOeXZBQSIsInZlciI6IjEuMCIsInhtc19zdCI6eyJzdWIiOiJld2hsazdSYmhWN25YbTc0dTVTQUlQTjcwYktpbnBMWE1OQlB3VEFTUXRZIn0sInhtc190Y2R0IjoxNDI5MjY3MjA4fQ.RxpyPNjTQaRlpAmM9BaKnU4zGoCry1_JZ-k4F-ADhlKoK-SRDdt6UhPfoysq4vxDrwCD5SSJwf93ygV2sM19etyHAKwS_ZWFDjrpyXEq0aftm_6A-LVTrI_KmjX3u64b8joyGGailTcMBc1gkk21jKt6nnoXNJuKXNXrK3mQGSI7xosG9WOmbWB54VoTCDEY0rUgIkO5_e0R9bIXURShNgiNgV5GpPhjAV1Oo3qWPEVrR41-8LkLMKqFbtLFfPouk7C4KlOB6AWb69Qew02h1KqIir_Xe3m1xiNqQ1l_bM2dYGdNgvGhoXQgge4JzGkrINOAkOp9KhuS_bRl7Cvjdw",
                    RefreshToken = "-7FXmM_2aSL25b4X-gFBQOTiVTf4ph2zQdl02erPQ3k02lPQlapDOFcjBLEcumEFhUTuy70kmHovua_emPvKZa19egLTTDt8F6IfIJ8g0hfsOk79EQenHJWd5lmcFU5Bpu2H4jXkvcfDSrbwza_HXp40bXuG-ZUE4BHwytKwk9n3X06xo6dxxuaQ_8CsW1MEXSy-nJpK9E1Wn3e1z4twuo15ejemgOFzD4_VYPMKlKHJXqzYhC5WMLk1-oA-dYaZ9g5TdEr3nJKeYYvxWnXP420nnLq8tLLR-Hu7CHoPRYeTzh-5_TWd3Os6SJXO_ucP-BETYvEoVRJVjfA8RvJz9JITqY2Rihkd1S28oe4mWyKg6PcbrbewiW7B8kGdKh2So2tPoTZw-vI_1CRV6X8m72zizO5QL6NZX2Egs6lRMMEWm3ZhvJYbFwuaR6x63ocYPbylFU1ll8x6iF2FBhVjrjVfn9B3Vj967J3WR2TB-ayYG8kGyXgHkXr1P0IRdAlLHoK93K1gRinjqXq3_qqARWta78NcJQ0q9g1fsuju6uGgVfShwMzloSZXlBQXMCPKp1Orkjs__5AJApgD5PsVPWMhTxkN3KSUiXU3liEYBuPUHi27s_jU4ZU9Id2c0-ItUjoa3S5IXcZ9W2IA-RCKgTzmeLk4Fs8_0M0-h07Yy6SnB_xq_jp7Ox-2RwQpwbmRxfmpqpCr9lY-7qF7Xer6E9GOKZrbivoqXmszEbK7H-fjeg43dpivTkH84Zwvb5natg8JhV2RBGK5YKBL2R4k04H30id9YdF34T_s-7FlEikGh7dZgv8XkVnbJbGjrfyPGZx7HfeqOGdK9Y3jjMXtcOb6SCAA",
                    Quota_Remaining = "5497452095469",
                    Quota_Total = "5497558138880",
                    Quota_Used = "97597600",
                    TokenDate = new DateTime(2019, 5, 29, 12, 0, 0)
                }    
            );

            modelBuilder.Entity<Fotografia>().HasData(
                new Fotografia
                {
                    ID = 1,
                    Nome = "test1.CR2",
                    ItemId = "0127OBJ5MUEW23YQE53RBZMCVLKSROJVLD",
                    ContaOnedriveFK = 1,
                    DownloadUrl = "https://politecnicotomar-my.sharepoint.com/personal/aluno19089_ipt_pt/_layouts/15/download.aspx?UniqueId=bcb52594-9d40-43dc-960a-ab54a2e4d563&Translate=false&tempauth=eyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0.eyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUwOTAyIiwiZXhwIjoiMTU1OTE1NDUwMiIsImVuZHBvaW50dXJsIjoiVXQzdlgyZ0JGSmxnRzROWU5GbHdmcGtjZTc3WVNjeGtBQ0VhVGRtTUVzZz0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE1NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiTVRnNE5qUTBPRFF0TTJGbE5DMDBOV014TFRrek1qY3RORFV3TkdVNVl6QmhNak5tIiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ.clFWUUJxWHI3Q1pTcFNUUklSaWh6VW5mOStkWUMwUzFtM2pONExibVVUYz0&ApiVersion=2.0",
                    Formato = "CR2",
                    Thumbnail_Large = "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5MUEW23YQE53RBZMCVLKSROJVLD%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiT2FpVXNURDJrMFkyN1ZSakEzOW1PelZNa0lzOXR1aDNteGIyTGNkQytWUT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EcjdWV21sNFBIYThxcU9RcmtmQmpoNS9XdE5yU01GY0lyWndoNy85VnlDQT0%26version%3DPublished&width=800&height=800",
                    Thumbnail_Medium = "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5MUEW23YQE53RBZMCVLKSROJVLD%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiT2FpVXNURDJrMFkyN1ZSakEzOW1PelZNa0lzOXR1aDNteGIyTGNkQytWUT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EcjdWV21sNFBIYThxcU9RcmtmQmpoNS9XdE5yU01GY0lyWndoNy85VnlDQT0%26version%3DPublished&width=176&height=176",
                    Thumbnail_Small = "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5MUEW23YQE53RBZMCVLKSROJVLD%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiT2FpVXNURDJrMFkyN1ZSakEzOW1PelZNa0lzOXR1aDNteGIyTGNkQytWUT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EcjdWV21sNFBIYThxcU9RcmtmQmpoNS9XdE5yU01GY0lyWndoNy85VnlDQT0%26version%3DPublished&width=96&height=96",
                    GaleriaFK = 1
                },
                new Fotografia
                {
                    ID = 2,
                    Nome = "test2.CR2",
                    ItemId = "0127OBJ5KZYF7DBHUCHJBKGZXHAFT7LHAB",
                    ContaOnedriveFK = 1,
                    DownloadUrl = "https://politecnicotomar-my.sharepoint.com/personal/aluno19089_ipt_pt/_layouts/15/download.aspx?UniqueId=307ec159-829e-423a-a366-e70167f59c01&Translate=false&tempauth=eyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0.eyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE1NTE3MyIsImVuZHBvaW50dXJsIjoiN01BcEUvbS81eGM2VkJtVmY5cFd1ai95WXFmQmZmQS9HTzRhWmNsb0RXWT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE1NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ.REkwWVUwU1R3RkYxZlNRQXdML3Z3L2Y5WC9YUWlkSGU1Qy9ZakwyNS9FND0&ApiVersion=2.0",
                    Formato = "CR2",
                    Thumbnail_Large = "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5KZYF7DBHUCHJBKGZXHAFT7LHAB%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoibTdaVkhNaTltSjVPYXUybjBIcDFkQnFySGIxYVdKdTNxYVg1cGtpeXhxWT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EVkxmYjM5aE5QaG9jZXd6RjJhRU80UVdJc2NWS3pTNC9YWGMzVlg1RE9Wbz0%26version%3DPublished&width=800&height=800",
                    Thumbnail_Medium = "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5KZYF7DBHUCHJBKGZXHAFT7LHAB%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoibTdaVkhNaTltSjVPYXUybjBIcDFkQnFySGIxYVdKdTNxYVg1cGtpeXhxWT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EVkxmYjM5aE5QaG9jZXd6RjJhRU80UVdJc2NWS3pTNC9YWGMzVlg1RE9Wbz0%26version%3DPublished&width=176&height=176",
                    Thumbnail_Small = "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5KZYF7DBHUCHJBKGZXHAFT7LHAB%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoibTdaVkhNaTltSjVPYXUybjBIcDFkQnFySGIxYVdKdTNxYVg1cGtpeXhxWT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EVkxmYjM5aE5QaG9jZXd6RjJhRU80UVdJc2NWS3pTNC9YWGMzVlg1RE9Wbz0%26version%3DPublished&width=96&height=96",
                    GaleriaFK = 1
                },
                new Fotografia
                {
                    ID = 3,
                    Nome = "test3.CR2",
                    ItemId = "0127OBJ5JTSLEZMMK5O5C3R373T4AEZJHP",
                    ContaOnedriveFK = 1,
                    DownloadUrl = "https://politecnicotomar-my.sharepoint.com/personal/aluno19089_ipt_pt/_layouts/15/download.aspx?UniqueId=96c99233-5d31-4577-b8ef-fb9f004ca4ef&Translate=false&tempauth=eyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0.eyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE1NTE3MyIsImVuZHBvaW50dXJsIjoiNFlQVnAya0tWWjh1N1daQ1B5NEQrNGtvcFllS2dPcy9FOVVmblZpTnJLVT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE1NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ.dEFTU0k3TlZOa052ZkxjcXlDdnBMYkxGZWV0NEIvZ2FwMjRld3I4ZmNzRT0&ApiVersion=2.0",
                    Formato = "CR2",
                    Thumbnail_Large = "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5JTSLEZMMK5O5C3R373T4AEZJHP%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiWWpkcVhnaklBejVSUEpyVmVuL0pxN28wMG9mOUhRaHNWdlB0TWxsRVA0Zz0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2ESHJBNTR0UTB1Wkx5K05UN2xmdHFBRTYvSlVsK1k3MmJTZUtHNXp2NS9BQT0%26version%3DPublished&width=800&height=800",
                    Thumbnail_Medium = "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5JTSLEZMMK5O5C3R373T4AEZJHP%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiWWpkcVhnaklBejVSUEpyVmVuL0pxN28wMG9mOUhRaHNWdlB0TWxsRVA0Zz0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2ESHJBNTR0UTB1Wkx5K05UN2xmdHFBRTYvSlVsK1k3MmJTZUtHNXp2NS9BQT0%26version%3DPublished&width=176&height=176",
                    Thumbnail_Small = "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5JTSLEZMMK5O5C3R373T4AEZJHP%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiWWpkcVhnaklBejVSUEpyVmVuL0pxN28wMG9mOUhRaHNWdlB0TWxsRVA0Zz0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2ESHJBNTR0UTB1Wkx5K05UN2xmdHFBRTYvSlVsK1k3MmJTZUtHNXp2NS9BQT0%26version%3DPublished&width=96&height=96",
                    GaleriaFK = 1
                },
                new Fotografia
                {
                    ID = 4,
                    Nome = "test4.CR2",
                    ItemId = "0127OBJ5NJXHVS5BWV6RBYY6F32FINA3SH",
                    ContaOnedriveFK = 1,
                    DownloadUrl = "https://politecnicotomar-my.sharepoint.com/personal/aluno19089_ipt_pt/_layouts/15/download.aspx?UniqueId=2eebb9a9-d586-43f4-8c78-bbd150d06e47&Translate=false&tempauth=eyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0.eyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE1NTE3MyIsImVuZHBvaW50dXJsIjoiRkUzYVJiK2NPN055WitKNlZ0cG15VXI1RnpMUGw3SUYxMDBjRXh5YjFvbz0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE1NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ.N2lQT09ubjZJREJ1NTNkUWhJMTlaRzFsNG13WlFUcXEwU0NEZC80VG8rWT0&ApiVersion=2.0",
                    Formato = "CR2",
                    Thumbnail_Large = "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5NJXHVS5BWV6RBYY6F32FINA3SH%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiY05uYmM2Y0NvamxNMTl2SndMQmFPNUVWcGJkdDB3cWtMVWtmTXJjekduRT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EUXI4VXlDYWt4VC9GMGFkSkR2OG1CMGduOTFIVFl4cUg3L2k0eWdpbjh3MD0%26version%3DPublished&width=800&height=800",
                    Thumbnail_Medium = "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5NJXHVS5BWV6RBYY6F32FINA3SH%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiY05uYmM2Y0NvamxNMTl2SndMQmFPNUVWcGJkdDB3cWtMVWtmTXJjekduRT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EUXI4VXlDYWt4VC9GMGFkSkR2OG1CMGduOTFIVFl4cUg3L2k0eWdpbjh3MD0%26version%3DPublished&width=176&height=176",
                    Thumbnail_Small = "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5NJXHVS5BWV6RBYY6F32FINA3SH%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiY05uYmM2Y0NvamxNMTl2SndMQmFPNUVWcGJkdDB3cWtMVWtmTXJjekduRT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EUXI4VXlDYWt4VC9GMGFkSkR2OG1CMGduOTFIVFl4cUg3L2k0eWdpbjh3MD0%26version%3DPublished&width=96&height=96",
                    GaleriaFK = 1
                },
                new Fotografia
                {
                    ID = 5,
                    Nome = "test5.CR2",
                    ItemId = "0127OBJ5IICVYQNNWR2BA3V3KCRMT7A5WF",
                    ContaOnedriveFK = 1,
                    DownloadUrl = "https://politecnicotomar-my.sharepoint.com/personal/aluno19089_ipt_pt/_layouts/15/download.aspx?UniqueId=06711508-d1b6-41d0-baed-428b27f076c5&Translate=false&tempauth=eyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0.eyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE1NTE3MyIsImVuZHBvaW50dXJsIjoiMzBDRXRtSVYrTWNUWWcrTDVFeDJ3a21tdTZGc05KSmMwVVJKeFhIczZyTT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE1NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ.a05BY29NYnpZR0RUaHZ3all5N3FQNllQOWplL3lSVko5UmFLb0YwblMwUT0&ApiVersion=2.0",
                    Formato = "CR2",
                    Thumbnail_Large = "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5IICVYQNNWR2BA3V3KCRMT7A5WF%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoicHo5VjVONGJuVXNpdk1IY0hLc1BJd3ExZHA0SVJVVHJiRVhYb2lDOWQxMD0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EU3RVNDJJMmhxRk5iQTBsYlpQMnB6d1RoTzYxeWdjc3JoTU9RandYQkJPdz0%26version%3DPublished&width=800&height=800",
                    Thumbnail_Medium = "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5IICVYQNNWR2BA3V3KCRMT7A5WF%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoicHo5VjVONGJuVXNpdk1IY0hLc1BJd3ExZHA0SVJVVHJiRVhYb2lDOWQxMD0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EU3RVNDJJMmhxRk5iQTBsYlpQMnB6d1RoTzYxeWdjc3JoTU9RandYQkJPdz0%26version%3DPublished&width=176&height=176",
                    Thumbnail_Small = "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5IICVYQNNWR2BA3V3KCRMT7A5WF%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoicHo5VjVONGJuVXNpdk1IY0hLc1BJd3ExZHA0SVJVVHJiRVhYb2lDOWQxMD0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EU3RVNDJJMmhxRk5iQTBsYlpQMnB6d1RoTzYxeWdjc3JoTU9RandYQkJPdz0%26version%3DPublished&width=96&height=96",
                    GaleriaFK = 1
                },
                new Fotografia
                {
                    ID = 6,
                    Nome = "test6.CR2",
                    ItemId = "0127OBJ5NKSZAFCQIH3FFK2XM7SBRH3OYE",
                    ContaOnedriveFK = 1,
                    DownloadUrl = "https://politecnicotomar-my.sharepoint.com/personal/aluno19089_ipt_pt/_layouts/15/download.aspx?UniqueId=514096aa-0741-4ad9-ad5d-9f90627dbb04&Translate=false&tempauth=eyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0.eyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE1NTE3MyIsImVuZHBvaW50dXJsIjoiRTc1RlVINlpDQkZHNERtY0NYOHJ1K2luVnA3MERCbll6eUlqWFFlT3h2TT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE1NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ.cHkwcXIvU21LUmtRUjZveHFsY1dMVFhPbTJxVzcwTlNOOHAxd2hzYTVTTT0&ApiVersion=2.0",
                    Formato = "CR2",
                    Thumbnail_Large = "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5NKSZAFCQIH3FFK2XM7SBRH3OYE%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiRTlXZ1REajNuQ0Y4cTIrTTg3YTFtYnhrRm5TaTZ6NmwraFcxM2c2SFJhWT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2ENTR1NlRRWHBHZmg4c1ZVZ0lLaVZuSW0xRm9qV1ZpRzVGclBiSU5WbU1Naz0%26version%3DPublished&width=800&height=800",
                    Thumbnail_Medium = "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5NKSZAFCQIH3FFK2XM7SBRH3OYE%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiRTlXZ1REajNuQ0Y4cTIrTTg3YTFtYnhrRm5TaTZ6NmwraFcxM2c2SFJhWT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2ENTR1NlRRWHBHZmg4c1ZVZ0lLaVZuSW0xRm9qV1ZpRzVGclBiSU5WbU1Naz0%26version%3DPublished&width=176&height=176",
                    Thumbnail_Small = "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5NKSZAFCQIH3FFK2XM7SBRH3OYE%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiRTlXZ1REajNuQ0Y4cTIrTTg3YTFtYnhrRm5TaTZ6NmwraFcxM2c2SFJhWT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2ENTR1NlRRWHBHZmg4c1ZVZ0lLaVZuSW0xRm9qV1ZpRzVGclBiSU5WbU1Naz0%26version%3DPublished&width=96&height=96",
                    GaleriaFK = 1
                },
                new Fotografia
                {
                    ID = 7,
                    Nome = "test7.CR2",
                    ItemId = "0127OBJ5K52HLG54V7NRCYRQRMVFEUQF7J",
                    ContaOnedriveFK = 1,
                    DownloadUrl = "https://politecnicotomar-my.sharepoint.com/personal/aluno19089_ipt_pt/_layouts/15/download.aspx?UniqueId=6ed6d15d-bff2-456c-88c2-2ca9494817e9&Translate=false&tempauth=eyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0.eyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE1NTE3MyIsImVuZHBvaW50dXJsIjoiaWIvVVBVT0hrcU1PYmREMWZBdzFOaFNBTjBFRjNGNTQ1K3JYb2VrK1ZiTT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE1NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ.R1lGa0JDU0s4d1N5cXF2SnNiMzB4UUhUMUVsV0tJR212dmsrWEpjS1F0bz0&ApiVersion=2.0",
                    Formato = "CR2",
                    Thumbnail_Large = "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5K52HLG54V7NRCYRQRMVFEUQF7J%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiQ1hTb25TMS9oQzVZNzBVK2pQODlwMHdGaTJnMW44MmlXTjI3VSswakZXaz0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EK3l2bllKTFRmS0paWlEwUXpuT3dHMEZmQXFvU0xka01XdTRyVzZXbEswWT0%26version%3DPublished&width=800&height=800",
                    Thumbnail_Medium = "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5K52HLG54V7NRCYRQRMVFEUQF7J%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiQ1hTb25TMS9oQzVZNzBVK2pQODlwMHdGaTJnMW44MmlXTjI3VSswakZXaz0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EK3l2bllKTFRmS0paWlEwUXpuT3dHMEZmQXFvU0xka01XdTRyVzZXbEswWT0%26version%3DPublished&width=176&height=176",
                    Thumbnail_Small = "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5K52HLG54V7NRCYRQRMVFEUQF7J%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiQ1hTb25TMS9oQzVZNzBVK2pQODlwMHdGaTJnMW44MmlXTjI3VSswakZXaz0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EK3l2bllKTFRmS0paWlEwUXpuT3dHMEZmQXFvU0xka01XdTRyVzZXbEswWT0%26version%3DPublished&width=96&height=96",
                    GaleriaFK = 1
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

        public DbSet<Galeria> Galerias { get; set; }

        public DbSet<Fotografia> Fotografias { get; set; }

        public DbSet<Metadado> Metadados { get; set; }

        public DbSet<ContaOnedrive> ContasOnedrive { get; set; }

        public DbSet<Galeria_Metadado> Galerias_Metadados { get; set; }
    }
}
