// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MNotifyHelperDotNet
//  Author           : RzR
//  Created On       : 2023-02-08 07:41
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-10 04:19
// ***********************************************************************
//  <copyright file="MNotifyPostConfigureOptionsNetFramework.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#if NET45

#region U S A G E S

using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using DomainCommonExtensions.DataTypeExtensions;
using MNotifyHelperDotNet.Helpers;
using MNotifyHelperDotNet.Models;
// ReSharper disable CollectionNeverUpdated.Local

#endregion

namespace MNotifyHelperDotNet.Configurations.Options
{
    /// <summary>
    ///     Post sign configuration
    /// </summary>
    /// <remarks></remarks>
    internal static class MNotifyPostConfigureOptionsNetFramework
    {
        internal static RemoteMNotifyOptions InitOptions()
        {
            var serviceConfigurationOptions = MNotifyConfigurationSectionNetFramework.GetRemoteMNotifyOptions();

            var remoteServiceClientAddress = serviceConfigurationOptions.ServiceClientAddress;
            if (remoteServiceClientAddress.IsNullOrEmpty())
            {
                throw new ArgumentException($"Please provide a {nameof(serviceConfigurationOptions.ServiceClientAddress)}");
            }

            var serviceCertificatePath = serviceConfigurationOptions.ServiceCertificatePath;
            if (serviceCertificatePath.IsNullOrEmpty())
            {
                throw new ArgumentException($"Please provide a {nameof(serviceConfigurationOptions.ServiceCertificatePath)}");
            }

            var serviceCertificatePassword = serviceConfigurationOptions.ServiceCertificatePassword;
            if (serviceCertificatePassword.IsNullOrEmpty())
            {
                throw new ArgumentException($"Please provide a {nameof(serviceConfigurationOptions.ServiceCertificatePassword)}");
            }

            var mSignOptions = new RemoteMNotifyOptions
            {
                ServiceClientAddress = remoteServiceClientAddress,
                ServiceCertificatePath = serviceCertificatePath,
                ServiceCertificatePassword = serviceCertificatePassword,
                BasicHttpBinding = new BasicHttpBinding
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
                    CloseTimeout = TimeSpan.FromMinutes(5)
                },
                BasicHttpsBinding = new BasicHttpsBinding
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
                    CloseTimeout = TimeSpan.FromMinutes(5)
                }
            };

            var certificate = new X509Certificate2Collection(CertificateLoader.Private(
                mSignOptions.ServiceCertificatePath,
                mSignOptions.ServiceCertificatePassword));

            if (certificate.Count.IsZero())
            {
                throw new ApplicationException("Invalid service certificate path or password");
            }

            if (certificate.Count.IsGreaterThanZero())
            {
                mSignOptions.ServiceCertificate = certificate[0];
            }

            mSignOptions.EndpointAddress = new EndpointAddress(mSignOptions.ServiceClientAddress);

            return mSignOptions;
        }
    }
}

#endif