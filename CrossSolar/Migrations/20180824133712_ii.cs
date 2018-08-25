using Microsoft.EntityFrameworkCore.Migrations;

namespace CrossSolar.Migrations
{
    public partial class ii : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Serial",
                table: "Panels",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<int>(
                name: "PanelId",
                table: "OneHourElectricitys",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OneHourElectricitys_PanelId",
                table: "OneHourElectricitys",
                column: "PanelId");

            migrationBuilder.AddForeignKey(
                name: "FK_OneHourElectricitys_Panels_PanelId",
                table: "OneHourElectricitys",
                column: "PanelId",
                principalTable: "Panels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OneHourElectricitys_Panels_PanelId",
                table: "OneHourElectricitys");

            migrationBuilder.DropIndex(
                name: "IX_OneHourElectricitys_PanelId",
                table: "OneHourElectricitys");

            migrationBuilder.AlterColumn<string>(
                name: "Serial",
                table: "Panels",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 16);

            migrationBuilder.AlterColumn<string>(
                name: "PanelId",
                table: "OneHourElectricitys",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
