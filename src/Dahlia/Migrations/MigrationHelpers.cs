using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentMigrator.Builders;
using FluentMigrator.Builders.Create;
using FluentMigrator.Builders.Create.Table;
using FluentMigrator.Builders.Delete;
using FluentMigrator.Builders.Insert;
using FluentNHibernate.Utils;

namespace Dahlia.Migrations
{

    public static class MigrationHelpers
    {
        

        public static IInsertDataOrInSchemaSyntax IntoTable<T>(this IInsertExpressionRoot root)
        {
            return root.IntoTable(typeof (T).Name);
        }

        public static IInSchemaSyntax Table<T>(this IDeleteExpressionRoot root)
        {
            return root.Table(typeof (T).Name);
        }

        public static ICreateTableWithColumnOrSchemaSyntax Table<T>(this ICreateExpressionRoot root)
        {
            return root.Table(typeof(T).Name);
        }

        public static ICreateTableColumnAsTypeSyntax WithColumn<TObject>(this ICreateTableWithColumnOrSchemaSyntax root,
                                                                   Expression<Func<TObject, object>> expression)
        {
            return root.WithColumn(expression.ToMember().Name);
        }

        public static ICreateTableColumnAsTypeSyntax WithColumn<TObject>(this ICreateTableColumnOptionOrWithColumnSyntax root,
                                                                   Expression<Func<TObject, object>> expression)
        {
            return root.WithColumn(expression.ToMember().Name);
        }
    }
}