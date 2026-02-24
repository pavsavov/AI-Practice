using Zalmo.Infrastructure;
using Zalmo.Core;

namespace Zalmo.UI;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
		// Load config and prefill fields
		var config = ConfigPersistenceService.LoadConfig();
		// Batch size removed
		FolderPatternEntry.Text = config.FolderNamingPattern;
		PeriodDaysEntry.Text = config.PeriodDays > 0 ? config.PeriodDays.ToString() : "30";
	}

	private async void OnContinueClicked(object? sender, EventArgs e)
	{
		// Validate and save config
		if (!int.TryParse(PeriodDaysEntry.Text, out int periodDays) || periodDays <= 0)
		{
			await DisplayAlert("Error", "Time period must be a positive integer.", "OK");
			return;
		}
		var pattern = string.IsNullOrWhiteSpace(FolderPatternEntry.Text) ? "yyyy.MM.dd" : FolderPatternEntry.Text;
		var config = new AppConfig { FolderNamingPattern = pattern, PeriodDays = periodDays };
		ConfigPersistenceService.SaveConfig(config);
		// Navigate to image library page
		await Navigation.PushAsync(new ImageLibraryPage());
	}
}
