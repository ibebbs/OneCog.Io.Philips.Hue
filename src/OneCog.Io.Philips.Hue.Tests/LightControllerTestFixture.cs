using FakeItEasy;
using Microsoft.Reactive.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Philips.Hue.Tests
{
    [TestFixture]
    public class LightControllerTestFixture
    {
        [Test]
        public void ShouldEmitFirstLightChangeWithin10ms()
        {
            TestScheduler scheduler = new TestScheduler();

            List<Light.ISource> changes = new List<Light.ISource>();

            Light.ISource lightA = Helper.ConstructBulb(0, 0.5, 0.5, 0.5);

            LightController subject = new LightController(scheduler);

            using (subject.Connect(new[] { Helper.ConstructBulb(0, 0.0, 0.0, 0.0) }))
            {
                using (subject.Subscribe(changes.Add))
                {
                    subject.OnNext(lightA);

                    Assert.That(changes.Count, Is.EqualTo(0));

                    scheduler.AdvanceBy(TimeSpan.FromMilliseconds(10).Ticks);

                    Assert.That(changes.Count, Is.EqualTo(1));
                    Assert.That(changes[0], Is.EqualTo(lightA).Using(LightComparer.Default));
                }
            }
        }
    }
}
