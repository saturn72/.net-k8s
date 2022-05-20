/*
  Warnings:

  - You are about to drop the column `clientId` on the `account` table. All the data in the column will be lost.
  - You are about to drop the column `clientSecret` on the `account` table. All the data in the column will be lost.
  - You are about to drop the column `healthcheckUrl` on the `account` table. All the data in the column will be lost.
  - You are about to drop the column `issuer` on the `account` table. All the data in the column will be lost.
  - You are about to drop the column `oidcAccountId` on the `account` table. All the data in the column will be lost.
  - You are about to drop the column `oidcDisplayName` on the `account` table. All the data in the column will be lost.
  - You are about to drop the column `oidcProvider` on the `account` table. All the data in the column will be lost.
  - You are about to drop the column `rawInfo` on the `account` table. All the data in the column will be lost.
  - You are about to drop the column `scheme` on the `account` table. All the data in the column will be lost.

*/
-- AlterTable
ALTER TABLE `account` DROP COLUMN `clientId`,
    DROP COLUMN `clientSecret`,
    DROP COLUMN `healthcheckUrl`,
    DROP COLUMN `issuer`,
    DROP COLUMN `oidcAccountId`,
    DROP COLUMN `oidcDisplayName`,
    DROP COLUMN `oidcProvider`,
    DROP COLUMN `rawInfo`,
    DROP COLUMN `scheme`,
    ADD COLUMN `oidcProviderId` VARCHAR(191) NULL;

-- CreateTable
CREATE TABLE `OidcProvider` (
    `id` VARCHAR(191) NOT NULL,
    `oidcAccountUuid` TINYTEXT NOT NULL,
    `clientId` TINYTEXT NULL,
    `clientSecret` TINYTEXT NULL,
    `displayName` TINYTEXT NOT NULL,
    `provider` TINYTEXT NULL,
    `issuer` TINYTEXT NULL,
    `loginCallback` TINYTEXT NOT NULL,
    `rawInfo` JSON NULL,
    `scheme` TINYTEXT NULL,
    `accountId` VARCHAR(191) NOT NULL,

    UNIQUE INDEX `OidcProvider_accountId_key`(`accountId`),
    PRIMARY KEY (`id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- AddForeignKey
ALTER TABLE `OidcProvider` ADD CONSTRAINT `OidcProvider_accountId_fkey` FOREIGN KEY (`accountId`) REFERENCES `Account`(`id`) ON DELETE RESTRICT ON UPDATE CASCADE;
