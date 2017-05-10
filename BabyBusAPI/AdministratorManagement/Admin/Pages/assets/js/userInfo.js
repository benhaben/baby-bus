/*
 * Core script to handle all login specific things
 */

var UserInfo = function () {

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
            //password: data.password,selectTypeHtml
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
    var getClassTypeDes = function (classType) {
        if (classType == 0) {
            return '大班';
        }
        if (classType == 1) {
            return '中班';
        }
        if (classType == 2) {
            return '小班';
        }
        if (classType == 3) {
            return '托班';
        }
        if (classType == 4) {
            return '其他';
        }
        if (classType == -1) {
            return '删除班';
        }
        return '-';
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
        if (permissionType == 0 || permissionType == 3 || permissionType == 4) {
            isShow = true;
        }
        return isShow;
    };

    var generateClsAndChildInfo = function (kinId, kinName) {
        $("#test").text("kinId" + kinId);
        $("#kinName").text(kinName);
        $("#kinId").val(kinId);
        var roleVal = $.cookie("Role");
        var roleType;
        if (roleVal == "Administrator") {
            $("#classTypeTab").css({ "display": "block" });
            $("#parentClassTypeTab").attr({ "class": "tabbable tabbable-custom tabbable-full-width" });
            roleType = 0;
        } else if (roleVal == "President") {
            $("#classTypeTab").css({ "display": "block" });
            $("#parentClassTypeTab").attr({ "class": "tabbable tabbable-custom tabbable-full-width" });
            roleType = 1;
        } else if (roleVal == "Teacher") {
            roleType = 2;
        }
        var classIdVal = $.cookie("ClassId");

        var permissionType = getPermissionType();
        var isShow = isShowCreateEditBtn(permissionType);

        if (isShow) {
            $("#createClassesBtn").css({ "display": "block" });
        }
        url = "/api/User/";
        showDiv();
        var classTypeVal = $("#classTypeVal").val();
        var classTypeDes = getClassTypeDes(classTypeVal);
        
        callApi(url, post, {
            Role: roleType,
            KindergartenId: kinId,
            Type: 1,
            ClassId: classIdVal,
            ClassType: classTypeVal
        }).done(function (data) {
            $("#allClassContent").empty();
            if (data.Attach.classAndChildInfo.length <= 0) {
                alert("暂时没有 【" + classTypeDes + "】 信息!");
                hideDiv();
                return;
            }
            var payTypes = "";
            var classIndex = 1;
            for (var classIndex in data.Attach.classAndChildInfo) {
                var clsInfos = data.Attach.classAndChildInfo[classIndex].classInfo;

                if (roleType == 2) {
                    var html = handlebarsHelp("#deleteClass-template", clsInfos);
                    $("#allClassContent").append(html);
                } else {
                    if (clsInfos.classType == '-1') {
                        var html = handlebarsHelp("#deleteClass-template", clsInfos);
                        $("#allClassContent").append(html);
                    } else {
                        var html = handlebarsHelp("#class-template", clsInfos);
                        $("#allClassContent").append(html);
                        if (clsInfos.classType != 0) {
                            $("#changeGraduateBtn" + clsInfos.classId).css({ "display": "none" });
                        }
                    }
                }
                /*if (classIndex == 0) {
                    $("#childContent" + clsInfos.classId).css({ "display": "block" });
                    $("#types" + clsInfos.classId).text("▲");
                }*/
                if (isShow) {
                    $("#classContent1" + clsInfos.classId).css({ "display": "block" });
                    $("#classContent2" + clsInfos.classId).css({ "display": "none" });
                }
                var childInfos = data.Attach.classAndChildInfo[classIndex].childInfos;
                var childInfoSum = [];
                var childIndex = 1;
                for (var childInfosIndex in childInfos) {
                    var childOne = childInfos[childInfosIndex];
                    var partenName = "-";
                    var partenPhone = "-";
                    var childPay = childOne.childPayType;

                    if (childPay != null && childPay != "") {

                        if (childPay != null && childPay != "" && childPay.length >= 0) {
                            if (childPay[0].PayType == 0) {
                                payTypes = "未付费";
                            } else if (childPay[0].PayType == 1) {
                                payTypes = "已付费";
                            }
                        }
                    } else {
                        payTypes = "未付费";
                    }
                    if (childOne.parentInfo != null && childOne.parentInfo != '') {
                        var parent = childOne.parentInfo;
                        if (parent != null && parent != '' && parent.length >= 0) {
                            if (parent[0].RealName != null && parent[0].RealName != '') {
                                partenName = parent[0].RealName;
                            }
                            if (parent[0].LoginName != null && parent[0].LoginName != '') {
                                //partenPhone = parent[0].LoginName;
                                partenPhone = parent[0].Phone;
                            }
                        }
                    }
                    if (childOne.image == null || childOne.image == "") {
                        var imageUrl = "images/defaultChildImage.png";
                    } else {
                        var imageUrl = "http:\/\/babybus.emolbase.com\/" + childOne.image;
                    }
                    var childInfoSumItm = {
                        childId: childOne.childId,
                        childName: childOne.childName,
                        classId: childOne.classId,
                        kinId: childOne.kinId,
                        childIndex: childIndex,
                        partenName: partenName,
                        partenPhone: partenPhone,
                        payType: payTypes,
                        imgUrl: imageUrl,
                    };
                    var childsHtml = handlebarsHelp("#childs-template", childInfoSumItm);
                    $("#childContent" + clsInfos.classId).append(childsHtml);

                    if (payTypes == "已付费") {
                        $("#pay" + childOne.childId).attr("class", "ribbon green");
                    } else {
                        $("#pay" + childOne.childId).attr("class", "ribbon red");
                    }
                    childIndex++;
                }

                /*var childsHtml = handlebarsHelp("#childs-template", childInfoSum);
                $("#childContent" + clsInfos.classId).append(childsHtml);*/
                classIndex++;
            }//loop data.Attach.classAndChildInfo end
            hideDiv();
        }).fail(function (data) {
            hideDiv();
            deferred.reject();
            console.log(data.message);
        });
    };

    var generateChildInfo = function (kindergartenId, kinName, allKinInfo) {

        var classId = $.cookie("paraUserInfoClsId");
        var childId = $.cookie("paraUserInfoChildId");
        var imageUrlVal = "images/defaultChildImage.png";

        if (classId == null || kindergartenId == null || childId == null) {
            alert("信息缺失！");
            return;
        }
        var permissionType = getPermissionType();
        var isShow = isShowCreateEditBtn(permissionType);
        if (isShow) {
            $("#editeChild").css({ "display": "block" });
            $("#createUsers").css({ "display": "block" });
            $("#delete" + childId).css({ "display": "block" });
        }

        var roleVal = $.cookie("Role");
        var permissionType = getPermissionType();
        //幼儿园信息
        if (permissionType == 0) {
            var html = handlebarsHelp("#selectKindergarHtml", allKinInfo);
            $("#kinderDisplayRole").append(html);
            $("#kindergarRolP").css({ "display": "none" });
            $("#kinderRolA").css({ "display": "block" });
        } else if (permissionType == 3 || permissionType == 4) {
            $("#kindergarRolP").css({ "display": "block" });
            $("#kinderRolA").css({ "display": "none" });
            $("#kinName").val(kinName);
            $("#kinId").val(kindergartenId);
        }

        //班级信息
        url = "/api/DB/";
        callApi(url, post, { DBType: 2, KindergartenId: kindergartenId }).done(function (data) {
            var selectClsHtml = handlebarsHelp("#selectTypeHtml", data.Attach.ClassName);
            $("#selectClsInfo").empty();
            $("#selectClsInfo").append(selectClsHtml);
        }).fail(function (data) {
            alert(data.message);
        });


        url = "/api/User/";
        showDiv();
        callApi(url, post, { Type: 2, KindergartenId: kindergartenId, ClassId: classId, ChildId: childId }).done(function (dataClass) {

            if (dataClass.Attach == null || dataClass.Attach.ChildInfo.length <= 0) {
                alert("宝贝信息有误!");
                hideDiv();
                return;
            }

            var child = dataClass.Attach.ChildInfo[0];
            if (child.ImageName != "" && child.ImageName != null) {
                imageUrlVal = "http:\/\/babybus.emolbase.com\/" + child.ImageName;
            }
            var image = {
                imagUrl: imageUrlVal
            }
            var childimages = handlebarsHelp("#imagesHtml", image);
            $("#childInfo2").append(childimages);

            var genderVal = "";
            if (child.Gender == 2) {
                genderVal = "女";
            } else if (child.Gender == 1) {
                genderVal = "男";
            } else {
                genderVal = "-";
            }

            var childInfo =
            {
                ChildId: child.ChildId,
                ChildName: child.ChildName,
                Gender: genderVal,
                KindergartenName: child.KindergartenName,
                ClassName: child.ClassName
            };
            var htmlDeleteChild = handlebarsHelp("#delectChildId", child);
            $("#delete_child_id").append(htmlDeleteChild);
            //childInfo
            var htmlChildren = handlebarsHelp("#children-userinfo-template", childInfo);
            $("#childInfo2").append(htmlChildren);

            var htmlPresident = handlebarsHelp("#president-userinfo-template", dataClass.Attach.President);
            $("#presidentInfo").append(htmlPresident);

            var htmlParent = handlebarsHelp("#parents-userinfo-template", dataClass.Attach.Parent);
            $("#parentInfo2").append(htmlParent);

            var htmlTeacher = handlebarsHelp("#teachers-userinfo-template", dataClass.Attach.Teacher);
            $("#childTeacherInfo").append(htmlTeacher);

            $("#childName").val(child.ChildName);
            $("#childNameHeader").text(child.ChildName);

            //var parents = dataClass.Attach.Parent[0];
            for (var parentIndex in dataClass.Attach.Parent) {
                var parent = dataClass.Attach.Parent[parentIndex];
                if (isShow) {
                    $("#editParent" + parent.UserId).css({ "display": "block" });
                }
            }
            $("#Option" + child.KindergartenId).attr("selected", "selected");
            $("#" + child.ClassId).attr("selected", "selected");
            $("input[name = 'render'][value = " + child.Gender + "]").attr("checked", true);


            //宝宝幼儿园默认值
            if (isShow) {
                $("#delete" + childId).css({ "display": "block" });
            }
            hideDiv();
        }).fail(function (dataClass) {
            hideDiv();
            console.log(data.message);
        });
    };

    var GenerateEditClassTeacherInfo = function (kindergartenId, kindergartenName) {

        var classId = $.cookie("paraShowTeacherInfoClsId");
        var className = $.cookie("paraShowTeacherInfoClsName");

        if (classId == null || kindergartenId == null) {
            alert("权限不足！");
            return;
        }
        $("#editTeacherInfoKinName").text(kindergartenName);
        $("#classNameHeader").text(className);
        var permissionType = getPermissionType();
        var isShow = isShowCreateEditBtn(permissionType);
        if (!isShow) {
            alert("权限不足");
            return;
        }
        url = "/api/User/";
        showDiv();
        callApi(url, post, { Type: 3, KindergartenId: kindergartenId, ClassId: classId }).done(function (dataClass) {

            if (dataClass.Attach == null || dataClass.Attach.TeacherInfo.length <= 0) {
                hideDiv();
                alert("未查询到教师信息!");
                return;
            }
            var teacherInfos = [];
            var teacherNum = 1;
            for (var teacherIndex in dataClass.Attach.TeacherInfo) {
                var teacher = dataClass.Attach.TeacherInfo[teacherIndex];

                var item = {
                    num: teacherNum,
                    RealName: teacher.RealName,
                    LoginName: teacher.Phone,
                    UserId: teacher.UserId,
                    KindergartenId: teacher.KindergartenId,
                    ClassId: teacher.ClassId,
                };
                teacherInfos.push(item);
                teacherNum++;
            }
            var htmlTeacher = handlebarsHelp("#showEditTeacherInfoHtml", teacherInfos);
            $("#showEditTeacherInfo").append(htmlTeacher);
            hideDiv();
        }).fail(function (dataClass) {
            hideDiv();
            console.log(data.message);
        });
    }

    var checkUserData = function (dataRoleType, userDataId) {
        url = "/api/DB/";
        showDiv();
        callApi(url, post, { DBType: 12, RoleType: dataRoleType, UserId: userDataId }).done(function (data) {

            if (dataRoleType == 1) {
                for (var jos in data.Attach.CheckParents) {
                    var checkParents = data.Attach.CheckParents[jos];
                    var text = "【" + checkParents.KindergartenName + "】" + checkParents.ClassName + checkParents.ChildName + "的家长已用";
                    $("#checkInformation").text(text);
                    $("#checkInformation").css({ "color": "#FF0000" });
                }
            } else if (dataRoleType == 2) {
                for (var jos in data.Attach.CheckTeachers) {
                    var checkTeachers = data.Attach.CheckTeachers[jos];
                    var textTeacher = "【" + checkTeachers.KindergartenName + "】" + checkTeachers.ClassName + "的" + checkTeachers.RealName + "已用";
                    $("#checkInformation").text(textTeacher);
                    $("#checkInformation").css({ "color": "#FF0000" });
                }
            } else if (dataRoleType == 3) {
                for (var jos in data.Attach.CheckTittlePresident) {
                    var checkTittlePresident = data.Attach.CheckTittlePresident[jos];
                    var textTittlePre = "【" + checkTittlePresident.KindergartenName + "】" + "的" + checkTittlePresident.RealName + "已用";
                    $("#checkInformation").text(textTittlePre);
                    $("#checkInformation").css({ "color": "#FF0000" });
                }
            } else if (dataRoleType == 4) {
                for (var jos in data.Attach.CheckPresidents) {
                    var checkPresidents = data.Attach.CheckPresidents[jos];
                    var textTittlePre = "【" + checkPresidents.KindergartenName + "】" + "的" + checkPresidents.RealName + "已用";
                    $("#checkInformation").text(textTittlePre);
                    $("#checkInformation").css({ "color": "#FF0000" });
                }
            }
            hideDiv();
        }).fail(function (data) {
            hideDiv();
            alert(data.message);
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

            var paraUserInfoKinIdVal = $.cookie("paraUserInfoKinId");
            var paraUserInfoKinNameVal = $.cookie("paraUserInfoKinName");

            var kindergartenId = $.cookie("KindergartenId");
            var classIds = $.cookie("ClassId");

            var roleVal = $.cookie("Role");
            var permissionType = getPermissionType();
            var userId = $.cookie("UserId");

            url = "/api/GenerateKindergartenInfo/";
            callApi(url, post, { kindergartenId: kindergartenId, permissionType: permissionType, userId: userId }).done(function (data) {
                var firstKinId = data.Attach.Kindergartens[0].KindergartenId;
                var firstKinName = data.Attach.Kindergartens[0].KindergartenName;

                var html = handlebarsHelp("#navKindergartens-template", data.Attach.Kindergartens);
                $("#navKindergartens").append(html);
                if (type == "one") {
                    if (paraUserInfoKinIdVal == null) {
                        $("#navKindergartens li:first").addClass("current");
                        $("#navKindergartens li:first").css({ "background": "#D9FFFF" });
                        $("#showKinNameInfo").text(firstKinName);
                        $.cookie("paraUserInfoKinId", firstKinId, { expires: 1, path: '/' });
                        $.cookie("paraUserInfoKinName", firstKinName, { expires: 1, path: '/' });
                        generateClsAndChildInfo(firstKinId, firstKinName);
                    } else {
                        $("#" + paraUserInfoKinIdVal).addClass("current");
                        $("#" + paraUserInfoKinIdVal).css({ "background": "#D9FFFF" });
                        $("#showKinNameInfo").text(paraUserInfoKinNameVal);
                        generateClsAndChildInfo(paraUserInfoKinIdVal, paraUserInfoKinNameVal);
                    }
                } else if (type == "two") {
                    $("#" + paraUserInfoKinIdVal).addClass("current");
                    $("#" + paraUserInfoKinIdVal).css({ "background": "#D9FFFF" });
                    $("#showKinNameInfo").text(paraUserInfoKinNameVal);
                    generateChildInfo(paraUserInfoKinIdVal, paraUserInfoKinNameVal, data.Attach.Kindergartens);
                } else if (type == "three") {
                    $("#" + paraUserInfoKinIdVal).addClass("current");
                    $("#" + paraUserInfoKinIdVal).css({ "background": "#D9FFFF" });
                    $("#showKinNameInfo").text(paraUserInfoKinNameVal);
                    GenerateEditClassTeacherInfo(paraUserInfoKinIdVal, paraUserInfoKinNameVal);
                }
            }).fail(function (data) {
                deferred.reject();
                console.log(data.message);
            });
            //return deferred.promise();
        },
        GetClassAndChildInfo: function (kinId, kinName) {

            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            var allLi = $("#navKindergartens li");
            for (var liIndex = 0; liIndex < allLi.length; liIndex++) {
                $("#" + allLi[liIndex].id).removeClass("current");
                $("#" + allLi[liIndex].id).css({ "background": "" });
            }
            $("#" + kinId).addClass("current");
            $("#" + kinId).css({ "background": "#D9FFFF" });
            generateClsAndChildInfo(kinId, kinName);
        },//userInfo.html
        GetClassAndChildInfoByClassType: function () {

            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }
            generateClsAndChildInfo($.cookie("paraUserInfoKinId"), $.cookie("paraUserInfoKinName"));
        },//userInfo.html
        GetUpdateInfo: function () {

            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            url = "/api/DB/";
            var childId = $.cookie("paraUserInfoChildId");
            var classId = $.cookie("paraUserInfoClsId");
            var childName = $("#childName").val();
            var className = $("#className").val();
            var classIdChoos = $("#selectClassName").val();
            var kinId = "-1";
            var genderVal = $("input[name = 'render']:checked").val();
            var permissionType = getPermissionType();
            if (permissionType == 0) {
                kinId = $("#selectKinderName option:selected").val();
            } else if (permissionType == 3 || permissionType == 4) {
                kinId = $("#kinId").val();
            }


            if (kinId == null || kinId == "-1" || kinId == "" || classIdChoos == null || classIdChoos == "" || classIdChoos == "-1") {
                alert("请填写完整信息！");
                return;
            }

            $("#edit_user_info").attr("disabled", true);
            if (childName != null && classIdChoos != -1) {
                showDiv();
                callApi(url, post, {

                    DBType: 1,
                    ChildId: childId,
                    ClassId: classId,
                    ClassName: className,
                    ChildName: childName,
                    ClassIdChoos: classIdChoos,
                    KindergartenId: kinId,
                    Gender: genderVal
                }).done(function (data) {
                    if (data.Attach.Success == "success") {
                        alert("修改成功！");
                        $.cookie("paraUserInfoClsId", classIdChoos, { expires: 1, path: '/' });
                        $.cookie("paraUserInfoChildId", childId, { expires: 1, path: '/' });
                        Utilities.Redirect("user_Info.html");
                    } else {
                        alert("修改失败！");
                        $("#edit_user_info").removeAttr("disabled");
                    }
                    hideDiv();
                }).fail(function (data) {
                    hideDiv();
                    alert(data.message);
                    $("#edit_user_info").removeAttr("disabled");
                });
            } else {
                hideDiv();
                alert("更改失败");
                $("#edit_user_info").removeAttr("disabled");
            }

        },
        GetCreateInfo: function () {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            var childName = $("#childName").val();
            var kindergartenId = $("#kinId").val();
            var classIdChoos = $("#clsId").val();
            var kinName = $("#kinName").val();
            var gender = $("input[name='render']:checked").val();
            var birthday = $("#dateVal").val();

            if (childName == null
                || childName == ""
                || kindergartenId == null
                || kindergartenId == ""
                || classIdChoos == null
                || classIdChoos == ""
                || classIdChoos == -1
                || birthday == null
                || birthday == '') {
                alert("信息填写不完整!");
                return;
            }
            url = "/api/DB/";
            $("#edit_user_info").attr("disabled", true);
            showDiv();
            callApi(url, post, {
                DBType: 0,
                ChildName: childName,
                KindergartenId: kindergartenId,
                ClassIdChoos: classIdChoos,
                Gender: gender,
                Birthday: birthday
            }).done(function (data) {
                if (data.Attach.States == "Success") {
                    $.cookie("paraUserInfoKinId", kindergartenId, { expires: 1, path: '/' });
                    $.cookie("paraUserInfoKinName", kinName, { expires: 1, path: '/' });
                    $.cookie("paraUserInfoClsId", classIdChoos, { expires: 1, path: '/' });
                    $.cookie("paraUserInfoChildId", data.Attach.ChildIdS, { expires: 1, path: '/' });
                    alert("添加成功！");
                    Utilities.Redirect("user_info.html");
                } else {
                    $("#edit_user_info").removeAttr("disabled");
                    alert("添加失败！");
                }
                hideDiv();
            }).fail(function (data) {
                hideDiv();
                alert(data.message);
            });
        },

        GetParentUserInfo: function () {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }
            var realNameVal = $.cookie("editParentRealName");
            var loginNameVal = $.cookie("editParentLoginName");
            var phonePareVal = $.cookie("editParentPhone");
            $("#phoneParent").val(phonePareVal);
            $("#realName").val(realNameVal);
            $("#loginName").val(loginNameVal);
            if (getPermissionType() == 0 || getPermissionType() == 3 || getPermissionType() == 4) {
                $("#loginTabId").css({ "display": "block" });
            }
        },
        GetCheckInfo: function () {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            var loginNameVal = $("#loginName").val();
            $("#create_user_info").attr("disabled", false);
            $("#create_president_info").attr("disabled", false);
            url = "/api/DB/";
            if (loginNameVal == null || loginNameVal == "") {
                $("#checkInformation").css({ "color": "#FF0000" });
                $("#checkInformation").text("请填写用户名!");
                $("#create_user_info").attr("disabled", true);
                $("#create_president_info").attr("disabled", true);
                $("#create_teacher_user_info").attr("disabled", true);
                return;
            }
            showDiv();
            callApi(url, post, { DBType: 4, LoginName: loginNameVal }).done(function(data) {
                if (data.Attach.CheckUserLoginName != 0) {
                    $("#create_user_info").attr("disabled", true);
                    $("#create_president_info").attr("disabled", true);
                    $("#create_teacher_user_info").attr("disabled", true);
                    var checkParentData = data.Attach.CheckParentData[0];
                    var checkParentRoleType = checkParentData.RoleType;
                    var checkParentUserId = checkParentData.UserId;
                    checkUserData(checkParentRoleType, checkParentUserId);
                } else {
                    $("#create_user_info").removeAttr("disabled");
                    $("#checkInformation").css({ "color": "blue" });
                    $("#checkInformation").text("用户名可用！");
                    $("#create_president_info").attr("disabled", false);
                    $("#create_teacher_user_info").attr("disabled", false);
                }
                hideDiv();
            }).fail(function(data) {
                hideDiv();
                alert(data.message);
            });
        },

        GetCheckParentsInfo: function () {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            var userParentIdVal = $.cookie("editParentId");
            var userParentLoginNameVal = $("#loginName").val();
            $("#edit_parent_user_info").attr("disabled", false);
            if (userParentLoginNameVal == null || userParentLoginNameVal == "") {
                $("#checkInformation").css({ "color": "#FF0000" });
                $("#checkInformation").text("请填写用户名!");
                $("#edit_parent_user_info").attr("disabled", true);
                return;
            }
            url = "/api/DB/";
            showDiv();
            callApi(url, post, { DBType: 6, UserId: userParentIdVal, LoginName: userParentLoginNameVal }).done(function (data) {
                if (data.Attach.CheckUpdateParent != 0) {
                    $("#edit_parent_user_info").attr("disabled", true);
                    var checkLoginName = data.Attach.CheckParentData[0];
                    var roleTypeVal = checkLoginName.RoleType;
                    var userParentDataValId = checkLoginName.UserId;
                    checkUserData(roleTypeVal, userParentDataValId);
                } else {
                    $("#edit_parent_user_info").removeAttr("disabled");
                    $("#checkInformation").css({ "color": "blue" });
                    $("#checkInformation").text("用户名可用可用！");
                }
                hideDiv();
            }).fail(function (data) {
                hideDiv();
                alert(data.message);
            });
        },

        GetCheckUpdateTeacherInfo: function () {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            var teacherLoginNameVal = $("#loginName").val();
            var teacherUserIdVal = $.cookie("paraUpdateTeacherId");
            if (teacherLoginNameVal == null || teacherLoginNameVal == "") {
                $("#checkInformation").css({ "color": "#FF0000" });
                $("#checkInformation").text("请填写用户名!");
                $("#edit_teacher_user_info").attr("disabled", true);
                return;
            }
            url = "/api/DB/";
            showDiv();
            callApi(url, post, { DBType: 10, LoginName: teacherLoginNameVal, UserId: teacherUserIdVal }).done(function (data) {
                if (data.Attach.CheckUpdateTeacher != 0) {
                    $("#edit_teacher_user_info").attr("disabled", true);
                    var teacherData = data.Attach.TeacherDatas[0];
                    var teacherUserRoleType = teacherData.RoleType;
                    var teacherUserId = teacherData.UserId;
                    checkUserData(teacherUserRoleType, teacherUserId);
                } else {
                    $("#edit_teacher_user_info").removeAttr("disabled");
                    $("#checkInformation").css({ "color": "blue" });
                    $("#checkInformation").text("用户名可用！");
                }
                hideDiv();
            }).fail(function (data) {
                hideDiv();
                alert(data.message);
            });

        },
        GetCreateUserInfo: function () {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            var roleVal = $.cookie("Role");
            var passWordVal = "123456";
            var realNameVal = $("#realName").val();
            var loginNameVal = $("#loginName").val();
            var kindergartenIdVal = $.cookie("paraUserInfoKinId");
            var classIdVal = $.cookie("paraUserInfoClsId");
            var childIdVal = $.cookie("paraUserInfoChildId");
            var phoneTeacherVal = $("#phoneParentsVal").val();
            if (realNameVal == null || realNameVal == ""
                || loginNameVal == null || loginNameVal == ""
                || kindergartenIdVal == null || kindergartenIdVal == ""
                || classIdVal == null || classIdVal == ""
                || childIdVal == null || childIdVal == ""
                || phoneTeacherVal == null || phoneTeacherVal == "") {
                alert("信息填写不完整!");
                return;
            }
            url = "/api/DB/";
            showDiv();
            callApi(url, post, { DBType: 4, LoginName: loginNameVal }).done(function (data) {
                if (data.Attach.CheckUserLoginName != 0) {
                    var checkParentData = data.Attach.CheckParentData[0];
                    var checkParentRoleType = checkParentData.RoleType;
                    var checkParentUserId = checkParentData.UserId[0];
                    checkUserData(checkParentRoleType, checkParentUserId);
                    hideDiv();
                    return;
                } else {
                    url = "/api/DB/";
                    callApi(url, post, {
                        DBType: 3,
                        RealName: realNameVal,
                        LoginName: loginNameVal,
                        KindergartenId: kindergartenIdVal,
                        ClassId: classIdVal,
                        ChildId: childIdVal,
                        Password: passWordVal,
                        Phone: phoneTeacherVal
                    }).done(function (data) {
                        if (data.Attach.Status == "success") {
                            alert("添加成功！");
                            redirect("user_info.html");
                        }
                        else {
                            alert("添加失败！");
                        }
                        hideDiv();
                    }).fail(function (data) {
                        hideDiv();
                        alert(data.message);
                    });
                }
            }).fail(function (data) {
                hideDiv();
                alert(data.message);
            });
        },

        GetCreateClasses: function () {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }
            var kindergartenId = $.cookie("paraUserInfoKinId");
            var className = $("#className").val();
            var classType = $("#classType").val();
            if (className == null || className == "" || kindergartenId == null || classType == null) {
                alert("请填写完整信息！");
                return;
            }
            url = "/api/UpdataData/";
            showDiv();
            callApi(url, post, {
                Types: 1,
                KindergartenId: kindergartenId,
                ClassName: className
            }).done(function (data) {
                if (data.Attach.Classes != 0) {
                    $("#checkInformation").css({ "color": "#FF0000" });
                    $("#checkInformation").text("班级重复，请重新填写!");
                    $("#create_classes_info").attr("disabled", true);
                    hideDiv();
                    return;
                } else {
                    callApi(url, post, {
                        Types: 0,
                        KindergartenId: kindergartenId,
                        ClassName: className,
                        ClassType: classType
                    }).done(function (data) {
                        if (data.Attach.Status == "Success") {
                            alert("添加成功！");
                            $.cookie("addClassInfo_classType", classType, { expires: 1, path: '/' });
                            redirect("user.html");
                        } else {
                            alert("添加失败！");
                        }
                        hideDiv();
                    }).fail(function(data) {
                        hideDiv();
                        data.message(data);
                    });
                }
            }).fail(function(data) {
                hideDiv();
                data.message(data);
            });

        },
        GetUpdateParentUserInfo: function () {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }
            var parentUserIdVal = $.cookie("editParentId");
            var phoneParentVal = $("#phoneParent").val();
            var loginNameVal = $("#loginName").val();
            var realNameVal = $("#realName").val();
            var roleVal = getPermissionType();
            $("#edit_parent_user_info").attr("disabled", false);
            if (roleVal == 0 || roleVal == 3 || roleVal == 4) {
                if (realNameVal == null || realNameVal == ""
                    || phoneParentVal == null || phoneParentVal == ""
                    || loginNameVal == null || loginNameVal == "") {
                    alert("请填写完整信息！");
                    $("#edit_parent_user_info").attr("disabled", true);
                    return;
                }
                url = "/api/DB/";
                showDiv();
                callApi(url, post, { DBType: 6, UserId: parentUserIdVal, LoginName: loginNameVal}).done(function(data) {
                    if (data.Attach.CheckUpdateParent != 0) {
                        var checkLoginName = data.Attach.CheckParentData[0];
                        var roleTypeVal = checkLoginName.RoleType;
                        var userParentDataValId = checkLoginName.UserId;
                        checkUserData(roleTypeVal, userParentDataValId);
                        hideDiv();
                        return;
                    } else {
                        url = "/api/DB/";
                        showDiv();
                        callApi(url, post, { DBType: 5, UserId: parentUserIdVal, LoginName: loginNameVal, Phone: phoneParentVal, RealName: realNameVal, RoleType: roleVal }).done(function (data) {
                            if (data.Attach.Status == "success") {
                                alert("修改成功！");
                                $("#edit_parent_user_info").attr("disabled", true);
                                redirect("user_info.html");
                            } else {
                                alert("修改失败！");
                                $("#edit_parent_user_info").attr("disabled", false);
                            }
                            hideDiv();
                        }).fail(function(data) {
                            alert(data.message);
                        });
                    }
                    hideDiv();
                }).fail(function(data) {
                    hideDiv();
                    alert(data.message);
                });
            } else {
                if (realNameVal == null || realNameVal == ""
                   || phoneParentVal == null || phoneParentVal == "") {
                    alert("请填写完整信息！");
                    $("#edit_parent_user_info").attr("disabled", true);
                    return;
                }
                url = "/api/DB/";
                showDiv();
                callApi(url, post, { DBType: 5, UserId: parentUserIdVal, LoginName: loginNameVal, Phone: phoneParentVal, RealName: realNameVal,RoleType:roleVal }).done(function (data) {
                    if (data.Attach.Status == "success") {
                        alert("修改成功！");
                        $("#edit_parent_user_info").attr("disabled", true);
                        redirect("user_info.html");
                    } else {
                        alert("修改失败！");
                        $("#edit_parent_user_info").attr("disabled", false);
                    }
                    hideDiv();
                }).fail(function (data) {
                    alert(data.message);
                });
            }

            // var kindergarten = $.cookie("paraUserInfoKinId");
        },

        GetUpdateTeachUserInfo: function () {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }
            var roleVal = getPermissionType();
            var teacherLoginNameVal = $("#loginName").val();
            var phoneVal = $("#phoneTeacher").val();
            var teacherRealNameVal = $("#realName").val();
            var teacherClassChooseVal = $("#selectClassName").val();
            var teacherUserIdVal = $.cookie("paraUpdateTeacherId");
            if (roleVal == 0 || roleVal == 3 || roleVal == 4) {
                if (phoneVal == "" || phoneVal == null
                    || teacherRealNameVal == "" || teacherRealNameVal == null
                    || teacherClassChooseVal == "" || teacherClassChooseVal == null
                    || teacherLoginNameVal == "" || teacherLoginNameVal == null) {
                    alert("请填写完整信息！");
                    return;
                }
                url = "/api/DB/";
                showDiv();
                callApi(url, post, { DBType: 10, LoginName: teacherLoginNameVal, UserId: teacherUserIdVal }).done(function (data) {
                    if (data.Attach.CheckUpdateTeacher != 0) {
                        $("#edit_teacher_user_info").attr("disabled", true);
                        var teacherData = data.Attach.TeacherDatas[0];
                        var teacherUserRoleType = teacherData.RoleType;
                        var teacherUserId = teacherData.UserId;
                        checkUserData(teacherUserRoleType, teacherUserId);
                        hideDiv();
                        return;
                    } else {
                        url = "/api/DB/";
                        showDiv();
                        callApi(url, post, {
                            DBType: 9,
                            LoginName: teacherLoginNameVal,
                            Phone: phoneVal,
                            ClassIdChoos: teacherClassChooseVal,
                            RealName: teacherRealNameVal,
                            UserId: teacherUserIdVal,
                            RoleType:roleVal
                        }).done(function (data) {
                            if (data.Attach.Status == "success") {
                                alert("修改成功！");
                                $("#edit_teacher_user_info").attr("disabled", true);
                                redirect("showEditTeacherInfo.html");
                            } else {
                                alert("修改失败！");
                                $("#edit_teacher_user_info").attr("disabled", false);
                            }
                        }).fail(function (data) {
                            hideDiv();
                            alert(data.message);
                        });
                    }
                    hideDiv();
                }).fail(function (data) {
                    hideDiv();
                    alert(data.message);
                });
            } else {
                if (phoneVal == "" || phoneVal == null
                    || teacherRealNameVal == "" || teacherRealNameVal == null
                    || teacherClassChooseVal == "" || teacherClassChooseVal == null) {
                    alert("请填写完整信息！");
                    return;
                }
                url = "/api/DB/";
                showDiv();
                callApi(url, post, {
                    DBType: 9,
                    LoginName: teacherLoginNameVal,
                    Phone: phoneVal,
                    ClassIdChoos: teacherClassChooseVal,
                    RealName: teacherRealNameVal,
                    UserId: teacherUserIdVal,
                    RoleType: roleVal
                }).done(function (data) {
                    if (data.Attach.Status == "success") {
                        alert("修改成功！");
                        $("#edit_teacher_user_info").attr("disabled", true);
                        redirect("showEditTeacherInfo.html");
                    } else {
                        alert("修改失败！");
                        $("#edit_teacher_user_info").attr("disabled", false);
                    }
                    hideDiv();
                }).fail(function (data) {
                    hideDiv();
                    alert(data.message);
                });
            }

        },

        GetCreateTeachersInfo: function () {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }
            var passWord = "123456";
            var roleType = 2;
            var teacherUserKindIdVal = $.cookie("paraUserInfoKinId");
            var teacherUserLoginNameVal = $("#loginName").val();
            var teacherUserRealNameVal = $("#realName").val();
            var teacherUserClassIdChooseVal = $("#clsId").val();
            var phoneTeacherVal = $("#phoneTeacher").val();
            if (teacherUserLoginNameVal == null || teacherUserLoginNameVal == ""
                || teacherUserRealNameVal == null || teacherUserRealNameVal == ""
                || teacherUserClassIdChooseVal == "" || teacherUserClassIdChooseVal == "-1"
                || phoneTeacherVal == null || phoneTeacherVal == "") {
                alert("信息填写不完整!");
                $("#create_teacher_user_info").attr("disabled", false);
                return;
            }
            url = "/api/DB/";
            showDiv();
            callApi(url, post, { DBType: 4, LoginName: teacherUserLoginNameVal }).done(function (data) {
                if (data.Attach.CheckUserLoginName != 0) {
                    $("#create_teacher_user_info").attr("disabled", true);
                    var checkParentData = data.Attach.CheckParentData[0];
                    var checkParentRoleType = checkParentData.RoleType;
                    var checkParentUserId = checkParentData.UserId;
                    checkUserData(checkParentRoleType, checkParentUserId);
                    hideDiv();
                    return;
                } else {
                    url = "/api/DB/";
                    callApi(url, post, {
                        DBType: 7,
                        KindergartenId: teacherUserKindIdVal,
                        LoginName: teacherUserLoginNameVal,
                        RealName: teacherUserRealNameVal,
                        ClassIdChoos: teacherUserClassIdChooseVal,
                        Password: passWord,
                        RoleType: roleType,
                        Phone: phoneTeacherVal
                    }).done(function (data) {
                        if (data.Attach.Status == "success") {
                            alert("添加成功！");
                            $("#create_teacher_user_info").attr("disabled", true);
                            redirect("user.html");
                        } else {
                            alert("添加失败！");
                            $("#create_teacher_user_info").attr("disabled", false);
                        }

                    }).fail(function (data) {
                        hideDiv();
                        alert(data.message);
                    });
                }
                hideDiv();
            }).fail(function (data) {
                hideDiv();
                alert(data.message);
            });

        },

        GetTeacherInfo: function () {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            var teactherUserIdVal = $.cookie("paraUpdateTeacherId");
            var kindergartenIdVal = $.cookie("paraUserInfoKinId");
            var kindergartenName = $.cookie("paraUserInfoKinName");


            url = "/api/DB/";
            showDiv();
            callApi(url, post, { DBType: 11, UserId: teactherUserIdVal, KindergartenId: kindergartenIdVal }).done(function (data) {
                var teacherClassIds = handlebarsHelp("#selectTypeHtml", data.Attach.ClassIds);
                $("#selectClsInfo").append(teacherClassIds);
                var teacher = data.Attach.TeacherDatas[0];
                var teacherName = teacher.RealName;
                var teacheNunber = teacher.LoginName;
                var teacheClassId = teacher.ClassId;
                var teacherPhoneVal = teacher.Phone;
                $("#kinName").val(kindergartenName);
                $("#realName").val(teacherName);
                $("#loginName").val(teacheNunber);
                $("#selectClassName").val(teacheClassId);
                $("#phoneTeacher").val(teacherPhoneVal);
                if (getPermissionType() == 0 || getPermissionType == 3 || getPermissionType == 4) {
                    $("#loginTeacher").css({ "display": "block" });
                }
                hideDiv();
            }).fail(function (data) {
                hideDiv();
                alert(data.message);
            })
        },
        GetCheckClasses: function () {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;

            }
            var kindergartenId = $.cookie("paraUserInfoKinId");
            var className = $("#className").val();
            if (className == null || className == "") {
                $("#checkInformation").css({ "color": "#FF0000" });
                $("#checkInformation").text("请填写完整信息!");
                $("#create_classes_info").attr("disabled", true);
                return;
            }
            url = "/api/UpdataData/";
            showDiv();
            callApi(url, post, { Types: 1, KindergartenId: kindergartenId, ClassName: className }).done(function (data) {
                if (data.Attach.Classes != 0) {
                    $("#checkInformation").css({ "color": "#FF0000" });
                    $("#checkInformation").text("班级重复，请重新填写!");
                    $("#create_classes_info").attr("disabled", true);
                } else {
                    $("#checkInformation").css({ "color": "blue" });
                    $("#checkInformation").text("班级名称正确！");
                    $("#create_classes_info").attr("disabled", false);
                }
                hideDiv();
            }).fail(function (data) {
                hideDiv();
                alert("Exception:" + data.message);
            });
        },
        GetUpdataClasses: function () {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            var kindId = $.cookie("paraUserInfoKinId");
            var clsId = $.cookie("paraShowInfoClsId");
            var clsName = $("#edit_className").val();
            var clsType = $("#classType").val();
            if (clsName == null 
                || clsName == ""
                || clsType == null) {
                alert("请填写完整信息！");
                return;
            }
            url = "/api/UpdataData/";
            showDiv();
            callApi(url, post, {
                Types: 3,
                KindergartenId: kindId,
                ClassName: clsName,
                ClassId: clsId
            }).done(function (data) {
                if (data.Attach.UpdataCheckClass != 0) {
                    $("#checkInformation").css({ "color": "#FF0000" });
                    $("#checkInformation").text("班级名称重复，请重新填写!");
                    $("#edit_classes_info").attr("disabled", true);
                } else {
                    callApi(url, post, {
                        Types: 2,
                        KindergartenId: kindId,
                        ClassName: clsName,
                        ClassId: clsId,
                        ClassType: clsType
                    }).done(function (data) {
                        if (data.Attach.Status == "success") {
                            alert("修改成功！");
                            $("#edit_classes_info").attr("disabled", true);
                            $.cookie("addClassInfo_classType", clsType, { expires: 1, path: '/' });
                            redirect("user.html");
                        } else {
                            alert("修改失败！");
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

        GetUdataCheckClasses: function () {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            var kindId = $.cookie("paraUserInfoKinId");
            var clsId = $.cookie("paraShowInfoClsId");
            var clsName = $("#edit_className").val();
            if (clsName == null || clsName == "") {
                $("#checkInformation").css({ "color": "#FF0000" });
                $("#checkInformation").text("请填写完整信息!");
                $("#edit_classes_info").attr("disabled", true);
                return;
            }
            url = "/api/UpdataData/";
            showDiv();
            callApi(url, post, { Types: 3, KindergartenId: kindId, ClassName: clsName, ClassId: clsId }).done(function (data) {
                if (data.Attach.UpdataCheckClass != 0) {
                    $("#checkInformation").css({ "color": "#FF0000" });
                    $("#checkInformation").text("班级名称重复，请重新填写!");
                    $("#edit_classes_info").attr("disabled", true);
                } else {
                    $("#checkInformation").css({ "color": "blue" });
                    $("#checkInformation").text("班级名称正确！");
                    $("#edit_classes_info").attr("disabled", false);
                }
                hideDiv();
            }).fail(function (data) {
                hideDiv();
                alert(data.message);
            })
        },
        GetCreatePresidentInfo: function () {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            var kinderIdVal = 22;
            var kinderNameVal = "三米阳光幼儿园";
            var presidentNameVal = $("#realName").val();
            var presidentLoginNameVal = $("#loginName").val();
            var presidentIdentityVal = $("#selectIdentity").val();
            var passWord = "123456";
            if (presidentNameVal == null || presidentNameVal == ""
                || presidentLoginNameVal == null || presidentLoginNameVal == ""
                || presidentIdentityVal == null || presidentIdentityVal == "" || presidentIdentityVal == "-1") {
                alert("请填写完整信息！");
                return;
            }
            url = "/api/DB/";
            showDiv();
            callApi(url, post, { DBType: 4, LoginName: presidentLoginNameVal }).done(function (data) {
                if (data.Attach.CheckUserLoginName != 0) {
                    alert(data.Attach.CheckUserLoginName);
                    $("#checkInformation").css({ "color": "#FF0000" });
                    $("#checkInformation").text("电话号码重复，请重新填写！");
                    $("#create_president_info").attr("disabled", true);
                    hideDiv();
                } else {
                    url = "/api/CreatePresident/";
                    callApi(url, post, {
                        KindergartenId: kinderIdVal,
                        RealName: presidentNameVal,
                        Password: passWord,
                        LoginName: presidentLoginNameVal,
                        RoleType: presidentIdentityVal,
                        Phone: presidentLoginNameVal
                    }).done(function (data) {
                        if (data.Attach.Status == "success") {
                            alert("添加成功！");
                            $("#create_president_info").attr("disabled", true);
                            redirect("user.html");
                        } else {
                            alert("添加失败！");
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

        GetDeleteChild: function () {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            var classIdVal = $.cookie("deleteChildClassId");
            var kindIdVal = $.cookie("deleteKindId");
            var childIdVal = $.cookie("deleteChildId");
            url = "/api/DeleteData/";
            showDiv();
            callApi(url, post, { DeleteType: 0, ClassId: classIdVal, KindergartenId: kindIdVal, ChildId: childIdVal }).done(function (data) {
                if (data.Attach == null) {
                    alert(data.Message);
                    hideDiv();
                    return;
                }
                if (data.Attach.Status == "success") {
                    alert("删除成功！");
                    $("#delete" + childIdVal).attr("disabled", true);
                    redirect("user.html");
                } else {
                    alert("删除失败！");
                }
                hideDiv();
            }).fail(function (data) {
                hideDiv();
                alert(data.message);
            })
        },
        GetDeleteTeacher: function () {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }
            var classIdVal = $.cookie("deleteClassId");
            var kinderIdVal = $.cookie("deleteKindId");
            var userIdVal = $.cookie("deleteUserId");
            url = "/api/DeleteData/";
            showDiv();
            callApi(url, post, { DeleteType: 1, ClassId: classIdVal, KindergartenId: kinderIdVal, UserId: userIdVal }).done(function (data) {
                if (data.Attach == null) {
                    alert(data.Message);
                    return;
                }
                if (data.Attach.DeleteTeacherStatus == "success") {
                    alert("删除成功！");
                    $("#userIdVal").attr("disabled", true);
                    redirect("showEditTeacherInfo.html");
                } else {
                    alert("删除失败！");
                }
                hideDiv();
            }).fail(function (data) {
                hideDiv();
                alert(data.message);
            })
        },
        deleteClass: function (classId) {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            url = "/api/DelPresidentInfo/";
            showDiv();
            callApi(url, post, { type: 2, mubiaoId: classId }).done(function (data) {

                if (data == null || data.Attach == null) {
                    alert("发生未知错误，请联系您的管理员.");
                    redirect("user.html");
                    return;
                }
                if (data.Attach.status == "success") {
                    alert("删除成功!");
                    var clsTypeVal = $("#classTypeVal").val();
                    $.cookie("addClassInfo_classType", clsTypeVal, { expires: 1, path: '/' });
                    redirect("user.html");
                } else if (data.Attach.status == "fail") {
                    alert("删除失败!");
                    redirect("user.html");
                } else {
                    alert("发生未知错误，请联系您的管理员.");
                    redirect("user.html");
                    return;
                }
            }).fail(function (data) {
                hideDiv();
                alert("异常：" + data.message);
            });
        },
        //班级毕业
        classGraduate: function (classId) {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            url = "/api/DelPresidentInfo/";
            showDiv();
            callApi(url, post, { type: 4, mubiaoId: classId }).done(function (data) {

                if (data == null || data.Attach == null) {
                    alert("发生未知错误，请联系您的管理员.");
                    redirect("user.html");
                    return;
                }
                if (data.Attach.status == "success") {
                    alert("修改成功!");
                    var clsTypeVal = -1;//已毕业
                    $.cookie("addClassInfo_classType", clsTypeVal, { expires: 1, path: '/' });
                    redirect("user.html");
                } else if (data.Attach.status == "fail") {
                    alert("修改失败!");
                    redirect("user.html");
                } else {
                    alert("发生未知错误，请联系您的管理员.");
                    redirect("user.html");
                    return;
                }
            }).fail(function (data) {
                hideDiv();
                alert("异常：" + data.message);
            });
        },
        GenerateCreateTeacherInfo: function () {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            //准备幼儿园信息
            var kindergartenId = $.cookie("paraUserInfoKinId");
            var kindergartenName = $.cookie("paraUserInfoKinName");
            $("#kinId").val(kindergartenId);
            $("#kinName").val(kindergartenName);
            //准备班级信息
            var classId = $.cookie("paraAddTeacherClsId");
            var className = $.cookie("paraAddTeacherClsName");
            $("#clsName").val(className);
            $("#clsId").val(classId);
        },
        GenerateCreateChildInfo: function () {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            //准备幼儿园信息
            var kindergartenId = $.cookie("paraUserInfoKinId");
            var kindergartenName = $.cookie("paraUserInfoKinName");
            $("#kinId").val(kindergartenId);
            $("#kinName").val(kindergartenName);
            //准备班级信息
            var classId = $.cookie("paraAddChildClsId");
            var className = $.cookie("paraAddChildClsName");
            $("#clsName").val(className);
            $("#clsId").val(classId);
        },

        GenerateDisCls: function (type) {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            var kindIdVal = $.cookie("paraUserInfoKinId");
            url = "/api/User/";
            callApi(url, post, { Type: 4, KindergartenId: kindIdVal }).done(function(data) {
                for (var jos in data.Attach.ClassIdDisplays) {
                    var clsId = data.Attach.ClassIdDisplays[jos];
                    if (type == "one") {
                        $("#childContent" + clsId.ClassId).css({ "display": "block" });
                        $("#types" + clsId.ClassId).text("▲");
                    } else if (type == "two") {
                        $("#childContent" + clsId.ClassId).css({ "display": "none" });
                        $("#types" + clsId.ClassId).text("▼");
                    }
                }
            }).fail(function(data) {
                alert(data.message);
            });
        },
        //展开一个，关闭所有
        GenerateOneDisCls: function (id) {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }
            var kindIdVal = $.cookie("paraUserInfoKinId");
            url = "/api/User/";
            callApi(url, post, { Type: 4, KindergartenId: kindIdVal }).done(function(data) {
                for (var jos in data.Attach.ClassIdDisplays) {
                    var clsId = data.Attach.ClassIdDisplays[jos].ClassId;
                    if (clsId == id) {
                        $("#childContent" + id).css({ display: "block" });
                        $("#types" + id).html("<a style='text-decoration: none;'>▲</a>");
                    } else {
                        $("#childContent" + clsId).css({ "display": "none" });
                        $("#types" + clsId).text("▼");
                    }
                }
            }).fail(function(data) {
                alert(data.message);
            });
        },
        ChangeKinAndChangeClassInfo: function (kinId) {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            url = "/api/DB/";
            callApi(url, post, { DBType: 2, KindergartenId: kinId }).done(function (data) {
                var selectClsHtml = handlebarsHelp("#selectTypeHtml", data.Attach.ClassName);
                $("#selectClsInfo").empty();
                $("#selectClsInfo").append(selectClsHtml);
            }).fail(function (data) {
                alert(data.message);
            });
        },
        ResetChildParentPwd: function (userId) {
            url = "/api/DB/";
            callApi(url, post, { DBType: 13, UserId: userId }).done(function (data) {
                if (data.Attach.Status == 0) {
                    alert("重置密码成功！");
                    redirect("user_info.html");
                } else {
                    alert("重置密码失败！");
                    return;
                }
            }).fail(function (data) {
                alert(data.message);
            })
        },
        deleteParent: function (preId) {
            
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
                    redirect("user_info.html");
                    return;
                }
                if (data.Attach.status == "success") {
                    alert("删除成功!");
                    redirect("user_info.html");
                } else if (data.Attach.status == "fail") {
                    alert("删除失败!");
                    redirect("user_info.html");
                } else {
                    alert("发生未知错误，请联系您的管理员.");
                    redirect("user_info.html");
                    return;
                }
                hideDiv();
            }).fail(function (data) {
                hideDiv();
                alert("异常：" + data.message);
            });
        },
        // editClassData.html
        generateUpdateClsInfo: function (preId) {
            
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }
            var classesName = $.cookie("paraShowInfoClsName");
            var classId = $.cookie("paraShowInfoClsId");
            var kindergartenName = $.cookie("paraUserInfoKinName");
            var clsType = $.cookie("paraShowInfoClsType");
            $("#kinName").val(kindergartenName);
            $("#edit_className").val(classesName);

            for (var index = 0; index < 4; index++) {
                if (index == clsType) {
                    $("#classTypePic" + index).attr("src", "images/checkClass.png");
                } else {
                    $("#classTypePic" + index).attr("src", "images/unCheckClass.png");
                }
            }
            $("#classType").val(clsType);

        },

    };
}();