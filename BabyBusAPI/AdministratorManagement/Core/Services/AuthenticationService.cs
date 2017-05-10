using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using AdministratorManagement.Core.DataAccess;
using AdministratorManagement.Models;
using BabyBus.AutoModel;

namespace AdministratorManagement.Core.Services
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;

        public AuthenticationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            //create couple of default users
            //              byte[]   salt   =   System.Text.Encoding.ASCII.GetBytes("7MSLIpBR"); 
            //
            ////                        var salt =  GenerateSalt(6);
            //           
            //            var saltedPass = GenerateSaltedHash(Encoding.UTF8.GetBytes("123456"), salt);
            //            var baibai = new Admin
            //            {
            //                Guid = Guid.NewGuid(),
            //                UserName = "13002982075",
            //                PasswordHash = Convert.ToBase64String(saltedPass),
            //                Salt = Convert.ToBase64String(salt),
            //                Role = Roles.Master
            //            };
            //
            //            var yin = new Admin
            //            {
            //                Guid = Guid.NewGuid(),
            //                UserName = "13359276945",
            //                PasswordHash = Convert.ToBase64String(saltedPass),
            //                Salt = Convert.ToBase64String(salt),
            //                Role = Roles.Master
            //            };
            //
            //            var yin1 = new Admin
            //            {
            //                Guid = Guid.NewGuid(),
            //                UserName = "15002919968",
            //                PasswordHash = Convert.ToBase64String(saltedPass),
            //                Salt = Convert.ToBase64String(salt),
            //                Role = Roles.Master
            //            };
            //
            //            var yin2 = new Admin
            //            {
            //                Guid = Guid.NewGuid(),
            //                UserName = "18991163252",
            //                PasswordHash = Convert.ToBase64String(saltedPass),
            //                Salt = Convert.ToBase64String(salt),
            //                Role = Roles.Master
            //            };
            //            _userRepository.Save(baibai);
            //            _userRepository.Save(yin);
            //            _userRepository.Save(yin1);
            //            _userRepository.Save(yin2);
        }

        public AuthenticationResult Authenticate(Admin loginModel)
        {
            bool hasAdminAccount = false;
            bool hasAppAccount = false;
            AuthenticationResult ret = null;

            var user = _userRepository.FindByUsername(loginModel.UserName);

            if (user != null)
            {
                loginModel.Guid = user.Guid;
                loginModel.UserName = user.UserName;
                loginModel.ClassId = user.ClassId;
                loginModel.KindergartenId = user.KindergartenId;
                loginModel.ClassName = user.ClassName;
                loginModel.KindergartenName = user.KindergartenName;
                loginModel.Role = user.Role;
                loginModel.UserId = user.UserId;

                var saltedHash = GenerateSaltedHash(Encoding.UTF8.GetBytes(loginModel.PasswordHash),
                    Convert.FromBase64String(user.Salt));

                if (Convert.ToBase64String(saltedHash) != user.PasswordHash)
                {
                    hasAdminAccount = false;

                    UV_Teacher userApp = _userRepository.FindByUserAppName(loginModel.UserName);

                    if (userApp.Password == loginModel.PasswordHash)
                    {
                        hasAdminAccount = true;
                        
                    }
                }
                else
                {
                    hasAdminAccount = true;
                }
            }
            else
            {
                hasAdminAccount = false;

                UV_Teacher userApp = _userRepository.FindByUserAppName(loginModel.UserName);

                if (userApp != null)
                {
                    loginModel.UserName = userApp.LoginName;
                    loginModel.ClassId = userApp.ClassId;
                    loginModel.KindergartenId = userApp.KindergartenId;
                    loginModel.ClassName = userApp.ClassName;
                    loginModel.KindergartenName = userApp.KindergartenName;
                    loginModel.UserId = userApp.UserId;

                    if (userApp.RoleType == 2)
                    {
                        loginModel.Role = "Teacher";
                    }
                    else if (userApp.RoleType == 3)
                    {
                        loginModel.Role = "President";
                    }
                    else if (userApp.RoleType == 4)
                    {
                        loginModel.Role = "SuperPresident";
                    }
                    else
                    {
                        loginModel.Role = "Consumer";
                    }
                    if (userApp.Password == loginModel.PasswordHash)
                    {
                        loginModel.Guid = Guid.NewGuid();
                        hasAppAccount = true;
                        _userRepository.Save(loginModel);
                    }
                    else
                    {
                        hasAppAccount = false;
                    }
                }

            }


            if (hasAdminAccount || hasAppAccount)
            {
                ret = new AuthenticationResult() { IsAuthenticated = true, UserLogin = loginModel };
            }
            else
            {
                ret = new AuthenticationResult();
            }
            return ret;
        }

        private static byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            byte[] plainTextWithSaltBytes =
              new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];
            }

            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }

        private static byte[] GenerateSalt(int saltSize)
        {
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[saltSize];
            rng.GetBytes(buff);
            return buff;
        }
    }
}