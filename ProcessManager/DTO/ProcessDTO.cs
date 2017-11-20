using System.Diagnostics;
using System.Drawing;
using System.Windows.Media;

namespace ProcessManager.DTO
{
	public class ProcessDTO
	{
		public Process Process { get; set; }
		public Icon Icon { get; set; }
		public ImageSource ImageSource { get; set; }
	}
}