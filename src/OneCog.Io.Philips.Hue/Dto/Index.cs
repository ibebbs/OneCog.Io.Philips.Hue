using System.Collections.ObjectModel;

namespace OneCog.Io.Philips.Hue.Dto
{
    public class Index<T> : KeyedCollection<int, Indexed<T>>
    {
        protected override int GetKeyForItem(Indexed<T> item)
        {
            return item.Index;
        }
    }
}
