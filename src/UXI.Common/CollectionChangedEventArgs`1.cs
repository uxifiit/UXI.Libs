using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.Common
{
    public class CollectionChangedEventArgs<T> : EventArgs
    {
        public IEnumerable<T> AddedItems { get; }
        public IEnumerable<T> RemovedItems { get; }

        private CollectionChangedEventArgs(IEnumerable<T> addedItems, IEnumerable<T> removedItems)
        {
            AddedItems = addedItems;
            RemovedItems = removedItems;
        }

        public static CollectionChangedEventArgs<T> Create(IEnumerable<T> addedItems, IEnumerable<T> removedItems)
        {
            return new CollectionChangedEventArgs<T>(Wrap(addedItems), Wrap(removedItems));
        }

        public static CollectionChangedEventArgs<T> Create(T addedItem, T removedItem)
        {
            return new CollectionChangedEventArgs<T>(Wrap(addedItem), Wrap(removedItem));
        }

        public static CollectionChangedEventArgs<T> CreateForAddedCollection(IEnumerable<T> addedItems)
        {
            return new CollectionChangedEventArgs<T>(Wrap(addedItems), Enumerable.Empty<T>());
        }

        public static CollectionChangedEventArgs<T> CreateForRemovedCollection(IEnumerable<T> removedItems)
        {

            return new CollectionChangedEventArgs<T>(Enumerable.Empty<T>(), Wrap(removedItems));
        }

        public static CollectionChangedEventArgs<T> CreateForAddedItem(T addedItem)
        {
            return new CollectionChangedEventArgs<T>(Wrap(addedItem), Enumerable.Empty<T>());
        }

        public static CollectionChangedEventArgs<T> CreateForRemovedItem(T removedItem)
        {
            return new CollectionChangedEventArgs<T>(Enumerable.Empty<T>(), Wrap(removedItem));
        }

        private static IEnumerable<T> Wrap(T item)
        {
            return item != null ? new ReadOnlyCollection<T>(new List<T>() { item }) : Enumerable.Empty<T>();
        }

        private static IEnumerable<T> Wrap(IEnumerable<T> items)
        {
            var list = items?.Where(i => i != null).ToList();
            return list != null && list.Any() 
                 ? list.AsReadOnly() 
                 : Enumerable.Empty<T>();
        }
    }
}
