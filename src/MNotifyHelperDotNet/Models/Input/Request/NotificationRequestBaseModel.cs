// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MNotifyHelperDotNet
//  Author           : RzR
//  Created On       : 2023-02-12 22:29
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-13 00:21
// ***********************************************************************
//  <copyright file="NotificationRequestBaseModel.cs" company="">
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

namespace MNotifyHelperDotNet.Models.Input.Request
{
    /// <summary>
    ///     Base notification model
    /// </summary>
    public class NotificationRequestBaseModel : INotifyPropertyChanged
    {
        private string _notificationType;
        private string _sender;

        /// <summary>
        ///     Notification type
        /// </summary>
        [MaxAllowedLength(200)]
        public string NotificationType
        {
            get => _notificationType;
            set
            {
                if (_notificationType == value)
                {
                    return;
                }

                _notificationType = value;
                OnPropertyChanged(nameof(NotificationType));
            }
        }

        /// <summary>
        ///     Sender notification
        /// </summary>
        [MaxAllowedLength(400)]
        public string Sender
        {
            get => _sender;
            set
            {
                if (_sender == value)
                {
                    return;
                }

                _sender = value;
                OnPropertyChanged(nameof(Sender));
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