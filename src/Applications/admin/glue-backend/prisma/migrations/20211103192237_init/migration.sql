-- AlterTable
ALTER TABLE `account` ADD COLUMN `branches` JSON NULL,
    MODIFY `comment` TINYTEXT NULL;
