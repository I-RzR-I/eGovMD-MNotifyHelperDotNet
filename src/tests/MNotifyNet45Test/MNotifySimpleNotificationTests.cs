// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MNotifyNet45Test
//  Author           : RzR
//  Created On       : 2023-02-14 00:25
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-14 00:29
// ***********************************************************************
//  <copyright file="MNotifySimpleNotificationTests.cs" company="">
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
using MNotifyHelperDotNet.Models.Input.Request;
using MNotifyHelperDotNet.Services;

#endregion

namespace MNotifyNet45Test
{
    [TestClass]
    public class MNotifySimpleNotificationTests
    {
        private IMNotifyService _mNotifyService;

        [TestInitialize]
        public void Initialize()
        {
            _mNotifyService = MNotifyService.Instance;
        }

        [TestMethod]
        public void SimpleTextNotification_Should_Be_Sent()
        {
            var sendResult = _mNotifyService.SendSimpleNotification(new NotificationSimpleRequestModel
            {
                ContentType = NotificationContentType.Text,
                Subject = Constants.TestSubject + $" {nameof(IMNotifyService.SendSimpleNotification)}",
                NotificationType = Constants.TestNotification +
                                   $" {nameof(IMNotifyService.SendSimpleNotification)}",
                Sender = Constants.TestSenderEmail,
                Recipient = Constants.TestRecipientEmail,
                Content = Constants.TestTextContent + $" {nameof(IMNotifyService.SendSimpleNotification)}"
            });

            Assert.IsTrue(sendResult.IsSuccess);
        }

        [TestMethod]
        public void SimpleHtmlNotification_Should_Be_Sent()
        {
            var sendResult = _mNotifyService.SendSimpleNotification(new NotificationSimpleRequestModel
            {
                ContentType = NotificationContentType.Html,
                Subject = Constants.TestSubject + $" {nameof(IMNotifyService.SendSimpleNotification)}",
                NotificationType = Constants.TestNotification +
                                   $" {nameof(IMNotifyService.SendSimpleNotification)}",
                Sender = Constants.TestSenderEmail,
                Recipient = Constants.TestRecipientEmail,
                Content = Constants.TestHtmlContent + $" {nameof(IMNotifyService.SendSimpleNotification)}"
            });

            Assert.IsTrue(sendResult.IsSuccess);
        }
    }
}