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
                .HasKey(gm => new { gm.MetadadoFK, gm.GaleriaFK });
            modelBuilder.Entity<Galeria_Metadado>()
                .HasOne(gm => gm.Galeria)
                .WithMany(g => g.Galerias_Metadados)
                .HasForeignKey(gm => gm.GaleriaFK);
            modelBuilder.Entity<Galeria_Metadado>()
                .HasOne(gm => gm.Metadado)
                .WithMany(m => m.Galerias_Metadados)
                .HasForeignKey(gm => gm.MetadadoFK);

            // n-m ---> Partilhavel_Fotografia
            modelBuilder.Entity<Partilhavel_Fotografia>()
                .HasKey(pf => new { pf.FotografiaFK, pf.PartilhavelFK });
            modelBuilder.Entity<Partilhavel_Fotografia>()
                .HasOne(pf => pf.Partilhavel)
                .WithMany(p => p.Partilhaveis_Fotografias)
                .HasForeignKey(pf => pf.PartilhavelFK);
            modelBuilder.Entity<Partilhavel_Fotografia>()
                .HasOne(pf => pf.Fotografia)
                .WithMany(f => f.Partilhaveis_Fotografias)
                .HasForeignKey(pf => pf.FotografiaFK);

            string id = "a18be9c0-aa65-4af8-bd17-00bd9344e575";
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = id,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper()
                },
                new IdentityRole
                {
                    Name = "Lab",
                    NormalizedName = "Lab".ToUpper()
                }
            );

            var hasher = new PasswordHasher<IdentityUser>();
            modelBuilder.Entity<IdentityUser>().HasData(
                new IdentityUser
                {
                    Id = id,
                    UserName = "labfotoipt@gmail.com",
                    NormalizedUserName = "labfotoipt@gmail.com".ToUpper(),
                    Email = "labfotoipt@gmail.com",
                    NormalizedEmail = "labfotoipt@gmail.com".ToUpper(),
                    EmailConfirmed = true,
                    AccessFailedCount = 0,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    SecurityStamp = string.Empty,
                    PasswordHash = hasher.HashPassword(null, "123Qwe!")
                }
            );

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = id,
                UserId = id
            });

            modelBuilder.Entity<Tipo>().HasData(
                new Tipo
                {
                    ID = 1,
                    Nome = "Académico",
                    Deletable = false
                },
                new Tipo
                {
                    ID = 2,
                    Nome = "Investigação",
                    Deletable = false
                },
                new Tipo
                {
                    ID = 3,
                    Nome = "Serviço Exterior",
                    Deletable = false
                },
                new Tipo
                {
                    ID = 4,
                    Nome = "Pessoal",
                    Deletable = false
                }
            );            

            modelBuilder.Entity<ServicoSolicitado>().HasData(
                new ServicoSolicitado
                {
                    ID = 1,
                    Nome = "Luz Visível",
                    Deletable = false
                },
                new ServicoSolicitado
                {
                    ID = 2,
                    Nome = "Luz U.V",
                    Deletable = false
                },
                new ServicoSolicitado
                {
                    ID = 3,
                    Nome = "Rasante",
                    Deletable = false
                },
                new ServicoSolicitado
                {
                    ID = 4,
                    Nome = "Infra-red",
                    Deletable = false
                },
                new ServicoSolicitado
                {
                    ID = 5,
                    Nome = "Luz Trasmitida",
                    Deletable = false
                }
            );
        }

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

        public DbSet<Partilhavel> Partilhaveis { get; set; }

        public DbSet<Partilhavel_Fotografia> Partilhaveis_Fotografias { get; set; }
        public DbSet<Log> Logs { get; set; }
    }
}
