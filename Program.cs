using System.Diagnostics;
using Serilog;

namespace Veeam
{
    class ProcessMonitor
	{
		private string name = "";
		private TimeSpan lifetime = TimeSpan.Zero;
		private TimeSpan frequency = TimeSpan.Zero;
		private Serilog.Core.Logger logger;

		public ProcessMonitor(string name, TimeSpan lifetime, TimeSpan frequency) {
			this.name = name;
			this.lifetime = lifetime;
			this.frequency = frequency;

			this.logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
		}

		private void checkRuntimeAndKill(Object? stateInfo) {
			Process[] processCollection = Process.GetProcessesByName(this.name);
			foreach (Process p in processCollection) {
				try {
					TimeSpan runtime = DateTime.Now - p.StartTime;
					if (runtime > this.lifetime) {
						p.Kill();
						logger.Information("Process {0} killed after {1}", p.ProcessName, runtime.TotalMinutes);
					}
				} catch(Exception e) {
					logger.Error(e.ToString());
					continue;
				}
			}
		}

		private void MonitorProcess()
		{
			this.logger.Information("Press Q to Exit");
			var autoEvent = new AutoResetEvent(false);
			var stateTimer = new Timer(this.checkRuntimeAndKill, 
                                   autoEvent, 0, (int) this.frequency.TotalMilliseconds);
			do {
				
			} while (Console.ReadKey(true).Key != ConsoleKey.Q);				
		}

		static void Main(string[] args)
		{
			if(args.Length != 3) {
				Console.WriteLine("expected paramaters not present -- Ex: dotnet run notepad 1 1");
				return;
			}

			var processName = args[0];
			TimeSpan lifetime = TimeSpan.Zero, frequency = TimeSpan.Zero;
			try {
				lifetime = TimeSpan.Parse("00:" + args[1] + ":00");
				frequency = TimeSpan.Parse("00:" + args[2] + ":00");
			} catch(Exception e) {
				Console.WriteLine(e.ToString());
				Console.WriteLine("Invalid arguments provided name: {0} lifetime: {1} frequency: {2}", processName, args[1], args[2]);
				return;
			}
			
			ProcessMonitor p = new ProcessMonitor(processName, lifetime, frequency);
			p.MonitorProcess();
		}
	}
}
