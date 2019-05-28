using System;
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
                    Quota_Total = table.Column<int>(nullable: false),
                    Quota_Remaining = table.Column<int>(nullable: false),
                    Quota_Used = table.Column<int>(nullable: false)
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
                values: new object[] { "efbd71e2-da58-467d-b5a7-38c0fdaeb8c1", "35331636-d853-4ffd-a018-c8c09f4a5888", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "Nome" },
                values: new object[,]
                {
                    { "73a9eaf0-43f6-43a6-bf98-f0bb4e8a93b7", 0, "623fcdbd-5d02-4743-9bdc-f442e0d539fe", "Utilizador", "admin1@admin1.com", false, false, null, "ADMIN1@ADMIN1.COM", "ADMIN1@ADMIN1.COM", "AQAAAAEAACcQAAAAEOe+Tvc+2z3rvKTuxbW3CIcTnIvNyRovVM2t4Y6gh7oCnVQz6OUGWzyVK+PspLXTmQ==", null, false, "", false, "admin1@admin1.com", "Admin1" },
                    { "fcbbb3e1-e6ce-43b3-922d-f7342c59e5f1", 0, "b94b12ce-1ac9-4a3c-8789-1c55b09d8b24", "Utilizador", "user1@user1.com", false, false, null, "USER1@USER1.COM", "USER1@USER1.COM", "AQAAAAEAACcQAAAAEHl6H4bk9peUHcFAGB7NPlZu9zFl5HvmkfKeURR/jDcRoA4tIJcFsEeJMwT68S6xKw==", null, false, "", false, "user1@user1.com", "User1" }
                });

            migrationBuilder.InsertData(
                table: "DataExecucao",
                columns: new[] { "ID", "Data" },
                values: new object[,]
                {
                    { 11, new DateTime(2019, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, new DateTime(2018, 7, 21, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, new DateTime(2018, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, new DateTime(2019, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 1, new DateTime(2019, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, new DateTime(2018, 2, 27, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, new DateTime(2019, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, new DateTime(2019, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, new DateTime(2018, 9, 16, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(2019, 2, 24, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, new DateTime(2018, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) }
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
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Galeria1", 1 },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Galeria2", 1 },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Galeria3", 1 },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Galeria4", 1 }
                });

            migrationBuilder.InsertData(
                table: "Servicos_DatasExecucao",
                columns: new[] { "DataExecucaoFK", "ServicoFK" },
                values: new object[,]
                {
                    { 6, 6 },
                    { 8, 5 },
                    { 5, 5 },
                    { 8, 8 },
                    { 2, 7 },
                    { 5, 7 },
                    { 7, 7 },
                    { 2, 2 },
                    { 4, 4 },
                    { 11, 8 },
                    { 3, 3 },
                    { 1, 1 },
                    { 11, 3 },
                    { 10, 3 },
                    { 9, 3 },
                    { 10, 8 }
                });

            migrationBuilder.InsertData(
                table: "Servicos_ServicosSolicitados",
                columns: new[] { "ServicoSolicitadoFK", "ServicoFK" },
                values: new object[,]
                {
                    { 5, 5 },
                    { 2, 5 },
                    { 3, 1 },
                    { 2, 1 },
                    { 1, 8 },
                    { 3, 7 },
                    { 2, 7 },
                    { 5, 3 },
                    { 3, 8 },
                    { 1, 2 },
                    { 4, 2 },
                    { 2, 6 },
                    { 4, 4 },
                    { 3, 4 },
                    { 2, 4 },
                    { 1, 4 },
                    { 5, 7 },
                    { 4, 6 }
                });

            migrationBuilder.InsertData(
                table: "Servicos_Tipos",
                columns: new[] { "TipoFK", "ServicoFK" },
                values: new object[,]
                {
                    { 3, 6 },
                    { 2, 2 },
                    { 2, 5 },
                    { 4, 7 },
                    { 3, 4 },
                    { 1, 3 },
                    { 1, 1 },
                    { 1, 6 },
                    { 4, 8 }
                });

            migrationBuilder.InsertData(
                table: "Galerias_Metadados",
                columns: new[] { "MetadadoFK", "GaleriaFK" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 },
                    { 4, 4 }
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
