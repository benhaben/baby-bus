using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Extensions;
using System.Web.OData.Query;
using AutoMapper;
using BabyBus.Core.Helper;
using BabyBus.Model.Entities.Login;
using BabyBus.Service.General;
using Microsoft.Ajax.Utilities;


namespace BabyBus.API.Controllers
{
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        //Post: api/User add & edit
        public ApiResponser Post(User user)
        {
            ApiResponser response = new ApiResponser(true);
            if (user.UserId != 0)
            {
                _userService.EditUser(user);
                response.Attach = user;
            }
            else
            {
               _userService.CreateUser(user);
                response.Attach = user;
            }
            return response;
        }

        //Get: api/User inquary
        public ApiResult<User> GetUser(ODataQueryOptions<User> options)
        {
            var response = new ApiResult<User>();
            var list = _userService.GetAllUser();

            var result = options.ApplyTo(list) as IEnumerable<User>;
            if (result != null)
            {
                var enumerable = result.ToList();
                response.Status = true;
                response.Items = enumerable;
                response.TotalCount = enumerable.Count;
            }
            else
            {
                response.Status = false;
                response.Message = "查询期间发生错误";
            }
            return response;
        }

        //public PageResult<User> GetUser(ODataQueryOptions<User> options)
        //{
        //    var settings = new ODataQuerySettings
        //    {
        //        PageSize = 10
        //    };
        //    var results = options.ApplyTo(_userService.GetAllUser().AsQueryable(), settings);
        //    return new PageResult<User>(
        //    results as IEnumerable<User>,
        //    Request.ODataProperties().NextLink,
        //    Request.ODataProperties().TotalCount);
        //}
        //public IHttpActionResult Get(int id)
        //{
        //    var location = _userService.GetUser(id);
        //    var viewModel = new User();
        //    Mapper.Map(location, viewModel);
        //    return Ok(viewModel);
        //}
        //public IHttpActionResult Post(User createuser)
        //{
        //    var user = new User();
        //    Mapper.Map(createuser, user);
        //    _userService.CreateUser(user);
        //    return Created(Url.Link("DefaultApi", new { controller = "User", id = createuser.UserId }), createuser);
        //}
        //public IHttpActionResult Put(int id, User createuser)
        //{
        //    createuser.UserId = id;
        //    var location = _userService.GetUser(id);
        //    Mapper.Map(createuser, location);
        //    _userService.EditUser(location);
        //    return Ok(createuser);
        //}
        //public IHttpActionResult Delete(int id)
        //{
        //    _userService.DeleteUser(id);
        //    return Ok();
        //}
    }
}
