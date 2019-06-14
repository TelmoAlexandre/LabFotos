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
                    TokenDate = table.Column<DateTime>(nullable: false),
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
                    ID = table.Column<string>(nullable: false),
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
                    ID = table.Column<string>(nullable: false),
                    Nome = table.Column<string>(maxLength: 255, nullable: false),
                    DataDeCriacao = table.Column<DateTime>(nullable: false),
                    IdentificacaoObra = table.Column<string>(nullable: false),
                    Observacoes = table.Column<string>(maxLength: 512, nullable: true),
                    HorasEstudio = table.Column<float>(nullable: true),
                    HorasPosProducao = table.Column<float>(nullable: true),
                    DataEntrega = table.Column<DateTime>(nullable: true),
                    Total = table.Column<float>(nullable: true),
                    Hide = table.Column<bool>(nullable: false),
                    RequerenteFK = table.Column<string>(nullable: true)
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
                    ServicoFK = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Galerias", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Galerias_Servicos_ServicoFK",
                        column: x => x.ServicoFK,
                        principalTable: "Servicos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Servicos_DatasExecucao",
                columns: table => new
                {
                    DataExecucaoFK = table.Column<int>(nullable: false),
                    ServicoFK = table.Column<string>(nullable: false)
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
                    ServicoFK = table.Column<string>(nullable: false)
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
                    ServicoFK = table.Column<string>(nullable: false)
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
                    ItemId = table.Column<string>(nullable: true),
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
                values: new object[] { "efbd71e2-da58-467d-b5a7-38c0fdaeb8c1", "cb800ec3-7f15-4821-b459-ea0b8c8aa2a9", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "Nome" },
                values: new object[,]
                {
                    { "73a9eaf0-43f6-43a6-bf98-f0bb4e8a93b7", 0, "29906239-bcff-4266-a0d9-99a9bdcd2c3c", "Utilizador", "admin1@admin1.com", false, false, null, "ADMIN1@ADMIN1.COM", "ADMIN1@ADMIN1.COM", "AQAAAAEAACcQAAAAEOn7VmX3VwGnt1AquPCD9F47trRhv1YkV+eBWxlfGZKtTLoE3AgeQN9XBolBATz8JA==", null, false, "", false, "admin1@admin1.com", "Admin1" },
                    { "fcbbb3e1-e6ce-43b3-922d-f7342c59e5f1", 0, "3b1c3e24-c8cf-4289-b389-b9ae43ebc3ad", "Utilizador", "user1@user1.com", false, false, null, "USER1@USER1.COM", "USER1@USER1.COM", "AQAAAAEAACcQAAAAEM9LXeaq1mx25naN0vO9KOJlxXhtxqo9uMXX6BYIEBdCSxGjcBIdtXEDRiHegwLCZw==", null, false, "", false, "user1@user1.com", "User1" }
                });

            migrationBuilder.InsertData(
                table: "ContasOnedrive",
                columns: new[] { "ID", "AccessToken", "DriveId", "Quota_Remaining", "Quota_Total", "Quota_Used", "RefreshToken", "TokenDate" },
                values: new object[] { 1, "eyJ0eXAiOiJKV1QiLCJub25jZSI6IkFRQUJBQUFBQUFEQ29NcGpKWHJ4VHE5Vkc5dGUtN0ZYanFlQnJmd0RNOHRIbzdVaExaTGdSN25mcnlhbjZxQ1d3aDYxTWt1dkdWdWo0MzAtQjZzSmRfS1hpNnZqT0lBQU1iel84cGJ5VFpOeVJnMlBMVXM4TkNBQSIsImFsZyI6IlJTMjU2IiwieDV0IjoiSEJ4bDltQWU2Z3hhdkNrY29PVTJUSHNETmEwIiwia2lkIjoiSEJ4bDltQWU2Z3hhdkNrY29PVTJUSHNETmEwIn0.eyJhdWQiOiJodHRwczovL2dyYXBoLm1pY3Jvc29mdC5jb20iLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC8yMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAvIiwiaWF0IjoxNTU5MjMzNDg0LCJuYmYiOjE1NTkyMzM0ODQsImV4cCI6MTU1OTIzNzM4NCwiYWNjdCI6MCwiYWNyIjoiMSIsImFpbyI6IkFTUUEyLzhMQUFBQXNjNHl5bjlWM010SWJjUjBUZDUzZDdmcTVQQjBUU1d5QmE5bjVablJjQ289IiwiYW1yIjpbInB3ZCJdLCJhcHBfZGlzcGxheW5hbWUiOiJMYWJGb3RvIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJhcHBpZGFjciI6IjEiLCJmYW1pbHlfbmFtZSI6IkFsZXhhbmRyZSIsImdpdmVuX25hbWUiOiJUZWxtbyIsImlwYWRkciI6IjE4OC4yNTEuMjI2LjEzOCIsIm5hbWUiOiJUZWxtbyBPbGl2ZWlyYSBBbGV4YW5kcmUiLCJvaWQiOiJjNzJkZTU2Yi01ZGZlLTQzMjgtOTMwZC0wNGEzZDcyZjI1MzMiLCJvbnByZW1fc2lkIjoiUy0xLTUtMjEtMzEwMDY3Mzc0Ni00MjcwNTgzNjI2LTI0MjUyNDIwNDktMTc4MjkiLCJwbGF0ZiI6IjMiLCJwdWlkIjoiMTAwMzdGRkU5MDg5NDYzNCIsInNjcCI6IkZpbGVzLlJlYWQgRmlsZXMuUmVhZC5BbGwgRmlsZXMuUmVhZFdyaXRlIEZpbGVzLlJlYWRXcml0ZS5BbGwgcHJvZmlsZSBvcGVuaWQgZW1haWwiLCJzaWduaW5fc3RhdGUiOlsia21zaSJdLCJzdWIiOiJZdVJhQVBLRVlKeU94eXlFY21kUjJNZG5hTE85NDk1QTNkQ2dFWlQ5MzlBIiwidGlkIjoiMjFlOTBkZmMtNTRmMS00YjIxLThmM2ItN2ZiOTc5OGVkMmUwIiwidW5pcXVlX25hbWUiOiJhbHVubzE5MDg5QGlwdC5wdCIsInVwbiI6ImFsdW5vMTkwODlAaXB0LnB0IiwidXRpIjoiVV9GbXNLblI3a20tMWVPcUZOeXZBQSIsInZlciI6IjEuMCIsInhtc19zdCI6eyJzdWIiOiJld2hsazdSYmhWN25YbTc0dTVTQUlQTjcwYktpbnBMWE1OQlB3VEFTUXRZIn0sInhtc190Y2R0IjoxNDI5MjY3MjA4fQ.RxpyPNjTQaRlpAmM9BaKnU4zGoCry1_JZ-k4F-ADhlKoK-SRDdt6UhPfoysq4vxDrwCD5SSJwf93ygV2sM19etyHAKwS_ZWFDjrpyXEq0aftm_6A-LVTrI_KmjX3u64b8joyGGailTcMBc1gkk21jKt6nnoXNJuKXNXrK3mQGSI7xosG9WOmbWB54VoTCDEY0rUgIkO5_e0R9bIXURShNgiNgV5GpPhjAV1Oo3qWPEVrR41-8LkLMKqFbtLFfPouk7C4KlOB6AWb69Qew02h1KqIir_Xe3m1xiNqQ1l_bM2dYGdNgvGhoXQgge4JzGkrINOAkOp9KhuS_bRl7Cvjdw", "b!0812_G3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29-ASFmlYSqg3p9xBheG7", "5497452095469", "5497558138880", "97597600", "OAQABAAAAAADCoMpjJXrxTq9VG9te-7FXmM_2aSL25b4X-gFBQOTiVTf4ph2zQdl02erPQ3k02lPQlapDOFcjBLEcumEFhUTuy70kmHovua_emPvKZa19egLTTDt8F6IfIJ8g0hfsOk79EQenHJWd5lmcFU5Bpu2H4jXkvcfDSrbwza_HXp40bXuG-ZUE4BHwytKwk9n3X06xo6dxxuaQ_8CsW1MEXSy-nJpK9E1Wn3e1z4twuo15ejemgOFzD4_VYPMKlKHJXqzYhC5WMLk1-oA-dYaZ9g5TdEr3nJKeYYvxWnXP420nnLq8tLLR-Hu7CHoPRYeTzh-5_TWd3Os6SJXO_ucP-BETYvEoVRJVjfA8RvJz9JITqY2Rihkd1S28oe4mWyKg6PcbrbewiW7B8kGdKh2So2tPoTZw-vI_1CRV6X8m72zizO5QL6NZX2Egs6lRMMEWm3ZhvJYbFwuaR6x63ocYPbylFU1ll8x6iF2FBhVjrjVfn9B3Vj967J3WR2TB-ayYG8kGyXgHkXr1P0IRdAlLHoK93K1gRinjqXq3_qqARWta78NcJQ0q9g1fsuju6uGgVfShwMzloSZXlBQXMCPKp1Orkjs__5AJApgD5PsVPWMhTxkN3KSUiXU3liEYBuPUHi27s_jU4ZU9Id2c0-ItUjoa3S5IXcZ9W2IA-RCKgTzmeLk4Fs8_0M0-h07Yy6SnB_xq_jp7Ox-2RwQpwbmRxfmpqpCr9lY-7qF7Xer6E9GOKZrbivoqXmszEbK7H-fjeg43dpivTkH84Zwvb5natg8JhV2RBGK5YKBL2R4k04H30id9YdF34T_s-7FlEikGh7dZgv8XkVnbJbGjrfyPGZx7HfeqOGdK9Y3jjMXtcOb6SCAA", new DateTime(2019, 5, 29, 12, 0, 0, 0, DateTimeKind.Unspecified) });

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
                    { "916d75a4-c12e-40c4-bf29-7ec1e31696ac", "manuel@ipt.pt", "Manuel", "Manuel", "987321546" },
                    { "86dafe89-cc9c-4308-ace8-b3ed1f54a346", "joao@ipt.pt", "João", "Maria", "987654321" },
                    { "56d513fa-cedd-40d9-bd58-12a7ee3f129c", "maria@ipt.pt", "Maria", "Maria", "987654321" },
                    { "d6e3fca1-c766-4333-8211-f63431b30181", "jose@ipt.pt", "José", "José", "123789456" },
                    { "a0f118c8-8e40-4433-a695-e5ca01788331", "fernando@ipt.pt", "Fernando", "Fernando", "123456789" }
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
                    { "a0f118c8-8e40-4433-a695-e5ca01788331", new DateTime(2018, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2018, 6, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 2f, 8f, "Tira larga, de tecido ou de madeira, que se dispõe transversalmente como ornato na parte superior de uma cortina.", "Sanefa", "Sanefa degradada na parte superior", "a0f118c8-8e40-4433-a695-e5ca01788331", 40f },
                    { "86dafe89-cc9c-4308-ace8-b3ed1f54a346", new DateTime(2019, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2019, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 4f, 10f, "Vasos/Jarrões a parecem integrados em retábulos,tronos, mesas de altar em várias igrejas em Tomar.", "Vaso talha prata dourada", "", "a0f118c8-8e40-4433-a695-e5ca01788331", 45f },
                    { "d6e3fca1-c766-4333-8211-f63431b30181", new DateTime(2017, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2017, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 20f, 43f, "Seis cadeiras em madeira de cerejeira com acabamento em verniz", "Conjunto de Cadeiras", "", "a0f118c8-8e40-4433-a695-e5ca01788331", 125f },
                    { "56d513fa-cedd-40d9-bd58-12a7ee3f129c", new DateTime(2019, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2019, 3, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 6f, 17f, "Plantação de árvores na escola.", "Dia da árvore", "Presença do presidente nas plantações.", "56d513fa-cedd-40d9-bd58-12a7ee3f129c", 65f },
                    { "0aa61784-ccc0-4dcd-9722-8fdfd3a1e298", new DateTime(2018, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2018, 2, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 6f, 2f, "Um cavalo queimado no matadouro", "Estátua do Cavalo queimado", "", "56d513fa-cedd-40d9-bd58-12a7ee3f129c", 0f },
                    { "916d75a4-c12e-40c4-bf29-7ec1e31696ac", new DateTime(2018, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, 3f, 4f, "Festa tradicional da aldeia de Azinhaga", "Festa do Bodo", "Muito bom", "86dafe89-cc9c-4308-ace8-b3ed1f54a346", 0f },
                    { "aca4875a-721e-4cfc-827d-d48c7050b543", new DateTime(2019, 11, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, 0f, 0f, "Fotos da tabanca do Maltez", "Feira do Cavalo, Golegã", "Grande festa, aprovado", "d6e3fca1-c766-4333-8211-f63431b30181", 0f },
                    { "b399e9c7-957e-4e85-9474-bad2cf8032c4", new DateTime(2019, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, 4f, 2f, "Exposição na galeria de Sintra", "Exposição de Arte contemporânea", "", "916d75a4-c12e-40c4-bf29-7ec1e31696ac", 0f }
                });

            migrationBuilder.InsertData(
                table: "Galerias",
                columns: new[] { "ID", "DataDeCriacao", "Nome", "ServicoFK" },
                values: new object[,]
                {
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Galeria2", "a0f118c8-8e40-4433-a695-e5ca01788331" },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Galeria3", "a0f118c8-8e40-4433-a695-e5ca01788331" },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Galeria4", "a0f118c8-8e40-4433-a695-e5ca01788331" },
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Galeria1", "aca4875a-721e-4cfc-827d-d48c7050b543" }
                });

            migrationBuilder.InsertData(
                table: "Servicos_DatasExecucao",
                columns: new[] { "DataExecucaoFK", "ServicoFK" },
                values: new object[,]
                {
                    { 10, "b399e9c7-957e-4e85-9474-bad2cf8032c4" },
                    { 11, "b399e9c7-957e-4e85-9474-bad2cf8032c4" },
                    { 8, "b399e9c7-957e-4e85-9474-bad2cf8032c4" },
                    { 6, "aca4875a-721e-4cfc-827d-d48c7050b543" },
                    { 8, "916d75a4-c12e-40c4-bf29-7ec1e31696ac" },
                    { 5, "916d75a4-c12e-40c4-bf29-7ec1e31696ac" },
                    { 2, "0aa61784-ccc0-4dcd-9722-8fdfd3a1e298" },
                    { 5, "0aa61784-ccc0-4dcd-9722-8fdfd3a1e298" },
                    { 2, "56d513fa-cedd-40d9-bd58-12a7ee3f129c" },
                    { 7, "0aa61784-ccc0-4dcd-9722-8fdfd3a1e298" },
                    { 3, "86dafe89-cc9c-4308-ace8-b3ed1f54a346" },
                    { 9, "86dafe89-cc9c-4308-ace8-b3ed1f54a346" },
                    { 1, "a0f118c8-8e40-4433-a695-e5ca01788331" },
                    { 10, "86dafe89-cc9c-4308-ace8-b3ed1f54a346" },
                    { 4, "d6e3fca1-c766-4333-8211-f63431b30181" },
                    { 11, "86dafe89-cc9c-4308-ace8-b3ed1f54a346" }
                });

            migrationBuilder.InsertData(
                table: "Servicos_ServicosSolicitados",
                columns: new[] { "ServicoSolicitadoFK", "ServicoFK" },
                values: new object[,]
                {
                    { 1, "d6e3fca1-c766-4333-8211-f63431b30181" },
                    { 5, "916d75a4-c12e-40c4-bf29-7ec1e31696ac" },
                    { 2, "916d75a4-c12e-40c4-bf29-7ec1e31696ac" },
                    { 2, "a0f118c8-8e40-4433-a695-e5ca01788331" },
                    { 3, "0aa61784-ccc0-4dcd-9722-8fdfd3a1e298" },
                    { 2, "0aa61784-ccc0-4dcd-9722-8fdfd3a1e298" },
                    { 5, "0aa61784-ccc0-4dcd-9722-8fdfd3a1e298" },
                    { 5, "86dafe89-cc9c-4308-ace8-b3ed1f54a346" },
                    { 3, "b399e9c7-957e-4e85-9474-bad2cf8032c4" },
                    { 3, "a0f118c8-8e40-4433-a695-e5ca01788331" },
                    { 1, "56d513fa-cedd-40d9-bd58-12a7ee3f129c" },
                    { 4, "56d513fa-cedd-40d9-bd58-12a7ee3f129c" },
                    { 1, "b399e9c7-957e-4e85-9474-bad2cf8032c4" },
                    { 4, "d6e3fca1-c766-4333-8211-f63431b30181" },
                    { 3, "d6e3fca1-c766-4333-8211-f63431b30181" },
                    { 2, "d6e3fca1-c766-4333-8211-f63431b30181" },
                    { 4, "aca4875a-721e-4cfc-827d-d48c7050b543" },
                    { 2, "aca4875a-721e-4cfc-827d-d48c7050b543" }
                });

            migrationBuilder.InsertData(
                table: "Servicos_Tipos",
                columns: new[] { "TipoFK", "ServicoFK" },
                values: new object[,]
                {
                    { 1, "aca4875a-721e-4cfc-827d-d48c7050b543" },
                    { 4, "0aa61784-ccc0-4dcd-9722-8fdfd3a1e298" },
                    { 2, "916d75a4-c12e-40c4-bf29-7ec1e31696ac" },
                    { 3, "0aa61784-ccc0-4dcd-9722-8fdfd3a1e298" },
                    { 2, "56d513fa-cedd-40d9-bd58-12a7ee3f129c" },
                    { 1, "86dafe89-cc9c-4308-ace8-b3ed1f54a346" },
                    { 3, "d6e3fca1-c766-4333-8211-f63431b30181" },
                    { 1, "a0f118c8-8e40-4433-a695-e5ca01788331" },
                    { 4, "b399e9c7-957e-4e85-9474-bad2cf8032c4" }
                });

            migrationBuilder.InsertData(
                table: "Fotografias",
                columns: new[] { "ID", "ContaOnedriveFK", "DownloadUrl", "Formato", "FotografiaOrigemFK", "FotografiaOrigemID", "GaleriaFK", "ItemId", "Nome", "Thumbnail_Large", "Thumbnail_Medium", "Thumbnail_Small" },
                values: new object[,]
                {
                    { 1, 1, "https://politecnicotomar-my.sharepoint.com/personal/aluno19089_ipt_pt/_layouts/15/download.aspx?UniqueId=bcb52594-9d40-43dc-960a-ab54a2e4d563&Translate=false&tempauth=eyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0.eyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUwOTAyIiwiZXhwIjoiMTU1OTE1NDUwMiIsImVuZHBvaW50dXJsIjoiVXQzdlgyZ0JGSmxnRzROWU5GbHdmcGtjZTc3WVNjeGtBQ0VhVGRtTUVzZz0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE1NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiTVRnNE5qUTBPRFF0TTJGbE5DMDBOV014TFRrek1qY3RORFV3TkdVNVl6QmhNak5tIiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ.clFWUUJxWHI3Q1pTcFNUUklSaWh6VW5mOStkWUMwUzFtM2pONExibVVUYz0&ApiVersion=2.0", "CR2", 0, null, 1, "0127OBJ5MUEW23YQE53RBZMCVLKSROJVLD", "test1.CR2", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5MUEW23YQE53RBZMCVLKSROJVLD%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiT2FpVXNURDJrMFkyN1ZSakEzOW1PelZNa0lzOXR1aDNteGIyTGNkQytWUT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EcjdWV21sNFBIYThxcU9RcmtmQmpoNS9XdE5yU01GY0lyWndoNy85VnlDQT0%26version%3DPublished&width=800&height=800", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5MUEW23YQE53RBZMCVLKSROJVLD%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiT2FpVXNURDJrMFkyN1ZSakEzOW1PelZNa0lzOXR1aDNteGIyTGNkQytWUT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EcjdWV21sNFBIYThxcU9RcmtmQmpoNS9XdE5yU01GY0lyWndoNy85VnlDQT0%26version%3DPublished&width=176&height=176", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5MUEW23YQE53RBZMCVLKSROJVLD%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiT2FpVXNURDJrMFkyN1ZSakEzOW1PelZNa0lzOXR1aDNteGIyTGNkQytWUT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EcjdWV21sNFBIYThxcU9RcmtmQmpoNS9XdE5yU01GY0lyWndoNy85VnlDQT0%26version%3DPublished&width=96&height=96" },
                    { 2, 1, "https://politecnicotomar-my.sharepoint.com/personal/aluno19089_ipt_pt/_layouts/15/download.aspx?UniqueId=307ec159-829e-423a-a366-e70167f59c01&Translate=false&tempauth=eyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0.eyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE1NTE3MyIsImVuZHBvaW50dXJsIjoiN01BcEUvbS81eGM2VkJtVmY5cFd1ai95WXFmQmZmQS9HTzRhWmNsb0RXWT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE1NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ.REkwWVUwU1R3RkYxZlNRQXdML3Z3L2Y5WC9YUWlkSGU1Qy9ZakwyNS9FND0&ApiVersion=2.0", "CR2", 0, null, 1, "0127OBJ5KZYF7DBHUCHJBKGZXHAFT7LHAB", "test2.CR2", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5KZYF7DBHUCHJBKGZXHAFT7LHAB%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoibTdaVkhNaTltSjVPYXUybjBIcDFkQnFySGIxYVdKdTNxYVg1cGtpeXhxWT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EVkxmYjM5aE5QaG9jZXd6RjJhRU80UVdJc2NWS3pTNC9YWGMzVlg1RE9Wbz0%26version%3DPublished&width=800&height=800", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5KZYF7DBHUCHJBKGZXHAFT7LHAB%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoibTdaVkhNaTltSjVPYXUybjBIcDFkQnFySGIxYVdKdTNxYVg1cGtpeXhxWT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EVkxmYjM5aE5QaG9jZXd6RjJhRU80UVdJc2NWS3pTNC9YWGMzVlg1RE9Wbz0%26version%3DPublished&width=176&height=176", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5KZYF7DBHUCHJBKGZXHAFT7LHAB%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoibTdaVkhNaTltSjVPYXUybjBIcDFkQnFySGIxYVdKdTNxYVg1cGtpeXhxWT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EVkxmYjM5aE5QaG9jZXd6RjJhRU80UVdJc2NWS3pTNC9YWGMzVlg1RE9Wbz0%26version%3DPublished&width=96&height=96" },
                    { 3, 1, "https://politecnicotomar-my.sharepoint.com/personal/aluno19089_ipt_pt/_layouts/15/download.aspx?UniqueId=96c99233-5d31-4577-b8ef-fb9f004ca4ef&Translate=false&tempauth=eyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0.eyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE1NTE3MyIsImVuZHBvaW50dXJsIjoiNFlQVnAya0tWWjh1N1daQ1B5NEQrNGtvcFllS2dPcy9FOVVmblZpTnJLVT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE1NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ.dEFTU0k3TlZOa052ZkxjcXlDdnBMYkxGZWV0NEIvZ2FwMjRld3I4ZmNzRT0&ApiVersion=2.0", "CR2", 0, null, 1, "0127OBJ5JTSLEZMMK5O5C3R373T4AEZJHP", "test3.CR2", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5JTSLEZMMK5O5C3R373T4AEZJHP%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiWWpkcVhnaklBejVSUEpyVmVuL0pxN28wMG9mOUhRaHNWdlB0TWxsRVA0Zz0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2ESHJBNTR0UTB1Wkx5K05UN2xmdHFBRTYvSlVsK1k3MmJTZUtHNXp2NS9BQT0%26version%3DPublished&width=800&height=800", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5JTSLEZMMK5O5C3R373T4AEZJHP%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiWWpkcVhnaklBejVSUEpyVmVuL0pxN28wMG9mOUhRaHNWdlB0TWxsRVA0Zz0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2ESHJBNTR0UTB1Wkx5K05UN2xmdHFBRTYvSlVsK1k3MmJTZUtHNXp2NS9BQT0%26version%3DPublished&width=176&height=176", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5JTSLEZMMK5O5C3R373T4AEZJHP%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiWWpkcVhnaklBejVSUEpyVmVuL0pxN28wMG9mOUhRaHNWdlB0TWxsRVA0Zz0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2ESHJBNTR0UTB1Wkx5K05UN2xmdHFBRTYvSlVsK1k3MmJTZUtHNXp2NS9BQT0%26version%3DPublished&width=96&height=96" },
                    { 4, 1, "https://politecnicotomar-my.sharepoint.com/personal/aluno19089_ipt_pt/_layouts/15/download.aspx?UniqueId=2eebb9a9-d586-43f4-8c78-bbd150d06e47&Translate=false&tempauth=eyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0.eyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE1NTE3MyIsImVuZHBvaW50dXJsIjoiRkUzYVJiK2NPN055WitKNlZ0cG15VXI1RnpMUGw3SUYxMDBjRXh5YjFvbz0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE1NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ.N2lQT09ubjZJREJ1NTNkUWhJMTlaRzFsNG13WlFUcXEwU0NEZC80VG8rWT0&ApiVersion=2.0", "CR2", 0, null, 1, "0127OBJ5NJXHVS5BWV6RBYY6F32FINA3SH", "test4.CR2", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5NJXHVS5BWV6RBYY6F32FINA3SH%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiY05uYmM2Y0NvamxNMTl2SndMQmFPNUVWcGJkdDB3cWtMVWtmTXJjekduRT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EUXI4VXlDYWt4VC9GMGFkSkR2OG1CMGduOTFIVFl4cUg3L2k0eWdpbjh3MD0%26version%3DPublished&width=800&height=800", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5NJXHVS5BWV6RBYY6F32FINA3SH%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiY05uYmM2Y0NvamxNMTl2SndMQmFPNUVWcGJkdDB3cWtMVWtmTXJjekduRT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EUXI4VXlDYWt4VC9GMGFkSkR2OG1CMGduOTFIVFl4cUg3L2k0eWdpbjh3MD0%26version%3DPublished&width=176&height=176", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5NJXHVS5BWV6RBYY6F32FINA3SH%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiY05uYmM2Y0NvamxNMTl2SndMQmFPNUVWcGJkdDB3cWtMVWtmTXJjekduRT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EUXI4VXlDYWt4VC9GMGFkSkR2OG1CMGduOTFIVFl4cUg3L2k0eWdpbjh3MD0%26version%3DPublished&width=96&height=96" },
                    { 5, 1, "https://politecnicotomar-my.sharepoint.com/personal/aluno19089_ipt_pt/_layouts/15/download.aspx?UniqueId=06711508-d1b6-41d0-baed-428b27f076c5&Translate=false&tempauth=eyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0.eyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE1NTE3MyIsImVuZHBvaW50dXJsIjoiMzBDRXRtSVYrTWNUWWcrTDVFeDJ3a21tdTZGc05KSmMwVVJKeFhIczZyTT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE1NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ.a05BY29NYnpZR0RUaHZ3all5N3FQNllQOWplL3lSVko5UmFLb0YwblMwUT0&ApiVersion=2.0", "CR2", 0, null, 1, "0127OBJ5IICVYQNNWR2BA3V3KCRMT7A5WF", "test5.CR2", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5IICVYQNNWR2BA3V3KCRMT7A5WF%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoicHo5VjVONGJuVXNpdk1IY0hLc1BJd3ExZHA0SVJVVHJiRVhYb2lDOWQxMD0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EU3RVNDJJMmhxRk5iQTBsYlpQMnB6d1RoTzYxeWdjc3JoTU9RandYQkJPdz0%26version%3DPublished&width=800&height=800", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5IICVYQNNWR2BA3V3KCRMT7A5WF%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoicHo5VjVONGJuVXNpdk1IY0hLc1BJd3ExZHA0SVJVVHJiRVhYb2lDOWQxMD0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EU3RVNDJJMmhxRk5iQTBsYlpQMnB6d1RoTzYxeWdjc3JoTU9RandYQkJPdz0%26version%3DPublished&width=176&height=176", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5IICVYQNNWR2BA3V3KCRMT7A5WF%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoicHo5VjVONGJuVXNpdk1IY0hLc1BJd3ExZHA0SVJVVHJiRVhYb2lDOWQxMD0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EU3RVNDJJMmhxRk5iQTBsYlpQMnB6d1RoTzYxeWdjc3JoTU9RandYQkJPdz0%26version%3DPublished&width=96&height=96" },
                    { 6, 1, "https://politecnicotomar-my.sharepoint.com/personal/aluno19089_ipt_pt/_layouts/15/download.aspx?UniqueId=514096aa-0741-4ad9-ad5d-9f90627dbb04&Translate=false&tempauth=eyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0.eyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE1NTE3MyIsImVuZHBvaW50dXJsIjoiRTc1RlVINlpDQkZHNERtY0NYOHJ1K2luVnA3MERCbll6eUlqWFFlT3h2TT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE1NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ.cHkwcXIvU21LUmtRUjZveHFsY1dMVFhPbTJxVzcwTlNOOHAxd2hzYTVTTT0&ApiVersion=2.0", "CR2", 0, null, 1, "0127OBJ5NKSZAFCQIH3FFK2XM7SBRH3OYE", "test6.CR2", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5NKSZAFCQIH3FFK2XM7SBRH3OYE%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiRTlXZ1REajNuQ0Y4cTIrTTg3YTFtYnhrRm5TaTZ6NmwraFcxM2c2SFJhWT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2ENTR1NlRRWHBHZmg4c1ZVZ0lLaVZuSW0xRm9qV1ZpRzVGclBiSU5WbU1Naz0%26version%3DPublished&width=800&height=800", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5NKSZAFCQIH3FFK2XM7SBRH3OYE%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiRTlXZ1REajNuQ0Y4cTIrTTg3YTFtYnhrRm5TaTZ6NmwraFcxM2c2SFJhWT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2ENTR1NlRRWHBHZmg4c1ZVZ0lLaVZuSW0xRm9qV1ZpRzVGclBiSU5WbU1Naz0%26version%3DPublished&width=176&height=176", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5NKSZAFCQIH3FFK2XM7SBRH3OYE%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiRTlXZ1REajNuQ0Y4cTIrTTg3YTFtYnhrRm5TaTZ6NmwraFcxM2c2SFJhWT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2ENTR1NlRRWHBHZmg4c1ZVZ0lLaVZuSW0xRm9qV1ZpRzVGclBiSU5WbU1Naz0%26version%3DPublished&width=96&height=96" },
                    { 7, 1, "https://politecnicotomar-my.sharepoint.com/personal/aluno19089_ipt_pt/_layouts/15/download.aspx?UniqueId=6ed6d15d-bff2-456c-88c2-2ca9494817e9&Translate=false&tempauth=eyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0.eyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE1NTE3MyIsImVuZHBvaW50dXJsIjoiaWIvVVBVT0hrcU1PYmREMWZBdzFOaFNBTjBFRjNGNTQ1K3JYb2VrK1ZiTT0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE1NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ.R1lGa0JDU0s4d1N5cXF2SnNiMzB4UUhUMUVsV0tJR212dmsrWEpjS1F0bz0&ApiVersion=2.0", "CR2", 0, null, 1, "0127OBJ5K52HLG54V7NRCYRQRMVFEUQF7J", "test7.CR2", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5K52HLG54V7NRCYRQRMVFEUQF7J%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiQ1hTb25TMS9oQzVZNzBVK2pQODlwMHdGaTJnMW44MmlXTjI3VSswakZXaz0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EK3l2bllKTFRmS0paWlEwUXpuT3dHMEZmQXFvU0xka01XdTRyVzZXbEswWT0%26version%3DPublished&width=800&height=800", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5K52HLG54V7NRCYRQRMVFEUQF7J%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiQ1hTb25TMS9oQzVZNzBVK2pQODlwMHdGaTJnMW44MmlXTjI3VSswakZXaz0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EK3l2bllKTFRmS0paWlEwUXpuT3dHMEZmQXFvU0xka01XdTRyVzZXbEswWT0%26version%3DPublished&width=176&height=176", "https://westeurope1-mediap.svc.ms/transform/thumbnail?provider=spo&inputFormat=CR2&cs=MmRhNzQ4NGMtOWVlYS00OWEzLWIzMzctZjU5YTk3Zjc5ZTQ3fFNQTw&correlationId=d2214e00-d015-487b-bd7c-7a622e18b441&docid=https%3A%2F%2Fpolitecnicotomar%2Dmy%2Esharepoint%2Ecom%2F%5Fapi%2Fv2%2E0%2Fdrives%2Fb%210812%5FG3q10uofJYDjbiF50gxK5lECPtEqi3cKXzbQsT29%2DASFmlYSqg3p9xBheG7%2Fitems%2F0127OBJ5K52HLG54V7NRCYRQRMVFEUQF7J%3Ftempauth%3DeyJ0eXAiOiJKV1QiLCJhbGciOiJub25lIn0%2EeyJhdWQiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAvcG9saXRlY25pY290b21hci1teS5zaGFyZXBvaW50LmNvbUAyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJpc3MiOiIwMDAwMDAwMy0wMDAwLTBmZjEtY2UwMC0wMDAwMDAwMDAwMDAiLCJuYmYiOiIxNTU5MTUxNTczIiwiZXhwIjoiMTU1OTE3MzE3MyIsImVuZHBvaW50dXJsIjoiQ1hTb25TMS9oQzVZNzBVK2pQODlwMHdGaTJnMW44MmlXTjI3VSswakZXaz0iLCJlbmRwb2ludHVybExlbmd0aCI6IjE2NyIsImlzbG9vcGJhY2siOiJUcnVlIiwiY2lkIjoiWkRJeU1UUmxNREF0WkRBeE5TMDBPRGRpTFdKa04yTXROMkUyTWpKbE1UaGlORFF4IiwidmVyIjoiaGFzaGVkcHJvb2Z0b2tlbiIsInNpdGVpZCI6IlptTTNObU5rWkRNdFpXRTJaQzAwWW1RM0xXRTROMk10T1RZd016aGtZamc0TldVMyIsImFwcF9kaXNwbGF5bmFtZSI6IkxhYkZvdG8iLCJzaWduaW5fc3RhdGUiOiJbXCJrbXNpXCJdIiwiYXBwaWQiOiIyZGE3NDg0Yy05ZWVhLTQ5YTMtYjMzNy1mNTlhOTdmNzllNDciLCJ0aWQiOiIyMWU5MGRmYy01NGYxLTRiMjEtOGYzYi03ZmI5Nzk4ZWQyZTAiLCJ1cG4iOiJhbHVubzE5MDg5QGlwdC5wdCIsInB1aWQiOiIxMDAzN0ZGRTkwODk0NjM0IiwiY2FjaGVrZXkiOiIwaC5mfG1lbWJlcnNoaXB8MTAwMzdmZmU5MDg5NDYzNEBsaXZlLmNvbSIsInNjcCI6Im15ZmlsZXMucmVhZCBhbGxmaWxlcy5yZWFkIG15ZmlsZXMud3JpdGUgYWxsZmlsZXMud3JpdGUiLCJ0dCI6IjIiLCJ1c2VQZXJzaXN0ZW50Q29va2llIjpudWxsfQ%2EK3l2bllKTFRmS0paWlEwUXpuT3dHMEZmQXFvU0xka01XdTRyVzZXbEswWT0%26version%3DPublished&width=96&height=96" }
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
