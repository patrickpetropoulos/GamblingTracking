using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Sports.Application;

namespace Sports.UnitTests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public async Task Test1()
        {
            var downloader = new MlbLineupsDownloader();
            var html = await downloader.DownloadHtml(new DateTime(2022, 4, 8));

            var parser = new MlbLineupsParser();
            await parser.ParseLineups(html);
        }
    }
}