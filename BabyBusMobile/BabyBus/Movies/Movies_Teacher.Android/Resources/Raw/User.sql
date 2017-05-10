/*
Navicat SQLite Data Transfer

Source Server         : babybus
Source Server Version : 30706
Source Host           : :0

Target Server Type    : SQLite
Target Server Version : 30706
File Encoding         : 65001

Date: 2014-08-22 08:47:16
*/

PRAGMA foreign_keys = OFF;

-- ----------------------------
-- Table structure for "main"."User"
-- ----------------------------
DROP TABLE "main"."User";
CREATE TABLE "User" (
"Id"  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
"RoleId"  INTEGER NOT NULL,
"RoleName"  TEXT NOT NULL,
"WeChatId"  INTEGER,
"RealName"  TEXT,
"LoginName"  TEXT NOT NULL,
"Password"  TEXT NOT NULL,
"Description"  TEXT,
"Token"  TEXT,
"HasLogin"  INTEGER,
"CityId"  INTEGER NOT NULL,
FOREIGN KEY ("CityId") REFERENCES "City" ("Id") ON DELETE RESTRICT ON UPDATE CASCADE
);

-- ----------------------------
-- Records of User
-- ----------------------------
INSERT INTO "main"."User" VALUES (1, 1, '老师', null, '沈寅', 'benhaben', '123qwe', null, null, 0, 1);
