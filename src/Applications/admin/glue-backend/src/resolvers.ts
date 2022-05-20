import accountQuery from "./resolvers/account/query";
import accountMutation from "./resolvers/account/mutation";
import agentAppQuery from "./resolvers/agentApp/query";
import agentAppMutation from "./resolvers/agentApp/mutation";
import agentBackendQuery from "./resolvers/agentBackend/query";
import agentBackendMutation from "./resolvers/agentBackend/mutation";
import avosetGoOnlineQuery from "./resolvers/avosetGoOnline/query";
import avosetGoOnlineMutation from "./resolvers/avosetGoOnline/mutation";
import identityProviderQuery from "./resolvers/identityProvider/query";
import identityProviderMutation from "./resolvers/identityProvider/mutation";
import messageBusQuery from "./resolvers/messageBus/query";
import messageBusMutation from "./resolvers/messageBus/mutation";
import programmingToolQuery from "./resolvers/programmingTool/query";
import programmingToolMutation from "./resolvers/programmingTool/mutation";
import tenantQuery from "./resolvers/tenant/query";
import tenantMutation from "./resolvers/tenant/mutation";

import { GraphQLUpload } from "graphql-upload";

export default {
  Upload: GraphQLUpload,
  Query: {
    ...accountQuery,
    ...agentAppQuery,
    ...agentBackendQuery,
    ...avosetGoOnlineQuery,
    ...identityProviderQuery,
    ...messageBusQuery,
    ...programmingToolQuery,
    ...tenantQuery
  },
  Mutation: {
    ...accountMutation,
    ...agentAppMutation,
    ...agentBackendMutation,
    ...avosetGoOnlineMutation,
    ...identityProviderMutation,
    ...messageBusMutation,
    ...programmingToolMutation,
    ...tenantMutation,
  },
  AccountState: {
    DRAFT: 'draft',
    VERIFIED: 'verified',
    PUBLISHED: 'published',
  },
  TenantState: {
    INACTIVE: 'inactive',
    ACTIVE: 'active'
  },
  MessageBusType: {
    RABBITMQ: "rabbitmq"
  },

  AccountOrError: {
    __resolveType(obj: any) {
      return obj.code ? 'Error' : 'Account';
    }
  },
  AgentAppOrError: {
    __resolveType(obj: any) {
      return obj.code ? 'Error' : 'AgentApp';
    }
  },
  AgentBackendOrError: {
    __resolveType(obj: any) {
      return obj.code ? 'Error' : 'AgentBackend';
    }
  },
  AvosetGoOnlineOrError: {
    __resolveType(obj: any) {
      return obj.code ? 'Error' : 'AvosetGoOnline';
    }
  },
  MessageBusOrError: {
    __resolveType(obj: any) {
      return obj.code ? 'Error' : 'MessageBus';
    }
  },
  IdentityProviderOrError: {
    __resolveType(obj: any) {
      return obj.code ? 'Error' : 'IdentityProvider';
    }
  },
  ProgrammingToolOrError: {
    __resolveType(obj: any) {
      return obj.code ? 'Error' : 'ProgrammingTool';
    }
  },
  TenantOrError: {
    __resolveType(obj: any) {
      return obj.code ? 'Error' : 'Tenant';
    }
  },
};
