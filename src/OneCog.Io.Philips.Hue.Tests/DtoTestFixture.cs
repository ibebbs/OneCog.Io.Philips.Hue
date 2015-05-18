using NUnit.Framework;
using OneCog.Io.Philips.Hue.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Philips.Hue.Tests
{
    [TestFixture]
    public class DtoTestFixture
    {
        [Test]
        public void CanDeserializeState()
        {
            State state = Serializer.Json.Deserialize<State>(Resources.ConfigurationResponse);

            Assert.That(state, Is.Not.Null);
        }
    }
}
