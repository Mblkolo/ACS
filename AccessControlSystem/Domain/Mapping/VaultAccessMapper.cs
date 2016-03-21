using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccessControlSystem.Domain.Mapping
{
    public class VaultAccessMapper : ClassMapping<VaultAccess>
    {
        public VaultAccessMapper()
        {
            Table("VaultAccess");
            Id(x => x.Id, m =>
            {
                m.Generator(Generators.Identity);
            });

            Property(x => x.IsSuccess, m => m.NotNullable(true));
            Property(x => x.AccessTime, m => m.NotNullable(true));
            Property(x => x.UserInfo);

            ManyToOne(x => x.Vault, m => m.Column("VaultId"));
        }
    }
}