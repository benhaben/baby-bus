/*
 * Core script to handle all login specific things
 */

var Kindergarten = function () {

    "use strict";

    /* * * * * * * * * * * *
    * callApi is a ajax proxy
    * * * * * * * * * * * */
    var callApi = function (urlParam, method, dataParam) {
        return $.ajax({
            //async: false,
            url: urlParam,
            type: method,
            data: dataParam,
            cache: false,
            //username: data.username,
            //password: data.password,
            //enctype: "application/x-www-form-urlencoded",
            //dataType: "json",
            //processData:false,
            //contentType: 'application/x-www-form-urlencoded'
        });

    };

    var redirect = function (paras) {
        var url = window.location.href;
        //var paraString = url.substring(url.indexOf("?") + 1, url.length).split("&");
        //var paraObj = {}
        //for (var i = 0; i < paraString.count; i++) {
        //    var j = paraString[i];
        //    paraObj[j.substring(0, j.indexOf("=")).toLowerCase()] = j.substring(j.indexOf("=") + 1, j.length);
        //}
        var returnValue = "";
        var urlSlash = url.split("/");
        urlSlash[urlSlash.length - 1] = paras;
        returnValue = urlSlash.join();
        returnValue = returnValue.replace(/\,/g, "/");
        if (returnValue !== "") {
            window.location = returnValue;
        }
    };

    var checkIsLogin = function () {
        var loginUserName = $.cookie("LoginUserName");
        var roleVal = $.cookie("Role");
        if (loginUserName == null
                || loginUserName == ''
                || roleVal == null
                || roleVal == '') {
            return false;
        }
        return true;
    };

    var handlebarsHelp = function (templateIdSelector, context) {
        var source = $(templateIdSelector).html();
        var template = Handlebars.compile(source);
        var html = template(context);
        return html;
    };

    var getPermissionType = function () {
        var permissionType = -1;
        var roleVal = $.cookie("Role");
        permissionType = roleVal;
        if (roleVal == "Administrator") {
            permissionType = 0;
        } else if (roleVal == "Parent") {
            permissionType = 1;
        } else if (roleVal == "Teacher") {
            permissionType = 2;
        } else if (roleVal == "President") {
            permissionType = 3;
        } else if (roleVal == "SuperPresident") {
            permissionType = 4;
        }
        return permissionType;
    };

    var showDiv = function () {
        
        $("#box").css({ "display": "block" });
        $("#Mybox").css({ "display": "block" });
        $("#bodyonline").css({ "overflow-y": "hidden" });
    };
    var hideDiv = function () {
        
        $("#box").css({ "display": "none" });
        $("#Mybox").css({ "display": "none" });
        $("#bodyonline").css({ "overflow-y": "scroll" });
    };
    var isShowCreateEditBtn = function (permissionType) {
        var isShow = false;
        if (permissionType == 0 || permissionType == 1 || permissionType == 2) {
            isShow = true;
        }
        return isShow;
    };

    var generatePresidentInfo = function (kindergartenId, kinName) {
        $("#kinName").text(kinName);
        $("#presidentInfoContent").empty();
        if (kindergartenId == null) {
            alert("信息缺失!");
            return;
        }
        $("#kinId").val(kindergartenId);

        url = "/api/PresidentInfo/";
        showDiv();
        callApi(url, post, { KindergartenId: kindergartenId }).done(function (data) {

            if (data.Attach.President == null || data.Attach.President.length <= 0) {
                alert("暂时还没有园长!");
                hideDiv();
                return;
            }

            var presidentInfos = [];
            var presidentNum = 1;
            for (var presidentIndex in data.Attach.President)
            {
                var president = data.Attach.President[presidentIndex];
                var item = {
                    num: presidentNum,
                    RealName: president.RealName,
                    LoginName: president.LoginName,
                    UserId: president.UserId,
                    Phone: president.Phone,
                };
                presidentInfos.push(item);
                presidentNum++;
            }
            var htmlPresident = handlebarsHelp("#presidentInfo-template", presidentInfos);
            $("#presidentInfoContent").append(htmlPresident);
            hideDiv();
        }).fail(function (dataClass) {
            hideDiv();
            console.log(data.message);
        });
    };

    var url = "/api/User/";
    var get = "GET";
    var post = "POST";

    return {
        //user.html     
        GenerateKindergartenAtNavigationBar: function (type) {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            //use in the user.html 
            var deferred = $.Deferred();

            //var paraUserInfoKinIdVal = $.cookie("paraUserInfoKinId");
            //var paraUserInfoKinNameVal = $.cookie("paraUserInfoKinName");

            var kindergartenId = $.cookie("KindergartenId");
            var classIds = $.cookie("ClassId");

            var roleVal = $.cookie("Role");
            var permissionType = getPermissionType();
            var userId = $.cookie("UserId");

            url = "/api/GenerateKindergartenInfo/";
            callApi(url, post, { kindergartenId: kindergartenId, permissionType: permissionType, userId: userId }).done(function (data) {

                var firstKinId = data.Attach.Kindergartens[0].KindergartenId;
                var firstKinName = data.Attach.Kindergartens[0].KindergartenName;

                var paraPresidentInfoKinIdVal = $.cookie("paraPresidentInfoKinId");
                var paraPresidentInfoKinNameVal = $.cookie("paraPresidentInfoKinName");

                var html = handlebarsHelp("#navPresidentKindergartens-template", data.Attach.Kindergartens);
                $("#navPresidentKindergartens").append(html);
                if (type == "one") {
                    if (paraPresidentInfoKinIdVal == null) {
                        $("#navPresidentKindergartens li:first").addClass("current");
                        $("#navPresidentKindergartens li:first").css({ "background": "#D9FFFF" });
                        $("#showKinNameInfo").text(firstKinName);
                        $.cookie("paraPresidentInfoKinId", firstKinId, { expires: 1, path: '/' });
                        $.cookie("paraPresidentInfoKinName", firstKinName, { expires: 1, path: '/' });

                        generatePresidentInfo(firstKinId, firstKinName);
                    } else {
                        $("#" + paraPresidentInfoKinIdVal).addClass("current");
                        $("#" + paraPresidentInfoKinIdVal).css({ "background": "#D9FFFF" });
                        $("#showKinNameInfo").text(paraPresidentInfoKinNameVal);
                        
                        generatePresidentInfo(paraPresidentInfoKinIdVal, paraPresidentInfoKinNameVal);
                    }
                }
            }).fail(function (data) {
                deferred.reject();
                console.log(data.message);
            });
            //return deferred.promise();
        },
        GetPresidentInfo: function (kinId, kinName) {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            var allLi = $("#navPresidentKindergartens li");
            for (var liIndex = 0; liIndex < allLi.length; liIndex++) {
                $("#" + allLi[liIndex].id).removeClass("current");
                $("#" + allLi[liIndex].id).css({ "background": "" });
            }
            $("#" + kinId).addClass("current");
            $("#" + kinId).css({ "background": "#D9FFFF" });
            generatePresidentInfo(kinId, kinName);
        },
        CreateKindergarten: function (kinName, city, description) {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            url = "/api/CreateKindergarten/";
            showDiv();
            callApi(url, post, {
                KindergartenName: kinName,
                City: city,
                Description: description
            }).done(function (data) {
               
                if (data.Attach == null) {
                    alert("发生未知错误，请联系您的管理员.");
                    hideDiv();
                    return;
                }
                if (data.Attach.status == "success") {
                    alert("添加成功!");
                    redirect("presidentInfo.html");
                } else if (data.Attach.status == "chongfu") {
                    $("#checkInformation").css({ "color": "#FF0000" });
                    $("#checkInformation").text("幼儿园已经存在了.");
                    hideDiv();
                    return;
                } else {
                    alert("发生未知错误，请联系您的管理员.");
                    hideDiv();
                    return;
                }
            }).fail(function (data) {
                hideDiv();
                deferred.reject();
                console.log(data.message);
            });
        },
        CreatePresientOnload: function () {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            var paraPresidentInfoKinIdVal = $.cookie("paraPresidentInfoKinId");
            var paraPresidentInfoKinNameVal = $.cookie("paraPresidentInfoKinName");
            if (paraPresidentInfoKinIdVal == null
                || paraPresidentInfoKinIdVal == ""
                || paraPresidentInfoKinNameVal == null
                || paraPresidentInfoKinNameVal == "") {
                alert("信息缺失！");
                return;
            }
            $("#kinName").val(paraPresidentInfoKinNameVal);
            $("#kinId").val(paraPresidentInfoKinIdVal);
            var permissionType = getPermissionType();
            if (permissionType != 0) {
                alert("权限不足");
                return;
            }
        },
        GetCreatePresidentInfo: function () {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            var kinderIdVal = $("#kinId").val();
            
            var presidentNameVal = $("#realName").val();
            var presidentLoginNameVal = $("#loginName").val();
            var presidentIdentityVal = $("#selectIdentity").val();
            var phonePresidentVal = $("#phonePresident").val();
            var passWord = "123456";
            if (presidentNameVal == null || presidentNameVal == ""
                || presidentLoginNameVal == null || presidentLoginNameVal == ""
                || presidentIdentityVal == null || presidentIdentityVal == ""
                || presidentIdentityVal == "-1" || phonePresidentVal == "" || phonePresidentVal == null) {
                alert("请填写完整信息！");
                return;
            }
            url = "/api/DB/";
            showDiv();
            callApi(url, post, { DBType: 4, LoginName: presidentLoginNameVal }).done(function (data) {
                
                if (data.Attach.CheckUserLoginName != 0) {
                    $("#checkInformation").css({ "color": "#FF0000" });
                    $("#checkInformation").text("用户名重复，请重新填写！");
                    $("#create_president_info").attr("disabled", true);
                } else {
                    url = "/api/CreatePresident/";
                    callApi(url, post, {
                        KindergartenId: kinderIdVal,
                        RealName: presidentNameVal,
                        Password: passWord,
                        LoginName: presidentLoginNameVal,
                        RoleType: presidentIdentityVal,
                        Phone: phonePresidentVal
                    }).done(function (data) {
                        if (data.Attach.Status == "success") {
                            alert("添加成功！");
                            $("#create_president_info").attr("disabled", true);
                            redirect("presidentInfo.html");
                        } else {
                            alert("添加失败！");
                            hideDiv();
                            return;
                        }
                    }).fail(function (data) {
                        hideDiv();
                        alert(data.message);
                    })
                }
            }).fail(function (data) {
                hideDiv();
                alert(data.message);
            })
        },
        deletePresident: function (preId) {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            url = "/api/DelPresidentInfo/";
            showDiv();
            callApi(url, post, { type: 1, mubiaoId: preId }).done(function (data) {
                
                if (data == null || data.Attach == null) {
                    alert("发生未知错误，请联系您的管理员.");
                    redirect("presidentInfo.html");
                    return;
                }
                if (data.Attach.status == "success") {
                    alert("删除成功!");
                    redirect("presidentInfo.html");
                } else if (data.Attach.status == "fail") {
                    alert("删除失败!");
                    redirect("presidentInfo.html");
                } else {
                    alert("发生未知错误，请联系您的管理员.");
                    redirect("presidentInfo.html");
                    return;
                }
                hideDiv();
            }).fail(function (data) {
                hideDiv();
                alert("异常：" + data.message);
            });
        },
        GetEditKindergarten: function () {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            var editKindId = $.cookie("paraPresidentInfoKinId");
            var kindNameVal = $("#kinName").val();
            var kindCityVal = $("#city").val();
            var kindDescriptionVal = $("#description").val();

            if (kindNameVal == null || kindNameVal == ""
                || kindCityVal == null || kindCityVal == "") {
                alert("请填写完整信息！");
                return;
            }
            url = "/api/EditKindergarten/";
            showDiv();
            callApi(url, post, { ActionType: 2, KindergartenName: kindNameVal, KindergartenId: editKindId, DescriptionL: kindDescriptionVal }).done(function (data) {
                if (data.Attach.CheckKind != 0) {
                    $("#checkInformation").css({ "color": "#FF0000" });
                    $("#checkInformation").text("幼儿园已经存在了...");
                    $("#edit_kindergarten_info").attr("disabled", true);
                }else{
                    url = "/api/EditKindergarten/";
                    callApi(url, post, { ActionType: 1, KindergartenName: kindNameVal, City: kindCityVal, Description: kindDescriptionVal, KindergartenId: editKindId }).done(function (data) {
                        if (data.Attach.Status == "success") {
                            alert("修改成功！");
                            $("#edit_kindergarten_info").attr("disabled", true);
                            $.cookie("paraPresidentInfoKinName", kindNameVal, { expires: 1, path: '/' });
                            redirect("presidentInfo.html");
                        } else {
                            alert("修改失败，请与管理员联系！");
                        }
                        hideDiv();
                    }).fail(function (data) {
                        hideDiv();
                        alert(data.message);
                    })
                }
            }).fail(function (data) {
                hideDiv();
                alert(data.message);
            })
        },
        GetCheckKindInfo: function () {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            var kindNameVal = $("#kinName").val();
            var editKindId = $.cookie("paraPresidentInfoKinId");
            if (kindNameVal == null || kindNameVal == "") {
                $("#checkInformation").css({ "color": "#FF0000" });
                $("#checkInformation").text("请填写幼儿园名称...");
                return;
            }
            url = "/api/EditKindergarten/";
            showDiv();
            callApi(url, post, { ActionType: 2, KindergartenName: kindNameVal, KindergartenId: editKindId }).done(function (data) {
                if (data.Attach.CheckKind != 0) {
                    $("#checkInformation").css({ "color": "#FF0000" });
                    $("#checkInformation").text("幼儿园已经存在了...");
                    $("#edit_kindergarten_info").attr("disabled", true);
                } else {
                    $("#checkInformation").css({ "color": "#0066CC" });
                    $("#checkInformation").text("幼儿园名称可用！");
                    $("#edit_kindergarten_info").attr("disabled", false);
                }
                hideDiv();
            }).fail(function (data) {
                hideDiv();
                alert(data.message);
            })
        },
        GenerKinderInfo: function () {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            var editKindId = $.cookie("paraPresidentInfoKinId");
            var editKindName = $.cookie("paraPresidentInfoKinName");
            $("#kinName").val(editKindName);
            url = "/api/EditKindergarten/";
            showDiv();
            callApi(url, post, { ActionType: 0, KindergartenId: editKindId }).done(function (data) {
                var html = handlebarsHelp("#editKinderId", data.Attach.KindergarInfo[0]);
                $("#createKindergarten").append(html);
                hideDiv();
            }).fail(function (data) {
                hideDiv();
                alert(data.message);
            })
        },
        deleteKindergarten: function (kinId) {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            url = "/api/DelPresidentInfo/";
            showDiv();
            callApi(url, post, { type: 3, mubiaoId: kinId }).done(function (data) {
                
                if (data == null || data.Attach == null) {
                    alert("发生未知错误，请联系您的管理员.");
                    redirect("presidentInfo.html");
                    return;
                }
                if (data.Attach.status == "success") {
                    alert("删除成功!");
                    redirect("presidentInfo.html");
                } else if (data.Attach.status == "fail") {
                    alert("删除失败!");
                    redirect("presidentInfo.html");
                } else {
                    alert("发生未知错误，请联系您的管理员.");
                    redirect("presidentInfo.html");
                    return;
                }
                hideDiv();
            }).fail(function (data) {
                hideDiv();
                alert("异常：" + data.message);
            });
        },

        
    };
}();