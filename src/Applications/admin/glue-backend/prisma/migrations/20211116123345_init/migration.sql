/*
  Warnings:

  - Added the required column `downloadableName` to the `AgentApp` table without a default value. This is not possible if the table is not empty.

*/
-- AlterTable
ALTER TABLE `agentapp` ADD COLUMN `downloadableName` TINYTEXT NOT NULL;
