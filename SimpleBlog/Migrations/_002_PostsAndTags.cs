using System.Data;
using FluentMigrator;
using SimpleBlog.Infrastructure;
using SimpleBlog.Infrastructure.Extensions;

namespace SimpleBlog.Migrations
{
    [Migration(2)]
    public class _002_PostsAndTags : Migration
    {
        public override void Up()
        {
            Create.Table("posts")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("user_id").AsInt32().ForeignKey("users", "id")
                .WithColumn("title").AsString(256)
                .WithColumn("slug").AsString(256)
                .WithColumn("content").AsText()
                .WithTimestamps();

            Create.Table("tags")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("slug").AsString(128)
                .WithColumn("name").AsString(128);

            Create.Table("posts_tags")
                .WithColumn("post_id").AsInt32().ForeignKey("posts", "id").OnDelete(Rule.Cascade)
                .WithColumn("tag_id").AsInt32().ForeignKey("tags", "id").OnDelete(Rule.Cascade);
        }

        public override void Down()
        {
            Delete.Table("posts_tags");

            Delete.Table("posts");

            Delete.Table("tags");
        }
    }
}