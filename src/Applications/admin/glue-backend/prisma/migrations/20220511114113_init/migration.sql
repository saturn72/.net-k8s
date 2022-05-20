-- CreateTable
CREATE TABLE `IdentityProvidersOnTenants` (
    `tenantId` VARCHAR(191) NOT NULL,
    `identityProviderId` VARCHAR(191) NOT NULL,

    PRIMARY KEY (`tenantId`, `identityProviderId`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- AddForeignKey
ALTER TABLE `IdentityProvidersOnTenants` ADD CONSTRAINT `IdentityProvidersOnTenants_tenantId_fkey` FOREIGN KEY (`tenantId`) REFERENCES `Tenant`(`id`) ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `IdentityProvidersOnTenants` ADD CONSTRAINT `IdentityProvidersOnTenants_identityProviderId_fkey` FOREIGN KEY (`identityProviderId`) REFERENCES `IdentityProvider`(`id`) ON DELETE RESTRICT ON UPDATE CASCADE;
