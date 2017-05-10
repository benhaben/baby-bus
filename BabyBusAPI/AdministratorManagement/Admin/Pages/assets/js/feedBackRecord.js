/*
 * Core script to handle all login specific things
 */

var FeedBackRecord = function () {
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


    var url = "/api/Manament/";
    var get = "GET";
    var post = "POST";

    return {

        GenertmentFeedBackInfo: function () {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            var statusOptions = [];
            //0,未处理；1,处理中；2,处理完成
            for (var statusIndex = 0; statusIndex < 3; statusIndex++) {
                var statusName = "-";
                if (statusIndex == 0) {
                    statusName = "未处理反馈";
                } else if (statusIndex == 1) {
                    statusName = "处理中反馈";
                } else if (statusIndex == 2) {
                    statusName = "已处理反馈";
                }
                var statusOptionsItem = {
                    statusNo: statusIndex,
                    statusName: statusName
                };
                statusOptions.push(statusOptionsItem);
            }//loop end
            var statusHtml = handlebarsHelp("#selectFbTypeHtml", statusOptions);
            $("#selectionFeedBackId").empty();
            $("#selectionFeedBackId").append(statusHtml);

            var HandleType = "";
            url = "/api/FeedBack/";
            showDiv();
            callApi(url, post, { recordType: 2 }).done(function (data) {

                var types = 1;
                for (var jos in data.Attach.FeedBackInfo) {
                    var index = 1;
                    var feedbackInfo = data.Attach.FeedBackInfo[jos];
                    if (feedbackInfo.key == 0) {
                        var handleType = "未处理反馈";
                    } else if (feedbackInfo.key == 1) {
                        var handleType = "处理中反馈";
                    } else if (feedbackInfo.key == 2) {
                        var handleType = "已处理反馈";
                    } else {
                        var handleType = "未知状态反馈";
                    }
                    var type = {
                        HandleType: handleType,
                        typeNo: types,
                    };
                    var htmlType = handlebarsHelp("#feedbackDetailType", type);
                    $("#fbDetailInfoId").append(htmlType)
                    var fbDetailInfo = feedbackInfo.feedbackDetailInfo;
                    for (var DetailInfo in fbDetailInfo) {
                        var item = {
                            FeedbackId: fbDetailInfo[DetailInfo].FeedbackId,
                            Content: fbDetailInfo[DetailInfo].Content,
                            userName: fbDetailInfo[DetailInfo].userName,
                            year: fbDetailInfo[DetailInfo].year,
                            month: fbDetailInfo[DetailInfo].month,
                            day: fbDetailInfo[DetailInfo].days,
                            Index: index,
                        };
                        index++;
                        var html = handlebarsHelp("#feedbackDetailInfoId", item);
                        $("#feed" + types).append(html);
                        if (feedbackInfo.key == 2) {
                            $("#checkbox" + fbDetailInfo[DetailInfo].FeedbackId).remove();
                            //$("#checkbox" + fbDetailInfo[DetailInfo].FeedbackId).attr("disable","disabled");
                            //$("#checkbox" + fbDetailInfo[DetailInfo].FeedbackId).css({ "-webkit-appearance": "none" });
                        }
                    }
                    if (fbDetailInfo == "" || fbDetailInfo == null) {
                        $("#feed" + types).css({ "display": "none" });
                    }
                    types++;
                }
                hideDiv();
            }).fail(function (data) {
                alert(data.message);
            })
        },

        GenertHandled: function () {
            var checkboxHandle = $("input[name = 'name']:checkbox");
            
            var checkBoxHandled = "";
            $(checkboxHandle).each(function () {
                if ($(this).is("input:checked")) {
                    checkBoxHandled += $(this).attr("value") + ",";
                }
            })
            if (checkBoxHandled.length <= 0) {
                alert("请选择已经处理完成的反馈信息！");
                return;
            }
            url = "/api/FeedBack/";
            callApi(url, post, { recordType: 4, CheckedBoxHandled: checkBoxHandled }).done(function (data) {
                if (data.Attach.HandledType == 0) {
                    alert("提交成功！");
                    redirect("FeedbackRecords.html");
                } else {
                    alert("提交失败！");
                }
            }).fail(function (data) {
                alert(data.message);
            })
        },

        GendertLodingHandle: function () {
            var checkbox = $("input[name = 'name']:checkbox");
            var checkBoxHandling = "";
            $(checkbox).each(function () {
                if($(this).is("input:checked")){
                    checkBoxHandling += $(this).attr("value") + ",";
                } 
            })
            if (checkBoxHandling.length <= 0) {
                alert("请选择要处理的反馈信息！");
                return;
            }
            url = "/api/FeedBack/";
            callApi(url, post, { recordType: 3, CheckBoxHandlling: checkBoxHandling }).done(function (data) {
                if (data.Attach.HandleType == 0) {
                    alert("提交成功！");
                    redirect("FeedbackRecords.html");
                } else {
                    alert("提交失败！");
                }

            }).fail(function (data) {
                alert(data.message);
            })
        },

        GenentSelectInfo: function (typeNo) {
            $("#fbDetailInfoId").empty();
            if (typeNo != null && typeNo != "") {
                if (typeNo == -1) {
                    var HandleType = "";
                    url = "/api/FeedBack/";
                    showDiv();
                    callApi(url, post, { recordType: 2 }).done(function (data) {

                        var types = 1;
                        for (var jos in data.Attach.FeedBackInfo) {
                            var index = 1;
                            var feedbackInfo = data.Attach.FeedBackInfo[jos];
                            if (feedbackInfo.key == 0) {
                                var handleType = "未处理反馈";
                            } else if (feedbackInfo.key == 1) {
                                var handleType = "处理中反馈";
                            } else if (feedbackInfo.key == 2) {
                                var handleType = "已处理反馈";
                            } else {
                                var handleType = "未知状态反馈";
                            }
                            var type = {
                                HandleType: handleType,
                                typeNo: types,
                            };
                            var htmlType = handlebarsHelp("#feedbackDetailType", type);
                            $("#fbDetailInfoId").append(htmlType)
                            var fbDetailInfo = feedbackInfo.feedbackDetailInfo;
                            for (var DetailInfo in fbDetailInfo) {
                                var item = {
                                    FeedbackId: fbDetailInfo[DetailInfo].FeedbackId,
                                    Content: fbDetailInfo[DetailInfo].Content,
                                    userName: fbDetailInfo[DetailInfo].userName,
                                    year: fbDetailInfo[DetailInfo].year,
                                    month: fbDetailInfo[DetailInfo].month,
                                    day: fbDetailInfo[DetailInfo].days,
                                    Index: index,
                                };
                                index++;
                                var html = handlebarsHelp("#feedbackDetailInfoId", item);
                                $("#feed" + types).append(html);
                                if (feedbackInfo.key == 2) {
                                    $("#checkbox" + fbDetailInfo[DetailInfo].FeedbackId).css({ "-webkit-appearance": "none" });
                                }
                            }
                            if (fbDetailInfo == "" || fbDetailInfo == null) {
                                $("#feed" + types).css({ "display": "none" });
                            }
                            types++;
                        }
                        hideDiv();
                    }).fail(function (data) {
                        alert(data.message);
                    })
                } else {
                    var checkText = $("#selectOptionTypeId").find("option:selected").text();
                    var titleName = {
                        HandleType: checkText,
                        typeNo: typeNo,
                    };
                    var htmlType = handlebarsHelp("#feedbackDetailType", titleName);
                    var index = 1;
                    $("#fbDetailInfoId").append(htmlType)
                    url = "/api/FeedBack/";
                    callApi(url, post, { recordType: 5, StatusTypes: typeNo }).done(function (data) {
                        if (data.Attach.SelectInfo == null || data.Attach.SelectInfo == "") {
                            alert("无反馈记录！");
                            return;
                        }
                        for (var jos in data.Attach.SelectInfo) {
                            var selectDetailInfo = data.Attach.SelectInfo[jos];
                            var item = {
                                FeedbackId: selectDetailInfo.FeedbackId,
                                Content: selectDetailInfo.Content,
                                userName: selectDetailInfo.userName,
                                year: selectDetailInfo.year,
                                month: selectDetailInfo.month,
                                day: selectDetailInfo.days,
                                Index: index,
                            };
                            index++;
                            var html = handlebarsHelp("#feedbackDetailInfoId", item);
                            $("#feed" + typeNo).append(html);
                            if (typeNo == 2) {
                                $("#checkbox" + selectDetailInfo.FeedbackId).css({ "-webkit-appearance": "none" });
                            }
                        }

                    }).fail(function (data) {
                        alert(data.message);
                    })

                }
            }
        }

    };

}();
