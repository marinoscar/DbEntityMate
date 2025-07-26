
namespace DbEntityMate.Core
{
    /// <summary>
    /// Represents a base entity with common properties for identification, auditing, and versioning.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Gets or sets the UTC timestamp when the entity was created.
        /// </summary>
        DateTime UtcCreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the UTC timestamp when the entity was last modified.
        /// </summary>
        DateTime UtcModifiedOn { get; set; }

        /// <summary>
        /// Gets or sets the version number of the entity, used for concurrency control.
        /// </summary>
        int Version { get; set; }

        /// <summary>
        /// Serializes the entity to a JSON string representation.
        /// </summary>
        /// <returns>A JSON string representing the entity.</returns>
        public string ToJson();
    }
}