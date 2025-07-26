using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbEntityMate.Core
{
    /// <summary>
    /// Provides a mechanism for generating unique string keys using GUIDs.
    /// </summary>
    /// <remarks>
    /// This implementation generates a new GUID, removes hyphens, and converts it to uppercase.
    /// The resulting key is a 32-character hexadecimal string.
    /// </remarks>
    public class GuidKeyProvider : IKeyProvider
    {
        /// <summary>
        /// Generates a new unique key as a 32-character uppercase hexadecimal string.
        /// </summary>
        /// <returns>
        /// A string representation of a GUID without hyphens, in uppercase.
        /// Example: "3F2504E04F8911D39A0C0305E82C3301"
        /// </returns>
        public string GenerateKey()
        {
            return Guid.NewGuid().ToString().Replace("-", "").ToUpperInvariant();
        }   
    }
}
