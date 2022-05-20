-- AlterTable
ALTER TABLE `agentbackend` MODIFY `identityProviderId` VARCHAR(191) NOT NULL;

-- AddForeignKey
ALTER TABLE `AgentBackend` ADD CONSTRAINT `AgentBackend_identityProviderId_fkey` FOREIGN KEY (`identityProviderId`) REFERENCES `IdentityProvider`(`id`) ON DELETE RESTRICT ON UPDATE CASCADE;
