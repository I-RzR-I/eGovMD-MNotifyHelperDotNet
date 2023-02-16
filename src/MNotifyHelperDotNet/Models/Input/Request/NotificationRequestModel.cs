// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MNotifyHelperDotNet
//  Author           : RzR
//  Created On       : 2023-02-11 09:31
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-12 22:36
// ***********************************************************************
//  <copyright file="NotificationRequestModel.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.ComponentModel;
using System.Runtime.CompilerServices;
using EntityMaxLengthTrim.Attributes;
using EntityMaxLengthTrim.Interceptors;
using MNotifyHelperDotNet.Enums;
using MNotifyHelperDotNet.Models.Input.Notification;

#endregion

// ReSharper disable ClassNeverInstantiated.Global

namespace MNotifyHelperDotNet.Models.Input.Request
{
    /// <summary>
    ///     Notification input model
    /// </summary>
    public class NotificationRequestModel : NotificationRequestBaseModel
    {
        private string _subject;

        /// <summary>
        ///     Sender notification
        /// </summary>
        public new Person Sender { get; set; }

        /// <summary>
        ///     Recipient notification
        /// </summary>
        public Person Recipient { get; set; }

        /// <summary>
        ///     Notification subject
        /// </summary>
        [MaxAllowedLength(200)]
        public string Subject
        {
            get => _subject;
            set
            {
                if (_subject == value)
                {
                    return;
                }

                _subject = value;
                OnPropertyChanged(nameof(Subject));
            }
        }

        /// <summary>
        ///     Notification content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        ///     Importance of notification
        /// </summary>
        public NotificationImportance NotificationImportance { get; set; }

        /// <summary>
        ///     Available languages
        /// </summary>
        public NotificationAvailableLanguages Language { get; set; }

        /// <summary>
        ///     Property changed event handler
        /// </summary>
        public new event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Property changed event
        /// </summary>
        /// <param name="propertyName">Changed property name</param>
        protected new virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            StringInterceptor.ApplyStringMaxAllowedLength(this, propertyName, false);
        }
    }
}