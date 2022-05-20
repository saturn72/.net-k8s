/*
  Warnings:

  - Added the required column `messageBusId` to the `Tenant` table without a default value. This is not possible if the table is not empty.

*/
-- AlterTable
ALTER TABLE `tenant` ADD COLUMN `messageBusId` VARCHAR(191) NOT NULL;

-- AddForeignKey
ALTER TABLE `Tenant` ADD CONSTRAINT `Tenant_messageBusId_fkey` FOREIGN KEY (`messageBusId`) REFERENCES `MessageBus`(`id`) ON DELETE RESTRICT ON UPDATE CASCADE;
