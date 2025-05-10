using FluentMigrator;

namespace SaveWise.Migrations
{
    [Migration(001)]
    public class CreateUsersTable : Migration
    {
        public override void Up()
        {
            if (!Schema.Table("users").Exists())
            {
                Create.Table("users")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("name").AsString().NotNullable()
                .WithColumn("age").AsInt32().NotNullable()
                .WithColumn("date_created").AsDateTime().WithDefault(SystemMethods.CurrentDateTime);
            }
        }

        public override void Down()
        {
            Delete.Table("users");
        }
    }
}
