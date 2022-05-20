import { ReadStream } from "fs";

export interface Error {
  code: string
  message: string
  token: string
}

export interface FileUpload {
  createReadStream(): ReadStream,
  filename: string,
  mimetype: string,
  encoding: string
}
export interface MediaInfo {
  id?: string
  bytes: Buffer
  encoding: string
  name: string
  mime: string
  provider?: string,
  providerSpace?: string
  providerPath?: string,
};
export interface AgentApp {
  id?: string
  alias: string
  comment?: string
  downloadableName: string
  mediaInfos?: MediaInfo[]
  mediaInfoIds?: string[]
  platform: "win7x86" | "win7x64" | "win10x86" | "win10x64" | undefined
  published?: Boolean
  type: string
  version: string
}
export class AccountState {
  static draft = 'draft'
  static verified = "verified"
  static published = "published"
}

export interface Account {
  id?: string
  branches?: Branch[]
  comment?: string
  createdAt?: string
  name: string,
  oidcProvider: OidcProviderInfo
  updatedAt?: string,
  state?: string
  agentApps?: AgentAppConfig[]
  avosetGoOnline: AvosetGoOnline
  programmingTool: ProgrammingTool
  programmingToolId: string
}

export interface AvosetGoOnline {
  id?: string
  comment?: string
  createdAt?: string
  healthcheckUrl: string
  name: string,
  updatedAt?: string,
  url: string
  websocketUri: string
}

export interface AgentAppConfig {
  agentApp: AgentApp
  agentAppId: string
  agentBackend: AgentBackend
  agentBackendId: string
  platform: string
}
export interface Branch {
  id?: string
  accountId?: string,
  branchId: string
  description?: string
  name: string
  win7AgentApp?: AgentAppConfig
  win10AgentApp?: AgentAppConfig
}

export interface AgentBackend {
  id?: string;
  alias: string
  comment?: string
  discoverUri: string
  identityProviderId: string
  identityProvider?: IdentityProvider
  healthcheckUrl: string
  url: string
  websocketUri: string
  tenants?: Tenant[]
  tenantIds?: string[],
}

export class MessageBusType {
  static rabbitmq = 'rabbitmq'
}

export interface MessageBus {
  id?: string
  name?: string
  comment?: string
  options?: any
  type: string
  icon: string;
}

export class TenantState {
  static inactive = 'inactive'
  static active = "active"
}

export interface Tenant {
  id?: string
  name?: string
  comment?: string
  color?: string
  icon?: string
  messageBusId: string;
  state: string
}


export interface OidcProviderInfo {
  accountUuid?: string
  clientId?: string
  clientSecret?: string
  displayName?: string
  issuer?: string
  provider?: string
  rawInfo?: string
  scheme?: string
}

export interface IdentityProvider {
  id?: string;
  alias: string
  comment?: string
  url: string
  healthcheckUrl: string
  tenants?: Tenant[]
  tenantIds?: string[],
}

export interface ProgrammingTool {
  id?: string;
  name: string
  comment?: string
  loginCallback: string
  url: string
  avosetGoOnline?: AvosetGoOnline
  avosetGoOnlineId: string
}
