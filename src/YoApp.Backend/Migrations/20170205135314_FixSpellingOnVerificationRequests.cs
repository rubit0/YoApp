using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YoApp.Backend.Migrations
{
    public partial class FixSpellingOnVerificationRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VerificationtRequests",
                table: "VerificationtRequests");

            migrationBuilder.RenameTable(
                name: "VerificationtRequests",
                newName: "VerificationRequests");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VerificationRequests",
                table: "VerificationRequests",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VerificationRequests",
                table: "VerificationRequests");

            migrationBuilder.RenameTable(
                name: "VerificationRequests",
                newName: "VerificationtRequests");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VerificationtRequests",
                table: "VerificationtRequests",
                column: "Id");
        }
    }
}
