using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScottyApps.ScottyBlogging.Entity;

namespace ScottyApps.ScottyBlogging.DomainTest
{
    [TestClass]
    public class DomainTest
    {
        [TestMethod]
        public void TestFilterTitle()
        {
            var illegalTitle = @"what  / is your \ question?  | please answer me!";
            Article article = new Article
                                  {
                                      Title = illegalTitle
                                  };
            Assert.AreEqual(article.LegalTitle, "what-is-your-question-please-answer-me!");
        }
    }
}
