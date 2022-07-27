SET NAMES utf8;
SET time_zone = '+00:00';
SET foreign_key_checks = 0;
SET sql_mode = 'NO_AUTO_VALUE_ON_ZERO';

CREATE DATABASE `endpoints-admin` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `endpoints-admin`;

DROP TABLE IF EXISTS `endpoints`;
CREATE TABLE `endpoints` (
  `id` varchar(36) NOT NULL DEFAULT 'UUID',
  `account` tinytext NOT NULL,
  `name` tinytext NOT NULL,
  `pathdelimiter` varchar(10) NOT NULL,
  `path` tinytext NOT NULL,
  `createdAt` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `createdByUserId` tinytext NOT NULL,
  `version` tinytext NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

INSERT INTO `endpoints` (`id`, `account`, `name`, `pathdelimiter`, `path`, `createdAt`, `createdByUserId`, `version`) VALUES
('(\'UUID()\')',	'sa',	'n1',	'/',	'sa/n1/v1.11ererfv11',	'2022-06-18 08:52:57',	'anonymous',	'v1.11ererfv11'),
('02131a61-eee5-11ec-9da9-0242ac150002',	'sa',	'n1',	'/',	'sa/n1/v1.ssss2',	'2022-06-18 08:59:49',	'anonymous',	'v1.ssss2'),
('86320ee2-ee1b-11ec-9da9-0242ac150002',	'saturn72',	'ep-1',	'/',	'account/ap-1/v1',	'2022-06-17 08:57:32',	'1234567890',	'v1'),
('acc2f1f5-ee4e-11ec-9da9-0242ac150002',	'acc',	'name',	'/',	'acc/name/e1',	'2022-06-17 15:03:41',	'123456789',	'e1'),
('fa4d07db-eee4-11ec-9da9-0242ac150002',	'sa',	'n1',	'/',	'sa/n1/v1.ssss',	'2022-06-18 08:59:36',	'anonymous',	'v1.ssss'),
('UUID',	'sa',	'n1',	'/',	'sa/n1/v1.sxx',	'2022-06-18 08:54:42',	'anonymous',	'v1.sxx');

DELIMITER ;;

CREATE TRIGGER `endpoints_bi` BEFORE INSERT ON `endpoints` FOR EACH ROW
SET new.id = UUID();;

DELIMITER ;