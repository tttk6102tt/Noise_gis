using Microsoft.Extensions.Options;
using NOISE_SITE.Options;
using Smooth.IoC.UnitOfWork.Abstractions;
using Smooth.IoC.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
namespace NOISE_SITE.Repository.Sessions
{
    public interface IAppSession : ISession
    {
    }

    public class AppSession : Session<SqlConnection>, IAppSession
    {
        public AppSession(IDbFactory session, IOptions<ConnectionConfig> connectionConfig)
                : base(session, connectionConfig.Value.DefaultConnection)
        {
            Dapper.FastCrud.OrmConfiguration.DefaultDialect = Dapper.FastCrud.SqlDialect.MsSql;
        }
    }
}