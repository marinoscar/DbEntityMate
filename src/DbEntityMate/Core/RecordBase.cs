using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbEntityMate.Core
{
    /// <summary>
    /// Represents a base class for entity records, providing dynamic field storage, key generation, 
    /// and common metadata such as creation and modification timestamps.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <b>RecordBase</b> is designed to be inherited by entity classes that require flexible field management.
    /// It uses a case-insensitive dictionary to store field values, allowing dynamic access and assignment
    /// via string keys. The class also manages standard metadata fields such as <see cref="Id"/>, 
    /// <see cref="UtcCreatedOn"/>, <see cref="UtcModifiedOn"/>, and <see cref="Version"/>.
    /// </para>
    /// <para>
    /// The default constructor uses <see cref="GuidKeyProvider"/> to generate a unique identifier for the record.
    /// Alternatively, a custom <see cref="IKeyProvider"/> can be supplied to the parameterized constructor.
    /// </para>
    /// </remarks>
    public class RecordBase : IRecord
    {
        /// <summary>
        /// Internal dictionary for storing field values, using case-insensitive keys.
        /// </summary>
        private readonly Dictionary<string, object?> _fields = new(StringComparer.OrdinalIgnoreCase);
        public RecordBase() : this(new GuidKeyProvider())
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordBase"/> class.
        /// Generates a unique <see cref="Id"/> using the provided <see cref="IKeyProvider"/>,
        /// and sets the creation and modification timestamps to the current UTC time.
        /// </summary>
        /// <param name="keyProvider">The key provider used to generate a unique identifier.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="keyProvider"/> is null.</exception>
        public RecordBase(IKeyProvider keyProvider)
        {
            var p = keyProvider ?? throw new ArgumentNullException(nameof(keyProvider));
            Id = p.GenerateKey();
            UtcCreatedOn = DateTime.UtcNow;
            UtcModifiedOn = DateTime.UtcNow;
            Version = 1;
        }

        /// <summary>
        /// Gets or sets the value of a field by name.
        /// </summary>
        /// <param name="name">The name of the field.</param>
        /// <returns>The value of the field if it exists.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the field does not exist when getting.</exception>
        public object? this[string name]
        {
            get => _fields.TryGetValue(name, out var value)
                        ? value
                        : throw new KeyNotFoundException($"Field '{name}' not found.");
            set => _fields[name] = value;
        }

        /// <summary>
        /// Gets the value of a field by name and converts it to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to convert the field value to.</typeparam>
        /// <param name="name">The name of the field.</param>
        /// <returns>The value of the field converted to type <typeparamref name="T"/>.</returns>
        public T? Get<T>(string name) => (T?)Convert.ChangeType(this[name], typeof(T));

        /// <summary>
        /// Sets the value of a field by name.
        /// </summary>
        /// <typeparam name="T">The type of the value to set.</typeparam>
        /// <param name="name">The name of the field.</param>
        /// <param name="value">The value to set.</param>
        public void Set<T>(string name, T value) => this[name] = value!;

        /// <summary>
        /// Gets or sets the unique identifier for the record.
        /// </summary>
        public string Id { get { return Get<string>(nameof(Id)); } set { Set(nameof(Id), value); } }

        /// <summary>
        /// Gets or sets the UTC timestamp when the record was created.
        /// </summary>
        public DateTime UtcCreatedOn { get { return Get<DateTime>(nameof(UtcCreatedOn)); } set { Set(nameof(UtcCreatedOn), value); } }

        /// <summary>
        /// Gets or sets the UTC timestamp when the record was last modified.
        /// </summary>
        public DateTime UtcModifiedOn { get { return Get<DateTime>(nameof(UtcModifiedOn)); } set { Set(nameof(UtcModifiedOn), value); } }

        /// <summary>
        /// Gets or sets the version number of the record.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Serializes the entity to a JSON string with indentation, avoiding circular references.
        /// </summary>
        /// <returns>A JSON string representation of the entity.</returns>
        public string ToJson()
        {
            var options = new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
            };
            return System.Text.Json.JsonSerializer.Serialize(this, this.GetType(), options);
        }

        /// <summary>
        /// Returns a JSON string representation of the entity.
        /// </summary>
        /// <returns>A JSON string representation of the entity.</returns>
        public override string ToString()
        {
            return ToJson();
        }
    }
}
