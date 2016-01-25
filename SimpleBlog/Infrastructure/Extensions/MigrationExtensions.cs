using FluentMigrator.Builders.Create.Table;

namespace SimpleBlog.Infrastructure.Extensions
{
    public static class MigrationExtensions
    {
        public static ICreateTableColumnOptionOrWithColumnSyntax WithTimestamps(this ICreateTableWithColumnSyntax that)
        {
            return that.
                WithColumn("created_at").AsDateTime().
                WithColumn("updated_at").AsDateTime().Nullable()
                .WithColumn("deleted_at").AsDateTime().Nullable();
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax WithTimestamps(this ICreateTableColumnOptionOrWithColumnSyntax that)
        {
            return that.
                WithColumn("created_at").AsDateTime().
                WithColumn("updated_at").AsDateTime().Nullable()
                .WithColumn("deleted_at").AsDateTime().Nullable();
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax AsText(this ICreateTableColumnAsTypeSyntax that)
        {
            return that.AsString(int.MaxValue);
        }
    }
}