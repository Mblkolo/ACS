using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccessControlSystem.Domain.Mapping
{
    public class VaultUserMapper : ClassMapping<VaultUsers>
    {
        public VaultUserMapper()
        {
            Table("VaultUsers");
            Id(x => x.Id, m =>
            {
                m.Generator(Generators.Identity);
            });

            ManyToOne(x => x.User, m => m.Column("UserId"));
            ManyToOne(x => x.Vault, m => m.Column("VaultId"));
        }
    }
}