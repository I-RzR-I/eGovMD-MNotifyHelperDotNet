// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MNotifyNet45Test
//  Author           : RzR
//  Created On       : 2023-02-14 00:25
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-14 00:29
// ***********************************************************************
//  <copyright file="MNotifySendNotificationTests.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MNotifyHelperDotNet.Abstractions;
using MNotifyHelperDotNet.Enums;
using MNotifyHelperDotNet.Models.Input.Notification;
using MNotifyHelperDotNet.Models.Input.Request;
using MNotifyHelperDotNet.Services;

#endregion

namespace MNotifyNet45Test
{
    [TestClass]
    public class MNotifySendNotificationTests
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
                .SendTextNotificationByEmail(new NotificationRequestModel
                {
                    Content =
                        Constants.TestTextContent + $" {nameof(IMNotifyService.SendTextNotificationByEmail)}",
                    Subject = Constants.TestSubject + $" {nameof(IMNotifyService.SendTextNotificationByEmail)}",
                    NotificationType = Constants.TestNotification +
                                       $" {nameof(IMNotifyService.SendTextNotificationByEmail)}",
                    Sender = new Person(Constants.TestSenderName, Constants.TestSenderEmail),
                    Recipient = new Person(Constants.TestRecipientName, Constants.TestRecipientEmail),
                    Language = NotificationAvailableLanguages.RO,
                    NotificationImportance = NotificationImportance.Normal
                });

            Assert.IsTrue(sendResult.IsSuccess);
        }

        [TestMethod]
        public void HtmlNotification_Should_Be_Sent()
        {
            var sendResult = _mNotifyService
                .SendHtmlNotificationByEmail(new NotificationRequestModel
                {
                    Content =
                        Constants.TestHtmlContent + $" {nameof(IMNotifyService.SendHtmlNotificationByEmail)}",
                    Subject = Constants.TestSubject + $" {nameof(IMNotifyService.SendHtmlNotificationByEmail)}",
                    NotificationType = Constants.TestNotification +
                                       $" {nameof(IMNotifyService.SendHtmlNotificationByEmail)}",
                    Sender = new Person(Constants.TestSenderName, Constants.TestSenderEmail),
                    Recipient = new Person(Constants.TestRecipientName, Constants.TestRecipientEmail),
                    Language = NotificationAvailableLanguages.RO,
                    NotificationImportance = NotificationImportance.Normal
                });

            Assert.IsTrue(sendResult.IsSuccess);
        }
    }
}