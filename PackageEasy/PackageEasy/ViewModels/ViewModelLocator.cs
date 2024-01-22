using CommunityToolkit.Mvvm.DependencyInjection;
using log4net;
using Microsoft.Extensions.DependencyInjection;
using PackageEasy.Common.Logs;
using PackageEasy.Domain.Interfaces;
using PackageEasy.Services;
using PackageEasy.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.ViewModels
{
    /// <summary>
    /// author:TT
    /// time:2023-03-11 15:28:05
    /// desc:ViewModelLocator
    /// </summary>
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            Ioc.Default.ConfigureServices(
            new ServiceCollection()
            .AddSingleton<IDataService, DataService>()
            .AddSingleton<MainViewModel>()
            .AddSingleton<HomeViewModel>()
            .AddSingleton<HomeView>()
            .BuildServiceProvider()
             ) ;
        }
        public MainViewModel Main
        {
            get
            {
                return Ioc.Default.GetService<MainViewModel>() ?? new MainViewModel(null);
            }
        }
        public HomeViewModel Home
        {
            get
            {
                return Ioc.Default.GetService<HomeViewModel>() ?? new HomeViewModel(null);
            }
        }
    }
}
