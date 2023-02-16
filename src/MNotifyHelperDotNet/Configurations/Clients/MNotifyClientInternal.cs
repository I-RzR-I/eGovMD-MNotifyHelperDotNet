// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MNotifyHelperDotNet
//  Author           : RzR
//  Created On       : 2023-02-10 03:43
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-13 23:13
// ***********************************************************************
//  <copyright file="MNotifyClientInternal.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************




#region U S A G E S

using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using AggregatedGenericResultMessage;
using AggregatedGenericResultMessage.Extensions.Result.Messages;
using DomainCommonExtensions.CommonExtensions;
using MNotifyHelperDotNet.Models;
using MNotifyRemoteService;

#if NET45
using MNotifyHelperDotNet.Configurations.Options;
#endif

#if NETSTANDARD2_0_OR_GREATER || NET || NETCOREAPP3_1_OR_GREATER
using Microsoft.Extensions.Options;
#endif

#endregion

namespace MNotifyHelperDotNet.Configurations.Clients
{
    /// <inheritdoc />
    public class MNotifyClientInternal : MNotifyClient
    {
#if NETSTANDARD2_0_OR_GREATER || NET || NETCOREAPP3_1_OR_GREATER
        /// <inheritdoc />
        public MNotifyClientInternal(IOptions<RemoteMNotifyOptions> options) : this(options.Value.BasicHttpsBinding,
            options.Value.EndpointAddress)
        {
            if (!options.IsNull() && !options.Value.IsNull())
                ClientCredentials.ClientCertificate.Certificate = options.Value.ServiceCertificate;
        }

        /// <inheritdoc />
        public MNotifyClientInternal(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress) => Instance = this;

        /// <summary>
        ///     Internal signing service
        /// </summary>
        /// <remarks></remarks>
        // ReSharper disable once InconsistentNaming
        public static MNotifyClientInternal Instance;
#endif

#if NET45

        /// <inheritdoc />
        // ReSharper disable once MemberCanBePrivate.Global
        public MNotifyClientInternal(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress)
        {
            if (!_remoteMNotifyOptions.IsNull())
                // ReSharper disable once PossibleNullReferenceException
            {
                ClientCredentials.ClientCertificate.Certificate = _remoteMNotifyOptions.ServiceCertificate;
            }
        }

        /// <summary>
        ///     Remote signing options
        /// </summary>
        private static readonly RemoteMNotifyOptions _remoteMNotifyOptions = MNotifyPostConfigureOptionsNetFramework.InitOptions();

        /// <summary>
        ///     Internal signing service
        /// </summary>
        /// <remarks></remarks>
#pragma warning disable IDE0090 // Use 'new(...)'
        public static readonly MNotifyClientInternal Instance = new MNotifyClientInternal(_remoteMNotifyOptions.BasicHttpsBinding, _remoteMNotifyOptions.EndpointAddress);
#pragma warning restore IDE0090 // Use 'new(...)'
#endif

        /// <summary>
        ///     Ping MNotify service
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public Result<PingResponse> Ping()
        {
            var client = Instance;
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                if (client.ChannelFactory.State != CommunicationState.Opened &&
                    client.ChannelFactory.State == CommunicationState.Closed)
                {
#if NETSTANDARD2_0_OR_GREATER || NET
                    client.OpenAsync().GetAwaiter().GetResult();
#endif
#if NET45
                    client.Open();
#endif
                }

                stopwatch.Stop();

                return Result<PingResponse>.Success(new PingResponse { Message = "Server is available.", Time = $"{stopwatch.Elapsed}", IsSuccess = true });
            }
            catch (Exception e)
            {
                var result = new Result<PingResponse> { IsSuccess = true };
                result.AddException(e);
                result.SetResult(new PingResponse { Message = "Communication problem: " + e.Message, Time = $"{stopwatch.Elapsed}", IsSuccess = false });

                return result;
            }
        }
    }
}