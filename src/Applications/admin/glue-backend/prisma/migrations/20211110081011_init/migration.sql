-- AlterTable
ALTER TABLE `account` ADD COLUMN `win10AppAgent` JSON NULL,
    ADD COLUMN `win7AppAgent` JSON NULL;

-- AlterTable
ALTER TABLE `agentapp` ADD COLUMN `platform` TINYTEXT NULL;
