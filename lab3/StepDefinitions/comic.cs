using System.Net.Http;
using TechTalk.SpecFlow;
using NUnit.Framework;

namespace YourNamespace
{
    [Binding]
    public class XkcdApiSteps
    {
        // Ініціалізуємо HttpClient для надсилання HTTP-запитів
        private HttpClient httpClient = new HttpClient();

        // Зберігає відповідь сервера після виконання запиту
        private HttpResponseMessage response;

        // Змінна для зберігання кінцевої точки API
        private string endpoint;

        // Крок, який встановлює URL кінцевої точки для API XKCD
        [Given(@"the XKCD API endpoint ""(.*)""")]
        public void GivenTheXkcdApiEndpoint(string url)
        {
            // Зберігаємо URL кінцевої точки
            endpoint = url;
        }

        // Крок, який відправляє GET-запит для отримання коміксу за вказаним номером
        [When(@"I send a GET request with comic number ""(.*)""")]
        public void WhenISendAGETRequestWithComicNumber(int comicNumber)
        {
            // Формуємо URL запиту, замінюючи {comicNumber} на фактичний номер коміксу
            string requestUrl = endpoint.Replace("{comicNumber}", comicNumber.ToString());

            // Виконуємо GET-запит та зберігаємо відповідь
            response = httpClient.GetAsync(requestUrl).Result;
        }

        // Крок, який відправляє GET-запит для отримання останнього коміксу (без вказання номера)
        [When(@"I send a GET request without specifying a comic number")]
        public void WhenISendAGETRequestWithoutSpecifyingComicNumber()
        {
            // Виконуємо GET-запит на базовий URL без номера коміксу
            response = httpClient.GetAsync(endpoint).Result;
        }

        // Крок, який перевіряє, що код статусу відповіді для коміксу за номером - 200
        [Then(@"the response status code for comic by number should be 200")]
        public void ThenTheResponseStatusCodeForComicByNumberShouldBe200()
        {
            // Перевіряємо, що код статусу відповіді - 200 (ОК)
            Assert.AreEqual(200, (int)response.StatusCode);
        }

        // Крок, який перевіряє, що відповідь містить комікс із вказаним номером
        [Then(@"the response should contain XKCD comic with number ""(.*)""")]
        public void ThenTheResponseShouldContainXkcdComicWithNumber(int expectedComicNumber)
        {
            // Перевіряємо, що запит успішний
            if (response.IsSuccessStatusCode)
            {
                // Отримуємо текстовий контент відповіді
                var content = response.Content.ReadAsStringAsync().Result;

                // Перевіряємо, що контент містить номер коміксу у форматі "num": <expectedComicNumber>
                Assert.True(content.Contains($"\"num\": {expectedComicNumber}"));
            }
            else
            {
                // Якщо запит не був успішним, виконуємо невдалу перевірку
                Assert.Fail("The request was not successful.");
            }
        }

        // Крок, який перевіряє, що код статусу відповіді для останнього коміксу - 200
        [Then(@"the response status code for the latest comic should be 200")]
        public void ThenTheResponseStatusCodeForTheLatestComicShouldBe200()
        {
            // Перевіряємо, що код статусу відповіді - 200 (ОК)
            Assert.AreEqual(200, (int)response.StatusCode);
        }

        // Крок, який перевіряє, що відповідь містить дані останнього коміксу
        [Then(@"the response should contain the latest XKCD comic")]
        public void ThenTheResponseShouldContainTheLatestXkcdComic()
        {
            // Перевіряємо, що запит успішний
            if (response.IsSuccessStatusCode)
            {
                // Отримуємо текстовий контент відповіді
                var content = response.Content.ReadAsStringAsync().Result;
                // Перевіряємо, що контент містить поле "num", яке вказує номер коміксу (для останнього коміксу номер може змінюватися)
                Assert.True(content.Contains("\"num\":"));
                Assert.True(content.Contains("\"num\": 3") || content.Contains("\"num\": 4") || content.Contains("\"num\": 5"));
            }
            else
            {
                // Якщо запит не був успішним, виконуємо невдалу перевірку
                Assert.Fail("The request was not successful.");
            }
        }
    }
}