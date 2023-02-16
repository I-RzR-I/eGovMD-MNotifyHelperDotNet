// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MNotifyHelperDotNet
//  Author           : RzR
//  Created On       : 2023-02-10 08:37
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-10 08:37
// ***********************************************************************
//  <copyright file="NotificationDeliveryChannel.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

namespace MNotifyHelperDotNet.Enums
{
    /// <summary>
    ///     Notification delivery channel
    /// </summary>
    public enum NotificationDeliveryChannel
    {
        /// <summary>
        ///     Unknown delivery channel
        /// </summary>
        Unknown = -1,

        /// <summary>
        ///     Delivery by email
        /// </summary>
        EMail = 1,

        /// <summary>
        ///     Delivery by SMS
        /// </summary>
        SMS = 2,
    }
}