/*
  Warnings:

  - Added the required column `prefix` to the `Tenant` table without a default value. This is not possible if the table is not empty.

*/
-- AlterTable
ALTER TABLE `tenant` ADD COLUMN `prefix` TINYTEXT NOT NULL;
