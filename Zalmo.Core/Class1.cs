namespace Zalmo.Core
{
	/// <summary>
	/// App configuration model for batch processing and folder naming.
	/// </summary>
	public class AppConfig
	{


		/// <summary>
		/// Folder naming pattern (e.g., yyyy.MM.dd, dd.MM.yyyy).
		/// </summary>
		public string FolderNamingPattern { get; set; } = "yyyy.MM.dd";

		/// <summary>
		/// Time period in days for grouping images into a single folder.
		/// </summary>
		public int PeriodDays { get; set; } = 30;
	}
}
