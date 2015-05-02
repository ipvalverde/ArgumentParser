using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace IPValverde.ArgumentParser.Exceptions
{
    /// <summary>
    /// Exception is thrown when a a duplicate argument identifier is found.
    /// An argument identifier can be its name or alias.
    /// </summary>
    public class DuplicateArgumentException : Exception, ISerializable
    {
        /// <summary>
        /// The argument name or alias which is  duplicated.
        /// </summary>
        public string ArgumentIdentifier { get; private set; }

        internal DuplicateArgumentException(string argumentIndentifier, string message) : base(message)
        {
            this.ArgumentIdentifier = argumentIndentifier;
        }
    }
}
