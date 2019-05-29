﻿using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LabFoto.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    Nome = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContasOnedrive",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DriveId = table.Column<string>(nullable: true),
                    AccessToken = table.Column<string>(nullable: true),
                    RefreshToken = table.Column<string>(nullable: true),
                    Quota_Total = table.Column<string>(nullable: true),
                    Quota_Remaining = table.Column<string>(nullable: true),
                    Quota_Used = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContasOnedrive", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DataExecucao",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Data = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataExecucao", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Metadados",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metadados", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Requerentes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: false),
                    Telemovel = table.Column<string>(maxLength: 12, nullable: true),
                    Email = table.Column<string>(nullable: false),
                    Responsavel = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requerentes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ServicosSolicitados",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicosSolicitados", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Tipos",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipos", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Servicos",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(maxLength: 255, nullable: false),
                    DataDeCriacao = table.Column<DateTime>(nullable: false),
                    IdentificacaoObra = table.Column<string>(nullable: false),
                    Observacoes = table.Column<string>(maxLength: 512, nullable: true),
                    HorasEstudio = table.Column<float>(nullable: true),
                    HorasPosProducao = table.Column<float>(nullable: true),
                    DataEntrega = table.Column<DateTime>(nullable: true),
                    Total = table.Column<float>(nullable: true),
                    Hide = table.Column<bool>(nullable: false),
                    RequerenteFK = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servicos", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Servicos_Requerentes_RequerenteFK",
                        column: x => x.RequerenteFK,
                        principalTable: "Requerentes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Galerias",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: true),
                    DataDeCriacao = table.Column<DateTime>(nullable: false),
                    ServicoFK = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Galerias", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Galerias_Servicos_ServicoFK",
                        column: x => x.ServicoFK,
                        principalTable: "Servicos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Servicos_DatasExecucao",
                columns: table => new
                {
                    DataExecucaoFK = table.Column<int>(nullable: false),
                    ServicoFK = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servicos_DatasExecucao", x => new { x.DataExecucaoFK, x.ServicoFK });
                    table.ForeignKey(
                        name: "FK_Servicos_DatasExecucao_DataExecucao_DataExecucaoFK",
                        column: x => x.DataExecucaoFK,
                        principalTable: "DataExecucao",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Servicos_DatasExecucao_Servicos_ServicoFK",
                        column: x => x.ServicoFK,
                        principalTable: "Servicos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Servicos_ServicosSolicitados",
                columns: table => new
                {
                    ServicoSolicitadoFK = table.Column<int>(nullable: false),
                    ServicoFK = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servicos_ServicosSolicitados", x => new { x.ServicoSolicitadoFK, x.ServicoFK });
                    table.ForeignKey(
                        name: "FK_Servicos_ServicosSolicitados_Servicos_ServicoFK",
                        column: x => x.ServicoFK,
                        principalTable: "Servicos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Servicos_ServicosSolicitados_ServicosSolicitados_ServicoSolicitadoFK",
                        column: x => x.ServicoSolicitadoFK,
                        principalTable: "ServicosSolicitados",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Servicos_Tipos",
                columns: table => new
                {
                    TipoFK = table.Column<int>(nullable: false),
                    ServicoFK = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servicos_Tipos", x => new { x.TipoFK, x.ServicoFK });
                    table.ForeignKey(
                        name: "FK_Servicos_Tipos_Servicos_ServicoFK",
                        column: x => x.ServicoFK,
                        principalTable: "Servicos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Servicos_Tipos_Tipos_TipoFK",
                        column: x => x.TipoFK,
                        principalTable: "Tipos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fotografias",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: true),
                    Formato = table.Column<string>(nullable: true),
                    DownloadUrl = table.Column<string>(nullable: true),
                    Thumbnail_Small = table.Column<string>(nullable: true),
                    Thumbnail_Medium = table.Column<string>(nullable: true),
                    Thumbnail_Large = table.Column<string>(nullable: true),
                    FotografiaOrigemFK = table.Column<int>(nullable: false),
                    FotografiaOrigemID = table.Column<int>(nullable: true),
                    ContaOnedriveFK = table.Column<int>(nullable: false),
                    GaleriaFK = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fotografias", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Fotografias_ContasOnedrive_ContaOnedriveFK",
                        column: x => x.ContaOnedriveFK,
                        principalTable: "ContasOnedrive",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fotografias_Fotografias_FotografiaOrigemID",
                        column: x => x.FotografiaOrigemID,
                        principalTable: "Fotografias",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Fotografias_Galerias_GaleriaFK",
                        column: x => x.GaleriaFK,
                        principalTable: "Galerias",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Galerias_Metadados",
                columns: table => new
                {
                    MetadadoFK = table.Column<int>(nullable: false),
                    GaleriaFK = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Galerias_Metadados", x => new { x.MetadadoFK, x.GaleriaFK });
                    table.ForeignKey(
                        name: "FK_Galerias_Metadados_Galerias_GaleriaFK",
                        column: x => x.GaleriaFK,
                        principalTable: "Galerias",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Galerias_Metadados_Metadados_MetadadoFK",
                        column: x => x.MetadadoFK,
                        principalTable: "Metadados",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "efbd71e2-da58-467d-b5a7-38c0fdaeb8c1", "8d625411-ec18-44ed-8988-6833e94642cf", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "Nome" },
                values: new object[,]
                {
                    { "73a9eaf0-43f6-43a6-bf98-f0bb4e8a93b7", 0, "7ab190eb-e04a-4572-b835-871e1e913b5b", "Utilizador", "admin1@admin1.com", false, false, null, "ADMIN1@ADMIN1.COM", "ADMIN1@ADMIN1.COM", "AQAAAAEAACcQAAAAENfK8mON08CTNG0qFGsgRdA0oxsjTzCIwn9ge9m98wBqVu8G5C3P3lr3cxFwdgJZ8w==", null, false, "", false, "admin1@admin1.com", "Admin1" },
                    { "fcbbb3e1-e6ce-43b3-922d-f7342c59e5f1", 0, "bbb091d8-952d-4c0a-b15c-ca1ded3ffd1b", "Utilizador", "user1@user1.com", false, false, null, "USER1@USER1.COM", "USER1@USER1.COM", "AQAAAAEAACcQAAAAEJ+E2CyIWZAxxWP/nBFxmHVeCKgdkzRLmqNW7E1mTYbzofoRQytrQZsoDVSCPYFPWw==", null, false, "", false, "user1@user1.com", "User1" }
                });

            migrationBuilder.InsertData(
                table: "ContasOnedrive",
                columns: new[] { "ID", "AccessToken", "DriveId", "Quota_Remaining", "Quota_Total", "Quota_Used", "RefreshToken" },
                values: new object[] { 1, null, "asdasd", null, null, null, null });

            migrationBuilder.InsertData(
                table: "DataExecucao",
                columns: new[] { "ID", "Data" },
                values: new object[,]
                {
                    { 4, new DateTime(2019, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, new DateTime(2019, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, new DateTime(2018, 2, 27, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, new DateTime(2018, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, new DateTime(2019, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, new DateTime(2018, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, new DateTime(2018, 7, 21, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, new DateTime(2019, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(2019, 2, 24, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 1, new DateTime(2019, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, new DateTime(2018, 9, 16, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Metadados",
                columns: new[] { "ID", "Nome" },
                values: new object[,]
                {
                    { 1, "Metadado1" },
                    { 2, "Metadado2" },
                    { 3, "Metadado3" },
                    { 4, "Metadado4" }
                });

            migrationBuilder.InsertData(
                table: "Requerentes",
                columns: new[] { "ID", "Email", "Nome", "Responsavel", "Telemovel" },
                values: new object[,]
                {
                    { 5, "manuel@ipt.pt", "Manuel", "Manuel", "987321546" },
                    { 3, "joao@ipt.pt", "João", "Maria", "987654321" },
                    { 2, "maria@ipt.pt", "Maria", "Maria", "987654321" },
                    { 4, "jose@ipt.pt", "José", "José", "123789456" },
                    { 1, "fernando@ipt.pt", "Fernando", "Fernando", "123456789" }
                });

            migrationBuilder.InsertData(
                table: "ServicosSolicitados",
                columns: new[] { "ID", "Nome" },
                values: new object[,]
                {
                    { 1, "Luz Visível" },
                    { 2, "Luz U.V" },
                    { 3, "Rasante" },
                    { 4, "Infra-red" },
                    { 5, "Luz Trasmitida" }
                });

            migrationBuilder.InsertData(
                table: "Tipos",
                columns: new[] { "ID", "Nome" },
                values: new object[,]
                {
                    { 1, "Académico" },
                    { 2, "Investigação" },
                    { 3, "Serviço Exterior" },
                    { 4, "Pessoal" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[,]
                {
                    { "73a9eaf0-43f6-43a6-bf98-f0bb4e8a93b7", "efbd71e2-da58-467d-b5a7-38c0fdaeb8c1" },
                    { "fcbbb3e1-e6ce-43b3-922d-f7342c59e5f1", "efbd71e2-da58-467d-b5a7-38c0fdaeb8c1" }
                });

            migrationBuilder.InsertData(
                table: "Servicos",
                columns: new[] { "ID", "DataDeCriacao", "DataEntrega", "Hide", "HorasEstudio", "HorasPosProducao", "IdentificacaoObra", "Nome", "Observacoes", "RequerenteFK", "Total" },
                values: new object[,]
                {
                    { 1, new DateTime(2018, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2018, 6, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 2f, 8f, "Tira larga, de tecido ou de madeira, que se dispõe transversalmente como ornato na parte superior de uma cortina.", "Sanefa", "Sanefa degradada na parte superior", 1, 40f },
                    { 3, new DateTime(2019, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2019, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 4f, 10f, "Vasos/Jarrões a parecem integrados em retábulos,tronos, mesas de altar em várias igrejas em Tomar.", "Vaso talha prata dourada", "", 1, 45f },
                    { 4, new DateTime(2017, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2017, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 20f, 43f, "Seis cadeiras em madeira de cerejeira com acabamento em verniz", "Conjunto de Cadeiras", "", 1, 125f },
                    { 2, new DateTime(2019, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2019, 3, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 6f, 17f, "Plantação de árvores na escola.", "Dia da árvore", "Presença do presidente nas plantações.", 2, 65f },
                    { 7, new DateTime(2018, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2018, 2, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 6f, 2f, "Um cavalo queimado no matadouro", "Estátua do Cavalo queimado", "", 2, 0f },
                    { 5, new DateTime(2018, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, 3f, 4f, "Festa tradicional da aldeia de Azinhaga", "Festa do Bodo", "Muito bom", 3, 0f },
                    { 6, new DateTime(2019, 11, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, 0f, 0f, "Fotos da tabanca do Maltez", "Feira do Cavalo, Golegã", "Grande festa, aprovado", 4, 0f },
                    { 8, new DateTime(2019, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, 4f, 2f, "Exposição na galeria de Sintra", "Exposição de Arte contemporânea", "", 5, 0f }
                });

            migrationBuilder.InsertData(
                table: "Galerias",
                columns: new[] { "ID", "DataDeCriacao", "Nome", "ServicoFK" },
                values: new object[,]
                {
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Galeria2", 1 },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Galeria3", 1 },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Galeria4", 1 },
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Galeria1", 6 }
                });

            migrationBuilder.InsertData(
                table: "Servicos_DatasExecucao",
                columns: new[] { "DataExecucaoFK", "ServicoFK" },
                values: new object[,]
                {
                    { 10, 8 },
                    { 11, 8 },
                    { 8, 8 },
                    { 6, 6 },
                    { 8, 5 },
                    { 5, 5 },
                    { 2, 7 },
                    { 5, 7 },
                    { 2, 2 },
                    { 7, 7 },
                    { 10, 3 },
                    { 1, 1 },
                    { 3, 3 },
                    { 9, 3 },
                    { 11, 3 },
                    { 4, 4 }
                });

            migrationBuilder.InsertData(
                table: "Servicos_ServicosSolicitados",
                columns: new[] { "ServicoSolicitadoFK", "ServicoFK" },
                values: new object[,]
                {
                    { 1, 8 },
                    { 3, 1 },
                    { 2, 1 },
                    { 4, 6 },
                    { 2, 6 },
                    { 5, 5 },
                    { 2, 5 },
                    { 3, 7 },
                    { 1, 4 },
                    { 4, 2 },
                    { 5, 7 },
                    { 5, 3 },
                    { 3, 8 },
                    { 2, 4 },
                    { 3, 4 },
                    { 1, 2 },
                    { 4, 4 },
                    { 2, 7 }
                });

            migrationBuilder.InsertData(
                table: "Servicos_Tipos",
                columns: new[] { "TipoFK", "ServicoFK" },
                values: new object[,]
                {
                    { 3, 4 },
                    { 3, 6 },
                    { 4, 7 },
                    { 1, 3 },
                    { 1, 1 },
                    { 2, 5 },
                    { 2, 2 },
                    { 1, 6 },
                    { 4, 8 }
                });

            migrationBuilder.InsertData(
                table: "Fotografias",
                columns: new[] { "ID", "ContaOnedriveFK", "DownloadUrl", "Formato", "FotografiaOrigemFK", "FotografiaOrigemID", "GaleriaFK", "Nome", "Thumbnail_Large", "Thumbnail_Medium", "Thumbnail_Small" },
                values: new object[,]
                {
                    { 1, 1, "https://politecnicotomar-my.sharepoint.com/personal/aluno19089_ipt_pt/_layouts/15/download.aspx?UniqueId=bcb52594-9d40-43dc-960a-ab54a2e4d563&Translate=false&tempauth=eyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0.eyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUwOTAyIiwiZXhwIjoiMTU1OTE1NDUwMiIsImVuZHBvaW50dXJsIjoiVXQzdlgyZ0JGSmxnRzROWU5GbHdmcGtjZTc3WVNjeGtBQ0VhVGRtTUVzZz0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE1NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiTVRnNE5qUTBPRFF0TTJGbE5DMDBOV014TFRrek1qY3RORFV3TkdVNVl6QmhNak5tIiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ.clFWUUJxWHI3Q1pTcFNUUklSaWh6VW5mOStkWUMwUzFtM2pONExibVVUYz0&ApiVersion=2.0", "CR2", 0, null, 1, "test1.CR2", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5MUEW23YQE53RBZMCVLKSROJVLD%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiT2FpVXNURDJrMFkyN1ZSakEzOW1PelZNa0lzOXR1aDNteGIyTGNkQytWUT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EcjdWV21sNFBIYThxcU9RcmtmQmpoNS9XdE5yU01GY0lyWndoNy85VnlDQT0%26version%3DPublished&width=800&height=800", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5MUEW23YQE53RBZMCVLKSROJVLD%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiT2FpVXNURDJrMFkyN1ZSakEzOW1PelZNa0lzOXR1aDNteGIyTGNkQytWUT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EcjdWV21sNFBIYThxcU9RcmtmQmpoNS9XdE5yU01GY0lyWndoNy85VnlDQT0%26version%3DPublished&width=176&height=176", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5MUEW23YQE53RBZMCVLKSROJVLD%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiT2FpVXNURDJrMFkyN1ZSakEzOW1PelZNa0lzOXR1aDNteGIyTGNkQytWUT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EcjdWV21sNFBIYThxcU9RcmtmQmpoNS9XdE5yU01GY0lyWndoNy85VnlDQT0%26version%3DPublished&width=96&height=96" },
                    { 2, 1, "https://politecnicotomar-my.sharepoint.com/personal/aluno19089_ipt_pt/_layouts/15/download.aspx?UniqueId=307ec159-829e-423a-a366-e70167f59c01&Translate=false&tempauth=eyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0.eyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE1NTE3MyIsImVuZHBvaW50dXJsIjoiN01BcEUvbS81eGM2VkJtVmY5cFd1ai95WXFmQmZmQS9HTzRhWmNsb0RXWT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE1NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ.REkwWVUwU1R3RkYxZlNRQXdML3Z3L2Y5WC9YUWlkSGU1Qy9ZakwyNS9FND0&ApiVersion=2.0", "CR2", 0, null, 1, "test2.CR2", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5KZYF7DBHUCHJBKGZXHAFT7LHAB%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoibTdaVkhNaTltSjVPYXUybjBIcDFkQnFySGIxYVdKdTNxYVg1cGtpeXhxWT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EVkxmYjM5aE5QaG9jZXd6RjJhRU80UVdJc2NWS3pTNC9YWGMzVlg1RE9Wbz0%26version%3DPublished&width=800&height=800", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5KZYF7DBHUCHJBKGZXHAFT7LHAB%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoibTdaVkhNaTltSjVPYXUybjBIcDFkQnFySGIxYVdKdTNxYVg1cGtpeXhxWT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EVkxmYjM5aE5QaG9jZXd6RjJhRU80UVdJc2NWS3pTNC9YWGMzVlg1RE9Wbz0%26version%3DPublished&width=176&height=176", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5KZYF7DBHUCHJBKGZXHAFT7LHAB%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoibTdaVkhNaTltSjVPYXUybjBIcDFkQnFySGIxYVdKdTNxYVg1cGtpeXhxWT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EVkxmYjM5aE5QaG9jZXd6RjJhRU80UVdJc2NWS3pTNC9YWGMzVlg1RE9Wbz0%26version%3DPublished&width=96&height=96" },
                    { 3, 1, "https://politecnicotomar-my.sharepoint.com/personal/aluno19089_ipt_pt/_layouts/15/download.aspx?UniqueId=96c99233-5d31-4577-b8ef-fb9f004ca4ef&Translate=false&tempauth=eyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0.eyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE1NTE3MyIsImVuZHBvaW50dXJsIjoiNFlQVnAya0tWWjh1N1daQ1B5NEQrNGtvcFllS2dPcy9FOVVmblZpTnJLVT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE1NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ.dEFTU0k3TlZOa052ZkxjcXlDdnBMYkxGZWV0NEIvZ2FwMjRld3I4ZmNzRT0&ApiVersion=2.0", "CR2", 0, null, 1, "test3.CR2", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5JTSLEZMMK5O5C3R373T4AEZJHP%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiWWpkcVhnaklBejVSUEpyVmVuL0pxN28wMG9mOUhRaHNWdlB0TWxsRVA0Zz0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2ESHJBNTR0UTB1Wkx5K05UN2xmdHFBRTYvSlVsK1k3MmJTZUtHNXp2NS9BQT0%26version%3DPublished&width=800&height=800", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5JTSLEZMMK5O5C3R373T4AEZJHP%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiWWpkcVhnaklBejVSUEpyVmVuL0pxN28wMG9mOUhRaHNWdlB0TWxsRVA0Zz0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2ESHJBNTR0UTB1Wkx5K05UN2xmdHFBRTYvSlVsK1k3MmJTZUtHNXp2NS9BQT0%26version%3DPublished&width=176&height=176", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5JTSLEZMMK5O5C3R373T4AEZJHP%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiWWpkcVhnaklBejVSUEpyVmVuL0pxN28wMG9mOUhRaHNWdlB0TWxsRVA0Zz0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2ESHJBNTR0UTB1Wkx5K05UN2xmdHFBRTYvSlVsK1k3MmJTZUtHNXp2NS9BQT0%26version%3DPublished&width=96&height=96" },
                    { 4, 1, "https://politecnicotomar-my.sharepoint.com/personal/aluno19089_ipt_pt/_layouts/15/download.aspx?UniqueId=2eebb9a9-d586-43f4-8c78-bbd150d06e47&Translate=false&tempauth=eyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0.eyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE1NTE3MyIsImVuZHBvaW50dXJsIjoiRkUzYVJiK2NPN055WitKNlZ0cG15VXI1RnpMUGw3SUYxMDBjRXh5YjFvbz0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE1NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ.N2lQT09ubjZJREJ1NTNkUWhJMTlaRzFsNG13WlFUcXEwU0NEZC80VG8rWT0&ApiVersion=2.0", "CR2", 0, null, 1, "test4.CR2", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5NJXHVS5BWV6RBYY6F32FINA3SH%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiY05uYmM2Y0NvamxNMTl2SndMQmFPNUVWcGJkdDB3cWtMVWtmTXJjekduRT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EUXI4VXlDYWt4VC9GMGFkSkR2OG1CMGduOTFIVFl4cUg3L2k0eWdpbjh3MD0%26version%3DPublished&width=800&height=800", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5NJXHVS5BWV6RBYY6F32FINA3SH%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiY05uYmM2Y0NvamxNMTl2SndMQmFPNUVWcGJkdDB3cWtMVWtmTXJjekduRT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EUXI4VXlDYWt4VC9GMGFkSkR2OG1CMGduOTFIVFl4cUg3L2k0eWdpbjh3MD0%26version%3DPublished&width=176&height=176", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5NJXHVS5BWV6RBYY6F32FINA3SH%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiY05uYmM2Y0NvamxNMTl2SndMQmFPNUVWcGJkdDB3cWtMVWtmTXJjekduRT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EUXI4VXlDYWt4VC9GMGFkSkR2OG1CMGduOTFIVFl4cUg3L2k0eWdpbjh3MD0%26version%3DPublished&width=96&height=96" },
                    { 5, 1, "https://politecnicotomar-my.sharepoint.com/personal/aluno19089_ipt_pt/_layouts/15/download.aspx?UniqueId=06711508-d1b6-41d0-baed-428b27f076c5&Translate=false&tempauth=eyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0.eyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE1NTE3MyIsImVuZHBvaW50dXJsIjoiMzBDRXRtSVYrTWNUWWcrTDVFeDJ3a21tdTZGc05KSmMwVVJKeFhIczZyTT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE1NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ.a05BY29NYnpZR0RUaHZ3all5N3FQNllQOWplL3lSVko5UmFLb0YwblMwUT0&ApiVersion=2.0", "CR2", 0, null, 1, "test5.CR2", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5IICVYQNNWR2BA3V3KCRMT7A5WF%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoicHo5VjVONGJuVXNpdk1IY0hLc1BJd3ExZHA0SVJVVHJiRVhYb2lDOWQxMD0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EU3RVNDJJMmhxRk5iQTBsYlpQMnB6d1RoTzYxeWdjc3JoTU9RandYQkJPdz0%26version%3DPublished&width=800&height=800", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5IICVYQNNWR2BA3V3KCRMT7A5WF%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoicHo5VjVONGJuVXNpdk1IY0hLc1BJd3ExZHA0SVJVVHJiRVhYb2lDOWQxMD0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EU3RVNDJJMmhxRk5iQTBsYlpQMnB6d1RoTzYxeWdjc3JoTU9RandYQkJPdz0%26version%3DPublished&width=176&height=176", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5IICVYQNNWR2BA3V3KCRMT7A5WF%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoicHo5VjVONGJuVXNpdk1IY0hLc1BJd3ExZHA0SVJVVHJiRVhYb2lDOWQxMD0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EU3RVNDJJMmhxRk5iQTBsYlpQMnB6d1RoTzYxeWdjc3JoTU9RandYQkJPdz0%26version%3DPublished&width=96&height=96" },
                    { 6, 1, "https://politecnicotomar-my.sharepoint.com/personal/aluno19089_ipt_pt/_layouts/15/download.aspx?UniqueId=514096aa-0741-4ad9-ad5d-9f90627dbb04&Translate=false&tempauth=eyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0.eyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE1NTE3MyIsImVuZHBvaW50dXJsIjoiRTc1RlVINlpDQkZHNERtY0NYOHJ1K2luVnA3MERCbll6eUlqWFFlT3h2TT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE1NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ.cHkwcXIvU21LUmtRUjZveHFsY1dMVFhPbTJxVzcwTlNOOHAxd2hzYTVTTT0&ApiVersion=2.0", "CR2", 0, null, 1, "test6.CR2", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5NKSZAFCQIH3FFK2XM7SBRH3OYE%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiRTlXZ1REajNuQ0Y4cTIrTTg3YTFtYnhrRm5TaTZ6NmwraFcxM2c2SFJhWT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2ENTR1NlRRWHBHZmg4c1ZVZ0lLaVZuSW0xRm9qV1ZpRzVGclBiSU5WbU1Naz0%26version%3DPublished&width=800&height=800", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5NKSZAFCQIH3FFK2XM7SBRH3OYE%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiRTlXZ1REajNuQ0Y4cTIrTTg3YTFtYnhrRm5TaTZ6NmwraFcxM2c2SFJhWT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2ENTR1NlRRWHBHZmg4c1ZVZ0lLaVZuSW0xRm9qV1ZpRzVGclBiSU5WbU1Naz0%26version%3DPublished&width=176&height=176", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5NKSZAFCQIH3FFK2XM7SBRH3OYE%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiRTlXZ1REajNuQ0Y4cTIrTTg3YTFtYnhrRm5TaTZ6NmwraFcxM2c2SFJhWT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2ENTR1NlRRWHBHZmg4c1ZVZ0lLaVZuSW0xRm9qV1ZpRzVGclBiSU5WbU1Naz0%26version%3DPublished&width=96&height=96" },
                    { 7, 1, "https://politecnicotomar-my.sharepoint.com/personal/aluno19089_ipt_pt/_layouts/15/download.aspx?UniqueId=6ed6d15d-bff2-456c-88c2-2ca9494817e9&Translate=false&tempauth=eyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0.eyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE1NTE3MyIsImVuZHBvaW50dXJsIjoiaWIvVVBVT0hrcU1PYmREMWZBdzFOaFNBTjBFRjNGNTQ1K3JYb2VrK1ZiTT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE1NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ.R1lGa0JDU0s4d1N5cXF2SnNiMzB4UUhUMUVsV0tJR212dmsrWEpjS1F0bz0&ApiVersion=2.0", "CR2", 0, null, 1, "test7.CR2", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5K52HLG54V7NRCYRQRMVFEUQF7J%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiQ1hTb25TMS9oQzVZNzBVK2pQODlwMHdGaTJnMW44MmlXTjI3VSswakZXaz0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EK3l2bllKTFRmS0paWlEwUXpuT3dHMEZmQXFvU0xka01XdTRyVzZXbEswWT0%26version%3DPublished&width=800&height=800", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5K52HLG54V7NRCYRQRMVFEUQF7J%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiQ1hTb25TMS9oQzVZNzBVK2pQODlwMHdGaTJnMW44MmlXTjI3VSswakZXaz0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EK3l2bllKTFRmS0paWlEwUXpuT3dHMEZmQXFvU0xka01XdTRyVzZXbEswWT0%26version%3DPublished&width=176&height=176", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5K52HLG54V7NRCYRQRMVFEUQF7J%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiQ1hTb25TMS9oQzVZNzBVK2pQODlwMHdGaTJnMW44MmlXTjI3VSswakZXaz0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EK3l2bllKTFRmS0paWlEwUXpuT3dHMEZmQXFvU0xka01XdTRyVzZXbEswWT0%26version%3DPublished&width=96&height=96" }
                });

            migrationBuilder.InsertData(
                table: "Galerias_Metadados",
                columns: new[] { "MetadadoFK", "GaleriaFK" },
                values: new object[,]
                {
                    { 2, 2 },
                    { 3, 3 },
                    { 4, 4 },
                    { 1, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Fotografias_ContaOnedriveFK",
                table: "Fotografias",
                column: "ContaOnedriveFK");

            migrationBuilder.CreateIndex(
                name: "IX_Fotografias_FotografiaOrigemID",
                table: "Fotografias",
                column: "FotografiaOrigemID");

            migrationBuilder.CreateIndex(
                name: "IX_Fotografias_GaleriaFK",
                table: "Fotografias",
                column: "GaleriaFK");

            migrationBuilder.CreateIndex(
                name: "IX_Galerias_ServicoFK",
                table: "Galerias",
                column: "ServicoFK");

            migrationBuilder.CreateIndex(
                name: "IX_Galerias_Metadados_GaleriaFK",
                table: "Galerias_Metadados",
                column: "GaleriaFK");

            migrationBuilder.CreateIndex(
                name: "IX_Servicos_RequerenteFK",
                table: "Servicos",
                column: "RequerenteFK");

            migrationBuilder.CreateIndex(
                name: "IX_Servicos_DatasExecucao_ServicoFK",
                table: "Servicos_DatasExecucao",
                column: "ServicoFK");

            migrationBuilder.CreateIndex(
                name: "IX_Servicos_ServicosSolicitados_ServicoFK",
                table: "Servicos_ServicosSolicitados",
                column: "ServicoFK");

            migrationBuilder.CreateIndex(
                name: "IX_Servicos_Tipos_ServicoFK",
                table: "Servicos_Tipos",
                column: "ServicoFK");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Fotografias");

            migrationBuilder.DropTable(
                name: "Galerias_Metadados");

            migrationBuilder.DropTable(
                name: "Servicos_DatasExecucao");

            migrationBuilder.DropTable(
                name: "Servicos_ServicosSolicitados");

            migrationBuilder.DropTable(
                name: "Servicos_Tipos");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ContasOnedrive");

            migrationBuilder.DropTable(
                name: "Galerias");

            migrationBuilder.DropTable(
                name: "Metadados");

            migrationBuilder.DropTable(
                name: "DataExecucao");

            migrationBuilder.DropTable(
                name: "ServicosSolicitados");

            migrationBuilder.DropTable(
                name: "Tipos");

            migrationBuilder.DropTable(
                name: "Servicos");

            migrationBuilder.DropTable(
                name: "Requerentes");
        }
    }
}
