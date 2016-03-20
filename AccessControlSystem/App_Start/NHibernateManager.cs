using AccessControlSystem.Domain;
using AccessControlSystem.Models;
using NHibernate;
using NHibernate.AspNet.Identity.Helpers;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccessControlSystem
{
    public static class NHibernateManager
    {
        private static volatile ISessionFactory _sessionFactory;
        private static object _lockRoot = new object();

        public static ISession Create()
        {
            if(_sessionFactory == null)
            {
                lock(_lockRoot)
                {
                    if(_sessionFactory == null)
                        _sessionFactory = CreateSessionFactory();
                }
            }
            return _sessionFactory.OpenSession();
        }

        private static ISessionFactory CreateSessionFactory()
        {
            var myEntities = new[] {
                typeof(ApplicationUser)
            };

            var configuration = new Configuration();
            configuration.Configure();
            configuration.AddDeserializedMapping(MappingHelper.GetIdentityMappings(myEntities), null);
            configuration.AddMapping(getDomainMapping());

            new SchemaExport(configuration).Execute(true, true, false);

            return configuration.BuildSessionFactory();
        }

        private static HbmMapping getDomainMapping()
        {
            var mapper = new ModelMapper();
            mapper.AddMappings(typeof(NHibernateManager).Assembly.GetExportedTypes());

            return mapper.CompileMappingForAllExplicitlyAddedEntities();
        }
    }
}