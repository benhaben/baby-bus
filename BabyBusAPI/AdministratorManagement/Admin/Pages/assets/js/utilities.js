/*
 * Core script to handle all login specific things
 */

var Utilities = function () {

    "use strict";

    $.fn.serializeObject = function () {
        var arrayData, objectData;
        arrayData = this.serializeArray();
        objectData = {};

        $.each(arrayData, function () {
            var value;

            if (this.value != null) {
                value = this.value;
            } else {
                value = '';
            }

            if (objectData[this.name] != null) {
                if (!objectData[this.name].push) {
                    objectData[this.name] = [objectData[this.name]];
                }

                objectData[this.name].push(value);
            } else {
                objectData[this.name] = value;
            }
        });

        return objectData;
    };


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
        }else if (roleVal == "Parent") {
            permissionType = 1;
        }else if (roleVal == "Teacher") {
            permissionType = 2;
        }else if (roleVal == "President") {
            permissionType = 3;
        }else if (roleVal == "SuperPresident") {
            permissionType = 4;
        } 
        return permissionType;
    };
    var showDiv = function () {
        document.documentElement.scrollTop = 0;
        $("#box").css({ "display": "block" });
        $("#Mybox").css({ "display": "block" });
        $("#bodyonline").css({ "overflow-y": "hidden" });
    };
    var hideDiv = function () {
        $("#box").css({ "display": "none" });
        $("#Mybox").css({ "display": "none" });
        $("#bodyonline").css({ "overflow-y": "scroll" });
    };
    var generateClassesAndChildrenAtAccordion = function () {
        //use in the user.html
        url = "/api/Management/";
        var classIds = $.cookie("ClassId");
        var roleVale = $.cookie("Role");
        var kindergartenId = $.cook("kinderId");
        callApi(url, post, { ManagementRequestType: 0, KindergartenId: kindergartenId, ClassId: classIds, Role: roleVale }).done(function (data) {
            var html = handlebarsHelp("#accordionClass-template", data.Attach);
            $("#tab_1_1").append(html);
            data.Attach.forEach(function (e) {
                var classId = e.ClassId;
                callApi(url, post, { ManagementRequestType: 0, KindergartenId: kindergartenId, ClassId: classId, Role: roleVale }).done(function (dataClass) {

                    var htmlChildren = handlebarsHelp("#children-template", dataClass.Attach.Parent);
                    var $collapseDiv = $("#collapse" + classId).find("div");
                    $collapseDiv.append(htmlChildren);
                    $collapseDiv.find("button.btn").bind("click", function () {
                        var childid = $(this).attr("childId");
                        var kindergartenId = $(this).attr("kindergartenId");
                        var classId = $(this).attr("classId");
                        $.cookie('childId', childid, { expires: 1, path: '/' });
                        $.cookie('kindergartenId', kindergartenId, { expires: 1, path: '/' });
                        $.cookie('classId', classId, { expires: 1, path: '/' });
                        redirect("user_info.html");
                    });
                }).fail(function (dataClass) {
                    console.log(data.message);
                });
            });

        }).fail(function (data) {
            console.log(data.message);
        });
    }


    var url = "/api/Management/";
    var get = "GET";
    var post = "POST";

    return {
        //public method

        Login: function (data) {
            
            url = "/api/AdminLogin/";
            return callApi(url, post, data);
        },
        SendNotice: function (data) {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            url = "/api/Notice/";
            return callApi(url, post, data);
        },
        TestLogin: function () {
            url = "/api/AdminLogin/";
            return callApi(url, get);
        },
        Redirect: function (paras) {
            redirect(paras);
        },
        GetUserCount: function () {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            url = "/api/Management/";
            var roleVal = $.cookie("Role");
            callApi(url, post, { ManagementRequestType: 2 }).done(function (data) {
                if (roleVal == "Administrator") {
                    document.getElementById("countfile").style.display = "block";
					$("#openId").css({ "display": "none" });
                    $("#closeId").css({ "display": "none" });
                }
                $("#userCount").text(data.Attach.TeachersCount + data.Attach.ParentsCount);
                $("#payUserCount").text(0);
                $("#kindergartenCount").text(data.Attach.KindergartensCount);
                $("#testCount").text(data.Attach.KindergartensCount);

            }).fail(function (data) {
                console.log(data.message);
            });
        },
        GenerateExcelInfo: function () {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            url = "/api/Test/";
            callApi(url, post).done(function (data) {
                alert(data.Attach);
            }).fail(function (data) {
                console.log(data.message);
            });
        },
        GetChangeClass: function () {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            url = "/api/DB/";
            var kindergartenId = $.cookie("kindergartenIds");

            callApi(url, post, { DBType: 2, KindergartenId: kindergartenId }).done(function (data) {
                for (var jos in data.Attach.ClassName) {
                    var classNames = data.Attach.ClassName[jos];
                    $("#selectClassName").append("<option value = " + classNames.ClassId + ">" + classNames.ClassName + "</option>")
                }
            }).fail(function (data) {
                alert(data.message);
            });
        },
        GetUpdateInfo: function () {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            url = "/api/DB/";
            var childId = $.cookie("childId");
            var classId = $.cookie("classId");
            var childName = $("#childName").val();
            var className = $("#className").val();
            var classIdChoos = $("#selectClassName").val();
            if (childName != null && classIdChoos != -1) {
                callApi(url, post, { DBType: 1, ChildId: childId, ClassId: classId, ClassName: className, ChildName: childName, ClassIdChoos: classIdChoos }).done(function (data) {
                }).fail(function (data) {
                    alert(data.message);
                });
            } else {
                alert("更改失败");
            }
        },
        GetCreateInfo: function () {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            url = "/api/DB/";
            var childName = $("#childName").val();
            var kindergartenId = $.cookie("kindergartenIds");
            var classIdChoos = $("#selectClassName").val();

            callApi(url, post, { DBType: 0, ChildName: childName, KindergartenId: kindergartenId, ClassIdChoos: classIdChoos }).done(function (data) {
                if (data.Attach.States == "Success") {
                    alert("添加成功！");

                } else {
                    alert("添加失败！");
                }
            }).fail(function (data) {
                alert(data.message);
            });
        },

        DrawHeader: function () {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }
            var loginUserName = $.cookie("LoginUserName");
            var roleVal = $.cookie("Role");

            var classIdVal = $.cookie("ClassId");
            var kindergartenIdVal = $.cookie("KindergartenId");
            var classNameVal = $.cookie("ClassName");
            var kindergartenNameVal = $.cookie("KindergartenName");

            if (loginUserName != null && loginUserName != "") {
                loginUserName = " , " + loginUserName;
            }
            var roleName = "";

            var permissionType = getPermissionType();
            if (permissionType == 0) {//admin
                roleName = "管理员";
            } else if (permissionType == 3 || permissionType == 4) {
                roleName = "园长";
            } else if (permissionType == 2) {
                $.cookie("paraClassId", classIdVal);
                roleName = kindergartenNameVal + "," + classNameVal + "      教师";
            } else if (permissionType == 1) {
                roleName = "家长";
            }

            var welcomeWord = "您好 " + loginUserName;

            $("#drawHeader").append("<div class='container' id='one'></div>");
            $("#one").append("<a class='navbar-brand' href='statistics.html' id='helloWorldVal'>" +
                                "<i class='icon-home'></i> <strong style='font-size:13px'>" + welcomeWord + "</strong></a>");

            $("#one").append("<a href='#' class='toggle-sidebar bs-tooltip' data-placement='bottom' data-original-title='左侧栏'> <i class='icon-reorder'></i> </a>");

            $("#one").append("<ul class='nav navbar-nav navbar-left hidden-xs hidden-sm' id='ulContent'></ul>");

            //tab

            //幼儿园【宝贝资料】<admian、园长、教师可以看>【园长信息】<admian可以看>【付费记录】<admian、园长可以看>
            if (permissionType == 0 || permissionType == 2 || permissionType == 3 || permissionType == 4) {//admin
                //$("#ulContent").append("<li><a ></a></li>");
                $("#ulContent").append("<li class='dropdown user' id='kindergartenTab'></li>");

                $("#kindergartenTab").append("<a href='#' class='dropdown-toggle' data-toggle='dropdown'>" +
                                        "幼儿园 " +
                                        "<i class='icon-caret-down small'></i>" +
                                      "</a>");
                $("#kindergartenTab").append("<ul class='dropdown-menu' id='kindergartenTabDetail'></ul>");
                $("#kindergartenTabDetail").append("<li><a href='user.html'>宝贝资料</a></li>");
                if (permissionType == 0) {
                    $("#kindergartenTabDetail").append("<li><a href='presidentInfo.html'>园长信息</a></li>");
                }
                if (permissionType == 0 || permissionType == 3 || permissionType == 4) {
                    $("#kindergartenTabDetail").append("<li><a href='PaymentRecordMonth.html'>付费记录</a></li>");
                }
            }

            //考勤【考勤/月】【考勤/日】<admian、园长、教师可以看>
            if (permissionType == 0 || permissionType == 2 || permissionType == 3 || permissionType == 4) {//admin
                $("#ulContent").append("<li class='dropdown user' id='attendanceTab'></li>");

                $("#attendanceTab").append("<a href='#' class='dropdown-toggle' data-toggle='dropdown'>" +
                                        "考 勤 " +
                                        "<i class='icon-caret-down small'></i>" +
                                      "</a>");

                $("#attendanceTab").append("<ul class='dropdown-menu' id='attendanceTabDetail'></ul>");
                $("#attendanceTabDetail").append("<li><a id='attendanceMonth' href='PrincipalAttendence.html'>考勤 / 月</a></li>");
                $("#attendanceTabDetail").append("<li><a id='attendanceDay' href='AttendanceDayInfo.html'>考勤 / 天</a></li>");

            }
            //消息【消息统计】<admian、园长、教师可以看>【发送消息】<园长、教师可以看>【消息记录】<admian、园长、教师可以看>
            if (permissionType == 0 || permissionType == 2 || permissionType == 3 || permissionType == 4) {//admin

                $("#ulContent").append("<li class='dropdown user' id='noticeTab'></li>");

                $("#noticeTab").append("<a href='#' class='dropdown-toggle' data-toggle='dropdown'>" +
                                        "消 息 " +
                                        "<i class='icon-caret-down small'></i>" +
                                      "</a>");
                $("#noticeTab").append("<ul class='dropdown-menu' id='noticeTabDetail'></ul>");
                $("#noticeTabDetail").append("<li><a id='noticeStatistics' href='noticeStatistics.html'>消息统计</a></li>");
                $("#noticeTabDetail").append("<li><a id='SendMessageRecords' href='SendMessageRecords.html'>消息记录</a></li>");
                if (permissionType == 2 || permissionType == 3 || permissionType == 4) {
                    $("#noticeTabDetail").append("<li><a id='SendNotice' href='SendNotice.html'>发送消息</a></li>");
                }
            }

            //多元智力【参数配置】【题目维护】【多元智力测评】<admian>
            if (permissionType == 0 || permissionType == 1 || permissionType == 2) {
                 
                if (permissionType == 0) {
                    $("#ulContent").append("<li class='dropdown user' id='duoyuanzhiliTab'></li>");

                    $("#duoyuanzhiliTab").append("<a href='#' class='dropdown-toggle' data-toggle='dropdown'>" +
                                            "多元智力  " +
                                            "<i class='icon-caret-down small'></i>" +
                                          "</a>");
                    $("#duoyuanzhiliTab").append("<ul class='dropdown-menu' id='duoyuanzhiliTabDetail'></ul>");
                    $("#duoyuanzhiliTabDetail").append("<li><a href='ParameterConfiguration.html' target='_self'>参数配置</a></li>");
                    $("#duoyuanzhiliTabDetail").append("<li><a href='InputTestQuestion.html' target='_self'>题目维护</a></li>");
                    $("#duoyuanzhiliTabDetail").append("<li><a href='ParentsIntelligenceQuestion.html' target='_self'>多元智力测评</a></li>");
                }
            }
            //体质检测<admian可查看>
            if (permissionType == 0) {
                $("#ulContent").append("<li class='dropdown user' id='physicalExaTab'></li>");
                $("#physicalExaTab").append("<a href='#' class='dropdown-toggle' data-toggle='dropdown'>" +
                                       "体质检测 " +
                                       "<i class='icon-caret-down small'></i>" +
                                     "</a>");

                $("#physicalExaTab").append("<ul class='dropdown-menu' id='physicalExaDetailTab'></ul>");
                $("#physicalExaDetailTab").append("<li><a id='physicalExaWeight' href='WeightForStandardHeight.html'>身高标准体重</a></li>");
                $("#physicalExaDetailTab").append("<li><a id='individualStId' href='IndividualStandard.html'>单项评分标准</a></li>");
                $("#physicalExaDetailTab").append("<li><a id='individualStId' href='phyExaminationPlan.html'>体质测评(有计划)</a></li>");
                $("#ulContent").append("<li class='dropdown user' id='giftedYoungTabloidId'></li>");
                $("#giftedYoungTabloidId").append("<a href='#' class='dropdown-toggle' data-toggle='dropdown'>" +
                                       "优幼小报 " +
                                       "<i class='icon-caret-down small'></i>" +
                                     "</a>");
                $("#giftedYoungTabloidId").append("<ul class='dropdown-menu' id='giftedYoungTabId'></ul>");
                $("#giftedYoungTabId").append("<li><a id='sendGiftedYongId' href='SendGiftedYoungTab.html'>发送优幼小报</a></li>");
                $("#giftedYoungTabId").append("<li><a id='sendGiftedYongDetailId' href='SendGiftedYoungDetail.html'>优幼小报记录</a></li>");
            }
            //广告配置<admian>
            if (permissionType == 0) {
                $("#ulContent").append("<li><a href='UploadAdvertisementPicture.html' target='_self'>广告配置</a></li>");
            }
            //反馈信息
            if (permissionType == 0) {
                $("#ulContent").append("<li><a href='FeedbackRecords.html' target='_self'>反馈信息</a></li>");
            }
            //退出
            $("#one").append("<ul class='nav navbar-nav navbar-right' id='ul2Content'></ul>");
            $("#ul2Content").append("<li class='dropdown user' id='hederLi'></li>");

            $("#hederLi").append("<a href='#' class='dropdown-toggle' data-toggle='dropdown'>" +
                                    "<i class='icon-male'></i>" +
                                    "<span class='username'>" + roleName + "</span>" +
                                    "<i class='icon-caret-down small'></i>" +
                                  "</a>");

            $("#hederLi").append("<ul class='dropdown-menu' id='permissionMenu'></ul>");
            $("#permissionMenu").append("<li><a id='logout'><i class='icon-user'></i> 退出</a></li>");

            $("#logout").click(
                function () {
                    //清除所有cookie

                    var allCookies = document.cookie.split(';');
                    //alert(allCookies.length);
                    for (var allCookiesIndex = 0; allCookiesIndex < allCookies.length; allCookiesIndex++) {
                        var cookie1 = allCookies[allCookiesIndex].split('=');
                        if (cookie1 != null && cookie1.length == 2) {
                            var cookieName = cookie1[0].trim();
                            $.cookie(cookieName, null, { expires: 1, path: '/' });
                        }
                    }
                    Utilities.Redirect("login.html");
                }
            );

        },

        GenerateKindergartenAtNavigationBar: function () {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            //use in the user.html 
            var deferred = $.Deferred();
            var roleVal = $.cookie("Role");
            var kindergartenId = $.cookie("KindergartenId");
            var classIds = $.cookie("ClassId");
            url = "/api/Management/";
            callApi(url, post, { ManagementRequestType: 1, Role: roleVal, KindergartenId: kindergartenId, ClassId: classIds }).done(function (data) {
                var html = handlebarsHelp("#navKindergartens-template", data.Attach.Kindergartens);
                $("#navKindergartens").append(html);
                $("#navKindergartens li:first").addClass("current");
                deferred.resolve();
            }).fail(function (data) {
                deferred.reject();
                console.log(data.message);
            });
            return deferred.promise();
        },

        GenerateClassesAndChildrenAtAccordion: function () {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }
            //use in the user.html
            generateClassesAndChildrenAtAccordion();
            /* url = "/api/Examine/";
             callApi(url, post, { ManagementRequestType: 1 }).done(function (data) {
                 alert("data");
                alert("data" + data.Attach);
             }).fail(function (data) {
                 alert(data.message);
             });*/
        },
        GenerateChildInfo: function () {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            //use in the user_info.html
            url = "/api/Management/";
            var classId = $.cookie("classId");
            var kindergartenId = $.cookie("kindergartenId");
            var childId = $.cookie("childId");
            callApi(url, post, { ManagementRequestType: 0, KindergartenId: kindergartenId, ClassId: classId, ChildId: childId }).done(function (dataClass) {
                $("#childName_user_info").text(dataClass.Attach.Parent[0].ChildName);

                var htmlChildren = handlebarsHelp("#children-userinfo-template", dataClass.Attach.Parent[0]);
                $("#userInfo").append(htmlChildren);

                var htmlParent = handlebarsHelp("#parents-userinfo-template", dataClass.Attach.Parent);
                $("#parentInfo").append(htmlParent);

                var htmlTeacher = handlebarsHelp("#teachers-userinfo-template", dataClass.Attach.Teacher);
                $("#teacherInfo").append(htmlTeacher);

            }).fail(function (dataClass) {
                console.log(data.message);
            });
        },
        InitEditControlForUserInfo: function () {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            $("#edit_user_info").click(function (event) {
                /* Act on the event */
                var postData = $("#edit_form_user_info").serializeObject();
                url = "/api/Management/";
                callApi(url, post, { ManagementRequestType: 5, ChildId: postData.ChildId, ClassName: postData.ClassName, KindergartenName: postData.KindergartenName }).done(function () {
                    // location.reload();
                    redirect("user.html");
                }).fail(function (e) {
                    alert(e.message);
                });
            });
        },
        GenertFeedback: function () {
            if (getPermissionType() == 0) {
                return;
            }
            $("#feedBackIdInfoId").append("<div style='float:right;position:relative;top:10px'><a onclick='openFeedBack()' id='openId' style='display:block'>" +
                "<div style='overflow:hidden;position:relative; height:35px;padding:0 20px 0 15px; color:#FFF; line-height: 35px;background: #30ABCC;vertical-align:middle;cursor:pointer;width: 170px; height: 30px;'><center><h3 style='position:relative;bottom:15px' id='opentitleId'></h3></center></div></a></div>");
            $("#opentitleId").append("<img src='images/chat.png' style='width:25px;float:left;position:relative;top:2px'/>");
            $("#opentitleId").append("<label> 请给我们留言</label>");
            $("#opentitleId").append("<i style='color:white;font-weight:900;float:right'>∧</i>");
            $("#feedBackIdInfoId").append("<div id='closeId' style='width:350px;height:255px;float:right;position:relative;top:100%;display:none;background-color:#FFF'>" +
                "<a onclick='closeFeedBack()' id = 'closeIdFeedBackId'><center><h3 style='overflow:hidden;position:relative; height:35px;padding:0 20px 0 15px; color:#FFF; line-height: 35px; background: #30ABCC;vertical-align:middle;cursor:pointer;position:relative;bottom:20px'id = 'closeInfoFeedBackId'></h3></center></a>");
            $("#closeInfoFeedBackId").append("<img src='images/chat.png' style='width:25px;float:left;position:relative;top:10px'/>");
            $("#closeInfoFeedBackId").append("<label> 请给我们留言</label>");
            $("#closeInfoFeedBackId").append("<i style='color:white;font-weight:900;float:right;position:relative;top:10px'>∨</i>");
            $("#closeId").append("<div style='position:relative;left:3%'id='feedBackInfoDataId'></div>");
            $("#feedBackInfoDataId").append("<p style='font-size:15px;position:relative;bottom:19px'>请留言，我们会尽快联系您，谢谢！</p>");
            $("#feedBackInfoDataId").append("<textarea id='feedbookId' style='position:relative;bottom:10px;width:95%;height:100px' placeholder='请输入您的问题'></textarea>");
            $("#feedBackInfoDataId").append("<div><button class='btn' id='submitFeedBackBtn' style='float:right;background-color:#e27522;position:relative;top:10px;right:7%' onclick='submitFeekbooc()'>留言</button></div>");
            $("#openId").click(function () {
                $("#openId").css({ "display": "none" });
                $("#closeId").css({ "display": "block" });
            })
            $("#closeIdFeedBackId").click(function () {
                $("#openId").css({ "display": "block" });
                $("#closeId").css({ "display": "none" });
            })
            $("#submitFeedBackBtn").click(function () {
                var userIdVal = $.cookie("UserId");
                var feedInfoVal = $("#feedbookId").val();
                if (feedInfoVal == null || feedInfoVal == "") {
                    alert("请填写问题！");
                    return;
                }
                url = "/api/FeedBack/";
                showDiv();
                callApi(url, post, { recordType: 1, UserId: userIdVal, Content: feedInfoVal }).done(function (data) {
                    if (data.Attach.Status == "0") {
                        alert("留言成功！");
                        $("#feedbookId").val("");
                    } else {
                        alert("请联系管理员权限！");
                    }
                    hideDiv();
                }).fail(function (data) {
                    alert(data.message);
                })
            })
        },
        GenentAdertisementPictureInfo: function (normalPics) {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }
            var roleVal = getPermissionType();
            if (roleVal == 0) {
                var userTypeVal = "admin";
                var userIdNow = 0;
                var userGuid = $.cookie('UserGuid');
            } else {
                var userTypeVal = "President";
                var userIdNow = $.cookie('UserId');
                var userGuid = "";
            }
            if ( normalPics == null) {
                alert("请选择一张图片！");
                return;
            }
            var descriptionVal = $("#picDescriptionId").val();
            if (descriptionVal == null) {
                var descriptionVal = "";
            }
            url = "/api/UploadAdvertisementPic/";
            showDiv();
            callApi(url, post, { advType: 1, NomalsPics: normalPics, UserId: userIdNow, Description: descriptionVal, UserType: userTypeVal, Guid: userGuid }).done(function (data) {
                if (data.Attach.UploadType == 0) {
                    alert("上传成功！");
                    redirect("UploadAdvertisementPicture.html");
                }
                hideDiv();
            }).fail(function (data) {
                alert(data.message);
            })
        },

        GenerAdvertisementDetailInfo: function () {
            url = "/api/UploadAdvertisementPic/";
            showDiv();
            callApi(url, post, { advType: 2 }).done(function (data) {
                if (data.Attach.AdvertisementDetailInfo == null || data.Attach.AdvertisementDetailInfo == "") {
                    alert("暂时无图片");
                    return;
                }
                var advPic = [];
                for (var jos in data.Attach.AdvertisementDetailInfo) {
                    var advPicVal = data.Attach.AdvertisementDetailInfo[jos];

                    var createTime = advPicVal.Year + "-" +advPicVal.Month + "-"+advPicVal.Day;
                    var desprition = "-";
                    if (advPicVal.Description != null
                        && advPicVal.Description != "") {
                        desprition = advPicVal.Description;
                    }
                    var item = {
                        AdvertisementPicId: advPicVal.AderId,
                        imgUrl: "http://babybus.emolbase.com\/" + advPicVal.NormalPics,
                        DetailTime: createTime,
                        Desprition: desprition,
                    };
                    // advPic.push(item);
                    var html = handlebarsHelp("#advUploadPicId", item);
                    $("#uploadPicId").append(html);

                    if (advPicVal.IsUsed == 1){
                        $("#"+advPicVal.AderId).attr("checked", true);
                    } else {
                        $("#img" + advPicVal.AderId).css({ "opacity": "0.6" });
                    }
                }
                hideDiv();
            }).fail(function (data) {
                alert(data.message);
            })
        },

        GenertSubmitPicInfo: function () {
            var checkedPicVal = $("input[name = 'name']:checkbox");
            var checkedTrueVal = "";
            var checkedFalseVal = "";
            var totalVal = $("#inputPicTotalId").val();
            if(totalVal == "" || totalVal == null){
                var totalVal = $("#inputPicTotalId").attr("placeholder");
            }
            var index = 0;
            $(checkedPicVal).each(function () {
                if ($(this).is("input:checked")) {
                    checkedTrueVal += $(this).attr("value") + ",";
                    index++;
                } else {
                    checkedFalseVal += $(this).attr("value") + ",";
                }
            })
            if (index > totalVal) {
                alert("您选择的图片超过规定数量，请重新选择使用的图片！");
                return;
            }
            url = "/api/UploadAdvertisementPic/";
            showDiv();
            callApi(url, post, { advType: 3, CheckedTrue: checkedTrueVal, CheckedFalse: checkedFalseVal }).done(function (data) {
                if (data.Attach.Updatasuccess == 0) {
                    alert("提交成功");
                    redirect("UploadAdvertisementPicture.html");
                } else {
                    alert("提交失败，请联系管理员！");
                    return; 
                }
                hideDiv();
            }).fail(function (data) {
                alert(data.message);
            })
        }

    };
}();