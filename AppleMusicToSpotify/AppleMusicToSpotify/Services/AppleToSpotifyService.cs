using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppleMusicToSpotify.Services
{
    public class AppleToSpotifyService
    {
        static ChromeDriver driver;
        static string appleMusicUrl;
        static string spotifyUrl;
        static string google = "https://www.google.com/";
        static Track track;


        static DateTime start;
        static DateTime end;

        public string GetSpotifyLinkFromApple(string appleMusicLink)
        {
            start = DateTime.Now;
            appleMusicUrl = appleMusicLink;
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments(new List<string>() { "no-sandbox", "headless", "disable-gpu" });
            track = new Track();
            driver = new ChromeDriver(chromeOptions);

            SearchFor(appleMusicUrl);
            if (driver.FindElements(By.XPath("//a[contains(@href, 'music.apple.com')]/h3")).Count > 0)
            {
                GetTrackDetails();
            }
            else
            {
                GetTrackDetailsFromApple();
            }
            GetSpotifyUrl();
            SearchFor($"{track.Artist} {track.TrackName} spotify");
            GetSpotifyLink();
            end = DateTime.Now;
            TimeSpan duration = end - start;
            driver.Close();
            driver.Quit();
            return spotifyUrl;
        }
        private static void GetTrackDetailsFromApple()
        {
            driver.Navigate().GoToUrl(appleMusicUrl);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            //while (!js.ExecuteScript("return document.readyState").ToString().Equals("complete"))
            while (driver.FindElements(By.XPath("//*[contains(@class, 'songs-list-row--selected')]")).Count < 1 
                    || string.IsNullOrWhiteSpace(driver.FindElement(By.XPath("//*[contains(@class, 'songs-list-row--selected')]")).Text)
                    || driver.FindElements(By.XPath("//*[contains(@href, '/artist/')]")).Count < 1)
            {
                Thread.Sleep(100);
            }
            

            String[] spearator = { "\r\n" };
            var fullString = driver.FindElement(By.XPath("//*[contains(@class, 'songs-list-row--selected')]")).Text;
            var stringArray = fullString.Split(spearator, System.StringSplitOptions.RemoveEmptyEntries);
            track.TrackName = stringArray[1];
            track.Artist = driver.FindElement(By.XPath("//*[contains(@href, '/artist/')]")).Text;
        }

        private static void GetSpotifyLink()
        {
            spotifyUrl = driver.FindElement(By.XPath("//a[contains(@href, 'spotify.com')]")).GetAttribute("href");

        }

        private static void GetSpotifyUrl()
        {
            driver.Navigate().GoToUrl(google);
        }

        private static void GetTrackDetails()
        {
            var trackString = driver.FindElement(By.XPath($"//a[contains(@href, 'music.apple.com')]/h3")).Text;
            trackString = trackString.Split('-')[0];
            String[] spearator = { " by " };

            var trackArr = trackString.Split(spearator, System.StringSplitOptions.RemoveEmptyEntries);
            track = new Track();
            track.Artist = trackArr[1].Replace("— Song on Apple Music", "").Trim();
            track.TrackName = trackArr[0].Trim();
        }

        private static void SearchFor(string searchTerm)
        {
            driver.Navigate().GoToUrl(google);
            var searchbox = driver.FindElement(By.XPath("//*[@title='Search']"));

            if (driver.FindElements(By.XPath("//*[text()='Reject all']/..")).Count > 0)
            {
                var rejectButton = driver.FindElement(By.XPath("//*[text()='Reject all']/.."));
                rejectButton.Click();
            }

            searchbox.Click();

            searchbox.SendKeys(searchTerm);
            searchbox.SendKeys(OpenQA.Selenium.Keys.Return);


        }
    }
}
