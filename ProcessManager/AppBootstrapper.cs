namespace ProcessManager
{
	using System;
	using System.Collections.Generic;
	using Caliburn.Micro;
	using ProcessManager.ViewModels;

	public class AppBootstrapper : BootstrapperBase
	{
		SimpleContainer container;

		static IEnumerable<System.Windows.DependencyObject> FluentRibbonChildResolver(Fluent.Ribbon ribbon)
		{
			foreach (var ti in ribbon.Tabs)
			{
				foreach (var group in ti.Groups)
				{
					foreach (var obj in BindingScope.GetNamedElements(group))
						yield return obj;
				}
			}
		}

		public AppBootstrapper()
		{
			Initialize();
		}

		protected override void Configure()
		{
			container = new SimpleContainer();

			container.Singleton<IWindowManager, WindowManager>();
			container.Singleton<IEventAggregator, EventAggregator>();
			container.PerRequest<IShell, ShellViewModel>();
			container.Singleton<ProcessListViewModel>();
			container.PerRequest<ProcessDetailsViewModel>();

			BindingScope.AddChildResolver<Fluent.Ribbon>(FluentRibbonChildResolver);
		}

		protected override object GetInstance(Type service, string key)
		{
			return container.GetInstance(service, key);
		}

		protected override IEnumerable<object> GetAllInstances(Type service)
		{
			return container.GetAllInstances(service);
		}

		protected override void BuildUp(object instance)
		{
			container.BuildUp(instance);
		}

		protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
		{
			DisplayRootViewFor<IShell>();
		}
	}
}