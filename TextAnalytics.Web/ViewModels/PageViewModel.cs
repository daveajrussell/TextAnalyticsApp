using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace TextAnalyticsApp.Web.ViewModels
{
    public class PageViewModel
    {
        public List<SelectListItem> Clients { get; set; }

        public PageViewModel(List<SelectListItem> clients)
        {
            Clients = clients;
        }
    }
}