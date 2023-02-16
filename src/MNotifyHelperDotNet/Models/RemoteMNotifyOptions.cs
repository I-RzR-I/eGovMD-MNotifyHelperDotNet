// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MNotifyHelperDotNet
//  Author           : RzR
//  Created On       : 2023-02-08 07:41
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-10 08:23
// ***********************************************************************
//  <copyright file="RemoteMNotifyOptions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

// ReSharper disable ClassNeverInstantiated.Global

#endregion

namespace MNotifyHelperDotNet.Models
{
    /// <summary>
    ///     Remote MSign option
    /// </summary>
    /// <remarks></remarks>
    public class RemoteMNotifyOptions
    {
        /// <summary>
        ///     Gets or sets link/address to remote service.
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        public string ServiceClientAddress { get; set; }

        /// <summary>
        ///     Path of service certificate.
        /// </summary>
        public string ServiceCertificatePath { get; set; }

        /// <summary>
        ///     Password for service certificate.
        /// </summary>
        public string ServiceCertificatePassword { get; set; }

        /// <summary>
        ///     Service certificate.
        /// </summary>
        public X509Certificate2 ServiceCertificate { get; set; }

        /// <summary>
        ///     Gets or sets BasicHttpsBinding
        /// </summary>
        public BasicHttpsBinding BasicHttpsBinding { get; set; }

        /// <summary>
        ///     Gets or sets BasicHttpBinding.
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        public BasicHttpBinding BasicHttpBinding { get; set; }

        /// <summary>
        ///     Gets or sets EndpointAddress
        /// </summary>
        public EndpointAddress EndpointAddress { get; set; }
    }
}