using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ReversiApi.Views
{
    public class _LayoutModel : PageModel
    {
        public string Session { get; set; }
        public void OnGet()
        {
            var session = HttpContext.Session.GetString("UserLoggedIn");
            if (session == null)
            {
                Session = null;
            } else {
                Session = session;
            }
        }
    }
}