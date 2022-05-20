/*
  Warnings:

  - The primary key for the `accountagentappconfig` table will be changed. If it partially fails, the table could be left without primary key constraint.
  - You are about to drop the column `id` on the `accountagentappconfig` table. All the data in the column will be lost.

*/
-- AlterTable
ALTER TABLE `accountagentappconfig` DROP PRIMARY KEY,
    DROP COLUMN `id`,
    ADD PRIMARY KEY (`accountId`, `agentAppId`, `agentBackendId`);
