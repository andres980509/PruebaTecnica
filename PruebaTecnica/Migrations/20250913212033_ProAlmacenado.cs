using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PruebaTecnica.Migrations
{
    /// <inheritdoc />
    public partial class ProAlmacenado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
    IF OBJECT_ID('CreadoDespues', 'P') IS NULL
    BEGIN
        EXEC('
            CREATE PROCEDURE CreadoDespues
                @fecha DATETIME
            AS
            BEGIN
                SELECT *
                FROM Patients
                WHERE CreatedAt > @fecha
            END
        ')
    END
");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
