using NHibernate.AspNet.Identity;
using System;
using System.Collections.Generic;

namespace AccessControlSystem.Domain
{
    public class VaultUsers
    {
        public virtual int Id { get; set; }
        public virtual IdentityUser User { get; set; }
        public virtual Vault Vault { get; set; }
    }
}