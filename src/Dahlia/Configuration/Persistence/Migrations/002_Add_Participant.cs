using Dahlia.Models;
using FluentMigrator;

namespace Dahlia.Configuration.Persistence.Migrations
{
    [Migration(002)]
    public class Add_Participant : Migration
    {
        public override void Up()
        {
            Create.Table<Participant>()
                .WithColumn<Participant>(x => x.Id).AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn<Participant>(x => x.FirstName).AsString(128).NotNullable()
                .WithColumn<Participant>(x => x.LastName).AsString().NotNullable()
                .WithColumn<Participant>(x => x.DateReceived).AsDateTime().NotNullable()
                .WithColumn<Participant>(x => x.Notes).AsString(8000).Nullable()
                .WithColumn<Participant>(x => x.PhysicalStatus).AsString(128).NotNullable();
        }

        public override void Down()
        {
            Delete.Table<Participant>();
        }
    }
}