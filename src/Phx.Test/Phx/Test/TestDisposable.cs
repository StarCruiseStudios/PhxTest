// -----------------------------------------------------------------------------
//  <copyright file="TestDisposable.cs" company="Star Cruise Studios LLC">
//      Copyright (c) 2022 Star Cruise Studios LLC. All rights reserved.
//      Licensed under the Apache License, Version 2.0.
//      See http://www.apache.org/licenses/LICENSE-2.0 for full license information.
//  </copyright>
// -----------------------------------------------------------------------------

namespace Phx.Test {
    using System.Diagnostics;
    using Phx.Debug;
    using Phx.Lang;

    /// <summary> An <see cref="ICheckedDisposable" /> that can be set and used inside test cases. </summary>
    [DebuggerDisplay(DebugDisplay.DEBUGGER_DISPLAY_STRING)]
    public sealed class TestDisposable : ICheckedDisposable, IDebugDisplay {
        /// <summary> Gets a value that indicates whether the object is disposed. </summary>
        public bool IsDisposed { get; private set; }

        /// <summary> Initializes a new instance of the <see cref="TestDisposable" /> class. </summary>
        /// <param name="isDisposed"> Set to <c> true </c> if the object is disposed. </param>
        public TestDisposable(bool isDisposed) {
            IsDisposed = isDisposed;
        }

        /// <inheritdoc />
        public void Dispose() {
            IsDisposed = true;
        }

        /// <inheritdoc />
        public string ToDebugDisplay() {
            return $"{this} {{ IsDisposed: {IsDisposed} }}";
        }
    }
}
