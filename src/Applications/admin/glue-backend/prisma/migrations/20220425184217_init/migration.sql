-- CreateTable
CREATE TABLE `MessageBus` (
    `id` VARCHAR(191) NOT NULL,
    `comment` VARCHAR(191) NULL,
    `createdAt` DATETIME(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3),
    `name` VARCHAR(255) NOT NULL,
    `state` TINYTEXT NOT NULL,
    `host` TINYTEXT NOT NULL,
    `port` INTEGER NULL,
    `type` TINYTEXT NOT NULL,

    UNIQUE INDEX `MessageBus_name_key`(`name`),
    PRIMARY KEY (`id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
