-- AlterTable
ALTER TABLE `account` ADD COLUMN `accountId` TINYTEXT NULL;

-- CreateTable
CREATE TABLE `BranchAgentAppConfig` (
    `platform` VARCHAR(191) NOT NULL,
    `branchId` VARCHAR(191) NOT NULL,
    `agentAppId` VARCHAR(191) NOT NULL,
    `agentBackendId` VARCHAR(191) NOT NULL,

    PRIMARY KEY (`branchId`, `agentAppId`, `agentBackendId`, `platform`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- AddForeignKey
ALTER TABLE `BranchAgentAppConfig` ADD CONSTRAINT `BranchAgentAppConfig_branchId_fkey` FOREIGN KEY (`branchId`) REFERENCES `Branch`(`id`) ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `BranchAgentAppConfig` ADD CONSTRAINT `BranchAgentAppConfig_agentAppId_fkey` FOREIGN KEY (`agentAppId`) REFERENCES `AgentApp`(`id`) ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `BranchAgentAppConfig` ADD CONSTRAINT `BranchAgentAppConfig_agentBackendId_fkey` FOREIGN KEY (`agentBackendId`) REFERENCES `AgentBackend`(`id`) ON DELETE RESTRICT ON UPDATE CASCADE;
