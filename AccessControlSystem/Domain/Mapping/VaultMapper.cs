using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccessControlSystem.Domain.Mapping
{
    public class VaultMapper : ClassMapping<Vault>
    {
        public VaultMapper()
        {
            Table("Vaults");
            Id(x => x.Id, m =>
            {
                m.Generator(Generators.Identity);
            });

            Property(x => x.Name, m => m.NotNullable(true));
            ManyToOne(x => x.Admin, m => m.Column("AdminUserId"));

            this.Bag(x => x.Users, m =>
            {
                m.Lazy(CollectionLazy.Lazy);
                m.Inverse(false);
                m.Key(k => k.Column("VaultId"));
            }, map => map.OneToMany());
        }
    }
}