// -----------------------------------------------------------------------------
//  <copyright file="Accumulator.cs" company="Star Cruise Studios LLC">
//      Copyright (c) 2022 Star Cruise Studios LLC. All rights reserved.
//      Licensed under the Apache License, Version 2.0.
//      See http://www.apache.org/licenses/LICENSE-2.0 for full license information.
//  </copyright>
// -----------------------------------------------------------------------------

namespace Phx.Test.Example {
    using Phx.Validation;

    public class Accumulator {
        public int Total { get; private set; }
        public void Add(int value) {
            Require.ThatValue((value >= 0).IsTrue());

            Total += value;
        }
    }
}
