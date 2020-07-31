using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WordCountTextFile.Models;

namespace WordCountTextFile.Controllers
{
    public class HomeController : Controller
    {
        static StreamReader URLStream(String fileurl)
        {
            return new StreamReader(new HttpClient().GetStreamAsync(fileurl).Result);
        }

        public IActionResult Index()
        {
            

            string line;
            string text = "";
            //assuming no words are split between lines and lines have complete words
            StreamReader s = URLStream(@"https://ia803105.us.archive.org/1/items/LordOfTheRingsApocalypticProphecies/Lord%20of%20the%20Rings%20Apocalyptic%20Prophecies_djvu.txt");
            String myline = s.ReadLine(); //First Line
            while ((line = s.ReadLine()) != null) //Subsequent Lines
            {
                line = line.ToLower();
                text +=line;
            }

            var wordList = text.Split(" ");
            var output = wordList.GroupBy(x => x).Select(x => new TextModel { Word = x.Key, Count = x.Count() }).OrderByDescending(x => x.Count);
            string Result = "";
            var i = 0;
            foreach (var item in output)
            {
                if (i < 10)
                {
                    Result += item.Word + " : " + item.Count + Environment.NewLine;
                    i++;
                }
                else
                {
                    break;
                }
            }


            //return View(Result);
            return Content(Result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
