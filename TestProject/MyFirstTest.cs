using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace TestProject
{
    [TestFixture]
    public class MyFirstTest
    {
        private ChromeDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void Start()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Manage().Window.Maximize();
        }

        [Test]
        public void FirstTest()
        {
            driver.Url = "http://www.google.com/";
        }

        [TearDown]
        public void Stop() 
        {
            driver.Quit();
            driver = null;
        }
    }
}
