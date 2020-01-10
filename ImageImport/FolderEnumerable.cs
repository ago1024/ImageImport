using MediaDevices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageImport
{
	class FolderEnumerable : IEnumerable<MediaDirectoryInfo>
	{
		private readonly MediaDevice Device;
		private readonly MediaDirectoryInfo Root;
		private readonly List<string> SkipList;

		public FolderEnumerable(MediaDevice device, MediaDirectoryInfo root, List<string> skipList)
		{
			this.Device = device;
			this.Root = root;
			this.SkipList = skipList;
		}

		public event EventHandler<ProgressEvent> ProgressHandler;

		public class ProgressEvent : EventArgs
		{
			public ProgressEvent(string fileName)
			{
				FileName = fileName;
			}

			public string FileName { get; }
		}

		protected void OnProgress(string fileName)
		{
			ProgressEvent e = new ProgressEvent(fileName);
			ProgressHandler?.Invoke(this, e);
		}

		public event EventHandler<LogEvent> LogHandler;

		public class LogEvent : EventArgs
		{
			public LogEvent(string message)
			{
				Message = message;
			}

			public string Message { get; }
		}

		protected void OnLog(string message)
		{
			LogEvent e = new LogEvent(message);
			LogHandler?.Invoke(this, e);
		}

		public IEnumerator<MediaDirectoryInfo> GetEnumerator()
		{
			var stack = new Stack<IEnumerator<MediaDirectoryInfo>>();
			IEnumerator<MediaDirectoryInfo> enumerator = this.Root.EnumerateDirectories().GetEnumerator();

			try
			{
				while (true)
				{
					bool hasNext;

					try
					{
						hasNext = enumerator.MoveNext();
					}
					catch (Exception e)
					{
						OnLog(e.ToString());
						hasNext = false;
					}

					if (hasNext)
					{
						MediaDirectoryInfo element = enumerator.Current;
						OnProgress(element.FullName);
						try
						{
							if (SkipList.Contains(element.FullName) || Device.FileExists(element.FullName + @"\.nomedia") || Device.FileExists(element.FullName + @"\.noimage"))
							{
								OnLog($"Skipping {element.FullName}");
								continue;
							}
							OnLog(element.FullName);
						}
						catch (Exception e)
						{
							OnLog(element.FullName);
							OnLog(e.ToString());
							continue;
						}

						yield return element;

						stack.Push(enumerator);
						enumerator = element.EnumerateDirectories().GetEnumerator();
						continue;
					}

					if (stack.Count > 0)
					{
						enumerator.Dispose();
						enumerator = stack.Pop();
					}
					else
					{
						yield break;
					}
				}

			}
			finally
			{
				enumerator.Dispose();
				while (stack.Count > 0)
				{
					enumerator = stack.Pop();
					enumerator.Dispose();
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
