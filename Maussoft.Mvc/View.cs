﻿using System;
using System.IO;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;

namespace Maussoft.Mvc
{
	public class View
	{
		protected WebContext Context=null;
		private StringWriter _writer = new StringWriter();

		public virtual void Header() {}
		public virtual void Content() {}
		public virtual void Footer() {}

		public string Render(WebContext context)
		{
			Context = context;
			_writer = new StringWriter ();
			Header (); Content (); Footer ();
			return _writer.ToString ();
		}

		private static string replace(string input, object[] arguments)
		{
			if (arguments.Length == 0) return input;
			return Regex.Replace(input, @"{([0-9]+)}", delegate(Match match)
				{
					int index;
					if (!int.TryParse (match.Groups [1].Value, out index)) return match.Value;
					if (index >= arguments.Length) return match.Value;
					return HttpUtility.HtmlEncode(arguments[index] as string);
				}
			);
		}

		public void Write(string format, params object[] arguments)
		{
			_writer.Write (replace (format,arguments));
		}

		public void WriteLine(string format = "", params object[] arguments)
		{
			_writer.WriteLine (replace (format,arguments));
		}

	}
}

