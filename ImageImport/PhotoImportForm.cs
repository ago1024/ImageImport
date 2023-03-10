using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MediaDevices;

namespace ImageImport
{
	public partial class PhotoImportForm : Form
	{
		private int GetIconIndex(DeviceType deviceType)
		{
			switch (deviceType)
			{
				default:
				case DeviceType.Generic:
					return 701;
				case DeviceType.Camera:
					return 702;
				case DeviceType.Phone:
					return 703;
				case DeviceType.MediaPlayer:
					return 704;
				case DeviceType.Video:
					return 705;
				case DeviceType.PersonalInformationManager:
					return 706;
				case DeviceType.AudioRecorder:
					return 707;
			}
		}

		public PhotoImportForm()
		{
			InitializeComponent();

			var path = Environment.ExpandEnvironmentVariables(@"%systemroot%\system32\wpdshext.dll");

			DeviceList.LargeImageList = new ImageList
			{
				ImageSize = new Size(32, 32),
				ColorDepth = ColorDepth.Depth32Bit
			};
			DeviceList.SmallImageList = new ImageList
			{
				ImageSize = new Size(16, 16),
				ColorDepth = ColorDepth.Depth32Bit
			};

			foreach (DeviceType deviceType in Enum.GetValues(typeof(DeviceType)))
			{
				var icon = GetIconIndex(deviceType);
				var large = IconLoader.GetIcon(path, icon, false);
				var small = IconLoader.GetIcon(path, icon, true);

				DeviceList.LargeImageList.Images.Add(large);
				DeviceList.SmallImageList.Images.Add(small);
			}

			Icon = IconLoader.GetIcon(path, GetIconIndex(DeviceType.Phone), false);

			Import.Enabled = DeviceList.SelectedItems.Count > 0;
		}

		private void DeviceList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
			Import.Enabled = DeviceList.SelectedItems.Count > 0;
		}

		private void ScanDevices_Click(object sender, EventArgs e)
		{
			var devices = MediaDevices.MediaDevice.GetDevices();

			DeviceList.Items.Clear();
			foreach (MediaDevice device in devices)
			{
				if (!device.IsConnected)
					device.Connect();
				var item = new ListViewItem(device.FriendlyName)
				{
					ImageIndex = (int)device.DeviceType,
					Tag = device
				};
				DeviceList.Items.Add(item);

				device.Disconnect();
			}
		}

		private async void Import_Click(object sender, EventArgs e)
		{
			try
			{
				DeviceList.Enabled = false;
				ScanDevices.Enabled = false;
				Import.Enabled = false;
				TargetFolder.Enabled = false;

				foreach (ListViewItem item in DeviceList.SelectedItems)
				{
					MediaDevice device = item.Tag as MediaDevice;
					if (device == null)
						return;

					if (!device.IsConnected)
						device.Connect();


					await Task.Run(() => CopyDevice(device));

					device.Disconnect();
				}

				Progress.Text = "Fertig";
			}
			finally
			{
				DeviceList.Enabled = true;
				ScanDevices.Enabled = true;
				Import.Enabled = DeviceList.SelectedItems.Count > 0;
				TargetFolder.Enabled = true;
			}
		}

		private void OnProgress(object sender, ImageEnumerable.ProgressEvent e)
		{
			Progress.Invoke((MethodInvoker) delegate {
				Progress.Text = e.FileName;
			});
		}

		private void OnLog(string message)
		{
			System.Diagnostics.Debug.WriteLine(message);
			Log.Invoke((MethodInvoker)delegate {
				Log.AppendText(message + Environment.NewLine);
			});
		}

		private void OnLog(string format, params object[] args)
		{
			OnLog(String.Format(format, args));
		}

		private void OnLog(object sender, ImageEnumerable.LogEvent e)
		{
			OnLog(e.Message);
		}

		private void CopyDevice(MediaDevice device)
		{
			MediaDirectoryInfo root = device.GetRootDirectory();
			string imageRoot = TargetFolder.Text;

			string[] searchPatterns = { "*.jpg" , "*.mp4" };

			var entries = searchPatterns.SelectMany(searchPattern => {
				ImageEnumerable images = new ImageEnumerable(device, root, imageRoot, searchPattern, new DefaultMediaFileFormatter());
				images.ProgressHandler += OnProgress;
				images.LogHandler += OnLog;
				return images;
			});

			foreach (var entry in entries)
			{
				if (!device.FileExists(entry.Source.FullName))
				{
					OnLog($"Missing file {entry.Source.FullName}");
					continue;
				}

				System.Diagnostics.Debug.WriteLine("{0}: importing {1}", entry.Source.FullName, entry.Target);

				try
				{
					Directory.CreateDirectory(Path.GetDirectoryName(entry.Target));

					using (Stream input = entry.Source.OpenRead())
					{
						using (Stream output = File.OpenWrite(entry.Target))
						{
							input.CopyTo(output);
						}
					}

					ListViewItem item = new ListViewItem(entry.Source.Name);
					item.SubItems.Add(entry.Source.FullName);
					item.SubItems.Add(entry.Target);

					ItemList.Invoke((MethodInvoker)delegate
					{
						ItemList.Items.Add(item);
					});
				}
				catch (Exception e)
				{
					File.Delete(entry.Target);
					OnLog(e.ToString());
				}
			}
		}

		public class ImageEntry {

			public ImageEntry(MediaFileInfo source, string target)
			{
				Source = source;
				Target = target;
			}

			public MediaFileInfo Source { get; }
			public string Target { get; }
		}

		private class ImageEnumerable : IEnumerable<ImageEntry>
		{
			private readonly MediaDevice Device;
			private readonly MediaDirectoryInfo Root;
			private readonly string TargetFolder;
			private readonly string SearchPattern;
			private readonly IMediaFileFormatter MediaFileFormatter;

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

			private void OnLog(string format, params object[] args)
			{
				OnLog(String.Format(format, args));
			}

			public ImageEnumerable(MediaDevice  device, MediaDirectoryInfo root, string targetFolder, string searchPattern, IMediaFileFormatter mediaFileFormatter)
			{
				Device = device;
				Root = root;
				TargetFolder = targetFolder;
				MediaFileFormatter = mediaFileFormatter;
				SearchPattern = searchPattern;
			}

			private List<string> GetSkipList()
			{
				string filename = Path.Combine(TargetFolder, ".skiplist");
				if (File.Exists(filename))
				{
					return File.ReadLines(filename).ToList();
				}
				return new List<string>();
			}

			public IEnumerator<ImageEntry> GetEnumerator()
			{
				var folders = new FolderEnumerable(Device, Root, GetSkipList());
				folders.LogHandler += (sender, e) => OnLog(e.Message);
				folders.ProgressHandler += (sender, e) => OnProgress(e.FileName);

				foreach (var folder in folders)
				{
					OnProgress(folder.FullName);

					foreach (MediaFileInfo file in folder.EnumerateFiles(SearchPattern))
					{
						OnProgress(file.FullName);

						string target = Path.Combine(TargetFolder, MediaFileFormatter.Format(file));
						string broken = Path.Combine(TargetFolder, string.Format(@"0001-01-01\{0}", file.Name));

						if (File.Exists(target) && (ulong)new FileInfo(target).Length == file.Length)
						{
							System.Diagnostics.Debug.WriteLine("{0}: {1} exists, skipping", file.FullName, target);
							continue;
						}
						else if (broken != target && !File.Exists(target) && File.Exists(broken) && (ulong)new FileInfo(broken).Length == file.Length)
						{
							OnLog("{0}: {1} exists in wrong folder: {2}, moving", file.FullName, target, broken);
							Directory.CreateDirectory(Path.GetDirectoryName(target));
							File.Move(broken, target);
							continue;
						}
						else if (File.Exists(target))
						{
							int i = 1;
							string newName = target;
							do
								newName = Path.Combine(Path.GetDirectoryName(target), Path.GetFileNameWithoutExtension(target) + "_" + i++ + Path.GetExtension(target));
							while (File.Exists(newName) && (ulong)new FileInfo(newName).Length != file.Length);

							if (File.Exists(newName))
							{
								System.Diagnostics.Debug.WriteLine("{0}: {1} exists, skipping", file.FullName, newName);
								continue;
							}

							System.Diagnostics.Debug.WriteLine("{0}: {1} exists, writing with new name {2}", file.FullName, target, newName);
							yield return new ImageEntry(file, newName);
						}
						else
						{
							yield return new ImageEntry(file, target);
						}
					}
				}
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}

	}

	public interface IMediaFileFormatter {

		string Format(MediaFileInfo file);
	}

	public class DefaultMediaFileFormatter : IMediaFileFormatter
	{

		public static readonly DateTime MinDate = DateTime.Parse("1990-01-01T00:00:00Z");

		private static DateTime? GetFileTime(MediaFileInfo file) {
			if (file.DateAuthored != null && file.DateAuthored >= MinDate) {
				return file.DateAuthored;
			}
			if (file.CreationTime != null && file.CreationTime >= MinDate) {
				return file.CreationTime;
			}
			if (file.LastWriteTime != null && file.LastWriteTime >= MinDate) {
				return file.LastWriteTime;
			}
			return file.DateAuthored;
		}

		public string Format(MediaFileInfo file)
		{
			return string.Format(@"{1:yyyy-MM-dd}\{0}", file.Name, GetFileTime(file));
		}
	}
}
