using System;
using Dahlia.Models;
using FluentMigrator;

namespace Dahlia.Migrations
{
    [Migration(4)]
    public class Add_Registration : Migration
    {
        public override void Up()
        {
            Create.Table<Registration>()
                .WithColumn<Registration>(x => x.Id).AsInt32().Identity()
                .WithColumn("Bed_Id").AsInt32().Nullable()
                .WithColumn("Participant_Id").AsInt32().NotNullable()
                .WithColumn("Retreat_Id").AsInt32().NotNullable();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}