using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Media;

namespace ProcessManager.DTO
{
	public class ProcessDTO
	{
		public Process Process { get; set; }
		public ImageSource ImageSource { get; set; }
		public string MemorySize => (Process.VirtualMemorySize64 / 1024).ToString(CultureInfo.InvariantCulture) + " K";
		public int ThreadCount => Process.Threads.Count;
		public int ModuleCount => Process.Modules.Count;
		public string Path { get; set; }
	}
}