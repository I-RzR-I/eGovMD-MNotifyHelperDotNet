// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MNotifyHelperDotNet
//  Author           : RzR
//  Created On       : 2023-02-10 03:42
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-15 19:38
// ***********************************************************************
//  <copyright file="MNotifyService.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************


#region U S A G E S

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using AggregatedGenericResultMessage;
using AggregatedGenericResultMessage.Abstractions;
using AggregatedGenericResultMessage.Extensions.Common;
using AggregatedGenericResultMessage.Extensions.Result;
using AggregatedGenericResultMessage.Extensions.Result.Messages;
using DomainCommonExtensions.ArraysExtensions;
using DomainCommonExtensions.DataTypeExtensions;
using MNotifyHelperDotNet.Abstractions;
using MNotifyHelperDotNet.Configurations.Clients;
using MNotifyHelperDotNet.Helpers;
using MNotifyHelperDotNet.Mapper;
using MNotifyHelperDotNet.Models;
using MNotifyHelperDotNet.Models.Input.Request;
using MNotifyHelperDotNet.Models.Input.Request.MultiData;
using MNotifyHelperDotNet.Models.Result;
using MNotifyRemoteService;
using NotificationChannelStatus = MNotifyHelperDotNet.Models.Result.NotificationChannelStatus;
using NotificationContentType = MNotifyHelperDotNet.Enums.NotificationContentType;

#if NETSTANDARD2_0_OR_GREATER || NET|| NETCOREAPP3_1_OR_GREATER
using Microsoft.Extensions.Options;
#endif

#if NET45
using MNotifyHelperDotNet.Configurations.Options;
#endif
#if DEBUG
using System.Diagnostics;
#endif

#endregion

// ReSharper disable NotAccessedField.Local

namespace MNotifyHelperDotNet.Services
{
    /// <inheritdoc cref="IMNotifyService" />
    public class MNotifyService : IMNotifyService
    {
        private readonly IMNotify _mNotify;
#pragma warning disable IDE0052 // Remove unread private members
        private readonly RemoteMNotifyOptions _mNotifyOptions;
#pragma warning restore IDE0052 // Remove unread private members

#if NETSTANDARD2_0_OR_GREATER || NET || NETCOREAPP3_1_OR_GREATER
        /// <summary>
        ///     Initializes a new instance of the <see cref="MNotifyHelperDotNet.Services.MNotifyService" /> class.
        /// </summary>
        /// <param name="mNotify">MNotify service</param>
        /// <param name="mNotifyOptions">MNotify option</param>
        /// <remarks></remarks>
        public MNotifyService(IMNotify mNotify, IOptions<RemoteMNotifyOptions> mNotifyOptions)
        {
            _mNotify = mNotify;
            _mNotifyOptions = mNotifyOptions.Value;
        }
#endif

#if NET45
        /// <summary>
        ///     MNotify service instance
        /// </summary>
        /// <remarks></remarks>
#pragma warning disable IDE0090 // Use 'new(...)'
        public static readonly MNotifyService Instance = new MNotifyService();
#pragma warning restore IDE0090 // Use 'new(...)'

        /// <summary>
        ///     Initializes a new instance of the <see cref="MNotifyHelperDotNet.Services.MNotifyService" /> class. 
        /// </summary>
        /// <remarks></remarks>
        public MNotifyService()
        {
            _mNotifyOptions = MNotifyPostConfigureOptionsNetFramework.InitOptions();
            _mNotify = MNotifyClientInternal.Instance;
        }

#endif

        #region S E N D   S I M  P L E   N O T I F I C A T I O N

        /// <inheritdoc />
        public async Task<IResult<NotificationResult>> SendSimpleNotificationAsync(NotificationSimpleRequestModel request)
            => await SendSimpleNotification(request, true);

#if NET45
        /// <inheritdoc />
        public IResult<NotificationResult> SendSimpleNotification(NotificationSimpleRequestModel request)
            => SendSimpleNotification(request, false)
                .GetAwaiter()
                .GetResult();

#endif

        /// <summary>
        ///     Send simple notification
        /// </summary>
        /// <param name="request"></param>
        /// <param name="isAsync">Is async request</param>
        /// <returns></returns>
        private async Task<IResult<NotificationResult>> SendSimpleNotification(NotificationSimpleRequestModel request, bool isAsync)
        {
            var isAlive = MNotifyClientInternal.Instance.Ping();
            if (!isAlive.IsSuccess || !isAlive.Response.IsSuccess)
                return Result<NotificationResult>.Failure()
                    .WithError(isAlive.GetFirstMessage())
                    .WithError(isAlive.Response.Message);

            if (request.IsNull())
                throw new ArgumentNullException(nameof(request));
            try
            {
                var sender = SenderRecipientHelper.GetSenderRecipientForSimpleNotification(request.Sender);
                var recipient = SenderRecipientHelper.GetSenderRecipientForSimpleNotification(request.Recipient);
                var notificationId = isAsync.Equals(true)
                    ? await _mNotify
                        .PostSimpleNotificationAsync(request.NotificationType, sender, recipient, request.Subject,
                            request.ContentType.MapNotificationContentTypeToServiceEnum(), request.Content)
                    : _mNotify
                        // ReSharper disable once MethodHasAsyncOverload
                        .PostSimpleNotification(request.NotificationType, sender, recipient, request.Subject,
                            request.ContentType.MapNotificationContentTypeToServiceEnum(), request.Content);

                return Result<NotificationResult>.Success(new NotificationResult(notificationId));
            }
            catch (FaultException ex)
            {
#if DEBUG
                Debug.WriteLine("Send notification fault: {0}: {1}", ex.Code.Name, ex.Reason);
#endif
                return Result<NotificationResult>.Failure()
                    .WithError($"Send notification fault: {ex.Code.Name}: {ex.Reason}", ex.Code.ToString())
                    .AddException(ex);
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine("Send notification fault: {0}", ex.Message);
#endif
                return Result<NotificationResult>.Failure()
                    .WithError($"Send notification fault: {ex.Message}")
                    .AddException(ex);
            }
        }

        #endregion

        #region S E N D   N O T I F I C A T I O N

        /// <inheritdoc />
        public async Task<IResult<NotificationResult>> SendTextNotificationByEmailAsync(NotificationRequestModel request)
            => await SendNotificationByEmail(request, NotificationContentType.Text, true);

        /// <inheritdoc />
        public async Task<IResult<NotificationResult>> SendHtmlNotificationByEmailAsync(NotificationRequestModel request)
            => await SendNotificationByEmail(request, NotificationContentType.Html, true);

#if NET45
        /// <inheritdoc />
        public IResult<NotificationResult> SendTextNotificationByEmail(NotificationRequestModel request)
            => SendNotificationByEmail(request, Enums.NotificationContentType.Text, false)
            .GetAwaiter()
            .GetResult();

        /// <inheritdoc />
        public IResult<NotificationResult> SendHtmlNotificationByEmail(NotificationRequestModel request)
            => SendNotificationByEmail(request, Enums.NotificationContentType.Html, false)
                .GetAwaiter()
                .GetResult();

#endif

        /// <summary>
        ///     Send notification by email
        /// </summary>
        /// <param name="request">Notification request</param>
        /// <param name="contentType">Send content type</param>
        /// <param name="isAsync">Is async request</param>
        /// <returns></returns>
        private async Task<IResult<NotificationResult>> SendNotificationByEmail(
            NotificationRequestModel request,
            NotificationContentType contentType, bool isAsync)
        {
            var isAlive = MNotifyClientInternal.Instance.Ping();
            if (!isAlive.IsSuccess || !isAlive.Response.IsSuccess)
                return Result<NotificationResult>.Failure()
                    .WithError(isAlive.GetFirstMessage())
                    .WithError(isAlive.Response.Message);

            if (request.IsNull())
                throw new ArgumentNullException(nameof(request));

            try
            {
                var sender = SenderRecipientHelper.GetSenderRecipientSendDetails(request.Sender);
                var recipient = SenderRecipientHelper.GetSenderRecipientSendDetails(request.Recipient);
                var notificationRequest = new NotificationRequest
                {
                    Recipients = new List<string>(1) { recipient },
                    Sender = sender,
                    Type = request.NotificationType,
                    Importance = request.NotificationImportance.MapNotificationImportanceToServiceEnum(),
                    SeparateRecipients = true,
                    Contents = new List<NotificationContent>(1)
                    {
                        new NotificationContent
                        {
                            Subject = request.Subject,
                            Content = request.Content,
                            Type = contentType.MapNotificationContentTypeToServiceEnum(),
                            Language = request.Language.GetDescription()
                        }
                    }
                };

                var notificationId = isAsync.Equals(true)
                    ? await _mNotify.PostNotificationRequestAsync(notificationRequest)
                    // ReSharper disable once MethodHasAsyncOverload
                    : _mNotify.PostNotificationRequest(notificationRequest);

                return Result<NotificationResult>.Success(new NotificationResult(notificationId));
            }
            catch (FaultException ex)
            {
#if DEBUG
                Debug.WriteLine("Send notification fault: {0}: {1}", ex.Code.Name, ex.Reason);
#endif
                return Result<NotificationResult>.Failure()
                    .WithError($"Send notification fault: {ex.Code.Name}: {ex.Reason}", ex.Code.ToString())
                    .AddException(ex);
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine("Send notification fault: {0}", ex.Message);
#endif
                return Result<NotificationResult>.Failure()
                    .WithError($"Send notification fault: {ex.Message}")
                    .AddException(ex);
            }
        }

        #endregion

        #region S E N D   N O T I F I C A T I O N   T O   M  U L T I P L E   P E R S O N S

        /// <inheritdoc />
        public async Task<IResult<NotificationResult>> SendTextNotificationRequestByEmailAsync(NotificationMultiRecipientRequestModel request)
            => await SendNotificationRequestByEmail(request, MNotifyRemoteService.NotificationContentType.Text, true);

        /// <inheritdoc />
        public async Task<IResult<NotificationResult>> SendHtmlNotificationRequestByEmailAsync(NotificationMultiRecipientRequestModel request)
            => await SendNotificationRequestByEmail(request, MNotifyRemoteService.NotificationContentType.Html, true);

#if NET45
        /// <inheritdoc />
        public IResult<NotificationResult> SendTextNotificationRequestByEmail(NotificationMultiRecipientRequestModel request)
            => SendNotificationRequestByEmail(request, MNotifyRemoteService.NotificationContentType.Text, false)
                .GetAwaiter()
                .GetResult();

        /// <inheritdoc />
        public IResult<NotificationResult> SendHtmlNotificationRequestByEmail(NotificationMultiRecipientRequestModel request)
            => SendNotificationRequestByEmail(request, MNotifyRemoteService.NotificationContentType.Html, false)
                .GetAwaiter()
                .GetResult();
#endif

        /// <summary>
        ///     Send notification by email to multiple users
        /// </summary>
        /// <param name="request">Send request</param>
        /// <param name="contentType">Content type</param>
        /// <param name="isAsync">Is async request</param>
        /// <returns></returns>
        private async Task<Result<NotificationResult>> SendNotificationRequestByEmail(
            NotificationMultiRecipientRequestModel request,
            MNotifyRemoteService.NotificationContentType contentType, bool isAsync)
        {
            var isAlive = MNotifyClientInternal.Instance.Ping();
            if (!isAlive.IsSuccess || !isAlive.Response.IsSuccess)
                return Result<NotificationResult>.Failure()
                    .WithError(isAlive.GetFirstMessage())
                    .WithError(isAlive.Response.Message);

            if (request.IsNull())
                throw new ArgumentNullException(nameof(request));

            try
            {
                var sender = SenderRecipientHelper.GetSenderRecipientSendDetails(request.Sender);
                var recipients = SenderRecipientHelper.GetSenderRecipientSendDetails(request.Recipients).ToList();
                var copyRecipients = SenderRecipientHelper.GetSenderRecipientSendDetails(request.Recipients.Where(x => x.CopyRecipient)).ToList();
                var hiddenRecipients = SenderRecipientHelper.GetSenderRecipientSendDetails(request.Recipients.Where(x => x.HiddenRecipient)).ToList();
                var notificationRequest = new NotificationRequest
                {
                    Recipients = recipients,
                    Sender = sender,
                    Type = request.NotificationType,
                    Importance = request.NotificationImportance.MapNotificationImportanceToServiceEnum(),
                    SeparateRecipients = request.SeparateRecipients,
                    CopyRecipients = copyRecipients.NotNull().ToList(),
                    HiddenRecipients = hiddenRecipients.ToList(),
                    Contents = request.Contents
                        .Select(x => new NotificationContent
                        {
                            Subject = x.Subject,
                            Content = x.Content,
                            Language = x.Language.GetDescription(),
                            Type = x.ContentType.MapNotificationContentTypeToServiceEnum(),
                            Attachments = x.Attachments.Select(z => new NotificationAttachment
                            {
                                Name = z.Name,
                                Type = z.Type,
                                Content = z.Content,
                                ID = z.Id,
                                Uri = z.Uri
                            }).NotNull().ToList()
                        }).ToList()
                };

                var requestNotificationId = isAsync.Equals(true)
                    ? await _mNotify.PostNotificationRequestAsync(notificationRequest)
                    // ReSharper disable once MethodHasAsyncOverload
                    : _mNotify.PostNotificationRequest(notificationRequest);

                return Result<NotificationResult>.Success(new NotificationResult(requestNotificationId));
            }
            catch (FaultException ex)
            {
#if DEBUG
                Debug.WriteLine("Send notification fault: {0}: {1}", ex.Code.Name, ex.Reason);
#endif
                return Result<NotificationResult>.Failure()
                    .WithError($"Send notification fault: {ex.Code.Name}: {ex.Reason}", ex.Code.ToString())
                    .WithError(ex);
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine("Send notification fault: {0}", ex.Message);
#endif
                return Result<NotificationResult>.Failure()
                    .WithError($"Send notification fault: {ex.Message}")
                    .WithError(ex);
            }
        }

        #endregion

        #region G E T   N O T I F I C A T I O N

        /// <inheritdoc />
        public async Task<IResult<IEnumerable<NotificationStatusResult>>> GetNotificationStatusAsync(
            NotificationResult notification,
            CancellationToken cancellationToken = default)
            => await GetNotificationStatus(notification, true);

#if NET45
        /// <inheritdoc />
        public IResult<IEnumerable<NotificationStatusResult>> GetNotificationStatus(NotificationResult notification)
            => GetNotificationStatus(notification, false)
                .GetAwaiter()
                .GetResult();
#endif

        /// <summary>
        ///     Get notification status
        /// </summary>
        /// <param name="notification">Notification</param>
        /// <param name="isAsync">Is async request</param>
        /// <returns></returns>
        private async Task<Result<IEnumerable<NotificationStatusResult>>> GetNotificationStatus(
            NotificationResult notification, bool isAsync)
        {
            var isAlive = MNotifyClientInternal.Instance.Ping();
            if (!isAlive.IsSuccess || !isAlive.Response.IsSuccess)
                return Result<IEnumerable<NotificationStatusResult>>.Failure()
                    .WithError(isAlive.GetFirstMessage())
                    .WithError(isAlive.Response.Message);

            if (notification.IsNull())
                throw new ArgumentNullException(nameof(notification));

            try
            {
                var status = isAsync.Equals(true)
                    ? await _mNotify.GetNotificationStatusAsync(notification.NotificationId)
                    // ReSharper disable once MethodHasAsyncOverload
                    : _mNotify.GetNotificationStatus(notification.NotificationId);
                var result = status.Select(x => new NotificationStatusResult
                {
                    Recipient = x.Recipient,
                    GeneralStatus = x.GeneralStatus.MapDeliveryStatusToLocalEnum(),
                    ChannelStatus = x.ChannelStatus
                        .Select(z => new NotificationChannelStatus
                        {
                            Status = z.Status.MapDeliveryStatusToLocalEnum(),
                            Channel = z.Channel.MapDeliveryChannelToLocalEnum()
                        })
                });

                return Result<IEnumerable<NotificationStatusResult>>.Success(result);
            }
            catch (FaultException ex)
            {
#if DEBUG
                Debug.WriteLine("Get notification status fault: {0}: {1}", ex.Code.Name, ex.Reason);
#endif

                return Result<IEnumerable<NotificationStatusResult>>.Failure()
                    .WithError($"Get notification status fault: {ex.Code.Name}: {ex.Reason}", ex.Code.ToString())
                    .WithError(ex);
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine("Get notification status fault: {0}", ex.Message);
#endif

                return Result<IEnumerable<NotificationStatusResult>>.Failure()
                    .WithError($"Get notification status fault: {ex.Message}")
                    .WithError(ex);
            }
        }

        #endregion
    }
}