import { gql } from 'apollo-server-express';

const typeDefs = gql`
scalar Upload

type Query{
  accounts:[Account],
  accountById(id:ID):AccountOrError
  agentApps:[AgentApp]
  agentAppById(id:ID):Boolean
  agentBackends:[AgentBackend]
  avosetGoOnlines:[AvosetGoOnline]
  identityProviders:[IdentityProvider]
  identityProviderById(id:ID):IdentityProviderOrError
  messageBusConfig: MessageBusConfig
  messageBuses:[MessageBus]
  messageBusById(id:ID):MessageBusOrError
  programmingTools:[ProgrammingTool]
  tenants:[Tenant],
  tenantById(id:ID):TenantOrError,
}

type Mutation{
  addAccount(account:AccountInput, verify:Boolean):AccountOrError
  addAgentApp(agentApp:AgentAppInput):AgentAppOrError
  addAgentBackend(agentBackend:AgentBackendInput):AgentBackendOrError
  addAvosetGo(avosetGo:AvosetGoOnlineInput):AvosetGoOnlineOrError
  addTenant(tenant:TenantInput):TenantOrError
  addIdentityProvider(identityProvider:IdentityProviderInput):IdentityProviderOrError
  addMessageBus(messageBus:MessageBusInput):MessageBusOrError
  addProgrammingTool(programmingTool:ProgrammingToolInput):ProgrammingToolOrError
  deleteById(id:ID):AccountOrError
  deleteMessageBusById(id:ID):MessageBusOrError
  deleteTenantById(id:ID):TenantOrError
  tryVerifyAccount(id:ID):AccountState
  togglePublish(id:ID):AccountState
  toggleTenantState(id:ID):TenantOrError
  updateIdentityProvider(identityProvider:IdentityProviderUpdateInput):IdentityProviderOrError
}

enum AccountState{
  DRAFT
  VERIFIED
  PUBLISHED
}

enum TenantState{
  INACTIVE
  ACTIVE
}

enum MessageBusType{
  RABBITMQ
}

type IdentityProvider{
  id:ID!
  alias:String!
  comment:String
  url:String!
  healthcheckUrl:String
  tenants:[Tenant]
}
  
type AgentApp{
  id: ID!
  alias: String!
  comment: String
  createdAt:String!
  downloadableName: String
  published:Boolean
  platform:String!
  type:String!
  version:String!
}

type Account{
  id:ID!,
  agentApps: [AgentAppConfig]
  branches: [AccountBranch]
  comment:String
  createdAt:String!
  updatedAt: String,
  name:String!,
  oidcProvider: OidcProvider!
  programmingTool: ProgrammingTool
  state: AccountState
}
type AgentAppConfig{
  agentAppId: String
  agentApp: AgentApp
  agentBackendId: String
  agentBackend: AgentBackend
  platform: String
}

type AccountBranch{
  branchId:ID!,
  name:String!,
  description:String
  agentApps: [AgentAppConfig]
}

type AgentBackend{
  id:ID!
  alias: String!
  comment: String
  discoverUri:String!
  healthcheckUrl:String!
  identityProvider:IdentityProvider!
  url:String!
  websocketUri:String
  tenants:[Tenant]
}

type MessageBus{
  id:ID!
  comment:String
  createdAt:String!
  icon: String!
  name: String!
  options: String!
  type: MessageBusType!
  tenants: [Tenant]
}

type RabbitMqMessageBusConfig
{
  minUsernameLength:Int
  minPasswordLength:Int
}
type MessageBusConfig
{
  rabbitmq: RabbitMqMessageBusConfig
}

type Tenant{
  id:ID!
  comment: String
  color: String
  createdAt:String!
  icon: String
  name: String!
  prefix: String!
  messageBus:MessageBus
  state: TenantState
  identityProviders: [IdentityProvider]
}

type OidcProvider{
  id:ID!
  accountUuid: String
  clientId: String
  clientSecret: String
  displayName: String
  issuer: String
  provider: String
  rawInfo: String
  scheme:String
}

type AvosetGoOnline{
  id: ID!
  comment: String
  healthcheckUrl:String
  name: String!
  url: String!
  websocketUri:String!
}

type ProgrammingTool{
  id: ID!
  comment: String
  loginCallback: String!
  name: String!
  url: String!
  avosetGoOnline: AvosetGoOnline!
}

type Error{
  code: String
  message: String
  token: String
}

union AccountOrError = Account | Error
union AgentAppOrError = AgentApp | Error
union AgentBackendOrError = AgentBackend | Error
union AvosetGoOnlineOrError = AvosetGoOnline | Error
union TenantOrError = Tenant | Error
union IdentityProviderOrError = IdentityProvider | Error
union ProgrammingToolOrError = ProgrammingTool | Error
union MessageBusOrError = MessageBus | Error

input AgentAppInput{
  alias:String!
  comment:String
  files: [Upload]
  platform:String!
  type:String!
  version:String!
}
input AgentBackendInput{
  alias: String!
  comment: String
  discoverUri:String!
  healthcheckUrl:String!
  identityProviderId:String!
  url:String!
  websocketUri:String
  tenantIds:[String]
}

input AccountInput{
  comment:String
  name:String!,
  oidcProvider:OidcProviderInput
  programmingToolId: String
  agentApps: [AgentAppConfigInput]
}

input AgentAppConfigInput{
  platform: String!
  agentAppId:String!
  agentBackendId:String!
}

input IdentityProviderInput{
  alias:String!
  comment:String
  url:String! 
  healthcheckUrl:String!
  tenantIds:[String]
}
input IdentityProviderUpdateInput{
  id:ID!
  alias:String!
  comment:String
  url:String! 
  healthcheckUrl:String!
  tenantIds:[String]
}

input MessageBusInput{
  name: String!
  options: String!
  type: String!
}

input TenantInput{
  comment: String
  color: String
  icon: String
  messageBusId: String
  name: String!
  prefix: String!
  state: TenantState 
}

input OidcProviderInput{
  accountUuid: String
  clientId: String
  clientSecret: String
  displayName: String
  issuer: String
  provider: String
  rawInfo: String
  scheme: String
}

input AvosetGoOnlineInput{
  comment: String
  healthcheckUrl:String
  name: String
  url: String
  websocketUri:String
}

input ProgrammingToolInput{
  comment: String
  loginCallback: String
  name: String
  url: String
  avosetGoOnlineId:String
}
`;
export default typeDefs;