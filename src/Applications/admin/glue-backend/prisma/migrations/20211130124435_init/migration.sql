/*
  Warnings:

  - You are about to drop the column `avosetGoOnlineId` on the `account` table. All the data in the column will be lost.

*/
-- DropForeignKey
ALTER TABLE `account` DROP FOREIGN KEY `Account_avosetGoOnlineId_fkey`;

-- AlterTable
ALTER TABLE `account` DROP COLUMN `avosetGoOnlineId`;
