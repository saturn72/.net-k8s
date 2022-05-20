-- CreateTable
CREATE TABLE `ProgrammingTool` (
    `id` VARCHAR(191) NOT NULL,
    `name` TINYTEXT NOT NULL,
    `url` TINYTEXT NOT NULL,
    `comment` TINYTEXT NULL,
    `avosetGoOnlineId` VARCHAR(191) NOT NULL,

    PRIMARY KEY (`id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- AddForeignKey
ALTER TABLE `ProgrammingTool` ADD CONSTRAINT `ProgrammingTool_avosetGoOnlineId_fkey` FOREIGN KEY (`avosetGoOnlineId`) REFERENCES `AvosetGoOnline`(`id`) ON DELETE RESTRICT ON UPDATE CASCADE;
