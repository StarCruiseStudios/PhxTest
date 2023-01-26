// -----------------------------------------------------------------------------
//  <copyright file="PendingException.cs" company="Star Cruise Studios LLC">
//      Copyright (c) 2023 Star Cruise Studios LLC. All rights reserved.
//      Licensed under the Apache License, Version 2.0.
//      See http://www.apache.org/licenses/LICENSE-2.0 for full license information.
//  </copyright>
// -----------------------------------------------------------------------------

namespace Phx.Test {
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    ///     Exception that is thrown from a pending step when PHX.Test is configured to fail on
    ///     pending steps.
    /// </summary>
    [Serializable]
    public class PendingException : Exception {
        /// <summary> Initializes a new instance of the <see cref="PendingException" /> class. </summary>
        /// <param name="message"> The reason a test is pending. </param>
        public PendingException(string message) : base(message) { }

        protected PendingException(
                SerializationInfo info,
                StreamingContext context) : base(info, context) { }
    }
}
