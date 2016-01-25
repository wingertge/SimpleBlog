using System.Collections.Generic;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace SimpleBlog.Models
{
    public class Tag
    {
        public virtual int Id { get; set; }
        public virtual string Slug { get; set; }
        public virtual string Name { get; set; }

        public virtual IList<Post> Posts { get; set; }

        public Tag()
        {
            Posts = new List<Post>();
        }
    }

    public class TagMap : ClassMapping<Tag>
    {
        public TagMap()
        {
            Table("tags");

            Id(a => a.Id, a => a.Generator(Generators.Identity));

            Property(a => a.Slug, a => a.NotNullable(true));
            Property(a => a.Name, a => a.NotNullable(true));

            Bag(a => a.Posts, a =>
            {
                a.Table("posts_tags");
                a.Key(b => b.Column("tag_id"));
            }, a => a.ManyToMany(b => b.Column("post_id")));
        }
    }
}