using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Zalmo.Core;

namespace Zalmo.Infrastructure
{
	using Microsoft.Extensions.Logging;

	public static class ImageMetadataService
	{
		private static ILogger? _logger;
		public static void SetLogger(ILogger logger) => _logger = logger;

		/// <summary>
		/// Extracts the date the photo was taken from EXIF metadata, or falls back to file creation time.
		/// </summary>
		/// <param name="filePath">Path to the image file.</param>
		/// <returns>DateTime the photo was taken or file creation time if EXIF is missing.</returns>
		public static DateTime GetPhotoTakenDate(string filePath)
		{
			try
			{
				var directories = ImageMetadataReader.ReadMetadata(filePath);
				var subIfdDirectory = directories
					.OfType<ExifSubIfdDirectory>()
					.FirstOrDefault();

				if (subIfdDirectory != null &&
					subIfdDirectory.TryGetDateTime(ExifDirectoryBase.TagDateTimeOriginal, out DateTime dateTaken))
				{
					_logger?.LogInformation("EXIF date found for {FilePath}: {Date}", filePath, dateTaken);
					return dateTaken;
				}
				_logger?.LogWarning("No EXIF date found for {FilePath}, using file creation time.", filePath);
			}
			catch (Exception ex)
			{
				_logger?.LogError(ex, "Error reading metadata for {FilePath}, using file creation time.", filePath);
			}
			// Fallback: file creation time
			return File.GetCreationTime(filePath);
		}
	}

	public static class ImageBatchGrouper
	{
		private static ILogger? _logger;
		public static void SetLogger(ILogger logger) => _logger = logger;

		/// <summary>
		/// Groups image files by date (from metadata or file creation), and copies them into DUMP/job-yyyy.MM.dd folders in parallel.
		/// </summary>
		/// <param name="imagePaths">List of image file paths.</param>
		/// <param name="dumpRoot">Root DUMP directory.</param>
		/// <param name="folderPattern">Pattern for folder naming (e.g., yyyy.MM.dd).</param>
		public static void GroupAndDumpImages(IEnumerable<string> imagePaths, string dumpRoot, string folderPattern, int periodDays)
		{
			System.IO.Directory.CreateDirectory(dumpRoot);
			// Sort images by date
			var datedImages = imagePaths.AsParallel()
				.Select(path => new
				{
					Path = path,
					Date = ImageMetadataService.GetPhotoTakenDate(path).Date
				})
				.OrderBy(x => x.Date)
				.ToList();

			// Group images by periodDays
			var groups = new List<List<string>>();
			List<string> currentGroup = new();
			DateTime? groupStart = null;
			foreach (var img in datedImages)
			{
				if (groupStart == null || (img.Date - groupStart.Value).TotalDays >= periodDays)
				{
					if (currentGroup.Count > 0)
						groups.Add(currentGroup);
					currentGroup = new List<string>();
					groupStart = img.Date;
				}
				currentGroup.Add(img.Path);
			}
			if (currentGroup.Count > 0)
				groups.Add(currentGroup);

			// Copy files into folders
			int groupIndex = 1;
			foreach (var group in groups)
			{
				var lastDate = ImageMetadataService.GetPhotoTakenDate(group.Last()).Date;
				var folderName = $"job-{lastDate:yyyy.MM.dd}-group{groupIndex}";
				var targetDir = Path.Combine(dumpRoot, folderName);
				System.IO.Directory.CreateDirectory(targetDir);
				foreach (var imgPath in group)
				{
					var fileName = Path.GetFileName(imgPath);
					var destPath = Path.Combine(targetDir, fileName);
					try
					{
						File.Copy(imgPath, destPath, overwrite: true);
						_logger?.LogInformation("Copied {Source} to {Destination}", imgPath, destPath);
					}
					catch (Exception ex)
					{
						_logger?.LogError(ex, "Failed to copy {Source} to {Destination}", imgPath, destPath);
					}
				}
				groupIndex++;
			}
		}
	}

	public static class ConfigPersistenceService
	{
		private static readonly string ConfigFile = "appconfig.json";

		public static void SaveConfig(AppConfig config)
		{
			var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
			File.WriteAllText(ConfigFile, json);
		}

		public static AppConfig LoadConfig()
		{
			if (!File.Exists(ConfigFile))
				return new AppConfig();
			var json = File.ReadAllText(ConfigFile);
			return JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
		}
	}
}
