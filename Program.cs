using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using BookLibararySystem;
using JsonFlatFileDataStore;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using BookLibararySystem.Services;
using BookLibararySystem.Services.Interfaces;

namespace BookLibararySystem
{
    class Program
    {
        private static IServiceProvider _serviceProvider;
        
        public static void RegisterServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<StartUp>();
            services.AddSingleton<IDisplayService, DisplayService>();
            services.AddSingleton<ILibraryService, LibraryService>();
            _serviceProvider = services.BuildServiceProvider(true);
        }

        static void Main(string[] args)
        {
            RegisterServices();
            IServiceScope scope = _serviceProvider.CreateScope();
            scope.ServiceProvider.GetRequiredService<StartUp>().Run(); //Runs the services.
        }
    }
}
