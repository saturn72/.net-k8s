/*
  Warnings:

  - You are about to drop the column `programmingToolId` on the `account` table. All the data in the column will be lost.

*/
-- DropForeignKey
ALTER TABLE `account` DROP FOREIGN KEY `Account_programmingToolId_fkey`;

-- DropForeignKey
ALTER TABLE `programmingtool` DROP FOREIGN KEY `ProgrammingTool_avosetGoOnlineId_fkey`;

-- AlterTable
ALTER TABLE `account` DROP COLUMN `programmingToolId`,
    ADD COLUMN `accountProgrammingToolConfigId` VARCHAR(191) NULL;

-- AlterTable
ALTER TABLE `programmingtool` MODIFY `avosetGoOnlineId` VARCHAR(191) NULL;

-- CreateTable
CREATE TABLE `AccountProgrammingToolConfig` (
    `id` VARCHAR(191) NOT NULL,
    `accountId` VARCHAR(191) NOT NULL,
    `programmingToolId` VARCHAR(191) NOT NULL,
    `loginCallback` VARCHAR(191) NOT NULL,

    UNIQUE INDEX `AccountProgrammingToolConfig_accountId_key`(`accountId`),
    PRIMARY KEY (`id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- AddForeignKey
ALTER TABLE `AccountProgrammingToolConfig` ADD CONSTRAINT `AccountProgrammingToolConfig_accountId_fkey` FOREIGN KEY (`accountId`) REFERENCES `Account`(`id`) ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `AccountProgrammingToolConfig` ADD CONSTRAINT `AccountProgrammingToolConfig_programmingToolId_fkey` FOREIGN KEY (`programmingToolId`) REFERENCES `ProgrammingTool`(`id`) ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `ProgrammingTool` ADD CONSTRAINT `ProgrammingTool_avosetGoOnlineId_fkey` FOREIGN KEY (`avosetGoOnlineId`) REFERENCES `AvosetGoOnline`(`id`) ON DELETE SET NULL ON UPDATE CASCADE;
