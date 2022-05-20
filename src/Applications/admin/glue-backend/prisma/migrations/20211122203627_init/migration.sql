/*
  Warnings:

  - You are about to drop the column `win10AgentApp` on the `branch` table. All the data in the column will be lost.
  - You are about to drop the column `win7AgentApp` on the `branch` table. All the data in the column will be lost.

*/
-- AlterTable
ALTER TABLE `branch` DROP COLUMN `win10AgentApp`,
    DROP COLUMN `win7AgentApp`;
