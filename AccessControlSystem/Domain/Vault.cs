using NHibernate.AspNet.Identity;

namespace AccessControlSystem.Domain
{
    public class Vault
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual IdentityUser User { get; set; }
    }
}