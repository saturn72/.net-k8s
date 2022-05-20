-- CreateTable
CREATE TABLE `IdentityProvider` (
    `id` VARCHAR(191) NOT NULL,
    `alias` TINYTEXT NULL,
    `comment` TINYTEXT NULL,
    `url` TINYTEXT NULL,
    `healthcheckUrl` TINYTEXT NULL,

    PRIMARY KEY (`id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
