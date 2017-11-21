using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using Caliburn.Micro;

namespace ProcessManager.ViewModels
{
	public class ShellViewModel : Conductor<IScreen>.Collection.AllActive, IShell
	{
		private readonly IWindowManager _windowManager;

		public ProcessListViewModel ProcessList { get; set; }
		public ProcessDetailsViewModel ProcessDetails { get; set; }

		public ShellViewModel(IWindowManager windowManager, ProcessListViewModel processList,
			ProcessDetailsViewModel processDetails)
		{
			_windowManager = windowManager;
			ProcessList = processList;
			ProcessDetails = processDetails;
		}

		public void RefreshProcessList()
		{
			ProcessList.RefreshProcessList();
		}

		public void KillSelectedProcess()
		{
			ProcessList.KillSelectedProcess();
		}

		public void SetIdlePriority()
		{
			ProcessList.SetPriority(ProcessPriorityClass.Idle);
		}

		public void SetNormalPriority()
		{
			ProcessList.SetPriority(ProcessPriorityClass.Normal);
		}

		public void SetHighPriority()
		{
			ProcessList.SetPriority(ProcessPriorityClass.High);
		}

		public void SetRealtimePriority()
		{
			ProcessList.SetPriority(ProcessPriorityClass.RealTime);
		}

		public void OnClose(CancelEventArgs eventArgs)
		{
			ProcessList.RestoreModifiedProcesses();
		}
	}
}