using CommunityToolkit.Mvvm.ComponentModel;

namespace AwtrixStatus;

public class AwtrixNotificationViewModel : ObservableObject
{
	private AwtrixNotification notification = new();

	public AwtrixNotificationViewModel()
	{
	}

	public AwtrixNotification Notification
	{
		get => this.notification;
		set
		{
			if (Equals(value, this.notification))
			{
				return;
			}

			this.notification = value;
			this.OnPropertyChanged();
		}
	}

	public string? Color
	{
		get => this.Notification.Color;
		set
		{
			if (value == this.Notification.Color)
			{
				return;
			}

			this.Notification.Color = value;
			this.OnPropertyChanged();
		}
	}

	public string? BackgroundColor
	{
		get => this.Notification.Background;
		set
		{
			if (value == this.Notification.Background)
			{
				return;
			}

			this.Notification.Background = value;
			this.OnPropertyChanged();
		}
	}
}