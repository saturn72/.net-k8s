/*
  Warnings:

  - Added the required column `avosetGoOnlineId` to the `Account` table without a default value. This is not possible if the table is not empty.

*/
-- AlterTable
ALTER TABLE `account` ADD COLUMN `avosetGoOnlineId` VARCHAR(191) NOT NULL;

-- AddForeignKey
ALTER TABLE `Account` ADD CONSTRAINT `Account_avosetGoOnlineId_fkey` FOREIGN KEY (`avosetGoOnlineId`) REFERENCES `AvosetGoOnline`(`id`) ON DELETE RESTRICT ON UPDATE CASCADE;
