using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProiectMedii.Migrations
{
    /// <inheritdoc />
    public partial class newChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UtilizatorId",
                table: "Ticket",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_UtilizatorId",
                table: "Ticket",
                column: "UtilizatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_User_UtilizatorId",
                table: "Ticket",
                column: "UtilizatorId",
                principalTable: "User",
                principalColumn: "UtilizatorId");
        }
    }
}
