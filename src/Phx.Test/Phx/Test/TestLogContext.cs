// -----------------------------------------------------------------------------
//  <copyright file="TestLogContext.cs" company="Star Cruise Studios LLC">
//      Copyright (c) 2022 Star Cruise Studios LLC. All rights reserved.
//      Licensed under the Apache License, Version 2.0.
//      See http://www.apache.org/licenses/LICENSE-2.0 for full license information.
//  </copyright>
// -----------------------------------------------------------------------------

namespace Phx.Test {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using NLog;
    using Phx.Lang;

    /// <summary> Provides helper methods for readability and auditing of test values. </summary>
    public sealed class TestLogContext {
        private const string TEST_SEPARATOR = "----------------------------------------";
        private const string LOG_SPACER = "     --------------------";
        private const string EVALUATION_DELIMITER = " -> ";
        private const string EXPECTATION_DELIMITER = " : ";

        private readonly ILogger logger;
        private readonly List<TestStep> testSteps = new();

        /// <summary> Initializes a new instance of the <see cref="TestLogContext" /> class. </summary>
        /// <param name="logger"> The logger used to log test steps and results. </param>
        public TestLogContext(ILogger logger) {
            this.logger = logger;
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
            var result = TestResults.FAILED;
            var messageBuilder = new StringBuilder(description);
            try {
                var evaluated = value();
                _ = messageBuilder.Append(EVALUATION_DELIMITER).Append(evaluated.ToDebugDisplayString());
                result = TestResults.PASSED;
                return evaluated;
            }
            finally {
                testSteps.Add(new ExecutionStep(TestSteps.Given, result, messageBuilder.ToString()));
            }
        }

        /// <summary> Defines a state or condition that is part of the initial test context that will be operated on. </summary>
        /// <param name="description"> Provides more details about the purpose of the value. </param>
        public void Given(string description) {
            testSteps.Add(new ExecutionStep(TestSteps.Given, TestResults.PASSED, description));
        }

        /// <summary> Defines an action that is being tested. </summary>
        /// <typeparam name="T"> The type of the result of the action. </typeparam>
        /// <param name="description"> Provides more details about the action that is executed. </param>
        /// <param name="action"> The action that is to be executed. </param>
        /// <returns> The value that is returned from the action execution. </returns>
        public T When<T>(string description, Func<T> action) {
            var result = TestResults.FAILED;
            var messageBuilder = new StringBuilder(description);
            try {
                var evaluated = action();
                _ = messageBuilder.Append(EVALUATION_DELIMITER).Append(evaluated.ToDebugDisplayString());
                result = TestResults.PASSED;
                return evaluated;
            }
            finally {
                testSteps.Add(new ExecutionStep(TestSteps.When, result, messageBuilder.ToString()));
            }
        }

        /// <summary> Defines an action that is being tested. </summary>
        /// <param name="description"> Provides more details about the action that is executed. </param>
        /// <param name="action"> The action that is to be executed. </param>
        public void When(string description, Action action) {
            var result = TestResults.FAILED;
            try {
                action();
                result = TestResults.PASSED;
            }
            finally {
                testSteps.Add(new ExecutionStep(TestSteps.When, result, description));
            }
        }

        /// <summary> Defines an action that is being tested. </summary>
        /// <param name="description"> Provides more details about the action that is executed. </param>
        public void When(string description) {
            testSteps.Add(new ExecutionStep(TestSteps.When, TestResults.PASSED, description));
        }

        /// <summary>
        /// Defines an event that will occur at a later time. This should be an action that is under test,
        /// but that will not be executed immediately. This is typically used with a verification that an
        /// action throws an exception.
        /// </summary>
        /// <param name="description"> Provides more details about the action that will be executed. </param>
        /// <param name="action">The action that will be executed.</param>
        /// <returns>The action that should be executed.</returns>
        public Action DeferredWhen(string description, Action action) {
            testSteps.Add(new ExecutionStep(TestSteps.When, TestResults.DEFERRED, description));
            return action;
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
            var result = TestResults.FAILED;
            var messageBuilder = new StringBuilder(description);
            try {
                var evaluated = assertion();
                _ = messageBuilder.Append(EVALUATION_DELIMITER).Append(evaluated.ToDebugDisplayString());
                result = TestResults.PASSED;
                return evaluated;
            }
            finally {
                testSteps.Add(new ExecutionStep(TestSteps.Then, result, messageBuilder.ToString()));
            }
        }

        /// <summary>
        ///     Defines an expected outcome of a test. This should be an assertion on a value that is returned from an action
        ///     under test.
        /// </summary>
        /// <param name="description"> Provides more details about the assertion. </param>
        /// <param name="assertion"> The assertion to be performed. </param>
        public void Then(string description, Action assertion) {
            var result = TestResults.FAILED;
            try {
                assertion();
                result = TestResults.PASSED;
            }
            finally {
                testSteps.Add(new ExecutionStep(TestSteps.Then, result, description));
            }
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
            var result = TestResults.FAILED;
            var messageBuilder = new StringBuilder(description)
                    .Append(EXPECTATION_DELIMITER)
                    .Append(expectedValue.ToDebugDisplayString());

            try {
                var evaluated = assertion(expectedValue);
                _ = messageBuilder.Append(EVALUATION_DELIMITER).Append(evaluated.ToDebugDisplayString());
                result = TestResults.PASSED;
                return evaluated;
            }
            finally {
                testSteps.Add(new ExecutionStep(TestSteps.Then, result, messageBuilder.ToString()));
            }
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
            var result = TestResults.FAILED;
            var messageBuilder = new StringBuilder(description)
                    .Append(EXPECTATION_DELIMITER)
                    .Append(expectedValue.ToDebugDisplayString());
            try {
                assertion(expectedValue);
                result = TestResults.PASSED;
            }
            finally {
                testSteps.Add(new ExecutionStep(TestSteps.Then, result, messageBuilder.ToString()));
            }
        }

        /// <summary> Logs a message that will appear inline with the other test steps in the log output. </summary>
        /// <param name="message"> The message to log. </param>
        public void Log(string message) {
            testSteps.Add(new LogStep(message));
        }

        /// <summary> Starts the logging context. </summary>
        /// <remarks> This method should be invoked from the test start lifecycle method. </remarks>
        /// <param name="displayName"> The display name of the executing test. </param>
        public void LogStart(string? displayName) {
            logger.Info(string.Empty);
            logger.Info(TEST_SEPARATOR);
            if (displayName != null) {
                logger.Info(displayName);
                logger.Info(LOG_SPACER);
            }
        }

        /// <summary> Ends the logging context and displays the executed steps. </summary>
        /// <remarks> This method should be invoked from the test cleanup lifecycle method. </remarks>
        /// <param name="success"> A value indicating whether the test completed successfully. </param>
        public void LogEnd(bool success) {
            LogSteps();
            var result = success
                    ? TestResults.PASSED
                    : TestResults.FAILED;
            logger.Info($"     TestResult: {result}");
        }

        private void LogSteps() {
            TestSteps prevStep = TestSteps.None;
            foreach (var step in testSteps) {
                if (step is ExecutionStep executionStep) {
                    if (prevStep != executionStep.Step) {
                        logger.Info(executionStep.Step.ToString());
                        prevStep = executionStep.Step;
                    }
                }

                logger.Info(step.ToString());
            }
        }
    }

    internal enum TestSteps {
        None,
        Given,
        When,
        Then
    }

    internal enum TestResults {
        PASSED,
        FAILED,
        DEFERRED
    }

    internal abstract class TestStep {
        public string Message { get; }

        internal TestStep(string message) {
            Message = message;
        }
    }

    internal sealed class ExecutionStep : TestStep {
        public TestSteps Step { get; }
        public TestResults Result { get; }

        public ExecutionStep(TestSteps step, TestResults result, string message) : base(message) {
            Step = step;
            Result = result;
        }

        public override string ToString() => $"  * {Message} [{Result}]";
    }

    internal sealed class LogStep : TestStep {
        internal LogStep(string message) : base(message) { }

        public override string ToString() => Message;
    }
}
