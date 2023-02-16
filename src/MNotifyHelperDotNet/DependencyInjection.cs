// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MNotifyHelperDotNet
//  Author           : RzR
//  Created On       : 2022-11-02 22:18
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-01-14 20:46
// ***********************************************************************
//  <copyright file="DependencyInjection.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#if NETSTANDARD2_0_OR_GREATER || NET || NETCOREAPP3_1_OR_GREATER

#region U S A G E S

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MNotifyHelperDotNet.Abstractions;
using MNotifyHelperDotNet.Configurations.Clients;
using MNotifyHelperDotNet.Configurations.Options;
using MNotifyHelperDotNet.Models;
using MNotifyHelperDotNet.Services;
using MNotifyRemoteService;

#endregion

namespace MNotifyHelperDotNet
{
    /// <summary>
    ///     MNotify service DI
    /// </summary>
    /// <remarks></remarks>
    public static class DependencyInjection
    {
        /// <summary>
        ///     Add MNotify service
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="configuration">App configuration</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static IServiceCollection AddNotifyService(this IServiceCollection services, IConfiguration configuration)
        {
            var mSignOption = configuration.GetSection(nameof(RemoteMNotifyOptions)).Get<RemoteMNotifyOptions>();
            services.AddOptions<RemoteMNotifyOptions>();

            services.Configure<RemoteMNotifyOptions>(options =>
            {
                options.ServiceClientAddress = mSignOption.ServiceClientAddress;
                options.ServiceCertificatePath = mSignOption.ServiceCertificatePath;
                options.ServiceCertificatePassword = mSignOption.ServiceCertificatePassword;
            });
            services.PostConfigure<RemoteMNotifyOptions>(options =>
            {
                options.ServiceClientAddress = mSignOption.ServiceClientAddress;
                options.ServiceCertificatePath = mSignOption.ServiceCertificatePath;
                options.ServiceCertificatePassword = mSignOption.ServiceCertificatePassword;
            });
            services.AddSingleton(mSignOption);

            services.AddSingleton<IPostConfigureOptions<RemoteMNotifyOptions>, MNotifyPostConfigureOptions>();

            services.AddScoped<IMNotify, MNotifyClientInternal>();
            services.AddScoped<IMNotifyService, MNotifyService>();

            return services;
        }
    }
}
#endif