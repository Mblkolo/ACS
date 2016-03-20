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
        public virtual List<VaultUsers> Users { get; set; }
        public virtual DateTime OpeningTime { get; set; }
        public virtual DateTime ClosingTime { get; set; }
    }
}