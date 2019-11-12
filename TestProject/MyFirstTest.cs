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

        [Test]
        public void CheckCountriesSorting()
        {
            driver.Url = "http://localhost/litecart/admin/?app=countries&doc=countries";
            Login();
            IWebElement countriesTable = driver.FindElement(By.Name("countries_form"));
            IList<IWebElement> rows = countriesTable.FindElements(By.ClassName("row"));
            List<string> names = new List<string>();
            List<string> countriesWithZones = new List<string>();
            foreach (IWebElement row in rows)
            {
                IWebElement country = row.FindElement(By.XPath(".//a"));
                names.Add(country.Text);
                string zones = row.FindElement(By.XPath("./td[6]")).Text;
                int count = Convert.ToInt32(zones);
                if (count > 0)
                    countriesWithZones.Add(country.Text);
            }
            List<string> sortedNames = new List<string>();
            sortedNames.AddRange(names);
            sortedNames.Sort();
            Assert.IsTrue(CompareLists(names, sortedNames));
            foreach (string country in countriesWithZones)
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[contains(text(),'" + country + "')]")));
                driver.FindElement(By.XPath("//a[contains(text(),'" + country + "')]")).Click(); 
                IWebElement zonesTable = driver.FindElement(By.Id("table-zones"));
                IList<IWebElement> zoneRows = zonesTable.FindElements(By.XPath(".//input[contains(@name,'[name]')]"));
                List<string> zoneNames = new List<string>();
                foreach (IWebElement zoneRow in zoneRows)
                {
                    zoneNames.Add(zoneRow.GetAttribute("value"));
                }
                zoneNames.Remove("");
                List<string> sortedZones = new List<string>();
                sortedZones.AddRange(zoneNames);
                sortedZones.Sort();
                Assert.IsTrue(CompareLists(sortedZones, zoneNames));
                driver.FindElement(By.XPath("//span[contains(text(),'Countries')]")).Click();     
            }
        }

        [Test]
        public void CheckGeoZonesSorting() 
        {
            driver.Url = "http://localhost/litecart/admin/?app=geo_zones&doc=geo_zones";
            Login();
            IWebElement countryTable = driver.FindElement(By.Name("geo_zones_form"));
            IList<IWebElement> countryRows = countryTable.FindElements(By.XPath(".//tr[@class='row']"));
            List<string> countryNames = new List<string>();
            foreach (IWebElement countryRow in countryRows)
                countryNames.Add(countryRow.FindElement(By.XPath(".//a")).Text);
            foreach (string countryName in countryNames) 
            {
                driver.FindElement(By.XPath("//a[contains(text(),'" + countryName + "')]")).Click();
                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("table-zones")));
                IWebElement zoneTable = driver.FindElement(By.Id("table-zones"));
                IList<IWebElement> zoneRows = zoneTable.FindElements(By.XPath(".//select[contains(@name,'[zone_code]')]"));
                List<string> zoneNames = new List<string>();
                foreach (IWebElement zoneRow in zoneRows) 
                {
                    SelectElement selectElement = new SelectElement(zoneRow);
                    zoneNames.Add(selectElement.SelectedOption.Text);
                }
                List<string> sortedZones = new List<string>();
                sortedZones.AddRange(zoneNames);
                sortedZones.Sort();
                Assert.IsTrue(CompareLists(sortedZones, zoneNames));
                driver.FindElement(By.XPath("//span[contains(text(),'Geo Zones')]")).Click();
            }
        }
       
    }
}
