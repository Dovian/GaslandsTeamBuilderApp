using GaslandsTeamBuilder.Models;
using GaslandsTeamBuilderCore;
using System.Web.Mvc;

namespace GaslandsTeamBuilder.Controllers
{
    public class AccountController : BaseController
    {
        ICoreLogic _coreLogic;

        public AccountController(ICoreLogic coreLogic)
        {
            _coreLogic = coreLogic;
        }

        public ActionResult SignOut()
        {
            IdentitySignout();

            return RedirectToAction("Index", "Account");
        }

        public ActionResult Index()
        {
            return View("Login", new LoginViewModel());
        }

        public string CreateUser(LoginViewModel model)
        {
            var result = "success";

            if (string.Equals(model.Password, model.PasswordCheck))
            {
                var userId = _coreLogic.CreateUser(model.Username, model.Password);
                if (userId == -1)
                {
                    result = "A user already exists with that username.";
                }
                else
                {
                    AppUserState appUserState = new AppUserState()
                    {
                        UserId = userId,
                        Username = model.Username
                    };

                    IdentitySignin(appUserState);
                }
            }
            else
            {
                result = "The passwords don't match.";
            }

            return result;
        }

        public ActionResult CreateUserPage()
        {
            return View("CreateUser", new LoginViewModel());
        }

        public string TryLogin(LoginViewModel model)
        {
            var result = "success";
            var userId = _coreLogic.Login(model.Username, model.Password);
            int tryInt;

            if (userId == "-1")
            {
                result = "Could not login with that username or password.";
            }
            else if (int.TryParse(userId, out tryInt))
            {
                AppUserState appUserState = new AppUserState()
                {
                    UserId = tryInt,
                    Username = model.Username
                };

                IdentitySignin(appUserState);
            }
            else
            {
                return userId;
            }

            return result;
        }
    }
}