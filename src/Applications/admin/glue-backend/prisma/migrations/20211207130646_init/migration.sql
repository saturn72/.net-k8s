/*
  Warnings:

  - You are about to drop the `accountprogrammingtoolconfig` table. If the table is not empty, all the data it contains will be lost.
  - Added the required column `loginCallback` to the `ProgrammingTool` table without a default value. This is not possible if the table is not empty.

*/
-- DropForeignKey
ALTER TABLE `accountprogrammingtoolconfig` DROP FOREIGN KEY `AccountProgrammingToolConfig_accountId_fkey`;

-- DropForeignKey
ALTER TABLE `accountprogrammingtoolconfig` DROP FOREIGN KEY `AccountProgrammingToolConfig_programmingToolId_fkey`;

-- AlterTable
ALTER TABLE `account` ADD COLUMN `programmingToolId` VARCHAR(191) NULL;

-- AlterTable
ALTER TABLE `programmingtool` ADD COLUMN `loginCallback` TINYTEXT NOT NULL;

-- DropTable
DROP TABLE `accountprogrammingtoolconfig`;

-- AddForeignKey
ALTER TABLE `Account` ADD CONSTRAINT `Account_programmingToolId_fkey` FOREIGN KEY (`programmingToolId`) REFERENCES `ProgrammingTool`(`id`) ON DELETE SET NULL ON UPDATE CASCADE;
