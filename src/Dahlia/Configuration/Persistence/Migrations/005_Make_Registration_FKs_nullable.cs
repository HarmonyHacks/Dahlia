using System;
using Dahlia.Models;
using FluentMigrator;

namespace Dahlia.Configuration.Persistence.Migrations
{
    [Migration(5)]
    public class Make_registratoin_FKs_nullable : Migration
    {
        public override void Up()
        {
            Delete.Table<Registration>();
            Create.Table<Registration>()
                .WithColumn<Registration>(x => x.Id).AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Bed_Id").AsInt32().Nullable()
                .WithColumn("Participant_Id").AsInt32().Nullable()
                .WithColumn("Retreat_Id").AsInt32().Nullable();
        }

        public override void Down()
        {
            Delete.Table<Registration>();
            Create.Table<Registration>()
                .WithColumn<Registration>(x => x.Id).AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Bed_Id").AsInt32().Nullable()
                .WithColumn("Participant_Id").AsInt32().NotNullable()
                .WithColumn("Retreat_Id").AsInt32().NotNullable();
        }
    }
}