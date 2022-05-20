/*
  Warnings:

  - You are about to drop the column `win10AppAgent` on the `account` table. All the data in the column will be lost.
  - You are about to drop the column `win7AppAgent` on the `account` table. All the data in the column will be lost.
  - You are about to drop the column `win10AppAgent` on the `branch` table. All the data in the column will be lost.
  - You are about to drop the column `win7AppAgent` on the `branch` table. All the data in the column will be lost.

*/
-- AlterTable
ALTER TABLE `account` DROP COLUMN `win10AppAgent`,
    DROP COLUMN `win7AppAgent`,
    ADD COLUMN `win10AgentApp` JSON NULL,
    ADD COLUMN `win7AgentApp` JSON NULL;

-- AlterTable
ALTER TABLE `branch` DROP COLUMN `win10AppAgent`,
    DROP COLUMN `win7AppAgent`,
    ADD COLUMN `win10AgentApp` JSON NULL,
    ADD COLUMN `win7AgentApp` JSON NULL,
    MODIFY `description` VARCHAR(191) NULL;
