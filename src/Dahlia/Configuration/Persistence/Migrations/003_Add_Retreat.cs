using Dahlia.Models;
using FluentMigrator;

namespace Dahlia.Configuration.Persistence.Migrations
{
    [Migration(003)]
    public class Add_Retreat : Migration
    {
        public override void Up()
        {
            Create.Table<Retreat>()
                .WithColumn<Retreat>(x => x.Id).AsInt32().Identity()
                .WithColumn<Retreat>(x => x.Description).AsString(256).Nullable()
                .WithColumn<Retreat>(x => x.StartDate).AsDateTime().NotNullable();
        }

        public override void Down()
        {
            Delete.Table<Retreat>();
        }
    }
}