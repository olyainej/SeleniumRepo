using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
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
            IList<IWebElement> products = driver.FindElements(By.XPath("//li[contains(@class, 'productMain')]"));
            foreach (IWebElement product in products)
            {
                IList<IWebElement> stickers = product.FindElements(By.XPath(".//div[contains(@class, 'sticker')]"));
                Assert.IsTrue(stickers.Count == 1, "stickers count was " + stickers.Count);
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

        [Test]
        public void ProductCheckTest() 
        {
            driver.Url = "http://localhost/litecart";
            IWebElement campaignBlock = driver.FindElement(By.Id("box-campaigns"));
            IWebElement productMain = campaignBlock.FindElement(By.TagName("li"));
            string nameMain = productMain.FindElement(By.ClassName("name")).Text;
            string priceMain = productMain.FindElement(By.ClassName("regular-price")).Text;
            string priceColorMain = productMain.FindElement(By.ClassName("regular-price")).GetCssValue("color").Replace("rgba(", string.Empty);
            string [] rgbRegularPriceMain = priceColorMain.Split(',');
            int redRegularPriceMain = Convert.ToInt32(rgbRegularPriceMain[0]);
            int greenRegularPriceMain = Convert.ToInt32(rgbRegularPriceMain[1]);
            int blueRegularPriceMain = Convert.ToInt32(rgbRegularPriceMain[2]);
            Assert.AreEqual(redRegularPriceMain, greenRegularPriceMain, blueRegularPriceMain);
            string styleRegularPiceMain = productMain.FindElement(By.ClassName("regular-price")).GetCssValue("text-decoration-line");
            Assert.AreEqual(styleRegularPiceMain, "line-through");
            Size sizeRegularPriceMain = productMain.FindElement(By.ClassName("regular-price")).Size;
            int widthRegularPriceMain = sizeRegularPriceMain.Width;
            int heightRegularPriceMain = sizeRegularPriceMain.Height;
            string campPriceMain = productMain.FindElement(By.ClassName("campaign-price")).Text;
            string campPriceColorMain = productMain.FindElement(By.ClassName("campaign-price")).GetCssValue("color").Replace("rgba(", string.Empty);
            string [] rgbCampPriceMain = campPriceColorMain.Split(',');
            int greenCampPriceMain = Convert.ToInt32(rgbCampPriceMain[1]);
            int blueCampPricemain = Convert.ToInt32(rgbCampPriceMain[2]);
            Assert.AreEqual(greenCampPriceMain, 0);
            Assert.AreEqual(blueCampPricemain, 0);
            int styleCampPriceMain = Convert.ToInt32(productMain.FindElement(By.ClassName("campaign-price")).GetCssValue("font-weight"));
            Assert.IsTrue(styleCampPriceMain >= 700);
            Size sizeCampPriceMain = productMain.FindElement(By.ClassName("campaign-price")).Size;
            int widthCampPriceMain = sizeCampPriceMain.Width;
            int heightCampPriceMain = sizeCampPriceMain.Height;
            Assert.True(widthCampPriceMain > widthRegularPriceMain);
            Assert.True(heightCampPriceMain > heightRegularPriceMain);
            productMain.Click();
            IWebElement productPage = driver.FindElement(By.Id("box-product"));
            string namePage = productPage.FindElement(By.ClassName("title")).Text;
            string pricePage = productPage.FindElement(By.ClassName("regular-price")).Text;
            string priceColorPage = productPage.FindElement(By.ClassName("regular-price")).GetCssValue("color").Replace("rgba(", string.Empty);
            string [] rgbRegularPricePage = priceColorPage.Split(',');
            int redRegularPricePage = Convert.ToInt32(rgbRegularPricePage[0]);
            int greenRegularPricePage = Convert.ToInt32(rgbRegularPricePage[1]);
            int blueRegularPricePage = Convert.ToInt32(rgbRegularPricePage[2]);
            Assert.AreEqual(redRegularPricePage, greenRegularPricePage, blueRegularPricePage);
            string styleRegularPricePage = productPage.FindElement(By.ClassName("regular-price")).GetCssValue("text-decoration-line");
            Assert.AreEqual(styleRegularPricePage, "line-through");
            Size sizeRegularPricePage = productPage.FindElement(By.ClassName("regular-price")).Size;
            int widthRegularPricePage = sizeRegularPricePage.Width;
            int heightRegularPricePage = sizeRegularPricePage.Height;
            string campPricePage = productPage.FindElement(By.ClassName("campaign-price")).Text;
            string campPriceColorPage = productPage.FindElement(By.ClassName("campaign-price")).GetCssValue("color").Replace("rgba(", string.Empty);
            string[] rgbCampPricePage = campPriceColorPage.Split(',');
            int greenCampPricePage = Convert.ToInt32(rgbCampPricePage[1]);
            int blueCampPricePage = Convert.ToInt32(rgbCampPricePage[2]);
            Assert.AreEqual(greenCampPricePage, 0);
            Assert.AreEqual(blueCampPricePage, 0);
            int styleCampPricePage = Convert.ToInt32(productPage.FindElement(By.ClassName("campaign-price")).GetCssValue("font-weight"));
            Assert.IsTrue(styleCampPricePage >= 700);
            Size sizeCampPricePage = productPage.FindElement(By.ClassName("campaign-price")).Size;
            int widthCampPricePage = sizeCampPricePage.Width;
            int heightCampPricePage = sizeCampPricePage.Height;
            Assert.IsTrue(widthCampPricePage > widthRegularPricePage);
            Assert.IsTrue(heightCampPricePage > heightRegularPricePage);
            Assert.AreEqual(nameMain, namePage);
            Assert.AreEqual(priceMain, pricePage);
            Assert.AreEqual(campPriceMain, campPricePage);
        }
    }
}
