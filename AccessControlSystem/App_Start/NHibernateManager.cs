﻿using AccessControlSystem.Domain;
using AccessControlSystem.Models;
using NHibernate;
using NHibernate.AspNet.Identity;
using NHibernate.AspNet.Identity.Helpers;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

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

            var sessionFactory = configuration.BuildSessionFactory();
            Seed(sessionFactory);
            return sessionFactory;
        }

        private static HbmMapping getDomainMapping()
        {
            var mapper = new ModelMapper();
            mapper.AddMappings(typeof(NHibernateManager).Assembly.GetExportedTypes());

            return mapper.CompileMappingForAllExplicitlyAddedEntities();
        }

        private static void Seed(ISessionFactory sessionFactory)
        {
            try
            {
                using (var session = sessionFactory.OpenSession())
                {
                    using(var tr = session.BeginTransaction())
                    {
                        var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(session));
                        var adminUser = new ApplicationUser{Email = "admin@example.com", UserName = "admin", EmailConfirmed = true};
                        var vaultAdminUser = new ApplicationUser{Email = "vault_admin@example.com", UserName = "vault_admin", EmailConfirmed = true};
                        var simpleUser = new ApplicationUser{Email = "user@example.com", UserName = "user", EmailConfirmed = true};

                        var res = manager.Create(adminUser, "Pass1,");
                        if (!res.Succeeded)
                            throw new Exception(String.Join(System.Environment.NewLine, res.Errors));

                        res = manager.Create(vaultAdminUser, "Pass1,");
                        if (!res.Succeeded)
                            throw new Exception(String.Join(System.Environment.NewLine, res.Errors));

                        res = manager.Create(simpleUser, "Pass1,");
                        if (!res.Succeeded)
                            throw new Exception(String.Join(System.Environment.NewLine, res.Errors));

                        var vault = new Vault { Admin = vaultAdminUser, Name = "Важные данные", OpeningTime = 1, ClosingTime = 23 };
                        session.Save(vault);

                        session.Save(new VaultUsers { User = vaultAdminUser, Vault = vault });
                        session.Save(new VaultUsers { User = simpleUser, Vault = vault });

                        tr.Commit();
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}