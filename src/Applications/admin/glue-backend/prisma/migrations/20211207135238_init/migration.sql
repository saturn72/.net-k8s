/*
  Warnings:

  - You are about to drop the column `loginCallback` on the `oidcprovider` table. All the data in the column will be lost.
  - You are about to drop the column `oidcAccountUuid` on the `oidcprovider` table. All the data in the column will be lost.
  - Added the required column `accountUuid` to the `OidcProvider` table without a default value. This is not possible if the table is not empty.

*/
-- AlterTable
ALTER TABLE `oidcprovider` DROP COLUMN `loginCallback`,
    DROP COLUMN `oidcAccountUuid`,
    ADD COLUMN `accountUuid` TINYTEXT NOT NULL;
