using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using TryCatchFail.CodeStock2011.FailTracker.Core.Domain;

namespace TryCatchFail.CodeStock2011.FailTracker.Core.Data
{
	public static class NHibernateBootstrapper
	{
		private static ISessionFactory _sessionFactory;
		private static Configuration _configuration;

		public static void Bootstrap()
		{
			_configuration = Fluently.Configure()
				.Database(MsSqlCeConfiguration.Standard.ConnectionString(c => c.FromConnectionStringWithKey("FailTrackerData")))
				.Mappings(m => m.AutoMappings.Add(AutoMap.AssemblyOf<Issue>(new FailTrackerConfig())))
				.BuildConfiguration();

			_sessionFactory = _configuration.BuildSessionFactory();
		}

		public static void UpdateSchema()
		{
			new SchemaUpdate(_configuration)
				.Execute(false, true);
		}

		public static void CreateSchema()
		{
			new SchemaExport(_configuration)
				.Execute(false, true, false);
		}

		public static ISession GetSession()
		{
			return _sessionFactory.OpenSession();
		}
	}
}