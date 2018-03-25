using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.Configuration.Storages
{
    /// <summary>
    /// Use for loading configuration files in different formats. Each configuration file format (e.g., ini, XML, config) should be supplied with its own loader. 
    /// </summary>
    public interface IStorageLoader
    {
        /// <summary>
        /// Attempts to find a configuration file on the given path, or completes the path if it was passed incomplete and uses it for search. If the search was successful, use the updated file path for calling the <seealso cref="Load(string)" />.
        /// </summary>
        /// <param name="path">Path to a file used to look up for a configuration file. If the file was found, the reference is updated with the valid path.</param>
        /// <returns>true if the configuration file was found, otherwise false.</returns>
        bool TryFindConfigurationFile(ref string path); 

        /// <summary>
        /// Loads the configuration file and returns it as an instance of <see cref="IStorage" />.
        /// </summary>
        /// <param name="path">Valid path to the configuration file. Use <seealso cref="TryFindConfigurationFile(ref string)" /> to retrieve the valid path.</param>
        /// <returns>Loaded configuration file as an instance of <see cref="IStorage"/>.</returns>
        IStorage Load(string path);
    }
}
