/*
  Warnings:

  - You are about to drop the column `host` on the `messagebus` table. All the data in the column will be lost.
  - You are about to drop the column `port` on the `messagebus` table. All the data in the column will be lost.

*/
-- AlterTable
ALTER TABLE `messagebus` DROP COLUMN `host`,
    DROP COLUMN `port`,
    ADD COLUMN `options` JSON NULL;
