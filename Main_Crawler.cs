using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras;

namespace PathFinder
{
    class Program
    {
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        static void checkString(string xp, IWebDriver driver)
        {
            string html = driver.PageSource;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            //string securityQuestion = doc.DocumentNode.SelectSingleNode(xp).InnerText;

            var htmlNodes = doc.DocumentNode.SelectNodes(xp);

            foreach (var node in htmlNodes)
            {
                Console.WriteLine(node.InnerText);
            }

            //Console.WriteLine(securityQuestion);
        }

        static bool checkExistence(string xp, IWebDriver driver)
        {
            return driver.FindElement(By.XPath(xp)).Displayed;
        }

        static void Main(string[] args)
        {
            if (CheckForInternetConnection() != true)
            {
                Console.WriteLine("NO INTERNET!");
                Console.ReadLine();
                return;
            }

            //Navigate using Selenium
            IWebDriver driver = new ChromeDriver(@"E:\Zoom\Login\Login\bin\Debug\netcoreapp2.1\");
            driver.Navigate().GoToUrl(@"https://www.canada.ca/en/immigration-refugees-citizenship/services/application/account.html");
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 10));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("wb-info")));
            //Continue to GCKey
            driver.FindElement(By.XPath("/html/body/div[2]/div/main/div[1]/div[12]/div/div/div[2]/p/a")).Click();

            //Fill in username and password
            //WaitHelper function from DotNetSeleniumExtras.WaitHelpers - check for the existence of element on new page
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("token1")));
            driver.FindElement(By.Id("token1")).SendKeys("kingwinvin18");
            driver.FindElement(By.Id("token2")).SendKeys("Minorproject@123");
            //Click Signin
            driver.FindElement(By.XPath("//*[@id='login']/div[3]/div/button")).Click();
            //Click Continue
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("continue")));
            driver.FindElement(By.Id("continue")).Click();
            //Click I Accept
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("_continue")));
            driver.FindElement(By.Id("_continue")).Click();
            //Give security answer - Operating using HAP for determining the question
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='askSecurityQuestionForm']/div[1]/label/strong[1]")));
            string html = driver.PageSource;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            string securityQuestion = doc.DocumentNode.SelectSingleNode("//*[@id='askSecurityQuestionForm']/div[1]/label").InnerHtml.ToString();
            checkString("//*[@id='askSecurityQuestionForm']/div[1]/label", driver);
            if (securityQuestion.Contains("favourite food"))
                driver.FindElement(By.Id("answer")).SendKeys("Chinese");
            else if (securityQuestion.Contains("best friend"))
                driver.FindElement(By.Id("answer")).SendKeys("Pooja Gadia");
            else if (securityQuestion.Contains("mobile name"))
                driver.FindElement(By.Id("answer")).SendKeys("Pixel");
            else if (securityQuestion.Contains("laptop name"))
                driver.FindElement(By.Id("answer")).SendKeys("Hewlett Packard");
            driver.FindElement(By.Id("_continue")).Click();



            Console.WriteLine("\nPrinting out the results of the first page for each combination:\n\n");
            //PART BELOW NEEDS TO BE CONVERTED TO A LOOP
            int i, j, k, l, m, n;
            string ans1 = "//*[@id='answerlist[1]']/option[";
            string ans2 = "//*[@id='answerlist[2]']/option[";
            string ans3 = "//*[@id='answerlist[3]']/option[";
            string ans4 = "//*[@id='answerlist[4]']/option[";
            string ans5 = "//*[@id='answerlist[5]']/option[";
            string ans6 = "//*[@id='answerlist[9]']/option[";
            
            for (i = 2; i <= 7; i++)
            {
                for (j = 2; j <= 4; j++)
                {
                    for (k = 2; k < 213; k++)
                    {
                        for (l = 2; l < 213; k++)
                        {
                            for (m = 2; m <= 3; m++)
                            {
                                for (n = 2; n < 106; n++)
                                {
                                    //Part to Create a new Application
                                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("applyCommand")));
                                    driver.FindElement(By.Id("applyCommand")).Click();
                                    //Select Visitor Visa
                                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("applyCommand")));
                                    driver.FindElement(By.Id("applyCommand")).Click();
                                    //Select elements from the dropdown list 
                                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("answerlist[1]")));
                                    checkString("//*[@id='answerlist[1]']", driver);
                                    driver.FindElement(By.XPath(ans1 + i.ToString() + "]")).Click();
                                    driver.FindElement(By.XPath(ans2 + j.ToString() + "]")).Click();
                                    driver.FindElement(By.XPath(ans3 + k.ToString() + "]")).Click();
                                    driver.FindElement(By.XPath(ans4 + l.ToString() + "]")).Click();
                                    driver.FindElement(By.XPath(ans5 + m.ToString() + "]")).Click();
                                    driver.FindElement(By.XPath(ans6 + n.ToString() + "]")).Click();
                                    driver.FindElement(By.XPath("//*[@id='answerlist[8]']/option[2]")).Click();
                                    driver.FindElement(By.XPath("//*[@id='answerlist[9]']/option[2]")).Click();
                                    //Click Next
                                    driver.FindElement(By.Id("_next")).Click();

                                    //GRAB DATA HERE AND SAVE IN FORMAT TO A CSV
                                    checkString("//*[@id='eapp-question-and-answer-alpha']/label", driver);

                                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("_exit")));
                                    driver.FindElement(By.Id("_exit")).Click();
                                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("_continue")));
                                    driver.FindElement(By.Id("_continue")).Click();
                                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("_delete_2")));
                                    driver.FindElement(By.Id("_delete_2")).Click();
                                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("_continue")));
                                    driver.FindElement(By.Id("_continue")).Click();
                                }
                            }
                        }
                    }
                }
            }

            
            //SIMPLE 1 ITERATION RUN BELOW
            /*
            //Part to Create a new Application
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("applyCommand")));
            driver.FindElement(By.Id("applyCommand")).Click();
            //Select Visitor Visa
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("applyCommand")));
            driver.FindElement(By.Id("applyCommand")).Click();
            //Select elements from the dropdown list 
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("answerlist[1]")));
            checkString("//*[@id='answerlist[1]']", driver);
            driver.FindElement(By.XPath("//*[@id='answerlist[1]']/option[2]")).Click();
            driver.FindElement(By.XPath("//*[@id='answerlist[2]']/option[2]")).Click();
            driver.FindElement(By.XPath("//*[@id='answerlist[3]']/option[2]")).Click();
            driver.FindElement(By.XPath("//*[@id='answerlist[4]']/option[2]")).Click();
            driver.FindElement(By.XPath("//*[@id='answerlist[5]']/option[2]")).Click();
            driver.FindElement(By.XPath("//*[@id='answerlist[7]']/option[24]")).Click();
            driver.FindElement(By.XPath("//*[@id='answerlist[8]']/option[9]")).Click();
            driver.FindElement(By.XPath("//*[@id='answerlist[9]']/option[7]")).Click();
            //Click Next
            driver.FindElement(By.Id("_next")).Click();

            //GRAB DATA HERE AND SAVE IN FORMAT TO A CSV
            checkString("//*[@id='eapp-question-and-answer-alpha']/label", driver);
            */



            //PART TO CHECK FOR EXISTENCE OF NEXT BUTTONS AND END POINTS TO BE ADDED




            //Exit and Delete the form
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("_exit")));
            driver.FindElement(By.Id("_exit")).Click();
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("_continue")));
            driver.FindElement(By.Id("_continue")).Click();
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("_delete_2")));
            driver.FindElement(By.Id("_delete_2")).Click();
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("_continue")));
            driver.FindElement(By.Id("_continue")).Click();

        }
    }
}

/*
NOTES:
Run Chrome in background - Increase efficieny? Speed?
Add Connection availability check
Add Popup handler
Handle page loading failed/ambiguities
Handle corner cases: If application already exists
Add timer: Give estimates for current processing steps and ETA - Progress Bar maybe?
*/