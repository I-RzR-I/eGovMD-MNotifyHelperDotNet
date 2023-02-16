// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MNotifyNet45Test
//  Author           : RzR
//  Created On       : 2023-02-14 00:25
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-14 00:29
// ***********************************************************************
//  <copyright file="MNotifySendMultipleNotificationTests.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MNotifyHelperDotNet.Abstractions;
using MNotifyHelperDotNet.Enums;
using MNotifyHelperDotNet.Models.Input.Notification;
using MNotifyHelperDotNet.Models.Input.Request.MultiData;
using MNotifyHelperDotNet.Services;

#endregion

namespace MNotifyNet45Test
{
    [TestClass]
    public class MNotifySendMultipleNotificationTests
    {
        private IMNotifyService _mNotifyService;

        [TestInitialize]
        public void Initialize()
        {
            _mNotifyService = MNotifyService.Instance;
        }

        [TestMethod]
        public void TextNotification_Should_Be_Sent()
        {
            var sendResult = _mNotifyService
                .SendTextNotificationRequestByEmail(new NotificationMultiRecipientRequestModel
                {
                    NotificationType = Constants.TestNotification +
                                       $" {nameof(IMNotifyService.SendTextNotificationRequestByEmail)}",
                    NotificationImportance = NotificationImportance.Normal,
                    SeparateRecipients = false,
                    Sender = new PersonMultipleRecipients(Constants.TestSenderName, Constants.TestSenderEmail),
                    Recipients = new List<PersonMultipleRecipients>
                    {
                        new PersonMultipleRecipients(Constants.TestSenderName, Constants.TestSenderEmail),
                        new PersonMultipleRecipients(Constants.TestRecipientName, Constants.TestRecipientEmail2)
                    },
                    Contents = new List<NotificationMultiContentRequestModel>
                    {
                        new NotificationMultiContentRequestModel
                        {
                            Content = Constants.TestTextContent +
                                      $" {nameof(IMNotifyService.SendTextNotificationRequestByEmail)}",
                            Subject = Constants.TestSubject +
                                      $" {nameof(IMNotifyService.SendTextNotificationRequestByEmail)}",
                            Language = NotificationAvailableLanguages.RO,
                            ContentType = NotificationContentType.Text
                        }
                    }
                });

            Assert.IsTrue(sendResult.IsSuccess);
        }

        [TestMethod]
        public void HtmlNotification_Should_Be_Sent()
        {
            var sendResult = _mNotifyService
                .SendHtmlNotificationRequestByEmail(new NotificationMultiRecipientRequestModel
                {
                    NotificationType = Constants.TestNotification +
                                       $" {nameof(IMNotifyService.SendHtmlNotificationRequestByEmail)}",
                    NotificationImportance = NotificationImportance.Normal,
                    SeparateRecipients = false,
                    Sender = new PersonMultipleRecipients(Constants.TestSenderName, Constants.TestSenderEmail),
                    Recipients = new List<PersonMultipleRecipients>
                    {
                        new PersonMultipleRecipients(Constants.TestRecipientName, Constants.TestRecipientEmail),
                        new PersonMultipleRecipients(Constants.TestRecipientName, Constants.TestRecipientEmail2)
                    },
                    Contents = new List<NotificationMultiContentRequestModel>
                    {
                        new NotificationMultiContentRequestModel
                        {
                            Content = Constants.TestHtmlContent +
                                      $" {nameof(IMNotifyService.SendHtmlNotificationRequestByEmail)}",
                            Subject = Constants.TestSubject +
                                      $" {nameof(IMNotifyService.SendHtmlNotificationRequestByEmail)}",
                            Language = NotificationAvailableLanguages.RO,
                            ContentType = NotificationContentType.Html
                        }
                    }
                });

            Assert.IsTrue(sendResult.IsSuccess);
        }
    }
}