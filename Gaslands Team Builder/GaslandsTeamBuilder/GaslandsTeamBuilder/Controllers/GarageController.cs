using GaslandsTeamBuilder.Models;
using GaslandsTeamBuilderCore;
using GaslandsTeamBuilderCore.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GaslandsTeamBuilder.Controllers
{
    [Authorize]
    public class GarageController : BaseController
    {
        ICoreLogic _coreLogic;

        public GarageController(ICoreLogic coreLogic)
        {
            _coreLogic = coreLogic;
        }

        // GET: Garage
        public ActionResult Index()
        {
            GarageViewModel model = new GarageViewModel()
            {
                Builds = _coreLogic.GetAllBuilds(_AppUserState.UserId),
                Teams = _coreLogic.GetAllTeams(_AppUserState.UserId)
            };

            return View("Home", model);
        }

        public ActionResult Team(int key = -1)
        {
            Team viewTeam = new Team();

            if (key == -1)
            {
                key = _coreLogic.CreateTeam(_AppUserState.UserId);
            }

            viewTeam = _coreLogic.GetTeam(key, _AppUserState.UserId);
            var builds = _coreLogic.GetAllBuilds(_AppUserState.UserId);

            TeamViewModel model = new TeamViewModel(viewTeam, builds);

            return View("Team", model);
        }

        public ActionResult Build(int key = -1)
        {
            Build viewBuild = new Build();

            if (key == -1)
            {
                key = _coreLogic.CreateBuild(_AppUserState.UserId);
                return RedirectToAction("Build", new { key = key });
            }

            viewBuild = _coreLogic.GetBuild(key, _AppUserState.UserId);
            var dropdowns = _coreLogic.GetDropDowns(viewBuild);

            BuildViewModel model = new BuildViewModel(viewBuild, dropdowns);
            model.SpecialRules = _coreLogic.GetSpecialRules(model.build);

            return View("Build", model);
        }

        #region AjaxCalls
        public ActionResult GetDashboardForBuild(int buildKey)
        {
            BuildViewModel model = new BuildViewModel();
            model.build = _coreLogic.GetBuild(buildKey, _AppUserState.UserId);
            model.SpecialRules = _coreLogic.GetSpecialRules(model.build);
            model.PrepForPrint();

            return View("BuildDashboardPrint", model);
        }

        public ActionResult ValidateBuild(int buildKey)
        {
            var validatedBuild = _coreLogic.GetBuild(buildKey, _AppUserState.UserId);
            var model = _coreLogic.ValidateBuild(validatedBuild, _AppUserState.UserId);

            return PartialView("ErrorListPartial", model);
        }

        public ActionResult GetDashboardForTeam(int teamKey)
        {
            TeamViewModel _team = new TeamViewModel();
            _team.team = _coreLogic.GetTeam(teamKey, _AppUserState.UserId);
            var builds = _team.team.TeamBuilds.Select(tb => _coreLogic.GetBuild(tb.Key, _AppUserState.UserId)).ToList();
            var model = builds.Select(b => new BuildViewModel(b, null, _coreLogic.GetSpecialRules(b).ToList())).ToList();
            foreach(BuildViewModel bvm in model)
            {
                bvm.PrepForPrint();
            }

            return View("TeamDashboardPrint", model);
        }

        public ActionResult ValidateTeam(int teamKey)
        {
            var validatedTeam = _coreLogic.GetTeam(teamKey, _AppUserState.UserId);
            var model = _coreLogic.ValidateTeam(validatedTeam, _AppUserState.UserId);

            return PartialView("ErrorListPartial", model);
        }

        public ActionResult SaveTeam(TeamViewModel dto)
        {
            var updatedTeam = _coreLogic.UpdateTeam(dto.team, _AppUserState.UserId);
            var builds = _coreLogic.GetAllBuilds(_AppUserState.UserId);

            TeamViewModel model = new TeamViewModel(updatedTeam, builds);

            return PartialView("TeamPartial", model);
        }

        public ActionResult DeleteTeam(int key)
        {
            _coreLogic.RemoveTeam(key, _AppUserState.UserId);

            return RedirectToAction("Index");
        }

        public ActionResult AddBuildToTeam(int teamKey, int buildKey)
        {
            var updatedTeam = _coreLogic.AddBuildToTeam(teamKey, buildKey, _AppUserState.UserId);
            var builds = _coreLogic.GetAllBuilds(_AppUserState.UserId);

            TeamViewModel model = new TeamViewModel(updatedTeam, builds);

            return PartialView("TeamPartial", model);
        }

        public ActionResult RemoveBuildFromTeam(int teamKey, int teamBuildKey)
        {
            var updatedTeam = _coreLogic.RemoveBuildFromTeam(teamKey, teamBuildKey, _AppUserState.UserId);
            var builds = _coreLogic.GetAllBuilds(_AppUserState.UserId);

            TeamViewModel model = new TeamViewModel(updatedTeam, builds);

            return PartialView("TeamPartial", model);
        }

        public ActionResult DeleteBuild(int key)
        {
            _coreLogic.RemoveBuild(key, _AppUserState.UserId);

            return RedirectToAction("Index");
        }

        public ActionResult SaveBuild(BuildViewModel dto)
        {
            var updatedBuild = _coreLogic.UpdateBuild(dto.build, _AppUserState.UserId);
            var dropdowns = _coreLogic.GetDropDowns(updatedBuild);

            BuildViewModel model = new BuildViewModel(updatedBuild, dropdowns);
            model.SpecialRules = _coreLogic.GetSpecialRules(model.build);

            return PartialView("BuildPartial", model);
        }

        public ActionResult AddPerk(int buildKey, int perkKey)
        {
            var updatedBuild = _coreLogic.AddPerk(buildKey, perkKey, _AppUserState.UserId);
            var dropdowns = _coreLogic.GetDropDowns(updatedBuild);

            BuildViewModel model = new BuildViewModel(updatedBuild, dropdowns);
            model.SpecialRules = _coreLogic.GetSpecialRules(model.build);

            return PartialView("BuildPartial", model);
        }

        public ActionResult RemovePerk(int buildKey, int perkKey)
        {
            var updatedBuild = _coreLogic.RemovePerk(buildKey, perkKey, _AppUserState.UserId);
            var dropdowns = _coreLogic.GetDropDowns(updatedBuild);

            BuildViewModel model = new BuildViewModel(updatedBuild, dropdowns);
            model.SpecialRules = _coreLogic.GetSpecialRules(model.build);

            return PartialView("BuildPartial", model);
        }

        public ActionResult AddUpgrade(int buildKey, int upgradeKey)
        {
            var updatedBuild = _coreLogic.AddUpgrade(buildKey, upgradeKey, _AppUserState.UserId);
            var dropdowns = _coreLogic.GetDropDowns(updatedBuild);

            BuildViewModel model = new BuildViewModel(updatedBuild, dropdowns);
            model.SpecialRules = _coreLogic.GetSpecialRules(model.build);

            return PartialView("BuildPartial", model);
        }

        public ActionResult RemoveUpgrade(int buildKey, int upgradeKey)
        {
            var updatedBuild = _coreLogic.RemoveUpgrade(buildKey, upgradeKey, _AppUserState.UserId);
            var dropdowns = _coreLogic.GetDropDowns(updatedBuild);

            BuildViewModel model = new BuildViewModel(updatedBuild, dropdowns);
            model.SpecialRules = _coreLogic.GetSpecialRules(model.build);

            return PartialView("BuildPartial", model);
        }

        public ActionResult AddWeapon(int buildKey, int weaponKey, string facing)
        {
            var updatedBuild = _coreLogic.AddWeapon(buildKey, weaponKey, facing, _AppUserState.UserId);
            var dropdowns = _coreLogic.GetDropDowns(updatedBuild);

            BuildViewModel model = new BuildViewModel(updatedBuild, dropdowns);
            model.SpecialRules = _coreLogic.GetSpecialRules(model.build);

            return PartialView("BuildPartial", model);
        }

        public ActionResult RemoveWeapon(int buildKey, int buildWeaponKey)
        {
            var updatedBuild = _coreLogic.RemoveWeapon(buildKey, buildWeaponKey, _AppUserState.UserId);
            var dropdowns = _coreLogic.GetDropDowns(updatedBuild);

            BuildViewModel model = new BuildViewModel(updatedBuild, dropdowns);
            model.SpecialRules = _coreLogic.GetSpecialRules(model.build);

            return PartialView("BuildPartial", model);
        }
        #endregion
    }
}