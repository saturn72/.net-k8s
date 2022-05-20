-- CreateTable
CREATE TABLE `Environment` (
    `id` VARCHAR(191) NOT NULL,
    `name` VARCHAR(255) NOT NULL,
    `state` TINYTEXT NOT NULL,
    `comment` VARCHAR(191) NULL,

    UNIQUE INDEX `Environment_name_key`(`name`),
    PRIMARY KEY (`id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
