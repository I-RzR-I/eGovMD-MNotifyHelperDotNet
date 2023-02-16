// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MNotifyNet5Test
//  Author           : RzR
//  Created On       : 2023-02-13 01:42
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-13 01:42
// ***********************************************************************
//  <copyright file="MNotifySimpleNotificationTests.cs" company="">
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
using MNotifyHelperDotNet.Models.Input.Request;

namespace MNotifyNet5Test
{
    [TestClass]
    public class MNotifySimpleNotificationTests
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
        public async Task SimpleTextNotification_Should_Be_Sent()
        {
            var mNotifyService = _serviceProvider.GetRequiredService<IMNotifyService>();

            var sendResult = await mNotifyService.SendSimpleNotificationAsync(new NotificationSimpleRequestModel()
            {
                ContentType = NotificationContentType.Text,
                Subject = Constants.TestSubject + $" {nameof(IMNotifyService.SendSimpleNotificationAsync)}",
                NotificationType = Constants.TestNotification + $" {nameof(IMNotifyService.SendSimpleNotificationAsync)}",
                Sender = Constants.TestSenderEmail,
                Recipient = Constants.TestRecipientEmail,
                Content = Constants.TestTextContent + $" {nameof(IMNotifyService.SendSimpleNotificationAsync)}"
            });

            Assert.IsTrue(sendResult.IsSuccess);
        }

        [TestMethod]
        public async Task SimpleHtmlNotification_Should_Be_Sent()
        {
            var mNotifyService = _serviceProvider.GetRequiredService<IMNotifyService>();

            var sendResult = await mNotifyService.SendSimpleNotificationAsync(new NotificationSimpleRequestModel()
            {
                ContentType = NotificationContentType.Html,
                Subject = Constants.TestSubject + $" {nameof(IMNotifyService.SendSimpleNotificationAsync)}",
                NotificationType = Constants.TestNotification + $" {nameof(IMNotifyService.SendSimpleNotificationAsync)}",
                Sender = Constants.TestSenderEmail,
                Recipient = Constants.TestRecipientEmail,
                Content = Constants.TestHtmlContent + $" {nameof(IMNotifyService.SendSimpleNotificationAsync)}"
            });

            Assert.IsTrue(sendResult.IsSuccess);
        }
    }
}