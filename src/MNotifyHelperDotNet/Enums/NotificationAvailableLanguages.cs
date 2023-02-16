// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MNotifyHelperDotNet
//  Author           : RzR
//  Created On       : 2023-02-12 20:18
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-12 20:20
// ***********************************************************************
//  <copyright file="NotificationAvailableLanguages.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.ComponentModel;

#endregion

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace MNotifyHelperDotNet.Enums
{
    /// <summary>
    ///     Available notification languages
    /// </summary>
    public enum NotificationAvailableLanguages
    {
        /// <summary>
        ///     English
        /// </summary>
        [Description("en")] 
        EN,

        /// <summary>
        ///     Romanian
        /// </summary>
        [Description("ro")] 
        RO,

        /// <summary>
        ///     Russian
        /// </summary>
        [Description("ru")]
        RU
    }
}