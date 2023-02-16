// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MNotifyHelperDotNet
//  Author           : RzR
//  Created On       : 2023-02-12 21:56
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-12 22:36
// ***********************************************************************
//  <copyright file="NotificationMultiContentRequestModel.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using EntityMaxLengthTrim.Attributes;
using EntityMaxLengthTrim.Interceptors;
using MNotifyHelperDotNet.Enums;

#endregion

// ReSharper disable ClassNeverInstantiated.Global

namespace MNotifyHelperDotNet.Models.Input.Request.MultiData
{
    /// <summary>
    ///     Notification content
    /// </summary>
    public class NotificationMultiContentRequestModel : INotifyPropertyChanged
    {
        private string _subject;

        /// <summary>
        ///     Content type
        /// </summary>
        public NotificationContentType ContentType { get; set; }

        /// <summary>
        ///     Available languages
        /// </summary>
        public NotificationAvailableLanguages Language { get; set; }

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
        ///     Content attachments
        /// </summary>
        public IEnumerable<NotificationMultiAttachmentRequestModel> Attachments { get; set; }
        
        /// <summary>
        ///     Property changed event handler
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Property changed event
        /// </summary>
        /// <param name="propertyName">Changed property name</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            StringInterceptor.ApplyStringMaxAllowedLength(this, propertyName, false);
        }
    }
}