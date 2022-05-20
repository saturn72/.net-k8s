-- AlterTable
ALTER TABLE `account` ADD COLUMN `programmingToolId` VARCHAR(191) NULL;

-- AddForeignKey
ALTER TABLE `Account` ADD CONSTRAINT `Account_programmingToolId_fkey` FOREIGN KEY (`programmingToolId`) REFERENCES `ProgrammingTool`(`id`) ON DELETE SET NULL ON UPDATE CASCADE;
