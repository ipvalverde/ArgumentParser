﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace IPValverde.ArgumentParser.Exceptions
{
    /// <summary>
    /// Thrown when an invalid argument is found.
    /// </summary>
    [Serializable]
    public class InvalidArgumentException : Exception
    {
        /// <summary>
        /// The invalid argument name.
        /// </summary>
        public string ArgumentName { get; private set; }

        internal InvalidArgumentException(string argumentName, string message) : base(message)
        {
            this.ArgumentName = argumentName;
        }
    }
}
