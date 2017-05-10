/*
 * Core script to handle all login specific things
 */

var Statistics = function () {

    "use strict";

    /* * * * * * * * * * * *
    * callApi is a ajax proxy
    * * * * * * * * * * * */
    var callApi = function (urlParam, method, dataParam) {
        return $.ajax({
            url: urlParam,
            type: method,
            data: dataParam,
            cache: false,
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
    
    var GenerateNoticeStatisticsInfo = function (kinId, kinName) {
        

        var year = parseInt($("#yearVal").val());
        var month = parseInt($("#monthVal").val());
        var date = new Date();
        var CurrentYear = parseInt(date.getFullYear());
        var CurrentMonth = parseInt(date.getMonth()) + 1;

        var statisticsTitle = kinName + year + "年" + month + "月消息数量统计";
        $("#headInfo").text(statisticsTitle);

        url = "/api/SendMessageStatistics/";
        showDiv();
        callApi(url, post, { kindergartenId: kinId, year: year, month: month }).done(function (data) {
            //alert(data.Attach.noticeStatistics);
            var noticeDatas = data.Attach.noticeStatistics;
            var xDatas = new Array();//数量
            var yDatas = new Array();//教师名称

            for (var noticeDatasIndex in noticeDatas) {
                var noticeData = noticeDatas[noticeDatasIndex];
                //alert(noticeData.month + "," + noticeData.noticeCount);
                var userId = noticeData.userId;
                var realName = noticeData.realName;
                var noticeCount = noticeData.noticeCount;
                var clsName = "未知";
                if (noticeData.clsName != null && noticeData.clsName != "") {
                    clsName = noticeData.clsName;
                } 
                var clsRealName = clsName + " - " + realName;
                if (realName != null && realName != "") {
                    yDatas.push(clsRealName);
                    xDatas.push(noticeCount);
                }
            }

            //绘图
            require.config({
                paths: {
                    echarts: './assets/js/ECharts/js'
                }
            });

            // Step:4 require echarts and use it in the callback.
            // Step:4 动态加载echarts然后在回调函数中开始使用，注意保持按需加载结构定义图表路径
            require(
                [
                    'echarts',
                    'echarts/chart/bar',
                    'echarts/chart/line'
                ],
                function (ec) {
                    //--- 折柱 ---
                    var myChart = ec.init(document.getElementById('main'));
                    var option = {
                        title: {
                            text: statisticsTitle,
                            //subtext: '统计数据',
                            //x: 'left'
                        },
                        tooltip: {
                            trigger: 'axis'
                        },
                        legend: {
                            data: ['消息数量']
                        },
                        toolbox: {
                            show: true,
                            feature: {
                                //mark: { show: true },
                                //dataView: { show: true, readOnly: false },
                                magicType: { show: true, type: ['line', 'bar'] },
                                restore: { show: true },
                                saveAsImage: { show: true }
                            }
                        },
                        calculable: true,
                        yAxis: [
                            {
                                type: 'value'
                            }
                        ],
                        xAxis: [
                            {
                                type: 'category',
                                data: yDatas
                            }
                        ],
                        series: [
                            {
                                "name": "消息数量",
                                "type": "bar",
                                "data": xDatas
                            }
                        ]
                    };
                    
                    myChart.setOption(option);

                }
            );
            hideDiv();
        }).fail(function (data) {
            hideDiv();
            alert("异常：" + data.message);
        });
    };



    var url = "/api/User/";
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

            var paraMessageKinIdVal = $.cookie("paraStatisticsKinId");
            var paraMessageKinNameVal = $.cookie("paraStatisticsKinName");

            var kindergartenId = $.cookie("KindergartenId");
            var classIds = $.cookie("ClassId");

            var permissionType = getPermissionType();
            var userId = $.cookie("UserId");

            url = "/api/GenerateKindergartenInfo/";
            callApi(url, post, { kindergartenId: kindergartenId, permissionType: permissionType, userId: userId }).done(function (data) {

                var html = handlebarsHelp("#statisticsNavKindergartens-template", data.Attach.Kindergartens);
                $("#statisticsNavKindergartens").append(html);
                if (type == "one") {
                    var firstKinId = data.Attach.Kindergartens[0].KindergartenId;
                    var firstKinName = data.Attach.Kindergartens[0].KindergartenName;
                    if (paraMessageKinIdVal == null) {
                        $("#statisticsNavKindergartens li:first").addClass("current");
                        $("#statisticsNavKindergartens li:first").css({ "background": "#D9FFFF" });
                        $("#showKinNameInfo").text(firstKinName);
                        GenerateNoticeStatisticsInfo(firstKinId, firstKinName);
                        $.cookie("paraStatisticsKinId", firstKinId, { expires: 1, path: '/' });
                        $.cookie("paraStatisticsKinName", firstKinName, { expires: 1, path: '/' });
                    } else {
                        $("#" + paraMessageKinIdVal).addClass("current");
                        $("#" + paraMessageKinIdVal).css({ "background": "#D9FFFF" });
                        $("#showKinNameInfo").text(paraMessageKinNameVal);
                        GenerateNoticeStatisticsInfo(paraMessageKinIdVal, paraMessageKinNameVal);
                    }
                }
            }).fail(function (data) {
                deferred.reject();
                console.log(data.message);
            });
        },
        GetNoticeStatisticsInfo: function (kinId, kinName) {

            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            var allLi = $("#statisticsNavKindergartens li");
            for (var liIndex = 0; liIndex < allLi.length; liIndex++) {
                $("#" + allLi[liIndex].id).removeClass("current");
                $("#" + allLi[liIndex].id).css({ "background": "" });
            }
            $("#" + kinId).addClass("current");
            $("#" + kinId).css({ "background": "#D9FFFF" });
            $("#showKinNameInfo").text(kinName);
            //var permissionType = getPermissionType();
            GenerateNoticeStatisticsInfo(kinId, kinName);
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
            for (var monthIndex = 1; monthIndex <= 12; monthIndex++)
            {
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
  


    };
}();