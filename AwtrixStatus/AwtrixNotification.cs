namespace AwtrixStatus;

public class AwtrixNotification
{
	public string? Text { get; set; }
	public int? TextCase { get; set; }
	public bool? TopText { get; set; }
	public int? TextOffset { get; set; }
	public bool? Center { get; set; }
	public string? Color { get; set; }
	public List<string>? Gradient { get; set; }
	public int? BlinkText { get; set; }
	public int? FadeText { get; set; }
	public string? Background { get; set; }
	public bool? Rainbow { get; set; }
	public string? Icon { get; set; }
	public int? PushIcon { get; set; }
	public int? Repeat { get; set; }
	public int? Duration { get; set; }
	public bool? Hold { get; set; }
	public string? Sound { get; set; }
	public string? Rtttl { get; set; }
	public bool? LoopSound { get; set; }
	public List<int>? Bar { get; set; }
	public List<int>? Line { get; set; }
	public bool? Autoscale { get; set; }
	public int? Progress { get; set; }
	public string? ProgressC { get; set; }
	public string? ProgressBC { get; set; }

	public int? Pos { get; set; }

	//public List<object>? Draw { get; set; } // Replace object with the appropriate type for the drawing instructions
	public int? Lifetime { get; set; }
	public int? LifetimeMode { get; set; }
	public bool? Stack { get; set; } = false;
	public bool? Wakeup { get; set; }
	public bool? NoScroll { get; set; }
	public List<string>? Clients { get; set; }
	public int? ScrollSpeed { get; set; }

	public string? Effect { get; set; }

	// public JObject EffectSettings { get; set; } // JObject is used to hold any JSON object
	public bool? Save { get; set; }
}