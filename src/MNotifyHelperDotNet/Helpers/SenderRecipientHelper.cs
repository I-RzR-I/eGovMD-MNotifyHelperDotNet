// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MNotifyHelperDotNet
//  Author           : RzR
//  Created On       : 2023-02-11 13:58
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-11 21:11
// ***********************************************************************
//  <copyright file="SenderRecipientHelper.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using System.Linq;
using MNotifyHelperDotNet.Models.Input.Notification;

#endregion

namespace MNotifyHelperDotNet.Helpers
{
    /// <summary>
    ///     Sender &amp; Recipient helper
    /// </summary>
    internal static class SenderRecipientHelper
    {
        internal static string GetSenderRecipientForSimpleNotification(string person) 
            => @"{" + "Email: '" + person + "'" + @"}";

        internal static string GetSenderRecipientSendDetails(Person person) 
            => @"{" + string.Format("Name: {0}{1}{0}, Email: {0}{2}{0}", @"'", person.Name, person.Email) + @"}";

        internal static IEnumerable<string> GetSenderRecipientSendDetails(IEnumerable<Person> persons)
            => persons.Select(person => @"{" + string.Format("Name: {0}{1}{0}, Email: {0}{2}{0}", @"'", person.Name, person.Email) + @"}").ToList();
    }
}