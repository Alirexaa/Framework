using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Persistence.EFCore.SqlServer;

public class SqlServerOptions
{
    public string ConnectionString { get; set; } = default!;
    public string InMemoryDatabaseName { get; set; } = "InMemory";
    public bool UseInMemoryDatabase { get; set; }
}

