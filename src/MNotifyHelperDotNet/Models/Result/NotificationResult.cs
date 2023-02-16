// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MNotifyHelperDotNet
//  Author           : RzR
//  Created On       : 2023-02-11 09:05
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-11 11:18
// ***********************************************************************
//  <copyright file="NotificationResult.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

namespace MNotifyHelperDotNet.Models.Result
{
    /// <summary>
    ///     Notification result
    /// </summary>
    public class NotificationResult
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MNotifyHelperDotNet.Models.Result.NotificationResult" /> class.
        /// </summary>
        /// <remarks></remarks>
        public NotificationResult()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MNotifyHelperDotNet.Models.Result.NotificationResult" /> class.
        /// </summary>
        /// <param name="notificationId">Notification id</param>
        /// <remarks></remarks>
        public NotificationResult(string notificationId) => NotificationId = notificationId;

        /// <summary>
        ///     Notification id
        /// </summary>
        public string NotificationId { get; set; }
    }
}