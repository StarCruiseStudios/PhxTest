// -----------------------------------------------------------------------------
//  <copyright file="TestUtilsTests.cs" company="Star Cruise Studios LLC">
//      Copyright (c) 2022 Star Cruise Studios LLC. All rights reserved.
//      Licensed under the Apache License, Version 2.0.
//      See http://www.apache.org/licenses/LICENSE-2.0 for full license information.
//  </copyright>
// -----------------------------------------------------------------------------

namespace Phx.Test {
    using System;
    using System.IO;
    using NUnit.Framework;
    using Phx.Validation;

    [TestFixture]
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    [Parallelizable(ParallelScope.All)]
    public class TestUtilsTests {
        [Test]
        public void TestForErrorTCaught() {
            Exception exception =
                    TestUtils.TestForError<DirectoryNotFoundException>(
                            () => throw new DirectoryNotFoundException());
            if (exception is not DirectoryNotFoundException) {
                Verify.Fail("Did not catch expected exception.");
            }
        }

        [Test]
        public void TestForErrorTCaughtDifferent() {
            Exception? caughtException = null;
            try {
                _ = TestUtils.TestForError<DirectoryNotFoundException>(
                        () => throw new InvalidOperationException());
            } catch (Exception e) {
                caughtException = e;
            }

            if (caughtException is not InvalidOperationException) {
                Verify.Fail("Did not catch expected exception.");
            }
        }

        [Test]
        public void TestForErrorTNoneCaught() {
            Exception? caughtException = null;
            try {
                _ = TestUtils.TestForError<DirectoryNotFoundException>(
                        () => _ = 10);
            } catch (Exception e) {
                caughtException = e;
            }

            Verify.That(caughtException.IsNotNull(), "No exception was caught.");
            Verify.That(caughtException!.IsType<VerificationFailedException>(),
                    "Did not catch the expected exception.");
        }

        [Test]
        public void TestForErrorCaught() {
            Exception exception =
                    TestUtils.TestForError(
                            typeof(DirectoryNotFoundException),
                            () => throw new DirectoryNotFoundException());
            if (exception is not DirectoryNotFoundException) {
                Verify.Fail("Did not catch expected exception.");
            }
        }

        [Test]
        public void TestForErrorCaughtDifferent() {
            Exception? caughtException = null;
            try {
                _ = TestUtils.TestForError(
                        typeof(DirectoryNotFoundException),
                        () => throw new InvalidOperationException());
            } catch (Exception e) {
                caughtException = e;
            }

            if (caughtException is not InvalidOperationException) {
                Verify.Fail("Did not catch expected exception.");
            }
        }

        [Test]
        public void TestForErrorNoneCaught() {
            Exception? caughtException = null;
            try {
                _ = TestUtils.TestForError(
                        typeof(DirectoryNotFoundException),
                        () => _ = 10);
            } catch (Exception e) {
                caughtException = e;
            }

            Verify.That(caughtException.IsNotNull(), "No exception was caught.");
            Verify.That(caughtException!.IsType<VerificationFailedException>(),
                    "Did not catch the expected exception.");
        }
    }
}
