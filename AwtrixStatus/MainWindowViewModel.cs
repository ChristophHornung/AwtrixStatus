using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Input;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Options;

namespace AwtrixStatus;

public class MainWindowViewModel : ObservableObject
{
	private TrafficState trafficState;
	private ClockSetting? selectedClock;

	public MainWindowViewModel(IOptions<ClockSettings> clockSettings)
	{
		this.ClockSettingsOptions = clockSettings;
		this.NotificationText = string.Empty;
		this.TrafficState = TrafficState.Green;
		this.TeamStatusHandler = new TeamsLogStatusUpdater();
		this.TeamStatusHandler.OnStatusChanged += this.TeamStatusHandlerOnStatusChanged;
		this.TeamStatusHandler.OnActivityChanged += this.TeamStatusHandlerOnActivityChanged;
		this.PropertyChanged += this.HandleSelfPropertyChangedInternal;
	}

	public ObservableCollection<string> Effects { get; } = new();

	public IOptions<ClockSettings> ClockSettingsOptions { get; }

	public List<PixelViewModel> Pixels { get; } =
		new(Enumerable.Range(0, 8 * 32).Select(_ => new PixelViewModel()).ToList());

	public AwtrixNotificationViewModel NotificationViewModel { get; } = new();

	public List<ClockSetting> Clocks => this.ClockSettingsOptions.Value.Clocks;

	public ClockSetting? SelectedClock
	{
		get => this.selectedClock;
		set
		{
			if (Equals(value, this.selectedClock))
			{
				return;
			}

			this.selectedClock = value;
			this.OnPropertyChanged();
		}
	}

	public ICommand SendCommand => new AsyncRelayCommand(this.OnSend);
	public ICommand SendPixelsCommand => new AsyncRelayCommand(this.OnSendPixels);
	public ICommand SetColorCommand => new RelayCommand<PixelViewModel>(this.SetColor);

	public string NotificationText { get; set; }

	public TeamsLogStatusUpdater TeamStatusHandler { get; set; }

	public TrafficState TrafficState
	{
		get => this.trafficState;
		set
		{
			this.trafficState = value;
			this.OnPropertyChanged();
			this.SendStateToAwtrix();
		}
	}

	public string? SelectedEffect { get; set; }

	public Color PaintColor { get; set; }

	private void SetColor(PixelViewModel? pvm)
	{
		pvm!.Color = this.PaintColor;
	}

	private async void HandleSelfPropertyChangedInternal(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(this.SelectedClock))
		{
			await this.LoadClockSettings();
		}
	}

	private async Task LoadClockSettings()
	{
		if (this.SelectedClock == null)
		{
			return;
		}

		string url = this.SelectedClock.Url;
		if (!url.EndsWith("/"))
		{
			url += "/";
		}

		string effectsUrl = url + "api/effects";
		using HttpClient client = new();
		client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

		string[]? response = await client.GetFromJsonAsync<string[]>(effectsUrl);
		if (response != null)
		{
			this.Effects.Clear();
			foreach (string effect in response)
			{
				this.Effects.Add(effect);
			}
		}
		else
		{
			this.Effects.Clear();
		}

		string settingsUrl = url + "api/settings";
		AwtrixSettings? settings = await client.GetFromJsonAsync<AwtrixSettings>(settingsUrl);
		Debug.Assert(settings != null, nameof(settings) + " != null");
		// Convert the settings.TColor integer to a hex string
		this.NotificationViewModel.Color = "#" + settings.TextColor.ToString("X6");
	}

	private async Task OnSend()
	{
		if (this.SelectedClock == null)
		{
			return;
		}

		string url = this.SelectedClock.Url;
		if (!url.EndsWith("/"))
		{
			url += "/";
		}

		url += "api/notify";
		using HttpClient client = new();
		client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		var serializeOptions = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			WriteIndented = true,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
		};

		string json = JsonSerializer.Serialize(this.NotificationViewModel.Notification, serializeOptions);

		Debug.WriteLine(json);

		HttpResponseMessage response = await client.PostAsync(url, new StringContent(json));
	}

	private async Task OnSendPixels()
	{
		if (this.SelectedClock == null)
		{
			return;
		}

		string url = this.SelectedClock.Url;
		if (!url.EndsWith("/"))
		{
			url += "/";
		}

		url += "api/notify";
		using HttpClient client = new();
		client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		var serializeOptions = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			WriteIndented = true,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
		};

		string json = JsonSerializer.Serialize(new
		{
			draw = new[]
			{
				new
				{
					db = new Object[]
					{
						0, 0, 32, 8, this.Pixels.SelectMany(p => new[] { p.Color.R<<16| p.Color.G<<8| p.Color.B }).ToArray()
					}
				}
			}
		}, serializeOptions);

		Debug.WriteLine(json);

		HttpResponseMessage response = await client.PostAsync(url, new StringContent(json));
	}

	private void TeamStatusHandlerOnActivityChanged(object? sender, bool e)
	{
		this.TrafficState = e ? TrafficState.Red : TrafficState.Green;
	}

	private void TeamStatusHandlerOnStatusChanged(object? sender, string e)
	{
	}

	private async void SendStateToAwtrix()
	{
		if (this.SelectedClock == null)
		{
			return;
		}

		string url = this.SelectedClock.Url;
		if (!url.EndsWith("/"))
		{
			url += "/";
		}

		url += "api/indicator1";

		// Send json to url with {"color": "#hexcolor"]
		using HttpClient client = new();
		client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

		string json = JsonSerializer.Serialize(new
		{
			color = this.TrafficState switch
			{
				TrafficState.Green => "0",
				TrafficState.Yellow => "#FFFF00",
				TrafficState.Red => "#FF0000",
				_ => throw new ArgumentOutOfRangeException()
			}
		});

		StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
		HttpResponseMessage response = await client.PostAsync(url, content);
	}
}