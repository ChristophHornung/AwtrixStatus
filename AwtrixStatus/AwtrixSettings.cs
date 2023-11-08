using System.Text.Json.Serialization;

namespace AwtrixStatus;

public class AwtrixSettings
{
	[JsonPropertyName("MATP")]
	public bool Matp { get; set; }

	[JsonPropertyName("ABRI")]
	public bool Abri { get; set; }

	[JsonPropertyName("BRI")]
	public int Bri { get; set; }

	[JsonPropertyName("ATRANS")]
	public bool Atrans { get; set; }

	[JsonPropertyName("TCOL")]
	public int TextColor { get; set; }

	[JsonPropertyName("TEFF")]
	public int Teff { get; set; }

	[JsonPropertyName("TSPEED")]
	public int Tspeed { get; set; }

	[JsonPropertyName("ATIME")]
	public int Atime { get; set; }

	[JsonPropertyName("TMODE")]
	public int Tmode { get; set; }

	[JsonPropertyName("CHCOL")]
	public int Chcol { get; set; }

	[JsonPropertyName("CTCOL")]
	public int Ctcol { get; set; }

	[JsonPropertyName("CBCOL")]
	public int Cbcol { get; set; }

	[JsonPropertyName("TFORMAT")]
	public string Tformat { get; set; }

	[JsonPropertyName("DFORMAT")]
	public string Dformat { get; set; }

	[JsonPropertyName("SOM")]
	public bool Som { get; set; }

	[JsonPropertyName("CEL")]
	public bool Cel { get; set; }

	[JsonPropertyName("BLOCKN")]
	public bool Blockn { get; set; }

	[JsonPropertyName("MAT")]
	public int Mat { get; set; }

	[JsonPropertyName("SOUND")]
	public bool Sound { get; set; }

	[JsonPropertyName("GAMMA")]
	public double Gamma { get; set; }

	[JsonPropertyName("UPPERCASE")]
	public bool Uppercase { get; set; }

	[JsonPropertyName("CCORRECTION")]
	public string Ccorrection { get; set; }

	[JsonPropertyName("CTEMP")]
	public string Ctemp { get; set; }

	[JsonPropertyName("WD")]
	public bool Wd { get; set; }

	[JsonPropertyName("WDCA")]
	public int Wdca { get; set; }

	[JsonPropertyName("WDCI")]
	public int Wdci { get; set; }

	[JsonPropertyName("TIME_COL")]
	public int TimeCol { get; set; }

	[JsonPropertyName("DATE_COL")]
	public int DateCol { get; set; }

	[JsonPropertyName("HUM_COL")]
	public int HumCol { get; set; }

	[JsonPropertyName("TEMP_COL")]
	public int TempCol { get; set; }

	[JsonPropertyName("BAT_COL")]
	public int BatCol { get; set; }

	[JsonPropertyName("SSPEED")]
	public int Sspeed { get; set; }

	[JsonPropertyName("TIM")]
	public bool Tim { get; set; }

	[JsonPropertyName("DAT")]
	public bool Dat { get; set; }

	[JsonPropertyName("HUM")]
	public bool Hum { get; set; }

	[JsonPropertyName("TEMP")]
	public bool Temp { get; set; }

	[JsonPropertyName("BAT")]
	public bool Bat { get; set; }
}