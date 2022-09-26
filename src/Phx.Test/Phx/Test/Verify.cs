// -----------------------------------------------------------------------------
//  <copyright file="Verify.cs" company="Star Cruise Studios LLC">
//      Copyright (c) 2022 Star Cruise Studios LLC. All rights reserved.
//      Licensed under the Apache License 2.0 License.
//      See http://www.apache.org/licenses/LICENSE-2.0 for full license information.
//  </copyright>
// -----------------------------------------------------------------------------

namespace Phx.Test {
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Phx.Validation;

    /// <summary> Contains verification methods that throw a verification exception if evaluated unsuccessfully. </summary>
    public static class Verify {
        /// <summary> Throws an <see cref="VerificationFailedException" /> if a verification fails on a value. </summary>
        /// <param name="result"> The verification result to evaluate. </param>
        /// <param name="failureMessage"> The message to use in case of a verification failure. </param>
        /// <exception cref="VerificationFailedException"> Thrown if the provided result is a failure. </exception>
        public static void That(ValidationResult result, string failureMessage = "Verification failed.") {
            switch (result) {
                case SuccessResult:
                    break;
                case FailureResult failure:
                    throw new VerificationFailedException(failureMessage, failure.Cause);
                default:
#pragma warning disable RCS1140
                    // Add exception to documentation comment: NotSupportedException should never be thrown.               
                    throw new NotSupportedException(result.GetType().AssemblyQualifiedName);
#pragma warning restore RCS1140
            }
        }

        /// <summary> Throws an <see cref="VerificationFailedException" /> because a verification fails. </summary>
        /// <param name="reason"> The message that describes the case of the verification failure. </param>
        /// <exception cref="VerificationFailedException"> Thrown when the method is invoked. </exception>
        [DoesNotReturn]
        public static void Fail(string reason) {
            throw new VerificationFailedException(reason);
        }
    }
}
