using System;

namespace WorklightSample
{
	public class CommandItem
	{
		public string Command { get; set; }
		public string Image { get; set; }
		public Action ItemSelected { get;set; }
	}
}

