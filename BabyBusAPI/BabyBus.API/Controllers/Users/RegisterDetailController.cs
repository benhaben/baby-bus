using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using BabyBus.API.Utils;
using BabyBus.Core.Helper;
using BabyBus.Model.Entities.Main;
using BabyBus.Service.Models;
using BabyBus.Service.Models.Enum;
using BabyBus.Service.WebAPI;
using Newtonsoft.Json;

namespace BabyBus.API.Controllers.Users
{
    public class RegisterDetailController : ApiController
    {
        private readonly IMemberService _service;
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof (RegisterDetailController));
        public RegisterDetailController(IMemberService service)
        {
            _service = service;
        }

        public ApiResponser Post(CheckoutModel checkModel)
        {
            var response = new ApiResponser(true);

            try
            {
                switch (checkModel.CheckoutType)
                {
                    case CheckoutType.RegisterDetail:
                        GetImageName(checkModel);
                        _service.ParendBinding(checkModel, ref response);
                        response.Attach = checkModel.ImageName;
                        break;
                        case CheckoutType.UploadImage:
                        GetImageName(checkModel);
                        _service.UpdateChildImage(checkModel);
                        response.Attach = checkModel.ImageName;
                        break;
                        case CheckoutType.UpdateAll:
                        GetImageName(checkModel);
                        _service.UpdateAll(checkModel);
                        response.Attach = checkModel.ImageName;
                        break;
                    default:
                        response.Status = false;
                        response.Message = "No match CheckOut Type";
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Debug(ex.ToString());
                response.Status = false;
                response.Message = ex.Message;
                return response;
            }

            return response;
        }



        private void GetImageName(CheckoutModel checkModel)
        {
            if (checkModel.HasHeadImage)
            {
                var guid = Guid.NewGuid();
                var fileName = guid + Config.PhotoSuffix;
                checkModel.ImageName = fileName;
            }
        }
    }
}
