using System.Collections;
using System.Collections.Generic;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace SimpleBlog.Models
{
    public class User
    {
        private const int WorkFactor = 13;

        public virtual int Id { get; set; }
        public virtual string Username { get; set; }
        public virtual string Email { get; set; }
        public virtual string PasswordHash { get; set; }

        public virtual IList<Role> Roles { get; set; }

        public User()
        {
            Roles = new List<Role>();
        }

        public virtual void SetPassword(string password)
        {
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
        }

        public virtual bool CheckPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
        }

        public static void FakeHash()
        {
            BCrypt.Net.BCrypt.HashPassword("", WorkFactor);
        }
    }

    public class UserMap : ClassMapping<User>
    {
        public UserMap()
        {
            Table("users");

            Id(a => a.Id, a => a.Generator(Generators.Identity));
            Property(a => a.Username, a => a.NotNullable(true));
            Property(a => a.Email, a => a.NotNullable(true));
            Property(a => a.PasswordHash, a => { a.Column("password_hash"); a.NotNullable(true); });

            Bag(a => a.Roles, a =>
            {
                a.Table("roles_users");
                a.Key(b => b.Column("user_id"));
            }, a => a.ManyToMany(b => b.Column("role_id")));
        }
    }
}