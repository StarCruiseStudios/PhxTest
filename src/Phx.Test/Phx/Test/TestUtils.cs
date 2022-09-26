// -----------------------------------------------------------------------------
//  <copyright file="TestUtils.cs" company="Star Cruise Studios LLC">
//      Copyright (c) 2022 Star Cruise Studios LLC. All rights reserved.
//      Licensed under the Apache License 2.0 License.
//      See http://www.apache.org/licenses/LICENSE-2.0 for full license information.
//  </copyright>
// -----------------------------------------------------------------------------

namespace Phx.Test {
    using System;

    /// <summary> Static class that provides utilities for running tests. </summary>
    public static class TestUtils {
        /// <summary> Runs a test and validates that an error of the specified type was thrown. </summary>
        /// <typeparam name="T"> The type of error to catch. </typeparam>
        /// <param name="test"> The test to run. </param>
        /// <returns> The caught exception. </returns>
        public static T TestForError<T>(Action test) where T : Exception {
            T? caughtException = null;
            try {
                test();
            } catch (T e) {
                caughtException = e;
                Console.WriteLine($"Caught expected error: {e}");
            } catch (Exception e) {
                Console.WriteLine($"Caught unexpected error: {e}");
                throw;
            }

            if (caughtException == null) {
                Verify.Fail($"Did not catch expected exception {typeof(T).Name}.");
            }

            return caughtException;
        }

        /// <summary> Runs a test and validates that an error of the specified type was thrown. </summary>
        /// <param name="exceptionType"> The type of error to catch. </param>
        /// <param name="test"> The test to run. </param>
        /// <returns> The caught exception. </returns>
        public static Exception TestForError(Type exceptionType, Action test) {
            Exception? caughtException = null;
            try {
                test();
            } catch (Exception e) {
                if (exceptionType.IsInstanceOfType(e)) {
                    caughtException = e;
                    Console.WriteLine($"Caught expected error: {e}");
                } else {
                    Console.WriteLine($"Caught unexpected error: {e}");
                    throw;
                }
            }

            if (caughtException == null) {
                Verify.Fail($"Did not catch expected exception {exceptionType.Name}.");
            }

            return caughtException;
        }
    }
}
