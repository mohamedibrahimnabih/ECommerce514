using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce514.Migrations
{
    /// <inheritdoc />
    public partial class AddDataToBrandModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("insert into Brands (Name, Description, Status) values ('Apple', 'Donec ut mauris eget massa tempor convallis. Nulla neque libero, convallis eget, eleifend luctus, ultricies eu, nibh. Quisque id justo sit amet sapien dignissim vestibulum. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nulla dapibus dolor vel est.', 1); insert into Brands (Name, Description, Status) values ('Samsung', 'Praesent id massa id nisl venenatis lacinia.', 1); insert into Brands (Name, Description, Status) values ('OPPO', 'Aenean fermentum. Donec ut mauris eget massa tempor convallis. Nulla neque libero, convallis eget, eleifend luctus, ultricies eu, nibh. Quisque id justo sit amet sapien dignissim vestibulum.', 0); insert into Brands (Name, Description, Status) values ('Honor', 'Phasellus sit amet erat. Nulla tempus. Vivamus in felis eu sapien cursus vestibulum.', 1); insert into Brands (Name, Description, Status) values ('Nokia', 'Quisque erat eros, viverra eget, congue eget, semper rutrum, nulla. Nunc purus. Phasellus in felis. Donec semper sapien a libero.', 1); insert into Brands (Name, Description, Status) values ('Canon', 'Nulla suscipit ligula in lacus. Curabitur at ipsum ac tellus semper interdum. Mauris ullamcorper purus sit amet nulla. Quisque arcu libero, rutrum ac, lobortis vel, dapibus at, diam. Nam tristique tortor eu pede.', 1); insert into Brands (Name, Description, Status) values ('Tecno', 'Lorem ipsum dolor sit amet, consectetuer adipiscing elit.', 1); ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("TRUNCATE TABLE Brands");
        }
    }
}
