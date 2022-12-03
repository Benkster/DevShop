using DevShop.Data;
using DevShop.Data.Models;
using DevShop.Authentication.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
namespace DevShop.Authentication
{
	/// <summary>
	/// The AuthManager handles everything related to authentication:
	/// # Registration
	/// # Hashing password
	/// # Validating the password on login
	/// # Logging in
	/// </summary>
	public class AuthManager : IAuthManager
	{
		#region Variables/Properties
		private readonly DevShopContext _context;
		private readonly IHttpContextAccessor _accessor;
		#endregion


		#region Constructors
		public AuthManager(DevShopContext context, IHttpContextAccessor accessor)
		{
			_context = context;
			_accessor = accessor;
		}
		#endregion



		#region Methods
		#region Public Methods
		/// <summary>
		/// Register a new user
		/// </summary>
		/// <param name="user">
		/// Contains the data for the new user
		/// </param>
		/// <returns>
		/// A boolean value indicating, whether the registration succeeded
		/// as well as a dictionary including all error-messages, if the registration failed
		/// </returns>
		public async Task<(bool success, Dictionary<string, string> errorMessages)> RegisterAsync(User user)
		{
			// Stores all error-messages, if the registration fails
			Dictionary<string, string> errorMessages = ValidateRegistration(user);


			// If there's at least one error-message, the registration failed
			if (errorMessages.Count > 0)
			{
				return (false, errorMessages);
			}


			// Encrypt the users password
			user.Password = HashPassword(user.Password);

			// Create the new user
			_context.Users.Add(user);
			await _context.SaveChangesAsync();


			// Registration succeeded
			return (true, errorMessages);
		}



		/// <summary>
		/// Logs a user in
		/// </summary>
		/// <param name="loginData">
		/// Contains the data, the user put in, necessary for login (E-Mail/Username and Password)
		/// </param>
		/// <returns>
		/// An error message, if the login failed or nothing, if the login succeeded
		/// </returns>
		public async Task<string> LoginAsync(LoginVM loginData)
		{
			// Get the user from the database via the given e-mail or username
			User user = await _context.Users.FirstOrDefaultAsync(u => u.Mail == loginData.UserName || u.UserName == loginData.UserName);


			// If no user has been found, the given e-mail or username is incorrect
			if (user == null)
			{
				return "Username/E-Mail incorrect";
			}


			// Check, whether the given password matches the password of the user in the databse
			if (!ValidatePasswordHash(loginData.Password, user.Password))
			{
				return "Password is invalid";
			}


			// Get the users role
			Role role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleNr == user.RoleNr);


			// Add claims for the cookie
			var claims = new List<Claim>();
			claims.Add(new Claim(ClaimTypes.Name, $"{user.UserName}"));
			claims.Add(new Claim(ClaimTypes.Email, $"{user.Mail}"));
			claims.Add(new Claim(ClaimTypes.Role, $"{role.Role1}"));


			var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			var authProperties = new AuthenticationProperties() {};


			// Sign the user in via setting a cookie
			await _accessor.HttpContext.SignInAsync(
				CookieAuthenticationDefaults.AuthenticationScheme,
				new ClaimsPrincipal(claimsIdentity),
				authProperties
			);


			// Login successful, return no error message
			return string.Empty;
		}
		#endregion



		#region Private Methods
		/// <summary>
		/// Basic validation to see, if the users data is valid and the user can be created
		/// </summary>
		/// <param name="user">
		/// Contains the data of the user, that should be validated
		/// </param>
		/// <returns>
		/// A dictionary containing all error-messages, in case the data is invalid
		/// </returns>
		private Dictionary<string, string> ValidateRegistration(User user)
		{
			// Stores all error-messages
			Dictionary<string, string> errorMessages = new Dictionary<string, string>();


			// No data has been passed
			if (user == null)
			{
				errorMessages.Add("user", "Failed to register. Please reload and try again");
			}

			
			// No E-Mail has been specified
			if (string.IsNullOrEmpty(user.Mail))
			{
				errorMessages.Add("mail", "E-Mail can't be empty");
			}


			// A user with the specified E-Mail already exists
			if (_context.Users.Any(u => u.Mail == user.Mail))
			{
				errorMessages.Add("mail", "The given E-Mail already exists");
			}


			// A user with the specified username already exists
			if (_context.Users.Any(u => u.UserName == user.UserName))
			{
				errorMessages.Add("username", "The given username is not available");
			}


			return errorMessages;
		}



		/// <summary>
		/// Encrypt the users password
		/// </summary>
		/// <param name="rawPassword">
		/// The users password as plain text, that should be encrypted
		/// </param>
		/// <returns>
		/// The hash of the users password
		/// </returns>
		private string HashPassword(string rawPassword)
		{
			// Add salt for extra security
			byte[] salt = new byte[50];
			// Actual hash data
			byte[] hashBytes = new byte[200];

			new RNGCryptoServiceProvider().GetBytes(salt);

			// Encrypt the password
			var encryptedPwd = new Rfc2898DeriveBytes(rawPassword, salt, 5000);

			// Create the hash of the encrypted password
			byte[] hash = encryptedPwd.GetBytes(150);

			Array.Copy(salt, 0, hashBytes, 0, 50);
			Array.Copy(hash, 0, hashBytes, 50, 150);


			return Convert.ToBase64String(hashBytes);
		}



		/// <summary>
		/// If a user tries to log in, this function compares the hash of the typed in password with the password stored in the database
		/// </summary>
		/// <param name="userPassword">
		/// The password, that the user typed in (plain text)
		/// </param>
		/// <param name="dbPassword">
		/// The hashed password that is stored in the database
		/// </param>
		/// <returns>
		/// True if the password is valid
		/// </returns>
		private bool ValidatePasswordHash(string userPassword, string dbPassword)
		{
			// Bytes of the hashed password, that is stored in the database
			byte[] dbPasswordHashBytes = Convert.FromBase64String(dbPassword);

			// Bytes for salt
			byte[] salt = new byte[50];

			
			// Fetch salt from hashed password
			Array.Copy(dbPasswordHashBytes, 0, salt, 0, 50);


			// Encrypt the password, that the user typed in
			var userPasswordBytes = new Rfc2898DeriveBytes(userPassword, salt, 5000);

			// Create the hash of the encrypted password
			byte[] userPasswordHash = userPasswordBytes.GetBytes(150);


			// Compare the hash from the database with the hash of the password, that the user typed in
			for (int index = 0; index < 150; index++)
			{
				// If the two hashes do not match, the typed in password is wrong (+ 50 to exclude salt)
				if (dbPasswordHashBytes[index + 50] != userPasswordHash[index])
				{
					return false;
				}
			}


			return true;
		}
		#endregion
		#endregion
	}
}
