﻿namespace BEL.DataAccessLayer
{
    using System.Collections.Generic;

    /// <summary>
    /// Global CachingProvider
    /// </summary>
    public class GlobalCachingProvider : CachingProviderBase, IGlobalCachingProvider
    {
        #region Singleton
        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalCachingProvider"/> class.
        /// </summary>
        protected GlobalCachingProvider()
        {
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static GlobalCachingProvider Instance
        {
            get
            {
                return Nested.Instance;
            }
        }

        /// <summary>
        /// Nested Class
        /// </summary>
        public class Nested
        {
            /// <summary>
            /// The instance
            /// </summary>
            internal static readonly GlobalCachingProvider Instance = new GlobalCachingProvider();
        }

        #endregion

        #region ICachingProvider

        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public virtual new void AddItem(string key, object value)
        {
            base.AddItem(key, value);
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// object Data
        /// </returns>
        public virtual object GetItem(string key)
        {
            ////Remove default is true because it's Global Cache!
            return base.GetItem(key, true);
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="remove">if set to <c>true</c> [remove].</param>
        /// <returns>
        /// item object
        /// </returns>
        public virtual new object GetItem(string key, bool remove)
        {
            return base.GetItem(key, remove);
        }

#pragma warning disable CS0114 // 'GlobalCachingProvider.GetAllKeys()' hides inherited member 'CachingProviderBase.GetAllKeys()'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        /// <summary>
        /// Gets all keys.
        /// </summary>
        /// <returns>
        /// list of keys
        /// </returns>
        public virtual List<string> GetAllKeys()
#pragma warning restore CS0114 // 'GlobalCachingProvider.GetAllKeys()' hides inherited member 'CachingProviderBase.GetAllKeys()'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            return base.GetAllKeys();
        }

#pragma warning disable CS0114 // 'GlobalCachingProvider.RemoveItem(string)' hides inherited member 'CachingProviderBase.RemoveItem(string)'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        /// <summary>
        /// Removes the item.
        /// </summary>
        /// <param name="key">The key.</param>
        public virtual void RemoveItem(string key)
#pragma warning restore CS0114 // 'GlobalCachingProvider.RemoveItem(string)' hides inherited member 'CachingProviderBase.RemoveItem(string)'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword.
        {
            base.RemoveItem(key);
        }
        #endregion
    }
}
