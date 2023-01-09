using DevShop.Data.ViewModels;
using Microsoft.AspNetCore.Components.Forms;

namespace DevShop.Data
{
	/// <summary>
	/// The FileManager allows uploading and deleting images/files
	/// </summary>
	public class FileManager
	{
		#region Variables
		private readonly IWebHostEnvironment _env;

		private string[] fileExt;
		#endregion


		#region Constructors
		public FileManager(IWebHostEnvironment env)
		{
			_env = env;
			fileExt = new string[] { "jpg", "jpeg", "gif", "png", "svg" };
		}
		#endregion



		#region Methods
		/// <summary>
		/// Uploads a given file.
		/// </summary>
		/// <param name="_path">
		/// The location, where the file will be stored (will be created if it does not yet exist)
		/// </param>
		/// <param name="_imageFile">
		/// Data of the file, that is beeing uploaded
		/// </param>
		/// <returns>
		/// True if the upload was successful, false if it failed
		/// </returns>
		public async Task<bool> UploadFile(string _path, ImageFile _imageFile)
		{
			try
			{
				// Create a buffer of the image
				var buffer = Convert.FromBase64String(_imageFile.Base64Data);

				// Get the full path of the loactaion
				string fullPath = _env.ContentRootPath + _path;

				// Get the type of the file
				string fileType = _imageFile.FileType.Split("/")[1];


				// If the given directory does not exist yet, create it
				if (!Directory.Exists(fullPath))
				{
					Directory.CreateDirectory(fullPath);
				}


				// Upload the file
				await File.WriteAllBytesAsync(fullPath + Path.DirectorySeparatorChar + _imageFile.FileName + "." + fileType, buffer);



				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}



		/// <summary>
		/// Stores the information of a file, that should be uploaded
		/// </summary>
		/// <param name="_args">
		/// Information about the onchange-event of an InputFile-Element
		/// </param>
		/// <param name="_fileName">
		/// Name of the file
		/// </param>
		/// <returns></returns>
		public async Task<ImageFile> StoreFile(InputFileChangeEventArgs _args, string _fileName)
		{
			// Stores the information of the file
			ImageFile storedFile = new ImageFile();

			// Get the selected file
			var selFile = _args.File;
			// Resize the file -> if the file is too big, it won't upload correctly and get corrupted (due to limited buffer-size)
			var resizedFile = await selFile.RequestImageFileAsync(selFile.ContentType, 640, 480);
			var buffer = new byte[resizedFile.Size];


			using (var stream = resizedFile.OpenReadStream(maxAllowedSize: 50000000))
			{
				await stream.ReadAsync(buffer);
			}


			// Store the data of the file
			storedFile.Base64Data = Convert.ToBase64String(buffer);
			storedFile.FileType = selFile.ContentType;
			storedFile.FileName = _fileName;


			return storedFile;
		}



		/// <summary>
		/// Checks, whether a given file exists or not
		/// </summary>
		/// <param name="_path">
		/// The location of the file
		/// </param>
		/// <returns>
		/// True and the extension of the file, if it exists
		/// </returns>
		public Dictionary<bool, string> FileExists(string _path)
		{
			// Information, whether file exists or not
			Dictionary<bool, string> result = new Dictionary<bool, string>();


			// Check all available extensions
			foreach (string ext in fileExt)
			{
				// If the file exists, return true and the extension of the file
				if (File.Exists(_env.ContentRootPath + _path + "." + ext))
				{
					result.Add(true, ext);

					return result;
				}
			}


			// The file does not exist
			result.Add(false, string.Empty);

			return result;
		}



		/// <summary>
		/// Deletes an existing file
		/// </summary>
		/// <param name="_path">
		/// The location of the file
		/// </param>
		/// <returns>
		/// True if the file was deleted
		/// </returns>
		public bool DeleteFile(string _path)
		{
			bool deleted = false;
			

			// Delete all files with the given name, no matter what extension the file has (files with the same name but different extensions will all get deleted)
			foreach (string ext in fileExt)
			{
				string fullPath = _env.ContentRootPath + _path + "." + ext;


				// Delete the file if it exists
				if (File.Exists(fullPath))
				{
					File.Delete(fullPath);
					deleted = true;
				}
			}


			return deleted;
		}
		#endregion
	}
}
