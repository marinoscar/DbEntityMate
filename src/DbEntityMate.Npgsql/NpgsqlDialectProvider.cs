using DbEntityMate.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbEntityMate.Npgsql
{
    public class NpgsqlDialectProvider
    {
        public string GetEntityCreateStatement(EntityMetadata entity)
        {
            return string.Empty;
        }
    }
}
