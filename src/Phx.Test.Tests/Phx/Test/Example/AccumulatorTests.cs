// -----------------------------------------------------------------------------
//  <copyright file="AccumulatorTests.cs" company="Star Cruise Studios LLC">
//      Copyright (c) 2022 Star Cruise Studios LLC. All rights reserved.
//      Licensed under the Apache License, Version 2.0.
//      See http://www.apache.org/licenses/LICENSE-2.0 for full license information.
//  </copyright>
// -----------------------------------------------------------------------------

namespace Phx.Test.Example {
    using System;
    using NUnit.Framework;
    using Phx.Validation;

    [TestFixture]
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    [Parallelizable(ParallelScope.All)]
    public class AccumulatorTests : LoggingTestClass {
        [Test]
        public void APositiveValueCanBeAdded() {
            var accumulator = Given("An accumulator instance", () => new Accumulator());
            var valueToAdd = Given("A positive number", () => 10);

            When("The value is added to the accumulator", () => accumulator.Add(valueToAdd));

            Then("The accumulator has the expected total",
                    10,
                    (expected) => { Verify.That(accumulator.Total.IsEqualTo(expected)); });
        }

        [Test]
        public void ANegativeValueCannotBeAdded() {
            var accumulator = Given("An accumulator instance", () => new Accumulator());
            var valueToAdd = Given("A negative number", () => -10);

            var action = DeferredWhen("The value is added to the accumulator",
                    () => accumulator.Add(valueToAdd));

            Then("The expected exception is thrown",
                    typeof(InvalidOperationException),
                    (expectedExceptionType) => { Verify.That(action.DoesThrow(expectedExceptionType)); });
        }
    }
}
