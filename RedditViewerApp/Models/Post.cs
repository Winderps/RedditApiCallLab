using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RedditViewerApp.Models
{
    public class Post
    {
        public string Title { get; set; }
        public int Ups { get; set; }
        public int Score { get; set; }
        public string Thumbnail { get; set; }
        public string Id { get; set; }
        public string Author { get; set; }
        public string Permalink { get; set; }
        public string Url { get; set; }

        public static string GetApiResponse(string subreddit, string type = "new", int count = 10, string query="", string sort="")
        {
            subreddit = subreddit.Replace("/r", "").Replace("/", "");

            string apiUrl = $"https://www.reddit.com/r/{subreddit}/{type}.json?limit={count}";

            if (!query.Equals(""))
            {
                apiUrl = apiUrl + $"&q={System.Uri.EscapeDataString(query)}&restrict_sr=true&sort={sort}";
            }
            
            HttpWebRequest request = WebRequest.CreateHttp(apiUrl);
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();

            StreamReader rd = new StreamReader(response.GetResponseStream());

            string output = rd.ReadToEnd();
            
            return output;
        }
        
        public static List<Post> GetRecentPosts(string subreddit, string type = "new", int count = 10, string query = "", string sort="")
        {
            string response = GetApiResponse(subreddit, type, count, query, sort);
            
            JObject convertedObject = JObject.Parse(response);
            List<JToken> postList = convertedObject["data"]?["children"]?.ToList();

            List<Post> posts = new List<Post>();
            if (postList == null)
            {
                return new List<Post>();
            }
            if (postList.Count == 0)
            {
                return new List<Post>();
            }

            foreach(JToken post in postList)
            {
                Post p = JsonConvert.DeserializeObject<Post>(post["data"]?.ToString() 
                                                             ?? "{ \"title\": \"Bad post data\" }");
                posts.Add(p);
            }

            return posts;
        }
    }
}