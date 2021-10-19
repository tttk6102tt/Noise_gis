using StructureMap;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Web.Configuration;

namespace NOISE_SITE.DependencyResolution
{
    public static class ObjectFactory
    {
        private static readonly Lazy<Container> _containerBuilder =
            new Lazy<Container>(defaultContainer, LazyThreadSafetyMode.ExecutionAndPublication);

        public static IContainer Container
        {
            get { return _containerBuilder.Value; }
        }

        private static Container defaultContainer()
        {
            return new Container(x =>
            {
                x.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });

                x.For<IDbConnection>().Use(new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString));
            });
        }
    }
}