/*
 * Core script to handle all login specific things
 */

var Message = function () {

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


    var selectTypeNo = function (typeNo) {
        var type;
        if (typeNo == 1) {
            type = "家庭作业";
            return type;
        }
        if (typeNo == 2) {
            type = "班级通知";
            return type;
        }
        if (typeNo == 4) {
            type = "园区通知";
            return type;
        }
        if (typeNo == 5) {
            type = "园务通知";
            return type;
        }
        if (typeNo == 6) {
            type = "成长记忆";
            return type;
        }
        if (typeNo == 7) {
            type = "食谱";
            return type;
        }
        if (typeNo == -3) {
            type = "请假";
            return type;
        }
        if (typeNo == -2) {
            type = "家园共育";
            return type;
        }
        if (typeNo == 0) {
            type = "园长提问";
            return type;
        }
        return type;
    }

    var getTypeByTypeNo = function (typeNo) {
        var type;
        if (typeNo == 1) {
            type = "家庭作业";
            return type;
        }
        if (typeNo == 2) {
            type = "班级通知";
            return type;
        }
        /*if (typeNo == 3) {
            type = "ClassEmergency";
        }*/
        if (typeNo == 4) {
            type = "园区通知";
            return type;
        }
        if (typeNo == 5) {
            type = "园务通知";
            return type;
        }
        if (typeNo == 6) {
            type = "成长记忆";
            return type;
        }
        if (typeNo == 7) {
            type = "食谱";
            return type;
        }
        return type;
    };
    var getTypeByQuestionTypeNo = function (typeNo) {
        var type;
        if (typeNo == 0) {
            type = "请假";
            return type;
        }
        if (typeNo == 1) {
            type = "家园共育";
            return type;
        }
        if (typeNo == 2) {
            type = "园长提问";
            return type;
        }
        return type;
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
    var getImageNameByTypeNo = function (typeNo) {
        var imageName;
        if (typeNo == 1) {
            imageName = "images/homework.png";
        }
        if (typeNo == 2) {
            imageName = "images/classCommon.png";
        }
        if (typeNo == 4) {
            imageName = "images/yqtz.png";
        }
        if (typeNo == 5) {
            imageName = "images/ywtz.png";
        }
        if (typeNo == 6) {
            imageName = "images/growMemory.png";
        }
        if (typeNo == 7) {
            imageName = "images/food2.png";
        }
        return imageName;
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
            return '园区';
        }
        return '-';
    };
    var getImgByTypeNoForQuestion = function (typeNo) {
        var imageName;
        if (typeNo == 0) {//请假
            imageName = "images/leave2.png";
        }
        if (typeNo == 1) {//家园共育
            imageName = "images/home2.png";
        }
        if (typeNo == 2) {//园长提问
            imageName = "images/question.png";
        }
        return imageName;
    };

    var GenerateSendMessageRecordsInfo = function (kindergartenIdVal, permissionType) {

        var year = parseInt($("#yearVal").val());
        var date = new Date();
        var CurrentYear = parseInt(date.getFullYear());
        var CurrentMonth = parseInt(date.getMonth()) + 1;

        var type;
        var classIdVal = $.cookie("paraClassId");
        if (permissionType == 2) {
            type = 1;
        } else if (permissionType == 0 || permissionType == 3 || permissionType == 4) {
            type = 0;
        } else {
            alert("权限不足!");
            return;
        }

        url = "/api/SendMessageRecords/";
        showDiv();
        callApi(url, post, {
            KindergartenId: kindergartenIdVal,
            Year: year,
            ClassId: classIdVal,
            Type: type
        }).done(function (data) {

            var monthMessageInfo = [];
            for (var index = 1; index <= 12; index++) {

                var classHomeworkCount = "-";
                var classCommonCount = "-";
                var classEmergencyCount = "-";
                var kindergartenAllCount = "-";
                var kindergartenStaffCount = "-";
                var growMemoryCount = "-";
                var sumCount = "-";
                var questionLeaveCount = "-";
                var questionTeacherCount = "-";
                var questionKindergartenCount = "-";
                var recipeCount = "-";

                for (var messageInfoIndex in data.Attach.messageInfo) {
                    if (data.Attach.messageInfo[messageInfoIndex].month == index) {
                        if (sumCount == "-") {
                            sumCount = 0;
                        }
                        var typeInfo = data.Attach.messageInfo[messageInfoIndex].typeinfo;
                        for (var typeInfoIndex in typeInfo) {
                            var typeNo = typeInfo[typeInfoIndex].type;
                            var typeCount = typeInfo[typeInfoIndex].typeCount;
                            //alert("year" + year + "month" + index + "typeCount" + typeCount + "typeNo" + typeNo);
                            if (typeNo == 1) {
                                classHomeworkCount = typeCount;
                                sumCount += typeCount;
                            }
                            if (typeNo == 2) {
                                classCommonCount = typeCount;
                                sumCount += typeCount;
                            }
                            if (typeNo == 3) {
                                classEmergencyCount = typeCount;
                            }
                            if (typeNo == 4) {
                                kindergartenAllCount = typeCount;
                                sumCount += typeCount;
                            }
                            if (typeNo == 5) {
                                kindergartenStaffCount = typeCount;
                                sumCount += typeCount;
                            }
                            if (typeNo == 6) {
                                growMemoryCount = typeCount;
                                sumCount += typeCount;
                            }
                            if (typeNo == 7) {
                                recipeCount = typeCount;
                                sumCount += typeCount;
                            }

                        }//loop typeInfo end
                    }
                }//loop data.Attach.attendencePlanInfo end

                for (var questionIndex in data.Attach.questionInfo) {
                    var question = data.Attach.questionInfo[questionIndex];
                    if (question.month == index) {
                        if (sumCount == "-") {
                            sumCount = 0;
                        }
                        var typeInfoQuestion = question.typeInfo;
                        for (var typeInfoQuestionIndex in typeInfoQuestion) {
                            var questionTypeNo = typeInfoQuestion[typeInfoQuestionIndex].type;
                            var questionTypeCount = typeInfoQuestion[typeInfoQuestionIndex].typeCount;
                            if (questionTypeNo == 0) {
                                questionLeaveCount = questionTypeCount;
                                sumCount += questionTypeCount;
                            } else if (questionTypeNo == 1) {
                                questionTeacherCount = questionTypeCount;
                                sumCount += questionTypeCount;
                            } else if (questionTypeNo == 2) {
                                questionKindergartenCount = questionTypeCount;
                                sumCount += questionTypeCount;
                            }
                        }//loop typeInfoQuestion end
                    }
                }//loop data.Attach.questionInfo end

                var item = {
                    month: index,
                    ClassHomework: classHomeworkCount,
                    ClassCommon: classCommonCount,
                    //ClassEmergency: classEmergencyCount,
                    KindergartenAll: kindergartenAllCount,
                    KindergartenStaff: kindergartenStaffCount,
                    GrowMemory: growMemoryCount,
                    sumCount: sumCount,
                    image: getImageByMonth(index),
                    questionLeave: questionLeaveCount,
                    questionTeacher: questionTeacherCount,
                    questionKindergarten: questionKindergartenCount,
                    recipe: recipeCount
                };
                monthMessageInfo.push(item);
            }

            var html = handlebarsHelp("#SendMessageRecordsHtml", monthMessageInfo);
            $("#monthMessageContent").empty();
            $("#monthMessageContent").append(html);

            if (year == CurrentYear) {
                for (var i = (CurrentMonth + 1) ; i < 13; i++) {
                    var m = i + '';
                    $("#a" + m).removeAttr("onclick");

                    $("#SumCount" + m).text('');
                    $("#msgInfoTable" + m).css("display","none");
                }
            }
            hideDiv();
        }).fail(function (data) {
            hideDiv();
            alert(data.message);
        });
    };
    var GenerateClassNoticeInfo = function (classInfo, msgInfo, questionInfo, permissionType) {
        var classMsgInfo = [];
        for (var classInfoIndex in classInfo) {
            var clsName = classInfo[classInfoIndex].className;
            var clsId = classInfo[classInfoIndex].classId;
            var classHomeworkCount = "-";
            var classCommonCount = "-";
            var growMemoryCount = "-";
            var sumCount = "-";
            var questionLeaveCount = "-";
            var questionTeacherCount = "-";

            for (var msgInfoIndex in msgInfo) {
                if (msgInfo[msgInfoIndex].classId == clsId) {
                    var typeInfo = msgInfo[msgInfoIndex].typeInfo;
                    if (sumCount == "-") {
                        sumCount = 0;
                    }
                    for (var typeInfoIndex in typeInfo) {
                        var typeNo = typeInfo[typeInfoIndex].typeNo;
                        var typeCount = typeInfo[typeInfoIndex].typeCount;
                        if (typeNo == 1) {
                            classHomeworkCount = typeCount;
                            sumCount += typeCount;
                        }
                        if (typeNo == 2) {
                            classCommonCount = typeCount;
                            sumCount += typeCount;
                        }
                        if (typeNo == 6) {
                            growMemoryCount = typeCount;
                            sumCount += typeCount;
                        }
                    }//loop typeInfo end
                }
            }//loop msgInfo end


            for (var questionIndex in questionInfo) {
                if (questionInfo[questionIndex].classId == clsId) {
                    if (sumCount == "-") {
                        sumCount = 0;
                    }
                    var questionTypeInfo = questionInfo[questionIndex].typeInfo;
                    for (var questionTypeInfoIndex in questionTypeInfo) {
                        var questionTypeNo = questionTypeInfo[questionTypeInfoIndex].type;
                        var questionTypeCount = questionTypeInfo[questionTypeInfoIndex].typeCount;
                        if (questionTypeNo == 0) {
                            questionLeaveCount = questionTypeCount;
                            sumCount += questionTypeCount;
                        }
                        if (questionTypeNo == 1) {
                            questionTeacherCount = questionTypeCount;
                            sumCount += questionTypeCount;
                        }
                    }//loop typeInfo end
                }
            }//loop questionInfo end

            var item = {
                className: clsName,
                classId: clsId,
                ClassHomework: classHomeworkCount,
                ClassCommon: classCommonCount,
                GrowMemory: growMemoryCount,
                sumCount: sumCount,
                questionLeave: questionLeaveCount,
                questionTeacher: questionTeacherCount,
                
            };
            classMsgInfo.push(item);
        }//loop classInfo end
        //-------------------------------------------
        var html = handlebarsHelp("#SendMsgClsesHtml", classMsgInfo);
        $("#SendMsgClsesContent").empty();
        $("#SendMsgClsesContent").append(html);
    }
    var GenerateKindergartenNoticeInfo = function (msgInfo, questionInfo, permissionType) {
        var classMsgInfo = [];

        var clsNameYx = '园区';
        var clsIdYx = 0;
        var kindergartenAllCountYx = "-";
        var kindergartenStaffCountYx = "-";
        var sumCountYx = questionInfo;
        var questionKindergartenCountYx = questionInfo;
        var recipeCountYx = "-";

        for (var yqyMsgInfoIndex in msgInfo) {
            if (msgInfo[yqyMsgInfoIndex].classId == 0) {
                var typeInfo = msgInfo[yqyMsgInfoIndex].typeInfo;
                for (var typeInfoIndex in typeInfo) {
                    var typeNo = typeInfo[typeInfoIndex].typeNo;
                    var typeCount = typeInfo[typeInfoIndex].typeCount;
                    if (typeNo == 4) {
                        kindergartenAllCountYx = typeCount;
                        sumCountYx += typeCount;
                    }
                    if (typeNo == 5) {
                        kindergartenStaffCountYx = typeCount;
                        sumCountYx += typeCount;
                    }
                    if (typeNo == 7) {
                        recipeCountYx = typeCount;
                        sumCountYx += typeCount;
                    }
                }//loop typeInfo end
            }
        }//loop msgInfo end

        var item = {
            className: clsNameYx,
            classId: clsIdYx,
            KindergartenAll: kindergartenAllCountYx,
            KindergartenStaff: kindergartenStaffCountYx,
            sumCount: sumCountYx,
            questionKindergarten: questionKindergartenCountYx,
            recipe: recipeCountYx
        };
        classMsgInfo.push(item);
        //-------------------------------------------
        var html = handlebarsHelp("#SendMsgKindergartenHtml", classMsgInfo);
        $("#SendMsgClsesContent").empty();
        $("#SendMsgClsesContent").append(html);
    }
    var GenerateSendMsgClsesInfo = function (kingdergartenId, permissionType) {
       
        if (kingdergartenId == null || permissionType == null) {
            alert("信息缺失");
            return;
        }
        var type = 0; //0代表幼儿园
        var classIdVal = $.cookie("ClassId");
        
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
        var year = $.cookie("paraMsgYear");
        var month = $.cookie("paraMsgMonth");

        $("#time").text(year + "年" + month + "月");
       
        $("#monthImg").attr("src", getImageByMonth(month));

        url = "/api/SendMsgRecdsClses/";
        var classTypeVal = $("#classTypeVal").val();
        var classTypeDes = getClassTypeDes(classTypeVal);

        showDiv();
        callApi(url, post, {
            KingdergartendId: kingdergartenId,
            Year: year,
            Month: month,
            ClassType: classTypeVal,
            Type: type,
            ClassId: classIdVal
        }).done(function (data) {
            if (classTypeVal != -1) {
                var classInfo = data.Attach.classInfo;
                if (classInfo.length <= 0) {
                    alert("暂时没有 【" + classTypeDes + "】 班级信息!");
                    $("#SendMsgClsesContent").empty();
                    hideDiv();
                    return;
                }
                var msgInfo = data.Attach.messageInfo;
                var questionInfo = data.Attach.questionMonthInfo;
                //准备班级信息
                GenerateClassNoticeInfo(classInfo, msgInfo, questionInfo, permissionType);
            } else {
                var msgInfo = data.Attach.messageInfo;
                var questionInfo = data.Attach.questionMonthInfoCount;
                GenerateKindergartenNoticeInfo(msgInfo, questionInfo, permissionType);
            }
            hideDiv();

        }).fail(function (data) {
            hideDiv();
            console.log(data.message);
        });
    };

    var drawMessageInfos = function (messageInfo, calssId) {
        for (var msgInfoIndex in messageInfo) {
            if (messageInfo[msgInfoIndex].typeNo == 8) {
                continue;
            }
            if (calssId == 0) {
                if (messageInfo[msgInfoIndex].typeNo == 1 || messageInfo[msgInfoIndex].typeNo == 2 || messageInfo[msgInfoIndex].typeNo == 3 || messageInfo[msgInfoIndex].typeNo == 6) {
                    continue;
                }
            } else {
                if (messageInfo[msgInfoIndex].typeNo == 3 || messageInfo[msgInfoIndex].typeNo == 4 || messageInfo[msgInfoIndex].typeNo == 5 || messageInfo[msgInfoIndex].typeNo == 7) {
                    continue;
                }
            }

            var typeNoVal = messageInfo[msgInfoIndex].typeNo;
            var typeNameVal = getTypeByTypeNo(typeNoVal);
            var item = {
                typeNo: typeNoVal,
                typeName: typeNameVal
            };
            var html = handlebarsHelp("#SendMsgClsDetailHeaderHtml", item);
            $("#sendMsgClsDetailHeader").append(html);
            var typeInfos = [];
            var typeInfo2 = messageInfo[msgInfoIndex].typeInfo;
            var index = 1;
            var image = getImageNameByTypeNo(typeNoVal);
            for (var typeInfoIndex in typeInfo2) {
                var msg = typeInfo2[typeInfoIndex].msg;
                var titileVal = msg.Title;
                if (msg.Title.length > 20) {
                    titileVal = msg.Title.substring(0, 20) + "...";
                }

                var typeInfosItem = {
                    title: titileVal,
                    content: msg.Content,
                    userName: typeInfo2[typeInfoIndex].userName,
                    year: typeInfo2[typeInfoIndex].year,
                    month: typeInfo2[typeInfoIndex].month,
                    day: typeInfo2[typeInfoIndex].day,
                    noticeId: msg.NoticeId,
                    infoNo: index,
                    imageName: image,
                    itmTypeNo: typeNoVal
                };
                typeInfos.push(typeInfosItem);
                index++;
            }//loop messageInfo[msgInfoIndex].typeInfo end
            var html = handlebarsHelp("#SendMsgClsDetailDetailHtml", typeInfos);
            $("#" + typeNoVal).append(html);
        }// loop messageInfo end
    }//drawMessageInfos end

    var drawQuestionInfos = function (questionInfo, classId) {
        for (var questionInfoIndex in questionInfo) {

            if (classId == 0) {
                if (questionInfo[questionInfoIndex].typeNo != 2) {
                    continue;
                }
            }
            else {
                if (questionInfo[questionInfoIndex].typeNo != 0
                    && questionInfo[questionInfoIndex].typeNo != 1) {
                    continue;
                }
            }

            var typeNoVal = questionInfo[questionInfoIndex].typeNo;
            var typeNameVal = getTypeByQuestionTypeNo(typeNoVal);
            var ysTypeNo = questionInfo[questionInfoIndex].typeNo;
            typeNoVal += "question";
            var typeQusType = questionInfo[questionInfoIndex].typeNo;
            var item = {
                typeNo: typeNoVal,
                typeName: typeNameVal
            };
            var html = handlebarsHelp("#SendMsgClsDetailHeaderHtml", item);
            $("#sendMsgClsDetailHeader").append(html);

            var typeInfos = [];
            var typeInfo2 = questionInfo[questionInfoIndex].typeInfo;
            var index = 1;
            var image = getImgByTypeNoForQuestion(ysTypeNo);
            for (var typeInfoIndex in typeInfo2) {
                var qs = typeInfo2[typeInfoIndex].question;

                var titleVal;
                var content = qs.Content;
                if (content.length > 20) {
                    titleVal = content.substring(0, 20) + "...";
                } else {
                    titleVal = content;
                }
                var typeInfosItem = {
                    title: titleVal,
                    userName: "作者",
                    year: typeInfo2[typeInfoIndex].year,
                    month: typeInfo2[typeInfoIndex].month,
                    day: typeInfo2[typeInfoIndex].day,
                    questionId: qs.QuestionId,
                    infoNo: index,
                    imageName: image,
                    itmTypeNo: typeQusType
                };
                typeInfos.push(typeInfosItem);
                index++;

            }//loop typeInfo2 end
            var html = handlebarsHelp("#questionHtml", typeInfos);
            $("#" + typeNoVal).append(html);

        }// loop messageInfo end
    }//drawMessageInfos end

    var GenerateSendMsgClsDetailInfo = function (kingdergartenId, permissionType) {

        var year = $.cookie("paraMsgYear");
        var month = $.cookie("paraMsgMonth");
        var calssId = $.cookie("paraMsgClassId");
        var className = $.cookie("paraMsgClassName");
        var noticeType = $.cookie("paraMsgNoticeType");

        $("#monthImg").attr("src", getImageByMonth(month));
        $("#time").text(year + "年" + month + "月      " + className);
        
        if (kingdergartenId == null) {
            alert("信息缺失");
            return;
        }

        var typeName = "all";
        var qusetionType;
        if (noticeType == -1) {
            typeName = "all";
        } else if (noticeType == 0) {
            typeName = "question";
            qusetionType = 2;
        } else if (noticeType == -2) {
            typeName = "question";
            qusetionType = 1;
        } else if (noticeType == -3) {
            typeName = "question";
            qusetionType = 0;
        } else {
            typeName = "message";
        }

        url = "/api/SendMsgClsDetail/";
        showDiv();
        callApi(url, post, {
            KingdergartendId: kingdergartenId,
            Year: year,
            Month: month,
            ClassId: calssId,
            NoticeType: noticeType,
            QusetionType: qusetionType,
            typeVal: typeName
        }).done(function (data) {
            var msgHeader = [];
            for (var typeIndex = -3; typeIndex <= 7 ; typeIndex++) {
                if (typeIndex == -1) {
                    continue;
                }
                if (calssId == 0) {
                    //-2教师提问,-3请假
                    if (typeIndex == 1 || typeIndex == 2 || typeIndex == 3 || typeIndex == 6 || typeIndex == -2 || typeIndex == -3) {
                        continue;
                    }
                } else {
                    //0园长提问
                    if (typeIndex == 3 || typeIndex == 4 || typeIndex == 5 || typeIndex == 7 || typeIndex == 0) {
                        continue;
                    }
                }

                var typeNoVal = typeIndex;
                var typeNameVal = selectTypeNo(typeIndex);
                var item = {
                    typeNo: typeNoVal,
                    typeName: typeNameVal
                };
                msgHeader.push(item);//msgHeader end
            }
            var html = handlebarsHelp("#selectTypeHtml", msgHeader);
            $("#selectType").empty();
            $("#selectType").append(html);


            if (noticeType != -1) {
                $("#selectNoticeType option[value='" + noticeType + "']").attr("selected", "selected");
            }
            $("#sendMsgClsDetailHeader").empty();
            if (typeName == "all") {
                var messageInfo = data.Attach.messageInfo;
                var questionInfo = data.Attach.questionInfo;
                if (messageInfo.length <= 0 && questionInfo.length <= 0) {
                    alert("无记录！");
                    hideDiv();
                    return;
                }
                //循环messageInfo
                drawMessageInfos(messageInfo, calssId);
                //循环questionInfo
                drawQuestionInfos(questionInfo, calssId);
            } else if (typeName == "message") {
                var messageInfo = data.Attach.messageInfo;
                if (messageInfo.length <= 0) {
                    alert("无记录！");
                    var noType = $("#selectNoticeType").val();
                    var noSelectName = getTypeByTypeNo(noType);
                    var noSelectType = {
                        typeNo: noType,
                        typeName: noSelectName
                    };
                    var html = handlebarsHelp("#SendMsgClsDetailHeaderHtml", noSelectType);
                    $("#sendMsgClsDetailHeader").append(html);
                    hideDiv();
                    return;
                }
                //循环messageInfo
                drawMessageInfos(messageInfo, calssId);
            } else if (typeName == "question") {
                var questionInfo = data.Attach.questionInfo;
                if (questionInfo.length <= 0) {
                    alert("无记录！");
                    var noType = $("#selectNoticeType").val();
                    var noSelectName = selectTypeNo(noType);
                    var noSelectType = {
                        typeNo: noType,
                        typeName: noSelectName
                    };
                    var html = handlebarsHelp("#SendMsgClsDetailHeaderHtml", noSelectType);
                    $("#sendMsgClsDetailHeader").append(html);
                    hideDiv();
                    return;
                }
                //循环questionInfo
                drawQuestionInfos(questionInfo, calssId);
            }

            hideDiv();
        }).fail(function (data) {
            hideDiv();
            console.log(data.message);
        });
    };
    var GenerateSendMsgDetailInfo = function (permissionType) {
        var noticeId = $.cookie("paraMsgNoticeId");
        var noticTypeNo = $.cookie("paraMsgNoticeItmTypeNo");
        var image = getImageNameByTypeNo(noticTypeNo);
        $("#noticeTypeImg").attr("src", image);
        var className = $.cookie("paraMsgClassName");
        $("#time").text(className);

        if (noticeId == null) {
            alert("信息缺失");
            return;
        }
        url = "/api/SendMsgDetail/";
        showDiv();
        callApi(url, post, { NoticeId: noticeId }).done(function (data) {
            //var typeInfos = [];
            if (data.Attach.length <= 0) {
                alert("无记录");
                hideDiv();
                return;
            }
            var msgInfo = data.Attach[0];
            var typeInfosItem = {
                title: msgInfo.msg.Title,
                content: msgInfo.msg.Content,
                userName: msgInfo.userName,
                year: msgInfo.year,
                month: msgInfo.month,
                day: msgInfo.day,
                typeName: getTypeByTypeNo(msgInfo.msg.NoticeType)
            };
            var html = handlebarsHelp("#SendMsgDetailDetailHtml", typeInfosItem);
            $("#SendMsgDetailDetailContent").append(html);

            var imgurls = msgInfo.msg.NormalPics;
            if (imgurls == null || imgurls == "") {
                hideDiv();
                return;
            }
            var allimgurl = imgurls.split(",");
            var imgurlInfos = [];
            for (var urlIndex = 0; urlIndex < allimgurl.length; urlIndex++) {
                var url = "http:\/\/babybus.emolbase.com\/" + allimgurl[urlIndex];
                var imgurlInfosItem = {
                    imgurl: url
                };
                imgurlInfos.push(imgurlInfosItem);
            }
            var html = handlebarsHelp("#SendMsgDetailImgHtml", imgurlInfos);
            $("#imgInfos").append(html);
            hideDiv();
        }).fail(function (data) {
            hideDiv();
            console.log(data.message);
        });
    };

    var GenerateAnswerQuestionInfo = function () {
        var classNameVal = $.cookie("paraMsgClassName");
        var selectTypesVal = $.cookie("pareMsgeType");
        var selectQuestionIdVal = $.cookie("pareMsgeQustionId");
        url = "/api/AnswerTheQuestion/";

        var image = getImgByTypeNoForQuestion(selectTypesVal);
        $("#questionTypeImg").attr("src", image);

        showDiv();
        callApi(url, post, { QuestionId: selectQuestionIdVal }).done(function (data) {
            var answerQusInfo = data.Attach.AnswerQus;
            var typeNameVal = getTypeByQuestionTypeNo(selectTypesVal);
            var index = 1;
            var answerDataInfo = [];
            for (var jos in answerQusInfo) {
                var dataInfo = {
                    typeName: typeNameVal,
                    UserNames: answerQusInfo[jos].userNames,
                    year: answerQusInfo[jos].year,
                    month: answerQusInfo[jos].month,
                    day: answerQusInfo[jos].days,
                    title: answerQusInfo[jos].title,
                };
                var html = handlebarsHelp("#sendAnswerQstId", dataInfo);
                $("#SendMsgDetailDetailContent").append(html);

                var answerInfo = answerQusInfo[jos].answers
                if (answerInfo.length <= 0) {
                    alert("无回答记录！");
                    $("#answerInfoTab").css({ "display": "none" });
                } else {
                    $("#answerInfoTab").css({ "display": "block" });
                }
                for (var answersData in answerInfo) {
                    var answer = {
                        Indexs: index,
                        Content: answerInfo[answersData].contents,
                        Year: answerInfo[answersData].years,
                        Month: answerInfo[answersData].months,
                        Day: answerInfo[answersData].dayes,
                        UserName: answerInfo[answersData].userName
                    };
                    answerDataInfo.push(answer);
                    index++;
                }
                var htmls = handlebarsHelp("#answersDataId", answerDataInfo);
                $("#SendMsgAnswerDataContent").append(htmls);
            }
            if (answerQusInfo[0].type == 2) {
                $("#time").text("园区");
            } else {
                $("#time").text(classNameVal);
            }
            hideDiv();
        }).fail(function (data) {
            hideDiv();
            alert(data.message);
        })
    };

    var generateGiftedYoungDetail = function (kinId, years, months) {
        $("#monthMessageContent").empty();
        var index = 1;
        var imageUrl;
        var giftedYoungArry = [];
        url = "/api/GiftedYoung/";
        showDiv();
        callApi(url, post, {giftedType:0, KindergartenId: kinId, Year: years, Month: months }).done(function (data) {
            if (data.Attach.GiftedYoungDetail != null && data.Attach.GiftedYoungDetail != "") {
                for (var jos in data.Attach.GiftedYoungDetail) {
                    var giftedVal = data.Attach.GiftedYoungDetail[jos];
                    if (giftedVal.kinderId == 0) {
                        imageUrl = "images/gifted2.png";
                    } else {
                        imageUrl = "images/gifted1.png";
                    }
                    var item = {
                        infoNo: index,
                        title: giftedVal.Titles,
                        userName: "管理员",
                        year: years,
                        month: giftedVal.Months,
                        day: giftedVal.Days,
                        noticeId: giftedVal.NoticeIds,
                        itmTypeNo: giftedVal.types,
                        imageName: imageUrl
                    };
                    index++;
                    giftedYoungArry.push(item);
                }
                var html = handlebarsHelp("#sendGiftedYoungInfoHTML", giftedYoungArry);
                $("#monthMessageContent").append(html);
            } else {
                alert("没有优幼小报记录");
                //$("#select").css({ "display": "none" });
                hideDiv();
                return;
            }
            hideDiv();
        }).fail(function (data) {
            alert(data.Message);
            hideDiv();
        });
    };

    var genertGiftedItemDetail = function () {
        var noticeIdVal = $.cookie("gifNoticId");
        url = "/api/GiftedYoung/";
        showDiv();
        callApi(url, post, { giftedType: 1, NoticeId: noticeIdVal }).done(function (data) {
            if (data.Attach.GiftedDetail != null && data.Attach.GiftedDetail != "") {
                var giftedVal = data.Attach.GiftedDetail[0];
                if (giftedVal.n.KindergartenId == 0) {
                    $("#noticeTypeImg").attr("src", "images/gifted2.png");
                    $("#time").text("全局优幼小报");
                } else {
                    $("#noticeTypeImg").attr("src", "images/gifted1.png");
                    $("#time").text($.cookie("giftedYoungKinName"));
                }
                var item = {
                    title: giftedVal.n.Title,
                    content: giftedVal.n.Content,
                    userName: "管理员",
                    year: giftedVal.years,
                    month: giftedVal.months,
                    day: giftedVal.days
                };
                var html = handlebarsHelp("#SendMsgDetailDetailHtml", item);
                $("#SendMsgDetailDetailContent").append(html);
                var imgurls = giftedVal.n.NormalPics;
                if (imgurls == null || imgurls == "") {
                    hideDiv();
                    return;
                }
                var allimgurl = imgurls.split(",");
                var imgurlInfos = [];
                for (var urlIndex = 0; urlIndex < allimgurl.length; urlIndex++) {
                    var url = "http:\/\/babybus.emolbase.com\/" + allimgurl[urlIndex];
                    var imgurlInfosItem = {
                        imgurl: url
                    };
                    imgurlInfos.push(imgurlInfosItem);
                }
                var html = handlebarsHelp("#SendMsgDetailImgHtml", imgurlInfos);
                $("#imgInfos").append(html);
            } else {
                alert("数据错误，请联系管理员");
            }
            hideDiv();
        }).fail(function(data) {
            alert(data.Message);
            hideDiv();
        });
    }


    var url = "/api/Management/";
    var get = "GET";
    var post = "POST";

    return {

        //左侧显示幼儿园信息
        GenerateKindergartenAtNavigationBar: function(type) {

            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            //use in the user.html 
            var deferred = $.Deferred();

            var paraMessageKinIdVal = $.cookie("paraMessageKinId");
            var paraMessageKinNameVal = $.cookie("paraMessageKinName");

            var kindergartenId = $.cookie("KindergartenId");
            var classIds = $.cookie("ClassId");

            var roleVal = $.cookie("Role");
            var permissionType = getPermissionType();
            var userId = $.cookie("UserId");

            url = "/api/GenerateKindergartenInfo/";
            callApi(url, post, { kindergartenId: kindergartenId, permissionType: permissionType, userId: userId }).done(function(data) {

                var html = handlebarsHelp("#messageNavKindergartens-template", data.Attach.Kindergartens);
                $("#messageNavKindergartens").append(html);

                if (type == "one") {
                    var firstKinId = data.Attach.Kindergartens[0].KindergartenId;
                    var firstKinName = data.Attach.Kindergartens[0].KindergartenName;
                    if (paraMessageKinIdVal == null) {
                        $("#messageNavKindergartens li:first").addClass("current");
                        $("#messageNavKindergartens li:first").css({ "background": "#D9FFFF" });
                        $("#showKinNameInfo").text(firstKinName);
                        GenerateSendMessageRecordsInfo(firstKinId, permissionType);
                        $.cookie("paraMessageKinId", firstKinId, { expires: 1, path: '/' });
                        $.cookie("paraMessageKinName", firstKinName, { expires: 1, path: '/' });
                    } else {
                        $("#" + paraMessageKinIdVal).addClass("current");
                        $("#" + paraMessageKinIdVal).css({ "background": "#D9FFFF" });
                        $("#showKinNameInfo").text(paraMessageKinNameVal);
                        GenerateSendMessageRecordsInfo(paraMessageKinIdVal, permissionType);
                    }
                } else if (type == "two") { //SendMsgClses.html
                    $("#" + paraMessageKinIdVal).addClass("current");
                    $("#" + paraMessageKinIdVal).css({ "background": "#D9FFFF" });
                    $("#showKinNameInfo").text(paraMessageKinNameVal);
                    GenerateSendMsgClsesInfo(paraMessageKinIdVal, permissionType);
                } else if (type == "three") { //SendMsgClsDetail.html
                    $("#" + paraMessageKinIdVal).addClass("current");
                    $("#" + paraMessageKinIdVal).css({ "background": "#D9FFFF" });
                    $("#showKinNameInfo").text(paraMessageKinNameVal);
                    GenerateSendMsgClsDetailInfo(paraMessageKinIdVal, permissionType);
                } else if (type == "four") {
                    $("#" + paraMessageKinIdVal).addClass("current");
                    $("#" + paraMessageKinIdVal).css({ "background": "#D9FFFF" });
                    $("#showKinNameInfo").text(paraMessageKinNameVal);
                    GenerateSendMsgDetailInfo(permissionType);
                } else if (type == "five") {
                    $("#" + paraMessageKinIdVal).addClass("current");
                    $("#" + paraMessageKinIdVal).css({ "background": "#D9FFFF" });
                    $("#showKinNameInfo").text(paraMessageKinNameVal);
                    GenerateAnswerQuestionInfo();
                }
            }).fail(function(data) {
                deferred.reject();
                console.log(data.message);
            });
        },
        GetSendMessageRecordsInfo: function(kinId, kinName) {

            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            var allLi = $("#messageNavKindergartens li");
            for (var liIndex = 0; liIndex < allLi.length; liIndex++) {
                $("#" + allLi[liIndex].id).removeClass("current");
                $("#" + allLi[liIndex].id).css({ "background": "" });
            }
            $("#" + kinId).addClass("current");
            $("#" + kinId).css({ "background": "#D9FFFF" });
            $("#showKinNameInfo").text(kinName);
            var permissionType = getPermissionType();
            GenerateSendMessageRecordsInfo(kinId, permissionType);
        }, //
        GetSendMsgClsDetailInfo: function() {

            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            var permissionType = getPermissionType();
            GenerateSendMsgClsDetailInfo($.cookie("paraMessageKinId"), permissionType);
        },
        GetMsgInfoByClassType: function() {

            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }
            var permissionType = getPermissionType();
            GenerateSendMsgClsesInfo($.cookie("paraMessageKinId"), permissionType);
        },
        GetSendGiftedYoungInfo: function() {
            var permissionType = getPermissionType();
            url = "/api/GenerateKindergartenInfo/";
            showDiv();
            callApi(url, post, { permissionType: permissionType }).done(function(data) {
                var htmls = handlebarsHelp("#kindergartenNameHTML", data.Attach.Kindergartens);
                //var html = handlebarsHelp("#kindergartenNameHTML", data.Attach.Kindergartens);
                $("#selectKindergarten").append(htmls);
                var html = handlebarsHelp("#choiceManyKindHTML", data.Attach.Kindergartens);
                $("#kinderallId").append(html);
                hideDiv();
            }).fail(function(data) {
                alert(data.Message);
                hideDiv();
            });
        },
        GetGigtedYungDetail: function(type) {
            //检查是否登录
            var isLogin = checkIsLogin();
            if (!isLogin) {
                redirect("login.html");
                return;
            }

            var years = parseInt($("#yearVal").val());
            var monthVal = $("#selectMontn").val();
            var kindergartenIdVal = $.cookie("giftedYoungKinId");
            var kindergartenNameVal = $.cookie("giftedYoungKinName");
            var permissionType = getPermissionType();
            url = "/api/GenerateKindergartenInfo/";
            callApi(url, post, { permissionType: permissionType }).done(function(data) {
                var html = handlebarsHelp("#messageNavKindergartens-template", data.Attach.Kindergartens);
                $("#messageNavKindergartens").append(html);
                var firstKinId = data.Attach.Kindergartens[0].KindergartenId;
                var firstKinName = data.Attach.Kindergartens[0].KindergartenName;
                if (type == "one") {
                    if (kindergartenIdVal == null) {
                        $("#messageNavKindergartens li:first").addClass("current");
                        $("#messageNavKindergartens li:first").css({ "background": "#D9FFFF" });
                        $("#showKinNameInfo").text(firstKinName);
                        generateGiftedYoungDetail(firstKinId, years, monthVal);
                        $.cookie("giftedYoungKinId", firstKinId, { expires: 1, path: '/' });
                        $.cookie("giftedYoungKinName", firstKinName, { expires: 1, path: '/' });
                    } else {
                        $("#" + kindergartenIdVal).addClass("current");
                        $("#" + kindergartenIdVal).css({ "background": "#D9FFFF" });
                        $("#showKinNameInfo").text(kindergartenNameVal);
                        generateGiftedYoungDetail(kindergartenIdVal, years, monthVal);
                    }
                }
                if (type == "two") {
                    $("#" + kindergartenIdVal).addClass("current");
                    $("#" + kindergartenIdVal).css({ "background": "#D9FFFF" });
                    $("#showKinNameInfo").text(kindergartenNameVal);
                    genertGiftedItemDetail();
                }
                   
            }).fail(function(data) {
                alert(data.Message);
            });
        },
        GetSendGigtedSubmit: function() {
            var title = $("#titleNotice").val();
            var content = $("#contentNotice").val();
            var noticeType = 8;
            var userIdVal = $.cookie('UserId');
            var normalPics = $("#imgName").val();
            if (title == null || title == "") {
                alert("请填写完整发送的信息！");
                return;
            }
            if (userIdVal == null || userIdVal == "") {
                userIdVal = 0;
            }
     
            var sendTypeVal = $("input[name = 'choiceKind']:checked").val();
            if (sendTypeVal == 1) {
                var kindergarIdVal = $("#selectKindergarten").val();
                url = "/api/SendInsertGiftedYoungInfo/";
                showDiv();
                callApi(url, post, {
                    KindergartenId: kindergarIdVal,
                    ClassId: 0,
                    UserId: userIdVal,
                    NoticeType: noticeType,
                    Title: title,
                    Content: content,
                    ThumPics: null,
                    NormalPics: normalPics,
                    ReceiverNumber: 0,
                    FavoriteCount: 0,
                    ReadedCount: 0,
                    ConfirmedCount: 0
                }).done(function (data) {
                    if (data.Attach.Status == "successful") {
                        alert("发送成功");
                    } else {
                        alert("发送失败");
                    }
                    hideDiv();
                }).fail(function (data) {
                    alert(data.Message);
                });
            } else if (sendTypeVal == 2) {
                var kinderIdVal = "";
                var selectVal = document.getElementsByName('kindergartenNameMany');
                if (selectVal == null || selectVal == "" || selectVal.length == 0) {
                    alert("请选择要发送的幼儿园！");
                    hideDiv();
                    return;
                }
                var str = selectVal.length;
                for (var i = 0; i < str; i++) {
                    if (selectVal[i].checked == true) {
                        kinderIdVal += selectVal[i].value + ',';
                    }
                }
                url = "/api/GiftedYoung/";
                showDiv();
                callApi(url, post, {
                    giftedType:2,
                    Kindergarten: kinderIdVal,
                    ClassId: 0,
                    UserId: userIdVal,
                    NoticeType: noticeType,
                    Title: title,
                    Content: content,
                    ThumPics: null,
                    NormalPics: normalPics,
                    ReceiverNumber: 0,
                    FavoriteCount: 0,
                    ReadedCount: 0,
                    ConfirmedCount: 0
                }).done(function (data) {
                    if (data.Attach.status == "success") {
                        alert("发送成功");
                        redirect("SendGiftedYoungTab.html");
                    } else {
                        alert("发送失败");
                    }
                    hideDiv();
                }).fail(function (data) {
                    alert(data.Message);
                });
            }
            

        },
        GetOnchangeGiftedInfo: function() {
            var kinderIdVal = $.cookie("giftedYoungKinId");
            var years = parseInt($("#yearVal").val());
            var monthVal = $("#selectMontn").val();
            generateGiftedYoungDetail(kinderIdVal, years, monthVal);
        }
};//end
}();