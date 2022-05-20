-- CreateTable
CREATE TABLE `AgentBackend` (
    `id` VARCHAR(191) NOT NULL,
    `alias` TINYTEXT NOT NULL,
    `comment` TINYTEXT NULL,
    `discoverUri` TINYTEXT NOT NULL,
    `healthcheckUrl` TINYTEXT NOT NULL,
    `identityProviderId` TINYTEXT NOT NULL,
    `url` TINYTEXT NOT NULL,
    `webSocketUri` TINYTEXT NULL,

    PRIMARY KEY (`id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
