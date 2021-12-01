using System;
using System.Data;
using backend.Interfaces;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace backend.Helper.Connection
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public ConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public IDbConnection Connection()
        {
            string connectionString = _configuration.GetConnectionString("Default");
            IDbConnection connection = new OracleConnection(connectionString);
            connection.Open();
            return connection;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}