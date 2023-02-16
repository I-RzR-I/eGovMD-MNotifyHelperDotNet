// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MNotifyHelperDotNet
//  Author           : RzR
//  Created On       : 2023-02-08 07:41
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-10 04:16
// ***********************************************************************
//  <copyright file="MNotifyPostConfigureOptions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#if NETSTANDARD2_0_OR_GREATER || NET || NETCOREAPP3_1_OR_GREATER

#region U S A G E S

using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using DomainCommonExtensions.DataTypeExtensions;
using Microsoft.Extensions.Options;
using MNotifyHelperDotNet.Models;

// ReSharper disable CollectionNeverUpdated.Local

#endregion

namespace MNotifyHelperDotNet.Configurations.Options
{
    /// <summary>
    ///     MNotify POST configuration options
    /// </summary>
    public class MNotifyPostConfigureOptions : IPostConfigureOptions<RemoteMNotifyOptions>
    {
        /// <inheritdoc />
        public void PostConfigure(string name, RemoteMNotifyOptions mNotifyOptions)
        {
            if (string.IsNullOrWhiteSpace(mNotifyOptions.ServiceClientAddress))
            {
                throw new ArgumentException($"Please provide a {nameof(mNotifyOptions.ServiceClientAddress)}");
            }

            if (string.IsNullOrWhiteSpace(mNotifyOptions.ServiceCertificatePath))
            {
                throw new ArgumentException($"Please provide a {nameof(mNotifyOptions.ServiceCertificatePath)}");
            }

            if (string.IsNullOrWhiteSpace(mNotifyOptions.ServiceCertificatePassword))
            {
                throw new ArgumentException($"Please provide a {nameof(mNotifyOptions.ServiceCertificatePassword)}");
            }

            var certificate = new X509Certificate2Collection(CertificateLoader.Private(
                mNotifyOptions.ServiceCertificatePath,
                mNotifyOptions.ServiceCertificatePassword));

            if (certificate.Count.IsZero())
            {
                throw new ApplicationException("Invalid service certificate path or password");
            }

            if (certificate.Count.IsGreaterThanZero())
            {
                mNotifyOptions.ServiceCertificate = certificate[0];
            }

            mNotifyOptions.BasicHttpsBinding = new BasicHttpsBinding
            {
                Security =
                {
                    Transport = new HttpTransportSecurity
                    {
                        ClientCredentialType = HttpClientCredentialType.Certificate
                    },
                    Mode = BasicHttpsSecurityMode.Transport
                },
                MaxReceivedMessageSize = 2147483647,
                CloseTimeout = TimeSpan.FromMinutes(20)
            };

            mNotifyOptions.BasicHttpBinding = new BasicHttpBinding
            {
                Security =
                {
                    Transport = new HttpTransportSecurity
                    {
                        ClientCredentialType = HttpClientCredentialType.Certificate
                    },
                    Mode = BasicHttpSecurityMode.Transport
                },
                MaxReceivedMessageSize = 2147483647,
                CloseTimeout = TimeSpan.FromMinutes(20)
            };

            mNotifyOptions.EndpointAddress = new EndpointAddress(mNotifyOptions.ServiceClientAddress);
        }
    }
}
#endif