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
        public string GetCreateTableStatement(EntityMetadata entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (string.IsNullOrWhiteSpace(entity.TableName)) throw new ArgumentException("TableName is required.", nameof(entity));
            if (entity.Fields == null || entity.Fields.Count == 0) throw new ArgumentException("Entity must have at least one field.", nameof(entity));

            var sb = new StringBuilder();
            sb.AppendLine($"CREATE TABLE IF NOT EXISTS \"{entity.TableName}\" (");

            for (int i = 0; i < entity.Fields.Count; i++)
            {
                var field = entity.Fields[i];
                sb.Append($"    \"{field.Name}\" {GetPostgresFieldDefinition(field)}");

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

        public string CreateForeignKeyStatements(FieldMetadata field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));
            if (string.IsNullOrWhiteSpace(field.TableName)) throw new ArgumentException("Field TableName is required.", nameof(field));
            if (string.IsNullOrWhiteSpace(field.Name)) throw new ArgumentException("Field Name is required.", nameof(field));

            // Assume "ParentEntityName" is the property indicating the parent table
            var parentTable = field.ParentEntityName;
            if (string.IsNullOrWhiteSpace(parentTable))
                return string.Empty;

            var fkName = $"FK_{field.TableName}_{field.Name}_{parentTable}";
            var sb = new StringBuilder();

            sb.AppendLine("DO $$");
            sb.AppendLine("BEGIN");
            sb.AppendLine($"    IF NOT EXISTS (");
            sb.AppendLine("        SELECT 1");
            sb.AppendLine("        FROM pg_constraint");
            sb.AppendLine($"        WHERE conname = '{fkName}'");
            sb.AppendLine("    ) THEN");
            sb.AppendLine($"        ALTER TABLE \"{field.TableName}\"");
            sb.AppendLine($"        ADD CONSTRAINT \"{fkName}\" FOREIGN KEY (\"{field.Name}\") REFERENCES \"{parentTable}\"(\"Id\");");
            sb.AppendLine("    END IF;");
            sb.AppendLine("END $$;");

            return sb.ToString();
        }

        public string GetCreatePrimaryKeyStatement(EntityMetadata entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (string.IsNullOrWhiteSpace(entity.TableName)) throw new ArgumentException("TableName is required.", nameof(entity));

            var constraintName = $"PK_{entity.TableName}";
            var tableName = entity.TableName;

            // PostgreSQL: Use DO block to check if constraint exists, then create if not
            var sb = new StringBuilder();
            sb.AppendLine("DO $$");
            sb.AppendLine("BEGIN");
            sb.AppendLine($"    IF NOT EXISTS (");
            sb.AppendLine("        SELECT 1");
            sb.AppendLine("        FROM pg_constraint");
            sb.AppendLine("        WHERE conname = '" + constraintName + "'");
            sb.AppendLine("    ) THEN");
            sb.AppendLine($"        ALTER TABLE \"{tableName}\" ADD CONSTRAINT \"{constraintName}\" PRIMARY KEY (\"Id\");");
            sb.AppendLine("    END IF;");
            sb.AppendLine("END $$;");
            return sb.ToString();
        }

        

        private string GetPostgresFieldDefinition(FieldMetadata field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));
            if (string.IsNullOrWhiteSpace(field.Name)) throw new ArgumentException("Field name is required.", nameof(field));
            if (string.IsNullOrWhiteSpace(field.Type)) throw new ArgumentException("Field type is required.", nameof(field));

            var type = field.Type.ToLowerInvariant();
            var sb = new StringBuilder();
            sb.Append($"\"{field.Name}\"");

            switch (type)
            {
                case "string":
                    int stringLength = field.Length ?? 10485760; // PostgreSQL TEXT is unlimited, but VARCHAR has a max of 10485760
                    if (stringLength < 1) stringLength = 10485760;
                    if (stringLength < 10485760)
                        sb.Append($" VARCHAR({stringLength})");
                    else
                        sb.Append(" TEXT");
                    break;
                case "decimal":
                    int precision = field.Precision ?? 131072; // PostgreSQL max precision for NUMERIC is 131072 digits before the decimal point
                    int scale = field.Length ?? 16383; // PostgreSQL max scale for NUMERIC is 16383 digits after the decimal point
                    sb.Append($" NUMERIC({precision},{scale})");
                    break;
                case "double":
                    sb.Append(" DOUBLE PRECISION");
                    break;
                case "float":
                    sb.Append(" REAL");
                    break;
                case "int":
                case "int32":
                case "integer":
                    sb.Append(" INTEGER");
                    break;
                case "long":
                case "int64":
                    sb.Append(" BIGINT");
                    break;
                case "short":
                case "int16":
                    sb.Append(" SMALLINT");
                    break;
                case "bool":
                case "boolean":
                    sb.Append(" BOOLEAN");
                    break;
                case "datetime":
                    sb.Append(" TIMESTAMP");
                    break;
                case "guid":
                    sb.Append(" UUID");
                    break;
                case "byte[]":
                case "binary":
                    sb.Append(" BYTEA");
                    break;
                default:
                    sb.Append(" TEXT");
                    break;
            }

            if (field.IsRequired)
                sb.Append(" NOT NULL");

            if (field.IsUnique)
                sb.Append(" UNIQUE");

            return sb.ToString();
        }
        
    }
}
