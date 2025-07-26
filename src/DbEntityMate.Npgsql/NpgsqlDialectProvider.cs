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
        public string CreateTable(EntityMetadata entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (string.IsNullOrWhiteSpace(entity.TableName)) throw new ArgumentException("TableName is required.", nameof(entity));
            if (entity.Fields == null || entity.Fields.Count == 0) throw new ArgumentException("Entity must have at least one field.", nameof(entity));

            var sb = new StringBuilder();
            sb.AppendLine($"CREATE TABLE IF NOT EXISTS \"{entity.TableName}\" (");

            for (int i = 0; i < entity.Fields.Count; i++)
            {
                var field = entity.Fields[i];
                sb.Append($"    \"{field.Name}\" {MapToPostgresType(field.Type)}");

                if (!field.IsRequired)
                    sb.Append(" NOT NULL");

                if (i < entity.Fields.Count - 1)
                    sb.AppendLine(",");
                else
                    sb.AppendLine();
            }

            sb.Append(");");
            return sb.ToString();
        }

        private string MapToPostgresType(string type)
        {
            // Basic mapping, extend as needed
            switch (type.ToLowerInvariant())
            {
                case "int":
                case "int32":
                case "integer":
                    return "INTEGER";
                case "long":
                case "int64":
                    return "BIGINT";
                case "short":
                case "int16":
                    return "SMALLINT";
                case "string":
                    return "TEXT";
                case "bool":
                case "boolean":
                    return "BOOLEAN";
                case "datetime":
                    return "TIMESTAMP";
                case "decimal":
                    return "DECIMAL";
                case "double":
                    return "DOUBLE PRECISION";
                case "float":
                    return "REAL";
                case "guid":
                    return "UUID";
                case "byte[]":
                case "binary":
                    return "BYTEA";
                default:
                    return "TEXT";
            }
        }
    }
}
