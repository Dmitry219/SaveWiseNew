using FluentMigrator;

namespace SaveWiseNew.DataAccess.Migrations;

[Migration(001)]
public class CreateUsersTable : Migration
{
    public override void Up()
    {
        Create.Table("users")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("name").AsString().NotNullable()
            .WithColumn("age").AsInt32().NotNullable()
            .WithColumn("date_created").AsDateTime().WithDefault(SystemMethods.CurrentUTCDateTime);
    }

    public override void Down()
    {
        Delete.Table("users");
    }
}
