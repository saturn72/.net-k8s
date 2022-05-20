/*
  Warnings:

  - You are about to drop the `environment` table. If the table is not empty, all the data it contains will be lost.

*/
-- DropTable
DROP TABLE `environment`;

-- CreateTable
CREATE TABLE `Tenant` (
    `id` VARCHAR(191) NOT NULL,
    `color` VARCHAR(191) NULL,
    `comment` VARCHAR(191) NULL,
    `icon` VARCHAR(191) NULL,
    `name` VARCHAR(255) NOT NULL,
    `state` TINYTEXT NOT NULL,

    UNIQUE INDEX `Tenant_name_key`(`name`),
    PRIMARY KEY (`id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
