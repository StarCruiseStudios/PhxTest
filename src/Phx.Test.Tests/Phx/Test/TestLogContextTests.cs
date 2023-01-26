// -----------------------------------------------------------------------------
//  <copyright file="TestLogContextTests.cs" company="Star Cruise Studios LLC">
//      Copyright (c) 2022 Star Cruise Studios LLC. All rights reserved.
//      Licensed under the Apache License, Version 2.0.
//      See http://www.apache.org/licenses/LICENSE-2.0 for full license information.
//  </copyright>
// -----------------------------------------------------------------------------

namespace Phx.Test {
    using NLog;
    using NSubstitute;
    using NUnit.Framework;
    using Phx.Validation;

    [TestFixture]
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    [Parallelizable(ParallelScope.All)]
    public class TestLogContextTests {
        private static readonly Logger testLogger = LogManager.GetCurrentClassLogger();
        private const string TEST_NAME = "TestName";
        private const string GIVEN = "Given";
        private const string WHEN = "When";
        private const string THEN = "Then";
        private const string TEST_RESULT_PASSED = "TestResult: **PASSED**";
        private const string TEST_RESULT_FAILED = "TestResult: **FAILED**";

        [Test]
        public void LogContextLogsSuccessfulTest() {
            var logger = Substitute.For<ILogger>();
            logger.Info(Arg.Do<string>(x => testLogger.Info(x)));
            var logContext = new TestLogContext(logger);

            logContext.LogStart(TEST_NAME);
            logContext.LogEnd(true);

            Received.InOrder(() => {
                logger.Info(Arg.Is<string>(it => it.Contains(TEST_NAME)));
                logger.Info(Arg.Is<string>(it => it.Contains(TEST_RESULT_PASSED)));
            });
        }

        [Test]
        public void LogContextLogsFailedTest() {
            var logger = Substitute.For<ILogger>();
            logger.Info(Arg.Do<string>(x => testLogger.Info(x)));
            var logContext = new TestLogContext(logger);

            logContext.LogStart(TEST_NAME);
            logContext.LogEnd(false);

            Received.InOrder(() => {
                logger.Info(Arg.Is<string>(it => it.Contains(TEST_NAME)));
                logger.Info(Arg.Is<string>(it => it.Contains(TEST_RESULT_FAILED)));
            });
        }

        [Test]
        public void GivenIsLoggedAndComputedValueIsReturned() {
            var logger = Substitute.For<ILogger>();
            logger.Info(Arg.Do<string>(x => testLogger.Info(x)));
            var logContext = new TestLogContext(logger);
            const string message = "test precondition";
            const int expected = 10;

            logContext.LogStart(TEST_NAME);
            var result = logContext.Given(message, () => expected);
            logContext.LogEnd(true);

            Verify.That(result.IsEqualTo(10));
            Received.InOrder(() => {
                logger.Info(Arg.Is<string>(it => it.Contains(GIVEN)));
                logger.Info(Arg.Is<string>(it => it.Contains(message)));
            });
        }

        [Test]
        public void GivenIsLoggedWithNoComputedValue() {
            var logger = Substitute.For<ILogger>();
            logger.Info(Arg.Do<string>(x => testLogger.Info(x)));
            var logContext = new TestLogContext(logger);
            const string message = "test precondition";

            logContext.LogStart(TEST_NAME);
            logContext.Given(message);
            logContext.LogEnd(true);

            Received.InOrder(() => {
                logger.Info(Arg.Is<string>(it => it.Contains(GIVEN)));
                logger.Info(Arg.Is<string>(it => it.Contains(message)));
            });
        }

        [Test]
        public void WhenIsLoggedAndComputedValueIsReturned() {
            var logger = Substitute.For<ILogger>();
            logger.Info(Arg.Do<string>(x => testLogger.Info(x)));
            var logContext = new TestLogContext(logger);
            const string message = "test action";
            const int expected = 10;

            logContext.LogStart(TEST_NAME);
            var result = logContext.When(message, () => expected);
            logContext.LogEnd(true);

            Verify.That(result.IsEqualTo(10));
            Received.InOrder(() => {
                logger.Info(Arg.Is<string>(it => it.Contains(WHEN)));
                logger.Info(Arg.Is<string>(it => it.Contains(message)));
            });
        }

        [Test]
        public void WhenIsLoggedWithNoComputedValue() {
            var logger = Substitute.For<ILogger>();
            logger.Info(Arg.Do<string>(x => testLogger.Info(x)));
            var logContext = new TestLogContext(logger);
            const string message = "test action";

            logContext.LogStart(TEST_NAME);
            logContext.When(message);
            logContext.LogEnd(true);

            Received.InOrder(() => {
                logger.Info(Arg.Is<string>(it => it.Contains(WHEN)));
                logger.Info(Arg.Is<string>(it => it.Contains(message)));
            });
        }
        
        [Test]
        public void DeferredWhenIsLogged() {
            var logger = Substitute.For<ILogger>();
            logger.Info(Arg.Do<string>(x => testLogger.Info(x)));
            var logContext = new TestLogContext(logger);
            const string message = "test action";
            var executed = false;

            logContext.LogStart(TEST_NAME);
            var action = logContext.DeferredWhen(message, () => executed = true);
            action();
            logContext.LogEnd(true);

            Verify.That(executed.IsTrue());
            Received.InOrder(() => {
                logger.Info(Arg.Is<string>(it => it.Contains(WHEN)));
                logger.Info(Arg.Is<string>(it => it.Contains(message)));
                logger.Info(Arg.Is<string>(it => it.Contains(THEN)));
                logger.Info(Arg.Is<string>(it => it.Contains(message)));
            });
        }
        
        [Test]
        public void TestFailsWhenDeferredActionIsNotExecuted() {
            var logger = Substitute.For<ILogger>();
            logger.Info(Arg.Do<string>(x => testLogger.Info(x)));
            var logContext = new TestLogContext(logger);
            const string message = "test action";
            var executed = false;

            logContext.LogStart(TEST_NAME);
            var action = logContext.DeferredWhen(message, () => executed = true);
            Verify.That(executed.IsFalse());
            logContext.LogEnd(true);

            Received.InOrder(() => {
                logger.Info(Arg.Is<string>(it => it.Contains("Deferred Action was not executed")));
                logger.Info(Arg.Is<string>(it => it.Contains(TEST_RESULT_FAILED)));
            });
        }

        [Test]
        public void ThenIsLoggedAndAssertionValueIsReturned() {
            var logger = Substitute.For<ILogger>();
            logger.Info(Arg.Do<string>(x => testLogger.Info(x)));
            var logContext = new TestLogContext(logger);
            const string message = "test assertion";
            const int expected = 10;

            logContext.LogStart(TEST_NAME);
            var result = logContext.Then(message, () => expected);
            logContext.LogEnd(true);

            Verify.That(result.IsEqualTo(10));
            Received.InOrder(() => {
                logger.Info(Arg.Is<string>(it => it.Contains(THEN)));
                logger.Info(Arg.Is<string>(it => it.Contains(message)));
            });
        }

        [Test]
        public void ThenIsLoggedWithNoAssertionValue() {
            var logger = Substitute.For<ILogger>();
            logger.Info(Arg.Do<string>(x => testLogger.Info(x)));
            var logContext = new TestLogContext(logger);
            const string message = "test assertion";

            logContext.LogStart(TEST_NAME);
            logContext.Then(message, () => { });
            logContext.LogEnd(true);

            Received.InOrder(() => {
                logger.Info(Arg.Is<string>(it => it.Contains(THEN)));
                logger.Info(Arg.Is<string>(it => it.Contains(message)));
            });
        }

        [Test]
        public void ThenWithExpectedIsLoggedAndAssertionValueIsReturned() {
            var logger = Substitute.For<ILogger>();
            logger.Info(Arg.Do<string>(x => testLogger.Info(x)));
            var logContext = new TestLogContext(logger);
            const string message = "test assertion";
            const int expectedValue = 10;

            logContext.LogStart(TEST_NAME);
            var result = logContext.Then(message, expectedValue, (expected) => expected);
            logContext.LogEnd(true);

            Verify.That(result.IsEqualTo(10));
            Received.InOrder(() => {
                logger.Info(Arg.Is<string>(it => it.Contains(THEN)));
                logger.Info(Arg.Is<string>(it => it.Contains(message) && it.Contains(expectedValue.ToString())));
            });
        }

        [Test]
        public void ThenWithExpectedIsLoggedWithNoAssertionValue() {
            var logger = Substitute.For<ILogger>();
            logger.Info(Arg.Do<string>(x => testLogger.Info(x)));
            var logContext = new TestLogContext(logger);
            const string message = "test assertion";
            const int expectedValue = 10;

            logContext.LogStart(TEST_NAME);
            logContext.Then(message, expectedValue, (_) => { });
            logContext.LogEnd(true);

            Received.InOrder(() => {
                logger.Info(Arg.Is<string>(it => it.Contains(THEN)));
                logger.Info(Arg.Is<string>(it => it.Contains(message) && it.Contains(expectedValue.ToString())));
            });
        }

        [Test]
        public void LogIsLogged() {
            var logger = Substitute.For<ILogger>();
            logger.Info(Arg.Do<string>(x => testLogger.Info(x)));
            var logContext = new TestLogContext(logger);
            const string message = "test log";

            logContext.LogStart(TEST_NAME);
            logContext.Log(message);
            logContext.LogEnd(true);

            logger.Received().Info(Arg.Is<string>(it => it.Contains(message)));
        }

        [Test]
        public void StepsAreLoggedInOrderReceived() {
            var logger = Substitute.For<ILogger>();
            logger.Info(Arg.Do<string>(x => testLogger.Info(x)));
            var logContext = new TestLogContext(logger);

            logContext.LogStart(TEST_NAME);
            logContext.Given("Given 1");
            logContext.Log("Log 1");
            logContext.When("When 1");
            logContext.Log("Log 2");
            logContext.Then("Then 1", () => { });
            logContext.Log("Log 3");
            logContext.Given("Given 2");
            logContext.When("When 2");
            logContext.Then("Then 2", () => { });
            logContext.LogEnd(true);

            Received.InOrder(() => {
                logger.Info(Arg.Is<string>(it => it.Contains("Given 1")));
                logger.Info(Arg.Is<string>(it => it.Contains("Log 1")));
                logger.Info(Arg.Is<string>(it => it.Contains("When 1")));
                logger.Info(Arg.Is<string>(it => it.Contains("Log 2")));
                logger.Info(Arg.Is<string>(it => it.Contains("Then 1")));
                logger.Info(Arg.Is<string>(it => it.Contains("Log 3")));
                logger.Info(Arg.Is<string>(it => it.Contains("Given 2")));
                logger.Info(Arg.Is<string>(it => it.Contains("When 2")));
                logger.Info(Arg.Is<string>(it => it.Contains("Then 2")));
            });
        }

        [Test]
        public void SimilarStepsAreGroupedTogether() {
            var logger = Substitute.For<ILogger>();
            logger.Info(Arg.Do<string>(x => testLogger.Info(x)));
            var logContext = new TestLogContext(logger);

            logContext.LogStart(TEST_NAME);
            logContext.Given("Given 1");
            logContext.Given("Given 2");
            logContext.Log("Log 1");
            logContext.When("When 1");
            logContext.When("When 2");
            logContext.Log("Log 2");
            logContext.Then("Then 1", () => { });
            logContext.Then("Then 2", () => { });
            logContext.Log("Log 3");
            logContext.LogEnd(true);

            Received.InOrder(() => {
                logger.Info(Arg.Is<string>(it => it.Contains(GIVEN)));
                logger.Info(Arg.Is<string>(it => it.Contains("Given 1")));
                logger.Info(Arg.Is<string>(it => it.Contains("Given 2")));
                logger.Info(Arg.Is<string>(it => it.Contains("Log 1")));
                logger.Info(Arg.Is<string>(it => it.Contains(WHEN)));
                logger.Info(Arg.Is<string>(it => it.Contains("When 1")));
                logger.Info(Arg.Is<string>(it => it.Contains("When 2")));
                logger.Info(Arg.Is<string>(it => it.Contains("Log 2")));
                logger.Info(Arg.Is<string>(it => it.Contains(THEN)));
                logger.Info(Arg.Is<string>(it => it.Contains("Then 1")));
                logger.Info(Arg.Is<string>(it => it.Contains("Then 2")));
                logger.Info(Arg.Is<string>(it => it.Contains("Log 3")));
            });
        }

        [Test]
        public void GivenIsSkippedIfNoGivenSteps() {
            var logger = Substitute.For<ILogger>();
            logger.Info(Arg.Do<string>(x => testLogger.Info(x)));
            var logContext = new TestLogContext(logger);

            logContext.LogStart(TEST_NAME);
            logContext.When("When 1");
            logContext.When("When 2");
            logContext.Then("Then 1", () => { });
            logContext.Then("Then 2", () => { });
            logContext.LogEnd(true);

            Received.InOrder(() => {
                logger.Info(Arg.Is<string>(it => it.Contains(WHEN)));
                logger.Info(Arg.Is<string>(it => it.Contains("When 1")));
                logger.Info(Arg.Is<string>(it => it.Contains("When 2")));
                logger.Info(Arg.Is<string>(it => it.Contains(THEN)));
                logger.Info(Arg.Is<string>(it => it.Contains("Then 1")));
                logger.Info(Arg.Is<string>(it => it.Contains("Then 2")));
            });

            logger.DidNotReceive().Info(Arg.Is<string>(it => it.Contains(GIVEN)));
        }

        [Test]
        public void WhenIsSkippedIfNoWhenSteps() {
            var logger = Substitute.For<ILogger>();
            logger.Info(Arg.Do<string>(x => testLogger.Info(x)));
            var logContext = new TestLogContext(logger);

            logContext.LogStart(TEST_NAME);
            logContext.Given("Given 1");
            logContext.Given("Given 2");
            logContext.Then("Then 1", () => { });
            logContext.Then("Then 2", () => { });
            logContext.LogEnd(true);

            Received.InOrder(() => {
                logger.Info(Arg.Is<string>(it => it.Contains(GIVEN)));
                logger.Info(Arg.Is<string>(it => it.Contains("Given 1")));
                logger.Info(Arg.Is<string>(it => it.Contains("Given 2")));
                logger.Info(Arg.Is<string>(it => it.Contains(THEN)));
                logger.Info(Arg.Is<string>(it => it.Contains("Then 1")));
                logger.Info(Arg.Is<string>(it => it.Contains("Then 2")));
            });

            logger.DidNotReceive().Info(Arg.Is<string>(it => it.Contains(WHEN)));
        }

        [Test]
        public void ThenIsSkippedIfNoThenSteps() {
            var logger = Substitute.For<ILogger>();
            logger.Info(Arg.Do<string>(x => testLogger.Info(x)));
            var logContext = new TestLogContext(logger);

            logContext.LogStart(TEST_NAME);
            logContext.Given("Given 1");
            logContext.Given("Given 2");
            logContext.When("When 1");
            logContext.When("When 2");
            logContext.LogEnd(true);

            Received.InOrder(() => {
                logger.Info(Arg.Is<string>(it => it.Contains(GIVEN)));
                logger.Info(Arg.Is<string>(it => it.Contains("Given 1")));
                logger.Info(Arg.Is<string>(it => it.Contains("Given 2")));
                logger.Info(Arg.Is<string>(it => it.Contains(WHEN)));
                logger.Info(Arg.Is<string>(it => it.Contains("When 1")));
                logger.Info(Arg.Is<string>(it => it.Contains("When 2")));
            });

            logger.DidNotReceive().Info(Arg.Is<string>(it => it.Contains(THEN)));
        }
    }
}
