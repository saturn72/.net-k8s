-- CreateTable
CREATE TABLE `Account` (
    `id` INTEGER NOT NULL AUTO_INCREMENT,
    `name` VARCHAR(255) NOT NULL,
    `comment` VARCHAR(191) NULL,
    `createdAt` DATETIME(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3),
    `updatedAt` DATETIME(3) NULL,
    `publishedAt` DATETIME(3) NULL,
    `isDraft` BOOLEAN NOT NULL DEFAULT false,
    `oidcDisplayName` TINYTEXT NULL,
    `scheme` TINYTEXT NULL,
    `clientId` TINYTEXT NULL,
    `clientSecret` TINYTEXT NULL,
    `issuer` TINYTEXT NULL,
    `rawInfo` TEXT NULL,

    UNIQUE INDEX `Account_name_key`(`name`),
    PRIMARY KEY (`id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
