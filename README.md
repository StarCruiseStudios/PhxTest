# PHX.Test

A toolkit for improving self documentation, readability, and debugging of tests.

PHX.Test assists in setting up a BDD given/when/then structure for readability
and test self-documentation, and it logs each provided value, input, output,
condition, action, and assertion in human readable text to assist in debugging
what is occurring in a test and what is going wrong.

This test class using Phx.Test:

```csharp
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
                (expected) => {
                    Verify.That(accumulator.Total.IsEqualTo(expected));
                });
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
```

Will generate the following output:

```markdown
----------------------------------------
## Phx.Test.Example.AccumulatorTests.APositiveValueCanBeAdded

**Given:**
* An accumulator instance : `Phx.Test.Example.Accumulator` -> **PASSED**
* A positive number : `10` -> **PASSED**

**When:**
* The value is added to the accumulator -> **PASSED**

**Then:**
* The accumulator has the expected total (`10`) -> **PASSED**

> TestResult: **PASSED**

 
----------------------------------------
## Phx.Test.Example.AccumulatorTests.ANegativeValueCannotBeAdded

**Given:**
* An accumulator instance : `Phx.Test.Example.Accumulator` -> **PASSED**
* A negative number : `-10` -> **PASSED**

**When:**
* The value is added to the accumulator -> **DEFERRED**

**Then:**
* A deferred action is executed: `The value is added to the accumulator` -> **PASSED**
* The expected exception is thrown (`System.InvalidOperationException`) -> **PASSED**

> TestResult: **PASSED**
```

## Set up

PHX.Test can be installed as a Nuget package using the .NET CLI.

```shell
dotnet add package Phx.Test
```

## Getting Started

PHX.Test is built on top of NUnit, and the only thing that is necessary to get
started is to write a test class that extends `Phx.Test.LoggingTestClass`.
```csharp
[TestFixture]
public class AccumulatorTests : LoggingTestClass {
    // ...
}
```

The class will have access to the `Given`, `When`, `DeferredWhen`, `Then`, and 
`Log` methods used to log test values and actions.

Following conventions of BDD:
* `Given` should be used to log test preconditions and input values, and will
  return the value for use in later test steps.
* `When` should be used to log the action under test and returns the result of
  the action for validation.
* `DeferredWhen` should be used when execution of the action under test must be
  delayed to validate it correctly. This is typically used in conjunction with
  a Then step to verify an exception was or was not thrown. If a deferred action
  is not executed by the time the test completes, the test will fail.
* `Then` should be used to validate the results of the action. An expected value
  can be passed in to be logged.
* `Log` can also be used to log any test messages that you want to appear inline
  in the test logs.
* `Pending` is used to indicate that a test's implementation is pending 
  completion. By default, pending tests will cause a failure. Setting the
  `PHX_TEST_FAIL_ON_PENDING` env var to `false` or overriding the test class's 
  `FailOnPending` property to return `false` will cause the test to complete 
  successfully and ignore the pending scenario.

## Additional Test Utilities
PHX.Test also provides utilities to make writing and debugging tests easier.

### TestDisposable
`TestDisposable` is an `IDisposable` that allows you to manually initialize it
in a desired disposed state, and to check the current disposed state.

```csharp
var disposable = new TestDisposable(false);
Verify.That(disposable.IsDisposed.IsFalse());

disposable.Dispose();
Verify.That(disposable.IsDisposed.IsTrue());
```

This can be useful when testing types that contain or manipulate `IDisposable`
instances.

### TestException
`TestException` is a simple exception type that can be thrown from mocked or
test code and is easily distinguishable from any exception that may be thrown
from production code.

```csharp
var action = () => { 
    PerformAction(() => { throw new TestException("This is a test."); } 
}

// We know `TestException` was thrown by our test code and not something else
// inside of the `PerformAction` method.
Verify.That(action.DoesThrow<TestException>());
```

### Verify
`Verify` is a [PHX.Validation](https://github.com/StarCruiseStudios/PhxValidation)
validator that will throw a `VerificationFailedException` when the validation
fails.

Test validator extension methods are also provided to help verify that an
exception was thrown by an `Action`.
```csharp
Verify.That(action.DoesThrow<TestException>());
```

---

<div align="center">
Copyright (c) 2022 Star Cruise Studios LLC. All rights reserved.<br/>
Licensed under the Apache License, Version 2.0.<br/>
See http://www.apache.org/licenses/LICENSE-2.0 for full license information.<br/>
</div>