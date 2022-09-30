// -----------------------------------------------------------------------------
//  <copyright file="VerifyTests.cs" company="Star Cruise Studios LLC">
//      Copyright (c) 2022 Star Cruise Studios LLC. All rights reserved.
//      Licensed under the Apache License, Version 2.0.
//      See http://www.apache.org/licenses/LICENSE-2.0 for full license information.
//  </copyright>
// -----------------------------------------------------------------------------

namespace Phx.Test {
    using NUnit.Framework;
    using Phx.Validation;

    [TestFixture]
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    [Parallelizable(ParallelScope.All)]
    public class VerifyTests {
        [Test]
        public void VerifyThatOnSuccess() {
            var result = ValidationResult.Success();

            Verify.That(result);
        }

        [Test]
        public void VerifyThatOnFailure() {
            var result = ValidationResult.Failure("Test failed");

            _ = TestUtils.TestForError<VerificationFailedException>(
                    () => Verify.That(result));
        }

        [Test]
        public void VerifyFail() {
            _ = TestUtils.TestForError<VerificationFailedException>(
                    () => Verify.Fail("Test failed"));
        }
    }
}
