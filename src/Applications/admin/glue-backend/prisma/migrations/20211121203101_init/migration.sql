/*
  Warnings:

  - The primary key for the `accountagentappconfig` table will be changed. If it partially fails, the table could be left without primary key constraint.

*/
-- AlterTable
ALTER TABLE `accountagentappconfig` DROP PRIMARY KEY,
    ADD PRIMARY KEY (`accountId`, `agentAppId`, `agentBackendId`, `platform`);
