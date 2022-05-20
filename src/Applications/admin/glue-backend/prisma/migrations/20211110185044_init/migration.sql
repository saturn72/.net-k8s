/*
  Warnings:

  - You are about to drop the column `mediaInfo` on the `agentapp` table. All the data in the column will be lost.

*/
-- AlterTable
ALTER TABLE `agentapp` DROP COLUMN `mediaInfo`,
    ADD COLUMN `mediaInfoId` TINYTEXT NULL;

-- CreateTable
CREATE TABLE `MediaInfo` (
    `id` VARCHAR(191) NOT NULL,
    `provider` TINYTEXT NOT NULL,
    `path` TINYTEXT NOT NULL,
    `space` TINYTEXT NOT NULL,

    PRIMARY KEY (`id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
