using Dahlia.Models;
using FluentMigrator;

namespace Dahlia.Configuration.Persistence.Migrations
{
    [Migration(001)]
    public class Add_Beds : Migration
    {
        public override void Up()
        {
            Create.Table<Bed>()
                .WithColumn<Bed>(x => x.Id).AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn<Bed>(x => x.Code).AsAnsiString(128).NotNullable()
                .WithColumn<Bed>(x => x.IsUpstairs).AsBoolean().NotNullable();

            Insert.IntoTable<Bed>()
                .Row(new {Code = "GH1-1", IsUpstairs = false})
                .Row(new {Code = "GH1-2", IsUpstairs = false})
                .Row(new {Code = "GH2-1", IsUpstairs = false})
                .Row(new {Code = "GH2-2", IsUpstairs = false})
                .Row(new {Code = "GH3-1", IsUpstairs = false})
                .Row(new {Code = "GH3-2", IsUpstairs = false})
                .Row(new {Code = "CS2-1", IsUpstairs = false})
                .Row(new {Code = "CS2-2", IsUpstairs = false})
                .Row(new {Code = "CS3-1", IsUpstairs = false})
                .Row(new {Code = "CS3-2", IsUpstairs = false})
                .Row(new {Code = "CS4-1", IsUpstairs = false})
                .Row(new {Code = "CS4-2", IsUpstairs = false})
                .Row(new {Code = "CS5-1", IsUpstairs = false})
                .Row(new {Code = "CS5-2", IsUpstairs = false})
                .Row(new {Code = "CS6-1", IsUpstairs = true})
                .Row(new {Code = "CS6-2", IsUpstairs = true})
                .Row(new {Code = "CS7-1", IsUpstairs = true})
                .Row(new {Code = "CS7-2", IsUpstairs = true})
                .Row(new {Code = "L1-1'", IsUpstairs = false})
                .Row(new {Code = "L2-1'", IsUpstairs = false})
                .Row(new {Code = "L2-2'", IsUpstairs = false})
                .Row(new {Code = "C1-1'", IsUpstairs = false})
                .Row(new {Code = "C1-2'", IsUpstairs = false})
                .Row(new {Code = "C2-1'", IsUpstairs = false})
                .Row(new {Code = "C2-2'", IsUpstairs = false})
                .Row(new {Code = "C3-1'", IsUpstairs = false})
                .Row(new {Code = "C3-2'", IsUpstairs = false})
                .Row(new {Code = "C4-1'", IsUpstairs = false})
                .Row(new {Code = "C4-2'", IsUpstairs = false});
        }

        public override void Down()
        {
            Delete.Table<Bed>();
        }
    }
}