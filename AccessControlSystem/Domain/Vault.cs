using NHibernate.AspNet.Identity;
using System;
using System.Collections.Generic;

namespace AccessControlSystem.Domain
{
    public class Vault
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual IdentityUser Admin { get; set; }
        public virtual ICollection<VaultUsers> Users { get; set; }
        public virtual int? OpeningTime { get; set; }
        public virtual int? ClosingTime { get; set; }
    }
}