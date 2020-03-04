﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DrawManager.Database.SqlServer.Migrations
{
    public partial class InitialDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Draws",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    AllowMultipleParticipations = table.Column<bool>(nullable: false),
                    ProgrammedFor = table.Column<DateTime>(nullable: false),
                    ExecutedOn = table.Column<DateTime>(nullable: true),
                    GroupName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Draws", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Entrants",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Subsidiary = table.Column<string>(nullable: true),
                    Office = table.Column<string>(nullable: true),
                    Unit = table.Column<string>(nullable: true),
                    Department = table.Column<string>(nullable: true),
                    SubDepartment = table.Column<string>(nullable: true),
                    Region = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    BranchOffice = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entrants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(nullable: true),
                    Hash = table.Column<byte[]>(nullable: true),
                    Salt = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Prizes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    AttemptsUntilChooseWinner = table.Column<int>(nullable: false),
                    ExecutedOn = table.Column<DateTime>(nullable: true),
                    DrawId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prizes_Draws_DrawId",
                        column: x => x.DrawId,
                        principalTable: "Draws",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DrawEntries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DrawId = table.Column<int>(nullable: false),
                    EntrantId = table.Column<int>(nullable: false),
                    RegisteredOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrawEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DrawEntries_Draws_DrawId",
                        column: x => x.DrawId,
                        principalTable: "Draws",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DrawEntries_Entrants_EntrantId",
                        column: x => x.EntrantId,
                        principalTable: "Entrants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrizeSelectionSteps",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrizeId = table.Column<int>(nullable: false),
                    EntrantId = table.Column<int>(nullable: false),
                    DrawEntryId = table.Column<int>(nullable: false),
                    RegisteredOn = table.Column<DateTime>(nullable: false),
                    PrizeSelectionStepType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrizeSelectionSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrizeSelectionSteps_DrawEntries_DrawEntryId",
                        column: x => x.DrawEntryId,
                        principalTable: "DrawEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrizeSelectionSteps_Entrants_EntrantId",
                        column: x => x.EntrantId,
                        principalTable: "Entrants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrizeSelectionSteps_Prizes_PrizeId",
                        column: x => x.PrizeId,
                        principalTable: "Prizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DrawEntries_DrawId",
                table: "DrawEntries",
                column: "DrawId");

            migrationBuilder.CreateIndex(
                name: "IX_DrawEntries_EntrantId",
                table: "DrawEntries",
                column: "EntrantId");

            migrationBuilder.CreateIndex(
                name: "IX_Prizes_DrawId",
                table: "Prizes",
                column: "DrawId");

            migrationBuilder.CreateIndex(
                name: "IX_PrizeSelectionSteps_DrawEntryId",
                table: "PrizeSelectionSteps",
                column: "DrawEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_PrizeSelectionSteps_EntrantId",
                table: "PrizeSelectionSteps",
                column: "EntrantId");

            migrationBuilder.CreateIndex(
                name: "IX_PrizeSelectionSteps_PrizeId",
                table: "PrizeSelectionSteps",
                column: "PrizeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrizeSelectionSteps");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "DrawEntries");

            migrationBuilder.DropTable(
                name: "Prizes");

            migrationBuilder.DropTable(
                name: "Entrants");

            migrationBuilder.DropTable(
                name: "Draws");
        }
    }
}
