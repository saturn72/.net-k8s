/*
  Warnings:

  - You are about to alter the column `rawInfo` on the `account` table. The data in that column could be lost. The data in that column will be cast from `Text` to `Json`.

*/
-- AlterTable
ALTER TABLE `account` MODIFY `rawInfo` JSON NULL;
