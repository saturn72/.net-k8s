-- AddForeignKey
ALTER TABLE `AccountAgentAppConfig` ADD CONSTRAINT `AccountAgentAppConfig_agentAppId_fkey` FOREIGN KEY (`agentAppId`) REFERENCES `AgentApp`(`id`) ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `AccountAgentAppConfig` ADD CONSTRAINT `AccountAgentAppConfig_agentBackendId_fkey` FOREIGN KEY (`agentBackendId`) REFERENCES `AgentBackend`(`id`) ON DELETE RESTRICT ON UPDATE CASCADE;
