// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MNotifyHelperDotNet
//  Author           : RzR
//  Created On       : 2023-02-08 07:41
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-10 08:21
// ***********************************************************************
//  <copyright file="TExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Linq;

#endregion

// ReSharper disable InconsistentNaming

namespace MNotifyHelperDotNet.Extensions
{
    /// <summary>
    ///     T extensions
    /// </summary>
    /// <remarks></remarks>
    public static class TExtensions
    {
        /// <summary>
        ///     Get sub array
        /// </summary>
        /// <param name="array">Source array</param>
        /// <param name="offset">Offset</param>
        /// <param name="length">Take</param>
        /// <returns></returns>
        /// <typeparam name="T">Source type</typeparam>
        /// <remarks></remarks>
        public static T[] SubArray<T>(this T[] array, int offset, int length) => array.Skip(offset)
            .Take(length)
            .ToArray();
    }
}