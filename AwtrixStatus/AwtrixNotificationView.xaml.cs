using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AwtrixStatus;

/// <summary>
/// Interaction logic for AwtrixNotificationView.xaml
/// </summary>
public partial class AwtrixNotificationView : UserControl
{
	public static readonly DependencyProperty EffectsProperty = DependencyProperty.Register(nameof(Effects),
		typeof(ObservableCollection<string>), typeof(AwtrixNotificationView), new PropertyMetadata(default(ObservableCollection<string>)));

	public static readonly DependencyProperty SendCommandProperty = DependencyProperty.Register(nameof(SendCommand),
		typeof(ICommand), typeof(AwtrixNotificationView), new PropertyMetadata(default(ICommand)));

	public AwtrixNotificationView()
	{
		this.InitializeComponent();
	}

	public ObservableCollection<string> Effects
	{
		get => (ObservableCollection<string>)this.GetValue(EffectsProperty);
		set => this.SetValue(EffectsProperty, value);
	}

	public ICommand SendCommand
	{
		get => (ICommand)this.GetValue(SendCommandProperty);
		set => this.SetValue(SendCommandProperty, value);
	}
}