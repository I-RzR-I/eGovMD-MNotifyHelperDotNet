// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MNotifyHelperDotNet
//  Author           : RzR
//  Created On       : 2023-02-11 13:10
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-13 23:07
// ***********************************************************************
//  <copyright file="EnumsMapper.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using MNotifyHelperDotNet.Enums;
using NotificationContentType = MNotifyRemoteService.NotificationContentType;
using NotificationImportance = MNotifyRemoteService.NotificationImportance;

#endregion

namespace MNotifyHelperDotNet.Mapper
{
    /// <summary>
    ///     Enums mapper
    /// </summary>
    internal static class EnumsMapper
    {
        /// <summary>
        ///     Map 'MNotifyRemoteService.NotificationDeliveryChannel' to local enum
        /// </summary>
        /// <param name="deliveryChannel">MNotifyRemoteService.NotificationDeliveryChannel</param>
        /// <returns></returns>
        internal static NotificationDeliveryChannel MapDeliveryChannelToLocalEnum(this MNotifyRemoteService.NotificationDeliveryChannel deliveryChannel)
            => deliveryChannel switch
            {
                MNotifyRemoteService.NotificationDeliveryChannel.EMail => NotificationDeliveryChannel.EMail,
                MNotifyRemoteService.NotificationDeliveryChannel.SMS => NotificationDeliveryChannel.SMS,
                _ => NotificationDeliveryChannel.Unknown
            };

        /// <summary>
        ///     Map 'MNotifyRemoteService.NotificationDeliveryStatus' to local enum
        /// </summary>
        /// <param name="deliveryStatus">MNotifyRemoteService.NotificationDeliveryStatus</param>
        /// <returns></returns>
        internal static NotificationDeliveryStatus MapDeliveryStatusToLocalEnum(this MNotifyRemoteService.NotificationDeliveryStatus deliveryStatus)
            => deliveryStatus switch
            {
                MNotifyRemoteService.NotificationDeliveryStatus.Confirmed => NotificationDeliveryStatus.Confirmed,
                MNotifyRemoteService.NotificationDeliveryStatus.Failed => NotificationDeliveryStatus.Failed,
                MNotifyRemoteService.NotificationDeliveryStatus.Pending => NotificationDeliveryStatus.Pending,
                MNotifyRemoteService.NotificationDeliveryStatus.Sent => NotificationDeliveryStatus.Sent,
                _ => NotificationDeliveryStatus.Unknown
            };

        /// <summary>
        ///     Map notification importance to remote service Enum
        /// </summary>
        /// <param name="notificationImportance">Notification importance</param>
        /// <returns></returns>
        internal static NotificationImportance MapNotificationImportanceToServiceEnum(this Enums.NotificationImportance notificationImportance)
            => notificationImportance switch
            {
                Enums.NotificationImportance.High => NotificationImportance.High,
                Enums.NotificationImportance.Low => NotificationImportance.Low,
                Enums.NotificationImportance.Normal => NotificationImportance.Normal,
                _ => NotificationImportance.Normal
            };

        /// <summary>
        ///     Map notification content type to remote service Enum
        /// </summary>
        /// <param name="contentType">Content type</param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal static NotificationContentType MapNotificationContentTypeToServiceEnum(this Enums.NotificationContentType contentType)
            => contentType switch
            {
                Enums.NotificationContentType.Html => NotificationContentType.Html,
                Enums.NotificationContentType.Text => NotificationContentType.Text,
                _ => NotificationContentType.Text
            };
    }
}