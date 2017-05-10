/*
 * Core script to handle all login specific things
 */

var PayRecord = function () {
    "use stict";

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

    var GeneratePayRecondMoth = function (kindIdVal, kindNameVal) {
        var monthChoice = $("#monthVal").val();
        var myData = new Date();
        var nowDataTime = myData.getMonth() + 1;
        var nowDataYear = myData.getFullYear();
        var yearVal = parseInt($("#yearVal").val());
        var kindIdVals = $.cookie("paraPayRecordsKinId");
        var kindNameVal = $.cookie("paraPayRecordsKinName");
        if (monthChoice != undefined) {
            if (monthChoice > nowDataTime && yearVal == nowDataYear) {
                alert("您选择的月份超过当前月份，无法显示信息！");
                redirect("PaymentRecordMonth.html");
                return;
            }
            var monthVal = monthChoice;
        } else {
            var monthVal = parseInt($("#monthVal").val());
        }
        var str = kindNameVal + yearVal + "年" + monthVal + "月" + "付费记录";
        $("#time").text(str);
        url = "/api/PayRecords/";
        showDiv();
        callApi(url, post, {
            PayRequestType: 1,
            KindergartenId: kindIdVals,
            Year: yearVal,
            Month: monthVal
        }).done(function (data) {
            var payRds = [];
            if (data.Attach.PayRecordItemClsCoutns == "") {
                alert("无班级信息！");
                var html = "";
                $("#payRecordMonths").html(html);
                return;
            }
            for (var joscls in data.Attach.PayRecordItemClsCoutns) {
                var payChildsNoVal = 0;
                var payChildsAlready = "0";
                var clsId = data.Attach.PayRecordItemClsCoutns[joscls].ClassId;
                if (data.Attach.PayChildsInfo != null && data.Attach.PayChildsInfo != "") {
                    for (var josKey in data.Attach.PayChildsInfo) {
                        if (clsId == data.Attach.PayChildsInfo[josKey].key) {
                            var payCls = data.Attach.PayChildsInfo[josKey];
                            var payChildsInfo = payCls.childsCount;
                            for (var josPayType in payChildsInfo) {
                                if (payChildsInfo[josPayType].key == 1) {
                                    payChildsAlready = payChildsInfo[josPayType].childs;
                                }
                            }
                        }
                    }
                }
                var payChildsNoVal = Number(data.Attach.PayRecordItemClsCoutns[joscls].ClassCount) - Number(payChildsAlready);
                var item = {
                    ClassName: data.Attach.PayRecordItemClsCoutns[joscls].ClassName,
                    ClassCount: data.Attach.PayRecordItemClsCoutns[joscls].ClassCount,
                    ChildsAlready: payChildsAlready,
                    ChildsNo: payChildsNoVal,
                    ClassId: clsId,
                };
                payRds.push(item);
                hideDiv();
            }
            if (monthChoice != undefined || yearVal != undefined) {
                var html = handlebarsHelp("#payRecordMounth", payRds);
                $("#payRecordMonths").html(html);
            } else {
                var html = handlebarsHelp("#payRecordMounth", payRds);
                $("#payRecordMonths").append(html);
            }

        }).fail(function (data) {
            alert(data.message);
        })
    };
    var GenerateEditPayChildsType = function () {
        var classIdVal = $.cookie("paraPayClsId");
        var classNameVal = $.cookie("parePayClsName");
        var yearVal = $.cookie("parePayYear");
        var monthVal = $.cookie("parePayMonth");
        var strHtml = classNameVal + yearVal + "年" + monthVal + "月付费详情";
        $("#time").text(strHtml);
        url = "/api/PayRecords/";
        showDiv();
        callApi(url, post, { PayRequestType: 2, ClassId: classIdVal, Year: yearVal, Month: monthVal }).done(function (data) {
            var html = handlebarsHelp("#paymentRecordChildInfo-tem", data.Attach.ChildInfo);
            $("#editPayTypeId").append(html);
            var index = 0;
            for (var chJos in data.Attach.ChildInfo) {
                var childIdVal = data.Attach.ChildInfo[chJos].ChildId;
                for (var jos in data.Attach.PayChildPayInfoType) {
                    if (childIdVal == data.Attach.PayChildPayInfoType[jos].ChildId) {
                        if (data.Attach.PayChildPayInfoType[jos].PayType == 1) {
                            $("#checkBoxs" + childIdVal).attr("checked", true);
                            $("#pay" + childIdVal).attr("class", "ribbon green");
                            $("#payChildTypeId" + childIdVal).text("已付费");
                            break;
                        } else {
                            $("#checkBoxs" + childIdVal).attr("checked", false);
                        }
                    } else {
                        continue;
                    }
                }
                index++;
            }
            hideDiv();
        }).fail(function (data) {
            alert(data.message);
        })
    };

    var url = "/api/Manament/";
    var get = "GET";
    var post = "POST";

    return {

        GeneratePayRecordsNavigationBar: function (type) {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            //use in the user.html 
            var deferred = $.Deferred();

            var paraKinIdVal = $.cookie("paraPayRecordsKinId");
            var paraKinNameVal = $.cookie("paraPayRecordsKinName");

            var kindergartenId = $.cookie("KindergartenId");
            var classIds = $.cookie("ClassId");

            var permissionType = getPermissionType();
            var userId = $.cookie("UserId");

            url = "/api/GenerateKindergartenInfo/";
            showDiv();
            callApi(url, post, { kindergartenId: kindergartenId, permissionType: permissionType, userId: userId }).done(function (data) {

                var html = handlebarsHelp("#statisticsNavPayRecords-template", data.Attach.Kindergartens);
                $("#payRecordsNavKindergartens").append(html);
                if (type == "one") {
                    if (paraKinIdVal == null) {
                        var firstKinId = data.Attach.Kindergartens[0].KindergartenId;
                        var firstKinName = data.Attach.Kindergartens[0].KindergartenName;
                        $("#payRecordsNavKindergartens li:first").addClass("current");
                        $("#payRecordsNavKindergartens li:first").css({ "background": "#D9FFFF" });
                        $("#showKinNameInfo").text(firstKinName);
                        //调用方法
                        $.cookie("paraPayRecordsKinId", firstKinId, { expires: 1, path: '/' });
                        $.cookie("paraPayRecordsKinName", firstKinName, { expires: 1, path: '/' });
                        GeneratePayRecondMoth(firstKinId, firstKinName);
                    } else {
                        $("#" + paraKinIdVal).addClass("current");
                        $("#" + paraKinIdVal).css({ "background": "#D9FFFF" });
                        $("#showKinNameInfo").text(paraKinNameVal);
                        //调用方法
                        GeneratePayRecondMoth(paraKinIdVal, paraKinNameVal);
                    }
                } else if (type == "two") {
                    $("#" + paraKinIdVal).addClass("current");
                    $("#" + paraKinIdVal).css({ "background": "#D9FFFF" });
                    //$("#showKinNameInfo").text(paraAttendanceKinNameVal);
                    GenerateEditPayChildsType();
                }
                hideDiv();
            }).fail(function (data) {
                deferred.reject();
                console.log(data.message);
            });
        },

        GenerateSubmit: function () {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }
            var checkDataAlready = $("input[name = 'name']:checkbox");
            var checkDataNo = $("input[name = 'name']").not("input:checked");
            var checkChildAlready = "";
            var checkChildNo = "";
            var kinderIdVal = $.cookie("paraPayRecordsKinId");
            var classIdVal = $.cookie("paraPayClsId");
            var yearVal = $.cookie("parePayYear");
            var monthVal = $.cookie("parePayMonth");
            $(checkDataAlready).each(function () {
                if ($(this).is("input:checked")) {
                    checkChildAlready += $(this).attr("value") + ",";
                } else {
                    checkChildNo += $(this).attr("value") + ",";
                }
            })
            url = "/api/PayRecords/";
            showDiv();
            callApi(url, post, { PayRequestType: 3, Year: yearVal, Month: monthVal, CheckChildAlready: checkChildAlready, CheckChildNo: checkChildNo, KindergartenId: kinderIdVal, ClassId: classIdVal }).done(function (data) {
                if (data.Attach.KK == 0) {
                    alert("更新成功！");
                    redirect("PaymentRecordChildInfo.html");
                } else {
                    alert("更新失败！");
                    redirect("PaymentRecordChildInfo.html");
                }
                hideDiv();
            }).fail(function (data) {
                alert(data.message);
            })
        },

        GenerateNoticeStatisticsYearMonthInfo: function (kinId, kinName) {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            //Year
            var date = new Date();
            var year = parseInt(date.getFullYear());
            $("#first").text(year);
            var second = year - 1;
            $("#second").text(second);
            var third = second - 1;
            $("#third").text(third);

            $("#yearVal").val(year);


            //Month
            var monthVals = [];
            for (var monthIndex = 1; monthIndex <= 12; monthIndex++) {
                var monthItm = { month: monthIndex };
                monthVals.push(monthItm);
            }
            var html = handlebarsHelp("#statisticsMonth-template", monthVals);
            $("#showMonth").empty();
            $("#showMonth").append(html);

            var month = date.getMonth();
            $("#monthVal").val(month + 1);
            $("#monthImg" + (month + 1)).attr("src", "images/noticeStatisticsCheck.png");
        },

        GetGeneratePayRecondMothInfo: function (kinId, kinName) {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            var allLi = $("#payRecordsNavKindergartens li");
            for (var liIndex = 0; liIndex < allLi.length; liIndex++) {
                $("#" + allLi[liIndex].id).removeClass("current");
                $("#" + allLi[liIndex].id).css({ "background": "" });
            }
            $("#" + kinId).addClass("current");
            $("#" + kinId).css({ "background": "#D9FFFF" });
            $("#showKinNameInfo").text(kinName);

            GeneratePayRecondMoth(kinId, kinName);
        },//

    };

}();