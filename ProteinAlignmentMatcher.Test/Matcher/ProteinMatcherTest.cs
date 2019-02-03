using System;
using MASReader;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProteinFundamentSearcher.Test
{
    [TestClass]
    public class ProteinMatcherTest
    {
        [TestMethod]
        public void SimpleMatchTest()
        {
            var match = ProteinMatcher.Match("AAAA", 0, "A2A", 1);

            Assert.IsTrue(match.HasMatch, "Has Match");
            Assert.AreEqual(0, match.Distance, "Distance is 0");
        }

        [TestMethod]
        public void SimpleMatchTestStartLater()
        {
            var match = ProteinMatcher.Match("XAAAA", 1, "A2A", 1);

            Assert.IsTrue(match.HasMatch, "Has Match");
            Assert.AreEqual(0, match.Distance, "Distance is 0");

        }
    }
}
