/*
  Warnings:

  - You are about to drop the column `mediaInfoId` on the `agentapp` table. All the data in the column will be lost.

*/
-- AlterTable
ALTER TABLE `agentapp` DROP COLUMN `mediaInfoId`,
    ADD COLUMN `mediaInfoIds` JSON NULL;
