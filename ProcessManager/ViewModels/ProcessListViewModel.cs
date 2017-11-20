using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using ProcessManager.DTO;

namespace ProcessManager.ViewModels
{
	public class ProcessListViewModel : Screen
	{
		private readonly IEventAggregator _events;

		#region Binded Properties

		private ProcessDTO _selectedProcess;
		private ObservableCollection<ProcessDTO> _activeProcesses;

		public ObservableCollection<ProcessDTO> ActiveProcesses
		{
			get => _activeProcesses;
			set
			{
				_activeProcesses = value;
				NotifyOfPropertyChange(() => ActiveProcesses);
			}
		}

		public ProcessDTO SelectedProcess
		{
			get => _selectedProcess;
			set
			{
				_selectedProcess = value;
				NotifyOfPropertyChange(() => SelectedProcess);
				_events.PublishOnUIThread(SelectedProcess);
			}
		}

		#endregion

		public ProcessListViewModel(IEventAggregator events)
		{
			_events = events;

			ActiveProcesses = new ObservableCollection<ProcessDTO>();

			Initialize();
			SelectedProcess = ActiveProcesses.FirstOrDefault();
		}

		public void Initialize()
		{
			var wmiQueryString = "SELECT ProcessId, ExecutablePath, CommandLine FROM Win32_Process";
			using (var searcher = new ManagementObjectSearcher(wmiQueryString))
			using (var results = searcher.Get())
			{
				var query = from p in Process.GetProcesses()
					join mo in results.Cast<ManagementObject>()
						on p.Id equals (int) (uint) mo["ProcessId"]
					select new
					{
						Process = p,
						Path = (string) mo["ExecutablePath"],
						CommandLine = (string) mo["CommandLine"],
					};

				foreach (var item in query)
				{
					// Do what you want with the Process, Path, and CommandLine
					var process = new ProcessDTO {Process = item.Process};
					if (item.Path != null)
					{
						var icon = Icon.ExtractAssociatedIcon(item.Path);
						using (var bmp = icon?.ToBitmap())
						{
							var stream = new MemoryStream();
							bmp?.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
							process.ImageSource = BitmapFrame.Create(stream);
						}
					}
					ActiveProcesses.Add(process);
				}
			}
		}
	}
}