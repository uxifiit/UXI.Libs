using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.Common
{
    /// <summary>
    /// Describes a class that is able to receive options of the specified type, e.g., from command line args.
    /// </summary>
    public interface IOptionsTarget
    {
        /// <summary>
        /// Gets the type of the options that are received by this class.
        /// </summary>
        Type OptionsType { get; }


        /// <summary>
        /// Receives the options of the type specified by the <seealso cref="OptionsType"/> property.
        /// </summary>
        /// <param name="options"></param>
        void ReceiveOptions(object options);
    }
}
