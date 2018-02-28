using PKS.MgmtServices.Core;
using PKS.Models;
using Topshelf;
using Topshelf.Runtime;

namespace PKS.MgmtServices.Host
{
    /// <summary>主程序</summary>
    class Program
    {
        /// <summary>入口</summary>
        static void Main(string[] args)
        {
            //Test();
            Run();
        }
        /// <summary>测试</summary>
        private static void Test()
        {
            //var converter = new Converters.OfficePPTToPdfConverter();
            //var converter = new Converters.OfficeExcelToPdfConverter();
            //var converter = new Converters.OfficeExcelToHtmlConverter();
            //var converter = new Converters.OfficeWordToHtmlConverter();
            var converter = new Converters.ITextSharpPdfToFulltextConverter();
            //var source = @"E:\项目\勘探协同(0831)\LH30-1_风险审1.ppt";
            //var source = @"E:\项目\勘探协同(0831)\共享成果与A2对比表.xlsx";
            //var source = @"E:\项目\勘探协同(0831)\WITSML随钻决策支持系统建设项目数据传输数据格式.docx";
            //var source = @"C:\Users\Administrator\Documents\263EM\hubo@jurassic.com.cn\receive_file\表2-历年勘探工作量.xlsx";

            var source = @"C:\Users\Administrator\Desktop\3a7cbe7c-df88-44d4-b94f-7562e9265bdd.pdf";
            var dest = @"C:\Users\Administrator\Desktop\3a7cbe7c-df88-44d4-b94f-7562e9265bdd_2.pdf";
            converter.Execute(source, dest);
        }
        /// <summary>入口</summary>
        static void Run()
        {
            HostFactory.Run(x =>
            {
                x.Service<MgmtServiceBootstrapper>(sc =>
                {
                    sc.ConstructUsing(Start);
                    sc.WhenStarted(tc => tc.Start());
                    sc.WhenStopped(tc => tc.Stop());
                });
                x.UseNLog();
                x.OnException(ex => MgmtServiceBootstrapper.Error("Topshelf:", ex));
                x.RunAsLocalSystem();
                x.StartAutomaticallyDelayed();
                x.SetServiceName(PKSWebConsts.MgmtServicesHost);
                x.SetDisplayName("PKS管理服务宿主");
                x.SetDescription("PKS Management Services Host");
            });
        }
        /// <summary>启动</summary>
        private static MgmtServiceBootstrapper Start(HostSettings settings)
        {
            var bootstrapper = new MgmtServiceBootstrapper();
            bootstrapper.Initialize();
            return bootstrapper;
        }
    }
}
