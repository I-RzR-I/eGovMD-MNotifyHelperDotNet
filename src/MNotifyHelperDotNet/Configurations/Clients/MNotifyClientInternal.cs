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
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Threading;
using RzR.Extensions.Domain.Primitives;
using RzR.ResultMessage;
using RzR.ResultMessage.Extensions.Result.Messages;
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
        /// <summary>
        ///     Serializes the replacement of an unusable <see cref="Instance" />
        /// </summary>
        private static readonly object RecreateLock = new object();

#if !NET45
        /// <summary>
        ///     True when <see cref="Ping" /> created this client, so Ping may abort it when it is replaced
        /// </summary>
        private bool _createdByPing;
#endif

        /// <summary>
        ///     Initializes a new instance of the <see cref="MNotifyClientInternal" /> class without publishing it as
        ///     <see cref="Instance" />
        /// </summary>
        /// <param name="binding">Client binding</param>
        /// <param name="remoteAddress">Remote service address</param>
        /// <param name="certificate">Client certificate</param>
        /// <remarks>Used only by <see cref="Ping" />, which publishes the fully configured client itself.</remarks>
        private MNotifyClientInternal(Binding binding, EndpointAddress remoteAddress, X509Certificate2 certificate) : base(binding, remoteAddress)
        {
            ClientCredentials.ClientCertificate.Certificate = certificate;
#if !NET45
            _createdByPing = true;
#endif
        }

#if NETSTANDARD2_0_OR_GREATER || NET || NETCOREAPP3_1_OR_GREATER
        /// <inheritdoc />
        public MNotifyClientInternal(IOptions<RemoteMNotifyOptions> options) : this(options.Value.BasicHttpsBinding,
            options.Value.EndpointAddress)
        {
            if (!options.IsNull() && !options.Value.IsNull())
                ClientCredentials.ClientCertificate.Certificate = options.Value.ServiceCertificate;
        }

        /// <inheritdoc />
        public MNotifyClientInternal(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress)
        {
            var previous = Interlocked.Exchange(ref Instance, this);
            if (previous != null && previous._createdByPing)
                AbortQuietly(previous);
        }

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
        public static MNotifyClientInternal Instance = new MNotifyClientInternal(_remoteMNotifyOptions.BasicHttpsBinding, _remoteMNotifyOptions.EndpointAddress);
#pragma warning restore IDE0090 // Use 'new(...)'
#endif

        /// <summary>
        ///     Ping MNotify service
        /// </summary>
        /// <returns></returns>
        /// <remarks>A Closed or Faulted <see cref="Instance" /> is replaced by a fresh client before answering.</remarks>
        public Result<PingResponse> Ping()
        {
            var client = Instance;
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                if (IsUnusable(client))
                    Recreate(client);

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

        /// <summary>
        ///     Check whether the client or its channel factory is Closed or Faulted
        /// </summary>
        /// <param name="client">Client</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static bool IsUnusable(MNotifyClientInternal client)
            => client.State is CommunicationState.Closed or CommunicationState.Faulted ||
               client.ChannelFactory.State is CommunicationState.Closed or CommunicationState.Faulted;

        /// <summary>
        ///     Abort a client and ignore any failure
        /// </summary>
        /// <param name="client">Client to abort</param>
        /// <remarks></remarks>
        private static void AbortQuietly(MNotifyClientInternal client)
        {
            try
            {
                client.Abort();
            }
            catch (Exception)
            {
                // best-effort: the client is already broken, a failing abort must not hide the Ping result
            }
        }

#if !NET45
        /// <summary>
        ///     Read the client certificate of a client in any communication state
        /// </summary>
        /// <param name="client">Client</param>
        /// <returns>The certificate, or null when the client has none</returns>
        /// <remarks>
        ///     The ClientCredentials property of a client in any state other than Created or Opening (Opened included) returns a
        ///     new empty instance, so the credentials are taken from the endpoint behaviors, where the original instance stays
        ///     registered.
        /// </remarks>
        private static X509Certificate2 GetClientCertificate(MNotifyClientInternal client)
        {
            foreach (var behavior in client.Endpoint.EndpointBehaviors)
            {
                if (behavior is ClientCredentials credentials)
                    return credentials.ClientCertificate.Certificate;
            }

            return null;
        }
#endif

        /// <summary>
        ///     Replace an unusable <see cref="Instance" /> with a fresh client
        /// </summary>
        /// <param name="old">The unusable client that Ping observed</param>
        /// <returns>The client that is <see cref="Instance" /> after the call</returns>
        /// <remarks>
        ///     Settings and the abort decision come from the instance re-read under the lock. The fresh client is published
        ///     with a compare-exchange because the public constructor publishes without taking the lock. A client that Ping
        ///     did not create (DI or consumer owned) is never aborted here, except on net45 where the library owns it.
        /// </remarks>
        private static MNotifyClientInternal Recreate(MNotifyClientInternal old)
        {
            lock (RecreateLock)
            {
                var current = Volatile.Read(ref Instance);
                if (current != null && !IsUnusable(current))
                    return current;

                var source = current ?? old;
#if NET45
                var fresh = new MNotifyClientInternal(_remoteMNotifyOptions.BasicHttpsBinding,
                    _remoteMNotifyOptions.EndpointAddress, _remoteMNotifyOptions.ServiceCertificate);
#else
                var fresh = new MNotifyClientInternal(source.Endpoint.Binding, source.Endpoint.Address,
                    GetClientCertificate(source));
#endif
                var winner = Interlocked.CompareExchange(ref Instance, fresh, current);
                if (!ReferenceEquals(winner, current))
                {
                    AbortQuietly(fresh);

                    return winner;
                }

#if NET45
                AbortQuietly(source);
#else
                if (source._createdByPing)
                    AbortQuietly(source);
#endif

                return fresh;
            }
        }
    }
}