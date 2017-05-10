/*
 * Core script to handle all login specific things
 */

var TestQuestion = function () {

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

    var parentsQuestionInfo = function () {

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

    var getImageUrls = function(type){
        var imageName = "";
        if(type ==1){
            imageName = "011.png";
        }else if(type == 2){
            imageName = "022.png";
        }else if(type == 3){
            imageName = "033.png";
        }else if(type == 4){
            imageName = "044.png";
        }else if(type == 5){
            imageName = "055.png";
        }else if(type == 6){
            imageName = "066.png";
        }else if(type == 7){
            imageName = "077.png";
        }else if(type == 8){
            imageName = "088.png";
        }
        return imageName;
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

    var testQuestType = function () {
        var questionTypeJson = {
            "1": "语言言语智力",
            "2": "数理逻辑智力",
            "3": "视觉空间智力",
            "4": "身体动觉智力",
            "5": "音乐节奏智力",
            "6": "人际交往智力",
            "7": "自然观察智力",
            "8":"自知自省智力"
        };
        return questionTypeJson;
    };

    var url = "/api/User/";
    var get = "GET";
    var post = "POST";

    return {
        //user.html     

        GenentEvaluationNormalInfo: function () {
            var genderNormals = '';
            var indexNormal = 0;
            url = "/api/ParameterConfigration/";
            showDiv();
            callApi(url, post,{TestType:1}).done(function (data) {
                for (var jos in data.Attach.ParameterInfoDetail) {
                    var parameterInfo = data.Attach.ParameterInfoDetail[jos];
                    var age = {
                        AgeType: parameterInfo.ageType
                    };
                    var htmlAge = handlebarsHelp("#ageNormalInfoId", age);
                    $("#allClassContent").append(htmlAge);
                    if (indexNormal == 0) {
                        $("#ageContent" + parameterInfo.ageType).css({ "display": "block" });
                    } else {
                        $("#ageContent" + parameterInfo.ageType).css({ "display": "none" });
                    }
                    var index = 0
                    var panelInfo = parameterInfo.allGenderNorms;
                    for (var panels in panelInfo) {
                        var genderNormalInfo = panelInfo[panels];
                        index++;
                        var paneels = {
                            Panel: index,
                            GenderNormGroupAgeTypeId: genderNormalInfo.genderNormATId,
                            ageType: parameterInfo.ageType,
                        }
                        var htmlPanel = handlebarsHelp("#panelNormalInfoId", paneels);
                        $("#ageContent" + parameterInfo.ageType).append(htmlPanel);
                        var genderInfo = genderNormalInfo.genderNorms;
                        for (var gend in genderInfo) {
                            var genders = genderInfo[gend];
                            if (genders.gender == 0) {
                                genderNormals = "女";
                            }
                            if (genders.gender == 1) {
                                genderNormals = "男";
                            }
                            var indexDetailInfo = 0;
                            var detailInfo = genders.details;
                            for (var de in detailInfo) {
                                var deInfo = detailInfo[de];
                                var detailItem = {
                                    Gender: genderNormals,
                                    EvaluationType: deInfo.EvaluationType,
                                    SpeechLanguage: deInfo.SpeechLanguage,
                                    MathematicalLogic: deInfo.MathematicalLogic,
                                    VisualSpace: deInfo.VisualSpace,
                                    Kinesthetic: deInfo.Kinesthetic,
                                    RhythmMusic: deInfo.RhythmMusic,
                                    Communication: deInfo.Communication,
                                    Intrapersonal: deInfo.Intrapersonal,
                                    NaturalObservation: deInfo.NaturalObservation,
                                    TotalScore: deInfo.TotalScore,
                                };
                                if (indexDetailInfo == 0) {
                                    var htmlItem = handlebarsHelp("#detailNormalOnceInfoId", detailItem);
                                    $("#NormalInfoTableId" + genderNormalInfo.genderNormATId).append(htmlItem);
                                } else {
                                    var htmlItem = handlebarsHelp("#detailNormalSecondInfoId", detailItem);
                                    $("#NormalInfoTableId" + genderNormalInfo.genderNormATId).append(htmlItem);
                                }
                                indexDetailInfo++;
                            }
                           
                        }
                        indexNormal++;
                    }
                }
                hideDiv();
            }).fail(function (data) {
                alert(data.message);
            })
        },

        GenentInputQuestionInfo: function () {
            $("#edit_user_info").attr("disabled", true);
            var selectTextVal = $("#selectTestQuestion option:selected").val();
            var checkVal = $($("input[name = 'OpenQuestionId']:checked")).is("input:checked");
            var inputWeighVal = $("#inputWeightId").val();
            var indexVal = $("#inputIndexId").val();
            var inputNormaVal = $("#inputTextNormalId").val();
            var inputContentVal = $("#inputContentId").val();
            var userVal = $.cookie("UserId");
            if (userVal == null || userVal == "") {
                var userVal = 0;
            }
            if (inputWeighVal == null || inputWeighVal == "" || indexVal == null || indexVal == "" || inputNormaVal == null || inputNormaVal == "") {
                alert("请填写完整信息！");
                $("#edit_user_info").removeAttr("disabled");
                return;
            }
            url = "/api/IntelligenceQuestion/";
            showDiv();
            callApi(url, post, {UserId:userVal, QuestionType: selectTextVal, IsActive: checkVal, QWeight: inputWeighVal, Number: indexVal, EvaluationIndicators: inputNormaVal, QuestionContent: inputContentVal }).done(function (data) {
                if (data.Attach.Status == "success") {
                    alert("录入题目成功！");
                } else {
                    alert("录入题目失败！");
                    $("#edit_user_info").removeAttr("disabled");
                }
                hideDiv();
            }).fail(function (data) {
                alet(data.message);
            })
        },

        GenetParentQuestionType: function () {
            var testQuestion = testQuestType();
            var type = [];
            for (var jos in testQuestion) {
                var imageUrl = "images/" + getImageUrls(jos);
                var item = {
                    QuestionType: jos,
                    QuestionTypeName: testQuestion[jos],
                    ImageUrl: imageUrl,
                };
                type.push(item);
            }
            var html = handlebarsHelp("#parentQuestionTile", type);
            $("#questionType-id").append(html);
        },

        GenertParentIntelligenceQuestionDetail: function () {
            var testQuestionVal = $.cookie("testQuestiongsType");
            var userIdVal = 11;
            url = "/api/ParameterConfigration/";
            showDiv();
            callApi(url, post, { TestType: 2, QuestionType: testQuestionVal,UserId:userIdVal}).done(function (data) {
                var html = handlebarsHelp("#parentTestQuestionHtml", data.Attach.TestQuestion);
                $("#parentIntelligenceQuestionTableId").append(html);
                $("#sorceTitle").text(data.Attach.ChidInfo[0].ChildName + "多元智力测评");
            hideDiv();
            }).fail(function (data) {
                alert(data.message);
            })
        },

        GenertTeacherScoreIntelligenceQuestion: function () {
            var testQuestionVal = $.cookie("testIntelligenceId");
            var userIdVal = 162;
            var child = [];
            url = "/api/ParameterConfigration/";
            showDiv();
            callApi(url, post, { TestType: 3, IntelligenceQuestionId: testQuestionVal, UserId: userIdVal }).done(function (data) {
                var htmlContent = handlebarsHelp("#childIntelligenceTitlHtmlId", data.Attach.QuestionContent);
                $("#intelligenceQuestionTitleId").append(htmlContent);
                for (var jos in data.Attach.TeacherSorceChilInfo) {
                    if (data.Attach.TeacherSorceChilInfo[jos].ImageName == null || data.Attach.TeacherSorceChilInfo[jos].ImageName == "") {
                        var imageUrl = "images/defaultChildImage.png";
                    }
                    else {
                        var imageUrl = "http:\/\/babybus.emolbase.com\/" + data.Attach.TeacherSorceChilInfo[jos].ImageName;
                    }
                    var item = {
                        ChildName: data.Attach.TeacherSorceChilInfo[jos].ChildName,
                        ChildId: data.Attach.TeacherSorceChilInfo[jos].ChildId,
                        ImageUrl:imageUrl,
                    };
                    child.push(item);
                }
                var html = handlebarsHelp("#childIntelligenceHtmlId", child);
                $("#childTestQuestionId").append(html);
                hideDiv();
            }).fail(function (data) {
                alert(data.message);
            })
        },

        GenertTeacherIntelligenceSorce: function () {
            var sorceInfo = "";
            var intelligenceQuestionIdVal = $.cookie("testIntelligenceId");
            var userVal = 162;
            var inAssType = 0;
           /* if (getPermissionType() == 1) {
                inAssType = 0;
            }
            if (getPermissionType() == 2) {
                inAssType = 1;
            }*/
            $("dl dd,dl dt dd").each(function () {
                var tr = $(this).attr("id");
                if (tr == null || tr == undefined) {
                    return;
                }
                var value = parseInt(tr.replace(/[^0-9]/ig, ""));
                var sorce = $("#score" + value).val();
                sorceInfo += value + "," + sorce + ";";
            })
            url = "/api/ParameterConfigration/";
            showDiv();
            callApi(url, post, { TestType: 4, InAssType: 1, Sorce: sorceInfo, IntelligenceQuestionId: intelligenceQuestionIdVal, UserId: userVal }).done(function (data) {
                if (data.Attach.Status == 0) {
                    alert("提交成功！");
                    redirect("TeachTestQuestionDetail.html");
                } else {
                    alert("提交失败，请重新提交或联系管理员！");
                    return;
                }
                hideDiv();
            }).fail(function (data) {
                alert(data.message);
            })
        },
        GenertParesentIntelligenceSorce: function () {
            var userIdVal = 11;
            var sorceInfo = "";
            $('dl dd, dl dt dd').each(function () {
                var trId = $(this).attr("id");
                if (trId == null || trId == undefined) {
                    return;
                }
                var value = parseInt(trId.replace(/[^0-9]/ig, ""));
                var sorce = $("#score" + value).val();
                sorceInfo += value + "," + sorce + ";";
            })
            url = "/api/ParameterConfigration/";
            showDiv();
            callApi(url, post, {TestType:5,UserId:userIdVal,Sorce:sorceInfo,InAssType:getPermissionType}).done(function (data) {
                if (data.Attach.Status == 0) {
                    alert("提交成功！");
                } else {
                    alert("提交失败，请重新提交或联系管理员！");
                    return;
                }
                hideDiv();
            }).fail(function (data) {
                alert(data.message);
            })
        }
    };
}();