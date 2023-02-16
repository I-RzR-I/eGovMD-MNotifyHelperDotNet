// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MNotifyHelperDotNet
//  Author           : RzR
//  Created On       : 2023-02-12 21:19
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-13 00:18
// ***********************************************************************
//  <copyright file="PersonMultipleRecipients.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

// ReSharper disable ClassNeverInstantiated.Global

namespace MNotifyHelperDotNet.Models.Input.Notification
{
    /// <summary>
    ///     Person model for multiple send
    /// </summary>
    public class PersonMultipleRecipients : Person
    {
        /// <summary>
        /// </summary>
        public PersonMultipleRecipients(string name, string email) : base(name, email)
        {
        }

        /// <summary>
        /// </summary>
        public PersonMultipleRecipients(string name, string email, bool copyRecipient = false, bool hiddenRecipient = false) : base(name, email)
        {
            CopyRecipient = copyRecipient;
            HiddenRecipient = hiddenRecipient;
        }

        /// <summary>
        ///     Copy recipient
        /// </summary>
        public bool CopyRecipient { get; set; }

        /// <summary>
        ///     Hidden recipient
        /// </summary>
        public bool HiddenRecipient { get; set; }
    }
}