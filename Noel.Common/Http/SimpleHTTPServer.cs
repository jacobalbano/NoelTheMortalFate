// MIT License - Copyright (c) 2016 Can Güney Aksakalli
// https://aksakalli.github.io/2014/02/24/simple-http-server-with-csparp.html

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System.Diagnostics;
using Noel.Common;
using Newtonsoft.Json;

namespace Noel.Common.Http
{
    public class SimpleHTTPServer<TApi> where TApi : new()
    {
        public string Route { get; }

        public string EmbedRootDir { get; }

        /// <summary>
        /// Construct server with given port.
        /// </summary>
        /// <param name="path">Directory path to serve.</param>
        /// <param name="port">Port of the server.</param>
        public SimpleHTTPServer(string route, string embedRootDir)
        {
            Route = route;
            EmbedRootDir = embedRootDir;
            serverThread = new Thread(Listen);
            serverThread.Start();
            dispatcher = new SimpleHTTPApiDispatcher(typeof(TApi));
        }

        /// <summary>
        /// Stop server and dispose all functions.
        /// </summary>
        public void Stop()
        {
            serverThread.Abort();
            listener.Stop();
        }

        private void Listen()
        {
            listener = new HttpListener();
            listener.Prefixes.Add(Route);
            listener.Start();
            while (true)
            {
                try
                {
                    HttpListenerContext context = listener.GetContext();
                    Process(context);
                }
                catch (Exception)
                {
                }
            }
        }

        private void Process(HttpListenerContext context)
        {
            string requestUrl = context.Request.Url.PathAndQuery;
            requestUrl = requestUrl.Substring(1);

            if (string.IsNullOrEmpty(requestUrl))
                requestUrl = "index.html";
            
            if (requestUrl.StartsWith("api/"))
                HandleApiCall(context, requestUrl);
            else
                HandleFileRequest(context, requestUrl);
        }

        private void HandleApiCall(HttpListenerContext context, string requestUrl)
        {
            using (var response = context.Response)
            {
                try
                {
                    var route = requestUrl.Substring("api/".Length);
                    var (name, args ) = route.Split('?');

                    switch (context.Request.HttpMethod)
                    {
                        case "GET":
                            response.ContentType = "application/json";
                            var responseBody = dispatcher.CallMethod(name, "GET", args);
                            var buffer = Encoding.UTF8.GetBytes(responseBody);
                            response.ContentLength64 = buffer.Length;
                            response.OutputStream.Write(buffer, 0, buffer.Length);
                            break;

                        case "POST":
                            using (var body = context.Request.InputStream)
                            using (var reader = new StreamReader(body, context.Request.ContentEncoding))
                            {
                                dispatcher.CallMethod(name, "POST", reader.ReadToEnd());
                                response.StatusCode = 204;
                            }
                            break;
                        default:
                            response.StatusCode = 404;
                            break;
                    }
                }
                catch (Exception e)
                {
                    response.StatusCode = 500;
                    response.ContentType = "application/json";
                    var buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(e));
                    response.ContentLength64 = buffer.Length;
                    response.OutputStream.Write(buffer, 0, buffer.Length);
                }
            }
        }

        private void HandleFileRequest(HttpListenerContext context, string requestUrl)
        {
            var filename = EmbedRootDir + requestUrl;
            if (EmbeddedFile.Exists(filename))
            {
                try
                {
                    using (var input = EmbeddedFile.GetStream(filename))
                    {
                        context.Response.ContentType = _mimeTypeMappings.TryGetValue(Path.GetExtension(filename), out var mime) ? mime : "application/octet-stream";
                        context.Response.ContentLength64 = input.Length;
                        context.Response.AddHeader("Date", DateTime.Now.ToString("r"));
                        context.Response.AddHeader("Last-Modified", File.GetLastWriteTime(filename).ToString("r"));

                        int nbytes;
                        byte[] buffer = new byte[1024 * 16];
                        while ((nbytes = input.Read(buffer, 0, buffer.Length)) > 0)
                            context.Response.OutputStream.Write(buffer, 0, nbytes);
                    }

                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.OutputStream.Flush();
                }
                catch (Exception)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }

            context.Response.OutputStream.Close();
        }


        private static readonly IDictionary<string, string> _mimeTypeMappings = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase) {
        #region extension to MIME type list
        {".asf", "video/x-ms-asf"},
        {".asx", "video/x-ms-asf"},
        {".avi", "video/x-msvideo"},
        {".bin", "application/octet-stream"},
        {".cco", "application/x-cocoa"},
        {".crt", "application/x-x509-ca-cert"},
        {".css", "text/css"},
        {".deb", "application/octet-stream"},
        {".der", "application/x-x509-ca-cert"},
        {".dll", "application/octet-stream"},
        {".dmg", "application/octet-stream"},
        {".ear", "application/java-archive"},
        {".eot", "application/octet-stream"},
        {".exe", "application/octet-stream"},
        {".flv", "video/x-flv"},
        {".gif", "image/gif"},
        {".hqx", "application/mac-binhex40"},
        {".htc", "text/x-component"},
        {".htm", "text/html"},
        {".html", "text/html"},
        {".ico", "image/x-icon"},
        {".img", "application/octet-stream"},
        {".iso", "application/octet-stream"},
        {".jar", "application/java-archive"},
        {".jardiff", "application/x-java-archive-diff"},
        {".jng", "image/x-jng"},
        {".jnlp", "application/x-java-jnlp-file"},
        {".jpeg", "image/jpeg"},
        {".jpg", "image/jpeg"},
        {".js", "application/x-javascript"},
        {".mml", "text/mathml"},
        {".mng", "video/x-mng"},
        {".mov", "video/quicktime"},
        {".mp3", "audio/mpeg"},
        {".mpeg", "video/mpeg"},
        {".mpg", "video/mpeg"},
        {".msi", "application/octet-stream"},
        {".msm", "application/octet-stream"},
        {".msp", "application/octet-stream"},
        {".pdb", "application/x-pilot"},
        {".pdf", "application/pdf"},
        {".pem", "application/x-x509-ca-cert"},
        {".pl", "application/x-perl"},
        {".pm", "application/x-perl"},
        {".png", "image/png"},
        {".prc", "application/x-pilot"},
        {".ra", "audio/x-realaudio"},
        {".rar", "application/x-rar-compressed"},
        {".rpm", "application/x-redhat-package-manager"},
        {".rss", "text/xml"},
        {".run", "application/x-makeself"},
        {".sea", "application/x-sea"},
        {".shtml", "text/html"},
        {".sit", "application/x-stuffit"},
        {".swf", "application/x-shockwave-flash"},
        {".tcl", "application/x-tcl"},
        {".tk", "application/x-tcl"},
        {".txt", "text/plain"},
        {".war", "application/java-archive"},
        {".wbmp", "image/vnd.wap.wbmp"},
        {".wmv", "video/x-ms-wmv"},
        {".xml", "text/xml"},
        {".xpi", "application/x-xpinstall"},
        {".zip", "application/zip"},
        #endregion
    };
        private readonly Thread serverThread;
        private HttpListener listener;
        private readonly SimpleHTTPApiDispatcher dispatcher;
    }
}