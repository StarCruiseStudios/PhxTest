// -----------------------------------------------------------------------------
//  <copyright file="DeferredTestAction.cs" company="Star Cruise Studios LLC">
//      Copyright (c) 2023 Star Cruise Studios LLC. All rights reserved.
//      Licensed under the Apache License, Version 2.0.
//      See http://www.apache.org/licenses/LICENSE-2.0 for full license information.
//  </copyright>
// -----------------------------------------------------------------------------

namespace Phx.Test {
    using System;

    /// <summary>
    ///     Wraps a test action that was deferred by a DeferredWhen step. The test class will track
    ///     this action and fail if it is not executed before the test is complete.
    /// </summary>
    public sealed class DeferredTestAction {
        /// <summary> Gets a description of the deferred action. </summary>
        public string Description { get; }

        /// <summary> Gets a value indicating whether the deferred action has been executed. </summary>
        public bool HasExecuted { get; private set; }

        private Action TestAction { get; }
        private TestLogContext Context { get; }

        /// <summary> Initializes a new instance of the <see cref="DeferredTestAction" /> class. </summary>
        /// <param name="description"> A description of the deferred action. </param>
        /// <param name="testAction"> The test action to execute. </param>
        /// <param name="context"> A reference to the test context this action will be executed in. </param>
        public DeferredTestAction(string description, Action testAction, TestLogContext context) {
            Description = description;
            TestAction = testAction;
            Context = context;
        }

        /// <summary> Executes the deferred action. </summary>
        public void Execute() {
            HasExecuted = true;
            Context.Then($"A deferred action is executed: `{Description}`", () => { });
            TestAction();
        }
    }
}
