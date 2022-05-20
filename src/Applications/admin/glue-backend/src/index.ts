import startup from "./startupTask";
import express from "express";
import cors from 'cors';
import { ApolloServer } from "apollo-server-express";
import { PrismaClient } from "@prisma/client";
import { graphqlUploadExpress } from 'graphql-upload';
import typeDefs from "./schema";
import resolvers from "./resolvers";
import { AccountAPI } from "./datasources/account";
import { AgentAppAPI } from "./datasources/agentApp";
import { AgentBackendAPI } from "./datasources/agentBackend";
import { AgentBackendServiceImpl } from "./services/agentBackend/agentBackendService";
import { IdentityProviderAPI } from "./datasources/identityProvider";
import { AgentAppController } from './controllers/agentAppController'
import { AgentAppServiceImpl } from './services/agentApp/agentAppServiceImpl';
import { MediaInfoServiceImpl } from './services/media/mediaInfoServiceImpl'
import { ProgrammingToolServiceImpl } from './services/programmingTool/programmingToolService'
import { AccountServiceImpl } from "./services/account/accountServiceImpl";
import { AvosetGoAPI } from "./datasources/avosetGoAPI";
import { AvosetGoServiceImpl } from "./services/avosetGo/avosetGoService";
import { ProgrammingToolAPI } from "./datasources/programmingToolAPI";
import { TenantAPI } from "./datasources/tenant";
import { TenantServiceImpl } from "./services/tenant/tenantService";
import { MessageBusAPI } from "./datasources/messageBus";
import { MessageBusServiceImpl } from "./services/messageBus/messageBusService";
import { IdentityProviderServiceImpl } from "./services/identityProvider/identityProviderService";

const prisma: PrismaClient = new PrismaClient();
const agentBackendService = new AgentBackendServiceImpl(prisma);
const accountService = new AccountServiceImpl(prisma);
const agentAppService = new AgentAppServiceImpl(prisma);
const avosetGoService = new AvosetGoServiceImpl(prisma);
const idpService = new IdentityProviderServiceImpl(prisma);
const programmingToolService = new ProgrammingToolServiceImpl(prisma);
const mediaInfoService = new MediaInfoServiceImpl(prisma);
const messageBusService = new MessageBusServiceImpl(prisma);
const tenantService = new TenantServiceImpl(prisma);

const server = new ApolloServer({
  typeDefs,
  resolvers,
  dataSources: () => ({
    accounts: new AccountAPI({ prisma, accountService }),
    agentApps: new AgentAppAPI(agentAppService, mediaInfoService),
    agentBackends: new AgentBackendAPI(agentBackendService),
    avosetGo: new AvosetGoAPI(avosetGoService),
    identityProviders: new IdentityProviderAPI(idpService),
    messageBus: new MessageBusAPI(messageBusService),
    programmingTool: new ProgrammingToolAPI(programmingToolService),
    tenants: new TenantAPI(tenantService),
  }),
});

async function startServer() {
  await server.start();
  const app = express()

  var corsOptions = {
    origin: [
      'http://localhost:3000',
      'https://studio.apollographql.com',
    ]
  }

  app.use(cors(corsOptions));
  //max file(s) size = 500mb.
  app.use(graphqlUploadExpress({ maxFieldSize: 500 * 1000000 }));
  server.applyMiddleware({ app });

  app.get('/api/agentapp/download/:agentId', function (req, res) {
    const ctrl = new AgentAppController({
      req,
      res,
      agentAppService,
      mediaInfoService
    });
    return ctrl.download();
  })

  const port = process.env.PORT || 4000;
  app.listen({ port }, () => {
    console.log(`🚀 Server ready at http://localhost:${port}${server.graphqlPath}`);
  });
}

startup.onStartup();
startServer();
