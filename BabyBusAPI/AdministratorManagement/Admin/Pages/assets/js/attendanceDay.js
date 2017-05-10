/*
 * Core script to handle all login specific things
 */

var AttendanceDay = function () {

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

    var handlebarsHelp = function (templateIdSelector, context) {
        var source = $(templateIdSelector).html();
        var template = Handlebars.compile(source);
        var html = template(context);
        return html;
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

    var GenerateAttendanceDayInfo = function (kinId, kinName) {
        $("#attDayInfoContent").empty();
       
        var year = $("#yearVal").val();
        var month = $("#monthVal").val();
        var day = $("#dayVal").val();

        $("#time").text(year + "年" + month + "月" + day + "日 考勤数据(天)");

        var permissionType = getPermissionType();
        var classIdVal = $.cookie("ClassId");
        var type;
        if (permissionType == 2) {//Teacher
            type = 1;
            if (classIdVal == null || classIdVal == "") {
                alert("信息缺失");
                return;
            }
        } else if (permissionType == 0 || permissionType == 3 || permissionType == 4) {
            $("#classTypeTab").css({ "display": "block" });
            type = 0;
        } else {
            alert("权限不足!");
            return;
        }

        var postData = {
            KindergartenId: kinId,
            Year: year,
            Month: month,
            Day: day,
            PermissionType: type,
            ClassId: classIdVal
        };
        

        url = "/api/AttendanceDayInfo/";
        
        showDiv();
        callApi(url, post, postData).done(function (data) {
            
            if (data.Attach == null
                || data.Attach.classChildCount == null
                || data.Attach.classChildCount.length <= 0) {
                alert("亲，当天没有考勤~");
                hideDiv();
                return;
            }
            var childInfos = data.Attach.classChildCount;
            var attenCqInfos = data.Attach.attendanceCqInfo;
            var attInfos = [];

            for (var childInfoIndex in childInfos) {
                var childInfo = childInfos[childInfoIndex];
                var cqCount = 0;
                var className = "-";
                var qqCount = 0;
                for (var attenCqInfoIndex in attenCqInfos) {
                    if (attenCqInfos[attenCqInfoIndex].classId == childInfo.classId) {
                        var cqInfo = attenCqInfos[attenCqInfoIndex];
                        cqCount = cqInfo.cqCount;
                        className = cqInfo.className;
                    }
                }//loop attenCqInfos end

                var qqCount = parseInt(childInfo.childCount) - parseInt(cqCount);
                if (className != "-") {
                    var attInfoItm = {
                        classId: childInfo.classId,
                        className: className,
                        cqCount: cqCount,
                        qqCount: qqCount,
                        childCount: childInfo.childCount
                    };
                    attInfos.push(attInfoItm);
                }
            }
            var html = handlebarsHelp("#AttendanceDayInfoHtml", attInfos);
            $("#attDayInfoContent").empty();
            $("#attDayInfoContent").append(html);

            hideDiv();
        }).fail(function (data) {
            hideDiv();
            alert("data.message" + data.message);
        });
    };
    
    var GenerateAttendanceDayDetailInfo = function () {

        var classId = $.cookie("paraAttendanceDayClassId");
        var className = $.cookie("paraAttendanceDayClassName");
        var year = $.cookie("paraAttendanceDayYear");
        var month = $.cookie("paraAttendanceDayMonth");
        var day = $.cookie("paraAttendanceDayDay");

        $("#time").text(className + "   " + year + "年" + month + "月" + day + "日 考勤数据(天)");

        var postData = {
            ClassId: classId,
            Year: year,
            Month: month,
            Day: day
        };
        showDiv();
        url = "/api/AttendanceDayDetailInfo/";
        callApi(url, post, postData).done(function (data) {
            
            if (data.Attach == null
                || data.Attach.childAttInfo == null
                || data.Attach.childAttInfo.length <= 0) {
                alert("亲，当天没有考勤~");
                hideDiv();
                return;
            }

            var sumCqCount = 0;
            var sumQqCount = 0;

            var childAttInfos = data.Attach.childAttInfo;
            $("#attDayDetailInfoContent1").empty();
            $("#attDayDetailInfoContent2").empty();
            for (var childInfoIndex in childAttInfos) {
                var childInfo = childAttInfos[childInfoIndex];
                var imgUrl = "images/defaultChildImage.png";
                if (childInfo.childImgName != null && childInfo.childImgName != "") {
                    imgUrl = "http:\/\/babybus.emolbase.com\/" + childInfo.childImgName;
                }
                var cqInfo = "-";
                if (childInfo.adInfo != null
                    && childInfo.adInfo[0] != null) {
                    var att = childInfo.adInfo[0];
                    if (att.Status == 1) {
                        cqInfo = "出勤";
                    } else if (att.Status == 0) {
                        cqInfo = "缺勤";
                    }
                }
                var attInfoItm = {
                    childId: childInfo.childId,
                    childName: childInfo.childName,
                    cqInfo: cqInfo,
                    imgUrl: imgUrl,
                };
                if (attInfoItm.cqInfo == "出勤") {
                    sumCqCount++;
                    var html = handlebarsHelp("#AttendanceDayDetailInfoHtml", attInfoItm);
                    $("#attDayDetailInfoContent1").append(html);
                    $("#cq" + attInfoItm.childId).attr("src", "images/childImg.jpg");
                    $("#cq" + attInfoItm.childId).attr("width", "25px");
                } else if (attInfoItm.cqInfo == "缺勤") {
                    sumQqCount++;
                    var html = handlebarsHelp("#AttendanceDayDetailInfoHtml", attInfoItm);
                    $("#attDayDetailInfoContent2").append(html);
                }
            }// loop childAttInfos end
            hideDiv();
        }).fail(function (data) {
            hideDiv();
            alert("data.message" + data.message);
        });


    };

    var url = "/api/Management/";
    var get = "GET";
    var post = "POST";

    return {

        //左侧显示幼儿园信息
        GenerateKindergartenAtNavigationBar: function (type) {
            
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            //use in the user.html 
            var deferred = $.Deferred();

            var paraAttendanceKinIdVal = $.cookie("paraAttendanceDayKinId");
            var paraAttendanceKinNameVal = $.cookie("paraAttendanceDayKinName");

            var kindergartenId = $.cookie("KindergartenId");
            var classIds = $.cookie("ClassId");

            var roleVal = $.cookie("Role");
            var permissionType = getPermissionType();
            var userId = $.cookie("UserId");

            url = "/api/GenerateKindergartenInfo/";
            callApi(url, post, { kindergartenId: kindergartenId, permissionType: permissionType, userId: userId }).done(function (data) {

                var html = handlebarsHelp("#attendanceNavKindergartens-template", data.Attach.Kindergartens);
                $("#attendanceNavKindergartens").append(html);

                if (type == "one") {//PrincipalAttendence.html
                    var firstKinId = data.Attach.Kindergartens[0].KindergartenId;
                    var firstKinName = data.Attach.Kindergartens[0].KindergartenName;
                    if (paraAttendanceKinIdVal == null) {
                        $("#attendanceNavKindergartens li:first").addClass("current");
                        $("#attendanceNavKindergartens li:first").css({ "background": "#D9FFFF" });
                        $("#showKinNameInfo").text(firstKinName);

                        GenerateAttendanceDayInfo(firstKinId, firstKinName);

                        $.cookie("paraAttendanceDayKinId", firstKinId, { expires: 1, path: '/' });
                        $.cookie("paraAttendanceDayKinName", firstKinName, { expires: 1, path: '/' });
                    } else {
                        $("#" + paraAttendanceKinIdVal).addClass("current");
                        $("#" + paraAttendanceKinIdVal).css({ "background": "#D9FFFF" });
                        $("#showKinNameInfo").text(paraAttendanceKinNameVal);

                        GenerateAttendanceDayInfo(paraAttendanceKinIdVal, paraAttendanceKinNameVal);
                    }
                } else if (type == "two") {
                    $("#" + paraAttendanceKinIdVal).addClass("current");
                    $("#" + paraAttendanceKinIdVal).css({ "background": "#D9FFFF" });
                    $("#showKinNameInfo").text(paraAttendanceKinNameVal);
                    GenerateAttendanceDayDetailInfo();
                }
                
            }).fail(function (data) {
                deferred.reject();
                console.log(data.message);
            });
        },
        GetAttendanceDayInfo: function (kinId, kinName) {

            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            var allLi = $("#attendanceNavKindergartens li");
            for (var liIndex = 0; liIndex < allLi.length; liIndex++) {
                $("#" + allLi[liIndex].id).removeClass("current");
                $("#" + allLi[liIndex].id).css({ "background": "" });
            }
            $("#" + kinId).addClass("current");
            $("#" + kinId).css({ "background": "#D9FFFF" });
            $("#showKinNameInfo").text(kinName);

            GenerateAttendanceDayInfo(kinId, kinName);
        },//
    };
}();