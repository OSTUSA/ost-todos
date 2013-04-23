using FluentMigrator;

namespace Infrastructure.Migrations.Migrations
{
    [Migration(2)]
    public class CreateTodoTables : Migration
    {
        public override void Up()
        {
            Create.Table("TodoList")
                  .WithColumn("Id").AsInt64().NotNullable().PrimaryKey().Identity()
                  .WithColumn("Name").AsString(255).NotNullable()
                  .WithColumn("Owner_id").AsInt64().NotNullable();

            Create.ForeignKey("FK7EF41F0268F7285D")
                  .FromTable("TodoList")
                  .ForeignColumn("Owner_id")
                  .ToTable("User")
                  .PrimaryColumn("Id");

            Create.Table("Todo")
                  .WithColumn("Id").AsInt64().NotNullable().PrimaryKey().Identity()
                  .WithColumn("Title").AsString(255).NotNullable()
                  .WithColumn("Completed").AsBoolean().NotNullable()
                  .WithColumn("List_id").AsInt64().NotNullable();

            Create.ForeignKey("FKAED63E4EC58792D0")
                  .FromTable("Todo")
                  .ForeignColumn("List_id")
                  .ToTable("TodoList")
                  .PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.Table("Todo");
            Delete.Table("TodoList");
        }
    }
}