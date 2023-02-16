// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MNotifyHelperDotNet
//  Author           : RzR
//  Created On       : 2023-02-13 22:40
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-13 22:52
// ***********************************************************************
//  <copyright file="MNotifyConfigurationSectionNetFramework.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#if NET45

#region U S A G E S

using System;
using System.Configuration;
using AggregatedGenericResultMessage.Extensions.Common;
using MNotifyHelperDotNet.Helpers.ConfSection;
using MNotifyHelperDotNet.Models;

#endregion

namespace MNotifyHelperDotNet.Helpers
{
    /// <summary>
    ///     MNotify configuration section NET Framework
    /// </summary>
    internal static class MNotifyConfigurationSectionNetFramework
    {
        /// <summary>
        ///     Get MNotify configuration options
        /// </summary>
        /// <returns></returns>
        internal static RemoteMNotifyOptions GetRemoteMNotifyOptions()
        {
            var configuration = new RemoteMNotifyOptions();
            var configurationOptions = ConfigurationManager.GetSection("MNotifyConfigurationSection") as MNotifyConfigurationSection;
            if (configurationOptions.IsNull() || configurationOptions?.ServiceOptions.Count == 0)
            {
                throw new ArgumentNullException($@"{nameof(configurationOptions)} is null or empty");
            }

            // ReSharper disable once PossibleNullReferenceException
            foreach (MNotifyOptionConfigurationElement element in configurationOptions?.ServiceOptions)
            {
                if (element.Key.IsNullOrEmpty())
                {
                    throw new ArgumentNullException($@"{nameof(element.Key)} is null");
                }

                if (element.Value.IsNullOrEmpty())
                {
                    throw new ArgumentNullException($@"{nameof(element.Value)} is null");
                }

                switch (element.Key)
                {
                    case nameof(configuration.ServiceClientAddress):
                        configuration.ServiceClientAddress = element.Value;
                        break;
                    case nameof(configuration.ServiceCertificatePath):
                        configuration.ServiceCertificatePath = element.Value;
                        break;
                    case nameof(configuration.ServiceCertificatePassword):
                        configuration.ServiceCertificatePassword = element.Value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(@$"{nameof(element.Key)} is out of range");
                }
            }

            return configuration;
        }
    }
}

#endif