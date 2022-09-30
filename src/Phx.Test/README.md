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

        var action = When<Action>("The value is added to the accumulator",() => {
            return () => { accumulator.Add(valueToAdd); };
        });

        Then("The expected exception is thrown", typeof(InvalidOperationException),
                (expectedExceptionType) => {
                    Verify.That(action.DoesThrow(expectedExceptionType));
                });
    }
}
```

Will generate the following output:

```text
 ---------------------------------------- 
 Phx.Test.Example.AccumulatorTests.APositiveValueCanBeAdded 
      -------------------- 
 Given 
   * An accumulator instance -> Phx.Test.Example.Accumulator [PASSED] 
   * A positive number -> 10 [PASSED] 
 When 
   * The value is added to the accumulator [PASSED] 
 Then 
   * The accumulator has the expected total : 10 [PASSED] 
      TestResult: PASSED 

 ---------------------------------------- 
 Phx.Test.Example.AccumulatorTests.ANegativeValueCannotBeAdded 
      -------------------- 
 Given 
   * An accumulator instance -> Phx.Test.Example.Accumulator [PASSED] 
   * A negative number -> -10 [PASSED] 
 When 
   * The value is added to the accumulator -> System.Action [PASSED] 
 Then 
   * The expected exception is thrown : System.InvalidOperationException [PASSED] 
      TestResult: PASSED 
```

## Set up

PHX.Test can be installed as a Nuget package using the .NET CLI.

```shell
dotnet add package Phx.Test
```

## Getting Started

Documentation and set up instructions can be found in
the [PHX.Test Repository](https://github.com/StarCruiseStudios/PhxTest)

---

Copyright (c) 2022 Star Cruise Studios LLC. All rights reserved.  
Licensed under the Apache License, Version 2.0.  
See http://www.apache.org/licenses/LICENSE-2.0 for full license information.
