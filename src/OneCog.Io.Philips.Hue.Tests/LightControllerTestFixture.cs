using FakeItEasy;
using Microsoft.Reactive.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Philips.Hue.Tests
{
    [TestFixture]
    public class LightControllerTestFixture
    {
        private void RunTest(IEnumerable<Light.ISource> initial, IObserver<Light.ISource> subscriber, Action<TestScheduler, LightController> test)
        {
            TestScheduler scheduler = new TestScheduler();
            LightController subject = new LightController(scheduler);

            using (subject.Connect(initial))
            {
                using (subject.Subscribe(subscriber))
                {
                    test(scheduler, subject);
                }
            }
        }

        [Test]
        public void ShouldEmitFirstLightChangeWithin10ms()
        {
            List<Light.ISource> changes = new List<Light.ISource>();

            RunTest(
                new[] { Helper.ConstructXyyBulb(0, 0.0, 0.0, 0.0) },
                Observer.Create<Light.ISource>(changes.Add),
                (scheduler, subject) =>
                {
                    Light.ISource lightA = Helper.ConstructXyyBulb(0, 0.5, 0.5, 0.5);

                    subject.OnNext(lightA);

                    Assert.That(changes.Count, Is.EqualTo(0));

                    scheduler.AdvanceBy(TimeSpan.FromMilliseconds(10).Ticks);

                    Assert.That(changes.Count, Is.EqualTo(1));
                    Assert.That(changes[0], Is.EqualTo(lightA).Using(LightComparer.Default));
                }
            );
        }

        [Test]
        public void ShouldEmitLightChangesAt10msIntervals()
        {
            List<Light.ISource> changes = new List<Light.ISource>();

            RunTest(
                new[] { Helper.ConstructRgbBulb(1, 0, 0, 0), Helper.ConstructRgbBulb(2, 0, 0, 0), Helper.ConstructRgbBulb(3, 0, 0, 0) },
                Observer.Create<Light.ISource>(changes.Add),
                (scheduler, subject) =>
                {
                    Light.ISource lightA = Helper.ConstructXyyBulb(1, 0.5, 0.5, 0.5);
                    Light.ISource lightB = Helper.ConstructXyyBulb(2, 0.3, 0.3, 0.3);
                    Light.ISource lightC = Helper.ConstructXyyBulb(3, 0.1, 0.1, 0.1);

                    subject.OnNext(lightA);
                    scheduler.AdvanceBy(TimeSpan.FromMilliseconds(2).Ticks);
                    subject.OnNext(lightB);
                    scheduler.AdvanceBy(TimeSpan.FromMilliseconds(2).Ticks);
                    subject.OnNext(lightC);
                    scheduler.AdvanceBy(TimeSpan.FromMilliseconds(2).Ticks);

                    Assert.That(changes.Count, Is.EqualTo(0));

                    scheduler.AdvanceBy(TimeSpan.FromMilliseconds(4).Ticks);

                    Assert.That(changes.Count, Is.EqualTo(1));

                    scheduler.AdvanceBy(TimeSpan.FromMilliseconds(10).Ticks);

                    Assert.That(changes.Count, Is.EqualTo(2));

                    scheduler.AdvanceBy(TimeSpan.FromMilliseconds(10).Ticks);

                    Assert.That(changes.Count, Is.EqualTo(3));

                    EnumerableAssert.AreEquivalent(changes, new[] { lightA, lightB, lightC, }, LightComparer.Default);
                }
            );
        }

        [Test]
        public void ShouldEmitLightWithMostSignificantChangeFirst()
        {
            List<Light.ISource> changes = new List<Light.ISource>();

            RunTest(
                new[] { Helper.ConstructRgbBulb(1, 0, 0, 0), Helper.ConstructRgbBulb(2, 0, 0, 0) },
                Observer.Create<Light.ISource>(changes.Add),
                (scheduler, subject) =>
                {
                    Light.ISource lightA = Helper.ConstructRgbBulb(1, 0.1, 0.1, 0.1);
                    Light.ISource lightB = Helper.ConstructRgbBulb(2, 1, 1, 1);

                    subject.OnNext(lightA);
                    scheduler.AdvanceBy(TimeSpan.FromMilliseconds(2).Ticks);
                    subject.OnNext(lightB);
                    scheduler.AdvanceBy(TimeSpan.FromMilliseconds(2).Ticks);

                    Assert.That(changes.Count, Is.EqualTo(0));

                    scheduler.AdvanceBy(TimeSpan.FromMilliseconds(6).Ticks);

                    Assert.That(changes.Count, Is.EqualTo(1));
                    Assert.That(changes[0], Is.EqualTo(lightB));

                    scheduler.AdvanceBy(TimeSpan.FromMilliseconds(10).Ticks);

                    Assert.That(changes.Count, Is.EqualTo(2));
                    Assert.That(changes[1], Is.EqualTo(lightA));
                }
            );
        }

        [Test]
        public void ShouldNotEmitLightThatHasNotChanged()
        {
            List<Light.ISource> changes = new List<Light.ISource>();

            RunTest(
                new[] { Helper.ConstructRgbBulb(1, 0, 0, 0) },
                Observer.Create<Light.ISource>(changes.Add),
                (scheduler, subject) =>
                {
                    Light.ISource lightA = Helper.ConstructRgbBulb(1, 0.1, 0.1, 0.1);

                    subject.OnNext(lightA);

                    scheduler.AdvanceBy(TimeSpan.FromMilliseconds(10).Ticks);

                    Assert.That(changes.Count, Is.EqualTo(1));
                    Assert.That(changes[0], Is.EqualTo(lightA));

                    Light.ISource lightB = Helper.ConstructRgbBulb(1, 0.1, 0.1, 0.1);

                    subject.OnNext(lightB);

                    scheduler.AdvanceBy(TimeSpan.FromMilliseconds(10).Ticks);

                    Assert.That(changes.Count, Is.EqualTo(1));
                }
            );
        }

        [Test]
        public void ShouldNotEmitLightThatHasNotChangedFromInitialState()
        {
            List<Light.ISource> changes = new List<Light.ISource>();

            RunTest(
                new[] { Helper.ConstructRgbBulb(1, 0.1, 0.1, 0.1) },
                Observer.Create<Light.ISource>(changes.Add),
                (scheduler, subject) =>
                {
                    Light.ISource lightA = Helper.ConstructRgbBulb(1, 0.1, 0.1, 0.1);

                    subject.OnNext(lightA);

                    scheduler.AdvanceBy(TimeSpan.FromMilliseconds(10).Ticks);

                    Assert.That(changes.Count, Is.EqualTo(0));
                }
            );
        }
    }
}
