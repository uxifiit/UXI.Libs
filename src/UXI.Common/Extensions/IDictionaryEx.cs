using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Common.Extensions;

namespace System.Collections.Generic
{
    public static class IDictionaryEx
    {
        /// <summary>
        /// Attempts to add new key/value pair to dictionary if the key has not already been defined. 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue">The key to be added.</typeparam>
        /// <param name="dictionary">The dictionary which the key/value pair should be added to.</param>
        /// <param name="key">The key to be added.</param>
        /// <param name="value">The key to be added.</param>
        /// <returns>true if the key/value was added, otherwise false.</returns>
        public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key) == false)
            {
                dictionary.Add(key, value);
                return true;
            }
            return false;
        }
 

        /// <summary>
        /// Uses the specified functions to add a key/value pair to the <see cref="IDictionary{TKey, TValue}"/>
        /// if the key does not already exist, or to update a key/value pair in the <see cref="IDictionary{TKey, TValue}"/>
        /// if the key already exists.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary">The dictionary which the key should be added to or updated in.</param>
        /// <param name="key">The key to be added or whose value should be updated</param>
        /// <param name="addValueFactory">The function used to generate a value for an absent key</param>
        /// <param name="updateValueFactory">The function used to generate a new value for an existing key based on the key's existing value</param>
        /// <returns> The new value for the key. This will be either be the result of addValueFactory (if the key was absent) or the result of updateValueFactory (if the key was present).</returns>
        /// <exception cref="System.ArgumentNullException">key, addValueFactory, or updateValueFactory is null.</exception>
        /// <exception cref="System.OverflowException">The dictionary already contains the maximum number of elements (<see cref="System.Int32.MaxValue"/>).</exception>
        public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory)
        {
            dictionary.ThrowIfNull(nameof(dictionary));
            key.ThrowIfNull(nameof(key));
            addValueFactory.ThrowIfNull(nameof(addValueFactory));
            updateValueFactory.ThrowIfNull(nameof(updateValueFactory));

            TValue oldValue;
            TValue newValue;
            if (dictionary.TryGetValue(key, out oldValue))
            {
                newValue = updateValueFactory.Invoke(key, oldValue);
                dictionary[key] = newValue;
            }
            else
            {
                newValue = addValueFactory.Invoke(key);
                dictionary.Add(key, newValue);        
            }

            return newValue;
        }


        /// <summary>
        /// Adds a key/value pair to the <see cref="IDictionary{TKey, TValue}"/>
        /// if the key does not already exist, or updates a key/value pair in the <see cref="IDictionary{TKey, TValue}"/>
        /// by using the specified function if the key already exists.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary">The dictionary which the key should be added to or updated in.</param>
        /// <param name="key">The key to be added or whose value should be updated</param>
        /// <param name="addValue">The value to be added for an absent key</param>
        /// <param name="updateValueFactory">The function used to generate a new value for an existing key based on the key's existing value</param>
        /// <returns>The new value for the key. This will be either be addValue (if the key was absent) or the result of updateValueFactory (if the key was present).</returns>
        /// <exception cref="System.ArgumentNullException">key or updateValueFactory is null.</exception>
        /// <exception cref="System.OverflowException">The dictionary already contains the maximum number of elements (<see cref="System.Int32.MaxValue"/>).</exception>
        public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory)
        {
            dictionary.ThrowIfNull(nameof(dictionary));
            key.ThrowIfNull(nameof(key));
            updateValueFactory.ThrowIfNull(nameof(updateValueFactory));

            TValue oldValue;
            TValue newValue;
            if (dictionary.TryGetValue(key, out oldValue))
            {
                newValue = updateValueFactory.Invoke(key, oldValue);
                dictionary[key] = newValue;
            }
            else
            {
                newValue = addValue;
                dictionary.Add(key, newValue);
            }

            return newValue;
        }
    }
}
