using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TextAnalyticsApp.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using TextAnalyticsApp.Data.Abstractions;

namespace TextAnalyticsApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUow _uow;

        public HomeController(IUow uow)
        {
            _uow = uow;
        }

        public IActionResult Index()
        {
            var clients = _uow
                .Clients
                .GetAll()
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToList();

            var pvm = new PageViewModel(clients);

            return View(pvm);
        }

        [Route("/clients/{id:int}")]
        public IActionResult Clients(int id)
        {
            var clients = _uow.Clients.GetAll().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
                Selected = x.Id == id
            }).ToList();

            var pvm = new PageViewModel(clients);

            return View("Index", pvm);
        }

        [HttpGet]
        [Route("/api/clients/getclientdata")]
        public IActionResult GetClientData()
        {
            var tweets = _uow.Tweets.GetAll()
                .GroupBy(o => new { o.ClientId })
                .ToList();

            var model = new TweetModel
            {
                Categories = tweets
                    .SelectMany(x => x)
                    .OrderBy(x => x.Datestamp)
                    .Select(x => $"{x.Datestamp:dd MMMM yyyy}")
                    .Distinct()
                    .ToList()
            };

            foreach (var clientGroupedTweetList in tweets)
            {
                var dateGroupedClientTweetList = clientGroupedTweetList
                        .GroupBy(x => x.Datestamp)
                        .OrderBy(x => x.Key)
                        .ToList();

                if (dateGroupedClientTweetList.Any())
                {
                    model.Series.Add(new TweetData
                    {
                        Name = dateGroupedClientTweetList.FirstOrDefault()?.FirstOrDefault()?.Client.Name ?? "",
                        Data = dateGroupedClientTweetList.Select(x => new[] { x.Average(o => o.Sentiment) }).ToList()
                    });
                }
            }

            return Json(model);
        }

        [HttpGet]
        [Route("/api/clients/getclientdata/{id:int}")]
        public IActionResult GetClientData(int id)
        {
            var model = default(TweetModel);

            var tweets = _uow
                    .Tweets
                    .GetAll()
                    .Where(o => o.ClientId == id)
                    .GroupBy(o => new { o.Datestamp })
                    .OrderBy(x => x.Key.Datestamp)
                    .ToList();

            if (tweets.Any())
            {
                model = new TweetModel
                {
                    Categories = tweets
                        .SelectMany(x => x)
                        .OrderBy(x => x.Datestamp)
                        .Select(x => $"{x.Datestamp:dd MMMM yyyy}")
                        .Distinct()
                        .ToList()
                };

                model.Series.Add(new TweetData
                {
                    Name = tweets.FirstOrDefault()?.FirstOrDefault()?.Client.Name ?? "",
                    Data = tweets.Select(x => new[] { x.Average(o => o.Sentiment) }).ToList()
                });
            }

            return Json(model);
        }
    }
}