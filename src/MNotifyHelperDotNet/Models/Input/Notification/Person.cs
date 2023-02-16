// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MNotifyHelperDotNet
//  Author           : RzR
//  Created On       : 2023-02-11 08:21
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-13 00:18
// ***********************************************************************
//  <copyright file="Person.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

// ReSharper disable ClassNeverInstantiated.Global

#region U S A G E S

using System.ComponentModel;
using System.Runtime.CompilerServices;
using EntityMaxLengthTrim.Attributes;
using EntityMaxLengthTrim.Interceptors;

#endregion

namespace MNotifyHelperDotNet.Models.Input.Notification
{
    /// <summary>
    ///     Notification person details
    /// </summary>
    public class Person : INotifyPropertyChanged
    {
        private string _email;
        private string _name;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MNotifyHelperDotNet.Models.Input.Notification.Person" /> class.
        /// </summary>
        /// <param name="name">Person name</param>
        /// <param name="email">Person email</param>
        /// <remarks></remarks>
        public Person(string name, string email)
        {
            Name = name;
            Email = email;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MNotifyHelperDotNet.Models.Input.Notification.Person" /> class.
        /// </summary>
        /// <remarks></remarks>
        public Person()
        {
        }

        /// <summary>
        ///     Usually person full name
        /// </summary>
        [MaxAllowedLength(400)]
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
        ///     Person contact email
        /// </summary>
        [MaxAllowedLength(400)]
        public string Email
        {
            get => _email;
            set
            {
                if (_email == value)
                {
                    return;
                }

                _email = value;
                OnPropertyChanged(nameof(Email));
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