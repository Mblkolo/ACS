using AccessControlSystem.Models;
using NHibernate;
using NHibernate.AspNet.Identity.Helpers;
using NHibernate.Cfg;
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

            new SchemaExport(configuration).Execute(true, true, false);

            return configuration.BuildSessionFactory();
        }
    }
}