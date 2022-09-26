// -----------------------------------------------------------------------------
//  <copyright file="LoggingTestClass.cs" company="Star Cruise Studios LLC">
//      Copyright (c) 2022 Star Cruise Studios LLC. All rights reserved.
//      Licensed under the Apache License 2.0 License.
//      See http://www.apache.org/licenses/LICENSE-2.0 for full license information.
//  </copyright>
// -----------------------------------------------------------------------------

namespace Phx.Test {
    using System;
    using NLog;
    using NUnit.Framework;
    using NUnit.Framework.Interfaces;

    /// <summary> Test base class that manages a <see cref="TestLogContext" /> and exposes convenience methods for invoking it. </summary>
    public abstract class LoggingTestClass {
        private readonly Logger logger;

        protected TestLogContext LogContext { get; set; } = null!;

        protected LoggingTestClass() {
            logger = LogManager.GetLogger(GetType().FullName);
        }

        /// <summary> Initializes the log context on test start. </summary>
        [SetUp]
        public virtual void SetUpTest() {
            LogContext = new TestLogContext(logger);
            LogContext.LogStart(TestContext.CurrentContext.Test.FullName);
        }

        /// <summary> Ends the log context on test cleanup. </summary>
        [TearDown]
        public virtual void TearDownTest() {
            LogContext.LogEnd(TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed);
        }

        /// <summary> Defines a value that is part of the initial test context that will be operated on. </summary>
        /// <remarks>
        ///     A function is provided instead of the direct value, so that any errors that occur while computing the value
        ///     are handled and displayed correctly.
        /// </remarks>
        /// <typeparam name="T"> The type of the given value. </typeparam>
        /// <param name="description"> Provides more details about the purpose of the value. </param>
        /// <param name="value"> Computes the given value that is returned so it can be assigned to a test variable. </param>
        /// <returns> The computed test value. </returns>
        public T Given<T>(string description, Func<T> value) {
            return LogContext.Given(description, value);
        }

        /// <summary> Defines a state or condition that is part of the initial test context that will be operated on. </summary>
        /// <param name="description"> Provides more details about the purpose of the value. </param>
        public void Given(string description) {
            LogContext.Given(description);
        }

        /// <summary> Defines an action that is being tested. </summary>
        /// <typeparam name="T"> The type of the result of the action. </typeparam>
        /// <param name="description"> Provides more details about the action that is executed. </param>
        /// <param name="action"> The action that is to be executed. </param>
        /// <returns> The value that is returned from the action execution. </returns>
        public T When<T>(string description, Func<T> action) {
            return LogContext.When(description, action);
        }

        /// <summary> Defines an action that is being tested. </summary>
        /// <param name="description"> Provides more details about the action that is executed. </param>
        /// <param name="action"> The action that is to be executed. </param>
        public void When(string description, Action action) {
            LogContext.When(description, action);
        }

        /// <summary> Defines an action that is being tested. </summary>
        /// <param name="description"> Provides more details about the action that is executed. </param>
        public void When(string description) {
            LogContext.When(description);
        }

        /// <summary>
        ///     Defines an expected outcome of a test. This should be an assertion on a value that is returned from an action
        ///     under test.
        /// </summary>
        /// <typeparam name="T"> The type of value that is returned from the assertion. </typeparam>
        /// <param name="description"> Provides more details about the assertion. </param>
        /// <param name="assertion"> The assertion to be performed. </param>
        /// <returns> The result of performing the assertion. </returns>
        public T Then<T>(string description, Func<T> assertion) {
            return LogContext.Then(description, assertion);
        }

        /// <summary>
        ///     Defines an expected outcome of a test. This should be an assertion on a value that is returned from an action
        ///     under test.
        /// </summary>
        /// <param name="description"> Provides more details about the assertion. </param>
        /// <param name="assertion"> The assertion to be performed. </param>
        public void Then(string description, Action assertion) {
            LogContext.Then(description, assertion);
        }

        /// <summary>
        ///     Defines an expected outcome of a test. This should be an assertion on a value that is returned from an action
        ///     under test.
        /// </summary>
        /// <typeparam name="T"> The type of value that is returned from the assertion. </typeparam>
        /// <typeparam name="U"> The type of expected value is asserted. </typeparam>
        /// <param name="description"> Provides more details about the assertion. </param>
        /// <param name="expectedValue">
        ///     The expected value that is validated by the assertion. This value is only provided for
        ///     logging. No addition validation is performed beyond the provided assertion function.
        /// </param>
        /// <param name="assertion"> The assertion to be performed. </param>
        /// <returns> The result of performing the assertion. </returns>
        public T Then<T, U>(string description, U expectedValue, Func<U, T> assertion) {
            return LogContext.Then(description, expectedValue, assertion);
        }

        /// <summary>
        ///     Defines an expected outcome of a test. This should be an assertion on a value that is returned from an action
        ///     under test.
        /// </summary>
        /// <typeparam name="U"> The type of expected value is asserted. </typeparam>
        /// <param name="description"> Provides more details about the assertion. </param>
        /// <param name="expectedValue">
        ///     The expected value that is validated by the assertion. This value is only provided for
        ///     logging. No addition validation is performed beyond the provided assertion function.
        /// </param>
        /// <param name="assertion"> The assertion to be performed. </param>
        public void Then<U>(string description, U expectedValue, Action<U> assertion) {
            LogContext.Then(description, expectedValue, assertion);
        }

        /// <summary> Logs a message that will appear inline with the other test steps in the log output. </summary>
        /// <param name="message"> The message to log. </param>
        public void Log(string message) {
            LogContext.Log(message);
        }
    }
}
