/*
  Warnings:

  - You are about to alter the column `state` on the `account` table. The data in that column could be lost. The data in that column will be cast from `Enum("account_state")` to `TinyText`.

*/
-- AlterTable
ALTER TABLE `account` MODIFY `state` TINYTEXT NOT NULL;
