using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Options;

namespace AwtrixStatus;

public class MainWindowViewModel : INotifyPropertyChanged
{
	private TrafficState trafficState;

	public MainWindowViewModel(IOptions<ClockSettings> clockSettings)
	{
		this.ClockSettingsOptions = clockSettings;
		this.NotificationText = string.Empty;
		this.TrafficState = TrafficState.Green;
		this.TeamStatusHandler = new TeamsLogStatusUpdater();
		this.TeamStatusHandler.OnStatusChanged += this.TeamStatusHandlerOnStatusChanged;
		this.TeamStatusHandler.OnActivityChanged += this.TeamStatusHandlerOnActivityChanged;
		//this.TeamStatusHandler.StartMonitoring();
	}

	public IOptions<ClockSettings> ClockSettingsOptions { get; }

	public List<ClockSetting> Clocks => this.ClockSettingsOptions.Value.Clocks;

	public ClockSetting? SelectedClock { get; set; }

	public ICommand SendCommand => new AsyncRelayCommand(this.OnSend);

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

	public event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

		string json = JsonSerializer.Serialize(new
		{
			text = this.NotificationText,
			effect = "SwirlOut",
			stack = false,
			duration = 15
		});

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