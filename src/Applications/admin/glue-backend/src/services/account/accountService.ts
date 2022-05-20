import { Account } from "../../domain"

export interface AccountService {
    addAccount(account: Account): Promise<Account>
    getAll(): Promise<Account[]>;
    getById(id: string): Promise<Account>
    isValidForCreate(account: Account): Promise<Boolean>;
    prepareForCreate(account: Account): Promise<void>
    updateById(id: string, data: any): Promise<Account>;
    updateAccountBranches(account: Account): Promise<void>
    deleteById(id: string): Promise<Account>;
}