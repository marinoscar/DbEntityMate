
namespace DbEntityMate.Core
{
    /// <summary>
    /// Represents a generic record entity with dynamic property access, identity, versioning, and audit information.
    /// </summary>
    public interface IRecord
    {
        /// <summary>
        /// Gets or sets the value of a property by its name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns>The value of the property, or <c>null</c> if not set.</returns>
        object? this[string name] { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the record.
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Gets or sets the UTC timestamp when the record was created.
        /// </summary>
        DateTime UtcCreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the UTC timestamp when the record was last modified.
        /// </summary>
        DateTime UtcModifiedOn { get; set; }

        /// <summary>
        /// Gets or sets the version number of the record, used for concurrency control.
        /// </summary>
        int Version { get; set; }

        /// <summary>
        /// Gets the value of a property by its name and converts it to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to convert the property value to.</typeparam>
        /// <param name="name">The name of the property.</param>
        /// <returns>The value of the property converted to type <typeparamref name="T"/>, or <c>null</c> if not set or conversion fails.</returns>
        T? Get<T>(string name);

        /// <summary>
        /// Sets the value of a property by its name.
        /// </summary>
        /// <typeparam name="T">The type of the value to set.</typeparam>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The value to set.</param>
        void Set<T>(string name, T value);

        /// <summary>
        /// Serializes the record to a JSON string representation.
        /// </summary>
        /// <returns>A JSON string representing the record.</returns>
        string ToJson();

    }
}