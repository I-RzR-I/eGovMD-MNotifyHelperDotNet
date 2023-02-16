// ***********************************************************************
//  Assembly         : RzR.Shared.eGovMD.MNotifyHelperDotNet
//  Author           : RzR
//  Created On       : 2023-02-13 22:11
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-02-13 22:58
// ***********************************************************************
//  <copyright file="MNotifyConfigurationSection.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#if NET45

#region U S A G E S

using System.Configuration;

#endregion

// ReSharper disable ClassNeverInstantiated.Global

namespace MNotifyHelperDotNet.Helpers.ConfSection
{
    /// <summary>
    ///     MNotify service configuration sections
    /// </summary>
    internal class MNotifyConfigurationSection : ConfigurationSection
    {
        private const string _collectionName = "RemoteMNotifyOptions";

        /// <summary>
        ///     MNotify service options
        /// </summary>
        [ConfigurationProperty(_collectionName)]
        [ConfigurationCollection(typeof(MNotifyRemoteOptionElementCollection), AddItemName = "option")]
        internal MNotifyRemoteOptionElementCollection ServiceOptions
            => (MNotifyRemoteOptionElementCollection)base[_collectionName];
    }

    /// <summary>
    ///     MNotify remote service configuration element collection
    /// </summary>
    internal class MNotifyRemoteOptionElementCollection : ConfigurationElementCollection
    {
        /// <inheritdoc />
        protected override ConfigurationElement CreateNewElement()
            => new MNotifyOptionConfigurationElement();

        /// <inheritdoc />
        protected override object GetElementKey(ConfigurationElement element)
            => ((MNotifyOptionConfigurationElement)element).Key;
    }

    /// <summary>
    ///     MNotify option configuration element
    /// </summary>
    internal class MNotifyOptionConfigurationElement : ConfigurationElement
    {
        /// <summary>
        ///     Option key
        /// </summary>
        [ConfigurationProperty("key", IsRequired = true, IsKey = true)]
        internal string Key
        {
            get => (string)this["key"];
            set => this["key"] = value;
        }

        /// <summary>
        ///     Option value
        /// </summary>
        [ConfigurationProperty("value", IsRequired = true)]
        internal string Value
        {
            get => (string)this["value"];
            set => this["value"] = value;
        }
    }
}

#endif