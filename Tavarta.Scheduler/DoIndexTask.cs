using System;
using System.Threading.Tasks;
using DNTScheduler;
using Tavarta.ServiceLayer.EFServiecs.Schedulers;

namespace Tavarta.Scheduler
{
    public class DoIndexTask : ScheduledTaskTemplate
    {

        IndexService _indexService = new IndexService();

        public override bool RunAt(DateTime utcNow)
        {
            if (IsShuttingDown || Pause)
                return false;

            var now = utcNow.AddHours(3.5);
            //return now.Minute % 1 == 0 && now.Second == 1;
            return (now.Day % 7 == 0) && (now.Hour == 0 && now.Minute == 1 && now.Second == 1);
            /*(now.DayOfWeek == DayOfWeek.Friday) &&
                   (now.Hour == 3) &&
                   (now.Minute == 1) &&
                   (now.Second == 1)*/
            //now.Hour == 23 && now.Minute == 33 && now.Second == 1;
        }
        public override void Run()
        {
            if (IsShuttingDown || Pause)
                return;
            _indexService.AllIndexRebuild();
            
            //_indexService.ReBuildRolesIndex();
            //_indexService.ReBuildUserIndex();
            //_indexService.ReBuildUserLoginsIndex();
            //_indexService.ReBuildUserRoleIndex();
            //_indexService.RebuildUserClaimIndex();

            System.Diagnostics.Trace.WriteLine("Running Do Index");
        }

        public override Task RunAsync()
        {
            return base.RunAsync();
        }

        public override string Name => "تهيه ایندکس";
        public override int Order { get; }
    }
}