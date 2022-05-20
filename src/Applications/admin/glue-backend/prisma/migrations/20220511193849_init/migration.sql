-- CreateTable
CREATE TABLE `_AgentBackendToTenant` (
    `A` VARCHAR(191) NOT NULL,
    `B` VARCHAR(191) NOT NULL,

    UNIQUE INDEX `_AgentBackendToTenant_AB_unique`(`A`, `B`),
    INDEX `_AgentBackendToTenant_B_index`(`B`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- AddForeignKey
ALTER TABLE `_AgentBackendToTenant` ADD FOREIGN KEY (`A`) REFERENCES `AgentBackend`(`id`) ON DELETE CASCADE ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `_AgentBackendToTenant` ADD FOREIGN KEY (`B`) REFERENCES `Tenant`(`id`) ON DELETE CASCADE ON UPDATE CASCADE;
