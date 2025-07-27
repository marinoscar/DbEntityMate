using DbEntityMate.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbEntityMate.Npgsql
{
    /// <summary>
    /// Provides methods for generating PostgreSQL SQL statements based on entity metadata.
    /// </summary>
    /// <remarks>This class includes functionality to create SQL statements for table creation, foreign key
    /// constraints, and primary key constraints, tailored for PostgreSQL databases. It ensures that constraints are
    /// only added if they do not already exist, preventing errors during repeated executions.</remarks>
    public class NpgsqlDialectProvider : IDialectProvider
    {
        /// <summary>
        /// Generates a PostgreSQL CREATE TABLE statement for the specified entity metadata.
        /// </summary>
        /// <param name="entity">The entity metadata describing the table structure.</param>
        /// <returns>A SQL string representing the CREATE TABLE statement.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="entity"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="entity"/> has no TableName or no fields.</exception>
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
                sb.Append($"    \"{field.Name}\" {MapField(field)}");

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




        /// <summary>
        /// Generates a PostgreSQL statement to add a foreign key constraint for the specified field, if it does not already exist.
        /// </summary>
        /// <param name="field">The field metadata representing the foreign key column.</param>
        /// <returns>
        /// A SQL string containing a DO block that checks for the existence of the foreign key constraint and adds it if missing.
        /// Returns an empty string if <paramref name="field.ParentEntityName"/> is null or empty.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="field"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="field.TableName"/> or <paramref name="field.Name"/> is null or empty.</exception>
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

        /// <summary>
        /// Generates a PostgreSQL statement to add a primary key constraint to the specified entity's table, if it does not already exist.
        /// </summary>
        /// <param name="entity">The entity metadata for which to create the primary key constraint.</param>
        /// <returns>
        /// A SQL string containing a DO block that checks for the existence of the primary key constraint and adds it if missing.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="entity"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="entity.TableName"/> is null or empty.</exception>
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

        private string MapField(FieldMetadata field)
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
