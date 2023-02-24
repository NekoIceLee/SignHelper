﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace SignHelper
{
    public class HelperService
    {
        HttpListener _listener;
        public HelperService(params string[] prefixes)
        {
            _listener = new HttpListener();
            _listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            _listener.Prefixes.Add("http://+:80/");
        }
        public void Init()
        {
            _listener.Start();
            GetNewContext();
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
            var streamreader = new StreamReader(req.InputStream, Encoding.UTF8);
            var streamwriter = new StreamWriter(resp.OutputStream, Encoding.UTF8);
            var cont = streamreader.ReadLine();
            cont ??= "";
            Console.WriteLine(cont);
            Dictionary<string, string> valuePairs = new Dictionary<string, string>(from ct in cont.Split('&') 
                                                                                   where string.IsNullOrEmpty(ct) == false
                                                                                   let k = ct.Split('=').First()
                                                                                   let v = ct.Split('=').Last()
                                                                                   select new KeyValuePair<string, string>(k,v));
            if (valuePairs.ContainsKey("id"))
            {
                var id = valuePairs["id"];
            }
            
            streamwriter.WriteLine($"{DateTime.Now:G}");
            streamwriter.Flush();
        }
        public void OnGet(HttpListenerContext context)
        {
            var resp = context.Response;
            var sw = new StreamWriter(resp.OutputStream);
            sw.WriteLine("Hello!");
            sw.Flush();
        }
    }
}
