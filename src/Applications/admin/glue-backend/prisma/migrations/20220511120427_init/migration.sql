/*
  Warnings:

  - You are about to drop the `identityprovidersontenants` table. If the table is not empty, all the data it contains will be lost.

*/
-- DropForeignKey
ALTER TABLE `identityprovidersontenants` DROP FOREIGN KEY `IdentityProvidersOnTenants_identityProviderId_fkey`;

-- DropForeignKey
ALTER TABLE `identityprovidersontenants` DROP FOREIGN KEY `IdentityProvidersOnTenants_tenantId_fkey`;

-- DropTable
DROP TABLE `identityprovidersontenants`;

-- CreateTable
CREATE TABLE `_IdentityProviderToTenant` (
    `A` VARCHAR(191) NOT NULL,
    `B` VARCHAR(191) NOT NULL,

    UNIQUE INDEX `_IdentityProviderToTenant_AB_unique`(`A`, `B`),
    INDEX `_IdentityProviderToTenant_B_index`(`B`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- AddForeignKey
ALTER TABLE `_IdentityProviderToTenant` ADD FOREIGN KEY (`A`) REFERENCES `IdentityProvider`(`id`) ON DELETE CASCADE ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `_IdentityProviderToTenant` ADD FOREIGN KEY (`B`) REFERENCES `Tenant`(`id`) ON DELETE CASCADE ON UPDATE CASCADE;
