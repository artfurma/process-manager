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
		private ObservableCollection<Process> _modifiedProcesses;

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
			_modifiedProcesses = new ObservableCollection<Process>();
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
					var process = new ProcessDTO {Process = item.Process, Path = item.Path};
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

		public void RefreshProcessList()
		{
//			if (Process.GetProcesses().Length != ActiveProcesses.Count)
//			{
//				ActiveProcesses.Clear();
//
//				var wmiQueryString = "SELECT ProcessId, ExecutablePath, CommandLine FROM Win32_Process";
//				using (var searcher = new ManagementObjectSearcher(wmiQueryString))
//				using (var results = searcher.Get())
//				{
//					var query = from p in Process.GetProcesses()
//						join mo in results.Cast<ManagementObject>()
//							on p.Id equals (int) (uint) mo["ProcessId"]
//						select new
//						{
//							Process = p,
//							Path = (string) mo["ExecutablePath"],
//							CommandLine = (string) mo["CommandLine"],
//						};
//
//					if (query.Count() != ActiveProcesses.Count)
//						foreach (var item in query)
//						{
//							var process = new ProcessDTO {Process = item.Process};
//							if (item.Path != null)
//							{
//								var icon = Icon.ExtractAssociatedIcon(item.Path);
//								using (var bmp = icon?.ToBitmap())
//								{
//									var stream = new MemoryStream();
//									bmp?.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
//									process.ImageSource = BitmapFrame.Create(stream);
//								}
//							}
//							ActiveProcesses.Add(process);
//						}
//				}
//				NotifyOfPropertyChange(() => ActiveProcesses);
//				NotifyOfPropertyChange(() => SelectedProcess);
//
//				return;
//			}

			foreach (var activeProcess in ActiveProcesses)
			{
				activeProcess.Process.Refresh();
			}

			NotifyOfPropertyChange(() => ActiveProcesses);
			NotifyOfPropertyChange(() => SelectedProcess);
			_events.PublishOnUIThread(SelectedProcess);
		}

		public void KillSelectedProcess()
		{
			var selected = SelectedProcess;
			SelectedProcess = ActiveProcesses.FirstOrDefault();
			selected.Process.StartInfo.FileName = selected.Path;

			_modifiedProcesses.Add(selected.Process);
			selected.Process.Kill();
			ActiveProcesses.Remove(selected);
		}

		public void SetPriority(ProcessPriorityClass priorityClass)
		{
			_modifiedProcesses.Add(SelectedProcess.Process);
			SelectedProcess.Process.PriorityClass = priorityClass;
//			SelectedProcess.Process.Refresh();
			_events.PublishOnUIThread(SelectedProcess);
		}

		public void RestoreModifiedProcesses()
		{
			var extractedProcesses = ActiveProcesses.Select(procdto => procdto.Process).ToList();

			foreach (var proc in _modifiedProcesses)
			{
				if (!extractedProcesses.Contains(proc))
				{
					var filename = proc.StartInfo.FileName;
					Process.Start(proc.StartInfo);
				}
			}
		}
	}
}