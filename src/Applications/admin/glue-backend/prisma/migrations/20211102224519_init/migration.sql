/*
  Warnings:

  - You are about to drop the column `scheme` on the `account` table. All the data in the column will be lost.
  - You are about to drop the column `state` on the `account` table. All the data in the column will be lost.

*/
-- AlterTable
ALTER TABLE `account` DROP COLUMN `scheme`,
    DROP COLUMN `state`,
    ADD COLUMN `published` BOOLEAN NOT NULL DEFAULT false;
