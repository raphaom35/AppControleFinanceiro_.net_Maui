using AppControleFinanceiro.Repositories;
using AppControleFinanceiro.Views;
using LiteDB;
using Microsoft.Extensions.Logging;

namespace AppControleFinanceiro;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.RegisterDatabaseAndRepositories()
			.RegisterViews();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
	public static MauiAppBuilder RegisterDatabaseAndRepositories(this MauiAppBuilder mauibuilder)
	{
		mauibuilder.Services.AddSingleton<LiteDatabase>(
			options =>
			{
				return new LiteDatabase($"Filename={AppSettings.DatabasePath};Connection=Shared");
            }
            );
		mauibuilder.Services.AddTransient<ITransactionRepository, TransactionRepository>();
        return mauibuilder;
	}
    public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauibuilder)
    {

        mauibuilder.Services.AddTransient<TransactionAdd>();
        mauibuilder.Services.AddTransient<TransactionList>();
        mauibuilder.Services.AddTransient<TransactionEdit>();
        return mauibuilder;
    }
}
