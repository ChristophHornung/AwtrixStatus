using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AwtrixStatus;

public class PixelViewModel : ObservableObject
{
	private Color color = Colors.Black;

	public Color Color
	{
		get => this.color;
		set
		{
			if (value.Equals(this.color))
			{
				return;
			}

			this.color = value;
			this.OnPropertyChanged();
		}
	}
}