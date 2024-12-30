using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBCompare.Models
{
    public class Connection
    {
        string connectionName { get ; set; }

        enumDataSource dataSource { get; set; }
        string fileServerPath { get; set; }
        string userName { get; set; }
        string password { get; set; }
        string databaseName { get; set; }

    }

    public enum enumDataSource
    {
        SQLServer = 0,
        MySQL = 1,
        SQLite = 2
    }
}