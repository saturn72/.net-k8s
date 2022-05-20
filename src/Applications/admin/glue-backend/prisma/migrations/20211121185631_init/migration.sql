/*
  Warnings:

  - The primary key for the `agentappsonaccounts` table will be changed. If it partially fails, the table could be left without primary key constraint.
  - Added the required column `agentBackendId` to the `AgentAppsOnAccounts` table without a default value. This is not possible if the table is not empty.

*/
-- AlterTable
ALTER TABLE `agentappsonaccounts` DROP PRIMARY KEY,
    ADD COLUMN `agentBackendId` VARCHAR(191) NOT NULL,
    ADD PRIMARY KEY (`accountId`, `agentAppId`, `agentBackendId`);

-- AddForeignKey
ALTER TABLE `AgentAppsOnAccounts` ADD CONSTRAINT `AgentAppsOnAccounts_agentBackendId_fkey` FOREIGN KEY (`agentBackendId`) REFERENCES `AgentBackend`(`id`) ON DELETE RESTRICT ON UPDATE CASCADE;
