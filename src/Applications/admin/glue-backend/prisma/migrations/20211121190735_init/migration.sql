/*
  Warnings:

  - You are about to drop the `agentappsonaccounts` table. If the table is not empty, all the data it contains will be lost.

*/
-- DropForeignKey
ALTER TABLE `agentappsonaccounts` DROP FOREIGN KEY `AgentAppsOnAccounts_accountId_fkey`;

-- DropForeignKey
ALTER TABLE `agentappsonaccounts` DROP FOREIGN KEY `AgentAppsOnAccounts_agentAppId_fkey`;

-- DropForeignKey
ALTER TABLE `agentappsonaccounts` DROP FOREIGN KEY `AgentAppsOnAccounts_agentBackendId_fkey`;

-- DropTable
DROP TABLE `agentappsonaccounts`;

-- CreateTable
CREATE TABLE `AccountAgentAppConfig` (
    `id` VARCHAR(191) NOT NULL,
    `platform` VARCHAR(191) NOT NULL,
    `accountId` VARCHAR(191) NOT NULL,
    `agentAppId` VARCHAR(191) NOT NULL,
    `agentBackendId` VARCHAR(191) NOT NULL,

    PRIMARY KEY (`id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- AddForeignKey
ALTER TABLE `AccountAgentAppConfig` ADD CONSTRAINT `AccountAgentAppConfig_accountId_fkey` FOREIGN KEY (`accountId`) REFERENCES `Account`(`id`) ON DELETE RESTRICT ON UPDATE CASCADE;
