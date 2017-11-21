using System.Windows;
using Caliburn.Micro;
using ProcessManager.DTO;

namespace ProcessManager.ViewModels
{
	public class ProcessDetailsViewModel : Screen, IHandle<ProcessDTO>
	{
		private readonly IEventAggregator _eventAggregator;

		#region Binded Properties

		private ProcessDTO _process;
		private Visibility _isVisible;

		public ProcessDTO Process
		{
			get => _process;
			set
			{
				_process = value;
				NotifyOfPropertyChange(() => Process);
			}
		}

		public Visibility IsVisible
		{
			get => _isVisible;
			set
			{
				_isVisible = value;
				NotifyOfPropertyChange(() => IsVisible);
			}
		}

		#endregion

		public ProcessDetailsViewModel(IEventAggregator eventAggregator)
		{
			_eventAggregator = eventAggregator;
			_eventAggregator.Subscribe(this);
			IsVisible = Visibility.Hidden;
		}

		protected override void OnActivate()
		{
			base.OnActivate();
		}

		public void Handle(ProcessDTO process)
		{
			if (process != null)
			{
				Process = process;
				IsVisible = Visibility.Visible;
			}
			else
			{
				IsVisible = Visibility.Hidden;
			}
		}
	}
}