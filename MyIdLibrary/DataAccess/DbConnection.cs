﻿using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace MyIdLibrary.DataAccess;

public class DbConnection : IDbConnection
{
    private readonly IConfiguration _config;

    public SqlConnection Connection { get; private set; } = new SqlConnection();

    public DbConnection(IConfiguration config)
    {
        _config = config;
        Connection.ConnectionString = _config.GetConnectionString("Default");
    }



}
