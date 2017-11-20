using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;

namespace ProcessManager.ViewModels
{
	public class ProcessDetailsViewModel : Screen
	{
		protected override void OnActivate()
		{
			MessageBox.Show("Details activated"); //Don't do this in a real VM.
			base.OnActivate();
		}
	}
}