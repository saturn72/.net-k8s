/*
  Warnings:

  - You are about to drop the column `published` on the `account` table. All the data in the column will be lost.
  - You are about to drop the column `verified` on the `account` table. All the data in the column will be lost.

*/
-- AlterTable
ALTER TABLE `account` DROP COLUMN `published`,
    DROP COLUMN `verified`,
    ADD COLUMN `state` ENUM('DRAFT', 'VERIFIED', 'PUBLISHED') NOT NULL DEFAULT 'DRAFT';
