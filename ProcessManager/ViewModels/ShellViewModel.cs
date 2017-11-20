using Caliburn.Micro;

namespace ProcessManager.ViewModels
{
	public class ShellViewModel : Conductor<IScreen>.Collection.OneActive, IShell
	{
		private readonly IWindowManager _windowManager;

		public ProcessListViewModel List { get; set; }
		public ProcessDetailsViewModel Details { get; set; }

		public ShellViewModel(IWindowManager windowManager)
		{
			_windowManager = windowManager;
			ActivateItem(new ProcessListViewModel());
//			ActivateItem(new ProcessDetailsViewModel());
		}
	}
}