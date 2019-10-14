using System;
using ClassLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iAboutMoney.UnitTests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void Test_GetBetWeenMethod_Expect_100()
        {
            
            string test = "xyzEgyenleg: +100 HUFxyzz";

            string result = SmsFileWorker.GetBetween(test, "Egyenleg: +", " HUF");

            Assert.AreEqual("100", result);
        }

        [TestMethod]
        public void Test_GetTokenMethod_Expect_64()
        {
            int number = 64;

            string testtoken = DropboxClass.GetToken();
            int result = testtoken.Length;

            Assert.AreEqual(number, result);
        }        
    }
}
