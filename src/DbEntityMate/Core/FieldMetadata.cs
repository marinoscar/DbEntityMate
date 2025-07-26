using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbEntityMate.Core
{

    /// </para>
    /// <para>
    /// This class is typically used to describe the schema of an entity or table, providing
    /// additional context for code generation, documentation, or runtime validation.
    /// </para>
    /// <para>
    /// <b>FieldMetadata</b> exposes strongly-typed properties for common field attributes, 
    /// such as <see cref="Name"/>, <see cref="Type"/>, <see cref="Length"/>, and <see cref="Precision"/>.
    /// All property values are stored in the dynamic field dictionary of <see cref="RecordBase"/>,
    /// allowing for flexible extension and dynamic access.
    /// </para>
    /// <para>
    /// This class is suitable for use in metadata-driven scenarios, such as dynamic form generation,
    /// schema introspection, and automated documentation tools.
    /// </para>
    /// </remarks>
    public class FieldMetadata : RecordBase, IRecord
    {
        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <remarks>
        /// The <b>Name</b> property uniquely identifies the field within its containing entity or table.
        /// It is required and should not be null or empty.
        /// </remarks>
        public string Name
        {
            get => (string)this[nameof(Name)] ?? string.Empty;
            set => this[nameof(Name)] = value;
        }

        public string? DisplayName
        {
            get => (string?)this[nameof(DisplayName)];
            set => this[nameof(DisplayName)] = value;
        }

        public string? ParentEntityName
        {
            get => (string?)this[nameof(ParentEntityName)];
            set => this[nameof(ParentEntityName)] = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the field is unique within its containing entity or table.
        /// </summary>
        /// <remarks>
        /// The <b>IsUnique</b> property specifies whether the field must have unique values across all records.
        /// This is typically used to enforce uniqueness constraints at the database or application level.
        /// </remarks>
        public bool IsUnique
        {
            get => (bool?)this[nameof(IsUnique)] ?? false;
            set => this[nameof(IsUnique)] = value;
        }

        /// <summary>
        /// Gets or sets the description of the field.
        /// </summary>
        /// <remarks>
        /// The <b>Description</b> property provides a human-readable explanation of the field's
        /// purpose, usage, or meaning. This information is useful for documentation and tooling.
        /// </remarks>
        public string? Description
        {
            get => (string?)this[nameof(Description)];
            set => this[nameof(Description)] = value;
        }

        /// <summary>
        /// Gets or sets the data type of the field.
        /// </summary>
        /// <remarks>
        /// The <b>Type</b> property specifies the data type of the field, such as "string", "int", "decimal", etc.
        /// The value should correspond to a valid .NET or database type, depending on the context.
        /// </remarks>
        public string? Type
        {
            get => (string?)this[nameof(Type)];
            set => this[nameof(Type)] = value;
        }

        /// <summary>
        /// Gets or sets the maximum length of the field, if applicable.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The <b>Length</b> property specifies the maximum number of characters or bytes allowed for the field value,
        /// depending on the field's data type. For example, for string or binary fields, this property indicates the
        /// maximum permitted length. If the field type does not support length constraints, this property may be <c>null</c>.
        /// </para>
        /// <para>
        /// The value is stored dynamically in the underlying field dictionary of the <see cref="RecordBase"/> class.
        /// </para>
        /// </remarks>
        public int? Length
        {
            get => (int?)this[nameof(Length)];
            set => this[nameof(Length)] = value;
        }

        /// <summary>
        /// Gets or sets the numeric precision of the field, if applicable.
        /// </summary>
        /// <remarks>
        /// The <b>Precision</b> property defines the total number of digits that can be stored for numeric fields.
        /// For non-numeric fields, this property may be <c>null</c>.
        /// </remarks>
        public int? Precision
        {
            get => (int?)this[nameof(Precision)];
            set => this[nameof(Precision)] = value;
        }

        /// <summary>
        /// Gets or sets example values for the field, as a comma-separated string.
        /// </summary>
        /// <remarks>
        /// The <b>SampleValues</b> property provides sample or typical values for the field, which can be used
        /// for documentation, testing, or UI hints. The format is typically a comma-separated list.
        /// </remarks>
        public string? SampleValues
        {
            get => (string?)this[nameof(SampleValues)];
            set => this[nameof(SampleValues)] = value;
        }

        /// <summary>
        /// Gets or sets alternative names or synonyms for the field, as a comma-separated string.
        /// </summary>
        /// <remarks>
        /// The <b>Sinonyms</b> property lists alternative names, aliases, or synonyms for the field.
        /// This can assist in mapping, search, or documentation scenarios.
        /// </remarks>
        public string? Sinonyms
        {
            get => (string?)this[nameof(Sinonyms)];
            set => this[nameof(Sinonyms)] = value;
        }
    }
}
