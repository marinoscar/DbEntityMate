using DbEntityMate.Core;

namespace DbEntityMate.Core
{
    /// <summary>
    /// Defines methods for generating SQL dialect-specific statements for database schema creation and modification.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <b>IDialectProvider</b> interface abstracts the generation of SQL statements for different database dialects.
    /// Implementations of this interface are responsible for producing valid SQL for creating tables, primary keys, and foreign key constraints
    /// based on the provided entity and field metadata. This enables support for multiple database engines (e.g., SQL Server, PostgreSQL, MySQL)
    /// by encapsulating dialect-specific syntax and conventions.
    /// </para>
    /// <para>
    /// Consumers of this interface can use it to automate schema generation, migration, or validation tasks in a database-agnostic manner.
    /// </para>
    /// </remarks>
    public interface IDialectProvider
    {
        /// <summary>
        /// Generates the SQL statement(s) required to create a foreign key constraint for the specified field.
        /// </summary>
        /// <param name="field">The <see cref="FieldMetadata"/> describing the field for which to create the foreign key constraint.</param>
        /// <returns>
        /// A string containing the SQL statement(s) to create the foreign key constraint.
        /// The format and content of the statement(s) are specific to the target database dialect.
        /// </returns>
        string CreateForeignKeyStatements(FieldMetadata field);

        /// <summary>
        /// Generates the SQL statement required to create a primary key constraint for the specified entity.
        /// </summary>
        /// <param name="entity">The <see cref="EntityMetadata"/> describing the entity for which to create the primary key.</param>
        /// <returns>
        /// A string containing the SQL statement to create the primary key constraint.
        /// The statement is formatted according to the conventions of the target database dialect.
        /// </returns>
        string GetCreatePrimaryKeyStatement(EntityMetadata entity);

        /// <summary>
        /// Generates the SQL statement required to create a table for the specified entity, including column definitions and constraints.
        /// </summary>
        /// <param name="entity">The <see cref="EntityMetadata"/> describing the entity for which to create the table.</param>
        /// <returns>
        /// A string containing the SQL statement to create the table, formatted for the target database dialect.
        /// </returns>
        string GetCreateTableStatement(EntityMetadata entity);
    }
}