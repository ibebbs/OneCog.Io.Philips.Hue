using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OneCog.Io.Philips.Hue.Dto
{
    public class Index<T> : KeyedCollection<int, Indexed<T>>
    {
        public Index() : base() { }

        public Index(IEnumerable<Indexed<T>> collection) : this()
        {
            AddRange(collection);
        }

        protected override int GetKeyForItem(Indexed<T> item)
        {
            return item.Index;
        }

        public void AddRange(IEnumerable<Indexed<T>> collection)
        {
            foreach (Indexed<T> item in collection)
            {
                Add(item);
            }
        }
    }
}
