using System.Windows;
using ConcurrentProgramming.Data;
using ConcurrentProgramming.Logic;
using ConcurrentProgramming.Presentation.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace ConcurrentProgramming.Presentation.View
{
    public partial class App : Application
    {
        private readonly ServiceProvider _servisProvider;

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _servisProvider = services.BuildServiceProvider();
        }
        
        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<MainViewModel>();
            services.AddSingleton(s => new MainWindow()
            {
                DataContext = s.GetRequiredService<MainViewModel>()
            });
            services.AddSingleton<IBallRepository, BallRepository>();
            services.AddSingleton<IBallManager, BallManager>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = _servisProvider.GetRequiredService<MainWindow>();
            MainWindow.Show();
            base.OnStartup(e);
        }
    }
}