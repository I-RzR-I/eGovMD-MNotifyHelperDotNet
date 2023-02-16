// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MNotifyHelperDotNet
//  Author           : RzR
//  Created On       : 2023-02-11 10:23
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-12 22:36
// ***********************************************************************
//  <copyright file="NotificationSimpleRequestModel.cs" company="">
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

#endregion

// ReSharper disable ClassNeverInstantiated.Global

namespace MNotifyHelperDotNet.Models.Input.Request
{
    /// <summary>
    ///     Simple notification
    /// </summary>
    public class NotificationSimpleRequestModel : NotificationRequestBaseModel
    {
        private string _recipient;
        private string _subject;

        /// <summary>
        ///     Recipient notification
        /// </summary>
        [MaxAllowedLength(400)]
        public string Recipient
        {
            get => _recipient;
            set
            {
                if (_recipient == value)
                {
                    return;
                }

                _recipient = value;
                OnPropertyChanged(nameof(Recipient));
            }
        }

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
        ///     Content type
        /// </summary>
        public NotificationContentType ContentType { get; set; }

        /// <summary>
        ///     Property changed event handler
        /// </summary>
        public new event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Property changed event
        /// </summary>
        /// <param name="propertyName">Changed property name</param>
        protected new void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            StringInterceptor.ApplyStringMaxAllowedLength(this, propertyName, false);
        }
    }
}