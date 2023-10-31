using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace AwtrixStatus;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	public MainWindow()
	{
		this.InitializeComponent();
		this.DataContext = App.Current.Services.GetRequiredService<MainWindowViewModel>();
	}
}