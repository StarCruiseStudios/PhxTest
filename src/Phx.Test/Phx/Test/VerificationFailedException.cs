// -----------------------------------------------------------------------------
//  <copyright file="VerificationFailedException.cs" company="Star Cruise Studios LLC">
//      Copyright (c) 2022 Star Cruise Studios LLC. All rights reserved.
//      Licensed under the Apache License, Version 2.0.
//      See http://www.apache.org/licenses/LICENSE-2.0 for full license information.
//  </copyright>
// -----------------------------------------------------------------------------

namespace Phx.Test {
    using System;
    using System.Runtime.Serialization;

    /// <summary> The exception that is thrown when a verification condition is false. </summary>
    [Serializable]
    public sealed class VerificationFailedException : Exception {
        /// <summary> Initializes a new instance of the <see cref="VerificationFailedException" /> class. </summary>
        public VerificationFailedException() { }

        /// <summary> Initializes a new instance of the <see cref="VerificationFailedException" /> class. </summary>
        /// <param name="message"> The message that describes the error. </param>
        public VerificationFailedException(string message)
                : base(message) { }

        /// <summary> Initializes a new instance of the <see cref="VerificationFailedException" /> class. </summary>
        /// <param name="message"> The message that describes the error. </param>
        /// <param name="innerException">
        ///     The exception that is the cause of the current exception, or <c> null </c> if no inner
        ///     exception is specified.
        /// </param>
        public VerificationFailedException(string message, Exception innerException)
                : base(message, innerException) { }

        /// <summary> Initializes a new instance of the <see cref="TestException" /> class. </summary>
        /// <param name="info">
        ///     The <see cref="SerializationInfo" /> that holds the serialized object data about the exception
        ///     being thrown.
        /// </param>
        /// <param name="context">
        ///     The <see cref="StreamingContext" /> that contains contextual information about the source or
        ///     destination.
        /// </param>
        public VerificationFailedException(SerializationInfo info, StreamingContext context)
                : base(info, context) { }
    }
}
