using System.IO;
using System.Text.RegularExpressions;

namespace AwtrixStatus;

public class TeamsLogStatusUpdater
{
	private CancellationTokenSource cancellationTokenSource;
	public event EventHandler<string>? OnStatusChanged;
	public event EventHandler<bool>? OnActivityChanged;

	private static bool DetermineCallActivityFromLog(string[] logLines)
	{
		string logContent = string.Join("\n", logLines);

		if (Regex.IsMatch(logContent,
			    "Resuming daemon App updates|SfB:TeamsNoCall|name: desktop_call_state_change_send, isOngoing: false"))
		{
			return false;
		}
		else if (Regex.IsMatch(logContent,
			         "Pausing daemon App updates|SfB:TeamsActiveCall|name: desktop_call_state_change_send, isOngoing: true"))
		{
			return true;
		}

		return false;
	}

	private static string? DetermineStatusFromLog(string[] logLines)
	{
		string logContent = string.Join("\n", logLines);

		string[] states = new[]
		{
			"Available", "Busy", "OnThePhone", "Away", "BeRightBack", "DoNotDisturb", "Focusing", "Presenting",
			"InAMeeting", "Offline"
		};

		foreach (string state in states)
		{
			if (Regex.IsMatch(logContent, $"Setting the taskbar overlay icon - {state}") ||
			    Regex.IsMatch(logContent, $"StatusIndicatorStateService: Added {state}") ||
			    Regex.IsMatch(logContent,
				    $"StatusIndicatorStateService: Added NewActivity \\(current state: {state} -> NewActivity"))
			{
				return state;
			}
		}

		return null; // Default value if no patterns match
	}

	public void StartMonitoring()
	{
		this.cancellationTokenSource = new CancellationTokenSource();

		Task.Run(() =>
		{
			string? currentStatus = null;
			bool? currentActivity = null;

			do
			{
				if (this.cancellationTokenSource.Token.IsCancellationRequested)
					break;

				string teamsLogPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
					"Microsoft\\Teams\\logs.txt");
				string[] logLines = File.ReadAllLines(teamsLogPath);

				string? status = DetermineStatusFromLog(logLines);
				bool activity = DetermineCallActivityFromLog(logLines);

				if (status != null && status != currentStatus)
				{
					currentStatus = status;
					OnOnStatusChanged(status);
				}

				if (activity != currentActivity)
				{
					currentActivity = activity;
					OnOnActivityChanged(activity);
				}

				Thread.Sleep(1000);
			} while (true);

		}, this.cancellationTokenSource.Token);
	}

	public void StopMonitoring()
	{
		this.cancellationTokenSource?.Cancel();
	}

	protected virtual void OnOnStatusChanged(string e)
	{
		this.OnStatusChanged?.Invoke(this, e);
	}

	protected virtual void OnOnActivityChanged(bool e)
	{
		this.OnActivityChanged?.Invoke(this, e);
	}
}