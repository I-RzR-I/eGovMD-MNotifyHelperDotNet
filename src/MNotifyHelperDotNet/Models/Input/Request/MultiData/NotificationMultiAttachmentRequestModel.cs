// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MNotifyHelperDotNet
//  Author           : RzR
//  Created On       : 2023-02-12 22:52
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-13 00:36
// ***********************************************************************
//  <copyright file="NotificationMultiAttachmentRequestModel.cs" company="">
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

#endregion

// ReSharper disable ClassNeverInstantiated.Global

namespace MNotifyHelperDotNet.Models.Input.Request.MultiData
{
    /// <summary>
    ///     Notification attachments
    /// </summary>
    public class NotificationMultiAttachmentRequestModel : INotifyPropertyChanged
    {
        private string _id;
        private string _name;
        private string _type;
        private string _uri;

        /// <summary>
        ///     Id
        /// </summary>
        [MaxAllowedLength(36)]
        public string Id
        {
            get => _id;
            set
            {
                if (_id == value)
                {
                    return;
                }

                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        /// <summary>
        ///     Name
        /// </summary>
        [MaxAllowedLength(200)]
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value)
                {
                    return;
                }

                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        /// <summary>
        ///     Type
        /// </summary>
        [MaxAllowedLength(50)]
        public string Type
        {
            get => _type;
            set
            {
                if (_type == value)
                {
                    return;
                }

                _type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        /// <summary>
        ///     Content
        /// </summary>
        public byte[] Content { get; set; }

        /// <summary>
        ///     Uri
        /// </summary>
        [MaxAllowedLength(4096)]
        public string Uri
        {
            get => _uri;
            set
            {
                if (_uri == value)
                {
                    return;
                }

                _uri = value;
                OnPropertyChanged(nameof(Uri));
            }
        }

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