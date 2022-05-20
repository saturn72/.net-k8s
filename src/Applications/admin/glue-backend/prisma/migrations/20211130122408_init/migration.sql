-- CreateTable
CREATE TABLE `AvosetGoOnline` (
    `id` VARCHAR(191) NOT NULL,
    `comment` TINYTEXT NULL,
    `createdAt` DATETIME(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3),
    `name` VARCHAR(255) NOT NULL,
    `updatedAt` DATETIME(3) NULL,
    `url` TINYTEXT NOT NULL,
    `websocketUri` TINYTEXT NULL,

    UNIQUE INDEX `AvosetGoOnline_name_key`(`name`),
    PRIMARY KEY (`id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
