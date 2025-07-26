using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbEntityMate.Core
{
    /// <summary>
    /// Represents metadata information for an entity, including its name, description, table name, and schema.
    /// Inherits from <see cref="RecordBase"/> to provide dynamic field storage and common metadata such as identity, timestamps, and versioning.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <b>EntityMetadata</b> is designed to encapsulate the essential metadata for a database entity.
    /// It provides strongly-typed properties for common entity attributes, while leveraging the dynamic field management of <see cref="RecordBase"/>.
    /// </para>
    /// <para>
    /// The class supports optional description, table name, and schema, allowing for flexible mapping to database structures.
    /// </para>
    /// </remarks>
    public class EntityMetadata : RecordBase, IRecord
    {
        /// <summary>
        /// Gets or sets the logical name of the entity.
        /// This property is required and uniquely identifies the entity within the context.
        /// </summary>
        public string Name
        {
            get => (string)this[nameof(Name)]!;
            set => this[nameof(Name)] = value;
        }

        /// <summary>
        /// Gets or sets an optional description for the entity.
        /// This can be used to provide additional information or documentation about the entity's purpose.
        /// </summary>
        public string? Description
        {
            get => (string?)this[nameof(Description)];
            set => this[nameof(Description)] = value;
        }

        /// <summary>
        /// Gets or sets the name of the database table associated with the entity.
        /// This property is optional and can be used to override default table naming conventions.
        /// </summary>
        public string? TableName
        {
            get => (string?)this[nameof(TableName)];
            set => this[nameof(TableName)] = value;
        }

        /// <summary>
        /// Gets or sets the database schema in which the entity's table resides.
        /// This property is optional and can be used to specify a custom schema.
        /// </summary>
        public string? Schema
        {
            get => (string?)this[nameof(Schema)];
            set => this[nameof(Schema)] = value;
        }
    }
}
