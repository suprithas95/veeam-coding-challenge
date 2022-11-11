using NUnit.Framework;
using Monitor;

namespace PMonitor.Tests
{
    [TestFixture]
    public class ProcessMonitor_checkRuntimeAndKill
    {

        [Test]
        public void TestForNonEmptyProcessMonitorOnVaidInputs()
        {
            Assert.DoesNotThrow(() => new ProcessMonitor("calculator", TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1)));
        }

        [Test]
        public void TestForErrorOnInvalidInputs()
        {
            Assert.Throws<Exception>(() => new ProcessMonitor("   ", TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1)));
        }
    }
}
