// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MNotifyNet5Test
//  Author           : RzR
//  Created On       : 2023-02-13 01:42
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-13 01:42
// ***********************************************************************
//  <copyright file="MNotifySendNotificationTests.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using System;
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

namespace MNotifyNet5Test
{
    [TestClass]
    public class MNotifySendNotificationTests
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
                .SendTextNotificationByEmailAsync(new NotificationRequestModel()
                {
                    Content = Constants.TestTextContent + $" {nameof(IMNotifyService.SendTextNotificationByEmailAsync)}",
                    Subject = Constants.TestSubject + $" {nameof(IMNotifyService.SendTextNotificationByEmailAsync)}",
                    NotificationType = Constants.TestNotification + $" {nameof(IMNotifyService.SendTextNotificationByEmailAsync)}",
                    Sender = new Person(Constants.TestSenderName, Constants.TestSenderEmail),
                    Recipient = new Person(Constants.TestRecipientName, Constants.TestRecipientEmail),
                    Language = NotificationAvailableLanguages.RO,
                    NotificationImportance = NotificationImportance.Normal
                });

            Assert.IsTrue(sendResult.IsSuccess);
        }

        [TestMethod]
        public async Task HtmlNotification_Should_Be_Sent()
        {
            var mNotifyService = _serviceProvider.GetRequiredService<IMNotifyService>();

            var sendResult = await mNotifyService
                .SendHtmlNotificationByEmailAsync(new NotificationRequestModel()
                {
                    Content = Constants.TestHtmlContent + $" {nameof(IMNotifyService.SendHtmlNotificationByEmailAsync)}",
                    Subject = Constants.TestSubject + $" {nameof(IMNotifyService.SendHtmlNotificationByEmailAsync)}",
                    NotificationType = Constants.TestNotification + $" {nameof(IMNotifyService.SendHtmlNotificationByEmailAsync)}",
                    Sender = new Person(Constants.TestSenderName, Constants.TestSenderEmail),
                    Recipient = new Person(Constants.TestRecipientName, Constants.TestRecipientEmail),
                    Language = NotificationAvailableLanguages.RO,
                    NotificationImportance = NotificationImportance.Normal
                });

            Assert.IsTrue(sendResult.IsSuccess);
        }
    }
}