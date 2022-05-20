/*
  Warnings:

  - Added the required column `name` to the `MediaInfo` table without a default value. This is not possible if the table is not empty.
  - Made the column `encoding` on table `mediainfo` required. This step will fail if there are existing NULL values in that column.
  - Made the column `mime` on table `mediainfo` required. This step will fail if there are existing NULL values in that column.

*/
-- AlterTable
ALTER TABLE `mediainfo` ADD COLUMN `name` TINYTEXT NOT NULL,
    MODIFY `encoding` TINYTEXT NOT NULL,
    MODIFY `mime` TINYTEXT NOT NULL;
