using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Assert = NUnit.Framework.Assert;

namespace TestProject 
{
    [TestFixture]
    public class MyFirstTest : TestBase
    {

        /*[Test]
        public void FirstTest()
        {
            driver.Url = "http://www.google.com/";
        }*/

        [Test]
        public void LoginTest()
        {
            driver.Url = "http://localhost/litecart/admin/login.php";
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();
        }

        //[Test]
        //public void CheckMenuTest() 
        //{
        //    driver.Url = "http://localhost/litecart/admin";
        //    Login();
        //    IWebElement panel = driver.FindElement(By.Id("box-apps-menu"));
        //    IList<IWebElement> items = panel.FindElements(By.TagName("li"));
        //    for (int i = 1; i <= items.Count; i++) 
        //    {
        //        panel = driver.FindElement(By.Id("box-apps-menu"));
        //        IWebElement item = panel.FindElement(By.XPath("//li[" + i + "]"));
        //        try
        //        {
        //            item.Click();
        //            panel = driver.FindElement(By.Id("box-apps-menu"));
        //            item = panel.FindElement(By.XPath("//li[" + i + "]"));
        //            IList<IWebElement> subItems = item.FindElements(By.TagName("li"));
                    
        //            System.Threading.Thread.Sleep(2000);
        //        }
        //        catch (StaleElementReferenceException) 
        //        {
        //            item = panel.FindElement(By.XPath("//li[" + i + "]"));
        //            item.Click();
        //            System.Threading.Thread.Sleep(2000);
        //        }

        //    }
           
        //}

        [Test]
        public void CheckStickersTest() 
        {
            driver.Url = "http://localhost/litecart";
            IList<IWebElement> products = driver.FindElements(By.ClassName("image-wrapper"));
            foreach (IWebElement product in products) 
            {
                IList<IWebElement> stickersNew = product.FindElements(By.XPath("./div[@class='sticker new']"));
                IList<IWebElement> stickersSale = product.FindElements(By.XPath("./div[@class='sticker sale']"));
                int stickersTotal = stickersNew.Count + stickersSale.Count;
                Assert.IsTrue(stickersTotal == 1, "stickers count was " + stickersTotal);
            }

        }
       
    }
}
