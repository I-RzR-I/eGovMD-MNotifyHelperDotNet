// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MNotifyHelperDotNet
//  Author           : RzR
//  Created On       : 2023-02-11 10:33
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-11 13:45
// ***********************************************************************
//  <copyright file="NotificationStatusResult.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using MNotifyHelperDotNet.Enums;

#endregion

// ReSharper disable ClassNeverInstantiated.Global

namespace MNotifyHelperDotNet.Models.Result
{
    /// <summary>
    ///     Notification status result
    /// </summary>
    public class NotificationStatusResult
    {
        /// <summary>
        ///     Notification recipient
        /// </summary>
        public string Recipient { get; set; }

        /// <summary>
        ///     Notification channel statuses
        /// </summary>
        public IEnumerable<NotificationChannelStatus> ChannelStatus { get; set; }

        /// <summary>
        ///     Notification general status
        /// </summary>
        public NotificationDeliveryStatus GeneralStatus { get; set; }
    }

    /// <summary>
    ///     Notification channel status
    /// </summary>
    public class NotificationChannelStatus
    {
        /// <summary>
        ///     Notification send channel
        /// </summary>
        public NotificationDeliveryChannel Channel { get; set; }

        /// <summary>
        ///     Notification send status
        /// </summary>
        public NotificationDeliveryStatus Status { get; set; }
    }
}