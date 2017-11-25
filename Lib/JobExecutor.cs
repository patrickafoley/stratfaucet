
using FluentScheduler;
using stratfaucet.Jobs;

namespace stratfaucet.Lib
{
    public class JobExecutor {

        public static void Init(WalletUtils walletUtils)
        {
            JobManager.AddJob(() => SendCoinJob.Execute(walletUtils), s => s
                .ToRunEvery(3)
                .Seconds());

        }

    }
}
