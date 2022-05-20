/*
  Warnings:

  - You are about to drop the column `webSocketUri` on the `agentbackend` table. All the data in the column will be lost.

*/
-- AlterTable
ALTER TABLE `agentbackend` DROP COLUMN `webSocketUri`,
    ADD COLUMN `websocketUri` TINYTEXT NULL;
