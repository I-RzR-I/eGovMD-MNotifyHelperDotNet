// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MNotifyHelperDotNet
//  Author           : RzR
//  Created On       : 2023-02-10 03:41
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-15 19:40
// ***********************************************************************
//  <copyright file="IMNotifyService.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AggregatedGenericResultMessage.Abstractions;
using MNotifyHelperDotNet.Models.Input.Request;
using MNotifyHelperDotNet.Models.Input.Request.MultiData;
using MNotifyHelperDotNet.Models.Result;

#endregion

namespace MNotifyHelperDotNet.Abstractions
{
    /// <summary>
    ///     MNotify service
    /// </summary>
    /// <remarks></remarks>
    public interface IMNotifyService
    {
        /// <summary>
        ///     Send simple notification
        /// </summary>
        /// <param name="request">Notification request</param>
        /// <returns></returns>
        Task<IResult<NotificationResult>> SendSimpleNotificationAsync(NotificationSimpleRequestModel request);

        /// <summary>
        ///     Sent text notification by email
        /// </summary>
        /// <param name="request">Notification request</param>
        /// <returns></returns>
        /// <remarks></remarks>
        Task<IResult<NotificationResult>> SendTextNotificationByEmailAsync(NotificationRequestModel request);

        /// <summary>
        ///     Sent HTML content notification by email
        /// </summary>
        /// <param name="request">Notification request</param>
        /// <returns></returns>
        /// <remarks></remarks>
        Task<IResult<NotificationResult>> SendHtmlNotificationByEmailAsync(NotificationRequestModel request);

        /// <summary>
        ///     Sent text notification by email to multiple persons
        /// </summary>
        /// <param name="request">Notification request</param>
        /// <returns></returns>
        /// <remarks></remarks>
        Task<IResult<NotificationResult>> SendTextNotificationRequestByEmailAsync(NotificationMultiRecipientRequestModel request);

        /// <summary>
        ///     Sent HTML content notification by email to multiple persons
        /// </summary>
        /// <param name="request">Notification request</param>
        /// <returns></returns>
        /// <remarks></remarks>
        Task<IResult<NotificationResult>> SendHtmlNotificationRequestByEmailAsync(NotificationMultiRecipientRequestModel request);

        /// <summary>
        ///     Get notification send status
        /// </summary>
        /// <param name="notification">Notification request</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        Task<IResult<IEnumerable<NotificationStatusResult>>> GetNotificationStatusAsync(
            NotificationResult notification,
            CancellationToken cancellationToken = default);

#if NET45
        /// <summary>
        ///     Send simple notification
        /// </summary>
        /// <param name="request">Notification request</param>
        /// <returns></returns>
        /// <remarks></remarks>
        IResult<NotificationResult> SendSimpleNotification(NotificationSimpleRequestModel request);

        /// <summary>
        ///     Send notification
        /// </summary>
        /// <param name="request">Notification request</param>
        /// <returns></returns>
        IResult<NotificationResult> SendTextNotificationByEmail(NotificationRequestModel request);

        /// <summary>
        ///     Send notification
        /// </summary>
        /// <param name="request">Notification request</param>
        /// <returns></returns>
        IResult<NotificationResult> SendHtmlNotificationByEmail(NotificationRequestModel request);

        /// <summary>
        ///     Sent text notification by email to multiple persons
        /// </summary>
        /// <param name="request">Notification request</param>
        /// <returns></returns>
        /// <remarks></remarks>
        IResult<NotificationResult> SendTextNotificationRequestByEmail(NotificationMultiRecipientRequestModel request);

        /// <summary>
        ///     Sent HTML content notification by email to multiple persons
        /// </summary>
        /// <param name="request">Notification request</param>
        /// <returns></returns>
        /// <remarks></remarks>
        IResult<NotificationResult> SendHtmlNotificationRequestByEmail(NotificationMultiRecipientRequestModel request);

        /// <summary>
        ///     Get notification send status
        /// </summary>
        /// <param name="notification">Notification request</param>
        /// <returns></returns>
        /// <remarks></remarks>
        IResult<IEnumerable<NotificationStatusResult>> GetNotificationStatus(
            NotificationResult notification);

#endif
    }
}