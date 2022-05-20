/*
  Warnings:

  - You are about to drop the column `path` on the `mediainfo` table. All the data in the column will be lost.
  - You are about to drop the column `space` on the `mediainfo` table. All the data in the column will be lost.
  - Added the required column `bytes` to the `MediaInfo` table without a default value. This is not possible if the table is not empty.

*/
-- AlterTable
ALTER TABLE `mediainfo` DROP COLUMN `path`,
    DROP COLUMN `space`,
    ADD COLUMN `bytes` BLOB NOT NULL,
    ADD COLUMN `encoding` TINYTEXT NULL,
    ADD COLUMN `mime` TINYTEXT NULL,
    ADD COLUMN `providerPath` TINYTEXT NULL,
    ADD COLUMN `providerSpace` TINYTEXT NULL,
    MODIFY `provider` TINYTEXT NULL;
