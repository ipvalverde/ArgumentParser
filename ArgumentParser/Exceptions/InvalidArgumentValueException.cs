﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace IPValverde.ArgumentParser.Exceptions
{
    /// <summary>
    /// Indicates that an argument has in invalid value.
    /// </summary>
    public class InvalidArgumentValueException : Exception, ISerializable
    {
        /// <summary>
        /// The name of the argument with invalid value.
        /// </summary>
        public string ArgumentName { get; private set; }

        /// <summary>
        /// The value provided for the argument.
        /// </summary>
        public string ArgumentValue { get; private set; }

        internal InvalidArgumentValueException(string argumentName, string message)
            : base(message)
        {
            this.ArgumentName = argumentName;
        }

        internal InvalidArgumentValueException(string argumentName, string argumentValue, string message)
            : this(argumentName, message)
        {
            this.ArgumentValue = argumentValue;
        }

        internal InvalidArgumentValueException(string argumentName, string argumentValue, string message, Exception innerException)
            : base(message, innerException)
        {
            this.ArgumentName = argumentName;
            this.ArgumentValue = argumentValue;
        }
    }
}
