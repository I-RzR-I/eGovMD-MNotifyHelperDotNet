// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MNotifyNet5Test
//  Author           : RzR
//  Created On       : 2023-02-13 01:42
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-13 01:42
// ***********************************************************************
//  <copyright file="MNotifySendMultipleNotificationTests.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MNotifyHelperDotNet;
using MNotifyHelperDotNet.Abstractions;
using MNotifyHelperDotNet.Enums;
using MNotifyHelperDotNet.Models.Input.Notification;
using MNotifyHelperDotNet.Models.Input.Request;
using MNotifyHelperDotNet.Models.Input.Request.MultiData;

namespace MNotifyNet5Test
{
    [TestClass]
    public class MNotifySendMultipleNotificationTests
    {
        private IConfiguration _configuration;
        private IServiceProvider _serviceProvider;

        [TestInitialize]
        public void Initialize()
        {
            var confBuilder = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.json"), false)
                .AddEnvironmentVariables();

            _configuration = confBuilder.Build();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddNotifyService(_configuration);

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [TestMethod]
        public async Task TextNotification_Should_Be_Sent()
        {
            var mNotifyService = _serviceProvider.GetRequiredService<IMNotifyService>();

            var sendResult = await mNotifyService
                .SendTextNotificationRequestByEmailAsync(new NotificationMultiRecipientRequestModel()
                {
                    NotificationType = Constants.TestNotification + $" {nameof(IMNotifyService.SendTextNotificationRequestByEmailAsync)}",
                    NotificationImportance = NotificationImportance.Normal,
                    SeparateRecipients = false,
                    Sender = new PersonMultipleRecipients(Constants.TestSenderName, Constants.TestSenderEmail),
                    Recipients = new List<PersonMultipleRecipients>()
                    {
                        new PersonMultipleRecipients(Constants.TestSenderName, Constants.TestSenderEmail),
                        new PersonMultipleRecipients(Constants.TestRecipientName, Constants.TestRecipientEmail2)
                    },
                    Contents = new List<NotificationMultiContentRequestModel>()
                    {
                        new NotificationMultiContentRequestModel()
                        {
                            Content = Constants.TestTextContent + $" {nameof(IMNotifyService.SendTextNotificationRequestByEmailAsync)}",
                            Subject = Constants.TestSubject + $" {nameof(IMNotifyService.SendTextNotificationRequestByEmailAsync)}",
                            Language = NotificationAvailableLanguages.RO,
                            ContentType = NotificationContentType.Text
                        }
                    }
                });

            Assert.IsTrue(sendResult.IsSuccess);
        }

        [TestMethod]
        public async Task HtmlNotification_Should_Be_Sent()
        {
            var mNotifyService = _serviceProvider.GetRequiredService<IMNotifyService>();

            var sendResult = await mNotifyService
                .SendHtmlNotificationRequestByEmailAsync(new NotificationMultiRecipientRequestModel()
                {
                    NotificationType = Constants.TestNotification + $" {nameof(IMNotifyService.SendHtmlNotificationRequestByEmailAsync)}",
                    NotificationImportance = NotificationImportance.Normal,
                    SeparateRecipients = false,
                    Sender = new PersonMultipleRecipients(Constants.TestSenderName, Constants.TestSenderEmail),
                    Recipients = new List<PersonMultipleRecipients>()
                    {
                        new PersonMultipleRecipients(Constants.TestRecipientName, Constants.TestRecipientEmail),
                        new PersonMultipleRecipients(Constants.TestRecipientName, Constants.TestRecipientEmail2)
                    },
                    Contents = new List<NotificationMultiContentRequestModel>()
                    {
                        new NotificationMultiContentRequestModel()
                        {
                            Content = Constants.TestHtmlContent + $" {nameof(IMNotifyService.SendHtmlNotificationRequestByEmailAsync)}",
                            Subject = Constants.TestSubject + $" {nameof(IMNotifyService.SendHtmlNotificationRequestByEmailAsync)}",
                            Language = NotificationAvailableLanguages.RO,
                            ContentType = NotificationContentType.Html
                        }
                    }
                });

            Assert.IsTrue(sendResult.IsSuccess);
        }
    }
}