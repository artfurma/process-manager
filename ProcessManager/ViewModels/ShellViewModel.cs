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
	}
}