using Caliburn.Micro;
using ProcessManager.DTO;

namespace ProcessManager.ViewModels
{
	public class ProcessDetailsViewModel : Screen, IHandle<ProcessDTO>
	{
		private readonly IEventAggregator _eventAggregator;

		#region Binded Properties

		private ProcessDTO _process;

		public ProcessDTO Process
		{
			get => _process;
			set
			{
				_process = value;
				NotifyOfPropertyChange(() => Process);
			}
		}

		#endregion

		public ProcessDetailsViewModel(IEventAggregator eventAggregator)
		{
			_eventAggregator = eventAggregator;
			_eventAggregator.Subscribe(this);
		}

		protected override void OnActivate()
		{
			base.OnActivate();
		}

		public void Handle(ProcessDTO process)
		{
			if (process != null)
				Process = process;
		}
	}
}