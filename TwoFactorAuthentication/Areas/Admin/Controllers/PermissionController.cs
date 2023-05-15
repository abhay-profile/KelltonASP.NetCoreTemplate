using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using TwoFactorAuthentication.Areas.Identity.Data;
using TwoFactorAuthentication.Models;
using TwoFactorAuthentication.RequestDTO;
using TwoFactorAuthentication.VMModels;

namespace TwoFactorAuthentication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PermissionController : Controller
    {
        private ApplicationDbContext _dbContext;
        private UserManager<ApplicationUser> _userManager;

        public PermissionController(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = applicationDbContext;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            List<RolesViewDataModel> rolesData = _dbContext.Roles.AsNoTracking().Select(r => new RolesViewDataModel
            {
                Id = r.Id,
                Name = r.Name
            }).ToList();


            return View(new RolesFeatureViewDataModel
            {
                Roles = rolesData,

            });
        }

        [HttpPost]
        public IActionResult UsersByRoles(string roleId)
        {

            var role = _dbContext.Roles.FirstOrDefault(r => r.Id == roleId);

            if (role != null)
            {
                // Get the user names in the role
                var UserIdelist = _dbContext.UserRoles.AsNoTracking().Where(u => u.RoleId == role.Id).Select(u => u.UserId).ToList();
                List<UsersByRoles> userlist = _dbContext.Users.AsNoTracking().Where(u => UserIdelist.Contains(u.Id)).Select(u => new UsersByRoles
                {
                    Id = u.Id,
                    Name = u.Email


                }).ToList();

                return PartialView("UserByRoles", userlist);
                // Do something with the user names
            }
            return Ok();

        }
        [HttpPost]
        public IActionResult CreateViewByRole(string roleId)
        {
            List<FeatureViewDataModel2> featuresCreatview = new List<FeatureViewDataModel2>();
            var allfeature = _dbContext.Features.ToList();
            foreach (var feature in allfeature)
            {
                var checkpermission = _dbContext.RolesPermissions.AsNoTracking().Where(r => r.FeatureId == feature.Id && r.RoleId == roleId).FirstOrDefault();
                if (checkpermission != null)
                {
                    featuresCreatview.Add(new()
                    {
                        Id = checkpermission.FeatureId,
                        Name = feature.Name,
                        IsCreate = checkpermission.IsCreate,
                        IsDeleted = checkpermission.IsDelete,
                        IsEdit = checkpermission.IsEdit,
                        IsView = checkpermission.IsView,

                    });


                }
                else
                {
                    featuresCreatview.Add(new()
                    {
                        Id = feature.Id,
                        Name = feature.Name,
                        IsView = false,
                        IsEdit = false,
                        IsCreate = false,
                        IsDeleted = false,
                    });
                }
            }
            return PartialView("_ViewPermissionByRole", featuresCreatview);

        }

        [HttpPost]
        public IActionResult CreatePermission(PermissionRequest request)
        {
            if (request == null)
            {
                return BadRequest();

            }
            else if (request.RoleId != null && request.UserId == null)
            {

                var existingPermissions = _dbContext.RolesPermissions.Where(rp => rp.RoleId == request.RoleId && request.FeaturePermissionRequests.Select(rf => rf.FeatureId).Contains(rp.FeatureId)).ToList();
                List<RolesPermissions> ExistRolePermission = new();
                List<RolesPermissions> AddNewRolePermission = new();
                var RoleExist = _dbContext.Roles.Where(r => r.Id == request.RoleId.ToString()).FirstOrDefault();
                if (RoleExist != null)
                {
                    foreach (var permission in request.FeaturePermissionRequests)
                    {
                        var existingPermission = existingPermissions.FirstOrDefault(rp => rp.FeatureId == permission.FeatureId);
                        if (existingPermission != null)
                        {
                            existingPermission.IsCreate = permission.IsCreate;
                            existingPermission.IsDelete = permission.IsDeleted;
                            existingPermission.IsView = permission.IsView;
                            existingPermission.IsEdit = permission.IsEdit;
                        }
                        else
                        {
                            AddNewRolePermission.Add(new RolesPermissions()
                            {

                                FeatureId = permission.FeatureId,
                                RoleId = RoleExist.Id,
                                IsCreate = permission.IsCreate,
                                IsDelete = permission.IsDeleted,
                                IsEdit = permission.IsEdit,
                                IsView = permission.IsView,


                            });

                        }



                    }

                    _dbContext.SaveChanges();
                    _dbContext.AddRange(AddNewRolePermission);
                    _dbContext.SaveChanges();
                    return Ok("Role Permission Save Successfully");
                }
            }
            else if (request.RoleId != null && request.UserId != null)
            {

                var existingUserPermissions = _dbContext.UserPermissions.Where(rp => rp.UserId == request.UserId && request.FeaturePermissionRequests.Select(rf => rf.FeatureId).Contains(rp.FeatureId)).ToList();
                List<UserPermissions> ExistUserPermission = new();
                List<UserPermissions> AddNewUserPermission = new();
                var UserExist = _dbContext.Roles.Where(r => r.Id == request.RoleId).FirstOrDefault();
                if (UserExist != null)
                {
                    foreach (var permission in request.FeaturePermissionRequests)
                    {
                        var existingUserPermission = existingUserPermissions.FirstOrDefault(rp => rp.FeatureId == permission.FeatureId);
                        if (existingUserPermission != null)
                        {

                            existingUserPermission.IsDelete = permission.IsDeleted;
                            existingUserPermission.IsCreate = permission.IsCreate;
                            existingUserPermission.IsView = permission.IsView;
                            existingUserPermission.IsEdit = permission.IsEdit;
                        }
                        else
                        {
                            AddNewUserPermission.Add(new UserPermissions()
                            {

                                FeatureId = permission.FeatureId,
                                UserId = request.UserId,
                                IsCreate = permission.IsCreate,
                                IsDelete = permission.IsDeleted,
                                IsEdit = permission.IsEdit,
                                IsView = permission.IsView,


                            });

                        }



                    }
                    _dbContext.SaveChanges();
                    _dbContext.AddRange(AddNewUserPermission);
                    _dbContext.SaveChanges();
                    return Ok("User Permission Save Successfully");


                }


            }



            return BadRequest("Something went worng");
        }

        [HttpPost]
        public IActionResult CreateViewByUser(string userId)
        {
            List<FeatureViewDataModel2> featuresCreatview = new List<FeatureViewDataModel2>();
            var allfeature = _dbContext.Features.ToList();
            foreach (var feature in allfeature)
            {
                var checkpermission = _dbContext.UserPermissions.AsNoTracking().Where(r => r.FeatureId == feature.Id && r.UserId == userId).FirstOrDefault();
                if (checkpermission != null)
                {
                    featuresCreatview.Add(new()
                    {
                        Id = checkpermission.FeatureId,
                        Name = feature.Name,
                        IsCreate = checkpermission.IsCreate,
                        IsDeleted = checkpermission.IsDelete,
                        IsEdit = checkpermission.IsEdit,
                        IsView = checkpermission.IsView,

                    });


                }
                else
                {
                    featuresCreatview.Add(new()
                    {
                        Id = feature.Id,
                        Name = feature.Name,
                        IsView = false,
                        IsEdit = false,
                        IsCreate = false,
                        IsDeleted = false,
                    });
                }
            }
            return PartialView("_ViewPermissionByRole", featuresCreatview);

        }
    }
}
