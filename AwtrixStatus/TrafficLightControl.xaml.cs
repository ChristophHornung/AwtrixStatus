using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AwtrixStatus;

public partial class TrafficLightControl : UserControl
{
	public static readonly DependencyProperty StateProperty =
		DependencyProperty.Register("State", typeof(TrafficState), typeof(TrafficLightControl),
			new FrameworkPropertyMetadata(TrafficState.Red, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
				OnStateChanged));

	public TrafficLightControl()
	{
		this.InitializeComponent();
	}

	public TrafficState State
	{
		get { return (TrafficState)this.GetValue(StateProperty); }
		set { this.SetValue(StateProperty, value); }
	}

	private static void OnStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		var control = (TrafficLightControl)d;
		control.UpdateTrafficLight((TrafficState)e.NewValue);
	}

	private void UpdateTrafficLight(TrafficState state)
	{
		this.RedLight.Fill = Brushes.Gray;
		this.YellowLight.Fill = Brushes.Gray;
		this.GreenLight.Fill = Brushes.Gray;

		switch (state)
		{
			case TrafficState.Red:
				this.RedLight.Fill = Brushes.Red;
				break;
			case TrafficState.Yellow:
				this.YellowLight.Fill = Brushes.Yellow;
				break;
			case TrafficState.Green:
				this.GreenLight.Fill = Brushes.Green;
				break;
		}
	}

	private void RedLight_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		this.State = TrafficState.Red;
	}

	private void YellowLight_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		this.State = TrafficState.Yellow;
	}

	private void GreenLight_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		this.State = TrafficState.Green;
	}
}