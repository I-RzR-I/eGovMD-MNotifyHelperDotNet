// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MNotifyHelperDotNet
//  Author           : RzR
//  Created On       : 2023-02-11 10:17
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-12 22:36
// ***********************************************************************
//  <copyright file="NotificationMultiRecipientRequestModel.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using MNotifyHelperDotNet.Enums;
using MNotifyHelperDotNet.Models.Input.Notification;

#endregion

// ReSharper disable ClassNeverInstantiated.Global

namespace MNotifyHelperDotNet.Models.Input.Request.MultiData
{
    /// <summary>
    ///     Notification input model
    /// </summary>
    public class NotificationMultiRecipientRequestModel : NotificationRequestBaseModel
    {
        /// <summary>
        ///     Sender notification
        /// </summary>
        public new Person Sender { get; set; }

        /// <summary>
        ///     Recipient notification
        /// </summary>
        public IEnumerable<PersonMultipleRecipients> Recipients { get; set; }

        /// <summary>
        ///     Notification contents
        /// </summary>
        public IEnumerable<NotificationMultiContentRequestModel> Contents { get; set; }

        /// <summary>
        ///     Importance of notification
        /// </summary>
        public NotificationImportance NotificationImportance { get; set; }

        /// <summary>
        ///     Separate recipients
        /// </summary>
        public bool SeparateRecipients { get; set; }
    }
}