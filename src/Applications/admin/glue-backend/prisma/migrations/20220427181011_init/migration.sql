-- AlterTable
ALTER TABLE `messagebus` ADD COLUMN `icon` TINYTEXT NULL;

-- AlterTable
ALTER TABLE `tenant` MODIFY `color` TINYTEXT NULL,
    MODIFY `icon` TINYTEXT NULL;
