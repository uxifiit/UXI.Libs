using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Validation;
using Microsoft.Owin.Cors;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.StaticFiles;
using Microsoft.Owin.StaticFiles.ContentTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Ninject;
using Ninject.Syntax;
using Ninject.Web.WebApi.Filter;
using Owin;
using UXI.Common;
using UXI.Common.Extensions;
using UXI.OwinServer.Common;

namespace UXI.OwinServer
{
    public class ServerHost : DisposableBase, IDisposable
    {
        private readonly IKernel _kernel;
        private IDisposable _server;
        private bool _boundTypes;

        public ServerHost(IKernel kernel)
        {
            _kernel = kernel;
        }


        public bool UseSignalR { get; set; }


        public bool UseFileServer { get; set; }


        public IContentTypeProvider CustomFileExtensionContentTypeProvider { get; set; }

        public IList<JsonConverter> Converters { get; } = new List<JsonConverter>()
        {
            new Newtonsoft.Json.Converters.StringEnumConverter(camelCaseText: false)
        };

        public IList<MediaTypeFormatter> Formatters { get; } = new List<MediaTypeFormatter>()
        {
            new BrowserJsonFormatter()
        };

        //netsh http add urlacl url=http://+:8080/ user=machine\username
        //netsh http delete urlacl url=http://+:8080/

        public void Start(string address)
        {
            _server = WebApp.Start(address, Startup);
        }

        public void Start(int port)
        {
            _server = WebApp.Start("http://+:" + port, Startup);
        }

        private void Startup(IAppBuilder appBuilder)
        {
#if DEBUG
            appBuilder.Properties["host.AppMode"] = "development";  // TODO why development ???
            appBuilder.UseErrorPage(new Microsoft.Owin.Diagnostics.ErrorPageOptions { ShowExceptionDetails = true });
#else
            appBuilder.UseErrorPage();
#endif

            ConfigureWebApi(appBuilder);

            if (UseSignalR)
            {
                ConfigureSignalR(appBuilder);
            }

            if (UseFileServer)
            {
                ConfigureFileServer(appBuilder);
            }
            else
            {
                appBuilder.UseWelcomePage("/owin/");
            }
        }

        private void ConfigureWebApi(IAppBuilder appBuilder)
        {
            HttpConfiguration config = new HttpConfiguration();

            bool bound = ObjectEx.GetAndReplace(ref _boundTypes, true);
            if (bound == false)
            {
                _kernel.Bind<DefaultModelValidatorProviders>().ToConstant(new DefaultModelValidatorProviders(config.Services.GetServices(typeof(ModelValidatorProvider)).Cast<ModelValidatorProvider>()));
                _kernel.Bind<DefaultFilterProviders>().ToConstant(new DefaultFilterProviders(new[] { new NinjectFilterProvider(_kernel) }.AsEnumerable()));
            }

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            var cors = new EnableCorsAttribute(origins: "*", headers: "*", methods: "*");
            config.EnableCors(cors);

            config.DependencyResolver = new Ninject.Web.WebApi.NinjectDependencyResolver(_kernel);
            
            Formatters.ForEach(config.Formatters.Add);

            config.Formatters.OfType<JsonMediaTypeFormatter>().ForEach(formatter =>
            {
                formatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                Converters.ForEach(formatter.SerializerSettings.Converters.Add);
            });

            appBuilder.UseWebApi(config);
        }

        [System.Diagnostics.DebuggerNonUserCode]
        private void ConfigureSignalR(IAppBuilder appBuilder)
        {
            appBuilder.Map("/signalr", builder =>
            {
                builder.UseCors(CorsOptions.AllowAll);
                var configuration = new Microsoft.AspNet.SignalR.HubConfiguration()
                {
                    Resolver = new NinjectSignalRDependencyResolver(_kernel),
                    EnableJavaScriptProxies = true
                };

                builder.RunSignalR(configuration);
            });
        }

        [System.Diagnostics.DebuggerNonUserCode]
        private void ConfigureFileServer(IAppBuilder appBuilder)
        {
            var fileSystem = new PhysicalFileSystem("www");
            var options = new FileServerOptions()
            {
                EnableDirectoryBrowsing = false,
                FileSystem = fileSystem
            };
            options.StaticFileOptions.FileSystem = fileSystem;
            options.StaticFileOptions.ServeUnknownFileTypes = false;

            if (CustomFileExtensionContentTypeProvider != null)
            {
                options.StaticFileOptions.ContentTypeProvider = CustomFileExtensionContentTypeProvider;
            }

            options.DefaultFilesOptions.DefaultFileNames = new[] 
            {
                "index.html"
            };

            appBuilder.UseFileServer(options); 
        }


        public void Stop()
        {
            var server = ObjectEx.GetAndReplace(ref _server, null);
            if (server != null)
            {
                server.Dispose();
            }

            bool bound = ObjectEx.GetAndReplace(ref _boundTypes, false);
            if (bound)
            {
                _kernel.Unbind<DefaultModelValidatorProviders>();
                _kernel.Unbind<DefaultFilterProviders>();
            }
        }


        private bool _disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (_disposed == false)
            {
                if (disposing)
                {
                    Stop();
                }
                _disposed = true;
            }
            base.Dispose(disposing);
        }
    }
}
