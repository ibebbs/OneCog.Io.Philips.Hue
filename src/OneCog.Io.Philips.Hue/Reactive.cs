using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Philips.Hue
{
    public class Reactive
    {
        private Subject<IScene> _subject;

        public Reactive(IApi api)
        {
            _subject = new Subject<IScene>();

        }

        public IObserver<IScene> Observer
        {
            get { return _subject; }
        }
    }
}
