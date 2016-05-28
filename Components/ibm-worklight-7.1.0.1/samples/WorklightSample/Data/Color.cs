using System;

namespace WorklightSample
{
	public struct Color
	{
		public static readonly Color XamarinPurple = 0xB455B6;
		public static readonly Color XamarinBlue = 0x3B99D4;
		public static readonly Color XamarinDarkBlue = 0x15304E;
		public static readonly Color XamarinGreen = 0x7EC368;
		public static readonly Color XamarinGray = 0x768282;
		public static readonly Color XamarinDarkGray = 0x2B3E50;
		public static readonly Color XamarinLightGray = 0xE2E7E7;
		public static readonly Color White = 0xFFFFFF;
		public static readonly Color Black = 0x000000;

		public double R, G, B;

		public static Color FromHex (int hex)
		{
			Func<int, int> at = offset => (hex >> offset) & 0xFF;
			return new Color {
				R = at (16) / 255.0,
				G = at (8) / 255.0,
				B = at (0) / 255.0
			};
		}

		public static implicit operator Color (int hex)
		{
			return FromHex (hex);
		}

		public Xamarin.Forms.Color ToFormsColor ()
		{
			return Xamarin.Forms.Color.FromRgb ((int)(255 * R), (int)(255 * G), (int)(255 * B));
		}
	}
}

