namespace DbEntityMate.Core
{
    /// <summary>
    /// Defines a contract for providing unique keys for entities.
    /// Implementations must provide a static method to generate a key.
    /// </summary>
    public interface IKeyProvider
    {
        /// <summary>
        /// Generates a unique key as a string.
        /// This method must be implemented as a static abstract member in implementing types.
        /// </summary>
        /// <returns>
        /// A unique string key.
        /// </returns>
        string GenerateKey();
    }
}   