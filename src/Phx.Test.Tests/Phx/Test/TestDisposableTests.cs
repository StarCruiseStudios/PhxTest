// -----------------------------------------------------------------------------
//  <copyright file="TestDisposableTests.cs" company="Star Cruise Studios LLC">
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
    public class TestDisposableTests {
        [Test]
        public void TestDispose() {
            var d = new TestDisposable(false);
            d.Dispose();
            Verify.That(d.IsDisposed.IsTrue(),
                    "Test disposable was not disposed correctly.");
        }

        [TestCase(true)]
        [TestCase(false)]
        public void TestInitialize(bool initialState) {
            var d = new TestDisposable(initialState);
            Verify.That(d.IsDisposed.IsEqualTo(initialState),
                    "Test disposable was not initialized correctly.");
        }
    }
}
