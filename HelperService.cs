using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace SignHelper
{
    public class HelperService
    {
        HttpListener _listener;
        public HelperService()
        {
            _listener = new HttpListener();
            _listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            _listener.Prefixes.Add("http://+:80/");
        }
        public void Init()
        {
            _listener.Start();
            Task.Run(GetNewContext);
        }
        public void GetNewContext()
        {
            while(true)
            {
                var context = _listener.GetContext();
                if (context == null)
                {
                    return;
                }
                var request = context.Request;
                var resp = context.Response;
                if (request.HttpMethod == "GET")
                {
                    OnGet(context);
                }
                if (request.HttpMethod == "POST")
                {
                    OnPost(context);
                }

                resp.ContentType = "text/html;charset=UTF-8";
                resp.ContentEncoding = Encoding.UTF8;
                resp.StatusCode = 200;
                resp.Close();
            }
        }
        public void OnPost(HttpListenerContext context)
        {
            var req = context.Request;
            var resp = context.Response;
            var Qstrings = context.Request.QueryString;
            var streamreader = new StreamReader(req.InputStream, Encoding.UTF8);
            var streamwriter = new StreamWriter(resp.OutputStream, Encoding.UTF8);
            
            var id = Qstrings["id"];
            var time = Qstrings["time"];
            if (id is not null && time is not null)
            {
                MySQLAPI.AddSignData(id, DateTime.Parse(time));
                streamwriter.WriteLine(time);
                streamwriter.Flush();
            }
        }
        public void OnGet(HttpListenerContext context)
        {
            var resp = context.Response;
            var sw = new StreamWriter(resp.OutputStream);
            var Qstrings = context.Request.QueryString;
            try
            {
                Console.WriteLine(context.Request.QueryString["id"]);
                var id = Qstrings["id"];
                if (id is null)
                {
                    return;
                }
                var signdata = SignLogic.GetTodaySign(MySQLAPI.GetTodaySignData(id));
                sw.WriteLine(signdata);
                sw.Flush();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            
        }
    }
}
