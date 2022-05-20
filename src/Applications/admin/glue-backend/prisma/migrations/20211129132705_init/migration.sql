/*
  Warnings:

  - You are about to drop the column `accountId` on the `account` table. All the data in the column will be lost.

*/
-- AlterTable
ALTER TABLE `account` DROP COLUMN `accountId`,
    ADD COLUMN `oidcAccountId` TINYTEXT NULL;
