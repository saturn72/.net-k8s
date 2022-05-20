/*
  Warnings:

  - You are about to drop the column `win10AgentApp` on the `account` table. All the data in the column will be lost.
  - You are about to drop the column `win7AgentApp` on the `account` table. All the data in the column will be lost.

*/
-- AlterTable
ALTER TABLE `account` DROP COLUMN `win10AgentApp`,
    DROP COLUMN `win7AgentApp`;

-- CreateTable
CREATE TABLE `AgentAppsOnAccounts` (
    `accountId` VARCHAR(191) NOT NULL,
    `agentAppId` VARCHAR(191) NOT NULL,
    `assignedAt` DATETIME(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3),

    PRIMARY KEY (`accountId`, `agentAppId`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- AddForeignKey
ALTER TABLE `AgentAppsOnAccounts` ADD CONSTRAINT `AgentAppsOnAccounts_accountId_fkey` FOREIGN KEY (`accountId`) REFERENCES `Account`(`id`) ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `AgentAppsOnAccounts` ADD CONSTRAINT `AgentAppsOnAccounts_agentAppId_fkey` FOREIGN KEY (`agentAppId`) REFERENCES `AgentApp`(`id`) ON DELETE RESTRICT ON UPDATE CASCADE;
