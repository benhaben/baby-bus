/*
Navicat SQLite Data Transfer

Source Server         : babybus
Source Server Version : 30706
Source Host           : :0

Target Server Type    : SQLite
Target Server Version : 30706
File Encoding         : 65001

Date: 2014-08-22 08:47:11
*/

PRAGMA foreign_keys = OFF;

-- ----------------------------
-- Table structure for "main"."City"
-- ----------------------------
DROP TABLE "main"."City";
CREATE TABLE "City" (
"Id"  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
"CityName"  TEXT NOT NULL
);

-- ----------------------------
-- Records of City
-- ----------------------------
INSERT INTO "main"."City" VALUES (1, '上海');
INSERT INTO "main"."City" VALUES (2, '西安');
