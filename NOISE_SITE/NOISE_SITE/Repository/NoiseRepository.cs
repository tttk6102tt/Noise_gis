using MicroOrm.Dapper.Repositories;
using MicroOrm.Dapper.Repositories.SqlGenerator;
using NOISE_SITE.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace NOISE_SITE.Repository
{
    public interface INoiseRepository : IDapperRepository<NOISE>
    {
    }
    public class NoiseRepository : DapperRepository<NOISE>, INoiseRepository
    {
        public NoiseRepository(IDbConnection Connection)
            : base(Connection, ESqlConnector.MSSQL)
        {

        }


    }
}