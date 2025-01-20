using System.Net;
using System.Net.Security;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace SethosImpact.Proxy;
internal class ProxyService
{
    private static readonly string[] s_redirectDomains =
    {
        ".gryphline.com",
        ".hg-cdn.com"
    };

    private readonly ProxyServer _server;

    public ProxyService()
    {
        _server = new ProxyServer();
        _server.CertificateManager.EnsureRootCertificate();

        _server.BeforeRequest += async (sender, e) =>
        {
            Console.WriteLine("Connessione?");
            // Ignora le richieste di tipo CONNECT
            if (e.HttpClient.Request.Method.Equals("CONNECT", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            // Verifica se l'host termina con "gryphline.com"
            if (e.HttpClient.Request.RequestUri.Host.EndsWith("gryphline.com", StringComparison.OrdinalIgnoreCase))
            {
                var uriBuilder = new UriBuilder(e.HttpClient.Request.RequestUri)
                {
                    Scheme = "http",
                    Host = "127.0.0.1",
                    Port = 8099
                };

                e.HttpClient.Request.RequestUri = uriBuilder.Uri;

               
               
                    // Aggiungi un nuovo valore Cookie
                    e.HttpClient.Request.Headers.AddHeader("Cookie", $"OriginalHost={e.HttpClient.Request.RequestUri.Host}; OriginalUrl={e.HttpClient.Request.RequestUri}");
                
            }

            await Task.CompletedTask;
        };
        ExplicitProxyEndPoint endPoint = new(IPAddress.Any, 8080, true);
        

        _server.AddEndPoint(endPoint);
        //_server.Start();

        //_server.SetAsSystemHttpProxy(endPoint);
        //_server.SetAsSystemHttpsProxy(endPoint);
    }

    public void Shutdown()
    {
        // _server.Stop();
        // _server.Dispose();
    }


    private Task BeforeRequest(object sender, SessionEventArgs args)
    {
        string hostname = args.HttpClient.Request.RequestUri.Host;
        string requestUrl = args.HttpClient.Request.Url;
        if (ShouldRedirect(hostname) && args.HttpClient.Request.Method != "CONNECT")
        {
            
            Uri local = new($"http://127.0.0.1:8099/");// 8888 port

            string replacedUrl = new UriBuilder(requestUrl)
            {
                Scheme = local.Scheme,
                Host = local.Host,
                Port = local.Port
            }.Uri.ToString();

           
            args.HttpClient.Request.Url = replacedUrl;
            
        }
        Console.WriteLine(requestUrl);
        return Task.CompletedTask;
    }

    private static bool ShouldRedirect(string hostname)
    {
        foreach (string domain in s_redirectDomains)
        {
            if (hostname.Contains(domain))
                return true;
        }
        return false;
    }
}