﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Shiny.IO;


namespace Shiny
{
    public class TizenShinyHost : ShinyHost
    {
        public static void Init(IShinyStartup startup = null, Action<IServiceCollection> platformBuild = null)
            => InitPlatform(startup, services =>
            {
                //services.AddSingleton<IEnvironment, EnvironmentImpl>();
                //services.AddSingleton<IConnectivity, ConnectivityImpl>();
                //services.AddSingleton<IPowerManager, PowerManagerImpl>();
                //services.AddSingleton<IJobManager, JobManager>();
                //services.AddSingleton<IRepository, FileSystemRepositoryImpl>();
                services.AddSingleton<IFileSystem, FileSystemImpl>();
                //services.AddSingleton<ISettings, SettingsImpl>();
                platformBuild?.Invoke(services);
            });
    }
}
