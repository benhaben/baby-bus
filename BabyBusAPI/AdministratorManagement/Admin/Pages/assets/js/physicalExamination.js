/*
 * Core script to handle all login specific things
 */

var PhysicalExamination = function () {

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

    var singleMedicalFraction = function (fraction, normal) {
        var fractionVal;
        if (normal.PhysicalExaEIems == 2 || normal.PhysicalExaEIems == 5 || normal.PhysicalExaEIems == 6) {
            if (fraction <= normal.OnePointsLower && fraction >= normal.OnePointsOnLine) {
                return fractionVal = 1;
            } else if (fraction <= normal.TwoPointsLower && fraction >= normal.TwoPointsOnLine) {
                return fractionVal = 2;
            } else if (fraction <= normal.ThreePointsLower && fraction >= normal.ThreePointsOnLine) {
                return fractionVal = 3;
            } else if (fraction <= normal.FourPointsLower && fraction >= normal.FourPointsOnLine) {
                return fractionVal = 4;
            } else if (fraction <= normal.FivePointsLower) {
                return fractionVal = 5;
            } else {
                return fractionVal = 0;
            }
        } else {
            if (fraction >= normal.OnePointsLower && fraction <= normal.OnePointsOnLine) {
                return fractionVal = 1;
            } else if (fraction >= normal.TwoPointsLower && fraction <= normal.TwoPointsOnLine) {
                return fractionVal = 2;
            } else if (fraction >= normal.ThreePointsLower && fraction <= normal.ThreePointsOnLine) {
                return fractionVal = 3;
            } else if (fraction >= normal.FourPointsLower && fraction <= normal.FourPointsOnLine) {
                return fractionVal = 4;
            } else if (fraction >= normal.FivePointsLower) {
                return fractionVal = 5;
            } else {
                return fractionVal = 0;
            }
        }
    };

    var individualAgeType = function () {
        var ageTypeJson = {
            "3.0": "3",
            "3.5": "3.5",
            "4.0": "4",
            "4.5": "4.5",
            "5.0": "5",
            "5.5": "5.5",
            "6.0": "6",
        };
        return ageTypeJson;
    };

    var checkIsNull = function (idVal) {
        var oneLowerVal = $("#oneLower" + idVal).val();
        var oneOnLineVal = $("#oneOnLine" + idVal).val();
        var twoLowerVal = $("#twoLower" + idVal).val();
        var twoOnLineVal = $("#twoOnLine" + idVal).val();
        var threeLowerVal = $("#threeLower" + idVal).val();
        var threeOnLineVal = $("#threeOnLine" + idVal).val();
        var fourLowerVal = $("#fourLower" + idVal).val();
        var fourOnLineVal = $("#fourOnLine" + idVal).val();
        var fiveLowerVal = $("#fiveLower" + idVal).val();
        var fiveOnLineVal = $("#fiveOnLine" + idVal).val();
        var selectAddAgeVal = $("#addIndividualStInfoId").val();
        var sendAddGenderVal = $("input[name='selectGender']:checked").val();
        if (oneLowerVal == "" || oneLowerVal == null ||
            oneOnLineVal == "" || oneOnLineVal == null ||
            twoLowerVal == "" || twoLowerVal == null ||
            twoOnLineVal == "" || twoOnLineVal == null ||
            threeLowerVal == "" || threeLowerVal == null ||
            threeOnLineVal == "" || threeOnLineVal == null ||
            fourLowerVal == "" || fourLowerVal == null ||
            fourOnLineVal == "" || fourOnLineVal == null ||
            fiveLowerVal == "" || fiveLowerVal == null ||
            fiveOnLineVal == "" || fiveOnLineVal == null ||
            selectAddAgeVal == -1 || sendAddGenderVal == null) {
            return false;
        }
        return true;
    };

    var projectType = function (e) {
        var type = "";
        if (e == 0) {
            return type = "身高";
        } else if (e == 1) {
            return type = "座位前屈";
        } else if (e == 2) {
            return type = "10米折返跑";
        } else if (e == 3) {
            return type = "立定跳远";
        } else if (e == 4) {
            return type = "网球掷远";
        } else if (e == 5) {
            return type = "双脚连续跳";
        } else if (e == 6) {
            return type = "走平衡木";
        } else {
            return type = "信息有误";
        }
    };

    var phyMedicalInfo = function () {
        var childIdVal = $.cookie("phyChildId");
        if (childIdVal == null || childIdVal == "") {
            alert("信息有误，请重新加载！");
            return;
        }
        url = "/api/PhyExaminationChildInfo/";
        callApi(url, post, { requestType: 0, ChildId: childIdVal }).done(function (data) {
            if (data.Attach.ChildPhyMedicalInfo == null || data.Attach.ChildPhyMedicalInfo == "") {
                alert("宝贝信息有误，请重新选择！");
                redirect("phyExaminationChildInfo.html");
                return;
            }
            for (var jos in data.Attach.ChildPhyMedicalInfo) {
                var childPhyInfo = data.Attach.ChildPhyMedicalInfo[jos];
                if (childPhyInfo.genders == 2) {
                    var genders = "女";
                } else if (childPhyInfo.genders == 1) {
                    var genders = "男";
                }
                if (childPhyInfo.images == null || childPhyInfo.images == "") {
                    var imageUrl = "images/defaultChildImage.png";
                } else {
                    var imageUrl = "http:\/\/babybus.emolbase.com\/" + childPhyInfo.images;
                }
                var item = {
                    imagUrl: imageUrl,
                    ChildId: childIdVal,
                    ChildName: childPhyInfo.childNames,
                    Gender: genders,
                    KindergartenName: childPhyInfo.kinderNames,
                    ClassName: childPhyInfo.clsNames,
                };
                var html = handlebarsHelp("#children-userinfo-template", item);
                $("#childInfo2").append(html);
                $("input[name = 'childGenders'][value = " + childPhyInfo.genders + "]").attr("checked", true);
                $("#childNameId").text(childPhyInfo.childNames);
            }
        }).fail(function (data) {
            alert(data.message);
        });
    };

    var phyChildInfo = function (kinId, kinName) {
        $("#test").text("kinId" + kinId);
        $("#kinName").text(kinName);
        $("#kinId").val(kinId);
        var roleVal = $.cookie("Role");
        var roleType;
        if (roleVal == "Administrator") {
            roleType = 0;
        } else if (roleVal == "President") {
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
        callApi(url, post, { Role: roleType, KindergartenId: kinId, Type: 1, ClassId: classIdVal }).done(function (data) {

            $("#allClassContent").empty();
            if (data.Attach.classAndChildInfo.length <= 0) {
                alert("无班级信息!");
                hideDiv();
                return;
            }
            var payTypes = "";
            var classIndex = 1;
            for (var classIndex in data.Attach.classAndChildInfo) {
                var clsInfos = data.Attach.classAndChildInfo[classIndex].classInfo;


                if (clsInfos.className.trim() == '删除') {
                    continue;
                } else {
                    var html = handlebarsHelp("#class-template", clsInfos);
                    $("#allClassContent").append(html);
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
                        imgUrl: imageUrl,
                        birthdayes: childOne.birthDays,
                        genders: childOne.genderInfo
                    };
                    var childsHtml = handlebarsHelp("#childs-template", childInfoSumItm);
                    $("#childContent" + clsInfos.classId).append(childsHtml);
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
    var phyChildType = function (kindId, phyPlanId) {
        $("#allClassContent").empty();
        $("#kinId").val(kinId);

        url = "/api/PhysicalExamination/";
        showDiv();
        callApi(url, post, { KindergardenId: kindId, DataType: 6, PlanPhyExmamintionId: phyPlanId }).done(function (data) {
            if (data.Attach.ClassAndChildDetail.length <= 0) {
                alert("无班级信息!");
                hideDiv();
                return;
            }
            for (var classIndex in data.Attach.ClassAndChildDetail) {
                var clsInfos = data.Attach.ClassAndChildDetail[classIndex];
                var itemClass = {
                    classId: clsInfos.ClassId,
                    className: clsInfos.ClassName
                };
                var html = handlebarsHelp("#class-template", itemClass);
                $("#allClassContent").append(html);
                var childInfo = clsInfos.childInfo;
                if (childInfo == null || childInfo == "") {
                    continue;
                }
                for (var childJos in childInfo) {
                    var childExaType;
                    var titleType;
                    var childDetail = childInfo[childJos];
                    var childPhyExaInfo = childDetail.planPhyExaInfoType;
                    if (childPhyExaInfo == null || childPhyExaInfo == "") {
                        childExaType = "进行体制检测";
                        titleType = "未体测";
                    } else {
                        childExaType = "查看体测报告";
                        titleType = "已体测";
                    }
                    var itemChild = {
                        childId: childDetail.ChildId,
                        childName: childDetail.ChildName,
                        classId: clsInfos.ClassId,
                        kinId: kindId,
                        phyTypeTitle: titleType,
                        phyExamintionType: childExaType,
                        birthdayes: childDetail.Birthday,
                        genders: childDetail.Gender,
                        planId: phyPlanId
                    };
                    var childsHtml = handlebarsHelp("#childs-template", itemChild);
                    $("#childContent" + clsInfos.ClassId).append(childsHtml);
                    if (titleType == "未体测") {
                        $("#pay" + childDetail.ChildId).attr("class", "ribbon red");
                    } else {
                        $("#pay" + childDetail.ChildId).attr("class", "ribbon green");
                    }
                }

            }//loop data.Attach.classAndChildInfo end
            hideDiv();
        }).fail(function (data) {
            hideDiv();
            deferred.reject();
            console.log(data.message);
        });
    };

    var phyExaminationPlanInfo = function (kindId, kinderName, times) {
        $("#allClassContent").empty();
        var planIdVal = $.cookie("phyPlanId");
        $("#kinName").text($.cookie("phyUserInfoKinName"));
        url = "/api/PhysicalExamination/";
        showDiv();
        callApi(url, post, { DataType: 5, KindergardenId: kindId, Year: times }).done(function (data) {
            if (data.Attach.PlanPhyInfo == null || data.Attach.PlanPhyInfo == "") {
                alert("没有体测计划信息！");
                $("#selectChildExaPhy").css({ "display": "none" });
                hideDiv();
                return;
            }
            $("#selectChildExaPhy").css({ "display": "block" });
            var index = 0;
            var planPhyId;
            var itemInfo = [];
            for (var jos in data.Attach.PlanPhyInfo) {
                var planPhyInfo = data.Attach.PlanPhyInfo[jos];
                if (index == 0) {
                    planPhyId = planPhyInfo.p.PhysicalExaminationPlaneId;
                }
                var titleInfo = planPhyInfo.p.PlanTitle;
                if (titleInfo.length > 15) {
                    titleInfo = titleInfo.substring(0, 15) + "…";
                }
                var planeYears = planPhyInfo.years;
                var planeMonth = planPhyInfo.months;
                var planeDays = planPhyInfo.days;
                var item = {
                    phyExaminationId: planPhyInfo.p.PhysicalExaminationPlaneId,
                    titles: titleInfo,
                    years: planeYears,
                    montn: planeMonth,
                    days: planeDays
                };
                itemInfo.push(item);
                index++;
            }
            var html = handlebarsHelp("#selectTileHTML", itemInfo);
            $("#selectTileId").empty();
            $("#selectTileId").append(html);
            if (planIdVal != null && planIdVal != "") {
                $("#selectTileId").val(planIdVal);
                phyChildType(kindId, planIdVal);
            } else {
                $("#selectTileId").val(planPhyId);
                phyChildType(kindId, planPhyId);
            }
            hideDiv();
        }).fail(function (data) {
            alert(data.message);
        });
    };

    var option = { //可以去官网上根据每个案例不同的option去写各种图形
        // title: {   //标题
        //     text: 'OP系统点击量'
        // },
        tooltip: {   //提示框，鼠标悬浮交互时的信息提示
            show: true,
            trigger: 'axis'
        },
        //toolbox: {
        //   // show: true,
        //    feature: {
        //        mark: { show: false,
        //            lineStyle :{
        //                width: 2,
        //                color: '#1e90ff',
        //                type: 'dashed'
        //            },
        //        },
        //        dataView: { show: true, readOnly: false },
        //        restore: { show: true },
        //        saveAsImage: { show: true },
                
                
        //    }
        //},
        /* legend: {    //图例，每个<a href="http://www.suchso.com/catalog.asp?tags=ECharts%E6%95%99%E7%A8%8B" class="keylink" title=" 图表" target="_blank">图表</a>最多仅有一个图例
             x: 'center',
             data: ['体制检测']
         },*/
        polar: [{    //极坐标 
            indicator: [{ text: '往返跑', max: 5 },
                        { text: '立定跳远', max: 5 },
                        { text: '网球掷远', max: 5 },
                        { text: '双脚连续跳', max: 5 },
                        { text: '走平衡木', max: 5 },
                        { text: '身高', max: 5 },
                        { text: '身高标准体重', max: 5 },
                        { text: '体前屈', max: 5 }
            ],
            radius: 100,
            startAngle: 135  // 改变雷达图的旋转度数
        }],
        series: [{         // 驱动图表生成的数据内容数组，数组中每一项为一个系列的选项及数据
            name: '总点击量',
            type: 'radar',
             itemStyle: {//图形样式，可设置图表内图形的默认样式和强调样式（悬浮时样式）：
                 normal: {
                     lineStyle: {
                         type:'solid',
                         color: "#ccc",
                         borderWidth:'200px'
                     },
                     linkStyle: {
                         type: 'line',
                         color: '#5182ab',
                         borderWidth: '200px'
                     },
                     nodeStyle: {
                         borderColor: '#000000',
                         borderWidth: '200px'
                     }
                 }
             },
            data: [{
                value: [],      //外部加载，也可以通过ajax去加载外部数据。
                name: ''

            }]

        }]
    };

    var phyExaminationReport = function (type) {
        var childIdVal = $.cookie("phyChildId");
        var kinderNameVal = $.cookie("phyUserInfoKinName");
        var planIdVal = $.cookie("phyPlanId");
        var childGenderVal = $.cookie("phyGender");
        var childNameVal = $.cookie("phyChildName");
        var genderVal;
        var gradVal;
        if (childGenderVal == 1) {
            genderVal = "男";
        } else {
            genderVal = "女";
        }
        url = "/api/PhyExaminationChildInfo/";
        callApi(url, post, { requestType: 2, ChildId: childIdVal, PhyExaminationPlanId: planIdVal }).done(function (data) {
            if (data.Attach.ChildPhyExaminationReport != null && data.Attach.ChildPhyExaminationReport != "") {
                for (var jos in data.Attach.ChildPhyExaminationReport) {
                    var weightDescriptionVal;
                    var heightDescriptionVal;
                    var sitDownDescrtiptionVal;
                    var powerDescriptionVal;
                    var speedDescriptionVal;
                    var balanceDescriptionVal;
                    var phyReport = data.Attach.ChildPhyExaminationReport[jos];
                    if (phyReport.pr.Grade == 1) {
                        gradVal = "优秀";
                    } else if (phyReport.pr.Grade == 2) {
                        gradVal = "良好";
                    } else if (phyReport.pr.Grade == 3) {
                        gradVal = "合格";
                    } else {
                        gradVal = "不合格";
                    }
                    if (phyReport.pr.WeightDesc == "-1") {
                        weightDescriptionVal = "体重大幅度低于正常范围，与热量吸收不足或饮食结构不合理有关";
                    } else if (phyReport.pr.WeightDesc == "-3") {
                        weightDescriptionVal = "体重低于正常范围，体型偏瘦，日常饮食注意增加热量的摄入，多参加体育锻炼";
                    } else if (phyReport.pr.WeightDesc == "5") {
                        weightDescriptionVal = "体重在正常范围内，体型匀称，营养状况良好";
                    } else if (phyReport.pr.WeightDesc == "+3") {
                        weightDescriptionVal = "体重超过正常范围，体型有发胖趋势，要合理调整饮食，多参加体育锻炼";
                    } else if (phyReport.pr.WeightDesc == "+1") {
                        weightDescriptionVal = "体重大幅度超过正常范围，体型肥胖，与热量摄入过剩或饮食结构不合理有关";
                    } else {
                        weightDescriptionVal = "数据不合理，请联系管理员";
                    }
                    if (phyReport.pr.HeightScore >= 1 && phyReport.pr.HeightScore <= 2) {
                        heightDescriptionVal = "较低";
                    } else if (phyReport.pr.HeightScore == 3) {
                        heightDescriptionVal = "一般";
                    } else if (phyReport.pr.HeightScore >= 4 && phyReport.pr.HeightScore <= 5) {
                        heightDescriptionVal = "较高";
                    } else {
                        heightDescriptionVal = "数据不合理，请联系管理员";
                    }
                    if (phyReport.pr.SeatBodyFlexionScore == 1) {
                        sitDownDescrtiptionVal = "躯干、腰部和髋关节等活动的幅度很小，相应部位韧带和肌肉的伸展性和弹性也很差。";
                    } else if (phyReport.pr.SeatBodyFlexionScore == 2) {
                        sitDownDescrtiptionVal = "躯干、腰部和髋关节等活动的幅度较小，相应部位韧带和肌肉的伸展性和弹性也较差。";
                    } else if (phyReport.pr.SeatBodyFlexionScore == 3) {
                        sitDownDescrtiptionVal = "躯干、腰部和髋关节等活动的幅度一般，相应部位的韧带、肌肉的伸展性和弹性一般。";
                    } else if (phyReport.pr.SeatBodyFlexionScore == 4) {
                        sitDownDescrtiptionVal = "躯干、腰部和髋关节等活动的幅度较大，相应部位的韧带和肌肉具有较好的伸展性和弹性。";
                    } else if (phyReport.pr.SeatBodyFlexionScore == 5) {
                        sitDownDescrtiptionVal = "躯干、腰部和髋关节等活动的幅度大，相应部位的韧带和肌肉具有很好的伸展性和弹性。";
                    } else {
                        sitDownDescrtiptionVal = "数据不合理，请联系管理员";
                    }
                    var powerVal = phyReport.pr.StandingLongJumpScore + phyReport.pr.TennisThrowFarScore + phyReport.pr.JumpWithBothFeetScore;
                    if (powerVal < 9) {
                        powerDescriptionVal = "不同种类力量发展水平均不高，力量素质较差。";
                    } else if ((powerVal > 9) && (phyReport.pr.StandingLongJumpScore == 1 || phyReport.pr.TennisThrowFarScore == 1 || phyReport.pr.JumpWithBothFeetScore == 1)) {
                        powerDescriptionVal = "力量素质一般，但不同种类力量发展不均衡。";
                    } else if (powerVal > 12) {
                        powerDescriptionVal = "力量素质好，不同种类力量发展均衡。";
                    } else if (powerVal >= 9 && powerVal <= 12) {
                        powerDescriptionVal = "力量素质一般，不同种类力量发展均衡。";
                    } else if (phyReport.pr.StandingLongJumpScore == 5 || phyReport.pr.TennisThrowFarScore == 5) {
                        if (phyReport.pr.StandingLongJumpScore == 5) {
                            if ((phyReport.pr.TennisThrowFarScore >= 1 && phyReport.pr.TennisThrowFarScore <= 2) && (phyReport.pr.JumpWithBothFeetScore >= 1 && phyReport.pr.JumpWithBothFeetScore <= 2)) {
                                powerDescriptionVal = "力量素质较差，但爆发力明显强于其他种类力量。";
                            }
                        } else if (phyReport.pr.TennisThrowFarScore == 5) {
                            if ((phyReport.pr.JumpWithBothFeetScore >= 1 && phyReport.pr.JumpWithBothFeetScore <= 2) && (phyReport.pr.StandingLongJumpScore >= 1 && phyReport.pr.StandingLongJumpScore <= 2)) {
                                powerDescriptionVal = "力量素质较差，但爆发力明显强于其他种类力量。";
                            }
                        }
                    } else if (phyReport.pr.JumpWithBothFeetScore == 5) {
                        powerDescriptionVal = "力量素质较差，但双脚协调用力发展水平较高。";
                    } else {
                        powerDescriptionVal = "数据不合理，请联系管理员";
                    }
                    var speedSorce = phyReport.pr.TenShuttleRunScore + phyReport.pr.JumpWithBothFeetScore;
                    if (speedSorce <= 2) {
                        speedDescriptionVal = "速度、灵敏、协调性很差,";
                    } else if (((phyReport.pr.TenShuttleRunScore == 1 && phyReport.pr.JumpWithBothFeetScore > 2) ||
                    (phyReport.pr.TenShuttleRunScore > 2 && phyReport.pr.JumpWithBothFeetScore == 1)) ||
                    (phyReport.pr.JumpWithBothFeetScore == 2 || phyReport.pr.TenShuttleRunScore == 2)) {
                        speedDescriptionVal = "速度、灵敏、协调性较差,";
                    } else if (5 <= speedSorce && speedSorce <= 6) {
                        speedDescriptionVal = "速度、灵敏、协调性一般,";
                    } else if (7 <= speedSorce && speedSorce <= 8) {
                        speedDescriptionVal = "有较好的速度、灵敏、协调性素质,";
                    } else if (9 <= speedSorce && speedSorce <= 10) {
                        speedDescriptionVal = "有良好的速度、灵敏、协调性素质,";
                    } else {
                        speedDescriptionVal = "速度、灵敏、协调性素质数据有误,请联系管理员";
                    }
                    if (phyReport.pr.WalkTheBalanceBeamScore == 1) {
                        balanceDescriptionVal = "平衡能力很差。";
                    } else if (phyReport.pr.WalkTheBalanceBeamScore == 2) {
                        balanceDescriptionVal = "平衡能力较差。";
                    } else if (phyReport.pr.WalkTheBalanceBeamScore == 3) {
                        balanceDescriptionVal = "平衡能力一般。";
                    } else if (phyReport.pr.WalkTheBalanceBeamScore == 4) {
                        balanceDescriptionVal = "有较好的平衡能力。";
                    } else if (phyReport.pr.WalkTheBalanceBeamScore == 5) {
                        balanceDescriptionVal = "有良好的平衡能力。";
                    } else {
                        balanceDescriptionVal = "平衡力数据有误，请联系管理员";
                    }
                    var item = {
                        ChildId: childIdVal,
                        phyExaminationTitle: phyReport.PlanTitle,
                        ChildNames: childNameVal,
                        ChildGender: genderVal,
                        Age: phyReport.pr.AgeGroup,
                        TestTime: phyReport.years + "-" + phyReport.months + "-" + phyReport.days,
                        phyMedicalId: phyReport.pr.PhysicalExaminationResultsId,
                        KindergartenName: kinderNameVal,
                        Height: phyReport.pr.Height,
                        HeightSorce: phyReport.pr.HeightScore,
                        Weights: phyReport.pr.Weights,
                        WeightSorce: phyReport.pr.WeightsScore,
                        TenmRun: phyReport.pr.TenShuttleRun,
                        TenmRunSorce: phyReport.pr.TenShuttleRunScore,
                        SitReach: phyReport.pr.SeatBodyFlexion,
                        SitReachSorce: phyReport.pr.SeatBodyFlexionScore,
                        StandingLongJump: phyReport.pr.StandingLongJump,
                        StandingLongJumpSorce: phyReport.pr.StandingLongJumpScore,
                        JumpBothFeet: phyReport.pr.JumpWithBothFeet,
                        JumpBothFeetSorce: phyReport.pr.JumpWithBothFeetScore,
                        TennisThrowFar: phyReport.pr.TennisThrowFar,
                        TennisThrowFarSorce: phyReport.pr.TennisThrowFarScore,
                        WalkTheBalance: phyReport.pr.WalkTheBalanceBeam,
                        WalkTheBalanceSorce: phyReport.pr.WalkTheBalanceBeamScore,
                        Grade: gradVal,
                        weightDescription: weightDescriptionVal,
                        heightDescription: heightDescriptionVal,
                        sitDownDescrtiption: sitDownDescrtiptionVal,
                        powerDescription: powerDescriptionVal,
                        speedDescription: speedDescriptionVal,
                        balanceDescription: balanceDescriptionVal,
                        TotalScore: phyReport.pr.Fraction
                    };
                    // $("#printPhyRecordId").append(html);
                    option.series[0].data[0].value = [phyReport.pr.TenShuttleRunScore,
                                                      phyReport.pr.StandingLongJumpScore,
                                                      phyReport.pr.TennisThrowFarScore,
                                                      phyReport.pr.JumpWithBothFeetScore,
                                                      phyReport.pr.WalkTheBalanceBeamScore,
                                                      phyReport.pr.HeightScore,
                                                      phyReport.pr.WeightsScore,
                                                      phyReport.pr.SeatBodyFlexionScore];
                    option.series[0].data[0].name = "体制检测";
                   
                    var html = handlebarsHelp("#phyExaminationReportHTML", item);
                    $("#childInfo2").append(html);
                    var myChart = echarts.init(document.getElementById('radarId' + childIdVal));
                    myChart.setOption(option, true);

                    if (type == "print") {
                        //打印begin
                        $("#recordsIdss").removeAttr("class");
                        $("#delete_child_id").removeAttr("class");

                       // window.print();
                        //打印end
                    }
                    
                }
            }
        }).fail(function (data) {
            alert(data.message);
        });
    }

    var url = "/api/User/";
    var get = "GET";
    var post = "POST";
    var tcTypeNum = 7;//体测项目个数

    return {
        //user.html     
        GenertPhysicalExaminationData: function () {
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }
            var selectVal = $("#selectTypeId").val();
            $("#weigherHeighId").empty();
            url = "/api/PhysicalExamination/";
            showDiv();
            callApi(url, post, { DataType: 1, SelectInfo: selectVal }).done(function (data) {
                var dataInfo = data.Attach.HeightWeightDetail;
                for (var jos in dataInfo) {
                    if (dataInfo[jos].key == 0) {
                        var genderInfo = "女";
                    } else {
                        var genderInfo = "男";
                    }
                    var itemTitle = {
                        gender: dataInfo[jos].key,
                        typeName: genderInfo,
                    };
                    var html = handlebarsHelp("#weigherHeighTitleHtml", itemTitle);
                    $("#weigherHeighId").append(html);
                    var detailInfo = dataInfo[jos].detailInfo;
                    var detail = [];
                    for (var detailJos in detailInfo) {

                        var item = {
                            Gender: genderInfo,
                            HeightOffline: detailInfo[detailJos].HeightOffline,
                            HeightOnline: detailInfo[detailJos].HeightOnline,
                            Thin1PintsOnLine: detailInfo[detailJos].Thin1PintsOnLine,
                            Thin3PointsOffLine: detailInfo[detailJos].Thin3PointsOffLine,
                            Thin3PointsOnLine: detailInfo[detailJos].Thin3PointsOnLine,
                            Normal5PointsOffLine: detailInfo[detailJos].Normal5PointsOffLine,
                            Normal5PointsOnLine: detailInfo[detailJos].Normal5PointsOnLine,
                            Fat3PointsLower: detailInfo[detailJos].Fat3PointsLower,
                            Fat3PointsOnLine: detailInfo[detailJos].Fat3PointsOnLine,
                            Fat1PointsLower: detailInfo[detailJos].Fat1PointsLower,
                        };
                        detail.push(item);
                    }
                    var htmlDetail = handlebarsHelp("#weigherHeighDataHtml", detail);
                    $("#weigherHeighId" + detailInfo[detailJos].Gender).append(htmlDetail);

                }
                hideDiv();
            }).fail(function (data) {
                alert(data.message);
            });
        },
        GenertAddWeightForStandarHeighInfo: function () {
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }
            var typeStly = 0;
            var userIdVal = $.cookie("UserId");
            var genderInfo = $("input[name = 'genderid']:checked").val();
            var heightOfflineVal = $("#heigh1").val();
            var heightOnlineVal = $("#heigh2").val();
            var thin1PintsOnLineVal = $("#thin1onId").val();
            var thin3PointsOffLineVal = $("#thin3loffId").val();
            var thin3PointsOnLineVal = $("#thin3onId").val();
            var normal5PointsOffLineVal = $("#normal5offId").val();
            var normal5PointsOnLineVal = $("#normal5OnId").val();
            var fat3PointsLowerVal = $("#fat3offId").val();
            var fat3PointsOnLineVal = $("#fat3OnId").val();
            var fat1PointsLowerVal = $("#fat1OffId").val();
            if (genderInfo == null || genderInfo == ""
                || heightOfflineVal == null || heightOfflineVal == ""
                || heightOnlineVal == null || heightOnlineVal == ""
                || thin1PintsOnLineVal == null || thin1PintsOnLineVal == ""
                || thin3PointsOffLineVal == null || thin3PointsOffLineVal == ""
                || thin3PointsOnLineVal == null || thin3PointsOnLineVal == ""
                || normal5PointsOffLineVal == null || normal5PointsOffLineVal == ""
                || normal5PointsOnLineVal == null || normal5PointsOnLineVal == ""
                || fat3PointsLowerVal == null || fat3PointsLowerVal == ""
                || fat3PointsOnLineVal == null || fat3PointsOnLineVal == ""
                || fat1PointsLowerVal == null || fat1PointsLowerVal == "") {
                alert("请填写完整信息！");
                return typeStly;
            }
            url = "/api/AadWeighForStandarHeigh/";
            showDiv();
            callApi(url, post, { UserId: userIdVal, Gender: genderVal, HeightOffline: heightOfflineVal, HeightOnline: heightOnlineVal, Thin1PintsOnLine: thin1PintsOnLineVal, Thin3PointsOffLine: thin3PointsOffLineVal, Thin3PointsOnLine: thin3PointsOnLineVal, Normal5PointsOffLine: normal5PointsOffLineVal, Normal5PointsOnLine: normal5PointsOnLineVal, Fat3PointsLower: fat1PointsLowerVal, Fat3PointsOnLine: fat3PointsOnLineVal, Fat1PointsLower: fat1PointsLowerVal }).done(function (data) {
                if (data.Attach.Status == 0) {
                    alert("添加成功！");
                    var obox = document.getElementById("box1");
                    var oBox = document.getElementById("Mybox1");
                    obox.style.display = "none";
                    oBox.style.display = "none";
                    redirect("WeightForStandardHeight.html");
                } else {
                    alert("添加失败，请重新添加或者联系管理员！");
                }
            }).fail(function (data) {
                alert(data.message);
            })
        },

        GenertDetailIndividualInfo: function () {
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }
            var ageTypeSelection = individualAgeType();
            var addProjectTitle = [];
            var addAgeTitle = [];
            for (var i = 0; i < tcTypeNum; i++) {
                var items = {
                    type: i,
                    TypeName: projectType(i)
                };
                addProjectTitle.push(items);
            }
            var htmlp = handlebarsHelp("#addIndividualStandarInfoHtml", addProjectTitle);
            $("#addWeigherHeighDataId").append(htmlp);
            for (var jos in ageTypeSelection) {
                var itema = {
                    type: jos,
                    AgeType: ageTypeSelection[jos],
                };
                addAgeTitle.push(itema);
            }
            var htmla = handlebarsHelp("#selectAgeTypeHtml", addAgeTitle);
            $("#selectTypeId").append(htmla);
            $("#addIndividualStInfoId").append(htmla);

        },

        GenertIndividualStandardInfo: function () {
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }
            var ageTypeSelection = individualAgeType();
            $("#individualStandDetailInfoId").empty();
            var selectAgeVal = $("#selectTypeId").val();
            var genderInfo = '';
            var index = 10;
            url = "/api/PhysicalExamination/";
            callApi(url, post, { DataType: 2, SelectAge: selectAgeVal }).done(function (data) {

                var individuaStInfo = data.Attach.IndividualSt;
                if (individuaStInfo == null || individuaStInfo == "") {
                    alert("无评分标准，请添加评分标准!");
                    return;
                }
                for (var jos in individuaStInfo) {
                    var individualStInfo = individuaStInfo[jos];
                    var age = individualStInfo.age;
                    var ageStr = String(age);
                    index++;
                    var itemAgeInfo = {
                        Age: ageStr,
                        AgeId: index
                    };
                    var htmlAge = handlebarsHelp("#individualStAgeHtml", itemAgeInfo);
                    $("#individualStandDetailInfoId").append(htmlAge);
                    if (index == 0) {
                        $("#ageContent" + ageStr).css({ "display": "block" });
                    } else {
                        $("#ageContent" + ageStr).css({ "display": "none" });
                    }
                    var individualStuGenderInfo = individualStInfo.individualGender;

                    for (var genJos in individualStuGenderInfo) {
                        var individualGenDetail = individualStuGenderInfo[genJos];
                        var gender = String(individualGenDetail.gender);
                        if (individualGenDetail.gender == 2) {
                            genderInfo = "女";
                        } else {
                            genderInfo = "男";
                        }
                        var itemGenderInfo = {
                            AgeType: index,
                            Gender: genderInfo,
                            GenderType: gender
                        };
                        var htmlGen = handlebarsHelp("#individualStGenderHtml", itemGenderInfo);
                        $("#ageContent" + index).append(htmlGen);
                        if (genderInfo == "男") {
                            $("#individualStandardId" + index + gender).css({ "border": "solid 1px #97CBFF" });
                        } else if (genderInfo = "女") {
                            $("#individualStandardId" + index + gender).css({ "border": "solid 1px #FFC0CB " });
                        }
                        var individualStuDetailInfo = individualGenDetail.individualStaDetail;
                        var detail = [];
                        for (var detailJos in individualStuDetailInfo) {
                            if (individualStuDetailInfo[detailJos].FivePointsOnLine == 0) {
                                var fiveDetailInfo = "以下";
                            } else if (individualStuDetailInfo[detailJos].FivePointsOnLine == 1) {
                                var fiveDetailInfo = "以上";
                            }
                            var item = {
                                PhysicalExaEIems: projectType(individualStuDetailInfo[detailJos].PhysicalExaEIems),
                                Age: individualStuDetailInfo[detailJos].Age,
                                Gender: genderInfo,
                                OnePointsLower: individualStuDetailInfo[detailJos].OnePointsLower,
                                OnePointsOnLine: individualStuDetailInfo[detailJos].OnePointsOnLine,
                                TwoPointsLower: individualStuDetailInfo[detailJos].TwoPointsLower,
                                TwoPointsOnLine: individualStuDetailInfo[detailJos].TwoPointsOnLine,
                                ThreePointsLower: individualStuDetailInfo[detailJos].ThreePointsLower,
                                ThreePointsOnLine: individualStuDetailInfo[detailJos].ThreePointsOnLine,
                                FourPointsLower: individualStuDetailInfo[detailJos].FourPointsLower,
                                FourPointsOnLine: individualStuDetailInfo[detailJos].FourPointsOnLine,
                                FivePointsLower: individualStuDetailInfo[detailJos].FivePointsLower,
                                FivePointsOnLine: fiveDetailInfo
                            };
                            detail.push(item);
                        }
                        var html = handlebarsHelp("#individuaStDataHtml", detail);
                        $("#individualStandardId" + index + gender).append(html);
                    }
                }
            }).fail(function (data) {
                alert(data.message);
            });
        },

        GenertSendAddIndividualStDetailInfo: function () {
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            var detailInfo = $("#addWeigherHeighDataId").find("tr");
            var sendAddIndividualStInfo = "";
            var selectAddAgeVal = $("#addIndividualStInfoId").val();
            var sendAddGenderVal = $("input[name='selectGender']:checked").val();
            if (selectAddAgeVal == 0) {
                alert("请填写完整信息!");
                return;
            }
            for (var i = 0; i < tcTypeNum; i++) {
                if (!checkIsNull(i)) {
                    alert("请填写完整信息!");
                    return;
                }
                var individualProjectTypeVal = $("#individualProjectType" + i).val();//项目编号
                var oneLowerVal = $("#oneLower" + i).val();
                var oneOnLineVal = $("#oneOnLine" + i).val();
                var twoLowerVal = $("#twoLower" + i).val();
                var twoOnLineVal = $("#twoOnLine" + i).val();
                var threeLowerVal = $("#threeLower" + i).val();
                var threeOnLineVal = $("#threeOnLine" + i).val();
                var fourLowerVal = $("#fourLower" + i).val();
                var fourOnLineVal = $("#fourOnLine" + i).val();
                var fiveLowerVal = $("#fiveLower" + i).val();
                var fiveOnLineVal = $("#fiveOnLine" + i).val();
                if (fiveOnLineVal == "以上") {
                    var fiveOnLine = 1;
                } else if (fiveOnLineVal == "以下") {
                    var fiveOnLine = 0;
                }
                if (sendAddIndividualStInfo != null && sendAddIndividualStInfo.trim() != '') {
                    sendAddIndividualStInfo += ';' + individualProjectTypeVal + "," + oneLowerVal + "," + oneOnLineVal + "," + twoLowerVal + "," + twoOnLineVal + "," + threeLowerVal + "," + threeOnLineVal + "," + fourLowerVal + "," + fourOnLineVal + "," + fiveLowerVal + "," + fiveOnLine;
                } else {
                    sendAddIndividualStInfo = individualProjectTypeVal + "," + oneLowerVal + "," + oneOnLineVal + "," + twoLowerVal + "," + twoOnLineVal + "," + threeLowerVal + "," + threeOnLineVal + "," + fourLowerVal + "," + fourOnLineVal + "," + fiveLowerVal + "," + fiveOnLine;
                }
            }
            alert(sendAddIndividualStInfo);
            url = "/api/PhysicalExamination/";
            showDiv();
            callApi(url, post, { DataType: 3, SendAddIndividualSt: sendAddIndividualStInfo, SelectAddAge: selectAddAgeVal, SendAddGender: sendAddGenderVal }).done(function (data) {
                if (data.Attach.Status == 0) {
                    alert("添加成功！");
                    redirect("IndividualStandard.html");
                } else {
                    alert("添加失败，请重新添加或者联系管理员！");
                }
                hideDiv();
            }).fail(function (data) {
                alert(data.message);
            });
        },

        GenertPhyMdicalInfo: function (type) {
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }
            var paraUserInfoKinIdVal = $.cookie("phyUserInfoKinId");
            var years = parseInt($("#yearVal").val());
            var paraUserInfoKinNameVal = $.cookie("phyUserInfoKinName");
            var permissionType = getPermissionType();
            var userId = $.cookie("UserId");
            url = "/api/GenerateKindergartenInfo/";
            callApi(url, post, { kindergartenId: paraUserInfoKinIdVal, permissionType: permissionType, userId: userId }).done(function (data) {
                var html = handlebarsHelp("#navKindergartens-template", data.Attach.Kindergartens);
                $("#navKindergartens").append(html);
                if (type == "one") {
                    var firstKinId = data.Attach.Kindergartens[0].KindergartenId;
                    var firstKinName = data.Attach.Kindergartens[0].KindergartenName;

                    if (paraUserInfoKinIdVal == null) {
                        $("#navKindergartens li:first").addClass("current");
                        $("#navKindergartens li:first").css({ "background": "#D9FFFF" });
                        $("#showKinNameInfo").text(firstKinName);
                        $.cookie("phyUserInfoKinId", firstKinId, { expires: 1, path: '/' });
                        $.cookie("phyUserInfoKinName", firstKinName, { expires: 1, path: '/' });
                        $("#inputPhyExKinderId").val(firstKinName);
                        phyExaminationPlanInfo(firstKinId, firstKinName, years);
                    } else {
                        $("#" + paraUserInfoKinIdVal).addClass("current");
                        $("#" + paraUserInfoKinIdVal).css({ "background": "#D9FFFF" });
                        $("#showKinNameInfo").text(paraUserInfoKinNameVal);
                        $("#inputPhyExKinderId").val(paraUserInfoKinNameVal);
                        phyExaminationPlanInfo(paraUserInfoKinIdVal, paraUserInfoKinNameVal, years);
                        //phyChildInfo(paraUserInfoKinIdVal, paraUserInfoKinNameVal);
                    }
                } else if (type == "two") {
                    $("#" + paraUserInfoKinIdVal).addClass("current");
                    $("#" + paraUserInfoKinIdVal).css({ "background": "#D9FFFF" });
                    $("#showKinNameInfo").text(paraUserInfoKinNameVal);
                    phyMedicalInfo();
                } else if (type == "three") {
                    $("#" + paraUserInfoKinIdVal).addClass("current");
                    $("#" + paraUserInfoKinIdVal).css({ "background": "#D9FFFF" });
                    $("#showKinNameInfo").text(paraUserInfoKinNameVal);
                    phyExaminationReport("one");
                }
            }).fail(function (data) {
                alert(data.message);
            });
        },
        GetClassAndChildInfo: function (kinId, kinName) {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            $("#selectTileId").empty();
            var allLi = $("#navKindergartens li");
            for (var liIndex = 0; liIndex < allLi.length; liIndex++) {
                $("#" + allLi[liIndex].id).removeClass("current");
                $("#" + allLi[liIndex].id).css({ "background": "" });
            }
            $("#" + kinId).addClass("current");
            $("#" + kinId).css({ "background": "#D9FFFF" });
            var paraUserInfoKinIdVal = $.cookie("phyUserInfoKinId");

            var years = parseInt($("#yearVal").val());
            var paraUserInfoKinNameVal = $.cookie("phyUserInfoKinName");
            phyExaminationPlanInfo(paraUserInfoKinIdVal, paraUserInfoKinNameVal, years);
            //phyChildInfo(kinId, kinName);
        },//userInfo.html
        GenerateOneDisCls: function (id) {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }
            var index = 10;
            url = "/api/PhysicalExamination/";
            callApi(url, post, { DataType: 7 }).done(function (data) {
                var indexVal = 1;
                for (var jos in data.Attach.AgeType) {
                    if (indexVal == id - index) {
                        $("#ageContent" + id).css({ display: "block" });
                        $("#types" + id).html("<a style='text-decoration: none;'>▲</a>");
                    } else {
                        $("#ageContent" + (index + indexVal)).css({ "display": "none" });
                        $("#types" + (index + indexVal)).text("▼");
                    }
                    indexVal++;
                }
            }).fail(function (data) {
                alert(data.message);
            });
        },

        GenertSendPhyMedicalInfo: function () {
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }
            //获取页面参数；
            var planIdVal = $.cookie("phyPlanId");
            var childBirthDayVal = $.cookie("phyBirthday");
            var childIdVal = $.cookie("phyChildId");
            var kinderIdVal = $.cookie("phyUserInfoKinId");
            var classIdVal = $.cookie("paraUserInfoClsId");
            var childHeightVal = $("#childHeightId").val();
            var childWeightVal = $("#childWeightId").val();
            var standingLongJumpVal = $("#standingLongJumpid").val();
            var jumpWithBothFeetVal = $("#jumpWithBothFeetid").val();
            var tennisThrowfarVal = $("#tennisThrowfarid").val();
            var walkTheBalanceBeamidVal = $("#walkTheBalanceBeamid").val();
            var childSeatBodyFlexionVal = $("#childSeatBodyFlexionid").val();
            var tenmShuttleRunVal = $("#tenmShuttleRunid").val();
            var userIdVal = $.cookie("UserId");
            var testTimeVal = $("#dateVal").val();
            var childGenderVal = $.cookie("phyGender");
            if (childBirthDayVal == null || childIdVal == null
                || kinderIdVal == null || classIdVal == null
                || childHeightVal == null || childHeightVal == ""
                || childWeightVal == null || childWeightVal == ""
                || standingLongJumpVal == null || standingLongJumpVal == ""
                || jumpWithBothFeetVal == null || jumpWithBothFeetVal == ""
                || tennisThrowfarVal == null || tennisThrowfarVal == ""
                || walkTheBalanceBeamidVal == null || walkTheBalanceBeamidVal == ""
                || childSeatBodyFlexionVal == null || childSeatBodyFlexionVal == ""
                || tenmShuttleRunVal == null || tenmShuttleRunVal == ""
                || testTimeVal == null || testTimeVal == "") {
                alert("请填写完整信息！");
                return;
            }
            if (userIdVal == null || userIdVal == "") {
                userIdVal = 0;
            }
            //计算小孩的年龄；
            var ageVal;

            //截取获取到测试的年/月/日
            var splitTestTimeVal = testTimeVal.split("-");
            if (splitTestTimeVal == null || splitTestTimeVal.length < 3) {
                alert("请填写完整信息！");
                return;
            }
            var testYear = splitTestTimeVal[0];
            var testMonth = splitTestTimeVal[1];

            var splitBithdayVal = childBirthDayVal.split("-");
            if (splitBithdayVal == null || splitBithdayVal.length < 3) {
                alert("宝贝信息不正确，请确认或联系管理员！");
                return;
            }

            var birthYeares = splitBithdayVal[0];
            var birthMonths = splitBithdayVal[1];
            var ageInfo = ((testYear - birthYeares) * 12 + (testMonth - birthMonths)) / 12.0;
            if (ageInfo < 3 || ageInfo >= 6.5) {
                alert("您的孩子不能进行体质检测！");
                return;
            } else {
                if (ageInfo >= 3 && ageInfo < 3.5) {
                    ageVal = 3;
                }
                else if (ageInfo >= 3.5 && ageInfo < 4) {
                    ageVal = 3.5;
                }
                else if (ageInfo >= 4 && ageInfo < 4.5) {
                    ageVal = 4;
                }
                else if (ageInfo >= 4.5 && ageInfo < 5) {
                    ageVal = 4.5;
                }
                else if (ageInfo >= 5 && ageInfo < 5.5) {
                    ageVal = 5;
                }
                else if (ageInfo >= 5.5 && ageInfo < 6) {
                    ageVal = 5.5;
                }
                else if (ageInfo >= 6 && ageInfo < 7) {
                    ageVal = 6;
                }
            }
            url = "/api/PhyExaminationChildInfo/";
            showDiv();
            callApi(url, post, { requestType: 1, ChildId: childIdVal, TestTime: testTimeVal, Height: childHeightVal, Age: ageVal, Gender: childGenderVal }).done(function (data) {
                var heightNormalVal;
                var standingLongJumNormalVal;
                var jumpWithBothFeetNormalVal;
                var tennisThrowfarNormalVal;
                var walkTheBalanceBeamidNormalVal;
                var childSeatBodyFlexionNormalVal;
                var tenmShuttleRunNormalVal;
                var weightScoreVal;
                var gradeVal;
                var weightDescVal;
                if (data.Attach.MedicalNormalInfo != null && data.Attach.MedicalNormalInfo != "") {
                    for (var jos in data.Attach.MedicalNormalInfo) {
                        var medicalNormal = data.Attach.MedicalNormalInfo[jos];
                        if (medicalNormal.PhysicalExaEIems == 0) {
                            heightNormalVal = medicalNormal;
                        } else if (medicalNormal.PhysicalExaEIems == 1) {
                            childSeatBodyFlexionNormalVal = medicalNormal;
                        } else if (medicalNormal.PhysicalExaEIems == 2) {
                            tenmShuttleRunNormalVal = medicalNormal;
                        } else if (medicalNormal.PhysicalExaEIems == 3) {
                            standingLongJumNormalVal = medicalNormal;
                        } else if (medicalNormal.PhysicalExaEIems == 4) {
                            tennisThrowfarNormalVal = medicalNormal;
                        } else if (medicalNormal.PhysicalExaEIems == 5) {
                            jumpWithBothFeetNormalVal = medicalNormal;
                        } else if (medicalNormal.PhysicalExaEIems == 6) {
                            walkTheBalanceBeamidNormalVal = medicalNormal;
                        }
                    }
                }
                if (data.Attach.WeightNormalInfo != null && data.Attach.WeightNormalInfo != "") {
                    for (var weighInf in data.Attach.WeightNormalInfo) {
                        var weightInfo = data.Attach.WeightNormalInfo[weighInf];
                        if (childWeightVal <= weightInfo.Thin1PintsOnLine) {
                            weightScoreVal = 1;
                            weightDescVal = "-1";
                        } else if (childWeightVal >= weightInfo.Fat1PointsLower) {
                            weightScoreVal = 1;
                            weightDescVal = "+1";
                        } else if (childWeightVal >= weightInfo.Fat3PointsLower && childWeightVal <= weightInfo.Fat3PointsOnLine) {
                            weightScoreVal = 3;
                            weightDescVal = "+3";
                        } else if (childWeightVal >= weightInfo.Thin3PointsOffLine && childWeightVal <= weightInfo.Thin3PointsOnLine) {
                            weightScoreVal = 3;
                            weightDescVal = "-3";
                        } else if (childWeightVal >= weightInfo.Normal5PointsOffLine && childWeightVal <= weightInfo.Normal5PointsOnLine) {
                            weightScoreVal = 5;
                            weightDescVal = "5";
                        } else {
                            weightScoreVal = 0;
                            weightDescVal = "0";
                        }
                    }
                } else {
                    weightScoreVal = 0;
                    weightDescVal = "0";
                }
                var heightscoreVal = singleMedicalFraction(childHeightVal, heightNormalVal);
                var childSeatBodyFlexionScoreVal = singleMedicalFraction(childSeatBodyFlexionVal, childSeatBodyFlexionNormalVal);
                var tenmShuttleRunScoreVal = singleMedicalFraction(tenmShuttleRunVal, tenmShuttleRunNormalVal);
                var standingLongJumScoreVal = singleMedicalFraction(standingLongJumpVal, standingLongJumNormalVal);
                var tennisThrowScoreVal = singleMedicalFraction(tennisThrowfarVal, tennisThrowfarNormalVal);
                var jumpWithBothScoreVal = singleMedicalFraction(jumpWithBothFeetVal, jumpWithBothFeetNormalVal);
                var walkTheBalanceScoreVal = singleMedicalFraction(walkTheBalanceBeamidVal, walkTheBalanceBeamidNormalVal);
                var fractionTallVal = heightscoreVal + childSeatBodyFlexionScoreVal + tenmShuttleRunScoreVal + standingLongJumScoreVal + tennisThrowScoreVal + jumpWithBothScoreVal + walkTheBalanceScoreVal + weightScoreVal;
                if (fractionTallVal > 31) {
                    gradeVal = 1;
                } else if (fractionTallVal >= 28 && fractionTallVal <= 31) {
                    gradeVal = 2;
                } else if (fractionTallVal >= 20 && fractionTallVal <= 27) {
                    gradeVal = 3;
                } else if (fractionTallVal < 20) {
                    gradeVal = 4;
                }
                url = "/api/ChildPhyMedicalResult/";
                showDiv();
                callApi(url, post, {
                    UserId: userIdVal,
                    ChildId: childIdVal,
                    KindergartenId: kinderIdVal,
                    ClassId: classIdVal,
                    Height: childHeightVal,
                    Weights: childWeightVal,
                    StandingLongJump: standingLongJumpVal,
                    JumpWithBothFeet: jumpWithBothFeetVal,
                    TennisThrowFar: tennisThrowfarVal,
                    WalkTheBalanceBeam: walkTheBalanceBeamidVal,
                    SeatBodyFlexion: childSeatBodyFlexionVal,
                    TenShuttleRun: tenmShuttleRunVal,
                    HeightScore: heightscoreVal,
                    StandingLongJumpScore: standingLongJumScoreVal,
                    JumpWithBothFeetScore: jumpWithBothScoreVal,
                    TennisThrowFarScore: tennisThrowScoreVal,
                    WalkTheBalanceBeamScore: walkTheBalanceScoreVal,
                    SeatBodyFlexionScore: childSeatBodyFlexionScoreVal,
                    TenShuttleRunScore: tenmShuttleRunScoreVal,
                    WeightsScore: weightScoreVal,
                    Fraction: fractionTallVal,
                    AgeGroup: ageVal,
                    Grade: gradeVal,
                    TestTime: testTimeVal,
                    Gender: childGenderVal,
                    PhysicalExaminationPlaneId: planIdVal,
                    WeightDesc: weightDescVal
                }).done(function (data) {
                    if (data.Attach.Status == 0) {
                        alert("体测结果提交成功！");
                        redirect("phyExaminationReport.html");
                    } else {
                        alert("体测结果提交失败！");
                    }
                    hideDiv();
                }).fail(function (data) {
                    alert(data.message);
                    hideDiv();
                });
                hideDiv();
            }).fail(function (data) {
                alert(data.message);
                hideDiv();
            });
        },

        GenertSubmintPhyPlan: function () {
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }
            var kinderIdVal = $.cookie("phyUserInfoKinId");
            var planeTimeVal = $("#dateVal").val();
            var descriptionVal = $("#inputPhyContentId").val();
            if (descriptionVal == null || descriptionVal == "") {
                descriptionVal = "";
            }
            var planTitleVal = $("#inputTileId").val();
            var userIdVal = $.cookie("UserId");
            var now = new Date();
            var planTime = planeTimeVal.replace("-", "/");
            var testTime = new Date(Date.parse(planTime));
            if (kinderIdVal == null || kinderIdVal == ""
                || planeTimeVal == null || planeTimeVal == ""
                || planTitleVal == null || planTitleVal == "") {
                alert("请填写完整信息！");
                return;
            }
            if (testTime.getFullYear() < now.getFullYear()) {
                alert("计划时间不能小于创建时间，请重新选择计划时间！");
                return;
            } else if (testTime.getMonth() < now.getMonth()) {
                alert("计划时间不能小于创建时间，请重新选择计划时间！");
                return;
            } else if (testTime.getDate() < now.getDate()) {
                alert("计划时间不能小于创建时间，请重新选择计划时间！");
                return;
            }
            if (userIdVal == null || userIdVal == "") {
                userIdVal = 0;
            }
            url = "/api/PhysicalExamination/";
            callApi(url, post, { DataType: 4, KindergardenId: kinderIdVal, PlanTitle: planTitleVal, PlanTime: planeTimeVal, Description: descriptionVal, UserId: userIdVal }).done(function (data) {
                if (data.Attach.Status == 0) {
                    alert("计划创建成功！");
                    $.cookie("phyPlanId", data.Attach.PhyPlanExaminationId, { expires: 1, path: '/' });
                    redirect("phyExaminationPlan.html");
                }
            }).fail(function (data) {
                alert(data.message);
            });
        },

        GenertChildYearphyExInfo: function () {
            var paraUserInfoKinIdVal = $.cookie("phyUserInfoKinId");
            var years = parseInt($("#yearVal").val());
            var paraUserInfoKinNameVal = $.cookie("phyUserInfoKinName");
            phyExaminationPlanInfo(paraUserInfoKinIdVal, paraUserInfoKinNameVal, years);
        },

        GenertOnchangeTitleInfo: function () {
            var paraUserInfoKinIdVal = $.cookie("phyUserInfoKinId");
            var phyPlanId = $("#selectTileId").val();
            phyChildType(paraUserInfoKinIdVal, phyPlanId);
        },

        GeneratePhyPlanDisCls: function (id) {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }
            var kindIdVal = $.cookie("phyUserInfoKinId");
            url = "/api/User/";
            callApi(url, post, { Type: 4, KindergartenId: kindIdVal }).done(function (data) {
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
            }).fail(function (data) {
                alert(data.message);
            });
        },

        GenertPrintRecordeInf: function () {
            phyExaminationReport("print");
        }

    };
}();
