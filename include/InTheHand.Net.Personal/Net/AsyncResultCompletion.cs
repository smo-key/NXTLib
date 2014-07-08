// 32feet.NET - Personal Area Networking for .NET
//
// NXTLib.BluetoothWrapper.AsyncResultCompletion
// 
// Copyright (c) 2010 In The Hand Ltd, All rights reserved.
// Copyright (c) 2010 Alan J McFarlane, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

namespace NXTLib.BluetoothWrapper
{
    /// <summary>
    /// Used with
    /// <see cref="M:NXTLib.BluetoothWrapper.AsyncResultNoResult.SetAsCompleted(System.Exception,NXTLib.BluetoothWrapper.AsyncResultCompletion)">
    /// AsyncResultNoResult.SetAsCompleted</see> and 
    /// <see cref="M:NXTLib.BluetoothWrapper.AsyncResult{TResult}.SetAsCompleted(TResult,AsyncResultNoResult.AsyncResultCompletion)">
    /// AsyncResult&lt;TResult&gt;.SetAsCompleted</see>.
    /// </summary>
    internal enum AsyncResultCompletion
    {
        /// <summary>
        /// Equivalent to <c>true</c> for the <see cref="T:System.Boolean"/>
        /// #x201C;completedSynchronously&#x201D; parameter.
        /// </summary>
        IsSync,
        /// <summary>
        /// Equivalent to <c>false</c> for the <see cref="T:System.Boolean"/>
        /// #x201C;completedSynchronously&#x201D; parameter.
        /// </summary>
        IsAsync,
        /// <summary>
        /// Forces the callback to run on a thread-pool thread.
        /// </summary>
        MakeAsync
    }

}
