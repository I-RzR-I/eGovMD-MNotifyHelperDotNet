// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MNotifyHelperDotNet
//  Author           : RzR
//  Created On       : 2023-02-10 08:32
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-10 08:32
// ***********************************************************************
//  <copyright file="NotificationDeliveryStatus.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

namespace MNotifyHelperDotNet.Enums
{
    /// <summary>
    ///     Notification delivery status
    /// </summary>
    public enum NotificationDeliveryStatus
    {
        /// <summary>
        ///     Unknown status
        /// </summary>
        Unknown = -1,

        /// <summary>
        ///     Pending
        /// </summary>
        Pending = 0,

        /// <summary>
        ///  Notification sent
        /// </summary>
        Sent = 1,

        /// <summary>
        ///     Confirmed
        /// </summary>
        Confirmed = 2,

        /// <summary>
        ///     Failed
        /// </summary>
        Failed = 3,
    }
}