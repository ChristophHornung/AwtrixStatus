using System.IO;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AwtrixStatus;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
	/// <summary>
	/// Gets the current <see cref="App"/> instance in use
	/// </summary>
	public new static App Current => (App)Application.Current;

	/// <summary>
	/// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
	/// </summary>
	public IServiceProvider Services { get; } = ConfigureServices();

	private static IServiceProvider ConfigureServices()
	{
		var services = new ServiceCollection();
		services.AddTransient<MainWindowViewModel>();

		// Register appsettings.json based configuration

		IConfiguration configuration = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appSettings.json", false, reloadOnChange: true)
			.Build();

		services.AddSingleton(configuration);
		services.AddOptions<ClockSettings>().BindConfiguration(ClockSettings.SectionName);

		return services.BuildServiceProvider();
	}
}