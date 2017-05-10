using System;
using BabyBus.Model.Entities.Login;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;

namespace BabyBus.APITest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Login()
        {
            var user = new User
            {
                LoginName = "admin",
                Password = "123"
            };
            
        }
    }
}
