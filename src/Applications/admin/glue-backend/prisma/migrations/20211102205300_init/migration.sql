/*
  Warnings:

  - You are about to drop the column `isDraft` on the `account` table. All the data in the column will be lost.
  - You are about to drop the column `publishedAt` on the `account` table. All the data in the column will be lost.

*/
-- AlterTable
ALTER TABLE `account` DROP COLUMN `isDraft`,
    DROP COLUMN `publishedAt`,
    ADD COLUMN `state` TINYTEXT NULL;
