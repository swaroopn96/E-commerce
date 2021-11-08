using Microsoft.EntityFrameworkCore.Migrations;

namespace Ecommerce.Migrations
{
    public partial class AddApplicationTypeToProduct_Remove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE [dbo].[Products] DROP CONSTRAINT [FK_Products_ApplicationType_ApplicationTypeId]");
            migrationBuilder.Sql("DROP INDEX [IX_Products_ApplicationTypeId] ON [dbo].[Products]");
            migrationBuilder.Sql("ALTER TABLE Products DROP COLUMN ApplicationTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
