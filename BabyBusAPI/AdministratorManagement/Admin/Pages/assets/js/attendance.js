/*
 * Core script to handle all login specific things
 */

var Attendance = function () {

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
    var getImageByMonth = function (month) {
        var images = {
            '1': "images/1.jpg",
            '2': "images/2.gif",
            '3': "images/3.gif",
            '4': "images/4.jpg",
            '5': "images/5.gif",
            '6': "images/6.jpg",
            '7': "images/7.gif",
            '8': "images/8.jpg",
            '9': "images/9.jpg",
            '10': "images/10.gif",
            '11': "images/11.gif",
            '12': "images/12.jpg"
        };
        return images[month];
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

    var GeneratePrincipalAttendanceInfo = function (kindergartenIdVal, permissionType, year) {

        var date = new Date();
        var CurrentYear = parseInt(date.getFullYear());
        var CurrentMonth = parseInt(date.getMonth()) + 1;

        var type;
        var classIdVal = $.cookie("paraClassId");

        $("#attLoction").html("<i class='icon-home'></i>"
                                 + " 考勤"
                                 + "<a href='PrincipalAttendence.html'> > 考勤概况</a>");

        if (permissionType == 2) {
            type = 1;
        } else if (permissionType == 0 || permissionType == 3 || permissionType == 4) {
            type = 0;
        } else {
            alert("权限不足!");
            return;
        }
        /*var image = ["images/one.png",
                    "images/two.png",
                    "images/three.png",
                    "images/four.png",
                    "images/five.png",
                    "images/six.png",
                    "images/seven.png",
                    "images/eight.png",
                    "images/nine.png",
                    "images/ten.png",
                    "images/elevent.png",
                    "images/december.png"];*/

        url = "/api/Attendance/";
        showDiv();
        callApi(url, post, {
            KindergartenId: kindergartenIdVal,
            Year: year,
            ClassId: classIdVal,
            Type: type
        }).done(function (data) {
            //--1
            var monthAndChildNum = [];

            for (var index = 1; index <= 12; index++) {
                var sAttendanceVal = '-';

                for (var masterInfoIndex in data.Attach.attendanceMasterInfo) {
                    if (data.Attach.attendanceMasterInfo[masterInfoIndex].key == index) {
                        sAttendanceVal = data.Attach.attendanceMasterInfo[masterInfoIndex].sum;
                        continue;
                    }
                }//loop data.Attach.attendanceMasterInfo end

                var item = {
                    month: index,
                    childNum: data.Attach.ChildCount,
                    sAttendance: sAttendanceVal,
                    imags: getImageByMonth(index)
                };
                monthAndChildNum.push(item);
            }
            var html = handlebarsHelp("#PrincipalAttendanceMonthHtml", monthAndChildNum);
            $("#monthContent").empty();
            $("#monthContent").append(html);
            //clear init value
            if (year == CurrentYear) {
                for (var i = (CurrentMonth + 1) ; i < 13; i++) {
                    var m = i + '';
                    $("#a" + m).removeAttr("onclick");

                    $("#" + m).text('');
                    $("#sAttendance" + m).text('');
                    $("#yAttendance" + m).text('');
                }
            }
            hideDiv();
        }).fail(function (data) {
            hideDiv();
            alert("data.message" + data.message);
        });
    };
    var GeneratePrincipalAttClsesInfo = function (kingdergartenId, permissionType) {
        if (kingdergartenId == null || permissionType == null) {
            alert("信息缺失");
            return;
        }
        var type = 0; //0代表幼儿园
        var classIdVal = $.cookie("ClassId");
        var classNameVal = $.cookie("ClassName");

        if (permissionType == 2) {//Teacher
            type = 1;
            if (classIdVal == null || classIdVal == "") {
                alert("信息缺失");
                return;
            }
        } else if (permissionType == 0 || permissionType == 3 || permissionType == 4) {
            $("#classTypeTab").css({ "display": "block" });
            $("#parentClassTypeTab").attr({ "class": "tabbable tabbable-custom tabbable-full-width" });
            type = 0;
        } else {
            alert("权限不足!");
            return;
        }
        
        var year = $.cookie("paraYear");
        var month = $.cookie("paraMonth");
        $("#monthImg").attr("src", getImageByMonth(month));
        $("#time").text(year + "年" + month + "月");

        $("#attLoction").html("<i class='icon-home'></i>"
                                 + " 考勤"
                                 + "<a href='PrincipalAttendence.html'> > 考勤概况</a>"
                                 + "<a href='PrincipalAttClses.html'> > 考勤详情</a>");

        url = "/api/AttendanceClasses/";
        var classTypeVal = $("#classTypeVal").val();
        var classTypeDes = getClassTypeDes(classTypeVal);
        showDiv();
        callApi(url, post, {
            Year: year,
            Month: month,
            KingdergartendId: kingdergartenId,
            Type: type,
            ClassId: classIdVal,
            ClassType: classTypeVal
        }).done(function (data) {

            if (data.Attach.AttendanceClass.length <= 0) {
                //alert("亲，还没有考勤信息~");
                alert("暂时没有 【" + classTypeDes + "】 班级信息!");
                $("#PrincipalAttClsesContent").empty();
                hideDiv();
                //redirect("PrincipalAttendence.html");
                return;
            }

            var classInfo = [];
            for (var jos in data.Attach.AttendanceClass) {
                var id = data.Attach.AttendanceClass[jos]['ClassId'];
                var name = data.Attach.AttendanceClass[jos]['ClassName'];
                
                var count = "-";
                var yAttendance = "-";
                var sAttendance = "-";
                var qAttendance = "-";
                var todayAttendance = 0;
                for (var childClassIndex in data.Attach.ChildClass) {
                    if (data.Attach.ChildClass[childClassIndex]['key'] == id) {
                        count = data.Attach.ChildClass[childClassIndex]['sum'];
                    }
                }// loop data.Attach.ChildClass end

                for (var actualAttendanceIndex in data.Attach.ActualAttendance) {
                    if (data.Attach.ActualAttendance[actualAttendanceIndex]['key'] == id) {
                        sAttendance = data.Attach.ActualAttendance[actualAttendanceIndex]['sAtten'];
                        qAttendance = data.Attach.ActualAttendance[actualAttendanceIndex]['qAtten'];
                    }
                }// loop data.Attach.ActualAttendance end

                for (var todayAttenInfoIndex in data.Attach.TodayAttenInfo) {
                    if (data.Attach.TodayAttenInfo[todayAttenInfoIndex]['classId'] == id) {
                        todayAttendance = data.Attach.TodayAttenInfo[todayAttenInfoIndex]['attendCount'];
                    }
                }// loop data.Attach.ActualAttendance end

                var item = {
                    classId: id,
                    className: name,
                    count: count,
                    sAttendance: sAttendance,
                    qAttendance: qAttendance,
                    tAttendance: todayAttendance
                };
                classInfo.push(item);
            }//generate val end
            var html = handlebarsHelp("#PrincipalAttClsesHtml", classInfo);
            $("#PrincipalAttClsesContent").empty();
            $("#PrincipalAttClsesContent").append(html);
            var myDate = new Date();
            var hour = myDate.getHours();

            var CurrentYear = parseInt(myDate.getFullYear());
            var CurrentMonth = parseInt(myDate.getMonth()) + 1;
            if (year == CurrentYear && month == CurrentMonth) {
                for (var clsInfoIndex in classInfo) {
                    var clsInfo = classInfo[clsInfoIndex];

                    if (clsInfo.tAttendance <= 0) {
                        $("#content" + clsInfo.classId).css({ "border-color": "red" });
                    }

                    if (clsInfo.qAttendance >= 1) {
                        $("#content" + clsInfo.classId).css({ "border-bottom-color": "yellow" });
                    }
                }
            }
            hideDiv();
        }).fail(function (data) {
            hideDiv();
            console.log(data.message);
        });
    };
    var GeneratePrincipalAttClsDetailInfo = function (kindergartenIdVal) {
        //var kindergartenIdVal = $.cookie("paraKindergartenId");
        var calssIdVal = $.cookie("paraClassId");
        var clssNameVal = $.cookie("paraClassName");
        var yearVal = $.cookie("paraYear");
        var monthVal = $.cookie("paraMonth");

        $("#attLoction").html("<i class='icon-home'></i>"
                                 + " 考勤"
                                 + "<a href='PrincipalAttendence.html'> > 考勤概况</a>"
                                 + "<a href='PrincipalAttClses.html'> > 考勤详情</a>"
                                 + "<a href='PrincipalAttClsDetail.html'> > 缺勤学生</a>");

        url = "/api/AttendanceClsDetail/";
        showDiv();
        callApi(url, post, { KindergartenId: kindergartenIdVal, calssId: calssIdVal, year: yearVal, month: monthVal }).done(function (data) {

            if (data.Attach.allChildsAtDetails.length <= 0) {
                alert("亲，还没有考勤信息~");
                redirect("PrincipalAttClses.html");
                return;
            }
            $("#monthImg").attr("src", getImageByMonth(monthVal));
            var classDateInfo = {
                className: clssNameVal,
                year: yearVal,
                month: monthVal,
                image: getImageByMonth(monthVal)
            };
            var html = handlebarsHelp("#PrincipalAttClsesHeaderHtml", classDateInfo);
            $("#clsDetailContent").append(html);
            //2015年6月16日更改Begin--------------------------------------------------------
            var acpInfos1 = [];
            var acpInfos2 = [];
            for (var acpIndex in data.Attach.allChildAndParentInfos) {
                var acpInfo = data.Attach.allChildAndParentInfos[acpIndex];
                if (acpInfo == null || acpInfo.childId == null) {
                    continue;
                }
                var phone = "---";
                if (acpInfo.parentInfo != null
                    && acpInfo.parentInfo.length > 0) {
                    phone = acpInfo.parentInfo[0].LoginName;
                }
                var acpInfoItm = {
                    childId: acpInfo.childId,
                    childName: acpInfo.childName,
                    qqCount: acpInfo.qqCount,
                    phone: phone,
                };
                if (acpInfo.qqCount > 0) {
                    acpInfos1.push(acpInfoItm);
                } else {
                    acpInfos2.push(acpInfoItm);
                }
            }
            var acpInfos = [];
            for (var acpInfo1Index in acpInfos1) {
                acpInfos.push(acpInfos1[acpInfo1Index]);
            }
            for (var acpInfo2Index in acpInfos2) {
                acpInfos.push(acpInfos2[acpInfo2Index]);
            }
            var detail1Html = handlebarsHelp("#PrincipalAttClsStuDetailInfo1Html", acpInfos);
            $("#clsDetailContent").append(detail1Html);

            /* var detail1Htm2 = handlebarsHelp("#PrincipalAttClsStuDetailInfo1Html", acpInfos2);
             $("#clsDetailContent").append(detail1Htm2);*/
            //表头2
            var new_month = monthVal++;
            var new_year = yearVal;
            if (new_month > 12) {
                new_year++;
                new_month -= 12;
            }
            var new_date = new Date(new_year, new_month, 1);
            var last_date = (new Date(new_date.getTime() - 1000 * 60 * 60 * 24)).getDate();
            //get month last day end

            var daysInfo = [];
            for (var index = 1; index <= last_date; index++) {
                var item = { day: index };
                daysInfo.push(item);
            }
            var detail2Html = handlebarsHelp("#PrincipalAttClsStuDetailInfo2Html", daysInfo);
            $("#headerTr").append(detail2Html);


            //添加


            //内容
            for (var atInfoIndex in data.Attach.allChildsAtDetails) {
                var atInfo = data.Attach.allChildsAtDetails[atInfoIndex];

                var cqInfos = [];
                var childIdVal = atInfo.childId.toString();
                for (var index2 = 1; index2 <= last_date; index2++) {
                    var cqInfo = "-";
                    var cqStatus = "-";
                    for (var detailIndex in atInfo.attendenceDetials) {
                        if (atInfo.attendenceDetials[detailIndex]['day'] == index2) {
                            var attendenceDetail = atInfo.attendenceDetials[detailIndex]['attendenceDetialInfo'];
                            if (attendenceDetail.Status == 0) {
                                cqInfo = "缺勤";
                                cqStatus = "x";
                            } else if (attendenceDetail.Status == 1) {
                                cqInfo = "出勤";
                                cqStatus = "o";
                            } else {
                                cqInfo = "-";
                                cqStatus = "-";
                            }
                        }
                    }// loop data.Attach.attendenceDetailInfo end

                    var cqItem = {
                        day: index2.toString(),
                        cq: cqInfo,
                        childId: childIdVal,
                        cqStatus: cqStatus,
                    };
                    cqInfos.push(cqItem);
                }//loop 天数 end

                var detail3Html = handlebarsHelp("#PrincipalAttClsStuDetailInfo3Html", cqInfos);
                $("#valueTr" + atInfo.childId).append(detail3Html);

                for (var cqInfosIndex in cqInfos) {
                    var cqInfo2 = cqInfos[cqInfosIndex];
                    var canShu1 = cqInfo2.day + "cqInfo" + cqInfo2.childId;
                    var cqInfoObj = $("#" + canShu1);

                    if (cqInfo2.cqStatus == "o") {
                        cqInfoObj.attr("class", "label label-success");
                    } else if (cqInfo2.cqStatus == "x") {
                        cqInfoObj.attr("class", "label label-danger");
                    }
                }
            }// loop data.Attach.allChildsAtDetails end

            //2015年6月16日更改End--------------------------------------------------------
            /*for (var acpIndex in data.Attach.allChildAndParentInfos) {
                var acpInfo = data.Attach.allChildAndParentInfos[acpIndex];
                var childId = acpInfo.childId.toString();
                for (var index3 = 1; index3 <= last_date; index3++) {
                    var day = index3.toString();
                    var canShu1 = day + "cqInfo" + childId;
                    var canShu2 = day + "cqStatus" + childId;
                    var cqInfoObj = $("#" + canShu1);
                    var cqStatus = $("#" + canShu2).val();
                    if (cqStatus == "o") {
                        cqInfoObj.attr("class", "label label-success");
                    } else if (cqStatus == "x") {
                        cqInfoObj.attr("class", "label label-danger");
                    }
                }
            }*/

            hideDiv();
        }).fail(function (data) {
            hideDiv();
            alert(data.message);
        });
    };
    var GeneratePrincipalClsStuDetailInfo = function () {
        var childIdVal = $.cookie("paraChildId");
        var yearVal = $.cookie("paraYear");
        var monthVal = $.cookie("paraMonth");

        $("#attLoction").html("<i class='icon-home'></i>"
                                 + " 考勤"
                                 + "<a href='PrincipalAttendence.html'> > 考勤概况</a>"
                                 + "<a href='PrincipalAttClses.html'> > 考勤详情</a>"
                                 + "<a href='PrincipalAttClsDetail.html'> > 缺勤学生</a>"
                                 + "<a href='PrincipalClsStuDetail.html'> > 缺勤学生详情</a>");
        url = "/api/ClassesStudentsDetail/";
        showDiv();
        callApi(url, post, { childId: childIdVal, year: yearVal, month: monthVal }).done(function (data) {

            if (data.Attach.childDetailInfo.length <= 0) {
                alert("亲，还没有考勤信息~");
                redirect("PrincipalAttClsDetail.html");
                return;
            }
            var kindergartenNameVal;
            var classNameVal;
            var childNameVal;
            var imageUrlVal;
            var loginNameVal;
            var genderVal = "-";

            // childInfo begin
            for (var js2 in data.Attach.childDetailInfo) {
                var childInfo = data.Attach.childDetailInfo[js2]['childInfo'];
                childNameVal = childInfo.ChildName;
                imageUrlVal = "images/defaultChildImage.png";
                if (childInfo.ImageName != null && childInfo.ImageName != "") {
                    imageUrlVal = "http:\/\/babybus.emolbase.com\/" + childInfo.ImageName;
                }
                if (childInfo.Gender == 1) {
                    genderVal = "男";
                } else if (childInfo.Gender == 2) {
                    genderVal = "女";
                }
                classNameVal = data.Attach.childDetailInfo[js2]['className'];
                kindergartenNameVal = data.Attach.childDetailInfo[js2]['kindergartenName'];

            }//loop data.Attach.childDetailInfo end

            var childInfo = {
                kindergartenName: kindergartenNameVal,
                className: classNameVal,
                childName: childNameVal,
                imageUrl: imageUrlVal,
                gender: genderVal
            };
            $("#childNameHeader").text(childNameVal);
            var childHtml = handlebarsHelp("#PrincipalAttClsStuDetailHeaderHtml", childInfo);
            $("#PrincipalClsStuDetailHtml").append(childHtml);

            //出勤记录
            //detail
            var detailInfo1 = {
                kindergartenName: kindergartenNameVal,
                className: classNameVal,
                childName: childNameVal,
                month: monthVal,
                year: yearVal
            };
            var detail1Html = handlebarsHelp("#PrincipalAttClsStuDetailInfo1Html", detailInfo1);
            $("#attendanceDetailHtml").append(detail1Html);
            //attendenceDetail begin

            //get month last day begin
            var new_month = monthVal++;
            var new_year = yearVal;
            if (new_month > 12) {
                new_year++;
                new_month -= 12;
            }
            var new_date = new Date(new_year, new_month, 1);
            var last_date = (new Date(new_date.getTime() - 1000 * 60 * 60 * 24)).getDate();
            //get month last day end

            var daysInfo = [];
            var cqInfos = [];
            for (var index = 1; index <= last_date; index++) {
                var item = { day: index };
                daysInfo.push(item);
                var cqInfo = "-";
                for (var detailIndex in data.Attach.attendenceDetailInfo) {
                    if (data.Attach.attendenceDetailInfo[detailIndex]['day'] == index) {
                        var attendenceDetail = data.Attach.attendenceDetailInfo[detailIndex]['attendenceDetialInfo'];
                        if (attendenceDetail.Status == 0) {
                            cqInfo = "x";
                        } else if (attendenceDetail.Status == 1) {
                            cqInfo = "o";
                        } else {
                            cqInfo = "-";
                        }
                    }
                }// loop data.Attach.attendenceDetailInfo end

                var cqItem = {
                    day: index,
                    cq: cqInfo
                };
                cqInfos.push(cqItem);
            }
            var detail2Html = handlebarsHelp("#PrincipalAttClsStuDetailInfo2Html", daysInfo);
            $("#headerTr").append(detail2Html);

            var detail3Html = handlebarsHelp("#PrincipalAttClsStuDetailInfo3Html", cqInfos);
            $("#valueTr").append(detail3Html);

            for (var cqInfosIndex in cqInfos) {
                var cqInfo2 = cqInfos[cqInfosIndex];
                //x缺勤，-未知情况，o出勤
                if (cqInfo2.cq == "x") {
                    $("#" + cqInfo2.day).css({ "background-color": "#e25856", "color": "white" });
                } else if (cqInfo2.cq == "-") {
                    $("#" + cqInfo2.day).css({ "background-color": "#d3d7d4" });
                } else if (cqInfo2.cq == "o") {
                    $("#" + cqInfo2.day).css({ "background-color": "#94b86e", "color": "white" });
                }
            }
            //attendenceDetail end


            //parentInfo
            var htmlParent = handlebarsHelp("#ClsStuDetialParentInfoHtml", data.Attach.Parent);
            $("#PrincipalClsStuParentInfo").append(htmlParent);

            var htmlTeacher = handlebarsHelp("#ClsStuDetialTeacherInfoHtml", data.Attach.Teacher);
            $("#PrincipalClsStuTeacherInfo").append(htmlTeacher);
            hideDiv();
        }).fail(function (data) {
            hideDiv();
            alert(data.message);
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

            var paraAttendanceKinIdVal = $.cookie("paraAttendanceKinId");
            var paraAttendanceKinNameVal = $.cookie("paraAttendanceKinName");

            var kindergartenId = $.cookie("KindergartenId");
            var classIds = $.cookie("ClassId");

            var roleVal = $.cookie("Role");
            var permissionType = getPermissionType();
            var userId = $.cookie("UserId");

            var year = parseInt($("#yearVal").val());
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
                        GeneratePrincipalAttendanceInfo(firstKinId, permissionType, year);
                        $.cookie("paraAttendanceKinId", firstKinId, { expires: 1, path: '/' });
                        $.cookie("paraAttendanceKinName", firstKinName, { expires: 1, path: '/' });
                    } else {
                        $("#" + paraAttendanceKinIdVal).addClass("current");
                        $("#" + paraAttendanceKinIdVal).css({ "background": "#D9FFFF" });
                        $("#showKinNameInfo").text(paraAttendanceKinNameVal);
                        GeneratePrincipalAttendanceInfo(paraAttendanceKinIdVal, permissionType, year);
                    }
                } else if (type == "two") {//PrincipalAttClses.html
                    $("#" + paraAttendanceKinIdVal).addClass("current");
                    $("#" + paraAttendanceKinIdVal).css({ "background": "#D9FFFF" });
                    $("#showKinNameInfo").text(paraAttendanceKinNameVal);
                    GeneratePrincipalAttClsesInfo(paraAttendanceKinIdVal, permissionType);
                } else if (type == "three") {//PrincipalAttClsDetail.html
                    $("#" + paraAttendanceKinIdVal).addClass("current");
                    $("#" + paraAttendanceKinIdVal).css({ "background": "#D9FFFF" });
                    $("#showKinNameInfo").text(paraAttendanceKinNameVal);
                    GeneratePrincipalAttClsDetailInfo(paraAttendanceKinIdVal);
                } else if (type == "four") {
                    $("#kin" + paraAttendanceKinIdVal).addClass("current");
                    $("#kin" + paraAttendanceKinIdVal).css({ "background": "#D9FFFF" });
                    $("#showKinNameInfo").text(paraAttendanceKinNameVal);
                    GeneratePrincipalClsStuDetailInfo();
                }

            }).fail(function (data) {
                deferred.reject();
                console.log(data.message);
            });
        },
        GetPrincipalAttendanceInfo: function (kinId, kinName) {

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
            var permissionType = getPermissionType();
            var year = parseInt($("#yearVal").val());
            GeneratePrincipalAttendanceInfo(kinId, permissionType, year);
        },//

        GenerateExcelInfoForKindergarten: function () {

            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            var year = parseInt($("#yearVal").val());
            var kindergartenIdVal = $.cookie("paraAttendanceKinId");

            url = "/api/ExportExcel/" + "?excelType=1&KindergartenId="
            + kindergartenIdVal + "&Year=" + year;

            $("#excelKin").attr("disabled", true);
            window.open(url);
            $("#excelKin").removeAttr("disabled");
        },

        GenerateExcelInfoForClass: function () {

            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            var year = $.cookie("paraYear");
            var month = $.cookie("paraMonth");
            var kindergartenIdVal = $.cookie("paraAttendanceKinId");

            var roleVal = $.cookie("Role");

            url = "/api/ExportExcel/" + "?excelType=2&KindergartenId="
                + kindergartenIdVal + "&Year=" + year;

            $("#excelKin").attr("disabled", true);
            window.open(url);
            $("#excelKin").removeAttr("disabled");
        },
        GenerateExcelInfoForChild: function () {

            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            var kindergartenIdVal = $.cookie("paraAttendanceKinId");
            var calssIdVal = $.cookie("paraClassId");
            var yearVal = $.cookie("paraYear");
            var monthVal = $.cookie("paraMonth");

            var roleVal = $.cookie("Role");

            url = "/api/ExportExcel/" + "?excelType=3&KindergartenId="
             + kindergartenIdVal + "&Year=" + yearVal;

            $("#excelKin").attr("disabled", true);
            window.open(url);
            $("#excelKin").removeAttr("disabled");
        },
        GetPrincipalAllKindergartenInfo: function () {

            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            url = "/api/AllKindergartens/";
            showDiv();
            callApi(url, post).done(function (data) {

                var kindergartensInfo = [];
                for (var kinIndex in data.Attach.KindergartensInfo) {
                    var kinId = data.Attach.KindergartensInfo[kinIndex].kindergartenId;
                    var kinName = data.Attach.KindergartensInfo[kinIndex].kindergartenName;
                    var kinClsCount = "-";
                    for (var clsIndex in data.Attach.classCount) {
                        var kinIdByCls = data.Attach.classCount[clsIndex].kindergartenId;
                        if (kinIdByCls == kinId) {
                            kinClsCount = data.Attach.classCount[clsIndex].classCount;
                        }
                    }//loop data.Attach.classCount end
                    var item = { kindergartenId: kinId, kindergartenName: kinName, clsCount: kinClsCount };
                    kindergartensInfo.push(item);
                }//loop kindergartensInfo end

                var html = handlebarsHelp("#PrincipalAllKindergartenHtml", kindergartensInfo);
                $("#attendance").append(html);
                hideDiv();
            }).fail(function (data) {
                hideDiv();
                alert(data.message);
            });
        },
        GetClassAttendanceInfoByClassType: function () {

            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }
            var permissionType = getPermissionType();
            GeneratePrincipalAttClsesInfo($.cookie("paraAttendanceKinId"), permissionType);
        },

    };
}();