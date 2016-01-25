using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace SimpleBlog.Models
{
    public class Post
    {
        public virtual int Id { get; set; }
        public virtual User User { get; set; }

        public virtual string Title { get; set; }
        public virtual string Slug { get; set; }
        public virtual string Content { get; set; }

        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime? UpdatedAt { get; set; }
        public virtual DateTime? DeletedAt { get; set; }

        public virtual bool IsDeleted { get { return DeletedAt != null; }}

        public virtual IList<Tag> Tags { get; set; }

        public Post()
        {
            Tags = new List<Tag>();
        }
    }

    public class PostMap : ClassMapping<Post>
    {
        public PostMap()
        {
            Table("posts");

            Id(a => a.Id, a => a.Generator(Generators.Identity));

            ManyToOne(a => a.User, a =>
            {
                a.Column("user_id");
                a.NotNullable(true);
            });

            Property(a => a.Title, a => a.NotNullable(true));
            Property(a => a.Slug, a => a.NotNullable(true));
            Property(a => a.Content, a => a.NotNullable(true));

            Property(a => a.CreatedAt, a =>
            {
                a.NotNullable(true);
                a.Column("created_at");
            });
            Property(a => a.UpdatedAt, a => a.Column("updated_at"));
            Property(a => a.DeletedAt, a => a.Column("deleted_at"));

            Bag(a => a.Tags, a =>
            {
                a.Table("posts_tags");
                a.Key(b => b.Column("post_id"));
            }, a => a.ManyToMany(b => b.Column("tag_id")));
        }
    }
}