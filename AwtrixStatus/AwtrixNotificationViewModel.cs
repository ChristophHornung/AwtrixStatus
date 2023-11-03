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
}